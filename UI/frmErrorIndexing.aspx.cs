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
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace Acurus.Capella.UI
{
    public partial class frmErrorIndexing : System.Web.UI.Page
    {
        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnErroredFilePath.Value = ConfigurationManager.AppSettings["ErroredFilePathURL"];
            }
        }
        #endregion

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object LoadGrid()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IndexingExceptionLogManager IndexingExceptionLogMngr = new IndexingExceptionLogManager();
            IList<IndexingExceptionLog> indexingExceptionLogList = IndexingExceptionLogMngr.GetAllActiveIndexingExceptionLog();

            foreach (var indexingExceptionLog in indexingExceptionLogList)
            {
                indexingExceptionLog.File_Name = Path.GetFileName(indexingExceptionLog.File_Name);
            }

            var newResult = new
            {
                data = JsonConvert.SerializeObject(indexingExceptionLogList),
                count = indexingExceptionLogList.Count,
                role = ClientSession.UserRole,
            };
            return newResult;
        }
    }
}