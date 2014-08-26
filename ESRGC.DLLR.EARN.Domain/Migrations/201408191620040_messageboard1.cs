namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messageboard1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Profile", "Conversation_ConversationID", "dbo.Conversation");
            DropIndex("dbo.Profile", new[] { "Conversation_ConversationID" });
            DropColumn("dbo.Profile", "Conversation_ConversationID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Profile", "Conversation_ConversationID", c => c.Int());
            CreateIndex("dbo.Profile", "Conversation_ConversationID");
            AddForeignKey("dbo.Profile", "Conversation_ConversationID", "dbo.Conversation", "ConversationID");
        }
    }
}
