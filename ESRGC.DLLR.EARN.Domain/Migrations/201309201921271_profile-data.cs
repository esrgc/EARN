namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profiledata : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contact", "PictureID", "dbo.Picture");
            DropIndex("dbo.Contact", new[] { "PictureID" });
            CreateTable(
                "dbo.Industry",
                c => new
                    {
                        IndustryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.IndustryID);
            
            CreateTable(
                "dbo.Organization",
                c => new
                    {
                        OrganizationID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Website = c.String(maxLength: 50),
                        Description = c.String(maxLength: 300),
                        IndustryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrganizationID)
                .ForeignKey("dbo.Industry", t => t.IndustryID, cascadeDelete: true)
                .Index(t => t.IndustryID);
            
            CreateTable(
                "dbo.Profile",
                c => new
                    {
                        ProfileId = c.Int(nullable: false, identity: true),
                        lat = c.Double(),
                        lon = c.Double(),
                        location = c.Geometry(),
                        PictureID = c.Int(),
                        ContactID = c.Int(nullable: false),
                        OrganizationID = c.Int(nullable: false),
                        UserGroupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProfileId)
                .ForeignKey("dbo.Contact", t => t.ContactID, cascadeDelete: true)
                .ForeignKey("dbo.Organization", t => t.OrganizationID, cascadeDelete: true)
                .ForeignKey("dbo.Picture", t => t.PictureID)
                .ForeignKey("dbo.UserGroup", t => t.UserGroupID, cascadeDelete: true)
                .Index(t => t.ContactID)
                .Index(t => t.OrganizationID)
                .Index(t => t.PictureID)
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
            
            AlterColumn("dbo.Contact", "Fax", c => c.String());
            AlterColumn("dbo.Contact", "Floor", c => c.String());
            DropColumn("dbo.Account", "SecretQuestion2");
            DropColumn("dbo.Account", "AnswerToSecretQuestion2");
            DropColumn("dbo.Account", "SecretQuestion3");
            DropColumn("dbo.Account", "AnswerToSecretQuestion3");
            DropColumn("dbo.Account", "AcceptNotification");
            DropColumn("dbo.Contact", "lat");
            DropColumn("dbo.Contact", "lon");
            DropColumn("dbo.Contact", "PictureID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contact", "PictureID", c => c.Int());
            AddColumn("dbo.Contact", "lon", c => c.Double());
            AddColumn("dbo.Contact", "lat", c => c.Double());
            AddColumn("dbo.Account", "AcceptNotification", c => c.Boolean());
            AddColumn("dbo.Account", "AnswerToSecretQuestion3", c => c.Binary(maxLength: 50));
            AddColumn("dbo.Account", "SecretQuestion3", c => c.String(maxLength: 50));
            AddColumn("dbo.Account", "AnswerToSecretQuestion2", c => c.Binary(maxLength: 50));
            AddColumn("dbo.Account", "SecretQuestion2", c => c.String(maxLength: 50));
            DropForeignKey("dbo.Profile", "UserGroupID", "dbo.UserGroup");
            DropForeignKey("dbo.Profile", "PictureID", "dbo.Picture");
            DropForeignKey("dbo.Profile", "OrganizationID", "dbo.Organization");
            DropForeignKey("dbo.Profile", "ContactID", "dbo.Contact");
            DropForeignKey("dbo.Organization", "IndustryID", "dbo.Industry");
            DropIndex("dbo.Profile", new[] { "UserGroupID" });
            DropIndex("dbo.Profile", new[] { "PictureID" });
            DropIndex("dbo.Profile", new[] { "OrganizationID" });
            DropIndex("dbo.Profile", new[] { "ContactID" });
            DropIndex("dbo.Organization", new[] { "IndustryID" });
            AlterColumn("dbo.Contact", "Floor", c => c.String());
            AlterColumn("dbo.Contact", "Fax", c => c.String());
            DropTable("dbo.UserGroup");
            DropTable("dbo.Profile");
            DropTable("dbo.Organization");
            DropTable("dbo.Industry");
            CreateIndex("dbo.Contact", "PictureID");
            AddForeignKey("dbo.Contact", "PictureID", "dbo.Picture", "PictureID");
        }
    }
}
