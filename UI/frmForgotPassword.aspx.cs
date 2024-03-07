using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Acurus.Capella.Core.DomainObjects;
using System.Net;
using System.IO;
using Acurus.Capella.Core.DTO;
using System.Collections;
using System.Runtime.Serialization;
using System.Data;
using System.Drawing;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Text;
using Acurus.Capella.UI;
using Telerik.Web.UI;
using Acurus.Capella.UI.UserControls;
using System.Configuration;

namespace Acurus.Capella.UI
{
    public partial class frmForgotPassword : System.Web.UI.Page
    {
        IList<Human> HumanList = new List<Human>();
        HumanManager objHumenmgr = new HumanManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CAP-1752
                hdnIsSSOLogin.Value = ConfigurationSettings.AppSettings["IsSSOLogin"] ?? "N";
                ClientSession.FlushSession();
                IList<StaticLookup> QuestionList;
                StaticLookupManager LookUpMngr = new StaticLookupManager();
                hdnHumanID.Value = null;
                QuestionList = LookUpMngr.getStaticLookupByFieldName("SECURITY QUESTIONS");
                if (QuestionList != null && QuestionList.Count > 0)
                {
                    cboSecurityQuestion1.Items.Add(new RadComboBoxItem(""));
                    cboSecurityQuestion2.Items.Add(new RadComboBoxItem(""));
                    for (int i = 0; i < QuestionList.Count; i++)
                    {
                        cboSecurityQuestion1.Items.Add(new RadComboBoxItem(QuestionList[i].Value));
                        cboSecurityQuestion2.Items.Add(new RadComboBoxItem(QuestionList[i].Value));
                    }
                }
                if (Request["PatientID"] != null)
                {
                    hdnHumanID.Value = Request["PatientID"];
                }
                if(Request["EmailID"]!=null)
                {
                    hdnEmailID.Value = Request["EmailID"].ToString();
                }
                if (Request["ScreenMode"] != null && Request["ScreenMode"]!="")
                {
                    hdnScreenMode.Value=Request["ScreenMode"].ToString ();
                }
                if (hdnScreenMode.Value == "Login")
                {
                    Panel1.GroupingText = "Forgot Password";
                    this.Title = "Forgot Password";
                    btnSave.Value = "OK";
                    btnClearAll.Value = "Cancel";
                    txtUserName.ReadOnly = true;
                    txtUserName.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                    txtUserName.Text = (string)Request["UserName"];
                    MessageWindow.Title = "Forgot Password";

                    UserManager userMngr = new UserManager();
                    IList<User> objUser = new List<User>();
                    objUser = userMngr.GetUser(txtUserName.Text);
                    Session["Question1"]=cboSecurityQuestion1.Text;
                    if (objUser != null)
                    {
                        cboSecurityQuestion1.SelectedIndex = cboSecurityQuestion1.Items.FindItemByText(objUser[0].Security_Question1).Index;
                        Session["Answer1"] = objUser[0].Answer1;
                        cboSecurityQuestion1.Enabled = false;
                        cboSecurityQuestion2.SelectedIndex = cboSecurityQuestion2.Items.FindItemByText(objUser[0].Security_Question2).Index;
                        Session["Answer2"] = objUser[0].Answer2;
                        cboSecurityQuestion2.Enabled = false;
                    }
                }
                else if (hdnScreenMode.Value == "PatientPortal" || hdnScreenMode.Value == "GuarantorPortal")
                {
                    Panel1.GroupingText = "Forgot Password";
                    this.Title = "Forgot Password";
                    btnSave.Value = "OK";
                    btnClearAll.Value = "Cancel";
                    lblUserName.Text = "User ID";
                    txtUserName.ReadOnly = true;
                    txtUserName.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                    txtUserName.Text = hdnEmailID.Value;
                    MessageWindow.Title = "Forgot Password";
                    ulong ulHuman_ID = Convert.ToUInt32(hdnHumanID.Value);
                    HumanManager humanMngr = new HumanManager();
                    Human objHuman = null;
                    objHuman = humanMngr.GetById(ulHuman_ID);
                    Session["HumanRecord"]= objHuman;
                    if (objHuman != null)
                    {
                        if (hdnScreenMode.Value == "GuarantorPortal")
                        {
                            cboSecurityQuestion1.SelectedIndex = cboSecurityQuestion1.Items.FindItemByText(objHuman.Representative_Security_Question1).Index;
                            Session["Answer1"] = objHuman.Representative_Answer1;
                            cboSecurityQuestion1.Enabled = false;
                            cboSecurityQuestion2.SelectedIndex = cboSecurityQuestion2.Items.FindItemByText(objHuman.Representative_Security_Question2).Index;
                            Session["Answer2"] = objHuman.Representative_Answer2;
                            cboSecurityQuestion2.Enabled = false;
                        }
                        else
                        {
                            cboSecurityQuestion1.SelectedIndex = cboSecurityQuestion1.Items.FindItemByText(objHuman.Security_Question1).Index;
                            Session["Answer1"] = objHuman.Answer1;
                            cboSecurityQuestion1.Enabled = false;
                            cboSecurityQuestion2.SelectedIndex = cboSecurityQuestion2.Items.FindItemByText(objHuman.Security_Question2).Index;
                            Session["Answer2"] = objHuman.Answer2;
                            cboSecurityQuestion2.Enabled = false;
                        }
                    }
                }
                else if (hdnScreenMode.Value == "PatientRegister" || hdnScreenMode.Value == "GuarantorRegister")
                {
                    Panel1.GroupingText = "Register";
                    this.Title = "Register";
                    btnSave.Value = "Save";
                    btnClearAll.Value = "Clear All";
                    lblUserName.Text = "User ID";
                    txtUserName.ReadOnly = true;
                    txtUserName.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                    txtUserName.Text = (string)Request["EmailID"];
                }
                else if (hdnScreenMode.Value == "UserRegister")
                {
                    Panel1.GroupingText = "Register";
                    this.Title = "Register";
                    btnSave.Value   = "Save";
                    btnClearAll.Value = "Clear All";
                    lblUserName.Text = "User Name";
                    txtUserName.ReadOnly = true;
                    txtUserName.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                    txtUserName.Text = (string)Request["UserName"];
                }
                else if (hdnScreenMode.Value == "PatientAccount" || hdnScreenMode.Value == "PatientQuarantorAccount"|| hdnScreenMode.Value=="Representative")
                {
                    Panel1.GroupingText = "PatientAccount";
                    this.Title = "PatientAccount";
                    btnSave.Value = "Save";
                    btnClearAll.Value = "Clear All";
                    lblUserName.Text = "User ID";
                    txtUserName.ReadOnly = true;
                    txtUserName.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                    txtUserName.Text = hdnEmailID.Value;
                    MessageWindow.Title = "PatientAccount";

                    ulong ulHuman_ID = Convert.ToUInt32(hdnHumanID.Value);
                    HumanManager humanMngr = new HumanManager();
                    Human objHuman = null;
                    objHuman = humanMngr.GetById(ulHuman_ID);
                    Session["HumanRecord"] = objHuman;
                    if (objHuman != null)
                    {
                        if (hdnScreenMode.Value == "PatientQuarantorAccount")
                        {
                            cboSecurityQuestion1.SelectedIndex = cboSecurityQuestion1.Items.FindItemByText(objHuman.Representative_Security_Question1).Index;
                            Session["Answer1"] = objHuman.Representative_Answer1;
                            cboSecurityQuestion2.SelectedIndex = cboSecurityQuestion2.Items.FindItemByText(objHuman.Representative_Security_Question2).Index;
                            Session["Answer2"] = objHuman.Representative_Answer2;
                        }
                        else
                        {
                            cboSecurityQuestion1.SelectedIndex = cboSecurityQuestion1.Items.FindItemByText(objHuman.Security_Question1).Index;
                            Session["Answer1"] = objHuman.Answer1;
                            cboSecurityQuestion2.SelectedIndex = cboSecurityQuestion2.Items.FindItemByText(objHuman.Security_Question2).Index;
                            Session["Answer2"] = objHuman.Answer2;
                        }
                    }
                }
                btnSave.Disabled = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (hdnScreenMode.Value == "Login")
                {
                    if (Session["Answer1"].ToString() == txtAnswer1.Text && Session["Answer2"].ToString() == txtAnswer2.Text)
                    {
                       //Response.Redirect("frmChangePassword.aspx?ScreenMode=" + "CHANGE PASSWORD USER" + "&UserName="+txtUserName.Text+ "&IsLoginOpen=YES");
                        Response.Redirect("frmChangePassword.aspx?ScreenMode=" + "FORGOT PASSWORD USER" + "&UserName=" + txtUserName.Text + "&IsLoginOpen=YES");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('780004');", true);
                        btnSave.Disabled = false;
                    }
                }
            else if (hdnScreenMode.Value == "PatientPortal" || hdnScreenMode.Value == "GuarantorPortal")
                {
                    HumanManager humanMngr = new HumanManager();
                    Human objHuman = null;
                    ulong HumanID = Convert.ToUInt32(hdnHumanID.Value);
                    objHuman = humanMngr.GetById(HumanID);
                        if (Session["Answer1"].ToString() == txtAnswer1.Text && Session["Answer2"].ToString() == txtAnswer2.Text)
                        {
                            if (hdnScreenMode.Value == "GuarantorPortal")
                            {
                                //Response.Redirect("frmChangePassword.aspx?ScreenMode=" + "CHANGE PASSWORD QUARANTOR" + "&EmailID=" + objHuman.Representative_Email + "&IsLoginOpen=YES" + "&PatientID=" + HumanID);
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ChangePswdPatientPortal('" + objHuman.Representative_Email + "','" + HumanID + "','" + "CHANGE PASSWORD QUARANTOR" + "');", true);
                            }
                            else
                            {
                                 /////Response.Redirect("frmChangePassword.aspx?ScreenMode=" + "CHANGE PASSWORD PATIENT" + "&EmailID=" + objHuman.EMail + "&IsLoginOpen=YES" + "&HumanID=" + HumanID);
                                //Response.Redirect("frmChangePassword.aspx?ScreenMode=" + "FORGOT PASSWORD PATIENT" + "&EmailID=" + objHuman.EMail + "&IsLoginOpen=YES" + "&PatientID=" + HumanID);
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ChangePswdPatientPortal('" + objHuman.EMail + "','" + HumanID + "','" + "FORGOT PASSWORD PATIENT" + "');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('780004');", true);
                            btnSave.Disabled = false;
                        }
                    
                }
                else if (hdnScreenMode.Value == "UserRegister")
                {
                    IList<User> userList = new List<User>();
                    UserManager userMngr = new UserManager();
                    userList = userMngr.GetUser(Request["UserName"]);
                    if (userList.Count > 0)
                    {
                        hdnScreenMode.Value = "USER REGISTRATION";
                        userList[0].Security_Question1 = cboSecurityQuestion1.Text;
                        userList[0].Answer1 = txtAnswer1.Text;
                        userList[0].Security_Question2 = cboSecurityQuestion2.Text;
                        userList[0].Answer2 = txtAnswer2.Text;
                        userMngr.UpdateUserDetails(userList, string.Empty);
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenCPassword('','','YES','" + userList[0].user_name + "');", true);
                        
                    }
                }
            else if (hdnScreenMode.Value == "PatientRegister" || hdnScreenMode.Value == "PatientAccount" || hdnScreenMode.Value == "GuarantorRegister" || hdnScreenMode.Value == "PatientQuarantorAccount"|| hdnScreenMode .Value =="Representative")
                {
                    HumanManager humanMngr = new HumanManager();
                    Human objHuman = null;
                    objHuman = humanMngr.GetById(Convert.ToUInt32(Request["PatientID"]));
                    if (objHuman != null)
                    {
                        if (hdnScreenMode.Value == "GuarantorRegister" || hdnScreenMode.Value == "PatientQuarantorAccount")
                        {
                            objHuman.Representative_Security_Question1 = cboSecurityQuestion1.Text;
                            objHuman.Representative_Security_Question2 = cboSecurityQuestion2.Text;
                            objHuman.Representative_Answer1 = txtAnswer1.Text;
                            objHuman.Representative_Answer2 = txtAnswer2.Text;
                            hdnScreenMode.Value = "FORGOT PASSWORD QUARANTOR";
                            HumanList.Add(objHuman);
                            objHumenmgr.UpdateHumanEmailDetails(HumanList, string.Empty);
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ChangePswdPatientPortal('" + objHuman.Representative_Email + "','" + objHuman.Id.ToString() + "','" + "FORGOT PASSWORD QUARANTOR" + "');", true);
                           // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenCPassword('" + objHuman.Id.ToString() + "','" + objHuman.Representative_Email + "','YES','');", true);
                        }
                        else if (hdnScreenMode.Value == "PatientRegister")
                        {
                            objHuman.Security_Question1 = cboSecurityQuestion1.Text;
                            objHuman.Security_Question2 = cboSecurityQuestion2.Text;
                            objHuman.Answer1 = txtAnswer1.Text;
                            objHuman.Answer2 = txtAnswer2.Text;
                            hdnScreenMode.Value = "USER REGISTRATION PATIENT PORTAL";
                            HumanList.Add(objHuman);
                            objHumenmgr.UpdateHumanEmailDetails(HumanList, string.Empty);
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenCPasswordPatientRegister('" + objHuman.Id.ToString() + "','" + objHuman.EMail + "','YES','" + "USER REGISTRATION" + "');", true);
                        }
                        else
                        {
                            objHuman.Security_Question1 = cboSecurityQuestion1.Text;
                            objHuman.Security_Question2 = cboSecurityQuestion2.Text;
                            objHuman.Answer1 = txtAnswer1.Text;
                            objHuman.Answer2 = txtAnswer2.Text;
                            hdnScreenMode.Value = "FORGOT PASSWORD PATIENT";
                            HumanList.Add(objHuman);
                            objHumenmgr.UpdateHumanEmailDetails(HumanList, string.Empty);
                            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ChangePswdPatientPortal('" + objHuman.EMail + "','" + objHuman.Id.ToString() + "','" + "FORGOT PASSWORD PATIENT" + "');", true);
                           ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenCPassword('" + objHuman.Id.ToString() + "','" + objHuman.EMail + "','YES','');", true);
                        }
                        
                        }
                }
            }
        
    }
}
