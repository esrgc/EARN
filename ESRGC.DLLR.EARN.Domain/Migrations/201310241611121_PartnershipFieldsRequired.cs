namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartnershipFieldsRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Partnership", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Partnership", "Status", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Partnership", "Description", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Partnership", "GrantStatus", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Partnership", "GrantStatus", c => c.String());
            AlterColumn("dbo.Partnership", "Description", c => c.String());
            AlterColumn("dbo.Partnership", "Status", c => c.String());
            AlterColumn("dbo.Partnership", "Name", c => c.String());
        }
    }
}
