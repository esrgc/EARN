namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orgdescriptionrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Organization", "Description", c => c.String(nullable: false, maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Organization", "Description", c => c.String(maxLength: 1000));
        }
    }
}
