namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mapcollection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MapCollections",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        name = c.String(),
                        description = c.String(),
                        data = c.String(),
                        active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MapCollections");
        }
    }
}
