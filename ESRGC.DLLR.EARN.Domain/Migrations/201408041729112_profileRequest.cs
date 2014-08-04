namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profileRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "ProfileID", c => c.Int());
            CreateIndex("dbo.Request", "ProfileID");
            AddForeignKey("dbo.Request", "ProfileID", "dbo.Profile", "ProfileID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Request", "ProfileID", "dbo.Profile");
            DropIndex("dbo.Request", new[] { "ProfileID" });
            DropColumn("dbo.Request", "ProfileID");
        }
    }
}
