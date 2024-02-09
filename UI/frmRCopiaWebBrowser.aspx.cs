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
using Acurus.Capella.DataAccess;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using System.Web.Services;
using System.IO;
using System.Xml;

namespace Acurus.Capella.UI
{
    public partial class frmRCopiaWebBrowser : System.Web.UI.Page
    {
        string sMyType = string.Empty;
        ulong ulMyHumanID = 0;

        //Boolean bMyMoveButton = false;
        //Boolean bMyMoveChkBox = false;
        UtilityManager utilityMngr = new UtilityManager();
        ulong ulMyEncID = 0;
        string Is_Prescription_to_be_Pushed = string.Empty;

        ActivityLogManager ActivitylogMngr = new ActivityLogManager();
        IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
        ActivityLog activity = new ActivityLog();

        //ulong ulMyPrescriptionId = 0;

        string Openingfrm = string.Empty;
        //private Object oDocument;

        //string MySentToRCopia = string.Empty;

        

        protected void Page_Load(object sender, EventArgs e)
        {
            RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(ClientSession.LegalOrg);

            if (Convert.ToString(Request["MyType"]) != null)
            {
                sMyType = Convert.ToString(Request["MyType"]);
            }
            if ((Convert.ToString(Request["HumanID"]) != null) && (Convert.ToString(Request["HumanID"]) != "0"))
            {
                UInt64.TryParse(Request["HumanID"], out ulMyHumanID);
                hdnHumanID.Value = ulMyHumanID.ToString();
                //ulMyHumanID = Convert.ToUInt64(Request["HumanID"]);
            }
            else
            {
                ulMyHumanID = ClientSession.HumanId;
                hdnHumanID.Value = ulMyHumanID.ToString();
            }
            //Jira CAP-1567
            Response.SetCookie(new HttpCookie("CeRxHumanID") { Value = hdnHumanID.Value.ToString(), HttpOnly = false });
            if ((Convert.ToString(Request["EncID"]) != null) && ((Convert.ToString(Request["EncID"])) != "0"))
            {
                UInt64.TryParse(Request["EncID"], out ulMyEncID);
                //ulMyEncID = Convert.ToUInt64(Request["EncID"]);
            }
            else
            {
                ulMyEncID = ClientSession.EncounterId;
            }
            if (Request["LabResult"] != null && Request["LabResult"] != "Y")
            {
                hdnLabResult.Value = "Y";
            }
            else
            {
                hdnLabResult.Value = "N";
            }

            if (!IsPostBack)
            {
                if (Convert.ToString(Request["LocalTime"]) != null)
                {
                    hdnLocalTime.Value = Convert.ToString(Request["LocalTime"]);
                }

                ulong EncounterID = ClientSession.EncounterId;
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);
                //Added by Saravanakumar
                //StaticLookupManager objStaticLookupMngr = new StaticLookupManager();
                //IList<StaticLookup> ilstStaticLookup = new List<StaticLookup>();
                //ilstStaticLookup = objStaticLookupMngr.getStaticLookupByFieldName("Medication Reason Not Performed");
                //if (ilstStaticLookup.Count > 0)
                //{
                //    cboCurrentMedicationDocumented.Items.Add("");
                //    ListItem lstMedicationDocumented = null;
                //    for (int i = 0; i < ilstStaticLookup.Count; i++)
                //    {
                //        lstMedicationDocumented = new ListItem();
                //        lstMedicationDocumented.Text = ilstStaticLookup[i].Value;
                //        lstMedicationDocumented.Value = ilstStaticLookup[i].Description;
                //        cboCurrentMedicationDocumented.Items.Add(lstMedicationDocumented);
                //    }
                //}
                string sLookupReasonnotPerformedXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                if (File.Exists(sLookupReasonnotPerformedXmlFilePath) == true)
                {
                    XmlDocument itemDoc = new XmlDocument();
                    XmlTextReader XmlText = new XmlTextReader(sLookupReasonnotPerformedXmlFilePath);
                    itemDoc.Load(XmlText);
                    XmlText.Close();
                    XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("MedicationReasonNotPerformedList");
                    if (xmlNodeList != null && xmlNodeList.Count > 0)
                    {
                        cboCurrentMedicationDocumented.Items.Add("");
                        ListItem lstMedicationDocumented = null;
                        for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                        {
                            lstMedicationDocumented = new ListItem();
                            lstMedicationDocumented.Text = xmlNodeList[0].ChildNodes[j].Attributes["Value"].Value.ToString();
                            lstMedicationDocumented.Value = xmlNodeList[0].ChildNodes[j].Attributes["Description"].Value.ToString();
                            cboCurrentMedicationDocumented.Items.Add(lstMedicationDocumented);
                        }
                    }
                }
            }



            IList<Encounter> objEncounter = new List<Encounter>();
            objEncounter.Add(ClientSession.FillEncounterandWFObject.EncRecord);
            if (objEncounter.Count > 0)
            {
                ulong count = objEncounter[0].No_of_Med_Orders_In_Paper;
                if (txtcount.Value == string.Empty && (!IsPostBack))
                {
                    if (count != 0)
                    {
                        txtcount.Value = count.ToString();
                    }
                    else
                    {
                        txtcount.Value = count.ToString();
                    }
                }
                if (!IsPostBack)
                {
                    if (objEncounter[0].Is_Medication_Reviewed == "Y")
                    {
                        chkCurrentMedicationDocumented.Checked = true;
                    }
                    else
                    {
                        if (ClientSession.UserCurrentProcess == "PROVIDER_PROCESS" || ClientSession.UserCurrentProcess == "CODER_REVIEW_CORRECTION")//BugID:55123 - Do not default and show CurrMedications, if the section is not enabled for the current process
                        {
                            if (objEncounter[0].Is_Medication_Reviewed == "")
                            {
                                chkCurrentMedicationDocumented.Checked = true;
                                EnableSave.Value = "true";
                            }
                            else
                                chkCurrentMedicationDocumented.Checked = false;
                        }
                        else
                            chkCurrentMedicationDocumented.Checked = false;
                    }
                    cboCurrentMedicationDocumented.SelectedIndex = cboCurrentMedicationDocumented.Items.IndexOf(cboCurrentMedicationDocumented.Items.FindByText(objEncounter[0].Reason_Not_Performed_Medication_Reviewed));
                }
            }
            if (chkCurrentMedicationDocumented.Checked)
            {
                cboCurrentMedicationDocumented.Disabled = true;
                lblCurrentMedicationDocumented.InnerText = lblCurrentMedicationDocumented.InnerText.Replace("*", "").TrimEnd();
                lblCurrentMedicationDocumented.Attributes.Add("style", "color:black");
                cboCurrentMedicationDocumented.SelectedIndex = 0;
            }
            else
            {
                cboCurrentMedicationDocumented.Disabled = false;
                if (!lblCurrentMedicationDocumented.InnerText.Contains("*"))
                {
                    lblCurrentMedicationDocumented.InnerText += " *";
                    lblCurrentMedicationDocumented.Attributes.Add("style", "color:red");
                }
            }
            //if (Request["IsMoveButton"]!=null)
            //bMyMoveButton = Convert.ToBoolean(Request["IsMoveButton"]);
            //if (Request["IsMoveCheckbox"] != null)
            //bMyMoveChkBox = Convert.ToBoolean(Request["IsMoveCheckbox"]);
            if (Convert.ToString(Request["IsPrescriptiontobePushed"]) != null)
                Is_Prescription_to_be_Pushed = Convert.ToString(Request["IsPrescriptiontobePushed"]);
            if (Convert.ToString(Request["openingFrom"]) != null)
                Openingfrm = Convert.ToString(Request["openingFrom"]);
           // hdnLabResult.Value = Convert.ToString(Request["openingFrom"]);
            //if (Request["IsSentToRCopia"] == null)
            //{
            //    MySentToRCopia = ClientSession.FillPatientChart.PatChartList[0].Is_Sent_To_RCopia;
            //}
            //else
            //{
            //    MySentToRCopia = Request["IsSentToRCopia"];
            //}

            //if (bMyMoveButton == true)
            //{
            //    btnMove.Visible = true;


            //}
            //else
            //{
            //    btnMove.Visible = false;

            //}

            //if (bMyMoveChkBox == true)
            //{
            //    chkMove.Visible = true;
            //    tXt.Visible = true;

            //}
            //else
            //{

            //    chkMove.Visible = false;
            //    tXt.Visible = false;
            //}

            //If the check box is already checked then - we need to check it again in the form load
            if (!IsPostBack)
            {
                //if (chkMove.Visible == true)
                //{
                if (Is_Prescription_to_be_Pushed == "Y")
                {
                    chkMove.Checked = true;
                }
                else
                {
                    chkMove.Checked = false;
                }
                //}
            }

            // btnClose.Visible = true;
            //If come through EMR Menu & MA_Review, show the close button.
            //if (bMyMoveButton == true)
            //{
            //    btnClose.Visible = true;
            //    btnClose.Style.Add("display", "block");
            //}
            //else
            //{
            //    btnClose.Visible = false;
            //    btnClose.Style.Add("display", "none");

            //}

            //If come through EMR Menu

            if (Openingfrm == "Tab")
            {
                if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" || ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
                {
                    radBrowser.Style.Add("height", "505px");
                    chkMove.Visible = true;
                    chkMove.Style.Add("display", "inline-block");
                    btnClose.Visible = false;
                    tXt.Visible = true;
                    tXt.Style.Add("display", "inline-block");
                    btnMove.Visible = false;
                    btnMove.Style.Add("display", "none");
                    btnMove.InnerText = "Move to MA";
                    if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
                    {
                        Panel2.Style.Add("display", "none");
                        btnSet2.Style.Add("display", "none");
                    }
                }
                else
                {
                    btnSet1.Style.Add("display", "none");
                    btnClose.Visible = false;
                    btnClose.Style.Add("display", "none");
                    chkMove.Style.Add("display", "none");
                    tXt.Style.Add("display", "none");
                    btnMove.Style.Add("display", "none");
                    btnSet2.Style.Add("display", "none");
                    pnl.Style.Add("display", "none");
                    Panel2.Style.Add("display", "none");
                    radBrowser.Style.Add("height", "568px");

                }
                btnpatientChart1.Visible = false;
            }
            else if (Openingfrm == "Queue")
            {
                chkMove.Visible = false;
                tXt.Visible = false;
                chkMove.Style.Add("display", "none");
                tXt.Style.Add("display", "none");
                btnMove.Style.Add("display", "inline-block");
                btnClose.Style.Add("display", "inline-block");
                btnMove.Visible = true;
                btnMove.InnerText = "Move to Next Process";
                btnSet1.Style.Add("display", "none");
                pnl.Style.Add("display", "none");
                Panel2.Style.Add("display", "none");
            }
            else
            {
                btnSet1.Style.Add("display", "none");
                btnClose.Visible = true;
                btnpatientChart1.Visible = true;
                btnClose.Style.Add("display", "block");
                chkMove.Style.Add("display", "none");
                tXt.Style.Add("display", "none");
                btnMove.Style.Add("display", "none");
                pnl.Style.Add("display", "none");
                Panel2.Style.Add("display", "none");
            }

            if (btnMove.InnerText == "Move to MA" && ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
            {
                btnMove.Visible = false;
                btnMove.Style.Add("display", "none");

            }

            if (sMyType=="MESSAGE" || sMyType=="REPORT")
            {
                btnpatientChart1.Visible = false;
                btnpatientChart1.Style.Add("display", "none");
            }

            IList<Rcopia_Settings> rcopiaSettings = new List<Rcopia_Settings>();
            Rcopia_SettingsManager objRCopiaManager = new Rcopia_SettingsManager();
            rcopiaSettings = objRCopiaManager.GetRcopia_Settings(ClientSession.LegalOrg);

            if (ulMyHumanID != 0)
            {
                RCopiaTransactionManager objRcopiaMngr = new RCopiaTransactionManager();
                objRcopiaMngr.SendPatientToRCopia(ulMyHumanID, string.Empty,ClientSession.LegalOrg);
            }


            if (rcopiaSettings == null)
            {
                rcopiaSettings = objRCopiaManager.GetRcopia_Settings(ClientSession.LegalOrg);
            }
            var temp = from g in rcopiaSettings where g.Command == "get_url" select g;
            IList<Rcopia_Settings> TempList = temp.ToList<Rcopia_Settings>();

            Rcopia_Settings objRcopSettings = new Rcopia_Settings();

            if (TempList.Count > 0)
            {
                objRcopSettings = TempList[0];
            }
            string RCopiaSystemName = objRcopSettings.System_Name;
            string RCopiaPracticeName = objRcopSettings.Practice_Name;
            string RCopiaPatientSystemName = objRcopSettings.System_Name;
            string RCopiaAccount = objRcopSettings.Vendor_Password;

            hdnLocalTime.Value = DateTime.Now.ToString();

            if (sMyType == "MESSAGE")
            {
                if (rcopiaSessionMngr.sGetURL.Contains("RCExtResponse") == true)
                {
                    string sRCopiaUser = string.Empty;
                    sRCopiaUser = ClientSession.RCopiaUserName;

                    //string sUrl = "action=login&service=rcopia&rcopia_portal_system_name=" + RCopiaSystemName + "&rcopia_practice_user_name=" + RCopiaPracticeName + "&rcopia_user_id=" + sRCopiaUser + "&rcopia_patient_system_name=" + RCopiaPatientSystemName + "&close_window=n&allow_popup_screens=n&logout_url=https://cert.drfirst.com&time=";
                    string sUrl = "action=login&service=rcopia&rcopia_portal_system_name=" + RCopiaSystemName + "&rcopia_practice_user_name=" + RCopiaPracticeName + "&rcopia_user_id=" + sRCopiaUser + "&rcopia_patient_system_name=" + RCopiaPatientSystemName + "&close_window=n&allow_popup_screens=n&logout_url=&time=";//BUGID:42252

                    string sGMT = Convert.ToDateTime(hdnLocalTime.Value).ToString("MMddyyHHmmss");
                    sUrl = sUrl + sGMT + "&limp_mode=y&startup_screen=message&skip_auth=n" + RCopiaAccount;
                    string sOutput = GetMD5Hash(sUrl);
                    sOutput = sUrl.Replace(RCopiaAccount, "&MAC=" + sOutput.ToUpper());

                    radBrowser.Attributes.Add("src", rcopiaSessionMngr.RCopiaSessionAddress + sOutput);
                }
                else
                {
                    string sRCopiaUser = string.Empty;
                    sRCopiaUser = ClientSession.RCopiaUserName;

                    string sUrl = "action=login&service=rcopia&rcopia_portal_system_name=" + RCopiaSystemName + "&rcopia_practice_user_name=" + RCopiaPracticeName + "&rcopia_user_id=" + sRCopiaUser + "&rcopia_patient_system_name=" + RCopiaPatientSystemName + "&close_window=n&allow_popup_screens=n&time=";//BUGID:42252

                    string sGMT = DateTime.UtcNow.ToString("MMddyyHHmmss");
                    sUrl = sUrl + sGMT + "&limp_mode=y&startup_screen=message" + RCopiaAccount;
                    string sOutput = GetMD5Hash(sUrl);
                    sOutput = sUrl.Replace(RCopiaAccount, "&MAC=" + sOutput.ToUpper());

                    radBrowser.Attributes.Add("src", rcopiaSessionMngr.RCopiaSessionAddress + sOutput);
                }
            }
            else if (sMyType == "REPORT")
            {
                if (rcopiaSessionMngr.sGetURL.Contains("RCExtResponse") == true)
                {
                    string sRCopiaUser = string.Empty;
                    sRCopiaUser = ClientSession.RCopiaUserName;

                    //string sUrl = "action=login&service=rcopia&rcopia_portal_system_name=" + RCopiaSystemName + "&rcopia_practice_user_name=" + RCopiaPracticeName + "&rcopia_user_id=" + sRCopiaUser + "&rcopia_patient_system_name=" + RCopiaPatientSystemName + "&close_window=n&allow_popup_screens=n&logout_url=https://cert.drfirst.com&time=";
                    string sUrl = "action=login&service=rcopia&rcopia_portal_system_name=" + RCopiaSystemName + "&rcopia_practice_user_name=" + RCopiaPracticeName + "&rcopia_user_id=" + sRCopiaUser + "&rcopia_patient_system_name=" + RCopiaPatientSystemName + "&close_window=n&allow_popup_screens=n&logout_url=&time=";//BUGID:42252

                    string sGMT = Convert.ToDateTime(hdnLocalTime.Value).ToString("MMddyyHHmmss");
                    sUrl = sUrl + sGMT + "&limp_mode=y&startup_screen=report&skip_auth=n" + RCopiaAccount;
                    string sOutput = GetMD5Hash(sUrl);
                    sOutput = sUrl.Replace(RCopiaAccount, "&MAC=" + sOutput.ToUpper());

                    radBrowser.Attributes.Add("src", rcopiaSessionMngr.RCopiaSessionAddress + sOutput);
                }
                else
                {
                    string sRCopiaUser = string.Empty;
                    sRCopiaUser = ClientSession.RCopiaUserName;

                    string sUrl = "action=login&service=rcopia&rcopia_portal_system_name=" + RCopiaSystemName + "&rcopia_practice_user_name=" + RCopiaPracticeName + "&rcopia_user_id=" + sRCopiaUser + "&rcopia_patient_system_name=" + RCopiaPatientSystemName + "&close_window=n&allow_popup_screens=n&time=";//BUGID:42252

                    string sGMT = DateTime.UtcNow.ToString("MMddyyHHmmss");
                    sUrl = sUrl + sGMT + "&limp_mode=y&startup_screen=report" + RCopiaAccount;
                    string sOutput = GetMD5Hash(sUrl);
                    sOutput = sUrl.Replace(RCopiaAccount, "&MAC=" + sOutput.ToUpper());

                    radBrowser.Attributes.Add("src", rcopiaSessionMngr.RCopiaSessionAddress + sOutput);
                }
            }
            else if (sMyType == "GENERAL")
            {
                if (rcopiaSessionMngr.sGetURL.Contains("RCExtResponse") == true)
                {
                    string sRCopiaUser = string.Empty;
                    sRCopiaUser = ClientSession.RCopiaUserName;

                    //string sUrl = "action=login&service=rcopia&startup_screen=patient&rcopia_portal_system_name=" + RCopiaSystemName + "&rcopia_practice_user_name=" + RCopiaPracticeName + "&rcopia_user_id=" + sRCopiaUser + "&rcopia_patient_system_name=" + RCopiaPatientSystemName + "&rcopia_patient_external_id=" + ulMyHumanID.ToString() + "&close_window=n&allow_popup_screens=n&logout_url=https://cert.drfirst.com&time=";
                    string sUrl = "action=login&service=rcopia&startup_screen=patient&rcopia_portal_system_name=" + RCopiaSystemName + "&rcopia_practice_user_name=" + RCopiaPracticeName + "&rcopia_user_id=" + sRCopiaUser + "&rcopia_patient_system_name=" + RCopiaPatientSystemName + "&rcopia_patient_external_id=" + ulMyHumanID.ToString() + "&close_window=n&allow_popup_screens=n&logout_url=&time=";//BUGID:42252

                    string sGMT = Convert.ToDateTime(hdnLocalTime.Value).ToString("MMddyyHHmmss");
                    sUrl = sUrl + sGMT + "&skip_auth=n" + RCopiaAccount;
                    string sOutput = GetMD5Hash(sUrl);
                    //CAP-819
                    if (RCopiaAccount?.Length > 0)
                        sOutput = sUrl.Replace(RCopiaAccount, "&MAC=" + sOutput.ToUpper());

                    radBrowser.Attributes.Add("src", rcopiaSessionMngr.RCopiaSessionAddress + sOutput);
                }
                else
                {
                    string sRCopiaUser = string.Empty;
                    sRCopiaUser = ClientSession.RCopiaUserName;

                    string sUrl = "action=login&service=rcopia&startup_screen=patient&rcopia_portal_system_name=" + RCopiaSystemName + "&rcopia_practice_user_name=" + RCopiaPracticeName + "&rcopia_user_id=" + sRCopiaUser + "&rcopia_patient_system_name=" + RCopiaPatientSystemName + "&rcopia_patient_external_id=" + ulMyHumanID.ToString() + "&close_window=n&allow_popup_screens=n&time=";//BUGID:42252

                    string sGMT = DateTime.UtcNow.ToString("MMddyyHHmmss");
                    sUrl = sUrl + sGMT + RCopiaAccount;

                    string sOutput = GetMD5Hash(sUrl);
                    //CAP-819
                    if (RCopiaAccount?.Length > 0)
                        sOutput = sUrl.Replace(RCopiaAccount, "&MAC=" + sOutput.ToUpper());

                    radBrowser.Attributes.Add("src", rcopiaSessionMngr.RCopiaSessionAddress + sOutput);
                }
            }
            //added for Medication Review Status
            else if (sMyType == "REVIEW")
            {

                radBrowser.Attributes.Add("src", "frmMedicationReviewStatus.aspx");

            }
            //BugID:47790 - added Current_process Check
            if (ClientSession.UserCurrentProcess != "PROVIDER_PROCESS" && ClientSession.UserCurrentProcess != "CODER_REVIEW_CORRECTION")//Changed PHYSICIAN_CORRECTION to CODER_REVIEW_CORRECTION for PA_Workflow change
            {
                pnl.Attributes.Add("disabled", "true");
                Panel2.Attributes.Add("disabled", "true");
                chkMove.Attributes.Add("disabled", "true");
            }

        }
        protected void chkCurrentMedicationDocumented_ServerChange(object sender, EventArgs e)
        {
            btnsave.Disabled = false;

            if (chkCurrentMedicationDocumented.Checked == false)
            {
                cboCurrentMedicationDocumented.Items.Clear();
                string sLookupReasonnotPerformedXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                if (File.Exists(sLookupReasonnotPerformedXmlFilePath) == true)
                {
                    XmlDocument itemDoc = new XmlDocument();
                    XmlTextReader XmlText = new XmlTextReader(sLookupReasonnotPerformedXmlFilePath);
                    itemDoc.Load(XmlText);
                    XmlText.Close();
                    XmlNodeList xmlNodeList1 = itemDoc.GetElementsByTagName("MedicationReasonNotPerformedList");
                    if (xmlNodeList1 != null && xmlNodeList1.Count > 0)
                    {
                        cboCurrentMedicationDocumented.Items.Add("");
                        ListItem lstMedicationDocumented = null;
                        for (int j = 0; j < xmlNodeList1[0].ChildNodes.Count; j++)
                        {
                            lstMedicationDocumented = new ListItem();
                            lstMedicationDocumented.Text = xmlNodeList1[0].ChildNodes[j].Attributes["Value"].Value.ToString();
                            lstMedicationDocumented.Value = xmlNodeList1[0].ChildNodes[j].Attributes["Description"].Value.ToString();
                            cboCurrentMedicationDocumented.Items.Add(lstMedicationDocumented);
                        }
                    }
                }
            }
        }


        //Added by Selvaraman - for EPrescritpion
        public string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }

        protected void btnMove_Click(object sender, EventArgs e)
        {
            if (btnMove.InnerHtml == "Move to MA")
            {
                SubmitPrescription();
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('210001');RadWindowClose();", true);

                return;
            }
            if (Convert.ToString(Request["PrescriptionID"]) == null || Convert.ToUInt64(Convert.ToString(Request["PrescriptionID"])) == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('210009');", true);
                return;
            }
            WFObjectManager wfObjectMngr = new WFObjectManager();

            if (Convert.ToString(Request["PrescriptionID"]) != null)
            {
                wfObjectMngr.MoveToNextProcess(Convert.ToUInt64(Convert.ToString(Request["PrescriptionID"])), "E-PRESCRIBE", 1, "UNKNOWN", Convert.ToDateTime(hdnLocalTime.Value), string.Empty, null, null);
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('210001');RadWindowClose();", true);//changes for bug-id 28716

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "Close();", true);
        }

        private void SubmitPrescription()
        {
            Prescription prescRecord = new Prescription();

            prescRecord.Encounter_ID = 0;
            prescRecord.Created_By = ClientSession.UserName;
            if (hdnLocalTime.Value == "")
                hdnLocalTime.Value = DateTime.Now.ToString();
            prescRecord.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value); // DateTime.Now.ToUniversalTime();
            prescRecord.Facility_Name = ClientSession.FacilityName;
            Int32 human_id = 0;
            Int32.TryParse(Request["HumanID"], out human_id);
            prescRecord.Human_ID = human_id;
            prescRecord.Modified_By = string.Empty;
            prescRecord.Modified_Date_And_Time = DateTime.MinValue;
            prescRecord.Physician_ID = 0;
            prescRecord.Prescription_Date = Convert.ToDateTime(hdnLocalTime.Value);// DateTime.Now;
            prescRecord.Version = 0;
            IList<Prescription> prescription = new List<Prescription>();
            prescription.Add(prescRecord);
            PrescriptionManager PresMngr = new PrescriptionManager();
            WFObject WFObj = new WFObject();
            WFObj.Obj_Type = "E-PRESCRIBE";
            WFObj.Current_Owner = "UNKNOWN";
            WFObj.Current_Process = "START";
            WFObj.Current_Arrival_Time = Convert.ToDateTime(hdnLocalTime.Value);
            WFObj.Fac_Name = ClientSession.FacilityName;

            ulong ulMyPrescriptionId = PresMngr.SavePrescription(prescription.ToArray<Prescription>(), null, null, string.Empty, WFObj);

        }

        //protected void btnClose_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "RadWindowClose();", true);
        //}

        protected void btnMedReview_Click(object sender, EventArgs e) // To Make entry in the activity_log
        {
            if (hdnLocalTime.Value == "")
                hdnLocalTime.Value = DateTime.Now.ToString();
            activity.Activity_Type = "Medication Reviewed";
            activity.Encounter_ID = ClientSession.EncounterId;
            activity.Human_ID = ClientSession.HumanId;
            activity.Activity_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
            ActivityLogList.Add(activity);
            ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
            btnMedReview.Disabled = true;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        [WebMethod(EnableSession = true)]
        public static bool CheckMedReviewStatus()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return false;
            }
            bool status = false;
            IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
            ActivityLogManager ActivitylogMngr = new ActivityLogManager();
            List<string> ActivityType = new List<string>();
            List<ulong> EncounterID = new List<ulong>();

            ActivityType.Add("Medication Reviewed");
            EncounterID.Add(ClientSession.EncounterId);
            ActivityLogList = ActivitylogMngr.CheckActivityExists(ActivityType, EncounterID);
            if (ActivityLogList.Count > 0)
            {
                status = true;
            }
            return status;
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            if (txtcount.Value == "")
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('9093002');AutoSaveUnsuccessful();", true);
                return;
            }
            if (!chkCurrentMedicationDocumented.Checked && cboCurrentMedicationDocumented.Items[cboCurrentMedicationDocumented.SelectedIndex].Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('210018');AutoSaveUnsuccessful();", true);
                return;
            }
            EncounterManager ObjEncMngr = new EncounterManager();
            IList<Encounter> objEncounter = new List<Encounter>();
            IList<Encounter> objEnc = new List<Encounter>();
            objEncounter.Add(ClientSession.FillEncounterandWFObject.EncRecord);
            if (objEncounter.Count > 0)
            {
                objEncounter[0].No_of_Med_Orders_In_Paper = Convert.ToUInt16(txtcount.Value);
                //Added by Saravanakumar
                if (chkCurrentMedicationDocumented.Checked)
                    objEncounter[0].Is_Medication_Reviewed = "Y";
                else
                    objEncounter[0].Is_Medication_Reviewed = "N";
                objEncounter[0].Reason_Not_Performed_Medication_Reviewed = cboCurrentMedicationDocumented.Items[cboCurrentMedicationDocumented.SelectedIndex].Text;
                objEncounter[0].Snomed_Reason_Not_Performed_Med_Reviewed = cboCurrentMedicationDocumented.Items[cboCurrentMedicationDocumented.SelectedIndex].Value;
            }
            try
            {
                objEnc = ObjEncMngr.UpdateEncounterPaperOrder(objEncounter, string.Empty);
                //Added by Saravanakumar
                if (chkCurrentMedicationDocumented.Checked)
                    objEncounter[0].Is_Medication_Reviewed = "Y";
                else
                    objEncounter[0].Is_Medication_Reviewed = "N";
                objEncounter[0].Reason_Not_Performed_Medication_Reviewed = cboCurrentMedicationDocumented.Items[cboCurrentMedicationDocumented.SelectedIndex].Text;
                objEncounter[0].Snomed_Reason_Not_Performed_Med_Reviewed = cboCurrentMedicationDocumented.Items[cboCurrentMedicationDocumented.SelectedIndex].Value;
            }
            catch (Exception msg)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();} alert('" + msg + "');AutoSaveUnsuccessful();", true);

            }
            if (objEnc.Count > 0)
            {
                ClientSession.FillEncounterandWFObject.EncRecord = objEnc[0];
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();} DisplayErrorMessage('9060009');AutoSaveSuccessful();RefreshNotification('RcopiaMedication');", true);
            btnsave.Disabled = true;
            EnableSave.Value = "false";
        }

        protected void chkMove_CheckedChanged(object sender, EventArgs e)
        {
            EncounterManager encounterMngr = new EncounterManager();
            if (ClientSession.EncounterId == 0)
            {
                return;
            }
            IList<Encounter> encList = new List<Encounter>();
            Encounter enc = new Encounter();
            if (chkMove.Checked == true)
            {
                encList = encounterMngr.GetEncounterByEncounterID(ClientSession.EncounterId);
                enc = encList[0];

                enc.Is_Prescription_To_Be_Moved = "Y";

                encounterMngr.UpdateEncounter(enc, string.Empty, new object[] { "false" });
                enc.Version += 1;
            }
            else
            {
                encList = encounterMngr.GetEncounterByEncounterID(ClientSession.EncounterId);
                enc = encList[0];
                enc.Is_Prescription_To_Be_Moved = "N";

                encounterMngr.UpdateEncounter(enc, string.Empty, new object[] { "false" });
                enc.Version += 1;
            }
            ClientSession.FillEncounterandWFObject.EncRecord = enc;
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }



    }
}
