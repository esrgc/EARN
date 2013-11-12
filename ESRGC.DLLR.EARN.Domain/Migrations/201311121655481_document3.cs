namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class document3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Document", "Description", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Document", "Description");
        }
    }
}
