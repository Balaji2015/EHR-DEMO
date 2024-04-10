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
using System.IO;
using Acurus.Capella.DataAccess;
namespace Acurus.Capella.UI
{
    public partial class frmSessionExpired : System.Web.UI.Page
    {
        public static bool bFirstTime = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.Cookies["MicrosoftAccessTokenId"] != null && !string.IsNullOrEmpty(Request.Cookies["MicrosoftAccessTokenId"].Value))
            {
                var postLogoutRedirectUri = ConfigurationSettings.AppSettings["okta:PostLogoutSessionExpiredRedirectUri"];
                Response.Redirect($"{ConfigurationManager.AppSettings["okta:LogoutURL"]}?id_token_hint={Request.Cookies["MicrosoftAccessTokenId"].Value}&post_logout_redirect_uri={postLogoutRedirectUri}", false);
                HttpCookie cookie = Request.Cookies["MicrosoftAccessTokenId"];
                cookie.Expires = DateTime.Now.AddMinutes(-5);
                Response.Cookies.Add(cookie);
                return;
            }

            if (Request.Cookies["RedirectUri"] != null && !string.IsNullOrEmpty(Request.Cookies["RedirectUri"].Value)) 
            {
                HttpCookie cookie = Request.Cookies["RedirectUri"];
                cookie.Expires = DateTime.Now.AddMinutes(-5);
                Response.Cookies.Add(cookie);
            }

                //Jira CAP-1567
                if (Request.Cookies["CeRxFlag"] != null && Request.Cookies["CeRxFlag"].Value == "true" && Request.Cookies["CeRxHumanID"].Value != "" && Request.Cookies["CeRxHumanID"].Value != "0")
            {
                string sErrorMessage = string.Empty;

                ulong ulHuman_id = Convert.ToUInt64(Request.Cookies["CeRxHumanID"].Value);
                string sUserName = Request.Cookies["CUserName"].Value;
                string sFacilityName = Request.Cookies["CFacilityName"].Value;
                string sLegalOrg = Request.Cookies["CLegalOrg"].Value;
                Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
                RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
                DateTime dtClientDate = DateTime.UtcNow;
                if (sUserName != null && sFacilityName != null && sUserName != "" && sFacilityName != "")
                    sErrorMessage = objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.DownloadAddress, sUserName, string.Empty, dtClientDate, sFacilityName, 0, ulHuman_id, sLegalOrg);
            }
            Response.SetCookie(new HttpCookie("CeRxHumanID") { Value = "", HttpOnly = false });
            Response.SetCookie(new HttpCookie("CeRxFlag") { Value = "false", HttpOnly = false });

            UserSessionManager userSessionMngr = new UserSessionManager();
          //  userSessionMngr.DeleteUserSessionAtSessionEnd(HttpContext.Current.Session.SessionID); 
            //Global.ht.Remove(ClientSession.UserName);
           // userSessionMngr.DeleteUserSessionFromXml(HttpContext.Current.Session.SessionID);
            UtilityManager.DeleteUserSessionFile(string.Empty, Session.SessionID);
            /* To clean the user-specific temp directory */
            if (Directory.Exists(Session.SessionID))
            {
                try { Directory.Delete(Session.SessionID); }
                catch { /* Intentionally Left Blank */ }
            }

            if (bFirstTime != true)
            {
                Response.Write("<script> window.top.location.href=\" " + Request.RawUrl + "\"; </script>");
                bFirstTime = true;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "if(top.window.document.getElementById('ctl00_Loading')!=null||top.window.document.getElementById('ctl00_Loading')!=undefined)top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
            if (Request.QueryString["Reason"] != null && Request.QueryString["Reason"].ToString() == "Abandoned")
                Label1.Text = "This session has expired since you logged in from another system. <br /> <br />" + "Date Time : " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt");
            else
                Label1.Text += "<br /> <br />Date Time : " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //Server.Transfer("frmLogin.aspx");
            // Added By manimaran for Bug Id-28418 on 10-12-2014
            if (Request.RawUrl.Contains(".aspx"))
            {
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
                    //CAP-1167
                    var CurrentUrl = Session["currenturl"]?.ToString();
                    //CAP-1752
                    var loginpage = (ConfigurationSettings.AppSettings["IsSSOLogin"] == "Y" ? "frmLoginNew.aspx" : "frmLogin.aspx");
                    if (!string.IsNullOrEmpty(CurrentUrl))
                    {
                        var returnURL = $"~/{loginpage}?redirecturl={HttpUtility.UrlEncode(CurrentUrl)}";
                        Session["currenturl"] = null;
                        Response.Write("<script> window.top.location.href=\"" + returnURL + "\"; </script>");
                    }
                    else
                    {
                        Response.Write($"<script> window.top.location.href=\"{loginpage}\"; </script>");
                    }
                  
                    
                }
                bFirstTime = false;
            }
            //Response.Write("<script> window.top.location.href=\" frmLogin.aspx\"; </script>");
        }
    }
}
