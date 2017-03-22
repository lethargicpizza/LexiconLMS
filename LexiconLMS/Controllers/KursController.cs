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
    public class KursController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Kurs
        public ActionResult Index()
        {
            return View(db.Kurser.ToList());
        }

        // GET: Klasslista
        public ActionResult Klasslista()
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var KursId = user.KursId;

            // IQueryable<ApplicationUser> klasslista = db.Users.Where(k => k.KursId == KursId);
            IQueryable<ApplicationUser> klasslista = from k in db.Users select k;
            klasslista = klasslista.OrderBy(k => k.FörNamn);



            return View(klasslista);
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
