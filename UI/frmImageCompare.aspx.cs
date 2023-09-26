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
using DocumentFormat.OpenXml.Wordprocessing;

namespace Acurus.Capella.UI
{
    public partial class frmImageCompare : System.Web.UI.Page
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
        int flag = 0;
        string docphy = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            /*To Avoid the file duplication in the local files and user specific temp folders*/
            if (!IsPostBack)
            {
                hdnpdfnotes.Value ="";
                hdnpdf.Value = "";
                docphy = Request.QueryString["DocPhy"].ToString();
                DocType = Request.QueryString["DocType"].ToString();
                DocDate = Request.QueryString["DocDate"].ToString();
                //  lblactimage.Text = DocType + "-" + DocDate;
                LoadCombo();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "exam", "setNotesValue();", true);
            }
         

                localPath = Server.MapPath(@"atala-capture-download/" + Session.SessionID);

                /*To check the existance for the cache folder */
                DirectoryInfo drCache = new DirectoryInfo(localPath);
                if (!drCache.Exists)
                { drCache.Create(); }

                ftpServerIP = System.Configuration.ConfigurationManager.AppSettings["ftpServerIP"];
                UNCPath = System.Configuration.ConfigurationManager.AppSettings["UNCPath"];
                source = Request.QueryString["Source"].ToString();
                switch (source.ToUpper())
                {


                    #region "Examination Photos"
                    case "EXAM":
                        {
                            btnsave.Attributes.Add("onclick", " return btnsaveClickCompare()");
                            trbuttons.Visible = true;
                            trmesshistory.Visible = true;
                            trmessage.Visible = true;
                            file_path = (string)Session["ExamData"];
                            string _fileName = file_path.Replace(ftpServerIP, UNCPath).Replace(@"/", @"\");
                            string[] fileGroups = _fileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (fileGroups.Count() > 1)
                            {
                                OpenImageExam(fileGroups);
                            }
                            else
                            { OpenImageExam(fileGroups[0]); }




                            ScriptManager.RegisterStartupScript(this, this.GetType(), "examLoad", "{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); };", true);

                        }


                        break;

                    #endregion

                    #region "Others"
                    default:
                        break;
                    #endregion

                }
            
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
            _plcImgsThumbs.Controls.Clear();
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
                        HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                        imgcontainer.ID = "pgact_" + StartPg.ToString();
                        imgcontainer.Attributes["class"] = "thumbnailCompare";
                        System.Web.UI.WebControls.Image Img = new System.Web.UI.WebControls.Image();
                        Img.CssClass = "lazy";
                        Img.Style.Add("height", "84px");
                        Img.Style.Add("width", "50px");
                        Img.ImageAlign = ImageAlign.Middle;
                        Img.Attributes.Add("onClick", "ChangePgact(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pgact_" + StartPg.ToString() + "','" + file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','1');");
                        Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                        Img.Attributes.Add("class", "lazy");
                        Img.ToolTip = FilePath;
                        Img.AlternateText = "Page_" + StartPg;
                        Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                        HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                        pgDiv.InnerText = StartPg.ToString();
                        pgDiv.Attributes["class"] = "imgPageCompare";
                        imgcontainer.Controls.Add(Img);
                        imgcontainer.Controls.Add(pgDiv);
                        _plcImgsThumbs.Controls.Add(imgcontainer);
                        divrotate.Style.Add("display", "inline-block");
                        _plcImgsThumbs.Controls.Add(lbl);
                        StartPg++;
                    }
                    /* To load total no of page in the doument for the newly scanned files */
                    Session["Page_Count"] = EndPg;
                    //Bind big img
                    //  hdnnotes.Value = file_Notes.ToString().Split('^')[0].ToString();
                    // hdnfileindexid.Value = file_Notes.ToString().Split('^')[1].ToString();
                    hdnnotes.Value = file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0].ToString();
                    hdnfileindexid.Value = file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1].ToString();
                    if (flag == 0)
                    {
                        _imgBigActual.Src = "ViewImg.aspx?View=1&FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();
                        bigImgPDF.Style.Add("display", "none");
                        _imgBigActual.Style.Add("display", "block");
                        PDFholder.Style.Add("display", "none");
                        imgholder.Style.Add("display", "block");

                    }
                   
                }
                else
                {


                  //  while (StartPg <= EndPg)
                  //  {
                        Label lbl = new Label();
                        if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                        else lbl.Text = "&nbsp;";
                        HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                        imgcontainer.ID = "pgact_" + StartPg.ToString();
                        imgcontainer.Attributes["class"] = "thumbnailCompare";
                        System.Web.UI.WebControls.Label Img = new System.Web.UI.WebControls.Label();
                        Img.CssClass = "lazy";
                        Img.Style.Add("height", "84px");
                        Img.Style.Add("width", "50px");
                        Img.Style.Add("align", "center");// = ImageAlign.Middle;
                        Img.Attributes.Add("onClick", "ChangePgact(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pgact_" + StartPg.ToString() + "','" + file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','1');");
                        Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                        Img.Attributes.Add("class", "lazy");
                        Img.Text = Path.GetFileName(FilePath).Substring(0,40);
                        Img.ToolTip = Path.GetFileName(FilePath);
                        Img.Style.Add("font-size", "11px");
                        Img.Style.Add("font-family", "Times New Roman");
                        Img.Style.Add("word-break", "break-all");
                        // Img.AlternateText = "Page_" + StartPg;
                        //Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                        HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                        pgDiv.InnerText = StartPg.ToString();
                        pgDiv.Attributes["class"] = "imgPageCompare";
                        imgcontainer.Controls.Add(Img);
                        imgcontainer.Controls.Add(pgDiv);
                        divrotate.Style.Add("display", "none");
                        _plcImgsThumbs.Controls.Add(imgcontainer);
                        _plcImgsThumbs.Controls.Add(lbl);
                        StartPg++;
                  //  }

                  //  Session["Page_Count"] = EndPg;
                    //Bind big img
                    //  hdnnotes.Value = file_Notes.ToString().Split('^')[0].ToString();
                    // hdnfileindexid.Value = file_Notes.ToString().Split('^')[1].ToString();
                    hdnnotes.Value = file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0].ToString();
                    hdnfileindexid.Value = file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1].ToString();
                    if (flag == 0)
                    {
                        string uri = FilePath;//Request.QueryString["FilePath"];
                        string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                        string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                        string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                        string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                        string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                        string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
                        //Jira #CAP-64
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
                                //Jira #CAP-64
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


                      
                        _imgBigActual.Src = "";
                        bigImgPDF.Style.Add("display", "block");
                        _imgBigActual.Style.Add("display", "none");
                        PDFholder.Style.Add("display", "block");
                        imgholder.Style.Add("display", "none");


                    }



                }

            }

            else
            {
                Response.Write("Please provde a file path");
            }
        }
        public void OpenFileExam(string[] fileGroups)
        {
            System.Web.UI.HtmlControls.HtmlMeta META = new System.Web.UI.HtmlControls.HtmlMeta();
            META.HttpEquiv = "Pragma";
            META.Content = "no-cache";
            Page.Header.Controls.Add(META);
            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            hdnPageBox.Value = Convert.ToString("1");
            int BigImgPg = 1;

            string file_Notes = (string)Session["NotesExam"];
            string[] fileNotes = file_Notes.Split("~".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int GroupCount = 0;
            // foreach (string file in fileGroups)
            _plcImgsThumbs.Controls.Clear();

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
                        int EndPg = TotalTIFPgs;
                        if (EndPg > TotalTIFPgs)
                            EndPg = TotalTIFPgs;
                        while (StartPg <= EndPg)
                        {
                            GroupCount++;
                            Label lbl = new Label();
                            if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                            else lbl.Text = "&nbsp;";
                            HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                            imgcontainer.ID = "pgact_" + GroupCount.ToString();
                            imgcontainer.Attributes["class"] = "thumbnailCompare";
                            System.Web.UI.WebControls.Image Img = new System.Web.UI.WebControls.Image();
                            Img.CssClass = "lazy";
                            Img.Style.Add("height", "84px");
                            Img.Style.Add("width", "55px");
                            Img.ImageAlign = ImageAlign.Middle;
                            Img.Attributes.Add("onClick", "ChangePgact(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pgact_" + GroupCount.ToString() + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','" + (i + 1).ToString() + "');");
                            Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                            Img.Attributes.Add("class", "lazy");
                            Img.AlternateText = "Page_" + GroupCount;
                            Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                            HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                            pgDiv.InnerText = GroupCount.ToString();
                            pgDiv.Attributes["class"] = "imgPageCompare";
                            imgcontainer.Controls.Add(Img);
                            imgcontainer.Controls.Add(pgDiv);
                            _plcImgsThumbs.Controls.Add(imgcontainer);
                            _plcImgsThumbs.Controls.Add(lbl);
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
                            HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                            imgcontainer.ID = "pgact_" + GroupCount.ToString();
                            imgcontainer.Attributes["class"] = "thumbnailCompare";
                            System.Web.UI.WebControls.Label Img = new System.Web.UI.WebControls.Label();
                            Img.CssClass = "lazy";
                            Img.Style.Add("height", "100px");
                            Img.Style.Add("width", "50px");
                            Img.Style.Add("align", "center");// = ImageAlign.Middle;
                            imgcontainer.Attributes.Add("onClick", "ChangePgact(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pgact_" + GroupCount.ToString() + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','" + (i + 1).ToString() + "');");
                            Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                            Img.Attributes.Add("class", "lazy");
                            Img.ToolTip = Path.GetFileName(FilePath);
                            Img.Style.Add("font-size", "11px");
                            Img.Style.Add("font-family", "Times New Roman");
                            Img.Style.Add("word-break", "break-all");
                            Img.Text = Path.GetFileName(FilePath).Substring(0, 40);
                            // Img.AlternateText = "Page_" + StartPg;
                            //Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                            HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                            pgDiv.InnerText = GroupCount.ToString();
                            pgDiv.Attributes["class"] = "imgPageCompare";
                            imgcontainer.Controls.Add(Img);
                            imgcontainer.Controls.Add(pgDiv);
                            _plcImgsThumbs.Controls.Add(imgcontainer);
                            _plcImgsThumbs.Controls.Add(lbl);
                            StartPg++;
                      //  }

                    }

                }

            }

            if (hdnpdfnotes.Value == "")
            {
                hdnnotes.Value = fileNotes[0].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0].ToString();
                hdnfileindexid.Value = fileNotes[0].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1].ToString();
            }
            PageLabel.Value = @" / " + GroupCount.ToString();

            if (!(fileGroups[0].ToUpper().Contains(".PDF")))
            {
                if (flag == 0)
                    _imgBigActual.Src = "ViewImg.aspx?View=1&FilePath=" + HttpUtility.UrlEncode(fileGroups[0]) + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();

                bigImgPDF.Style.Add("display", "none");
                _imgBigActual.Style.Add("display", "block");
                PDFholderComp.Style.Add("display", "none");
                divrotate.Style.Add("display", "inline-block");
                imgholdercom.Style.Add("display", "block");
            }
            else
            {
                if (flag == 0 && hdnpdfnotes.Value == "")
                {
                     string uri = fileGroups[0];//Request.QueryString["FilePath"];
                        string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                        string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                        string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                        string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                        string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                        string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];

                    //Jira #CAP-64
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
                            //Jira #CAP-64
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

                        divrotate.Style.Add("display", "none");
                      
                        _imgBigActual.Src = "";
                        bigImgPDF.Style.Add("display", "block");
                        _imgBigActual.Style.Add("display", "none");
                        PDFholder.Style.Add("display", "block");
                        imgholder.Style.Add("display", "none");


                    }


                   
                }

          

        }


        public void btnhidden_click(object sender, EventArgs e)
        {

            if (hdnpdf.Value != "")
            {
                flag = 2;
                string uri = hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " "); ;//Request.QueryString["FilePath"];
                string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
                //Jira #CAP-64
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
                            if (hdnpdf.Value != "")
                            {
                                if (File.Exists(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")) == true)
                                {

                                    if (File.Exists(DirWaitFolder.FullName + "\\" + Path.GetFileName(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " "))) == true)
                                    {
                                        File.Delete(DirWaitFolder.FullName + "\\" + Path.GetFileName(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")));
                                    }
                                    File.Copy(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " "), DirWaitFolder.FullName + "\\" + Path.GetFileName(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")), true);
                                }
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
                        //Jira #CAP-64
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
             
                divrotate.Style.Add("display", "none");
                PageBox.Value = hdnpgno.Value; // Convert.ToInt16(Path.GetFileName(FilePath).Split('_')[4].Split('.')[0]).ToString();
                _imgBigActual.Src = "";
                bigImgPDF.Style.Add("display", "block");
                _imgBigActual.Style.Add("display", "none");
                PDFholder.Style.Add("display", "block");
                imgholder.Style.Add("display", "none");


                _imgBigCompare.Src = hdnimagesource1.Value;



                if (hdnimagesource1.Value == "")
                {

                    bigImgPDFCompare.Style.Add("display", "block");
                    _imgBigCompare.Style.Add("display", "none");

                    PDFholderComp.Style.Add("display", "block");

                    imgholdercom.Style.Add("display", "none");
                }
                else
                {
                    bigImgPDFCompare.Style.Add("display", "none");
                    _imgBigCompare.Style.Add("display", "block");

                    PDFholderComp.Style.Add("display", "none");

                    imgholdercom.Style.Add("display", "block");
                }

                IList<FileManagementIndex> lstTemp = new List<FileManagementIndex>();
                lstTemp = (IList<FileManagementIndex>)Session["CompareDropdownExamList"];
                string target = "";
               string  Notes = "";
                if (lstTemp != null)
                {
                    foreach (FileManagementIndex item in lstTemp)
                    {
                        target += item.File_Path.ToString() + "|";
                        Notes += item.Exam_Photos_Notes.ToString() + "^$#@^" + item.Id.ToString() + "~";
                    }

                    file_path = target;
                    ftpServerIP = System.Configuration.ConfigurationManager.AppSettings["ftpServerIP"];
                    UNCPath = System.Configuration.ConfigurationManager.AppSettings["UNCPath"];
                    string _fileName = file_path.Replace(ftpServerIP, UNCPath).Replace(@"/", @"\");
                    string[] fileGroups = _fileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (fileGroups.Count() > 1)
                    {
                        OpenImageExamCompare(fileGroups);
                    }
                    else
                    { OpenImageExamCompare(fileGroups[0]); }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "pdfchange", "pdfchange();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "examNotes", "setNotesValueActual();setNotesValueCompare();", true);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "examLoadChange", " { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "pdfchange", "pdfchange();", true);
                
            }
        }




        public void btnhiddencompare_click (object sender, EventArgs e)
        {

            if (hdnpdfcompare.Value != "")
            {
                flag = 2;
                string uri = hdnpdfcompare.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " "); ;//Request.QueryString["FilePath"];
                string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
                //Jira #CAP-64
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
                            if (hdnpdfcompare.Value != "")
                            {
                                if (File.Exists(hdnpdfcompare.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")) == true)
                                {

                                    if (File.Exists(DirWaitFolder.FullName + "\\" + Path.GetFileName(hdnpdfcompare.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " "))) == true)
                                    {
                                        File.Delete(DirWaitFolder.FullName + "\\" + Path.GetFileName(hdnpdfcompare.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")));
                                    }
                                    File.Copy(hdnpdfcompare.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " "), DirWaitFolder.FullName + "\\" + Path.GetFileName(hdnpdfcompare.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")), true);
                                }
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
                                sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(hdnpdfcompare.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")) ;
                            }
                            else
                            {
                                sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(hdnpdfcompare.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")); ;
                            }


                     
                            bigImgPDFCompare.Attributes.Add("src", sImgPath);

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
                        //Jira #CAP-64
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


                PageBoxcom.Value = hdnpgno1.Value;

                divrotatecomp.Style.Add("display", "none");
                _imgBigCompare.Src = "";
                bigImgPDFCompare.Style.Add("display", "block");
                _imgBigCompare.Style.Add("display", "none");

                PDFholderComp.Style.Add("display", "block");
             
                imgholdercom.Style.Add("display", "none");
                IList<FileManagementIndex> lstTemp = new List<FileManagementIndex>();
                lstTemp = (IList<FileManagementIndex>)Session["CompareDropdownExamList"];
                string target = "";
                string Notes = "";

                _imgBigActual.Src = hdnimagesource.Value;



                if (hdnimagesource.Value == "")
                {


                    bigImgPDF.Style.Add("display", "block");
                    _imgBigActual.Style.Add("display", "none");

                    PDFholder.Style.Add("display", "block");

                    imgholder.Style.Add("display", "none");
                    if (hdnpdf.Value != "")
                    {
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
                            sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//" + sProjectName + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " "));
                        }
                        else
                        {
                            sImgPath = CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//atala-capture-download//" + Session.SessionID + "//ExamPDF//" + Path.GetFileName(hdnpdf.Value.Replace("sin", "\\").Replace("%5c", "//").Replace("+", " ")); ;
                        }



                        bigImgPDF.Attributes.Add("src", sImgPath);
                    }
                }
                else
                {
                    bigImgPDF.Style.Add("display", "none");
                    _imgBigActual.Style.Add("display", "block");

                    PDFholder.Style.Add("display", "none");
                    
                    imgholder.Style.Add("display", "block");
                   
                }


                if (lstTemp != null)
                {
                    foreach (FileManagementIndex item in lstTemp)
                    {
                        target += item.File_Path.ToString() + "|";
                        Notes += item.Exam_Photos_Notes.ToString() + "^$#@^" + item.Id.ToString() + "~";
                    }

                    file_path = target;
                    ftpServerIP = System.Configuration.ConfigurationManager.AppSettings["ftpServerIP"];
                    UNCPath = System.Configuration.ConfigurationManager.AppSettings["UNCPath"];
                    string _fileName = file_path.Replace(ftpServerIP, UNCPath).Replace(@"/", @"\");
                    string[] fileGroups = _fileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (fileGroups.Count() > 1)
                    {
                        OpenImageExamCompare(fileGroups);
                    }
                    else
                    { OpenImageExamCompare(fileGroups[0]); }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "pdfchange", "pdfchangecompare();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "examNotes", "setNotesValueActual();setNotesValueCompare();", true);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "pdfchange", "pdfchangecompare();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "pdfchange", "pdfchange();", true);

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
                // objFileManagementIndexManager.SaveUpdateDeleteWithTransaction(ref templist, FileManagementIndexList, null, string.Empty);
                // bugId:53173
                templist = objFileManagementIndexManager.GetListbySourceAndHumanId(FileManagementIndexList[0].Human_ID, "EXAM");
                HttpContext.Current.Session.Add("ExamList", templist);
            }
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
                _imgBigActual.Attributes.Add("alt", "Uploaded File Not available in the location, it may be deleted or corrupted.Please Contact Support.");
                /*File Not Available Handled in the UI Part*/
            }

        }

        public void OpenImageExam(string[] filenameGroups)
        {

            try
            {
                OpenFileExam(filenameGroups);
                PageBox.Value = "1";
            }
            catch
            {
                _imgBigActual.Attributes.Add("alt", "Uploaded File Not available in the location, it may be deleted or corrupted.Please Contact Support.");                /*File Not Available Handled in the UI Part*/
            }

        }




        public int TotalTIFPgs
        {
            get
            {
                if (ViewState["TotalTIFPgs"] == null)
                {
                    TIF TheFile = new TIF(FilePath);
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

        public void LoadCombo()
        {

            DropDownimagelist.Attributes.Add("onmouseover", "return DropDownimagelist_mouse();");
            droplistfile.Attributes.Add("onmouseover", "return droplistfile_mouse();");
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

                droplistfile.Items.Add("--Select Image to Compare--");
                //   DropDownimagelist.Items.Add("--Select Image to Compare--");











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
                                droplistfile.Items.Add(lstExamdate[n] + " , Date of Exam : " + UtilityManager.ConvertToLocal(testtaken).ToString("dd-MMM-yyyy") + "," + name);
                                DropDownimagelist.Items.Add(lstExamdate[n] + " , Date of Exam : " + UtilityManager.ConvertToLocal(testtaken).ToString("dd-MMM-yyyy") + "," + name);

                                if (lstExamdate[n].ToUpper() == DocType.ToUpper() && UtilityManager.ConvertToLocal(testtaken).ToString("dd-MMM-yyyy") == DocDate && file_management_list[0].Appointment_Provider_ID.ToString() == docphy)
                                    DropDownimagelist.SelectedIndex = index;
                                index++;
                            }
                        }
                    }
                }


            }
            else
            {


            }
        }

        protected void droplistfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtmessagecomp.Enabled = true;
            flag = 1;
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
                                                  where (doc.Document_Type == droplistfile.SelectedItem.Text.Split(',')[0].Trim()) && (UtilityManager.ConvertToLocal(doc.Document_Date).ToString("dd-MMM-yyyy") == droplistfile.SelectedItem.Text.Split(',')[1].Split(':')[1].Trim())
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
                Session["CompareDropdownExamList"] = lstTemp;
                foreach (FileManagementIndex item in lstTemp)
                {
                    target += item.File_Path.ToString() + "|";
                    Notes += item.Exam_Photos_Notes.ToString() + "^$#@^" + item.Id.ToString() + "~";
                }
                // lblcompimage.Text = droplistfile.SelectedItem.Text.Split(',')[0].Trim() + "-" + droplistfile.SelectedItem.Text.Split(',')[2].Trim();
                string sDoctype = droplistfile.SelectedItem.Text.Split(',')[0].Trim(); //e.Item.Cells[3].Text.ToString();
                string sDocdate = droplistfile.SelectedItem.Text.Split(',')[1].Split(':')[1].Trim(); //e.Item.Cells[5].Text.ToString();
                IList<string> ExamString = new List<string>();




                if (!droplistfile.SelectedItem.Text.Contains("--Select Image to Compare--"))
                {
                    string docsecdate = DropDownimagelist.SelectedItem.Text.Split(',')[1].Split(':')[1].Trim();
                    lbldays.Text = Math.Abs((Convert.ToDateTime(sDocdate) - Convert.ToDateTime(docsecdate)).Days).ToString() + " Day(s) apart";

                }
                Session.Add("NotesExamCompare", Notes);
                file_path = target;
                ftpServerIP = System.Configuration.ConfigurationManager.AppSettings["ftpServerIP"];
                UNCPath = System.Configuration.ConfigurationManager.AppSettings["UNCPath"];
                string _fileName = file_path.Replace(ftpServerIP, UNCPath).Replace(@"/", @"\");
                string[] fileGroups = _fileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (fileGroups.Count() > 1)
                {
                    OpenImageExamCompare(fileGroups);
                }
                else
                { OpenImageExamCompare(fileGroups[0]); }



                PageBox.Value = hdnpgno.Value;
                txtmsghistory.Text = hdnmsghst.Value;
                txtmessage.Text = hdnmessage.Value;


                _imgBigActual.Src = hdnimagesource.Value;



                if (hdnimagesource.Value == "")
                {


                    bigImgPDF.Style.Add("display", "block");
                    _imgBigActual.Style.Add("display", "none");

                    PDFholder.Style.Add("display", "block");

                    imgholder.Style.Add("display", "none");
                }
                else
                {
                    bigImgPDF.Style.Add("display", "none");
                    _imgBigActual.Style.Add("display", "block");

                    PDFholder.Style.Add("display", "none");

                    imgholder.Style.Add("display", "block");
                }
                    
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "examCompare", "setNotesValueCompare();test();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "examLoadChange", " { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);

            }
            //  }

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
                Session.Add("ExamData", target);
                Session.Add("NotesExam", Notes);
                if (!droplistfile.SelectedItem.Text.Contains("--Select Image to Compare--"))
                {
                    string docsecdate = droplistfile.SelectedItem.Text.Split(',')[1].Split(':')[1].Trim();
                    lbldays.Text = Math.Abs((Convert.ToDateTime(sDocdate) - Convert.ToDateTime(docsecdate)).Days).ToString() + " Day(s) apart";
                }
                file_path = target;
                ftpServerIP = System.Configuration.ConfigurationManager.AppSettings["ftpServerIP"];
                UNCPath = System.Configuration.ConfigurationManager.AppSettings["UNCPath"];
                string _fileName = file_path.Replace(ftpServerIP, UNCPath).Replace(@"/", @"\");
                string[] fileGroups = _fileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (fileGroups.Count() > 1)
                {
                    OpenImageExam(fileGroups);
                }
                else
                { OpenImageExam(fileGroups[0]); }



                PageBoxcom.Value = hdnpgno1.Value;

                txtmsghistorycomp.Text = hdnmsghst1.Value;

                txtmessagecomp.Text = hdnmessage1.Value;

                _imgBigCompare.Src = hdnimagesource1.Value;



                if (hdnimagesource1.Value == "")
                {
                    
                    bigImgPDFCompare.Style.Add("display", "block");
                    _imgBigCompare.Style.Add("display", "none");

                    PDFholderComp.Style.Add("display", "block");

                    imgholdercom.Style.Add("display", "none");
                }
                else
                {
                    bigImgPDFCompare.Style.Add("display", "none");
                    _imgBigCompare.Style.Add("display", "block");

                    PDFholderComp.Style.Add("display", "none");

                    imgholdercom.Style.Add("display", "block");
                }

                lstTemp = (IList<FileManagementIndex>)Session["CompareDropdownExamList"];
                target = "";
                Notes = "";
                if (lstTemp != null)
                {
                    foreach (FileManagementIndex item in lstTemp)
                    {
                        target += item.File_Path.ToString() + "|";
                        Notes += item.Exam_Photos_Notes.ToString() + "^$#@^" + item.Id.ToString() + "~";
                    }

                    file_path = target;
                    ftpServerIP = System.Configuration.ConfigurationManager.AppSettings["ftpServerIP"];
                    UNCPath = System.Configuration.ConfigurationManager.AppSettings["UNCPath"];
                    _fileName = file_path.Replace(ftpServerIP, UNCPath).Replace(@"/", @"\");
                    fileGroups = _fileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (fileGroups.Count() > 1)
                    {
                        OpenImageExamCompare(fileGroups);
                    }
                    else
                    { OpenImageExamCompare(fileGroups[0]); }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "examNotes", "setNotesValueActual();test1();", true);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "examLoadChange", " { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);

            }
            //  }

        }

        [WebMethod(EnableSession = true)]
        public static string dropfilechange(string filetext)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string result = "success";
            IFileManagementIndexManager objfilemanager = new FileManagementIndexManager();
            IList<FileManagementIndex> file_exam_lst = new List<FileManagementIndex>();
            // file_exam_lst = (IList<FileManagementIndex>)Session["ExamList"];
            file_exam_lst = (IList<FileManagementIndex>)HttpContext.Current.Session["ExamList"];
            List<string> FilePaths = new List<string>();
            List<string> fExten = new List<string>();
            bool isViewable = false;



            file_exam_lst = objfilemanager.GetIndexedListByHumanId(ClientSession.HumanId, "Exam");
            // file_exam_lst = (IList<FileManagementIndex>)Session["ExamList"];
            IList<FileManagementIndex> File_ListForDownlaod = new List<FileManagementIndex>();
            IList<FileManagementIndex> lstTemp = (from doc in file_exam_lst
                                                  where (doc.Document_Type == filetext.Split(',')[0].Trim()) && (UtilityManager.ConvertToLocal(doc.Document_Date).ToString("dd-MMM-yyyy") == filetext.Split(',')[2].Trim())
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
                string sDoctype = filetext.Split(',')[0].Trim(); //e.Item.Cells[3].Text.ToString();
                string sDocdate = filetext.Split(',')[2].Trim(); //e.Item.Cells[5].Text.ToString();
                IList<string> ExamString = new List<string>();
                HttpContext.Current.Session.Add("NotesExamCompare", Notes);
                // Session.Add("NotesExamCompare", Notes);
                string file_path = target;
                string ftpServerIP = System.Configuration.ConfigurationManager.AppSettings["ftpServerIP"];
                string UNCPath = System.Configuration.ConfigurationManager.AppSettings["UNCPath"];
                string _fileName = file_path.Replace(ftpServerIP, UNCPath).Replace(@"/", @"\");
                string[] fileGroups = _fileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                frmImageCompare obj = new frmImageCompare();
                if (fileGroups.Count() > 1)
                {
                    obj.OpenImageExamCompare(fileGroups);
                }
                else
                { obj.OpenImageExamCompare(fileGroups[0]); }


                //   ScriptManager.RegisterStartupScript(this, this.GetType(), "examCompare", "setNotesValueCompare();", true);
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "examLoadChange", " { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);

            }
            //  }
            return result;
        }


        public void OpenFileExamCompare(string filename)
        {
            _plcImgsThumbsComp.Controls.Clear();
            System.Web.UI.HtmlControls.HtmlMeta META = new System.Web.UI.HtmlControls.HtmlMeta();
            META.HttpEquiv = "Pragma";
            META.Content = "no-cache";
            Page page = (Page)HttpContext.Current.Handler;
            page.Header.Controls.Add(META);
            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            hdnPageBoxCompare.Value = Convert.ToString("1");

            //Get FilePath
            // FilePath = Request.QueryString["File"];
            FilePath = filename;
            string file_Notes = (string)Session["NotesExamCompare"].ToString().Split('~')[0];

            if (FilePath != "")
            {
                //Determine Start/End Pages
                int StartPg = 1;
                if (Request.QueryString["StartPage"] != null)
                    StartPg = System.Convert.ToInt16(Request.QueryString["StartPage"]);
                PageLabelcom.Value = @" / " + 1.ToString();
                if (!(FilePath.ToUpper().Contains(".PDF")))
                {

                    int BigImgPg = StartPg;
                    int EndPg = TotalTIFPgs;
                    if (EndPg > TotalTIFPgs)
                        EndPg = TotalTIFPgs;


                    //Add/configure the thumbnails
                    while (StartPg <= EndPg)
                    {
                        Label lbl = new Label();
                        if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                        else lbl.Text = "&nbsp;";
                        HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                        imgcontainer.ID = "pgCompare_" + StartPg.ToString();
                        imgcontainer.Attributes["class"] = "thumbnailCompare";
                        System.Web.UI.WebControls.Image Img = new System.Web.UI.WebControls.Image();
                        Img.CssClass = "lazy";
                        Img.Style.Add("height", "84px");
                        Img.Style.Add("width", "50px");
                        Img.ImageAlign = ImageAlign.Middle;
                        Img.Attributes.Add("onClick", "ChangePgCompare(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pgCompare_" + StartPg.ToString() + "','" + file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','1');");
                        Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                        Img.Attributes.Add("class", "lazy");
                        Img.AlternateText = "Page_" + StartPg;
                        Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                        Img.Attributes.Add("src", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                        HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                        pgDiv.InnerText = StartPg.ToString();
                        pgDiv.Attributes["class"] = "imgPageCompare";
                        imgcontainer.Controls.Add(Img);
                        imgcontainer.Controls.Add(pgDiv);
                        divrotatecomp.Style.Add("display", "inline-block");
                        _plcImgsThumbsComp.Controls.Add(imgcontainer);
                        _plcImgsThumbsComp.Controls.Add(lbl);
                        StartPg++;
                    }
                    /* To load total no of page in the doument for the newly scanned files */
                    Session["Page_Count"] = EndPg;
                    //Bind big img
                    hdnnotesCompare.Value = file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0].ToString();
                    hdnfileindexidCompare.Value = file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1].ToString();
                    if (flag == 1)
                    {
                        _imgBigCompare.Src = "ViewImg.aspx?View=1&FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();

                        bigImgPDFCompare.Style.Add("display", "none");
                        _imgBigCompare.Style.Add("display", "block");

                        PDFholderComp.Style.Add("display", "none");

                        imgholdercom.Style.Add("display", "block");


                    }

                   
                }

                else
                {


                    Label lbl = new Label();
                    if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                    else lbl.Text = "&nbsp;";
                    HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                    imgcontainer.ID = "pgCompare_" + StartPg.ToString();
                    imgcontainer.Attributes["class"] = "thumbnailCompare";
                    System.Web.UI.WebControls.Label Img = new System.Web.UI.WebControls.Label();
                    Img.CssClass = "lazy";
                    Img.Style.Add("height", "84px");
                    Img.Style.Add("width", "50px");
                    Img.Style.Add("align", "center");// = ImageAlign.Middle;
                    Img.Attributes.Add("onClick", "ChangePgCompare(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pgCompare_" + StartPg.ToString() + "','" + file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','1');");
                    Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                    Img.Attributes.Add("class", "lazy");
                    Img.Text = Path.GetFileName(FilePath).Substring(0, 40);
                    Img.ToolTip = Path.GetFileName(FilePath);
                    Img.Style.Add("font-size", "11px");
                    Img.Style.Add("font-family", "Times New Roman");
                    Img.Style.Add("word-break", "break-all");
                    // Img.AlternateText = "Page_" + StartPg;
                    //Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                    HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                    pgDiv.InnerText = StartPg.ToString();
                    pgDiv.Attributes["class"] = "imgPageCompare";
                    imgcontainer.Controls.Add(Img);
                    imgcontainer.Controls.Add(pgDiv);
                    _plcImgsThumbsComp.Controls.Add(imgcontainer);
                    _plcImgsThumbsComp.Controls.Add(lbl);
                    divrotatecomp.Style.Add("display", "none");
                    StartPg++;
                    //  }

                    //  Session["Page_Count"] = EndPg;
                    //Bind big img
                    //  hdnnotes.Value = file_Notes.ToString().Split('^')[0].ToString();
                    // hdnfileindexid.Value = file_Notes.ToString().Split('^')[1].ToString();
                    hdnnotesCompare.Value = file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0].ToString();
                    hdnfileindexidCompare.Value = file_Notes.Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1].ToString();
                    if (flag == 1)
                    {
                        string uri = FilePath;//Request.QueryString["FilePath"];
                        string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                        string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                        string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                        string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                        string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                        string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
                        //Jira #CAP-64
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


                                    bigImgPDFCompare.Attributes.Add("src", sImgPath);

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
                                //Jira #CAP-64
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


                        _imgBigCompare.Src = "";
                        bigImgPDFCompare.Style.Add("display", "block");
                        _imgBigCompare.Style.Add("display", "none");

                        PDFholderComp.Style.Add("display", "block");

                        imgholdercom.Style.Add("display", "none");


                    }




                }
            }
            else
            {
                Response.Write("Please provde a file path");
            }
        }
        public void OpenFileExamCompare(string[] fileGroups)
        {
            _plcImgsThumbsComp.Controls.Clear();
            System.Web.UI.HtmlControls.HtmlMeta META = new System.Web.UI.HtmlControls.HtmlMeta();
            META.HttpEquiv = "Pragma";
            META.Content = "no-cache";
            Page page = (Page)HttpContext.Current.Handler;
            Page.Header.Controls.Add(META);
            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            hdnPageBoxCompare.Value = Convert.ToString("1");
            int BigImgPg = 1;

            string file_Notes = (string)Session["NotesExamCompare"];
            string[] fileNotes = file_Notes.Split("~".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int GroupCount = 0;
            // foreach (string file in fileGroups)
            for (int i = 0; i < fileGroups.Length; i++)
            {






                FilePath = fileGroups[i];

                if (FilePath != "")
                {

                    //Determine Start/End Pages
                    int StartPg = 1;
                    if (Request.QueryString["StartPage"] != null)
                        StartPg = System.Convert.ToInt16(Request.QueryString["StartPage"]);
                    if (!(FilePath.ToUpper().Contains(".PDF")))
                    {
                        

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
                            HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                            imgcontainer.ID = "pgCompare_" + GroupCount.ToString();
                            imgcontainer.Attributes["class"] = "thumbnailCompare";
                            System.Web.UI.WebControls.Image Img = new System.Web.UI.WebControls.Image();
                            Img.CssClass = "lazy";
                            Img.Style.Add("height", "84px");
                            Img.Style.Add("width", "50px");
                            Img.ImageAlign = ImageAlign.Middle;
                            Img.Attributes.Add("onClick", "ChangePgCompare(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pgCompare_" + GroupCount.ToString() + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','" + (i + 1).ToString() + "');");
                            Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                            Img.Attributes.Add("class", "lazy");
                            Img.AlternateText = "Page_" + GroupCount;
                            Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                            Img.Attributes.Add("src", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                            HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                            pgDiv.InnerText = GroupCount.ToString();
                            pgDiv.Attributes["class"] = "imgPageCompare";
                            imgcontainer.Controls.Add(Img);
                            imgcontainer.Controls.Add(pgDiv);
                            _plcImgsThumbsComp.Controls.Add(imgcontainer);
                            _plcImgsThumbsComp.Controls.Add(lbl);
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
                        HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
                        imgcontainer.ID = "pgCompare_" + GroupCount.ToString();
                        imgcontainer.Attributes["class"] = "thumbnailCompare";
                        System.Web.UI.WebControls.Label Img = new System.Web.UI.WebControls.Label();
                        Img.CssClass = "lazy";
                        Img.Style.Add("height", "84px");
                        Img.Style.Add("width", "50px");
                        Img.Style.Add("align", "center");// = ImageAlign.Middle;
                        Img.Attributes.Add("onClick", "ChangePgCompare(" + StartPg.ToString() + @",'" + HttpUtility.UrlEncode(FilePath) + @"','" + "pgCompare_" + GroupCount.ToString() + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0] + "','" + fileNotes[i].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1] + "','" + (i + 1).ToString() + "');");
                        Img.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
                        Img.Attributes.Add("class", "lazy");
                        Img.ToolTip = Path.GetFileName(FilePath);
                        Img.Style.Add("font-size", "11px");
                        Img.Style.Add("font-family", "Times New Roman");
                        Img.Style.Add("word-break", "break-all");
                        Img.Text = Path.GetFileName(FilePath).Substring(0, 40);
                        // Img.AlternateText = "Page_" + StartPg;
                        //Img.Attributes.Add("data-original", "ViewImg.aspx?FilePath=" + HttpUtility.UrlEncode(FilePath) + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth);
                        HtmlGenericControl pgDiv = new HtmlGenericControl("div");
                        pgDiv.InnerText = GroupCount.ToString();
                        pgDiv.Attributes["class"] = "imgPageCompare";
                        imgcontainer.Controls.Add(Img);
                        imgcontainer.Controls.Add(pgDiv);
                        _plcImgsThumbsComp.Controls.Add(imgcontainer);
                        _plcImgsThumbsComp.Controls.Add(lbl);
                        StartPg++;
                        //  }

                    }
                }

            }
            hdnnotesCompare.Value = fileNotes[0].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[0].ToString();
            hdnfileindexidCompare.Value = fileNotes[0].Split(new string[] { "^$#@^" }, StringSplitOptions.None)[1].ToString();
            PageLabelcom.Value = @" / " + GroupCount.ToString();
            if (!(fileGroups[0].ToUpper().Contains(".PDF")))
            {

                if (flag == 1)
                {
                    _imgBigCompare.Src = "ViewImg.aspx?View=1&FilePath=" + HttpUtility.UrlEncode(fileGroups[0]) + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();
                    hdnimagesource1.Value = "ViewImg.aspx?View=1&FilePath=" + HttpUtility.UrlEncode(fileGroups[0]) + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();
                    bigImgPDFCompare.Style.Add("display", "none");
                    _imgBigCompare.Style.Add("display", "block");

                    PDFholderComp.Style.Add("display", "none");
                    divrotatecomp.Style.Add("display", "inline-block");
                    imgholdercom.Style.Add("display", "block");
                }
            }

            else
            {
                if (flag == 1)
                {
                    string uri = fileGroups[0];//Request.QueryString["FilePath"];
                    string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
                    string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
                    string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                    string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
                    string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
                    string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
                    //Jira #CAP-64
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


                                bigImgPDFCompare.Attributes.Add("src", sImgPath);

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
                            //Jira #CAP-64
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

                    divrotatecomp.Style.Add("display", "none");
                    _imgBigCompare.Src = "";

                    bigImgPDFCompare.Style.Add("display", "block");
                    PDFholderComp.Style.Add("display", "block");
                    _imgBigCompare.Style.Add("display", "none");
                    imgholdercom.Style.Add("display", "none");


                }
            }
        }

        public void OpenImageExamCompare(string filename)
        {

            try
            {


                OpenFileExamCompare((filename));
                PageBoxcom.Value = "1";
            }
            catch 
            {
                _imgBigCompare.Attributes.Add("alt", "Uploaded File Not available in the location, it may be deleted or corrupted.Please Contact Support.");
                //string ts = "";
                /*File Not Available Handled in the UI Part*/
            }

        }

        public void OpenImageExamCompare(string[] filenameGroups)
        {

            try
            {
                OpenFileExamCompare(filenameGroups);
                PageBoxcom.Value = "1";
            }
            catch
            {
                _imgBigCompare.Attributes.Add("alt", "Uploaded File Not available in the location, it may be deleted or corrupted.Please Contact Support.");
                /*File Not Available Handled in the UI Part*/
            }

        }

    }
}