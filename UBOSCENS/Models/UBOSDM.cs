using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UBOSCENS.Models
{
    public class UBOSDM
    {
    }
    public class MOClass
    {
        public String title { get; set; }
        public String version { get; set; }
        public String type { get; set; }
        public String copyright { get; set; }
        public String copyrightShort { get; set; }
        public String copyrightUrl { get; set; }
        public MOcrs crs { get; set; }
        public MOhctransform hc_transform { get; set; }
        public List<MOfeatures> features { get; set; }
    }
    public class MOfeatures
    {
        public String type { get; set; }
        public String id { get; set; }
        public MOproperties properties { get; set; }
        public MOgeometry geometry { get; set; }
    }
    public class MOgeometry
    {
        public String type { get; set; }
        public object coordinates { get; set; }
    }
    public class MOproperties
    {
        public String hc_group { get; set; }
        public float hc_middle_x { get; set; }
        public float hc_middle_y { get; set; }
        public String hc_key { get; set; }
        public String hc_a2 { get; set; }
        public String labelrank { get; set; }
        public String hasc { get; set; }
        public String alt_name { get; set; }
        public String woe_id { get; set; }
        public String sub_region { get; set; }
        public String fips { get; set; }
        public String postal_code { get; set; }
        public String name { get; set; }
        public String country { get; set; }
        public String type_en { get; set; }
        public String region { get; set; }
        public String longitude { get; set; }
        public String woe_name { get; set; }
        public String woe_label { get; set; }
        public String type { get; set; }



    }
    public class MOcrs
    {
        public String type { get; set; }
        public MOcrsproperties properties { get; set; }
    }
    public class MOcrsproperties
    {
        public string name { get; set; }
    }
    public class MOhctransform
    {
        public MOdefault defaults { get; set; }
    }
    public class MOdefault
    {
        public String crs { get; set; }
        public float scale { get; set; }
        public float jsonres { get; set; }
        public float jsonmarginX { get; set; }
        public float jsonmarginY { get; set; }
        public float xoffset { get; set; }
        public float yoffset { get; set; }
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