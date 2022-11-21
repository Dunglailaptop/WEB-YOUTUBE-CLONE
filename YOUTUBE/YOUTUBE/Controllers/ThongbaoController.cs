using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YOUTUBE.Models;

namespace YOUTUBE.Controllers
{
    [Authorize(Roles = "phanquyen")]
    public class ThongbaoController : Controller
    {
        // GET: Thongbao
        DataClasses1DataContext db = new DataClasses1DataContext();
        public ActionResult Index()
        {
            
            return View();
        }
    }
}