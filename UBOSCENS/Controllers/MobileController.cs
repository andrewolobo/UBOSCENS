using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UBOSCENS.Models;

namespace UBOSCENS.Controllers
{
    public class MobileController : Controller
    {
        public List<String> th = new List<String>();
        // GET: Mobile
        public ActionResult Index()
        {
            ViewBag.table = getTable(midComplexDemo());
            ViewBag.graph = getGraph(midComplexDemo());
            Debug.WriteLine(th.Count());
            ViewBag.titles = th;
            return View();
        }
        public ActionResult GraphDemo()
        {
            ViewBag.table = getTable(midComplexDemo());
            ViewBag.graph = getGraph(midComplexDemo());
            Debug.WriteLine(th.Count());
            ViewBag.titles = th;
            return View();
        }
        public Categorization Initial()
        {
            Indicator i = new Indicator();
            i.Name = "Percentage Distribution of Institutional Population by type of institution by Sex";
            Tables percentage_d = new Tables();
            percentage_d.Name = "Percentage Distribution of Institutional Population by type of institution by Sex";
            Categorization pop = new Categorization();
            pop.Name = "Population Type";
            pop.Category = ("Educational Religious Health Hostel Prison Orphanage/Reformatory Refugees IDP_Camps").Split(' ').ToList();
            DataSet master = new DataSet();
            pop.Series = new List<DataSet>();
            master.Title = "Male";
            master.SeriesItems = ("44,1347,35023,03031,85035,2805,14553,9007").Split(',').ToList();
            pop.Series.Add(master);
            master = new DataSet();
            master.Title = "Female";
            master.SeriesItems = ("44,1347,35023,03031,85035,2805,14553,9007").Split(',').ToList();
            pop.Series.Add(master);
            List<Categorization> pop_types = new List<Categorization>();
            pop_types.Add(pop);
            percentage_d.Categorization = pop_types;
            i.Tables = new List<Tables>() { percentage_d };
            return i.Tables.First().Categorization.First();
        }
        public Categorization midComplexDemo()
        {
            Indicator i = new Indicator();
            i.Name = "Population by Residence, Region and Sex";
            Tables residence = new Tables();
            residence.Name = "Residence";

            Categorization rural = new Categorization();
            rural.Name = "By Residence";
            rural.Category = ("Rural, Urban").Split(',').ToList();
            List<DataSet> l_series = new List<DataSet>();

            DataSet rural_series = new DataSet();
            rural_series.Title = "Male(1991)";
            rural_series.SeriesItems = ("7,243,221 7,538,862").Split(' ').ToList();
            l_series.Add(rural_series);

            rural_series = new DataSet();
            rural_series.Title = "Female(1991)";
            rural_series.SeriesItems = ("7,243,221 7,538,862").Split(' ').ToList();
            l_series.Add(rural_series);
            rural.Series = l_series;

            rural_series = new DataSet();
            rural_series.Title = "Male(2004)";
            rural_series.SeriesItems = ("7,243,221 7,538,862").Split(' ').ToList();
            l_series.Add(rural_series);

            rural_series = new DataSet();
            rural_series.Title = "Female(2004)";
            rural_series.SeriesItems = ("7,243,221 7,538,862").Split(' ').ToList();
            l_series.Add(rural_series);

            rural.Series = l_series;



            //Create a List of Categorizations
            List<Categorization> c_list = new List<Categorization>();
            c_list.Add(rural);
            residence.Categorization = c_list;
            //Create a list of tables
            List<Tables> tables = new List<Tables>();
            tables.Add(residence);
            i.Tables = tables;
            return i.Tables.First().Categorization.First();
        }
        public string getGraph(Categorization list)
        {
            List<graphStructure> graph_list = new List<graphStructure>();
            foreach (var item in list.Series)
            {
                graphStructure graphmapper = new graphStructure();
                graphmapper.name = item.Title;
                graphmapper.data = item.SeriesItems.Select(x => Convert.ToDouble(x.Replace(",", ""))).ToList();
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

    }

}
class graphStructure
{
    public string name { get; set; }
    public List<Double> data { get; set; }
}
