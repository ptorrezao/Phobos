using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
