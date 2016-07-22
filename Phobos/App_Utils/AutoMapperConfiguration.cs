using AutoMapper;
using Phobos.Library.Models;
using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Phobos.Library.Utils;
using System.Text;
using System.Security.Cryptography;

namespace Phobos.App_Utils
{
    public static class AutoMapperConfiguration
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                #region MessageMailBoxItemViewModel <-> UserMessage
                cfg.CreateMap<MessageMailBoxItemViewModel, UserMessage>()
                         .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.MessageId))
                         .ForMember(dest => dest.MessageDate, opts => opts.MapFrom(src => src.Date))
                         .ForMember(dest => dest.IsFavorite, opts => opts.MapFrom(src => src.IsFavorite))
                         .ForMember(dest => dest.Sender, opts => opts.MapFrom(src => AutoMapperConfiguration.GetMapper().Map<UserAccount>(src.Sender)))
                         .ForMember(dest => dest.Owner, opts => opts.MapFrom(src => AutoMapperConfiguration.GetMapper().Map<UserAccount>(src.Owner)))
                         .ForMember(dest => dest.Receiver, opts => opts.MapFrom(src => AutoMapperConfiguration.GetMapper().Map<UserAccount>(src.Receiver)));
                cfg.CreateMap<UserMessage, MessageMailBoxItemViewModel>()
                    .ForMember(dest => dest.MessageId, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.MessageDate))
                    .ForMember(dest => dest.Sender, opts => opts.MapFrom(src => AutoMapperConfiguration.GetMapper().Map<UserAccountViewModel>(src.Sender)))
                    .ForMember(dest => dest.Receiver, opts => opts.MapFrom(src => AutoMapperConfiguration.GetMapper().Map<UserAccountViewModel>(src.Receiver)))
                    .ForMember(dest => dest.Intro, opts => opts.MapFrom(src => src.Message.TruncateLongString(30, "...", true)));
                #endregion

                #region MessageMailBoxFolderViewModel <-> UserMessageFolder
                cfg.CreateMap<UserMessageFolder, MessageMailBoxFolderViewModel>()
                           .ForMember(dest => dest.QtdNewMessages, opts => opts.MapFrom(src => src.Messages.Count))
                           .ForMember(dest => dest.IsEditable, opts => opts.MapFrom(src => !src.IsDraftFolder && !src.IsInboxFolder && !src.IsSentFolder))
                           .ForMember(dest => dest.User, opts => opts.MapFrom(src => AutoMapperConfiguration.GetMapper().Map<UserAccount>(src.User)))
                           .ForMember(dest => dest.FolderId, opts => opts.MapFrom(src => src.Id));

                cfg.CreateMap<MessageMailBoxFolderViewModel, UserMessageFolder>()
                   .ForMember(dest => dest.User, opts => opts.MapFrom(src => AutoMapperConfiguration.GetMapper().Map<UserAccount>(src.User)))
                   .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.FolderId));
                #endregion

                cfg.CreateMap<UserMessageFolder, MessageMailBoxFolderItemViewModel>()
                    .ForMember(dest => dest.FolderId, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Name));

                cfg.CreateMap<UserMessage, UserMessageViewModel>()
                    .ForMember(dest => dest.Message, opts => opts.MapFrom(src => src.Message.TruncateLongString(30, "...", true)))
                    .ForMember(dest => dest.SentDate, opts => opts.MapFrom(src => src.SendDate))
                    .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title))
                    .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                    .ForMember(dest => dest.User, opts => opts.MapFrom(src => AutoMapperConfiguration.GetMapper().Map<UserAccountViewModel>(src.Receiver)));

                cfg.CreateMap<UserRole, UserRoleUpdateViewModel>()
                    .ForMember(dest => dest.OldName, opts => opts.MapFrom(src => src.Name))
                    .ForMember(dest => dest.AllUsers, opts => opts.MapFrom(src => AutoMapperConfiguration.GetMapper().Map<List<UserAccountRoleItemViewModel>>(SessionManager.AllUsers)))
                    .ForMember(dest => dest.Users, opts => opts.MapFrom(src => AutoMapperConfiguration.GetMapper().Map<List<UserAccountRoleItemViewModel>>(src.UserAccounts)))
                    .ForMember(dest => dest.IsAdmin, opts => opts.MapFrom(src => src.Name == "Administrator"));

                cfg.CreateMap<UserAccount, UserAccountRoleItemViewModel>()
                    .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.Username));

                cfg.CreateMap<UserAccountViewModel, UserAccountRoleItemViewModel>()
                    .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.Username));

                cfg.CreateMap<UserRole, UserRoleViewModel>()
                    .ForMember(dest => dest.IsAdmin, opts => opts.MapFrom(src => src.Name == "Administrator"));

                #region UserNotificationViewModel <-> UserNotification
                cfg.CreateMap<UserNotification, UserNotificationViewModel>()
                    .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title))
                    .ForMember(dest => dest.Color, opts => opts.MapFrom(src => src.IconColor))
                    .ForMember(dest => dest.Read, opts => opts.MapFrom(src => src.Read))
                    .ForMember(dest => dest.FontAwesome, opts => opts.MapFrom(src => " fa-" + src.Icon))
                    .ForMember(dest => dest.Link, opts => opts.MapFrom(src => src.Link));

                cfg.CreateMap<UserNotificationViewModel, UserNotification>()
                    .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title))
                    .ForMember(dest => dest.IconColor, opts => opts.MapFrom(src => src.Color))
                    .ForMember(dest => dest.Read, opts => opts.MapFrom(src => src.Read))
                    .ForMember(dest => dest.Icon, opts => opts.MapFrom(src => src.FontAwesome.Replace(" fa-", "")))
                    .ForMember(dest => dest.Link, opts => opts.MapFrom(src => src.Link));
                #endregion

                #region UserAccount <-> UserAccountViewModel
                cfg.CreateMap<UserAccount, UserAccountViewModel>()
                           .ForMember(dest => dest.CurrentStatus, opts => opts.MapFrom(src => src.CurrentStatus))
                           .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
                           .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
                           .ForMember(dest => dest.Roles, opts =>
                           {
                               opts.NullSubstitute(new List<UserRoleViewModel>());
                               opts.MapFrom(src => AutoMapperConfiguration.GetMapper().Map<List<UserRoleViewModel>>(src.Roles));
                           })
                           .ForMember(dest => dest.ImageAlt, opts => opts.MapFrom(src => string.Format("{0}{2}{1}", src.FirstName, src.LastName, (!string.IsNullOrEmpty(src.FirstName) && !string.IsNullOrEmpty(src.LastName) ? " " : ""))))
                           .ForMember(dest => dest.ImageUrl, opts =>
                           {
                               opts.NullSubstitute("/Content/themes/AdminLTE/img/user2-160x160.jpg");
                               opts.MapFrom(src => @"http://www.gravatar.com/avatar/" + src.Username.GetAsHash("") + ".jpg");
                           })
                           .ForMember(dest => dest.MemberSince, opts => opts.MapFrom(src => src.MemberSinceDate))
                           .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.Username))
                           .ForMember(dest => dest.Position, opts => opts.MapFrom(src => src.Position))
                           .ForMember(dest => dest.UseGravatar, opts => opts.MapFrom(src => true));

                cfg.CreateMap<UserAccountViewModel, UserAccount>()
                    .ForMember(dest => dest.MemberSinceDate, opts => opts.MapFrom(src => src.MemberSince));
                #endregion
            });
            return config.CreateMapper();
        }

    }
}