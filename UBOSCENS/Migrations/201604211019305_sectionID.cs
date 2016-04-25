namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sectionID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VisualizerStatistics", "sectionID", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VisualizerStatistics", "sectionID");
        }
    }
}
