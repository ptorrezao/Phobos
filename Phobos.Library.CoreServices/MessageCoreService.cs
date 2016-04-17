using Ninject;
using Phobos.Library.Interfaces.Repos;
using Phobos.Library.Interfaces.Services;
using Phobos.Library.Models;
using Phobos.Library.Models.Enums;
using Phobos.Library.Utils;
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

        public void SendMessage(string userName, UserMessage createdMessage)
        {
            if (createdMessage.Sender.Username == userName)
            {
                //// Create a new instance of this message and send it;
                var sentMessage = ObjectCopier.Clone<UserMessage>(createdMessage);
                sentMessage.Id = 0;
                sentMessage.Owner = sentMessage.Receiver;
                sentMessage.SendDate = DateTime.Now;
                this.Repository.SaveMessage(sentMessage);

                //// Mark the current message as Sent.
                createdMessage.Sent = true;
                createdMessage.SendDate = DateTime.Now;

                this.Repository.SaveMessage(createdMessage);
            }
        }

        public UserMessage SaveMessage(string userName, UserMessage newMessage)
        {
            if (newMessage.Sender.Username == userName)
            {
                return this.Repository.SaveMessage(newMessage);
            }

            return null;
        }

        public void DeleteMessage(int messageId)
        {
             this.Repository.DeleteMessage(messageId);
        }
    }
}
