namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class message : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        SenderID = c.Int(nullable: false),
                        ReceiverID = c.Int(nullable: false),
                        Title = c.String(),
                        Header = c.String(),
                        Message1 = c.String(),
                        Message2 = c.String(),
                        Message3 = c.String(),
                        FootNote = c.String(),
                        IsRead = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false),
                        LinkToAction = c.String(),
                        EmailSent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MessageID)
                .ForeignKey("dbo.Profile", t => t.ReceiverID)
                .ForeignKey("dbo.Profile", t => t.SenderID)
                .Index(t => t.ReceiverID)
                .Index(t => t.SenderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Message", "SenderID", "dbo.Profile");
            DropForeignKey("dbo.Message", "ReceiverID", "dbo.Profile");
            DropIndex("dbo.Message", new[] { "SenderID" });
            DropIndex("dbo.Message", new[] { "ReceiverID" });
            DropTable("dbo.Message");
        }
    }
}
