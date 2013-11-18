namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notificationmessage3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Message3", c => c.String());
            AddColumn("dbo.Notification", "FootNote", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "FootNote");
            DropColumn("dbo.Notification", "Message3");
        }
    }
}
