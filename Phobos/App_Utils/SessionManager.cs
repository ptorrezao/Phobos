using Phobos.Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

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


    }
}