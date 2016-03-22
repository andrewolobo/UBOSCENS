namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedFacts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Facts",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        data = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Facts");
        }
    }
}
