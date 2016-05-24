using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SE2_IMDB.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string input = "no input :(")
        {
            ViewBag.test = input;
            ViewBag.numTimes = 5;
            return View();
        }
        
    }
}