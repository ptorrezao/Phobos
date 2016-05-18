using System;
using System.Collections.Generic;
using System.Linq;
using Phobos.Library.Interfaces.Repos;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Phobos.Library.Models;
using System.Data.Entity;

namespace Phobos.Library.CoreServices.Db
{
    public class MessageRepo : IMessageRepo
    {
        const string inboxFolderName = "Inbox";
        const string sentFolderName = "Sent";

        public List<UserMessage> GetLastMessages(string userName, int qtd, bool orderDesc)
        {
            using (var context = new PhobosCoreContext())
            {
                return context.UserMessages
                    .Include(x => x.Receiver)
                    .Include(x => x.Sender)
                    .Include(x => x.Folder)
                    .Where(x => x.Receiver.Username == userName).OrderByDescending(x => x.SendDate).Take(qtd).ToList();
            }
        }

        public List<UserMessageFolder> GetAllFolders(string userName)
        {
            using (var context = new PhobosCoreContext())
            {
                ////Create Inbox if necessary
                var inboxFolder = this.GetInboxFolder(userName);

                ////Create Sent if necessary
                var sentFolder = this.GetSentFolder(userName);

                return context.UserMessageFolders
                    .Include(x => x.User)
                    .Where(x => x.User.Username == userName).ToList();
            }
        }

        public UserMessageFolder GetFolder(string userName, int folderId)
        {
            using (var context = new PhobosCoreContext())
            {
                var folder = context.UserMessageFolders
                    .Include(x => x.User)
                    .Where(x => x.User.Username == userName && x.Id == folderId)
                    .FirstOrDefault();

                return folder;
            }
        }

        public UserMessageFolder GetInboxFolder(string userName)
        {
            using (var context = new PhobosCoreContext())
            {
                var folder = context.UserMessageFolders
                    .Include(x => x.User)
                    .Where(x => x.User.Username == userName && x.Name == inboxFolderName)
                    .FirstOrDefault();

                if (folder == default(UserMessageFolder))
                {
                    folder = new UserMessageFolder()
                    {
                        User = context.Users.First(x => x.Username == userName),
                        Name = inboxFolderName,
                    };

                    context.UserMessageFolders.Add(folder);
                    context.SaveChanges();
                }

                return folder;
            }
        }

        public UserMessageFolder GetSentFolder(string userName)
        {
            using (var context = new PhobosCoreContext())
            {
                var folder = context.UserMessageFolders
                    .Include(x => x.User)
                    .Where(x => x.User.Username == userName && x.Name == sentFolderName)
                    .FirstOrDefault();


                if (folder == default(UserMessageFolder))
                {
                    folder = new UserMessageFolder()
                    {
                        User = context.Users.First(x => x.Username == userName),
                        Name = sentFolderName,
                    };

                    context.UserMessageFolders.Add(folder);
                    context.SaveChanges();
                }

                return folder;
            }
        }

        public List<UserMessage> GetMessages(string userName, int folderId)
        {
            using (var context = new PhobosCoreContext())
            {
                return context.UserMessages
                    .Include(x => x.Receiver)
                    .Include(x => x.Sender)
                    .Include(x => x.Folder)
                    .Include(x => x.Owner)
                    .Where(x => x.Receiver.Username == userName &&
                                x.Folder.Id == folderId)
                    .OrderByDescending(x => x.SendDate)
                    .ToList();
            }
        }

        public UserMessageFolder CreateDefaultFolder(string userName)
        {
            using (var context = new PhobosCoreContext())
            {
                var folder = new UserMessageFolder()
                {
                    User = context.Users.First(x => x.Username == userName),
                    Name = inboxFolderName,
                };

                context.UserMessageFolders.Add(folder);

                context.SaveChanges();

                return folder;
            }
        }

        public UserMessage SaveMessage(UserMessage sentMessage)
        {
            using (var context = new PhobosCoreContext())
            {
                sentMessage.Owner = context.Users.First(x => x.Username == sentMessage.Owner.Username);
                sentMessage.Sender = context.Users.First(x => x.Username == sentMessage.Sender.Username);
                sentMessage.Receiver = context.Users.First(x => x.Username == sentMessage.Receiver.Username);
                sentMessage.Folder = sentMessage.Folder == null ? context.UserMessageFolders.FirstOrDefault(x => x.User.Username == sentMessage.Receiver.Username && x.Name == inboxFolderName) : context.UserMessageFolders.First(x => x.Id == sentMessage.Folder.Id);
                context.UserMessages.Add(sentMessage);
                context.SaveChanges();

                return sentMessage;
            }
        }

        public void DeleteMessage(int messageId)
        {
            using (var context = new PhobosCoreContext())
            {
                var msg = context.UserMessages.First(x => x.Id == messageId);

                context.UserMessages.Remove(msg);

                context.SaveChanges();
            }
        }

        public UserMessage GetMessage(string userName, int id)
        {
            using (var context = new PhobosCoreContext())
            {
                return context.UserMessages
                    .Include(x => x.Receiver)
                    .Include(x => x.Sender)
                    .Include(x => x.Folder)
                    .Include(x => x.Owner)
                    .Where(x => x.Receiver.Username == userName &&
                                x.Folder.Id == x.Folder.Id &&
                                x.Id == id)
                    .OrderByDescending(x => x.SendDate)
                    .FirstOrDefault();
            }
        }


    }
}
