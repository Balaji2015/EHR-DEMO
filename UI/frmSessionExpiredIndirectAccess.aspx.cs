using Acurus.Capella.DataAccess.ManagerObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Acurus.Capella.UI
{
    public partial class frmSessionExpiredIndirectAccess : System.Web.UI.Page
    {
        public static bool bFirstTime = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            //CAP-2475
            //if (Request.Cookies["MicrosoftAccessTokenId"] != null && !string.IsNullOrEmpty(Request.Cookies["MicrosoftAccessTokenId"].Value))
            //{
            //    //CAP-2337
            //    var postLogoutRedirectUri = string.Empty;
            //    if ((Request?.Headers["X-Forwarded-Host"] ?? "") == ConfigurationSettings.AppSettings["AkidoChartDomain"])
            //    {
            //        string subdomain = Request.Url.Authority.Contains("test6") ? "" : "";

            //        if (string.IsNullOrWhiteSpace(subdomain))
            //        {
            //            postLogoutRedirectUri = $"https://{ConfigurationSettings.AppSettings["AkidoChartDomain"]}/frmSessionExpiredIndirectAccess.aspx";

            //        }
            //        else
            //        {
            //            postLogoutRedirectUri = $"https://{ConfigurationSettings.AppSettings["AkidoChartDomain"]}/{subdomain}/frmSessionExpiredIndirectAccess.aspx";

            //        }
            //    }
            //    else
            //    {
            //        postLogoutRedirectUri = ConfigurationSettings.AppSettings["okta:PostLogoutSessionIndirectRedirectUri"];
            //    }
            //    Response.Redirect($"{ConfigurationManager.AppSettings["okta:LogoutURL"]}?id_token_hint={Request.Cookies["MicrosoftAccessTokenId"].Value}&post_logout_redirect_uri={postLogoutRedirectUri}", false);
            //    HttpCookie cookie = Request.Cookies["MicrosoftAccessTokenId"];
            //    cookie.Expires = DateTime.Now.AddMinutes(-5);
            //    Response.Cookies.Add(cookie);
            //    return;
            //}

            if (Request.Cookies["RedirectUri"] != null && !string.IsNullOrEmpty(Request.Cookies["RedirectUri"].Value))
            {
                HttpCookie cookie = Request.Cookies["RedirectUri"];
                cookie.Expires = DateTime.Now.AddMinutes(-5);
                Response.Cookies.Add(cookie);
            }

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
                    //CAP-1752,CAP-2316
                    var loginpage = (ConfigurationSettings.AppSettings["IsSSOLogin"] == "Y" ? "frmLoginNew.aspx?IsLoginRequired=true" : "frmLogin.aspx");
                    Response.Write($"<script> window.top.location.href=\"{loginpage}\"; </script>");
                }
                bFirstTime = false;
            }
            //Response.Write("<script> window.top.location.href=\" frmLogin.aspx\"; </script>");
        }
    }
}