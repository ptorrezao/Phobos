﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Phobos.Library.Models.ViewModels
{
    public class UserRoleViewModel
    {
        [Display(Name = "Group Name")]
        public string Name { get; set; }

        public bool IsAdmin { get; set; }
    }

}
