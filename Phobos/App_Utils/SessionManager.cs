using Phobos.Library.Interfaces;
using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Ninject;
using Phobos.Library.Models;
using Phobos.App_Utils;

namespace Phobos
{
    public static class SessionManager
    {
        public static TemplateEnum TemplateName
        {

            get
            {
                return (TemplateEnum)((HttpContext.Current.Session["template"]) ?? TemplateEnum.AdminLTE);
            }

            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Session["template"] = value;
                }
            }
        }

        public static UserAccountViewModel UserAccount
        {
            get
            {

                return (UserAccountViewModel)(HttpContext.Current.Session["userAccount"] ?? null);
            }

            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Session["userAccount"] = value;
                }
            }
        }

        public static string CurrentUsername
        {
            get
            {
                return SessionManager.UserAccount.Username;
            }
        }

        public static List<UserAccountViewModel> AllUsers
        {
            get
            {
                //// ToDo: Put in place a decent cache mecanism!!
                List<UserAccountViewModel> listOfUserViewModel = HttpRuntime.Cache.Get("listOfUsers") as List<UserAccountViewModel>;

                if (listOfUserViewModel == null || listOfUserViewModel.Count == 0)
                {
                    IUserManagementService usersDb = MvcApplication.GetKernel().Get<IUserManagementService>();
                    List<UserAccount> listOfUsers = usersDb.GetAllUsers();
                    listOfUserViewModel = AutoMapperConfiguration.GetMapper().Map<List<UserAccountViewModel>>(listOfUsers);
                    HttpRuntime.Cache.Insert("listOfUsers", listOfUserViewModel, null, DateTime.UtcNow.AddMinutes(60), TimeSpan.FromMinutes(0));
                }

                return listOfUserViewModel;
            }
        }
    }
}