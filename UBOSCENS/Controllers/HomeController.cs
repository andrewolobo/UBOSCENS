using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UBOSCENS.Libraries;
using UBOSCENS.Models;

namespace UBOSCENS.Controllers
{
    public class StatisticsModel
    {
        public Guid id;
        public String Title;
        public Dictionary<String, String> data = new Dictionary<string, string>();
    }
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();
            DataFunctions d = new DataFunctions();
            var statList = db.VStats.Select(x => x).Take(3).ToList();
            foreach (var stat in statList)
            {
                stat.data = d.getGraph((JsonConvert.DeserializeObject<Indicator>(stat.data)).Tables.First().Categorization.First());
            }
            ViewBag.upperstat = statList;
            var facts = db.Facts.Select(x => x);
            List<StatisticsModel> allStats = new List<StatisticsModel>();
            var sections = db.FPSections.Select(x=>x);
            foreach(var section in sections){
                StatisticsModel stat = new StatisticsModel();
                stat.id = section.id;
                stat.Title= section.Title;
                var results = db.FrontPageStatistics.Where(x=>x.SectionID==section.id);
                foreach(var result in results){
                    stat.data.Add(result.Title,result.id.ToString());
                }
                allStats.Add(stat);
            }
            var stories = db.FeaturedStories.Where(x => x.Active == true).Select(x => x).Take(5);
            ViewBag.stats = allStats;
            ViewBag.facts = facts;
            ViewBag.statistics = JsonConvert.SerializeObject(allStats);
            ViewBag.stories = stories.ToList();
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}