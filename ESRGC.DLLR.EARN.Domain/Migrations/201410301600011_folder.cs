namespace ESRGC.DLLR.EARN.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class folder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Folder",
                c => new
                    {
                        FolderID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                        Description = c.String(),
                        ParentFolderID = c.Int(),
                    })
                .PrimaryKey(t => t.FolderID)
                .ForeignKey("dbo.Folder", t => t.ParentFolderID)
                .Index(t => t.ParentFolderID);
            
            AddColumn("dbo.Document", "FolderID", c => c.Int());
            CreateIndex("dbo.Document", "FolderID");
            AddForeignKey("dbo.Document", "FolderID", "dbo.Folder", "FolderID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Folder", "ParentFolderID", "dbo.Folder");
            DropForeignKey("dbo.Document", "FolderID", "dbo.Folder");
            DropIndex("dbo.Folder", new[] { "ParentFolderID" });
            DropIndex("dbo.Document", new[] { "FolderID" });
            DropColumn("dbo.Document", "FolderID");
            DropTable("dbo.Folder");
        }
    }
}
