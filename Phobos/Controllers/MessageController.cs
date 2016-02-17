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
            Mapper.CreateMap<UserMessage, MessageMailBoxItemViewModel>()
                .ForMember(dest => dest.Sender, opts => opts.MapFrom(src => src.Sender.FirstName))
                .ForMember(dest => dest.Intro, opts => opts.MapFrom(src => src.Message)); 
            Mapper.CreateMap<UserMessageFolder, MessageMailBoxFolderViewModel>();
            Mapper.CreateMap<UserMessageFolder, MessageMailBoxFolderItemViewModel>();

            var foldersForUser = messageService.GetAllFoldersForUser(SessionManager.CurrentUsername);
            var currentFolder = messageService.GetFolder(SessionManager.CurrentUsername, id);

            var model = new MessageMailBoxViewModel()
            {
                CurrentFolder = Mapper.Map<UserMessageFolder, MessageMailBoxFolderViewModel>(currentFolder),
                Folders = Mapper.Map<List<UserMessageFolder>, List<MessageMailBoxFolderViewModel>>(foldersForUser)
            };

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