using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Filters;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorize(Roles = "User", Users = "wangliang")]
        public ActionResult About()
        {
            return View();
        }
    }
}
