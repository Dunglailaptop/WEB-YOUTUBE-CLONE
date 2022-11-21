using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YOUTUBE.Models;
namespace YOUTUBE.Controllers
{
    [Authorize(Roles = "phanquyen")]
    public class LastVideoController : Controller
    {

        DataClasses1DataContext db = new DataClasses1DataContext();
        public ActionResult Index()
        {
            return View();
        }
        // GET: LastVideo
        [HttpPost]
        public ActionResult Index(lastvideo cv)
        {
            if (Session["acc"]!=null)
            {
                int matk = Convert.ToInt32(Session["acc"]);
                int mavideo = Convert.ToInt32(Request.Form["mavideo"]);
                lastvideo lv = db.lastvideos.Where(x => x.matk == matk && x.mavideo == mavideo).SingleOrDefault();
                if(lv ==null)
                {
                    db.luuvideoxemsau(matk, mavideo);

                    return RedirectToAction("Detail", "VIDEO");
                }else if(lv!=null)
                {
                    TempData["daluu"] = "Video đã lưu";
                    return RedirectToAction("Detail", "VIDEO");
                }
              
            }
            else
            {
                return RedirectToAction("logins", "Account");
            }
         return View();
        }
        public ActionResult showlastvideo()
        {
            if (Session["acc"]!=null)
            {
                int idacc = Convert.ToInt32(Session["acc"]);
                List<lastvideo> ls = db.lastvideos.ToList();
                List<video> vi = db.videos.ToList();
                List<account> a = db.accounts.ToList();
                var innerjoin = (from lastv in ls
                                 join v in vi on lastv.mavideo equals v.Mavideo into table1
                                 from v in table1.DefaultIfEmpty()
                                 join ac in a on v.matk equals ac.matk
                                 where lastv.matk == idacc
                                 select new innerjoin
                                 {
                                     videoo=v,accountt=ac,lastvideos=lastv
                                 }).ToList();
            return View(innerjoin);
            }else
            {
                return RedirectToAction("logins", "Account");
            }
           
        }
    }
}