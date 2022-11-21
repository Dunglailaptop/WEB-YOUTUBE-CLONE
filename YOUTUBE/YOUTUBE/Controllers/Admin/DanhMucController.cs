using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YOUTUBE.Models;
namespace YOUTUBE.Controllers
{
   
    public class DanhMucController : Controller
    {
        // GET: DanhMuc
        DataClasses1DataContext db = new DataClasses1DataContext();
        [Authorize(Roles = "phanquyen"), Authorize(Roles = "admin")]
        public ActionResult danhsachdanhmuc()
        {
            List<danhmuc> dm = db.danhmucs.ToList();

            return View(dm);
        }
        public ActionResult create()
        {
          
            return View();
        }
        [Authorize(Roles = "phanquyen"), Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult create(danhmuc dm)
        {
            dm.MaQL = Convert.ToInt32(Request.Form["maql"]);
            db.taodanhmuc(dm.tendm,dm.MaQL);
            return RedirectToAction("danhsachdanhmuc");
        }
        [Authorize(Roles = "phanquyen"), Authorize(Roles = "admin")]
        public ActionResult edit(int id)
        {
            danhmuc dm = db.danhmucs.Where(x => x.MaDM == id).SingleOrDefault();
            return View(dm);
        }
        [Authorize(Roles = "phanquyen"), Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult edit(danhmuc dm)
        {
            dm.tendm = Request.Form["tendm"];
            dm.MaDM =Convert.ToInt32(Request.Form["madm"]);
                db.SUADANHMUC(dm.MaDM, dm.tendm);
                return RedirectToAction("danhsachdanhmuc","DanhMuc");
            
           
        
        }
        [Authorize(Roles = "phanquyen")]
        public ActionResult timkiemtheodanhmuc()
        {

            if (Session["acc"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("logins", "Account");
            }
        }
        [Authorize(Roles = "phanquyen")]
        [HttpPost]
        public ActionResult timkiemtheodanhmuc(List<video> videos)
        {

            if (Request.Form["iddm"] != null)
            {
                int timkiemdanhmuc = Convert.ToInt32(Request.Form["iddm"]);
                videos = db.videos.ToList();
                List<account> a = db.accounts.ToList();
                List<User> u = db.Users.ToList();
                var innerjoin = (from vi in videos
                                 join ac in a on vi.matk equals ac.matk into table1
                                 from ac in table1.DefaultIfEmpty()
                                 join us in u on ac.matk equals us.matk
                                 where vi.categoryid==timkiemdanhmuc
                                 select new innerjoin { videoo = vi, accountt = ac, user = us }).ToList();
                return View(innerjoin);
            }
            else
            {

                return RedirectToAction("LOADVIDEO", "VIDEO");
            }



        }
    }
}