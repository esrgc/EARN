namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletegrantStatus : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Partnership", "GrantStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Partnership", "GrantStatus", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
