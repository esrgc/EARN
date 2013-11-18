namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationMessage2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Message2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "Message2");
        }
    }
}
