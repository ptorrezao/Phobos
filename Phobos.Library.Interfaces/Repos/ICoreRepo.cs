using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phobos.Library.Models;

namespace Phobos.Library.Interfaces.Repos
{
    public interface ICoreRepo
    {
        Configuration GetConfiguration(string key);
        void AddConfiguration(string key, string value);
    }
}
