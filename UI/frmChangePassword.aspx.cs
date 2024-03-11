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
using Acurus.Capella.UI;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UI
{
    public partial class frmChangePassword : System.Web.UI.Page
    {
        UserManager userMngr = new UserManager();
        HumanManager humanMngr = new HumanManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CAP-1752
                hdnIsSSOLogin.Value = ConfigurationSettings.AppSettings["IsSSOLogin"] ?? "N";

                this.Title = "Change Password";
                pnlOldPassword.Visible = true;
                txtUserName.Text = ClientSession.UserName;
                if (Request["ScreenMode"] != null && Request["ScreenMode"] != "")
                    hdnScreenMode.Value = Request["ScreenMode"].ToString();
                if (Request["PatientID"] != null)
                    Session["PatientID"] = Request["PatientID"].ToString();
                if (hdnScreenMode.Value.ToUpper() == "CHANGE PASSWORD USER")
                {
                    txtOldPassword.Enabled = true;
                    lblUserName.Text = "User Name *";
                    txtUserName.Text = ClientSession.UserName;
                    txtUserName.BorderColor = System.Drawing.Color.Black;
                    txtUserName.BorderWidth = Unit.Pixel(1);
                    txtOldPassword.Focus();
                }
                else if (hdnScreenMode.Value.ToUpper() == "FORGOT PASSWORD PATIENT" || hdnScreenMode.Value.ToUpper() == "FORGOT PASSWORD QUARANTOR")
                {
                    txtOldPassword.Enabled = false;
                    pnlOldPassword.Visible = false;//Added By Manimaran for Bugid-25982
                    lblNewPassword.Text = "Create Password";
                    lblUserName.Text = "Mail ID *";
                    txtUserName.Text = Request["EmailID"].ToString();
                    txtUserName.BorderColor = System.Drawing.Color.Black;
                    txtUserName.BorderWidth = Unit.Pixel(1);
                    if (Request["IsLoginOpen"] != null)
                        IsLoginOpen.Value = Request["IsLoginOpen"].ToString();
                }
                else if (hdnScreenMode.Value.ToUpper() == "FORGOT PASSWORD USER")
                {
                    pnlOldPassword.Visible = false;//Added By Manimaran for Bugid-25982
                    lblNewPassword.Text = "Select Password";
                    txtOldPassword.Enabled = false;
                    lblUserName.Text = "User Name *";
                    txtUserName.Text = Request["UserName"].ToString();
                    txtUserName.BorderColor = System.Drawing.Color.Black;
                    txtUserName.BorderWidth = Unit.Pixel(1);
                    if (Request["IsLoginOpen"] != null)
                        IsLoginOpen.Value = Request["IsLoginOpen"].ToString();
                }
                else if (hdnScreenMode.Value.ToUpper() == "CHANGE PASSWORD PATIENT" || hdnScreenMode.Value.ToUpper() == "CHANGE PASSWORD QUARANTOR")
                {
                    txtOldPassword.Enabled = true;
                    lblUserName.Text = "Mail ID *";
                    txtUserName.Text = Request["EmailID"].ToString();
                    txtUserName.BorderColor = System.Drawing.Color.Black;
                    txtUserName.BorderWidth = Unit.Pixel(1);
                }
                else if (hdnScreenMode.Value.ToUpper() == "PATIENT PORTAL")
                {
                    txtOldPassword.Enabled = true;
                    lblUserName.Text = "Mail ID *";
                    txtUserName.Text = Request["EmailID"].ToString();
                    txtUserName.BorderColor = System.Drawing.Color.Black;
                    txtUserName.BorderWidth = Unit.Pixel(1);
                }
                else if (hdnScreenMode.Value.ToUpper() == "USER REGISTRATION")
                {
                    pnlOldPassword.Visible = false;
                    lblNewPassword.Text = "Create Password";
                    txtOldPassword.Enabled = false;
                    lblUserName.Text = "User Name *";
                    txtUserName.Text = Request["UserName"].ToString();
                    txtUserName.BorderColor = System.Drawing.Color.Black;
                    txtUserName.BorderWidth = Unit.Pixel(1);
                    if (Request["IsLoginOpen"] != null)
                        IsLoginOpen.Value = Request["IsLoginOpen"].ToString();
                }
                else if (hdnScreenMode.Value.ToUpper() == "USER REGISTRATION PATIENT PORTAL")
                {
                    pnlOldPassword.Visible = false;
                    lblNewPassword.Text = "Create Password";
                    txtOldPassword.Enabled = false;
                    lblUserName.Text = "User Name *";
                    txtUserName.Text = Request["EmailID"].ToString();
                    txtUserName.BorderColor = System.Drawing.Color.Black;
                    txtUserName.BorderWidth = Unit.Pixel(1);
                    if (Request["IsLoginOpen"] != null)
                        IsLoginOpen.Value = Request["IsLoginOpen"].ToString();
                } 
                else
                {
                    SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                    objSecurity.ApplyUserPermissions(this.Page);
                    txtUserName.Text = ClientSession.UserName;
                    btnClose.Visible = false;
                }
            }
            if (IsPostBack)
                hdnToFindPostback.Value = "true";
        }
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
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            if (hdnScreenMode.Value.ToUpper() == "CHANGE PASSWORD USER" || hdnScreenMode.Value.ToUpper() == "CHANGE PASSWORD PATIENT" || hdnScreenMode.Value.ToUpper() == "CHANGE PASSWORD QUARANTOR" || hdnScreenMode.Value.ToUpper() == "PATIENT PORTAL" && hdnScreenMode.Value.ToUpper() != "")
            {
                if (hdnOld.Value == string.Empty || txtOldPassword.Enabled == false)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('050003');loadchangepassword();", true);
                    txtOldPassword.Focus();
                    return;
                }
            }
            if (hdnScreenMode.Value.ToUpper() == "CHANGE PASSWORD PATIENT" || hdnScreenMode.Value.ToUpper() == "FORGOT PASSWORD PATIENT" || hdnScreenMode.Value.ToUpper() == "PATIENT PORTAL" || hdnScreenMode.Value.ToUpper() == "CHANGE PASSWORD QUARANTOR" || hdnScreenMode.Value.ToUpper() == "FORGOT PASSWORD QUARANTOR" || hdnScreenMode.Value.ToUpper() == "USER REGISTRATION PATIENT PORTAL")
            {
                if (Request["PatientID"] != null)
                {                   
                    Human objHuman = humanMngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(Request["PatientID"]))[0];
                    if (txtOldPassword.Text == string.Empty)
                    {
                        if (txtNewPassword.Text != "false" && hdnNew.Value == string.Empty)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780010');loadchangepassword();", true);
                            txtNewPassword.Focus();
                            return;
                        }
                        string sNewPwd = txtNewPassword.Text;
                        string expression = @"^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{0,12}$";
                        Match match = Regex.Match(sNewPwd, expression, RegexOptions.IgnoreCase);
                        if (!match.Success)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('010019');loadchangepassword();", true);
                            txtNewPassword.Focus();
                            return;
                        }
                        if (hdnConfirm.Value == string.Empty)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780011');loadchangepassword();", true);
                            txtConfirmPassword.Focus();
                            return;
                        }
                        if (hdnNew.Value != hdnConfirm.Value)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780012');loadchangepassword();", true);
                            txtConfirmPassword.Focus();
                            return;
                        }
                        if (hdnScreenMode.Value.ToUpper() == "CHANGE PASSWORD QUARANTOR" || hdnScreenMode.Value.ToUpper() == "FORGOT PASSWORD QUARANTOR")                       
                            objHuman.Representative_Password = hdnConfirm.Value;                       
                        else                        
                            objHuman.Password = hdnConfirm.Value;                        
                        IList<Human> HumanList = new List<Human>();
                        HumanList.Add(objHuman);
                        humanMngr.UpdateHumanEmailDetails(HumanList, string.Empty);
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}OpenLogin('" + objHuman.Id + "','" + Request["EmailID"].ToString() + "');loadchangepassword();", true);
                    }
                    else
                    {
                        if (hdnScreenMode.Value.ToUpper() == "CHANGE PASSWORD QUARANTOR")
                        {
                            if (objHuman.Representative_Email == txtUserName.Text && objHuman.Representative_Password == hdnOld.Value)
                            {
                                if (txtNewPassword.Text != "false" && hdnNew.Value == string.Empty)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780010');loadchangepassword();", true);
                                    txtNewPassword.Focus();
                                    return;
                                }
                                string sNewPwd = hdnNew.Value;
                                string expression = @"^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{0,12}$";
                                Match match = Regex.Match(sNewPwd, expression, RegexOptions.IgnoreCase);
                                if (!match.Success)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('010019');loadchangepassword();", true);
                                    txtNewPassword.Focus();
                                    return;
                                }
                                if (hdnConfirm.Value == string.Empty)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780011');loadchangepassword();", true);
                                    txtConfirmPassword.Focus();
                                    return;
                                }
                                if (hdnNew.Value != hdnConfirm.Value)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780012');loadchangepassword();", true);
                                    txtConfirmPassword.Focus();
                                    return;
                                }
                                objHuman.Representative_Password = hdnConfirm.Value;
                                IList<Human> HumanList = new List<Human>();
                                HumanList.Add(objHuman);
                                humanMngr.UpdateHumanEmailDetails(HumanList, string.Empty);
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}OpenLogin('" + objHuman.Id + "','" + Request["EmailID"].ToString() + "');loadchangepassword();", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('010017');loadchangepassword();", true);
                                txtOldPassword.Focus();
                                return;
                            }
                        }
                        else
                        {
                            if (objHuman.EMail == txtUserName.Text && objHuman.Password == hdnOld.Value)
                            {
                                if (txtNewPassword.Text != "false" && hdnNew.Value == string.Empty)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780010');loadchangepassword();", true);
                                    txtNewPassword.Focus();
                                    return;
                                }
                                string sNewPwd = hdnNew.Value;
                                string expression = @"^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{0,12}$";
                                Match match = Regex.Match(sNewPwd, expression, RegexOptions.IgnoreCase);
                                if (!match.Success)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('010019');loadchangepassword();", true);
                                    txtNewPassword.Focus();
                                    return;
                                }
                                if (hdnConfirm.Value == string.Empty)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780011');loadchangepassword();", true);
                                    txtConfirmPassword.Focus();
                                    return;
                                }
                                if (hdnConfirm.Value != hdnNew.Value)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780012');loadchangepassword();", true);
                                    txtConfirmPassword.Focus();
                                    return;
                                }
                                objHuman.Password = hdnConfirm.Value;
                                IList<Human> HumanList = new List<Human>();
                                HumanList.Add(objHuman);
                                humanMngr.UpdateHumanEmailDetails(HumanList, string.Empty);
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}OpenLogin('" + objHuman.Id + "','" + Request["EmailID"].ToString() + "');loadchangepassword();", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('010017');loadchangepassword();", true);
                                txtOldPassword.Focus();
                                return;
                            }
                        }
                    }
                }

            }
            else if (hdnScreenMode.Value.ToUpper() == "FORGOT PASSWORD USER" || hdnScreenMode.Value.ToUpper() == "CHANGE PASSWORD USER" || hdnScreenMode.Value.ToUpper() == "USER REGISTRATION")
            {
               // UserManager UserMngr = new UserManager(); //code comment by balaji.Tj 2015-12-10
                IList<User> UserList;
                UserList = userMngr.GetUser(txtUserName.Text);
                if (UserList!=null && UserList.Count > 0) //code modified by balaji.Tj 2015-12-10
                {
                    if (txtOldPassword.Text == string.Empty || txtOldPassword.ReadOnly == true)
                    {
                        if (txtNewPassword.Text != "false" && hdnNew.Value == string.Empty)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780010');loadchangepassword();", true);
                            txtNewPassword.Focus();
                            return;
                        }
                        string sNewPwd = hdnNew.Value;
                        string expression = @"^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{0,12}$";
                        Match match = Regex.Match(sNewPwd, expression, RegexOptions.IgnoreCase);
                        if (!match.Success)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('010019');loadchangepassword();", true);
                            txtNewPassword.Focus();
                            return;
                        }
                        if (hdnOld.Value == hdnNew.Value)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780013');loadchangepassword();", true);
                            //txtNewPassword.Text = "";
                            txtNewPassword.Focus();
                            return;
                        }
                        if (hdnConfirm.Value == string.Empty)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780011');loadchangepassword();", true);
                            txtConfirmPassword.Focus();
                            return;
                        }
                        if (hdnConfirm.Value != hdnNew.Value)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780012');loadchangepassword();", true);
                            txtConfirmPassword.Focus();
                            return;
                        }
                        
                        UserList[0].password = hdnConfirm.Value;
                        userMngr.UpdatePassword(UserList, string.Empty);
                        string HumanId = string.Empty;                        
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenLogin('" + string.Empty + "','" + string.Empty + "');", true);
                    }
                    else
                    {
                        txtOldPassword.Text = Encryptionbase64Encode(hdnOld.Value);
                        //if (ClientSession.EncryptedPassword == txtOldPassword.Text && UserList[0].user_name == txtUserName.Text)
                        UserManager userManger = new UserManager();
                        if (UserList[0].user_name == txtUserName.Text && userManger.CheckIfValidPwd(txtUserName.Text, txtOldPassword.Text))//BugID:54512
                        {
                            if (txtNewPassword.Text != "false" && hdnNew.Value == string.Empty)
                            {
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780010');loadchangepassword();", true);
                                txtNewPassword.Focus();
                                return;
                            }
                            string sNewPwd = hdnNew.Value;
                            string expression = @"^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{0,12}$";
                            Match match = Regex.Match(sNewPwd, expression, RegexOptions.IgnoreCase);
                            if (!match.Success)
                            {
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('010019');loadchangepassword();", true);
                                txtNewPassword.Focus();
                                return;
                            }
                            if (hdnOld.Value == hdnNew.Value)
                            {
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780013');loadchangepassword();", true);
                                //txtNewPassword.Text = "";
                                txtNewPassword.Focus();                                
                                return;
                            }
                            if (hdnConfirm.Value == string.Empty)
                            {
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780011');loadchangepassword();", true);
                                txtConfirmPassword.Focus();
                                return;
                            }
                            if (hdnNew.Value != hdnConfirm.Value)
                            {
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('780012');loadchangepassword();", true);
                                txtConfirmPassword.Focus();
                                return;
                            }
                            UserList[0].password = hdnConfirm.Value;
                            userMngr.UpdatePassword(UserList, string.Empty);
                            if (hdnScreenMode.Value.ToUpper() == "CHANGE PASSWORD USER")
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('010011');loadchangepassword();window.close()", true);
                            else
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}ShowErrorMessageList('010011');loadchangepassword();window.close()", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ToRetainValues(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('010017');loadchangepassword();", true);
                            txtOldPassword.Focus();
                            return;
                        }
                    }
                }              
            }
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (Session["PatientID"] != null)
            {
                string Human_ID = Session["PatientID"].ToString();
                string EmailID = Request["EmailID"].ToString();
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ChangePwdClose('" + Human_ID + "','" + EmailID + "');loadchangepassword();", true);                
            }
            else
            {
                if (hdnScreenMode.Value.ToUpper() == "FORGOT PASSWORD USER" || hdnScreenMode.Value.ToUpper() == "USER REGISTRATION" || hdnScreenMode.Value.ToUpper()== "FORGOT PASSWORD QUARANTOR")
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "CloseUserCPwd();", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "window.close();loadchangepassword();", true);              
            }
        }                   
    }
}
