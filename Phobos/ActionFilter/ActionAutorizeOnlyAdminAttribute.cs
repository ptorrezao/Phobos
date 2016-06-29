using Ninject;
using Phobos.Library.Interfaces;
using Phobos.Library.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Phobos.ActionFilter
{
    public class ActionAutorizeOnlyAdminAttribute : ActionAutorizeAttribute
    {
        public ActionAutorizeOnlyAdminAttribute()
            : base(true)
        { }
    }
}