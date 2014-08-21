namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messageboardjoindate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MessageBoard", "JoinedDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Account", "ProfileID");
            CreateIndex("dbo.Notification", "AccountID");
            CreateIndex("dbo.Request", "SenderID");
            CreateIndex("dbo.Request", "ReceiverID");
            CreateIndex("dbo.Request", "NotificationID");
            CreateIndex("dbo.Request", "PartnershipID");
            CreateIndex("dbo.Request", "ProfileID");
            CreateIndex("dbo.Comment", "AuthorID");
            CreateIndex("dbo.Comment", "PartnershipID");
            CreateIndex("dbo.Profile", "PictureID");
            CreateIndex("dbo.Profile", "ContactID");
            CreateIndex("dbo.Profile", "OrganizationID");
            CreateIndex("dbo.Profile", "UserGroupID");
            CreateIndex("dbo.Profile", "CategoryID");
            CreateIndex("dbo.Category", "UserGroupID");
            CreateIndex("dbo.MessageBoard", "ConversationID");
            CreateIndex("dbo.MessageBoard", "ProfileID");
            CreateIndex("dbo.MessageBoard", "Message_MessageID");
            CreateIndex("dbo.Message", "SenderID");
            CreateIndex("dbo.Message", "ReceiverID");
            CreateIndex("dbo.Message", "ConversationID");
            CreateIndex("dbo.PartnershipDetail", "PartnershipID");
            CreateIndex("dbo.PartnershipDetail", "ProfileID");
            CreateIndex("dbo.ProfileTag", "ProfileID");
            CreateIndex("dbo.ProfileTag", "TagID");
            CreateIndex("dbo.PartnershipTag", "PartnershipID");
            CreateIndex("dbo.PartnershipTag", "TagID");
            CreateIndex("dbo.Document", "PartnershipID");
            CreateIndex("dbo.Document", "ProfileID");
            CreateIndex("dbo.Connection", "ProfileID");
            CreateIndex("dbo.Connection", "ConnectionProfileID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Connection", new[] { "ConnectionProfileID" });
            DropIndex("dbo.Connection", new[] { "ProfileID" });
            DropIndex("dbo.Document", new[] { "ProfileID" });
            DropIndex("dbo.Document", new[] { "PartnershipID" });
            DropIndex("dbo.PartnershipTag", new[] { "TagID" });
            DropIndex("dbo.PartnershipTag", new[] { "PartnershipID" });
            DropIndex("dbo.ProfileTag", new[] { "TagID" });
            DropIndex("dbo.ProfileTag", new[] { "ProfileID" });
            DropIndex("dbo.PartnershipDetail", new[] { "ProfileID" });
            DropIndex("dbo.PartnershipDetail", new[] { "PartnershipID" });
            DropIndex("dbo.Message", new[] { "ConversationID" });
            DropIndex("dbo.Message", new[] { "ReceiverID" });
            DropIndex("dbo.Message", new[] { "SenderID" });
            DropIndex("dbo.MessageBoard", new[] { "Message_MessageID" });
            DropIndex("dbo.MessageBoard", new[] { "ProfileID" });
            DropIndex("dbo.MessageBoard", new[] { "ConversationID" });
            DropIndex("dbo.Category", new[] { "UserGroupID" });
            DropIndex("dbo.Profile", new[] { "CategoryID" });
            DropIndex("dbo.Profile", new[] { "UserGroupID" });
            DropIndex("dbo.Profile", new[] { "OrganizationID" });
            DropIndex("dbo.Profile", new[] { "ContactID" });
            DropIndex("dbo.Profile", new[] { "PictureID" });
            DropIndex("dbo.Comment", new[] { "PartnershipID" });
            DropIndex("dbo.Comment", new[] { "AuthorID" });
            DropIndex("dbo.Request", new[] { "ProfileID" });
            DropIndex("dbo.Request", new[] { "PartnershipID" });
            DropIndex("dbo.Request", new[] { "NotificationID" });
            DropIndex("dbo.Request", new[] { "ReceiverID" });
            DropIndex("dbo.Request", new[] { "SenderID" });
            DropIndex("dbo.Notification", new[] { "AccountID" });
            DropIndex("dbo.Account", new[] { "ProfileID" });
            DropColumn("dbo.MessageBoard", "JoinedDate");
        }
    }
}
