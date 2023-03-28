using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using log4net;
using System.Reflection;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Acurus.Capella.PatientPortal
{
    public class Global : System.Web.HttpApplication
    {
        //public static Hashtable ht = new Hashtable();
        private static readonly ILog log = LogManager.GetLogger("Error");
        private static readonly ILog logFile = LogManager.GetLogger("Session");
        private static bool isMultiLogIn = false;

        protected void Application_Start(object sender, EventArgs e)
        {
            // ht.Clear();
            PropertyInfo p = typeof(System.Web.HttpRuntime).GetProperty("FileChangesMonitor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            object o = p.GetValue(null, null);
            FieldInfo f = o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
            object monitor = f.GetValue(o);
            MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", BindingFlags.Instance | BindingFlags.NonPublic);
            m.Invoke(monitor, new object[] { });

            ApplicationObject.facilityLibraryList = UtilityManager.GetFacilityList();

            ProcessMasterManager objProcessMngr = new ProcessMasterManager();
            ApplicationObject.processMasterList = objProcessMngr.GetAllProcessList();

            ElementManager objElementManager = new ElementManager();
            ApplicationObject.elementList = objElementManager.GetAllElement(string.Empty);

            MapXMLBlobManager mapXMLBlobManager = new MapXMLBlobManager();
            ApplicationObject.ilstMapXMLBlob = mapXMLBlobManager.GetMapXMLBlobList();

            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

            //Session.Timeout = 1440; 
            if (HttpContext.Current.Session != null)
                log4net.GlobalContext.Properties["SessionID"] = HttpContext.Current.Session.SessionID;
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("Web.config")));
            ClientSession.UserName = (Request["OpenPatChart"] != null ? Request["UserName"].ToString() : string.Empty);//Changed for CarePointe
            CheckLogin();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session != null)
            {
                if (ClientSession.SavedSession == "TRUE")
                {
                    string sUser = ClientSession.UserName;
                    UserSessionManager userSessionMngr = new UserSessionManager();
                    // IList<UserSession> lstUserID = userSessionMngr.GetCurrentSessionByUserName(sUser);

                    //  IList<UserSession> lstUserID = userSessionMngr.GetUserSessionFromXml(sUser);
                    String User = ClientSession.UserName;
                    //Object lstUserID = ht[ClientSession.UserName];
                    IList<string> lstUser = UtilityManager.FindUserSessionFiles(ClientSession.UserName, string.Empty);
                    if (lstUser.Count != 0)
                    {
                        isMultiLogIn = false;
                        var objIsActiveSession = (from item in lstUser where item.Contains(Session.SessionID) select item).ToList<string>();
                        //if (Convert.ToString(lstUserID).Equals(HttpContext.Current.Session.SessionID) == false )//&& Convert.ToString(ht["IsLogin-" + sUser]) == "TRUE")
                        if (objIsActiveSession.Count == 0)
                        {
                            isMultiLogIn = true;
                            HttpContext.Current.Session.Abandon();
                            //if (ht.ContainsKey("IsLogin-" + sUser))
                            //{
                            //    ht.Remove("IsLogin-" + sUser);
                            //}
                            HttpContext.Current.Application.Remove("user");
                            if (IsAjaxRequest(HttpContext.Current.Request))
                            {
                                HttpContext.Current.Response.StatusCode = 999;
                                HttpContext.Current.Response.Status = "999 Session Expired";
                                //HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx?Reason=Abandoned";
                                HttpContext.Current.Response.StatusDescription = "frmSessionExpiredMultiLogin.aspx?Reason=Abandoned";
                            }
                            else
                            {
                                //Response.Redirect("~/frmSessionExpired.aspx?Reason=Abandoned");
                                Response.Redirect("~/frmSessionExpiredMultiLogin.aspx?Reason=Abandoned");
                            }
                        }
                        //var isPresent = (from obj in lstUserID where obj.Current_Session_ID != HttpContext.Current.Session.SessionID select obj).ToList();
                        // isMultiLogIn = false;
                        // if (isPresent.Count > 0)
                        // {
                        //     isMultiLogIn = true;
                        //     HttpContext.Current.Session.Abandon();
                        //     HttpContext.Current.Application.Remove("user");
                        //     if (IsAjaxRequest(HttpContext.Current.Request))
                        //     {
                        //         HttpContext.Current.Response.StatusCode = 999;
                        //         HttpContext.Current.Response.Status = "999 Session Expired";
                        //         HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx?Reason=Abandoned";
                        //     }
                        //     else
                        //         Response.Redirect("~/frmSessionExpired.aspx?Reason=Abandoned");
                        // }
                    }
                    else
                    {
                        isMultiLogIn = true;
                        HttpContext.Current.Session.Abandon();
                        HttpContext.Current.Application.Remove("user");
                        //if (ht.ContainsKey("IsLogin-" + sUser))
                        //{
                        //    ht.Remove("IsLogin-" + sUser);
                        //}
                        if (IsAjaxRequest(HttpContext.Current.Request))
                        {
                            HttpContext.Current.Response.StatusCode = 999;
                            HttpContext.Current.Response.Status = "999 Session Expired";
                            //HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx?Reason=Abandoned";
                            HttpContext.Current.Response.StatusDescription = "frmSessionExpiredMultiLogin.aspx?Reason=Abandoned";
                        }
                        else
                        {
                            //Response.Redirect("~/frmSessionExpired.aspx?Reason=Abandoned");
                            Response.Redirect("~/frmSessionExpiredMultiLogin.aspx?Reason=Abandoned");
                        }
                    }
                }
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                if (Server.GetLastError() != null)
                {
                    Exception objErr = Server.GetLastError().GetBaseException();
                    string message = System.Environment.NewLine + System.Environment.NewLine + "------------------------------BEGINNING OF THIS EXCEPTION------------------------------------" + System.Environment.NewLine + System.Environment.NewLine +
                      "MESSAGE: " + objErr.Message + System.Environment.NewLine +
                      "TYPE: " + objErr.GetType() + System.Environment.NewLine +
                      "TIME: " + DateTime.Now.ToString() + " . UTC TIME: " + DateTime.UtcNow.ToString() + System.Environment.NewLine +
                      "SOURCE: " + objErr.Source + System.Environment.NewLine +
                        //"FORM: " + Request.Form.ToString() + System.Environment.NewLine +
                        //"QUERYSTRING: " + Request.QueryString.ToString() + System.Environment.NewLine +
                      "TARGETSITE: " + objErr.TargetSite + System.Environment.NewLine +
                      "CURRENT USER: " + Convert.ToString(ClientSession.UserName) + System.Environment.NewLine +
                      "ENCOUNTER ID: " + Convert.ToString(ClientSession.EncounterId) + System.Environment.NewLine +
                      "HUMAN ID: " + Convert.ToString(ClientSession.HumanId) + System.Environment.NewLine +
                      "PHYSICIAN ID: " + Convert.ToString(ClientSession.PhysicianId) + System.Environment.NewLine +
                      "STACKTRACE: " + objErr.StackTrace;
                    if (objErr.InnerException != null)
                    {
                        message += System.Environment.NewLine + System.Environment.NewLine + "----------------------------------------------------------------" + System.Environment.NewLine + "INNER EXCEPTION DETAILS :" + System.Environment.NewLine +
                            "MESSAGE: " + objErr.InnerException.Message + System.Environment.NewLine +
                            "\nTYPE: " + objErr.InnerException.GetType() + System.Environment.NewLine +
                      "SOURCE: " + objErr.InnerException.Source + System.Environment.NewLine +
                      "TARGETSITE: " + objErr.InnerException.TargetSite + System.Environment.NewLine +
                      "STACKTRACE: " + objErr.InnerException.StackTrace;
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


                        message += System.Environment.NewLine + System.Environment.NewLine + "SIZE OF CURRENT SESSION: " + totalSessionBytes.ToString() + " bytes" + System.Environment.NewLine;
                    }
                    catch
                    {
                        message += System.Environment.NewLine + System.Environment.NewLine + "SIZE OF CURRENT SESSION: Unable to calculate since Session is unavailable in this context." + System.Environment.NewLine;
                    }

                    Exception exc = Server.GetLastError().GetBaseException();

                    if (exc != null && exc.Message != null)
                    {
                        if ((exc.Message.Contains("System.Web.HttpUnhandledException") || exc.Message.Contains("Script controls may not be registered after PreRender")) && ((exc.Message.Contains("Row was updated or deleted by another transaction") == false) && ((exc.InnerException != null) ? (exc.InnerException.Message.Contains("Row was updated or deleted by another transaction") == false) : true)))
                        {
                            Server.ClearError();
                            message += "ERROR HANDLING METHOD : Server.ClearError()" + System.Environment.NewLine + System.Environment.NewLine;
                        }
                        else
                        {
                            try
                            {
                                if (exc.Message.IndexOf("Transaction XML") > -1)
                                {
                                    //  Console.Error=function (){alert}
                                    //System.Web.UI.Page Errorpage = (System.Web.UI.Page)HttpContext.Current.Handler;
                                    //Errorpage.ClientScript.RegisterClientScriptBlock(GetType(), "MyScriptKey", " alert('XML is not found.Kindly contact support. ');", true);
                                    //   Response.Write("<script type='text/javascript'> alert('XML is not found.Kindly contact support. ');</script>");
                                }
                                else if (exc.Message.IndexOf("To Process is not found") > -1)//BugID:53884
                                {

                                }
                                else if (exc.Message.ToUpper().Contains("THE PROCESS CANNOT ACCESS THE FILE") && (exc.Message.Contains("Human_") || exc.Message.Contains("Encounter_")) && exc.Message.ToUpper().Contains(".XML' BECAUSE IT IS BEING USED BY ANOTHER PROCESS"))//BugID:56782
                                {
                                    HttpContext.Current.Server.ClearError();
                                    HttpContext.Current.Response.Redirect("~/ErrorPage.aspx?Message=XmlBeingUsedByAnotherProcess");
                                    //Errorpage.ClientScript.RegisterClientScriptBlock(GetType(), "MyScriptKey", " alert('XML is not found.Kindly contact support. ');", true);
                                    // Response.Write("<script type='text/javascript'> alert('This patient XML is being used by another user. Please try again later. ');</script>");

                                }
                                //Session["Unhandled_Exception"] = exc;
                                else
                                {
                                    if (Server.GetLastError() != null)
                                        UIManager.UnhandledException = Server.GetLastError().GetBaseException();

                                    HttpContext.Current.Server.ClearError();
                                    //  HttpContext.Current.Response.Redirect("~/ErrorPage.aspx?Message=" + message + "");// Response.Redirect("~/ErrorPage.aspx?handler=Application_Error%20-%20Global.asax");
                                    HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");

                                    message += "ERROR HANDLING METHOD : Custom Error page" + System.Environment.NewLine + System.Environment.NewLine + "Message : " + exc.Message + System.Environment.NewLine;
                                }

                            }
                            catch (Exception exe)
                            {
                                if (exe.InnerException != null && exe.Message != null)
                                    message += "ERROR HANDLING METHOD : CheckLogin()" + System.Environment.NewLine + "InnerException : " + exe.InnerException + System.Environment.NewLine + "Message : " + exe.Message + System.Environment.NewLine;



                                // CheckLogin();
                            }
                        }
                    }
                    message += System.Environment.NewLine + System.Environment.NewLine + "------------------------------END OF THIS EXCEPTION------------------------------------" + System.Environment.NewLine + System.Environment.NewLine;
                    log.Error(message, objErr);

                }
            }
            catch (Exception ex) { }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            logFile.Info("Before Session End");
            UserSessionManager userSessionMngr = new UserSessionManager();
            //userSessionMngr.DeleteUserSessionAtSessionEnd(Session.SessionID);
            //if (HttpContext.Current != null)
            //{
            //    ht.Remove(ClientSession.UserName);
            //    if (ht.ContainsKey("IsLogin-" + ClientSession.UserName))
            //    {
            //        ht.Remove("IsLogin-" + ClientSession.UserName);
            //    }
            //}
            // userSessionMngr.DeleteUserSessionFromXml(Session.SessionID);
            UtilityManager.DeleteUserSessionFile(string.Empty, Session.SessionID);
            logFile.Info(DateTime.UtcNow.ToString() + "- After Session End", null);
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        void CheckLogin()
        {
            string SessionData = string.Empty;

            if (Request.RawUrl.Contains(".aspx"))
            {
                string Url = Request.RawUrl;
                string[] Url1 = Url.Split('/');
                string Link = Url;// Url1[Url1.Length - 1].ToString();
                try
                {
                    if (Session != null)
                        SessionData = ClientSession.UserName;
                    if (SessionData == "" && Link.ToUpper().Contains("FRMLOGIN.ASPX") == false && Link.ToUpper() != "FRMLOGIN.ASPX" && Link.ToUpper().Contains("WEBFRMLOGOUT.ASPX") == false && Link.ToUpper().Contains("WEBFRMLOGIN.ASPX") == false)
                    {
                        // Added By manimaran for Bug Id-28418 on 10-12-2014
                        if (Link.ToUpper().Contains("EMAIL") == true)
                        {
                            string[] SplitLink = Link.Split('?');
                            if (Convert.ToString(SplitLink[1]) != string.Empty)
                            {
                                if (SplitLink[1].Contains("ScreenMode"))
                                    SplitLink[1] = SplitLink[1].Substring((SplitLink[1].IndexOf("&") + 1), (SplitLink[1].Length - 1) - (SplitLink[1].IndexOf("&")));
                                Response.Redirect("~/frmSessionExpired.aspx?" + SplitLink[1].ToString());
                            }
                        }
                        else
                        {
                            if (IsAjaxRequest(HttpContext.Current.Request))
                            {
                                HttpContext.Current.Response.StatusCode = 999;
                                HttpContext.Current.Response.Status = "999 Session Expired";
                            }
                            if (isMultiLogIn == true)
                            {
                                if (IsAjaxRequest(HttpContext.Current.Request))
                                {
                                    //HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx?Reason=Abandoned";
                                    HttpContext.Current.Response.StatusDescription = "frmSessionExpiredMultiLogin.aspx?Reason=Abandoned";
                                }
                                else
                                {
                                   // Response.Redirect("~/frmSessionExpired.aspx?Reason=Abandoned");
                                    Response.Redirect("~/frmSessionExpiredMultiLogin.aspx?Reason=Abandoned");
                                }
                                isMultiLogIn = false;
                            }
                            else
                            {
                                if (IsAjaxRequest(HttpContext.Current.Request))
                                {
                                    HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";

                                }
                                else
                                    Response.Redirect("~/frmSessionExpired.aspx");
                                //if ( SessionData == "")
                                //{
                                //    HttpContext.Current.Response.StatusCode = 999;
                                //    HttpContext.Current.Response.Status = "999 Session Expired";
                                //    HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                                //    Response.Redirect("~/frmSessionExpired.aspx");
                                //}
                            }
                        }
                    }
                }
                catch
                {
                    if (IsAjaxRequest(HttpContext.Current.Request))
                    {
                        HttpContext.Current.Response.StatusCode = 999;
                        HttpContext.Current.Response.Status = "999 Session Expired";
                    }
                    if (isMultiLogIn == true)
                    {
                        if (IsAjaxRequest(HttpContext.Current.Request))
                        {
                           // HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx?Reason=Abandoned";
                            HttpContext.Current.Response.StatusDescription = "frmSessionExpiredMultiLogin.aspx?Reason=Abandoned";
                        }
                        else
                        {
                            //Response.Redirect("~/frmSessionExpired.aspx?Reason=Abandoned");
                            Response.Redirect("~/frmSessionExpiredMultiLogin.aspx?Reason=Abandoned");
                        }
                        isMultiLogIn = false;
                    }
                    else
                    {
                        if (SessionData == "")
                        {
                            HttpContext.Current.Response.StatusCode = 999;
                            HttpContext.Current.Response.Status = "999 Session Expired";
                            HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                            Response.Redirect("~/frmSessionExpired.aspx");
                        }
                        //if (IsAjaxRequest(HttpContext.Current.Request))
                        //    HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                        //else
                        //    Response.Redirect("~/frmSessionExpired.aspx");
                    }
                }
            }
        }

        bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            var context = HttpContext.Current;
            var isCallbackRequest = false;// callback requests are ajax requests
            if (context != null && context.CurrentHandler != null && context.CurrentHandler is System.Web.UI.Page)
            {
                isCallbackRequest = ((System.Web.UI.Page)context.CurrentHandler).IsCallback;
            }
            return isCallbackRequest || (request["X-Requested-With"] == "XMLHttpRequest") || (request.Headers["X-Requested-With"] == "XMLHttpRequest");
        }
    }
}