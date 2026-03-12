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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using log4net;
using System.Reflection;
using MySql.Data.MySqlClient;
//[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Acurus.Capella.UI
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger("Error");
        public static class DBConnector
        {
            static MySqlDataAdapter MyDataAdap = null;
            private static string ReadConnection()
            {
                string ConnectionData;
                ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                return ConnectionData;
            }
            public static DataSet ReadData(string Query)
            {
                DataSet dsReturn = new DataSet();
                MyDataAdap = new MySqlDataAdapter(Query, ReadConnection());
                MyDataAdap.SelectCommand.CommandTimeout = 300;
                MyDataAdap.Fill(dsReturn);
                return dsReturn;
            }

            public static int WriteData(string Query)
            {
                int iReturn = 0;
                using (MySqlConnection con = new MySqlConnection(ReadConnection()))
                {
                    using (MySqlCommand cmd = new MySqlCommand(Query))
                    {
                        cmd.Connection = con;
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            iReturn = 1;
                        }
                        catch
                        {
                            iReturn = 2;
                        }
                    }
                }
                return iReturn;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                // Create safe error messages.
                string generalErrorMsg = "A problem has occurred on this web site. Please try again. If this error continues, please contact support.";
                //string httpErrorMsg = "An HTTP error occurred. Page Not found. Please try again.";
                string unhandledErrorMsg = "The error was unhandled by application code.";
                //
                if (Request.RawUrl.Contains("ErrorPage.aspx"))
                {
                    btnLogin.Visible = true;
                }

                // Display safe error message.

                string version = "";
                if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                    version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                string[] server = version.Split('|');
                string serverno = "";
                if (server.Length > 1)
                    serverno = server[1].Trim();
                // Determine where error was handled.
                string errHandler = Request.QueryString["handler"];
                if (errHandler == null)
                    errHandler = "Error Page";

                // Get the last error from the server.
                Exception ex = null;
                if (Server.GetLastError() != null)
                    ex = Server.GetLastError().GetBaseException();

                // If the exception no longer exists, create a generic exception.
                if (ex == null)
                    ex = UIManager.UnhandledException;
                //ex = (Exception)Session["Unhandled_Exception"];
                if (ex == null)
                    ex = new Exception(unhandledErrorMsg);


                string sMessage = "";
                string statserrorlogmessage = string.Empty;
                string statserrorlogstacktrace = string.Empty;

                if (Request.QueryString["Message"] != null)
                    sMessage = Request.QueryString["Message"];

                int iReturn;
                if (sMessage == "XmlBeingUsedByAnotherProcess")
                {
                    sMessage = "This patient XML is being used by another user. Please try again later." + "<br/>" + "<br/>";
                    if (ex.InnerException != null)
                    {
                        sMessage += "Exception type   : " + ex.InnerException.GetType() + "<br/>" + "<br/>" +
                                     "Exception Message: " + ex.InnerException.Message + "<br/>" + "<br/>" +
                                     "Stack trace      : " + ex.InnerException.StackTrace + "<br/>" + "<br/>";
                    }
                    else
                    {
                        sMessage += "Exception type   : " + ex.GetType() + "<br/>" + "<br/>" +
                                    "Exception Message: " + ex.Message + "<br/>" + "<br/>" +
                                    "Stack trace      : " + ex.StackTrace + "<br/>" + "<br/>";
                    }
                    btnLogin.Visible = true;
                    statserrorlogmessage = sMessage;
                }
                else if (sMessage != string.Empty)
                {
                    sMessage = sMessage.Split(new string[] { "|$|" }, StringSplitOptions.None)[0];
                    statserrorlogmessage = sMessage.Split(new string[] { "|$|" }, StringSplitOptions.None)[0];
                    if (sMessage.Split(new string[] { "|$|" }, StringSplitOptions.None).Length > 1)
                        statserrorlogstacktrace = sMessage.Split(new string[] { "|$|" }, StringSplitOptions.None)[1];

                    //sMessage.Split(""|$|);
                }
                // Show error details to only you (developer). LOCAL ACCESS ONLY.
                //if (Request.IsLocal)
                {
                    detailedErrorPanel.Visible = true;

                    string message = "Exception Message: " + ex.Message + "<br/>" + "<br/>";



                    innerMessage.Text = message;
                    int flag = 0;

                    //Introduced for detailed message in the stats_apperrorlog table

                    if (ex != null && ex.Message != null && sMessage == string.Empty)
                        statserrorlogmessage = ex.Message;

                    if (ex != null && ex.StackTrace != null)
                        statserrorlogstacktrace = ex.StackTrace;
                    if (ex != null && ex.InnerException != null && ex.InnerException.Message != null && sMessage == string.Empty)
                    {
                        statserrorlogmessage = statserrorlogmessage + ex.InnerException.Message;
                    }
                    if (ex != null && ex.InnerException != null && ex.InnerException.StackTrace != null && sMessage == string.Empty)
                    {
                        statserrorlogstacktrace = statserrorlogstacktrace + ex.InnerException.StackTrace;
                    }

                    if (sMessage.ToLower().Contains("there is an unclosed literal string") == true || sMessage.ToLower().Contains("root element is missing") == true || sMessage.ToLower().Contains("unexpected end of file") == true || sMessage.ToLower().Contains("is an unexpected token") == true)
                    {
                        // Labelmsg.Text = "An XML error occured. Please close and reopen the patient chart to solve the issue.";
                        Labelmsg.Text = "To fix this error and reopen the patient chart, Please click on MyQ (or) EMR -> Open Patient Chart.";
                        btnLogin.Visible = false;
                        generalErrorMsg = "A Microsoft error has occurred. ";

                    }
                    if (ex.Message != null && ex.Message.ToLower().Contains("there is an unclosed literal string") == true || ex.Message.ToLower().Contains("root element is missing") == true || ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                    {
                        // Labelmsg.Text = "An XML error occured. Please close and reopen the patient chart to solve the issue.";
                        Labelmsg.Text = "To fix this error and reopen the patient chart, Please click on MyQ (or) EMR -> Open Patient Chart.";
                        btnLogin.Visible = false;
                        generalErrorMsg = "A Microsoft error has occurred. ";
                    }

                    if (sMessage != "" && sMessage != null && statserrorlogmessage != string.Empty)
                    {
                        flag = 1;

                        // string insertQuery = "insert into  stats_apperrorlog values(0,'" + ex.Message.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + "', '" + serverno + "','" + DateTime.Now + "','" + ClientSession.UserName + "','" + ClientSession.EncounterId + "','" + ClientSession.HumanId + "','" + ClientSession.PhysicianId + "','" + ex.StackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        string insertQuery = "insert into  stats_apperrorlog values(0,'" + statserrorlogmessage.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + "', '" + serverno + "','" + DateTime.Now + "','" + ClientSession.UserName + "','" + ClientSession.EncounterId + "','" + ClientSession.HumanId + "','" + ClientSession.PhysicianId + "','" + statserrorlogstacktrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        iReturn = DBConnector.WriteData(insertQuery);
                        string Query = "select * from stats_apperrorlog order by Id desc limit 1";
                        DataSet ds = DBConnector.ReadData(Query);
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {


                            friendlyErrorMsg.Text = DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + "_" + ds.Tables[0].Rows[0][0].ToString() + ": " + generalErrorMsg;

                        }
                        innerMessage.Text = sMessage;

                    }
                    else if (ex.InnerException != null)
                    {
                        flag = 1;
                        message += //"---BEGIN InnerException--- " + "<br/>" +
                                   //"Exception type   : " + ex.InnerException.GetType() + "<br/>" + "<br/>" +
                                   "Exception Message: " + ex.InnerException.Message + "<br/>" + "<br/>";
                        //  "Stack trace      : " + ex.InnerException.StackTrace + "<br/>" + "<br/>" +
                        // "---END Inner Exception----";


                        // string insertQuery = "insert into  stats_apperrorlog values(0,'" + ex.InnerException.Message.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + "', '" + serverno + "','" + DateTime.Now + "','" + ClientSession.UserName + "','" + ClientSession.EncounterId + "','" + ClientSession.HumanId + "','" + ClientSession.PhysicianId + "','" + ex.InnerException.StackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        string insertQuery = "insert into  stats_apperrorlog values(0,'" + statserrorlogmessage.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + "', '" + serverno + "','" + DateTime.Now + "','" + ClientSession.UserName + "','" + ClientSession.EncounterId + "','" + ClientSession.HumanId + "','" + ClientSession.PhysicianId + "','" + statserrorlogstacktrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        iReturn = DBConnector.WriteData(insertQuery);
                        string Query = "select * from stats_apperrorlog order by Id desc limit 1";
                        DataSet ds = DBConnector.ReadData(Query);
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            friendlyErrorMsg.Text = DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + "_" + ds.Tables[0].Rows[0][0].ToString() + ": " + generalErrorMsg;

                        }
                        innerMessage.Text = message;
                    }



                    //---- to log exception details
                    try
                    {
                        if (flag == 0)
                        {
                            message = "Exception Message: " + ex.Message + "<br/>" + "<br/>";


                            //  string insertQuery = "insert into  stats_apperrorlog values(0,'" + ex.Message.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + "', '" + serverno + "','" + DateTime.Now + "','" + ClientSession.UserName + "','" + ClientSession.EncounterId + "','" + ClientSession.HumanId + "','" + ClientSession.PhysicianId + "','" + ex.StackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                            string insertQuery = "insert into  stats_apperrorlog values(0,'" + statserrorlogmessage.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + "', '" + serverno + "','" + DateTime.Now + "','" + ClientSession.UserName + "','" + ClientSession.EncounterId + "','" + ClientSession.HumanId + "','" + ClientSession.PhysicianId + "','" + statserrorlogstacktrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                            iReturn = DBConnector.WriteData(insertQuery);
                            string Query = "select * from stats_apperrorlog order by Id desc limit 1";
                            DataSet ds = DBConnector.ReadData(Query);
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                friendlyErrorMsg.Text = DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + "_" + ds.Tables[0].Rows[0][0].ToString() + ": " + generalErrorMsg;

                            }
                            innerMessage.Text = message;
                        }
                        string log_message = System.Environment.NewLine + System.Environment.NewLine + "------------------------------BEGINNING OF THIS EXCEPTION------------------------------------" + System.Environment.NewLine + System.Environment.NewLine +
                          "MESSAGE: " + ex.Message + System.Environment.NewLine +
                          "TYPE: " + Convert.ToString(ex.GetType()) + System.Environment.NewLine +
                          "TIME: " + DateTime.Now.ToString() + " . UTC TIME: " + DateTime.UtcNow.ToString() + System.Environment.NewLine +
                          "SOURCE: " + Convert.ToString(ex.Source) + System.Environment.NewLine +
                          //"FORM: " + Request.Form.ToString() + System.Environment.NewLine +
                          //"QUERYSTRING: " + Request.QueryString.ToString() + System.Environment.NewLine +
                          "TARGETSITE: " + Convert.ToString(ex.TargetSite) + System.Environment.NewLine +
                          "CURRENT USER: " + Convert.ToString(ClientSession.UserName) + System.Environment.NewLine +
                          "ENCOUNTER ID: " + Convert.ToString(ClientSession.EncounterId) + System.Environment.NewLine +
                          "HUMAN ID: " + Convert.ToString(ClientSession.HumanId) + System.Environment.NewLine +
                          "PHYSICIAN ID: " + Convert.ToString(ClientSession.PhysicianId) + System.Environment.NewLine +
                          "STACKTRACE: " + Convert.ToString(ex.StackTrace);
                        Exception baseException = ex;
                        for (; ; )
                        {
                            if (baseException.InnerException != null)
                            {
                                log_message += System.Environment.NewLine + System.Environment.NewLine + "----------------------------------------------------------------" + System.Environment.NewLine + "NEXT LEVEL INNER EXCEPTION DETAILS :" + System.Environment.NewLine +
                                    "MESSAGE: " + Convert.ToString(baseException.InnerException.Message) + System.Environment.NewLine +
                                    "\nTYPE: " + Convert.ToString(baseException.InnerException.GetType()) + System.Environment.NewLine +
                              "SOURCE: " + Convert.ToString(baseException.InnerException.Source) + System.Environment.NewLine +
                              "TARGETSITE: " + Convert.ToString(baseException.InnerException.TargetSite) + System.Environment.NewLine +
                              "STACKTRACE: " + Convert.ToString(baseException.InnerException.StackTrace);
                                baseException = baseException.InnerException;
                            }
                            else
                            {
                                break;
                            }
                        }

                        long totalSessionBytes = 0;
                        BinaryFormatter b = new BinaryFormatter();
                        MemoryStream m;
                        try
                        {
                            foreach (var obj in Session)
                            {
                                m = new MemoryStream();
                                b.Serialize(m, obj);
                                totalSessionBytes += m.Length;
                            }


                            log_message += System.Environment.NewLine + System.Environment.NewLine + "SIZE OF CURRENT SESSION: " + totalSessionBytes.ToString() + " bytes" + System.Environment.NewLine;
                        }
                        catch
                        {
                            log_message += System.Environment.NewLine + System.Environment.NewLine + "SIZE OF CURRENT SESSION: Unable to calculate since Session is unavailable in this context." + System.Environment.NewLine;
                        }
                        log_message += System.Environment.NewLine + System.Environment.NewLine + "------------------------------END OF THIS EXCEPTION------------------------------------" + System.Environment.NewLine + System.Environment.NewLine;
                        log.Error(log_message, ex);
                    }
                    catch
                    {
                        log.Error("Unable to log details of Exception", ex);
                    }
                }

                // Clear the error from the server.
                Server.ClearError();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display')='none'", true);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Request.RawUrl.Contains(".aspx"))
            {
                //Clear all session Folders 
                UtilityManager.DeleteUserSessionFile(string.Empty, Session.SessionID);
                ClientSession.SavedSession = "DELETED";
                if (Directory.Exists(Server.MapPath("Documents\\" + Session.SessionID)) == true) // Handled To Delete Isolated Temp directory Created For Logged in users
                {
                    try
                    {
                        System.IO.Directory.Delete(Server.MapPath("Documents\\" + Session.SessionID), true);
                    }
                    catch
                    {
                    }
                }

                if (Directory.Exists(Server.MapPath("atala-capture-download\\" + Session.SessionID)) == true)
                {
                    try
                    {
                        System.IO.Directory.Delete(Server.MapPath("atala-capture-download\\" + Session.SessionID), true);

                        foreach (string filename in Directory.GetFiles(Server.MapPath("atala-capture-download")))
                        {

                            FileInfo file = new FileInfo(filename);
                            if (file.Name.StartsWith(Session.SessionID))
                            {
                                file.Delete();
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                if (Directory.Exists(Server.MapPath("atala-capture-upload\\" + Session.SessionID)) == true)
                {
                    try
                    {
                        System.IO.Directory.Delete(Server.MapPath("atala-capture-upload\\" + Session.SessionID), true);
                    }
                    catch
                    {
                    }
                }

                HttpContext.Current.Application.Remove("user");
                Session["ShowAllState"] = null;
                Session["GeneralQShowAll"] = null;

                //Redirect to login Page 

                string Url = Request.RawUrl;
                string[] Url1 = Url.Split('/');
                string Link = Url1[Url1.Length - 1].ToString();

                string UrlReferrer = HttpContext.Current.Request.UrlReferrer.ToString();
                string[] UrlReferrer1 = UrlReferrer.Split('/');
                string LinkReferrer = UrlReferrer1[UrlReferrer1.Length - 1].ToString();

                if (Link.ToUpper().Contains("EMAIL") == true)
                {
                    string[] SplitLink = Link.Split('?');
                    if (Convert.ToString(SplitLink[1]) != string.Empty)
                        Response.Write("<script> window.top.location.href=\" webfrmLogin.aspx?" + SplitLink[1].ToString() + "\"; </script>");
                }
                else if (LinkReferrer.ToUpper().Contains("EMAIL") == true)
                {
                    string[] SplitLink = LinkReferrer.Split('?');
                    if (Convert.ToString(SplitLink[1]) != string.Empty)
                        Response.Write("<script> window.top.location.href=\" webfrmLogin.aspx?" + SplitLink[1].ToString() + "\"; </script>");
                }
                else
                {
                    //CAP-1752
                    var loginpage = (ConfigurationSettings.AppSettings["IsSSOLogin"] == "Y" ? "frmLoginNew.aspx" : "frmLogin.aspx");
                    Response.Write($"<script> window.top.location.href=\"{loginpage}\"; </script>");
                }
            }
        }
    }
}
