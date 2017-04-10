using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LexiconLMS.Models;

namespace LexiconLMS.Controllers
{
    [Authorize]
    public class AktivitetController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Aktivitet
        [Authorize(Roles = "Lärare")]
        public ActionResult Index()
        {
            var aktiviteter = db.Aktiviteter.Include(a => a.AktivitetsTyp).OrderBy(o => o.Modul.Kurs.Namn).ThenBy(t => t.StartTid);
            return View(aktiviteter.ToList());
        }

        // GET: Aktivitet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aktivitet aktivitet = db.Aktiviteter.Find(id);
            if (aktivitet == null)
            {
                return HttpNotFound();
            }
            return View(aktivitet);
        }

        // GET: Aktivitet/Create
        [Authorize(Roles = "Lärare")]
        public ActionResult Create(int? ModulId)
        {
            ViewBag.AktivitetsTypId = new SelectList(db.AktivitetsTyper, "Id", "Typ");

            if (ModulId != null)
                TempData["RedirectTo"] = Url.Action("Edit", "Moduls", new { id = ModulId });

            var startvärden = new Aktivitet();    // Dimitris källarlösning (startvärden)
            startvärden.ModulId = ModulId;
            startvärden.StartTid = DateTime.Now;
            startvärden.SlutTid = TimeSpan.Parse(DateTime.Now.AddHours(3).ToShortTimeString());

            return View(startvärden);
        }

        // POST: Aktivitet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lärare")]
        public ActionResult Create([Bind(Include = "Id,Namn,StartTid,SlutTid,AktivitetsTypId,ModulId")] Aktivitet aktivitet)
        {
            var modul = db.Moduler.Find(aktivitet.ModulId);

            ViewBag.AktivitetsTypId = new SelectList(db.AktivitetsTyper, "Id", "Typ", aktivitet.AktivitetsTypId);

            if (ModelState.IsValid)
            {

                if (aktivitet.StartTid < modul.StartDatum)
                {
                    //ViewBag.ErrorMessage = "Startdatum får inte vara innan Kursens Startdatum";
                    TempData["Händelse"] = "Startdatum får inte vara innan modulens startdatum";
                    TempData["Status"] = "Misslyckat";
                    return View(aktivitet);
                }

                if (aktivitet.StartTid.Date > modul.SlutDatum.Date)
                {
                    //ViewBag.ErrorMessage = "Startdatum får inte vara innan Kursens Startdatum";
                    TempData["Händelse"] = "Startdatum får inte vara efter modulens startdatum";
                    TempData["Status"] = "Misslyckat";
                    return View(aktivitet);
                }

                foreach (var a in modul.Aktiviteter)
                {
                    if ((a.StartTid >= aktivitet.StartTid) && (a.StartTid <= (aktivitet.StartTid + aktivitet.SlutTid)))
                    {
                        //ViewBag.ErrorMessage = "En annan Modul finns redan i detta Tidsintervall ";
                        TempData["Händelse"] = "En annan aktivitet finns redan i detta tidsintervall ";
                        TempData["Status"] = "Misslyckat";
                        return View(aktivitet);
                    }
                }

                db.Aktiviteter.Add(aktivitet);
                db.SaveChanges();

                TempData["Händelse"] = $"Lyckat! Skapat aktiviteten {aktivitet.Namn}";
                TempData["Status"] = "Lyckat";

                return RedirectToAction("Edit", "Moduls", new { id = aktivitet.ModulId });
                // return RedirectToAction("Index");
            }

            //ViewBag.AktivitetsTypId = new SelectList(db.AktivitetsTyper, "Id", "Typ", aktivitet.AktivitetsTypId);

            if (TempData["RedirectTo"] != null)
                return Redirect(TempData["RedirectTo"].ToString());

            return View(aktivitet);
        }

        // GET: Aktivitet/Edit/5
        [Authorize(Roles = "Lärare")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aktivitet aktivitet = db.Aktiviteter.Find(id);
            if (aktivitet == null)
            {
                return HttpNotFound();
            }
            ViewBag.AktivitetsTypId = new SelectList(db.AktivitetsTyper, "Id", "Typ", aktivitet.AktivitetsTypId);
            ViewBag.ModulId = new SelectList(db.Moduler, "Id", "Namn", aktivitet.ModulId);
            return View(aktivitet);
        }

        // POST: Aktivitet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lärare")]
        public ActionResult Edit([Bind(Include = "Id,Namn,StartTid,SlutTid,AktivitetsTypId,ModulId")] Aktivitet aktivitet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aktivitet).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Händelse"] = $"Lyckat! Redigerat aktiviteten {aktivitet.Namn}";
                TempData["Status"] = "Lyckat";

                return RedirectToAction("Edit", "Moduls", new { id = aktivitet.ModulId });
            }
            ViewBag.AktivitetsTypId = new SelectList(db.AktivitetsTyper, "Id", "Typ", aktivitet.AktivitetsTypId);
            ViewBag.ModulId = new SelectList(db.Moduler, "Id", "Namn", aktivitet.ModulId);

            return RedirectToAction("Edit", "Moduls", new { id = aktivitet.ModulId } );

            // return View(aktivitet);
        }

        // Delete som inte visar någon bekräftelse
        [Authorize(Roles = "Lärare")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aktivitet aktivitet = db.Aktiviteter.Find(id);


            if (aktivitet == null)
            {
                return HttpNotFound();
            }

            int modulid = (int)aktivitet.ModulId;

            db.Aktiviteter.Remove(aktivitet);
            db.SaveChanges();

            TempData["Händelse"] = $"Lyckat! Tagit bort aktiviteten {aktivitet.Namn}";
            TempData["Status"] = "Lyckat";

            Modul modul = db.Moduler.Find(modulid);

            return RedirectToAction("Edit", "Moduls",  modul);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
