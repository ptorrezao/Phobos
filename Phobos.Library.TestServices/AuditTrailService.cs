using Phobos.Library.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.TestServices
{
    public class AuditTrailService : IAuditTrailService
    {
        public void LogMessage(string message, string user, object userAccount)
        {
            
        }
    }
}
