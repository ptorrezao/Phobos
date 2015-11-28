﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phobos.Library.Models;

namespace Phobos.Library.Interfaces.Services
{
    public interface IAuditTrailService
    {
        void LogMessage(string message, string user, object userAccount);
    }
}