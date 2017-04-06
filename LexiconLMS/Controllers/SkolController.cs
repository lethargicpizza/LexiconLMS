using LexiconLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using LexiconLMS.Src;

namespace LexiconLMS.Controllers
{
    public class SkolController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("Lärare"))
            {
                return RedirectToAction("Index", "Kurs");
            }
            else
            {
                return RedirectToAction("ElevIndex", "Skol");
            }
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
                if (idag >= modul.StartDatum.DayOfYear && idag <= modul.SlutDatum.DayOfYear)
                {
                    pågåendeModul = modul;
                    break;
                }
            }

            List<Dag> dagar = new List<Dag>();
            List<Aktivitet> kommandeAktiviteter = new List<Aktivitet>();

            if (pågåendeModul != null)
            {
                int veckaFrammåt = idag + 7;
                aktiviteter = db.Aktiviteter.Where(a => a.ModulId == pågåendeModul.Id);

                int förraDagen = 0;
                int nästaDag = 0;
                Dag dag = null;
                int räknare = 0;
                int räknareStopp = 6;
                foreach (var aktivitet in aktiviteter)
                {
                    if (aktivitet.StartTid.DayOfYear < idag)
                        continue;

                    nästaDag = aktivitet.StartTid.DayOfYear;
                    if (räknare == räknareStopp)
                        break;

                    if (nästaDag != förraDagen)
                    {
                        if (dag != null)
                        {
                            dagar.Add(dag);
                            räknare++;
                        }

                        dag = new Dag();
                        dag.Datum = aktivitet.StartTid;
                    }

                    dag.Aktiviteter.Add(aktivitet);
                    förraDagen = nästaDag;
                }
            }

            var viewModel = new ElevIndexViewModel
            {
                Användarnamn = användare.FullNamn,
                PågåendeKurs = kurs,
                PågåendeModul = pågåendeModul,
                KommandeDagar = dagar,
            };

            return View(viewModel);
        }
    }
}