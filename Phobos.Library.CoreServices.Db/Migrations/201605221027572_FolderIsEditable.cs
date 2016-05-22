namespace Phobos.Library.CoreServices.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FolderIsEditable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserMessageFolders", "IsDraftFolder", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserMessageFolders", "IsInboxFolder", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserMessageFolders", "IsSentFolder", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserMessageFolders", "IsSentFolder");
            DropColumn("dbo.UserMessageFolders", "IsInboxFolder");
            DropColumn("dbo.UserMessageFolders", "IsDraftFolder");
        }
    }
}
