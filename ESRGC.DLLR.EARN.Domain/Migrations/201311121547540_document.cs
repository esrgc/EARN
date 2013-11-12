namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class document : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Document",
                c => new
                    {
                        DocumentID = c.Int(nullable: false, identity: true),
                        MimeType = c.String(),
                        Data = c.Binary(),
                        Created = c.DateTime(nullable: false),
                        PartnershipID = c.Int(nullable: false),
                        ProfileID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentID)
                .ForeignKey("dbo.Partnership", t => t.PartnershipID, cascadeDelete: true)
                .ForeignKey("dbo.Profile", t => t.ProfileID, cascadeDelete: true)
                .Index(t => t.PartnershipID)
                .Index(t => t.ProfileID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Document", "ProfileID", "dbo.Profile");
            DropForeignKey("dbo.Document", "PartnershipID", "dbo.Partnership");
            DropIndex("dbo.Document", new[] { "ProfileID" });
            DropIndex("dbo.Document", new[] { "PartnershipID" });
            DropTable("dbo.Document");
        }
    }
}
