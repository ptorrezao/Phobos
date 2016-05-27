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

        protected override void Seed(Phobos.Library.CoreServices.Db.PhobosCoreContext context)
        {
            this.PrepareDefaultFolders(context);

            this.AddRole(context, userRoleName);
            this.AddRole(context, adminRoleName);

            this.AddActionAuthorizations(context, "EditProfile", "Account");
            this.AddActionAuthorizations(context, "Index", "Message");
            this.AddActionAuthorizations(context, "GetFolderBox", "Message");
            this.AddActionAuthorizations(context, "Compose", "Message");
            this.AddActionAuthorizations(context, "CreateFolder", "Message");
            this.AddActionAuthorizations(context, "EditFolder", "Message");
            this.AddActionAuthorizations(context, "Move", "Message");
            this.AddActionAuthorizations(context, "Remove", "Message");
            this.AddActionAuthorizations(context, "FindNextMessage", "Message");
            this.AddActionAuthorizations(context, "ReadMessage", "Message");
            this.AddActionAuthorizations(context, "MarkAsFavorite", "Message");
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
            
            var actionAuthorization = context.ActionAuthorizations.FirstOrDefault(x => x.Action == controller && x.Controller == action);
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
                actionAuthorization.Roles = selectedRoles;
                context.Entry(actionAuthorization).State = EntityState.Modified;
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
        }
    }
}
