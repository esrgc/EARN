namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class message2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Message", "ReceiverID", "dbo.Profile");
            DropForeignKey("dbo.Message", "SenderID", "dbo.Profile");
            DropIndex("dbo.Message", new[] { "ReceiverID" });
            DropIndex("dbo.Message", new[] { "SenderID" });
            AlterColumn("dbo.Message", "SenderID", c => c.Int());
            AlterColumn("dbo.Message", "ReceiverID", c => c.Int());
            CreateIndex("dbo.Message", "ReceiverID");
            CreateIndex("dbo.Message", "SenderID");
            AddForeignKey("dbo.Message", "ReceiverID", "dbo.Profile", "ProfileID");
            AddForeignKey("dbo.Message", "SenderID", "dbo.Profile", "ProfileID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Message", "SenderID", "dbo.Profile");
            DropForeignKey("dbo.Message", "ReceiverID", "dbo.Profile");
            DropIndex("dbo.Message", new[] { "SenderID" });
            DropIndex("dbo.Message", new[] { "ReceiverID" });
            AlterColumn("dbo.Message", "ReceiverID", c => c.Int(nullable: false));
            AlterColumn("dbo.Message", "SenderID", c => c.Int(nullable: false));
            CreateIndex("dbo.Message", "SenderID");
            CreateIndex("dbo.Message", "ReceiverID");
            AddForeignKey("dbo.Message", "SenderID", "dbo.Profile", "ProfileID");
            AddForeignKey("dbo.Message", "ReceiverID", "dbo.Profile", "ProfileID");
        }
    }
}
