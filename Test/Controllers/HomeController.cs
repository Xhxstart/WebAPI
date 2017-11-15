using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
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
        [HttpPost, Route("/Login")]
        public async Task<JsonResult> Login(User user)
        {
            return await Task.Run(() =>
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
             });
        }

        // put: /Home/Login
        //[HttpPut, Route("/Login")]
        //public JsonResult Login1()
        //{
        //    string username = Request["username"];
        //    string pwd = Request["pwd"];

        //    message msg = null;

        //    if (username == "rain" && pwd == "m123")
        //    {
        //        msg = new message(true, "Success");
        //    }
        //    else
        //    {
        //        msg = new message(false, "Fail");
        //    }

        //    return Json(msg);
        //});
        //}
    }
}