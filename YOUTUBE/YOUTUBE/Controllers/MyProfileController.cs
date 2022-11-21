using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YOUTUBE.Models;

namespace YOUTUBE.Controllers
{
    [Authorize(Roles = "phanquyen")]
    public class MyProfileController : Controller
    {
        // GET: MyProfile
        DataClasses1DataContext db = new DataClasses1DataContext();
        public ActionResult Detail()
        {
            int idacc = Convert.ToInt32(Session["acc"]);
            List<account> ac = db.accounts.ToList();
            List<User> u = db.Users.ToList();
            var innerjoin = (from acc in ac
                             join us in u on acc.matk equals us.matk
                             where acc.matk == idacc 
                             select new innerjoin { accountt=acc,user=us}).SingleOrDefault();
            return View(innerjoin);
        }
        public ActionResult Edit()
        {
            int idacc = Convert.ToInt32(Session["acc"]);
            List<account> ac = db.accounts.ToList();
            List<User> u = db.Users.ToList();
            var innerjoin = (from acc in ac
                             join us in u on acc.matk equals us.matk
                             where acc.matk == idacc
                             select new innerjoin { accountt = acc, user = us }).SingleOrDefault();

            return View(innerjoin);

        }
        [HttpPost]
        public ActionResult Edit(innerjoin innerjoin,HttpPostedFileBase imgfile)
        {
            if(innerjoin !=null)
            {
                imgfile = Request.Files["imgfile"];
                innerjoin.user.imageuser = uploadImage(imgfile);
                db.suathongtinMyProfile(innerjoin.user.matk, innerjoin.user.hoten, 
                    innerjoin.user.email, innerjoin.user.diachi, innerjoin.user.SDT, innerjoin.user.imageuser);

                return RedirectToAction("Detail");
            }else
            {
                return View();
            }
         

        }
        public string uploadImage(HttpPostedFileBase imgfile2)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();

            if (imgfile2 != null && imgfile2.ContentLength > 0)
            {
                string extension = Path.GetExtension(imgfile2.FileName);
                if (extension.ToLower().Equals(".jgp") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("/Content/img/"), random + Path.GetFileName(imgfile2.FileName));
                        imgfile2.SaveAs(path);
                        path = "/Content/img/" + random + Path.GetFileName(imgfile2.FileName);





                        //var stream = new FileStream(path, FileMode.Create);
                        //stream.CopyTo(imgfile.InputStream);



                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }

                }
                else
                {
                    Response.Write("<script>alert('only jpg,png,jpeg' )</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('please select a file' )</script>");
                path = "-1";
            }


            return path;
        }
    }
}