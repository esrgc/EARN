namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notificationheader : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Header", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "Header");
        }
    }
}
