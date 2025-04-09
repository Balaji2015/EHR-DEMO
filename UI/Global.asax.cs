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
using System.Diagnostics;
using System.Text.RegularExpressions;
using Acurus.Capella.UI.Extensions;
using System.Web.Http;
using Acurus.Capella.Core.DTOJson;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Acurus.Capella.UI
{
    public class Global : System.Web.HttpApplication
    {
        //public static Hashtable ht = new Hashtable();
        private static readonly ILog log = LogManager.GetLogger("Error");
        private static bool isMultiLogIn = false;

        protected void Application_Start(object sender, EventArgs e)
        {
            UtilityManager.inserttologgingtableforSessionTimeout("Application is starting", string.Empty, string.Empty);

            // ht.Clear();
            PropertyInfo p = typeof(System.Web.HttpRuntime).GetProperty("FileChangesMonitor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            object o = p.GetValue(null, null);
            FieldInfo f = o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
            object monitor = f.GetValue(o);
            MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", BindingFlags.Instance | BindingFlags.NonPublic);
            m.Invoke(monitor, new object[] { });

            var theRuntime = typeof(HttpRuntime).GetField("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            var fcmField = typeof(HttpRuntime).GetField("_fcm", BindingFlags.NonPublic | BindingFlags.Instance);
            var fcm = fcmField.GetValue(theRuntime);
            fcmField.FieldType.GetMethod("Stop", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(fcm, null);

            ApplicationObject.facilityLibraryList = UtilityManager.GetFacilityList();
            ScnTabManager objscntabman = new ScnTabManager();
            ApplicationObject.scntab = objscntabman.GetAllScreensUsingPatent_ID();
            ClientManager clientMngr = new ClientManager();
            ApplicationObject.ClientList = clientMngr.GetClientList();

            MapXMLBlobManager mapXMLBlobManager = new MapXMLBlobManager();
            ApplicationObject.ilstMapXMLBlob = mapXMLBlobManager.GetMapXMLBlobList();

            ApplicationObject.XsltTransformSplitupList = ConfigureBase<XsltTransformSplitupList>.ReadJson("XsltTransformSplitup.json");
            try
            {
                ProcessMasterManager objProcessMngr = new ProcessMasterManager();
                ApplicationObject.processMasterList = objProcessMngr.GetAllProcessList();
            }
            catch (Exception ex)
            {
                if (ex.Message != null && ex.Message == "Unable to connect to Databse.")
                {
                    throw new Exception("Unable to connect to Databse.");
                }
            }

            ElementManager objElementManager = new ElementManager();
            ApplicationObject.elementList = objElementManager.GetAllElement(string.Empty);
            GlobalConfiguration.Configure(WebAPIConfig.Register);
            //log4net.Config.XmlConfigurator.Configure();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //UtilityManager.inserttologgingtable(ClientSession.UserName.ToString(), ClientSession.UserRole.ToString(), ClientSession.FacilityName,
            //     ClientSession.LocalDate + ";" + ClientSession.LocalTime, "Session is starting - Session Timeout limit is - "+Session.Timeout, DateTime.Now, "0", "frmSessionExpires");
            UtilityManager.inserttologgingtableforSessionTimeout("Session is starting", Request.Url.ToString(), string.Empty);
            //CAP-1167            
            var currentURL = Request.Url.AbsoluteUri.ToString();
            if (DirectURLUtility.IsValidRedirectUrlForLogin(currentURL))
            {
                Session["currenturl"] = HttpUtility.UrlEncode(Request.Url.AbsoluteUri);
            }

            //Session.Timeout = 1440; o
            //if (HttpContext.Current.Session != null)
            //    log4net.GlobalContext.Properties["SessionID"] = HttpContext.Current.Session.SessionID;
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("Web.config")));
            ClientSession.UserName = (Request["OpenPatChart"] != null ? Request["UserName"].ToString() : string.Empty);//Changed for CarePointe

            //  UtilityManager.inserttologgingtableforSessionTimeout("Checklogin - Start", Request.Url.ToString(), string.Empty);
            CheckLogin();
            // UtilityManager.inserttologgingtableforSessionTimeout("Checklogin - End", Request.Url.ToString(), string.Empty);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            UtilityManager.inserttologgingtableforSessionTimeout("Application_BeginRequest - Start", Request.Url.ToString(), string.Empty);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            // UtilityManager.inserttologgingtable(ClientSession.UserName.ToString(), ClientSession.UserRole.ToString(), ClientSession.FacilityName + "; SavedSession is " + ClientSession.SavedSession,
            //         ClientSession.LocalDate + ";" + ClientSession.LocalTime, "Application_AcquireRequestState - " + Request.Url.ToString(), DateTime.Now, "0", "");

            UtilityManager.inserttologgingtableforSessionTimeout("Application_AcquireRequestState - Start", Request.Url.ToString(), string.Empty);

            if (HttpContext.Current.Session != null)
            {

                if (ClientSession.SavedSession == "TRUE")
                {
                    UtilityManager.inserttologgingtableforSessionTimeout("Application_AcquireRequestState -Checking if the User is already login in another system", Request.Url.ToString(), string.Empty);

                    string sUser = ClientSession.UserName;
                    UserSessionManager userSessionMngr = new UserSessionManager();
                    // IList<UserSession> lstUserID = userSessionMngr.GetCurrentSessionByUserName(sUser);

                    //  IList<UserSession> lstUserID = userSessionMngr.GetUserSessionFromXml(sUser);
                    String User = ClientSession.UserName;
                    //Object lstUserID = ht[ClientSession.UserName];
                    IList<string> lstUser = UtilityManager.FindUserSessionFiles(ClientSession.UserName, string.Empty);

                    // UtilityManager.inserttologgingtable(ClientSession.UserName.ToString(), ClientSession.UserRole.ToString(), ClientSession.FacilityName + "; SavedSession is " + ClientSession.SavedSession,
                    //ClientSession.LocalDate + ";" + ClientSession.LocalTime, "Application_AcquireRequestState - FindUserSessionCount - " + lstUser.Count +"; SessionID - "+Session.SessionID+" - " + Request.Url.ToString(), DateTime.Now, "0", "");

                    UtilityManager.inserttologgingtableforSessionTimeout("Application_AcquireRequestState - Checking the FindUserSessionCount", Request.Url.ToString(), "FindUserSessionCount -" + lstUser.Count.ToString());

                    if (lstUser.Count != 0)
                    {

                        isMultiLogIn = false;
                        var objIsActiveSession = (from item in lstUser where item.Contains(Session.SessionID) select item).ToList<string>();

                        // UtilityManager.inserttologgingtable(ClientSession.UserName.ToString(), ClientSession.UserRole.ToString(), ClientSession.FacilityName + "; SavedSession is " + ClientSession.SavedSession,
                        //ClientSession.LocalDate + ";" + ClientSession.LocalTime, "Application_AcquireRequestState - objIsActiveSessionCount - " + objIsActiveSession.Count + "; SessionID - " + Session.SessionID + " - " + Request.Url.ToString(), DateTime.Now, "0", "");
                        UtilityManager.inserttologgingtableforSessionTimeout("Application_AcquireRequestState - Checking the objIsActiveSessionCount", Request.Url.ToString(), "objIsActiveSessionCount - " + objIsActiveSession.Count.ToString());
                        //if (Convert.ToString(lstUserID).Equals(HttpContext.Current.Session.SessionID) == false )//&& Convert.ToString(ht["IsLogin-" + sUser]) == "TRUE")
                        if (objIsActiveSession.Count == 0)
                        {

                            //string filepath = System.Configuration.ConfigurationSettings.AppSettings["UserSessionFolderPath"] + "\\logsession.txt";
                            //string messege = Session.SessionID + "USerName" + ClientSession.UserName + ClientSession.SavedSession;
                            //StreamWriter SW;

                            //if (File.Exists(filepath))
                            //{
                            //    TextWriter tw = new StreamWriter(filepath, true);
                            //    //tw.WriteLine("==============================" + System.Environment.NewLine + ex.StackTrace.ToString() + System.Environment.NewLine + error + System.Environment.NewLine);
                            //    tw.WriteLine(messege + Environment.NewLine);
                            //    tw.Close();
                            //}
                            //else
                            //{
                            //    SW = File.CreateText(filepath);
                            //   // SW.WriteLine("========================= Health Plan Report Generation Error Log Start" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + "==========================" + System.Environment.NewLine + ex.StackTrace.ToString() + error + System.Environment.NewLine);
                            //    SW.WriteLine(messege + Environment.NewLine);
                            //    SW.Close();

                            //}
                            UtilityManager.inserttologgingtableforSessionTimeout("Application_AcquireRequestState - Multilogin True and Abandon the Session", Request.Url.ToString(), string.Empty);
                            isMultiLogIn = true;
                            HttpContext.Current.Session.Abandon();
                            //if (ht.ContainsKey("IsLogin-" + sUser))
                            //{
                            //    ht.Remove("IsLogin-" + sUser);
                            //}
                            HttpContext.Current.Application.Remove("user");
                            if (IsAjaxRequest(HttpContext.Current.Request))
                            {
                                //UtilityManager.inserttologgingtable(ClientSession.UserName.ToString(), ClientSession.UserRole.ToString(), ClientSession.FacilityName + "; SavedSession is " + ClientSession.SavedSession,
                                //ClientSession.LocalDate + ";" + ClientSession.LocalTime, "Redirecting to Session Expired - Linenumber=132 " + " - " + Request.Url.ToString(), DateTime.Now, "0", "");
                                UtilityManager.inserttologgingtableforSessionTimeout("Redirecting to Session Expired due to Multilogin - Linenumber=132 ", Request.Url.ToString(), string.Empty);

                                HttpContext.Current.Response.StatusCode = 999;
                                HttpContext.Current.Response.Status = "999 Session Expired";
                                //HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=132";
                                HttpContext.Current.Response.StatusDescription = "frmSessionExpiredMultiLogin.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=132";
                            }
                            else
                            {
                                // UtilityManager.inserttologgingtable(ClientSession.UserName.ToString(), ClientSession.UserRole.ToString(), ClientSession.FacilityName + "; SavedSession is " + ClientSession.SavedSession,
                                //ClientSession.LocalDate + ";" + ClientSession.LocalTime, "Redirecting to Session Expired - Linenumber=135 " + " - " + Request.Url.ToString(), DateTime.Now, "0", "");

                                UtilityManager.inserttologgingtableforSessionTimeout("Redirecting to Session Expired due to Multilogin - Linenumber=135 ", Request.Url.ToString(), string.Empty);

                                //Response.Redirect("~/frmSessionExpired.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=135");
                                Response.Redirect("~/frmSessionExpiredMultiLogin.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=135");
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
                        UtilityManager.inserttologgingtableforSessionTimeout("Application_AcquireRequestState - Multilogin True and abandoning Session", Request.Url.ToString(), string.Empty);

                        isMultiLogIn = true;
                        HttpContext.Current.Session.Abandon();
                        HttpContext.Current.Application.Remove("user");
                        //if (ht.ContainsKey("IsLogin-" + sUser))
                        //{
                        //    ht.Remove("IsLogin-" + sUser);
                        //}
                        if (IsAjaxRequest(HttpContext.Current.Request))
                        {
                            //UtilityManager.inserttologgingtable(ClientSession.UserName.ToString(), ClientSession.UserRole.ToString(), ClientSession.FacilityName + "; SavedSession is " + ClientSession.SavedSession,
                            //ClientSession.LocalDate + ";" + ClientSession.LocalTime, "Redirecting to Session Expired - Linenumber=167 " + " - " + Request.Url.ToString(), DateTime.Now, "0", "");

                            UtilityManager.inserttologgingtableforSessionTimeout("Redirecting to Session Expired due to Multilogin - Linenumber=167 ", Request.Url.ToString(), string.Empty);

                            HttpContext.Current.Response.StatusCode = 999;
                            HttpContext.Current.Response.Status = "999 Session Expired";
                            //HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=167";
                            HttpContext.Current.Response.StatusDescription = "frmSessionExpiredMultiLogin.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=167";
                        }
                        else
                        {
                            //        UtilityManager.inserttologgingtable(ClientSession.UserName.ToString(), ClientSession.UserRole.ToString(), ClientSession.FacilityName + "; SavedSession is " + ClientSession.SavedSession,
                            //ClientSession.LocalDate + ";" + ClientSession.LocalTime, "Redirecting to Session Expired - Linenumber=170 " + " - " + Request.Url.ToString(), DateTime.Now, "0", "");
                            UtilityManager.inserttologgingtableforSessionTimeout("Redirecting to Session Expired due to Multilogin - Linenumber=170 ", Request.Url.ToString(), string.Empty);

                            // Response.Redirect("~/frmSessionExpired.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=170");
                            Response.Redirect("~/frmSessionExpiredMultiLogin.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=170");
                            //HttpContext.Current.Response.StatusCode = 999;
                            //HttpContext.Current.Response.Status = "999 Session Expired";
                            //HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=174";
                        }
                    }
                }
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // UtilityManager.inserttologgingtableforSessionTimeout("Application_AuthenticateRequest - Start", Request.Url.ToString(), string.Empty);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            UtilityManager.inserttologgingtableforSessionTimeout("Application_Error - Start", Request.Url.ToString(), string.Empty);

            try
            {
                if (Server.GetLastError() != null)
                {
                    UtilityManager.inserttologgingtableforSessionTimeout("Application_Error - Get the Base Exception details", Request.Url.ToString(), string.Empty);

                    Exception objErr = Server.GetLastError().GetBaseException();

                    string message = System.Environment.NewLine + System.Environment.NewLine + "------------------------------BEGINNING OF THIS EXCEPTION------------------------------------" + System.Environment.NewLine + System.Environment.NewLine +
                      "MESSAGE: " + objErr.Message + System.Environment.NewLine +
                      "TYPE: " + objErr.GetType() + System.Environment.NewLine +
                      "TIME: " + DateTime.Now.ToString() + " . UTC TIME: " + DateTime.UtcNow.ToString() + System.Environment.NewLine +
                      "SOURCE: " + objErr.Source + System.Environment.NewLine +
                      //"FORM: " + Request.Form.ToString() + System.Environment.NewLine +
                      //"QUERYSTRING: " + Request.QueryString.ToString() + System.Environment.NewLine +
                      "TARGETSITE: " + objErr.TargetSite + System.Environment.NewLine;  // +
                    // ClientSession.UserName!=null? "CURRENT USER: " + Convert.ToString(ClientSession.UserName)+ System.Environment.NewLine : string.Empty  +
                    // ClientSession.EncounterId!=null? "ENCOUNTER ID: " + Convert.ToString(ClientSession.EncounterId) + System.Environment.NewLine :string.Empty +
                    //ClientSession.HumanId != null ? "HUMAN ID: " + Convert.ToString(ClientSession.HumanId) + System.Environment.NewLine : string.Empty +
                    //ClientSession.PhysicianId != null ? "PHYSICIAN ID: " + Convert.ToString(ClientSession.PhysicianId) + System.Environment.NewLine : string.Empty +
                    //"STACKTRACE: " + objErr.StackTrace;

                    if (objErr.Message != null && objErr.Message == "Unable to connect to Databse.")
                    {
                        throw new Exception("Unable to connect to Databse.");
                    }

                    try
                    {
                        if (ClientSession.UserName != null)
                            message += "CURRENT USER: " + Convert.ToString(ClientSession.UserName) + System.Environment.NewLine;
                        if (ClientSession.EncounterId != null)
                            message += "ENCOUNTER ID: " + Convert.ToString(ClientSession.EncounterId) + System.Environment.NewLine;
                        if (ClientSession.HumanId != null)
                            message += "HUMAN ID: " + Convert.ToString(ClientSession.HumanId) + System.Environment.NewLine;
                        if (ClientSession.PhysicianId != null)
                            message += "PHYSICIAN ID: " + Convert.ToString(ClientSession.PhysicianId) + System.Environment.NewLine;
                    }
                    catch
                    {

                    }

                    message += "STACKTRACE: " + objErr.StackTrace;

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

                    //Gitlab #3897 - Handling the Unhandled Exception - Start
                    string sMessage = "";
                    string statserrorlogstacktrace = "";

                    if (exc != null && exc.Message != null )
                        sMessage = exc.Message;

                    if (exc != null && exc.StackTrace != null)
                        statserrorlogstacktrace = exc.StackTrace;
                    if (exc != null && exc.InnerException != null && exc.InnerException.Message != null && sMessage == string.Empty)
                    {
                        sMessage = sMessage + exc.InnerException.Message;
                    }
                    if (exc != null && exc.InnerException != null && exc.InnerException.StackTrace != null && sMessage == string.Empty)
                    {
                        statserrorlogstacktrace = statserrorlogstacktrace + exc.InnerException.StackTrace;
                    }

                    string version = "";
                    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                    string[] server = version.Split('|');
                    string serverno = "";
                    if (server.Length > 1)
                        serverno = server[1].Trim();
                    //Gitlab #3897 - Handling the Unhandled Exception - End 

                    if (exc != null && exc.Message != null)
                    {
                        if ((exc.Message.Contains("System.Web.HttpUnhandledException") || exc.Message.Contains("Script controls may not be registered after PreRender")) && ((exc.Message.Contains("Row was updated or deleted by another transaction") == false) && ((exc.InnerException != null) ? (exc.InnerException.Message.Contains("Row was updated or deleted by another transaction") == false) : true)))
                        {
                            //Gitlab #3897 - Handling the Unhandled Exception - Start
                            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMessage.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + "', '" + serverno + "','" + DateTime.Now + "','" + ClientSession.UserName + "','" + ClientSession.EncounterId + "','" + ClientSession.HumanId + "','" + ClientSession.PhysicianId + "','" + statserrorlogstacktrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                            int iReturn = DBConnector.WriteData(insertQuery);
                            //Gitlab #3897 - Handling the Unhandled Exception - End

                            Server.ClearError();
                            message += "ERROR HANDLING METHOD : Server.ClearError()" + System.Environment.NewLine + System.Environment.NewLine;
                        }
                        else
                        {
                            try
                            {
                                if (exc.Message.IndexOf("Transaction XML") > -1)
                                {
                                    //Gitlab #3897 - Handling the Unhandled Exception - Start
                                    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMessage.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + "', '" + serverno + "','" + DateTime.Now + "','" + ClientSession.UserName + "','" + ClientSession.EncounterId + "','" + ClientSession.HumanId + "','" + ClientSession.PhysicianId + "','" + statserrorlogstacktrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                    int iReturn = DBConnector.WriteData(insertQuery);
                                    //Gitlab #3897 - Handling the Unhandled Exception - End
                                    //  Console.Error=function (){alert}
                                    //System.Web.UI.Page Errorpage = (System.Web.UI.Page)HttpContext.Current.Handler;
                                    //Errorpage.ClientScript.RegisterClientScriptBlock(GetType(), "MyScriptKey", " alert('XML is not found.Kindly contact support. ');", true);
                                    //   Response.Write("<script type='text/javascript'> alert('XML is not found.Kindly contact support. ');</script>");
                                }
                                else if (exc.Message.IndexOf("To Process is not found") > -1)//BugID:53884
                                {
                                    //Gitlab #3897 - Handling the Unhandled Exception - Start
                                    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMessage.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + "', '" + serverno + "','" + DateTime.Now + "','" + ClientSession.UserName + "','" + ClientSession.EncounterId + "','" + ClientSession.HumanId + "','" + ClientSession.PhysicianId + "','" + statserrorlogstacktrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                    int iReturn = DBConnector.WriteData(insertQuery);
                                    //Gitlab #3897 - Handling the Unhandled Exception - End
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
            catch
            {

            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            //UtilityManager.inserttologgingtable(ClientSession.UserName.ToString(), ClientSession.UserRole.ToString(), ClientSession.FacilityName,
            //     ClientSession.LocalDate + ";" + ClientSession.LocalTime, "Session is ending - Session Timeout limit is - " + Session.Timeout, DateTime.Now, "0", "frmSessionExpires");
            UtilityManager.inserttologgingtableforSessionTimeout("Session is ending", string.Empty, string.Empty);

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
        }

        protected void Application_End(object sender, EventArgs e)
        {
            UtilityManager.inserttologgingtableforSessionTimeout("Application_End",string.Empty, string.Empty);

            HttpRuntime runtime = (HttpRuntime)typeof(System.Web.HttpRuntime).InvokeMember("_theRuntime",

                                                                                    BindingFlags.NonPublic

                                                                                    | BindingFlags.Static

                                                                                    | BindingFlags.GetField,

                                                                                    null,

                                                                                    null,

                                                                                    null);



            if (runtime == null)

                return;



            string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage",

                                                                             BindingFlags.NonPublic

                                                                             | BindingFlags.Instance

                                                                             | BindingFlags.GetField,

                                                                             null,

                                                                             runtime,

                                                                             null);



            string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack",

                                                                           BindingFlags.NonPublic

                                                                           | BindingFlags.Instance

                                                                           | BindingFlags.GetField,

                                                                           null,

                                                                           runtime,

                                                                           null);



            if (!EventLog.SourceExists(".NET Runtime"))
            {

                EventLog.CreateEventSource(".NET Runtime", "Application");

            }



            EventLog log = new EventLog();

            log.Source = ".NET Runtime";

            log.WriteEntry(String.Format("\r\n\r\n_shutDownMessage={0}\r\n\r\n_shutDownStack={1}",

                                         shutDownMessage,

                                         shutDownStack),

                                         EventLogEntryType.Error);
        }

        void CheckLogin()
        {
            string SessionData = string.Empty;

            UtilityManager.inserttologgingtableforSessionTimeout("CheckLogin - Check if Rawurl contains aspx", Request.Url.ToString(), string.Empty);

            if (Request.RawUrl.Contains(".aspx"))
            {
                string Url = Request.RawUrl;
                string[] Url1 = Url.Split('/');
                string Link = Url;// Url1[Url1.Length - 1].ToString();
                try
                {
                    UtilityManager.inserttologgingtableforSessionTimeout("CheckLogin - Check if session is null", Request.Url.ToString(), string.Empty);

                    if (Session != null)
                        SessionData = ClientSession.UserName;

                    if (SessionData == "" && Link.ToUpper().Contains("FRMLOGIN.ASPX") == false && Link.ToUpper() != "FRMLOGIN.ASPX" && Link.ToUpper().Contains("WEBFRMLOGOUT.ASPX") == false && Link.ToUpper().Contains("WEBFRMLOGIN.ASPX") == false && Link.ToUpper().Contains("FRMIMPERSONATEUSER.ASPX") == false && Link.ToUpper().Contains("FRMSELECTLEGALORG.ASPX") == false && Link.ToUpper().Contains("FRMLANDINGSCREEN.ASPX") == false && Link.ToUpper().Contains("FRMLOGINNEW.ASPX") == false)
                    {
                        UtilityManager.inserttologgingtableforSessionTimeout("CheckLogin - session is empty and URL is not Login Page", Request.Url.ToString(), string.Empty);

                        // Added By manimaran for Bug Id-28418 on 10-12-2014
                        //if (Link.ToUpper().Contains("EMAIL") == true)
                        //{
                        //    string[] SplitLink = Link.Split('?');
                        //    if (Convert.ToString(SplitLink[1]) != string.Empty)
                        //    {
                        //        if (SplitLink[1].Contains("ScreenMode"))
                        //            SplitLink[1] = SplitLink[1].Substring((SplitLink[1].IndexOf("&") + 1), (SplitLink[1].Length - 1) - (SplitLink[1].IndexOf("&")));
                        //        Response.Redirect("~/frmSessionExpired.aspx?" + SplitLink[1].ToString());
                        //    }
                        //}
                        // else
                        // {

                        if (IsAjaxRequest(HttpContext.Current.Request))
                        {
                            UtilityManager.inserttologgingtableforSessionTimeout("CheckLogin - Redirecting to Session Expired - line number 489 ", Request.Url.ToString(), string.Empty);

                            HttpContext.Current.Response.StatusCode = 999;
                            HttpContext.Current.Response.Status = "999 Session Expired";
                        }
                        if (isMultiLogIn == true)
                        {
                            UtilityManager.inserttologgingtableforSessionTimeout("Multilogin True", Request.Url.ToString(), string.Empty);
                            if (IsAjaxRequest(HttpContext.Current.Request))
                            {
                                UtilityManager.inserttologgingtableforSessionTimeout("Check Login - Redirecting to Session Expired due to Multilogin - line number 382 in global.asax", Request.Url.ToString(), string.Empty);
                                //HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=382";
                                HttpContext.Current.Response.StatusDescription = "frmSessionExpiredMultiLogin.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=382";
                            }
                            else
                            {
                                UtilityManager.inserttologgingtableforSessionTimeout("Check Login - Redirecting to Session Expired due to Multilogin - line number 384 in global.asax", Request.Url.ToString(), string.Empty);
                                // Response.Redirect("~/frmSessionExpired.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=384");
                                Response.Redirect("~/frmSessionExpiredMultiLogin.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=384");
                            }
                            isMultiLogIn = false;
                        }
                        else
                        {
                            if (IsAjaxRequest(HttpContext.Current.Request))
                            {
                                UtilityManager.inserttologgingtableforSessionTimeout("Check Login - Redirecting to Session Expired due to Timeout or Unknown reason - line number 391 in global.asax", Request.Url.ToString(), string.Empty);
                                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx?From=Globalasaxcs|Linenumber=391";

                            }
                            else
                            {
                                UtilityManager.inserttologgingtableforSessionTimeout("Check Login - Redirecting to Session Expired due to Timeout or Unknown reason - line number 395 in global.asax", Request.Url.ToString(), string.Empty);
                                //CAP-1075
                                //CAP-1167
                                //CAP-2316
                                var currentURL = Request.Url.AbsoluteUri.ToString();
                                if (DirectURLUtility.IsValidRedirectUrlForLogin(currentURL))
                                {
                                    var CurrentUrl = HttpUtility.UrlDecode(Session["currenturl"]?.ToString());

                                    if (!string.IsNullOrEmpty(CurrentUrl))
                                    {
                                        //CAP-1752
                                        var loginpage = (ConfigurationSettings.AppSettings["IsSSOLogin"] == "Y" ? "frmLoginNew.aspx" : "frmLogin.aspx");
                                        var returnURL = string.IsNullOrEmpty(CurrentUrl) ? $"~/{loginpage}" : $"~/{loginpage}?redirecturl={HttpUtility.UrlEncode(CurrentUrl)}";
                                        Session["currenturl"] = null;
                                        Response.Redirect(returnURL);
                                    }
                                }
                                else
                                {
                                    Response.Redirect("~/frmSessionExpired.aspx?From=Globalasaxcs|Linenumber=395");
                                }

                            }
                            //if ( SessionData == "")
                            //{
                            //    HttpContext.Current.Response.StatusCode = 999;
                            //    HttpContext.Current.Response.Status = "999 Session Expired";
                            //    HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                            //    Response.Redirect("~/frmSessionExpired.aspx");
                            //}
                        }
                        //  }
                    }
                }
                catch
                {
                    if (IsAjaxRequest(HttpContext.Current.Request))
                    {
                        UtilityManager.inserttologgingtableforSessionTimeout("CheckLogin - Redirecting to Session Expired line number 537", Request.Url.ToString(), string.Empty);

                        HttpContext.Current.Response.StatusCode = 999;
                        HttpContext.Current.Response.Status = "999 Session Expired";
                    }
                    if (isMultiLogIn == true)
                    {
                        UtilityManager.inserttologgingtableforSessionTimeout("Check Login - Multilogin True", Request.Url.ToString(), string.Empty);
                        if (IsAjaxRequest(HttpContext.Current.Request))
                        {
                            UtilityManager.inserttologgingtableforSessionTimeout("Check Login - Redirecting to Session Expired due to Multilogin - line number 417 in global.asax", Request.Url.ToString(), string.Empty);
                            //HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=417";
                            HttpContext.Current.Response.StatusDescription = "frmSessionExpiredMultiLogin.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=417";
                        }
                        else
                        {
                            UtilityManager.inserttologgingtableforSessionTimeout("Check Login - Redirecting to Session Expired due to Multilogin - line number 419 in global.asax", Request.Url.ToString(), string.Empty);
                            //Response.Redirect("~/frmSessionExpired.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=419");
                            Response.Redirect("~/frmSessionExpiredMultiLogin.aspx?Reason=Abandoned&From=Globalasaxcs|Linenumber=419");
                        }
                        isMultiLogIn = false;
                    }
                    else
                    {
                        if (SessionData == "")
                        {
                            UtilityManager.inserttologgingtableforSessionTimeout("Check Login - Redirecting to Session Expired due to Timeout or Unknown reason - line number 429 in global.asax", Request.Url.ToString(), string.Empty);

                            HttpContext.Current.Response.StatusCode = 999;
                            HttpContext.Current.Response.Status = "999 Session Expired";
                            HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx?From=Globalasaxcs|Linenumber=428";
                            Response.Redirect("~/frmSessionExpired.aspx?From=Globalasaxcs|Linenumber=429");
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
            UtilityManager.inserttologgingtableforSessionTimeout("IsAjaxRequest - Check if request is ajax request", Request.Url.ToString(), string.Empty);

            if (request == null)
            {
                UtilityManager.inserttologgingtableforSessionTimeout("IsAjaxRequest - request is null", Request.Url.ToString(), string.Empty);

                throw new ArgumentNullException("request");
            }
            UtilityManager.inserttologgingtableforSessionTimeout("IsAjaxRequest - check if context is null", Request.Url.ToString(), string.Empty);

            var context = HttpContext.Current;
            var isCallbackRequest = false;// callback requests are ajax requests
            if (context != null && context.CurrentHandler != null && context.CurrentHandler is System.Web.UI.Page)
            {
                isCallbackRequest = ((System.Web.UI.Page)context.CurrentHandler).IsCallback;
            }

            UtilityManager.inserttologgingtableforSessionTimeout("IsAjaxRequest - return - " + isCallbackRequest + " and " + (request["X-Requested-With"] == "XMLHttpRequest") + " and " + (request.Headers["X-Requested-With"] == "XMLHttpRequest"), Request.Url.ToString(), string.Empty);

            return isCallbackRequest || (request["X-Requested-With"] == "XMLHttpRequest") || (request.Headers["X-Requested-With"] == "XMLHttpRequest");
        }



        protected void Application_EndRequest(object sender, EventArgs e)
        {
            UtilityManager.inserttologgingtableforSessionTimeout("Application_EndRequest - Start", string.Empty, string.Empty);
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            UtilityManager.inserttologgingtableforSessionTimeout("Application_PreRequestHandlerExecute - Start", string.Empty, string.Empty);
        }

        protected void Application_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            UtilityManager.inserttologgingtableforSessionTimeout("Application_PostRequestHandlerExecute - Start", Request.Url.ToString(), string.Empty);
        }










    }
}