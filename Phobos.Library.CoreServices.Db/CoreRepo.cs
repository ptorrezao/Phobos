using Phobos.Library.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phobos.Library.Models;

namespace Phobos.Library.CoreServices.Db
{
    public class CoreRepo : ICoreRepo
    {
        public void AddConfiguration(string key, string value)
        {
            using (var context = new PhobosCoreContext())
            {
                var config = context.Configurations.FirstOrDefault(x => x.Key == key);
                if (config == default(Configuration))
                {
                    context.Configurations.Add(new Configuration() { Key = key, Value = value });
                    context.SaveChanges();
                }
            }
        }

        public Configuration GetConfiguration(string key)
        {
            using (var context = new PhobosCoreContext())
            {
                var config = context.Configurations.FirstOrDefault(x => x.Key == key);
                if (config == default(Configuration))
                {
                    throw new Exception("Configuration (" + key + ") is not set.");
                }

                return config;
            }
        }
    }
}
