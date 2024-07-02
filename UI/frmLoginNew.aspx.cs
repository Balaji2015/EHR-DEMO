using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.Collections;
using System.Linq;
using System.IO;
using System.Web.Services;
using System.Collections.Specialized;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Drawing.Charts;
using Acurus.Capella.UI.Extensions;
using System.Configuration;
using RestSharp;
using Acurus.Capella.UI.OktaResponseModel;
using System.Text.RegularExpressions;

namespace Acurus.Capella.UI
{
    public partial class frmLoginNew : System.Web.UI.Page
    {
        UserManager UserMngr = new UserManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigurationSettings.AppSettings["IsSSOLogin"] == null || ConfigurationSettings.AppSettings["IsSSOLogin"] == "N")
            {
                Response.Redirect("frmLogin.aspx");
                return;
            }         

            DateTime dtStartTime = DateTime.Now;

            if (hdnGroupId != null && hdnGroupId.Value == "")
            {
                hdnGroupId.Value = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + dtStartTime.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            }

            if (Request["OpenPatChart"] != null && Request["OpenPatChart"] == "true")//Added for CarePointe
            {
                UtilityManager.inserttologgingtableforSessionTimeout("Login Page Load - before calling btnOk_Click for redirection", Request.Url.ToString(), string.Empty);


                UtilityManager.inserttologgingtableforSessionTimeout("Login Page Load - After calling btnOk_Click for redirection", Request.Url.ToString(), string.Empty);

            }
            if (!IsPostBack)
            {
                //CAP-2171
                if (Request.Url.Authority == (ConfigurationManager.AppSettings["RootURL"] ?? ""))
                {
                    Response.SetCookie(new HttpCookie("IsOktaUser") { Value = "Y", Expires = DateTime.Now.AddDays(1) });
                }

                if (Request.Cookies["IsOktaUser"] == null || (Request.Cookies["IsOktaUser"]?.Value ?? "N") == "N")
                {
                    var oktaVerificationURL = CheckOktaAuthorizationUrl();
                    Response.Redirect(oktaVerificationURL, false);
                    return;
                }

                if (Request.Form["EHRUserName"] == null)
                {
                    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Login : Start", dtStartTime, hdnGroupId.Value, "frmLoginNew");
                    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Login PageLoad : Start", DateTime.Now, hdnGroupId.Value, "frmLoginNew");
                }
            }
            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                hdnVersion.Value = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"];
            if (ClientSession.LegalOrg != null)
                hdnProjectName.Value = ClientSession.LegalOrg;

            if (System.Configuration.ConfigurationSettings.AppSettings["Reportpath"] != null)
                hdnreportPath.Value = System.Configuration.ConfigurationSettings.AppSettings["Reportpath"];
            if (System.Configuration.ConfigurationSettings.AppSettings["LoginHeader"] != null)
                hdnLoginheader.Value = System.Configuration.ConfigurationSettings.AppSettings["LoginHeader"];
            if (System.Configuration.ConfigurationSettings.AppSettings["versionkey"] != null)
                hdnVersionKey.Value = System.Configuration.ConfigurationSettings.AppSettings["versionkey"];
            if (System.Configuration.ConfigurationSettings.AppSettings["EVServiceLink"] != null)
                hdnServiceLink.Value = System.Configuration.ConfigurationSettings.AppSettings["EVServiceLink"];
            if (System.Configuration.ConfigurationSettings.AppSettings["EVProjectName"] != null)
                hdnEvProjectName.Value = System.Configuration.ConfigurationSettings.AppSettings["EVProjectName"];
            //ClientSession.LocalOffSetTime = hdnLocalTime.Value;
            //ClientSession.LocalDate = hdnLocalDate.Value;
            //ClientSession.UniversalTime = hdnUniversaloffset.Value;
            //ClientSession.LocalTime = hdnLocalDateAndTime.Value;
            //if (hdnFollowsDayLightSavings.Value.ToLower() == "true")
            //    ClientSession.bFollows_DST = true;
            //else
            //    ClientSession.bFollows_DST = false;

            var bFollows_DST = false;
            if (hdnFollowsDayLightSavings.Value.ToLower() == "true")
                bFollows_DST = true;

            Response.SetCookie(new HttpCookie("LocalOffSetTime") { Value = hdnLocalTime.Value, Expires = DateTime.Now.AddDays(1) });
            Response.SetCookie(new HttpCookie("LocalDate") { Value = hdnLocalDate.Value, Expires = DateTime.Now.AddDays(1) });
            Response.SetCookie(new HttpCookie("UniversalTime") { Value = hdnUniversaloffset.Value, Expires = DateTime.Now.AddDays(1) });
            Response.SetCookie(new HttpCookie("LocalTime") { Value = hdnLocalDateAndTime.Value, Expires = DateTime.Now.AddDays(1) });
            Response.SetCookie(new HttpCookie("bFollows_DST") { Value = bFollows_DST.ToString(), Expires = DateTime.Now.AddDays(1) });

            if (!string.IsNullOrWhiteSpace(Request.QueryString["redirecturl"]))
            {
                var returnUrl = HttpUtility.UrlDecode(Request.QueryString["redirecturl"]);
                Response.SetCookie(new HttpCookie("RedirectUri") { Value = returnUrl, Expires = DateTime.Now.AddDays(1) });
            }
            else
            {
                HttpCookie cookie = Request.Cookies["RedirectUri"];
                if(cookie != null) { 
                cookie.Expires = DateTime.Now.AddMinutes(-5);
                Response.Cookies.Add(cookie);
                    }
            }

        }


        protected void btnNextLogin_Click(object sender, EventArgs e)
        {
            if (txtUserName.Value != null && txtUserName.Value != "")
            {
                var oktaDomain = ConfigurationSettings.AppSettings["okta:OktaDomain"];
                var client = new RestClient(oktaDomain);
                var request = new RestRequest(ConfigurationSettings.AppSettings["okta:WebFinger"], Method.Get);
                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
                request.AddParameter("resource", $"acct:{txtUserName.Value}");
                request.AddParameter("rel", "okta:idp");
                RestResponse response = client.ExecuteAsync(request).Result;

                OktaUserIDPModel result = JsonConvert.DeserializeObject<OktaUserIDPModel>(response.Content);

                if ((result?.links?.Count ?? 0) > 0)
                {
                    if ((result.links[0]?.titles?.und ?? string.Empty).Equals("Azure IDP", StringComparison.InvariantCultureIgnoreCase))
                    {
                        hdnOktaAccountType.Value = "Azure IDP";

                        var regexPattern = "^[^\\.\\s][\\w\\-]+(\\.[\\w\\-]+)*@([\\w-]+\\.)+[\\w-]{2,}$";
                        var isEmail = Regex.IsMatch(txtUserName.Value, regexPattern);
                        if (isEmail)
                        {
                            //ClientSession.UserAccountType = "Microsoft";
                            var redirectURL = GetOktaAuthorizationUrl(txtUserName.Value);
                            Response.Redirect(redirectURL,
                             false);
                        }
                        else
                        {
                            txtUserName.Disabled = true;
                            txtPassword.Visible = true;
                            btnSignin.Visible = true;
                            btnNext.Visible = false;
                            divpanelsucess.Style.Add("height", "301px");
                        }
                    }
                    else
                    {
                        hdnOktaAccountType.Value = "Okta IDP";
                        txtUserName.Disabled = true;
                        txtPassword.Visible = true;
                        btnSignin.Visible = true;
                        btnNext.Visible = false;
                        divpanelsucess.Style.Add("height", "301px");
                    }
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('010025')", true);
                }
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('010002');", true);
            }
        }

        protected void btnSignin_Click(object sender, EventArgs e)
        {
            if (hdnOktaAccountType.Value == "Okta IDP")
            {
                var options = new RestClientOptions(ConfigurationSettings.AppSettings["okta:OktaDomain"])
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest($"{ConfigurationSettings.AppSettings["okta:Authentication"]}", Method.Post);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"SSWS {ConfigurationSettings.AppSettings["okta:APIToken"]}");
                var body = @"{" + "\n" +
                     @"  ""username"": """ + txtUserName.Value + @"""," + "\n" +
                     @"  ""password"": """ + txtPassword.Value + @"""," + "\n" +
                     @"  ""options"": {" + "\n" +
                     @"    ""multiOptionalFactorEnroll"": true," + "\n" +
                     @"    ""warnBeforePasswordExpired"": true" + "\n" +
                     @"  }" + "\n" +
                     @"}";
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = client.ExecuteAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    OktaUserResponseModel result = JsonConvert.DeserializeObject<OktaUserResponseModel>(response.Content);
                    //ClientSession.EmailAddress = result?._embedded?.user?.profile?.login ?? string.Empty;
                    //CAP-2142
                    var oktaURL = GetOktaUrl(result.sessionToken);
                    Response.Redirect(oktaURL, false);
                    //ClientSession.UserAccountType = "Okta";
                    //Response.Redirect($"~/frmLandingScreen.aspx?UserAccountType=Okta&EMailAddress={ClientSession.EmailAddress}", false);
                    //Server.Transfer($"~/frmLandingScreen.aspx?UserAccountType=Okta&EMailAddress={sEmailAddress}");
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        IList<User> login = new List<User>();
                        LoginDTO objLoginDTO = new LoginDTO();
                        bool Base64Pwd = true;
                        objLoginDTO = UserMngr.CheckUserDetails(txtUserName.Value.Trim(), Encryptionbase64Encode(txtPassword.Value), out Base64Pwd, true);
                        if (objLoginDTO != null)// objLoginDTO.lstLookUp != null)
                        {
                            login = objLoginDTO.User;
                            //!objLoginDTO.Any(x => x.Is_Direct_Login.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                            //if (objLoginDTO.User.Count > 0 && objLoginDTO.User.Any(x => x.Is_Direct_Login.Equals("Y", StringComparison.InvariantCultureIgnoreCase)))
                                if (objLoginDTO.User.Count > 0)
                                {
                                //ClientSession.UserName = login[0].user_name;
                                //ClientSession.EmailAddress = login[0].EMail_Address;
                                //ClientSession.UserAccountType = "Capella";
                                //Response.Redirect($"~/frmLandingScreen.aspx?UserAccountType=Capella&RequestedUserName={ClientSession.UserName}", false);
                                Server.Transfer($"~/frmLandingScreen.aspx?UserAccountType=Capella&RequestedUserName={login[0].user_name}");
                            }
                            else
                            {
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('010001');", true);
                            }
                        }
                        else
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('010001');", true);
                        }
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('010025')", true);
                    }
                }
            }
            else
            {
                IList<User> login = new List<User>();
                LoginDTO objLoginDTO = new LoginDTO();
                bool Base64Pwd = true;
                objLoginDTO = UserMngr.CheckUserDetails(txtUserName.Value.Trim(), Encryptionbase64Encode(txtPassword.Value), out Base64Pwd, true);
                if (objLoginDTO != null)// objLoginDTO.lstLookUp != null)
                {
                    login = objLoginDTO.User;
                    //if (objLoginDTO.User.Count > 0 && objLoginDTO.User.Any(x => x.Is_Direct_Login.Equals("Y", StringComparison.InvariantCultureIgnoreCase)))
                    if (objLoginDTO.User.Count > 0)
                    {
                        //ClientSession.UserName = login[0].user_name;
                        //ClientSession.EmailAddress = login[0].EMail_Address;
                        //ClientSession.UserAccountType = "Capella";
                        //Response.Redirect($"~/frmLandingScreen.aspx?UserAccountType=Capella&RequestedUserName={ClientSession.UserName}", false);
                        Server.Transfer($"~/frmLandingScreen.aspx?UserAccountType=Capella&RequestedUserName={login[0].user_name}");
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('010001');", true);
                    }
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('010001');", true);
                }
            }
        }

        private string GetOktaAuthorizationUrl(string email)
        {
            // Replace with your Okta domain and other necessary parameters
            var oktaDomain = ConfigurationSettings.AppSettings["okta:OktaDomain"];
            string oktaAuthorizeEndpoint = $"{ConfigurationSettings.AppSettings["okta:AuthorizeURL"]}";
            string clientId = ConfigurationSettings.AppSettings["okta:ClientId"];
            string redirectUri = ConfigurationSettings.AppSettings["okta:RedirectUri"];
            if (Request.Url.Authority == (ConfigurationManager.AppSettings["RootURL"] ?? ""))
            {
                redirectUri = $"https://{ConfigurationManager.AppSettings["RootURL"]}/frmLandingScreen.aspx";
            }
            return $"{oktaAuthorizeEndpoint}?client_id={clientId}&response_type=code&redirect_uri={HttpUtility.UrlEncode(redirectUri)}&scope=openid+profile+email&state={HttpUtility.UrlEncode(Guid.NewGuid().ToString())}&login_hint={HttpUtility.UrlEncode(email)}";
        }

        private string CheckOktaAuthorizationUrl()
        {
            // Replace with your Okta domain and other necessary parameters
            var oktaDomain = ConfigurationSettings.AppSettings["okta:OktaDomain"];
            string oktaAuthorizeEndpoint = $"{ConfigurationSettings.AppSettings["okta:AuthorizeURL"]}";
            string clientId = ConfigurationSettings.AppSettings["okta:ClientId"];
            string redirectUri = ConfigurationSettings.AppSettings["okta:RedirectUri"];
            if (Request.Url.Authority == (ConfigurationManager.AppSettings["RootURL"] ?? ""))
            {
                redirectUri = $"https://{ConfigurationManager.AppSettings["RootURL"]}/frmLandingScreen.aspx";
            }
            return $"{oktaAuthorizeEndpoint}?client_id={clientId}&response_type=code&redirect_uri={HttpUtility.UrlEncode(redirectUri)}&prompt=none&scope=openid+profile+email&state={HttpUtility.UrlEncode(Guid.NewGuid().ToString())}";
        }

        private string GetOktaUrl(string sessionToken)
        {
            var oktaDomain = ConfigurationSettings.AppSettings["okta:OktaDomain"];
            string oktaAuthorizeEndpoint = $"{ConfigurationSettings.AppSettings["okta:AuthorizeURL"]}";
            string clientId = ConfigurationSettings.AppSettings["okta:ClientId"];
            string redirectUri = ConfigurationSettings.AppSettings["okta:RedirectUri"];
            if (Request.Url.Authority == (ConfigurationManager.AppSettings["RootURL"] ?? ""))
            {
                redirectUri = $"https://{ConfigurationManager.AppSettings["RootURL"]}/frmLandingScreen.aspx";
            }
                
            //CAP-2142
            return $"{oktaAuthorizeEndpoint}?client_id={clientId}&response_type=code&scope=openid+profile+email&response_mode=query&prompt=none&redirect_uri={HttpUtility.UrlEncode(redirectUri)}&state={HttpUtility.UrlEncode(Guid.NewGuid().ToString())}&nonce=n-0S6_WzA2Mj&sessionToken={sessionToken}";
        }

        //To encrypt the password
        public static string Encryptionbase64Encode(string sData)
        {
            try
            {
                byte[] encData_byte = new byte[sData.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                //CAP-1942
                throw new Exception("Error in base64Encode" + ex.Message,ex);
            }
        }

    }
}