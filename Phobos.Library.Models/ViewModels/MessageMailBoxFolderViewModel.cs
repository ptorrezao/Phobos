using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phobos.Library.Models.ViewModels
{
    public class MessageMailBoxFolderViewModel
    {
        public int QtdNewMessages { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int FolderId { get; set; }
        public bool Selected { get; set; }
        public TextColor IconColor { get; set; }
        public List<MessageMailBoxItemViewModel> Messages { get; set; }
    }
}
