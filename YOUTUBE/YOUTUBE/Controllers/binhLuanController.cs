using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YOUTUBE.Models;

namespace YOUTUBE.Controllers
{
    //[Authorize(Roles = "Nguoidung"), Authorize(Roles ="Admin")]
    [Authorize(Roles = "phanquyen")]
    public class binhLuanController : Controller
    {
        // GET: binhLuan
        DataClasses1DataContext db = new DataClasses1DataContext();
      
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(comment c)
        {
            int idv = Convert.ToInt32(Session["mavideo"]);
            string noidung = Request.Form["noidung"];
            int idac = Convert.ToInt32(Session["acc"]);
            db.nhapbinhluan(idac, noidung, idv);
            return RedirectToAction("Detail", "VIDEO");
        }
    }
}