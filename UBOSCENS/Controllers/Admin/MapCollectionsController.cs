using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UBOSCENS.Libraries;
using UBOSCENS.Models;

namespace UBOSCENS.Controllers.Admin
{
    public class MapCollectionsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: MapCollections
        public ActionResult Index()
        {
            return View(db.MapCollection.ToList());
        }

        // GET: MapCollections/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MapCollection mapCollection = db.MapCollection.Find(id);
            if (mapCollection == null)
            {
                return HttpNotFound();
            }
            return View(mapCollection);
        }

        // GET: MapCollections/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MapCollections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,description,data,active")] MapCollection mapCollection, IEnumerable<HttpPostedFileBase> file)
        {
            DataFunctions d = new DataFunctions();
            var data = "";
            Debug.WriteLine(file.Count());
            if (file.ElementAt(0) != null && file.ElementAt(0).ContentLength > 0)
            {

                string path = Path.Combine(Server.MapPath("~/Uploads/DataDumps/"),
                Path.GetFileName(file.ElementAt(0).FileName));
                file.ElementAt(0).SaveAs(path);
                data = d.CSVReader(file.ElementAt(0).FileName);
            }
            else
            {
                //ViewBag.Message = "You have not specified a file.";
            }
            if (ModelState.IsValid)
            {
                mapCollection.id = Guid.NewGuid();
                mapCollection.data = data;
                db.MapCollection.Add(mapCollection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mapCollection);
        }

        // GET: MapCollections/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MapCollection mapCollection = db.MapCollection.Find(id);
            if (mapCollection == null)
            {
                return HttpNotFound();
            }
            return View(mapCollection);
        }

        // POST: MapCollections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,data,active")] MapCollection mapCollection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mapCollection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mapCollection);
        }

        // GET: MapCollections/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MapCollection mapCollection = db.MapCollection.Find(id);
            if (mapCollection == null)
            {
                return HttpNotFound();
            }
            return View(mapCollection);
        }

        // POST: MapCollections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            MapCollection mapCollection = db.MapCollection.Find(id);
            db.MapCollection.Remove(mapCollection);
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
