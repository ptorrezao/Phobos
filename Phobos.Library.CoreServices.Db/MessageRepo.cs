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
        public const string InboxFolderName = "Inbox";
        public const string SentFolderName = "Sent";
        public const string DraftFolderName = "Draft";
     
        public List<UserMessage> GetLastMessages(string userName, int qtd, bool orderDesc)
        {
            using (var context = new PhobosCoreContext())
            {
                return context.UserMessages
                    .Include(x => x.Receiver)
                    .Include(x => x.Receiver.Roles)
                    .Include(x => x.Sender)
                    .Include(x => x.Sender.Roles)
                    .Include(x => x.Folder)
                    .Where(x => x.Receiver.Username == userName && x.IsDraft == false && x.Folder.IsInboxFolder)
                    .OrderByDescending(x => x.SendDate)
                    .Take(qtd)
                    .ToList();
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

                ////Create Draft if necessary
                var draftFolder = this.GetDraftFolder(userName);

                return context.UserMessageFolders
                    .Include(x => x.User)
                    .Include(x => x.User.Roles)
                    .Include(x => x.Messages)
                    .Include(x => x.Messages.Select(c => c.Sender))
                    .Include(x => x.Messages.Select(c => c.Sender.Roles))
                    .Include(x => x.Messages.Select(c => c.Receiver))
                    .Include(x => x.Messages.Select(c => c.Receiver.Roles))
                    .Include(x => x.Messages.Select(c => c.Owner))
                    .Include(x => x.Messages.Select(c => c.Owner.Roles))
                    .Where(x => x.User.Username == userName).ToList();
            }
        }

        public UserMessageFolder GetFolder(string userName, int folderId)
        {
            using (var context = new PhobosCoreContext())
            {
                var folder = context.UserMessageFolders
                    .Include(x => x.User)
                    .Include(x => x.User.Roles)
                    .Where(x => x.User.Username == userName && (x.Id == folderId || (folderId == 0 && x.IsInboxFolder)))
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
                    .Include(x => x.User.Roles)
                    .Where(x => x.User.Username == userName && x.Name == InboxFolderName)
                    .FirstOrDefault();

                if (folder == default(UserMessageFolder))
                {
                    folder = new UserMessageFolder()
                    {
                        User = context.Users.First(x => x.Username == userName),
                        Name = InboxFolderName,
                        IsInboxFolder = true
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
                    .Include(x => x.User.Roles)
                    .Where(x => x.User.Username == userName && x.Name == SentFolderName)
                    .FirstOrDefault();


                if (folder == default(UserMessageFolder))
                {
                    folder = new UserMessageFolder()
                    {
                        User = context.Users.First(x => x.Username == userName),
                        Name = SentFolderName,
                        IsSentFolder = true
                    };

                    context.UserMessageFolders.Add(folder);
                    context.SaveChanges();
                }

                return folder;
            }
        }

        public UserMessageFolder GetDraftFolder(string userName)
        {
            using (var context = new PhobosCoreContext())
            {
                var folder = context.UserMessageFolders
                    .Include(x => x.User)
                    .Include(x => x.User.Roles)
                    .Where(x => x.User.Username == userName && x.Name == DraftFolderName)
                    .FirstOrDefault();


                if (folder == default(UserMessageFolder))
                {
                    folder = new UserMessageFolder()
                    {
                        User = context.Users.First(x => x.Username == userName),
                        Name = DraftFolderName,
                        IsDraftFolder = true
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
                var messages = context.UserMessages
                                    .Include(x => x.Receiver)
                                    .Include(x => x.Receiver.Roles)
                                    .Include(x => x.Sender)
                                    .Include(x => x.Sender.Roles)
                                    .Include(x => x.Folder)
                                    .Include(x => x.Owner)
                                    .Where(x => (x.Receiver.Username == userName && x.Folder.Id == folderId) ||
                                                (x.Sender.Username == userName && x.Folder.Id == folderId && (x.Folder.Name == SentFolderName || x.Folder.Name == DraftFolderName)))
                                    .OrderByDescending(x => x.SendDate)
                                    .ToList();

                messages.ForEach(msg => msg.IsDraft = msg.Folder.Name == DraftFolderName);

                return messages;
            }
        }

        public UserMessageFolder CreateDefaultFolder(string userName)
        {
            using (var context = new PhobosCoreContext())
            {
                var folder = new UserMessageFolder()
                {
                    User = context.Users.Include(x => x.Roles).First(x => x.Username == userName),
                    Name = InboxFolderName,
                    IsInboxFolder = true
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
                sentMessage.Owner = context.Users.Include(x => x.Roles).First(x => x.Username == sentMessage.Owner.Username);
                sentMessage.Sender = context.Users.Include(x => x.Roles).First(x => x.Username == sentMessage.Sender.Username);
                sentMessage.Receiver = context.Users.Include(x => x.Roles).First(x => x.Username == sentMessage.Receiver.Username);

                if (sentMessage.Folder == null && sentMessage.IsDraft)
                {
                    sentMessage.Folder = context.UserMessageFolders.FirstOrDefault(x => x.User.Username == sentMessage.Owner.Username && x.Name == DraftFolderName);
                    sentMessage.IsDraft = sentMessage.Folder.Name == DraftFolderName;
                }
                else if (sentMessage.Folder == null && !sentMessage.IsDraft)
                {
                    sentMessage.Folder = context.UserMessageFolders.FirstOrDefault(x => x.User.Username == sentMessage.Receiver.Username && x.Name == InboxFolderName);
                }
                else if (sentMessage.Folder != null)
                {
                    sentMessage.Folder = context.UserMessageFolders.FirstOrDefault(x => x.Id == sentMessage.Folder.Id);
                }

                if (sentMessage.Id == 0)
                {
                    context.UserMessages.Add(sentMessage);
                }
                else
                {
                    context.UserMessages.First(x => x.Id == sentMessage.Id).Message = sentMessage.Message;
                    context.UserMessages.First(x => x.Id == sentMessage.Id).Title = sentMessage.Title;
                    context.UserMessages.First(x => x.Id == sentMessage.Id).Receiver = sentMessage.Receiver;
                    context.UserMessages.First(x => x.Id == sentMessage.Id).Sent = sentMessage.Sent;
                    context.UserMessages.First(x => x.Id == sentMessage.Id).IsFavorite = sentMessage.IsFavorite;
                }

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
                var message = context.UserMessages
                    .Include(x => x.Receiver)
                    .Include(x => x.Receiver.Roles)
                    .Include(x => x.Sender)
                    .Include(x => x.Sender.Roles)
                    .Include(x => x.Folder)
                    .Include(x => x.Owner)
                    .Where(x => x.Receiver.Username == userName &&
                                x.Folder.Id == x.Folder.Id &&
                                x.Id == id)
                    .OrderByDescending(x => x.SendDate)
                    .FirstOrDefault();

                if (message != null)
                {
                    message.IsDraft = message.Folder.Name == DraftFolderName;
                }

                return message;
            }
        }

        public UserMessageFolder SaveFolder(UserMessageFolder model)
        {
            using (var context = new PhobosCoreContext())
            {
                model.User = context.Users.Include(x => x.Roles).First(x => x.Username == model.User.Username);

                if (model.Id == 0 || !context.UserMessageFolders.Any(x => x.Id == model.Id))
                {
                    context.UserMessageFolders.Add(model);
                }
                else
                {
                    context.UserMessageFolders.First(x => x.Id == model.Id).Icon = model.Icon;
                    context.UserMessageFolders.First(x => x.Id == model.Id).IconColor = model.IconColor;
                    context.UserMessageFolders.First(x => x.Id == model.Id).Name = model.Name;
                }

                context.SaveChanges();

                return model;
            }
        }

        public void MoveMessageToFolder(string userName, int msgId, int newFolderId)
        {
            using (var context = new PhobosCoreContext())
            {
                var msg = context.UserMessages.First(x => x.Id == msgId && x.Owner.Username == userName);
                var newFolder = context.UserMessageFolders.Where(x => x.Id == newFolderId && x.User.Username == userName).First();

                msg.Folder = newFolder;
                context.SaveChanges();
            }
        }

        public void DeleteFolder(string userName, int id)
        {
            using (var context = new PhobosCoreContext())
            {
                var folder = context.UserMessageFolders.FirstOrDefault(x => x.Id == id && x.User.Username == userName);

                if (folder != null)
                {
                    context.UserMessageFolders.Remove(folder);

                    context.SaveChanges();
                }
            }
        }
    }
}
