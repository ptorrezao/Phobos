using Phobos.Library.Interfaces.Repos;
using Phobos.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phobos.UnitTest.Repos
{
    public class TestCoreRepo : ICoreRepo
    {
        static List<Configuration> context = new List<Configuration>();
        public Configuration GetConfiguration(string key)
        {
            return context.FirstOrDefault(x => x.Key == key);
        }

        public void AddConfiguration(string key, string value)
        {
            if (context.Any(x => x.Key == key))
            {
                context.FirstOrDefault(x => x.Key == key).Value = value;
            }
            else
            {
                context.Add(new Configuration()
                {
                    Key = key,
                    Value = value,
                });
            }
        }
    }
}
