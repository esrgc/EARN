namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class conversation : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.MessageBoard", name: "MessageID", newName: "Message_MessageID");
            CreateTable(
                "dbo.Conversation",
                c => new
                    {
                        ConversationID = c.Int(nullable: false, identity: true),
                        Started = c.DateTime(nullable: false),
                        LastMessageDate = c.DateTime(nullable: false),
                        LastMessage = c.String(),
                    })
                .PrimaryKey(t => t.ConversationID);
            
            AddColumn("dbo.Profile", "Conversation_ConversationID", c => c.Int());
            AddColumn("dbo.MessageBoard", "ConversationID", c => c.Int());
            AddColumn("dbo.Message", "ConversationID", c => c.Int());
            CreateIndex("dbo.Message", "ConversationID");
            CreateIndex("dbo.Profile", "Conversation_ConversationID");
            CreateIndex("dbo.MessageBoard", "ConversationID");
            AddForeignKey("dbo.Message", "ConversationID", "dbo.Conversation", "ConversationID");
            AddForeignKey("dbo.Profile", "Conversation_ConversationID", "dbo.Conversation", "ConversationID");
            AddForeignKey("dbo.MessageBoard", "ConversationID", "dbo.Conversation", "ConversationID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageBoard", "ConversationID", "dbo.Conversation");
            DropForeignKey("dbo.Profile", "Conversation_ConversationID", "dbo.Conversation");
            DropForeignKey("dbo.Message", "ConversationID", "dbo.Conversation");
            DropIndex("dbo.MessageBoard", new[] { "ConversationID" });
            DropIndex("dbo.Profile", new[] { "Conversation_ConversationID" });
            DropIndex("dbo.Message", new[] { "ConversationID" });
            DropColumn("dbo.Message", "ConversationID");
            DropColumn("dbo.MessageBoard", "ConversationID");
            DropColumn("dbo.Profile", "Conversation_ConversationID");
            DropTable("dbo.Conversation");
            RenameColumn(table: "dbo.MessageBoard", name: "Message_MessageID", newName: "MessageID");
        }
    }
}
