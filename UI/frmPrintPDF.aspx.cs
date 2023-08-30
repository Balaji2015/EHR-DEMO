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
using System.IO;
using Telerik.Web.UI;
using System.Net;
using System.Xml;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;

namespace Acurus.Capella.UI
{
    public partial class frmPrintPDF : System.Web.UI.Page
    {
        public RadTab tab1 = null;
        ulong Human_id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //divLoading.Style.Add(HtmlTextWriterStyle.Display, "block");

            if (Request["PageTitle"] != null)
            {
                Page.Title = Request["PageTitle"].ToString();
            }
            if (Request["FaxSubject"] != null)
            {
                FaxSubject.Value = Request["FaxSubject"].ToString();
            }

            if (!IsPostBack)
            {


                if (Request["SI"] != null)
                {
                    SelectedItems.Value = Request["SI"].ToString();
                }
                if (Request["Human_ID"] != null)
                {
                    Human_id = Convert.ToUInt64(Request["Human_ID"]);

                }
                else
                {
                    Human_id = ClientSession.HumanId;
                }

                if (Request["Location"] != null)
                {
                    hdnScreenMode.Value = Request["Location"].ToString().ToUpper();
                }
                if (Request["HumanName"] != null)
                {
                    hdnHumanName.Value = Request["HumanName"].ToString();
                }

                if (Request["FromOrder"] != null)
                    btnprint.Text = "Print";
                else if (Request["ButtonName"] != null && Request["ButtonName"] != "")
                {
                    btnprint.Text = Request["ButtonName"].ToString();
                    if (Request["PDFLOADHeight"] != null && Request["PDFLOADHeight"] != "")
                        PDFLOAD.Style.Add(HtmlTextWriterStyle.Height, Request["PDFLOADHeight"].ToString());
                }
                else
                    btnprint.Text = "Print";

                if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y" && ClientSession.UserPermissionDTO.Scntab != null)
                {
                    var scn_id = (from p in ClientSession.UserPermissionDTO.Scntab where p.SCN_Name == "frmEFax" select p).ToList();
                    if (scn_id.Count() > 0)
                    {
                        var EnableEFax = from p in ClientSession.UserPermissionDTO.Screens where p.SCN_ID == Convert.ToInt32(scn_id[0].SCN_ID) && p.Permission == "U" select p;
                        if (EnableEFax.Count() > 0)
                            btnSendfax.Enabled = true;
                        else
                            btnSendfax.Enabled = false;
                    }

                }


                if (SelectedItems.Value.Trim() != string.Empty)
                {
                    if (SelectedItems.Value.Contains('|') == true)
                    {
                        string[] strSplit = SelectedItems.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in strSplit)
                        {
                            if (Request["Location"] != null)
                            {
                                if (Request["Location"].ToUpper() == "DYNAMIC")
                                {
                                    string[] index = null;
                                    tab1 = new RadTab();
                                    index = s.ToString().Split('\\');
                                    //string[] index = SelectedItems.Value.Split('\\');
                                    string name = index[index.Length - 1];
                                    tab1.Text = name;
                                    RadTabStrip2.Tabs.Add(tab1);
                                    tab1.PageViewID = RadPageView1.ID;
                                }
                                else if (Request["Location"].ToUpper() == "STATIC")
                                {
                                    tab1 = new RadTab();
                                    if (SelectedItems.Value.Contains("Vaccination Information Statement"))
                                    {
                                        string[] tabname = s.ToString().Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                                        tab1.Text = tabname[tabname.Count() - 1];
                                        tab1.Value = s.ToString();
                                    }
                                    else
                                    {
                                        tab1.Text = s.ToString();
                                    }
                                    RadTabStrip2.Tabs.Add(tab1);
                                    tab1.PageViewID = RadPageView1.ID;
                                }

                                else if (Request["Location"].ToString().ToUpper() == "EV")
                                {
                                    string[] index = null;
                                    tab1 = new RadTab();
                                    index = s.ToString().Split('\\');
                                    string name = index[index.Length - 1];
                                    tab1.Text = Path.GetFileName(s); ;
                                    tab1.Value = s;
                                    RadTabStrip2.Tabs.Add(tab1);
                                    tab1.PageViewID = RadPageView1.ID;
                                    tab1.Attributes.Add("HumanID", Human_id.ToString());

                                }
                            }
                        }
                        if (Request["Location"] != null)
                        {
                            if (Request["Location"].ToString().ToUpper() == "STATIC")
                            {
                                if (SelectedItems.Value.Contains("Vaccination Information Statement"))
                                {
                                    PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + strSplit[0].ToString() + "&SI=" + SelectedItems.Value.ToString() + "#zoom=100" + "&Location=" + Request["Location"].ToString());
                                    FaxCurrentFileName.Value = strSplit[0].ToString();
                                }
                                else
                                {
                                    PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + Server.MapPath("Documents\\Physician_Specific_Documents\\Patient Education\\" + strSplit[0].ToString() + ".pdf") + "&SI=" + strSplit[0].ToString() + "#zoom=100" + "&Location=" + Request["Location"].ToString());
                                    FaxCurrentFileName.Value = Server.MapPath("Documents\\Physician_Specific_Documents\\Patient Education\\" + strSplit[0].ToString() + ".pdf");
                                }
                            }

                            else if (Request["Location"].ToString().ToUpper() == "EV")
                            {

                                string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpEVServerIP"];
                                string ftpUserName = System.Configuration.ConfigurationSettings.AppSettings["ftpEVUserID"];
                                string ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpEVPassword"];
                                string grouplocalPath = Server.MapPath("atala-capture-download//" + Session.SessionID + "//EV");
                                DirectoryInfo dir = new DirectoryInfo(grouplocalPath);
                                if (!dir.Exists)
                                {
                                    dir.Create();
                                }
                                FileInfo[] file = dir.GetFiles();
                                for (int i = 0; i < file.Length; i++)
                                {
                                    File.Delete(file[i].FullName);
                                }
                                string[] files = Directory.GetFiles(grouplocalPath);
                                FTPImageProcess ftpImage = new FTPImageProcess();
                                if (Human_id == 0)
                                    Human_id = ClientSession.HumanId;

                                ftpImage.DownloadFromImageServerforEV(Human_id.ToString(), ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(strSplit[0]), grouplocalPath);
                                string orig_image = grouplocalPath + "\\" + Path.GetFileName(strSplit[0]);
                                string FileLocalPath = orig_image;
                                PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + FileLocalPath + "&SI=" + strSplit[0].ToString() + "#zoom=100" + "&Location=" + Request["Location"].ToString() + "&Human_ID=" + Human_id + "&PageTitle=Eligibility Verification - Response File");
                                FaxCurrentFileName.Value = FileLocalPath;
                            }
                            else if (Request["Location"].ToString().ToUpper() == "DYNAMIC")
                            {
                                PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + Server.MapPath("~" + strSplit[0].ToString()) + "&SI=" + Request["SI"].ToString() + "#zoom=100" + "&Location=" + Request["Location"].ToString());
                                FaxCurrentFileName.Value = Server.MapPath("~" + strSplit[0].ToString());
                            }

                        }
                    }
                    else
                    {
                        if (Request["Location"] != null)
                        {
                            if (Request["Location"].ToString().ToUpper() == "STATIC")
                            {
                                tab1 = new RadTab();
                                if (SelectedItems.Value.Contains("Vaccination Information Statement"))
                                {
                                    string[] tabname = SelectedItems.Value.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                                    tab1.Text = tabname[tabname.Count() - 1];
                                    tab1.Value = SelectedItems.Value;
                                }
                                else
                                {
                                    tab1.Text = SelectedItems.Value;
                                }
                                RadTabStrip2.Tabs.Add(tab1);
                                tab1.PageViewID = RadPageView1.ID;
                                if (SelectedItems.Value.Contains("Vaccination Information Statement"))
                                {
                                    PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + SelectedItems.Value + "&SI=" + SelectedItems.Value.ToString() + "#zoom=100" + "&Location=" + Request["Location"].ToString());
                                    FaxCurrentFileName.Value = SelectedItems.Value;
                                }
                                //Cap - 854
                                else if (SelectedItems.Value == "Cash Price List")
                                {
                                   XmlDocument xml_doc = new XmlDocument();
                                    if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\staticlookup.xml"))
                                    {
                                        xml_doc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\staticlookup.xml");
                                        XmlNodeList xml_nodelst = xml_doc.GetElementsByTagName("HelpMenu");
                                        foreach (XmlNode xml_node in xml_nodelst)
                                        {
                                            if (xml_node.Attributes.GetNamedItem("Name").Value != "" && xml_node.Attributes.GetNamedItem("Name").Value== "Cash Price List" && xml_node.Attributes.GetNamedItem("ReferenceLink").Value!="")
                                            {
                                                string sPath = xml_node.Attributes.GetNamedItem("ReferenceLink").Value;
                                                PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + Server.MapPath(sPath) + "&SI=" + SelectedItems.Value.ToString() + "#zoom=100" + "&Location=" + Request["Location"].ToString());
                                                FaxCurrentFileName.Value = Server.MapPath(sPath);
                                            }
                                        }
                                    }
                                    btnSendfax.Visible = false;
                                    btnprint.Visible = false;
                                }
                                else
                                {
                                    PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + Server.MapPath("Documents\\Physician_Specific_Documents\\Patient Education\\" + SelectedItems.Value + ".pdf") + "&SI=" + SelectedItems.Value.ToString() + "#zoom=100" + "&Location=" + Request["Location"].ToString());
                                    FaxCurrentFileName.Value = Server.MapPath("Documents\\Physician_Specific_Documents\\Patient Education\\" + SelectedItems.Value + ".pdf");
                                }
                            }
                            else if (Request["Location"].ToString().ToUpper() == "DYNAMIC")
                            {

                                tab1 = new RadTab();
                                string[] index = SelectedItems.Value.Split('\\');
                                string name = index[index.Length - 1];
                                tab1.Text = name;
                                RadTabStrip2.Tabs.Add(tab1);
                                tab1.PageViewID = RadPageView1.ID;
                                if (Request["SI"].ToString().ToUpper().Contains("SUMMARY"))
                                {
                                    tab1.Visible = false;
                                    btnprint.Visible = false;
                                }

                                PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + Server.MapPath("~" + SelectedItems.Value) + "&SI=" + SelectedItems.Value.ToString() + "#zoom=100" + "&Location=" + Request["Location"].ToString());
                                FaxCurrentFileName.Value = Server.MapPath("~" + SelectedItems.Value);

                            }

                            else if (Request["Location"].ToString().ToUpper() == "EV")
                            {

                                tab1 = new RadTab();
                                string[] index = SelectedItems.Value.Split('\\');
                                string name = index[index.Length - 1];
                                tab1.Text = Path.GetFileName(SelectedItems.Value);
                                RadTabStrip2.Tabs.Add(tab1);
                                tab1.PageViewID = RadPageView1.ID;
                                if (Request["SI"].ToString().ToUpper().Contains("SUMMARY"))
                                {
                                    tab1.Visible = false;
                                    btnprint.Visible = false;
                                }

                                string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpEVServerIP"];
                                string ftpUserName = System.Configuration.ConfigurationSettings.AppSettings["ftpEVUserID"];
                                string ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpEVPassword"];
                                string grouplocalPath = Server.MapPath("atala-capture-download//" + Session.SessionID + "//EV");
                                DirectoryInfo dir = new DirectoryInfo(grouplocalPath);
                                if (!dir.Exists)
                                {
                                    dir.Create();
                                }
                                FileInfo[] file = dir.GetFiles();
                                for (int i = 0; i < file.Length; i++)
                                {
                                    File.Delete(file[i].FullName);
                                }
                                string[] files = Directory.GetFiles(grouplocalPath);
                                FTPImageProcess ftpImage = new FTPImageProcess();
                                if (Human_id == 0)
                                    Human_id = ClientSession.HumanId;
                                ftpImage.DownloadFromImageServerforEV(Human_id.ToString(), ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(Request["SI"].ToString()), grouplocalPath);
                                string orig_image = grouplocalPath + "\\" + Path.GetFileName(SelectedItems.Value);
                                string FileLocalPath = orig_image;
                                PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + FileLocalPath + "&SI=" + Request["SI"].ToString() + "#zoom=100" + "&Location=" + Request["Location"].ToString() + "&Human_ID=" + Human_id + "&PageTitle=Eligibility Verification - Response File");
                                FaxCurrentFileName.Value = FileLocalPath;

                            }

                            else if (Request["Location"].ToUpper() == "CHART")
                            {
                                PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + Server.MapPath("Documents\\" + Session.SessionID + "\\" + SelectedItems.Value) + "&SI=" + SelectedItems.Value.ToString() + "#zoom=100" + "&Location=" + Request["Location"].ToString());
                                FaxCurrentFileName.Value = Server.MapPath("Documents\\" + Session.SessionID + "\\" + SelectedItems.Value);
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            }
                        }

                    }
                    if (Request.QueryString["pdf"] != null)
                    {

                        string strPdf = Request.QueryString["pdf"].ToString();
                        FileStream fs = null;
                        BinaryReader br = null;
                        byte[] data = null;

                        try
                        {

                            fs = new FileStream(strPdf.Replace("~", ""), FileMode.Open, FileAccess.Read, FileShare.Read);
                            br = new BinaryReader(fs, System.Text.Encoding.Default);
                            data = new byte[Convert.ToInt32(fs.Length)];
                            br.Read(data, 0, data.Length);
                            Response.Clear();
                            if (Request["Location"] == "CHART")
                            {
                                Response.ContentType = "image/png";
                            }
                            else
                            {
                                Response.ContentType = "application/pdf";
                            }
                            Response.AddHeader("Content-Length", fs.Length.ToString());

                            Response.BinaryWrite(data);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.Message);
                        }
                        finally
                        {
                            if (fs != null)
                            {
                                fs.Close();
                                fs.Dispose();
                            }
                            if (br != null)
                                br.Close();
                            data = null;
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "RadAlertScript", "ShowMessage('" + Request.QueryString["pdf"] + "');", true);
                    }

                }

                #region Summary Of Care

                if (Request["ParentForm"] != null && Request["ParentForm"] == "PatientChart")
                {
                    this.Title = "Summary Of Care";
                    string localPath = System.Configuration.ConfigurationSettings.AppSettings["CapellaConfigurationSetttings"];
                    string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                    string ftpUserName = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
                    string ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];
                    FTPImageProcess _ftpImageProcess = new FTPImageProcess();
                    string file_path = Request.QueryString["FilePath"].ToString().Replace("HASHSYMBOL", "#");

                    if (file_path != string.Empty)
                    {
                        string simagePathname = file_path;
                        string _fileName = Path.GetFileName(file_path);
                        bool Is_Exist_In_Local = false;
                        DirectoryInfo localDirInfo = new DirectoryInfo(localPath + "\\Summary_Of_Care");
                        if (!localDirInfo.Exists)
                        {
                            localDirInfo.Create();
                        }
                        FileInfo[] pdfFiles = localDirInfo.GetFiles("*.pdf");
                        if (pdfFiles.Length > 0)
                        {
                            foreach (FileInfo tempFile in pdfFiles)
                            {
                                if (tempFile.Name == _fileName)
                                {
                                    Is_Exist_In_Local = true;
                                    simagePathname = localPath + "\\Summary_Of_Care\\" + _fileName;
                                    break;
                                }
                            }
                        }

                        if (!Is_Exist_In_Local)
                        {
                            if (_ftpImageProcess.DownloadFromImageServer(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserName, ftpPassword, _fileName, localPath + "\\Summary_Of_Care"))
                            {
                                simagePathname = localPath + "\\Summary_Of_Care\\" + _fileName;
                            }

                        }
                        if (simagePathname != string.Empty)
                        {

                            DirectoryInfo virdir = new DirectoryInfo(Page.MapPath("atala-capture-download/" + Session.SessionID + "/Summary_Of_Care"));
                            if (!virdir.Exists)
                            {
                                virdir.Create();
                            }
                            FileInfo[] file = virdir.GetFiles();
                            for (int i = 0; i < file.Length; i++)
                            {
                                File.Delete(file[i].FullName);
                            }
                            string filename = "atala-capture-download/" + Session.SessionID + "/Summary_Of_Care/" + _fileName;
                            string filepath = Page.MapPath(filename);
                            File.Copy(simagePathname, filepath, true);
                            //
                            Uri CurrentURL = new Uri(Request.Url.ToString());
                            string sProjectName = string.Empty;
                            string sImgPath = string.Empty;
                            for (int i = 0; i < CurrentURL.Segments.Length - 1; i++)
                            {
                                if (CurrentURL.Segments[i] != "/" && CurrentURL.Segments[i] != "//" && CurrentURL.Segments[i].StartsWith("frm") != true && sProjectName == string.Empty)
                                {
                                    sProjectName = CurrentURL.Segments[i];
                                }
                            }
                            if (sProjectName != string.Empty)
                            {
                                sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//" + filename;
                            }
                            else
                            {
                                sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + filename;
                            }


                            PDFLOAD.Attributes.Add("src", sImgPath);



                            //





                            // PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?ParentForm=PatientChart" + "&FilePath=" + Request.QueryString["FilePath"] + "#zoom=100" + "&PdfPath=" + filepath);
                            FaxCurrentFileName.Value = filepath;
                            //if (Request["PdfPath"] != null && Request["PdfPath"] != string.Empty)
                            //{
                            //    string strPdf = Request.QueryString["PdfPath"].ToString();
                            //    FileStream fs = null;
                            //    BinaryReader br = null;
                            //    byte[] data = null;
                            //    try
                            //    {

                            //        fs = new FileStream(strPdf, FileMode.Open, FileAccess.Read, FileShare.Read);
                            //        br = new BinaryReader(fs, System.Text.Encoding.Default);
                            //        data = new byte[Convert.ToInt32(fs.Length)];
                            //        br.Read(data, 0, data.Length);
                            //        Response.Clear();
                            //        Response.ContentType = "application/pdf";
                            //        Response.BinaryWrite(data);
                            //        HttpContext.Current.ApplicationInstance.CompleteRequest();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Response.Write(ex.Message);
                            //    }
                            //    finally
                            //    {
                            //        fs.Close();
                            //        fs.Dispose();
                            //        br.Close();
                            //        data = null;
                            //    }
                            //}
                        }
                    }
                }
                #endregion

                if (Request["InterpretationNotes"] != null && Request["InterpretationNotes"] == "PatientChart")
                {

                    btnprint.Visible = false;
                    btnSendfax.Visible = false;
                    if (Request["IntNotes"] != null)
                    {
                        string sPhysicianSignDate = string.Empty;
                        string sPhysicianSignName = string.Empty;
                        string sFacAddress = string.Empty;
                        if (Request["PhySigDate"] != null)
                        {
                            sPhysicianSignDate = Request["PhySigDate"].ToString();
                        }
                        if (Request["PhySigName"] != null)
                        {
                            sPhysicianSignName = Request["PhySigName"].ToString();
                        }
                        if (Request["FacAddress"] != null)
                        {
                            sFacAddress = Request["FacAddress"].ToString();
                        }
                        //Cap - 878
                       // PrintInterpretationNotesPDF(Request["IntNotes"].ToString().Replace("\"", ""), sPhysicianSignDate.Replace("\"", ""), sPhysicianSignName.Replace("\"", ""), sFacAddress.Replace("\"", "").Replace("\\r\\n", "\r\n"));
                        PrintInterpretationNotesPDF(Request["IntNotes"].ToString().Replace("$|$|$|$|", "&").Replace("!^!^!^!^", "#").Replace("~|~|~|~|", "+").Replace("\"", ""), sPhysicianSignDate.Replace("\"", ""), sPhysicianSignName.Replace("\"", ""), sFacAddress.Replace("\"", "").Replace("\\r\\n","\r\n"));
                    }
                }
                RadTabStrip2.Enabled = true;
            }

            //var idname = from u in ClientSession.UserPermissionDTO.Userscntab where u.scn_id == Convert.ToUInt64("101113") select u;
            //if (idname.ToList().Count > 0 && idname.ToList()[0].Permission == "R")
            //    btnSendfax.Enabled = false;
            if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y" && ClientSession.UserPermissionDTO.Scntab != null)
                btnSendfax.Enabled = true;
            else
            {
                btnSendfax.Enabled = false;
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();} {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

        }

        protected void RadTabStrip2_TabClick(object sender, RadTabStripEventArgs e)
        {
            if (Request["Location"] != null)
            {
                if (Request["Location"].ToString().ToUpper() == "STATIC")
                {
                    if (Request["SI"].Contains("Vaccination Information Statement"))
                    {
                        PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + e.Tab.Value + "&SI=" + e.Tab.Value + "#zoom=100" + "&Location=" + Request["Location"].ToString());
                        FaxCurrentFileName.Value = e.Tab.Value;
                    }
                    else
                    {
                        PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + Server.MapPath("Documents\\Physician_Specific_Documents\\Patient Education\\" + e.Tab.Text + ".pdf") + "&SI=" + e.Tab.Text + "#zoom=100" + "&Location=" + Request["Location"].ToString());
                        FaxCurrentFileName.Value = Server.MapPath("Documents\\Physician_Specific_Documents\\Patient Education\\" + e.Tab.Text + ".pdf");
                    }
                }
                else if (Request["Location"].ToString().ToUpper() == "DYNAMIC")
                {
                    PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + Server.MapPath("Documents\\" + Session.SessionID + "\\" + e.Tab.Text) + "&SI=" + e.Tab.Text + "#zoom=100" + "&Location=" + Request["Location"].ToString());
                    FaxCurrentFileName.Value = Server.MapPath("Documents\\" + Session.SessionID + "\\" + e.Tab.Text);
                }

                else if (Request["Location"].ToString().ToUpper() == "EV")
                {

                    string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpEVServerIP"];
                    string ftpUserName = System.Configuration.ConfigurationSettings.AppSettings["ftpEVUserID"];
                    string ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpEVPassword"];
                    string grouplocalPath = Server.MapPath("atala-capture-download//" + Session.SessionID + "//EV");
                    DirectoryInfo dir = new DirectoryInfo(grouplocalPath);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    FileInfo[] file = dir.GetFiles();
                    for (int i = 0; i < file.Length; i++)
                    {
                        File.Delete(file[i].FullName);
                    }
                    string[] files = Directory.GetFiles(grouplocalPath);
                    FTPImageProcess ftpImage = new FTPImageProcess();
                    if (Human_id == 0)
                        Human_id = Convert.ToUInt64(e.Tab.Attributes["HumanID"]);

                    ftpImage.DownloadFromImageServerforEV(Human_id.ToString(), ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(e.Tab.Value), grouplocalPath);
                    string orig_image = grouplocalPath + "\\" + Path.GetFileName(e.Tab.Value);
                    string FileLocalPath = orig_image;
                    PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?pdf=" + FileLocalPath + "&SI=" + e.Tab.Text.ToString() + "#zoom=100" + "&Location=" + Request["Location"].ToString() + "&Human_ID=" + Human_id + "&PageTitle=Eligibility Verification - Response File");
                    FaxCurrentFileName.Value = FileLocalPath;
                }
            }
            if (Request.QueryString["pdf"] != null)
            {
                string strPdf = Request.QueryString["pdf"].ToString();
                // System.IO.FileStream fs = new System.IO.FileStream(Server.MapPath("Documents\\PatientEducationMaterials\\" + strPdf + ".pdf"), System.IO.FileMode.Open, System.IO.FileAccess.Read);
                FileStream fs = null;
                BinaryReader br = null;
                byte[] data = null;
                try
                {

                    fs = new FileStream(strPdf, FileMode.Open, FileAccess.Read, FileShare.Read);
                    br = new BinaryReader(fs, System.Text.Encoding.Default);
                    data = new byte[Convert.ToInt32(fs.Length)];
                    br.Read(data, 0, data.Length);
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(data);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                    br.Close();
                    data = null;
                }
            }
        }
        void PrintInterpretationNotesPDF(string sReviewNotes, string sPhysicianSignDate, string sPhysicianSignName, string sFaclilityName)
        {
            PrintOrders print = new PrintOrders();
            string sOutput = string.Empty;

            string sDirPath = Server.MapPath("Documents/" + Session.SessionID);

            DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);
            if (!ObjSearchDir.Exists)
            {

                ObjSearchDir.Create();
            }
            string TargetFileDirectory = Server.MapPath("Documents\\" + Session.SessionID);

            string sPhysicianName = string.Empty;
            string sSignDate = sPhysicianSignDate;

            XmlDocument xmldoc1 = new XmlDocument();
            string strXmlFilePath1 = string.Empty;
            //string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
            //if (File.Exists(strXmlFilePath1) == true && ClientSession.CurrentPhysicianId != 0)
            //{
            //    xmldoc1.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
            //    XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/PhysicianAddress/p" + ClientSession.CurrentPhysicianId);
            //    if (nodeMatchingPhysicianAddress != null)
            //    {
            //        sPhysicianName = nodeMatchingPhysicianAddress.Attributes["Physician_prefix"].Value.ToString() + " " +
            //        nodeMatchingPhysicianAddress.Attributes["Physician_First_Name"].Value.ToString() + " " +
            //        nodeMatchingPhysicianAddress.Attributes["Physician_Middle_Name"].Value.ToString() + " " +
            //        nodeMatchingPhysicianAddress.Attributes["Physician_Last_Name"].Value.ToString() + " " +
            //        nodeMatchingPhysicianAddress.Attributes["Physician_Suffix"].Value.ToString();
            //    }
            //}
            string sPatientInfo = string.Empty;
            IList<string> ilstHumanTag = new List<string>();
            ilstHumanTag.Add("HumanList");

            IList<object> ilstHumanBlobList = new List<object>();
            ilstHumanBlobList = UtilityManager.ReadBlob(ClientSession.HumanId, ilstHumanTag);

            Human objFillHuman = new Human();

            if (ilstHumanBlobList != null && ilstHumanBlobList.Count > 0)
            {
                if (ilstHumanBlobList[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobList[0]).Count; iCount++)
                    {
                        objFillHuman = ((Human)((IList<object>)ilstHumanBlobList[0])[iCount]);
                    }
                }
            }
            sPatientInfo = "Patient Name: " + objFillHuman.Last_Name + "," + objFillHuman.First_Name + "  " + objFillHuman.MI + "  " + objFillHuman.Suffix + Environment.NewLine +
             "Date of Birth: " + objFillHuman.Birth_Date.ToString("dd-MMM-yyyy") + Environment.NewLine + "MRN: " + objFillHuman.Medical_Record_Number + Environment.NewLine;



           //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
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



           //        if (itemDoc.GetElementsByTagName("HumanList") != null && itemDoc.GetElementsByTagName("HumanList").Count > 0)
           //        {
           //            xmlTagName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes;

           //            if (xmlTagName != null)
           //            {
           //                for (int j = 0; j < xmlTagName.Count; j++)
           //                {
           //                    if (xmlTagName[j].Attributes["Id"].Value == ClientSession.HumanId.ToString())
           //                    {
           //                        DateTime dt = Convert.ToDateTime(xmlTagName[j].Attributes["Birth_Date"].Value);
           //                        sPatientInfo = "Patient Name: " + xmlTagName[j].Attributes["Last_Name"].Value + "," + xmlTagName[j].Attributes["First_Name"].Value + "  " + xmlTagName[j].Attributes["MI"].Value + "  " + xmlTagName[j].Attributes["Suffix"].Value + Environment.NewLine +
           //   "Date of Birth: " + dt.ToString("dd-MMM-yyyy") + Environment.NewLine + "MRN: " + xmlTagName[j].Attributes["Medical_Record_Number"].Value + Environment.NewLine;
           //                    }
           //                }
           //            }

           //        }
           //        fs.Close();
           //        fs.Dispose();
           //    }
           //}

           string[] reviewcomments = sReviewNotes.Split(new string[] { "]]]" }, StringSplitOptions.None);
            string NotesHistory = "";
            string notesattribute = "";

            for (int i = 0; i < reviewcomments.Length; i++)
            {
                if (reviewcomments[i].Trim() != string.Empty)
                {
                    if (reviewcomments[i].Contains("Test Reviewed: ") == true)
                    {
                        NotesHistory = NotesHistory + reviewcomments[i].Substring(0, reviewcomments[i].IndexOf(";")).Replace("[[[Test Reviewed: ", "") + Environment.NewLine;
                        notesattribute = notesattribute + reviewcomments[i].Substring(reviewcomments[i].IndexOf("[[[") + 3, reviewcomments[i].Length - reviewcomments[i].IndexOf("[[[") - 3);
                    }
                    else
                    {
                        NotesHistory = NotesHistory + reviewcomments[i];
                    }
                }

            }
            NotesHistory = NotesHistory.Replace("<br/>", "\n");

            //txtProvNoteshistory.Attributes.Add("InterpretationText", notesattribute.Replace("\n\n\n", "\n"));

            string sNotes = notesattribute.Replace("\n\n\n", "\n"); // sReviewNotes;

            if (sNotes != null && sNotes.Contains("Test Reviewed: ") == true)
            {
                string[] sTemplate = sNotes.Split(new string[] { "Test Reviewed: " }, StringSplitOptions.None);
                if (sTemplate.Count() > 1)
                {
                    for (int i = 0; i < sTemplate.Length; i++)
                    {
                        if (sTemplate[i].Trim() != string.Empty && sTemplate[i].Contains(';'))
                        {
                            string sInterpretationTitle = sTemplate[i].Split(';')[0];
                            string Notes = "Test Reviewed: " + sInterpretationTitle + ";" + sTemplate[i].Split(';')[1].Replace("\\n", "\n").Replace("\\t", "\t").Replace("\\r", "\r").Replace("\"", "");


                            //if (NotesHistory.Replace("\n\n\n", "\n").Contains('\n') == true && NotesHistory.Replace("\n\n\n", "\n").Split('\n').Length > (i - 1))
                            //{
                            //    if (NotesHistory.Replace("\n\n\n", "\n").Split('\n')[i - 1].Contains("(") == true && NotesHistory.Replace("\n\n\n", "\n").Split('\n')[i - 1].Contains(")") == true)
                            //    {
                            //        sSignDate = ExtractBetween(NotesHistory.Replace("\n\n\n", "\n").Split('\n')[i - 1], "(", ")");
                            //    }
                            //}

                            xmldoc1 = new XmlDocument();
                            strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
                            if (File.Exists(strXmlFilePath1) == true)
                            {
                                xmldoc1.Load(strXmlFilePath1);
                                if (NotesHistory.StartsWith("@") == true && NotesHistory.Contains("(") == true)
                                //if (sNotes.Contains("@") == true && sNotes.Contains("(") == true)
                                {
                                    //    string[] sPhyName = txtProvNoteshistory.Text.Split(new string[] { "\n" }, StringSplitOptions.None);
                                    string[] sPhyName = NotesHistory.Split(new string[] { "\n" }, StringSplitOptions.None);
                                    for (int j = 0; j < sPhyName.Length; j++)
                                    {
                                        if (sPhyName[j].Contains(sInterpretationTitle) == true)
                                        {
                                            XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/UserList/User[@User_Name='" + sPhyName[j].Substring(1, sPhyName[j].IndexOf(sInterpretationTitle) - 25) + "']"); // + ClientSession.CurrentPhysicianId);
                                            if (nodeMatchingPhysicianAddress != null)
                                            {
                                                sPhysicianName = nodeMatchingPhysicianAddress.Attributes["person_name"].Value.ToString();
                                                // sSignDate = sPhyName[j].Substring(sPhyName[j].IndexOf(sInterpretationTitle) - 23, sPhyName[j].IndexOf(sInterpretationTitle) - 12);
                                                //sSignDate = ExtractBetween(sPhyName[j], "(", ")");
                                            }
                                        }
                                    }
                                }
                            }

                            //string sFaclilityName = ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper(); //ClientSession.FacilityName 

                            if (sOutput == string.Empty)
                            {
                                sOutput = print.PrintInterpretationNotes(Notes, sPhysicianName, sFaclilityName, sPatientInfo, ClientSession.HumanId.ToString(), TargetFileDirectory, sInterpretationTitle, sSignDate, sPhysicianSignName);
                            }
                            else
                            {
                                sOutput += "|" + print.PrintInterpretationNotes(Notes, sPhysicianName, sFaclilityName, sPatientInfo, ClientSession.HumanId.ToString(), TargetFileDirectory, sInterpretationTitle, sSignDate, sPhysicianSignName);
                            }
                        }
                    }
                }
            }


            string[] sPath = sOutput.Split('|');
            string sPrintPathName = string.Empty;
            string shdnFileName = string.Empty;

            for (int iCount = 0; iCount < sPath.Length; iCount++)
            {
                sPrintPathName = sPath[iCount];
                string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                string[] FileNameTemp = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                //if (hdnFileName.Value == string.Empty)
                //{
                if (FileNameTemp.Length > 0)
                {

                    if (shdnFileName == string.Empty)
                    {
                        shdnFileName = "Documents\\" + Session.SessionID.ToString() + "\\" + FileNameTemp[0].ToString();
                    }
                    else
                    {
                        shdnFileName += "|" + "Documents\\" + Session.SessionID.ToString() + "\\" + FileNameTemp[0].ToString();
                    }
                }
            }


            // }
            //string FaxSubject = sOutput.Split('|')[1];

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Open Print Receipt-Window", "OpenPrintRecipt_Window('" + FaxSubject + "');", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "PrintInterpretation", "PrintInterpretation();", true);


            PDFLOAD.Attributes.Add("src", "frmPrintPDF.aspx?SI=" + shdnFileName + "&Location=DYNAMIC&FaxSubject=''");

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

    }
}
