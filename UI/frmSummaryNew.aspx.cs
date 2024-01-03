using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using HTMLtoPDF;
using System.Net;
using Acurus.Capella.Core.DomainObjects;
using Telerik.Web;
using Telerik.Web.UI;

using Acurus.Capella.DataAccess.ManagerObjects;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Configuration;
using System.Web.Services;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System.Threading;



namespace Acurus.Capella.UI
{
    public partial class frmSummaryNew : System.Web.UI.Page
    {
        //string strXmlEncounterPath = string.Empty;
        string ScreenMode = string.Empty;
        string strTransformSource = string.Empty;
        ulong Encounter_Id = 0;
        string sXMLHumanDoc = string.Empty;
        string sXMLEncounterDoc = string.Empty;
        HumanBlobManager HumanBlobMngr = new HumanBlobManager();
        EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
        string sTabMode = "false";
        //Cap - 1414, 1415, 1449
        string sIsAkidoEncounter = "false";

        string sIsPhoneEncounter = "N";
        UtilityManager UtilityMngr = new UtilityManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            Loadsummary();
        }


        public void Loadsummary()
        {
            if (Request.QueryString["TabMode"] != null && Request.QueryString["TabMode"].ToString() != string.Empty)
            {
                sTabMode = Request.QueryString["TabMode"].ToString();
            }
            Stopwatch objTimer = new Stopwatch();
            objTimer.Start();
            string transformtime = "";
            // DownloadFrame.TransformSource = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR.xsl");

            ulong Human_ID = 0;
            string HumanID = "";
            EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
            IList<Encounter_Blob> ilstEncounterBlob = new List<Encounter_Blob>();

            if (Request.QueryString["EncounterId"] != null)
            {
                Encounter_Id = Convert.ToUInt32(Request.QueryString["EncounterId"].ToString());
                hdnEncounterId.Value = Request.QueryString["EncounterId"].ToString();
                // ClientSession.Selectedencounterid = Encounter_Id;
            }

            if (Request.QueryString["HumanID"] != null)
            {
                Human_ID = Convert.ToUInt32(Request.QueryString["HumanID"].ToString());
                // ClientSession.Selectedencounterid = Encounter_Id;
            }

            //if (System.Configuration.ConfigurationSettings.AppSettings["IsAkidoNoteSummary"] == "Y" && Request["AkidoSummary"] == null)
            //{
            //    try
            //    {
            //        var myUri = new Uri(System.Configuration.ConfigurationSettings.AppSettings["AkidoNoteStatusURL"].ToString().Replace("[CapellaEncounterID]", Encounter_Id.ToString()));
            //        string AccessToken = System.Configuration.ConfigurationSettings.AppSettings["AkidoNoteStatusURLToken"].ToString();
            //        var myWebRequest = WebRequest.Create(myUri);
            //        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            //        myHttpWebRequest.PreAuthenticate = true;
            //        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
            //        myHttpWebRequest.Accept = "application/json";

            //        var myWebResponse = myWebRequest.GetResponse();
            //        var responseStream = myWebResponse.GetResponseStream();

            //        var myStreamReader = new StreamReader(responseStream, Encoding.Default);
            //        var json = myStreamReader.ReadToEnd();

            //        if (json.ToString() != "[]")
            //        {
            //            xslFrame.Visible = false;
            //            //AkidoFrame.Visible = true;
            //            //iFrameAkidoSummary.Attributes.Add("src", System.Configuration.ConfigurationSettings.AppSettings["AkidoNoteURL"].ToString().Replace("[CapellaEncounterID]", ClientSession.EncounterId.ToString()).Replace("[ClientName]", ClientSession.LegalOrg));

            //            btntreatment.Visible = false;
            //            btnwellness.Visible = false;
            //            Button1.Visible = false;
            //            btnPrint.Visible = false;
            //            btnCancelPhoneEnc.Visible = false;
            //            txtSearch.Visible = false;
            //            dvsignphy.Visible = false;
            //            dvsignreviewphy.Visible = false;

            //            responseStream.Close();
            //            myWebResponse.Close();

            //            string sAkidoURL = System.Configuration.ConfigurationSettings.AppSettings["AkidoNoteURL"].ToString().Replace("[CapellaEncounterID]", Encounter_Id.ToString()).Replace("[ClientName]", ClientSession.LegalOrg.ToLower());

            //            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "Summary", "AkidoNoteClickSum('" + sAkidoURL + "');", true);
            //            return;
            //        }

            //        responseStream.Close();
            //        myWebResponse.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.ToString());
            //    }
            //}
            
            if (Encounter_Id == 0)
            {
                Encounter_Id = ClientSession.EncounterId;
                Human_ID = ClientSession.HumanId;
            }
            //Jira #CAP-855
            string sIsAkidoEncounter = "false";
            string sExMessage = "";
            sIsAkidoEncounter = UtilityManager.IsAkidoEncounter(Encounter_Id.ToString(), out sExMessage);
            if (Request.QueryString["Menu"] == null && System.Configuration.ConfigurationSettings.AppSettings["IsAkidoNoteSummary"] == "Y" && sIsAkidoEncounter == "true")
            {
                xslFrame.Visible = false;
                //AkidoFrame.Visible = true;
                //iFrameAkidoSummary.Attributes.Add("src", System.Configuration.ConfigurationSettings.AppSettings["AkidoNoteURL"].ToString().Replace("[CapellaEncounterID]", ClientSession.EncounterId.ToString()).Replace("[ClientName]", ClientSession.LegalOrg));

                btntreatment.Visible = false;
                btnwellness.Visible = false;
                Button1.Visible = false;
                btnPrint.Visible = false;
                btnCancelPhoneEnc.Visible = false;
                txtSearch.Visible = false;
                dvsignphy.Visible = false;
                dvsignreviewphy.Visible = false;

                string sAkidoURL = System.Configuration.ConfigurationSettings.AppSettings["AkidoNoteURL"].ToString().Replace("[CapellaEncounterID]", Encounter_Id.ToString()).Replace("[ClientName]", ClientSession.LegalOrg.ToLower());

                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "Summary", "AkidoNoteClickSum('" + sAkidoURL + "');", true);
                return;
            }
            //Cap - 1414, 1415, 1449
            //else if (Request.QueryString["Menu"] != null && System.Configuration.ConfigurationSettings.AppSettings["IsAkidoNoteSummary"] == "Y" && sIsAkidoEncounter == "true")
            else if (Request.QueryString["Menu"] != null && System.Configuration.ConfigurationSettings.AppSettings["IsAkidoNoteSummary"] == "Y" && sIsAkidoEncounter == "true" && !Request.QueryString["Menu"].Contains("FAX"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('1011197'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            //Jira #CAP-855
            else if (Request.QueryString["Menu"] == null && System.Configuration.ConfigurationSettings.AppSettings["IsAkidoNoteSummary"] == "Y" && sIsAkidoEncounter == "Exception")
            {
                
                string sErrorMessag = DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + "_1011199: " + "We have encountered an error retrieving the clinical note.  Try again and if this persists, contact support. $|$ Exception Message: " + sExMessage.Replace("'","");
                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryHumanIDAlert('"+ sErrorMessag + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            else if (Request.QueryString["Menu"] != null && System.Configuration.ConfigurationSettings.AppSettings["IsAkidoNoteSummary"] == "Y" && sIsAkidoEncounter == "Exception")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('1011199'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Page Load : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            if (Request.QueryString["IsPatientList"] != null && Request.QueryString["IsPatientList"].ToString() == "Y")
            {
                //btnconword.Visible = false;
                //btnword.Visible = false;
                txtSearch.Visible = false;
                btnServiceProcedureCode.Visible = false;
                IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                ActivityLog activity = new ActivityLog();
                activity.Human_ID = Human_ID;
                activity.Encounter_ID = Encounter_Id;

                activity.Sent_To = string.Empty;
                activity.Activity_Date_And_Time = UtilityManager.ConvertToUniversal();
                activity.Role = "Auditor";
                activity.Subject = string.Empty;
                activity.Message = string.Empty;
                activity.Activity_Type = "View Encounter Summary";
                activity.Activity_By = ClientSession.UserName;
                ActivityLogList.Add(activity);
                ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
            }

            string FileName = string.Empty;
            if (Request.QueryString["DisasterRecovery"] != null && Request.QueryString["DisasterRecovery"].Contains("True"))
            {
                btnPrint.Visible = false;
                FileName = "DR_Encounter" + "_" + Encounter_Id + ".xml";
                string FilePath_DR = string.Empty;
                FilePath_DR = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPathDR"], FileName);
                //if (File.Exists(FilePath_DR))
                //{
                //    strXmlEncounterPath = FilePath_DR;
                //    if (File.Exists(Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPathDR"], "EHR_DR.xsl")))
                //        strTransformSource = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPathDR"], "EHR_DR.xsl");
                //}
                //else if (File.Exists(Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPathDRArchive"], FileName)))
                //{
                //    strXmlEncounterPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPathDRArchive"], FileName);
                //    if (File.Exists(Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPathDRArchive"], "EHR_DR.xsl")))
                //        strTransformSource = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPathDRArchive"], "EHR_DR.xsl");
                //}
            }
            else
            {
                if (Encounter_Id != 0)
                {
                    FileName = "Encounter" + "_" + Encounter_Id + ".xml";

                }
                else
                {
                    Encounter_Id = ClientSession.EncounterId;
                    Human_ID = ClientSession.HumanId;
                    FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
                    // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "DisplayErrorMessage('110063'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);


                }
                //if (File.Exists(Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName)))
                //    strXmlEncounterPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);




                //XmlTextReader xmltxtReader = null;

                int flag = 0;

                //IList<string> ilstSummaryTag_List = new List<string>();
                //ilstSummaryTag_List.Add("EncounterList");

                //EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
                //IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(Encounter_Id);
                ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(Encounter_Id);
                if (ilstEncounterBlob.Count > 0)
                {
                    string sXMLContent = string.Empty;
                    XmlDocument xmlDoc = new XmlDocument();
                    try
                    {
                        sXMLContent = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                        if (sXMLContent.Substring(0, 1) != "<")
                            sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                        xmlDoc.LoadXml(sXMLContent);

                        XmlNodeList xmlNodeList = xmlDoc.GetElementsByTagName("EncounterList");
                        if (xmlNodeList.Count > 0)
                        {
                            for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                            {
                                HumanID = xmlNodeList[0].ChildNodes[j].Attributes["Human_ID"].Value;
                                break;
                            }
                        }
                        else
                        {
                             ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryHumanIDAlert('EncounterList Tag is not found. Please contact support team to regenerate the XML.');", true);
                            return;  
                        }

                    }
                    catch
                    {
                        if (Encounter_Id != 0)
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + Encounter_Id.ToString() + "','Encounter','summary');", true);

                        }
                        else if (ClientSession.EncounterId != 0)
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + ClientSession.EncounterId.ToString() + "','Encounter','summary');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryAlert();", true);
                        }
                        return;
                        //throw new Exception("Encounter XML is invalid");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryAlert();", true);
                    return;
                }

                //IList<object> ilstSummaryBlob_Final = new List<object>();
                //try
                //{
                //    ilstSummaryBlob_Final = UtilityManager.ReadBlob(Encounter_Id, ilstSummaryTag_List);

                //    if (ilstSummaryBlob_Final != null && ilstSummaryBlob_Final.Count > 0)
                //    {
                //        if (ilstSummaryBlob_Final[0] != null)
                //        {
                //            for (int iCount = 0; iCount < ((IList<object>)ilstSummaryBlob_Final[0]).Count; iCount++)
                //            {
                //                HumanID = ((Encounter)((IList<object>)ilstSummaryBlob_Final[0])[iCount]).Human_ID.ToString();
                //            }
                //        }
                //    }
                //}
                //catch (Exception XMLExcep)
                //{
                //    if (Encounter_Id != 0)
                //    {
                //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + Encounter_Id.ToString() + "','Encounter','summary');", true);

                //    }
                //    else
                //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + ClientSession.EncounterId.ToString() + "','Encounter','summary');", true);

                //    return;

                //}

                // try 
                //{
                //    if (File.Exists(strXmlEncounterPath) == true)
                //    {
                //        try
                //        {
                //            using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //            {
                //                XmlDocument itemDoc = new XmlDocument();
                //                xmltxtReader = new XmlTextReader(fs);
                //                itemDoc.Load(xmltxtReader);
                //                xmltxtReader.Close();
                //                XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("EncounterList");
                //                if (xmlNodeList.Count > 0)
                //                {
                //                    for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                //                    {
                //                        ////objStaticLookup = new StaticLookup();
                //                        if (xmlNodeList[0].ChildNodes[j].Attributes["Local_Time"].Value == "")
                //                        {
                //                            flag = 1;
                //                            xmlNodeList[0].ChildNodes[j].Attributes["Local_Time"].Value = UtilityManager.ConvertToLocal(Convert.ToDateTime(xmlNodeList[0].ChildNodes[j].Attributes["Date_of_Service"].Value)).ToString("yyyy-MM-dd hh:mm:ss tt");
                //                        }
                //                        HumanID = xmlNodeList[0].ChildNodes[j].Attributes["Human_ID"].Value;


                //                        //iFieldLookupList.Add(objStaticLookup);
                //                    }
                //                    if (flag == 1)
                //                    {
                //                       // itemDoc.Save(strXmlEncounterPath);
                //                        int trycount = 0;
                //                    trytosaveagain:
                //                        try
                //                        {
                //                            itemDoc.Save(strXmlEncounterPath);
                //                        }
                //                        catch (Exception xmlexcep)
                //                        {
                //                            trycount++;
                //                            if (trycount <= 3)
                //                            {
                //                                int TimeMilliseconds = 0;
                //                                if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                //                                    TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                //                                Thread.Sleep(TimeMilliseconds);
                //                                string sMsg = string.Empty;
                //                                string sExStackTrace = string.Empty;

                //                                string version = "";
                //                                if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                //                                    version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                //                                string[] server = version.Split('|');
                //                                string serverno = "";
                //                                if (server.Length > 1)
                //                                    serverno = server[1].Trim();

                //                                if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                //                                    sMsg = xmlexcep.InnerException.Message;
                //                                else
                //                                    sMsg = xmlexcep.Message;

                //                                if (xmlexcep != null && xmlexcep.StackTrace != null)
                //                                    sExStackTrace = xmlexcep.StackTrace;

                //                                string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                //                                string ConnectionData;
                //                                ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                //                                using (MySqlConnection con = new MySqlConnection(ConnectionData))
                //                                {
                //                                    using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                //                                    {
                //                                        cmd.Connection = con;
                //                                        try
                //                                        {
                //                                            con.Open();
                //                                            cmd.ExecuteNonQuery();
                //                                            con.Close();
                //                                        }
                //                                        catch
                //                                        {
                //                                        }
                //                                    }
                //                                }
                //                                goto trytosaveagain;
                //                            }
                //                        }
                //                    }
                //                }
                //                fs.Close();
                //                fs.Dispose();
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            // if (ex.Message.ToLower().Contains("input string was not") == true || ex.Message.ToLower().Contains("element") == true||ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                //            {

                //                xmltxtReader.Close();
                //                if (Encounter_Id != 0)
                //                {
                //                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + Encounter_Id.ToString() + "','Encounter','summary');", true);

                //                }
                //                else
                //                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + ClientSession.EncounterId.ToString() + "','Encounter','summary');", true);



                //                return;
                //            }

                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception(ex.Message + " - " + strXmlEncounterPath);
                //}





                //string humanFileName = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "Human_" + HumanID + ".xml");
                flag = 0;
                //try
                //{
                //    if (File.Exists(humanFileName) == true)
                //    {
                //        try
                //        {
                //            using (FileStream fs = new FileStream(humanFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                //            {
                //                XmlDocument itemDoc = new XmlDocument();
                //                xmltxtReader = new XmlTextReader(fs);
                //                itemDoc.Load(xmltxtReader);
                //                xmltxtReader.Close();
                //                XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("EncounterList");
                //                if (xmlNodeList.Count > 0)
                //                {
                //                    for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                //                    {
                //                        ////objStaticLookup = new StaticLookup();
                //                        if (xmlNodeList[0].ChildNodes[j].Attributes["Human_ID"].Value == HumanID)
                //                        {
                //                            if (xmlNodeList[0].ChildNodes[j].Attributes["Local_Time"].Value == "")
                //                            {
                //                                flag = 1;
                //                                xmlNodeList[0].ChildNodes[j].Attributes["Local_Time"].Value = UtilityManager.ConvertToLocal(Convert.ToDateTime(xmlNodeList[0].ChildNodes[j].Attributes["Date_of_Service"].Value)).ToString("yyyy-MM-dd hh:mm:ss tt");

                //                            }
                //                            break;

                //                        }


                //                        //iFieldLookupList.Add(objStaticLookup);
                //                    }
                //                    if (flag == 1)
                //                    {
                //                       // itemDoc.Save(humanFileName);
                //                        int trycount = 0;
                //                    trytosaveagain:
                //                        try
                //                        {
                //                            itemDoc.Save(humanFileName);
                //                        }
                //                        catch (Exception xmlexcep)
                //                        {
                //                            trycount++;
                //                            if (trycount <= 3)
                //                            {
                //                                int TimeMilliseconds = 0;
                //                                if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                //                                    TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                //                                Thread.Sleep(TimeMilliseconds);
                //                                string sMsg = string.Empty;
                //                                string sExStackTrace = string.Empty;

                //                                string version = "";
                //                                if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                //                                    version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                //                                string[] server = version.Split('|');
                //                                string serverno = "";
                //                                if (server.Length > 1)
                //                                    serverno = server[1].Trim();

                //                                if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                //                                    sMsg = xmlexcep.InnerException.Message;
                //                                else
                //                                    sMsg = xmlexcep.Message;

                //                                if (xmlexcep != null && xmlexcep.StackTrace != null)
                //                                    sExStackTrace = xmlexcep.StackTrace;

                //                                string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                //                                string ConnectionData;
                //                                ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                //                                using (MySqlConnection con = new MySqlConnection(ConnectionData))
                //                                {
                //                                    using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                //                                    {
                //                                        cmd.Connection = con;
                //                                        try
                //                                        {
                //                                            con.Open();
                //                                            cmd.ExecuteNonQuery();
                //                                            con.Close();
                //                                        }
                //                                        catch
                //                                        {
                //                                        }
                //                                    }
                //                                }
                //                                goto trytosaveagain;
                //                            }
                //                        }
                //                    }
                //                }
                //                fs.Close();
                //                fs.Dispose();
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            // if (ex.Message.ToLower().Contains("input string was not") == true || ex.Message.ToLower().Contains("element") == true||ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                //            {
                //                xmltxtReader.Close();

                //                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + HumanID.ToString() + "','Human','summary');", true);


                //                //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");
                //                return;
                //            }

                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception(ex.Message + " - " + humanFileName);
                //}

            }
            if (Request.QueryString["PhoneEncounter"] != null && Request.QueryString["PhoneEncounter"].Contains("True"))
            {
                btnCancelPhoneEnc.Visible = true;
                btnServiceProcedureCode.Visible = false;
                Button1.Visible = false;
                btnwellness.Visible = false;
                btntreatment.Visible = false;
                if (File.Exists(Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Phone_Encounter_Summary.xsl")))
                {
                    // jira cap-499
                    sIsPhoneEncounter = "Y";
                    strTransformSource = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Phone_Encounter_Summary.xsl");
                }
                //For Phone encounter cancel appointment
                if (Encounter_Id != 0)
                {
                    EncounterManager encmngr = new EncounterManager();
                    IList<Encounter> objenc = encmngr.GetEncounterByEncounterIDIncludeArchive(Encounter_Id);
                    if (objenc != null && objenc.Count > 0)
                        hdnBatchStatus.Value = objenc[0].Batch_Status.ToString();
                }
            }
            else
            {
                btnCancelPhoneEnc.Visible = false;
                Button1.Visible = true;
                btnwellness.Visible = true;
                btntreatment.Visible = true;
                if (Request.QueryString["DisasterRecovery"] != null && Request.QueryString["DisasterRecovery"].Contains("True"))
                {
                    //DownloadFrame.TransformSource = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPathDR"], "EHR_DR.xsl");
                }
                else
                {
                    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary XSLT Transformation : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
                    if (File.Exists(Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR.xsl")))
                        strTransformSource = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR.xsl");
                    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary XSLT Transformation : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
                }

            }

            try
            {
                //Jira CAP-379
                UtilityManager utilitymngr = new UtilityManager();
                Boolean bAlert = utilitymngr.LoadBlobHumanXML(Convert.ToUInt64(HumanID), Encounter_Id, ilstEncounterBlob, sTabMode, out sXMLHumanDoc, sIsPhoneEncounter);

                if (bAlert == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryHumanIDAlert('Human xml is not preserved for the encounter. Please contact support.');", true);
                    return;
                }
                //IList<Human_Blob> ilstHumanBlob = new List<Human_Blob>();
                //ilstHumanBlob = HumanBlobMngr.GetHumanBlob(Convert.ToUInt64(HumanID));
                XmlDocument xmlHumanDoc = new XmlDocument();
                //if (ilstHumanBlob.Count > 0)
                if (sXMLHumanDoc != null && sXMLHumanDoc != "" && sXMLHumanDoc != string.Empty)
                {
                    //sXMLHumanDoc = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                    if (sXMLHumanDoc.Substring(0, 1) != "<")
                        sXMLHumanDoc = sXMLHumanDoc.Substring(1, sXMLHumanDoc.Length - 1);
                    //Jira #CAP-115
                    sXMLHumanDoc = UtilityManager.ReplaceSpecialCharaters(sXMLHumanDoc);
                    xmlHumanDoc.LoadXml(sXMLHumanDoc);
                }
                else {
                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryHumanIDAlert('Human xml is not preserved for the encounter. Please contact support.');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                // if (ex.Message.ToLower().Contains("input string was not") == true || ex.Message.ToLower().Contains("element") == true||ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                {
                    //xmltxtReader.Close();

                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + HumanID.ToString() + "','Human','summary');", true);


                    //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");
                    return;
                }

            }

            try
            {
                //IList<Encounter_Blob> ilstEncounterBlob = new List<Encounter_Blob>();
                //ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(Encounter_Id);
                if (ilstEncounterBlob.Count > 0)
                {
                    sXMLEncounterDoc = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                    if (sXMLEncounterDoc.Substring(0, 1) != "<")
                        sXMLEncounterDoc = sXMLEncounterDoc.Substring(1, sXMLEncounterDoc.Length - 1);
                    //Jira #CAP-115
                    sXMLEncounterDoc = UtilityManager.ReplaceSpecialCharaters(sXMLEncounterDoc);
                }
            }
            catch (Exception ex)
            {
                // if (ex.Message.ToLower().Contains("input string was not") == true || ex.Message.ToLower().Contains("element") == true||ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                {
                    //xmltxtReader.Close();

                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + Encounter_Id.ToString() + "','Encounter','summary');", true);


                    //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");
                    return;
                }

            }
            //CAP-1514
            if (!IsPostBack)
            {

                // if (ScreenMode != null && ScreenMode == Request.QueryString["sMenu"])
                //  if (Request.QueryString["sMenu"] != null && Request.QueryString["sMenu"].Contains("MyQArchive"))
                // {
                //   this.Page.Title = "Summary";
                //}

                if (Request.QueryString["Menu"] != null && Request.QueryString["Menu"].Contains("Word"))
                {

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "downloadFile();", true);

                }
                else if (Request.QueryString["Menu"] != null && Request.QueryString["Menu"].Contains("PDF"))
                {

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "downloadFilePDF();", true);

                }
                else if (Request.QueryString["Menu"] != null && Request.QueryString["Menu"].Contains("FAX"))
                {

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "downloadFileFax();", true);

                }
                else
                {
                    // if (File.Exists(strXmlEncounterPath) == true)
                    if (sXMLEncounterDoc != string.Empty)
                    {
                        txtSearch.Style.Add("display", "block");
                        Stopwatch objTimernew = new Stopwatch();
                        objTimernew.Start();
                        //DownloadFrame.DocumentSource = strXmlEncounterPath;
                        objTimernew.Stop();
                        transformtime = objTimernew.Elapsed.TotalSeconds.ToString();
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryAlert();", true);
                        return;
                    }
                }

            }
            if (Request.QueryString["Menu"] == null)
            {

                //if (File.Exists(strXmlEncounterPath) == true)
                if (sXMLEncounterDoc != string.Empty)
                {

                    txtSearch.Style.Add("display", "block");
                    Stopwatch objTimernew = new Stopwatch();
                    objTimernew.Start();
                    //DownloadFrame.DocumentSource = strXmlEncounterPath;
                    objTimernew.Stop();
                    transformtime = objTimernew.Elapsed.TotalSeconds.ToString();


                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryAlert();", true);
                    return;
                }
            }
            objTimer.Stop();
            string totaltime = objTimer.Elapsed.TotalSeconds.ToString();
            if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y" && ClientSession.UserPermissionDTO.Scntab != null)
            {
                var scn_id = (from p in ClientSession.UserPermissionDTO.Scntab where p.SCN_Name == "frmEFax" select p).ToList();
                if (scn_id.Count() > 0)
                {
                    var EnableEFax = from p in ClientSession.UserPermissionDTO.Screens where p.SCN_ID == Convert.ToInt32(scn_id[0].SCN_ID) && p.Permission == "U" select p;
                    if (EnableEFax.Count() > 0)
                    {
                        btnFax.Visible = true;
                        btnconfax.Visible = true;
                    }
                    else
                    {
                        btnFax.Visible = false;
                        btnconfax.Visible = false;
                    }
                }

            }

            //if (File.Exists(strXmlEncounterPath))
            //{
            //    XslCompiledTransform objXSLTransform = new XslCompiledTransform();
            //    XsltSettings settings = new XsltSettings(true, false);
            //    objXSLTransform.Load(strTransformSource, settings, new XmlUrlResolver());

            //    // Creating StringBuilder object to hold html data and creates TextWriter object to hold data from XslCompiled.Transform method
            //    StringBuilder htmlOutput = new StringBuilder();
            //    TextWriter htmlWriter = new StringWriter(htmlOutput);

            //    // Creating XmlReader object to read XML content
            //    XmlReader reader = XmlReader.Create(strXmlEncounterPath);

            //    // Call Transform() method to create html string and write in TextWriter object.
            //    objXSLTransform.Transform(reader, null, htmlWriter);
            //    ltlDownloadFrame.Text = htmlOutput.ToString();

            //    // Closing xmlreader object
            //    reader.Close();


            //    //
            //    string Encounter_signedDate = "";
            //    string Encounter_Provider_Name = "";
            //    string Encounter_Reviewed_signedDate = "";
            //    string Encounter_Reviewed_Name = "";
            //    string Encounter_Reviewed_Id = "";
            //    XDocument xmlDocumentType = XDocument.Load(strXmlEncounterPath);

            //    foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
            //    {
            //        foreach (XElement Encounter in elements.Elements())
            //        {
            //            DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value);
            //            Encounter_Reviewed_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm tt");

            //            DateTime dtPro = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
            //            Encounter_signedDate = UtilityManager.ConvertToLocal(dtPro).ToString("dd-MMM-yyyy hh:mm tt");

            //            Encounter_Reviewed_Id = Encounter.Attribute("Encounter_Provider_Review_ID").Value;

            //        }

            //        //if (Encounter_signedDate == "" || Encounter_signedDate == "01-Jan-0001 12:00:00 AM")
            //        //{
            //        // foreach (XElement Encounter in elements.Elements())
            //        // {
            //        //     DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
            //        //     Encounter_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm:ss tt");
            //        //}
            //        //}
            //    }
            //    //Provider Name 
            //    foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            //    {
            //        foreach (XElement Encounter in elements.Elements())
            //        {
            //            Encounter_Provider_Name = Encounter.Value;
            //            break;
            //        }
            //        break;
            //    }
            //    //Provider Reviewed Name 
            //    if (Encounter_Reviewed_Id != "")
            //    {
            //        string xmlFilepathUser = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
            //        if (File.Exists(xmlFilepathUser))
            //        {
            //            XmlDocument xdoc = new XmlDocument();
            //            XmlTextReader itext = new XmlTextReader(xmlFilepathUser);
            //            xdoc.Load(itext);
            //            itext.Close();
            //            XmlNodeList xnodelst = xdoc.GetElementsByTagName("User");
            //            if (xnodelst != null && xnodelst.Count > 0)
            //            {
            //                foreach (XmlNode xnode in xnodelst)
            //                {
            //                    if (xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() != "0" && xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() == Encounter_Reviewed_Id)
            //                    {
            //                        Encounter_Reviewed_Name = xnode.Attributes.GetNamedItem("person_name").Value;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM")
            //        lblSignedPhysician.InnerText = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;

            //    string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
            //    StaticLookupManager staticMngr = new StaticLookupManager();
            //    string strfooterProviderReviewed = string.Empty;
            //    IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
            //    if (CommonList.Count > 0 && Encounter_Reviewed_Name != "" && Encounter_Reviewed_signedDate != "" && Encounter_Reviewed_signedDate != "01-Jan-0001 12:00 AM")
            //        lblReviewSignedPhysician.InnerText = CommonList[0].Value.Replace("<Physician>", Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate).Replace("|", "");
            //    //
            //}
            //else
            //{

            //    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryAlert();", true);
            //}


            if (sXMLEncounterDoc != string.Empty)
            {

                StringBuilder sb = new StringBuilder();
                sb.Append(sXMLEncounterDoc.ToString().Replace("</notes>", "").Replace("</Modules>", ""));

                string SUB = sXMLHumanDoc.ToString().Substring(0, sXMLHumanDoc.LastIndexOf("?>") + 2);

                sb.Append(sXMLHumanDoc.ToString().Replace(SUB, "").Replace("<notes>", "").Replace("<Modules>", ""));
                //Jira #CAP-344 - OldCode
                //StringBuilder htmlOutput = new StringBuilder();
                //TextWriter htmlWriter = new StringWriter(htmlOutput);

                XmlReader xmlr = XmlReader.Create(new StringReader(sb.ToString()));

                //XslCompiledTransform objXSLTransform = new XslCompiledTransform();
                //XsltSettings settingsxsl = new XsltSettings(true, false);
                //objXSLTransform.Load(strTransformSource, settingsxsl, new XmlUrlResolver());

                //objXSLTransform.Transform(xmlr, null, htmlWriter);
                //ltlDownloadFrame.Text = htmlWriter.ToString();
               
                ltlDownloadFrame.Text = UtilityManager.PrintSummaryUsingXSLT(strTransformSource, xmlr).ToString();

                //
                string Encounter_signedDate = "";
                string Encounter_Provider_Name = "";
                string Encounter_Reviewed_signedDate = "";
                string Encounter_Reviewed_Name = "";
                string Encounter_Reviewed_Id = "";
                string sIsPhoneEncounter = "";
                string sCreatedBy = "";
             
                //XDocument xmlDocumentType = XDocument.Load(strXmlEncounterPath);
                TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
                XDocument xmlDocumentType = XDocument.Load(EncXMLContent);

                foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
                {
                    foreach (XElement Encounter in elements.Elements())
                    {
                        DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value);
                        Encounter_Reviewed_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm tt");

                        DateTime dtPro = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                        Encounter_signedDate = UtilityManager.ConvertToLocal(dtPro).ToString("dd-MMM-yyyy hh:mm tt");

                        Encounter_Reviewed_Id = Encounter.Attribute("Encounter_Provider_Review_ID").Value;
                        sIsPhoneEncounter= Encounter.Attribute("Is_Phone_Encounter").Value;
                        sCreatedBy= Encounter.Attribute("Created_By").Value;
                    }

                    //if (Encounter_signedDate == "" || Encounter_signedDate == "01-Jan-0001 12:00:00 AM")
                    //{
                    // foreach (XElement Encounter in elements.Elements())
                    // {
                    //     DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                    //     Encounter_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm:ss tt");
                    //}
                    //}
                }
                //Provider Name 
                foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
                {
                    foreach (XElement Encounter in elements.Elements())
                    {
                        Encounter_Provider_Name = Encounter.Value;
                        break;
                    }
                    break;
                }
                //Provider Reviewed Name 
                if (Encounter_Reviewed_Id != "")
                {
                    string xmlFilepathUser = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
                    if (File.Exists(xmlFilepathUser))
                    {
                        XmlDocument xdoc = new XmlDocument();
                        XmlTextReader itext = new XmlTextReader(xmlFilepathUser);
                        xdoc.Load(itext);
                        itext.Close();
                        XmlNodeList xnodelst = xdoc.GetElementsByTagName("User");
                        if (xnodelst != null && xnodelst.Count > 0)
                        {
                            foreach (XmlNode xnode in xnodelst)
                            {
                                if (xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() != "0" && xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() == Encounter_Reviewed_Id)
                                {
                                    Encounter_Reviewed_Name = xnode.Attributes.GetNamedItem("person_name").Value;
                                }
                            }
                        }
                    }
                }

                //Jira #CAP-858
                //if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM")
                //    lblSignedPhysician.InnerText = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;

                if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && sIsPhoneEncounter != "Y")
                {
                    lblSignedPhysician.InnerText = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
                }
                else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && sIsPhoneEncounter == "Y")
                {
                    if (Encounter_Provider_Name != "")
                    {
                        lblSignedPhysician.InnerText = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
                    }
                    else
                    {
                        lblSignedPhysician.InnerText = "Electronically Signed by " + sCreatedBy + " at " + Encounter_signedDate;
                    }
                }
                

                string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
                StaticLookupManager staticMngr = new StaticLookupManager();
                string strfooterProviderReviewed = string.Empty;
                IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
                if (CommonList.Count > 0 && Encounter_Reviewed_Name != "" && Encounter_Reviewed_signedDate != "" && Encounter_Reviewed_signedDate != "01-Jan-0001 12:00 AM")
                    lblReviewSignedPhysician.InnerText = CommonList[0].Value.Replace("<Physician>", Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate).Replace("|", "");
                //
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryAlert();", true);
                return;
            }

            //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryTimeStamp('" + totaltime + "','" + transformtime + "');", true);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Page Load : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Summary", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);


        }
        protected void btnWord_Click(object sender, EventArgs e)
        {
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Progress Notes : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            Stopwatch objTimernew = new Stopwatch();
            objTimernew.Start();
            //string xmlDataFile = strXmlEncounterPath;
            string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml";
            //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);
            // string xsltFile = HttpContext.Current.Server.MapPath("EHR_Progress_Notes.xsl");
            // string xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Progress_Notes.xsl");

            string xsltFile = string.Empty;
            if (Request.QueryString["PhoneEncounter"] != null && Request.QueryString["PhoneEncounter"].Contains("True"))
            {
                // jira cap-499
                sIsPhoneEncounter = "Y";
                xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Phone_Encounter_Notes.xsl");
            }
            else
            {
                xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Progress_Notes.xsl");
            }

            string OutputNamingConvention = System.Configuration.ConfigurationSettings.AppSettings["ProgressNotesNamingConvention"];
            string sNamingConvention = string.Empty;
            string[] OutputName = OutputNamingConvention.Split('~');
            string NotesName = OutputNamingConvention;
            string result = string.Empty;
            string[] dtFormat = OutputNamingConvention.Split('^');
            string sDateFormat = string.Empty;

            if (dtFormat.Count() > 1)
            {
                if (dtFormat[1].Contains("@"))
                {
                    dtFormat[1] = dtFormat[1].Split('@')[0];
                }
            }
            string[] FormatCase = OutputNamingConvention.Split('@');
            for (int i = 0; i < OutputName.Length; i++)
            {
                if (OutputName[i] != "")
                {
                    result = ExtractBetween(OutputName[i], "[", "]");
                    //if (result.ToUpper().Contains("FACILITY"))
                    //{
                    //    if (FormatCase.Count() > 1)
                    //    {
                    //        if (FormatCase[1].ToUpper().Contains("YES"))
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", "")).ToUpper();
                    //        else
                    //        {
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //        }
                    //        NotesName = NotesName.Replace(FormatCase[1], "");
                    //    }
                    //    else
                    //    {
                    //        NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //    }

                    //}

                    if (result.ToUpper().Contains("HUMAN"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.HumanId.ToString());
                    }
                    if (result.ToUpper().Contains("ENCOUNTER"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.EncounterId.ToString());

                    }
                    if (result.ToUpper().Contains("MEMBER") || result.ToUpper().Contains("LAST") || result.ToUpper().Contains("FIRST") || result.ToUpper().Contains("DOB"))
                    {
                        string sExternalAccNo = string.Empty;
                        string sLastName = string.Empty;
                        string sFirstName = string.Empty;
                        string sDOB = string.Empty;
                        //try
                        //{
                        //if (File.Exists(strXmlHumanPath) == true)
                        if (sXMLHumanDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLEncounterDoc);
                            //using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //itemDoc.Load(fs);

                            //XmlText.Close();
                            if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                            {
                                if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes.Count > 0)
                                {
                                    if (result.ToUpper().Contains("MEMBER"))
                                    {
                                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value != "")
                                        {
                                            sExternalAccNo = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value.ToString();
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", sExternalAccNo);
                                    }
                                    else if (result.ToUpper().Contains("LAST"))
                                    {
                                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != "")
                                        {
                                            if (FormatCase.Count() > 1)
                                            {
                                                if (FormatCase[1].ToUpper().Contains("YES"))
                                                    sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToUpper().ToString();
                                                else
                                                {
                                                    sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                                }
                                                NotesName = NotesName.Replace(FormatCase[1], "");
                                            }
                                            else
                                            {
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                            }
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", (sLastName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                    }
                                    else if (result.ToUpper().Contains("FIRST"))
                                    {
                                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != "")
                                        {
                                            if (FormatCase.Count() > 1)
                                            {
                                                if (FormatCase[1].ToUpper().Contains("YES"))
                                                    sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToUpper().ToString();
                                                else
                                                {
                                                    sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                                }
                                                NotesName = NotesName.Replace(FormatCase[1], "");
                                            }
                                            else
                                            {
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                            }
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", (sFirstName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                    }
                                    else if (result.ToUpper().Contains("DOB") || (OutputName[i].ToUpper().Contains("DATE") && OutputName[i].ToUpper().Contains("BIRTH")))
                                    {
                                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value != "")
                                        {
                                            if (dtFormat.Count() > 1)
                                            {
                                                if (dtFormat[1] == "yyyyMMdd")
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                                else if (dtFormat[1] == "MMddyyyy")
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyyyy");
                                                else if (dtFormat[1] == "MMddyy")
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyy");
                                                else if (dtFormat[1] == "yyMMdd")
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyMMdd");
                                                else
                                                {
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                                }
                                                NotesName = NotesName.Replace(dtFormat[1], "");
                                            }
                                            else
                                            {
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            }
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", sDOB);
                                    }
                                }
                            }
                            //fs.Close();
                            //fs.Dispose();
                            // }
                        }

                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception(ex.Message + " - " + strXmlHumanPath);
                        //}


                    }
                    if ((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE")) || result.ToUpper().Contains("FACILITY"))
                    {
                        //try
                        //{
                        //if (File.Exists(strXmlEncounterPath) == true)
                        if (sXMLEncounterDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlEncounterPath);
                            // itemDoc.Load(XmlText);
                            //using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //  itemDoc.Load(fs);

                            //  XmlText.Close();
                            itemDoc.LoadXml(sXMLEncounterDoc);
                            string sDOS = string.Empty;
                            if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                            {
                                if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes.Count > 0)
                                {
                                    if ((result.ToUpper().Contains("DOS") || OutputName[i].ToUpper().Contains("DATE")) && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value != "")
                                    {
                                        if (dtFormat.Count() > 1)
                                        {
                                            if (dtFormat[1] == "yyyyMMdd")
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                            else if (dtFormat[1] == "MMddyyyy")
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyyyy");
                                            else if (dtFormat[1] == "MMddyy")
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyy");
                                            else if (dtFormat[1] == "yyMMdd")
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyMMdd");
                                            else
                                            {
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                            }
                                            NotesName = NotesName.Replace(dtFormat[1], "");
                                        }
                                        else
                                        {
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", sDOS);
                                    }
                                    else if (result.ToUpper().Contains("FACILITY") && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Trim() != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", "")).ToUpper();
                                            else
                                            {
                                                NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                        }
                                    }
                                    else
                                    {
                                        NotesName = NotesName.Replace("[" + result + "]", "");
                                    }

                                }

                            }
                            //fs.Close();
                            // fs.Dispose();
                            // }
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception(ex.Message + " - " + strXmlHumanPath);
                        //}
                    }
                }
            }
            if (NotesName.Contains('['))
            {
                string[] sName = NotesName.Split('~');

                for (int i = 0; i < sName.Length; i++)
                {
                    if (sName[i] != "")
                    {
                        if (sName[i].Contains("["))
                        {
                            result = ExtractBetween(sName[i], "[", "]");
                            NotesName = NotesName.Replace("[" + result + "]", "");
                        }
                    }
                }
            }
            //NotesName = NotesName.Replace("~", "").Replace("__", "_").Replace("^", "").Replace("@", "");
            NotesName = UtilityMngr.ReplaceSpecialCharaterInFileName(NotesName);
           string WordOutputName = NotesName + ".html";
            string outputDocument = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], WordOutputName);
            #region HPvalue
            ////for hp value
            //EncounterManager objManager = new EncounterManager();
            //string[] arry = objManager.GetInusranceDetails(ClientSession.HumanId);
            //string sPriPlan = string.Empty;
            //string sPriCarrier = string.Empty;
            //if (arry[1] != null)
            //{
            //    if (arry[0] == "PRIMARY")
            //    {
            //        sPriPlan = arry[2].ToString();
            //        if (File.Exists(xmlDataFile) == true)
            //        {
            //            XmlDocument itemDoc = new XmlDocument();
            //            XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //            itemDoc.Load(XmlText);
            //            XmlText.Close();

            //            XmlNodeList xmlPatientName = itemDoc.GetElementsByTagName("HP");
            //            xmlPatientName[0].Attributes[0].Value = sPriPlan;
            //            itemDoc.Save(strXmlFilePath);
            //        }
            //    }
            //}
            ////
            #endregion

            //Jira #CAP-344 - OldCode
            //DataSet ds;
            //XmlDataDocument xmlDoc;
            //XslCompiledTransform xslTran;
            //XmlElement root;
            //XPathNavigator nav;
            //XmlTextWriter writer;
            //XsltSettings settings = new XsltSettings(true, false);
            //ds = new DataSet();
            ////ds.ReadXml(xmlDataFile);
            //// ds.ReadXml(new XmlTextReader(new StringReader(sXMLEncounterDoc)));
            //StringBuilder sb = new StringBuilder();
            //sb.Append(sXMLEncounterDoc.ToString().Replace("</notes>", "").Replace("</Modules>", ""));

            //string SUB = sXMLHumanDoc.ToString().Substring(0, sXMLHumanDoc.LastIndexOf("?>") + 2);

            //sb.Append(sXMLHumanDoc.ToString().Replace(SUB, "").Replace("<notes>", "").Replace("<Modules>", ""));

            //ds.ReadXml(new XmlTextReader(new StringReader(sb.ToString())));
            //xmlDoc = new XmlDataDocument(ds);
            //xslTran = new XslCompiledTransform();
            //using (var stream = File.Open(xsltFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            //{
            //    // xslTran.Load(xsltFile);
            //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Progress Notes XSLT Load : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            //    xslTran.Load(xsltFile, settings, new XmlUrlResolver());
            //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Progress Notes XSLT Load : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            //}
            //root = xmlDoc.DocumentElement;
            //nav = root.CreateNavigator();
            //if (File.Exists(outputDocument))
            //{
            //    File.Delete(outputDocument);
            //}
            //writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8);
            //xslTran.Transform(nav, writer);
            //writer.Close();
            //writer = null;
            //nav = null;
            //root = null;
            //xmlDoc = null;
            //ds = null;
            //xslTran = null;

            //Jira #CAP-344 - NewCode
            UtilityManager.PrintPDFUsingXSLT(sXMLEncounterDoc, sXMLHumanDoc, xsltFile, outputDocument, sGroup_ID_Log);
            System.IO.FileInfo file = new System.IO.FileInfo(outputDocument);

            string Encounter_signedDate = "";
            string Encounter_Provider_Name = "";
            string Encounter_Reviewed_signedDate = "";
            string Encounter_Reviewed_Name = "";
            string Encounter_Reviewed_Id = "";
            TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
            XDocument xmlDocumentType = XDocument.Load(EncXMLContent);

            foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value);
                    Encounter_Reviewed_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm tt");

                    DateTime dtPro = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                    Encounter_signedDate = UtilityManager.ConvertToLocal(dtPro).ToString("dd-MMM-yyyy hh:mm tt");

                    Encounter_Reviewed_Id = Encounter.Attribute("Encounter_Provider_Review_ID").Value;

                }

                //if (Encounter_signedDate == "" || Encounter_signedDate == "01-Jan-0001 12:00:00 AM")
                //{
                // foreach (XElement Encounter in elements.Elements())
                // {
                //     DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                //     Encounter_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm:ss tt");
                //}
                //}
            }
            //Provider Name 
            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    Encounter_Provider_Name = Encounter.Value;
                    break;
                }
                break;
            }
            //Provider Reviewed Name 
            if (Encounter_Reviewed_Id != "")
            {
                string xmlFilepathUser = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
                if (File.Exists(xmlFilepathUser))
                {
                    XmlDocument xdoc = new XmlDocument();
                    XmlTextReader itext = new XmlTextReader(xmlFilepathUser);
                    xdoc.Load(itext);
                    itext.Close();
                    XmlNodeList xnodelst = xdoc.GetElementsByTagName("User");
                    if (xnodelst != null && xnodelst.Count > 0)
                    {
                        foreach (XmlNode xnode in xnodelst)
                        {
                            if (xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() != "0" && xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() == Encounter_Reviewed_Id)
                            {
                                Encounter_Reviewed_Name = xnode.Attributes.GetNamedItem("person_name").Value;
                            }
                        }
                    }
                }
            }


            string htmlString = System.IO.File.ReadAllText(outputDocument);
            string sProHeader = System.Configuration.ConfigurationSettings.AppSettings["ProgressNotesMainHeader"];
            string headerstring = string.Empty;
            if (sProHeader == "Y")
            {
                int index = htmlString.IndexOf("<table");
                int index1 = htmlString.LastIndexOf("</thead></table>");

                headerstring = htmlString.Substring(index, index1);

                string LogoPath = System.Configuration.ConfigurationSettings.AppSettings["ProgressNotesLogoPath"];
                string headerlogo = string.Empty;
                if (LogoPath == string.Empty)
                {
                    headerlogo = "<table><thead><tr><td></td>";
                }
                else
                {
                    headerlogo = "<table width='100%' height='30%' cellpadding='0' cellspacing='0' border='0'><thead><tr valign='middle'><td valign='top' width='10%'><img src='" + LogoPath + "'  alt='logo' />";
                }
                headerstring = headerstring.Replace("<table><thead><tr>", "");
                headerstring = headerlogo + headerstring + "</thead></table>";
                headerstring.Replace("height=' 30% '", "");
                htmlString = htmlString.Substring(index1 + 8, htmlString.Length - index1 - 8);
            }
            else
            {
                int index = htmlString.IndexOf("<table");
                int index1 = htmlString.IndexOf("</table>");
                headerstring = htmlString.Substring(index, index1);
                headerstring = headerstring + "</table>";
                headerstring.Replace("height=' 30% '", "");
                //htmlString = htmlString.Replace(headerstring, "");
                htmlString = htmlString.Substring(index1 + 8, htmlString.Length - index1 - 8);
            }
            string strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
            //string strfooterProviderReviewed = "I " + Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate +
            //     " have reviewed the chart and agree with the management plan with the changes to the plan as indicated.";

            string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
            StaticLookupManager staticMngr = new StaticLookupManager();
            string strfooterProviderReviewed = string.Empty;
            IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
            if (CommonList.Count > 0)
                strfooterProviderReviewed = CommonList[0].Value.Replace("<Physician>", Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate).Replace("|", "");


            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            var strBody = new StringBuilder();
            //string strDocBody = "";

            //strBody.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word'xmlns='http://www.w3.org/TR/REC-html40'><head><title></title><!--[if gte mso 9]><xml><w:WordDocument><w:View>Print</w:View><w:Zoom>100</w:Zoom><w:DoNotOptimizeForBrowser/></w:WordDocument></xml><![endif]--><style>p.MsoFooter, li.MsoFooter, div.MsoFooter{margin:0in;margin-bottom:.2000pt;mso-pagination:widow-orphan;tab-stops:center 3.0in right 6.0in;font-size:9.0pt;}<style><!-- /* Style Definitions */@page Section1{size:8.5in 11.0in; margin:1.0in 1.25in 1.0in 1.25in ;mso-header-margin:.5in;mso-header:h1;mso-footer: f1; mso-footer-margin:.5in;}div.Section1{page:Section1;}table#hrdftrtbl{margin:0in 0in 0in 9in;}-->.MyClass123{content:url('');</style></head>");
            //strBody.Append("<body><div class=Section1><basefont  face=Times New Roman  size=4 >");
            //strBody.Append(@"<table id='hrdftrtbl' border='0' cellspacing='0' cellpadding='0'><tr><td></td><td><div style='mso-element:header' id=h1><p class=MsoHeader style='text-align:center;font-size:16'>" + headerstring + @"</p></div></td></tr></table>");

            //strBody.Append(htmlString);
            //strBody.Append("<table id='hrdftrtbl' border='0' cellspacing='0' cellpadding='0'><tr><td><div style='mso-element:footer' id=f1><p class=MsoFooter>" + strfooter + @"<span style=mso-tab-count:2'>Page&nbsp;</span>&nbsp;<span style='mso-field-code:"" PAGE ""'></span>&nbsp;of&nbsp;<span style='mso-field-code:"" NUMPAGES ""'>&nbsp;&nbsp;</span>&nbsp;</p></div></td></tr></table>");
            //strBody.Append("</body></html>");   

            StringBuilder sbTop = new System.Text.StringBuilder();
            sbTop.Append(@"
<html 
xmlns:o='urn:schemas-microsoft-com:office:office' 
xmlns:w='urn:schemas-microsoft-com:office:word'
xmlns='http://www.w3.org/TR/REC-html40'>
<head><title></title>

<!--[if gte mso 9]>
<xml>
<w:WordDocument>
<w:View>Print</w:View>
<w:Zoom>90</w:Zoom>
<w:DoNotOptimizeForBrowser/>
</w:WordDocument>
</xml>
<![endif]-->


<style>
p.MsoFooter, li.MsoFooter, div.MsoFooter
{
margin:0in;
margin-bottom:.0001pt;
mso-pagination:widow-orphan;
tab-stops:center 3.0in right 6.0in;
font-size:12.0pt;
}
<style>

<!-- /* Style Definitions */

@page Section1
{
size:8.5in 11.0in; 
margin:1.0in 1.25in 1.0in 1.25in ;
mso-header-margin:.5in;
mso-header:h1;
mso-footer: f1; 
mso-footer-margin:.5in;
}


div.Section1
{
page:Section1;
}

table#hrdftrtbl
{
margin:0in 0in 0in 9in;
}
-->
</style></head>

<body lang=EN-US style='tab-interval:.5in'>
<div class=Section1>");
            sbTop.Append("<br/>" + htmlString);




            sbTop.Append(@"<table id='hrdftrtbl' border='1' cellspacing='0' cellpadding='0'>
<tr><td>
<div style='mso-element:header;font-family:Arial;font-size:8pt' id=h1 >");
            sbTop.Append(headerstring.Replace("$", "") + "<br/>");
            sbTop.Append(@"
</div>
</td>
<td>");

            if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && Encounter_Reviewed_signedDate != "" && Encounter_Reviewed_signedDate != "01-Jan-0001 12:00 AM")
            {
                sbTop.Append(@"<div style='mso-element:footer;font-family:Arial;font-size:9pt' id=f1><p class=MsoFooter style='font-family:Arial;font-size:9pt'>" + strfooterProvider + "<br/>" + strfooterProviderReviewed +
@"<span style=padding-right:10px;mso-tab-count:2'>Page&#32;</span><span style='mso-field-code:"" PAGE "";font-family:Arial;font-size:9pt'>
</span>&nbsp;of&nbsp;<span style='mso-field-code:"" NUMPAGES "";font-family:Arial;font-size:9pt'></span></p></div>
</td></tr>

</table>

</body></html>
");
            }
            else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM")
            {
                sbTop.Append(@"<div style='mso-element:footer;font-family:Arial;font-size:9pt' id=f1><p class=MsoFooter style='font-family:Arial;font-size:9pt'>" + strfooterProvider +
@"<span style=padding-right:10px;mso-tab-count:2'>Page&#32;</span><span style='mso-field-code:"" PAGE "";font-family:Arial;font-size:9pt'>
</span>&nbsp;of&nbsp;<span style='mso-field-code:"" NUMPAGES "";font-family:Arial;font-size:9pt'></span></p></div>
</td></tr>

</table>

</body></html>
");
            }
            else
            {

                sbTop.Append(@"<div style='mso-element:footer;' id=f1><p class=MsoFooter><span style=padding-right:10px;mso-tab-count:2;font-family:Arial;font-size:9pt'>Page&#32;</span><span style='mso-field-code:"" PAGE "";font-family:Arial;font-size:9pt'>
</span>&nbsp;of&nbsp;<span style='mso-field-code:"" NUMPAGES "";font-family:Arial;font-size:9pt'></span>
</p></div>
</td></tr>
</table>
</body></html>
");
            }
            sbTop.Replace("\uFFFD", " ");
            objTimernew.Stop();
            string transformtime = objTimernew.Elapsed.TotalSeconds.ToString();
            string totaltime = "notavailable";
            //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryTimeStamp('" + transformtime + "','" + totaltime + "');", true);
            if (System.Configuration.ConfigurationManager.AppSettings["DocToPdf"].ToUpper() == "Y")
            {
                string path = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"], Session.SessionID + "DocToPdf") + "/" + ClientSession.FacilityName.Replace(",", "") + "_ProgressNotes_" + sNamingConvention + ".doc";
                if (File.Exists(path))
                {
                    File.Delete(path);
                    string pdfPathCheck = path.Replace(".doc", ".pdf").ToString();
                    File.Delete(pdfPathCheck);
                }

                if (!File.Exists(path))
                {
                    string strbody = sbTop.ToString();
                    File.WriteAllText(path, strbody);
                }
                DocToPdf objDoctoPdf = new DocToPdf();
                FileInfo filepath = new FileInfo(path);
                Boolean ConvertCheck = objDoctoPdf.ConvertDocToPdf(filepath);
                if (ConvertCheck)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + NotesName + ".pdf");
                    Response.ContentType = "application/pdf";
                    Response.TransmitFile(path.Replace(".doc", ".pdf"));
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {

                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + NotesName + ".doc");
                Response.ContentType = "application/msword";
                Response.Write(sbTop);
                Response.Flush();
                Response.End();
            }
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Progress Notes : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
        }
        protected void btnPDF_Click(object sender, EventArgs e)
        {
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Progress Notes PDF : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            Stopwatch objTimernew = new Stopwatch();
            objTimernew.Start();
            //string xmlDataFile = strXmlEncounterPath;
            string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml";
            //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);
            // string xsltFile = HttpContext.Current.Server.MapPath("EHR_Progress_Notes.xsl");
            // string xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Progress_Notes.xsl");

            string xsltFile = string.Empty;
            if (Request.QueryString["PhoneEncounter"] != null && Request.QueryString["PhoneEncounter"].Contains("True"))
            {// jira cap-499
                sIsPhoneEncounter = "Y";
                xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Phone_Encounter_Notes.xsl");
            }
            else
            {
                xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Progress_Notes.xsl");
            }

            string OutputNamingConvention = System.Configuration.ConfigurationSettings.AppSettings["ProgressNotesNamingConvention"];
            string sNamingConvention = string.Empty;
            string[] OutputName = OutputNamingConvention.Split('~');
            string NotesName = OutputNamingConvention;
            string result = string.Empty;
            string[] dtFormat = OutputNamingConvention.Split('^');
            string sDateFormat = string.Empty;

            if (dtFormat.Count() > 1)
            {
                if (dtFormat[1].Contains("@"))
                {
                    dtFormat[1] = dtFormat[1].Split('@')[0];
                }
            }
            string[] FormatCase = OutputNamingConvention.Split('@');
            for (int i = 0; i < OutputName.Length; i++)
            {
                if (OutputName[i] != "")
                {
                    result = ExtractBetween(OutputName[i], "[", "]");
                    //if (result.ToUpper().Contains("FACILITY"))
                    //{
                    //    if (FormatCase.Count() > 1)
                    //    {
                    //        if (FormatCase[1].ToUpper().Contains("YES"))
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", "")).ToUpper();
                    //        else
                    //        {
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //        }
                    //        NotesName = NotesName.Replace(FormatCase[1], "");
                    //    }
                    //    else
                    //    {
                    //        NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //    }

                    //}

                    if (result.ToUpper().Contains("HUMAN"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.HumanId.ToString());
                    }
                    if (result.ToUpper().Contains("ENCOUNTER"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.EncounterId.ToString());

                    }
                    if (result.ToUpper().Contains("MEMBER") || result.ToUpper().Contains("LAST") || result.ToUpper().Contains("FIRST") || result.ToUpper().Contains("DOB"))
                    {
                        string sExternalAccNo = string.Empty;
                        string sLastName = string.Empty;
                        string sFirstName = string.Empty;
                        string sDOB = string.Empty;

                        //if (File.Exists(strXmlHumanPath) == true)
                        if (sXMLHumanDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLHumanDoc);
                            //using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                            {
                                if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes.Count > 0)
                                {
                                    if (result.ToUpper().Contains("MEMBER"))
                                    {
                                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value != "")
                                        {
                                            sExternalAccNo = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value.ToString();
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", sExternalAccNo);
                                    }
                                    else if (result.ToUpper().Contains("LAST"))
                                    {
                                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != "")
                                        {
                                            if (FormatCase.Count() > 1)
                                            {
                                                if (FormatCase[1].ToUpper().Contains("YES"))
                                                    sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToUpper().ToString();
                                                else
                                                {
                                                    sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                                }
                                                NotesName = NotesName.Replace(FormatCase[1], "");
                                            }
                                            else
                                            {
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                            }
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", (sLastName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                    }
                                    else if (result.ToUpper().Contains("FIRST"))
                                    {
                                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != "")
                                        {
                                            if (FormatCase.Count() > 1)
                                            {
                                                if (FormatCase[1].ToUpper().Contains("YES"))
                                                    sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToUpper().ToString();
                                                else
                                                {
                                                    sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                                }
                                                NotesName = NotesName.Replace(FormatCase[1], "");
                                            }
                                            else
                                            {
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                            }
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", (sFirstName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                    }
                                    else if (result.ToUpper().Contains("DOB") || (OutputName[i].ToUpper().Contains("DATE") && OutputName[i].ToUpper().Contains("BIRTH")))
                                    {
                                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value != "")
                                        {
                                            if (dtFormat.Count() > 1)
                                            {
                                                if (dtFormat[1] == "yyyyMMdd")
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                                else if (dtFormat[1] == "MMddyyyy")
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyyyy");
                                                else if (dtFormat[1] == "MMddyy")
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyy");
                                                else if (dtFormat[1] == "yyMMdd")
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyMMdd");
                                                else
                                                {
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                                }
                                                NotesName = NotesName.Replace(dtFormat[1], "");
                                            }
                                            else
                                            {
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            }
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", sDOB);
                                    }
                                }
                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }

                    }
                    if ((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE")) || result.ToUpper().Contains("FACILITY"))
                    {
                        //if (File.Exists(strXmlEncounterPath) == true)
                        if (sXMLEncounterDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlEncounterPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLEncounterDoc);
                            //using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            string sDOS = string.Empty;
                            if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                            {
                                if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes.Count > 0)
                                {
                                    if (((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE"))) && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value != "")
                                    {
                                        if (dtFormat.Count() > 1)
                                        {
                                            if (dtFormat[1] == "yyyyMMdd")
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                            else if (dtFormat[1] == "MMddyyyy")
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyyyy");
                                            else if (dtFormat[1] == "MMddyy")
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyy");
                                            else if (dtFormat[1] == "yyMMdd")
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyMMdd");
                                            else
                                            {
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                            }
                                            NotesName = NotesName.Replace(dtFormat[1], "");
                                        }
                                        else
                                        {
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", sDOS);
                                    }

                                    else if (result.ToUpper().Contains("FACILITY") && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Trim() != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", "")).ToUpper();
                                            else
                                            {
                                                NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                        }
                                    }
                                    else
                                    {
                                        NotesName = NotesName.Replace("[" + result + "]", "");
                                    }

                                }

                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }
                    }
                }
            }
            if (NotesName.Contains('['))
            {
                string[] sName = NotesName.Split('~');

                for (int i = 0; i < sName.Length; i++)
                {
                    if (sName[i] != "")
                    {
                        if (sName[i].Contains("["))
                        {
                            result = ExtractBetween(sName[i], "[", "]");
                            NotesName = NotesName.Replace("[" + result + "]", "");
                        }
                    }
                }
            }
            //NotesName = NotesName.Replace("~", "").Replace("__", "_").Replace("^", "").Replace("@", "");
            NotesName= UtilityMngr.ReplaceSpecialCharaterInFileName(NotesName);
            string WordOutputName = NotesName + ".html";
            string outputDocument = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], WordOutputName);
            #region HPvalue
            ////for hp value
            //EncounterManager objManager = new EncounterManager();
            //string[] arry = objManager.GetInusranceDetails(ClientSession.HumanId);
            //string sPriPlan = string.Empty;
            //string sPriCarrier = string.Empty;
            //if (arry[1] != null)
            //{
            //    if (arry[0] == "PRIMARY")
            //    {
            //        sPriPlan = arry[2].ToString();
            //        if (File.Exists(xmlDataFile) == true)
            //        {
            //            XmlDocument itemDoc = new XmlDocument();
            //            XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //            itemDoc.Load(XmlText);
            //            XmlText.Close();

            //            XmlNodeList xmlPatientName = itemDoc.GetElementsByTagName("HP");
            //            xmlPatientName[0].Attributes[0].Value = sPriPlan;
            //            itemDoc.Save(strXmlFilePath);
            //        }
            //    }
            //}
            ////
            #endregion
            //Jira #CAP-344 - OldCode
            //DataSet ds;
            //XmlDataDocument xmlDoc;
            //XslCompiledTransform xslTran;
            //XmlElement root;
            //XPathNavigator nav;
            //XmlTextWriter writer;
            //XsltSettings settings = new XsltSettings(true, false);
            //ds = new DataSet();
            ////ds.ReadXml(xmlDataFile);
            ////ds.ReadXml(new XmlTextReader(new StringReader(sXMLEncounterDoc)));

            //StringBuilder sb = new StringBuilder();
            //sb.Append(sXMLEncounterDoc.ToString().Replace("</notes>", "").Replace("</Modules>", ""));

            //string SUB = sXMLHumanDoc.ToString().Substring(0, sXMLHumanDoc.LastIndexOf("?>") + 2);

            //sb.Append(sXMLHumanDoc.ToString().Replace(SUB, "").Replace("<notes>", "").Replace("<Modules>", ""));
            //ds.ReadXml(new XmlTextReader(new StringReader(sb.ToString())));
            //xmlDoc = new XmlDataDocument(ds);
            //xslTran = new XslCompiledTransform();
            //using (var stream = File.Open(xsltFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            //{
            //    // xslTran.Load(xsltFile);
            //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Progress Notes PDF XSLT Load : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            //    xslTran.Load(xsltFile, settings, new XmlUrlResolver());
            //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Progress Notes PDF XSLT Load : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            //}
            //root = xmlDoc.DocumentElement;
            //nav = root.CreateNavigator();
            //if (File.Exists(outputDocument))
            //{
            //    File.Delete(outputDocument);
            //}
            //writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8);
            //xslTran.Transform(nav, writer);
            //writer.Close();
            //writer = null;
            //nav = null;
            //root = null;
            //xmlDoc = null;
            //ds = null;
            //xslTran = null;

            //Jira #CAP-344 - NewCode
            UtilityManager.PrintPDFUsingXSLT(sXMLEncounterDoc, sXMLHumanDoc, xsltFile, outputDocument, sGroup_ID_Log);
            System.IO.FileInfo file = new System.IO.FileInfo(outputDocument);

            string Encounter_signedDate = "";
            string Encounter_Provider_Name = "";
            string Encounter_Reviewed_signedDate = "";
            string Encounter_Reviewed_Name = "";
            string Encounter_Reviewed_Id = "";
            string sIsphoneEncounter = "";
            string sCreatedBy = "";

            TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
            XDocument xmlDocumentType = XDocument.Load(EncXMLContent);
            foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value);
                    Encounter_Reviewed_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm tt");

                    DateTime dtPro = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                    Encounter_signedDate = UtilityManager.ConvertToLocal(dtPro).ToString("dd-MMM-yyyy hh:mm tt");

                    Encounter_Reviewed_Id = Encounter.Attribute("Encounter_Provider_Review_ID").Value;
                    sIsphoneEncounter = Encounter.Attribute("Is_Phone_Encounter").Value;
                    sCreatedBy = Encounter.Attribute("Created_By").Value;
                }

                //if (Encounter_signedDate == "" || Encounter_signedDate == "01-Jan-0001 12:00:00 AM")
                //{
                // foreach (XElement Encounter in elements.Elements())
                // {
                //     DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                //     Encounter_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm:ss tt");
                //}
                //}
            }
            //Provider Name 
            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    Encounter_Provider_Name = Encounter.Value;
                    break;
                }
                break;
            }
            //Provider Reviewed Name 
            if (Encounter_Reviewed_Id != "")
            {
                string xmlFilepathUser = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
                if (File.Exists(xmlFilepathUser))
                {
                    XmlDocument xdoc = new XmlDocument();
                    XmlTextReader itext = new XmlTextReader(xmlFilepathUser);
                    xdoc.Load(itext);
                    itext.Close();
                    XmlNodeList xnodelst = xdoc.GetElementsByTagName("User");
                    if (xnodelst != null && xnodelst.Count > 0)
                    {
                        foreach (XmlNode xnode in xnodelst)
                        {
                            if (xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() != "0" && xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() == Encounter_Reviewed_Id)
                            {
                                Encounter_Reviewed_Name = xnode.Attributes.GetNamedItem("person_name").Value;
                            }
                        }
                    }
                }
            }


            string htmlString = System.IO.File.ReadAllText(outputDocument);
            //Jira CAP-1015
            htmlString = htmlString.Replace("amp;", "");
            

            string sProHeader = System.Configuration.ConfigurationSettings.AppSettings["ProgressNotesMainHeader"];
            string headerstring = string.Empty;
            if (sProHeader == "Y")
            {
                int index = htmlString.IndexOf("<table");
                int index1 = htmlString.LastIndexOf("</thead></table>");

                headerstring = htmlString.Substring(index, index1);

                string LogoPath = System.Configuration.ConfigurationSettings.AppSettings["ProgressNotesLogoPath"];
                string headerlogo = string.Empty;
                if (LogoPath == string.Empty)
                {
                    headerlogo = "<table><thead><tr><td></td>";
                }
                else
                {
                    headerlogo = "<table width='100%' height='30%' cellpadding='0' cellspacing='0' border='0'><thead><tr valign='middle'><td valign='top' width='10%'><img src='" + LogoPath + "'  alt='logo' />";
                }
                headerstring = headerstring.Replace("<table><thead><tr>", "");
                headerstring = headerlogo + headerstring + "</thead></table>";
                headerstring.Replace("height=' 30% '", "");
                htmlString = htmlString.Substring(index1 + 16, htmlString.Length - index1 - 16);
            }
            else
            {
                int index = htmlString.IndexOf("<table");
                int index1 = htmlString.IndexOf("</table>");
                headerstring = htmlString.Substring(index, index1);
                headerstring = headerstring + "</table>";
                headerstring.Replace("height=' 30% '", "");
                //htmlString = htmlString.Replace(headerstring, "");
                htmlString = htmlString.Substring(index1 + 8, htmlString.Length - index1 - 8);
            }


            // string strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;

            string strfooterProvider = "";
            if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && sIsphoneEncounter != "Y")
            {
                strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
            }
            else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && sIsphoneEncounter == "Y")
            {
                if (Encounter_Provider_Name != "")
                {
                    strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
                }
                else
                {
                    strfooterProvider = "Electronically Signed by " + sCreatedBy + " at " + Encounter_signedDate;
                }
            }
            //string strfooterProviderReviewed = "I " + Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate +
            //     " have reviewed the chart and agree with the management plan with the changes to the plan as indicated.";

            string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
            StaticLookupManager staticMngr = new StaticLookupManager();
            string strfooterProviderReviewed = string.Empty;
            IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
            if (CommonList.Count > 0)
                strfooterProviderReviewed = CommonList[0].Value.Replace("<Physician>", Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate).Replace("|", "");


            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            var strBody = new StringBuilder();

            string sbTop = "";

            sbTop = sbTop + htmlString;



            string strfooterF = "";

            string strfooterPA = "";
            string strfooterP = "";
            if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && Encounter_Reviewed_signedDate != "" && Encounter_Reviewed_signedDate != "01-Jan-0001 12:00 AM")
            {
                strfooterPA = strfooterProvider;

                strfooterP = strfooterProviderReviewed;
                strfooterF = " ";
                //  strfooterProvider = strfooterProvider + "<br/>" + strfooterProviderReviewed;
            }
            else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM")
            {
                strfooterF = strfooterProvider;
            }
            else
            {


                strfooterF = "";

            }




            string strHtml = string.Empty;
            string pdfFileName = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], NotesName + "test" + ".pdf");//System.Configuration.ConfigurationSettings.AppSettings["XMLPath"] + NotesName + "test" + ".pdf";

            string pdfFileNamewithHeader = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], NotesName + ".pdf");//)System.Configuration.ConfigurationSettings.AppSettings["XMLPath"] + NotesName + ".pdf";

            if (htmlString.Length > 0)
            {
                try
                {
                    CreatePDFFromHTMLFile(sbTop, pdfFileName);
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToUpper().Contains("NO PAGES"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('1011187'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;

                    }
                    else
                        throw ex;
                }

                //StringReader sr = new StringReader(sbTop.ToString());
                //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0.0f);
                ////pdfDoc.attribute("AutoPrint").Value = true;
                //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                //pdfDoc.Open();
                //htmlparser.Parse(sr);
                //pdfDoc.Close();




                var reader = new PdfReader(pdfFileName);

                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {

                    // var document = new Document(reader.GetPageSizeWithRotation(1));
                    var document1 = new Document(new Rectangle(625, 975));
                    //Document document1 = new Document(new Rectangle(650, 975)); 
                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = reader.NumberOfPages;

                    //  string header = headerstring;
                    string[] Header = headerstring.Split(new string[] { "#$%" }, StringSplitOptions.None);
                    sProHeader = "N";// System.Configuration.ConfigurationSettings.AppSettings["WellnessNotesMainHeader"];

                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        var importedPage = writerpdf.GetImportedPage(reader, i);


                        var con = writerpdf.DirectContent;

                        // contentByte.BeginText();

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();

                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 9);


                        //if (strfooterProvider != "")
                        //{
                        //    con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProvider, 30, 10, 0);
                        //    //if (strfooterProviderReviewed != "")
                        //    //    con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProviderReviewed, 30, 10, 0);
                        //    con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 10, 0);
                        //}
                        //else if (strfooterF != "")
                        //{
                        //    con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 10, 0);
                        //    con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 10, 0);
                        //}
                        //else
                        //{
                        //    con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 10, 0);
                        //}
                        if (strfooterF == "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else if (strfooterPA != "" && strfooterP != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterPA, 30, 30, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterP, 30, 20, 0);

                            //ColumnText ct = new ColumnText(con);
                            //ct.SetSimpleColumn(new Phrase(new Chunk(strfooterPA, FontFactory.GetFont(FontFactory.TIMES_ITALIC, 9, Font.NORMAL))),
                            //                   35, 77, 530, 36, 14, iTextSharp.text.Element.ALIGN_LEFT | iTextSharp.text.Element.ALIGN_BOTTOM);
                            //ct.Go();

                            //ct = new ColumnText(con);
                            //ct.SetSimpleColumn(new Phrase(new Chunk(strfooterP, FontFactory.GetFont(FontFactory.TIMES_ITALIC, 9, Font.NORMAL))),
                            //                   35, 65, 530, 36, 14, iTextSharp.text.Element.ALIGN_LEFT | iTextSharp.text.Element.ALIGN_BOTTOM);
                            //ct.Go(); 

                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }

                        //else
                        //{
                        //    con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 10, 0);
                        //    con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 10, 0);
                        //}


                        #region Column1
                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;

                        int iPCPYaxices = 0;
                        int iOrderingProviderYaxices = 0;
                        for (int j = 1; j < Header.Length; j += 4)
                        {
                            con.SetFontAndSize(baseFont1, 10);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            con.ShowText(Header[j].Split(':')[0].ToString());
                            X += 103;
                            con.SetColorFill(BaseColor.BLUE);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            if (Header[j].Split(':')[0].ToUpper().Contains("PATIENT"))
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() + " " : string.Empty) + "\n");
                            }
                            // Jira CAP-981 for else if (pcp) contion
                            else if (Header[j].Split(':')[0].ToUpper().Contains("PCP") && (Header[j].Split(':')[1].Length > 29))
                            {
                                con.ShowText(": " + (Header[j].Split(':')[1].Substring(0, 28)));

                                con.SetTextMatrix(pageSize.GetLeft(X) - 32, pageSize.GetTop(Y) - 10);
                                con.ShowText(Header[j].Split(':')[1].Substring(28, Header[j].Split(':')[1].Length - 28));
                                iPCPYaxices = 10;
                                Y = Y + iPCPYaxices;
                            }
                            // Jira CAP-981 for else if (Ordering Provider) contion
                            else if (Header[j].Split(':')[0].ToUpper().Contains("ORDERING PROVIDER") && (Header[j].Split(':')[1].Length > 29))
                            {
                                con.ShowText(": " + (Header[j].Split(':')[1].Substring(0, 28)));

                                con.SetTextMatrix(pageSize.GetLeft(X) - 32, pageSize.GetTop(Y) - 10);
                                con.ShowText(Header[j].Split(':')[1].Substring(28, Header[j].Split(':')[1].Length - 28));
                                iOrderingProviderYaxices = 10;
                                Y = Y + iOrderingProviderYaxices;
                            }
                            else
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() : string.Empty) + "\n");
                            }

                            X -= 103;
                            Y += 10;
                        }

                        // Jira CAP-981 - Start
                        Y = Y - iPCPYaxices - iOrderingProviderYaxices;
                        // Jira CAP-981 - End
                        #endregion


                        Y += 10;
                        con.MoveTo(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                        con.LineTo(pageSize.GetRight(X) + 40, pageSize.GetTop(Y));
                        con.Stroke();

                        #region Column2
                        X = 360f;
                        Y = 20f;

                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }
                        else
                            Y += 10;
                        for (int j = 3; j < Header.Length; j += 4)
                        {
                            con.SetFontAndSize(baseFont1, 10);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 20, pageSize.GetTop(Y));
                            //con.SetTextMatrix(pageSize.GetLeft(X) - 50, pageSize.GetTop(Y));//For Bug ID: 70064
                            con.ShowText(Header[j].Split(':')[0].ToString());
                            X += 97;
                            con.SetColorFill(BaseColor.BLUE);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            if (Header[j].Split(':')[0].ToUpper().Contains("ENCOUNTER"))
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length >= 3 ? Header[j].Split(':')[1].ToString() + ":" + Header[j].Split(':')[2].ToString() + ":" + Header[j].Split(':')[3].ToString() : string.Empty) + "\n");
                            }

                            else if (Header[j].Split(':')[0].ToUpper().Contains("FACILITY"))
                            {
                                if (Header[j].Split(':').Length > 1)
                                {
                                    string[] facility = Header[j].Split(':')[1].Split(',');
                                    string fac = "";
                                    if (facility.Length > 0)
                                    {
                                        for (int k = 0; k < facility.Length; k++)
                                        {
                                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                                            if (k == 0)
                                            {
                                                con.ShowText(":" + facility[k].ToString() + ",");
                                                Y += 10;

                                            }
                                            else if (k == facility.Length - 1)
                                            {
                                                fac = fac + facility[k] + ".";
                                            }
                                            else
                                            {
                                                fac = fac + facility[k] + ",";


                                            }
                                            // X -= 97;

                                        }
                                        con.SetTextMatrix(pageSize.GetLeft(X) - 37, pageSize.GetTop(Y));
                                        con.ShowText(fac + "\n");

                                    }
                                }
                            }

                            else
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() : string.Empty) + "\n");
                            }
                            X -= 97;
                            Y += 10;
                        }



                        #endregion
                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();
                        if (sProHeader == "Y")
                        {

                            con.AddTemplate(importedPage, 0, 0);
                        }
                        else
                            con.AddTemplate(importedPage, 0, 60);

                    }

                    document1.Close();
                    writerpdf.Close();
                    fileStream.Close();
                    fileStream.Dispose();
                }
                if (File.Exists(pdfFileName))
                {
                    File.Delete(pdfFileName);
                    // string pdfPathCheck = path.Replace(".doc", ".pdf").ToString();

                }
            }
            else
            {
                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {

                    // var document = new Document(reader.GetPageSizeWithRotation(1));


                    var document1 = new Document(new Rectangle(625, 975));
                    //Document document1 = new Document(new Rectangle(650, 975)); 
                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = 1;

                    //  string header = headerstring;
                    string[] Header = headerstring.Split(new string[] { "#$%" }, StringSplitOptions.None);
                    sProHeader = "N";// System.Configuration.ConfigurationSettings.AppSettings["WellnessNotesMainHeader"];

                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        //var importedPage = writerpdf.GetImportedPage(reader, i);


                        var con = writerpdf.DirectContent;

                        // contentByte.BeginText();

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();
                        if (sProHeader == "Y")
                        {


                            //float U = 75;
                            //float S = 60f;


                        }
                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 8);

                        if (strfooterProvider != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProvider, 30, 30, 0);
                            if (strfooterProviderReviewed != "")
                                con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProviderReviewed, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else if (strfooterF != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }


                        Y += 10;
                        con.MoveTo(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));


                        #region Column1
                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;
                        int iPCPYaxices = 0;
                        int iOrderingProviderYaxices = 0;
                        for (int j = 1; j < Header.Length; j += 4)
                        {
                            con.SetFontAndSize(baseFont1, 10);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            con.ShowText(Header[j].Split(':')[0].ToString());
                            X += 103;
                            con.SetColorFill(BaseColor.BLUE);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            if (Header[j].Split(':')[0].ToUpper().Contains("PATIENT"))
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() + " " : string.Empty) + "\n");
                            }
                            // Jira CAP-981 for else if (pcp) contion
                            else if (Header[j].Split(':')[0].ToUpper().Contains("PCP") && (Header[j].Split(':')[1].Length > 29))
                            {
                                con.ShowText(": " + (Header[j].Split(':')[1].Substring(0, 28)));

                                con.SetTextMatrix(pageSize.GetLeft(X) - 32, pageSize.GetTop(Y) - 10);
                                con.ShowText(Header[j].Split(':')[1].Substring(28, Header[j].Split(':')[1].Length - 28));
                                iPCPYaxices = 10;
                                Y = Y + iPCPYaxices;
                            }
                            // Jira CAP-981 for else if (Ordering Provider) contion
                            else if (Header[j].Split(':')[0].ToUpper().Contains("ORDERING PROVIDER") && (Header[j].Split(':')[1].Length > 29))
                            {
                                con.ShowText(": " + (Header[j].Split(':')[1].Substring(0, 28)));

                                con.SetTextMatrix(pageSize.GetLeft(X) - 32, pageSize.GetTop(Y) - 10);
                                con.ShowText(Header[j].Split(':')[1].Substring(28, Header[j].Split(':')[1].Length - 28));
                                iOrderingProviderYaxices = 10;
                                Y = Y + iOrderingProviderYaxices;
                            }
                            else
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() : string.Empty) + "\n");
                            }

                            X -= 103;
                            Y += 10;
                        }

                        // Jira CAP-981 - Start
                        Y = Y - iPCPYaxices - iOrderingProviderYaxices;
                        // Jira CAP-981 - End
                        #endregion


                        con.LineTo(pageSize.GetRight(X) + 40, pageSize.GetTop(Y));
                        con.Stroke();

                        #region Column2
                        X = 360f;
                        Y = 20f;

                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;
                        for (int j = 3; j < Header.Length; j += 4)
                        {
                            con.SetFontAndSize(baseFont1, 10);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 20, pageSize.GetTop(Y));
                            //con.SetTextMatrix(pageSize.GetLeft(X) - 50, pageSize.GetTop(Y));//For Bug ID: 70064
                            con.ShowText(Header[j].Split(':')[0].ToString());
                            X += 97;
                            con.SetColorFill(BaseColor.BLUE);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            if (Header[j].Split(':')[0].ToUpper().Contains("ENCOUNTER"))
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length >= 3 ? Header[j].Split(':')[1].ToString() + ":" + Header[j].Split(':')[2].ToString() + ":" + Header[j].Split(':')[3].ToString() : string.Empty) + "\n");
                            }

                            else if (Header[j].Split(':')[0].ToUpper().Contains("FACILITY"))
                            {
                                if (Header[j].Split(':').Length > 1)
                                {
                                    string[] facility = Header[j].Split(':')[1].Split(',');
                                    string fac = "";
                                    if (facility.Length > 0)
                                    {
                                        for (int k = 0; k < facility.Length; k++)
                                        {
                                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                                            if (k == 0)
                                            {
                                                con.ShowText(":" + facility[k].ToString() + ",");
                                                Y += 10;

                                            }
                                            else if (k == facility.Length - 1)
                                            {
                                                fac = facility[k] + ".";
                                            }
                                            else
                                            {
                                                fac = facility[k] + ",";


                                            }
                                            // X -= 97;

                                        }
                                        con.SetTextMatrix(pageSize.GetLeft(X) - 37, pageSize.GetTop(Y));
                                        con.ShowText(fac + "\n");

                                    }
                                }
                            }

                            else
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() : string.Empty) + "\n");
                            }
                            X -= 97;
                            Y += 10;
                        }



                        #endregion
                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();

                    }

                    document1.Close();
                    writerpdf.Close();
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }




            Response.ContentType = "application/x-download";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", NotesName + ".pdf"));
            Response.WriteFile(pdfFileNamewithHeader);

            Response.Flush();
            System.IO.File.Delete(pdfFileNamewithHeader);
            Response.End();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Progress Notes PDF : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
        }


        protected void btnsendFax_Click(object sender, EventArgs e)
        {
            //Cap - 1414, 1415, 1449
            string sFaxSubject = string.Empty;
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            if (sIsAkidoEncounter == "false") 
            { 
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Send FAX : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            
            Stopwatch objTimernew = new Stopwatch();
            objTimernew.Start();
            //string xmlDataFile = strXmlEncounterPath;
            string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml";
            //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);
            // string xsltFile = HttpContext.Current.Server.MapPath("EHR_Progress_Notes.xsl");
            // string xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Progress_Notes.xsl");

            string xsltFile = string.Empty;
            if (Request.QueryString["PhoneEncounter"] != null && Request.QueryString["PhoneEncounter"].Contains("True"))
            {// jira cap-499
                sIsPhoneEncounter = "Y";
                xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Phone_Encounter_Notes.xsl");
            }
            else
            {
                xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Progress_Notes.xsl");
            }

            string OutputNamingConvention = System.Configuration.ConfigurationSettings.AppSettings["ProgressNotesNamingConvention"];
            string sNamingConvention = string.Empty;
            string[] OutputName = OutputNamingConvention.Split('~');
            string NotesName = OutputNamingConvention;
            string result = string.Empty;
            string[] dtFormat = OutputNamingConvention.Split('^');
            string sDateFormat = string.Empty;

            if (dtFormat.Count() > 1)
            {
                if (dtFormat[1].Contains("@"))
                {
                    dtFormat[1] = dtFormat[1].Split('@')[0];
                }
            }
            string[] FormatCase = OutputNamingConvention.Split('@');
            for (int i = 0; i < OutputName.Length; i++)
            {
                if (OutputName[i] != "")
                {
                    result = ExtractBetween(OutputName[i], "[", "]");
                    //if (result.ToUpper().Contains("FACILITY"))
                    //{
                    //    if (FormatCase.Count() > 1)
                    //    {
                    //        if (FormatCase[1].ToUpper().Contains("YES"))
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", "")).ToUpper();
                    //        else
                    //        {
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //        }
                    //        NotesName = NotesName.Replace(FormatCase[1], "");
                    //    }
                    //    else
                    //    {
                    //        NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //    }

                    //}

                    if (result.ToUpper().Contains("HUMAN"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.HumanId.ToString());
                    }
                    if (result.ToUpper().Contains("ENCOUNTER"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.EncounterId.ToString());

                    }
                    if (result.ToUpper().Contains("MEMBER") || result.ToUpper().Contains("LAST") || result.ToUpper().Contains("FIRST") || result.ToUpper().Contains("DOB"))
                    {
                        string sExternalAccNo = string.Empty;
                        string sLastName = string.Empty;
                        string sFirstName = string.Empty;
                        string sDOB = string.Empty;

                        //if (File.Exists(strXmlHumanPath) == true)
                        if (sXMLHumanDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLHumanDoc);
                            //using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //itemDoc.Load(fs);

                            //XmlText.Close();
                            if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                            {
                                if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes.Count > 0)
                                {
                                    if (result.ToUpper().Contains("MEMBER"))
                                    {
                                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value != "")
                                        {
                                            sExternalAccNo = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value.ToString();
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", sExternalAccNo);
                                    }
                                    else if (result.ToUpper().Contains("LAST"))
                                    {
                                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != "")
                                        {
                                            if (FormatCase.Count() > 1)
                                            {
                                                if (FormatCase[1].ToUpper().Contains("YES"))
                                                    sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToUpper().ToString();
                                                else
                                                {
                                                    sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                                }
                                                NotesName = NotesName.Replace(FormatCase[1], "");
                                            }
                                            else
                                            {
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                            }
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", (sLastName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                    }
                                    else if (result.ToUpper().Contains("FIRST"))
                                    {
                                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != "")
                                        {
                                            if (FormatCase.Count() > 1)
                                            {
                                                if (FormatCase[1].ToUpper().Contains("YES"))
                                                    sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToUpper().ToString();
                                                else
                                                {
                                                    sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                                }
                                                NotesName = NotesName.Replace(FormatCase[1], "");
                                            }
                                            else
                                            {
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                            }
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", (sFirstName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                    }
                                    else if (result.ToUpper().Contains("DOB") || (OutputName[i].ToUpper().Contains("DATE") && OutputName[i].ToUpper().Contains("BIRTH")))
                                    {
                                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value != "")
                                        {
                                            if (dtFormat.Count() > 1)
                                            {
                                                if (dtFormat[1] == "yyyyMMdd")
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                                else if (dtFormat[1] == "MMddyyyy")
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyyyy");
                                                else if (dtFormat[1] == "MMddyy")
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyy");
                                                else if (dtFormat[1] == "yyMMdd")
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyMMdd");
                                                else
                                                {
                                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                                }
                                                NotesName = NotesName.Replace(dtFormat[1], "");
                                            }
                                            else
                                            {
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            }
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", sDOB);
                                    }
                                }
                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }

                    }
                    if ((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE")) || result.ToUpper().Contains("FACILITY"))
                    {
                        //if (File.Exists(strXmlEncounterPath) == true)
                        if (sXMLEncounterDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            // XmlTextReader XmlText = new XmlTextReader(strXmlEncounterPath);
                            itemDoc.LoadXml(sXMLEncounterDoc);
                            // itemDoc.Load(XmlText);
                            //using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            string sDOS = string.Empty;
                            if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                            {
                                if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes.Count > 0)
                                {
                                    if (((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE"))) && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value != "")
                                    {
                                        if (dtFormat.Count() > 1)
                                        {
                                            if (dtFormat[1] == "yyyyMMdd")
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                            else if (dtFormat[1] == "MMddyyyy")
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyyyy");
                                            else if (dtFormat[1] == "MMddyy")
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyy");
                                            else if (dtFormat[1] == "yyMMdd")
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyMMdd");
                                            else
                                            {
                                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                            }
                                            NotesName = NotesName.Replace(dtFormat[1], "");
                                        }
                                        else
                                        {
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        }
                                        NotesName = NotesName.Replace("[" + result + "]", sDOS);
                                    }
                                    else if (result.ToUpper().Contains("FACILITY") && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Trim() != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", "")).ToUpper();
                                            else
                                            {
                                                NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                        }
                                    }
                                    else
                                    {
                                        NotesName = NotesName.Replace("[" + result + "]", "");
                                    }

                                }

                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }
                    }
                }
            }
            if (NotesName.Contains('['))
            {
                string[] sName = NotesName.Split('~');

                for (int i = 0; i < sName.Length; i++)
                {
                    if (sName[i] != "")
                    {
                        if (sName[i].Contains("["))
                        {
                            result = ExtractBetween(sName[i], "[", "]");
                            NotesName = NotesName.Replace("[" + result + "]", "");
                        }
                    }
                }
            }
            //NotesName = NotesName.Replace("~", "").Replace("__", "_").Replace("^", "").Replace("@", "");
            NotesName = UtilityMngr.ReplaceSpecialCharaterInFileName(NotesName);
            string WordOutputName = NotesName + ".html";
            string outputDocument = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], WordOutputName);
            #region HPvalue
            ////for hp value
            //EncounterManager objManager = new EncounterManager();
            //string[] arry = objManager.GetInusranceDetails(ClientSession.HumanId);
            //string sPriPlan = string.Empty;
            //string sPriCarrier = string.Empty;
            //if (arry[1] != null)
            //{
            //    if (arry[0] == "PRIMARY")
            //    {
            //        sPriPlan = arry[2].ToString();
            //        if (File.Exists(xmlDataFile) == true)
            //        {
            //            XmlDocument itemDoc = new XmlDocument();
            //            XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //            itemDoc.Load(XmlText);
            //            XmlText.Close();

            //            XmlNodeList xmlPatientName = itemDoc.GetElementsByTagName("HP");
            //            xmlPatientName[0].Attributes[0].Value = sPriPlan;
            //            itemDoc.Save(strXmlFilePath);
            //        }
            //    }
            //}
            ////
            #endregion

            //Jira #CAP-344 - OldCode
            //DataSet ds;
            //XmlDataDocument xmlDoc;
            //XslCompiledTransform xslTran;
            //XmlElement root;
            //XPathNavigator nav;
            //XmlTextWriter writer;
            //XsltSettings settings = new XsltSettings(true, false);
            //ds = new DataSet();
            ////ds.ReadXml(xmlDataFile);
            //// ds.ReadXml(new XmlTextReader(new StringReader(sXMLEncounterDoc)));
            //StringBuilder sb = new StringBuilder();
            //sb.Append(sXMLEncounterDoc.ToString().Replace("</notes>", "").Replace("</Modules>", ""));

            //string SUB = sXMLHumanDoc.ToString().Substring(0, sXMLHumanDoc.LastIndexOf("?>") + 2);

            //sb.Append(sXMLHumanDoc.ToString().Replace(SUB, "").Replace("<notes>", "").Replace("<Modules>", ""));
            //ds.ReadXml(new XmlTextReader(new StringReader(sb.ToString())));

            //xmlDoc = new XmlDataDocument(ds);
            //xslTran = new XslCompiledTransform();
            //using (var stream = File.Open(xsltFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            //{
            //    // xslTran.Load(xsltFile);
            //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Send FAX XSLT Load : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            //    xslTran.Load(xsltFile, settings, new XmlUrlResolver());
            //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Send FAX XSLT Load : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            //}
            //root = xmlDoc.DocumentElement;
            //nav = root.CreateNavigator();
            //if (File.Exists(outputDocument))
            //{
            //    File.Delete(outputDocument);
            //}
            //writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8);
            //xslTran.Transform(nav, writer);
            //writer.Close();
            //writer = null;
            //nav = null;
            //root = null;
            //xmlDoc = null;
            //ds = null;
            //xslTran = null;

            //Jira #CAP-344 - NewCode
            UtilityManager.PrintPDFUsingXSLT(sXMLEncounterDoc, sXMLHumanDoc, xsltFile, outputDocument, sGroup_ID_Log);
            System.IO.FileInfo file = new System.IO.FileInfo(outputDocument);

            string Encounter_signedDate = "";
            string Encounter_Provider_Name = "";
            string Encounter_Reviewed_signedDate = "";
            string Encounter_Reviewed_Name = "";
            string Encounter_Reviewed_Id = "";
            TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
            XDocument xmlDocumentType = XDocument.Load(EncXMLContent);

            foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value);
                    Encounter_Reviewed_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm tt");

                    DateTime dtPro = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                    Encounter_signedDate = UtilityManager.ConvertToLocal(dtPro).ToString("dd-MMM-yyyy hh:mm tt");

                    Encounter_Reviewed_Id = Encounter.Attribute("Encounter_Provider_Review_ID").Value;

                }

                //if (Encounter_signedDate == "" || Encounter_signedDate == "01-Jan-0001 12:00:00 AM")
                //{
                // foreach (XElement Encounter in elements.Elements())
                // {
                //     DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                //     Encounter_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm:ss tt");
                //}
                //}
            }
            //Provider Name 
            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    Encounter_Provider_Name = Encounter.Value;
                    break;
                }
            }
            //Provider Reviewed Name 
            if (Encounter_Reviewed_Id != "")
            {
                string xmlFilepathUser = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
                if (File.Exists(xmlFilepathUser))
                {
                    XmlDocument xdoc = new XmlDocument();
                    XmlTextReader itext = new XmlTextReader(xmlFilepathUser);
                    xdoc.Load(itext);
                    itext.Close();
                    XmlNodeList xnodelst = xdoc.GetElementsByTagName("User");
                    if (xnodelst != null && xnodelst.Count > 0)
                    {
                        foreach (XmlNode xnode in xnodelst)
                        {
                            if (xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() != "0" && xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() == Encounter_Reviewed_Id)
                            {
                                Encounter_Reviewed_Name = xnode.Attributes.GetNamedItem("person_name").Value;
                            }
                        }
                    }
                }
            }


            string htmlString = System.IO.File.ReadAllText(outputDocument);
            //Jira CAP-1015
            htmlString = htmlString.Replace("amp;", "");


            string sProHeader = System.Configuration.ConfigurationSettings.AppSettings["ProgressNotesMainHeader"];
            string headerstring = string.Empty;
            if (sProHeader == "Y")
            {
                int index = htmlString.IndexOf("<table");
                int index1 = htmlString.LastIndexOf("</thead></table>");

                headerstring = htmlString.Substring(index, index1);

                string LogoPath = System.Configuration.ConfigurationSettings.AppSettings["ProgressNotesLogoPath"];
                string headerlogo = string.Empty;
                if (LogoPath == string.Empty)
                {
                    headerlogo = "<table><thead><tr><td></td>";
                }
                else
                {
                    headerlogo = "<table width='100%' height='30%' cellpadding='0' cellspacing='0' border='0'><thead><tr valign='middle'><td valign='top' width='10%'><img src='" + LogoPath + "'  alt='logo' />";
                }
                headerstring = headerstring.Replace("<table><thead><tr>", "");
                headerstring = headerlogo + headerstring + "</thead></table>";
                headerstring.Replace("height=' 30% '", "");
                htmlString = htmlString.Substring(index1 + 16, htmlString.Length - index1 - 16);
            }
            else
            {
                int index = htmlString.IndexOf("<table");
                int index1 = htmlString.IndexOf("</table>");
                headerstring = htmlString.Substring(index, index1);
                headerstring = headerstring + "</table>";
                headerstring.Replace("height=' 30% '", "");
                //htmlString = htmlString.Replace(headerstring, "");
                htmlString = htmlString.Substring(index1 + 8, htmlString.Length - index1 - 8);
            }
            string strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
            //string strfooterProviderReviewed = "I " + Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate +
            //     " have reviewed the chart and agree with the management plan with the changes to the plan as indicated.";

            string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
            StaticLookupManager staticMngr = new StaticLookupManager();
            string strfooterProviderReviewed = string.Empty;
            IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
            if (CommonList.Count > 0)
                strfooterProviderReviewed = CommonList[0].Value.Replace("<Physician>", Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate).Replace("|", "");


            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            var strBody = new StringBuilder();

            string sbTop = "";

            sbTop = sbTop + htmlString;



            string strfooterF = "";


            if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && Encounter_Reviewed_signedDate != "" && Encounter_Reviewed_signedDate != "01-Jan-0001 12:00 AM")
            {
                strfooterProvider = strfooterProvider + "<br/>" + strfooterProviderReviewed;
            }
            else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM")
            {
                strfooterF = strfooterProvider;
            }
            else
            {


                strfooterF = "";
                strfooterProvider = "";
                strfooterProviderReviewed = "";
            }




            string strHtml = string.Empty;
            string pdfFileName = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], NotesName + "test" + ".pdf");//System.Configuration.ConfigurationSettings.AppSettings["XMLPath"] + NotesName + "test" + ".pdf";

            DirectoryInfo virdir = new DirectoryInfo(Page.MapPath("atala-capture-download/" + Session.SessionID + "/ProgressNotesFax"));
            if (!virdir.Exists)
            {
                virdir.Create();
            }
            // string pdfFileNamewithHeader = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], NotesName + ".pdf");//)System.Configuration.ConfigurationSettings.AppSettings["XMLPath"] + NotesName + ".pdf";
            string filename = "atala-capture-download/" + Session.SessionID + "/ProgressNotesFax/" + NotesName + ".pdf";
            string pdfFileNamewithHeader = Page.MapPath(filename);


            if (htmlString.Length > 0)
            {

                //  CreatePDFFromHTMLFile(sbTop, pdfFileName);

                try
                {
                    CreatePDFFromHTMLFile(sbTop, pdfFileName);
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToUpper().Contains("NO PAGES"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('1011187'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;

                    }
                    else
                        throw ex;
                }
                //StringReader sr = new StringReader(sbTop.ToString());
                //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0.0f);
                ////pdfDoc.attribute("AutoPrint").Value = true;
                //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                //pdfDoc.Open();
                //htmlparser.Parse(sr);
                //pdfDoc.Close();




                var reader = new PdfReader(pdfFileName);

                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {

                    // var document = new Document(reader.GetPageSizeWithRotation(1));
                    var document1 = new Document(new Rectangle(625, 975));
                    //Document document1 = new Document(new Rectangle(650, 975)); 
                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = reader.NumberOfPages;

                    //  string header = headerstring;
                    string[] Header = headerstring.Split(new string[] { "#$%" }, StringSplitOptions.None);
                    sProHeader = "N";// System.Configuration.ConfigurationSettings.AppSettings["WellnessNotesMainHeader"];

                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        var importedPage = writerpdf.GetImportedPage(reader, i);


                        var con = writerpdf.DirectContent;

                        // contentByte.BeginText();

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();

                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 9);


                        if (strfooterProvider != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProvider, 30, 30, 0);
                            if (strfooterProviderReviewed != "")
                                con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProviderReviewed, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else if (strfooterF != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }


                        //else
                        //{
                        //    con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 10, 0);
                        //    con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 10, 0);
                        //}


                        #region Column1
                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;
                        for (int j = 1; j < Header.Length; j += 4)
                        {
                            con.SetFontAndSize(baseFont1, 10);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            con.ShowText(Header[j].Split(':')[0].ToString());
                            X += 103;
                            con.SetColorFill(BaseColor.BLUE);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            if (Header[j].Split(':')[0].ToUpper().Contains("PATIENT"))
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() + " " : string.Empty) + "\n");
                            }
                            else
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() : string.Empty) + "\n");
                            }

                            X -= 103;
                            Y += 10;
                        }


                        #endregion


                        Y += 10;
                        con.MoveTo(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                        con.LineTo(pageSize.GetRight(X) + 40, pageSize.GetTop(Y));
                        con.Stroke();

                        #region Column2
                        X = 360f;
                        Y = 20f;

                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;
                        for (int j = 3; j < Header.Length; j += 4)
                        {
                            con.SetFontAndSize(baseFont1, 10);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 20, pageSize.GetTop(Y));
                            //con.SetTextMatrix(pageSize.GetLeft(X) - 50, pageSize.GetTop(Y));//For Bug ID: 70064
                            con.ShowText(Header[j].Split(':')[0].ToString());
                            X += 97;
                            con.SetColorFill(BaseColor.BLUE);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            if (Header[j].Split(':')[0].ToUpper().Contains("ENCOUNTER"))
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length >= 3 ? Header[j].Split(':')[1].ToString() + ":" + Header[j].Split(':')[2].ToString() + ":" + Header[j].Split(':')[3].ToString() : string.Empty) + "\n");
                            }

                            else if (Header[j].Split(':')[0].ToUpper().Contains("FACILITY"))
                            {
                                if (Header[j].Split(':').Length > 1)
                                {
                                    string[] facility = Header[j].Split(':')[1].Split(',');
                                    string fac = "";
                                    if (facility.Length > 0)
                                    {
                                        for (int k = 0; k < facility.Length; k++)
                                        {
                                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                                            if (k == 0)
                                            {
                                                con.ShowText(":" + facility[k].ToString() + ",");
                                                Y += 10;

                                            }
                                            else if (k == facility.Length - 1)
                                            {
                                                fac = fac + facility[k] + ".";
                                            }
                                            else
                                            {
                                                fac = fac + facility[k] + ",";


                                            }
                                            // X -= 97;

                                        }
                                        con.SetTextMatrix(pageSize.GetLeft(X) - 37, pageSize.GetTop(Y));
                                        con.ShowText(fac + "\n");

                                    }
                                }
                            }

                            else
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() : string.Empty) + "\n");
                            }
                            X -= 97;
                            Y += 10;
                        }



                        #endregion
                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();
                        if (sProHeader == "Y")
                        {

                            con.AddTemplate(importedPage, 0, 0);
                        }
                        else
                            con.AddTemplate(importedPage, 0, 60);

                    }

                    document1.Close();
                    writerpdf.Close();
                    fileStream.Close();
                    fileStream.Dispose();
                }
                if (File.Exists(pdfFileName))
                {
                    File.Delete(pdfFileName);
                    // string pdfPathCheck = path.Replace(".doc", ".pdf").ToString();

                }
            }
            else
            {
                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {

                    // var document = new Document(reader.GetPageSizeWithRotation(1));


                    var document1 = new Document(new Rectangle(625, 975));
                    //Document document1 = new Document(new Rectangle(650, 975)); 
                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = 1;

                    //  string header = headerstring;
                    string[] Header = headerstring.Split(new string[] { "#$%" }, StringSplitOptions.None);
                    sProHeader = "N";// System.Configuration.ConfigurationSettings.AppSettings["WellnessNotesMainHeader"];

                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        //var importedPage = writerpdf.GetImportedPage(reader, i);


                        var con = writerpdf.DirectContent;

                        // contentByte.BeginText();

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();
                        if (sProHeader == "Y")
                        {


                            //float U = 75;
                            //float S = 60f;


                        }
                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 8);

                        if (strfooterProvider != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProvider, 30, 30, 0);
                            if (strfooterProviderReviewed != "")
                                con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProviderReviewed, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else if (strfooterF != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }


                        Y += 10;
                        con.MoveTo(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));


                        #region Column1
                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;
                        for (int j = 1; j < Header.Length; j += 4)
                        {
                            con.SetFontAndSize(baseFont1, 10);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            con.ShowText(Header[j].Split(':')[0].ToString());
                            X += 103;
                            con.SetColorFill(BaseColor.BLUE);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            if (Header[j].Split(':')[0].ToUpper().Contains("PATIENT"))
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() + " " : string.Empty) + "\n");
                            }
                            else
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() : string.Empty) + "\n");
                            }

                            X -= 103;
                            Y += 10;
                        }


                        #endregion


                        con.LineTo(pageSize.GetRight(X) + 40, pageSize.GetTop(Y));
                        con.Stroke();

                        #region Column2
                        X = 360f;
                        Y = 20f;

                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;
                        for (int j = 3; j < Header.Length; j += 4)
                        {
                            con.SetFontAndSize(baseFont1, 10);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 20, pageSize.GetTop(Y));
                            //con.SetTextMatrix(pageSize.GetLeft(X) - 50, pageSize.GetTop(Y));//For Bug ID: 70064
                            con.ShowText(Header[j].Split(':')[0].ToString());
                            X += 97;
                            con.SetColorFill(BaseColor.BLUE);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            if (Header[j].Split(':')[0].ToUpper().Contains("ENCOUNTER"))
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length >= 3 ? Header[j].Split(':')[1].ToString() + ":" + Header[j].Split(':')[2].ToString() + ":" + Header[j].Split(':')[3].ToString() : string.Empty) + "\n");
                            }

                            else if (Header[j].Split(':')[0].ToUpper().Contains("FACILITY"))
                            {
                                if (Header[j].Split(':').Length > 1)
                                {
                                    string[] facility = Header[j].Split(':')[1].Split(',');
                                    string fac = "";
                                    if (facility.Length > 0)
                                    {
                                        for (int k = 0; k < facility.Length; k++)
                                        {
                                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                                            if (k == 0)
                                            {
                                                con.ShowText(":" + facility[k].ToString() + ",");
                                                Y += 10;

                                            }
                                            else if (k == facility.Length - 1)
                                            {
                                                fac = facility[k] + ".";
                                            }
                                            else
                                            {
                                                fac = facility[k] + ",";


                                            }
                                            // X -= 97;

                                        }
                                        con.SetTextMatrix(pageSize.GetLeft(X) - 37, pageSize.GetTop(Y));
                                        con.ShowText(fac + "\n");

                                    }
                                }
                            }

                            else
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() : string.Empty) + "\n");
                            }
                            X -= 97;
                            Y += 10;
                        }



                        #endregion
                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();

                    }

                    document1.Close();
                    writerpdf.Close();
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }



            hdnFilePath.Value = pdfFileNamewithHeader;
        }
            // itemDoc.Load(XmlText);
            string sFaxFirstname = string.Empty;
            string sFaxLastName = string.Empty;
            string sFaxDOS = string.Empty;
            //if (File.Exists(strXmlHumanPath) == true)
            if (sXMLHumanDoc != string.Empty)
            {
                XmlDocument itemDoc = new XmlDocument();
                // XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                itemDoc.LoadXml(sXMLHumanDoc);
                //using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //{
                //itemDoc.Load(fs);

                // XmlText.Close();
                if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                {
                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes.Count > 0)
                    {
                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != null)
                            sFaxFirstname = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != null)
                            sFaxLastName = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();

                    }
                }
                //    fs.Close();
                //    fs.Dispose();
                //}
            }
            //DOS
            string sRefProvider = string.Empty;
            if (sXMLEncounterDoc != string.Empty)
            {
                XmlDocument itemDoc = new XmlDocument();
                itemDoc.LoadXml(sXMLEncounterDoc);
                //XmlTextReader XmlText = new XmlTextReader(strXmlEncounterPath);
                //using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //{
                //    itemDoc.Load(fs);

                //    XmlText.Close();
                string sDOS = string.Empty;

                if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                {
                    if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes.Count > 0)
                    {
                        if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value != "")
                        {
                            sFaxDOS = "_" + Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("dd-MMM-yyyy");

                        }
                    }
                    if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Physician").Value != "")
                    {

                        sRefProvider = " |" + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Physician").Value +
                            "| NPI: " + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Provider_NPI").Value +
                            "| Facility: " + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Facility").Value +
                            "| Address:" + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Address").Value +
                            "| Fax No:" + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Fax_No").Value +
                            "| Phone No:" + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Phone_No").Value;
                    }
                }
                //    fs.Close();
                //    fs.Dispose();
                //}
            }
            //



            sFaxSubject = "Progress Notes" + sFaxLastName + sFaxFirstname + sFaxDOS;//<Patient Name>_<Date_of_service> 
            //Cap - 1414, 1415, 1449
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "EFax", "OpenEfax('" + sFaxSubject + "','" + sRefProvider + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "EFax", "OpenEfax('" + sFaxSubject + "','" + sRefProvider + "','N');", true);

            //Response.ContentType = "application/x-download";
            //Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", NotesName + ".pdf"));
            //Response.WriteFile(pdfFileNamewithHeader);

            //Response.Flush();
            //System.IO.File.Delete(pdfFileNamewithHeader);
            //Response.End();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Send FAX  : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
        
        }

        public void CreatePDFFromHTMLFile(string HtmlStream, string FileName)
        {

            object TargetFile = FileName;
            string ModifiedFileName = string.Empty;
            string FinalFileName = string.Empty;



            GeneratePDF.HtmlToPdfBuilder builder = new GeneratePDF.HtmlToPdfBuilder(iTextSharp.text.PageSize.A4);
            GeneratePDF.HtmlPdfPage first = builder.AddPage();
            first.AppendHtml(HtmlStream);
            byte[] file = builder.RenderPdf();
            File.WriteAllBytes(TargetFile.ToString(), file);

            iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(TargetFile.ToString());
            ModifiedFileName = TargetFile.ToString();
            ModifiedFileName = ModifiedFileName.Insert(ModifiedFileName.Length - 4, "1");
            iTextSharp.text.pdf.PdfEncryptor.Encrypt(reader, new FileStream(ModifiedFileName, FileMode.Append), iTextSharp.text.pdf.PdfWriter.STRENGTH128BITS, "", "", iTextSharp.text.pdf.PdfWriter.ALLOW_SCREENREADERS);

            // iTextSharp.text.pdf.PdfEncryptor.Encrypt()
            reader.Close();
            if (File.Exists(TargetFile.ToString()))
                File.Delete(TargetFile.ToString());
            FinalFileName = ModifiedFileName.Remove(ModifiedFileName.Length - 5, 1);
            File.Copy(ModifiedFileName, FinalFileName);
            if (File.Exists(ModifiedFileName))
                File.Delete(ModifiedFileName);



        }
        string ExtractBetween(string text, string start, string end)
        {
            int iStart = text.IndexOf(start);
            iStart = (iStart == -1) ? 0 : iStart + start.Length;
            int iEnd = text.LastIndexOf(end);
            if (iEnd == -1)
            {
                iEnd = text.Length;
            }
            int len = iEnd - iStart;

            return text.Substring(iStart, len);
        }





        protected void btnWordconsult_Click(object sender, EventArgs e)
        {
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation Document : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            //string FileName = strXmlEncounterPath;// "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml"; ;
            // string strXmlEncounterPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);


            //string xmlDataFile = strXmlEncounterPath;
            string xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Consultation_Notes.xsl");
            string OutputNamingConvention = System.Configuration.ConfigurationSettings.AppSettings["ConsultationNotesNamingConvention"];
            string sNamingConvention = string.Empty;
            string[] OutputName = OutputNamingConvention.Split('~');
            string NotesName = OutputNamingConvention;
            string result = string.Empty;
            string[] dtFormat = OutputNamingConvention.Split('^');
            string sDateFormat = string.Empty;
            if (dtFormat.Count() > 1)
            {
                if (dtFormat[1].Contains("@"))
                {
                    dtFormat[1] = dtFormat[1].Split('@')[0];
                }
            }
            string[] FormatCase = OutputNamingConvention.Split('@');
            for (int i = 0; i < OutputName.Length; i++)
            {
                if (OutputName[i] != "")
                {
                    result = ExtractBetween(OutputName[i], "[", "]");
                    //if (result.ToUpper().Contains("FACILITY"))
                    //{
                    //    if (FormatCase.Count() > 1)
                    //    {
                    //        if (FormatCase[1].ToUpper().Contains("YES"))
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", "")).ToUpper();
                    //        else
                    //        {
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //        }
                    //        NotesName = NotesName.Replace(FormatCase[1], "");
                    //    }
                    //    else
                    //    {
                    //        NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //    }

                    //}

                    if (result.ToUpper().Contains("HUMAN"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.HumanId.ToString());
                    }
                    if (result.ToUpper().Contains("ENCOUNTER"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.EncounterId.ToString());

                    }
                    if (result.ToUpper().Contains("MEMBER") || result.ToUpper().Contains("LAST") || result.ToUpper().Contains("FIRST") || result.ToUpper().Contains("DOB"))
                    {
                        string sExternalAccNo = string.Empty;
                        string sLastName = string.Empty;
                        string sFirstName = string.Empty;
                        string sDOB = string.Empty;
                        //try
                        //{
                        //if (File.Exists(strXmlHumanPath) == true)
                        if (sXMLHumanDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLHumanDoc);
                            //using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                            {
                                if (result.ToUpper().Contains("MEMBER"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value != "")
                                    {
                                        sExternalAccNo = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value.ToString();
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sExternalAccNo);
                                }
                                else if (result.ToUpper().Contains("LAST"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToUpper().ToString();
                                            else
                                            {
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", (sLastName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                }
                                else if (result.ToUpper().Contains("FIRST"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToUpper().ToString();
                                            else
                                            {
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", (sFirstName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                }
                                else if (result.ToUpper().Contains("DOB") || (OutputName[i].ToUpper().Contains("DATE") && OutputName[i].ToUpper().Contains("BIRTH")))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value != "")
                                    {
                                        if (dtFormat.Count() > 1)
                                        {
                                            if (dtFormat[1] == "yyyyMMdd")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            else if (dtFormat[1] == "MMddyyyy")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyyyy");
                                            else if (dtFormat[1] == "MMddyy")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyy");
                                            else if (dtFormat[1] == "yyMMdd")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyMMdd");
                                            else
                                            {
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            }
                                            NotesName = NotesName.Replace(dtFormat[1], "");
                                        }
                                        else
                                        {
                                            sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sDOB);
                                }
                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception(ex.Message + " - " + strXmlHumanPath);
                        //}

                    }
                    if ((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE")) || result.ToUpper().Contains("FACILITY"))
                    {
                        //try
                        //{
                        //if (File.Exists(strXmlEncounterPath) == true)
                        if (sXMLEncounterDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlEncounterPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLEncounterDoc);
                            //using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                            {
                                string sDOS = string.Empty;
                                if (((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE"))) && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value != "")
                                {
                                    if (dtFormat.Count() > 1)
                                    {
                                        if (dtFormat[1] == "yyyyMMdd")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        else if (dtFormat[1] == "MMddyyyy")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyyyy");
                                        else if (dtFormat[1] == "MMddyy")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyy");
                                        else if (dtFormat[1] == "yyMMdd")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyMMdd");
                                        else
                                        {
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        }
                                        NotesName = NotesName.Replace(dtFormat[1], "");
                                    }
                                    else
                                    {
                                        sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sDOS);
                                }
                                else if (result.ToUpper().Contains("FACILITY") && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Trim() != "")
                                {
                                    if (FormatCase.Count() > 1)
                                    {
                                        if (FormatCase[1].ToUpper().Contains("YES"))
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", "")).ToUpper();
                                        else
                                        {
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                        }
                                        NotesName = NotesName.Replace(FormatCase[1], "");
                                    }
                                    else
                                    {
                                        NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                    }
                                }
                                else
                                {
                                    NotesName = NotesName.Replace("[" + result + "]", "");
                                }
                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception(ex.Message + " - " + strXmlEncounterPath);
                        //}
                    }
                }
            }
            if (NotesName.Contains('['))
            {
                string[] sName = NotesName.Split('~');

                for (int i = 0; i < sName.Length; i++)
                {
                    if (sName[i] != "")
                    {
                        if (sName[i].Contains("["))
                        {
                            result = ExtractBetween(sName[i], "[", "]");
                            NotesName = NotesName.Replace("[" + result + "]", "");
                        }
                    }
                }
            }
            //NotesName = NotesName.Replace("~", "").Replace("__", "_").Replace("^", "").Replace("@", "");
            NotesName = UtilityMngr.ReplaceSpecialCharaterInFileName(NotesName);
            string WordOutputName = NotesName + ".html";
            string outputDocument = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], WordOutputName);

            //Jira #CAP-344 - OldCode
            //DataSet ds;
            //XmlDataDocument xmlDoc;
            //XslCompiledTransform xslTran;
            //XmlElement root;
            //XPathNavigator nav;
            //XmlTextWriter writer;
            //XsltSettings settings = new XsltSettings(true, false);

            //ds = new DataSet();
            ////ds.ReadXml(xmlDataFile);
            //// ds.ReadXml(new XmlTextReader(new StringReader(sXMLEncounterDoc)));
            //StringBuilder sb = new StringBuilder();
            //sb.Append(sXMLEncounterDoc.ToString().Replace("</notes>", "").Replace("</Modules>", ""));

            //string SUB = sXMLHumanDoc.ToString().Substring(0, sXMLHumanDoc.LastIndexOf("?>") + 2);

            //sb.Append(sXMLHumanDoc.ToString().Replace(SUB, "").Replace("<notes>", "").Replace("<Modules>", ""));
            //ds.ReadXml(new XmlTextReader(new StringReader(sb.ToString())));

            //xmlDoc = new XmlDataDocument(ds);
            //xslTran = new XslCompiledTransform();
            //// xslTran.Load(xsltFile);
            //UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation Document XSLT Load : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            //xslTran.Load(xsltFile, settings, new XmlUrlResolver());
            //UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation Document XSLT Load : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            //root = xmlDoc.DocumentElement;


            //nav = root.CreateNavigator();
            //if (File.Exists(outputDocument))
            //{
            //    File.Delete(outputDocument);
            //}
            //writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8);
            //xslTran.Transform(nav, writer);
            //writer.Close();
            //writer = null;
            //nav = null;
            //root = null;
            //xmlDoc = null;
            //ds = null;

            //Jira #CAP-344 - NewCode
            UtilityManager.PrintPDFUsingXSLT(sXMLEncounterDoc, sXMLHumanDoc, xsltFile, outputDocument, sGroup_ID_Log);
            System.IO.FileInfo file = new System.IO.FileInfo(outputDocument);
            string htmlString = System.IO.File.ReadAllText(outputDocument);

            //  IList<PatientPane> lstpane = new List<PatientPane>();
            //   lstpane = ClientSession.PatientPaneList.ToList<PatientPane>();
            string Patient_Name = "";
            // if(lstpane.Count>0)
            // Patient_Name=lstpane[0].Last_Name + "," + "" + lstpane[0].First_Name + " " + lstpane[0].MI;


            string Encounter_signedDate = "";
            string Encounter_Provider_Name = "";
            string Encounter_Reviewed_signedDate = "";
            string Encounter_Reviewed_Name = "";
            string Encounter_Reviewed_Id = "";
            TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
            XDocument xmlDocumentType = XDocument.Load(EncXMLContent);

            foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value);
                    Encounter_Reviewed_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm tt");

                    DateTime dtPro = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                    Encounter_signedDate = UtilityManager.ConvertToLocal(dtPro).ToString("dd-MMM-yyyy hh:mm tt");

                    Encounter_Reviewed_Id = Encounter.Attribute("Encounter_Provider_Review_ID").Value;

                }

            }
            //Provider Name 
            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    Encounter_Provider_Name = Encounter.Value;
                    break;
                }
            }
            //Provider Reviewed Name 
            if (Encounter_Reviewed_Id != "")
            {
                string xmlFilepathUser = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
                if (File.Exists(xmlFilepathUser))
                {
                    XmlDocument xdoc = new XmlDocument();
                    XmlTextReader itext = new XmlTextReader(xmlFilepathUser);
                    xdoc.Load(itext);
                    itext.Close();
                    XmlNodeList xnodelst = xdoc.GetElementsByTagName("User");
                    if (xnodelst != null && xnodelst.Count > 0)
                    {
                        foreach (XmlNode xnode in xnodelst)
                        {
                            if (xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() != "0" && xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() == Encounter_Reviewed_Id)
                            {
                                Encounter_Reviewed_Name = xnode.Attributes.GetNamedItem("person_name").Value;
                            }
                        }
                    }
                }
            }




            string Provider_Speciality = "";
            TextReader EncXMLContentdoc = new StringReader(sXMLEncounterDoc);
            xmlDocumentType = XDocument.Load(EncXMLContentdoc);
            TextReader HumanXMLContentdoc = new StringReader(sXMLHumanDoc);
            XDocument xmlDocument = XDocument.Load(HumanXMLContentdoc);

            string Encounter_Provider_id = "";

            foreach (XElement elements in xmlDocument.Descendants("HumanList"))
            {

                foreach (XElement Human in elements.Elements())
                {
                    if (Human.Attribute("First_Name") != null && Human.Attribute("Last_Name") != null && Human.Attribute("MI") != null)
                        Patient_Name = Human.Attribute("Last_Name").Value + "," + "" + Human.Attribute("First_Name").Value + " " + Human.Attribute("MI").Value;

                }
            }





            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {

                foreach (XElement Encounter in elements.Elements())
                {
                    Encounter_Provider_Name = Encounter.Value;
                    break;

                }
                break;
            }
            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    // if (Encounter.LastAttribute != null && Encounter.LastAttribute.Name.ToString() == "Specialties")
                    if (Encounter.Attribute("Specialties") != null)
                    {
                        Provider_Speciality = Encounter.LastAttribute.Value;
                        break;


                    }


                }

            }



            //string strfooter = "Electronically Signed by ";


            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            var strBody = new StringBuilder();
            string imgpath = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath"];
            string imgpath_phy = System.Configuration.ConfigurationSettings.AppSettings["phyConsultationLogoPath"];
            string imgpath_PalliativeCare = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath_PalliativeCare"];
            string imgpath_Integrum = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath_Integrum"];

            string phyheaderstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_phy + "' alt='logo' height='100' width='200'></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string header = "<table style='width:100%'><tr><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerLogo_PalliativeCare = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_PalliativeCare + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerLogo_Integrum = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_Integrum + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";


            StringBuilder sbTop = new System.Text.StringBuilder();
            sbTop.Append(@"
<html 
xmlns:o='urn:schemas-microsoft-com:office:office' 
xmlns:w='urn:schemas-microsoft-com:office:word'
xmlns='http://www.w3.org/TR/REC-html40'>
<head><title></title>

<!--[if gte mso 9]>
<xml>
<w:WordDocument>
<w:View>Print</w:View>
<w:Zoom>90</w:Zoom>
<w:DoNotOptimizeForBrowser/>
</w:WordDocument>
</xml>
<![endif]-->


<style>
p.MsoFooter, li.MsoFooter, div.MsoFooter
{
margin:0in;
margin-bottom:.0001pt;
mso-pagination:widow-orphan;
tab-stops:center 3.0in right 6.0in;
font-size:12.0pt;
}
<style>

<!-- /* Style Definitions */

@page Section1
{
size:8.5in 11.0in; 
margin:1.0in 1.25in 1.0in 1.25in ;
mso-header-margin:.5in;
mso-header:h1;
mso-footer: f1; 
mso-footer-margin:.5in;
}


div.Section1
{
page:Section1;
}

table#hrdftrtbl
{
margin:0in 0in 0in 9in;
}
-->
</style></head>

<body lang=EN-US style='tab-interval:.5in'>
<div class=Section1>");
            sbTop.Append(htmlString);
            sbTop.Append(@"<table id='hrdftrtbl' border='1' cellspacing='0' cellpadding='0'>
<tr><td>
<div style='mso-element:header' id=h1 >");
            if (System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoIntegrum"] != null && System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoIntegrum"] == "ALL")
                sbTop.Append(headerLogo_Integrum);
            else if (Provider_Speciality.ToUpper().Contains("ENDOCRINOLOGY"))
                sbTop.Append(headerstring);
            else if (Encounter_Provider_id == "4198")
                sbTop.Append(phyheaderstring);
            else if (Encounter_Provider_id == "3101" || Encounter_Provider_id == "321")
                sbTop.Append(headerLogo_PalliativeCare);
            else
                sbTop.Append(header);

            sbTop.Append(@"
</div>
</td>
<td>");

            string strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
            //string strfooterProviderReviewed = "I " + Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate +
            //     " have reviewed the chart and agree with the management plan with the changes to the plan as indicated.";

            string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
            StaticLookupManager staticMngr = new StaticLookupManager();
            string strfooterProviderReviewed = string.Empty;
            IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
            if (CommonList.Count > 0)
                strfooterProviderReviewed = CommonList[0].Value.Replace("<Physician>", Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate).Replace("|", "");


            string strfooterF = "";


            if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && Encounter_Reviewed_signedDate != "" && Encounter_Reviewed_signedDate != "01-Jan-0001 12:00 AM")
            {
                strfooterProvider = strfooterProvider + "<br/>" + strfooterProviderReviewed;
            }
            else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM")
            {
                strfooterF = strfooterProvider;
            }
            else
            {


                strfooterF = "";
                strfooterProvider = "";
                strfooterProviderReviewed = "";
            }
            if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && Encounter_Reviewed_signedDate != "" && Encounter_Reviewed_signedDate != "01-Jan-0001 12:00 AM")
            {
                sbTop.Append(@"<div style='mso-element:footer;font-family:Arial;font-size:9pt' id=f1><p class=MsoFooter style='font-family:Arial;font-size:9pt'>" + strfooterProvider +
@"<span style=padding-right:10px;mso-tab-count:2'>Page&#32;</span><span style='mso-field-code:"" PAGE "";font-family:Arial;font-size:9pt'>
</span>&nbsp;of&nbsp;<span style='mso-field-code:"" NUMPAGES "";font-family:Arial;font-size:9pt'></span></p></div>
</td></tr>

</table>

</body></html>
");
            }
            else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM")
            {
                sbTop.Append(@"<div style='mso-element:footer;font-family:Arial;font-size:9pt' id=f1><p class=MsoFooter style='font-family:Arial;font-size:9pt'>" + strfooterProvider +
@"<span style=padding-right:10px;mso-tab-count:2'>Page&#32;</span><span style='mso-field-code:"" PAGE "";font-family:Arial;font-size:9pt'>
</span>&nbsp;of&nbsp;<span style='mso-field-code:"" NUMPAGES "";font-family:Arial;font-size:9pt'></span></p></div>
</td></tr>

</table>

</body></html>
");
            }
            else
            {

                sbTop.Append(@"<div style='mso-element:footer;' id=f1><p class=MsoFooter><span style=padding-right:10px;mso-tab-count:2;font-family:Arial;font-size:9pt'>Page&#32;</span><span style='mso-field-code:"" PAGE "";font-family:Arial;font-size:9pt'>
</span>&nbsp;of&nbsp;<span style='mso-field-code:"" NUMPAGES "";font-family:Arial;font-size:9pt'></span>
</p></div>
</td></tr>
</table>
</body></html>
");
            }

            sbTop.Replace("\uFFFD", " ");
            if (System.Configuration.ConfigurationManager.AppSettings["DocToPdf"].ToUpper() == "Y")
            {
                string path = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"], Session.SessionID + "DocToPdf") + "/" + ClientSession.FacilityName.Replace(",", "") + "_Consultation_Notes_" + ClientSession.EncounterId + ".doc";
                if (File.Exists(path))
                {
                    File.Delete(path);
                    string pdfPathCheck = path.Replace(".doc", ".pdf").ToString();
                    File.Delete(pdfPathCheck);
                }

                if (!File.Exists(path))
                {
                    string strbody = sbTop.ToString();
                    File.WriteAllText(path, strbody);
                }
                DocToPdf objDoctoPdf = new DocToPdf();
                FileInfo filepath = new FileInfo(path);
                Boolean ConvertCheck = objDoctoPdf.ConvertDocToPdf(filepath);
                if (ConvertCheck)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + NotesName + ".pdf");
                    Response.ContentType = "application/pdf";
                    Response.TransmitFile(path.Replace(".doc", ".pdf"));
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + NotesName + ".doc");
                Response.ContentType = "application/msword";


                Response.Write(sbTop);
                Response.Flush();
                Response.End();
            }

            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation Document : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

        }
        protected void btnPDFconsult_Click(object sender, EventArgs e)
        {
            //Jira #CAP-731 -start
            //Jira #CAP-855
            
            string sExMessage = "";
            sIsAkidoEncounter = UtilityManager.IsAkidoEncounter(ClientSession.EncounterId.ToString(), out sExMessage);
            if (System.Configuration.ConfigurationSettings.AppSettings["IsAkidoNoteSummary"] == "Y" && sIsAkidoEncounter == "true")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "alert('The Notes can not be generated for this encounter, as this encounter is part of the Akido Note.'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            else if (System.Configuration.ConfigurationSettings.AppSettings["IsAkidoNoteSummary"] == "Y" && sIsAkidoEncounter == "Exception")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('1011199'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            //Jira #CAP-731 -end
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            //string FileName = strXmlEncounterPath; ;// "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml"; ;
            // string strXmlEncounterPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);


            //string xmlDataFile = strXmlEncounterPath;
            string xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Consultation_Notes.xsl");
            string OutputNamingConvention = System.Configuration.ConfigurationSettings.AppSettings["ConsultationNotesNamingConvention"];
            string sNamingConvention = string.Empty;
            string[] OutputName = OutputNamingConvention.Split('~');
            string NotesName = OutputNamingConvention;
            string result = string.Empty;
            string[] dtFormat = OutputNamingConvention.Split('^');
            string sDateFormat = string.Empty;
            if (dtFormat.Count() > 1)
            {
                if (dtFormat[1].Contains("@"))
                {
                    dtFormat[1] = dtFormat[1].Split('@')[0];
                }
            }
            string[] FormatCase = OutputNamingConvention.Split('@');
            for (int i = 0; i < OutputName.Length; i++)
            {
                if (OutputName[i] != "")
                {
                    result = ExtractBetween(OutputName[i], "[", "]");
                    //if (result.ToUpper().Contains("FACILITY"))
                    //{
                    //    if (FormatCase.Count() > 1)
                    //    {
                    //        if (FormatCase[1].ToUpper().Contains("YES"))
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", "")).ToUpper();
                    //        else
                    //        {
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //        }
                    //        NotesName = NotesName.Replace(FormatCase[1], "");
                    //    }
                    //    else
                    //    {
                    //        NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //    }

                    //}

                    if (result.ToUpper().Contains("HUMAN"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.HumanId.ToString());
                    }
                    if (result.ToUpper().Contains("ENCOUNTER"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.EncounterId.ToString());

                    }
                    if (result.ToUpper().Contains("MEMBER") || result.ToUpper().Contains("LAST") || result.ToUpper().Contains("FIRST") || result.ToUpper().Contains("DOB"))
                    {
                        string sExternalAccNo = string.Empty;
                        string sLastName = string.Empty;
                        string sFirstName = string.Empty;
                        string sDOB = string.Empty;
                        //try
                        //{
                        //if (File.Exists(strXmlHumanPath) == true)
                        if (sXMLHumanDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLHumanDoc);
                            //using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                            {
                                if (result.ToUpper().Contains("MEMBER"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value != "")
                                    {
                                        sExternalAccNo = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value.ToString();
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sExternalAccNo);
                                }
                                else if (result.ToUpper().Contains("LAST"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToUpper().ToString();
                                            else
                                            {
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", (sLastName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                }
                                else if (result.ToUpper().Contains("FIRST"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToUpper().ToString();
                                            else
                                            {
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", (sFirstName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                }
                                else if (result.ToUpper().Contains("DOB") || (OutputName[i].ToUpper().Contains("DATE") && OutputName[i].ToUpper().Contains("BIRTH")))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value != "")
                                    {
                                        if (dtFormat.Count() > 1)
                                        {
                                            if (dtFormat[1] == "yyyyMMdd")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            else if (dtFormat[1] == "MMddyyyy")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyyyy");
                                            else if (dtFormat[1] == "MMddyy")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyy");
                                            else if (dtFormat[1] == "yyMMdd")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyMMdd");
                                            else
                                            {
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            }
                                            NotesName = NotesName.Replace(dtFormat[1], "");
                                        }
                                        else
                                        {
                                            sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sDOB);
                                }
                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception(ex.Message + " - " + strXmlHumanPath);
                        //}

                    }
                    if ((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE")) || result.ToUpper().Contains("FACILITY"))
                    {

                        //try
                        //{
                        //if (File.Exists(strXmlEncounterPath) == true)
                        if (sXMLEncounterDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlEncounterPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLEncounterDoc);
                            //using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                            {
                                string sDOS = string.Empty;

                                if ((result.ToUpper().Contains("DOS") || OutputName[i].ToUpper().Contains("DATE")) && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value != "")
                                {
                                    if (dtFormat.Count() > 1)
                                    {
                                        if (dtFormat[1] == "yyyyMMdd")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        else if (dtFormat[1] == "MMddyyyy")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyyyy");
                                        else if (dtFormat[1] == "MMddyy")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyy");
                                        else if (dtFormat[1] == "yyMMdd")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyMMdd");
                                        else
                                        {
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        }
                                        NotesName = NotesName.Replace(dtFormat[1], "");
                                    }
                                    else
                                    {
                                        sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sDOS);
                                }
                                else if (result.ToUpper().Contains("FACILITY") && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Trim() != "")
                                {
                                    if (FormatCase.Count() > 1)
                                    {
                                        if (FormatCase[1].ToUpper().Contains("YES"))
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", "")).ToUpper();
                                        else
                                        {
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                        }
                                        NotesName = NotesName.Replace(FormatCase[1], "");
                                    }
                                    else
                                    {
                                        NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                    }
                                }
                                else
                                {
                                    NotesName = NotesName.Replace("[" + result + "]", "");
                                }

                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception(ex.Message + " - " + strXmlEncounterPath);
                        //}
                    }
                }
            }
            if (NotesName.Contains('['))
            {
                string[] sName = NotesName.Split('~');

                for (int i = 0; i < sName.Length; i++)
                {
                    if (sName[i] != "")
                    {
                        if (sName[i].Contains("["))
                        {
                            result = ExtractBetween(sName[i], "[", "]");
                            NotesName = NotesName.Replace("[" + result + "]", "");
                        }
                    }
                }
            }
            //NotesName = NotesName.Replace("~", "").Replace("__", "_").Replace("^", "").Replace("@", "");
            NotesName = UtilityMngr.ReplaceSpecialCharaterInFileName(NotesName);
            string WordOutputName = NotesName + ".html";
            string outputDocument = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], WordOutputName);
            //Jira #CAP-344 - OldCode
            //DataSet ds;
            //XmlDataDocument xmlDoc;
            //XslCompiledTransform xslTran;
            //XmlElement root;
            //XPathNavigator nav;
            //XmlTextWriter writer;
            //XsltSettings settings = new XsltSettings(true, false);

            //ds = new DataSet();
            ////ds.ReadXml(xmlDataFile);
            ////ds.ReadXml(new XmlTextReader(new StringReader(sXMLEncounterDoc)));
            //StringBuilder sb = new StringBuilder();
            //sb.Append(sXMLEncounterDoc.ToString().Replace("</notes>", "").Replace("</Modules>", ""));

            //string SUB = sXMLHumanDoc.ToString().Substring(0, sXMLHumanDoc.LastIndexOf("?>") + 2);

            //sb.Append(sXMLHumanDoc.ToString().Replace(SUB, "").Replace("<notes>", "").Replace("<Modules>", ""));
            //ds.ReadXml(new XmlTextReader(new StringReader(sb.ToString())));

            //xmlDoc = new XmlDataDocument(ds);
            //xslTran = new XslCompiledTransform();
            //// xslTran.Load(xsltFile);
            //UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF XSLT Load : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            //xslTran.Load(xsltFile, settings, new XmlUrlResolver());
            //UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF XSLT Load : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            //root = xmlDoc.DocumentElement;


            //nav = root.CreateNavigator();
            //if (File.Exists(outputDocument))
            //{
            //    File.Delete(outputDocument);
            //}
            //writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8);
            //xslTran.Transform(nav, writer);
            //writer.Close();
            //writer = null;
            //nav = null;
            //root = null;
            //xmlDoc = null;
            //ds = null;

            //Jira #CAP-344 - NewCode
            UtilityManager.PrintPDFUsingXSLT(sXMLEncounterDoc, sXMLHumanDoc, xsltFile, outputDocument, sGroup_ID_Log);
            System.IO.FileInfo file = new System.IO.FileInfo(outputDocument);
            //  string htmlString = System.IO.File.ReadAllText(outputDocument);

            //  IList<PatientPane> lstpane = new List<PatientPane>();
            //   lstpane = ClientSession.PatientPaneList.ToList<PatientPane>();
            string Patient_Name = "";
            // if(lstpane.Count>0)
            // Patient_Name=lstpane[0].Last_Name + "," + "" + lstpane[0].First_Name + " " + lstpane[0].MI;

            string Encounter_signedDate = "";

            string Encounter_Provider_Name = "";
            string Provider_Speciality = "";
            TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
            XDocument xmlDocumentType = XDocument.Load(EncXMLContent);
            TextReader HumanXMLContentdoc = new StringReader(sXMLHumanDoc);
            XDocument xmlDocument = XDocument.Load(HumanXMLContentdoc);
            string Encounter_Provider_id = "";

            foreach (XElement elements in xmlDocument.Descendants("HumanList"))
            {

                foreach (XElement Human in elements.Elements())
                {
                    if (Human.Attribute("First_Name") != null && Human.Attribute("Last_Name") != null && Human.Attribute("MI") != null)
                        Patient_Name = Human.Attribute("Last_Name").Value + "," + "" + Human.Attribute("First_Name").Value + " " + Human.Attribute("MI").Value;

                }
            }





            string Encounter_Reviewed_signedDate = "";
            string Encounter_Reviewed_Name = "";
            string Encounter_Reviewed_Id = "";
            // xmlDocumentType = XDocument.Load(strXmlEncounterPath);

            foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value);
                    Encounter_Reviewed_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm tt");

                    DateTime dtPro = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                    Encounter_signedDate = UtilityManager.ConvertToLocal(dtPro).ToString("dd-MMM-yyyy hh:mm tt");

                    Encounter_Reviewed_Id = Encounter.Attribute("Encounter_Provider_Review_ID").Value;

                }

                //if (Encounter_signedDate == "" || Encounter_signedDate == "01-Jan-0001 12:00:00 AM")
                //{
                // foreach (XElement Encounter in elements.Elements())
                // {
                //     DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                //     Encounter_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm:ss tt");
                //}
                //}
            }
            //Provider Name 
            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    Encounter_Provider_Name = Encounter.Value;
                    break;
                }
                break;
            }
            //Provider Reviewed Name 
            if (Encounter_Reviewed_Id != "")
            {
                string xmlFilepathUser = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
                if (File.Exists(xmlFilepathUser))
                {
                    XmlDocument xdoc = new XmlDocument();
                    XmlTextReader itext = new XmlTextReader(xmlFilepathUser);
                    xdoc.Load(itext);
                    itext.Close();
                    XmlNodeList xnodelst = xdoc.GetElementsByTagName("User");
                    if (xnodelst != null && xnodelst.Count > 0)
                    {
                        foreach (XmlNode xnode in xnodelst)
                        {
                            if (xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() != "0" && xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() == Encounter_Reviewed_Id)
                            {
                                Encounter_Reviewed_Name = xnode.Attributes.GetNamedItem("person_name").Value;
                            }
                        }
                    }
                }

            }


            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    // if (Encounter.LastAttribute != null && Encounter.LastAttribute.Name.ToString() == "Specialties")
                    if (Encounter.Attribute("Specialties") != null)
                    {
                        Provider_Speciality = Encounter.LastAttribute.Value;
                        break;


                    }


                }

            }



            string strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
            //string strfooterProviderReviewed = "I " + Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate +
            //     " have reviewed the chart and agree with the management plan with the changes to the plan as indicated.";

            string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
            StaticLookupManager staticMngr = new StaticLookupManager();
            string strfooterProviderReviewed = string.Empty;
            IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
            if (CommonList.Count > 0)
                strfooterProviderReviewed = CommonList[0].Value.Replace("<Physician>", Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate).Replace("|", "");


            string htmlString = System.IO.File.ReadAllText(outputDocument);
            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            var strBody = new StringBuilder();
            string imgpath = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath"];
            string imgpath_phy = System.Configuration.ConfigurationSettings.AppSettings["phyConsultationLogoPath"];
            string imgpath_PalliativeCare = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath_PalliativeCare"];
            string imgpath_Integrum = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath_Integrum"];

            string phyheaderstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_phy + "' alt='logo' height='100' width='200'></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string header = "<table style='width:100%'><tr><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerLogo_PalliativeCare = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_PalliativeCare + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerLogo_Integrum = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_Integrum + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string finalHeadeString = string.Empty;
            string PatientName = "RE: " + Patient_Name;

            string sProHeader = System.Configuration.ConfigurationSettings.AppSettings["ProgressNotesMainHeader"];
            if (System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoIntegrum"] != null && System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoIntegrum"] == "ALL")
                finalHeadeString = imgpath_Integrum;
            else if (Provider_Speciality.ToUpper().Contains("ENDOCRINOLOGY"))
                finalHeadeString = imgpath;
            else if (Encounter_Provider_id == "4198")
                finalHeadeString = imgpath_phy;
            else if (Encounter_Provider_id == "3101" || Encounter_Provider_id == "321")
                finalHeadeString = imgpath_PalliativeCare;
            else
                finalHeadeString = "";


            string strfooterF = "";

            string strfooterPA = "";
            string strfooterP = "";
            if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && Encounter_Reviewed_signedDate != "" && Encounter_Reviewed_signedDate != "01-Jan-0001 12:00 AM")
            {
                strfooterPA = strfooterProvider;

                strfooterP = strfooterProviderReviewed;
                strfooterF = " ";
                //  strfooterProvider = strfooterProvider + "<br/>" + strfooterProviderReviewed;
            }
            else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM")
            {
                strfooterF = strfooterProvider;
            }
            else
            {


                strfooterF = "";

            }


            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            // var strBody = new StringBuilder();

            string sbTop = "";

            sbTop = sbTop + htmlString;


            string strHtml = string.Empty;
            string pdfFileName = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], NotesName + "test" + ".pdf");//System.Configuration.ConfigurationSettings.AppSettings["XMLPath"] + NotesName + "test" + ".pdf";

            string pdfFileNamewithHeader = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], NotesName + ".pdf");//)System.Configuration.ConfigurationSettings.AppSettings["XMLPath"] + NotesName + ".pdf";

            if (htmlString.Length > 0)
            {
                try
                {
                    CreatePDFFromHTMLFile(sbTop, pdfFileName);
                }

                catch (Exception ex)
                {
                    if (ex.Message.ToUpper().Contains("NO PAGES"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('1011187'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;

                    }
                    else
                        throw ex;
                }

                var reader = new PdfReader(pdfFileName);

                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {


                    var document1 = new Document(new Rectangle(625, 975));

                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = reader.NumberOfPages;



                    sProHeader = "N";
                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        var importedPage = writerpdf.GetImportedPage(reader, i);


                        var con = writerpdf.DirectContent;

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();

                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 9);

                        if (strfooterF == "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else if (strfooterPA != "" && strfooterP != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterPA, 30, 30, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterP, 30, 20, 0);

                            //ColumnText ct = new ColumnText(con);
                            //ct.SetSimpleColumn(new Phrase(new Chunk(strfooterPA, FontFactory.GetFont(FontFactory.TIMES_ITALIC, 9, Font.NORMAL))),
                            //                   35, 77, 530, 36, 14, iTextSharp.text.Element.ALIGN_LEFT | iTextSharp.text.Element.ALIGN_BOTTOM);
                            //ct.Go();

                            //ct = new ColumnText(con);
                            //ct.SetSimpleColumn(new Phrase(new Chunk(strfooterP, FontFactory.GetFont(FontFactory.TIMES_ITALIC, 9, Font.NORMAL))),
                            //                   35, 65, 530, 36, 14, iTextSharp.text.Element.ALIGN_LEFT | iTextSharp.text.Element.ALIGN_BOTTOM);
                            //ct.Go(); 

                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }

                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;

                        con.SetFontAndSize(baseFont1, 10);
                        con.SetColorFill(BaseColor.BLACK);
                        con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));

                        X += 103;

                        con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                        // string imagepath = "";

                        if (finalHeadeString != "")
                        {
                            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(finalHeadeString);
                            logo.ScaleAbsolute(50, 50);
                            logo.SetAbsolutePosition(pageSize.GetLeft(X) - 150, pageSize.GetTop(Y) - 30);
                            con.AddImage(logo);


                        }


                        con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PatientName, pageSize.GetLeft(X) + 200, pageSize.GetTop(Y) - 30, 0);
                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();
                        if (sProHeader == "Y")
                        {

                            con.AddTemplate(importedPage, 0, 0);
                        }
                        else
                            con.AddTemplate(importedPage, 0, 60);

                    }

                    document1.Close();
                    writerpdf.Close();
                    fileStream.Close();
                    fileStream.Dispose();
                }
                if (File.Exists(pdfFileName))
                {
                    File.Delete(pdfFileName);

                }
            }
            else
            {
                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {
                    var document1 = new Document(new Rectangle(625, 975));

                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = 1;


                    sProHeader = "N";

                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        var con = writerpdf.DirectContent;

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();
                        if (sProHeader == "Y")
                        {


                            //float U = 75;
                            //float S = 60f;


                        }
                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 8);

                        if (strfooterProvider != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProvider, 30, 30, 0);
                            if (strfooterProviderReviewed != "")
                                con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProviderReviewed, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else if (strfooterF != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }


                        Y += 10;
                        con.MoveTo(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));


                        #region Column1
                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;

                        con.SetFontAndSize(baseFont1, 10);
                        con.SetColorFill(BaseColor.BLACK);
                        con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                        if (finalHeadeString != "")
                        {
                            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(finalHeadeString);
                            logo.ScaleAbsolute(50, 50);
                            logo.SetAbsolutePosition(pageSize.GetLeft(X) - 150, pageSize.GetTop(Y) - 30);
                            con.AddImage(logo);


                        }


                        con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PatientName, pageSize.GetLeft(X) + 200, pageSize.GetTop(Y) - 30, 0);



                        #endregion


                        con.LineTo(pageSize.GetRight(X) + 40, pageSize.GetTop(Y));
                        con.Stroke();


                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();

                    }

                    document1.Close();
                    writerpdf.Close();
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }




            Response.ContentType = "application/x-download";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", NotesName + ".pdf"));
            Response.WriteFile(pdfFileNamewithHeader);

            Response.Flush();
            System.IO.File.Delete(pdfFileNamewithHeader);
            Response.End();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

        }
        public void hdnbtngeneratexmlsummary_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Summary", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Summary", "reloadPatientSummaryBar();{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

            //  Patientchartload();
        }
        protected void btnsendFaxconsult_Click(object sender, EventArgs e)
        {
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation Send Fax : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            string sFaxSubject = string.Empty;
            //string FileName = strXmlEncounterPath; //"Encounter" + "_" + ClientSession.EncounterId + ".xml";
            string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml"; ;
            // string strXmlEncounterPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);


            //string xmlDataFile = strXmlEncounterPath;
            string xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Consultation_Notes.xsl");
            string OutputNamingConvention = System.Configuration.ConfigurationSettings.AppSettings["ConsultationNotesNamingConvention"];
            string sNamingConvention = string.Empty;
            string[] OutputName = OutputNamingConvention.Split('~');
            string NotesName = OutputNamingConvention;
            string result = string.Empty;
            string[] dtFormat = OutputNamingConvention.Split('^');
            string sDateFormat = string.Empty;
            if (dtFormat.Count() > 1)
            {
                if (dtFormat[1].Contains("@"))
                {
                    dtFormat[1] = dtFormat[1].Split('@')[0];
                }
            }
            string[] FormatCase = OutputNamingConvention.Split('@');
            for (int i = 0; i < OutputName.Length; i++)
            {
                if (OutputName[i] != "")
                {
                    result = ExtractBetween(OutputName[i], "[", "]");
                    //if (result.ToUpper().Contains("FACILITY"))
                    //{
                    //    if (FormatCase.Count() > 1)
                    //    {
                    //        if (FormatCase[1].ToUpper().Contains("YES"))
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", "")).ToUpper();
                    //        else
                    //        {
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //        }
                    //        NotesName = NotesName.Replace(FormatCase[1], "");
                    //    }
                    //    else
                    //    {
                    //        NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //    }

                    //}

                    if (result.ToUpper().Contains("HUMAN"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.HumanId.ToString());
                    }
                    if (result.ToUpper().Contains("ENCOUNTER"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.EncounterId.ToString());

                    }
                    if (result.ToUpper().Contains("MEMBER") || result.ToUpper().Contains("LAST") || result.ToUpper().Contains("FIRST") || result.ToUpper().Contains("DOB"))
                    {
                        string sExternalAccNo = string.Empty;
                        string sLastName = string.Empty;
                        string sFirstName = string.Empty;
                        string sDOB = string.Empty;
                        //if (File.Exists(strXmlHumanPath) == true)
                        if (sXMLHumanDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLHumanDoc);
                            //using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                            {
                                if (result.ToUpper().Contains("MEMBER"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value != "")
                                    {
                                        sExternalAccNo = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value.ToString();
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sExternalAccNo);
                                }
                                else if (result.ToUpper().Contains("LAST"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToUpper().ToString();
                                            else
                                            {
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", (sLastName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                }
                                else if (result.ToUpper().Contains("FIRST"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToUpper().ToString();
                                            else
                                            {
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", (sFirstName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                }
                                else if (result.ToUpper().Contains("DOB") || (OutputName[i].ToUpper().Contains("DATE") && OutputName[i].ToUpper().Contains("BIRTH")))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value != "")
                                    {
                                        if (dtFormat.Count() > 1)
                                        {
                                            if (dtFormat[1] == "yyyyMMdd")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            else if (dtFormat[1] == "MMddyyyy")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyyyy");
                                            else if (dtFormat[1] == "MMddyy")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyy");
                                            else if (dtFormat[1] == "yyMMdd")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyMMdd");
                                            else
                                            {
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            }
                                            NotesName = NotesName.Replace(dtFormat[1], "");
                                        }
                                        else
                                        {
                                            sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sDOB);
                                }
                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }

                    }
                    if ((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE")) || result.ToUpper().Contains("FACILITY"))
                    {
                        //if (File.Exists(strXmlEncounterPath) == true)
                        if (sXMLEncounterDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlEncounterPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLEncounterDoc);
                            //using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //itemDoc.Load(fs);

                            //XmlText.Close();
                            if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                            {
                                string sDOS = string.Empty;
                                if (((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE"))) && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value != "")
                                {
                                    if (dtFormat.Count() > 1)
                                    {
                                        if (dtFormat[1] == "yyyyMMdd")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        else if (dtFormat[1] == "MMddyyyy")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyyyy");
                                        else if (dtFormat[1] == "MMddyy")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyy");
                                        else if (dtFormat[1] == "yyMMdd")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyMMdd");
                                        else
                                        {
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        }
                                        NotesName = NotesName.Replace(dtFormat[1], "");
                                    }
                                    else
                                    {
                                        sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sDOS);
                                }
                                else if (result.ToUpper().Contains("FACILITY") && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Trim() != "")
                                {
                                    if (FormatCase.Count() > 1)
                                    {
                                        if (FormatCase[1].ToUpper().Contains("YES"))
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", "")).ToUpper();
                                        else
                                        {
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                        }
                                        NotesName = NotesName.Replace(FormatCase[1], "");
                                    }
                                    else
                                    {
                                        NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                    }
                                }
                                else
                                {
                                    NotesName = NotesName.Replace("[" + result + "]", "");
                                }
                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }
                    }
                }
            }
            if (NotesName.Contains('['))
            {
                string[] sName = NotesName.Split('~');

                for (int i = 0; i < sName.Length; i++)
                {
                    if (sName[i] != "")
                    {
                        if (sName[i].Contains("["))
                        {
                            result = ExtractBetween(sName[i], "[", "]");
                            NotesName = NotesName.Replace("[" + result + "]", "");
                        }
                    }
                }
            }
            //NotesName = NotesName.Replace("~", "").Replace("__", "_").Replace("^", "").Replace("@", "");
            NotesName = UtilityMngr.ReplaceSpecialCharaterInFileName(NotesName);
            string WordOutputName = NotesName + ".html";
            string outputDocument = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], WordOutputName);
            //Jira #CAP-344 - OldCode
            //DataSet ds;
            //XmlDataDocument xmlDoc;
            //XslCompiledTransform xslTran;
            //XmlElement root;
            //XPathNavigator nav;
            //XmlTextWriter writer;
            //XsltSettings settings = new XsltSettings(true, false);

            //ds = new DataSet();
            ////ds.ReadXml(xmlDataFile);
            ////ds.ReadXml(new XmlTextReader(new StringReader(sXMLEncounterDoc)));
            //StringBuilder sb = new StringBuilder();
            //sb.Append(sXMLEncounterDoc.ToString().Replace("</notes>", "").Replace("</Modules>", ""));

            //string SUB = sXMLHumanDoc.ToString().Substring(0, sXMLHumanDoc.LastIndexOf("?>") + 2);

            //sb.Append(sXMLHumanDoc.ToString().Replace(SUB, "").Replace("<notes>", "").Replace("<Modules>", ""));
            //ds.ReadXml(new XmlTextReader(new StringReader(sb.ToString())));

            //xmlDoc = new XmlDataDocument(ds);
            //xslTran = new XslCompiledTransform();
            //// xslTran.Load(xsltFile);
            //UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation Send Fax XSLT Load : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            //xslTran.Load(xsltFile, settings, new XmlUrlResolver());
            //UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation Send Fax XSLT Load : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            //root = xmlDoc.DocumentElement;


            //nav = root.CreateNavigator();
            //if (File.Exists(outputDocument))
            //{
            //    File.Delete(outputDocument);
            //}
            //writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8);
            //xslTran.Transform(nav, writer);
            //writer.Close();
            //writer = null;
            //nav = null;
            //root = null;
            //xmlDoc = null;
            //ds = null;
            //Jira #CAP-344 - NewCode
            UtilityManager.PrintPDFUsingXSLT(sXMLEncounterDoc, sXMLHumanDoc, xsltFile, outputDocument, sGroup_ID_Log);
            System.IO.FileInfo file = new System.IO.FileInfo(outputDocument);
            //  string htmlString = System.IO.File.ReadAllText(outputDocument);

            //  IList<PatientPane> lstpane = new List<PatientPane>();
            //   lstpane = ClientSession.PatientPaneList.ToList<PatientPane>();
            string Patient_Name = "";
            // if(lstpane.Count>0)
            // Patient_Name=lstpane[0].Last_Name + "," + "" + lstpane[0].First_Name + " " + lstpane[0].MI;

            string Encounter_signedDate = "";

            string Encounter_Provider_Name = "";
            string Provider_Speciality = "";
            TextReader EncXMLContentdoc = new StringReader(sXMLEncounterDoc);
            XDocument xmlDocumentType = XDocument.Load(EncXMLContentdoc);
            TextReader HumanXMLContentdoc = new StringReader(sXMLHumanDoc);
            XDocument xmlDocument = XDocument.Load(HumanXMLContentdoc);
            string Encounter_Provider_id = "";

            foreach (XElement elements in xmlDocument.Descendants("HumanList"))
            {

                foreach (XElement Human in elements.Elements())
                {
                    if (Human.Attribute("First_Name") != null && Human.Attribute("Last_Name") != null && Human.Attribute("MI") != null)
                        Patient_Name = Human.Attribute("Last_Name").Value + "," + "" + Human.Attribute("First_Name").Value + " " + Human.Attribute("MI").Value;

                }
            }





            string Encounter_Reviewed_signedDate = "";
            string Encounter_Reviewed_Name = "";
            string Encounter_Reviewed_Id = "";
            // XDocument xmlDocumentType = XDocument.Load(strXmlEncounterPath);

            foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value);
                    Encounter_Reviewed_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm tt");

                    DateTime dtPro = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                    Encounter_signedDate = UtilityManager.ConvertToLocal(dtPro).ToString("dd-MMM-yyyy hh:mm tt");

                    Encounter_Reviewed_Id = Encounter.Attribute("Encounter_Provider_Review_ID").Value;

                }

                //if (Encounter_signedDate == "" || Encounter_signedDate == "01-Jan-0001 12:00:00 AM")
                //{
                // foreach (XElement Encounter in elements.Elements())
                // {
                //     DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                //     Encounter_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm:ss tt");
                //}
                //}
            }
            //Provider Name 
            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    Encounter_Provider_Name = Encounter.Value;
                    break;
                }
            }
            //Provider Reviewed Name 
            if (Encounter_Reviewed_Id != "")
            {
                string xmlFilepathUser = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
                if (File.Exists(xmlFilepathUser))
                {
                    XmlDocument xdoc = new XmlDocument();
                    XmlTextReader itext = new XmlTextReader(xmlFilepathUser);
                    xdoc.Load(itext);
                    itext.Close();
                    XmlNodeList xnodelst = xdoc.GetElementsByTagName("User");
                    if (xnodelst != null && xnodelst.Count > 0)
                    {
                        foreach (XmlNode xnode in xnodelst)
                        {
                            if (xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() != "0" && xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() == Encounter_Reviewed_Id)
                            {
                                Encounter_Reviewed_Name = xnode.Attributes.GetNamedItem("person_name").Value;
                            }
                        }
                    }
                }
            }


            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    // if (Encounter.LastAttribute != null && Encounter.LastAttribute.Name.ToString() == "Specialties")
                    if (Encounter.Attribute("Specialties") != null)
                    {
                        Provider_Speciality = Encounter.LastAttribute.Value;
                        break;


                    }


                }

            }



            string strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
            //string strfooterProviderReviewed = "I " + Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate +
            //     " have reviewed the chart and agree with the management plan with the changes to the plan as indicated.";

            string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
            StaticLookupManager staticMngr = new StaticLookupManager();
            string strfooterProviderReviewed = string.Empty;
            IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
            if (CommonList.Count > 0)
                strfooterProviderReviewed = CommonList[0].Value.Replace("<Physician>", Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate).Replace("|", "");

            string htmlString = System.IO.File.ReadAllText(outputDocument);
            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            var strBody = new StringBuilder();
            string imgpath = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath"];
            string imgpath_phy = System.Configuration.ConfigurationSettings.AppSettings["phyConsultationLogoPath"];
            string imgpath_PalliativeCare = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath_PalliativeCare"];
            string imgpath_Integrum = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath_Integrum"];

            string phyheaderstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_phy + "' alt='logo' height='100' width='200'></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string header = "<table style='width:100%'><tr><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerLogo_PalliativeCare = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_PalliativeCare + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerLogo_Integrum = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_Integrum + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string finalHeadeString = string.Empty;
            string PatientName = "RE: " + Patient_Name;

            string sProHeader = System.Configuration.ConfigurationSettings.AppSettings["ProgressNotesMainHeader"];
            if (System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoIntegrum"] != null && System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoIntegrum"] == "ALL")
                finalHeadeString = imgpath_Integrum;
            else if (Provider_Speciality.ToUpper().Contains("ENDOCRINOLOGY"))
                finalHeadeString = imgpath;
            else if (Encounter_Provider_id == "4198")
                finalHeadeString = imgpath_phy;
            else if (Encounter_Provider_id == "3101" || Encounter_Provider_id == "321")
                finalHeadeString = imgpath_PalliativeCare;
            else
                finalHeadeString = "";


            string strfooterF = "";

            string strfooterPA = "";
            string strfooterP = "";
            if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && Encounter_Reviewed_signedDate != "" && Encounter_Reviewed_signedDate != "01-Jan-0001 12:00 AM")
            {
                strfooterPA = strfooterProvider;

                strfooterP = strfooterProviderReviewed;
                strfooterF = " ";
                //  strfooterProvider = strfooterProvider + "<br/>" + strfooterProviderReviewed;
            }
            else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM")
            {
                strfooterF = strfooterProvider;
            }
            else
            {


                strfooterF = "";

            }




            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            // var strBody = new StringBuilder();

            string sbTop = "";

            sbTop = sbTop + htmlString;






            DirectoryInfo virdir = new DirectoryInfo(Page.MapPath("atala-capture-download/" + Session.SessionID + "/ConsultationNotesFax"));
            if (!virdir.Exists)
            {
                virdir.Create();
            }
            string strHtml = string.Empty;
            string pdfFileName = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], NotesName + "test" + ".pdf");//System.Configuration.ConfigurationSettings.AppSettings["XMLPath"] + NotesName + "test" + ".pdf";

            string filename = "atala-capture-download/" + Session.SessionID + "/ConsultationNotesFax/" + NotesName + ".pdf";

            string pdfFileNamewithHeader = Page.MapPath(filename);
            if (htmlString.Length > 0)
            {
                try
                {
                    CreatePDFFromHTMLFile(sbTop, pdfFileName);

                }
                catch (Exception ex)
                {
                    if (ex.Message.ToUpper().Contains("NO PAGES"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('1011187'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;

                    }
                    else
                        throw ex;
                }
                var reader = new PdfReader(pdfFileName);

                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {


                    var document1 = new Document(new Rectangle(625, 975));

                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = reader.NumberOfPages;



                    sProHeader = "N";
                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        var importedPage = writerpdf.GetImportedPage(reader, i);


                        var con = writerpdf.DirectContent;

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();

                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 9);

                        if (strfooterF == "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else if (strfooterPA != "" && strfooterP != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterPA, 30, 30, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterP, 30, 20, 0);

                            //ColumnText ct = new ColumnText(con);
                            //ct.SetSimpleColumn(new Phrase(new Chunk(strfooterPA, FontFactory.GetFont(FontFactory.TIMES_ITALIC, 9, Font.NORMAL))),
                            //                   35, 77, 530, 36, 14, iTextSharp.text.Element.ALIGN_LEFT | iTextSharp.text.Element.ALIGN_BOTTOM);
                            //ct.Go();

                            //ct = new ColumnText(con);
                            //ct.SetSimpleColumn(new Phrase(new Chunk(strfooterP, FontFactory.GetFont(FontFactory.TIMES_ITALIC, 9, Font.NORMAL))),
                            //                   35, 65, 530, 36, 14, iTextSharp.text.Element.ALIGN_LEFT | iTextSharp.text.Element.ALIGN_BOTTOM);
                            //ct.Go(); 

                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }

                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;

                        con.SetFontAndSize(baseFont1, 10);
                        con.SetColorFill(BaseColor.BLACK);
                        con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));

                        X += 103;

                        con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                        // string imagepath = "";

                        if (finalHeadeString != "")
                        {
                            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(finalHeadeString);
                            logo.ScaleAbsolute(50, 50);
                            logo.SetAbsolutePosition(pageSize.GetLeft(X) - 150, pageSize.GetTop(Y) - 30);
                            con.AddImage(logo);


                        }


                        con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PatientName, pageSize.GetLeft(X) + 200, pageSize.GetTop(Y) - 30, 0);
                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();
                        if (sProHeader == "Y")
                        {

                            con.AddTemplate(importedPage, 0, 0);
                        }
                        else
                            con.AddTemplate(importedPage, 0, 60);

                    }

                    document1.Close();
                    writerpdf.Close();
                    fileStream.Close();
                    fileStream.Dispose();
                }
                if (File.Exists(pdfFileName))
                {
                    File.Delete(pdfFileName);

                }
            }
            else
            {
                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {
                    var document1 = new Document(new Rectangle(625, 975));

                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = 1;


                    sProHeader = "N";

                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        var con = writerpdf.DirectContent;

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();
                        if (sProHeader == "Y")
                        {


                            //float U = 75;
                            //float S = 60f;


                        }
                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 8);

                        if (strfooterProvider != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProvider, 30, 30, 0);
                            if (strfooterProviderReviewed != "")
                                con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProviderReviewed, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else if (strfooterF != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }


                        Y += 10;
                        con.MoveTo(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));


                        #region Column1
                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;

                        con.SetFontAndSize(baseFont1, 10);
                        con.SetColorFill(BaseColor.BLACK);
                        con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                        if (finalHeadeString != "")
                        {
                            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(finalHeadeString);
                            logo.ScaleAbsolute(50, 50);
                            logo.SetAbsolutePosition(pageSize.GetLeft(X) - 150, pageSize.GetTop(Y) - 30);
                            con.AddImage(logo);


                        }


                        con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PatientName, pageSize.GetLeft(X) + 200, pageSize.GetTop(Y) - 30, 0);



                        #endregion


                        con.LineTo(pageSize.GetRight(X) + 40, pageSize.GetTop(Y));
                        con.Stroke();


                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();

                    }

                    document1.Close();
                    writerpdf.Close();
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }


            hdnFilePath.Value = pdfFileNamewithHeader;
            //For Bug ID 63721
            string sFaxFirstname = string.Empty;
            string sFaxLastName = string.Empty;
            string sFaxDOS = string.Empty;
            //if (File.Exists(strXmlHumanPath) == true)
            if (sXMLHumanDoc != string.Empty)
            {
                XmlDocument itemDoc = new XmlDocument();
                //XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                itemDoc.LoadXml(sXMLHumanDoc);
                //using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //{
                //itemDoc.Load(fs);

                //XmlText.Close();
                if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                {
                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes.Count > 0)
                    {
                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != null)
                            //Jira CAP-1588
                            //sFaxFirstname = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                            sFaxFirstname = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != null)
                            //Jira CAP-1588
                            //sFaxLastName = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                            sFaxLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();

                    }
                }
                //    fs.Close();
                //    fs.Dispose();
                //}
            }
            //DOS
            string sRefProvider = string.Empty;
            //  if (File.Exists(strXmlEncounterPath) == true)
            if (sXMLEncounterDoc != string.Empty)
            {
                XmlDocument itemDoc = new XmlDocument();
                //XmlTextReader XmlText = new XmlTextReader(strXmlEncounterPath);
                itemDoc.LoadXml(sXMLEncounterDoc);
                //using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //{
                //    itemDoc.Load(fs);

                //    XmlText.Close();
                string sDOS = string.Empty;
                //Jira CAP-1387
                string sEmailId = string.Empty;

                if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                {
                    if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes.Count > 0)
                    {
                        if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value != "")
                        {
                            sFaxDOS = "_" + Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("dd-MMM-yyyy");

                        }
                        //Jira CAP-1358
                        //if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Physician").Value != "")
                        //{

                        //    sRefProvider = " |" + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Physician").Value +
                        //        "| NPI: " + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Provider_NPI").Value +
                        //        "| Facility: " + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Facility").Value +
                        //        "| Address:" + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Address").Value +
                        //        "| Fax No:" + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Fax_No").Value +
                        //        "| Phone No:" + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Phone_No").Value;
                        //}

                        //Jira CAP-1358
                        if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Physician").Value != "")
                        {
                            //Jira CAP-1387 - Start
                            if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\PhysicianAddressDetails.xml"))
                            {
                                string sPhysicianXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\PhysicianAddressDetails.xml";
                                XmlDocument itemPhysiciandoc = new XmlDocument();
                                XmlTextReader XmlPhysicianText = new XmlTextReader(sPhysicianXmlPath);
                                itemPhysiciandoc.Load(XmlPhysicianText);

                                XmlNodeList xmlphy = itemPhysiciandoc.ChildNodes[1].ChildNodes;
                                foreach (XmlNode xmlphyitem in xmlphy)
                                {
                                    if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Provider_NPI").Value != "" && xmlphyitem.Attributes[8].Value == itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Provider_NPI").Value)
                                    {
                                        sEmailId = xmlphyitem.Attributes[9].Value;
                                        break;
                                    }
                                }
                            }
                            //Jira CAP-1387 - End

                            sRefProvider = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Physician").Value +
                                " | NPI: " + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Provider_NPI").Value +
                                " | FACILITY: " + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Facility").Value +
                                " | ADDR:" + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Address").Value +
                                " | PH:" + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Phone_No").Value +
                                " | Email:" + sEmailId +
                                " | FAX:" + itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Referring_Fax_No").Value;
                        }
                    }
                }
                //    fs.Close();
                //    fs.Dispose();
                //}
            }
            //Jira CAP-1588
            //sFaxSubject = "Consultation Notes" + sFaxLastName + sFaxFirstname + sFaxDOS;//<Patient Name>_<Date_of_service> 
            sFaxSubject = "Referral for " + sFaxLastName + " " + sFaxFirstname;
            //Cap - 1414, 1415, 1449
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "EFax", "OpenEfax('" + sFaxSubject + "','" + sRefProvider + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "EFax", "OpenEfax('" + sFaxSubject + "','" + sRefProvider + "','Y');", true);

            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation Send Fax : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
        }


        [WebMethod(EnableSession = true)]
        public static string CheckServiceProcedureCodeStatus(string Encounter)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            #region CAP-1355
            //string sReturn = string.Empty;


            //WFObjectManager WFMngr = new WFObjectManager();
            //WFObject WFBillingObj = new WFObject();
            //WFBillingObj = WFMngr.GetByObjectSystemId(ClientSession.EncounterId, "BILLING");

            //if (WFBillingObj.Current_Process == "")
            //{
            //    sReturn = "110092";
            //    goto ReturnResult;
            //}

            //if (WFBillingObj.Current_Process == "BATCHING_COMPLETE")
            //{
            //    sReturn = "670002";
            //    goto ReturnResult;
            //}

            //EncounterManager EncMngr = new EncounterManager();
            //IList<Encounter> EncObj = new List<Encounter>();
            //EncObj = EncMngr.GetEncounterByEncounterID(ClientSession.EncounterId);

            //if (EncObj.Count > 0)
            //{
            //    if (EncObj[0].Batch_Status == "CLOSED")
            //    {
            //        sReturn = "670002";
            //        goto ReturnResult;
            //    }

            //    if (EncObj[0].Assigned_Med_Asst_User_Name == ClientSession.UserName)
            //    {
            //        sReturn = "Success";
            //        ClientSession.FillEncounterandWFObject = null;
            //    }
            //    else if (Convert.ToUInt64(EncObj[0].Encounter_Provider_ID) == ClientSession.CurrentPhysicianId || (EncObj[0].Encounter_Provider_Review_ID != 0 && Convert.ToUInt64(EncObj[0].Encounter_Provider_Review_ID) == ClientSession.CurrentPhysicianId))
            //    {
            //        sReturn = "Success";
            //        ClientSession.FillEncounterandWFObject = null;
            //    }
            //    else
            //        sReturn = "670003";
            //}
            //else
            //    sReturn = "110092";

            //ReturnResult: var result = new { Return = sReturn };
            //return JsonConvert.SerializeObject(result);
            #endregion
            ClientSession.FillEncounterandWFObject = null;
            var result = new { Return = "Success" };
            return JsonConvert.SerializeObject(result);
        }
    }

}