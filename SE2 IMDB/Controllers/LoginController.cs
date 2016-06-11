using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SE2_IMDB.Models.Entity;

namespace SE2_IMDB.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // Post: Login
        [HttpPost]
        public ActionResult Index(Account account)
        {
            if (ModelState.IsValid)
            {
                if (Account.Authenticate(account.Username, account.Password, System.Web.HttpContext.Current))
                    return RedirectToAction("Index", "Home");
                ViewBag.Error = "Account Credentials invalid!";

            }
            else return View();

            return View(account);
        }
    }
}