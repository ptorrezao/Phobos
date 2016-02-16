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
            for (int i = 1; i < 50; i++)
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
            model.Folders.Add(new MessageMailBoxFolderViewModel()
            {
                Name = "ALD",
                Icon = "Tags",
                QtdNewMessages = 0,
                Selected = false,
                FolderId = 1,
                IconColor = TextColor.Red,
                Messages = new List<MessageMailBoxItemViewModel>()
            });
            foreach (var item in model.Folders)
            {
                model.CurrentFolder.Folders.Add(new MessageMailBoxFolderItemViewModel()
                {
                    FolderId = item.FolderId,
                    Title = item.Name
                });
            }

            return View(model);
        }

        public ActionResult Compose()
        {
            return View();
        }

        public ActionResult MarkAsFavorite(int Id, string returnUrl)
        {
            if (!Url.IsLocalUrl(returnUrl))
            {

            }

            return this.Redirect(returnUrl);
        }
        public ActionResult ReadMessage(int Id)
        {
            return this.RedirectToAction("Index");
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Remove(string[] selectedIds)
        {
            return null;
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Move(MessageMailBoxFolderViewModel model, string[] selectedIds)
        {
            return null;
        }
    }
}