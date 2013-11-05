namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comment2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comment", "Content", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comment", "Content", c => c.String(maxLength: 1000));
        }
    }
}
