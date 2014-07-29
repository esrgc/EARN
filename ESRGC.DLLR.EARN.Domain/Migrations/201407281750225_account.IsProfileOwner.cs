namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class accountIsProfileOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "IsProfileOwner", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Account", "IsProfileOwner");
        }
    }
}
