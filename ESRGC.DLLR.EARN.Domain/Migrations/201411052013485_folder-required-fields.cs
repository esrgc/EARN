namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class folderrequiredfields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Folder", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Folder", "Name", c => c.String());
        }
    }
}
