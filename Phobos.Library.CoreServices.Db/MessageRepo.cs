using Phobos.Library.Interfaces.Repos;
using Phobos.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.CoreServices.Db
{
    public class MessageRepo : IMessageRepo
    {
        public List<UserMessage> GetLastMessages(string userName, int qtd, bool orderDesc)
        {
            throw new NotImplementedException();
        }

        public List<UserMessageFolder> GetAllFolders(string userName)
        {
            throw new NotImplementedException();
        }

        public UserMessageFolder GetFolder(string userName, int folderId)
        {
            throw new NotImplementedException();
        }

        public List<UserMessage> GetMessages(string userName, int folderId)
        {
            throw new NotImplementedException();
        }
    }
}
