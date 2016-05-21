namespace Phobos.Library.CoreServices.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionAuthorizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Action = c.String(),
                        Controller = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionAuthorization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActionAuthorizations", t => t.ActionAuthorization_Id)
                .Index(t => t.ActionAuthorization_Id);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        LockedDate = c.DateTime(),
                        BirthDate = c.DateTime(),
                        LastLoginDate = c.DateTime(),
                        MemberSinceDate = c.DateTime(nullable: false),
                        CurrentStatus = c.Int(nullable: false),
                        FirstName = c.String(),
                        IsLocked = c.Boolean(nullable: false),
                        LastName = c.String(),
                        Password = c.String(),
                        Position = c.String(),
                        FailedAttempts = c.Int(nullable: false),
                        UserRole_Id = c.Int(),
                        ActionAuthorization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Username)
                .ForeignKey("dbo.UserRoles", t => t.UserRole_Id)
                .ForeignKey("dbo.ActionAuthorizations", t => t.ActionAuthorization_Id)
                .Index(t => t.UserRole_Id)
                .Index(t => t.ActionAuthorization_Id);
            
            CreateTable(
                "dbo.UserMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        SendDate = c.DateTime(),
                        MessageDate = c.DateTime(nullable: false),
                        Title = c.String(),
                        HasAttachment = c.Boolean(nullable: false),
                        IsFavorite = c.Boolean(nullable: false),
                        Sent = c.Boolean(nullable: false),
                        Folder_Id = c.Int(),
                        Owner_Username = c.String(maxLength: 128),
                        Receiver_Username = c.String(maxLength: 128),
                        Sender_Username = c.String(maxLength: 128),
                        UserAccount_Username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserMessageFolders", t => t.Folder_Id)
                .ForeignKey("dbo.UserAccounts", t => t.Owner_Username)
                .ForeignKey("dbo.UserAccounts", t => t.Receiver_Username)
                .ForeignKey("dbo.UserAccounts", t => t.Sender_Username)
                .ForeignKey("dbo.UserAccounts", t => t.UserAccount_Username)
                .Index(t => t.Folder_Id)
                .Index(t => t.Owner_Username)
                .Index(t => t.Receiver_Username)
                .Index(t => t.Sender_Username)
                .Index(t => t.UserAccount_Username);
            
            CreateTable(
                "dbo.UserMessageFolders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IconColor = c.Int(nullable: false),
                        Icon = c.String(),
                        User_Username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAccounts", t => t.User_Username)
                .Index(t => t.User_Username);
            
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        User_Username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAccounts", t => t.User_Username)
                .Index(t => t.User_Username);
            
            CreateTable(
                "dbo.UserTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Percentage = c.Double(nullable: false),
                        User_Username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAccounts", t => t.User_Username)
                .Index(t => t.User_Username);
            
            CreateTable(
                "dbo.Configurations",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Key);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAccounts", "ActionAuthorization_Id", "dbo.ActionAuthorizations");
            DropForeignKey("dbo.UserRoles", "ActionAuthorization_Id", "dbo.ActionAuthorizations");
            DropForeignKey("dbo.UserAccounts", "UserRole_Id", "dbo.UserRoles");
            DropForeignKey("dbo.UserTasks", "User_Username", "dbo.UserAccounts");
            DropForeignKey("dbo.UserNotifications", "User_Username", "dbo.UserAccounts");
            DropForeignKey("dbo.UserMessages", "UserAccount_Username", "dbo.UserAccounts");
            DropForeignKey("dbo.UserMessages", "Sender_Username", "dbo.UserAccounts");
            DropForeignKey("dbo.UserMessages", "Receiver_Username", "dbo.UserAccounts");
            DropForeignKey("dbo.UserMessages", "Owner_Username", "dbo.UserAccounts");
            DropForeignKey("dbo.UserMessageFolders", "User_Username", "dbo.UserAccounts");
            DropForeignKey("dbo.UserMessages", "Folder_Id", "dbo.UserMessageFolders");
            DropIndex("dbo.UserTasks", new[] { "User_Username" });
            DropIndex("dbo.UserNotifications", new[] { "User_Username" });
            DropIndex("dbo.UserMessageFolders", new[] { "User_Username" });
            DropIndex("dbo.UserMessages", new[] { "UserAccount_Username" });
            DropIndex("dbo.UserMessages", new[] { "Sender_Username" });
            DropIndex("dbo.UserMessages", new[] { "Receiver_Username" });
            DropIndex("dbo.UserMessages", new[] { "Owner_Username" });
            DropIndex("dbo.UserMessages", new[] { "Folder_Id" });
            DropIndex("dbo.UserAccounts", new[] { "ActionAuthorization_Id" });
            DropIndex("dbo.UserAccounts", new[] { "UserRole_Id" });
            DropIndex("dbo.UserRoles", new[] { "ActionAuthorization_Id" });
            DropTable("dbo.Configurations");
            DropTable("dbo.UserTasks");
            DropTable("dbo.UserNotifications");
            DropTable("dbo.UserMessageFolders");
            DropTable("dbo.UserMessages");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.UserRoles");
            DropTable("dbo.ActionAuthorizations");
        }
    }
}
