namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sections : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sections", "RevisionID", c => c.Guid(nullable: false));
            AddColumn("dbo.Sections", "SubSection", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sections", "SubSection");
            DropColumn("dbo.Sections", "RevisionID");
        }
    }
}
