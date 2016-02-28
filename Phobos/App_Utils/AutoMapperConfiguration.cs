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
                cfg.CreateMap<UserMessage, MessageMailBoxItemViewModel>()
                    .ForMember(dest => dest.Sender, opts => opts.MapFrom(src => src.Sender.FirstName))
                    .ForMember(dest => dest.Intro, opts => opts.MapFrom(src => src.Message.TruncateLongString(30, "...")));
                cfg.CreateMap<UserMessageFolder, MessageMailBoxFolderViewModel>();
                cfg.CreateMap<UserMessageFolder, MessageMailBoxFolderItemViewModel>();
                cfg.CreateMap<UserMessage, UserMessageViewModel>()
                    .ForMember(dest => dest.Message, opts => opts.MapFrom(src => src.Message.TruncateLongString(30, "...")))
                    .ForMember(dest => dest.SentDate, opts => opts.MapFrom(src => src.SendDate))
                    .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title))
                    .ForMember(dest => dest.User, opts => opts.MapFrom(src => AutoMapperConfiguration.GetMapper().Map<UserAccountViewModel>(src.Receiver)));
                cfg.CreateMap<UserAccount, UserAccountViewModel>()
                    .ForMember(dest => dest.CurrentStatus, opts => opts.MapFrom(src => src.CurrentStatus))
                    .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
                    .ForMember(dest => dest.ImageAlt, opts => opts.MapFrom(src => string.Format("{0} {1}", src.FirstName, src.LastName)))
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

            });
            return config.CreateMapper();
        }
    }
}