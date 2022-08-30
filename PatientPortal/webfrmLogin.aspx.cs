using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using Acurus.Capella.PatientPortal;
using System.Web.Script.Serialization;

namespace PatientPortal
{
    public partial class webfrmLogin : System.Web.UI.Page
    {
        IList<Human> humanList = new List<Human>();
        protected void Page_Load(object sender, EventArgs e)
        {
            Acurus.Capella.PatientPortal.PatientPortalServiceReference.PatientPortalWebServiceSoapClient objPatientPortal = new Acurus.Capella.PatientPortal.PatientPortalServiceReference.PatientPortalWebServiceSoapClient();
            //string sOutput = objPatientPortal.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64("3678"));

            //IList<Human> humanTemp = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Human[]>(sOutput);

            //return;
            
            HumanManager hnProxy = new HumanManager();
       
            if (!IsPostBack)
            {
                this.Page.Title = "Patient Access-Login";
                if (Request.QueryString["PatientID"] != null)
                {
                    humanList = hnProxy.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(Request.QueryString["PatientID"]));
                    //Acurus.Capella.PatientPortal.PatientPortalServiceReference.PatientPortalWebServiceSoapClient objPatientPortal = new Acurus.Capella.PatientPortal.PatientPortalServiceReference.PatientPortalWebServiceSoapClient();
                    //string sOutput = objPatientPortal.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(Request.QueryString["PatientID"]));
                    //Human humanTemp = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Human>(sOutput);
                }
                else if (Request.QueryString["HumanID"] != null)
                    humanList = hnProxy.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(Request.QueryString["HumanID"]));
                if (Request.QueryString["Email"] != null)
                {
                    hdnEmailID.Value = Request.QueryString["Email"].ToString();
                }
                else if (Request.QueryString["EmailID"] != null)
                {
                    hdnEmailID.Value = Request.QueryString["EmailID"].ToString();
                }
                if (Request.QueryString["PatientID"] != null)
                {
                    hdnHumanID.Value = Request.QueryString["PatientID"].ToString();
                }
                if (humanList.Count > 0)
                {
                    if (hdnEmailID.Value != "")
                    {
                        if (hdnEmailID.Value == humanList[0].Representative_Email)
                        {
                            rdbtnRepresentativeLogin.Checked = true;
                        }
                        else if (hdnEmailID.Value == humanList[0].EMail)
                        {
                            rdbtnPatientLogin.Checked = true;
                        }
                    }
                    Confirmation();
                }

            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            HumanManager hnProxy = new HumanManager();
       
            if (rdbtnRepresentativeLogin.Checked == true)
            {
                humanList = hnProxy.CheckQuarantorPatientPortal(UserName.Text, Password.Text);
            }
            else
            {
                humanList = hnProxy.CheckPatientPortal(UserName.Text, Password.Text);
            }

            if (humanList.Count > 0)
            {
                ClientSession.LocalOffSetTime = hdnLocalTime.Value;
                //Session["LocalDate"] = hdnLocalDate.Value;
                ClientSession.LocalDate = hdnLocalDate.Value;
                ClientSession.UniversalTime = hdnUniversaloffset.Value;
                //Session["LocalDateAndTime"] = hdnLocalDateAndTime.Value;
                ClientSession.LocalTime = hdnLocalDateAndTime.Value;
                ClientSession.HumanId = humanList[0].Id;
                if (humanList[0].Is_Mail_Sent == "Y")
                {
                    if (rdbtnRepresentativeLogin.Checked == true)
                    {
                        ClientSession.UserName = humanList[0].Representative_Email.ToString();
                        CreateNewUserSession();//BugID:50185 
                        //Server.Transfer("webfrmPatientPortal.aspx?PatientID=" + humanList[0].Id.ToString() + "&EmailID=" + humanList[0].Representative_Email.ToString() + "&Role=" + "Representative");
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenPatientPortalPage('" + humanList[0].Id.ToString() + "','" + humanList[0].Representative_Email.ToString() + "','Representative');", true);


                    
                    }
                    else
                    {
                        ClientSession.UserName = humanList[0].EMail.ToString();
                        CreateNewUserSession();//BugID:50185 
                        //Server.Transfer("webfrmPatientPortal.aspx?PatientID=" + humanList[0].Id.ToString() + "&EmailID=" + humanList[0].EMail.ToString() + "&Role=" + "Patient");
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenPatientPortalPage('" + humanList[0].Id.ToString() + "','" + humanList[0].EMail.ToString() + "','Patient');", true);
                    }

                }
                else
                {
                    FailureText.Text = "Permission Denied";
                }
            }
            else
            {
                FailureText.Text = "Invalid Username or Password";
            }


        }

        private void ReadPdfFile()
        {
            string path = @"D:\\ent_phy1.pdf";
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(path);

            if (buffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
            }
        }

        private void CreateNewUserSession()
        {
            String User = ClientSession.UserName;

            IList<string> lstUser = UtilityManager.FindUserSessionFiles(User, string.Empty);
            if (lstUser.Count > 0)
            {
                var objIsActiveSession = (from item in lstUser where item.Contains(Session.SessionID) select item).ToList<string>();
                if (objIsActiveSession.Count == 0)
                {
                    UtilityManager.DeleteUserSessionFile(ClientSession.UserName, string.Empty);
                    UtilityManager.CreateUserSessionFile(ClientSession.UserName, Session.SessionID);
                    ClientSession.SavedSession = "TRUE";
                    UtilityManager.WriteApplicationAccessInfo(ClientSession.UserName, ClientSession.FacilityName);//user access log
                }
            }
            else
            {
                UtilityManager.CreateUserSessionFile(ClientSession.UserName, Session.SessionID);
                ClientSession.SavedSession = "TRUE";
                UtilityManager.WriteApplicationAccessInfo(ClientSession.UserName, ClientSession.FacilityName);//user access log
            }
        }

        private void ReadDocFile()
        {
            //Microsoft.Office.Interop.Word.ApplicationClass wordApp = new Microsoft.Office.Interop.Word.ApplicationClass();

            //string filePath = @"D:\\abc.doc";

            //object file = filePath;

            //object nullobj = System.Reflection.Missing.Value;


            //Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(ref file,
            //ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj,
            //ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj,
            //ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj);


            //Microsoft.Office.Interop.Word.Document doc1 = wordApp.ActiveDocument;

            //string m_Content = doc1.Content.Text;


            //doc.Close(ref nullobj, ref nullobj, ref nullobj);
        }

        protected void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            HumanManager hnProxy = new HumanManager();
       
            if (Request.QueryString["PatientID"] != null && hdnEmailID.Value != "")
            {
                string PatientName = string.Empty;
                humanList = hnProxy.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(Request.QueryString["PatientID"]));
                if (humanList.Count > 0)
                {
                    if ((humanList[0].Security_Question1 == string.Empty || humanList[0].Security_Question2 == string.Empty) && rdbtnPatientLogin.Checked == true && humanList[0].EMail != string.Empty)
                    {
                        PatientName = humanList[0].First_Name + " " + humanList[0].Last_Name + " " + humanList[0].MI;
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenModalWindowToRegister('" + PatientName.ToString() + "','" + humanList[0].EMail.ToString() + "','" + humanList[0].Id.ToString() + "','" + "PatientRegister" + "');", true);
                        //Response.Redirect("frmForgotPassword.aspx?ScreenMode=PatientRegister" + "&PatientName=" + PatientName.ToString() + "&EmailID=" + humanList[0].EMail.ToString() + "&PatientID=" + humanList[0].Id.ToString());

                    }
                    else if ((humanList[0].Representative_Security_Question1 == string.Empty || humanList[0].Representative_Security_Question2 == string.Empty) && rdbtnRepresentativeLogin.Checked == true && humanList[0].Representative_Email != string.Empty)
                    {
                        PatientName = humanList[0].Guarantor_First_Name + " " + humanList[0].Guarantor_Last_Name + " " + humanList[0].Guarantor_MI;
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenModalWindowToRegister('" + PatientName.ToString() + "','" + humanList[0].EMail.ToString() + "','" + humanList[0].Id.ToString() + "','" + "GuarantorRegister" + "');", true);
                        //Response.Redirect("frmForgotPassword.aspx?ScreenMode=GuarantorRegister" + "&PatientName=" + PatientName.ToString() + "&EmailID=" + humanList[0].Representative_Email.ToString() + "&PatientID=" + humanList[0].Id.ToString());
                    }
                    else
                    {
                        if (rdbtnPatientLogin.Checked == true && humanList[0].EMail != string.Empty)
                        {
                            PatientName = humanList[0].First_Name + " " + humanList[0].Last_Name + " " + humanList[0].MI;
                            //Response.Redirect("frmForgotPassword.aspx?ScreenMode=PatientPortal" + "&PatientName=" + PatientName.ToString() + "&EmailID=" + humanList[0].EMail.ToString() + "&PatientID=" + humanList[0].Id.ToString());
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenModalWindowToRegister('" + PatientName.ToString() + "','" + humanList[0].EMail.ToString() + "','" + humanList[0].Id.ToString() + "','" + "PatientPortal" + "');", true);
                        }
                        else if (rdbtnRepresentativeLogin.Checked == true && humanList[0].Representative_Email != string.Empty)
                        {
                            PatientName = humanList[0].Guarantor_First_Name + " " + humanList[0].Guarantor_Last_Name + " " + humanList[0].Guarantor_MI;
                            //Response.Redirect("frmForgotPassword.aspx?ScreenMode=GuarantorPortal" + "&PatientName=" + PatientName.ToString() + "&EmailID=" + humanList[0].Representative_Email.ToString() + "&PatientID=" + humanList[0].Id.ToString());
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenModalWindowToRegister('" + PatientName.ToString() + "','" + humanList[0].EMail.ToString() + "','" + humanList[0].Id.ToString() + "','" + "GuarantorPortal" + "');", true);

                        }
                    }
                }

            }


        }
        private void Confirmation()
        {
            HumanManager hnProxy = new HumanManager();
       
            string PatientName = string.Empty;
            if ((Request.QueryString["PatientID"] != null && Request.QueryString["PatientID"] != "") || Request.QueryString["HumanID"] != null)
            {
                if (Request.QueryString["HumanID"] != null)
                    humanList = hnProxy.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(Request.QueryString["HumanID"]));
                else
                    humanList = hnProxy.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(Request.QueryString["PatientID"]));
                if (rdbtnPatientLogin.Checked == true && humanList[0].Password == string.Empty && humanList[0].EMail != string.Empty)
                {
                    PatientName = humanList[0].First_Name + " " + humanList[0].Last_Name + " " + humanList[0].MI;
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenModalWindowToRegister('" + PatientName.ToString() + "','" + humanList[0].EMail.ToString() + "','" + humanList[0].Id.ToString() + "','" + "PatientRegister" + "');", true);
                    //Response.Redirect("frmForgotPassword.aspx?ScreenMode=PatientRegister" + "&PatientName=" + PatientName.ToString() + "&EmailID=" + humanList[0].EMail.ToString() + "&PatientID=" + humanList[0].Id.ToString());

                }
                else if (rdbtnRepresentativeLogin.Checked == true && humanList[0].Representative_Password == string.Empty && humanList[0].Representative_Email != string.Empty)
                {
                    PatientName = humanList[0].Guarantor_First_Name + " " + humanList[0].Guarantor_Last_Name + " " + humanList[0].Guarantor_MI;
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenModalWindowToRegister('" + PatientName.ToString() + "','" + humanList[0].EMail.ToString() + "','" + humanList[0].Id.ToString() + "','" + "GuarantorRegister" + "');", true);
                    //Response.Redirect("frmForgotPassword.aspx?ScreenMode=GuarantorRegister" + "&PatientName=" + PatientName.ToString() + "&EmailID=" + humanList[0].Representative_Email.ToString() + "&PatientID="+humanList[0].Id.ToString());

                }
                else
                {
                    if (rdbtnPatientLogin.Checked == true)
                    {
                        UserName.Text = humanList[0].EMail;
                        UserName.Enabled = false;
                        Password.Focus();
                    }
                    else
                    {
                        UserName.Text = humanList[0].Representative_Email;
                        UserName.Enabled = false;
                        Password.Focus();
                    }
                }
            }
        }

        protected void rdbtnPatientLogin_CheckedChanged(object sender, EventArgs e)
        {
            Confirmation();
        }

        protected void rdbtnRepresentativeLogin_CheckedChanged(object sender, EventArgs e)
        {
            Confirmation();
        }
    }
}
