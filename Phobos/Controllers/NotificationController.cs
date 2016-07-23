using Phobos.ActionFilter;
using Phobos.App_Utils;
using Phobos.Library.Interfaces.Services;
using Phobos.Library.Models;
using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phobos.Controllers
{
    [ActionAutorize]
    [PhobosInitialization]
    public class NotificationController : Controller
    {
        public INotificationService NotificationService { get; set; }

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

        public ActionResult Index()
        {
            var notifications = NotificationService.GetNotifications(SessionManager.CurrentUsername);
            var mapper = AutoMapperConfiguration.GetMapper();

            var model = mapper.Map<List<UserNotification>, List<UserNotificationViewModel>>(notifications);

            return View(model);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Remove(string[] selectedIds)
        {
            if (selectedIds != null && selectedIds.Count() > 0)
            {
                foreach (var selectedId in selectedIds)
                {
                    int selectedInt = 0;
                    if (int.TryParse(selectedId, out selectedInt))
                    {
                        this.NotificationService.DeleteNotification(SessionManager.CurrentUsername, selectedInt);
                    }
                }
            }
            return this.RedirectToAction("Index");
        }
    }
}