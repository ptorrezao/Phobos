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
        bool SendMessage(string username, string msg);
        List<UserMessage> GetLastMessages(string userName, int qtd, bool orderDesc);
        List<UserMessageFolder> GetAllFoldersForUser(string userName);
        UserMessageFolder GetFolder(string userName, int? id);
        UserMessage SendMessage(string userName, UserMessage createdMessage);
        UserMessage SaveMessage(string userName, UserMessage newMessage);
        void DeleteMessage(string userName, int id);
        UserMessage GetMessage(string userName, int id);
        UserMessageFolder SaveFolder(string userName, UserMessageFolder model);
        void MoveMessageToFolder(string userName, int selectedInt, int newFolderIdInt);
        void DeleteFolder(string userName, int id);
    }
}
