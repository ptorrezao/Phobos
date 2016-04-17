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
                    .Where(x => x.User.Username == userName)
                    .FirstOrDefault();

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
                    .Where(x => x.Receiver.Username == userName && x.Folder.Id == x.Folder.Id)
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
                    Name = "Inbox",
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
                context.UserMessages.Add(sentMessage);
                context.SaveChanges();

                return sentMessage;
            }
        }


        public void DeleteMessage(int messageId)
        {
            using (var context = new PhobosCoreContext())
            {
                var msg = context.UserMessages.First(x => x.Id== messageId);

                context.UserMessages.Remove(msg);

                context.SaveChanges();
            }
        }
    }
}
