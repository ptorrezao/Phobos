namespace Phobos.Library.CoreServices.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColorandIcontoNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserNotifications", "IconColor", c => c.Int(nullable: false));
            AddColumn("dbo.UserNotifications", "Icon", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserNotifications", "Icon");
            DropColumn("dbo.UserNotifications", "IconColor");
        }
    }
}
