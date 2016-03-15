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
using UBOSCENS.Models;

namespace UBOSCENS.Controllers.Admin
{
    public class FeaturedStoriesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: FeaturedStories
        public ActionResult Index()
        {
            return View(db.FeaturedStories.ToList());
        }

        // GET: FeaturedStories/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeaturedStory featuredStory = db.FeaturedStories.Find(id);
            if (featuredStory == null)
            {
                return HttpNotFound();
            }
            return View(featuredStory);
        }

        // GET: FeaturedStories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeaturedStories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "title,Content,Image,Active")] FeaturedStory featuredStory, IEnumerable<HttpPostedFileBase> file, HttpPostedFileBase files)
        {
            Debug.WriteLine(file.Count());
            if (file.ElementAt(0) != null && file.ElementAt(0).ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Uploads/FeaturedImages/"),
                    Path.GetFileName(file.ElementAt(0).FileName));
                    file.ElementAt(0).SaveAs(path);
                    featuredStory.Image = "~/Uploads/FeaturedImages/" + file.ElementAt(0).FileName;
                }

                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    Debug.WriteLine("Shit Happened");
                }
            else
            {
                Debug.WriteLine("Image is empty");
                //ViewBag.Message = "You have not specified a file.";
            }
            if (files != null && files.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Uploads/FeaturedImages/"),
                    Path.GetFileName(files.FileName));
                    files.SaveAs(path);
                    featuredStory.Image = "~/Uploads/FeaturedImages/" + file.ElementAt(0).FileName;
                }

                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    Debug.WriteLine("Shit Happened");
                }
            else
            {
                Debug.WriteLine("Image is empty");
                //ViewBag.Message = "You have not specified a file.";
            }
            if (ModelState.IsValid)
            {
                featuredStory.id = Guid.NewGuid();
                featuredStory.CreatedAt = DateTime.Now;
                db.FeaturedStories.Add(featuredStory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(featuredStory);
        }

        // GET: FeaturedStories/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeaturedStory featuredStory = db.FeaturedStories.Find(id);
            if (featuredStory == null)
            {
                return HttpNotFound();
            }
            return View(featuredStory);
        }

        // POST: FeaturedStories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,Content,StoryId,Image,Active,CreatedAt")] FeaturedStory featuredStory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(featuredStory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(featuredStory);
        }

        // GET: FeaturedStories/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeaturedStory featuredStory = db.FeaturedStories.Find(id);
            if (featuredStory == null)
            {
                return HttpNotFound();
            }
            return View(featuredStory);
        }

        // POST: FeaturedStories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            FeaturedStory featuredStory = db.FeaturedStories.Find(id);
            db.FeaturedStories.Remove(featuredStory);
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
