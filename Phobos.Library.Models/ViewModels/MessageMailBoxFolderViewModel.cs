﻿using Phobos.Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Phobos.Library.Models.ViewModels
{
    public class MessageMailBoxFolderViewModel
    {
        public MessageMailBoxFolderViewModel()
        {
            Folders = new List<MessageMailBoxFolderViewModel>();
            Messages = new List<MessageMailBoxItemViewModel>();
        }

        public int QtdNewMessages { get; set; }
        public string Name { get; set; }
        [UIHint("FontAwesomeIcon")]
        public string Icon { get; set; }
        public int FolderId { get; set; }
        public bool Selected { get; set; }
        [DisplayName("Icon Color")]
        public TextColor IconColor { get; set; }
        public List<MessageMailBoxItemViewModel> Messages { get; set; }
        public int NewFolderIdForMessages { get; set; }
        public List<MessageMailBoxFolderViewModel> Folders { get; set; }
        public bool IsEditable { get; set; }
        public UserAccountViewModel User { get; set; }
    }
}
