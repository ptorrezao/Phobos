namespace Phobos.Library.CoreServices.Db.Migrations
{
    using Phobos.Library.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Phobos.Library.CoreServices.Db.PhobosCoreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Phobos.Library.CoreServices.Db.PhobosCoreContext";
        }

        const string userRoleName = "User";
        const string adminRoleName = "Administrator";
        readonly string[] defaultUsers = { userRoleName };
        readonly string[] defaultAdminUsers = { adminRoleName };

        protected override void Seed(PhobosCoreContext context)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
            this.AddRole(context, userRoleName);
            this.AddRole(context, adminRoleName);

            this.AddUsers(context);
            this.AddAdministratorUsers(context);

            this.AddCurrentUsersToRoles(context, defaultUsers);

            this.AddActionAuthorizations(context, "EditProfile", "Account");
            this.AddActionAuthorizations(context, "ListUsers", "Account");
            this.AddActionAuthorizations(context, "CreateProfile", "Account", new string[1] { "Administrator" });

            this.AddActionAuthorizations(context, "Index", "Home");

            this.AddActionAuthorizations(context, "GetFolderBox", "Message");

            this.AddActionAuthorizations(context, "GoToNotification", "Notification");
            this.AddActionAuthorizations(context, "Index", "Notification");

            this.AddActionAuthorizations(context, "Index", "Message");
            this.AddActionAuthorizations(context, "Compose", "Message");
            this.AddActionAuthorizations(context, "CreateFolder", "Message");
            this.AddActionAuthorizations(context, "EditFolder", "Message");
            this.AddActionAuthorizations(context, "RemoveFolder", "Message");
            this.AddActionAuthorizations(context, "Move", "Message");
            this.AddActionAuthorizations(context, "Remove", "Message");
            this.AddActionAuthorizations(context, "FindNextMessage", "Message");
            this.AddActionAuthorizations(context, "ReadMessage", "Message");
            this.AddActionAuthorizations(context, "MarkAsFavorite", "Message");


            this.PrepareDefaultFolders(context);

        }

        private void AddAdministratorUsers(PhobosCoreContext context)
        {
            var role = context.Roles.FirstOrDefault(x => x.Name == adminRoleName);

            if (role == default(UserRole)) { context.Roles.Add(new UserRole() { Name = adminRoleName }); }

            var user = context.Users.FirstOrDefault(x => x.Username == "admin@admin.pt");

            if (user == null)
            {
                user = new UserAccount()
                {
                    Username = "admin@admin.pt",
                    BirthDate = DateTime.Now,
                    Password = "da574a7b983b052fef321bd761d49f70", //Admin#123
                    Position = "Administrator",
                    FirstName = "Administrator",
                    MemberSinceDate = DateTime.Now,
                    CurrentStatus = Models.Enums.UserStatusEnum.Offline,
                    LockedDate = null,
                    LastLoginDate = null,
                    IsLocked = false,
                    LastName = null,
                };

                user.Roles.Add(role);
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        private void AddUsers(PhobosCoreContext context)
        {
            string[] firstNames = { "", "Mark", "Paul", "Peter", "John", "Rob" };

            for (int i = 1; i <= 5; i++)
            {
                var user = context.Users.FirstOrDefault(x => x.Username == "user" + i + "@phobos.pt");

                if (user == null)
                {
                    user = new UserAccount()
                    {
                        Username = "user" + i + "@phobos.pt",
                        BirthDate = DateTime.Now,
                        Password = "01ab2cfd811481348e8148b44a77ac4e", //User#123
                        Position = "User",
                        FirstName = firstNames[i],
                        MemberSinceDate = DateTime.Now
                    };
                    context.Users.Add(user);
                    context.SaveChanges();
                }

            }
        }

        private void AddCurrentUsersToRoles(PhobosCoreContext context, string[] defaultUsers)
        {
            var selectedRoles = defaultUsers.Select(x => context.Roles.Where(role => role.Name == x).First()).ToList();

            foreach (UserAccount user in context.Users)
            {
                foreach (UserRole role in selectedRoles)
                {
                    if (!user.Roles.Any(x => x.Id == role.Id))
                    {
                        user.Roles.Add(role);
                    }
                }
            }
        }

        private void AddRole(PhobosCoreContext context, string roleName)
        {
            var role = context.Roles.FirstOrDefault(x => x.Name == roleName);
            if (role == default(UserRole))
            {
                context.Roles.Add(new UserRole()
                {
                    Name = roleName
                });
            }
            context.SaveChanges();
        }

        private void AddActionAuthorizations(PhobosCoreContext context, string action, string controller, string[] roles = null)
        {
            roles = roles ?? defaultUsers;

            var actionAuthorization = context.ActionAuthorizations.FirstOrDefault(x => x.Action == action && x.Controller == controller);
            var selectedRoles = roles.Select(x => context.Roles.Where(role => role.Name == x).First()).ToList();

            if (actionAuthorization == default(ActionAuthorization))
            {
                actionAuthorization = new ActionAuthorization()
                {
                    Controller = controller,
                    Action = action,
                    Roles = selectedRoles
                };

                context.ActionAuthorizations.Add(actionAuthorization);
            }
            else
            {
                foreach (var role in selectedRoles)
                {
                    if (!actionAuthorization.Roles.Any(x => x.Id == role.Id))
                    {
                        actionAuthorization.Roles.Add(role);
                    }
                }
            }

            context.SaveChanges();
        }

        private void PrepareDefaultFolders(Phobos.Library.CoreServices.Db.PhobosCoreContext context)
        {
            foreach (var folder in context.UserMessageFolders)
            {
                folder.IsDraftFolder = folder.Name == MessageRepo.DraftFolderName;
                folder.IsInboxFolder = folder.Name == MessageRepo.InboxFolderName;
                folder.IsSentFolder = folder.Name == MessageRepo.SentFolderName;

                if (folder.IsDraftFolder || folder.IsInboxFolder || folder.IsSentFolder)
                {
                    folder.Icon = folder.IsDraftFolder ? "edit" : folder.Icon;
                    folder.Icon = folder.IsInboxFolder ? "inbox" : folder.Icon;
                    folder.Icon = folder.IsSentFolder ? "send" : folder.Icon;
                    folder.IconColor = Models.Enums.TextColor.Blue;
                }
            }

            context.SaveChanges();
        }
    }
}
