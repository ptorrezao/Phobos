namespace Phobos.Library.CoreServices.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IsRead : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserNotifications", "Read", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserNotifications", "Read");
        }
    }
}
