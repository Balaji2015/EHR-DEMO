using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.UI.Extensions;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace Acurus.Capella.UI
{
    public partial class frmErroredSignedReports : System.Web.UI.Page
    {
        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnErroredFilePath.Value = ConfigurationManager.AppSettings["ErroredFilePathURL"];
                PageLabel.InnerText = "/0";
                PageBox.Value = "0";
            }
            PDFholder.Style.Add("display", "none");
            bigImagePDF.Style.Add("display", "none");
            imgholder.Style.Add("display", "block");
        }
        #endregion

        [WebMethod(EnableSession = true)]
        public static string SourceImageDelete(string filePath)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            if (File.Exists(filePath))
            {
                IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                ActivityLog activity = new ActivityLog
                {
                    Human_ID = ClientSession.HumanId,
                    Encounter_ID = 0,
                    Sent_To = string.Empty,
                    Activity_Date_And_Time = Convert.ToDateTime(ClientSession.LocalTime),
                    Role = ClientSession.UserRole,
                    Subject = string.Empty,
                    Message = string.Empty,
                    Activity_Type = "Delete Errored Signed Reports",
                    Activity_By = ClientSession.UserName,
                    Fax_File_Path = filePath
                };
                ActivityLogList.Add(activity);
                ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);

                File.Delete(filePath);
                return "Success";
            }
            else
            {
                return "Selected File is already deleted.";
            }
        }

        [WebMethod(EnableSession = true)]
        public static string LoadGrid()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            IList<string> lstDocuments = new List<string>();
            
            string path = ConfigurationManager.AppSettings["ErroredFilePath_Local"];
            if (Directory.Exists(path))
            {
                lstDocuments = Directory.GetFiles(path).ToList();
            }
            return JsonConvert.SerializeObject(lstDocuments);
        }

        [WebMethod(EnableSession = true)]
        public static string OpenGridFile(string filename, string filepath)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            StringBuilder sFullPath = new StringBuilder();
            sFullPath.Append(filepath);

            int pageCount = 1;
            try
            {
                if (File.Exists(sFullPath.ToString()))
                {
                    //IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                    //ActivityLog activity = new ActivityLog
                    //{
                    //    Human_ID = ClientSession.HumanId,
                    //    Encounter_ID = 0,
                    //    Sent_To = string.Empty,
                    //    Activity_Date_And_Time = Convert.ToDateTime(ClientSession.LocalTime),
                    //    Role = ClientSession.UserRole,
                    //    Subject = string.Empty,
                    //    Message = string.Empty,
                    //    Activity_Type = "View Errored Signed Reports",
                    //    Activity_By = ClientSession.UserName,
                    //    Fax_File_Path = sFullPath.ToString()
                    //};
                    //ActivityLogList.Add(activity);
                    //ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                    //ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);

                    if (Path.GetExtension(filename).ToLower() == ".pdf")
                    {
                        using (FileStream fs = new FileStream(sFullPath.ToString(), FileMode.Open, FileAccess.Read))
                        {
                            StreamReader sr = new StreamReader(fs);
                            // string pdf = sr.ReadToEnd();
                            Regex rx = new Regex(@"/Type\s*/Page[^s]");
                            MatchCollection match = rx.Matches(sr.ReadToEnd());
                            pageCount = match.Count;
                            if (pageCount == 0)
                            {
                                PdfReader pdfReader = new PdfReader(sFullPath.ToString());
                                pageCount = pdfReader.NumberOfPages;
                            }
                            fs.Close();
                            fs.Dispose();
                        }
                    }
                    else
                    {
                        using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(sFullPath.ToString()))
                        {
                            pageCount = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                            imgbg.Dispose();
                        }
                    }
                }
            }
            catch
            {
            }
            return pageCount.ToString();
        }
    }
}