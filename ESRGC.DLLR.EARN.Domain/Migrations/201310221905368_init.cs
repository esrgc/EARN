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
                        CategoryID = c.Int(),
                        LastUpdate = c.DateTime(),
                        About = c.String(),
                    })
                .PrimaryKey(t => t.ProfileID)
                .ForeignKey("dbo.Category", t => t.CategoryID)
                .ForeignKey("dbo.Contact", t => t.ContactID, cascadeDelete: true)
                .ForeignKey("dbo.Organization", t => t.OrganizationID, cascadeDelete: true)
                .ForeignKey("dbo.Picture", t => t.PictureID)
                .ForeignKey("dbo.UserGroup", t => t.UserGroupID, cascadeDelete: true)
                .Index(t => t.CategoryID)
                .Index(t => t.ContactID)
                .Index(t => t.OrganizationID)
                .Index(t => t.PictureID)
                .Index(t => t.UserGroupID);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserGroupID = c.Int(),
                    })
                .PrimaryKey(t => t.CategoryID)
                .ForeignKey("dbo.UserGroup", t => t.UserGroupID)
                .Index(t => t.UserGroupID);
            
            CreateTable(
                "dbo.UserGroup",
                c => new
                    {
                        UserGroupID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.UserGroupID);
            
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
                        MailStop = c.String(),
                        Phone = c.String(nullable: false, maxLength: 20),
                        PhoneExt = c.String(maxLength: 4),
                        Fax = c.String(),
                        Phone2 = c.String(),
                        PhoneExt2 = c.String(maxLength: 4),
                        Email = c.String(nullable: false, maxLength: 50),
                        EmailList = c.String(),
                        LastUpdate = c.DateTime(nullable: false),
                        Floor = c.String(),
                    })
                .PrimaryKey(t => t.ContactID);
            
            CreateTable(
                "dbo.Organization",
                c => new
                    {
                        OrganizationID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Website = c.String(maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 1000),
                        FacebookLink = c.String(),
                        LinkedInLink = c.String(),
                        TwitterLink = c.String(),
                        StreetAddress = c.String(nullable: false),
                        StreetAddress2 = c.String(),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false, maxLength: 2),
                        Zip = c.String(nullable: false, maxLength: 5),
                        Department = c.String(),
                        Building = c.String(),
                    })
                .PrimaryKey(t => t.OrganizationID);
            
            CreateTable(
                "dbo.PartnershipDetail",
                c => new
                    {
                        PartnershipDetailID = c.Int(nullable: false, identity: true),
                        PartnershipID = c.Int(nullable: false),
                        ProfileID = c.Int(nullable: false),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.PartnershipDetailID)
                .ForeignKey("dbo.Partnership", t => t.PartnershipID, cascadeDelete: true)
                .ForeignKey("dbo.Profile", t => t.ProfileID, cascadeDelete: true)
                .Index(t => t.PartnershipID)
                .Index(t => t.ProfileID);
            
            CreateTable(
                "dbo.Partnership",
                c => new
                    {
                        PartnershipID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Status = c.String(),
                        Description = c.String(),
                        GrantStatus = c.String(),
                    })
                .PrimaryKey(t => t.PartnershipID);
            
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
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Geometry = c.Geometry(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.TagID);
            
            CreateTable(
                "dbo.Connection",
                c => new
                    {
                        ProfileID = c.Int(nullable: false),
                        ConnectionProfileID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProfileID, t.ConnectionProfileID })
                .ForeignKey("dbo.Profile", t => t.ProfileID)
                .ForeignKey("dbo.Profile", t => t.ConnectionProfileID)
                .Index(t => t.ProfileID)
                .Index(t => t.ConnectionProfileID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Account", "ProfileID", "dbo.Profile");
            DropForeignKey("dbo.Profile", "UserGroupID", "dbo.UserGroup");
            DropForeignKey("dbo.ProfileTag", "TagID", "dbo.Tag");
            DropForeignKey("dbo.ProfileTag", "ProfileID", "dbo.Profile");
            DropForeignKey("dbo.Profile", "PictureID", "dbo.Picture");
            DropForeignKey("dbo.PartnershipDetail", "ProfileID", "dbo.Profile");
            DropForeignKey("dbo.PartnershipDetail", "PartnershipID", "dbo.Partnership");
            DropForeignKey("dbo.Profile", "OrganizationID", "dbo.Organization");
            DropForeignKey("dbo.Profile", "ContactID", "dbo.Contact");
            DropForeignKey("dbo.Connection", "ConnectionProfileID", "dbo.Profile");
            DropForeignKey("dbo.Connection", "ProfileID", "dbo.Profile");
            DropForeignKey("dbo.Category", "UserGroupID", "dbo.UserGroup");
            DropForeignKey("dbo.Profile", "CategoryID", "dbo.Category");
            DropIndex("dbo.Account", new[] { "ProfileID" });
            DropIndex("dbo.Profile", new[] { "UserGroupID" });
            DropIndex("dbo.ProfileTag", new[] { "TagID" });
            DropIndex("dbo.ProfileTag", new[] { "ProfileID" });
            DropIndex("dbo.Profile", new[] { "PictureID" });
            DropIndex("dbo.PartnershipDetail", new[] { "ProfileID" });
            DropIndex("dbo.PartnershipDetail", new[] { "PartnershipID" });
            DropIndex("dbo.Profile", new[] { "OrganizationID" });
            DropIndex("dbo.Profile", new[] { "ContactID" });
            DropIndex("dbo.Connection", new[] { "ConnectionProfileID" });
            DropIndex("dbo.Connection", new[] { "ProfileID" });
            DropIndex("dbo.Category", new[] { "UserGroupID" });
            DropIndex("dbo.Profile", new[] { "CategoryID" });
            DropTable("dbo.Connection");
            DropTable("dbo.Tag");
            DropTable("dbo.ProfileTag");
            DropTable("dbo.Picture");
            DropTable("dbo.Partnership");
            DropTable("dbo.PartnershipDetail");
            DropTable("dbo.Organization");
            DropTable("dbo.Contact");
            DropTable("dbo.UserGroup");
            DropTable("dbo.Category");
            DropTable("dbo.Profile");
            DropTable("dbo.Account");
        }
    }
}
