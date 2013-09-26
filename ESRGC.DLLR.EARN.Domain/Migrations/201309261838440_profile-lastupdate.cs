namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profilelastupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profile", "LastUpdate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profile", "LastUpdate");
        }
    }
}
