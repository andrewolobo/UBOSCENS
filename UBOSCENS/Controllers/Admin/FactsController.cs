using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UBOSCENS.Models;

namespace UBOSCENS.Controllers.Admin
{
    public class FactsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Facts
        public ActionResult Index()
        {
            return View(db.Facts.ToList());
        }

        // GET: Facts/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facts facts = db.Facts.Find(id);
            if (facts == null)
            {
                return HttpNotFound();
            }
            return View(facts);
        }

        // GET: Facts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Facts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,data")] Facts facts)
        {
            if (ModelState.IsValid)
            {
                facts.id = Guid.NewGuid();
                db.Facts.Add(facts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(facts);
        }

        // GET: Facts/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facts facts = db.Facts.Find(id);
            if (facts == null)
            {
                return HttpNotFound();
            }
            return View(facts);
        }

        // POST: Facts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,data")] Facts facts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(facts);
        }

        // GET: Facts/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facts facts = db.Facts.Find(id);
            if (facts == null)
            {
                return HttpNotFound();
            }
            return View(facts);
        }

        // POST: Facts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Facts facts = db.Facts.Find(id);
            db.Facts.Remove(facts);
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
