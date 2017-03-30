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
    public class KursController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Kurs
        public ActionResult Index(string search = null)
        {
            // return View(db.Kurser.ToList());

            if (search == null)
            {
                var kurser = db.Kurser.OrderByDescending(s => s.StartDatum).ThenBy(s => s.Namn);
                return View(kurser);
            }
            else
            {
                var kurser = db.Kurser.OrderByDescending(s => s.StartDatum).ThenBy(s => s.Namn).Where(i => i.Namn.Contains(search));
                return View(kurser);
            }
        }

        // GET: Klasslista
        public ActionResult Klasslista()
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var KursId = user.KursId;

            // IQueryable<ApplicationUser> klasslista = db.Users.Where(k => k.KursId == KursId);
            // IQueryable<ApplicationUser> klasslista = from k in db.Users where k.KursId == KursId select k;

            if (KursId != null)
            {
                var klasslista = db.Users.Where(k => k.KursId == KursId).OrderBy(k => k.FörNamn);
                return View(klasslista);
            }

            return View();
        }

        // GET: Kurs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kurs kurs = db.Kurser.Find(id);
            if (kurs == null)
            {
                return HttpNotFound();
            }
            return View(kurs);
        }

        // GET: Kurs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kurs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Namn,Beskrivning,StartDatum")] Kurs kurs)
        {
            if (ModelState.IsValid)
            {
                db.Kurser.Add(kurs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kurs);
        }

        public ActionResult RemoveModul(int modulId, int kursId)
        {
            // modulen ska få KursId = null
            Modul modul = db.Moduler.Find(modulId);
            modul.KursId = null;
            db.Entry(modul).State = EntityState.Modified;
            db.SaveChanges();

            Kurs kurs = db.Kurser.Find(kursId);

            return RedirectToAction("Edit", kurs);
        }

        public ActionResult RemoveKursmedlem(string userId, int kursId)
        {
            // kursmedlemmen ska få KursId = null
            var medlemmar = db.Users.Where(u => u.Id == userId);

            if (medlemmar.Count() > 0)
            {
                var medlem = medlemmar.First();
                medlem.KursId = null;
                db.Entry(medlem).State = EntityState.Modified;
                db.SaveChanges();
            }

            Kurs kurs = db.Kurser.Find(kursId);

            return RedirectToAction("Edit", kurs);
        }
    

        // GET: Kurs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kurs kurs = db.Kurser.Find(id);
            if (kurs == null)
            {
                return HttpNotFound();
            }
            return View(kurs);
        }

        // POST: Kurs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Namn,Beskrivning,StartDatum")] Kurs kurs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kurs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kurs);
        }

        // GET: Kurs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kurs kurs = db.Kurser.Find(id);
            if (kurs == null)
            {
                return HttpNotFound();
            }
            return View(kurs);
        }

        // POST: Kurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kurs kurs = db.Kurser.Find(id);
            db.Kurser.Remove(kurs);
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
