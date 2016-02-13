using Phobos.ActionFilter;
using Phobos.Library.Models.Enums;
using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phobos.Controllers
{
    [ActionAutorize(true)]
    [PhobosInitialization]
    public class MessageController : Controller
    {
        public ActionResult Index(int? id)
        {
            var model = new MessageMailBoxViewModel()
            {
                CurrentFolder = new MessageMailBoxFolderViewModel()
                {
                    Name = "Inbox",
                    Icon = "Home",
                    QtdNewMessages = 0,
                    Selected = true,
                    FolderId = id ?? 0,
                    IconColor = TextColor.Black,
                    Messages = new List<MessageMailBoxItemViewModel>()
                }
            };
            model.CurrentFolder.Messages.Add(new MessageMailBoxItemViewModel() { Date = DateTime.Now.AddMinutes(-1), HasAttachment = false, Intro = "asdsadas...", IsFavorite = false, MessageId = 1, Sender = "me", Title = "title" });
            model.CurrentFolder.Messages.Add(new MessageMailBoxItemViewModel() { Date = DateTime.Now.AddMinutes(-6), HasAttachment = true, Intro = "asdsadas...", IsFavorite = true, MessageId =2, Sender = "me", Title = "title" });
            model.CurrentFolder.Messages.Add(new MessageMailBoxItemViewModel() { Date = DateTime.Now.AddMinutes(-65), HasAttachment = true, Intro = "asdsadas...", IsFavorite = false, MessageId = 3, Sender = "me", Title = "title" });
            model.Folders.Add(model.CurrentFolder);

            return View(model);
        }
        public ActionResult Compose()
        {
            return View();
        }

    }
}