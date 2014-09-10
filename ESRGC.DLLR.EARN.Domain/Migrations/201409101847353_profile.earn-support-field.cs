namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profileearnsupportfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profile", "EARNSupport", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profile", "EARNSupport");
        }
    }
}
