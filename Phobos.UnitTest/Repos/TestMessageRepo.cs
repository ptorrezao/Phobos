using Phobos.Library.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phobos.UnitTest.Repos
{
    public class TestMessageRepo : IMessageRepo
    {
        public List<Library.Models.UserMessage> GetLastMessages(string userName, int qtd)
        {
            throw new NotImplementedException();
        }

        public List<Library.Models.UserMessageFolder> GetAllFolders(string userName)
        {
            throw new NotImplementedException();
        }

        public Library.Models.UserMessageFolder GetFolder(string userName, int folderId)
        {
            throw new NotImplementedException();
        }

        public Library.Models.UserMessageFolder GetInboxFolder(string userName)
        {
            throw new NotImplementedException();
        }

        public Library.Models.UserMessageFolder GetSentFolder(string userName)
        {
            throw new NotImplementedException();
        }

        public List<Library.Models.UserMessage> GetMessages(string userName, int folderId)
        {
            throw new NotImplementedException();
        }

        public Library.Models.UserMessageFolder CreateDefaultFolder(string userName)
        {
            throw new NotImplementedException();
        }

        public Library.Models.UserMessage SaveMessage(Library.Models.UserMessage sentMessage)
        {
            throw new NotImplementedException();
        }

        public void DeleteMessage(int messageId)
        {
            throw new NotImplementedException();
        }

        public Library.Models.UserMessage GetMessage(string userName, int id)
        {
            throw new NotImplementedException();
        }

        public Library.Models.UserMessageFolder SaveFolder(Library.Models.UserMessageFolder model)
        {
            throw new NotImplementedException();
        }

        public void MoveMessageToFolder(string userName, int msgId, int newFolderId)
        {
            throw new NotImplementedException();
        }

        public void DeleteFolder(string userName, int id)
        {
            throw new NotImplementedException();
        }
    }
}
