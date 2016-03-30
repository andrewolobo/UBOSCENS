﻿using Newtonsoft.Json;
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
using VersFx.Formats.Text.Epub;
using VersFx.Formats.Text.Epub.Entities;

namespace UBOSCENS.Controllers.Admin
{
    public class DataImportController : Controller
    {
        public List<String> th = new List<String>();
        private DatabaseContext db = new DatabaseContext();
        public String file = "Report.epub";
        public String oprop = "mapdata.txt";

        // GET: DataImport
        public ActionResult Index()
        {
            return View(db.ImportLogs.ToList());
        }
        public String SelectActions()
        {
            return "";

        }
        public ActionResult AddToTable()
        {
            return View();
        }
        public Dictionary<string, string> ParseMap()
        {
            Dictionary<String, String> mapcollection = new Dictionary<String, String>();
            string line = "";
            string total = "";
            string location = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/UGD44/") + "/" + oprop;
            System.IO.StreamReader file = new System.IO.StreamReader(location);
            if (System.IO.File.Exists(location))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line != null)
                    {
                        total += line;
                    }
                }

            }
            var item = JsonConvert.DeserializeObject<MOClass>(total);
            foreach (var s in item.features)
            {
                mapcollection.Add(s.id,s.properties.name);
                Debug.WriteLine(s.properties.name);
            }
            return mapcollection;
        }
        public ActionResult MapExcel()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MapExcel([Bind(Include = "id,name,description,data,active")] MapCollection maps, IEnumerable<HttpPostedFileBase> file)
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
                maps.id = Guid.NewGuid();
                maps.data = data;
                db.MapCollection.Add(maps);
                db.SaveChanges();
                return RedirectToAction("Index", "FP");
            }
            return View(maps);
        }
        public String ReturnMap(Guid? id)
        {
            DatabaseContext db = new DatabaseContext();
            var maps = db.MapCollection.Where(x => x.id == id).Select(x => x).FirstOrDefault();
            var indicator = JsonConvert.DeserializeObject<Indicator>(maps.data);
            var cat = indicator.Tables.First().Categorization.First();
            return getMap(cat);

        }
        [HttpPost]
        public ActionResult AddTable(IEnumerable<HttpPostedFileBase> file)
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
            var alog = db.ImportLogs.Select(x => x).First();
            var ilog = JsonConvert.DeserializeObject<Indicator>(alog.Data);
            ilog.Tables.Add(ilog.Tables.First());
            alog.Data = JsonConvert.SerializeObject(ilog);
            db.SaveChanges();
            return RedirectToAction("Index", "DataImport");
        }
        public String setTimer()
        {
            DatabaseContext db = new DatabaseContext();
            db.PopulationTimer.Select(x => x).ToList().ForEach(i => i.Active = false);
            db.SaveChanges();
            db.PopulationTimer.Add(new PopulationTimer() { rate = 19, count = 24227297, asOf = new DateTime(2002, 01, 01), Active = true });
            db.SaveChanges();
            return null;
        }
        public String getTimer()
        {
            DatabaseContext db = new DatabaseContext();
            var timer = db.PopulationTimer.Where(x => x.Active == true).Select(x => x).First();
            var new_people = DateTime.Now.Subtract(timer.asOf).TotalSeconds / timer.rate;
            var population = timer.count + new_people;
            return JsonConvert.SerializeObject(new { population = (Int32)population, rate = timer.rate });

        }
        // GET: DataImport/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportLog importLog = db.ImportLogs.Find(id);
            if (importLog == null)
            {
                return HttpNotFound();
            }
            return View(importLog);
        }
        public String getRevisions()
        {
            var data = db.Revisions.Select(x => x);
            return JsonConvert.SerializeObject(data);

        }
        public String getSections(Guid? id)
        {
            var data = db.Sections.Where(x => x.RevisionID == id).Select(x => x);
            return JsonConvert.SerializeObject(data);

        }
        public String getSubSections(Guid? id)
        {
            var data = db.SubSections.Where(x => x.SectionID == id).Select(x => x);
            return JsonConvert.SerializeObject(data);
        }
        public List<EpubChapter> Chapters()
        {
            EpubBook epub = EpubReader.OpenBook(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Epub/") + file);
            List<EpubChapter> chapters = epub.Chapters.Where(x => x.Title.Contains("CHAPTER")).Select(x => x).ToList();
            return chapters;
        }
        public EpubChapter getNextChapter(String chapterName)
        {
            EpubBook epub = EpubReader.OpenBook(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Epub/") + file);
            EpubChapter chapters = epub.Chapters.SkipWhile(item => item.Title != chapterName).Skip(1).FirstOrDefault();
            return chapters;
        }
        public EpubChapter getPreviousChapter(String chapterName)
        {
            EpubBook epub = EpubReader.OpenBook(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Epub/") + file);
            epub.Chapters.Reverse();
            EpubChapter chapters = epub.Chapters.SkipWhile(item => item.Title != chapterName).Skip(1).FirstOrDefault();
            return chapters;
        }
        public EpubChapter getPreviousSubChapter(String chapterName)
        {
            EpubBook epub = EpubReader.OpenBook(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Epub/") + file);
            epub.Chapters.Reverse();
            EpubChapter chapters = epub.Chapters.Where(x => x.Title == chapterName).First().SubChapters.SkipWhile(item => item.Title != chapterName).Skip(1).FirstOrDefault();
            return chapters;
        }
        public EpubChapter getNextSubChapter(String chapterName)
        {
            EpubBook epub = EpubReader.OpenBook(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Epub/") + file);
            EpubChapter chapters = epub.Chapters.Where(x => x.Title == chapterName).First().SubChapters.SkipWhile(item => item.Title != chapterName).Skip(1).FirstOrDefault();
            return chapters;
        }
        public ActionResult getContent(String chaptername)
        {
            EpubBook epub = EpubReader.OpenBook(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Epub/") + file);
            String stuff = "";
            foreach (EpubChapter chapter in epub.Chapters)
            {
                string chapterTitle = chapter.Title;
                string chapterHtmlContent = chapter.HtmlContent;
                if (chapterTitle.Contains(chaptername))
                {
                    stuff += chapterHtmlContent;
                }
                foreach (var subChapter in chapter.SubChapters)
                {
                    if (subChapter.Title.Contains(chaptername))
                    {
                        stuff += subChapter.HtmlContent;
                    }
                }
            }
            ViewBag.content = stuff;
            return View();
        }
        public ActionResult getChapters()
        {
            ViewBag.chapters = Chapters();
            return View();
        }
        public ActionResult VisualizerHome()
        {
            DatabaseContext db = new DatabaseContext();
            var statList = db.VStats.Select(x => x).ToList();
            ViewBag.stats = statList;
            return View();
        }
        public String epubReader()
        {
            EpubBook epub = EpubReader.OpenBook(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Epub/") + file);
            String stuff = "";
            foreach (EpubChapter chapter in epub.Chapters)
            {
                string chapterTitle = chapter.Title;
                string chapterHtmlContent = chapter.HtmlContent;
                stuff += chapterTitle + " ";
                if (chapterTitle.Contains("CHAPTER 3: POPULATION CHARACTERISTICS"))
                {
                    stuff += chapterHtmlContent;
                }
                List<EpubChapter> subChapters = chapter.SubChapters;
            }
            return stuff;
        }
        public String getTableData(Guid? id)
        {
            var data = db.ImportLogs.Where(x => x.SectionID == id).Select(x => x);
            return JsonConvert.SerializeObject(data);

        }
        public String getFilter(Guid? identifier, List<String> data)
        {

            DatabaseContext db = new DatabaseContext();
            var result = db.VStats.Where(x => x.id == identifier).Select(x => x.data).First();
            var decoded = JsonConvert.DeserializeObject<Indicator>(result);
            Categorization global = decoded.Tables.First().Categorization.First();
            Categorization filter = new Categorization();
            filter.Name = identifier.ToString();
            filter.Series = global.Series;
            filter.Category = new List<string>();
            filter.Series.ForEach(n =>
            {
                n.SeriesItems = new List<string>();
            });
            Categorization globals = JsonConvert.DeserializeObject<Indicator>(result).Tables.First().Categorization.First();

            foreach (var d in data)
            {
                foreach (var n in global.Category.Select((value, i) => new { i, value }))
                {

                    if (n.value.Equals(d))
                    {
                        filter.Category.Add(d);
                        foreach (var serie in globals.Series)
                        {
                            var item = filter.Series.Where(x => x.Title.Equals(serie.Title)).Select(x => x.SeriesItems).FirstOrDefault();
                            item.Add(serie.SeriesItems.ElementAt(n.i));
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject(new { Graph = getGraphRaw(filter), Table = getTableRaw(filter) });


        }
        public ActionResult Visualize(Guid? id)
        {
            if (id != null)
            {
                DatabaseContext db = new DatabaseContext();
                var result = db.VStats.Where(x => x.id == id).Select(x => x.data).First();
                var decoded = JsonConvert.DeserializeObject<Indicator>(result);
                var selections = decoded.Tables.First().Categorization.First().Category;
                ViewBag.selections = selections;
                ViewBag.identifier = id;
                ViewBag.table = getTable(decoded.Tables.First().Categorization.First());
                ViewBag.graph = getGraph(decoded.Tables.First().Categorization.First());
                Debug.WriteLine(th.Count());
                ViewBag.titles = th;
            }
            return View();
        }
        public ActionResult VisualizeCurrent(Guid? id)
        {
            DatabaseContext db = new DatabaseContext();
            var result = db.VStats.Where(x => x.id == id).Select(x => x.data).First();
            var decoded = JsonConvert.DeserializeObject<Indicator>(result);
            ViewBag.table = getTable(decoded.Tables.First().Categorization.First());
            ViewBag.graph = getGraph(decoded.Tables.First().Categorization.First());
            Debug.WriteLine(th.Count());
            ViewBag.titles = th;
            return View();
        }
        public String getGraphData(Guid? id)
        {
            var result = JsonConvert.DeserializeObject<Indicator>(db.FrontPageStatistics.Where(x => x.id == id).Select(x => x.data).First());
            var decoded = getGraph(result.Tables.First().Categorization.First());
            return decoded;
        }
        // GET: DataImport/Create
        public ActionResult Create()
        {
            return View();
        }
        public string getGraph(Categorization list)
        {
            List<graphStructure> graph_list = new List<graphStructure>();
            foreach (var item in list.Series)
            {
                graphStructure graphmapper = new graphStructure();
                graphmapper.name = item.Title;
                graphmapper.data = item.SeriesItems.Select(x => Convert.ToDouble(x.Replace(".00", ""))).ToList();
                graph_list.Add(graphmapper);
            }
            object graph = new { Title = list.Name, xAxis = list.Category.ToArray(), yAxis = graph_list };
            return JsonConvert.SerializeObject(graph);
        }
        public string getMap(Categorization list)
        {
            var h = 0;
            var mapcollection = ParseMap();
            var cupetit = "";
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
                variable.Add("hc-key", mapcollection.ContainsValue(list.Category[x])?mapcollection.FirstOrDefault(v => v.Value.Equals(list.Category[x])).Key:list.Category[x]);
                h = x;
                foreach (var serie in list.Series)
                {
                    cupetit = serie.SeriesItems.ElementAt(h);
                    variable.Add("value",serie.SeriesItems.ElementAt(h));
                }
                rows.Add(new { hc_key = mapcollection.ContainsValue(list.Category[x]) ? mapcollection.FirstOrDefault(v => v.Value.Equals(list.Category[x])).Key : list.Category[x], value = Int32.Parse(cupetit) });
            }

            return ((JsonConvert.SerializeObject(rows)).Replace("_", "-")).Replace("UG.","ug-");
        }
        public Object getGraphRaw(Categorization list)
        {
            List<graphStructure> graph_list = new List<graphStructure>();
            foreach (var item in list.Series)
            {
                graphStructure graphmapper = new graphStructure();
                graphmapper.name = item.Title;
                graphmapper.data = item.SeriesItems.Select(x => Convert.ToDouble(x.Replace(".00", ""))).ToList();
                graph_list.Add(graphmapper);
            }
            object graph = new { Title = list.Name, xAxis = list.Category.ToArray(), yAxis = graph_list };
            return graph;
        }
        public Object getTableRaw(Categorization list)
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

            return rows;
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
                        if (!value.Equals(""))
                        {
                            categorization_category.Add(value);
                        }

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
                var value = serie.Key.Split('_')[0];
                d.Title = value;
                d.Title = d.Title.ToLower();
                //Trick: For each Column in the table, select all the row values from other_holder that match its position value
                var list = other_holder.Where(x => otherDic[x] % col_count == serie.Value).Select(x => x).ToList();
                d.SeriesItems = new List<string>();
                foreach (var item in list)
                {
                    if (!(((item.Split('_')).Count() >= 1 ? (item.Split('_'))[0] : item)).Equals(""))
                    {
                        d.SeriesItems.Add((item.Split('_')).Count() >= 1 ? (item.Split('_'))[0] : item);
                    }

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
        // POST: DataImport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Data")] ImportLog importLog, IEnumerable<HttpPostedFileBase> file)
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
                importLog.id = Guid.NewGuid();
                importLog.SectionID = Guid.NewGuid();
                importLog.TableID = Guid.NewGuid();
                importLog.SubTableID = Guid.NewGuid();
                importLog.Data = data;
                importLog.CreatedAt = DateTime.Now;
                db.ImportLogs.Add(importLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(importLog);
        }

        // GET: DataImport/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportLog importLog = db.ImportLogs.Find(id);
            if (importLog == null)
            {
                return HttpNotFound();
            }
            return View(importLog);
        }

        // POST: DataImport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,SectionID,TableID,SubTableID,Data,CreatedAt")] ImportLog importLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(importLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(importLog);
        }

        // GET: DataImport/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportLog importLog = db.ImportLogs.Find(id);
            if (importLog == null)
            {
                return HttpNotFound();
            }
            return View(importLog);
        }

        // POST: DataImport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ImportLog importLog = db.ImportLogs.Find(id);
            db.ImportLogs.Remove(importLog);
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
