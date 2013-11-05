namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notificationEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "EmailVerified", c => c.Boolean(nullable: false));
            AddColumn("dbo.Account", "VerificationCode", c => c.String());
            AddColumn("dbo.Notification", "EmailSent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "EmailSent");
            DropColumn("dbo.Account", "VerificationCode");
            DropColumn("dbo.Account", "EmailVerified");
        }
    }
}
