namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contactStateAbbrLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contact", "State", c => c.String(nullable: false, maxLength: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contact", "State", c => c.String(nullable: false));
        }
    }
}
