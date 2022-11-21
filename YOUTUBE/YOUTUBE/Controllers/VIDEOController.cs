using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
//using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YOUTUBE.Models;
namespace YOUTUBE.Controllers
{

    [Authorize(Roles = "phanquyen")]
    public class VIDEOController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        // GET: VIDEO
       
        public ActionResult Detail(int ?mv,int ?id)
        {
            if (Session["acc"] != null)
            {
                if (mv != null && id != 0)
                {
                    Session["mv"] = mv;
                    Session["id"] = id;
                }

                List<video> v = db.videos.ToList();
                List<account> a = db.accounts.ToList();
                List<User> u = db.Users.ToList();
                //video vi;
                //account ac;
                //User us;
                var quey = (from vi in v
                            join ac in a on vi.matk equals ac.matk into table1
                            from ac in table1.DefaultIfEmpty()
                            join uu in u on ac.matk equals uu.matk
                            where vi.Mavideo == Convert.ToInt32(Session["mv"])
                            select new innerjoin { videoo = vi, accountt = ac, user = uu }).SingleOrDefault();
                var quey2 = from vi in v
                            join ac in a on vi.matk equals ac.matk into table1
                            from ac in table1.DefaultIfEmpty()
                            join uu in u on ac.matk equals uu.matk
                            where vi.categoryid == Convert.ToInt32(Session["id"])
                            select new innerjoin { videoo = vi, accountt = ac, user = uu };


                Session["filevideo"] = quey.videoo.filevideo;
                Session["mavideo"] = quey.videoo.Mavideo;
                Session["iduser"] = quey.accountt.matk;
                int idacc = Convert.ToInt32(Session["acc"]);
                dangkykenh c = db.dangkykenhs.Where(x => x.matk == quey.accountt.matk && x.matkuser == idacc).SingleOrDefault();
                channel c2 = db.channels.Where(x => x.matk == quey.accountt.matk).SingleOrDefault();
                if (c2 == null && c == null)
                {
                    Session["dangky"] = null;
                    Session["dadangky"] = null;
                    Session["kenhchuatao"] = "ok";
                }
                else if (c2 != null && c == null)
                {
                    Session["kenhchuatao"] = null;
                    Session["makenh"] = c2.MaDK;
                    Session["dangky"] = c2.MaDK;
                    Session["dadangky"] = null;
                }


                if (c2 != null && c != null)
                {
                    Session["kenhchuatao"] = null;
                    Session["dangky"] = null;
                    Session["dadangky"] = c2.MaDK;


                }


                Session["namevideo"] = quey.videoo.nametitle;
                Session["noidung"] = quey.videoo.noidung;
                Session["imageuser"] = quey.user.imageuser;
                Session["nameuser"] = quey.accountt.username;
                int mavideo = Convert.ToInt32(Session["mavideo"]);
                List<User> us = db.Users.ToList();
                List<comment> comments = db.comments.ToList();
                List<video> ve = db.videos.ToList();
                var quey3 = from use in us
                            join co in comments on use.matk equals co.matk
                            where co.mavideo == mavideo
                            select new innerjoin { commentt = co, user = use };
                TempData["binhluan"] = quey3.ToList();
                int account = Convert.ToInt32(Session["acc"]);
                List<User> uss = db.Users.ToList();
                List<dangkykenh> ch = db.dangkykenhs.ToList();
                List<account> accc = db.accounts.ToList();
                var quy = from use in uss
                          join ace in accc on use.matk equals ace.matk into table1
                          from ace in table1.DefaultIfEmpty()
                          join chh in ch on ace.matk equals chh.matk
                          where chh.matkuser == account
                          select new innerjoin { dk = chh, user = use, accountt = ace };
                Session["dskenhdy"] = quy.ToList();
                return View(quey2);
            }else
            {
                return RedirectToAction("logins", "Account");
            }
                
        }

        public ActionResult LOADVIDEO()
        {
            //if (Session["acc"]!=null)
            //{
                List<video> v = db.videos.ToList();
                List<User> us = db.Users.ToList();
                List<account> ac = db.accounts.ToList();
                var quy = from vi in v
                          join ace in ac on vi.matk equals ace.matk into table1
                          from ace in table1.DefaultIfEmpty()
                          join user in us on ace.matk equals user.matk
                          select new innerjoin { videoo = vi, user = user, accountt = ace };
            int idacc = Convert.ToInt32(Session["acc"]);
            List<thongbao> tb = db.thongbaos.ToList();
            List<account> accout = db.accounts.ToList();
            List<User> u = db.Users.ToList();
            List<dangkykenh> dk = db.dangkykenhs.ToList(); 
            var innerjoin = (from thog in tb
                             join acc in accout on thog.matk equals acc.matk into table1
                             from acc in table1.DefaultIfEmpty()
                             join user in u on acc.matk equals user.matk into table2
                             from user in table2.DefaultIfEmpty()
                             join dangkyk in dk on user.matk equals dangkyk.matk
                             where dangkyk.matkuser==idacc
                             select new innerjoin { dangkykenhs=dangkyk,accountt = acc, thongbaos = thog, user = user }).ToList();
            Session["thongbao"] = innerjoin;
            List<danhmuc> dm = db.danhmucs.ToList();
            Session["danhmuc"] = dm;
            return View(quy);
            //}else
            //{
            //    return RedirectToAction("logins", "Account");
            //}
         
        }

        [Authorize(Roles = "phanquyen")]
        public ActionResult create()
        {
            List<danhmuc> dm = db.danhmucs.ToList();
            TempData["danhmuc"]=dm;
            int id = Convert.ToInt32(Session["acc"]);
            List<account> accounts = db.accounts.ToList();
            List<video> videos = db.videos.Where(x=>x.matk==id).ToList();
            var innerjoin = (from vi in videos
                             join a in accounts on vi.matk equals a.matk
                             where a.matk == id
                             select new innerjoin { accountt = a, videoo = vi }).ToList();
            Session["danhsachvideocuaban"]=innerjoin;
            return View();
        }
        [HttpPost]
        public ActionResult create(video v, HttpPostedFileBase imgfile, HttpPostedFileBase imgfile2)
        {
            
            v.filevideo = uploadvideo(imgfile);
            v.imagetitle = uploadImage(imgfile2);
            v.matk =Convert.ToInt32(Request.Form["matk"]);
            v.categoryid = Convert.ToInt32(Request.Form["MaDM"]);
            db.createvideo(v.filevideo, v.imagetitle, v.nametitle, v.categoryid, v.noidung, v.matk);
            thongbao tb= new thongbao();
            tb.noidung = "Vừa có video mới click để xem";
            tb.matk = v.matk;
            db.thongbaouser(tb.matk,tb.noidung);
            ViewBag.Message = "Them thanh cong video";
            List<danhmuc> dm = db.danhmucs.ToList();
            TempData["danhmuc"] = dm;
            return View();
        }
        public string uploadvideo(HttpPostedFileBase imgfile)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();

            if (imgfile != null && imgfile.ContentLength > 0)
            {
                string extension = Path.GetExtension(imgfile.FileName);
                //if (extension.ToLower().Equals(".jgp") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                //{
                    try
                    {
                        path = Path.Combine(Server.MapPath("/Content/video/"), random + Path.GetFileName(imgfile.FileName));
                        imgfile.SaveAs(path);
                        path = "/Content/video/" + random + Path.GetFileName(imgfile.FileName);





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
        public ActionResult Edit(int id)
        {
            video v = db.videos.FirstOrDefault(x => x.Mavideo == id);
            TempData["filevideo"] = v.filevideo;
            TempData["imagevideo"] = v.imagetitle;
            List<danhmuc> dm = db.danhmucs.ToList();
            TempData["danhmuc"] = dm;
            return View(v);
        }
        [HttpPost]
        public ActionResult Edit(video v, HttpPostedFileBase imgfile, HttpPostedFileBase imgfile2)
        {

            if (v != null)
            {

                //HttpPostedFileBase imgfile = Request.Files["imgfile"];
                //HttpPostedFileBase imgfile2 = Request.Files["imgfile2"];
                v.filevideo = uploadvideo(imgfile);
                v.imagetitle = uploadImage(imgfile2);


                //v.matk = Convert.ToInt32(Request.Form["matk"]);
                //v.Mavideo = Convert.ToInt32(Request.Form["MaDM"]);
                db.suavideo(v.Mavideo, v.filevideo, v.imagetitle, v.nametitle, v.categoryid, v.noidung, v.matk);
                ViewBag.Message = "sua video thanh cong";
                return RedirectToAction("create");
            }

            return View(); 
        }
        public ActionResult Delete(int id)
        {
            //if (id == null)
            //{
            //    return RedirectToAction("Index");
            //}
            //int mavideo = Convert.ToInt32(Request.Form["mavideo"]);
            video v = db.videos.FirstOrDefault(x => x.Mavideo == id);
            if(v!= null)
            {
                db.xoavideo(v.Mavideo);
                TempData["thongbaoxoa"] = "xoa video thanh cong";
                return RedirectToAction("create");
            }
            
            return RedirectToAction("logins","Account");
        }
    }
}