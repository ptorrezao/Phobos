using Phobos.Library.Interfaces.Repos;
using Phobos.Library.Models;
using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phobos.UnitTest.Repos
{
    public class TestNotificationRepo : INotificationRepo
    {

        static List<UserNotification> notifications = new List<UserNotification>() { };

        public bool SendNotification(UserNotification userNotification)
        {
            userNotification.Id = notifications.Count + 1;

            notifications.Add(userNotification);

            return true;
        }

        public List<UserNotification> GetLastNotificationsForUser(string userName, int qtd, bool onlyUnread)
        {
            return notifications.Where(x => x.User.Username == userName).Take(qtd).ToList();
        }

        public void ClearNotifications(NotificationType notificationType, string userName)
        {
            notifications.Where(x => x.User.Username == userName && x.Type == notificationType).ToList().ForEach(x => x.Read = true);
        }

        public void MarkNotificationAsRead(int id)
        {
            notifications.Where(x => x.Id == id).ToList().ForEach(x => x.Read = true);
        }
    }
}
