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
            for (int i = 1; i < 15; i++)
            {
                model.CurrentFolder.Messages.Add(new MessageMailBoxItemViewModel()
                {
                    Date = DateTime.Now.AddHours(-i),
                    HasAttachment = false,
                    Intro = "Trying to find a solution to this problem...",
                    IsFavorite = i % 2 == 0,
                    MessageId = i,
                    Sender = "Alexander Pierce",
                    Title = "AdminLTE 2.0 Issue "
                });

            }
            model.Folders.Add(model.CurrentFolder);

            return View(model);
        }

        [HttpGet]
        public ActionResult IndexGrid(String search)
        {
            var model = new MessageMailBoxViewModel()
            {
                CurrentFolder = new MessageMailBoxFolderViewModel()
                {
                    Name = "Inbox",
                    Icon = "Home",
                    QtdNewMessages = 0,
                    Selected = true,
                    FolderId = 1,
                    IconColor = TextColor.Black,
                    Messages = new List<MessageMailBoxItemViewModel>()
                }
            };
            for (int i = 1; i < 15; i++)
            {
                model.CurrentFolder.Messages.Add(new MessageMailBoxItemViewModel()
                {
                    Date = DateTime.Now.AddHours(-i),
                    HasAttachment = false,
                    Intro = "Trying to find a solution to this problem...",
                    IsFavorite = i % 2 == 0,
                    MessageId = i,
                    Sender = "Alexander Pierce",
                    Title = "AdminLTE 2.0 Issue "
                });

            }

            return PartialView("_FolderContentItemList", model.CurrentFolder);
        }
        public ActionResult Compose()
        {
            return View();
        }

    }
}