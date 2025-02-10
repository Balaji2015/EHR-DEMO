using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using System.Text;
using Acurus.Capella.Core;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Acurus.Capella.UI.RCopia;
using Acurus.Capella.Core.DTO;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml;
using System.Web.SessionState;
using System.Web.Services;
using System.Drawing;
using Newtonsoft.Json;
using log4net;
using System.Xml.XPath;
using System.Xml.Xsl;
using Acurus.Capella.Core.DTOJson;
using iTextSharp.text;

namespace Acurus.Capella.UI
{
    public partial class frmRCopiaToolbar : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger("Error");
        static string RcopiaRefill = string.Empty;
        static string Rcopiarx_pending = string.Empty;
        static string Rcopiarx_need_signing = string.Empty;
        static string Rcopiarx_change = string.Empty;
        static int NotificationCount = 0;
        static string strNotifiaction = string.Empty;
        static string NotificationSummaryText = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //LoadRCopiaNotification();

        }
        class VitalsRanges
        {
            public string vitalname;
            public string range;
            public string format;
        }
        //Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
        class ReviewProtocols
        {
            public string UserID { get; set; }
            public string UserName { get; set; }
            public string Protocol { get; set; }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string LoadRCopiaNotification()
        {

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            if (ClientSession.Is_RCopia_Notification_Required != "Y" || ClientSession.RCopiaUserName == string.Empty)
            {
                return "";
            }

            RCopiaGenerateXML objrcopGenXML = new RCopiaGenerateXML();
            string sInputXML = string.Empty;
            //System.Threading.Thread.Sleep(400);
            sInputXML = objrcopGenXML.CreateGetNotificationCountXML(ClientSession.LegalOrg);
            string sOutputXML = string.Empty;
            RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(ClientSession.LegalOrg);
            //System.Threading.Thread.Sleep(400);
            sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.DownloadAddress + sInputXML, 1);
            if (sOutputXML != null && sOutputXML.StartsWith("HttpPostError") == true)
            {
                return sOutputXML;
            }
            RCopia.RCopiaXMLResponseProcess objRcopResponseXML = new RCopiaXMLResponseProcess();
            //Jira CAP-1367
            //RCopia.RCopiaXMLResponseProcess.ilstNotification.Clear();
            IList<Rcopia_NotificationDTO> ilstNotification;
            ////System.Threading.Thread.Sleep(400);
            //objRcopResponseXML.ReadXMLResponse(sOutputXML);
            objRcopResponseXML.ReadXMLResponse(sOutputXML,out ilstNotification);
            string returnText = "";
            try
            {
                //Jira CAP-1367
                //returnText = FillRCopiaNotification();
                returnText = FillRCopiaNotification(ilstNotification);
            }
            catch
            {
            }

            return returnText;
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string LoadImportedPatients()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<Human> lstHuman = new List<Human>();
            HumanManager objhuman = new HumanManager();
            string[] AppointmentStatus = { "WAITING FOR CALL" };
            lstHuman = objhuman.GetHumanbyAppointmentStatus(AppointmentStatus);

            return lstHuman.Count.ToString();


        }

        //Jira CAP-1367
        // public static string FillRCopiaNotification()
        public static string FillRCopiaNotification(IList<Rcopia_NotificationDTO> ilstnotification)
        {
            string strTex = string.Empty;
            //System.Threading.Thread.Sleep(400);
            //Jira CAP-1367
            //IList<Rcopia_NotificationDTO> ilstnotification = RCopia.RCopiaXMLResponseProcess.ilstNotification;
            if (ilstnotification != null)
            {
                for (int i = 0; i < ilstnotification.Count; i++)
                {
                    if (ilstnotification[i].Type.ToLower() == "refill")
                    {
                        RcopiaRefill = ilstnotification[i].Type.ToUpper() + " : " + ilstnotification[i].Number;
                    }
                    else if (ilstnotification[i].Type.ToLower() == "rx_pending")
                    {
                        Rcopiarx_pending = ilstnotification[i].Type.ToUpper() + " : " + ilstnotification[i].Number;
                    }
                    else if (ilstnotification[i].Type.ToLower() == "rx_need_signing")
                    {
                        Rcopiarx_need_signing = ilstnotification[i].Type.ToUpper() + " : " + ilstnotification[i].Number;
                    }
                    else if (ilstnotification[i].Type.ToLower() == "rxchange")
                    {
                        Rcopiarx_change = ilstnotification[i].Type.ToUpper() + " : " + ilstnotification[i].Number;
                    }

                }
            }

            return (RcopiaRefill + "#$%" + Rcopiarx_pending + "#$%" + Rcopiarx_need_signing + "#$%" + Rcopiarx_change + "#$%");
        }

        //BugID:48547
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string LoadMailClinicalInfo()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string fileCnt = "0", inboxCnt = "0";
            #region MailBoxCount
            ActivityLogManager activityMngr = new ActivityLogManager();
            IList<ActivityLog> activityLog = new List<ActivityLog>();
            IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
            string emailId = string.Empty;
            if (ClientSession.PhysicianId != 0)
            {
                // ilstPhysicianLibrary = activityMngr.ilstPhysicianLibraryforGetMailID(ClientSession.PhysicianId);
                //XmlDocument xmldoc1 = new XmlDocument();
                //string sPhysicianXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\PhysicianAddressDetails.xml";
                //XmlDocument itemPhysiciandoc = new XmlDocument();
                //XmlTextReader XmlPhysicianText = new XmlTextReader(sPhysicianXmlPath);
                //itemPhysiciandoc.Load(XmlPhysicianText);

                //XmlNodeList xmlphy = itemPhysiciandoc.GetElementsByTagName("p" + ClientSession.PhysicianId);
                //if (xmlphy.Count > 0)
                //{

                //    emailId = xmlphy[0].Attributes[9].Value;

                //}
                //CAP-2780
                PhysicianAddressDetailsList physicianAddressDetailsList = ConfigureBase<PhysicianAddressDetailsList>.ReadJson("PhysicianAddressDetails.json");
                if (physicianAddressDetailsList != null)
                {
                    var matchingAddress = physicianAddressDetailsList.PhysicianAddress
                                                     .FirstOrDefault(address => address.Physician_Library_ID == ClientSession.PhysicianId.ToString());

                    if (matchingAddress != null)
                    {
                        emailId = matchingAddress?.Physician_EMail ?? string.Empty;
                    }
                }

            }
            activityLog = activityMngr.GetInboxEntries(emailId);
            if (activityLog != null && activityLog.Count > 0)
                inboxCnt = activityLog.Count().ToString();
            #endregion
            #region CCFileCount
            string PhiMailDirectory = System.Configuration.ConfigurationManager.AppSettings["phiMailDownloadDirectory"].ToString() + "\\" + ClientSession.PhysicianId;
            if (!Directory.Exists(PhiMailDirectory))
            {
                Directory.CreateDirectory(PhiMailDirectory);
            }
            frmBrowse browse = new frmBrowse();
            browse.ReceiveMailDownload(System.Configuration.ConfigurationManager.AppSettings["PhyDirectMail"].ToString(), true);
            if (Directory.Exists(PhiMailDirectory))
            {
                DirectoryInfo directorySelected = new DirectoryInfo(PhiMailDirectory);
                FileInfo[] XmlFile = directorySelected.GetFiles("*.xml");
                if (XmlFile != null && XmlFile.Count() > 0)
                    fileCnt = XmlFile.Count().ToString();
            }
            #endregion


            var countInfo = new
            {
                MailCount = inboxCnt,
                FileCount = fileCnt,

            };
            return JsonConvert.SerializeObject(countInfo);
        }


        public static string FillText(string Heading, string strRule, string Message, string Attributes, string Developer, string FundingSource, string link, string ReleaseDate, string Classification, string ResolveScreen)
        {

            if (Heading != string.Empty && Heading.Trim() != "")
            {
                NotificationCount = NotificationCount + 1;
                if (Classification != string.Empty && Classification.Trim() != "")
                    strNotifiaction += "<u>[#" + Heading + " - " + Classification + "]</u>" + Environment.NewLine;
                else
                    strNotifiaction += "<u>[#" + Heading + "]</u>" + Environment.NewLine;
            }
            if (strRule != string.Empty && strRule.Trim() != "")
            {
                strNotifiaction += "[ Rule: ]" + strRule;
            }
            if (Message != string.Empty && Message.Trim() != "")
            {
                strNotifiaction += "[ Message: ]" + Message + Environment.NewLine;
            }
            if (Attributes != string.Empty && Attributes.Trim() != "")
            {
                strNotifiaction += Environment.NewLine + "<p style='font-size: 12.5px;font-family: Microsoft Sans Serif;'>###" + link + "  target='_blank' ><b> Citation:</b>!" + Attributes + "</p>" + Environment.NewLine;
            }
            if (Developer != string.Empty && Developer.Trim() != "")
            {
                strNotifiaction += "[# Developer: ]" + Developer + "  |  ";
            }
            if (FundingSource != string.Empty && FundingSource.Trim() != "")
            {
                strNotifiaction += "[# Funding Source: ]" + FundingSource + "  |  ";
            }
            if (ReleaseDate != string.Empty && ReleaseDate.Trim() != "")
            {
                strNotifiaction += "[# Release Date: ]" + ReleaseDate + Environment.NewLine;
            }
            if (ResolveScreen != string.Empty && ResolveScreen.Trim() != "")
            {
                strNotifiaction += "~~RESOLVE~~" + ResolveScreen;
            }

            if (Classification == "MACRA MEASURE")
            {
                if (NotificationSummaryText == string.Empty)
                {
                    NotificationSummaryText = "<ul><li>" + Heading + "</li>";
                }
                else
                {
                    NotificationSummaryText += "<li>" + Heading + "</li>";
                }
            }
            return strNotifiaction += "~@ ";
        }




        public static string ExtractBetween(string text, string start, string end)
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



        [WebMethod(EnableSession = true)]
        public static string[] LoadPatientSummaryBar(string EncID, string Enc_DOS)
        {

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return new string[] { "Session Expired" };
            }
            string sGroup_ID_Log = "0-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable("0", ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "RCopiaToolbar : Start", DateTime.Now, sGroup_ID_Log, "frmRCopiaToolbar");
            UtilityManager.inserttologgingtable("0", ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "RCopiaToolbar - LoadPatientSummaryBar API : Start", DateTime.Now, sGroup_ID_Log, "frmRCopiaToolbar");
            FillPatientSummaryBarDTO objFillChart = new FillPatientSummaryBarDTO();
            EncounterManager EncMngr = new EncounterManager();
            UInt64 encId = 0;
            DateTime encDOS = DateTime.MinValue;
            if (EncID != null && EncID != string.Empty && ulong.TryParse(EncID, out encId))
                encId = Convert.ToUInt64(EncID);
            if (Enc_DOS != null && Enc_DOS != string.Empty)
            {
                encDOS = Convert.ToDateTime(Enc_DOS.TrimEnd('-'));
                UtilityManager.ConvertToUniversal(encDOS).ToString("yyyy-MM-dd hh:mm:ss");
            }

            UtilityManager.inserttologgingtable("0", ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "RCopiaToolbar - LoadPatientSummaryBar - Call LoadPatientSummaryBarDTo : Start", DateTime.Now, sGroup_ID_Log, "frmRCopiaToolbar");
            objFillChart = LoadPatientSummaryBarDTo(encId, encDOS);
            UtilityManager.inserttologgingtable("0", ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "RCopiaToolbar - LoadPatientSummaryBar - Call LoadPatientSummaryBarDTo : End", DateTime.Now, sGroup_ID_Log, "frmRCopiaToolbar");
            //objFillChart = EncMngr.LoadPatientSummaryBar(ClientSession.Selectedencounterid, ClientSession.HumanId, UtilityManager.ConvertToLocal(DateTime.UtcNow), ClientSession.UserName);
            IList<ChiefComplaints> lstchiefcompliants = new List<ChiefComplaints>();
            IList<NonDrugAllergy> lstnondrugallergy = new List<NonDrugAllergy>();
            IList<Rcopia_Allergy> lstdrugallergy = new List<Rcopia_Allergy>();
            IList<Rcopia_Medication> lstMedication = new List<Rcopia_Medication>();
            IList<ProblemList> lstProblem = new List<ProblemList>();
            IList<PatientResults> Vitalslist = new List<PatientResults>();
            lstchiefcompliants = objFillChart.ChiefComplaintList;
            lstdrugallergy = objFillChart.AllergyList;
            lstnondrugallergy = objFillChart.NonDrugAllergyList;
            lstProblem = objFillChart.PblmMedList;
            lstMedication = objFillChart.MedicationList;
            Vitalslist = objFillChart.VitalsList;
            var toolTipcomplaints = string.Empty;
            var toolTipAllergies = string.Empty;
            var toolTipProblem = string.Empty;
            var toolTipVitals = string.Empty;
            var toolTipMedication = string.Empty;
            string[] strarray = new string[10];
            UtilityManager objmngr = new UtilityManager();
            StringBuilder sbToolTip = new StringBuilder();
            string strchiefcomplaints = objmngr.GetChiefComplaintsInfo(lstchiefcompliants, out toolTipcomplaints); objmngr.GetChiefComplaintsInfo(lstchiefcompliants, out toolTipcomplaints);
            string strAllergies = objmngr.GetAllergyInfo(lstnondrugallergy, lstdrugallergy, out toolTipAllergies);
            string strProblmlist = objmngr.GetProblemList(lstProblem, out toolTipProblem);
            string strVitals = objmngr.GetVitalsInfo(Vitalslist, out toolTipVitals);
            string strmedication = objmngr.GetMedication(objFillChart, out toolTipMedication);
            strarray[0] = strAllergies;
            strarray[1] = strchiefcomplaints;
            strarray[2] = strProblmlist;
            strarray[3] = strVitals;
            strarray[4] = strmedication;
            strarray[5] = toolTipAllergies;
            strarray[6] = toolTipcomplaints;
            strarray[7] = toolTipProblem;
            strarray[8] = toolTipVitals;
            strarray[9] = toolTipMedication;
            UtilityManager.inserttologgingtable("0", ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "RCopiaToolbar - LoadPatientSummaryBar API : End", DateTime.Now, sGroup_ID_Log, "frmRCopiaToolbar");
            UtilityManager.inserttologgingtable("0", ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "RCopiaToolbar : End", DateTime.Now, sGroup_ID_Log, "frmRCopiaToolbar");
            return strarray;
        }
        /*  LoadPatientSummaryBar with XSLT XML transformation 
        [WebMethod(EnableSession = true)]
        public static string[] LoadPatientSummaryBar(string EncID, string Enc_DOS)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return new string[] { "Session Expired" };
            }
            string sGroup_ID_Log = "0-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable("0", ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "RCopiaToolbar : Start", DateTime.Now, sGroup_ID_Log, "frmRCopiaToolbar");
            UtilityManager.inserttologgingtable("0", ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "RCopiaToolbar - LoadPatientSummaryBar API : Start", DateTime.Now, sGroup_ID_Log, "frmRCopiaToolbar");
            var toolTipcomplaints = string.Empty;
            var toolTipAllergies = string.Empty;
            var toolTipProblem = string.Empty;
            var toolTipVitals = string.Empty;
            var toolTipMedication = string.Empty;
            string strchiefcomplaints = string.Empty;
            string strAllergies = string.Empty;
            string strProblmlist = string.Empty;
            string strVitals = string.Empty;
            string strmedication = string.Empty;
            string[] strarray = new string[10];
            string xsltFile = string.Empty;
            string Human_Enc_id = string.Empty;

            UInt64 encId = 0;
            DateTime encDOS = DateTime.MinValue;
            if (EncID != null && EncID != string.Empty && ulong.TryParse(EncID, out encId))
                encId = Convert.ToUInt64(EncID);

            if (encId > 0)
                ClientSession.EncounterId = encId;

            if (ClientSession.EncounterId != 0)
            {
                xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Patient_Summary_Encounter.xsl");
                Human_Enc_id = "Encounter_" + ClientSession.EncounterId + ".xml";

            }
            else
            {
                xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Patient_Summary_Human.xsl");
                Human_Enc_id = "Human_" + ClientSession.HumanId + ".xml";
            }


            string strXmlEncounterPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], Human_Enc_id);
            if (File.Exists(strXmlEncounterPath) == true && xsltFile != null)
            {
                UtilityManager.inserttologgingtable("0", ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "RCopiaToolbar - LoadPatientSummaryBar - Call LoadPatientSummaryBarDTo : Start", DateTime.Now, sGroup_ID_Log, "frmRCopiaToolbar");
                string output = string.Empty;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(strXmlEncounterPath);
                XslTransform xslTransform = new XslTransform();
                xslTransform.Load(xsltFile);
                StringWriter stringWriter = new StringWriter();
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
                xmlTextWriter.Formatting = System.Xml.Formatting.Indented;
                xslTransform.Transform(xmlDocument, null, xmlTextWriter);
                xmlTextWriter.Flush();
                output = stringWriter.ToString().Replace("\r\n<br />", "<br />").Replace("\r\n<br/>", "<br/>");
                if (output != string.Empty)
                {
                    strAllergies = "Allergies:" + ExtractBetween(output, "Allergies:", "Chief Complaints:");
                    if (strAllergies.Trim().Replace("\r\n", "").Replace("<br />", "") != "Allergies:")
                        toolTipAllergies = strAllergies;

                    strchiefcomplaints = "Chief Complaints:" + ExtractBetween(output, "Chief Complaints:", "Problem List:");
                    if (strchiefcomplaints.Trim().Replace("\r\n", "").Replace("<br />", "") != "Chief Complaints:")
                        toolTipcomplaints = strchiefcomplaints;

                    strProblmlist = "Problem List:" + ExtractBetween(output, "Problem List:", "Vitals:");
                    if (strProblmlist.Trim().Replace("\r\n", "").Replace("<br />", "") != "Problem List:")
                        toolTipProblem = strProblmlist;

                    strVitals = "Vitals:" + ExtractBetween(output, "Vitals:", "Medications:");
                    if (strVitals.Trim().Replace("\r\n", "").Replace("<br />", "") != "Vitals:")
                        toolTipVitals = strVitals;

                    strmedication = "Medications:" + ExtractBetween(output, "Medications:", "Medications End:");
                    if (strmedication.Trim().Replace("\r\n", "").Replace("<br />", "") != "Medications:")
                        toolTipMedication = strmedication;

                }
                UtilityManager.inserttologgingtable("0", ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "RCopiaToolbar - LoadPatientSummaryBar - Call LoadPatientSummaryBarDTo : End", DateTime.Now, sGroup_ID_Log, "frmRCopiaToolbar");
            }

            strarray[0] = strAllergies;
            strarray[1] = strchiefcomplaints;
            strarray[2] = strProblmlist;
            strarray[3] = strVitals;
            strarray[4] = strmedication;
            strarray[5] = toolTipAllergies;
            strarray[6] = toolTipcomplaints;
            strarray[7] = toolTipProblem;
            strarray[8] = toolTipVitals;
            strarray[9] = toolTipMedication;
            UtilityManager.inserttologgingtable("0", ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "RCopiaToolbar - LoadPatientSummaryBar API : End", DateTime.Now, sGroup_ID_Log, "frmRCopiaToolbar");
            UtilityManager.inserttologgingtable("0", ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "RCopiaToolbar : End", DateTime.Now, sGroup_ID_Log, "frmRCopiaToolbar");
            return strarray;
        }
        */

        [WebMethod(EnableSession = true)]
        public static string getInsuranceDetails()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            EncounterManager objManager = new EncounterManager();
            string[] arry = objManager.GetInusranceDetails(ClientSession.HumanId);
            string sPriPlan = string.Empty;
            string sPriCarrier = string.Empty;
            string sSecPlan = string.Empty;
            string sSecCarrier = string.Empty;
            string tooltip_txt = string.Empty;
            if (arry[1] != null)
            {
                if (arry[0] == "PRIMARY")
                {
                    sPriPlan = arry[2].ToString();
                    sPriCarrier = arry[3].ToString();
                }
                if (arry[0] == "SECONDARY")
                {
                    sSecPlan = arry[2].ToString();
                    sSecCarrier = arry[3].ToString();
                }

            }
            if (arry[5] != null)
            {
                if (arry[4] == "PRIMARY")
                {
                    sPriPlan = arry[6].ToString();
                    sPriCarrier = arry[7].ToString();
                }
                if (arry[4] == "SECONDARY")
                {
                    sSecPlan = arry[6].ToString();
                    sSecCarrier = arry[7].ToString();
                }

            }

            if (sPriPlan != string.Empty)
            {
                tooltip_txt += "Pri Plan:" + sPriCarrier + " - " + sPriPlan + "\n ";
            }
            else
            {
                tooltip_txt += "Pri Plan: No Primary Insurance \n ";
            }
            if (sSecPlan != string.Empty)
            {
                tooltip_txt += "Sec Plan:" + sSecCarrier + " - " + sSecPlan + "\n   ";
            }
            else
            {
                tooltip_txt += "Sec Plan: No Secondary Insurance ";
            }
            return tooltip_txt;
        }

        [WebMethod(EnableSession = true)]
        public static string CheckACOEligiblity(string strHumanID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            HumanManager objHumanManager = new HumanManager();
            ulong HumanID = Convert.ToUInt32(strHumanID);
            string IsAcoElgible = objHumanManager.IsACOEligibility(HumanID);
            return IsAcoElgible;

        }
        public static FillPatientSummaryBarDTO LoadPatientSummaryBarDTo(UInt64 encId, DateTime encDos)
        {

            FillPatientSummaryBarDTO objFillPatientChart = new FillPatientSummaryBarDTO();
            DateTime CurrentDOS = DateTime.MinValue;
            if (encId > 0)
                ClientSession.EncounterId = encId;

            if (ClientSession.EncounterId != 0)//BugID:52634,52632 
            {
                IList<string> ilstPatientSummaryBarTagEncounterList = new List<string>();
                ilstPatientSummaryBarTagEncounterList.Add("EncounterList");
                ilstPatientSummaryBarTagEncounterList.Add("ChiefComplaintsList");

                IList<object> ilstEncBlobFinal = new List<object>();
                ilstEncBlobFinal = UtilityManager.ReadBlob(ClientSession.EncounterId, ilstPatientSummaryBarTagEncounterList);

                if (ilstEncBlobFinal != null && ilstEncBlobFinal.Count > 0)
                {
                    if (ilstEncBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstEncBlobFinal[0]).Count; iCount++)
                        {
                            objFillPatientChart.EncounterIDList.Add(Convert.ToUInt32(((Encounter)((IList<object>)ilstEncBlobFinal[0])[iCount]).Id));

                            objFillPatientChart.EncounterDateList.Add(Convert.ToDateTime(((Encounter)((IList<object>)ilstEncBlobFinal[0])[iCount]).Date_of_Service));
                        }
                    }

                    if (ilstEncBlobFinal[1] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstEncBlobFinal[1]).Count; iCount++)
                        {
                            if (((ChiefComplaints)((IList<object>)ilstEncBlobFinal[1])[iCount]).HPI_Element == "Chief Complaints")
                            {
                                objFillPatientChart.ChiefComplaintList.Add((ChiefComplaints)((IList<object>)ilstEncBlobFinal[1])[iCount]);
                            }
                        }
                    }
                }

                //string FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                //if (File.Exists(strXmlFilePath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                //    XmlNodeList xmlTagName = null;
                //    // itemDoc.Load(XmlText);
                //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);

                //        XmlText.Close();

                //        /* objFillPatientChart.EncounterIDList.Add(ClientSession.EncounterId);
                //          objFillPatientChart.EncounterDateList.Add(encDos);*/



                //        //if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                //        //{
                //        //    xmlTagName = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes;

                //        //    if (xmlTagName.Count > 0)
                //        //    {
                //        //        for (int j = 0; j < xmlTagName.Count; j++)
                //        //        {
                //        //            objFillPatientChart.EncounterIDList.Add(Convert.ToUInt32(xmlTagName[j].Attributes.GetNamedItem("Id").Value));

                //        //            objFillPatientChart.EncounterDateList.Add(Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Date_of_Service").Value));
                //        //            //if (xmlTagName[j].Attributes[0].Value == ClientSession.EncounterId.ToString())
                //        //            //    CurrentDOS = Convert.ToDateTime(xmlTagName[j].Attributes[4].Value);
                //        //        }
                //        //        //    for (int j = 0; j < xmlTagName.Count; j++)
                //        //        //    {
                //        //        //       if (EncounterID.Count < 2)
                //        //        //{
                //        //        //    if (EncounterLst[k].Date_of_Service <= CurrentDOS)
                //        //        //    {
                //        //        //        if (CurrentDOS.ToString() != DateTime.MinValue.ToString())
                //        //    }
                //        //}
                //        if (itemDoc.GetElementsByTagName("ChiefComplaintsList")[0] != null)
                //        {
                //            xmlTagName = itemDoc.GetElementsByTagName("ChiefComplaintsList")[0].ChildNodes;

                //            XmlElement xmlElemnt = itemDoc.DocumentElement;
                //            XmlNode xmlCCNode = null;
                //            xmlCCNode = xmlElemnt.SelectSingleNode("/notes/Modules/ChiefComplaintsList/ChiefComplaints[@HPI_Element='Chief Complaints']");
                //            if (xmlCCNode != null)
                //            {
                //                ChiefComplaints ChiefComplaints = new ChiefComplaints();
                //                ChiefComplaints.HPI_Value = xmlCCNode.Attributes.GetNamedItem("HPI_Value").Value.ToString();
                //                objFillPatientChart.ChiefComplaintList.Add(ChiefComplaints);
                //            }

                //            //if (xmlTagName.Count > 0)
                //            //{
                //            //    for (int j = 0; j < xmlTagName.Count; j++)
                //            //    {
                //            //        if (xmlTagName[j].Attributes.GetNamedItem("HPI_Element").Value == "Chief Complaints")
                //            //        {

                //            //            string TagName = xmlTagName[j].Name;
                //            //            XmlSerializer xmlserializer = new XmlSerializer(typeof(ChiefComplaints));
                //            //            ChiefComplaints ChiefComplaints = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ChiefComplaints;
                //            //            IEnumerable<PropertyInfo> propInfo = null;
                //            //            ChiefComplaints = (ChiefComplaints)ChiefComplaints;
                //            //            propInfo = from obji in ((ChiefComplaints)ChiefComplaints).GetType().GetProperties() select obji;

                //            //            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //            //            {
                //            //                XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //            //                {
                //            //                    foreach (PropertyInfo property in propInfo)
                //            //                    {
                //            //                        if (property.Name == nodevalue.Name)
                //            //                        {
                //            //                            if (property.PropertyType.Name.ToUpper() == "UINT64")
                //            //                                property.SetValue(ChiefComplaints, Convert.ToUInt64(nodevalue.Value), null);
                //            //                            else if (property.PropertyType.Name.ToUpper() == "STRING")
                //            //                                property.SetValue(ChiefComplaints, Convert.ToString(nodevalue.Value), null);
                //            //                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //            //                                property.SetValue(ChiefComplaints, Convert.ToDateTime(nodevalue.Value), null);
                //            //                            else if (property.PropertyType.Name.ToUpper() == "INT32")
                //            //                                property.SetValue(ChiefComplaints, Convert.ToInt32(nodevalue.Value), null);
                //            //                            else
                //            //                                property.SetValue(ChiefComplaints, nodevalue.Value, null);
                //            //                        }
                //            //                    }
                //            //                }
                //            //            }
                //            //            objFillPatientChart.ChiefComplaintList.Add(ChiefComplaints);
                //            //        }
                //            //    }

                //            //}
                //        }
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}
                /* if (objFillPatientChart.EncounterDateList.Count == 0)
                     objFillPatientChart.EncounterDateList.Add(encDos);*/
            }

            IList<string> ilstPatientSummaryBarTagHumanList = new List<string>();
            ilstPatientSummaryBarTagHumanList.Add("ProblemListList");
            ilstPatientSummaryBarTagHumanList.Add("PatientResultsList");
            ilstPatientSummaryBarTagHumanList.Add("Rcopia_MedicationList");
            ilstPatientSummaryBarTagHumanList.Add("Rcopia_AllergyList");
            ilstPatientSummaryBarTagHumanList.Add("NonDrugAllergyList");

            IList<object> ilstHumanBlobFinal = new List<object>();
            ilstHumanBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstPatientSummaryBarTagHumanList);

            //CAP-2906
            if (ilstHumanBlobFinal != null && ilstHumanBlobFinal.Count > 0)
            {
                //if (ilstHumanBlobFinal[0] != null)
                if (ilstHumanBlobFinal[0] != null && ((IList<object>)ilstHumanBlobFinal[0]).Count > 0)
                {
                    if ((((IList<object>)ilstHumanBlobFinal[0])[0]) != null && (((IList<object>)ilstHumanBlobFinal[0])[0]).GetType().Name == "ProblemList")
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobFinal[0]).Count; iCount++)
                        {
                            objFillPatientChart.PblmMedList.Add((ProblemList)((IList<object>)ilstHumanBlobFinal[0])[iCount]);
                        }
                    }
                }

                //if (ilstHumanBlobFinal[1] != null)
                if (ilstHumanBlobFinal[1] != null && ((IList<object>)ilstHumanBlobFinal[1]).Count > 0)
                {
                    if ((((IList<object>)ilstHumanBlobFinal[1])[0]) != null && (((IList<object>)ilstHumanBlobFinal[1])[0]).GetType().Name == "PatientResults")
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobFinal[1]).Count; iCount++)
                        {
                            if (((PatientResults)((IList<object>)ilstHumanBlobFinal[1])[iCount]).Encounter_ID == ClientSession.EncounterId && ((PatientResults)((IList<object>)ilstHumanBlobFinal[1])[iCount]).Results_Type == "Vitals")
                            {
                                objFillPatientChart.VitalsList.Add((PatientResults)((IList<object>)ilstHumanBlobFinal[1])[iCount]);
                            }
                        }
                    }
                }

                //if (ilstHumanBlobFinal[2] != null)
                if (ilstHumanBlobFinal[2] != null && ((IList<object>)ilstHumanBlobFinal[2]).Count > 0)
                {
                    if ((((IList<object>)ilstHumanBlobFinal[2])[0]) != null && ((IList<object>)ilstHumanBlobFinal[2]).Count > 0 && (((IList<object>)ilstHumanBlobFinal[2])[0]).GetType().Name == "Rcopia_Medication")
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobFinal[2]).Count; iCount++)
                        {
                            if (((Rcopia_Medication)((IList<object>)ilstHumanBlobFinal[2])[iCount]).Human_ID == ClientSession.HumanId && ((Rcopia_Medication)((IList<object>)ilstHumanBlobFinal[2])[iCount]).Deleted == "N")
                            {
                                objFillPatientChart.MedicationList.Add((Rcopia_Medication)((IList<object>)ilstHumanBlobFinal[2])[iCount]);
                            }
                        }
                    }
                }

                //if (ilstHumanBlobFinal[3] != null)
                if (ilstHumanBlobFinal[3] != null && ((IList<object>)ilstHumanBlobFinal[3]).Count > 0)
                {
                    if ((((IList<object>)ilstHumanBlobFinal[3])[0]) != null && (((IList<object>)ilstHumanBlobFinal[3])[0]).GetType().Name == "Rcopia_Allergy")
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobFinal[3]).Count; iCount++)
                        {
                            if (((Rcopia_Allergy)((IList<object>)ilstHumanBlobFinal[3])[iCount]).Human_ID == ClientSession.HumanId && ((Rcopia_Allergy)((IList<object>)ilstHumanBlobFinal[3])[iCount]).Deleted == "N")
                            {
                                objFillPatientChart.AllergyList.Add((Rcopia_Allergy)((IList<object>)ilstHumanBlobFinal[3])[iCount]);
                            }
                        }
                    }
                }

                //if (ilstHumanBlobFinal[4] != null)
                if (ilstHumanBlobFinal[4] != null && ((IList<object>)ilstHumanBlobFinal[4]).Count > 0)
                {
                    if ((((IList<object>)ilstHumanBlobFinal[4])[0]) != null && (((IList<object>)ilstHumanBlobFinal[4])[0]).GetType().Name == "NonDrugAllergy")
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobFinal[4]).Count; iCount++)
                        {
                            if (((NonDrugAllergy)((IList<object>)ilstHumanBlobFinal[4])[iCount]).Is_Present == "Y")
                            {
                                objFillPatientChart.NonDrugAllergyList.Add((NonDrugAllergy)((IList<object>)ilstHumanBlobFinal[4])[iCount]);
                            }
                        }
                    }

                    if (objFillPatientChart.NonDrugAllergyList != null && objFillPatientChart.NonDrugAllergyList.Count > 0)
                    {
                        IList<NonDrugAllergy> lstNDACurrEnc = new List<NonDrugAllergy>();
                        lstNDACurrEnc = (from item in objFillPatientChart.NonDrugAllergyList where item.Encounter_Id == ClientSession.EncounterId select item).ToList<NonDrugAllergy>();
                        if (lstNDACurrEnc != null && lstNDACurrEnc.Count > 0)
                        {
                            objFillPatientChart.NonDrugAllergyList = lstNDACurrEnc;
                        }
                        else
                        {
                            IList<ulong> lstEncId = (from item in objFillPatientChart.NonDrugAllergyList select item.Encounter_Id).Distinct().ToList<ulong>();
                            ulong maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                            foreach (ulong item in lstEncId)
                                if (item > maxEncId && item < ClientSession.EncounterId)
                                    maxEncId = item;
                            lstNDACurrEnc = (from item in objFillPatientChart.NonDrugAllergyList where item.Encounter_Id == maxEncId select item).ToList<NonDrugAllergy>();
                            objFillPatientChart.NonDrugAllergyList = lstNDACurrEnc;
                        }
                    }
                    else
                    {
                        objFillPatientChart.NonDrugAllergyList = objFillPatientChart.NonDrugAllergyList;
                    }
                }
            }

            //string HumanFileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string HumanXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
            //if (File.Exists(HumanXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(HumanXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    XmlElement xmlHumanElemnt = null;

            //    //itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(HumanXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        try
            //        {
            //            itemDoc.Load(fs);
            //            xmlHumanElemnt = itemDoc.DocumentElement;
            //        }
            //        catch
            //        {
            //            return objFillPatientChart;
            //        }

            //        XmlText.Close();
            //        if (ClientSession.EncounterId != 0)//BugID:52634,52632 
            //        {
            //            if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes;

            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {
            //                        if (Convert.ToUInt32(xmlTagName[j].Attributes.GetNamedItem("Id").Value) == ClientSession.EncounterId)
            //                        {
            //                            objFillPatientChart.EncounterIDList.Add(Convert.ToUInt32(xmlTagName[j].Attributes.GetNamedItem("Id").Value));
            //                            objFillPatientChart.EncounterDateList.Add(Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Local_Time").Value));
            //                        }
            //                    }

            //                }
            //            }

            //        }
            //        if (objFillPatientChart.EncounterDateList.Count == 0)
            //            objFillPatientChart.EncounterDateList.Add(encDos);

            //        if (itemDoc.GetElementsByTagName("ProblemListList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("ProblemListList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {

            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(ProblemList));
            //                    ProblemList ProblemList = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ProblemList;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((ProblemList)ProblemList).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (property.Name == nodevalue.Name)
            //                                {
            //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                        property.SetValue(ProblemList, Convert.ToUInt64(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                        property.SetValue(ProblemList, Convert.ToString(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                        property.SetValue(ProblemList, Convert.ToDateTime(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                        property.SetValue(ProblemList, Convert.ToInt32(nodevalue.Value), null);
            //                                    else
            //                                        property.SetValue(ProblemList, nodevalue.Value, null);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    objFillPatientChart.PblmMedList.Add(ProblemList);
            //                }
            //            }
            //        }

            //        if (itemDoc.GetElementsByTagName("PatientResultsList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("PatientResultsList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value) == ClientSession.EncounterId && (Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Results_Type").Value) == "Vitals"))
            //                    {

            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(PatientResults));
            //                        PatientResults PatientResults = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as PatientResults;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        propInfo = from obji in ((PatientResults)PatientResults).GetType().GetProperties() select obji;

            //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                        {

            //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                            {
            //                                foreach (PropertyInfo property in propInfo)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(PatientResults, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(PatientResults, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(PatientResults, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(PatientResults, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(PatientResults, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }

            //                        }
            //                        objFillPatientChart.VitalsList.Add(PatientResults);
            //                    }
            //                }
            //            }
            //        }

            //        objFillPatientChart.Vitals = objFillPatientChart.VitalsList.Count;
            //        if (itemDoc.GetElementsByTagName("Rcopia_MedicationList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("Rcopia_MedicationList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Human_ID").Value) == ClientSession.HumanId && xmlTagName[j].Attributes.GetNamedItem("Deleted").Value == "N")
            //                    {

            //                        //if (((DateTime.Compare(objFillPatientChart.EncounterDateList[0], Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Start_Date").Value)) >= 0)
            //                        //        && (Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Stop_Date").Value) != DateTime.MinValue) && (Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Start_Date").Value) != DateTime.MinValue)
            //                        //        && (DateTime.Compare(objFillPatientChart.EncounterDateList[0], Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Stop_Date").Value)) <= 0)) ||
            //                        //       ((DateTime.Compare(objFillPatientChart.EncounterDateList[0], Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Start_Date").Value)) >= 0)
            //                        //       && (Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Stop_Date").Value) == DateTime.MinValue) && (Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Start_Date").Value) != DateTime.MinValue)
            //                        //        ))
            //                        //{

            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(Rcopia_Medication));
            //                        Rcopia_Medication Rcopia_Medication = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Rcopia_Medication;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        propInfo = from obji in ((Rcopia_Medication)Rcopia_Medication).GetType().GetProperties() select obji;

            //                        // if ((objFillPatientChart.EncounterDateList[0] > Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Start_Date").Value)) &&(objFillPatientChart.EncounterDateList[0] < Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Stop_Date").Value)))

            //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                        {
            //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                            {
            //                                foreach (PropertyInfo property in propInfo)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(Rcopia_Medication, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(Rcopia_Medication, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(Rcopia_Medication, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(Rcopia_Medication, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(Rcopia_Medication, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }

            //                        }



            //                        objFillPatientChart.MedicationList.Add(Rcopia_Medication);
            //                        //}
            //                    }
            //                }
            //            }
            //        }

            //        if (itemDoc.GetElementsByTagName("Rcopia_AllergyList")[0] != null)
            //        {
            //            XmlNodeList xmlRCopiaAllergyNode = null;
            //            xmlRCopiaAllergyNode = xmlHumanElemnt.SelectNodes("/notes/Modules/Rcopia_AllergyList/Rcopia_Allergy[@Deleted='N']");
            //            Rcopia_Allergy Rcopia_Allergy = null;
            //            if (xmlRCopiaAllergyNode != null)
            //            {
            //                foreach (XmlNode xmlnod in xmlRCopiaAllergyNode)
            //                {
            //                    Rcopia_Allergy = new Rcopia_Allergy();
            //                    Rcopia_Allergy.Allergy_Name = xmlnod.Attributes.GetNamedItem("Allergy_Name").Value.ToString();
            //                    Rcopia_Allergy.Reaction = xmlnod.Attributes.GetNamedItem("Reaction").Value.ToString();
            //                    objFillPatientChart.AllergyList.Add(Rcopia_Allergy);
            //                }
            //            }


            //            //xmlTagName = itemDoc.GetElementsByTagName("Rcopia_AllergyList")[0].ChildNodes;

            //            //if (xmlTagName.Count > 0)
            //            //{
            //            //    for (int j = 0; j < xmlTagName.Count; j++)
            //            //    {
            //            //        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Human_ID").Value) == ClientSession.HumanId && xmlTagName[j].Attributes.GetNamedItem("Deleted").Value.Equals("N"))
            //            //        {
            //            //            string TagName = xmlTagName[j].Name;
            //            //            XmlSerializer xmlserializer = new XmlSerializer(typeof(Rcopia_Allergy));
            //            //            Rcopia_Allergy Rcopia_Allergy = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Rcopia_Allergy;
            //            //            IEnumerable<PropertyInfo> propInfo = null;
            //            //            propInfo = from obji in ((Rcopia_Allergy)Rcopia_Allergy).GetType().GetProperties() select obji;

            //            //            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //            //            {

            //            //                XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //            //                {
            //            //                    foreach (PropertyInfo property in propInfo)
            //            //                    {
            //            //                        if (property.Name == nodevalue.Name)
            //            //                        {
            //            //                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //            //                                property.SetValue(Rcopia_Allergy, Convert.ToUInt64(nodevalue.Value), null);
            //            //                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //            //                                property.SetValue(Rcopia_Allergy, Convert.ToString(nodevalue.Value), null);
            //            //                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //            //                                property.SetValue(Rcopia_Allergy, Convert.ToDateTime(nodevalue.Value), null);
            //            //                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //            //                                property.SetValue(Rcopia_Allergy, Convert.ToInt32(nodevalue.Value), null);
            //            //                            else
            //            //                                property.SetValue(Rcopia_Allergy, nodevalue.Value, null);
            //            //                        }
            //            //                    }
            //            //                }

            //            //            }

            //            //            objFillPatientChart.AllergyList.Add(Rcopia_Allergy);
            //            //        }
            //            //    }
            //            //}
            //        }
            //        if (itemDoc.GetElementsByTagName("NonDrugAllergyList")[0] != null)
            //        {
            //            XmlNodeList xmlRCopiaAllergyNode = null;
            //            xmlRCopiaAllergyNode = xmlHumanElemnt.SelectNodes("/notes/Modules/NonDrugAllergyList/NonDrugAllergy[@Is_Present='Y']");
            //            NonDrugAllergy NonDrugAllergy = null;
            //            if (xmlRCopiaAllergyNode != null)
            //            {
            //                foreach (XmlNode xmlnod in xmlRCopiaAllergyNode)
            //                {
            //                    NonDrugAllergy = new NonDrugAllergy();
            //                    NonDrugAllergy.Non_Drug_Allergy_History_Info = xmlnod.Attributes.GetNamedItem("Non_Drug_Allergy_History_Info").Value.ToString();
            //                    NonDrugAllergy.Description = xmlnod.Attributes.GetNamedItem("Description").Value.ToString();
            //                    objFillPatientChart.NonDrugAllergyList.Add(NonDrugAllergy);
            //                }
            //            }

            //            ////xmlTagName = itemDoc.GetElementsByTagName("NonDrugAllergyList")[0].ChildNodes;

            //            ////if (xmlTagName.Count > 0)
            //            ////{
            //            ////    for (int j = 0; j < xmlTagName.Count; j++)
            //            ////    {
            //            ////        if ((xmlTagName[j].Attributes.GetNamedItem("Is_Present").Value == "Y"))
            //            ////        {
            //            ////            string TagName = xmlTagName[j].Name;
            //            ////            XmlSerializer xmlserializer = new XmlSerializer(typeof(NonDrugAllergy));
            //            ////            NonDrugAllergy NonDrugAllergy = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as NonDrugAllergy;
            //            ////            IEnumerable<PropertyInfo> propInfo = null;
            //            ////            propInfo = from obji in ((NonDrugAllergy)NonDrugAllergy).GetType().GetProperties() select obji;

            //            ////            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //            ////            {

            //            ////                XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //            ////                {
            //            ////                    foreach (PropertyInfo property in propInfo)
            //            ////                    {
            //            ////                        if (property.Name == nodevalue.Name)
            //            ////                        {
            //            ////                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //            ////                                property.SetValue(NonDrugAllergy, Convert.ToUInt64(nodevalue.Value), null);
            //            ////                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //            ////                                property.SetValue(NonDrugAllergy, Convert.ToString(nodevalue.Value), null);
            //            ////                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //            ////                                property.SetValue(NonDrugAllergy, Convert.ToDateTime(nodevalue.Value), null);
            //            ////                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //            ////                                property.SetValue(NonDrugAllergy, Convert.ToInt32(nodevalue.Value), null);
            //            ////                            else
            //            ////                                property.SetValue(NonDrugAllergy, nodevalue.Value, null);
            //            ////                        }
            //            ////                    }
            //            ////                }

            //            ////            }

            //            ////            objFillPatientChart.NonDrugAllergyList.Add(NonDrugAllergy);
            //            ////        }
            //            ////    }
            //            ////}
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //    if (objFillPatientChart.NonDrugAllergyList != null && objFillPatientChart.NonDrugAllergyList.Count > 0)
            //    {
            //        IList<NonDrugAllergy> lstNDACurrEnc = new List<NonDrugAllergy>();
            //        lstNDACurrEnc = (from item in objFillPatientChart.NonDrugAllergyList where item.Encounter_Id == ClientSession.EncounterId select item).ToList<NonDrugAllergy>();
            //        if (lstNDACurrEnc != null && lstNDACurrEnc.Count > 0)
            //        {
            //            objFillPatientChart.NonDrugAllergyList = lstNDACurrEnc;
            //        }
            //        else
            //        {
            //            IList<ulong> lstEncId = (from item in objFillPatientChart.NonDrugAllergyList select item.Encounter_Id).Distinct().ToList<ulong>();
            //            ulong maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
            //            foreach (ulong item in lstEncId)
            //                if (item > maxEncId && item < ClientSession.EncounterId)
            //                    maxEncId = item;
            //            lstNDACurrEnc = (from item in objFillPatientChart.NonDrugAllergyList where item.Encounter_Id == maxEncId select item).ToList<NonDrugAllergy>();
            //            objFillPatientChart.NonDrugAllergyList = lstNDACurrEnc;
            //        }
            //    }
            //    else
            //    {
            //        objFillPatientChart.NonDrugAllergyList = objFillPatientChart.NonDrugAllergyList;
            //    }
            //    if (objFillPatientChart.MedicationList.Count > 0)
            //    {
            //        objFillPatientChart.MedicationList = objFillPatientChart.MedicationList.OrderBy(x => x.Brand_Name).ToList<Rcopia_Medication>();
            //    }
            //}

            return objFillPatientChart;
        }

        [WebMethod(EnableSession = true)]
        public static string LoadNotification(string ScreenName)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string notification = string.Empty;
            NotificationCount = 0;
            strNotifiaction = string.Empty;
            NotificationSummaryText = string.Empty;
            if (ClientSession.UserCurrentProcess != null)
            {

                DateTime dtcurrenttime = UtilityManager.ConvertToUniversal();
                EncounterManager objManager = new EncounterManager();

                IList<CDSRuleMaster> lstCDSRules = new List<CDSRuleMaster>();
                if (ClientSession.CDSNotificationRule != null && ClientSession.CDSNotificationRule.Count == 0)
                {
                    CDSRuleMasterManager ClinicalDecMgr = new CDSRuleMasterManager();
                    lstCDSRules = ClinicalDecMgr.GellCDSRuleMaster();
                    ClientSession.CDSNotificationRule = lstCDSRules;
                }
                else
                {
                    lstCDSRules = ClientSession.CDSNotificationRule;
                }

                IList<Notification> lstNotification = new List<Notification>();
                //if (ClientSession.Notification != null && ClientSession.Notification.Count == 0)//commented for performance tunning 23.04.2018 ,Bug Id 53764 
                //{
                if (ClientSession.HumanId != null && ClientSession.HumanId != 0 && ClientSession.EncounterId != null)
                {
                    NotificationManager objNotificationMngr = new NotificationManager();
                    lstNotification = objNotificationMngr.GetNotificationForHumanID(ClientSession.HumanId, ClientSession.EncounterId);

                    //Sort the Notification list according to the CDS rule master table sort_order column
                    lstNotification = (from cds in lstCDSRules
                                       join notify in lstNotification
                                         on cds.Clinincal_Decision_Name equals notify.CDS_Rule_Master_Name
                                       select notify).ToList<Notification>();
                }


                //    ClientSession.Notification = lstNotification;
                //}
                //else
                //{
                //    lstNotification = ClientSession.Notification;
                //}
                IList<UserLookup> lstUserLookup = new List<UserLookup>();
                if (ClientSession.NotificationUserLookup != null && ClientSession.NotificationUserLookup.Count == 0 || ScreenName.Trim().ToUpper() == "ALL")
                {
                    UserLookupManager objUserLookupManager = new UserLookupManager();
                    lstUserLookup = objUserLookupManager.GetFieldLookupList(ClientSession.UserName.Trim().ToUpper(), "MANAGE CCDS");
                    ClientSession.NotificationUserLookup = lstUserLookup;
                }
                else
                {
                    lstUserLookup = ClientSession.NotificationUserLookup;
                }

                //Bug id 48027
                //string sPhysicianXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\PhysicianAddressDetails.xml";
                //XmlDocument itemPhysiciandoc = new XmlDocument();
                //XmlTextReader XmlPhysicianText = new XmlTextReader(sPhysicianXmlPath);
                //itemPhysiciandoc.Load(XmlPhysicianText);
                string sPhysicianSpecialty = string.Empty;

                //XmlNodeList xmlphy = itemPhysiciandoc.GetElementsByTagName("p" + ClientSession.FillEncounterandWFObject.EncRecord.Encounter_Provider_ID.ToString());
                //if (xmlphy.Count > 0)
                //{
                //    sPhysicianSpecialty = xmlphy[0].Attributes[7].Value;
                //}
                //CAP-2780
                PhysicianAddressDetailsList physicianAddressDetailsList = ConfigureBase<PhysicianAddressDetailsList>.ReadJson("PhysicianAddressDetails.json");
                if (physicianAddressDetailsList != null)
                {
                    var matchingAddress = physicianAddressDetailsList.PhysicianAddress
                                                     .FirstOrDefault(address => address.Physician_Library_ID == ClientSession.FillEncounterandWFObject.EncRecord.Encounter_Provider_ID.ToString());

                    if (matchingAddress != null)
                    {
                        sPhysicianSpecialty = matchingAddress?.Specialties ?? string.Empty;
                    }
                }
                if (ScreenName.Trim().ToUpper() == "NOTIFY")
                {
                    if (lstNotification.Count > 0)
                    {
                        foreach (Notification item in lstNotification)
                        {
                            if (item.Status.Trim().ToUpper() == "ACTIVE")
                            {
                                IList<UserLookup> lstLookupList = lstUserLookup.Where(a => a.Value.Trim().ToUpper() == item.CDS_Rule_Master_Name.Trim().ToUpper()).ToList<UserLookup>();
                                if (lstLookupList != null && lstLookupList.Count > 0)
                                {
                                    //IList<CDSRuleMaster> lst = lstCDSRules.Where(a => a.Clinincal_Decision_Name.Trim().ToUpper() == item.CDS_Rule_Master_Name.Trim().ToUpper() && a.Role_Privilege.ToUpper().Contains(ClientSession.UserRole.ToUpper())).ToList<CDSRuleMaster>();
                                    IList<CDSRuleMaster> lst = lstCDSRules.Where(a => a.Clinincal_Decision_Name.Trim().ToUpper() == item.CDS_Rule_Master_Name.Trim().ToUpper() && a.Role_Privilege.ToUpper().Contains(ClientSession.UserCurrentProcess.ToUpper())).ToList<CDSRuleMaster>();

                                    if (lst != null && lst.Count > 0)
                                    {
                                        if (lst[0].Role_Privilege.Contains(','))
                                        {
                                            //Bug id 48027

                                            string[] sArry = lst[0].Role_Privilege.Split(',');

                                            foreach (string sRole in sArry)
                                            {
                                                if (sRole.ToUpper() == ClientSession.UserCurrentProcess.ToUpper())
                                                {
                                                    if (lst[0].Physician_Specialty.Trim() != string.Empty)
                                                    {
                                                        string[] arySpeciality = sPhysicianSpecialty.Split(',');
                                                        var ary = lst[0].Physician_Specialty.Split(',').Intersect(arySpeciality);
                                                        Boolean bCommon = false;

                                                        foreach (string s in ary)
                                                        {
                                                            bCommon = true;
                                                        }
                                                        if (bCommon == true)
                                                        {
                                                            notification = FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        notification = FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            notification = FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                        }
                                    }
                                }
                                else
                                {

                                    //IList<CDSRuleMaster> lst = lstCDSRules.Where(a => a.Clinincal_Decision_Name.Trim().ToUpper() == item.CDS_Rule_Master_Name.Trim().ToUpper()&& a.Is_Manage_CDS_Allowed == "N"  && a.Role_Privilege.ToUpper().Contains(ClientSession.UserRole.ToUpper())).ToList<CDSRuleMaster>();
                                    IList<CDSRuleMaster> lst = lstCDSRules.Where(a => a.Clinincal_Decision_Name.Trim().ToUpper() == item.CDS_Rule_Master_Name.Trim().ToUpper() && a.Is_Manage_CDS_Allowed == "N" && a.Role_Privilege.ToUpper().Contains(ClientSession.UserCurrentProcess.ToUpper())).ToList<CDSRuleMaster>();

                                    if (lst != null && lst.Count > 0)
                                    {
                                        if (lst[0].Role_Privilege.Contains(','))
                                        {
                                            string[] sArry = lst[0].Role_Privilege.Split(',');
                                            foreach (string sRole in sArry)
                                            {
                                                //  if (sRole.ToUpper() == ClientSession.UserRole.ToUpper())
                                                if (sRole.ToUpper() == ClientSession.UserCurrentProcess.ToUpper())
                                                {
                                                    if (lst[0].Physician_Specialty.Trim() != string.Empty)
                                                    {
                                                        string[] arySpeciality = sPhysicianSpecialty.Split(',');
                                                        var ary = lst[0].Physician_Specialty.Split(',').Intersect(arySpeciality);
                                                        Boolean bCommon = false;

                                                        foreach (string s in ary)
                                                        {
                                                            bCommon = true;
                                                        }
                                                        if (bCommon == true)
                                                        {
                                                            notification = FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        notification = FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            notification = FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    notification = notification.Replace("\r\n", "$").Replace("^", "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;").Replace("$", "<br/>").Replace("<[", "<strong>").Replace("[#", "<strong>").Replace("[", "<br/><strong>").Replace("]", "</strong> ").Replace("###", "<a href=").Replace("!", "</a>").Replace("Quotes", "\"") + "~~~" + NotificationCount.ToString() + "~*~" + NotificationSummaryText;
                    //ClientSession.Notification = null;
                }
                else
                {
                    if (ClientSession.HumanId != 0 && ClientSession.UserName != null && ClientSession.CDSNotificationRule != null)
                    {
                        //For Exception provider  //Bug ID 51397 removed temporary...
                        //string sEncounteProviderName = string.Empty;
                        //if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.Fill_Encounter_and_WFObject != null && ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord.Encounter_Provider_ID != null)
                        //{
                        //    int iEncProviderID = ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord.Encounter_Provider_ID;
                        //    string xmlFilepathUser = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
                        //    if (File.Exists(xmlFilepathUser))
                        //    {
                        //        XmlDocument xdoc = new XmlDocument();
                        //        XmlTextReader itext = new XmlTextReader(xmlFilepathUser);
                        //        xdoc.Load(itext);
                        //        itext.Close();
                        //        XmlNodeList xnodelst = xdoc.GetElementsByTagName("User");
                        //        if (xnodelst != null && xnodelst.Count > 0)
                        //        {
                        //            foreach (XmlNode xnode in xnodelst)
                        //            {
                        //                if (xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() != "0" && xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() == iEncProviderID.ToString())
                        //                {
                        //                    sEncounteProviderName = xnode.Attributes.GetNamedItem("User_Name").Value;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}

                        notification = objManager.GetNotification(ClientSession.EncounterId, ClientSession.HumanId, ClientSession.UserName, ScreenName, lstCDSRules, lstNotification, lstUserLookup, dtcurrenttime, ClientSession.UserCurrentProcess, sPhysicianSpecialty);//, sEncounteProviderName);
                        notification = notification.Replace("\r\n", "$").Replace("^", "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;").Replace("$", "<br/>").Replace("<[", "<strong>").Replace("[#", "<strong>").Replace("[", "<br/><strong>").Replace("]", "</strong> ").Replace("###", "<a href=").Replace("!", "</a>").Replace("Quotes", "\"");
                        // ClientSession.Notification = null;
                    }
                }
                //Commented For Bug ID 56265 , 56264 
                //BugID:47780
                //if (notification != null && notification.Trim() != String.Empty && notification.Contains("~~~"))
                //{
                //    ClientSession.NotificationCount = notification.Split(new[] { "~~~" }, StringSplitOptions.None)[1].Split(new[] { "~*~" }, StringSplitOptions.None)[0];
                //    if (notification.ToUpper().IndexOf("MANDATORY") > -1)
                //    {
                //        ClientSession.bIsMandatoryNotifPresent = true;
                //    }
                //    else
                //    {
                //        ClientSession.bIsMandatoryNotifPresent = false;
                //    }
                //}
            }
            return notification;

        }
        //this method added for Patient trend analysis by naveena 22.8.2017
        [WebMethod(EnableSession = true)]
        public static string LoadFlowSheetData(string FieldName, string PeriodValue)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<PatientResults> VitalsList = new List<PatientResults>();
            string sFlowSheetValue = string.Empty;

            IList<string> ilstPatientSummaryBarTagEncounterList = new List<string>();
            ilstPatientSummaryBarTagEncounterList.Add("PatientResultsList");

            IList<object> ilstEncBlobFinal = new List<object>();
            ilstEncBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstPatientSummaryBarTagEncounterList);

            if (ilstEncBlobFinal != null && ilstEncBlobFinal.Count > 0)
            {
                if (ilstEncBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstEncBlobFinal[0]).Count; iCount++)
                    {
                        VitalsList.Add((PatientResults)((IList<object>)ilstEncBlobFinal[0])[iCount]);
                    }
                }
            }

            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    //  itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();

            //        if (itemDoc.GetElementsByTagName("PatientResultsList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("PatientResultsList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(PatientResults));
            //                    PatientResults lstVitals = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as PatientResults;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((PatientResults)lstVitals).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (property.Name == nodevalue.Name)
            //                                {
            //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                        property.SetValue(lstVitals, Convert.ToUInt64(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                        property.SetValue(lstVitals, Convert.ToString(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                        property.SetValue(lstVitals, Convert.ToDateTime(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                        property.SetValue(lstVitals, Convert.ToInt32(nodevalue.Value), null);
            //                                    else
            //                                        property.SetValue(lstVitals, nodevalue.Value, null);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    VitalsList.Add(lstVitals);
            //                }
            //            }
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}
            /*
            int iXAxisCount = 0;

            string sStartViewPoint = string.Empty;
            string sEndViewPoint = string.Empty;
            string sPlotPoints = string.Empty;
            int sYminValue = 0;
            int sYmaxValue = 0;
            if (VitalsList.Count > 0)
            {
                IList<PatientResults> lstVitalResults = VitalsList.Where(x => x.Loinc_Observation == FieldName).ToList<PatientResults>();
                IList<PatientResults> lstResults = lstVitalResults.OrderBy(x => x.Captured_date_and_time).ToList<PatientResults>();
                if (lstResults != null && lstResults.Count > 0)
                {
                    //lstResults = lstResults.OrderByDescending(x => x.Captured_date_and_time).ToList<PatientResults>();//For Overall value
                    if (PeriodValue == "18 Months")
                    {
                        DateTime dtMonths = DateTime.Now.AddMonths(-18);
                        lstResults = lstResults.Where(x => x.Captured_date_and_time <= UtilityManager.ConvertToUniversal(DateTime.Now) && x.Captured_date_and_time >= UtilityManager.ConvertToUniversal(dtMonths)).ToList<PatientResults>();
                    }
                    else if (PeriodValue == "5 Years")
                    {
                        DateTime dtYear = DateTime.Now.AddYears(-5);
                        lstResults = lstResults.Where(x => x.Captured_date_and_time <= UtilityManager.ConvertToUniversal(DateTime.Now) && x.Captured_date_and_time >= UtilityManager.ConvertToUniversal(dtYear)).ToList<PatientResults>();
                    }
                    if (lstResults[0].Value != string.Empty)
                    {
                        sYminValue = Convert.ToInt32(lstResults[0].Value);
                        sYmaxValue = Convert.ToInt32(lstResults[0].Value);
                    }

                    foreach (var vValue in lstResults)
                    {
                        if (sFlowSheetValue == string.Empty)
                        {
                            sFlowSheetValue = UtilityManager.ConvertToLocal(vValue.Captured_date_and_time).ToString("dd/MMM/yy") + "^" + vValue.Value;
                            iXAxisCount++;
                        }
                        else
                        {
                            sFlowSheetValue += "~" + UtilityManager.ConvertToLocal(vValue.Captured_date_and_time).ToString("dd/MMM/yy") + "^" + vValue.Value;
                            iXAxisCount++;
                        }
                        if (vValue.Value != string.Empty)
                        {
                            if (Convert.ToInt32(vValue.Value) < sYminValue)
                                sYminValue = Convert.ToInt32(vValue.Value);
                            if (Convert.ToInt32(vValue.Value) > sYmaxValue)
                                sYmaxValue = Convert.ToInt32(vValue.Value);
                        }
                    }
                    if (iXAxisCount > 0)
                        iXAxisCount = iXAxisCount + 3; //to enhance the grid lines view 

                    //Calculate End Plot point before ascending the captured datetime
                    if (lstResults.Count > 0)
                    {
                        DateTime EndViewPoint = UtilityManager.ConvertToUniversal(lstResults[0].Captured_date_and_time.AddMonths(3));//X axis ending Plot point
                        sEndViewPoint = EndViewPoint.ToString("dd/MMM/yy");
                        DateTime StartViewPoint = UtilityManager.ConvertToUniversal(lstResults[0].Captured_date_and_time.AddMonths(-3));//X axis starting Plot point
                        sStartViewPoint = StartViewPoint.ToString("dd/MMM/yy");

                        if (lstResults.Count > 1)
                        {
                            lstResults = lstResults.OrderBy(x => x.Captured_date_and_time).ToList<PatientResults>();
                            StartViewPoint = lstResults[0].Captured_date_and_time.AddMonths(-3);
                            sStartViewPoint = StartViewPoint.ToString("dd/MMM/yy");
                        }
                    }

                }
                sYminValue = 5 * (int)Math.Round(sYminValue / 5.0) - 5;
                sYmaxValue = 5 * (int)Math.Round(sYmaxValue / 5.0) + 5;
                int interval = 5 * (int)Math.Round(((sYmaxValue - sYminValue) / 10.0) / 5.0);
                int[] xRange=new int[10];
                xRange[0] = sYminValue;
                for (int i = 1; i < 11; i++)
                {
                    xRange[i] = xRange[i - 1] + interval;
                }
                sPlotPoints = sStartViewPoint + "&" + sEndViewPoint + "|" + sYminValue + "&" + sYmaxValue;
            }
            IList<VitalsRanges> VitalsRangesList = new List<VitalsRanges>();
            string sLookupXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
            if (File.Exists(sLookupXmlFilePath) == true)
            {
                XmlDocument itemDoc = new XmlDocument();
                XmlTextReader XmlText = new XmlTextReader(sLookupXmlFilePath);
                itemDoc.Load(XmlText);
                XmlText.Close();
                XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("FlowSheetList");

                if (xmlNodeList != null && xmlNodeList.Count > 0)
                {
                    for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                    {
                        VitalsRanges vitRange = new VitalsRanges();
                        vitRange.vitalname = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Field_Name").Value.ToString();
                        vitRange.range = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("intervals").Value.ToString();
                        vitRange.format = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("displayformat").Value.ToString();
                        VitalsRangesList.Add(vitRange);
                    }
                }
            }
            var VitalRanges = VitalsRangesList.Select(a => new
            {
                Field_name = a.vitalname,
                Range = a.range,
                Format = a.format
            });
            string json = JsonConvert.SerializeObject(VitalRanges);
            string flowSheetDetails = JsonConvert.SerializeObject(sFlowSheetValue + "$" + iXAxisCount.ToString() + "*" + sPlotPoints);
            var Result = new
            {
                VitalsRange = json,
                FlowSheetData = flowSheetDetails,
            };
            return JsonConvert.SerializeObject(Result);
           */
            double sYminValue = 0;
            double sYmaxValue = 0;
            string sXminValue = string.Empty;
            string sXmaxValue = string.Empty;
            string xDisplayFormat = string.Empty;
            string xDisplayText = string.Empty;
            string yDisplayFormat = string.Empty;
            string yDisplayText = string.Empty;
            int ValueSets = 0;
            double[] yRange = new double[11];
            string[] xRange = { };
            int plotPointsCount = 0;

            if (VitalsList.Count > 0)
            {
                IList<PatientResults> lstVitalResults = VitalsList.Where(x => x.Loinc_Observation == FieldName && x.Value != "").ToList<PatientResults>();
                IList<PatientResults> lstResults = lstVitalResults.OrderBy(x => x.Captured_date_and_time).ToList<PatientResults>();
                if (lstResults != null && lstResults.Count > 0)
                {
                    //lstResults = lstResults.OrderByDescending(x => x.Captured_date_and_time).ToList<PatientResults>();//For Overall value
                    if (PeriodValue == "18 Months")
                    {
                        DateTime dtMonths = DateTime.Now.AddMonths(-18);
                        lstResults = lstResults.Where(x => x.Captured_date_and_time <= UtilityManager.ConvertToUniversal(DateTime.Now) && x.Captured_date_and_time >= UtilityManager.ConvertToUniversal(dtMonths)).ToList<PatientResults>();
                    }
                    else if (PeriodValue == "5 Years")
                    {
                        DateTime dtYear = DateTime.Now.AddYears(-5);
                        lstResults = lstResults.Where(x => x.Captured_date_and_time <= UtilityManager.ConvertToUniversal(DateTime.Now) && x.Captured_date_and_time >= UtilityManager.ConvertToUniversal(dtYear)).ToList<PatientResults>();
                    }
                    if (lstResults != null && lstResults.Count > 0)
                    {
                        if (lstResults[0].Value != string.Empty)
                        {
                            if (lstResults[0].Value.IndexOf('/') != -1)
                            {
                                sYminValue = Convert.ToDouble(lstResults[0].Value.Split('/')[1]);
                                sYmaxValue = Convert.ToDouble(lstResults[0].Value.Split('/')[0]);
                            }
                            else
                            {
                                sYminValue = Convert.ToDouble(lstResults[0].Value);
                                sYmaxValue = Convert.ToDouble(lstResults[0].Value);
                            }
                        }

                        xRange = new string[lstResults.Count];


                        foreach (var vValue in lstResults)
                        {
                            if (sFlowSheetValue == string.Empty)
                            {
                                sFlowSheetValue = UtilityManager.ConvertToLocal(vValue.Captured_date_and_time).ToString("dd/MMM/yy") + "^" + vValue.Value;
                                xRange[plotPointsCount] = UtilityManager.ConvertToLocal(vValue.Captured_date_and_time).ToString("dd/MMM/yy");
                                plotPointsCount++;
                            }
                            else
                            {
                                sFlowSheetValue += "~" + UtilityManager.ConvertToLocal(vValue.Captured_date_and_time).ToString("dd/MMM/yy") + "^" + vValue.Value;
                                xRange[plotPointsCount] = UtilityManager.ConvertToLocal(vValue.Captured_date_and_time).ToString("dd/MMM/yy");
                                plotPointsCount++;
                            }
                            if (vValue.Value != string.Empty)
                            {
                                if (vValue.Value.IndexOf('/') != -1)
                                {
                                    if (Convert.ToDouble(vValue.Value.Split('/')[1]) < sYminValue)
                                        sYminValue = Convert.ToDouble(vValue.Value.Split('/')[1]);
                                    if (Convert.ToDouble(vValue.Value.Split('/')[0]) > sYmaxValue)
                                        sYmaxValue = Convert.ToDouble(vValue.Value.Split('/')[0]);
                                }
                                else
                                {
                                    if (Convert.ToDouble(vValue.Value) < sYminValue)
                                        sYminValue = Convert.ToDouble(vValue.Value);
                                    if (Convert.ToDouble(vValue.Value) > sYmaxValue)
                                        sYmaxValue = Convert.ToDouble(vValue.Value);
                                }
                            }
                        }

                    }
                    double MultiplyValue = 5.0;
                    if (FieldName == "HbA1C")
                        MultiplyValue = 0.5;
                    sYminValue = (MultiplyValue * (int)Math.Round(sYminValue / MultiplyValue)) - MultiplyValue;
                    sYmaxValue = (MultiplyValue * (int)Math.Round(sYmaxValue / MultiplyValue)) + MultiplyValue;
                    double diff = (sYmaxValue - sYminValue) / 8.0;
                    double interval = 0.0;
                    if (diff > 1.0)
                    {
                        interval = Math.Ceiling(diff);
                        double x = diff - Math.Truncate(diff);
                        if (x >= 0.5)
                            interval = interval + 1;
                    }
                    else
                    {
                        interval = Math.Round(diff, 1);
                    }

                    yRange[0] = sYminValue;
                    for (int i = 1; i < 11; i++)
                    {
                        yRange[i] = yRange[i - 1] + interval;
                    }
                    if (lstResults.Count > 0)
                    {
                        DateTime StartViewPoint = UtilityManager.ConvertToLocal(lstResults[0].Captured_date_and_time);
                        sXminValue = StartViewPoint.ToString("dd/MMM/yy");
                        DateTime EndViewPoint = UtilityManager.ConvertToLocal(lstResults[lstResults.Count - 1].Captured_date_and_time);
                        sXmaxValue = EndViewPoint.ToString("dd/MMM/yy");
                    }
                }
            }
            IList<VitalsRanges> VitalsRangesList = new List<VitalsRanges>();
            //string sLookupXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
            //if (File.Exists(sLookupXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(sLookupXmlFilePath);
            //    itemDoc.Load(XmlText);
            //    XmlText.Close();
            //    XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("FlowSheetList");

            //    if (xmlNodeList != null && xmlNodeList.Count > 0)
            //    {
            //        for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
            //        {
            //            if (FieldName.ToUpper() == xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Field_Name").Value.ToUpper().ToString())
            //            {
            //                XmlNode xNode = xmlNodeList[0].ChildNodes[j];
            //                xDisplayFormat = xNode.Attributes.GetNamedItem("xDisplayFormat").Value;
            //                xDisplayText = xNode.Attributes.GetNamedItem("xDisplayText").Value;
            //                yDisplayFormat = xNode.Attributes.GetNamedItem("Ydisplayformat").Value;
            //                yDisplayText = xNode.Attributes.GetNamedItem("YDisplayText").Value;
            //                ValueSets = Convert.ToInt32(xNode.Attributes.GetNamedItem("ValueSets").Value);
            //            }
            //        }
            //    }
            //}
            //CAP-2787
            StaticLookupList staticLookupList = ConfigureBase<StaticLookupList>.ReadJson("staticlookup.json");
            var flowSheetList = staticLookupList.FlowSheetList.ToList();
            if (flowSheetList != null && flowSheetList.Count() > 0)
            {
                foreach (var flowSheet in flowSheetList)
                {
                    if (FieldName.ToUpper() == flowSheet.Field_Name.ToUpper().ToString())
                    {
                        xDisplayFormat = flowSheet.xDisplayFormat;
                        xDisplayText = flowSheet.xDisplayText;
                        yDisplayFormat = flowSheet.Ydisplayformat;
                        yDisplayText = flowSheet.YDisplayText;
                        ValueSets = Convert.ToInt32(flowSheet.valueSets);
                    }
                }
            }
            //string sLookupXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
            //if (File.Exists(sLookupXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(sLookupXmlFilePath);
            //    itemDoc.Load(XmlText);
            //    XmlText.Close();
            //    XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("FlowSheetList");

            //    if (xmlNodeList != null && xmlNodeList.Count > 0)
            //    {
            //        for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
            //        {
            //            if (FieldName.ToUpper() == xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Field_Name").Value.ToUpper().ToString())
            //            {
            //                XmlNode xNode = xmlNodeList[0].ChildNodes[j];
            //                xDisplayFormat = xNode.Attributes.GetNamedItem("xDisplayFormat").Value;
            //                xDisplayText = xNode.Attributes.GetNamedItem("xDisplayText").Value;
            //                yDisplayFormat = xNode.Attributes.GetNamedItem("Ydisplayformat").Value;
            //                yDisplayText = xNode.Attributes.GetNamedItem("YDisplayText").Value;
            //                ValueSets = Convert.ToInt32(xNode.Attributes.GetNamedItem("ValueSets").Value);
            //            }
            //        }
            //    }
            //}
            //CAP-2787
            staticLookupList = ConfigureBase<StaticLookupList>.ReadJson("staticlookup.json");
            if (staticLookupList != null)
            {
                flowSheetList = staticLookupList.FlowSheetList.ToList();

                if (flowSheetList != null)
                {
                    foreach (var flowSheet in flowSheetList)
                    {
                        if (FieldName.ToUpper() == flowSheet.Field_Name.ToUpper().ToString())
                        {
                            xDisplayFormat = flowSheet.xDisplayFormat;
                            xDisplayText = flowSheet.xDisplayText;
                            yDisplayFormat = flowSheet.Ydisplayformat;
                            yDisplayText = flowSheet.YDisplayText;
                            ValueSets = Convert.ToInt32(flowSheet.valueSets);
                        }
                    }
                }
            }

            string xAxisDetails = string.Empty;
            string yAaxisDetails = string.Empty;
            string dataPoints = string.Empty;
            string Annotate = "true";

            if (plotPointsCount > 12)
                Annotate = "false";

            dataPoints = JsonConvert.SerializeObject(sFlowSheetValue);
            var xAxisDtls = new
            {
                MinValue = sXminValue,
                MaxValue = sXmaxValue,
                Intervals = xRange,
                DisplayFormat = xDisplayFormat,
                DisplayText = xDisplayText
            };

            var yAxisDtls = new
            {
                MinValue = sYminValue,
                MaxValue = sYmaxValue,
                Intervals = yRange,
                DisplayFormat = yDisplayFormat,
                DisplayText = yDisplayText
            };

            xAxisDetails = JsonConvert.SerializeObject(xAxisDtls);
            yAaxisDetails = JsonConvert.SerializeObject(yAxisDtls);

            var Result = new
            {
                PlotPoints = dataPoints,
                DisplaySets = ValueSets,
                XAxis = xAxisDetails,
                YAxis = yAaxisDetails,
                IsAnnotationReq = Annotate
            };
            return JsonConvert.SerializeObject(Result);
        }
        //Added for Provider_Review PhysicianAssistant WorkFlow Change
        [WebMethod(EnableSession = true)]
        public static string GetBirtReportDetails()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sProjectType = ClientSession.LegalOrg;
            //System.Configuration.ConfigurationManager.AppSettings["ProjectType"].ToString();
            //string sBIRTReportUrl = System.Configuration.ConfigurationManager.AppSettings["BIRTReportUrl"].ToString() + "CAPELLA_" + sProjectType;

            string sBIRTReportUrl = string.Empty;
            sBIRTReportUrl = System.Configuration.ConfigurationManager.AppSettings["BIRTReportUrl_" + sProjectType].ToString() + "CAPELLA_" + sProjectType;

            NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration().Configure();
            //string[] conString = cfg.GetProperty(NHibernate.Cfg.Environment.ConnectionString).ToString().Split(';');
            string[] conString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ToString().Split(';');
            string sDataBase = string.Empty;
            string sDataSource = string.Empty;
            string sUserId = string.Empty;
            string sPassword = string.Empty;
            string sPort = "3306";
            for (int i = 0; i < conString.Length; i++)
            {
                if (conString[i].ToString().ToUpper().Contains("DATABASE=") == true)
                {
                    sDataBase = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("DATA SOURCE") == true)
                {
                    sDataSource = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("USER ID") == true)
                {
                    sUserId = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("PASSWORD") == true)
                {
                    sPassword = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("PORT") == true)
                {
                    sPort = conString[i].ToString().Split('=')[1];
                }
            }
            //string sodaURL = "jdbc:mysql://" + sDataSource + ":" + sPort + "/" + sDataBase;
            string sodaURL = string.Empty;
            string sAzure = System.Configuration.ConfigurationManager.AppSettings["Azure"].ToString();
            if (sAzure == "Y")
                sodaURL = "jdbc:mysql://" + sDataSource + ":" + sPort + "/" + sDataBase + "?useSSL=true&requireSSL=false";
            else
                sodaURL = "jdbc:mysql://" + sDataSource + ":" + sPort + "/" + sDataBase;

            string sodaUser = sUserId;
            string sodaPassword = sPassword;
            string sDBConnection = "&odaURL=" + sodaURL + "&odaUser=" + sodaUser + "&odaPassword=" + sodaPassword + "&legal_org=" + ClientSession.LegalOrg;
            var result = new { BIRTUrl = sBIRTReportUrl, DBConnection = sDBConnection, UserRole = ClientSession.UserRole };
            return JsonConvert.SerializeObject(result);
        }

        [WebMethod(EnableSession = true)]
        public static string GetProtocols(string UserID, string UserRole)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            //Jira CAP-2782
            //string sPhysicianXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\PhysicianReviewProtocols.xml";
            //XElement root = XElement.Load(sPhysicianXmlPath);
            //IList<ReviewProtocols> lstProtocols = new List<ReviewProtocols>();
            //if (UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
            //{
            //    IEnumerable<XElement> xPhyAsstList =
            //         from el in root.Elements("PhyAsstSpecificProtocol")
            //         where el.Attribute("id").Value.Equals(UserID)
            //         select el;
            //    foreach (XElement elPhyAsst in xPhyAsstList)
            //    {
            //        IEnumerable<XElement> xPhyList =
            //         from el in xPhyAsstList.Elements("PhysicianList").Elements("Physician")
            //         orderby el.Attribute("id").Value
            //         select el;
            //        foreach (XElement elPhy in xPhyList)
            //        {
            //            ReviewProtocols objProtocol = new ReviewProtocols();
            //            objProtocol.UserID = elPhy.Attribute("id").Value.ToString();
            //            objProtocol.UserName = elPhy.Attribute("value").Value.ToString();
            //            objProtocol.Protocol = elPhy.Attribute("rule").Value.ToString();
            //            lstProtocols.Add(objProtocol);
            //        }
            //    }
            //}
            //else if (UserRole.ToUpper() == "PHYSICIAN")
            //{
            //    IEnumerable<XElement> xPhyList =
            //        from el in root.Elements("PhyAsstSpecificProtocol").Elements("PhysicianList").Elements("Physician")
            //        where el.Attribute("id").Value.Equals(UserID)
            //        orderby el.Attribute("id").Value
            //        select el;
            //    xPhyList = xPhyList.GroupBy(a => a.Attribute("rule").Value).Select(a => a.First()).ToArray();
            //    foreach (XElement elPhy in xPhyList)
            //    {
            //        ReviewProtocols objProtocol = new ReviewProtocols();
            //        objProtocol.UserID = elPhy.Attribute("id").Value.ToString();
            //        objProtocol.UserName = elPhy.Attribute("value").Value.ToString();
            //        objProtocol.Protocol = elPhy.Attribute("rule").Value.ToString();
            //        lstProtocols.Add(objProtocol);
            //    }
            //}

            //Jira CAP-2782
            PhyAsstSpecificProtocolsList phyAsstSpecificProtocolList = new PhyAsstSpecificProtocolsList();
            phyAsstSpecificProtocolList = ConfigureBase<PhyAsstSpecificProtocolsList>.ReadJson("PhysicianReviewProtocols.json");
            List<PhyAsstSpecificProtocols> ilstPhysAsslist = new List<PhyAsstSpecificProtocols>();
            IList<ReviewProtocols> lstProtocols = new List<ReviewProtocols>();
            if (ilstPhysAsslist != null)
            {
                if (UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                {
                    ilstPhysAsslist = phyAsstSpecificProtocolList.PhyAsstSpecificProtocols.Where(x => x.Physician_Assiatant_id == UserID).ToList();
                    if ((ilstPhysAsslist?.Count ?? 0) > 0)
                    {
                        foreach (PhyAsstSpecificProtocols phyass in ilstPhysAsslist)
                        {
                            foreach (var item in phyass.Physician.OrderBy(a => a.id).ToList())
                            {
                                ReviewProtocols objProtocol = new ReviewProtocols();
                                objProtocol.UserID = item.id;
                                objProtocol.UserName = item.value;
                                objProtocol.Protocol = item.rule;
                                lstProtocols.Add(objProtocol);
                            }
                        }
                    }
                }
                else if (UserRole.ToUpper() == "PHYSICIAN")
                {
                    ilstPhysAsslist = phyAsstSpecificProtocolList.PhyAsstSpecificProtocols.Where(x => x.Physician.Any(a=>a.id == UserID)).ToList();
                    List<Physician> physicians = new List<Physician>(); 
                    if ((ilstPhysAsslist?.Count ?? 0) > 0)
                    {
                        foreach (PhyAsstSpecificProtocols phyass in ilstPhysAsslist)
                        {
                            physicians.AddRange(phyass.Physician);
                        }
                        foreach (var item in physicians.GroupBy(a => a.rule).Select(x => x.OrderBy(a => a.id).FirstOrDefault()).ToList())
                        {
                            ReviewProtocols objProtocol = new ReviewProtocols();
                            objProtocol.UserID = item.id;
                            objProtocol.UserName = item.value;
                            objProtocol.Protocol = item.rule;
                            lstProtocols.Add(objProtocol);
                        }
                    }
                }
            }


            var returnList = JsonConvert.SerializeObject(lstProtocols);
            return returnList;
        }


        [WebMethod(EnableSession = true)]
        public static string GetRAFScore(string human_id)
        {

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            //Jira CAP-1183
            //string RAFxmlPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["RAF_XMLPath"].ToString(), System.Configuration.ConfigurationManager.AppSettings["ProjectName"].ToString());
            string RAFxmlPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["RAF_XMLPath"].ToString(), ClientSession.LegalOrg.ToString());
            string FileName = "RAF" + "_" + ClientSession.HumanId + ".xml";
            string strXmlFilePath = Path.Combine(RAFxmlPath, FileName);
            string RAFscore = "";
            string HPNRAFScore = "";

            if (File.Exists(strXmlFilePath) == true)
            {
                XmlDocument itemDoc = new XmlDocument();
                XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);


                itemDoc.Load(XmlText);

                XmlNodeList nodescur = itemDoc.SelectNodes("//RafScores[@Year='" + System.DateTime.Now.Year.ToString() + "']");
                XmlNodeList nodesprev = itemDoc.SelectNodes("//RafScores[@Year='" + (System.DateTime.Now.Year - 1).ToString() + "']");
                XmlNodeList nodesprev1 = itemDoc.SelectNodes("//RafScores[@Year='" + (System.DateTime.Now.Year - 2).ToString() + "']");


                XmlText.Close();

                if (nodescur.Count > 0)
                {

                    if (RAFscore == "")
                    {
                        if (nodescur[0].Attributes["Normalized_RAF_Coding_Intensity"].Value.ToString() != "")
                            RAFscore = nodescur[0].Attributes["Year"].Value.ToString() + " : " + nodescur[0].Attributes["Normalized_RAF_Coding_Intensity"].Value.ToString();
                        else
                            RAFscore = nodescur[0].Attributes["Year"].Value.ToString() + " : " + "NA";
                    }
                    else
                    {
                        if (nodescur[0].Attributes["Normalized_RAF_Coding_Intensity"].Value.ToString() != "")
                            RAFscore = RAFscore + "|" + nodescur[0].Attributes["Year"].Value.ToString() + " : " + nodescur[0].Attributes["Normalized_RAF_Coding_Intensity"].Value.ToString();
                        else
                            RAFscore = RAFscore + "|" + nodescur[0].Attributes["Year"].Value.ToString() + " : " + "NA";

                    }
                    if (HPNRAFScore == "")
                    {
                        if (nodescur[0].Attributes["HPN_RAF"].Value.ToString() != "")
                            HPNRAFScore = "HPN " + nodescur[0].Attributes["Year"].Value.ToString() + " : " + nodescur[0].Attributes["HPN_RAF"].Value.ToString();
                        else
                            HPNRAFScore = "HPN " + nodescur[0].Attributes["Year"].Value.ToString() + " : " + "NA";

                    }
                    else
                    {
                        if (nodescur[0].Attributes["HPN_RAF"].Value.ToString() != "")
                            HPNRAFScore = HPNRAFScore + "|" + "HPN " + nodescur[0].Attributes["Year"].Value.ToString() + " : " + nodescur[0].Attributes["HPN_RAF"].Value.ToString();
                        else
                            HPNRAFScore = HPNRAFScore + "|" + "HPN " + nodescur[0].Attributes["Year"].Value.ToString() + " : " + "NA";

                    }
                }
                else
                {

                    if (RAFscore == "")
                    {

                        RAFscore = (System.DateTime.Now.Year).ToString() + " : " + "NA";
                    }
                    else
                    {

                        RAFscore = RAFscore + "|" + (System.DateTime.Now.Year).ToString() + " : " + "NA";

                    }
                    if (HPNRAFScore == "")
                    {

                        HPNRAFScore = "HPN " + (System.DateTime.Now.Year).ToString() + " : " + "NA";

                    }
                    else
                    {

                        HPNRAFScore = HPNRAFScore + "|" + "HPN " + (System.DateTime.Now.Year).ToString() + " : " + "NA";

                    }

                }
                if (nodesprev.Count > 0)
                {

                    if (RAFscore == "")
                    {
                        if (nodesprev[0].Attributes["Normalized_RAF_Coding_Intensity"].Value.ToString() != "")
                            RAFscore = nodesprev[0].Attributes["Year"].Value.ToString() + " : " + nodesprev[0].Attributes["Normalized_RAF_Coding_Intensity"].Value.ToString();
                        else
                            RAFscore = nodesprev[0].Attributes["Year"].Value.ToString() + " : " + "NA";
                    }
                    else
                    {
                        if (nodesprev[0].Attributes["Normalized_RAF_Coding_Intensity"].Value.ToString() != "")
                            RAFscore = RAFscore + "|" + nodesprev[0].Attributes["Year"].Value.ToString() + " : " + nodesprev[0].Attributes["Normalized_RAF_Coding_Intensity"].Value.ToString();
                        else
                            RAFscore = RAFscore + "|" + nodesprev[0].Attributes["Year"].Value.ToString() + " : " + "NA";

                    }
                    if (HPNRAFScore == "")
                    {
                        if (nodesprev[0].Attributes["HPN_RAF"].Value.ToString() != "")
                            HPNRAFScore = "HPN " + nodesprev[0].Attributes["Year"].Value.ToString() + " : " + nodesprev[0].Attributes["HPN_RAF"].Value.ToString();
                        else
                            HPNRAFScore = "HPN " + nodesprev[0].Attributes["Year"].Value.ToString() + " : " + "NA";

                    }
                    else
                    {
                        if (nodesprev[0].Attributes["HPN_RAF"].Value.ToString() != "")
                            HPNRAFScore = HPNRAFScore + "|" + "HPN " + nodesprev[0].Attributes["Year"].Value.ToString() + " : " + nodesprev[0].Attributes["HPN_RAF"].Value.ToString();
                        else
                            HPNRAFScore = HPNRAFScore + "|" + "HPN " + nodesprev[0].Attributes["Year"].Value.ToString() + " : " + "NA";

                    }
                }
                else
                {

                    if (RAFscore == "")
                    {

                        RAFscore = (System.DateTime.Now.Year - 1).ToString() + " : " + "NA";
                    }
                    else
                    {

                        RAFscore = RAFscore + "|" + (System.DateTime.Now.Year - 1).ToString() + " : " + "NA";

                    }
                    if (HPNRAFScore == "")
                    {

                        HPNRAFScore = "HPN " + (System.DateTime.Now.Year - 1).ToString() + " : " + "NA";

                    }
                    else
                    {

                        HPNRAFScore = HPNRAFScore + "|" + "HPN " + (System.DateTime.Now.Year - 1).ToString() + " : " + "NA";

                    }

                }
                if (nodesprev1.Count > 0)
                {

                    if (RAFscore == "")
                    {
                        if (nodesprev1[0].Attributes["Normalized_RAF_Coding_Intensity"].Value.ToString() != "")
                            RAFscore = nodesprev1[0].Attributes["Year"].Value.ToString() + " : " + nodesprev1[0].Attributes["Normalized_RAF_Coding_Intensity"].Value.ToString();
                        else
                            RAFscore = nodesprev1[0].Attributes["Year"].Value.ToString() + " : " + "NA";
                    }
                    else
                    {
                        if (nodesprev1[0].Attributes["Normalized_RAF_Coding_Intensity"].Value.ToString() != "")
                            RAFscore = RAFscore + "|" + nodesprev1[0].Attributes["Year"].Value.ToString() + " : " + nodesprev1[0].Attributes["Normalized_RAF_Coding_Intensity"].Value.ToString();
                        else
                            RAFscore = RAFscore + "|" + nodesprev1[0].Attributes["Year"].Value.ToString() + " : " + "NA";

                    }
                    if (HPNRAFScore == "")
                    {
                        if (nodesprev1[0].Attributes["HPN_RAF"].Value.ToString() != "")
                            HPNRAFScore = "HPN " + nodesprev1[0].Attributes["Year"].Value.ToString() + " : " + nodesprev1[0].Attributes["HPN_RAF"].Value.ToString();
                        else
                            HPNRAFScore = "HPN " + nodesprev1[0].Attributes["Year"].Value.ToString() + " : " + "NA";

                    }
                    else
                    {
                        if (nodesprev1[0].Attributes["HPN_RAF"].Value.ToString() != "")
                            HPNRAFScore = HPNRAFScore + "|" + "HPN " + nodesprev1[0].Attributes["Year"].Value.ToString() + " : " + nodesprev1[0].Attributes["HPN_RAF"].Value.ToString();
                        else
                            HPNRAFScore = HPNRAFScore + "|" + "HPN " + nodesprev1[0].Attributes["Year"].Value.ToString() + " : " + "NA";

                    }
                }
                else
                {

                    if (RAFscore == "")
                    {

                        RAFscore = (System.DateTime.Now.Year - 2).ToString() + " : " + "NA";
                    }
                    else
                    {

                        RAFscore = RAFscore + "|" + (System.DateTime.Now.Year - 2).ToString() + " : " + "NA";

                    }
                    if (HPNRAFScore == "")
                    {

                        HPNRAFScore = "HPN " + (System.DateTime.Now.Year - 2).ToString() + " : " + "NA";

                    }
                    else
                    {

                        HPNRAFScore = HPNRAFScore + "|" + "HPN " + (System.DateTime.Now.Year - 2).ToString() + " : " + "NA";

                    }

                }

            }

            var Result = new
            {
                // HPN = HPNRAFScore,
                RAF = RAFscore + "|" + HPNRAFScore

            };
            var returnList = JsonConvert.SerializeObject(Result);
            return returnList;
        }


        [WebMethod(EnableSession = true)]
        public static string ErrorLogEntry(string ErrorMessage, string ErrorLineNo, string ErrorColumnNo, string ErrorUrl, string ErrorStack)
        {
            string notification = string.Empty;
            //CAP-2379 & CAP-2389
            if (!ErrorMessage.Equals("User not permitted", StringComparison.InvariantCultureIgnoreCase))
            {
                if (ClientSession.UserName == string.Empty)
                {
                    HttpContext.Current.Response.StatusCode = 999;
                    HttpContext.Current.Response.Status = "999 Session Expired";
                    HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                    return string.Empty;
                }
            }
            //jira #CAP-30 - Old Code
            //string message = System.Environment.NewLine + System.Environment.NewLine + "------------------------------BEGINNING OF THIS SCRIPT ERROR------------------------------------" + System.Environment.NewLine + System.Environment.NewLine +
            //"MESSAGE: " + ErrorMessage + System.Environment.NewLine + 
            //"TIME: " + DateTime.Now.ToString() + " . UTC TIME: " + DateTime.UtcNow.ToString() + System.Environment.NewLine +
            //"SOURCE: " + ErrorUrl + System.Environment.NewLine +
            //"CURRENT USER: " + Convert.ToString(ClientSession.UserName) + System.Environment.NewLine +
            //"ENCOUNTER ID: " + Convert.ToString(ClientSession.EncounterId) + System.Environment.NewLine +
            //"HUMAN ID: " + Convert.ToString(ClientSession.HumanId) + System.Environment.NewLine +
            //"PHYSICIAN ID: " + Convert.ToString(ClientSession.PhysicianId) + System.Environment.NewLine +
            //"LINE NUMBER: " + ErrorLineNo + System.Environment.NewLine +
            //"COLUMN NUMBER: " + ErrorColumnNo + System.Environment.NewLine +
            //"STACKTRACE: " + ErrorStack + System.Environment.NewLine;
            //message += System.Environment.NewLine + System.Environment.NewLine + "------------------------------END OF THIS SCRIPT ERROR------------------------------------" + System.Environment.NewLine + System.Environment.NewLine;
            //log.Error(message);

            //jira #CAP-30 - New Code
            string version = "";
            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

            string[] server = version.Split('|');
            string serverno = "";
            if (server.Length > 1)
                serverno = server[1].Trim();

            //CAP-29 - It will validating blank string or data.
            //string message =
            //"MESSAGE: " + ErrorMessage + System.Environment.NewLine +
            // "SOURCE: " + ErrorUrl + System.Environment.NewLine +
            //"LINE NUMBER: " + ErrorLineNo + System.Environment.NewLine +
            //"COLUMN NUMBER: " + ErrorColumnNo + System.Environment.NewLine;
            string message =
            "MESSAGE: " + ErrorMessage + System.Environment.NewLine +
            (!string.IsNullOrWhiteSpace(ErrorUrl) ? "SOURCE: " + ErrorUrl + System.Environment.NewLine : "") +
            (!string.IsNullOrWhiteSpace(ErrorLineNo) ? "LINE NUMBER: " + ErrorLineNo + System.Environment.NewLine : "") +
            (!string.IsNullOrWhiteSpace(ErrorColumnNo) ? "COLUMN NUMBER: " + ErrorColumnNo + System.Environment.NewLine : "");

            string insertQuery = "insert into  stats_apperrorlog values(0,'" + message.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + "', '" + serverno + "','" + DateTime.Now + "','" + ClientSession.UserName + "','" + ClientSession.EncounterId + "','" + ClientSession.HumanId + "','" + ClientSession.PhysicianId + "','" + ErrorStack.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
            int iReturn = DBConnector.WriteData(insertQuery);

            string Query = "select * from stats_apperrorlog order by Id desc limit 1";
            DataSet ds = DBConnector.ReadData(Query);
            string sFriendlyErrorMsg = string.Empty;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //friendlyErrorMsg.Text = DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + "_" + ds.Tables[0].Rows[0][0].ToString() + ": " + generalErrorMsg;
                sFriendlyErrorMsg = DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + "_" + ds.Tables[0].Rows[0][0].ToString() + ": " + "A problem has occurred on this web site. Please try again. If this error continues, please contact support. <br><br>" + ErrorMessage;

            }
            var returnList = JsonConvert.SerializeObject(sFriendlyErrorMsg);
            return returnList;
        }

        [WebMethod(EnableSession = true)]
        public static string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}



