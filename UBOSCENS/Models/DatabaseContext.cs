using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UBOSCENS.Models
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext()
            : base("UBOSConnection")
        {
        }
        public DbSet<Story> Stories { get; set; }
        public DbSet<FeaturedStory> FeaturedStories { get; set; }
        public DbSet<Timer> Timer { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<subTable> SubTables { get; set; }
        public DbSet<ImportLog> ImportLogs { get; set; }

        public System.Data.Entity.DbSet<UBOSCENS.Models.Revision> Revisions { get; set; }

        public System.Data.Entity.DbSet<UBOSCENS.Models.SubSection> SubSections { get; set; }

        public System.Data.Entity.DbSet<UBOSCENS.Models.FPSection> FPSections { get; set; }

        public System.Data.Entity.DbSet<UBOSCENS.Models.FrontPageStatistics> FrontPageStatistics { get; set; }
        public System.Data.Entity.DbSet<UBOSCENS.Models.PopulationTimer> PopulationTimer { get; set; }
        public System.Data.Entity.DbSet<UBOSCENS.Models.Facts> Facts { get; set; }
        public System.Data.Entity.DbSet<UBOSCENS.Models.VisualizerStatistics> VStats { get; set; }
        public System.Data.Entity.DbSet<UBOSCENS.Models.MapCollection> MapCollection { get; set; }
        public System.Data.Entity.DbSet<UBOSCENS.Models.Events> Events { get; set; }
        public System.Data.Entity.DbSet<UBOSCENS.Models.FPPageStats> FPStats { get; set; }
        public System.Data.Entity.DbSet<UBOSCENS.Models.FPSideStats> FPSidestats { get; set; }
        

    }
}