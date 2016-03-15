namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FPSections2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FPSections",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.FrontPageStatistics",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        SectionID = c.Guid(nullable: false),
                        Title = c.String(),
                        data = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FrontPageStatistics");
            DropTable("dbo.FPSections");
        }
    }
}
