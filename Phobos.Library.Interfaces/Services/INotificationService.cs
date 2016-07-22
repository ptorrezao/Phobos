﻿using Phobos.Library.Models;
using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Interfaces.Services
{
    public interface INotificationService
    {
        bool SendNotification(UserNotification userNotification);

        List<UserNotification> GetLastNotifications(string userName, int qtd);

        void ClearNotifications(NotificationType notificationType, string userName);

        void MarkNotificationAsRead(int id);
    }
}
