namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ef6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Account", "ProfileID", c => c.Int());
            AlterColumn("dbo.Notification", "AccountID", c => c.Int(nullable: false));
            AlterColumn("dbo.Request", "SenderID", c => c.Int(nullable: false));
            AlterColumn("dbo.Request", "ReceiverID", c => c.Int(nullable: false));
            AlterColumn("dbo.Request", "PartnershipID", c => c.Int());
            AlterColumn("dbo.Request", "Notification_NotificationID", c => c.Int());
            AlterColumn("dbo.Profile", "PictureID", c => c.Int());
            AlterColumn("dbo.Profile", "ContactID", c => c.Int(nullable: false));
            AlterColumn("dbo.Profile", "OrganizationID", c => c.Int(nullable: false));
            AlterColumn("dbo.Profile", "UserGroupID", c => c.Int(nullable: false));
            AlterColumn("dbo.Profile", "CategoryID", c => c.Int());
            AlterColumn("dbo.Category", "UserGroupID", c => c.Int());
            AlterColumn("dbo.Comment", "AuthorID", c => c.Int(nullable: false));
            AlterColumn("dbo.Comment", "PartnershipID", c => c.Int(nullable: false));
            AlterColumn("dbo.Partnership", "PictureID", c => c.Int());
            AlterColumn("dbo.Document", "PartnershipID", c => c.Int(nullable: false));
            AlterColumn("dbo.Document", "ProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnershipDetail", "PartnershipID", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnershipDetail", "ProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnershipTag", "PartnershipID", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnershipTag", "TagID", c => c.Int(nullable: false));
            AlterColumn("dbo.ProfileTag", "ProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.ProfileTag", "TagID", c => c.Int(nullable: false));
            AlterColumn("dbo.MessageBoard", "ConversationID", c => c.Int());
            AlterColumn("dbo.MessageBoard", "ProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.MessageBoard", "Message_MessageID", c => c.Int());
            AlterColumn("dbo.Message", "SenderID", c => c.Int());
            AlterColumn("dbo.Message", "ReceiverID", c => c.Int());
            AlterColumn("dbo.Message", "ConversationID", c => c.Int());
            AlterColumn("dbo.ProfileRequest", "SenderID", c => c.Int(nullable: false));
            AlterColumn("dbo.ProfileRequest", "ReceiverID", c => c.Int(nullable: false));
            AlterColumn("dbo.ProfileRequest", "ProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.Connection", "ProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.Connection", "ConnectionProfileID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Connection", "ConnectionProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.Connection", "ProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.ProfileRequest", "ProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.ProfileRequest", "ReceiverID", c => c.Int(nullable: false));
            AlterColumn("dbo.ProfileRequest", "SenderID", c => c.Int(nullable: false));
            AlterColumn("dbo.Message", "ConversationID", c => c.Int());
            AlterColumn("dbo.Message", "ReceiverID", c => c.Int());
            AlterColumn("dbo.Message", "SenderID", c => c.Int());
            AlterColumn("dbo.MessageBoard", "Message_MessageID", c => c.Int());
            AlterColumn("dbo.MessageBoard", "ProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.MessageBoard", "ConversationID", c => c.Int());
            AlterColumn("dbo.ProfileTag", "TagID", c => c.Int(nullable: false));
            AlterColumn("dbo.ProfileTag", "ProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnershipTag", "TagID", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnershipTag", "PartnershipID", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnershipDetail", "ProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnershipDetail", "PartnershipID", c => c.Int(nullable: false));
            AlterColumn("dbo.Document", "ProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.Document", "PartnershipID", c => c.Int(nullable: false));
            AlterColumn("dbo.Partnership", "PictureID", c => c.Int());
            AlterColumn("dbo.Comment", "PartnershipID", c => c.Int(nullable: false));
            AlterColumn("dbo.Comment", "AuthorID", c => c.Int(nullable: false));
            AlterColumn("dbo.Category", "UserGroupID", c => c.Int());
            AlterColumn("dbo.Profile", "CategoryID", c => c.Int());
            AlterColumn("dbo.Profile", "UserGroupID", c => c.Int(nullable: false));
            AlterColumn("dbo.Profile", "OrganizationID", c => c.Int(nullable: false));
            AlterColumn("dbo.Profile", "ContactID", c => c.Int(nullable: false));
            AlterColumn("dbo.Profile", "PictureID", c => c.Int());
            AlterColumn("dbo.Request", "Notification_NotificationID", c => c.Int());
            AlterColumn("dbo.Request", "PartnershipID", c => c.Int());
            AlterColumn("dbo.Request", "ReceiverID", c => c.Int(nullable: false));
            AlterColumn("dbo.Request", "SenderID", c => c.Int(nullable: false));
            AlterColumn("dbo.Notification", "AccountID", c => c.Int(nullable: false));
            AlterColumn("dbo.Account", "ProfileID", c => c.Int());
        }
    }
}
