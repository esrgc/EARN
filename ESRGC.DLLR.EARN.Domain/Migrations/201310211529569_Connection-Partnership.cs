namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectionPartnership : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Connection",
                c => new
                    {
                        ConnectionID = c.Int(nullable: false, identity: true),
                        ProfileID = c.Int(nullable: false),
                        ProfileID2 = c.Int(nullable: false),
                        Profile_ProfileID = c.Int(),
                        Profile2_ProfileID = c.Int(),
                        Profile_ProfileID1 = c.Int(),
                    })
                .PrimaryKey(t => t.ConnectionID)
                .ForeignKey("dbo.Profile", t => t.Profile_ProfileID)
                .ForeignKey("dbo.Profile", t => t.Profile2_ProfileID)
                .ForeignKey("dbo.Profile", t => t.Profile_ProfileID1)
                .Index(t => t.Profile_ProfileID)
                .Index(t => t.Profile2_ProfileID)
                .Index(t => t.Profile_ProfileID1);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartnershipDetail", "ProfileID", "dbo.Profile");
            DropForeignKey("dbo.PartnershipDetail", "PartnershipID", "dbo.Partnership");
            DropForeignKey("dbo.Connection", "Profile_ProfileID1", "dbo.Profile");
            DropForeignKey("dbo.Connection", "Profile2_ProfileID", "dbo.Profile");
            DropForeignKey("dbo.Connection", "Profile_ProfileID", "dbo.Profile");
            DropIndex("dbo.PartnershipDetail", new[] { "ProfileID" });
            DropIndex("dbo.PartnershipDetail", new[] { "PartnershipID" });
            DropIndex("dbo.Connection", new[] { "Profile_ProfileID1" });
            DropIndex("dbo.Connection", new[] { "Profile2_ProfileID" });
            DropIndex("dbo.Connection", new[] { "Profile_ProfileID" });
            DropTable("dbo.Partnership");
            DropTable("dbo.PartnershipDetail");
            DropTable("dbo.Connection");
        }
    }
}
