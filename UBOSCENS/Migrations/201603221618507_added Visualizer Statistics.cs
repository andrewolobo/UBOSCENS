namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedVisualizerStatistics : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VisualizerStatistics",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        data = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VisualizerStatistics");
        }
    }
}
