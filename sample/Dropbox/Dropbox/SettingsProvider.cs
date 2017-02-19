using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Dropbox
{
    public static class SettingsProvider
    {
        public static IDictionary<string, string> GetSettings()
        {
            return ConfigurationManager.AppSettings.ToDictionary();
        }

        public static string GetValue(string key)
        {
            string value;
            return GetSettings().TryGetValue(key, out value) ? value : null;
        }

        private static IDictionary<string, string> ToDictionary(this NameValueCollection col)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var k in col.AllKeys)
            {
                dict.Add(k, col[k]);
            }
            return dict;
        }
    }
}
