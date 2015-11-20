using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phobos
{
    public static class SessionManager
    {
        public static TemplateEnum TemplateName
        {

            get
            {
                return (TemplateEnum)HttpContext.Current.Session["template"];
            }

            set
            {
                HttpContext.Current.Session["template"] = value;
            }
        }

        public static UserAccountViewModel UserAccount
        {
            get
            {
                return (UserAccountViewModel)HttpContext.Current.Session["userAccount"];
            }

            set
            {
                HttpContext.Current.Session["userAccount"] = value;
            }
        }
    }
}