namespace Phobos.Library.CoreServices.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRole_Functionality : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRoles", "ActionAuthorization_Id", "dbo.ActionAuthorizations");
            DropIndex("dbo.UserRoles", new[] { "ActionAuthorization_Id" });
            CreateTable(
                "dbo.UserRoleActionAuthorizations",
                c => new
                    {
                        UserRole_Id = c.Int(nullable: false),
                        ActionAuthorization_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserRole_Id, t.ActionAuthorization_Id })
                .ForeignKey("dbo.UserRoles", t => t.UserRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.ActionAuthorizations", t => t.ActionAuthorization_Id, cascadeDelete: true)
                .Index(t => t.UserRole_Id)
                .Index(t => t.ActionAuthorization_Id);
            
            AddColumn("dbo.UserRoles", "Name", c => c.String());
            DropColumn("dbo.UserRoles", "ActionAuthorization_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserRoles", "ActionAuthorization_Id", c => c.Int());
            DropForeignKey("dbo.UserRoleActionAuthorizations", "ActionAuthorization_Id", "dbo.ActionAuthorizations");
            DropForeignKey("dbo.UserRoleActionAuthorizations", "UserRole_Id", "dbo.UserRoles");
            DropIndex("dbo.UserRoleActionAuthorizations", new[] { "ActionAuthorization_Id" });
            DropIndex("dbo.UserRoleActionAuthorizations", new[] { "UserRole_Id" });
            DropColumn("dbo.UserRoles", "Name");
            DropTable("dbo.UserRoleActionAuthorizations");
            CreateIndex("dbo.UserRoles", "ActionAuthorization_Id");
            AddForeignKey("dbo.UserRoles", "ActionAuthorization_Id", "dbo.ActionAuthorizations", "Id");
        }
    }
}
