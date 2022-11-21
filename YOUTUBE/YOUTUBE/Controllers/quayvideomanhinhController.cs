using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YOUTUBE.Controllers
{
    [Authorize(Roles = "phanquyen")]
    public class quayvideomanhinhController : Controller
    {
        // GET: quayvideomanhinh
        public ActionResult Index()
        {
            return View();
        }
    }
}