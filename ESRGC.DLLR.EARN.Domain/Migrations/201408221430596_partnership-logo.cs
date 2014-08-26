namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class partnershiplogo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Partnership", "PictureID", c => c.Int());
            CreateIndex("dbo.Partnership", "PictureID");
            AddForeignKey("dbo.Partnership", "PictureID", "dbo.Picture", "PictureID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Partnership", "PictureID", "dbo.Picture");
            DropIndex("dbo.Partnership", new[] { "PictureID" });
            DropColumn("dbo.Partnership", "PictureID");
        }
    }
}
