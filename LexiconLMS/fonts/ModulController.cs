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
    public class ModulController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Modul
        public ActionResult Index()
        {
            var moduler = db.Moduler.Include(m => m.Kurs);
            return View(moduler.ToList());
        }

        // GET: Modul/Details/5
        public ActionResult Details(int? id)
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

        // GET: Modul/Create
        public ActionResult Create()
        {
            ViewBag.KursId = new SelectList(db.Kurser, "Id", "Namn");
            return View();
        }

        // POST: Modul/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Namn,Beskrivning,StartDatum,SlutDatum,KursId")] Modul modul)
        {
            if (ModelState.IsValid)
            {
                db.Moduler.Add(modul);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KursId = new SelectList(db.Kurser, "Id", "Namn", modul.KursId);
            return View(modul);
        }

        // GET: Modul/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.KursId = new SelectList(db.Kurser, "Id", "Namn", modul.KursId);
            return View(modul);
        }

        // POST: Modul/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Namn,Beskrivning,StartDatum,SlutDatum,KursId")] Modul modul)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modul).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KursId = new SelectList(db.Kurser, "Id", "Namn", modul.KursId);
            return View(modul);
        }

        // GET: Modul/Delete/5
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

        // POST: Modul/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
