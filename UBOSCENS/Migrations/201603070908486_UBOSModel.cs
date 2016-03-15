namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UBOSModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tables", "Model", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tables", "Model");
        }
    }
}
