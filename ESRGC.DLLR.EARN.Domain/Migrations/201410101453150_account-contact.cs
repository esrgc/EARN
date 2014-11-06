namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class accountcontact : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MessageBoard", "Message_MessageID", "dbo.Message");
            DropIndex("dbo.MessageBoard", new[] { "Message_MessageID" });
            AddColumn("dbo.Account", "ContactID", c => c.Int());
            CreateIndex("dbo.Account", "ContactID");
            AddForeignKey("dbo.Account", "ContactID", "dbo.Contact", "ContactID");
            //DropColumn("dbo.MessageBoard", "Message_MessageID");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.MessageBoard", "Message_MessageID", c => c.Int());
            DropForeignKey("dbo.Account", "ContactID", "dbo.Contact");
            DropIndex("dbo.Account", new[] { "ContactID" });
            DropColumn("dbo.Account", "ContactID");
            //CreateIndex("dbo.MessageBoard", "Message_MessageID");
            //AddForeignKey("dbo.MessageBoard", "Message_MessageID", "dbo.Message", "MessageID");
        }
    }
}
//