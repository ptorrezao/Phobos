using Phobos.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Interfaces.Repos
{
    public interface IMessageRepo
    {
        List<UserMessage> GetLastMessages(string userName, int qtd, bool orderDesc);
        List<UserMessageFolder> GetAllFolders(string userName);
        UserMessageFolder GetFolder(string userName, int folderId);
        List<UserMessage> GetMessages(string userName, int folderId);
        UserMessageFolder CreateDefaultFolder(string userName);
        UserMessage SaveMessage(UserMessage sentMessage);

        void DeleteMessage(int messageId);
    }
}
