using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Acurus.Capella.Core.DomainObjects;
using System.Diagnostics;
using System.Net;
using System.IO;
using Acurus.Capella.Core.DTO;
using System.Collections;
using System.Runtime.Serialization;
using System.Data;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Text;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using DocumentFormat.OpenXml.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Telerik.Web.UI.Chat;
using Telerik.Web.UI.Skins;
using Acurus.Capella.UI.Extensions;
using User = Acurus.Capella.Core.DomainObjects.User;
using System.Collections.Specialized;

namespace Acurus.Capella.UI
{
    public partial class frmSelectLegalOrg : System.Web.UI.Page
    {
        UserManager UserMngr = new UserManager();
        DirectURLUtility directURLUtility = new DirectURLUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CAP-2007
                string isChangeLegalOrg = ConfigurationManager.AppSettings["IsChangeLegalOrg"];
                hdnIsLegaLOrgValidation.Value = isChangeLegalOrg;

                UserManager userManager = new UserManager();
                IList<User> ilstUsers = new List<User>();

                if (isChangeLegalOrg.Equals("Y", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(ClientSession.EmailAddress))
                {
                    ilstUsers = userManager.GetUserByEmailAddress(ClientSession.EmailAddress);
                }

                if (ilstUsers != null && ilstUsers.Count > 0)
                {
                    foreach (var item in ilstUsers)
                    {
                        System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem();
                        listItem.Value = item.user_name;
                        listItem.Text = item.Legal_Org;
                        cboLegalOrg.Items.Add(listItem);
                    }
                }
                else
                {
                    //CAP-2007
                    if (!isChangeLegalOrg.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                    {
                        rowLegalOrg.Visible = false;
                        titleLabel.Text = "Select Facility";
                        spnNotes.InnerText = "* Please ensure you have saved your work before proceeding to a different Facility.";
                    }
                    System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem();
                    listItem.Value = ClientSession.UserName;
                    listItem.Text = ClientSession.LegalOrg;
                    cboLegalOrg.Items.Add(listItem);
                }
                cboLegalOrg.SelectedValue = ClientSession.UserName;
                FillFacilityComboBox(ClientSession.LegalOrg);
            }
            if (Request.Form["EHRUserName"] != null) //Load Balancer - Automatic Single Sign On
            {
                ClientSession.SavedSession = "DELETED";
                UtilityManager.inserttologgingtableforSessionTimeout("Login Page Load - Before Calling LandingintoEHR - Input is" + Request.Form["EHRUserName"], Request.Url.ToString(), string.Empty);

                LandingintoEHR(Request.Form["EHRUserName"], Request.Form["EHRFacilityName"], Request.Form["EHRhdnLocalTime"], Request.Form["EHRhdnLocalDate"], Request.Form["EHRhdnUniversaloffset"], Request.Form["EHRhdnLocalDateAndTime"], Request.Form["EHRhdnFollowsDayLightSavings"], Request.Form["UserRole"], Request.Form["RCopiaUserName"], Request.Form["EMailAddress"], Request.Form["Is_RCopia_Notification_Required"], Request.Form["PhysicianId"], Request.Form["Landing_Screen_ID"], hdnGroupId.Value, Request.Form["PersonName"], Request.Form["LegalOrg"], Request.Form["UserCarrier"], Request.Form["IsFirstTimeCall"], Request.Form["DefaultServer"], Request.Form["IsAllFacilities"]);

                UtilityManager.inserttologgingtableforSessionTimeout("Login Page Load - After Calling LandingintoEHR - Input is", Request.Url.ToString(), string.Empty);

            }
        }
        private void FillFacilityComboBox(string legal_org)
        {
            cboFacilityName.Items.Clear();

            XmlDocument xmldoc = new XmlDocument();

            string strXmlFilePath = System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\Facility_Library.xml");
            if (File.Exists(strXmlFilePath) == true)
            {
                xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "Facility_Library" + ".xml");
                XmlNodeList xmlFacilityList = xmldoc.GetElementsByTagName("Facility");

                if (xmlFacilityList.Count > 0)
                {
                    foreach (XmlNode item in xmlFacilityList)
                    {
                        if (item != null && item.Attributes.GetNamedItem("Legal_Org").Value == legal_org)
                            cboFacilityName.Items.Add(item.Attributes[0].Value);
                    }

                    if (!string.IsNullOrWhiteSpace(ClientSession.EmailAddress))
                    {
                        var userDetails = UserMngr.GetUserDetailsByEmailAddressAndLegalOrg(ClientSession.EmailAddress, legal_org);
                        cboFacilityName.Value = (userDetails?.Count??0) > 0 ? userDetails[0]?.Default_Facility?? ClientSession.FacilityName : ClientSession.FacilityName;
                    }
                    else
                    {
                        cboFacilityName.Value = ClientSession.FacilityName;
                    }
                }
            }
        }
        //CAP-2007
        protected void cboLegalOrg_Change(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cboLegalOrg.SelectedItem.Text))
            {
                FillFacilityComboBox(cboLegalOrg.SelectedItem.Text);
                ClientScript.RegisterStartupScript(this.GetType(), "SelectLegalOrg", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "SelectLegalOrg", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart(); alert('Please select valid legal org.');}", true);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            string isChangeLegalOrg = ConfigurationManager.AppSettings["IsChangeLegalOrg"];
            //CAP-2007
            if (isChangeLegalOrg.Equals("Y", StringComparison.InvariantCultureIgnoreCase) && cboLegalOrg.SelectedItem.Text != ClientSession.LegalOrg)
            {
                if (!string.IsNullOrEmpty(cboLegalOrg.SelectedItem.Text) && !string.IsNullOrEmpty(cboFacilityName.Value))
                {
                    //ClientSession.FacilityName = cboFacilityName.Value;
                    //ClientSession.LegalOrg = cboLegalOrg.Text;
                    //hdnEMailAddress.Value = ClientSession.EmailAddress;
                    hdnChangedFacilityName.Value = cboFacilityName.Value;
                    hdnChangedLegalOrg.Value = cboLegalOrg.SelectedItem.Text;


                    IList<string> lstUser = UtilityManager.FindUserSessionFiles(cboLegalOrg.SelectedValue, string.Empty);
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
                        hdbbtnLogOutAndLogIn_ServerClick(sender, e);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(cboLegalOrg.SelectedItem.Text))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "SelectLegalOrg", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart(); DisplayErrorMessage('010026');}", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "SelectLegalOrg", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart(); DisplayErrorMessage('010005');}", true);
                    }
                }
            }
            else
            {
                ClientSession.FacilityName = cboFacilityName.Value;
                //CAP-1911
                Response.SetCookie(new HttpCookie("CFacilityName") { Value = ClientSession.FacilityName.ToString(), HttpOnly = false });
                ClientScript.RegisterStartupScript(this.GetType(), "SelectLegalOrg", "{RadWindowClose();changeReload();}", true);
            }
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "SelectLegalOrg", "RadWindowClose();", true);
        }
        //CAP-2007
        protected void hdbbtnLogOutAndLogIn_ServerClick(object sender, EventArgs e)
        {
            LogOut();
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "document.getElementById('hdnMultiUserLogin').click();", true);
        }
        public void LogOut()
        {
            ////Response.Write("<script> window.top.location.href=\" frmLogin.aspx\"; </script>");

            string sUser = ClientSession.UserName;
            UserSessionManager userSessionMngr = new UserSessionManager();

            UserSession objUserSession = new UserSession();
            objUserSession.User_Name = sUser;
            // userSessionMngr.DeleteUserSessionUsingUserSessionIED(sUser);
            //if (sUser != "" && sUser != string.Empty)
            //{
            //if (Global.ht[ClientSession.UserName]!=null)
            //Global.ht.Remove(ClientSession.UserName);
            //userSessionMngr.InsertUpdateDeleteUserSessionXml(objUserSession, "", "DELETE");
            //}
            //else
            //{
            // userSessionMngr.DeleteUserSessionFromXml(Session.SessionID);
            //}

            UtilityManager.DeleteUserSessionFile(string.Empty, Session.SessionID);
            ClientSession.SavedSession = "DELETED";
            ClientSession.WindowList = new ArrayList();
            if (Session != null && Session.SessionID != "")
            {
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

                if (Directory.Exists(Server.MapPath("atala-capture-download\\" + Session.SessionID)))
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
                    catch { }
                }
                if (Directory.Exists(Server.MapPath("atala-capture-upload\\" + Session.SessionID)))
                {
                    try
                    {
                        System.IO.Directory.Delete(Server.MapPath("atala-capture-upload\\" + Session.SessionID), true);
                    }
                    catch { }
                }
            }
            try
            {

                HttpContext.Current.Application.Remove("user");
                Session["ShowAllState"] = null;
                Session["GeneralQShowAll"] = null;
                ////CAP-1167
                Session.Clear();
                Session.Abandon();
                //CAP-1311
                ClientSession.FlushSession();
                foreach (string cookieName in Request.Cookies.AllKeys)
                {
                    HttpCookie myCookie = Request.Cookies[cookieName];
                    myCookie.Value = ""; // Clear the cookie's value
                    myCookie.Secure = true;
                    myCookie.Expires = DateTime.Now.AddYears(-1);// Expire the cookies
                    Response.Cookies.Add(myCookie); // Update the client-side cookie
                }
                //CAP-2166
                Response.SetCookie(new HttpCookie("IsOktaUser") { Value = "Y", Expires = DateTime.Now.AddMinutes(5) });
            }
            catch
            { }
            //HttpContext.Current.Session.Abandon();
        }
        protected void hdnbtnLogin_ServerClick(object sender, EventArgs e)
        {
            if (ClientSession.UserName == string.Empty)
                return;


            UserSessionManager userSessionMngr = new UserSessionManager();
            // IList<UserSession> lstUser = userSessionMngr.GetCurrentSessionByUserName(ClientSession.UserName);
            //IList<UserSession> lstUser = userSessionMngr.GetUserSessionFromXml(ClientSession.UserName);
            ////IList<UserSession> lstUser = (List<UserSession>)Session["UserSessionList"];
            //if (lstUser.Count > 0)
            //{
            //    UserSession objUserSession = new UserSession();
            //    objUserSession.User_Name = ClientSession.UserName;
            //    objUserSession.Current_Session_ID = HttpContext.Current.Session.SessionID;
            //    objUserSession.Last_Logged_Time = DateTime.UtcNow;
            //    objUserSession.Version = lstUser[0].Version;
            //    // userSessionMngr.UpdateUserSession(lstUser[0], ClientSession.UserName, string.Empty);
            //    userSessionMngr.InsertUpdateDeleteUserSessionXml(objUserSession, string.Empty, "UPDATE");
            //    ClientSession.SavedSession = "TRUE";
            //}
            String User = ClientSession.UserName;
            //Object lstUserID = Global.ht[ClientSession.UserName];
            IList<string> lstUser = UtilityManager.FindUserSessionFiles(User, string.Empty);
            if (lstUser.Count > 0)
            {
                //if (Convert.ToString(lstUserID).Equals(HttpContext.Current.Session.SessionID) == false)
                //{
                var objIsActiveSession = (from item in lstUser where item.Contains(Session.SessionID) select item).ToList<string>();
                //if (Convert.ToString(lstUserID).Equals(HttpContext.Current.Session.SessionID) == false )//&& Convert.ToString(ht["IsLogin-" + sUser]) == "TRUE")
                if (objIsActiveSession.Count == 0)
                {
                    //Global.ht[ClientSession.UserName] = HttpContext.Current.Session.SessionID;
                    //UtilityManager.inserttologgingtableforSessionTimeout("hdnbtnLogin_Click - before calling DeleteUserSessionFile", Request.Url.ToString(), string.Empty);

                    //UtilityManager.DeleteUserSessionFile(ClientSession.UserName, string.Empty);

                    //UtilityManager.CreateUserSessionFile(ClientSession.UserName, Session.SessionID);
                    //ImpersonateUser
                    if (Session["Default_Server"] != null && (Session["Default_Server"].ToString().ToUpper().Contains("FRMLOGIN.ASPX") == true || Session["Default_Server"].ToString().ToUpper().Contains("FRMSELECTLEGALORG.ASPX") == true || Session["Default_Server"].ToString().ToUpper().Contains("FRMLOGINNEW.ASPX") == true))
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
                    //if (Global.ht.ContainsKey("IsLogin-" + ClientSession.UserName))
                    //{
                    //    Global.ht["IsLogin-" + ClientSession.UserName] = "TRUE";
                    //}
                    //else
                    //{
                    //    Global.ht.Add("IsLogin-" + ClientSession.UserName, "TRUE"); 
                    //}
                }

            }

            if (ClientSession.UserPermissionDTO != null)
            {
                //Load Balancer - Redirect to a Default Server for the user
                //ImpersonateUser
                if (Session["Default_Server"] != null && (Session["Default_Server"].ToString().ToUpper().Contains("FRMLOGIN.ASPX") == true || Session["Default_Server"].ToString().ToUpper().Contains("FRMSELECTLEGALORG.ASPX") == true || Session["Default_Server"].ToString().ToUpper().Contains("FRMLOGINNEW.ASPX") == true))
                {
                    NameValueCollection data = new NameValueCollection();
                    data.Add("UserName", ClientSession.UserName);
                    data.Add("EHRUserName", ClientSession.UserName);
                    data.Add("EHRFacilityName", ClientSession.FacilityName);
                    data.Add("EHRhdnLocalTime", hdnLocalTime.Value);
                    data.Add("EHRhdnLocalDate", hdnLocalDate.Value);
                    data.Add("EHRhdnUniversaloffset", hdnUniversaloffset.Value);
                    data.Add("EHRhdnLocalDateAndTime", hdnLocalDateAndTime.Value);
                    data.Add("EHRhdnFollowsDayLightSavings", hdnFollowsDayLightSavings.Value);
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

                    //CAP-1167
                    var serverRedirectUrl = directURLUtility.GetServerRedirectURLByDirectURL(Request.QueryString["redirecturl"]?.ToString(), Session["Default_Server"].ToString());
                    HttpHelper.RedirectAndPOST(this.Page, serverRedirectUrl, data);
                    return;
                }

                IList<ScnTab> ScnTabList = ClientSession.UserPermissionDTO.Scntab;
                if (Session["LandingScnID"] != null)
                {
                    var ScnTabRecord = from s in ScnTabList where s.SCN_ID == (int)Session["LandingScnID"] select s;
                    //divLoading.Style.Add("display", "none");
                    //CAP-1167
                    var returnURL = Request.QueryString["redirecturl"]?.ToString();

                    if (!string.IsNullOrEmpty(returnURL))
                    {
                        //Response.Redirect(returnURL);
                    }
                    else
                    {
                        //Response.Redirect(ScnTabRecord.ToList<ScnTab>()[0].SCN_Name + ".aspx");

                    }
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "AfterOkClick();", true);
                }

            }
        }


        protected void hdnMultiUserLogin_Click(object sender, EventArgs e)
        {
            ClientSession.UserName = cboLegalOrg.SelectedValue;
            ClientSession.LegalOrg = hdnChangedLegalOrg.Value;
            ClientSession.FacilityName = hdnChangedFacilityName.Value;            
            UtilityManager.inserttologgingtableforSessionTimeout("btnLogin_Click - Start", Request.Url.ToString(), string.Empty);


            //To check if the login is success
            IList<User> login = new List<User>();
            LoginDTO objLoginDTO = new LoginDTO();


            if (ApplicationObject.scntab != null)
            {
                objLoginDTO = UserMngr.CheckUserDetailsLegalOrg(ClientSession.UserName.ToUpper(), false);
                if (objLoginDTO.UserPermissionDTO != null)
                    objLoginDTO.UserPermissionDTO.Scntab = ApplicationObject.scntab;
            }
            else
                objLoginDTO = UserMngr.CheckUserDetailsLegalOrg(ClientSession.UserName.ToUpper(), true);

            //To check if the cache data to be loaded
            //IList<LastModifiedLocalLookup> lstLookUp = new List<LastModifiedLocalLookup>();
            if (objLoginDTO != null)// objLoginDTO.lstLookUp != null)
            {
                login = objLoginDTO.User;
                ClientSession.EmailAddress = login[0].EMail_Address;
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




            //Set the client session variables
            if (login != null && login.Count > 0)
            {
                if (login[0].status == "A")
                {
                    if (login[0].Is_Down_Time == "Y")
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('010014');", true);

                        return;
                    }

                    //Load Balancer - Redirect to a Default Server for the user
                    //ImpersonateUser
                    login[0].Default_Server = "https://localhost:44351/frmLogin.aspx";
                    if (login[0].Default_Server != string.Empty && login[0].Default_Server.ToUpper().Contains("FRMLOGIN.ASPX") == true || login[0].Default_Server.ToUpper().Contains("FRMLOGINNEW.ASPX"))
                    {
                        //ImpersonateUser - To change the Default Server Login page to the current page
                        if (login[0].Default_Server.Contains("frmLogin.aspx") == true)
                        {
                            login[0].Default_Server = login[0].Default_Server.Replace("frmLogin.aspx", "frmSelectLegalOrg.aspx");
                        }
                        else
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('10113404');", true);
                            return;
                        }
                        Session["Default_Server"] = login[0].Default_Server;

                        NameValueCollection data = new NameValueCollection();
                        data.Add("UserName", ClientSession.UserName.ToUpper());
                        data.Add("EHRUserName", ClientSession.UserName.ToUpper());
                        data.Add("EHRFacilityName", ClientSession.FacilityName);
                        data.Add("EHRhdnLocalTime", hdnLocalTime.Value);
                        data.Add("EHRhdnLocalDate", hdnLocalDate.Value);
                        data.Add("EHRhdnUniversaloffset", hdnUniversaloffset.Value);
                        data.Add("EHRhdnLocalDateAndTime", hdnLocalDateAndTime.Value);
                        data.Add("EHRhdnFollowsDayLightSavings", hdnFollowsDayLightSavings.Value);
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
                        var serverRedirectUrl = directURLUtility.GetServerRedirectURLByDirectURL(Request.QueryString["redirecturl"]?.ToString(), login[0].Default_Server);

                        //For Bug ID :74036 
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Login", "sessionStorage.setItem('StartLoading', 'true');StartLoadFromPatChart();", true);


                        HttpHelper.RedirectAndPOST(this.Page, serverRedirectUrl, data);

                        //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Login", "sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();", true);
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Login", "sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();AfterOkClick();", true);
                        return;
                    }
                    else if (login[0].Default_Server == string.Empty && objLoginDTO.DefaultServerCount > 0)
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('010024');", true);
                        //txtUserName.Focus();
                        return;
                    }
                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MsgLogin", "AlertUser();", true);
                    //return;
                    ClientSession.UserRole = login[0].role;
                    // ClientSession.PersonName = login[0].person_name;
                    ClientSession.RCopiaUserName = login[0].RCopia_User_Name;
                    //Added by Srividhya on 21-Nov-2015
                    //if (hdnFacltyName.Value.Trim() != string.Empty)
                    ClientSession.FacilityName = login[0].Default_Facility;
                    //else if (ddlFacility.Value != "" && ddlFacility.Value != "0")
                    //    ClientSession.FacilityName = ddlFacility.Value;
                    //    ClientSession.FacilityName = ddlFacility.Value;
                    //Commented By Nijanthan(17-11-15)
                    //IList<FacilityLibrary> ilstFacilityLibrary = new List<FacilityLibrary>();//(List<FacilityLibrary>)ClientSession.FacilityLst.Facility_Library_List;
                    //ilstFacilityLibrary = (from obj in ilstFacilityLibrary where obj.Fac_Name == ClientSession.FacilityName select obj).ToList<FacilityLibrary>();
                    //if (ilstFacilityLibrary.Count > 0)
                    //{
                    //    ClientSession.POSDescription = ilstFacilityLibrary[0].POS + "|" + ilstFacilityLibrary[0].POS_Description;
                    //}
                    //else
                    //{
                    //    ClientSession.POSDescription = string.Empty;
                    //}

                    //ClientSession.FacilityLst = null;
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

                    //HttpCookie cookieLocal = new HttpCookie("CurrPhyId", login[0].Physician_Library_ID.ToString());
                    //cookieLocal.HttpOnly=false;
                    Response.SetCookie(new HttpCookie("CurrPhyId") { Value = login[0].Physician_Library_ID.ToString(), HttpOnly = false });
                    Response.SetCookie(new HttpCookie("UserRoll") { Value = login[0].role.ToString(), HttpOnly = false });
                    ScnTabManager objScnTabmngr = new ScnTabManager();
                    //ClientSession.UserPermissionDTO = objScnTabmngr.GetUserPermisssions(ClientSession.UserName, ClientSession.UserRole);
                    ClientSession.UserPermissionDTO = objLoginDTO.UserPermissionDTO;
                    //ClientSession.ListProcEncounter = null;
                    //ClientSession.EncryptedPassword = UIManager.Encryptionbase64Encode(txtPassword.Value);//BugID:54512

                    //if (ClientSession.UserPermissionDTO.ListProc.Count > 0)
                    //{
                    //    ArrayList listProc = new ArrayList();

                    //    for (int proc = 0; proc < ClientSession.UserPermissionDTO.ListProc.Count; proc++)
                    //    {
                    //        listProc.Add(ClientSession.UserPermissionDTO.ListProc[proc].Process_Name);
                    //    }

                    //   // ClientSession.ListProcEncounter = listProc;
                    //}
                    IList<ScnTab> ScnTabList;
                    ScnTabList = ClientSession.UserPermissionDTO.Scntab;


                    var ScnTabRecord = from s in ScnTabList where s.SCN_ID == login[0].Landing_Screen_ID select s;


                    ClientSession.LocalOffSetTime = hdnLocalTime.Value;
                    //Session["LocalDate"] = hdnLocalDate.Value;
                    ClientSession.LocalDate = hdnLocalDate.Value;
                    ClientSession.UniversalTime = hdnUniversaloffset.Value;
                    //Session["LocalDateAndTime"] = hdnLocalDateAndTime.Value;
                    ClientSession.LocalTime = hdnLocalDateAndTime.Value;
                    if (hdnFollowsDayLightSavings.Value.ToLower() == "true")
                        ClientSession.bFollows_DST = true;
                    else
                        ClientSession.bFollows_DST = false;

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
                    //UserSessionManager userSessionMngr = new UserSessionManager();
                    //IList<UserSession> lstUser = objLoginDTO.UserSession;

                    //if (lstUser.Count > 0)
                    //{
                    //    Session["UserSessionList"] = lstUser;
                    //    Page.Visible = true;
                    //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MsgLogin", "AlertUser();", true);
                    //    return;
                    //}
                    String User = ClientSession.UserName;
                    //Object lstUserID = Global.ht[ClientSession.UserName];
                    IList<string> lstUser = UtilityManager.FindUserSessionFiles(User, string.Empty);

                    if (lstUser.Count > 0)
                    {
                        //var objIsActiveSession = (from item in lstUser where item.Contains(Session.SessionID) select item).ToList<string>();
                        ////if (Convert.ToString(lstUserID).Equals(HttpContext.Current.Session.SessionID) == false )
                        //if (objIsActiveSession.Count == 0 && Request["OpenPatChart"] == null)//Changed for CarePointe
                        //{
                        //    //Global.ht[ClientSession.UserName] = HttpContext.Current.Session.SessionID;
                        //    //ClientSession.SavedSession = "TRUE";
                        //    Page.Visible = true;
                        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MsgLogin", "AlertUser();", true);
                        //    return;
                        //}
                    }
                    else
                    {
                        //UserSession objUserSession = new UserSession();
                        //objUserSession.User_Name = ClientSession.UserName;
                        //objUserSession.Current_Session_ID = HttpContext.Current.Session.SessionID;
                        //objUserSession.Last_Logged_Time = DateTime.UtcNow;
                        //objUserSession.MacAddress = string.Empty;
                        // userSessionMngr.InsertUserSession(objUserSession, string.Empty);
                        //Global.ht.Add(ClientSession.UserName, HttpContext.Current.Session.SessionID);
                        //  userSessionMngr.InsertUpdateDeleteUserSessionXml(objUserSession, string.Empty, "INSERT");

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
                        var returnURL = Request.QueryString["redirecturl"]?.ToString();
                        if (!string.IsNullOrEmpty(returnURL))
                        {
                            //Server.Transfer(returnURL);
                        }
                        else
                        {
                            //Server.Transfer("frmPatientChart.aspx");
                        }
                    }
                    else
                    {
                        //CAP-1167
                        var returnURL = Request.QueryString["redirecturl"]?.ToString();
                        if (!string.IsNullOrEmpty(returnURL))
                        {
                            //Server.Transfer(returnURL);
                        }
                        else
                        {
                            //Server.Transfer(ScnTabRecord.ToList<ScnTab>()[0].SCN_Name + ".aspx");
                        }

                    }
                    //Response.Redirect("frmMyQueue.aspx");
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('010003');", true);
                    //txtPassword.Value = string.Empty;
                    //txtUserName.Value = string.Empty;
                    //txtUserName.Focus();
                    return;
                }
            }
            //else if (!Base64Pwd)//BugID:53715
            //{
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('010023');", true);
            //    //txtUserName.Focus();
            //    //txtPassword.Value = string.Empty;
            //    //txtUserName.Value = string.Empty;
            //    return;
            //}
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('010001');", true);
                //txtUserName.Focus();
                //txtPassword.Value = string.Empty;
                //txtUserName.Value = string.Empty;
                return;
            }
            // divLoading.Style.Add("display", "none");

            Page.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MsgLogin", "AfterOkClick();", true);
            return;
        }

        public string LandingintoEHR(string sUserName, string sFacilityName, string shdnLocalTime, string shdnLocalDate, string shdnUniversaloffset, string shdnLocalDateAndTime, string shdnFollowsDayLightSavings, string sUserRole, string sRCopiaUserName, string sEMailAddress, string sIs_RCopia_Notification_Required, string sPhysicianId, string sLanding_Screen_ID, string shdnGroupId, string sPersonName, string sLegalOrg, string sUserCarrier, string sIsFirstTimeCall, string sDefaultServer, string sIsAllFacilities)
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

            hdnFacltyName.Value = sFacilityName;
            hdnLocalTime.Value = shdnLocalTime;
            hdnLocalDate.Value = shdnLocalDate;
            hdnUniversaloffset.Value = shdnUniversaloffset;
            hdnLocalDateAndTime.Value = shdnLocalDateAndTime;
            hdnFollowsDayLightSavings.Value = shdnFollowsDayLightSavings;
            hdnroleLanding.Value = sUserRole;
            hdnRCopia_User_NameLanding.Value = sRCopiaUserName;
            hdnEMailAddress.Value = sEMailAddress;
            hdnIs_RCopia_Notification_RequiredLanding.Value = sIs_RCopia_Notification_Required;
            hdnPhysician_Library_IDLanding.Value = sPhysicianId;
            hdnLanding_Screen_IDLanding.Value = sLanding_Screen_ID;
            hdnPersonName.Value = sPersonName;
            Session["Default_Server"] = sDefaultServer;
            Session["LandingScnID"] = sLanding_Screen_ID;

            UtilityManager.inserttologgingtableforSessionTimeout("LandingintoEHR - Before CreateUserSessionFile - Start - input is - " + sUserName, Request.Url.ToString(), string.Empty);

            //if (sIsFirstTimeCall == "true")
            //{
            //    IList<string> ilstUser = UtilityManager.FindUserSessionFiles(ClientSession.UserName, string.Empty);
            //    if (ilstUser.Count > 0)
            //    {
            //        var objIsActiveSession = (from item in ilstUser where item.Contains(HttpContext.Current.Session.SessionID) select item).ToList<string>();

            //        if (objIsActiveSession.Count == 0)//Changed for CarePointe
            //        {
            //            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('010021');", true);
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MsgLogin", "AlertUser();", true);
            //            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Login btnOk : End", DateTime.Now, shdnGroupId, "frmLogin");
            //            return string.Empty;
            //        }
            //    }
            //}

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
                    var returnURL = Request.QueryString["redirecturl"]?.ToString();
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
    }
}
