namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestnowrefprofile : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Request", "ReceiverID", "dbo.Account");
            DropForeignKey("dbo.Request", "SenderID", "dbo.Account");
            AddColumn("dbo.Request", "Account_AccountID", c => c.Int());
            AddColumn("dbo.Request", "Account_AccountID1", c => c.Int());
            CreateIndex("dbo.Request", "Account_AccountID");
            CreateIndex("dbo.Request", "Account_AccountID1");
            AddForeignKey("dbo.Request", "Account_AccountID", "dbo.Account", "AccountID");
            AddForeignKey("dbo.Request", "Account_AccountID1", "dbo.Account", "AccountID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Request", "Account_AccountID1", "dbo.Account");
            DropForeignKey("dbo.Request", "Account_AccountID", "dbo.Account");
            DropIndex("dbo.Request", new[] { "Account_AccountID1" });
            DropIndex("dbo.Request", new[] { "Account_AccountID" });
            DropColumn("dbo.Request", "Account_AccountID1");
            DropColumn("dbo.Request", "Account_AccountID");
            AddForeignKey("dbo.Request", "SenderID", "dbo.Account", "AccountID");
            AddForeignKey("dbo.Request", "ReceiverID", "dbo.Account", "AccountID");
        }
    }
}
