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
    public class AktivitetController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Aktivitet
        public ActionResult Index()
        {
            var aktiviteter = db.Aktiviteter.Include(a => a.AktivitetsTyp);
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
        public ActionResult Create()
        {
            ViewBag.AktivitetsTypId = new SelectList(db.AktivitetsTyper, "Id", "Typ");
            return View();
        }

        // POST: Aktivitet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Namn,StartTid,SlutTid,AktivitetsTypId")] Aktivitet aktivitet)
        {
            if (ModelState.IsValid)
            {
                db.Aktiviteter.Add(aktivitet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AktivitetsTypId = new SelectList(db.AktivitetsTyper, "Id", "Typ", aktivitet.AktivitetsTypId);
            return View(aktivitet);
        }

        // GET: Aktivitet/Edit/5
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
            return View(aktivitet);
        }

        // POST: Aktivitet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Namn,StartTid,SlutTid,AktivitetsTypId")] Aktivitet aktivitet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aktivitet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AktivitetsTypId = new SelectList(db.AktivitetsTyper, "Id", "Typ", aktivitet.AktivitetsTypId);
            return View(aktivitet);
        }

        // GET: Aktivitet/Delete/5
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
            return View(aktivitet);
        }

        // POST: Aktivitet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Aktivitet aktivitet = db.Aktiviteter.Find(id);
            db.Aktiviteter.Remove(aktivitet);
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
