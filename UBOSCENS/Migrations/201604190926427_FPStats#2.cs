namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FPStats2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FPPageStats",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        name = c.String(),
                        data = c.String(),
                        type = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FPPageStats");
        }
    }
}
