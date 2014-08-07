namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messageBoard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageBoard",
                c => new
                    {
                        MessageBoardID = c.Int(nullable: false, identity: true),
                        MessageID = c.Int(),
                        ProfileID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MessageBoardID)
                .ForeignKey("dbo.Message", t => t.MessageID)
                .ForeignKey("dbo.Profile", t => t.ProfileID, cascadeDelete: true)
                .Index(t => t.MessageID)
                .Index(t => t.ProfileID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageBoard", "ProfileID", "dbo.Profile");
            DropForeignKey("dbo.MessageBoard", "MessageID", "dbo.Message");
            DropIndex("dbo.MessageBoard", new[] { "ProfileID" });
            DropIndex("dbo.MessageBoard", new[] { "MessageID" });
            DropTable("dbo.MessageBoard");
        }
    }
}
