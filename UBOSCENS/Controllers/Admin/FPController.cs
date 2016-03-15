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
    public class FPController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: FP
        public ActionResult Index()
        {
            return View(db.FPSections.ToList());
        }

        // GET: FP/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FPSection fPSection = db.FPSections.Find(id);
            if (fPSection == null)
            {
                return HttpNotFound();
            }
            return View(fPSection);
        }

        // GET: FP/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Title")] FPSection fPSection)
        {
            if (ModelState.IsValid)
            {
                fPSection.id = Guid.NewGuid();
                db.FPSections.Add(fPSection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fPSection);
        }

        // GET: FP/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FPSection fPSection = db.FPSections.Find(id);
            if (fPSection == null)
            {
                return HttpNotFound();
            }
            return View(fPSection);
        }

        // POST: FP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Title")] FPSection fPSection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fPSection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fPSection);
        }

        // GET: FP/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FPSection fPSection = db.FPSections.Find(id);
            if (fPSection == null)
            {
                return HttpNotFound();
            }
            return View(fPSection);
        }

        // POST: FP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            FPSection fPSection = db.FPSections.Find(id);
            db.FPSections.Remove(fPSection);
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
