namespace Phobos.Library.CoreServices.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_LinkToNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserNotifications", "Link", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserNotifications", "Link");
        }
    }
}
