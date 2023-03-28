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
using Telerik.Web.UI;
using System.Text;
using Acurus.Capella.Core;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Acurus.Capella.UI.RCopia;
using Acurus.Capella.Core.DTO;
using System.Runtime.Serialization;
using System.IO;
using System.Web.SessionState;
using System.Reflection;

namespace Acurus.Capella.UI
{
    public partial class C5PO : System.Web.UI.MasterPage
    {
        ArrayList windowLst = new ArrayList();

        protected void Page_Load(object sender, EventArgs e)
        {
            string SystemTime = string.Empty;
            if (this.Page.AppRelativeVirtualPath.ToUpper().Contains("FRMMYQUEUE") == true || this.Page.AppRelativeVirtualPath.ToUpper().Contains("FRMPATIENTCHART") == true)
            {
                ModalWindow.Visible = false;
            }
            //else if (this.Page.AppRelativeVirtualPath.ToUpper().Contains("FRMPATIENTCHART") == true)
            //{
            //    ModalWindow.Visible = false;
            //}



            if (hdnSystemTime.Value != string.Empty)
                Session["SystemTime"] = hdnSystemTime.Value;
            //((Label)mnuC5PO.Items.FindItemByValue("UserName").FindControl("lblYouareloggedinas")).Text = "You are logged in as" + " " + ClientSession.UserName + "  " + "in" + "  " + ClientSession.FacilityName;
            //lblLogged.Text = "You are logged in as" + " " + ClientSession.UserName + "  " + "in" + "  " + ClientSession.FacilityName;
            if (!IsPostBack)
            {
                //ClientSession.FlushSession();
                if (Request["ScreenName"] != null && Request["ScreenName"].ToString() == "ERX")
                {
                    string local_Date = string.Empty;
                    if (hdnLocalDate.Value != string.Empty)
                    {
                        local_Date = hdnLocalDate.Value;
                    }
                    else
                    {
                        local_Date = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
                    }



                    if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.PatChartList.Count > 0)
                    {
                        if (Request["HumanID"] != null && Request["HumanID"] != string.Empty)
                        {
                            ModalWindow.Visible = true;
                            ModalWindow.VisibleOnPageLoad = true;
                            ModalWindow.VisibleStatusbar = false;
                            ModalWindow.ReloadOnShow = true;
                            ModalWindow.ShowContentDuringLoad = false;
                            //ModalWindow.Height = Unit.Pixel(650);
                            //ModalWindow.Width = Unit.Pixel(960);
                            ModalWindow.Height = Unit.Pixel(635);
                            ModalWindow.Width = Unit.Pixel(1060);
                            ModalWindow.Behaviors = WindowBehaviors.Close;
                            ModalWindow.NavigateUrl = "frmRCopiaWebBrowser.aspx?MyType=GENERAL" + "&HumanID=" + Request["HumanID"].ToString()
                                + "&EncID=0" + "&PrescriptionID=0" + "&IsMoveButton=false" + "&IsMoveCheckbox=false"
                                + "&IsPrescriptiontobePushed=N" + "&openingFrom=Menu&LocalTime=" + local_Date;
                            //ModalWindow.NavigateUrl = "frmRCopiaWebBrowser.aspx?MyType=GENERAL" + "&HumanID=" + Request["HumanID"].ToString()
                            //   + "&EncID=0" + "&PrescriptionID=0" + "&IsMoveButton=false" + "&IsMoveCheckbox=false"
                            //   + "&IsPrescriptiontobePushed=N" + "&openingFrom=Menu" +
                            //   "&IsSentToRCopia=" + ClientSession.FillPatientChart.PatChartList[0].Is_Sent_To_RCopia
                            //   + "&LocalTime=" + local_Date;
                        }
                    }
                }
                else if (Request["ScreenName"] != null && Request["ScreenName"].ToString() == "Enter Vitals")
                {
                    string Date = string.Empty;
                    if (Request["Date"] != null)
                    {
                        Date = Request["Date"].ToString();
                    }
                    else
                    {
                        Date = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
                    }

                    if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.PatChartList.Count > 0)
                    {
                        if (Request["HumanID"] != null && Request["HumanID"] != string.Empty)
                        {
                            ModalWindow.Visible = true;
                            ModalWindow.VisibleOnPageLoad = true;
                            ModalWindow.VisibleStatusbar = false;
                            ModalWindow.ReloadOnShow = true;
                            ModalWindow.ShowContentDuringLoad = false;
                            ModalWindow.Height = Unit.Pixel(775);
                            ModalWindow.Width = Unit.Pixel(1255);
                            ModalWindow.Behaviors = WindowBehaviors.None;
                            ModalWindow.NavigateUrl = "frmImportVitals.aspx?MyHumanID=" + Request["HumanID"].ToString() + "&Date=" + Date;
                        }
                    }
                }
                else if (Request["ScreenName"] != null && Request["ScreenName"].ToString() == "Manage Problem List")
                {
                    //string sPath = "htmlManageProblemList.html?version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "");
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OpenManageProbList", "function OpenModalManageProblemList() {"+
                    //        "$(document).ready(function(){$(top.window.document).find('#ProcessModal').modal({ backdrop: 'static', keyboard: false }, 'show');var sPath = '';"+
                    //        "sPath = '"+sPath+"';$(top.window.document).find('#mdldlg')[0].style.width = '970px';"+
                    //        "$(top.window.document).find('#ProcessModal')[0].style.width = '';"+
                    //        "$(top.window.document).find('#ProcessFrame')[0].contentDocument.location.href = sPath;"+
                    //        "$(top.window.document).find('#ModalTitle')[0].textContent = 'Manage Problem List';"+
                    //        "});}OpenModalManageProblemList();", true);

                    ModalWindow.Visible = true;
                    ModalWindow.VisibleOnPageLoad = true;
                    ModalWindow.VisibleStatusbar = false;
                    ModalWindow.ReloadOnShow = true;
                    ModalWindow.ShowContentDuringLoad = false;
                    //ModalWindow.Height = Unit.Pixel(650);
                    //ModalWindow.Width = Unit.Pixel(960);
                    ModalWindow.Height = Unit.Pixel(635);
                    ModalWindow.Width = Unit.Pixel(1060);
                    ModalWindow.Title = "Manage Problem List";
                    ModalWindow.Behaviors = WindowBehaviors.Close;
                    ModalWindow.NavigateUrl = "htmlManageProblemList.html?version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "");


                }
                else if (Request["ScreenName"] != null && Request["ScreenName"].ToString() == "Potential Diagnosis")
                {
                    string sPath = "htmlPotentialDiagnosis.html?version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OpenpotentialDiagnosis", "function OpenModalPotentialDiagnosis() {" +
                    "$(document).ready(function(){$(top.window.document).find('#ProcessModal').modal({ backdrop: 'static', keyboard: false }, 'show');var sPath = '';" +
                    "sPath = '" + sPath + "';$(top.window.document).find('#mdldlg')[0].style.width = '970px';" +
                    "$(top.window.document).find('#ProcessModal')[0].style.width = '';" +
                    "$(top.window.document).find('#ProcessFrame')[0].contentDocument.location.href = sPath;" +
                    "$(top.window.document).find('#ModalTitle')[0].textContent = 'Manage Potential Diagnosis';" +
                    "$(top.window.document).find('#btnClose').hide();" +
                    "});}OpenModalPotentialDiagnosis();", true);
                }
                if (Request["ScreenName"] != null && Request["ScreenName"].ToString() == "PFSH")
                {
                    if (ClientSession.FillPatientChart.PatChartList.Count > 0)
                    {
                        if (Request["HumanID"] != null && Request["HumanID"] != string.Empty)
                        {
                            if (System.Text.RegularExpressions.Regex.IsMatch(Request["HumanID"], "^[0-9]*$") == true)
                            {
                                ClientSession.HumanId = Convert.ToUInt32(Request["HumanID"]);
                            }
                            if (Request["EncounterID"] != null && Request["EncounterID"] != string.Empty && System.Text.RegularExpressions.Regex.IsMatch(Request["EncounterID"], "^[0-9]*$") == true)
                            {
                                ClientSession.EncounterId = Convert.ToUInt32(Request["EncounterID"]);
                            }
                            string FacilityName = ClientSession.FacilityName.Replace("#", "HASH");
                            PFSHWindow.NavigateUrl = "HtmlPFSH.html?MyHumanID=" + Request["HumanID"].ToString() + "&openingfrom=" + Convert.ToString(Request["openingfrom"] + "&FacilityName=" + FacilityName + "&UserRole=" + ClientSession.UserRole + "&EncounterID=" + ClientSession.EncounterId) + "&version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "");
                            PFSHWindow.Visible = true;
                            PFSHWindow.VisibleOnPageLoad = true;
                            PFSHWindow.VisibleStatusbar = false;
                            PFSHWindow.ReloadOnShow = true;
                            PFSHWindow.Height = Unit.Pixel(760);
                            PFSHWindow.Width = Unit.Pixel(1185);
                            PFSHWindow.Behaviors = WindowBehaviors.None;
                            PFSHWindow.Top = Unit.Pixel(50);
                            PFSHWindow.Left = Unit.Pixel(40);
                            PFSHWindow.Modal = true;
                        }
                    }
                }
                if (Request["ScreenName"] != null && Request["ScreenName"].ToString() == "PhoneEncounter")
                {
                    if (ClientSession.FillPatientChart.PatChartList.Count > 0)
                    {
                        if (Request["HumanID"] != null && Request["HumanID"] != string.Empty)
                        {
                            if (System.Text.RegularExpressions.Regex.IsMatch(Request["HumanID"], "^[0-9]*$") == true)
                            {
                                ClientSession.HumanId = Convert.ToUInt32(Request["HumanID"]);
                            }
                            ModalWindow.Visible = true;
                            ModalWindow.VisibleOnPageLoad = true;
                            ModalWindow.VisibleStatusbar = false;
                            ModalWindow.ReloadOnShow = true;
                            ModalWindow.ShowContentDuringLoad = false;
                            ModalWindow.Height = Unit.Pixel(800);
                            ModalWindow.Width = Unit.Pixel(1220);
                            ModalWindow.Top = Unit.Pixel(1);
                            ModalWindow.Left = Unit.Pixel(72);
                            ModalWindow.Behaviors = WindowBehaviors.Close;
                            ModalWindow.NavigateUrl = "HtmlPhoneEncounter.html?openingfrom=" + Convert.ToString(Request["openingfrom"]) + "&MyHumanID=" + Request["HumanID"].ToString();
                            ModalWindow.Modal = true;
                            //ModalWindow.NavigateUrl = "frmPhoneEncounter.aspx?MyHumanID=" + Request["HumanID"].ToString() + "&openingfrom=" + Convert.ToString(Request["openingfrom"]);
                        }
                    }
                }
                //LoadRCopiaNotification();

                if (ClientSession.HumanId != 0)
                {
                    hdnHumanID.Value = ClientSession.HumanId.ToString();
                }

                if (ClientSession.HumanId != 0)
                {
                    hdnEncounterId.Value = ClientSession.EncounterId.ToString();
                }

                //added for medication review status 
                if (ClientSession.UserRole != null)
                {
                    hdnuserrole.Value = ClientSession.UserRole.ToString();
                }

                //((Label)mnuC5PO.Items.FindItemByValue("UserName").FindControl("lblYouareloggedinas")).Text = "You are logged in as" + " " + ClientSession.UserName + "  " + "in" + "  " + ClientSession.FacilityName;
                //lblLogged.Text = "You are logged in as" + " " + ClientSession.UserName + "  " + "in" + "  " + ClientSession.FacilityName;
                if (Request["ScreenName"] != null && Request["ScreenName"].ToString() == "CreateOrder")
                {
                    if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.PatChartList.Count > 0)
                    {
                        if (Request["HumanID"] != null && Request["HumanID"] != string.Empty)
                        {
                            ModalWindow.Visible = true;
                            ModalWindow.VisibleOnPageLoad = true;
                            ModalWindow.VisibleStatusbar = false;
                            ModalWindow.ReloadOnShow = true;
                            ModalWindow.ShowContentDuringLoad = true;
                            ModalWindow.Height = Unit.Pixel(800);
                            ModalWindow.Width = Unit.Pixel(1224);
                            ModalWindow.Behaviors = WindowBehaviors.None;
                            ModalWindow.NavigateUrl = "frmOrdersPatientBar.aspx";
                            int browserHt = 0;
                            if (Request.Cookies["Resolution"] != null && int.TryParse(Request.Cookies["Resolution"].Value, out browserHt) && browserHt < 900)
                            {
                                ModalWindow.Top = Unit.Pixel(0);
                                ModalWindow.Left = Unit.Pixel(200);
                            }
                        }
                    }
                }
                else if (Request["ScreenName"] != null && Request["ScreenName"].ToString() == "MRE")
                {
                    if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.PatChartList.Count > 0)
                    {
                        if (Request["HumanID"] != null && Request["HumanID"] != string.Empty)
                        {
                            ModalWindow.Visible = true;
                            ModalWindow.VisibleOnPageLoad = true;
                            ModalWindow.VisibleStatusbar = false;
                            ModalWindow.ReloadOnShow = true;
                            ModalWindow.ShowContentDuringLoad = true;
                            ModalWindow.Height = Unit.Pixel(850);
                            ModalWindow.Width = Unit.Pixel(1160);
                            ModalWindow.Behaviors = WindowBehaviors.None;
                            ModalWindow.NavigateUrl = "frmManualResultEntry.aspx?MyHumanID=" + hdnHumanID.Value;
                        }
                    }
                }
                //if (hdnProjectName.Value == string.Empty && System.Configuration.ConfigurationManager.AppSettings["ProjectName"] != null)
                //hdnProjectName.Value = System.Configuration.ConfigurationManager.AppSettings["ProjectName"].ToString();
                if (ClientSession.LegalOrg != null)
                    hdnProjectName.Value = ClientSession.LegalOrg;
                    if (hdnProjectIPAddress.Value == string.Empty && System.Configuration.ConfigurationManager.AppSettings["ProjectIPAddress"] != null)
                    hdnProjectIPAddress.Value = System.Configuration.ConfigurationManager.AppSettings["ProjectIPAddress"].ToString();
                if (hdnMedReconcileURL.Value == string.Empty)
                    hdnMedReconcileURL.Value = System.Configuration.ConfigurationManager.AppSettings["MedReconcileURL"].ToString();

                string username = "", userrole = "", userid = "";

                if (ClientSession.UserName != null)
                    userid = ClientSession.UserName;
                if (ClientSession.UserRole != null)
                    userrole = ClientSession.UserRole;

                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "setusername('" + userid + "|" + userrole + "|" + username + "');", true);


            }
            //FillRCopiaNotification();
            SecurityServiceUtility objSecurity = new SecurityServiceUtility();
            objSecurity.ApplyUserPermissions(this.Page);
            if (mnuEMR_smnuMyQ.Disabled == true)
            {
                tsMyQ.Disabled = true;
                tsMyQ.Attributes.Remove("onclick");
                tsMyQ.Style["cursor"] = "default";
            }
            if (mnuAppointments_smnuScheduler.Disabled == true)
            {
                tsAppointments.Disabled = true;
                tsAppointments.Attributes.Remove("onclick");
                tsAppointments.Style["cursor"] = "default";
            }
            if (mnuEMR_smnuPhoneEncounter.Disabled == true)
            {
                tsPhoneEncounter.Disabled = true;
                tsPhoneEncounter.Attributes.Remove("onclick");
                tsPhoneEncounter.Style["cursor"] = "default";
            }
            if (mnuPatient_smnuPatientComm.Disabled == true)
            {
                tsTask.Disabled = true;
                tsTask.Attributes.Remove("onclick");
                tsTask.Style["cursor"] = "default";
            }
            if (mnuFile_smnuBulkScanning.Disabled == true)
            {
                tsUploadDocuments.Disabled = true;
                tsUploadDocuments.Attributes.Remove("onclick");
                tsUploadDocuments.Style["cursor"] = "default";
            }
            if (mnuExchange_smnuMailbox.Disabled == true)
            {
                tsMailbox.Disabled = true;
                tsMailbox.Attributes.Remove("onclick");
                tsMailbox.Style["cursor"] = "default";
            }
            if (mnuExchange_smnuClinicalExchange_smnuExport.Disabled == true)
            {
                tsbrowse.Disabled = true;
                tsbrowse.Attributes.Remove("onclick");
                tsbrowse.Style["cursor"] = "default";
            }

            if (ClientSession.UserName == "" && ClientSession.FacilityName == "")
            {
                //Response.Redirect("~/frmSessionExpired.aspx");
                Response.Redirect("~/frmSessionExpiredIndirectAccess.aspx");
            }
            else
            {
                //lblLogged.Text = "You are logged in as" + " " + ClientSession.UserName + "  " + "in" + "  " + ClientSession.FacilityName;

                var client = from c in ApplicationObject.ClientList where c.Legal_Org == ClientSession.LegalOrg select c;
                IList<Client> currentClientList = client.ToList<Client>();

                if (currentClientList.Count>0)
                {
                    lblLogged.Text = currentClientList[0].Client_Name + " - " + ClientSession.UserName + "  " + "in" + "  " + ClientSession.FacilityName;
                }

                //To Load RCopia Notification
                LoadRCopiaNotification();
            }
            //int indexValue = 0;
            //foreach (RadMenuItem menu in mnuC5PO.Items)
            //{
            //    if (menu.Text == "Window")
            //    {
            //        indexValue = menu.Index;
            //    }
            //}
            // windowLst= (ArrayList)ClientSession.WindowList;
            // log.Info("C5PO start mnuC5PO items add");
            //for(int iLoop=0;iLoop<windowLst.Count;iLoop++)
            //{

            //    mnuC5PO.Items[indexValue].Items.Add(new RadMenuItem(iLoop.ToString()+" - "+windowLst[iLoop].ToString().Split('#')[0]));

            //}
            //log.Info("C5PO end mnuC5PO items add");
            //if (this.Page.AppRelativeVirtualPath.ToUpper().Contains("FRMMYQUEUE") != true)
            //{
            //    mnuC5PO.Items.FindItemByValue("CloseButton").Visible = true;
            //    mnuC5PO.Items.FindItemByValue("CloseButton").Enabled = true;
            //}
            //else
            //{
            //    mnuC5PO.Items.FindItemByValue("CloseButton").Visible = false;
            //}
            /*added for medication review status */
            //if (hdnuserrole.Value == "Physician" || hdnuserrole.Value == "Physician Assistant")
            //{
            //    int toolbarIndex = tbGeneral.Items.FindItemByValue("tsRcopiarx_review_status").Index;
            //    tbGeneral.Items[toolbarIndex].Visible = true;
            //}

            hdnLocalTime.Value = UtilityManager.ConvertToUniversal().ToString().Replace('A', ' ').Replace('P', ' ').Replace('M', ' ');
            windowLst = (ArrayList)ClientSession.WindowList;

            if (windowLst != null)
            {
                for (int iLoop = 0; iLoop < windowLst.Count; iLoop++)
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    ulwindow.Controls.Add(li);
                    if (windowLst[iLoop].ToString().Split('~').Count() > 1)
                    {
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        //anchor.Attributes.Add("href", "frmPatientChart.aspx?hdnLocalTime= " + hdnLocalTime.Value + " &HumanId= " + windowLst[iLoop].ToString().Split('-')[2].Split('#')[0] + "&EncounterId=" + windowLst[iLoop].ToString().Split('-')[2].Split('#')[1].Split('$')[0]);
                        anchor.Attributes.Add("href", "frmPatientChart.aspx?&HumanID= " + windowLst[iLoop].ToString().Split('~')[1].Split('#')[0].Trim() + "&Source=WindowItem&PSBEncID=" + windowLst[iLoop].ToString().Split('~')[1].Split('#')[1].Split('$')[0].Trim()+"&PSBDos="+ windowLst[iLoop].ToString().Split('^')[1].Trim());
                        anchor.InnerText = iLoop.ToString() + " - " + windowLst[iLoop].ToString().Split('#')[0];

                        li.Controls.Add(anchor);
                    }
                    //mnuC5PO.Items[indexValue].Items.Add(new RadMenuItem(iLoop.ToString() + " - " + windowLst[iLoop].ToString().Split('#')[0]));

                }
            }
            if (this.Page.AppRelativeVirtualPath.ToUpper().Contains("FRMMYQUEUE") != true)
            {
                closebutton.Visible = true;
                closebutton.Disabled = false;
            }
            else
            {
                closebutton.Visible = false;
            }
            if (ClientSession.UserRole != null && (ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" || ClientSession.UserRole.ToUpper() == "PHYSICIAN") && CheckIfReviewStatusApplicable())
            {
                tsReviewStatus.Visible = true;
            }
            else
            {
                tsReviewStatus.Visible = false;
            }
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
            {
                hdnIsAncillary.Value = ilstFacAncillary[0].Fac_Name;
            }

        }

        public void LoadRCopiaNotification()
        {
            if (ClientSession.Is_RCopia_Notification_Required != "Y" || ClientSession.RCopiaUserName == string.Empty)
            {
                tsRefill.Style.Add("display", "none");
                tsRx_Change.Style.Add("display", "none");
                tsRx_Pending.Style.Add("display", "none");
                tsRx_Need_Signing.Style.Add("display", "none");
                tsRx_RefreshRcopia.Style.Add("display", "none");
                return;
            }

            RCopiaGenerateXML objrcopGenXML = new RCopiaGenerateXML();
            string sInputXML = string.Empty;
            //System.Threading.Thread.Sleep(400);
            sInputXML = objrcopGenXML.CreateGetNotificationCountXML(ClientSession.LegalOrg);
            string sOutputXML = string.Empty;
            RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(ClientSession.LegalOrg);
            //System.Threading.Thread.Sleep(400);
            sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.DownloadAddress + sInputXML, 1);
            RCopia.RCopiaXMLResponseProcess objRcopResponseXML = new RCopiaXMLResponseProcess();
            RCopia.RCopiaXMLResponseProcess.ilstNotification.Clear();
            //System.Threading.Thread.Sleep(400);
            objRcopResponseXML.ReadXMLResponse(sOutputXML);
            try
            {
                FillRCopiaNotification();
            }
            catch
            {
            }
        }

        public void FillRCopiaNotification()
        {
            string strTex = string.Empty;
            //System.Threading.Thread.Sleep(400);
            IList<Rcopia_NotificationDTO> ilstnotification = RCopia.RCopiaXMLResponseProcess.ilstNotification;
            if (ilstnotification != null)
            {
                for (int i = 0; i < ilstnotification.Count; i++)
                {
                    if (ilstnotification[i].Type.ToLower() == "refill")
                    {
                        tsRefill.InnerText = ilstnotification[i].Type.ToUpper() + " : " + ilstnotification[i].Number;

                    }
                    else if (ilstnotification[i].Type.ToLower() == "rx_pending")
                    {
                        tsRx_Pending.InnerText = ilstnotification[i].Type.ToUpper() + " : " + ilstnotification[i].Number;
                    }
                    else if (ilstnotification[i].Type.ToLower() == "rx_need_signing")
                    {
                        tsRx_Need_Signing.InnerText = ilstnotification[i].Type.ToUpper() + " : " + ilstnotification[i].Number;
                    }
                    else if (ilstnotification[i].Type.ToLower() == "rxchange")
                    {
                        tsRx_Change.InnerText = ilstnotification[i].Type.ToUpper() + " : " + ilstnotification[i].Number;
                    }

                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (hdnCurrentTab.Value.Contains("CloseChart"))
            {
                bool bPresent = false;
                int iLoop = 0;
                ArrayList windowLst = (ArrayList)ClientSession.WindowList;
                if (windowLst != null)
                {
                    for (iLoop = 0; iLoop < windowLst.Count; iLoop++)
                    {
                        if (windowLst[iLoop].ToString().Contains(ClientSession.HumanId.ToString()))
                        {
                            bPresent = true;
                            break;
                        }
                    }
                }
                if (bPresent == true)
                    windowLst.RemoveAt(iLoop);
                ClientSession.WindowList = windowLst;
                Response.Write("<script> window.top.location.href=\" frmMyQueue.aspx\"; </script>");
            }
            else if (hdnCurrentTab.Value.Contains("LogOut"))
            {
                Response.Write("<script> window.top.location.href=\" frmLogin.aspx\"; </script>");
                //string sUser = ClientSession.UserName;
                //UserSession objUserSession = new UserSession();
                //objUserSession.User_Name = sUser;
                //UserSessionManager userSessionMngr = new UserSessionManager();
                // userSessionMngr.DeleteUserSessionUsingUserSessionIED(sUser);
                //Global.ht.Remove(ClientSession.UserName);
                // userSessionMngr.InsertUpdateDeleteUserSessionXml(objUserSession, "", "DELETE");
                UtilityManager.DeleteUserSessionFile(string.Empty, Session.SessionID);
                ClientSession.SavedSession = "DELETED";
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

                if (Directory.Exists(Server.MapPath("atala-capture-download\\" + Session.SessionID)) == true)
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
                    catch
                    {
                    }
                }
                if (Directory.Exists(Server.MapPath("atala-capture-upload\\" + Session.SessionID)) == true)
                {
                    try
                    {
                        System.IO.Directory.Delete(Server.MapPath("atala-capture-upload\\" + Session.SessionID), true);
                    }
                    catch
                    {
                    }
                }

                HttpContext.Current.Application.Remove("user");
                Session["ShowAllState"] = null;
                Session["GeneralQShowAll"] = null;
            }
            else
            {
                ulong uHumanId = 0;
                if (ulong.TryParse(hdnHumanID.Value, out uHumanId))
                {
                    ClientSession.HumanId = Convert.ToUInt32(hdnHumanID.Value);
                    Server.Transfer("frmPatientChart.aspx?HumanID=" + hdnHumanID.Value + "&ScreenMode=Menu");
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertNoPatient", "DisplayErrorMessage('7200042');", true);
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            ulong uHumanId = 0;
            if (ulong.TryParse(hdnERXHumanId.Value, out uHumanId))
            {
                ClientSession.HumanId = Convert.ToUInt32(hdnERXHumanId.Value);
                ClientSession.EncounterId = 0;
                Response.Write("<script> window.top.location.href=\"frmPatientChart.aspx?HumanID=" + hdnERXHumanId.Value + "&ScreenName=ERX\"; </script>");
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertNoPatient", "DisplayErrorMessage('7200042');", true);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            ulong uHumanId = 0;
            if (ulong.TryParse(hdnHumanID.Value, out uHumanId))
            {
                ClientSession.HumanId = Convert.ToUInt32(hdnHumanID.Value);
                ClientSession.EncounterId = 0;
                Response.Write("<script> window.top.location.href=\"frmPatientChart.aspx?HumanID=" + hdnHumanID.Value + "&ScreenName=CreateOrder\"; </script>");
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertNoPatient", "DisplayErrorMessage('7200042');", true);
        }
        protected void HiddenButton_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Application.Remove("user");
            HttpContext.Current.Application.Remove("count");
        }

        public void AddWindowItem(string sPage)
        {
            bool bPresent = false;
            int iLoop = 0;
            windowLst = (ArrayList)ClientSession.WindowList;
            if (windowLst != null)
            {
                for (iLoop = 0; iLoop < windowLst.Count; iLoop++)
                {
                    if (windowLst[iLoop].ToString().Contains(sPage))
                    {
                        bPresent = true;
                    }
                }
            }
            else
                windowLst = new ArrayList();
            if (bPresent == false)
                windowLst.Add(sPage);
            ClientSession.WindowList = windowLst;
        }

        protected void btnMRE_Click(object sender, EventArgs e)
        {
            ulong uHumanId = 0;
            if (ulong.TryParse(hdnHumanID.Value, out uHumanId))
            {
                ClientSession.HumanId = Convert.ToUInt32(hdnHumanID.Value);
                ClientSession.EncounterId = 0;
                Response.Write("<script> window.top.location.href=\"frmPatientChart.aspx?HumanID=" + hdnHumanID.Value + "&ScreenName=MRE\"; </script>");
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertNoPatient", "DisplayErrorMessage('7200042');", true);
        }

        protected void btnlogout_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.top.location.href=\" frmLogin.aspx\"; </script>");

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
            }
            catch
            { }
            //HttpContext.Current.Session.Abandon();
        }

        protected void closebutton_Click(object sender, EventArgs e)
        {
            bool bPresent = false;
            int iLoop = 0;
            ArrayList windowLst = (ArrayList)ClientSession.WindowList;
            if (windowLst != null)
            {
                for (iLoop = 0; iLoop < windowLst.Count; iLoop++)
                {
                    if (windowLst[iLoop].ToString().Contains(ClientSession.HumanId.ToString()))
                    {
                        bPresent = true;
                        break;
                    }
                }
            }
            if (bPresent == true)
                windowLst.RemoveAt(iLoop);
            ClientSession.WindowList = windowLst;
            Response.Write("<script> window.top.location.href=\" frmMyQueueNew.aspx\"; </script>");
        }
        protected bool CheckIfReviewStatusApplicable()
        {
            bool revStatusApplicable = false;
            IList<MapPhysicianPhysicianAssistant> lstphy_PhyAsst = new List<MapPhysicianPhysicianAssistant>();
            MapPhysicianPhysicianAssitantManager objPhy_PhyAsst = new MapPhysicianPhysicianAssitantManager();
            lstphy_PhyAsst = objPhy_PhyAsst.GetMapPhysicianPhyAsstListByIDRole(Convert.ToInt32(ClientSession.CurrentPhysicianId), ClientSession.UserRole);
            if (lstphy_PhyAsst != null && lstphy_PhyAsst.Count > 0)
                revStatusApplicable = true;
            return revStatusApplicable;
        }
    }
}
