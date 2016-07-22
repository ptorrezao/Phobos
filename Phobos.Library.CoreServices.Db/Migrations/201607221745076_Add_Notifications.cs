namespace Phobos.Library.CoreServices.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Notifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserNotifications", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserNotifications", "Type");
        }
    }
}
