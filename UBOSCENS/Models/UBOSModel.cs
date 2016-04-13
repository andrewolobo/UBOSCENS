using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UBOSCENS.Models
{
    public class UBOSModel
    {
    }
    public class Story
    {
        public Guid id { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public String Image { get; set; }
        public DateTime published { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class Events
    {
        public Guid id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime When { get; set; }
        public bool Active { get; set; }
    }
    public class MapCollection
    {
        public Guid id { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public String data { get; set; }
        public bool active { get; set; }

    }
    public class FPSection
    {
        public Guid id { get; set; }
        public String Title { get; set; }
    }
    public class FrontPageStatistics
    {
        public Guid id { get; set; }
        public Guid SectionID { get; set; }
        public String Title { get; set; }
        public String data { get; set; }
    }
    public class VisualizerStatistics
    {
        public Guid id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }

        public String data { get; set; }
    }
    public class FeaturedStory
    {
        public Guid id { get; set; }
        public String title { get; set; }
        public String Content { get; set; }
        public Guid StoryId { get; set; }
        public String Image { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class Timer
    {
        public Guid id { get; set; }
        public Int32 timer { get; set; }
        public Int32 rate { get; set; }
        public bool Active{get;set;}
    }
    public class PopulationTimer
    {
        public Guid id { get; set; }
        public Int32 count { get; set; }
        public DateTime asOf { get; set; }
        public Int32 rate { get; set; }
        public bool Active { get; set; }
    }
    public class Facts
    {
        public Guid id { get; set; }
        public String data { get; set; }
    }
    public class Table
    {
        public Guid id { get; set; }
        public String Title { get; set; }
        public Guid SectionID { get; set; }
        public String Model { get; set; }
    }
    public class subTable{
        public Guid id { get; set; }
        public Int32 altID { get; set; }
        public String title { get; set; }
        public Guid TableID { get; set; }
    }
    public class Revision
    {
        public Guid id { get; set; }
        public String Title { get; set; }
    }
    public class Section
    {
        public Guid id { get; set; }
        public Guid RevisionID { get; set; }
        public String Title { get; set; }

    }
    public class SubSection
    {
        public Guid id { get; set; }
        public Guid SectionID { get; set; }
        public String Title { get; set; }
        public Guid ImportLogID { get; set; }
        
    }
    public class ImportLog
    {
        public Guid id { get; set; }
        public Guid SectionID { get; set; }
        public Guid TableID { get; set; }
        public Guid SubTableID { get; set; }
        public String Data { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}