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
using Acurus.Capella.Core.DTO;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Text.RegularExpressions;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Drawing;
using Acurus.Capella.UI;
using Acurus.Capella.UI.UserControls;
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Web.Services;

namespace Acurus.Capella.UI
{
    public partial class frmPatientDemographics : System.Web.UI.Page
    {
        IList<StaticLookup> iStaticlookuplist;
        IList<State> iStatelist;
        Human objHuman = new Human();
        Human objHumanList = new Human();
        HumanDTO objHumanDTO = new HumanDTO();
        ulong objHumanId, ulPatientID = 0;
        string sReplace, sExternalAccno;
        bool bInsuredHuman = true, bFormCloseCheck, bSaveCheck = false, bEditNameValid = false;
        public static string sPatlastname, sPatfirstname, sSSN;
        public static DateTime dtPatDob;
        HumanManager HumanMngr = new HumanManager();
        StateManager StateMngr = new StateManager();
        IList<StaticLookup> Relationlist = null;
        CarrierManager CarrierMngr = new CarrierManager();
        StaticLookupManager staticLookUpMngr = new StaticLookupManager();
        InsurancePlanManager InsMngr = new InsurancePlanManager();
        FacilityManager FacilityMngr = new FacilityManager();
        PhysicianManager phyMngr = new PhysicianManager();
        PatientInsuredPlanManager PatInsManager = new PatientInsuredPlanManager();
        PatGuarantorManager patGuarantorMngr = new PatGuarantorManager();
        PatientNotesManager PatientNotesMngr = new PatientNotesManager();
        IList<StaticLookup> ilstStatiLookUpList = new List<StaticLookup>();
        StaticLookupManager objStaticLookUpMngr = new StaticLookupManager();
        FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
        PatientNotesManager objPatientNotesMngr = new PatientNotesManager();


        void Page_PreInit(Object sender, EventArgs e)
        {
            if (Request.QueryString["EnableMaster"] != null)
            {
                if (Request.QueryString["EnableMaster"].ToString().ToUpper().Trim() == "TRUE")
                {
                    this.MasterPageFile = "~/C5PO.Master";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Startloadcursor", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);

            txtNotes.txtDLC.Attributes.Add("onkeypress", "change(this);");
            txtNotes.txtDLC.Attributes.Add("onchange", "change(this);");

            //srividhya            
            btnAddMessage.Attributes.Add("onclick", "javascript:return AddMessageDemo();");
            //Added by Chithra
            //txtPCPProvider.Attributes.Add("readonly", "readonly");
            //txtNPI.Attributes.Add("readonly", "readonly");
            TxtSexualOrientationSpecify.Text = hdnSexualOrientationSpecify.Value;
            TxtGenderIdentity.Text = hdnGenderIdentity.Value;
            if (hdnBirthOrder.Value != "")
            {
                for (int i = 0; i < ddlBirthOrder.Items.Count; i++)
                {
                    if (Convert.ToString(ddlBirthOrder.Items[i].Text).ToUpper() == hdnBirthOrder.Value.ToUpper())
                    {
                        ddlBirthOrder.SelectedIndex = i;
                        break;
                    }
                }
            }

            //CAP-70 Set selected value to the patient sax.
            if (HiddenPatientSex.Value != "")
            {
                for (int i = 0; i < ddlPatientsex.Items.Count; i++)
                {
                    if (Convert.ToString(ddlPatientsex.Items[i].Text).ToUpper() == HiddenPatientSex.Value.ToUpper())
                    {
                        ddlPatientsex.SelectedIndex = i;
                        break;
                    }
                }
            }

            this.Page.Title = "Patient Demographics" + "-" + ClientSession.UserName;

            if (Request.QueryString["EnableMaster"] != null)
            {
                if (Request.QueryString["EnableMaster"].ToString().ToUpper().Trim() == "TRUE")
                {
                    pnlAccountInfo.BackColor = Color.White;
                    pnlEmergencyInfo.BackColor = Color.White;
                    pnlGuarantorInfo.BackColor = Color.White;
                    pnlPatientInfo.BackColor = Color.White;
                    pnlMessageInfo.BackColor = Color.White;
                }
            }

            if (!IsPostBack)
            {
                txtRace.Attributes.Add("ReadOnly", "true");
                txtGranularity.Attributes.Add("ReadOnly", "true");
                //Cap - 1394
                //dtpDateOfDeath.Attributes.Add("Readonly", "true");
                //Cap - 1529
                if (ddlPatientStatus.Text.ToUpper() == "ALIVE")
                {
                    dtpDateOfDeath.Attributes.Add("readOnly", "true");
                    dtpDateOfDeath.Text = string.Empty;
                }
                else if (ddlPatientStatus.Text.ToUpper() == "DECEASED")
                {
                    dtpDateOfDeath.Attributes.Remove("readonly");
                    dtpDateOfDeath.CssClass = "Editabletxtbox";
                }
                   
                if (chkEnrollOnlineAccess.Checked == false)
                    btnSendEmail.Enabled = false;
                IList<string> HumanIDList = new List<string>();
                hdnHumanId.Value = "0";
                hdnEncounterID.Value = "0";
                hdnHumanIDList.Value = "";
                hdnRaceTag.Value = "";
                hdnAccNoFromViewReport.Value = "";
                GetPhysicianId();
                if (Request.QueryString["EncounterId"] != null)
                    hdnEncounterID.Value = Request.QueryString["EncounterId"].ToString();
                else
                    hdnEncounterID.Value = "0";
                System.Diagnostics.Stopwatch LoadTime = new System.Diagnostics.Stopwatch();
                LoadTime.Start();
                ddlAssignedTo.Items.Clear();
                hdnFacilityName.Value = ClientSession.FacilityName;
                IList<string> patientlst = new List<string>();
                PatientNotesManager objPatNotesMngr = new PatientNotesManager();
                patientlst = objPatNotesMngr.MapPhysicianUserListForFacility(ClientSession.FacilityName, ClientSession.LegalOrg);
                if (patientlst != null && patientlst.Count > 0)
                {
                    ddlAssignedTo.Items.Add("");
                    foreach (var i in patientlst)
                    {
                        //CAP-1091
                        //ddlAssignedTo.Items.Add(i.ToString().Split('|')[1]);
                        ListItem li = new ListItem();
                        li.Text = i.ToString().Split('|')[1];
                        li.Value = i.ToString().Split('|')[0];
                        ddlAssignedTo.Items.Add(li);
                    }
                }
                if (this.PreviousPage != null)
                {
                    if (this.PreviousPage.Form.Name == "frmFindPatient")
                    {
                        if (((frmFindPatient)this.PreviousPage).hdnHumanID.Value.ToString() != string.Empty && System.Text.RegularExpressions.Regex.IsMatch(((frmFindPatient)this.PreviousPage).hdnHumanID.Value, "^[0-9]*$") == true)
                        {
                            ulPatientID = Convert.ToUInt64(((frmFindPatient)this.PreviousPage).hdnHumanID.Value);
                            hdnPatientID.Value = ulPatientID.ToString();
                        }
                    }
                }
                else
                {
                    ulPatientID = 0;
                    hdnPatientID.Value = ulPatientID.ToString();
                }
                if (Request["bInsurance"] != null && Request["HumanId"] != null && Request["HumanId"].ToString() != "undefined" && Request["HumanId"].ToString() != "" && System.Text.RegularExpressions.Regex.IsMatch(Request["HumanId"].ToString(), "^[0-9]*$") == true)
                {
                    bInsuredHuman = false;
                    hdnbInsuredHuman.Value = "False";
                    ulPatientID = Convert.ToUInt64(Request["HumanId"].ToString());
                    hdnPatientID.Value = Request["HumanId"].ToString();
                }
                if (Request["HumanId"] != null && Request["HumanId"].ToString() != "undefined" && Request["HumanId"].ToString() != "" && System.Text.RegularExpressions.Regex.IsMatch(Request["HumanId"].ToString(), "^[0-9]*$") == true)
                {
                    ulPatientID = Convert.ToUInt64(Request["HumanId"].ToString());
                    hdnPatientID.Value = Request["HumanId"].ToString();
                }
                if (Request["FromAddGuarantor"] != null)
                    hdnFromAddPatient.Value = Request["FromAddGuarantor"].ToString();
                btnAddGuarantor.Enabled = false;
                btnSelectGaurantor.Enabled = false;
                dtpEmerDOB.Text = string.Empty;
                dtpGuarantorDOB.Text = string.Empty;
                dtpPatientDOB.Text = string.Empty;
                if (bInsuredHuman == false)
                    DisableGroupbox();
                txtMedicalRecordno.Focus();
                if (chkGuarantorIsPatient.Enabled != false)
                {

                }
                if (ClientSession.LocalDate != null && ClientSession.LocalDate.ToString() != "")
                    dtpAccCreationDate.Text = Convert.ToDateTime(ClientSession.LocalDate).ToString("dd-MMM-yyyy");
                LoadComboboxValues();
                if (ulPatientID != 0)
                {
                    LoadHumanDetails(ulPatientID, string.Empty, string.Empty);
                    DisablePatientDetails();
                    if (chkGuarantorIsPatient.Enabled == true)
                    {
                        if (chkGuarantorIsPatient.Checked == true)
                        {
                            dtpGuarantorDOB.Enabled = false;
                            DisableTableLayout(pnlGuarantorInfo);
                            txtGuarantorFirstName.Text = txtPatientfirstname.Text;
                            txtGuarantorLastName.Text = txtPatientlastname.Text;
                            txtGuarantorMiddleName.Text = txtPatientmiddlename.Text;
                            txtGuarantorAddress.Text = txtPatientAddress.Text;
                            txtGuarantorAddressLine2.Text = txtPatientAddressLine2.Text;
                            txtGuarantorCity.Text = txtCity.Text;
                            msktxtGuarantorZipCode.Text = msktxtZipcode.Text;
                            //CAP-1975
                            txtGuaEmail.Text = txtEmail.Text;
                            for (int k = 0; k < ddlGuarantorSex.Items.Count; k++)
                            {
                                if (ddlGuarantorSex.Items[k].Text == ddlPatientsex.SelectedItem.Text)
                                {
                                    ddlGuarantorSex.SelectedIndex = k;
                                    hdnGuarantorSex.Value = ddlPatientsex.SelectedItem.Text;
                                    break;
                                }
                            }
                            for (int l = 0; l < ddlGuarantorState.Items.Count; l++)
                            {
                                if (ddlGuarantorState.Items[l].Text == ddlState.SelectedItem.Text)
                                {
                                    ddlGuarantorState.SelectedIndex = l;
                                    hdnGuarantorState.Value = ddlState.SelectedItem.Text;
                                    break;
                                }
                            }
                            for (int i = 0; i < ddlGuarantorRelationship.Items.Count; i++)
                            {
                                if (Convert.ToString(ddlGuarantorRelationship.Items[i].Text).ToUpper() == "SELF")
                                {
                                    ddlGuarantorRelationship.SelectedIndex = i;
                                    break;
                                }
                            }
                            if (dtpPatientDOB.Text != "")
                            {
                                dtpGuarantorDOB.Text = Convert.ToDateTime(dtpPatientDOB.Text).ToString("dd-MMM-yyyy");
                            }
                            msktxtGuarantorCellNo.Text = msktxtCellPhno.Text;
                            msktxtGuarantorHomeNo.Text = msktxtHomePhno.Text;
                            ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                        }
                        if (chkGuarantorIsPatient.Checked == false)
                        {
                            btnAddGuarantor.Enabled = true;
                            btnSelectGaurantor.Enabled = true;
                            btnViewGaurantor.Enabled = true;
                            DisableTableLayout(pnlGuarantorInfo);
                            ddlGuarantorRelationship.Enabled = true;
                            ddlGuarantorRelationship.BackColor = Color.White;
                            ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                        }
                    }
                    btnEditName.Enabled = true;
                    IList<PatientNotes> GetIsPopUp = objPatientNotesMngr.GetIsPopUp(ulPatientID.ToString());
                    if (GetIsPopUp != null && GetIsPopUp.Count > 0)
                        hdnIsPopUp.Value = "Y";
                    else
                        hdnIsPopUp.Value = "N";
                    ClientScript.RegisterStartupScript(typeof(Page), "PatientDemographics", "OpenDemographics();", true);
                }
                else
                {
                    if (Request["sPatlastname"] != null)
                        txtPatientlastname.Text = Request["sPatlastname"];
                    if (Request["sFirstName"] != null)
                        txtPatientfirstname.Text = Request["sFirstName"];
                    if (Request["sExtAccNo"] != null)
                        txtExternalAccNo.Text = Request["sExtAccNo"];
                    if (Request["DOB"] != null)
                    {
                        if (Request["DOB"].Trim() != "__-___-____")
                            dtpPatientDOB.Text = Convert.ToDateTime(Request["DOB"]).ToString("dd-MMM-yyyy");
                    }
                    btnSendEmail.Enabled = false;
                    if (dtPatDob != DateTime.MinValue)
                        dtpPatientDOB.Text = dtPatDob.ToString("dd-MMM-yyyy");
                    msktxtSSN.Text = sSSN;
                    txtNoofPolicies.Text = "0";
                    ddlPatientSignature.Text = "No";
                    //btnViewUpdateInsurance.Enabled = false;
                    if (bInsuredHuman == true)
                        chkGuarantorIsPatient.Checked = true;
                    if (chkGuarantorIsPatient.Enabled == true)
                    {
                        if (chkGuarantorIsPatient.Checked == true)
                        {
                            dtpGuarantorDOB.Enabled = false;
                            DisableTableLayout(pnlGuarantorInfo);
                            txtGuarantorFirstName.Text = txtPatientfirstname.Text;
                            txtGuarantorLastName.Text = txtPatientlastname.Text;
                            txtGuarantorMiddleName.Text = txtPatientmiddlename.Text;
                            txtGuarantorAddress.Text = txtPatientAddress.Text;
                            txtGuarantorAddressLine2.Text = txtPatientAddressLine2.Text;
                            txtGuarantorCity.Text = txtCity.Text;
                            msktxtGuarantorZipCode.Text = msktxtZipcode.Text;
                            for (int k = 0; k < ddlGuarantorSex.Items.Count; k++)
                            {
                                if (ddlGuarantorSex.Items[k].Text == ddlPatientsex.SelectedItem.Text)
                                {
                                    ddlGuarantorSex.SelectedIndex = k;
                                    hdnGuarantorSex.Value = ddlPatientsex.SelectedItem.Text;
                                    break;
                                }
                            }
                            for (int l = 0; l < ddlGuarantorState.Items.Count; l++)
                            {
                                if (ddlGuarantorState.Items[l].Text == ddlState.SelectedItem.Text)
                                {
                                    ddlGuarantorState.SelectedIndex = l;
                                    hdnGuarantorState.Value = ddlState.SelectedItem.Text;
                                    break;
                                }
                            }

                            for (int i = 0; i < ddlGuarantorRelationship.Items.Count; i++)
                            {
                                if (Convert.ToString(ddlGuarantorRelationship.Items[i].Text).ToUpper() == "SELF")
                                {
                                    ddlGuarantorRelationship.SelectedIndex = i;
                                    break;
                                }
                            }
                            if (dtpPatientDOB.Text != "")
                                dtpGuarantorDOB.Text = dtpPatientDOB.Text;
                            msktxtGuarantorCellNo.Text = msktxtCellPhno.Text;
                            msktxtGuarantorHomeNo.Text = msktxtHomePhno.Text;
                            ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                        }
                        if (chkGuarantorIsPatient.Checked == false)
                        {
                            btnAddGuarantor.Enabled = true;
                            btnSelectGaurantor.Enabled = true;
                            btnViewGaurantor.Enabled = true;
                            DisableTableLayout(pnlGuarantorInfo);
                            ddlGuarantorRelationship.Enabled = true;
                            ddlGuarantorRelationship.BackColor = Color.White;
                            ClearGuarantorInfo();
                            ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                        }
                    }
                    ddlPatientStatus.SelectedIndex = 0;
                    btnEditName.Enabled = false;
                }
                bFormCloseCheck = false;
                LoadTime.Stop();
                btnSave.Enabled = false;
                txtAccountNo.Attributes.Add("readonly", "readonly");

                if (ddlPatientRelation.SelectedItem.Text.ToUpper() == "SELF")
                {
                    //Human_Token lstPatientResult = new Human_Token();
                    //Human_TokenManager objPatientResult = new Human_TokenManager();
                    //lstPatientResult = objPatientResult.GetHumanTokenbyhumanid(ulPatientID);
                    string Dob = "";
                    if (dtpPatientDOB.Text != "")
                    {
                        Dob = Convert.ToDateTime(dtpPatientDOB.Text).ToString("dd-MMM-yyyy");
                    }

                    string lstPatientResult = txtPatientlastname.Text + ", " + txtPatientfirstname.Text + " |" + "DOB: " + Dob + "|" + ddlPatientsex.Text + " | ACC#: " + ulPatientID.ToString() + " | PATIENT TYPE: REGULAR";
                    if (lstPatientResult != null)
                    {
                        HiddenPatientName.Value = "" + "&" + ulPatientID.ToString(); //lstPatientResult + "&" + ulPatientID.ToString();
                        // txtSelectinsured.Text = lstPatientResult;
                        txtSelectinsured.Text = "";
                        txtSelectinsured.Enabled = false;
                        txtSelectinsured.CssClass = "nonEditabletxtbox";
                        txtSelectinsured.Attributes.Add("data-human-id", ulPatientID.ToString());
                        imgClearplanText.Disabled = true;
                        btnaddins.Disabled = true;

                    }
                }

                ShownDemographics();
                if (Request["ScreenName"] != null)
                {
                    if (Request["ScreenName"] == "Demographics")
                    {
                        btnAddGuarantor.Enabled = false;
                        btnSelectGaurantor.Enabled = false;
                    }
                }
                if (Request["DisableFindPat"] != null)
                {
                    if (Request["DisableFindPat"] == "TRUE")
                    {
                        btnAddGuarantor.Enabled = false;
                        btnSelectGaurantor.Enabled = false;
                    }
                }
                btnAddMessage.Enabled = false;
            }



            //Capella - 1361
            if (chkGuarantorIsPatient.Enabled == true)
            {
                if (chkGuarantorIsPatient.Checked == true)
                {
                    dtpGuarantorDOB.Enabled = false;
                    DisableTableLayout(pnlGuarantorInfo);
                    txtGuarantorFirstName.Text = txtPatientfirstname.Text;
                    txtGuarantorLastName.Text = txtPatientlastname.Text;
                    txtGuarantorMiddleName.Text = txtPatientmiddlename.Text;
                    txtGuarantorAddress.Text = txtPatientAddress.Text;
                    txtGuarantorAddressLine2.Text = txtPatientAddressLine2.Text;
                    txtGuarantorCity.Text = txtCity.Text;
                    msktxtGuarantorZipCode.Text = msktxtZipcode.Text;
                    for (int k = 0; k < ddlGuarantorSex.Items.Count; k++)
                    {
                        if (ddlGuarantorSex.Items[k].Text == ddlPatientsex.SelectedItem.Text)
                        {
                            ddlGuarantorSex.SelectedIndex = k;
                            hdnGuarantorSex.Value = ddlPatientsex.SelectedItem.Text;
                            break;
                        }
                    }
                    for (int l = 0; l < ddlGuarantorState.Items.Count; l++)
                    {
                        if (ddlGuarantorState.Items[l].Text == ddlState.SelectedItem.Text)
                        {
                            ddlGuarantorState.SelectedIndex = l;
                            hdnGuarantorState.Value = ddlState.SelectedItem.Text;
                            break;
                        }
                    }

                    for (int i = 0; i < ddlGuarantorRelationship.Items.Count; i++)
                    {
                        if (Convert.ToString(ddlGuarantorRelationship.Items[i].Text).ToUpper() == "SELF")
                        {
                            ddlGuarantorRelationship.SelectedIndex = i;
                            break;
                        }
                    }
                    if (dtpPatientDOB.Text != "")
                        dtpGuarantorDOB.Text = dtpPatientDOB.Text;
                    msktxtGuarantorCellNo.Text = msktxtCellPhno.Text;
                    msktxtGuarantorHomeNo.Text = msktxtHomePhno.Text;
                    ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                }
                if (chkGuarantorIsPatient.Checked == false)
                {
                    btnAddGuarantor.Enabled = true;
                    btnSelectGaurantor.Enabled = true;
                    btnViewGaurantor.Enabled = true;
                    DisableTableLayout(pnlGuarantorInfo);
                    ddlGuarantorRelationship.Enabled = true;
                    ddlGuarantorRelationship.BackColor = Color.White;
                    //ClearGuarantorInfo();
                    ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                }
            }






            if (chkGuarantorIsPatient.Enabled && chkGuarantorIsPatient.Checked)
            {
                if (dtpPatientDOB.Text != string.Empty)
                    dtpGuarantorDOB.Text = Convert.ToDateTime(dtpPatientDOB.Text).ToString("dd-MMM-yyyy");
            }

            btnSave.Attributes.Add("Onclick", "javascript:return PatientInformationValidation();");
            btnSendEmail.Attributes.Add("Onclick", "javascript:return PatientInformationValidation();");
            //Added by Srividhya on 21-Feb-2013
            Session["WFObjIDNotStarted"] = "0";
            Session["CompAmt"] = "0";
            //Added by priyangha 
            if (Request["HumanId"] != null && Request["HumanId"].ToString() != "undefined" && System.Text.RegularExpressions.Regex.IsMatch(Request["HumanId"], "^[0-9]*$") == true)
            {
                ClientSession.HumanId = Convert.ToUInt64(Request["HumanId"]);
            }
            else if (hdnPatientID.Value != "" && hdnPatientID.Value != "0" && System.Text.RegularExpressions.Regex.IsMatch(hdnPatientID.Value, "^[0-9]*$") == true)
            {
                ClientSession.HumanId = Convert.ToUInt64(hdnPatientID.Value);
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
            if (txtRace.Enabled == true)
            {
                fileupload.Enabled = true;
            }
            else
            {
                fileupload.Enabled = false;
            }
            //CAP-2540
            if (txtAccountNo.Text == "")
            {
                txtNotes.txtDLC.Enabled = false;
                txtNotes.txtDLC.CssClass = "nonEditabletxtbox";
                ComboBoxColorChange(ddlMessageDescription);
                ComboBoxColorChange(ddlAssignedTo);
                chkshowall.Checked = false;
                chkshowall.Enabled = false;

            }

            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Stoploadcursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }


        [WebMethod(EnableSession = true)]
        public static string loadGrid(string uPatientId)
        {
            ulong insPlanId;
            Human InsuredHumanList = new Human();
            HumanManager HumanMngr = new HumanManager();
            Human objHumanList = new Human();
            CarrierManager CarrierMngr = new CarrierManager();
            InsurancePlanManager InsMngr = new InsurancePlanManager();
            DataTable dt = new DataTable();
            //clear the grid rows.
            //  grdPolicyInformation.DataSource = null;
            //grdPolicyInformation.DataBind();
            // objHumanList = EncounterManager.Instance.GetHumanByHumanID( Convert.ToUInt64(Session["ulMyHumanID"]));
            IList<Human> humanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(uPatientId));
            if (humanList != null && humanList.Count > 0) //code added by balaji.TJ 2015-12-17         
                objHumanList = humanList[0];
            if (objHumanList != null)
            {
                if (objHumanList.PatientInsuredBag != null && objHumanList.PatientInsuredBag.Count > 0)
                {
                    //IList<PatientInsuredPlan> PatInsOrderedList = objHumanList.PatientInsuredBag.OrderBy(x => x.Sort_Order).ToList<PatientInsuredPlan>();
                    IList<PatientInsuredPlan> PatInsOrderedList = objHumanList.PatientInsuredBag.OrderBy(x => x.Insurance_Type).ToList<PatientInsuredPlan>();


                    dt.Columns.Add("Policy_Holder_ID", typeof(string));
                    dt.Columns.Add("Plan_ID", typeof(string));
                    dt.Columns.Add("Plan_Name", typeof(string));
                    dt.Columns.Add("Group_Number", typeof(string));
                    dt.Columns.Add("Insurance_Type", typeof(string));
                    dt.Columns.Add("Patient_Name", typeof(string));
                    dt.Columns.Add("Relationship", typeof(string));
                    dt.Columns.Add("Relationship_Number", typeof(int));
                    dt.Columns.Add("Active", typeof(string));
                    dt.Columns.Add("Effective_Start_Date", typeof(string));
                    dt.Columns.Add("Termination_Date", typeof(string));
                    dt.Columns.Add("Insured_Name", typeof(string));
                    dt.Columns.Add("Insured_Human_ID", typeof(string));
                    dt.Columns.Add("Insured_DOB", typeof(string));
                    dt.Columns.Add("Insured_Sex", typeof(string));
                    dt.Columns.Add("Id", typeof(string));
                    dt.Columns.Add("Carrier_Name", typeof(string));
                    dt.Columns.Add("CarrierID", typeof(string));
                    dt.Columns.Add("PlanType", typeof(string));
                    dt.Columns.Add("Sortorder", typeof(int));
                    dt.Columns.Add("Specify_Other", typeof(string));
                    dt.Columns.Add("Insured_Details", typeof(string));
                    dt.Columns.Add("PCP_Name", typeof(string));
                    dt.Columns.Add("PCP_ID", typeof(string));
                    dt.Columns.Add("PCP_Grid_Name", typeof(string));
                    dt.Columns.Add("PCP_Textbox_Name", typeof(string));
                    dt.Columns.Add("PCP_NPI", typeof(string));
                    dt.Columns.Add("Category", typeof(string)); 
                    IList<ulong> vPhyId = PatInsOrderedList.Select(aa => aa.PCP_ID).Distinct().ToArray();
                    FindPhysican physician_dto = new FindPhysican();
                    PhysicianManager objPhysicianManager = new PhysicianManager();
                    physician_dto = objPhysicianManager.FindPhysicianID(vPhyId);

                    foreach (PatientInsuredPlan obj in PatInsOrderedList)//objHumanList.PatientInsuredBag)
                    {
                        IList<Human> humanInsList = HumanMngr.GetPatientDetailsUsingPatientInformattion(obj.Insured_Human_ID);
                        if (humanInsList != null && humanInsList.Count > 0)
                            InsuredHumanList = humanInsList[0];
                        ulong InsID = 0;
                        InsurancePlan objInsurancePlan = new InsurancePlan();
                        if (dt != null)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Sortorder"] = Convert.ToInt32(obj.Sort_Order);
                            dr["Policy_Holder_ID"] = obj.Policy_Holder_ID;
                            dr["Plan_ID"] = obj.Insurance_Plan_ID;
                            insPlanId = Convert.ToUInt64(dr[1]);
                            IList<InsurancePlan> insList = InsMngr.GetInsurancebyID(insPlanId);
                            if (insList != null && insList.Count > 0) //code added by balaji.TJ 2015-12-17               
                                objInsurancePlan = insList[0];
                            if (objInsurancePlan != null)
                            {
                                //Jira #CAP-146 - Able to add duplicate insurance
                                //dr["Plan_Name"] = objInsurancePlan.Ins_Plan_Name;
                                dr["Plan_Name"] = objInsurancePlan.Ins_Plan_Name.ToUpper();
                                Carrier objcarrierName = CarrierMngr.GetCarrierUsingId(Convert.ToUInt64(objInsurancePlan.Carrier_ID));
                                if (objcarrierName != null)
                                    dr["Carrier_Name"] = objcarrierName.Carrier_Name;
                                //Added by srividhya on 6-Aug-2014
                                dr["CarrierID"] = objInsurancePlan.Carrier_ID.ToString();
                                dr["PlanType"] = objInsurancePlan.Financial_Class_Name;
                            }
                            dr["Group_Number"] = obj.Group_Number;
                            dr["Insurance_Type"] = obj.Insurance_Type;
                            dr["Patient_Name"] = "";
                            dr["Relationship"] = obj.Relationship;
                            dr["Relationship_Number"] = obj.Relationship_No;
                            dr["Active"] = obj.Active;
                            if (obj.Effective_Start_Date != DateTime.MinValue)
                                dr["Effective_Start_Date"] = obj.Effective_Start_Date.ToString("dd-MMM-yyyy");
                            if (obj.Termination_Date != DateTime.MinValue)
                                dr["Termination_Date"] = obj.Termination_Date.ToString("dd-MMM-yyyy");
                            if (InsuredHumanList != null)
                            {
                                //Jira #CAP-146 - Able to add duplicate insurance
                                //dr["Insured_Name"] = InsuredHumanList.Last_Name + " " + InsuredHumanList.First_Name;
                                dr["Insured_Name"] = InsuredHumanList.Last_Name + "," + InsuredHumanList.First_Name + " " + InsuredHumanList.MI;
                                dr["Insured_DOB"] = InsuredHumanList.Birth_Date.ToString("dd-MMM-yyyy");
                                dr["Insured_Sex"] = InsuredHumanList.Sex;
                                dr["Insured_Details"] = InsuredHumanList.Last_Name + " " + InsuredHumanList.First_Name + "|DOB: " + Convert.ToDateTime(InsuredHumanList.Birth_Date).ToString("dd-MMM-yyyyy") + "|" + InsuredHumanList.Sex + "| ACC#:" + InsuredHumanList.Id + "| PATIENT TYPE:" + InsuredHumanList.Human_Type;
                            }
                            dr["Insured_Human_ID"] = obj.Insured_Human_ID;
                            dr["Id"] = obj.Id;
                            dr["Specify_Other"] = obj.Other_Insurance_Comments;
                            dr["PCP_Name"] = obj.PCP_Name;
                            dr["PCP_ID"] = obj.PCP_ID;

                            var Phy = from p in physician_dto.PhyList where p.PhyId == obj.PCP_ID select p;
                            IList<PhysicianFacilityDTO> ilstCurrentPhyFacDTO = Phy.ToList<PhysicianFacilityDTO>();

                            if (ilstCurrentPhyFacDTO.Count>0)
                            {
                                //Cap - 2116
                                string sPcpGridName = string.Empty;
                                if (ilstCurrentPhyFacDTO[0].Category.ToUpper() == "ORGANIZATION")
                                {
                                    sPcpGridName = ilstCurrentPhyFacDTO[0].PhyCompany;
                                }
                                else
                                {
                                    sPcpGridName = ilstCurrentPhyFacDTO[0].PhyPrefix + " " + ilstCurrentPhyFacDTO[0].PhyFirstName + " " + ilstCurrentPhyFacDTO[0].PhyMiddleName + " " + ilstCurrentPhyFacDTO[0].PhyLastName;
                                }

                                //string sPcpTextboxName = ilstCurrentPhyFacDTO[0].PhyPrefix + " " + ilstCurrentPhyFacDTO[0].PhyFirstName + " " + ilstCurrentPhyFacDTO[0].PhyMiddleName + " " + ilstCurrentPhyFacDTO[0].PhyLastName + "(" + ilstCurrentPhyFacDTO[0].PhySuffix + ")" + " | " +
                                //                              "NPI:" + ilstCurrentPhyFacDTO[0].PhyNPI + " | " +
                                //                              ilstCurrentPhyFacDTO[0].PhySpecialtyCode + " | " +
                                //                              "FACILITY:" + ilstCurrentPhyFacDTO[0].PhyFacility + " | " +
                                //                              "ADDR: " + ilstCurrentPhyFacDTO[0].PhyAddrs + ", " +
                                //                              ilstCurrentPhyFacDTO[0].PhyCity + "," +
                                //                              ilstCurrentPhyFacDTO[0].PhyState + " " +
                                //                              ilstCurrentPhyFacDTO[0].PhyZip + " | " +
                                //                              ((ilstCurrentPhyFacDTO[0].PhyPhone.Trim()) != "" ? "PH:" + ilstCurrentPhyFacDTO[0].PhyPhone + " | " : "") +
                                //                              (ilstCurrentPhyFacDTO[0].PhyFax.Trim() != "" ? "FAX:" + ilstCurrentPhyFacDTO[0].PhyFax : "");
                                string sPcpTextboxName = string.Empty;
                                if (ilstCurrentPhyFacDTO[0].Category.ToUpper() == "ORGANIZATION")
                                {
                                     sPcpTextboxName = ilstCurrentPhyFacDTO[0].PhyCompany + " | " +
                                                              "NPI:" + ilstCurrentPhyFacDTO[0].PhyNPI + " | " +
                                                              "Facility:" + ilstCurrentPhyFacDTO[0].PhyFacility + " | " +
                                                              "Address: " + ilstCurrentPhyFacDTO[0].PhyAddrs + ", " +
                                                              ilstCurrentPhyFacDTO[0].PhyCity + "," +
                                                              ilstCurrentPhyFacDTO[0].PhyState + " " +
                                                              ilstCurrentPhyFacDTO[0].PhyZip + " | " +
                                                              ((ilstCurrentPhyFacDTO[0].PhyPhone.Trim()) != "" ? "Phone No:" + ilstCurrentPhyFacDTO[0].PhyPhone + " | " : "") +
                                                              (ilstCurrentPhyFacDTO[0].PhyFax.Trim() != "" ? "Fax No:" + ilstCurrentPhyFacDTO[0].PhyFax : "");
                                }
                                else
                                {
                                     sPcpTextboxName = ilstCurrentPhyFacDTO[0].PhyPrefix + " " + ilstCurrentPhyFacDTO[0].PhyFirstName + " " + ilstCurrentPhyFacDTO[0].PhyMiddleName + " " + ilstCurrentPhyFacDTO[0].PhyLastName + "(" + ilstCurrentPhyFacDTO[0].PhySuffix + ")" + " | " +
                                                              "NPI:" + ilstCurrentPhyFacDTO[0].PhyNPI + " | "  +
                                                              "Facility:" + ilstCurrentPhyFacDTO[0].PhyFacility + " | " +
                                                              "Address: " + ilstCurrentPhyFacDTO[0].PhyAddrs + ", " +
                                                              ilstCurrentPhyFacDTO[0].PhyCity + "," +
                                                              ilstCurrentPhyFacDTO[0].PhyState + " " +
                                                              ilstCurrentPhyFacDTO[0].PhyZip + " | " +
                                                              ((ilstCurrentPhyFacDTO[0].PhyPhone.Trim()) != "" ? "Phone No:" + ilstCurrentPhyFacDTO[0].PhyPhone + " | " : "") +
                                                              (ilstCurrentPhyFacDTO[0].PhyFax.Trim() != "" ? "Fax No:" + ilstCurrentPhyFacDTO[0].PhyFax : "");
                                }
                                dr["PCP_Grid_Name"] = sPcpGridName;
                                dr["PCP_Textbox_Name"] = sPcpTextboxName;
                                dr["PCP_NPI"] = ilstCurrentPhyFacDTO[0].PhyNPI;
                                dr["Category"] = ilstCurrentPhyFacDTO[0].Category;

                            }
                            else
                            {
                                dr["PCP_Grid_Name"] = "";
                                dr["PCP_Textbox_Name"] = "";
                                dr["PCP_NPI"] = "";
                            }


                            //for (int icount = 0; icount<=physician_dto.PhyList.Count; icount++)
                            //{
                            //    if (obj.PCP_ID!=0 && obj.PCP_ID == physician_dto.PhyList[icount].PhyId)
                            //    {

                            //        string sPcpGridName = physician_dto.PhyList[icount].PhyPrefix + " " + physician_dto.PhyList[icount].PhyFirstName + " " + physician_dto.PhyList[icount].PhyMiddleName + " " + physician_dto.PhyList[icount].PhyLastName;


                            //        string sPcpTextboxName = physician_dto.PhyList[icount].PhyPrefix + " " + physician_dto.PhyList[icount].PhyFirstName + " " + physician_dto.PhyList[icount].PhyMiddleName + " " + physician_dto.PhyList[icount].PhyLastName + "(" + physician_dto.PhyList[icount].PhySuffix + ")" + " | " +
                            //                                      "NPI:" + physician_dto.PhyList[icount].PhyNPI + " | " +
                            //                                      physician_dto.PhyList[icount].PhySpecialtyCode + " | " +
                            //                                      "FACILITY:" + physician_dto.PhyList[icount].PhyFacility + " | " +
                            //                                      "ADDR: " + physician_dto.PhyList[icount].PhyAddrs + ", " +
                            //                                      physician_dto.PhyList[icount].PhyCity + "," +
                            //                                      physician_dto.PhyList[icount].PhyState + " " +
                            //                                      physician_dto.PhyList[icount].PhyZip + " | " +
                            //                                      ((physician_dto.PhyList[icount].PhyPhone.Trim()) != "" ? "PH:" + physician_dto.PhyList[icount].PhyPhone + " | " : "") +
                            //                                      (physician_dto.PhyList[icount].PhyFax.Trim() != "" ? "FAX:" + physician_dto.PhyList[icount].PhyFax : "");
                            //        dr["PCP_Grid_Name"] = sPcpGridName;
                            //        dr["PCP_Textbox_Name"] = sPcpTextboxName;
                            //        dr["PCP_NPI"] = physician_dto.PhyList[icount].PhyNPI;
                            //        break;
                            //    }
                            //    else
                            //    {
                            //        dr["PCP_Grid_Name"] = "";
                            //        dr["PCP_Textbox_Name"] = "";
                            //        dr["PCP_NPI"] = "";
                            //    }

                            //}



                            dt.Rows.Add(dr);

                        }
                    }
                }

            }
            return JsonConvert.SerializeObject(dt);
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string SetInsuranceType(string humanid, string insuranceType, string id, string active, string PatientName
          , string insurehumanid, string policyholderid, string insid, string Effective_Start_Date, string Termination_Date, string relationship)
        {

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            StaticLookupManager StaticLookupMngr = new StaticLookupManager();
            IList<StaticLookup> Staticlist = StaticLookupMngr.getStaticLookupByFieldName("INSURANCE TYPE");
            DataTable dtSettype = new DataTable();
            PatientInsuredPlanManager PatInsuredMngr = new PatientInsuredPlanManager();
            //insuranceCheck = true;
            //get the patient insurance details using human id from pat_insured table. number of policies for the human will be loaded                  in the list.
            //IList<PatientInsuredPlan> patientInsuredPlanList;
            PatientInsuredPlan patInsured = new PatientInsuredPlan();
            string sOriginalInsType = "";
            HumanManager HumanMngr = new HumanManager();
            IList<PatientInsuredPlan> PatientinsuredList = new List<PatientInsuredPlan>();
            // patientInsuredPlanList = PatInsuredMngr.getInsurancePoliciesByHumanId((Convert.ToUInt64(humanid)));
            PatientInsuredPlan objplan = new PatientInsuredPlan();
            string errormsg = "Success";

            string sCarrierIDList = System.Configuration.ConfigurationSettings.AppSettings["MedicareCarrierIDList"];
            string[] CarrierIDList = sCarrierIDList.Split(',');
            InsurancePlanManager InsMngr = new InsurancePlanManager();
            InsurancePlan objInsPlan = new InsurancePlan();
            IList<InsurancePlan> insList = InsMngr.GetInsurancebyID(Convert.ToUInt64(insid));
            if (insList != null && insList.Count > 0) //code added by balaji.TJ
            {
                objInsPlan = insList[0];
            }
            if (objInsPlan != null)
            {
                IList<Carrier> CarrierList = InsMngr.GetCarrierList();

                if (CarrierIDList.Contains<String>(objInsPlan.Carrier_ID.ToString()) == true)
                {
                    string sResult = UtilityManager.ValidatePolicyHolderID(policyholderid);
                    if (sResult.StartsWith("Fail") == true)
                    {

                        errormsg = "Policy Holder ID is Invalid!&#xA;Format Example:$@" + sResult.Split('|')[1];

                    }
                }
            }
            //if (patientInsuredPlanList != null && patientInsuredPlanList.Count == 0)
            //{
            //    objplan = new PatientInsuredPlan();
            //    objplan.Human_ID = Convert.ToUInt64(humanid);
            //    objplan.Insurance_Type = insuranceType;
            //    objplan.Active = active;
            //    objplan.Insured_Human_ID = Convert.ToUInt64(insurehumanid);
            //    objplan.Insurance_Plan_ID = Convert.ToUInt64(insid);
            //    if (Effective_Start_Date != "")
            //        objplan.Effective_Start_Date = Convert.ToDateTime(Effective_Start_Date);
            //    objplan.Id = Convert.ToUInt64(id);
            //    if (Termination_Date != "")
            //        objplan.Termination_Date = Convert.ToDateTime(Termination_Date);
            //    if (Staticlist != null && Staticlist.Count > 0)
            //    {
            //        for (int j = 0; j < Staticlist.Count; j++)
            //        {
            //            if (Staticlist[j].Value == insuranceType)
            //            {
            //                objplan.Sort_Order = Staticlist[j].Sort_Order;

            //                break;
            //            }
            //        }
            //    }
            //    patientInsuredPlanList.Add(objplan);
            //}


            //if (patientInsuredPlanList != null && patientInsuredPlanList.Count > 0) //code added by balaji.TJ 2015-12-17
            //{
            //    if (id == "0")

            //    {
            //        objplan = new PatientInsuredPlan();
            //        objplan.Human_ID = Convert.ToUInt64(humanid);
            //        objplan.Insurance_Type = insuranceType;
            //        objplan.Active = active;
            //        objplan.Insured_Human_ID = Convert.ToUInt64(insurehumanid);
            //        objplan.Insurance_Plan_ID = Convert.ToUInt64(insid);
            //        if (Effective_Start_Date != "")
            //            objplan.Effective_Start_Date = Convert.ToDateTime(Effective_Start_Date);
            //        if (Termination_Date != "")
            //            objplan.Termination_Date = Convert.ToDateTime(Termination_Date);
            //        if (Staticlist != null && Staticlist.Count > 0)
            //        {
            //            for (int j = 0; j < Staticlist.Count; j++)
            //            {
            //                if (Staticlist[j].Value == insuranceType)
            //                {
            //                    objplan.Sort_Order = Staticlist[j].Sort_Order;

            //                    break;
            //                }
            //            }
            //        }
            //        objplan.Id = Convert.ToUInt64(id);
            //        patientInsuredPlanList.Add(objplan);
            //    }


            //    for (int i = 0; i < patientInsuredPlanList.Count; i++)
            //    {
            //        patInsured = patientInsuredPlanList[i];
            //        if (patientInsuredPlanList[i].Id == Convert.ToUInt64(id))
            //        {
            //            sOriginalInsType = patientInsuredPlanList[i].Insurance_Type;
            //            patientInsuredPlanList[i].Insurance_Type = insuranceType;
            //            patInsured.Insurance_Type = insuranceType;
            //            if (Staticlist != null && Staticlist.Count > 0)
            //            {
            //                for (int j = 0; j < Staticlist.Count; j++)
            //                {
            //                    if (Staticlist[j].Value == insuranceType)
            //                    {
            //                        patInsured.Sort_Order = Staticlist[j].Sort_Order;
            //                        patientInsuredPlanList[i].Sort_Order = Staticlist[j].Sort_Order;
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //        //if the insurance type for the any row is primary then it is changed as old primary and the new one is primary.
            //        if ((patientInsuredPlanList[i].Insurance_Type.ToUpper() == insuranceType.ToUpper()) && (patientInsuredPlanList[i].Id != Convert.ToUInt64(id)))
            //        {
            //            patInsured.Insurance_Type = "OLD " + insuranceType;
            //            patientInsuredPlanList[i].Insurance_Type = "OLD " + insuranceType;
            //            if (Staticlist != null && Staticlist.Count > 0)
            //            {
            //                for (int j = 0; j < Staticlist.Count; j++)
            //                {
            //                    if (Staticlist[j].Value == "OLD " + insuranceType)
            //                    {
            //                        patInsured.Sort_Order = Staticlist[j].Sort_Order;
            //                        patientInsuredPlanList[i].Sort_Order = Staticlist[j].Sort_Order;
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //        // if (patInsured.Id != 0)
            //        {
            //            PatientinsuredList.Add(patInsured);
            //        }
            //    }

            //    if (insuranceType == "PRIMARY")
            //    {
            //        //PatInsuredMngr.BatchUpdatePatInsured(PatientinsuredList.ToArray<PatientInsuredPlan>(), Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[17].Text), string.Empty);

            //        //if (Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[17].Text) != 0 && PatientinsuredList.Count > 0)
            //        //{
            //        //    IList<Human> lstUpdateHuman = new List<Human>();

            //        //    lstUpdateHuman = HumanMngr.GetPatientDetailsUsingPatientInformattion(PatientinsuredList[0].Human_ID);
            //        //    if (lstUpdateHuman.Count() > 0)
            //        //    {
            //        //        lstUpdateHuman[0].Primary_Carrier_ID = Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[17].Text);
            //        //        HumanMngr.UpdateBatchToHuman(lstUpdateHuman[0], null, null, grdPolicyInformation.SelectedRow.Cells[3].Text);
            //        //    }
            //        //}
            //    }
            //    else
            //    {
            //        //PatInsuredMngr.BatchUpdatePatInsured(PatientinsuredList.ToArray<PatientInsuredPlan>(), 0, string.Empty);

            //        //if (PatientinsuredList.Count > 0 && sOriginalInsType == "PRIMARY")
            //        //{
            //        //    IList<Human> lstUpdateHuman = new List<Human>();

            //        //    lstUpdateHuman = HumanMngr.GetPatientDetailsUsingPatientInformattion(PatientinsuredList[0].Human_ID);
            //        //    if (lstUpdateHuman.Count() > 0)
            //        //    {
            //        //        lstUpdateHuman[0].Primary_Carrier_ID = 0;
            //        //        HumanMngr.UpdateBatchToHuman(lstUpdateHuman[0], null, null, string.Empty);
            //        //    }
            //        //}
            //    }


            //    dtSettype = new DataTable();

            //    dtSettype.Columns.Add("Policy_Holder_ID", typeof(string));
            //    dtSettype.Columns.Add("Plan_ID", typeof(string));
            //    dtSettype.Columns.Add("Plan_Name", typeof(string));
            //    dtSettype.Columns.Add("Group_Number", typeof(string));
            //    dtSettype.Columns.Add("Insurance_Type", typeof(string));
            //    dtSettype.Columns.Add("Patient_Name", typeof(string));
            //    dtSettype.Columns.Add("Relationship", typeof(string));
            //    dtSettype.Columns.Add("Active", typeof(string));
            //    dtSettype.Columns.Add("Effective_Start_Date", typeof(string));
            //    dtSettype.Columns.Add("Termination_Date", typeof(string));
            //    dtSettype.Columns.Add("Insured_Name", typeof(string));
            //    dtSettype.Columns.Add("Insured_Human_ID", typeof(string));
            //    dtSettype.Columns.Add("Insured_DOB", typeof(string));
            //    dtSettype.Columns.Add("Insured_Sex", typeof(string));
            //    dtSettype.Columns.Add("Id", typeof(string));
            //    dtSettype.Columns.Add("Carrier_Name", typeof(string));
            //    dtSettype.Columns.Add("CarrierID", typeof(string));
            //    dtSettype.Columns.Add("PlanType", typeof(string));
            //    IList<PatientInsuredPlan> PatInsOrderedList = patientInsuredPlanList.OrderBy(x => x.Sort_Order).ToList<PatientInsuredPlan>();
            //    Human InsuredHumanList = new Human();
            //    InsurancePlanManager InsMngr = new InsurancePlanManager();
            //    CarrierManager CarrierMngr = new CarrierManager();
            //    foreach (PatientInsuredPlan p in PatInsOrderedList) //patientInsuredPlanList)
            //    {
            //        //if ((active == "true" && p.Active.ToUpper() == "YES") || (active == "false"))//&& p.Active.ToUpper() == "NO"))
            //        {
            //            ulong insPlanId;
            //            //store the insurance plan id in a variable.
            //            //get the insurance plan details from insurance_plan table using insurance plan id.
            //            IList<Human> HumanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(p.Insured_Human_ID);
            //            if (HumanList != null && HumanList.Count > 0)  //code added by balaji.tj 2015-12-17                          
            //                InsuredHumanList = HumanList[0];
            //            InsurancePlan objInsurancePlan = new InsurancePlan();
            //            if (dtSettype != null)
            //            {
            //                DataRow dr = dtSettype.NewRow();
            //                dr["Policy_Holder_ID"] = p.Policy_Holder_ID;
            //                dr["Plan_ID"] = p.Insurance_Plan_ID;
            //                insPlanId = Convert.ToUInt64(dr[1]);
            //                IList<InsurancePlan> insList = InsMngr.GetInsurancebyID(insPlanId);
            //                if (insList != null && insList.Count > 0)
            //                    objInsurancePlan = insList[0];
            //                if (objInsurancePlan != null)
            //                {
            //                    dr["Plan_Name"] = objInsurancePlan.Ins_Plan_Name;
            //                    Carrier objcarrierName = CarrierMngr.GetCarrierUsingId(Convert.ToUInt64(objInsurancePlan.Carrier_ID));
            //                    if (objcarrierName != null)
            //                        dr["Carrier_Name"] = objcarrierName.Carrier_Name;
            //                    //Added by srividhya on 6-Aug-2014
            //                    dr["CarrierID"] = objInsurancePlan.Carrier_ID.ToString();
            //                    dr["PlanType"] = objInsurancePlan.Financial_Class_Name;
            //                }
            //                dr["Group_Number"] = p.Group_Number;
            //                dr["Insurance_Type"] = p.Insurance_Type;
            //                dr["Patient_Name"] = PatientName;
            //                dr["Relationship"] = p.Relationship;
            //                dr["Active"] = p.Active;
            //                if (p.Effective_Start_Date != DateTime.MinValue)
            //                    dr["Effective_Start_Date"] = p.Effective_Start_Date.ToString("dd-MMM-yyyy");
            //                if (p.Termination_Date != DateTime.MinValue)
            //                    dr["Termination_Date"] = p.Termination_Date.ToString("dd-MMM-yyyy");
            //                if (InsuredHumanList != null)
            //                    dr["Insured_Name"] = InsuredHumanList.Last_Name + " " + InsuredHumanList.First_Name;
            //                dr["Insured_Human_ID"] = p.Insured_Human_ID;
            //                dr["Insured_DOB"] = InsuredHumanList.Birth_Date.ToString("dd-MMM-yyyy");
            //                dr["Insured_Sex"] = InsuredHumanList.Sex;
            //                dr["Id"] = p.Id;
            //                dtSettype.Rows.Add(dr);
            //                // ((DataTable)Session["dt"]).Rows.Add(dr);
            //            }
            //        }
            //    }

            //}

            var result = new
            {
                Sortorderlookup = Staticlist,
                ValidationError = errormsg
            };
            return JsonConvert.SerializeObject(result);
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string SavePlanData(object[] name, string Human_id)
        {
            IList<PatientInsuredPlan> savelist = new List<PatientInsuredPlan>();
            IList<PatientInsuredPlan> updatelist = new List<PatientInsuredPlan>();
            IList<PatientInsuredPlan> Getlist = new List<PatientInsuredPlan>();
            IList<PatientInsuredPlan> Templistlist = new List<PatientInsuredPlan>();
            Eligibility_VerficationManager EligibilityMngr = new Eligibility_VerficationManager();
            PatientInsuredPlanManager objmanager = new PatientInsuredPlanManager();
            string pcpname = string.Empty;
            string pcpnpi = string.Empty;
            ulong carrierid = 0;
            if (Human_id != "")
            {
                Getlist = objmanager.getInsurancePoliciesByHumanId(Convert.ToUInt64(Human_id));
            }
            IList<Eligibility_Verification> EligList = new List<Eligibility_Verification>();
            IList<Human> lstUpdateHuman = new List<Human>();
            HumanManager HumanMngr = new HumanManager();
            CarrierManager CarrierMngr = new CarrierManager();
            string sCarrierName = "";
            foreach (object[] oj in name)
            {
                PatientInsuredPlan obj = new PatientInsuredPlan();
                Templistlist = (from m in Getlist where m.Id == Convert.ToUInt64(oj[12]) select m).ToList<PatientInsuredPlan>();
                if (Templistlist.Count > 0)
                {
                    Templistlist[0].Insurance_Type = oj[0].ToString();
                    Templistlist[0].Policy_Holder_ID = oj[2].ToString();
                    Templistlist[0].Relationship = oj[3].ToString();
                    if (oj[7].ToString().Trim() != String.Empty)
                        Templistlist[0].Effective_Start_Date = Convert.ToDateTime(oj[7].ToString());
                    if (oj[8].ToString().Trim() != String.Empty)
                        Templistlist[0].Termination_Date = Convert.ToDateTime(oj[8].ToString());
                    if (oj[9].ToString().ToUpper() == "ACTIVE")

                        Templistlist[0].Active = "Yes";
                    else
                        Templistlist[0].Active = "No";

                    Templistlist[0].Sort_Order = Convert.ToInt32(oj[10].ToString());
                    Templistlist[0].Insurance_Plan_ID = Convert.ToUInt64(oj[11].ToString());
                    if (Convert.ToUInt64(oj[13].ToString()) == 0)
                    {
                        Templistlist[0].Insured_Human_ID = Convert.ToUInt64(Human_id);
                    }
                    else
                    {
                        Templistlist[0].Insured_Human_ID = Convert.ToUInt64(oj[13].ToString());
                    }
                    Templistlist[0].Modified_By = ClientSession.UserName;
                    Templistlist[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    Templistlist[0].Other_Insurance_Comments = oj[6].ToString();
                    if (oj[19].ToString() != null)
                    {
                        Templistlist[0].PCP_Name = oj[19].ToString();
                    }
                    if (oj[18].ToString() != null)
                    {
                        Templistlist[0].PCP_ID = Convert.ToUInt64(oj[18].ToString());
                    }
                    Templistlist[0].Relationship_No = Convert.ToInt32(oj[15].ToString());
                    Templistlist[0].Assignment = "Yes";
                    updatelist.Add(Templistlist[0]);
                    EligList = EligibilityMngr.GetEligDetailsUsingHumanandPolicyHolderID(Convert.ToUInt64(Human_id), oj[2].ToString());
                    if (oj[0].ToString().ToUpper() == "PRIMARY")
                    {
                        carrierid = Convert.ToUInt64(oj[17].ToString());
                        pcpname = oj[19].ToString();
                        pcpnpi = oj[21].ToString();
                    }
                    if (EligList != null && EligList.Count > 0)
                    {

                        EligList[0].Insurance_Plan_ID = Convert.ToUInt64(oj[11].ToString());

                        EligibilityMngr.UpdateEligibilityVerificationInformatnion(EligList[0], string.Empty);
                        EligList = EligibilityMngr.GetEligDetailsUsingHumanandPolicyHolderIDandInsPlanID(Convert.ToUInt64(Human_id), oj[2].ToString(), Convert.ToUInt64(oj[11].ToString()));
                    }

                }
                else
                {
                    obj.Human_ID = Convert.ToUInt64(Human_id);
                    obj.Insurance_Type = oj[0].ToString();
                    obj.Policy_Holder_ID = oj[2].ToString();
                    obj.Relationship = oj[3].ToString();
                    if (oj[7].ToString().Trim() != String.Empty)
                        obj.Effective_Start_Date = Convert.ToDateTime(oj[7].ToString());
                    if (oj[8].ToString().Trim() != String.Empty)
                        obj.Termination_Date = Convert.ToDateTime(oj[8].ToString());
                    if (oj[9].ToString().ToUpper() == "ACTIVE")

                        obj.Active = "Yes";
                    else
                        obj.Active = "No";

                    obj.Sort_Order = Convert.ToInt32(oj[10].ToString());
                    obj.Insurance_Plan_ID = Convert.ToUInt64(oj[11].ToString());
                    if (Convert.ToUInt64(oj[13].ToString()) == 0)
                    {
                        obj.Insured_Human_ID = Convert.ToUInt64(Human_id);
                    }
                    else
                    {
                        obj.Insured_Human_ID = Convert.ToUInt64(oj[13].ToString());
                    }

                    if (oj[0].ToString().ToUpper() == "PRIMARY")
                    {
                        carrierid = Convert.ToUInt64(oj[17].ToString());
                        pcpname = oj[19].ToString();
                        pcpnpi = oj[21].ToString();
                    }


                    obj.Created_By = ClientSession.UserName;
                    obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    obj.PCP_Name = oj[19].ToString();
                    obj.PCP_ID = Convert.ToUInt64(oj[18].ToString());
                    obj.Other_Insurance_Comments = oj[6].ToString();
                    obj.Relationship_No = Convert.ToInt32(oj[15].ToString());
                    obj.Assignment = "Yes";
                    savelist.Add(obj);

                }



            }
            objmanager.BatchAddUpdatePatInsured(savelist, updatelist, String.Empty);
            IList<Human> humanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(Human_id));
            Human objHumanList = new Human();
            if (humanList != null && humanList.Count > 0) //code added by balaji.TJ 2015-12-17         
                objHumanList = humanList[0];
            if (objHumanList != null)
            {
                if (objHumanList.PatientInsuredBag != null && objHumanList.PatientInsuredBag.Count > 0)
                {
                    //IList<PatientInsuredPlan> PatInsOrderedList = objHumanList.PatientInsuredBag.OrderBy(x => x.Sort_Order).ToList<PatientInsuredPlan>();
                    IList<PatientInsuredPlan> PatInsOrderedList = (from m in objHumanList.PatientInsuredBag where m.Insurance_Type.ToUpper() == "PRIMARY" && m.Active.ToUpper() == "YES" select m).ToList<PatientInsuredPlan>();


                    if (PatInsOrderedList.Count > 0)
                    {

                        lstUpdateHuman = HumanMngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(Human_id));
                        if (lstUpdateHuman.Count() > 0)
                        {
                            lstUpdateHuman[0].Primary_Carrier_ID = carrierid;
                            lstUpdateHuman[0].PCP_Name = pcpname;
                            lstUpdateHuman[0].PCP_NPI = pcpnpi;
                            Carrier objcarrierName = CarrierMngr.GetCarrierUsingId(Convert.ToUInt64(lstUpdateHuman[0].Primary_Carrier_ID));
                            if (objcarrierName != null)
                                sCarrierName = objcarrierName.Carrier_Name;
                            HumanMngr.UpdateBatchToHuman(lstUpdateHuman[0], null, null, sCarrierName);
                        }
                    }
                    else
                    {

                        lstUpdateHuman = HumanMngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(Human_id));
                        if (lstUpdateHuman.Count() > 0)
                        {
                            lstUpdateHuman[0].Primary_Carrier_ID = 0;
                            lstUpdateHuman[0].PCP_Name = string.Empty;
                            sCarrierName = "";
                            HumanMngr.UpdateBatchToHuman(lstUpdateHuman[0], null, null, sCarrierName);
                        }

                    }
                }
            }



            return "Success";
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            string YesNoMessage = hdnYesNoMessage.Value;
            hdnYesNoMessage.Value = string.Empty;
            System.Diagnostics.Stopwatch SaveTime = new System.Diagnostics.Stopwatch();
            SaveTime.Start();
            ulong myHumanID = 0;
            if (PatientInformationValidation() == false)
                goto l;
            if (hdnPatientID.Value.ToString() != string.Empty)
                myHumanID = Convert.ToUInt64(hdnPatientID.Value);
            if (myHumanID == 0)//(ulong)Session["ulPatientID"] == 0)
            {
                objHuman = new Human();
                SavePatientDetails();
                if (hdnLocalTime.Value != string.Empty)
                    objHuman.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                objHuman.Created_By = ClientSession.UserName;
                objHuman.Birth_Date = Convert.ToDateTime(dtpPatientDOB.Text);
                //CAP-602 - Add new field in Human table so pass data to the DB
                objHuman.Dynamics_Number = txtDynamicsNumber.Text;
                if (chkGuarantorIsPatient.Checked == false)
                {
                    if (dtpGuarantorDOB.Text == "")
                        objHuman.Guarantor_Birth_Date = Convert.ToDateTime("1/1/1900");
                    else if (Dobvalidation(Convert.ToDateTime(dtpGuarantorDOB.Text)) == true)
                        objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpGuarantorDOB.Text);
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420033');", true);
                        dtpGuarantorDOB.Focus();
                        goto l;
                    }
                    ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                }
                else
                {
                    if (dtpPatientDOB.Text != "")
                        objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpPatientDOB.Text);
                    ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                }

                if (dtpEmerDOB.Text != "")
                {
                    if (Dobvalidation(Convert.ToDateTime(dtpEmerDOB.Text)) == true)
                    {
                        if (dtpEmerDOB.Enabled == true)
                            objHuman.Emergency_BirthDate = Convert.ToDateTime(dtpEmerDOB.Text);
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420034');", true);
                        dtpEmerDOB.Focus();
                        goto l;
                    }
                    //}
                }

                string sAccountExtNo = string.Empty;
                if (txtExternalAccNo.Text != "999999999999999")
                    sAccountExtNo = txtExternalAccNo.Text;
                HumanDTO CheckHuman = new HumanDTO();
                //Cap - 1883
                //CheckHuman = HumanMngr.GetPatientDetailsUsingPatientDetails(txtPatientlastname.Text, txtPatientfirstname.Text, Convert.ToDateTime(dtpPatientDOB.Text), ddlPatientsex.Text, txtMedicalRecordno.Text, sAccountExtNo);
                CheckHuman = HumanMngr.GetPatientDetailsUsingPatientDetails(txtPatientlastname.Text, txtPatientfirstname.Text, Convert.ToDateTime(dtpPatientDOB.Text), ddlPatientsex.Text, txtMedicalRecordno.Text, sAccountExtNo, ClientSession.LegalOrg);
                int iSave = 6;
                if (CheckHuman.HumanDetails != null)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}ConfirmHumanDuplicate();", true);
                    goto l;
                }

                if (iSave == 2)
                {
                    txtPatientlastname.Focus();
                    goto l;
                }
                if (CheckHuman.MedicalRecordNoList == true)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420044');", true);
                    txtMedicalRecordno.Focus();
                    goto l;

                }

                if (CheckHuman.Patient_Account_External == true)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420046');", true);
                    txtExternalAccNo.Focus();
                    goto l;
                }

                if (ddlMessageDescription.Text != string.Empty || txtNotes.txtDLC.Text != string.Empty || ddlAssignedTo.Text != string.Empty)
                {
                    AddDetails();
                }

                PatGuarantor objPatguarantor = null;
                int iGuarantorID = 0;
                if (chkGuarantorIsPatient.Checked != true)
                {
                    if (hdnGuarantorID.Value != string.Empty)
                    {
                        iGuarantorID = Convert.ToInt32(hdnGuarantorID.Value);
                        if (iGuarantorID != 0)
                        {
                            objPatguarantor = new PatGuarantor();
                            objPatguarantor.Active = "YES";
                            objPatguarantor.Created_By = ClientSession.UserName;
                            if (hdnLocalTime.Value != string.Empty)
                            {
                                objPatguarantor.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                                objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                            }
                            objPatguarantor.Guarantor_Human_ID = iGuarantorID;
                            objPatguarantor.Relationship = ddlGuarantorRelationship.SelectedItem.Text;
                            if (ddlGuarantorRelationship.SelectedItem.Text != string.Empty && ddlGuarantorRelationship.SelectedIndex != -1)
                            {
                                string temp = ddlGuarantorRelationship.Items[ddlGuarantorRelationship.SelectedIndex].Value;
                                string[] Relation = temp.Split('-');
                                if (Relation.Length == 2)
                                {
                                    objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);
                                }
                            }
                        }
                    }
                    ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                }
                else if (chkGuarantorIsPatient.Checked == true)
                {
                    objPatguarantor = new PatGuarantor();
                    objPatguarantor.Active = "YES";
                    objPatguarantor.Created_By = ClientSession.UserName;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objPatguarantor.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                    }
                    objPatguarantor.Guarantor_Human_ID = iGuarantorID;
                    objPatguarantor.Relationship = ddlGuarantorRelationship.SelectedItem.Text;
                    if (ddlGuarantorRelationship.SelectedItem.Text != string.Empty && ddlGuarantorRelationship.SelectedIndex != -1)
                    {
                        string temp = ddlGuarantorRelationship.Items[ddlGuarantorRelationship.SelectedIndex].Value;
                        string[] Relation = temp.Split('-');
                        if (Relation.Length == 2)
                        {
                            objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);
                        }
                    }
                    ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                }
                Human humanObj = null;
                string sFTPPath = UploadPhoto();

                if (sFTPPath != string.Empty)
                    objHuman.Photo_Path = sFTPPath;

                string sCarrier = string.Empty;
                //if (txtPrimaryInsCarrierName.Text != string.Empty && txtPrimaryInsCarrierName.Text.Trim() != "")
                //{
                //    sCarrier = txtPrimaryInsCarrierName.Text;
                //}

                humanObj = HumanMngr.AppendBatchToHuman(objHuman, objPatguarantor, sCarrier);
                IList<Human> ilstHuman = new List<Human>();
                ilstHuman.Add(objHuman);
                if (humanObj.Id != 0)
                {
                    #region Commented By Deepak
                    //string HumanFileName = "Human" + "_" + humanObj.Id + ".xml";
                    //string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
                    //if (File.Exists(strXmlHumanFilePath) == false && humanObj.Id > 0)
                    //{
                    //    string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                    //    string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                    //    XmlDocument itemDoc = new XmlDocument();
                    //    XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                    //    itemDoc.Load(XmlText);
                    //    XmlNodeList xmlnode = itemDoc.GetElementsByTagName("EncounterDetails");
                    //    xmlnode[0].ParentNode.RemoveChild(xmlnode[0]);

                    //    int iAge = 0;
                    //    iAge = UtilityManager.CalculateAge(humanObj.Birth_Date);

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
                    #endregion

                    UpdateAgeinBlob(humanObj.Id, humanObj.Birth_Date);
                }
                if (humanObj != null)
                {
                    objHumanId = humanObj.Id;
                    hdnHumanId.Value = objHumanId.ToString();
                }
                ulPatientID = objHumanId;
                hdnPatientID.Value = ulPatientID.ToString();
                txtAccountNo.Text = objHumanId.ToString();
                hdnDemoAccNumber.Value = objHumanId.ToString();
                //Added by Srividhya
                Session["HumanID"] = txtAccountNo.Text;

                if (Request["Functionality"] == null)
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}GetAddPatientGuarantor('" + YesNoMessage + "');DisplayErrorMessage('420020');", true);
                else
                {
                    string sHuman = JsonConvert.SerializeObject(objHuman);
                    sHuman = sHuman.Replace("'", "&apos");
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}GetAddPatientGuarantor('" + YesNoMessage + "','" + sHuman + "');DisplayErrorMessage('420020');", true);
                }

                DisablePatientDetails();
                RefreshGuarantor();
                bSaveCheck = true;
                hdnSaveFlag.Value = "false";
                bFormCloseCheck = false;
                btnEditName.Enabled = true;
                btnViewMessage.Enabled = true;
                btnSave.Enabled = false;
                if (hdnbInsuredHuman.Value.ToUpper() == "FALSE")
                {
                }
                else
                {
                    //btnViewUpdateInsurance.Enabled = true;
                }
            }

            else
            {
                if (ddlMessageDescription.Text != string.Empty || txtNotes.txtDLC.Text != string.Empty || ddlAssignedTo.Text != string.Empty)
                {
                    AddDetails();
                }

                objHumanDTO = HumanMngr.GetHumanInuranceAndCarrierDetailsRCM(myHumanID, string.Empty, string.Empty);
                objHuman = objHumanDTO.HumanDetails;
                PatientInsuredPlan objPatInsPLan = null;
                SavePatientDetails();
                if (hdnLocalTime.Value != string.Empty)
                    objHuman.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                objHuman.Modified_By = ClientSession.UserName;
                if (chkGuarantorIsPatient.Checked == false)
                {
                    if (dtpGuarantorDOB.Text != "")
                    {
                        if (Dobvalidation(Convert.ToDateTime(dtpGuarantorDOB.Text)) == true)
                        {
                            objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpGuarantorDOB.Text);
                        }
                        else
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420033');", true);
                            dtpGuarantorDOB.Focus();
                            goto l;
                        }
                    }
                    ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                }
                else
                {
                    if (dtpPatientDOB.Text != "")
                    {
                        objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpPatientDOB.Text);
                    }
                    ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                }
                if (dtpEmerDOB.Text != "")//(dtpEmerDOB.SelectedDate != null)//&& 
                {
                    if (Dobvalidation(Convert.ToDateTime(dtpEmerDOB.Text)) == true)
                    {
                        objHuman.Emergency_BirthDate = Convert.ToDateTime(dtpEmerDOB.Text);
                    }
                    else
                    {
                        //ApplicationObject.erroHandler.DisplayErrorMessage("420034", "Patient Demographics", this.Page);
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420034');", true);
                        dtpEmerDOB.Focus();
                        goto l;
                    }
                }
                string sAccountExtNo = string.Empty;

                if (txtExternalAccNo.Text != "999999999" && txtExternalAccNo.Text != sExternalAccno)
                {
                    sAccountExtNo = txtExternalAccNo.Text;
                }
                //CAP-602 - Add new field in Human table so pass data to the DB
                objHuman.Dynamics_Number = txtDynamicsNumber.Text;
                HumanDTO CheckHuman = new HumanDTO();
                if (txtMedicalRecordno.Text.ToUpper() != objHuman.Medical_Record_Number.ToUpper() && txtExternalAccNo.Text.ToUpper() != objHuman.Patient_Account_External.ToUpper())
                    //Cap - 1883
                    //CheckHuman = HumanMngr.GetPatientDetailsUsingPatientDetails(string.Empty, string.Empty, DateTime.MinValue, string.Empty, txtMedicalRecordno.Text, sAccountExtNo);
                    CheckHuman = HumanMngr.GetPatientDetailsUsingPatientDetails(string.Empty, string.Empty, DateTime.MinValue, string.Empty, txtMedicalRecordno.Text, sAccountExtNo, ClientSession.LegalOrg);
                if (txtMedicalRecordno.Text.ToUpper() != objHumanDTO.HumanDetails.Medical_Record_Number.ToUpper() && CheckHuman.MedicalRecordNoList == true)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420044');", true);
                    txtMedicalRecordno.Focus();
                    goto l;
                }

                if (txtExternalAccNo.Text.ToUpper() != objHumanDTO.HumanDetails.Patient_Account_External.ToUpper() && CheckHuman.Patient_Account_External == true)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420046');", true);
                    txtExternalAccNo.Focus();
                    goto l;
                }

                PatGuarantor objPatguarantor = null;
                int iUpdateGuarantorID = 0;
                if (chkGuarantorIsPatient.Checked != true)
                {
                    if (hdnGuarantorID.Value != string.Empty)
                    {
                        iUpdateGuarantorID = Convert.ToInt32(hdnGuarantorID.Value);
                    }
                    ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                }
                else
                {
                    if (txtAccountNo.Text != string.Empty)
                    {
                        iUpdateGuarantorID = Convert.ToInt32(txtAccountNo.Text);
                    }
                    ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                }
                if (iUpdateGuarantorID != 0)
                {
                    IList<PatGuarantor> patguarantorlist = new List<PatGuarantor>();
                    if (txtAccountNo.Text != string.Empty) //code added by balaji.TJ 
                        patguarantorlist = patGuarantorMngr.GetPatGuarantorDetails(Convert.ToInt32(txtAccountNo.Text), iUpdateGuarantorID);
                    if (patguarantorlist != null && patguarantorlist.Count > 0) //code added by balaji
                    {
                        objPatguarantor = patguarantorlist[0];
                        if (txtAccountNo.Text != string.Empty) //code added by balaji.TJ 
                            objPatguarantor.Human_ID = Convert.ToInt32(txtAccountNo.Text);
                        objPatguarantor.Active = "YES";
                        objPatguarantor.Modified_By = ClientSession.UserName;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objPatguarantor.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                            objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                        }
                        objPatguarantor.Guarantor_Human_ID = iUpdateGuarantorID;
                        objPatguarantor.Relationship = ddlGuarantorRelationship.SelectedItem.Text;
                        if (ddlGuarantorRelationship.SelectedItem.Text != string.Empty && ddlGuarantorRelationship.SelectedIndex != -1)
                        {
                            string temp = ddlGuarantorRelationship.Items[ddlGuarantorRelationship.SelectedIndex].Value;
                            string[] Relation = temp.Split('-');
                            if (Relation.Length == 2)
                                objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);
                        }
                    }
                    else
                    {
                        objPatguarantor = new PatGuarantor();
                        if (txtAccountNo.Text != string.Empty) //code added by balaji.TJ 
                            objPatguarantor.Human_ID = Convert.ToInt32(txtAccountNo.Text);
                        objPatguarantor.Active = "YES";
                        objPatguarantor.Created_By = ClientSession.UserName;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objPatguarantor.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                            objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                        }
                        objPatguarantor.Guarantor_Human_ID = iUpdateGuarantorID;
                        objPatguarantor.Relationship = ddlGuarantorRelationship.SelectedItem.Text;
                        if (ddlGuarantorRelationship.SelectedItem.Text != string.Empty && ddlGuarantorRelationship.SelectedIndex != -1)
                        {
                            string temp = ddlGuarantorRelationship.Items[ddlGuarantorRelationship.SelectedIndex].Value;
                            string[] Relation = temp.Split('-');
                            if (Relation.Length == 2)
                                objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);
                        }
                    }
                }
                string sFTPPath = UploadPhoto();

                if (sFTPPath != string.Empty)
                    objHuman.Photo_Path = sFTPPath;

                string sPriCarrier = string.Empty;
                //if (txtPrimaryInsCarrierName.Text != string.Empty && txtPrimaryInsCarrierName.Text.Trim() != "")
                //{
                //    sPriCarrier = txtPrimaryInsCarrierName.Text;
                //}
                objHumanDTO.HumanDetails = HumanMngr.UpdateBatchToHuman(objHuman, objPatguarantor, objPatInsPLan, sPriCarrier);
                //  objHumanDTO.HumanDetails = HumanMngr.UpdateBatchToHuman(objHuman, objPatguarantor, objPatInsPLan,string.Empty);

                IList<Human> ilstHuman = new List<Human>();
                ilstHuman.Add(objHuman);


                if (objHuman.Id != 0)
                {
                    #region Commented By Deepak

                    //string HumanFileName = "Human" + "_" + objHuman.Id + ".xml";
                    //string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
                    //if (File.Exists(strXmlHumanFilePath) == false && objHuman.Id > 0)
                    //{
                    //    string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                    //    string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                    //    XmlDocument itemDoc = new XmlDocument();
                    //    XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                    //    itemDoc.Load(XmlText);
                    //    XmlNodeList xmlnode = itemDoc.GetElementsByTagName("EncounterDetails");
                    //    xmlnode[0].ParentNode.RemoveChild(xmlnode[0]);

                    //    int iAge = 0;
                    //    iAge = UtilityManager.CalculateAge(objHuman.Birth_Date);

                    //    XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
                    //    if (xmlAge != null && xmlAge.Count > 0)
                    //        xmlAge[0].Attributes[0].Value = iAge.ToString();
                    //    else
                    //    {
                    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
                    //    }

                    //    XmlText.Close();
                    //  //  itemDoc.Save(strXmlHumanFilePath);
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
                    #endregion
                    UpdateAgeinBlob(objHuman.Id, objHuman.Birth_Date);
                }
                if (YesNoMessage == "Yes")
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}YesNoCancel();", true);
                else
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420020');RefreshNotification('Demographics');", true);
                if (ddlGenderIdentity.SelectedValue.ToString().ToUpper().IndexOf("PLEASE SPECIFY") <= -1)
                {
                    TextBoxColorChange(TxtGenderIdentity);
                }
                else
                {
                    TxtGenderIdentity.CssClass = "Editabletxtbox";
                    TxtGenderIdentity.ReadOnly = false;
                }
                if (ddlSexualOrientation.SelectedValue.ToString().ToUpper().IndexOf("PLEASE DESCRIBE") <= -1)
                {
                    TextBoxColorChange(TxtSexualOrientationSpecify);
                }
                else
                {
                    TxtSexualOrientationSpecify.CssClass = "Editabletxtbox";
                    TxtSexualOrientationSpecify.ReadOnly = false;
                }

                DisablePatientDetails();
                //RefreshGuarantor();
                bSaveCheck = true;
                bFormCloseCheck = false;
                btnEditName.Enabled = true;
                btnSave.Enabled = false;
                hdnSaveFlag.Value = "false";

                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Stoploadcursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420020');RefreshNotification('Demographics');", true);

            }

        l:
            SaveTime.Stop();
            //Added By priyangha
            if (ddlPatientStatus.Text == "ALIVE")
            {
                dtpDateOfDeath.Text = string.Empty;
                ddlReasonForDeath.SelectedIndex = 0;
                dtpDateOfDeath.Attributes.Add("readOnly", "readOnly");
                ComboBoxColorChange(ddlReasonForDeath);
                dtpDateOfDeath.Attributes.Add("disabled", "disabled");
            }
            string Raf_Cal_Parameters = string.Empty;
            if (objHumanDTO.HumanDetails != null)
                Raf_Cal_Parameters = objHumanDTO.HumanDetails.Id + "^" + objHumanDTO.HumanDetails.Birth_Date + "^" + objHumanDTO.HumanDetails.Sex;

            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "saveplan", "saveplanDetails();", true);
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Stoploadcursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }


        public void LoadComboboxValues()
        {
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("SEX");
            IEnumerable<StaticLookup> List = null;
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            iStatelist = StateMngr.Getstate();
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlPatientsex.Items.Add(i.Value);
                    ddlGuarantorSex.Items.Add(i.Value);
                    ddlEmerSex.Items.Add(i.Value);
                }
            }
            ddlState.Items.Add("");
            ddlLicenseState.Items.Add("");
            ddlGuarantorState.Items.Add("");
            ddlEmerState.Items.Add("");
            if (iStatelist != null)
            {
                var State_Code_List = iStatelist.Select(aa => aa.State_Code).Distinct().ToArray();
                foreach (string State_Code in State_Code_List)
                {
                    ddlState.Items.Add(State_Code);
                    ddlLicenseState.Items.Add(State_Code);
                    ddlGuarantorState.Items.Add(State_Code);
                    ddlEmerState.Items.Add(State_Code);
                }
            }
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PATIENT RELATIONSHIP");
            if (iStaticlookuplist != null)
            {
                for (int i = 0; i < iStaticlookuplist.Count; i++)
                {
                    ddlPatientRelation.Items.Add(iStaticlookuplist[i].Value);
                    ddlPatientRelation.Items[i].Value = iStaticlookuplist[i].Description;
                }
            }
            //Added By Priyangha For MesageDescription
            iStaticlookuplist.Clear();

            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("MESSAGE DESCRIPTION");

            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }
            ddlMessageDescription.Items.Add("");
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlMessageDescription.Items.Add(i.Value);
                }
            }
            //Added By priyangha 
            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("IMMUNZATION REGISTRY STATUS");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }

            ddlImmunizationRegStatus.Items.Add("");
            string sSelectValue = string.Empty;
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlImmunizationRegStatus.Items.Add(i.Value);
                    if (sSelectValue == string.Empty)
                        sSelectValue = i.Default_Value;
                }

            }
            //added saravanakumar for macra
            if (sSelectValue != string.Empty)
            {
                ListItem item = new ListItem();
                item.Text = sSelectValue;
                ddlImmunizationRegStatus.SelectedIndex = ddlImmunizationRegStatus.Items.IndexOf(item);
            }
            else
                ddlImmunizationRegStatus.SelectedIndex = 0;

            iStaticlookuplist.Clear();

            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PUBLICITY CODE");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }
            ddlPublicityCode.Items.Add("");
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlPublicityCode.Items.Add(i.Value);
                }
            }
            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("EMPLOYMENT STATUS");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }

            ddlEmploymentStatus.Items.Add("");
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlEmploymentStatus.Items.Add(i.Value);
                }
            }

            iStaticlookuplist.Clear();

            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("MARITAL STATUS DEMOGRAPHICS");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }
            ddlPatientMaritalStatus.Items.Add("");
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlPatientMaritalStatus.Items.Add(i.Value);
                }
            }

            iStaticlookuplist.Clear();

            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("ACCOUNT STATUS");

            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }

            ddlAccountStatus.Items.Add("");
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlAccountStatus.Items.Add(i.Value);
                }
            }

            if (ulPatientID == 0)
            {
                for (int i = 0; i < ddlAccountStatus.Items.Count; i++)
                {
                    if (ddlAccountStatus.Items[i].Text.ToUpper() == "ACTIVE")
                    {
                        ddlAccountStatus.SelectedIndex = i;
                        ddlAccountStatus.Enabled = false;
                        break;
                    }
                }
            }

            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("COLLECTION PRIORITY");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }


            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PATIENT SIGNATURE");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }

            ddlPatientSignature.Items.Add("");
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlPatientSignature.Items.Add(i.Value);
                }
            }

            iStaticlookuplist.Clear();

            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("DEMO STATUS");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }
            ddlDemoStatus.Items.Add("");
            if (List != null)
            {
                foreach (FieldLookup i in List)
                {
                    ddlDemoStatus.Items.Add(i.Value);
                }
            }

            IList<FacilityLibrary> facList;
            //facList = ApplicationObject.facilityLibraryList;//Codereview FacilityMngr.GetFacilityList();
            var fac = from f in ApplicationObject.facilityLibraryList where f.Legal_Org == ClientSession.LegalOrg select f;
            facList = fac.ToList<FacilityLibrary>();
            ddlDefaultFacility.Items.Add("");
            if (facList != null && facList.Count > 0)
            {
                for (int i = 0; i < facList.Count; i++)
                {
                    ListItem cboItem = new ListItem();
                    cboItem.Text = facList[i].Fac_Name;
                    this.ddlDefaultFacility.Items.Add(cboItem);
                }
            }

            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RELATIONSHIP");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                var v = from r in iStaticlookuplist where r.Value == "Self" orderby r.Sort_Order select r;
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
                Relationlist = v.ToList<StaticLookup>();
            }
            ddlRelation.Items.Add("");
            ddlGuarantorRelationship.Items.Add("");
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlRelation.Items.Add(i.Value);
                    ddlGuarantorRelationship.Items.Add(i.Value);
                    ddlGuarantorRelationship.Items[ddlGuarantorRelationship.Items.Count - 1].Value = i.Sort_Order + "-" + i.Description;
                }
            }

            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PREFERRED LANGUAGE");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }
            ddlPreferredLanguage.Items.Add("");
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlPreferredLanguage.Items.Add(i.Value);
                }
                ddlPreferredLanguage.Text = "Preferred Language not indicated";

            }
            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RACE");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }
            listRace.Items.Add("[Empty]");
            if (List != null)
            {
                foreach (var i in List)
                {
                    listRace.Items.Add(i.Value);
                    listRace.Items[listRace.Items.Count - 1].Value = i.Sort_Order + "-" + i.Description;//i.Description;
                    listRace.Items[listRace.Items.Count - 1].Attributes["title"] = i.Value;
                }
            }
            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("GRANULARITY");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }
            ListGranularity.Items.Add("[Empty]");
            if (List != null)
            {
                foreach (var i in List)
                {
                    ListGranularity.Items.Add(i.Value);
                    ListGranularity.Items[ListGranularity.Items.Count - 1].Value = i.Sort_Order + "-" + i.Description;//i.Description;
                    ListGranularity.Items[ListGranularity.Items.Count - 1].Attributes["title"] = i.Value;
                }
            }
            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("ETHNICITY");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }

            ddlEthnicity.Items.Add("");
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlEthnicity.Items.Add(i.Value);
                    ddlEthnicity.Items[ddlEthnicity.Items.Count - 1].Value = i.Description;
                }

                for (int i = 0; i < ddlEthnicity.Items.Count; i++)
                {
                    if (ddlEthnicity.Items[i].Text == "Ethnicity Not indicated")
                    {
                        ddlEthnicity.SelectedIndex = i;
                        break;
                    }
                }
            }

            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("REFERRED TO COLLECTION");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0) //codede by balaji.tj
            {
                for (int i = 0; i < iStaticlookuplist.Count; i++)
                {
                    ddlReferredToCollection.Items.Add(iStaticlookuplist[i].Value);
                }
                ddlReferredToCollection.Text = (from h in iStaticlookuplist where h.Default_Value != string.Empty select h).ToList<StaticLookup>()[0].Value;
            }
            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PREFERRED CONFIDENTIAL CORRESPONDENCE MODE");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }
            ddlPreferredCorrespondenceMode.Items.Add("");
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlPreferredCorrespondenceMode.Items.Add(i.Value);
                }
            }
            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PATIENT STATUS DEMOGRAPHICS");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlPatientStatus.Items.Add(i.Value);
                }
            }
            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("REASON FOR DEATH DEMOGRAPHICS");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }
            ddlReasonForDeath.Items.Add("");
            if (List != null)
            {
                foreach (var i in List)
                {
                    ddlReasonForDeath.Items.Add(i.Value);
                    ddlReasonForDeath.Items[ddlReasonForDeath.Items.Count - 1].Attributes["title"] = i.Value;
                }
            }
            iStaticlookuplist.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("HUMAN TYPE");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }
            if (List != null)
            {
                foreach (var i in List)
                {
                    cboHumanType.Items.Add(i.Value);
                }
                cboHumanType.SelectedIndex = 0;
            }
            ddlDeclaredBankruptcy.Items.Clear();
            ilstStatiLookUpList = objStaticLookUpMngr.getStaticLookupByFieldName("DECLAREDBANKRUPTCY", "Sort_Order");
            if (ilstStatiLookUpList != null && ilstStatiLookUpList.Count > 0) //code added by balaji.tj
            {
                for (int i = 0; i < ilstStatiLookUpList.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = i.ToString();
                    item.Text = ilstStatiLookUpList[i].Value;
                    ddlDeclaredBankruptcy.Items.Add(item);


                }
            }
            ddlCGRelation.Items.Clear();
            iStaticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RELATION", "Sort_Order");
            if (iStaticlookuplist != null && iStaticlookuplist.Count > 0)
            {
                List = from h in iStaticlookuplist orderby h.Sort_Order select h;
            }
            if (List != null)
            {
                ddlCGRelation.Items.Add("");
                foreach (FieldLookup i in List)
                {
                    ddlCGRelation.Items.Add(i.Value);

                    if (i.Value == i.Default_Value)
                        ddlCGRelation.SelectedValue = i.Value;
                }
            }
            ddlDataSharingPreference.Items.Add("");
            ddlDataSharingPreference.Items.Add("Yes");
            ddlDataSharingPreference.Items.Add("No");

            ddlBirthIndicator.Items.Add("");
            ddlBirthIndicator.Items.Add("Yes");
            ddlBirthIndicator.Items.Add("No");

            ddlBirthOrder.Items.Add("");
            ddlBirthOrder.Items.Add("1");
            ddlBirthOrder.Items.Add("2");
            ddlBirthOrder.Items.Add("3");
            ddlBirthOrder.Items.Add("4");
            ddlBirthOrder.Items.Add("5");
            ddlBirthOrder.Items.Add("6");
            ddlBirthOrder.Items.Add("7");
            ddlBirthOrder.Items.Add("8");
            ddlBirthOrder.Items.Add("9");



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
                    Dictionary<string, string> dictSEXUALORIENTATION = new Dictionary<string, string>();
                    Dictionary<string, string> dictGenderIdentity = new Dictionary<string, string>();
                    Dictionary<string, string> dictPaitentSuffix = new Dictionary<string, string>();
                    for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                    {

                        if (xmlNodeList[0].ChildNodes[j].Attributes[0].Value == "SEXUAL ORIENTATION")
                            dictSEXUALORIENTATION.Add(xmlNodeList[0].ChildNodes[j].Attributes[1].Value, xmlNodeList[0].ChildNodes[j].Attributes[1].Value);
                        else if (xmlNodeList[0].ChildNodes[j].Attributes[0].Value == "GENDER IDENTITY")
                            dictGenderIdentity.Add(xmlNodeList[0].ChildNodes[j].Attributes[1].Value, xmlNodeList[0].ChildNodes[j].Attributes[1].Value);
                        else if (xmlNodeList[0].ChildNodes[j].Attributes[0].Value == "PATIENT SUFFIX")
                            dictPaitentSuffix.Add(xmlNodeList[0].ChildNodes[j].Attributes[1].Value, xmlNodeList[0].ChildNodes[j].Attributes[1].Value);
                    }

                    ddlSexualOrientation.DataSource = dictSEXUALORIENTATION;
                    ddlSexualOrientation.DataTextField = "Key";
                    ddlSexualOrientation.DataValueField = "Value";
                    ddlSexualOrientation.DataBind();

                    ddlGenderIdentity.DataSource = dictGenderIdentity;
                    ddlGenderIdentity.DataTextField = "Key";
                    ddlGenderIdentity.DataValueField = "Value";
                    ddlGenderIdentity.DataBind();

                    ddlSuffix.DataSource = dictPaitentSuffix;
                    ddlSuffix.DataTextField = "Key";
                    ddlSuffix.DataValueField = "Value";
                    ddlSuffix.DataBind();
                }
            }
        }

        public void DisableGroupbox()
        {
            DisableTableLayout(pnlGuarantorInfo);
            DisableTableLayout(pnlEmergencyInfo);
            DisableTableLayout(pnlAccountInfo);
            chkGuarantorIsPatient.Enabled = false;
        }
        public void DisableTableLayout(Panel tablelayout)
        {
            for (int i = 0; i < tablelayout.Controls.Count; i++)
            {
                if (tablelayout.Controls[i].GetType().ToString().Contains("RadMaskedTextBox"))
                {
                    RadMaskedTextBox MskTxtBox = (RadMaskedTextBox)tablelayout.Controls[i];
                    MaskedTextBoxColorChange(MskTxtBox);
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("TextBox"))
                {
                    TextBox txtBox = (TextBox)tablelayout.Controls[i];
                    TextBoxColorChange(txtBox);
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("DropDownList"))
                {
                    DropDownList combobox = (DropDownList)tablelayout.Controls[i];
                    ComboBoxColorChange(combobox);
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("CheckBox"))
                {
                    CheckBox checkbox = (CheckBox)tablelayout.Controls[i];
                    if (checkbox.ID != "chkGuarantorIsPatient")
                    {
                        CheckBoxColorChange(checkbox);
                    }
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("RadDatePicker"))
                {
                    RadDatePicker datetimepicker = (RadDatePicker)tablelayout.Controls[i];
                    DateTimePickerColorChange(datetimepicker);
                }

            }
        }

        public void LoadHumanDetails(ulong ulPatientID, string BatchName, string DOOS)
        {

            objHumanDTO = HumanMngr.GetHumanInuranceAndCarrierDetailsRCM(ulPatientID, BatchName, DOOS);
            if (objHumanDTO != null)
            {
                if (objHumanDTO.HumanDetails.Photo_Path.ToString() != string.Empty)
                    LoadPatientPhoto(objHumanDTO.HumanDetails.Photo_Path.ToString());

                txtAccountNo.Text = objHumanDTO.HumanDetails.Id.ToString();
                hdnDemoAccNumber.Value = objHumanDTO.HumanDetails.Id.ToString();
                //Added by Srividhya
                Session["HumanID"] = txtAccountNo.Text;
                txtPatientlastname.Text = objHumanDTO.HumanDetails.Last_Name.ToString();
                txtPatientfirstname.Text = objHumanDTO.HumanDetails.First_Name.ToString();
                txtPatientmiddlename.Text = objHumanDTO.HumanDetails.MI.ToString();
                txtMedicalRecordno.Text = objHumanDTO.HumanDetails.Medical_Record_Number.ToString();
                TxtSexualOrientationSpecify.Text = objHumanDTO.HumanDetails.Sexual_Orientation_Specify.ToString();
                TxtGenderIdentity.Text = objHumanDTO.HumanDetails.Gender_Identity_Specify.ToString();
                for (int i = 0; i < ddlGenderIdentity.Items.Count; i++)
                {
                    if (Convert.ToString(ddlGenderIdentity.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Gender_Identity.ToUpper())
                    {
                        ddlGenderIdentity.SelectedIndex = i;
                        if (ddlGenderIdentity.SelectedValue.ToString().ToUpper().IndexOf("PLEASE SPECIFY") > -1)
                        {
                            TxtGenderIdentity.ReadOnly = false;
                            TxtGenderIdentity.CssClass = "Editabletxtbox";
                        }

                        break;
                    }
                }

                for (int i = 0; i < ddlPatientsex.Items.Count; i++)
                {
                    if (Convert.ToString(ddlPatientsex.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Sex.ToUpper())
                    {
                        ddlPatientsex.SelectedIndex = i;
                        HiddenPatientSex.Value = objHumanDTO.HumanDetails.Sex;
                        break;
                    }
                }

                for (int i = 0; i < ddlSexualOrientation.Items.Count; i++)
                {
                    if (Convert.ToString(ddlSexualOrientation.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Sexual_Orientation.ToUpper())
                    {
                        ddlSexualOrientation.SelectedIndex = i;
                        if (ddlSexualOrientation.SelectedValue.ToString().ToUpper().IndexOf("PLEASE DESCRIBE") > -1)
                        {
                            TxtSexualOrientationSpecify.ReadOnly = false;
                            TxtSexualOrientationSpecify.CssClass = "Editabletxtbox";
                        }
                        break;
                    }
                }
                dtpPatientDOB.Text = objHumanDTO.HumanDetails.Birth_Date.ToString("dd-MMM-yyyy"); ;
                txtPatientAddress.Text = objHumanDTO.HumanDetails.Street_Address1.ToString();
                txtPatientAddressLine2.Text = objHumanDTO.HumanDetails.Street_Address2.ToString();
                msktxtZipcode.Text = objHumanDTO.HumanDetails.ZipCode.ToString();
                txtCity.Text = objHumanDTO.HumanDetails.City.ToString();

                for (int i = 0; i < ddlState.Items.Count; i++)
                {
                    if (Convert.ToString(ddlState.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.State.ToUpper())
                    {
                        ddlState.SelectedIndex = i;
                        break;
                    }
                }
                for (int i = 0; i < ddlSuffix.Items.Count; i++)
                {
                    if (Convert.ToString(ddlSuffix.Items[i].Value).ToUpper() == objHumanDTO.HumanDetails.Suffix.ToUpper())
                    {
                        ddlSuffix.SelectedIndex = i;
                        break;
                    }
                }
                msktxtSSN.Text = objHumanDTO.HumanDetails.SSN;
                for (int i = 0; i < ddlPatientMaritalStatus.Items.Count; i++)
                {
                    if (Convert.ToString(ddlPatientMaritalStatus.Items[i].Value).ToUpper() == objHumanDTO.HumanDetails.Marital_Status.ToUpper())
                    {
                        ddlPatientMaritalStatus.SelectedIndex = i;
                        break;
                    }
                }
                msktxtCellPhno.Text = objHumanDTO.HumanDetails.Cell_Phone_Number;
                msktxtHomePhno.Text = objHumanDTO.HumanDetails.Home_Phone_No;
                txtEmail.Text = objHumanDTO.HumanDetails.EMail;
                txtGuaEmail.Text = objHumanDTO.HumanDetails.Gaurantor_Email;
                //Added By Manimaran
                txtReptEmail.Text = objHumanDTO.HumanDetails.Representative_Email;
                txtDriverLicenseno.Text = objHumanDTO.HumanDetails.Driver_License_Num;
                for (int i = 0; i < ddlLicenseState.Items.Count; i++)
                {
                    if (Convert.ToString(ddlLicenseState.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Driver_State.ToUpper())
                    {
                        ddlLicenseState.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < ddlEmploymentStatus.Items.Count; i++)
                {
                    if (Convert.ToString(ddlEmploymentStatus.Items[i].Value).ToUpper() == objHumanDTO.HumanDetails.Employment_Status.ToUpper())
                    {
                        ddlEmploymentStatus.SelectedIndex = i;
                        break;
                    }
                }
                txtEmployerName.Text = objHumanDTO.HumanDetails.Employer_Name;
                msktxtWorkPhoneNo.Text = objHumanDTO.HumanDetails.Work_Phone_No;
                txtExtensionNumber.Text = objHumanDTO.HumanDetails.Work_Phone_Ext;
                if (objHumanDTO.HumanDetails.PatientInsuredBag != null && objHumanDTO.HumanDetails.PatientInsuredBag.Count > 0)
                    txtNoofPolicies.Text = objHumanDTO.HumanDetails.PatientInsuredBag.Count.ToString();

                //srividhya
                dtpAccCreationDate.Text = UtilityManager.ConvertToLocal(objHumanDTO.HumanDetails.Created_Date_And_Time).ToString("dd-MMM-yyyy");
                txtPatientStatementFormat.Text = objHumanDTO.HumanDetails.Patient_Statement_Format;
                for (int i = 0; i < ddlPatientSignature.Items.Count; i++)
                {
                    if (Convert.ToString(ddlPatientSignature.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.SigOn_File.ToUpper())
                    {
                        ddlPatientSignature.SelectedIndex = i;
                        break;
                    }
                }
                for (int i = 0; i < ddlAccountStatus.Items.Count; i++)
                {
                    if (Convert.ToString(ddlAccountStatus.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Account_Status.ToUpper())
                    {
                        ddlAccountStatus.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < ddlDemoStatus.Items.Count; i++)
                {
                    if (Convert.ToString(ddlDemoStatus.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Demo_Status.ToUpper())
                    {
                        ddlDemoStatus.SelectedIndex = i;
                        break;
                    }
                }
                txtGuarantorLastName.Text = objHumanDTO.HumanDetails.Guarantor_Last_Name;
                txtGuarantorFirstName.Text = objHumanDTO.HumanDetails.Guarantor_First_Name;
                txtGuarantorMiddleName.Text = objHumanDTO.HumanDetails.Guarantor_MI;
                for (int i = 0; i < ddlGuarantorSex.Items.Count; i++)
                {
                    if (Convert.ToString(ddlGuarantorSex.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Guarantor_Sex.ToUpper())
                    {
                        ddlGuarantorSex.SelectedIndex = i;
                        hdnGuarantorSex.Value = ddlGuarantorSex.Items[i].Text;
                        break;
                    }
                }
                if ((objHumanDTO.HumanDetails.Guarantor_Birth_Date == Convert.ToDateTime("1/1/0001")) || (objHumanDTO.HumanDetails.Guarantor_Birth_Date == Convert.ToDateTime("1/1/1900")))
                {
                    dtpGuarantorDOB.Text = string.Empty;

                }
                else
                {
                    dtpGuarantorDOB.Text = objHumanDTO.HumanDetails.Guarantor_Birth_Date.ToString("dd-MMM-yyyy");
                }
                txtGuarantorAddress.Text = objHumanDTO.HumanDetails.Guarantor_Street_Address1;
                txtGuarantorAddressLine2.Text = objHumanDTO.HumanDetails.Guarantor_Street_Address2;
                txtGuarantorCity.Text = objHumanDTO.HumanDetails.Guarantor_City;

                msktxtGuarantorZipCode.Text = objHumanDTO.HumanDetails.Guarantor_Zip_Code;
                for (int i = 0; i < ddlGuarantorRelationship.Items.Count; i++)
                {
                    if (Convert.ToString(ddlGuarantorRelationship.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Guarantor_Relationship.ToUpper())
                    {
                        ddlGuarantorRelationship.SelectedIndex = i;
                        break;
                    }
                }
                for (int i = 0; i < ddlGuarantorState.Items.Count; i++)
                {
                    if (Convert.ToString(ddlGuarantorState.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Guarantor_State.ToUpper())
                    {
                        ddlGuarantorState.SelectedIndex = i;
                        hdnGuarantorState.Value = objHumanDTO.HumanDetails.Guarantor_State;
                        break;
                    }
                }
                if (objHumanDTO.HumanDetails.Guarantor_Is_Patient.ToUpper() == "Y")
                {
                    chkGuarantorIsPatient.Checked = true;
                }
                msktxtGuarantorCellNo.Text = objHumanDTO.HumanDetails.Guarantor_CellPhone_Number;
                msktxtGuarantorHomeNo.Text = objHumanDTO.HumanDetails.Guarantor_Home_Phone_Number;
                txtEmerLastName.Text = objHumanDTO.HumanDetails.Emergency_Cnt_Last_Name;
                txtEmerMiddleName.Text = objHumanDTO.HumanDetails.Emergency_Cnt_MI;
                txtEmerFirstName.Text = objHumanDTO.HumanDetails.Emergency_Cnt_First_Name;
                txtEmerAddress.Text = objHumanDTO.HumanDetails.Emergency_Cnt_StreetAddr1;
                txtEmerCity.Text = objHumanDTO.HumanDetails.Emergency_Cnt_City;
                msktxtEmerZipCode.Text = objHumanDTO.HumanDetails.Emergency_Cnt_ZipCode;
                for (int i = 0; i < ddlEmerState.Items.Count; i++)
                {
                    if (Convert.ToString(ddlEmerState.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Emergency_Cnt_State.ToUpper())
                    {
                        ddlEmerState.SelectedIndex = i;
                        break;
                    }
                }
                for (int i = 0; i < ddlRelation.Items.Count; i++)
                {
                    if (Convert.ToString(ddlRelation.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Emer_Relation.ToUpper())
                    {
                        ddlRelation.SelectedIndex = i;
                        break;
                    }
                }
                msktxtEmerCell.Text = objHumanDTO.HumanDetails.Emergency_Cnt_CellPhone_Number;
                msktxtEmerHome.Text = objHumanDTO.HumanDetails.Emergency_Cnt_Home_Phone_Number;
                if ((objHumanDTO.HumanDetails.Emergency_BirthDate == Convert.ToDateTime("1/1/0001")) || (objHumanDTO.HumanDetails.Emergency_BirthDate == Convert.ToDateTime("1/1/1900")))
                {
                    dtpEmerDOB.Text = string.Empty;
                }
                else
                {
                    dtpEmerDOB.Text = objHumanDTO.HumanDetails.Emergency_BirthDate.ToString("dd-MMM-yyyy");
                }
                for (int i = 0; i < ddlEmerSex.Items.Count; i++)
                {
                    if (Convert.ToString(ddlEmerSex.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Emergency_Cnt_Sex.ToUpper())
                    {
                        ddlEmerSex.SelectedIndex = i;
                        break;
                    }
                }
                txtExternalAccNo.Text = objHumanDTO.HumanDetails.Patient_Account_External;
                //CAP-602 - Add new field in Human table so get data to the DB
                txtDynamicsNumber.Text = objHumanDTO.HumanDetails.Dynamics_Number;
                for (int i = 0; i < ddlDefaultFacility.Items.Count; i++)
                {
                    if (Convert.ToString(ddlDefaultFacility.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Facility_Name.ToUpper())
                    {
                        ddlDefaultFacility.SelectedIndex = i;
                        break;
                    }
                }
                for (int i = 0; i < ddlPreferredLanguage.Items.Count; i++)
                {
                    if (Convert.ToString(ddlPreferredLanguage.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Preferred_Language.ToUpper())
                    {
                        ddlPreferredLanguage.SelectedIndex = i;
                        break;
                    }
                }
                if (objHumanDTO.HumanDetails.Is_Translator_Required.ToUpper() == "Y")
                {
                    chkReqTranslator.Checked = true;
                }
                txtRace.Text = objHumanDTO.HumanDetails.Race.ToString();
                txtGranularity.Text = objHumanDTO.HumanDetails.Granularity.ToString();

                TxtPreviousName.Text = objHumanDTO.HumanDetails.Previous_Name.ToString();

                for (int i = 0; i < ddlEthnicity.Items.Count; i++)
                {
                    if (Convert.ToString(ddlEthnicity.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Ethnicity.ToUpper())
                    {
                        ddlEthnicity.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < ddlGuarantorRelationship.Items.Count; i++)
                {
                    if (Convert.ToString(ddlGuarantorRelationship.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Guarantor_Relationship.ToUpper())
                    {
                        ddlGuarantorRelationship.SelectedIndex = i;
                        break;
                    }
                }
                sExternalAccno = txtExternalAccNo.Text;
                if (objHumanDTO.HumanDetails.People_In_Collection.ToUpper() == "N")
                {
                    for (int i = 0; i < ddlReferredToCollection.Items.Count; i++)
                    {
                        if (Convert.ToString(ddlReferredToCollection.Items[i].Text).ToUpper() == "NO")
                        {
                            ddlReferredToCollection.SelectedIndex = i;
                            break;
                        }
                    }
                }

                else
                {
                    for (int i = 0; i < ddlReferredToCollection.Items.Count; i++)
                    {
                        if (Convert.ToString(ddlReferredToCollection.Items[i].Text).ToUpper() == "YES")
                        {
                            ddlReferredToCollection.SelectedIndex = i;
                            break;
                        }
                    }
                }

                if (objHumanDTO.HumanDetails.PatientInsuredBag != null && objHumanDTO.HumanDetails.PatientInsuredBag.Count > 0)
                {
                    for (int i = 0; i < objHumanDTO.HumanDetails.PatientInsuredBag.Count; i++)
                    {
                        if (objHumanDTO.HumanDetails.PatientInsuredBag[i].Active.ToUpper() == "YES" && objHumanDTO.HumanDetails.PatientInsuredBag[i].Insurance_Type.ToUpper() == "PRIMARY")
                        {
                            if (objHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_ID != 0)
                            {
                                IList<PhysicianLibrary> phylist = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(objHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_ID));
                                if (phylist != null && phylist.Count > 0)
                                {
                                    PhysicianLibrary objPhyLib = phylist[0];
                                    if (objPhyLib != null)
                                    {
                                        string sPhyName = objPhyLib.PhyPrefix + " " + objPhyLib.PhyFirstName + " " + objPhyLib.PhyMiddleName + " " + objPhyLib.PhyLastName + " " + objPhyLib.PhySuffix;
                                        //txtPCPProvider.Text = sPhyName;
                                        //txtNPI.Text = objPhyLib.PhyNPI.ToString();
                                        txtPCPProviderTag.Value = objHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_ID.ToString();
                                        break;
                                    }
                                }
                            }
                            else if (objHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_Name != string.Empty && objHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_NPI != string.Empty)
                            {
                                // txtPCPProvider.Text = objHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_Name;
                                //txtNPI.Text = objHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_NPI;
                                break;
                            }
                        }
                    }
                }
                else if (objHumanDTO.HumanDetails.Encounter_Provider_ID != 0)
                {
                    IList<PhysicianLibrary> phylist = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(objHumanDTO.HumanDetails.Encounter_Provider_ID));
                    if (phylist != null && phylist.Count > 0)
                    {
                        PhysicianLibrary objPhyLib = phylist[0];

                        if (objPhyLib != null)
                        {
                            string sPhyName = objPhyLib.PhyPrefix + " " + objPhyLib.PhyFirstName + " " + objPhyLib.PhyMiddleName + " " + objPhyLib.PhyLastName + " " + objPhyLib.PhySuffix;
                            // txtPCPProvider.Text = sPhyName;
                            //txtNPI.Text = objPhyLib.PhyNPI.ToString();
                            txtPCPProviderTag.Value = objHumanDTO.HumanDetails.Encounter_Provider_ID.ToString();
                        }
                    }
                }

                //txtPrimaryInsPlanName.Text = objHumanDTO.InsuranceDetails.Ins_Plan_Name;
                hdnPrimInsPlanID.Value = objHumanDTO.InsuranceDetails.Id.ToString();
                Carrier objCarrier = CarrierMngr.GetCarrierUsingId(Convert.ToUInt64(objHumanDTO.InsuranceDetails.Carrier_ID));
                //txtPrimaryInsCarrierName.Text = objCarrier.Carrier_Name;
                //txtSecInsPlanName.Text = objHumanDTO.SecInsuranceDetails.Ins_Plan_Name;
                hdnSecInsPlanID.Value = objHumanDTO.SecInsuranceDetails.Id.ToString();
                Carrier objsecCarrier = CarrierMngr.GetCarrierUsingId(Convert.ToUInt64(objHumanDTO.SecInsuranceDetails.Carrier_ID));
                //txtSecInsCarrierName.Text = objsecCarrier.Carrier_Name;
                //txtPrimaryInsEvStatus.Text = objHumanDTO.Primary_EligiblityStatus;
                //txtSecInsEVStatus.Text = objHumanDTO.Secondry_EligiblityStatus;

                //txtsecinsuredname.Text = objHumanDTO.sSecondaryInsuranceName;
                //txtSecRelation.Text = objHumanDTO.sSecondaryInsuranceRelation;

                if (objHumanDTO.HumanDetails.Is_Mail_Sent == "Y")
                {
                    chkEnrollOnlineAccess.Checked = true;
                    btnSendEmail.Enabled = true;
                }
                else
                {
                    chkEnrollOnlineAccess.Checked = false;
                    btnSendEmail.Enabled = false;
                }
                msktxtFaxNumber.Text = objHumanDTO.HumanDetails.Fax_Number;
                for (int i = 0; i < ddlPreferredCorrespondenceMode.Items.Count; i++)
                {
                    if (Convert.ToString(ddlPreferredCorrespondenceMode.Items[i].Value).ToUpper() == objHumanDTO.HumanDetails.Preferred_Confidential_Correspodence_Mode.ToUpper())
                    {
                        ddlPreferredCorrespondenceMode.SelectedIndex = i;
                        break;
                    }
                }

                if ((objHumanDTO.HumanDetails.Date_Of_Death == Convert.ToDateTime("1/1/0001")) || (objHumanDTO.HumanDetails.Date_Of_Death == Convert.ToDateTime("1/1/1900")))
                    dtpDateOfDeath.Text = string.Empty;
                else

                    dtpDateOfDeath.Text = objHumanDTO.HumanDetails.Date_Of_Death.ToString("dd-MMM-yyyy");

                for (int i = 0; i < ddlPatientStatus.Items.Count; i++)
                {
                    if (Convert.ToString(ddlPatientStatus.Items[i].Value).ToUpper() == objHumanDTO.HumanDetails.Patient_Status.ToUpper())
                    {
                        ddlPatientStatus.SelectedIndex = i;
                        break;
                    }
                }
                for (int i = 0; i < ddlReasonForDeath.Items.Count; i++)
                {
                    if (Convert.ToString(ddlReasonForDeath.Items[i].Value).ToUpper() == objHumanDTO.HumanDetails.Reason_For_Death.ToUpper())
                    {
                        ddlReasonForDeath.SelectedIndex = i;
                        break;
                    }
                }

                txtRecentScannedStatus.Text = objHumanDTO.InsuranceCopy + "\n" + objHumanDTO.PatientInfo;
                txtRecentVerificationStatus.Text = objHumanDTO.EligiblityVerified;
                //  txtPriInsID.Text = objHumanDTO.sPriInsuredID.ToString();
                //txtPriRelation.Text = objHumanDTO.sPriRelation;
                //txtPriInsuredName.Text = objHumanDTO.sPriInsuredName;
                //Added by priyangha
                txtMothersMaidenName.Text = objHumanDTO.HumanDetails.Mothers_Maiden_Name;
                if (objHumanDTO.HumanDetails.Immunization_Registry_Status != string.Empty)
                    ddlImmunizationRegStatus.Text = objHumanDTO.HumanDetails.Immunization_Registry_Status;
                ddlPublicityCode.Text = objHumanDTO.HumanDetails.Publicity_Code;
                string sDataSharing = string.Empty;
                if (objHumanDTO.HumanDetails.Data_Sharing_Preference.ToUpper() == "Y")
                    sDataSharing = "No";
                else if (objHumanDTO.HumanDetails.Data_Sharing_Preference.ToUpper() == "N")
                    sDataSharing = "Yes";
                for (int i = 0; i < ddlDataSharingPreference.Items.Count; i++)
                {
                    if (Convert.ToString(ddlDataSharingPreference.Items[i].Text).ToUpper() == sDataSharing.ToUpper())
                    {
                        ddlDataSharingPreference.SelectedIndex = i;
                        break;
                    }
                }
                string sIndicator = string.Empty;
                if (objHumanDTO.HumanDetails.Birth_Indicator.ToUpper() == "Y")
                {
                    ddlBirthOrder.Enabled = true;
                    sIndicator = "No";
                }

                else if (objHumanDTO.HumanDetails.Birth_Indicator.ToUpper() == "N")
                {
                    ddlBirthOrder.Enabled = false;
                    sIndicator = "Yes";
                }

                for (int i = 0; i < ddlBirthIndicator.Items.Count; i++)
                {
                    if (Convert.ToString(ddlBirthIndicator.Items[i].Text).ToUpper() == sIndicator.ToUpper())
                    {
                        ddlBirthIndicator.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < ddlBirthOrder.Items.Count; i++)
                {
                    if (Convert.ToString(ddlBirthOrder.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Birth_Order.ToUpper())
                    {
                        ddlBirthOrder.SelectedIndex = i;
                        break;
                    }
                }

                //for (int i = 0; i < objHumanDTO.HumanDetails.PatientInsuredBag.Count; i++)
                //{
                //    if (objHumanDTO.HumanDetails.PatientInsuredBag[i].Insurance_Type.ToUpper() == "SECONDARY" && objHumanDTO.HumanDetails.PatientInsuredBag[i].Active.ToUpper() == "YES")
                //    {
                //        txtSecInsuredID.Text = objHumanDTO.HumanDetails.PatientInsuredBag[i].Insured_Human_ID.ToString();
                //        break;
                //    }

                //}
                for (int i = 0; i < cboHumanType.Items.Count; i++)
                {
                    if (Convert.ToString(cboHumanType.Items[i].Value).ToUpper() == objHumanDTO.HumanDetails.Human_Type.ToUpper())
                    {
                        cboHumanType.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < ddlDeclaredBankruptcy.Items.Count; i++)
                {
                    if (Convert.ToString(ddlDeclaredBankruptcy.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Declared_Bankruptcy.ToUpper())
                    {
                        ddlDeclaredBankruptcy.SelectedIndex = i;
                        break;
                    }
                }
                hdnPatientType.Value = objHumanDTO.HumanDetails.Human_Type.ToUpper();
            }
            txtCGFirstName.Text = objHumanDTO.HumanDetails.Care_Giver_First_Name.ToString();
            txtCGLastName.Text = objHumanDTO.HumanDetails.Care_Giver_Last_Name.ToString();
            msktxtCGPhNo.Text = objHumanDTO.HumanDetails.Care_Giver_Phone_Number;
            for (int i = 0; i < ddlCGRelation.Items.Count; i++)
            {
                if (Convert.ToString(ddlCGRelation.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Care_Giver_Relation.ToString().ToUpper())
                {
                    ddlCGRelation.SelectedIndex = i;
                    break;
                }
            }




            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Stoploadcursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }
        public Boolean PatientInformationValidation()
        {
            if (txtEmail.Text != string.Empty && CheckEmailDuplicate() == false)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('420059');", true);
                txtEmail.Focus();
                return false;
            }
            return true;
        }
        bool bSendMailClick = false;
        public Boolean Dobvalidation(DateTime date)
        {
            if (hdnLocalTime.Value != string.Empty)
            {
                if (date > Convert.ToDateTime(hdnLocalTime.Value))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public Boolean PhNoValid(string sPhno)
        {
            sReplace = sPhno.Replace("_", "");

            if (sReplace.Length < 13)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool IsEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
        private bool CheckEmailDuplicate()
        {
            //Cap - 2033
            //Human humanRec = HumanMngr.GetHumanIfDuplicateEMail(txtEmail.Text);
            Human humanRec = HumanMngr.GetHumanIfDuplicateEMail(txtEmail.Text, ClientSession.LegalOrg);
            Human objHuman = new Human();
            ulong ulHumanID = 0;

            if (txtAccountNo.Text != string.Empty)
            {
                ulHumanID = Convert.ToUInt64(txtAccountNo.Text);
            }

            if (humanRec == null)
            {
                return true;
            }
            else
            {
                if (ulHumanID != humanRec.Id && txtEmail.Text.ToUpper().Trim() == humanRec.EMail.ToUpper().Trim())
                {
                    return false;
                }
                return true;
            }
        }
        public void DisablePatientDetails()
        {
            TextBoxColorChange(txtPatientlastname);
            TextBoxColorChange(txtPatientfirstname);
            TextBoxColorChange(txtPatientmiddlename);
            ComboBoxColorChange(ddlPatientsex);
            ComboBoxColorChange(ddlSuffix);
            //Cap - 669
            //dtpPatientDOB.Enabled = false;
            dtpPatientDOB.CssClass = "nonEditabletxtbox";
            dtpPatientDOB.Attributes.Add("disabled", "disabled");
        }
        public void TextBoxColorChange(TextBox txtbox)
        {
            txtbox.ReadOnly = true;
            txtbox.CssClass = "nonEditabletxtbox";
            if (txtbox.ID == "dtpGuarantorDOB")
            {
                txtbox.ReadOnly = true;
            }
            if (txtbox.ID == "dtpEmerDOB")
            {
                txtbox.ReadOnly = true;
            }
        }
        public void TextBoxColorChangeWhite(TextBox txtbox)
        {
            txtbox.ReadOnly = false;
            txtbox.CssClass = "Editabletxtbox";
        }
        public void ComboBoxColorChange(DropDownList combobox, Boolean bToNormal)
        {
            if (bToNormal == false)
            {
                //Cap - 669
                //combobox.Enabled = false;
                combobox.Attributes.Add("disabled", "disabled");
                combobox.CssClass = "nonEditabletxtbox";
            }
            else
            {
                //Cap - 669
                //combobox.Enabled = true;
                combobox.Attributes.Remove("disabled");
                combobox.Attributes.Add("enabled", "enabled");
                combobox.CssClass = "Editabletxtbox";
            }
        }
        public void ComboBoxColorChange(DropDownList combobox)
        {
            //Cap - 669
            //combobox.Enabled = false;
            combobox.Attributes.Add("disabled", "disabled");
            combobox.CssClass = "nonEditabletxtbox";

        }
        public void CheckBoxColorChange(CheckBox checkbox)
        {
            checkbox.Enabled = false;

        }
        public void SavePatientDetails()
        {
            objHuman.First_Name = txtPatientfirstname.Text;
            objHuman.Last_Name = txtPatientlastname.Text;
            objHuman.MI = txtPatientmiddlename.Text;
            //objHuman.Birth_Date = dtpPatientDOB.SelectedDate.Value;
            objHuman.Birth_Date = Convert.ToDateTime(dtpPatientDOB.Text);
            objHuman.Declared_Bankruptcy = ddlDeclaredBankruptcy.SelectedItem.Text;// "N";
            objHuman.Sex = HiddenPatientSex.Value;
            objHuman.Street_Address1 = txtPatientAddress.Text;
            objHuman.Street_Address2 = txtPatientAddressLine2.Text;
            objHuman.City = txtCity.Text;
            objHuman.State = ddlState.SelectedItem.Text;
            objHuman.Sexual_Orientation = ddlSexualOrientation.SelectedValue.ToString();
            objHuman.Legal_Org = ClientSession.LegalOrg;

            objHuman.Sexual_Orientation_Specify = TxtSexualOrientationSpecify.Text; //hdnSexualOrientationSpecify.Value ;
            if (ddlGenderIdentity.SelectedValue.ToString() == "" && objHuman.Sex != string.Empty && objHuman.Sex.ToUpper() != "UNKNOWN")
            {
                for (int i = 0; i < ddlGenderIdentity.Items.Count; i++)
                {
                    if (Convert.ToString(ddlGenderIdentity.Items[i].Text).ToUpper() == objHuman.Sex.ToUpper())
                    {
                        ddlGenderIdentity.SelectedValue = Convert.ToString(ddlGenderIdentity.Items[i].Text);
                    }
                }
                objHuman.Gender_Identity = objHuman.Sex.ToUpper();
            }
            else
            {
                objHuman.Gender_Identity = ddlGenderIdentity.SelectedValue.ToString();
            }

            objHuman.Gender_Identity_Specify = TxtGenderIdentity.Text;
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
            //if (MaskedTxtBoxValidation(msktxtHomePhno.Text).Length != string.Empty)
            if (msktxtHomePhno.TextWithLiterals != "() -")
            {
                objHuman.Home_Phone_No = msktxtHomePhno.TextWithLiterals;
            }
            else
            {
                objHuman.Home_Phone_No = string.Empty;
            }
            objHuman.SigOn_File = ddlPatientSignature.SelectedItem.Text;
            objHuman.Encounter_Provider_ID = 0;
            objHuman.Suffix = ddlSuffix.SelectedItem.Text;
            objHuman.Marital_Status = ddlPatientMaritalStatus.SelectedItem.Text;
            objHuman.Employment_Status = ddlEmploymentStatus.SelectedItem.Text;
            objHuman.Account_Status = ddlAccountStatus.SelectedItem.Text;
            if (msktxtSSN.TextWithLiterals != "--")
            {
                objHuman.SSN = msktxtSSN.TextWithLiterals;
            }
            else
            {
                objHuman.SSN = string.Empty;
            }
            objHuman.Patient_Statement_Format = txtPatientStatementFormat.Text;
            objHuman.EMail = txtEmail.Text;
            objHuman.Representative_Email = txtReptEmail.Text;
            if (msktxtWorkPhoneNo.TextWithLiterals != "() -")
            {
                objHuman.Work_Phone_No = msktxtWorkPhoneNo.TextWithLiterals;
            }
            else
            {
                objHuman.Work_Phone_No = string.Empty;
            }
            if (ddlReferredToCollection.Text.ToUpper() == "NO")
                objHuman.People_In_Collection = "N";
            else
                objHuman.People_In_Collection = "Y";
            objHuman.Patient_Date_Last_Billed = DateTime.MinValue;
            objHuman.Employer_Name = txtEmployerName.Text;
            objHuman.Driver_State = ddlLicenseState.Text;
            objHuman.Driver_License_Num = txtDriverLicenseno.Text;
            objHuman.Guarantor_Birth_Date = DateTime.MinValue;
            objHuman.Patient_UnApplied_Payments = 0;
            objHuman.Emergency_BirthDate = DateTime.MinValue;
            if (msktxtCellPhno.TextWithLiterals != "() -")
            {
                objHuman.Cell_Phone_Number = msktxtCellPhno.TextWithLiterals;
            }
            else
            {
                objHuman.Cell_Phone_Number = string.Empty;
            }
            objHuman.Demo_Update_Time_Stamp = DateTime.MinValue;
            objHuman.Batch_ID = 0;
            objHuman.Past_Due = 0;
            objHuman.Demo_Status = ddlDemoStatus.Text;
            objHuman.Medical_Record_Number = txtMedicalRecordno.Text;
            objHuman.Care_Giver_First_Name = txtCGFirstName.Text;
            objHuman.Care_Giver_Last_Name = txtCGLastName.Text;
            objHuman.Care_Giver_Relation = ddlCGRelation.Text;
            objHuman.Care_Giver_Phone_Number = msktxtCGPhNo.Text;
            if (chkGuarantorIsPatient.Enabled == true)
            {
                if (chkGuarantorIsPatient.Checked == true)
                {

                    objHuman.Gaurantor_Email = txtGuaEmail.Text;
                    objHuman.Guarantor_Is_Patient = "Y";
                    objHuman.Guarantor_Last_Name = txtPatientlastname.Text;
                    objHuman.Guarantor_First_Name = txtPatientfirstname.Text;
                    objHuman.Guarantor_MI = txtPatientmiddlename.Text;
                    objHuman.Guarantor_Street_Address1 = txtPatientAddress.Text;
                    objHuman.Guarantor_Street_Address2 = txtPatientAddressLine2.Text;
                    objHuman.Guarantor_City = txtCity.Text;
                    objHuman.Guarantor_Sex = ddlPatientsex.Text;
                    objHuman.Guarantor_State = ddlState.Text;
                    objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpPatientDOB.Text);
                    if (msktxtZipcode.TextWithLiterals != "-")
                    {
                        if (msktxtZipcode.TextWithLiterals.Length == 6 && msktxtZipcode.TextWithLiterals.Length < 10)
                        {
                            string[] Split = Convert.ToString(msktxtZipcode.TextWithLiterals).Split('-');
                            if (Split.Length == 2 && Split[1] == string.Empty)
                            {
                                objHuman.Guarantor_Zip_Code = Split[0].ToString();
                            }
                        }
                        else
                        {
                            objHuman.Guarantor_Zip_Code = msktxtZipcode.TextWithLiterals;
                        }
                        //objHuman.Guarantor_Zip_Code = msktxtGuarantorZipCode.Text;
                    }
                    else
                    {
                        objHuman.Guarantor_Zip_Code = string.Empty;
                    }

                    if (msktxtHomePhno.TextWithLiterals != "() -")
                    {
                        objHuman.Guarantor_Home_Phone_Number = msktxtHomePhno.TextWithLiterals;
                    }
                    else
                    {
                        objHuman.Guarantor_Home_Phone_Number = string.Empty;
                    }
                    if (msktxtCellPhno.TextWithLiterals != "() -")
                    {
                        objHuman.Guarantor_CellPhone_Number = msktxtCellPhno.TextWithLiterals;
                    }
                    else
                    {
                        objHuman.Guarantor_CellPhone_Number = string.Empty;
                    }
                    ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                }
                else
                {
                    objHuman.Guarantor_Is_Patient = "N";
                    objHuman.Guarantor_Last_Name = txtGuarantorLastName.Text;
                    objHuman.Guarantor_First_Name = txtGuarantorFirstName.Text;
                    objHuman.Guarantor_MI = txtGuarantorMiddleName.Text;
                    objHuman.Guarantor_Street_Address1 = txtGuarantorAddress.Text;
                    objHuman.Guarantor_Street_Address2 = txtGuarantorAddressLine2.Text;
                    objHuman.Guarantor_City = txtGuarantorCity.Text;
                    //CAP-1440 - In Testing: Guarantor Information displayed as blank in Demographics screen
                    objHuman.Guarantor_Sex = hdnGuarantorSex.Value;
                    objHuman.Guarantor_State = hdnGuarantorState.Value;
                    for (int k = 0; k < ddlGuarantorSex.Items.Count; k++)
                    {
                        if (ddlGuarantorSex.Items[k].Text == hdnGuarantorSex.Value)
                        {
                            ddlGuarantorSex.SelectedIndex = k;
                            break;
                        }
                    }
                    for (int l = 0; l < ddlGuarantorState.Items.Count; l++)
                    {
                        if (ddlGuarantorState.Items[l].Text == hdnGuarantorState.Value)
                        {
                            ddlGuarantorState.SelectedIndex = l;
                            break;
                        }
                    }

                    if (msktxtGuarantorZipCode.TextWithLiterals != "-")
                    {
                        if (msktxtGuarantorZipCode.TextWithLiterals.Length == 6 && msktxtGuarantorZipCode.TextWithLiterals.Length < 10)
                        {
                            string[] Split = Convert.ToString(msktxtGuarantorZipCode.TextWithLiterals).Split('-');
                            if (Split.Length == 2 && Split[1] == string.Empty)
                            {
                                objHuman.Guarantor_Zip_Code = Split[0].ToString();
                            }
                        }
                        else
                        {
                            objHuman.Guarantor_Zip_Code = msktxtGuarantorZipCode.TextWithLiterals;
                        }
                        //objHuman.Guarantor_Zip_Code = msktxtGuarantorZipCode.Text;
                    }
                    else
                    {
                        objHuman.Guarantor_Zip_Code = string.Empty;
                    }

                    if (msktxtGuarantorHomeNo.TextWithLiterals != "() -")
                    {
                        objHuman.Guarantor_Home_Phone_Number = msktxtGuarantorHomeNo.TextWithLiterals;
                    }
                    else
                    {
                        objHuman.Guarantor_Home_Phone_Number = string.Empty;
                    }
                    if (msktxtGuarantorCellNo.TextWithLiterals != "() -")
                    {
                        objHuman.Guarantor_CellPhone_Number = msktxtGuarantorCellNo.TextWithLiterals;
                    }
                    else
                    {
                        objHuman.Guarantor_CellPhone_Number = string.Empty;
                    }
                    ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                }
            }
            objHuman.Emergency_Cnt_First_Name = txtEmerFirstName.Text;
            objHuman.Emergency_Cnt_Last_Name = txtEmerLastName.Text;
            objHuman.Emergency_Cnt_MI = txtEmerMiddleName.Text;
            objHuman.Emergency_Cnt_StreetAddr1 = txtEmerAddress.Text;
            objHuman.Emergency_Cnt_City = txtEmerCity.Text;
            objHuman.Emergency_Cnt_State = ddlEmerState.Text;
            if (msktxtEmerZipCode.TextWithLiterals != "-")
            {
                if (msktxtEmerZipCode.TextWithLiterals.Length == 6 && msktxtEmerZipCode.TextWithLiterals.Length < 10)
                {
                    string[] Split = Convert.ToString(msktxtEmerZipCode.TextWithLiterals).Split('-');
                    if (Split.Length == 2 && Split[1] == string.Empty)
                    {
                        objHuman.Emergency_Cnt_ZipCode = Split[0].ToString();
                    }
                }
                else
                {
                    objHuman.Emergency_Cnt_ZipCode = msktxtEmerZipCode.TextWithLiterals;
                }
            }
            else
            {
                objHuman.Emergency_Cnt_ZipCode = string.Empty;
            }
            if (msktxtEmerHome.TextWithLiterals != "() -")
            {

                objHuman.Emergency_Cnt_Home_Phone_Number = msktxtEmerHome.TextWithLiterals;
            }
            else
            {
                objHuman.Emergency_Cnt_Home_Phone_Number = string.Empty;
            }

            if (msktxtEmerCell.TextWithLiterals != "() -")
            {
                objHuman.Emergency_Cnt_CellPhone_Number = msktxtEmerCell.TextWithLiterals;
            }
            else
            {
                objHuman.Emergency_Cnt_CellPhone_Number = string.Empty;
            }
            objHuman.Emer_Relation = ddlRelation.Text;
            objHuman.Emergency_Cnt_Sex = ddlEmerSex.Text;
            objHuman.Work_Phone_Ext = txtExtensionNumber.Text;
            objHuman.Facility_Name = ddlDefaultFacility.Text;

            #region  Commented for Bugid 22806
            if (txtPCPProviderTag.Value.Trim() != string.Empty)
            {
                objHuman.Encounter_Provider_ID = Convert.ToUInt16(txtPCPProviderTag.Value);
            }
            else
            {
                objHuman.Encounter_Provider_ID = 0;
            }
            // objHuman.PCP_NPI = txtNPI.Text;
            // objHuman.PCP_Name = txtPCPProvider.Text;
            #endregion
            objHuman.Patient_Account_External = txtExternalAccNo.Text;
            objHuman.Preferred_Language = ddlPreferredLanguage.Text;
            if (chkReqTranslator.Checked == true)
            {
                objHuman.Is_Translator_Required = "Y";
            }
            else
            {
                objHuman.Is_Translator_Required = "N";
            }
            objHuman.Race_Alias = "";
            objHuman.Race_No = "0";
            if (hdnRaceTag.Value != string.Empty)
            {
                string[] Split = (hdnRaceTag.Value).Split(',');
                for (int i = 0; i < Split.Count(); i++)
                {
                    string[] Race = Split[i].Split('-');
                    if (Race.Count() == 3)
                    {
                        if (objHuman.Race_Alias == "" && objHuman.Race_No == "0")
                        {
                            objHuman.Race_Alias = Race[1].ToString();
                            objHuman.Race_No = Race[2].ToString();
                        }
                        else
                        {
                            objHuman.Race_Alias = objHuman.Race_Alias + "," + Race[1].ToString();
                            objHuman.Race_No = objHuman.Race_No + "," + Race[2].ToString();
                        }
                    }
                }
                //objHuman.Race = txtRace.Text;

            }
            objHuman.Race = txtRace.Text;
            objHuman.Granularity = txtGranularity.Text;
            objHuman.Previous_Name = TxtPreviousName.Text;
            objHuman.Ethnicity = ddlEthnicity.SelectedItem.Text;
            if (ddlEthnicity.SelectedItem.Text != string.Empty && ddlEthnicity.SelectedIndex != -1)
                objHuman.Ethnicity_No = Convert.ToInt32(ddlEthnicity.Items[ddlEthnicity.SelectedIndex].Value);
            objHuman.Guarantor_Relationship = ddlGuarantorRelationship.SelectedItem.Text;
            if (ddlGuarantorRelationship.SelectedItem.Text != string.Empty && ddlGuarantorRelationship.SelectedIndex != -1)
            {
                string temp = ddlGuarantorRelationship.Items[ddlGuarantorRelationship.SelectedIndex].Value;
                string[] Relation = temp.Split('-');
                if (Relation.Length == 2)
                {
                    objHuman.Guarantor_Relationship_No = Convert.ToInt16(Relation[1]);
                }
            }
            else if (ddlGuarantorRelationship.Text == string.Empty)
            {
                objHuman.Guarantor_Relationship_No = 0;
            }
            else if (ddlGuarantorRelationship.SelectedItem.Text.ToUpper() == "SELF")
            {
                objHuman.Guarantor_Relationship_No = 1;
            }
            if (ddlPreferredCorrespondenceMode.SelectedItem.Text != string.Empty)
            {
                objHuman.Preferred_Confidential_Correspodence_Mode = ddlPreferredCorrespondenceMode.SelectedItem.Text;
            }
            if (chkEnrollOnlineAccess.Checked == true)//&& bSendMailClick == true)
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
                objHuman.Mail_Sent_Date = DateTime.MinValue;
            }
            if (msktxtFaxNumber.TextWithLiterals != "() -")
            {
                objHuman.Fax_Number = msktxtFaxNumber.TextWithLiterals;
            }
            else
            {
                objHuman.Fax_Number = string.Empty;
            }
            if (ddlPatientStatus.Text == "DECEASED")
            {
                //Cap - 1583
                dtpDateOfDeath.Attributes.Remove("readonly");
                dtpDateOfDeath.CssClass = "Editabletxtbox";

                objHuman.Patient_Status = ddlPatientStatus.Text;
                if (dtpDateOfDeath.Text == "")
                {
                    objHuman.Date_Of_Death = Convert.ToDateTime("1/1/0001"); ;
                }
                else
                {
                    objHuman.Date_Of_Death = Convert.ToDateTime(dtpDateOfDeath.Text);
                }
                objHuman.Reason_For_Death = ddlReasonForDeath.Text;
            }
            else
            {
                objHuman.Patient_Status = ddlPatientStatus.Text;
                objHuman.Date_Of_Death = Convert.ToDateTime("1/1/0001");
                objHuman.Reason_For_Death = string.Empty;
            }
            objHuman.Human_Type = cboHumanType.Text;
            objHuman.Mothers_Maiden_Name = txtMothersMaidenName.Text;
            objHuman.Immunization_Registry_Status = ddlImmunizationRegStatus.SelectedItem.Text;
            objHuman.Publicity_Code = ddlPublicityCode.SelectedItem.Text;
            if (ddlDataSharingPreference.SelectedItem.Text == "Yes")
                objHuman.Data_Sharing_Preference = "N";
            else if (ddlDataSharingPreference.SelectedItem.Text == "No")
                objHuman.Data_Sharing_Preference = "Y";

            if (ddlBirthIndicator.SelectedItem.Text == "Yes")
                objHuman.Birth_Indicator = "N";
            else if (ddlBirthIndicator.SelectedItem.Text == "No")
                objHuman.Birth_Indicator = "Y";
            objHuman.Birth_Order = ddlBirthOrder.SelectedItem.Text;
            //Cerner
            objHuman.Is_Cerner_Registered = "N";
            objHuman.Cerner_Universal_Patient_ID = "";

        }

        protected void chkGuarantorIsPatient_CheckedChanged(object sender, EventArgs e)
        {
            bFormCloseCheck = true;
            btnSave.Enabled = true;
            if (chkGuarantorIsPatient.Checked == true)
            {

                txtGuaEmail.Text = txtEmail.Text;
                btnAddGuarantor.Enabled = false;
                btnSelectGaurantor.Enabled = false;
                dtpGuarantorDOB.Enabled = false;
                DisableTableLayout(pnlGuarantorInfo);
                txtGuarantorFirstName.Text = txtPatientfirstname.Text;
                txtGuarantorLastName.Text = txtPatientlastname.Text;
                txtGuarantorMiddleName.Text = txtPatientmiddlename.Text;
                txtGuarantorAddress.Text = txtPatientAddress.Text;
                txtGuarantorAddressLine2.Text = txtPatientAddressLine2.Text;
                txtGuarantorCity.Text = txtCity.Text;
                msktxtGuarantorZipCode.Text = msktxtZipcode.Text;
                txtGuaEmail.Text = txtEmail.Text;


                for (int k = 0; k < ddlGuarantorSex.Items.Count; k++)
                {
                    if (ddlGuarantorSex.Items[k].Text == ddlPatientsex.SelectedItem.Text)
                    {
                        ddlGuarantorSex.ClearSelection();
                        ddlGuarantorSex.Items.FindByValue(ddlPatientsex.SelectedItem.Text).Selected = true;
                        hdnGuarantorSex.Value = ddlPatientsex.SelectedItem.Text;
                        //ddlGuarantorSex.SelectedIndex = k;
                        break;
                    }
                }
                for (int l = 0; l < ddlGuarantorState.Items.Count; l++)
                {
                    if (ddlGuarantorState.Items[l].Text == ddlState.SelectedItem.Text)
                    {

                        ddlGuarantorState.ClearSelection();
                        ddlGuarantorState.Items.FindByValue(ddlState.SelectedItem.Text).Selected = true;
                        hdnGuarantorState.Value = ddlState.SelectedItem.Text;
                        //ddlGuarantorState.SelectedIndex = l;
                        break;
                    }
                }
                for (int i = 0; i < ddlGuarantorRelationship.Items.Count; i++)
                {
                    if (Convert.ToString(ddlGuarantorRelationship.Items[i].Text).ToUpper() == "SELF")
                    {
                        ddlGuarantorRelationship.SelectedIndex = i;
                        break;
                    }
                }
                if (dtpPatientDOB.Text != "")
                {

                    dtpGuarantorDOB.Text = dtpPatientDOB.Text;
                }
                msktxtGuarantorCellNo.Text = msktxtCellPhno.Text;
                msktxtGuarantorHomeNo.Text = msktxtHomePhno.Text;
                if (ddlGuarantorRelationship.SelectedItem.Text == string.Empty)
                {
                    ddlGuarantorRelationship.SelectedIndex = 0;
                }
                ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
            }
            else if (chkGuarantorIsPatient.Checked == false)
            {
                btnAddGuarantor.Enabled = true;
                btnSelectGaurantor.Enabled = true;
                dtpGuarantorDOB.Text = string.Empty;
                DisableTableLayout(pnlGuarantorInfo);
                ddlGuarantorRelationship.Enabled = true;
                ddlGuarantorRelationship.BackColor = Color.White;
                ddlGuarantorSex.ClearSelection();
                ddlGuarantorState.ClearSelection();
                ClearGuarantorInfo();
                ddlGuarantorRelationship.CssClass = "Editabletxtbox";
            }
        }
        public void ClearGuarantorInfo()
        {
            for (int i = 0; i < pnlGuarantorInfo.Controls.Count; i++)
            {
                var ctrl = this.pnlGuarantorInfo.Controls[i];
                if (ctrl is RadMaskedTextBox)
                {
                    var txtControl = ctrl as WebControl;
                    RadMaskedTextBox txtbox = (RadMaskedTextBox)txtControl;
                    txtbox.Text = string.Empty;
                }
                else if (ctrl is TextBox)
                {
                    var txtControl = ctrl as WebControl;
                    TextBox txtbox = (TextBox)txtControl;
                    txtbox.Text = string.Empty;
                }
                else if (ctrl is DropDownList)
                {
                    var ComboControl = ctrl as WebControl;
                    DropDownList cbobox = (DropDownList)ComboControl;
                    cbobox.SelectedIndex = 0;
                }

            }
        }
        public void ClearPatientInfo()
        {
            for (int i = 0; i < pnlPatientInfo.Controls.Count; i++)
            {
                var ctrl = this.pnlPatientInfo.Controls[i];

                if (ctrl is TextBox)
                {
                    var txtControl = ctrl as WebControl;
                    TextBox txtbox = (TextBox)txtControl;
                    txtbox.Text = string.Empty;
                }
                else if (ctrl is DropDownList)
                {
                    var ComboControl = ctrl as WebControl;
                    DropDownList cbobox = (DropDownList)ComboControl;
                    cbobox.SelectedIndex = 0;
                }
                else if (ctrl is CheckBox)
                {
                    var CheckControl = ctrl as WebControl;
                    CheckBox checkbox = (CheckBox)CheckControl;
                    checkbox.Checked = false;
                }
            }
            for (int i = 0; i < pnlGuarantorInfo.Controls.Count; i++)
            {
                var ctrl = this.pnlGuarantorInfo.Controls[i];

                if (ctrl is TextBox)
                {
                    var txtControl = ctrl as WebControl;
                    TextBox txtbox = (TextBox)txtControl;
                    txtbox.Text = string.Empty;
                }
                else if (ctrl is DropDownList)
                {
                    var ComboControl = ctrl as WebControl;
                    DropDownList cbobox = (DropDownList)ComboControl;
                    cbobox.SelectedIndex = 0;
                }
            }
            for (int i = 0; i < pnlEmergencyInfo.Controls.Count; i++)
            {
                var ctrl = this.pnlEmergencyInfo.Controls[i];

                if (ctrl is TextBox)
                {
                    var txtControl = ctrl as WebControl;
                    TextBox txtbox = (TextBox)txtControl;
                    txtbox.Text = string.Empty;
                }
                else if (ctrl is DropDownList)
                {
                    var ComboControl = ctrl as WebControl;
                    DropDownList cbobox = (DropDownList)ComboControl;
                    cbobox.SelectedIndex = 0;

                }
            }
            for (int i = 0; i < pnlAccountInfo.Controls.Count; i++)
            {
                var ctrl = this.pnlAccountInfo.Controls[i];

                if (ctrl is TextBox)
                {
                    var txtControl = ctrl as WebControl;
                    TextBox txtbox = (TextBox)txtControl;
                    txtbox.Text = string.Empty;
                }
                else if (ctrl is DropDownList)
                {
                    var ComboControl = ctrl as WebControl;
                    DropDownList cbobox = (DropDownList)ComboControl;
                    cbobox.SelectedIndex = 0;
                }
            }
        }
        public void ComboBoxColorChangeWhite(DropDownList combobox)
        {
            //cap - 669
            //combobox.Enabled = true;
            combobox.Attributes.Remove("disabled");
            combobox.Attributes.Add("enabled", "enabled");
            combobox.CssClass = "Editabletxtbox";
        }

        public void InsuranceFormOpen()
        {
            if (Convert.ToUInt64(hdnPatientID.Value) == 0)//Session["ulPatientID"]) == 0)
            {
                string link = "frmAddInsurancePolicies.aspx?HumanId=" + hdnPatientID.Value + "&InsuranceType=" + true + "&LastName=" + txtPatientlastname.Text + "&FirstName=" + txtPatientfirstname.Text + "&ExAccountNo=" + txtExternalAccNo.Text;
            }
            else
            {
                string link = "frmAddInsurancePolicies.aspx?HumanId=" + Convert.ToUInt64(txtAccountNo.Text) + "&InsuranceType=" + true + "&LastName=" + txtPatientlastname.Text + "&FirstName=" + txtPatientfirstname.Text + "&ExAccountNo=" + txtExternalAccNo.Text;

            }
        }
        public void InsuranceLoad()
        {
            txtPatientlastname.ReadOnly = true;
            txtPatientlastname.CssClass = "nonEditabletxtbox";
            txtPatientfirstname.ReadOnly = true;
            txtPatientmiddlename.ReadOnly = true;

            txtPatientfirstname.CssClass = "nonEditabletxtbox";

            txtPatientmiddlename.CssClass = "nonEditabletxtbox";

            IList<PatientInsuredPlan> patinsuredlist;
            if (Convert.ToUInt64(hdnPatientID.Value) == 0)
            {
                patinsuredlist = PatInsManager.getInsurancePoliciesByHumanId(Convert.ToUInt64(hdnPatientID.Value));
            }
            else
            {
                patinsuredlist = PatInsManager.getInsurancePoliciesByHumanId(Convert.ToUInt64(txtAccountNo.Text));
            }
            //Added To refresh policy Details
            txtNoofPolicies.Text = patinsuredlist.Count.ToString();
            //txtPrimaryInsCarrierClass.Text = string.Empty;
            // txtPrimaryInsPlanName.Text = string.Empty;
            hdnPrimInsPlanID.Value = "0";

            //txtPrimaryInsCarrierName.Text = string.Empty;
            //txtPriInsuredName.Text = string.Empty;
            //txtSecInsPlanName.Text = string.Empty;
            hdnSecInsPlanID.Value = "0";

            //txtPriRelation.Text = string.Empty;
            //txtPrimaryInsEvStatus.Text = string.Empty;
            // txtSecInsCarrierName.Text = string.Empty;
            //txtSecInsEVStatus.Text = string.Empty;
            //txtPriInsID.Text = string.Empty;
            foreach (PatientInsuredPlan p in patinsuredlist)
            {
                if (p.Insurance_Type.ToUpper() == "PRIMARY" && p.Active.ToUpper() == "YES")
                {
                    //txtPriRelation.Text = p.Relationship;
                    //txtPriInsID.Text = p.Insured_Human_ID.ToString();
                    IList<Human> humanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(p.Insured_Human_ID);
                    if (humanList != null && humanList.Count > 0)
                    {
                        //  txtPriInsuredName.Text = humanList[0].Last_Name + "," + humanList[0].First_Name + " " + humanList[0].MI;
                    }
                    InsurancePlan objInsurancePlan = new InsurancePlan();
                    objInsurancePlan = InsMngr.GetInsurancebyID(p.Insurance_Plan_ID)[0];
                    if (objInsurancePlan != null)
                    {
                        // txtPrimaryInsCarrierClass.Text = objInsurancePlan.Financial_Class_Name;
                        //  txtPrimaryInsPlanName.Text = objInsurancePlan.Ins_Plan_Name;
                        hdnPrimInsPlanID.Value = objInsurancePlan.Id.ToString();
                        //  txtPrimaryInsEvStatus.Text = "ACTIVE";
                        Carrier objCarrier = null;
                        IList<Carrier> CarrierList = InsMngr.GetCarrierList();
                        if (CarrierList != null && CarrierList.Count > 0)
                        {
                            objCarrier = (from h in CarrierList where h.Id == Convert.ToUInt64(objInsurancePlan.Carrier_ID) select h).ToList()[0];
                        }
                        if (objCarrier != null)
                        {
                            // txtPrimaryInsCarrierName.Text = objCarrier.Carrier_Name;
                        }

                    }
                }
                else if (p.Insurance_Type.ToUpper() == "SECONDARY" && p.Active.ToUpper() == "YES")
                {
                    InsurancePlan objInsurancePlan = new InsurancePlan();
                    objInsurancePlan = InsMngr.GetInsurancebyID(p.Insurance_Plan_ID)[0];
                    if (objInsurancePlan != null)
                    {

                        IList<Human> humanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(p.Insured_Human_ID);
                        if (humanList != null && humanList.Count > 0)
                        {
                            // txtsecinsuredname.Text = humanList[0].Last_Name + "," + humanList[0].First_Name + " " + humanList[0].MI;
                        }
                        // txtSecRelation.Text = p.Relationship;


                        // txtSecInsPlanName.Text = objInsurancePlan.Ins_Plan_Name;
                        hdnSecInsPlanID.Value = objInsurancePlan.Id.ToString();
                        // txtSecInsEVStatus.Text = "ACTIVE";
                        Carrier objCarrier = null;
                        IList<Carrier> CarrierList = InsMngr.GetCarrierList();
                        if (CarrierList != null && CarrierList.Count > 0)
                        {
                            objCarrier = (from h in CarrierList where h.Id == Convert.ToUInt64(objInsurancePlan.Carrier_ID) select h).ToList()[0];
                        }
                        if (objCarrier != null)
                        {
                            // txtSecInsCarrierName.Text = objCarrier.Carrier_Name;
                        }
                    }

                }
            }

        }

        protected void chkEnrollOnlineAccess_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnrollOnlineAccess.Checked == true)
            {
                btnSendEmail.Enabled = true;
                spanemail.Attributes.Remove("class");
                spanemail.Attributes.Add("class", "MandLabelstyle");
                spanemailstar.Visible = true;

            }
            else
            {
                btnSendEmail.Enabled = false;
                spanemail.Attributes.Remove("class");
                spanemail.Attributes.Add("class", "spanstyle");
                spanemailstar.Visible = false;
            }
            bFormCloseCheck = true;
            btnSave.Enabled = true;
        }

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            bool saveSuccess = false;
            ulong huId = 0;
            bool bValidation = false;
            btnSendEmail.Enabled = false;
            if (chkEnrollOnlineAccess.Checked != false)
            {
                if (txtEmail.Text.Trim() == string.Empty && txtReptEmail.Text.Trim() == string.Empty)
                {
                    // ApplicationObject.erroHandler.DisplayErrorMessage("420056", "Patient Demographics", this.Page);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('420056');", true);
                    txtEmail.Focus();
                    btnSendEmail.Enabled = true;
                    return;
                }
                else if (txtEmail.Text.Trim() == string.Empty && txtReptEmail.Text.Trim() != string.Empty)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('1007005');", true);
                    txtEmail.Focus();
                    btnSendEmail.Enabled = true;
                    return;
                }
                else if (txtEmail.Text.Trim() == txtReptEmail.Text.Trim())
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('1007006');", true);
                    txtReptEmail.Focus();
                    btnSendEmail.Enabled = true;
                    return;
                }

                else if (txtEmail.Text != string.Empty && IsEmail(txtEmail.Text) == false)
                {
                    // ApplicationObject.erroHandler.DisplayErrorMessage("420060", "Patient Demographics", this.Page);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('420060');", true);
                    txtEmail.Focus();
                    btnSendEmail.Enabled = true;
                    return;
                }
                else if (txtReptEmail.Text != string.Empty && IsEmail(txtReptEmail.Text) == false)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('420060');", true);
                    txtEmail.Focus();
                    btnSendEmail.Enabled = true;
                    return;
                }
                else if (txtEmail.Text != string.Empty && CheckEmailDuplicate() == false)
                {

                    // ApplicationObject.erroHandler.DisplayErrorMessage("420059", "Patient Demographics", this.Page);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('420059');", true);
                    btnSendEmail.Enabled = true;
                    txtEmail.Focus();
                    return;
                }


                else
                {
                    if (hdnPatientID.Value.ToString() != string.Empty)
                    {
                        huId = Convert.ToUInt64(hdnPatientID.Value);
                    }
                    if (PatientInformationValidation() == true)
                    {
                        bValidation = true;
                    }
                    else
                    {
                        bValidation = false;
                    }
                    bSendMailClick = true;
                    if (huId != 0 && bValidation == true)
                    {
                        PatientInsuredPlan objPatInsPLan = null;
                        HumanDTO objHumanDTO = HumanMngr.GetHumanInuranceAndCarrierDetailsRCM(huId, string.Empty, string.Empty);
                        objHuman = objHumanDTO.HumanDetails;
                        SavePatientDetails();
                        #region Commentedforbug id 22806

                        #endregion
                        objHuman.EMail = txtEmail.Text;
                        objHuman.Representative_Email = txtReptEmail.Text;
                        objHuman.SSN = msktxtSSN.TextWithLiterals;
                        objHuman.Is_Mail_Sent = "Y";
                        objHuman.Modified_By = ClientSession.UserName;
                        objHuman.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        //objHuman.Password = "";
                        //objHuman.Representative_Password = "";
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objHuman.Mail_Sent_Date = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        //objHuman.Password = string.Empty;
                        PatGuarantor objPatguarantor = null;
                        int iUpdateGuarantorID = 0;

                        if (chkGuarantorIsPatient.Checked != true)
                        {
                            if (hdnGuarantorID.Value != string.Empty)
                            {
                                iUpdateGuarantorID = Convert.ToInt32(hdnGuarantorID.Value);
                            }
                            ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                        }
                        else
                        {
                            if (txtAccountNo.Text != string.Empty)
                            {
                                iUpdateGuarantorID = Convert.ToInt32(txtAccountNo.Text);
                            }
                        }
                        if (iUpdateGuarantorID != 0)
                        {
                            IList<PatGuarantor> patguarantorlist = new List<PatGuarantor>();
                            if (txtAccountNo.Text != string.Empty) //code added by balaji.TJ
                                patguarantorlist = patGuarantorMngr.GetPatGuarantorDetails(Convert.ToInt32(txtAccountNo.Text), iUpdateGuarantorID);
                            if (patguarantorlist != null && patguarantorlist.Count > 0) //code added by balaji.TJ
                            {
                                objPatguarantor = patguarantorlist[0];
                                if (txtAccountNo.Text != string.Empty)//code added by balaji.TJ
                                    objPatguarantor.Human_ID = Convert.ToInt32(txtAccountNo.Text);
                                objPatguarantor.Active = "YES";
                                objPatguarantor.Modified_By = ClientSession.UserName;
                                if (hdnLocalTime.Value != string.Empty)
                                {
                                    objPatguarantor.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                                    objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                                }
                                objPatguarantor.Guarantor_Human_ID = iUpdateGuarantorID;
                                objPatguarantor.Relationship = ddlGuarantorRelationship.SelectedItem.Text;
                                if (ddlGuarantorRelationship.SelectedItem.Text != string.Empty && ddlGuarantorRelationship.SelectedIndex != -1)
                                {
                                    string temp = ddlGuarantorRelationship.Items[ddlGuarantorRelationship.SelectedIndex].Value;
                                    string[] Relation = temp.Split('-');
                                    if (Relation.Length == 2)
                                    {
                                        objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);
                                    }
                                }
                            }
                            else
                            {
                                objPatguarantor = new PatGuarantor();
                                if (txtAccountNo.Text != string.Empty) //code added by balaji.TJ
                                    objPatguarantor.Human_ID = Convert.ToInt32(txtAccountNo.Text);
                                objPatguarantor.Active = "YES";
                                objPatguarantor.Created_By = ClientSession.UserName;
                                if (hdnLocalTime.Value != string.Empty)
                                {
                                    objPatguarantor.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                                    objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                                }
                                objPatguarantor.Guarantor_Human_ID = iUpdateGuarantorID;
                                objPatguarantor.Relationship = ddlGuarantorRelationship.SelectedItem.Text;
                                if (ddlGuarantorRelationship.SelectedItem.Text != string.Empty && ddlGuarantorRelationship.SelectedIndex != -1)
                                {
                                    string temp = ddlGuarantorRelationship.Items[ddlGuarantorRelationship.SelectedIndex].Value;
                                    string[] Relation = temp.Split('-');
                                    if (Relation.Length == 2)
                                    {
                                        objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);

                                    }

                                }
                            }
                        }

                        string sFTPPath = UploadPhoto();

                        if (sFTPPath != string.Empty)
                            objHuman.Photo_Path = sFTPPath;

                        string sPriCarrier = string.Empty;
                        //if (txtPrimaryInsCarrierName.Text != string.Empty && txtPrimaryInsCarrierName.Text.Trim() != "")
                        //{
                        //    sPriCarrier = txtPrimaryInsCarrierName.Text;
                        //}
                        objHumanDTO.HumanDetails = HumanMngr.UpdateBatchToHuman(objHuman, objPatguarantor, objPatInsPLan, sPriCarrier);

                        // objHumanDTO.HumanDetails = HumanMngr.UpdateBatchToHuman(objHuman, objPatguarantor, objPatInsPLan,string.Empty);
                        if (objHumanDTO.HumanDetails != null && objHumanDTO.HumanDetails.Id != 0)
                        {
                            saveSuccess = true;
                            huId = objHumanDTO.HumanDetails.Id;
                        }
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('380048');", true);
                        //RefreshGuarantor();
                    }
                    // Creating New Account
                    else
                    {
                        if (bValidation == false)
                        {
                            goto l;
                        }
                        objHuman = new Human();
                        SavePatientDetails();
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objHuman.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        objHuman.Created_By = ClientSession.UserName;
                        //objHuman.Birth_Date = dtpPatientDOB.SelectedDate.Value;
                        objHuman.Birth_Date = Convert.ToDateTime(dtpPatientDOB.Text);


                        if (chkGuarantorIsPatient.Checked == false)
                        {

                            if (dtpGuarantorDOB.Text == "")
                            {
                                objHuman.Guarantor_Birth_Date = Convert.ToDateTime("1/1/1900");
                            }

                            else if (Dobvalidation(Convert.ToDateTime(dtpGuarantorDOB.Text)) == true)
                            {
                                objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpGuarantorDOB.Text);
                            }
                            else
                            {
                                //ApplicationObject.erroHandler.DisplayErrorMessage("420033", "Patient Demographics", this.Page);
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('420033');", true);
                                dtpGuarantorDOB.Focus();
                                goto l;
                            }
                            ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                        }
                        else
                        {
                            //objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpGuarantorDOB.SelectedDate.Value);
                            objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpPatientDOB.Text);
                            ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                        }

                        //if (dtpEmerDOB.SelectedDate != null)
                        if (dtpEmerDOB.Text != "")
                        {
                            //if (Dobvalidation(Convert.ToDateTime(dtpEmerDOB.SelectedDate.Value)) == true)
                            if (Dobvalidation(Convert.ToDateTime(dtpEmerDOB.Text)) == true)
                            {
                                if (dtpEmerDOB.Enabled == true)
                                {
                                    //objHuman.Emergency_BirthDate = dtpEmerDOB.SelectedDate.Value;
                                    objHuman.Emergency_BirthDate = Convert.ToDateTime(dtpEmerDOB.Text);
                                }
                            }
                            else
                            {
                                // ApplicationObject.erroHandler.DisplayErrorMessage("420034", "Patient Demographics", this.Page);
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('420034');", true);
                                dtpEmerDOB.Focus();
                                goto l;
                            }
                        }
                        string sAccountExtNo = string.Empty;

                        if (txtExternalAccNo.Text != "999999999999999")
                        {
                            sAccountExtNo = txtExternalAccNo.Text;
                        }

                        HumanDTO CheckHuman = new HumanDTO();
                        //Cap - 1883
                        //CheckHuman = HumanMngr.GetPatientDetailsUsingPatientDetails(txtPatientlastname.Text, txtPatientfirstname.Text, Convert.ToDateTime(dtpPatientDOB.Text), ddlPatientsex.Text, txtMedicalRecordno.Text, sAccountExtNo);
                        CheckHuman = HumanMngr.GetPatientDetailsUsingPatientDetails(txtPatientlastname.Text, txtPatientfirstname.Text, Convert.ToDateTime(dtpPatientDOB.Text), ddlPatientsex.Text, txtMedicalRecordno.Text, sAccountExtNo, ClientSession.LegalOrg);
                        int iSave = 6;

                        if (CheckHuman.HumanDetails != null)
                        {
                            //iSave = ApplicationObject.erroHandler.DisplayErrorMessage("420032", "Patient Demographics", this.Page);
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('420032');", true);
                        }

                        if (iSave == 2)
                        {
                            txtPatientlastname.Focus();
                            goto l;
                        }
                        if (CheckHuman.MedicalRecordNoList == true)
                        {
                            // ApplicationObject.erroHandler.DisplayErrorMessage("420044", "Patient Demographics", this.Page);
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('420044');", true);
                            txtMedicalRecordno.Focus();
                            goto l;

                        }

                        if (CheckHuman.Patient_Account_External == true)
                        {
                            //ApplicationObject.erroHandler.DisplayErrorMessage("420046", "Patient Demographics", this.Page);
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('420046');", true);
                            txtExternalAccNo.Focus();
                            goto l;
                        }
                        PatGuarantor objPatguarantor = null;

                        int iGuarantorID = 0;

                        if (chkGuarantorIsPatient.Checked != true)
                        {
                            if (hdnGuarantorID.Value != string.Empty)
                            {
                                iGuarantorID = Convert.ToInt32(hdnGuarantorID.Value);

                                if (iGuarantorID != 0)
                                {
                                    objPatguarantor = new PatGuarantor();
                                    objPatguarantor.Active = "YES";
                                    objPatguarantor.Created_By = ClientSession.UserName;
                                    if (hdnLocalTime.Value != string.Empty)
                                    {
                                        objPatguarantor.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);

                                        objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                                    }
                                    objPatguarantor.Guarantor_Human_ID = iGuarantorID;
                                    objPatguarantor.Relationship = ddlGuarantorRelationship.SelectedItem.Text;
                                    if (ddlGuarantorRelationship.SelectedItem.Text != string.Empty && ddlGuarantorRelationship.SelectedIndex != -1)
                                    {
                                        string temp = ddlGuarantorRelationship.Items[ddlGuarantorRelationship.SelectedIndex].Value;
                                        string[] Relation = temp.Split('-');
                                        if (Relation.Length == 2)
                                        {
                                            objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);

                                        }


                                    }
                                }
                            }
                            ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                        }
                        else if (chkGuarantorIsPatient.Checked == true)
                        {

                            objPatguarantor = new PatGuarantor();
                            objPatguarantor.Active = "YES";
                            objPatguarantor.Created_By = ClientSession.UserName;
                            if (hdnLocalTime.Value != string.Empty)
                            {
                                objPatguarantor.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);

                                objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                            }
                            objPatguarantor.Guarantor_Human_ID = iGuarantorID;
                            objPatguarantor.Relationship = ddlGuarantorRelationship.SelectedItem.Text;
                            if (ddlGuarantorRelationship.SelectedItem.Text != string.Empty && ddlGuarantorRelationship.SelectedIndex != -1)
                            {
                                string temp = ddlGuarantorRelationship.Items[ddlGuarantorRelationship.SelectedIndex].Value;
                                string[] Relation = temp.Split('-');
                                if (Relation.Length == 2)
                                {
                                    objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);

                                }


                            }
                            ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                        }



                        Human humanObj = null;

                        string sFTPPath = UploadPhoto();

                        if (sFTPPath != string.Empty)
                            objHuman.Photo_Path = sFTPPath;


                        string sCarrier = string.Empty;
                        //if (txtPrimaryInsCarrierName.Text != string.Empty && txtPrimaryInsCarrierName.Text.Trim() != "")
                        //{
                        //    sCarrier = txtPrimaryInsCarrierName.Text;
                        //}


                        humanObj = HumanMngr.AppendBatchToHuman(objHuman, objPatguarantor, sCarrier);
                        IList<Human> ilstHuman = new List<Human>();
                        ilstHuman.Add(objHuman);
                        if (humanObj.Id != 0)
                        {
                            #region commented By Deepak
                            //string HumanFileName = "Human" + "_" + humanObj.Id + ".xml";
                            //string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
                            //if (File.Exists(strXmlHumanFilePath) == false && humanObj.Id > 0)
                            //{
                            //    string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                            //    string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                            //    XmlDocument itemDoc = new XmlDocument();
                            //    XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                            //    itemDoc.Load(XmlText);
                            //    XmlNodeList xmlnode = itemDoc.GetElementsByTagName("EncounterDetails");
                            //    xmlnode[0].ParentNode.RemoveChild(xmlnode[0]);

                            //    int iAge = 0;
                            //    iAge = UtilityManager.CalculateAge(humanObj.Birth_Date);

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
                            #endregion
                            UpdateAgeinBlob(humanObj.Id, humanObj.Birth_Date);
                        }

                        if (humanObj != null)
                        {
                            objHumanId = humanObj.Id;

                            hdnHumanId.Value = objHumanId.ToString();
                        }
                        ulPatientID = objHumanId;
                        //  Session["ulPatientID"] = ulPatientID;
                        hdnPatientID.Value = ulPatientID.ToString();
                        txtAccountNo.Text = objHumanId.ToString();
                        hdnDemoAccNumber.Value = objHumanId.ToString();
                        //Added by Srividhya
                        Session["HumanID"] = txtAccountNo.Text;
                        //ApplicationObject.erroHandler.DisplayErrorMessage("420018", "Patient Demographics", this.Page);
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "DisplayErrorMessage('380048');", true);
                        DisablePatientDetails();
                        RefreshGuarantor();
                        //  objHumanDTO.HumanDetails = HumanMngr.GetPatientDetailsUsingPatientInformattion(objHumanId)[0];

                        bSaveCheck = true;
                        hdnSaveFlag.Value = "false";
                        bFormCloseCheck = false;
                        btnEditName.Enabled = true;
                        btnSave.Enabled = false;
                        if (hdnbInsuredHuman.Value.ToUpper() == "FALSE")
                        {
                            //frmAddInsurancePolicies.ulInsuredHumanId = objHumanId; need to add once modaal window completed
                            //this.Close();
                        }
                        else
                        {
                            //btnEditName.Enabled = true;
                            // btnViewUpdateInsurance.Enabled = true;
                            //DisableTableLayout(pnlPatientInfo);
                            //DisableGroupbox();
                        }

                    l:
                        if (bSaveCheck != true)
                        {
                            btnSave.Enabled = true;
                            btnSendEmail.Enabled = true;
                        }
                    }

                }
            }



        }
        public void DateTimePickerColorChange(RadDatePicker datetimepicker)
        {
            datetimepicker.Enabled = false;
        }

        protected void txtCity_TextChanged(object sender, EventArgs e)
        {
            bFormCloseCheck = true;
            if (chkGuarantorIsPatient.Checked == true && chkGuarantorIsPatient.Enabled == true)
            {
                txtGuarantorCity.Text = txtCity.Text;
                ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
            }
            btnSave.Enabled = true;
        }

        protected void btnEditName_Click(object sender, EventArgs e)
        {
            bEditNameValid = true;
            //btnSave.Enabled = true;
            TextBoxColorChangeWhite(txtPatientlastname);
            TextBoxColorChangeWhite(txtPatientfirstname);
            TextBoxColorChangeWhite(txtPatientmiddlename);
            txtPatientlastname.Focus();
            ComboBoxColorChangeWhite(ddlPatientsex);
            ComboBoxColorChangeWhite(ddlSuffix);
            //cap-669
            //dtpPatientDOB.Enabled = true;
            dtpPatientDOB.Attributes.Remove("disabled");
            dtpPatientDOB.Attributes.Add("enabled", "enabled");
            dtpPatientDOB.CssClass = "Editabletxtbox";

            if (ddlPatientStatus.Text.ToUpper() != "DECEASED")
            {
                dtpDateOfDeath.Enabled = false;
            }
            //Cap - 1529
            else
            {
                dtpDateOfDeath.Enabled = true;
            }
            //DateTimePickerColorChangeWhite(dtpPatientDOB);
            btnEditName.Enabled = false;
        }
        public void DateTimePickerColorChangeWhite(RadDatePicker datetimepicker)
        {

            datetimepicker.Enabled = true;
        }

        protected void FindPCPProvider_Click(object sender, EventArgs e)
        {
            //  MessageBox.Open("frmfindreferralphysician.aspx");
        }

        protected void ddlPatientStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (chkGuarantorIsPatient.Checked)
            {
                txtGuarantorAddressLine2.Text = txtPatientAddressLine2.Text;
                txtGuarantorAddress.Text = txtPatientAddress.Text;
                txtGuarantorCity.Text = txtCity.Text;

                txtGuarantorLastName.Text = txtPatientlastname.Text;
                txtGuarantorFirstName.Text = txtPatientfirstname.Text;
                ddlGuarantorSex.SelectedItem.Text = ddlPatientsex.SelectedItem.Text;
                hdnGuarantorSex.Value = ddlPatientsex.SelectedItem.Text;

                msktxtGuarantorHomeNo.Text = msktxtHomePhno.Text;
                msktxtGuarantorCellNo.Text = msktxtCellPhno.Text;
                txtGuaEmail.Text = txtEmail.Text;

                txtGuarantorMiddleName.Text = txtPatientmiddlename.Text;
                ddlGuarantorState.SelectedItem.Text = ddlState.SelectedItem.Text;
                hdnGuarantorState.Value = ddlState.SelectedItem.Text;
                ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
            }


            if (ddlPatientStatus.Text.ToUpper() == "ALIVE")
            {
                if (hdncancel.Value == "")
                {
                    ddlPatientStatus.SelectedIndex = 0;
                    //DateTimePickerColorChange(dtpDateOfDeath);
                    dtpDateOfDeath.Enabled = false;
                    dtpDateOfDeath.Text = string.Empty;
                    ComboBoxColorChange(ddlReasonForDeath, false);
                    MaskedTextBoxColorChange(dtpDateOfDeath);
                    //dtpDateOfDeath.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                    lblDateOfDeath.ForeColor = Color.Black;
                    lblReasonForDeath.ForeColor = Color.Black;
                }
                else
                {
                    ddlPatientStatus.SelectedIndex = 1;
                    ComboBoxColorChange(ddlReasonForDeath, true);
                    dtpDateOfDeath.Enabled = true;
                    dtpDateOfDeath.BackColor = System.Drawing.Color.White;
                    MaskedTextBoxColorChangewhite(dtpDateOfDeath);
                    lblDateOfDeath.Text = "Date of Death*";
                    lblReasonForDeath.Text = "Reason For Death*";
                    hdncancel.Value = "";

                }
            }
            else if (ddlPatientStatus.Text.ToUpper() == "DECEASED")
            {

                if (hdncancel.Value == "")
                {
                    ddlPatientStatus.SelectedIndex = 1;
                    ComboBoxColorChange(ddlReasonForDeath, true);
                    dtpDateOfDeath.Enabled = true;
                    dtpDateOfDeath.BackColor = System.Drawing.Color.White;
                    //Cap - 1529
                    dtpDateOfDeath.Attributes.Remove("readonly"); 
                    MaskedTextBoxColorChangewhite(dtpDateOfDeath);
                    lblDateOfDeath.Text = "Date of Death*";
                    lblReasonForDeath.Text = "Reason For Death*";
                }
                else
                {
                    ddlPatientStatus.SelectedIndex = 0;
                    dtpDateOfDeath.Enabled = false;
                    dtpDateOfDeath.Text = string.Empty;
                    MaskedTextBoxColorChange(dtpDateOfDeath);
                    ComboBoxColorChange(ddlReasonForDeath, false);
                    lblDateOfDeath.ForeColor = Color.Black;
                    lblReasonForDeath.ForeColor = Color.Black;
                    hdncancel.Value = "";

                }
            }

            ClientScript.RegisterStartupScript(typeof(Page), "Demographics", "changelabel();", true);
            bFormCloseCheck = true;
            btnSave.Enabled = true;


        }

        protected void btnAddGuarantor_Click(object sender, EventArgs e)
        {
            RefreshAddGuarantor();

        }

        protected void btnSelectGaurantor_Click(object sender, EventArgs e)
        {
            IList<Human> humanlist = new List<Human>();
            if (hdnGuarantorID.Value != string.Empty)
            {
                humanlist = HumanMngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(hdnGuarantorID.Value));
                if (humanlist.Count > 0 && humanlist != null)
                {
                    txtGuarantorFirstName.Text = humanlist[0].First_Name;
                    txtGuarantorCity.Text = humanlist[0].City;
                    txtGuarantorLastName.Text = humanlist[0].Last_Name;
                    txtGuarantorMiddleName.Text = humanlist[0].MI;
                    //txtGuarantorAddress.Text = humanlist[0].Street_Address1 + humanlist[0].Street_Address2;
                    txtGuarantorAddress.Text = humanlist[0].Street_Address1;
                    txtGuarantorAddressLine2.Text = humanlist[0].Street_Address2;
                    for (int i = 0; i < ddlGuarantorSex.Items.Count; i++)
                    {
                        if (Convert.ToString(ddlGuarantorSex.Items[i].Value).ToUpper() == humanlist[0].Sex.ToUpper())
                        {
                            ddlGuarantorSex.SelectedIndex = i;
                            hdnGuarantorSex.Value = humanlist[0].Sex;
                        }
                    }
                    for (int i = 0; i < ddlGuarantorState.Items.Count; i++)
                    {
                        if (Convert.ToString(ddlGuarantorState.Items[i].Value).ToUpper() == humanlist[0].State.ToUpper())
                        {
                            ddlGuarantorState.SelectedIndex = i;
                            hdnGuarantorState.Value = humanlist[0].State;
                        }
                    }
                    msktxtGuarantorCellNo.Text = humanlist[0].Cell_Phone_Number;
                    msktxtGuarantorHomeNo.Text = humanlist[0].Home_Phone_No;
                    msktxtGuarantorZipCode.Text = humanlist[0].Guarantor_Zip_Code;
                    //dtpGuarantorDOB.SelectedDate = humanlist[0].Birth_Date;//.ToString("dd-MMM-yyyy");
                    dtpGuarantorDOB.Text = humanlist[0].Birth_Date.ToString("dd-MMM-yyyy");

                }
            }
        }

        private void ChangeLabelStyle(Label lbl, Boolean bIstoNormal)
        {
            if (bIstoNormal == true)
            {
                lbl.Text = lbl.Text.Replace("*", "");
                lbl.ForeColor = Color.Black;
            }
            else
            {
                if (lbl.Text.Contains("*") == false) lbl.Text = lbl.Text + "*";
                lbl.ForeColor = Color.Red;
            }
        }

        protected void hdnBtnLoadInsurance_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            ulong ulHumanID = 0;
            if (hdnPatientID.Value != string.Empty)
            {
                ulHumanID = Convert.ToUInt64(hdnPatientID.Value);
            }
            //txtPrimaryInsPlanName.Text = string.Empty;
            //txtPrimaryInsCarrierName.Text = string.Empty;
            //txtPriInsuredName.Text = string.Empty;
            //txtSecInsPlanName.Text = string.Empty;
            //txtPriRelation.Text = string.Empty;
            //txtPrimaryInsEvStatus.Text = string.Empty;
            //txtSecInsCarrierName.Text = string.Empty;
            //txtSecInsEVStatus.Text = string.Empty;
            //txtPriInsID.Text = string.Empty;
            //txtSecInsuredID.Text = string.Empty;
            HumanDTO objRefreshHumanDTO = HumanMngr.GetHumanInuranceAndCarrierDetailsRCM(ulHumanID, string.Empty, string.Empty);
            // txtPrimaryInsPlanName.Text = objRefreshHumanDTO.InsuranceDetails.Ins_Plan_Name;
            hdnPrimInsPlanID.Value = objRefreshHumanDTO.InsuranceDetails.Id.ToString();

            Carrier objCarrier = CarrierMngr.GetCarrierUsingId(Convert.ToUInt64(objRefreshHumanDTO.InsuranceDetails.Carrier_ID));
            //txtPrimaryInsCarrierName.Text = objCarrier.Carrier_Name;
            //txtSecInsPlanName.Text = objRefreshHumanDTO.SecInsuranceDetails.Ins_Plan_Name;
            hdnSecInsPlanID.Value = objRefreshHumanDTO.SecInsuranceDetails.Id.ToString();

            Carrier objsecCarrier = CarrierMngr.GetCarrierUsingId(Convert.ToUInt64(objRefreshHumanDTO.SecInsuranceDetails.Carrier_ID));
            //txtSecInsCarrierName.Text = objsecCarrier.Carrier_Name;
            //txtPrimaryInsEvStatus.Text = objRefreshHumanDTO.Primary_EligiblityStatus;
            //txtSecInsEVStatus.Text = objRefreshHumanDTO.Secondry_EligiblityStatus;
            txtRecentScannedStatus.Text = objRefreshHumanDTO.InsuranceCopy + "\n" + objRefreshHumanDTO.PatientInfo;
            txtRecentVerificationStatus.Text = objRefreshHumanDTO.EligiblityVerified;
            if (objRefreshHumanDTO.HumanDetails.PatientInsuredBag != null && objRefreshHumanDTO.HumanDetails.PatientInsuredBag.Count > 0)
            {
                txtNoofPolicies.Text = objRefreshHumanDTO.HumanDetails.PatientInsuredBag.Count.ToString();

                for (int i = 0; i < objRefreshHumanDTO.HumanDetails.PatientInsuredBag.Count; i++)
                {
                    //txtPriInsID.Text = objRefreshHumanDTO.sPriInsuredID;
                    //txtPriRelation.Text = objRefreshHumanDTO.sPriRelation;
                    //txtPriInsuredName.Text = objRefreshHumanDTO.sPriInsuredName;
                    //txtsecinsuredname.Text = objRefreshHumanDTO.sSecondaryInsuranceName;
                    //txtSecRelation.Text = objRefreshHumanDTO.sSecondaryInsuranceRelation;
                    //txtSecInsuredID.Text = objRefreshHumanDTO.HumanDetails.PatientInsuredBag[i].Insured_Human_ID.ToString();
                    if (objRefreshHumanDTO.HumanDetails.PatientInsuredBag[i].Insurance_Type.ToUpper() == "SECONDARY" && objRefreshHumanDTO.HumanDetails.PatientInsuredBag[i].Active.ToUpper() == "YES")
                    {
                        //Added by thamizh to show secondary insuard name and realation
                        //txtsecinsuredname.Text = objRefreshHumanDTO.sSecondaryInsuranceName;
                        //txtSecRelation.Text = objRefreshHumanDTO.sSecondaryInsuranceRelation;

                        //txtSecInsuredID.Text = objRefreshHumanDTO.HumanDetails.PatientInsuredBag[i].Insured_Human_ID.ToString();
                    }


                    if (objRefreshHumanDTO.HumanDetails.PatientInsuredBag[i].Active.ToUpper() == "YES" && objRefreshHumanDTO.HumanDetails.PatientInsuredBag[i].Insurance_Type.ToUpper() == "PRIMARY")
                    {
                        if (objRefreshHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_ID != 0)
                        {
                            IList<PhysicianLibrary> phylist = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(objRefreshHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_ID));
                            if (phylist != null && phylist.Count > 0)
                            {
                                PhysicianLibrary objPhyLib = phylist[0];
                                if (objPhyLib != null)
                                {
                                    string sPhyName = objPhyLib.PhyPrefix + " " + objPhyLib.PhyFirstName + " " + objPhyLib.PhyMiddleName + " " + objPhyLib.PhyLastName + " " + objPhyLib.PhySuffix;
                                    //txtPCPProvider.Text = sPhyName;
                                    //txtNPI.Text = objPhyLib.PhyNPI.ToString();
                                    txtPCPProviderTag.Value = objRefreshHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_ID.ToString();
                                    break;
                                }
                            }
                        }
                        else if (objRefreshHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_Name != string.Empty && objRefreshHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_NPI != string.Empty)
                        {
                            //txtPCPProvider.Text = objRefreshHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_Name;
                            //txtNPI.Text = objRefreshHumanDTO.HumanDetails.PatientInsuredBag[i].PCP_NPI;
                            break;
                        }
                    }


                    else if (objRefreshHumanDTO.HumanDetails.Encounter_Provider_ID != 0)
                    {
                        IList<PhysicianLibrary> phylist = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(objRefreshHumanDTO.HumanDetails.Encounter_Provider_ID));
                        if (phylist != null && phylist.Count > 0)
                        {
                            PhysicianLibrary objPhyLib = phylist[0];

                            if (objPhyLib != null)
                            {
                                string sPhyName = objPhyLib.PhyPrefix + " " + objPhyLib.PhyFirstName + " " + objPhyLib.PhyMiddleName + " " + objPhyLib.PhyLastName + " " + objPhyLib.PhySuffix;
                                //txtPCPProvider.Text = sPhyName;
                                //txtNPI.Text = objPhyLib.PhyNPI.ToString();
                                txtPCPProviderTag.Value = objRefreshHumanDTO.HumanDetails.Encounter_Provider_ID.ToString();
                            }
                        }
                    }
                }
            }

            UpdateGuarantorDetails(objRefreshHumanDTO);
            //CAP-1750
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "showVal", " sessionStorage.removeItem('ActiveAnyGuarantor');", true);
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "showVal", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnSaveAndNext_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);

        }
        public void UpdateGuarantorDetails(HumanDTO objHumanDTO)
        {
            txtGuarantorLastName.Text = objHumanDTO.HumanDetails.Guarantor_Last_Name;
            txtGuarantorFirstName.Text = objHumanDTO.HumanDetails.Guarantor_First_Name;
            txtGuarantorMiddleName.Text = objHumanDTO.HumanDetails.Guarantor_MI;
            for (int i = 0; i < ddlGuarantorSex.Items.Count; i++)
            {
                if (Convert.ToString(ddlGuarantorSex.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Guarantor_Sex.ToUpper())
                {
                    ddlGuarantorSex.SelectedIndex = i;
                    hdnGuarantorSex.Value = ddlGuarantorSex.Items[i].Text;
                    break;
                }
            }
            // ddlGuarantorSex.SelectedItem.Text = objHumanDTO.HumanDetails.Guarantor_Sex;
            if ((objHumanDTO.HumanDetails.Guarantor_Birth_Date == Convert.ToDateTime("1/1/0001")) || (objHumanDTO.HumanDetails.Guarantor_Birth_Date == Convert.ToDateTime("1/1/1900")))
            {
                //dtpGuarantorDOB.SelectedDate = null;// string.Empty;
                dtpGuarantorDOB.Text = string.Empty;

            }
            else
            {

                dtpGuarantorDOB.Text = objHumanDTO.HumanDetails.Guarantor_Birth_Date.ToString("dd-MMM-yyyy");
            }

            txtGuarantorAddress.Text = objHumanDTO.HumanDetails.Guarantor_Street_Address1;
            txtGuarantorAddressLine2.Text = objHumanDTO.HumanDetails.Guarantor_Street_Address2;
            txtGuarantorCity.Text = objHumanDTO.HumanDetails.Guarantor_City;
            if (objHumanDTO.HumanDetails.Guarantor_Zip_Code.Length == 4)
            {
                msktxtGuarantorZipCode.Text = objHumanDTO.HumanDetails.Guarantor_Zip_Code;
                msktxtGuarantorZipCode.Text.Insert(5, "-");
            }
            for (int i = 0; i < ddlGuarantorSex.Items.Count; i++)
            {
                if (Convert.ToString(ddlGuarantorSex.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Guarantor_Sex.ToUpper())
                {
                    ddlGuarantorSex.SelectedIndex = i;
                    hdnGuarantorSex.Value = objHumanDTO.HumanDetails.Guarantor_Sex;
                    break;
                }
            }
            //   ddlGuarantorSex.SelectedItem.Text = objHumanDTO.HumanDetails.Guarantor_Sex;
            for (int i = 0; i < ddlGuarantorState.Items.Count; i++)
            {
                if (Convert.ToString(ddlGuarantorState.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Guarantor_State.ToUpper())
                {
                    ddlGuarantorState.SelectedIndex = i;
                    hdnGuarantorState.Value = objHumanDTO.HumanDetails.Guarantor_State;
                    break;
                }
            }
            for (int i = 0; i < ddlGuarantorRelationship.Items.Count; i++)
            {
                if (Convert.ToString(ddlGuarantorRelationship.Items[i].Text).ToUpper() == objHumanDTO.HumanDetails.Guarantor_Relationship.ToUpper())
                {
                    ddlGuarantorRelationship.SelectedIndex = i;
                    break;
                }
            }
            // ddlGuarantorState.SelectedItem.Text = objHumanDTO.HumanDetails.Guarantor_State;
            if (objHumanDTO.HumanDetails.Guarantor_Is_Patient.ToUpper() == "Y")
            {
                chkGuarantorIsPatient.Checked = true;
            }
            else
            {
                chkGuarantorIsPatient.Checked = false;
            }
            if (chkGuarantorIsPatient.Checked == true)
            {
                btnAddGuarantor.Enabled = false;
                btnSelectGaurantor.Enabled = false;
                ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
            }
            else
            {
                btnAddGuarantor.Enabled = true;
                btnSelectGaurantor.Enabled = true;
                ddlGuarantorRelationship.CssClass = "Editabletxtbox";
            }
            msktxtGuarantorCellNo.Text = objHumanDTO.HumanDetails.Guarantor_CellPhone_Number;
            msktxtGuarantorHomeNo.Text = objHumanDTO.HumanDetails.Guarantor_Home_Phone_Number;
            msktxtGuarantorZipCode.Text = objHumanDTO.HumanDetails.Guarantor_Zip_Code;
        }
        public void ShownDemographics()
        {
            txtNotes.DName = "pbDropDown";

            //SecurityServiceUtility objSecurityServiceUtility = new SecurityServiceUtility();
            //objSecurityServiceUtility.ApplyUserPermissions(this);
            btnSave.Enabled = false;
            ddlGuarantorSex.Enabled = false;
            ddlGuarantorState.Enabled = false;
            ddlGuarantorRelationship.Enabled = false;

            if (ddlPatientStatus.Text.ToUpper() == "ALIVE")
            {
                //DateTimePickerColorChange(dtpDateOfDeath);
                dtpDateOfDeath.Enabled = false;
                dtpDateOfDeath.Text = string.Empty;
                ComboBoxColorChange(ddlReasonForDeath, false);
                //dtpDateOfDeath.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);


            }
            else if (ddlPatientStatus.Text.ToUpper() == "DECEASED")
            {

                ComboBoxColorChange(ddlReasonForDeath, true);
                //DateTimePickerColorChangeWhite(dtpDateOfDeath);
                dtpDateOfDeath.Enabled = true;
                //Cap - 1529
                dtpDateOfDeath.Attributes.Remove("readonly"); 
                //dtpDateOfDeath.BackColor = System.Drawing.Color.White;
                lblDateOfDeath.ForeColor = Color.Red;
                lblReasonForDeath.ForeColor = Color.Red;


            }
            string sIndicator = string.Empty;
            if (ddlBirthOrder.SelectedItem.Text == "1")
            {
                ddlBirthOrder.Enabled = false;
            }
            else
            {
                if (txtMothersMaidenName.Enabled == true)
                {
                    ddlBirthOrder.Enabled = true;
                }
            }

            if (hdnPatientID.Value == "0")
            {
                //DisableGroupbox();
                //Cap - 669
                //ddlPatientsex.Enabled = true;
                //ddlSuffix.Enabled = true;
                ddlPatientsex.Attributes.Remove("disabled");
                ddlSuffix.Attributes.Remove("disabled");
                ddlPatientsex.Attributes.Add("enabled", "enabled");
                ddlSuffix.Attributes.Add("enabled", "enabled");

                if (chkGuarantorIsPatient.Checked == true)
                {
                    ddlGuarantorSex.Enabled = false;
                    //btnEditName.Enabled = false;
                    ddlGuarantorState.Enabled = false;
                    ddlGuarantorRelationship.Enabled = false;
                    btnAddGuarantor.Enabled = false;
                    btnSelectGaurantor.Enabled = false;
                    ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                }
                else
                {
                    ddlGuarantorSex.Enabled = true;
                    btnEditName.Enabled = true;
                    ddlGuarantorState.Enabled = true;
                    ddlGuarantorRelationship.Enabled = true;
                    btnAddGuarantor.Enabled = true;
                    btnSelectGaurantor.Enabled = true;
                    btnViewGaurantor.Enabled = true;
                    ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                }
                btnEditName.Enabled = false;
                // btnViewUpdateInsurance.Enabled = false;
            }
            else
            {
                //Cap - 669
                //ddlPatientsex.Enabled = false;
                //ddlSuffix.Enabled = false;
                //dtpPatientDOB.Enabled = false;
                ddlPatientsex.Attributes.Add("disabled", "disabled");
                ddlSuffix.Attributes.Add("disabled", "disabled");
                dtpPatientDOB.Attributes.Add("disabled", "disabled");

                if (chkGuarantorIsPatient.Checked == true)
                {
                    ddlGuarantorSex.Enabled = false;
                    // btnEditName.Enabled = true;
                    ddlGuarantorState.Enabled = false;
                    ddlGuarantorRelationship.Enabled = false;
                    btnAddGuarantor.Enabled = false;
                    btnSelectGaurantor.Enabled = false;
                    ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                }
                else
                {
                    //ddlGuarantorSex.Enabled = true;
                    btnEditName.Enabled = true;
                    // ddlGuarantorState.Enabled = true;
                    ddlGuarantorRelationship.Enabled = true;
                    btnAddGuarantor.Enabled = true;
                    btnSelectGaurantor.Enabled = true;
                    btnViewGaurantor.Enabled = true;
                    ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                }

            }
            if (hdnbInsuredHuman.Value.ToUpper() == "FALSE")
            {
                DisableGroupbox();
            }
            dtpGuarantorDOB.Enabled = false;

        }

        void GetPhysicianId()
        {
            IList<MapFacilityPhysician> patientlst = new List<MapFacilityPhysician>();

            // patientlst = PatientNotesMngr.GetPhysicianId(ClientSession.FacilityName);
            if (patientlst != null && patientlst.Count > 0)
            {
                Session["PhysicianID"] = patientlst[0].Phy_Rec_ID;
                hdnPhysicianID.Value = Session["PhysicianID"].ToString();
            }


        }



        void AddDetails()
        {
            string Type = string.Empty;
            IList<PatientNotes> patientlst = new List<PatientNotes>();
            PatientNotes objpatientnotes = new PatientNotes();
            if ((txtNotes.txtDLC.Text == string.Empty))
            {
                //CAP-726 - add loader
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "DisplayErrorMessage('7580009'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            if (ddlMessageDescription.SelectedIndex == 0)
            {
                //CAP-726 - add loader
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "DisplayErrorMessage('7580008'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;

            }
            else
            {
                if (ddlAssignedTo.SelectedIndex == 0)
                {
                    if (txtAccountNo.Text != string.Empty)
                        objpatientnotes.Human_ID = Convert.ToUInt64(txtAccountNo.Text);
                    objpatientnotes.Assigned_To = string.Empty;
                    objpatientnotes.Relationship = string.Empty;
                    objpatientnotes.Facility_Name = string.Empty;
                    objpatientnotes.Caller_Name = string.Empty;
                    objpatientnotes.Message_Orign = string.Empty;

                    objpatientnotes.Message_Description = ddlMessageDescription.SelectedItem.Text;

                    objpatientnotes.Notes = "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(Convert.ToDateTime(hdnLocalTime.Value)).ToString("dd-MMM-yyyy hh:mm tt") + "): " + txtNotes.txtDLC.Text;
                    objpatientnotes.Created_By = ClientSession.UserName;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objpatientnotes.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);// DateTime.Now;
                    }
                    objpatientnotes.Message_Date_And_Time = UtilityManager.ConvertToUniversal();

                    objpatientnotes.Modified_By = string.Empty;
                    objpatientnotes.Version = 1;
                    objpatientnotes.Is_PatientChart = "N";
                    objpatientnotes.Line_ID = 0;
                    objpatientnotes.Type = "MESSAGE";

                    //srividhya
                    if (hdnEncounterID.Value != "")
                    {
                        objpatientnotes.Encounter_ID = Convert.ToInt32(hdnEncounterID.Value);
                    }
                    objpatientnotes.Statement_ChargeLine_ID = 0;
                    if (txtAccountNo.Text != string.Empty) //code added by balaji.TJ 
                        objpatientnotes.SourceID = Convert.ToInt32(txtAccountNo.Text);
                    objpatientnotes.Source = "PATIENT";
                    objpatientnotes.IsDelete = "N";
                    objpatientnotes.Is_PopupEnable = "N";
                    objpatientnotes.Priority = "NORMAL";
                    patientlst.Add(objpatientnotes);
                    IList<PatientNotes> patientdetail = PatientNotesMngr.AddToPatientNotes(patientlst);
                    btnAddMessage.Enabled = false;
                    ddlMessageDescription.SelectedIndex = -1;
                    txtNotes.txtDLC.Text = string.Empty;
                    //CAP-726 - add loader
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "DisplayErrorMessage('420082'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    divLoading.Style.Add("display", "none !important");
                }
                else
                {
                    PatientNotesManager patientnotesmngr = new PatientNotesManager();
                    if (txtAccountNo.Text != string.Empty)
                    {
                        objpatientnotes.Human_ID = Convert.ToUInt64(txtAccountNo.Text);
                    }
                    //CAP-1091
                    objpatientnotes.Assigned_To = ddlAssignedTo.SelectedItem.Value; // ddlAssignedTo.SelectedItem.Text;
                    objpatientnotes.Facility_Name = ClientSession.FacilityName;
                    objpatientnotes.Message_Orign = "Demographics";
                    objpatientnotes.Notes = "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(Convert.ToDateTime(hdnLocalTime.Value)).ToString("dd-MMM-yyyy hh:mm tt") + "): " + txtNotes.txtDLC.Text;
                    objpatientnotes.Created_By = ClientSession.UserName;
                    objpatientnotes.Type = "TASK";
                    //objpatientnotes.Is_PatientChart = PatientChart;
                    objpatientnotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objpatientnotes.Message_Description = ddlMessageDescription.SelectedItem.Text;
                    objpatientnotes.Message_Date_And_Time = UtilityManager.ConvertToUniversal();
                    //CAP-1125
                    objpatientnotes.Is_PatientChart = "N";

                    WFObject objWf = new WFObject();
                    objWf.Current_Process = "START";
                    //CAP-1091
                    objWf.Current_Owner = ddlAssignedTo.SelectedItem.Value; // ddlAssignedTo.SelectedItem.Text;
                    objWf.Fac_Name = ClientSession.FacilityName;
                    objWf.Obj_Type = "TASK";
                    objWf.Current_Arrival_Time = UtilityManager.ConvertToUniversal();
                    patientnotesmngr.SavePatientMessage(objpatientnotes, objWf, null);
                    HttpContext.Current.Session["IsPatientCommunicated"] = true;
                    //CAP-726 - add loader
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "DisplayErrorMessage('420082'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                }
            }

            ddlMessageDescription.Text = string.Empty;
            txtNotes.txtDLC.Text = string.Empty;
            ddlAssignedTo.Text = string.Empty;
            chkshowall.Checked = false;
        }

        protected void btnFindPCPProvider_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }


        public void MaskedTextBoxColorChange(RadMaskedTextBox txtbox)
        {
            txtbox.ReadOnly = true;
            txtbox.CssClass = "nonEditabletxtbox";
            // txtbox.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
            if (txtbox.ID == "dtpGuarantorDOB")
            {
                txtbox.ReadOnly = true;
            }
            if (txtbox.ID == "dtpEmerDOB")
            {
                txtbox.ReadOnly = true;
            }

        }
        public void MaskedTextBoxColorChangewhite(RadMaskedTextBox txtbox)
        {
            txtbox.ReadOnly = false;
            txtbox.CssClass = "Editabletxtbox";
        }
        protected void btnAddGuarantorRefresh_Click(object sender, EventArgs e)
        {
            RefreshAddGuarantor();
        }


        public void RefreshAddGuarantor()
        {
            if (hdnGuarantorID.Value != string.Empty)
            {
                IList<Human> humanlist = new List<Human>();
                if (hdnGuarantorID.Value != string.Empty)
                {
                    if (hdnGuarantorID.Value != "" && hdnGuarantorID.Value != "undefined")
                    {
                        humanlist = HumanMngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(hdnGuarantorID.Value));

                        if (humanlist != null && humanlist.Count > 0)
                        {
                            txtGuaEmail.Text = humanlist[0].Gaurantor_Email;
                            txtGuarantorFirstName.Text = humanlist[0].First_Name;
                            txtGuarantorCity.Text = humanlist[0].City;
                            txtGuarantorLastName.Text = humanlist[0].Last_Name;
                            txtGuarantorMiddleName.Text = humanlist[0].MI;
                            // txtGuarantorAddress.Text = humanlist[0].Street_Address1 + humanlist[0].Street_Address2;
                            txtGuarantorAddress.Text = humanlist[0].Street_Address1;
                            txtGuarantorAddressLine2.Text = humanlist[0].Street_Address2;
                            for (int i = 0; i < ddlGuarantorSex.Items.Count; i++)
                            {
                                if (Convert.ToString(ddlGuarantorSex.Items[i].Value).ToUpper() == humanlist[0].Sex.ToUpper())
                                {
                                    ddlGuarantorSex.SelectedIndex = i;
                                    hdnGuarantorSex.Value = ddlGuarantorSex.Items[i].Value;
                                }
                            }
                            for (int i = 0; i < ddlGuarantorState.Items.Count; i++)
                            {
                                if (Convert.ToString(ddlGuarantorState.Items[i].Value).ToUpper() == humanlist[0].State.ToUpper())
                                {
                                    ddlGuarantorState.SelectedIndex = i;
                                    hdnGuarantorState.Value = ddlGuarantorState.Items[i].Value;
                                }
                            }
                            msktxtGuarantorCellNo.Text = humanlist[0].Cell_Phone_Number;
                            msktxtGuarantorHomeNo.Text = humanlist[0].Home_Phone_No;
                            msktxtGuarantorZipCode.Text = humanlist[0].ZipCode;
                            //dtpGuarantorDOB.SelectedDate = humanlist[0].Birth_Date;//.ToString("dd-MMM-yyyy");
                            dtpGuarantorDOB.Text = humanlist[0].Birth_Date.ToString("dd-MMM-yyyy");

                        }
                    }
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            AddDetails();

        }
        public void RefreshGuarantor()
        {
            if (chkGuarantorIsPatient.Enabled == true)
            {
                if (chkGuarantorIsPatient.Checked == true)
                {

                    dtpGuarantorDOB.Enabled = false;
                    //DisableTableLayout(pnlGuarantorInfo);
                    txtGuaEmail.Text = txtEmail.Text;
                    txtGuarantorFirstName.Text = txtPatientfirstname.Text;
                    txtGuarantorLastName.Text = txtPatientlastname.Text;
                    txtGuarantorMiddleName.Text = txtPatientmiddlename.Text;
                    txtGuarantorAddress.Text = txtPatientAddress.Text;
                    txtGuarantorAddressLine2.Text = txtPatientAddressLine2.Text;
                    txtGuarantorCity.Text = txtCity.Text;
                    msktxtGuarantorZipCode.Text = msktxtZipcode.Text;

                    for (int k = 0; k < ddlGuarantorSex.Items.Count; k++)
                    {
                        if (ddlGuarantorSex.Items[k].Text == ddlPatientsex.SelectedItem.Text)
                        {
                            ddlGuarantorSex.SelectedIndex = k;
                            hdnGuarantorSex.Value = ddlPatientsex.SelectedItem.Text;
                            break;
                        }
                    }
                    for (int l = 0; l < ddlGuarantorState.Items.Count; l++)
                    {
                        if (ddlGuarantorState.Items[l].Text == ddlState.SelectedItem.Text)
                        {
                            ddlGuarantorState.SelectedIndex = l;
                            hdnGuarantorState.Value = ddlState.SelectedItem.Text;
                            break;
                        }
                    }

                    for (int i = 0; i < ddlGuarantorRelationship.Items.Count; i++)
                    {

                        if (Convert.ToString(ddlGuarantorRelationship.Items[i].Text).ToUpper() == "SELF")
                        {
                            ddlGuarantorRelationship.SelectedIndex = i;
                            break;
                        }

                    }

                    if (dtpPatientDOB.Text != "")
                    {
                        dtpGuarantorDOB.Text = dtpPatientDOB.Text;
                    }
                    msktxtGuarantorCellNo.Text = msktxtCellPhno.Text;
                    msktxtGuarantorHomeNo.Text = msktxtHomePhno.Text;
                    if (ddlGuarantorRelationship.Text == string.Empty)
                    {
                        //    ddlGuarantorRelationship.SelectedIndex = 0;
                    }
                    ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                }
            }
        }

        protected void dtpPatientDOB_TextChanged(object sender, EventArgs e)
        {
            dtpGuarantorDOB.Text = dtpPatientDOB.Text;
        }


        protected void btnUncheckGurantor_Click(object sender, EventArgs e)
        {
            btnAddGuarantor.Enabled = true;
            btnSelectGaurantor.Enabled = true;
            dtpGuarantorDOB.Text = string.Empty;
            DisableTableLayout(pnlGuarantorInfo);
            ddlGuarantorRelationship.Enabled = true;
            ddlGuarantorRelationship.BackColor = Color.White;
            ClearGuarantorInfo();
        }

        protected void btnFindpatientClick_Click(object sender, EventArgs e)
        {

            if (hdnPatientID.Value.ToString() != string.Empty)
            {
                btnSendEmail.Enabled = true;
                // btnViewUpdateInsurance.Enabled = true;
                btnSave.Enabled = true;
                if (hdnPatientID.Value != string.Empty)
                    LoadHumanDetails(Convert.ToUInt64(hdnPatientID.Value), string.Empty, string.Empty);
            }
        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PatientDemographics", "OpenAddressHistory();", true);
        }

        protected void btnViewUpdateInsurance_Click(object sender, EventArgs e)
        {
            InsuranceLoad();
        }

        //protected void ddlPatientsex_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    HiddenPatientSex.Value = ddlPatientsex.SelectedItem.Text;
        //}

        protected void txtMothersMaidenName_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }

        protected void ImgbtnDropdown_Click(object sender, ImageClickEventArgs e)
        {

            if (hdnimgurl.Value != "DropDown")
            {
                if (ImgbtnDropdown.ImageUrl.Contains("~/Resources/Dropdownimg.jpg"))
                {
                    listRace.Style.Value = "display:block;position:absolute;width:120px;";
                    hdnimgurl.Value = "DropDown";
                }
            }
            else
            {
                listRace.Style.Value = "display:none;position:absolute;width:120px;";
                hdnimgurl.Value = "";

            }


        }

        protected void btnCheckDuplicate_Click(object sender, EventArgs e)
        {
            string YesNoMessage = hdnYesNoMessage.Value;
            hdnYesNoMessage.Value = string.Empty;
            System.Diagnostics.Stopwatch SaveTime = new System.Diagnostics.Stopwatch();
            SaveTime.Start();
            ulong myHumanID = 0;
            if (PatientInformationValidation() == false)
                goto l;
            if (hdnPatientID.Value.ToString() != string.Empty)
                myHumanID = Convert.ToUInt64(hdnPatientID.Value);
            if (myHumanID == 0)//(ulong)Session["ulPatientID"] == 0)
            {
                objHuman = new Human();
                SavePatientDetails();
                if (hdnLocalTime.Value != string.Empty)
                    objHuman.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                objHuman.Created_By = ClientSession.UserName;
                //objHuman.Birth_Date = Convert.ToDateTime(dtpPatientDOB.SelectedDate.Value);
                objHuman.Birth_Date = Convert.ToDateTime(dtpPatientDOB.Text);
                if (chkGuarantorIsPatient.Checked == false)
                {
                    //if (dtpGuarantorDOB.SelectedDate == null)
                    if (dtpGuarantorDOB.Text == "")
                        objHuman.Guarantor_Birth_Date = Convert.ToDateTime("1/1/1900");
                    //else if (Dobvalidation(Convert.ToDateTime(dtpGuarantorDOB.SelectedDate.Value)) == true)
                    //{
                    //    objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpGuarantorDOB.SelectedDate.Value);
                    //}
                    else if (Dobvalidation(Convert.ToDateTime(dtpGuarantorDOB.Text)) == true)
                        objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpGuarantorDOB.Text);
                    else
                    {
                        //ApplicationObject.erroHandler.DisplayErrorMessage("420033", "Patient Demographics", this.Page);
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420033');", true);
                        dtpGuarantorDOB.Focus();
                        goto l;
                    }
                    ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                }
                else
                {
                    //if (dtpPatientDOB.SelectedDate != null)
                    //    objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpPatientDOB.SelectedDate.Value);
                    if (dtpPatientDOB.Text != "")
                        objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpPatientDOB.Text);
                    ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                }

                //if (dtpEmerDOB.SelectedDate != null)
                if (dtpEmerDOB.Text != "")
                {
                    //if (dtpEmerDOB.SelectedDate != "")
                    //{
                    //if (Dobvalidation(Convert.ToDateTime(dtpEmerDOB.SelectedDate.Value)) == true)
                    if (Dobvalidation(Convert.ToDateTime(dtpEmerDOB.Text)) == true)
                    {
                        if (dtpEmerDOB.Enabled == true)
                            objHuman.Emergency_BirthDate = Convert.ToDateTime(dtpEmerDOB.Text);
                    }
                    else
                    {
                        //ApplicationObject.erroHandler.DisplayErrorMessage("420034", "Patient Demographics", this.Page);
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420034');", true);
                        dtpEmerDOB.Focus();
                        goto l;
                    }
                    //}
                }

                string sAccountExtNo = string.Empty;
                if (txtExternalAccNo.Text != "999999999999999")
                    sAccountExtNo = txtExternalAccNo.Text;
                HumanDTO CheckHuman = new HumanDTO();
                //Cap - 1883
                // CheckHuman = HumanMngr.GetPatientDetailsUsingPatientDetails(txtPatientlastname.Text, txtPatientfirstname.Text, Convert.ToDateTime(dtpPatientDOB.Text), ddlPatientsex.Text, txtMedicalRecordno.Text, sAccountExtNo);
                CheckHuman = HumanMngr.GetPatientDetailsUsingPatientDetails(txtPatientlastname.Text, txtPatientfirstname.Text, Convert.ToDateTime(dtpPatientDOB.Text), ddlPatientsex.Text, txtMedicalRecordno.Text, sAccountExtNo, ClientSession.LegalOrg);
                int iSave = 6;

                if (iSave == 2)
                {
                    txtPatientlastname.Focus();
                    goto l;
                }
                if (CheckHuman.MedicalRecordNoList == true)
                {
                    //ApplicationObject.erroHandler.DisplayErrorMessage("420044", "Patient Demographics", this.Page);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420044');", true);
                    txtMedicalRecordno.Focus();
                    goto l;

                }

                if (CheckHuman.Patient_Account_External == true)
                {
                    //ApplicationObject.erroHandler.DisplayErrorMessage("420046", "Patient Demographics", this.Page);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420046');", true);
                    txtExternalAccNo.Focus();
                    goto l;
                }
                PatGuarantor objPatguarantor = null;
                int iGuarantorID = 0;
                if (chkGuarantorIsPatient.Checked != true)
                {
                    if (hdnGuarantorID.Value != string.Empty)
                    {
                        iGuarantorID = Convert.ToInt32(hdnGuarantorID.Value);
                        if (iGuarantorID != 0)
                        {
                            objPatguarantor = new PatGuarantor();
                            objPatguarantor.Active = "YES";
                            objPatguarantor.Created_By = ClientSession.UserName;
                            if (hdnLocalTime.Value != string.Empty)
                            {
                                objPatguarantor.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                                objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                            }
                            objPatguarantor.Guarantor_Human_ID = iGuarantorID;
                            objPatguarantor.Relationship = ddlGuarantorRelationship.SelectedItem.Text;
                            if (ddlGuarantorRelationship.SelectedItem.Text != string.Empty && ddlGuarantorRelationship.SelectedIndex != -1)
                            {
                                string temp = ddlGuarantorRelationship.Items[ddlGuarantorRelationship.SelectedIndex].Value;
                                string[] Relation = temp.Split('-');
                                if (Relation.Length == 2)
                                {
                                    objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);
                                }
                            }
                        }
                    }
                    ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                }
                else if (chkGuarantorIsPatient.Checked == true)
                {
                    objPatguarantor = new PatGuarantor();
                    objPatguarantor.Active = "YES";
                    objPatguarantor.Created_By = ClientSession.UserName;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objPatguarantor.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                    }
                    objPatguarantor.Guarantor_Human_ID = iGuarantorID;
                    objPatguarantor.Relationship = ddlGuarantorRelationship.SelectedItem.Text;
                    if (ddlGuarantorRelationship.SelectedItem.Text != string.Empty && ddlGuarantorRelationship.SelectedIndex != -1)
                    {
                        string temp = ddlGuarantorRelationship.Items[ddlGuarantorRelationship.SelectedIndex].Value;
                        string[] Relation = temp.Split('-');
                        if (Relation.Length == 2)
                        {
                            objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);
                        }
                    }
                    ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                }
                Human humanObj = null;

                string sFTPPath = UploadPhoto();

                if (sFTPPath != string.Empty)
                    objHuman.Photo_Path = sFTPPath;


                string sCarrier = string.Empty;
                //if (txtPrimaryInsCarrierName.Text != string.Empty && txtPrimaryInsCarrierName.Text.Trim() != "")
                //{
                //    sCarrier = txtPrimaryInsCarrierName.Text;
                //}


                humanObj = HumanMngr.AppendBatchToHuman(objHuman, objPatguarantor, sCarrier);
                IList<Human> ilstHuman = new List<Human>();
                ilstHuman.Add(objHuman);
                if (humanObj.Id != 0)
                {
                    #region commented By Deepak
                    //string HumanFileName = "Human" + "_" + humanObj.Id + ".xml";
                    //string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
                    //if (File.Exists(strXmlHumanFilePath) == false && humanObj.Id > 0)
                    //{
                    //    string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                    //    string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                    //    XmlDocument itemDoc = new XmlDocument();
                    //    XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                    //    itemDoc.Load(XmlText);
                    //    XmlNodeList xmlnode = itemDoc.GetElementsByTagName("EncounterDetails");
                    //    xmlnode[0].ParentNode.RemoveChild(xmlnode[0]);

                    //    int iAge = 0;
                    //    iAge = UtilityManager.CalculateAge(humanObj.Birth_Date);

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
                    #endregion
                    UpdateAgeinBlob(humanObj.Id, humanObj.Birth_Date);
                }

                if (humanObj != null)
                {
                    objHumanId = humanObj.Id;
                    hdnHumanId.Value = objHumanId.ToString();
                }
                ulPatientID = objHumanId;
                //  Session["ulPatientID"] = ulPatientID;
                hdnPatientID.Value = ulPatientID.ToString();
                txtAccountNo.Text = objHumanId.ToString();
                hdnDemoAccNumber.Value = objHumanId.ToString();
                //Added by Srividhya
                Session["HumanID"] = txtAccountNo.Text;

                if (Request["Functionality"] == null)
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}GetAddPatientGuarantor('" + YesNoMessage + "');DisplayErrorMessage('420020');", true);
                else
                {
                    string sHuman = JsonConvert.SerializeObject(humanObj);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}GetAddPatientGuarantor('" + YesNoMessage + "','" + sHuman + "');DisplayErrorMessage('420020');", true);
                }

                DisablePatientDetails();
                RefreshGuarantor();
                bSaveCheck = true;
                hdnSaveFlag.Value = "false";
                bFormCloseCheck = false;
                btnEditName.Enabled = true;
                btnViewMessage.Enabled = true;
                btnSave.Enabled = false;
                if (hdnbInsuredHuman.Value.ToUpper() == "FALSE")
                {
                    //frmAddInsurancePolicies.ulInsuredHumanId = objHumanId; need to add once modaal window completed
                    //this.Close();
                }
                else
                {
                    //  btnViewUpdateInsurance.Enabled = true;
                }
            }

            else
            {
                objHumanDTO = HumanMngr.GetHumanInuranceAndCarrierDetailsRCM(myHumanID, string.Empty, string.Empty);
                objHuman = objHumanDTO.HumanDetails;
                PatientInsuredPlan objPatInsPLan = null;
                SavePatientDetails();
                if (hdnLocalTime.Value != string.Empty)
                    objHuman.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                objHuman.Modified_By = ClientSession.UserName;
                #region Commented for Bugid 22806

                #endregion

                if (chkGuarantorIsPatient.Checked == false)
                {
                    //if (dtpGuarantorDOB.SelectedDate != null)
                    if (dtpGuarantorDOB.Text != "")
                    {
                        if (Dobvalidation(Convert.ToDateTime(dtpGuarantorDOB.Text)) == true)
                        {
                            objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpGuarantorDOB.Text);
                        }
                        else
                        {
                            //ApplicationObject.erroHandler.DisplayErrorMessage("420033", "Patient Demographics", this.Page);
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420033');", true);
                            dtpGuarantorDOB.Focus();
                            goto l;
                        }
                    }
                    ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                }
                else
                {

                    if (dtpPatientDOB.Text != "")
                    {
                        objHuman.Guarantor_Birth_Date = Convert.ToDateTime(dtpPatientDOB.Text);
                    }
                    ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                }
                //if (dtpEmerDOB.Text != "")
                if (dtpEmerDOB.Text != "")//(dtpEmerDOB.SelectedDate != null)//&& 
                {

                    if (Dobvalidation(Convert.ToDateTime(dtpEmerDOB.Text)) == true)
                    {
                        objHuman.Emergency_BirthDate = Convert.ToDateTime(dtpEmerDOB.Text);
                    }
                    else
                    {
                        //ApplicationObject.erroHandler.DisplayErrorMessage("420034", "Patient Demographics", this.Page);
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420034');", true);
                        dtpEmerDOB.Focus();
                        goto l;
                    }
                }
                string sAccountExtNo = string.Empty;

                if (txtExternalAccNo.Text != "999999999" && txtExternalAccNo.Text != sExternalAccno)
                {
                    sAccountExtNo = txtExternalAccNo.Text;
                }
                HumanDTO CheckHuman = new HumanDTO();
                if (txtMedicalRecordno.Text.ToUpper() != objHuman.Medical_Record_Number.ToUpper() && txtExternalAccNo.Text.ToUpper() != objHuman.Patient_Account_External.ToUpper())
                    //Cap - 1883
                    //CheckHuman = HumanMngr.GetPatientDetailsUsingPatientDetails(string.Empty, string.Empty, DateTime.MinValue, string.Empty, txtMedicalRecordno.Text, sAccountExtNo);
                    CheckHuman = HumanMngr.GetPatientDetailsUsingPatientDetails(string.Empty, string.Empty, DateTime.MinValue, string.Empty, txtMedicalRecordno.Text, sAccountExtNo, ClientSession.LegalOrg);
                if (txtMedicalRecordno.Text.ToUpper() != objHumanDTO.HumanDetails.Medical_Record_Number.ToUpper() && CheckHuman.MedicalRecordNoList == true)
                {
                    // ApplicationObject.erroHandler.DisplayErrorMessage("420044", "Patient Demographics", this.Page);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420044');", true);
                    txtMedicalRecordno.Focus();
                    goto l;
                }

                if (txtExternalAccNo.Text.ToUpper() != objHumanDTO.HumanDetails.Patient_Account_External.ToUpper() && CheckHuman.Patient_Account_External == true)
                {
                    //ApplicationObject.erroHandler.DisplayErrorMessage("420046", "Patient Demographics", this.Page);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420046');", true);
                    txtExternalAccNo.Focus();
                    goto l;
                }

                PatGuarantor objPatguarantor = null;
                int iUpdateGuarantorID = 0;
                if (chkGuarantorIsPatient.Checked != true)
                {
                    if (hdnGuarantorID.Value != string.Empty)
                    {
                        iUpdateGuarantorID = Convert.ToInt32(hdnGuarantorID.Value);
                    }
                    ddlGuarantorRelationship.CssClass = "Editabletxtbox";
                }
                else
                {
                    if (txtAccountNo.Text != string.Empty)
                    {
                        iUpdateGuarantorID = Convert.ToInt32(txtAccountNo.Text);
                    }
                    ddlGuarantorRelationship.CssClass = "nonEditabletxtbox";
                }
                if (iUpdateGuarantorID != 0)
                {
                    IList<PatGuarantor> patguarantorlist = new List<PatGuarantor>();
                    if (txtAccountNo.Text != string.Empty) //code added by balaji.TJ 
                        patguarantorlist = patGuarantorMngr.GetPatGuarantorDetails(Convert.ToInt32(txtAccountNo.Text), iUpdateGuarantorID);
                    if (patguarantorlist != null && patguarantorlist.Count > 0) //code added by balaji
                    {
                        objPatguarantor = patguarantorlist[0];
                        if (txtAccountNo.Text != string.Empty) //code added by balaji.TJ 
                            objPatguarantor.Human_ID = Convert.ToInt32(txtAccountNo.Text);
                        objPatguarantor.Active = "YES";
                        objPatguarantor.Modified_By = ClientSession.UserName;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objPatguarantor.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                            objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                        }
                        objPatguarantor.Guarantor_Human_ID = iUpdateGuarantorID;
                        objPatguarantor.Relationship = ddlGuarantorRelationship.SelectedItem.Text;
                        if (ddlGuarantorRelationship.SelectedItem.Text != string.Empty && ddlGuarantorRelationship.SelectedIndex != -1)
                        {
                            string temp = ddlGuarantorRelationship.Items[ddlGuarantorRelationship.SelectedIndex].Value;
                            string[] Relation = temp.Split('-');
                            if (Relation.Length == 2)
                                objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);
                        }
                    }
                    else
                    {
                        objPatguarantor = new PatGuarantor();
                        if (txtAccountNo.Text != string.Empty) //code added by balaji.TJ 
                            objPatguarantor.Human_ID = Convert.ToInt32(txtAccountNo.Text);
                        objPatguarantor.Active = "YES";
                        objPatguarantor.Created_By = ClientSession.UserName;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objPatguarantor.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                            objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                        }
                        objPatguarantor.Guarantor_Human_ID = iUpdateGuarantorID;
                        objPatguarantor.Relationship = ddlGuarantorRelationship.SelectedItem.Text;
                        if (ddlGuarantorRelationship.SelectedItem.Text != string.Empty && ddlGuarantorRelationship.SelectedIndex != -1)
                        {
                            string temp = ddlGuarantorRelationship.Items[ddlGuarantorRelationship.SelectedIndex].Value;
                            string[] Relation = temp.Split('-');
                            if (Relation.Length == 2)
                                objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);
                        }
                    }
                }

                string sFTPPath = UploadPhoto();

                if (sFTPPath != string.Empty)
                    objHuman.Photo_Path = sFTPPath;

                string sPriCarrier = string.Empty;
                //if (txtPrimaryInsCarrierName.Text != string.Empty && txtPrimaryInsCarrierName.Text.Trim() != "")
                //{
                //    sPriCarrier = txtPrimaryInsCarrierName.Text;
                //}
                objHumanDTO.HumanDetails = HumanMngr.UpdateBatchToHuman(objHuman, objPatguarantor, objPatInsPLan, sPriCarrier);

                //objHumanDTO.HumanDetails = HumanMngr.UpdateBatchToHuman(objHuman, objPatguarantor, objPatInsPLan,string.Empty);

                //IList<Human> ilstHuman = new List<Human>();
                IList<Human> ilstHuman = new List<Human>();
                ilstHuman.Add(objHuman);


                if (YesNoMessage == "Yes")
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}YesNoCancel();", true);
                else
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420020');", true);
                DisablePatientDetails();
                //RefreshGuarantor();
                bSaveCheck = true;
                bFormCloseCheck = false;
                btnEditName.Enabled = true;
                btnSave.Enabled = false;
                hdnSaveFlag.Value = "false";
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Stoploadcursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420020');", true);
            }

        l:

            SaveTime.Stop();
            //Added By priyangha
            if (ddlPatientStatus.Text == "ALIVE")
            {
                //dtpDateOfDeath.SelectedDate = null;//string.Empty;
                dtpDateOfDeath.Text = string.Empty;
                ddlReasonForDeath.SelectedIndex = 0;
                // dtpDateOfDeath.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                dtpDateOfDeath.Attributes.Add("readOnly", "readOnly");
                ComboBoxColorChange(ddlReasonForDeath);
                dtpDateOfDeath.Attributes.Add("disabled", "disabled");
            }

            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Stoploadcursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
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

            string message = string.Empty;
            Exception objErr = new Exception(); // Server.GetLastError().GetBaseException();


            if (fileupload.HasFile)
            {
                try
                {
                    if (fileupload.PostedFile.ContentType.Contains("image") == true)
                    {
                        if (fileupload.PostedFile.ContentLength < 10000000)
                        {
                            sPath = Server.MapPath("~/atala-capture-download/" + Session.SessionID);  //Page.MapPath("atala-capture-download/" + Session.SessionID);
                            DirectoryInfo virdir = new DirectoryInfo(sPath);
                            if (!virdir.Exists)
                            {
                                virdir.Create();
                            }

                            string sImgFileName = txtPatientlastname.Text + "_" + txtPatientfirstname.Text + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
                            string filename = sPath + "//" + sImgFileName;
                            fileupload.PostedFile.SaveAs(filename);

                            sFTPPath = ftpImage.UploadToImageServer("PatientPhoto", ftpServerIP, ftpUserName, ftpPassword, filename, string.Empty, out string sCheckFileNotFoundException);
                            if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                                return string.Empty;
                            }
                            imgOverAllSummary.ImageUrl = "~//atala-capture-download/" + Session.SessionID + "//" + sImgFileName;
                        }
                    }
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

            ftpImage.DownloadFromImageServer("PatientPhoto", ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(sPhotoPath), sPath,out string sCheckFileNotFoundExceptions);
            if (sCheckFileNotFoundExceptions != "" && sCheckFileNotFoundExceptions.Contains("CheckFileNotFoundException"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExceptions.Split('~')[1] + "\");", true);
                return;
            }

            imgOverAllSummary.ImageUrl = "~//atala-capture-download/" + Session.SessionID + "//" + Path.GetFileName(sPhotoPath);
        }

        public static string laodAssigned(string chkshowall, string facility)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<string> patientlst = new List<string>();
            PatientNotesManager objPatNotesMngr = new PatientNotesManager();
            if (chkshowall == "false" && facility == "")
                patientlst = objPatNotesMngr.MapPhysicianUserListForFacility(ClientSession.FacilityName, ClientSession.LegalOrg);
            else if (chkshowall == "false" && facility != "")
                patientlst = objPatNotesMngr.MapPhysicianUserListForFacility(facility, ClientSession.LegalOrg);
            else
                patientlst = objPatNotesMngr.MapPhysicianUserListForFacility("SHOW ALL", ClientSession.LegalOrg);
            var Result = new { AssignedTo = patientlst };
            return JsonConvert.SerializeObject(Result);
        }

        public void UpdateAgeinBlob(ulong ulHumanID, DateTime BirthDate)
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
            iAge = UtilityManager.CalculateAge(BirthDate);

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
                //CAP-1942
                throw new Exception(xmlexcep.Message.ToString(),xmlexcep);

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
