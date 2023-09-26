using System;
using System.Collections;
using System.Collections.Generic;
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
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using Telerik.Web;
using Telerik.Web.UI;
using Acurus.Capella.UI;
using Acurus.Capella.UI.Extensions;
using System.Text.RegularExpressions;
using System.Net;
using System.Web.Services;
using System.Threading;
using Ionic.Zip;


namespace Acurus.Capella.UI
{
    public partial class frmImageViewer : System.Web.UI.Page
    {
        FileManagementIndexManager _fileIndexMngr = new FileManagementIndexManager();
        FTPImageProcess _ftpImageProcess = new FTPImageProcess();

        string localPath = string.Empty;
        string ftpServerIP = string.Empty;
        string simagePathname = string.Empty;
        string source = string.Empty;
        string file_path = string.Empty;
        string UNCPath = string.Empty;


        /*  New Viewer Parameters */
        int DefaultThumbHieght = 100;
        int DefaultThumbWidth = 100;
        int DefaultBigHieght = 750;
        int DefaultBigWidth = 750;
        int PagerSize = 8;
        string DocType = "";
        string DocDate = "";
        string docphy = "";
        int flag = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            ftpServerIP = System.Configuration.ConfigurationManager.AppSettings["ftpServerIP"];
            UNCPath = System.Configuration.ConfigurationManager.AppSettings["UNCPath"];
            source = Request.QueryString["Source"].ToString();

            if (!IsPostBack)
            {
                hdnpdfnotes.Value = "";
                /*To Avoid the file duplication in the local files and user specific temp folders*/
                localPath = Server.MapPath(@"atala-capture-download/" + Session.SessionID);

                /*To check the existance for the cache folder */
                DirectoryInfo drCache = new DirectoryInfo(localPath);
                if (!drCache.Exists)
                { drCache.Create(); }

               // hdnFileName.Value = Request.QueryString["FileName"].ToString().Replace("HASHSYMBOL", "#");

                switch (source.ToUpper())
                {
                        
                    #region "From Indexing"
                    case "INDEX":
                        Download.Style.Add("display", "none");
                        file_path = Request.QueryString["FileName"].ToString().Replace("HASHSYMBOL", "#");
                        hdnFileName.Value = file_path;
                        if (file_path != string.Empty)
                        {
                            simagePathname = file_path;
                            OpenFileIndex(simagePathname);
                        }
                        break;
                    #endregion
                    #region "From Result"
                    case "RESULT":
                        {
                            file_path = Request.QueryString["FilePath"].ToString().Replace("HASHSYMBOL", "#");
                            hdnFileName.Value = file_path;
                            //string _fileName = file_path.Replace(ftpServerIP, UNCPath);
                            if (file_path != string.Empty)
                            {
                                if ((string)Session["Scan_Index"] != "INDEX_SCREEN")
                                {
                                    OpenImage(file_path);
                                }
                                else
                                {
                                    OpenFileIndex(file_path);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region "From MRE"

                    case "MRE":
                        {
                            file_path = Request.QueryString["FileName"].ToString().Replace("HASHSYMBOL", "#");
                            hdnFileName.Value = file_path;
                            string _fileName = file_path.Replace(ftpServerIP, UNCPath);
                            if (_fileName != string.Empty)
                            {
                                OpenImage(_fileName);
                            }
                        }


                        break;
                    #endregion
                    #region "From CMG Results"
                    case "CMGRESULT":
                        {
                            file_path = Request.QueryString["FilePath"].ToString();
                            hdnFileName.Value = file_path;
                            string _fileName = file_path.Replace(ftpServerIP, UNCPath);
                            _fileName = _fileName.Replace("/", "\\");
                            OpenImage(_fileName);
                            break;
                        }
                    #endregion
                    case "EXAM":
                        {
                            //Download.Style.Add("display", "none");
                            btnsave.Attributes.Add("onclick", " return btnsaveClick()");
                            trbuttons.Visible = true;
                            trmesshistory.Visible = true;
                            trmessage.Visible = true;
                            trselect.Visible = true;
                            if (Session["ExamData"]!=null)
                                file_path = (string)Session["ExamData"];
                            string _fileName = file_path.Replace(ftpServerIP, UNCPath).Replace(@"/", @"\");
                            hdnFileName.Value = file_path;
                            string[] fileGroups = _fileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (fileGroups != null && fileGroups.Count() > 1)
                            {
                                OpenImageExam(fileGroups);
                            }
                            else
                            {
                                OpenImageExam(fileGroups[0]);
                            }

                            DocType = Request.QueryString["DocType"].ToString();
                            DocDate = Request.QueryString["DocDate"].ToString();
                            docphy = Request.QueryString["DocPhy"].ToString();
                            LoadCombo();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "exam", "setNotesValue();", true);
                            break;
                        }
                    case "ADINDEX":
                        {
                            if (Session["ADFilePAth"].ToString() != null)
                            {
                                file_path = Session["ADFilePAth"].ToString();
                                hdnFileName.Value = file_path;
                                if (file_path != string.Empty)
                                {
                                    simagePathname = file_path;
                                    OpenFileIndex(simagePathname);
                                    Label2.Visible = false;
                                    txtmsghistory.Visible = false;
                                    Label1.Visible = false;
                                    txtmessage.Visible = false;
                                    btnsave.Visible = false;
                                    btnViewerclose.Visible = false;
                                }
                            }
                            break;
                        }
                    case "EFAX":
                        {
                            var ActivityId = Request.QueryString["ActivityId"].ToString();
                            ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                            IList<ActivityLog> UpdateActivityLogList = new List<ActivityLog>();
                            UpdateActivityLogList = ActivitylogMngr.GetFaxActivity(ActivityId);
                            btnsave.Attributes.Add("display", "none");
                            //trbuttons.Visible = true;
                            trmesshistory.Visible = false;
                            trmessage.Visible = false;
                            trselect.Visible = false;
                            if (UpdateActivityLogList.Count > 0)
                            {
                                if (UpdateActivityLogList[0].Fax_Sent_File_Path.Trim() != "")
                                    file_path = (string)UpdateActivityLogList[0].Fax_Sent_File_Path;
                                else if (UpdateActivityLogList[0].Fax_File_Path.Trim() != "")
                                    file_path = (string)UpdateActivityLogList[0].Fax_File_Path;
                            }
                            else
                                file_path = "";
                            Session["EFaxData"] = file_path;
                            string _fileName = file_path.Replace(ftpServerIP, UNCPath).Replace(@"/", @"\");
                            string[] fileGroups = _fileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (fileGroups.Count() > 0)
                            {
                                for (int i = 0; i < fileGroups.Count(); i++)
                                {
                                    //string sfax = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["UNCPathFax"], System.Configuration.ConfigurationManager.AppSettings["Twilio_Ready_for_Send_Path"]);
                                    //fileGroups[i] = Path.Combine(sfax ,fileGroups[i].TrimStart());


                                    fileGroups[i] = fileGroups[i].Replace(ftpServerIP, UNCPath).Replace(@"/", @"\");
                                }
                            }
                            if (fileGroups != null && fileGroups.Count() > 1)
                                OpenImageEFax(fileGroups);
                            else
                            {
                                if (file_path != null && file_path != "")
                                    OpenImageEFax(fileGroups[0].Replace(ftpServerIP, UNCPath).Replace(@"/", @"\"));
                            }
                            hdnFileName.Value = file_path;
                            break;
                        }
                    #region "Others"
                    default:
                        break;
                    #endregion
                }
            }

        }
        public void LoadCombo()
        {
            IList<FileManagementIndex> file_exam_lst = new List<FileManagementIndex>();
            file_exam_lst = (IList<FileManagementIndex>)Session["ExamList"];
            if (file_exam_lst != null && file_exam_lst.Count > 0)
            {

                //var test = (from doc in file_exam_lst
                //            where (doc.Document_Type == DocType)
                //            select doc).ToList<FileManagementIndex>();


                //test = (from doc in test
                //        where (UtilityManager.ConvertToLocal(doc.Document_Date).ToString("dd-MMM-yyyy") == DocDate)
                //        select doc).ToList<FileManagementIndex>();

                //for (int j = 0; j < test.Count;j++ )
                //    file_exam_lst = (from doc in file_exam_lst
                //                     where (doc.Id != test[j].Id)
                //                     select doc).ToList<FileManagementIndex>(); ;


                // DropDownimagelist.Items.Add("--Select Image to Compare--");



                IList<string> lstDate = ((from doc in file_exam_lst

                                          orderby doc.Document_Date descending
                                          select doc.Document_Date.ToString("dd-MMM-yyyy")).Distinct()).ToList<string>();
                IList<FileManagementIndex> filelist = (from doc in file_exam_lst
                                                       orderby doc.Document_Date descending
                                                       select doc).ToList<FileManagementIndex>();
                int index = 0;

                for (int k = 0; k < lstDate.Count; k++)
                {



                    IList<string> file_management_list_phy = ((from doc in file_exam_lst
                                                               where doc.Document_Date.ToString("dd-MMM-yyyy") == lstDate[k]
                                                               orderby doc.Document_Date descending
                                                               select doc.Appointment_Provider_ID.ToString()).Distinct()).ToList<string>();

                    for (int m = 0; m < file_management_list_phy.Count; m++)
                    {

                        IList<string> lstExamdate = ((from doc in file_exam_lst
                                                      where doc.Document_Date.ToString("dd-MMM-yyyy") == lstDate[k] &&
                                                      doc.Document_Type != string.Empty
                                                      && doc.Appointment_Provider_ID.ToString() == file_management_list_phy[m]

                                                      select doc.Document_Type).Distinct()).ToList<string>();


                        for (int n = 0; n < lstExamdate.Count; n++)
                        {
                            string name = "";

                            IList<FileManagementIndex> file_management_list = (from doc in file_exam_lst
                                                                               where doc.Document_Date.ToString("dd-MMM-yyyy") == lstDate[k]
                                                                               && doc.Document_Type == lstExamdate[n]
                                                                               && doc.Appointment_Provider_ID.ToString() == file_management_list_phy[m]
                                                                                && (doc.File_Path.ToUpper().Contains(".JPG") || doc.File_Path.ToUpper().Contains(".PNG")
                                                                                || doc.File_Path.ToUpper().Contains(".JPEG") || doc.File_Path.ToUpper().Contains(".BMP") || doc.File_Path.ToUpper().Contains(".PDF"))
                                                                               select doc).ToList<FileManagementIndex>();
                            if (file_management_list.Count() > 0)
                            {
                                PhysicianManager obj = new PhysicianManager();
                                PhysicianLibrary phy = new PhysicianLibrary();
                                try
                                {
                                    if (file_management_list[0].Appointment_Provider_ID != 0)
                                    {
                                        phy = obj.GetById(file_management_list[0].Appointment_Provider_ID);
                                        name = phy.PhyPrefix + " " + phy.PhyFirstName + " " + phy.PhyMiddleName + " " + phy.PhyLastName + " " + phy.PhySuffix;
                                    }


                                }
                                catch
                                {
                                    name = "";
                                }

                                DateTime testtaken = file_management_list[0].Document_Date;

                                DropDownimagelist.Items.Add(lstExamdate[n] + " , Date of Exam : " + UtilityManager.ConvertToLocal(testtaken).ToString("dd-MMM-yyyy") + "," + name);
                                if (lstExamdate[n].ToUpper() == DocType.ToUpper() && UtilityManager.ConvertToLocal(testtaken).ToString("dd-MMM-yyyy") == DocDate && file_management_list[0].Appointment_Provider_ID.ToString() == docphy)
                                    DropDownimagelist.SelectedIndex = index;
                                index++;
                            }

                        }
                    }
                }


            }
        }
        protected void DropDownimagelist_SelectedIndexChanged(object sender, EventArgs e)
        {
            flag = 0;
            IFileManagementIndexManager objfilemanager = new FileManagementIndexManager();
            IList<FileManagementIndex> file_exam_lst = new List<FileManagementIndex>();
            file_exam_lst = (IList<FileManagementIndex>)Session["ExamList"];

            List<string> FilePaths = new List<string>();
            List<string> fExten = new List<string>();
            bool isViewable = false;



            file_exam_lst = objfilemanager.GetIndexedListByHumanId(ClientSession.HumanId, "Exam");
            // file_exam_lst = (IList<FileManagementIndex>)Session["ExamList"];
            IList<FileManagementIndex> File_ListForDownlaod = new List<FileManagementIndex>();
            IList<FileManagementIndex> lstTemp = (from doc in file_exam_lst
                                                  where (doc.Document_Type == DropDownimagelist.SelectedItem.Text.Split(',')[0].Trim()) && (UtilityManager.ConvertToLocal(doc.Document_Date).ToString("dd-MMM-yyyy") == DropDownimagelist.SelectedItem.Text.Split(',')[1].Split(':')[1].Trim())
                                                  select doc).ToList<FileManagementIndex>();
            File_ListForDownlaod = File_ListForDownlaod.Concat(lstTemp).ToList();

            //if (lstTemp.Count > 1)
            //{
            //    DownloadFile();
            //}
            //else
            // {
            foreach (FileManagementIndex item in lstTemp)
            {
                fExten.Add(item.File_Path.Substring(item.File_Path.LastIndexOf('.')).ToString().ToUpper());
            }
            fExten = fExten.Distinct().ToList<string>();

            for (int k = 0; k < fExten.Count; k++)
            {
                if (fExten[k] == ".JPG" || fExten[k] == ".PNG" || fExten[k] == ".JPEG" || fExten[k] == ".BMP" || fExten[k] == ".PDF")
                {
                    isViewable = true;
                }
                else
                {
                    isViewable = false;
                }
            }
            if (!isViewable)
            {
                // DownloadFile();
            }
            else
            {
                string target = string.Empty;
                string Notes = string.Empty;
                foreach (FileManagementIndex item in lstTemp)
                {
                    target += item.File_Path.ToString() + "|";
                    Notes += item.Exam_Photos_Notes.ToString() + "^$#@^" + item.Id.ToString() + "~";
                }
                // lblactimage.Text = DropDownimagelist.SelectedItem.Text.Split(',')[0].Trim() + "-" + DropDownimagelist.SelectedItem.Text.Split(',')[2].Trim();
                string sDoctype = DropDownimagelist.SelectedItem.Text.Split(',')[0].Trim(); //e.Item.Cells[3].Text.ToString();
                string sDocdate = DropDownimagelist.SelectedItem.Text.Split(',')[1].Split(':')[1].Trim(); //e.Item.Cells[5].Text.ToString();
                IList<string> ExamString = new List<string>();

                Session.Add("NotesExam", Notes);
                file_path = target;
                ftpServerIP = System.Configuration.ConfigurationManager.AppSettings["ftpServerIP"];
                UNCPath = System.Configuration.ConfigurationManager.AppSettings["UNCPath"];
                string _fileName = file_path.Replace(ftpServerIP, UNCPath).Replace(@"/", @"\");
                string[] fileGroups = _fileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (fileGroups != null && fileGroups.Count() > 1)
                {
                    OpenImageExam(fileGroups);
                }
                else
                { OpenImageExam(fileGroups[0]); }




                ScriptManager.RegisterStartupScript(this, this.GetType(), "examNotes", "setNotesValue();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "examLoadChange", " { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);

            }
            //  }

        }
        public void OpenFileExam(string filename)
        {
            System.Web.UI.HtmlControls.HtmlMeta META = new System.Web.UI.HtmlControls.HtmlMeta();
            META.HttpEquiv = "Pragma";
            META.Content = "no-cache";
            Page.Header.Controls.Add(META);
            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            hdnPageBox.Value = Convert.ToString("1");

            //Get FilePath
            // FilePath = Request.QueryString["File"];
            FilePath = filename;
            string file_Notes = (string)Session["NotesExam"].ToString().Split('~')[0];
            // _plcImgsThumbs.Controls.Clear();
            if (FilePath != "")
            {
                //Determine Start/End Pages
                int StartPg = 1;
                if (Request.QueryString["StartPage"] != null)
                    StartPg = System.Convert.ToInt16(Request.QueryString["StartPage"]);

                PageLabel.Value = @" / " + 1.ToString();

                //Add/configure the thumbnails
                if (!(FilePath.ToUpper().Contains(".PDF")))
                {

                    int BigImgPg = StartPg;
                    //int EndPg = TotalTIFPgs;
                    //if (EndPg > TotalTIFPgs)
                    //    EndPg = TotalTIFPgs;

                    int EndPg = 0;
                    if ((FilePath.ToUpper().Contains(".TIF")))
                    {
                        try
                        {
                            using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(FilePath.ToString()))
                            {
                                EndPg = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                                imgbg.Dispose();
                            }
                        }
                        catch
                        {
                            EndPg = TotalTIFPgs;
                            if (EndPg > TotalTIFPgs)
                                EndPg = TotalTIFPgs;
                        }
                    }
                    else
                    {
                        EndPg = 1;
                    }


                    while (StartPg <= EndPg)
                    {
                        Label lbl = new Label();
                        if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                        else lbl.Text = "&nbsp;";
                        //HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                        //imgcontainer.ID = "pg_" + StartPg.ToString();
                        //imgcontainer.Attributes["class"] = "thumbnail";
                        System.Web.UI.WebControls.Image Img = new System.Web.UI.WebControls.Image();
                        Img.CssClass = "lazy";
                        Img.Style.Add("height", "84px");
                        Img.Style.Add("width", "79px");
                        Img.ImageAlign = ImageAlign.Middle;
                        Img.Attributes.Add("onClick", "ChangePg(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + StartPg.ToString() + "','" + file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','1');");
                        Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                        Img.Attributes.Add("class", "lazy");
                        Img.ToolTip = FilePath;
                        Img.AlternateText = "Page_" + StartPg;
                        Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                        HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                        pgDiv.InnerText = StartPg.ToString();
                        pgDiv.Attributes["class"] = "imgPageCompare";
                        //imgcontainer.Controls.Add(Img);
                        //imgcontainer.Controls.Add(pgDiv);
                        //_plcImgsThumbs.Controls.Add(imgcontainer);
                        divrotate.Style.Add("display", "inline-block");
                        // _plcImgsThumbs.Controls.Add(lbl);
                        StartPg++;
                    }
                    /* To load total no of page in the doument for the newly scanned files */
                    Session["Page_Count"] = EndPg;
                    //Bind big img
                    //  hdnnotes.Value = file_Notes.ToString().Split('^')[0].ToString();
                    // hdnfileindexid.Value = file_Notes.ToString().Split('^')[1].ToString();
                    hdnnotes.Value = file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0].ToString();
                    hdnfileindexid.Value = file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1].ToString();

                    _imgBig.Src = "ViewImg.aspx?View=1&FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();
                    bigImgPDF.Style.Add("display", "none");
                    _imgBig.Style.Add("display", "block");
                    PDFholder.Style.Add("display", "none");
                    imgholder.Style.Add("display", "block");



                }
                else
                {


                    //  while (StartPg <= EndPg)
                    //  {
                    Label lbl = new Label();
                    if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                    else lbl.Text = "&nbsp;";
                    //HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                    //imgcontainer.ID = "pg_" + StartPg.ToString();
                    //imgcontainer.Attributes["class"] = "thumbnail";
                    System.Web.UI.WebControls.Label Img = new System.Web.UI.WebControls.Label();
                    Img.CssClass = "lazy";
                    Img.Style.Add("height", "84px");
                    Img.Style.Add("width", "50px");
                    Img.Style.Add("align", "center");// = ImageAlign.Middle;
                    Img.Attributes.Add("onClick", "ChangePg(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + StartPg.ToString() + "','" + file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','1');");
                    Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                    Img.Attributes.Add("class", "lazy");
                    if (Path.GetFileName(FilePath).Length > 40)
                        Img.Text = Path.GetFileName(FilePath).Substring(0, 40);
                    else
                        Img.Text = Path.GetFileName(FilePath);
                    Img.ToolTip = Path.GetFileName(FilePath);
                    Img.Style.Add("font-size", "15px");
                    Img.Style.Add("font-family", "Times New Roman");
                    Img.Style.Add("word-break", "break-all");
                    // Img.AlternateText = "Page_" + StartPg;
                    //Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                    HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                    pgDiv.InnerText = StartPg.ToString();
                    pgDiv.Attributes["class"] = "imgPage";
                    //imgcontainer.Controls.Add(Img);
                    // imgcontainer.Controls.Add(pgDiv);
                    //_plcImgsThumbs.Controls.Add(imgcontainer);
                    //  _plcImgsThumbs.Controls.Add(lbl);
                    divrotate.Style.Add("display", "none");
                    StartPg++;
                    //  }

                    //  Session["Page_Count"] = EndPg;
                    //Bind big img
                    //  hdnnotes.Value = file_Notes.ToString().Split('^')[0].ToString();
                    // hdnfileindexid.Value = file_Notes.ToString().Split('^')[1].ToString();
                    hdnnotes.Value = file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0].ToString();
                    hdnfileindexid.Value = file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1].ToString();

                    string uri = FilePath;//Request.QueryString["FilePath"];
                    string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                    string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                    string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                    string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                    string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                    string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
                    //Jira #CAP-67 
                    int iTryCount = 1;
                TryAgain:
                    try
                    {
                        using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                        {
                            if (unc.NetUseWithCredentials(UNCAuthPath, userName, domain, password))
                            {

                                DirectoryInfo DirWaitFolder = new DirectoryInfo(Server.MapPath("~/atala-capture-download/" + Session.SessionID + "//ExamPDF"));
                                if (!DirWaitFolder.Exists)
                                    Directory.CreateDirectory(DirWaitFolder.FullName);

                                if (File.Exists(FilePath) == true)
                                {
                                    if (File.Exists(DirWaitFolder.FullName + "\\" + Path.GetFileName(FilePath)) == true)
                                    {
                                        File.Delete(DirWaitFolder.FullName + "\\" + Path.GetFileName(FilePath));
                                    }

                                    File.Copy(FilePath, DirWaitFolder.FullName + "\\" + Path.GetFileName(FilePath), true);
                                }
                                Thread.Sleep(5000);
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
                                    sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(FilePath); ;
                                }
                                else
                                {
                                    sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(FilePath); ;
                                }


                                bigImgPDF.Attributes.Add("src", sImgPath);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string sErrorMessage = "";
                        if (UtilityManager.CheckFileNotFoundException(ex, out sErrorMessage))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\" " + sErrorMessage + "\");", true);
                        }
                        else
                        {
                            //Jira #CAP-67 
                            if (iTryCount <= 3)
                            {
                                iTryCount = iTryCount + 1;
                                Thread.Sleep(1500);
                                goto TryAgain;
                            }
                            else
                            {

                                UtilityManager.RetryExecptionLog(ex, iTryCount);
                                throw (ex);
                            }
                        }
                    }



                    _imgBig.Src = "";
                    bigImgPDF.Style.Add("display", "block");
                    _imgBig.Style.Add("display", "none");
                    PDFholder.Style.Add("display", "block");
                    imgholder.Style.Add("display", "none");






                }

            }

            else
            {
                Response.Write("Please provde a file path");
            }
        }
       /* public void OpenFileExam(string[] fileGroups)
        {
            System.Web.UI.HtmlControls.HtmlMeta META = new System.Web.UI.HtmlControls.HtmlMeta();
            META.HttpEquiv = "Pragma";
            META.Content = "no-cache";
            Page.Header.Controls.Add(META);
            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            //  hdnPageBox.Value = Convert.ToString("1");
            int BigImgPg = 1;

            string file_Notes = (string)Session["NotesExam"];
            string[] fileNotes = file_Notes.Split("~".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int GroupCount = 0;
            // foreach (string file in fileGroups)
            // _plcImgsThumbs.Controls.Clear();

            for (int i = 0; i < fileGroups.Length; i++)
            {
                int StartPg = 1;
                if (Request.QueryString["StartPage"] != null)
                    StartPg = System.Convert.ToInt16(Request.QueryString["StartPage"]);
               
                FilePath = fileGroups[i];
                if (FilePath != "")
                {


                    if (!(FilePath.ToUpper().Contains(".PDF")))
                    {
                        //Determine Start/End Pages

                        // PageLabel.Value = @" / " + EndPg.ToString();
                        //Add/configure the thumbnails
                        //int EndPg = TotalTIFPgs;
                        //if (EndPg > TotalTIFPgs)
                        //    EndPg = TotalTIFPgs;

                        int EndPg = 0;
                        if ((FilePath.ToUpper().Contains(".TIF")))
                        {
                            try
                            {
                                using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(FilePath.ToString()))
                                {
                                    EndPg = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                                    imgbg.Dispose();
                                }
                            }
                            catch
                            {
                                EndPg = TotalTIFPgs;
                                if (EndPg > TotalTIFPgs)
                                    EndPg = TotalTIFPgs;
                            }
                        }
                        else
                        {
                            EndPg = 1;
                        }

                        while (StartPg <= EndPg)
                        {
                            GroupCount++;
                            Label lbl = new Label();
                            if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                            else lbl.Text = "&nbsp;";
                            //// HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                            //   imgcontainer.ID = "pg_" + GroupCount.ToString();
                            // imgcontainer.Attributes["class"] = "thumbnail";
                            System.Web.UI.WebControls.Image Img = new System.Web.UI.WebControls.Image();
                            Img.CssClass = "lazy";
                            Img.Style.Add("height", "84px");
                            Img.Style.Add("width", "79px");
                            Img.ImageAlign = ImageAlign.Middle;
                            Img.Attributes.Add("onClick", "ChangePg(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + GroupCount.ToString() + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','" + (i + 1).ToString() + "');");
                            Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                            Img.Attributes.Add("class", "lazy");
                            Img.AlternateText = "Page_" + GroupCount;
                            
                            Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                            HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                            pgDiv.InnerText = GroupCount.ToString();
                            pgDiv.Attributes["class"] = "imgPage";
                            //imgcontainer.Controls.Add(Img);
                            // imgcontainer.Controls.Add(pgDiv);
                            divrotate.Style.Add("display", "inline-block");
                            // _plcImgsThumbs.Controls.Add(imgcontainer);
                            //_plcImgsThumbs.Controls.Add(lbl);

                            StartPg++;
                        }


                        // To load total no of page in the doument for the newly scanned files 
                        Session["Page_Count"] = EndPg;


                    }
                    else
                    {


                        // while (StartPg <= EndPg)
                        // {
                        GroupCount++;
                        Label lbl = new Label();
                        if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                        else lbl.Text = "&nbsp;";
                        //HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                        //imgcontainer.ID = "pg_" + GroupCount.ToString();
                        //imgcontainer.Attributes["class"] = "thumbnail";
                        System.Web.UI.WebControls.Label Img = new System.Web.UI.WebControls.Label();
                        Img.CssClass = "lazy";
                        Img.Style.Add("height", "100px");
                        Img.Style.Add("width", "55px");
                        Img.Style.Add("align", "center");// = ImageAlign.Middle;
                        // imgcontainer.Attributes.Add("onClick", "ChangePg(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + GroupCount.ToString() + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','" + (i + 1).ToString() + "');");
                        Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                        Img.Attributes.Add("class", "lazy");
                        Img.ToolTip = Path.GetFileName(FilePath);
                        Img.Style.Add("font-size", "15px");
                        Img.Style.Add("word-break", "break-all");
                        Img.Style.Add("font-family", "Times New Roman");
                        if (Path.GetFileName(FilePath).Length > 40)
                            Img.Text = Path.GetFileName(FilePath).Substring(0, 40);
                        else
                            Img.Text = Path.GetFileName(FilePath);
                        // Img.AlternateText = "Page_" + StartPg;
                        //Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                        HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                        pgDiv.InnerText = GroupCount.ToString();
                        pgDiv.Attributes["class"] = "imgPage";
                        // imgcontainer.Controls.Add(Img);


                        // imgcontainer.Controls.Add(pgDiv);
                        //_plcImgsThumbs.Controls.Add(imgcontainer);
                        //_plcImgsThumbs.Controls.Add(lbl);
                        StartPg++;
                        //  }

                    }

                }

            }



            PageLabel.Value = @" / " + GroupCount.ToString();
            if (flag == 0)
            {

                PageBox.Value = "1";
                hdnPageBox.Value = Convert.ToString("1");
                hdnnotes.Value = fileNotes[0].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0].ToString();
                hdnfileindexid.Value = fileNotes[0].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1].ToString();
                hdnFileNameExam.Value = String.Join("|||", fileGroups);
                if (!(fileGroups[0].ToUpper().Contains(".PDF")))
                {
                    divrotate.Style.Add("display", "inline-block");
                    _imgBig.Src = "ViewImg.aspx?View=1&FilePath=" + HttpUtility.UrlEncode(fileGroups[0]) + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();

                    bigImgPDF.Style.Add("display", "none");
                    _imgBig.Style.Add("display", "block");
                    PDFholder.Style.Add("display", "none");

                    imgholder.Style.Add("display", "block");
                }
                else
                {

                    divrotate.Style.Add("display", "none");
                    string uri = fileGroups[0];//Request.QueryString["FilePath"];
                    string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                    string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                    string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                    string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                    string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                    string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
                    try
                    {
                        using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                        {
                            if (unc.NetUseWithCredentials(UNCAuthPath, userName, domain, password))
                            {

                                DirectoryInfo DirWaitFolder = new DirectoryInfo(Server.MapPath("~/atala-capture-download/" + Session.SessionID + "//ExamPDF"));
                                if (!DirWaitFolder.Exists)
                                    Directory.CreateDirectory(DirWaitFolder.FullName);

                                if (File.Exists(fileGroups[0]) == true)
                                {

                                    if (File.Exists(DirWaitFolder.FullName + "\\" + Path.GetFileName(fileGroups[0])) == true)
                                    {
                                        File.Delete(DirWaitFolder.FullName + "\\" + Path.GetFileName(fileGroups[0]));
                                    }

                                    File.Copy(fileGroups[0], DirWaitFolder.FullName + "\\" + Path.GetFileName(fileGroups[0]), true);
                                }
                                Thread.Sleep(5000);
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
                                    sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(fileGroups[0]); ;
                                }
                                else
                                {
                                    sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(fileGroups[0]); ;
                                }


                                bigImgPDF.Attributes.Add("src", sImgPath);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }



                    _imgBig.Src = "";
                    bigImgPDF.Style.Add("display", "block");
                    _imgBig.Style.Add("display", "none");
                    PDFholder.Style.Add("display", "block");
                    imgholder.Style.Add("display", "none");


                }

            }

        }


        */

         public void OpenFileExam(string[] fileGroups)
        {
            System.Web.UI.HtmlControls.HtmlMeta META = new System.Web.UI.HtmlControls.HtmlMeta();
            META.HttpEquiv = "Pragma";
            META.Content = "no-cache";
            Page.Header.Controls.Add(META);
            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            //  hdnPageBox.Value = Convert.ToString("1");
            int BigImgPg = 1;

            string file_Notes = (string)Session["NotesExam"];
            string[] fileNotes = file_Notes.Split("~".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int GroupCount = 0;
            // foreach (string file in fileGroups)
            // _plcImgsThumbs.Controls.Clear();
            string[] FilePathNew = new string[fileGroups.Length];
            for (int i = 0; i < fileGroups.Length; i++)
            {
                int StartPg = 1;
                if (Request.QueryString["StartPage"] != null)
                    StartPg = System.Convert.ToInt16(Request.QueryString["StartPage"]);

                FilePath = fileGroups[i];
                if (FilePath != "")
                {
                    if (!(FilePath.ToUpper().Contains(".PDF")))
                    {
                        int EndPg = 0;
                        if ((FilePath.ToUpper().Contains(".TIF")))
                        {
                            try
                            {
                                using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(FilePath.ToString()))
                                {
                                    EndPg = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                                    imgbg.Dispose();
                                }
                            }
                            catch
                            {
                                EndPg = TotalTIFPgs;
                                if (EndPg > TotalTIFPgs)
                                    EndPg = TotalTIFPgs;
                            }
                        }
                        else
                        {
                            EndPg = 1;
                        }

                        while (StartPg <= EndPg)
                        {
                            GroupCount++;
                            Label lbl = new Label();
                            if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                            else lbl.Text = "&nbsp;";
                            //// HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                            //   imgcontainer.ID = "pg_" + GroupCount.ToString();
                            // imgcontainer.Attributes["class"] = "thumbnail";
                            System.Web.UI.WebControls.Image Img = new System.Web.UI.WebControls.Image();
                            Img.CssClass = "lazy";
                            Img.Style.Add("height", "84px");
                            Img.Style.Add("width", "79px");
                            Img.ImageAlign = ImageAlign.Middle;
                            Img.Attributes.Add("onClick", "ChangePg(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + GroupCount.ToString() + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','" + (i + 1).ToString() + "');");
                            Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                            Img.Attributes.Add("class", "lazy");
                            Img.AlternateText = "Page_" + GroupCount;

                            Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                            HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                            pgDiv.InnerText = GroupCount.ToString();
                            pgDiv.Attributes["class"] = "imgPage";
                            //imgcontainer.Controls.Add(Img);
                            // imgcontainer.Controls.Add(pgDiv);
                            divrotate.Style.Add("display", "inline-block");
                            // _plcImgsThumbs.Controls.Add(imgcontainer);
                            //_plcImgsThumbs.Controls.Add(lbl);
                            FilePathNew[i]=FilePath;
                            StartPg++;
                        }


                        /* To load total no of page in the doument for the newly scanned files */
                        Session["Page_Count"] = EndPg;


                    }
                    else
                    {


                        // while (StartPg <= EndPg)
                        // {
                        GroupCount++;
                        Label lbl = new Label();
                        if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                        else lbl.Text = "&nbsp;";
                        //HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                        //imgcontainer.ID = "pg_" + GroupCount.ToString();
                        //imgcontainer.Attributes["class"] = "thumbnail";
                        System.Web.UI.WebControls.Label Img = new System.Web.UI.WebControls.Label();
                        Img.CssClass = "lazy";
                        Img.Style.Add("height", "100px");
                        Img.Style.Add("width", "55px");
                        Img.Style.Add("align", "center");// = ImageAlign.Middle;
                        // imgcontainer.Attributes.Add("onClick", "ChangePg(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + GroupCount.ToString() + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','" + (i + 1).ToString() + "');");
                        Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                        Img.Attributes.Add("class", "lazy");
                        Img.ToolTip = Path.GetFileName(FilePath);
                        Img.Style.Add("font-size", "15px");
                        Img.Style.Add("word-break", "break-all");
                        Img.Style.Add("font-family", "Times New Roman");
                        if (Path.GetFileName(FilePath).Length > 40)
                            Img.Text = Path.GetFileName(FilePath).Substring(0, 40);
                        else
                            Img.Text = Path.GetFileName(FilePath);
                        // Img.AlternateText = "Page_" + StartPg;
                        //Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                        HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                        pgDiv.InnerText = GroupCount.ToString();
                        pgDiv.Attributes["class"] = "imgPage";
                        // imgcontainer.Controls.Add(Img);9ii.


                        // imgcontainer.Controls.Add(pgDiv);
                        //_plcImgsThumbs.Controls.Add(imgcontainer);
                        //_plcImgsThumbs.Controls.Add(lbl);
                        StartPg++;
                        //  }
                        //To download multiple pdf
                        {

                            divrotate.Style.Add("display", "none");
                            string uri = FilePath;//Request.QueryString["FilePath"];
                            string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                            string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                            string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                            string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                            string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                            string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
                            //Jira #CAP-67 
                            int iTryCount = 1;
                        TryAgain:
                            try
                            {
                                using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                                {
                                    if (unc.NetUseWithCredentials(UNCAuthPath, userName, domain, password))
                                    {

                                        DirectoryInfo DirWaitFolder = new DirectoryInfo(Server.MapPath("~/atala-capture-download/" + Session.SessionID + "//ExamPDF"));
                                        if (!DirWaitFolder.Exists)
                                            Directory.CreateDirectory(DirWaitFolder.FullName);

                                        if (File.Exists(FilePath) == true)
                                        {

                                            if (File.Exists(DirWaitFolder.FullName + "\\" + Path.GetFileName(FilePath)) == true)
                                            {
                                                File.Delete(DirWaitFolder.FullName + "\\" + Path.GetFileName(FilePath));
                                            }

                                            File.Copy(FilePath, DirWaitFolder.FullName + "\\" + Path.GetFileName(FilePath), true);
                                        }
                                       // Thread.Sleep(5000);
                                        Uri CurrentURL = new Uri(Request.Url.ToString());
                                        string sProjectName = string.Empty;
                                        string sImgPath = string.Empty;
                                        for (int j = 0; j < CurrentURL.Segments.Length - 1; j++)
                                        {
                                            if (CurrentURL.Segments[j] != "/" && CurrentURL.Segments[j] != "//" && CurrentURL.Segments[j].StartsWith("frm") != true && sProjectName == string.Empty)
                                            {
                                                sProjectName = CurrentURL.Segments[j];
                                            }
                                        }
                                        if (sProjectName != string.Empty)
                                        {
                                            sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(FilePath);
                                        }
                                        else
                                        {
                                            sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(FilePath); ;
                                        }
                                        if (i == 0)
                                        {
                                            bigImgPDF.Attributes.Add("src", sImgPath);
                                        }
                                        FilePathNew[i] = sImgPath;

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string sErrorMessage = "";
                                if (UtilityManager.CheckFileNotFoundException(ex, out sErrorMessage))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\" " + sErrorMessage + "\");", true);
                                }
                                else
                                {
                                    //Jira #CAP-67 
                                    if (iTryCount <= 3)
                                    {
                                        iTryCount = iTryCount + 1;
                                        Thread.Sleep(1500);
                                        goto TryAgain;
                                    }
                                    else
                                    {
                                        UtilityManager.RetryExecptionLog(ex, iTryCount);
                                        throw (ex);
                                    }
                                }
                            }
                            _imgBig.Src = "";
                            bigImgPDF.Style.Add("display", "block");
                            _imgBig.Style.Add("display", "none");
                            PDFholder.Style.Add("display", "block");
                            imgholder.Style.Add("display", "none");
                        }

                    }

                }

            }



            PageLabel.Value = @" / " + GroupCount.ToString();
            if (flag == 0)
            {
                PageBox.Value = "1";
                hdnPageBox.Value = Convert.ToString("1");
                hdnnotes.Value = fileNotes[0].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0].ToString();
                hdnfileindexid.Value = fileNotes[0].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1].ToString();
                //hdnFileNameExam.Value = String.Join("|||", fileGroups);
                hdnFileNameExam.Value = String.Join("|||", FilePathNew);


                if (!(FilePathNew[0].ToUpper().Contains(".PDF")))
                {
                    divrotate.Style.Add("display", "inline-block");
                    _imgBig.Src = "ViewImg.aspx?View=1&FilePath=" + HttpUtility.UrlEncode(FilePathNew[0]) + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();
                    bigImgPDF.Style.Add("display", "none");
                    _imgBig.Style.Add("display", "block");
                    PDFholder.Style.Add("display", "none");
                    imgholder.Style.Add("display", "block");
                }
                else
                {
                    divrotate.Style.Add("display", "none");
                    bigImgPDF.Attributes.Add("src", FilePathNew[0]);
                    _imgBig.Src = "";
                    bigImgPDF.Style.Add("display", "block");
                    _imgBig.Style.Add("display", "none");
                    PDFholder.Style.Add("display", "block");
                    imgholder.Style.Add("display", "none");
                }

            }

        }

        public void btnhiddenfax_click(object sender, EventArgs e)
        {

            if (hdnpdf.Value != "")
            {

                flag = 1;
                string uri = hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " "); ;//Request.QueryString["FilePath"];
                string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
                //Jira #CAP-67 
                int iTryCount = 1;
            TryAgain:
                try
                {
                    using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                    {
                        if (unc.NetUseWithCredentials(UNCAuthPath, userName, domain, password))
                        {

                            DirectoryInfo DirWaitFolder = new DirectoryInfo(Server.MapPath("~/atala-capture-download/" + Session.SessionID + "//ExamPDF"));
                            if (!DirWaitFolder.Exists)
                                Directory.CreateDirectory(DirWaitFolder.FullName);

                            if (File.Exists(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")) == true)
                            {
                                File.Copy(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " "), DirWaitFolder.FullName + "\\" + Path.GetFileName(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")), true);
                            }
                            Thread.Sleep(5000);
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
                                sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")); ;
                            }
                            else
                            {
                                sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")); ;
                            }


                            bigImgPDF.Attributes.Add("src", sImgPath);

                        }
                    }
                }
                catch (Exception ex)
                {
                    string sErrorMessage = "";
                    if (UtilityManager.CheckFileNotFoundException(ex, out sErrorMessage))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\" " + sErrorMessage + "\");", true);
                    }
                    else
                    {
                        //Jira #CAP-67 
                        if (iTryCount <= 3)
                        {
                            iTryCount = iTryCount + 1;
                            Thread.Sleep(1500);
                            goto TryAgain;
                        }
                        else
                        {
                            UtilityManager.RetryExecptionLog(ex, iTryCount);
                            throw (ex);
                        }
                    }
                }


                _imgBig.Src = "";
                bigImgPDF.Style.Add("display", "block");
                _imgBig.Style.Add("display", "none");
                PDFholder.Style.Add("display", "block");
                imgholder.Style.Add("display", "none");


                ftpServerIP = System.Configuration.ConfigurationManager.AppSettings["ftpServerIP"];
                UNCPath = System.Configuration.ConfigurationManager.AppSettings["UNCPath"];
                file_path = (string)Session["EFaxData"];
                string _fileName = file_path.Replace(ftpServerIP, UNCPath).Replace(@"/", @"\");
                string[] fileGroups = _fileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                file_path = (string)Session["EFaxData"];
                if (fileGroups.Count() > 0)
                {
                    for (int i = 0; i < fileGroups.Count(); i++)
                    {
                        fileGroups[i] = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["ScanningPath_Fax"], fileGroups[i].TrimStart());
                    }
                }
                if (fileGroups.Count() > 1)
                {

                    OpenImageEFax(fileGroups);
                }
                else
                {
                    OpenImageEFax(fileGroups[0]);
                }
                divrotate.Style.Add("display", "none");
                // PageBox.Value = hdnPageBox.Value;
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "pdfchange", "pdfchangeview();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "examLoadChange", " { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Noates", "SetFaxValue();", true);
                // 

            }
        }
        public void btnhidden_click(object sender, EventArgs e)
        {

            if (hdnpdf.Value != "")
            {

                flag = 1;
                string uri = hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " "); ;//Request.QueryString["FilePath"];
                string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
                //Jira #CAP-67 
                int iTryCount = 1;
            TryAgain:
                try
                {
                    using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                    {
                        if (unc.NetUseWithCredentials(UNCAuthPath, userName, domain, password))
                        {

                            DirectoryInfo DirWaitFolder = new DirectoryInfo(Server.MapPath("~/atala-capture-download/" + Session.SessionID + "//ExamPDF"));
                            if (!DirWaitFolder.Exists)
                                Directory.CreateDirectory(DirWaitFolder.FullName);

                            if (File.Exists(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")) == true)
                            {
                                File.Copy(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " "), DirWaitFolder.FullName + "\\" + Path.GetFileName(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")), true);
                            }
                            Thread.Sleep(5000);
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
                                sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")); ;
                            }
                            else
                            {
                                sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")); ;
                            }


                            bigImgPDF.Attributes.Add("src", sImgPath);

                        }
                    }
                }
                catch (Exception ex)
                {
                    string sErrorMessage = "";
                    if (UtilityManager.CheckFileNotFoundException(ex, out sErrorMessage))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\" " + sErrorMessage + "\");", true);
                    }
                    else
                    {
                        //Jira #CAP-67 
                        if (iTryCount <= 3)
                        {
                            iTryCount = iTryCount + 1;
                            Thread.Sleep(1500);
                            goto TryAgain;
                        }
                        else
                        {
                            UtilityManager.RetryExecptionLog(ex, iTryCount);
                            throw (ex);
                        }
                    }
                }


                _imgBig.Src = "";
                bigImgPDF.Style.Add("display", "block");
                _imgBig.Style.Add("display", "none");
                PDFholder.Style.Add("display", "block");
                imgholder.Style.Add("display", "none");


                ftpServerIP = System.Configuration.ConfigurationManager.AppSettings["ftpServerIP"];
                UNCPath = System.Configuration.ConfigurationManager.AppSettings["UNCPath"];
                file_path = (string)Session["ExamData"];
                string _fileName = file_path.Replace(ftpServerIP, UNCPath).Replace(@"/", @"\");
                string[] fileGroups = _fileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (fileGroups.Count() > 1)
                {
                    OpenImageExam(fileGroups);
                }
                else
                {
                    OpenImageExam(fileGroups[0]);
                }
                divrotate.Style.Add("display", "none");
                PageBox.Value = hdnPageBox.Value;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "pdfchange", "pdfchangeview();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "examLoadChange", " { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Noates", "setNotesValue();", true);
                // 

            }
        }





        [WebMethod(EnableSession = true)]
        public static string RefreshNotes(string FileId)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            FileManagementIndexManager objFileManagementIndexManager = new FileManagementIndexManager();
            FileManagementIndex objFileManagementIndex = new FileManagementIndex();
            IList<FileManagementIndex> FileManagementIndexList = new List<FileManagementIndex>();
            IList<FileManagementIndex> templist = new List<FileManagementIndex>();
            objFileManagementIndex = objFileManagementIndexManager.GetById(Convert.ToUInt64(FileId));
            string NotesFOrmat = "";//MessageHistory + "<br />@" + ClientSession.UserName + "(" + Convert.ToDateTime(DateTime).ToString("dd-MMM-yyyy hh:mm tt") + "): " + Notes;
            if (objFileManagementIndex != null)
            {
                NotesFOrmat = objFileManagementIndex.Exam_Photos_Notes;// +"<br />@" + ClientSession.UserName + "(" + Convert.ToDateTime(DateTime).ToString("dd-MMM-yyyy hh:mm tt") + "): " + Notes;



            }
            //  if (FileManagementIndexList.Count>0)
            //objFileManagementIndexManager.SaveUpdateDeleteWithTransaction(ref templist, FileManagementIndexList, null, string.Empty);

            return NotesFOrmat;
        }

        [WebMethod(EnableSession = true)]
        public static string SaveNotes(string MessageHistory, string DateTime, string FileId, string Notes)
        {
            if (ClientSession.UserName != null && ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            FileManagementIndexManager objFileManagementIndexManager = new FileManagementIndexManager();
            FileManagementIndex objFileManagementIndex = new FileManagementIndex();
            IList<FileManagementIndex> FileManagementIndexList = new List<FileManagementIndex>();
            IList<FileManagementIndex> templist = new List<FileManagementIndex>();
            objFileManagementIndex = objFileManagementIndexManager.GetById(Convert.ToUInt64(FileId));
            string NotesFOrmat = MessageHistory + "<br />@" + ClientSession.UserName + "(" + Convert.ToDateTime(DateTime).ToString("dd-MMM-yyyy hh:mm tt") + "): " + Notes;
            if (objFileManagementIndex != null)
            {
                NotesFOrmat = "@" + ClientSession.UserName + "(" + Convert.ToDateTime(DateTime).ToString("dd-MMM-yyyy hh:mm tt") + "): " + Notes + "<br />" + objFileManagementIndex.Exam_Photos_Notes;

                objFileManagementIndex.Exam_Photos_Notes = NotesFOrmat;

                objFileManagementIndex.Modified_By = ClientSession.UserName;
                objFileManagementIndex.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                FileManagementIndexList.Add(objFileManagementIndex);

            }
            if (FileManagementIndexList.Count > 0)
            {
                objFileManagementIndexManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref templist, ref FileManagementIndexList, null, string.Empty, true, true, FileManagementIndexList[0].Human_ID, string.Empty);
                //BugId:53173
                templist = objFileManagementIndexManager.GetListbySourceAndHumanId(FileManagementIndexList[0].Human_ID, "EXAM");
                HttpContext.Current.Session.Add("ExamList", templist);
            }
            //objFileManagementIndexManager.SaveUpdateDeleteWithTransaction(ref templist, FileManagementIndexList, null, string.Empty);

            //  templist = (IList<FileManagementIndex>)(HttpContext.Current.Session["ExamList"]);

            return NotesFOrmat;
        }
        public void OpenImageExam(string filename)
        {

            try
            {
                OpenFileExam((filename));
                PageBox.Value = "1";
            }
            catch
            {
                /*File Not Available Handled in the UI Part*/
            }

        }

        public void OpenImageExam(string[] filenameGroups)
        {

            try
            {
                OpenFileExam(filenameGroups);
                //PageBox.Value = "1";
            }
            catch
            {
                /*File Not Available Handled in the UI Part*/
            }

        }
        public void OpenImage(string filename)
        {

            try
            {
                OpenFile((filename));
                PageBox.Value = "1";
            }
            catch
            {
                /*File Not Available Handled in the UI Part*/
            }

        }

        public void OpenImage(string[] filenameGroups)
        {

            try
            {
                OpenFile(filenameGroups);
                PageBox.Value = "1";
            }
            catch
            {
                /*File Not Available Handled in the UI Part*/
            }

        }

        #region "Image Operations"

        public void OpenFile(string filename)
        {
            System.Web.UI.HtmlControls.HtmlMeta META = new System.Web.UI.HtmlControls.HtmlMeta();
            META.HttpEquiv = "Pragma";
            META.Content = "no-cache";
            Page.Header.Controls.Add(META);
            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            hdnPageBox.Value = Convert.ToString("1");

            //Get FilePath
            // FilePath = Request.QueryString["File"];
            FilePath = filename;

            if (FilePath != "")
            {
                //Determine Start/End Pages
                int StartPg = 1;
                if (Request.QueryString["StartPage"] != null)
                    StartPg = System.Convert.ToInt16(Request.QueryString["StartPage"]);
                int BigImgPg = StartPg;
                int EndPg = TotalTIFPgs;
                if (EndPg > TotalTIFPgs)
                    EndPg = TotalTIFPgs;

                PageLabel.Value = @" / " + EndPg.ToString();
                //Add/configure the thumbnails
                while (StartPg <= EndPg)
                {
                    Label lbl = new Label();
                    if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                    else lbl.Text = "&nbsp;";
                    // HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                    // imgcontainer.ID = "pg_" + StartPg.ToString();
                    // imgcontainer.Attributes["class"] = "thumbnail";
                    System.Web.UI.WebControls.Image Img = new System.Web.UI.WebControls.Image();
                    Img.CssClass = "lazy";
                    Img.Style.Add("height", "84px");
                    Img.Style.Add("width", "79px");
                    Img.ImageAlign = ImageAlign.Middle;
                    Img.Attributes.Add("onClick", "ChangePg(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + StartPg.ToString() + "');");
                    Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                    Img.Attributes.Add("class", "lazy");
                    Img.AlternateText = "Page_" + StartPg;
                    Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                    HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                    pgDiv.InnerText = StartPg.ToString();
                    pgDiv.Attributes["class"] = "imgPage";
                    //imgcontainer.Controls.Add(Img);
                    //imgcontainer.Controls.Add(pgDiv);
                    // _plcImgsThumbs.Controls.Add(imgcontainer);
                    //_plcImgsThumbs.Controls.Add(lbl);
                    StartPg++;
                }
                /* To load total no of page in the doument for the newly scanned files */
                Session["Page_Count"] = EndPg;
                //Bind big img
                _imgBig.Src = "ViewImg.aspx?View=1&FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();


                //Config actions

                //ConfigPagers
                //Config page 1 - whatever
                if ((TotalTIFPgs / PagerSize) >= 1)
                {
                    //HyperLink _hl = new HyperLink();
                    //Label lbl = new Label(); lbl.Text = "&nbsp;";
                    //if (Request.Url.ToString().IndexOf("&StartPage=") >= 0)
                    //    _hl.NavigateUrl = Request.Url.ToString().Substring(0, Request.Url.ToString().IndexOf("&StartPage=")) + "&StartPage=1";
                    //else
                    //    _hl.NavigateUrl = Request.Url.ToString() + "&StartPage=1";
                    //if ((1 + PagerSize) > TotalTIFPgs)
                    //    _hl.Text = "1-" + TotalTIFPgs;
                    //else
                    //    _hl.Text = "1-" + PagerSize;
                    //_plcImgsThumbsPager.Controls.Add(_hl);
                    //_plcImgsThumbsPager.Controls.Add(lbl);
                }
                //Config the rest of the page pagers
                for (int i = 1; i <= (TotalTIFPgs / PagerSize); i++)
                {
                    //HyperLink _hl = new HyperLink();
                    //Label lbl1 = new Label(); lbl1.Text = "&nbsp;";
                    //if (Request.Url.ToString().IndexOf("&StartPage=") >= 0)
                    //    _hl.NavigateUrl = Request.Url.ToString().Substring(0, Request.Url.ToString().IndexOf("&StartPage=")) + "&StartPage=" + ((i * PagerSize) + 1).ToString();
                    //else
                    //    _hl.NavigateUrl = Request.Url.ToString() + "&StartPage=" + ((i * PagerSize) + 1).ToString();
                    //if (i == (TotalTIFPgs / PagerSize))
                    //    _hl.Text = ((i * PagerSize) + 1).ToString() + "-" + TotalTIFPgs;
                    //else
                    //    _hl.Text = ((i * PagerSize) + 1).ToString() + "-" + (((i + 1) * PagerSize)).ToString();
                    //_plcImgsThumbsPager.Controls.Add(_hl);
                    //_plcImgsThumbsPager.Controls.Add(lbl1);
                }
            }
            else
            {
                Response.Write("Please provde a file path");
            }
        }

        public void OpenFileIndex(string filename)
        {
            System.Web.UI.HtmlControls.HtmlMeta META = new System.Web.UI.HtmlControls.HtmlMeta();
            META.HttpEquiv = "Pragma";
            META.Content = "no-cache";
            Page.Header.Controls.Add(META);
            Response.Expires = -1;
            Response.CacheControl = "no-cache";
            hdnPageBox.Value = Convert.ToString("1");
            FilePath = filename;
            string file = Path.GetExtension(FilePath);

            if (FilePath != "")
            {
                int StartPg = 1;
                int EndPg = 0;
                if (Request.QueryString["StartPage"] != null)
                    StartPg = System.Convert.ToInt16(Request.QueryString["StartPage"]);

                if (file.ToUpper() != ".PDF")
                {
                    FilePath = filename.Replace(ftpServerIP, UNCPath);

                    PageBox.Value = StartPg.ToString();
                    int BigImgPg = StartPg;
                    //EndPg = TotalTIFPgs;
                    //if (EndPg > TotalTIFPgs)
                    //    EndPg = TotalTIFPgs;
                    //try
                    //{
                    //    using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(FilePath.ToString()))
                    //    {
                    //        EndPg = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                    //        imgbg.Dispose();
                    //    }
                    //}
                    //catch
                    //{
                    if (file.ToUpper() == ".TIF" || file.ToUpper() == ".TIFF")
                    {
                        try
                        {
                            using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(FilePath.ToString()))
                            {
                                EndPg = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                                imgbg.Dispose();
                            }
                        }
                        catch(Exception ex)
                        {
                            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "FrmImagviewer Line No - 1385 - FilePath - " + FilePath+" - " + ex.Message, DateTime.Now, "0", "frmimageviewer");
                           
                            EndPg = TotalTIFPgs;
                            if (EndPg > TotalTIFPgs)
                                EndPg = TotalTIFPgs;
                        }
                    }
                    else
                    {
                        EndPg = 1;
                    }
                    //}



                    PageLabel.Value = @" / " + EndPg.ToString();
                    while (StartPg <= EndPg)
                    {
                        //FilePath=FilePath.Replace("%", "%25").Replace("#", "%23").Replace("<", "%3C").Replace(">", "%3E").Replace("|", "%7C"); 
                        Label lbl = new Label();
                        if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                        else lbl.Text = "&nbsp;";
                        // HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                        //imgcontainer.ID = "pg_" + StartPg.ToString();
                        // imgcontainer.Attributes["class"] = "thumbnail";
                        System.Web.UI.WebControls.Image Img = new System.Web.UI.WebControls.Image();
                        Img.CssClass = "lazy";
                        Img.Style.Add("height", "84px");
                        Img.Style.Add("width", "79px");
                        Img.ImageAlign = ImageAlign.Middle;
                        Img.Attributes.Add("onClick", "ChangePg(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + StartPg.ToString() + "');");
                        Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                        Img.Attributes.Add("class", "lazy");
                        Img.AlternateText = "Page_" + StartPg;
                        Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                        HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                        pgDiv.InnerText = StartPg.ToString();
                        pgDiv.Attributes["class"] = "imgPage";
                        //  imgcontainer.Controls.Add(Img);
                        // imgcontainer.Controls.Add(pgDiv);
                        // _plcImgsThumbs.Controls.Add(imgcontainer);
                        // _plcImgsThumbs.Controls.Add(lbl);
                        StartPg++;
                    }
                    _imgBig.Src = "ViewImg.aspx?View=1&FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();
                }
                else
                {
                    string sImgPath = string.Empty;
                    if (source.ToUpper() != "RESULT")
                    {
                        Uri CurrentURL = new Uri(Request.Url.ToString());
                        string sProjectName = string.Empty;

                        //for (int i = 0; i < CurrentURL.Segments.Length - 1; i++)
                        //{
                        //    if (CurrentURL.Segments[i] != "/" && CurrentURL.Segments[i] != "//" && CurrentURL.Segments[i].StartsWith("frm") != true && sProjectName == string.Empty)
                        //    {
                        //        sProjectName = CurrentURL.Segments[i];
                        //    }
                        //}
                        //if (sProjectName != string.Empty)
                        //{
                        //    sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-upload//Indexing_Files//" + ClientSession.FacilityName + "//" + ClientSession.HumanId + "//" + Path.GetFileName(FilePath); ;
                        //}
                        //else
                        //{
                        //    sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-upload//Indexing_Files//" + ClientSession.FacilityName + "//" + ClientSession.HumanId + "//" + Path.GetFileName(FilePath); ;
                        //}

                        //Uri myuri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                        //string sImgPath = myuri.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-upload//Indexing_Files//" + ClientSession.FacilityName + "//" + ClientSession.HumanId + "//" + Path.GetFileName(FilePath);

                        // sImgPath = ConfigurationManager.AppSettings["IndexingTempPDfUrl"] + "//atala-capture-upload//Indexing_Files//" + ClientSession.FacilityName + "//" + ClientSession.HumanId + "//" + Path.GetFileName(filename);
                        filename = filename.Replace(ConfigurationManager.AppSettings["ScanningPath_Local"], ConfigurationManager.AppSettings["ScanningPathUrl"]);
                        sImgPath = filename.Replace("%", "%25").Replace("#", "%23").Replace("<", "%3C").Replace(">", "%3E").Replace("|", "%7C");
                        PDFholder.Style.Add("height", "585px !important");
                        PDFholder.Style.Add("width", "1219px !important");
                        PDFholder.Style.Add("margin-left", "-12%!important");
                        bigImgPDF.Attributes.Add("src", sImgPath);
                    }
                    else
                    {
                        //Open Patient chart view
                        string uri = FilePath;//Request.QueryString["FilePath"];
                        string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                        string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                        string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                        string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                        string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                        string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
                        //Jira #CAP-67 
                        int iTryCount = 1;
                    TryAgain:
                        try
                        {
                            using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                            {
                                if (unc.NetUseWithCredentials(UNCAuthPath, userName, domain, password))
                                {
                                    var bytes = System.IO.File.ReadAllBytes(uri.Replace(ftpIP, UNCPath));
                                    Response.ContentType = "application/pdf";
                                    Response.AddHeader("Content-disposition", "filename=" + Path.GetFileName(uri.Replace(ftpIP, UNCPath)));
                                    Response.BinaryWrite(bytes.ToArray());
                                    bigImgPDF.Attributes.Add("src", bytes.ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string sErrorMessage = "";
                            if (UtilityManager.CheckFileNotFoundException(ex, out sErrorMessage))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\" " + sErrorMessage + "\");", true);
                            }
                            else
                            {
                                //Jira #CAP-67 
                                if (iTryCount <= 3)
                                {
                                    iTryCount = iTryCount + 1;
                                    Thread.Sleep(1500);
                                    goto TryAgain;
                                }
                                else
                                {
                                    UtilityManager.RetryExecptionLog(ex, iTryCount);
                                    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "FrmImagviewer Line No - 1490 - " + ex.Message + " - Username is " + userName + " -  Password " + password + " - UNCAuthPath " + UNCAuthPath + " - UNCPAth" + UNCPath + "-URI " + uri.Replace(ftpIP, UNCPath), DateTime.Now, "0", "frmimageviewer");
                                    throw (ex);
                                }
                            }
                        }

                        bigImgPDF.Attributes.Add("src", FilePath);
                    }
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ViewPDFfile", "ViewPDF();", true);
                }
                Session["Page_Count"] = EndPg;
            }
            else
            {
                Response.Write("Please provde a file path");
            }
        }

        public int TotalTIFPgs
        {
            get
            {
                if (ViewState["TotalTIFPgs"] == null)
                {
                    TIF TheFile = new TIF(FilePath);
                    if(TheFile!=null)
                        ViewState["TotalTIFPgs"] = TheFile.PageCount;
                    TheFile.Dispose();
                }
                return System.Convert.ToInt16(ViewState["TotalTIFPgs"]);
            }
            set
            {
                ViewState["TotalTIFPgs"] = value;
            }
        }

        public String FilePath
        {
            get
            {
                if (ViewState["FilePath"] == null)
                {
                    return "";
                }
                return ViewState["FilePath"].ToString();
            }
            set
            {
                ViewState["FilePath"] = value;
            }
        }


        public void OpenFile(string[] fileGroups)
        {
            System.Web.UI.HtmlControls.HtmlMeta META = new System.Web.UI.HtmlControls.HtmlMeta();
            META.HttpEquiv = "Pragma";
            META.Content = "no-cache";
            Page.Header.Controls.Add(META);
            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            hdnPageBox.Value = Convert.ToString("1");
            int BigImgPg = 1;


            int GroupCount = 0;
            foreach (string file in fileGroups)
            {
                FilePath = file;

                if (FilePath != "")
                {

                    //Determine Start/End Pages
                    int StartPg = 1;
                    if (Request.QueryString["StartPage"] != null)
                        StartPg = System.Convert.ToInt16(Request.QueryString["StartPage"]);

                    int EndPg = TotalTIFPgs;
                    if (EndPg > TotalTIFPgs)
                        EndPg = TotalTIFPgs;

                    // PageLabel.Value = @" / " + EndPg.ToString();
                    //Add/configure the thumbnails
                    while (StartPg <= EndPg)
                    {
                        GroupCount++;
                        Label lbl = new Label();
                        if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                        else lbl.Text = "&nbsp;";
                        //HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                        //imgcontainer.ID = "pg_" + GroupCount.ToString();
                        //imgcontainer.Attributes["class"] = "thumbnail";
                        System.Web.UI.WebControls.Image Img = new System.Web.UI.WebControls.Image();
                        Img.CssClass = "lazy";
                        Img.Style.Add("height", "84px");
                        Img.Style.Add("width", "79px");
                        Img.ImageAlign = ImageAlign.Middle;
                        Img.Attributes.Add("onClick", "ChangePg(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + GroupCount.ToString() + "');");
                        Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                        Img.Attributes.Add("class", "lazy");
                        Img.AlternateText = "Page_" + GroupCount;
                        Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                        HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                        pgDiv.InnerText = GroupCount.ToString();
                        pgDiv.Attributes["class"] = "imgPage";
                        // imgcontainer.Controls.Add(Img);
                        // imgcontainer.Controls.Add(pgDiv);
                        // _plcImgsThumbs.Controls.Add(imgcontainer);
                        // _plcImgsThumbs.Controls.Add(lbl);
                        StartPg++;
                    }


                    /* To load total no of page in the doument for the newly scanned files */
                    Session["Page_Count"] = EndPg;



                }

            }
            PageLabel.Value = @" / " + GroupCount.ToString();
            _imgBig.Src = "ViewImg.aspx?View=1&FilePath=" + HttpUtility.UrlEncode(fileGroups[0]) + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();

        }
        #endregion
        // public void btnhiddenEFax_click(object sender, EventArgs e)
        //{
        //    var ActivityId = Request.QueryString["ActivityId"].ToString();
        //    ActivityLogManager ActivitylogMngr = new ActivityLogManager();
        //    IList<ActivityLog> UpdateActivityLogList = new List<ActivityLog>();
        //    UpdateActivityLogList = ActivitylogMngr.GetFaxActivity(ActivityId);
        //    Session["EFAXData"] = UpdateActivityLogList[0].Fax_File_Name;
        //}

        public void OpenFileFax(string[] fileGroups)
        {
            System.Web.UI.HtmlControls.HtmlMeta META = new System.Web.UI.HtmlControls.HtmlMeta();
            META.HttpEquiv = "Pragma";
            META.Content = "no-cache";
            Page.Header.Controls.Add(META);
            Response.Expires = -1;
            Response.CacheControl = "no-cache";
            int BigImgPg = 1;
            int GroupCount = 0;
            //_plcImgsThumbs.Controls.Clear();
            for (int i = 0; i < fileGroups.Length; i++)
            {
                int StartPg = 1;
                if (Request.QueryString["StartPage"] != null)
                    StartPg = System.Convert.ToInt16(Request.QueryString["StartPage"]);

                FilePath = fileGroups[i];
                if (FilePath != "")
                {
                    if (!(FilePath.ToUpper().Contains(".PDF")))
                    {
                        //Add/configure the thumbnails
                        int EndPg = TotalTIFPgs;
                        if (EndPg > TotalTIFPgs)
                            EndPg = TotalTIFPgs;
                        while (StartPg <= EndPg)
                        {
                            GroupCount++;
                            Label lbl = new Label();
                            if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                            else lbl.Text = "&nbsp;";
                            // HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                            // imgcontainer.ID = "pg_" + GroupCount.ToString();
                            // imgcontainer.Attributes["class"] = "thumbnail";
                            System.Web.UI.WebControls.Image Img = new System.Web.UI.WebControls.Image();
                            Img.CssClass = "lazy";
                            Img.Style.Add("height", "84px");
                            Img.Style.Add("width", "79px");
                            Img.ImageAlign = ImageAlign.Middle;
                            Img.Attributes.Add("onClick", "ChangePgfax(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + GroupCount.ToString() + "','" + "" + "','" + 0 + "','" + (i + 1).ToString() + "');");
                            Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                            Img.Attributes.Add("class", "lazy");
                            Img.AlternateText = "Page_" + GroupCount;
                            Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                            Img.Attributes.Add("src", HttpUtility.UrlEncode(FilePath));
                            HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                            pgDiv.InnerText = GroupCount.ToString();
                            pgDiv.Attributes["class"] = "imgPage";
                            // imgcontainer.Controls.Add(Img);
                            // imgcontainer.Controls.Add(pgDiv);
                            divrotate.Style.Add("display", "inline-block");
                            // _plcImgsThumbs.Controls.Add(imgcontainer);
                            //  _plcImgsThumbs.Controls.Add(lbl);
                            StartPg++;
                        }
                        /* To load total no of page in the doument for the newly scanned files */
                        Session["Page_Count"] = EndPg;
                    }
                    else
                    {
                        GroupCount++;
                        Label lbl = new Label();
                        if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                        else lbl.Text = "&nbsp;";
                        //  HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                        //  imgcontainer.ID = "pg_" + GroupCount.ToString();
                        //  imgcontainer.Attributes["class"] = "thumbnail";
                        System.Web.UI.WebControls.Label Img = new System.Web.UI.WebControls.Label();
                        Img.CssClass = "lazy";
                        Img.Style.Add("height", "100px");
                        Img.Style.Add("width", "55px");
                        Img.Style.Add("align", "center");// = ImageAlign.Middle;
                        //imgcontainer.Attributes.Add("onClick", "ChangePg(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + GroupCount.ToString() + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','" + (i + 1).ToString() + "');");
                        //imgcontainer.Attributes.Add("onClick", "ChangePgfax(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + GroupCount.ToString() + "','" + "" + "','" + "" + "','" + (i + 1).ToString() + "');");
                        Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                        Img.Attributes.Add("class", "lazy");
                        Img.ToolTip = Path.GetFileName(FilePath);
                        Img.Style.Add("font-size", "15px");
                        Img.Style.Add("word-break", "break-all");
                        Img.Style.Add("font-family", "Times New Roman");
                        //Img.Text = Path.GetFileName(FilePath).Substring(0, 40); 
                        Img.Text = Path.GetFileName(FilePath).Replace(".pdf", "");
                        HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                        pgDiv.InnerText = GroupCount.ToString();
                        pgDiv.Attributes["class"] = "imgPage";
                        //  imgcontainer.Controls.Add(Img);
                        //  imgcontainer.Controls.Add(pgDiv);
                        //  _plcImgsThumbs.Controls.Add(imgcontainer);
                        //  _plcImgsThumbs.Controls.Add(lbl);
                        StartPg++;
                    }

                }

            }

            PageLabel.Value = @" / " + GroupCount.ToString();
            if (flag == 0)
            {
                PageBox.Value = "1";
                hdnPageBox.Value = Convert.ToString("1");
                if (!(fileGroups[0].ToUpper().Contains(".PDF")))
                {
                    divrotate.Style.Add("display", "inline-block");
                    _imgBig.Src = "ViewImg.aspx?View=1&FilePath=" + HttpUtility.UrlEncode(fileGroups[0]) + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();
                    bigImgPDF.Style.Add("display", "none");
                    _imgBig.Style.Add("display", "block");
                    PDFholder.Style.Add("display", "none");
                    imgholder.Style.Add("display", "block");
                }
                else
                {

                    divrotate.Style.Add("display", "none");
                    string uri = fileGroups[0];//Request.QueryString["FilePath"];
                    string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                    string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                    string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                    string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                    string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                    string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
                    //Jira #CAP-67 
                    int iTryCount = 1;
                TryAgain:
                    try
                    {
                        using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                        {
                            if (unc.NetUseWithCredentials(UNCAuthPath, userName, domain, password))
                            {

                                DirectoryInfo DirWaitFolder = new DirectoryInfo(Server.MapPath("~/atala-capture-download/" + Session.SessionID + "//ExamPDF"));
                                if (!DirWaitFolder.Exists)
                                    Directory.CreateDirectory(DirWaitFolder.FullName);

                                if (File.Exists(fileGroups[0]) == true)
                                {

                                    if (File.Exists(DirWaitFolder.FullName + "\\" + Path.GetFileName(fileGroups[0])) == true)
                                    {
                                        File.Delete(DirWaitFolder.FullName + "\\" + Path.GetFileName(fileGroups[0]));
                                    }

                                    File.Copy(fileGroups[0], DirWaitFolder.FullName + "\\" + Path.GetFileName(fileGroups[0]), true);
                                }
                                Thread.Sleep(5000);
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
                                    sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(fileGroups[0]); ;
                                }
                                else
                                {
                                    sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(fileGroups[0]); ;
                                }
                                bigImgPDF.Attributes.Add("src", sImgPath);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string sErrorMessage = "";
                        if (UtilityManager.CheckFileNotFoundException(ex, out sErrorMessage))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\" " + sErrorMessage + "\");", true);
                        }
                        else
                        {
                            //Jira #CAP-67 
                            if (iTryCount <= 3)
                            {
                                iTryCount = iTryCount + 1;
                                Thread.Sleep(1500);
                                goto TryAgain;
                            }
                            else
                            {
                                UtilityManager.RetryExecptionLog(ex, iTryCount);
                                throw (ex);
                            }
                        }
                    }

                    _imgBig.Src = "";
                    bigImgPDF.Style.Add("display", "block");
                    _imgBig.Style.Add("display", "none");
                    PDFholder.Style.Add("display", "block");
                    imgholder.Style.Add("display", "none");


                }

            }

        }
        public void OpenFileFax(string filename)
        {
            System.Web.UI.HtmlControls.HtmlMeta META = new System.Web.UI.HtmlControls.HtmlMeta();
            META.HttpEquiv = "Pragma";
            META.Content = "no-cache";
            Page.Header.Controls.Add(META);
            Response.Expires = -1;
            Response.CacheControl = "no-cache";
            hdnPageBox.Value = Convert.ToString("1");
            //Get FilePath          
            FilePath = filename;
            // _plcImgsThumbs.Controls.Clear();
            if (FilePath != "")
            {
                //Determine Start/End Pages
                int StartPg = 1;
                if (Request.QueryString["StartPage"] != null)
                    StartPg = System.Convert.ToInt16(Request.QueryString["StartPage"]);

                PageLabel.Value = @" / " + 1.ToString();

                //Add/configure the thumbnails
                if (!(FilePath.ToUpper().Contains(".PDF")))
                {
                    int BigImgPg = StartPg;
                    int EndPg = TotalTIFPgs;
                    if (EndPg > TotalTIFPgs)
                        EndPg = TotalTIFPgs;
                    while (StartPg <= EndPg)
                    {
                        Label lbl = new Label();
                        if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                        else lbl.Text = "&nbsp;";
                        //HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                        //imgcontainer.ID = "pg_" + StartPg.ToString();
                        //imgcontainer.Attributes["class"] = "thumbnail";
                        System.Web.UI.WebControls.Image Img = new System.Web.UI.WebControls.Image();
                        Img.CssClass = "lazy";
                        Img.Style.Add("height", "84px");
                        Img.Style.Add("width", "79px");
                        Img.ImageAlign = ImageAlign.Middle;
                        Img.Attributes.Add("onClick", "ChangePgfax(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + StartPg.ToString() + "','" + "" + "','" + "" + "','1');");
                        Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                        Img.Attributes.Add("class", "lazy");
                        Img.ToolTip = FilePath;
                        Img.AlternateText = "Page_" + StartPg;
                        Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                        HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                        pgDiv.InnerText = StartPg.ToString();
                        pgDiv.Attributes["class"] = "imgPageCompare";
                        // imgcontainer.Controls.Add(Img);
                        // imgcontainer.Controls.Add(pgDiv);
                        // _plcImgsThumbs.Controls.Add(imgcontainer);
                        divrotate.Style.Add("display", "inline-block");
                        // _plcImgsThumbs.Controls.Add(lbl);
                        StartPg++;
                    }
                    /* To load total no of page in the doument for the newly scanned files */
                    Session["Page_Count"] = EndPg;
                    //Bind big img                   
                    _imgBig.Src = "ViewImg.aspx?View=1&FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();
                    bigImgPDF.Style.Add("display", "none");
                    _imgBig.Style.Add("display", "block");
                    PDFholder.Style.Add("display", "none");
                    imgholder.Style.Add("display", "block");
                }
                else
                {

                    Label lbl = new Label();
                    if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                    else lbl.Text = "&nbsp;";
                    //HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                    // imgcontainer.ID = "pg_" + StartPg.ToString();
                    // imgcontainer.Attributes["class"] = "thumbnail";
                    System.Web.UI.WebControls.Label Img = new System.Web.UI.WebControls.Label();
                    Img.CssClass = "lazy";
                    Img.Style.Add("height", "84px");
                    Img.Style.Add("width", "50px");
                    Img.Style.Add("align", "center");// = ImageAlign.Middle;
                    Img.Attributes.Add("onClick", "ChangePgfax(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pg_" + StartPg.ToString() + "','" + "" + "','" + "" + "','1');");
                    Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                    Img.Attributes.Add("class", "lazy");
                    if (Path.GetFileName(FilePath).Length > 40)
                        Img.Text = Path.GetFileName(FilePath).Substring(0, 40);
                    else
                        Img.Text = Path.GetFileName(FilePath);
                    Img.ToolTip = Path.GetFileName(FilePath);
                    Img.Style.Add("font-size", "15px");
                    Img.Style.Add("font-family", "Times New Roman");
                    Img.Style.Add("word-break", "break-all");
                    // Img.AlternateText = "Page_" + StartPg;                    
                    HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                    pgDiv.InnerText = StartPg.ToString();
                    pgDiv.Attributes["class"] = "imgPage";
                    // imgcontainer.Controls.Add(Img);
                    // imgcontainer.Controls.Add(pgDiv);
                    // _plcImgsThumbs.Controls.Add(imgcontainer);
                    // _plcImgsThumbs.Controls.Add(lbl);
                    divrotate.Style.Add("display", "none");
                    StartPg++;
                    string uri = FilePath;//Request.QueryString["FilePath"];
                    string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                    string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                    string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                    string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                    string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                    string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
                    //Jira #CAP-67 
                    int iTryCount = 1;
                TryAgain:
                    try
                    {
                        using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                        {
                            if (unc.NetUseWithCredentials(UNCAuthPath, userName, domain, password))
                            {

                                DirectoryInfo DirWaitFolder = new DirectoryInfo(Server.MapPath("~/atala-capture-download/" + Session.SessionID + "//ExamPDF"));
                                if (!DirWaitFolder.Exists)
                                    Directory.CreateDirectory(DirWaitFolder.FullName);

                                if (File.Exists(FilePath) == true)
                                {
                                    if (File.Exists(DirWaitFolder.FullName + "\\" + Path.GetFileName(FilePath)) == true)
                                    {
                                        File.Delete(DirWaitFolder.FullName + "\\" + Path.GetFileName(FilePath));
                                    }

                                    File.Copy(FilePath, DirWaitFolder.FullName + "\\" + Path.GetFileName(FilePath), true);
                                }
                                Thread.Sleep(5000);
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
                                    sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(FilePath); ;
                                }
                                else
                                {
                                    sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(FilePath); ;
                                }


                                bigImgPDF.Attributes.Add("src", sImgPath);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string sErrorMessage = "";
                        if (UtilityManager.CheckFileNotFoundException(ex, out sErrorMessage))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\" " + sErrorMessage + "\");", true);
                        }
                        else
                        {
                            //Jira #CAP-67 
                            if (iTryCount <= 3)
                            {
                                iTryCount = iTryCount + 1;
                                Thread.Sleep(1500);
                                goto TryAgain;
                            }
                            else
                            {
                                UtilityManager.RetryExecptionLog(ex, iTryCount);
                                throw (ex);
                            }
                        }
                    }
                    _imgBig.Src = "";
                    bigImgPDF.Style.Add("display", "block");
                    _imgBig.Style.Add("display", "none");
                    PDFholder.Style.Add("display", "block");
                    imgholder.Style.Add("display", "none");
                }
            }
            else
            {
                Response.Write("Please provde a file path");
            }
        }
        public void OpenImageEFax(string filename)
        {

            try
            {
                OpenFileFax((filename));
                PageBox.Value = "1";
            }
            catch
            {
                /*File Not Available Handled in the UI Part*/
            }

        }

        public void OpenImageEFax(string[] filenameGroups)
        {

            try
            {
                OpenFileFax(filenameGroups);
                //PageBox.Value = "1";
            }
            catch
            {
                /*File Not Available Handled in the UI Part*/
            }

        }


        protected void hdnDownloadbtn_Click(object sender, EventArgs e)
        {
            List<string> FilePaths = new List<string>();
            string sHumanID=string.Empty;
            
            FTPImageProcess _ftpImageProcess = new FTPImageProcess();
            string localPath = string.Empty;
            string ftpServerIP = string.Empty;
            string ftpUserName = string.Empty;
            string ftpPassword = string.Empty;
            string sFilePath = hdnFileName.Value.TrimEnd('|'); // Request.QueryString["FileName"].ToString().Replace("HASHSYMBOL", "#");
            

            localPath = System.Configuration.ConfigurationSettings.AppSettings["LocalPath"];
            ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
            ftpUserName = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
            ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];
            string slocalPath = Server.MapPath("atala-capture-download//" + Session.SessionID + "//Download_ImageViewer");
            DirectoryInfo virdir = new DirectoryInfo(slocalPath);
            if (!virdir.Exists)
            {
                virdir.Create();
            }
            FileInfo[] file = virdir.GetFiles();
            for (int i = 0; i < file.Length; i++)
            {
                File.Delete(file[i].FullName);
            }
            FTPImageProcess ftpImage = new FTPImageProcess();
            if (!sFilePath.Contains('|'))
            {
                if (ClientSession.HumanId != 0)
                {
                    sHumanID = ClientSession.HumanId.ToString();
                }
                else
                {
                    //Get HumanId from filePath
                    DirectoryInfo dir = new DirectoryInfo(sFilePath.Replace("ftp:", "").Replace("http:", ""));
                    sHumanID = dir.Parent.Name.ToString();
                }
                ftpImage.DownloadFromImageServer(sHumanID, ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(sFilePath), slocalPath, out string sCheckFileNotFoundException);
                if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                    return;
                }
                string sAtalaPath = Server.MapPath(@"atala-capture-download/" + Session.SessionID + "/Download_ImageViewer/" + Path.GetFileName(sFilePath));
                sAtalaPath = sAtalaPath.Replace(@"/", @"\").Replace(@"\\\\", @"\\").Replace(@"\\\", @"\\").Replace(@"\\", @"\");
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(sFilePath));
                Response.TransmitFile(sAtalaPath);
                Response.End();
            }
            else
            {
            //Exam Photos multiple file download
                string[] FileNames = sFilePath.Split('|');
                for (int i = 0; i < FileNames.Length; i++)
                {
                    if (i == 0)
                    {
                        if (ClientSession.HumanId != 0)
                        {
                            sHumanID = ClientSession.HumanId.ToString();
                        }
                        else
                        {
                            //Get HumanId from filePath
                            DirectoryInfo dir = new DirectoryInfo(FileNames[i].Replace("ftp:", "").Replace("http:", ""));
                            sHumanID = dir.Parent.Name.ToString();
                        }
                    }
                    ftpImage.DownloadFromImageServer(sHumanID, ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(FileNames[i]), slocalPath, out string sCheckFileNotFoundException);
                    if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                        return;
                    }
                    string orig_image = slocalPath + "\\" + Path.GetFileName(FileNames[i]);
                   
                    FilePaths.Add(orig_image);
                }
                using (ZipFile zip = new ZipFile())
                {
                    for (int i = 0; i < FilePaths.Count; i++)
                    {
                        zip.AddFile(FilePaths[i], "");
                    }
                    string fileName = DropDownimagelist.SelectedItem.Text.Split(',')[0].Trim().Replace(' ', '_') + "_" + DropDownimagelist.SelectedItem.Text.Split(',')[1].Split(':')[1].Trim() +"_"+ sHumanID + ".zip";
                    zip.Save(Path.Combine(virdir.FullName, fileName));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Open", "downloadURI('" + HttpUtility.UrlEncode(Page.ResolveClientUrl("atala-capture-download//" + Session.SessionID + "//Download_ImageViewer//" + fileName)) + "','" + fileName + "');", true);
                    //Response.Clear();
                    //Response.AppendHeader("Content-Disposition", "attachment;filename=" + hdnDocumentType.Value + "_" + hdnDate.Value + "(" + ClientSession.HumanId + ").zip");
                    //Response.ContentType = "application/zip";
                    //zip.Save(Response.OutputStream);
                    //Response.Flush();
                    //Response.End();
                }
            }
        }
    }
}
