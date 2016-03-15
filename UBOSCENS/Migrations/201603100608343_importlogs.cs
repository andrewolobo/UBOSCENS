namespace UBOSCENS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class importlogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportLogs", "Data", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportLogs", "Data");
        }
    }
}
