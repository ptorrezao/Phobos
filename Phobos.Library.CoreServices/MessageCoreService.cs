using Ninject;
using Phobos.Library.Interfaces;
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
                folder = this.Repository.GetInboxFolder(userName);
            }

            folder.Messages = this.Repository.GetMessages(userName, folder.Id);
            return folder;
        }

        public UserMessage SendMessage(string userName, UserMessage createdMessage)
        {
            UserMessageFolder sentFolder = this.Repository.GetSentFolder(userName);
            
            if (createdMessage.Sender.Username == userName)
            {
                //// Create a new instance of this message and send it;
                UserMessage sentMessage = new UserMessage()
                {
                    Attachments = createdMessage.Attachments,
                    Folder = createdMessage.Folder,
                    HasAttachment = createdMessage.HasAttachment,
                    IsFavorite = createdMessage.IsFavorite,
                    Message = createdMessage.Message,
                    Title = createdMessage.Title,
                    MessageDate = DateTime.Now,
                    SendDate = DateTime.Now,
                    Owner = new UserAccount() { Username = createdMessage.Receiver.Username },
                    Receiver = new UserAccount() { Username = createdMessage.Receiver.Username },
                    Sender = new UserAccount() { Username = createdMessage.Owner.Username },
                    Sent = true
                };
                this.Repository.SaveMessage(sentMessage);

                //// Mark the current message as Sent.
                createdMessage.Sent = true;
                createdMessage.SendDate = DateTime.Now;
                createdMessage.Folder = sentFolder;
                this.Repository.SaveMessage(createdMessage);
            }

            return createdMessage;
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


        public UserMessage GetMessage(string userName, int id)
        {
            return this.Repository.GetMessage(userName, id);
        }
    }
}
