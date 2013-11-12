namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class document2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Document", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Document", "Name");
        }
    }
}
