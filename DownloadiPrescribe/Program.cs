using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Xml;
using Acurus.Capella.DataAccess;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Net.Http;
using Acurus.Capella.DataAccess;
using System.Threading.Tasks;
using NHibernate;
using System.Collections;
using static Acurus.Capella.DataAccess.RCopiaXMLResponseProcess;
using System.Text.RegularExpressions;
using System.Configuration;

namespace DownloadiPrescribe
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Execution Started...");
                DateTime dtRCopia_Last_Updated_Date_Time = DateTime.MinValue;
                string sLegalOrgListConfig = ConfigurationManager.AppSettings["sLegalOrgList"];
                List<string> sLegalOrgList = new List<string>(sLegalOrgListConfig.Split(','));
                Rcopia_Update_InfoManager rcopiaUpdateMngr = new Rcopia_Update_InfoManager();
                CultureInfo culture = new CultureInfo("en-US");
                foreach (var sLegalOrg in sLegalOrgList)
                {
                    Console.WriteLine($"Running for legal org {sLegalOrg}");
                    DateTime.TryParse(rcopiaUpdateMngr.GetRcopiaUpdateInfoCommandNameAndLegalOrg("get_rcopia_event", sLegalOrg), out dtRCopia_Last_Updated_Date_Time);
                    RCopiaGenerateXML rcopiaXML = new RCopiaGenerateXML();
                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
                    RCopiaXMLResponseProcess rcopiaResponseXML = new RCopiaXMLResponseProcess();
                    Console.WriteLine("Calling iPrescribe API...");
                    string sInputXML = rcopiaXML.CreateGetRcopiaEventXML(dtRCopia_Last_Updated_Date_Time, sLegalOrg);
                    string sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.DownloadAddress + sInputXML, 1);
                    EventXMLResponseModel responseModel = rcopiaResponseXML.ReadEventXMLResponse(sOutputXML, sLegalOrg);              
                    foreach (var patientId in responseModel.ilstPatientIds)
                    {
                        Console.WriteLine($"Calling DownloadRCopiaInfo method for Human_Id {patientId}");

                        HumanManager humanManager = new HumanManager();
                        var human = humanManager.GetHumanFromHumanID(Convert.ToUInt64(patientId));
                        if (human != null && human.Id != 0)
                        {
                            try
                            {
                                Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
                                objUpdateInfoMngr.DownloadRCopiaInfo(string.Empty, "Acurus", string.Empty, DateTime.Now, string.Empty, 0, Convert.ToUInt64(patientId), sLegalOrg);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Something went wrong :- " + ex.Message);
                            }
                        }
                    }

                    Console.WriteLine($"Update LastUpdatedDate in Rcopia_Update_info table for legal org {sLegalOrg}");
                    rcopiaUpdateMngr.InsertinToRcopia_Update_info("get_rcopia_event", Convert.ToDateTime(responseModel.dtLastUpdateDate, culture), string.Empty, string.Empty, sLegalOrg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong :- " + ex.Message);
            }
        }
    }
}
