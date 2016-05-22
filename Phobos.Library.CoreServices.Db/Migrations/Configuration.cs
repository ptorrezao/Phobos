namespace Phobos.Library.CoreServices.Db.Migrations
{
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

        protected override void Seed(Phobos.Library.CoreServices.Db.PhobosCoreContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

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
