namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestnotification2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Category", c => c.String());
            AddColumn("dbo.Request", "NotificationID", c => c.Int(nullable: false));
            AddColumn("dbo.Request", "PartnershipID", c => c.Int());
            CreateIndex("dbo.Request", "NotificationID");
            CreateIndex("dbo.Request", "PartnershipID");
            AddForeignKey("dbo.Request", "NotificationID", "dbo.Notification", "NotificationID", cascadeDelete: true);
            AddForeignKey("dbo.Request", "PartnershipID", "dbo.Partnership", "PartnershipID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Request", "PartnershipID", "dbo.Partnership");
            DropForeignKey("dbo.Request", "NotificationID", "dbo.Notification");
            DropIndex("dbo.Request", new[] { "PartnershipID" });
            DropIndex("dbo.Request", new[] { "NotificationID" });
            DropColumn("dbo.Request", "PartnershipID");
            DropColumn("dbo.Request", "NotificationID");
            DropColumn("dbo.Notification", "Category");
        }
    }
}
