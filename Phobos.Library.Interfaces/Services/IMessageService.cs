using Phobos.Library.Models;
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
        List<UserMessage> GetLastMessages(string userName, int qtd, bool orderDesc);
        List<UserMessageFolder> GetAllFoldersForUser(string userName);
        UserMessageFolder GetFolder(string userName, int? id);
        UserMessage SendMessage(string p, UserMessage createdMessage);
        UserMessage SaveMessage(string p, UserMessage newMessage);
        void DeleteMessage(int p);
        UserMessage GetMessage(string userName, int id);
    }
}
