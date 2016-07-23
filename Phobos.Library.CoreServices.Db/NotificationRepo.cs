using System;
using System.Collections.Generic;
using System.Linq;
using Phobos.Library.Interfaces.Repos;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Phobos.Library.Models;
using System.Data.Entity;
using Phobos.Library.Models.Enums;

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

        public List<UserNotification> GetLastNotificationsForUser(string userName, int qtd, bool onlyUnread)
        {
            using (var context = new PhobosCoreContext())
            {
                var userMessages = context.UserNotifications
                    .Where(x => x.User.Username == userName);

                if (onlyUnread)
                {
                    userMessages = userMessages.Where(x => x.Read == false);
                }

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

        public List<UserNotification> GetNotifications(string userName)
        {
            using (var context = new PhobosCoreContext())
            {
                var userMessages = context.UserNotifications
                    .Include(x=>x.User)
                    .Where(x => x.User.Username == userName);

                return userMessages.ToList();
            }
        }

        public void DeleteNotification(string userName, int selectedInt)
        {
            using (var context = new PhobosCoreContext())
            {
                var notifications = context.UserNotifications
                    .Where(x => x.User.Username == userName
                        && x.Id == selectedInt);

                context.UserNotifications.RemoveRange(notifications);
                context.SaveChanges();
            }
        }
    }
}
