using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace UtilityExtensions
{
    [ConfigurationCollection(typeof(ConfigurationElement))]
    public class ConfigurationElementCollection<T> : ConfigurationElementCollection where T : ConfigurationElement, new()
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((T)(element)).ToString();
        }
        public T this[int idx]
        {
            get { return (T)BaseGet(idx); }
        }
    }
}
