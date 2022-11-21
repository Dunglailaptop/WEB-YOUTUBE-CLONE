using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YOUTUBE.Models;
namespace YOUTUBE.Controllers
{
    [Authorize(Roles = "phanquyen")]
    public class timkiemController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        // GET: timkiem
        public ActionResult timkiem()
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
        [HttpPost]
        public ActionResult timkiem(List<video> videos)
        {

            if (Request.Form["n"] != null)
            {
                string timkiem = Request.Form["n"];
                videos = db.videos.ToList();
                List<account> a = db.accounts.ToList();
                List<User> u = db.Users.ToList();
                var innerjoin = (from vi in videos
                                 join ac in a on vi.matk equals ac.matk into table1
                                 from ac in table1.DefaultIfEmpty()
                                 join us in u on ac.matk equals us.matk
                                 where vi.nametitle.Contains(timkiem)
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