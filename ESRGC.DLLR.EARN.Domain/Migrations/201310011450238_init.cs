namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        AccountID = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(maxLength: 50),
                        Password = c.Binary(maxLength: 32),
                        InitialPassword = c.String(maxLength: 32),
                        Role = c.String(maxLength: 20),
                        Active = c.Boolean(nullable: false),
                        SecretQuestion = c.String(maxLength: 50),
                        AnswerToSecretQuestion = c.Binary(maxLength: 50),
                        MemberSince = c.DateTime(),
                        LastLogin = c.DateTime(),
                        LastUpdate = c.DateTime(),
                        ProfileID = c.Int(),
                    })
                .PrimaryKey(t => t.AccountID)
                .ForeignKey("dbo.Profile", t => t.ProfileID)
                .Index(t => t.ProfileID);
            
            CreateTable(
                "dbo.Profile",
                c => new
                    {
                        ProfileID = c.Int(nullable: false, identity: true),
                        PictureID = c.Int(),
                        ContactID = c.Int(nullable: false),
                        OrganizationID = c.Int(nullable: false),
                        UserGroupID = c.Int(nullable: false),
                        CommunityID = c.Int(nullable: false),
                        LastUpdate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProfileID)
                .ForeignKey("dbo.Community", t => t.CommunityID, cascadeDelete: true)
                .ForeignKey("dbo.Contact", t => t.ContactID, cascadeDelete: true)
                .ForeignKey("dbo.Organization", t => t.OrganizationID, cascadeDelete: true)
                .ForeignKey("dbo.Picture", t => t.PictureID)
                .ForeignKey("dbo.UserGroup", t => t.UserGroupID, cascadeDelete: true)
                .Index(t => t.CommunityID)
                .Index(t => t.ContactID)
                .Index(t => t.OrganizationID)
                .Index(t => t.PictureID)
                .Index(t => t.UserGroupID);
            
            CreateTable(
                "dbo.Community",
                c => new
                    {
                        CommunityID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CommunityID);
            
            CreateTable(
                "dbo.Organization",
                c => new
                    {
                        OrganizationID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Website = c.String(maxLength: 50),
                        Description = c.String(maxLength: 1000),
                        FacebookLink = c.String(),
                        LinkedInLink = c.String(),
                        TwitterLink = c.String(),
                        Community_CommunityID = c.Int(),
                    })
                .PrimaryKey(t => t.OrganizationID)
                .ForeignKey("dbo.Community", t => t.Community_CommunityID)
                .Index(t => t.Community_CommunityID);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ContactID = c.Int(nullable: false, identity: true),
                        NameTitle = c.String(),
                        FirstName = c.String(nullable: false),
                        Middle = c.String(),
                        LastName = c.String(nullable: false),
                        NameExtension = c.String(),
                        JobTitle = c.String(),
                        Department = c.String(),
                        MailStop = c.String(),
                        StreetAddress = c.String(nullable: false),
                        StreetAddress2 = c.String(),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        Zip = c.String(nullable: false),
                        Phone = c.String(nullable: false, maxLength: 20),
                        PhoneExt = c.String(maxLength: 4),
                        Fax = c.String(),
                        Phone2 = c.String(),
                        PhoneExt2 = c.String(maxLength: 4),
                        Email = c.String(nullable: false, maxLength: 50),
                        EmailList = c.String(),
                        LastUpdate = c.DateTime(nullable: false),
                        Floor = c.String(),
                        Building = c.String(),
                    })
                .PrimaryKey(t => t.ContactID);
            
            CreateTable(
                "dbo.Picture",
                c => new
                    {
                        PictureID = c.Int(nullable: false, identity: true),
                        ImageData = c.Binary(),
                        ImageMimeType = c.String(),
                    })
                .PrimaryKey(t => t.PictureID);
            
            CreateTable(
                "dbo.ProfileTag",
                c => new
                    {
                        ProfileTagID = c.Int(nullable: false, identity: true),
                        ProfileID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProfileTagID)
                .ForeignKey("dbo.Profile", t => t.ProfileID, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.TagID, cascadeDelete: true)
                .Index(t => t.ProfileID)
                .Index(t => t.TagID);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        TagID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Description = c.String(),
                        Geometry = c.Geometry(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.TagID);
            
            CreateTable(
                "dbo.UserGroup",
                c => new
                    {
                        UserGroupID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.UserGroupID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Account", "ProfileID", "dbo.Profile");
            DropForeignKey("dbo.Profile", "UserGroupID", "dbo.UserGroup");
            DropForeignKey("dbo.ProfileTag", "TagID", "dbo.Tag");
            DropForeignKey("dbo.ProfileTag", "ProfileID", "dbo.Profile");
            DropForeignKey("dbo.Profile", "PictureID", "dbo.Picture");
            DropForeignKey("dbo.Profile", "OrganizationID", "dbo.Organization");
            DropForeignKey("dbo.Profile", "ContactID", "dbo.Contact");
            DropForeignKey("dbo.Profile", "CommunityID", "dbo.Community");
            DropForeignKey("dbo.Organization", "Community_CommunityID", "dbo.Community");
            DropIndex("dbo.Account", new[] { "ProfileID" });
            DropIndex("dbo.Profile", new[] { "UserGroupID" });
            DropIndex("dbo.ProfileTag", new[] { "TagID" });
            DropIndex("dbo.ProfileTag", new[] { "ProfileID" });
            DropIndex("dbo.Profile", new[] { "PictureID" });
            DropIndex("dbo.Profile", new[] { "OrganizationID" });
            DropIndex("dbo.Profile", new[] { "ContactID" });
            DropIndex("dbo.Profile", new[] { "CommunityID" });
            DropIndex("dbo.Organization", new[] { "Community_CommunityID" });
            DropTable("dbo.UserGroup");
            DropTable("dbo.Tag");
            DropTable("dbo.ProfileTag");
            DropTable("dbo.Picture");
            DropTable("dbo.Contact");
            DropTable("dbo.Organization");
            DropTable("dbo.Community");
            DropTable("dbo.Profile");
            DropTable("dbo.Account");
        }
    }
}
