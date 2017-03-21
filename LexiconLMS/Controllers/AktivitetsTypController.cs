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
    public class AktivitetsTypController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AktivitetsTyps
        public ActionResult Index()
        {
            return View(db.AktivitetsTyper.ToList());
        }

        // GET: AktivitetsTyps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AktivitetsTyp aktivitetsTyp = db.AktivitetsTyper.Find(id);
            if (aktivitetsTyp == null)
            {
                return HttpNotFound();
            }
            return View(aktivitetsTyp);
        }

        // GET: AktivitetsTyps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AktivitetsTyps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Typ")] AktivitetsTyp aktivitetsTyp)
        {
            if (ModelState.IsValid)
            {
                db.AktivitetsTyper.Add(aktivitetsTyp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aktivitetsTyp);
        }

        // GET: AktivitetsTyps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AktivitetsTyp aktivitetsTyp = db.AktivitetsTyper.Find(id);
            if (aktivitetsTyp == null)
            {
                return HttpNotFound();
            }
            return View(aktivitetsTyp);
        }

        // POST: AktivitetsTyps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Typ")] AktivitetsTyp aktivitetsTyp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aktivitetsTyp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aktivitetsTyp);
        }

        // GET: AktivitetsTyps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AktivitetsTyp aktivitetsTyp = db.AktivitetsTyper.Find(id);
            if (aktivitetsTyp == null)
            {
                return HttpNotFound();
            }
            return View(aktivitetsTyp);
        }

        // POST: AktivitetsTyps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AktivitetsTyp aktivitetsTyp = db.AktivitetsTyper.Find(id);
            db.AktivitetsTyper.Remove(aktivitetsTyp);
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
