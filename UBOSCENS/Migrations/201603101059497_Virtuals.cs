namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Virtuals : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Revisions",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.Sections", "RevisionID_id", c => c.Guid());
            CreateIndex("dbo.Sections", "RevisionID_id");
            AddForeignKey("dbo.Sections", "RevisionID_id", "dbo.Revisions", "id");
            DropColumn("dbo.Sections", "RevisionID");
            DropColumn("dbo.Sections", "SubSection");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sections", "SubSection", c => c.Guid(nullable: false));
            AddColumn("dbo.Sections", "RevisionID", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Sections", "RevisionID_id", "dbo.Revisions");
            DropIndex("dbo.Sections", new[] { "RevisionID_id" });
            DropColumn("dbo.Sections", "RevisionID_id");
            DropTable("dbo.Revisions");
        }
    }
}
