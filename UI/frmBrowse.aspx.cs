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
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DTO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web.Services;

using EMRDirect.phiMail;
using Ionic.Zip;
using Telerik.Web.UI;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Net;

namespace Acurus.Capella.UI
{
    public partial class frmBrowse : System.Web.UI.Page
    {
        #region Declarations

        string sMyDialogMode = string.Empty;
        IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
        ActivityLogManager ActivitylogMngr = new ActivityLogManager();
        ActivityLog activity = new ActivityLog();
        string PhiMailDirectory = System.Configuration.ConfigurationManager.AppSettings["phiMailDownloadDirectory"].ToString();
        IList<string> ilstFile = new List<string>();
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    sMyDialogMode = Request["DialogMode"].ToString();
                    ViewState["sMyDialogMode"] = sMyDialogMode;
                    FillGrid();




                }
                catch (Exception BrowseLoad)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Browse Load", "alert('" + BrowseLoad.Message + "');", true);
                }
            }
            string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
            if (File.Exists(strXmlFilePath) == true)
            {
                XmlDocument itemDoc = new XmlDocument();
                XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                itemDoc.Load(XmlText);
                XmlText.Close();
                XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("ErrorFileNamesList");
                if (xmlNodeList.Count > 0)
                {
                    for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                    {
                        ilstFile.Add(xmlNodeList[0].ChildNodes[j].InnerText);
                    }
                }
            }
            lnkActiveHistory.Attributes.Add("onclick", "return ActivityHistoryClick();");
            if (ConfigurationSettings.AppSettings["Is_Cerner"] != null && ConfigurationSettings.AppSettings["Is_Cerner"].ToString().ToUpper() == "Y")
            {
                btnCernerDownload.Visible = true;
            }
            else
            {
                btnCernerDownload.Visible = false;
            }

        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                string FileName = string.Empty;
                string FileType = string.Empty;
                string file_path_for_Save_Summary = string.Empty;
                bool NegFile = false;
                foreach (GridDataItem item in grdImport.SelectedItems)
                {
                    //LinkButton lnk = item["FileName"].Controls[0] as LinkButton;
                    //FileName = lnk.Text;
                    FileName = item["FileName"].Text;
                    FileType = item["Type"].Text;
                }
                foreach (GridDataItem item in grdVerifiedNegativeFiles.SelectedItems)
                {
                    //LinkButton lnk = item["FileName"].Controls[0] as LinkButton;
                    //FileName = lnk.Text;
                    FileName = item["FileName"].Text;
                    FileType = item["Type"].Text;
                    NegFile = true;
                }
                if (NegFile)
                {
                    hdnFilePath.Value = PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\NegativeFiles\\" + FileName;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "openErrorForm('ReviewedFile');", true);
                    return;
                }
                if (FileName == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('000015'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
                if (ViewState["sMyDialogMode"] != null)
                    sMyDialogMode = ViewState["sMyDialogMode"].ToString();
                string targetdir = "Documents\\" + Session.SessionID + "\\" + System.Configuration.ConfigurationManager.AppSettings["ClinicalSummaryPathName"];
                string sDirPath = Server.MapPath("Documents/" + Session.SessionID + "//" + System.Configuration.ConfigurationManager.AppSettings["ClinicalSummaryPathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd"));
                DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);
                if (!ObjSearchDir.Exists)
                    ObjSearchDir.Create();
                ObjSearchDir.Parent.CreateSubdirectory("stylesheet");
                if (File.Exists(Server.MapPath(targetdir + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + FileName)))
                {
                    File.Delete(Server.MapPath(targetdir + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + FileName));
                }

                File.Copy(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\" + FileName, Server.MapPath(targetdir + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + FileName));
                //fuImport.PostedFile.SaveAs(Server.MapPath(targetdir + "\\" + DateTime.Now.ToString("yyyyMMdd")+ "\\" + filename));
                file_path_for_Save_Summary = Server.MapPath(targetdir + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + FileName);
                hdnFilePath.Value = targetdir + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + FileName;
                //string filePath = Server.MapPath("Documents\\" + Session.SessionID + "\\" + filename);
                /*
                if ((radCCDA_Ambulatory.Checked == true && file_path_for_Save_Summary != string.Empty) || (radCCDA_Inpatient.Checked == true && file_path_for_Save_Summary != string.Empty))
                {
                     //frmSummaryOfCare objsummary = new frmSummaryOfCare();
                    //objsummary.PrintPDF(file_path_for_Save_Summary, "CCD", Convert.ToDateTime(hdnLocalTime.Value));
                    hdnCCR.Value = "CCD";
                    ClientSession.Save_Summary = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPDFScript", "OpenPDF();", true);
                    return;
                }
                else if ((radC32.Checked == true && file_path_for_Save_Summary != string.Empty)  || (radCCR.Checked == true && file_path_for_Save_Summary != string.Empty))
                {
                    hdnCCR.Value = "C32";
                    if (hdnFilePath.Value != string.Empty)
                    {
                        string path = "Documents/" + Session.SessionID + "//" + System.Configuration.ConfigurationSettings.AppSettings["ClinicalSummaryPathName"] + "/stylesheet";
                        DirectoryInfo ObjSearchDir = new DirectoryInfo(Server.MapPath(path));
                        if (!ObjSearchDir.Exists)
                            ObjSearchDir.Create();
                        ObjSearchDir.Parent.CreateSubdirectory("stylesheet");
                        System.IO.File.Copy(Server.MapPath("SampleXML/CDA.xsl"), Server.MapPath(targetdir + "/stylesheet/CDA.xsl"), true);
                        System.IO.File.Copy(Server.MapPath("SampleXML/CCR.xsl"), Server.MapPath(targetdir + "/stylesheet/CCR.xsl"), true);
                        hdnFilePath.Value = targetdir + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + filename;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenXMLScript", "ShowXML();", true);
                        return;
                    }
                }*/
                if (Path.GetExtension(FileName).ToUpper() == ".XML")
                {

                    if ((FileType == "CCDA" || FileType == "C32") && file_path_for_Save_Summary != string.Empty)
                    {
                        if (FileType == "C32")
                            hdnCCR.Value = "C32";
                        else
                            hdnCCR.Value = "CCD";
                        ClientSession.Save_Summary = false;
                        // UtilityManager um = new UtilityManager();
                        //bool ISCCD_Valid = false;
                        // string ErrorDetail = string.Empty;
                        string path = Server.MapPath(hdnFilePath.Value);
                        //ISCCD_Valid = um.ValidateCCD(path, out ErrorDetail);
                        //if (ISCCD_Valid)
                        //{
                        if (ilstFile.Contains(FileName))
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "openErrorForm('');", true);
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPDFScript", "OpenPDF();", true);
                            if (!Directory.Exists(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles"))
                                Directory.CreateDirectory(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles");
                            if (File.Exists(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles\\" + FileName))
                                File.Delete(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles\\" + FileName);

                            //BugID:51185 - File moved to ImportedFiles folder on Reconcile Click.
                            //File.Move(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\" + FileName, PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles\\" + FileName);
                            FillGrid();
                        }
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPDFScript", "ShowAlert('"+ErrorDetail+"');", true);
                        //}
                    }
                    else if (FileType == "CCR" && file_path_for_Save_Summary != string.Empty)
                    {
                        hdnCCR.Value = "C32";
                        if (hdnFilePath.Value != string.Empty)
                        {
                            string path = "Documents/" + Session.SessionID + "//" + System.Configuration.ConfigurationManager.AppSettings["ClinicalSummaryPathName"] + "/stylesheet";
                            DirectoryInfo IsDirectory = new DirectoryInfo(Server.MapPath(path));
                            if (!IsDirectory.Exists)
                                IsDirectory.Create();
                            IsDirectory.Parent.CreateSubdirectory("stylesheet");
                            System.IO.File.Copy(Server.MapPath("SampleXML/CDA.xsl"), Server.MapPath(targetdir + "/stylesheet/CDA.xsl"), true);
                            System.IO.File.Copy(Server.MapPath("SampleXML/CCR.xsl"), Server.MapPath(targetdir + "/stylesheet/CCR.xsl"), true);
                            hdnFilePath.Value = targetdir + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + FileName;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenXMLScript", "ShowXML();", true);
                            if (!Directory.Exists(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles"))
                                Directory.CreateDirectory(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles");
                            if (File.Exists(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles\\" + FileName))
                                File.Delete(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles\\" + FileName);

                            File.Move(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\" + FileName, PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles\\" + FileName);
                            FillGrid();
                        }
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                }
                else
                {
                    try
                    {
                        if (!Directory.Exists(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles"))
                            Directory.CreateDirectory(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles");
                        if (File.Exists(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles" + "\\" + FileName))
                        {
                            File.Delete(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles" + "\\" + FileName);
                        }

                        File.Move(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\" + FileName, PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles" + "\\" + FileName);
                        FillGrid();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                        string strURL = PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles" + "\\" + FileName;
                        Response.Clear();
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                        Response.TransmitFile(strURL);
                        Response.End();

                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception BrowseOk) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Browse Ok", "alert('" + BrowseOk.Message + "');", true); }
        }

        protected void btnReceive_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["phiMailDownloadDirectory"].ToString())))
            {
                Directory.Delete(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["phiMailDownloadDirectory"].ToString()), true);
            }
            ReceiveMail(txtReceive.Text);
        }


        #endregion

        #region "Methods"

        public void ReceiveMail(string sender)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            PhiMailConnector pcConnection;
            try
            {
                if (System.Configuration.ConfigurationSettings.AppSettings["phiMailCertficatepath"] != null)
                    PhiMailConnector.SetServerCertificate(System.Configuration.ConfigurationSettings.AppSettings["phiMailCertficatepath"].ToString());

                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection certCollection = (X509Certificate2Collection)store.Certificates.Find(
                    X509FindType.FindByIssuerName, "EMR Direct Test", false); //"EMR Direct Test"
                if (certCollection.Count == 0) throw new Exception("No client certificate found");
                if (certCollection.Count > 1) throw new Exception("More than one certificate found");
                X509Certificate2 xcert = certCollection[0];
                store.Close();

                PhiMailConnector.SetClientCertificate(xcert);

                //PhiMailConnector.SetCheckRevocation(false);
                pcConnection = new PhiMailConnector(System.Configuration.ConfigurationSettings.AppSettings["phiMailServer"].ToString(), Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["phiMailPortNo"]));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message != null)
                    {
                        throw new Exception(ex.InnerException.Message);
                    }
                    else
                    {
                        throw new Exception(ex.InnerException.ToString());
                    }
                }
                else
                {
                    if (ex.Message != null)
                        throw new Exception(ex.Message.ToString());
                }

                // ex.Message;
                return;
            }

            try
            {
                bool receive = true;
                string sUser = string.Empty;
                string sPassword = null;

                XmlDocument xmldoc1 = new XmlDocument();
                string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
                if (File.Exists(strXmlFilePath1) == true)
                {
                    xmldoc1.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                    XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/PhysicianAddress/p" + ClientSession.PhysicianId);
                    if (nodeMatchingPhysicianAddress != null)
                    {
                        sUser = nodeMatchingPhysicianAddress.Attributes["Physician_EMail"].Value.ToString();
                    }
                }

                if (sUser == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('1007019','','receive');", true);
                    return;
                }

                pcConnection.AuthenticateUser(sUser, sPassword);
                string Header;
                if (receive)
                {
                    while (true)
                    {
                        PhiMailConnector.CheckResult cr = pcConnection.Check();
                        if (cr == null)
                        {
                            // fired When there is no new message from inbox queue
                            break;
                        }
                        else if (cr.IsMail())
                        {
                            for (int i = 0; i <= cr.NumAttachments; i++)
                            {
                                // Get content for part i of the current message.
                                PhiMailConnector.ShowResult showRes = pcConnection.Show(i);

                                // List all the headers
                                for (int j = 0; i == 0 && j < showRes.Headers.Count; j++)
                                    Header = "Header: " + showRes.Headers[j];

                                if (showRes.PartNum != 0)
                                {
                                    // Writing Attachment Part in The Location, In the Directory Name Of Sender
                                    string query = @"SELECT *  FROM physician_library  where Physician_EMail='" + cr.Recipient + "'";
                                    DataSet dsReturn = DBConnector.ReadData(query);
                                    DataTable dt = dsReturn.Tables[0];

                                    if (dt.Rows.Count > 0)
                                    {
                                        if (!Directory.Exists(Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["phiMailDownloadDirectory"].ToString() + "\\" + dt.Rows[0].Field<UInt32>("Physician_Library_ID"))))
                                        {
                                            Directory.CreateDirectory(Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["phiMailDownloadDirectory"].ToString() + "\\" + dt.Rows[0].Field<UInt32>("Physician_Library_ID")));
                                        }
                                        try
                                        {
                                            File.WriteAllBytes(Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["phiMailDownloadDirectory"].ToString()) + "\\" + dt.Rows[0].Field<UInt32>("Physician_Library_ID") + "\\" + DateTime.Now.ToString("yyyyMMdd_HH_mm_ss") + "_" + showRes.Filename, showRes.Data);
                                            Activity_Log_Entry(sender, showRes.Filename);
                                            CreateAuditentry(showRes.Filename);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                            break;
                                        }
                                    }

                                }


                            }
                            pcConnection.AcknowledgeMessage();
                        }
                        else
                        {
                            pcConnection.AcknowledgeStatus();
                        }
                    }
                    try
                    {
                        // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Orders", "DisplayErrorMessage('1007008','','');", true);
                        string DirectoryPath = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["phiMailDownloadDirectory"].ToString() + "\\" + ClientSession.PhysicianId);
                        DownLoadZIPFormateCATII(DirectoryPath);
                    }
                    catch
                    {
                        // do nothing
                    }
                }

            }


            catch
            {
                // generic exception handling for connector errors.
                // Console.WriteLine("Exception = " + ex);
            }

        }
        //public void ReceiveMail(string sender)
        //{
        //    PhiMailConnector pcConnection;

        //    try
        //    {
        //        PhiMailConnector.SetTrustAnchor((System.Configuration.ConfigurationSettings.AppSettings["phiMailCertficatepath"].ToString()));
        //        PhiMailConnector.SetCheckRevocation(false);
        //        pcConnection = new PhiMailConnector(System.Configuration.ConfigurationSettings.AppSettings["phiMailServer"].ToString(), Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["phiMailPortNo"]));
        //    }
        //    catch 
        //    {
        //        // ex.Message;
        //        return;
        //    }
        //    try
        //    {
        //        bool receive = true;
        //        pcConnection.AuthenticateUser(System.Configuration.ConfigurationSettings.AppSettings["phiMailUsername"].ToString(), System.Configuration.ConfigurationSettings.AppSettings["phiMailPassword"].ToString());
        //        string Header;
        //        if (receive)
        //        {
        //            while (true)
        //            {
        //                PhiMailConnector.CheckResult cr = pcConnection.Check();
        //                if (cr == null)
        //                {
        //                    // fired When there is no new message from inbox queue
        //                    break;
        //                }
        //                else if (cr.IsMail())
        //                {
        //                    for (int i = 0; i <= cr.NumAttachments; i++)
        //                    {
        //                        // Get content for part i of the current message.
        //                        PhiMailConnector.ShowResult showRes = pcConnection.Show(i);

        //                        // List all the headers
        //                        for (int j = 0; i == 0 && j < showRes.Headers.Count; j++)
        //                            Header = "Header: " + showRes.Headers[j];

        //                        if (showRes.PartNum != 0)
        //                        {
        //                            // Writing Attachment Part in The Location, In the Directory Name Of Sender

        //                            if (!Directory.Exists(Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["phiMailDownloadDirectory"].ToString() + "\\" + ClientSession.PhysicianId)))
        //                            {
        //                                Directory.CreateDirectory(Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["phiMailDownloadDirectory"].ToString() + "\\" + ClientSession.PhysicianId));
        //                            }
        //                            try
        //                            {
        //                                File.WriteAllBytes(Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["phiMailDownloadDirectory"].ToString()) + "\\" + ClientSession.PhysicianId + "\\" + showRes.Filename, showRes.Data);
        //                                Activity_Log_Entry(sender, showRes.Filename);
        //                                CreateAuditentry(showRes.Filename);
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                throw ex;
        //                                break;
        //                            }

        //                        }


        //                    }
        //                    pcConnection.AcknowledgeMessage();
        //                }
        //                else
        //                {
        //                    pcConnection.AcknowledgeStatus();
        //                }
        //            }
        //            try
        //            {
        //                // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Orders", "DisplayErrorMessage('1007008','','');", true);
        //                string DirectoryPath = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["phiMailDownloadDirectory"].ToString() + "\\" + ClientSession.PhysicianId);
        //                DownLoadZIPFormateCATII(DirectoryPath);
        //            }
        //            catch 
        //            {
        //                // do nothing
        //            }
        //        }

        //    }


        //    catch 
        //    {
        //        // generic exception handling for connector errors.
        //        // Console.WriteLine("Exception = " + ex);
        //    }

        //}
        public void FillGrid()
        {
            if (Directory.Exists(PhiMailDirectory + "\\" + ClientSession.PhysicianId))
            {
                RefreshNegFilesGrid();

                DirectoryInfo directorySelected = new DirectoryInfo(PhiMailDirectory + "\\" + ClientSession.PhysicianId);
                //FileInfo[] XmlFile = directorySelected.GetFiles("*.xml");
                FileInfo[] XmlFile = directorySelected.GetFiles();
                grdImport.DataSource = null;
                grdImport.DataBind();
                DataTable dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Type", typeof(string));
                hdnFileCnt.Value = XmlFile.Count() > 0 ? XmlFile.Count().ToString() : "0";//BugID:48547
                for (int i = 0; i < XmlFile.Count(); i++)
                {
                    if (Path.GetExtension(XmlFile[i].Name).ToUpper() != ".XSL")
                    {
                        DataRow dr = dt.NewRow();
                        dr["Name"] = XmlFile[i].Name;
                        //XDocument xmlDoc = XDocument.Load(PhiMailDirectory + "\\" + XmlFile[i].Name);
                        //var cssUrlQuery = from node in xmlDoc.Nodes()
                        //                  where node.NodeType == XmlNodeType.ProcessingInstruction
                        //                  select Regex.Match(((XProcessingInstruction)node).Data, "href=\"(?<url>.*?)\"").Groups["url"].Value;
                        //foreach (object obj in cssUrlQuery)
                        //{
                        //    string val = obj.ToString();
                        //}
                        //As Per discussed with selvaraman added this code on 17-12-2015 3:13:20 PM
                        if (Path.GetExtension(XmlFile[i].Name).ToUpper() == ".XML")
                        {
                            string xmlString = System.IO.File.ReadAllText(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\" + XmlFile[i].Name);
                            if (xmlString.ToUpper().Contains("CCR:"))
                            {
                                dr["Type"] = "CCR";
                            }
                            else if (xmlString.ToUpper().Contains("C32_CDA"))
                            {
                                dr["Type"] = "C32";
                            }
                            else
                            {
                                dr["Type"] = "CCDA";
                            }
                        }
                        else
                        {
                            dr["Type"] = Path.GetExtension(XmlFile[i].Name).ToUpper().Replace(".", "");
                        }
                        dt.Rows.Add(dr);
                    }
                    grdImport.DataSource = dt;
                    grdImport.DataBind();
                }
            }
            else
            {
                grdImport.DataSource = null;
                DataTable dt = new DataTable();
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                grdImport.DataSource = dt;
                grdImport.DataBind();
            }
        }

        public void RefreshNegFilesGrid()
        {
            if (Directory.Exists(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\NegativeFiles"))
            {
                DirectoryInfo directorySelectedNegative = new DirectoryInfo(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\NegativeFiles");
                FileInfo[] XmlFilesNegative = directorySelectedNegative.GetFiles();
                grdVerifiedNegativeFiles.DataSource = null;
                grdVerifiedNegativeFiles.DataBind();
                DataTable dtNeg = new DataTable();
                dtNeg.Columns.Add("Name", typeof(string));
                dtNeg.Columns.Add("Type", typeof(string));

                for (int i = 0; i < XmlFilesNegative.Count(); i++)
                {
                    if (Path.GetExtension(XmlFilesNegative[i].Name).ToUpper() != ".XSL")
                    {
                        DataRow dr = dtNeg.NewRow();
                        dr["Name"] = XmlFilesNegative[i].Name;
                        if (Path.GetExtension(XmlFilesNegative[i].Name).ToUpper() == ".XML")
                        {
                            string xmlString = System.IO.File.ReadAllText(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\NegativeFiles\\" + XmlFilesNegative[i].Name);
                            if (xmlString.ToUpper().Contains("CCR:"))
                            {
                                dr["Type"] = "CCR";
                            }
                            else if (xmlString.ToUpper().Contains("C32_CDA"))
                            {
                                dr["Type"] = "C32";
                            }
                            else
                            {
                                dr["Type"] = "CCDA";
                            }
                        }
                        else
                        {
                            dr["Type"] = Path.GetExtension(XmlFilesNegative[i].Name).ToUpper().Replace(".", "");
                        }

                        dtNeg.Rows.Add(dr);
                    }

                }
                grdVerifiedNegativeFiles.DataSource = dtNeg;
                grdVerifiedNegativeFiles.DataBind();
            }
        }
        public void Activity_Log_Entry(string sRec, string FileName)
        {
            //Comment
            activity.Human_ID = 0;
            activity.Encounter_ID = 0;
            activity.Sent_To = sRec;
            activity.From_Address = "DIRECT MESSAGE";//BugID:50195
            activity.Activity_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
            activity.Role = "Provider";
            activity.Subject = FileName;
            activity.Message = "";
            activity.Activity_By = ClientSession.UserName;
            activity.Activity_Type = "CCD Import";
            ActivityLogList.Add(activity);
            ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);

        }

        private void CreateAuditentry(string TransactType)//BugID:50433
        {
            AuditLogManager alManager = new AuditLogManager();
            string TransactionType = "RECEIVE";
            alManager.InsertIntoAuditLog("CCD IMPORT", TransactionType, Convert.ToInt32(ClientSession.HumanId), ClientSession.UserName);//BugID:49685
        }

        [System.Web.Services.WebMethod(EnableSession = true)]

        public static string GetActivities(string FieldValues)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
            ActivityLogManager ActivitylogMngr = new ActivityLogManager();
            List<string> ActivityType = new List<string>();
            ActivityType.Add(FieldValues.Split(',')[0]);
            ActivityType.Add(FieldValues.Split(',')[1]);
            string sActivityLog = string.Empty;
            ActivityLogList = ActivitylogMngr.GetActivityTypeByusername(ActivityType, ClientSession.UserName.ToString());
            for (int i = 0; i < ActivityLogList.Count; i++)
            {
                if (ActivityLogList[i].Activity_Type.ToUpper() == "CCD IMPORT")
                {
                    if (sActivityLog == string.Empty)
                    {
                        if (ActivityLogList[i].Message != "")
                            sActivityLog = "*" + " Imported On " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") + " | From:" + ActivityLogList[i].Sent_To + " | File Name: " + ActivityLogList[i].Subject + " | Message: " + ActivityLogList[i].Message;
                        else
                        {
                            sActivityLog = "*" + " Imported On " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") + " | From:" + ActivityLogList[i].Sent_To + " | File Name: " + ActivityLogList[i].Subject;
                        }
                    }


                    //    sActivityLog = "*" + ActivityLogList[i].Subject + " " + " Imported On " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    else
                    //    sActivityLog += Environment.NewLine + "*" + ActivityLogList[i].Subject + " " + " Imported On " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    {
                        if (ActivityLogList[i].Message != "")
                            sActivityLog += Environment.NewLine + "*" + " Imported On " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") + " | From:" + ActivityLogList[i].Sent_To + " | File Name: " + ActivityLogList[i].Subject + " | Message: " + ActivityLogList[i].Message;
                        else
                        {
                            sActivityLog += Environment.NewLine + "*" + " Imported On " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") + " | From:" + ActivityLogList[i].Sent_To + " | File Name: " + ActivityLogList[i].Subject;
                        }
                    }
                }



                if (ActivityLogList[i].Activity_Type.ToUpper() == "CCD EXPORT")
                {
                    IList<Human> HumanList = new List<Human>();
                    HumanManager ObjHuman = new HumanManager();
                    EncounterManager obj = new EncounterManager();
                    HumanList = ObjHuman.GetPatientDetailsUsingPatientInformattion(ActivityLogList[i].Human_ID);
                    Encounter lstencounter = obj.GetById(Convert.ToUInt64(ActivityLogList[i].Encounter_ID));
                    string dos = "";
                    if (lstencounter != null && lstencounter.Date_of_Service != null)
                        dos = UtilityManager.ConvertToLocal(lstencounter.Date_of_Service).ToString("dd-MMM-yyyy");

                    if (HumanList.Count > 0)
                    {
                        string PatientName = string.Empty;
                        for (int j = 0; j < HumanList.Count; j++)
                        {
                            PatientName = HumanList[j].First_Name + " " + HumanList[j].Last_Name + " " + HumanList[j].MI;
                            if (sActivityLog == "")
                                sActivityLog = "*" + " Exported Clinical Summary_ " + dos + " For " + PatientName + " on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                            else
                                sActivityLog += Environment.NewLine + "*" + " Exported Clinical Summary_ " + dos + " For " + PatientName + " on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                        }
                    }
                }

            }
            return sActivityLog;


        }

        public void DownLoadZIPFormateCATII(string DirName)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;

                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }
                if (Directory.Exists(DirName))
                {
                    DirectoryInfo directorySelected = new DirectoryInfo(DirName);

                    FileInfo[] XmlFile = directorySelected.GetFiles("*.xml");
                    grdImport.DataSource = null;
                    grdImport.DataBind();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Type", typeof(string));
                    dt.Columns.Add("Name", typeof(string));

                    for (int i = 0; i < XmlFile.Count(); i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Type"] = XmlFile[i].Name;
                        dr["Name"] = XmlFile[i].Name.Split('_')[0];
                        dt.Rows.Add(dr);
                    }
                    grdImport.DataSource = dt;
                    grdImport.DataBind();

                    zip.AddDirectoryByName(directorySelected.FullName.Substring(directorySelected.FullName.LastIndexOf("\\") + 1));
                    foreach (FileInfo fileToCompress in XmlFile)
                    {
                        string filePath = fileToCompress.FullName;
                        zip.AddFile(filePath, directorySelected.FullName.Substring(directorySelected.FullName.LastIndexOf("\\") + 1));
                    }
                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = directorySelected.Name + ".zip"; //String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.Flush();
                }
            }
        }
        //BugID:48547
        public void ReceiveMailDownload(string sender, bool isNotificationCountFill)
        {
            PhiMailConnector pcConnection;
            bool IsMail = false;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                if (System.Configuration.ConfigurationSettings.AppSettings["phiMailCertficatepath"] != null)
                    PhiMailConnector.SetServerCertificate(System.Configuration.ConfigurationSettings.AppSettings["phiMailCertficatepath"].ToString());

                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection certCollection = (X509Certificate2Collection)store.Certificates.Find(
                    X509FindType.FindByIssuerName, "EMR Direct Test", false); //"EMR Direct Test"
                if (certCollection.Count == 0) throw new Exception("No client certificate found");
                if (certCollection.Count > 1) throw new Exception("More than one certificate found");
                X509Certificate2 xcert = certCollection[0];
                store.Close();

                PhiMailConnector.SetClientCertificate(xcert);

                //PhiMailConnector.SetCheckRevocation(false);
                pcConnection = new PhiMailConnector(System.Configuration.ConfigurationSettings.AppSettings["phiMailServer"].ToString(), Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["phiMailPortNo"]));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message != null)
                    {
                        throw new Exception(ex.InnerException.Message);
                    }
                    else
                    {
                        throw new Exception(ex.InnerException.ToString());
                    }
                }
                else
                {
                    if (ex.Message != null)
                        throw new Exception(ex.Message.ToString());
                }

                // ex.Message;
                return;
            }
            try
            {
                bool receive = true;
                string sUser = string.Empty;
                string sPassword = null;

                XmlDocument xmldoc1 = new XmlDocument();
                string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
                if (File.Exists(strXmlFilePath1) == true)
                {
                    xmldoc1.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                    XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/PhysicianAddress/p" + ClientSession.PhysicianId);
                    if (nodeMatchingPhysicianAddress != null)
                    {
                        sUser = nodeMatchingPhysicianAddress.Attributes["Physician_EMail"].Value.ToString();
                    }
                }

                if (sUser == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('1007019','','receive');", true);
                    return;
                }

                pcConnection.AuthenticateUser(sUser, sPassword);
                string Header;
                if (receive)
                {
                    while (true)
                    {
                        PhiMailConnector.CheckResult cr = pcConnection.Check();
                        if (cr == null)
                        {
                            // fired When there is no new message from inbox queue

                            break;
                        }
                        else if (cr.IsMail())
                        {
                            for (int i = 0; i <= cr.NumAttachments; i++)
                            {
                                // Get content for part i of the current message.
                                PhiMailConnector.ShowResult showRes = pcConnection.Show(i);

                                // List all the headers
                                for (int j = 0; i == 0 && j < showRes.Headers.Count; j++)
                                    Header = "Header: " + showRes.Headers[j];

                                if (showRes.PartNum != 0)
                                {
                                    string query = @"SELECT *  FROM physician_library  where Physician_EMail='" + cr.Recipient + "'";
                                    DataSet dsReturn = DBConnector.ReadData(query);
                                    DataTable dt = dsReturn.Tables[0];

                                    if (dt.Rows.Count > 0)
                                    {
                                        // Writing Attachment Part in The Location, In the Directory Name Of Sender

                                        if (!Directory.Exists(PhiMailDirectory + "\\" + dt.Rows[0].Field<UInt32>("Physician_Library_ID")))
                                        {
                                            Directory.CreateDirectory(PhiMailDirectory + "\\" + dt.Rows[0].Field<UInt32>("Physician_Library_ID"));
                                        }
                                        try
                                        {
                                            File.WriteAllBytes(PhiMailDirectory + "\\" + dt.Rows[0].Field<UInt32>("Physician_Library_ID") + "\\" + DateTime.Now.ToString("yyyyMMdd_HH_mm_ss") + "_" + showRes.Filename, showRes.Data);
                                            IsMail = true;
                                            Activity_Log_Entry(sender, showRes.Filename);
                                            CreateAuditentry(showRes.Filename);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                    }
                                }
                                //if (showRes.PartNum == 0)
                                //{
                                //    string BodyMessage = System.Text.Encoding.UTF8.GetString(showRes.Data);
                                //}


                            }
                            pcConnection.AcknowledgeMessage();
                        }
                        else
                        {
                            pcConnection.AcknowledgeStatus();
                        }
                    }
                    if (!isNotificationCountFill)
                    {
                        try
                        {
                            FillGrid();
                            //DownLoadZIPFormateCATII(DirectoryPath);
                        }
                        catch
                        {
                            // do nothing
                        }
                    }
                    if (IsMail)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Receive Mail", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Receive Mail", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    }
                }

            }
            catch
            {
                // generic exception handling for connector errors.
                // Console.WriteLine("Exception = " + ex);
            }

        }

        #endregion

        //protected void grdImport_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        //{
        //    //IList<string> ilstXmlType = new List<string>();

        //    //grdImport.DataSource = null;
        //    //DataTable dt = new DataTable();
        //    //dt.Columns.Add("Type", typeof(string));
        //    //dt.Columns.Add("Name", typeof(string));
        //    //for (int i = 0; i < ilstXmlType.Count; i++)
        //    //{
        //    //    DataRow dr = dt.NewRow();
        //    //    dr["Type"] = "";
        //    //    dr["Name"] = "";
        //    //    dt.Rows.Add(dr);
        //    //}
        //    //grdImport.DataSource = dt;
        //    //grdImport.DataBind();

        //    //if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        //    //{
        //    //    if (e.Item is GridDataItem)
        //    //    {
        //    //        GridDataItem itemValue = (GridDataItem)e.Item;

        //    //        LinkButton btnName = (LinkButton)itemValue["btnName"].Controls[0];
        //    //        btnName.Text = "";
        //    //        btnName.Attributes.Add("OnClick", "LoadProof(null, null)");
        //    //        btnName.CommandArgument = string.Format("return Click('{0}';", itemValue.GetDataKeyValue("CustomID"));
        //    //    }
        //    //}
        //}

        protected void btnReceiveMail_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(PhiMailDirectory + "\\" + ClientSession.PhysicianId))
                {
                    Directory.CreateDirectory(PhiMailDirectory + "\\" + ClientSession.PhysicianId);
                }
                string PhyDirectMail = string.Empty;
                string PhyEdgeMail = string.Empty;
                PhysicianManager phyLibManager = new PhysicianManager();
                PhysicianLibrary pbLib = phyLibManager.GetById(ClientSession.PhysicianId);
                if (pbLib != null && pbLib.Id != 0)
                {
                    PhyDirectMail = pbLib.PhyEMail;
                    PhyEdgeMail = pbLib.Physician_Other_EMail_Username;
                }
                if (PhyDirectMail.Trim() != string.Empty || PhyEdgeMail.Trim() != string.Empty)
                {
                    ReceiveMailDownload(PhyDirectMail, false);
                    ReceiveMailDownload(PhyEdgeMail, false);
                }


            }
            catch (Exception ReceiveMail)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Browse Receive Mail", "alert('" + ReceiveMail.Message + "');", true);
            }
        }

        protected void grdVerifiedNegativeFiles_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {
                string FileName = ((GridItem)(grdVerifiedNegativeFiles.Items[e.Item.ItemIndex])).Cells[2].Text;
                if (!Directory.Exists(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\NegativeFiles\\DeletedFiles"))
                    Directory.CreateDirectory(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\NegativeFiles\\DeletedFiles");
                if (File.Exists(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\NegativeFiles\\DeletedFiles\\" + FileName))
                    File.Delete(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\NegativeFiles\\DeletedFiles\\" + FileName);

                File.Move(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\NegativeFiles\\" + FileName, PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\NegativeFiles\\DeletedFiles\\" + FileName);
            }
            RefreshNegFilesGrid();
        }

        protected void btnCernerDownload_ServerClick(object sender, EventArgs e)
        {
            if (ClientSession.HumanId.ToString() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CernerAlert", "alert('Please select a patient through Open Patient Chart menu and then try to Download from Cerner.');{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            string sReturn = string.Empty;
            CernerUtility cernerutils = new CernerUtility();
            cernerutils.RegisterPatient(ClientSession.HumanId.ToString());
            sReturn = cernerutils.RetrievePatientUniversalID(ClientSession.HumanId.ToString());
            sReturn = cernerutils.RegistryStoredQueryRequest(ClientSession.HumanId.ToString());
            //if (sReturn.Contains("Please schedule an appointment"))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "CernerAlert", "alert('" + sReturn + "');", true);
            //    return;
            //}
            
            //Refresh Directory
            FillGrid();
            if (sReturn != string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CernerReturnAlert", "alert('" + sReturn.Replace("^", "").Replace("'", "") + "');{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CernerAlertStop", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
        }
    }
}
