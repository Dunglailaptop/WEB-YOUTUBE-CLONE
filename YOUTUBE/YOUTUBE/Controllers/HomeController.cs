using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YOUTUBE.Models;

namespace YOUTUBE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public ActionResult Index()
        {
         
            return View();
        }
        
        public ActionResult About()
        {
        

            return View();
        }

        public ActionResult Contact()
        {
          
            return View();
        }
    }
}