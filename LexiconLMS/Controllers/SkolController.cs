using LexiconLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace LexiconLMS.Controllers
{
    public class SkolController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Lärare")]
        public ActionResult LärareIndex()
        {
            return View();
        }

        [Authorize]
        public ActionResult ElevIndex()
        {
            string användarId = User.Identity.GetUserId();
            var användare = db.Users.Where(u => u.Id == användarId).FirstOrDefault();

            var moduler = db.Moduler.Where(m => m.KursId == användare.KursId);
            var kurs = db.Kurser.Where(k => k.Id == användare.KursId).FirstOrDefault();

            IEnumerable<Aktivitet> aktiviteter = null;
            Modul pågåendeModul = null;

            int idag = DateTime.Today.DayOfYear;
            foreach (var modul in moduler)
            {
                if(idag >= modul.StartDatum.DayOfYear && idag <= modul.SlutDatum.DayOfYear)
                {
                    pågåendeModul = modul;
                    break;
                }
            }

            if(pågåendeModul != null)
            {
                int veckaFrammåt = idag + 6;
                aktiviteter = pågåendeModul.Aktiviteter.Where(a => a.StartTid.DayOfYear >= idag && (a.StartTid.DayOfYear <= veckaFrammåt));
            }

            var viewModel = new ElevIndexViewModel
            {
                Användarnamn = användare.FullNamn,
                PågåendeKurs = kurs,
                PågåendeModul = pågåendeModul,
                VeckansAktiviteter = aktiviteter,
            };

            return View(viewModel);
        }
    }
}