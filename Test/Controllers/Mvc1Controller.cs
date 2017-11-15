using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Test.Controllers
{
    public class Mvc1Controller : Controller
    {
        // GET: Mvc1
        public ActionResult Index()
        {
            return View();
        }
    }
}