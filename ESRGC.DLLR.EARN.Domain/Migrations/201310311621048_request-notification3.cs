namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestnotification3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Request", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Request", "Created");
            DropColumn("dbo.Notification", "Created");
        }
    }
}
