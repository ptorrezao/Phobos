namespace Phobos.Library.CoreServices.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAuthoriazation_Functionality : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserAccounts", "ActionAuthorization_Id", "dbo.ActionAuthorizations");
            DropIndex("dbo.UserAccounts", new[] { "ActionAuthorization_Id" });
            CreateTable(
                "dbo.UserAccountActionAuthorizations",
                c => new
                    {
                        UserAccount_Username = c.String(nullable: false, maxLength: 128),
                        ActionAuthorization_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserAccount_Username, t.ActionAuthorization_Id })
                .ForeignKey("dbo.UserAccounts", t => t.UserAccount_Username, cascadeDelete: true)
                .ForeignKey("dbo.ActionAuthorizations", t => t.ActionAuthorization_Id, cascadeDelete: true)
                .Index(t => t.UserAccount_Username)
                .Index(t => t.ActionAuthorization_Id);
            
            DropColumn("dbo.UserAccounts", "ActionAuthorization_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserAccounts", "ActionAuthorization_Id", c => c.Int());
            DropForeignKey("dbo.UserAccountActionAuthorizations", "ActionAuthorization_Id", "dbo.ActionAuthorizations");
            DropForeignKey("dbo.UserAccountActionAuthorizations", "UserAccount_Username", "dbo.UserAccounts");
            DropIndex("dbo.UserAccountActionAuthorizations", new[] { "ActionAuthorization_Id" });
            DropIndex("dbo.UserAccountActionAuthorizations", new[] { "UserAccount_Username" });
            DropTable("dbo.UserAccountActionAuthorizations");
            CreateIndex("dbo.UserAccounts", "ActionAuthorization_Id");
            AddForeignKey("dbo.UserAccounts", "ActionAuthorization_Id", "dbo.ActionAuthorizations", "Id");
        }
    }
}
