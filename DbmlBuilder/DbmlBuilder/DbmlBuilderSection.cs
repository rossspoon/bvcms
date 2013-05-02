using System.Configuration;

namespace DbmlBuilder 
{
    public class DbmlBuilderSection : ConfigurationSection 
    {
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers 
        {
            get { return (ProviderSettingsCollection)base["providers"]; }
        }

        [ConfigurationProperty("defaultProvider", DefaultValue = "SqlDataProvider")]
        public string DefaultProvider 
        {
            get { return (string)base["defaultProvider"]; }
            set { base["defaultProvider"] = value; }
        }
    }
}
