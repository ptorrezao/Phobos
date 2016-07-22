using Phobos.Library.Interfaces.Repos;
using Phobos.Library.Models;
using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.CoreServices.Db
{
    public class NotificationRepo : INotificationRepo
    {
        public bool SendNotification(UserNotification userNotification)
        {
            using (var context = new PhobosCoreContext())
            {
                userNotification.User = context.Users.First(x => x.Username == userNotification.User.Username);

                context.UserNotifications.Add(userNotification);

                return context.SaveChanges() > 0;
            }
        }

        public List<UserNotification> GetLastNotificationsForUser(string userName, int qtd)
        {
            using (var context = new PhobosCoreContext())
            {
                var userMessages = context.UserNotifications
                    .Where(x => x.User.Username == userName);

                if (userMessages.Count() == 0)
                {
                    return new List<UserNotification>();

                }
                return userMessages.Take(qtd).ToList();
            }
        }

        public void ClearNotifications(NotificationType notificationType, string userName)
        {
            using (var context = new PhobosCoreContext())
            {
                var userMessages = context.UserNotifications
                    .Where(x => x.User.Username == userName 
                        && x.Type == notificationType 
                        && !x.Read);

                foreach (var item in userMessages)
                {
                    item.Read = true;
                }

                context.SaveChanges();
            }
        }


        public void MarkNotificationAsRead(int id)
        {
            using (var context = new PhobosCoreContext())
            {
                var userMessages = context.UserNotifications
                    .Where(x => x.Id==id);

                foreach (var item in userMessages)
                {
                    item.Read = true;
                }

                context.SaveChanges();
            }
        }
    }
}
