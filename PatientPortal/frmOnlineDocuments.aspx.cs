/****************************************************************************************************************
 *  Form Name : frmOnlineDocuments | Module Name : Scanning And Indexing | User Role : Medical Assistants , Front-Office
 * 
 * Change History :
 * --> Changed Scanned Source from Clinical-NAS in to WebServer as primary source and Clinical NAS as secondary Source
 * --> Removed Copying the files temporarily to atala-capture-download, Instead Opened the files from source 
 * --> Replaced the atalasoft controls with the native controls availble in the web technology (image,div)
 * --> Removed creating thumbnails for files available, instead showing the file names
 * --> Added Network Path Exception Handling With friendly alert message , incase of network path not accessible
 * --> Removed Clinical NAS Ping on Find Documents Click
 * --> Included tip for the user
 * --> Moved the Button Index Event to Client side to the target Page in a Separate Window resides in the C5P0.Master
 * *****************************************************************************************************************/


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
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Telerik.Web.UI;
using Acurus.Capella.Core.DTO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Xml;
using iTextSharp.text.pdf;
using System.Web.Services;
using Newtonsoft.Json;
using System.Threading;
using Acurus.Capella.Core.DTOJson;


namespace Acurus.Capella.PatientPortal
{
    public partial class frmOnlineDocuments : System.Web.UI.Page
    {

        #region "Declaration & Instantiation"
        string filePath = string.Empty;
        IList<string> lstDocuments = new List<string>();
        IList<Scan> lstScanList = new List<Scan>();
        ScanManager scanProxy = new ScanManager();
        string Offset;
        string simagePathname = string.Empty;
        string source = string.Empty;
        string file_path = string.Empty;
        string _fileName = string.Empty;
        IList<String> lstfile_name = new List<string>();
        DateTime selectedDate = DateTime.MinValue;
        IList<string> ilstselectedDate = new List<string>();
        IList<StaticLookup> docSublist = new List<StaticLookup>();
        int pageCount = 0;
        #endregion

        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["IS_MovedToNextProcess"] = "false";
                Session["Is_Web_Portal"] = false;
                if (Request.QueryString["Screen"] != null && Request.QueryString["Screen"] == "PatientPortalOnlineDoumnets")
                {
                    Session["Is_Web_Portal"] = true;
                    DocumentType.Style.Add("display", "block");
                    SubDocumentType.Style.Add("display", "block");
                    cboDocumentType.Style.Add("display", "block");
                    cboDocumentSubType.Style.Add("display", "block");
                    pnlScanDoc.Style.Add("display", "none");
                    btnSaveOnline.Visible = true;
                    btnIndex.Style.Add("display", "none");
                    //  AllFilesValidator.Style.Add("display", "none");
                }
                //else
                //{
                //    uplValidator.Style.Add("display", "none");
                //}
                #region "Commented Bolcks For Reference"

                #region "Exam Photos"
                //if (Request.QueryString["Screen"] != null && Request.QueryString["Screen"] == "ExamPhotos")
                //{
                //    IList<string> ExamImport = new List<string>();
                //    ExamImport = (IList<string>)Session["ExamData"];
                //    IList<FileManagementIndex> lstFileIndex = new List<FileManagementIndex>();
                //    FileManagementIndexManager _fileIndexMngr = new FileManagementIndexManager();
                //    lstFileIndex = _fileIndexMngr.GetIndexedListByHumanId(ClientSession.HumanId, "EXAM");


                //    IList<FileManagementIndex> lstgrouptype = (from doc in lstFileIndex
                //                                               where (doc.Document_Type == ExamImport[2].ToString()) && (doc.Document_Date.ToString("dd-MMM-yyyy") == UtilityManager.ConvertToUniversal(Convert.ToDateTime(ExamImport[3])).ToString("dd-MMM-yyyy"))
                //                                               select doc).ToList<FileManagementIndex>();
                //    string grouplocalPath = localPath + "\\" + ExamImport[1].ToString() + "\\" + (Convert.ToDateTime(ExamImport[3])).ToString("dd-MMM-yyyy");
                //    DirectoryInfo dir = new DirectoryInfo(grouplocalPath);
                //    if (!dir.Exists)
                //    {
                //        dir.Create();
                //    }
                //    DirectoryInfo virdir = new DirectoryInfo(Server.MapPath("atala-capture-download//" + Session.SessionID + "//Exam_Photos"));
                //    if (!virdir.Exists)
                //    {
                //        virdir.Create();
                //    }
                //    FileInfo[] file = virdir.GetFiles();
                //    for (int i = 0; i < file.Length; i++)
                //    {
                //        File.Delete(file[i].FullName);
                //    }
                //    string[] files = Directory.GetFiles(grouplocalPath);
                //    FTPImageProcess ftpImage = new FTPImageProcess();
                //    hdnfilename.Value = string.Empty;
                //    string[] DisplayFiles = new string[lstgrouptype.Count];
                //    for (int i = 0; i < lstgrouptype.Count; i++)
                //    {
                //        if (files.Length > 0)
                //        {
                //            if (!files.Contains(grouplocalPath + "\\" + Path.GetFileName(lstgrouptype[i].File_Path)))
                //            {
                //                ftpImage.DownloadFromImageServer(ExamImport[0].ToString(), ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(lstgrouptype[i].File_Path), grouplocalPath);
                //            }
                //        }
                //        else
                //        {
                //            ftpImage.DownloadFromImageServer(ExamImport[0].ToString(), ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(lstgrouptype[i].File_Path), grouplocalPath);
                //        }
                //        string orig_image = grouplocalPath + "\\" + Path.GetFileName(lstgrouptype[i].File_Path);
                //        // string filename = "atala-capture-download/" + Session.SessionID + "/Exam_Photos/" + (i + 1).ToString() + ".tif";
                //        string filename = "atala-capture-download/" + Session.SessionID + "/Exam_Photos/" + Path.GetFileName(orig_image);
                //        string filepath = Page.MapPath(filename);
                //        File.Copy(orig_image, filepath, true);
                //        File.SetAttributes(filepath, FileAttributes.Normal);
                //        if (hdnfilename.Value == "")
                //            hdnfilename.Value = filename;
                //        else
                //            hdnfilename.Value = hdnfilename.Value + "%" + filename;
                //        DisplayFiles[i] = Path.GetFileName(filename);
                //    }
                //    //WebThumbnailViewer2.OpenUrl("atala-capture-download/" + Session.SessionID + "/Exam_Photos", "*.tif");
                //    WebThumbnailViewer2.OpenUrl("atala-capture-download/" + Session.SessionID + "/Exam_Photos", DisplayFiles);
                //    WebThumbnailViewer2.SelectedIndex = 0;
                //    Session.Remove("ExamData");
                //    if (!DisplayFiles[0].Contains(".dcm"))
                //    {
                //        WebThumbnailViewer1.OpenUrl("atala-capture-download/" + Session.SessionID + "/Exam_Photos/" + DisplayFiles[0]);
                //        WebThumbnailViewer1.SelectedIndex = 0;
                //        tblOnlineDocuments.Rows[1].Cells[0].Width = "20%";
                //        tblOnlineDocuments.Rows[1].Cells[1].Width = "79%";
                //        tvMetadata.Visible = false;
                //        tvMetadata.Width = 1;
                //        tblOnlineDocuments.Rows[1].Cells[2].Width = "1%";

                //    }
                //    else
                //    {
                //        WebThumbnailViewer1.OpenUrl("atala-capture-download/" + Session.SessionID + "/Exam_Photos/" + DisplayFiles[0]);
                //        WebThumbnailViewer1.SelectedIndex = 0;
                //        DicomViewer(Page.MapPath("atala-capture-download/" + Session.SessionID + "/Exam_Photos/" + DisplayFiles[0]));
                //    }
                //    finddocuments.Visible = false;
                //}
                #endregion

                #region "CMG Result"
                //else if (Request.QueryString["Screen"] != null && Request.QueryString["Screen"] == "CmgResult")
                //{
                //    FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
                //    filelist = objfileproxy.GetImagesforAnnotations(ClientSession.HumanId, Convert.ToUInt64(Request["OrderSubmitId"].ToString()), "ORDER");
                //    IList<FileManagementIndex> lstfile = new List<FileManagementIndex>();
                //    lstfile = (from doc in filelist
                //               where doc.Result_Master_ID == ClientSession.EncounterId && doc.Source == "ORDER" && doc.File_Path != string.Empty
                //               select doc).ToList<FileManagementIndex>();
                //    string grouplocalPath = localPath + "\\Cmg_Result\\" + ClientSession.HumanId.ToString() + "\\" + ClientSession.EncounterId.ToString();
                //    DirectoryInfo dir = new DirectoryInfo(grouplocalPath);
                //    if (!dir.Exists)
                //    {
                //        dir.Create();
                //    }
                //    DirectoryInfo virdir = new DirectoryInfo(Server.MapPath("atala-capture-download//" + Session.SessionID + "//Cmg_Result"));
                //    if (!virdir.Exists)
                //    {
                //        virdir.Create();
                //    }
                //    FileInfo[] file = virdir.GetFiles();
                //    for (int i = 0; i < file.Length; i++)
                //    {
                //        File.Delete(file[i].FullName);
                //    }
                //    string[] files = Directory.GetFiles(grouplocalPath);
                //    FTPImageProcess ftpImage = new FTPImageProcess();
                //    hdnfilename.Value = string.Empty;
                //    for (int i = 0; i < lstfile.Count; i++)
                //    {
                //        if (files.Length > 0)
                //        {
                //            if (!files.Contains(grouplocalPath + "\\" + Path.GetFileName(lstfile[i].File_Path)))
                //            {
                //                ftpImage.DownloadFromImageServer(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(lstfile[i].File_Path), grouplocalPath);
                //            }
                //        }
                //        else
                //        {
                //            ftpImage.DownloadFromImageServer(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(lstfile[i].File_Path), grouplocalPath);
                //        }
                //        string orig_image = grouplocalPath + "\\" + Path.GetFileName(lstfile[i].File_Path);
                //        string filename = "atala-capture-download/" + Session.SessionID + "/Cmg_Result/" + (i + 1).ToString() + ".tif";
                //        string filepath = Page.MapPath(filename);
                //        File.Copy(orig_image, filepath, true);
                //        File.SetAttributes(filepath, FileAttributes.Normal);
                //        if (hdnfilename.Value == "")
                //            hdnfilename.Value = filename;
                //        else
                //            hdnfilename.Value = hdnfilename.Value + "%" + filename;
                //    }
                //    WebThumbnailViewer2.OpenUrl("atala-capture-download/" + Session.SessionID + "/Cmg_Result", "*.tif");
                //    WebThumbnailViewer2.SelectedIndex = 0;
                //    WebThumbnailViewer1.OpenUrl("atala-capture-download/" + Session.SessionID + "/Cmg_Result/1.tif");
                //    WebThumbnailViewer1.SelectedIndex = 0;
                //    WebAnnotationViewer1.Centered = true;
                //    finddocuments.Visible = false;



                //}

                #endregion

                #region "Inhouse Orders"
                //else if ((Request.QueryString["Screen"] != null && Request.QueryString["Screen"] == "ResultView") || (Request.QueryString["Screen"] != null && Request.QueryString["Screen"] == "ResultViewOrder")) 
                //{
                //    btnClose.Visible = false;
                //    finddocuments.Visible = false;
                //    FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
                //    IList<FileManagementIndex> lstfile = new List<FileManagementIndex>();
                //    lstfile = objfileproxy.GetImagesforAnnotations(ClientSession.HumanId, Convert.ToUInt64(Request["OrderSubmitId"].ToString()), "ORDER");
                //    string grouplocalPath = localPath + "\\Result_Files\\" + ClientSession.HumanId.ToString() + "\\" + ClientSession.EncounterId.ToString();
                //    DirectoryInfo dir = new DirectoryInfo(grouplocalPath);
                //    if (!dir.Exists)
                //    {
                //        dir.Create();
                //    }
                //    DirectoryInfo virdir = new DirectoryInfo(Server.MapPath("atala-capture-download//" + Session.SessionID + "//Result_Files"));
                //    if (!virdir.Exists)
                //    {
                //        virdir.Create();
                //    }
                //    FileInfo[] file = virdir.GetFiles();
                //    for (int i = 0; i < file.Length; i++)
                //    {
                //        File.Delete(file[i].FullName);
                //    }
                //    string[] files = Directory.GetFiles(grouplocalPath);
                //    FTPImageProcess ftpImage = new FTPImageProcess();
                //    hdnfilename.Value = string.Empty;
                //    for (int i = 0; i < lstfile.Count; i++)
                //    {
                //        if (files.Length > 0)
                //        {
                //            if (!files.Contains(grouplocalPath + "\\" + Path.GetFileName(lstfile[i].File_Path)))
                //            {
                //                ftpImage.DownloadFromImageServer(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(lstfile[i].File_Path), grouplocalPath);
                //            }
                //        }
                //        else
                //        {
                //            ftpImage.DownloadFromImageServer(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(lstfile[i].File_Path), grouplocalPath);
                //        }
                //        string orig_image = grouplocalPath + "\\" + Path.GetFileName(lstfile[i].File_Path);
                //        string filename = "atala-capture-download/" + Session.SessionID + "/Result_Files/" + (i + 1).ToString() + ".tif";
                //        string filepath = Page.MapPath(filename);
                //        File.Copy(orig_image, filepath, true);
                //        File.SetAttributes(filepath, FileAttributes.Normal);

                //        if (hdnfilename.Value == "")
                //            hdnfilename.Value = filename;
                //        else
                //            hdnfilename.Value = hdnfilename.Value + "%" + filename;
                //    }

                //    WebThumbnailViewer2.OpenUrl("atala-capture-download/" + Session.SessionID + "/Result_Files", "*.tif");
                //    WebThumbnailViewer2.SelectedIndex = 0;
                //    WebThumbnailViewer1.OpenUrl("atala-capture-download/" + Session.SessionID + "/Result_Files/1.tif");
                //    WebThumbnailViewer1.SelectedIndex = 0;
                //    WebAnnotationViewer1.Centered = true;
                //    finddocuments.Visible = false;
                //    btnIndex.Visible = false;
                //    UploadImage.Visible = false;
                //}
                #endregion

                #region "View Result"
                //else if ((Request.QueryString["Screen"] != null && Request.QueryString["Screen"] == "RESULT"))
                //{
                //    btnClose.Visible = false;
                //    finddocuments.Visible = false;
                //    FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
                //    IList<FileManagementIndex> lstfile = new List<FileManagementIndex>();
                //    lstfile = objfileproxy.GetImagesforAnnotations(Convert.ToUInt64(Request.QueryString["Human"]), Convert.ToUInt64(Request["OrderSubmitId"].ToString()), "SCAN");
                //    string grouplocalPath = localPath + "\\Result_Files\\" + ClientSession.HumanId.ToString();
                //    DirectoryInfo dir = new DirectoryInfo(grouplocalPath);
                //    if (!dir.Exists)
                //    {
                //        dir.Create();
                //    }
                //    DirectoryInfo virdir = new DirectoryInfo(Server.MapPath("atala-capture-download//" + Session.SessionID + "//Result_Files"));
                //    if (!virdir.Exists)
                //    {
                //        virdir.Create();
                //    }
                //    FileInfo[] file = virdir.GetFiles();
                //    for (int i = 0; i < file.Length; i++)
                //    {
                //        File.Delete(file[i].FullName);
                //    }
                //    string[] files = Directory.GetFiles(grouplocalPath);
                //    FTPImageProcess ftpImage = new FTPImageProcess();
                //    hdnfilename.Value = string.Empty;
                //    for (int i = 0; i < lstfile.Count; i++)
                //    {
                //        if (files.Length > 0)
                //        {
                //            if (!files.Contains(grouplocalPath + "\\" + Path.GetFileName(lstfile[i].File_Path)))
                //            {
                //                ftpImage.DownloadFromImageServer(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(lstfile[i].File_Path), grouplocalPath);
                //            }
                //        }
                //        else
                //        {
                //            ftpImage.DownloadFromImageServer(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(lstfile[i].File_Path), grouplocalPath);
                //        }
                //        string orig_image = grouplocalPath + "\\" + Path.GetFileName(lstfile[i].File_Path);
                //        string filename = "atala-capture-download/" + Session.SessionID + "/Result_Files/" + (i + 1).ToString() + ".tif";
                //        string filepath = Page.MapPath(filename);
                //        File.Copy(orig_image, filepath, true);
                //        File.SetAttributes(filepath, FileAttributes.Normal);

                //        if (hdnfilename.Value == "")
                //            hdnfilename.Value = filename;
                //        else
                //            hdnfilename.Value = hdnfilename.Value + "%" + filename;
                //    }
                //    /* Handled The Deletion Of File Downloaded IN the LocalDisk Of Server  */
                //    try
                //    {
                //        dir.Delete(true);
                //    }
                //    catch { /* Do Nothing If Directory Cannot Be Deleted, Same Will be Handled By Scheduled Task Running On Server */ }
                //    WebThumbnailViewer2.OpenUrl("atala-capture-download/" + Session.SessionID + "/Result_Files", "*.tif");
                //    WebThumbnailViewer2.SelectedIndex = 0;
                //    WebThumbnailViewer1.OpenUrl("atala-capture-download/" + Session.SessionID + "/Result_Files/1.tif");
                //    WebThumbnailViewer1.SelectedIndex = 0;
                //    WebAnnotationViewer1.Height = Unit.Pixel(500);
                //    WebAnnotationViewer1.Width = Unit.Pixel(500);
                //    WebAnnotationViewer1.Centered = true;
                //    finddocuments.Visible = false;
                //    btnIndex.Visible = false;
                //    UploadImage.Visible = false;
                //}
                #endregion

                #endregion

                #region "Online Documents"
                {
                    Offset = Request.Cookies["Tz"].Value;
                    if (Offset.StartsWith("-"))
                    {
                        DateTime localTime = DateTime.Now.ToUniversalTime().Subtract(new TimeSpan(0, Convert.ToInt32(Offset), 0));
                        dtpScannedDate.Value = localTime.ToString("d-MMM-yyyy");
                    }
                    else
                    {
                        DateTime localTime = DateTime.Now.AddMinutes(Convert.ToDouble(Offset));
                        dtpScannedDate.Value = localTime.ToString("d-MMM-yyyy");
                    }

                    txtSelectedFacility.Value = ClientSession.FacilityName;
                    btnIndex.Disabled = true;
                    this.Title = "Upload Scanned Documents";
                }
                #endregion
                if (Session["Is_Web_Portal"] != null && Session["Is_Web_Portal"].ToString().ToLower() == "true")
                    LoadDocType();
            }
        }

        protected void btnFindDocuments_Click(object sender, EventArgs e)
        {
            if (chckShowOldFiles.Checked)
            {
                filePath = System.Configuration.ConfigurationManager.AppSettings[ClientSession.FacilityName.Replace(" ", "_") + "_ScanningPath"];
                if (filePath == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ConfigurationErros", "DisplayErrorMessage('172503');", true);
                }
                Session["FilePath"] = filePath;
            }
            else
            {
                filePath = System.Configuration.ConfigurationManager.AppSettings["ScanningPath_Local"];
                if (filePath == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ConfigurationErros", "DisplayErrorMessage('172503');", true);
                }
                Session["FilePath"] = filePath;
            }

            DirectoryInfo onlineChartdirInfo = new DirectoryInfo(filePath + "\\" + ClientSession.FacilityName + "\\Scanned_Images");
            // Session["SelectedDateList"] = null;
            if (onlineChartdirInfo.Exists)
            {

                FileInfo[] onlineChartfileInfo = null;
                selectedDate = Convert.ToDateTime(dtpScannedDate.Value);
                onlineChartdirInfo = new DirectoryInfo(filePath + "\\" + ClientSession.FacilityName + "\\Scanned_Images\\" + selectedDate.ToString("yyyyMMdd"));
                //if (Session["SelectedDateList"] != null)
                //{
                //    ilstselectedDate = ((IList<string>)HttpContext.Current.Session["SelectedDateList"]).Distinct().ToList();
                //}
                //ilstselectedDate.Add(selectedDate.ToString("yyyy-MM-dd"));
                // List<string> distinctselectedDate = ilstselectedDate.Distinct().ToList();
                //Session["SelectedDateList"] = distinctselectedDate;
                //string[] sFileExtension = { "*.tif", "*.jpeg", "*.png", "*.jpg", "*.bmp", "*.pdf" };
                //if (distinctselectedDate != null && distinctselectedDate.Count > 0)
                //{
                //    foreach (string strDate in distinctselectedDate)
                //    {
                //foreach (string strExension in sFileExtension)
                //{
                //onlineChartfileInfo = onlineChartdirInfo.GetFiles("*ONLINE_*" + strDate + strExension);
                //onlineChartfileInfo = onlineChartdirInfo.GetFiles("*ONLINE_*" + selectedDate.ToString("yyyy-MM-dd") + "*");
                if (!onlineChartdirInfo.Exists)
                {
                    btnIndex.Disabled = true;
                    if (Session["IS_MovedToNextProcess"] != null && Session["IS_MovedToNextProcess"].ToString() == "true")
                        Session["IS_MovedToNextProcess"] = "false";
                    else
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NoScannedFilesMsg", "DisplayErrorMessage('114002');", true);
                    return;
                }
                onlineChartfileInfo = onlineChartdirInfo.GetFiles();
                if (onlineChartfileInfo.Count() > 0)
                {
                    string[] sFileExtension = { ".tif", ".jpeg", ".png", ".jpg", ".bmp", ".pdf" };
                    // IList<Scan> lstDocumentsScan = new List<Scan>();

                    foreach (FileInfo fi in onlineChartfileInfo)
                    {
                        string sExtension = Path.GetExtension(fi.FullName).ToLower();
                        if (sFileExtension.Contains(sExtension))
                        {
                            lstDocuments.Add(fi.ToString());
                            //Scan objscan = new Scan();
                            //objscan.Scan_ID = 0;
                            //objscan.Scanned_File_Name = fi.ToString();
                            //lstDocumentsScan.Add(objscan);
                        }
                    }
                    //}
                    //}
                    //}

                    if (lstDocuments.Count > 0)
                    {
                        lstScanList = scanProxy.GetOnlineDocumentsList(ClientSession.FacilityName, lstDocuments.ToArray(), selectedDate);
                        Session["LoadList"] = lstScanList;
                    }
                    // lstScanList = lstDocumentsScan;
                    int fileIteration = 0;
                    if (lstScanList.Count > 0 || (Session["BrowseLoadList"] != null && ((IList<Scan>)HttpContext.Current.Session["BrowseLoadList"]).Count > 0))
                    {
                        foreach (Scan scannedRecord in lstScanList)
                        {
                            int iTryCount = 0;
                        retry:
                            FileInfo item = new FileInfo(Path.Combine(onlineChartdirInfo.FullName, scannedRecord.Scanned_File_Name));
                            if (item.Exists == true && item.Length > 0)//To check the File size
                            {
                                iTryCount = 0;
                                fileIteration++;
                                pageCount = 0;
                                HtmlGenericControl gnDiv = new HtmlGenericControl("div");
                                gnDiv.ID = "file_" + fileIteration.ToString();
                                gnDiv.Style.Add("height", "auto");
                                gnDiv.Style.Add("width", "450px");
                                gnDiv.Style.Add("margin-bottom", "4px");
                                gnDiv.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                                if (Path.GetExtension(item.FullName).ToUpper() != ".PDF")
                                {
                                    using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(item.FullName))
                                    {
                                        pageCount = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                                        imgbg.Dispose();
                                    }
                                    gnDiv.Attributes.Add("onclick", "choosefiles('file_" + fileIteration.ToString() + "','" + HttpUtility.UrlEncode(item.FullName) + "','" + pageCount + "');");
                                }
                                else
                                {
                                    string sDirPath = Server.MapPath("~/atala-capture-upload/Indexing_Files/");
                                    DirectoryInfo dir = new DirectoryInfo(sDirPath);
                                    if (!dir.Exists)
                                    {
                                        dir.Create();
                                    }
                                    FileInfo[] dirfiles = dir.GetFiles();
                                    string SelectedFilePath = sDirPath + "/" + item.Name;
                                    if (dirfiles.Length > 0)
                                    {
                                        if (File.Exists(SelectedFilePath) == true)
                                        {
                                            IList<Scan> ilstScan = new List<Scan>();
                                            ilstScan = ((IList<Scan>)HttpContext.Current.Session["LoadList"]);
                                            if (ilstScan != null && ilstScan.Count > 0)
                                            {
                                                bool CheckHasFile = ilstScan.Any(cus => cus.Scanned_File_Name == item.Name);
                                                if (CheckHasFile == false)
                                                {
                                                    for (int j = 0; j < dirfiles.Length; j++)
                                                    {
                                                        if (item.Name == dirfiles[j].Name)
                                                        {
                                                            File.Delete(dirfiles[j].FullName);
                                                        }
                                                    }
                                                    File.Copy(item.FullName, SelectedFilePath, true);
                                                    // currentFile.SaveAs(SelectedFilePath);
                                                }
                                                else
                                                {
                                                    //already file Exists
                                                }
                                            }
                                        }
                                        else
                                        {
                                            File.Copy(item.FullName, SelectedFilePath, true);
                                        }
                                    }
                                    else
                                    {
                                        File.Copy(item.FullName, SelectedFilePath, true);
                                    }
                                    using (FileStream fs = new FileStream(item.FullName, FileMode.Open, FileAccess.Read))
                                    {
                                        StreamReader sr = new StreamReader(fs);
                                        string pdf = sr.ReadToEnd();
                                        Regex rx = new Regex(@"/Type\s*/Page[^s]");
                                        MatchCollection match = rx.Matches(pdf);
                                        pageCount = match.Count;
                                        if (pageCount == 0)
                                        {
                                            PdfReader pdfReader = new PdfReader(item.FullName);
                                            pageCount = pdfReader.NumberOfPages;

                                        }
                                        sr.Dispose();
                                        fs.Dispose();
                                    }

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
                                        sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-upload//Indexing_Files//";
                                    }
                                    else
                                    {
                                        sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-upload//Indexing_Files//";
                                    }
                                    //string strFilePath = Path.Combine(sDirPath, item.Name).Replace("\\", "\\\\");
                                    string strFilePath = Path.Combine(sImgPath, item.Name).Replace("\\", "\\\\");
                                    string strLocalpath = "\\\\atala-capture-upload\\\\Indexing_Files\\\\" + item.Name;
                                    gnDiv.Attributes.Add("onclick", "choosePDFfiles('file_" + fileIteration.ToString() + "','" + strFilePath + "','" + pageCount + "','" + strLocalpath + "');");
                                }
                                gnDiv.InnerText = item.Name;
                                fileThumbs.Controls.Add(gnDiv);
                            }
                            else if (item.Exists == false)
                            {
                                if (iTryCount < 3)
                                {
                                    iTryCount++;
                                    Thread.Sleep(2000);
                                    goto retry;
                                }
                            }
                        }

                        if (Session["BrowseLoadList"] != null && ((IList<Scan>)HttpContext.Current.Session["BrowseLoadList"]).Count > 0)
                        {
                            IList<Scan> ilstBrowseScan = ((IList<Scan>)HttpContext.Current.Session["BrowseLoadList"]);
                            foreach (Scan scannedRecord in ilstBrowseScan)
                            {
                                DirectoryInfo dirOtherFileInfo = new DirectoryInfo(Server.MapPath("~/atala-capture-upload/Indexing_Files/"));
                                int iTryCount = 0;
                            retry:
                                FileInfo item = new FileInfo(Path.Combine(dirOtherFileInfo.FullName, scannedRecord.Scanned_File_Name));
                                if (item.Exists == true && item.Length > 0)//To check the File size
                                {
                                    iTryCount = 0;
                                    fileIteration++;
                                    int pageCount = 0;

                                    HtmlGenericControl gnDiv = new HtmlGenericControl("div");
                                    gnDiv.ID = "file_" + fileIteration.ToString();
                                    gnDiv.Style.Add("height", "auto");
                                    gnDiv.Style.Add("width", "450px");
                                    gnDiv.Style.Add("margin-bottom", "4px");
                                    gnDiv.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                                    if (Path.GetExtension(item.FullName).ToUpper() != ".PDF")
                                    {
                                        using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(item.FullName))
                                        {
                                            pageCount = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                                            imgbg.Dispose();
                                        }
                                        gnDiv.Attributes.Add("onclick", "choosefiles('file_" + fileIteration.ToString() + "','" + HttpUtility.UrlEncode(item.FullName) + "','" + pageCount + "');");
                                    }
                                    else
                                    {
                                        using (FileStream fs = new FileStream(item.FullName, FileMode.Open, FileAccess.Read))
                                        {
                                            StreamReader sr = new StreamReader(fs);
                                            string pdf = sr.ReadToEnd();
                                            Regex rx = new Regex(@"/Type\s*/Page[^s]");
                                            MatchCollection match = rx.Matches(pdf);
                                            pageCount = match.Count;
                                            if (pageCount == 0)
                                            {

                                                PdfReader pdfReader = new PdfReader(item.FullName);
                                                pageCount = pdfReader.NumberOfPages;

                                            }
                                            sr.Dispose();
                                            fs.Dispose();
                                        }
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
                                            sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-upload//Indexing_Files//";
                                        }
                                        else
                                        {
                                            sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-upload//Indexing_Files//";
                                        }
                                        // string strFilePath = "\\\\atala-capture-upload\\\\Indexing_Files\\\\" + item.Name;
                                        string strLocalpath = "\\\\atala-capture-upload\\\\Indexing_Files\\\\" + item.Name;
                                        string strFilePath = Path.Combine(sImgPath, item.Name).Replace("\\", "\\\\");

                                        gnDiv.Attributes.Add("onclick", "choosePDFfiles('file_" + fileIteration.ToString() + "','" + strFilePath + "','" + pageCount + "','" + strLocalpath + "');");
                                    }
                                    gnDiv.InnerText = item.Name;
                                    fileThumbs.Controls.Add(gnDiv);
                                }
                                else if (item.Exists == false)
                                {
                                    if (iTryCount < 3)
                                    {
                                        iTryCount++;
                                        Thread.Sleep(2000);
                                        goto retry;
                                    }
                                }
                            }
                        }

                        hdnFiles.Value = string.Empty;
                    }
                    else
                    {
                        btnIndex.Disabled = true;
                        if (Session["IS_MovedToNextProcess"] != null && Session["IS_MovedToNextProcess"].ToString() == "true")
                            Session["IS_MovedToNextProcess"] = "false";
                        else
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NoScannedFilesMsg", "DisplayErrorMessage('114002');", true);
                        return;
                    }
                }
                else
                {
                    btnIndex.Disabled = true;
                    if (Session["IS_MovedToNextProcess"] != null && Session["IS_MovedToNextProcess"].ToString() == "true")
                        Session["IS_MovedToNextProcess"] = "false";
                    else
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NoScannedFilesMsg", "DisplayErrorMessage('114002');", true);
                    return;
                }

            }
            else
            {
                btnIndex.Disabled = true;
                /* Directory Not found Exception */
                if (Session["IS_MovedToNextProcess"] != null && Session["IS_MovedToNextProcess"].ToString() == "true")
                    Session["IS_MovedToNextProcess"] = "false";
                else
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "scannedFolderException", "DisplayErrorMessage('114001');", true);
                return;
            }
            divLoading.Style.Add("display", "none");
        }

        #endregion

        protected void btnUpload_ServerClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "StartLoadingImage", "StartLoadOnUploadFile();", true);
            hdnSelecteFile.Value = string.Empty;
            try
            {
                int fileIteration = 0;
                if (fileupload.HasFile)
                {
                    Session["BrowseLoadList"] = null;
                    Session["LoadList"] = null;
                    // Session["SelectedDateList"] = null;
                    Session["BrowseFileNames"] = null;
                    //if (Request.QueryString["Screen"] != null && Request.QueryString["Screen"] == "PatientPortalOnlineDoumnets")//For patient portal
                    //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Clearall", "clickClearAll();", true);
                    try
                    {
                        string sDirPath = Server.MapPath("~/atala-capture-upload/Indexing_Files/");
                        DirectoryInfo dir = new DirectoryInfo(sDirPath);
                        if (!dir.Exists)
                        {
                            dir.Create();
                        }
                        FileInfo[] dirfiles = dir.GetFiles();
                        HttpFileCollection uploadedFiles = Request.Files;
                        IList<string> ilstFileName = new List<string>();
                        if (uploadedFiles.Count > 0)
                        {
                            for (int fileCount = 0; fileCount < uploadedFiles.Count; fileCount++)
                            {
                                HttpPostedFile currentFile = uploadedFiles[fileCount];
                                string fileName = Path.GetFileName(currentFile.FileName);
                                ilstFileName.Add(fileName);
                            }
                        }
                        var ErrorFile = ilstFileName.Where(aa => aa.ToString().ToUpper().Contains(".PDF")).Select(aa => aa.ToString());
                        if (ErrorFile.Count() > 0)
                        {
                            var bErrorCheck = ErrorFile.Where(aa => aa.Contains("#"));
                            if (bErrorCheck.Count() > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "StopLoadingImage", "StopLoadOnErrorUploadFile();", true);
                                return;
                            }
                        }


                        if (Session["BrowseFileNames"] != null)
                        {
                            lstDocuments = ((IList<string>)HttpContext.Current.Session["BrowseFileNames"]).Distinct().ToList();
                        }
                        if (uploadedFiles.Count > 0)
                        {
                            for (int fileCount = 0; fileCount < uploadedFiles.Count; fileCount++)
                            {
                                HttpPostedFile currentFile = uploadedFiles[fileCount];
                                string fileName = Path.GetFileName(currentFile.FileName);
                                ilstFileName.Add(fileName);
                                if (currentFile.ContentLength > 0)
                                {
                                    lstDocuments.Add(fileName);
                                    string SelectedFilePath = sDirPath + "/" + fileName;
                                    hdnfilepath.Value = SelectedFilePath;
                                    if (dirfiles.Length > 0)
                                    {
                                        if (File.Exists(SelectedFilePath) == true)
                                        {
                                            IList<Scan> ilstScan = new List<Scan>();
                                            ilstScan = ((IList<Scan>)HttpContext.Current.Session["LoadList"]);
                                            if (Session["LoadList"] != null)
                                            {
                                                bool CheckHasFile = ilstScan.Any(cus => cus.Scanned_File_Name == fileName);
                                                if (CheckHasFile == false)
                                                {
                                                    for (int j = 0; j < dirfiles.Length; j++)
                                                    {
                                                        if (fileName == dirfiles[j].Name)
                                                        {
                                                            File.Delete(dirfiles[j].FullName);
                                                        }
                                                    }
                                                    currentFile.SaveAs(SelectedFilePath);
                                                }
                                                else
                                                {
                                                    //already file Exists
                                                    File.Delete(SelectedFilePath);
                                                    currentFile.SaveAs(SelectedFilePath);
                                                }
                                            }
                                            else
                                            {
                                                //already file Exists
                                                File.Delete(SelectedFilePath);
                                                currentFile.SaveAs(SelectedFilePath);
                                            }
                                        }
                                        else
                                        {
                                            currentFile.SaveAs(SelectedFilePath);
                                        }
                                    }
                                    else
                                    {
                                        currentFile.SaveAs(SelectedFilePath);
                                    }
                                }
                            }
                        }

                        if (lstDocuments.Count > 0)
                        {
                            lstScanList = scanProxy.GetOnlineDocumentsList(ClientSession.FacilityName, lstDocuments.Distinct().ToArray(), selectedDate);
                            Session["BrowseLoadList"] = lstScanList;
                            Session["BrowseFileNames"] = lstDocuments.Distinct().ToList();
                        }

                        if (lstScanList.Count > 0)
                        {
                            foreach (Scan scannedRecord in lstScanList)
                            {
                                int iTryCount = 0;
                            retry:
                                FileInfo item = new FileInfo(Path.Combine(sDirPath, scannedRecord.Scanned_File_Name));
                                if (item.Exists == true)
                                {
                                    iTryCount = 0;
                                    fileIteration++;
                                    int pageCount = 0;
                                    HtmlGenericControl gnDiv = new HtmlGenericControl("div");
                                    gnDiv.ID = "file_" + fileIteration.ToString();
                                    gnDiv.Style.Add("height", "auto");
                                    gnDiv.Style.Add("width", "450px");
                                    gnDiv.Style.Add("margin-bottom", "4px");
                                    gnDiv.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                                    if (Path.GetExtension(item.FullName).ToUpper() != ".PDF")
                                    {
                                        using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(item.FullName))
                                        {
                                            pageCount = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                                            imgbg.Dispose();
                                        }
                                        gnDiv.Attributes.Add("onclick", "choosefiles('file_" + fileIteration.ToString() + "','" + HttpUtility.UrlEncode(item.FullName) + "','" + pageCount + "');");
                                        hdnSelecteFile.Value = "choosefiles('file_" + fileIteration.ToString() + "','" + HttpUtility.UrlEncode(item.FullName) + "','" + pageCount + "');";
                                    }
                                    else
                                    {
                                        using (FileStream fs = new FileStream(item.FullName, FileMode.Open, FileAccess.Read))
                                        {
                                            StreamReader sr = new StreamReader(fs);
                                            string pdf = sr.ReadToEnd();
                                            Regex rx = new Regex(@"/Type\s*/Page[^s]");
                                            MatchCollection match = rx.Matches(pdf);
                                            pageCount = match.Count;
                                            if (pageCount == 0)
                                            {

                                                PdfReader pdfReader = new PdfReader(item.FullName);
                                                pageCount = pdfReader.NumberOfPages;

                                            }

                                            sr.Dispose();
                                            fs.Dispose();
                                        }
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
                                            sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-upload//Indexing_Files//";
                                        }
                                        else
                                        {
                                            sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-upload//Indexing_Files//";
                                        }
                                        string strLocalpath = "\\\\atala-capture-upload\\\\Indexing_Files\\\\" + item.Name;
                                        string strFilePath = Path.Combine(sImgPath, item.Name).Replace("\\", "\\\\");
                                        gnDiv.Attributes.Add("onclick", "choosePDFfiles('file_" + fileIteration.ToString() + "','" + strFilePath + "','" + pageCount + "','" + strLocalpath + "');");
                                        hdnSelecteFile.Value = "choosePDFfiles('file_" + fileIteration.ToString() + "','" + strFilePath + "','" + pageCount + "','" + strLocalpath + "');";
                                    }
                                    gnDiv.InnerText = item.Name;
                                    fileThumbs.Controls.Add(gnDiv);
                                }
                                else if (item.Exists == false)
                                {
                                    if (iTryCount < 3)
                                    {
                                        iTryCount++;
                                        Thread.Sleep(2000);
                                        goto retry;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                        throw (ex);
                    }
                    //fileupload.Enabled = false;
                }
                else if (Session["BrowseLoadList"] != null)//This condition for webportal Login browse list
                {
                    string sDirPath = Server.MapPath("~/atala-capture-upload/Indexing_Files/");
                    lstScanList = ((IList<Scan>)HttpContext.Current.Session["BrowseLoadList"]);
                    if (lstScanList.Count > 0)
                    {
                        foreach (Scan scannedRecord in lstScanList)
                        {
                            int iTryCount = 0;
                        retry:
                            FileInfo item = new FileInfo(Path.Combine(sDirPath, scannedRecord.Scanned_File_Name));
                            if (item.Exists == true)
                            {
                                iTryCount = 0;
                                fileIteration++;
                                int pageCount = 0;
                                HtmlGenericControl gnDiv = new HtmlGenericControl("div");
                                gnDiv.ID = "file_" + fileIteration.ToString();
                                gnDiv.Style.Add("height", "auto");
                                gnDiv.Style.Add("width", "450px");
                                gnDiv.Style.Add("margin-bottom", "4px");
                                gnDiv.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                                if (Path.GetExtension(item.FullName).ToUpper() != ".PDF")
                                {
                                    using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(item.FullName))
                                    {
                                        pageCount = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                                        imgbg.Dispose();
                                    }
                                    gnDiv.Attributes.Add("onclick", "choosefiles('file_" + fileIteration.ToString() + "','" + HttpUtility.UrlEncode(item.FullName) + "','" + pageCount + "');");
                                    hdnSelecteFile.Value = "choosefiles('file_" + fileIteration.ToString() + "','" + HttpUtility.UrlEncode(item.FullName) + "','" + pageCount + "');";
                                }
                                else
                                {
                                    using (FileStream fs = new FileStream(item.FullName, FileMode.Open, FileAccess.Read))
                                    {
                                        StreamReader sr = new StreamReader(fs);
                                        string pdf = sr.ReadToEnd();
                                        Regex rx = new Regex(@"/Type\s*/Page[^s]");
                                        MatchCollection match = rx.Matches(pdf);
                                        pageCount = match.Count;
                                        if (pageCount == 0)
                                        {

                                            PdfReader pdfReader = new PdfReader(item.FullName);
                                            pageCount = pdfReader.NumberOfPages;

                                        }
                                        sr.Dispose();
                                        fs.Dispose();
                                    }
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
                                        sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-upload//Indexing_Files//";
                                    }
                                    else
                                    {
                                        sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-upload//Indexing_Files//";
                                    }
                                    string strLocalpath = "\\\\atala-capture-upload\\\\Indexing_Files\\\\" + item.Name;
                                    string strFilePath = Path.Combine(sImgPath, item.Name).Replace("\\", "\\\\");
                                    gnDiv.Attributes.Add("onclick", "choosePDFfiles('file_" + fileIteration.ToString() + "','" + strFilePath + "','" + pageCount + "','" + strLocalpath + "');");
                                    hdnSelecteFile.Value = "choosePDFfiles('file_" + fileIteration.ToString() + "','" + strFilePath + "','" + pageCount + "','" + strLocalpath + "');";
                                }
                                gnDiv.InnerText = item.Name;
                                fileThumbs.Controls.Add(gnDiv);
                            }
                            else if (item.Exists == false)
                            {
                                if (iTryCount < 3)
                                {
                                    iTryCount++;
                                    Thread.Sleep(2000);
                                    goto retry;
                                }
                            }
                        }
                    }
                }


                if (Session["LoadList"] != null)
                {
                    IList<Scan> lstFindDocument = ((IList<Scan>)HttpContext.Current.Session["LoadList"]);
                    if (lstFindDocument.Count > 0)
                    {
                        filePath = System.Configuration.ConfigurationManager.AppSettings[ClientSession.FacilityName.Replace(" ", "_") + "_ScanningPath"];
                        DirectoryInfo onlineChartdirInfo = new DirectoryInfo(Session["FilePath"] + "\\" + ClientSession.FacilityName + "\\Scanned_Images");
                        //DirectoryInfo onlineChartdirInfo = new DirectoryInfo(filePath + "\\" + ClientSession.FacilityName + "\\Scanned_Images");
                        foreach (Scan scannedRecord in lstFindDocument)
                        {
                            int iTryCount = 0;
                        retry:
                            FileInfo item = new FileInfo(Path.Combine(onlineChartdirInfo.FullName, scannedRecord.Scanned_File_Name));
                            if (item.Exists == true && item.Length > 0)//To check the File size
                            {
                                iTryCount = 0;
                                fileIteration++;
                                int pageCount = 0;
                                System.Drawing.Image imgbg = System.Drawing.Image.FromFile(item.FullName);
                                pageCount = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                                HtmlGenericControl gnDiv = new HtmlGenericControl("div");
                                gnDiv.ID = "file_" + fileIteration.ToString();
                                gnDiv.Style.Add("height", "auto");
                                gnDiv.Style.Add("width", "450px");
                                gnDiv.Style.Add("margin-bottom", "4px");
                                gnDiv.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                                gnDiv.Attributes.Add("onclick", "choosefiles('file_" + fileIteration.ToString() + "','" + HttpUtility.UrlEncode(item.FullName) + "','" + pageCount + "');");
                                gnDiv.InnerText = item.Name;
                                fileThumbs.Controls.Add(gnDiv);
                                imgbg.Dispose();
                            }
                            else if (item.Exists == false)
                            {
                                if (iTryCount < 3)
                                {
                                    iTryCount++;
                                    Thread.Sleep(2000);
                                    goto retry;
                                }
                            }

                        }
                        hdnFiles.Value = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw (ex);
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "StopLoadingImage", "StopLoadOnUploadFile();" + hdnSelecteFile.Value, true);
        }

        protected void btnClose_ServerClick(object sender, EventArgs e)
        {
            Session["BrowseLoadList"] = null;
            Session["LoadList"] = null;
            Session["BrowseFileNames"] = null;
            Session["IS_MovedToNextProcess"] = null;
            Session["Is_Web_Portal"] = null;
            Session["FilePath"] = null;
            Session["IS_MovedToNextProcess"] = null;
            //Session["SelectedDateList"] = null;
            DirectoryInfo DirWaitFolder = new DirectoryInfo(Server.MapPath("~/atala-capture-upload/Indexing_Files/Wait_To_Delete"));
            if (DirWaitFolder.Exists)
            {
                if (DirWaitFolder.GetFiles().Count() > 0)
                {
                    DirWaitFolder.GetFiles().ToList().ForEach(f => f.Delete());
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "close", "btnClose_Clicked();", true);
        }

        protected void btnClearAll_ServerClick(object sender, EventArgs e)
        {
            Session["BrowseLoadList"] = null;
            Session["LoadList"] = null;
            // Session["SelectedDateList"] = null;
            Session["BrowseFileNames"] = null;
            fileThumbs.Controls.Clear();
            if (Request.QueryString["Screen"] != null && Request.QueryString["Screen"] == "PatientPortalOnlineDoumnets")
            {
                cboDocumentType.SelectedIndex = 0;
                cboDocumentSubType.SelectedItem.Text = "";
            }
            hdnSelecteFile.Value = string.Empty;
        }

        protected void cboDocumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDocumentType.SelectedIndex > 0)
            {
                cboDocumentSubType.Enabled = true;

            }

            LoadDocumentSubType(cboDocumentType.SelectedItem.Text.ToUpper());
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "btnUploadClick", "clickUpload();", true);
        }

        public void LoadDocType()
        {
            //IList<StaticLookup> docSublist = new List<StaticLookup>();
            //XDocument xmlDocumentType = XDocument.Load(Server.MapPath(@"ConfigXML\Doctype.xml"));
            //StaticLookup objStatics = null;
            //ListItem liDropdown = null;
            //foreach (XElement elements in xmlDocumentType.Descendants("DocElement"))
            //{
            //    string xmlValue = elements.Attribute("name").Value;               
            //    liDropdown = new ListItem(xmlValue, xmlValue);
            //    cboDocumentType.Items.Add(liDropdown);
            //    int sortOrder = 0;
            //    foreach (XElement subDocs in elements.Elements())
            //    {
            //        string subDoc = subDocs.Attribute("name").Value;
            //        sortOrder++;
            //        objStatics = new StaticLookup();
            //        objStatics.Field_Name = xmlValue;
            //        objStatics.Value = subDoc;
            //        objStatics.Sort_Order = sortOrder;
            //        docSublist.Add(objStatics);
            //    }               
            //}

            //CAP-2767
            IList<StaticLookup> docSublist = new List<StaticLookup>();
            doctypeList objDoctypeList = new doctypeList();
            objDoctypeList = ConfigureBase<doctypeList>.ReadJson("Doctype.json");
            StaticLookup objStatics = null;
            ListItem liDropdown = null;
            foreach (Acurus.Capella.Core.DTOJson.Doctype dt in objDoctypeList.DocType)
            {
                liDropdown = new ListItem(dt.name, dt.name);
                cboDocumentType.Items.Add(liDropdown);
                int sortOrder = 0;
                foreach (subDoc sb in dt.subDoc)
                {
                    sortOrder++;
                    objStatics = new StaticLookup();
                    objStatics.Field_Name = dt.name;
                    objStatics.Value = sb.name;
                    objStatics.Sort_Order = sortOrder;
                    docSublist.Add(objStatics);
                }
            }

            cboDocumentType.Items.Insert(0, "");
            cboDocumentSubType.Items.Insert(0, "");
            Session["DocSubList"] = docSublist;

        }

        public void LoadDocumentSubType(string Item)
        {
            btnSaveOnline.Enabled = true;
            divLoading.Style.Add("display", "none");
            if (Session["DocSubList"] != null)
            {
                docSublist = (IList<StaticLookup>)Session["DocSubList"];
                IList<StaticLookup> userLookup = (from doc in docSublist where doc.Field_Name.ToUpper() == Item.ToUpper() select doc).ToList<StaticLookup>();
                cboDocumentSubType.Items.Clear();

                DataTable dt = new DataTable();
                DataColumn col1 = new DataColumn("Text", typeof(string));
                dt.Columns.Add(col1);
                DataColumn col2 = new DataColumn("Value", typeof(string));
                dt.Columns.Add(col2);
                for (int j = 0; j < userLookup.Count; j++)
                {

                    DataRow dr = dt.NewRow();
                    dr["Text"] = userLookup[j].Value;
                    dr["Value"] = userLookup[j].Value;
                    dt.Rows.Add(dr);
                }
                cboDocumentSubType.DataValueField = "Value";
                cboDocumentSubType.DataTextField = "Text";
                cboDocumentSubType.DataSource = dt;
                cboDocumentSubType.DataBind();
                cboDocumentSubType.Items.Insert(0, "");
                cboDocumentSubType.SelectedIndex = 0;

                cboDocumentSubType.Enabled = true;

            }
        }

        protected void btnSaveOnline_Click(object sender, EventArgs e)
        {
            IList<Scan> ScanSavelist = ((IList<Scan>)HttpContext.Current.Session["BrowseLoadList"]);
            if (ScanSavelist == null)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FileAlert", "alert('Please choose any file.');", true);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "StopLoadingImage", "StopLoadOnUploadFile();", true);
                return;
            }
            if (ScanSavelist != null && cboDocumentType.SelectedValue.Trim() != "" && cboDocumentSubType.SelectedValue.Trim() != "")
            {
                if (ScanSavelist.Count > 0)
                {
                    if (cboDocumentType.SelectedValue.Trim().ToUpper() == "LEGAL DOCUMENTS")
                    {
                        if ((cboDocumentSubType.SelectedValue.Trim().ToUpper() == "ADVANCE DIRECTIVE" || cboDocumentSubType.SelectedValue.Trim().ToUpper() == "BIRTH PLAN") && Path.GetExtension(ScanSavelist[0].Scanned_File_Name).ToUpper() != ".PDF")
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ADAlert", "alert('Selected Subdocument type allows only PDF files.');", true);
                            return;
                        }
                    }
                }
                IList<Scan> insertSavelist = new List<Scan>();

                int pageCount = 0;
                string file_name = string.Empty;
                //DateTime dtDocDate = UtilityManager.ConvertToUniversal();
                DateTime dtDocDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 07:30:00"));

                string ChoosedFilename = string.Empty;


                //Getting total no of  page count 
                string sDirPath = Server.MapPath("~/atala-capture-upload/Indexing_Files/");
                if (ScanSavelist.Count > 0)
                {
                    int iTryCount = 0;
                retry:
                    FileInfo item = new FileInfo(Path.Combine(sDirPath, ScanSavelist[0].Scanned_File_Name));
                    ChoosedFilename = ScanSavelist[0].Scanned_File_Name;
                    if (item.Exists == true)
                    {
                        if (Path.GetExtension(ChoosedFilename).ToUpper() != ".PDF")
                        {
                            using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(item.FullName))
                            {
                                pageCount = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                                imgbg.Dispose();
                            }
                        }
                        else
                        {
                            using (FileStream fs = new FileStream(item.FullName, FileMode.Open, FileAccess.Read))
                            {
                                StreamReader sr = new StreamReader(fs);
                                string pdf = sr.ReadToEnd();
                                Regex rx = new Regex(@"/Type\s*/Page[^s]");
                                MatchCollection match = rx.Matches(pdf);
                                pageCount = match.Count;
                                if (pageCount == 0)
                                {

                                    PdfReader pdfReader = new PdfReader(item.FullName);
                                    pageCount = pdfReader.NumberOfPages;

                                }
                                sr.Dispose();
                                fs.Dispose();
                            }
                        }
                    }
                    else if (item.Exists == false)
                    {
                        if (iTryCount < 3)
                        {
                            iTryCount++;
                            Thread.Sleep(2000);
                            goto retry;
                        }
                    }
                }

                //File Name creation
                string lastNumToAdd = string.Empty;
                int prevNum = 0;
                Scan_IndexManager objScanIndexMngr = new Scan_IndexManager();
                IList<scan_index> ilstScanindex = new List<scan_index>();

                ilstScanindex = objScanIndexMngr.GetScanIndexForHuman(ClientSession.HumanId);
                if (ilstScanindex != null && ilstScanindex.Count > 0)
                {
                    int[] sortIndexNum = new int[ilstScanindex.Count];
                    for (int i = 0; i < ilstScanindex.Count; i++)
                    {
                        file_name = Path.GetFileName(ilstScanindex[i].Indexed_File_Path);
                        prevNum = Convert.ToInt32(file_name.Substring(file_name.LastIndexOf("_") + 1, (file_name.LastIndexOf(".") - 1) - file_name.LastIndexOf("_")));
                        sortIndexNum[i] = prevNum;
                    }
                    prevNum = sortIndexNum.Max();
                    hdnNo.Value = Convert.ToString(prevNum + 1);
                    lastNumToAdd = Convert.ToString(prevNum + 1);
                    if (lastNumToAdd.Length == 1)
                        lastNumToAdd = "0" + lastNumToAdd;
                }
                else
                {
                    hdnNo.Value = 1.ToString();
                    lastNumToAdd = hdnNo.Value;
                    if (lastNumToAdd.Length == 1)
                        lastNumToAdd = "0" + lastNumToAdd;

                }
                file_name = "Patient_portal_ONLINE_" + dtDocDate.ToString("yyyyMMdd") + "_" + ClientSession.HumanId.ToString() + "_" + lastNumToAdd + Path.GetExtension(ChoosedFilename);

                //Move to local folder 
                string sourceFile = Server.MapPath("~/atala-capture-upload/Indexing_Files/") + ChoosedFilename;
                string drt_path = Server.MapPath("~/atala-capture-upload/Indexing_Files/Patient_Portal") + "\\" + ClientSession.HumanId.ToString();
                string filePath = Server.MapPath("~/atala-capture-upload/Indexing_Files/Patient_Portal") + "\\" + ClientSession.HumanId.ToString() + "\\" + Path.GetFileName(file_name);
                string LocalfilePath = "~/atala-capture-upload/Indexing_Files/Patient_Portal" + "\\" + ClientSession.HumanId.ToString() + "\\" + Path.GetFileName(file_name);
                DirectoryInfo dirt = new DirectoryInfo(drt_path);
                if (!dirt.Exists)
                {
                    dirt.Create();
                }
                System.IO.File.Copy(sourceFile, filePath, true);

                //Scan index  conversion save
                scan_index scanIndexObject = new scan_index();
                IList<scan_index> ScanConversionInserlist = new List<scan_index>();
                objScanIndexMngr = new Scan_IndexManager();
                Scan_IndexDTO AddedListOfScanIndex = null;


                scanIndexObject.Indexed_File_Path = LocalfilePath;
                scanIndexObject.Page_Selected = pageCount.ToString();
                scanIndexObject.Document_Type = cboDocumentType.SelectedItem.Text;
                scanIndexObject.Document_Sub_Type = cboDocumentSubType.Text;
                scanIndexObject.Document_Date = dtDocDate;
                scanIndexObject.Created_By = ClientSession.UserName;
                scanIndexObject.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                scanIndexObject.Human_ID = ClientSession.HumanId;
                ScanConversionInserlist.Add(scanIndexObject);

                //For Patient portal there is no facility to get the Close_type from workflow ,So get any facility from config Xml

                string facility = string.Empty;
                XmlDocument xmldoc = new XmlDocument();
                string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\Facility_Library.xml");
                if (File.Exists(strXmlFilePath) == true)
                {
                    xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "Facility_Library" + ".xml");
                    XmlNodeList xmlFacilityList = xmldoc.GetElementsByTagName("Facility");
                    if (xmlFacilityList.Count > 0)
                    {
                        facility = xmlFacilityList[0].Attributes["Name"].Value.ToString();

                    }
                }

                DateTime dtScanReceivedDate = Convert.ToDateTime(dtpScannedDate.Value);
                //Save Scan_Index_Conversion table, Scan table ,WorkFlow table 
                AddedListOfScanIndex = objScanIndexMngr.SaveUpdateDeleteOnlineDocuments(ScanConversionInserlist, null, null, ClientSession.HumanId, 0, string.Empty, string.Empty, facility, LocalfilePath, pageCount, ChoosedFilename, dtScanReceivedDate, "Online Chart - LOCAL");

                //Move to Next process

                string uri = string.Empty;
                IList<FileManagementIndex> fileManagementIndexList = new List<FileManagementIndex>();
                IList<WFObject> lstWF_object = new List<WFObject>();
                FileManagementIndexManager fileManagementIndexmanager = new FileManagementIndexManager();
                IList<scan_index> scanIndexList = ScanConversionInserlist;

                ulong scan_ID = 0;
                if (scanIndexList.Count > 0)
                {
                    scan_ID = scanIndexList[0].Scan_ID;
                }


                ulong[] uScanID = new ulong[scanIndexList.Count];

                #region FTP Transfer
                string ftpServerIP = System.Configuration.ConfigurationManager.AppSettings["ftpServerIP"];
                string ftpUserName = System.Configuration.ConfigurationManager.AppSettings["ftpUserID"];
                string ftpPassword = System.Configuration.ConfigurationManager.AppSettings["ftpPassword"];
                FTPImageProcess _ftpImageProcess = new FTPImageProcess();
                string serverPath = string.Empty;

                if (_ftpImageProcess.CreateDirectory(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserName, ftpPassword))
                {
                    for (int i = 0; i < scanIndexList.Count; i++)
                    {
                        serverPath = _ftpImageProcess.UploadToImageServer(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserName, ftpPassword, Server.MapPath(scanIndexList[i].Indexed_File_Path), string.Empty);
                        if (serverPath != string.Empty)
                        {
                            FileManagementIndex filemanagementIndex = new FileManagementIndex();
                            filemanagementIndex.Created_By = scanIndexList[i].Created_By;
                            filemanagementIndex.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            filemanagementIndex.Document_Date = scanIndexList[i].Document_Date;
                            filemanagementIndex.Document_Type = scanIndexList[i].Document_Type;
                            filemanagementIndex.Document_Sub_Type = scanIndexList[i].Document_Sub_Type;
                            filemanagementIndex.Source = "SCAN";
                            filemanagementIndex.Human_ID = scanIndexList[i].Human_ID;
                            filemanagementIndex.Order_ID = scanIndexList[i].Order_ID;
                            filemanagementIndex.Scan_Index_Conversion_ID = scanIndexList[i].Id;
                            filemanagementIndex.File_Path = serverPath;
                            filemanagementIndex.Encounter_ID = scanIndexList[i].Encounter_ID;
                            uScanID[i] = scanIndexList[i].Scan_ID;
                            fileManagementIndexList.Add(filemanagementIndex);

                        }
                        #region "Scanned File Trashing"
                        try
                        {
                            File.Delete(Server.MapPath(scanIndexList[i].Indexed_File_Path));
                        }
                        catch
                        {

                        }
                        #endregion
                    }
                }
                #endregion
                fileManagementIndexmanager.SaveUpdateDeleteFileManagementIndexForOnline_and_Wfobject(fileManagementIndexList.ToArray(), uScanID, ApplicationObject.macAddress, UtilityManager.ConvertToUniversal());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "DisplayAlert", "clickClearAll();alert('Files are uploaded successfully.');", true);
                btnSaveOnline.Enabled = false;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "btnUploadClick", "clickUpload();", true);
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "StopLoadingImage", "StopLoadOnUploadFile();", true);
            hdnfilepath.Value = "";
        }

        //[WebMethod]
        //public static string CheckDuplicateFile(string FileName)
        //{
        //    string sPath = string.Empty;
        //    if (FileName.Trim() != "" && FileName.Trim() != string.Empty)
        //        sPath = Path.GetFileName(HttpUtility.UrlDecode(FileName));
        //    ScanManager scanProxy = new ScanManager();
        //    bool bCheck = scanProxy.GetOnlineDocumentsList(ClientSession.FacilityName, sPath);
        //    var result = bCheck;
        //    return JsonConvert.SerializeObject(result);
        //}
    }
}





