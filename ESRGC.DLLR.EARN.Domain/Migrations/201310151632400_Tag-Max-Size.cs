namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TagMaxSize : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tag", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tag", "Name", c => c.String(nullable: false, maxLength: 20));
        }
    }
}