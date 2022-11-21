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
    public class ChannelController : Controller
    {
        // GET: Channel
        DataClasses1DataContext db = new DataClasses1DataContext();
      
        public ActionResult Index()
        {
            if (Session["acc"] != null)
            {
                int ac = Convert.ToInt32(Session["acc"]);
                channel channe = db.channels.Where(x => x.matk == ac).SingleOrDefault();
                List<channel> c=db.channels.ToList();
                List<video> video = db.videos.ToList();
                List<account> a = db.accounts.ToList();
                
                    var innerjoin = (from v in video
                                     join ch in c on v.matk equals ch.matk into table1
                                     from ch in table1.DefaultIfEmpty()
                                     join acc in a on ch.matk equals acc.matk
                                     select new innerjoin { videoo = v, channels = ch, accountt = acc }).ToList();
                if (c != null)
                {
                    Session["listvideochannel"] = innerjoin;
                    return View(channe);
                }
                else
                {
                    return RedirectToAction("taokenh", "Channel");
                }

            }
            else
            {
                return RedirectToAction("logins", "Account");
            }
           
          //return View();
        }
        [HttpPost]
        public ActionResult dangkykenh(dangkykenh dk)
        {
            string mak = Request.Form["makenh"];
            if (mak!= null)
            {
                int matkuser = int.Parse(Request.Form["matkuser"]);
                int matk = Convert.ToInt32(Request.Form["matk"]);
                dangkykenh d = db.dangkykenhs.Where(x => x.matk == matk && x.matkuser==matkuser).SingleOrDefault();
                if (d != null)
                {
                  
                    
                    return RedirectToAction("LOADVIDEO", "VIDEO");
                }
                else if (d == null)
                {
                    dk.matk = matk;
                    dk.makenh = int.Parse(mak);
                    dk.matkuser= matkuser;
                    db.dangkychannel(dk.matk,dk.matkuser,dk.makenh);
                   
                    return RedirectToAction("Detail","VIDEO");
                }
            }else
            {
                ViewBag.Message = "Bạn cần phải đăng nhập";
            }
           
            return View();
           
        }
       
        public ActionResult taokenh()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult taokenh(channel c,HttpPostedFileBase file)
        {
            if (Session["acc"] != null)
            {
                c.matk = Convert.ToInt32(Request.Form["matk"]);
                c.images = uploadImage(file);
                db.taokenh(c.tenkenh, c.images, c.matk);
                channel channels = db.channels.Where(x => x.matk == c.matk).SingleOrDefault();
                Session["kenh"] = channels;
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("logins", "Account");
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
                //if (extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                //{
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

                //}
                //else
                //{
                //    Response.Write("<script>alert('only jpg,png,jpeg' )</script>");
                //}
            }
            else
            {
                Response.Write("<script>alert('please select a file' )</script>");
                path = "-1";
            }


            return path;
        }
        public ActionResult kenhnguoidung(int ?id)
        {
            if (id != null)
            {
                
                channel channe = db.channels.Where(x => x.matk == id).SingleOrDefault();

                List<channel> c = db.channels.ToList();
                List<video> video = db.videos.ToList();
                List<account> a = db.accounts.ToList();
                List<User> us=db.Users.ToList();
                var innerjoinchannel = (from user in us
                                        join chann in c on user.matk equals chann.matk
                                        where user.matk == id
                                        select new innerjoin { channels = chann, user = user }).SingleOrDefault();
                var innerjoin = (from vi in video
                                 join ch in c on vi.matk equals ch.matk into table1
                                 from ch in table1.DefaultIfEmpty()
                                 join acc in a on ch.matk equals acc.matk
                                 where acc.matk == id
                                 select new innerjoin { videoo = vi, channels = ch, accountt = acc }).ToList();
                if (c != null)
                {
                    Session["listvideochannel"] = innerjoin;
                    return View(innerjoinchannel);
                }
                else
                {
                    return RedirectToAction("taokenh", "Channel");
                }

            }
            else
            {
                return RedirectToAction("logins", "Account");
            }

            return View();
        }
    }
}
