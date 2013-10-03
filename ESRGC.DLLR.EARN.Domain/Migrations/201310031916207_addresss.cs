namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addresss : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Organization", "Zip", c => c.String(nullable: false, maxLength: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Organization", "Zip", c => c.String(nullable: false));
        }
    }
}
