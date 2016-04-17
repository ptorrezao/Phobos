using Ninject;
using Phobos.Library.Interfaces.Repos;
using Phobos.Library.Interfaces.Services;
using Phobos.Library.Models;
using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.CoreServices
{
    public class MessageCoreService : IMessageService
    {
        [Inject]
        public IMessageRepo Repository { get; set; }

        public bool SendMessage(string username, string v)
        {
            return false;
        }

        public List<UserMessage> GetLastMessages(string userName, int qtd, bool orderDesc)
        {
            return this.Repository.GetLastMessages(userName, qtd, orderDesc);
        }

        public List<UserMessageFolder> GetAllFoldersForUser(string userName)
        {
            return this.Repository.GetAllFolders(userName);
        }

        public UserMessageFolder GetFolder(string userName, int? id)
        {
            UserMessageFolder folder = this.Repository.GetFolder(userName, id ?? 0);

            if (folder == null && id == null)
            {
                folder = this.Repository.CreateDefaultFolder(userName);
            }

            folder.Messages = this.Repository.GetMessages(userName, folder.Id);
            return folder;
        }
    }
}
