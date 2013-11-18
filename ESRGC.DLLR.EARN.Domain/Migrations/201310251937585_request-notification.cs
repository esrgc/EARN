namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestnotification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        NotificationID = c.Int(nullable: false, identity: true),
                        AccountID = c.Int(nullable: false),
                        Message = c.String(),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.NotificationID)
                .ForeignKey("dbo.Account", t => t.AccountID, cascadeDelete: true)
                .Index(t => t.AccountID);
            
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        RequestID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Message = c.String(),
                        Status = c.String(),
                        SenderID = c.Int(nullable: false),
                        ReceiverID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequestID)
                .ForeignKey("dbo.Account", t => t.ReceiverID)
                .ForeignKey("dbo.Account", t => t.SenderID)
                .Index(t => t.ReceiverID)
                .Index(t => t.SenderID);
            
            AddColumn("dbo.Partnership", "LastUpdate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Request", "SenderID", "dbo.Account");
            DropForeignKey("dbo.Request", "ReceiverID", "dbo.Account");
            DropForeignKey("dbo.Notification", "AccountID", "dbo.Account");
            DropIndex("dbo.Request", new[] { "SenderID" });
            DropIndex("dbo.Request", new[] { "ReceiverID" });
            DropIndex("dbo.Notification", new[] { "AccountID" });
            DropColumn("dbo.Partnership", "LastUpdate");
            DropTable("dbo.Request");
            DropTable("dbo.Notification");
        }
    }
}
