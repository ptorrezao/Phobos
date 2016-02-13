using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models.ViewModels
{
    public class MessageMailBoxViewModel
    {
        public MessageMailBoxViewModel()
        {
            Folders = new List<MessageMailBoxFolderViewModel>();
            Tags = new List<MessageMailBoxTagsViewModel>();
        }

        public MessageMailBoxFolderViewModel CurrentFolder { get; set; }
   
        public List<MessageMailBoxFolderViewModel> Folders { get; set; }

        public List<MessageMailBoxTagsViewModel> Tags { get; set; }
    }
}
