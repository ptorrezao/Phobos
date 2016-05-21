namespace Phobos.Library.CoreServices.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Draft_Funtionality : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserMessages", "IsDraft", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserMessages", "IsDraft");
        }
    }
}
