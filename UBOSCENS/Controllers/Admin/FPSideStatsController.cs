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
    public class FPSideStatsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: FPSideStats
        public ActionResult Index()
        {
            return View(db.FPSidestats.ToList());
        }

        // GET: FPSideStats/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FPSideStats fPSideStats = db.FPSidestats.Find(id);
            if (fPSideStats == null)
            {
                return HttpNotFound();
            }
            return View(fPSideStats);
        }

        // GET: FPSideStats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FPSideStats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title,ratio,percentage")] FPSideStats fPSideStats)
        {
            if (ModelState.IsValid)
            {
                fPSideStats.id = Guid.NewGuid();
                db.FPSidestats.Add(fPSideStats);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fPSideStats);
        }

        // GET: FPSideStats/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FPSideStats fPSideStats = db.FPSidestats.Find(id);
            if (fPSideStats == null)
            {
                return HttpNotFound();
            }
            return View(fPSideStats);
        }

        // POST: FPSideStats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,ratio,percentage")] FPSideStats fPSideStats)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fPSideStats).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fPSideStats);
        }

        // GET: FPSideStats/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FPSideStats fPSideStats = db.FPSidestats.Find(id);
            if (fPSideStats == null)
            {
                return HttpNotFound();
            }
            return View(fPSideStats);
        }

        // POST: FPSideStats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            FPSideStats fPSideStats = db.FPSidestats.Find(id);
            db.FPSidestats.Remove(fPSideStats);
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
