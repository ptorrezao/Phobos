using Phobos.Library.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phobos.Controllers
{
    public class NotificationController : Controller
    {
        public NotificationController(
            INotificationService notificationService)
        {
            this.NotificationService = notificationService;
        }

        public ActionResult GoToNotification(int id, string url)
        {
            this.NotificationService.MarkNotificationAsRead(id);

            if (string.IsNullOrEmpty(url))
            {
                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                return this.Redirect(url);
            }
        }

        public INotificationService NotificationService { get; set; }
    }
}