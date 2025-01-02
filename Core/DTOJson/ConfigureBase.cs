using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Acurus.Capella.Core.DTOJson
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

        public static void SaveJson(string fileName, string jsonText)
        {
            //try
            //{
            string IISServer1 = ConfigurationManager.AppSettings["IISServer1"];
            string IISServer2 = ConfigurationManager.AppSettings["IISServer2"];
            if (!string.IsNullOrEmpty(IISServer1))
            {
                string jsonFilePath = IISServer1 + fileName;
                if (!File.Exists(jsonFilePath))
                {
                    File.WriteAllText(jsonFilePath, jsonText);
                }
                else
                {
                    if (Directory.Exists(Path.Combine(IISServer1, "BK")))
                    {
                        File.Copy(jsonFilePath, Path.Combine(IISServer1, "BK", fileName), false);
                    }
                    string tempFilePath = IISServer1 + fileName + ".tmp";
                    File.WriteAllText(tempFilePath, jsonText);

                    File.Replace(tempFilePath, jsonFilePath, null);
                }
            }
            if (!string.IsNullOrEmpty(IISServer2))
            {
                string jsonFilePath = IISServer2 + fileName;
                if (!File.Exists(jsonFilePath))
                {
                    File.WriteAllText(jsonFilePath, jsonText);
                }
                else
                {
                    if (Directory.Exists(Path.Combine(IISServer2, "BK")))
                    {
                        File.Copy(jsonFilePath, Path.Combine(IISServer2, "BK", fileName), false);
                    }
                    string tempFilePath = IISServer2 + fileName + ".tmp";
                    File.WriteAllText(tempFilePath, jsonText);

                    File.Replace(tempFilePath, jsonFilePath, null);
                }
            }

            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine(fileName + " Exported");
            //Console.ForegroundColor = ConsoleColor.White;
            //}
            //catch (Exception ex)
            //{
            //    //Console.WriteLine(ex.Message);
            //}
        }
    }
}