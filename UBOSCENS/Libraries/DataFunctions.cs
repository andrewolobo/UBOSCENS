using LinqToExcel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using UBOSCENS.Models;

namespace UBOSCENS.Libraries
{
    public class DataFunctions
    {
        public Dictionary<string, string> ParseMap(string oprop)
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
                mapcollection.Add(s.id, s.properties.name);
                Debug.WriteLine(s.properties.name);
            }
            return mapcollection;
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

        public String ExcelImport(String file)
        {
            var excel = new ExcelQueryFactory();
            excel.FileName = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/DistrictImport/") + file;
            var i = from x in excel.Worksheet<DistrictClass>()
                    select x;
            Dictionary<String, Dictionary<String, Int32>> DistrictData = new Dictionary<string, Dictionary<String, Int32>>();
            foreach (var iv in i)
            {
                if (iv.District != null)
                {
                    Dictionary<string, Int32> data = new Dictionary<string, Int32>();
                    data.Add("Male Resident Population", iv.Male);
                    data.Add("Female Resident Population", iv.Female);
                    data.Add("Rural Population", iv.Rural);
                    data.Add("Urban Population", iv.Urban);
                    data.Add("HouseHold Population", iv.Household);
                    data.Add("Non-HouseHold Population", iv.NonHousehold);
                    data.Add("District Total", iv.Total);
                    DistrictData.Add(iv.District, data);
                }

            }
            return JsonConvert.SerializeObject(DistrictData);
        }
        public string getMap(Categorization list, string location)
        {
            var h = 0;
            var mapcollection = ParseMap(location);
            var cupetit = "";
            List<object> rows = new List<object>();
            Dictionary<String, String> variable = new Dictionary<string, string>();
            //th.Add("Category");
            foreach (var serie in list.Series)
            {
                //th.Add(serie.Title);
            }
            for (int x = 0; x < list.Category.Count; x++)
            {
                variable = new Dictionary<string, string>();
                variable.Add("hc-key", mapcollection.ContainsValue(list.Category[x]) ? mapcollection.FirstOrDefault(v => v.Value.Equals(list.Category[x])).Key : list.Category[x]);
                h = x;
                foreach (var serie in list.Series)
                {
                    cupetit = serie.SeriesItems.ElementAt(h);
                    variable.Add("value", serie.SeriesItems.ElementAt(h));
                }
                rows.Add(new { hc_key = mapcollection.ContainsValue(list.Category[x]) ? mapcollection.FirstOrDefault(v => v.Value.Equals(list.Category[x])).Key : list.Category[x], value = Int32.Parse(cupetit) });
            }

            return ((JsonConvert.SerializeObject(rows)).Replace("_", "-")).Replace("UG.", "ug-");
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
    }
    class DistrictClass
    {
        [DataDescriptor(Title = "District Name")]
        public String District { get; set; }
        [DataDescriptor(Title = "Male Residents")]
        public Int32 Male { get; set; }
        [DataDescriptor(Title = "Female Resident Population")]
        public Int32 Female { get; set; }
        [DataDescriptor(Title = "Rural Population")]
        public Int32 Rural { get; set; }
        [DataDescriptor(Title = "Urban Population")]
        public Int32 Urban { get; set; }
        [DataDescriptor(Title = "Household Population")]
        public Int32 Household { get; set; }
        [DataDescriptor(Title = "Non-household Population")]
        public Int32 NonHousehold { get; set; }
        [DataDescriptor(Title = "District Total")]
        public Int32 Total { get; set; }
        //var name = player.GetAttributeFrom<DisplayAttribute>("PlayerDescription").Name;
        //public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        //{
        //    var attrType = typeof(T);
        //    var property = instance.GetType().GetProperty(propertyName);
        //    return (T)property.GetCustomAttributes(attrType, false).First();
        //}
    }
    class DataDescriptor : Attribute
    {


        public String Title { get; set; }
        public String Description { get; set; }
        public Int32 Version { get; set; }
    }
}