using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EMRDirect.phiMail;
using Ionic.Zip;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Telerik.Web.UI;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;



namespace Acurus.Capella.UI
{
    public partial class frmBulkExport : System.Web.UI.Page
    {
        #region Declarations

        IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
        IList<String> aryAttachmentList = new List<String>();
        ActivityLogManager ActivitylogMngr = new ActivityLogManager();
        FillPhysicianUser PhyUserList;
        PhysicianManager mngrPhysicians = new PhysicianManager();
        EncounterManager objEncountermanager = new EncounterManager();
        string path = string.Empty;
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime LocalDate = UtilityManager.ConvertToLocal(DateTime.UtcNow);
                dtpToDate.SelectedDate = LocalDate;
                dtpFromDate.SelectedDate = Convert.ToDateTime(LocalDate).AddMonths(-2);
                dtpFromDate.MaxDate = LocalDate;
                dtpToDate.MaxDate = LocalDate;
                if (grdGetPatient.DataSource == null)
                {
                    grdGetPatient.DataSource = new string[] { };
                    grdGetPatient.DataBind();


                }

                RadDatestart.SelectedDate = Convert.ToDateTime(ClientSession.LocalDate);
                RadDateexport.SelectedDate = Convert.ToDateTime(ClientSession.LocalDate);
                DateTime dt = Convert.ToDateTime(ClientSession.LocalDate + " " + ClientSession.LocalTime);
                dtpRecurTime.SelectedTime = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
                dtpNonRecurTime.SelectedTime = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
                chkNonRec.Checked = true;
                chkRec.Checked = false;
                if (chkNonRec.Checked == true)
                {
                    chkRec.Checked = false;
                    RadDatestart.Enabled = false;
                    dtpRecurTime.Enabled = false;
                    RadDateexport.Enabled = true;
                    dtpNonRecurTime.Enabled = true;
                    rdschedular.Enabled = false;
                    //rdschedular.FindItemByValue("daily").Selected = true;
                    //rdschedular.Items.SelectedValue = "One Time";
                    //rdschedular.SelectedValue = "daily";
                    rdschedular.Items.Clear();
                    // rdschedular.Items.Add(new RadComboBoxItem("One Time"));

                    string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                    if (File.Exists(strXmlFilePath) == true)
                    {
                        XmlDocument itemDoc = new XmlDocument();
                        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                        //XmlNodeList xmlTagName = null;
                        itemDoc.Load(XmlText);
                        XmlText.Close();

                        XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("BulkExportSchedulerpath");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            Dictionary<string, string> dictBulkExport = new Dictionary<string, string>();
                            for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                            {
                                dictBulkExport.Add(xmlNodeList[0].ChildNodes[j].Attributes[1].Value, xmlNodeList[0].ChildNodes[j].Attributes[1].Value);
                            }

                            cboDestination.DataSource = dictBulkExport;
                            cboDestination.DataTextField = "Key";
                            cboDestination.DataValueField = "Value";
                            cboDestination.DataBind();
                        }

                    }
                }

                PhyUserList = mngrPhysicians.GetPhysicianandUser(true, ClientSession.FacilityName, ClientSession.LegalOrg);
                if (PhyUserList.PhyList != null && PhyUserList.PhyList.Count > 0)
                {
                    PhyUserList.PhyList = (from PhysicianList in PhyUserList.PhyList join UserList in PhyUserList.UserList on PhysicianList.Id equals UserList.Physician_Library_ID where PhysicianList.Category.ToUpper().Trim() != "MACHINE" select new { PhyList = PhysicianList }.PhyList).ToList<PhysicianLibrary>();

                    PhyUserList.UserList = (from PhysicianList in PhyUserList.PhyList join UserList in PhyUserList.UserList on PhysicianList.Id equals UserList.Physician_Library_ID where PhysicianList.Category.ToUpper().Trim() != "MACHINE" select new { UseList = UserList }.UseList).ToList<User>();
                }
                ViewState["PhyUserListFacility"] = PhyUserList;
                cboProvider.Items.Clear();
                for (int iIndex = 0; iIndex < PhyUserList.PhyList.Count; iIndex++)
                {
                    string sPhyName = PhyUserList.PhyList[iIndex].PhyPrefix + " " + PhyUserList.PhyList[iIndex].PhyFirstName + " " + PhyUserList.PhyList[iIndex].PhyMiddleName + " " + PhyUserList.PhyList[iIndex].PhyLastName + " " + PhyUserList.PhyList[iIndex].PhySuffix;
                    cboProvider.Items.Add(new Telerik.Web.UI.RadComboBoxItem(PhyUserList.UserList[iIndex].user_name.ToString() + " - " + sPhyName, PhyUserList.PhyList[iIndex].Id.ToString()));
                    if (PhyUserList.PhyList[iIndex].Id == ClientSession.PhysicianId)
                    {
                        cboProvider.Items[iIndex].Selected = true;
                    }
                    else
                    {
                        cboProvider.Items[iIndex].Selected = false;
                    }
                }

                if (ClientSession.UserRole.ToUpper() == "PHYSICIAN")
                {
                    cboProvider.Enabled = false;
                    chkShowAllProviders.Enabled = false;
                }
            }

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {


            IList<Encounter> lstEncounter = new List<Encounter>();
            Encounter objEncounter = new Encounter();
            for (int i = 0; i < grdGetPatient.MasterTableView.Items.Count; i++)
            {
                objEncounter = new Encounter();
                if (((System.Web.UI.HtmlControls.HtmlInputCheckBox)grdGetPatient.MasterTableView.Items[i].FindControl("chkselect")).Checked == true)
                {
                    objEncounter.Id = Convert.ToUInt32(grdGetPatient.Items[i].Cells[8].Text);
                    objEncounter.Human_ID = Convert.ToUInt32(grdGetPatient.Items[i].Cells[3].Text);
                    lstEncounter.Add(objEncounter);
                }

            }

            hdnSelectedPath.Value = string.Empty;

            // IList<Encounter> lstEncounter = new List<Encounter>();
            //EncounterManager encMngr = new EncounterManager();
            //string sFromDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("yyyy-MM-dd HH:mm:ss");
            //string sToDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpToDate.SelectedDate).AddDays(1).AddMilliseconds(-1)).ToString("yyyy-MM-dd HH:mm:ss");
            //lstEncounter = encMngr.GetEncounterListForBulkExport(Convert.ToInt32(cboProvider.SelectedValue), sFromDate, sToDate);

            string sMyPath = string.Empty;
            ArrayList aryPrint = new ArrayList();
            frmClinicalSummary frmClin = new frmClinicalSummary();
            ArrayList aryPrintNew = new ArrayList();

            //Bulk Export - Old Code - Start
            //if (lstEncounter.Count > 0)
            //{
            //    for (int i = 0; i < lstEncounter.Count; i++)
            //    {
            //        //aryPrint = frmClin.PrintClinicalSummary(lstEncounter[i], lsthuman[i], false, ref sMyPath, string.Empty, true, false);
            //        aryPrint = frmClin.PrintClinicalSummary(lstEncounter[i].Id, lstEncounter[i].Human_ID, false, ref sMyPath, string.Empty, true, false);

            //        if (aryPrint != null && aryPrint.Count > 0)
            //        {
            //            for (int j = 0; j < aryPrint.Count; j++)
            //            {
            //                aryPrintNew.Add(aryPrint[j]);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "DisplayErrorMessage('1007011');", true);
            //    return;
            //}
            //string PDFPath = string.Empty;
            //string XMlPath = string.Empty;
            //if (aryPrintNew.Count > 0)
            //{
            //    XMlPath = aryPrintNew[0].ToString();
            //}
            //string listallfiles = string.Empty;
            //for (int i = 0; i < aryPrintNew.Count; i++)
            //{
            //    string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };

            //    if (aryPrintNew[i].ToString().EndsWith(".xml") == true)
            //    {
            //        string sPrintPathName = aryPrintNew[i].ToString();
            //        string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);

            //        string Filenm = "Documents\\" + Session.SessionID.ToString() + FileName[0].ToString();
            //        aryAttachmentList.Add(Filenm);
            //        if (i == 0)
            //        {
            //            listallfiles = Filenm;
            //        }
            //        else
            //        {
            //            listallfiles = "~" + Filenm;
            //        }

            //        Session["aryAttachmentList"] = aryAttachmentList;
            //    }
            //}
            ////ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ShowFile();", true);
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Downloadfile();", true);
            //AuditLogManager alManager = new AuditLogManager();
            //string TransactionType = "GENERATE";
            //alManager.InsertIntoAuditLog("BULK EXPORT", TransactionType, Convert.ToInt32(ClientSession.HumanId), ClientSession.UserName);//BugID:49685
            //Bulk Export - Old Code - End


            string sCheckedItems = "Reason Of Visit,Vitals,Clinical Instruction,Immunizations,Mental Status,Care Plan,Laboratory Test(s),Smoking Status,Allergy,Functional Status,Procedure(s),Laboratory Values/Results,Encounter,Goals,Assessment,Medication,Medications Administered During visit,Treatment Plan,Problem List,Reason for Referral,Implants,Future Appointment,Health Concern,Lab Test,Laboratory Information,Diagnostics Tests Pending,Future Scheduled Tests,Patient Decision Aids";
            if (lstEncounter.Count > 0)
            {
                string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["ClinicalSummaryPathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                Directory.CreateDirectory(sFolderPathName);

                string sPrintPathName = string.Empty;

                for (int i = 0; i < lstEncounter.Count; i++)
                {
                    sPrintPathName = sFolderPathName + "\\" + "Clinical_Summary_" + lstEncounter[i].Human_ID.ToString() + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";

                    string sStatus = UtilityManager.GenerateCCD(lstEncounter[i].Human_ID, lstEncounter[i].Id, sCheckedItems, sPrintPathName, string.Empty);

                    if (sStatus == "Success")
                    {
                        string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                        string[] XMLFileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                        if (hdnSelectedPath.Value == string.Empty)
                        {
                            hdnSelectedPath.Value = "Documents\\" + Session.SessionID.ToString() + XMLFileName[0].ToString();
                        }
                        if (hdnSelectedPath.Value != null && hdnSelectedPath.Value != string.Empty)
                        {
                            DirectoryInfo ObjSearchDir = new DirectoryInfo(Server.MapPath(hdnSelectedPath.Value));
                            if (!Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet").Exists)
                            {
                                Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet");
                            }
                            System.IO.File.Copy(Server.MapPath("SampleXML/CDA.xsl"), Server.MapPath("Documents/" + Session.SessionID.ToString() + "/" + ObjSearchDir.Parent.Parent + "/stylesheet/CDA.xsl"), true);
                            aryPrintNew.Add(sPrintPathName);

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
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "DisplayErrorMessage('1007011');", true);
                return;
            }

            string PDFPath = string.Empty;
            string XMlPath = string.Empty;
            if (aryPrintNew.Count > 0)
            {
                XMlPath = aryPrintNew[0].ToString();
            }
            string listallfiles = string.Empty;
            for (int i = 0; i < aryPrintNew.Count; i++)
            {
                string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };

                if (aryPrintNew[i].ToString().EndsWith(".xml") == true)
                {
                    string sPrintPathName = aryPrintNew[i].ToString();
                    string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);

                    string Filenm = "Documents\\" + Session.SessionID.ToString() + FileName[0].ToString();
                    aryAttachmentList.Add(Filenm);
                    if (i == 0)
                    {
                        listallfiles = Filenm;
                    }
                    else
                    {
                        listallfiles = "~" + Filenm;
                    }

                    Session["aryAttachmentList"] = aryAttachmentList;
                }
            }
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ShowFile();", true);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Downloadfile();", true);
            AuditLogManager alManager = new AuditLogManager();
            string TransactionType = "GENERATE";
            alManager.InsertIntoAuditLog("BULK EXPORT", TransactionType, Convert.ToInt32(ClientSession.HumanId), ClientSession.UserName);//BugID:49685



            //ShowFiles(listallfiles);
        }

        //[System.Web.Services.WebMethod(EnableSession = true)]
        //private void ShowFiles()
        //{
        //    if (aryAttachmentList.Count > 0)
        //    {
        //        string path = "Documents//" + Session.SessionID.ToString() + "//" + System.Configuration.ConfigurationSettings.AppSettings["ClinicalSummaryPathName"] + "/stylesheet";
        //        DirectoryInfo ObjSearchDir = new DirectoryInfo(Server.MapPath(path));
        //        if (!ObjSearchDir.Exists)
        //            ObjSearchDir.Create();
        //        ObjSearchDir.Parent.CreateSubdirectory("stylesheet");

        //        System.IO.File.Copy(Server.MapPath("SampleXML/CDA.xsl"), Server.MapPath(path + "/CDA.xsl"), true);
        //        System.IO.File.Copy(Server.MapPath("SampleXML/CCR.xsl"), Server.MapPath(path + "/CCR.xsl"), true);
        //        DownLoadZIPFormateCATII((IList<string>)Session["aryAttachmentList"]);
        //    }
        //}

        protected void chkShowAllProviders_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowAllProviders.Checked)
            {
                if (ViewState["PhyUserListAll"] == null)
                {
                    PhyUserList = mngrPhysicians.GetPhysicianandUser(false, "", ClientSession.LegalOrg);
                    if (PhyUserList.PhyList != null && PhyUserList.PhyList.Count > 0)
                    {
                        PhyUserList.PhyList = (from PhysicianList in PhyUserList.PhyList join UserList in PhyUserList.UserList on PhysicianList.Id equals UserList.Physician_Library_ID where PhysicianList.Category.ToUpper().Trim() != "MACHINE" select new { PhyList = PhysicianList }.PhyList).ToList<PhysicianLibrary>();

                        PhyUserList.UserList = (from PhysicianList in PhyUserList.PhyList join UserList in PhyUserList.UserList on PhysicianList.Id equals UserList.Physician_Library_ID where PhysicianList.Category.ToUpper().Trim() != "MACHINE" select new { UseList = UserList }.UseList).ToList<User>();

                    }
                }
                else
                {
                    PhyUserList = (FillPhysicianUser)ViewState["PhyUserListAll"];
                }
                ViewState["PhyUserListAll"] = PhyUserList;
                cboProvider.Items.Clear();
                for (int iIndex = 0; iIndex < PhyUserList.PhyList.Count; iIndex++)
                {
                    string sPhyName = PhyUserList.PhyList[iIndex].PhyPrefix + " " + PhyUserList.PhyList[iIndex].PhyFirstName + " " + PhyUserList.PhyList[iIndex].PhyMiddleName + " " + PhyUserList.PhyList[iIndex].PhyLastName + " " + PhyUserList.PhyList[iIndex].PhySuffix;
                    cboProvider.Items.Add(new Telerik.Web.UI.RadComboBoxItem(PhyUserList.UserList[iIndex].user_name.ToString() + " - " + sPhyName, PhyUserList.PhyList[iIndex].Id.ToString()));
                }
            }
            else
            {
                PhyUserList = (FillPhysicianUser)ViewState["PhyUserListFacility"];
                cboProvider.Items.Clear();
                for (int iIndex = 0; iIndex < PhyUserList.PhyList.Count; iIndex++)
                {
                    string sPhyName = PhyUserList.PhyList[iIndex].PhyPrefix + " " + PhyUserList.PhyList[iIndex].PhyFirstName + " " + PhyUserList.PhyList[iIndex].PhyMiddleName + " " + PhyUserList.PhyList[iIndex].PhyLastName + " " + PhyUserList.PhyList[iIndex].PhySuffix;
                    cboProvider.Items.Add(new Telerik.Web.UI.RadComboBoxItem(PhyUserList.UserList[iIndex].user_name.ToString() + " - " + sPhyName, PhyUserList.PhyList[iIndex].Id.ToString()));
                }
            }
        }

        #endregion

        #region Methods


        public void DownLoadZIPFormateCATII(IList<string> filelist)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;

                DirectoryInfo directorySelected = new DirectoryInfo(Server.MapPath(filelist[0]));
                //DirectoryInfo[] diArr = directorySelected.GetDirectories();
                //zip.AddDirectoryByName(directorySelected.FullName.Substring(directorySelected.FullName.LastIndexOf("\\") + 1));

                foreach (FileInfo fileToCompress in directorySelected.Parent.GetFiles("*.xml"))
                {
                    foreach (string filename in filelist)
                    {
                        if (Server.MapPath(filename) == fileToCompress.FullName)
                        {
                            string filePath = fileToCompress.FullName;
                            zip.AddFile(filePath, "");
                        }
                    }
                }
                string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                DirectoryInfo dirSave = new DirectoryInfo(Server.MapPath("atala-capture-download//" + Session.SessionID));
                if (!dirSave.Exists)
                {
                    dirSave.Create();
                }
                zip.Save(Server.MapPath("atala-capture-download//" + Session.SessionID + "//" + zipName));
                //AuditLog Entry here for BulkDownload
                AuditLogManager alManager = new AuditLogManager();
                alManager.InsertIntoAuditLog("Clinical Exchange", "COPY", 0, ClientSession.UserName);//BugID:49685
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "downloadURI('" + Page.ResolveClientUrl("atala-capture-download//" + Session.SessionID + "//" + zipName) + "');", true);
                //Response.Clear();
                //Response.ContentType = "application/zip";
                //Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                //zip.Save(Response.OutputStream);
                //Response.End();
            }
        }

        #endregion


        protected void Invisiblebuttons_Click(object sender, EventArgs e)
        {
            if (Session["aryAttachmentList"] != null)
            {
                string path = "Documents//" + Session.SessionID.ToString() + "//" + System.Configuration.ConfigurationSettings.AppSettings["ClinicalSummaryPathName"] + "/stylesheet";
                DirectoryInfo ObjSearchDir = new DirectoryInfo(Server.MapPath(path));
                if (!ObjSearchDir.Exists)
                    ObjSearchDir.Create();
                ObjSearchDir.Parent.CreateSubdirectory("stylesheet");

                System.IO.File.Copy(Server.MapPath("SampleXML/CDA.xsl"), Server.MapPath(path + "/CDA.xsl"), true);
                System.IO.File.Copy(Server.MapPath("SampleXML/CCR.xsl"), Server.MapPath(path + "/CCR.xsl"), true);
                DownLoadZIPFormateCATII((IList<string>)Session["aryAttachmentList"]);


                IList<string> lst = (IList<string>)Session["aryAttachmentList"];
                foreach (string filename in lst)
                {
                    System.Web.UI.HtmlControls.HtmlAnchor anchor = new System.Web.UI.HtmlControls.HtmlAnchor();
                    anchor.ID = filename;
                    anchor.HRef = filename;
                    anchor.InnerText = filename;
                    anchor.Attributes.Add("runat", "server");
                    anchor.Attributes.Add("target", "_blank");
                    pnlView.Controls.Add(anchor);
                    pnlView.Controls.Add(new LiteralControl("<br/><br/>"));
                }


                IList<String> aryAttachmentListNew = new List<String>();
                Session["aryAttachmentList"] = aryAttachmentListNew;
                if (ViewState["PhyUserListFacility"] != null)
                {
                    PhyUserList = (FillPhysicianUser)ViewState["PhyUserListFacility"];
                    if (PhyUserList.PhyList != null && PhyUserList.PhyList.Count > 0)
                    {
                        PhyUserList.PhyList = (from PhysicianList in PhyUserList.PhyList join UserList in PhyUserList.UserList on PhysicianList.Id equals UserList.Physician_Library_ID where PhysicianList.Category.ToUpper().Trim() != "MACHINE" select new { PhyList = PhysicianList }.PhyList).ToList<PhysicianLibrary>();

                        PhyUserList.UserList = (from PhysicianList in PhyUserList.PhyList join UserList in PhyUserList.UserList on PhysicianList.Id equals UserList.Physician_Library_ID where PhysicianList.Category.ToUpper().Trim() != "MACHINE" select new { UseList = UserList }.UseList).ToList<User>();
                    }
                    cboProvider.Items.Clear();
                    for (int iIndex = 0; iIndex < PhyUserList.PhyList.Count; iIndex++)
                    {
                        string sPhyName = PhyUserList.PhyList[iIndex].PhyPrefix + " " + PhyUserList.PhyList[iIndex].PhyFirstName + " " + PhyUserList.PhyList[iIndex].PhyMiddleName + " " + PhyUserList.PhyList[iIndex].PhyLastName + " " + PhyUserList.PhyList[iIndex].PhySuffix;
                        cboProvider.Items.Add(new Telerik.Web.UI.RadComboBoxItem(PhyUserList.UserList[iIndex].user_name.ToString() + " - " + sPhyName, PhyUserList.PhyList[iIndex].Id.ToString()));
                        if (PhyUserList.PhyList[iIndex].Id == ClientSession.PhysicianId)
                        {
                            cboProvider.Items[iIndex].Selected = true;
                        }
                        else
                        {
                            cboProvider.Items[iIndex].Selected = false;
                        }
                    }

                    if (ClientSession.UserRole.ToUpper() == "PHYSICIAN")
                    {
                        cboProvider.Enabled = false;
                        chkShowAllProviders.Enabled = false;
                    }
                }

            }
        }


        protected void btnschedular_Click(object sender, EventArgs e)
        {
            //string FileName = "Scheduler.xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["CCDSchedulertrackerFilePath"], FileName);
            string strXmlFilePath = System.Configuration.ConfigurationSettings.AppSettings["CCDSchedulertrackerFilePath"];
            if (File.Exists(strXmlFilePath) == true)
            {
                XmlDocument itemDoc = new XmlDocument();
                XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                XmlNodeList xmlTagName = null;
                itemDoc.Load(XmlText);
                XmlText.Close();

                //Delete
                if (itemDoc.GetElementsByTagName("CCDScheduler")[0] != null)
                {
                    xmlTagName = itemDoc.GetElementsByTagName("CCDScheduler")[0].ChildNodes;
                    foreach (XmlNode objnode in xmlTagName)
                    {
                        if (objnode.Attributes.GetNamedItem("PhyID").Value == ClientSession.PhysicianId.ToString())
                        {
                            objnode.ParentNode.RemoveChild(objnode);
                            break;
                        }

                    }
                    //itemDoc.Save(strXmlFilePath);
                    int trycount = 0;
                trytosaveagain:
                    try
                    {
                        itemDoc.Save(strXmlFilePath);
                    }
                    catch (Exception xmlexcep)
                    {
                        trycount++;
                        if (trycount <= 3)
                        {
                            int TimeMilliseconds = 0;
                            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                            Thread.Sleep(TimeMilliseconds);
                            string sMsg = string.Empty;
                            string sExStackTrace = string.Empty;

                            string version = "";
                            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                            string[] server = version.Split('|');
                            string serverno = "";
                            if (server.Length > 1)
                                serverno = server[1].Trim();

                            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                sMsg = xmlexcep.InnerException.Message;
                            else
                                sMsg = xmlexcep.Message;

                            if (xmlexcep != null && xmlexcep.StackTrace != null)
                                sExStackTrace = xmlexcep.StackTrace;

                            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                            string ConnectionData;
                            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                            {
                                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                {
                                    cmd.Connection = con;
                                    try
                                    {
                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            goto trytosaveagain;
                        }
                    }


                    //create tag
                    XmlNode Newnode = null;
                    Newnode = itemDoc.CreateNode(XmlNodeType.Element, "Scheduler", "");

                    XmlAttribute attlabel = itemDoc.CreateAttribute("PhyID");
                    attlabel.Value = ClientSession.PhysicianId.ToString();
                    Newnode.Attributes.Append(attlabel);

                    XmlAttribute attlabe2 = itemDoc.CreateAttribute("DOS_From_Date");
                    attlabe2.Value = Convert.ToDateTime(dtpFromDate.SelectedDate).ToString("yyyy-MM-dd");
                    Newnode.Attributes.Append(attlabe2);

                    XmlAttribute attlabe3 = itemDoc.CreateAttribute("DOS_To_Date");
                    attlabe3.Value = Convert.ToDateTime(dtpToDate.SelectedDate).ToString("yyyy-MM-dd");
                    Newnode.Attributes.Append(attlabe3);


                    if (chkNonRec.Checked == true)
                    {
                        XmlAttribute attlabe4 = itemDoc.CreateAttribute("Frequency");
                        attlabe4.Value = "One Time";
                        Newnode.Attributes.Append(attlabe4);
                        XmlAttribute attlabe5 = itemDoc.CreateAttribute("NonRecurringDate");
                        string nonrec = Convert.ToDateTime(RadDateexport.SelectedDate).ToString("yyyy-MM-dd");
                        attlabe5.Value = Convert.ToDateTime(nonrec + " " + dtpNonRecurTime.SelectedTime).ToString("yyyy-MM-dd HH:mm:ss");
                        Newnode.Attributes.Append(attlabe5);

                        XmlAttribute attlabe6 = itemDoc.CreateAttribute("RecurringDate");
                        attlabe6.Value = "";
                        Newnode.Attributes.Append(attlabe6);
                    }
                    else
                    {
                        XmlAttribute attlabe4 = itemDoc.CreateAttribute("Frequency");
                        attlabe4.Value = rdschedular.Text;
                        Newnode.Attributes.Append(attlabe4);

                        XmlAttribute attlabe5 = itemDoc.CreateAttribute("NonRecurringDate");
                        attlabe5.Value = "";
                        Newnode.Attributes.Append(attlabe5);

                        XmlAttribute attlabe6 = itemDoc.CreateAttribute("RecurringDate");
                        string rec = Convert.ToDateTime(RadDatestart.SelectedDate).ToString("yyyy-MM-dd");
                        attlabe6.Value = Convert.ToDateTime(rec + " " + dtpRecurTime.SelectedTime).ToString("yyyy-MM-dd HH:mm:ss");
                        Newnode.Attributes.Append(attlabe6);
                    }



                    XmlAttribute attlabe7 = itemDoc.CreateAttribute("Path");
                    attlabe7.Value = cboDestination.SelectedItem.Value;
                    Newnode.Attributes.Append(attlabe7);

                    XmlNodeList xmlSectionList = itemDoc.GetElementsByTagName("CCDScheduler");
                    xmlSectionList[0].AppendChild(Newnode);
                    //itemDoc.Save(strXmlFilePath);
                    int trycount1 = 0;
                trytosaveagain1:
                    try
                    {
                        itemDoc.Save(strXmlFilePath);
                    }
                    catch (Exception xmlexcep)
                    {
                        trycount1++;
                        if (trycount1 <= 3)
                        {
                            int TimeMilliseconds = 0;
                            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                            Thread.Sleep(TimeMilliseconds);
                            string sMsg = string.Empty;
                            string sExStackTrace = string.Empty;

                            string version = "";
                            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                            string[] server = version.Split('|');
                            string serverno = "";
                            if (server.Length > 1)
                                serverno = server[1].Trim();

                            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                sMsg = xmlexcep.InnerException.Message;
                            else
                                sMsg = xmlexcep.Message;

                            if (xmlexcep != null && xmlexcep.StackTrace != null)
                                sExStackTrace = xmlexcep.StackTrace;

                            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                            string ConnectionData;
                            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                            {
                                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                {
                                    cmd.Connection = con;
                                    try
                                    {
                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            goto trytosaveagain1;
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ScheduleLoad();", true);
                // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "DisplayErrorMessage('1007016');", true);
            }
            //if (chkNonRec.Checked == false)
            //{
            //    rdschedular.Items.Clear();
            //    rdschedular.Items.Add(new RadComboBoxItem("Every Day"));
            //    rdschedular.Items.Add(new RadComboBoxItem("Every Month"));
            //}
            //else
            //{
            //    rdschedular.Items.Clear();              
            //}           
        }


        protected void chkNonRecChanged(object sender, EventArgs e)
        {
            if (chkNonRec.Checked == false)
            {
                chkRec.Checked = true;
                RadDatestart.Enabled = true;
                dtpRecurTime.Enabled = true;
                RadDateexport.Enabled = false;
                dtpNonRecurTime.Enabled = false;
                rdschedular.Enabled = true;
                //rdschedular.SelectedValue = "Every Day";
                rdschedular.Items.Clear();
                rdschedular.Items.Add(new RadComboBoxItem("Every Day"));
                rdschedular.Items.Add(new RadComboBoxItem("Every Month"));
            }
            if (chkNonRec.Checked == true)
            {
                chkRec.Checked = false;
                RadDatestart.Enabled = false;
                dtpRecurTime.Enabled = false;
                RadDateexport.Enabled = true;
                dtpNonRecurTime.Enabled = true;
                rdschedular.Enabled = false;
                rdschedular.Items.Clear();
            }

        }
        protected void chkRecChanged(object sender, EventArgs e)
        {
            if (chkRec.Checked == false)
            {
                chkNonRec.Checked = true;
                RadDatestart.Enabled = false;
                dtpRecurTime.Enabled = false;
                RadDateexport.Enabled = true;
                dtpNonRecurTime.Enabled = true;
                rdschedular.Enabled = false;
                //rdschedular.SelectedValue = "One Time";
                rdschedular.Items.Clear();
                // rdschedular.Items.Add(new RadComboBoxItem("One Time"));
            }
            if (chkRec.Checked == true)
            {
                chkNonRec.Checked = false;
                RadDatestart.Enabled = true;
                dtpRecurTime.Enabled = true;
                RadDateexport.Enabled = false;
                dtpNonRecurTime.Enabled = false;
                rdschedular.Enabled = true;
                //rdschedular.SelectedValue = "Every Day";
                rdschedular.Items.Clear();
                rdschedular.Items.Add(new RadComboBoxItem("Every Day"));
                rdschedular.Items.Add(new RadComboBoxItem("Every Month"));
            }

        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            string sFromDate;
            string sToDate;
            string sProviderId = string.Empty;
            sFromDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpFromDate.SelectedDate)).ToString("yyyy-MM-dd HH:mm:ss");
            sToDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpToDate.SelectedDate).AddDays(1).AddMilliseconds(-1)).ToString("yyyy-MM-dd HH:mm:ss");
            sProviderId = cboProvider.SelectedValue;
            ArrayList objPatientLstOutput = new ArrayList();
            objPatientLstOutput = objEncountermanager.GetPatientListBulkAccess(sFromDate, sToDate, sProviderId);
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Patient_Account_Num", typeof(string)));
            dt.Columns.Add(new DataColumn("Patient_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("DOB", typeof(string)));
            dt.Columns.Add(new DataColumn("Gender", typeof(string)));
            dt.Columns.Add(new DataColumn("DOS", typeof(string)));
            dt.Columns.Add(new DataColumn("Encounter_id", typeof(string)));


            for (int i = 0; i < objPatientLstOutput.Count; i++)
            {
                object[] objGetPatientColumns = (object[])objPatientLstOutput[i];
                dr = dt.NewRow();
                dr["Patient_Account_Num"] = objGetPatientColumns[0];
                dr["Patient_Name"] = objGetPatientColumns[1];
                dr["DOB"] = objGetPatientColumns[2];
                dr["Gender"] = objGetPatientColumns[3];
                dr["DOS"] = UtilityManager.ConvertToLocal(Convert.ToDateTime(objGetPatientColumns[4].ToString()));
                dr["Encounter_id"] = objGetPatientColumns[5];
                dt.Rows.Add(dr);
            }


            grdGetPatient.DataSource = dt;
            grdGetPatient.DataBind();

        }
    }
}