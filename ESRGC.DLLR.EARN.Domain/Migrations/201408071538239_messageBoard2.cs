namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messageBoard2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MessageBoard", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MessageBoard", "Type");
        }
    }
}
