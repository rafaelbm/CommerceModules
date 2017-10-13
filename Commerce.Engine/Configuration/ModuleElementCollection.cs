using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.Engine.Configuration
{
    [ConfigurationCollection(typeof(ModuleElement))]
    public class ModuleElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleElement)element).Name;
        }

        internal ModuleElement GetByName(string name)
        {
            return this.Cast<ModuleElement>().FirstOrDefault(item => item.Name == name);
        }

        internal ModuleElement this[int index] => (ModuleElement)this.BaseGet(index);
    }
}
