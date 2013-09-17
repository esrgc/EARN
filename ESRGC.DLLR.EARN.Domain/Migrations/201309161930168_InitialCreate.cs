namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                        ContactID = c.Int(),
                        Role = c.String(maxLength: 20),
                        Active = c.Boolean(nullable: false),
                        SecretQuestion = c.String(maxLength: 50),
                        AnswerToSecretQuestion = c.Binary(maxLength: 50),
                        SecretQuestion2 = c.String(maxLength: 50),
                        AnswerToSecretQuestion2 = c.Binary(maxLength: 50),
                        SecretQuestion3 = c.String(maxLength: 50),
                        AnswerToSecretQuestion3 = c.Binary(maxLength: 50),
                        MemberSince = c.DateTime(),
                        LastLogin = c.DateTime(),
                        AcceptNotification = c.Boolean(),
                        LastUpdate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AccountID)
                .ForeignKey("dbo.Contact", t => t.ContactID)
                .Index(t => t.ContactID);
            
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
                        StreetAddress = c.String(),
                        StreetAddress2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Phone = c.String(nullable: false, maxLength: 20),
                        PhoneExt = c.String(maxLength: 4),
                        FAX = c.String(),
                        Phone2 = c.String(),
                        PhoneExt2 = c.String(maxLength: 4),
                        Email = c.String(nullable: false, maxLength: 50),
                        EmailList = c.String(),
                        LastUpdate = c.DateTime(nullable: false),
                        FLOOR = c.String(),
                        Building = c.String(),
                        lat = c.Double(),
                        lon = c.Double(),
                        PictureID = c.Int(),
                    })
                .PrimaryKey(t => t.ContactID)
                .ForeignKey("dbo.Picture", t => t.PictureID)
                .Index(t => t.PictureID);
            
            CreateTable(
                "dbo.Picture",
                c => new
                    {
                        PictureID = c.Int(nullable: false, identity: true),
                        ImageData = c.Binary(),
                        ImageMimeType = c.String(),
                    })
                .PrimaryKey(t => t.PictureID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Account", "ContactID", "dbo.Contact");
            DropForeignKey("dbo.Contact", "PictureID", "dbo.Picture");
            DropIndex("dbo.Account", new[] { "ContactID" });
            DropIndex("dbo.Contact", new[] { "PictureID" });
            DropTable("dbo.Picture");
            DropTable("dbo.Contact");
            DropTable("dbo.Account");
        }
    }
}
