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
using System.Text.RegularExpressions;
using Acurus.Capella.UI.Extensions;
using RestSharp;

namespace Acurus.Capella.UI
{
    public partial class C5PO : System.Web.UI.MasterPage
    {
        ArrayList windowLst = new ArrayList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["IsOpenPatientChart"] != null)
            {
                hdnIsOpenPatientChart.Value = System.Configuration.ConfigurationManager.AppSettings["IsOpenPatientChart"].ToString();
            }
            string SystemTime = string.Empty;
            if (this.Page.AppRelativeVirtualPath.ToUpper().Contains("FRMMYQUEUE") == true || this.Page.AppRelativeVirtualPath.ToUpper().Contains("FRMPATIENTCHART") == true)
            {
                ModalWindow.Visible = false;
            }

            //CAP-2007
            hdnCheckLegalOrg.Value = ConfigurationManager.AppSettings["IsChangeLegalOrg"];

            //CAP-1167
            var currentURL = Request.Url.AbsoluteUri.ToString();
            if (DirectURLUtility.IsValidRedirectUrlForLogin(currentURL))
            {
                Session["currenturl"] = HttpUtility.UrlEncode(Request.Url.AbsoluteUri);
            }

            //CAP-1311
            if (HttpContext.Current.Request.Cookies["CUserName"]?.Value == null && HttpContext.Current.Request.Cookies["CFacilityName"]?.Value == null)
            {
                //CAP-1752
                var loginpage = (ConfigurationSettings.AppSettings["IsSSOLogin"] == "Y" ? "frmLoginNew.aspx" : "frmLogin.aspx");
                if (Session["currenturl"] != null && !string.IsNullOrWhiteSpace(Session["currenturl"].ToString()))
                {
                    Response.Redirect($"~/{loginpage}?redirecturl={Session["currenturl"].ToString()}");
                }
                else
                {
                    Response.Redirect($"~/{loginpage}");
                }

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
                //CAP-1768
                if (Request["ScreenName"] != null && Request["ScreenName"].ToString().Equals("ERX", StringComparison.OrdinalIgnoreCase))
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
                    //CAP-1506
                    if (Request.QueryString["IsDirectURL"] != null)
                    {
                        string IsDirectURL = Request.QueryString["IsDirectURL"];
                        if (IsDirectURL.Equals("y",StringComparison.InvariantCultureIgnoreCase))
                        {
                            hdnIsDirectLink.Value = "true";
                        }
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
                            //CAP-1738 - In Testing and Production : Enter vitals screen is partially hidden.
                            //ModalWindow.Visible = true;
                            //ModalWindow.VisibleOnPageLoad = true;
                            //ModalWindow.VisibleStatusbar = false;
                            //ModalWindow.ReloadOnShow = true;
                            //ModalWindow.ShowContentDuringLoad = false;
                            //ModalWindow.Height = Unit.Pixel(775);
                            //ModalWindow.Width = Unit.Pixel(1255);
                            //ModalWindow.Behaviors = WindowBehaviors.None;
                            //ModalWindow.NavigateUrl = "frmImportVitals.aspx?MyHumanID=" + Request["HumanID"].ToString() + "&Date=" + Date;
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, " window.setTimeout(function () {OpenModal('Enter Vitals');}, 2000);", true);
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
                //CAP-2055
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


                            if (Request.QueryString["IsDirectURL"] != null)
                            {
                                string IsDirectURL = Request.QueryString["IsDirectURL"];
                                if (IsDirectURL.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    hdnIsDirectLink.Value = "true";
                                }
                            }

                            //CAP-1428
                            //ModalWindow.Visible = true;
                            //ModalWindow.VisibleOnPageLoad = true;
                            //ModalWindow.VisibleStatusbar = false;
                            //ModalWindow.ReloadOnShow = true;
                            //ModalWindow.ShowContentDuringLoad = false;
                            //ModalWindow.Height = Unit.Pixel(800);
                            //ModalWindow.Width = Unit.Pixel(1220);
                            //ModalWindow.Top = Unit.Pixel(1);
                            //ModalWindow.Left = Unit.Pixel(72);
                            //ModalWindow.Behaviors = WindowBehaviors.Close;
                            //ModalWindow.NavigateUrl = "HtmlPhoneEncounter.html?openingfrom=" + Convert.ToString(Request["openingfrom"]) + "&MyHumanID=" + Request["HumanID"].ToString();
                            //ModalWindow.Modal = true;
                            ////ModalWindow.NavigateUrl = "frmPhoneEncounter.aspx?MyHumanID=" + Request["HumanID"].ToString() + "&openingfrom=" + Convert.ToString(Request["openingfrom"]);
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, " window.setTimeout(function () {OpenModal('Phone Encounter');}, 2000);", true);
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
                //CAP-1768
                if (Request["ScreenName"] != null && Request["ScreenName"].ToString().Equals("CreateOrder", StringComparison.OrdinalIgnoreCase))
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
                            ModalWindow.Height = Unit.Pixel(700);
                            ModalWindow.Width = Unit.Pixel(1224);
                            ModalWindow.Behaviors = WindowBehaviors.None;
                            ModalWindow.NavigateUrl = "frmOrdersPatientBar.aspx";
                            int browserHt = 0;
                            if (Request.Cookies["Resolution"] != null && int.TryParse(Request.Cookies["Resolution"].Value, out browserHt) && browserHt < 900)
                            {
                                ModalWindow.Top = Unit.Pixel(0);
                                ModalWindow.Left = Unit.Pixel(200);
                            }
                            if (Request.QueryString["IsDirectURL"] != null)
                            {
                                string IsDirectURL = Request.QueryString["IsDirectURL"];
                                if (IsDirectURL.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    hdnIsDirectLink.Value = "true";
                                }
                            }
                        }
                    }
                }
                //CAP-1506
                //CAP-1768
                else if (Request["ScreenName"] != null && Request["ScreenName"].ToString().Equals("OrderManagement", StringComparison.OrdinalIgnoreCase))
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
                            ModalWindow.Height = Unit.Pixel(670);
                            ModalWindow.Width = Unit.Pixel(1200);
                            ModalWindow.Behaviors = WindowBehaviors.None;
                            ModalWindow.NavigateUrl = "frmOrderManagement.aspx";
                            //CAP-1506
                            if (Request.QueryString["IsDirectURL"] != null)
                            {
                                string IsDirectURL = Request.QueryString["IsDirectURL"];
                                if (IsDirectURL.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    hdnIsDirectLink.Value = "true";
                                }
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
                //CAP-2056
                else if (Request["ScreenName"] != null && Request["ScreenName"].ToString().Equals("PatientCommunication", StringComparison.OrdinalIgnoreCase))
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
                            ModalWindow.Height = Unit.Pixel(670);
                            ModalWindow.Width = Unit.Pixel(1050);
                            ModalWindow.Behaviors = WindowBehaviors.None;
                            ModalWindow.NavigateUrl = "frmPatientCommunication.aspx";
                            if (Request.QueryString["IsDirectURL"] != null)
                            {
                                string IsDirectURL = Request.QueryString["IsDirectURL"];
                                if (IsDirectURL.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    hdnIsDirectLink.Value = "true";
                                }
                            }
                        }
                    }
                }
                //CAP-2057
                else if (Request["ScreenName"] != null && Request["ScreenName"].ToString().Equals("Demographics", StringComparison.OrdinalIgnoreCase))
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
                            ModalWindow.Height = Unit.Pixel(700);
                            ModalWindow.Width = Unit.Pixel(1130);
                            ModalWindow.Behaviors = WindowBehaviors.None;
                            ModalWindow.NavigateUrl = "frmPatientDemographics.aspx?HumanId=" + Request["HumanID"];
                            if (Request.QueryString["IsDirectURL"] != null)
                            {
                                string IsDirectURL = Request.QueryString["IsDirectURL"];
                                if (IsDirectURL.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    hdnIsDirectLink.Value = "true";
                                }
                            }
                        }
                    }
                }
                //CAP-2059
                else if (Request["ScreenName"] != null && Request["ScreenName"].ToString() == "Reports")
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, " window.setTimeout(function () {OpenModal('REPORT GENERATOR');}, 2000);", true);
                    if (Request.QueryString["IsDirectURL"] != null)
                    {
                        string IsDirectURL = Request.QueryString["IsDirectURL"];
                        if (IsDirectURL.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                        {
                            hdnIsDirectLink.Value = "true";
                        }
                    }

                }
                // CAP-2054
                else if (Request["ScreenName"] != null && Request["ScreenName"].ToString().Equals("Indexing", StringComparison.OrdinalIgnoreCase))
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
                            ModalWindow.Height = Unit.Pixel(710);
                            ModalWindow.Width = Unit.Pixel(1200);
                            ModalWindow.Behaviors = WindowBehaviors.None;
                            ModalWindow.NavigateUrl = "frmIndexing.aspx?HumanId="+ hdnHumanID.Value+ "&ScreenMode=Bulk Scanning and Fax&Screen=OnlineDocuments";
                            if (Request.QueryString["IsDirectURL"] != null)
                            {
                                string IsDirectURL = Request.QueryString["IsDirectURL"];
                                if (IsDirectURL.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    hdnIsDirectLink.Value = "true";
                                }
                            }
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

                //string username = "", userrole = "", userid = "";

                //if (ClientSession.UserName != null)
                //    userid = ClientSession.UserName;
                //if (ClientSession.UserRole != null)
                //    userrole = ClientSession.UserRole;

                //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "setusername('" + userid + "|" + userrole + "|" + username + "');", true);


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

            //CAP-1167
            if (ClientSession.UserName == "" && ClientSession.FacilityName == "")
            {
                //CAP-1075
                if (DirectURLUtility.IsValidRedirectUrlForLogin(currentURL))
                {
                    var loginpage = (ConfigurationSettings.AppSettings["IsSSOLogin"] == "Y" ? "frmLoginNew.aspx" : "frmLogin.aspx");
                    var SessionCurrentUrl = Session["currenturl"]?.ToString();
                    if (!string.IsNullOrEmpty(SessionCurrentUrl))
                    {
                        //CAP-1752
                        var returnURL = $"~/{loginpage}?redirecturl={HttpUtility.UrlEncode(SessionCurrentUrl)}";
                        Session["currenturl"] = null;
                        Response.Redirect(returnURL);
                    }
                }
                //Jira CAP-1567
                else if (Request.Cookies["CeRxFlag"] == null || (Request.Cookies["CeRxFlag"] != null && Request.Cookies["CeRxFlag"].Value != "true"))
                {
                    Response.Redirect("~/frmSessionExpiredIndirectAccess.aspx");
                }
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
                //LoadRCopiaNotification();
                string sLoadRcopia = string.Empty;
                sLoadRcopia = LoadRCopiaNotification();
                if (!IsPostBack)
                {
                    string username = "", userrole = "", userid = "";

                    if (ClientSession.UserName != null)
                        userid = ClientSession.UserName;
                    if (ClientSession.UserRole != null)
                        userrole = ClientSession.UserRole;
                    string ScriptMethod = "setusername('" + userid + "|" + userrole + "|" + username + "'); ";
                    if (sLoadRcopia != null && (sLoadRcopia.StartsWith("HttpPostError") == true || sLoadRcopia.StartsWith("LoadRCopiaNotification") == true))
                    {
                        sLoadRcopia = sLoadRcopia.Replace("'", "");
                        ScriptMethod += " RcopiaErrorAlert('" + sLoadRcopia + "'); ";
                    }
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, ScriptMethod, true);
                }
                else if (sLoadRcopia != null && (sLoadRcopia.StartsWith("HttpPostError") == true || sLoadRcopia.StartsWith("LoadRCopiaNotification") == true))
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "RcopiaErrorAlert('" + sLoadRcopia + "');", true);
                }
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
                        //CAP-1651
                        if ((Request.QueryString["IsDirectURL"] ?? string.Empty).ToUpper() != "Y")
                        {
                            anchor.Attributes.Add("href", "frmPatientChart.aspx?&HumanID= " + windowLst[iLoop].ToString().Split('~')[1].Split('#')[0].Trim() + "&Source=WindowItem&PSBEncID=" + windowLst[iLoop].ToString().Split('~')[1].Split('#')[1].Split('$')[0].Trim() + "&PSBDos=" + windowLst[iLoop].ToString().Split('^')[1].Trim());
                            anchor.InnerText = iLoop.ToString() + " - " + windowLst[iLoop].ToString().Split('#')[0];

                            li.Controls.Add(anchor);
                        }

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

        public string LoadRCopiaNotification()
        {
            try
            {

                if (ClientSession.Is_RCopia_Notification_Required != "Y" || ClientSession.RCopiaUserName == string.Empty)
                {
                    tsRefill.Style.Add("display", "none");
                    tsRx_Change.Style.Add("display", "none");
                    tsRx_Pending.Style.Add("display", "none");
                    tsRx_Need_Signing.Style.Add("display", "none");
                    tsRx_RefreshRcopia.Style.Add("display", "none");
                    return string.Empty;
                }

                RCopiaGenerateXML objrcopGenXML = new RCopiaGenerateXML();
                string sInputXML = string.Empty;
                //System.Threading.Thread.Sleep(400);
                sInputXML = objrcopGenXML.CreateGetNotificationCountXML(ClientSession.LegalOrg);
                string sOutputXML = string.Empty;
                RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(ClientSession.LegalOrg);
                //System.Threading.Thread.Sleep(400);
                sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.DownloadAddress + sInputXML, 1);
                if (sOutputXML != null && sOutputXML.StartsWith("HttpPostError") == true)
                {
                    return sOutputXML;
                }
                RCopia.RCopiaXMLResponseProcess objRcopResponseXML = new RCopiaXMLResponseProcess();
                //Jira CAP-1367
                //RCopia.RCopiaXMLResponseProcess.ilstNotification.Clear();
                IList<Rcopia_NotificationDTO> ilstNotification;
                ////System.Threading.Thread.Sleep(400);
                //Jira CAP-1367
                //objRcopResponseXML.ReadXMLResponse(sOutputXML);
                objRcopResponseXML.ReadXMLResponse(sOutputXML, out ilstNotification);
                try
                {
                    //Jira CAP-1367
                    //FillRCopiaNotification();
                    FillRCopiaNotification(ilstNotification);
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                return "LoadRCopiaNotification :  " + DateTime.Now.ToString() + "<br/> <br/> "+ex.Message;
            }
            return string.Empty;
        }

        //Jira CAP-1367
        //public void FillRCopiaNotification()
        public void FillRCopiaNotification(IList<Rcopia_NotificationDTO>  ilstnotification)
        {
            string strTex = string.Empty;
            //System.Threading.Thread.Sleep(400);
            //Jira CAP-1367
            //IList<Rcopia_NotificationDTO> ilstnotification = RCopia.RCopiaXMLResponseProcess.ilstNotification;
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
                //CAP-1752
                var loginpage = (ConfigurationSettings.AppSettings["IsSSOLogin"] == "Y" ? "frmLoginNew.aspx" : "frmLogin.aspx");
                Response.Write($"<script> window.top.location.href=\"{loginpage}\"; </script>");
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
            //CAP-1752
            if (ConfigurationSettings.AppSettings["IsSSOLogin"] != "Y")
            {
                Response.Write($"<script> window.top.location.href=\"frmLogin.aspx\"; </script>");
            }

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
            //string CBeforeIfName = "BeforeIfLogoutVariable" + DateTime.Now.ToString("hh-mm-ss");
            //Response.SetCookie(new HttpCookie(CBeforeIfName) { Value = "UserAccountType=" + ClientSession.UserAccountType.ToString(), HttpOnly = false });

            #region Logout Microsoft SSO
            if (ConfigurationSettings.AppSettings["IsSSOLogin"] == "Y")
            {
                if (ClientSession.UserAccountType == "Microsoft" || ClientSession.UserAccountType == "Okta")
                {

                    //string CName = "LogoutVariable" + DateTime.Now.ToString("hh-mm-ss");
                    //Response.SetCookie(new HttpCookie(CName) { Value = "UserAccountType="+ClientSession.UserAccountType.ToString(), HttpOnly = false });
                    //SSO_Logout
                    var token = ClientSession.AccessToken;
                    var id_token = ClientSession.AccessTokenId;

                    //Revoke Token
                    var oktaDomain = ConfigurationSettings.AppSettings["okta:OktaDomain"];
                    var options = new RestClientOptions(oktaDomain)
                    {
                        MaxTimeout = -1,
                    };

                    var clientId = ConfigurationSettings.AppSettings["okta:ClientId"];
                    var clientSecret = ConfigurationSettings.AppSettings["okta:ClientSecret"];

                    var redirectUri = ConfigurationSettings.AppSettings["okta:RedirectUri"];
                    var postLogoutRedirectUri = ConfigurationSettings.AppSettings["okta:PostLogoutRedirectUri"];
                    var base64EncodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

                    var client = new RestClient(options);
                    var request = new RestRequest($"{ConfigurationManager.AppSettings["okta:RevokeURL"]}", Method.Post);
                    request.AddHeader("accept", "application/json");
                    request.AddHeader("authorization", $"Basic {base64EncodedString}");
                    request.AddParameter("token_type_hint", "access_token");
                    request.AddParameter("token", $"{token}");
                    RestResponse response = client.ExecuteAsync(request).Result;

                    //New code
                    HttpContext.Current.Session.Abandon();
                    try
                    {

                        HttpContext.Current.Application.Remove("user");
                        Session["ShowAllState"] = null;
                        Session["GeneralQShowAll"] = null;
                        //CAP-1167
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
                    }
                    catch
                    { }
                    //New code end

                    //Redirect To Logout Page
                    Response.Redirect($"{ConfigurationManager.AppSettings["okta:LogoutURL"]}?id_token_hint={id_token}&post_logout_redirect_uri={postLogoutRedirectUri}", false);
                }
                else
                {
                    Response.Write($"<script> window.top.location.href=\"frmLoginNew.aspx\"; </script>");
                }
            }
            #endregion

            HttpContext.Current.Session.Abandon();
            try
            {

                HttpContext.Current.Application.Remove("user");
                Session["ShowAllState"] = null;
                Session["GeneralQShowAll"] = null;
                //CAP-1167
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
