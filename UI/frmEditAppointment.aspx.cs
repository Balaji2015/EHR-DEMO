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
using Acurus.Capella.Core.DTO;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Telerik.Web.Design;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Drawing;
using System.Globalization;
using Acurus.Capella.UI.UserControls;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using Newtonsoft.Json;
using System.Web.Services;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI
{
    public partial class frmEditAppointment : System.Web.UI.Page
    {
        #region Declarations

        ulong ulMyEncID;
        ulong ulMyHumanID;
        public ulong ulMyPhysicianID;
        int[] duration;
        string[] description;
        string MyProviderName;
        string sMyObjType = "ENCOUNTER";
        string MyStatus, MyFacilityName;

        public DateTime MyTime = DateTime.Now;
        public DateTime CalendarDate;

        bool saveCheckingFlag = false;
        bool EditWFobj = false;
        bool checkddlvisit = false;
        bool encounterArc = false;
        IList<string> lstOrdersID = new List<string>();
        IList<Encounter> Encntlist = new List<Encounter>();
        IList<FacilityLibrary> FacList = new List<FacilityLibrary>();

        Encounter EncRecord = new Encounter();
        EncounterManager EncMngr = new EncounterManager();
        PhysicianPOVManager PhyPOVMngr = new PhysicianPOVManager();
        WFObjectManager wfMngr = new WFObjectManager();
        StaticLookupManager staticMngr = new StaticLookupManager();

        int ulordID = 0;

        #region UnUsed Variables
        //DateTime MyApptDate;
        //int[] duration;
        //string[] description;
        //bool bPOVCheck;
        //PhysicianManager PhyMngr = new PhysicianManager();
        //UserLookupManager userMngr = new UserLookupManager();
        //OrdersManager OrderMngr = new OrdersManager();
        //PatientNotesManager objPatientNotesMngr = new PatientNotesManager();
        //AuthorizationManager AuthMngr = new AuthorizationManager();
        //FacilityManager FacMngr = new FacilityManager();
        #endregion
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            //log4net.GlobalContext.Properties["UserName"] = ClientSession.UserName;
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("Web.config")));
            //logger.Debug("--------------------frmEditAppointment Page Load Started--------------------");
            string time_taken = "";
            Stopwatch OverAllPageLoad = new Stopwatch();
            OverAllPageLoad.Start();

            txtPurposeofVisit.txtDLC.Attributes.Add("onkeypress", "EnableSaveButton(this);");
            txtPurposeofVisit.txtDLC.Attributes.Add("onchange", "EnableSaveButton(this);");
            txtNotes.txtDLC.Attributes.Add("onkeypress", "EnableSaveButton(this);");
            txtNotes.txtDLC.Attributes.Add("onchange", "EnableSaveButton(this);");
            //btnSave.Attributes.Add("Onclick", "javascript:return showTime();");
            txtPurposeofVisit.ListboxHeight = 150;
            txtPurposeofVisit.DName = "pbPurposeVisitDropDown";
            txtNotes.DName = "pbNotesDropDown";
            if (!IsPostBack)
            {
                //logger.Debug("Page Load occuring for the first time");
                IList<FacilityLibrary> facList;
                hdnParentscreen.Value = "EditAppointMents";
                hdnPbClick.Value = "Plus";
                hdnpbNotesClick.Value = "Plus";
                hdnTestClick.Value = "Plus";
                System.Diagnostics.Stopwatch LoadTime = new System.Diagnostics.Stopwatch();
                LoadTime.Start();

                //logger.Debug("Request['Human_id']=" + Request["Human_id"]);
                //logger.Debug("Request['facility']=" + Request["facility"]);
                //logger.Debug("Request['PhysicianName']=" + Request["PhysicianName"]);
                //logger.Debug("Request['PhysicianID']=" + Request["PhysicianID"]);
                //logger.Debug("Request['SelectedDate']=" + Request["SelectedDate"]);
                //logger.Debug("Request['CurrentProcess']=" + Request["CurrentProcess"]);
                //logger.Debug("Request['EncounterID']=" + Request["EncounterID"]);

                if (Request["Human_id"] != null && Request["Human_id"].ToString() != "")
                {
                    try
                    {
                        ulMyHumanID = Convert.ToUInt64(Request["Human_id"]);
                        hdnHumanID.Value = Request["Human_id"].ToString();
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Human_ID to UInt threw an error.", exp);
                        throw (exp);
                    }
                }

                if (Request["facility"] != null && Request["facility"].ToString() != "")
                {
                    hdnFacilityName.Value = Request["facility"].ToString().Replace("_", "#");
                }

                if (Request["PhysicianName"] != null)
                {
                    MyProviderName = Request["PhysicianName"].ToString();
                }

                if (Request["PhysicianID"] != null && Request["PhysicianID"].ToString() != "")
                {
                    try
                    {
                        ulMyPhysicianID = Convert.ToUInt64(Request["PhysicianID"]);
                        hdnPhysicianID.Value = ulMyPhysicianID.ToString();
                        ClientSession.PhysicianId = ulMyPhysicianID;
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Physician_ID to UInt threw an error.", exp);
                        throw (exp);
                    }
                }

                if (Request["SelectedDate"] != null)
                {
                    hdnSelectedDateTime.Value = Request["SelectedDate"].ToString();
                }

                if (Request["CurrentProcess"] != null)
                {
                    MyStatus = Request["CurrentProcess"];
                    hdnCurrentProcess.Value = MyStatus;
                }

                if (Request["EncounterID"] != null && Request["EncounterID"].ToString() != "")
                {
                    try
                    {
                        ulMyEncID = Convert.ToUInt64(Request["EncounterID"]);
                        hdnEncounterID.Value = ulMyEncID.ToString();
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Encounter_ID to UInt threw an error.", exp);
                        throw (exp);
                    }
                }

                //   txtAuthorizationNo.Attributes.Add("readOnly", "readOnly");
                FillNewEditAppointment fillneweditappt = null;
                chkReschedule.Enabled = false;
                //  btnFindPhysician.Enabled = true;
                FillPOV();

                //  txtReferringProvider.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //  txtReferringFacility.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //  txtProviderNPI.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //   txtReferingAddress.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //   msktxtReferingPhoneNo.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //   msktxtReferingFaxNo.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                // txtProviderNPI.Attributes.Add("onkeypress", "return false;");
                // txtReferringProvider.Attributes.Add("onkeypress", "return false;");
                // txtReferringFacility.Attributes.Add("onkeypress", "return false;");
                //  txtReferingAddress.Attributes.Add("onkeypress", "return false;");
                // txtProviderNPI.Attributes.Add("onkeydown", "return false;");
                //  txtReferringProvider.Attributes.Add("onkeydown", "return false;");
                //txtReferringFacility.Attributes.Add("onkeydown", "return false;");
                //  txtReferingAddress.Attributes.Add("onkeydown", "return false;");
                // msktxtReferingFaxNo.ReadOnly = true;
                // msktxtReferingPhoneNo.ReadOnly = true;

                if (ulMyEncID != 0)
                {
                    imgClearProviderText.Style.Add("top", "224px !important");
                    imgEditProvider.Style.Add("top", "224px !important");
                    //logger.Debug("Screen is in Edit Appoinment Mode as Encounter_ID is" + ulMyEncID.ToString());
                    if (ClientSession.UserName != null)
                        this.Page.Title = "Edit Appointment" + "-" + ClientSession.UserName;
                    Stopwatch GetEncounterAndHumanRecordDBCall = new Stopwatch();
                    GetEncounterAndHumanRecordDBCall.Start();
                    //logger.Debug("GetEncounterAndHumanRecord DB Call Starting");
                    fillneweditappt = EncMngr.GetEncounterAndHumanRecord(ulMyEncID, ulMyHumanID);
                    //logger.Debug("GetEncounterAndHumanRecord DB Call Completed.");
                    GetEncounterAndHumanRecordDBCall.Stop();
                    //logger.Debug("GetEncounterAndHumanRecord DB Call Time Taken :" + GetEncounterAndHumanRecordDBCall.Elapsed.Seconds + "." + GetEncounterAndHumanRecordDBCall.Elapsed.Milliseconds + "s");
                    time_taken += "GetEncounterAndHumanRecordDBCall : " + GetEncounterAndHumanRecordDBCall.Elapsed.Seconds + "." + GetEncounterAndHumanRecordDBCall.Elapsed.Milliseconds + "s; ";
                    ulordID = fillneweditappt.EncounterRecord.Order_Submit_ID;
                    //Jira CAP-2216
                    //if (fillneweditappt.EncounterRecord.Encounter_ID == 0)
                    if (fillneweditappt.EncounterRecord.Id == 0)
                    {
                        MyStatus = "ARCHIEVE";
                        hdnCurrentProcess.Value = "ARCHIEVE";
                        EncRecord = EncMngr.GetEncounterByEncounterIDArchive(ulMyEncID);
                        dtpApptDate.Enabled = false;
                        dtpStartTime.Enabled = false;

                        // ComboBoxColorChange(ddlVisitType, false);

                        //NumericUpDownColorChange(ddlDuration, false);

                        ComboBoxColorChange(ddlPhysicianName, false);
                        chkShowAllPhysicians.Enabled = false;
                        // Commented by valli 
                        //   txtProviderNPI.Attributes.Add("onkeypress", "return false;");
                        //   txtReferringProvider.Attributes.Add("onkeypress", "return false;");
                        //  txtReferringFacility.Attributes.Add("onkeypress", "return false;");
                        // txtReferingAddress.Attributes.Add("onkeypress", "return false;");
                        // txtProviderNPI.Attributes.Add("onkeydown", "return false;");
                        // txtReferringProvider.Attributes.Add("onkeydown", "return false;");
                        // txtReferringFacility.Attributes.Add("onkeydown", "return false;");
                        //  txtReferingAddress.Attributes.Add("onkeydown", "return false;");
                        //  msktxtReferingFaxNo.ReadOnly = true;
                        //  msktxtReferingPhoneNo.ReadOnly = true;
                        // TextBoxColorChange(txtReferringProvider, false);
                        //  TextBoxColorChange(txtReferringFacility, false);
                        //TextBoxColorChange(txtReferingAddress, false);
                        //  MaskedTextBoxColorChange(msktxtReferingFaxNo, false);
                        // MaskedTextBoxColorChange(msktxtReferingPhoneNo, false);
                        //Jira CAP-2216
                        //chkSelfReferred.Enabled = false;
                        //btnFindPhysician.Enabled = false;
                        //   btnFindAuthorization.Enabled = false;
                        chkReschedule.Enabled = false;
                        //Jira CAP-2216
                        //chkSelfReferred.Enabled = false;
                        chkShowAllPhysicians.Enabled = false;
                        encounterArc = true;
                    }
                    else
                    {
                        EncRecord = fillneweditappt.EncounterRecord;
                    }
                    //logger.Debug("EncRecord.Visit_Type=" + EncRecord.Visit_Type);
                    if (ddlVisitType.Items.Count > 0)
                    {
                        ddlVisitType.Text = EncRecord.Visit_Type.ToUpper();
                    }

                    //Jira #CAP-168 - if condition is commented for set the iteam for the deactivate user 
                    // if (ddlVisitType.Items.Count > 0)
                    // {
                    if (ddlVisitType.Items.FindItemByText(EncRecord.Visit_Type.ToUpper()) == null)
                    {
                        RadComboBoxItem items = new RadComboBoxItem();
                        items.Text = EncRecord.Visit_Type.ToUpper();
                        ddlVisitType.Items.Add(items);
                        checkddlvisit = true;
                        HdnEditVisit.Value = EncRecord.Visit_Type.ToUpper() + "|" + EncRecord.Duration_Minutes.ToString();
                    }
                    ddlVisitType.Items.FindItemByText(EncRecord.Visit_Type.ToUpper()).Selected = true;
                    //}
                    ddlDuration.Text = EncRecord.Duration_Minutes.ToString();
                    //Cap - 3217
                    if (EncRecord.Is_Auth_Verified == "Y")
                    {
                        IsAuthVerified.Checked = true;  
                    }
                    else
                    {
                        IsAuthVerified.Checked = false;
                    }
                    //logger.Debug("EncRecord.Visit_Type=" + EncRecord.Visit_Type);

                    // commented by valli
                    //if (EncRecord.Referring_Provider_NPI != string.Empty && EncRecord.PCP_Provider_NPI == string.Empty)
                    //{
                    //    txtProviderNPI.Text = EncRecord.Referring_Provider_NPI;
                    //}
                    //else if (EncRecord.PCP_Provider_NPI != string.Empty && EncRecord.Referring_Provider_NPI == string.Empty)
                    //{
                    //    txtProviderNPI.Text = EncRecord.PCP_Provider_NPI;
                    //}
                    //else if (EncRecord.PCP_Provider_NPI != string.Empty && EncRecord.Referring_Provider_NPI != string.Empty)
                    //{
                    //    txtProviderNPI.Text = EncRecord.Referring_Provider_NPI;
                    //}



                    hdnEncounter_Physician_id.Value = fillneweditappt.Encounter_Provider_ID.ToString();
                    //logger.Debug("Encounter_Provider_ID=" + fillneweditappt.Encounter_Provider_ID.ToString());
                    if (!checkddlvisit)
                    {
                        for (int i = 0; i < ddlVisitType.Items.Count; i++)
                        {
                            if (ddlVisitType.Text == ddlVisitType.Items.FindItemByText(ddlVisitType.Items[i].Text).Text)
                            {
                                int iIndexValue = ddlVisitType.SelectedIndex;
                                // txtVisitDescription.Text = ddlVisitType.Items.FindItemByText(ddlVisitType.Items[i].Text).Value.Split(new string[] { "$#%" }, StringSplitOptions.None)[1];

                                txtVisitDescription.Text = description[iIndexValue];
                                HdnEditVisit.Value = ddlVisitType.Text.ToUpper() + "|" + duration[iIndexValue].ToString();
                            }
                        }
                    }
                    else
                    {
                        txtVisitDescription.Text = "";
                    }
                    DateTime dtTime = UtilityManager.ConvertToLocal(EncRecord.Appointment_Date);
                    //logger.Debug("Appointment_Date=" + dtTime.ToString("yyyy-MMM-dd hh:mm:ss"));
                    dtpStartTime.SelectedTime = new TimeSpan(dtTime.Hour, dtTime.Minute, dtTime.Second);

                    txtPurposeofVisit.txtDLC.Text = EncRecord.Purpose_of_Visit;
                    txtNotes.txtDLC.Text = EncRecord.Notes;
                    txtPatientAccountNumber.Text = EncRecord.Human_ID.ToString();
                    //logger.Debug("EncRecord.Human_ID=" + EncRecord.Human_ID.ToString());
                    //txtTest.Text = fillneweditappt.Test;//not been used right now 12-01-2016 by vasanth

                    foreach (string str in fillneweditappt.CptAndItsOrderId)
                    {
                        hdnOrderList.Value += str + "-";
                        lstOrdersID.Add(str);
                    }

                    if (lstOrdersID.Count == 0)
                    {
                        lstOrdersID = new List<string>();
                        hdnOrderList.Value = string.Empty;
                    }
                    if (fillneweditappt != null)
                    {
                        txtPatientName.Text = fillneweditappt.Last_Name + "," + fillneweditappt.First_Name +
                       "  " + fillneweditappt.MI + "  " + fillneweditappt.Suffix;

                        //if (fillneweditappt.bAuthcount == false)
                        //{
                        //    if (fillneweditappt.AuthNo != string.Empty)
                        //    {
                        //        txtAuthorizationNo.Text = fillneweditappt.AuthNo;
                        //        hdnAuthId.Value = fillneweditappt.AuthorizationId.ToString();
                        //    }
                        //}
                        //else
                        //{
                        //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "OpenAuthorization();", true);
                        //}
                    }
                    //facList = ApplicationObject.facilityLibraryList;
                    var fac = from f in ApplicationObject.facilityLibraryList where f.Legal_Org == ClientSession.LegalOrg select f;
                    facList = fac.ToList<FacilityLibrary>();
                    //logger.Debug("Filling Facility Combobox");
                    if (facList != null)
                    {
                        //logger.Debug("Facility list count=" + facList.Count.ToString());
                        for (int i = 0; i < facList.Count; i++)
                        {
                            RadComboBoxItem cboItem = new RadComboBoxItem();
                            cboItem.Text = facList[i].Fac_Name;
                            this.cboFacility.Items.Add(cboItem);
                            if (hdnFacilityName.Value != null && hdnFacilityName.Value != "")
                            {
                                if (hdnFacilityName.Value == facList[i].Fac_Name)
                                {
                                    cboFacility.SelectedIndex = i;
                                }
                            }
                            else
                            {
                                if (ClientSession.FacilityName == facList[i].Fac_Name)
                                {
                                    cboFacility.SelectedIndex = i;
                                }
                            }
                        }
                    }
                    //else
                    //logger.Debug("Facility list is null. Note it is Application Object. So it must be some serious issue.");
                    //if (cboFacility.SelectedItem.Text.ToUpper() == sFacilityCmg.ToUpper())
                    //{
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                    {
                        ChangeproviderforCMGAncillary(true);
                    }
                    else
                        ChangeproviderforCMGAncillary(false);
                    if (fillneweditappt != null)
                    {
                        txtPatientDOB.Text = fillneweditappt.Birth_Date.ToString("dd-MMM-yyyy");
                        txtHomePhoneNumber.Text = fillneweditappt.Home_Phone_No;
                        txtCellPhoneNumber.Text = fillneweditappt.Cell_Phone_No;
                        txtHumanType.Text = fillneweditappt.HumanType;
                    }
                    if (MyStatus != null && MyStatus.ToUpper() == "SCHEDULED" && encounterArc == false)
                    {
                        chkReschedule.Enabled = true;
                    }
                    if (MyStatus != null && MyStatus.ToUpper() != "SCHEDULED")
                    {
                        dtpApptDate.Enabled = false;
                        dtpStartTime.Enabled = false;

                        // ComboBoxColorChange(ddlVisitType, false);

                        // NumericUpDownColorChange(ddlDuration, false);

                        ComboBoxColorChange(ddlPhysicianName, false);
                        chkShowAllPhysicians.Enabled = false;
                        // commented by valli
                        //  txtProviderNPI.Attributes.Add("onkeypress", "return false;");
                        // txtReferringProvider.Attributes.Add("onkeypress", "return false;");
                        // txtReferringFacility.Attributes.Add("onkeypress", "return false;");
                        //  txtReferingAddress.Attributes.Add("onkeypress", "return false;");
                        // txtProviderNPI.Attributes.Add("onkeydown", "return false;");
                        // txtReferringProvider.Attributes.Add("onkeydown", "return false;");
                        // txtReferringFacility.Attributes.Add("onkeydown", "return false;");
                        //  txtReferingAddress.Attributes.Add("onkeydown", "return false;");
                        //  msktxtReferingFaxNo.ReadOnly = true;
                        //  msktxtReferingPhoneNo.ReadOnly = true;
                        //  TextBoxColorChange(txtReferringProvider, false);
                        //   TextBoxColorChange(txtReferringFacility, false);
                        // TextBoxColorChange(txtReferingAddress, false);
                        //  MaskedTextBoxColorChange(msktxtReferingFaxNo, false);
                        // MaskedTextBoxColorChange(msktxtReferingPhoneNo, false);
                        //Jira CAP-2216
                        //chkSelfReferred.Enabled = false;
                        //btnFindPhysician.Enabled = false;
                        // btnFindAuthorization.Enabled = false;
                        chkReschedule.Enabled = false;
                        //Jira CAP-2216
                        //chkSelfReferred.Enabled = false;
                        chkShowAllPhysicians.Enabled = false;
                        imgClearProviderText.Attributes.Remove("onclick");
                        //Jira #CAP-69 - labels are missing
                        imgClearProviderText.Disabled = true;
                        imgEditProvider.Disabled = true;

                    }
                }
                else
                {
                    //logger.Debug("Screen is in New Appoinment Mode as Encounter_ID is 0");
                    if (ClientSession.UserName != null)
                        this.Page.Title = "New Appointment" + "-" + ClientSession.UserName;
                    #region old db call
                    // fillneweditappt = EncMngr.GetEncounterAndHumanRecord(ulMyEncID, ulMyHumanID);

                    //  EncRecord = fillneweditappt.EncounterRecord;
                    //txtPatientAccountNumber.Text = fillneweditappt.Human_ID.ToString();

                    //if (fillneweditappt != null)
                    //{
                    //    txtPatientName.Text = fillneweditappt.Last_Name + "," + fillneweditappt.First_Name +
                    //   "  " + fillneweditappt.MI + "  " + fillneweditappt.Suffix;
                    //}

                    // txtPatientDOB.Text = fillneweditappt.Birth_Date.ToString("dd-MMM-yyyy");
                    //txtHumanType.Text = fillneweditappt.HumanType;

                    //txtHomePhoneNumber.Text = fillneweditappt.Home_Phone_No;
                    //txtCellPhoneNumber.Text = fillneweditappt.Cell_Phone_No;

                    //ddlVisitType.Text = EncRecord.Visit_Type;
                    #endregion
                    //fillneweditappt.Human_ID = ulMyHumanID;
                    txtPatientAccountNumber.Text = ulMyHumanID.ToString();
                    //logger.Debug("Request['PatientName']=" + Request["PatientName"]);
                    //logger.Debug("Request['PatientDOB']=" + Request["PatientDOB"]);
                    //logger.Debug("Request['HumanType']=" + Request["HumanType"]);
                    //logger.Debug("Request['Home_Phone']=" + Request["Home_Phone"]);
                    //logger.Debug("Request['Cell_Phone']=" + Request["Cell_Phone"]);
                    //logger.Debug("Request['Encounter_Provider_ID']=" + Request["Encounter_Provider_ID"]);

                    if (Request["PatientName"] != null && Request["PatientName"].ToString() != "")
                    {
                        txtPatientName.Text = Request["PatientName"].ToString();
                    }

                    if (Request["PatientDOB"] != null && Request["PatientDOB"].ToString() != "")
                    {
                        try
                        {
                            txtPatientDOB.Text = Convert.ToDateTime(Request["PatientDOB"]).ToString("dd-MMM-yyyy");
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of Patient DOB to DateTime threw an error.", exp);
                            throw (exp);
                        }
                    }

                    if (Request["HumanType"] != null && Request["HumanType"].ToString() != "")
                    {
                        txtHumanType.Text = Request["HumanType"];
                    }

                    if (Request["Home_Phone"] != null && Request["Home_Phone"].ToString() != "")
                    {

                        txtHomePhoneNumber.Text = Request["Home_Phone"];

                    }

                    if (Request["Cell_Phone"] != null && Request["Cell_Phone"].ToString() != "")
                    {
                        txtCellPhoneNumber.Text = Request["Cell_Phone"];
                    }

                    if (Request["Encounter_Provider_ID"] != null && Request["Encounter_Provider_ID"].ToString() != "")
                    {
                        hdnEncounter_Physician_id.Value = Request["Encounter_Provider_ID"];
                    }
                    //facList = ApplicationObject.facilityLibraryList;
                    var facNewApptList = from f in ApplicationObject.facilityLibraryList where f.Legal_Org == ClientSession.LegalOrg select f;
                    facList = facNewApptList.ToList<FacilityLibrary>();
                    //logger.Debug("Filling Facility Combobox");
                    if (facList != null)
                    {
                        //logger.Debug("Facility list count=" + facList.Count.ToString());
                        for (int i = 0; i < facList.Count; i++)
                        {
                            RadComboBoxItem cboItem = new RadComboBoxItem();
                            cboItem.Text = facList[i].Fac_Name;
                            this.cboFacility.Items.Add(cboItem);
                            if (hdnFacilityName.Value != "")
                            {
                                if (hdnFacilityName.Value == facList[i].Fac_Name)
                                {
                                    cboFacility.SelectedIndex = i;
                                }
                            }
                            else
                            {
                                if (ClientSession.FacilityName == facList[i].Fac_Name)
                                {
                                    cboFacility.SelectedIndex = i;
                                }
                            }
                        }
                    }

                    //else
                    //logger.Debug("Facility list is null. Note it is Application Object. So it must be some serious issue.");

                    //for (int i = 0; i < ddlVisitType.Items.Count; i++)
                    //{
                    //    if (ddlVisitType.SelectedItem.Text == ddlVisitType.Items.FindItemByText(ddlVisitType.Items[i].Text).Text)
                    //    {
                    //        int iIndexValue = ddlVisitType.SelectedIndex;
                    //        txtVisitDescription.Text = description[iIndexValue];
                    //    }
                    //}
                    /* Changed for bug id=38345 
                    lblReferringName.Text = "PCP. Provider";
                    lblReferingFacility.Text = "PCP. Facility";
                    lblReferingAddress.Text = "PCP. Address";
                    lblReferingPhoneNo.Text = "PCP. Phone";
                    lblReferingFaxNo.Text = "PCP. Fax";
                    chkSelfReferred.Visible = false;
                    tabReferringProvAndPCP.SelectedIndex = 1; */

                    chkSelfReferred.Visible = true;
                    //    lblReferringName.Text = "Ref. Provider";
                    // lblReferingFacility.Text = "Ref. Facility";
                    //lblReferingAddress.Text = "Ref. Address";
                    //lblReferingPhoneNo.Text = "Ref. Phone";
                    //lblReferingFaxNo.Text = "Ref. Fax";
                    tabReferringProvAndPCP.SelectedIndex = 0;

                    if (hdnEncounter_Physician_id.Value != null && hdnEncounter_Physician_id.Value != "" && hdnEncounter_Physician_id.Value != "0")//vasanth
                    {
                        var fac = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                        IList<FacilityLibrary> ilstFac = fac.ToList<FacilityLibrary>();
                        if (ilstFac.Count > 0 && ilstFac[0].Is_Ancillary != "Y")
                        {

                            #region old db call commented for Jira #CAP-151 - Edited PCP info occurs in Referring Provider info tab 
                            ////if (sAncillary != string.Empty && sAncillary != cboFacility.SelectedItem.Text.Trim())
                            ////{
                            ////  hdnEncounter_Physician_id.Value = fillneweditappt.Encounter_Provider_ID.ToString();
                            ////PhysicianManager phyMngr = new PhysicianManager();

                            //// new code to obtain physician list from xml -- Pujhitha
                            //PhysicianLibrary objPhyLib = GetPhysicianDetailsByPhyID(hdnEncounter_Physician_id.Value.Trim());

                            ////IList<PhysicianLibrary> phylist = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(hdnEncounter_Physician_id.Value));
                            ////if (phylist != null && phylist.Count > 0)
                            ////{


                            //if (objPhyLib != null)
                            //{
                            //    //old code
                            //    // string sPhyName = objPhyLib.PhyPrefix + " " + objPhyLib.PhyFirstName + " " + objPhyLib.PhyLastName + " " + objPhyLib.PhySuffix;
                            //    //Gitlab# 2485 - Physician Name Display Change
                            //    string sPhyName = string.Empty;
                            //     if (objPhyLib.PhyLastName != String.Empty)
                            //        sPhyName += objPhyLib.PhyLastName;
                            //    if (objPhyLib.PhyFirstName != String.Empty)
                            //    {
                            //        if (sPhyName != String.Empty)
                            //            sPhyName += "," + objPhyLib.PhyFirstName;
                            //        else
                            //            sPhyName += objPhyLib.PhyFirstName;
                            //    }
                            //    if (objPhyLib.PhyMiddleName != String.Empty)
                            //        sPhyName += " " + objPhyLib.PhyMiddleName;
                            //    if (objPhyLib.PhySuffix != String.Empty)
                            //        sPhyName += "," + objPhyLib.PhySuffix;

                            //    //  txtReferringProvider.Text = sPhyName;
                            //    //  txtReferringFacility.Text = objPhyLib.PhyNotes;
                            //    //txtReferingAddress.Text = objPhyLib.PhyAddress1;
                            //    //  msktxtReferingPhoneNo.Text = objPhyLib.PhyTelephone;
                            //    //  msktxtReferingFaxNo.Text = objPhyLib.PhyFax;
                            //    //  txtProviderNPI.Text = objPhyLib.PhyNPI;


                            //    //Jira #CAP-69 - labels are missing
                            //    txtProviderSearch.Text = sPhyName + "| NPI: " + objPhyLib.PhyNPI +
                            //        "| Facility: " + "" + "| Address:" + objPhyLib.PhyAddress1 +
                            //        "| Phone No:" + objPhyLib.PhyTelephone + "| Fax No:" + objPhyLib.PhyFax ;

                            //    //hdnrenprovider.Value = " |" + sPhyName + "|" + objPhyLib.PhyNPI + "|" + "" + "|" + "" + "|" + objPhyLib.PhyAddress1 + "|"
                            //    //    + objPhyLib.PhyFax + "|" + objPhyLib.PhyTelephone;

                            //    //hdnpcpprovider.Value = " |" + sPhyName + "|" + objPhyLib.PhyNPI + "|" + "" + "|" + "" + "|" + objPhyLib.PhyAddress1 + "|"
                            //    //    + objPhyLib.PhyFax + "|" + objPhyLib.PhyTelephone;

                            //    //hdnrenprovidersearch.Value = " |" + sPhyName + "|" + objPhyLib.PhyNPI + "|" + "" + "|" + "" + "|" + objPhyLib.PhyAddress1 + "|"
                            //     // + objPhyLib.PhyFax + "|" + objPhyLib.PhyTelephone;

                            //    hdnrenprovider.Value = sPhyName + "| NPI: " + objPhyLib.PhyNPI +
                            //        "| Facility: " + "" + "| Address:" + objPhyLib.PhyAddress1 +
                            //        "| Phone No:" + objPhyLib.PhyTelephone + "| Fax No:" + objPhyLib.PhyFax ;

                            //    hdnpcpprovider.Value = sPhyName + "| NPI: " + objPhyLib.PhyNPI +
                            //        "| Facility: " + "" + "| Address:" + objPhyLib.PhyAddress1 +
                            //        "| Phone No:" + objPhyLib.PhyTelephone + "| Fax No:" + objPhyLib.PhyFax ;

                            //    txtProviderSearch.Enabled = false;
                            //    hdnrenprovidersearch.Value = sPhyName + "| NPI: " + objPhyLib.PhyNPI + "|Facility: " + "" + "|Address:" + objPhyLib.PhyAddress1 + "| " +
                            //        " Phone No:" + objPhyLib.PhyTelephone + "| Fax No:"
                            //       + objPhyLib.PhyFax ;

                            //    // HdnRefPhy.Value = sPhyName + "|" + objPhyLib.PhyAddress1 + "|" + objPhyLib.PhyTelephone + "|" + objPhyLib.PhyFax + "|" + objPhyLib.PhyNPI + "|" + objPhyLib.PhyNotes;
                            //    // HdnPcpPhy.Value = sPhyName + "|" + objPhyLib.PhyAddress1 + "|" + objPhyLib.PhyTelephone + "|" + objPhyLib.PhyFax + "|" + objPhyLib.PhyNPI + "|" + objPhyLib.PhyNotes;

                            //    if (ddlPhysicianName.Items.Count > 0)//added for bug id=38345 
                            //    {
                            //        //if (txtReferringProvider.Text.Trim() != string.Empty && txtReferringProvider.Text.Contains(ddlPhysicianName.SelectedItem.Text))
                            //        //{
                            //        //    chkSelfReferred.Checked = true;
                            //        //    btnFindPhysician.Enabled = false;
                            //        //}
                            //    }
                            //    else
                            //        chkSelfReferred.Checked = false;
                            //}
                            #endregion
                            if (ddlPhysicianName.Items.Count > 0)//added for bug id=38345 
                            {
                                //if (txtReferringProvider.Text.Trim() != string.Empty && txtReferringProvider.Text.Contains(ddlPhysicianName.SelectedItem.Text))
                                //{
                                //    chkSelfReferred.Checked = true;
                                //    btnFindPhysician.Enabled = false;
                                //}
                            }
                            else
                                chkSelfReferred.Checked = false;

                        }
                        else
                        {
                            hdnEncounter_Physician_id.Value = "0";
                            HdnRefPhy.Value = "|||||";//for static
                            HdnPcpPhy.Value = "|||||";
                        }
                        //}
                    }
                    else
                    {
                        hdnEncounter_Physician_id.Value = "0";
                        HdnRefPhy.Value = "|||||";//for static
                        HdnPcpPhy.Value = "|||||";
                    }

                }

                if (hdnSelectedDateTime.Value != null && hdnSelectedDateTime.Value != string.Empty)
                {
                    DateTime dt = new DateTime();
                    TimeSpan ts = new TimeSpan();
                    if (hdnSelectedDateTime.Value != string.Empty)
                    {
                        try
                        {
                            dt = Convert.ToDateTime(hdnSelectedDateTime.Value);
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of Calender Selected Date from hdnSelectedDateTime.Value='" + hdnSelectedDateTime.Value + "' to DateTime threw an error.", exp);
                            throw (exp);
                        }
                        ts = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
                    }

                    dtpStartTime.SelectedTime = ts;
                    //if (dtpStartTime.SelectedTime == TimeSpan.Zero)
                    //{
                    //    //logger.Debug("Request.QueryString['LocalTime']=" + Request.QueryString["LocalTime"].ToString());
                    //    if (Request.QueryString["LocalTime"] != null && Request.QueryString["LocalTime"].ToString() != "")
                    //    {
                    //        try
                    //        {
                    //            DateTime dt1 = Convert.ToDateTime(Request.QueryString["LocalTime"].ToString());
                    //            DateTime localtime = UtilityManager.ConvertToLocal(dt1);
                    //            TimeSpan ts1 = new TimeSpan(localtime.Hour, localtime.Minute, localtime.Second);
                    //            dtpStartTime.SelectedTime = ts1;
                    //        }
                    //        catch (Exception exp)
                    //        {
                    //            //logger.Debug("Conversion of Request.QueryString['LocalTime']=" + Request.QueryString["LocalTime"].ToString() + " to DateTime threw an error.", exp);
                    //            throw (exp);
                    //        }
                    //    }
                    //}
                }

                if (hdnSelectedDateTime.Value != null && hdnSelectedDateTime.Value != string.Empty)
                {
                    try
                    {
                        dtpApptDate.SelectedDate = Convert.ToDateTime(hdnSelectedDateTime.Value);
                        CalendarDate = Convert.ToDateTime(dtpApptDate.SelectedDate.Value);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Calender Selected Date from hdnSelectedDateTime.Value='" + hdnSelectedDateTime.Value + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                }

                saveCheckingFlag = true;


                //not been used right now 12-01-2016 by vasanth
                //if (hdnFacilityName.Value.ToUpper() == staticMngr.getStaticLookupByFieldName("CMG FACILITY NAME")[0].Value.ToUpper())
                //{
                //    lblTest.Text += "*";
                //    lblTest.ForeColor = Color.Red;

                //}

                // from here

                //FillPhysicianUser PhyUserList;     
                //logger.Debug("Loading Physician List for Facility='" + hdnFacilityName.Value.Trim().ToUpper() + "'");
                IList<PhysicianLibrary> PhysicianList = null;
                if (hdnFacilityName.Value != null)
                {
                    PhysicianList = UtilityManager.GetPhysicianList(hdnFacilityName.Value.Trim(), ClientSession.LegalOrg);//PhyMngr.GetPhysicianandUser(true, hdnFacilityName.Value);
                    //CAP-3499
                    if (PhysicianList != null)
                    {
                        var otherPhysicianList = UtilityManager.GetInActiveProviderList(hdnFacilityName.Value.Trim(), ClientSession.LegalOrg, true);
                        foreach (var physician in otherPhysicianList)
                        {
                            PhysicianList.Add(physician);
                        }
                    }
                }

                //if (hdnFacilityName.Value.ToUpper().Trim() == System.Configuration.ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper())
                //{
                //    if (PhysicianList.Count > 0)
                //    {
                //        PhysicianList = (from p in PhysicianList                                               
                //                               where p.Category.ToUpper().Trim() == "MACHINE"
                //                               select p).ToList<PhysicianLibrary>();                        
                //    }
                //}
                ddlPhysicianName.Items.Clear();
                bool phyCheck = false;
                //logger.Debug("Physician List count='" + PhysicianList.Count + "'");

                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = "";
                item.Value = "0";
                XmlDocument xmldoc = new XmlDocument();
                ddlPhysicianName.Items.Add(item);
                if (PhysicianList != null)
                {

                    for (int i = 0; i < PhysicianList.Count; i++)
                    {

                        item = new RadComboBoxItem();

                        //if (sAncillary != string.Empty && sAncillary == cboFacility.SelectedItem.Text.Trim())
                        //{
                        var fac = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                        IList<FacilityLibrary> ilstFac = fac.ToList<FacilityLibrary>();
                        if (ilstFac.Count > 0 && ilstFac[0].Is_Ancillary == "Y")
                        {
                            //Jira CAP-2777
                            //string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
                            //if (File.Exists(strXmlFilePathTech) == true)
                            {
                                //Jira CAP-2777
                                //xmldoc = new XmlDocument();
                                //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                                if (PhysicianList[i].PhyColor != "0")
                                {
                                    //Jira CAP-2777
                                    //XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + PhysicianList[i].PhyColor);
                                    //if (xmlTec != null && xmlTec.Count > 0)
                                    //{
                                    //    item.Text = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                    //    item.Value = xmlTec[0].Attributes.GetNamedItem("machine_technician_library_id").Value;
                                    //}

                                    //Jira CAP-2777
                                    MachinetechnicianList machinetechnicianList = new MachinetechnicianList();
                                    machinetechnicianList = ConfigureBase<MachinetechnicianList>.ReadJson("machine_technician.json");
                                    if (machinetechnicianList?.MachineTechnician != null)
                                    {
                                        List<Machinetechnician> machinetechnicians = new List<Machinetechnician>();
                                        machinetechnicians = machinetechnicianList.MachineTechnician.Where(x => x.machine_technician_library_id == PhysicianList[i].PhyColor).ToList();
                                        if (machinetechnicians.Count > 0)
                                        {
                                            item.Text = machinetechnicians[0].machine_name + " - " + PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                            item.Value = machinetechnicians[0].machine_technician_library_id;
                                        }
                                    }

                                }
                                else
                                {
                                    //old code
                                    //item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                    //Gitlab# 2485 - Physician Name Display Change
                                    if (PhysicianList[i].PhyLastName != String.Empty)
                                        item.Text += PhysicianList[i].PhyLastName;
                                    if (PhysicianList[i].PhyFirstName != String.Empty)
                                    {
                                        if (item.Text != String.Empty)
                                            item.Text += "," + PhysicianList[i].PhyFirstName;
                                        else
                                            item.Text += PhysicianList[i].PhyFirstName;
                                    }
                                    if (PhysicianList[i].PhyMiddleName != String.Empty)
                                        item.Text += " " + PhysicianList[i].PhyMiddleName;
                                    if (PhysicianList[i].PhySuffix != String.Empty)
                                        item.Text += "," + PhysicianList[i].PhySuffix;
                                    item.Value = PhysicianList[i].Id.ToString();
                                }

                                ddlPhysicianName.Items.Add(item);
                                if (item.Value == ulMyPhysicianID.ToString())
                                {
                                    phyCheck = true;
                                    ddlPhysicianName.SelectedIndex = i + 1;
                                    hdnEditApptPhyID.Value = ddlPhysicianName.SelectedValue.ToString();//added by vasanth
                                }
                            }
                        }
                        else
                        {
                            string sPhyName = string.Empty;
                            if (PhysicianList != null)
                            //old code
                            // sPhyName = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyLastName;
                            //Gitlab# 2485 - Physician Name Display Change
                            {
                                if (PhysicianList[i].PhyLastName != String.Empty)
                                    sPhyName += PhysicianList[i].PhyLastName;
                                if (PhysicianList[i].PhyFirstName != String.Empty)
                                {
                                    if (sPhyName != String.Empty)
                                        sPhyName += "," + PhysicianList[i].PhyFirstName;
                                    else
                                        sPhyName += PhysicianList[i].PhyFirstName;
                                }
                                if (PhysicianList[i].PhyMiddleName != String.Empty)
                                    sPhyName += " " + PhysicianList[i].PhyMiddleName;
                                if (PhysicianList[i].PhySuffix != String.Empty)
                                    sPhyName += "," + PhysicianList[i].PhySuffix;
                            }

                            RadComboBoxItem item1 = new RadComboBoxItem();
                            item1.Value = PhysicianList[i].Id.ToString();
                            item1.Text = sPhyName;
                            ddlPhysicianName.Items.Add(item1);
                            if (item1.Value == ulMyPhysicianID.ToString())
                            {
                                phyCheck = true;
                                ddlPhysicianName.SelectedIndex = i + 1;
                                hdnEditApptPhyID.Value = ddlPhysicianName.SelectedValue.ToString();//added by vasanth
                            }
                        }
                    }
                    //CAP-2024
                    var comboBoxItems = ddlPhysicianName.Items.Cast<RadComboBoxItem>().ToList();
                    ddlPhysicianName.Items.Clear();
                    //CAP-3693
                    ddlPhysicianName.Items.AddRange(comboBoxItems.Where(a => !string.IsNullOrWhiteSpace(a.Text)).OrderBy(a => a.Text.Trim()).ToArray());
                }
                //Jira #CAP-168
                if (ddlPhysicianName.Items.Count == 0 || ddlPhysicianName.SelectedItem.Text == "" || ddlPhysicianName.SelectedItem.Text == string.Empty || ddlPhysicianName.SelectedItem.Text == null)
                {
                    string sPhyName = string.Empty;
                    IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
                    PhysicianManager phymngr = new PhysicianManager();
                    ilstPhysicianLibrary = phymngr.GetphysiciannameByPhyID(ulMyPhysicianID);
                    if (ilstPhysicianLibrary != null && ilstPhysicianLibrary.Count > 0)
                    {
                        if (ilstPhysicianLibrary[0].PhyLastName != String.Empty)
                            sPhyName += ilstPhysicianLibrary[0].PhyLastName;
                        if (ilstPhysicianLibrary[0].PhyFirstName != String.Empty)
                        {
                            if (sPhyName != String.Empty)
                                sPhyName += "," + ilstPhysicianLibrary[0].PhyFirstName;
                            else
                                sPhyName += ilstPhysicianLibrary[0].PhyFirstName;
                        }
                        if (ilstPhysicianLibrary[0].PhyMiddleName != String.Empty)
                            sPhyName += " " + ilstPhysicianLibrary[0].PhyMiddleName;
                        if (ilstPhysicianLibrary[0].PhySuffix != String.Empty)
                            sPhyName += "," + ilstPhysicianLibrary[0].PhySuffix;

                        RadComboBoxItem item1 = new RadComboBoxItem();
                        item1.Value = ilstPhysicianLibrary[0].Id.ToString();
                        item1.Text = sPhyName;
                        ddlPhysicianName.Items.Add(item1);
                        int iselectedindex = ddlPhysicianName.Items.IndexOf(item1);
                        ddlPhysicianName.SelectedIndex = iselectedindex;
                        phyCheck = true;
                    }

                }
                //Jira #CAP-168
                //if (!phyCheck)
                if (!phyCheck)
                {
                    chkShowAllPhysicians.Checked = true;
                    chkShowAllPhysicians_CheckedChanged(sender, e);
                    hdnEditApptPhyID.Value = ddlPhysicianName.SelectedValue.ToString();//added by vasanth
                }


                //end here
                if (EncRecord != null && EncRecord.Referring_Physician != null && (EncRecord.Referring_Physician != string.Empty && EncRecord.PCP_Physician == string.Empty) || (EncRecord.Referring_Physician == string.Empty && EncRecord.PCP_Physician == string.Empty) && ulMyEncID != 0)
                {
                    tabReferringProvAndPCP.SelectedIndex = 0;
                    //if (ddlPhysicianName.SelectedItem.Text == EncRecord.Referring_Physician)
                    if (EncRecord.Referring_Physician != null && EncRecord.Referring_Physician.Trim() != string.Empty && EncRecord.Referring_Physician.Contains(ddlPhysicianName.SelectedItem.Text))
                    {
                        chkSelfReferred.Checked = false;
                        // btnFindPhysician.Enabled = false;
                    }
                    else
                    {
                        if (EncRecord.Is_Self_Referred == "Y")
                            chkSelfReferred.Checked = true;
                        else
                            chkSelfReferred.Checked = false;
                        //btnFindPhysician.Enabled = true;
                    }

                    chkSelfReferred.Visible = true;

                    // commented by valli


                    //   lblReferringName.Text = "Ref. Provider";
                    // lblReferingFacility.Text = "Ref. Facility";
                    //  lblReferingAddress.Text = "Ref. Address";
                    //  lblReferingPhoneNo.Text = "Ref. Phone";
                    //  lblReferingFaxNo.Text = "Ref. Fax";
                    //   txtReferringFacility.Text = EncRecord.Referring_Facility;
                    //   txtReferringProvider.Text = EncRecord.Referring_Physician;
                    //  txtReferingAddress.Text = EncRecord.Referring_Address;
                    // msktxtReferingPhoneNo.Text = EncRecord.Referring_Phone_No;
                    //  msktxtReferingFaxNo.Text = EncRecord.Referring_Fax_No;
                    //   txtProviderNPI.Text = EncRecord.Referring_Provider_NPI;
                    //  btnFindPhysician.Enabled = true;
                    //    txtReferringProvider.Attributes.Add("onkeypress", "return false;");
                    // txtReferringFacility.Attributes.Add("onkeypress", "return false;");
                    //  txtReferingAddress.Attributes.Add("onkeypress", "return false;");
                    // txtProviderNPI.Attributes.Add("onkeypress", "return false;");
                    //  txtProviderNPI.Attributes.Add("onkeydown", "return false;");
                    //  txtReferringProvider.Attributes.Add("onkeydown", "return false;");
                    // txtReferringFacility.Attributes.Add("onkeydown", "return false;");
                    //   txtReferingAddress.Attributes.Add("onkeydown", "return false;");
                    // msktxtReferingFaxNo.ReadOnly = true;
                    //  msktxtReferingPhoneNo.ReadOnly = true;
                    if (EncRecord.Referring_Physician != "")
                    {
                        txtProviderSearch.Enabled = false;
                        txtProviderSearch.Text = EncRecord.Referring_Physician + "| NPI: " + EncRecord.Referring_Provider_NPI +
                                  "| Facility: " + EncRecord.Referring_Facility + "| Address:" + EncRecord.Referring_Address +
                                  "| Phone No:" + EncRecord.Referring_Phone_No + "| Fax No:" + EncRecord.Referring_Fax_No;

                        //Jira #CAP-69 - labels are missing
                        //hdnrenprovider.Value = " |" + EncRecord.Referring_Physician + "|" + EncRecord.Referring_Provider_NPI + "|" + "" + "|" + EncRecord.Referring_Facility +
                        //    "|" + EncRecord.Referring_Address + "|"
                        //    + EncRecord.Referring_Fax_No + "|" + EncRecord.Referring_Phone_No;

                        //hdnrenprovidersearch.Value = " |" + EncRecord.Referring_Physician + "|" + EncRecord.Referring_Provider_NPI + "|" + "" + "|" + EncRecord.Referring_Facility +
                        //    "|" + EncRecord.Referring_Address + "|"
                        //    + EncRecord.Referring_Fax_No + "|" + EncRecord.Referring_Phone_No;

                        hdnrenprovider.Value = EncRecord.Referring_Physician + "| NPI: " + EncRecord.Referring_Provider_NPI +
                                  "| Facility: " + EncRecord.Referring_Facility + "| Address:" + EncRecord.Referring_Address +
                                  "| Phone No:" + EncRecord.Referring_Phone_No + "| Fax No:" + EncRecord.Referring_Fax_No;

                        hdnrenprovidersearch.Value = EncRecord.Referring_Physician + "| NPI: " + EncRecord.Referring_Provider_NPI +
                                  "| Facility: " + EncRecord.Referring_Facility + "| Address:" + EncRecord.Referring_Address +
                                  "| Phone No:" + EncRecord.Referring_Phone_No + "| Fax No:" + EncRecord.Referring_Fax_No;
                        hdnRefEditPhyId.Value = EncRecord.Referring_Physician;
                    }

                }
                else if (EncRecord != null && EncRecord.PCP_Physician != string.Empty && EncRecord.Referring_Physician == string.Empty)
                {
                    tabReferringProvAndPCP.SelectedIndex = 1;
                    chkSelfReferred.Visible = false;
                    //   lblReferringName.Text = "PCP. Provider";
                    //   lblReferingFacility.Text = "PCP. Facility";
                    //lblReferingAddress.Text = "PCP. Address";
                    //lblReferingPhoneNo.Text = "PCP. Phone";
                    //lblReferingFaxNo.Text = "PCP. Fax";
                    //   txtReferringFacility.Text = EncRecord.PCP_Facility;
                    //    txtReferringProvider.Text = EncRecord.PCP_Physician;
                    //txtReferingAddress.Text = EncRecord.PCP_Address;
                    //msktxtReferingPhoneNo.Text = EncRecord.PCP_Phone_No;
                    //msktxtReferingFaxNo.Text = EncRecord.PCP_Fax_No;
                    //  txtProviderNPI.Text = EncRecord.PCP_Provider_NPI;
                    //btnFindPhysician.Enabled = true;
                    //  txtReferringProvider.Attributes.Add("onkeypress", "return false;");
                    //  txtReferringFacility.Attributes.Add("onkeypress", "return false;");
                    //txtReferingAddress.Attributes.Add("onkeypress", "return false;");
                    //  txtProviderNPI.Attributes.Add("onkeypress", "return false;");
                    //  txtProviderNPI.Attributes.Add("onkeydown", "return false;");
                    //  txtReferringProvider.Attributes.Add("onkeydown", "return false;");
                    //  txtReferringFacility.Attributes.Add("onkeydown", "return false;");
                    //txtReferingAddress.Attributes.Add("onkeydown", "return false;");
                    //msktxtReferingFaxNo.ReadOnly = true;
                    //msktxtReferingPhoneNo.ReadOnly = true;
                    if (EncRecord.PCP_Physician != "")
                    {
                        txtProviderSearch.Text = EncRecord.PCP_Physician + "| NPI: " + EncRecord.PCP_Provider_NPI +
                                  "| Facility: " + EncRecord.PCP_Facility + "| Address:" + EncRecord.PCP_Address +
                                  "| Phone No:" + EncRecord.PCP_Phone_No + "| Fax No:" + EncRecord.PCP_Fax_No;

                        //Jira #CAP-69 - labels are missing
                        //hdnpcpprovider.Value = " |" + EncRecord.PCP_Physician + "|" + EncRecord.PCP_Provider_NPI + "|" + "" + "|" + EncRecord.PCP_Facility +
                        //    "|" + EncRecord.PCP_Address + "|"
                        //    + EncRecord.PCP_Fax_No + "|" + EncRecord.PCP_Phone_No;

                        //hdnpcpprovidersearch.Value = " |" + EncRecord.PCP_Physician + "|" + EncRecord.PCP_Provider_NPI + "|" + "" + "|" + EncRecord.PCP_Facility +
                        //    "|" + EncRecord.PCP_Address + "|"
                        //    + EncRecord.PCP_Fax_No + "|" + EncRecord.PCP_Phone_No;

                        //Jira cap - 3068
                        //hdnpcpprovider.Value = EncRecord.Referring_Physician + "| NPI: " + EncRecord.Referring_Provider_NPI +
                        //        "| Facility: " + EncRecord.Referring_Facility + "| Address:" + EncRecord.Referring_Address +
                        //        "| Phone No:" + EncRecord.Referring_Phone_No + "| Fax No:" + EncRecord.Referring_Fax_No;

                        //hdnpcpprovidersearch.Value = EncRecord.Referring_Physician + "| NPI: " + EncRecord.Referring_Provider_NPI +
                        //          "| Facility: " + EncRecord.Referring_Facility + "| Address:" + EncRecord.Referring_Address +
                        //          "| Phone No:" + EncRecord.Referring_Phone_No + "| Fax No:" + EncRecord.Referring_Fax_No;

                        hdnpcpprovider.Value = EncRecord.PCP_Physician + "| NPI: " + EncRecord.PCP_Provider_NPI +
                                  "| Facility: " + EncRecord.PCP_Facility + "| Address:" + EncRecord.PCP_Address +
                                  "| Phone No:" + EncRecord.PCP_Phone_No + "| Fax No:" + EncRecord.PCP_Fax_No;

                        hdnpcpprovidersearch.Value = EncRecord.PCP_Physician + "| NPI: " + EncRecord.PCP_Provider_NPI +
                                  "| Facility: " + EncRecord.PCP_Facility + "| Address:" + EncRecord.PCP_Address +
                                  "| Phone No:" + EncRecord.PCP_Phone_No + "| Fax No:" + EncRecord.PCP_Fax_No;


                        hdnpcpEditPhyId.Value = EncRecord.Reading_Provider_ID.ToString();
                        txtProviderSearch.Enabled = false;
                    }
                }
                else if (EncRecord != null && EncRecord.Referring_Physician != string.Empty && EncRecord.PCP_Physician != string.Empty)
                {
                    tabReferringProvAndPCP.SelectedIndex = 0;
                    chkSelfReferred.Visible = true;
                    chkSelfReferred.Checked = false;
                    //    lblReferringName.Text = "Ref. Provider";
                    //  lblReferingFacility.Text = "Ref. Facility";
                    //lblReferingAddress.Text = "Ref. Address";
                    //lblReferingPhoneNo.Text = "Ref. Phone";
                    //lblReferingFaxNo.Text = "Ref. Fax";
                    //   txtReferringFacility.Text = EncRecord.Referring_Facility;
                    //   txtReferringProvider.Text = EncRecord.Referring_Physician;
                    //txtReferingAddress.Text = EncRecord.Referring_Address;
                    //msktxtReferingPhoneNo.Text = EncRecord.Referring_Phone_No;
                    //msktxtReferingFaxNo.Text = EncRecord.Referring_Fax_No;
                    //txtProviderNPI.Text = EncRecord.Referring_Provider_NPI;
                    // txtReferringProvider.Attributes.Add("onkeypress", "return false;");
                    //txtReferringFacility.Attributes.Add("onkeypress", "return false;");
                    //txtReferingAddress.Attributes.Add("onkeypress", "return false;");
                    //txtProviderNPI.Attributes.Add("onkeypress", "return false;");
                    //txtProviderNPI.Attributes.Add("onkeydown", "return false;");
                    // txtReferringProvider.Attributes.Add("onkeydown", "return false;");
                    //txtReferringFacility.Attributes.Add("onkeydown", "return false;");
                    //txtReferingAddress.Attributes.Add("onkeydown", "return false;");
                    //msktxtReferingPhoneNo.ReadOnly = true;
                    //msktxtReferingFaxNo.ReadOnly = true;
                    if (EncRecord.Referring_Physician != "")
                    {
                        txtProviderSearch.Text = EncRecord.Referring_Physician + "| NPI: " + EncRecord.Referring_Provider_NPI +
                                   "| Facility: " + EncRecord.Referring_Facility + "| Address:" + EncRecord.Referring_Address +
                                   "| Phone No:" + EncRecord.Referring_Phone_No + "| Fax No:" + EncRecord.Referring_Fax_No;
                        //Jira #CAP-69 - labels are missing
                        //hdnrenprovider.Value = " |" + EncRecord.Referring_Physician + "|" + EncRecord.Referring_Provider_NPI + "|" + "" + "|" + EncRecord.Referring_Facility +
                        //    "|" + EncRecord.Referring_Address + "|"
                        //    + EncRecord.Referring_Fax_No + "|" + EncRecord.Referring_Phone_No;
                        //hdnrenprovidersearch.Value = EncRecord.Referring_Physician + "|" + EncRecord.Referring_Provider_NPI + "|" + "" + "|" + EncRecord.Referring_Facility +
                        // "|" + EncRecord.Referring_Address + "|"
                        // + EncRecord.Referring_Fax_No + "|" + EncRecord.Referring_Phone_No;

                        hdnrenprovider.Value = EncRecord.Referring_Physician + "| NPI: " + EncRecord.Referring_Provider_NPI +
                                   "| Facility: " + EncRecord.Referring_Facility + "| Address:" + EncRecord.Referring_Address +
                                   "| Phone No:" + EncRecord.Referring_Phone_No + "| Fax No:" + EncRecord.Referring_Fax_No;
                        hdnrenprovidersearch.Value = EncRecord.Referring_Physician + "| NPI: " + EncRecord.Referring_Provider_NPI +
                                   "| Facility: " + EncRecord.Referring_Facility + "| Address:" + EncRecord.Referring_Address +
                                   "| Phone No:" + EncRecord.Referring_Phone_No + "| Fax No:" + EncRecord.Referring_Fax_No;
                        hdnRefEditPhyId.Value = EncRecord.Reading_Provider_ID.ToString();
                        txtProviderSearch.Enabled = false;
                    }



                }
                //CAP-2373
                if (EncRecord != null)
                {
                    hdbselref.Value = EncRecord.Is_Self_Referred ?? "";
                }
                // commented by valli

                //if (txtReferringProvider.Text.Trim() != string.Empty && txtReferringProvider.Text.Contains(ddlPhysicianName.SelectedItem.Text))
                //{
                //    chkSelfReferred.Checked = true;
                //    btnFindPhysician.Enabled = false;
                //}
                //else
                //{
                //    chkSelfReferred.Checked = false;
                //    btnFindPhysician.Enabled = true;
                //}

                //if (txtProviderSearch.Text.Trim() != string.Empty && txtProviderSearch.Text.Contains(ddlPhysicianName.SelectedItem.Text))
                //{
                //    chkSelfReferred.Checked = true;
                //    // btnFindPhysician.Enabled = false;
                //}
                //else
                //{
                //    chkSelfReferred.Checked = false;
                //    //    btnFindPhysician.Enabled = true;
                //} //Commentted for BugID:56036


                DisableTableLayout(pnlReschedule);

                // FillReasonCode();                
                if (EncRecord != null && EncRecord.Reschedule_Reason_Code != string.Empty)
                {
                    ddlReasonCode.Text = EncRecord.Reschedule_Reason_Code;
                    if (ddlReasonCode.Items.Count == 0)
                        FillReasonCode();
                    if (ddlReasonCode.Items.Count > 0)
                        ddlReasonCode.Items.FindItemByText(EncRecord.Reschedule_Reason_Code).Selected = true;
                    txtReasonCode.Text = EncRecord.Reschedule_Reason_Text;
                }

                txtReasonCode.Focus();
                if (txtReasonCode.Text == string.Empty)
                    txtReasonCode.Text = ddlReasonCode.Text;

                LoadTime.Stop();
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);
                //if (cboFacility.SelectedItem.Text.ToUpper() == sFacilityCmg.ToUpper())
                //{
                var vfacAcillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                IList<FacilityLibrary> lstFacAncillary = vfacAcillary.ToList<FacilityLibrary>();
                if (lstFacAncillary.Count > 0 && lstFacAncillary[0].Is_Ancillary == "Y")
                {
                    ChangeproviderforCMGAncillary(true);
                }
                else
                    ChangeproviderforCMGAncillary(false);
                if (this.Page.Title.Contains("New Appointment") == true && txtPurposeofVisit.txtDLC.Enabled == true)
                {
                    btnSave.Enabled = true;
                    chkReschedule.Enabled = false;
                }
                else if (MyStatus != null && MyStatus.ToUpper() != "SCHEDULED")
                {
                    btnSave.Enabled = false;
                    chkReschedule.Enabled = false;
                    chkShowAllPhysicians.Enabled = false;
                    //Jira CAP-2216
                    //chkSelfReferred.Enabled = false;
                    dtpApptDate.Enabled = false;
                    dtpStartTime.Enabled = false;
                    dtpStartTime.Enabled = true;
                    ddlPhysicianName.Enabled = false;
                    ddlVisitType.Enabled = false;
                    ddlDuration.Enabled = false;
                }
                else if (this.Page.Title.Contains("Edit Appointment") == true)
                {
                    DateTimePickerColorChange(this.dtpApptDate, true);
                    TimePickerColorChange(dtpStartTime, true);
                    DisableTableLayout(pnlReschedule);
                    btnFindAvailableSlot.Enabled = false;
                }
                //txtPurposeofVisit.SetTheUBACForDynamicControls();
                //txtNotes.SetTheUBACForDynamicControls();
                #region not been used right now 12-01-2016 by vasanth
                //if (hdnFacilityName.Value != System.Configuration.ConfigurationSettings.AppSettings["CMGFacilityName"])
                //{
                //    pbTestDropDown.Enabled = false;
                //    pbTestDropDown.ImageUrl = "~/Resources/plus_new_disabled.gif"; ;    

                //    pbTestClear.Enabled = false;
                //    pbTestClear.ImageUrl = "~/Resources/close_disabled.png";

                //}
                //if (pbTestDropDown.Enabled == true)
                //{
                //    if (this.Page.Title.Contains("Edit") == true)
                //    {
                //        pbTestDropDown.Enabled = true;
                //        pbTestDropDown.ImageUrl = "~/Resources/pbAdd.png";
                //        pbTestClear.Enabled = true;
                //        pbTestClear.ImageUrl = "~/Resources/close_small_pressed.gif";

                //    }
                //}
                #endregion
                //DateTimePickerColorChange(dtpApptDate, true);
                //TimePickerColorChange(dtpStartTime, true);
                // if (EncRecord.Referring_Physician != string.Empty || EncRecord.PCP_Physician != string.Empty)

                if (EncRecord != null && ulMyEncID != 0)//vasanth
                {
                    HdnRefPhy.Value = EncRecord.Referring_Physician + "|" + EncRecord.Referring_Address + "|" + EncRecord.Referring_Phone_No + "|" + EncRecord.Referring_Fax_No + "|" + EncRecord.Referring_Provider_NPI + "|" + EncRecord.Referring_Facility;
                    HdnPcpPhy.Value = EncRecord.PCP_Physician + "|" + EncRecord.PCP_Address + "|" + EncRecord.PCP_Phone_No + "|" + EncRecord.PCP_Fax_No + "|" + EncRecord.PCP_Provider_NPI + "|" + EncRecord.PCP_Facility;
                }
                pcpDefaultDemographics();
                /*added BugId : 59356*/

            }
            else
            {
                if (chkSelfReferred.Checked == true)
                {
                    imgClearProviderText.Attributes.Remove("onclick");
                }
                // Commented by valli need to check
                if (tabReferringProvAndPCP.SelectedIndex == 0)
                {
                    imgClearProviderText.Style.Add("top", "234px !important");
                    imgEditProvider.Style.Add("top", "234px !important");
                    //if (sAncillary != string.Empty && sAncillary != cboFacility.SelectedItem.Text.Trim())
                    //{
                    var fac = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFac = fac.ToList<FacilityLibrary>();
                    if (ilstFac.Count > 0 && ilstFac[0].Is_Ancillary != "Y")
                    {
                        if (HdnRefPhy != null && HdnRefPhy.Value.Trim() != "" && HdnRefPhy.Value != string.Empty)
                        {
                            // txtReferringProvider.Text = HdnRefPhy.Value.Split('|')[0].ToString();
                            // txtReferingAddress.Text = HdnRefPhy.Value.Split('|')[1].ToString();
                            // msktxtReferingPhoneNo.Text = HdnRefPhy.Value.Split('|')[2].ToString();
                            //  msktxtReferingFaxNo.Text = HdnRefPhy.Value.Split('|')[3].ToString();
                            // txtProviderNPI.Text = HdnRefPhy.Value.Split('|')[4].ToString();
                            // txtReferringFacility.Text = HdnRefPhy.Value.Split('|')[5].ToString();
                        }
                    }

                }
                else if (tabReferringProvAndPCP.SelectedIndex == 1)
                {
                    imgEditProvider.Style.Add("top", "224px !important");
                    imgClearProviderText.Style.Add("top", "224px !important");
                    if (EncRecord != null && EncRecord.PCP_Physician != null && EncRecord.PCP_Physician != "")
                    {
                        //Jira #CAP-69 - labels are missing
                        //hdnpcpprovidersearch.Value = " |" + EncRecord.PCP_Physician + "|" + EncRecord.PCP_Provider_NPI + "|" + "" + "|" + EncRecord.PCP_Facility +
                        //    "|" + EncRecord.PCP_Address + "|"
                        //    + EncRecord.PCP_Fax_No + "|" + EncRecord.PCP_Phone_No;


                        hdnpcpprovidersearch.Value = EncRecord.PCP_Physician + "| NPI: " + EncRecord.PCP_Provider_NPI +
                                 "| Facility: " + EncRecord.PCP_Facility + "| Address:" + EncRecord.PCP_Address + "| Fax No:" + EncRecord.PCP_Fax_No +
                                 "| Phone No:" + EncRecord.PCP_Phone_No;

                    }
                    // if (HdnPcpPhy.Value != null && HdnPcpPhy.Value.Trim() != "")
                    // {
                    //txtReferringProvider.Text = HdnPcpPhy.Value.Split('|')[0].ToString();
                    //txtReferingAddress.Text = HdnPcpPhy.Value.Split('|')[1].ToString();
                    // msktxtReferingPhoneNo.Text = HdnPcpPhy.Value.Split('|')[2].ToString();
                    // msktxtReferingFaxNo.Text = HdnPcpPhy.Value.Split('|')[3].ToString();
                    // txtProviderNPI.Text = HdnPcpPhy.Value.Split('|')[4].ToString();
                    // txtReferringFacility.Text = HdnPcpPhy.Value.Split('|')[5].ToString();
                    // }

                }
            }
            bool issuccess = FillPatientStrip(Convert.ToUInt64(Request["Human_id"]));
         
            if (!issuccess)
            {
                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + Request["Human_id"].ToString() + "','Human','Appointment');", true);


                //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");
                return;
            }
            if (cboFacility.SelectedItem != null && cboFacility.SelectedItem.Text != "")//&& cboFacility.SelectedItem.Text.ToUpper() == sFacilityCmg.ToUpper())
            {
                var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                {
                    hdnFacility.Value = "true";
                }
                else
                {
                    hdnFacility.Value = "false";
                }
            }
            else
            {
                hdnFacility.Value = "false";
            }
            foreach (RadTab rdTab in tabReferringProvAndPCP.Tabs)
            {
                if (rdTab.Text == "Enter PCP info")
                {
                    if (hdnFacility.Value.Trim().ToUpper() == "TRUE")
                    {
                        rdTab.Enabled = false;
                    }
                    else
                    {
                        rdTab.Enabled = true;
                    }

                }
            }

            if (hdnEncounterID.Value != null && hdnEncounterID.Value != "0")
            {
                this.Page.Title = "Edit Appointment" + "-" + ClientSession.UserName;
                btnSave.Enabled = false;
            }
            else
            {
                this.Page.Title = "New Appointment" + "-" + ClientSession.UserName;
            }

            if (MyStatus != null)
            {
                if (MyStatus.ToUpper() != "SCHEDULED" && MyStatus != string.Empty)
                {
                    this.Page.Title = "View Appointment" + "-" + ClientSession.UserName;
                    chkReschedule.Enabled = false;
                    //Jira CAP-2216
                    //chkSelfReferred.Enabled = false;
                    chkShowAllPhysicians.Enabled = false;
                    btnSave.Enabled = false;
                    txtPurposeofVisit.txtDLC.Enabled = false;
                    //    chkWillingnessInCancellation.Enabled = false;
                    txtNotes.txtDLC.Enabled = false;
                    DisableTableLayout(pnlReschedule);
                    chkReschedule.Enabled = false;
                    chkShowAllPhysicians.Enabled = false;
                    //Jira CAP-2216
                    //chkSelfReferred.Enabled = false;
                    btnSave.Enabled = false;
                    DateTimePickerColorChange(this.dtpApptDate, true);
                    TimePickerColorChange(dtpStartTime, true);
                    cboFacility.Enabled = false;
                    //Jira #CAP-158 -  Not able to navigate tab 
                    // pnlScheduleAppointment.Enabled = false;
                    pnlAppointmentDetails.Enabled = false;
                    //Jira CAP-2216
                    //pnlReferringDetails.Enabled = false;
                    cboOrder.Enabled = false;
                    //Jira CAP-2216 - End
                    pnlVisit.Enabled = false;
                    txtProviderSearch.Enabled = false;
                    imgClearProviderText.Visible = false;
                    imgEditProvider.Visible = false;
                    btnPatientDemographics.Enabled = false;
                    btnPatientTask.Enabled = false;
                    tabReferringProvAndPCP.Enabled = true;
                }



            }
            if (chkReschedule.Enabled == false)
            {
                chkReschedule.ForeColor = Color.Gray;
                DisableTableLayout(pnlReschedule);
                btnFindAvailableSlot.Enabled = false;
            }
            if (tabReferringProvAndPCP.SelectedIndex == 0)
            {
                if (hdnrenprovider.Value != "")
                {
                    txtProviderSearch.Enabled = false;
                    imgEditProvider.Style.Add("display","none");
                }
                else
                {
                    txtProviderSearch.Enabled = true;
                    imgEditProvider.Style.Add("display","block");
                }
            }
            else
            {
                if (hdnpcpprovidersearch.Value != "")
                {
                    txtProviderSearch.Enabled = false;
                    imgEditProvider.Style.Add("display","none");
                }
                else
                {
                    txtProviderSearch.Enabled = true;
                    imgEditProvider.Style.Add("display","block");
                }

            }
            //Jira CAP-2216 - Start
            if (Request["EncounterID"] != null && Request["EncounterID"] != string.Empty && hdnCurrentProcess.Value != "ARCHIEVE")
            {
                EnablePhytxtBox(Convert.ToUInt64(Request["EncounterID"]));
            }
            else if (hdnCurrentProcess.Value == "ARCHIEVE")
            {
                chkSelfReferred.Enabled = false;
                pnlReferringDetails.Enabled = false;
            }
            if (chkSelfReferred != null && chkSelfReferred.Visible == true && chkSelfReferred.Checked == true)
            {
                imgClearProviderText.Visible = false;
                imgEditProvider.Visible = false;
            }
            //Jira CAP-2216 - End
            //Jira #CAP-69 - labels are missing
            OverAllPageLoad.Stop();
            time_taken += "OverAllPageLoad : " + OverAllPageLoad.Elapsed.Seconds + "." + OverAllPageLoad.Elapsed.Milliseconds + "s; ";
            hdnTimeTaken.Value = time_taken;
            //CAP-3268
            if (!IsPostBack)
            {
                PhysicianManager physicianManager = new PhysicianManager();
                var lstMappedFacility = physicianManager.GetFacilityListMappedByPhysician(ClientSession.PhysicianId);
                if (lstMappedFacility != null && lstMappedFacility.Any(a => a.Facility_Name == hdnFacilityName.Value && a.Status == "N"))
                {
                    DisablePanelControls(pnlScheduleAppointment);
                    btnPatientTask.Enabled = false;
                    btnPatientDemographics.Enabled = false;
                    btnSave.Enabled = false;
                    hdnHideScheduleAppointment.Value = "true";
                }
            }
            //logger.Debug("--------------------frmEditAppointment Page Load Completed. Time Taken :'" + OverAllPageLoad.Elapsed.Seconds + "." + OverAllPageLoad.Elapsed.Milliseconds + "s'--------------------");
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Appointment", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }
        //Jira CAP-2216
        public void EnablePhytxtBox(ulong ulEncounter_id)
        {
            hdnEnableProviderSearch.Value = "";
            hdnDisableSelfReferred.Value = "";
            if (tabReferringProvAndPCP.SelectedTab.Text != "Enter Ord provider details")
            {
                if (ulEncounter_id > 0 && ApplicationObject.scntab != null)
                {
                    WFObject WFObj = null;
                    WFObj = wfMngr.GetByObjectSystemId(ulEncounter_id, "DOCUMENTATION");

                    var scnid = from sc in ApplicationObject.scntab where sc.SCN_Name == "frmEditAppointment" select sc.SCN_ID;
                    var userPermission = from u in ClientSession.UserPermissionDTO.Userscntab where u.scn_id == Convert.ToUInt64(scnid.ToList<int>()[0]) && u.user_name == ClientSession.UserName && u.Permission == "U" select u;
                    
                    if (userPermission.ToList<user_scn_tab>().Count > 0)
                    {
                        bool EnableProviderSearch = false;


                        if (WFObj != null && WFObj.Id > 0 && WFObj.Current_Process == "DOCUMENT_COMPLETE")
                        {
                            txtProviderSearch.Enabled = false;
                            imgClearProviderText.Visible = false;
                            imgEditProvider.Visible = false;
                            imgEditProvider.Style.Add("display", "none");
                            imgClearProviderText.Attributes.Remove("onclick");
                            hdnDisableSelfReferred.Value = "true";
                            EnableProviderSearch = false;
                            hdnEnableProviderSearch.Value = "false";


                        }
                        else
                        {
                            if (txtProviderSearch.Text == "")
                            {
                                txtProviderSearch.Enabled = true;
                                imgEditProvider.Visible = true;
                                imgEditProvider.Style.Add("display", "block");
                            }
                            else {
                                txtProviderSearch.Enabled = false;
                                imgEditProvider.Visible = true;
                                imgEditProvider.Style.Add("display", "none");
                            }
                            imgClearProviderText.Visible = true;
                            //imgEditProvider.Disabled = false;
                            imgClearProviderText.Attributes.Add("onclick", "return ProviderSearchclear();");
                            hdnDisableSelfReferred.Value = "false";
                            EnableProviderSearch = true;
                            hdnEnableProviderSearch.Value = "true";

                        }
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "EnableProviderSearch", "EnableProviderSearch('" + EnableProviderSearch.ToString().ToLower() + "');", true);
                    }

                }
            }
        }
            
        public void hdnbtngeneratexml_Click(object sender, EventArgs e)
        {

            //  Patientchartload();
        }
        public bool FillPatientStrip(ulong humanID)
        {
            bool issuccess = true;
            string sdivPatientstrip = UtilityManager.FillPatientStrip(humanID);
            if (sdivPatientstrip != null)
            {
                divPatientstrip.InnerText = sdivPatientstrip;
            }


            // Assign PatientStrip Values
            else
            {
                issuccess = false;
            }
            //string FileName = "Human" + "_" + humanID + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //try
            //{
            //    if (File.Exists(strXmlFilePath) == true)
            //    {
            //        XmlDocument itemDoc = new XmlDocument();
            //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //        {
            //            try
            //            {
            //                itemDoc.Load(fs);

            //                XmlNodeList xmlhumanList = itemDoc.GetElementsByTagName("Human");
            //                Human objFillHuman = new Human();
            //                IList<Human> lstHuman = new List<Human>();
            //                if (xmlhumanList != null && xmlhumanList.Count > 0)
            //                {
            //                    objFillHuman.Id = Convert.ToUInt64(xmlhumanList[0].Attributes.GetNamedItem("Id").Value);
            //                    objFillHuman.Birth_Date = Convert.ToDateTime(xmlhumanList[0].Attributes.GetNamedItem("Birth_Date").Value);
            //                    objFillHuman.First_Name = xmlhumanList[0].Attributes.GetNamedItem("First_Name").Value;
            //                    objFillHuman.Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Last_Name").Value;
            //                    objFillHuman.MI = xmlhumanList[0].Attributes.GetNamedItem("MI").Value;
            //                    objFillHuman.Sex = xmlhumanList[0].Attributes.GetNamedItem("Sex").Value;
            //                    objFillHuman.Suffix = xmlhumanList[0].Attributes.GetNamedItem("Suffix").Value;
            //                    objFillHuman.Medical_Record_Number = xmlhumanList[0].Attributes.GetNamedItem("Medical_Record_Number").Value;
            //                    objFillHuman.Home_Phone_No = xmlhumanList[0].Attributes.GetNamedItem("Home_Phone_No").Value;
            //                    objFillHuman.Human_Type = xmlhumanList[0].Attributes.GetNamedItem("Human_Type").Value;
            //                    objFillHuman.Patient_Account_External = xmlhumanList[0].Attributes.GetNamedItem("Patient_Account_External").Value;
            //                    objFillHuman.Cell_Phone_Number = xmlhumanList[0].Attributes.GetNamedItem("Cell_Phone_Number").Value;
            //                    if (xmlhumanList[0].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value != null && xmlhumanList[0].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value != string.Empty)
            //                        objFillHuman.ACO_Is_Eligible_Patient = xmlhumanList[0].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value.ToString();
            //                    else
            //                        objFillHuman.ACO_Is_Eligible_Patient = "";
            //                    lstHuman.Add(objFillHuman);
            //                }

            //                string phoneno = "";

            //                if (lstHuman != null && lstHuman.Count > 0)
            //                {

            //                    if (objFillHuman.Home_Phone_No.Length == 14)
            //                    {
            //                        phoneno = objFillHuman.Home_Phone_No;
            //                    }
            //                    else
            //                    {
            //                        phoneno = objFillHuman.Cell_Phone_Number;
            //                    }

            //                }

            //                string sPatientSex = string.Empty;

            //                if (objFillHuman.Sex != string.Empty)
            //                {
            //                    if (objFillHuman.Sex.Substring(0, 1).ToUpper() == "U")
            //                    {
            //                        sPatientSex = "UNK";
            //                    }
            //                    else
            //                    {
            //                        sPatientSex = objFillHuman.Sex.Substring(0, 1);
            //                    }
            //                }
            //                else
            //                {
            //                    sPatientSex = "";
            //                }

            //                string sAcoEligiblePatient = string.Empty;
            //                sAcoEligiblePatient = objFillHuman.ACO_Is_Eligible_Patient;

            //                if (lstHuman != null && lstHuman.Count > 0)
            //                {
            //                    divPatientstrip.InnerText = objFillHuman.Last_Name + "," + objFillHuman.First_Name +
            //               "  " + objFillHuman.MI + "  " + objFillHuman.Suffix + "   |   " +
            //                objFillHuman.Birth_Date.ToString("dd-MMM-yyyy") + "   |   " +
            //               (CalculateAge(objFillHuman.Birth_Date)).ToString() +
            //               "  year(s)    |   " + sPatientSex + "   |   Acc #:" + humanID +
            //               "   |   " + "Med Rec #:" + objFillHuman.Medical_Record_Number + "   |   " +
            //               "Phone #:" + phoneno + "   |   Patient Type:" + objFillHuman.Human_Type + "   |   ";
            //                }
            //                else
            //                {
            //                    divPatientstrip.InnerText = " " + "   |" + "|" + "|" + "|" + "|";
            //                }

            //                if (sAcoEligiblePatient != null && sAcoEligiblePatient != string.Empty && sAcoEligiblePatient != "N")
            //                {
            //                    divPatientstrip.InnerText += sAcoEligiblePatient + "   |   ";
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                issuccess = false;
            //                return issuccess;
            //            }
            //            fs.Close();
            //            fs.Dispose();
            //        }
            //    }

            //    else
            //    {
            //        divPatientstrip.InnerText = " " + "   |" + "|" + "|" + "|" + "|";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message + " - " + strXmlFilePath);
            //}

            return issuccess;
        }


        //Added
        public int CalculateAge(DateTime birthDate)
        {
            // cache the current time
            DateTime now = DateTime.Today; // today is fine, don't need the timestamp from now
            // get the difference in years
            int years = 0;
            if (birthDate != null)
                years = now.Year - birthDate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;

            return years;
        }
        //End






        private void Page_PreRender(object sender, System.EventArgs e)
        {
            txtNotes.txtDLC.Enabled = true;
            txtPurposeofVisit.txtDLC.Enabled = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //logger.Debug("--------------------frmEditAppointment Save Click Event Started--------------------");
            hdnConfirmBlockAppointment.Value = "";
            System.Diagnostics.Stopwatch SaveTime = new System.Diagnostics.Stopwatch();
            SaveTime.Start();
            SaveAppointment("SCHEDULED");
            SaveTime.Stop();
            divLoading.Style.Add("display", "none");
            //lblSave.Text = "Save Time -Minutes: " + SaveTime.Elapsed.Minutes.ToString() + " Seconds :" + SaveTime.Elapsed.Seconds.ToString() + " MilliSec : " + SaveTime.Elapsed.Milliseconds.ToString();
            //logger.Debug("--------------------frmEditAppointment Save Click Event Completed. Time Taken :'" + SaveTime.Elapsed.Seconds + "." + SaveTime.Elapsed.Milliseconds + "s'--------------------");
        }

        protected void btnConfirmAppointment_Click(object sender, EventArgs e)
        {
            //logger.Debug("--------------------frmEditAppointment btnConfirmAppointment Click Event Started--------------------");
            Stopwatch ConfirmAppointmentTime = new Stopwatch();
            ConfirmAppointmentTime.Start();
            DateTime selectedDate = DateTime.Now;
            string[] sp = null;

            if (dtpApptDate.SelectedDate.Value != null && dtpApptDate.SelectedDate.Value != DateTime.MinValue)
            {
                try
                {
                    selectedDate = Convert.ToDateTime(dtpApptDate.SelectedDate.Value);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Calender Selectde Date of Value='" + dtpApptDate.SelectedDate.Value + "' to DateTime threw an error.", exp);
                    throw (exp);
                }
                sp = selectedDate.ToString().Split(' ');
            }

            DateTime MyCalendarDateTime;
            try
            {
                MyCalendarDateTime = Convert.ToDateTime(dtpApptDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + ":" + dtpStartTime.SelectedTime.Value.Seconds);
            }
            catch (Exception exp)
            {
                //logger.Debug("Conversion of Appointment Date Time of value='" + dtpApptDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + dtpStartTime.SelectedTime.Value.Seconds + "' to DateTime threw an error.", exp);
                throw (exp);
            }

            ulong HumanID = 0, PhysicianID = 0, EncounterID = 0;
            int Duration = 0;
            try
            {
                //logger.Debug("Human_ID from hdnHumanID.Value='" + hdnHumanID.Value + "'");
                HumanID = Convert.ToUInt64(hdnHumanID.Value);
            }
            catch (Exception exp)
            {
                //logger.Debug("Conversion of Human_ID from hdnHumanID.Value='" + hdnHumanID.Value + "' to UInt threw an error.", exp);
                throw (exp);
            }
            try
            {
                //logger.Debug("PhysicianID from ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value='" + ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value + "'");
                PhysicianID = Convert.ToUInt64(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value);
            }
            catch (Exception exp)
            {
                //logger.Debug("Conversion of PhysicianID from ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value='" + ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value + "' to UInt threw an error.", exp);
                throw (exp);
            }
            try
            {
                //logger.Debug("Duration from ddlDuration.Text='" + ddlDuration.Text + "'");
                Duration = Convert.ToInt16(ddlDuration.Text);
            }
            catch (Exception exp)
            {
                //logger.Debug("Conversion of Duration from ddlDuration.Text='" + ddlDuration.Text + "' to UInt threw an error.", exp);
                // throw (exp);
            }
            try
            {
                //logger.Debug("EncounterID from hdnEncounterID.Value='" + hdnEncounterID.Value + "'");
                EncounterID = Convert.ToUInt64(hdnEncounterID.Value);
            }
            catch (Exception exp)
            {
                //logger.Debug("Conversion of EncounterID from hdnEncounterID.Value='" + hdnEncounterID.Value + "' to UInt threw an error.", exp);
                throw (exp);
            }
            //logger.Debug("CheckDuplicateAppointment DB Call Starting");
            Stopwatch CheckDuplicateAppointmentDBCall = new Stopwatch();
            CheckDuplicateAppointmentDBCall.Start();
            AppointmentPreChecks appointmentPreCheckList = EncMngr.CheckDuplicateAppointment(HumanID, PhysicianID, MyCalendarDateTime, cboFacility.Text, selectedDate, dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + ":" + dtpStartTime.SelectedTime.Value.Seconds, Duration, EncounterID);
            CheckDuplicateAppointmentDBCall.Stop();
            //logger.Debug("CheckDuplicateAppointment DB Call Completed. Time Taken : " + CheckDuplicateAppointmentDBCall.Elapsed.Seconds + "." + CheckDuplicateAppointmentDBCall.Elapsed.Milliseconds + "s.");

            Encounter EncRecord = new Encounter();
            WFObject WFObj = new WFObject();

            FacList = ApplicationObject.facilityLibraryList.Where(item => item.Fac_Name.Trim().ToUpper() == cboFacility.Text.Trim().ToUpper()).ToList<FacilityLibrary>();//FacMngr.GetFacilityListByFacilityName(cboFacility.Text);
            if (FacList != null && FacList.Count > 0)
            {
                hdnPOS.Value = FacList[0].POS;
            }
            if (hdnEncounterID.Value != null && hdnEncounterID.Value.ToString() != "0")
            {
                EncRecord = appointmentPreCheckList.CurrentEncounter[0];
            }
            //UtilityManager utilityMngr = new UtilityManager();

            //For Edit Appointment

            EncRecord.Purpose_of_Visit = txtPurposeofVisit.txtDLC.Text;
            EncRecord.Notes = txtNotes.txtDLC.Text;

            if (EncRecord != null && EncRecord.Id != 0 && chkReschedule.Checked == false)
            {
                string tt = "";
                try
                {
                    try
                    {
                        tt = Convert.ToDateTime(dtpStartTime.DbSelectedDate).ToString("tt");
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Appointment Time of value='" + dtpStartTime.DbSelectedDate + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    if (dtpStartTime.SelectedTime.Value != null)
                        EncRecord.Appointment_Date = Convert.ToDateTime(sp[0] + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + tt);
                    EncRecord.Appointment_Date = UtilityManager.ConvertToUniversal(EncRecord.Appointment_Date);// change to universal
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Appointment Date of value='" + sp[0] + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + Convert.ToDateTime(dtpStartTime.DbSelectedDate).ToString("tt") + "' to DateTime threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    EncRecord.Duration_Minutes = Convert.ToInt32(ddlDuration.Text);
                    //Cap - 3217
                    if (IsAuthVerified.Checked == true)
                    {
                        EncRecord.Is_Auth_Verified = "Y";
                    }
                    else
                    {
                        EncRecord.Is_Auth_Verified = "N";
                    }
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Duration_Minutes of value='" + ddlDuration.Text + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                EncRecord.Purpose_of_Visit = txtPurposeofVisit.txtDLC.Text;
                try
                {
                    //if (sFacilityCmg.ToUpper() == cboFacility.SelectedItem.Text.ToUpper())
                    //{
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                    {
                        if (ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value != null)
                            EncRecord.Machine_Technician_Library_ID = Convert.ToInt32(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value);
                        EncRecord.Appointment_Provider_ID = GetPhysicianLibIDByTechID(EncRecord.Machine_Technician_Library_ID);
                    }
                    else
                        EncRecord.Appointment_Provider_ID = Convert.ToInt32(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value); //ulMyPhysicianID;
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Duration_Minutes of value='" + ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value + "' to Int threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    //if (sFacilityCmg.ToUpper() == cboFacility.SelectedItem.Text.ToUpper())
                    //{
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                    {
                        ulMyPhysicianID = PhysicianId(Convert.ToUInt64(EncRecord.Machine_Technician_Library_ID)); //Convert.ToUInt64(EncRecord.Machine_Technician_Library_ID);
                    }
                    else
                        ulMyPhysicianID = Convert.ToUInt64(EncRecord.Appointment_Provider_ID);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of PhysicianID of value='" + EncRecord.Appointment_Provider_ID + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    EncRecord.Encounter_Provider_ID = Convert.ToInt32(ulMyPhysicianID);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of PhysicianID of value='" + ulMyPhysicianID + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                EncRecord.Visit_Type = ddlVisitType.Text;
                try
                {
                    EncRecord.Human_ID = Convert.ToUInt64(txtPatientAccountNumber.Text);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of HumanID of value='" + txtPatientAccountNumber.Text + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                if (cboFacility.Text != EncRecord.Facility_Name)
                {
                    WFObj = wfMngr.GetByObjectSystemId(EncRecord.Id, sMyObjType);
                    WFObj.Fac_Name = cboFacility.Text;
                    EditWFobj = true;
                }
                EncRecord.Facility_Name = cboFacility.Text;
                EncRecord.Notes = txtNotes.txtDLC.Text;
                //EncRecord.Is_Batch_Created = "N";
                //EncRecord.Is_EandM_Submitted = "N";
                if (tabReferringProvAndPCP.SelectedIndex == 0)
                {

                    // Commented by valli

                    // EncRecord.Referring_Facility = txtReferringFacility.Text.Trim();
                    // EncRecord.Referring_Physician = txtReferringProvider.Text;
                    //  EncRecord.Referring_Address = txtReferingAddress.Text.Trim();
                    //  EncRecord.Referring_Phone_No = msktxtReferingPhoneNo.TextWithLiterals;
                    // EncRecord.Referring_Fax_No = msktxtReferingFaxNo.TextWithLiterals;
                    //  EncRecord.Referring_Provider_NPI = txtProviderNPI.Text.Trim();
                    if (hdnpcpprovider.Value != null && hdnpcpprovider.Value != "" && hdnpcpprovider.Value != "|||||")
                    {
                        //Jira #CAP-69 - labels are missing
                        //EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[3].ToString();
                        //EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        //EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[4].ToString();
                        //EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[6].ToString();
                        //EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].ToString();
                        //EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].ToString();

                        EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[3].Split(':')[1].ToString();
                        EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                    }
                    else
                    {
                        EncRecord.PCP_Facility = "";
                        EncRecord.PCP_Physician = "";
                        EncRecord.PCP_Address = "";
                        EncRecord.PCP_Phone_No = "";
                        EncRecord.PCP_Fax_No = "";
                        EncRecord.PCP_Provider_NPI = "";
                    }
                    if (!chkSelfReferred.Checked)
                    {
                        if (hdnrenprovider.Value != null && hdnrenprovider.Value.Trim() != "" && hdnrenprovider.Value != "|||||")
                        {
                            EncRecord.Is_Self_Referred = "N";

                            EncRecord.Referring_Facility = hdnrenprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Physician = hdnrenprovider.Value.Split('|')[0].ToString();
                            EncRecord.Referring_Address = hdnrenprovider.Value.Split('|')[3].Split(':')[1].ToString();
                            EncRecord.Referring_Phone_No = hdnrenprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Fax_No = hdnrenprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();


                        }
                        else
                        {
                            EncRecord.Is_Self_Referred = "N";
                            EncRecord.Referring_Facility = "";
                            EncRecord.Referring_Physician = "";
                            EncRecord.Referring_Address = "";
                            EncRecord.Referring_Phone_No = "";
                            EncRecord.Referring_Fax_No = "";
                            EncRecord.Referring_Provider_NPI = "";
                        }
                    }
                    else
                    {
                        EncRecord.Is_Self_Referred = "Y";
                        EncRecord.Referring_Facility = "";
                        EncRecord.Referring_Physician = "";
                        EncRecord.Referring_Address = "";
                        EncRecord.Referring_Phone_No = "";
                        EncRecord.Referring_Fax_No = "";
                        EncRecord.Referring_Provider_NPI = "";
                    }
                }
                else if (tabReferringProvAndPCP.SelectedIndex == 1)
                {

                    // Commented by valli

                    if (hdnpcpprovider.Value != null && hdnpcpprovider.Value != "" && hdnpcpprovider.Value != "|||||")
                    {

                        //Jira #CAP-69 - labels are missing
                        //EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[3].ToString();
                        //EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        //EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[4].ToString();
                        //EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[6].ToString();
                        //EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].ToString();
                        //EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].ToString();

                        EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[3].Split(':')[1].ToString();
                        EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();


                    }
                    else
                    {
                        EncRecord.PCP_Facility = "";
                        EncRecord.PCP_Physician = "";
                        EncRecord.PCP_Address = "";
                        EncRecord.PCP_Phone_No = "";
                        EncRecord.PCP_Fax_No = "";
                        EncRecord.PCP_Provider_NPI = "";
                    }
                    if (!chkSelfReferred.Checked)
                    {
                        if (hdnrenprovider.Value != null && hdnrenprovider.Value.Trim() != "" && hdnrenprovider.Value != "|||||")
                        {
                            EncRecord.Is_Self_Referred = "N";
                            //Jira #CAP-69 - labels are missing
                            //EncRecord.Referring_Facility = hdnrenprovider.Value.Split('|')[4].ToString();//vasanth for Saving Both ref And Pcp
                            //EncRecord.Referring_Physician = hdnrenprovider.Value.Split('|')[1].ToString();
                            //EncRecord.Referring_Address = hdnrenprovider.Value.Split('|')[5].ToString();
                            //EncRecord.Referring_Phone_No = hdnrenprovider.Value.Split('|')[7].ToString();
                            //EncRecord.Referring_Fax_No = hdnrenprovider.Value.Split('|')[6].ToString();
                            //EncRecord.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[2].ToString();

                            EncRecord.Referring_Facility = hdnrenprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Physician = hdnrenprovider.Value.Split('|')[0].ToString();
                            EncRecord.Referring_Address = hdnrenprovider.Value.Split('|')[3].Split(':')[1].ToString();
                            EncRecord.Referring_Phone_No = hdnrenprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Fax_No = hdnrenprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                        }
                        else
                        {
                            EncRecord.Is_Self_Referred = "N";
                            EncRecord.Referring_Facility = "";
                            EncRecord.Referring_Physician = "";
                            EncRecord.Referring_Address = "";
                            EncRecord.Referring_Phone_No = "";
                            EncRecord.Referring_Fax_No = "";
                            EncRecord.Referring_Provider_NPI = "";
                        }
                    }
                    else
                    {
                        EncRecord.Is_Self_Referred = "Y";
                        EncRecord.Referring_Facility = "";
                        EncRecord.Referring_Physician = "";
                        EncRecord.Referring_Address = "";
                        EncRecord.Referring_Phone_No = "";
                        EncRecord.Referring_Fax_No = "";
                        EncRecord.Referring_Provider_NPI = "";
                    }
                    //EncRecord.PCP_Facility = txtReferringFacility.Text.Trim();
                    //EncRecord.PCP_Physician = txtReferringProvider.Text;
                    //EncRecord.PCP_Address = txtReferingAddress.Text.Trim();
                    //EncRecord.PCP_Phone_No = msktxtReferingPhoneNo.TextWithLiterals;
                    //EncRecord.PCP_Fax_No = msktxtReferingFaxNo.TextWithLiterals;
                    //EncRecord.PCP_Provider_NPI = txtProviderNPI.Text.Trim();

                }
                EncRecord.Notes = txtNotes.txtDLC.Text;
                //Cap - 2505
                //if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty)
                if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty && cboOrder.SelectedItem.Text != "Akido Order")
                {

                    EncRecord.Order_Submit_ID = Convert.ToInt32(cboOrder.SelectedItem.Text.Split('|')[0]);
                }
                else
                    EncRecord.Order_Submit_ID = 0;
                //  EncRecord.Is_Encounter_SuperBill = "N";

                // Commented by valli

                //if (chkWillingnessInCancellation.Checked == true)
                //{
                //    EncRecord.Willing_For_Prior_Appointment = "Y";

                //}
                //else
                //{
                //    EncRecord.Willing_For_Prior_Appointment = "N";

                //}
                //Added by Bala for ADD MESSAGES IN APPOINTMENTS in 20-12-2013
                if (txtNotes.txtDLC.Text != string.Empty)
                {
                    PatientNotesManager patNotesMngr = new PatientNotesManager();
                    IList<PatientNotes> listPatientNotes = new List<PatientNotes>();
                    PatientNotes objPat = new PatientNotes();
                    try
                    {
                        objPat.Human_ID = Convert.ToUInt64(txtPatientAccountNumber.Text);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of HumanID of value='" + txtPatientAccountNumber.Text + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Message_Orign = "Appointment";
                    if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    {
                        try
                        {
                            objPat.Message_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                            throw (exp);
                        }
                    }

                    objPat.Message_Description = "Appointment";
                    objPat.Notes = txtNotes.txtDLC.Text;
                    if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    {
                        try
                        {
                            objPat.Created_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                            throw (exp);
                        }
                    }

                    objPat.Created_By = ClientSession.UserName;
                    //objPat.Modified_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    objPat.Is_PatientChart = "N";
                    objPat.Line_ID = 0;
                    objPat.Type = "MESSAGE";
                    try
                    {
                        objPat.Encounter_ID = Convert.ToInt32(ulMyEncID);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Encounter_ID of value='" + ulMyEncID + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Statement_ChargeLine_ID = 0;
                    try
                    {
                        objPat.SourceID = Convert.ToInt32(ulMyEncID);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Encounter_ID of value='" + ulMyEncID + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Source = "APPOINTMENT";
                    objPat.IsDelete = "N";
                    objPat.Is_PopupEnable = "N";
                    objPat.Priority = "NORMAL";
                    listPatientNotes.Add(objPat);
                    //logger.Debug("AddToPatientNotes DB Call Starting");
                    Stopwatch AddToPatientNotesDBCall = new Stopwatch();
                    AddToPatientNotesDBCall.Start();
                    patNotesMngr.AddToPatientNotes(listPatientNotes);
                    AddToPatientNotesDBCall.Stop();
                    //logger.Debug("AddToPatientNotes DB Call Completed. Time Taken : " + AddToPatientNotesDBCall.Elapsed.Seconds + "." + AddToPatientNotesDBCall.Elapsed.Milliseconds);
                }
                if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                {
                    try
                    {
                        EncRecord.Modified_Date_and_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                }
                EncRecord.Modified_By = ClientSession.UserName;
                //logger.Debug("UpdateEncounterForRCM DB Call Starting");
                object[] temp = new object[] { "false" };
                Stopwatch UpdateEncounterForRCMDBCall = new Stopwatch();
                IList<WFObject> lstWFobj = new List<WFObject>();
                if (EditWFobj)
                {
                    lstWFobj.Add(WFObj);
                }
                UpdateEncounterForRCMDBCall.Start();
                if (EncRecord != null)

                    EncMngr.UpdateEncounterForRCM(EncRecord, lstWFobj, EditWFobj, string.Empty, "", temp);


                // EncMngr.UpdateEncounterForRCM(EncRecord, lstWFobj, EditWFobj, string.Empty, txtAuthorizationNo.Text, temp);
                EncMngr.TriggerSPforProvReviewStatusTracker("PROVIDER_CHANGE", EncRecord.Id);//SP checks if prov is changed..
                // EncMngr.UpdateEncounterForRCM(EncRecord, string.Empty, txtAuthorizationNo.Text, temp);// GenerateIsCMGOrderObject()////not been used right now 03-02-2016 by vasanth
                UpdateEncounterForRCMDBCall.Stop();
                //logger.Debug("UpdateEncounterForRCM DB Call Completed. Time taken : " + UpdateEncounterForRCMDBCall.Elapsed.Seconds + "." + UpdateEncounterForRCMDBCall.Elapsed.Milliseconds + "s.");
            }

            //For Reschedule
            if (EncRecord.Id != 0 && chkReschedule.Checked == true)
            {
                //if (rchtxtReasonText.Text.Trim() == string.Empty)
                //{
                //    ApplicationObject.erroHandler.DisplayErrorMessage("110048", this.Text);
                //    return;
                //}
                EncRecord.Reschedule_Reason_Code = ddlReasonCode.Text;
                EncRecord.Reschedule_Reason_Text = txtReasonCode.Text;
                EncRecord.Notes = txtNotes.txtDLC.Text;
                EncRecord.Visit_Type = ddlVisitType.Text;

                if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                {
                    try
                    {
                        EncRecord.Modified_Date_and_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                }
                EncRecord.Modified_By = ClientSession.UserName;

                //logger.Debug("UpdateEncounterForRCM DB Call Starting");
                object[] temp = new object[] { "false" };
                Stopwatch UpdateEncounterForRCMDBCall = new Stopwatch();
                UpdateEncounterForRCMDBCall.Start();
                if (EncRecord != null)
                    EncMngr.UpdateEncounterForRCM(EncRecord, null, false, string.Empty, "", temp);
                //  EncMngr.UpdateEncounterForRCM(EncRecord, null, false, string.Empty, txtAuthorizationNo.Text, temp);
                EncMngr.TriggerSPforProvReviewStatusTracker("PROVIDER_CHANGE", EncRecord.Id);//SP checks if prov is changed..
                // EncMngr.UpdateEncounterForRCM(EncRecord, string.Empty, txtAuthorizationNo.Text, temp);// GenerateIsCMGOrderObject()////not been used right now 03-02-2016 by vasanth
                UpdateEncounterForRCMDBCall.Stop();
                //logger.Debug("UpdateEncounterForRCM DB Call Completed. Time taken : " + UpdateEncounterForRCMDBCall.Elapsed.Seconds + "." + UpdateEncounterForRCMDBCall.Elapsed.Milliseconds + "s.");

                //logger.Debug("MoveToNextProcess DB Call Starting");
                Stopwatch MoveToNextProcessDBCall = new Stopwatch();
                MoveToNextProcessDBCall.Start();
                wfMngr.MoveToNextProcess(EncRecord.Id, "ENCOUNTER", 4, "UNKNOWN", System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty, null, null);
                EncMngr.TriggerSPforProvReviewStatusTracker("INVALID", EncRecord.Id);//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
                MoveToNextProcessDBCall.Stop();
                //logger.Debug("MoveToNextProcess DB Call Completed. Time taken : " + MoveToNextProcessDBCall.Elapsed.Seconds + "." + MoveToNextProcessDBCall.Elapsed.Milliseconds + "s.");

                Encounter NewEnc = new Encounter();
                string tt1 = "";
                try
                {
                    try
                    {
                        tt1 = Convert.ToDateTime(dtpStartTime.DbSelectedDate).ToString("tt");
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Appointment Time of value='" + dtpStartTime.DbSelectedDate + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    if (dtpStartTime.SelectedTime.Value != null)
                        NewEnc.Appointment_Date = Convert.ToDateTime(sp[0] + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + tt1);
                    NewEnc.Appointment_Date = UtilityManager.ConvertToUniversal(NewEnc.Appointment_Date);// change to universal
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Appointment Date of value='" + sp[0] + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + Convert.ToDateTime(dtpStartTime.DbSelectedDate).ToString("tt") + "' to DateTime threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    NewEnc.Duration_Minutes = Convert.ToInt32(ddlDuration.Text);
                    //Cap - 3217
                    if (IsAuthVerified.Checked == true)
                    {
                        NewEnc.Is_Auth_Verified = "Y";
                    }
                    else
                    {
                        NewEnc.Is_Auth_Verified = "N";
                    }
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Duration_Minutes of value='" + ddlDuration.Text + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                NewEnc.Purpose_of_Visit = txtPurposeofVisit.txtDLC.Text;
                try
                {
                    //if (sFacilityCmg.ToUpper() == cboFacility.SelectedItem.Text.ToUpper())
                    //{
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                    {
                        NewEnc.Machine_Technician_Library_ID = Convert.ToInt32(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value);
                        NewEnc.Appointment_Provider_ID = GetPhysicianLibIDByTechID(NewEnc.Machine_Technician_Library_ID);
                    }
                    else
                        NewEnc.Appointment_Provider_ID = Convert.ToInt32(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value); //ulMyPhysicianID;
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Duration_Minutes of value='" + ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value + "' to Int threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    //if (sFacilityCmg.ToUpper() == cboFacility.SelectedItem.Text.ToUpper())
                    //{
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                    {
                        ulMyPhysicianID = Convert.ToUInt64(NewEnc.Machine_Technician_Library_ID);
                    }
                    else
                        ulMyPhysicianID = Convert.ToUInt64(NewEnc.Appointment_Provider_ID);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of PhysicianID of value='" + NewEnc.Appointment_Provider_ID + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    NewEnc.Encounter_Provider_ID = Convert.ToInt32(ulMyPhysicianID);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of PhysicianID of value='" + ulMyPhysicianID + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                NewEnc.Visit_Type = ddlVisitType.Text;
                try
                {
                    NewEnc.Human_ID = Convert.ToUInt64(txtPatientAccountNumber.Text);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of HumanID of value='" + txtPatientAccountNumber.Text + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                NewEnc.Facility_Name = cboFacility.Text;
                NewEnc.Is_Batch_Created = "N";
                NewEnc.Is_EandM_Submitted = "N";
                if (tabReferringProvAndPCP.SelectedIndex == 0)
                {


                    // Commennted by valli need to check
                    //NewEnc.Referring_Facility = txtReferringFacility.Text.Trim();
                    //NewEnc.Referring_Physician = txtReferringProvider.Text;
                    //NewEnc.Referring_Address = txtReferingAddress.Text.Trim();
                    //NewEnc.Referring_Phone_No = msktxtReferingPhoneNo.TextWithLiterals;
                    //NewEnc.Referring_Fax_No = msktxtReferingFaxNo.TextWithLiterals;
                    //NewEnc.Referring_Provider_NPI = txtProviderNPI.Text.Trim();

                    if (hdnpcpprovider.Value != "" && hdnpcpprovider.Value != null && hdnpcpprovider.Value != "|||||")
                    {
                        //Jira #CAP-69 - labels are missing
                        //NewEnc.PCP_Facility = hdnpcpprovider.Value.Split('|')[4].ToString();
                        //NewEnc.PCP_Physician = hdnpcpprovider.Value.Split('|')[1].ToString();
                        //NewEnc.PCP_Address = hdnpcpprovider.Value.Split('|')[5].ToString();
                        //NewEnc.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[7].ToString();
                        //NewEnc.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[6].ToString();
                        //NewEnc.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[2].ToString();

                        NewEnc.PCP_Facility = hdnpcpprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        NewEnc.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        NewEnc.PCP_Address = hdnpcpprovider.Value.Split('|')[3].Split(':')[1].ToString();
                        NewEnc.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        NewEnc.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                        NewEnc.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();


                    }
                    if (hdnrenprovider.Value != null && hdnrenprovider.Value.Trim() != "" && hdnrenprovider.Value != "|||||")
                    {
                        NewEnc.Referring_Facility = hdnrenprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        NewEnc.Referring_Physician = hdnrenprovider.Value.Split('|')[0].ToString();
                        NewEnc.Referring_Address = hdnrenprovider.Value.Split('|')[3].Split(':')[1].ToString();
                        NewEnc.Referring_Phone_No = hdnrenprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        NewEnc.Referring_Fax_No = hdnrenprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                        NewEnc.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                    }
                }
                else if (tabReferringProvAndPCP.SelectedIndex == 1)
                {
                    //NewEnc.PCP_Facility = txtReferringFacility.Text.Trim();
                    //NewEnc.PCP_Physician = txtReferringProvider.Text;
                    //NewEnc.PCP_Address = txtReferingAddress.Text.Trim();
                    //NewEnc.PCP_Phone_No = msktxtReferingPhoneNo.TextWithLiterals;
                    //NewEnc.PCP_Fax_No = msktxtReferingFaxNo.TextWithLiterals;
                    //NewEnc.PCP_Provider_NPI = txtProviderNPI.Text.Trim();
                    if (hdnpcpprovider.Value != "" && hdnpcpprovider.Value != null && hdnpcpprovider.Value != "|||||")
                    {
                        //Jira #CAP-69 - labels are missing
                        //NewEnc.PCP_Facility = hdnpcpprovider.Value.Split('|')[4].ToString();
                        //NewEnc.PCP_Physician = hdnpcpprovider.Value.Split('|')[1].ToString();
                        //NewEnc.PCP_Address = hdnpcpprovider.Value.Split('|')[5].ToString();
                        //NewEnc.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[7].ToString();
                        //NewEnc.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[6].ToString();
                        //NewEnc.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[2].ToString();

                        NewEnc.PCP_Facility = hdnpcpprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        NewEnc.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        NewEnc.PCP_Address = hdnpcpprovider.Value.Split('|')[3].Split(':')[1].ToString();
                        NewEnc.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        NewEnc.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                        NewEnc.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                    }
                    if (hdnrenprovider.Value != null && hdnrenprovider.Value.Trim() != "" && hdnrenprovider.Value != "|||||")
                    {
                        NewEnc.Referring_Facility = hdnrenprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        NewEnc.Referring_Physician = hdnrenprovider.Value.Split('|')[0].ToString();
                        NewEnc.Referring_Address = hdnrenprovider.Value.Split('|')[3].Split(':')[1].ToString();
                        NewEnc.Referring_Phone_No = hdnrenprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        NewEnc.Referring_Fax_No = hdnrenprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                        NewEnc.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                    }
                }

                NewEnc.Notes = txtNotes.txtDLC.Text;
                //Added by Bala for ADD MESSAGES IN APPOINTMENTS in 20-12-2013
                if (txtNotes.txtDLC.Text != string.Empty)
                {
                    PatientNotesManager patNotesMngr = new PatientNotesManager();
                    IList<PatientNotes> listPatientNotes = new List<PatientNotes>();
                    PatientNotes objPat = new PatientNotes();
                    objPat.Human_ID = Convert.ToUInt64(txtPatientAccountNumber.Text);
                    objPat.Message_Orign = "Appointment";
                    if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    {
                        try
                        {
                            objPat.Message_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                            throw (exp);
                        }
                    }

                    objPat.Message_Description = "Appointment";
                    objPat.Notes = txtNotes.txtDLC.Text;
                    if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    {
                        try
                        {
                            objPat.Created_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                            throw (exp);
                        }
                    }

                    if (ClientSession.UserName != null)
                        objPat.Created_By = ClientSession.UserName;
                    ///objPat.Modified_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    objPat.Is_PatientChart = "N";
                    objPat.Line_ID = 0;
                    objPat.Type = "MESSAGE";
                    try
                    {
                        objPat.Encounter_ID = Convert.ToInt32(ulMyEncID);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Encounter_ID of value='" + ulMyEncID + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Statement_ChargeLine_ID = 0;
                    try
                    {
                        objPat.SourceID = Convert.ToInt32(ulMyEncID);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Encounter_ID of value='" + ulMyEncID + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Source = "APPOINTMENT";
                    objPat.IsDelete = "N";
                    objPat.Is_PopupEnable = "N";
                    objPat.Priority = "NORMAL";
                    listPatientNotes.Add(objPat);
                    //logger.Debug("AddToPatientNotes DB Call Starting");
                    Stopwatch AddToPatientNotesDBCall = new Stopwatch();
                    AddToPatientNotesDBCall.Start();
                    if (listPatientNotes != null)
                        patNotesMngr.AddToPatientNotes(listPatientNotes);
                    AddToPatientNotesDBCall.Stop();
                    //logger.Debug("AddToPatientNotes DB Call Completed. Time Taken : " + AddToPatientNotesDBCall.Elapsed.Seconds + "." + AddToPatientNotesDBCall.Elapsed.Milliseconds);
                }
                WFObj.Obj_Type = sMyObjType;
                if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                {
                    try
                    {
                        WFObj.Current_Arrival_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                }
                WFObj.Current_Owner = "UNKNOWN";
                WFObj.Fac_Name = hdnFacilityName.Value;
                WFObj.Obj_System_Id = NewEnc.Id;
                WFObj.Current_Process = "START";
                //WFObj.Id = WFProxy.InsertToWorkFlowObject(WFObj);

                if (NewEnc.Id == 0)
                {
                    if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    {
                        try
                        {
                            NewEnc.Created_Date_and_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                            throw (exp);
                        }
                    }
                    NewEnc.Created_By = ClientSession.UserName;
                    NewEnc.Is_Physician_Asst_Process = "N";
                    NewEnc.Notes = txtNotes.txtDLC.Text;
                    NewEnc.Is_Encounter_SuperBill = "N";
                    //Cap - 2505
                    // if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty)
                    if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty && cboOrder.SelectedItem.Text != "Akido Order")
                    {

                        EncRecord.Order_Submit_ID = Convert.ToInt32(cboOrder.SelectedItem.Text.Split('|')[0]);
                    }
                    else
                        EncRecord.Order_Submit_ID = 0;
                    //if (chkWillingnessInCancellation.Checked == true)
                    //{
                    //    NewEnc.Willing_For_Prior_Appointment = "Y";

                    //}
                    //else
                    //{
                    //    NewEnc.Willing_For_Prior_Appointment = "N";

                    //}
                    EncMngr = new EncounterManager();
                    //logger.Debug("CreateEncounterForRCM DB Call Starting");
                    temp = new object[] { "false" };
                    Stopwatch CreateEncounterForRCMDBCall = new Stopwatch();
                    CreateEncounterForRCMDBCall.Start();
                    if (NewEnc != null)

                        NewEnc.Id = EncMngr.CreateEncounterForRCM(NewEnc, WFObj, string.Empty, "", temp);


                    // NewEnc.Id = EncMngr.CreateEncounterForRCM(NewEnc, WFObj, string.Empty, txtAuthorizationNo.Text, temp);// GenerateIsCMGOrderObject()////not been used right now 03-02-2016 by vasanth
                    EncMngr.TriggerSPforProvReviewStatusTracker("WALK_IN", NewEnc.Id);//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
                    CreateEncounterForRCMDBCall.Stop();
                    //logger.Debug("CreateEncounterForRCM DB Call Completed. Time Taken : " + CreateEncounterForRCMDBCall.Elapsed.Seconds + "." + CreateEncounterForRCMDBCall.Elapsed.Milliseconds);
                }
            }

            //Other than Reschedule - New Appointment
            if (chkReschedule.Checked != true && EncRecord.Id == 0)
            {
                string tt1 = "";
                try
                {
                    try
                    {
                        tt1 = Convert.ToDateTime(dtpStartTime.DbSelectedDate).ToString("tt");
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Appointment Time of value='" + dtpStartTime.DbSelectedDate + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    if (dtpStartTime.SelectedTime.Value != null)
                        EncRecord.Appointment_Date = Convert.ToDateTime(sp[0] + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + tt1);
                    EncRecord.Appointment_Date = UtilityManager.ConvertToUniversal(EncRecord.Appointment_Date);// change to universal
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Appointment Date of value='" + sp[0] + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + Convert.ToDateTime(dtpStartTime.DbSelectedDate).ToString("tt") + "' to DateTime threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    EncRecord.Duration_Minutes = Convert.ToInt32(ddlDuration.Text);
                    //Cap - 3217
                    if (IsAuthVerified.Checked == true)
                    {
                        EncRecord.Is_Auth_Verified = "Y";
                    }
                    else
                    {
                        EncRecord.Is_Auth_Verified = "N";
                    }
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Duration_Minutes of value='" + ddlDuration.Text + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                EncRecord.Purpose_of_Visit = txtPurposeofVisit.txtDLC.Text;
                try
                {
                    //if (sFacilityCmg.ToUpper() == cboFacility.SelectedItem.Text.ToUpper())
                    //{
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                    {
                        if (ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value != null)
                            EncRecord.Machine_Technician_Library_ID = Convert.ToInt32(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value);
                        EncRecord.Appointment_Provider_ID = GetPhysicianLibIDByTechID(EncRecord.Machine_Technician_Library_ID);
                    }
                    else
                        EncRecord.Appointment_Provider_ID = Convert.ToInt32(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value); //ulMyPhysicianID;
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Duration_Minutes of value='" + ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value + "' to Int threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    //if (sFacilityCmg.ToUpper() == cboFacility.SelectedItem.Text.ToUpper())
                    //{
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                    {
                        ulMyPhysicianID = PhysicianId(Convert.ToUInt64(EncRecord.Machine_Technician_Library_ID)); //Convert.ToUInt64(EncRecord.Machine_Technician_Library_ID);
                    }
                    else
                        ulMyPhysicianID = Convert.ToUInt64(EncRecord.Appointment_Provider_ID);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of PhysicianID of value='" + EncRecord.Appointment_Provider_ID + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    EncRecord.Encounter_Provider_ID = Convert.ToInt32(ulMyPhysicianID);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of PhysicianID of value='" + ulMyPhysicianID + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                EncRecord.Visit_Type = ddlVisitType.Text;
                try
                {
                    EncRecord.Human_ID = Convert.ToUInt64(txtPatientAccountNumber.Text);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of HumanID of value='" + txtPatientAccountNumber.Text + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                EncRecord.Facility_Name = cboFacility.Text;
                EncRecord.Place_Of_Service = hdnPOS.Value;
                EncRecord.Is_Batch_Created = "N";
                EncRecord.Is_EandM_Submitted = "N";
                if (tabReferringProvAndPCP.SelectedIndex == 0)
                {

                    //Commented by valli need to check




                    //EncRecord.Referring_Facility = txtReferringFacility.Text.Trim();
                    //EncRecord.Referring_Physician = txtReferringProvider.Text;
                    //EncRecord.Referring_Address = txtReferingAddress.Text.Trim();
                    //EncRecord.Referring_Phone_No = msktxtReferingPhoneNo.TextWithLiterals;
                    //EncRecord.Referring_Fax_No = msktxtReferingFaxNo.TextWithLiterals;
                    //EncRecord.Referring_Provider_NPI = txtProviderNPI.Text.Trim();

                    if (hdnpcpprovider.Value != "" && hdnpcpprovider.Value != null && hdnpcpprovider.Value != "|||||")
                    {
                        //Jira #CAP-69 - labels are missing
                        //EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[4].ToString();
                        //EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[1].ToString();
                        //EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[5].ToString();
                        //EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[7].ToString();
                        //EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[6].ToString();
                        //EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[2].ToString();

                        EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[3].Split(':')[1].ToString();
                        EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                    }
                    if (!chkSelfReferred.Checked)
                    {
                        if (hdnrenprovider.Value != null && hdnrenprovider.Value.Trim() != "" && hdnrenprovider.Value != "|||||")
                        {
                            EncRecord.Is_Self_Referred = "N";
                            EncRecord.Referring_Facility = hdnrenprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Physician = hdnrenprovider.Value.Split('|')[0].ToString();
                            EncRecord.Referring_Address = hdnrenprovider.Value.Split('|')[3].Split(':')[1].ToString();
                            EncRecord.Referring_Phone_No = hdnrenprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Fax_No = hdnrenprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                        }
                    }
                    else
                    {
                        EncRecord.Is_Self_Referred = "Y";
                        EncRecord.Referring_Facility = string.Empty;
                        EncRecord.Referring_Physician = string.Empty;
                        EncRecord.Referring_Address = string.Empty;
                        EncRecord.Referring_Phone_No = string.Empty;
                        EncRecord.Referring_Fax_No = string.Empty;
                        EncRecord.Referring_Provider_NPI = string.Empty;
                    }
                }
                else if (tabReferringProvAndPCP.SelectedIndex == 1)
                {
                    //EncRecord.PCP_Facility = txtReferringFacility.Text.Trim();
                    //EncRecord.PCP_Physician = txtReferringProvider.Text;
                    //EncRecord.PCP_Address = txtReferingAddress.Text.Trim();
                    //EncRecord.PCP_Phone_No = msktxtReferingPhoneNo.TextWithLiterals;
                    //EncRecord.PCP_Fax_No = msktxtReferingFaxNo.TextWithLiterals;
                    //EncRecord.PCP_Provider_NPI = txtProviderNPI.Text.Trim();
                    if (hdnpcpprovider.Value != "" && hdnpcpprovider.Value != null && hdnpcpprovider.Value != "|||||")
                    {
                        //Jira #CAP-69 - labels are missing
                        //EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[4].ToString();
                        //EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[1].ToString();
                        //EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[5].ToString();
                        //EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[7].ToString();
                        //EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[6].ToString();
                        //EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[2].ToString();

                        EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[3].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                    }
                    if (!chkSelfReferred.Checked)
                    {
                        if (hdnrenprovider.Value != null && hdnrenprovider.Value.Trim() != "" && hdnrenprovider.Value != "|||||")
                        {
                            EncRecord.Is_Self_Referred = "N";
                            EncRecord.Referring_Facility = hdnrenprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Physician = hdnrenprovider.Value.Split('|')[0].ToString();
                            EncRecord.Referring_Address = hdnrenprovider.Value.Split('|')[3].Split(':')[1].ToString();
                            EncRecord.Referring_Phone_No = hdnrenprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Fax_No = hdnrenprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                        }
                    }
                    else
                    {
                        EncRecord.Is_Self_Referred = "Y";
                        EncRecord.Referring_Facility = string.Empty;
                        EncRecord.Referring_Physician = string.Empty;
                        EncRecord.Referring_Address = string.Empty;
                        EncRecord.Referring_Phone_No = string.Empty;
                        EncRecord.Referring_Fax_No = string.Empty;
                        EncRecord.Referring_Provider_NPI = string.Empty;
                    }
                }


                EncRecord.Notes = txtNotes.txtDLC.Text;
                EncRecord.Is_Encounter_SuperBill = "N";
                //Cap - 2505
                //(cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty)
                if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty && cboOrder.SelectedItem.Text != "Akido Order")
                {

                    EncRecord.Order_Submit_ID = Convert.ToInt32(cboOrder.SelectedItem.Text.Split('|')[0]);
                }
                else
                    EncRecord.Order_Submit_ID = 0;
                //if (chkWillingnessInCancellation.Checked == true)
                //{
                //    EncRecord.Willing_For_Prior_Appointment = "Y";

                //}
                //else
                //{
                //    EncRecord.Willing_For_Prior_Appointment = "N";

                //}
                //Added by Bala for ADD MESSAGES IN APPOINTMENTS in 20-12-2013
                if (txtNotes.txtDLC.Text != string.Empty)
                {
                    PatientNotesManager patNotesMngr = new PatientNotesManager();
                    IList<PatientNotes> listPatientNotes = new List<PatientNotes>();
                    PatientNotes objPat = new PatientNotes();
                    try
                    {
                        objPat.Human_ID = Convert.ToUInt64(txtPatientAccountNumber.Text);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of HumanID of value='" + txtPatientAccountNumber.Text + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Message_Orign = "Appointment";
                    if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    {
                        try
                        {
                            objPat.Message_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                            throw (exp);
                        }
                    }

                    objPat.Message_Description = "Appointment";
                    objPat.Notes = txtNotes.txtDLC.Text;
                    if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    {
                        try
                        {
                            objPat.Created_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                            throw (exp);
                        }
                    }

                    if (ClientSession.UserName != null)
                        objPat.Created_By = ClientSession.UserName;
                    //objPat.Modified_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    objPat.Is_PatientChart = "N";
                    objPat.Line_ID = 0;
                    objPat.Type = "MESSAGE";
                    try
                    {
                        objPat.Encounter_ID = Convert.ToInt32(ulMyEncID);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Encounter_ID of value='" + ulMyEncID + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Statement_ChargeLine_ID = 0;
                    try
                    {
                        objPat.SourceID = Convert.ToInt32(ulMyEncID);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Encounter_ID of value='" + ulMyEncID + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Source = "APPOINTMENT";
                    objPat.IsDelete = "N";
                    objPat.Is_PopupEnable = "N";
                    objPat.Priority = "NORMAL";
                    listPatientNotes.Add(objPat);
                    //logger.Debug("AddToPatientNotes DB Call Starting");
                    Stopwatch AddToPatientNotesDBCall = new Stopwatch();
                    AddToPatientNotesDBCall.Start();
                    if (listPatientNotes != null)
                        patNotesMngr.AddToPatientNotes(listPatientNotes);
                    AddToPatientNotesDBCall.Stop();
                    //logger.Debug("AddToPatientNotes DB Call Completed. Time Taken : " + AddToPatientNotesDBCall.Elapsed.Seconds + "." + AddToPatientNotesDBCall.Elapsed.Milliseconds);
                }
                if (hdnEncounterID.Value != null && hdnEncounterID.Value != string.Empty)
                {
                    ulMyEncID = 0;
                }
                if (ulMyEncID == 0)
                {
                    WFObj.Obj_Type = sMyObjType;
                    if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    {
                        try
                        {
                            WFObj.Current_Arrival_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                            throw (exp);
                        }
                    }
                    WFObj.Current_Owner = "UNKNOWN";
                    if (hdnFacilityName.Value != null && hdnFacilityName.Value != "")
                        WFObj.Fac_Name = hdnFacilityName.Value;
                    else
                        WFObj.Fac_Name = cboFacility.SelectedItem.Text;
                    WFObj.Obj_System_Id = EncRecord.Id;
                    WFObj.Current_Process = "START";
                    //WFObj.Id = WFProxy.InsertToWorkFlowObject(WFObj);
                }

                if (EncRecord.Id == 0)
                {
                    if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    {
                        try
                        {
                            EncRecord.Created_Date_and_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                            throw (exp);
                        }
                    }
                    EncRecord.Created_By = ClientSession.UserName;
                    EncRecord.Is_Physician_Asst_Process = "N";
                    EncRecord.Notes = txtNotes.txtDLC.Text;
                    EncMngr = new EncounterManager();
                    //logger.Debug("CreateEncounterForRCM DB Call Starting");
                    object[] temp = new object[] { "false" };
                    Stopwatch CreateEncounterForRCMDBCall = new Stopwatch();
                    CreateEncounterForRCMDBCall.Start();
                    if (EncRecord != null)

                        EncRecord.Id = EncMngr.CreateEncounterForRCM(EncRecord, WFObj, string.Empty, "", temp);// GenerateIsCMGOrderObject()////not been used right now 03-02-2016 by vasanth
                    //   EncRecord.Id = EncMngr.CreateEncounterForRCM(EncRecord, WFObj, string.Empty, txtAuthorizationNo.Text, temp);// GenerateIsCMGOrderObject()////not been used right now 03-02-2016 by vasanth
                    EncMngr.TriggerSPforProvReviewStatusTracker("WALK_IN", EncRecord.Id);//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
                    CreateEncounterForRCMDBCall.Stop();
                    //logger.Debug("CreateEncounterForRCM DB Call Completed. Time Taken : " + CreateEncounterForRCMDBCall.Elapsed.Seconds + "." + CreateEncounterForRCMDBCall.Elapsed.Milliseconds);
                }
            }
            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110019');", true);
            try
            {
                if (dtpApptDate.SelectedDate.Value != null)
                    CalendarDate = Convert.ToDateTime(dtpApptDate.SelectedDate.Value);
                MyTime = Convert.ToDateTime(dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + ":" + dtpStartTime.SelectedTime.Value.Seconds);
            }
            catch (Exception exp)
            {
                //logger.Debug("Conversion of Calender Selected Date of value='" + dtpApptDate.SelectedDate.Value + "' to DateTime threw an error.", exp);
                throw (exp);
            }

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Edit Appointment", "CloseWindowForConfirmAppointment();   { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);
            //logger.Debug("--------------------frmEditAppointment BtnConfirmAppointment Click Event Completed. Time Taken :'" + ConfirmAppointmentTime.Elapsed.Seconds + "." + ConfirmAppointmentTime.Elapsed.Milliseconds + "s'--------------------");
        }

        protected void chkSelfReferred_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelfReferred.Checked == true)
            {
                rdoReferringProvider.Checked = false;
                rdoPcp.Checked = false;
                HdnRefPhy.Value = "|||||||";
                txtProviderSearch.Text = string.Empty;
                hdnRefEditPhyId.Value = string.Empty;

                //Commentted for BugId: 56036
                //if (ddlPhysicianName.Items.Count > 0)
                //{
                //    //txtReferringFacility.Text = string.Empty;
                //    //txtReferringProvider.Text = string.Empty;
                //    //txtReferingAddress.Text = string.Empty;
                //    //msktxtReferingPhoneNo.Text = string.Empty;
                //    //msktxtReferingFaxNo.Text = string.Empty;
                //    //txtProviderNPI.Text = string.Empty;
                //    rdoReferringProvider.Checked = false;
                //    rdoPcp.Checked = false;
                //    //  txtReferringFacility.Text = hdnFacilityName.Value;
                //    PhysicianLibrary objPhysicianLibrary = GetPhysicianDetailsByPhyID(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value);//PhyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value));
                //    if (objPhysicianLibrary != null)
                //    {
                //        //  txtReferringFacility.Text = cboFacility.SelectedItem.Text;
                //        FacilityManager obj = new FacilityManager();
                //        IList<FacilityLibrary> lstfacirty = obj.GetFacilityByFacilityname(cboFacility.SelectedItem.Text);
                //        //if (lstfacirty != null && lstfacirty.Count > 0)
                //        //    txtReferingAddress.Text = lstfacirty[0].Fac_Address1;
                //        //else
                //        //    txtReferingAddress.Text = objPhysicianLibrary.PhyAddress1.ToString();
                //        //msktxtReferingPhoneNo.Text = objPhysicianLibrary.PhyTelephone.ToString();
                //        //msktxtReferingFaxNo.Text = string.Empty;
                //        //if (msktxtReferingFaxNo.Text == "(   )    -")
                //        //{
                //        //    msktxtReferingFaxNo.Text = objPhysicianLibrary.PhyFax.ToString();
                //        //}
                //        //txtProviderNPI.Text = objPhysicianLibrary.PhyNPI.ToString();

                //        //}
                //        //PhysicianManager phyMngr = new PhysicianManager();
                //        //IList<PhysicianLibrary> phylist = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(hdnEncounter_Physician_id.Value));
                //        //if (phylist != null && phylist.Count > 0)
                //        //{
                //        //    PhysicianLibrary objPhyLib = phylist[0];

                //        //    if (objPhyLib != null)
                //        //    {
                //        string sPhyName = string.Empty;
                //        if (objPhysicianLibrary != null)
                //        {
                //            string facilityadd = "";
                //            if (lstfacirty.Count > 0)
                //            {
                //                facilityadd = lstfacirty[0].Fac_Address1;
                //            }

                //            //objPhysicianLibrary.PhyLastName + ", " + objPhysicianLibrary.PhyFirstName + " " + objPhysicianLibrary.PhyMiddleName + "(" + objPhysicianLibrary.PhySuffix + ")" + " | " +
                //            //                                 "NPI:" + objPhysicianLibrary.PhyNPI + " | " +
                //            //                                 "" + " | " +
                //            //                                 "FACILITY:" + "" + " | " +
                //            //                                 "ADDR: " + objPhysicianLibrary.PhyAddress1 + ", " +
                //            //                                 objPhysicianLibrary.PhyCity + "," +
                //            //                                 objPhysicianLibrary.PhyState + " " +
                //            //                                 objPhysicianLibrary.PhyZip + " | " +
                //            //                                 ((objPhysicianLibrary.PhyTelephone.Trim()) != "" ? "PH:" + objPhysicianLibrary.PhyTelephone + " | " : "") 
                //            //                                 (objPhysicianLibrary.PhyFax.Trim() != "" ? "FAX:" + objPhysicianLibrary.PhyFax : "");


                //            sPhyName = objPhysicianLibrary.PhyPrefix + " " + objPhysicianLibrary.PhyFirstName + " " + objPhysicianLibrary.PhyLastName + " " + objPhysicianLibrary.PhySuffix;
                //            // txtReferringProvider.Text = sPhyName;
                //            // HdnRefPhy.Value = sPhyName + "|" + objPhysicianLibrary.PhyAddress1 + "|" + objPhysicianLibrary.PhyTelephone + "|" + objPhysicianLibrary.PhyFax + "|" + objPhysicianLibrary.PhyNPI + "|" + objPhysicianLibrary.PhyNotes; ;//for static
                //            hdnrenprovider.Value = objPhysicianLibrary.Id + "|" + sPhyName + "|" + objPhysicianLibrary.PhyNPI + "|" + "" + "|" + cboFacility.SelectedItem.Text + "|" +
                //               facilityadd + "|" + objPhysicianLibrary.PhyFax + "|" + objPhysicianLibrary.PhyTelephone;
                //            hdnrenprovidersearch.Value = objPhysicianLibrary.PhyLastName + ", " + objPhysicianLibrary.PhyFirstName + " " +
                //                 objPhysicianLibrary.PhyMiddleName + "(" + objPhysicianLibrary.PhySuffix + ")" + " | " +
                //                                                   "NPI:" + objPhysicianLibrary.PhyNPI + " | " +
                //                                                  "" + " | " +
                //                                                   "FACILITY:" + cboFacility.SelectedItem.Text + " | " +
                //                                                   "ADDR: " + facilityadd + ", " +
                //                                                   objPhysicianLibrary.PhyCity + "," +
                //                                                   objPhysicianLibrary.PhyState + " " +
                //                                                   objPhysicianLibrary.PhyZip + " | " +
                //                                                   ((objPhysicianLibrary.PhyTelephone.Trim()) != "" ? "PH:" + objPhysicianLibrary.PhyTelephone + " | " : "") +
                //                                                   (objPhysicianLibrary.PhyFax.Trim() != "" ? "FAX:" + objPhysicianLibrary.PhyFax : "");


                //            txtProviderSearch.Text = objPhysicianLibrary.PhyLastName + ", " + objPhysicianLibrary.PhyFirstName + " " +
                //                objPhysicianLibrary.PhyMiddleName + "(" + objPhysicianLibrary.PhySuffix + ")" + " | " +
                //                                                  "NPI:" + objPhysicianLibrary.PhyNPI + " | " +
                //                                                 "" + " | " +
                //                                                  "FACILITY:" + cboFacility.SelectedItem.Text + " | " +
                //                                                  "ADDR: " + facilityadd + ", " +
                //                                                  objPhysicianLibrary.PhyCity + "," +
                //                                                  objPhysicianLibrary.PhyState + " " +
                //                                                  objPhysicianLibrary.PhyZip + " | " +
                //                                                  ((objPhysicianLibrary.PhyTelephone.Trim()) != "" ? "PH:" + objPhysicianLibrary.PhyTelephone + " | " : "") +
                //                                                  (objPhysicianLibrary.PhyFax.Trim() != "" ? "FAX:" + objPhysicianLibrary.PhyFax : "");

                //        }
                //        //}
                //    }
                //    else
                //    {
                //        HdnRefPhy.Value = "|||||||";//for static
                //    }
                //    //txtReferringProvider.ReadOnly = true;
                //    //txtReferringFacility.ReadOnly = true;
                //    //txtReferingAddress.ReadOnly = true;
                //    //msktxtReferingPhoneNo.ReadOnly = true;
                //    //msktxtReferingFaxNo.ReadOnly = true;
                //    //txtProviderNPI.ReadOnly = true;
                //    //btnFindPhysician.Enabled = false;
                //    //txtReferringProvider.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //    //TextBoxColorChange(txtReferringProvider, false);
                //    //txtReferringFacility.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //    //TextBoxColorChange(txtReferringFacility, false);
                //    //txtProviderNPI.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //    //TextBoxColorChange(txtReferingAddress, false);
                //    //txtProviderNPI.Attributes.Add("onkeypress", "return false;");
                //    //txtReferringProvider.Attributes.Add("onkeypress", "return false;");
                //    //txtReferringFacility.Attributes.Add("onkeypress", "return false;");
                //    //txtReferingAddress.Attributes.Add("onkeypress", "return false;");
                //    //txtProviderNPI.Attributes.Add("onkeydown", "return false;");
                //    //txtReferringProvider.Attributes.Add("onkeydown", "return false;");
                //    //txtReferringFacility.Attributes.Add("onkeydown", "return false;");
                //    //txtReferingAddress.Attributes.Add("onkeydown", "return false;");
                //    //txtReferingAddress.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //    //MaskedTextBoxColorChange(msktxtReferingPhoneNo, false);
                //    //msktxtReferingPhoneNo.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //    //MaskedTextBoxColorChange(msktxtReferingFaxNo, false);
                //    //msktxtReferingFaxNo.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //    //TextBoxColorChange(txtProviderNPI, false);
                //    //btnFindPhysician.Enabled = false;
                //    //msktxtReferingFaxNo.ReadOnly = true;
                //    //msktxtReferingPhoneNo.ReadOnly = true;
                //    //btnSave.Attributes.Add("disabled", "false");


                //}
                //else
                //{
                //    chkSelfReferred.Checked = false;
                //}

                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "showVal", "EnableSaveButtononunload(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // txtProviderSearch.Enabled = false; ////change
                imgClearProviderText.Attributes.Remove("onclick");


            }
            else
            {
                imgClearProviderText.Attributes.Add("onclick", "return ProviderSearchclear();");


                hdnrenprovider.Value = "";
                txtProviderSearch.Text = "";
                hdnrenprovidersearch.Value = "";
                txtProviderSearch.Enabled = true;
                //txtReferringProvider.Text = string.Empty; //by vasanth
                //txtReferringFacility.Text = string.Empty;
                //txtReferingAddress.Text = string.Empty;
                //msktxtReferingFaxNo.Text = string.Empty;
                //msktxtReferingPhoneNo.Text = string.Empty;
                //txtProviderNPI.Text = string.Empty;

                //txtReferringProvider.ReadOnly = false;
                //txtReferringFacility.ReadOnly = false;
                //txtReferingAddress.ReadOnly = false;
                //msktxtReferingPhoneNo.ReadOnly = false;
                //msktxtReferingFaxNo.ReadOnly = false;
                //txtProviderNPI.ReadOnly = false;
                //btnFindPhysician.Enabled = true;
                btnSave.Enabled = true;
                //btnSave.Attributes.Add("disabled", "false");
                //txtReferringProvider.BackColor = System.Drawing.Color.White;
                //txtReferringFacility.BackColor = System.Drawing.Color.White;
                //txtProviderNPI.BackColor = System.Drawing.Color.White;
                //txtReferingAddress.BackColor = System.Drawing.Color.White;
                //msktxtReferingPhoneNo.BackColor = System.Drawing.Color.White;
                //msktxtReferingFaxNo.BackColor = System.Drawing.Color.White;
                // Commented by valli
                //  if (HdnRefPhy.Value != null && HdnRefPhy.Value != "" && HdnRefPhy.Value != string.Empty)
                // {
                //if (txtReferringProvider.Text.Trim() != HdnRefPhy.Value.Split('|')[0].ToString().Trim())
                //{
                //    txtReferringFacility.Text = string.Empty;
                //    txtReferringProvider.Text = string.Empty;
                //    txtReferingAddress.Text = string.Empty;
                //    msktxtReferingPhoneNo.Text = string.Empty;
                //    msktxtReferingFaxNo.Text = string.Empty;
                //    txtProviderNPI.Text = string.Empty;
                //    txtReferringProvider.Text = HdnRefPhy.Value.Split('|')[0].ToString();
                //    txtReferingAddress.Text = HdnRefPhy.Value.Split('|')[1].ToString();
                //    msktxtReferingPhoneNo.Text = HdnRefPhy.Value.Split('|')[2].ToString();
                //    msktxtReferingFaxNo.Text = HdnRefPhy.Value.Split('|')[3].ToString();
                //    txtProviderNPI.Text = HdnRefPhy.Value.Split('|')[4].ToString();
                //    txtReferringFacility.Text = HdnRefPhy.Value.Split('|')[5].ToString();
                //}
                //else
                //{
                //    //PhysicianManager phyMngr = new PhysicianManager();
                //    //IList<PhysicianLibrary> phylist = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(hdnEncounter_Physician_id.Value));
                //    //if (phylist != null && phylist.Count > 0)
                //    //{
                //    PhysicianLibrary objPhyLib = GetPhysicianDetailsByPhyID(hdnEncounter_Physician_id.Value);

                //    if (objPhyLib != null)
                //    {
                //        txtReferringFacility.Text = string.Empty;
                //        txtReferringProvider.Text = string.Empty;
                //        txtReferingAddress.Text = string.Empty;
                //        msktxtReferingPhoneNo.Text = string.Empty;
                //        msktxtReferingFaxNo.Text = string.Empty;
                //        txtProviderNPI.Text = string.Empty;
                //        string sPhyName = objPhyLib.PhyPrefix + " " + objPhyLib.PhyFirstName + " " + objPhyLib.PhyLastName + " " + objPhyLib.PhySuffix;
                //        txtReferringFacility.Text = objPhyLib.PhyNotes;
                //        txtReferringProvider.Text = sPhyName;
                //        txtReferingAddress.Text = objPhyLib.PhyAddress1;
                //        msktxtReferingPhoneNo.Text = objPhyLib.PhyTelephone;
                //        msktxtReferingFaxNo.Text = objPhyLib.PhyFax;
                //        txtProviderNPI.Text = objPhyLib.PhyNPI;
                //        HdnRefPhy.Value = sPhyName + "|" + objPhyLib.PhyAddress1 + "|" + objPhyLib.PhyTelephone + "|" + objPhyLib.PhyFax + "|" + objPhyLib.PhyNPI + "|" + objPhyLib.PhyNotes;//for static
                //        //}
                //    }
                //    else
                //    {
                //        HdnRefPhy.Value = "|||||";//for static
                //        txtReferringFacility.Text = "";
                //        txtReferringProvider.Text = "";
                //        txtReferingAddress.Text = "";
                //        msktxtReferingPhoneNo.Text = "";
                //        msktxtReferingFaxNo.Text = "";
                //        txtProviderNPI.Text = "";
                //    }
                //}
                // }

                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "showVal", "EnableSaveButtononunload(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            //Jira CAP-2216 - Start
            if (Request["EncounterID"] != null && Request["EncounterID"] != string.Empty && hdnCurrentProcess.Value != "ARCHIEVE")
            {
                EnablePhytxtBox(Convert.ToUInt64(Request["EncounterID"]));
            }
            else if (hdnCurrentProcess.Value == "ARCHIEVE")
            {
                chkSelfReferred.Enabled = false;
                pnlReferringDetails.Enabled = false;
            }
            if (chkSelfReferred != null && chkSelfReferred.Visible == true && chkSelfReferred.Checked == true)
            {
                imgClearProviderText.Visible = false;
                imgEditProvider.Visible = false;
            }
        }

        protected void chkShowAllPhysicians_CheckedChanged(object sender, EventArgs e)
        {
            IList<PhysicianLibrary> PhyList;

            if (chkShowAllPhysicians.Checked == true)
            {
                PhyList = UtilityManager.GetPhysicianList("", ClientSession.LegalOrg);//PhyMngr.GetPhysicianandUser(false, string.Empty);
                //CAP-3499
                if (PhyList != null)
                {
                    var otherPhysicianList = UtilityManager.GetInActiveProviderList("", ClientSession.LegalOrg, true);
                    foreach (var physician in otherPhysicianList)
                    {
                        PhyList.Add(physician);
                    }
                }
            }
            else
            {
                PhyList = UtilityManager.GetPhysicianList(cboFacility.Text.Trim().ToUpper(), ClientSession.LegalOrg);//PhyMngr.GetPhysicianandUser(true, cboFacility.Text);
                //CAP-3499
                if (PhyList != null)
                {
                    var otherPhysicianList = UtilityManager.GetInActiveProviderList(cboFacility.Text.Trim(), ClientSession.LegalOrg, true);
                    foreach (var physician in otherPhysicianList)
                    {
                        PhyList.Add(physician);
                    }
                }
            }
            ddlPhysicianName.Items.Clear();

            XmlDocument xmldoc = new XmlDocument();
            for (int i = 0; i < PhyList.Count; i++)
            {
                //string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;
                RadComboBoxItem item1 = new RadComboBoxItem();
                if (i == 0)
                {
                    RadComboBoxItem item2 = new RadComboBoxItem();
                    item2.Text = "";
                    item2.Value = "0";
                    ddlPhysicianName.Items.Add(item2);
                }
                //if (sAncillary != string.Empty && sAncillary == cboFacility.SelectedItem.Text.Trim())
                //{
                var fac = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                IList<FacilityLibrary> ilstFac = fac.ToList<FacilityLibrary>();
                if (ilstFac.Count > 0 && ilstFac[0].Is_Ancillary == "Y")
                {
                    //Jira CAP-2777
                    //string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
                    //if (File.Exists(strXmlFilePathTech) == true)
                    {
                        //Jira CAP-2777
                        //xmldoc = new XmlDocument();
                        //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                        if (PhyList[i].PhyColor != "" && PhyList[i].PhyColor != "0")
                        {
                            //Jira CAP-2777
                            //XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + PhyList[i].PhyColor);
                            //if (xmlTec != null)
                            //{
                            //    item1.Text = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName;
                            //    item1.Value = xmlTec[0].Attributes.GetNamedItem("machine_technician_library_id").Value;
                            //}

                            //Jira CAP-2777
                            MachinetechnicianList machinetechnicianList = new MachinetechnicianList();
                            machinetechnicianList = ConfigureBase<MachinetechnicianList>.ReadJson("machine_technician.json");
                            if (machinetechnicianList?.MachineTechnician != null)
                            {
                                List<Machinetechnician> machinetechnicians = new List<Machinetechnician>();
                                machinetechnicians = machinetechnicianList.MachineTechnician.Where(x => x.machine_technician_library_id == PhyList[i].PhyColor).ToList();
                                if (machinetechnicians.Count > 0)
                                {
                                    item1.Text = machinetechnicians[0].machine_name + " - " + PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName;
                                    item1.Value = machinetechnicians[0].machine_technician_library_id;
                                }
                            }

                        }
                        else
                        {
                            item1.Text = PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName;
                            item1.Value = PhyList[i].Id.ToString();
                        }

                        ddlPhysicianName.Items.Add(item1);
                        //if (item1.Value == ulMyPhysicianID.ToString())
                        //{
                        //    phyCheck = true;
                        //    ddlPhysicianName.SelectedIndex = i;
                        //    hdnEditApptPhyID.Value = ddlPhysicianName.SelectedValue.ToString();//added by vasanth
                        //}
                    }
                }
                else
                {
                    //CAP-1995 - Testing & Production: In Appointment Scheduler, Duplicate provide list shows in the Provider Name field. 
                    //Jira CAP-2777
                    //string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
                    //if (File.Exists(strXmlFilePathTech) == true)
                    {
                        string sPhyName = string.Empty;
                        string PhyId = string.Empty;

                        //Jira CAP-2777
                        //xmldoc = new XmlDocument();
                        //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                        if (PhyList[i].PhyColor != "" && PhyList[i].PhyColor != "0")
                        {
                            //Jira CAP-2777
                            //XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + PhyList[i].PhyColor);
                            //if (xmlTec != null)
                            //{
                            //    sPhyName = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName;
                            //    PhyId = xmlTec[0].Attributes.GetNamedItem("machine_technician_library_id").Value;
                            //}

                            //Jira CAP-2777
                            MachinetechnicianList machinetechnicianList = new MachinetechnicianList();
                            machinetechnicianList = ConfigureBase<MachinetechnicianList>.ReadJson("machine_technician.json");
                            if (machinetechnicianList?.MachineTechnician != null)
                            {
                                List<Machinetechnician> machinetechnicians = new List<Machinetechnician>();
                                machinetechnicians = machinetechnicianList.MachineTechnician.Where(x => x.machine_technician_library_id == PhyList[i].PhyColor).ToList();
                                if (machinetechnicians.Count > 0)
                                {
                                    sPhyName = machinetechnicians[0].machine_name + " - " + PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName;
                                    PhyId = machinetechnicians[0].machine_technician_library_id;
                                }
                            }
                        }
                        else
                        {
                            if (PhyList[i] != null)
                            {
                                //old code
                                //sPhyName = PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyLastName;
                                //Gitlab# 2485 - Physician Name Display Change
                                if (PhyList[i].PhyLastName != String.Empty)
                                    sPhyName += PhyList[i].PhyLastName;
                                if (PhyList[i].PhyFirstName != String.Empty)
                                {
                                    if (sPhyName != String.Empty)
                                        sPhyName += "," + PhyList[i].PhyFirstName;
                                    else
                                        sPhyName += PhyList[i].PhyFirstName;
                                }
                                if (PhyList[i].PhyMiddleName != String.Empty)
                                    sPhyName += " " + PhyList[i].PhyMiddleName;
                                if (PhyList[i].PhySuffix != String.Empty)
                                    sPhyName += "," + PhyList[i].PhySuffix;

                                PhyId = PhyList[i].Id.ToString();
                            }
                        }
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Value = PhyId;
                        item.Text = sPhyName;
                        ddlPhysicianName.Items.Add(item);
                    }
                }
                //if (ddlPhysicianName.Items[i].Value == hdnPhysicianID.Value)
                //{
                //    ddlPhysicianName.SelectedIndex = i;
                //}
                if (ddlPhysicianName.Items[i + 1].Value != null)
                {
                    if (PhyList[i].PhyNotes == "Default" && chkShowAllPhysicians.Checked == false && ddlPhysicianName.Items[i + 1].Value != hdnPhysicianID.Value)
                    {
                        ddlPhysicianName.SelectedIndex = i + 1;
                    }
                    else if (PhyList[i].PhyNotes != "Default" && chkShowAllPhysicians.Checked == false && ddlPhysicianName.Items[i + 1].Value == hdnPhysicianID.Value)
                    {
                        ddlPhysicianName.SelectedIndex = i + 1;
                    }
                    else if (PhyList[i].PhyNotes == "Default" && chkShowAllPhysicians.Checked == false && ddlPhysicianName.Items[i + 1].Value == hdnPhysicianID.Value)
                    {
                        ddlPhysicianName.SelectedIndex = i + 1;
                    }
                    else if (chkShowAllPhysicians.Checked == true)
                    {
                        if (ddlPhysicianName.Items[i + 1].Value == hdnPhysicianID.Value && hdnPhysicianID.Value != "0")
                        {
                            ddlPhysicianName.SelectedIndex = i + 1;
                        }
                    }
                }
            }
            //CAP-2024
            var comboBoxItems = ddlPhysicianName.Items.Cast<RadComboBoxItem>().ToList();
            ddlPhysicianName.Items.Clear();
            //CAP-3693
            ddlPhysicianName.Items.AddRange(comboBoxItems.Where(a => !string.IsNullOrWhiteSpace(a.Text)).OrderBy(a => a.Text.Trim()).ToArray());

            btnSave.Enabled = true;

            if (chkSelfReferred.Checked == true)
            {
                //chkSelfReferred.Checked = false; Bug ID: 60606
                chkSelfReferred_CheckedChanged(sender, e);
            }
            ClientSession.PhysicianId = ddlPhysicianName.SelectedValue != null && ddlPhysicianName.SelectedValue.Trim() != "" ? Convert.ToUInt64(ddlPhysicianName.SelectedValue) : 0;
            hdnEditApptPhyID.Value = ddlPhysicianName.SelectedValue != null && ddlPhysicianName.SelectedValue.Trim() != "" ? ddlPhysicianName.SelectedValue.ToString() : "0";
            FillPOV();
            //if (cboFacility.SelectedItem.Text.ToUpper() == sFacilityCmg.ToUpper())
            //{
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
            {
                ChangeproviderforCMGAncillary(true);
            }
            else
                ChangeproviderforCMGAncillary(false);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ShowAllProvidersClicked", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void chkReschedule_CheckedChanged(object sender, EventArgs e)
        {

            if (chkReschedule.Checked == true)
            {
                //pnlReschedule.Enabled = true;
                EnableTableLayout(pnlReschedule);
                DateTimePickerColorChange(dtpApptDate, false);
                TimePickerColorChange(dtpStartTime, false);
                btnSave.Enabled = true;
                btnFindAvailableSlot.Enabled = true;
                FillReasonCode();

                if (ddlReasonCode.Items.Count > 0)
                {
                    ddlReasonCode.SelectedIndex = 0;
                    RadComboBoxSelectedIndexChangedEventArgs e1 = new RadComboBoxSelectedIndexChangedEventArgs(string.Empty, string.Empty, string.Empty, string.Empty);
                    ddlReasonCode_SelectedIndexChanged(sender, e1);
                    //ddlReasonCode_SelectedIndexChanged(sender, e);
                }
            }
            else
            {
                if (hdnSelectedDateTime.Value != null && hdnSelectedDateTime.Value != string.Empty)
                {
                    try
                    {
                        dtpApptDate.SelectedDate = Convert.ToDateTime(hdnSelectedDateTime.Value);
                        CalendarDate = Convert.ToDateTime(dtpApptDate.SelectedDate.Value);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Calender Selected Date from hdnSelectedDateTime.Value='" + hdnSelectedDateTime.Value + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                }
                if (hdnSelectedDateTime.Value != null && hdnSelectedDateTime.Value != string.Empty)
                {
                    DateTime dt = new DateTime();
                    TimeSpan ts = new TimeSpan();
                    if (hdnSelectedDateTime.Value != string.Empty)
                    {
                        try
                        {
                            dt = Convert.ToDateTime(hdnSelectedDateTime.Value);
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of Calender Selected Date from hdnSelectedDateTime.Value='" + hdnSelectedDateTime.Value + "' to DateTime threw an error.", exp);
                            throw (exp);
                        }
                        ts = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
                    }

                    dtpStartTime.SelectedTime = ts;
                }
                // pnlReschedule.Enabled = false;
                DisableTableLayout(pnlReschedule);
                DateTimePickerColorChange(this.dtpApptDate, true);
                TimePickerColorChange(dtpStartTime, true);
                btnSave.Enabled = true;
                btnFindAvailableSlot.Enabled = false;

                if (ddlReasonCode.Items.Count > 0)
                {
                    ddlReasonCode.SelectedIndex = 0;
                    RadComboBoxSelectedIndexChangedEventArgs e1 = new RadComboBoxSelectedIndexChangedEventArgs(string.Empty, string.Empty, string.Empty, string.Empty);
                    ddlReasonCode_SelectedIndexChanged(sender, e1);
                    //ddlReasonCode_SelectedIndexChanged(sender, e);
                }
            }
        }

        protected void ddlPhysicianName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            if (hdnrenprovidersearch.Value != "" && tabReferringProvAndPCP.SelectedIndex == 0)

                txtProviderSearch.Text = hdnrenprovidersearch.Value;

            if (hdnpcpprovidersearch.Value != "" && tabReferringProvAndPCP.SelectedIndex == 1)

                txtProviderSearch.Text = hdnpcpprovidersearch.Value;

            if (ddlPhysicianName.SelectedValue != null)
                ClientSession.PhysicianId = Convert.ToUInt64(ddlPhysicianName.SelectedValue);
            hdnEditApptPhyID.Value = ddlPhysicianName.SelectedValue.ToString();
            FillPOV();
            if (chkSelfReferred.Checked == true)
            {
                chkSelfReferred.Checked = false;
                chkSelfReferred_CheckedChanged(sender, e);
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PhysicianSelected", " EnableSaveButton(this);{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void ddlVisitType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //string[] splitvalues = ddlVisitType.SelectedItem.Value.Split(new string[] { "$#%" }, StringSplitOptions.None);
            //if (splitvalues != null && splitvalues.Length == 2)
            //{
            //    ddlDuration.Text = splitvalues[0];
            //    txtVisitDescription.Text = splitvalues[1];
            //}
            if (hdnrenprovidersearch.Value != "" && tabReferringProvAndPCP.SelectedIndex == 0)

                txtProviderSearch.Text = hdnrenprovidersearch.Value;

            if (hdnpcpprovidersearch.Value != "" && tabReferringProvAndPCP.SelectedIndex == 1)

                txtProviderSearch.Text = hdnpcpprovidersearch.Value;

            if (HdnEditVisit.Value.Split('|').Count() > 0)
            {
                if (ddlVisitType.SelectedItem.Text != HdnEditVisit.Value.Split('|')[0])
                {
                    if (Session["duration"] != null)
                    {
                        int[] iTemp = ((int[])Session["duration"]);
                        if (iTemp != null)
                            ddlDuration.Text = iTemp[ddlVisitType.SelectedIndex].ToString();
                    }
                    if (Session["description"] != null)
                    {
                        string[] sDesc = ((string[])Session["description"]);
                        if (sDesc != null)
                            txtVisitDescription.Text = sDesc[ddlVisitType.SelectedIndex];
                    }
                }
                else if (ddlVisitType.SelectedItem.Text == HdnEditVisit.Value.Split('|')[0])
                {

                    if (HdnEditVisit.Value.Split('|').Count() > 2)
                    {
                        ddlDuration.Text = HdnEditVisit.Value.Split('|')[1];
                    }
                    txtVisitDescription.Text = "";
                }
            }
            HdnEditVisit.Value = ddlVisitType.SelectedItem.Text + "|" + ddlDuration.Text;
            btnSave.Enabled = true;
        }

        protected void ddlReasonCode_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (ddlReasonCode.SelectedItem.Text.Trim() != "Other")
            {
                txtReasonCode.Text = ddlReasonCode.SelectedItem.Text;
                //rchtxtReasonText.Focus();
                //rchtxtReasonText.SelectionStart= cboReasonCode.Text.Length;

                TextBoxColorChange(txtReasonCode, false);
            }
            else
            {
                txtReasonCode.Text = string.Empty;
                txtReasonCode.Focus();

                TextBoxColorChange(txtReasonCode, true);
            }
            btnSave.Enabled = true;
        }

        protected void btnHumanDetailUpdate_Click(object sender, EventArgs e)
        {
            IList<Human> objhuman = new List<Human>();
            //HumanManager humanMngr = new HumanManager();
            //objhuman = humanMngr.patientdetails(hdnHumanID.Value);

            IList<string> ilstEditAppTagList = new List<string>();
            ilstEditAppTagList.Add("HumanList");

            IList<object> ilstEditAppFinal = new List<object>();
            ilstEditAppFinal = UtilityManager.ReadBlob(Convert.ToUInt64(hdnHumanID.Value), ilstEditAppTagList);

            if (ilstEditAppFinal.Count > 0 && ilstEditAppFinal != null)
            {
                if (ilstEditAppFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstEditAppFinal[0]).Count; iCount++)
                    {
                        objhuman.Add((Human)((IList<object>)ilstEditAppFinal[0])[iCount]);
                    }
                }

            }


            //string FileName = "Human" + "_" + hdnHumanID.Value + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    // itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();

            //        if (itemDoc.GetElementsByTagName("HumanList") != null && itemDoc.GetElementsByTagName("HumanList").Count > 0)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes;

            //            if (xmlTagName != null && xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    if (xmlTagName[j].Attributes["Id"].Value == hdnHumanID.Value)
            //                    {
            //                        Human objhumanlst = new Human();
            //                        objhumanlst.Id = Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Id").Value);
            //                        objhumanlst.Birth_Date = Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Birth_Date").Value);
            //                        objhumanlst.First_Name = xmlTagName[j].Attributes.GetNamedItem("First_Name").Value;
            //                        objhumanlst.Last_Name = xmlTagName[j].Attributes.GetNamedItem("Last_Name").Value;
            //                        objhumanlst.MI = xmlTagName[j].Attributes.GetNamedItem("MI").Value;
            //                        objhumanlst.Sex = xmlTagName[j].Attributes.GetNamedItem("Sex").Value;
            //                        objhumanlst.Suffix = xmlTagName[j].Attributes.GetNamedItem("Suffix").Value;
            //                        objhumanlst.Medical_Record_Number = xmlTagName[j].Attributes.GetNamedItem("Medical_Record_Number").Value;
            //                        objhumanlst.Home_Phone_No = xmlTagName[j].Attributes.GetNamedItem("Home_Phone_No").Value;
            //                        objhumanlst.Human_Type = xmlTagName[j].Attributes.GetNamedItem("Human_Type").Value;
            //                        objhumanlst.Patient_Account_External = xmlTagName[j].Attributes.GetNamedItem("Patient_Account_External").Value;
            //                        objhumanlst.Cell_Phone_Number = xmlTagName[j].Attributes.GetNamedItem("Cell_Phone_Number").Value;

            //                        objhuman.Add(objhumanlst);
            //                    }
            //                }
            //            }
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}
            if (objhuman.Count > 0)
            {
                txtPatientName.Text = objhuman[0].First_Name;
                txtPatientDOB.Text = objhuman[0].Birth_Date.ToString("dd-MMM-yyyy");
                txtHumanType.Text = objhuman[0].Human_Type;
                txtHomePhoneNumber.Text = objhuman[0].Home_Phone_No;
                txtCellPhoneNumber.Text = objhuman[0].Cell_Phone_Number;
                //  if (txtReferringProvider.Text.Trim() != "" || txtReferringProvider.Text.Trim() != string.Empty)
                // {

                string phoneno = "";

                if (objhuman != null && objhuman.Count > 0)
                {

                    if (objhuman[0].Home_Phone_No.Length == 14)
                    {
                        phoneno = objhuman[0].Home_Phone_No;
                    }
                    else
                    {
                        phoneno = objhuman[0].Cell_Phone_Number;
                    }

                }




                if (objhuman != null && objhuman.Count > 0)
                {
                    string sSex = string.Empty;
                    if (objhuman[0].Sex != null && objhuman[0].Sex.Trim() != "")
                        sSex = objhuman[0].Sex.Substring(0, 1);

                    divPatientstrip.InnerText = objhuman[0].Last_Name + "," + objhuman[0].First_Name +
               "  " + objhuman[0].MI + "  " + objhuman[0].Suffix + "   |   " +
                objhuman[0].Birth_Date.ToString("dd-MMM-yyyy") + "   |   " +
               (CalculateAge(objhuman[0].Birth_Date)).ToString() +
               "  year(s)    |   " + sSex + "   |   Acc #:" + objhuman[0].Id +
               "   |   " + "Med Rec #:" + objhuman[0].Medical_Record_Number + "   |   " +
               "Phone #:" + phoneno + "   |   Patient Type:" + objhuman[0].Human_Type;
                }
                else
                {
                    divPatientstrip.InnerText = " " + "   |" + "|" + "|" + "|" + "|";
                }
                if (tabReferringProvAndPCP.SelectedIndex == 0)
                {
                    if (chkSelfReferred.Visible)
                    {
                        if (chkSelfReferred.Checked)
                        {

                            // Commented by valli need to check

                            //  HdnRefPhy.Value = txtReferringProvider.Text + "|" + txtReferingAddress.Text + "|" + msktxtReferingPhoneNo.Text + "|" + msktxtReferingFaxNo.Text + "|" + txtProviderNPI.Text + "|" + txtReferringFacility.Text;//for static
                        }
                    }
                }

                //  }

                if (HdnPcpPhy.Value == "|||||" || HdnRefPhy.Value == "|||||")
                {
                    if (hdnEncounter_Physician_id.Value != "")
                    {
                        if (objhuman[0].Encounter_Provider_ID.ToString() != hdnEncounter_Physician_id.Value)
                        {
                            hdnEncounter_Physician_id.Value = objhuman[0].Encounter_Provider_ID.ToString();

                            if (hdnEncounter_Physician_id.Value != "" && hdnEncounter_Physician_id.Value != "0")//vasanth
                            {
                                //tabReferringProvAndPCP.SelectedIndex = 1;
                                //lblReferringName.Text = "PCP. Provider";
                                //lblReferingFacility.Text = "PCP. Facility";
                                //lblReferingAddress.Text = "PCP. Address";
                                //lblReferingPhoneNo.Text = "PCP. Phone";
                                //lblReferingFaxNo.Text = "PCP. Fax";
                                chkSelfReferred.Visible = false;
                                //  hdnEncounter_Physician_id.Value = fillneweditappt.Encounter_Provider_ID.ToString();
                                //PhysicianManager phyMngr = new PhysicianManager();
                                //IList<PhysicianLibrary> phylist = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(hdnEncounter_Physician_id.Value));
                                //if (phylist != null && phylist.Count > 0)
                                //{
                                PhysicianLibrary objPhyLib = GetPhysicianDetailsByPhyID(hdnEncounter_Physician_id.Value);//phylist[0];

                                if (objPhyLib != null)
                                {

                                    //old code
                                    // string sPhyName = objPhyLib.PhyPrefix + " " + objPhyLib.PhyFirstName + " " + objPhyLib.PhyLastName + " " + objPhyLib.PhySuffix;
                                    //Gitlab# 2485 - Physician Name Display Change
                                    string sPhyName = String.Empty;
                                    if (objPhyLib.PhyLastName != String.Empty)
                                        sPhyName += objPhyLib.PhyLastName;
                                    if (objPhyLib.PhyFirstName != String.Empty)
                                    {
                                        if (sPhyName != String.Empty)
                                            sPhyName += "," + objPhyLib.PhyFirstName;
                                        else
                                            sPhyName += objPhyLib.PhyFirstName;
                                    }
                                    if (objPhyLib.PhyMiddleName != String.Empty)
                                        sPhyName += " " + objPhyLib.PhyMiddleName;
                                    if (objPhyLib.PhySuffix != String.Empty)
                                        sPhyName += "," + objPhyLib.PhySuffix;


                                    //txtReferringProvider.Text = sPhyName;
                                    //txtReferringFacility.Text = objPhyLib.PhyNotes;
                                    //txtReferingAddress.Text = objPhyLib.PhyAddress1;
                                    //msktxtReferingPhoneNo.Text = objPhyLib.PhyTelephone;
                                    //msktxtReferingFaxNo.Text = objPhyLib.PhyFax;
                                    //txtProviderNPI.Text = objPhyLib.PhyNPI;
                                    //txtReferringFacility.Text = objPhyLib.PhyNotes;
                                    if (HdnRefPhy.Value == "|||||")
                                        HdnRefPhy.Value = sPhyName + "|" + objPhyLib.PhyAddress1 + "|" + objPhyLib.PhyTelephone + "|" + objPhyLib.PhyFax + "|" + objPhyLib.PhyNPI + "|" + objPhyLib.PhyNotes;
                                    if (HdnPcpPhy.Value == "|||||")
                                        HdnPcpPhy.Value = sPhyName + "|" + objPhyLib.PhyAddress1 + "|" + objPhyLib.PhyTelephone + "|" + objPhyLib.PhyFax + "|" + objPhyLib.PhyNPI + "|" + objPhyLib.PhyNotes;
                                    btnSave.Enabled = true;
                                }
                                //}
                            }
                            else
                            {
                                hdnEncounter_Physician_id.Value = "0";
                                HdnRefPhy.Value = "|||||";//for static
                                HdnPcpPhy.Value = "|||||";
                            }
                        }
                    }
                }
                else
                {
                    if (Request["Encounter_Provider_ID"] != null && Request["Encounter_Provider_ID"].ToString() != "")
                    {
                        if (Request["Encounter_Provider_ID"].ToString() == hdnEncounter_Physician_id.Value)
                        {
                            //   tabReferringProvAndPCP.SelectedIndex = 1;
                            //lblReferringName.Text = "PCP. Provider";
                            //lblReferingFacility.Text = "PCP. Facility";
                            //lblReferingAddress.Text = "PCP. Address";
                            //lblReferingPhoneNo.Text = "PCP. Phone";
                            //lblReferingFaxNo.Text = "PCP. Fax";
                            // chkSelfReferred.Visible = false;

                            PhysicianLibrary objPhyLib = GetPhysicianDetailsByPhyID(objhuman[0].Encounter_Provider_ID.ToString());//phylist[0];

                            if (objPhyLib != null)
                            {
                                //old code
                                //string sPhyName = objPhyLib.PhyPrefix + " " + objPhyLib.PhyFirstName + " " + objPhyLib.PhyLastName + " " + objPhyLib.PhySuffix;
                                //Gitlab# 2485 - Physician Name Display Change
                                string sPhyName = string.Empty;
                                if (objPhyLib.PhyLastName != String.Empty)
                                    sPhyName += objPhyLib.PhyLastName;
                                if (objPhyLib.PhyFirstName != String.Empty)
                                {
                                    if (sPhyName != String.Empty)
                                        sPhyName += "," + objPhyLib.PhyFirstName;
                                    else
                                        sPhyName += objPhyLib.PhyFirstName;
                                }
                                if (objPhyLib.PhyMiddleName != String.Empty)
                                    sPhyName += " " + objPhyLib.PhyMiddleName;
                                if (objPhyLib.PhySuffix != String.Empty)
                                    sPhyName += "," + objPhyLib.PhySuffix;

                                //txtReferringProvider.Text = sPhyName;
                                //txtReferringFacility.Text = objPhyLib.PhyNotes;
                                //txtReferingAddress.Text = objPhyLib.PhyAddress1;
                                //msktxtReferingPhoneNo.Text = objPhyLib.PhyTelephone;
                                //msktxtReferingFaxNo.Text = objPhyLib.PhyFax;
                                //txtProviderNPI.Text = objPhyLib.PhyNPI;
                                //txtReferringFacility.Text = objPhyLib.PhyNotes;

                                HdnPcpPhy.Value = sPhyName + "|" + objPhyLib.PhyAddress1 + "|" + objPhyLib.PhyTelephone + "|" + objPhyLib.PhyFax + "|" + objPhyLib.PhyNPI + "|" + objPhyLib.PhyNotes;
                                btnSave.Enabled = true;
                            }
                        }
                    }

                    hdnEncounter_Physician_id.Value = objhuman[0].Encounter_Provider_ID.ToString();
                }

            }

        }

        protected void btnAppointmentSlot_Click(object sender, EventArgs e)
        {
            if (hdnApptDate.Value != null && hdnApptDate.Value != string.Empty)
            {
                dtpApptDate.SelectedDate = Convert.ToDateTime(hdnApptDate.Value);
            }
            if (hdnApptTime.Value != null && hdnApptTime.Value != string.Empty)
            {
                string[] splitTxt = hdnApptTime.Value.Split(' ');
                DateTime dt = Convert.ToDateTime(splitTxt[0].ToString());
                dtpStartTime.SelectedTime = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
            }
            if (hdnFacilityName.Value != null && hdnFacilityName.Value != string.Empty)
            {
                cboFacility.Items.FindItemByText(hdnFacilityName.Value).Selected = true;
            }
            if (hdnProviderName.Value != null && hdnProviderName.Value != string.Empty)
            {
                ddlPhysicianName.Items.FindItemByText(hdnProviderName.Value).Selected = true;
            }
        }
        protected void cboOrder_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ulordID = 0;
            btnSave.Enabled = true;
            //Cap - 2505
            //if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty)
            if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty && cboOrder.SelectedItem.Text != "Akido Order")
            {
                string[] str = cboOrder.SelectedItem.Value.Split('@');

                //Commented by Valli

                if (str[0] != "" && str.Count() > 1)
                {
                    txtProviderSearch.Text = "Ord. Provider: " + str[0].ToString() + " | Provider NPI: " + str[1].ToString() +
                        " | Ord. Facility: " + str[2].ToString() +
                        " | Ord. Address: " + str[3].ToString() +
                        " | Ord. Phone #: " + str[4].ToString() +
                        " | Ord. Fax #: " + str[5].ToString();

                    hdnrenprovider.Value = str[0].ToString() + "| NPI: " + str[1].ToString() + "| Facility: " + str[2].ToString() + "| Address:" + str[3].ToString() + "| Phone No:"
                              + str[5].ToString() + "| Fax No:" + str[4].ToString();

                    hdnrenprovidersearch.Value = "Ord. Provider: " + str[0].ToString() + " | Provider NPI: " + str[1].ToString() +
                    " | Ord. Facility: " + str[2].ToString() +
                    " | Ord. Address: " + str[3].ToString() +
                    " | Ord. Phone #: " + str[4].ToString() +
                    " | Ord. Fax #: " + str[5].ToString();

                    //if (str[0] != null)
                    //    txtReferringProvider.Text = str[0].ToString();
                    //if (str[1] != null)
                    //    txtProviderNPI.Text = str[1].ToString();
                    //if (str[2] != null)
                    //    txtReferringFacility.Text = str[2].ToString();
                    //if (str[3] != null)
                    //    txtReferingAddress.Text = str[3].ToString();
                    //if (str[4] != null)
                    //    msktxtReferingPhoneNo.Text = str[4].ToString();
                    //if (str[5] != null)
                    //    msktxtReferingFaxNo.Text = str[5].ToString();
                    //txtReferringProvider.ReadOnly = true;
                    //txtProviderNPI.ReadOnly = true;
                    //txtReferringFacility.ReadOnly = true;
                    //txtReferingAddress.ReadOnly = true;
                    //msktxtReferingPhoneNo.ReadOnly = true;
                    //msktxtReferingFaxNo.ReadOnly = true;
                    //txtReferringProvider.Attributes.Add("onkeypress", "return false;");
                    //txtReferringFacility.Attributes.Add("onkeypress", "return false;");
                    //txtReferingAddress.Attributes.Add("onkeypress", "return false;");
                    //txtProviderNPI.Attributes.Add("onkeypress", "return false;");
                    //txtProviderNPI.Attributes.Add("onkeydown", "return false;");
                    //txtReferringProvider.Attributes.Add("onkeydown", "return false;");
                    //txtReferringFacility.Attributes.Add("onkeydown", "return false;");
                    //txtReferingAddress.Attributes.Add("onkeydown", "return false;");

                    //txtProviderNPI.Style.Add("background-color", "#BFDBFF");


                    //txtReferingAddress.Style.Add("background-color", "#BFDBFF");


                    //txtReferringProvider.Style.Add("background-color", "#BFDBFF");

                    //txtReferringFacility.Style.Add("background-color", "#BFDBFF");

                    //msktxtReferingPhoneNo.Style.Add("background-color", "#BFDBFF");

                    //msktxtReferingFaxNo.Style.Add("background-color", "#BFDBFF");

                }
                else
                {
                    hdnrenprovider.Value = "";

                    hdnrenprovidersearch.Value = "";
                    txtProviderSearch.Text = string.Empty;
                    hdnRefEditPhyId.Value = string.Empty;
                    //txtReferringProvider.ReadOnly = false;
                    //txtProviderNPI.ReadOnly = false;
                    //txtReferringFacility.ReadOnly = false;
                    //txtReferingAddress.ReadOnly = false;
                    //msktxtReferingPhoneNo.ReadOnly = false;
                    //msktxtReferingFaxNo.ReadOnly = false;
                    //txtProviderNPI.Attributes.Remove("onkeypress");
                    //txtProviderNPI.Attributes.Remove("onkeydown");
                    //txtProviderNPI.Style.Add("background-color", "#ffffff");

                    //txtReferingAddress.Attributes.Remove("onkeypress");
                    //txtReferingAddress.Attributes.Remove("onkeydown");
                    //txtReferingAddress.Style.Add("background-color", "#ffffff");

                    //txtReferringProvider.Attributes.Remove("onkeydown");
                    //txtReferringProvider.Attributes.Remove("onkeypress");
                    //txtReferringProvider.Style.Add("background-color", "#ffffff");

                    //txtReferringFacility.Attributes.Remove("onkeydown");
                    //txtReferringFacility.Attributes.Remove("onkeypress");
                    //txtReferringFacility.Style.Add("background-color", "#ffffff");

                    //msktxtReferingPhoneNo.Style.Add("background-color", "#ffffff");

                    //msktxtReferingFaxNo.Style.Add("background-color", "#ffffff");

                    //txtReferringProvider.Text = string.Empty;
                    //txtProviderNPI.Text = string.Empty;
                    //txtReferringFacility.Text = string.Empty;
                    //txtReferingAddress.Text = string.Empty;
                    //msktxtReferingPhoneNo.Text = string.Empty;
                    //msktxtReferingFaxNo.Text = string.Empty;
                }
                lblProviderSearch.ForeColor = Color.Black;
                lblProviderSearch.Text = lblProviderSearch.Text.Replace("*", "");

                //Cap - 2594
                imgClearProviderText.Visible = false;
                txtProviderSearch.Enabled = false;
                imgEditProvider.Visible = false;

            }
            //Cap - 1179
            else
            {
                //Cap - 2594
                imgClearProviderText.Visible = true;
                txtProviderSearch.Enabled = true;
                imgEditProvider.Visible = true;
                imgEditProvider.Style.Add("display", "block");


                hdnrenprovider.Value = "";
                hdnrenprovidersearch.Value = "";
                txtProviderSearch.Text = string.Empty;
                txtProviderSearch.Enabled = true;
                hdnRefEditPhyId.Value = string.Empty;
                if (cboOrder.SelectedItem.Text == "Akido Order")
                {
                    lblProviderSearch.ForeColor = Color.Red;
                    lblProviderSearch.Text += "*";
                }
            }
            //{
            //    //txtReferringProvider.Text = string.Empty;
            //    //txtProviderNPI.Text = string.Empty;
            //    //txtReferringFacility.Text = string.Empty;
            //    //txtReferingAddress.Text = string.Empty;
            //    //msktxtReferingPhoneNo.Text = string.Empty;
            //    //msktxtReferingFaxNo.Text = string.Empty;

            //    PhysicianLibrary objPhysicianLibrary = GetPhysicianDetailsByPhyID(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value);//PhyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value));
            //    if (objPhysicianLibrary != null)
            //    {
            //        FacilityManager obj = new FacilityManager();
            //        IList<FacilityLibrary> lstfacirty = obj.GetFacilityByFacilityname(cboFacility.SelectedItem.Text);
            //        string sPhyName = string.Empty;
            //        if (objPhysicianLibrary != null)
            //        {
            //            string facilityadd = "";
            //            if (lstfacirty.Count > 0)
            //            {
            //                facilityadd = lstfacirty[0].Fac_Address1;
            //            }
            //            //old code
            //            // sPhyName = objPhysicianLibrary.PhyPrefix + " " + objPhysicianLibrary.PhyFirstName + " " + objPhysicianLibrary.PhyLastName + " " + objPhysicianLibrary.PhySuffix;
            //            //Gitlab# 2485 - Physician Name Display Change
            //            sPhyName = string.Empty;
            //            if (objPhysicianLibrary.PhyLastName != String.Empty)
            //                sPhyName += objPhysicianLibrary.PhyLastName;
            //            if (objPhysicianLibrary.PhyFirstName != String.Empty)
            //            {
            //                if (sPhyName != String.Empty)
            //                    sPhyName += "," + objPhysicianLibrary.PhyFirstName;
            //                else
            //                    sPhyName += objPhysicianLibrary.PhyFirstName;
            //            }
            //            if (objPhysicianLibrary.PhyMiddleName != String.Empty)
            //                sPhyName += " " + objPhysicianLibrary.PhyMiddleName;
            //            if (objPhysicianLibrary.PhySuffix != String.Empty)
            //                sPhyName += "," + objPhysicianLibrary.PhySuffix;
            //            if (sPhyName.Trim() != "")
            //            {
            //                hdnrenprovider.Value = objPhysicianLibrary.Id + "|" + sPhyName + "|" + objPhysicianLibrary.PhyNPI + "|" + "" + "|" + cboFacility.SelectedItem.Text + "|" +
            //                   facilityadd + "|" + objPhysicianLibrary.PhyTelephone + "|" + objPhysicianLibrary.PhyFax;
            //                //Jira #CAP-156 - Index was outside bounds 
            //                //hdnrenprovidersearch.Value = objPhysicianLibrary.PhyLastName + ", " + objPhysicianLibrary.PhyFirstName + " " +
            //                //    objPhysicianLibrary.PhyMiddleName + "(" + objPhysicianLibrary.PhySuffix + ")" + " | " +
            //                //                                      "NPI:" + objPhysicianLibrary.PhyNPI + " | " +
            //                //                                     "" + " | " +
            //                //                                      "FACILITY:" + cboFacility.SelectedItem.Text + " | " +
            //                //                                      "ADDR: " + facilityadd + ", " +
            //                //                                      objPhysicianLibrary.PhyCity + "," +
            //                //                                      objPhysicianLibrary.PhyState + " " +
            //                //                                      objPhysicianLibrary.PhyZip + " | " +
            //                //                                      ((objPhysicianLibrary.PhyTelephone.Trim()) != "" ? "PH:" + objPhysicianLibrary.PhyTelephone + " | " : "") +
            //                //                                      (objPhysicianLibrary.PhyFax.Trim() != "" ? "FAX:" + objPhysicianLibrary.PhyFax : "");

            //                hdnrenprovidersearch.Value = objPhysicianLibrary.PhyLastName + ", " + objPhysicianLibrary.PhyFirstName + " " +
            //                     objPhysicianLibrary.PhyMiddleName + "(" + objPhysicianLibrary.PhySuffix + ")" + " | " +
            //                                                       "NPI:" + objPhysicianLibrary.PhyNPI + " | " +
            //                                                      "" + " | " +
            //                                                       "FACILITY:" + cboFacility.SelectedItem.Text + " | " +
            //                                                       "ADDR: " + facilityadd + ", " +
            //                                                       objPhysicianLibrary.PhyCity + "," +
            //                                                       objPhysicianLibrary.PhyState + " " +
            //                                                       objPhysicianLibrary.PhyZip + " | " +
            //                                                       ((objPhysicianLibrary.PhyTelephone.Trim()) != "" ? "PH:" + objPhysicianLibrary.PhyTelephone + " | " : "PH: | ") +
            //                                                       (objPhysicianLibrary.PhyFax.Trim() != "" ? "FAX:" + objPhysicianLibrary.PhyFax : "FAX:");

            //                //Jira #CAP-156 - Index was outside bounds 
            //                //txtProviderSearch.Text = objPhysicianLibrary.PhyLastName + ", " + objPhysicianLibrary.PhyFirstName + " " +
            //                //    objPhysicianLibrary.PhyMiddleName + "(" + objPhysicianLibrary.PhySuffix + ")" + " | " +
            //                //                                      "NPI:" + objPhysicianLibrary.PhyNPI + " | " +
            //                //                                     "" + " | " +
            //                //                                      "FACILITY:" + cboFacility.SelectedItem.Text + " | " +
            //                //                                      "ADDR: " + facilityadd + ", " +
            //                //                                      objPhysicianLibrary.PhyCity + "," +
            //                //                                      objPhysicianLibrary.PhyState + " " +
            //                //                                      objPhysicianLibrary.PhyZip + " | " +
            //                //                                      ((objPhysicianLibrary.PhyTelephone.Trim()) != "" ? "PH:" + objPhysicianLibrary.PhyTelephone + " | " : "") +
            //                //                                      (objPhysicianLibrary.PhyFax.Trim() != "" ? "FAX:" + objPhysicianLibrary.PhyFax : "");
            //                txtProviderSearch.Text = objPhysicianLibrary.PhyLastName + ", " + objPhysicianLibrary.PhyFirstName + " " +
            //                   objPhysicianLibrary.PhyMiddleName + "(" + objPhysicianLibrary.PhySuffix + ")" + " | " +
            //                                                     "NPI:" + objPhysicianLibrary.PhyNPI + " | " +
            //                                                    "" + " | " +
            //                                                     "FACILITY:" + cboFacility.SelectedItem.Text + " | " +
            //                                                     "ADDR: " + facilityadd + ", " +
            //                                                     objPhysicianLibrary.PhyCity + "," +
            //                                                     objPhysicianLibrary.PhyState + " " +
            //                                                     objPhysicianLibrary.PhyZip + " | " +
            //                                                     ((objPhysicianLibrary.PhyTelephone.Trim()) != "" ? "PH:" + objPhysicianLibrary.PhyTelephone + " | " : "PH: | ") +
            //                                                     (objPhysicianLibrary.PhyFax.Trim() != "" ? "FAX:" + objPhysicianLibrary.PhyFax : "FAX:");

            //            }
            //        }
            //    }
            //}
        }
        protected void cboFacility_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (hdnrenprovidersearch.Value != "" && tabReferringProvAndPCP.SelectedIndex == 0)

                txtProviderSearch.Text = hdnrenprovidersearch.Value;

            if (hdnpcpprovidersearch.Value != "" && tabReferringProvAndPCP.SelectedIndex == 1)

                txtProviderSearch.Text = hdnpcpprovidersearch.Value;
            btnSave.Enabled = true;
            IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
            if (chkShowAllPhysicians.Checked == true)
            {
                PhysicianList = UtilityManager.GetPhysicianList("", ClientSession.LegalOrg);
                //CAP-3499
                if (PhysicianList != null)
                {
                    var otherPhysicianList = UtilityManager.GetInActiveProviderList("", ClientSession.LegalOrg, true);
                    foreach (var physician in otherPhysicianList)
                    {
                        PhysicianList.Add(physician);
                    }
                }
            }
            else
            {
                if (cboFacility.SelectedItem != null)
                {
                    PhysicianList = UtilityManager.GetPhysicianList(cboFacility.SelectedItem.Text, ClientSession.LegalOrg);//PhyMngr.GetPhysicianListbyFacility(cboFacility.SelectedItem.Text, "Y");                
                    //CAP-3499
                    if (PhysicianList != null)
                    {
                        var otherPhysicianList = UtilityManager.GetInActiveProviderList(cboFacility.SelectedItem.Text.Trim(), ClientSession.LegalOrg, true);
                        foreach (var physician in otherPhysicianList)
                        {
                            PhysicianList.Add(physician);
                        }
                    }
                }
                //if (PhysicianList != null)
                //{
                //if (cboFacility.Text.ToUpper() == System.Configuration.ConfigurationManager.AppSettings["CMGFacilityName"].ToString().ToUpper())//staticMngr.getStaticLookupByFieldName("CMG FACILITY NAME")[0].Value.ToUpper())
                //{
                //    PhysicianList = PhysicianList.Where(a => a.Category == "MACHINE").ToList<PhysicianLibrary>();
                //}                    
                //}
            }
            ddlPhysicianName.Items.Clear();


            RadComboBoxItem item = new RadComboBoxItem();
            item.Value = "0";
            item.Text = "";

            bool phyCheck = false;

            XmlDocument xmldoc = new XmlDocument();
            ddlPhysicianName.Items.Add(item);



            if (PhysicianList != null)
            {
                MachinetechnicianList machinetechnicianList = new MachinetechnicianList();
                machinetechnicianList = ConfigureBase<MachinetechnicianList>.ReadJson("machine_technician.json");
                for (int i = 0; i < PhysicianList.Count; i++)
                {
                    item = new RadComboBoxItem();

                    //if (sAncillary != string.Empty && sAncillary == cboFacility.SelectedItem.Text.Trim())
                    //{
                    var fac = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFac = fac.ToList<FacilityLibrary>();
                    if (ilstFac.Count > 0 && ilstFac[0].Is_Ancillary == "Y")
                    {
                        //Jira CAP-2777
                        //string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
                        //if (File.Exists(strXmlFilePathTech) == true)
                        {
                            //Jira CAP-2777
                            //xmldoc = new XmlDocument();
                            //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                            if (PhysicianList[i].PhyColor != "0")
                            {
                                //Jira CAP-2777
                                //XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + PhysicianList[i].PhyColor);
                                //if (xmlTec != null)
                                //{
                                //    item.Text = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                //    item.Value = xmlTec[0].Attributes.GetNamedItem("machine_technician_library_id").Value;
                                //}
                                //Jira CAP-2777
                                if (machinetechnicianList?.MachineTechnician != null)
                                {
                                    List<Machinetechnician> machinetechnicians = new List<Machinetechnician>();
                                    machinetechnicians = machinetechnicianList.MachineTechnician.Where(x => x.machine_technician_library_id == PhysicianList[i].PhyColor).ToList();
                                    if (machinetechnicians.Count > 0)
                                    {
                                        item.Text = machinetechnicians[0].machine_name + " - " + PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                        item.Value = machinetechnicians[0].machine_technician_library_id;
                                    }
                                }
                            }
                            else
                            {
                                item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                item.Value = PhysicianList[i].Id.ToString();
                            }

                            ddlPhysicianName.Items.Add(item);
                            if (item.Value == ulMyPhysicianID.ToString())
                            {
                                phyCheck = true;
                                ddlPhysicianName.SelectedIndex = i + 1;
                                hdnEditApptPhyID.Value = ddlPhysicianName.SelectedValue.ToString();//added by vasanth
                            }
                        }
                    }
                    else
                    {
                        //old code
                        // string sPhyName = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyLastName;
                        //Gitlab# 2485 - Physician Name Display Change

                        //Jira CAP-2777
                        //string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
                        //if (File.Exists(strXmlFilePathTech) == true)
                        {
                            string sPhyName = string.Empty;
                            string PhyId = string.Empty;
                            //Jira CAP-2777
                            //xmldoc = new XmlDocument();
                            //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                            if (PhysicianList[i].PhyColor != "" && PhysicianList[i].PhyColor != "0")
                            {
                                //Jira CAP-2777
                                //XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + PhysicianList[i].PhyColor);
                                //if (xmlTec != null)
                                //{
                                //    sPhyName = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                //    PhyId = xmlTec[0].Attributes.GetNamedItem("machine_technician_library_id").Value;
                                //}

                                //Jira CAP-2777
                                if (machinetechnicianList?.MachineTechnician != null)
                                {
                                    List<Machinetechnician> machinetechnicians = new List<Machinetechnician>();
                                    machinetechnicians = machinetechnicianList.MachineTechnician.Where(x => x.machine_technician_library_id == PhysicianList[i].PhyColor).ToList();
                                    if (machinetechnicians.Count > 0)
                                    {
                                        sPhyName = machinetechnicians[0].machine_name + " - " + PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                        PhyId = machinetechnicians[0].machine_technician_library_id;
                                    }
                                }
                            }
                            else
                            {
                                if (PhysicianList[i].PhyLastName != String.Empty)
                                    sPhyName += PhysicianList[i].PhyLastName;
                                if (PhysicianList[i].PhyFirstName != String.Empty)
                                {
                                    if (sPhyName != String.Empty)
                                        sPhyName += "," + PhysicianList[i].PhyFirstName;
                                    else
                                        sPhyName += PhysicianList[i].PhyFirstName;
                                }
                                if (PhysicianList[i].PhyMiddleName != String.Empty)
                                    sPhyName += " " + PhysicianList[i].PhyMiddleName;
                                if (PhysicianList[i].PhySuffix != String.Empty)
                                    sPhyName += "," + PhysicianList[i].PhySuffix;
                            }
                            item = new RadComboBoxItem();
                            item.Value = PhysicianList[i].Id.ToString();
                            item.Text = sPhyName;
                            ddlPhysicianName.Items.Add(item);
                        }
                        //if (ddlPhysicianName.Items[i].Value == hdnPhysicianID.Value)
                        //{
                        //Modified by balaji.Tj
                        //if (PhysicianList[i].PhyNotes == "Default" && chkShowAllPhysicians.Checked == false && ddlPhysicianName.Items[i].Value != hdnPhysicianID.Value)

                        // }
                    }
                    if (PhysicianList[i].PhyNotes == "Default" && chkShowAllPhysicians.Checked == false && ddlPhysicianName.Items[i + 1].Value != hdnPhysicianID.Value)
                    {
                        ddlPhysicianName.SelectedIndex = i + 1;
                    }
                    else if (PhysicianList[i].PhyNotes != "Default" && chkShowAllPhysicians.Checked == false && ddlPhysicianName.Items[i + 1].Value == hdnPhysicianID.Value)
                    //else if (PhysicianList[i].PhyNotes != "Default" && chkShowAllPhysicians.Checked == false && ddlPhysicianName.Items[i].Value == hdnPhysicianID.Value)
                    {
                        ddlPhysicianName.SelectedIndex = i + 1;
                    }
                    else if (PhysicianList[i].PhyNotes == "Default" && chkShowAllPhysicians.Checked == false && ddlPhysicianName.Items[i + 1].Value == hdnPhysicianID.Value)
                    //else if (PhysicianList[i].PhyNotes == "Default" && chkShowAllPhysicians.Checked == false && ddlPhysicianName.Items[i].Value == hdnPhysicianID.Value)
                    {
                        ddlPhysicianName.SelectedIndex = i + 1;
                    }
                    else if (chkShowAllPhysicians.Checked == true)
                    {
                        if (ddlPhysicianName.Items[i].Value == hdnPhysicianID.Value && hdnPhysicianID.Value != "0")
                        {
                            ddlPhysicianName.SelectedIndex = i + 1;
                        }
                    }
                }
                //CAP-2024
                var comboBoxItems = ddlPhysicianName.Items.Cast<RadComboBoxItem>().ToList();
                ddlPhysicianName.Items.Clear();
                //CAP-3693
                ddlPhysicianName.Items.AddRange(comboBoxItems.Where(a => !string.IsNullOrWhiteSpace(a.Text)).OrderBy(a => a.Text.Trim()).ToArray());
            }

            ClientSession.PhysicianId = ddlPhysicianName.SelectedValue != null && ddlPhysicianName.SelectedValue.Trim() != "" ? Convert.ToUInt64(ddlPhysicianName.SelectedValue) : 0;
            hdnEditApptPhyID.Value = ddlPhysicianName.SelectedValue != null && ddlPhysicianName.SelectedValue.Trim() != "" ? ddlPhysicianName.SelectedValue.ToString() : "0";

            FillPOV();
            hdnFacilityName.Value = cboFacility.SelectedItem.Text;

            //if (cboFacility.SelectedItem.Text.ToUpper() == sFacilityCmg.ToUpper())
            //{
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
            {
                //Cap - 3361
                //ChangeproviderforCMGAncillary(true);
                hdnFacility.Value = "true";
            }
            else
            {
                ChangeproviderforCMGAncillary(false);
                hdnFacility.Value = "false";
            }
            if (chkSelfReferred.Checked == true && ddlPhysicianName.SelectedValue.Trim() != "")
            {
                chkSelfReferred.Checked = false;
                chkSelfReferred_CheckedChanged(sender, e);
            }
            else
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FacilitySelected", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        public void ChangeproviderforCMGAncillary(Boolean bcheck)
        {
            if (bcheck)
            {
                cboOrder.Items.Clear();
                cboOrder.Enabled = true;
                //if (ClientSession.FacilityName != null && ClientSession.FacilityName.ToUpper() == sFacilityCmg.ToUpper())
                //if (cboFacility.SelectedItem.Text.ToUpper() == sFacilityCmg.ToUpper())
                //{
                var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                {
                    btnOrder.Enabled = true;
                }
                else
                {
                    btnOrder.Enabled = false;
                }

                imgClearProviderText.Style.Add("top", "246px !important");
                imgEditProvider.Style.Add("top", "246px !important");
                lblOrder.ForeColor = Color.Red;
                lblOrder.Text = lblOrder.Text.Replace("*", "");
                lblOrder.Text += "*";
                tabReferringProvAndPCP.Tabs[0].Text = "Enter Ord provider details";
                //lblReferringName.Text = "Ord. Provider";
                //lblReferingFacility.Text = "Ord. Facility";
                //lblReferingAddress.Text = "Ord. Address";
                //lblReferingPhoneNo.Text = "Ord. Phone #";
                //lblReferingFaxNo.Text = "Ord. Fax #";
                lblProviderName.Text = "Machine - Technician";
                //btnFindPhysician.Enabled = false;
                chkSelfReferred.Enabled = false;
                OrdersManager objorder = new OrdersManager();
                IList<string> ilstOrder = new List<string>();

                if (hdnHumanID.Value != null && hdnHumanID.Value != "")
                {
                    ulong ulHuman = 0;
                    ulHuman = Convert.ToUInt32(hdnHumanID.Value);

                    var ancillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstAncillaryFacility = ancillary.ToList<FacilityLibrary>();

                    //XDocument xmlLab = XDocument.Load(Server.MapPath(@"ConfigXML\LabList.xml"));
                    //IEnumerable<XElement> xml = xmlLab.Element("LabList")
                    //   .Elements("Lab").Where(a => a.Attribute("type").Value.ToString() != "DME" && a.Attribute("name").Value.ToString() == ilstAncillaryFacility[0].Short_Name)
                    //   .OrderBy(s => (int)s.Attribute("sort_order"));
                    //string xmlValue = string.Empty;
                    //if (xml != null)
                    //{
                    //    foreach (XElement LabElement in xml)
                    //    {
                    //        xmlValue = LabElement.Attribute("id").Value;
                    //    }
                    //}

                    //CAP-2773
                    Lablist objlablist = new Lablist();
                    objlablist = ConfigureBase<Lablist>.ReadJson("LabList.json");
                    List<Labs> listLabList = new List<Labs>();
                    listLabList = objlablist.Lab.Where(a => a.type != "DME" && a.name == ilstAncillaryFacility[0].Short_Name)
                        .OrderBy(s => (int)Convert.ToInt32(s.sort_order)).ToList();
                    string xmlValue = string.Empty;
                    if (listLabList != null)
                    {
                        foreach (Labs objlab in listLabList)
                        {
                            xmlValue = objlab.id;
                        }
                    }

                    //Cap - 2505
                    if (ConfigurationSettings.AppSettings["IsAkidoOrder"] != null && ConfigurationSettings.AppSettings["IsAkidoOrder"] == "Y")
                    {
                        cboOrder.Items.Add(new RadComboBoxItem());
                        cboOrder.Items.Add("Akido Order");
                    }
                    //Cap - 3361
                    //ilstOrder = objorder.GetOrderByHuman(ulHuman, cboFacility.SelectedItem.Text, Convert.ToUInt64(xmlValue));                    
                    ilstOrder = objorder.GetOrderByHuman(ulHuman, cboFacility.SelectedItem.Text, Convert.ToUInt64(xmlValue),true);
                    if (ilstOrder != null && ilstOrder.Count() > 0)
                    {
                        //Cap - 2505
                        if(cboOrder.Items.Count==0)
                         cboOrder.Items.Add(new RadComboBoxItem());

                        for (int i = 0; i < ilstOrder.Count; i++)
                        {
                            string[] str = ilstOrder[i].ToString().Split('^');
                            RadComboBoxItem ilst = new RadComboBoxItem();
                            ilst.Text = str[0];
                            if (ilstOrder[i].ToString().Contains('^'))
                                ilst.Value = str[1];
                            else
                                ilst.Value = ilstOrder[i].ToString().Split('|')[0];
                            cboOrder.Items.Add(ilst);
                        }
                    }
                    Boolean val = false;
                    if (ulordID != 0)
                    {
                        int selectorder = 0;
                        for (int j = 0; j < cboOrder.Items.Count; j++)
                        {
                            if (cboOrder.Items[j].Text.Split('|')[0].ToString().Trim() == ulordID.ToString())
                            {
                                selectorder = j;
                                if (cboOrder.Items[j].Text.Split('|')[2].ToString().Trim() != "")
                                    val = true;
                                break;
                            }
                        }
                        cboOrder.SelectedIndex = selectorder;
                        //Cap - 2505
                        if(ulordID!=0 && cboOrder.SelectedIndex==0)
                        {
                            cboOrder.SelectedIndex = 1;
                        }

                    }
                    //Cap - 2505
                    if(ulMyEncID !=0 && ulordID==0)
                    {
                        cboOrder.SelectedIndex = 1;
                    }
                    //Cap - 3045
                    if (tabReferringProvAndPCP.SelectedIndex == 0 && cboOrder != null && cboOrder.SelectedItem.Text != "Akido Order")
                    {
                        imgClearProviderText.Visible = false;
                        imgEditProvider.Visible = false;
                    }
                    else if (tabReferringProvAndPCP.SelectedIndex == 0 && cboOrder != null && cboOrder.SelectedItem.Text == "Akido Order")
                    {
                        imgClearProviderText.Visible = true;
                        imgEditProvider.Visible = true;
                    }
                    if (tabReferringProvAndPCP.SelectedIndex == 1)
                    {
                        imgClearProviderText.Visible = true;
                        imgEditProvider.Visible = true;
                    }


                    //if (cboOrder.Items.Count > 0)
                    //    btnOrder.Enabled = true;
                    //else
                    //{
                    //    btnOrder.Enabled = true;
                    //}
                    //For bug ID 52793 
                    if (val)
                    {
                        //txtReferringProvider.ReadOnly = true;
                        //txtProviderNPI.ReadOnly = true;
                        //txtReferringFacility.ReadOnly = true;
                        //txtReferingAddress.ReadOnly = true;
                        //msktxtReferingPhoneNo.ReadOnly = true;
                        //msktxtReferingFaxNo.ReadOnly = true;
                        //txtReferringProvider.Attributes.Add("onkeypress", "return false;");
                        //txtReferringFacility.Attributes.Add("onkeypress", "return false;");
                        //txtReferingAddress.Attributes.Add("onkeypress", "return false;");
                        //txtProviderNPI.Attributes.Add("onkeypress", "return false;");
                        //txtProviderNPI.Attributes.Add("onkeydown", "return false;");
                        //txtReferringProvider.Attributes.Add("onkeydown", "return false;");
                        //txtReferringFacility.Attributes.Add("onkeydown", "return false;");
                        //txtReferingAddress.Attributes.Add("onkeydown", "return false;");

                        //txtProviderNPI.Style.Add("background-color", "#BFDBFF");


                        //txtReferingAddress.Style.Add("background-color", "#BFDBFF");


                        //txtReferringProvider.Style.Add("background-color", "#BFDBFF");

                        //txtReferringFacility.Style.Add("background-color", "#BFDBFF");

                        //msktxtReferingPhoneNo.Style.Add("background-color", "#BFDBFF");

                        //msktxtReferingFaxNo.Style.Add("background-color", "#BFDBFF");
                    }

                    else
                    {
                        //txtReferringProvider.ReadOnly = false;
                        //txtProviderNPI.ReadOnly = false;
                        //txtReferringFacility.ReadOnly = false;
                        //txtReferingAddress.ReadOnly = false;
                        //msktxtReferingPhoneNo.ReadOnly = false;
                        //msktxtReferingFaxNo.ReadOnly = false;
                        //txtProviderNPI.Attributes.Remove("onkeypress");
                        //txtProviderNPI.Attributes.Remove("onkeydown");
                        //txtProviderNPI.Style.Add("background-color", "#ffffff");

                        //txtReferingAddress.Attributes.Remove("onkeypress");
                        //txtReferingAddress.Attributes.Remove("onkeydown");
                        //txtReferingAddress.Style.Add("background-color", "#ffffff");

                        //txtReferringProvider.Attributes.Remove("onkeydown");
                        //txtReferringProvider.Attributes.Remove("onkeypress");
                        //txtReferringProvider.Style.Add("background-color", "#ffffff");

                        //txtReferringFacility.Attributes.Remove("onkeydown");
                        //txtReferringFacility.Attributes.Remove("onkeypress");
                        //txtReferringFacility.Style.Add("background-color", "#ffffff");

                        //msktxtReferingPhoneNo.Style.Add("background-color", "#ffffff");

                        //msktxtReferingFaxNo.Style.Add("background-color", "#ffffff");
                    }
                }
            }
            else
            {
                if (tabReferringProvAndPCP.SelectedIndex == 0)
                {
                    imgClearProviderText.Visible = true;
                    imgEditProvider.Visible = true;
                }
                imgClearProviderText.Style.Add("top", "234px !important");
                imgEditProvider.Style.Add("top", "234px !important");
                cboOrder.Items.Clear();
                cboOrder.Enabled = false;
                btnOrder.Enabled = false;
                lblOrder.ForeColor = Color.Black;
                lblOrder.Text = lblOrder.Text.Replace("*", "");
                tabReferringProvAndPCP.Tabs[0].Text = "Referring Provider Info";
                //lblReferringName.Text = "Ref. Provider";
                //lblReferingFacility.Text = "Ref. Facility";
                //lblReferingAddress.Text = "Ref. Address";
                //lblReferingPhoneNo.Text = "Ref. Phone #";
                //lblReferingFaxNo.Text = "Ref. Fax #";
                lblProviderName.Text = "Prov. Name";
                // btnFindPhysician.Enabled = true;
                chkSelfReferred.Enabled = true;
            }
        }
        protected void btnOrderCreate_Click(object sender, EventArgs e)
        {
            if (cboOrder.Items.Count > 0)
            {
                if (hdnOrderSubmitdata.Value != null && hdnOrderSubmitdata.Value != string.Empty)
                {
                    RadComboBoxItem ilst = new RadComboBoxItem();
                    ilst.Text = hdnOrderSubmitdata.Value;
                    ilst.Value = hdnOrderSubmitdata.Value.Split('|')[0].ToString();
                    cboOrder.Items.Add(ilst);
                    cboOrder.Items.FindItemByText(hdnOrderSubmitdata.Value).Selected = true;
                }

            }
            else
            {
                cboOrder.Items.Add(new RadComboBoxItem());
                if (hdnOrderSubmitdata.Value != null && hdnOrderSubmitdata.Value != string.Empty)
                {
                    RadComboBoxItem ilst = new RadComboBoxItem();
                    ilst.Text = hdnOrderSubmitdata.Value;
                    ilst.Value = hdnOrderSubmitdata.Value.Split('|')[0].ToString();
                    cboOrder.Items.Add(ilst);
                    cboOrder.Items.FindItemByText(hdnOrderSubmitdata.Value).Selected = true;
                }
            }
            //txtReferringProvider.Text = string.Empty;
            //txtProviderNPI.Text = string.Empty;
            //txtReferringFacility.Text = string.Empty;
            //txtReferingAddress.Text = string.Empty;
            //msktxtReferingPhoneNo.Text = string.Empty;
            //msktxtReferingFaxNo.Text = string.Empty;
            //txtReferringProvider.ReadOnly = false;
            //txtProviderNPI.ReadOnly = false;
            //txtReferringFacility.ReadOnly = false;
            //txtReferingAddress.ReadOnly = false;
            //msktxtReferingPhoneNo.ReadOnly = false;
            //msktxtReferingFaxNo.ReadOnly = false;
            //txtProviderNPI.Attributes.Remove("onkeypress");
            //txtProviderNPI.Attributes.Remove("onkeydown");
            //txtProviderNPI.Style.Add("background-color", "#ffffff");

            //txtReferingAddress.Attributes.Remove("onkeypress");
            //txtReferingAddress.Attributes.Remove("onkeydown");
            //txtReferingAddress.Style.Add("background-color", "#ffffff");

            //txtReferringProvider.Attributes.Remove("onkeydown");
            //txtReferringProvider.Attributes.Remove("onkeypress");
            //txtReferringProvider.Style.Add("background-color", "#ffffff");

            //txtReferringFacility.Attributes.Remove("onkeydown");
            //txtReferringFacility.Attributes.Remove("onkeypress");
            //txtReferringFacility.Style.Add("background-color", "#ffffff");

            //msktxtReferingPhoneNo.Style.Add("background-color", "#ffffff");

            //msktxtReferingFaxNo.Style.Add("background-color", "#ffffff");
        }
        protected void chkWillingnessInCancellation_CheckedChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }

        protected void rdoReferringProvider_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoReferringProvider.Checked == true)
            {
                chkSelfReferred.Checked = false;
                rdoPcp.Checked = false;
                //lblReferringName.Text = "Ref. Provider";
                //lblReferingFacility.Text = "Ref. Facility";
                //lblReferingAddress.Text = "Ref. Address";
                //lblReferingPhoneNo.Text = "Ref. Phone";
                //lblReferingFaxNo.Text = "Ref. Fax";
                //txtReferringFacility.Text = string.Empty;
                //txtReferringProvider.Text = string.Empty;
                //txtReferingAddress.Text = string.Empty;
                //msktxtReferingPhoneNo.Text = string.Empty;
                //msktxtReferingFaxNo.Text = string.Empty;
                //txtProviderNPI.Text = string.Empty;
                //txtReferringProvider.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtReferringFacility.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtReferingAddress.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtProviderNPI.Attributes.Add("onkeypress", "return isNumberKey(this);");
                //txtReferringProvider.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtReferringFacility.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtProviderNPI.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtReferingAddress.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //msktxtReferingPhoneNo.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //msktxtReferingFaxNo.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //  msktxtReferingPhoneNo.ReadOnly = false;
                //   msktxtReferingFaxNo.ReadOnly = false;
                //btnFindPhysician.Enabled = true;
                if (hdnEncounterID.Value != null && hdnEncounterID.Value != string.Empty)
                {
                    Encntlist = EncMngr.GetEncounterByEncounterID(Convert.ToUInt32(hdnEncounterID.Value));
                    if (Encntlist.Count > 0)
                    {
                        if (Encntlist[0].Referring_Physician != string.Empty)
                        {
                            //txtReferringFacility.Text = Encntlist[0].Referring_Facility;
                            //txtReferringProvider.Text = Encntlist[0].Referring_Physician;
                            //txtReferingAddress.Text = Encntlist[0].Referring_Address;
                            //msktxtReferingPhoneNo.Text = Encntlist[0].Referring_Phone_No;
                            //msktxtReferingFaxNo.Text = Encntlist[0].Referring_Fax_No;
                            //txtProviderNPI.Text = Encntlist[0].Referring_Provider_NPI;
                            //txtReferringProvider.Attributes.Add("onkeypress", "return false;");
                            //txtReferringFacility.Attributes.Add("onkeypress", "return false;");
                            //txtReferingAddress.Attributes.Add("onkeypress", "return false;");
                            //txtProviderNPI.Attributes.Add("onkeypress", "return false;");
                            //txtProviderNPI.Attributes.Add("onkeydown", "return false;");
                            //txtReferringProvider.Attributes.Add("onkeydown", "return false;");
                            //txtReferringFacility.Attributes.Add("onkeydown", "return false;");
                            //txtReferingAddress.Attributes.Add("onkeydown", "return false;");
                            //msktxtReferingFaxNo.ReadOnly = true;
                            //msktxtReferingPhoneNo.ReadOnly = true;
                        }
                    }
                }
            }
            else
            {
                //txtReferringProvider.Text = string.Empty;
                //txtReferringFacility.Text = string.Empty;
                //txtReferingAddress.Text = string.Empty;
                //msktxtReferingFaxNo.Text = string.Empty;
                //msktxtReferingPhoneNo.Text = string.Empty;
                //txtProviderNPI.Text = string.Empty;
                //txtReferringProvider.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtReferringFacility.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtReferingAddress.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtProviderNPI.Attributes.Add("onkeypress", "return isNumberKey(this);");
                //btnFindPhysician.Enabled = false;
                //    msktxtReferingPhoneNo.ReadOnly = false;
                //    msktxtReferingFaxNo.ReadOnly = false;
            }
        }
        // Commented by Valli
        protected void rdoPcp_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoPcp.Checked == true)
            {
                chkSelfReferred.Checked = false;
                rdoReferringProvider.Checked = false;
                //lblReferringName.Text = "PCP. Provider";
                //lblReferingFacility.Text = "PCP. Facility";
                //lblReferingAddress.Text = "PCP. Address";
                //lblReferingPhoneNo.Text = "PCP. Phone";
                //lblReferingFaxNo.Text = "PCP. Fax";
                //txtReferringFacility.Text = string.Empty;
                //txtReferringProvider.Text = string.Empty;
                //txtReferingAddress.Text = string.Empty;
                //msktxtReferingPhoneNo.Text = string.Empty;
                //msktxtReferingFaxNo.Text = string.Empty;
                //txtProviderNPI.Text = string.Empty;
                //txtReferringProvider.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtReferringFacility.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtProviderNPI.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtReferingAddress.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //msktxtReferingPhoneNo.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //msktxtReferingFaxNo.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtReferringProvider.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtReferringFacility.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtReferingAddress.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtProviderNPI.Attributes.Add("onkeypress", "return isNumberKey(this);");
                //  msktxtReferingFaxNo.ReadOnly = false;
                //   msktxtReferingPhoneNo.ReadOnly = false;
                //btnFindPhysician.Enabled = true;
                if (hdnEncounterID.Value != null && hdnEncounterID.Value != string.Empty)
                {
                    Encntlist = EncMngr.GetEncounterByEncounterID(Convert.ToUInt32(hdnEncounterID.Value));
                    if (Encntlist != null && Encntlist.Count > 0)
                    {
                        if (Encntlist[0].PCP_Physician != string.Empty)
                        {
                            //txtReferringFacility.Text = Encntlist[0].PCP_Facility;
                            //txtReferringProvider.Text = Encntlist[0].PCP_Physician;
                            //txtReferingAddress.Text = Encntlist[0].PCP_Address;
                            //msktxtReferingPhoneNo.Text = Encntlist[0].PCP_Phone_No;
                            //msktxtReferingFaxNo.Text = Encntlist[0].PCP_Fax_No;
                            //txtProviderNPI.Text = Encntlist[0].PCP_Provider_NPI;
                            //txtReferringProvider.Attributes.Add("onkeypress", "return false;");
                            //txtReferringFacility.Attributes.Add("onkeypress", "return false;");
                            //txtReferingAddress.Attributes.Add("onkeypress", "return false;");
                            //txtProviderNPI.Attributes.Add("onkeypress", "return false;");
                            //txtProviderNPI.Attributes.Add("onkeydown", "return false;");
                            //txtReferringProvider.Attributes.Add("onkeydown", "return false;");
                            //txtReferringFacility.Attributes.Add("onkeydown", "return false;");
                            //txtReferingAddress.Attributes.Add("onkeydown", "return false;");
                            //msktxtReferingFaxNo.ReadOnly = true;
                            //msktxtReferingPhoneNo.ReadOnly = true;
                        }
                    }
                }
            }
            else
            {
                //txtReferringProvider.Text = string.Empty;
                //txtReferringFacility.Text = string.Empty;
                //txtReferingAddress.Text = string.Empty;
                //msktxtReferingFaxNo.Text = string.Empty;
                //msktxtReferingPhoneNo.Text = string.Empty;
                //txtProviderNPI.Text = string.Empty;
                //txtReferringProvider.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtReferringFacility.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtReferingAddress.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtProviderNPI.Attributes.Add("onkeypress", "return isNumberKey(this);");
                //btnFindPhysician.Enabled = false;
                //  msktxtReferingPhoneNo.ReadOnly = false;
                //  msktxtReferingFaxNo.ReadOnly = false;
            }
        }

        protected void btnReferralandPCP_Click(object sender, EventArgs e)
        {
            //    txtReferringProvider.Attributes.Add("onkeypress", "return false;");
            //    txtReferringFacility.Attributes.Add("onkeypress", "return false;");
            //    txtReferingAddress.Attributes.Add("onkeypress", "return false;");
            //    txtProviderNPI.Attributes.Add("onkeypress", "return false;");
            //    txtProviderNPI.Attributes.Add("onkeydown", "return false;");
            //    txtReferringProvider.Attributes.Add("onkeydown", "return false;");
            //    txtReferringFacility.Attributes.Add("onkeydown", "return false;");
            //    txtReferingAddress.Attributes.Add("onkeydown", "return false;");
            //    msktxtReferingPhoneNo.ReadOnly = true;
            //    msktxtReferingFaxNo.ReadOnly = true;
        }


        protected void tabReferringProvAndPCP_TabClick(object sender, RadTabStripEventArgs e)
        {
            if (hdnbtnsave.Value.ToString().ToUpper() == "TRUE")
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;

            if (tabReferringProvAndPCP.SelectedIndex == 0)
            {
                //Cap - 1234
                hdnTabRefPcpChange.Value = "Referring";

                var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                //Cap - 2594
                //if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y" && cboOrder.SelectedItem.Text != "Akido Order")
                {
                    imgClearProviderText.Visible = false;
                    imgEditProvider.Visible = false;
                } //Cap - 2594
                else if(ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y" && cboOrder.SelectedItem.Text == "Akido Order")
                {
                    imgClearProviderText.Visible = true;
                    imgEditProvider.Visible = true;
                }
                else
                {
                    imgClearProviderText.Visible = true;
                    imgEditProvider.Visible = true;
                }

                imgClearProviderText.Style.Add("top", "234px !important");
                imgEditProvider.Style.Add("top", "234px !important");

                chkSelfReferred.Visible = true;
                if (hdnEncounterID.Value != null && hdnEncounterID.Value != string.Empty)
                {
                    chkSelfReferred.Checked = false;
                }

                if (hdbselref.Value == "Y")
                {
                    chkSelfReferred.Checked = true;
                    imgClearProviderText.Attributes.Remove("onclick");
                }
                else
                {
                    chkSelfReferred.Checked = false;
                    imgClearProviderText.Attributes.Add("onclick", "return ProviderSearchclear()");
                }

                if (hdnrenprovider.Value != string.Empty)
                {
                    txtProviderSearch.Enabled = false;
                }
                //lblReferringName.Text = "Ref. Provider";
                //lblReferingFacility.Text = "Ref. Facility";
                //lblReferingAddress.Text = "Ref. Address";
                //lblReferingPhoneNo.Text = "Ref. Phone";
                //lblReferingFaxNo.Text = "Ref. Fax";
                //txtReferringFacility.Text = string.Empty;
                //txtReferringProvider.Text = string.Empty;
                //txtReferingAddress.Text = string.Empty;
                //msktxtReferingPhoneNo.Text = string.Empty;
                //msktxtReferingFaxNo.Text = string.Empty;
                //txtProviderNPI.Text = string.Empty;
                //txtReferringProvider.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtReferringFacility.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtReferingAddress.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtProviderNPI.Attributes.Add("onkeypress", "return isNumberKey(this);");
                //txtReferringProvider.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtReferringFacility.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtProviderNPI.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtReferingAddress.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //msktxtReferingPhoneNo.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //msktxtReferingFaxNo.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //  msktxtReferingPhoneNo.ReadOnly = false;
                //   msktxtReferingFaxNo.ReadOnly = false;
                //btnFindPhysician.Enabled = true;
                if (hdnEncounterID.Value != string.Empty)
                {
                    if (hdnEncounterID.Value != "0")
                    {
                        Encntlist = EncMngr.GetEncounterByEncounterID(Convert.ToUInt32(hdnEncounterID.Value));

                        if (Encntlist.Count > 0)
                        {
                            if (Encntlist[0].Referring_Physician != string.Empty)
                            {
                                //            txtReferringFacility.Text = Encntlist[0].Referring_Facility;
                                //            txtReferringProvider.Text = Encntlist[0].Referring_Physician;
                                //            txtReferingAddress.Text = Encntlist[0].Referring_Address;
                                //            msktxtReferingPhoneNo.Text = Encntlist[0].Referring_Phone_No;
                                //            msktxtReferingFaxNo.Text = Encntlist[0].Referring_Fax_No;
                                //            txtProviderNPI.Text = Encntlist[0].Referring_Provider_NPI;
                                //            txtReferringProvider.Attributes.Add("onkeypress", "return false;");
                                //            txtReferringFacility.Attributes.Add("onkeypress", "return false;");
                                //            txtReferingAddress.Attributes.Add("onkeypress", "return false;");
                                //            txtProviderNPI.Attributes.Add("onkeypress", "return false;");
                                //            txtProviderNPI.Attributes.Add("onkeydown", "return false;");
                                //            txtReferringProvider.Attributes.Add("onkeydown", "return false;");
                                //            txtReferringFacility.Attributes.Add("onkeydown", "return false;");
                                //            txtReferingAddress.Attributes.Add("onkeydown", "return false;");
                                //            msktxtReferingPhoneNo.ReadOnly = true;
                                //            msktxtReferingFaxNo.ReadOnly = true;
                                //        }
                                //    }


                                txtProviderSearch.Text = Encntlist[0].Referring_Physician + "| NPI: " + Encntlist[0].Referring_Provider_NPI + "| Facility: " + Encntlist[0].Referring_Facility + "| Address:" + Encntlist[0].Referring_Address + "| Phone No:" + Encntlist[0].Referring_Phone_No + "| Fax No:" + Encntlist[0].Referring_Fax_No;
                                //hdnrenprovider.Value = " |" + Encntlist[0].Referring_Physician + "|" + EncRecord.Referring_Provider_NPI + "|" + "" + "|" + EncRecord.Referring_Facility + "|" + EncRecord.Referring_Address + "|" + EncRecord.Referring_Fax_No + "|" + EncRecord.Referring_Phone_No;
                                hdnRefEditPhyId.Value = Encntlist[0].Referring_Physician;
                            }
                            else
                            {
                                txtProviderSearch.Text = "";
                                hdnRefEditPhyId.Value = string.Empty;
                            }
                        }

                    }
                    else
                    {
                        txtProviderSearch.Text = "";
                        hdnRefEditPhyId.Value = string.Empty;
                    }
                    if (hdnrenprovidersearch.Value != null && hdnrenprovidersearch.Value != "" && hdnrenprovidersearch.Value != string.Empty && hdnrenprovidersearch.Value != "| NPI: | Facility: | Address:| Phone No:| Fax No:")//Added by Vasanth 04-01-2016
                    {
                        //txtReferringProvider.Text = HdnRefPhy.Value.Split('|')[0].ToString();
                        //txtReferingAddress.Text = HdnRefPhy.Value.Split('|')[1].ToString();
                        //msktxtReferingPhoneNo.Text = HdnRefPhy.Value.Split('|')[2].ToString();
                        //msktxtReferingFaxNo.Text = HdnRefPhy.Value.Split('|')[3].ToString();
                        //txtProviderNPI.Text = HdnRefPhy.Value.Split('|')[4].ToString();
                        //txtReferringFacility.Text = HdnRefPhy.Value.Split('|')[5].ToString();
                        txtProviderSearch.Text = hdnrenprovidersearch.Value;
                    }
                    if (ddlPhysicianName.Items.Count > 0)
                    {
                        //if (txtReferringProvider.Text.Trim() != string.Empty && txtReferringProvider.Text.Contains(ddlPhysicianName.SelectedItem.Text))
                        //{
                        //    chkSelfReferred.Checked = true;
                        //    btnFindPhysician.Enabled = false;
                        //}
                    }
                    else
                        chkSelfReferred.Checked = false;
                }
                //Jira CAP-2338
                //if (Encntlist.Count > 0)
                if (Encntlist.Count > 0 && hdnrenprovider.Value == "")
                    hdnrenprovider.Value = Encntlist[0].Referring_Physician + "| NPI: " + Encntlist[0].Referring_Provider_NPI + "|  Facility: " + Encntlist[0].Referring_Facility + "| Address:" + Encntlist[0].Referring_Address + "| Phone No:" + Encntlist[0].Referring_Phone_No + "| Fax No:" + Encntlist[0].Referring_Fax_No;

            }
            else if (tabReferringProvAndPCP.SelectedIndex == 1)
            {
                //Cap - 1234
                hdnTabRefPcpChange.Value = "PCP";

                imgClearProviderText.Visible = true;
                imgEditProvider.Visible = true;
                imgClearProviderText.Style.Add("top", "224px !important");
                imgEditProvider.Style.Add("top", "224px !important");
                chkSelfReferred.Visible = false;
                //Jira #CAP-69 - labels are missing
                pcpDefaultDemographics();

                //lblReferringName.Text = "PCP. Provider";
                //lblReferingFacility.Text = "PCP. Facility";
                //lblReferingAddress.Text = "PCP. Address";
                //lblReferingPhoneNo.Text = "PCP. Phone";
                //lblReferingFaxNo.Text = "PCP. Fax";
                //txtReferringFacility.Text = string.Empty;
                //txtReferringProvider.Text = string.Empty;
                //txtReferingAddress.Text = string.Empty;
                //msktxtReferingPhoneNo.Text = string.Empty;
                //msktxtReferingFaxNo.Text = string.Empty;
                //txtProviderNPI.Text = string.Empty;
                //txtReferringProvider.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtReferringFacility.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtProviderNPI.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtReferingAddress.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //msktxtReferingPhoneNo.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //msktxtReferingFaxNo.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //txtReferringProvider.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtReferringFacility.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtReferingAddress.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                //txtProviderNPI.Attributes.Add("onkeypress", "return isNumberKey(this);");
                // msktxtReferingPhoneNo.ReadOnly = false;
                //msktxtReferingFaxNo.ReadOnly = false;
                //btnFindPhysician.Enabled = true;

                //if (hdnEncounter_Physician_id.Value != "" && hdnEncounter_Physician_id.Value!=null)//vasanth for default value for pcp and refer
                //{
                //    if (hdnEncounterID.Value == "0")
                //    {

                //        PhysicianManager phyMngr = new PhysicianManager();
                //        IList<PhysicianLibrary> phylist = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(hdnEncounter_Physician_id.Value));
                //        if (phylist != null && phylist.Count > 0)
                //        {
                //            PhysicianLibrary objPhyLib = phylist[0];

                //            if (objPhyLib != null)
                //            {
                //                string sPhyName = objPhyLib.PhyPrefix + " " + objPhyLib.PhyFirstName + " " + objPhyLib.PhyMiddleName + " " + objPhyLib.PhyLastName + " " + objPhyLib.PhySuffix;
                //                //  txtReferringFacility.Text = objPhyLib.PCP_Facility;
                //                txtReferringProvider.Text = sPhyName;
                //                txtReferingAddress.Text = objPhyLib.PhyAddress1;
                //                msktxtReferingPhoneNo.Text = objPhyLib.PhyTelephone;
                //                msktxtReferingFaxNo.Text = objPhyLib.PhyFax;
                //                txtProviderNPI.Text = objPhyLib.PhyNPI;

                //            }
                //        }
                //}

                //}
            }
            //Jira #CAP-158 -  Not able to navigate tab 
            if (hdnCurrentProcess.Value != null && hdnCurrentProcess.Value != "" && hdnCurrentProcess.Value.ToUpper() != "SCHEDULED")
            {
                this.Page.Title = "View Appointment" + "-" + ClientSession.UserName;
                chkReschedule.Enabled = false;
                //Jira CAP-2216
                //chkSelfReferred.Enabled = false;
                chkShowAllPhysicians.Enabled = false;
                //Jira CAP-2338
                //btnSave.Enabled = false;
                txtPurposeofVisit.txtDLC.Enabled = false;
                txtNotes.txtDLC.Enabled = false;
                DisableTableLayout(pnlReschedule);
                chkReschedule.Enabled = false;
                chkShowAllPhysicians.Enabled = false;
                //Jira CAP-2216
                //chkSelfReferred.Enabled = false;
                //Jira CAP-2338
                //btnSave.Enabled = false;
                DateTimePickerColorChange(this.dtpApptDate, true);
                TimePickerColorChange(dtpStartTime, true);
                cboFacility.Enabled = false;
                pnlAppointmentDetails.Enabled = false;
                //Jira CAP-2216 - Start
                //pnlReferringDetails.Enabled = false;
                cboOrder.Enabled = false;
                //Jira CAP-2216 - End
                pnlVisit.Enabled = false;
                txtProviderSearch.Enabled = false;
                imgClearProviderText.Visible = false;
                imgEditProvider.Visible = false;
                btnPatientDemographics.Enabled = false;
                btnPatientTask.Enabled = false;

            }
            //Jira CAP-2216 - Start
            if (Request["EncounterID"] != null && Request["EncounterID"] != string.Empty && hdnCurrentProcess.Value != "ARCHIEVE")
            {
                EnablePhytxtBox(Convert.ToUInt64(Request["EncounterID"]));
            }
            else if (hdnCurrentProcess.Value == "ARCHIEVE")
            {
                chkSelfReferred.Enabled = false;
                pnlReferringDetails.Enabled = false;
            }
            if (chkSelfReferred != null && chkSelfReferred.Visible == true && chkSelfReferred.Checked == true)
            {
                imgClearProviderText.Visible = false;
                imgEditProvider.Visible = false;
            }
            //Jira CAP-2216 - End
        }
        //Jira #CAP-69 - labels are missing
        public void pcpDefaultDemographics()
        {
            if (!IsPostBack)
            {
                if (hdnEncounterID.Value != null && hdnEncounterID.Value != string.Empty)
                {
                    if (hdnEncounterID.Value != "0")
                    {
                        Encntlist = EncMngr.GetEncounterByEncounterID(Convert.ToUInt32(hdnEncounterID.Value));
                        if (Encntlist.Count > 0)
                        {
                            if (Encntlist[0].PCP_Physician != string.Empty)
                            {
                                //txtReferringFacility.Text = Encntlist[0].PCP_Facility;
                                //txtReferringProvider.Text = Encntlist[0].PCP_Physician;
                                //txtReferingAddress.Text = Encntlist[0].PCP_Address;
                                //msktxtReferingPhoneNo.Text = Encntlist[0].PCP_Phone_No;
                                //msktxtReferingFaxNo.Text = Encntlist[0].PCP_Fax_No;
                                //txtProviderNPI.Text = Encntlist[0].PCP_Provider_NPI;
                                //txtReferringProvider.Attributes.Add("onkeypress", "return false;");
                                //txtReferringFacility.Attributes.Add("onkeypress", "return false;");
                                //txtReferingAddress.Attributes.Add("onkeypress", "return false;");
                                //txtProviderNPI.Attributes.Add("onkeypress", "return false;");
                                //txtProviderNPI.Attributes.Add("onkeydown", "return false;");
                                //txtReferringProvider.Attributes.Add("onkeydown", "return false;");
                                //txtReferringFacility.Attributes.Add("onkeydown", "return false;");
                                //txtReferingAddress.Attributes.Add("onkeydown", "return false;");
                                //msktxtReferingPhoneNo.ReadOnly = true;
                                //msktxtReferingFaxNo.ReadOnly = true;

                                //txtProviderSearch.Text = " |" + Encntlist[0].Referring_Physician + "| NPI: " + Encntlist[0].Referring_Provider_NPI + "| Facility: " + Encntlist[0].Referring_Facility + "| Address:" + Encntlist[0].Referring_Address + "| Fax No:" + Encntlist[0].Referring_Fax_No + "| Phone No:" + Encntlist[0].Referring_Phone_No;
                                //  hdnpcpprovider.Value = " |" + Encntlist[0].Referring_Physician + "|" + Encntlist[0].Referring_Provider_NPI + "|" + "" + "|" + Encntlist[0].Referring_Facility + "|" + Encntlist[0].Referring_Address + "|" + Encntlist[0].Referring_Fax_No + "|" + Encntlist[0].Referring_Phone_No; ; ;
                                txtProviderSearch.Text = Encntlist[0].PCP_Physician + "| NPI: " + Encntlist[0].PCP_Provider_NPI +
                                 "| Facility: " + Encntlist[0].PCP_Facility + "| Address:" + Encntlist[0].PCP_Address +
                                 "| Phone No:" + Encntlist[0].PCP_Phone_No + "| Fax No:" + Encntlist[0].PCP_Fax_No;
                                //Jira #CAP-69 - labels are missing
                                //hdnpcpprovider.Value = " |" + Encntlist[0].PCP_Physician + "|" + Encntlist[0].PCP_Provider_NPI + "|" + "" + "|" + Encntlist[0].PCP_Facility +
                                //    "|" + Encntlist[0].PCP_Address + "|"
                                //    + Encntlist[0].PCP_Fax_No + "|" + Encntlist[0].PCP_Phone_No;

                                //hdnpcpprovidersearch.Value = " |" + Encntlist[0].PCP_Physician + "|" + Encntlist[0].PCP_Provider_NPI + "|" + "" + "|" + Encntlist[0].PCP_Facility +
                                //    "|" + Encntlist[0].PCP_Address + "|"
                                //    + Encntlist[0].PCP_Fax_No + "|" + Encntlist[0].PCP_Phone_No;

                                hdnpcpprovider.Value = Encntlist[0].PCP_Physician + "| NPI: " + Encntlist[0].PCP_Provider_NPI +
                                 "| Facility: " + Encntlist[0].PCP_Facility + "| Address:" + Encntlist[0].PCP_Address +
                                 "| Phone No:" + Encntlist[0].PCP_Phone_No + "| Fax No:" + Encntlist[0].PCP_Fax_No;

                                hdnpcpprovidersearch.Value = Encntlist[0].PCP_Physician + "| NPI: " + Encntlist[0].PCP_Provider_NPI +
                                 "| Facility: " + Encntlist[0].PCP_Facility + "| Address:" + Encntlist[0].PCP_Address +
                                 "| Phone No:" + Encntlist[0].PCP_Phone_No + "| Fax No:" + Encntlist[0].PCP_Fax_No;
                                hdnpcpEditPhyId.Value = Encntlist[0].Referring_Physician;
                            }
                            else
                            {
                                txtProviderSearch.Text = "";
                                hdnpcpEditPhyId.Value = string.Empty;
                            }
                        }
                    }
                    else
                    {

                        FindPhysican InsuredList = new FindPhysican();
                        PatientInsuredPlanManager objPhysicianManager = new PatientInsuredPlanManager();
                        InsuredList = objPhysicianManager.FindPhysicianByInsureList(Convert.ToUInt64(Request["Human_id"]));

                        if (InsuredList.PhyList.Count > 0)
                        {
                            //Jira #CAP-156 - Index was outside bounds 
                            //txtProviderSearch.Text = InsuredList.PhyList[0].PhyPrefix + " " + InsuredList.PhyList[0].PhyFirstName + " " + InsuredList.PhyList[0].PhyMiddleName + " " + InsuredList.PhyList[0].PhyLastName + "(" + InsuredList.PhyList[0].PhySuffix + ")" + " | " +
                            //                                 "NPI:" + InsuredList.PhyList[0].PhyNPI + " | " +
                            //                                 "Facility:" + InsuredList.PhyList[0].PhyFacility + " | " +
                            //                                 "Address:" + InsuredList.PhyList[0].PhyAddrs + ", " +
                            //                                 InsuredList.PhyList[0].PhyCity + "," +
                            //                                 InsuredList.PhyList[0].PhyState + " " +
                            //                                 InsuredList.PhyList[0].PhyZip + " | " +
                            //                                 ((InsuredList.PhyList[0].PhyPhone.Trim()) != "" ? "Phone No:" + InsuredList.PhyList[0].PhyPhone + " | " : "") +
                            //                                 (InsuredList.PhyList[0].PhyFax.Trim() != "" ? "Fax No:" + InsuredList.PhyList[0].PhyFax : "");

                            txtProviderSearch.Text = InsuredList.PhyList[0].PhyPrefix + " " + InsuredList.PhyList[0].PhyFirstName + " " + InsuredList.PhyList[0].PhyMiddleName + " " + InsuredList.PhyList[0].PhyLastName + "(" + InsuredList.PhyList[0].PhySuffix + ")" + " | " +
                                                         "NPI:" + InsuredList.PhyList[0].PhyNPI + " | " +
                                                         "Facility:" + InsuredList.PhyList[0].PhyFacility + " | " +
                                                         "Address:" + InsuredList.PhyList[0].PhyAddrs + ", " +
                                                         InsuredList.PhyList[0].PhyCity + "," +
                                                         InsuredList.PhyList[0].PhyState + " " +
                                                         InsuredList.PhyList[0].PhyZip + " | " +
                                                         ((InsuredList.PhyList[0].PhyPhone.Trim()) != "" ? "Phone No:" + InsuredList.PhyList[0].PhyPhone + " | " : "Phone No: | ") +
                                                         (InsuredList.PhyList[0].PhyFax.Trim() != "" ? "Fax No:" + InsuredList.PhyList[0].PhyFax : "Fax No:");
                            hdnpcpEditPhyId.Value = InsuredList.PhyList[0].PhyId.ToString();
                            //Jira #CAP-156 - Index was outside bounds 
                            //hdnpcpprovider.Value = InsuredList.PhyList[0].PhyPrefix + " " + InsuredList.PhyList[0].PhyFirstName + " " + InsuredList.PhyList[0].PhyMiddleName + " " + InsuredList.PhyList[0].PhyLastName + "(" + InsuredList.PhyList[0].PhySuffix + ")" + " | " +
                            //                          "NPI:" + InsuredList.PhyList[0].PhyNPI + " | " +
                            //                          "Facility:" + InsuredList.PhyList[0].PhyFacility + " | " +
                            //                          "Address:" + InsuredList.PhyList[0].PhyAddrs + ", " +
                            //                          InsuredList.PhyList[0].PhyCity + "," +
                            //                          InsuredList.PhyList[0].PhyState + " " +
                            //                          InsuredList.PhyList[0].PhyZip + " | " +
                            //                          ((InsuredList.PhyList[0].PhyPhone.Trim()) != "" ? "Phone No:" + InsuredList.PhyList[0].PhyPhone + " | " : "") +
                            //                          (InsuredList.PhyList[0].PhyFax.Trim() != "" ? "Fax No:" + InsuredList.PhyList[0].PhyFax : "");

                            hdnpcpprovider.Value = InsuredList.PhyList[0].PhyPrefix + " " + InsuredList.PhyList[0].PhyFirstName + " " + InsuredList.PhyList[0].PhyMiddleName + " " + InsuredList.PhyList[0].PhyLastName + "(" + InsuredList.PhyList[0].PhySuffix + ")" + " | " +
                                                      "NPI:" + InsuredList.PhyList[0].PhyNPI + " | " +
                                                      "Facility:" + InsuredList.PhyList[0].PhyFacility + " | " +
                                                      "Address:" + InsuredList.PhyList[0].PhyAddrs + ", " +
                                                      InsuredList.PhyList[0].PhyCity + "," +
                                                      InsuredList.PhyList[0].PhyState + " " +
                                                      InsuredList.PhyList[0].PhyZip + " | " +
                                                      ((InsuredList.PhyList[0].PhyPhone.Trim()) != "" ? "Phone No:" + InsuredList.PhyList[0].PhyPhone + " | " : "Phone No: | ") +
                                                      (InsuredList.PhyList[0].PhyFax.Trim() != "" ? "Fax No:" + InsuredList.PhyList[0].PhyFax : "Fax No:");

                            //Jira #CAP-156 - Index was outside bounds 
                            //hdnpcpprovidersearch.Value = InsuredList.PhyList[0].PhyPrefix + " " + InsuredList.PhyList[0].PhyFirstName + " " + InsuredList.PhyList[0].PhyMiddleName + " " + InsuredList.PhyList[0].PhyLastName + "(" + InsuredList.PhyList[0].PhySuffix + ")" + " | " +
                            //                          "NPI:" + InsuredList.PhyList[0].PhyNPI + " | " +
                            //                          "Facility:" + InsuredList.PhyList[0].PhyFacility + " | " +
                            //                          "Address:" + InsuredList.PhyList[0].PhyAddrs + ", " +
                            //                          InsuredList.PhyList[0].PhyCity + "," +
                            //                          InsuredList.PhyList[0].PhyState + " " +
                            //                          InsuredList.PhyList[0].PhyZip + " | " +
                            //                          ((InsuredList.PhyList[0].PhyPhone.Trim()) != "" ? "Phone No:" + InsuredList.PhyList[0].PhyPhone + " | " : "") +
                            //                          (InsuredList.PhyList[0].PhyFax.Trim() != "" ? "Fax No:" + InsuredList.PhyList[0].PhyFax : "");

                            hdnpcpprovidersearch.Value = InsuredList.PhyList[0].PhyPrefix + " " + InsuredList.PhyList[0].PhyFirstName + " " + InsuredList.PhyList[0].PhyMiddleName + " " + InsuredList.PhyList[0].PhyLastName + "(" + InsuredList.PhyList[0].PhySuffix + ")" + " | " +
                                                     "NPI:" + InsuredList.PhyList[0].PhyNPI + " | " +
                                                     "Facility:" + InsuredList.PhyList[0].PhyFacility + " | " +
                                                     "Address:" + InsuredList.PhyList[0].PhyAddrs + ", " +
                                                     InsuredList.PhyList[0].PhyCity + "," +
                                                     InsuredList.PhyList[0].PhyState + " " +
                                                     InsuredList.PhyList[0].PhyZip + " | " +
                                                     ((InsuredList.PhyList[0].PhyPhone.Trim()) != "" ? "Phone No:" + InsuredList.PhyList[0].PhyPhone + " | " : "Phone No: | ") +
                                                     (InsuredList.PhyList[0].PhyFax.Trim() != "" ? "Fax No:" + InsuredList.PhyList[0].PhyFax : "Fax No:");
                        }
                        else
                        {
                            txtProviderSearch.Text = "";
                            hdnpcpEditPhyId.Value = string.Empty;
                        }

                    }

                    if (hdnpcpprovidersearch.Value != null && hdnpcpprovidersearch.Value.Trim() != "")//Added by Vasanth
                    {
                        //txtReferringProvider.Text = HdnPcpPhy.Value.Split('|')[0].ToString();
                        //txtReferingAddress.Text = HdnPcpPhy.Value.Split('|')[1].ToString();
                        //msktxtReferingPhoneNo.Text = HdnPcpPhy.Value.Split('|')[2].ToString();
                        //msktxtReferingFaxNo.Text = HdnPcpPhy.Value.Split('|')[3].ToString();
                        //txtProviderNPI.Text = HdnPcpPhy.Value.Split('|')[4].ToString();
                        //txtReferringFacility.Text = HdnPcpPhy.Value.Split('|')[5].ToString();

                        txtProviderSearch.Text = hdnpcpprovidersearch.Value;
                    }

                }

                if (hdnCurrentProcess.Value != null && hdnCurrentProcess.Value.ToUpper() == "SCHEDULED")
                {
                    if (txtProviderSearch.Text != "")
                    {
                        txtProviderSearch.Enabled = false;
                    }
                    else
                    {
                        txtProviderSearch.Enabled = true;
                    }
                }
            }
            //Cap - 1234
            else
            {
                txtProviderSearch.Text = hdnpcpprovidersearch.Value;
            }

        }

        #region Empty Events. Some cause error when removed. Clean up when Time permits

        protected void pbDropDown_Click(object sepnder, ImageClickEventArgs e)
        {

            if (hdnpbNotesClick.Value == "Minus")
            {
                //pnlNotes.Visible = false;
                //pbNotesDropDown.ImageUrl = "~/Resources/Plus_new.gif";
                //hdnpbNotesClick.Value = "Plus";
            }

            //not been used right now 12-01-2016 by vasanth
            //if (hdnTestClick.Value == "Minus")
            //{
            //    pnlTest.Visible = false;
            //    pbTestDropDown.ImageUrl = "~/Resources/pbAdd.png";
            //    hdnTestClick.Value = "Plus";
            //}

            if (hdnPbClick.Value == "Plus")
            {
                //pnlPurpoesListBox.Visible = true;
                ////pbPurposeVisitDown.ImageUrl = "~/Resources/minus_new.gif";
                //hdnPbClick.Value = "Minus";
                //IList<UserLookup> fieldList = new List<UserLookup>();
                //if (hdnPhysicianID.Value != string.Empty)
                //{
                //    lslPurposeofVisit.Items.Clear();
                //    fieldList = userMngr.GetFieldLookupList(Convert.ToUInt64(hdnPhysicianID.Value), "PURPOSE OF VISIT");
                //    for (int i = 0; i < fieldList.Count; i++)
                //    {
                //        lslPurposeofVisit.Items.Add(fieldList[i].Value);
                //    }
                //}
            }
            else
            {
                // pnlPurpoesListBox.Visible = false;
                //// pbPurposeVisitDown.ImageUrl = "~/Resources/Plus_new.gif";
                // hdnPbClick.Value = "Plus";
            }




        }
        protected void btnSave_Click1(object sender, EventArgs e)
        {

        }
        protected void pbNotesDropDown_Click(object sender, ImageClickEventArgs e)
        {
            if (hdnPbClick.Value == "Minus")
            {
                //pnlPurpoesListBox.Visible = false;
                ////pbPurposeVisitDown.ImageUrl = "~/Resources/Plus_new.gif";
                //hdnPbClick.Value = "Plus";

            }
            //not been used right now 12-01-2016 by vasanth
            //if (hdnTestClick.Value == "Minus")
            //{
            //    pnlTest.Visible = false;
            //    pbTestDropDown.ImageUrl = "~/Resources/pbAdd.png";
            //    hdnTestClick.Value = "Plus";
            //}

            if (hdnpbNotesClick.Value == "Plus")
            {
                //pnlNotes.Visible = true;
                //pbNotesDropDown.ImageUrl = "~/Resources/Minus_new.gif";
                //hdnpbNotesClick.Value = "Minus";
                //IList<UserLookup> fieldList = new List<UserLookup>();
                //if (hdnPhysicianID.Value != string.Empty)
                //{
                //    lstNotes.Items.Clear();
                //    fieldList = userMngr.GetFieldLookupList(Convert.ToUInt64(hdnPhysicianID.Value), "APPOINTMENT NOTES");
                //    for (int i = 0; i < fieldList.Count; i++)
                //    {
                //        lstNotes.Items.Add(fieldList[i].Value);
                //    }
                //}
            }
            else
            {
                //pnlNotes.Visible = false;
                //pbNotesDropDown.ImageUrl = "~/Resources/Plus_new.gif";
                //hdnpbNotesClick.Value = "Plus";
            }

        }
        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (txtPurposeofVisit.Text.Contains(lslPurposeofVisit.SelectedItem.Text) == false)		
            //{		
            //    if (txtPurposeofVisit.Text == string.Empty)		
            //    {		
            //        txtPurposeofVisit.Text = lslPurposeofVisit.SelectedItem.Text;		
            //    }		
            //    else		
            //    {		
            //        txtPurposeofVisit.Text = txtPurposeofVisit.Text + "," + lslPurposeofVisit.SelectedItem.Text;		
            //    }		
            //}		
        }
        protected void hdnLocalTime_ValueChanged(object sender, EventArgs e)
        {
        }
        protected void lstNotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (txtNotes.Text.Contains(lstNotes.SelectedItem.Text) == false)		
            //{		
            //    if (txtNotes.Text == string.Empty)		
            //    {		
            //        txtNotes.Text = lstNotes.SelectedItem.Text;		
            //    }		
            //    else		
            //    {		
            //        txtNotes.Text = txtNotes.Text + "," + lstNotes.SelectedItem.Text;		
            //    }		
            //}		
        }
        #region not been used right now 12-01-2016 by vasanth
        //protected void lstTest_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ListItem item = lstTest.SelectedItem as ListItem;

        //    if (item == null)
        //    {
        //        return;
        //    }
        //    //if (bPOVCheck == true)
        //    //{

        //    string[] splitChar = new string[] { " , " };
        //    string[] sTemp = txtTest.Text.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
        //    if (sTemp.Length > 0)
        //    {
        //        foreach (string s in sTemp)
        //        {
        //            if (s.Trim().Equals(item.Text.Trim()))
        //            {
        //                return;
        //            }
        //        }
        //    }
        //    if (txtTest.Text == string.Empty)
        //    {
        //        txtTest.Text = item.Text;
        //    }

        //    else
        //    {
        //        txtTest.Text += " , " + item.Text;

        //    }
        //    if (!lstOrdersID.Contains(item.Value.ToString()))
        //    {
        //        lstOrdersID.Add(item.Value.ToString());
        //        hdnOrderList.Value += item.Value + "-";
        //    }

        //    //}

        //    bPOVCheck = false;
        //    btnSave.Enabled = true;
        //}


        //protected void pbTestDropDown_Click(object sender, ImageClickEventArgs e)
        //{
        //    if (hdnTestClick.Value == "Plus")
        //    {
        //        pnlTest.Visible = true;
        //        pbTestDropDown.ImageUrl = "~/Resources/pbminus.png";
        //        hdnTestClick.Value = "Minus";
        //        if (hdnHumanID.Value != string.Empty)
        //        {
        //            ulMyHumanID = Convert.ToUInt64(hdnHumanID.Value);
        //        }
        //        IList<Orders> temp = OrderMngr.GetOutstandingCPTForOrderGenerateProcess(ulMyHumanID);
        //        lstTest.Items.Clear();
        //        //foreach (string srt in temp)
        //        //{
        //        //    lstTest.Items.Add(new RadListBoxItem(srt));
        //        //}

        //        for (int i = 0; i < temp.Count; i++)
        //        {
        //            ListItem objItem = new ListItem();
        //            objItem.Text = temp[i].Lab_Procedure + "-" + temp[i].Lab_Procedure_Description;
        //            objItem.Value = temp[i].Id.ToString();
        //            lstTest.Items.Add(objItem);
        //        }

        //    }
        //    else
        //    {
        //        pnlTest.Visible = false;
        //        pbTestDropDown.ImageUrl = "~/Resources/pbAdd.png";
        //        hdnTestClick.Value = "Plus";
        //    }
        //}

        //protected void pbTestClear_Click(object sender, ImageClickEventArgs e)
        //{
        //    if (hdnEncounterID.Value != string.Empty)
        //    {
        //        ulMyEncID = 0;
        //    }
        //    if (ulMyEncID != 0)
        //    {
        //        string[] str = hdnOrderList.Value.Split('-').ToArray();
        //        for (int i = 0; i < str.Length; i++)
        //        {
        //            if (str[i] != string.Empty)
        //            {
        //                lstOrdersID.Add(str[i]);
        //            }
        //        }

        //        OrderMngr.DeleteCMGEncounterIDFromOrders(lstOrdersID, string.Empty);

        //    }
        //    lstOrdersID = new List<string>();
        //    hdnOrderList.Value = string.Empty;
        //    txtTest.Text = string.Empty;

        //}
        #endregion
        protected void pbClear_Click(object sender, ImageClickEventArgs e)
        {
            ///txtPurposeofVisit.Text = string.Empty;		
        }
        protected void pbNotesClear_Click(object sender, ImageClickEventArgs e)
        {
            //txtNotes.Text = string.Empty;		
        }
        protected void pbNotesLibrary_Click(object sender, ImageClickEventArgs e)
        {
        }

        #endregion
        #endregion

        #region Methods

        void lst_VisibleChanged(object sender, EventArgs e)
        {
        }

        void SaveAppointment(string Status)
        {
            string YesNoCancel = hdnMessageType.Value;
            hdnMessageType.Value = string.Empty;

            DateTime selectedDate = DateTime.Now;
            string[] sp = null;
            if (dtpApptDate.SelectedDate.Value != null && dtpApptDate.SelectedDate.Value == DateTime.MinValue)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110005'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            if (dtpApptDate.SelectedDate.Value != null && dtpApptDate.SelectedDate.Value != DateTime.MinValue)
            {

                selectedDate = Convert.ToDateTime(dtpApptDate.SelectedDate.Value);
                sp = selectedDate.ToString().Split(' ');
            }
            else
            {
                // RadWindowManager1.RadAlert("Please select Appontment Date ", 300, 100, "text", "");
                dtpApptDate.Focus();
                return;
            }
            if (dtpStartTime.SelectedTime == null)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110080'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            if (lblpatientName.Text == string.Empty)
            {

                //ApplicationObject.erroHandler.DisplayErrorMessage("110004", "Edit Appointment", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110004'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            if (txtPatientAccountNumber.Text == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110013", "Edit Appointment", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110013'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            if (dtpApptDate.SelectedDate.Value != null && dtpApptDate.SelectedDate.Value == DateTime.MinValue)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110005'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            if (dtpApptDate.SelectedDate.Value != null && dtpApptDate.SelectedDate.Value == DateTime.MinValue)
            //if (dtpApptDate.Value == dtpApptDate.NullDate)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110005", "Edit Appointment", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110005'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            //commented for new Date Time Picker
            //if (dtpStartTime.te.ToString() == "1900-01-01 12:00:00")
            //{
            //    ApplicationObject.erroHandler.DisplayErrorMessage("110028", "Edit Appointment", this.Page);
            //    //dtpStartTime.Value = MyTime;
            //    return;
            //}



            if (ddlPhysicianName.Text == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110003", "Edit Appointment", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110064'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

            if (ddlVisitType.Text.Trim() == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110006", "Edit Appointment", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110006'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                ddlVisitType.Focus();
                return;
            }

            if (ddlDuration.Text.Trim() == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110007", "Edit Appointment", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110007'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ddlDuration.Focus();
                return;
            }

            if (ddlVisitType.Text.ToUpper() == "CONSULT")
            {
                if (txtProviderSearch.Text.Trim() == string.Empty && (hdnrenprovider.Value != null && hdnrenprovider.Value == ""))
                {
                    //ApplicationObject.erroHandler.DisplayErrorMessage("110050", "Edit Appointment", this.Page);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110050'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    ddlVisitType.Focus();
                    return;
                }
            }




            //if (chkSelfReferred.Checked == true && txtReferringProvider.Text == string.Empty)
            //{
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110050');", true);
            //    //ddlVisitType.Focus();
            //    return;
            //}
            //if (rdoReferringProvider.Checked == true && txtReferringProvider.Text == string.Empty)
            //{
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110083');", true);
            //    //ddlVisitType.Focus();
            //    return;
            //}
            //if (rdoPcp.Checked == true && txtReferringProvider.Text == string.Empty)
            //{
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110082');", true);
            //    //ddlVisitType.Focus();
            //    return;
            //}

            // Need to Check Valli


            //if (msktxtReferingPhoneNo.Text != "" && PhNoValid(msktxtReferingPhoneNo.Text) == false)
            //{
            //    //ApplicationObject.erroHandler.DisplayErrorMessage("110051", "Edit Appointment", this.Page);
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110051'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    msktxtReferingPhoneNo.Focus();
            //    return;

            //}

            //if (msktxtReferingFaxNo.Text != "" && PhNoValid(msktxtReferingFaxNo.Text) == false)
            //{

            //    //ApplicationObject.erroHandler.DisplayErrorMessage("110052", "Edit Appointment", this.Page);
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110052'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    msktxtReferingFaxNo.Focus();
            //    return;

            //}
            //if (txtReferringFacility.Text != string.Empty && txtReferringProvider.Text == string.Empty)
            //{
            //    //ApplicationObject.erroHandler.DisplayErrorMessage("110053", "Edit Appointment", this.Page);
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110053'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    txtReferringProvider.Focus();
            //    return;

            //}
            //if (txtProviderNPI.Text != string.Empty && txtReferringProvider.Text == string.Empty)
            //{
            //    //ApplicationObject.erroHandler.DisplayErrorMessage("110053", "Edit Appointment", this.Page);
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110081'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    txtProviderNPI.Focus();
            //    return;

            //}


            //if (txtReferingAddress.Text != string.Empty && txtReferringProvider.Text == string.Empty)
            //{
            //    //ApplicationObject.erroHandler.DisplayErrorMessage("110054", "Edit Appointment", this.Page);
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110054'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    txtReferringProvider.Focus();
            //    return;

            //}
            //if (msktxtReferingPhoneNo.TextWithLiterals != "() -" && txtReferringProvider.Text == string.Empty)
            //{
            //    //   ApplicationObject.erroHandler.DisplayErrorMessage("110055", "Edit Appointment", this.Page);
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110055'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    txtReferringProvider.Focus();
            //    return;

            //}
            //if (msktxtReferingFaxNo.TextWithLiterals != "() -" && txtReferringProvider.Text == string.Empty)
            //{
            //    //ApplicationObject.erroHandler.DisplayErrorMessage("110056", "Edit Appointment", this.Page);
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110056'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    txtReferringProvider.Focus();
            //    return;

            //}


            // End valli 



            #region not been used right now 12-01-2016 by vasanth
            //if (lblTest.Text.Contains("*") && txtTest.Text.Trim() == string.Empty)
            //{
            //    //ApplicationObject.erroHandler.DisplayErrorMessage("110071", "Edit Appointment", this.Page);
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110071'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

            //    txtTest.Focus();
            //    return;
            //}
            #endregion
            //if (txtReferringFacility.Text != string.Empty)
            //{
            //    if (txtReferringProvider.Text == string.Empty)
            //    {
            //        ApplicationObject.erroHandler.DisplayErrorMessage("110046", "Edit Appointment", this.Page);
            //        txtReferringProvider.Focus();
            //        return;
            //    }
            //}

            //if (txtReferringProvider.Text != string.Empty)
            //{
            //    if (txtReferringFacility.Text == string.Empty)
            //    {
            //        ApplicationObject.erroHandler.DisplayErrorMessage("110046", "Edit Appointment", this.Page);
            //        txtReferringFacility.Focus();
            //        return;
            //    }
            //}

            //PreCheck - Duplicate Appointment

            DateTime MyCalendarDateTime;
            string tt = "";
            try
            {
                try
                {
                    tt = Convert.ToDateTime(dtpStartTime.DbSelectedDate).ToString("tt");
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Appointment Time of value='" + dtpStartTime.DbSelectedDate + "' to DateTime threw an error.", exp);
                    throw (exp);
                }
                MyCalendarDateTime = Convert.ToDateTime(dtpApptDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + tt);
            }
            catch (Exception exp)
            {
                //logger.Debug("Conversion of Appointment Date Time of value='" + dtpApptDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + tt + "' to DateTime threw an error.", exp);
                throw (exp);
            }

            ulong HumanID = 0, PhysicianID = 0, EncounterID = 0;
            int Duration = 0;
            try
            {
                //logger.Debug("Human_ID from hdnHumanID.Value='" + hdnHumanID.Value + "'");
                HumanID = Convert.ToUInt64(hdnHumanID.Value);
            }
            catch (Exception exp)
            {
                //logger.Debug("Conversion of Human_ID from hdnHumanID.Value='" + hdnHumanID.Value + "' to UInt threw an error.", exp);
                throw (exp);
            }
            try
            {
                //logger.Debug("PhysicianID from ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value='" + ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value + "'");
                PhysicianID = Convert.ToUInt64(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value);
            }
            catch (Exception exp)
            {
                //logger.Debug("Conversion of PhysicianID from ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value='" + ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value + "' to UInt threw an error.", exp);
                throw (exp);
            }
            try
            {
                //logger.Debug("Duration from ddlDuration.Text='" + ddlDuration.Text + "'");
                Duration = Convert.ToInt16(ddlDuration.Text);
            }
            catch (Exception exp)
            {
                //logger.Debug("Conversion of Duration from ddlDuration.Text='" + ddlDuration.Text + "' to UInt threw an error.", exp);
                throw (exp);
            }
            try
            {
                //logger.Debug("EncounterID from hdnEncounterID.Value='" + hdnEncounterID.Value + "'");
                EncounterID = Convert.ToUInt64(hdnEncounterID.Value);
            }
            catch (Exception exp)
            {
                //logger.Debug("Conversion of EncounterID from hdnEncounterID.Value='" + hdnEncounterID.Value + "' to UInt threw an error.", exp);
                throw (exp);
            }
            //logger.Debug("CheckDuplicateAppointment DB Call Starting");

            // Need to check valli

            //if (txtProviderNPI.Text != string.Empty)
            //{
            //    HumanManager humanManager = new HumanManager();
            //    Human objHumanPCP = new Human();
            //    objHumanPCP = humanManager.GetHumanFromHumanID(HumanID);
            //    if (objHumanPCP != null && objHumanPCP.Id != 0)
            //    {
            //        if (HdnPcpPhy.Value != null)
            //        {
            //            objHumanPCP.PCP_Name = HdnPcpPhy.Value.Split('|')[0].ToString();
            //            objHumanPCP.PCP_NPI = HdnPcpPhy.Value.Split('|')[4].ToString();
            //        }
            //        if (HdnPCPID.Value != null && HdnPCPID.Value != string.Empty)
            //        objHumanPCP.Encounter_Provider_ID = Convert.ToInt32(HdnPCPID.Value);
            //    }
            //    humanManager.UpdateToHuman(objHumanPCP, string.Empty);
            //}

            // End valli

            Stopwatch CheckDuplicateAppointmentDBCall = new Stopwatch();
            CheckDuplicateAppointmentDBCall.Start();
            AppointmentPreChecks appointmentPreCheckList = EncMngr.CheckDuplicateAppointment(HumanID, PhysicianID, MyCalendarDateTime, cboFacility.Text, selectedDate, dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + ":" + dtpStartTime.SelectedTime.Value.Seconds, Duration, EncounterID);
            CheckDuplicateAppointmentDBCall.Stop();
            //logger.Debug("CheckDuplicateAppointment DB Call Completed. Time Taken : " + CheckDuplicateAppointmentDBCall.Elapsed.Seconds + "." + CheckDuplicateAppointmentDBCall.Elapsed.Milliseconds + "s.");
            if (checkingAppointmentExistsForSamePatient(appointmentPreCheckList.SamePatient) && checkingAppointmentExistsForDifferentPatient(appointmentPreCheckList.DifferentPatient))
            {

            }
            else
            {
                return;
            }
            //bool bPastApptCration = false;
            //Comment by bala for bug id: 18473 

            //if (Convert.ToDateTime(dtpApptDate.Text) < DateTime.Today)
            //{                

            //    //int Return = this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110034');", true);// ApplicationObject.erroHandler.DisplayErrorMessage("110034", "Edit Appointment", this.Page);
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110034');", true);// ApplicationObject.erroHandler.DisplayErrorMessage("110034", "Edit Appointment", this.Page);
            //    // Page.ClientScript.RegisterStartupScript(this.GetType(), "showAl", "confirm_user();", true);
            //    // bPastApptCration = true;
            //    //if (Return != 1)
            //    //{
            //    //    return;
            //    //}
            //}
            //if (bPastApptCration != true)
            //{

            //PreCheck - BlockDays Checking

            if (appointmentPreCheckList.bBlock == true && (appointmentPreCheckList.Block_Type == "RECURSIVE" || appointmentPreCheckList.Block_Type == "NON RECURSIVE"))
            {
                //if (YesNoCancel != "Yes")
                //{
                //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "BlockDays", "ConfirmBlockAppointment();", true);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "BlockDays", "ConfirmBlockAppointment(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
                //}
                //else
                //{
                //    hdnConfirmBlockAppointment.Value = "C";
                //}
            }
            else if (appointmentPreCheckList.bBlock == true && (appointmentPreCheckList.Block_Type != "RECURSIVE" || appointmentPreCheckList.Block_Type != "NON RECURSIVE"))
            {
                //Check Type of vist
                //logger.Debug("GetBlockCategory DB Call Starting");
                Stopwatch GetBlockCategoryDBCall = new Stopwatch();
                GetBlockCategoryDBCall.Start();
                string MyPhyIntList = PhyPOVMngr.GetBlockCategory(ddlVisitType.Text, ClientSession.LegalOrg);
                GetBlockCategoryDBCall.Stop();
                //logger.Debug("GetBlockCategory DB Call Completed. Time Taken : " + GetBlockCategoryDBCall.Elapsed.Seconds + "." + GetBlockCategoryDBCall.Elapsed.Milliseconds + "s.");
                //TOVLookupManager tovLookupMnger = new TOVLookupManager();
                //IList<TOVLookup> list = new List<TOVLookup>();
                //string sBlockCategory = tovLookupMnger.GetBlockCategory(ddlVisitType.Text);
                //if (sBlockCategory.ToUpper() != appointmentPreCheckList.Block_Type.ToUpper())
                //{
                //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "BlockDays", "ConfirmBlockAppointment('" + appointmentPreCheckList.Block_Type + "');", true);
                //    return;
                //}
                if (MyPhyIntList.ToUpper() != appointmentPreCheckList.Block_Type.ToUpper())
                {
                    //if (YesNoCancel != "Yes")
                    //{
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "BlockDays", "ConfirmBlockAppointment('" + appointmentPreCheckList.Block_Type + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                    //}
                    //else
                    //{
                    //    hdnConfirmBlockAppointment.Value = "C";
                    //}
                }

            }


            FacList = ApplicationObject.facilityLibraryList.Where(item => item.Fac_Name.Trim().ToUpper() == cboFacility.Text.Trim().ToUpper()).ToList<FacilityLibrary>();//FacMngr.GetFacilityListByFacilityName(cboFacility.Text);
            if (FacList != null && FacList.Count > 0)
            {
                hdnPOS.Value = FacList[0].POS;
            }

            Encounter EncRecord = new Encounter();
            WFObject WFObj = new WFObject();
            if (hdnEncounterID.Value != null && hdnEncounterID.Value.ToString() != "0")
            {
                if (appointmentPreCheckList.CurrentEncounter.Count > 0)
                {
                    EncRecord = appointmentPreCheckList.CurrentEncounter[0];
                }
            }
            //UtilityManager utilityMngr = new UtilityManager();

            //For Edit Appointment

            EncRecord.Purpose_of_Visit = txtPurposeofVisit.txtDLC.Text;
            EncRecord.Notes = txtNotes.txtDLC.Text;

            if (EncRecord.Id != 0 && chkReschedule.Checked == false)
            {
                string tt1 = "";
                try
                {
                    try
                    {
                        tt1 = Convert.ToDateTime(dtpStartTime.DbSelectedDate).ToString("tt");
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Appointment Time of value='" + dtpStartTime.DbSelectedDate + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    if (dtpStartTime.SelectedTime.Value != null)
                        EncRecord.Appointment_Date = Convert.ToDateTime(sp[0] + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + tt1);
                    EncRecord.Appointment_Date = UtilityManager.ConvertToUniversal(EncRecord.Appointment_Date);// change to universal
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Appointment Date of value='" + sp[0] + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + Convert.ToDateTime(dtpStartTime.DbSelectedDate).ToString("tt") + "' to DateTime threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    EncRecord.Duration_Minutes = Convert.ToInt32(ddlDuration.Text);
                    //Cap - 3217
                    if(IsAuthVerified.Checked == true)
                    {
                        EncRecord.Is_Auth_Verified = "Y";
                    }
                    else
                    {
                        EncRecord.Is_Auth_Verified = "N";
                    }
                    
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Duration_Minutes of value='" + ddlDuration.Text + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                EncRecord.Purpose_of_Visit = txtPurposeofVisit.txtDLC.Text;
                try
                {
                    //if (sFacilityCmg.ToUpper() == cboFacility.SelectedItem.Text.ToUpper())
                    //{
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                    {
                        if (ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value != null)
                            EncRecord.Machine_Technician_Library_ID = Convert.ToInt32(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value);
                        EncRecord.Appointment_Provider_ID = GetPhysicianLibIDByTechID(EncRecord.Machine_Technician_Library_ID);
                    }
                    else
                        EncRecord.Appointment_Provider_ID = Convert.ToInt32(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value); //ulMyPhysicianID;
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Duration_Minutes of value='" + ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value + "' to Int threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    //if (sFacilityCmg.ToUpper() == cboFacility.SelectedItem.Text.ToUpper())
                    //{
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                    {
                        ulMyPhysicianID = PhysicianId(Convert.ToUInt64(EncRecord.Machine_Technician_Library_ID));
                    }
                    else
                        ulMyPhysicianID = Convert.ToUInt64(EncRecord.Appointment_Provider_ID);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of PhysicianID of value='" + EncRecord.Appointment_Provider_ID + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    EncRecord.Encounter_Provider_ID = Convert.ToInt32(ulMyPhysicianID);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of PhysicianID of value='" + ulMyPhysicianID + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                EncRecord.Visit_Type = ddlVisitType.Text;
                try
                {
                    EncRecord.Human_ID = Convert.ToUInt64(txtPatientAccountNumber.Text);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of HumanID of value='" + txtPatientAccountNumber.Text + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                if (cboFacility.Text != EncRecord.Facility_Name)
                {
                    WFObj = wfMngr.GetByObjectSystemId(EncRecord.Id, sMyObjType);
                    WFObj.Fac_Name = cboFacility.Text;
                    EditWFobj = true;
                }
                EncRecord.Facility_Name = cboFacility.Text;
                EncRecord.Notes = txtNotes.txtDLC.Text;
                //     EncRecord.Is_Batch_Created = "N";
                //     EncRecord.Is_EandM_Submitted = "N";
                if (tabReferringProvAndPCP.SelectedIndex == 0)
                {
                    // commented by Valli
                    //   EncRecord.Referring_Facility = hdnrenprovider.Value;
                    //EncRecord.Referring_Physician = txtReferringProvider.Text;
                    //EncRecord.Referring_Address = txtReferingAddress.Text.Trim();
                    //EncRecord.Referring_Phone_No = msktxtReferingPhoneNo.TextWithLiterals;
                    //EncRecord.Referring_Fax_No = msktxtReferingFaxNo.TextWithLiterals;
                    //EncRecord.Referring_Provider_NPI = txtProviderNPI.Text.Trim();
                    if (!chkSelfReferred.Checked)
                    {
                        if (hdnrenprovider.Value != null && hdnrenprovider.Value != "" && hdnrenprovider.Value != "|||||")
                        {
                            EncRecord.Is_Self_Referred = "N";
                            //Jira #CAP-69 - labels are missing
                            //EncRecord.Referring_Facility = hdnrenprovider.Value.Split('|')[4].ToString();
                            //EncRecord.Referring_Physician = hdnrenprovider.Value.Split('|')[1].ToString();
                            //EncRecord.Referring_Address = hdnrenprovider.Value.Split('|')[5].ToString();
                            //EncRecord.Referring_Phone_No = hdnrenprovider.Value.Split('|')[7].ToString();
                            //EncRecord.Referring_Fax_No = hdnrenprovider.Value.Split('|')[6].ToString();
                            //EncRecord.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[2].ToString();

                            EncRecord.Referring_Facility = hdnrenprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Physician = hdnrenprovider.Value.Split('|')[0].ToString();
                            EncRecord.Referring_Address = hdnrenprovider.Value.Split('|')[3].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Phone_No = hdnrenprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Fax_No = hdnrenprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                        }
                        else
                        {
                            EncRecord.Is_Self_Referred = "N";
                            EncRecord.Referring_Facility = "";
                            EncRecord.Referring_Physician = "";
                            EncRecord.Referring_Address = "";
                            EncRecord.Referring_Phone_No = "";
                            EncRecord.Referring_Fax_No = "";
                            EncRecord.Referring_Provider_NPI = "";
                        }
                    }
                    else
                    {
                        EncRecord.Is_Self_Referred = "Y";
                        EncRecord.Referring_Facility = "";
                        EncRecord.Referring_Physician = "";
                        EncRecord.Referring_Address = "";
                        EncRecord.Referring_Phone_No = "";
                        EncRecord.Referring_Fax_No = "";
                        EncRecord.Referring_Provider_NPI = "";
                    }
                    if (hdnpcpprovider.Value != null && hdnpcpprovider.Value.Trim() != "" && hdnpcpprovider.Value != "|||||")
                    {
                        //Jira #CAP-69 - labels are missing
                        //EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[4].ToString();
                        //EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[1].ToString();
                        //EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[5].ToString();
                        //EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[7].ToString();
                        //EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[6].ToString();
                        //EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[2].ToString();

                        EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[3].Split(':')[1].ToString();
                        EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                    }
                    else
                    {
                        EncRecord.PCP_Facility = "";
                        EncRecord.PCP_Physician = "";
                        EncRecord.PCP_Address = "";
                        EncRecord.PCP_Phone_No = "";
                        EncRecord.PCP_Fax_No = "";
                        EncRecord.PCP_Provider_NPI = "";
                    }
                }
                else if (tabReferringProvAndPCP.SelectedIndex == 1)
                {
                    // commented by Valli
                    //EncRecord.PCP_Facility = txtReferringFacility.Text.Trim();
                    //EncRecord.PCP_Physician = txtReferringProvider.Text;
                    //EncRecord.PCP_Address = txtReferingAddress.Text.Trim();
                    //EncRecord.PCP_Phone_No = msktxtReferingPhoneNo.TextWithLiterals.Trim();
                    //EncRecord.PCP_Fax_No = msktxtReferingFaxNo.TextWithLiterals.Trim();
                    //EncRecord.PCP_Provider_NPI = txtProviderNPI.Text.Trim();
                    if (hdnpcpprovider.Value != null && hdnpcpprovider.Value.Trim() != "" && hdnpcpprovider.Value != "|||||")
                    {
                        //Jira #CAP-69 - labels are missing
                        //EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[4].ToString();
                        //EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[1].ToString();
                        //EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[5].ToString();
                        //EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[7].ToString();
                        //EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[6].ToString();
                        //EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[2].ToString();

                        EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[3].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                    }
                    else
                    {
                        EncRecord.PCP_Facility = "";
                        EncRecord.PCP_Physician = "";
                        EncRecord.PCP_Address = "";
                        EncRecord.PCP_Phone_No = "";
                        EncRecord.PCP_Fax_No = "";
                        EncRecord.PCP_Provider_NPI = "";
                    }
                    if (!chkSelfReferred.Checked)
                    {
                        if (hdnrenprovider.Value != null && hdnrenprovider.Value.Trim() != "" && hdnrenprovider.Value != "|||||")
                        {
                            EncRecord.Is_Self_Referred = "N";
                            //Jira #CAP-69 - labels are missing
                            //EncRecord.Referring_Facility = hdnrenprovider.Value.Split('|')[4].ToString();//vasanth for Saving Both ref And Pcp
                            //EncRecord.Referring_Physician = hdnrenprovider.Value.Split('|')[1].ToString();
                            //EncRecord.Referring_Address = hdnrenprovider.Value.Split('|')[5].ToString();
                            //EncRecord.Referring_Phone_No = hdnrenprovider.Value.Split('|')[7].ToString();
                            //EncRecord.Referring_Fax_No = hdnrenprovider.Value.Split('|')[6].ToString();
                            //EncRecord.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[2].ToString();

                            EncRecord.Referring_Facility = hdnrenprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Physician = hdnrenprovider.Value.Split('|')[0].ToString();
                            EncRecord.Referring_Address = hdnrenprovider.Value.Split('|')[3].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Phone_No = hdnrenprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Fax_No = hdnrenprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                            EncRecord.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                        }
                        else
                        {
                            EncRecord.Is_Self_Referred = "N";
                            EncRecord.Referring_Facility = "";
                            EncRecord.Referring_Physician = "";
                            EncRecord.Referring_Address = "";
                            EncRecord.Referring_Phone_No = "";
                            EncRecord.Referring_Fax_No = "";
                            EncRecord.Referring_Provider_NPI = "";
                        }
                    }
                    else
                    {
                        EncRecord.Is_Self_Referred = "Y";
                        EncRecord.Referring_Facility = "";
                        EncRecord.Referring_Physician = "";
                        EncRecord.Referring_Address = "";
                        EncRecord.Referring_Phone_No = "";
                        EncRecord.Referring_Fax_No = "";
                        EncRecord.Referring_Provider_NPI = "";
                    }
                }

                EncRecord.Notes = txtNotes.txtDLC.Text;
                // EncRecord.Is_Encounter_SuperBill = "N";
                //if (chkWillingnessInCancellation.Checked == true)
                //{
                //    EncRecord.Willing_For_Prior_Appointment = "Y";

                //}
                //else
                //{
                //    EncRecord.Willing_For_Prior_Appointment = "N";

                //}
                //Added by Bala for ADD MESSAGES IN APPOINTMENTS in 20-12-2013
                if (txtNotes.txtDLC.Text != string.Empty)
                {
                    PatientNotesManager patNotesMngr = new PatientNotesManager();
                    IList<PatientNotes> listPatientNotes = new List<PatientNotes>();
                    PatientNotes objPat = new PatientNotes();
                    try
                    {
                        objPat.Human_ID = Convert.ToUInt64(txtPatientAccountNumber.Text);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of HumanID of value='" + txtPatientAccountNumber.Text + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Message_Orign = "Appointment";
                    //if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    //{
                    try
                    {
                        objPat.Message_Date_And_Time = UtilityManager.ConvertToUniversal();//DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    //}
                    objPat.Message_Description = "Appointment";
                    objPat.Notes = txtNotes.txtDLC.Text;

                    //if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    //{
                    try
                    {
                        objPat.Created_Date_And_Time = UtilityManager.ConvertToUniversal();//DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    //}
                    if (ClientSession.UserName != null)
                        objPat.Created_By = ClientSession.UserName;
                    //objPat.Modified_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    objPat.Is_PatientChart = "N";
                    objPat.Line_ID = 0;
                    objPat.Type = "MESSAGE";
                    objPat.Encounter_ID = Convert.ToInt32(ulMyEncID);
                    objPat.Statement_ChargeLine_ID = 0;
                    objPat.SourceID = Convert.ToInt32(ulMyEncID);
                    objPat.Source = "APPOINTMENT";
                    objPat.IsDelete = "N";
                    objPat.Is_PopupEnable = "N";
                    objPat.Priority = "NORMAL";
                    listPatientNotes.Add(objPat);
                    //logger.Debug("AddToPatientNotes DB Call Starting");
                    Stopwatch AddToPatientNotesDBCall = new Stopwatch();
                    AddToPatientNotesDBCall.Start();
                    patNotesMngr.AddToPatientNotes(listPatientNotes);
                    AddToPatientNotesDBCall.Stop();
                    //logger.Debug("AddToPatientNotes DB Call Completed. Time Taken : " + AddToPatientNotesDBCall.Elapsed.Seconds + "." + AddToPatientNotesDBCall.Elapsed.Milliseconds);
                }

                //if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                //{
                try
                {
                    EncRecord.Modified_Date_and_Time = UtilityManager.ConvertToUniversal();//DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                    throw (exp);
                }
                //}
                EncRecord.Modified_By = ClientSession.UserName;
                //Cap - 2505
                // if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty)
                if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty && cboOrder.SelectedItem.Text != "Akido Order")
                {

                    EncRecord.Order_Submit_ID = Convert.ToInt32(cboOrder.SelectedItem.Text.Split('|')[0]);
                }
                else
                    EncRecord.Order_Submit_ID = 0;
                object[] temp = new object[] { "false" };
                IList<WFObject> lstWFobj = new List<WFObject>();
                if (EditWFobj)
                {
                    lstWFobj.Add(WFObj);
                }
                EncMngr.UpdateEncounterForRCM(EncRecord, lstWFobj, EditWFobj, string.Empty, "", temp);

                //   EncMngr.UpdateEncounterForRCM(EncRecord, lstWFobj, EditWFobj, string.Empty, txtAuthorizationNo.Text, temp);// GenerateIsCMGOrderObject()////not been used right now 03-02-2016 by vasanth
                EncMngr.TriggerSPforProvReviewStatusTracker("PROVIDER_CHANGE", EncRecord.Id);//SP checks if prov is changed..
                lstOrdersID = new List<string>();
                hdnOrderList.Value = string.Empty;
                #region oldcode not in use
                //if (txtAuthorizationNo.Text != string.Empty)
                //{
                //    IList<AuthorizationEncounter> authEncList = new List<AuthorizationEncounter>();
                //    AuthorizationEncounterManager authEncMngr = new AuthorizationEncounterManager();
                //    authEncList = authEncMngr.GetAuthdetailsByAuthID(Convert.ToUInt64(hdnAuthId.Value));
                //    IList<Authorization> authList = new List<Authorization>();
                //    AuthorizationManager authMngr = new AuthorizationManager();
                //    authList = authMngr.GetAuthorizationRecords(Convert.ToUInt64(hdnAuthSelectId.Value));
                //    if (authEncList.Count > 0)
                //    {
                //        if (authEncList[0].Authorization_ID == Convert.ToUInt64(hdnAuthId.Value) && authEncList[0].Is_Tied_To_Encounter == "Y" && authEncList[0].Authorization_ID != authList[0].Id)
                //        {
                //            //Update the Old Auth List
                //            IList<AuthorizationEncounter> objAuthEncUpdteList = new List<AuthorizationEncounter>();
                //            AuthorizationEncounter objAuthUpdate = new AuthorizationEncounter();
                //            objAuthUpdate.Id = authEncList[0].Id;
                //            objAuthUpdate.Authorization_ID = authEncList[0].Authorization_ID;
                //            objAuthUpdate.Encounter_ID = authEncList[0].Encounter_ID;
                //            objAuthUpdate.Human_ID = authEncList[0].Human_ID;
                //            objAuthUpdate.Is_Active = authEncList[0].Is_Active;
                //            objAuthUpdate.Is_Tied_To_Encounter = "N";
                //            objAuthUpdate.Modified_By = ClientSession.UserName;
                //            objAuthUpdate.Modified_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                //            objAuthUpdate.Version = authEncList[0].Version;
                //            objAuthEncUpdteList.Add(objAuthUpdate);
                //            authEncMngr.UpdateIsActive(objAuthEncUpdteList, string.Empty);

                //            //Create a New Auth List
                //            IList<AuthorizationEncounter> objAuthEncList = new List<AuthorizationEncounter>();
                //            AuthorizationEncounter objAuth = new AuthorizationEncounter();
                //            objAuth.Authorization_ID = authList[0].Id;
                //            objAuth.Encounter_ID = ulMyEncID;
                //            objAuth.Human_ID = ulMyHumanID;
                //            objAuth.Is_Active = "Y";
                //            objAuth.Is_Tied_To_Encounter = "Y";
                //            objAuth.Created_By = ClientSession.UserName;
                //            objAuth.Created_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                //            objAuthEncList.Add(objAuth);
                //            authEncMngr.AddlsttoAuthEncounter(objAuthEncList);
                //        }

                //    }
                //    else
                //    {
                //        IList<AuthorizationEncounter> objAuthEncList = new List<AuthorizationEncounter>();
                //        AuthorizationEncounter objAuth = new AuthorizationEncounter();
                //        objAuth.Authorization_ID = Convert.ToUInt64(hdnAuthId.Value);
                //        objAuth.Encounter_ID = ulMyEncID;
                //        objAuth.Human_ID = ulMyHumanID;
                //        objAuth.Is_Active = "Y";
                //        objAuth.Is_Tied_To_Encounter = "Y";
                //        objAuth.Created_By = ClientSession.UserName;
                //        objAuth.Created_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                //        objAuthEncList.Add(objAuth);
                //        authEncMngr.AddlsttoAuthEncounter(objAuthEncList);
                //    }
                //}
                #endregion
            }

            //For Reschedule
            if (EncRecord.Id != 0 && chkReschedule.Checked == true)
            {
                if (txtReasonCode.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('110048');", true);// ApplicationObject.erroHandler.DisplayErrorMessage("110048", this.Text);
                    return;
                }

                EncRecord.Reschedule_Reason_Code = ddlReasonCode.Text;
                EncRecord.Reschedule_Reason_Text = txtReasonCode.Text;
                EncRecord.Notes = txtNotes.txtDLC.Text;
                EncRecord.Visit_Type = ddlVisitType.Text;
                //Cap - 2505
                // if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty)
                if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty && cboOrder.SelectedItem.Text != "Akido Order")
                {

                    EncRecord.Order_Submit_ID = Convert.ToInt32(cboOrder.SelectedItem.Text.Split('|')[0]);
                }
                else
                    EncRecord.Order_Submit_ID = 0;

                //if (hdnLocalTime.Value != null&&hdnLocalTime.Value != string.Empty&&hdnLocalTime.Value.Trim() != "")
                //{
                try
                {
                    EncRecord.Modified_Date_and_Time = UtilityManager.ConvertToUniversal();//DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of LocalTime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                    throw (exp);
                }
                //}

                EncRecord.Modified_By = ClientSession.UserName;
                object[] temp = new object[] { "false" };
                //EncMngr.UpdateEncounterForRCM(EncRecord, null, false, string.Empty, txtAuthorizationNo.Text, temp);


                EncMngr.UpdateEncounterForRCM(EncRecord, null, false, string.Empty, "", temp);

                EncMngr.TriggerSPforProvReviewStatusTracker("PROVIDER_CHANGE", EncRecord.Id);//SP checks if prov is changed..
                //EncMngr.UpdateEncounterForRCM(EncRecord, string.Empty, txtAuthorizationNo.Text, temp);// GenerateIsCMGOrderObject()////not been used right now 03-02-2016 by vasanth


                wfMngr.MoveToNextProcess(EncRecord.Id, "ENCOUNTER", 4, "UNKNOWN", System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty, null, null);
                EncMngr.TriggerSPforProvReviewStatusTracker("INVALID", EncRecord.Id);//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review

                Encounter NewEnc = new Encounter();
                string tt1 = "";
                try
                {
                    try
                    {
                        tt1 = Convert.ToDateTime(dtpStartTime.DbSelectedDate).ToString("tt");
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Appointment Time of value='" + dtpStartTime.DbSelectedDate + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    if (dtpStartTime.SelectedTime.Value != null)
                        NewEnc.Appointment_Date = Convert.ToDateTime(sp[0] + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + tt1);
                    NewEnc.Appointment_Date = UtilityManager.ConvertToUniversal(NewEnc.Appointment_Date);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Appointment Date Time of value='" + sp[0] + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + tt + "' to DateTime threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    NewEnc.Duration_Minutes = Convert.ToInt32(ddlDuration.Text);
                    //Cap - 3217
                    if (IsAuthVerified.Checked == true)
                    {
                        NewEnc.Is_Auth_Verified = "Y";
                    }
                    else
                    {
                        NewEnc.Is_Auth_Verified = "N";
                    }
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Duration Time of value='" + ddlDuration.Text + "' to Int threw an error.", exp);
                    throw (exp);
                }
                NewEnc.Purpose_of_Visit = txtPurposeofVisit.txtDLC.Text;
                try
                {
                    //if (sFacilityCmg.ToUpper() == cboFacility.SelectedItem.Text.ToUpper())
                    //{
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                    {
                        NewEnc.Machine_Technician_Library_ID = Convert.ToInt32(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value);
                        ulMyPhysicianID = PhysicianId(Convert.ToUInt64(NewEnc.Machine_Technician_Library_ID));// Convert.ToUInt64(NewEnc.Machine_Technician_Library_ID);
                        NewEnc.Appointment_Provider_ID = GetPhysicianLibIDByTechID(NewEnc.Machine_Technician_Library_ID);
                    }
                    else
                    {
                        NewEnc.Appointment_Provider_ID = Convert.ToInt32(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value); //ulMyPhysicianID;
                        ulMyPhysicianID = Convert.ToUInt64(NewEnc.Appointment_Provider_ID);
                    }

                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Appointment_Provider_ID of value='" + ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    NewEnc.Encounter_Provider_ID = Convert.ToInt32(ulMyPhysicianID);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Encounter_Provider_ID of value='" + ulMyPhysicianID + "' to Int threw an error.", exp);
                    throw (exp);
                }
                NewEnc.Visit_Type = ddlVisitType.Text;
                try
                {
                    NewEnc.Human_ID = Convert.ToUInt64(txtPatientAccountNumber.Text);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Human_ID of value='" + txtPatientAccountNumber.Text + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                //Cap - 2505
                //if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty)
                if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty && cboOrder.SelectedItem.Text != "Akido Order")
                {

                    NewEnc.Order_Submit_ID = Convert.ToInt32(cboOrder.SelectedItem.Text.Split('|')[0]);
                }
                else
                    NewEnc.Order_Submit_ID = 0;
                NewEnc.Facility_Name = cboFacility.Text;
                NewEnc.Is_Batch_Created = "N";
                NewEnc.Is_EandM_Submitted = "N";
                NewEnc.File_Reference_Number = EncRecord.Id;
                if (tabReferringProvAndPCP.SelectedIndex == 0)
                {
                    // Commented by valli Need to check


                    if (hdnpcpprovider.Value != "" && hdnpcpprovider.Value != null && hdnpcpprovider.Value != "|||||")
                    {
                        //Jira #CAP-69 - labels are missing
                        //NewEnc.PCP_Facility = hdnpcpprovider.Value.Split('|')[4].ToString();
                        //NewEnc.PCP_Physician = hdnpcpprovider.Value.Split('|')[1].ToString();
                        //NewEnc.PCP_Address = hdnpcpprovider.Value.Split('|')[5].ToString();
                        //NewEnc.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[7].ToString();
                        //NewEnc.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[6].ToString();
                        //NewEnc.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[2].ToString();

                        NewEnc.PCP_Facility = hdnpcpprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        NewEnc.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        NewEnc.PCP_Address = hdnpcpprovider.Value.Split('|')[3].Split(':')[1].ToString();
                        NewEnc.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        NewEnc.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].Split(':')[1].ToString();
                        NewEnc.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();

                    }
                    if (!chkSelfReferred.Checked)
                    {
                        if (hdnrenprovider.Value != null && hdnrenprovider.Value.Trim() != "" && hdnrenprovider.Value != "|||||")
                        {
                            NewEnc.Is_Self_Referred = "N";
                            //Jira #CAP-69 - labels are missing
                            //NewEnc.Referring_Facility = hdnrenprovider.Value.Split('|')[4].ToString();//vasanth for Saving Both ref And Pcp
                            //NewEnc.Referring_Physician = hdnrenprovider.Value.Split('|')[1].ToString();
                            //NewEnc.Referring_Address = hdnrenprovider.Value.Split('|')[5].ToString();
                            //NewEnc.Referring_Phone_No = hdnrenprovider.Value.Split('|')[7].ToString();
                            //NewEnc.Referring_Fax_No = hdnrenprovider.Value.Split('|')[6].ToString();
                            //NewEnc.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[2].ToString();


                            NewEnc.Referring_Facility = hdnrenprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                            NewEnc.Referring_Physician = hdnrenprovider.Value.Split('|')[0].ToString();
                            NewEnc.Referring_Address = hdnrenprovider.Value.Split('|')[3].Split(':')[1].ToString();
                            NewEnc.Referring_Phone_No = hdnrenprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                            NewEnc.Referring_Fax_No = hdnrenprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                            NewEnc.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                        }
                    }
                    else
                    {
                        NewEnc.Is_Self_Referred = "Y";
                        NewEnc.Referring_Facility = string.Empty;
                        NewEnc.Referring_Physician = string.Empty;
                        NewEnc.Referring_Address = string.Empty;
                        NewEnc.Referring_Phone_No = string.Empty;
                        NewEnc.Referring_Fax_No = string.Empty;
                        NewEnc.Referring_Provider_NPI = string.Empty;
                    }
                }

                else if (tabReferringProvAndPCP.SelectedIndex == 1)
                {

                    // Commented by valli Need to check

                    //NewEnc.PCP_Facility = txtReferringFacility.Text.Trim();
                    //NewEnc.PCP_Physician = txtReferringProvider.Text;
                    //NewEnc.PCP_Address = txtReferingAddress.Text.Trim();
                    //NewEnc.PCP_Phone_No = msktxtReferingPhoneNo.TextWithLiterals;
                    //NewEnc.PCP_Fax_No = msktxtReferingFaxNo.TextWithLiterals;
                    //NewEnc.PCP_Provider_NPI = txtProviderNPI.Text.Trim();

                    if (hdnpcpprovider.Value != "" && hdnpcpprovider.Value != null && hdnpcpprovider.Value != "|||||")
                    {
                        //Jira #CAP-69 - labels are missing
                        //NewEnc.PCP_Facility = hdnpcpprovider.Value.Split('|')[4].ToString();
                        //NewEnc.PCP_Physician = hdnpcpprovider.Value.Split('|')[1].ToString();
                        //NewEnc.PCP_Address = hdnpcpprovider.Value.Split('|')[5].ToString();
                        //NewEnc.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[7].ToString();
                        //NewEnc.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[6].ToString();
                        //NewEnc.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[2].ToString();

                        NewEnc.PCP_Facility = hdnpcpprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        NewEnc.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        NewEnc.PCP_Address = hdnpcpprovider.Value.Split('|')[3].Split(':')[1].ToString();
                        NewEnc.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        NewEnc.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].Split(':')[1].ToString();
                        NewEnc.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                    }
                    if (!chkSelfReferred.Checked)
                    {
                        if (hdnrenprovider.Value != null && hdnrenprovider.Value.Trim() != "" && hdnrenprovider.Value != "|||||")
                        {
                            NewEnc.Is_Self_Referred = "N";
                            //Jira #CAP-69 - labels are missing
                            //NewEnc.Referring_Facility = hdnrenprovider.Value.Split('|')[4].ToString();//vasanth for Saving Both ref And Pcp
                            //NewEnc.Referring_Physician = hdnrenprovider.Value.Split('|')[1].ToString();
                            //NewEnc.Referring_Address = hdnrenprovider.Value.Split('|')[5].ToString();
                            //NewEnc.Referring_Phone_No = hdnrenprovider.Value.Split('|')[7].ToString();
                            //NewEnc.Referring_Fax_No = hdnrenprovider.Value.Split('|')[6].ToString();
                            //NewEnc.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[2].ToString();

                            NewEnc.Referring_Facility = hdnrenprovider.Value.Split('|')[3].Split(':')[1].Trim().ToString();
                            NewEnc.Referring_Physician = hdnrenprovider.Value.Split('|')[1].ToString();
                            NewEnc.Referring_Address = hdnrenprovider.Value.Split('|')[4].Split(':')[1].ToString();
                            NewEnc.Referring_Phone_No = hdnrenprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                            NewEnc.Referring_Fax_No = hdnrenprovider.Value.Split('|')[6].Split(':')[1].Trim().ToString();
                            NewEnc.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        }
                    }
                    else
                    {
                        NewEnc.Is_Self_Referred = "Y";
                        NewEnc.Referring_Facility = string.Empty;
                        NewEnc.Referring_Physician = string.Empty;
                        NewEnc.Referring_Address = string.Empty;
                        NewEnc.Referring_Phone_No = string.Empty;
                        NewEnc.Referring_Fax_No = string.Empty;
                        NewEnc.Referring_Provider_NPI = string.Empty;
                    }



                    //if (hdnpcpprovider.Value != "")
                    //{
                    //    NewEnc.PCP_Facility = hdnrenprovider.Value.Split('|')[4].ToString();
                    //    NewEnc.PCP_Physician = hdnrenprovider.Value.Split('|')[1].ToString();
                    //    NewEnc.PCP_Address = hdnrenprovider.Value.Split('|')[5].ToString();
                    //    NewEnc.PCP_Phone_No = hdnrenprovider.Value.Split('|')[7].ToString();
                    //    NewEnc.PCP_Fax_No = hdnrenprovider.Value.Split('|')[6].ToString();
                    //    NewEnc.PCP_Provider_NPI = hdnrenprovider.Value.Split('|')[2].ToString();
                    //}
                    //if (hdnrenprovider.Value != "")
                    //{

                    //    NewEnc.Referring_Facility = hdnrenprovider.Value.Split('|')[4].ToString();
                    //    NewEnc.Referring_Physician = hdnrenprovider.Value.Split('|')[1].ToString();
                    //    NewEnc.Referring_Address = hdnrenprovider.Value.Split('|')[5].ToString();
                    //    NewEnc.Referring_Phone_No = hdnrenprovider.Value.Split('|')[7].ToString();
                    //    NewEnc.Referring_Fax_No = hdnrenprovider.Value.Split('|')[6].ToString();
                    //    NewEnc.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[2].ToString();
                    //}
                }



                NewEnc.Notes = txtNotes.txtDLC.Text;
                NewEnc.Is_Encounter_SuperBill = "N";
                //if (chkWillingnessInCancellation.Checked == true)
                //{
                //    NewEnc.Willing_For_Prior_Appointment = "Y";

                //}
                //else
                //{
                //    NewEnc.Willing_For_Prior_Appointment = "N";

                //}
                //Added by Bala for ADD MESSAGES IN APPOINTMENTS in 20-12-2013
                if (txtNotes.txtDLC.Text != string.Empty)
                {
                    PatientNotesManager patNotesMngr = new PatientNotesManager();
                    IList<PatientNotes> listPatientNotes = new List<PatientNotes>();
                    PatientNotes objPat = new PatientNotes();
                    try
                    {
                        objPat.Human_ID = Convert.ToUInt64(txtPatientAccountNumber.Text);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Human_ID of value='" + txtPatientAccountNumber.Text + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Message_Orign = "Appointment";
                    //if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    //{
                    try
                    {
                        objPat.Message_Date_And_Time = UtilityManager.ConvertToUniversal();//DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Localtime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    //}
                    objPat.Message_Description = "Appointment";
                    objPat.Notes = txtNotes.txtDLC.Text;
                    //if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    //{
                    try
                    {
                        objPat.Created_Date_And_Time = UtilityManager.ConvertToUniversal(); //DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Localtime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    //}
                    if (ClientSession.UserName != null)
                        objPat.Created_By = ClientSession.UserName;
                    //objPat.Modified_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    objPat.Is_PatientChart = "N";
                    objPat.Line_ID = 0;
                    objPat.Type = "MESSAGE";
                    try
                    {
                        objPat.Encounter_ID = Convert.ToInt32(ulMyEncID);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Encounter_ID of value='" + ulMyEncID + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Statement_ChargeLine_ID = 0;
                    try
                    {
                        objPat.SourceID = Convert.ToInt32(ulMyEncID);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Encounter_ID of value='" + ulMyEncID + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Source = "APPOINTMENT";
                    objPat.IsDelete = "N";
                    objPat.Is_PopupEnable = "N";
                    objPat.Priority = "NORMAL";
                    listPatientNotes.Add(objPat);
                    //logger.Debug("AddToPatientNotes DB Call Starting");
                    Stopwatch AddToPatientNotesDBCall = new Stopwatch();
                    AddToPatientNotesDBCall.Start();
                    patNotesMngr.AddToPatientNotes(listPatientNotes);
                    AddToPatientNotesDBCall.Stop();
                    //logger.Debug("AddToPatientNotes DB Call Completed. Time Taken : " + AddToPatientNotesDBCall.Elapsed.Seconds + "." + AddToPatientNotesDBCall.Elapsed.Milliseconds);
                }


                WFObj.Obj_Type = sMyObjType;
                //if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                //{
                try
                {
                    WFObj.Current_Arrival_Time = UtilityManager.ConvertToUniversal(); //DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Localtime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                    throw (exp);
                }
                //}
                WFObj.Current_Owner = "UNKNOWN";
                WFObj.Fac_Name = hdnFacilityName.Value;
                WFObj.Obj_System_Id = NewEnc.Id;
                WFObj.Current_Process = "START";
                //WFObj.Id = WFProxy.InsertToWorkFlowObject(WFObj);

                if (NewEnc.Id == 0)
                {
                    //if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    //{
                    try
                    {
                        NewEnc.Created_Date_and_Time = UtilityManager.ConvertToUniversal();//DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Localtime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    //}
                    NewEnc.Created_By = ClientSession.UserName;
                    NewEnc.Is_Physician_Asst_Process = "N";
                    NewEnc.Notes = txtNotes.txtDLC.Text;
                    EncMngr = new EncounterManager();
                    //logger.Debug("CreateEncounterForRCM DB Call Starting");
                    temp = new object[] { "false" };
                    Stopwatch CreateEncounterForRCMDBCall = new Stopwatch();
                    CreateEncounterForRCMDBCall.Start();

                    NewEnc.Id = EncMngr.CreateEncounterForRCM(NewEnc, WFObj, string.Empty, "", temp);
                    // NewEnc.Id = EncMngr.CreateEncounterForRCM(NewEnc, WFObj, string.Empty, txtAuthorizationNo.Text, temp);// GenerateIsCMGOrderObject()////not been used right now 03-02-2016 by vasanth
                    EncMngr.TriggerSPforProvReviewStatusTracker("WALK_IN", NewEnc.Id);//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
                    CreateEncounterForRCMDBCall.Stop();
                    //logger.Debug("CreateEncounterForRCM DB Call Completed. Time Taken : " + CreateEncounterForRCMDBCall.Elapsed.Seconds + "." + CreateEncounterForRCMDBCall.Elapsed.Milliseconds);
                    lstOrdersID = new List<string>();
                    hdnOrderList.Value = string.Empty;
                }
            }

            //Other than Reschedule - New Appointment
            if (chkReschedule.Checked != true && EncRecord.Id == 0)
            {
                //srividhya
                //EncRecord.Appointment_Date = ToLoalTime.ToUniversal(Session["UniversalTime"].ToString(), Convert.ToDateTime(sp[0] + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + Convert.ToDateTime(dtpStartTime.DbSelectedDate).ToString("tt")));
                string tt1 = "";
                try
                {
                    try
                    {
                        tt1 = Convert.ToDateTime(dtpStartTime.DbSelectedDate).ToString("tt");
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Appointment Time of value='" + dtpStartTime.DbSelectedDate + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    EncRecord.Appointment_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(sp[0] + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + tt1));
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Appointment DateTime of value='" + sp[0] + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + " " + tt + "' to DateTime threw an error.", exp);
                    throw (exp);
                }
                //  EncRecord.Appointment_Date = UtilityManager.ConvertToUniversal(EncRecord.Appointment_Date);
                //EncRecord.Check_Date = UtilityManager.ConvertToUniversal(DateTime.MinValue);
                try
                {
                    EncRecord.Duration_Minutes = Convert.ToInt32(ddlDuration.Text);
                    //Cap - 3217
                    if (IsAuthVerified.Checked == true)
                    {
                        EncRecord.Is_Auth_Verified = "Y";
                    }
                    else
                    {
                        EncRecord.Is_Auth_Verified = "N";
                    }
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Duration Time of value='" + ddlDuration.Text + "' to Int threw an error.", exp);
                    throw (exp);
                }
                EncRecord.Purpose_of_Visit = txtPurposeofVisit.txtDLC.Text;
                try
                {
                    //if (sFacilityCmg.ToUpper() == cboFacility.SelectedItem.Text.ToUpper())
                    //{
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                    {
                        EncRecord.Machine_Technician_Library_ID = Convert.ToInt32(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value);
                        EncRecord.Appointment_Provider_ID = GetPhysicianLibIDByTechID(EncRecord.Machine_Technician_Library_ID);
                    }
                    else
                        EncRecord.Appointment_Provider_ID = Convert.ToInt32(ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value); //ulMyPhysicianID;
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Appointment_Provider_ID of value='" + ddlPhysicianName.Items[ddlPhysicianName.SelectedIndex].Value + "' to Int threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    //if (sFacilityCmg.ToUpper() == cboFacility.SelectedItem.Text.ToUpper())
                    //{
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacility.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                    {
                        ulMyPhysicianID = PhysicianId(Convert.ToUInt64(EncRecord.Machine_Technician_Library_ID)); //Convert.ToUInt64(EncRecord.Machine_Technician_Library_ID);
                    }
                    else
                        ulMyPhysicianID = Convert.ToUInt64(EncRecord.Appointment_Provider_ID);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Appointment_Provider_ID of value='" + EncRecord.Appointment_Provider_ID + "' to UInt threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    EncRecord.Encounter_Provider_ID = Convert.ToInt32(ulMyPhysicianID);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Encounter_Provider_ID of value='" + ulMyPhysicianID + "' to Int threw an error.", exp);
                    throw (exp);
                }
                EncRecord.Visit_Type = ddlVisitType.Text;
                try
                {
                    EncRecord.Human_ID = Convert.ToUInt64(txtPatientAccountNumber.Text);
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Human_ID of value='" + txtPatientAccountNumber.Text + "' to Int threw an error.", exp);
                    throw (exp);
                }
                EncRecord.Facility_Name = cboFacility.Text;
                EncRecord.Place_Of_Service = hdnPOS.Value;
                EncRecord.Is_Batch_Created = "N";
                EncRecord.Is_EandM_Submitted = "N";

                // Commented by valli need to check
                if (tabReferringProvAndPCP.SelectedIndex == 0)
                {
                    if (!chkSelfReferred.Checked)
                    {
                        if (hdnrenprovider.Value != "" && hdnrenprovider.Value != "|||||")
                        {
                            EncRecord.Is_Self_Referred = "N";
                            //Jira #CAP-69 - labels are missing
                            //EncRecord.Referring_Facility = hdnrenprovider.Value.Split('|')[4].ToString();
                            //EncRecord.Referring_Physician = hdnrenprovider.Value.Split('|')[1].ToString();
                            //EncRecord.Referring_Address = hdnrenprovider.Value.Split('|')[5].ToString();
                            //EncRecord.Referring_Phone_No = hdnrenprovider.Value.Split('|')[7].ToString();
                            //EncRecord.Referring_Fax_No = hdnrenprovider.Value.Split('|')[6].ToString();
                            //EncRecord.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[2].ToString();

                            EncRecord.Referring_Facility = hdnrenprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString().ToString();
                            EncRecord.Referring_Physician = hdnrenprovider.Value.Split('|')[0].ToString();
                            EncRecord.Referring_Address = hdnrenprovider.Value.Split('|')[3].Split(':')[1].Trim().ToString().ToString();
                            EncRecord.Referring_Phone_No = hdnrenprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString().ToString();
                            EncRecord.Referring_Fax_No = hdnrenprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString().ToString();
                            EncRecord.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString().ToString();

                        }
                    }
                    else
                    {
                        EncRecord.Is_Self_Referred = "Y";
                        EncRecord.Referring_Facility = string.Empty;
                        EncRecord.Referring_Physician = string.Empty;
                        EncRecord.Referring_Address = string.Empty;
                        EncRecord.Referring_Phone_No = string.Empty;
                        EncRecord.Referring_Fax_No = string.Empty;
                        EncRecord.Referring_Provider_NPI = string.Empty;
                    }

                    //EncRecord.Referring_Facility = txtReferringFacility.Text.Trim();
                    //EncRecord.Referring_Physician = txtReferringProvider.Text;
                    //EncRecord.Referring_Address = txtReferingAddress.Text.Trim();
                    //EncRecord.Referring_Phone_No = msktxtReferingPhoneNo.TextWithLiterals;
                    //EncRecord.Referring_Fax_No = msktxtReferingFaxNo.TextWithLiterals;
                    //EncRecord.Referring_Provider_NPI = txtProviderNPI.Text.Trim();

                    if (HdnPcpPhy != null && HdnPcpPhy.Value != string.Empty && HdnPcpPhy.Value != "|||||")
                    {
                        EncRecord.PCP_Facility = HdnPcpPhy.Value.Split('|')[5].ToString();//vasanth for Saving Both ref And Pcp
                        EncRecord.PCP_Physician = HdnPcpPhy.Value.Split('|')[0].ToString();
                        EncRecord.PCP_Address = HdnPcpPhy.Value.Split('|')[1].ToString();
                        EncRecord.PCP_Phone_No = HdnPcpPhy.Value.Split('|')[2].ToString();
                        EncRecord.PCP_Fax_No = HdnPcpPhy.Value.Split('|')[3].ToString();
                        EncRecord.PCP_Provider_NPI = HdnPcpPhy.Value.Split('|')[4].ToString();

                    }
                    //Jira #CAP-69 - labels are missing
                    else if (hdnpcpprovider.Value != "" && hdnpcpprovider.Value != null && hdnpcpprovider.Value != "|||||")
                    {
                        EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[3].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                    }
                }

                else if (tabReferringProvAndPCP.SelectedIndex == 1)
                {
                    //EncRecord.PCP_Facility = txtReferringFacility.Text.Trim();
                    //EncRecord.PCP_Physician = txtReferringProvider.Text;
                    //EncRecord.PCP_Address = txtReferingAddress.Text.Trim();
                    //EncRecord.PCP_Phone_No = msktxtReferingPhoneNo.TextWithLiterals;
                    //EncRecord.PCP_Fax_No = msktxtReferingFaxNo.TextWithLiterals;
                    //EncRecord.PCP_Provider_NPI = txtProviderNPI.Text.Trim();
                    if (hdnpcpprovider.Value != "" && hdnpcpprovider.Value != null && hdnpcpprovider.Value != "|||||")
                    {
                        //Jira #CAP-69 - labels are missing
                        //EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[4].ToString();
                        //EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[1].ToString();
                        //EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[5].ToString();
                        //EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[7].ToString();
                        //EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[6].ToString();
                        //EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[2].ToString();

                        EncRecord.PCP_Facility = hdnpcpprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Physician = hdnpcpprovider.Value.Split('|')[0].ToString();
                        EncRecord.PCP_Address = hdnpcpprovider.Value.Split('|')[3].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Phone_No = hdnpcpprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Fax_No = hdnpcpprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString();
                        EncRecord.PCP_Provider_NPI = hdnpcpprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString();
                    }
                    if (!chkSelfReferred.Checked)
                    {
                        if (hdnrenprovider.Value != null && hdnrenprovider.Value.Trim() != "" && hdnrenprovider.Value != "|||||")
                        {
                            EncRecord.Is_Self_Referred = "N";
                            EncRecord.Referring_Facility = hdnrenprovider.Value.Split('|')[2].Split(':')[1].Trim().ToString().ToString();
                            EncRecord.Referring_Physician = hdnrenprovider.Value.Split('|')[0].ToString();
                            EncRecord.Referring_Address = hdnrenprovider.Value.Split('|')[3].Split(':')[1].Trim().ToString().ToString();
                            EncRecord.Referring_Phone_No = hdnrenprovider.Value.Split('|')[4].Split(':')[1].Trim().ToString().ToString();
                            EncRecord.Referring_Fax_No = hdnrenprovider.Value.Split('|')[5].Split(':')[1].Trim().ToString().ToString();
                            EncRecord.Referring_Provider_NPI = hdnrenprovider.Value.Split('|')[1].Split(':')[1].Trim().ToString().ToString();
                        }
                    }
                    else
                    {
                        EncRecord.Is_Self_Referred = "Y";
                        EncRecord.Referring_Facility = string.Empty;
                        EncRecord.Referring_Physician = string.Empty;
                        EncRecord.Referring_Address = string.Empty;
                        EncRecord.Referring_Phone_No = string.Empty;
                        EncRecord.Referring_Fax_No = string.Empty;
                        EncRecord.Referring_Provider_NPI = string.Empty;
                    }
                }
                EncRecord.Notes = txtNotes.txtDLC.Text;
                EncRecord.Is_Encounter_SuperBill = "N";
                //if (chkWillingnessInCancellation.Checked == true)
                //{
                //    EncRecord.Willing_For_Prior_Appointment = "Y";

                //}
                //else
                //{
                //    EncRecord.Willing_For_Prior_Appointment = "N";

                //}
                //Added by Bala for ADD MESSAGES IN APPOINTMENTS in 20-12-2013
                if (txtNotes.txtDLC.Text != string.Empty)
                {
                    PatientNotesManager patNotesMngr = new PatientNotesManager();
                    IList<PatientNotes> listPatientNotes = new List<PatientNotes>();
                    PatientNotes objPat = new PatientNotes();
                    try
                    {
                        objPat.Human_ID = Convert.ToUInt64(txtPatientAccountNumber.Text);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Human_ID of value='" + txtPatientAccountNumber.Text + "' to Int threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Message_Orign = "Appointment";
                    //if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    //{
                    try
                    {
                        objPat.Message_Date_And_Time = UtilityManager.ConvertToUniversal(); //DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Localtime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    //}
                    objPat.Message_Description = "Appointment";
                    objPat.Notes = txtNotes.txtDLC.Text;
                    //if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    //{
                    try
                    {
                        objPat.Created_Date_And_Time = UtilityManager.ConvertToUniversal(); //DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Localtime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    //}
                    if (ClientSession.UserName != null)
                        objPat.Created_By = ClientSession.UserName;
                    //objPat.Modified_Date_And_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    objPat.Is_PatientChart = "N";
                    objPat.Line_ID = 0;
                    objPat.Type = "MESSAGE";
                    try
                    {
                        objPat.Encounter_ID = Convert.ToInt32(ulMyEncID);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Encounter_ID of value='" + ulMyEncID + "' to Int threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Statement_ChargeLine_ID = 0;
                    try
                    {
                        objPat.SourceID = Convert.ToInt32(ulMyEncID);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Encounter_ID of value='" + ulMyEncID + "' to Int threw an error.", exp);
                        throw (exp);
                    }
                    objPat.Source = "APPOINTMENT";
                    objPat.IsDelete = "N";
                    objPat.Is_PopupEnable = "N";
                    objPat.Priority = "NORMAL";
                    listPatientNotes.Add(objPat);
                    //logger.Debug("AddToPatientNotes DB Call Starting");
                    Stopwatch AddToPatientNotesDBCall = new Stopwatch();
                    AddToPatientNotesDBCall.Start();
                    patNotesMngr.AddToPatientNotes(listPatientNotes);
                    AddToPatientNotesDBCall.Stop();
                    //logger.Debug("AddToPatientNotes DB Call Completed. Time Taken : " + AddToPatientNotesDBCall.Elapsed.Seconds + "." + AddToPatientNotesDBCall.Elapsed.Milliseconds);
                }


                if (ulMyEncID == 0)
                {
                    WFObj.Obj_Type = sMyObjType;
                    if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    {
                        //  WFObj.Current_Arrival_Time = DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    }
                    WFObj.Current_Owner = "UNKNOWN";
                    if (hdnFacilityName.Value != "")
                        WFObj.Fac_Name = hdnFacilityName.Value;
                    else
                        WFObj.Fac_Name = cboFacility.SelectedItem.Text;
                    WFObj.Obj_System_Id = EncRecord.Id;
                    WFObj.Current_Process = "START";
                    //WFObj.Id = WFProxy.InsertToWorkFlowObject(WFObj);
                }

                if (EncRecord.Id == 0)
                {
                    //if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty && hdnLocalTime.Value.Trim() != "")
                    //{
                    try
                    {
                        EncRecord.Created_Date_and_Time = UtilityManager.ConvertToUniversal(); //DateTime.ParseExact(hdnLocalTime.Value.ToString(), "M/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Localtime of value='" + hdnLocalTime.Value.ToString() + "' to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    //}
                    EncRecord.Created_By = ClientSession.UserName;
                    EncRecord.Is_Physician_Asst_Process = "N";
                    EncRecord.Notes = txtNotes.txtDLC.Text;
                    //Cap - 2505
                    //if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty)
                    if (cboOrder.SelectedItem != null && cboOrder.SelectedItem.Text != string.Empty && cboOrder.SelectedItem.Text != "Akido Order")
                    {

                        EncRecord.Order_Submit_ID = Convert.ToInt32(cboOrder.SelectedItem.Text.Split('|')[0]);
                    }
                    else
                        EncRecord.Order_Submit_ID = 0;
                    EncMngr = new EncounterManager();
                    //logger.Debug("CreateEncounterForRCM DB Call Starting");
                    object[] temp = new object[] { "false" };
                    Stopwatch CreateEncounterForRCMDBCall = new Stopwatch();
                    CreateEncounterForRCMDBCall.Start();


                    EncRecord.Id = EncMngr.CreateEncounterForRCM(EncRecord, WFObj, string.Empty, "", temp);// GenerateIsCMGOrderObject()////not been used right now 03-02-2016 by vasanth

                    // EncRecord.Id = EncMngr.CreateEncounterForRCM(EncRecord, WFObj, string.Empty, txtAuthorizationNo.Text, temp);// GenerateIsCMGOrderObject()////not been used right now 03-02-2016 by vasanth
                    EncMngr.TriggerSPforProvReviewStatusTracker("WALK_IN", EncRecord.Id);//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
                    CreateEncounterForRCMDBCall.Stop();
                    //logger.Debug("CreateEncounterForRCM DB Call Completed. Time Taken : " + CreateEncounterForRCMDBCall.Elapsed.Seconds + "." + CreateEncounterForRCMDBCall.Elapsed.Milliseconds);
                    lstOrdersID = new List<string>();
                }
            }
            // ApplicationObject.erroHandler.DisplayErrorMessage("110019", "Edit Appointment", this.Page);
            //if (YesNoCancel != "Yes")
            //{
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110019'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //}
            try
            {
                CalendarDate = Convert.ToDateTime(dtpApptDate.SelectedDate.Value);
                MyTime = Convert.ToDateTime(dtpApptDate.SelectedDate.Value);
            }
            catch (Exception exp)
            {
                //logger.Debug("Conversion of Calender Selected Date of value='" + dtpApptDate.SelectedDate.Value + "' to DateTime threw an error.", exp);
                throw (exp);
            }
            //if (YesNoCancel != "Yes")
            //{
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "showVal", "CloseWindow();", true);
            //}

            // ScriptManager.RegisterStartupScript(EditAppointment, EditAppointment.GetType(), "showVal", "CloseWindow();", true);
            //if (YesNoCancel == "Yes")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Exits1();", true);
            //}
        }

        public ulong PhysicianId(ulong Machinelibraryid)
        {
            ulong ulPhyID = 0;

            //Jira CAP-2777
            //XmlDocument xmldoc = new XmlDocument();

            //string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
            //if (File.Exists(strXmlFilePathTech) == true)
            {
                //Jira CAP-2777
                //xmldoc = new XmlDocument();
                //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                if (Machinelibraryid.ToString() != "0")
                {
                    //Jira CAP-2777
                    //XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + Machinelibraryid.ToString());
                    //if (xmlTec != null && xmlTec.Count > 0)
                    //    ulPhyID = Convert.ToUInt32(xmlTec[0].Attributes.GetNamedItem("Physician_Library_ID").Value);

                    //Jira CAP-2777
                    MachinetechnicianList machinetechnicianList = new MachinetechnicianList();
                    machinetechnicianList = ConfigureBase<MachinetechnicianList>.ReadJson("machine_technician.json");
                    if (machinetechnicianList?.MachineTechnician != null)
                    {
                        List<Machinetechnician> machinetechnicians = new List<Machinetechnician>();
                        machinetechnicians = machinetechnicianList.MachineTechnician.Where(x => x.machine_technician_library_id == Machinelibraryid.ToString()).ToList();
                        if (machinetechnicians.Count > 0)
                        {
                            ulPhyID = Convert.ToUInt32(machinetechnicians[0].Physician_Library_ID);
                        }
                    }
                }
            }
            return ulPhyID;
        }

        private void FillPOV()
        {
            #region oldcode
            //Encounter EncRecord = new Encounter();
            //IList<PhysicianPOV> MyPhyIntList = new List<PhysicianPOV>();
            //if (hdnPhysicianID.Value != "")
            //    MyPhyIntList = PhyPOVMngr.GetPhysicianInterListbyPhysicianID(Convert.ToUInt64(hdnPhysicianID.Value));
            //if (MyPhyIntList.Count == 0)
            //{
            //    //ApplicationObject.erroHandler.DisplayErrorMessage("110026", "Edit Appointment", this.Page);
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110026');", true);
            //    return;
            //}

            //PhysicianPOV physicianIntermediate;

            //int iMyDefaultIndex = 0;

            //ddlVisitType.Items.Clear();

            //duration = new int[MyPhyIntList.Count];
            //description = new string[MyPhyIntList.Count];

            //if (MyPhyIntList != null)
            //{
            //    for (int i = 0; i < MyPhyIntList.Count; i++)
            //    {
            //        physicianIntermediate = MyPhyIntList[i];
            //        RadComboBoxItem item = new RadComboBoxItem();
            //        item.Text = physicianIntermediate.Purpose_of_Visit.ToUpper();
            //        ddlVisitType.Items.Add(item);
            //        duration[i] = physicianIntermediate.Duration;
            //        description[i] = physicianIntermediate.Description;

            //        if (physicianIntermediate.Default_Value == physicianIntermediate.Purpose_of_Visit)
            //        {
            //            iMyDefaultIndex = i;
            //        }


            //    }
            //}
            //if (duration.Count() > 0)
            //{
            //    Session["duration"] = duration;
            //}
            //if (description.Count() > 0)
            //{
            //    Session["description"] = description;
            //}

            ////   cboDuration.Value = duration[0];
            //ddlVisitType.SelectedIndex = iMyDefaultIndex;
            //ddlDuration.Text = duration[iMyDefaultIndex].ToString();

            #endregion

            //logger.Debug("Filling Type Of Visit Combobox");
            ddlVisitType.Items.Clear();

            //Jira CAP-2779
            //string povphysicianId = "P" + ClientSession.PhysicianId.ToString();
            //XmlDocument xmldocUser = new XmlDocument();
            //xmldocUser.Load(Server.MapPath(@"ConfigXML\Physician_POV.xml"));
            //XmlNodeList xmlUserList = xmldocUser.GetElementsByTagName(povphysicianId);
            //duration = new int[xmlUserList.Count];
            //description = new string[xmlUserList.Count];
            //string DefaultValue = "";

            //if (xmlUserList.Count > 0)
            //{
            //    //logger.Debug("XML tag " + povphysicianId + " found");
            //    string Purpose_of_Visit = "";
            //    string Duration = "";
            //    string Description = "";
            //    int icount = 0;
            //    int iselected = -1;

            //    DefaultValue = xmlUserList[0].Attributes[4].Value;
            //    foreach (XmlNode item in xmlUserList)
            //    {
            //        if (item.Attributes[5].Value == ClientSession.LegalOrg)
            //        {
            //            Purpose_of_Visit = item.Attributes[0].Value;
            //            Duration = item.Attributes[1].Value;
            //            Description = item.Attributes[3].Value;
            //            RadComboBoxItem items = new RadComboBoxItem();
            //            items.Text = Purpose_of_Visit.ToUpper();
            //            //items.Value = Duration + "$#%" + Description;
            //            ddlVisitType.Items.Add(items);
            //            string visitType = string.IsNullOrEmpty(HdnEditVisit.Value.Split('|')[0].ToUpper()) ? DefaultValue.ToUpper() : HdnEditVisit.Value.Split('|')[0].ToUpper();
            //            if (ddlVisitType.Items[icount].Text.ToUpper() == visitType)
            //                iselected = icount;
            //            //ddlVisitType.Items[icount].Selected = true;
            //            //  else
            //            //   ddlVisitType.Items[icount].Selected = false;
            //            duration[icount] = Convert.ToInt32(Duration);
            //            description[icount] = Description;
            //            icount++;
            //        }
            //    }
            //    ddlVisitType.SelectedIndex = iselected;

            //    //string[] splitvalues = ddlVisitType.SelectedItem.Value.Split(new string[] { "$#%" }, StringSplitOptions.None);
            //    //if (splitvalues != null && splitvalues.Length == 2)
            //    //{
            //    //    ddlDuration.Text = splitvalues[0];
            //    //    txtVisitDescription.Text = splitvalues[1];
            //    //}
            //    if (ddlVisitType.SelectedIndex != -1)
            //    {
            //        ddlDuration.Text = duration[ddlVisitType.SelectedIndex].ToString();
            //        txtVisitDescription.Text = description[ddlVisitType.SelectedIndex].ToString();
            //    }
            //}
            //else if (xmlUserList.Count == 0)

            //Jira CAP-2779
            Physician_POVList physicianPOVList = new Physician_POVList();
            physicianPOVList = ConfigureBase<Physician_POVList>.ReadJson("Physician_POV.json");
            if (physicianPOVList != null)
            {
                List<Physician_POV> ilstPhyPov = new List<Physician_POV>();
                ilstPhyPov = physicianPOVList.Physician_POV.Where(x => x.Physician_Library_ID == ClientSession.PhysicianId.ToString()).ToList();
                duration = new int[ilstPhyPov.Count];
                description = new string[ilstPhyPov.Count];
                string DefaultValue = "";
                if (ilstPhyPov.Count > 0)
                {
                    string Purpose_of_Visit = "";
                    string Duration = "";
                    string Description = "";
                    int icount = 0;
                    int iselected = -1;

                    DefaultValue = ilstPhyPov[0].Default_Value;
                    ilstPhyPov = ilstPhyPov.Where(x => x.Legal_Org == ClientSession.LegalOrg).ToList();
                    foreach (Physician_POV physicianPOV in ilstPhyPov)
                    {
                        
                            Purpose_of_Visit = physicianPOV.Purpose_of_Visit;
                            Duration = physicianPOV.Duration;
                            Description = physicianPOV.Description;
                            RadComboBoxItem items = new RadComboBoxItem();
                            items.Text = Purpose_of_Visit.ToUpper();
                            ddlVisitType.Items.Add(items);
                            string visitType = string.IsNullOrEmpty(HdnEditVisit.Value.Split('|')[0].ToUpper()) ? DefaultValue.ToUpper() : HdnEditVisit.Value.Split('|')[0].ToUpper();
                            if (ddlVisitType.Items[icount].Text.ToUpper() == visitType)
                                iselected = icount;
                            duration[icount] = Convert.ToInt32(Duration);
                            description[icount] = Description;
                            icount++;
                    }
                    ddlVisitType.SelectedIndex = iselected;

                    if (ddlVisitType.SelectedIndex != -1)
                    {
                        ddlDuration.Text = duration[ddlVisitType.SelectedIndex].ToString();
                        txtVisitDescription.Text = description[ddlVisitType.SelectedIndex].ToString();
                    }
                }
                else if (ilstPhyPov.Count == 0)
                {
                    //Jira #CAP-168 - check the deactivate (selectedUsers) user count 
                    IList<PhysicianLibrary> PhysicianList = UtilityManager.GetPhysicianList(hdnFacilityName.Value.Trim(), ClientSession.LegalOrg);
                    //CAP-3499
                    if (PhysicianList != null)
                    {
                        var otherPhysicianList = UtilityManager.GetInActiveProviderList(hdnFacilityName.Value.Trim(), ClientSession.LegalOrg, true);
                        foreach (var physician in otherPhysicianList)
                        {
                            PhysicianList.Add(physician);
                        }
                    }
                    var user = from u in PhysicianList where u.PhyId == ulMyPhysicianID select u.PhyId;
                    IList<ulong> selectedUsers = new List<ulong>();
                    selectedUsers = user.ToList<ulong>();
                    if (selectedUsers.Count > 0)
                    {
                        //logger.Debug("XML tag " + povphysicianId + " not found");
                        ddlVisitType.SelectedIndex = -1;
                        ddlDuration.Text = "";
                        txtVisitDescription.Text = "";
                        if (!(Request["Imported"] != null && Request["Imported"].ToString() == "Y"))
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110026');", true);
                            return;
                        }
                    }
                }
            }
            if (duration.Count() > 0)
            {
                Session["duration"] = duration;
            }
            if (description.Count() > 0)
            {
                Session["description"] = description;
            }
        }

        /*unused*/
        public string convertToTime(string t)
        {
            string[] sp = t.Split(' ');
            string[] sp1 = sp[1].Split(':');
            if (sp[2] == "PM")
            {
                if (sp1[0] != "12")
                {
                    sp1[0] = (Convert.ToInt16(sp1[0]) + 12).ToString();
                }
            }
            else
            {
                if (sp1[0] == "12")
                {
                    sp1[0] = "00";
                }
            }
            return (sp1[0] + ":" + sp1[1] + ":" + sp1[2]);

        }

        private bool checkingAppointmentExistsForSamePatient(IList<Encounter> appointmentList)
        {
            Encounter EncRecord = null;

            DateTime MyCalendarDateTime = Convert.ToDateTime(dtpApptDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + ":" + dtpStartTime.SelectedTime.Value.Seconds);
            //appointmentList = apptProxy.GetAppointmentByPatientIDandPhysicianIDandDate(ulMyHumanID, ulMyPhysicianID, MyCalendarDateTime,true);

            if (appointmentList.Count > 0)
            {
                for (int i = 0; i < appointmentList.Count; i++)
                {
                    EncRecord = appointmentList[i];
                    DateTime startTime = EncRecord.Appointment_Date;
                    DateTime endTime = startTime.AddMinutes(Convert.ToDouble(EncRecord.Duration_Minutes));

                    //If Reschedule - there will be two line items in encounter table
                    if (chkReschedule.Checked == true)
                    {
                        continue;
                    }
                    if (hdnEncounterID.Value != string.Empty)
                    {
                        ulMyEncID = Convert.ToUInt64(hdnEncounterID.Value);
                    }
                    if (EncRecord.Id != ulMyEncID)
                    {
                        if (((MyCalendarDateTime >= startTime && MyCalendarDateTime <= endTime) ||
                            (MyCalendarDateTime.AddMinutes(Convert.ToDouble(EncRecord.Duration_Minutes)) >= startTime
                             && MyCalendarDateTime.AddMinutes(Convert.ToDouble(EncRecord.Duration_Minutes)) <= endTime)))
                        {
                            ArrayList aryMsg = new ArrayList();
                            aryMsg.Add(EncRecord.Facility_Name);
                            if (MyFacilityName == EncRecord.Facility_Name)
                            {
                                //ApplicationObject.erroHandler.DisplayErrorMessage("110031", "Edit Appointment", aryMsg, this.Page);
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110031');", true);
                                return false;
                            }
                        }
                    }
                }
            }

            int iCheckCount = 0;
            if (hdnEncounterID.Value.ToString() == "0") //New Appointment
            {
                iCheckCount = 0;
            }
            else
            {
                iCheckCount = 1;
            }

            //if (appointmentList.Count > 0)
            if (appointmentList.Count > iCheckCount)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Previous", "ConfirmPreviousAppointment();", true);
                return false;
                //int d = ApplicationObject.erroHandler.DisplayErrorMessage("110045", this.Text);
                //if (d == 1)
                //{
                //    return true;
                //}
                //else if (d == 2)
                //{
                //    return false;
                //}
            }

            return true;
        }

        private bool checkingAppointmentExistsForDifferentPatient(IList<Encounter> appointmentList)
        {
            Encounter EncRecord = null;
            DateTime MyCalendarDateTime = DateTime.MinValue;
            if (dtpApptDate.SelectedDate.Value != null)
                MyCalendarDateTime = Convert.ToDateTime(dtpApptDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + dtpStartTime.SelectedTime.Value.Hours + ":" + dtpStartTime.SelectedTime.Value.Minutes + ":" + dtpStartTime.SelectedTime.Value.Seconds);
            if (Convert.ToUInt64(hdnEncounterID.Value) != 0 && chkReschedule.Checked == true)
            {
                if (txtReasonCode.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('110048');", true);// ApplicationObject.erroHandler.DisplayErrorMessage("110048", this.Text);
                    return false;
                }
            }
            if (appointmentList.Count > 0)
            {
                for (int i = 0; i < appointmentList.Count; i++)
                {
                    EncRecord = appointmentList[i];
                    TimeSpan ts = new TimeSpan(0, EncRecord.Duration_Minutes, 0);
                    //  DateTime startTime = EncRecord.Appointment_Date;//change it to local
                    //  DateTime endTime = startTime.AddMinutes(Convert.ToDouble(EncRecord.Duration_Minutes));

                    DateTime startTime = UtilityManager.ConvertToLocal(EncRecord.Appointment_Date);
                    DateTime endTime = UtilityManager.ConvertToLocal(EncRecord.Appointment_Date.AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds));
                    if (EncRecord.Id != Convert.ToUInt64(hdnEncounterID.Value))
                    {

                        if ((MyCalendarDateTime >= startTime && MyCalendarDateTime <= endTime) ||
                            (MyCalendarDateTime.AddMinutes(Convert.ToDouble(EncRecord.Duration_Minutes)) >= startTime
                             && MyCalendarDateTime.AddMinutes(Convert.ToDouble(EncRecord.Duration_Minutes)) <= endTime)
                            && hdnCurrentProcess.Value.ToUpper() != "CANCELLED")
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Overwrite", "ConfirmOverwriteAppointment();", true);
                            return false;
                            //int d = ApplicationObject.erroHandler.DisplayErrorMessage("110047", "Edit Appointment", this.Page);
                            //if (d == 1)
                            //{
                            //    return true;
                            //}
                            //else if (d == 2)
                            //{
                            //    return false;
                            //}
                            //return false;
                        }
                    }
                }
            }

            //if (appointmentList.Count > 0)
            //{
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Previous", "ConfirmPreviousAppointment();", true);
            //    return false;
            //    //int d = ApplicationObject.erroHandler.DisplayErrorMessage("110045", this.Text);
            //    //if (d == 1)
            //    //{
            //    //    return true;
            //    //}
            //    //else if (d == 2)
            //    //{
            //    //    return false;
            //    //}
            //}

            return true;
        }

        public Boolean PhNoValid(string sPhno)
        {
            string sReplace = sPhno.Replace("_", "");

            if (sReplace.Length < 10)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ComboBoxColorChange(RadComboBox combobox, Boolean bToNormal)
        {
            if (bToNormal == false)
            {
                //combobox.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
                combobox.Enabled = false;
                //combobox.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);


            }
            else
            {
                combobox.Enabled = true;
                combobox.BackColor = Color.White;
                // combobox.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);

            }
        }

        public void NumericUpDownColorChange(TextBox numUpDown, Boolean bToNormal)
        {
            if (bToNormal == false)
            {
                // numUpDown.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
                numUpDown.Enabled = false;
                numUpDown.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                numUpDown.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                numUpDown.Enabled = true;
                // numUpDown.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
                numUpDown.BackColor = System.Drawing.Color.White;
                numUpDown.ForeColor = System.Drawing.Color.FromArgb(200, 224, 255);
            }
        }

        public void TextBoxColorChange(TextBox txtbox, Boolean bToNormal)
        {
            if (bToNormal == false)
            {
                //txtbox.ReadOnly = true;
                // txtbox.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                txtbox.CssClass = "nonEditabletxtbox";
                txtbox.Attributes.Add("onkeydown", "return false;");
                txtbox.Attributes.Add("onkeypress", "return false;");
            }
            else
            {
                //txtbox.ReadOnly = false;
                // txtbox.BackColor = System.Drawing.Color.White;
                txtbox.CssClass = "Editabletxtbox";
                txtbox.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
                txtbox.Attributes.Add("onkeydown", "return true;");
                //if (txtbox.ID == "txtProviderNPI")
                //    txtProviderNPI.Attributes.Add("onkeypress", "return isNumberKey(this);");
            }
        }

        public void MaskedTextBoxColorChange(RadMaskedTextBox txtbox, Boolean bToNormal)
        {
            if (bToNormal == false)
            {
                //txtbox.ReadOnly = true;
                txtbox.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                txtbox.Attributes.Add("onkeydown", "return false;");
                txtbox.Attributes.Add("onkeypress", "return false;");
            }
            else
            {
                //txtbox.ReadOnly = false;
                txtbox.BackColor = System.Drawing.Color.White;
                txtbox.Attributes.Add("onkeypress", "return EnableSaveButton(this);");
            }
        }

        public void DateTimePickerColorChange(RadDatePicker datetimepicker, Boolean bToNormal)
        {
            if (bToNormal == false)
            {
                //datetimepicker.Font = fonts
                datetimepicker.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                datetimepicker.Enabled = true;
                datetimepicker.ForeColor = Color.Black;
                //Extender.Enabled = false;

            }
            else
            {
                //datetimepicker.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
                datetimepicker.Enabled = false;
                datetimepicker.BackColor = System.Drawing.Color.White;
                // Extender.Enabled = true;
                datetimepicker.ForeColor = System.Drawing.Color.Black;
            }
        }

        public void TimePickerColorChange(RadTimePicker datetimepicker, Boolean bToNormal)
        {
            if (bToNormal == false)
            {
                //datetimepicker.Font = fonts
                datetimepicker.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //datetimepicker.Enabled = false;
                datetimepicker.Enabled = true;
                datetimepicker.ForeColor = Color.Black;

            }
            else
            {
                //datetimepicker.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
                //  datetimepicker.Enabled = true;
                datetimepicker.BackColor = System.Drawing.Color.White;
                datetimepicker.Enabled = false;
                datetimepicker.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void FillReasonCode()
        {
            IList<StaticLookup> fieldlist;
            FieldLookup lookup;
            //Latha - 10 Aug 2011 - Start - AllLookups singleton
            //AllLookups objAllLookups = new AllLookups();            
            fieldlist = staticMngr.getStaticLookupByFieldName("RESCHEDULE REASON CODE", "Sort_Order");
            //Latha - 10 Aug 2011 - End

            ddlReasonCode.Items.Clear();
            if (fieldlist != null)
            {
                for (int i = 0; i < fieldlist.Count; i++)
                {
                    lookup = fieldlist[i];
                    ddlReasonCode.Items.Add(new RadComboBoxItem(lookup.Value));
                }
            }
            ddlReasonCode.Items.Add(new RadComboBoxItem("Other"));
            ddlReasonCode.SelectedIndex = 0;

        }

        //not been used right now 03-02-2016 by vasanth
        //object[] GenerateIsCMGOrderObject()
        //{
        //    string[] str = new string[] { };
        //    if (hdnOrderList.Value != string.Empty)
        //    {
        //        str = hdnOrderList.Value.Split('-').ToArray();
        //    }

        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        if (str[i] != string.Empty)
        //        {
        //            lstOrdersID.Add(str[i]);
        //        }
        //    }
        //    if (hdnHumanID.Value != string.Empty)
        //    {
        //        ulMyHumanID = Convert.ToUInt64(hdnHumanID.Value);
        //    }
        //    if (EncRecord.Id != 0 && chkReschedule.Checked == true)
        //    {
        //        object[] temp = new object[] { "true", ulMyHumanID, string.Join(",", lstOrdersID.ToArray<string>()), EncRecord.Id };
        //        return temp;
        //    }
        //    // not been used right now 12-01-2016 by vasanth
        //    //else if (hdnFacilityName.Value.ToUpper() == System.Configuration.ConfigurationSettings.AppSettings["CMGFacilityName"] && txtTest.Text.Trim() != string.Empty)
        //    //{
        //    //    object[] temp = new object[] { "true", ulMyHumanID, string.Join(",", lstOrdersID.ToArray<string>()) };
        //    //    return temp;
        //    //}
        //    else
        //    {
        //        object[] temp = new object[] { "false" };
        //        return temp;
        //    }
        //}

        public void EnableTableLayout(Panel tablelayout)
        {
            for (int i = 0; i < tablelayout.Controls.Count; i++)
            {
                if (tablelayout.Controls[i].GetType().ToString().Contains("TextBox"))
                {
                    TextBox txtBox = (TextBox)tablelayout.Controls[i];
                    TextBoxColorChange(txtBox, true);
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("RadComboBox"))
                {
                    RadComboBox combobox = (RadComboBox)tablelayout.Controls[i];
                    ComboBoxColorChange(combobox, true);
                }
            }
        }

        public void DisableTableLayout(Panel tablelayout)
        {
            for (int i = 0; i < tablelayout.Controls.Count; i++)
            {
                if (tablelayout.Controls[i].GetType().ToString().Contains("TextBox"))
                {
                    TextBox txtBox = (TextBox)tablelayout.Controls[i];
                    TextBoxColorChange(txtBox, false);
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("RadComboBox"))
                {
                    RadComboBox combobox = (RadComboBox)tablelayout.Controls[i];
                    ComboBoxColorChange(combobox, false);
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("CheckBox"))
                {
                    CheckBox checkbox = (CheckBox)tablelayout.Controls[i];
                    if (checkbox.ID != "chkGuarantorIsPatient")
                    {
                        //                        CheckBoxColorChange(checkbox,);
                    }
                }

            }
            tablelayout.ForeColor = Color.Gray;
        }

        public PhysicianLibrary GetPhysicianDetailsByPhyID(string physician_id)
        {
            //logger.Debug("GetPhysicianDetailsByPhyID method called to retrieve physician details fromk 2 XMLs using Physician_ID");
            PhysicianLibrary objPhyLib = new PhysicianLibrary();
            XmlDocument xmldoc = new XmlDocument();
            //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianFacilityMapping.xml");
            PhysicianFacilityMappingList physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");
            if (physicianFacilityMappingList != null)
            {
                //logger.Debug("Reading PhysicianFacilityMapping.xml");
                //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianFacilityMapping" + ".xml");
                // XmlNode nodeMatchingPhysician = xmldoc.SelectSingleNode("/ROOT/PhyList/Facility/Physician[@ID='" + physician_id + "']");
                var physicianList = physicianFacilityMappingList.PhysicianFacility.Select(x => new { physician = x.Physician.FirstOrDefault(y => y.ID == ClientSession.PhysicianId.ToString()), name = x.name }).FirstOrDefault();

                //if (nodeMatchingPhysician == null)
                if (physicianList?.physician == null)
                {
                    //logger.Debug("XML tag '/ROOT/PhyList/Facility/Physician[@ID='" + physician_id + "']' not found");
                    var unmappedPhysician = physicianFacilityMappingList.UnmappedPhysician.FirstOrDefault(x => x.ID == physician_id.ToString()); //xmldoc.SelectSingleNode("/ROOT/UnMappedPhyList/Physician[@ID='" + physician_id + "']");
                    if (unmappedPhysician != null) 
                    {
                        physicianList.physician.prefix = unmappedPhysician.prefix;
                        physicianList.physician.firstname = unmappedPhysician.firstname;
                        physicianList.physician.middlename = unmappedPhysician.middlename;
                        physicianList.physician.lastname = unmappedPhysician.lastname;
                        physicianList.physician.suffix = unmappedPhysician.suffix;
                        physicianList.physician.username = unmappedPhysician.username;
                        physicianList.physician.ID = unmappedPhysician.ID;
                        physicianList.physician.status = unmappedPhysician.status;
                        physicianList.physician.npi = unmappedPhysician.npi;
                        physicianList.physician.machine_technician_id = unmappedPhysician.machine_technician_id;
                        physicianList.physician.Legal_Org = unmappedPhysician.Legal_Org;
                    }

                    //if (nodeMatchingPhysician != null)
                    //logger.Debug("XML tag '/ROOT/UnMappedPhyList/Physician[@ID='" + physician_id + "']' found");
                    //else
                    //logger.Debug("XML tag '/ROOT/UnMappedPhyList/Physician[@ID='" + physician_id + "']' not found");
                }
                //else
                //logger.Debug("XML tag '/ROOT/PhyList/Facility/Physician[@ID='" + physician_id + "']' found");
                if (physicianList?.physician != null)
                {
                    objPhyLib = new PhysicianLibrary();
                    objPhyLib.PhyPrefix = physicianList.physician.prefix;
                    objPhyLib.PhyFirstName = physicianList.physician.firstname;
                    objPhyLib.PhyMiddleName = physicianList.physician.middlename;
                    objPhyLib.PhyLastName = physicianList.physician.lastname;
                    objPhyLib.PhySuffix = physicianList.physician.suffix;
                    try
                    {
                        objPhyLib.PhyId = Convert.ToUInt32(physicianList.physician.ID);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Physician_ID of value '" + nodeMatchingPhysician.Attributes["ID"].Value.ToString() + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    try
                    {
                        objPhyLib.Id = Convert.ToUInt32(physicianList.physician.ID);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Physician_ID of value '" + nodeMatchingPhysician.Attributes["ID"].Value.ToString() + "' to UInt threw an error.", exp);
                        throw (exp);
                    }
                    objPhyLib.Is_Active = physicianList.physician.status;
                    if (physicianList.name != null)
                        objPhyLib.PhyNotes = physicianList.name;//physician.ParentNode.Attributes["name"].Value.ToString();//used for the purpose of facility in this form alone.
                }
                //XmlDocument xmldoc1 = new XmlDocument();
                //string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
                //if (File.Exists(strXmlFilePath) == true)
                //{
                //    //logger.Debug("Reading PhysicianAddressDetails.xml");
                //    xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                //    XmlNode nodeMatchingPhysicianAddress = xmldoc.SelectSingleNode("/PhysicianAddress/p" + physician_id);
                //    if (nodeMatchingPhysicianAddress != null)
                //    {
                //        objPhyLib.PhyAddress1 = nodeMatchingPhysicianAddress.Attributes["Physician_Address1"].Value.ToString();
                //        objPhyLib.PhyAddress2 = nodeMatchingPhysicianAddress.Attributes["Physician_Address2"].Value.ToString();
                //        objPhyLib.PhyCity = nodeMatchingPhysicianAddress.Attributes["Physician_City"].Value.ToString();
                //        objPhyLib.PhyState = nodeMatchingPhysicianAddress.Attributes["Physician_State"].Value.ToString();
                //        objPhyLib.PhyZip = nodeMatchingPhysicianAddress.Attributes["Physician_Zip"].Value.ToString();
                //        objPhyLib.PhyTelephone = nodeMatchingPhysicianAddress.Attributes["Physician_Telephone"].Value.ToString();
                //        objPhyLib.PhyFax = nodeMatchingPhysicianAddress.Attributes["Physician_Fax"].Value.ToString();
                //        objPhyLib.PhyNPI = nodeMatchingPhysicianAddress.Attributes["Physician_NPI"].Value.ToString();
                //        //logger.Debug("XML tag '/PhysicianAddress/p" + physician_id + "' found");
                //    }
                //    //else
                //    //logger.Debug("XML tag '/PhysicianAddress/p" + physician_id + "' not found");
                //}
                //CAP-2780
                PhysicianAddressDetailsList physicianAddressDetailsList = ConfigureBase<PhysicianAddressDetailsList>.ReadJson("PhysicianAddressDetails.json");

                if (physicianAddressDetailsList != null)
                {
                    var matchingAddress = physicianAddressDetailsList.PhysicianAddress
                                                     .FirstOrDefault(address => address.Physician_Library_ID == physician_id.ToString());

                    if (matchingAddress != null)
                    {
                        objPhyLib.PhyAddress1 = matchingAddress.Physician_Address1;
                        objPhyLib.PhyAddress2 = matchingAddress.Physician_Address2;
                        objPhyLib.PhyCity = matchingAddress.Physician_City;
                        objPhyLib.PhyState = matchingAddress.Physician_State;
                        objPhyLib.PhyZip = matchingAddress.Physician_Zip;
                        objPhyLib.PhyTelephone = matchingAddress.Physician_Telephone;
                        objPhyLib.PhyFax = matchingAddress.Physician_Fax;
                        objPhyLib.PhyNPI = matchingAddress.Physician_NPI;
                    }
                }
            }
                return objPhyLib;
        }
        //BugID:53106
        private int GetPhysicianLibIDByTechID(int MachineTechID)
        {
            int PhyLibID = 0;
            //Jira CAP-2777
            //XmlDocument xmldoc = new XmlDocument();
            //string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
            //if (File.Exists(strXmlFilePathTech) == true)
            {
                //Jira CAP-2777
                //xmldoc = new XmlDocument();
                //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                //XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + MachineTechID.ToString());
                //if (xmlTec != null && xmlTec.Count > 0)
                //    PhyLibID = Convert.ToInt32(xmlTec[0].Attributes.GetNamedItem("Physician_Library_ID").Value.ToString());

                MachinetechnicianList machinetechnicianList = new MachinetechnicianList();
                machinetechnicianList = ConfigureBase<MachinetechnicianList>.ReadJson("machine_technician.json");
                if (machinetechnicianList?.MachineTechnician != null)
                {
                    List<Machinetechnician> machinetechnicians = new List<Machinetechnician>();
                    machinetechnicians = machinetechnicianList.MachineTechnician.Where(x => x.machine_technician_library_id == MachineTechID.ToString()).ToList();
                    if (machinetechnicians.Count > 0)
                    {
                        PhyLibID = Convert.ToInt32(machinetechnicians[0].Physician_Library_ID.ToString());
                    }
                }
            }
            return PhyLibID;
        }

        [WebMethod(EnableSession = true)]
        public static string GetHumanDetails(string humanid)
        {

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            //Cap - 1234
            string PatientStrip = "";

            string sdivPatientstrip = UtilityManager.FillPatientStrip(Convert.ToUInt64(humanid));
            if (sdivPatientstrip != null)
            {
                PatientStrip = sdivPatientstrip;
            }


            //IList<Human> objhuman = new List<Human>();
            ////HumanManager humanMngr = new HumanManager();
            ////objhuman = humanMngr.patientdetails(hdnHumanID.Value);
            //string PatientStrip = "";
            //IList<string> ilstEditAppTagList = new List<string>();
            //ilstEditAppTagList.Add("HumanList");

            //IList<object> ilstEditAppFinal = new List<object>();
            //ilstEditAppFinal = UtilityManager.ReadBlob(Convert.ToUInt64(humanid), ilstEditAppTagList);

            //if (ilstEditAppFinal.Count > 0 && ilstEditAppFinal != null)
            //{
            //    if (ilstEditAppFinal[0] != null)
            //    {
            //        for (int iCount = 0; iCount < ((IList<object>)ilstEditAppFinal[0]).Count; iCount++)
            //        {
            //            objhuman.Add((Human)((IList<object>)ilstEditAppFinal[0])[iCount]);
            //        }
            //    }
            //}

            //if (objhuman != null && objhuman.Count > 0)
            //{

            //    string phoneno = "";


            //    if (objhuman[0].Home_Phone_No.Length == 14)
            //    {
            //        phoneno = objhuman[0].Home_Phone_No;
            //    }
            //    else
            //    {
            //        phoneno = objhuman[0].Cell_Phone_Number;
            //    }

            //    // cache the current time
            //    DateTime now = DateTime.Today; // today is fine, don't need the timestamp from now
            //                                   // get the difference in years
            //    int years = 0;
            //    if (objhuman[0].Birth_Date != null)
            //        years = now.Year - objhuman[0].Birth_Date.Year;
            //    // subtract another year if we're before the
            //    // birth day in the current year
            //    if (now.Month < objhuman[0].Birth_Date.Month || (now.Month == objhuman[0].Birth_Date.Month && now.Day < objhuman[0].Birth_Date.Day))
            //        --years;


            //    string sSex = string.Empty;
            //    if (objhuman[0].Sex != null && objhuman[0].Sex.Trim() != "")
            //        sSex = objhuman[0].Sex.Substring(0, 1);
            //    //Cap - 1234
            //    //     PatientStrip = objhuman[0].Last_Name + "," + objhuman[0].First_Name +
            //    //"  " + objhuman[0].MI + "  " + objhuman[0].Suffix + "   |   " +
            //    // objhuman[0].Birth_Date.ToString("dd-MMM-yyyy") + "   |   " +
            //    //years.ToString() +
            //    //"  year(s)    |   " + sSex + "   |   Acc #:" + objhuman[0].Id +
            //    //"   |   " + "Med Rec #:" + objhuman[0].Medical_Record_Number + "   |   " +
            //    //"Phone #:" + phoneno + "   |   Patient Type:" + objhuman[0].Human_Type;

            //    PatientStrip = objhuman[0].Last_Name + "," + objhuman[0].First_Name +
            //"  " + objhuman[0].MI + "  " + objhuman[0].Suffix + "   |   " +
            // objhuman[0].Birth_Date.ToString("dd-MMM-yyyy") + "   |   " +
            //years.ToString() +
            //"  year(s)    |   " + sSex + "   |   Acc #:" + objhuman[0].Id +
            //"   |   " + "Med Rec #:" + objhuman[0].Medical_Record_Number + "   |   " +
            //"Home Phone #:" + objhuman[0].Home_Phone_No + "   |   Cell Phone #:" + objhuman[0].Cell_Phone_Number +
            //"   |   Patient Type:" + objhuman[0].Human_Type;

            //}
            else
            {
                PatientStrip = " " + "   |" + "|" + "|" + "|" + "|";
            }


            return JsonConvert.SerializeObject(PatientStrip);
        }

        [WebMethod(EnableSession = true)]
        public static string PcpPrimaryDefault(string humanid, string EncounterId)
        {
            string value = string.Empty;
            EncounterManager EncMngr = new EncounterManager();
            IList<Encounter> Encntlist = new List<Encounter>();

            FindPhysican InsuredList = new FindPhysican();
            PatientInsuredPlanManager objPhysicianManager = new PatientInsuredPlanManager();
            InsuredList = objPhysicianManager.FindPhysicianByInsureList(Convert.ToUInt64(humanid));

            Encntlist = EncMngr.GetEncounterByEncounterID(Convert.ToUInt32(EncounterId));
            if (Encntlist.Count > 0)
            {
                if (Encntlist[0].PCP_Physician != string.Empty)
                {
                    value = Encntlist[0].Referring_Physician + "&" + Encntlist[0].PCP_Physician + "| NPI: " + Encntlist[0].PCP_Provider_NPI +
                         "| Facility: " + Encntlist[0].PCP_Facility + "| Address:" + Encntlist[0].PCP_Address +
                         "| Phone No:" + Encntlist[0].PCP_Phone_No + "| Fax No:" + Encntlist[0].PCP_Fax_No;
                }
            }

            else if (InsuredList.PhyList.Count > 0)
            {
                //Jira #CAP-156 - Index was outside bounds 
                //value = InsuredList.PhyList[0].PhyPrefix + " " + InsuredList.PhyList[0].PhyFirstName + " " + InsuredList.PhyList[0].PhyMiddleName + " " + InsuredList.PhyList[0].PhyLastName + "(" + InsuredList.PhyList[0].PhySuffix + ")" + " | " +
                //                                 "NPI:" + InsuredList.PhyList[0].PhyNPI + " | " +
                //                                 "Facility:" + InsuredList.PhyList[0].PhyFacility + " | " +
                //                                 "Address:" + InsuredList.PhyList[0].PhyAddrs + ", " +
                //                                 InsuredList.PhyList[0].PhyCity + "," +
                //                                 InsuredList.PhyList[0].PhyState + " " +
                //                                 InsuredList.PhyList[0].PhyZip + " | " +
                //                                 ((InsuredList.PhyList[0].PhyPhone.Trim()) != "" ? "Phone No:" + InsuredList.PhyList[0].PhyPhone + " | " : "") +
                //                                 (InsuredList.PhyList[0].PhyFax.Trim() != "" ? "Fax No:" + InsuredList.PhyList[0].PhyFax : "");

                value = InsuredList.PhyList[0].PhyId+ "&" +InsuredList.PhyList[0].PhyPrefix + " " + InsuredList.PhyList[0].PhyFirstName + " " + InsuredList.PhyList[0].PhyMiddleName + " " + InsuredList.PhyList[0].PhyLastName + "(" + InsuredList.PhyList[0].PhySuffix + ")" + " | " +
                                                 "NPI:" + InsuredList.PhyList[0].PhyNPI + " | " +
                                                 "Facility:" + InsuredList.PhyList[0].PhyFacility + " | " +
                                                 "Address:" + InsuredList.PhyList[0].PhyAddrs + ", " +
                                                 InsuredList.PhyList[0].PhyCity + "," +
                                                 InsuredList.PhyList[0].PhyState + " " +
                                                 InsuredList.PhyList[0].PhyZip + " | " +
                                                 ((InsuredList.PhyList[0].PhyPhone.Trim()) != "" ? "Phone No:" + InsuredList.PhyList[0].PhyPhone + " | " : "Phone No: | ") +
                                                 (InsuredList.PhyList[0].PhyFax.Trim() != "" ? "Fax No:" + InsuredList.PhyList[0].PhyFax : "Fax No:");


            }
            else
            {
                value = "";

            }
            return JsonConvert.SerializeObject(value);
        }

        protected void DisablePanelControls(Control panel)
        {
            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is WebControl webCtrl)
                {
                    webCtrl.Enabled = false;
                }

                // Recursively disable nested controls
                if (ctrl.HasControls())
                {
                    DisablePanelControls(ctrl);
                }
            }
        }
        #endregion
    }
}
