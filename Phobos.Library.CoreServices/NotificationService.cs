using Ninject;
using Phobos.Library.Interfaces.Repos;
using Phobos.Library.Interfaces.Services;
using Phobos.Library.Models;
using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.CoreServices
{
    public class NotificationService : INotificationService
    {
        #region Injects
        [Inject]
        public INotificationRepo Repository { get; set; }
        #endregion

        public bool SendNotification(UserNotification userNotification)
        {
            return this.Repository.SendNotification(userNotification);
        }

        public List<UserNotification> GetLastNotifications(string userName, int qtd, bool onlyUnread)
        {
            return this.Repository.GetLastNotificationsForUser(userName, qtd, true);
        }

        public void ClearNotifications(NotificationType notificationType, string userName)
        {
            this.Repository.ClearNotifications(notificationType, userName);
        }


        public void MarkNotificationAsRead(int id)
        {
            this.Repository.MarkNotificationAsRead(id);
        }
    }
}
