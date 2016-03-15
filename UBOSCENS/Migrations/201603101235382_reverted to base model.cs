namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revertedtobasemodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sections", "RevisionID_id", "dbo.Revisions");
            DropIndex("dbo.Sections", new[] { "RevisionID_id" });
            AddColumn("dbo.Sections", "RevisionID", c => c.Guid(nullable: false));
            DropColumn("dbo.Sections", "RevisionID_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sections", "RevisionID_id", c => c.Guid());
            DropColumn("dbo.Sections", "RevisionID");
            CreateIndex("dbo.Sections", "RevisionID_id");
            AddForeignKey("dbo.Sections", "RevisionID_id", "dbo.Revisions", "id");
        }
    }
}
