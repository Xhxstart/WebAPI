using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Products.Models;

namespace Products.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Home/Index2
        public ActionResult Index2()
        {
            return View();
        }

        // Post: /Home/Login
        [HttpPost]
        public JsonResult Login()
        {
            string username = Request["username"];
            string pwd = Request["pwd"];

            message msg = null;

            if (username == "rain" && pwd == "m123")
            {
                msg = new message(true, "Success");
            }
            else
            {
                msg = new message(false, "Fail");
            }

            return Json(msg);
        }
    }
}