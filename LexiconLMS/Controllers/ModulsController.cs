﻿using System;
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
        public ActionResult Index()
        {

            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var KursId = user.KursId;

            if (user.Kurs != null)
            {
                ViewBag.AktuellKurs = user.Kurs.Beskrivning;

                //var moduler = db.Moduler.Include(m => m.Kurs);
                var moduler = db.Moduler.Where(k => k.KursId == KursId).OrderBy(k => k.StartDatum);


                return View(moduler.ToList());
            }

            return View();
        }

        // GET: Moduls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Modul modul = db.Moduler.Find(id);

            

            ModulDetaljViewModel modulDetaljViewModel = null;

            if (modul == null)
            {
                return HttpNotFound();
            }

            return View(modul);
        }

        // GET: Moduls/Create
        public ActionResult Create()
        {
            ViewBag.KursId = new SelectList(db.Kurser, "Id", "Namn");
            return View();
        }

        // POST: Moduls/Create
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

        // GET: Moduls/Edit/5
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

        // POST: Moduls/Edit/5
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

        // GET: Moduls/Delete/5
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
