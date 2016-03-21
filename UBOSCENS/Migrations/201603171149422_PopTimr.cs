namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopTimr : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PopulationTimers",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        count = c.Int(nullable: false),
                        asOf = c.DateTime(nullable: false),
                        rate = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PopulationTimers");
        }
    }
}
