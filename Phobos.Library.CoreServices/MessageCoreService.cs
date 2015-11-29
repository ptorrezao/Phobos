using Phobos.Library.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.CoreServices
{
    public class MessageCoreService : IMessageService
    {
        public bool SendMessage(string username, string msg)
        {
            return true;
        }
    }
}
