using Phobos.Library.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.CoreServices
{
    public class AuditTrailCoreService : IAuditTrailService
    {
        public void LogInfoMessage(string logMessage, string userName, DateTime now)
        {
        }

        public void LogMessage(string message, string user, object userAccount)
        {
        }

        public void LogWarningMessage(string msg, string userName, DateTime now)
        {
        }
    }
}
