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
    [ActionAutorize]
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

            var currentFolderViewModel = mapper.Map<UserMessageFolder, MessageMailBoxFolderViewModel>(currentFolder);
            currentFolderViewModel.Folders = mapper.Map<List<UserMessageFolder>, List<MessageMailBoxFolderViewModel>>(foldersForUser);

            var model = new MessageMailBoxViewModel()
            {
                CurrentFolder = currentFolderViewModel,
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

                //// newMessage.Attachments = this.SaveAttachements(files);

                newMessage = messageService.SendMessage(SessionManager.CurrentUsername, newMessage);

                return this.RedirectToAction("ReadMessage", new { id = newMessage.Id });
            }
            else if (submit == "Draft")
            {
                UserMessage newMessage = AutoMapperConfiguration.GetMapper().Map<UserMessage>(model);

                ////  newMessage.Attachments = this.SaveAttachements(files);

                UserMessage createdMessage = messageService.SaveMessage(SessionManager.CurrentUsername, newMessage);

                return this.RedirectToAction("Index", new { id = createdMessage.Folder.Id });
            }
            else if (submit == "Discard")
            {
                if (model.MessageId > 0)
                {
                    messageService.DeleteMessage(SessionManager.CurrentUsername, model.MessageId);
                }

                return this.RedirectToAction("Index");
            }

            return this.View(model);
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

            if (message == null)
            {
                return this.RedirectToAction("Index");
            }

            if (message.Owner.Username == SessionManager.CurrentUsername && message.IsDraft)
            {
                MessageMailBoxItemViewModel newMessage = AutoMapperConfiguration.GetMapper().Map<MessageMailBoxItemViewModel>(message);
                return View("Compose", newMessage);
            }
            else
            {
                MessageMailBoxItemViewModel newMessage = AutoMapperConfiguration.GetMapper().Map<MessageMailBoxItemViewModel>(message);

                return View(newMessage);
            }
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
            if (selectedIds != null && selectedIds.Count() > 0)
            {
                foreach (var selectedId in selectedIds)
                {
                    int selectedInt = 0;
                    if (int.TryParse(selectedId, out selectedInt))
                    {
                        this.messageService.DeleteMessage(SessionManager.CurrentUsername, selectedInt);
                    }
                }
            }
            return this.RedirectToAction("Index");
        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Move(string newFolderId, string[] selectedIds)
        {
            if (selectedIds != null && selectedIds.Count() > 0)
            {
                foreach (var selectedId in selectedIds)
                {
                    int selectedInt = 0;
                    int newFolderIdInt = 0;

                    if (int.TryParse(selectedId, out selectedInt))
                    {
                        if (int.TryParse(newFolderId, out newFolderIdInt))
                        {
                            this.messageService.MoveMessageToFolder(SessionManager.CurrentUsername, selectedInt, newFolderIdInt);
                        }
                    }
                }
            }
            return this.RedirectToAction("Index");

            return this.RedirectToAction("Index");
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(string search)
        {
            return this.RedirectToAction("Index");
        }

        public ActionResult EditFolder(int Id)
        {
            var mapper = AutoMapperConfiguration.GetMapper();
            var foldersForUser = messageService.GetAllFoldersForUser(SessionManager.CurrentUsername);
            var folder = messageService.GetFolder(SessionManager.CurrentUsername, Id);
            if (folder != null)
            {
                var viewModel = mapper.Map<UserMessageFolder, MessageMailBoxFolderViewModel>(folder);
                viewModel.Folders = mapper.Map<List<UserMessageFolder>, List<MessageMailBoxFolderViewModel>>(foldersForUser);
                return this.View(viewModel);
            }

            return this.RedirectToAction("Index");
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditFolder(MessageMailBoxFolderViewModel viewModel)
        {
            var mapper = AutoMapperConfiguration.GetMapper();
            var model = mapper.Map<MessageMailBoxFolderViewModel, UserMessageFolder>(viewModel);

            if (Request.Form["submit"] == "Save")
            {
                var updatedFolder = messageService.SaveFolder(SessionManager.CurrentUsername, model);
                viewModel = mapper.Map<UserMessageFolder, MessageMailBoxFolderViewModel>(updatedFolder);
            }

            var foldersForUser = messageService.GetAllFoldersForUser(SessionManager.CurrentUsername);
            var folders = mapper.Map<List<UserMessageFolder>, List<MessageMailBoxFolderViewModel>>(foldersForUser);

            return this.PartialView("_FolderBox", folders);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveFolder(string selectedId)
        {
            messageService.DeleteFolder(SessionManager.CurrentUsername, int.Parse(selectedId));

            return this.RedirectToAction("Index");
        }

        public ActionResult CreateFolder()
        {
            var mapper = AutoMapperConfiguration.GetMapper();
            var foldersForUser = messageService.GetAllFoldersForUser(SessionManager.CurrentUsername);

            var folder = new UserMessageFolder()
            {
                User = new UserAccount()
                {
                    Username = SessionManager.CurrentUsername
                },
            };

            var viewModel = mapper.Map<UserMessageFolder, MessageMailBoxFolderViewModel>(folder);
            viewModel.Folders = mapper.Map<List<UserMessageFolder>, List<MessageMailBoxFolderViewModel>>(foldersForUser);

            return this.View("EditFolder", viewModel);
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
    }
}