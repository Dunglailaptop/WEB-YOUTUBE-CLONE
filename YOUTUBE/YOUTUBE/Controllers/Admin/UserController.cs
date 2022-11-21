using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YOUTUBE.Models;
namespace YOUTUBE.Controllers
{
    [Authorize(Roles = "phanquyen"), Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        // GET: User
        public ActionResult Index()
        {
            List<laydanhsachResult> result = new List<laydanhsachResult>();
            return View(result);
        }
        public ActionResult Create()
        {
            List<laydanhsachResult> result = new List<laydanhsachResult>();
            return View(result);
        }
    }
}