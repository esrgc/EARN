namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profileRequest1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Request", "ProfileID", "dbo.Profile");
            DropForeignKey("dbo.Request", "Account_AccountID", "dbo.Account");
            DropForeignKey("dbo.Request", "Account_AccountID1", "dbo.Account");
            DropForeignKey("dbo.Request", "NotificationID", "dbo.Notification");
            DropIndex("dbo.Request", new[] { "NotificationID" });
            DropIndex("dbo.Request", new[] { "ProfileID" });
            DropIndex("dbo.Request", new[] { "Account_AccountID" });
            DropIndex("dbo.Request", new[] { "Account_AccountID1" });
            RenameColumn(table: "dbo.Request", name: "NotificationID", newName: "Notification_NotificationID");
            CreateTable(
                "dbo.ProfileRequest",
                c => new
                    {
                        ProfileRequestID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Message = c.String(),
                        Status = c.String(),
                        SenderID = c.Int(nullable: false),
                        ReceiverID = c.Int(nullable: false),
                        ProfileID = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProfileRequestID)
                .ForeignKey("dbo.Profile", t => t.ProfileID, cascadeDelete: true)
                .ForeignKey("dbo.Account", t => t.ReceiverID)
                .ForeignKey("dbo.Account", t => t.SenderID)
                .Index(t => t.SenderID)
                .Index(t => t.ReceiverID)
                .Index(t => t.ProfileID);
            
            AlterColumn("dbo.Request", "Notification_NotificationID", c => c.Int());
            CreateIndex("dbo.Request", "Notification_NotificationID");
            AddForeignKey("dbo.Request", "Notification_NotificationID", "dbo.Notification", "NotificationID");
            DropColumn("dbo.Request", "ProfileID");
            DropColumn("dbo.Request", "Account_AccountID");
            DropColumn("dbo.Request", "Account_AccountID1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "Account_AccountID1", c => c.Int());
            AddColumn("dbo.Request", "Account_AccountID", c => c.Int());
            AddColumn("dbo.Request", "ProfileID", c => c.Int());
            DropForeignKey("dbo.Request", "Notification_NotificationID", "dbo.Notification");
            DropForeignKey("dbo.ProfileRequest", "SenderID", "dbo.Account");
            DropForeignKey("dbo.ProfileRequest", "ReceiverID", "dbo.Account");
            DropForeignKey("dbo.ProfileRequest", "ProfileID", "dbo.Profile");
            DropIndex("dbo.ProfileRequest", new[] { "ProfileID" });
            DropIndex("dbo.ProfileRequest", new[] { "ReceiverID" });
            DropIndex("dbo.ProfileRequest", new[] { "SenderID" });
            DropIndex("dbo.Request", new[] { "Notification_NotificationID" });
            AlterColumn("dbo.Request", "Notification_NotificationID", c => c.Int(nullable: false));
            DropTable("dbo.ProfileRequest");
            RenameColumn(table: "dbo.Request", name: "Notification_NotificationID", newName: "NotificationID");
            CreateIndex("dbo.Request", "Account_AccountID1");
            CreateIndex("dbo.Request", "Account_AccountID");
            CreateIndex("dbo.Request", "ProfileID");
            CreateIndex("dbo.Request", "NotificationID");
            AddForeignKey("dbo.Request", "NotificationID", "dbo.Notification", "NotificationID", cascadeDelete: true);
            AddForeignKey("dbo.Request", "Account_AccountID1", "dbo.Account", "AccountID");
            AddForeignKey("dbo.Request", "Account_AccountID", "dbo.Account", "AccountID");
            AddForeignKey("dbo.Request", "ProfileID", "dbo.Profile", "ProfileID", cascadeDelete: true);
        }
    }
}
