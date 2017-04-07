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
    public class ModulsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Moduls
        [Authorize(Roles = "Lärare")]
        public ActionResult Index()
        {

            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var KursId = user.KursId;

            if (user.Kurs != null)
            {
                ViewBag.KursNamn = user.Kurs.Namn;

                //var moduler = db.Moduler.Include(m => m.Kurs);
                var moduler = db.Moduler.Where(k => k.KursId == KursId).OrderBy(k => k.StartDatum);

                return View(moduler.ToList());
            }

            if (User.IsInRole("Lärare"))
            {
                var moduler = db.Moduler;

                if (moduler != null)
                {
                    return View(moduler.ToList());
                }
            }

            return View();
        }

        // GET: Moduls/Details/5
        public ActionResult Details(int? id, int? kursId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Modul modul = db.Moduler.Find(id);

            if (modul == null)
            {
                return HttpNotFound();
            }

            var modulDetailsViewModel = new ModulDetailsViewModel();
            var kurs = db.Kurser.Find(modul.KursId);

            modulDetailsViewModel.kurs = kurs;
            modulDetailsViewModel.modul = modul;

            var aktiviteterDB = db.Aktiviteter.Where(m => m.Modul.Id == id).OrderBy(k => k.StartTid);
            var aktiviteter = aktiviteterDB.ToList();

            modulDetailsViewModel.aktiviteter = aktiviteter;

            for (int i = 0; i < aktiviteter.Count(); i++)
            {
                modulDetailsViewModel.datum.Add(aktiviteter[i].StartTid.ToString("yyyy-MM-dd"));
                modulDetailsViewModel.startTid.Add(aktiviteter[i].StartTid.ToString("HH:mm"));
                modulDetailsViewModel.slutTid.Add(aktiviteter[i].SlutTid.ToString(@"hh\:mm"));
            }

            return View(modulDetailsViewModel);

        }

        // GET: Aktivitet/Create
        [Authorize(Roles = "Lärare")]
        public ActionResult Create(int? KursId)
        {
            //ViewBag.AktivitetsTypId = new SelectList(db.AktivitetsTyper, "Id", "Typ");

            if (KursId != null)
                TempData["RedirectTo"] = Url.Action("Edit", "Kurs", new { id = KursId });

            var Kurs = db.Kurser.Find(KursId);

            ViewBag.Kursnamn = Kurs.Namn;
            ViewBag.KursId = Kurs.Id;


            return View();
        }


        //// GET: Moduls/Create
        //public ActionResult Create(int id)
        //{
        //    //ViewBag.KursId = new SelectList(db.Kurser, "Id", "Namn");

        //    ViewBag.KursId = id;

        //    return View();
        //}

        // POST: Moduls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lärare")]
        public ActionResult Create([Bind(Include = "Id,Namn,Beskrivning,StartDatum,SlutDatum,KursId")] Modul modul)
        {
            if (ModelState.IsValid)
            {
                db.Moduler.Add(modul);
                db.SaveChanges();
                return RedirectToAction("Edit", "Kurs", new { id = modul.KursId });
            }

            //ViewBag.AktivitetsTypId = new SelectList(db.AktivitetsTyper, "Id", "Typ", aktivitet.AktivitetsTypId);

            if (TempData["RedirectTo"] != null)
                return Redirect(TempData["RedirectTo"].ToString());

            return View(modul);

        //    if (ModelState.IsValid)
        //    {
        //        modul.KursId = ViewBag.KursId;
        //        db.Moduler.Add(modul);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

            //    //ViewBag.KursId = new SelectList(db.Kurser, "Id", "Namn", modul.KursId);
            //    return View(modul);
            }

        // GET: Moduls/Edit/5
        [Authorize(Roles = "Lärare")]
        public ActionResult Edit(int? id, string search = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Modul modul = db.Moduler.Find(id);
            if (modul == null)
            {
                return HttpNotFound();
            }

            //var kursId = new SelectList(db.Kurser, "Id", "Namn", modul.KursId);

            Kurs kurs = db.Kurser.Find(modul.KursId);

            var modulEditViewModel = new ModulEditViewModel();
            modulEditViewModel.kurs = kurs;
            modulEditViewModel.modul = modul;

            if (search == null)
            {
                var aktiviteter = db.Aktiviteter.Where(m => m.Modul.Id == id).OrderBy(k => k.StartTid);
                modulEditViewModel.aktiviteter = aktiviteter.ToList();
            }
            else
            {
                var aktiviteter = db.Aktiviteter.OrderByDescending(s => s.StartTid).ThenBy(s => s.Namn).Where(i => i.Namn.Contains(search));
                modulEditViewModel.aktiviteter = aktiviteter.ToList();
            }
            
            //modulEditViewModel.aktiviteter = aktiviteter.ToList();

            return View(modulEditViewModel);
        }

        // POST: Moduls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lärare")]
        public ActionResult Edit([Bind(Include = "Id,Namn,Beskrivning,StartDatum,SlutDatum,KursId")] Modul modul)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modul).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Edit", "Kurs", new { id = modul.KursId });
            }
            ViewBag.KursId = new SelectList(db.Kurser, "Id", "Namn", modul.KursId);
            return View(modul);
        }

        // GET: Moduls/Delete/5
        [Authorize(Roles = "Lärare")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modul modul = db.Moduler.Find(id);
            if (modul == null)
            {
                return HttpNotFound();
            }
            return View(modul);
        }

        // POST: Moduls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lärare")]
        public ActionResult DeleteConfirmed(int id)
        {
            Modul modul = db.Moduler.Find(id);
            db.Moduler.Remove(modul);
            db.SaveChanges();

            return RedirectToAction("Index");
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
