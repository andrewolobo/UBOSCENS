using Newtonsoft.Json;
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
    public class VisualizerStatisticsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: VisualizerStatistics
        public ActionResult Index()
        {
            return View(db.VStats.ToList());
        }

        // GET: VisualizerStatistics/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisualizerStatistics visualizerStatistics = db.VStats.Find(id);
            if (visualizerStatistics == null)
            {
                return HttpNotFound();
            }
            return View(visualizerStatistics);
        }

        // GET: VisualizerStatistics/Create
        public ActionResult Create()
        {
            return View();
        }
        public String CSVReader(String filename)
        {
            //Directory.GetFiles("/content/images/thumbs")
            //Server.MapPath("~/Content/images/thumbs")
            //var reader = new StreamReader(file_directory);


            var reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/DataDumps/") + filename);
            var line_identifier = 0;
            var row_identifier = 0;
            var col_count = 0;

            //Default Creator
            Indicator i = new Indicator();
            i.Name = "Random Table";
            i.Tables = new List<Tables>();
            UBOSCENS.Models.Tables table = new UBOSCENS.Models.Tables();
            table.Name = "Random Table Name";
            i.Tables.Add(table);

            Categorization cat = new Categorization();
            cat.Name = "Random Table Category";

            List<UBOSCENS.Models.DataSet> cat_list = new List<UBOSCENS.Models.DataSet>();
            List<string> top_holder = new List<string>();
            List<string> other_holder = new List<string>();

            Dictionary<String, Int32> seriesID = new Dictionary<String, Int32>();
            Dictionary<String, Int32> otherDic = new Dictionary<String, Int32>();
            List<String> categorization_category = new List<String>();
            //Prevents Repetion in the Dictionaries
            var guid = Guid.NewGuid();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                if (col_count == 0)
                {
                    col_count = values.Count();
                }
                foreach (var value in values)
                {
                    //Add Items in the First Column to the  Cagegorization.category
                    if (line_identifier % values.Count() == 0 && line_identifier > 0)
                    {
                        categorization_category.Add(value);
                        Debug.WriteLine("Column Categories :" + value);
                    }
                    if (row_identifier > 0)
                    {
                        if (line_identifier % values.Count() > 0)
                        {
                            guid = Guid.NewGuid();
                            other_holder.Add(value + "_" + guid);
                            otherDic.Add(value + "_" + guid, line_identifier);
                            Debug.WriteLine("The Table Data:" + value);
                        }
                    }
                    if (row_identifier == 0)
                    {
                        if (line_identifier % values.Count() > 0)
                        {
                            guid = Guid.NewGuid();
                            top_holder.Add(value + "_" + guid);
                            Debug.WriteLine("The Header Columns:" + value);
                            seriesID.Add(value + "_" + guid, line_identifier);
                        }
                    }
                    //Add the Name of the first Column as the Name of the categorization
                    if (line_identifier == 0)
                    {
                        cat.Name = values[0];
                        Debug.WriteLine("Left Top Column Value:" + value);

                    }

                    line_identifier++;
                }
                row_identifier++;
            }
            foreach (var serie in seriesID)
            {
                UBOSCENS.Models.DataSet d = new UBOSCENS.Models.DataSet();
                d.Title = serie.Key.Split(' ')[0];
                //Trick: For each Column in the table, select all the row values from other_holder that match its position value
                var list = other_holder.Where(x => otherDic[x] % col_count == serie.Value).Select(x => x).ToList();
                d.SeriesItems = new List<string>();
                foreach (var item in list)
                {
                    d.SeriesItems.Add((item.Split('_')).Count() >= 1 ? (item.Split('_'))[0] : item);
                }

                cat_list.Add(d);
            }
            cat.Category = categorization_category.ToList();
            cat.Series = cat_list;
            List<Categorization> lister = new List<Categorization>();
            lister.Add(cat);
            table.Categorization = lister;
            return JsonConvert.SerializeObject(i);
        }
        // POST: VisualizerStatistics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Title,Description,data")] VisualizerStatistics visualizerStatistics, IEnumerable<HttpPostedFileBase> file)
        {
            var data = "";
            Debug.WriteLine(file.Count());
            if (file.ElementAt(0) != null && file.ElementAt(0).ContentLength > 0)
            {

                string path = Path.Combine(Server.MapPath("~/Uploads/DataDumps/"),
                Path.GetFileName(file.ElementAt(0).FileName));
                file.ElementAt(0).SaveAs(path);
                data = CSVReader(file.ElementAt(0).FileName);
            }
            else
            {
                //ViewBag.Message = "You have not specified a file.";
            }
            if (ModelState.IsValid)
            {
                visualizerStatistics.id = Guid.NewGuid();
                db.VStats.Add(visualizerStatistics);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(visualizerStatistics);
        }

        // GET: VisualizerStatistics/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisualizerStatistics visualizerStatistics = db.VStats.Find(id);
            if (visualizerStatistics == null)
            {
                return HttpNotFound();
            }
            return View(visualizerStatistics);
        }

        // POST: VisualizerStatistics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Title,Description,data")] VisualizerStatistics visualizerStatistics)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visualizerStatistics).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(visualizerStatistics);
        }

        // GET: VisualizerStatistics/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisualizerStatistics visualizerStatistics = db.VStats.Find(id);
            if (visualizerStatistics == null)
            {
                return HttpNotFound();
            }
            return View(visualizerStatistics);
        }

        // POST: VisualizerStatistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            VisualizerStatistics visualizerStatistics = db.VStats.Find(id);
            db.VStats.Remove(visualizerStatistics);
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
