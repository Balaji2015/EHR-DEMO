using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using System.IO;
using System.Runtime.Serialization;
using System.Data;
using Telerik.Web.UI;
using System.Collections;
using System.Xml.Linq;
using System.Web.Script.Services;
using Ionic.Zip;

namespace Acurus.Capella.PatientPortal
{
    public partial class webfrmPatientPortal : System.Web.UI.Page
    {
        EncounterManager encPRoxy = new EncounterManager();
        HumanManager hnProxy = new HumanManager();
        ResultMasterManager resultProxy = new ResultMasterManager();
        Rcopia_MedicationManager rcopiaProxy = new Rcopia_MedicationManager();
        //frmWellnessNotes objWellnessNotes = new frmWellnessNotes();
        IList<FileManagementIndex> fileManageExistList1 = new List<FileManagementIndex>();
        ulong ulMyHumanID = 0;
        frmClinicalSummary objClinicalSummary = new frmClinicalSummary();
        frmSummaryOfCare objSummaryofCare = new frmSummaryOfCare();
        bool bZip = false;

        [System.Web.Services.WebMethod]
        public static string showreport(string encounter_id)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            frmClinicalSummary objClinicalSummary = new frmClinicalSummary();
            frmSummaryOfCare objSummaryofCare = new frmSummaryOfCare();
            string path = string.Empty;
            string sMyPath = string.Empty;
            string PDFPath = string.Empty;
            string XMlPath = string.Empty;
            string sFilePathPDF = string.Empty;
            var filepath = string.Empty;
            ArrayList FilePath = new ArrayList();
            //   hdnEncounterId.Value = cboEncounter.SelectedValue;
            //var xDox = new XDocument();
            System.Xml.XmlDocument XOX = new System.Xml.XmlDocument();
            //ArrayList sXMlPath = new ArrayList();
            //path = objWellnessNotes.PrintWellnessNotes(Convert.ToUInt32(cboEncounter.SelectedValue), Convert.ToUInt64(Request.QueryString["PatientID"]), true, ref sMyPath, "", false, fileManageExistList1);
            if ((encounter_id != null && encounter_id != ""))
            {
                FilePath = objClinicalSummary.PrintClinicalSummary(Convert.ToUInt32(encounter_id), ClientSession.HumanId, false, ref sMyPath, "", true, true);
                sFilePathPDF = objSummaryofCare.PrintPDF(FilePath[0].ToString(), "PatientPortal", DateTime.MinValue);
                // FilePath = objClinicalSummary.ImportCCD(XOX, FilePath[0].ToString(), false, Convert.ToUInt64(Request.QueryString["PatientID"]), true);
                //string[] Split = new string[] { Server.MapPath("") };
                //string[] FileName = path.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < FilePath.Count; i++)
                {
                    string[] Split = new string[] { System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath };
                    string[] fileName = FilePath[i].ToString().Split(Split, StringSplitOptions.RemoveEmptyEntries);

                    if (fileName[0].ToUpper().Contains(".XML"))
                    {
                        XMlPath = fileName[0].ToString();
                    }

                    if (fileName[0].ToUpper().Contains(".PDF"))
                    {
                        PDFPath = fileName[0].ToString();
                    }
                }
                if (sFilePathPDF != string.Empty)
                {


                    string[] SplitPdf = new string[] { System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath };
                    string[] FileName = sFilePathPDF.Split(SplitPdf, StringSplitOptions.RemoveEmptyEntries);

                    PDFPath = FileName[0].ToString();
                    // path = PDFPath;
                }
                //  frmWord.Attributes.Add("src", "frmPrintPDF.aspx?SI=" + PDFPath + "&Location=DYNAMIC");
                HttpContext.Current.Session["Path"] = PDFPath;
                HttpContext.Current.Session["xmlPath"] = XMlPath;
                // Session["Path"] = PDFPath;
                //  Session["xmlPath"] = XMlPath;
                IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                ActivityLog activity = new ActivityLog();
                activity.Human_ID = Convert.ToUInt64(ClientSession.HumanId);
                activity.Encounter_ID = Convert.ToUInt32(encounter_id);
                activity.Activity_Type = "View";
                activity.Role = "";

                activity.Activity_Date_And_Time = System.DateTime.UtcNow;
                ActivityLogList.Add(activity);
                ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
                // btnSend.Attributes.Add("onclick", "return SendDocument('" + PDFPath.Replace("\\", "$$").ToString() + "," + cboEncounter.SelectedValue + "," + hdnPatientName.Value + "," + lblEmailIDActual.Text + "');");
                //  btnSend.Enabled = true;
                //  btnDownload.Enabled = true;
                //imgmessage.Disabled = false;
            }
            return PDFPath;
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {     

            string path = string.Empty;
            string sMyPath = string.Empty;
            string PDFPath = string.Empty;
            string XMlPath = string.Empty;
            string sFilePathPDF = string.Empty;
            ArrayList FilePath = new ArrayList();
            hdnEncounterId.Value = cboEncounter.SelectedValue;
            //var xDox = new XDocument();
            System.Xml.XmlDocument XOX = new System.Xml.XmlDocument();
            //ArrayList sXMlPath = new ArrayList();
            //path = objWellnessNotes.PrintWellnessNotes(Convert.ToUInt32(cboEncounter.SelectedValue), Convert.ToUInt64(Request.QueryString["PatientID"]), true, ref sMyPath, "", false, fileManageExistList1);
            if ((cboEncounter.SelectedValue != null && cboEncounter.SelectedValue != "") && (Request.QueryString["PatientID"] != null && Request.QueryString["PatientID"].ToString() != ""))
            {
                //FilePath = objClinicalSummary.PrintClinicalSummary(Convert.ToUInt32(cboEncounter.SelectedValue), Convert.ToUInt64(Request.QueryString["PatientID"]), false, ref sMyPath, "", true, true);

                string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["ClinicalSummaryPathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                Directory.CreateDirectory(sFolderPathName);

                string sPrintPathName = string.Empty;

                sPrintPathName = sFolderPathName + "\\" + "Clinical_Summary_" + ClientSession.HumanId.ToString() + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";

                string sCheckedItems = "Reason Of Visit,Vitals,Clinical Instruction,Immunizations,Mental Status,Care Plan,Laboratory Test(s),Smoking Status,Allergy,Functional Status,Procedure(s),Laboratory Values/Results,Encounter,Goals,Assessment,Medication,Medications Administered During visit,Treatment Plan,Problem List,Reason for Referral,Implants,Future Appointment,Health Concern,Lab Test,Laboratory Information,Diagnostics Tests Pending,Future Scheduled Tests,Patient Decision Aids, Social History";

                string sStatus = UtilityManager.GenerateCCD(ClientSession.HumanId, ClientSession.EncounterId, sCheckedItems, sPrintPathName, string.Empty);
                if (sStatus == "Success")
                {
                    string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                    string[] XMLFileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                    if (hdnXmlPath.Value == string.Empty)
                    {
                        hdnXmlPath.Value = "Documents\\" + Session.SessionID.ToString() + XMLFileName[0].ToString();
                    }
                    if (hdnXmlPath.Value != null && hdnXmlPath.Value != string.Empty)
                    {
                        DirectoryInfo ObjSearchDir = new DirectoryInfo(Server.MapPath(hdnXmlPath.Value));
                        if (!Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet").Exists)
                        {
                            Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet");
                        }
                        System.IO.File.Copy(Server.MapPath("SampleXML/CDA.xsl"), Server.MapPath("Documents/" + Session.SessionID.ToString() + "/" + ObjSearchDir.Parent.Parent + "/stylesheet/CDA.xsl"), true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenAltovaPDF();", true);

                        //AuditLogManager alManager = new AuditLogManager();
                        //string TransactionType = "GENERATE - CCD";
                        //alManager.InsertIntoAuditLog("EXPORT", TransactionType, Convert.ToInt32(ClientSession.HumanId), ClientSession.UserName);//BugID:49685
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenErrorAltova();", true);
                    }

                }
                else if (sStatus == "1011192")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenWarningAltova();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenErrorAltova();", true);
                }

                sFilePathPDF = objSummaryofCare.PrintPDF(FilePath[0].ToString(), "PatientPortal", DateTime.MinValue);
                // FilePath = objClinicalSummary.ImportCCD(XOX, FilePath[0].ToString(), false, Convert.ToUInt64(Request.QueryString["PatientID"]), true);
                //string[] Split = new string[] { Server.MapPath("") };
                //string[] FileName = path.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < FilePath.Count; i++)
                {
                    string[] Split = new string[] { Server.MapPath("") };
                    string[] fileName = FilePath[i].ToString().Split(Split, StringSplitOptions.RemoveEmptyEntries);

                    if (fileName[0].ToUpper().Contains(".XML"))
                    {
                        XMlPath = fileName[0].ToString();
                    }

                    if (fileName[0].ToUpper().Contains(".PDF"))
                    {
                        PDFPath = fileName[0].ToString();
                    }
                }
                if (sFilePathPDF != string.Empty)
                {

                    string[] SplitPdf = new string[] { Server.MapPath("") };
                    string[] FileName = sFilePathPDF.Split(SplitPdf, StringSplitOptions.RemoveEmptyEntries);

                    PDFPath = FileName[0].ToString();
                }
                //  frmWord.Attributes.Add("src", "frmPrintPDF.aspx?SI=" + PDFPath + "&Location=DYNAMIC");
                Session["Path"] = PDFPath;
                Session["xmlPath"] = XMlPath;
                IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                ActivityLog activity = new ActivityLog();
                activity.Human_ID = Convert.ToUInt64(Request.QueryString["PatientID"]);
                activity.Encounter_ID = Convert.ToUInt32(cboEncounter.SelectedValue);
                activity.Activity_Type = "View";
                activity.Role = hdnRole.Value;
                if (hdnLocalTime.Value != string.Empty)
                    activity.Activity_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                ActivityLogList.Add(activity);
                ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
                // btnSend.Attributes.Add("onclick", "return SendDocument('" + PDFPath.Replace("\\", "$$").ToString() + "," + cboEncounter.SelectedValue + "," + hdnPatientName.Value + "," + lblEmailIDActual.Text + "');");
                //  btnSend.Enabled = true;
                //  btnDownload.Enabled = true;
                imgmessage.Disabled = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lnkadddocument.Attributes.Add("onclick", "return OpenOnlinedocuments()");
            //btngeneratelink.Attributes.Add("onclick", "return openGenrateLink()");
            btndelete.Attributes.Add("onclick", "return deletefile()");
            btnDownload.Attributes.Add("onclick", "return DownloadClick()");
            btnSend.Attributes.Add("onclick", "return  SendDocument()");

            string sPatientName = string.Empty;

            if (!IsPostBack)
            {
                this.Page.Title = "Patient Portal";
                IList<Human> humanList = new List<Human>();
                if (Request.QueryString["Role"] != null && Request.QueryString["Role"] != "")
                {
                    hdnRole.Value = Request.QueryString["Role"].ToString();
                }
                if (Request.QueryString["PatientID"] != null)
                {
                    ulMyHumanID = Convert.ToUInt64(Request.QueryString["PatientID"]);
                    HumanID.Value = Request.QueryString["PatientID"].ToString();
                    var serializer = new NetDataContractSerializer();
                    humanList = hnProxy.GetPatientDetailsUsingPatientInformattion(ulMyHumanID);
                    hdnPatientName.Value = humanList[0].Last_Name + " " + humanList[0].First_Name + " " + humanList[0].MI;
                    cboEncounter.Items.Clear();
                    IList<Encounter> EncList = encPRoxy.GetEncounterUsingHumanID(ulMyHumanID);
                    Session["EncounterList"] = EncList;
                    for (int i = 0; i < EncList.Count; i++)
                    {
                        if (EncList[i].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy") != "01-Jan-0001")
                        {
                            // cboEncounter.Items.Add(new ListItem(EncList[i].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm tt")));
                            cboEncounter.Items.Add(new ListItem(UtilityManager.ConvertToLocal(EncList[i].Date_of_Service).ToString("dd-MMM-yyyy hh:mm tt")));
                            cboEncounter.Items[i].Value = EncList[i].Id.ToString();
                        }
                    }


                }
                hdnEncounterId.Value = cboEncounter.SelectedValue;
                sPatientName = humanList[0].Last_Name + "," + humanList[0].First_Name;
                if (hdnRole.Value == "Representative")
                {
                    lblEmailIDActual.Text = humanList[0].Representative_Email;
                    ClientSession.UserName = lblEmailIDActual.Text;
                    lnkPatientAccount.Attributes.Add("onclick", "return PatientAccountClick('" + ulMyHumanID + "','" + humanList[0].Last_Name + " " + humanList[0].First_Name + "','" + humanList[0].Representative_Email + "');");

                }
                else
                {
                    lblEmailIDActual.Text = humanList[0].EMail;
                    ClientSession.UserName = lblEmailIDActual.Text;
                    lnkPatientAccount.Attributes.Add("onclick", "return PatientAccountClick('" + ulMyHumanID + "','" + humanList[0].Last_Name + " " + humanList[0].First_Name + "','" + humanList[0].EMail + "');");
                }
                lnkActiveHistory.Attributes.Add("onclick", "return ActivityHistoryClick('" + ulMyHumanID + "','" + sPatientName + "');");
                string mailDetails = Convert.ToUInt64(Request.QueryString["PatientID"]) + "," + lblEmailIDActual.Text + "," + cboEncounter.SelectedValue;
                hdnMailDetails.Value = mailDetails;
                imgmessage.Attributes.Add("onclick", "return SendMail('" + Convert.ToUInt64(Request.QueryString["PatientID"]) + "," + lblEmailIDActual.Text + "," + cboEncounter.SelectedValue + "');");
                // btnSend.Attributes.Add("onclick", "return SendMessage('" + "" + "');");
                // btnSendMessage.Attributes.Add("onclick", "return btnSendMessage();");
                // btnDownload.Enabled = false;
                //  btnSend.Enabled = false;
                imgmessage.Disabled = false;
                lblPatientStrip.Items[0].Text = FillPatientSummaryBarforPatientChart(humanList[0].Last_Name, humanList[0].First_Name, humanList[0].MI, humanList[0].Suffix, humanList[0].Birth_Date, humanList[0].Id, humanList[0].Medical_Record_Number, humanList[0].Home_Phone_No, humanList[0].Sex, humanList[0].Account_Status, humanList[0].SSN, "", "", "", "", "");

            }
            RadioButton rdbtnPDF = RadWindow1.ContentContainer.FindControl("rdnPdf") as RadioButton;
            rdbtnPDF.Attributes.Add("onclick", "EnableBtnDownload();");
            RadioButton rdbtnXml = RadWindow1.ContentContainer.FindControl("rdnXml") as RadioButton;
            rdbtnXml.Attributes.Add("onclick", "EnableBtnDownload();");

            RadioButton rdbtnSendPDF = SendRecordWindow.ContentContainer.FindControl("rbtSendRecordPDF") as RadioButton;
            rdbtnSendPDF.Attributes.Add("onclick", "EnableBtnSend();");
            RadioButton rdbtnSendXML = SendRecordWindow.ContentContainer.FindControl("rbtSendRecordXML") as RadioButton;
            rdbtnSendXML.Attributes.Add("onclick", "EnableBtnSend();");
            RadioButton rdbtnSendBoth = SendRecordWindow.ContentContainer.FindControl("rbtSendRecordBoth") as RadioButton;
            rdbtnSendBoth.Attributes.Add("onclick", "EnableBtnSend();");

            ActivityLogManager ActivitylogMngr = new ActivityLogManager();
            IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
            ActivityLog activity = new ActivityLog();

            activity.Human_ID = ulMyHumanID;
            activity.Encounter_ID = 0;
            activity.Activity_Type = "Patient is accessing Patient Portal";
            activity.Activity_By = ClientSession.UserName;
            activity.Role = "Patient";
            activity.Activity_Date_And_Time = DateTime.Now;
            ActivityLogList.Add(activity);
            ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
        }



        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //Session.Abandon();
            ulMyHumanID = Convert.ToUInt64(Request.QueryString["PatientID"]);
            string Email = Request.QueryString["EmailID"].ToString();
            Response.Redirect("webfrmLogin.aspx?PatientID=" + ulMyHumanID + "&Email=" + Email);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            frmWord.Attributes["src"] = "about:blank";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string PDFPath = string.Empty;
            string XMLPath = string.Empty;
            if (hdnIsZip.Value != null && hdnIsZip.Value == "true")
            {
                PDFPath = Session["Zip_PdfPath"].ToString();
                XMLPath = Session["Zip_xmlPath"].ToString();
            }
            else
            {
                PDFPath = Session["Path"].ToString();
                XMLPath = Session["xmlPath"].ToString();
            }


            if (IsPostBack)
            {
                if (PDFPath.IndexOf("Dictionary") != -1)
                {
                    if (hdnEncList.Value != string.Empty)
                    {
                        CreateZipFile("pdf|xml");
                        //BugId:49570 
                        PDFPath = Session["Zip_PdfPathLink"].ToString();
                        XMLPath = Session["Zip_xmlPathLink"].ToString();
                        bZip = true;
                    }
                    else
                    {
                        bZip = false;
                    }
                }
                else if (PDFPath.IndexOf("Bulk_Acess_SOC_") != -1)
                {
                    bZip = true;
                }

            }


            if (rdnPdf.Checked == true)
            {
                IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                ActivityLog activity = new ActivityLog();
                activity.Human_ID = Convert.ToUInt64(Request.QueryString["PatientID"]);
                if (!bZip)
                {
                    activity.Encounter_ID = Convert.ToUInt32(cboEncounter.SelectedValue);
                    activity.Activity_Type = "Downloaded in PDF";
                }
                else
                {
                    activity.Encounter_ID = 0;
                    activity.Activity_Type = "Downloaded as zip(PDF)";
                }
                activity.Role = hdnRole.Value;
                if (hdnLocalTime.Value != string.Empty)
                    activity.Activity_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                ActivityLogList.Add(activity);
                ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);

                if (!bZip)
                    Response.ContentType = "Application/pdf";
                else
                    Response.ContentType = "application/x-zip-compressed";
                string fPath = PDFPath.IndexOf("\\") == -1 ? PDFPath : PDFPath.Substring(PDFPath.LastIndexOf("\\") + 1);
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fPath);
                PDFPath = PDFPath.IndexOf("Bulk_Acess_SOC_") == -1 ? PDFPath : "atala-capture-download//" + Session.SessionID + "//" + PDFPath;
                Response.TransmitFile(Server.MapPath(PDFPath));
                Response.End();
            }
            else if (rdnXml.Checked == true)
            {
                IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                ActivityLog activity = new ActivityLog();
                activity.Human_ID = Convert.ToUInt64(Request.QueryString["PatientID"]);
                if (!bZip)
                {
                    activity.Encounter_ID = Convert.ToUInt32(cboEncounter.SelectedValue);
                    activity.Activity_Type = "Downloaded in XML";
                }
                else
                {
                    activity.Encounter_ID = 0;
                    activity.Activity_Type = "Downloaded as zip(XML)";
                }
                activity.Role = hdnRole.Value;
                if (hdnLocalTime.Value != string.Empty)
                    activity.Activity_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                ActivityLogList.Add(activity);
                ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);


                if (!bZip)
                    Response.ContentType = "Application/xml";
                else
                    Response.ContentType = "application/x-zip-compressed";
                string fPath = XMLPath.IndexOf("\\") == -1 ? XMLPath : XMLPath.Substring(XMLPath.LastIndexOf("\\") + 1);
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fPath);
                XMLPath = XMLPath.IndexOf("Bulk_Acess_SOC_") == -1 ? XMLPath : "atala-capture-download//" + Session.SessionID + "//" + XMLPath;
                Response.TransmitFile(Server.MapPath(XMLPath));
                Response.End();
            }

        }



        [System.Web.Services.WebMethod]
        public static string GetTextboxValues(string FieldValues)
        {
            if (ClientSession.UserName == string.Empty)
            {
                string Url = HttpContext.Current.Request.UrlReferrer.ToString();
                string[] Url1 = Url.Split('/');
                string Link = Url1[Url1.Length - 1].ToString();
                if (Link.ToUpper().Contains("EMAIL") == true)
                {
                    string[] SplitLink = Link.Split('?');
                    if (Convert.ToString(SplitLink[1]) != string.Empty)
                    {
                        HttpContext.Current.Response.StatusCode = 999;
                        HttpContext.Current.Response.Status = "999 Session Expired";
                        HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx?" + SplitLink[1].ToString();
                    }
                }

                return "Session Expired";
            }
            IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
            ActivityLogManager ActivitylogMngr = new ActivityLogManager();
            ulong ulHuman_Id = Convert.ToUInt32(FieldValues.Split(',')[0]);

            ulong ulEncounter_Id = 0;
            if (FieldValues.Split(',')[1] != "")
                ulEncounter_Id = Convert.ToUInt32(FieldValues.Split(',')[1]);
            string sActivityLog = string.Empty;
            //ActivityLogList = ActivitylogMngr.GetLogUsingHumanandEncounterID(ulHuman_Id, ulEncounter_Id);
            ActivityLogList = ActivitylogMngr.GetLogUsingHumanandEncounterID(ulHuman_Id);
            for (int i = 0; i < ActivityLogList.Count; i++)
            {
                if (ActivityLogList[i].Activity_Type.ToUpper() == "VIEW")
                {
                    if (sActivityLog == string.Empty)
                        sActivityLog = "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has viewed the Record on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                        sActivityLog += Environment.NewLine + "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has viewed the Record on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                }
                if (ActivityLogList[i].Activity_Type.ToUpper() == "DOWNLOADED IN PDF")
                {
                    if (sActivityLog == "")
                        sActivityLog = "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + "has downloaded the Record in PDF Format on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                        sActivityLog += Environment.NewLine + "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has downloaded the Record in PDF Format on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                }
                if (ActivityLogList[i].Activity_Type.ToUpper() == "DOWNLOADED IN XML")
                {
                    if (sActivityLog == "")
                        sActivityLog = "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + "has downloaded the Record in XML Format on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                        sActivityLog += Environment.NewLine + "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has downloaded the Record in XML Format on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                }

                if (ActivityLogList[i].Activity_Type.ToUpper() == "SEND")
                {
                    if (sActivityLog == "")
                        sActivityLog = "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has sent the Record on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                        sActivityLog += Environment.NewLine + "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has sent the Record on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                }
                if (ActivityLogList[i].Activity_Type.ToUpper() == "SEND MESSAGE")
                {
                    if (sActivityLog == "")
                        sActivityLog = "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has sent the Message to" + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                        sActivityLog += Environment.NewLine + "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + "has sent the Message to " + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                }
                if (ActivityLogList[i].Activity_Type.ToUpper() == "TRANSMITTED BOTH PDF XML")
                {
                    if (sActivityLog == "")
                        sActivityLog = "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has transmitted the PDF&XML to" + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                        sActivityLog += Environment.NewLine + "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + "has transmitted the PDF&XML to" + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                }
                if (ActivityLogList[i].Activity_Type.ToUpper() == "TRANSMITTED PDF")
                {
                    if (sActivityLog == "")
                        sActivityLog = "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has transmitted the PDF to " + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                        sActivityLog += Environment.NewLine + "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + "has transmitted the PDF to " + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                }
                if (ActivityLogList[i].Activity_Type.ToUpper() == "TRANSMITTED XML")
                {
                    if (sActivityLog == "")
                        sActivityLog = "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has transmitted the XML to " + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                        sActivityLog += Environment.NewLine + "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + "has transmitted the XML to" + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                }
                if (ActivityLogList[i].Activity_Type.ToUpper() == "TRANSMITTED ZIP BOTH PDF XML")
                {
                    if (sActivityLog == "")
                        sActivityLog = "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has transmitted the zip of PDF&XML to" + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                        sActivityLog += Environment.NewLine + "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + "has transmitted the zip of PDF&XML to" + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                }
                if (ActivityLogList[i].Activity_Type.ToUpper() == "TRANSMITTED ZIP PDF")
                {
                    if (sActivityLog == "")
                        sActivityLog = "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has transmitted the zip of PDF to " + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                        sActivityLog += Environment.NewLine + "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + "has transmitted the zip of PDF to " + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                }
                if (ActivityLogList[i].Activity_Type.ToUpper() == "TRANSMITTED ZIP XML")
                {
                    if (sActivityLog == "")
                        sActivityLog = "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has transmitted the zip of XML to " + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                        sActivityLog += Environment.NewLine + "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + "has transmitted the zip of XML to" + " " + ActivityLogList[i].Sent_To.ToString() + " " + "on" + " " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                }
                if (ActivityLogList[i].Activity_Type.ToUpper() == "DOWNLOADED AS ZIP(XML)")
                {
                    if (sActivityLog == "")
                        sActivityLog = "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + "has downloaded the Record in zip of XML Format on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                        sActivityLog += Environment.NewLine + "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has downloaded the Record in zip of XML Format on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                }
                if (ActivityLogList[i].Activity_Type.ToUpper() == "DOWNLOADED AS ZIP(PDF)")
                {
                    if (sActivityLog == "")
                        sActivityLog = "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + "has downloaded the Record in zip of PDF Format on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                        sActivityLog += Environment.NewLine + "*" + FieldValues.Split(',')[2] + "," + FieldValues.Split(',')[3] + "-" + ActivityLogList[i].Role + " " + " has downloaded the Record in zip of PDF Format on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                }

                //Transmitted Zip BOTH PDF XML
                //Transmitted zip PDF
                //Transmitted zip XML
                //Downloaded as zip(XML)
                //Downloaded as zip(PDF)

            }
            return sActivityLog;


        }
        public string FillPatientSummaryBarforPatientChart(string LastName, string FirstName, string MI, string Suffix, DateTime DOB, ulong ulHumanID, string MedRecNo, string HomePhoneNo, string Sex, string PatientStatus, string SSN, string PatientType, string sPriPlan, string sPriCarrier, string sSecPlan, string sSecCarrier)
        {

            string sMySummary;
            if (PatientStatus == "DECEASED")
            {
                sMySummary = LastName + "," + FirstName +
                   "  " + MI + "  " + Suffix + "   |   " +
                   DOB.ToString("dd-MMM-yyyy") + "   |   " +
                   (CalculateAge(DOB)).ToString() +
                   "  year(s)    |   " + Sex.Substring(0, 1) + "   |   " + PatientStatus + "   |   Acc #:" + ulHumanID +
                   "   |   " + "Med Rec #:" + MedRecNo + "   |   " +
                   "Phone #:" + HomePhoneNo + "   |   ";
            }
            else
            {
                sMySummary = LastName + "," + FirstName +
               "  " + MI + "  " + Suffix + "   |   " +
               DOB.ToString("dd-MMM-yyyy") + "   |   " +
               (CalculateAge(DOB)).ToString() +
               "  year(s)    |   " + Sex.Substring(0, 1) + "   |   Acc #:" + ulHumanID +
               "   |   " + "Med Rec #:" + MedRecNo + "   |   " +
               "Phone #:" + HomePhoneNo + "   |   ";

            }

            if (sPriPlan != string.Empty)
            {
                sMySummary += "Pri Plan:" + sPriCarrier + " - " + sPriPlan + "   |   ";
            }
            if (sSecPlan != string.Empty)
            {
                sMySummary += "Sec Plan:" + sSecCarrier + " - " + sSecPlan + "   |   ";
            }
            if (SSN != string.Empty)
            {
                sMySummary += "SSN:" + SSN + "   |   ";
            }
            if (PatientType != string.Empty)
            {
                sMySummary += "Patient Type:" + PatientType + "   |   ";
            }

            return sMySummary;
        }
        public int CalculateAge(DateTime birthDate)
        {
            // cache the current time
            DateTime now = DateTime.Today; // today is fine, don't need the timestamp from now
            // get the difference in years
            int years = now.Year - birthDate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;

            return years;
        }
        protected void btnSendDocument_Click(object sender, EventArgs e)
        {
            RadioButton rdbtnPDF = SendRecordWindow.ContentContainer.FindControl("rbtSendRecordPDF") as RadioButton;
            RadioButton rdbtnXML = SendRecordWindow.ContentContainer.FindControl("rbtSendRecordXML") as RadioButton;
            RadioButton rdbtnBoth = SendRecordWindow.ContentContainer.FindControl("rbtSendRecordBoth") as RadioButton;
            string PDFPath = string.Empty;
            string XMLPath = string.Empty;
            if (hdnIsZip.Value != null && hdnIsZip.Value == "true")//BugID:49297
            {
                PDFPath = Session["Zip_PdfPath"].ToString();
                XMLPath = Session["Zip_xmlPath"].ToString();
            }
            else
            {
                PDFPath = Session["Path"].ToString();
                XMLPath = Session["xmlPath"].ToString();
            }

            if (rdbtnPDF.Checked == true || rdbtnXML.Checked == true || rdbtnBoth.Checked == true)
            {
                MessageWindow.Modal = true;
                MessageWindow.Height = 450;
                MessageWindow.VisibleOnPageLoad = true;
                MessageWindow.Width = 600;
                MessageWindow.CenterIfModal = true;
                MessageWindow.VisibleTitlebar = true;
                MessageWindow.VisibleStatusbar = false;
                string IS_Patient_Portal = string.Empty;
                if (PDFPath.IndexOf("Dictionary") != -1)
                {
                    if (hdnEncList.Value != string.Empty)
                    {
                        CreateZipFile("pdf|xml");
                        PDFPath = Session["Zip_PdfPathLink"].ToString();
                        XMLPath = Session["Zip_xmlPathLink"].ToString();
                    }
                }
                // if (hdnPatientPortal.Value.ToUpper() == "YES")
                IS_Patient_Portal = "YES";
                if (rdbtnPDF.Checked == true)
                {
                    MessageWindow.NavigateUrl = "frmSendHealthRecord.aspx?FileName=" + PDFPath.Replace("\\", "$$").ToString() + "&Encounter_ID=" + cboEncounter.SelectedValue + "&LoginEmailID=" + lblEmailIDActual.Text + "&Role=" + hdnRole.Value + "&hdnEncIDs=" + hdnEncList.Value + "&Bulkaccess=" + hdnbulkaccess.Value + "&IS_Patient_Portal=" + IS_Patient_Portal;
                    //Response.Redirect("frmSendHealthRecord.aspx?FileName=" + PDFPath.Replace("\\", "$$").ToString() + "&Encounter_ID=" + cboEncounter.SelectedValue + "&LoginEmailID=" + lblEmailIDActual.Text + "&Role=" + hdnRole.Value);
                    //btnSend.Attributes.Add("onclick", "return SendMessage('" + PDFPath.Replace("\\", "$$").ToString() + "," + cboEncounter.SelectedValue + "," + hdnPatientName.Value + "," + lblEmailIDActual.Text + "');");
                }
                else if (rdbtnXML.Checked == true)
                {
                    MessageWindow.NavigateUrl = "frmSendHealthRecord.aspx?FileName=" + XMLPath.Replace("\\", "$$").ToString() + "&Encounter_ID=" + cboEncounter.SelectedValue + "&LoginEmailID=" + lblEmailIDActual.Text + "&Role=" + hdnRole.Value + "&hdnEncIDs=" + hdnEncList.Value + "&Bulkaccess=" + hdnbulkaccess.Value + "&IS_Patient_Portal=" + IS_Patient_Portal;
                    //Response.Redirect("frmSendHealthRecord.aspx?FileName=" + ulMyHumanID + "&Email=" + Email + "&Email=" + Email + "&Email=" + Email);
                    //btnSend.Attributes.Add("onclick", "return SendMessage('" + XMLPath.Replace("\\", "$$").ToString() + "," + cboEncounter.SelectedValue + "," + hdnPatientName.Value + "," + lblEmailIDActual.Text + "');");
                }
                else if (rdbtnBoth.Checked == true)
                {
                    string PDFXML = PDFPath + "|" + XMLPath;
                    MessageWindow.NavigateUrl = "frmSendHealthRecord.aspx?FileName=" + PDFXML.Replace("\\", "$$").ToString() + "&Encounter_ID=" + cboEncounter.SelectedValue + "&LoginEmailID=" + lblEmailIDActual.Text + "&Role=" + hdnRole.Value + "&hdnEncIDs=" + hdnEncList.Value + "&Bulkaccess=" + hdnbulkaccess.Value + "&IS_Patient_Portal=" + IS_Patient_Portal;
                    //Response.Redirect("frmSendHealthRecord.aspx?FileName=" + ulMyHumanID + "&Email=" + Email + "&Email=" + Email + "&Email=" + Email);
                    //btnSend.Attributes.Add("onclick", "return SendMessage('" + PDFXML.Replace("\\", "$$").ToString() + "," + cboEncounter.SelectedValue + "," + hdnPatientName.Value + "," + lblEmailIDActual.Text + "');");
                }
            }
            hdnIsZip.Value = "";
            hdnbulkaccess.Value = "";
        }

        protected void btndelete_Click(object sender, EventArgs e)
        {

            string path = hdnfilepath.Value;
            string filemanagementIndex = hdnindexid.Value;
            IList<FileManagementIndex> lstfile = new List<FileManagementIndex>();
            IList<FileManagementIndex> lstfileinsert = new List<FileManagementIndex>();
            IList<Scan> lstscan = new List<Scan>();
            IList<Scan> lstscaninsert = new List<Scan>();
            IList<scan_index> lstscanindex = new List<scan_index>();
            IList<scan_index> lstscanindexinsert = new List<scan_index>();
            FileManagementIndex objFileManagementIndex = new FileManagementIndex();
            scan_index objScan_Index = new scan_index();
            Scan objscan = new Scan();
            FileManagementIndexManager objIFileManagementIndexManager = new FileManagementIndexManager();
            ScanManager objscanmanager = new ScanManager();
            Scan_IndexManager objscanindex = new Scan_IndexManager();

            WFObjectManager objwfobjectmanger = new WFObjectManager();
            IList<WFObject> objwfobject = new List<WFObject>();
            objFileManagementIndex = objIFileManagementIndexManager.GetById(Convert.ToUInt64(filemanagementIndex));
            if (objFileManagementIndex != null)
            {
                objScan_Index = objscanindex.GetById(objFileManagementIndex.Scan_Index_Conversion_ID);
                lstfile.Add(objFileManagementIndex);
                objIFileManagementIndexManager.SaveUpdateDeleteWithTransaction(ref lstfileinsert, null, lstfile, "");

            }

            if (objScan_Index != null)
            {
                lstscanindex.Add(objScan_Index);
                objscanindex.SaveUpdateDeleteWithTransaction(ref lstscanindexinsert, null, lstscanindex, "");

                lstscanindex = objscanindex.GetDetailsbyscanid(objScan_Index.Scan_ID);
                lstscan = objscanmanager.GetscanbyId(objScan_Index.Scan_ID);

            }
            if (lstscan.Count > 0)
            {
                objwfobject = objwfobjectmanger.GetDetailbyObjectsystemId(Convert.ToUInt64(lstscan[0].Id));
                if (objwfobject.Count > 0)
                {
                    objwfobjectmanger.MoveToNextProcess(objwfobject[0].Obj_System_Id, objwfobject[0].Obj_Type, 4, "UNKNOWN", System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), objwfobject[0].Current_Process, null, null);
                }

            }
            if (lstscanindex.Count == 0)
            {

                if (lstscan.Count > 0)
                {
                    // lstscan.Add(objscan);
                    objscanmanager.SaveUpdateDeleteWithTransaction(ref lstscaninsert, null, lstscan, "");
                }
            }

            FTPImageProcess objftp = new FTPImageProcess();
            string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
            string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
            string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];

            bool result = objftp.DeleteFromImageServer(ClientSession.HumanId.ToString(), ftpServerIP, userName, password, Path.GetFileName(path));
            if (result)
            {
                string s = "fdgdfg";
            }
        }

        protected void CreateZipFile(string type)
        {
            Dictionary<string, string> newdicPDF = (Dictionary<string, string>)Session["Zip_PdfPath"];
            Dictionary<string, string> newdicXML = (Dictionary<string, string>)Session["Zip_xmlPath"];
            IList<string> PDFfilesList = new List<string>();
            IList<string> XMLfilesList = new List<string>();


            IList<string> EncIdslist = hdnEncList.Value.Split(',').ToList<string>();
            hdnEncList.Value = string.Empty;
            switch (type)
            {
                case "pdf":
                    {
                        foreach (var item in newdicPDF)
                        {
                            if (EncIdslist.IndexOf(item.Key) != -1)
                                PDFfilesList.Add(item.Value);
                        }
                        CreateZipFilefromList(PDFfilesList, type);
                    }
                    break;
                case "xml":
                    {
                        foreach (var item in newdicXML)
                        {
                            if (EncIdslist.IndexOf(item.Key) != -1)
                                XMLfilesList.Add(item.Value);
                        }
                        CreateZipFilefromList(XMLfilesList, type);
                    }
                    break;
                case "pdf|xml":
                    {
                        foreach (var item in newdicPDF)
                        {
                            if (EncIdslist.IndexOf(item.Key) != -1)
                                PDFfilesList.Add(item.Value);
                        }
                        CreateZipFilefromList(PDFfilesList, type.Split('|')[0]);
                        foreach (var item in newdicXML)
                        {
                            if (EncIdslist.IndexOf(item.Key) != -1)
                                XMLfilesList.Add(item.Value);
                        }
                        CreateZipFilefromList(XMLfilesList, type.Split('|')[1]);
                    }
                    break;
                default: break;
            }
        }

        protected void CreateZipFilefromList(IList<string> filesList, string type)
        {
            string zipName = string.Empty;
            if (filesList != null && filesList.Count > 0)
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;

                    DirectoryInfo directorySelected = new DirectoryInfo(Server.MapPath(filesList[0]));

                    string format = "*." + type;
                    foreach (FileInfo fileToCompress in directorySelected.Parent.GetFiles(format))
                    {
                        foreach (string filename in filesList)
                        {
                            if (Server.MapPath(filename) == fileToCompress.FullName)
                            {
                                string filePath = fileToCompress.FullName;
                                zip.AddFile(filePath, "");
                            }
                        }
                    }
                    if (type == "xml")
                    {
                        zip.AddFile(Server.MapPath("\\SampleXML\\CDA.xsl"), "");
                    }
                    zipName = String.Format("Bulk_Acess_SOC_" + type.ToUpper() + "_{0}.zip", DateTime.Now.ToString("dd_MMM_yyyy hh_mm tt"));
                    DirectoryInfo dirSave = new DirectoryInfo(Server.MapPath("atala-capture-download//" + Session.SessionID));
                    if (!dirSave.Exists)
                    {
                        dirSave.Create();
                    }
                    zip.Save(Server.MapPath("atala-capture-download//" + Session.SessionID + "//" + zipName));
                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "downloadURI('" + Page.ResolveClientUrl("atala-capture-download//" + Session.SessionID + "//" + zipName) + "');", true);
                }
                if (type == "pdf")
                {
                    Session["Zip_PdfPathLink"] = zipName;
                }
                else if (type == "xml")
                {
                    Session["Zip_xmlPathLink"] = zipName;
                }
            }
        }

        public void btnsendResult_Click(object sender, EventArgs e)
        {
            string filename = Session["ResultFilePath"].ToString();


            MessageWindow.NavigateUrl = "frmSendHealthRecord.aspx?ResultFileName=" + filename;
        }



        [System.Web.Services.WebMethod]
        public static string CheckDelete(string FileMngtIndexID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sReturn = "";
            if (FileMngtIndexID != null && FileMngtIndexID != "")
            {
                IList<FileManagementIndex> FileMngtList = new List<FileManagementIndex>();
                FileManagementIndexManager objIFileManagementIndexManager = new FileManagementIndexManager();
                FileMngtList = objIFileManagementIndexManager.GetFileListUsingFileIndexID(Convert.ToUInt32(FileMngtIndexID));

                if (FileMngtList.Count() > 0 && FileMngtList[0].Created_By.Trim().ToUpper() == ClientSession.UserName.Trim().ToUpper())
                {
                    sReturn = "Success";
                }
            }
            return sReturn;
        }
    }
}