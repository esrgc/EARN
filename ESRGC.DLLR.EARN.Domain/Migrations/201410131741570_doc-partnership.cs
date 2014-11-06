namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class docpartnership : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Document", "PartnershipID", "dbo.Partnership");
            DropIndex("dbo.Document", new[] { "PartnershipID" });
            AlterColumn("dbo.Document", "PartnershipID", c => c.Int());
            CreateIndex("dbo.Document", "PartnershipID");
            AddForeignKey("dbo.Document", "PartnershipID", "dbo.Partnership", "PartnershipID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Document", "PartnershipID", "dbo.Partnership");
            DropIndex("dbo.Document", new[] { "PartnershipID" });
            AlterColumn("dbo.Document", "PartnershipID", c => c.Int(nullable: false));
            CreateIndex("dbo.Document", "PartnershipID");
            AddForeignKey("dbo.Document", "PartnershipID", "dbo.Partnership", "PartnershipID", cascadeDelete: true);
        }
    }
}
