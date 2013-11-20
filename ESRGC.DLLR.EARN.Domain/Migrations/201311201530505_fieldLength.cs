namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fieldLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contact", "FirstName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Contact", "Middle", c => c.String(maxLength: 100));
            AlterColumn("dbo.Contact", "LastName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Organization", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Organization", "StreetAddress", c => c.String(nullable: false, maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Organization", "StreetAddress", c => c.String(nullable: false));
            AlterColumn("dbo.Organization", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Contact", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Contact", "Middle", c => c.String());
            AlterColumn("dbo.Contact", "FirstName", c => c.String(nullable: false));
        }
    }
}
