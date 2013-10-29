namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartnershipInvite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Request", "Discriminator");
        }
    }
}
