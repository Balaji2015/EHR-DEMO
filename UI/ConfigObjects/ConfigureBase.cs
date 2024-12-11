using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Acurus.Capella.UI
{
    public class ConfigureBase<T>
    {
        public static T ReadJson(string sFileName)
        {
            if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + sFileName))
            {
                var str = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + sFileName);
                return JsonConvert.DeserializeObject<T>(str);
            }
            return JsonConvert.DeserializeObject<T>(string.Empty);
        }
    }
}