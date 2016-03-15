namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeaturedStory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeaturedStories", "title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeaturedStories", "title");
        }
    }
}
