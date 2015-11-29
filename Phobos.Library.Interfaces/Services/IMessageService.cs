using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Interfaces.Services
{
    public interface IMessageService
    {
        bool SendMessage(string username, string v);
    }
}
