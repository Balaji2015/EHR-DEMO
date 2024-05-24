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
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Acurus.Capella.Core.DTO;
using System.Text.RegularExpressions;
using System.Drawing;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.UI;
using Telerik.Web.UI;
using System.IO;
using System.Xml;
using System.Threading;
using System.Xml.Serialization;
using System.Reflection;
using Newtonsoft.Json;
using System.Text;
using MySql.Data.MySqlClient;
using DocumentFormat.OpenXml.ExtendedProperties;

namespace Acurus.Capella.UI
{
    public partial class frmQuickPatientCreate : System.Web.UI.Page
    {
        #region Declarations
        string facility = "";
        int flag = 0;
        //IList<FieldLookup> iFieldLookuplist;
        IList<StaticLookup> iStaticLookuplist;
        //  private static readonly ILog logger = LogManager.GetLogger(typeof(frmQuickPatientCreate));
        ulong ulEncounterID = 0, MyHumanId = 0, ulMyLookupPhyID = 0;
        int iEligibility;
        string sMyEncStatus = string.Empty, ScreenMode = string.Empty;
        //Flag Variable
        bool bFormclose = true, bClose, bShowPatInfo, bValidPaymentInfo = false;
        public static string sPatlastname, sPatfirstname, sExtAccNo;
        public static DateTime dtPatDob = DateTime.MinValue;
        IList<Human> ListToUpdateHuman = new List<Human>();
        IList<Human> ListToSaveHuman = new List<Human>();
        FillQuickPatient objCheckOut;
        IList<VisitPayment> SaveVisitPaymentList = new List<VisitPayment>();
        IList<Check> SaveCheckList = new List<Check>();
        IList<Eligibility_Verification> ListToSaveEligibility = new List<Eligibility_Verification>();
        IList<PPHeader> SavePPHeaderList = new List<PPHeader>();
        IList<PPLineItem> SavePPLineItemList = new List<PPLineItem>();
        IList<AccountTransaction> SaveAccountTransactionList = new List<AccountTransaction>();
        IList<VisitPaymentHistory> SaveVisitPaymenHistoryList = new List<VisitPaymentHistory>();
        IList<VisitPaymentArc> SaveVisitPaymentArcList = new List<VisitPaymentArc>();
        IList<CheckArc> SaveCheckArcList = new List<CheckArc>();
        IList<PPHeaderArc> SavePPHeaderArcList = new List<PPHeaderArc>();
        IList<PPLineItemArc> SavePPLineItemArcList = new List<PPLineItemArc>();
        IList<AccountTransactionArc> SaveAccountTransactionArcList = new List<AccountTransactionArc>();
        IList<VisitPaymentHistoryArc> SaveVisitPaymenHistoryArcList = new List<VisitPaymentHistoryArc>();
        VisitPaymentManager visitMgr = new VisitPaymentManager();
        VisitPaymentArcManager visitArcMgr = new VisitPaymentArcManager();

        //CarrierManager CarrMngr = new CarrierManager();
        //IList<Carrier> ListCarrier = new List<Carrier>();
        public const ulong Cash_Carrier_ID = UtilityManager.CASH_CARRIER_ID;
        IList<PatientNotes> listPatientNotes = new List<PatientNotes>();
        PatientNotes objPat = new PatientNotes();
        // Human humanLoadRecord = null;
        bool bQuit = false;
        bool bEdit = false;
        bool bSendMailClick = false;
        InsurancePlanManager InsMngr = new InsurancePlanManager();
        StaticLookupManager staticLookUpMngr = new StaticLookupManager();
        HumanManager HumanMngr = new HumanManager();
        IList<PatientInsuredPlan> PatInsOrderedList = new List<PatientInsuredPlan>();
        PatientInsuredPlan objPatInsuredPlan = null;
        IList<InsurancePlan> inslist;
        IList<ulong> InsIDList = new List<ulong>();

        FillQuickPatient objCheckOutLoad = null;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            this.Page.Title = "Quick Patient Create " + " - " + ClientSession.UserName;
            //string sAncillary = "";
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)//BugID:53787,53466
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();

            if (System.Configuration.ConfigurationManager.AppSettings["DefaultCheckInButton"] != null)
            {
                this.Form.DefaultButton = System.Configuration.ConfigurationManager.AppSettings["DefaultCheckInButton"].ToString();
            }

            Viewpdf.Attributes.Add("onclick", "openPdf();return false;");
            if (IsPostBack && fileupload.PostedFile != null)
            {
                if (fileupload.PostedFile.ContentType.Contains("image") == true)
                {
                    if (fileupload.PostedFile.ContentLength < 10000000)
                    {
                        lblFileUpload.Text = fileupload.FileName;
                        string sPath = Server.MapPath("~/atala-capture-download/" + Session.SessionID);  //Page.MapPath("atala-capture-download/" + Session.SessionID);
                        DirectoryInfo virdir = new DirectoryInfo(sPath);
                        if (!virdir.Exists)
                        {
                            virdir.Create();
                        }
                        string sImgFileName = txtPatientLastName.Text + "_" + txtPatientFirstName.Text + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
                        string filename = sPath + "//" + sImgFileName;
                        fileupload.PostedFile.SaveAs(filename);
                        hdnUploadFile.Value = filename;
                        btnSave.Enabled = true;
                        btnSave.CssClass = "aspresizedgreenbutton";
                    }
                }
            }
            if (!IsPostBack)
            {



                txtRecOnAcc.Text = "0.00";
                btnAdd.Attributes.Add("onclick", "return  WaitCursor();");
                System.Diagnostics.Stopwatch LoadTime = new System.Diagnostics.Stopwatch();
                LoadTime.Start();

                txtPatientAccountNo.Attributes.Add("readonly", "true");

                hdnCarrierName.Value = "PATIENT";

                string buttonnames = System.Configuration.ConfigurationManager.AppSettings["disablecheckin"].ToString();
                string[] button = buttonnames.Split('|');
                for (int i = 0; i < button.Length; i++)
                {
                    tbl.FindControl(button[i]).Visible = true;

                }

                if (Request["EncounterID"] != null && Request["humanID"] != null && Request["EncStatus"] != null && Request["bShowPat"] != null && Request["sScreenMode"] != null)
                {
                    ulEncounterID = Convert.ToUInt64(Request["EncounterID"]);
                    hdnEncounterID.Value = Request["EncounterID"].ToString();
                    MyHumanId = Convert.ToUInt64(Request["humanID"]);
                    hdnHumanID.Value = Request["humanID"].ToString();
                    sMyEncStatus = Request["EncStatus"];
                    hdnEncStatus.Value = Request["EncStatus"];
                    bShowPatInfo = Convert.ToBoolean(Request["bShowPat"]);
                    hdnbShowPatInfo.Value = Request["bShowPat"].ToString();
                    ScreenMode = Request["sScreenMode"];

                }
                if (Request["cpt"] != null)
                {
                    hdnCPT.Value = Request["cpt"].ToString();
                    txttestappear.Text = Request["cpt"].ToString();
                    txttestappear.ToolTip = Request["cpt"].ToString();
                }
                txttestappear.Text = hdnCPT.Value.ToString();
                txttestappear.ToolTip = hdnCPT.Value.ToString();
                if (Request["Facility"] != null)
                {
                    facility = Request["Facility"].ToString();
                    hdnfacilityanc.Value = Request["Facility"].ToString();
                }
                if (Request["ParentScreen"] != null)
                {
                    hdnParentScreen.Value = Request["ParentScreen"].ToString();
                }
                if (Request["sScreenMode"] != null)
                {
                    hdnScreenMode.Value = Request["sScreenMode"].ToString();
                }
                //if (ScreenMode == "FIND PATIENT" || ScreenMode == "ELIGIBILITY" || ScreenMode == "PATIENT SUMMARY" || sAncillary.ToUpper() == ClientSession.FacilityName.ToUpper())
                if (ScreenMode == "FIND PATIENT" || ScreenMode == "ELIGIBILITY" || ScreenMode == "PATIENT SUMMARY" || (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y"))
                {
                    btnSave.Text = "Save";
                }
                if (ScreenMode == "CheckedIn")
                {
                    this.Page.Title = "Check In" + " - " + ClientSession.UserName;
                }
                if (ScreenMode == "COLLECT COPAY")
                {
                    this.Page.Title = "Collect/Edit Payment" + " - " + ClientSession.UserName;
                    hdnScreenMode.Value = "COLLECT COPAY";
                    btnSave.Text = "Save";

                }
                dtpPatientDOB.Text = string.Empty;

                dtpEffectiveStartDate.Text = string.Empty;
                dtpTerminationDate.Text = string.Empty;
                dtpCheckDate.Text = string.Empty;
                dtpCheckDate.Enabled = false;
                chkShowAllEV.Enabled = false;
                chkShowAllEVPlan.Enabled = false;



                LoadComboBox();
                cboHumanType.SelectedIndex = 2;
                cboRelation.SelectedIndex = 0;
                if (MyHumanId != 0)
                {
                    LoadPatientDetails();
                    LoadEligibilityVerification();

                    //Patientinformationdisable();
                    if (ScreenMode == "ELIGIBILITY")
                    {
                        //gbPaymentInformation.Visible = false;
                        //btnEditName.Visible = false;
                        //this.Page.Title = "Authorization & EV" + " - " + ClientSession.UserName;
                        //btnSave.Enabled = false;
                        //btnUploadDocuments.Style["margin-top"] = "5%";
                        //btnUploadDocuments.Style["margin-left"] = "3%";
                        //btnSave.Style["margin-top"] = "6%";
                        //btnSave.Style["margin-right"] = "-3%";
                        //Button1.Style["margin-top"] = "3%";
                        gbPaymentInformation.Visible = false;
                        btnEditName.Visible = false;
                        this.Page.Title = "Authorization & EV" + " - " + ClientSession.UserName;
                        btnSave.Enabled = false;
                        btnSave.CssClass = "aspresizedgreenbutton";
                        btnUploadDocuments.Style["margin-top"] = "7%";
                        btnUploadDocuments.Style["margin-left"] = "3%";
                        btnSave.Style["margin-top"] = "-6%";
                        btnSave.Style["margin-right"] = "-3%";
                        Button1.Style["margin-top"] = "0%";
                        Button1.Style["margin-right"] = "-2%";
                        Button1.Style["margin-left"] = "2%";
                        Button1.Style["margin-top"] = "-1%";
                        btnViewUpdateInsurance.Style["margin-left"] = "2%";
                        //Jira CAP-2129
                        btnProviderSave.Visible = false;
                        tdProviderSave.Visible = false;
                    }
                    if (ScreenMode == "EVSUMMARY")
                    {
                        gbPaymentInformation.Visible = false;
                        btnEditName.Visible = false;
                        this.Page.Title = "EV Summary" + " - " + ClientSession.UserName;
                        chkEligibilityVerified.Enabled = false;
                        Button1.Visible = false;
                        btnViewUpdateInsurance.Visible = false;
                        btnUpload.Visible = false;
                        cboPatientSuffix.Enabled = false;
                        btnSave.Enabled = false;
                        btnSave.CssClass = "aspresizedgreenbutton";
                    }
                    cboMethodOfPayment_SelectedIndexChanged(sender, e);


                }
                else
                {
                    txtMedicalRecordNo.Focus();
                    gbPatientInformation.Style["height"] = "100%";
                    gbPatientInformation.Style["margin-top"] = "10px";
                    gbPaymentInformation.Visible = false;
                    gbExistingPolicies.Visible = false;
                    gbEligibilityVerification.Visible = false;
                    btnUploadDocuments.Visible = false;
                    Button1.Visible = false;
                    btnViewUpdateInsurance.Visible = false;
                    btnSave.Style["margin-top"] = "1%";
                    btnSave.Style["width"] = "57%";
                    btnSave.Style["margin-right"] = "0%";
                    btnQuit.Style["margin-top"] = "2%";


                    if (Request["sPatlastname"] != null)
                    {
                        txtPatientLastName.Text = Request["sPatlastname"];
                    }
                    if (Request["sFirstName"] != null)
                    {
                        txtPatientFirstName.Text = Request["sFirstName"];
                    }
                    if (Request["sExtAccNo"] != null)
                    {
                        txtExternalAccNo.Text = Request["sExtAccNo"];
                    }
                    if (Request["DOB"] != null)
                    {
                        if (Request["DOB"].Trim() != "__-___-____")
                        {

                            dtpPatientDOB.Text = Request["DOB"].ToString();
                        }

                    }
                    pnlAuthorization.Visible = false;
                    lblAuthTip.Visible = false;
                    btnAuthorization.Visible = false;
                }

                bClose = false;
                if (hdnScreenMode.Value.ToUpper() == "SCANNING")
                {
                    gbEligibilityVerification.Visible = false;
                    gbExistingPolicies.Visible = false;
                    btnEditName.Enabled = false;
                    btnSave.Enabled = false;
                    btnSave.CssClass = "aspresizedgreenbutton";
                }
                EligibilityDisabled();
                if (MyHumanId != 0)
                {
                    Patientinformationdisable();
                }
                if (sMyEncStatus.ToUpper() == "MA_PROCESS" || sMyEncStatus.ToUpper() == "CHECK_OUT_WAIT" || sMyEncStatus.ToUpper() == "CHECK_OUT" || sMyEncStatus.ToUpper() == "CHECK_OUT_COMPLETE")
                {
                    btnSave.Enabled = false;
                    btnSave.CssClass = "aspresizedgreenbutton";
                    btnEditName.Enabled = false;
                    chkEligibilityVerified.Enabled = false;
                    MaskedTextBoxColorChange(msktxtSSN, false);
                    MaskedTextBoxColorChange(msktxtZipcode, false);
                    txtMedicalRecordNo.Enabled = false;
                    TextBoxColorChange(txtMedicalRecordNo, false);
                    chkOnlineAccess.Enabled = false;
                    MaskedTextBoxColorChange(msktxtCellPhno, false);
                    MaskedTextBoxColorChange(msktxtHomePhno, false);
                    TextBoxColorChange(txtExternalAccNo, false);
                    cboPreferredConfidentialCoreespondenceMode.Enabled = false;
                    txtMail.ReadOnly = false;
                }
                if (bShowPatInfo == true)
                {
                    Patientinformationdisable();
                    chkEligibilityVerified.Enabled = false;
                    chkMultiplePayments.Enabled = false;
                    if (hdnScreenMode.Value != "COLLECT COPAY")
                        tablelayoutdisable(gbPaymentInformation);
                    btnQuit.Focus();
                    btnEditName.Visible = false;
                    chkOnlineAccess.Enabled = false;
                    btnSave.Visible = false;
                    //CAP-2102
                    btnProviderSave.Visible = false;
                    btnUploadDocuments.Visible = false;
                    lblAuthTip.Visible = false;
                    btnAuthorization.Visible = false;
                    ddlauthPayer.Enabled = false;
                    ddlauthinsplan.Enabled = false;
                    txtauthnumber.Enabled = false;
                    txtauthValidfrom.Enabled = false;
                    txtauthvalidTo.Enabled = false;

                    MaskedTextBoxColorChange(txtauthValidfrom, false);
                    MaskedTextBoxColorChange(txtauthvalidTo, false);
                    TextBoxColorChange(txtauthnumber, false);
                    ComboBoxColorChange(ddlauthPayer, false);
                    ComboBoxColorChange(ddlauthinsplan, false);
                }

                if (chkOnlineAccess.Checked == true)
                {
                    btnSendMail.Enabled = true;
                    txtMail.ReadOnly = false;

                }
                else
                {
                    btnSendMail.Enabled = false;
                }
                if (hdnScreenMode.Value == "SCANNING" || hdnScreenMode.Value == "FIND PATIENT")
                {
                    btnEditName.Visible = false;
                    btnSave.Enabled = false;
                    btnSave.CssClass = "aspresizedgreenbutton";
                }
                if (hdnScreenMode.Value == "PATIENT SUMMARY")
                {
                    tablelayoutdisable(gbPaymentInformation);
                    cboMethodOfPayment.Enabled = false;
                    Button1.Visible = false;
                    fileupload.Enabled = false;
                    btnClear.Enabled = false;
                    btnUpload.Disabled = true;
                    btnViewUpdateInsurance.Visible = false;
                }
                if (hdnScreenMode.Value == "COLLECT COPAY")
                {
                    tablelayoutdisable(gbEligibilityVerification);
                    tablelayoutdisable(gbExistingPolicies);
                    cboMethodOfPayment.Enabled = true;
                    chkMultiplePayments.Enabled = true;
                }
                if (hdnScreenMode.Value == "ELIGIBILITY")
                {

                    if (sMyEncStatus.ToUpper() == "MA_PROCESS")
                    {

                        gbEligibilityVerification.Enabled = false;
                    }
                    else
                    {
                        chkEligibilityVerified.Enabled = true;

                    }
                    tablelayoutenable(gbEligibilityVerification);
                    disablecontrolsoneligibilty();
                }
                DateTimePickerColorChangeForWindows(dtpCheckDate, false);
                TextBoxColorChange(txtEligibilityVerificationDate, false);
                TextBoxColorChange(txtpaidBy, false);
                ComboBoxColorChange(cboRelation, false);
                txtpaidBy.Text = txtPatientLastName.Text + ',' + txtPatientFirstName.Text;
                LoadTime.Stop();
                lblLoad.Text = "Page Load -Minutes: " + LoadTime.Elapsed.Minutes.ToString() + " Seconds :" + LoadTime.Elapsed.Seconds.ToString() + " MilliSec : " + LoadTime.Elapsed.Milliseconds.ToString();
                btnSave.Attributes.Add("Onclick", "javascript:return ValidatePatientInformation();");
                btnSendMail.Attributes.Add("Onclick", "javascript:return SendEmailValidation();");
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "showTime();", true);
                if (hdnScreenMode.Value != "PATIENT SUMMARY")
                {
                    btnAdd.Enabled = true;
                    btnClear.Enabled = true;
                }

                TextBoxColorChange(txtRecOnAcc, false);
                TextBoxColorChange(txtRefundAmount, false);
                MaskedTextBoxColorChange(dtpCheckDate, false);
                if ((ClientSession.UserRole.ToUpper() == "FRONT OFFICE") && (sMyEncStatus.ToUpper() == "MA_PROCESS" || sMyEncStatus.ToUpper() == "CHECK_OUT_WAIT" || sMyEncStatus.ToUpper() == "CHECK_OUT" || sMyEncStatus.ToUpper() == "CHECK_OUT_COMPLETE"))
                {
                    cboMethodOfPayment.Enabled = false;
                    btnAdd.Enabled = false;
                    btnClear.Enabled = false;
                    ComboBoxColorChange(cboMethodOfPayment, false);


                }

            }

            //CAP-923
            if (Request["sScreenMode"] == "CheckedIn")
            {
                this.Page.Title = "Check In" + " - " + ClientSession.UserName;
            }
            //CAP-1395
            if (Request["sScreenMode"] == "ELIGIBILITY")
            {
                this.Page.Title = "Authorization & EV" + " - " + ClientSession.UserName;
            }
            //CAP-1531
            if (Request["sScreenMode"] == "PATIENT SUMMARY")
            {
                this.Page.Title = "Patient Summary" + " - " + ClientSession.UserName;
            }

            if (Button1.Visible == true)
            {
                if (ClientSession.UserPermissionDTO != null && ClientSession.UserPermissionDTO.Scntab != null)
                {
                    var scn_id = (from p in ClientSession.UserPermissionDTO.Scntab where p.SCN_Name == "frmPerformEV" select p).ToList();
                    if (scn_id.Count() > 0 && ClientSession.UserPermissionDTO.Screens != null)

                    {
                        var SendEV = from p in ClientSession.UserPermissionDTO.Screens where p.SCN_ID == Convert.ToInt32(scn_id[0].SCN_ID) && p.Permission == "U" select p;
                        if (SendEV.Count() > 0)
                        {
                            Button1.Visible = true;
                            Viewpdf.Visible = true;
                        }
                        else
                        {
                            Button1.Visible = false;
                            Viewpdf.Visible = false;
                        }
                    }
                    else
                        Button1.Visible = false;
                }

            }

            string sFormName = string.Empty;
            string screenName = string.Empty;
            IList<ScnTab> TempCheckScnTab = new List<ScnTab>();
            string sBACType = string.Empty;
            IList<ScnTab> MyScnTab;

            if (this.Items["Title"] == null)
            {
                sFormName = this.AppRelativeVirtualPath;
                string[] formname = sFormName.Split('/');
                screenName = formname[1].Substring(0, formname[1].IndexOf('.'));
            }
            else
            {
                sFormName = this.Items["Title"].ToString();
                string[] formname = sFormName.Split('/');
                screenName = sFormName;// formname[1].Substring(0, formname[1].IndexOf('.'));
            }
            if (ClientSession.UserPermissionDTO.Scntab != null && ClientSession.UserPermissionDTO.Scntab.Count != 0)
            {
                var ScnTabCheck = from c in ClientSession.UserPermissionDTO.Scntab where c.SCN_Name.ToUpper() == screenName.ToUpper() select c;
                if (ScnTabCheck != null)
                {
                    TempCheckScnTab = ScnTabCheck.ToList<ScnTab>();
                    if (TempCheckScnTab.Count != 0)
                    {
                        sBACType = TempCheckScnTab[0].Is_UBAC_Or_PBAC;
                    }
                }
            }
            if (sBACType == "UBAC")
            {
                if (ClientSession.UserPermissionDTO != null && ClientSession.UserPermissionDTO.Scntab != null)
                {
                    var Screen = from s in ClientSession.UserPermissionDTO.Scntab where s.SCN_Name.ToUpper() == screenName.ToUpper() select s;
                    MyScnTab = Screen.ToList<ScnTab>();
                    if (MyScnTab.Count != 0 && ClientSession.UserPermissionDTO.Userscntab != null)
                    {
                        var MyuserScnTab = from c in ClientSession.UserPermissionDTO.Userscntab where c.scn_id == Convert.ToUInt64(MyScnTab[0].SCN_ID) select c;

                        if (MyuserScnTab.ToList<user_scn_tab>().Count == 0)
                        {
                            if (ClientSession.UserCurrentProcess == string.Empty || ClientSession.UserCurrentProcess == "DISABLE")
                            {
                                ClientSession.processCheck = true;
                                ClientSession.UserCurrentProcess = "DISABLE";
                            }
                        }
                        else
                        {
                            ClientSession.processCheck = false;
                        }
                    }
                }
            }

            SecurityServiceUtility objSecurityServiceUtility = new SecurityServiceUtility();
            objSecurityServiceUtility.ApplyUserPermissions(this);
            //CAP-1572
            btnUploadDocuments.CssClass = "aspresizedbluebutton";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }

        private void disablecontrolsoneligibilty()
        {

            TextBoxColorChange(txtPolicyHolderID, false);
            TextBoxColorChange(txtGroupNumber, false);
            TextBoxColorChange(txtDemoNote, false);
            ComboBoxColorChange(cboVerificationType, false);
            ComboBoxColorChange(ddlPayerName, false);
            ComboBoxColorChange(ddlPlanName, false);
            MaskedTextBoxColorChange(dtpEffectiveStartDate, false);
            MaskedTextBoxColorChange(dtpTerminationDate, false);
            MaskedTextBoxColorChange(msktxtZipcode, false);
            //CAP-1327
            MaskedTextBoxColorChange(msktxtCellPhno, false);
            TextBoxColorChange(txtClaimMailingName, false);
            TextBoxColorChange(txtStreet, false);
            TextBoxColorChange(txtClaimMailingName, false);
            TextBoxColorChange(txtClaimCity, false);
            TextBoxColorChange(txtClaimCity2, false);

            MaskedTextBoxColorChange(dtpPCPEffectiveDate, false);
            TextBoxColorChange(txtInsurancetype, false);
            TextBoxColorChange(txtPlanNumber, false);
            TextBoxColorChange(txtSubscriberName, false);
            TextBoxColorChange(txtPCPName, false);
            TextBoxColorChange(txtRelationship, false);
            TextBoxColorChange(txtPCP_NPI, false);
            TextBoxColorChange(txtGroupNumber, false);
            TextBoxColorChange(txtOrganization, false);
            TextBoxColorChange(txtErrorMessage, false);


            TextBoxColorChange(txtGroupName, false);
            TextBoxColorChange(txtIPAName, false);
            TextBoxColorChange(txtPCPVisitInCopay, false);
            TextBoxColorChange(txtPCPVisitInCoIns, false);
            TextBoxColorChange(txtPCPVisitOutCopay, false);
            TextBoxColorChange(txtPCPVisitOutCoIns, false);
            TextBoxColorChange(txtSpecialityVisitInCopay, false);
            TextBoxColorChange(txtSpecialityVisitInCoIns, false);
            TextBoxColorChange(txtSpecialityVisitOutCopay, false);
            TextBoxColorChange(txtSpecialityVisitOutCoIns, false);
            TextBoxColorChange(txtMedicationInCopay, false);
            TextBoxColorChange(txtMedicationInCoIns, false);
            TextBoxColorChange(txtMedicationOutCopay, false);
            TextBoxColorChange(txtMedicationOutCoIns, false);
            TextBoxColorChange(txtUrgentCareInCopay, false);
            TextBoxColorChange(txtUrgentCareCoInIns, false);
            TextBoxColorChange(txtUrgentCareOutCopay, false);
            TextBoxColorChange(txtUrgentCareOutCoIns, false);
            TextBoxColorChange(txtPCPInNetworkMessage, false);
            TextBoxColorChange(txtPCPOutNetworkMessage, false);
            TextBoxColorChange(txtInDeductiblePlan, false);
            TextBoxColorChange(txtInPockot, false);
            TextBoxColorChange(txtOutDeductiblePlan, false);
            TextBoxColorChange(txtOutPocket, false);
            TextBoxColorChange(txtInDeductiblemet, false);
            TextBoxColorChange(txtInpocketmet, false);
            TextBoxColorChange(txtOutDeductiblemet, false);
            TextBoxColorChange(txtOutpocketmet, false);
            TextBoxColorChange(txtInFamilyDeductible, false);
            TextBoxColorChange(txtInFamilypocket, false);
            TextBoxColorChange(txtOutFamilyDeductible, false);
            TextBoxColorChange(txtOutFamilypocket, false);
            TextBoxColorChange(txtInFamilyDeductiblemet, false);
            TextBoxColorChange(txtInFamilymetpocket, false);
            TextBoxColorChange(txtOutFamilyDeductiblemet, false);
            TextBoxColorChange(txtOutFamilymetpocket, false);
            TextBoxColorChange(txtDeductibleInNetworkMessage, false);
            TextBoxColorChange(txtDeductibleOutNetworkMessage, false);
        }
        public void LoadComboBox()
        {
            string[] FieldName = { "VERIFICATIONTYPE", "METHOD OF PAYMENT", "SEX", "PREFERRED CONFIDENTIAL CORRESPONDENCE MODE", "HUMAN TYPE", "RELATIONSHIP_FOR_PAYMENT" };
            objCheckOut = HumanMngr.GetStaticLookupandCarrier(FieldName);
            IList<StaticLookup> VERIFICATIONTYPEStaticLst = null;
            VERIFICATIONTYPEStaticLst = objCheckOut.StaticLookupList.Where(l => l.Field_Name == "VERIFICATIONTYPE").ToList<StaticLookup>();

            cboVerificationType.Items.Clear();
            cboVerificationType.Items.Add("");


            if (VERIFICATIONTYPEStaticLst != null && VERIFICATIONTYPEStaticLst.Count > 0)
            {
                foreach (FieldLookup j in VERIFICATIONTYPEStaticLst)
                {
                    cboVerificationType.Items.Add(j.Value);
                }
            }

            IList<StaticLookup> METHODOFPAYMENTStaticLst = null;
            METHODOFPAYMENTStaticLst = objCheckOut.StaticLookupList.Where(l => l.Field_Name == "METHOD OF PAYMENT").ToList<StaticLookup>();

            cboMethodOfPayment.Items.Clear();
            cboMethodOfPayment.Items.Add("");


            if (METHODOFPAYMENTStaticLst != null && METHODOFPAYMENTStaticLst.Count > 0)
            {

                foreach (FieldLookup j in METHODOFPAYMENTStaticLst)
                {
                    cboMethodOfPayment.Items.Add(j.Value);
                }
            }

            IList<StaticLookup> SEXStaticLst = null;
            SEXStaticLst = objCheckOut.StaticLookupList.Where(l => l.Field_Name == "SEX").ToList<StaticLookup>();

            cboPatientSex.Items.Clear();

            if (SEXStaticLst != null && SEXStaticLst.Count > 0)
                SEXStaticLst = (from h in SEXStaticLst orderby h.Sort_Order select h).ToList<StaticLookup>();
            if (SEXStaticLst != null && SEXStaticLst.Count > 0)
            {

                foreach (FieldLookup j in SEXStaticLst)
                {
                    cboPatientSex.Items.Add(j.Value);
                }
            }

            IList<StaticLookup> RelationshipStaticLst = null;
            RelationshipStaticLst = objCheckOut.StaticLookupList.Where(l => l.Field_Name == "RELATIONSHIP_FOR_PAYMENT").ToList<StaticLookup>();

            if (RelationshipStaticLst != null && RelationshipStaticLst.Count > 0)
            {

                foreach (FieldLookup j in RelationshipStaticLst)
                {
                    cboRelation.Items.Add(j.Value);
                }
            }


            IList<StaticLookup> PreferredConfidentialCorrespondenceModeStaticLst = null;
            PreferredConfidentialCorrespondenceModeStaticLst = objCheckOut.StaticLookupList.Where(l => l.Field_Name == "PREFERRED CONFIDENTIAL CORRESPONDENCE MODE").ToList<StaticLookup>();

            cboPreferredConfidentialCoreespondenceMode.Items.Clear();
            cboPreferredConfidentialCoreespondenceMode.Items.Add("");


            if (PreferredConfidentialCorrespondenceModeStaticLst != null && PreferredConfidentialCorrespondenceModeStaticLst.Count > 0)
            {

                foreach (FieldLookup j in PreferredConfidentialCorrespondenceModeStaticLst)
                {
                    cboPreferredConfidentialCoreespondenceMode.Items.Add(j.Value);
                }
            }

            IList<StaticLookup> HUMANTYPEStaticLst = null;
            HUMANTYPEStaticLst = objCheckOut.StaticLookupList.Where(l => l.Field_Name == "HUMAN TYPE").ToList<StaticLookup>();

            cboHumanType.Items.Clear();
            cboPatientSuffix.Items.Clear();
            string sLookupXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
            if (File.Exists(sLookupXmlFilePath) == true)
            {
                XmlDocument itemDoc = new XmlDocument();
                XmlTextReader XmlText = new XmlTextReader(sLookupXmlFilePath);
                itemDoc.Load(XmlText);
                XmlText.Close();
                XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("DemographicsList");
                if (xmlNodeList != null && xmlNodeList.Count > 0)
                {
                    Dictionary<string, string> dictPaitentSuffix = new Dictionary<string, string>();
                    for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                    {
                        if (xmlNodeList[0].ChildNodes[j].Attributes[0].Value == "PATIENT SUFFIX")
                            dictPaitentSuffix.Add(xmlNodeList[0].ChildNodes[j].Attributes[1].Value, xmlNodeList[0].ChildNodes[j].Attributes[1].Value);
                    }

                    cboPatientSuffix.DataSource = dictPaitentSuffix;
                    cboPatientSuffix.DataTextField = "Key";
                    cboPatientSuffix.DataValueField = "Value";
                    cboPatientSuffix.DataBind();
                }



            }
            if (HUMANTYPEStaticLst != null && HUMANTYPEStaticLst.Count > 0)
            {
                foreach (FieldLookup j in HUMANTYPEStaticLst)
                {
                    cboHumanType.Items.Add(j.Value);
                }
            }
            //IList<Carrier> carrierLst = objCheckOut.CarrierList;

            //if (carrierLst != null && carrierLst.Count > 0)
            //{
            //    Session["CarrierList"] = objCheckOut.CarrierList;
            //    ddlPayerName.Items.Add("");
            //    ddlPayerName.Items.Add("OTHER");
            //    ddlauthPayer.Items.Add("");
            //    ddlauthPayer.Items.Add("OTHER");
            //    for (int i = 0; i < carrierLst.Count; i++)
            //    {
            //        ListItem lstCarrier = new ListItem();
            //        lstCarrier.Text = carrierLst[i].Carrier_Name;
            //        lstCarrier.Value = carrierLst[i].Id.ToString();
            //        ddlPayerName.Items.Add(lstCarrier);
            //        //ddlauthPayer.Items.Add(lstCarrier);

            //        ListItem lstAuthCarrier = new ListItem();
            //        lstAuthCarrier.Text = carrierLst[i].Carrier_Name;
            //        lstAuthCarrier.Value = carrierLst[i].Id.ToString();
            //        ddlauthPayer.Items.Add(lstAuthCarrier);

            //    }
            //}

        }
        #region Web Methods

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string LoadEVDetails(string PlanData)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            try
            {
                Eligibility_VerficationManager obj = new Eligibility_VerficationManager();
                ArrayList Filelist = new ArrayList();
                Filelist = obj.GetEvhumnaPlanResponseFile(Convert.ToUInt64(PlanData.Split('|')[0]), Convert.ToUInt64(PlanData.Split('|')[1]), PlanData.Split('|')[2]);


                return JsonConvert.SerializeObject(Filelist);
            }
            catch (Exception exception)
            {
                var lstFinalResult = new
                {
                    Error = "The following error occurred :" + exception.Message + ". Please contact support."
                };
                return JsonConvert.SerializeObject(lstFinalResult);
            }





        }



        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string LoadAuthorization(string EncounterID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            try
            {

                EncounterManager obj = new EncounterManager();
                IList<Encounter> lst = new List<Encounter>();
                lst = obj.GetEncounterByEncounterID(Convert.ToUInt64(EncounterID));
                AuthorizationManager objauth = new AuthorizationManager();
                ArrayList AuthDetails = new ArrayList();
                IList<AuthorizationEncounter> lstauthenc = new List<AuthorizationEncounter>();
                AuthorizationEncounterManager objauthenc = new AuthorizationEncounterManager();
                string flag = "N";
                lstauthenc = objauthenc.GetAuthdetailsByEncID(Convert.ToUInt64(EncounterID));
                ulong[] id = new ulong[] { };
                List<ulong> procid = new List<ulong>();
                if (lstauthenc.Count > 0)
                {
                    for (int i = 0; i < lstauthenc.Count; i++)
                    {
                        procid.Add(lstauthenc[i].Authorization_Procedure_ID);
                    }
                    AuthDetails = objauth.GetAuthorizationProcedureforencounter(procid);
                    flag = "Y";
                }
                else if (lst.Count > 0)
                {
                    AuthDetails = objauth.GetAuthorizationProcedureforHuman(lst[0].Human_ID.ToString(), Convert.ToDateTime(lst[0].Appointment_Date).ToString("yyyy-MM-dd"));
                }

                var result = new { AuthDetails = AuthDetails, IsAuth = flag };
                return JsonConvert.SerializeObject(result);

            }
            catch (Exception exception)
            {
                var lstFinalResult = new
                {
                    Error = "The following error occurred :" + exception.Message + ". Please contact support."
                };
                return JsonConvert.SerializeObject(lstFinalResult);
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string SaveAuthorization(string AuthData)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            try
            {
                IList<AuthorizationEncounter> lstAuthenc = new List<AuthorizationEncounter>();
                if (AuthData != "")
                {
                    string[] AuthCPt = AuthData.Split('|');
                    AuthorizationProcedureManager objproman = new AuthorizationProcedureManager();
                    IList<AuthorizationProcedure> lstproc = new List<AuthorizationProcedure>();
                    IList<AuthorizationProcedure> lstprocsave = new List<AuthorizationProcedure>();
                    for (int i = 0; i < AuthCPt.Length; i++)
                    {
                        AuthorizationEncounter obj = new AuthorizationEncounter();
                        obj.Authorization_ID = Convert.ToUInt64(AuthCPt[i].Split('~')[0]);
                        obj.Authorization_Procedure_ID = Convert.ToUInt64(AuthCPt[i].Split('~')[1]);
                        obj.Human_ID = Convert.ToUInt64(AuthCPt[i].Split('~')[2]);
                        obj.Encounter_ID = Convert.ToUInt64(AuthCPt[i].Split('~')[3]);
                        obj.Created_By = ClientSession.UserName;
                        obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        lstAuthenc.Add(obj);
                        lstproc = objproman.GetAuthProcDetailsUsingAuthProID(Convert.ToUInt64(AuthCPt[i].Split('~')[1]));
                        if (lstproc.Count > 0)
                        {
                            lstproc[0].Units_Used = lstproc[0].Units_Used + 1;
                            objproman.SaveUpdateDeleteWithTransaction(ref lstprocsave, lstproc, null, string.Empty);
                        }

                    }


                    AuthorizationEncounterManager objauthman = new AuthorizationEncounterManager();
                    objauthman.SaveUpdateDeleteWithTransaction(ref lstAuthenc, null, null, string.Empty);
                }
                return "Success";
            }
            catch (Exception exception)
            {
                var lstFinalResult = new
                {
                    Error = "The following error occurred :" + exception.Message + ". Please contact support."
                };
                return "error";
            }
        }



        #endregion


        private void LoadPatientDetails()
        {
            string insuranceplan_id = "";
            string human_id = "";
            string[] insurancetypes = new[] { "PRIMARY", "SECONDARY", "TERTIARY", "" };

            Human humanLoadRecord = null;
            if (hdnEncounterID.Value != string.Empty && hdnEncounterID.Value != "0")
            {
                objCheckOutLoad = HumanMngr.LoadQuickPatient(Convert.ToUInt64(hdnEncounterID.Value));
            }
            else if (hdnEncounterID.Value != string.Empty && hdnEncounterID.Value.Trim() != ""&& hdnEncounterID.Value == "0" && MyHumanId != 0)
            {
                objCheckOutLoad = HumanMngr.LoadQuickPatient(Convert.ToUInt64(hdnEncounterID.Value), MyHumanId);
            }
            if (objCheckOutLoad != null && objCheckOutLoad.HumanObj != null)
            {
                humanLoadRecord = objCheckOutLoad.HumanObj;
            }
            if (objCheckOutLoad == null || objCheckOutLoad.EncounterObj == null)
            {
                return;
            }
            if (hdnEncounterID.Value != string.Empty && hdnEncounterID.Value != "0" && humanLoadRecord.Photo_Path.ToString() != string.Empty)
                LoadPatientPhoto(humanLoadRecord.Photo_Path.ToString());

            DataTable dtHuman = new DataTable();
            dtHuman.Columns.Add("Policy Holder Id", typeof(string));
            dtHuman.Columns.Add("Plan #", typeof(string));
            dtHuman.Columns.Add("Plan Name", typeof(string));
            dtHuman.Columns.Add("Group #", typeof(string));
            dtHuman.Columns.Add("Ins. Type", typeof(string));
            dtHuman.Columns.Add("Insured Party", typeof(string));
            dtHuman.Columns.Add("Relationship", typeof(string));
            dtHuman.Columns.Add("Active", typeof(string));
            dtHuman.Columns.Add("Eff. Start Date", typeof(string));
            dtHuman.Columns.Add("Term. Date", typeof(string));
            dtHuman.Columns.Add("Carrier #", typeof(string));
            dtHuman.Columns.Add("Insured Name", typeof(string));
            dtHuman.Columns.Add("Insured #", typeof(string));
            dtHuman.Columns.Add("SPC CoPay $", typeof(string));
            dtHuman.Columns.Add("PCP CoPay $", typeof(string));
            dtHuman.Columns.Add("Deduct. $", typeof(string));
            dtHuman.Columns.Add("Deduct. Met $", typeof(string));
            dtHuman.Columns.Add("Co ins. %", typeof(string));
            dtHuman.Columns.Add("Eligibility_Type", typeof(string));
            dtHuman.Columns.Add("Carrier Name", typeof(string));

            if (objCheckOutLoad.HumanObj != null)
            {
                //Added by Priyangha//
                if (objCheckOutLoad.HumanObj.Declared_Bankruptcy == "Y")
                {
                    lblPaymentInCollection.ForeColor = Color.Red;
                    lblDeclaredBankruptcy.ForeColor = Color.Blue;
                    lblDeclaredBankruptcy.Text += " " + objCheckOutLoad.HumanObj.Declared_Bankruptcy.ToUpper() + "";
                }
                if (objCheckOutLoad.HumanObj.Declared_Bankruptcy == "N")
                {
                    lblDeclaredBankruptcy.Text += " " + objCheckOutLoad.HumanObj.Declared_Bankruptcy.ToUpper() + "";
                }

                //****************//
                txtPatientAccountNo.Text = Convert.ToString(objCheckOutLoad.HumanObj.Id);
                txtMedicalRecordNo.Text = objCheckOutLoad.HumanObj.Medical_Record_Number;

                txtExternalAccNo.Text = objCheckOutLoad.HumanObj.Patient_Account_External;
                txtPatientLastName.Text = objCheckOutLoad.HumanObj.Last_Name;
                txtPatientFirstName.Text = objCheckOutLoad.HumanObj.First_Name;
                txtPatientMI.Text = objCheckOutLoad.HumanObj.MI;
                dtpPatientDOB.Text = objCheckOutLoad.HumanObj.Birth_Date.ToString("dd-MMM-yyyy");
                cboPatientSex.Text = objCheckOutLoad.HumanObj.Sex;
                msktxtCellPhno.Text = objCheckOutLoad.HumanObj.Cell_Phone_Number;
                msktxtHomePhno.Text = objCheckOutLoad.HumanObj.Home_Phone_No;
                msktxtZipcode.Text = objCheckOutLoad.HumanObj.ZipCode;
                txtMail.Text = objCheckOutLoad.HumanObj.EMail;
                msktxtSSN.Text = objCheckOutLoad.HumanObj.SSN;
                ViewState["email"] = objCheckOutLoad.HumanObj.EMail;
                string Is_Mail_Sent = objCheckOutLoad.HumanObj.Is_Mail_Sent;
                hdnPastDue.Value = objCheckOutLoad.HumanObj.Past_Due.ToString();
                if (objCheckOutLoad.HumanObj.Preferred_Confidential_Correspodence_Mode != null && objCheckOutLoad.HumanObj.Preferred_Confidential_Correspodence_Mode != "")
                {
                    cboPreferredConfidentialCoreespondenceMode.Text = objCheckOutLoad.HumanObj.Preferred_Confidential_Correspodence_Mode;
                }
                if (Is_Mail_Sent == "Y")
                {
                    chkOnlineAccess.Checked = true;

                }
                else if (Is_Mail_Sent == "N")
                {
                    chkOnlineAccess.Checked = false;

                }
                for (int i = 0; i < cboHumanType.Items.Count; i++)
                {
                    if (Convert.ToString(cboHumanType.Items[i].Value).ToUpper() == objCheckOutLoad.HumanObj.Human_Type.ToUpper())
                    {
                        cboHumanType.SelectedIndex = i;
                        break;
                    }
                }
                for (int i = 0; i < cboPatientSuffix.Items.Count; i++)
                {
                    if (Convert.ToString(cboPatientSuffix.Items[i].Value).ToUpper() == objCheckOutLoad.HumanObj.Suffix.ToUpper())
                    {
                        cboPatientSuffix.SelectedIndex = i;
                        break;
                    }
                }
                PaymentInformationClearAll();
                txtPastDue.Text = Convert.ToString(objCheckOutLoad.HumanObj.Past_Due);
                lblPaymentInCollection.Text = "Patient In Collection : " + objCheckOutLoad.HumanObj.People_In_Collection.ToUpper() + " ";
                human_id = txtPatientAccountNo.Text;
                if (objCheckOutLoad.HumanObj.PatientInsuredBag != null)
                    //PatInsOrderedList = objCheckOutLoad.HumanObj.PatientInsuredBag.OrderBy(x => x.Sort_Order).Where(s => insurancetypes.Contains(s.Insurance_Type) && s.Active == "Yes").ToList<PatientInsuredPlan>();
                    PatInsOrderedList = objCheckOutLoad.HumanObj.PatientInsuredBag.OrderBy(x => x.Insurance_Type).Where(s => insurancetypes.Contains(s.Insurance_Type) && s.Active == "Yes").ToList<PatientInsuredPlan>();

                InsIDList = PatInsOrderedList.Select(C => C.Insurance_Plan_ID).ToList();
                IList<InsurancePlan> ilstInsPlan = new List<InsurancePlan>();
                ilstInsPlan = InsMngr.GetInsuranceListbyIDList(InsIDList.Cast<ulong>().ToArray());

                Session["PatInsuredList"] = ilstInsPlan;


                ddlPayerName.Items.Clear();
                ddlauthPayer.Items.Clear();
                ddlPlanName.Items.Clear();
                ddlauthinsplan.Items.Clear();

                for (int i = 0; i < PatInsOrderedList.Count; i++)
                {
                    DataRow dr = dtHuman.NewRow();

                    dr[0] = PatInsOrderedList[i].Policy_Holder_ID;
                    dr[1] = PatInsOrderedList[i].Insurance_Plan_ID.ToString();
                    //Bug Id :45792
                    IList<InsurancePlan> lstinsp = new List<InsurancePlan>();
                    var insp = from f in ilstInsPlan where f.Id == PatInsOrderedList[i].Insurance_Plan_ID select f;
                    lstinsp = insp.ToList<InsurancePlan>();

                    if (lstinsp != null && lstinsp.Count > 0)
                    {
                        CarrierManager carrierMngr = new CarrierManager();
                        IList<Carrier> ilstCarrier = carrierMngr.GetAll();

                        dr[2] = lstinsp[0].Ins_Plan_Name;
                        dr[10] = lstinsp[0].Carrier_ID;

                        var carr = from f in ilstCarrier where f.Id == Convert.ToUInt64(lstinsp[0].Carrier_ID) select f;

                        ListItem lstCarrier = new ListItem();

                        if (i == 0)
                        {
                            lstCarrier.Text = "";
                            lstCarrier.Value = "";
                            ddlPayerName.Items.Add(lstCarrier);
                        }

                        lstCarrier = new ListItem();
                        lstCarrier.Text = carr.ToList<Carrier>()[0].Carrier_Name;
                        lstCarrier.Value = lstinsp[0].Carrier_ID.ToString();
                        ddlPayerName.Items.Add(lstCarrier);
                        ddlPayerName.SelectedIndex = 1;
                        dr[19] = carr.ToList<Carrier>()[0].Carrier_Name;

                        ListItem lstAuthCarrier = new ListItem();
                        ListItem lstAuthInsurancePlan = new ListItem();

                        if (i == 0)
                        {
                            lstAuthCarrier = new ListItem();
                            lstAuthCarrier.Text = "";
                            lstAuthCarrier.Value = "";
                            ddlauthPayer.Items.Add(lstAuthCarrier);
                        }

                        lstAuthCarrier = new ListItem();
                        lstAuthCarrier.Text = carr.ToList<Carrier>()[0].Carrier_Name;
                        lstAuthCarrier.Value = lstinsp[0].Carrier_ID.ToString();
                        ddlauthPayer.Items.Add(lstAuthCarrier);
                        ddlauthPayer.SelectedIndex = 1;
                    }
                    else
                    {
                        dr[2] = "";
                        dr[10] = "";
                    }
                    dr[3] = PatInsOrderedList[i].Group_Number;
                    dr[4] = PatInsOrderedList[i].Insurance_Type;
                    dr[5] = objCheckOutLoad.HumanObj.Last_Name + " " + objCheckOutLoad.HumanObj.First_Name;
                    dr[6] = PatInsOrderedList[i].Relationship;
                    dr[7] = PatInsOrderedList[i].Active;

                    if (PatInsOrderedList[i].Insurance_Type.ToUpper() == "PRIMARY")
                    {
                        insuranceplan_id = PatInsOrderedList[i].Insurance_Plan_ID.ToString();
                        ddlauthPayer.SelectedValue = lstinsp[0].Carrier_ID.ToString();
                    }
                    dr[11] = objCheckOutLoad.HumanObj.Last_Name + " " + objCheckOutLoad.HumanObj.First_Name;
                    dr[12] = PatInsOrderedList[i].Insured_Human_ID.ToString();

                    dtHuman.Rows.Add(dr);




                    var patientlist = (from c in objCheckOutLoad.EligibilityList where c.Insurance_Plan_ID.ToString() == PatInsOrderedList[i].Insurance_Plan_ID.ToString() select c).ToList<Eligibility_Verification>();
                    if (patientlist.Count() > 0)
                    {
                        dr[14] = patientlist[0].PCP_Copay.ToString();
                        dr[13] = patientlist[0].SPC_Copay.ToString();
                        dr[15] = patientlist[0].Deductible_For_Plan.ToString();
                        dr[16] = patientlist[0].Deductible_Met_So_Far.ToString();
                        dr[17] = patientlist[0].Coinsurance.ToString();
                        dr[18] = patientlist[0].Eligibility_Type.ToString();

                        dr[8] = patientlist[0].Effective_Date.ToString("dd-MMM-yyyy");

                        if (patientlist[0].Effective_Date != DateTime.MinValue)
                            dr[8] = patientlist[0].Effective_Date.ToString("dd-MMM-yyyy");
                        else
                            dr[8] = string.Empty;

                        if (patientlist[0].Termination_Date != DateTime.MinValue)
                            dr[9] = patientlist[0].Termination_Date.ToString("dd-MMM-yyyy");
                        else
                            dr[9] = string.Empty;
                        //dr[19] = patientlist[0].Plan_Type.ToString();
                        //dr[20] = patientlist[0].Subscriber_ID.ToString();
                        //dr[21] = patientlist[0].Organization.ToString();
                        //dr[22] = patientlist[0].Subscriber_Name.ToString();
                        //dr[23] = patientlist[0].PCP_Name.ToString();
                        //dr[24] = patientlist[0].Relationship_to_Subscriber.ToString();
                        //dr[25] = patientlist[0].PCP_NPI.ToString();
                        //dr[26] = patientlist[0].Group_Number.ToString();

                        //if (patientlist[0].PCP_Effective_Date != DateTime.MinValue)
                        //    dr[27] = patientlist[0].PCP_Effective_Date.ToString("dd-MMM-yyyy");
                        //else
                        //    dr[27] = string.Empty;
                        //dr[28] = patientlist[0].Group_Name.ToString();


                    }

                }


                if (ddlauthPayer.SelectedItem != null && ddlauthPayer.SelectedItem.Text == string.Empty && ddlauthinsplan.SelectedItem == null)
                {
                    txtauthnumber.Enabled = false;
                    txtauthnumber.CssClass = "nonEditabletxtbox";
                    txtauthnumber.Text = string.Empty;
                }



                grdExistingPolicies.DataSource = dtHuman;
                grdExistingPolicies.DataBind();
                if (grdExistingPolicies.Rows.Count == 0)
                {
                    CreateEmptyPolicyGridGrid();




                    ddlPayerName.Text = string.Empty;
                    txtClaimCity.Text = string.Empty;
                    txtClaimMailingName.Text = string.Empty;
                    txtStreet.Text = string.Empty;
                    txtClaimMailingName.Text = string.Empty;
                    txtClaimCity2.Text = string.Empty;
                    txtPolicyHolderID.Text = string.Empty;
                    txtClaimState.Text = string.Empty;
                    ddlPlanName.Text = string.Empty;
                    txtGroupNumber.Text = string.Empty;
                    txtInsurancetype.Text = string.Empty;
                    txtDemoNote.Text = string.Empty;

                    txtPCPVisitInCopay.Text = string.Empty;
                    txtPCPVisitInCoIns.Text =
                    txtPCPVisitOutCopay.Text = string.Empty;
                    txtPCPVisitOutCoIns.Text = string.Empty;
                    txtSpecialityVisitInCopay.Text = string.Empty;
                    txtSpecialityVisitInCoIns.Text = string.Empty;
                    txtSpecialityVisitOutCopay.Text = string.Empty;
                    txtSpecialityVisitOutCoIns.Text = string.Empty;
                    txtMedicationInCopay.Text = string.Empty;
                    txtMedicationInCoIns.Text = string.Empty;
                    txtMedicationOutCopay.Text = string.Empty;
                    txtMedicationOutCoIns.Text = string.Empty;
                    txtUrgentCareInCopay.Text = string.Empty;
                    txtUrgentCareCoInIns.Text = string.Empty;
                    txtUrgentCareOutCopay.Text = string.Empty;
                    txtUrgentCareOutCoIns.Text = string.Empty;
                    txtInDeductiblePlan.Text = string.Empty;
                    txtInPockot.Text = string.Empty;
                    txtOutDeductiblePlan.Text = string.Empty;
                    txtOutPocket.Text = string.Empty;
                    txtInDeductiblemet.Text = string.Empty;
                    txtInpocketmet.Text = string.Empty;
                    txtOutDeductiblemet.Text = string.Empty;
                    txtOutpocketmet.Text = string.Empty;
                    txtInFamilyDeductible.Text = string.Empty;
                    txtInFamilypocket.Text = string.Empty;
                    txtOutFamilyDeductible.Text = string.Empty;
                    txtOutFamilypocket.Text = string.Empty;
                    txtInFamilyDeductiblemet.Text = string.Empty;
                    txtInFamilymetpocket.Text = string.Empty;
                    txtOutFamilyDeductiblemet.Text = string.Empty;
                    txtOutFamilymetpocket.Text = string.Empty;
                    txtPlanNumber.Text = string.Empty;
                    txtSubscriberName.Text = string.Empty;
                    txtRelationship.Text = string.Empty;
                    txtGroupName.Text = string.Empty;
                    txtPCPName.Text = string.Empty;
                    txtPCP_NPI.Text = string.Empty;
                    dtpPCPEffectiveDate.Text = string.Empty;
                    txtIPAName.Text = string.Empty;
                    txtPCPInNetworkMessage.Text = string.Empty;
                    txtPCPOutNetworkMessage.Text = string.Empty;
                    txtDeductibleInNetworkMessage.Text = string.Empty;
                    txtDeductibleOutNetworkMessage.Text = string.Empty;
                    txtInsurancetype.Text = string.Empty;
                    txtOrganization.Text = string.Empty;

                    dtpEffectiveStartDate.Text = string.Empty;
                    dtpTerminationDate.Text = string.Empty;
                    dtpPCPEffectiveDate.Text = string.Empty;
                }
                else if (grdExistingPolicies.Rows.Count > 0)
                {
                    if (insuranceplan_id != "")
                    {
                        try
                        {
                            ddlchange();
                            ddlauthinsplan.SelectedValue = insuranceplan_id;
                        }
                        catch(Exception ex)
                        {

                        }
                    }

                    grdExistingPolicies.SelectedIndex = 0;

                    grdExistingPolicies_SelectedIndexChanged(new object(), new EventArgs());


                }
            }

            if (ddlauthPayer.SelectedItem != null && ddlauthPayer.SelectedItem.Text != string.Empty && ddlauthinsplan.SelectedItem != null && ddlauthinsplan.SelectedItem.Text != string.Empty && grdExistingPolicies.Rows.Count > 0)
            {
                txtauthnumber.Enabled = true;
                txtauthnumber.CssClass = "Editabletxtbox";
            }

            LoadPaymentInfoGrid(objCheckOutLoad.VisitPaymentDTO);

            //authauth
            txtauthnumber.Text = objCheckOutLoad.EncounterObj.Authorization_Number;
            try
            {
                if (objCheckOutLoad.EncounterObj.Auth_Insurance_Plan_ID != 0)
                {
                    //IList<InsurancePlan> patInsList = (IList<InsurancePlan>)Session["PatInsuredList"];
                    //var insplan = from ip in patInsList where ip.Id == Convert.ToUInt64(objCheckOutLoad.EncounterObj.Auth_Insurance_Plan_ID) select ip;
                    IList<InsurancePlan> insList = InsMngr.GetInsurancebyID(Convert.ToUInt64(objCheckOutLoad.EncounterObj.Auth_Insurance_Plan_ID));
                    if (insList.Count > 0)
                    {
                        ddlauthPayer.SelectedValue = insList[0].Carrier_ID.ToString();
                        ddlAuthourPayerName_SelectedIndexChanged(new object(), new EventArgs());
                        ddlauthinsplan.SelectedValue = insList[0].Id.ToString();

                    }
                }
            }
            catch
            {
                //lst[0].Auth_Insurance_Plan_ID = 0;
            }
            txtauthValidfrom.Text = objCheckOutLoad.EncounterObj.Valid_From;
            txtauthvalidTo.Text = objCheckOutLoad.EncounterObj.Valid_To;
            if (objCheckOutLoad.EligibilityList != null && objCheckOutLoad.EligibilityList.Count > 0)
            {
                Session["EligibilityList"] = objCheckOutLoad.EligibilityList;
                if (grdExistingPolicies.Rows.Count > 0)
                {
                    int iRowIndex = grdExistingPolicies.SelectedIndex;
                    var EligibilityList = (from c in objCheckOutLoad.EligibilityList where c.Insurance_Plan_ID.ToString() == grdExistingPolicies.Rows[0].Cells[2].Text.ToString() select c).OrderByDescending(a => a.Created_Date_And_Time).ToList<Eligibility_Verification>();
                    if (EligibilityList.Count() > 0)
                    {
                        if (objCheckOutLoad.EligibilityList[0].Payer_Name.ToString() != string.Empty)
                        {
                            if (objCheckOutLoad.EligibilityList[0].Eligibility_Type.ToUpper() == "ELECTRONIC")
                            {
                                cboVerificationType.Items.Remove("ELECTRONIC");
                                cboVerificationType.Items.Add("ELECTRONIC");
                                cboVerificationType.SelectedValue = "ELECTRONIC";
                            }
                            else
                            {
                                cboVerificationType.Items.Remove("ELECTRONIC");
                                cboVerificationType.SelectedValue = objCheckOutLoad.EligibilityList[0].Eligibility_Type;
                            }

                            txtEligibilityVerificationDate.Text = objCheckOutLoad.EligibilityList[0].Eligibility_Verified_Date.ToString("dd-MMM-yyyy");
                            cboVerificationType.Text = objCheckOutLoad.EligibilityList[0].Eligibility_Type.ToString();


                            txtPolicyHolderID.Text = objCheckOutLoad.EligibilityList[0].Policy_Holder_ID;
                            txtGroupNumber.Text = objCheckOutLoad.EligibilityList[0].Group_Number;
                            if (objCheckOutLoad.EligibilityList[0].Effective_Date != DateTime.MinValue)
                            {
                                dtpEffectiveStartDate.Text = objCheckOutLoad.EligibilityList[0].Effective_Date.ToString("dd-MMM-yyyy");
                            }
                            if (objCheckOutLoad.EligibilityList[0].Termination_Date != DateTime.MinValue)
                            {
                                dtpTerminationDate.Text = objCheckOutLoad.EligibilityList[0].Termination_Date.ToString("dd-MMM-yyyy");
                            }


                            txtDemoNote.Text = objCheckOutLoad.EligibilityList[0].Demo_Note.ToString();
                            //break;


                            txtInsurancetype.Text = objCheckOutLoad.EligibilityList[0].Plan_Type.ToString();
                            txtPlanNumber.Text = objCheckOutLoad.EligibilityList[0].Plan_Number.ToString();
                            txtSubscriberName.Text = objCheckOutLoad.EligibilityList[0].Subscriber_Name.ToString();
                            txtPCPName.Text = objCheckOutLoad.EligibilityList[0].PCP_Name.ToString();
                            txtRelationship.Text = objCheckOutLoad.EligibilityList[0].Relationship_to_Subscriber.ToString();
                            txtPCP_NPI.Text = objCheckOutLoad.EligibilityList[0].PCP_NPI.ToString();
                            txtGroupNumber.Text = objCheckOutLoad.EligibilityList[0].Group_Number.ToString();
                            if (objCheckOutLoad.EligibilityList[0].PCP_Effective_Date != DateTime.MinValue && objCheckOutLoad.EligibilityList[0].PCP_Effective_Date.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                            {
                                dtpPCPEffectiveDate.Text = objCheckOutLoad.EligibilityList[0].PCP_Effective_Date.ToString("dd-MMM-yyyy");
                            }
                            txtGroupName.Text = objCheckOutLoad.EligibilityList[0].Group_Name.ToString();
                            txtIPAName.Text = objCheckOutLoad.EligibilityList[0].IPA_Name.ToString();

                            txtPCPVisitInCopay.Text = objCheckOutLoad.EligibilityList[0].PCP_Office_Visit_InNet_Copay.ToString();
                            txtPCPVisitInCoIns.Text = objCheckOutLoad.EligibilityList[0].PCP_Office_Visit_InNet_CoIns.ToString();
                            txtPCPVisitOutCopay.Text = objCheckOutLoad.EligibilityList[0].PCP_Office_Visit_OutNet_Copay.ToString();
                            txtPCPVisitOutCoIns.Text = objCheckOutLoad.EligibilityList[0].PCP_Office_Visit_OutNet_CoIns.ToString();

                            txtSpecialityVisitInCopay.Text = objCheckOutLoad.EligibilityList[0].Specialty_Office_Visit_InNet_Copay.ToString();
                            txtSpecialityVisitInCoIns.Text = objCheckOutLoad.EligibilityList[0].Specialty_Office_Visit_InNet_CoIns.ToString();
                            txtSpecialityVisitOutCopay.Text = objCheckOutLoad.EligibilityList[0].Specialty_Office_Visit_OutNet_Copay.ToString();
                            txtSpecialityVisitOutCoIns.Text = objCheckOutLoad.EligibilityList[0].Specialty_Office_Visit_OutNet_CoIns.ToString();

                            txtMedicationInCopay.Text = objCheckOutLoad.EligibilityList[0].Inj_Medication_InNet_Copay.ToString();
                            txtMedicationInCoIns.Text = objCheckOutLoad.EligibilityList[0].Inj_Medication_InNet_CoIns.ToString();
                            txtMedicationOutCopay.Text = objCheckOutLoad.EligibilityList[0].Inj_Medication_OutNet_Copay.ToString();
                            txtMedicationOutCoIns.Text = objCheckOutLoad.EligibilityList[0].Inj_Medication_OutNet_CoIns.ToString();

                            txtUrgentCareInCopay.Text = objCheckOutLoad.EligibilityList[0].Urgent_Care_InNet_Copay.ToString();
                            txtUrgentCareCoInIns.Text = objCheckOutLoad.EligibilityList[0].Urgent_Care_InNet_CoIns.ToString();
                            txtUrgentCareOutCopay.Text = objCheckOutLoad.EligibilityList[0].Urgent_Care_OutNet_Copay.ToString();
                            txtUrgentCareOutCoIns.Text = objCheckOutLoad.EligibilityList[0].Urgent_Care_OutNet_CoIns.ToString();

                            txtPCPInNetworkMessage.Text = objCheckOutLoad.EligibilityList[0].PCP_InNetwork_Copay_Message.ToString();
                            txtPCPOutNetworkMessage.Text = objCheckOutLoad.EligibilityList[0].PCP_OutNetwork_Copay_Message.ToString();

                            txtInDeductiblePlan.Text = objCheckOutLoad.EligibilityList[0].Ind_per_plan_InNet_Deductible.ToString();
                            txtInPockot.Text = objCheckOutLoad.EligibilityList[0].Ind_per_plan_InNet_Out_of_Pocket.ToString();
                            txtOutDeductiblePlan.Text = objCheckOutLoad.EligibilityList[0].Ind_per_plan_OutNet_Deductible.ToString();
                            txtOutPocket.Text = objCheckOutLoad.EligibilityList[0].Ind_per_plan_OutNet_Out_of_Pocket.ToString();

                            txtInDeductiblemet.Text = objCheckOutLoad.EligibilityList[0].Ind_met_InNet_Deductible.ToString();
                            txtInpocketmet.Text = objCheckOutLoad.EligibilityList[0].Ind_met_InNet_Out_of_Pocket.ToString();
                            txtOutDeductiblemet.Text = objCheckOutLoad.EligibilityList[0].Ind_met_OutNet_Deductible.ToString();
                            txtOutpocketmet.Text = objCheckOutLoad.EligibilityList[0].Ind_met_OutNet_Out_of_Pocket.ToString();

                            txtInFamilyDeductible.Text = objCheckOutLoad.EligibilityList[0].Family_per_plan_InNet_Deductible.ToString();
                            txtInFamilypocket.Text = objCheckOutLoad.EligibilityList[0].Family_per_plan_InNet_Out_of_Pocket.ToString();
                            txtOutFamilyDeductible.Text = objCheckOutLoad.EligibilityList[0].Family_met_OutNet_Deductible.ToString();
                            txtOutFamilypocket.Text = objCheckOutLoad.EligibilityList[0].Family_per_plan_OutNet_Out_of_Pocket.ToString();

                            txtInFamilyDeductiblemet.Text = objCheckOutLoad.EligibilityList[0].Family_met_InNet_Deductible.ToString();
                            txtInFamilymetpocket.Text = objCheckOutLoad.EligibilityList[0].Family_met_InNet_Out_of_Pocket.ToString();
                            txtOutFamilyDeductiblemet.Text = objCheckOutLoad.EligibilityList[0].Family_met_InNet_Out_of_Pocket.ToString();
                            txtOutFamilymetpocket.Text = objCheckOutLoad.EligibilityList[0].Family_met_OutNet_Out_of_Pocket.ToString();

                            txtDeductibleInNetworkMessage.Text = objCheckOutLoad.EligibilityList[0].Deductible_InNetwork_Message.ToString();
                            txtDeductibleOutNetworkMessage.Text = objCheckOutLoad.EligibilityList[0].Deductible_OutNetwork_Message.ToString();

                            //txtPCPCopay.Text = objCheckOutLoad.EligibilityList[0].PCP_Copay.ToString();
                            //txtSPCCopay.Text = objCheckOutLoad.EligibilityList[0].SPC_Copay.ToString();
                            //txtDeductible.Text = objCheckOutLoad.EligibilityList[0].Deductible_For_Plan.ToString();
                            //txtDeductibleMet.Text = objCheckOutLoad.EligibilityList[0].Deductible_Met_So_Far.ToString();
                            //txtCoInsurance.Text = objCheckOutLoad.EligibilityList[0].Coinsurance.ToString();
                            txtDemoNote.Text = objCheckOutLoad.EligibilityList[0].Comments.ToString(); // change
                            //break; 

                        }
                    }
                }


            }
        }
        public void Patientinformationdisable()
        {
            tablelayoutdisable(gbPatientInformation);
            tablelayoutdisable(Panel3);
        }
        public void tablelayoutdisable(Panel tablelayout)
        {

            if (hdnbShowPatInfo.Value != string.Empty)
                bShowPatInfo = Convert.ToBoolean(hdnbShowPatInfo.Value);
            for (int i = 0; i < tablelayout.Controls.Count; i++)
            {
                if (tablelayout.Controls[i].GetType().ToString().Contains("RadMaskedTextBox"))
                {
                    RadMaskedTextBox MskTxtBox = (RadMaskedTextBox)tablelayout.Controls[i];
                    //Cap - 669
                    //if (MskTxtBox.ID == "msktxtZipcode")
                    //{
                    //    MaskedTextBoxColorChange(MskTxtBox, false);
                    //}
                    if (MskTxtBox.ID == "dtpPatientDOB")
                    {
                        MaskedTextBoxColorChange(MskTxtBox, false);
                    }
                    else if (MskTxtBox.ID == "dtpEffectiveStartDate")
                    {
                        MaskedTextBoxColorChange(MskTxtBox, false);
                    }
                    else if (MskTxtBox.ID == "dtpCheckDate")
                    {
                        MaskedTextBoxColorChange(MskTxtBox, false);
                    }
                    else if (MskTxtBox.ID == "dtpTerminationDate")
                    {
                        MaskedTextBoxColorChange(MskTxtBox, false);
                    }
                    else if (MskTxtBox.ID == "dtpPCPEffectiveDate")
                    {
                        MaskedTextBoxColorChange(MskTxtBox, false);
                    }
                    if (bShowPatInfo == true)
                        MaskedTextBoxColorChange(MskTxtBox, false);
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("TextBox"))
                {


                    TextBox txtBox = (TextBox)tablelayout.Controls[i];
                    if (txtBox.ID != "txtClaimMailingAddress" || txtBox.ID != "txtClaimCity" || txtBox.ID != "msktxtZipcode" || txtBox.ID != "txtMail" || txtBox.ID != "txtClaimCity2")
                    {
                        TextBoxColorChange(txtBox, false);
                    }
                    if (txtBox.ID == "txtEligibilityVerificationDate")
                    {
                        TextBoxColorChange(txtBox, true);
                    }
                    if (txtBox.ID != "txtMail" && txtBox.ID != "txtMedicalRecordNo" && txtBox.ID != "txtExternalAccNo" && txtBox.ID != "txtEligibilityVerificationDate")
                        TextBoxColorChange(txtBox, false);
                    else if (txtBox.ID == "txtMedicalRecordNo" || txtBox.ID == "txtExternalAccNo")
                        TextBoxColorChange(txtBox, true);
                    if (bShowPatInfo == true)
                        TextBoxColorChange(txtBox, false);
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("DropDownList"))
                {
                    DropDownList combobox = (DropDownList)tablelayout.Controls[i];
                    //Cap - 669
                   // if (combobox.ID != "cboPreferredConfidentialCoreespondenceMode")
                   if (combobox.ID != "cboPreferredConfidentialCoreespondenceMode" && combobox.ID != "cboHumanType")
                        ComboBoxColorChange(combobox, false);
                    if (bShowPatInfo == true)
                        ComboBoxColorChange(combobox, false);
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("DateTimePicker"))
                {
                    RadMaskedTextBox datetimepicker = (RadMaskedTextBox)tablelayout.Controls[i];
                    DateTimePickerColorChangeForWindows(datetimepicker, false);
                    if (bShowPatInfo == true)
                        DateTimePickerColorChangeForWindows(datetimepicker, false);
                }
            }
        }
        public void tablelayoutenable(Panel tablelayout)
        {
            for (int i = 0; i < tablelayout.Controls.Count; i++)
            {
                if (tablelayout.Controls[i].GetType().ToString().Contains("RadMaskedTextBox"))
                {
                    RadMaskedTextBox MskTxtBox = (RadMaskedTextBox)tablelayout.Controls[i];
                    MaskedTextBoxColorChange(MskTxtBox, true);
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("TextBox"))
                {
                    TextBox txtBox = (TextBox)tablelayout.Controls[i];
                    if (txtBox.ID != "txtClaimMailingAddress" && txtBox.ID != "txtClaimCity" && txtBox.ID != "msktxtZipcode" && txtBox.ID != "txtClaimState" && txtBox.ID != "txtClaimCity2")
                    {
                        TextBoxColorChange(txtBox, true);
                    }
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("DropDownList"))
                {
                    DropDownList combobox = (DropDownList)tablelayout.Controls[i];
                    ComboBoxColorChange(combobox, true);
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("DateTimePicker"))
                {
                    RadMaskedTextBox datetimepicker = (RadMaskedTextBox)tablelayout.Controls[i];
                    DateTimePickerColorChangeForWindows(datetimepicker, true);
                }
            }
        }
        public void ComboBoxColorChange(DropDownList combobox, Boolean bToNormal)
        {
            if (bToNormal == false)
            {
                combobox.Enabled = false;
                combobox.CssClass = "nonEditabletxtbox";
                //Cap - 669
                //combobox.Attributes.Add("readonly", "readonly");
                combobox.Attributes.Add("disabled", "disabled");

            }
            else
            {
                combobox.Enabled = true;
                combobox.CssClass = "Editabletxtbox";
                
                //Cap - 669
                // combobox.Attributes.Add("readonly", "");
                combobox.Attributes.Remove("disabled");
                combobox.Attributes.Add("enabled", "enabled");
            }
        }
        public void PolicyInformationClearall()
        {
            cboVerificationType.Text = string.Empty;
            txtPolicyHolderID.Text = string.Empty;
            txtGroupNumber.Text = string.Empty;

            dtpEffectiveStartDate.Text = string.Empty;
            dtpTerminationDate.Text = string.Empty;
            txtDemoNote.Text = string.Empty;
            txtInsurancetype.Text = string.Empty;
            txtPlanNumber.Text = string.Empty;
            txtSubscriberName.Text = string.Empty;
            txtPCPName.Text = string.Empty;
            txtOrganization.Text = string.Empty;
            txtErrorMessage.Text = string.Empty;
            txtPCP_NPI.Text = string.Empty;
            txtGroupNumber.Text = string.Empty;
            dtpPCPEffectiveDate.Text = string.Empty;
            txtGroupName.Text = string.Empty;
            txtIPAName.Text = string.Empty;
            txtPCPVisitInCopay.Text = string.Empty;
            txtPCPVisitInCoIns.Text = string.Empty;
            txtPCPVisitOutCopay.Text = string.Empty;
            txtPCPVisitOutCoIns.Text = string.Empty;
            txtSpecialityVisitInCopay.Text = string.Empty;
            txtSpecialityVisitInCoIns.Text = string.Empty;
            txtSpecialityVisitOutCopay.Text = string.Empty;
            txtSpecialityVisitOutCoIns.Text = string.Empty;
            txtMedicationInCopay.Text = string.Empty;
            txtMedicationInCoIns.Text = string.Empty;
            txtMedicationOutCopay.Text = string.Empty;
            txtMedicationOutCoIns.Text = string.Empty;
            txtUrgentCareInCopay.Text = string.Empty;
            txtUrgentCareCoInIns.Text = string.Empty;
            txtUrgentCareOutCopay.Text = string.Empty;
            txtUrgentCareOutCoIns.Text = string.Empty;
            txtPCPInNetworkMessage.Text = string.Empty;
            txtPCPOutNetworkMessage.Text = string.Empty;
            txtInDeductiblePlan.Text = string.Empty;
            txtInPockot.Text = string.Empty;
            txtOutDeductiblePlan.Text = string.Empty;
            txtOutPocket.Text = string.Empty;
            txtInDeductiblemet.Text = string.Empty;
            txtInpocketmet.Text = string.Empty;
            txtOutDeductiblemet.Text = string.Empty;
            txtOutpocketmet.Text = string.Empty;
            txtInFamilyDeductible.Text = string.Empty;
            txtInFamilypocket.Text = string.Empty;
            txtOutFamilyDeductible.Text = string.Empty;
            txtOutFamilypocket.Text = string.Empty;
            txtInFamilyDeductiblemet.Text = string.Empty;
            txtInFamilymetpocket.Text = string.Empty;
            txtOutFamilyDeductiblemet.Text = string.Empty;
            txtOutFamilymetpocket.Text = string.Empty;
            txtDeductibleInNetworkMessage.Text = string.Empty;
            txtDeductibleOutNetworkMessage.Text = string.Empty;
        }
        public void TextBoxColorChange(TextBox txtbox, Boolean bToNormal)
        {
            if (bToNormal == false)
            {
                txtbox.ReadOnly = true;
                txtbox.CssClass = "nonEditabletxtbox";

            }
            else
            {
                txtbox.ReadOnly = false;
                txtbox.CssClass = "Editabletxtbox";

            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {

        }
        private void LoadPaymentInfoGrid(IList<VisitPaymentDTO> VisitPaymentDTO)
        {
            grdPaymentInformation.DataSource = null;
            grdPaymentInformation.DataBind();
            DataTable dtInsuranace = new DataTable();
            dtInsuranace.Columns.Add("MethodofPayment", typeof(string));
            dtInsuranace.Columns.Add("CheckCardNo", typeof(string));
            dtInsuranace.Columns.Add("AuthNo", typeof(string));
            dtInsuranace.Columns.Add("PastDue", typeof(string));
            dtInsuranace.Columns.Add("PatientPayment", typeof(string));
            dtInsuranace.Columns.Add("RecOnAcc", typeof(string));
            dtInsuranace.Columns.Add("RefundAmount", typeof(string));
            dtInsuranace.Columns.Add("CheckDate", typeof(string));
            dtInsuranace.Columns.Add("PaymentNotes", typeof(string));
            dtInsuranace.Columns.Add("VisitID", typeof(string));
            dtInsuranace.Columns.Add("PPHeaderID", typeof(string));
            dtInsuranace.Columns.Add("PPLineID", typeof(string));
            dtInsuranace.Columns.Add("CheckID", typeof(string));
            dtInsuranace.Columns.Add("relationship", typeof(string));
            dtInsuranace.Columns.Add("Amtpaidby", typeof(string));
            dtInsuranace.Columns.Add("receiptdate", typeof(string));
            dtInsuranace.Columns.Add("PaymentNote", typeof(string));
            dtInsuranace.Columns.Add("TransactionDate&Time", typeof(string));


            decimal PaymentAmount = 0;
            decimal RefundAmount = 0;
            decimal RecOnAcc = 0;
            if (VisitPaymentDTO != null && VisitPaymentDTO.Count > 0)
            {
                for (int i = 0; i < VisitPaymentDTO.Count; i++)
                {
                    DataRow dr = dtInsuranace.NewRow();
                    dr["MethodofPayment"] = VisitPaymentDTO[i].Method_of_Payment;
                    dr["CheckCardNo"] = VisitPaymentDTO[i].Check_Card_No;
                    dr["AuthNo"] = VisitPaymentDTO[i].Auth_No;
                    dr["PatientPayment"] = VisitPaymentDTO[i].Patient_Payment;
                    PaymentAmount = PaymentAmount + VisitPaymentDTO[i].Patient_Payment;
                    dr["RefundAmount"] = VisitPaymentDTO[i].Refund_Amount;
                    RefundAmount = RefundAmount + VisitPaymentDTO[i].Refund_Amount;
                    dr["RecOnAcc"] = VisitPaymentDTO[i].Rec_On_Acc;
                    RecOnAcc = RecOnAcc + VisitPaymentDTO[i].Rec_On_Acc;
                    dr["PastDue"] = hdnPastDue.Value;  //txtPastDue.Text;
                    if (VisitPaymentDTO[i].Check_Date != DateTime.MinValue)
                    {
                        dr["CheckDate"] = VisitPaymentDTO[i].Check_Date.ToString("dd-MMM-yyyy");
                    }
                    dr["PaymentNotes"] = VisitPaymentDTO[i].Payment_Note;
                    dr["VisitID"] = VisitPaymentDTO[i].Visit_Payment_Id;
                    dr["PPHeaderID"] = VisitPaymentDTO[i].PP_Header_Id;
                    dr["PPLineID"] = VisitPaymentDTO[i].PP_Line_Item_Id;
                    dr["CheckID"] = VisitPaymentDTO[i].Check_Table_Int_Id;
                    dr["relationship"] = VisitPaymentDTO[i].Relationship;
                    dr["Amtpaidby"] = VisitPaymentDTO[i].Amount_Paid_By;
                    if (VisitPaymentDTO[i].Created_Date_and_Time != DateTime.MinValue)
                    {
                        dr["receiptdate"] = VisitPaymentDTO[i].Created_Date_and_Time.ToString("dd-MMM-yyyy");
                    }
                    dr["PaymentNote"] = VisitPaymentDTO[i].Payment_Note;
                    if (VisitPaymentDTO[i].Modified_Date_and_Time != DateTime.MinValue)
                    {
                        dr["TransactionDate&Time"] = UtilityManager.ConvertToLocal(VisitPaymentDTO[i].Modified_Date_and_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    }
                    else
                    {
                        dr["TransactionDate&Time"] = UtilityManager.ConvertToLocal(VisitPaymentDTO[i].Created_Date_and_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    }

                    dtInsuranace.Rows.Add(dr);
                }
                grdPaymentInformation.DataSource = dtInsuranace;
                grdPaymentInformation.DataBind();


                if ((ClientSession.UserRole.ToUpper() == "FRONT OFFICE" && hdnEncStatus.Value == "MA_PROCESS") || (ScreenMode == "PATIENT SUMMARY"))
                {
                    for (int iNumber = 0; iNumber < grdPaymentInformation.Rows.Count; iNumber++)
                    {
                        ImageButton lnkedit = (ImageButton)grdPaymentInformation.Rows[iNumber].FindControl("EditGridRow");
                        lnkedit.Enabled = false;
                        lnkedit.ImageUrl = "~/Resources/edit disabled.png";
                        ImageButton lnkDel = (ImageButton)grdPaymentInformation.Rows[iNumber].FindControl("DeleteGridRow");
                        lnkDel.Enabled = false;
                        lnkDel.ImageUrl = "~/Resources/close_disabled.png";

                    }
                }
                txtTotalAmount.Text = Convert.ToString((PaymentAmount + RecOnAcc) - RefundAmount);
                hdnTotalPayment.Value = Convert.ToString((PaymentAmount + RecOnAcc) - RefundAmount);
            }
            else
            {
                DataRow dr = dtInsuranace.NewRow();
                dtInsuranace.Rows.Add(dr);
                grdPaymentInformation.DataSource = dtInsuranace;
                grdPaymentInformation.DataBind();
                grdPaymentInformation.Rows[0].Visible = false;
            }


        }

        public void LoadEligibilityVerification()
        {

            if (grdExistingPolicies.Rows.Count != 0)
            {
                if (grdExistingPolicies.Rows[0].Cells[1]!=null&&grdExistingPolicies.Rows[0].Cells[1].Text != "&nbsp;")
                {
                    txtPolicyHolderID.Text = grdExistingPolicies.Rows[0].Cells[1].Text;
                }

                if (grdExistingPolicies.Rows[0].Cells[19] != null && grdExistingPolicies.Rows[0].Cells[19].Text == "ELECTRONIC")
                {
                    cboVerificationType.Items.Add("ELECTRONIC");
                    cboVerificationType.SelectedValue = "ELECTRONIC";
                }
                else
                {
                    if (grdExistingPolicies.Rows[0].Cells[19] != null && grdExistingPolicies.Rows[0].Cells[19].Text != "&nbsp;")
                        cboVerificationType.SelectedValue = grdExistingPolicies.Rows[0].Cells[19].Text;
                }

                if (grdExistingPolicies.Rows[0].Cells[4] != null && grdExistingPolicies.Rows[0].Cells[4].Text != "&nbsp;")
                {
                    txtGroupNumber.Text = grdExistingPolicies.Rows[0].Cells[4].Text;
                }
                if (grdExistingPolicies.Rows[0].Cells[9] != null && grdExistingPolicies.Rows[0].Cells[9].Text != "&nbsp;")
                {
                    if (grdExistingPolicies.Rows[0].Cells[9].Text == "01-Jan-0001")
                    {
                        dtpEffectiveStartDate.Text = "";
                    }
                    else
                    {
                        dtpEffectiveStartDate.Text = grdExistingPolicies.Rows[0].Cells[9].Text;
                    }
                }
                else if (grdExistingPolicies.Rows[0].Cells[9] != null && grdExistingPolicies.Rows[0].Cells[9].Text == "&nbsp;")
                {
                    dtpEffectiveStartDate.Text = "";
                }
                if (grdExistingPolicies.Rows[0].Cells[10] != null && grdExistingPolicies.Rows[0].Cells[10].Text != "&nbsp;")
                {
                    if (grdExistingPolicies.Rows[0].Cells[10].Text == "01-Jan-0001")
                    {
                        dtpTerminationDate.Text = "";
                    }
                    else
                    {
                        dtpTerminationDate.Text = grdExistingPolicies.Rows[0].Cells[10].Text;
                    }
                }
                else if (grdExistingPolicies.Rows[0].Cells[10] != null && grdExistingPolicies.Rows[0].Cells[10].Text == "&nbsp;")
                {
                    dtpTerminationDate.Text = "";
                }
                if (grdExistingPolicies.Rows[0].Cells[11]!=null)
                    ddlPayerName.SelectedValue = grdExistingPolicies.Rows[0].Cells[11].Text;
                if (grdExistingPolicies.Rows[0].Cells[2] != null)
                    ddlPlanName.SelectedValue = grdExistingPolicies.Rows[0].Cells[2].Text;
                // txtClaimMailingName.Text = ddlPayerName.SelectedValue;

                if (Session["EligibilityList"] != null)
                {
                    IList<Eligibility_Verification> lstEligibilityList = (IList<Eligibility_Verification>)Session["EligibilityList"];

                    if (grdExistingPolicies.Rows[0].Cells[2].Text != "&nbsp;" && grdExistingPolicies.Rows[0].Cells[2].Text != null)
                    {
                        var EligibilityList = (from c in lstEligibilityList where c.Insurance_Plan_ID == Convert.ToUInt64(grdExistingPolicies.Rows[0].Cells[2].Text) && c.Policy_Holder_ID == grdExistingPolicies.Rows[0].Cells[1].Text select c).OrderByDescending(a => a.Created_Date_And_Time);
                        if (EligibilityList.ToList<Eligibility_Verification>().Count() > 0)
                        {
                            if (txtEligibilityVerificationDate.Text == string.Empty)
                            {
                                if (EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_From_Date.ToString("dd-MMM-yyyy") == EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_To_Date.ToString("dd-MMM-yyyy"))
                                {
                                    txtEligibilityVerificationDate.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_From_Date.ToString("dd-MMM-yyyy");

                                }
                                else
                                {
                                    txtEligibilityVerificationDate.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_From_Date.ToString("dd-MMM-yyyy") + " to " + EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_To_Date.ToString("dd-MMM-yyyy");

                                }
                            }

                            if (txtEligibilityVerificationDate.Text == "01-Jan-0001")
                            {
                                txtEligibilityVerificationDate.Text = string.Empty;
                            }


                            //if (EligibilityList.ToList<Eligibility_Verification>()[0].Response_Message != null && EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status != null)
                            //{
                            //    txtErrorMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Response_Message.ToString() + " - " + EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString();

                            //}
                            if (EligibilityList.ToList<Eligibility_Verification>().Count > 0)
                            {
                                if (EligibilityList.ToList<Eligibility_Verification>()[0].Response_Message != null && EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status != null)
                                    {
                                        string status = "";

                                        if (EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString() != "" && !EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString().ToUpper().Contains("FAIL"))
                                        {
                                            status = EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString();
                                        }
                                        if (EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString() != "")
                                        {
                                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Response_Message.ToString() != "")
                                                status = EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString() + " - " + EligibilityList.ToList<Eligibility_Verification>()[0].Response_Message.ToString();
                                            else
                                                status = EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString();

                                        }

                                        else if (EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString().ToUpper().Contains("CONTACT SUPPORT"))
                                        {
                                            status = "Error";
                                        }

                                        else if (EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Check_Mode.ToString().ToUpper().Trim() != "" && EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Check_Mode.ToString().ToUpper() == "MANUAL")
                                        {
                                            status = "EV-PERFORMED MANUALLY";
                                        }
                                        else
                                        {
                                            status = "EV-NOT PERFORMED";
                                        }

                                        txtErrorMessage.Text = status;

                                    }
                               
                            }
                            else
                            {
                                txtErrorMessage.Text = "EV-NOT PERFORMED";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Comments != null)
                            {
                                txtDemoNote.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Comments.ToString();
                            }

                            //new Columns add

                            txtInsurancetype.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Plan_Type.ToString();
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Plan_Number != null)
                            {
                                txtPlanNumber.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Plan_Number.ToString();
                            }
                            else
                            {
                                txtPlanNumber.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Organization != null)
                            {
                                txtOrganization.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Organization.ToString();
                            }
                            else
                            {
                                txtOrganization.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Subscriber_Name != null)
                            {
                                txtSubscriberName.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Subscriber_Name.ToString();
                            }
                            else
                            {
                                txtSubscriberName.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Name != null)
                            {
                                txtPCPName.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Name.ToString();
                            }
                            else
                            {
                                txtPCPName.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Relationship_to_Subscriber != null)
                            {
                                txtRelationship.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Relationship_to_Subscriber.ToString();
                            }
                            else
                            {
                                txtRelationship.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_NPI != null)
                            {
                                txtPCP_NPI.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_NPI.ToString();
                            }
                            else
                            {
                                txtPCP_NPI.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Group_Number != null)
                            {
                                txtGroupNumber.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Group_Number.ToString();
                            }
                            else
                            {
                                txtGroupNumber.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Effective_Date.ToString("dd-MMM-yyyy") != null && EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Effective_Date.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                            {
                                dtpPCPEffectiveDate.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Effective_Date.ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                dtpPCPEffectiveDate.Text = "";
                            }
                            //if (dtpPCPEffectiveDate.Text == "01-Jan-0001")
                            //{
                            //    dtpPCPEffectiveDate.Text = string.Empty;
                            //}

                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Group_Name != null)
                            {
                                txtGroupName.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Group_Name.ToString();
                            }
                            else
                            {
                                txtGroupName.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].IPA_Name != null)
                            {
                                txtIPAName.Text = EligibilityList.ToList<Eligibility_Verification>()[0].IPA_Name.ToString();
                            }
                            else
                            {
                                txtIPAName.Text = "";
                            }


                            txtPCPVisitInCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Office_Visit_InNet_Copay.ToString();
                            txtPCPVisitInCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Office_Visit_InNet_CoIns.ToString();
                            txtPCPVisitOutCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Office_Visit_OutNet_Copay.ToString();
                            txtPCPVisitOutCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Office_Visit_OutNet_CoIns.ToString();

                            txtSpecialityVisitInCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Specialty_Office_Visit_InNet_Copay.ToString();
                            txtSpecialityVisitInCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Specialty_Office_Visit_InNet_CoIns.ToString();
                            txtSpecialityVisitOutCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Specialty_Office_Visit_OutNet_Copay.ToString();
                            txtSpecialityVisitOutCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Specialty_Office_Visit_OutNet_CoIns.ToString();

                            txtMedicationInCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Inj_Medication_InNet_Copay.ToString();
                            txtMedicationInCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Inj_Medication_InNet_CoIns.ToString();
                            txtMedicationOutCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Inj_Medication_OutNet_Copay.ToString();
                            txtMedicationOutCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Inj_Medication_OutNet_CoIns.ToString();

                            txtUrgentCareInCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Urgent_Care_InNet_Copay.ToString();
                            txtUrgentCareCoInIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Urgent_Care_InNet_CoIns.ToString();
                            txtUrgentCareOutCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Urgent_Care_OutNet_Copay.ToString();
                            txtUrgentCareOutCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Urgent_Care_OutNet_CoIns.ToString();

                            if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_InNetwork_Copay_Message != null)
                            {
                                txtPCPInNetworkMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_InNetwork_Copay_Message.ToString();
                            }
                            else
                            {
                                txtPCPInNetworkMessage.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_OutNetwork_Copay_Message != null)
                            {
                                txtPCPOutNetworkMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_OutNetwork_Copay_Message.ToString();
                            }
                            else
                            {
                                txtPCPOutNetworkMessage.Text = "";
                            }

                            txtInDeductiblePlan.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_per_plan_InNet_Deductible.ToString();
                            txtInPockot.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_per_plan_InNet_Out_of_Pocket.ToString();
                            txtOutDeductiblePlan.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_per_plan_OutNet_Deductible.ToString();
                            txtOutPocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_per_plan_OutNet_Out_of_Pocket.ToString();

                            txtInDeductiblemet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_met_InNet_Deductible.ToString();
                            txtInpocketmet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_met_InNet_Out_of_Pocket.ToString();
                            txtOutDeductiblemet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_met_OutNet_Deductible.ToString();
                            txtOutpocketmet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_met_OutNet_Out_of_Pocket.ToString();

                            txtInFamilyDeductible.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_per_plan_InNet_Deductible.ToString();
                            txtInFamilypocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_per_plan_InNet_Out_of_Pocket.ToString();
                            txtOutFamilyDeductible.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_per_plan_OutNet_Deductible.ToString();
                            txtOutFamilypocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_per_plan_OutNet_Out_of_Pocket.ToString();

                            txtInFamilyDeductiblemet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_met_InNet_Deductible.ToString();
                            txtInFamilymetpocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_met_InNet_Out_of_Pocket.ToString();
                            txtOutFamilyDeductiblemet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_met_OutNet_Deductible.ToString();
                            txtOutFamilymetpocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_met_OutNet_Out_of_Pocket.ToString();


                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Deductible_InNetwork_Message != null)
                            {
                                txtDeductibleInNetworkMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Deductible_InNetwork_Message.ToString();
                            }
                            else
                            {
                                txtDeductibleInNetworkMessage.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Deductible_OutNetwork_Message != null)
                            {
                                txtDeductibleOutNetworkMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Deductible_OutNetwork_Message.ToString();
                            }
                            else
                            {
                                txtDeductibleOutNetworkMessage.Text = "";
                            }

                        }
                        else
                        {

                            txtPCPVisitInCopay.Text = string.Empty;
                            txtPCPVisitInCoIns.Text =
                            txtPCPVisitOutCopay.Text = string.Empty;
                            txtPCPVisitOutCoIns.Text = string.Empty;
                            txtSpecialityVisitInCopay.Text = string.Empty;
                            txtSpecialityVisitInCoIns.Text = string.Empty;
                            txtSpecialityVisitOutCopay.Text = string.Empty;
                            txtSpecialityVisitOutCoIns.Text = string.Empty;
                            txtMedicationInCopay.Text = string.Empty;
                            txtMedicationInCoIns.Text = string.Empty;
                            txtMedicationOutCopay.Text = string.Empty;
                            txtMedicationOutCoIns.Text = string.Empty;
                            txtUrgentCareInCopay.Text = string.Empty;
                            txtUrgentCareCoInIns.Text = string.Empty;
                            txtUrgentCareOutCopay.Text = string.Empty;
                            txtUrgentCareOutCoIns.Text = string.Empty;
                            txtInDeductiblePlan.Text = string.Empty;
                            txtInPockot.Text = string.Empty;
                            txtOutDeductiblePlan.Text = string.Empty;
                            txtOutPocket.Text = string.Empty;
                            txtInDeductiblemet.Text = string.Empty;
                            txtInpocketmet.Text = string.Empty;
                            txtOutDeductiblemet.Text = string.Empty;
                            txtOutpocketmet.Text = string.Empty;
                            txtInFamilyDeductible.Text = string.Empty;
                            txtInFamilypocket.Text = string.Empty;
                            txtOutFamilyDeductible.Text = string.Empty;
                            txtOutFamilypocket.Text = string.Empty;
                            txtInFamilyDeductiblemet.Text = string.Empty;
                            txtInFamilymetpocket.Text = string.Empty;
                            txtOutFamilyDeductiblemet.Text = string.Empty;
                            txtOutFamilymetpocket.Text = string.Empty;
                            txtPlanNumber.Text = string.Empty;
                            txtSubscriberName.Text = string.Empty;
                            txtRelationship.Text = string.Empty;
                            txtGroupName.Text = string.Empty;
                            txtPCPName.Text = string.Empty;
                            txtPCP_NPI.Text = string.Empty;
                            dtpPCPEffectiveDate.Text = string.Empty;
                            txtIPAName.Text = string.Empty;
                            txtPCPInNetworkMessage.Text = string.Empty;
                            txtPCPOutNetworkMessage.Text = string.Empty;
                            txtDeductibleInNetworkMessage.Text = string.Empty;
                            txtDeductibleOutNetworkMessage.Text = string.Empty;
                            txtInsurancetype.Text = string.Empty;
                            txtOrganization.Text = string.Empty;



                            if (hdnPCTime.Value != string.Empty && chkEligibilityVerified.Checked == true)
                            {
                                DateTime dt = Convert.ToDateTime(hdnLocalTime.Value);
                                txtEligibilityVerificationDate.Text = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy");// Convert.ToDateTime(hdnPCTime.Value).ToString("dd-MMM-yyyy");
                            }
                        }
                    }
                }

            }
            IList<InsurancePlan> InsList = new List<InsurancePlan>();
            InsurancePlanManager obj = new InsurancePlanManager();
            if (grdExistingPolicies.Rows.Count > 0 && grdExistingPolicies.Rows[0].Cells[2].Text != null)
            {
                IList<InsurancePlan> patInsList = (IList<InsurancePlan>)Session["PatInsuredList"];
                var insplan = from ip in patInsList where ip.Id == Convert.ToUInt64(grdExistingPolicies.Rows[0].Cells[2].Text) select ip;
                InsList = insplan.ToList<InsurancePlan>();

                InsurancePlan objInsPlan = null;
                if (InsList != null && InsList.Count > 0)
                {
                    objInsPlan = InsList[0];
                    txtClaimCity.Text = objInsPlan.Claim_City;
                    txtClaimCity2.Text = objInsPlan.Claim_City;
                    txtClaimState.Text = objInsPlan.Claim_State;
                    //msktxtZipcode.Text = objInsPlan.Claim_ZipCode;
                    txtStreet.Text = objInsPlan.Claim_Address;
                    ddlPlanName.SelectedValue = objInsPlan.Id.ToString();
                }
            }
        }

        public void DateTimePickerColorChange(TextBox datetimepicker, Boolean bToNormal)
        {
            if (bToNormal == false)
            {

                datetimepicker.Enabled = false;
            }
            else
            {
                datetimepicker.Enabled = true;
            }
        }

        protected void ddlPayerName_SelectedIndexChanged(object sender, EventArgs e) /*change 2 txtbx*/
        {

            if (ddlPayerName.SelectedIndex != 0)
            {
                bClose = true;
                //chkShowAllEVPlan.Checked = false;
                if (ddlPayerName.SelectedItem.Text == "OTHER")
                {
                    ddlPlanName.Items.Clear();
                    ddlPlanName.Items.Add("OTHER");
                    txtClaimMailingName.Text = string.Empty;
                    txtClaimCity.Text = string.Empty;
                    txtClaimCity2.Text = string.Empty;
                    txtClaimState.Text = string.Empty;
                    msktxtZipcode.Text = string.Empty;
                }
                else
                {


                    //inslist = ((InsMngr.GetInsurancebyCarrierID(Convert.ToUInt64(ddlPayerName.Items[ddlPayerName.SelectedIndex].Value))).OrderBy(s => s.Ins_Plan_Name)).ToList<InsurancePlan>();
                    //ddlPlanName.Items.Clear();
                    //ddlPlanName.Items.Add("");
                    //if (inslist != null)
                    //{
                    //    for (int i = 0; i < inslist.Count; i++)
                    //    {
                    //        ListItem ddlItem = new ListItem();
                    //        ddlItem.Text = inslist[i].Ins_Plan_Name;
                    //        ddlItem.Value = inslist[i].Id.ToString();
                    //        ddlPlanName.Items.Add(ddlItem);
                    //        txtClaimMailingName.Text = ddlItem.Text;

                    //        txtStreet.Text = inslist[i].Claim_Address;
                    //        txtClaimCity.Text = inslist[i].Claim_City;
                    //        txtClaimCity2.Text = inslist[i].Claim_City;
                    //        //msktxtClaimZipCode.Text = inslist[i].Claim_ZipCode;
                    //        //txtClaimState.Text = inslist[i].Claim_State;


                    //    }

                    if (chkShowAllEVPlan.Checked == true)
                    {

                        ddlPlanName.Items.Clear();
                        ddlPlanName.Items.Add("OTHER");

                        inslist = ((InsMngr.GetInsurancebyCarrierID(Convert.ToUInt64(ddlPayerName.Items[ddlPayerName.SelectedIndex].Value))).OrderBy(s => s.Ins_Plan_Name)).ToList<InsurancePlan>();

                        ddlPlanName.Items.Clear();
                        ddlPlanName.Items.Add("");

                        if (inslist != null)
                        {
                            for (int i = 0; i < inslist.Count; i++)
                            {
                                ListItem ddlItem = new ListItem();
                                ddlItem.Text = inslist[i].Ins_Plan_Name;
                                ddlItem.Value = inslist[i].Id.ToString();
                                ddlPlanName.Items.Add(ddlItem);

                            }
                            for (int i = 0; i < ddlPlanName.Items.Count; i++)
                            {
                                if (grdExistingPolicies.SelectedRow != null && ddlPlanName.Items[i].Value == grdExistingPolicies.SelectedRow.Cells[2].Text)
                                {
                                    ddlPlanName.SelectedIndex = i;
                                }
                            }
                        }
                    }

                    else
                    {
                        ddlPlanName.Items.Clear();
                        if (grdExistingPolicies.SelectedRow != null)
                        {

                            ListItem ddlItem = new ListItem();
                            ddlItem.Text = grdExistingPolicies.SelectedRow.Cells[3].Text;
                            ddlItem.Value = grdExistingPolicies.SelectedRow.Cells[2].Text;
                            ddlPlanName.Items.Add(ddlItem);
                        }
                    }

                    //ddlPlanName.Items.Clear();
                    //for (int i = 0; i < grdExistingPolicies.Rows.Count; i++)
                    //{
                    //    if (grdExistingPolicies.Rows[i].Cells[11].Text.Trim() == ddlPayerName.SelectedValue.ToString().Trim())
                    //    {
                    //        ListItem ddlItem = new ListItem();
                    //        ddlItem.Text = grdExistingPolicies.Rows[i].Cells[3].Text;
                    //        ddlItem.Value = grdExistingPolicies.Rows[i].Cells[2].Text;
                    //        ddlPlanName.Items.Add(ddlItem);
                    //    }

                    //}

                }
                btnSave.Enabled = true;
                btnSave.CssClass = "aspresizedgreenbutton";
            }
            else
            {
                ddlPlanName.Items.Clear();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }

        protected void ddlAuthourPayerName_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlauthPayer.SelectedIndex != 0)
            {
                bClose = true;
                //chkShowAllAuthPlan.Checked = false;
                if (ddlauthPayer.SelectedItem.Text == "OTHER")
                {
                    ddlauthinsplan.Items.Clear();
                    ddlauthinsplan.Items.Add("OTHER");
                    //txtauthnumber.Enabled = true;
                    //txtauthnumber.CssClass = "Editabletxtbox";
                }
                else
                {

                    if (chkShowAllAuthPlan.Checked == true)
                    {

                        ddlauthinsplan.Items.Clear();
                        ddlauthinsplan.Items.Add("OTHER");

                        inslist = ((InsMngr.GetInsurancebyCarrierID(Convert.ToUInt64(ddlauthPayer.Items[ddlauthPayer.SelectedIndex].Value))).OrderBy(s => s.Ins_Plan_Name)).ToList<InsurancePlan>();

                        ddlauthinsplan.Items.Clear();
                        ddlauthinsplan.Items.Add("");

                        if (inslist != null)
                        {
                            for (int i = 0; i < inslist.Count; i++)
                            {
                                ListItem ddlItem = new ListItem();
                                ddlItem.Text = inslist[i].Ins_Plan_Name;
                                ddlItem.Value = inslist[i].Id.ToString();
                                ddlauthinsplan.Items.Add(ddlItem);

                            }
                            for (int i = 0; i < ddlauthinsplan.Items.Count; i++)
                            {
                                if (grdExistingPolicies.SelectedRow != null && ddlauthinsplan.Items[i].Value == grdExistingPolicies.SelectedRow.Cells[2].Text)
                                {
                                    ddlauthinsplan.SelectedIndex = i;
                                }
                            }
                        }
                    }
                    else
                    {
                        ddlauthinsplan.Items.Clear();
                        if (grdExistingPolicies.SelectedRow != null)
                        {

                            ListItem ddlItem = new ListItem();
                            ddlItem.Text = grdExistingPolicies.SelectedRow.Cells[3].Text;
                            ddlItem.Value = grdExistingPolicies.SelectedRow.Cells[2].Text;
                            ddlauthinsplan.Items.Add(ddlItem);
                        }
                    }


                    //inslist = ((InsMngr.GetInsurancebyCarrierID(Convert.ToUInt64(ddlauthPayer.Items[ddlauthPayer.SelectedIndex].Value))).OrderBy(s => s.Ins_Plan_Name)).ToList<InsurancePlan>();


                    //ddlauthinsplan.Items.Add("");

                    //if (inslist != null)
                    //{
                    //    for (int i = 0; i < inslist.Count; i++)
                    //    {
                    //        ListItem ddlItem = new ListItem();
                    //        ddlItem.Text = inslist[i].Ins_Plan_Name;
                    //        ddlItem.Value = inslist[i].Id.ToString();
                    //        ddlauthinsplan.Items.Add(ddlItem);
                    //        //txtStreet.Text = inslist[i].Claim_Address;
                    //        //txtClaimCity.Text = inslist[i].Claim_City;
                    //        //txtClaimCity2.Text = inslist[i].Claim_City;
                    //        //// msktxtClaimZipCode.Text = inslist[i].Claim_ZipCode;
                    //        //txtClaimState.Text = inslist[i].Claim_State;
                    //    }

                    // ddlauthinsplan.SelectedIndex = 0;
                    //for (int i = 0; i < ddlauthinsplan.Items.Count; i++)
                    //{
                    //ddlauthinsplan.Items.Clear();
                    //for (int i = 0; i < grdExistingPolicies.Rows.Count; i++)
                    //{
                    //    if(grdExistingPolicies.Rows[i].Cells[11].Text.Trim()==ddlauthPayer.SelectedValue.ToString().Trim())
                    //    {
                    //        ListItem ddlItem = new ListItem();
                    //        ddlItem.Text =grdExistingPolicies.Rows[i].Cells[3].Text;
                    //        ddlItem.Value =grdExistingPolicies.Rows[i].Cells[2].Text;
                    //        ddlauthinsplan.Items.Add(ddlItem);
                    //    }

                    //}
                    //if (grdExistingPolicies.SelectedRow != null)
                    //{
                    //    //ddlPlanName.SelectedIndex = i;
                    //    // ddlauthinsplan.SelectedIndex = i;

                    //    ListItem ddlItem = new ListItem();
                    //    ddlItem.Text = grdExistingPolicies.SelectedRow.Cells[3].Text;
                    //    ddlItem.Value = grdExistingPolicies.SelectedRow.Cells[2].Text;
                    //    ddlauthinsplan.Items.Add(ddlItem);
                    //}
                    // }
                    //}
                }
                btnSave.Enabled = true;
                btnSave.CssClass = "aspresizedgreenbutton";
                if (ddlauthinsplan.SelectedItem != null && ddlauthinsplan.SelectedItem.Text != null && ddlauthinsplan.SelectedItem.Text != string.Empty)
                {
                    txtauthnumber.Enabled = true;
                    txtauthnumber.CssClass = "Editabletxtbox";

                }
                else
                {
                    txtauthnumber.Enabled = false;
                    txtauthnumber.CssClass = "nonEditabletxtbox";
                }
            }
            else
            {
                ddlauthinsplan.Items.Clear();
                txtauthnumber.Enabled = false;
                txtauthnumber.CssClass = "nonEditabletxtbox";
                txtauthnumber.Text = string.Empty;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }



        protected void ddlchange()
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);

            if (ddlauthPayer.SelectedIndex != 0)
            {
                bClose = true;
                if (ddlauthPayer.SelectedItem.Text == "OTHER")
                {
                    ddlauthinsplan.Items.Clear();
                    ddlauthinsplan.Items.Add("OTHER");

                }
                else
                {
                    inslist = ((InsMngr.GetInsurancebyCarrierID(Convert.ToUInt64(ddlauthPayer.Items[ddlauthPayer.SelectedIndex].Value))).OrderBy(s => s.Ins_Plan_Name)).ToList<InsurancePlan>();

                    ddlauthinsplan.Items.Clear();
                    ddlauthinsplan.Items.Add("");

                    if (inslist != null)
                    {
                        for (int i = 0; i < inslist.Count; i++)
                        {
                            ListItem ddlItem = new ListItem();
                            ddlItem.Text = inslist[i].Ins_Plan_Name;
                            ddlItem.Value = inslist[i].Id.ToString();
                            ddlauthinsplan.Items.Add(ddlItem);


                            txtStreet.Text = inslist[i].Claim_Address;
                            txtClaimCity.Text = inslist[i].Claim_City;
                            txtClaimCity2.Text = inslist[i].Claim_City;
                            //  msktxtClaimZipCode.Text = inslist[i].Claim_ZipCode;
                            txtClaimState.Text = inslist[i].Claim_State;


                        }

                        ddlauthinsplan.SelectedIndex = 0;
                    }

                }


                btnSave.Enabled = true;
                btnSave.CssClass = "aspresizedgreenbutton";
            }
            else
            {
                ddlauthinsplan.Items.Clear();
                txtauthnumber.Enabled = false;
                txtauthnumber.CssClass = "nonEditabletxtbox";
                txtauthnumber.Text = string.Empty;

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }
        public void checkin(bool bCheckInToMa)
        {

            //string FileName = "Human" + "_" + txtPatientAccountNo.Text + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

           

            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlTextReader XmlText1 = null;
            //    try
            //    {


            //        XmlDocument itemDoc = new XmlDocument();
            //        XmlText1 = new XmlTextReader(strXmlFilePath);
            //        itemDoc.Load(XmlText1);
            //        XmlText1.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        XmlText1.Close();
            //        UtilityManager.GenerateXML(txtPatientAccountNo.Text, "Human");
            //        goto ln;
            //    }
            //}


            
            if (cboMethodOfPayment.Text != string.Empty)
            {
                if (cboRelation.Text == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380051'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    btnAdd.Enabled = true;
                    return;
                }
                if (txtpaidBy.Text == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380052'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    btnAdd.Enabled = true;
                    return;
                }
                if ((txtPaymentAmount.Text == "" || txtPaymentAmount.Text == "0.00") && (txtRecOnAcc.Text == "" || txtRecOnAcc.Text == "0.00") && (txtPastDue.Text == "" || txtPastDue.Text == "0.00") && (txtRefundAmount.Text == "" || txtRefundAmount.Text == "0.00"))
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380059');", true);
                    btnAdd.Enabled = true;
                    return;
                }
            }

            if (txtPatientAccountNo.Text == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "alert('The selected appointment is moved to archive table. So, this transaction is not supported.'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                btnAdd.Enabled = false;
                return;
            }
            string YesNoMessage = hdnMessageType.Value;
            hdnMessageType.Value = string.Empty;
            if (hdnValidation.Value == "false")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

            System.Diagnostics.Stopwatch SaveTime = new System.Diagnostics.Stopwatch();
            SaveTime.Start();


            if (iEligibility == 1)
            {
                iEligibility = 0;
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

            if (hdnCarrierName.Value != string.Empty)
            {
                if (Session["CarrierList"] != null)
                {
                    IList<Carrier> objCarrier = (IList<Carrier>)Session["CarrierList"];
                    if (objCarrier.Count > 0)
                    {
                        IList<Carrier> CarrierLst = null;
                        CarrierLst = (from a in objCarrier where a.Carrier_Name == hdnCarrierName.Value select a).ToList<Carrier>();

                        if (CarrierLst != null && CarrierLst.Count > 0)
                        {
                            hdnCarrierId.Value = Convert.ToString(CarrierLst[0].Id);
                        }
                    }
                }
            }

            if (chkEligibilityVerified.Checked == true)//&& EligibilitySaveAndValidation() == false)
            {
                if (txtPolicyHolderID.Text != string.Empty)
                {
                    string sCarrierIDList = System.Configuration.ConfigurationSettings.AppSettings["MedicareCarrierIDList"];
                    string[] CarrierIDList = sCarrierIDList.Split(',');

                    if (CarrierIDList.Contains<String>(ddlPayerName.SelectedValue) == true)
                    {
                        string sResult = UtilityManager.ValidatePolicyHolderID(txtPolicyHolderID.Text);
                        if (sResult.StartsWith("Fail") == true)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380057','','" + sResult.Split('|')[1] + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            btnAdd.Enabled = true;
                            return;
                        }
                    }
                }
            }

            if (txtPatientLastName.ReadOnly == false)
            {

                if ((hdnScreenMode.Value.ToUpper() != "CHECKEDIN" && txtPatientAccountNo.Text == string.Empty) || (txtPatientAccountNo.Text == "0"))
                {
                    string sReturnOfCheckHumanDuplicate = CheckHumanDuplicate();
                    if (sReturnOfCheckHumanDuplicate != "")
                    {


                        if (sReturnOfCheckHumanDuplicate == "ReSave")
                            sReturnOfCheckHumanDuplicate = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}" + sReturnOfCheckHumanDuplicate, true);
                        return;
                    }

                    if (txtMail.Text != string.Empty && Convert.ToString(ViewState["email"]) != txtMail.Text && CheckEmailDuplicate() == false)
                    {

                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380040'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        hdnIsmailsend.Value = "";
                        txtMail.Focus();
                        chkOnlineAccess.Checked = false;
                        btnSave.Enabled = true;
                        btnSave.CssClass = "aspresizedgreenbutton";
                        txtMail.Text = string.Empty;
                        return;


                    }
                    else
                    {
                        Human objHuman = new Human();

                        objHuman.First_Name = txtPatientFirstName.Text.Trim();
                        objHuman.Last_Name = txtPatientLastName.Text.Trim();
                        objHuman.MI = txtPatientMI.Text.Trim();
                        if (cboPatientSex.Items.FindByValue(cboPatientSex.SelectedItem.Text.ToString().Trim()) != null && cboPatientSex.SelectedItem.Text != string.Empty)
                        {
                            objHuman.Sex = cboPatientSex.SelectedItem.Text;
                        }
                        objHuman.Legal_Org = ClientSession.LegalOrg;
                        objHuman.Encounter_Provider_ID = 0;
                        objHuman.Account_Status = "Active";
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objHuman.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        objHuman.Created_By = ClientSession.UserName;
                        objHuman.Patient_Date_Last_Billed = DateTime.MinValue;
                        objHuman.Guarantor_Birth_Date = DateTime.MinValue;
                        objHuman.Patient_UnApplied_Payments = 0;
                        objHuman.Emergency_BirthDate = DateTime.MinValue;
                        objHuman.Demo_Update_Time_Stamp = DateTime.MinValue;
                        objHuman.Batch_ID = 0;
                        objHuman.Past_Due = 0;
                        objHuman.Medical_Record_Number = txtMedicalRecordNo.Text;
                        objHuman.Patient_Account_External = txtExternalAccNo.Text;

                        if (bSendMailClick == true && chkOnlineAccess.Checked == true)
                        {
                            objHuman.Is_Mail_Sent = "Y";
                            if (hdnLocalTime.Value != string.Empty)
                            {
                                objHuman.Mail_Sent_Date = Convert.ToDateTime(hdnLocalTime.Value);
                            }
                        }
                        else
                        {
                            objHuman.Is_Mail_Sent = "N";
                        }
                        if (cboPreferredConfidentialCoreespondenceMode.SelectedItem != null)
                        {
                            objHuman.Preferred_Confidential_Correspodence_Mode = cboPreferredConfidentialCoreespondenceMode.SelectedItem.Text;
                        }
                        objHuman.EMail = txtMail.Text;
                        if (msktxtSSN.TextWithLiterals == "--")
                        {
                            objHuman.SSN = string.Empty;
                        }
                        else if (msktxtSSN.TextWithLiterals != "--")
                        {
                            objHuman.SSN = msktxtSSN.TextWithLiterals;
                        }
                        if (msktxtCellPhno.TextWithLiterals != "() -")
                        {
                            objHuman.Cell_Phone_Number = msktxtCellPhno.TextWithLiterals;
                        }
                        else if (msktxtCellPhno.TextWithLiterals == "() -")
                        {
                            objHuman.Cell_Phone_Number = string.Empty;
                        }
                        if (msktxtHomePhno.TextWithLiterals != "() -")
                        {
                            objHuman.Home_Phone_No = msktxtHomePhno.TextWithLiterals;
                        }
                        else if (msktxtHomePhno.TextWithLiterals == "() -")
                        {
                            objHuman.Home_Phone_No = string.Empty;
                        }

                        if (msktxtZipcode.TextWithLiterals != "-")
                        {
                            if (msktxtZipcode.TextWithLiterals.Length == 6 && msktxtZipcode.TextWithLiterals.Length < 10)
                            {
                                string[] Split = Convert.ToString(msktxtZipcode.TextWithLiterals).Split('-');
                                if (Split.Length == 2 && Split[1] == string.Empty)
                                {
                                    objHuman.ZipCode = Split[0].ToString();
                                }
                            }
                            else
                            {

                                objHuman.ZipCode = msktxtZipcode.TextWithLiterals;
                            }
                        }
                        else
                        {
                            objHuman.ZipCode = string.Empty;
                        }

                        objHuman.People_In_Collection = "N";
                        objHuman.Birth_Date = Convert.ToDateTime(dtpPatientDOB.Text);
                        objHuman.Human_Type = cboHumanType.Text;
                        objHuman.Suffix = cboPatientSuffix.SelectedItem.Text;

                        string sFTPPath = UploadPhoto();

                        if (sFTPPath != string.Empty)
                            objHuman.Photo_Path = sFTPPath;

                        ListToSaveHuman.Add(objHuman);

                    }
                }
            }
            FillQuickPatient objCheckOut = null;
            Human humanLoadRecord = null;
            ulong humanId = 0;
            if (txtPatientAccountNo.Text != string.Empty && txtPatientAccountNo.Text != "0")
            {
                humanId = Convert.ToUInt64(txtPatientAccountNo.Text);
            }

            objCheckOut = HumanMngr.LoadQuickPatient(Convert.ToUInt64(hdnEncounterID.Value), humanId);

            if (objCheckOut != null && objCheckOut.HumanObj != null)
            {
                humanLoadRecord = objCheckOut.HumanObj;
            }
            if (humanLoadRecord != null && humanId != 0)
            {
                if (txtMail.Text != string.Empty && hdnIsmailsend.Value != "true")
                {
                    if (Convert.ToString(ViewState["email"]) != txtMail.Text)
                    {
                        if (CheckEmailDuplicate() == false)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "QuickpatientCreate", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('380040');", true);
                            btnSendMail.Enabled = false;
                            txtMail.Focus();
                            hdnIsmailsend.Value = "";
                            chkOnlineAccess.Checked = false;
                            divLoading.Style.Add("display", "none");
                            txtMail.Text = string.Empty;
                            return;
                        }

                    }
                }
                Human objHumanRecord = new Human();
                if (objCheckOut != null && humanLoadRecord == null)
                {
                    objHumanRecord = objCheckOut.HumanObj;
                }
                else if (humanLoadRecord != null)
                {
                    objHumanRecord = humanLoadRecord;
                }
                objHumanRecord.First_Name = txtPatientFirstName.Text;
                objHumanRecord.Last_Name = txtPatientLastName.Text;
                objHumanRecord.MI = txtPatientMI.Text;
                if (cboPatientSex.Text.Trim() != string.Empty)
                    objHumanRecord.Sex = cboPatientSex.Text;
                objHumanRecord.Legal_Org = ClientSession.LegalOrg;
                objHumanRecord.Patient_Account_External = txtExternalAccNo.Text;
                objHumanRecord.Medical_Record_Number = txtMedicalRecordNo.Text;
                objHumanRecord.Birth_Date = Convert.ToDateTime(dtpPatientDOB.Text);

                if (msktxtSSN.TextWithLiterals == "--")
                {
                    objHumanRecord.SSN = string.Empty;
                }
                else if (msktxtSSN.TextWithLiterals != "--")
                {
                    objHumanRecord.SSN = msktxtSSN.TextWithLiterals;
                }
                if (msktxtCellPhno.TextWithLiterals != "() -")
                {
                    objHumanRecord.Cell_Phone_Number = msktxtCellPhno.TextWithLiterals;
                }
                else if (msktxtCellPhno.TextWithLiterals == "() -")
                {
                    objHumanRecord.Cell_Phone_Number = string.Empty;
                }
                if (msktxtHomePhno.TextWithLiterals != "() -")
                {
                    objHumanRecord.Home_Phone_No = msktxtHomePhno.TextWithLiterals;
                }
                else if (msktxtHomePhno.TextWithLiterals == "() -")
                {
                    objHumanRecord.Home_Phone_No = string.Empty;
                }
                if (chkOnlineAccess.Checked == true)
                {
                    objHumanRecord.Is_Mail_Sent = "Y";
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objHumanRecord.Mail_Sent_Date = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                }
                else
                {
                    objHumanRecord.Is_Mail_Sent = "N";
                }
                if (msktxtZipcode.TextWithLiterals != "-")
                {
                    if (msktxtZipcode.TextWithLiterals.Length == 6 && msktxtZipcode.TextWithLiterals.Length < 10)
                    {
                        string[] Split = Convert.ToString(msktxtZipcode.TextWithLiterals).Split('-');
                        if (Split.Length == 2 && Split[1] == string.Empty)
                        {
                            objHumanRecord.ZipCode = Split[0].ToString();
                        }
                    }
                    else
                    {

                        objHumanRecord.ZipCode = msktxtZipcode.TextWithLiterals;
                    }
                }
                else
                {
                    objHumanRecord.ZipCode = string.Empty;
                }

                objHumanRecord.Preferred_Confidential_Correspodence_Mode = cboPreferredConfidentialCoreespondenceMode.SelectedItem.Text;
                objHumanRecord.EMail = txtMail.Text;
                objHumanRecord.Password = string.Empty;
                objHumanRecord.Human_Type = cboHumanType.Text;
                objHumanRecord.Suffix = cboPatientSuffix.SelectedItem.Text;

                string sFTPPath = UploadPhoto();

                if (sFTPPath != string.Empty)
                    objHumanRecord.Photo_Path = sFTPPath;

                ListToUpdateHuman.Add(objHumanRecord);

            }

            if (chkEligibilityVerified.Checked == true)
            {

                Eligibility_Verification objEligibility = new Eligibility_Verification();
                if (hdnHumanID.Value != string.Empty)
                {
                    objEligibility.Human_ID = Convert.ToUInt64(hdnHumanID.Value);
                }
                objEligibility.Eligibility_Verified_By = ClientSession.UserName;
                if (hdnLocalTime.Value != string.Empty)
                {
                    objEligibility.Eligibility_Verified_Date = Convert.ToDateTime(hdnLocalTime.Value);
                }


                if (dtpEffectiveStartDate.Text != "")
                {
                    objEligibility.Effective_Date = Convert.ToDateTime(dtpEffectiveStartDate.Text);
                }
                else
                {
                    objEligibility.Effective_Date = Convert.ToDateTime(DateTime.MinValue);
                }
                objEligibility.Policy_Holder_ID = txtPolicyHolderID.Text;
                objEligibility.Group_Number = txtGroupNumber.Text;
                if (dtpTerminationDate.Text != "")
                {
                    objEligibility.Termination_Date = Convert.ToDateTime(dtpTerminationDate.Text);
                }

                else
                {
                    objEligibility.Termination_Date = DateTime.MinValue;
                }

                if (cboVerificationType.Items.FindByValue(cboVerificationType.SelectedItem.Text.ToString().Trim()) != null && cboVerificationType.SelectedItem != null)
                {
                    objEligibility.Eligibility_Type = cboVerificationType.SelectedItem.Text.ToString().Trim();
                }
                objEligibility.Comments = txtDemoNote.Text;


                if (txtInsurancetype.Text != string.Empty)
                {
                    objEligibility.Plan_Type = Convert.ToString(txtInsurancetype.Text);
                }
                if (txtPlanNumber.Text != string.Empty)
                {
                    objEligibility.Plan_Number = Convert.ToString(txtPlanNumber.Text);
                }
                if (txtOrganization.Text != string.Empty)
                {
                    objEligibility.Organization = Convert.ToString(txtOrganization.Text);
                }
                if (txtSubscriberName.Text != string.Empty)
                {
                    objEligibility.Subscriber_Name = Convert.ToString(txtSubscriberName.Text);
                }
                if (txtPCPName.Text != string.Empty)
                {
                    objEligibility.PCP_Name = Convert.ToString(txtPCPName.Text);
                }
                if (txtRelationship.Text != string.Empty)
                {
                    objEligibility.Relationship_to_Subscriber = Convert.ToString(txtRelationship.Text);
                }
                if (txtPCP_NPI.Text != string.Empty)
                {
                    objEligibility.PCP_NPI = Convert.ToString(txtPCP_NPI.Text);
                }
                if (txtGroupNumber.Text != string.Empty)
                {
                    objEligibility.Group_Number = Convert.ToString(txtGroupNumber.Text);
                }
                if (dtpPCPEffectiveDate.Text != string.Empty)
                {
                    objEligibility.PCP_Effective_Date = Convert.ToDateTime(dtpPCPEffectiveDate.Text);
                }
                if (txtGroupName.Text != string.Empty)
                {
                    objEligibility.Group_Name = Convert.ToString(txtGroupName.Text);
                }
                if (txtIPAName.Text != string.Empty)
                {
                    objEligibility.IPA_Name = Convert.ToString(txtIPAName.Text);
                }


                if (txtPCPVisitInCopay.Text != string.Empty)
                {
                    objEligibility.PCP_Office_Visit_InNet_Copay = Convert.ToDouble(txtPCPVisitInCopay.Text);
                }
                if (txtPCPVisitInCoIns.Text != string.Empty)
                {
                    objEligibility.PCP_Office_Visit_InNet_CoIns = Convert.ToDouble(txtPCPVisitInCoIns.Text);
                }
                if (txtPCPVisitOutCopay.Text != string.Empty)
                {
                    objEligibility.PCP_Office_Visit_OutNet_Copay = Convert.ToDouble(txtPCPVisitOutCopay.Text);
                }
                if (txtPCPVisitOutCoIns.Text != string.Empty)
                {
                    objEligibility.PCP_Office_Visit_OutNet_CoIns = Convert.ToDouble(txtPCPVisitOutCoIns.Text);
                }


                if (txtSpecialityVisitInCopay.Text != string.Empty)
                {
                    objEligibility.Specialty_Office_Visit_InNet_Copay = Convert.ToDouble(txtSpecialityVisitInCopay.Text);
                }
                if (txtSpecialityVisitInCoIns.Text != string.Empty)
                {
                    objEligibility.Specialty_Office_Visit_InNet_CoIns = Convert.ToDouble(txtSpecialityVisitInCoIns.Text);
                }
                if (txtSpecialityVisitOutCopay.Text != string.Empty)
                {
                    objEligibility.Specialty_Office_Visit_OutNet_Copay = Convert.ToDouble(txtSpecialityVisitOutCopay.Text);
                }
                if (txtSpecialityVisitOutCoIns.Text != string.Empty)
                {
                    objEligibility.Specialty_Office_Visit_OutNet_CoIns = Convert.ToDouble(txtSpecialityVisitOutCoIns.Text);
                }


                if (txtMedicationInCopay.Text != string.Empty)
                {
                    objEligibility.Inj_Medication_InNet_Copay = Convert.ToDouble(txtMedicationInCopay.Text);
                }
                if (txtMedicationInCoIns.Text != string.Empty)
                {
                    objEligibility.Inj_Medication_InNet_CoIns = Convert.ToDouble(txtMedicationInCoIns.Text);
                }
                if (txtMedicationOutCopay.Text != string.Empty)
                {
                    objEligibility.Inj_Medication_OutNet_Copay = Convert.ToDouble(txtMedicationOutCopay.Text);
                }
                if (txtMedicationOutCoIns.Text != string.Empty)
                {
                    objEligibility.Inj_Medication_OutNet_CoIns = Convert.ToDouble(txtMedicationOutCoIns.Text);
                }


                if (txtUrgentCareInCopay.Text != string.Empty)
                {
                    objEligibility.Urgent_Care_InNet_Copay = Convert.ToDouble(txtUrgentCareInCopay.Text);
                }
                if (txtUrgentCareCoInIns.Text != string.Empty)
                {
                    objEligibility.Urgent_Care_InNet_CoIns = Convert.ToDouble(txtUrgentCareCoInIns.Text);
                }
                if (txtUrgentCareOutCopay.Text != string.Empty)
                {
                    objEligibility.Urgent_Care_OutNet_Copay = Convert.ToDouble(txtUrgentCareOutCopay.Text);
                }
                if (txtUrgentCareOutCoIns.Text != string.Empty)
                {
                    objEligibility.Urgent_Care_OutNet_CoIns = Convert.ToDouble(txtUrgentCareOutCoIns.Text);
                }


                if (txtPCPInNetworkMessage.Text != string.Empty)
                {
                    objEligibility.PCP_InNetwork_Copay_Message = Convert.ToString(txtPCPInNetworkMessage.Text);
                }
                if (txtPCPOutNetworkMessage.Text != string.Empty)
                {
                    objEligibility.PCP_OutNetwork_Copay_Message = Convert.ToString(txtPCPOutNetworkMessage.Text);
                }


                if (txtInDeductiblePlan.Text != string.Empty)
                {
                    objEligibility.Ind_per_plan_InNet_Deductible = Convert.ToDouble(txtInDeductiblePlan.Text);
                }
                if (txtInPockot.Text != string.Empty)
                {
                    objEligibility.Ind_per_plan_InNet_Out_of_Pocket = Convert.ToDouble(txtInPockot.Text);
                }
                if (txtOutDeductiblePlan.Text != string.Empty)
                {
                    objEligibility.Ind_per_plan_OutNet_Deductible = Convert.ToDouble(txtOutDeductiblePlan.Text);
                }
                if (txtOutPocket.Text != string.Empty)
                {
                    objEligibility.Ind_per_plan_OutNet_Out_of_Pocket = Convert.ToDouble(txtOutPocket.Text);
                }


                if (txtInDeductiblemet.Text != string.Empty)
                {
                    objEligibility.Ind_met_InNet_Deductible = Convert.ToDouble(txtInDeductiblemet.Text);
                }
                if (txtInpocketmet.Text != string.Empty)
                {
                    objEligibility.Ind_met_InNet_Out_of_Pocket = Convert.ToDouble(txtInpocketmet.Text);
                }

                if (txtOutDeductiblemet.Text != string.Empty)
                {
                    objEligibility.Ind_met_OutNet_Deductible = Convert.ToDouble(txtOutDeductiblemet.Text);
                }
                if (txtOutpocketmet.Text != string.Empty)
                {
                    objEligibility.Ind_met_OutNet_Out_of_Pocket = Convert.ToDouble(txtOutpocketmet.Text);
                }


                if (txtInFamilyDeductible.Text != string.Empty)
                {
                    objEligibility.Family_per_plan_InNet_Deductible = Convert.ToDouble(txtInFamilyDeductible.Text);
                }
                if (txtInFamilypocket.Text != string.Empty)
                {
                    objEligibility.Family_per_plan_InNet_Out_of_Pocket = Convert.ToDouble(txtInFamilypocket.Text);
                }
                if (txtOutFamilyDeductible.Text != string.Empty)
                {
                    objEligibility.Family_per_plan_OutNet_Deductible = Convert.ToDouble(txtOutFamilyDeductible.Text);
                }
                if (txtOutFamilypocket.Text != string.Empty)
                {
                    objEligibility.Family_per_plan_OutNet_Out_of_Pocket = Convert.ToDouble(txtOutFamilypocket.Text);
                }


                if (txtInFamilyDeductiblemet.Text != string.Empty)
                {
                    objEligibility.Family_met_InNet_Deductible = Convert.ToDouble(txtInFamilyDeductiblemet.Text);
                }
                if (txtInFamilymetpocket.Text != string.Empty)
                {
                    objEligibility.Family_met_InNet_Out_of_Pocket = Convert.ToDouble(txtInFamilymetpocket.Text);
                }
                if (txtOutFamilyDeductiblemet.Text != string.Empty)
                {
                    objEligibility.Family_met_OutNet_Deductible = Convert.ToDouble(txtOutFamilyDeductiblemet.Text);
                }
                if (txtOutFamilymetpocket.Text != string.Empty)
                {
                    objEligibility.Family_met_OutNet_Out_of_Pocket = Convert.ToDouble(txtOutFamilymetpocket.Text);
                }


                if (txtDeductibleInNetworkMessage.Text != string.Empty)
                {
                    objEligibility.Deductible_InNetwork_Message = Convert.ToString(txtDeductibleInNetworkMessage.Text);
                }
                if (txtDeductibleOutNetworkMessage.Text != string.Empty)
                {
                    objEligibility.Deductible_OutNetwork_Message = Convert.ToString(txtDeductibleOutNetworkMessage.Text);
                }




                //Added by Bala for ADD MESSAGES IN EV in 20-12-2013
                if (txtDemoNote.Text != string.Empty)
                {
                    objPat.Message_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    objPat.Message_Description = "Eligibility Verification";
                    objPat.Notes = txtDemoNote.Text;
                    objPat.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    objPat.Created_By = ClientSession.UserName;

                    objPat.Is_PatientChart = "N";
                    objPat.Line_ID = 0;
                    objPat.Type = "MESSAGE";
                    objPat.Encounter_ID = Convert.ToInt32(hdnEncounterID.Value);
                    objPat.Statement_ChargeLine_ID = 0;
                    objPat.SourceID = Convert.ToInt32(hdnEncounterID.Value);
                    objPat.Source = "EV";
                    objPat.IsDelete = "N";
                    objPat.Is_PopupEnable = "N";
                    objPat.Priority = "NORMAL";

                    listPatientNotes.Add(objPat);

                }

                objEligibility.Insurance_Plan_ID = Convert.ToUInt64(ddlPlanName.SelectedValue);
                objEligibility.Insurance_Plan_Name = ddlPlanName.SelectedItem.Text;
                objEligibility.Payer_Name = ddlPayerName.SelectedItem.Text;

                objEligibility.Created_By = ClientSession.UserName;
                if (hdnLocalTime.Value != string.Empty)
                {
                    objEligibility.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);


                }

                bool planNameFlag = false;

                if (planNameFlag == true)
                {
                    objEligibility.Insurance_Plan_ID = Convert.ToUInt64(ddlPlanName.Items[ddlPlanName.SelectedIndex].Value);
                    objEligibility.Insurance_Plan_Name = ddlPlanName.SelectedItem.Text;
                }
                objEligibility.Eligibility_Check_Mode = "MANUAL";// bug Id:67077 
                objEligibility.Eligibility_Status = "EV-PERFORMED MANUALLY";// bug Id:67077 

                PatientInsuredPlanManager objpat = new PatientInsuredPlanManager();
                IList<PatientInsuredPlan> lstpat = new List<PatientInsuredPlan>();
                lstpat = objpat.getInsurancePoliciesByHumanId(Convert.ToUInt64(hdnHumanID.Value));
                lstpat = (from m in lstpat where m.Insurance_Plan_ID == (Convert.ToUInt64(ddlPlanName.Items[ddlPlanName.SelectedIndex].Value)) && m.Active.ToUpper() == "YES" select m).ToList<PatientInsuredPlan>();
                if (lstpat.Count > 0)
                    objEligibility.Insurance_Type = lstpat[0].Insurance_Type;

                ListToSaveEligibility.Add(objEligibility);

                if (grdExistingPolicies.SelectedIndex != null && grdExistingPolicies.SelectedIndex != -1)
                {
                    if (ddlPayerName.SelectedValue != grdExistingPolicies.SelectedRow.Cells[11].Text || ddlPlanName.SelectedValue != grdExistingPolicies.SelectedRow.Cells[2].Text || txtPolicyHolderID.Text != grdExistingPolicies.SelectedRow.Cells[1].Text)
                    {
                        if (objCheckOut.HumanObj.PatientInsuredBag != null)
                            PatInsOrderedList = objCheckOut.HumanObj.PatientInsuredBag.Where(aa => aa.Policy_Holder_ID.ToString() == txtPolicyHolderID.Text).Select(aa => aa).ToList();

                        if (PatInsOrderedList.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "QuickpatientCreate", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('380056');", true);
                            return;
                        }
                        else
                        {
                            objPatInsuredPlan = new PatientInsuredPlan();
                            objPatInsuredPlan.Active = "Yes";
                            objPatInsuredPlan.Assignment = "No";
                            objPatInsuredPlan.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                            objPatInsuredPlan.Created_By = ClientSession.UserName;

                            if (dtpEffectiveStartDate.Text != "")
                            {
                                objPatInsuredPlan.Effective_Start_Date = Convert.ToDateTime(dtpEffectiveStartDate.Text);
                            }
                            else
                            {
                                objPatInsuredPlan.Effective_Start_Date = Convert.ToDateTime(DateTime.MinValue);
                            }
                            objPatInsuredPlan.Policy_Holder_ID = txtPolicyHolderID.Text;
                            objPatInsuredPlan.Group_Number = txtGroupNumber.Text;
                            if (dtpTerminationDate.Text != "")
                            {
                                objPatInsuredPlan.Termination_Date = Convert.ToDateTime(dtpTerminationDate.Text);
                            }
                            else
                            {
                                objPatInsuredPlan.Termination_Date = DateTime.MinValue;
                            }
                            objPatInsuredPlan.Human_ID = Convert.ToUInt64(hdnHumanID.Value);
                            objPatInsuredPlan.Insurance_Plan_ID = Convert.ToUInt64(ddlPlanName.SelectedValue); //Convert.ToUInt64(grdExistingPolicies.Rows[grdExistingPolicies.SelectedIndex].Cells[2].Text);
                            objPatInsuredPlan.Insured_Human_ID = Convert.ToUInt64(hdnHumanID.Value);
                            objPatInsuredPlan.Relationship = "SELF";
                            objPatInsuredPlan.Relationship_No = 1;
                            objPatInsuredPlan.Sort_Order = 4;
                        }
                    }
                }
                else
                {
                    objPatInsuredPlan = new PatientInsuredPlan();
                    objPatInsuredPlan.Active = "Yes";
                    objPatInsuredPlan.Assignment = "No";

                    objPatInsuredPlan.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    objPatInsuredPlan.Created_By = ClientSession.UserName;

                    if (dtpEffectiveStartDate.Text != "")
                    {
                        objPatInsuredPlan.Effective_Start_Date = Convert.ToDateTime(dtpEffectiveStartDate.Text);
                    }
                    else
                    {
                        objPatInsuredPlan.Effective_Start_Date = Convert.ToDateTime(DateTime.MinValue);
                    }
                    objPatInsuredPlan.Policy_Holder_ID = txtPolicyHolderID.Text;
                    objPatInsuredPlan.Group_Number = txtGroupNumber.Text;
                    if (dtpTerminationDate.Text != "")
                    {
                        objPatInsuredPlan.Termination_Date = Convert.ToDateTime(dtpTerminationDate.Text);
                    }

                    else
                    {
                        objPatInsuredPlan.Termination_Date = DateTime.MinValue;
                    }

                    objPatInsuredPlan.Human_ID = Convert.ToUInt64(hdnHumanID.Value);
                    objPatInsuredPlan.Insurance_Plan_ID = Convert.ToUInt64(ddlPlanName.SelectedValue); // Convert.ToUInt64(grdExistingPolicies.Rows[grdExistingPolicies.SelectedIndex].Cells[2].Text);
                    objPatInsuredPlan.Insured_Human_ID = Convert.ToUInt64(hdnHumanID.Value);
                    objPatInsuredPlan.Relationship = "SELF";
                    objPatInsuredPlan.Relationship_No = 1;
                    objPatInsuredPlan.Sort_Order = 4;
                }
            }
            if (cboMethodOfPayment.Text != string.Empty)
            {
                if (gbPaymentInformation.Visible == true && chkMultiplePayments.Checked == false)//&& AppointmentSaveAndValidation() == false)
                {

                    VisitPayment objPayment = new VisitPayment();
                    Check objCheck = new Check();
                    PPHeader objPPHeader = new PPHeader();
                    PPLineItem objPPLineItem = new PPLineItem();
                    AccountTransaction objAccountTransaction = new AccountTransaction();


                    if (txtPaymentAmount.ReadOnly == false)
                    {
                        objPayment.Patient_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                        objCheck.Payment_Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                        objPPHeader.Total_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                        objPPLineItem.Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                        objAccountTransaction.Amount = -(Convert.ToDecimal(txtPaymentAmount.Text));
                    }
                    if (txtRefundAmount.Text != "")
                        objPayment.Refund_Amount = Convert.ToDecimal(txtRefundAmount.Text);
                    if (txtRecOnAcc.Text != "")
                        objPayment.Rec_On_Acc = Convert.ToDecimal(txtRecOnAcc.Text);

                    if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
                    {
                        objPayment.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                        objCheck.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                    }
                    else
                    {
                        objPayment.Check_Date = DateTime.MinValue;
                        objCheck.Check_Date = DateTime.MinValue;
                    }

                    objPayment.Relationship = cboRelation.SelectedItem.Text;
                    objPayment.Amount_Paid_By = txtpaidBy.Text;

                    objPayment.Check_Card_No = txtCheckNo.Text;
                    objPayment.Auth_No = txtAuthNo.Text;
                    objPayment.Method_of_Payment = cboMethodOfPayment.Text;
                    objPayment.Payment_Note = txtPaymentNote.Text;
                    objPayment.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                    objPayment.Facility_Name = ClientSession.FacilityName;

                    if (hdnEncounterID.Value != string.Empty)
                    {
                        objPayment.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    }
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objPayment.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                    objPayment.Created_By = ClientSession.UserName;
                    objPayment.Is_Delete = "N";
                    SaveVisitPaymentList.Add(objPayment);
                    FillVisitPaymentHistory(objPayment, null, string.Empty);
                    objCheck.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                    objCheck.Payment_Type = cboMethodOfPayment.Text;
                    objCheck.Created_By = ClientSession.UserName;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objCheck.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                    objCheck.Carrier_Patient_Name = txtPatientLastName.Text;
                    if (hdnEncounterID.Value != string.Empty)
                    {
                        objCheck.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                        if (cboMethodOfPayment.Text == "Cash")
                        {
                            objCheck.Payment_ID = hdnEncounterID.Value;
                        }
                    }
                    if (txtCheckNo.Text != string.Empty)
                        objCheck.Payment_ID = txtCheckNo.Text;
                    if (hdnCarrierId.Value != string.Empty)
                    {
                        objCheck.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                    }
                    objCheck.Is_Delete = "N";
                    SaveCheckList.Add(objCheck);

                    objPPHeader.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                    objPPHeader.Created_By = string.Empty;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objPPHeader.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                    if (hdnEncounterID.Value != string.Empty)
                    {
                        objPPHeader.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    }
                    if (hdnCarrierId.Value != string.Empty)
                    {
                        objPPHeader.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                    }
                    objPPHeader.Is_Delete = "N";
                    SavePPHeaderList.Add(objPPHeader);

                    objPPLineItem.Claim_Type = "PATIENT";
                    objPPLineItem.Line_Type = "UNAPPLIED";
                    objPPLineItem.Created_By = ClientSession.UserName;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objPPLineItem.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                    objPPLineItem.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                    if (hdnEncounterID.Value != string.Empty)
                    {
                        objPPLineItem.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    }
                    if (hdnCarrierId.Value != string.Empty)
                    {
                        objPPLineItem.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                    }
                    objPPLineItem.Is_Delete = "N";
                    SavePPLineItemList.Add(objPPLineItem);

                    objAccountTransaction.Deposit_Date = UtilityManager.ConvertToLocal(objCheckOut.EncounterObj.Appointment_Date);
                    objAccountTransaction.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                    objAccountTransaction.Claim_Type = "PATIENT";
                    objAccountTransaction.Line_Type = "UNAPPLIED";
                    objAccountTransaction.Source_Type = "PP_LINE_ITEM";
                    objAccountTransaction.Created_By = ClientSession.UserName;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objAccountTransaction.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                    if (hdnEncounterID.Value != string.Empty)
                    {
                        objAccountTransaction.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    }
                    if (hdnCarrierId.Value != string.Empty)
                    {
                        objAccountTransaction.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                    }
                    objAccountTransaction.Is_Delete = "N";
                    SaveAccountTransactionList.Add(objAccountTransaction);
                    objAccountTransaction = new AccountTransaction();
                    if (txtRefundAmount.Text != string.Empty && txtRefundAmount.Text != "0.00" && txtRefundAmount.Text != "0")
                    {
                        objAccountTransaction.Amount = -(Convert.ToDecimal(txtRefundAmount.Text));
                        objAccountTransaction.Reversal_Refund_Category = "REFUND";
                        objAccountTransaction.Deposit_Date = UtilityManager.ConvertToLocal(objCheckOut.EncounterObj.Appointment_Date);
                        objAccountTransaction.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                        objAccountTransaction.Claim_Type = "PATIENT";
                        objAccountTransaction.Line_Type = "UNAPPLIED";
                        objAccountTransaction.Source_Type = "PP_LINE_ITEM";
                        objAccountTransaction.Created_By = ClientSession.UserName;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objAccountTransaction.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        objAccountTransaction.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                        objAccountTransaction.Is_Delete = "N";
                        SaveAccountTransactionList.Add(objAccountTransaction);
                    }

                }
                if (cboMethodOfPayment.Text != string.Empty)
                {
                    if (gbPaymentInformation.Visible == true && chkMultiplePayments.Checked == true)
                    {

                        VisitPayment objPayment = new VisitPayment();
                        Check objCheck = new Check();
                        PPHeader objPPHeader = new PPHeader();
                        PPLineItem objPPLineItem = new PPLineItem();
                        AccountTransaction objAccountTransaction = new AccountTransaction();


                        if (txtPaymentAmount.ReadOnly == false)
                        {
                            objPayment.Patient_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                            objCheck.Payment_Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                            objPPHeader.Total_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                            objPPLineItem.Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                            objAccountTransaction.Amount = -(Convert.ToDecimal(txtPaymentAmount.Text));
                        }
                        if (txtRefundAmount.Text != "")
                            objPayment.Refund_Amount = Convert.ToDecimal(txtRefundAmount.Text);
                        if (txtRecOnAcc.Text != "")
                            objPayment.Rec_On_Acc = Convert.ToDecimal(txtRecOnAcc.Text);

                        if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
                        {
                            objPayment.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                            objCheck.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                        }
                        else
                        {
                            objPayment.Check_Date = DateTime.MinValue;
                            objCheck.Check_Date = DateTime.MinValue;
                        }

                        objPayment.Check_Card_No = txtCheckNo.Text;
                        objPayment.Auth_No = txtAuthNo.Text;
                        objPayment.Method_of_Payment = cboMethodOfPayment.Text;
                        objPayment.Payment_Note = txtPaymentNote.Text;
                        objPayment.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                        objPayment.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                        objPayment.Relationship = cboRelation.SelectedItem.Text;
                        objPayment.Amount_Paid_By = txtpaidBy.Text;
                        objPayment.Facility_Name = ClientSession.FacilityName;

                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objPayment.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        objPayment.Created_By = ClientSession.UserName;
                        objPayment.Is_Delete = "N";
                        SaveVisitPaymentList.Add(objPayment);
                        FillVisitPaymentHistory(objPayment, null, string.Empty);
                        objCheck.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                        objCheck.Payment_Type = cboMethodOfPayment.Text;
                        objCheck.Created_By = ClientSession.UserName;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objCheck.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        objCheck.Carrier_Patient_Name = txtPatientLastName.Text;
                        objCheck.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                        if (cboMethodOfPayment.Text == "Cash")
                        {
                            objCheck.Payment_ID = hdnEncounterID.Value;
                        }
                        if (txtCheckNo.Text != string.Empty)
                            objCheck.Payment_ID = txtCheckNo.Text;
                        if (hdnCarrierId.Value != string.Empty)
                        {
                            objCheck.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                        }
                        objCheck.Is_Delete = "N";
                        SaveCheckList.Add(objCheck);

                        objPPHeader.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                        objPPHeader.Created_By = ClientSession.UserName;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objPPHeader.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        if (hdnEncounterID.Value != string.Empty)
                            objPPHeader.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                        if (hdnCarrierId.Value != string.Empty)
                        {
                            objPPHeader.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                        }
                        objPPHeader.Is_Delete = "N";
                        SavePPHeaderList.Add(objPPHeader);

                        objPPLineItem.Claim_Type = "PATIENT";
                        objPPLineItem.Line_Type = "UNAPPLIED";
                        objPPLineItem.Created_By = ClientSession.UserName;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objPPLineItem.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        objPPLineItem.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                        if (hdnEncounterID.Value != string.Empty)
                            objPPLineItem.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                        if (hdnCarrierId.Value != string.Empty)
                        {
                            objPPLineItem.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                        }
                        objPPLineItem.Is_Delete = "N";
                        SavePPLineItemList.Add(objPPLineItem);



                        objAccountTransaction.Deposit_Date = UtilityManager.ConvertToLocal(objCheckOut.EncounterObj.Appointment_Date);
                        objAccountTransaction.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                        objAccountTransaction.Claim_Type = "PATIENT";
                        objAccountTransaction.Line_Type = "UNAPPLIED";
                        objAccountTransaction.Source_Type = "PP_LINE_ITEM";
                        objAccountTransaction.Created_By = ClientSession.UserName;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objAccountTransaction.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        if (hdnEncounterID.Value != string.Empty)
                            objAccountTransaction.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                        if (hdnCarrierId.Value != string.Empty)
                        {
                            objAccountTransaction.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                        }
                        objAccountTransaction.Is_Delete = "N";
                        SaveAccountTransactionList.Add(objAccountTransaction);
                        objAccountTransaction = new AccountTransaction();
                        if (txtRefundAmount.Text != string.Empty && txtRefundAmount.Text != "0.00" && txtRefundAmount.Text != "0")
                        {
                            objAccountTransaction.Amount = -(Convert.ToDecimal(txtRefundAmount.Text));
                            objAccountTransaction.Reversal_Refund_Category = "REFUND";
                            objAccountTransaction.Deposit_Date = UtilityManager.ConvertToLocal(objCheckOut.EncounterObj.Appointment_Date);
                            objAccountTransaction.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                            objAccountTransaction.Claim_Type = "PATIENT";
                            objAccountTransaction.Line_Type = "UNAPPLIED";
                            objAccountTransaction.Source_Type = "PP_LINE_ITEM";
                            objAccountTransaction.Created_By = ClientSession.UserName;
                            if (hdnLocalTime.Value != string.Empty)
                            {
                                objAccountTransaction.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                            }
                            objAccountTransaction.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                            objAccountTransaction.Is_Delete = "N";
                            SaveAccountTransactionList.Add(objAccountTransaction);
                        }
                    }
                }
            }


            int iTryCount = 0;
        retry:

            XmlTextReader XmlText = null;
            try
            {
                if (hdnEncounterID.Value != string.Empty && hdnScreenMode.Value.ToUpper() == "CHECKEDIN")
                {
                    UtilityManager.ulMyFindPatientID = HumanMngr.BatchOperationsToQuickPatient(ListToSaveHuman.ToArray<Human>(), ListToUpdateHuman.ToArray<Human>(), ListToSaveEligibility.ToArray<Eligibility_Verification>(), SaveVisitPaymentList.ToArray<VisitPayment>(), SaveCheckList.ToArray<Check>(),
                        SavePPHeaderList.ToArray<PPHeader>(), SavePPLineItemList.ToArray<PPLineItem>(), SaveAccountTransactionList.ToArray<AccountTransaction>(), listPatientNotes.ToArray<PatientNotes>(), objPatInsuredPlan, string.Empty, Convert.ToUInt64(hdnEncounterID.Value), SaveVisitPaymenHistoryList, bCheckInToMa);
                    txtPatientAccountNo.Text = UtilityManager.ulMyFindPatientID.ToString();


                    //string strXmlHumanFilePath = string.Empty;
                    //if (ListToUpdateHuman.Count > 0)
                    //{
                    //    string HumanFileName = "Human" + "_" + ListToUpdateHuman[0].Id + ".xml";
                    //    strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
                    //}
                    //if (File.Exists(strXmlHumanFilePath) == false && ListToUpdateHuman[0].Id > 0)
                    //{

                    //    if (ListToUpdateHuman != null && ListToUpdateHuman.Count > 0)
                    //    {

                    //        string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                    //        string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                    //        XmlDocument itemDoc = new XmlDocument();
                    //         XmlText = new XmlTextReader(sXmlPath);
                    //        itemDoc.Load(XmlText);
                    //        int iAge = 0;
                    //        iAge = UtilityManager.CalculateAge(ListToUpdateHuman[0].Birth_Date);

                    //        XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
                    //        if (xmlAge != null && xmlAge.Count > 0)
                    //            xmlAge[0].Attributes[0].Value = iAge.ToString();
                    //        else
                    //        {
                    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
                    //        }

                    //        XmlText.Close();
                    //       // itemDoc.Save(strXmlHumanFilePath);
                    //        int trycount = 0;
                    //    trytosaveagain:
                    //        try
                    //        {
                    //            itemDoc.Save(strXmlHumanFilePath);
                    //        }
                    //        catch (Exception xmlexcep)
                    //        {
                    //            trycount++;
                    //            if (trycount <= 3)
                    //            {
                    //                int TimeMilliseconds = 0;
                    //                if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                    //                    TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                    //                Thread.Sleep(TimeMilliseconds);
                    //                string sMsg = string.Empty;
                    //                string sExStackTrace = string.Empty;

                    //                string version = "";
                    //                if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                    //                    version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                    //                string[] server = version.Split('|');
                    //                string serverno = "";
                    //                if (server.Length > 1)
                    //                    serverno = server[1].Trim();

                    //                if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                    //                    sMsg = xmlexcep.InnerException.Message;
                    //                else
                    //                    sMsg = xmlexcep.Message;

                    //                if (xmlexcep != null && xmlexcep.StackTrace != null)
                    //                    sExStackTrace = xmlexcep.StackTrace;

                    //                string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    //                string ConnectionData;
                    //                ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    //                using (MySqlConnection con = new MySqlConnection(ConnectionData))
                    //                {
                    //                    using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                    //                    {
                    //                        cmd.Connection = con;
                    //                        try
                    //                        {
                    //                            con.Open();
                    //                            cmd.ExecuteNonQuery();
                    //                            con.Close();
                    //                        }
                    //                        catch
                    //                        {
                    //                        }
                    //                    }
                    //                }
                    //                goto trytosaveagain;
                    //            }
                    //        }

                    //    }
                    //}

                    //else
                    {

                        if (ListToUpdateHuman != null && ListToUpdateHuman.Count > 0)
                        {
                            UpdateAgeinBlob(ListToUpdateHuman[0].Id);

                        //    XmlDocument itemDoc = new XmlDocument();
                        //    string sXMLContent = String.Empty;
                        //    HumanBlobManager HumanBlobMngr = new HumanBlobManager();
                        //    Human_Blob objHumanblob = null;
                        //    IList<Human_Blob> ilstHumanBlob = HumanBlobMngr.GetHumanBlob(ListToUpdateHuman[0].Id);
                        //    if (ilstHumanBlob.Count > 0)
                        //    {
                        //        objHumanblob = ilstHumanBlob[0];
                        //        sXMLContent = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                        //        itemDoc.LoadXml(sXMLContent);
                        //    }

                                
                        //    // XmlText = new XmlTextReader(strXmlHumanFilePath);
                        //    //itemDoc.Load(XmlText);

                        //    int iAge = 0;
                        //    iAge = UtilityManager.CalculateAge(ListToUpdateHuman[0].Birth_Date);

                        //    XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
                        //    if (xmlAge != null && xmlAge.Count > 0)
                        //        xmlAge[0].Attributes[0].Value = iAge.ToString();
                        //    else
                        //    {
                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
                        //    }

                        //    XmlText.Close();
                        //   // itemDoc.Save(strXmlHumanFilePath);
                        //    //int trycount = 0;
                        ////trytosaveagain:
                        //    try
                        //    {
                        //        //itemDoc.Save(strXmlHumanFilePath);
                        //        IList<Human_Blob> ilstUpdateBlob = new List<Human_Blob>();
                        //        byte[] bytes = null;
                        //        try
                        //        {
                        //            bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);
                        //        }
                        //        catch (Exception ex)
                        //        {

                        //        }
                        //        objHumanblob.Human_XML = bytes;
                        //        ilstUpdateBlob.Add(objHumanblob);
                        //        HumanBlobMngr.SaveHumanBlobWithTransaction(ilstUpdateBlob, string.Empty);
                        //    }
                        //    catch (Exception xmlexcep)
                        //    {
                        //        throw new Exception(xmlexcep.Message.ToString());

                        //        //trycount++;
                        //        //if (trycount <= 3)
                        //        //{
                        //        //    int TimeMilliseconds = 0;
                        //        //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                        //        //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                        //        //    Thread.Sleep(TimeMilliseconds);
                        //        //    string sMsg = string.Empty;
                        //        //    string sExStackTrace = string.Empty;

                        //        //    string version = "";
                        //        //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        //        //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                        //        //    string[] server = version.Split('|');
                        //        //    string serverno = "";
                        //        //    if (server.Length > 1)
                        //        //        serverno = server[1].Trim();

                        //        //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                        //        //        sMsg = xmlexcep.InnerException.Message;
                        //        //    else
                        //        //        sMsg = xmlexcep.Message;

                        //        //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                        //        //        sExStackTrace = xmlexcep.StackTrace;

                        //        //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        //        //    string ConnectionData;
                        //        //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        //        //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                        //        //    {
                        //        //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                        //        //        {
                        //        //            cmd.Connection = con;
                        //        //            try
                        //        //            {
                        //        //                con.Open();
                        //        //                cmd.ExecuteNonQuery();
                        //        //                con.Close();
                        //        //            }
                        //        //            catch
                        //        //            {
                        //        //            }
                        //        //        }
                        //        //    }
                        //        //    goto trytosaveagain;
                        //        //}
                        //    }
                        }
                    }

                    btnSave.Enabled = false;
                    btnSave.CssClass = "aspresizedgreenbutton";
                    if (hdnEncounterID.Value != "0" && hdnEncounterID.Value != "")
                    {
                    }
                }

                else if (hdnEncounterID.Value != string.Empty && hdnScreenMode.Value.ToUpper() == "SCANNING")
                {
                    UtilityManager.ulMyFindPatientID = HumanMngr.BatchOperationsToQuickPatient(ListToSaveHuman.ToArray<Human>(), ListToUpdateHuman.ToArray<Human>(), ListToSaveEligibility.ToArray<Eligibility_Verification>(), SaveVisitPaymentList.ToArray<VisitPayment>(), SaveCheckList.ToArray<Check>(),
                        SavePPHeaderList.ToArray<PPHeader>(), SavePPLineItemList.ToArray<PPLineItem>(), SaveAccountTransactionList.ToArray<AccountTransaction>(), listPatientNotes.ToArray<PatientNotes>(), objPatInsuredPlan, string.Empty, 0, SaveVisitPaymenHistoryList);
                    txtPatientAccountNo.Text = UtilityManager.ulMyFindPatientID.ToString();



                    if (ListToUpdateHuman != null && ListToUpdateHuman.Count > 0)
                    {
                        //string HumanFileName = "Human" + "_" + ListToUpdateHuman[0].Id + ".xml";
                        //string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);


                        //if (File.Exists(strXmlHumanFilePath) == false && ListToUpdateHuman[0].Id > 0)
                        //{



                        //    string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                        //    string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                        //    XmlDocument itemDoc = new XmlDocument();
                        //     XmlText = new XmlTextReader(sXmlPath);
                        //    itemDoc.Load(XmlText);
                        //    int iAge = 0;
                        //    iAge = UtilityManager.CalculateAge(ListToUpdateHuman[0].Birth_Date);

                        //    XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
                        //    if (xmlAge != null && xmlAge.Count > 0)
                        //        xmlAge[0].Attributes[0].Value = iAge.ToString();
                        //    else
                        //    {
                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
                        //    }

                        //    XmlText.Close();
                        //    //itemDoc.Save(strXmlHumanFilePath);
                        //    int trycount = 0;
                        //trytosaveagain:
                        //    try
                        //    {
                        //        itemDoc.Save(strXmlHumanFilePath);
                        //    }
                        //    catch (Exception xmlexcep)
                        //    {
                        //        trycount++;
                        //        if (trycount <= 3)
                        //        {
                        //            int TimeMilliseconds = 0;
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                        //                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                        //            Thread.Sleep(TimeMilliseconds);
                        //            string sMsg = string.Empty;
                        //            string sExStackTrace = string.Empty;

                        //            string version = "";
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        //                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                        //            string[] server = version.Split('|');
                        //            string serverno = "";
                        //            if (server.Length > 1)
                        //                serverno = server[1].Trim();

                        //            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                        //                sMsg = xmlexcep.InnerException.Message;
                        //            else
                        //                sMsg = xmlexcep.Message;

                        //            if (xmlexcep != null && xmlexcep.StackTrace != null)
                        //                sExStackTrace = xmlexcep.StackTrace;

                        //            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        //            string ConnectionData;
                        //            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        //            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                        //            {
                        //                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                        //                {
                        //                    cmd.Connection = con;
                        //                    try
                        //                    {
                        //                        con.Open();
                        //                        cmd.ExecuteNonQuery();
                        //                        con.Close();
                        //                    }
                        //                    catch
                        //                    {
                        //                    }
                        //                }
                        //            }
                        //            goto trytosaveagain;
                        //        }
                        //    }

                        //}
                        //else
                        {
                            UpdateAgeinBlob(ListToUpdateHuman[0].Id);

                        //    XmlDocument itemDoc = new XmlDocument();
                        //    string sXMLContent = String.Empty;
                        //    HumanBlobManager HumanBlobMngr = new HumanBlobManager();
                        //    Human_Blob objHumanblob = null;
                        //    IList<Human_Blob> ilstHumanBlob = HumanBlobMngr.GetHumanBlob(ListToUpdateHuman[0].Id);
                        //    if (ilstHumanBlob.Count > 0)
                        //    {
                        //        objHumanblob = ilstHumanBlob[0];
                        //        sXMLContent = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                        //        itemDoc.LoadXml(sXMLContent);
                        //    }

                        //    //XmlDocument itemDoc = new XmlDocument();
                        //    // XmlText = new XmlTextReader(strXmlHumanFilePath);
                        //    //itemDoc.Load(XmlText);


                        //    int iAge = 0;
                        //    iAge = UtilityManager.CalculateAge(ListToUpdateHuman[0].Birth_Date);

                        //    XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
                        //    if (xmlAge != null && xmlAge.Count > 0)
                        //        xmlAge[0].Attributes[0].Value = iAge.ToString();
                        //    else
                        //    {
                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
                        //    }

                        //    XmlText.Close();
                        //    //itemDoc.Save(strXmlHumanFilePath);
                        //    //int trycount = 0;
                        ////trytosaveagain:
                        //    try
                        //    {
                        //        //itemDoc.Save(strXmlHumanFilePath);

                        //        IList<Human_Blob> ilstUpdateBlob = new List<Human_Blob>();
                        //        byte[] bytes = null;
                        //        try
                        //        {
                        //            bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);
                        //        }
                        //        catch (Exception ex)
                        //        {

                        //        }
                        //        objHumanblob.Human_XML = bytes;
                        //        ilstUpdateBlob.Add(objHumanblob);
                        //        HumanBlobMngr.SaveHumanBlobWithTransaction(ilstUpdateBlob, string.Empty);
                        //    }
                        //    catch (Exception xmlexcep)
                        //    {
                        //        throw new Exception(xmlexcep.Message.ToString());
                        //        //trycount++;
                        //        //if (trycount <= 3)
                        //        //{
                        //        //    int TimeMilliseconds = 0;
                        //        //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                        //        //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                        //        //    Thread.Sleep(TimeMilliseconds);
                        //        //    string sMsg = string.Empty;
                        //        //    string sExStackTrace = string.Empty;

                        //        //    string version = "";
                        //        //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        //        //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                        //        //    string[] server = version.Split('|');
                        //        //    string serverno = "";
                        //        //    if (server.Length > 1)
                        //        //        serverno = server[1].Trim();

                        //        //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                        //        //        sMsg = xmlexcep.InnerException.Message;
                        //        //    else
                        //        //        sMsg = xmlexcep.Message;

                        //        //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                        //        //        sExStackTrace = xmlexcep.StackTrace;

                        //        //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        //        //    string ConnectionData;
                        //        //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        //        //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                        //        //    {
                        //        //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                        //        //        {
                        //        //            cmd.Connection = con;
                        //        //            try
                        //        //            {
                        //        //                con.Open();
                        //        //                cmd.ExecuteNonQuery();
                        //        //                con.Close();
                        //        //            }
                        //        //            catch
                        //        //            {
                        //        //            }
                        //        //        }
                        //        //    }
                        //        //    goto trytosaveagain;
                        //        //}
                        //    }
                        }
                    }


                    btnSave.Enabled = false;
                    btnSave.CssClass = "aspresizedgreenbutton";
                }
                else if (hdnEncounterID.Value != string.Empty && hdnScreenMode.Value.ToUpper() == "EDIT NAME")
                {
                    UtilityManager.ulMyFindPatientID = HumanMngr.BatchOperationsToQuickPatient(null, ListToUpdateHuman.ToArray<Human>(), ListToSaveEligibility.ToArray<Eligibility_Verification>(), SaveVisitPaymentList.ToArray<VisitPayment>(), SaveCheckList.ToArray<Check>(),
                        SavePPHeaderList.ToArray<PPHeader>(), SavePPLineItemList.ToArray<PPLineItem>(), SaveAccountTransactionList.ToArray<AccountTransaction>(), listPatientNotes.ToArray<PatientNotes>(), objPatInsuredPlan, string.Empty, objCheckOut.EncounterObj.Id, SaveVisitPaymenHistoryList);
                    txtPatientAccountNo.Text = UtilityManager.ulMyFindPatientID.ToString();



                    if (ListToUpdateHuman != null && ListToUpdateHuman.Count > 0)
                    {
                        UpdateAgeinBlob(ListToUpdateHuman[0].Id);

                        //string HumanFileName = "Human" + "_" + ListToUpdateHuman[0].Id + ".xml";
                        //string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);

                        //if (File.Exists(strXmlHumanFilePath) == false && ListToUpdateHuman[0].Id > 0)
                        //{

                        //    string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                        //    string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                        //    XmlDocument itemDoc = new XmlDocument();
                        //     XmlText = new XmlTextReader(sXmlPath);
                        //    itemDoc.Load(XmlText);
                        //    int iAge = 0;
                        //    iAge = UtilityManager.CalculateAge(ListToUpdateHuman[0].Birth_Date);

                        //    XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
                        //    if (xmlAge != null && xmlAge.Count > 0)
                        //        xmlAge[0].Attributes[0].Value = iAge.ToString();
                        //    else
                        //    {
                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
                        //    }

                        //    XmlText.Close();
                        //   // itemDoc.Save(strXmlHumanFilePath);
                        //    int trycount = 0;
                        //trytosaveagain:
                        //    try
                        //    {
                        //        itemDoc.Save(strXmlHumanFilePath);
                        //    }
                        //    catch (Exception xmlexcep)
                        //    {
                        //        trycount++;
                        //        if (trycount <= 3)
                        //        {
                        //            int TimeMilliseconds = 0;
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                        //                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                        //            Thread.Sleep(TimeMilliseconds);
                        //            string sMsg = string.Empty;
                        //            string sExStackTrace = string.Empty;

                        //            string version = "";
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        //                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                        //            string[] server = version.Split('|');
                        //            string serverno = "";
                        //            if (server.Length > 1)
                        //                serverno = server[1].Trim();

                        //            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                        //                sMsg = xmlexcep.InnerException.Message;
                        //            else
                        //                sMsg = xmlexcep.Message;

                        //            if (xmlexcep != null && xmlexcep.StackTrace != null)
                        //                sExStackTrace = xmlexcep.StackTrace;

                        //            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        //            string ConnectionData;
                        //            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        //            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                        //            {
                        //                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                        //                {
                        //                    cmd.Connection = con;
                        //                    try
                        //                    {
                        //                        con.Open();
                        //                        cmd.ExecuteNonQuery();
                        //                        con.Close();
                        //                    }
                        //                    catch
                        //                    {
                        //                    }
                        //                }
                        //            }
                        //            goto trytosaveagain;
                        //        }
                        //    }

                        //}
                        //else
                        //{
                        //    //GenerateXml XMLObj = new GenerateXml();
                        //    XmlDocument itemDoc = new XmlDocument();
                        //     XmlText = new XmlTextReader(strXmlHumanFilePath);
                        //    itemDoc.Load(XmlText);

                        //    int iAge = 0;
                        //    iAge = UtilityManager.CalculateAge(ListToUpdateHuman[0].Birth_Date);

                        //    XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
                        //    if (xmlAge != null && xmlAge.Count > 0)
                        //        xmlAge[0].Attributes[0].Value = iAge.ToString();
                        //    else
                        //    {
                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
                        //    }

                        //    XmlText.Close();
                        //    //itemDoc.Save(strXmlHumanFilePath);
                        //    int trycount = 0;
                        //trytosaveagain:
                        //    try
                        //    {
                        //        itemDoc.Save(strXmlHumanFilePath);
                        //    }
                        //    catch (Exception xmlexcep)
                        //    {
                        //        trycount++;
                        //        if (trycount <= 3)
                        //        {
                        //            int TimeMilliseconds = 0;
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                        //                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                        //            Thread.Sleep(TimeMilliseconds);
                        //            string sMsg = string.Empty;
                        //            string sExStackTrace = string.Empty;

                        //            string version = "";
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        //                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                        //            string[] server = version.Split('|');
                        //            string serverno = "";
                        //            if (server.Length > 1)
                        //                serverno = server[1].Trim();

                        //            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                        //                sMsg = xmlexcep.InnerException.Message;
                        //            else
                        //                sMsg = xmlexcep.Message;

                        //            if (xmlexcep != null && xmlexcep.StackTrace != null)
                        //                sExStackTrace = xmlexcep.StackTrace;

                        //            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        //            string ConnectionData;
                        //            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        //            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                        //            {
                        //                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                        //                {
                        //                    cmd.Connection = con;
                        //                    try
                        //                    {
                        //                        con.Open();
                        //                        cmd.ExecuteNonQuery();
                        //                        con.Close();
                        //                    }
                        //                    catch
                        //                    {
                        //                    }
                        //                }
                        //            }
                        //            goto trytosaveagain;
                        //        }
                        //    }

                        //}
                    }

                    btnSave.Enabled = false;
                    btnSave.CssClass = "aspresizedgreenbutton";
                }
                else if (hdnEncounterID.Value == "0" && hdnScreenMode.Value.ToUpper() != "CHECKEDIN" && hdnIsmailsend.Value == "true")
                {
                    UtilityManager.ulMyFindPatientID = HumanMngr.BatchOperationsToQuickPatient(ListToSaveHuman.ToArray<Human>(), ListToUpdateHuman.ToArray<Human>(), ListToSaveEligibility.ToArray<Eligibility_Verification>(), SaveVisitPaymentList.ToArray<VisitPayment>(), SaveCheckList.ToArray<Check>(),
                        SavePPHeaderList.ToArray<PPHeader>(), SavePPLineItemList.ToArray<PPLineItem>(), SaveAccountTransactionList.ToArray<AccountTransaction>(), listPatientNotes.ToArray<PatientNotes>(), objPatInsuredPlan, string.Empty, objCheckOut.EncounterObj.Id, SaveVisitPaymenHistoryList);
                    txtPatientAccountNo.Text = UtilityManager.ulMyFindPatientID.ToString();



                    if (ListToUpdateHuman != null && ListToUpdateHuman.Count > 0)
                    {
                        UpdateAgeinBlob(ListToUpdateHuman[0].Id);

                        //string HumanFileName = "Human" + "_" + ListToUpdateHuman[0].Id + ".xml";
                        //string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
                        //if (File.Exists(strXmlHumanFilePath) == false && ListToUpdateHuman[0].Id > 0)
                        //{

                        //    string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                        //    string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                        //    XmlDocument itemDoc = new XmlDocument();
                        //     XmlText = new XmlTextReader(sXmlPath);
                        //    itemDoc.Load(XmlText);
                        //    int iAge = 0;
                        //    iAge = UtilityManager.CalculateAge(ListToUpdateHuman[0].Birth_Date);

                        //    XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
                        //    if (xmlAge != null && xmlAge.Count > 0)
                        //        xmlAge[0].Attributes[0].Value = iAge.ToString();
                        //    else
                        //    {
                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
                        //    }

                        //    XmlText.Close();
                        //   // itemDoc.Save(strXmlHumanFilePath);
                        //    int trycount = 0;
                        //trytosaveagain:
                        //    try
                        //    {
                        //        itemDoc.Save(strXmlHumanFilePath);
                        //    }
                        //    catch (Exception xmlexcep)
                        //    {
                        //        trycount++;
                        //        if (trycount <= 3)
                        //        {
                        //            int TimeMilliseconds = 0;
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                        //                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                        //            Thread.Sleep(TimeMilliseconds);
                        //            string sMsg = string.Empty;
                        //            string sExStackTrace = string.Empty;

                        //            string version = "";
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        //                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                        //            string[] server = version.Split('|');
                        //            string serverno = "";
                        //            if (server.Length > 1)
                        //                serverno = server[1].Trim();

                        //            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                        //                sMsg = xmlexcep.InnerException.Message;
                        //            else
                        //                sMsg = xmlexcep.Message;

                        //            if (xmlexcep != null && xmlexcep.StackTrace != null)
                        //                sExStackTrace = xmlexcep.StackTrace;

                        //            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        //            string ConnectionData;
                        //            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        //            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                        //            {
                        //                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                        //                {
                        //                    cmd.Connection = con;
                        //                    try
                        //                    {
                        //                        con.Open();
                        //                        cmd.ExecuteNonQuery();
                        //                        con.Close();
                        //                    }
                        //                    catch
                        //                    {
                        //                    }
                        //                }
                        //            }
                        //            goto trytosaveagain;
                        //        }
                        //    }

                        //}
                        //else
                        //{

                        //    XmlDocument itemDoc = new XmlDocument();
                        //     XmlText = new XmlTextReader(strXmlHumanFilePath);
                        //    itemDoc.Load(XmlText);

                        //    int iAge = 0;
                        //    iAge = UtilityManager.CalculateAge(ListToUpdateHuman[0].Birth_Date);

                        //    XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
                        //    if (xmlAge != null && xmlAge.Count > 0)
                        //        xmlAge[0].Attributes[0].Value = iAge.ToString();
                        //    else
                        //    {
                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
                        //    }

                        //    XmlText.Close();
                        //   // itemDoc.Save(strXmlHumanFilePath);
                        //    int trycount = 0;
                        //trytosaveagain:
                        //    try
                        //    {
                        //        itemDoc.Save(strXmlHumanFilePath);
                        //    }
                        //    catch (Exception xmlexcep)
                        //    {
                        //        trycount++;
                        //        if (trycount <= 3)
                        //        {
                        //            int TimeMilliseconds = 0;
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                        //                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                        //            Thread.Sleep(TimeMilliseconds);
                        //            string sMsg = string.Empty;
                        //            string sExStackTrace = string.Empty;

                        //            string version = "";
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        //                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                        //            string[] server = version.Split('|');
                        //            string serverno = "";
                        //            if (server.Length > 1)
                        //                serverno = server[1].Trim();

                        //            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                        //                sMsg = xmlexcep.InnerException.Message;
                        //            else
                        //                sMsg = xmlexcep.Message;

                        //            if (xmlexcep != null && xmlexcep.StackTrace != null)
                        //                sExStackTrace = xmlexcep.StackTrace;

                        //            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        //            string ConnectionData;
                        //            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        //            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                        //            {
                        //                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                        //                {
                        //                    cmd.Connection = con;
                        //                    try
                        //                    {
                        //                        con.Open();
                        //                        cmd.ExecuteNonQuery();
                        //                        con.Close();
                        //                    }
                        //                    catch
                        //                    {
                        //                    }
                        //                }
                        //            }
                        //            goto trytosaveagain;
                        //        }
                        //    }

                        //}
                    }

                    btnSave.Enabled = false;
                    btnSave.CssClass = "aspresizedgreenbutton";

                }

                else
                {

                    IList<Human> objhumanupdate = new List<Human>();
                    UtilityManager.ulMyFindPatientID = HumanMngr.BatchOperationsToQuickPatient(ListToSaveHuman.ToArray<Human>(), ListToUpdateHuman.ToArray<Human>(), ListToSaveEligibility.ToArray<Eligibility_Verification>(), SaveVisitPaymentList.ToArray<VisitPayment>(), SaveCheckList.ToArray<Check>(),
                            SavePPHeaderList.ToArray<PPHeader>(), SavePPLineItemList.ToArray<PPLineItem>(), SaveAccountTransactionList.ToArray<AccountTransaction>(), listPatientNotes.ToArray<PatientNotes>(), objPatInsuredPlan, string.Empty, 0, SaveVisitPaymenHistoryList);
                    if (ListToSaveHuman != null && ListToSaveHuman.Count > 0)
                    {
                        txtPatientAccountNo.Text = Convert.ToString(ListToSaveHuman[0].Id);
                    }


                    if (ListToSaveHuman != null && ListToSaveHuman.Count > 0)
                    {

                        //string HumanFileName = "Human" + "_" + ListToSaveHuman[0].Id + ".xml";
                        //string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
                        //if (File.Exists(strXmlHumanFilePath) == false && ListToSaveHuman[0].Id > 0)
                        //{

                        //    string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                        //    string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                        //    XmlDocument itemDoc = new XmlDocument();
                        //     XmlText = new XmlTextReader(sXmlPath);
                        //    itemDoc.Load(XmlText);
                        //    int iAge = 0;
                        //    iAge = UtilityManager.CalculateAge(ListToSaveHuman[0].Birth_Date);

                        //    XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
                        //    if (xmlAge != null && xmlAge.Count > 0)
                        //        xmlAge[0].Attributes[0].Value = iAge.ToString();
                        //    else
                        //    {
                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
                        //    }

                        //    XmlText.Close();
                        //    //itemDoc.Save(strXmlHumanFilePath);
                        //    int trycount = 0;
                        //trytosaveagain:
                        //    try
                        //    {
                        //        itemDoc.Save(strXmlHumanFilePath);
                        //    }
                        //    catch (Exception xmlexcep)
                        //    {
                        //        trycount++;
                        //        if (trycount <= 3)
                        //        {
                        //            int TimeMilliseconds = 0;
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                        //                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                        //            Thread.Sleep(TimeMilliseconds);
                        //            string sMsg = string.Empty;
                        //            string sExStackTrace = string.Empty;

                        //            string version = "";
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        //                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                        //            string[] server = version.Split('|');
                        //            string serverno = "";
                        //            if (server.Length > 1)
                        //                serverno = server[1].Trim();

                        //            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                        //                sMsg = xmlexcep.InnerException.Message;
                        //            else
                        //                sMsg = xmlexcep.Message;

                        //            if (xmlexcep != null && xmlexcep.StackTrace != null)
                        //                sExStackTrace = xmlexcep.StackTrace;

                        //            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        //            string ConnectionData;
                        //            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        //            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                        //            {
                        //                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                        //                {
                        //                    cmd.Connection = con;
                        //                    try
                        //                    {
                        //                        con.Open();
                        //                        cmd.ExecuteNonQuery();
                        //                        con.Close();
                        //                    }
                        //                    catch
                        //                    {
                        //                    }
                        //                }
                        //            }
                        //            goto trytosaveagain;
                        //        }
                        //    }

                        //}
                        ClientSession.HumanId = ListToSaveHuman[0].Id;
                    }

                    btnSave.Enabled = false;
                    //CAP-1513 & CAP-1531
                    btnSave.CssClass = "aspresizedgreenbutton";

                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("used by another process"))
                {
                    if (iTryCount < 3)
                    {
                        iTryCount++;
                        Thread.Sleep(2000);
                        goto retry;
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (ex.Message.ToLower().Contains("there is an unclosed literal string") == true || ex.Message.ToLower().Contains("root element is missing") == true || ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                    {
                        if (XmlText != null)
                            XmlText.Close();
                        UtilityManager.GenerateXML(ListToUpdateHuman[0].Id.ToString(), "Human");
                        goto retry;
                    }
                }

            }



            IList<Encounter> lst = new List<Encounter>();
            if (hdnEncounterID.Value != "" && Convert.ToUInt64(hdnEncounterID.Value) != 0)
            {
                EncounterManager obj = new EncounterManager();

                IList<Encounter> savelst = new List<Encounter>();
                lst = obj.GetEncounterByEncounterID(Convert.ToUInt64(hdnEncounterID.Value));
                Encounter objE = new Encounter();
                objE = lst[0];
                ClientSession.FillEncounterandWFObject.EncRecord = objE;
                ViewState["apptdatetime"] = objE.Appointment_Date;
                if (lst.Count > 0)
                {
                    lst[0].Authorization_Number = txtauthnumber.Text;
                    try
                    {
                        lst[0].Auth_Insurance_Plan_ID = Convert.ToInt32(ddlauthinsplan.SelectedValue);
                    }
                    catch
                    {
                        lst[0].Auth_Insurance_Plan_ID = 0;
                    }
                    lst[0].Valid_From = txtauthValidfrom.Text;
                    lst[0].Valid_To = txtauthvalidTo.Text;

                    obj.SaveUpdateDelete_DBAndXML_WithTransaction(ref savelst, ref lst, null, string.Empty, false, false, 0, string.Empty);

                }
            }

            if (hdnEncounterID.Value != "0" && hdnEncounterID.Value != "")
            {
                //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
                //{
                    IList<User> lstnew = new List<User>();
                    UserManager objnew = new UserManager();
                    lstnew = objnew.getUserByPHYID(Convert.ToUInt32(lst[0].Encounter_Provider_ID));
                    if (lstnew.Count > 0)
                    {
                        //if (txttestappear.Text!=string.Empty)
                        //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToUpper().Trim() == lst[0].Facility_Name.ToUpper().Trim())
                        var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == lst[0].Facility_Name select f;
                        IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                        if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                        {
                            WFObjectManager WFMngr = new WFObjectManager();
                            WFMngr.UpdateOwner(Convert.ToUInt32(hdnEncounterID.Value), "DOCUMENTATION", lstnew[0].user_name, string.Empty);
                        }
                    }
                //}
            }
            bFormclose = false;
            bClose = false;
            bValidPaymentInfo = false;
            SaveTime.Stop();
            lblSave.Text = "Save Time -Minutes: " + SaveTime.Elapsed.Minutes.ToString() + " Seconds :" + SaveTime.Elapsed.Seconds.ToString() + " MilliSec : " + SaveTime.Elapsed.Milliseconds.ToString();
            // Response.Write("<script>self.close()</script>");
            divLoading.Style.Add("display", "none");
            //code committed by balaji 20141/11/29
            string args = "";
            if (hdnScreenMode.Value.ToUpper() == "EDIT NAME" || hdnScreenMode.Value.ToUpper() == "CHECKEDIN" || hdnScreenMode.Value.ToUpper() == "SCANNING" || hdnIsmailsend.Value == "true")
            {
                if (ListToUpdateHuman != null && ListToUpdateHuman.Count > 0)
                {

                    args = ListToUpdateHuman[0].Id + "|" +
                    ListToUpdateHuman[0].Last_Name + "|" +
                    ListToUpdateHuman[0].First_Name + "|" +
                    ListToUpdateHuman[0].MI + "|" +
                    ListToUpdateHuman[0].Birth_Date.ToString("dd-MMM-yyyy") + "|" +
                    ListToUpdateHuman[0].Encounter_Provider_ID + "|" +
                    ListToUpdateHuman[0].Cell_Phone_Number + "|" +
                    ListToUpdateHuman[0].Home_Phone_No + "|" +
                    ListToUpdateHuman[0].Human_Type + "|" +
                    ListToUpdateHuman[0].Sex;
                }
            }
            else
            {
                if (ListToSaveHuman != null && ListToSaveHuman.Count > 0)
                {

                    args = ListToSaveHuman[0].Id + "|" +
                    ListToSaveHuman[0].Last_Name + "|" +
                    ListToSaveHuman[0].First_Name + "|" +
                    ListToSaveHuman[0].MI + "|" +
                    ListToSaveHuman[0].Birth_Date.ToString("dd-MMM-yyyy") + "|" +
                    ListToSaveHuman[0].Encounter_Provider_ID + "|" +
                    ListToSaveHuman[0].Cell_Phone_Number + "|" +
                    ListToSaveHuman[0].Home_Phone_No + "|" +
                    ListToSaveHuman[0].Human_Type + "|" +
                    ListToSaveHuman[0].Sex;
                }

            }
            LoadPatientDetails();
            LoadEligibilityVerification();
            if (flag == 1)
            {
                HumanManager objhumanmanager = new HumanManager();
                objhumanmanager.SendingMail(ListToSaveHuman[0]);
            }
            if (hdnScreenMode.Value.ToUpper() != "ELIGIBILITY")
            {

                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "showVal", "  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}SaveAuthEncounter();RefreshNotification('Demographics');Exit('" + args.Replace("'", "&apos") + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "showVal", "  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('380018');SaveAuthEncounter();RefreshNotification('Demographics');", true);
                btnSave.Enabled = false;
                btnSave.CssClass = "aspresizedgreenbutton";
            }

        }
        //ma process
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var bCheckInToMa = ((Button)sender).Text != "Check In to Provider";
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                Response.Redirect("~/frmSessionExpired.aspx");

            }
            checkin(bCheckInToMa);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "EV", "setCPtvalue();", true);

        }

        //physician
        //protected void btnSaveandMove_Click(object sender, EventArgs e)
        //{
        //    if (ClientSession.UserName == string.Empty)
        //    {
        //        HttpContext.Current.Response.StatusCode = 999;
        //        HttpContext.Current.Response.Status = "999 Session Expired";
        //        HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
        //        Response.Redirect("~/frmSessionExpired.aspx");

        //    }
        //    checkin();
        //    // EncounterManager obj = new EncounterManager();
        //    EncounterManager objEncounterManager = new EncounterManager();
        //    IList<Encounter> savelst = new List<Encounter>();
        //    //  savelst = obj.GetEncounterByEncounterID(Convert.ToUInt64(hdnEncounterID.Value));
        //    ulong EncProviderID = 0;
        //    string FacilityName = ClientSession.FacilityName;
        //    WFObject lstwfobject = new WFObject();
        //    //For bug id 56836
        //    string sCurrentTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        //    string sLocal_Time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        //    DateTime dtDateofservice = new DateTime();
        //    DateTime dtApptdate = new DateTime();

        //    if (ViewState["apptdatetime"] != null)
        //        dtApptdate = Convert.ToDateTime(ViewState["apptdatetime"]);

        //    if (sCurrentTime != string.Empty && Convert.ToDateTime(sCurrentTime.Split(' ')[0]) == Convert.ToDateTime(dtApptdate.ToString().Split(' ')[0]))
        //    {
        //        dtDateofservice = UtilityManager.ConvertToUniversal(DateTime.Now);
        //        sLocal_Time = UtilityManager.ConvertToLocal(dtDateofservice).ToString("yyyy-MM-dd hh:mm:ss tt");
        //    }
        //    else
        //    {
        //        dtDateofservice = dtApptdate;
        //        sLocal_Time = UtilityManager.ConvertToLocal(dtApptdate).ToString("yyyy-MM-dd hh:mm:ss tt");
        //    }
        //    lstwfobject = objEncounterManager.UpdateEncounterforCheckin(Convert.ToUInt64(hdnEncounterID.Value), ClientSession.UserName, ClientSession.UserName, UtilityManager.ConvertToUniversal(DateTime.Now),
        //  dtDateofservice, ClientSession.CurrentObjectType, "", string.Empty, sLocal_Time, ref  EncProviderID, ref FacilityName);

        //    if (txtPatientAccountNo.Text != "")
        //        ClientSession.HumanId = Convert.ToUInt64(txtPatientAccountNo.Text);
        //    else
        //        ClientSession.HumanId = Convert.ToUInt64(hdnHumanID.Value);

        //    ClientSession.EncounterId = Convert.ToUInt64(hdnEncounterID.Value);
        //    ClientSession.UserCurrentProcess = lstwfobject.Current_Process;

        //    frmRCopiaToolbar.LoadNotification("All");

        //    ClientSession.EncounterId = 0;
        //    ClientSession.HumanId = 0;
        //    ClientSession.FillEncounterandWFObject = null;
        //    ClientSession.UserCurrentProcess = "";
        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveAuth", "SaveAuthEncounter();", true);

        //}

        public Boolean AmountValidation(string Amount)
        {
            /*Check whether entered amount is greater than 9999999.99 and throw a error message*/
            if (Convert.ToDouble(Amount) > 9999999.99)
            {

                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380019');", true);
                return false;
            }
            else
            {
                return true;
            }
        }
        public Boolean PhNoValid(string sPhno)
        {

            string sReplace = sPhno.Replace("_", "");

            if (sReplace.Length < 13)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool CheckEmailDuplicate()
        {

            Human humanRec = HumanMngr.GetHumanIfDuplicateEMail(txtMail.Text);
            FillQuickPatient objCheckOut = null;
            Human humanLoadRecord = null;
            if (hdnEncounterID.Value != string.Empty && hdnEncounterID.Value != "0")
            {
                objCheckOut = HumanMngr.LoadQuickPatient(Convert.ToUInt64(hdnEncounterID.Value));
            }
            if (objCheckOut != null && objCheckOut.HumanObj != null)
            {
                humanLoadRecord = objCheckOut.HumanObj;
            }
            Human objHuman = new Human();

            if (objCheckOut != null)
            {
                objHuman = objCheckOut.HumanObj;
            }
            if (objCheckOut == null)
            {
                if (humanLoadRecord != null)
                {
                    objHuman = humanLoadRecord;
                }
            }

            if (humanRec == null)
            {
                return true;
            }
            else
            {
                if (objHuman.Id != humanRec.Id)
                {
                    return false;
                }
                return true;
            }
        }
        public Boolean ZipCodeValid(string sPhno)
        {
            string pattern = @"^\d{5}-(\d{4})?$";
            Regex match = new Regex(pattern);
            return match.IsMatch(sPhno);
        }
        public string CheckHumanDuplicate()
        {
            string sAccountExtNo = string.Empty;
            if (txtExternalAccNo.Text != "999999999")
            {
                sAccountExtNo = txtExternalAccNo.Text;
            }
            if (dtpPatientDOB.Text.Substring(0, 2) == "00")
                return "DisplayErrorMessage('380002');";
            else if (dtpPatientDOB.Text.Substring(5, 4) != "" && Convert.ToInt32(dtpPatientDOB.Text.Substring(5, 4)) < 1900) //== "0000" || dtpPatientDOB.Text.Substring(7, 11) == "0001")
                return "DisplayErrorMessage('380002');";
            HumanDTO CheckHuman = new HumanDTO();
            //Cap -1883
            //CheckHuman = HumanMngr.GetPatientDetailsUsingPatientDetails(txtPatientLastName.Text, txtPatientFirstName.Text, Convert.ToDateTime(dtpPatientDOB.Text), cboPatientSex.Text, txtMedicalRecordNo.Text, sAccountExtNo);
            CheckHuman = HumanMngr.GetPatientDetailsUsingPatientDetails(txtPatientLastName.Text, txtPatientFirstName.Text, Convert.ToDateTime(dtpPatientDOB.Text), cboPatientSex.Text, txtMedicalRecordNo.Text, sAccountExtNo, ClientSession.LegalOrg);
            int iSave = 1;
            ulong ulDuplicateID = 0;
            if (hdnHumanID.Value != string.Empty)
            {
                ulDuplicateID = Convert.ToUInt64(hdnHumanID.Value);
            }
            /*Throws a message if Patient is already available.*/
            if (CheckHuman.HumanDetails != null && CheckHuman.HumanDetails.Id != ulDuplicateID)
            {
                divLoading.Style.Add("display", "none");
                return "ConfirmHumanDuplicate();";
            }
            if (iSave == 2)
            {
                txtPatientLastName.Focus();
                return "ReSave";
            }
            if (CheckHuman.MedicalRecordNoList == true && CheckHuman.ulMedicalRecordID != ulDuplicateID)
            {
                txtMedicalRecordNo.Focus();
                divLoading.Style.Add("display", "none");
                return "DisplayErrorMessage('380027');";
            }
            if (CheckHuman.Patient_Account_External == true && CheckHuman.HumanDetails != null && CheckHuman.HumanDetails.Id != ulDuplicateID)
            {

                txtExternalAccNo.Focus();
                divLoading.Style.Add("display", "none");
                return "DisplayErrorMessage('380030');";
            }
            return "";
        }

        protected void chkEligibilityVerified_CheckedChanged(object sender, EventArgs e)
        {
            bClose = true;

            if (chkEligibilityVerified.Checked == true)
            {

                if (grdExistingPolicies.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('330020'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    txtEligibilityVerificationDate.Text = string.Empty;
                    chkEligibilityVerified.Checked = false;
                }
                else
                {

                    cboVerificationType.Items.Remove("ELECTRONIC");
                    spanEffectiveDate.Attributes.Remove("class");

                    spanEffectiveDate.Attributes.Add("class", "MandLabelstyle");

                    spanPolicyID.Attributes.Remove("class");

                    spanPolicyID.Attributes.Add("class", "MandLabelstyle");

                    spanPayername.Attributes.Remove("class");

                    spanPayername.Attributes.Add("class", "MandLabelstyle");

                    spanPlanName.Attributes.Remove("class");

                    spanPlanName.Attributes.Add("class", "MandLabelstyle");


                    TextBoxColorChange(txtPolicyHolderID, true);


                    spanPayerstar.Visible = true;
                    spanPlanstar.Visible = true;
                    spanPolicyIDstar.Visible = true;
                    spanEffDatestar.Visible = true;
                    /*Method to enable Policy Information group box*/
                    PolicyInformationEnableall();
                    TextBoxColorChange(txtEligibilityVerificationDate, false);
                    txtErrorMessage.Text = string.Empty;
                    txtErrorMessage.Enabled = false;
                    TextBoxColorChange(txtStreet, false);
                    txtStreet.Enabled = false;
                    TextBoxColorChange(txtClaimCity2, false);
                    txtClaimCity2.Enabled = false;
                    TextBoxColorChange(txtClaimMailingName, false);
                    txtClaimMailingName.Enabled = false;
                    chkShowAllEV.Enabled = true;
                    chkShowAllEVPlan.Enabled = true;

                    DateTime dt = Convert.ToDateTime(hdnLocalTime.Value);
                    txtEligibilityVerificationDate.Text = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy");


                }

            }
            else
            {
                if (grdExistingPolicies.Rows.Count > 0 && grdExistingPolicies.SelectedRow != null)
                {
                    if (grdExistingPolicies.SelectedRow.Cells[19].Text == "ELECTRONIC")
                    {
                        cboVerificationType.Items.Add("ELECTRONIC");
                        cboVerificationType.SelectedValue = "ELECTRONIC";
                    }
                    else
                    {
                        if (grdExistingPolicies.SelectedRow.Cells[19].Text == "&nbsp;")
                        {
                            cboVerificationType.SelectedValue = "";
                        }
                        else
                        {
                            cboVerificationType.SelectedValue = grdExistingPolicies.SelectedRow.Cells[19].Text;
                        }

                    }
                }
                EligibilityDisabled();
                chkShowAllEV.Enabled = false;
                chkShowAllEVPlan.Enabled = false;

                txtEligibilityVerificationDate.Text = string.Empty;
                TextBoxColorChange(txtEligibilityVerificationDate, false);

                TextBoxColorChange(txtPolicyHolderID, false);


                spanEffectiveDate.Attributes.Remove("class");

                spanEffectiveDate.Attributes.Add("class", "spanstyle");

                spanPolicyID.Attributes.Remove("class");

                spanPolicyID.Attributes.Add("class", "spanstyle");

                spanPayername.Attributes.Remove("class");

                spanPayername.Attributes.Add("class", "spanstyle");

                spanPlanName.Attributes.Remove("class");

                spanPlanName.Attributes.Add("class", "spanstyle");

                spanPayerstar.Visible = false;
                spanPlanstar.Visible = false;
                spanPolicyIDstar.Visible = false;
                spanEffDatestar.Visible = false;
                txtErrorMessage.Text = string.Empty;
                string human_id = "";
                human_id = txtPatientAccountNo.Text;
                IList<Eligibility_Verification> list = new List<Eligibility_Verification>();
                Eligibility_VerficationManager objc = new Eligibility_VerficationManager();
                list = objc.GetDateUsingEligibilityVerification(Convert.ToUInt64(human_id));

                if (list.Count > 0)
                {
                    if (list[0].Requested_For_From_Date != null && list[0].Requested_For_To_Date != null)
                    {
                        if (list[0].Requested_For_From_Date.ToString("dd-MMM-yyyy") == list[0].Requested_For_To_Date.ToString("dd-MMM-yyyy"))
                        {
                            txtEligibilityVerificationDate.Text = list[0].Requested_For_From_Date.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            txtEligibilityVerificationDate.Text = list[0].Requested_For_From_Date.ToString("dd-MMM-yyyy") + " to " + list[0].Requested_For_To_Date.ToString("dd-MMM-yyyy");
                        }
                    }
                    if (txtEligibilityVerificationDate.Text == "01-Jan-0001")
                    {
                        txtEligibilityVerificationDate.Text = string.Empty;
                    }
                }
                else
                {
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        DateTime dt = Convert.ToDateTime(hdnLocalTime.Value);
                        txtEligibilityVerificationDate.Text = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy");
                    }

                }

            }

            btnSave.Enabled = true;
            btnSave.CssClass = "aspresizedgreenbutton";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "QuickpatientCreate", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        void EligibilityDisabled()
        {
            PolicyInformationDisableall();
        }

        public void PolicyInformationEnableall()
        {
            tablelayoutenable(gbEligibilityVerification);

        }

        private void ChangeLabelStyle(Label lbl, Boolean bIstoNormal)
        {
            if (bIstoNormal == true)
            {
                lbl.Text = lbl.Text.Replace("*", "");
                lbl.ForeColor = Color.Black;
                lbl.CssClass = lbl.CssClass.Replace("MandLabelstyle", "spanstyle");
            }
            else
            {
                if (lbl.Text.Contains("*") == false)
                {
                    lbl.Text = lbl.Text + "*";
                    lbl.CssClass = lbl.CssClass.Replace("spanstyle", "MandLabelstyle");
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ChangeLabel", "warningmethod();", true);
        }

        public void PolicyInformationDisableall()
        {
            tablelayoutdisable(gbEligibilityVerification);
        }

        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            // bool successFlag = false;
            btnSendMail.Enabled = false; /****/

            if (chkOnlineAccess.Checked == true)
            {
                if (txtMail.Text.Trim() == string.Empty)
                {

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380038'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    txtMail.Focus();
                    btnSendMail.Enabled = true;
                    return;
                }

                else
                {
                    bSendMailClick = true;
                    FillQuickPatient objCheckOut = null;
                    Human humanLoadRecord = null;
                    if (hdnEncounterID.Value != string.Empty && hdnEncounterID.Value != "0")
                    {
                        objCheckOut = HumanMngr.LoadQuickPatient(Convert.ToUInt64(hdnEncounterID.Value));
                    }
                    if (objCheckOut != null && objCheckOut.HumanObj != null)
                    {
                        humanLoadRecord = objCheckOut.HumanObj;
                    }
                    if (humanLoadRecord == null)
                    {
                        if (hdnScreenMode.Value.ToUpper() != "CHECKEDIN" && txtPatientAccountNo.Text == string.Empty)
                        {
                            flag = 1;
                            btnSave_Click(sender, e);
                        }
                    }
                    else
                    {
                        if (txtMail.Text != string.Empty)
                        {
                            if (Convert.ToString(ViewState["email"]) != txtMail.Text)
                            {
                                if (CheckEmailDuplicate() == false)
                                {

                                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380040'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                    txtMail.Focus();
                                    chkOnlineAccess.Checked = false;
                                    hdnIsmailsend.Value = "";
                                    txtMail.Text = string.Empty;
                                    return;
                                }
                            }
                        }

                        Human objHumanRecord = new Human();

                        if (objCheckOut != null && humanLoadRecord == null)
                        {
                            objHumanRecord = objCheckOut.HumanObj;
                        }
                        else if (humanLoadRecord != null)
                        {
                            objHumanRecord = humanLoadRecord;
                        }
                        objHumanRecord.First_Name = txtPatientFirstName.Text;
                        objHumanRecord.Last_Name = txtPatientLastName.Text;
                        objHumanRecord.MI = txtPatientMI.Text;
                        if (cboPatientSex.Text.Trim() != string.Empty)
                            objHumanRecord.Sex = cboPatientSex.Text;
                        objHumanRecord.Legal_Org = ClientSession.LegalOrg;
                            
                        objHumanRecord.Patient_Account_External = txtExternalAccNo.Text;
                        objHumanRecord.Medical_Record_Number = txtMedicalRecordNo.Text;

                        if (msktxtSSN.TextWithLiterals == "--")
                        {
                            objHumanRecord.SSN = string.Empty;
                        }
                        else if (msktxtSSN.TextWithLiterals != "--")
                        {
                            objHumanRecord.SSN = msktxtSSN.TextWithLiterals;
                        }
                        if (msktxtCellPhno.TextWithLiterals != "() -")
                        {
                            objHumanRecord.Cell_Phone_Number = msktxtCellPhno.TextWithLiterals;
                        }
                        else if (msktxtCellPhno.TextWithLiterals == "() -")
                        {
                            objHumanRecord.Cell_Phone_Number = string.Empty;
                        }
                        if (msktxtHomePhno.TextWithLiterals != "() -")
                        {
                            objHumanRecord.Home_Phone_No = msktxtHomePhno.TextWithLiterals;
                        }
                        else if (msktxtHomePhno.TextWithLiterals == "() -")
                        {
                            objHumanRecord.Home_Phone_No = string.Empty;
                        }
                        if (chkOnlineAccess.Checked == true)
                        {
                            objHumanRecord.Is_Mail_Sent = "Y";
                            if (hdnLocalTime.Value != string.Empty)
                            {
                                objHumanRecord.Mail_Sent_Date = Convert.ToDateTime(hdnLocalTime.Value);
                            }
                        }
                        else
                        {
                            objHumanRecord.Is_Mail_Sent = "N";
                        }
                        if (msktxtZipcode.TextWithLiterals != "-")
                        {
                            if (msktxtZipcode.TextWithLiterals.Length == 6 && msktxtZipcode.TextWithLiterals.Length < 10)
                            {
                                string[] Split = Convert.ToString(msktxtZipcode.TextWithLiterals).Split('-');
                                if (Split.Length == 2 && Split[1] == string.Empty)
                                {
                                    objHumanRecord.ZipCode = Split[0].ToString();
                                }
                            }
                            else
                            {

                                objHumanRecord.ZipCode = msktxtZipcode.TextWithLiterals;
                            }
                        }
                        else
                        {
                            objHumanRecord.ZipCode = string.Empty;
                        }

                        objHumanRecord.Preferred_Confidential_Correspodence_Mode = cboPreferredConfidentialCoreespondenceMode.SelectedItem.Text;
                        objHumanRecord.EMail = txtMail.Text;
                        objHumanRecord.Password = string.Empty;
                        ListToUpdateHuman.Add(objHumanRecord);


                        if (ListToUpdateHuman.Count > 0)
                        {
                            humanLoadRecord = HumanMngr.UpdateToHuman(ListToUpdateHuman[0], string.Empty);
                            ListToUpdateHuman.Clear();
                        }

                        else
                        {

                            btnSendMail.Enabled = true;
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380048'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                if (chkOnlineAccess.Checked)
                    hdnIsmailsend.Value = "true";
            }
        }

        protected void ddlPlanName_SelectedIndexChanged(object sender, EventArgs e) /*change 2 txtbx*/
        {
            if (ddlPlanName.SelectedIndex != 0)
            {


                if (ddlPayerName.SelectedItem.Text == "OTHER" && ddlPlanName.SelectedItem.Text == "OTHER")
                {

                    txtClaimMailingName.Text = string.Empty;
                    txtClaimCity.Text = string.Empty;
                    txtClaimCity2.Text = string.Empty;
                    msktxtZipcode.Text = string.Empty;

                    txtClaimState.Text = string.Empty;
                }
                else if (ddlPayerName.SelectedItem.Text != "OTHER")
                {
                    IList<InsurancePlan> patInsList = (IList<InsurancePlan>)Session["PatInsuredList"];
                    var insplan = from ip in patInsList where ip.Id == Convert.ToUInt64(ddlPlanName.Items[ddlPlanName.SelectedIndex].Value) select ip;
                    IList<InsurancePlan> insplanlist = (insplan.ToList<InsurancePlan>().OrderBy(s => s.Ins_Plan_Name)).ToList<InsurancePlan>();

                    if (insplanlist != null && insplanlist.Count > 0)
                    {
                        txtStreet.Text = insplanlist[0].Claim_Address;
                        txtClaimCity.Text = insplanlist[0].Claim_City;
                        //msktxtZipcode.Text = insplanlist[0].Claim_ZipCode;
                        txtClaimCity2.Text = insplanlist[0].Claim_City;
                        txtClaimState.Text = insplanlist[0].Claim_State;

                    }
                }
            }
        }

        public void delete_SelectedRow()
        {
            //GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            int Rowindex = grdPaymentInformation.SelectedRow.RowIndex;
            // grdPaymentInformation.SelectedIndex = Rowindex;
            ulong Check = 0;
            ulong PPHeaderId = 0;
            ulong PPLineId = 0;
            ulong VisitID = 0;
            if (grdPaymentInformation.Rows[Rowindex] != null)
            {
                VisitID = Convert.ToUInt64(grdPaymentInformation.Rows[Rowindex].Cells[11].Text);
                PPHeaderId = Convert.ToUInt64(grdPaymentInformation.Rows[Rowindex].Cells[12].Text);
                PPLineId = Convert.ToUInt64(grdPaymentInformation.Rows[Rowindex].Cells[13].Text);
                Check = Convert.ToUInt64(grdPaymentInformation.Rows[Rowindex].Cells[14].Text);
            }
            //if (ddlFacilitywithDOS.SelectedItem.Value.Split('|')[2].ToString().ToUpper() == "MAIN")
            //{

            SaveVisitPaymentList = new List<VisitPayment>();

            HumanManager HumanMngr = new HumanManager();
            FillQuickPatient objcheckout = HumanMngr.GetVisitPaymentDetails(grdPaymentInformation.Rows[Rowindex].Cells[11].Text, grdPaymentInformation.Rows[Rowindex].Cells[12].Text, grdPaymentInformation.Rows[Rowindex].Cells[13].Text, grdPaymentInformation.Rows[Rowindex].Cells[14].Text);
            if (objcheckout.VisitPaymentList.Count > 0 && objcheckout.VisitPaymentList[0] != null)
            {
                objcheckout.VisitPaymentList[0].Is_Delete = "Y";
                objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                objcheckout.VisitPaymentList[0].Modified_By = ClientSession.UserName;
                objcheckout.VisitPaymentList[0].Payment_Note = txtPaymentNote.Text;
                SaveVisitPaymentList.Add(objcheckout.VisitPaymentList[0]);
            }
            PPHeaderManager PPHeaderMngr = new PPHeaderManager();
            SavePPHeaderList = new List<PPHeader>();
            if (objcheckout.PPHeaderList.Count > 0 && objcheckout.PPHeaderList[0] != null)
            {
                objcheckout.PPHeaderList[0].Is_Delete = "Y";
                objcheckout.PPHeaderList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                objcheckout.PPHeaderList[0].Modified_By = ClientSession.UserName;
                SavePPHeaderList.Add(objcheckout.PPHeaderList[0]);
            }
            PPLineItemManager PPLineMngr = new PPLineItemManager();
            SavePPLineItemList = new List<PPLineItem>();
            if (objcheckout.PPLineItemList.Count > 0 && objcheckout.PPLineItemList[0] != null)
            {
                objcheckout.PPLineItemList[0].Is_Delete = "Y";
                objcheckout.PPLineItemList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                objcheckout.PPLineItemList[0].Modified_By = ClientSession.UserName;
                SavePPLineItemList.Add(objcheckout.PPLineItemList[0]);
            }
            CheckManager CheckMngr = new CheckManager();
            SaveCheckList = new List<Check>();
            if (objcheckout.CheckList.Count > 0 && objcheckout.CheckList[0] != null)
            {
                objcheckout.CheckList[0].Is_Delete = "Y";
                objcheckout.CheckList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                objcheckout.CheckList[0].Modified_By = ClientSession.UserName;
                SaveCheckList.Add(objcheckout.CheckList[0]);
            }


            AccountTransactionManager AccTranMngr = new AccountTransactionManager();
            IList<AccountTransaction> ilistAccTran = new List<AccountTransaction>();
            if (objcheckout != null && objcheckout.AccountTransaction.Count > 0)
            {
                for (int iNumber = 0; iNumber < ilistAccTran.Count; iNumber++)
                {
                    objcheckout.AccountTransaction[iNumber].Is_Delete = "Y";
                    objcheckout.AccountTransaction[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objcheckout.AccountTransaction[iNumber].Modified_By = ClientSession.UserName;
                }
            }

            VisitPaymentHistoryManager VisitPaymentHistoryMngr = new VisitPaymentHistoryManager();
            SaveVisitPaymenHistoryList = new List<VisitPaymentHistory>();
            if (objcheckout != null && objcheckout.VisitPaymentHistoryList.Count > 0)
            {
                objcheckout.VisitPaymentHistoryList[0].Payment_Note = txtPaymentNote.Text;
                objcheckout.VisitPaymentHistoryList[0].Is_Delete = "Y";
                objcheckout.VisitPaymentHistoryList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                objcheckout.VisitPaymentHistoryList[0].Modified_By = ClientSession.UserName;
                SaveVisitPaymenHistoryList.Add(objcheckout.VisitPaymentHistoryList[0]);

                FillVisitPaymentHistory(objcheckout.VisitPaymentList[0], null, "DEBIT");
            }

            IList<VisitPaymentDTO> VisitPaymentDTO = new List<VisitPaymentDTO>();
            if (hdnEncounterID.Value != "")
                VisitPaymentDTO = visitMgr.UpdateVisitPayment(SaveVisitPaymentList, SaveCheckList, SavePPHeaderList, SavePPLineItemList, ilistAccTran, SaveAccountTransactionList, SaveVisitPaymenHistoryList, Convert.ToUInt32(hdnEncounterID.Value));



            bValidPaymentInfo = true;
            SaveVisitPaymentList.Clear();
            LoadPaymentInfoGrid(VisitPaymentDTO);
            paymentinformationdisableall();
            PaymentInformationClearAll();
            cboMethodOfPayment.SelectedIndex = 0;
            btnAdd.Text = "Add";
            btnClear.Text = "Clear All";
            if (grdPaymentInformation.Rows.Count == 1 && grdPaymentInformation.Rows[0].Visible == false)
            {
                txtTotalAmount.Text = "0.00";
            }
            //}
            //else
            //{
            //    SaveVisitPaymentArcList = new List<VisitPaymentArc>();

            //    HumanManager HumanMngr = new HumanManager();
            //    FillQuickPatientArc objcheckout = HumanMngr.GetVisitPaymentDetailsArc(grdPaymentInformation.Rows[Rowindex].Cells[11].Text, grdPaymentInformation.Rows[Rowindex].Cells[12].Text, grdPaymentInformation.Rows[Rowindex].Cells[13].Text, grdPaymentInformation.Rows[Rowindex].Cells[14].Text);
            //    if (objcheckout.VisitPaymentList.Count > 0 && objcheckout.VisitPaymentList[0] != null)
            //    {
            //        objcheckout.VisitPaymentList[0].Is_Delete = "Y";
            //        objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            //        objcheckout.VisitPaymentList[0].Modified_By = ClientSession.UserName;
            //        SaveVisitPaymentArcList.Add(objcheckout.VisitPaymentList[0]);
            //    }
            //    //PPHeaderManager PPHeaderMngr = new PPHeaderManager();
            //    SavePPHeaderArcList = new List<PPHeaderArc>();
            //    if (objcheckout.PPHeaderList.Count > 0 && objcheckout.PPHeaderList[0] != null)
            //    {
            //        objcheckout.PPHeaderList[0].Is_Delete = "Y";
            //        objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            //        objcheckout.PPHeaderList[0].Modified_By = ClientSession.UserName;
            //        SavePPHeaderArcList.Add(objcheckout.PPHeaderList[0]);
            //    }
            //    //PPLineItemManager PPLineMngr = new PPLineItemManager();
            //    SavePPLineItemArcList = new List<PPLineItemArc>();
            //    if (objcheckout.PPLineItemList.Count > 0 && objcheckout.PPLineItemList[0] != null)
            //    {
            //        objcheckout.PPLineItemList[0].Is_Delete = "Y";
            //        objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            //        objcheckout.PPLineItemList[0].Modified_By = ClientSession.UserName;
            //        SavePPLineItemArcList.Add(objcheckout.PPLineItemList[0]);
            //    }
            //    //CheckManager CheckMngr = new CheckManager();
            //    SaveCheckArcList = new List<CheckArc>();
            //    if (objcheckout.CheckList.Count > 0 && objcheckout.CheckList[0] != null)
            //    {
            //        objcheckout.CheckList[0].Is_Delete = "Y";
            //        objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            //        objcheckout.CheckList[0].Modified_By = ClientSession.UserName;
            //        SaveCheckArcList.Add(objcheckout.CheckList[0]);
            //    }


            //    //AccountTransactionManager AccTranMngr = new AccountTransactionManager();
            //    IList<AccountTransactionArc> ilistAccTran = new List<AccountTransactionArc>();
            //    if (objcheckout != null && objcheckout.AccountTransaction.Count > 0)
            //    {
            //        for (int iNumber = 0; iNumber < ilistAccTran.Count; iNumber++)
            //        {
            //            objcheckout.AccountTransaction[iNumber].Is_Delete = "Y";
            //            objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            //            objcheckout.AccountTransaction[iNumber].Modified_By = ClientSession.UserName;
            //        }
            //    }
            //    IList<VisitPaymentDTO> VisitPaymentDTO = new List<VisitPaymentDTO>();
            //    if (hdnEncounterID.Value != "")
            //        VisitPaymentDTO = visitArcMgr.UpdateVisitPaymentArc(SaveVisitPaymentArcList, SaveCheckArcList, SavePPHeaderArcList, SavePPLineItemArcList, objcheckout.AccountTransaction, null, null, Convert.ToUInt32(hdnEncounterID.Value));


            //    bValidPaymentInfo = true;
            //    SaveVisitPaymentList.Clear();
            //    LoadPaymentInfoGrid(VisitPaymentDTO);
            //    paymentinformationdisableall();
            //    PaymentInformationClearAll();
            //    cboMethodOfPayment.SelectedIndex = 0;
            //    btnAdd.Text = "Add";
            //    btnClear.Text = "Clear All";
            //    if (grdPaymentInformation.Rows.Count == 1 && grdPaymentInformation.Rows[0].Visible == false)
            //    {
            //        txtTotalAmount.Text = "0.00";
            //    }
            //}
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('180037');", true);


        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            if (cboMethodOfPayment.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380042');", true);
                btnAdd.Enabled = true;
                return;
            }
            if (cboRelation.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380051');", true);
                btnAdd.Enabled = true;
                return;
            }
            if (txtpaidBy.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380052');", true);
                btnAdd.Enabled = true;
                return;
            }
            if (gbPaymentInformation.Visible == true)
            {
                if (txtCheckNo.ReadOnly == false && txtCheckNo.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380021');", true);
                    chkMultiplePayments.Checked = false;
                    txtCheckNo.Focus();
                    return;
                }

            }
            if ((txtPaymentAmount.Text == "" || txtPaymentAmount.Text == "0.00") && (txtRecOnAcc.Text == "" || txtRecOnAcc.Text == "0.00") && (txtPastDue.Text == "" || txtPastDue.Text == "0.00") && (txtRefundAmount.Text == "" || txtRefundAmount.Text == "0.00"))
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380059');", true);
                btnAdd.Enabled = true;
                return;
            }
            if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
            {
                try
                {
                    DateTime dtTemp = Convert.ToDateTime(dtpCheckDate.Text);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('102012');", true);
                    btnAdd.Enabled = true;
                    return;
                }
            }
            if (btnAdd.Text == "Confirm Delete")
            {
                if (txtPaymentNote.Text == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Delete-Alert", "alert('Please provide the reason for deletion in the payment note textbox'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
                else
                {
                    //Jira Cap - 517
                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('380060');", true);
                    delete_SelectedRow();
                    spanPaymentNotes.Attributes.Remove("class"); /*added*/
                    spanPaymentNotes.Attributes.Add("class", "spanstyle");
                    spanPatientNotestar.Visible = false;
                    btnAdd.Enabled = false;
                    cboMethodOfPayment.Enabled = true;
                    cboMethodOfPayment.CssClass = "Editabletxtbox";
                    return;
                }
            }


            if (btnAdd.Text == "Add")
            {
                VisitPayment objPayment = new VisitPayment();
                Check objCheck = new Check();
                PPHeader objPPHeader = new PPHeader();
                PPLineItem objPPLineItem = new PPLineItem();
                AccountTransaction objAccountTransaction = new AccountTransaction();
                objPayment.Facility_Name = ClientSession.FacilityName;
                if (hdnEncounterID.Value != string.Empty && hdnEncounterID.Value != "0")
                {
                    objCheckOut = HumanMngr.LoadQuickPatient(Convert.ToUInt64(hdnEncounterID.Value));
                }

                if (txtPaymentAmount.ReadOnly == false)
                {
                    objPayment.Patient_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                    objCheck.Payment_Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                    objPPHeader.Total_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                    objPPLineItem.Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                    objAccountTransaction.Amount = -(Convert.ToDecimal(txtPaymentAmount.Text));
                }

                if (txtRefundAmount.Text != string.Empty)
                {
                    objPayment.Refund_Amount = Convert.ToDecimal(txtRefundAmount.Text);
                }
                if (txtRecOnAcc.Text != string.Empty)
                    objPayment.Rec_On_Acc = Convert.ToDecimal(txtRecOnAcc.Text);

                if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
                {
                    objPayment.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                    objCheck.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                }
                else
                {
                    objPayment.Check_Date = DateTime.MinValue;
                    objCheck.Check_Date = DateTime.MinValue;
                }


                objPayment.Relationship = cboRelation.SelectedItem.Text;
                objPayment.Amount_Paid_By = txtpaidBy.Text;

                objPayment.Check_Card_No = txtCheckNo.Text;
                objPayment.Auth_No = txtAuthNo.Text;
                objPayment.Method_of_Payment = cboMethodOfPayment.Text;
                objPayment.Payment_Note = txtPaymentNote.Text;
                objPayment.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                objPayment.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                if (hdnLocalTime.Value != string.Empty)
                {
                    objPayment.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                }
                objPayment.Created_By = ClientSession.UserName;
                objPayment.Is_Delete = "N";
                SaveVisitPaymentList.Add(objPayment);
                FillVisitPaymentHistory(objPayment, null, string.Empty);

                objCheck.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                objCheck.Payment_Type = cboMethodOfPayment.Text;
                objCheck.Created_By = ClientSession.UserName;
                if (hdnLocalTime.Value != string.Empty)
                {
                    objCheck.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                }
                objCheck.Carrier_Patient_Name = txtPatientLastName.Text;
                if (hdnEncounterID.Value != string.Empty)
                {
                    objCheck.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    if (cboMethodOfPayment.Text == "Cash")
                    {
                        objCheck.Payment_ID = hdnEncounterID.Value;
                    }
                }
                if (txtCheckNo.Text != string.Empty)
                    objCheck.Payment_ID = txtCheckNo.Text;
                if (hdnCarrierId.Value != string.Empty)
                {
                    objCheck.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                }
                objCheck.Is_Delete = "N";
                SaveCheckList.Add(objCheck);

                objPPHeader.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                objPPHeader.Created_By = ClientSession.UserName;
                if (hdnLocalTime.Value != string.Empty)
                {
                    objPPHeader.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                }
                if (hdnEncounterID.Value != string.Empty)
                {
                    objPPHeader.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                }
                if (hdnCarrierId.Value != string.Empty)
                {
                    objPPHeader.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value); ;
                }
                objPPHeader.Is_Delete = "N";
                SavePPHeaderList.Add(objPPHeader);
                objPPLineItem.Claim_Type = "PATIENT";
                objPPLineItem.Line_Type = "UNAPPLIED";
                objPPLineItem.Created_By = ClientSession.UserName;
                if (hdnLocalTime.Value != string.Empty)
                {
                    objPPLineItem.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                }
                objPPLineItem.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                objPPLineItem.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                if (hdnCarrierId.Value != string.Empty)
                {
                    objPPLineItem.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                }
                objPPLineItem.Is_Delete = "N";
                SavePPLineItemList.Add(objPPLineItem);

                objAccountTransaction.Deposit_Date = UtilityManager.ConvertToLocal(objCheckOut.EncounterObj.Appointment_Date);
                objAccountTransaction.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                objAccountTransaction.Claim_Type = "PATIENT";
                objAccountTransaction.Line_Type = "UNAPPLIED";
                objAccountTransaction.Source_Type = "PP_LINE_ITEM";
                objAccountTransaction.Created_By = ClientSession.UserName;
                if (hdnLocalTime.Value != string.Empty)
                {
                    objAccountTransaction.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                }
                objAccountTransaction.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                if (hdnCarrierId.Value != string.Empty)
                {
                    objAccountTransaction.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                }
                objAccountTransaction.Is_Delete = "N";
                SaveAccountTransactionList.Add(objAccountTransaction);

                objAccountTransaction = new AccountTransaction();
                if (txtRefundAmount.Text != string.Empty && txtRefundAmount.Text != "0.00" && txtRefundAmount.Text != "0")
                {
                    objAccountTransaction.Amount = -(Convert.ToDecimal(txtRefundAmount.Text));
                    objAccountTransaction.Reversal_Refund_Category = "REFUND";
                    objAccountTransaction.Deposit_Date = UtilityManager.ConvertToLocal(objCheckOut.EncounterObj.Appointment_Date);
                    objAccountTransaction.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                    objAccountTransaction.Claim_Type = "PATIENT";
                    objAccountTransaction.Line_Type = "UNAPPLIED";
                    objAccountTransaction.Source_Type = "PP_LINE_ITEM";
                    objAccountTransaction.Created_By = ClientSession.UserName;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objAccountTransaction.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                    objAccountTransaction.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    objAccountTransaction.Is_Delete = "N";
                    //objAccountTransaction.Carrier_ID = Cash_Carrier_ID;
                    SaveAccountTransactionList.Add(objAccountTransaction);
                }

                IList<VisitPaymentDTO> VisitList;
                if (hdnEncounterID.Value != string.Empty)
                {
                    VisitList = visitMgr.SaveVisitPayment(Convert.ToUInt64(hdnEncounterID.Value), SaveVisitPaymentList.ToArray<VisitPayment>(), SaveCheckList.ToArray<Check>(),
                    SavePPHeaderList.ToArray<PPHeader>(), SavePPLineItemList.ToArray<PPLineItem>(), SaveAccountTransactionList.ToArray<AccountTransaction>(), SaveVisitPaymenHistoryList.ToArray<VisitPaymentHistory>(), string.Empty);
                    //ApplicationObject.erroHandler.DisplayErrorMessage("380034", "Quick Patient Create", this.Page);
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380034');", true);
                    bValidPaymentInfo = true;
                    SaveVisitPaymentList.Clear();
                    LoadPaymentInfoGrid(VisitList);
                    paymentinformationdisableall();
                    PaymentInformationClearAll();
                    cboMethodOfPayment.SelectedIndex = 0;

                    TextBoxColorChange(txtRecOnAcc, false);
                    TextBoxColorChange(txtRefundAmount, false);
                    TextBoxColorChange(txtpaidBy, false);
                    ComboBoxColorChange(cboRelation, false);
                    cboRelation.SelectedIndex = 0;
                    txtpaidBy.Text = txtPatientLastName.Text + ',' + txtPatientFirstName.Text;
                }
                divLoading.Style.Add("display", "none");
            }
            else
            {
                SaveVisitPaymentList = new List<VisitPayment>();
                IList<AccountTransaction> ilistAccTran = new List<AccountTransaction>();

                HumanManager HumanMngr = new HumanManager();
                FillQuickPatient objcheckout = HumanMngr.GetVisitPaymentDetails(hdnVisitID.Value, hdnPPHeaderID.Value, hdnPPLineItemID.Value, hdnCheckID.Value);

                if (hdnVisitID.Value != "")
                {
                    if (objcheckout.VisitPaymentList[0] != null)
                    {
                        FillVisitPaymentHistory(objcheckout.VisitPaymentList[0], null, "DEBIT");
                        if (txtPaymentAmount.Text != "")
                            objcheckout.VisitPaymentList[0].Patient_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                        if (txtRefundAmount.Text != string.Empty)
                            objcheckout.VisitPaymentList[0].Refund_Amount = Convert.ToDecimal(txtRefundAmount.Text);
                        if (hdnLocalTime.Value != string.Empty)
                            objcheckout.VisitPaymentList[0].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        objcheckout.VisitPaymentList[0].Modified_By = ClientSession.UserName;
                        objcheckout.VisitPaymentList[0].Facility_Name = ClientSession.FacilityName;
                        if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
                        {
                            objcheckout.VisitPaymentList[0].Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                        }
                        else
                        {
                            objcheckout.VisitPaymentList[0].Check_Date = DateTime.MinValue;
                        }

                        objcheckout.VisitPaymentList[0].Check_Card_No = txtCheckNo.Text;
                        objcheckout.VisitPaymentList[0].Auth_No = txtAuthNo.Text;
                        objcheckout.VisitPaymentList[0].Method_of_Payment = cboMethodOfPayment.Text;
                        objcheckout.VisitPaymentList[0].Payment_Note = txtPaymentNote.Text;

                        objcheckout.VisitPaymentList[0].Relationship = cboRelation.Text;
                        objcheckout.VisitPaymentList[0].Amount_Paid_By = txtpaidBy.Text;


                        if (txtRecOnAcc.Text != "")
                            objcheckout.VisitPaymentList[0].Rec_On_Acc = Convert.ToDecimal(txtRecOnAcc.Text);
                        SaveVisitPaymentList.Add(objcheckout.VisitPaymentList[0]);
                        FillVisitPaymentHistory(objcheckout.VisitPaymentList[0], null, "CREDIT");
                    }
                }
                if (hdnPPHeaderID.Value != "")
                {
                    PPHeaderManager PPHeaderMngr = new PPHeaderManager();

                    SavePPHeaderList = new List<PPHeader>();
                    if (objcheckout != null && objcheckout.PPHeaderList != null && objcheckout.PPHeaderList.Count()>0&&objcheckout.PPHeaderList[0] != null)
                    {
                        if (txtPaymentAmount.Text != "")
                            objcheckout.PPHeaderList[0].Total_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                        if (hdnLocalTime.Value != string.Empty)
                            objcheckout.PPHeaderList[0].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        objcheckout.PPHeaderList[0].Modified_By = ClientSession.UserName;
                        SavePPHeaderList.Add(objcheckout.PPHeaderList[0]);
                    }
                }
                if (hdnPPLineItemID.Value != "")
                {
                    PPLineItemManager PPLineMngr = new PPLineItemManager();
                    SavePPLineItemList = new List<PPLineItem>();
                    //PPLineItem objPPLine = PPLineMngr.GetById(Convert.ToUInt32(hdnPPLineItemID.Value));
                    if (objcheckout.PPLineItemList[0] != null)
                    {
                        if (txtPaymentAmount.Text != "")
                            objcheckout.PPLineItemList[0].Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                        if (hdnLocalTime.Value != string.Empty)
                            objcheckout.PPLineItemList[0].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        objcheckout.PPLineItemList[0].Modified_By = ClientSession.UserName;
                        SavePPLineItemList.Add(objcheckout.PPLineItemList[0]);
                    }
                    AccountTransactionManager AccTranMngr = new AccountTransactionManager();

                    if (objcheckout.AccountTransaction != null && objcheckout.AccountTransaction.Count > 0)
                    {
                        for (int iNumber = 0; iNumber < ilistAccTran.Count; iNumber++)
                        {
                            if (txtPaymentAmount.Text != "")
                                objcheckout.AccountTransaction[iNumber].Amount = -Convert.ToDecimal(txtPaymentAmount.Text);
                            if (hdnLocalTime.Value != string.Empty)
                                objcheckout.AccountTransaction[iNumber].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                            objcheckout.AccountTransaction[iNumber].Modified_By = ClientSession.UserName;
                        }
                        AccountTransaction objAccountTransaction = new AccountTransaction();
                        IList<AccountTransaction> ilistRefundTrans = objcheckout.AccountTransaction.Where(a => a.Reversal_Refund_Category == "REFUND").ToList<AccountTransaction>();
                        if (ilistRefundTrans != null && ilistRefundTrans.Count > 0)
                        {
                            if (txtRefundAmount.Text != "")
                                ilistRefundTrans[0].Amount = -Convert.ToDecimal(txtRefundAmount.Text);
                        }
                        else if (txtRefundAmount.Text != string.Empty && objcheckout.AccountTransaction.Count == 1 && txtRefundAmount.Text != "0.00" && txtRefundAmount.Text != "0")
                        {
                            objAccountTransaction.Amount = -(Convert.ToDecimal(txtRefundAmount.Text));
                            objAccountTransaction.Reversal_Refund_Category = "REFUND";
                            objAccountTransaction.Deposit_Date = objcheckout.AccountTransaction[0].Deposit_Date;
                            objAccountTransaction.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                            objAccountTransaction.Claim_Type = "PATIENT";
                            objAccountTransaction.Line_Type = "UNAPPLIED";
                            objAccountTransaction.Source_Type = "PP_LINE_ITEM";
                            objAccountTransaction.Created_By = ClientSession.UserName;
                            if (hdnLocalTime.Value != string.Empty)
                            {
                                objAccountTransaction.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                            }
                            objAccountTransaction.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                            objAccountTransaction.Is_Delete = "N";
                            //objAccountTransaction.Carrier_ID = Cash_Carrier_ID;
                            SaveAccountTransactionList.Add(objAccountTransaction);
                        }
                    }
                }
                if (hdnCheckID.Value != "")
                {
                    CheckManager CheckMngr = new CheckManager();
                    SaveCheckList = new List<Check>();
                    if (objcheckout.CheckList[0] != null)
                    {
                        if (txtPaymentAmount.Text != "")
                            objcheckout.CheckList[0].Payment_Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                        if (hdnLocalTime.Value != string.Empty)
                            objcheckout.PPHeaderList[0].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        objcheckout.CheckList[0].Modified_By = ClientSession.UserName;
                        if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
                        {
                            objcheckout.CheckList[0].Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                        }
                        else
                        {
                            objcheckout.CheckList[0].Check_Date = DateTime.MinValue;
                        }
                        objcheckout.CheckList[0].Payment_Type = cboMethodOfPayment.Text;
                        objcheckout.CheckList[0].Carrier_Patient_Name = txtPatientLastName.Text;
                        if (txtCheckNo.Text != string.Empty)
                            objcheckout.CheckList[0].Payment_ID = txtCheckNo.Text;
                        SaveCheckList.Add(objcheckout.CheckList[0]);
                    }
                }
                IList<VisitPaymentDTO> VisitPayDTO = new List<VisitPaymentDTO>();
                if (hdnEncounterID.Value != "")
                    VisitPayDTO = visitMgr.UpdateVisitPayment(SaveVisitPaymentList, SaveCheckList, SavePPHeaderList, SavePPLineItemList, ilistAccTran, SaveAccountTransactionList, SaveVisitPaymenHistoryList, Convert.ToUInt32(hdnEncounterID.Value));
                bValidPaymentInfo = true;
                SaveVisitPaymentList.Clear();
                LoadPaymentInfoGrid(VisitPayDTO);
                paymentinformationdisableall();
                PaymentInformationClearAll();
                cboMethodOfPayment.SelectedIndex = 0;
                TextBoxColorChange(txtRecOnAcc, false);
                TextBoxColorChange(txtRefundAmount, false);
                TextBoxColorChange(txtpaidBy, true);
                ComboBoxColorChange(cboRelation, true);
                TextBoxColorChange(txtpaidBy, false);
                ComboBoxColorChange(cboRelation, false);
                cboRelation.SelectedIndex = 0;
                txtpaidBy.Text = txtPatientLastName.Text + ',' + txtPatientFirstName.Text;

                btnAdd.Text = "Add";
                btnClear.Text = "Clear All";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ADD", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnendwaitcursor_Click(object sender, EventArgs e)
        {
            divLoading.Style.Add("display", "none");
        }

        public void paymentinformationdisableall()
        {
            TextBoxColorChange(txtCheckNo, false);
            TextBoxColorChange(txtAuthNo, false);
            TextBoxColorChange(txtPaymentAmount, false);
            DateTimePickerColorChangeForWindows(dtpCheckDate, false);
            TextBoxColorChange(txtPaymentNote, false);
        }

        public void DateTimePickerColorChangeForWindows(RadMaskedTextBox datetimepicker, Boolean bToNormal)
        {
            if (datetimepicker.ID != "dtpPatientDOB" && datetimepicker.ID != "dtpEffectiveStartDate")
            {
                if (bToNormal == false)
                {
                    datetimepicker.Enabled = false;
                    datetimepicker.Text = string.Empty;
                }
                else
                {
                    datetimepicker.Enabled = true;
                }
            }
            else
            {
                if (bToNormal == false)
                {
                    //Cap - 669
                    //datetimepicker.Enabled = false;
                    datetimepicker.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    //Cap - 669
                    //datetimepicker.Enabled = true;
                    datetimepicker.Attributes.Remove("disabled");
                    datetimepicker.Attributes.Add("enabled", "enabled");

                }
            }
        }

        public void PaymentInformationClearAll()
        {
            //Added By thamizh to clear realtionship and amount paid by 
            cboRelation.SelectedIndex = 0;
            if (cboRelation.SelectedItem.Text == "Others")
            {
                txtpaidBy.Text = txtPatientLastName.Text + ',' + txtPatientFirstName.Text;
                TextBoxColorChange(txtpaidBy, true);
            }
            else if (cboRelation.SelectedItem.Text == "Patient")
            {
                txtpaidBy.Text = txtPatientLastName.Text + ',' + txtPatientFirstName.Text; // objCheckOutLoad.HumanObj.Last_Name + ',' + objCheckOutLoad.HumanObj.First_Name;
                TextBoxColorChange(txtpaidBy, false);
            }

            txtAuthNo.Text = string.Empty;
            txtPaymentAmount.Text = string.Empty;
            txtCheckNo.Text = string.Empty;
            txtRefundAmount.Text = string.Empty;

            txtRecOnAcc.Text = string.Empty;
            txtPaymentNote.Text = string.Empty;
            txtPastDue.Text = string.Empty;
            chkMultiplePayments.Checked = false;
        }

        protected void btnSave_Click1(object sender, EventArgs e)
        {

        }

        protected void cboMethodOfPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cboMethodOfPayment.Attributes.Add("onchange", "Loading();");
            btnAdd.Enabled = true;
            if (chkMultiplePayments.Checked == true)
            {
                btnAdd.Enabled = true;
                btnClear.Enabled = true;
            }
            /*Disable the Payment information groupbox if 'Method of Payment' is empty*/
            if (cboMethodOfPayment.SelectedItem.Text == string.Empty)
            {
                paymentinformationdisableall();
                //PaymentInformationClearAll();
                txtRecOnAcc.Text = "0.00";
                txtPaymentAmount.Text = "0.00";
                txtRefundAmount.Text = "0.00";
                TextBoxColorChange(txtRecOnAcc, false);
                TextBoxColorChange(txtRefundAmount, false);

                TextBoxColorChange(txtpaidBy, false);
                ComboBoxColorChange(cboRelation, false);
                cboRelation.SelectedIndex = 0;
                if (cboRelation.SelectedItem.Text == "Patient")
                {
                    txtpaidBy.Text = txtPatientLastName.Text + ',' + txtPatientFirstName.Text;
                    TextBoxColorChange(txtpaidBy, false);
                }
                btnAdd.Enabled = false;
            }
            /*Enable the PaymentAmount Textbox for "Cash"*/
            else if (cboMethodOfPayment.SelectedItem.Text.ToUpper() == "CASH")
            {
                paymentinformationdisableall();
                paymentamountenable();
                txtRecOnAcc.Text = "0.00";
                txtPaymentAmount.Text = "0.00";
                txtRefundAmount.Text = "0.00";
                TextBoxColorChange(txtRecOnAcc, true);
                TextBoxColorChange(txtRefundAmount, true);
                txtCheckNo.Text = string.Empty;

                TextBoxColorChange(txtpaidBy, true);
                ComboBoxColorChange(cboRelation, true);
                cboRelation.SelectedIndex = 0;
                if (cboRelation.SelectedItem.Text == "Patient")
                {
                    txtpaidBy.Text = txtPatientLastName.Text + ',' + txtPatientFirstName.Text;
                    TextBoxColorChange(txtpaidBy, false);
                }



                spanRelation.Attributes.Remove("class"); /*added*/

                spanRelation.Attributes.Add("class", "MandLabelstyle");
                spanRelationstar.Visible = true;

                spanPaidBy.Attributes.Remove("class");

                spanPaidBy.Attributes.Add("class", "MandLabelstyle");
                spanPaidStar.Visible = true;

                spanCheck.Attributes.Remove("class"); /*added*/
                spanCheck.Attributes.Add("class", "spanstyle");
                spanCheckStar.Visible = true;
                spanCheckStar.Visible = false;


            }
            /*Enable the PaymentAmount and Check no Textbox for "Cheque"*/
            else if (cboMethodOfPayment.SelectedItem.Text.ToUpper() == "CHECK")
            {
                paymentamountenable();
                TextBoxColorChange(txtCheckNo, true);
                TextBoxColorChange(txtAuthNo, false);
                dtpCheckDate.Enabled = true;
                txtRecOnAcc.Text = "0.00";
                txtPaymentAmount.Text = "0.00";
                txtRefundAmount.Text = "0.00";
                DateTimePickerColorChangeForWindows(dtpCheckDate, true);
                TextBoxColorChange(txtRecOnAcc, true);
                TextBoxColorChange(txtRefundAmount, true);
                MaskedTextBoxColorChange(dtpCheckDate, true);
                dtpCheckDate.Enabled = true;
                txtCheckNo.Text = string.Empty;

                TextBoxColorChange(txtpaidBy, true);
                ComboBoxColorChange(cboRelation, true);
                cboRelation.SelectedIndex = 0;
                if (cboRelation.SelectedItem.Text == "Patient")
                {
                    txtpaidBy.Text = txtPatientLastName.Text + ',' + txtPatientFirstName.Text;
                    TextBoxColorChange(txtpaidBy, false);
                }

                spanCheck.Attributes.Remove("class");

                spanCheck.Attributes.Add("class", "MandLabelstyle");
                spanCheckStar.Visible = true;


                spanRelation.Attributes.Remove("class"); /*added*/

                spanRelation.Attributes.Add("class", "MandLabelstyle");
                spanRelationstar.Visible = true;

                spanPaidBy.Attributes.Remove("class");

                spanPaidBy.Attributes.Add("class", "MandLabelstyle");
                spanPaidStar.Visible = true;


            }
            else if (cboMethodOfPayment.SelectedItem.Text.ToUpper() == "CREDIT CARD" || cboMethodOfPayment.SelectedItem.Text.ToUpper() == "DEBIT CARD")
            {
                paymentinformationdisableall();
                paymentamountenable();
                //ChangeLabelStyle(lblCheckNo, false);
                TextBoxColorChange(txtCheckNo, true);
                TextBoxColorChange(txtAuthNo, true);
                txtRecOnAcc.Text = "0.00";
                txtPaymentAmount.Text = "0.00";
                txtRefundAmount.Text = "0.00";
                TextBoxColorChange(txtRecOnAcc, true);
                TextBoxColorChange(txtRefundAmount, true);
                txtCheckNo.Text = string.Empty;

                TextBoxColorChange(txtpaidBy, true);
                ComboBoxColorChange(cboRelation, true);
                cboRelation.SelectedIndex = 0;
                if (cboRelation.SelectedItem.Text == "Patient")
                {
                    txtpaidBy.Text = txtPatientLastName.Text + ',' + txtPatientFirstName.Text;
                    TextBoxColorChange(txtpaidBy, false);
                }

                spanCheck.Attributes.Remove("class");

                spanCheck.Attributes.Add("class", "MandLabelstyle");
                spanCheckStar.Visible = true;


                spanRelation.Attributes.Remove("class"); /*added*/

                spanRelation.Attributes.Add("class", "MandLabelstyle");
                spanRelationstar.Visible = true;

                spanPaidBy.Attributes.Remove("class");

                spanPaidBy.Attributes.Add("class", "MandLabelstyle");
                spanPaidStar.Visible = true;

            }
            /*Enable all the controls in Payment information Group box.*/
            else
            {
                paymentamountenable();
                TextBoxColorChange(txtCheckNo, true);
                TextBoxColorChange(txtAuthNo, false);
                DateTimePickerColorChangeForWindows(dtpCheckDate, true);
                txtRecOnAcc.Text = "0.00";
                txtPaymentAmount.Text = "0.00";
                txtRefundAmount.Text = "0.00";
                TextBoxColorChange(txtRecOnAcc, false);
                TextBoxColorChange(txtRefundAmount, false);
                txtCheckNo.Text = string.Empty;
                ComboBoxColorChange(cboRelation, true);
                cboRelation.SelectedIndex = 0;
                if (cboRelation.SelectedItem.Text == "Patient")
                {
                    txtpaidBy.Text = txtPatientLastName.Text + ',' + txtPatientFirstName.Text;
                    TextBoxColorChange(txtpaidBy, false);
                }
                spanCheck.Attributes.Remove("class");

                spanCheck.Attributes.Add("class", "MandLabelstyle");
                spanCheckStar.Visible = true;



                spanRelation.Attributes.Remove("class"); /*added*/

                spanRelation.Attributes.Add("class", "MandLabelstyle");
                spanRelationstar.Visible = true;

                spanPaidBy.Attributes.Remove("class");

                spanPaidBy.Attributes.Add("class", "MandLabelstyle");
                spanPaidStar.Visible = true;

            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "  {warningmethod();}", true);
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "  {warningmethod();}, {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        public void paymentamountenable()
        {
            TextBoxColorChange(txtPaymentAmount, true);
            TextBoxColorChange(txtPaymentNote, true);
        }

        protected void btnEditName_Click(object sender, EventArgs e)
        {
            TextBoxColorChange(txtPatientFirstName, true);
            TextBoxColorChange(txtPatientLastName, true);
            TextBoxColorChange(txtPatientMI, true);
            ComboBoxColorChange(cboPatientSex, true);
            ComboBoxColorChange(cboPatientSuffix, true);
            MaskedTextBoxColorChange(msktxtZipcode, true);
            //CAP-1327
            MaskedTextBoxColorChange(msktxtCellPhno, true);
            //Cap - 669
            dtpPatientDOB.Attributes.Remove("disabled");
            dtpPatientDOB.CssClass = "Editabletxtbox";
            dtpPatientDOB.ReadOnly = false;
            dtpPatientDOB.Enabled =true;

            ScreenMode = "EDIT NAME";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (btnClear.Text == "Cancel")
            {
                btnAdd.Text = "Add";
                btnClear.Text = "Clear All";
            }
            cboMethodOfPayment.Enabled = true;
            cboMethodOfPayment.CssClass = "Editabletxtbox";
            spanPaymentNotes.Attributes.Remove("class"); /*added*/
            spanPaymentNotes.Attributes.Add("class", "spanstyle");
            spanPatientNotestar.Visible = false;
            spanPaymentNotes.Style["margin-left"] = "17px";
            btnAdd.Text = "Add";
            btnAdd.Enabled = false;
            spanCheck.Attributes.Remove("class"); /*added*/
            spanCheck.Attributes.Add("class", "spanstyle");
            spanCheckStar.Visible = false;
        }

        protected void btnCheckDuplicate_Click(object sender, EventArgs e)
        {

            Human objHuman = new Human();

            objHuman.First_Name = txtPatientFirstName.Text.Trim();
            objHuman.Last_Name = txtPatientLastName.Text.Trim();
            objHuman.MI = txtPatientMI.Text.Trim();
            if (cboPatientSex.Items.FindByValue(cboPatientSex.SelectedItem.Text.ToString().Trim()) != null && cboPatientSex.SelectedItem.Text != string.Empty)
            {
                objHuman.Sex = cboPatientSex.SelectedItem.Text;
            }
            objHuman.Legal_Org = ClientSession.LegalOrg;
            objHuman.Encounter_Provider_ID = 0;
            objHuman.Account_Status = "Active";
            if (hdnLocalTime.Value != string.Empty)
            {
                objHuman.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
            }
            objHuman.Created_By = ClientSession.UserName;
            objHuman.Patient_Date_Last_Billed = DateTime.MinValue;
            objHuman.Guarantor_Birth_Date = DateTime.MinValue;
            objHuman.Patient_UnApplied_Payments = 0;
            objHuman.Emergency_BirthDate = DateTime.MinValue;
            objHuman.Demo_Update_Time_Stamp = DateTime.MinValue;
            objHuman.Batch_ID = 0;
            objHuman.Past_Due = 0;
            objHuman.Medical_Record_Number = txtMedicalRecordNo.Text;
            objHuman.Patient_Account_External = txtExternalAccNo.Text;

            if (bSendMailClick == true && chkOnlineAccess.Checked == true)
            {
                objHuman.Is_Mail_Sent = "Y";
                if (hdnLocalTime.Value != string.Empty)
                {
                    objHuman.Mail_Sent_Date = Convert.ToDateTime(hdnLocalTime.Value);
                }
            }
            else
            {
                objHuman.Is_Mail_Sent = "N";
            }
            if (cboPreferredConfidentialCoreespondenceMode.SelectedItem != null)
            {
                objHuman.Preferred_Confidential_Correspodence_Mode = cboPreferredConfidentialCoreespondenceMode.SelectedItem.Text;
            }
            objHuman.EMail = txtMail.Text;
            if (msktxtSSN.TextWithLiterals == "--")
            {
                objHuman.SSN = string.Empty;
            }
            else if (msktxtSSN.TextWithLiterals != "--")
            {
                objHuman.SSN = msktxtSSN.TextWithLiterals;
            }
            if (msktxtCellPhno.TextWithLiterals != "() -")
            {
                objHuman.Cell_Phone_Number = msktxtCellPhno.TextWithLiterals;
            }
            else if (msktxtCellPhno.TextWithLiterals == "() -")
            {
                objHuman.Cell_Phone_Number = string.Empty;
            }
            if (msktxtHomePhno.TextWithLiterals != "() -")
            {
                objHuman.Home_Phone_No = msktxtHomePhno.TextWithLiterals;
            }
            else if (msktxtHomePhno.TextWithLiterals == "() -")
            {
                objHuman.Home_Phone_No = string.Empty;
            }

            if (msktxtZipcode.TextWithLiterals != "-")
            {
                if (msktxtZipcode.TextWithLiterals.Length == 6 && msktxtZipcode.TextWithLiterals.Length < 10)
                {
                    string[] Split = Convert.ToString(msktxtZipcode.TextWithLiterals).Split('-');
                    if (Split.Length == 2 && Split[1] == string.Empty)
                    {
                        objHuman.ZipCode = Split[0].ToString();
                    }
                }
                else
                {

                    objHuman.ZipCode = msktxtZipcode.TextWithLiterals;
                }
            }
            else
            {
                objHuman.ZipCode = string.Empty;
            }

            objHuman.People_In_Collection = "N";

            objHuman.Birth_Date = Convert.ToDateTime(dtpPatientDOB.Text);
            ListToSaveHuman.Add(objHuman);


            FillQuickPatient objCheckOut = null;
            Human humanLoadRecord = null;
            if (hdnEncounterID.Value != string.Empty && hdnEncounterID.Value != "0")
            {
                objCheckOut = HumanMngr.LoadQuickPatient(Convert.ToUInt64(hdnEncounterID.Value));
            }
            if (objCheckOut != null && objCheckOut.HumanObj != null)
            {
                humanLoadRecord = objCheckOut.HumanObj;
            }
            if (humanLoadRecord != null)
            {
                if (txtMail.Text != string.Empty)
                {
                    if (CheckEmailDuplicate() == false)
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380040');", true);
                        txtMail.Focus();
                        chkOnlineAccess.Checked = false;
                        txtMail.Text = string.Empty;
                        return;
                    }
                }
                Human objHumanRecord = new Human();
                if (objCheckOut != null && humanLoadRecord == null)
                {
                    objHumanRecord = objCheckOut.HumanObj;
                }
                else if (humanLoadRecord != null)
                {
                    objHumanRecord = humanLoadRecord;
                }
                objHumanRecord.First_Name = txtPatientFirstName.Text;
                objHumanRecord.Last_Name = txtPatientLastName.Text;
                objHumanRecord.MI = txtPatientMI.Text;
                if (cboPatientSex.Text.Trim() != string.Empty)
                    objHumanRecord.Sex = cboPatientSex.Text;
                objHumanRecord.Legal_Org = ClientSession.LegalOrg;
                objHumanRecord.Patient_Account_External = txtExternalAccNo.Text;
                objHumanRecord.Medical_Record_Number = txtMedicalRecordNo.Text;

                if (msktxtSSN.TextWithLiterals == "--")
                {
                    objHumanRecord.SSN = string.Empty;
                }
                else if (msktxtSSN.TextWithLiterals != "--")
                {
                    objHumanRecord.SSN = msktxtSSN.TextWithLiterals;
                }
                if (msktxtCellPhno.TextWithLiterals != "() -")
                {
                    objHumanRecord.Cell_Phone_Number = msktxtCellPhno.TextWithLiterals;
                }
                else if (msktxtCellPhno.TextWithLiterals == "() -")
                {
                    objHumanRecord.Cell_Phone_Number = string.Empty;
                }
                if (msktxtHomePhno.TextWithLiterals != "() -")
                {
                    objHumanRecord.Home_Phone_No = msktxtHomePhno.TextWithLiterals;
                }
                else if (msktxtHomePhno.TextWithLiterals == "() -")
                {
                    objHumanRecord.Home_Phone_No = string.Empty;
                }
                if (chkOnlineAccess.Checked == true)
                {
                    objHumanRecord.Is_Mail_Sent = "Y";
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objHumanRecord.Mail_Sent_Date = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                }
                else
                {
                    objHumanRecord.Is_Mail_Sent = "N";
                }
                if (msktxtZipcode.TextWithLiterals != "-")
                {
                    if (msktxtZipcode.TextWithLiterals.Length == 6 && msktxtZipcode.TextWithLiterals.Length < 10)
                    {
                        string[] Split = Convert.ToString(msktxtZipcode.TextWithLiterals).Split('-');
                        if (Split.Length == 2 && Split[1] == string.Empty)
                        {
                            objHuman.ZipCode = Split[0].ToString();
                        }
                    }
                    else
                    {

                        objHuman.ZipCode = msktxtZipcode.TextWithLiterals;
                    }
                }
                else
                {
                    objHuman.ZipCode = string.Empty;
                }
                objHumanRecord.Preferred_Confidential_Correspodence_Mode = cboPreferredConfidentialCoreespondenceMode.SelectedItem.Text;

                objHumanRecord.EMail = txtMail.Text;
                objHumanRecord.Password = string.Empty;
                ListToUpdateHuman.Add(objHumanRecord);

            }
            if (chkEligibilityVerified.Checked == true)
            {

                if (txtPolicyHolderID.Text == string.Empty)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380015');", true);
                    txtPolicyHolderID.Focus();
                    return;
                }

                string sCarrierIDList = System.Configuration.ConfigurationSettings.AppSettings["MedicareCarrierIDList"];
                string[] CarrierIDList = sCarrierIDList.Split(',');

                if (CarrierIDList.Contains<String>(ddlPayerName.SelectedValue) == true)
                {
                    string sResult = UtilityManager.ValidatePolicyHolderID(txtPolicyHolderID.Text);
                    if (sResult.StartsWith("Fail") == true)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380057','','" + sResult.Split('|')[1] + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        txtPolicyHolderID.Focus();
                        return;
                    }
                }

                if (dtpEffectiveStartDate.Text == "")
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380016');", true);
                    dtpEffectiveStartDate.Focus();
                    return;
                }

                if (Convert.ToDateTime(dtpEffectiveStartDate.Text) < Convert.ToDateTime(dtpPatientDOB.Text))
                {

                    //ApplicationObject.erroHandler.DisplayErrorMessage("380003", "QuickPatientCreate", this.Page);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380003');", true);
                    dtpEffectiveStartDate.Focus();
                    return;
                }

                if (dtpTerminationDate.Text != "" && Convert.ToDateTime(dtpEffectiveStartDate.Text) > Convert.ToDateTime(dtpTerminationDate.Text))
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380005');", true);
                    dtpTerminationDate.Focus();
                    return;
                }
                if (txtPCPVisitInCopay.Text != string.Empty && AmountValidation(txtPCPVisitInCopay.Text) == false)
                {
                    txtPCPVisitInCopay.Focus();
                    return;
                }
                if (txtPCPVisitInCoIns.Text != string.Empty && AmountValidation(txtPCPVisitInCoIns.Text) == false)
                {
                    txtPCPVisitInCoIns.Focus();
                    return;
                }
                if (txtPCPVisitOutCopay.Text != string.Empty && AmountValidation(txtPCPVisitOutCopay.Text) == false)
                {
                    txtPCPVisitOutCopay.Focus();
                    return;
                }
                if (txtPCPVisitOutCoIns.Text != string.Empty && AmountValidation(txtPCPVisitOutCoIns.Text) == false)
                {
                    txtPCPVisitOutCoIns.Focus();
                    return;
                }
                if (txtSpecialityVisitInCopay.Text != string.Empty && AmountValidation(txtSpecialityVisitInCopay.Text) == false)
                {
                    txtSpecialityVisitInCopay.Focus();
                    return;

                }
                if (txtSpecialityVisitInCoIns.Text != string.Empty && AmountValidation(txtSpecialityVisitInCoIns.Text) == false)
                {
                    txtSpecialityVisitInCoIns.Focus();
                    return;
                }
                if (txtSpecialityVisitOutCopay.Text != string.Empty && AmountValidation(txtSpecialityVisitOutCopay.Text) == false)
                {
                    txtSpecialityVisitOutCopay.Focus();
                    return;
                }
                if (txtSpecialityVisitOutCoIns.Text != string.Empty && AmountValidation(txtSpecialityVisitOutCoIns.Text) == false)
                {
                    txtSpecialityVisitOutCoIns.Focus();
                    return;
                }
                if (txtMedicationInCopay.Text != string.Empty && AmountValidation(txtMedicationInCopay.Text) == false)
                {
                    txtMedicationInCopay.Focus();
                    return;
                }
                if (txtMedicationInCoIns.Text != string.Empty && AmountValidation(txtMedicationInCoIns.Text) == false)
                {
                    txtMedicationInCoIns.Focus();
                    return;
                }
                if (txtMedicationOutCopay.Text != string.Empty && AmountValidation(txtMedicationOutCopay.Text) == false)
                {
                    txtMedicationOutCopay.Focus();
                    return;
                }
                if (txtMedicationOutCoIns.Text != string.Empty && AmountValidation(txtMedicationOutCoIns.Text) == false)
                {
                    txtMedicationOutCoIns.Focus();
                    return;
                }
                if (txtUrgentCareInCopay.Text != string.Empty && AmountValidation(txtUrgentCareInCopay.Text) == false)
                {
                    txtUrgentCareInCopay.Focus();
                    return;
                }
                if (txtUrgentCareCoInIns.Text != string.Empty && AmountValidation(txtUrgentCareCoInIns.Text) == false)
                {
                    txtUrgentCareCoInIns.Focus();
                    return;
                }
                if (txtUrgentCareOutCopay.Text != string.Empty && AmountValidation(txtUrgentCareOutCopay.Text) == false)
                {
                    txtUrgentCareOutCopay.Focus();
                    return;
                }
                if (txtUrgentCareOutCoIns.Text != string.Empty && AmountValidation(txtUrgentCareOutCoIns.Text) == false)
                {
                    txtUrgentCareOutCoIns.Focus();
                    return;
                }
                if (txtInDeductiblePlan.Text != string.Empty && AmountValidation(txtInDeductiblePlan.Text) == false)
                {
                    txtInDeductiblePlan.Focus();
                    return;
                }
                if (txtInPockot.Text != string.Empty && AmountValidation(txtInPockot.Text) == false)
                {
                    txtInPockot.Focus();
                    return;
                }
                if (txtOutDeductiblePlan.Text != string.Empty && AmountValidation(txtOutDeductiblePlan.Text) == false)
                {
                    txtOutDeductiblePlan.Focus();
                    return;
                }
                if (txtOutPocket.Text != string.Empty && AmountValidation(txtOutPocket.Text) == false)
                {
                    txtOutPocket.Focus();
                    return;
                }
                if (txtInDeductiblemet.Text != string.Empty && AmountValidation(txtInDeductiblemet.Text) == false)
                {
                    txtInDeductiblemet.Focus();
                    return;
                }
                if (txtInpocketmet.Text != string.Empty && AmountValidation(txtInpocketmet.Text) == false)
                {
                    txtInpocketmet.Focus();
                    return;
                }
                if (txtOutDeductiblemet.Text != string.Empty && AmountValidation(txtOutDeductiblemet.Text) == false)
                {
                    txtOutDeductiblemet.Focus();
                    return;
                }
                if (txtOutpocketmet.Text != string.Empty && AmountValidation(txtOutpocketmet.Text) == false)
                {
                    txtOutpocketmet.Focus();
                    return;
                }
                if (txtInFamilyDeductible.Text != string.Empty && AmountValidation(txtInFamilyDeductible.Text) == false)
                {
                    txtInFamilyDeductible.Focus();
                    return;
                }
                if (txtInFamilypocket.Text != string.Empty && AmountValidation(txtInFamilypocket.Text) == false)
                {
                    txtInFamilypocket.Focus();
                    return;
                }
                if (txtOutFamilyDeductible.Text != string.Empty && AmountValidation(txtOutFamilyDeductible.Text) == false)
                {
                    txtOutFamilyDeductible.Focus();
                    return;
                }
                if (txtOutFamilypocket.Text != string.Empty && AmountValidation(txtOutFamilypocket.Text) == false)
                {
                    txtOutFamilypocket.Focus();
                    return;
                }
                if (txtInFamilyDeductiblemet.Text != string.Empty && AmountValidation(txtInFamilyDeductiblemet.Text) == false)
                {
                    txtInFamilyDeductiblemet.Focus();
                    return;
                }
                if (txtInFamilymetpocket.Text != string.Empty && AmountValidation(txtInFamilymetpocket.Text) == false)
                {
                    txtInFamilymetpocket.Focus();
                    return;
                }
                if (txtOutFamilyDeductiblemet.Text != string.Empty && AmountValidation(txtOutFamilyDeductiblemet.Text) == false)
                {
                    txtOutFamilyDeductiblemet.Focus();
                    return;
                }
                if (txtOutFamilymetpocket.Text != string.Empty && AmountValidation(txtOutFamilymetpocket.Text) == false)
                {
                    txtOutFamilymetpocket.Focus();
                    return;
                }

                Eligibility_Verification objEligibility = new Eligibility_Verification();
                if (hdnHumanID.Value != string.Empty)
                {
                    objEligibility.Human_ID = Convert.ToUInt64(hdnHumanID.Value);
                }
                objEligibility.Eligibility_Verified_By = ClientSession.UserName;
                if (hdnLocalTime.Value != string.Empty)
                {
                    objEligibility.Eligibility_Verified_Date = Convert.ToDateTime(hdnLocalTime.Value);
                }



                objEligibility.Effective_Date = Convert.ToDateTime(dtpEffectiveStartDate.Text);
                //objEligibility.Payer_Name = ddlPayerName.SelectedItem.Text;
                objEligibility.Policy_Holder_ID = txtPolicyHolderID.Text;
                objEligibility.Group_Number = txtGroupNumber.Text;

                if (dtpTerminationDate.Text != "")
                {
                    objEligibility.Termination_Date = Convert.ToDateTime(dtpTerminationDate.Text);
                }
                /*Assigning Minimum date for termination date*/
                else
                {
                    objEligibility.Termination_Date = DateTime.MinValue;
                }
                /*Check PCPCopay textbox is not empty and assign PCPCopay*/

                if (cboVerificationType.Items.FindByValue(cboVerificationType.SelectedItem.Text.ToString().Trim()) != null && cboVerificationType.SelectedItem != null)
                {
                    objEligibility.Eligibility_Type = cboVerificationType.SelectedItem.Text.ToString().Trim();
                }
                else
                {
                    objEligibility.Eligibility_Type = "";
                }
                objEligibility.Demo_Note = txtDemoNote.Text;


                objEligibility.Created_By = ClientSession.UserName;
                if (hdnLocalTime.Value != string.Empty)
                {
                    objEligibility.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);

                    objEligibility.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                }
                objEligibility.Modified_By = ClientSession.UserName;

                if (txtInsurancetype.Text != string.Empty)
                {
                    objEligibility.Plan_Type = Convert.ToString(txtInsurancetype.Text);
                }
                if (txtPlanNumber.Text != string.Empty)
                {
                    objEligibility.Plan_Number = Convert.ToString(txtPlanNumber.Text);
                }
                if (txtOrganization.Text != string.Empty)
                {
                    objEligibility.Organization = Convert.ToString(txtOrganization.Text);
                }
                if (txtSubscriberName.Text != string.Empty)
                {
                    objEligibility.Subscriber_Name = Convert.ToString(txtSubscriberName.Text);
                }
                if (txtPCPName.Text != string.Empty)
                {
                    objEligibility.PCP_Name = Convert.ToString(txtPCPName.Text);
                }
                if (txtRelationship.Text != string.Empty)
                {
                    objEligibility.Relationship_to_Subscriber = Convert.ToString(txtRelationship.Text);
                }
                if (txtPCP_NPI.Text != string.Empty)
                {
                    objEligibility.PCP_NPI = Convert.ToString(txtPCP_NPI.Text);
                }
                if (txtGroupNumber.Text != string.Empty)
                {
                    objEligibility.Group_Number = Convert.ToString(txtGroupNumber.Text);
                }
                if (dtpPCPEffectiveDate.Text != string.Empty)
                {
                    objEligibility.PCP_Effective_Date = Convert.ToDateTime(dtpPCPEffectiveDate.Text);
                }
                if (txtGroupName.Text != string.Empty)
                {
                    objEligibility.Group_Name = Convert.ToString(txtGroupName.Text);
                }
                if (txtIPAName.Text != string.Empty)
                {
                    objEligibility.IPA_Name = Convert.ToString(txtIPAName.Text);
                }
                if (txtPCPVisitInCopay.Text != string.Empty)
                {
                    objEligibility.PCP_Office_Visit_InNet_Copay = Convert.ToDouble(txtPCPVisitInCopay.Text);
                }
                if (txtPCPVisitInCoIns.Text != string.Empty)
                {
                    objEligibility.PCP_Office_Visit_InNet_CoIns = Convert.ToDouble(txtPCPVisitInCoIns.Text);
                }
                if (txtPCPVisitOutCopay.Text != string.Empty)
                {
                    objEligibility.PCP_Office_Visit_OutNet_Copay = Convert.ToDouble(txtPCPVisitOutCopay.Text);
                }
                if (txtPCPVisitOutCoIns.Text != string.Empty)
                {
                    objEligibility.PCP_Office_Visit_OutNet_CoIns = Convert.ToDouble(txtPCPVisitOutCoIns.Text);
                }
                if (txtSpecialityVisitInCopay.Text != string.Empty)
                {
                    objEligibility.Specialty_Office_Visit_InNet_Copay = Convert.ToDouble(txtSpecialityVisitInCopay.Text);
                }
                if (txtSpecialityVisitInCoIns.Text != string.Empty)
                {
                    objEligibility.Specialty_Office_Visit_InNet_CoIns = Convert.ToDouble(txtSpecialityVisitInCoIns.Text);
                }
                if (txtSpecialityVisitOutCopay.Text != string.Empty)
                {
                    objEligibility.Specialty_Office_Visit_OutNet_Copay = Convert.ToDouble(txtSpecialityVisitOutCopay.Text);
                }
                if (txtSpecialityVisitOutCoIns.Text != string.Empty)
                {
                    objEligibility.Specialty_Office_Visit_OutNet_CoIns = Convert.ToDouble(txtSpecialityVisitOutCoIns.Text);
                }
                if (txtMedicationInCopay.Text != string.Empty)
                {
                    objEligibility.Inj_Medication_InNet_Copay = Convert.ToDouble(txtMedicationInCopay.Text);
                }
                if (txtMedicationInCoIns.Text != string.Empty)
                {
                    objEligibility.Inj_Medication_InNet_CoIns = Convert.ToDouble(txtMedicationInCoIns.Text);
                }
                if (txtMedicationOutCopay.Text != string.Empty)
                {
                    objEligibility.Inj_Medication_OutNet_Copay = Convert.ToDouble(txtMedicationOutCopay.Text);
                }
                if (txtMedicationOutCoIns.Text != string.Empty)
                {
                    objEligibility.Inj_Medication_OutNet_CoIns = Convert.ToDouble(txtMedicationOutCoIns.Text);
                }
                if (txtUrgentCareInCopay.Text != string.Empty)
                {
                    objEligibility.Urgent_Care_InNet_Copay = Convert.ToDouble(txtUrgentCareInCopay.Text);
                }
                if (txtUrgentCareCoInIns.Text != string.Empty)
                {
                    objEligibility.Urgent_Care_InNet_CoIns = Convert.ToDouble(txtUrgentCareCoInIns.Text);
                }
                if (txtUrgentCareOutCopay.Text != string.Empty)
                {
                    objEligibility.Urgent_Care_OutNet_CoIns = Convert.ToDouble(txtUrgentCareOutCopay.Text);
                }
                if (txtUrgentCareOutCoIns.Text != string.Empty)
                {
                    objEligibility.Urgent_Care_OutNet_CoIns = Convert.ToDouble(txtUrgentCareOutCoIns.Text);
                }
                if (txtPCPInNetworkMessage.Text != string.Empty)
                {
                    objEligibility.PCP_InNetwork_Copay_Message = Convert.ToString(txtPCPInNetworkMessage.Text);
                }
                if (txtPCPOutNetworkMessage.Text != string.Empty)
                {
                    objEligibility.PCP_OutNetwork_Copay_Message = Convert.ToString(txtPCPOutNetworkMessage.Text);
                }
                if (txtInDeductiblePlan.Text != string.Empty)
                {
                    objEligibility.Ind_per_plan_InNet_Deductible = Convert.ToDouble(txtInDeductiblePlan.Text);
                }
                if (txtInPockot.Text != string.Empty)
                {
                    objEligibility.Ind_per_plan_InNet_Out_of_Pocket = Convert.ToDouble(txtInPockot.Text);
                }



                if (txtOutDeductiblePlan.Text != string.Empty)
                {
                    objEligibility.Ind_per_plan_OutNet_Deductible = Convert.ToDouble(txtOutDeductiblePlan.Text);
                }
                if (txtOutPocket.Text != string.Empty)
                {
                    objEligibility.Ind_per_plan_OutNet_Out_of_Pocket = Convert.ToDouble(txtOutPocket.Text);
                }
                if (txtInDeductiblemet.Text != string.Empty)
                {
                    objEligibility.Ind_met_InNet_Deductible = Convert.ToDouble(txtInDeductiblemet.Text);
                }
                if (txtInpocketmet.Text != string.Empty)
                {
                    objEligibility.Ind_met_InNet_Out_of_Pocket = Convert.ToDouble(txtInpocketmet.Text);
                }
                if (txtOutDeductiblemet.Text != string.Empty)
                {
                    objEligibility.Ind_met_OutNet_Deductible = Convert.ToDouble(txtOutDeductiblemet.Text);
                }
                if (txtOutpocketmet.Text != string.Empty)
                {
                    objEligibility.Ind_met_OutNet_Out_of_Pocket = Convert.ToDouble(txtOutpocketmet.Text);
                }
                if (txtInFamilyDeductible.Text != string.Empty)
                {
                    objEligibility.Family_per_plan_InNet_Deductible = Convert.ToDouble(txtInFamilyDeductible.Text);
                }
                if (txtInFamilypocket.Text != string.Empty)
                {
                    objEligibility.Family_per_plan_InNet_Out_of_Pocket = Convert.ToDouble(txtInFamilypocket.Text);
                }
                if (txtOutFamilyDeductible.Text != string.Empty)
                {
                    objEligibility.Family_met_OutNet_Deductible = Convert.ToDouble(txtOutFamilyDeductible.Text);
                }
                if (txtOutFamilypocket.Text != string.Empty)
                {
                    objEligibility.Family_per_plan_OutNet_Out_of_Pocket = Convert.ToDouble(txtOutFamilypocket.Text);
                }
                if (txtInFamilyDeductiblemet.Text != string.Empty)
                {
                    objEligibility.Family_met_InNet_Deductible = Convert.ToDouble(txtInFamilyDeductiblemet.Text);
                }
                if (txtInFamilymetpocket.Text != string.Empty)
                {
                    objEligibility.Family_met_InNet_Out_of_Pocket = Convert.ToDouble(txtInFamilymetpocket.Text);
                }
                if (txtOutFamilyDeductiblemet.Text != string.Empty)
                {
                    objEligibility.Family_met_OutNet_Deductible = Convert.ToDouble(txtOutFamilyDeductiblemet.Text);
                }
                if (txtOutFamilymetpocket.Text != string.Empty)
                {
                    objEligibility.Family_met_OutNet_Out_of_Pocket = Convert.ToDouble(txtOutFamilymetpocket.Text);
                }
                if (txtDeductibleInNetworkMessage.Text != string.Empty)
                {
                    objEligibility.Deductible_InNetwork_Message = Convert.ToString(txtDeductibleInNetworkMessage.Text);
                }
                if (txtDeductibleOutNetworkMessage.Text != string.Empty)
                {
                    objEligibility.Deductible_OutNetwork_Message = Convert.ToString(txtDeductibleOutNetworkMessage.Text);
                }

                ListToSaveEligibility.Add(objEligibility);


            }
            if (cboMethodOfPayment.Text != string.Empty)
            {
                if (gbPaymentInformation.Visible == true && chkMultiplePayments.Checked == false)//&& AppointmentSaveAndValidation() == false)
                {

                    if (gbPaymentInformation.Visible == true)
                    {
                        if (txtCheckNo.ReadOnly == false && txtCheckNo.Text.Trim() == string.Empty)
                        {
                            //ApplicationObject.erroHandler.DisplayErrorMessage("380021", "Quick Patient Create", this.Page);
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380021');", true);
                            txtCheckNo.Focus();
                            return;
                        }

                        if (txtPaymentAmount.ReadOnly == false)
                        {
                            if (txtPaymentAmount.Text.Trim() == string.Empty)
                            {

                                // ApplicationObject.erroHandler.DisplayErrorMessage("380020", "Quick Patient Create", this.Page);
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380020');", true);
                                txtPaymentAmount.Focus();
                                return;
                            }
                            else if (Convert.ToDecimal(txtPaymentAmount.Text) <= Convert.ToDecimal(0))
                            {
                                //ApplicationObject.erroHandler.DisplayErrorMessage("380020", "Quick Patient Create", this.Page);
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380020');", true);
                                txtPaymentAmount.Focus();
                                return;
                            }
                        }
                        /*Null date to be checked.*/
                        if (dtpCheckDate.Text == "" && dtpCheckDate.Enabled == true)
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380006');", true);
                            return;
                        }
                    }

                    VisitPayment objPayment = new VisitPayment();
                    Check objCheck = new Check();
                    PPHeader objPPHeader = new PPHeader();
                    PPLineItem objPPLineItem = new PPLineItem();
                    AccountTransaction objAccountTransaction = new AccountTransaction();

                    /*Validates the PaymentAmount Textbox.*/
                    if (txtPaymentAmount.ReadOnly == false)
                    {
                        objPayment.Patient_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                        objCheck.Payment_Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                        objPPHeader.Total_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                        objPPLineItem.Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                        objAccountTransaction.Amount = -(Convert.ToDecimal(txtPaymentAmount.Text));
                    }

                    if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
                    {
                        objPayment.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                        objCheck.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                    }
                    else
                    {
                        objPayment.Check_Date = DateTime.MinValue;
                        objCheck.Check_Date = DateTime.MinValue;
                    }

                    objPayment.Check_Card_No = txtCheckNo.Text;
                    objPayment.Auth_No = txtAuthNo.Text;
                    objPayment.Method_of_Payment = cboMethodOfPayment.Text;
                    objPayment.Payment_Note = txtPaymentNote.Text;
                    objPayment.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                    if (hdnEncounterID.Value != string.Empty)
                    {
                        objPayment.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    }
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objPayment.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                    objPayment.Created_By = ClientSession.UserName;
                    SaveVisitPaymentList.Add(objPayment);

                    objCheck.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                    objCheck.Payment_Type = cboMethodOfPayment.Text;
                    objCheck.Created_By = string.Empty;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objCheck.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                    objCheck.Carrier_Patient_Name = txtPatientLastName.Text;
                    if (hdnEncounterID.Value != string.Empty)
                    {
                        objCheck.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    }
                    if (txtCheckNo.Text != string.Empty)
                        objCheck.Payment_ID = txtCheckNo.Text;
                    if (hdnCarrierId.Value != string.Empty)
                    {
                        objCheck.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                    }
                    SaveCheckList.Add(objCheck);

                    objPPHeader.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                    objPPHeader.Created_By = string.Empty;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objPPHeader.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                    if (hdnEncounterID.Value != string.Empty)
                    {
                        objPPHeader.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    }
                    if (hdnCarrierId.Value != string.Empty)
                    {
                        objPPHeader.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                    }
                    SavePPHeaderList.Add(objPPHeader);

                    objPPLineItem.Claim_Type = "PATIENT";
                    objPPLineItem.Line_Type = "UNAPPLIED";
                    objPPLineItem.Created_By = string.Empty;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objPPLineItem.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                    objPPLineItem.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                    if (hdnEncounterID.Value != string.Empty)
                    {
                        objPPLineItem.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    }
                    if (hdnCarrierId.Value != string.Empty)
                    {
                        objPPLineItem.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                    }
                    SavePPLineItemList.Add(objPPLineItem);

                    objAccountTransaction.Deposit_Date = UtilityManager.ConvertToLocal(objCheckOut.EncounterObj.Appointment_Date);
                    objAccountTransaction.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                    objAccountTransaction.Claim_Type = "PATIENT";
                    objAccountTransaction.Line_Type = "UNAPPLIED";
                    objAccountTransaction.Source_Type = "PP_LINE_ITEM";
                    objAccountTransaction.Created_By = string.Empty;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objAccountTransaction.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                    if (hdnEncounterID.Value != string.Empty)
                    {
                        objAccountTransaction.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    }
                    if (hdnCarrierId.Value != string.Empty)
                    {
                        objAccountTransaction.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                    }
                    SaveAccountTransactionList.Add(objAccountTransaction);

                    //   }
                }
                if (cboMethodOfPayment.Text != string.Empty)
                {
                    if (gbPaymentInformation.Visible == true && chkMultiplePayments.Checked == true)//&& AppointmentSaveAndValidation() == false)
                    {


                        if (gbPaymentInformation.Visible == true)
                        {
                            if (txtCheckNo.ReadOnly == false && txtCheckNo.Text.Trim() == string.Empty)
                            {
                                //ApplicationObject.erroHandler.DisplayErrorMessage("380021", "Quick Patient Create", this.Page);
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380021');", true);
                                txtCheckNo.Focus();
                                return;
                            }

                            if (txtPaymentAmount.ReadOnly == false)
                            {
                                if (txtPaymentAmount.Text.Trim() == string.Empty)
                                {

                                    //ApplicationObject.erroHandler.DisplayErrorMessage("380020", "Quick Patient Create", this.Page);
                                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380020');", true);
                                    txtPaymentAmount.Focus();
                                    return;
                                }
                                else if (Convert.ToDecimal(txtPaymentAmount.Text) <= Convert.ToDecimal(0))
                                {
                                    //ApplicationObject.erroHandler.DisplayErrorMessage("380020", "Quick Patient Create", this.Page);
                                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380020');", true);
                                    txtPaymentAmount.Focus();
                                    return;
                                }
                            }
                            /*Null date to be checked.*/
                            if (dtpCheckDate.Text == null && dtpCheckDate.Enabled == true)
                            {

                                // ApplicationObject.erroHandler.DisplayErrorMessage("380006", "Quick Patient Create", this.Page);
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380006');", true);
                                return;
                            }
                        }

                        VisitPayment objPayment = new VisitPayment();
                        Check objCheck = new Check();
                        PPHeader objPPHeader = new PPHeader();
                        PPLineItem objPPLineItem = new PPLineItem();
                        AccountTransaction objAccountTransaction = new AccountTransaction();

                        /*Validates the PaymentAmount Textbox.*/
                        if (txtPaymentAmount.ReadOnly == false)
                        {
                            objPayment.Patient_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                            objCheck.Payment_Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                            objPPHeader.Total_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                            objPPLineItem.Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                            objAccountTransaction.Amount = -(Convert.ToDecimal(txtPaymentAmount.Text));
                        }

                        if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
                        {
                            objPayment.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                            objCheck.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                        }
                        else
                        {
                            objPayment.Check_Date = DateTime.MinValue;
                            objCheck.Check_Date = DateTime.MinValue;
                        }

                        objPayment.Check_Card_No = txtCheckNo.Text;
                        objPayment.Auth_No = txtAuthNo.Text;
                        objPayment.Method_of_Payment = cboMethodOfPayment.Text;
                        objPayment.Payment_Note = txtPaymentNote.Text;
                        objPayment.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                        objPayment.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objPayment.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        objPayment.Created_By = ClientSession.UserName;
                        SaveVisitPaymentList.Add(objPayment);

                        objCheck.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                        objCheck.Payment_Type = cboMethodOfPayment.Text;
                        objCheck.Created_By = string.Empty;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objCheck.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        objCheck.Carrier_Patient_Name = txtPatientLastName.Text;
                        objCheck.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value); ;
                        if (txtCheckNo.Text != string.Empty)
                            objCheck.Payment_ID = txtCheckNo.Text;
                        if (hdnCarrierId.Value != string.Empty)
                        {
                            objCheck.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                        }
                        SaveCheckList.Add(objCheck);

                        objPPHeader.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                        objPPHeader.Created_By = string.Empty;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objPPHeader.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        if (hdnEncounterID.Value != string.Empty)
                            objPPHeader.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                        if (hdnCarrierId.Value != string.Empty)
                        {
                            objPPHeader.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                        }
                        SavePPHeaderList.Add(objPPHeader);

                        objPPLineItem.Claim_Type = "PATIENT";
                        objPPLineItem.Line_Type = "UNAPPLIED";
                        objPPLineItem.Created_By = string.Empty;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objPPLineItem.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        objPPLineItem.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                        if (hdnEncounterID.Value != string.Empty)
                            objPPLineItem.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                        if (hdnCarrierId.Value != string.Empty)
                        {
                            objPPLineItem.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                        }
                        SavePPLineItemList.Add(objPPLineItem);

                        //srividhya
                        //objAccountTransaction.Deposit_Date = ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), objCheckOut.EncounterObj.Appointment_Date);
                        objAccountTransaction.Deposit_Date = UtilityManager.ConvertToLocal(objCheckOut.EncounterObj.Appointment_Date);
                        objAccountTransaction.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                        objAccountTransaction.Claim_Type = "PATIENT";

                        objAccountTransaction.Line_Type = "UNAPPLIED";
                        objAccountTransaction.Source_Type = "PP_LINE_ITEM";
                        objAccountTransaction.Created_By = string.Empty;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objAccountTransaction.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        if (hdnEncounterID.Value != string.Empty)
                            objAccountTransaction.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                        if (hdnCarrierId.Value != string.Empty)
                        {
                            objAccountTransaction.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                        }
                        SaveAccountTransactionList.Add(objAccountTransaction);

                    }

                }
            }


            int iTryCount = 0;
        tryagain:
            XmlTextReader XmlText = null;
            try
            {
               
                if (hdnEncounterID.Value != string.Empty && hdnScreenMode.Value.ToUpper() == "CHECKEDIN")
                {
                    UtilityManager.ulMyFindPatientID = HumanMngr.BatchOperationsToQuickPatient(ListToSaveHuman.ToArray<Human>(), ListToUpdateHuman.ToArray<Human>(), ListToSaveEligibility.ToArray<Eligibility_Verification>(), SaveVisitPaymentList.ToArray<VisitPayment>(), SaveCheckList.ToArray<Check>(),
                        SavePPHeaderList.ToArray<PPHeader>(), SavePPLineItemList.ToArray<PPLineItem>(), SaveAccountTransactionList.ToArray<AccountTransaction>(), listPatientNotes.ToArray<PatientNotes>(), objPatInsuredPlan, string.Empty, objCheckOut.EncounterObj.Id, SaveVisitPaymenHistoryList);
                    txtPatientAccountNo.Text = UtilityManager.ulMyFindPatientID.ToString();
                }
                else if (hdnEncounterID.Value != string.Empty && hdnScreenMode.Value.ToUpper() == "SCANNING")
                {
                    UtilityManager.ulMyFindPatientID = HumanMngr.BatchOperationsToQuickPatient(ListToSaveHuman.ToArray<Human>(), ListToUpdateHuman.ToArray<Human>(), ListToSaveEligibility.ToArray<Eligibility_Verification>(), SaveVisitPaymentList.ToArray<VisitPayment>(), SaveCheckList.ToArray<Check>(),
                        SavePPHeaderList.ToArray<PPHeader>(), SavePPLineItemList.ToArray<PPLineItem>(), SaveAccountTransactionList.ToArray<AccountTransaction>(), listPatientNotes.ToArray<PatientNotes>(), objPatInsuredPlan, string.Empty, 0, SaveVisitPaymenHistoryList);
                    txtPatientAccountNo.Text = UtilityManager.ulMyFindPatientID.ToString();
                }
                else if (hdnEncounterID.Value != string.Empty && hdnScreenMode.Value.ToUpper() == "EDIT NAME")
                {
                    UtilityManager.ulMyFindPatientID = HumanMngr.BatchOperationsToQuickPatient(null, ListToUpdateHuman.ToArray<Human>(), ListToSaveEligibility.ToArray<Eligibility_Verification>(), SaveVisitPaymentList.ToArray<VisitPayment>(), SaveCheckList.ToArray<Check>(),
                        SavePPHeaderList.ToArray<PPHeader>(), SavePPLineItemList.ToArray<PPLineItem>(), SaveAccountTransactionList.ToArray<AccountTransaction>(), listPatientNotes.ToArray<PatientNotes>(), objPatInsuredPlan, string.Empty, objCheckOut.EncounterObj.Id, SaveVisitPaymenHistoryList);
                    txtPatientAccountNo.Text = UtilityManager.ulMyFindPatientID.ToString();
                }
                else
                {
                    UtilityManager.ulMyFindPatientID = HumanMngr.BatchOperationsToQuickPatient(ListToSaveHuman.ToArray<Human>(), ListToUpdateHuman.ToArray<Human>(), ListToSaveEligibility.ToArray<Eligibility_Verification>(), SaveVisitPaymentList.ToArray<VisitPayment>(), SaveCheckList.ToArray<Check>(),
                            SavePPHeaderList.ToArray<PPHeader>(), SavePPLineItemList.ToArray<PPLineItem>(), SaveAccountTransactionList.ToArray<AccountTransaction>(), listPatientNotes.ToArray<PatientNotes>(), objPatInsuredPlan, string.Empty, 0, SaveVisitPaymenHistoryList);
                    txtPatientAccountNo.Text = UtilityManager.ulMyFindPatientID.ToString();

                    //string HumanFileName = "Human" + "_" + ListToSaveHuman[0].Id + ".xml";
                    //string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
                    //if (File.Exists(strXmlHumanFilePath) == false && ListToSaveHuman[0].Id > 0)
                    //{

                    //    if (ListToSaveHuman != null && ListToSaveHuman.Count > 0)
                    //    {

                    //        string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                    //        string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                    //        XmlDocument itemDoc = new XmlDocument();
                    //         XmlText = new XmlTextReader(sXmlPath);
                    //        itemDoc.Load(XmlText);
                    //        int iAge = 0;
                    //        iAge = UtilityManager.CalculateAge(ListToSaveHuman[0].Birth_Date);

                    //        XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
                    //        if (xmlAge != null && xmlAge.Count > 0)
                    //            xmlAge[0].Attributes[0].Value = iAge.ToString();
                    //        else
                    //        {
                    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
                    //        }

                    //        XmlText.Close();
                    //       //itemDoc.Save(strXmlHumanFilePath);

                    //        int trycount = 0;
                    //    trytosaveagain:
                    //        try
                    //        {
                    //            itemDoc.Save(strXmlHumanFilePath);
                    //        }
                    //        catch (Exception xmlexcep)
                    //        {
                    //            trycount++;
                    //            if (trycount <= 3)
                    //            {
                    //                int TimeMilliseconds = 0;
                    //                if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                    //                    TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                    //                Thread.Sleep(TimeMilliseconds);
                    //                string sMsg = string.Empty;
                    //                string sExStackTrace = string.Empty;

                    //                string version = "";
                    //                if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                    //                    version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                    //                string[] server = version.Split('|');
                    //                string serverno = "";
                    //                if (server.Length > 1)
                    //                    serverno = server[1].Trim();

                    //                if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                    //                    sMsg = xmlexcep.InnerException.Message;
                    //                else
                    //                    sMsg = xmlexcep.Message;

                    //                if (xmlexcep != null && xmlexcep.StackTrace != null)
                    //                    sExStackTrace = xmlexcep.StackTrace;

                    //                string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    //                string ConnectionData;
                    //                ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    //                using (MySqlConnection con = new MySqlConnection(ConnectionData))
                    //                {
                    //                    using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                    //                    {
                    //                        cmd.Connection = con;
                    //                        try
                    //                        {
                    //                            con.Open();
                    //                            cmd.ExecuteNonQuery();
                    //                            con.Close();
                    //                        }
                    //                        catch
                    //                        {
                    //                        }
                    //                    }
                    //                }
                    //                goto trytosaveagain;
                    //            }
                    //        }
                    //    }
                    //}
                }
            }

            catch (Exception ex)
            {
                if (ex.Message.Contains("used by another process"))
                {
                    if (iTryCount < 3)
                    {
                        iTryCount++;
                        Thread.Sleep(2000);
                        goto tryagain;
                    }
                    else
                    {
                    }
                }
                else
                {
                    XmlText.Close();
                    UtilityManager.GenerateXML(ListToSaveHuman[0].Id.ToString(), "Human");
                    goto tryagain;

                }
               

              
            }
            bFormclose = false;
            bClose = false;
            bValidPaymentInfo = false;

            string args = "";
            if (hdnDupsendmail.Value != "true")
            {
                if (ListToSaveHuman != null && ListToSaveHuman.Count > 0)
                {
                    args = ListToSaveHuman[0].Id + "|" +
                    ListToSaveHuman[0].Last_Name + "|" +
                    ListToSaveHuman[0].First_Name + "|" +
                    ListToSaveHuman[0].MI + "|" +
                    ListToSaveHuman[0].Birth_Date.ToString("dd-MMM-yyyy") + "|" +
                    ListToSaveHuman[0].Encounter_Provider_ID + "|" +
                    ListToSaveHuman[0].Cell_Phone_Number + "|" +
                    ListToSaveHuman[0].Home_Phone_No + "|" +
                    ListToSaveHuman[0].Human_Type + "|" +
                    ListToUpdateHuman[0].Sex;
                }

                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "showVal", "  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}Exit('" + args.Replace("'", "&apos") + "');", true);


            }
            else
            {
                hdnIsmailsend.Value = "true";
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380048'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }

        }

        protected void ddlAuthourPlanName_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (ddlauthinsplan.SelectedIndex != 0)
            {


                if (ddlauthPayer.SelectedItem.Text == "OTHER" && ddlauthinsplan.SelectedItem.Text == "OTHER")
                {

                }
                else if (ddlauthPayer.SelectedItem.Text != "OTHER")
                {
                    IList<InsurancePlan> patInsList = (IList<InsurancePlan>)Session["PatInsuredList"];
                    var insplan = from ip in patInsList where ip.Id == Convert.ToUInt64(ddlauthinsplan.Items[ddlauthinsplan.SelectedIndex].Value) select ip;
                    IList<InsurancePlan> insplanlist = (insplan.ToList<InsurancePlan>().OrderBy(s => s.Ins_Plan_Name)).ToList<InsurancePlan>();

                    //IList<InsurancePlan> insplanlist = ((InsMngr.GetInsurancebyID(Convert.ToUInt64(ddlauthinsplan.Items[ddlauthinsplan.SelectedIndex].Value))).OrderBy(s => s.Ins_Plan_Name)).ToList<InsurancePlan>();
                    if (insplanlist != null && insplanlist.Count > 0)
                    {
                        txtStreet.Text = insplanlist[0].Claim_Address;
                        //msktxtZipcode.Text = insplanlist[0].Claim_ZipCode;
                        txtClaimCity.Text = insplanlist[0].Claim_City;
                        txtClaimCity2.Text = insplanlist[0].Claim_City;
                        txtClaimState.Text = insplanlist[0].Claim_State;
                    }

                }
            }


            if (ddlauthinsplan.SelectedItem != null && ddlauthinsplan.SelectedItem.Text != null && ddlauthinsplan.SelectedItem.Text != string.Empty)
            {
                txtauthnumber.Enabled = true;
                txtauthnumber.CssClass = "Editabletxtbox";

            }
            else
            {
                txtauthnumber.Enabled = false;
                txtauthnumber.Text = string.Empty;
                txtauthnumber.CssClass = "nonEditabletxtbox";
            }



            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }

        protected void cboVerificationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            btnSave.CssClass = "aspresizedgreenbutton";
        }



        protected void grdExistingPolicies_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (grdExistingPolicies.SelectedRow.Cells[1].Text != "&nbsp;")
            {
                txtPolicyHolderID.Text = grdExistingPolicies.SelectedRow.Cells[1].Text;
            }
            else
            {
                txtPolicyHolderID.Text = "";
            }
            if (grdExistingPolicies.SelectedRow.Cells[9].Text != "&nbsp;")
            {
                if (grdExistingPolicies.SelectedRow.Cells[9].Text == "01-Jan-0001")
                {
                    dtpEffectiveStartDate.Text = "";
                }
                else
                {
                    dtpEffectiveStartDate.Text = grdExistingPolicies.SelectedRow.Cells[9].Text;
                }
            }
            if (grdExistingPolicies.SelectedRow.Cells[10].Text != "&nbsp;")
            {
                if (grdExistingPolicies.SelectedRow.Cells[10].Text == "01-Jan-0001")
                {
                    dtpTerminationDate.Text = "";
                }
                else
                {
                    dtpTerminationDate.Text = grdExistingPolicies.SelectedRow.Cells[10].Text;
                }
            }

            if (grdExistingPolicies.SelectedRow != null)
            {
                if (grdExistingPolicies.SelectedRow.Cells[19].Text == "ELECTRONIC" && grdExistingPolicies.SelectedRow.Cells[19].Text != "null")
                {
                    //CAP-828
                    if (cboVerificationType.Items.FindByValue("ELECTRONIC") != null)
                    {
                        cboVerificationType.SelectedValue = "ELECTRONIC";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "DisplayErrorMessage('1011201');", true);
                    }
                }
                else
                {
                    if (grdExistingPolicies.SelectedRow.Cells[19].Text == "&nbsp;")
                    {
                        cboVerificationType.SelectedValue = "";
                    }
                    else
                    {
                        cboVerificationType.SelectedValue = grdExistingPolicies.SelectedRow.Cells[19].Text;
                    }

                }
            }
            if (grdExistingPolicies.SelectedRow.Cells[4].Text != "&nbsp;")
            {
                txtGroupNumber.Text = grdExistingPolicies.SelectedRow.Cells[4].Text;
            }
            else
            {
                txtGroupNumber.Text = "";
            }

            //if (grdExistingPolicies.SelectedRow.Cells[8].Text != "&nbsp;")
            //{
            //    txtDemoNote.Text = grdExistingPolicies.SelectedRow.Cells[8].Text;

            //    //if (grdExistingPolicies.SelectedRow.Cells[4].Text != "&nbsp;")
            //    //{
            //    //    txtDeductibleMet.Text = grdExistingPolicies.SelectedRow.Cells[17].Text;
            //    //}
            //    //else
            //    //{
            //    //    txtDeductibleMet.Text = "";
            //    //}
            //}


            for (int i = 0; i < ddlPayerName.Items.Count; i++)
            {
                if (ddlPayerName.Items[i].Value == grdExistingPolicies.SelectedRow.Cells[11].Text)
                {
                    ddlPayerName.SelectedIndex = i;
                    ddlPayerName_SelectedIndexChanged(sender, e);
                    ddlauthPayer.SelectedIndex = i;
                    ddlAuthourPayerName_SelectedIndexChanged(sender, e);
                    break;
                }
            }

            for (int i = 0; i < ddlPlanName.Items.Count; i++)
            {
                if (ddlPlanName.Items[i].Value == grdExistingPolicies.SelectedRow.Cells[2].Text)
                {
                    ddlPlanName.SelectedIndex = i;
                    break;
                    // ddlauthinsplan.SelectedIndex = i;
                }
            }
            for (int i = 0; i < ddlauthinsplan.Items.Count; i++)
            {
                if (ddlauthinsplan.Items[i].Value == grdExistingPolicies.SelectedRow.Cells[2].Text)
                {
                    ddlauthinsplan.SelectedIndex = i;
                    break;
                }
            }




            if (Session["EligibilityList"] != null)
            {

                IList<Eligibility_Verification> lstEligibilityList = (IList<Eligibility_Verification>)Session["EligibilityList"];
                var EligibilityList = (from c in lstEligibilityList where c.Insurance_Plan_ID == Convert.ToUInt64(grdExistingPolicies.Rows[0].Cells[2].Text) && c.Policy_Holder_ID == grdExistingPolicies.Rows[0].Cells[1].Text select c).OrderByDescending(a => a.Created_Date_And_Time);
                if (EligibilityList.ToList<Eligibility_Verification>().Count() > 0)
                {
                    if (EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_From_Date.ToString("dd-MMM-yyyy") == EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_To_Date.ToString("dd-MMM-yyyy"))
                    {
                        txtEligibilityVerificationDate.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_From_Date.ToString("dd-MMM-yyyy");

                    }
                    else
                    {
                        txtEligibilityVerificationDate.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_From_Date.ToString("dd-MMM-yyyy") + " to " + EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_To_Date.ToString("dd-MMM-yyyy");

                    }

                    if (txtEligibilityVerificationDate.Text == "01-Jan-0001")
                    {
                        txtEligibilityVerificationDate.Text = string.Empty;
                    }
                    if (EligibilityList.ToList<Eligibility_Verification>().Count > 0)
                    {
                        if (EligibilityList.ToList<Eligibility_Verification>().Count > 0)
                        {
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Response_Message != null && EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status != null)
                            {
                                string status = "";

                                if (EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString() != "" && !EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString().ToUpper().Contains("FAIL"))
                                {
                                    status = EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString();
                                }
                                if (EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString() != "")
                                {
                                    if (EligibilityList.ToList<Eligibility_Verification>()[0].Response_Message.ToString() != "")
                                        status = EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString() + " - " + EligibilityList.ToList<Eligibility_Verification>()[0].Response_Message.ToString();
                                    else
                                        status = EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString();

                                }

                                else if (EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString().ToUpper().Contains("CONTACT SUPPORT"))
                                {
                                    status = "ERROR";
                                }

                                else if (EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Check_Mode.ToString().ToUpper().Trim() != "" && EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Check_Mode.ToString().ToUpper() == "MANUAL")
                                {
                                    status = "EV-PERFORMED MANUALLY";
                                }
                                else
                                {
                                    status = "EV-NOT PERFORMED";
                                }

                                txtErrorMessage.Text = status;

                            }
                        }
                    }
                    else
                    {
                        txtErrorMessage.Text = "EV-NOT PERFORMED";
                    }
                    if (EligibilityList.ToList<Eligibility_Verification>()[0].Comments != null)
                    {
                        txtDemoNote.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Comments.ToString();
                    }

                    //new Columns add

                    txtInsurancetype.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Plan_Type.ToString();


                    if (EligibilityList.ToList<Eligibility_Verification>()[0].Plan_Number != null)
                    {
                        txtPlanNumber.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Plan_Number.ToString();
                    }
                    else
                    {
                        txtPlanNumber.Text = "";
                    }
                    if (EligibilityList.ToList<Eligibility_Verification>()[0].Organization != null)
                    {
                        txtOrganization.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Organization.ToString();
                    }
                    else
                    {
                        txtOrganization.Text = "";
                    }
                    if (EligibilityList.ToList<Eligibility_Verification>()[0].Subscriber_Name != null)
                    {
                        txtSubscriberName.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Subscriber_Name.ToString();
                    }
                    else
                    {
                        txtSubscriberName.Text = "";
                    }
                    if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Name != null)
                    {
                        txtPCPName.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Name.ToString();
                    }
                    else
                    {
                        txtPCPName.Text = "";
                    }
                    if (EligibilityList.ToList<Eligibility_Verification>()[0].Relationship_to_Subscriber != null)
                    {
                        txtRelationship.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Relationship_to_Subscriber.ToString();
                    }
                    else
                    {
                        txtRelationship.Text = "";
                    }
                    if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_NPI != null)
                    {
                        txtPCP_NPI.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_NPI.ToString();
                    }
                    else
                    {
                        txtPCP_NPI.Text = "";
                    }
                    if (EligibilityList.ToList<Eligibility_Verification>()[0].Group_Number != null)
                    {
                        txtGroupNumber.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Group_Number.ToString();
                    }
                    else
                    {
                        txtGroupNumber.Text = "";
                    }
                    if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Effective_Date.ToString("dd-MMM-yyyy") != null)
                    {
                        dtpPCPEffectiveDate.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Effective_Date.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        dtpPCPEffectiveDate.Text = "";
                    }
                    if (dtpPCPEffectiveDate.Text == "01-Jan-0001")
                    {
                        dtpPCPEffectiveDate.Text = string.Empty;
                    }
                    if (EligibilityList.ToList<Eligibility_Verification>()[0].Group_Name != null)
                    {
                        txtGroupName.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Group_Name.ToString();
                    }
                    else
                    {
                        txtGroupName.Text = "";
                    }
                    if (EligibilityList.ToList<Eligibility_Verification>()[0].IPA_Name != null)
                    {
                        txtIPAName.Text = EligibilityList.ToList<Eligibility_Verification>()[0].IPA_Name.ToString();
                    }
                    else
                    {
                        txtIPAName.Text = "";
                    }



                    txtPCPVisitInCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Office_Visit_InNet_Copay.ToString();
                    txtPCPVisitInCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Office_Visit_InNet_CoIns.ToString();
                    txtPCPVisitOutCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Office_Visit_OutNet_Copay.ToString();
                    txtPCPVisitOutCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Office_Visit_OutNet_CoIns.ToString();

                    txtSpecialityVisitInCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Specialty_Office_Visit_InNet_Copay.ToString();
                    txtSpecialityVisitInCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Specialty_Office_Visit_InNet_CoIns.ToString();
                    txtSpecialityVisitOutCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Specialty_Office_Visit_OutNet_Copay.ToString();
                    txtSpecialityVisitOutCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Specialty_Office_Visit_OutNet_CoIns.ToString();


                    txtMedicationInCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Inj_Medication_InNet_Copay.ToString();
                    txtMedicationInCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Inj_Medication_InNet_CoIns.ToString();
                    txtMedicationOutCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Inj_Medication_OutNet_Copay.ToString();
                    txtMedicationOutCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Inj_Medication_OutNet_CoIns.ToString();


                    txtUrgentCareInCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Urgent_Care_InNet_Copay.ToString();
                    txtUrgentCareCoInIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Urgent_Care_InNet_CoIns.ToString();
                    txtUrgentCareOutCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Urgent_Care_OutNet_Copay.ToString();
                    txtUrgentCareOutCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Urgent_Care_OutNet_CoIns.ToString();


                    if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_InNetwork_Copay_Message != null)
                    {
                        txtPCPInNetworkMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_InNetwork_Copay_Message.ToString();
                    }
                    else
                    {
                        txtPCPInNetworkMessage.Text = "";
                    }
                    if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_OutNetwork_Copay_Message != null)
                    {
                        txtPCPOutNetworkMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_OutNetwork_Copay_Message.ToString();
                    }
                    else
                    {
                        txtPCPOutNetworkMessage.Text = "";
                    }


                    txtInDeductiblePlan.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_per_plan_InNet_Deductible.ToString();
                    txtInPockot.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_per_plan_InNet_Out_of_Pocket.ToString();
                    txtOutDeductiblePlan.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_per_plan_OutNet_Deductible.ToString();
                    txtOutPocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_per_plan_OutNet_Out_of_Pocket.ToString();

                    txtInDeductiblemet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_met_InNet_Deductible.ToString();
                    txtInpocketmet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_met_InNet_Out_of_Pocket.ToString();
                    txtOutDeductiblemet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_met_OutNet_Deductible.ToString();
                    txtOutpocketmet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_met_OutNet_Out_of_Pocket.ToString();

                    txtInFamilyDeductible.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_per_plan_InNet_Deductible.ToString();
                    txtInFamilypocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_per_plan_InNet_Out_of_Pocket.ToString();
                    txtOutFamilyDeductible.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_per_plan_OutNet_Deductible.ToString();
                    txtOutFamilypocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_per_plan_OutNet_Out_of_Pocket.ToString();

                    txtInFamilyDeductiblemet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_met_InNet_Deductible.ToString();
                    txtInFamilymetpocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_met_InNet_Out_of_Pocket.ToString();
                    txtOutFamilyDeductiblemet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_met_OutNet_Deductible.ToString();
                    txtOutFamilymetpocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_met_OutNet_Out_of_Pocket.ToString();

                    if (EligibilityList.ToList<Eligibility_Verification>()[0].Deductible_InNetwork_Message != null)
                    {
                        txtDeductibleInNetworkMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Deductible_InNetwork_Message.ToString();
                    }
                    else
                    {
                        txtDeductibleInNetworkMessage.Text = "";
                    }
                    if (EligibilityList.ToList<Eligibility_Verification>()[0].Deductible_OutNetwork_Message != null)
                    {
                        txtDeductibleOutNetworkMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Deductible_OutNetwork_Message.ToString();
                    }
                    else
                    {
                        txtDeductibleOutNetworkMessage.Text = "";
                    }

                }
                else
                {
                    txtPCPVisitInCopay.Text = string.Empty;
                    txtPCPVisitInCoIns.Text =
                    txtPCPVisitOutCopay.Text = string.Empty;
                    txtPCPVisitOutCoIns.Text = string.Empty;
                    txtSpecialityVisitInCopay.Text = string.Empty;
                    txtSpecialityVisitInCoIns.Text = string.Empty;
                    txtSpecialityVisitOutCopay.Text = string.Empty;
                    txtSpecialityVisitOutCoIns.Text = string.Empty;
                    txtMedicationInCopay.Text = string.Empty;
                    txtMedicationInCoIns.Text = string.Empty;
                    txtMedicationOutCopay.Text = string.Empty;
                    txtMedicationOutCoIns.Text = string.Empty;
                    txtUrgentCareInCopay.Text = string.Empty;
                    txtUrgentCareCoInIns.Text = string.Empty;
                    txtUrgentCareOutCopay.Text = string.Empty;
                    txtUrgentCareOutCoIns.Text = string.Empty;
                    txtInDeductiblePlan.Text = string.Empty;
                    txtInPockot.Text = string.Empty;
                    txtOutDeductiblePlan.Text = string.Empty;
                    txtOutPocket.Text = string.Empty;
                    txtInDeductiblemet.Text = string.Empty;
                    txtInpocketmet.Text = string.Empty;
                    txtOutDeductiblemet.Text = string.Empty;
                    txtOutpocketmet.Text = string.Empty;
                    txtInFamilyDeductible.Text = string.Empty;
                    txtInFamilypocket.Text = string.Empty;
                    txtOutFamilyDeductible.Text = string.Empty;
                    txtOutFamilypocket.Text = string.Empty;
                    txtInFamilyDeductiblemet.Text = string.Empty;
                    txtInFamilymetpocket.Text = string.Empty;
                    txtOutFamilyDeductiblemet.Text = string.Empty;
                    txtOutFamilymetpocket.Text = string.Empty;
                    txtPlanNumber.Text = string.Empty;
                    txtSubscriberName.Text = string.Empty;
                    txtRelationship.Text = string.Empty;
                    txtGroupName.Text = string.Empty;
                    txtPCPName.Text = string.Empty;
                    txtPCP_NPI.Text = string.Empty;
                    dtpPCPEffectiveDate.Text = string.Empty;
                    txtIPAName.Text = string.Empty;
                    txtPCPInNetworkMessage.Text = string.Empty;
                    txtPCPOutNetworkMessage.Text = string.Empty;
                    txtDeductibleInNetworkMessage.Text = string.Empty;
                    txtDeductibleOutNetworkMessage.Text = string.Empty;
                    txtInsurancetype.Text = string.Empty;
                    txtOrganization.Text = string.Empty;

                }

            }

            IList<InsurancePlan> InsList = new List<InsurancePlan>();
            InsurancePlanManager obj = new InsurancePlanManager();
            if (grdExistingPolicies.Rows.Count > 0 && grdExistingPolicies.Rows[grdExistingPolicies.SelectedIndex].Cells[2].Text != null)
            {
                IList<InsurancePlan> patInsList = (IList<InsurancePlan>)Session["PatInsuredList"];
                var insplan = from ip in patInsList where ip.Id == Convert.ToUInt64(grdExistingPolicies.Rows[grdExistingPolicies.SelectedIndex].Cells[2].Text) select ip;
                InsList = insplan.ToList<InsurancePlan>();

                InsurancePlan objInsPlan = null;
                if (InsList != null && InsList.Count > 0)
                {
                    objInsPlan = InsList[0];

                    // msktxtClaimZipCode.Text = objInsPlan.Claim_ZipCode;
                    txtClaimCity.Text = objInsPlan.Claim_City;
                    txtClaimCity2.Text = objInsPlan.Claim_City;
                    txtStreet.Text = objInsPlan.Claim_Address;
                    txtClaimState.Text = objInsPlan.Claim_State;
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);


        }


        protected void grdExistingPolicies_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView _gridView = (GridView)sender;
            // Get the selected index and the command name
            int _selectedIndex = int.Parse(e.CommandArgument.ToString());
            string _commandName = e.CommandName;




            switch (_commandName)
            {
                case ("SingleClick"):
                    _gridView.SelectedIndex = _selectedIndex;
                    hdnSelectedIndex.Value = _selectedIndex.ToString();

                    cboVerificationType.Items.Remove("ELECTRONIC");
                    if (grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[19].Text == "ELECTRONIC" && grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[19].Text != null && grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[19].Text != "&nbsp;")
                    {

                        cboVerificationType.Items.Add("ELECTRONIC");
                        cboVerificationType.SelectedValue = "ELECTRONIC";
                        if (chkEligibilityVerified.Checked == true)
                        {
                            cboVerificationType.SelectedValue = "ELECTRONIC";
                            cboVerificationType.Items.Remove("ELECTRONIC");
                        }
                    }
                    else
                    {
                        cboVerificationType.Items.Remove("ELECTRONIC");

                        if (grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[19].Text == "&nbsp;")
                        {
                            cboVerificationType.SelectedValue = "";
                        }
                        else
                        {
                            cboVerificationType.SelectedValue = grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[19].Text;
                        }
                    }

                    if (grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[1].Text != "&nbsp;")
                    {
                        txtPolicyHolderID.Text = grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[1].Text;
                    }
                    else
                    {
                        txtPolicyHolderID.Text = "";
                    }
                    if (grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[9].Text != "&nbsp;")
                    {
                        if (grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[9].Text == "01-Jan-0001")
                        {
                            dtpEffectiveStartDate.Text = "";
                        }
                        else
                        {
                            dtpEffectiveStartDate.Text = grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[9].Text;
                        }
                    }
                    else if (grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[9].Text == "&nbsp;")
                    {
                        dtpEffectiveStartDate.Text = "";
                    }
                    if (grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[10].Text != "&nbsp;")
                    {
                        if (grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[10].Text == "01-Jan-0001")
                        {
                            dtpTerminationDate.Text = "";
                        }
                        else
                        {
                            dtpTerminationDate.Text = grdExistingPolicies.SelectedRow.Cells[10].Text;
                        }
                    }
                    else if (grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[10].Text == "&nbsp;")
                    {
                        dtpTerminationDate.Text = "";
                    }

                    if (grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[4].Text != "&nbsp;")
                    {
                        txtGroupNumber.Text = grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[4].Text;
                    }
                    else
                    {
                        txtGroupNumber.Text = "";
                    }

                    for (int i = 0; i < ddlPayerName.Items.Count; i++)
                    {
                        if (ddlPayerName.Items[i].Value == grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[11].Text)
                        {
                            ddlPayerName.SelectedIndex = i;
                            ddlPayerName_SelectedIndexChanged(sender, e);
                            break;
                        }
                    }

                    for (int i = 0; i < ddlauthPayer.Items.Count; i++)
                    {
                        if (ddlauthPayer.Items[i].Value == grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[11].Text)
                        {
                            ddlauthPayer.SelectedIndex = i;
                            ddlAuthourPayerName_SelectedIndexChanged(sender, e);
                            break;
                        }
                    }

                    for (int i = 0; i < ddlPlanName.Items.Count; i++)
                    {
                        if (ddlPlanName.Items[i].Value == grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[2].Text)
                        {
                            ddlPlanName.SelectedIndex = i;
                            break;
                        }
                    }

                    for (int i = 0; i < ddlauthinsplan.Items.Count; i++)
                    {
                        if (ddlauthinsplan.Items[i].Value == grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[2].Text)
                        {
                            ddlauthinsplan.SelectedIndex = i;
                            break;
                        }
                    }


                    if (Session["EligibilityList"] != null)
                    {

                        IList<Eligibility_Verification> lstEligibilityList = (IList<Eligibility_Verification>)Session["EligibilityList"];
                        var EligibilityList = (from c in lstEligibilityList where c.Insurance_Plan_ID == Convert.ToUInt64(grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[2].Text) && c.Policy_Holder_ID == grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[1].Text select c).OrderByDescending(a => a.Created_Date_And_Time);

                        if (EligibilityList.ToList<Eligibility_Verification>().Count() > 0)
                        {
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_From_Date.ToString("dd-MMM-yyyy") == EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_To_Date.ToString("dd-MMM-yyyy"))
                            {
                                txtEligibilityVerificationDate.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_From_Date.ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                txtEligibilityVerificationDate.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_From_Date.ToString("dd-MMM-yyyy") + " to " + EligibilityList.ToList<Eligibility_Verification>()[0].Requested_For_To_Date.ToString("dd-MMM-yyyy");
                            }

                            //if (EligibilityList.ToList<Eligibility_Verification>()[0].Response_Message != null && EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status != null)
                            //{
                            //    txtErrorMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Response_Message + " - " + EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status;
                            //}
                            if (EligibilityList.ToList<Eligibility_Verification>().Count > 0)
                            {
                                if (EligibilityList.ToList<Eligibility_Verification>()[0].Response_Message != null && EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status != null)
                                {
                                    string status = "";

                                    if (EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString() != "" && !EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString().ToUpper().Contains("FAIL"))
                                    {
                                        status = EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString();
                                    }
                                    if (EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString() != "")
                                    {
                                        if (EligibilityList.ToList<Eligibility_Verification>()[0].Response_Message.ToString() != "")
                                            status = EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString() + " - " + EligibilityList.ToList<Eligibility_Verification>()[0].Response_Message.ToString();
                                        else
                                            status = EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString();

                                    }

                                    else if (EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Status.ToString().ToUpper().Contains("CONTACT SUPPORT"))
                                    {
                                        status = "Error";
                                    }

                                    else if (EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Check_Mode.ToString().ToUpper().Trim() != "" && EligibilityList.ToList<Eligibility_Verification>()[0].Eligibility_Check_Mode.ToString().ToUpper() == "MANUAL")
                                    {
                                        status = "EV-PERFORMED MANUALLY";
                                    }
                                    else
                                    {
                                        status = "EV-NOT PERFORMED";
                                    }

                                    txtErrorMessage.Text = status;

                                }
                            }
                            else
                            {
                                txtErrorMessage.Text = "EV-NOT PERFORMED";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Comments != string.Empty && EligibilityList.ToList<Eligibility_Verification>()[0].Comments != "&nbsp;")
                            {
                                txtDemoNote.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Comments.ToString();
                            }
                            else
                            {
                                txtDemoNote.Text = "";
                            }


                            txtInsurancetype.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Plan_Type.ToString();

                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Plan_Number != null)
                            {
                                txtPlanNumber.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Plan_Number.ToString();
                            }
                            else
                            {
                                txtPlanNumber.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Organization != null)
                            {
                                txtOrganization.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Organization.ToString();
                            }
                            else
                            {
                                txtOrganization.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Subscriber_Name != null)
                            {
                                txtSubscriberName.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Subscriber_Name.ToString();
                            }
                            else
                            {
                                txtSubscriberName.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Name != null)
                            {
                                txtPCPName.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Name.ToString();
                            }
                            else
                            {
                                txtPCPName.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Relationship_to_Subscriber != null)
                            {
                                txtRelationship.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Relationship_to_Subscriber.ToString();
                            }
                            else
                            {
                                txtRelationship.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_NPI != null)
                            {
                                txtPCP_NPI.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_NPI.ToString();
                            }
                            else
                            {
                                txtPCP_NPI.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Group_Number != null)
                            {
                                txtGroupNumber.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Group_Number.ToString();
                            }
                            else
                            {
                                txtGroupNumber.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Effective_Date.ToString("dd-MMM-yyyy") != null)
                            {
                                dtpPCPEffectiveDate.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Effective_Date.ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                dtpPCPEffectiveDate.Text = "";
                            }
                            if (dtpPCPEffectiveDate.Text == "01-Jan-0001")
                            {
                                dtpPCPEffectiveDate.Text = string.Empty;
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Group_Name != null)
                            {
                                txtGroupName.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Group_Name.ToString();
                            }
                            else
                            {
                                txtGroupName.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].IPA_Name != null)
                            {
                                txtIPAName.Text = EligibilityList.ToList<Eligibility_Verification>()[0].IPA_Name.ToString();
                            }
                            else
                            {
                                txtIPAName.Text = "";
                            }



                            txtPCPVisitInCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Office_Visit_InNet_Copay.ToString();
                            txtPCPVisitInCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Office_Visit_InNet_CoIns.ToString();
                            txtPCPVisitOutCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Office_Visit_OutNet_Copay.ToString();
                            txtPCPVisitOutCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_Office_Visit_OutNet_CoIns.ToString();

                            txtSpecialityVisitInCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Specialty_Office_Visit_InNet_Copay.ToString();
                            txtSpecialityVisitInCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Specialty_Office_Visit_InNet_CoIns.ToString();
                            txtSpecialityVisitOutCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Specialty_Office_Visit_OutNet_Copay.ToString();
                            txtSpecialityVisitOutCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Specialty_Office_Visit_OutNet_CoIns.ToString();

                            txtMedicationInCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Inj_Medication_InNet_Copay.ToString();
                            txtMedicationInCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Inj_Medication_InNet_CoIns.ToString();
                            txtMedicationOutCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Inj_Medication_OutNet_Copay.ToString();
                            txtMedicationOutCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Inj_Medication_OutNet_CoIns.ToString();

                            txtUrgentCareInCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Urgent_Care_InNet_Copay.ToString();
                            txtUrgentCareCoInIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Urgent_Care_InNet_CoIns.ToString();
                            txtUrgentCareOutCopay.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Urgent_Care_OutNet_Copay.ToString();
                            txtUrgentCareOutCoIns.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Urgent_Care_OutNet_CoIns.ToString();


                            if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_InNetwork_Copay_Message != null)
                            {
                                txtPCPInNetworkMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_InNetwork_Copay_Message.ToString();
                            }
                            else
                            {
                                txtPCPInNetworkMessage.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].PCP_OutNetwork_Copay_Message != null)
                            {
                                txtPCPOutNetworkMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].PCP_OutNetwork_Copay_Message.ToString();
                            }
                            else
                            {
                                txtPCPOutNetworkMessage.Text = "";
                            }




                            txtInDeductiblePlan.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_per_plan_InNet_Deductible.ToString();
                            txtInPockot.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_per_plan_InNet_Out_of_Pocket.ToString();
                            txtOutDeductiblePlan.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_per_plan_OutNet_Deductible.ToString();
                            txtOutPocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_per_plan_OutNet_Out_of_Pocket.ToString();

                            txtInDeductiblemet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_met_InNet_Deductible.ToString();
                            txtInpocketmet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_met_InNet_Out_of_Pocket.ToString();
                            txtOutDeductiblemet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_met_OutNet_Deductible.ToString();
                            txtOutpocketmet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Ind_met_OutNet_Out_of_Pocket.ToString();

                            txtInFamilyDeductible.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_per_plan_InNet_Deductible.ToString();
                            txtInFamilypocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_per_plan_InNet_Out_of_Pocket.ToString();
                            txtOutFamilyDeductible.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_per_plan_OutNet_Deductible.ToString();
                            txtOutFamilypocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_per_plan_OutNet_Out_of_Pocket.ToString();

                            txtInFamilyDeductiblemet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_met_InNet_Deductible.ToString();
                            txtInFamilymetpocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_met_InNet_Out_of_Pocket.ToString();
                            txtOutFamilyDeductiblemet.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_met_OutNet_Deductible.ToString();
                            txtOutFamilymetpocket.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Family_met_OutNet_Out_of_Pocket.ToString();


                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Deductible_InNetwork_Message != null)
                            {
                                txtDeductibleInNetworkMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Deductible_InNetwork_Message.ToString();
                            }
                            else
                            {
                                txtDeductibleInNetworkMessage.Text = "";
                            }
                            if (EligibilityList.ToList<Eligibility_Verification>()[0].Deductible_OutNetwork_Message != null)
                            {
                                txtDeductibleOutNetworkMessage.Text = EligibilityList.ToList<Eligibility_Verification>()[0].Deductible_OutNetwork_Message.ToString();
                            }
                            else
                            {
                                txtDeductibleOutNetworkMessage.Text = "";
                            }


                        }
                        else
                        {
                            txtEligibilityVerificationDate.Text = string.Empty;
                            txtErrorMessage.Text = string.Empty;
                            txtDemoNote.Text = "";



                            txtPCPVisitInCopay.Text = string.Empty;
                            txtPCPVisitInCoIns.Text =
                            txtPCPVisitOutCopay.Text = string.Empty;
                            txtPCPVisitOutCoIns.Text = string.Empty;
                            txtSpecialityVisitInCopay.Text = string.Empty;
                            txtSpecialityVisitInCoIns.Text = string.Empty;
                            txtSpecialityVisitOutCopay.Text = string.Empty;
                            txtSpecialityVisitOutCoIns.Text = string.Empty;
                            txtMedicationInCopay.Text = string.Empty;
                            txtMedicationInCoIns.Text = string.Empty;
                            txtMedicationOutCopay.Text = string.Empty;
                            txtMedicationOutCoIns.Text = string.Empty;
                            txtUrgentCareInCopay.Text = string.Empty;
                            txtUrgentCareCoInIns.Text = string.Empty;
                            txtUrgentCareOutCopay.Text = string.Empty;
                            txtUrgentCareOutCoIns.Text = string.Empty;
                            txtInDeductiblePlan.Text = string.Empty;
                            txtInPockot.Text = string.Empty;
                            txtOutDeductiblePlan.Text = string.Empty;
                            txtOutPocket.Text = string.Empty;
                            txtInDeductiblemet.Text = string.Empty;
                            txtInpocketmet.Text = string.Empty;
                            txtOutDeductiblemet.Text = string.Empty;
                            txtOutpocketmet.Text = string.Empty;
                            txtInFamilyDeductible.Text = string.Empty;
                            txtInFamilypocket.Text = string.Empty;
                            txtOutFamilyDeductible.Text = string.Empty;
                            txtOutFamilypocket.Text = string.Empty;
                            txtInFamilyDeductiblemet.Text = string.Empty;
                            txtInFamilymetpocket.Text = string.Empty;
                            txtOutFamilyDeductiblemet.Text = string.Empty;
                            txtOutFamilymetpocket.Text = string.Empty;
                            txtPlanNumber.Text = string.Empty;
                            txtSubscriberName.Text = string.Empty;
                            txtRelationship.Text = string.Empty;
                            txtGroupName.Text = string.Empty;
                            txtPCPName.Text = string.Empty;
                            txtPCP_NPI.Text = string.Empty;
                            dtpPCPEffectiveDate.Text = string.Empty;
                            txtIPAName.Text = string.Empty;
                            txtPCPInNetworkMessage.Text = string.Empty;
                            txtPCPOutNetworkMessage.Text = string.Empty;
                            txtDeductibleInNetworkMessage.Text = string.Empty;
                            txtDeductibleOutNetworkMessage.Text = string.Empty;
                            txtInsurancetype.Text = string.Empty;
                            txtOrganization.Text = string.Empty;

                        }
                    }

                    if (txtEligibilityVerificationDate.Text == "01-Jan-0001")
                    {
                        txtEligibilityVerificationDate.Text = string.Empty;
                    }

                    if (grdExistingPolicies.Rows.Count > 0)  //doubt//
                    {
                        if (Session["EligibilityList"] != null)
                        {
                            IList<Eligibility_Verification> lstEligibilityList = (IList<Eligibility_Verification>)Session["EligibilityList"];
                            int iRowIndex = _gridView.SelectedIndex;
                            var EligibilityList = (from c in lstEligibilityList where c.Insurance_Plan_ID.ToString() == grdExistingPolicies.Rows[iRowIndex].Cells[2].Text.ToString() && c.Policy_Holder_ID == grdExistingPolicies.Rows[iRowIndex].Cells[1].Text select c).OrderByDescending(a => a.Created_Date_And_Time).ToList<Eligibility_Verification>();
                            if (EligibilityList.Count() > 0)
                            {
                                if (EligibilityList[0].Payer_Name.ToString() != string.Empty)
                                {
                                    txtPolicyHolderID.Text = EligibilityList[0].Policy_Holder_ID;
                                    txtGroupNumber.Text = EligibilityList[0].Group_Number;
                                    if (EligibilityList[0].Effective_Date != DateTime.MinValue)
                                    {
                                        dtpEffectiveStartDate.Text = EligibilityList[0].Effective_Date.ToString("dd-MMM-yyyy");
                                    }
                                    if (EligibilityList[0].Termination_Date != DateTime.MinValue)
                                    {
                                        //dtpTerminationDate.SelectedDate = objCheckOut.EligibilityList[iCount].Termination_Date;//.ToString("dd-MMM-yyyy");
                                        dtpTerminationDate.Text = EligibilityList[0].Termination_Date.ToString("dd-MMM-yyyy");
                                    }



                                    txtDemoNote.Text = EligibilityList[0].Demo_Note.ToString();

                                    //txtPCPCopay.Text = EligibilityList[0].PCP_Copay.ToString();
                                    //txtSPCCopay.Text = EligibilityList[0].SPC_Copay.ToString();
                                    //txtDeductible.Text = EligibilityList[0].Deductible_For_Plan.ToString();
                                    //txtDeductibleMet.Text = EligibilityList[0].Deductible_Met_So_Far.ToString();
                                    //txtCoInsurance.Text = EligibilityList[0].Coinsurance.ToString();
                                    txtDemoNote.Text = EligibilityList[0].Comments.ToString();//change comments
                                    //break;
                                }
                            }
                        }
                    }


                    IList<InsurancePlan> InsList = new List<InsurancePlan>();
                    InsurancePlanManager obj = new InsurancePlanManager();
                    if (grdExistingPolicies.Rows.Count > 0 && grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[2].Text != null && Session["PatInsuredList"] != null)
                    {
                        //InsList = obj.GetInsurancebyID(Convert.ToUInt64(grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[2].Text));

                        IList<InsurancePlan> patInsList = (IList<InsurancePlan>)Session["PatInsuredList"];
                        var insplan = from ip in patInsList where ip.Id == Convert.ToUInt64(grdExistingPolicies.Rows[_gridView.SelectedIndex].Cells[2].Text) select ip;
                        InsList = insplan.ToList<InsurancePlan>();

                        InsurancePlan objInsPlan = null;
                        if (InsList != null && InsList.Count > 0)
                        {
                            objInsPlan = InsList[0];

                            //msktxtZipcode.Text = objInsPlan.Claim_ZipCode;
                            txtClaimCity.Text = objInsPlan.Claim_City;
                            txtClaimCity2.Text = objInsPlan.Claim_City;
                            txtStreet.Text = objInsPlan.Claim_Address;
                            txtClaimState.Text = objInsPlan.Claim_State;


                            //msktxtClaimZipCode.Text = objInsPlan.Claim_ZipCode;
                            txtClaimCity.Text = objInsPlan.Claim_City;
                            txtClaimCity2.Text = objInsPlan.Claim_City;
                            txtStreet.Text = objInsPlan.Claim_Address;
                            txtClaimState.Text = objInsPlan.Claim_State;
                        }
                    }
                    break;
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "closeload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();loadAuthafterEV()}", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);


        }

        protected void grdExistingPolicies_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Attributes.Add("style", "display:none");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes.Add("style", "display:none");
                LinkButton _singleClickButton = (LinkButton)e.Row.Cells[20].Controls[0];

                string _jsSingle =
                ClientScript.GetPostBackClientHyperlink(_singleClickButton, "");

                e.Row.Attributes["onclick"] = _jsSingle;

            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow r in grdExistingPolicies.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    Page.ClientScript.RegisterForEventValidation
                            (r.UniqueID + "$ctl00");
                    Page.ClientScript.RegisterForEventValidation
                            (r.UniqueID + "$ctl01");
                }
            }
            base.Render(writer);

            //divLoading.Attributes.Add("display", "none");
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "WaitCursor();", true);

        }

        public void MaskedTextBoxColorChange(RadMaskedTextBox msktxtbox, bool bToNormal)
        {
            if (bToNormal == false)
            {
                msktxtbox.ReadOnly = true;
                //msktxtbox.CssClass = "";
                msktxtbox.CssClass = "nonEditabletxtbox";
                //msktxtbox.Attributes.Add("disabled", "disabled");
                //msktxtbox.Enabled = false;
                // Cap - 669
                msktxtbox.Attributes.Add("disabled", "disabled");

            }
            else
            {
                msktxtbox.ReadOnly = false;
                //msktxtbox.CssClass = "";
                msktxtbox.CssClass = "Editabletxtbox";
                //msktxtbox.Attributes.Add("disabled", "");
                //msktxtbox.Enabled = true;
                //Cap - 669
                msktxtbox.Attributes.Remove("disabled");
                msktxtbox.Attributes.Add("enable", "enable");
            }
        }

        public void CreateEmptyPolicyGridGrid()
        {
            DataTable dtHuman = new DataTable();
            dtHuman.Columns.Add("Policy Holder Id", typeof(string));
            dtHuman.Columns.Add("Plan #", typeof(string));
            dtHuman.Columns.Add("Plan Name", typeof(string));
            dtHuman.Columns.Add("Group #", typeof(string));
            dtHuman.Columns.Add("Ins. Type", typeof(string));
            dtHuman.Columns.Add("Insured Party", typeof(string));
            dtHuman.Columns.Add("Relationship", typeof(string));
            dtHuman.Columns.Add("Active", typeof(string));
            dtHuman.Columns.Add("Eff. Start Date", typeof(string));
            dtHuman.Columns.Add("Term. Date", typeof(string));
            dtHuman.Columns.Add("Carrier #", typeof(string));
            dtHuman.Columns.Add("Insured Name", typeof(string));
            dtHuman.Columns.Add("Insured #", typeof(string));
            dtHuman.Columns.Add("SPC CoPay $", typeof(string));
            dtHuman.Columns.Add("PCP CoPay $", typeof(string));
            dtHuman.Columns.Add("Deduct. $", typeof(string));
            dtHuman.Columns.Add("Co ins. %", typeof(string));
            dtHuman.Columns.Add("Eligibility_Type", typeof(string));
            dtHuman.Columns.Add("Carrier Name", typeof(string));

            grdExistingPolicies.DataSource = dtHuman;
            grdExistingPolicies.DataBind();
        }



        protected void btnUploadDocuments_Click(object sender, EventArgs e)
        {
            RadOnlineWindow.Visible = true;
            RadOnlineWindow.VisibleOnPageLoad = true;
            RadOnlineWindow.Height = 880;
            RadOnlineWindow.Width = 1200;
            RadOnlineWindow.Behaviors = WindowBehaviors.Move | WindowBehaviors.Close;
            RadOnlineWindow.VisibleStatusbar = false;
            RadOnlineWindow.Modal = true;
            RadOnlineWindow.NavigateUrl = "frmOnlineDocuments.aspx?Screen=OnlineDocuments" + "&HuamnId=" + hdnHumanID.Value;
        }

        protected void chkOnlineAccess_CheckedChanged(object sender, EventArgs e)
        {

            if (chkOnlineAccess.Checked)
            {
                spanemail.Attributes.Remove("class");

                spanemail.Attributes.Add("class", "MandLabelstyle");
                spanemailstar.Visible = true;


                txtMail.Attributes.Remove("class");
                txtMail.Attributes.Add("class", "Editabletxtbox");
                txtMail.CssClass = txtMail.CssClass.Replace("nonEditabletxtbox", "Editabletxtbox");
                txtMail.Enabled = true;
                // btnSave.Enabled = false;
                btnSendMail.Enabled = true;
                txtMail.ReadOnly = false;
            }
            else
            {

                spanemail.Attributes.Remove("class");

                spanemail.Attributes.Add("class", "spanstyle");
                spanemailstar.Visible = true;
                spanemailstar.Visible = false;

                btnSendMail.Enabled = false;
                txtMail.Attributes.Remove("class");
                txtMail.Attributes.Add("class", "nonEditabletxtbox");
                txtMail.CssClass = txtMail.CssClass.Replace("Editabletxtbox", "nonEditabletxtbox");
                txtMail.Enabled = false;
                btnSave.Enabled = true;
                btnSave.CssClass = "aspresizedgreenbutton";
                txtMail.Text = string.Empty;
            }
            if (hdnScreenMode.Value.ToUpper() == "FIND PATIENT")
            {
                if (chkOnlineAccess.Checked)
                {
                    txtMail.ReadOnly = false;
                    btnSendMail.Enabled = true;
                    spanemailstar.Visible = true;
                    btnSave.Enabled = true;
                    btnSave.CssClass = "aspresizedgreenbutton";
                }
                else
                {
                    btnSendMail.Enabled = false;
                    txtMail.ReadOnly = true;
                    spanemailstar.Visible = false;
                    btnSave.Enabled = true;
                    btnSave.CssClass = "aspresizedgreenbutton";
                    txtMail.Text = string.Empty;
                }
            }
            else if (hdnScreenMode.Value.ToUpper() == "CHECKEDIN")
            {
                if (chkOnlineAccess.Checked)
                {

                    txtMail.ReadOnly = false;
                    TextBoxColorChange(txtMail, true);
                    spanemailstar.Visible = true;
                    btnSendMail.Enabled = true;
                    btnSave.Enabled = true;
                    btnSave.CssClass = "aspresizedgreenbutton";

                }
                else
                {
                    txtMail.ReadOnly = true;
                    TextBoxColorChange(txtMail, false);
                    btnSendMail.Enabled = false;
                    spanemailstar.Visible = false;

                }
            }

        }

        protected void grdPaymentInformation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditC")
            {
                PaymentInformationClearAll();
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int Rowindex = row.RowIndex;
                grdPaymentInformation.SelectedIndex = Rowindex;
                if (cboMethodOfPayment.Items.Count > 0)
                    cboMethodOfPayment.SelectedIndex = cboMethodOfPayment.Items.IndexOf(cboMethodOfPayment.Items.FindByText(grdPaymentInformation.Rows[Rowindex].Cells[2].Text));
                cboMethodOfPayment_SelectedIndexChanged(sender, e);
                if (grdPaymentInformation.Rows[Rowindex].Cells[3].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[3].Text != "&nbsp;")
                    txtCheckNo.Text = grdPaymentInformation.Rows[Rowindex].Cells[3].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[4].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[4].Text != "&nbsp;")
                    txtAuthNo.Text = grdPaymentInformation.Rows[Rowindex].Cells[4].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[5].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[5].Text != "&nbsp;")
                    txtPastDue.Text = grdPaymentInformation.Rows[Rowindex].Cells[5].Text;
                txtPaymentAmount.Text = grdPaymentInformation.Rows[Rowindex].Cells[6].Text;
                txtRecOnAcc.Text = grdPaymentInformation.Rows[Rowindex].Cells[7].Text;
                txtRefundAmount.Text = grdPaymentInformation.Rows[Rowindex].Cells[8].Text;

                if (grdPaymentInformation.Rows[Rowindex].Cells[9].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[9].Text != "&nbsp;")
                    dtpCheckDate.Text = grdPaymentInformation.Rows[Rowindex].Cells[9].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[10].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[10].Text != "&nbsp;")
                    txtPaymentNote.Text = grdPaymentInformation.Rows[Rowindex].Cells[10].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[11].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[11].Text != "&nbsp;")
                    hdnVisitID.Value = grdPaymentInformation.Rows[Rowindex].Cells[11].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[12].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[12].Text != "&nbsp;")
                    hdnPPHeaderID.Value = grdPaymentInformation.Rows[Rowindex].Cells[12].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[13].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[13].Text != "&nbsp;")
                    hdnPPLineItemID.Value = grdPaymentInformation.Rows[Rowindex].Cells[13].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[14].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[14].Text != "&nbsp;")
                    hdnCheckID.Value = grdPaymentInformation.Rows[Rowindex].Cells[14].Text;

                if (grdPaymentInformation.Rows[Rowindex].Cells[15].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[15].Text != "&nbsp;")
                {
                    if (grdPaymentInformation.Rows[Rowindex].Cells[15].Text == "Patient")
                    {
                        cboRelation.SelectedIndex = 0;
                    }
                    if (grdPaymentInformation.Rows[Rowindex].Cells[15].Text == "Others")
                    {
                        cboRelation.SelectedIndex = 1;
                    }
                }
                if (grdPaymentInformation.Rows[Rowindex].Cells[16].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[16].Text != "&nbsp;")
                    txtpaidBy.Text = grdPaymentInformation.Rows[Rowindex].Cells[16].Text;

                btnAdd.Text = "Update";
                btnClear.Text = "Cancel";
            }
            else if (e.CommandName == "DeleteRow")
            {
                //GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                //int Rowindex = row.RowIndex;
                //grdPaymentInformation.SelectedIndex = Rowindex;
                //ulong Check = 0;
                //ulong PPHeaderId = 0;
                //ulong PPLineId = 0;
                //ulong VisitID = 0;
                //VisitID = Convert.ToUInt64(grdPaymentInformation.Rows[Rowindex].Cells[11].Text);
                //PPHeaderId = Convert.ToUInt64(grdPaymentInformation.Rows[Rowindex].Cells[12].Text);
                //PPLineId = Convert.ToUInt64(grdPaymentInformation.Rows[Rowindex].Cells[13].Text);
                //Check = Convert.ToUInt64(grdPaymentInformation.Rows[Rowindex].Cells[14].Text);
                //SaveVisitPaymentList = new List<VisitPayment>();

                //HumanManager HumanMngr = new HumanManager();
                //FillQuickPatient objcheckout = HumanMngr.GetVisitPaymentDetails(grdPaymentInformation.Rows[Rowindex].Cells[11].Text, grdPaymentInformation.Rows[Rowindex].Cells[12].Text, grdPaymentInformation.Rows[Rowindex].Cells[13].Text, grdPaymentInformation.Rows[Rowindex].Cells[14].Text);
                //if (objcheckout.VisitPaymentList[0] != null)
                //{
                //    objcheckout.VisitPaymentList[0].Is_Delete = "Y";
                //    if (hdnLocalTime.Value != string.Empty)
                //        objcheckout.VisitPaymentList[0].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                //    objcheckout.VisitPaymentList[0].Modified_By = ClientSession.UserName;
                //    SaveVisitPaymentList.Add(objcheckout.VisitPaymentList[0]);
                //}
                //PPHeaderManager PPHeaderMngr = new PPHeaderManager();
                //SavePPHeaderList = new List<PPHeader>();
                //if (objcheckout.PPHeaderList[0] != null)
                //{
                //    objcheckout.PPHeaderList[0].Is_Delete = "Y";
                //    if (hdnLocalTime.Value != string.Empty)
                //        objcheckout.PPHeaderList[0].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                //    objcheckout.PPHeaderList[0].Modified_By = ClientSession.UserName;
                //    SavePPHeaderList.Add(objcheckout.PPHeaderList[0]);
                //}
                //PPLineItemManager PPLineMngr = new PPLineItemManager();
                //SavePPLineItemList = new List<PPLineItem>();
                //if (objcheckout.PPLineItemList[0] != null)
                //{
                //    objcheckout.PPLineItemList[0].Is_Delete = "Y";
                //    if (hdnLocalTime.Value != string.Empty)
                //        objcheckout.PPLineItemList[0].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                //    objcheckout.PPLineItemList[0].Modified_By = ClientSession.UserName;
                //    SavePPLineItemList.Add(objcheckout.PPLineItemList[0]);
                //}
                //CheckManager CheckMngr = new CheckManager();
                //SaveCheckList = new List<Check>();
                //if (objcheckout.CheckList[0] != null)
                //{
                //    objcheckout.CheckList[0].Is_Delete = "Y";
                //    if (hdnLocalTime.Value != string.Empty)
                //        objcheckout.CheckList[0].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                //    objcheckout.CheckList[0].Modified_By = ClientSession.UserName;
                //    SaveCheckList.Add(objcheckout.CheckList[0]);
                //}


                //AccountTransactionManager AccTranMngr = new AccountTransactionManager();
                //IList<AccountTransaction> ilistAccTran = new List<AccountTransaction>();
                //if (objcheckout != null && objcheckout.AccountTransaction.Count > 0)
                //{
                //    for (int iNumber = 0; iNumber < ilistAccTran.Count; iNumber++)
                //    {
                //        objcheckout.AccountTransaction[iNumber].Is_Delete = "Y";
                //        if (hdnLocalTime.Value != string.Empty)
                //            objcheckout.AccountTransaction[iNumber].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                //        objcheckout.AccountTransaction[iNumber].Modified_By = ClientSession.UserName;
                //    }
                //}
                //IList<VisitPaymentDTO> VisitPaymentDTO = new List<VisitPaymentDTO>();
                //if (hdnEncounterID.Value != "")
                //    VisitPaymentDTO = visitMgr.UpdateVisitPayment(SaveVisitPaymentList, SaveCheckList, SavePPHeaderList, SavePPLineItemList, objcheckout.AccountTransaction, null, null, Convert.ToUInt32(hdnEncounterID.Value)); bValidPaymentInfo = true;
                //bValidPaymentInfo = true;
                //SaveVisitPaymentList.Clear();
                //LoadPaymentInfoGrid(VisitPaymentDTO);
                //paymentinformationdisableall();
                //PaymentInformationClearAll();
                //cboMethodOfPayment.SelectedIndex = 0;
                //btnAdd.Text = "Add";
                //btnClear.Text = "Clear All";
                //if (grdPaymentInformation.Rows.Count == 1 && grdPaymentInformation.Rows[0].Visible == false)
                //{
                //    txtTotalAmount.Text = "0.00";
                //}
                PaymentInformationClearAll();
                GridViewRow rows = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int Rowindexs = rows.RowIndex;
                grdPaymentInformation.SelectedIndex = Rowindexs;
                if (cboMethodOfPayment.Items.Count > 0)
                    cboMethodOfPayment.SelectedIndex = cboMethodOfPayment.Items.IndexOf(cboMethodOfPayment.Items.FindByText(grdPaymentInformation.Rows[Rowindexs].Cells[2].Text));
                cboMethodOfPayment_SelectedIndexChanged(sender, e);
                if (grdPaymentInformation.Rows[Rowindexs].Cells[3].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[3].Text != "&nbsp;")
                    txtCheckNo.Text = grdPaymentInformation.Rows[Rowindexs].Cells[3].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[4].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[4].Text != "&nbsp;")
                    txtAuthNo.Text = grdPaymentInformation.Rows[Rowindexs].Cells[4].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[5].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[5].Text != "&nbsp;")
                    txtPastDue.Text = grdPaymentInformation.Rows[Rowindexs].Cells[5].Text;
                txtPaymentAmount.Text = grdPaymentInformation.Rows[Rowindexs].Cells[6].Text;
                txtRecOnAcc.Text = grdPaymentInformation.Rows[Rowindexs].Cells[7].Text;
                txtRefundAmount.Text = grdPaymentInformation.Rows[Rowindexs].Cells[8].Text;

                if (grdPaymentInformation.Rows[Rowindexs].Cells[9].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[9].Text != "&nbsp;")
                    dtpCheckDate.Text = grdPaymentInformation.Rows[Rowindexs].Cells[9].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[10].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[10].Text != "&nbsp;")
                    txtPaymentNote.Text = grdPaymentInformation.Rows[Rowindexs].Cells[10].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[11].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[11].Text != "&nbsp;")
                    hdnVisitID.Value = grdPaymentInformation.Rows[Rowindexs].Cells[11].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[12].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[12].Text != "&nbsp;")
                    hdnPPHeaderID.Value = grdPaymentInformation.Rows[Rowindexs].Cells[12].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[13].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[13].Text != "&nbsp;")
                    hdnPPLineItemID.Value = grdPaymentInformation.Rows[Rowindexs].Cells[13].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[14].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[14].Text != "&nbsp;")
                    hdnCheckID.Value = grdPaymentInformation.Rows[Rowindexs].Cells[14].Text;

                if (grdPaymentInformation.Rows[Rowindexs].Cells[15].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[15].Text != "&nbsp;")
                {
                    if (grdPaymentInformation.Rows[Rowindexs].Cells[15].Text == "Patient")
                    {
                        cboRelation.SelectedIndex = 0;
                    }
                    if (grdPaymentInformation.Rows[Rowindexs].Cells[15].Text == "Others")
                    {
                        cboRelation.SelectedIndex = 1;
                    }
                }
                if (grdPaymentInformation.Rows[Rowindexs].Cells[16].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[16].Text != "&nbsp;")
                    txtpaidBy.Text = grdPaymentInformation.Rows[Rowindexs].Cells[16].Text;

                btnAdd.Text = "Confirm Delete";
                paymentinformationdisableall();
                ComboBoxColorChange(cboRelation, false);
                TextBoxColorChange(txtPaymentNote, true);
                spanPaymentNotes.Attributes.Remove("class"); /*added*/
                spanPaymentNotes.Attributes.Add("class", "MandLabelstyle");
                spanPatientNotestar.Visible = true;
                txtPaymentNote.Enabled = true;
            }
        }

        protected void cboRelation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRelation.SelectedItem.Text == "Others")
            {
                txtpaidBy.Text = "";
                TextBoxColorChange(txtpaidBy, true);
            }
            else if (cboRelation.SelectedItem.Text == "Patient")
            {
                txtpaidBy.Text = txtPatientLastName.Text + ',' + txtPatientFirstName.Text;
                TextBoxColorChange(txtpaidBy, false);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ChangeLabel", "warningmethod();", true);
        }

        private string UploadPhoto()
        {
            FTPImageProcess ftpImage = new FTPImageProcess();
            string ftpServerIP = string.Empty;
            string ftpUserName = string.Empty;
            string ftpPassword = string.Empty;

            ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
            ftpUserName = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
            ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];



            string sPath = string.Empty;
            string sFTPPath = string.Empty;
            // if (fileupload.HasFile)
            if (hdnUploadFile.Value != null)
            {
                try
                {

                    string filename = hdnUploadFile.Value;
                    sFTPPath = ftpImage.UploadToImageServer("PatientPhoto", ftpServerIP, ftpUserName, ftpPassword, filename, string.Empty, out string sCheckFileNotFoundException);
                    if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                        return string.Empty;
                    }
                    //imgOverAllSummary.ImageUrl = "~//atala-capture-download/" + Session.SessionID + "//" + sImgFileName;
                    imgOverAllSummary.ImageUrl = "~//atala-capture-download/" + Session.SessionID + "//" + Path.GetFileName(filename);


                    //    }
                    //}
                }
                catch
                {
                    return string.Empty;

                }
            }
            return sFTPPath;
        }

        public void LoadPatientPhoto(string sPhotoPath)
        {
            FTPImageProcess ftpImage = new FTPImageProcess();
            string ftpServerIP = string.Empty;
            string ftpUserName = string.Empty;
            string ftpPassword = string.Empty;

            ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
            ftpUserName = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
            ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];

            string sPath = Page.MapPath("atala-capture-download/" + Session.SessionID);


            DirectoryInfo virdir = new DirectoryInfo(sPath);
            if (!virdir.Exists)
            {
                virdir.Create();
            }

            ftpImage.DownloadFromImageServer("PatientPhoto", ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(sPhotoPath), sPath, out string sCheckFileNotFoundException);
            if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                return;
            }
            imgOverAllSummary.ImageUrl = "~//atala-capture-download/" + Session.SessionID + "//" + Path.GetFileName(sPhotoPath);



        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            LoadPatientDetails();
            chkEligibilityVerified.Checked = false;
            chkEligibilityVerified_CheckedChanged(sender, e);
            LoadEligibilityVerification();
        }

        protected void btnloadgrid_Click(object sender, EventArgs e)
        {
            LoadPatientDetails();
            chkEligibilityVerified.Checked = false;
            chkEligibilityVerified_CheckedChanged(sender, e);
            LoadEligibilityVerification();
        }

        protected void chkShowAllAuth_CheckedChanged(object sender, EventArgs e)
        {
            ddlauthPayer.Items.Clear();

            if (chkShowAllAuth.Checked == true)
            {
                chkShowAllAuthPlan.Checked = true;
                chkShowAllAuthPlan.Enabled = false;

                CarrierManager carrierMngr = new CarrierManager();
                IList<Carrier> carrierLst = carrierMngr.GetAll();

                if (carrierLst != null && carrierLst.Count > 0)
                {
                    ddlauthPayer.Items.Add("");
                    ddlauthPayer.Items.Add("OTHER");
                    for (int i = 0; i < carrierLst.Count; i++)
                    {
                        ListItem lstAuthCarrier = new ListItem();
                        lstAuthCarrier.Text = carrierLst[i].Carrier_Name;
                        lstAuthCarrier.Value = carrierLst[i].Id.ToString();
                        ddlauthPayer.Items.Add(lstAuthCarrier);
                    }
                }
            }
            else
            {
                chkShowAllAuthPlan.Enabled = true;
                ddlauthPayer.Items.Add("");
                //ddlauthPayer.Items.Add("OTHER");

                CarrierManager carrierMngr = new CarrierManager();
                IList<Carrier> carrierLst = carrierMngr.GetAll();

                for (int i = 0; i < grdExistingPolicies.Rows.Count; i++)
                {
                    ListItem lstAuthCarrier = new ListItem();
                    IList<string> tempcarrierLst = new List<string>();

                    if (carrierLst != null && carrierLst.Count > 0)
                    {
                        var car = from c in carrierLst where c.Id == Convert.ToUInt64(grdExistingPolicies.Rows[i].Cells[11].Text) select c.Carrier_Name;
                        tempcarrierLst = car.ToList<string>();
                        if (tempcarrierLst != null && tempcarrierLst.Count > 0)
                        {
                            lstAuthCarrier.Text = car.ToList<string>()[0].ToString();
                            lstAuthCarrier.Value = grdExistingPolicies.Rows[i].Cells[11].Text;
                        }
                    }
                    //lstAuthCarrier.Text = grdExistingPolicies.Rows[i].Cells[3].Text;
                    // lstAuthCarrier.Value = grdExistingPolicies.Rows[i].Cells[11].Text;
                    ddlauthPayer.Items.Add(lstAuthCarrier);
                }
            }
            for (int i = 0; i < ddlauthPayer.Items.Count; i++)
            {
                if (grdExistingPolicies.Rows.Count > 0)
                {
                    if (grdExistingPolicies.SelectedRow != null && ddlauthPayer.Items[i].Value == grdExistingPolicies.SelectedRow.Cells[11].Text)
                    {
                        ddlauthPayer.SelectedIndex = i;
                        ddlAuthourPayerName_SelectedIndexChanged(sender, e);
                        break;
                    }
                }
            }

            btnSave.Enabled = true;
            btnSave.CssClass = "aspresizedgreenbutton";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "QuickpatientCreate", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }



        protected void chkShowAllEV_CheckedChanged(object sender, EventArgs e)
        {
            ddlPayerName.Items.Clear();

            if (chkShowAllEV.Checked == true)
            {
                chkShowAllEVPlan.Checked = true;
                chkShowAllEVPlan.Enabled = false;

                CarrierManager carrierMngr = new CarrierManager();
                IList<Carrier> carrierLst = carrierMngr.GetAll();

                if (carrierLst != null && carrierLst.Count > 0)
                {
                    ddlPayerName.Items.Add("");
                    ddlPayerName.Items.Add("OTHER");
                    for (int i = 0; i < carrierLst.Count; i++)
                    {
                        ListItem lstCarrier = new ListItem();
                        lstCarrier.Text = carrierLst[i].Carrier_Name;
                        lstCarrier.Value = carrierLst[i].Id.ToString();
                        ddlPayerName.Items.Add(lstCarrier);
                    }
                }
            }
            else
            {
                chkShowAllEVPlan.Enabled = true;
                ddlPayerName.Items.Add("");
                CarrierManager carrierMngr = new CarrierManager();
                IList<Carrier> carrierLst = carrierMngr.GetAll();

                for (int i = 0; i < grdExistingPolicies.Rows.Count; i++)
                {

                    ListItem lstCarrier = new ListItem();

                    IList<string> tempcarrierLst = new List<string>();

                    if (carrierLst != null && carrierLst.Count > 0)
                    {

                        var car = from c in carrierLst where c.Id == Convert.ToUInt64(grdExistingPolicies.Rows[i].Cells[11].Text) select c.Carrier_Name;
                        tempcarrierLst = car.ToList<string>();
                        if (tempcarrierLst != null && tempcarrierLst.Count > 0)
                        {
                            lstCarrier.Text = car.ToList<string>()[0].ToString();
                            lstCarrier.Value = grdExistingPolicies.Rows[i].Cells[11].Text;
                        }

                    }
                    //lstCarrier.Text = grdExistingPolicies.Rows[i].Cells[3].Text;
                    //lstCarrier.Value = grdExistingPolicies.Rows[i].Cells[11].Text;
                    ddlPayerName.Items.Add(lstCarrier);
                }
            }

            for (int i = 0; i < ddlPayerName.Items.Count; i++)
            {
                if (grdExistingPolicies.SelectedRow != null && grdExistingPolicies.SelectedRow.Cells[11] != null)
                {
                    if (ddlPayerName.Items[i].Value == grdExistingPolicies.SelectedRow.Cells[11].Text)
                    {
                        ddlPayerName.SelectedIndex = i;
                        ddlPayerName_SelectedIndexChanged(sender, e);
                        break;
                    }
                }
            }

            btnSave.Enabled = true;
            btnSave.CssClass = "aspresizedgreenbutton";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "QuickpatientCreate", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }


        ////added

        protected void chkShowAllAuthPlan_CheckedChanged(object sender, EventArgs e)
        {

            ddlauthinsplan.Items.Clear();


            if (chkShowAllAuthPlan.Checked == true)
            {

                ddlauthinsplan.Items.Clear();
                ddlauthinsplan.Items.Add("OTHER");


                if (ddlauthPayer.SelectedItem != null && ddlauthPayer.SelectedIndex >= 0)
                    inslist = ((InsMngr.GetInsurancebyCarrierID(Convert.ToUInt64(ddlauthPayer.Items[ddlauthPayer.SelectedIndex].Value))).OrderBy(s => s.Ins_Plan_Name)).ToList<InsurancePlan>();

                ddlauthinsplan.Items.Clear();
                ddlauthinsplan.Items.Add("");

                if (inslist != null)
                {
                    for (int i = 0; i < inslist.Count; i++)
                    {
                        ListItem ddlItem = new ListItem();
                        ddlItem.Text = inslist[i].Ins_Plan_Name;
                        ddlItem.Value = inslist[i].Id.ToString();
                        ddlauthinsplan.Items.Add(ddlItem);

                    }
                    for (int i = 0; i < ddlauthinsplan.Items.Count; i++)
                    {
                        if (grdExistingPolicies.SelectedRow != null && ddlauthinsplan.Items[i].Value == grdExistingPolicies.SelectedRow.Cells[2].Text)
                        {
                            ddlauthinsplan.SelectedIndex = i;
                        }
                    }
                }
            }
            else
            {
                ddlauthinsplan.Items.Clear();
                if (grdExistingPolicies.Rows.Count > 0 && grdExistingPolicies.SelectedRow != null)
                {

                    ListItem ddlItem = new ListItem();
                    ddlItem.Text = grdExistingPolicies.SelectedRow.Cells[3].Text;
                    ddlItem.Value = grdExistingPolicies.SelectedRow.Cells[2].Text;
                    ddlauthinsplan.Items.Add(ddlItem);
                }
            }
            btnSave.Enabled = true;
            btnSave.CssClass = "aspresizedgreenbutton";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void chkShowAllEVPlan_CheckedChanged(object sender, EventArgs e)
        {
            ddlPlanName.Items.Clear();
            if (chkShowAllEVPlan.Checked == true)
            {

                ddlPlanName.Items.Clear();
                ddlPlanName.Items.Add("OTHER");


                inslist = ((InsMngr.GetInsurancebyCarrierID(Convert.ToUInt64(ddlPayerName.Items[ddlPayerName.SelectedIndex].Value))).OrderBy(s => s.Ins_Plan_Name)).ToList<InsurancePlan>();

                ddlPlanName.Items.Clear();
                ddlPlanName.Items.Add("");

                if (inslist != null)
                {
                    for (int i = 0; i < inslist.Count; i++)
                    {
                        ListItem ddlItem = new ListItem();
                        ddlItem.Text = inslist[i].Ins_Plan_Name;
                        ddlItem.Value = inslist[i].Id.ToString();
                        ddlPlanName.Items.Add(ddlItem);

                    }
                    for (int i = 0; i < ddlPlanName.Items.Count; i++)
                    {
                        if (grdExistingPolicies.SelectedRow != null && ddlPlanName.Items[i].Value == grdExistingPolicies.SelectedRow.Cells[2].Text)
                        {
                            ddlPlanName.SelectedIndex = i;
                        }
                    }
                }
            }

            else
            {
                ddlPlanName.Items.Clear();
                if (grdExistingPolicies.SelectedRow != null)
                {

                    ListItem ddlItem = new ListItem();
                    ddlItem.Text = grdExistingPolicies.SelectedRow.Cells[3].Text;
                    ddlItem.Value = grdExistingPolicies.SelectedRow.Cells[2].Text;
                    ddlPlanName.Items.Add(ddlItem);
                }
            }

            btnSave.Enabled = true;
            btnSave.CssClass = "aspresizedgreenbutton";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }


        private void FillVisitPaymentHistory(VisitPayment EditVisitPayment, VisitPaymentArc EditVisitPaymentArc, string sCreditType)
        {
            VisitPaymentHistory objVisitPaymentHistory = new VisitPaymentHistory();
            VisitPaymentHistoryArc objVisitPaymentHistoryArc = new VisitPaymentHistoryArc();



            if (btnAdd.Text == "Add")
            {
                SaveVisitPaymenHistoryList = new List<VisitPaymentHistory>();
                SaveVisitPaymenHistoryArcList = new List<VisitPaymentHistoryArc>();

                objVisitPaymentHistory.Visit_Payment_ID = EditVisitPayment.Id;
                objVisitPaymentHistory.Amount_Paid_By = EditVisitPayment.Amount_Paid_By;
                objVisitPaymentHistory.Auth_No = EditVisitPayment.Auth_No;
                objVisitPaymentHistory.Check_Card_No = EditVisitPayment.Check_Card_No;
                objVisitPaymentHistory.Check_Date = EditVisitPayment.Check_Date;
                objVisitPaymentHistory.Created_By = EditVisitPayment.Created_By;
                objVisitPaymentHistory.Created_Date_And_Time = EditVisitPayment.Created_Date_And_Time;
                objVisitPaymentHistory.Encounter_ID = EditVisitPayment.Encounter_ID;
                objVisitPaymentHistory.Human_ID = EditVisitPayment.Human_ID;
                objVisitPaymentHistory.Is_Delete = EditVisitPayment.Is_Delete;
                objVisitPaymentHistory.Method_of_Payment = EditVisitPayment.Method_of_Payment;
                objVisitPaymentHistory.Modified_By = EditVisitPayment.Modified_By;
                objVisitPaymentHistory.Modified_Date_And_Time = EditVisitPayment.Modified_Date_And_Time;
                objVisitPaymentHistory.Patient_Payment = (EditVisitPayment.Patient_Payment);
                objVisitPaymentHistory.Payment_Message_ID = EditVisitPayment.Payment_Message_ID;
                objVisitPaymentHistory.Payment_Note = EditVisitPayment.Payment_Note;
                objVisitPaymentHistory.Rec_On_Acc = EditVisitPayment.Rec_On_Acc;
                objVisitPaymentHistory.Refund_Amount = EditVisitPayment.Refund_Amount;
                objVisitPaymentHistory.Relationship = EditVisitPayment.Relationship;
                objVisitPaymentHistory.Version = EditVisitPayment.Version;
                objVisitPaymentHistory.Voucher_No = EditVisitPayment.Voucher_No;
                objVisitPaymentHistory.Facility_Name = EditVisitPayment.Facility_Name;

                SaveVisitPaymenHistoryList.Add(objVisitPaymentHistory);


            }
            else
            {

                if (sCreditType == "DEBIT")
                {
                    SaveVisitPaymenHistoryList = new List<VisitPaymentHistory>();
                    SaveVisitPaymenHistoryArcList = new List<VisitPaymentHistoryArc>();

                    objVisitPaymentHistory.Visit_Payment_ID = EditVisitPayment.Id;
                    objVisitPaymentHistory.Amount_Paid_By = EditVisitPayment.Amount_Paid_By;
                    objVisitPaymentHistory.Auth_No = EditVisitPayment.Auth_No;
                    objVisitPaymentHistory.Check_Card_No = EditVisitPayment.Check_Card_No;
                    objVisitPaymentHistory.Check_Date = EditVisitPayment.Check_Date;
                    objVisitPaymentHistory.Created_By = ClientSession.UserName; // EditVisitPayment.Created_By;
                    objVisitPaymentHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal(); // EditVisitPayment.Created_Date_And_Time;
                    objVisitPaymentHistory.Encounter_ID = EditVisitPayment.Encounter_ID;
                    objVisitPaymentHistory.Human_ID = EditVisitPayment.Human_ID;
                    objVisitPaymentHistory.Is_Delete = EditVisitPayment.Is_Delete;
                    objVisitPaymentHistory.Method_of_Payment = EditVisitPayment.Method_of_Payment;
                    objVisitPaymentHistory.Modified_By = string.Empty; // EditVisitPayment.Modified_By;
                    objVisitPaymentHistory.Modified_Date_And_Time = DateTime.MinValue; // EditVisitPayment.Modified_Date_And_Time;
                    objVisitPaymentHistory.Patient_Payment = -(EditVisitPayment.Patient_Payment);
                    objVisitPaymentHistory.Payment_Message_ID = EditVisitPayment.Payment_Message_ID;
                    objVisitPaymentHistory.Payment_Note = string.Empty; // EditVisitPayment.Payment_Note;
                    objVisitPaymentHistory.Rec_On_Acc = -(EditVisitPayment.Rec_On_Acc);
                    objVisitPaymentHistory.Refund_Amount = -(EditVisitPayment.Refund_Amount);
                    objVisitPaymentHistory.Relationship = EditVisitPayment.Relationship;
                    objVisitPaymentHistory.Version = EditVisitPayment.Version;
                    objVisitPaymentHistory.Voucher_No = EditVisitPayment.Voucher_No;
                    objVisitPaymentHistory.Facility_Name = EditVisitPayment.Facility_Name;

                    SaveVisitPaymenHistoryList.Add(objVisitPaymentHistory);
                }
                else if (sCreditType == "CREDIT")
                {

                    objVisitPaymentHistory = new VisitPaymentHistory();

                    objVisitPaymentHistory.Visit_Payment_ID = EditVisitPayment.Id;
                    objVisitPaymentHistory.Amount_Paid_By = EditVisitPayment.Amount_Paid_By;
                    objVisitPaymentHistory.Auth_No = EditVisitPayment.Auth_No;
                    objVisitPaymentHistory.Check_Card_No = EditVisitPayment.Check_Card_No;
                    objVisitPaymentHistory.Check_Date = EditVisitPayment.Check_Date;
                    objVisitPaymentHistory.Created_By = EditVisitPayment.Created_By;
                    objVisitPaymentHistory.Created_Date_And_Time = EditVisitPayment.Created_Date_And_Time;
                    objVisitPaymentHistory.Encounter_ID = EditVisitPayment.Encounter_ID;
                    objVisitPaymentHistory.Human_ID = EditVisitPayment.Human_ID;
                    objVisitPaymentHistory.Is_Delete = EditVisitPayment.Is_Delete;
                    objVisitPaymentHistory.Method_of_Payment = EditVisitPayment.Method_of_Payment;
                    objVisitPaymentHistory.Modified_By = EditVisitPayment.Modified_By;
                    objVisitPaymentHistory.Modified_Date_And_Time = EditVisitPayment.Modified_Date_And_Time;
                    objVisitPaymentHistory.Patient_Payment = EditVisitPayment.Patient_Payment;
                    objVisitPaymentHistory.Payment_Message_ID = EditVisitPayment.Payment_Message_ID;
                    objVisitPaymentHistory.Payment_Note = EditVisitPayment.Payment_Note;
                    objVisitPaymentHistory.Rec_On_Acc = EditVisitPayment.Rec_On_Acc;
                    objVisitPaymentHistory.Refund_Amount = EditVisitPayment.Refund_Amount;
                    objVisitPaymentHistory.Relationship = EditVisitPayment.Relationship;
                    objVisitPaymentHistory.Version = EditVisitPayment.Version;
                    objVisitPaymentHistory.Voucher_No = EditVisitPayment.Voucher_No;
                    objVisitPaymentHistory.Facility_Name = EditVisitPayment.Facility_Name;

                    SaveVisitPaymenHistoryList.Add(objVisitPaymentHistory);
                }

            }



        }

        public void UpdateAgeinBlob(ulong ulHumanID)
        {
            XmlDocument itemDoc = new XmlDocument();
            string sXMLContent = String.Empty;
            HumanBlobManager HumanBlobMngr = new HumanBlobManager();
            Human_Blob objHumanblob = null;
            IList<Human_Blob> ilstHumanBlob = HumanBlobMngr.GetHumanBlob(ulHumanID);
            if (ilstHumanBlob.Count > 0)
            {
                objHumanblob = ilstHumanBlob[0];
                sXMLContent = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                if (sXMLContent.Substring(0, 1) != "<")
                    sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                itemDoc.LoadXml(sXMLContent);
            }


            // XmlText = new XmlTextReader(strXmlHumanFilePath);
            //itemDoc.Load(XmlText);

            int iAge = 0;
            iAge = UtilityManager.CalculateAge(ListToUpdateHuman[0].Birth_Date);

            XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
            if (xmlAge != null && xmlAge.Count > 0)
                xmlAge[0].Attributes[0].Value = iAge.ToString();
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
            }

            //XmlText.Close();
            // itemDoc.Save(strXmlHumanFilePath);
            //int trycount = 0;
            //trytosaveagain:
            try
            {
                //itemDoc.Save(strXmlHumanFilePath);
                IList<Human_Blob> ilstUpdateBlob = new List<Human_Blob>();
                byte[] bytes = null;
                try
                {
                    bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);
                }
                catch (Exception ex)
                {

                }
                objHumanblob.Human_XML = bytes;
                ilstUpdateBlob.Add(objHumanblob);
                HumanBlobMngr.SaveHumanBlobWithTransaction(ilstUpdateBlob, string.Empty);
            }
            catch (Exception xmlexcep)
            {
                throw new Exception(xmlexcep.Message.ToString());

                //trycount++;
                //if (trycount <= 3)
                //{
                //    int TimeMilliseconds = 0;
                //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                //    Thread.Sleep(TimeMilliseconds);
                //    string sMsg = string.Empty;
                //    string sExStackTrace = string.Empty;

                //    string version = "";
                //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                //    string[] server = version.Split('|');
                //    string serverno = "";
                //    if (server.Length > 1)
                //        serverno = server[1].Trim();

                //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                //        sMsg = xmlexcep.InnerException.Message;
                //    else
                //        sMsg = xmlexcep.Message;

                //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                //        sExStackTrace = xmlexcep.StackTrace;

                //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                //    string ConnectionData;
                //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                //    {
                //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                //        {
                //            cmd.Connection = con;
                //            try
                //            {
                //                con.Open();
                //                cmd.ExecuteNonQuery();
                //                con.Close();
                //            }
                //            catch
                //            {
                //            }
                //        }
                //    }
                //    goto trytosaveagain;
                //}
            }
        }
    }
}