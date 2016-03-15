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
    public class SubSectionsController : Controller
    {
        public List<String> th = new List<String>();
        private DatabaseContext db = new DatabaseContext();

        // GET: SubSections
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();
            ViewBag.list = db.Sections.Select(x => x).ToList();
            return View(db.SubSections.ToList());
        }

        // GET: SubSections/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubSection subSection = db.SubSections.Find(id);
            if (subSection == null)
            {
                return HttpNotFound();
            }
            return View(subSection);
        }

        // GET: SubSections/Create
        public ActionResult Create()
        {
            ViewBag.list = db.Sections.Select(x => x).ToList();
            return View();
        }
        public ActionResult Visualize(Guid? id)
        {
            DatabaseContext db = new DatabaseContext();
            var result = db.ImportLogs.Where(x => x.id == id).Select(x => x.Data).First();
            var decoded = JsonConvert.DeserializeObject<Indicator>(result);
            ViewBag.table = getTable(decoded.Tables.First().Categorization.First());
            ViewBag.graph = getGraph(decoded.Tables.First().Categorization.First());
            Debug.WriteLine(th.Count());
            ViewBag.titles = th;
            return View();

            return View();
        }
        public string getGraph(Categorization list)
        {
            List<graphStructure> graph_list = new List<graphStructure>();
            foreach (var item in list.Series)
            {
                graphStructure graphmapper = new graphStructure();
                graphmapper.name = item.Title;
                graphmapper.data = item.SeriesItems.Select(x => Int32.Parse(x.Replace(".00", ""))).ToList();
                graph_list.Add(graphmapper);
            }
            object graph = new { Title = list.Name, xAxis = list.Category.ToArray(), yAxis = graph_list };
            return JsonConvert.SerializeObject(graph);
        }
        //Generates Structure for Dynatables
        public string getTable(Categorization list)
        {
            var h = 0;
            List<object> rows = new List<object>();
            Dictionary<String, String> variable = new Dictionary<string, string>();
            th.Add("Category");
            foreach (var serie in list.Series)
            {
                th.Add(serie.Title);
            }
            for (int x = 0; x < list.Category.Count; x++)
            {
                variable = new Dictionary<string, string>();
                variable.Add("category", list.Category[x]);
                h = x;
                foreach (var serie in list.Series)
                {
                    variable.Add(serie.Title.ToLower(), serie.SeriesItems.ElementAt(h));
                }
                rows.Add(variable);
            }

            return JsonConvert.SerializeObject(rows);
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
                            other_holder.Add(value);
                            otherDic.Add(value, line_identifier);


                            Debug.WriteLine("The Table Data:" + value);
                        }
                    }
                    if (row_identifier == 0)
                    {
                        if (line_identifier % values.Count() > 0)
                        {
                            top_holder.Add(value);
                            Debug.WriteLine("The Header Columns:" + value);
                            seriesID.Add(value, line_identifier);
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
                d.Title = serie.Key;
                //Trick: For each Column in the table, select all the row values from other_holder that match its position value
                d.SeriesItems = other_holder.Where(x => otherDic[x] % col_count == serie.Value).Select(x => x).ToList();
                cat_list.Add(d);
            }
            cat.Category = categorization_category.ToList();
            cat.Series = cat_list;
            List<Categorization> lister = new List<Categorization>();
            lister.Add(cat);
            table.Categorization = lister;
            return JsonConvert.SerializeObject(i);
        }
        // POST: SubSections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,SectionID,Title,ImportLogID")] SubSection subSection, IEnumerable<HttpPostedFileBase> file)
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
            Debug.WriteLine(data);
            if (ModelState.IsValid)
            {
                subSection.id = Guid.NewGuid();
                subSection.ImportLogID = Guid.NewGuid();
                db.SubSections.Add(subSection);
                db.SaveChanges();
                db.ImportLogs.Add(new ImportLog() { id = Guid.NewGuid(), TableID = Guid.NewGuid(), SubTableID = Guid.NewGuid(), CreatedAt = DateTime.Now, SectionID = subSection.id, Data = data });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subSection);
        }

        // GET: SubSections/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubSection subSection = db.SubSections.Find(id);
            if (subSection == null)
            {
                return HttpNotFound();
            }
            return View(subSection);
        }

        // POST: SubSections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,SectionID,Title,ImportLogID")] SubSection subSection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subSection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subSection);
        }

        // GET: SubSections/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubSection subSection = db.SubSections.Find(id);
            if (subSection == null)
            {
                return HttpNotFound();
            }
            return View(subSection);
        }

        // POST: SubSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SubSection subSection = db.SubSections.Find(id);
            db.SubSections.Remove(subSection);
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
