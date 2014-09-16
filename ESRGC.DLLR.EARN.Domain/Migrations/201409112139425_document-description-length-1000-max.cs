namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class documentdescriptionlength1000max : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Document", "Description", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Document", "Description", c => c.String(maxLength: 200));
        }
    }
}
