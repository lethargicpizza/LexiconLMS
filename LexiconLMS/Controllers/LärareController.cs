using LexiconLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LexiconLMS.Controllers
{
    [Authorize(Roles = "Lärare")]
    public class LärareController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Lärare
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Användare()
        {
            return View(db.Users);
        }
    }
}