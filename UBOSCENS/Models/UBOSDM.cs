using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UBOSCENS.Models
{
    public class UBOSDM
    {
    }
    public class Indicator
    {
        public String Name { get; set; }
        public List<Tables> Tables { get; set; }
    }
    public class fStats
    {
        public Guid id { get; set; }
        public List<fStatObject> statistic { get; set; }
    }
    public class fStatObject
    {
        public Guid id { get; set; }
        public String title { get; set; }
        //Graph, Comparison, Counter. e.t.c
        public String Type { get; set; }
        public List<String> Item { get; set; }
    }
    public class Tables
    {
        public String Name { get; set; }
        //These could also be considered as subtables
        public List<Categorization> Categorization { get; set; }

    }
    public class Categorization
    {
        public String Name { get; set; }

        //The Categories and Series should be of the same length. They are a match
        public List<String> Category { get; set; }
        public List<DataSet> Series { get; set; }
    }

    public class DataSet
    {
        public String Title { get; set; }
        public List<String> SeriesItems { get; set; }
    }


}