namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messageboardtype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "Type", c => c.String());
            DropColumn("dbo.MessageBoard", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MessageBoard", "Type", c => c.String());
            DropColumn("dbo.Message", "Type");
        }
    }
}
