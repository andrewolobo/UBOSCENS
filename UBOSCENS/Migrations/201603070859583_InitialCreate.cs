namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FeaturedStories",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        Content = c.String(),
                        StoryId = c.Guid(nullable: false),
                        Image = c.String(),
                        Active = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ImportLogs",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        SectionID = c.Guid(nullable: false),
                        TableID = c.Guid(nullable: false),
                        SubTableID = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Stories",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        Content = c.String(),
                        Active = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.subTables",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        altID = c.Int(nullable: false),
                        title = c.String(),
                        TableID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        Title = c.String(),
                        SectionID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Timers",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        timer = c.Int(nullable: false),
                        rate = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Timers");
            DropTable("dbo.Tables");
            DropTable("dbo.subTables");
            DropTable("dbo.Stories");
            DropTable("dbo.Sections");
            DropTable("dbo.ImportLogs");
            DropTable("dbo.FeaturedStories");
        }
    }
}
