using AutoMapper;
using Phobos.ActionFilter;
using Phobos.Library.Interfaces;
using Phobos.Library.Interfaces.Services;
using Phobos.Library.Models.Enums;
using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Phobos.Library.Models;
using Ninject;
using Phobos.Library.Utils;
using Phobos.App_Utils;

namespace Phobos.Controllers
{
    [ActionAutorize(true)]
    [PhobosInitialization]
    public class MessageController : Controller
    {
        private IUserManagementService userManagementService;
        private IMessageService messageService;
        private IAuditTrailService auditTrailService;

        public MessageController(
            IMessageService messageService,
            IUserManagementService userManagementService,
            IAuditTrailService auditTrailService)
        {
            this.userManagementService = userManagementService;
            this.messageService = messageService;
            this.auditTrailService = auditTrailService;
        }

        public ActionResult Index(int? id)
        {
            var foldersForUser = messageService.GetAllFoldersForUser(SessionManager.CurrentUsername);
            var currentFolder = messageService.GetFolder(SessionManager.CurrentUsername, id);
            var mapper = AutoMapperConfiguration.GetMapper();

            var model = new MessageMailBoxViewModel()
            {
                CurrentFolder = mapper.Map<UserMessageFolder, MessageMailBoxFolderViewModel>(currentFolder),
                Folders = mapper.Map<List<UserMessageFolder>, List<MessageMailBoxFolderViewModel>>(foldersForUser)
            };

            SessionManager.CurrentFolderId = currentFolder.Id;

            return View(model);
        }

        public ActionResult GetFolderBox()
        {
            var mapper = AutoMapperConfiguration.GetMapper();

            var foldersForUser = messageService.GetAllFoldersForUser(SessionManager.CurrentUsername);
            var viewModel = mapper.Map<List<UserMessageFolder>, List<MessageMailBoxFolderViewModel>>(foldersForUser);
            return this.PartialView("_FolderBox", viewModel);
        }

        public ActionResult Compose()
        {
            UserMessage model = new UserMessage() { };
            model.Owner = model.Sender = userManagementService.GetUser(SessionManager.CurrentUsername);
            MessageMailBoxItemViewModel newMessage = AutoMapperConfiguration.GetMapper().Map<MessageMailBoxItemViewModel>(model);
            return View(newMessage);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Compose(string submit, MessageMailBoxItemViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            if (submit == "Send")
            {
                UserMessage newMessage = AutoMapperConfiguration.GetMapper().Map<UserMessage>(model);

                newMessage.Attachments = this.SaveAttachements(files);

                newMessage = messageService.SendMessage(SessionManager.CurrentUsername, newMessage);

                return this.RedirectToAction("ReadMessage", new { id = newMessage.Id });
            }
            else if (submit == "Draft")
            {
                UserMessage newMessage = AutoMapperConfiguration.GetMapper().Map<UserMessage>(model);

                newMessage.Attachments = this.SaveAttachements(files);

                UserMessage createdMessage = messageService.SaveMessage(SessionManager.CurrentUsername, newMessage);

                return this.RedirectToAction("ReadMessage", new { id = createdMessage.Id, allowEdit = true });
            }
            else if (submit == "Discard")
            {
                if (model.MessageId > 0)
                {
                    messageService.DeleteMessage(model.MessageId);
                }
            }

            return this.View(model);
        }

        private List<string> SaveAttachements(IEnumerable<HttpPostedFileBase> files)
        {
            var listOfFiles = new List<string>();
            foreach (var item in files)
            {
                if (item != null)
                {
                    var newFileName = string.Format("{2}\\{0}_{1}", Guid.NewGuid(), item.FileName, HttpContext.Request.PhysicalApplicationPath);
                    item.SaveAs(newFileName);
                    listOfFiles.Add(newFileName);
                }
            }
            return listOfFiles;
        }

        public ActionResult MarkAsFavorite(int Id, string returnUrl)
        {
            if (!Url.IsLocalUrl(returnUrl))
            {
                var message = messageService.GetMessage(SessionManager.CurrentUsername, Id);
                message.IsFavorite = !message.IsFavorite;
                messageService.SaveMessage(SessionManager.CurrentUsername, message);
            }

            return this.Redirect(returnUrl);
        }

        public ActionResult ReadMessage(int Id)
        {
            var message = messageService.GetMessage(SessionManager.CurrentUsername, Id);
            MessageMailBoxItemViewModel newMessage = AutoMapperConfiguration.GetMapper().Map<MessageMailBoxItemViewModel>(message);

            if (newMessage== null)
            {
                return this.RedirectToAction("Index");
            }

            return View(newMessage);
        }

        public ActionResult FindNextMessage(int Id, bool IsPrevious = false)
        {
            UserMessage message = messageService.GetMessage(SessionManager.CurrentUsername, Id);
            var folder = messageService.GetFolder(SessionManager.CurrentUsername, message.Folder.Id);
            int currentElementIndex = folder.Messages.Select(x => x.Id).ToList().IndexOf(message.Id);
            int nextElementIndex = IsPrevious ? currentElementIndex - 1 : currentElementIndex + 1;


            if (IsPrevious && nextElementIndex < 0)
            {
                nextElementIndex = folder.Messages.Count - 1;
            }
            else if (!IsPrevious && nextElementIndex > folder.Messages.Count - 1)
            {
                nextElementIndex = 0;
            }

            return this.RedirectToAction("ReadMessage", new { Id = folder.Messages[nextElementIndex].Id });
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Remove(string[] selectedIds)
        {
            foreach (var selectedId in selectedIds)
            {
                int selectedInt = 0;
                if (int.TryParse(selectedId, out selectedInt))
                {
                    this.messageService.DeleteMessage(selectedInt);
                }
            }

            return this.RedirectToAction("Index");
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(string search)
        {
            return this.RedirectToAction("Index");
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Move(MessageMailBoxFolderViewModel model, string[] selectedIds)
        {
            return this.RedirectToAction("Index");
        }
    }
}