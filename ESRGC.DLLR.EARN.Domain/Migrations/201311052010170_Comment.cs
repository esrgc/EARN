namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 1000),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        AuthorID = c.Int(nullable: false),
                        PartnershipID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Profile", t => t.AuthorID, cascadeDelete: true)
                .ForeignKey("dbo.Partnership", t => t.PartnershipID, cascadeDelete: true)
                .Index(t => t.AuthorID)
                .Index(t => t.PartnershipID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "PartnershipID", "dbo.Partnership");
            DropForeignKey("dbo.Comment", "AuthorID", "dbo.Profile");
            DropIndex("dbo.Comment", new[] { "PartnershipID" });
            DropIndex("dbo.Comment", new[] { "AuthorID" });
            DropTable("dbo.Comment");
        }
    }
}
