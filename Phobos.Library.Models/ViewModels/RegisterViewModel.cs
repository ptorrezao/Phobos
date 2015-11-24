using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.Library.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string Password { get; set; }
        public string Name { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }
    }
}
