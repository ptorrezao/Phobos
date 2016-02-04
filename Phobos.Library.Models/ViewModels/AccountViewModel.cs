using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models.ViewModels
{
    public class AccountViewModel
    {
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
