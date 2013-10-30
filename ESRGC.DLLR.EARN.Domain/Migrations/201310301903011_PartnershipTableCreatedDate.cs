namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartnershipTableCreatedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Partnership", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Partnership", "Created");
        }
    }
}
