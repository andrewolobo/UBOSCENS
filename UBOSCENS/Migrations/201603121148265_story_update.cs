namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class story_update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stories", "Title", c => c.String());
            AddColumn("dbo.Stories", "Image", c => c.String());
            AddColumn("dbo.Stories", "published", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stories", "published");
            DropColumn("dbo.Stories", "Image");
            DropColumn("dbo.Stories", "Title");
        }
    }
}
