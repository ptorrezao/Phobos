namespace Phobos.Library.CoreServices.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupUsersUnderRole : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserAccounts", "UserRole_Id", "dbo.UserRoles");
            DropIndex("dbo.UserAccounts", new[] { "UserRole_Id" });
            CreateTable(
                "dbo.UserAccountUserRoles",
                c => new
                    {
                        UserAccount_Username = c.String(nullable: false, maxLength: 128),
                        UserRole_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserAccount_Username, t.UserRole_Id })
                .ForeignKey("dbo.UserAccounts", t => t.UserAccount_Username, cascadeDelete: true)
                .ForeignKey("dbo.UserRoles", t => t.UserRole_Id, cascadeDelete: true)
                .Index(t => t.UserAccount_Username)
                .Index(t => t.UserRole_Id);
            
            DropColumn("dbo.UserAccounts", "UserRole_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserAccounts", "UserRole_Id", c => c.Int());
            DropForeignKey("dbo.UserAccountUserRoles", "UserRole_Id", "dbo.UserRoles");
            DropForeignKey("dbo.UserAccountUserRoles", "UserAccount_Username", "dbo.UserAccounts");
            DropIndex("dbo.UserAccountUserRoles", new[] { "UserRole_Id" });
            DropIndex("dbo.UserAccountUserRoles", new[] { "UserAccount_Username" });
            DropTable("dbo.UserAccountUserRoles");
            CreateIndex("dbo.UserAccounts", "UserRole_Id");
            AddForeignKey("dbo.UserAccounts", "UserRole_Id", "dbo.UserRoles", "Id");
        }
    }
}
