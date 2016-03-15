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
    public class FrontPageStatisticsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: FrontPageStatistics
        public ActionResult Index()
        {
            return View(db.FrontPageStatistics.ToList());
        }

        // GET: FrontPageStatistics/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FrontPageStatistics frontPageStatistics = db.FrontPageStatistics.Find(id);
            if (frontPageStatistics == null)
            {
                return HttpNotFound();
            }
            return View(frontPageStatistics);
        }

        // GET: FrontPageStatistics/Create
        public ActionResult Create()
        {
            var list = db.FPSections.Select(x => x).ToList();
            ViewBag.list = list;
            return View();
        }

        // POST: FrontPageStatistics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,SectionID,Title,data")] FrontPageStatistics frontPageStatistics)
        {
            if (ModelState.IsValid)
            {
                frontPageStatistics.id = Guid.NewGuid();
                db.FrontPageStatistics.Add(frontPageStatistics);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(frontPageStatistics);
        }

        // GET: FrontPageStatistics/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FrontPageStatistics frontPageStatistics = db.FrontPageStatistics.Find(id);
            if (frontPageStatistics == null)
            {
                return HttpNotFound();
            }
            return View(frontPageStatistics);
        }

        // POST: FrontPageStatistics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,SectionID,Title,data")] FrontPageStatistics frontPageStatistics)
        {
            if (ModelState.IsValid)
            {
                db.Entry(frontPageStatistics).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(frontPageStatistics);
        }

        // GET: FrontPageStatistics/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FrontPageStatistics frontPageStatistics = db.FrontPageStatistics.Find(id);
            if (frontPageStatistics == null)
            {
                return HttpNotFound();
            }
            return View(frontPageStatistics);
        }

        // POST: FrontPageStatistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            FrontPageStatistics frontPageStatistics = db.FrontPageStatistics.Find(id);
            db.FrontPageStatistics.Remove(frontPageStatistics);
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
