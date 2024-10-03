using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.UI.Extensions;
using Acurus.Capella.UI.OktaResponseModel;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static QRCoder.PayloadGenerator.ShadowSocksConfig;

namespace Acurus.Capella.UI
{
    public partial class frmLandingScreen : System.Web.UI.Page
    {
        UserManager UserMngr = new UserManager();
        DirectURLUtility directURLUtility = new DirectURLUtility();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            //Direct URL should be suspended
            string sUserName = string.Empty;
            string sUserAccountType = string.Empty;
            string sMSUserEmail = string.Empty;

            var code = Request.Params["code"];
            var stateParm = string.Empty;
            var state = string.Empty;
            //CAP-2041
            string localtime = null;
            string localdate = null;
            string universaloffset = null;
            string localDateAndTime = null;
            string bDayLightSavings = null;
            //CAP-2019
            try
            {
                //CAP-2041
                #region Retrieve Params From State
                stateParm = Encoding.UTF8.GetString(Convert.FromBase64String(Request.Params["state"] ?? ""));
                string[] parts = stateParm.Split('|');
                if (parts.Length > 1)
                {
                    state = parts[1];
                }

                if(parts.Length > 2)
                {
                    localtime = parts[2];
            }

                if (parts.Length > 3)
                {
                    localdate = parts[3];
                }

                if (parts.Length > 4)
                {
                    universaloffset = parts[4];
                }

                if (parts.Length > 5)
                {
                    localDateAndTime = parts[5];
                }

                if (parts.Length > 6)
                {
                    bDayLightSavings = parts[6];
                }
                #endregion
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error decoding Base64 string: {ex.Message}");
            }

            var isLoginRequired = !string.IsNullOrWhiteSpace(Request.Params["error"]) && (Request.Params["error"]??"") == "login_required";
            if (!IsPostBack)
            {
                if (isLoginRequired)
                {
                    //CAP-2166
                    Response.SetCookie(new HttpCookie("IsOktaUser") { Value = "Y", Expires = DateTime.Now.AddSeconds(30) });

                    var redirectUrl = "/frmLoginNew.aspx?IsLoginRequired=true";
                    //CAP-2458
                    var returnURL = string.Empty;
                    if (!string.IsNullOrWhiteSpace(state))
                    {
                        returnURL = HttpUtility.UrlDecode(state);
                    }
                    //CAP-2549
                    //else
                    //{
                    //    returnURL = HttpUtility.UrlDecode(Request.Cookies["RedirectUri"]?.Value);
                    //}
                    if (!string.IsNullOrWhiteSpace(returnURL))
                    {
                        redirectUrl += $"&redirecturl={HttpUtility.UrlEncode(returnURL)}";
                    }

                    Response.Redirect(redirectUrl);
                    return;
                }


                if (!string.IsNullOrEmpty(code))
                {
                    //CAP-2142
                    var userResponse = GenerateAccessToken(code);
                    sMSUserEmail = userResponse.Item1;
                    sUserAccountType = userResponse.Item2;
                }

                //Jira CAP-1893
                //string sUserAccountType = !string.IsNullOrWhiteSpace(ClientSession.UserAccountType) ? ClientSession.UserAccountType : (Request.Form["UserAccountType"] ?? string.Empty);
                if (string.IsNullOrWhiteSpace(sUserAccountType))
                {
                    if (Request.Form["UserAccountType"] != null && Request.Form["UserAccountType"] == "Microsoft")
                    {
                        sUserAccountType = "Microsoft";
                }
                else
                {
                        sUserAccountType = Request.Form["UserAccountType"] ?? Request.QueryString["UserAccountType"] ?? string.Empty;
                }
            }
            
                //Remove ClientSession.UserAccountType Dependency
                //sUserAccountType = !string.IsNullOrWhiteSpace(ClientSession.UserAccountType) ? ClientSession.UserAccountType : (Request.Form["UserAccountType"] ?? Request.QueryString["UserAccountType"] ?? string.Empty);

            //For Debug
            //string msg = $"ClientSession.UserAccountType={ClientSession.UserAccountType}$Request.Form[UserAccountType]={Request.Form["UserAccountType"]}$Request.QueryString[UserAccountType]={Request.QueryString["UserAccountType"]}$code={code}$sUserAccountType={sUserAccountType}$";
            //string CName = "checking" + DateTime.Now.ToString("hh-mm-ss");
            //Response.SetCookie(new HttpCookie(CName) { Value = msg.ToString(), HttpOnly = false });

            //var code = Request.Params["code"];
            //if (!string.IsNullOrEmpty(code))
            //{
            //    //ClientSession.UserAccountType = "Microsoft";
            //    GenerateAccessToken(code);
            //}

                if (Request.Form["AccessToken"] != null && Request.Form["AccessTokenId"] != null)
            {
                ClientSession.AccessToken = Request.Form["AccessToken"];
                ClientSession.AccessTokenId = Request.Form["AccessTokenId"];
                Response.SetCookie(new HttpCookie("MicrosoftAccessTokenId") { Value = Request.Form["AccessTokenId"] });
            }

                if (sUserAccountType == "Capella")
            {
                    sUserName = (Request.Form["UserName"] ?? Request.QueryString["RequestedUserName"] ?? string.Empty);
                    ClientSession.UserName = sUserName;
                }
                else
                {
                    sUserName = !string.IsNullOrWhiteSpace(sMSUserEmail) ? sMSUserEmail : (Request.Form["EMailAddress"] ?? Request.QueryString["EMailAddress"] ?? string.Empty);
                    ClientSession.EmailAddress = sUserName;
                }
            }
            else
            {
                sUserAccountType = ClientSession.UserAccountType;
                sUserName = ClientSession.UserAccountType == "Capella" ? ClientSession.UserName : ClientSession.EmailAddress;
            }

           
            #region Region - Login Page Load

            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                hdnVersion.Value = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"];
            //if (System.Configuration.ConfigurationSettings.AppSettings["ProjectName"] != null)
            //    hdnProjectName.Value = System.Configuration.ConfigurationSettings.AppSettings["ProjectName"];
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
            //if (System.Configuration.ConfigurationSettings.AppSettings["Reportpathhttp"] != null)
            //    hdnReportPathhttp.Value = System.Configuration.ConfigurationSettings.AppSettings["Reportpathhttp"];
            //CAP-2041
            ClientSession.LocalOffSetTime = Request.Form["EHRhdnLocalTime"] ?? localtime ?? Request.Cookies["LocalOffSetTime"]?.Value ??  "";
            ClientSession.LocalDate = Request.Form["EHRhdnLocalDate"] ?? localdate ?? Request.Cookies["LocalDate"]?.Value ?? "";
            ClientSession.UniversalTime = Request.Form["EHRhdnUniversaloffset"] ?? universaloffset ?? Request.Cookies["UniversalTime"]?.Value ?? "";
            ClientSession.LocalTime = Request.Form["EHRhdnLocalDateAndTime"] ?? localDateAndTime ?? Request.Cookies["LocalTime"]?.Value ?? "";
            bool.TryParse(Request.Form["EHRhdnFollowsDayLightSavings"] ?? bDayLightSavings ?? Request.Cookies["bFollows_DST"]?.Value, out bool bFollows_DST);
            ClientSession.bFollows_DST = bFollows_DST;

            if (!string.IsNullOrEmpty(Request.Form["EHRhdnLocalTime"]??localtime))
            {
                Response.SetCookie(new HttpCookie("LocalOffSetTime") { Value = Request.Form["EHRhdnLocalTime"]?? localtime, Expires = DateTime.Now.AddDays(1) });      
            }
            
            if (!string.IsNullOrEmpty(Request.Form["EHRhdnLocalDate"]??localdate))
            {
                Response.SetCookie(new HttpCookie("LocalDate") { Value = Request.Form["EHRhdnLocalDate"] ?? localdate, Expires = DateTime.Now.AddDays(1) });      
            }
            
            if (!string.IsNullOrEmpty(Request.Form["EHRhdnUniversaloffset"]??universaloffset))
            {
                Response.SetCookie(new HttpCookie("UniversalTime") { Value = Request.Form["EHRhdnUniversaloffset"] ?? universaloffset, Expires = DateTime.Now.AddDays(1) });      
            }
            
            if (!string.IsNullOrEmpty(Request.Form["EHRhdnLocalDateAndTime"]??localDateAndTime))
            {
                Response.SetCookie(new HttpCookie("LocalTime") { Value = Request.Form["EHRhdnLocalDateAndTime"] ?? localDateAndTime, Expires = DateTime.Now.AddDays(1) });      
            }
            
            if (!string.IsNullOrEmpty(Request.Form["EHRhdnFollowsDayLightSavings"]??bDayLightSavings))
            {
                Response.SetCookie(new HttpCookie("bFollows_DST") { Value = Request.Form["EHRhdnFollowsDayLightSavings"] ?? localDateAndTime, Expires = DateTime.Now.AddDays(1) });      
            }


            //CAP-1922 & CAP-1955,CAP-2171
            var responseRedirectUrl = string.Empty;
            if (!string.IsNullOrWhiteSpace(Request.Form["RedirectURL"]))
            {
                var returnUrl = Request.Form["RedirectURL"];
                responseRedirectUrl = Request.Form["RedirectURL"];
                var redirectURL = directURLUtility.GetDomainSpecificRedirectURL(returnUrl, Request.Form["DefaultServer"]);
                responseRedirectUrl = redirectURL;//Request.Form["RedirectURL"];
                Response.SetCookie(new HttpCookie("RedirectUri") { Value = redirectURL, Expires = DateTime.Now.AddDays(1) });
            }
            else
            {
                //CAP-2171
                if(!string.IsNullOrEmpty(state))
                {
                    Response.SetCookie(new HttpCookie("RedirectUri") { Value = state, Expires = DateTime.Now.AddDays(1) });
                    responseRedirectUrl = state;
                }
                else if (!string.IsNullOrWhiteSpace(Request.Url.Query))
                {
                    var returnUrl = HttpUtility.ParseQueryString(Request.Url.Query)["redirecturl"]; 
                    responseRedirectUrl = returnUrl;
                    Response.SetCookie(new HttpCookie("RedirectUri") { Value = returnUrl, Expires = DateTime.Now.AddDays(1) });
                }
                else
                {
                    ExpireRedirectUrlCookie();
                }
            }

            //CAP-2549
            //if (string.IsNullOrWhiteSpace(responseRedirectUrl))
            //{
            //    responseRedirectUrl = Request.Cookies["RedirectUri"]?.Value ?? string.Empty;
            //}

            if (string.IsNullOrWhiteSpace(sUserAccountType))
            {
                if (string.IsNullOrWhiteSpace(sUserName))
                {
                    //CAP-2389 & CAP-2379
                    var unauthorizedUserMessage = GenerateUnAuthorizedUserMessage(sUserName, sUserAccountType);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "invalidrequest", $"ScriptErrorLogEntry('User not permitted', '0', '0', 'frmLandingScreen.aspx', '{unauthorizedUserMessage.Item1}', 'false');DisplayErrorMessage('000009', '', '{string.Join("-", unauthorizedUserMessage.Item2)}');", true);
                }
                else
                {
                    Response.Redirect("/frmLoginNew.aspx");
                }
                return;
            }
            else
            {
                ClientSession.UserAccountType = sUserAccountType;
            }

            if (string.IsNullOrWhiteSpace(sUserName))
            {
                //CAP-2389 & CAP-2379
                var unauthorizedUserMessage = GenerateUnAuthorizedUserMessage(sUserName, sUserAccountType);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "invalidusername", $"ScriptErrorLogEntry('User not permitted', '0', '0', 'frmLandingScreen.aspx', '{unauthorizedUserMessage.Item1}', 'false');DisplayErrorMessage('000009', '', '{string.Join("-", unauthorizedUserMessage.Item2)}');", true);
                return;
            }

            if (Request.Form["EHRUserName"] != null) //Load Balancer - Automatic Single Sign On
            {
                ClientSession.SavedSession = "DELETED";
                UtilityManager.inserttologgingtableforSessionTimeout("Login Page Load - Before Calling LandingintoEHR - Input is" + Request.Form["EHRUserName"], Request.Url.ToString(), string.Empty);
                //CAP-2469
                LandingintoEHR(Request.Form["EHRUserName"], Request.Form["EHRFacilityName"], Request.Form["EHRhdnLocalTime"] ?? localtime, Request.Form["EHRhdnLocalDate"] ?? localdate, Request.Form["EHRhdnUniversaloffset"] ?? universaloffset, Request.Form["EHRhdnLocalDateAndTime"] ?? localDateAndTime, Request.Form["EHRhdnFollowsDayLightSavings"] ?? bDayLightSavings, Request.Form["UserRole"], Request.Form["RCopiaUserName"], Request.Form["EMailAddress"], Request.Form["Is_RCopia_Notification_Required"], Request.Form["PhysicianId"], Request.Form["Landing_Screen_ID"], hdnGroupId.Value, Request.Form["PersonName"], Request.Form["LegalOrg"], Request.Form["UserCarrier"], Request.Form["IsFirstTimeCall"], Request.Form["DefaultServer"], Request.Form["IsAllFacilities"], responseRedirectUrl);

                UtilityManager.inserttologgingtableforSessionTimeout("Login Page Load - After Calling LandingintoEHR - Input is", Request.Url.ToString(), string.Empty);

                return;

            }
            #endregion

            #region btnOK_Click

            DateTime dtStartTime = DateTime.Now;

            if (hdnGroupId != null && hdnGroupId.Value == "")
            {
                hdnGroupId.Value = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + dtStartTime.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            }

            UtilityManager.inserttologgingtableforSessionTimeout("btnLogin_Click - Start", Request.Url.ToString(), string.Empty);

            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Login btnOk : Start", DateTime.Now, hdnGroupId.Value, "frmLogin");

            //To check if the login is success
            IList<User> login = new List<User>();
            LoginDTO objLoginDTO = new LoginDTO();

            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Login CheckUserDetails DB call : Start", DateTime.Now, hdnGroupId.Value, "frmLogin");

            if (sUserAccountType == "Capella")
            {
                if (ApplicationObject.scntab != null)
                {
                    objLoginDTO = UserMngr.CheckUserDetailsByUsername(sUserName, false);
                    if (objLoginDTO.UserPermissionDTO != null)
                        objLoginDTO.UserPermissionDTO.Scntab = ApplicationObject.scntab;
                }
                else
                    objLoginDTO = UserMngr.CheckUserDetailsByUsername(sUserName, true);
            }
            else
            {
            if (ApplicationObject.scntab != null)
            {
                objLoginDTO = UserMngr.GetUserDetailsByOktaEmailAddress(sUserName, false);
                if (objLoginDTO.UserPermissionDTO != null)
                    objLoginDTO.UserPermissionDTO.Scntab = ApplicationObject.scntab;
            }
            else
                objLoginDTO = UserMngr.GetUserDetailsByOktaEmailAddress(sUserName, true);
            }

            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Login CheckUserDetails DB call : End", DateTime.Now, hdnGroupId.Value, "frmLogin");

            //To check if the cache data to be loaded
            //IList<LastModifiedLocalLookup> lstLookUp = new List<LastModifiedLocalLookup>();
            if (objLoginDTO != null && objLoginDTO.User.Count > 0)// objLoginDTO.lstLookUp != null)
            {
                login = objLoginDTO.User;
                ClientSession.UserName = login[0].user_name.Trim().ToUpper();
                //lstLookUp = objLoginDTO.lstLookUp;
            }
            //----Added By Nijanthan(17-11-15)
            //if(lstLookUp!=null)
            //{
            //    foreach (LastModifiedLocalLookup objLookup in lstLookUp)
            //    {
            //        sContent += objLookup.Field_Name + "|" + objLookup.Last_Modified_Date.ToString() + "%";
            //    }
            //    sContent = sContent.TrimEnd('%');
            //}


            if (login != null && login.Count > 0 && login[0].password.Trim() == "Password1!")
            {
                Response.Redirect("frmForgotPassword.aspx?UserName=" + login[0].user_name.Trim().ToUpper() + "&ScreenMode=UserRegister");
            }

            //Set the client session variables
            if (login != null && login.Count > 0)
            {
                if (login[0].status == "A" && (login[0].Is_Direct_Login.Equals("Y", StringComparison.InvariantCultureIgnoreCase) || ClientSession.UserAccountType == "Capella"))
                {
                    if (login[0].Is_Down_Time == "Y")
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('010014');", true);
                        UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Login btnOk : End", DateTime.Now, hdnGroupId.Value, "frmLogin");

                        return;
                    }

                    //Load Balancer - Redirect to a Default Server for the user
                    ////ImpersonateUser
                    if (login[0].Default_Server != string.Empty && login[0].Default_Server.ToUpper().Contains("FRMLOGIN.ASPX") == true || login[0].Default_Server.ToUpper().Contains("FRMLANDINGSCREEN.ASPX") == true)
                    {
                        //ImpersonateUser - To change the Default Server Login page to the current page
                        if (login[0].Default_Server.Contains("frmLogin.aspx") == true)
                        {
                            if ((Request?.Headers["X-Forwarded-Host"] ?? "") == ConfigurationSettings.AppSettings["AkidoChartDomain"])
                            {
                                string subdomain = Request.Url.Authority.Contains("test6") ? "" : "";

                                if (!string.IsNullOrWhiteSpace(subdomain))
                                {
                                    login[0].Default_Server = $"https://{ConfigurationSettings.AppSettings["AkidoChartDomain"]}/{subdomain}/frmLogin.aspx";
                                }
                                else
                                {
                                    login[0].Default_Server = $"https://{ConfigurationSettings.AppSettings["AkidoChartDomain"]}/frmLogin.aspx";
                                }
                            }

                            login[0].Default_Server = login[0].Default_Server.Replace("frmLogin.aspx", "frmLandingScreen.aspx");
                        }
                        else
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('10113404');", true);
                            return;
                        }

                        Session["Default_Server"] = login[0].Default_Server;

                        NameValueCollection data = new NameValueCollection();
                        data.Add("UserName", login[0].user_name.ToString().ToUpper());
                        data.Add("EHRUserName", login[0].user_name.ToString().ToUpper());
                        data.Add("EHRFacilityName", login[0].Default_Facility);
                        data.Add("EHRhdnLocalTime", ClientSession.LocalOffSetTime);
                        data.Add("EHRhdnLocalDate", ClientSession.LocalDate);
                        data.Add("EHRhdnUniversaloffset", ClientSession.UniversalTime);
                        data.Add("EHRhdnLocalDateAndTime", ClientSession.LocalTime);
                        data.Add("EHRhdnFollowsDayLightSavings", ClientSession.bFollows_DST.ToString());
                        data.Add("UserRole", login[0].role);
                        data.Add("RCopiaUserName", login[0].RCopia_User_Name);
                        data.Add("EMailAddress", login[0].EMail_Address);
                        data.Add("Is_RCopia_Notification_Required", login[0].Is_RCopia_Notification_Required);
                        data.Add("PhysicianId", login[0].Physician_Library_ID.ToString());
                        data.Add("Landing_Screen_ID", login[0].Landing_Screen_ID.ToString());
                        data.Add("PersonName", login[0].person_name);
                        data.Add("LegalOrg", login[0].Legal_Org);
                        data.Add("UserCarrier", objLoginDTO.UserCarrier);
                        data.Add("IsFirstTimeCall", "true");
                        data.Add("DefaultServer", login[0].Default_Server);
                        data.Add("IsAllFacilities", login[0].Is_All_Facilities);
                        data.Add("UserAccountType", sUserAccountType);
                        data.Add("AccessToken", ClientSession.AccessToken);
                        data.Add("AccessTokenId", ClientSession.AccessTokenId);
                        //CAP-2171
                        data.Add("RedirectURL", responseRedirectUrl ?? string.Empty);
                        hdnroleLanding.Value = login[0].role;
                        hdnRCopia_User_NameLanding.Value = login[0].RCopia_User_Name;
                        hdnIs_RCopia_Notification_RequiredLanding.Value = login[0].Is_RCopia_Notification_Required;
                        hdnPhysician_Library_IDLanding.Value = login[0].Physician_Library_ID.ToString();
                        hdnLanding_Screen_IDLanding.Value = login[0].Landing_Screen_ID.ToString();
                        hdnPersonName.Value = login[0].person_name.ToString();
                        hdnEMailAddress.Value = login[0].EMail_Address;
                        ClientSession.Is_RCopia_Notification_Required = login[0].Is_RCopia_Notification_Required;
                        ClientSession.PhysicianId = login[0].Physician_Library_ID;
                        ClientSession.CurrentPhysicianId = login[0].Physician_Library_ID;
                        ClientSession.LegalOrg = login[0].Legal_Org;
                        ClientSession.UserCarrier = objLoginDTO.UserCarrier.ToString();
                        UtilityManager.inserttologgingtableforSessionTimeout("hdnbtnLogin_Click - After setting the parameters for redirection - " + login[0].user_name, Request.Url.ToString(), string.Empty);
                        ClientSession.Is_All_Facilities = login[0].Is_All_Facilities;
                        //IList<string> ilstUser = UtilityManager.FindUserSessionFiles(ClientSession.UserName, string.Empty);
                        //if (ilstUser.Count > 0)
                        //{
                        //    var objIsActiveSession = (from item in ilstUser where item.Contains(HttpContext.Current.Session.SessionID) select item).ToList<string>();

                        //    if (objIsActiveSession.Count == 0)//Changed for CarePointe
                        //    {
                        //        //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('010021');", true);
                        //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MsgLogin", "AlertUser();", true);
                        //        UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Login btnOk : End", DateTime.Now, hdnGroupId.Value, "frmLogin");
                        //        return;
                        //    }
                        //}
                        //else
                        //{
                        //    //CreateUserSessionFile(UserName, Session.SessionID);
                        //    UtilityManager.CreateUserSessionFile(ClientSession.UserName, HttpContext.Current.Session.SessionID);
                        //}
                        //CAP-1167
                        //CAP-1922
                        //CAP-2469
                        var serverRedirectUrl = directURLUtility.GetServerRedirectURLByDirectURL(responseRedirectUrl??string.Empty, login[0].Default_Server);
                        ExpireRedirectUrlCookie();
                        //For Bug ID :74036 
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Login", "sessionStorage.setItem('StartLoading', 'true');StartLoadFromPatChart();", true);


                        HttpHelper.RedirectAndPOST(this.Page, serverRedirectUrl, data);

                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Login", "sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();", true);
                        UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Login btnOk : End", DateTime.Now, hdnGroupId.Value, "frmLogin");
                        return;
                    }
                    else if (login[0].Default_Server == string.Empty && objLoginDTO.DefaultServerCount > 0)
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('010024');", true);
                        return;
                    }
                    ClientSession.UserRole = login[0].role;
                    ClientSession.RCopiaUserName = login[0].RCopia_User_Name;
                    ClientSession.FacilityName = login[0].Default_Facility;
                    if (ClientSession.FacilityName != string.Empty)
                    {
                        Session["Facility"] = ClientSession.FacilityName;
                    }
                    Response.SetCookie(new HttpCookie("CUserRole") { Value = login[0].role.ToString(), HttpOnly = false });
                    Response.SetCookie(new HttpCookie("CUserName") { Value = login[0].user_name.ToString(), HttpOnly = false });
                    Response.SetCookie(new HttpCookie("CFacilityName") { Value = ClientSession.FacilityName.ToString(), HttpOnly = false });
                    Response.SetCookie(new HttpCookie("CPersonName") { Value = login[0].person_name.ToString(), HttpOnly = false });
                    Response.SetCookie(new HttpCookie("CEMailAddress") { Value = login[0].EMail_Address.ToString(), HttpOnly = false });
                    Response.SetCookie(new HttpCookie("CLegalOrg") { Value = login[0].Legal_Org.ToString(), HttpOnly = false });
                    Response.SetCookie(new HttpCookie("CUserCarrier") { Value = objLoginDTO.UserCarrier.ToString(), HttpOnly = false });

                    //ClientSession.DefaultNoofDays = login[0].Default_MyQ_Days;
                    ClientSession.Is_RCopia_Notification_Required = login[0].Is_RCopia_Notification_Required;
                    ClientSession.PhysicianId = login[0].Physician_Library_ID;
                    ClientSession.CurrentPhysicianId = login[0].Physician_Library_ID;
                    ClientSession.LegalOrg = login[0].Legal_Org;
                    ClientSession.UserCarrier = objLoginDTO.UserCarrier.ToString();
                    Session["LandingScnID"] = login[0].Landing_Screen_ID;
                    ClientSession.Is_All_Facilities = login[0].Is_All_Facilities;

                    Response.SetCookie(new HttpCookie("CurrPhyId") { Value = login[0].Physician_Library_ID.ToString(), HttpOnly = false });
                    Response.SetCookie(new HttpCookie("UserRoll") { Value = login[0].role.ToString(), HttpOnly = false });
                    ScnTabManager objScnTabmngr = new ScnTabManager();
                    ClientSession.UserPermissionDTO = objLoginDTO.UserPermissionDTO;
                    IList<ScnTab> ScnTabList;
                    ScnTabList = ClientSession.UserPermissionDTO.Scntab;


                    var ScnTabRecord = from s in ScnTabList where s.SCN_ID == login[0].Landing_Screen_ID select s;


                    //ClientSession.LocalOffSetTime = hdnLocalTime.Value;
                    //ClientSession.LocalDate = hdnLocalDate.Value;
                    //ClientSession.UniversalTime = hdnUniversaloffset.Value;
                    //ClientSession.LocalTime = hdnLocalDateAndTime.Value;
                    //if (hdnFollowsDayLightSavings.Value.ToLower() == "true")
                    //    ClientSession.bFollows_DST = true;
                    //else
                    //    ClientSession.bFollows_DST = false;

                    //if (txtPassword.Value == "Password1!" || DateTime.Now.Subtract(login[0].Password_Changed_Date).Days == Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ResetPasswordDaysLimit"]))
                    //{
                    //    //frmChangePassword frmChange = new frmChangePassword(true);
                    //    //frmChange.ShowDialog();
                    //}
                    Page.Visible = false;
                    if (ScnTabRecord != null && ScnTabRecord.ToList<ScnTab>().Count > 0)
                    {
                        if (ScnTabRecord.ToList<ScnTab>()[0].SCN_Name == "frmMyQueue")
                        {
                            ArrayList arrList = new ArrayList();
                            arrList.Add("My Q");
                            ClientSession.WindowList = arrList;
                        }
                        else if (ScnTabRecord.ToList<ScnTab>()[0].SCN_Name == "frmAPPOINTMENTS")
                        {
                            ArrayList arrList = new ArrayList();
                            arrList.Add("Appointments - Scheduler");
                            ClientSession.WindowList = arrList;
                        }
                    }
                    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Login btnOk : End", DateTime.Now, hdnGroupId.Value, "frmLogin");
                    String User = ClientSession.UserName;
                    IList<string> lstUser = UtilityManager.FindUserSessionFiles(User, string.Empty);

                    
                    if (lstUser.Count > 0)
                    {
                        var objIsActiveSession = (from item in lstUser where item.Contains(Session.SessionID) select item).ToList<string>();
                        if (objIsActiveSession.Count == 0 && Request["OpenPatChart"] == null)//Changed for CarePointe
                        {
                            Page.Visible = true;
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MsgLogin", "AlertUser();", true);
                            return;
                        }
                    }
                    else
                    {
                        UtilityManager.inserttologgingtableforSessionTimeout("btnOK_Click - Before calling CreateUserSessionFile - WithoutLoadBalancerr", Request.Url.ToString(), string.Empty);

                        UtilityManager.CreateUserSessionFile(ClientSession.UserName, Session.SessionID);
                        ClientSession.SavedSession = "TRUE";
                        string LoggedInFacility = string.Empty;
                        if (ClientSession.FacilityName.Trim() != string.Empty)
                            LoggedInFacility = ClientSession.FacilityName;
                        //else if (hdnFacltyName.Value.Trim() != string.Empty)
                        //    LoggedInFacility = hdnFacltyName.Value;
                        //else
                        //    LoggedInFacility = ddlFacility.Value;
                        UtilityManager.WriteApplicationAccessInfo(ClientSession.UserName, LoggedInFacility);//user access log
                    }
                    if (Request["OpenPatChart"] != null && Request["OpenPatChart"] == "true")//Changed for CarePointe
                    {
                        HttpContext.Current.Session["PatChartRedirectVlaues"] = Request["HumanID"].ToString() + "&" + Request["EncounterID"].ToString() + "&" + Request["hdnLocalTime"].ToString();
                        ClientSession.LocalOffSetTime = Request["hdnLocalTimeoffset"].ToString();
                        ClientSession.LocalDate = Request["hdnLocalDate"].ToString();
                        ClientSession.UniversalTime = Server.UrlEncode(Request["hdnUnvOffset"].ToString());
                        ClientSession.LocalTime = Request["hdnLocalDateTime"].ToString();
                        if (Request["hdnFDayLghtSavings"].ToString().ToLower() == "true")
                            ClientSession.bFollows_DST = true;
                        else
                            ClientSession.bFollows_DST = false;
                        //CAP-1167
                        //CAP-1922
                        //CAP-2469
                        var returnURL = responseRedirectUrl;
                        ExpireRedirectUrlCookie();
                        if (!string.IsNullOrEmpty(returnURL))
                        {
                            Server.Transfer(returnURL);
                        }
                        else
                        {
                            Server.Transfer("frmPatientChart.aspx");
                        }
                    }
                    else
                    {
                        //CAP-1167
                        //CAP-1922
                        //CAP-2469
                        var returnURL = responseRedirectUrl;
                        ExpireRedirectUrlCookie();
                        if (!string.IsNullOrEmpty(returnURL))
                        {
                            Response.Redirect(returnURL);
                        }
                        else
                        {
                            Response.Redirect(ScnTabRecord.ToList<ScnTab>()[0].SCN_Name + ".aspx");
                        }

                    }
                    //Response.Redirect("frmMyQueue.aspx");
                }
                else
                {
                    //CAP-2389 & CAP-2379
                    var unauthorizedUserMessage = GenerateUnAuthorizedUserMessage(sUserName, sUserAccountType);
                    // ScriptErrorLogEntry(evt.message, evt.lineno, evt.colno, evt.filename, evt?.error?.stack, true);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, $"ScriptErrorLogEntry('User not permitted', '0', '0', 'frmLandingScreen.aspx', '{unauthorizedUserMessage.Item1}', 'false');DisplayErrorMessage('000009', '', '{string.Join("-", unauthorizedUserMessage.Item2)}');", true);
                    return;
                }
            }
            else
            {
                //CAP-2389 & CAP-2379
                var unauthorizedUserMessage = GenerateUnAuthorizedUserMessage(sUserName, sUserAccountType);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, $"ScriptErrorLogEntry('User not permitted', '0', '0', 'frmLandingScreen.aspx', '{unauthorizedUserMessage.Item1}', 'false');DisplayErrorMessage('000009', '', '{string.Join("-", unauthorizedUserMessage.Item2)}');", true);
                //Response.Redirect("/frmLoginNew.aspx");
                //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('010001');", true);
                return;
            }
            #endregion
        }

        protected void hdnbtnLogin_Click(object sender, EventArgs e)
        {
            if (ClientSession.UserName == string.Empty)
                return;

            UtilityManager.inserttologgingtableforSessionTimeout("hdnbtnLogin_Click - Start", Request.Url.ToString(), string.Empty);



            UserSessionManager userSessionMngr = new UserSessionManager();
            String User = ClientSession.UserName;
            IList<string> lstUser = UtilityManager.FindUserSessionFiles(User, string.Empty);
            if (lstUser.Count > 0)
            {
                var objIsActiveSession = (from item in lstUser where item.Contains(Session.SessionID) select item).ToList<string>();
                if (objIsActiveSession.Count == 0)
                {
                    //ImpersonateUser
                    if (Session["Default_Server"] != null && (Session["Default_Server"].ToString().ToUpper().Contains("FRMLOGIN.ASPX") == true))
                    {
                        ClientSession.SavedSession = "DELETED";
                    }
                    else
                    {
                        UtilityManager.inserttologgingtableforSessionTimeout("hdnbtnLogin_Click - before calling DeleteUserSessionFile", Request.Url.ToString(), string.Empty);

                        UtilityManager.DeleteUserSessionFile(ClientSession.UserName, string.Empty);

                        UtilityManager.CreateUserSessionFile(ClientSession.UserName, Session.SessionID);

                        ClientSession.SavedSession = "TRUE";


                        string LoggedInFacility = string.Empty;
                        if (ClientSession.FacilityName.Trim() != string.Empty)
                            LoggedInFacility = ClientSession.FacilityName;
                        //else if (hdnFacltyName.Value.Trim() != string.Empty)
                        //    LoggedInFacility = hdnFacltyName.Value;
                        //else
                        //    LoggedInFacility = ddlFacility.Value;
                        UtilityManager.WriteApplicationAccessInfo(ClientSession.UserName, LoggedInFacility);//user access log
                    }
                }

            }

            if (ClientSession.UserPermissionDTO != null)
            {
                //Load Balancer - Redirect to a Default Server for the user
                //ImpersonateUser
                if (Session["Default_Server"] != null && (Session["Default_Server"].ToString().ToUpper().Contains("FRMLOGIN.ASPX") == true || Session["Default_Server"].ToString().ToUpper().Contains("FRMLANDINGSCREEN.ASPX") == true))
                {
                    NameValueCollection data = new NameValueCollection();
                    data.Add("UserName", ClientSession.UserName);
                    data.Add("EHRUserName", ClientSession.UserName);
                    data.Add("EHRFacilityName", ClientSession.FacilityName);
                    data.Add("EHRhdnLocalTime", ClientSession.LocalOffSetTime);
                    data.Add("EHRhdnLocalDate", ClientSession.LocalDate);
                    data.Add("EHRhdnUniversaloffset", ClientSession.UniversalTime);
                    data.Add("EHRhdnLocalDateAndTime", ClientSession.LocalTime);
                    data.Add("EHRhdnFollowsDayLightSavings", ClientSession.bFollows_DST.ToString());
                    data.Add("UserRole", hdnroleLanding.Value);
                    data.Add("RCopiaUserName", hdnRCopia_User_NameLanding.Value);
                    data.Add("EMailAddress", hdnEMailAddress.Value);
                    data.Add("Is_RCopia_Notification_Required", hdnIs_RCopia_Notification_RequiredLanding.Value);
                    data.Add("PhysicianId", hdnPhysician_Library_IDLanding.Value);
                    data.Add("Landing_Screen_ID", hdnLanding_Screen_IDLanding.Value);
                    data.Add("PersonName", hdnPersonName.Value);
                    data.Add("LegalOrg", ClientSession.LegalOrg);
                    data.Add("UserCarrier", ClientSession.UserCarrier.ToString());
                    data.Add("IsFirstTimeCall", "false");
                    data.Add("DefaultServer", Session["Default_Server"].ToString());
                    data.Add("IsAllFacilities", ClientSession.Is_All_Facilities.ToString());
                    data.Add("UserAccountType", ClientSession.UserAccountType);
                    data.Add("AccessToken", ClientSession.AccessToken);
                    data.Add("AccessTokenId", ClientSession.AccessTokenId);
                    data.Add("RedirectURL", Request.Cookies["RedirectUri"]?.Value ?? string.Empty);

                    //CAP-1167
                    //CAP-1922
                    var serverRedirectUrl = directURLUtility.GetServerRedirectURLByDirectURL(Request.Cookies["RedirectUri"]?.Value, Session["Default_Server"].ToString());
                    ExpireRedirectUrlCookie();
                    HttpHelper.RedirectAndPOST(this.Page, serverRedirectUrl, data);
                    return;
                }

                IList<ScnTab> ScnTabList = ClientSession.UserPermissionDTO.Scntab;
                if (Session["LandingScnID"] != null)
                {
                    var ScnTabRecord = from s in ScnTabList where s.SCN_ID == (int)Session["LandingScnID"] select s;
                    //CAP-1167
                    //CAP-1922
                    var returnURL = Request.Cookies["RedirectUri"]?.Value;
                    ExpireRedirectUrlCookie();
                    if (!string.IsNullOrEmpty(returnURL))
                    {
                        Response.Redirect(returnURL);
                    }
                    else
                    {
                        Response.Redirect(ScnTabRecord.ToList<ScnTab>()[0].SCN_Name + ".aspx");

                    }
                }
            }
        }

        public string LandingintoEHR(string sUserName, string sFacilityName, string shdnLocalTime, string shdnLocalDate, string shdnUniversaloffset, string shdnLocalDateAndTime, string shdnFollowsDayLightSavings, string sUserRole, string sRCopiaUserName, string sEMailAddress, string sIs_RCopia_Notification_Required, string sPhysicianId, string sLanding_Screen_ID, string shdnGroupId, string sPersonName, string sLegalOrg, string sUserCarrier, string sIsFirstTimeCall, string sDefaultServer, string sIsAllFacilities, string sRedirectURL)
        {
            UtilityManager.inserttologgingtableforSessionTimeout("LandingintoEHR API - Start - input is - " + sUserName, Request.Url.ToString(), string.Empty);

            ClientSession.UserName = sUserName.Trim().ToUpper();
            ClientSession.FacilityName = sFacilityName;
            ClientSession.LocalOffSetTime = shdnLocalTime;
            ClientSession.LocalDate = shdnLocalDate;
            ClientSession.UniversalTime = shdnUniversaloffset;
            ClientSession.LocalTime = shdnLocalDateAndTime;
            ClientSession.LegalOrg = sLegalOrg;
            ClientSession.UserCarrier = sUserCarrier;
            ClientSession.EmailAddress = sEMailAddress;
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "LandingintoEHR : Start", DateTime.Now, shdnGroupId, "frmLogin");

            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                hdnVersion.Value = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"];
            if (System.Configuration.ConfigurationSettings.AppSettings["ProjectName"] != null)
                hdnProjectName.Value = System.Configuration.ConfigurationSettings.AppSettings["ProjectName"];
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
            //if (System.Configuration.ConfigurationSettings.AppSettings["Reportpathhttp"] != null)
            //    hdnReportPathhttp.Value = System.Configuration.ConfigurationSettings.AppSettings["Reportpathhttp"];

            //hdnFacltyName.Value = sFacilityName;
            //hdnLocalTime.Value = shdnLocalTime;
            //hdnLocalDate.Value = shdnLocalDate;
            //hdnUniversaloffset.Value = shdnUniversaloffset;
            //hdnLocalDateAndTime.Value = shdnLocalDateAndTime;
            //hdnFollowsDayLightSavings.Value = shdnFollowsDayLightSavings;
            hdnroleLanding.Value = sUserRole;
            hdnRCopia_User_NameLanding.Value = sRCopiaUserName;
            hdnEMailAddress.Value = sEMailAddress;
            hdnIs_RCopia_Notification_RequiredLanding.Value = sIs_RCopia_Notification_Required;
            hdnPhysician_Library_IDLanding.Value = sPhysicianId;
            hdnLanding_Screen_IDLanding.Value = sLanding_Screen_ID;
            hdnPersonName.Value = sPersonName;
            Session["Default_Server"] = sDefaultServer;
            Session["LandingScnID"] = sLanding_Screen_ID;
            //CAP-2469
            var redirectURL = sRedirectURL;


            UtilityManager.inserttologgingtableforSessionTimeout("LandingintoEHR - Before CreateUserSessionFile - Start - input is - " + sUserName, Request.Url.ToString(), string.Empty);

            if (sIsFirstTimeCall == "true")
            {
                IList<string> ilstUser = UtilityManager.FindUserSessionFiles(ClientSession.UserName, string.Empty);
                if (ilstUser.Count > 0)
                {
                    var objIsActiveSession = (from item in ilstUser where item.Contains(HttpContext.Current.Session.SessionID) select item).ToList<string>();

                    if (objIsActiveSession.Count == 0)//Changed for CarePointe
                    {
                        //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('010021');", true);
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MsgLogin", "AlertUser();", true);
                        UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Login btnOk : End", DateTime.Now, shdnGroupId, "frmLogin");
                        return string.Empty;
                    }
                }
            }

            UtilityManager.DeleteUserSessionFile(ClientSession.UserName, string.Empty);

            UtilityManager.CreateUserSessionFile(ClientSession.UserName, HttpContext.Current.Session.SessionID);
            ClientSession.SavedSession = "TRUE";
            UtilityManager.WriteApplicationAccessInfo(ClientSession.UserName, sFacilityName);
            UserManager UserMngr = new UserManager();

            //To check if the login is success
            IList<User> login = new List<User>();
            LoginDTO objLoginDTO = new LoginDTO();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "LandingintoEHR DB call: Start", DateTime.Now, shdnGroupId, "frmLogin");
            if (ApplicationObject.scntab != null)
            {
                objLoginDTO = UserMngr.CheckUserDetailsWithoutPassword(sUserName, false);
                if (objLoginDTO.UserPermissionDTO != null)
                    objLoginDTO.UserPermissionDTO.Scntab = ApplicationObject.scntab;
            }
            else
                objLoginDTO = UserMngr.CheckUserDetailsWithoutPassword(sUserName, true);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "LandingintoEHR DB call: End", DateTime.Now, shdnGroupId, "frmLogin");
            //To check if the cache data to be loaded
            if (objLoginDTO != null)// objLoginDTO.lstLookUp != null)
            {
                //login = objLoginDTO.User;
                User objUser = new User();
                objUser.role = sUserRole;
                objUser.RCopia_User_Name = sRCopiaUserName;
                objUser.Is_RCopia_Notification_Required = sIs_RCopia_Notification_Required;
                if (sPhysicianId != null && sPhysicianId != "")
                    objUser.Physician_Library_ID = Convert.ToUInt64(sPhysicianId);
                if (sLanding_Screen_ID != null && sLanding_Screen_ID != "" && System.Text.RegularExpressions.Regex.IsMatch(sLanding_Screen_ID, "^[0-9]*$") == true)
                {
                    objUser.Landing_Screen_ID = Convert.ToInt32(sLanding_Screen_ID);
                }
                objUser.user_name = sUserName.ToUpper();
                objUser.EMail_Address = sEMailAddress;
                objUser.Legal_Org = sLegalOrg;
                objUser.Is_All_Facilities = sIsAllFacilities;
                login.Add(objUser);
            }

            //Set the client session variables
            if (login != null && login.Count > 0)
            {
                //if (login[0].status == "A")
                //{

                ClientSession.UserRole = login[0].role;
                ClientSession.RCopiaUserName = login[0].RCopia_User_Name;
                ClientSession.Is_RCopia_Notification_Required = login[0].Is_RCopia_Notification_Required;
                ClientSession.PhysicianId = login[0].Physician_Library_ID;
                ClientSession.CurrentPhysicianId = login[0].Physician_Library_ID;
                ClientSession.LegalOrg = login[0].Legal_Org;
                ClientSession.UserCarrier = sUserCarrier;
                ClientSession.Is_All_Facilities = login[0].Is_All_Facilities;

                ScnTabManager objScnTabmngr = new ScnTabManager();
                ClientSession.UserPermissionDTO = objLoginDTO.UserPermissionDTO;

                Response.SetCookie(new HttpCookie("CUserRole") { Value = login[0].role.ToString(), HttpOnly = false });
                Response.SetCookie(new HttpCookie("CUserName") { Value = login[0].user_name.ToString(), HttpOnly = false });
                Response.SetCookie(new HttpCookie("CFacilityName") { Value = ClientSession.FacilityName.ToString(), HttpOnly = false });
                Response.SetCookie(new HttpCookie("CurrPhyId") { Value = login[0].Physician_Library_ID.ToString(), HttpOnly = false });
                Response.SetCookie(new HttpCookie("UserRoll") { Value = login[0].role.ToString(), HttpOnly = false });
                Response.SetCookie(new HttpCookie("CPersonName") { Value = sPersonName, HttpOnly = false });
                Response.SetCookie(new HttpCookie("CEMailAddress") { Value = login[0].EMail_Address.ToString(), HttpOnly = false });
                Response.SetCookie(new HttpCookie("CLegalOrg") { Value = login[0].Legal_Org.ToString(), HttpOnly = false });
                Response.SetCookie(new HttpCookie("CUserCarrier") { Value = ClientSession.UserCarrier.ToString(), HttpOnly = false });

                Session["LandingScnID"] = login[0].Landing_Screen_ID;

                IList<ScnTab> ScnTabList;
                ScnTabList = ClientSession.UserPermissionDTO.Scntab;


                var ScnTabRecord = from s in ScnTabList where s.SCN_ID == login[0].Landing_Screen_ID select s;


                if (shdnFollowsDayLightSavings.ToLower() == "true")
                    ClientSession.bFollows_DST = true;
                else
                    ClientSession.bFollows_DST = false;

                if (ScnTabRecord != null && ScnTabRecord.ToList<ScnTab>().Count > 0)
                {
                    if (ScnTabRecord.ToList<ScnTab>()[0].SCN_Name == "frmMyQueue")
                    {
                        ArrayList arrList = new ArrayList();
                        arrList.Add("My Q");
                        ClientSession.WindowList = arrList;
                    }
                    else if (ScnTabRecord.ToList<ScnTab>()[0].SCN_Name == "frmAPPOINTMENTS")
                    {
                        ArrayList arrList = new ArrayList();
                        arrList.Add("Appointments - Scheduler");
                        ClientSession.WindowList = arrList;
                    }


                    //Response.Redirect(ScnTabRecord.ToList<ScnTab>()[0].SCN_Name + ".aspx");
                    //CAP-1167
                    string sFileName = ScnTabRecord.ToList<ScnTab>()[0].SCN_Name + ".aspx";
                    //CAP-1922
                    //CAP-2308,CAP-2469
                    var returnURL = string.Empty;
                    if (!string.IsNullOrWhiteSpace(Request.Cookies["RedirectUri"]?.Value))
                    {
                    if (!string.IsNullOrWhiteSpace(redirectURL))
                    {
                            returnURL = HttpUtility.UrlDecode(redirectURL);
                    }
                    else
                    {
                        returnURL = HttpUtility.UrlDecode(Request.Cookies["RedirectUri"]?.Value);
                    }
                    }
                    ExpireRedirectUrlCookie();

                    if (!string.IsNullOrEmpty(returnURL))
                    {
                        sFileName = returnURL;
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Redit", "EHRLanding('" + sFileName + "');", true);
                }

                //if (Request["OpenPatChart"] != null && Request["OpenPatChart"] == "true")//Changed for CarePointe
                //{
                //    HttpContext.Current.Session["PatChartRedirectVlaues"] = Request["HumanID"].ToString() + "&" + Request["EncounterID"].ToString() + "&" + Request["hdnLocalTime"].ToString();
                //    ClientSession.LocalOffSetTime = Request["hdnLocalTimeoffset"].ToString();
                //    ClientSession.LocalDate = Request["hdnLocalDate"].ToString();
                //    ClientSession.UniversalTime = Server.UrlEncode(Request["hdnUnvOffset"].ToString());
                //    ClientSession.LocalTime = Request["hdnLocalDateTime"].ToString();
                //    if (Request["hdnFDayLghtSavings"].ToString().ToLower() == "true")
                //        UIManager.bFollows_DST = true;
                //    else
                //        UIManager.bFollows_DST = false;
                //    Server.Transfer("frmPatientChart.aspx");
                //}
                //else
                //{
                //    Response.Redirect(ScnTabRecord.ToList<ScnTab>()[0].SCN_Name + ".aspx");
                //}
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "LandingintoEHR : End", DateTime.Now, shdnGroupId, "frmLogin");
                //}
            }
            else
            {
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "LandingintoEHR : End", DateTime.Now, shdnGroupId, "frmLogin");
            }
            return string.Empty;
        }
        //CAP-2142
        protected Tuple<string, string> GenerateAccessToken(string code)
        {
            string redirectUri = string.Empty;
            var clientId = ConfigurationSettings.AppSettings["okta:ClientId"];
            var clientSecret = ConfigurationSettings.AppSettings["okta:ClientSecret"];
            var base64EncodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

            var options = new RestClientOptions(ConfigurationSettings.AppSettings["okta:OktaDomain"])
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"{ConfigurationSettings.AppSettings["okta:TokenURL"]}", RestSharp.Method.Post);
            request.AddHeader("accept", "application/json");
            request.AddHeader("authorization", $"Basic {base64EncodedString}");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "authorization_code");
            //CAP-2337
            if ((Request?.Headers["X-Forwarded-Host"] ?? "") == ConfigurationSettings.AppSettings["AkidoChartDomain"])
            {
                string subdomain = Request.Url.Authority.Contains("test6") ? "" : "";

                if (string.IsNullOrWhiteSpace(subdomain))
                {
                    redirectUri = $"https://{ConfigurationSettings.AppSettings["AkidoChartDomain"]}/frmLandingScreen.aspx";
                }
                else
                {
                    redirectUri = $"https://{ConfigurationSettings.AppSettings["AkidoChartDomain"]}/{subdomain}/frmLandingScreen.aspx";
                }
            }
            else
            {
                redirectUri = ConfigurationSettings.AppSettings["okta:RedirectUri"];
            }
            request.AddParameter("redirect_uri", redirectUri);
            request.AddParameter("code", $"{code}");
            RestResponse response = client.ExecuteAsync(request).Result;

            TokenResponse myDeserializedClass = JsonConvert.DeserializeObject<TokenResponse>(response.Content);

            //Get User Information
            var useroptions = new RestClientOptions(ConfigurationSettings.AppSettings["okta:OktaDomain"])
            {
                MaxTimeout = -1,
            };
            var userclient = new RestClient(useroptions);
            var userrequest = new RestRequest($"{ConfigurationSettings.AppSettings["okta:UserInfoURL"]}", RestSharp.Method.Get);
            userrequest.AddHeader("Authorization", $"Bearer {myDeserializedClass.access_token}");
            RestResponse userresponse = userclient.ExecuteAsync(userrequest).Result;

            UserInfo userInfoResponse = JsonConvert.DeserializeObject<UserInfo>(userresponse.Content);

            //CAP-2142
            var userAccountType = GetOktaIDPType(userInfoResponse?.email ?? "");

            //ClientSession.EmailAddress =  userInfoResponse?.email??"";
            ClientSession.AccessToken = myDeserializedClass?.access_token ?? "";
            ClientSession.AccessTokenId = myDeserializedClass?.id_token ?? "";

            //CAP-2142
            Response.SetCookie(new HttpCookie("MicrosoftAccessTokenId") { Value = ClientSession.AccessTokenId });

            //CAP-242
            return new Tuple<string,string>(userInfoResponse?.email ?? "", userAccountType);
        }

        public void ExpireRedirectUrlCookie()
        {
            if (Request.Cookies["RedirectUri"] != null && !string.IsNullOrEmpty(Request.Cookies["RedirectUri"].Value))
            {
                HttpCookie cookie = Request.Cookies["RedirectUri"];
                cookie.Expires = DateTime.Now.AddMinutes(-5);
                Response.Cookies.Add(cookie);
            }
        }
        //CAP-2142
        public string GetOktaIDPType(string emailaddress)
        {
            try
            {
                if (!string.IsNullOrEmpty(emailaddress))
                {
                    var oktaDomain = ConfigurationSettings.AppSettings["okta:OktaDomain"];
                    var client = new RestClient(oktaDomain);
                    var request = new RestRequest(ConfigurationSettings.AppSettings["okta:WebFinger"], RestSharp.Method.Get);
                    request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
                    request.AddParameter("resource", $"acct:{emailaddress}");
                    request.AddParameter("rel", "okta:idp");
                    RestResponse response = client.ExecuteAsync(request).Result;
                    OktaUserIDPModel result = JsonConvert.DeserializeObject<OktaUserIDPModel>(response.Content);

                    if ((result?.links?.Count ?? 0) > 0)
                    {
                        if ((result.links[0]?.titles?.und ?? string.Empty).Equals("Azure IDP", StringComparison.InvariantCultureIgnoreCase))
                        {
                            return "Microsoft";
                        }
                        else
                        {
                            return "Okta";
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return string.Empty;
        }

        //CAP-2389 & CAP-2379
        private Tuple<string, List<string>> GenerateUnAuthorizedUserMessage(string sEmailAddress, string sUserAccountType)
        {
            List<string> listParams = new List<string>() { (sEmailAddress ?? "").Replace("-", "*"), (sUserAccountType ?? "").Replace("-", "*"), (Request.Url.AbsoluteUri ?? "").Replace("-", "*") };

            var alertMessage = $"You are not a permitted user. Please contact the System Administrator. UserName: {sEmailAddress}, UserAccountType: {sUserAccountType}, Requested URL: {Request.Url.AbsoluteUri}";

            return new Tuple<string, List<string>>(alertMessage, listParams);
        }

        public class TokenResponse
        {
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string access_token { get; set; }
            public string scope { get; set; }
            public string id_token { get; set; }
        }

        public class UserInfo
        {
            public string sub { get; set; }
            public string name { get; set; }
            public string locale { get; set; }
            public string email { get; set; }
            public string preferred_username { get; set; }
            public string given_name { get; set; }
            public string family_name { get; set; }
            public string zoneinfo { get; set; }
            public int updated_at { get; set; }
            public bool email_verified { get; set; }
        }
    }
}