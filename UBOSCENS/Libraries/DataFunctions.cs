using LinqToExcel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using UBOSCENS.Models;

namespace UBOSCENS.Libraries
{
    public class DataFunctions
    {
        public String CSVReader(String filename)
        {
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
        public Dictionary<string, Dictionary<String, String>> ExcelImportNew()
        {
            var oprop = "";
            Dictionary<String, String> mapcollection = new Dictionary<String, String>();
            string line = "";
            string total = "";
            oprop = "districts.file";
            string location = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/DistrictImport/") + "/" + oprop;
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
            var item = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<String, String>>>(total);
            return item;
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
        public String ExcelImport()
        {
            var file = "DDistrictImport.xlsx";
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
                    data.Add("Male Resident Population", iv.MaleResidentPopulation);
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
        public String ExcelImport(bool value)
        {
            var file = "DDistrictImport.xlsx";
            var excel = new ExcelQueryFactory();
            excel.FileName = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/DistrictImport/") + file;
            Dictionary<String, Dictionary<String, String>> DistrictData = new Dictionary<string, Dictionary<String, String>>();
            Dictionary<int, String> headers = new Dictionary<int, string>();
            var list = excel.WorksheetNoHeader().ToList();
            var flag = 0;
            foreach (var iv in list)
            {
                if (flag == 0)
                {
                    foreach (var item in iv)
                    {
                        headers.Add(flag, iv[flag]);
                        flag++;
                    }
                }
                else
                {
                    if (iv[0].ToString().Length > 1)
                    {
                        Dictionary<string, string> data = new Dictionary<string, string>();
                        for (var index = 1; index < headers.Count(); index++)
                        {
                            data.Add(headers[index], iv[index]);
                        }
                        DistrictData.Add(iv[0], data);
                    }
                }


            }
            return JsonConvert.SerializeObject(DistrictData);
        }

        public Dictionary<string, Dictionary<String, String>> ExcelImport(String file)
        {
            var excel = new ExcelQueryFactory();
            excel.FileName = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/DistrictImport/") + file;
            Dictionary<String, Dictionary<String, String>> DistrictData = new Dictionary<string, Dictionary<String, String>>();
            Dictionary<int, String> headers = new Dictionary<int, string>();
            var list = excel.WorksheetNoHeader().ToList();
            var flag = 0;
            foreach (var iv in list)
            {
                if (flag == 0)
                {
                    foreach (var item in iv)
                    {
                        headers.Add(flag, iv[flag]);
                        flag++;
                    }
                }
                else
                {
                    if (iv[0].ToString().Length>1)
                    {
                        Dictionary<string, string> data = new Dictionary<string, string>();
                        for (var index = 1; index < headers.Count();index++ )
                        {
                            data.Add(headers[index], iv[index]);
                        }
                        DistrictData.Add(iv[0], data);
                    }
                }


            }
            return DistrictData;
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
        public Int32 MaleResidentPopulation { get; set; }
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
    }
    class DataDescriptor : Attribute
    {


        public String Title { get; set; }
        public String Description { get; set; }
        public Int32 Version { get; set; }
    }
}