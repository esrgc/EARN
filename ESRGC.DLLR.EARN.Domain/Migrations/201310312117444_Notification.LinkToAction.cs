namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationLinkToAction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "LinkToAction", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "LinkToAction");
        }
    }
}
