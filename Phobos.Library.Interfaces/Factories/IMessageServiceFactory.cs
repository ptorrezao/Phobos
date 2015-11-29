using Phobos.Library.Interfaces.Services;
using Phobos.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Interfaces.Factories
{
    public interface IMessageServiceFactory
    {
        void SendMessageTroughtAllService(Message msg);

        List<IMessageService> GetService();
    }
}
