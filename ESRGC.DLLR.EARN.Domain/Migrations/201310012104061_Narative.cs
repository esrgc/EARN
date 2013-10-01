namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Narative : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profile", "Narative", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profile", "Narative");
        }
    }
}
