using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Phobos.Library.Models;

namespace Phobos.Library.CoreServices.Db
{
    public class PhobosCoreContext : DbContext
    {
        public PhobosCoreContext()
        {

        }

        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<ActionAuthorization> ActionAuthorizations { get; set; }
        public DbSet<UserAccount> Users { get; set; }
        public DbSet<UserMessage> UserMessages { get;set; }
        public DbSet<UserNotification> UserNotifications { get;set; }
        public DbSet<UserTask> UserTasks { get;set; }

    }
}
