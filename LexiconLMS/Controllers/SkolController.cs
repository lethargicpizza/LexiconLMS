using LexiconLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace LexiconLMS.Controllers
{
    [Authorize]
    public class SkolController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ElevIndex()
        {
            string användarId = User.Identity.GetUserId();
            var användare = db.Users.Where(u => u.Id == användarId).FirstOrDefault();
            var moduler = db.Moduler.Where(m => m.KursId == användare.KursId);

            Modul pågåendeModul = null;
            IEnumerable<Aktivitet> aktiviteter = null;

            int idag = DateTime.Today.DayOfYear;

            foreach (var modul in moduler)
            {
                if(modul.StartDatum.DayOfYear >= idag && modul.SlutDatum.DayOfYear <= idag)
                {
                    pågåendeModul = modul;
                }
            }

            if(pågåendeModul != null)
            {
                int offset = 7;
                aktiviteter = pågåendeModul.Aktiviteter.Where(a => a.StartTid.DayOfYear >= idag && a.StartTid.DayOfYear <= idag+offset);
            }

            var viewModel = new ElevIndexViewModel
            {
                Användarnamn = användare.FullNamn,
                PågåendeModul = pågåendeModul,
                VeckansAktiviteter = aktiviteter,
            };

            return View(viewModel);
        }


    }
}