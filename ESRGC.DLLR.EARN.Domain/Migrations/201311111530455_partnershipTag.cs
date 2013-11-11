namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class partnershipTag : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PartnershipTag",
                c => new
                    {
                        PartnershipTagID = c.Int(nullable: false, identity: true),
                        PartnershipID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PartnershipTagID)
                .ForeignKey("dbo.Partnership", t => t.PartnershipID, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.TagID, cascadeDelete: true)
                .Index(t => t.PartnershipID)
                .Index(t => t.TagID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartnershipTag", "TagID", "dbo.Tag");
            DropForeignKey("dbo.PartnershipTag", "PartnershipID", "dbo.Partnership");
            DropIndex("dbo.PartnershipTag", new[] { "TagID" });
            DropIndex("dbo.PartnershipTag", new[] { "PartnershipID" });
            DropTable("dbo.PartnershipTag");
        }
    }
}
