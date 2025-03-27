using Acurus.Capella.Core.DomainObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Acurus.Capella.Core.DTOJson
{
    public class ConfigureBase<T>
    {
        public static T ReadJson(string sFileName)
        {
            if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + sFileName))
            {
                //CAP-2945
                //var str = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + sFileName);
                //var str = string.Empty;
                using (FileStream fs = File.OpenRead(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + sFileName))
                {
                    //using (StreamReader reader = new StreamReader(fs))
                    //{
                    //    var stringBuilder = new StringBuilder();
                    //    char[] buffer = new char[16384];
                    //    int bytesRead;
                    //    while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                    //    {
                    //        stringBuilder.Append(buffer, 0, bytesRead);
                    //    }
                    //    str = stringBuilder.ToString();
                    //}


                    using (StreamReader reader = new StreamReader(fs))
                    using (JsonTextReader jsonReader = new JsonTextReader(reader))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        return serializer.Deserialize<T>(jsonReader);
                    }
                }
                //return JsonConvert.DeserializeObject<T>(str);
            }
            return JsonConvert.DeserializeObject<T>(string.Empty);
        }

        //Cap - 3094, 3103
        //public static void SaveJson(string fileName, string jsonText)
        //{
        //    //try
        //    //{
        //    string IISServer1 = ConfigurationManager.AppSettings["IISServer1"];
        //    string IISServer2 = ConfigurationManager.AppSettings["IISServer2"];
        //    if (!string.IsNullOrEmpty(IISServer1))
        //    {
        //        string jsonFilePath = IISServer1 + fileName;
        //        if (!File.Exists(jsonFilePath))
        //        {
        //            File.WriteAllText(jsonFilePath, jsonText);
        //        }
        //        else
        //        {
        //            if (Directory.Exists(Path.Combine(IISServer1, "BK")))
        //            {
        //                File.Copy(jsonFilePath, Path.Combine(IISServer1, "BK", fileName), false);
        //            }
        //            string tempFilePath = IISServer1 + fileName + ".tmp";
        //            File.WriteAllText(tempFilePath, jsonText);

        //            File.Replace(tempFilePath, jsonFilePath, null);
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(IISServer2))
        //    {
        //        string jsonFilePath = IISServer2 + fileName;
        //        if (!File.Exists(jsonFilePath))
        //        {
        //            File.WriteAllText(jsonFilePath, jsonText);
        //        }
        //        else
        //        {
        //            if (Directory.Exists(Path.Combine(IISServer2, "BK")))
        //            {
        //                File.Copy(jsonFilePath, Path.Combine(IISServer2, "BK", fileName), false);
        //            }
        //            string tempFilePath = IISServer2 + fileName + ".tmp";
        //            File.WriteAllText(tempFilePath, jsonText);

        //            File.Replace(tempFilePath, jsonFilePath, null);
        //        }
        //    }

        //    //Console.ForegroundColor = ConsoleColor.Red;
        //    //Console.WriteLine(fileName + " Exported");
        //    //Console.ForegroundColor = ConsoleColor.White;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    //Console.WriteLine(ex.Message);
        //    //}
        //}
    }
}