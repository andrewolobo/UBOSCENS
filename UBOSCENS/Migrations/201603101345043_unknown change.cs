namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class unknownchange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubSections",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        SectionID = c.Guid(nullable: false),
                        Title = c.String(),
                        ImportLogID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SubSections");
        }
    }
}
