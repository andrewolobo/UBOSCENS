namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fpsidestats1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FPSideStats",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        title = c.String(),
                        ratio = c.String(),
                        percentage = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FPSideStats");
        }
    }
}
