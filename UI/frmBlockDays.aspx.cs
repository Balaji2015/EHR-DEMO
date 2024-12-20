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
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Drawing;
using Telerik.Web.UI;
using System.Xml;
using System.IO;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI
{
    public partial class frmBlockDays : System.Web.UI.Page
    {
        BlockdaysManager blockDaysMngr = new BlockdaysManager();
        ListView lvw = new ListView();
        ArrayList arrDay = new ArrayList();
        ArrayList BlockArrDay = new ArrayList();
        FillBlockDays fillBlkDayObj = new FillBlockDays();
        Blockdays blockDayObj = new Blockdays();
        DateTime fromDate = DateTime.MinValue, toDate = DateTime.MinValue;
        IList<Blockdays> saveList = new List<Blockdays>();
        IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
        AppointmentLookupManager AppointmentMngr = new AppointmentLookupManager();
        int saveCount = 0;
        ulong GroupID;
        string updtFacName = string.Empty, updtDay = string.Empty, checkedDay = string.Empty;
        ulong updtPhyId = 0;
        DateTime updtFromDt = DateTime.MinValue, updtToDt = DateTime.MinValue;
        ArrayList errList = new ArrayList();
        IList<string> days = new List<string>();
        bool formLoad = false;
        ulong updtGroupID;
        string sMyFacility = string.Empty;
        IList<ulong> ulPhy = new List<ulong>();
        IList<string> lstEditAlternateWeeks = new List<string>();
        IList<string> lstEditAlternateMonths = new List<string>();
        bool bChk = false;
        Boolean bTabChange = false;
        ArrayList TimeList = null;
       // string sAncillary = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Added By Nijanthan for Tab order
            if (Page.IsPostBack)
            {
                WebControl wcICausedPostBack = (WebControl)GetControlThatCausedPostBack(sender as Page);
                if (wcICausedPostBack != null)
                {
                    int indx = wcICausedPostBack.TabIndex;
                    var ctrl = from control in wcICausedPostBack.Parent.Controls.OfType<WebControl>()
                               where control.TabIndex > indx
                               select control;
                    ctrl.DefaultIfEmpty(wcICausedPostBack).First().Focus();
                }


            }
           
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            //---------------------------

            txtRecurringDescription.DName = "pbRecurDropDown";
            txtDescriptionTOF.DName = "pbTOFDropDown";
            txtDescription.DName = "pbNonRecurDropDown";
            string[] Values;
            Values = Request.Form.GetValues("Result");
            hdnGruopId.Value = blockDaysMngr.GetMaxGroupID().ToString();
            //if (hdnIndex.Value != "")
            //{
            //    string s3 = hdnBlockDayType.Value;
            //    string[] sarry = hdnFromTime.Value.Split(':');
            //    int hour = Convert.ToInt32(sarry[0]);
            //    string[] sarryMin = sarry[1].Split(' ');
            //    int Minute = Convert.ToInt32(sarryMin[0]);
            //    string s = sarryMin[1].ToString();
            //    string[] sarryTo = hdnToTime.Value.Split(':');
            //    int hour1 = Convert.ToInt32(sarryTo[0]);
            //    string[] sarryMinTo = sarryTo[1].Split(' ');
            //    int Minute1 = Convert.ToInt32(sarryMinTo[0]);
            //    string s1 = sarryMinTo[1].ToString();
            //    string[] sDay = hdnDays.Value.Split(',');
            //    for (int i = 0; i < sDay.Count(); i++)
            //    {
            //        CheckBox chk = (CheckBox)pnlSelectDate.FindControl("chk" + sDay[i].Trim());
            //        if (chk != null)
            //        {
            //            chk.Checked = true;
            //        }
            //    }

            //    if (hdnPhySelected.Value != string.Empty)
            //    {
            //        CheckBoxList chkboxlst = (CheckBoxList)pnlProvider.FindControl("chklstboxProvider");
            //        if (chkboxlst.Items.Count > 0)
            //        {
            //            for (int i = 0; i < chkboxlst.Items.Count; i++)
            //            {
            //                if (hdnPhySelected.Value == chkboxlst.Items[i].Text)
            //                {
            //                    chkboxlst.Items[i].Selected = true;
            //                }
            //            }
            //        }
            //    }

            //    switch (hdnBlockDayType.Value)
            //    {
            //        case "RECURSIVE":
            //            txtRecurringDescription.txtDLC.Text = hdnRecDescription.Value;
            //            chkRecurSelecttime.Enabled = true;
            //            btnSaveForRecurring.Enabled = true;
            //            btnClearForRecurring.Enabled = true;
            //            btnSaveForRecurring.Text = "Updadte";
            //            btnClearForRecurring.Text = "Cancel";
            //            chkRecurSelecttime.Enabled = true;
            //            chkRecurSelecttime.Checked = true;
            //            if (chkRecurSelecttime.Checked == true)
            //            {
            //                dtpRecurFromTime.Enabled = true;
            //                dtpRecurToTime.Enabled = true;
            //            }
            //            tbblockDays.ActiveTabIndex = 0;
            //            dtpRecurFromDate.SelectedDate = Convert.ToDateTime(hdnFromDate.Value);
            //            dtpRecurToDate.SelectedDate = Convert.ToDateTime(hdnToDate.Value);
            //            DateTime dt = Convert.ToDateTime(hdnFromTime.Value);
            //            dtpRecurFromTime.SelectedTime = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
            //            DateTime dt1 = Convert.ToDateTime(hdnToTime.Value);
            //            dtpRecurToTime.SelectedTime = new TimeSpan(dt1.Hour, dt1.Minute, dt1.Second);

            //            break;
            //        case "NON RECURSIVE":
            //            btnSaveForNonRecur.Enabled = true;
            //            btnCancelForNonRecur.Enabled = true;
            //            btnSaveForNonRecur.Text = "Update";
            //            btnCancelForNonRecur.Text = "Cancel";
            //            chkNonRecurSelectTime.Enabled = true;
            //            chkNonRecurSelectTime.Checked = true;
            //            if (chkNonRecurSelectTime.Checked == true)
            //            {
            //                dtpNonRecurFromTime.Enabled = true;
            //                dtpNonRecurToTime.Enabled = true;
            //            }
            //            dtpNonRecurDate.SelectedDate = Convert.ToDateTime(hdnFromDate.Value);
            //            txtDescription.txtDLC.Text = hdnNonRecurDescription.Value;
            //            tbblockDays.ActiveTabIndex = 1;
            //            DateTime dtNonRec = Convert.ToDateTime(hdnFromTime.Value);
            //            dtpNonRecurFromTime.SelectedTime = new TimeSpan(dtNonRec.Hour, dtNonRec.Minute, dtNonRec.Second);
            //            DateTime dt1NonRec = Convert.ToDateTime(hdnToTime.Value);
            //            dtpNonRecurToTime.SelectedTime = new TimeSpan(dt1NonRec.Hour, dt1NonRec.Minute, dt1NonRec.Second);
            //            break;
            //        default:
            //            txtDescriptionTOF.txtDLC.Text = hdnRecDescription.Value;
            //            //btnSaveTOF.Text = "Update";
            //            btnSaveTOF.Enabled = true;
            //            tbblockDays.ActiveTabIndex = 2;
            //            DateTime dtTOV = Convert.ToDateTime(hdnFromTime.Value);
            //            dtpFromTimeTOF.SelectedTime = new TimeSpan(dtTOV.Hour, dtTOV.Minute, dtTOV.Second);
            //            //dtpFromTimeTOF.Hour = hour;
            //            //dtpFromTimeTOF.Minute = Minute;
            //            //if (s.ToUpper() == "PM")
            //            //{
            //            //    dtpFromTimeTOF.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            //            //}
            //            //else
            //            //{
            //            //    dtpFromTimeTOF.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            //            //}
            //            DateTime dtTOV1 = Convert.ToDateTime(hdnFromTime.Value);
            //            dtpToTimeTOF.SelectedTime = new TimeSpan(dtTOV1.Hour, dtTOV1.Minute, dtTOV1.Second);
            //            //dtpToTimeTOF.Hour = hour1;
            //            //dtpToTimeTOF.Minute = Minute1;
            //            //if (s1.ToUpper() == "PM")
            //            //{
            //            //    dtpToTimeTOF.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            //            //}
            //            //else
            //            //{
            //            //    dtpToTimeTOF.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            //            //}
            //            break;


            //    }
            //    hdnIndex.Value = "";

            //}


            if (!IsPostBack)
            {

                hdnMessageType.Value = string.Empty;
                //rpvBlockRecurring.Attributes.Add("OnClientClick ", "BlockdaysRecurAutoSave(BlockRecur); return false;");
                //rpvBlockNonRecurring.Attributes.Add("OnClientClick ", "BlockdaysRecurAutoSave(BlockTOV); return false;");
                //tbblockNonRecurring.Attributes.Add("OnClientClick ", "BlockdaysRecurAutoSave(BlockNonRecur); return false;");
                txtRecurringDescription.txtDLC.Attributes.Add("onchange", "EnableSave();");
                txtDescriptionTOF.txtDLC.Attributes.Add("onchange", "EnableSave();");
                txtDescription.txtDLC.Attributes.Add("onchange", "EnableSave();");
                txtRecurringDescription.txtDLC.Attributes.Add("onkeypress", "EnableSave();");
                txtDescriptionTOF.txtDLC.Attributes.Add("onkeypress", "EnableSave();");
                txtDescription.txtDLC.Attributes.Add("onkeypress", "EnableSave();");
                hdnPhyID.Value = ClientSession.PhysicianId.ToString();
                hdnUserName.Value = ClientSession.UserName;
                this.Page.Title = "Block Days" + "-" + ClientSession.UserName;
                btnSaveForRecurring.Text = "Save";
                btnSaveForNonRecur.Text = "Save";
                btnClearForRecurring.Text = "Clear All";
                btnCancelForNonRecur.Text = "Clear All";
                if (ClientSession.LocalDate != string.Empty)
                {
                    //srividhya
                    //hdnLocalTime.Value = Convert.ToDateTime(Session["LocalDate"] + " " + Session["LocalDateAndTime"]).ToString();
                    hdnLocalTime.Value = Convert.ToDateTime(ClientSession.LocalDate + " " + ClientSession.LocalTime).ToString();
                }
                hdnSelectedIndex.Value = "Plus";
                hdnNonRecurDescription.Value = "Plus";

                if (hdnLocalTime.Value != "")
                {
                    //dtpRecurFromDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    //dtpNonRecurDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    //dtpRecurToDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    //dtpFromdateTOF.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    //dtpTodateTOF.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);


                    dtpRecurFromDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    dtpNonRecurDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    dtpRecurToDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    dtpFromdateTOF.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    dtpTodateTOF.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);

                    DateTime dttime = new DateTime();
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        dttime = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                    string sHour = string.Empty;
                    string sHour1 = string.Empty;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        sHour = string.Format("{0:HH:mm:ss tt}", Convert.ToDateTime(hdnLocalTime.Value).AddHours(1).ToShortTimeString());
                        sHour1 = string.Format("{0:HH:mm:ss tt}", Convert.ToDateTime(hdnLocalTime.Value).ToShortTimeString());
                    }

                    //if (MKB.TimePicker.TimeSelector.AmPmSpec.AM.ToString() == Convert.ToDateTime(sHour1).ToString("tt"))
                    //{
                    //    //dtpRecurFromTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                    //    //dtpNonRecurFromTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                    //    //dtpFromTimeTOF.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                    //}
                    //else
                    //{
                    //    //dtpRecurFromTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                    //    //dtpNonRecurFromTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                    //    //dtpFromTimeTOF.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                    //}
                    //if (MKB.TimePicker.TimeSelector.AmPmSpec.AM.ToString() == Convert.ToDateTime(sHour).ToString("tt"))
                    //{
                    //    //dtpRecurToTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                    //    //dtpNonRecurToTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                    //    //dtpToTimeTOF.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                    //}
                    //else
                    //{
                    //    //dtpRecurToTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                    //    //dtpNonRecurToTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                    //    //dtpToTimeTOF.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                    //}

                    //if ((Convert.ToDateTime(hdnLocalTime.Value).Hour > 12 || Convert.ToDateTime(hdnLocalTime.Value).Hour > 12) && Convert.ToDateTime(hdnLocalTime.Value).Minute > 01)
                    //{
                    //    //dtpNonRecurFromTime.Hour = Convert.ToDateTime(hdnLocalTime.Value).Hour - 12;
                    //    //dtpNonRecurFromTime.Minute = Convert.ToDateTime(hdnLocalTime.Value).Minute;
                    //    // dtpRecurFromTime.Hour = Convert.ToDateTime(hdnLocalTime.Value).Hour - 12;
                    //    //dtpRecurFromTime.Minute = Convert.ToDateTime(hdnLocalTime.Value).Minute;
                    //    //dtpFromTimeTOF.Hour = Convert.ToDateTime(hdnLocalTime.Value).Hour - 12;
                    //    //dtpFromTimeTOF.Minute = Convert.ToDateTime(hdnLocalTime.Value).Minute;
                    //}
                    //else
                    //{
                    //    //dtpNonRecurFromTime.Hour = Convert.ToDateTime(hdnLocalTime.Value).Hour;
                    //    //dtpNonRecurFromTime.Minute = Convert.ToDateTime(hdnLocalTime.Value).Minute;
                    //    //dtpRecurFromTime.Hour = Convert.ToDateTime(hdnLocalTime.Value).Hour;
                    //    //dtpRecurFromTime.Minute = Convert.ToDateTime(hdnLocalTime.Value).Minute;
                    //    //dtpFromTimeTOF.Hour = Convert.ToDateTime(hdnLocalTime.Value).Hour;
                    //    //dtpFromTimeTOF.Minute = Convert.ToDateTime(hdnLocalTime.Value).Minute;
                    //}

                    //if ((Convert.ToDateTime(hdnLocalTime.Value).AddHours(1).Hour > 12 || Convert.ToDateTime(hdnLocalTime.Value).AddHours(1).Hour > 12) && Convert.ToDateTime(hdnLocalTime.Value).AddHours(1).Minute > 01)
                    //{
                    //    //dtpRecurToTime.Minute = Convert.ToDateTime(hdnLocalTime.Value).Minute;
                    //    //dtpRecurToTime.Hour = Convert.ToDateTime(hdnLocalTime.Value).AddHours(1).Hour - 12;
                    //    //dtpNonRecurToTime.Hour = Convert.ToDateTime(hdnLocalTime.Value).AddHours(1).Hour - 12;
                    //    //dtpNonRecurToTime.Minute = Convert.ToDateTime(hdnLocalTime.Value).Minute;
                    //    //dtpToTimeTOF.Hour = Convert.ToDateTime(hdnLocalTime.Value).AddHours(1).Hour - 12;
                    //    //dtpToTimeTOF.Minute = Convert.ToDateTime(hdnLocalTime.Value).Minute;
                    //}
                    //else
                    //{
                    //    //dtpRecurToTime.Minute = Convert.ToDateTime(hdnLocalTime.Value).Minute;
                    //    //dtpRecurToTime.Hour = Convert.ToDateTime(hdnLocalTime.Value).AddHours(1).Hour;
                    //    //dtpNonRecurToTime.Hour = Convert.ToDateTime(hdnLocalTime.Value).AddHours(1).Hour;
                    //    //dtpNonRecurToTime.Minute = Convert.ToDateTime(hdnLocalTime.Value).Minute;
                    //    //dtpToTimeTOF.Hour = Convert.ToDateTime(hdnLocalTime.Value).AddHours(1).Hour;
                    //    //dtpToTimeTOF.Minute = Convert.ToDateTime(hdnLocalTime.Value).Minute;
                    //}


                    chkRecurSelecttime.Checked = true;
                    if (chkRecurSelecttime.Checked == true)
                    {
                        dtpRecurFromTime.Enabled = true;
                        dtpRecurToTime.Enabled = true;
                    }
                    chkNonRecurSelectTime.Checked = true;
                    if (chkNonRecurSelectTime.Checked == true)
                    {
                        dtpNonRecurFromTime.Enabled = true;
                        dtpNonRecurToTime.Enabled = true;
                    }
                    chktypeofvisitSelectTime.Checked = true;
                    if (chktypeofvisitSelectTime.Checked == true)
                    {
                        dtpFromTimeTOF.Enabled = true;
                        dtpToTimeTOF.Enabled = true;
                    }
                    btnClearForRecurring.Enabled = true;
                    btnCancelForNonRecur.Enabled = true;
                    string sMacAddress = string.Empty;

                    //this.dtpRecurFromTime.Size = new Size(233, 22);
                    //this.dtpRecurToTime.Size = new Size(233, 22);
                    //this.dtpNonRecurFromTime.Size = new Size(233, 22);
                    //this.dtpNonRecurToTime.Size = new Size(233, 22);
                    //hdnGruopId.Value = blockDaysMngr.GetMaxGroupID().ToString();
                    //ToolTip objToolTip = new ToolTip();
                    //objToolTip.SetToolTip(pbNonRecurClear, "Clear");
                    //objToolTip.SetToolTip(pbRecurClear, "Clear");
                    //objToolTip.SetToolTip(pbNonRecurDB, "Library");
                    //objToolTip.SetToolTip(pbRecurDB, "Library");
                    //objToolTip.SetToolTip(pbNonRecurDropDown, "DropDown");
                    //objToolTip.SetToolTip(pbRecurDropDown, "DropDown");
                    //Checkin for null value added by Janani on 2/8/10

                    string sFacName = string.Empty;
                    if (Request.QueryString["FacilityName"] != null)
                    {
                        sFacName = Request.QueryString["FacilityName"].ToString().Replace("_", "#");
                    }
                    else
                    {
                        sFacName = ClientSession.FacilityName;
                    }
                    FillLogin login = new FillLogin();
                    //login.Facility_Library_List = ApplicationObject.facilityLibraryList;//Codereview FacilityMngr.GetFacilityList();
                    var fac = from f in ApplicationObject.facilityLibraryList where f.Legal_Org == ClientSession.LegalOrg select f;
                    login.Facility_Library_List = fac.ToList<FacilityLibrary>();
                    if (login.Facility_Library_List.Count > 0)
                    {
                        for (int i = 0; i < login.Facility_Library_List.Count; i++)
                        {
                            RadComboBoxItem item = new RadComboBoxItem();
                            item.Value = i.ToString();
                            item.Text = login.Facility_Library_List[i].Fac_Name.ToString();
                            ddlFacilityName.Items.Add(item);
                            if (sFacName == login.Facility_Library_List[i].Fac_Name.ToString())
                            {
                                //ddlFacilityName.SelectedItem.Text = login.Facility_Library_List[i].Fac_Name.ToString();
                                ddlFacilityName.SelectedIndex = i;
                            }


                        }
                    }
                    ddlFacilityName.Text = ClientSession.FacilityName;
                    RadComboBoxSelectedIndexChangedEventArgs e1 = new RadComboBoxSelectedIndexChangedEventArgs(string.Empty, string.Empty, string.Empty, string.Empty);
                    ddlFacilityName_SelectedIndexChanged(sender, e1);
                    lblId.Text = "0";
                    //txtRecurringDescription.TextChanged += new EventHandler(txtRecurringDescription_TextChanged);
                    //txtDescription.TextChanged += new EventHandler(txtDescription_TextChanged);

                    ddlFacilityName.Focus();
                    hdnRecDescription.Value = "False";
                    SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                    objSecurity.ApplyUserPermissions(this.Page);
                    btnSaveForRecurring.Enabled = false;
                    btnSaveForNonRecur.Enabled = false;
                    btnSaveTOF.Enabled = false;
                    //btnClearForRecurring.Enabled = false;
                    //btnCancelForNonRecur.Enabled = false;
                    btnClearTOF.Enabled = false;
                    //dtpRecurFromTime.Enabled = false;
                    //dtpRecurToTime.Enabled = false;
                    //dtpNonRecurFromTime.Enabled = false;
                    //dtpNonRecurToTime.Enabled = false;
                    //dtpFromTimeTOF.Enabled = false;
                    //dtpToTimeTOF.Enabled = false;
                    dtpRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                    dtpRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                    dtpNonRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                    dtpNonRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                    dtpFromTimeTOF.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                    dtpToTimeTOF.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                }

                if (ClientSession.UserRole.ToUpper() == "FRONT OFFICE")
                {
                    dtpRecurFromDate.Enabled = false;
                    dtpRecurToDate.Enabled = false;
                    dtpNonRecurDate.Enabled = false;
                    dtpFromdateTOF.Enabled = false;
                    dtpTodateTOF.Enabled = false;
                    txtRecurringDescription.txtDLC.Enabled = false;
                    txtDescription.txtDLC.Enabled = false;
                    //txtDescriptionTOF.txtDLC.Enabled = false;
                    cboTypeOfVisit.Enabled = false;
                    btnClearTOF.Enabled = false;
                    ddlFacilityName.Enabled = false;
                    dtpRecurFromTime.Enabled = false;
                    dtpRecurToTime.Enabled = false;
                    dtpNonRecurFromTime.Enabled = false;
                    dtpNonRecurToTime.Enabled = false;
                    dtpToTimeTOF.Enabled = false;
                    dtpFromTimeTOF.Enabled = false;
                    pnlBlockDays.Enabled = false;
                    btnSaveForRecurring.Enabled = false;
                    btnSaveForNonRecur.Enabled = false;
                    btnSaveTOF.Enabled = false;
                    btnClearForRecurring.Enabled = false;
                    btnCancelForNonRecur.Enabled = false;
                    btnClearTOF.Enabled = false;
                }

            }

        }
        private void Page_PreRender(object sender, System.EventArgs e)
        {
            txtRecurringDescription.txtDLC.Enabled = true;
            txtDescription.txtDLC.Enabled = true;
            hdnForMedicalAssistant.Value = "false";

            //if (ClientSession.UserRole.Trim().ToUpper() == "MEDICAL ASSISTANT")
            //{
            //    hdnForMedicalAssistant.Value = "MEDICAL ASSISTANT";
            //    txtDescription.txtDLC.Attributes.Add("disabled", "disabled");
            //    txtRecurringDescription.txtDLC.Attributes.Add("disabled", "disabled");
            //}
            if ((hdnToFindSource.Value == "ToEnable") && (tabBlockDays.SelectedIndex == 0))
            {
                btnSaveForRecurring.Enabled = true;
            }
            else
            {
                btnSaveForRecurring.Enabled = false;
            }
            if ((hdnToFindSource.Value == "ToEnableNon") && (tabBlockDays.SelectedIndex == 1))
            {
                btnSaveForNonRecur.Enabled = true;
            }
            else
            {
                btnSaveForNonRecur.Enabled = false;
            }
            if (btnSaveForRecurring.Text == "Update")
            {
                btnClearForRecurring.Text = "Cancel";
            }
            if (btnSaveForNonRecur.Text == "Update")
            {
                btnCancelForNonRecur.Text = "Cancel";
            }
        }
        //protected void ddlFacilityName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    PhysicianList = new List<PhysicianLibrary>();

        //    PhysicianManager physicianMngr = new PhysicianManager();
        //    PhysicianList = physicianMngr.GetPhysicianListbyFacility(ddlFacilityName.SelectedItem.ToString(), "Y");
        //    chklstboxProvider.Items.Clear();
        //    //Checkin for null value added by Gopal on 2/8/10
        //    if (PhysicianList != null)
        //    {
        //        if (PhysicianList.Count > 0)
        //        {
        //            string[] sPhysicianChecked = null;
        //            if (Request.QueryString["PhysicianSelected"] != null)
        //            {
        //                sPhysicianChecked = Request.QueryString["PhysicianSelected"].ToString().Split('|');
        //            }


        //            for (int i = 0; i < PhysicianList.Count; i++)
        //            {
        //                chklstboxProvider.Items.Add(PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix);
        //                chklstboxProvider.Items[i].Value = PhysicianList[i].Id.ToString();
        //                string sPhyName = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
        //                if (sPhysicianChecked != null)
        //                {
        //                    if (sPhysicianChecked.Contains(sPhyName.TrimEnd()))
        //                    {
        //                        chklstboxProvider.Items[i].Selected = true;
        //                    }
        //                }
        //            }
        //            //TO check the phy what we have selected in the appointments
        //            //string[] sIndex = Request["PhysicianSelectedIndex"].ToString().Split(',');
        //            //int iIndex = sIndex.Count();
        //            //for (int j = 0; j < iIndex; j++)
        //            //{
        //            //    chklstboxProvider.Items[Convert.ToInt32(sIndex[j])].Selected = true;
        //            //}

        //        }
        //    }
        //}

        private void LoadNotes(string temp, ListBox lstDescription)
        {
            IList<UserLookup> fieldlist = null;
            UserLookupManager userLookUpMngr = new UserLookupManager();
            fieldlist = userLookUpMngr.GetFieldLookupList(temp);
            lstDescription.Items.Clear();
            if (fieldlist != null)
            {
                for (int j = 0; j < fieldlist.Count; j++)
                {
                    lstDescription.Items.Add(fieldlist[j].Value);
                }
            }
        }
        //Added By Nijanthan
        protected Control GetControlThatCausedPostBack(Page page)
        {
            Control control = null;

            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    Control c = page.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button || c is System.Web.UI.WebControls.ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;

        }

        //----------------------
        private void cleartxt()
        {
            DateTime dttime = DateTime.Now;
            //DateTime dttime = Convert.ToDateTime(hdnLocalTime.Value);
            //string sHour = string.Format("{0:HH:mm:ss tt}", Convert.ToDateTime(hdnLocalTime.Value).AddHours(1).ToShortTimeString());
            //string sHour1 = string.Format("{0:HH:mm:ss tt}", Convert.ToDateTime(hdnLocalTime.Value).ToShortTimeString());
            lblId.Text = "0";
            if (tabBlockDays.SelectedIndex == 0)
            {
                rdmpBlockDays.PageViews[0].Selected = true;
                btnSaveForRecurring.Text = "Save";
                btnClearForRecurring.Text = "Clear All";
                if (hdnLocalTime.Value != string.Empty)
                {
                    dtpRecurFromDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    dtpRecurToDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                }


                //dtpRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                //dtpRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                dtpRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                dtpRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                //chkRecurSelecttime.Checked = true;
                dtpRecurFromTime.Enabled = true;
                dtpRecurToTime.Enabled = true;
                chkRecurSelecttime.Checked = false;
                chkMonday.Checked = false;
                chkTuesday.Checked = false;
                chkWednesday.Checked = false;
                chkThursday.Checked = false;
                chkFriday.Checked = false;
                chkSaturday.Checked = true;
                chkSunday.Checked = true;
                chkAlternateWeeks.Checked = false;
                chkAlternateMonths.Checked = false;
                txtRecurringDescription.txtDLC.Text = string.Empty;
                btnSaveForRecurring.Enabled = false;
                btnClearForRecurring.Enabled = true;
                //ddlFacilityName.SelectedIndex = 0;
                hdnBlockDayType.Value = string.Empty;
                hdnBlockdaysId.Value = string.Empty;
                hdnNonRecBlockDaysId.Value = string.Empty;
                chkRecurSelecttime.Checked = true;
                hdnSaveForDlc.Value = "false";
            }
            else if (tabBlockDays.SelectedIndex == 1)
            {
                rdmpBlockDays.PageViews[1].Selected = true;
                btnSaveForNonRecur.Text = "Save";
                btnCancelForNonRecur.Text = "Clear All";
                dtpNonRecurDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                //dtpNonRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                //dtpNonRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                dtpRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                dtpRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                dtpNonRecurFromTime.Enabled = true;
                dtpNonRecurToTime.Enabled = true;
                txtDescription.txtDLC.Text = string.Empty;
                btnSaveForNonRecur.Enabled = false;
                btnCancelForNonRecur.Enabled = true;
                chkNonRecurSelectTime.Checked = false;
                hdnBlockDayType.Value = string.Empty;
                hdnNonRecBlockDaysId.Value = string.Empty;
                hdnBlockdaysId.Value = string.Empty;
                chkNonRecurSelectTime.Checked = true;
                hdnSaveForDlc.Value = "false";
            }
            else if (tabBlockDays.SelectedIndex == 2)
            {
                btnSaveTOF.Text = "Save";
                btnClearTOF.Text = "Clear All";
                if (hdnLocalTime.Value != string.Empty)
                {
                    dtpFromdateTOF.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    dtpTodateTOF.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                }

                dtpFromTimeTOF.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                dtpToTimeTOF.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                dtpFromdateTOF.Enabled = false;
                dtpToTimeTOF.Enabled = false;
                chktypeofvisitSelectTime.Checked = true;
                chkMondayTOF.Checked = false;
                chkTuesdayTOF.Checked = false;
                chkWednesdayTOF.Checked = false;
                chkThursdayTOF.Checked = false;
                chkFridayTOF.Checked = true;
                chkSaturdayTOF.Checked = true;
                chkSundayTOF.Checked = false;
                txtDescriptionTOF.txtDLC.Text = string.Empty;
                btnSaveTOF.Enabled = false;
                btnClearTOF.Enabled = false;
                ddlFacilityName.SelectedIndex = 0;
                cboTypeOfVisit.SelectedIndex = 0;
                chktypeofvisitSelectTime.Checked = true;
                hdnSaveForDlc.Value = "false";
            }
        }
        public void AddBlockDays()
        {
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ddlFacilityName.SelectedItem.Text select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
               
            List<string> lstDays = new List<string>();
            DateTime DateInDB;
            string sDatesForBlock = string.Empty;
            string YesRecur = hdnMessageType.Value;
            string YesForTabClick = hdnForTabClick.Value;
            hdnForTabClick.Value = string.Empty;
            hdnMessageType.Value = string.Empty;
            IList<Blockdays> UpdateList = new List<Blockdays>();
            IList<Blockdays> deletelist = new List<Blockdays>();
            IList<DateTime> lstAlternateWeeks = new List<DateTime>();
            IList<DateTime> lstAlternateMonths = new List<DateTime>();
            //if (chkAlternateMonths.Checked)
            //{
            //    int iAlterWeeks = fromDate.Date.;
            //    for (int m = fromDate.Date; m <= toDate.Month; m++)
            //    {
            //        if (m == iAlterMonth)
            //        {
            //            lstAlternateMonths.Add(m.ToString());
            //            iAlterMonth = fromDate.Month + 2;
            //        }
            //    }
            //}
            
            saveList.Clear();
            saveCount = 0;
            arrDay.Clear();
            days.Clear();
            bChk = false;
            if (btnSaveForRecurring.Text == "Save")
            {
                hdnBlockdaysId.Value = string.Empty;
            }
            if (chklstboxProvider.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110111'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                hdnToFindSource.Value = "ToEnable";
                btnSaveForRecurring.Enabled = true;
                hdnSaveForDlc.Value = "true";
                bChk = true;
                return;
            }
            if (chklstboxProvider.SelectedItem == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110114'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                btnSaveForRecurring.Enabled = true;
                hdnToFindSource.Value = "ToEnable";
                hdnSaveForDlc.Value = "true";
                bChk = true;
                return;
            }
            //if (chkRecurSelecttime.Checked == false)
            //{
            //    ScriptManager.RegisterStartupScript(this,this.GetType (), "BlockDays", "DisplayErrorMessage('110205');", true);
            //    bChk = true;
            //    return;
            //}
            //Get the Checked Days into an arraylist
            if (chkMonday.Checked == true)
            {
                arrDay.Add(chkMonday.Text);
                lstDays.Add(chkMonday.Text);
            }
            if (chkTuesday.Checked == true)
            {
                arrDay.Add(chkTuesday.Text);
                lstDays.Add(chkTuesday.Text);
            }
            if (chkWednesday.Checked == true)
            {
                arrDay.Add(chkWednesday.Text);
                lstDays.Add(chkWednesday.Text);
            }
            if (chkThursday.Checked == true)
            {
                arrDay.Add(chkThursday.Text);
                lstDays.Add(chkThursday.Text);
            }
            if (chkFriday.Checked == true)
            {
                arrDay.Add(chkFriday.Text);
                lstDays.Add(chkFriday.Text);
            }
            if (chkSaturday.Checked == true)
            {
                arrDay.Add(chkSaturday.Text);
                lstDays.Add(chkSaturday.Text);
            }
            if (chkSunday.Checked == true)
            {
                arrDay.Add(chkSunday.Text);
                lstDays.Add(chkSunday.Text);
            }
            if ((dtpRecurFromDate.SelectedDate.Value) == DateTime.MinValue)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110121'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                btnSaveForRecurring.Enabled = false;
                hdnSaveForDlc.Value = "false";
                hdnToFindSource.Value = "ToEnable";
                dtpRecurFromDate.Focus();
                bChk = true;
                return;
            }
            else if ((dtpRecurToDate.SelectedDate.Value) == DateTime.MinValue)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110122'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                dtpRecurToDate.Focus();
                btnSaveForRecurring.Enabled = false;
                hdnToFindSource.Value = "ToEnable";
                hdnSaveForDlc.Value = "false";
                bChk = true;
                return;
            }

            if ((Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value)) < UtilityManager.ConvertToLocal(Convert.ToDateTime(hdnLocalTime.Value)).Date && lblId.Text == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110101'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                btnSaveForRecurring.Enabled = false;
                hdnSaveForDlc.Value = "false";
                hdnToFindSource.Value = "ToEnable";
                dtpRecurFromDate.Focus();
                bChk = true;
                return;
            }
            //Checks whether From Date and To Date are same or not
            else if (Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value) == Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110102'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                dtpRecurToDate.Focus();
                btnSaveForRecurring.Enabled = false;
                hdnToFindSource.Value = "ToEnable";
                hdnSaveForDlc.Value = "false";
                bChk = true;
                return;
            }

            //Checks Whether the To Date occurs before the From Date
            else if (UtilityManager.ConvertToLocal(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value)) < UtilityManager.ConvertToLocal(Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value)))
            {
                // ApplicationObject.erroHandler.DisplayErrorMessage("110108", "BlockDays", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110108'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                dtpRecurToDate.Focus();
                btnSaveForRecurring.Enabled = false;
                hdnToFindSource.Value = "ToEnable";
                hdnSaveForDlc.Value = "false";
                bChk = true;
                return;
            }

            //Checks atleast a single Day is checked
            else if (arrDay.Count == 0)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110103", "BlockDays", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110103'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                btnSaveForRecurring.Enabled = false;
                hdnSaveForDlc.Value = "false";
                hdnToFindSource.Value = "ToEnable";
                bChk = true;
                return;
            }

            if (txtRecurringDescription.txtDLC.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110115'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                txtRecurringDescription.Focus();
                btnSaveForRecurring.Enabled = false;
                hdnToFindSource.Value = "ToEnable";
                hdnSaveForDlc.Value = "false";
                bChk = true;
                return;
            }
            if (dtpRecurFromTime.SelectedTime == null)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "BlockDays", "DisplayErrorMessage('110127'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            if (dtpRecurToTime.SelectedTime == null)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "BlockDays", "DisplayErrorMessage('110128'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            if (dtpRecurFromTime.SelectedTime.Value > dtpRecurToTime.SelectedTime.Value)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110118", "BlockDays", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110118'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                btnSaveForRecurring.Enabled = false;
                hdnSaveForDlc.Value = "false";
                hdnToFindSource.Value = "ToEnable";
                bChk = true;
                dtpRecurFromTime.Focus();
                return;
            }

            if (dtpRecurFromTime.SelectedTime.Value == dtpRecurToTime.SelectedTime.Value)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110119", "BlockDays", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110119'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                btnSaveForRecurring.Enabled = false;
                hdnSaveForDlc.Value = "false";
                hdnToFindSource.Value = "ToEnable";
                bChk = true;
                dtpRecurToTime.Focus();
                return;
            }
            if (chkAlternateWeeks.Checked)
            {
                DateTime fromDate1 = dtpRecurFromDate.SelectedDate.Value;
                DateTime toDate1 = dtpRecurToDate.SelectedDate.Value;
                //DateTime iAlterWeeks = fromDate1.Date;
                ArrayList arrAlternateWeeks = arrDay;
                int icount = 0;
                while (fromDate1.Date <= toDate1.Date)
                {
                    for (int a = 0; a < arrAlternateWeeks.Count; a++)
                    {
                        if (icount < arrAlternateWeeks.Count)
                        {
                            if (arrAlternateWeeks[a].ToString() == fromDate1.DayOfWeek.ToString())
                            {
                                lstAlternateWeeks.Add(UtilityManager.ConvertToUniversal(fromDate1));
                                DateTime iAlterWeeks = fromDate1.Date.AddDays(14);
                                while (iAlterWeeks.Date <= toDate1.Date)
                                {
                                    lstAlternateWeeks.Add(UtilityManager.ConvertToUniversal(iAlterWeeks));
                                    iAlterWeeks = iAlterWeeks.Date.AddDays(14);
                                }
                                icount = icount + 1;
                            }
                        }

                    }
                    fromDate1 = fromDate1.AddDays(1);
                }
            }
            if (chkAlternateMonths.Checked)
            {

                DateTime fromDate1 = dtpRecurFromDate.SelectedDate.Value;
                DateTime toDate1 = dtpRecurToDate.SelectedDate.Value;
                ArrayList arrAlternateMonths = arrDay;
                int icount = 0;
                int icurrentmonth = fromDate1.Month;
                Boolean mismatchmonth = false;
                DateTime iAltestartrMonth = fromDate1.Date;
                while (iAltestartrMonth.Date <= toDate1.Date)
                {
                    int iSelectedMonth = iAltestartrMonth.Month;
                    if (mismatchmonth == true)
                    {
                        int iyear=0;
                        if (iSelectedMonth < 12){
                            iyear= iAltestartrMonth.Year;
                            icurrentmonth = iSelectedMonth + 1;
                        }
                        else if (iSelectedMonth == 12){
                            icurrentmonth = 1;
                           iyear= iAltestartrMonth.Year+1;
                        }
                        mismatchmonth = false;
                        iAltestartrMonth = new DateTime(iyear, icurrentmonth, 1);
                        iSelectedMonth = iAltestartrMonth.Month;
                    }
                    for (int a = 0; a < arrAlternateMonths.Count; a++)
                    {
                        if (icurrentmonth == iSelectedMonth)
                        {
                            //if (icount < arrAlternateMonths.Count)
                            {
                                if (arrAlternateMonths[a].ToString() == iAltestartrMonth.DayOfWeek.ToString())
                                {
                                    lstAlternateMonths.Add(UtilityManager.ConvertToUniversal(iAltestartrMonth));
                                }
                            }
                        }
                        else
                            mismatchmonth = true;
                    }
                    iAltestartrMonth = iAltestartrMonth.AddDays(1);
                }



                //
                //while (fromDate1.Date <= toDate1.Date)
                //{
                //    for (int a = 0; a < arrAlternateMonths.Count; a++)
                //    {
                //        if (icount < arrAlternateMonths.Count)
                //        {
                //            if (arrAlternateMonths[a].ToString() == fromDate1.DayOfWeek.ToString())
                //            {
                //                lstAlternateMonths.Add(UtilityManager.ConvertToUniversal(fromDate1));
                //                int iCurrentMonth = fromDate1.Month;
                //                DateTime dtcurrentMonth = fromDate1.AddDays(7);
                //                while (dtcurrentMonth <= toDate1.Date)
                //                {
                //                    int iMonth=dtcurrentMonth.Month;
                //                    if (iCurrentMonth == iMonth)
                //                    {
                //                        if (arrAlternateMonths[a].ToString() == dtcurrentMonth.DayOfWeek.ToString())
                //                        {
                //                            lstAlternateMonths.Add(UtilityManager.ConvertToUniversal(dtcurrentMonth));
                //                            dtcurrentMonth = dtcurrentMonth.AddDays(7);
                //                        }
                //                    }
                //                    else
                //                        break;
                //                }
                //                //DateTime iAlterMoths = fromDate1.Date.AddMonths(2);
                //                DateTime iAlterMoths = fromDate1.Date.AddMonths(2);
                //                iAlterMoths = new DateTime(iAlterMoths.Year,iAlterMoths.Month,1);
                                
                //                while (iAlterMoths <= toDate1.Date)
                //                {
                //                    //for (int b = 0; b < arrAlternateMonths.Count; b++)
                //                    //{
                //                    //    if (icount < arrAlternateMonths.Count)
                //                    //    {
                //                    if (arrAlternateMonths[a].ToString() == iAlterMoths.DayOfWeek.ToString())
                //                    {
                //                        lstAlternateMonths.Add(UtilityManager.ConvertToUniversal(iAlterMoths));
                //                        int iAlternateMonth = iAlterMoths.Month;
                //                        DateTime dtAlternateMonth = iAlterMoths.AddDays(7);
                //                        while (dtAlternateMonth <= toDate1.Date)
                //                        {
                //                            int iMonth = dtAlternateMonth.Month;
                //                            if (iAlternateMonth == iMonth)
                //                            {
                //                                if (arrAlternateMonths[a].ToString() == dtAlternateMonth.DayOfWeek.ToString())
                //                                {
                //                                    lstAlternateMonths.Add(UtilityManager.ConvertToUniversal(dtAlternateMonth));
                //                                    dtAlternateMonth = dtAlternateMonth.AddDays(7);
                //                                }
                //                            }
                //                            else
                //                                break;
                //                        }

                //                        iAlterMoths = iAlterMoths.Date.AddMonths(2);
                //                        iAlterMoths = new DateTime(iAlterMoths.Year, iAlterMoths.Month, 1);

                //                    }
                //                    //    }
                //                    //}
                //                    iAlterMoths = iAlterMoths.AddDays(1);

                //                }
                //                icount = icount + 1;
                //            }
                //        }

                //    }
                //    fromDate1 = fromDate1.AddDays(1);
                //}
            }
            ulong UGroupID = Convert.ToUInt32(hdnGruopId.Value);
            for (int p = 0; p < chklstboxProvider.Items.Count; p++)
            {
                if (chklstboxProvider.Items[p].Selected == true)
                {
                    UGroupID=UGroupID + 1;
                    ListItem item = chklstboxProvider.SelectedItem as ListItem;
                    if (hdnBlockdaysId.Value != string.Empty)
                    {
                        string[] blockdaysId = new string[] { };
                        ArrayList ArryDysListUpdate = new ArrayList();
                        ArrayList ArrayDate = new ArrayList();
                        ArrayList BlockArryDate = new ArrayList();
                        IList<Blockdays> BlockDayList = new List<Blockdays>();
                        if (hdnBlockdaysId.Value != string.Empty)
                        {
                            blockdaysId = hdnBlockdaysId.Value.Split(',');
                        }

                        BlockDayList = blockDaysMngr.GetBlockDaysDetByBlockID(blockdaysId);
                        DateInDB = BlockDayList[0].To_Date_Choosen;
                        fromDate = Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value);
                        toDate = Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value);
                        while (fromDate.Date <= toDate.Date)
                        {
                            if (lstDays.Contains(fromDate.DayOfWeek.ToString())) //Added if condtion for unwanted added list
                            {
                                ArrayDate.Add(fromDate.ToString());
                                ArryDysListUpdate.Add(fromDate.DayOfWeek.ToString());
                            }
                            fromDate = fromDate.AddDays(1);

                        }

                        List<string> listdays1 = new List<string>();

                        if (arrDay.Count > 0 && ArryDysListUpdate.Count > 0)
                        {
                            var Days = (from string m in arrDay
                                        where !ArryDysListUpdate.Contains(m)
                                        select m).ToArray();
                            listdays1 = Days.ToList<string>();
                        }


                        if (listdays1.Count > 0)
                        {
                            string sDays = string.Empty;
                            for (int iNumber = 0; iNumber < listdays1.Count; iNumber++)
                            {
                                if (sDays == string.Empty)
                                {
                                    sDays = listdays1[iNumber];
                                }
                            }
                            sDays = sDays.TrimEnd(',');
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110109','','" + sDays + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            btnSaveForRecurring.Enabled = false;
                            hdnSaveForDlc.Value = "false";
                            hdnToFindSource.Value = "ToEnable";
                            return;
                        }

                        if (BlockDayList.Count > 0)
                        {
                            for (int i = 0; i < BlockDayList.Count; i++)
                            {
                                BlockArryDate.Add(BlockDayList[i].Block_Date.ToString());
                                BlockArrDay.Add(BlockDayList[i].Day_Choosen);
                            }



                            var GetAdditionalDays = (from string m in ArrayDate
                                                     where !BlockArryDate.Contains(m)
                                                     select m).ToArray();

                            // arrDay.Union(BlockArrDay.Except(List1.Intersect(List2));
                            var ResultQuery1 = (from string num in arrDay
                                                from string num1 in BlockArrDay
                                                where num == num1
                                                select num).ToArray();
                            string resultDay = "";

                            for (int i = 0; i < ResultQuery1.Count(); i++)
                            {

                                resultDay += ResultQuery1[i] + ",";
                            }
                            //arrDay.Ex

                            var GetSameDays = (from string m in ArrayDate
                                               where BlockArryDate.Contains(m)
                                               select m).ToArray();



                            var eq = (from string m in arrDay
                                      where !BlockArrDay.Contains(m)
                                      select m).ToArray();
                            lstDays = eq.ToList<string>();
                            //var ResultQuery2 = (from string num in arrDay
                            //                   join string num1 in BlockArrDay on num equals num1
                            //                   select num).ToArray();
                            //var ResultQuery2 = (from string num in arrDay
                            //                    from string num1 in BlockArrDay
                            //                    where num != num1
                            //                    select num).ToArray();
                            //var GetCheckedDays = (from string m in arrDay
                            //                      where !BlockArrDay.Contains(m)
                            //                      select m).ToArray();

                            // List<string> AdditionalDay = new List<string>();
                            List<string> AdditonalDays = new List<string>();

                            //string FromTime1 = dtpRecurFromTime.SelectedTime.ToString();
                            //string[] SFromTime1 = FromTime1.Split(':');
                            //string TotalMins1 = SFromTime1[0] + ":" + SFromTime1[1];
                            for (int k = 0; k < GetAdditionalDays.Count(); k++)
                            {
                                //DateTime AdditinalDate1 = Convert.ToDateTime(GetAdditionalDays[k]).AddMinutes(Convert.ToDateTime(TotalMins1).TimeOfDay.TotalMinutes);
                                //DateTime AdditinalDate = UtilityManager.ConvertToLocal(AdditinalDate1).Date;
                                //DateTime AdditinalDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(GetAdditionalDays[k])).Date;
                                DateTime AdditinalDate = Convert.ToDateTime(GetAdditionalDays[k]).Date;
                                string AddDay = AdditinalDate.DayOfWeek.ToString();
                                if (arrDay.Contains(AddDay))
                                {

                                    AdditonalDays.Add(AdditinalDate.Date.ToString());
                                }

                                //AdditionalDay.Add(AdditinalDate.DayOfWeek.ToString());
                            }
                            List<string> GetUpdateDays = new List<string>();
                            for (int k = 0; k < GetSameDays.Count(); k++)
                            {
                                DateTime AdditinalDate1 = (Convert.ToDateTime(GetSameDays[k])).Date;
                                string AddDay1 = AdditinalDate1.DayOfWeek.ToString();
                                if (arrDay.Contains(AddDay1))
                                {

                                    GetUpdateDays.Add(AdditinalDate1.Date.ToString());
                                }

                                //AdditionalDay.Add(AdditinalDate.DayOfWeek.ToString());
                            }

                            var GetDaysForDelete = (from string m in BlockArryDate
                                                    where !GetUpdateDays.Contains(m)
                                                    select m).ToArray();



                            if (AdditonalDays.Count() > 0)
                            {
                                for (int j = 0; j < AdditonalDays.Count(); j++)
                                {
                                    //while (fromDate.Date <= toDate.Date)
                                    //{
                                    //    //for (int a = 0; a < arrDay.Count; a++)
                                    //{
                                    //if (eq[j].ToString() == fromDate.DayOfWeek.ToString())
                                    //{
                                    //lstDays.Remove(fromDate.DayOfWeek.ToString());
                                    blockDayObj = new Blockdays();
                                    //if (sAncillary != string.Empty && sAncillary == ddlFacilityName.SelectedItem.Text.Trim())
                                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                                    {
                                        blockDayObj.Machine_Technician_Library_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                                    }
                                    else { 
                                    blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                                    }
                                    blockDayObj.Facility_Name = ddlFacilityName.SelectedItem.Text;
                                    blockDayObj.Block_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(Convert.ToDateTime(AdditonalDays[j]).ToString("yyyy-MM-dd")));
                                    //blockDayObj.From_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                                    blockDayObj.From_Date_Choosen = UtilityManager.ConvertToUniversal(Convert.ToDateTime((Convert.ToDateTime(AdditonalDays[j]).ToString("yyyy-MM-dd"))));//Convert.ToDateTime(Convert.ToDateTime(GetAdditionalDays[j]).ToString("yyyy-MM-dd"));
                                    blockDayObj.To_Date_Choosen = UtilityManager.ConvertToUniversal(Convert.ToDateTime(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm")));
                                    blockDayObj.Reason = txtRecurringDescription.txtDLC.Text;
                                    blockDayObj.Day_Choosen = Convert.ToDateTime(AdditonalDays[j]).DayOfWeek.ToString();// eq[j].ToString();
                                    blockDayObj.Block_Type = "RECURSIVE";
                                    if (chkRecurSelecttime.Checked == true)
                                    {
                                        //int Hour = 0;
                                        //int Hour1 = 0;
                                        string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                                        string[] SFromTime = FromTime.Split(':');
                                        blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                                        string ToTime = dtpRecurToTime.SelectedTime.ToString();
                                        string[] SToTime = ToTime.Split(':');
                                        blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];

                                    }
                                    else
                                    {
                                        //int Hour = 0;
                                        //int Hour1 = 0;
                                        string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                                        string[] SFromTime = FromTime.Split(':');
                                        blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                                        string ToTime = dtpRecurToTime.SelectedTime.ToString();
                                        string[] SToTime = ToTime.Split(':');
                                        blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];
                                        //TimeList = blockDaysMngr.GetStartAndEndTimeFromFacilityLibrary(ddlFacilityName.SelectedItem.Text);
                                        ////blockDayObj.From_Time = "00:01";
                                        ////blockDayObj.To_Time = "23:59";
                                        //blockDayObj.From_Time = TimeList[0].ToString();
                                        //blockDayObj.To_Time = TimeList[1].ToString();
                                    }
                                    if (chkAlternateWeeks.Checked)
                                        blockDayObj.Is_Alternate_Weeks = "Y";
                                    else
                                        blockDayObj.Is_Alternate_Weeks = "N";
                                    if (chkAlternateMonths.Checked)
                                        blockDayObj.Is_Alternate_Months = "Y";
                                    else
                                        blockDayObj.Is_Alternate_Months = "N";
                                    blockDayObj.Created_By = ClientSession.UserName;
                                    blockDayObj.Created_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value)).Date;
                                    blockDayObj.Blockdays_Group_ID = BlockDayList[0].Blockdays_Group_ID;
                                    sDatesForBlock = sDatesForBlock + fromDate.Date.ToString() + ",";
                                    saveList.Add(blockDayObj);
                                    //}
                                    ////}
                                    //fromDate = fromDate.AddDays(1);
                                    //}

                                }
                            }

                            //srividhya commented
                            //if (eq.Count() > 0)
                            //{
                            //    for (int j = 0; j < eq.Count(); j++)
                            //    {
                            //        while (fromDate.Date <= toDate.Date)
                            //        {
                            //            //for (int a = 0; a < arrDay.Count; a++)
                            //            //{
                            //            if (eq[j].ToString() == fromDate.DayOfWeek.ToString())
                            //            {
                            //                lstDays.Remove(fromDate.DayOfWeek.ToString());
                            //                blockDayObj = new Blockdays();
                            //                blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                            //                blockDayObj.Facility_Name = ddlFacilityName.SelectedItem.Text;
                            //                blockDayObj.Block_Date = fromDate;
                            //                //blockDayObj.From_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                            //                blockDayObj.From_Date_Choosen = fromDate.Date;
                            //                blockDayObj.To_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                            //                blockDayObj.Reason = txtRecurringDescription.txtDLC.Text;
                            //                blockDayObj.Day_Choosen = eq[j].ToString();
                            //                blockDayObj.Block_Type = "RECURSIVE";
                            //                if (chkRecurSelecttime.Checked == true)
                            //                {
                            //                    int Hour = 0;
                            //                    int Hour1 = 0;
                            //                    string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                            //                    string[] SFromTime = FromTime.Split(':');
                            //                    blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                            //                    string ToTime = dtpRecurToTime.SelectedTime.ToString();
                            //                    string[] SToTime = ToTime.Split(':');
                            //                    blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];

                            //                }
                            //                else
                            //                {
                            //                    int Hour = 0;
                            //                    int Hour1 = 0;
                            //                    string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                            //                    string[] SFromTime = FromTime.Split(':');
                            //                    blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                            //                    string ToTime = dtpRecurToTime.SelectedTime.ToString();
                            //                    string[] SToTime = ToTime.Split(':');
                            //                    blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];
                            //                    //TimeList = blockDaysMngr.GetStartAndEndTimeFromFacilityLibrary(ddlFacilityName.SelectedItem.Text);
                            //                    ////blockDayObj.From_Time = "00:01";
                            //                    ////blockDayObj.To_Time = "23:59";
                            //                    //blockDayObj.From_Time = TimeList[0].ToString();
                            //                    //blockDayObj.To_Time = TimeList[1].ToString();
                            //                }
                            //                blockDayObj.Created_By = ClientSession.UserName;
                            //                blockDayObj.Created_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value));
                            //                blockDayObj.Blockdays_Group_ID = BlockDayList[0].Blockdays_Group_ID;
                            //                sDatesForBlock = sDatesForBlock + fromDate.Date.ToString() + ",";
                            //                saveList.Add(blockDayObj);
                            //            }
                            //            //}
                            //            fromDate = fromDate.AddDays(1);
                            //        }

                            //    }
                            //    if (lstDays.Count > 0)
                            //    {
                            //        string sDays = string.Empty;
                            //        for (int iNumber = 0; iNumber < lstDays.Count; iNumber++)
                            //        {
                            //            if (sDays == string.Empty)
                            //            {
                            //                sDays = lstDays[iNumber];
                            //            }
                            //        }
                            //        sDays = sDays.TrimEnd(',');
                            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "SaveUnsuccessful();DisplayErrorMessage('110109','','" + sDays + "');", true);
                            //        btnSaveForRecurring.Enabled = false;
                            //        hdnSaveForDlc.Value = "false";
                            //        hdnToFindSource.Value = "ToEnable";
                            //        return;
                            //    }
                            //}
                            //srividhya commented

                            fromDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value)).Date;
                            for (int i = 0; i < BlockDayList.Count; i++)
                            {
                                //BlockDayList= BlockDayList.Where(u => u.Day_Choosen == ResultQuery1[u]).ToList<Blockdays>();

                                //if (resultDay.Contains(BlockDayList[i].Day_Choosen))
                                if (GetUpdateDays.Contains(BlockDayList[i].Block_Date.ToString()) == true)
                                {
                                    //while (fromDate.Date <= toDate.Date)
                                    //{
                                    //    for (int a = 0; a < arrDay.Count; a++)
                                    //    {
                                    //        if (arrDay[a].ToString() == fromDate.DayOfWeek.ToString())
                                    //        {
                                    blockDayObj = new Blockdays();
                                    blockDayObj = BlockDayList[i];
                                    //blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                                   // if (sAncillary != string.Empty && sAncillary == ddlFacilityName.SelectedItem.Text.Trim())
                                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                                    {
                                        blockDayObj.Machine_Technician_Library_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                                    }
                                    else
                                    {
                                        blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                                    }
                                    blockDayObj.Facility_Name = ddlFacilityName.SelectedItem.Text;
                                    //blockDayObj.Block_Date = fromDate;
                                    //blockDayObj.From_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                                    //blockDayObj.From_Date_Choosen = fromDate.Date;
                                    blockDayObj.To_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                                    blockDayObj.Reason = txtRecurringDescription.txtDLC.Text;
                                    //blockDayObj.Day_Choosen = arrDay[a].ToString();
                                    //blockDayObj.Block_Date = fromDate;
                                    blockDayObj.Block_Type = "RECURSIVE";
                                    if (chkRecurSelecttime.Checked == true)
                                    {
                                        //int Hour = 0;
                                        //int Hour1 = 0;
                                        string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                                        string[] SFromTime = FromTime.Split(':');
                                        blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                                        string ToTime = dtpRecurToTime.SelectedTime.ToString();
                                        string[] SToTime = ToTime.Split(':');
                                        blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];
                                        //blockDayObj.From_Date_Choosen = fromDate.Date;
                                        blockDayObj.To_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                                    }
                                    else
                                    {
                                        //int Hour = 0;
                                        //int Hour1 = 0;
                                        string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                                        string[] SFromTime = FromTime.Split(':');
                                        blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                                        string ToTime = dtpRecurToTime.SelectedTime.ToString();
                                        string[] SToTime = ToTime.Split(':');
                                        blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];
                                        //blockDayObj.From_Date_Choosen = fromDate.Date;
                                        blockDayObj.To_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                                        //TimeList = blockDaysMngr.GetStartAndEndTimeFromFacilityLibrary(ddlFacilityName.SelectedItem.Text);
                                        ////blockDayObj.From_Time = "00:01";
                                        ////blockDayObj.To_Time = "23:59";
                                        //blockDayObj.From_Time = TimeList[0].ToString();
                                        //blockDayObj.To_Time = TimeList[1].ToString();
                                    }
                                    if (chkAlternateWeeks.Checked)
                                        blockDayObj.Is_Alternate_Weeks = "Y";
                                    else
                                        blockDayObj.Is_Alternate_Weeks = "N";
                                    if (chkAlternateMonths.Checked)
                                        blockDayObj.Is_Alternate_Months = "Y";
                                    else
                                        blockDayObj.Is_Alternate_Months = "N";
                                    blockDayObj.Modified_By = ClientSession.UserName;
                                    blockDayObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value));
                                    UpdateList.Add(blockDayObj);
                                    //        }
                                    //    }
                                    //    fromDate = fromDate.AddDays(1);
                                    //}
                                }

                            }

                            //if (GetUpdateDays.Count > 0)
                            //{
                            //    for (int i = 0; i < BlockDayList.Count; i++)
                            //    {
                            //        if (Convert. == BlockDayList[i].Block_Date)
                            //        {
                            //            blockDayObj = new Blockdays();
                            //            blockDayObj = BlockDayList[i];
                            //            blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                            //            blockDayObj.Facility_Name = ddlFacilityName.SelectedItem.Text;
                            //            //blockDayObj.Block_Date = fromDate;
                            //            //blockDayObj.From_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                            //            //blockDayObj.From_Date_Choosen = fromDate.Date;
                            //            blockDayObj.To_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                            //            blockDayObj.Reason = txtRecurringDescription.txtDLC.Text;
                            //            //blockDayObj.Day_Choosen = arrDay[a].ToString();
                            //            //blockDayObj.Block_Date = fromDate;
                            //            blockDayObj.Block_Type = "RECURSIVE";
                            //            if (chkRecurSelecttime.Checked == true)
                            //            {
                            //                int Hour = 0;
                            //                int Hour1 = 0;
                            //                string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                            //                string[] SFromTime = FromTime.Split(':');
                            //                blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                            //                string ToTime = dtpRecurToTime.SelectedTime.ToString();
                            //                string[] SToTime = ToTime.Split(':');
                            //                blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];
                            //                //blockDayObj.From_Date_Choosen = fromDate.Date;
                            //                blockDayObj.To_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                            //            }
                            //            else
                            //            {
                            //                int Hour = 0;
                            //                int Hour1 = 0;
                            //                string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                            //                string[] SFromTime = FromTime.Split(':');
                            //                blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                            //                string ToTime = dtpRecurToTime.SelectedTime.ToString();
                            //                string[] SToTime = ToTime.Split(':');
                            //                blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];
                            //                //blockDayObj.From_Date_Choosen = fromDate.Date;
                            //                blockDayObj.To_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                            //                //TimeList = blockDaysMngr.GetStartAndEndTimeFromFacilityLibrary(ddlFacilityName.SelectedItem.Text);
                            //                ////blockDayObj.From_Time = "00:01";
                            //                ////blockDayObj.To_Time = "23:59";
                            //                //blockDayObj.From_Time = TimeList[0].ToString();
                            //                //blockDayObj.To_Time = TimeList[1].ToString();
                            //            }
                            //            blockDayObj.Modified_By = ClientSession.UserName;
                            //            blockDayObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value));
                            //            UpdateList.Add(blockDayObj);
                            //        }
                            //    }

                            //}

                            ArrayList arylstDeleteDays = new ArrayList();
                            for (int i = 0; i < GetDaysForDelete.Count(); i++)
                            {
                                arylstDeleteDays.Add(GetDaysForDelete[i].ToString());
                            }

                            if (GetDaysForDelete.Count() > 0)
                            {

                                blockDayObj = new Blockdays();
                                for (int i = 0; i < BlockDayList.Count(); i++)
                                {
                                    if (arylstDeleteDays.Contains(BlockDayList[i].Block_Date.ToString()) == true)
                                    {
                                        blockDayObj = BlockDayList[i];
                                        deletelist.Add(blockDayObj);
                                    }
                                }

                            }

                            //if (GetDaysForDelete.Count() > 0)
                            //{
                            //    List<string> DeleteDaysList = new List<string>();
                            //    blockDayObj = new Blockdays();
                            //    for (int a = 0; a < GetDaysForDelete.Count(); a++)
                            //    {
                            //        blockDayObj = GetDaysForDelete[a];
                            //    }
                            //}
                            //srividhya commented
                            //fromDate = Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value);
                            //if ((DateInDB) < (Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value)))
                            //{
                            //    for (int iNumber = 0; iNumber < arrDay.Count; iNumber++)
                            //    {
                            //        if (arrDay[iNumber].ToString() == fromDate.DayOfWeek.ToString())
                            //        {
                            //            if (saveList.Count > 0)
                            //            {
                            //                if (!sDatesForBlock.Contains(fromDate.Date.ToString()))
                            //                {
                            //                    blockDayObj = new Blockdays();
                            //                    blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                            //                    blockDayObj.Facility_Name = ddlFacilityName.SelectedItem.Text;
                            //                    blockDayObj.Block_Date = fromDate;
                            //                    //blockDayObj.From_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                            //                    blockDayObj.From_Date_Choosen = fromDate.Date;
                            //                    blockDayObj.To_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                            //                    blockDayObj.Reason = txtRecurringDescription.txtDLC.Text;
                            //                    blockDayObj.Day_Choosen = arrDay[iNumber].ToString();
                            //                    blockDayObj.Block_Type = "RECURSIVE";
                            //                    if (chkRecurSelecttime.Checked == true)
                            //                    {
                            //                        int Hour = 0;
                            //                        int Hour1 = 0;
                            //                        string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                            //                        string[] SFromTime = FromTime.Split(':');
                            //                        blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                            //                        string ToTime = dtpRecurToTime.SelectedTime.ToString();
                            //                        string[] SToTime = ToTime.Split(':');
                            //                        blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];

                            //                    }
                            //                    else
                            //                    {
                            //                        int Hour = 0;
                            //                        int Hour1 = 0;
                            //                        string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                            //                        string[] SFromTime = FromTime.Split(':');
                            //                        blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                            //                        string ToTime = dtpRecurToTime.SelectedTime.ToString();
                            //                        string[] SToTime = ToTime.Split(':');
                            //                        blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];
                            //                    }
                            //                    blockDayObj.Created_By = ClientSession.UserName;
                            //                    blockDayObj.Created_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value));
                            //                    blockDayObj.Blockdays_Group_ID = BlockDayList[0].Blockdays_Group_ID;
                            //                    sDatesForBlock = sDatesForBlock + fromDate.Date.ToString() + ",";
                            //                    saveList.Add(blockDayObj);
                            //                }
                            //            }
                            //            else
                            //            {
                            //                blockDayObj = new Blockdays();
                            //                blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                            //                blockDayObj.Facility_Name = ddlFacilityName.SelectedItem.Text;
                            //                blockDayObj.Block_Date = fromDate;
                            //                blockDayObj.From_Date_Choosen = fromDate.Date;
                            //                blockDayObj.To_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                            //                blockDayObj.Reason = txtRecurringDescription.txtDLC.Text;
                            //                blockDayObj.Day_Choosen = arrDay[iNumber].ToString();
                            //                blockDayObj.Block_Type = "RECURSIVE";
                            //                if (chkRecurSelecttime.Checked == true)
                            //                {
                            //                    int Hour = 0;
                            //                    int Hour1 = 0;
                            //                    string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                            //                    string[] SFromTime = FromTime.Split(':');
                            //                    blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                            //                    string ToTime = dtpRecurToTime.SelectedTime.ToString();
                            //                    string[] SToTime = ToTime.Split(':');
                            //                    blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];

                            //                }
                            //                else
                            //                {
                            //                    int Hour = 0;
                            //                    int Hour1 = 0;
                            //                    string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                            //                    string[] SFromTime = FromTime.Split(':');
                            //                    blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                            //                    string ToTime = dtpRecurToTime.SelectedTime.ToString();
                            //                    string[] SToTime = ToTime.Split(':');
                            //                    blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];
                            //                }
                            //                blockDayObj.Created_By = ClientSession.UserName;
                            //                blockDayObj.Created_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value));
                            //                blockDayObj.Blockdays_Group_ID = BlockDayList[0].Blockdays_Group_ID;
                            //                sDatesForBlock = sDatesForBlock + fromDate.Date.ToString() + ",";
                            //                saveList.Add(blockDayObj);
                            //            }
                            //        }
                            //    }
                            //}
                            //srividhya commented
                        }

                        //}
                        //}

                        //        }
                        //    }
                        //    fromDate = fromDate.AddDays(1);
                        //}


                        //    }
                        //}
                        //fromDate = fromDate.AddDays(1);

                        //Checkin for null value added by Janani on 2/8/10
                        //if (saveList != null)
                        //{
                        //    if (saveList.Count > 0)
                        //    {
                        //        days = (from d in saveList select d.Day_Choosen).ToList<string>();
                        //    }
                        //}

                        //for (int i = 0; i < arrDay.Count; i++)
                        //{
                        //    if (!days.Contains(arrDay[i].ToString()))
                        //    {
                        //        errList = new ArrayList();
                        //        errList.Add(arrDay[i]);
                        //        string stxt = errList[0].ToString();
                        //        //ApplicationObject.erroHandler.DisplayErrorMessage("110109", "BlockDays",errList, this.Page);
                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110109','','" + stxt + "');", true);
                        //        btnSaveForRecurring.Enabled = true;
                        //        bChk = true;
                        //        return;
                        //    }
                        //}

                    }
                    else
                    {
                        string FromTime1 = dtpRecurFromTime.SelectedTime.ToString();
                        string[] SFromTime1 = FromTime1.Split(':');
                        string TotalMins = SFromTime1[0] + ":" + SFromTime1[1];
                        //fromDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value).AddMinutes(Convert.ToDateTime(TotalMins).TimeOfDay.Minutes)).Date;
                        //toDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value).AddMinutes(Convert.ToDateTime(TotalMins).TimeOfDay.Minutes)).Date;

                        //fromDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value)).Date;
                       // toDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value)).Date;

                      
                        fromDate = dtpRecurFromDate.SelectedDate.Value;
                        toDate = dtpRecurToDate.SelectedDate.Value;
                        while (fromDate.Date <= toDate.Date)
                        {
                            for (int a = 0; a < arrDay.Count; a++)
                            {

                                if (arrDay[a].ToString() == fromDate.DayOfWeek.ToString())
                                {
                                    lstDays.Remove(fromDate.DayOfWeek.ToString());
                                    blockDayObj = new Blockdays();
                                    //blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                                   // if (sAncillary != string.Empty && sAncillary == ddlFacilityName.SelectedItem.Text.Trim())
                                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                                    {
                                        blockDayObj.Machine_Technician_Library_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                                    }
                                    else
                                    {
                                        blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                                    }
                                    blockDayObj.Facility_Name = ddlFacilityName.SelectedItem.Text;
                                    //blockDayObj.Block_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value).AddMinutes(Convert.ToDateTime(TotalMins).TimeOfDay.Minutes));
                                    blockDayObj.Block_Date = UtilityManager.ConvertToUniversal(fromDate);
                                    //blockDayObj.From_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurFromDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                                    blockDayObj.From_Date_Choosen = fromDate.Date;
                                    blockDayObj.To_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpRecurToDate.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                                    blockDayObj.Reason = txtRecurringDescription.txtDLC.Text;
                                    blockDayObj.Day_Choosen = arrDay[a].ToString();
                                    blockDayObj.Block_Type = "RECURSIVE";
                                    if (chkRecurSelecttime.Checked == true)
                                    {
                                        //int Hour = 0;
                                        //int Hour1 = 0;
                                        string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                                        string[] SFromTime = FromTime.Split(':');
                                        blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                                        string ToTime = dtpRecurToTime.SelectedTime.ToString();
                                        string[] SToTime = ToTime.Split(':');
                                        blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];

                                    }
                                    else
                                    {
                                        //int Hour = 0;
                                        //int Hour1 = 0;
                                        string FromTime = dtpRecurFromTime.SelectedTime.ToString();
                                        string[] SFromTime = FromTime.Split(':');
                                        blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                                        string ToTime = dtpRecurToTime.SelectedTime.ToString();
                                        string[] SToTime = ToTime.Split(':');
                                        blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];
                                        //TimeList = blockDaysMngr.GetStartAndEndTimeFromFacilityLibrary(ddlFacilityName.SelectedItem.Text);
                                        //blockDayObj.From_Time = TimeList[0].ToString();
                                        //blockDayObj.To_Time = TimeList[1].ToString();
                                    }
                                    if (chkAlternateWeeks.Checked)
                                        blockDayObj.Is_Alternate_Weeks = "Y";
                                    else
                                        blockDayObj.Is_Alternate_Weeks = "N";
                                    if (chkAlternateMonths.Checked)
                                        blockDayObj.Is_Alternate_Months = "Y";
                                    else
                                        blockDayObj.Is_Alternate_Months = "N";
                                    blockDayObj.Created_By = ClientSession.UserName;
                                    blockDayObj.Created_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value)).Date;
                                    //blockDayObj.Blockdays_Group_ID = Convert.ToUInt32(hdnGruopId.Value) + 1;
                                    blockDayObj.Blockdays_Group_ID = UGroupID;
                                    saveList.Add(blockDayObj);
                                    //}
                                }
                            }
                            fromDate = fromDate.AddDays(1);
                        }
                        # region viji
                        if (lstDays.Count > 0)
                        {
                            string sDays = string.Empty;
                            for (int iNumber = 0; iNumber < lstDays.Count; iNumber++)
                            {
                                //if (sDays == string.Empty)
                                //{
                                //    sDays = lstDays[iNumber];
                                //}
                                if (iNumber == 0)
                                {
                                    sDays = lstDays[iNumber];
                                }
                                else
                                {
                                    sDays += " , " + lstDays[iNumber];
                                }

                            }
                            sDays = sDays.TrimEnd(',');
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110109','','" + sDays + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            btnSaveForRecurring.Enabled = false;
                            hdnSaveForDlc.Value = "false";
                            hdnToFindSource.Value = "ToEnable";
                            return;
                        }
#endregion
                    }

                }

            }
            if(lstAlternateWeeks.Count>0)
            {
                saveList = saveList.Where(aa => lstAlternateWeeks.Contains(aa.Block_Date)).ToList();
                //UpdateList = UpdateList.Where(aa => lstAlternateWeeks.Contains(aa.Block_Date)).ToList();
            }
            if (lstAlternateMonths.Count > 0)
            {
                saveList = saveList.Where(aa => lstAlternateMonths.Contains(aa.Block_Date)).ToList();
                //UpdateList = UpdateList.Where(aa => lstAlternateMonths.Contains(aa.Block_Date)).ToList();
            }
            if (btnSaveForRecurring.Text == "Save")
            {
                if (saveList != null)
                {
                    if (saveList.Count > 0)
                    {
                        if (Convert.ToUInt64(lblId.Text) == 0)
                        {
                            GroupID = blockDaysMngr.InsertBlockDaysList(saveList.ToArray<Blockdays>(), "");
                            saveCount++;
                            //ApplicationObject.erroHandler.DisplayErrorMessage("110106", "BlockDays", this.Page);
                            if ((YesRecur == "Yes") && (YesForTabClick == ""))
                            {
                                hdnToFindSource.Value = "";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "var TabClick = top.window.document.getElementById('ctl00_hdnTabClick'); var which_tab = TabClick.value.split('$#$')[0]; var screen_name; if (which_tab != 'first'){ screen_name = 'BlockDaysTabClick'; } SavedSuccessfully_NowProceed(screen_name);DisplayErrorMessage('110106');GetRadNewWindow(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            }
                            else
                            {
                                hdnToFindSource.Value = "";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "var TabClick = top.window.document.getElementById('ctl00_hdnTabClick'); var which_tab = TabClick.value.split('$#$')[0]; var screen_name; if (which_tab != 'first'){ screen_name = 'BlockDaysTabClick'; } SavedSuccessfully_NowProceed(screen_name);DisplayErrorMessage('110106'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            }
                        }
                        else
                        {
                            if (saveList[0].Physician_ID!=0)
                              GroupID = blockDaysMngr.UpdateRecursiveBlockdays(updtGroupID, saveList[0].Physician_ID, saveList.ToArray<Blockdays>(), "");
                            else
                                GroupID = blockDaysMngr.UpdateRecursiveBlockdaysUsingTechID(updtGroupID, saveList[0].Machine_Technician_Library_ID, saveList.ToArray<Blockdays>(), "");
                            //blkDayProxy.DeleteUsingFromAndToDates(updtFacName, updtPhyId, "RECURSIVE", updtFromDt, updtToDt, updtDay, saveList.ToArray<Blockdays>());
                            // blkDayProxy.InsertBlockDaysList(saveList.ToArray<Blockdays>());
                            saveCount++;
                            //ApplicationObject.erroHandler.DisplayErrorMessage("110107", "BlockDays", this.Page);
                            if ((YesRecur == "Yes") && (YesForTabClick == ""))
                            {
                                hdnToFindSource.Value = "";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "var TabClick = top.window.document.getElementById('ctl00_hdnTabClick'); var which_tab = TabClick.value.split('$#$')[0]; var screen_name; if (which_tab != 'first'){ screen_name = 'BlockDaysTabClick'; } SavedSuccessfully_NowProceed(screen_name);DisplayErrorMessage('110106');GetRadNewWindow(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            }
                            else
                            {
                                hdnToFindSource.Value = "";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "var TabClick = top.window.document.getElementById('ctl00_hdnTabClick'); var which_tab = TabClick.value.split('$#$')[0]; var screen_name; if (which_tab != 'first'){ screen_name = 'BlockDaysTabClick'; } SavedSuccessfully_NowProceed(screen_name);DisplayErrorMessage('110107'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            }
                        }
                        //DateTime dttime = DateTime.Now;
                        //dtpRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                        //dtpRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);

                    }
                }
            }
            else if (btnSaveForRecurring.Text == "Update")
            {
                //if (saveList != null)
                //{
                //    if (saveList.Count > 0)
                //    {
                //        if (Convert.ToUInt64(lblId.Text) == 0)
                //        {
                //            if (hdnBlockdaysId.Value != string.Empty)
                //            {
                //                GroupID = blockDaysMngr.UpdateBlockDays(blockDayObj, "");
                //                saveCount++;
                //                //ApplicationObject.erroHandler.DisplayErrorMessage("110106", "BlockDays", this.Page);
                //                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110106');", true);
                //            }
                //        }
                //        else
                //        {
                //            GroupID = blockDaysMngr.UpdateRecursiveBlockdays(updtGroupID, saveList[0].Physician_ID, saveList.ToArray<Blockdays>(), "");
                //            //blkDayProxy.DeleteUsingFromAndToDates(updtFacName, updtPhyId, "RECURSIVE", updtFromDt, updtToDt, updtDay, saveList.ToArray<Blockdays>());
                //            // blkDayProxy.InsertBlockDaysList(saveList.ToArray<Blockdays>());
                //            saveCount++;
                //            //ApplicationObject.erroHandler.DisplayErrorMessage("110107", "BlockDays", this.Page);
                //            ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110107');", true);
                //        }
                //    }
                //}
                blockDaysMngr.SaveUpdateBlockDays(saveList, UpdateList, deletelist, string.Empty);
                if ((YesRecur == "Yes") && (YesForTabClick == ""))
                {
                    hdnToFindSource.Value = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "var TabClick = top.window.document.getElementById('ctl00_hdnTabClick'); var which_tab = TabClick.value.split('$#$')[0]; var screen_name; if (which_tab != 'first'){ screen_name = 'BlockDaysTabClick'; } SavedSuccessfully_NowProceed(screen_name);DisplayErrorMessage('110106');GetRadNewWindow(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                }
                else
                {
                    hdnToFindSource.Value = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "var TabClick = top.window.document.getElementById('ctl00_hdnTabClick'); var which_tab = TabClick.value.split('$#$')[0]; var screen_name; if (which_tab != 'first'){ screen_name = 'BlockDaysTabClick'; } SavedSuccessfully_NowProceed(screen_name);DisplayErrorMessage('110106'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                }//DateTime dttime = DateTime.Now;
                //dtpRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                //dtpRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);

            }
            cleartxt();


        }


        protected void btnSaveForNonRecur_Click(object sender, EventArgs e)
        {
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ddlFacilityName.SelectedItem.Text select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            string YesForNonRecur = hdnMessageType.Value;
            string YesForTabChange = hdnForTabClick.Value;
            hdnForTabClick.Value = string.Empty;
            hdnMessageType.Value = string.Empty;
            IList<Blockdays> blkBlockedList;
            btnSaveForNonRecur.Enabled = false;
            hdnSaveForDlc.Value = "false";
            saveCount = 0;
            saveList.Clear();
            bChk = false;
            if (btnSaveForNonRecur.Text == "Save")
            {
                hdnNonRecBlockDaysId.Value = string.Empty;
            }
            if (hdnNonRecBlockDaysId.Value != string.Empty)
            {
                lblId.Text = hdnNonRecBlockDaysId.Value;
            }
            if (ddlFacilityName.Text == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110110","Block Days", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110110'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                btnSaveForNonRecur.Enabled = true;
                hdnToFindSource.Value = "ToEnableNon";
                hdnSaveForDlc.Value = "true";
                bChk = true;
                return;
            }
            if (chklstboxProvider.Items.Count == 0)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110111","Block Days", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110111'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                btnSaveForNonRecur.Enabled = true;
                hdnToFindSource.Value = "ToEnableNon";
                hdnSaveForDlc.Value = "true";
                bChk = true;
                return;
            }
            if (chklstboxProvider.SelectedItem == null)
            {
                // ApplicationObject.erroHandler.DisplayErrorMessage("110114","Block Days", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110114'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                btnSaveForNonRecur.Enabled = true;
                hdnToFindSource.Value = "ToEnableNon";
                hdnSaveForDlc.Value = "true";
                bChk = true;
                return;
            }
            //Validation
            //if (chkNonRecurSelectTime.Checked == false)
            //{
            //    ScriptManager.RegisterStartupScript(this,this.GetType (), "BlockDays", "DisplayErrorMessage('110205');", true);
            //    bChk = true;
            //    return;
            //}
            if (dtpNonRecurDate.SelectedDate.Value == DateTime.MinValue)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110123'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                dtpNonRecurDate.Focus();
                btnSaveForNonRecur.Enabled = true;
                hdnToFindSource.Value = "ToEnableNon";
                hdnSaveForDlc.Value = "true";
                bChk = true;
                return;
            }


            if (Convert.ToDateTime(dtpNonRecurDate.SelectedDate.Value).Date < Convert.ToDateTime(hdnLocalTime.Value).Date)
            {
                if (lblId.Text == "0")
                {
                    //ApplicationObject.erroHandler.DisplayErrorMessage("110105","Block Days", this.Page);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110105'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    dtpNonRecurDate.Focus();
                    btnSaveForNonRecur.Enabled = true;
                    hdnToFindSource.Value = "ToEnableNon";
                    hdnSaveForDlc.Value = "true";
                    bChk = true;
                    return;
                }
            }
            if (txtDescription.txtDLC.Text.Trim() == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110115","Block Days", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110115'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                txtDescription.Focus();
                btnSaveForNonRecur.Enabled = true;
                hdnToFindSource.Value = "ToEnableNon";
                hdnSaveForDlc.Value = "true";
                bChk = true;
                return;

            }
            if (dtpNonRecurFromTime.SelectedTime == null)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "BlockDays", "DisplayErrorMessage('110127'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            if (dtpNonRecurToTime.SelectedTime == null)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "BlockDays", "DisplayErrorMessage('110128'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            string[] sp = null;
            sp = dtpNonRecurDate.SelectedDate.Value.ToString().Split(' ');
            string tt = "";//Convert.ToDateTime(dtpNonRecurDate.DbSelectedDate).ToString("tt");


            if (dtpNonRecurFromTime.SelectedTime.Value.Hours >= 12)
                tt = "PM";

            else
                tt = Convert.ToDateTime(dtpNonRecurDate.DbSelectedDate).ToString("tt");

            DateTime curdate = Convert.ToDateTime(sp[0] + " " + dtpNonRecurFromTime.SelectedTime.Value.Hours + ":" + dtpNonRecurFromTime.SelectedTime.Value.Minutes + " " + tt);
            DateTime Univblockdate = UtilityManager.ConvertToUniversal(curdate);// change to universal
            if (chkNonRecurSelectTime.Checked == true)
            {
                if (Convert.ToDateTime(dtpNonRecurDate.SelectedDate.Value).Date == Convert.ToDateTime(hdnLocalTime.Value).Date && Univblockdate.TimeOfDay < Convert.ToDateTime(hdnLocalTime.Value).TimeOfDay && lblId.Text == "0")
                {
                    // ApplicationObject.erroHandler.DisplayErrorMessage("110112","Block Days", this.Page);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110112'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    btnSaveForNonRecur.Enabled = true;
                    hdnToFindSource.Value = "ToEnableNon";
                    hdnSaveForDlc.Value = "true";
                    bChk = true;
                    return;
                }
                //if (dtpNonRecurFromTime.Value >= dtpNonRecurToTime.Value)
                //{
                //    ApplicationObject.erroHandler.DisplayErrorMessage("110104", this.Text);
                //    btnSaveForNonRecur.Enabled = true;
                //    return;
                //}                

            }

            if (dtpNonRecurFromTime.SelectedTime.Value > dtpNonRecurToTime.SelectedTime.Value)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110118","Block Days", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110118'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                btnSaveForNonRecur.Enabled = true;
                hdnToFindSource.Value = "ToEnableNon";
                hdnSaveForDlc.Value = "true";
                bChk = true;
                dtpNonRecurFromTime.Focus();
                return;
            }
            ulong UGroupID = Convert.ToUInt32(hdnGruopId.Value);
            for (int p = 0; p < chklstboxProvider.Items.Count; p++)
            {
                // ListViewItem item = lvwProviders.CheckedItems[p] as ListViewItem;
                if (chklstboxProvider.Items[p].Selected == true)
                {
                   UGroupID= UGroupID + 1;
                    if (Convert.ToUInt64(lblId.Text) == 0)
                    {
                        blockDayObj = new Blockdays();
                       // blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                        //if (sAncillary != string.Empty && sAncillary == ddlFacilityName.SelectedItem.Text.Trim())
                        if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                        {
                            blockDayObj.Machine_Technician_Library_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                        }
                        else
                        {
                            blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                        }
                        blockDayObj.Facility_Name = ddlFacilityName.SelectedItem.Text;
                        blockDayObj.Block_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpNonRecurDate.SelectedDate.Value));
                        blockDayObj.From_Date_Choosen = UtilityManager.ConvertToUniversal(dtpNonRecurDate.SelectedDate.Value);
                        blockDayObj.To_Date_Choosen = UtilityManager.ConvertToUniversal(dtpNonRecurDate.SelectedDate.Value);
                        blockDayObj.Reason = txtDescription.txtDLC.Text;
                        if (chkNonRecurSelectTime.Checked == true)
                        {
                            string FromTime = dtpNonRecurFromTime.SelectedTime.ToString();
                            string[] SFromTime = FromTime.Split(':');
                            blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                            string ToTime = dtpNonRecurToTime.SelectedTime.ToString();
                            string[] SToTime = ToTime.Split(':');
                            blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];
                        }
                        else
                        {
                            string FromTime = dtpNonRecurFromTime.SelectedTime.ToString();
                            string[] SFromTime = FromTime.Split(':');
                            blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                            string ToTime = dtpNonRecurToTime.SelectedTime.ToString();
                            string[] SToTime = ToTime.Split(':');
                            blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];
                            //TimeList = blockDaysMngr.GetStartAndEndTimeFromFacilityLibrary(ddlFacilityName.SelectedItem.Text);
                            ////blockDayObj.From_Time = "00:01";
                            ////blockDayObj.To_Time = "23:59";
                            //blockDayObj.From_Time = TimeList[0].ToString();
                            //blockDayObj.To_Time = TimeList[1].ToString();
                        }
                        //blockDayObj.Blockdays_Group_ID = Convert.ToUInt32(hdnGruopId.Value) + 1;//commented by naveena //Issue when they selected multiple providers.
                        blockDayObj.Blockdays_Group_ID = UGroupID;
                        blockDayObj.Day_Choosen = Convert.ToDateTime(dtpNonRecurDate.SelectedDate.Value).DayOfWeek.ToString();
                        blockDayObj.Block_Type = "NON RECURSIVE";
                        blockDayObj.Created_By = ClientSession.UserName;
                        blockDayObj.Modified_By = ClientSession.UserName;
                        blockDayObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value));
                        blockDayObj.Created_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value));
                        saveList.Add(blockDayObj);
                    }
                    else
                    {
                        DateTime PreviousDate = DateTime.Now;
                        blkBlockedList = blockDaysMngr.GetBlockedDaysByBlockId(Convert.ToUInt64(lblId.Text));
                        if (blkBlockedList.Count > 0)
                        {
                            blockDayObj = blkBlockedList[0];
                            PreviousDate = blkBlockedList[0].From_Date_Choosen;
                            //blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                           // if (sAncillary != string.Empty && sAncillary == ddlFacilityName.SelectedItem.Text.Trim())
                            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                            {
                                blockDayObj.Machine_Technician_Library_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                            }
                            else
                            {
                                blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                            }
                            blockDayObj.Facility_Name = ddlFacilityName.SelectedItem.Text;
                            blockDayObj.Block_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpNonRecurDate.SelectedDate.Value));
                            blockDayObj.Reason = txtDescription.txtDLC.Text;
                            blockDayObj.From_Date_Choosen = UtilityManager.ConvertToUniversal(dtpNonRecurDate.SelectedDate.Value);
                            blockDayObj.To_Date_Choosen = UtilityManager.ConvertToUniversal(dtpNonRecurDate.SelectedDate.Value);
                            blockDayObj.Day_Choosen = (Convert.ToDateTime(dtpNonRecurDate.SelectedDate.Value).DayOfWeek).ToString();
                            if (chkNonRecurSelectTime.Checked == true)
                            {
                                string minfrom, minTo;
                                if (dtpNonRecurFromTime.SelectedTime.Value.Minutes.ToString() == "0")
                                    minfrom = "00";
                                else
                                    minfrom = dtpNonRecurFromTime.SelectedTime.Value.Minutes.ToString();


                                if (dtpNonRecurToTime.SelectedTime.Value.Minutes.ToString() == "0")
                                    minTo = "00";
                                else
                                    minTo = dtpNonRecurToTime.SelectedTime.Value.Minutes.ToString();
                                blockDayObj.From_Time = dtpNonRecurFromTime.SelectedTime.Value.Hours + ":"+ minfrom;
                                //blockDayObj.From_Time = dtpNonRecurFromTime.SelectedTime.Value.Hours + ":" + ":" + minfrom;// +dtpNonRecurFromTime.SelectedTime.Value.Minutes + ":" + dtpNonRecurFromTime.SelectedTime.Value.Seconds;
                                blockDayObj.To_Time = dtpNonRecurToTime.SelectedTime.Value.Hours + ":" + minTo;//":" + dtpNonRecurToTime.SelectedTime.Value.Minutes + ":" + dtpNonRecurToTime.SelectedTime.Value.Seconds;
                                //if (dtpNonRecurFromTime.AmPm.ToString() == "PM")
                                //{
                                //    blockDayObj.From_Time = (dtpNonRecurFromTime.Hour + 12) + ":" + dtpNonRecurFromTime.Minute;
                                //}
                                //else
                                //{
                                //    blockDayObj.From_Time = dtpNonRecurFromTime.Hour + ":" + dtpNonRecurFromTime.Minute;
                                //}
                                //if (dtpNonRecurToTime.AmPm.ToString() == "PM")
                                //{
                                //    blockDayObj.To_Time = (dtpNonRecurToTime.Hour + 12) + ":" + dtpNonRecurToTime.Minute;
                                //}
                                //else
                                //{
                                //    blockDayObj.To_Time = dtpNonRecurToTime.Hour + ":" + dtpNonRecurToTime.Minute;
                                //}
                            }
                            else
                            {
                                blockDayObj.From_Time = string.Empty;
                                blockDayObj.To_Time = string.Empty;
                            }
                            blockDayObj.Modified_By = ClientSession.UserName;
                            blockDayObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value));
                            if (blockDayObj.From_Date_Choosen == blockDayObj.To_Date_Choosen)
                            {
                                blockDayObj.Block_Type = "NON RECURSIVE";
                            }
                            else
                            {
                                blockDayObj.Block_Type = "RECURSIVE";
                            }
                        }
                        blockDaysMngr.UpdateBlockDays(blockDayObj, "", PreviousDate);
                        saveCount++;
                        // ApplicationObject.erroHandler.DisplayErrorMessage("110107", "Block Days", this.Page);
                        // ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "var TabClick = top.window.document.getElementById('ctl00_hdnTabClick'); var which_tab = TabClick.value.split('$#$')[0]; var screen_name; if (which_tab != 'first'){ screen_name = 'BlockDaysTabClick'; } SavedSuccessfully_NowProceed(screen_name);DisplayErrorMessage('110107');", true);
                    }

                    if (saveList != null)
                    {
                        if (saveList.Count > 0)
                        {
                            GroupID = blockDaysMngr.InsertBlockDaysList(saveList.ToArray<Blockdays>(), "");
                            saveCount++;
                            saveList.Clear();//BugID:52958
                            //ApplicationObject.erroHandler.DisplayErrorMessage("110106", "Block Days", this.Page);
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "var TabClick = top.window.document.getElementById('ctl00_hdnTabClick'); var which_tab = TabClick.value.split('$#$')[0]; var screen_name; if (which_tab != 'first'){ screen_name = 'BlockDaysTabClick'; } SavedSuccessfully_NowProceed(screen_name);DisplayErrorMessage('110106');", true);
                        }
                    }
                }

            }
            if (saveCount > 0)
            {
                if ((YesForNonRecur == "Yes") && (YesForTabChange == ""))
                {
                    hdnToFindSource.Value = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "var TabClick = top.window.document.getElementById('ctl00_hdnTabClick'); var which_tab = TabClick.value.split('$#$')[0]; var screen_name; if (which_tab != 'first'){ screen_name = 'BlockDaysTabClick'; } SavedSuccessfully_NowProceed(screen_name);DisplayErrorMessage('110106');GetRadNewWindow(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                }
                else
                {
                    hdnToFindSource.Value = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "var TabClick = top.window.document.getElementById('ctl00_hdnTabClick'); var which_tab = TabClick.value.split('$#$')[0]; var screen_name; if (which_tab != 'first'){ screen_name = 'BlockDaysTabClick'; } SavedSuccessfully_NowProceed(screen_name);DisplayErrorMessage('110106'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                }
            }
            cleartxt();
            divLoading.Style.Add("display", "none");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }


        protected void btnSaveForRecurring_Click(object sender, EventArgs e)
        {
            btnSaveForRecurring.Enabled = false;
            hdnSaveForDlc.Value = "false";
            //UIManager.BeginAction(this);
            saveCount = 0;
            AddBlockDays();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //divLoading.Style.Add("display", "none");
            //UIManager.EndAction(this);
        }

        protected void ibtnRecurDropDown_Click(object sender, ImageClickEventArgs e)
        {
            //Added By Gopal
            //if (hdnSelectedIndex.Value == "Plus")
            //{
            //    pnlListBox.Visible = true;
            //    hdnSelectedIndex.Value = "Minus";
            //    lstRecurDescription.Height = txtRecurringDescription.Height;
            //    ibtnRecurDropDown.ImageUrl = "~/Resources/minus_new.gif";
            //    LoadNotes("BLOCK DAYS DESCRIPTION", lstRecurDescription);
            //}
            //else
            //{
            //    ibtnRecurDropDown.ImageUrl = "~/Resources/plus_new.gif";
            //    hdnSelectedIndex.Value = "Plus";
            //    pnlListBox.Visible = false;
            //}
        }



        protected void ibtnRecurClear_Click(object sender, ImageClickEventArgs e)
        {
            // txtRecurringDescription.Text = string.Empty;
        }

        protected void ibtnNonRecurClear_Click(object sender, ImageClickEventArgs e)
        {
            //txtDescription.Text = string.Empty;
        }

        protected void ibtnNonRecurDropDown_Click(object sender, ImageClickEventArgs e)
        {

            //if (hdnNonRecurDescription.Value == "Plus")
            //{
            //    pnlNonRecurListBox.Visible = true;
            //    ibtnNonRecurDropDown.ImageUrl = "~/Resources/minus_new.gif";
            //    hdnNonRecurDescription.Value = "Minus";
            //    LoadNotes("BLOCK DAYS DESCRIPTION", lstNoRecurDescription);
            //}
            //else
            //{
            //    ibtnNonRecurDropDown.ImageUrl = "~/Resources/plus_new.gif";
            //    hdnNonRecurDescription.Value = "Plus";
            //    pnlNonRecurListBox.Visible = false;
            //}

        }

        protected void dtpRecurFromDate_TextChanged1(object sender, EventArgs e)
        {
            btnSaveForRecurring.Enabled = true;
            btnClearForRecurring.Enabled = true;
            hdnSaveForDlc.Value = "true";
        }

        protected void dtpRecurToDate_TextChanged1(object sender, EventArgs e)
        {
            btnSaveForRecurring.Enabled = true;
            btnClearForRecurring.Enabled = true;
            hdnSaveForDlc.Value = "true";
        }

        protected void chkMonday_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMonday.Checked == true)
            {
                btnSaveForRecurring.Enabled = true;
                btnClearForRecurring.Enabled = true;
                hdnSaveForDlc.Value = "true";
            }
        }

        protected void chkTuesday_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTuesday.Checked == true)
            {
                btnSaveForRecurring.Enabled = true;
                btnClearForRecurring.Enabled = true;
                hdnSaveForDlc.Value = "true";
            }
        }

        protected void chkWednesday_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWednesday.Checked == true)
            {
                btnSaveForRecurring.Enabled = true;
                btnClearForRecurring.Enabled = true;
                hdnSaveForDlc.Value = "true";
            }
        }

        protected void chkThursday_CheckedChanged(object sender, EventArgs e)
        {
            if (chkThursday.Checked == true)
            {
                btnSaveForRecurring.Enabled = true;
                btnClearForRecurring.Enabled = true;
                hdnSaveForDlc.Value = "true";
            }
        }

        protected void chkFriday_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFriday.Checked == true)
            {
                btnSaveForRecurring.Enabled = true;
                btnClearForRecurring.Enabled = true;
                hdnSaveForDlc.Value = "true";
            }
        }

        protected void chkSaturday_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSaturday.Checked == true)
            {
                btnSaveForRecurring.Enabled = true;
                btnClearForRecurring.Enabled = true;
                hdnSaveForDlc.Value = "true";

            }
        }

        protected void chkSunday_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSunday.Checked == true)
            {
                btnSaveForRecurring.Enabled = true;
                btnClearForRecurring.Enabled = true;
                hdnSaveForDlc.Value = "true";
            }
        }

        protected void dtpNonRecurDate_TextChanged(object sender, EventArgs e)
        {
            btnSaveForNonRecur.Enabled = true;
            btnCancelForNonRecur.Enabled = true;
            hdnSaveForDlc.Value = "true";
        }

        protected void lstRecurDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (txtRecurringDescription.Text.Contains(lstRecurDescription.SelectedItem.Text) == false)
            //{
            //    if (txtRecurringDescription.Text == string.Empty)
            //    {
            //        txtRecurringDescription.Text = lstRecurDescription.SelectedItem.Text;
            //    }
            //    else
            //    {
            //        txtRecurringDescription.Text = txtRecurringDescription.Text + "," + lstRecurDescription.SelectedItem.Text;
            //    }
            //}


        }

        protected void lstNoRecurDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            //    if (txtDescription.Text.Contains(lstNoRecurDescription.SelectedItem.Text) == false)
            //    {
            //        if (txtDescription.Text == string.Empty)
            //        {
            //            txtDescription.Text = lstNoRecurDescription.SelectedItem.Text;
            //        }
            //        else
            //        {
            //            txtDescription.Text = txtDescription.Text + "," + lstNoRecurDescription.SelectedItem.Text;
            //        }
            //    }

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BlockdaysManager objBlockDaysMngr = new BlockdaysManager();
            IList<Blockdays> ilstBlockDaysList = new List<Blockdays>();
            ilstBlockDaysList = objBlockDaysMngr.GetBlockedDaysByBlockId(Convert.ToUInt64(hdnBlockdaysId.Value));
            for (int i = 0; i < ilstBlockDaysList.Count; i++)
            {
                if (ilstBlockDaysList[0].Block_Type == "RECURSIVE")
                {
                    tabBlockDays.SelectedIndex = 0;
                    dtpRecurFromDate.SelectedDate = ilstBlockDaysList[0].From_Date_Choosen;
                    dtpRecurToDate.SelectedDate = ilstBlockDaysList[0].To_Date_Choosen;
                    if (ilstBlockDaysList[0].From_Time != string.Empty)
                    {
                        //string[] sarry = ilstBlockDaysList[0].From_Time.Split(':');
                        //dtpRecurFromTime.Hour = Convert.ToInt32(sarry[0]);
                        //dtpRecurFromTime.Minute = Convert.ToInt32(sarry[1]);
                        //if (sarry[2].ToString().ToUpper() == "PM")
                        //{
                        //    dtpRecurFromTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                        //}
                        //else
                        //{
                        //    dtpRecurFromTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                        //}
                        DateTime dt = Convert.ToDateTime(ilstBlockDaysList[0].From_Time.ToString());
                        dtpRecurFromTime.SelectedTime = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
                    }
                    if (ilstBlockDaysList[0].To_Time != string.Empty)
                    {
                        //string[] sarry = ilstBlockDaysList[0].To_Time.ToString().Split(':');
                        //dtpRecurToTime.Hour = Convert.ToInt32(sarry[0]);
                        //dtpRecurToTime.Minute = Convert.ToInt32(sarry[1]);
                        //if (sarry[2].ToString().ToUpper() == "PM")
                        //{
                        //    dtpRecurToTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                        //}
                        //else
                        //{
                        //    dtpRecurToTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                        //}
                        DateTime dt1 = Convert.ToDateTime(ilstBlockDaysList[0].To_Time.ToString());
                        dtpRecurFromTime.SelectedTime = new TimeSpan(dt1.Hour, dt1.Minute, dt1.Second);
                    }
                }
                else
                {
                    tabBlockDays.SelectedIndex = 1;
                    dtpNonRecurDate.SelectedDate = ilstBlockDaysList[0].From_Date_Choosen;
                    if (ilstBlockDaysList[0].From_Time != string.Empty)
                    {
                        //string[] sarry = ilstBlockDaysList[0].From_Time.Split(':');
                        //dtpNonRecurFromTime.Hour = Convert.ToInt32(sarry[0]);
                        //dtpNonRecurFromTime.Minute = Convert.ToInt32(sarry[1]);
                        //if (sarry[2].ToString().ToUpper() == "PM")
                        //{
                        //    dtpNonRecurFromTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                        //}
                        //else
                        //{
                        //    dtpNonRecurFromTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                        //}
                        DateTime dt = Convert.ToDateTime(ilstBlockDaysList[0].From_Time.ToString());
                        dtpNonRecurFromTime.SelectedTime = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
                    }
                    if (ilstBlockDaysList[0].To_Date_Choosen.ToString() != string.Empty)
                    {
                        //string[] sarry = ilstBlockDaysList[0].To_Date_Choosen.ToString().Split(':');
                        //dtpNonRecurToTime.Hour = Convert.ToInt32(sarry[0]);
                        //dtpNonRecurToTime.Minute = Convert.ToInt32(sarry[1]);
                        //if (sarry[2].ToString().ToUpper() == "PM")
                        //{
                        //    dtpNonRecurToTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                        //}
                        //else
                        //{
                        //    dtpNonRecurToTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                        //}
                        DateTime dt1 = Convert.ToDateTime(ilstBlockDaysList[0].To_Date_Choosen.ToString());
                        dtpNonRecurToTime.SelectedTime = new TimeSpan(dt1.Hour, dt1.Minute, dt1.Second);
                    }
                }

            }
        }

        protected void btnSaveTOF_Click(object sender, EventArgs e)
        {
            string YesForSaveTOF = hdnMessageType.Value;
            string YesForSaveTab = hdnForTabClick.Value;
            hdnForTabClick.Value = string.Empty;
            hdnMessageType.Value = string.Empty;
            saveList.Clear();
            saveCount = 0;
            arrDay.Clear();
            days.Clear();
            bChk = false;
            if (chklstboxProvider.Items.Count == 0)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110111", "BlockDays", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110111');", true);
                btnSaveForRecurring.Enabled = true;
                hdnSaveForDlc.Value = "true";
                //UIManager.EndAction(this);
                bChk = true;
                return;
            }
            if (chklstboxProvider.SelectedItem == null)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110114", "BlockDays", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110114');", true);
                btnSaveForRecurring.Enabled = true;
                hdnSaveForDlc.Value = "true";
                bChk = true;
                return;
            }

            if (cboTypeOfVisit.Text == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110120", "Block Days", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110120');", true);
                cboTypeOfVisit.Focus();
                btnSaveTOF.Enabled = true;
                hdnSaveForDlc.Value = "true";
                bChk = true;
                return;

            }
            //Get the Checked Days into an arraylist


            if (chkMondayTOF.Checked == true)
            {
                arrDay.Add(chkMondayTOF.Text);

            }
            if (chkTuesdayTOF.Checked == true)
            {
                arrDay.Add(chkTuesdayTOF.Text);
            }
            if (chkWednesdayTOF.Checked == true)
            {
                arrDay.Add(chkWednesdayTOF.Text);
            }
            if (chkThursdayTOF.Checked == true)
            {
                arrDay.Add(chkThursdayTOF.Text);
            }
            if (chkFridayTOF.Checked == true)
            {
                arrDay.Add(chkFridayTOF.Text);
            }
            if (chkSaturdayTOF.Checked == true)
            {
                arrDay.Add(chkSaturdayTOF.Text);
            }
            if (chkSundayTOF.Checked == true)
            {
                arrDay.Add(chkSundayTOF.Text);
            }


            if (dtpFromdateTOF.SelectedDate.Value == DateTime.MinValue)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110121');", true);
                btnSaveTOF.Enabled = true;
                hdnSaveForDlc.Value = "true";
                dtpFromdateTOF.Focus();
                bChk = true;
                return;
            }
            else if (dtpTodateTOF.SelectedDate.Value == DateTime.MinValue)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110122');", true);
                dtpTodateTOF.Focus();
                hdnSaveForDlc.Value = "true";
                btnSaveTOF.Enabled = true;
                bChk = true;
                return;
            }

            if (Convert.ToDateTime(dtpFromdateTOF.SelectedDate.Value).Date < Convert.ToDateTime(hdnLocalTime.Value).Date && lblId.Text == "0")
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110101", "BlockDays", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110101');", true);
                btnSaveTOF.Enabled = true;
                hdnSaveForDlc.Value = "true";
                dtpFromdateTOF.Focus();
                bChk = true;
                return;
            }
            //Checks whether From Date and To Date are same or not
            else if (Convert.ToDateTime(dtpTodateTOF.SelectedDate.Value).Date == Convert.ToDateTime(dtpFromdateTOF.SelectedDate.Value).Date)
            {
                // ApplicationObject.erroHandler.DisplayErrorMessage("110102", "BlockDays", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110102');", true);
                dtpTodateTOF.Focus();
                btnSaveTOF.Enabled = true;
                hdnSaveForDlc.Value = "true";
                bChk = true;
                return;
            }

            //Checks Whether the To Date occurs before the From Date
            else if (Convert.ToDateTime(dtpTodateTOF.SelectedDate.Value) < Convert.ToDateTime(dtpFromdateTOF.SelectedDate.Value))
            {
                // ApplicationObject.erroHandler.DisplayErrorMessage("110108", "BlockDays", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110108');", true);
                dtpTodateTOF.Focus();
                hdnSaveForDlc.Value = "true";
                btnSaveTOF.Enabled = true;
                bChk = true;
                return;
            }

            //Checks atleast a single Day is checked
            else if (arrDay.Count == 0)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110103", "BlockDays", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110103');", true);
                btnSaveTOF.Enabled = true;
                hdnSaveForDlc.Value = "true";
                bChk = true;
                return;
            }
            if (dtpFromTimeTOF.SelectedTime.Value > dtpToTimeTOF.SelectedTime.Value)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110118", "BlockDays", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110118');", true);
                btnSaveTOF.Enabled = true;
                bChk = true;
                hdnSaveForDlc.Value = "true";
                dtpFromTimeTOF.Focus();
                return;
            }

            if (dtpFromTimeTOF.SelectedTime.Value == dtpToTimeTOF.SelectedTime.Value)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110119", "BlockDays", this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110119');", true);
                btnSaveTOF.Enabled = true;
                bChk = true;
                hdnSaveForDlc.Value = "true";
                dtpToTimeTOF.Focus();
                return;
            }

          ulong UGroupID= Convert.ToUInt32(hdnGruopId.Value);
          var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ddlFacilityName.SelectedItem.Text select f;
          IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            for (int p = 0; p < chklstboxProvider.Items.Count; p++)
            {
                if (chklstboxProvider.Items[p].Selected == true)
                {
                    UGroupID=UGroupID+1;
                    fromDate = Convert.ToDateTime(dtpFromdateTOF.SelectedDate.Value);
                    toDate = Convert.ToDateTime(dtpTodateTOF.SelectedDate.Value);
                    ListItem item = chklstboxProvider.SelectedItem as ListItem;
                    while (fromDate.Date <= toDate.Date)
                    {
                        for (int a = 0; a < arrDay.Count; a++)
                        {
                            checkedDay = arrDay[a].ToString();
                            if (arrDay[a].ToString() == fromDate.DayOfWeek.ToString())
                            {
                                //addFlag = true;
                                blockDayObj = new Blockdays();
                                if (hdnBlockdaysId.Value != string.Empty)
                                {
                                    blockDayObj.Id = Convert.ToUInt32(hdnBlockdaysId.Value);
                                }
                              //  blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                                //if (sAncillary != string.Empty && sAncillary == ddlFacilityName.SelectedItem.Text.Trim())
                                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                                {
                                    blockDayObj.Machine_Technician_Library_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                                }
                                else
                                {
                                    blockDayObj.Physician_ID = Convert.ToUInt32(chklstboxProvider.Items[p].Value);
                                }
                                blockDayObj.Facility_Name = ddlFacilityName.SelectedItem.Text.ToString();
                                blockDayObj.Block_Date = UtilityManager.ConvertToUniversal(fromDate);
                                //blockDayObj.From_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpFromdateTOF.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                                blockDayObj.From_Date_Choosen = UtilityManager.ConvertToUniversal(fromDate.Date);
                                blockDayObj.To_Date_Choosen = Convert.ToDateTime(Convert.ToDateTime(dtpTodateTOF.SelectedDate.Value).ToString("yyyy-MM-dd HH:ss:mm"));
                                blockDayObj.Reason = txtDescriptionTOF.txtDLC.Text;
                                if (chktypeofvisitSelectTime.Checked == true)
                                {
                                    string FromTime = dtpFromTimeTOF.SelectedTime.ToString();
                                    string[] SFromTime = FromTime.Split(':');
                                    blockDayObj.From_Time = SFromTime[0] + ":" + SFromTime[1];
                                    string ToTime = dtpToTimeTOF.SelectedTime.ToString();
                                    string[] SToTime = ToTime.Split(':');
                                    blockDayObj.To_Time = SToTime[0] + ":" + SToTime[1];

                                }
                                else
                                {
                                    TimeList = blockDaysMngr.GetStartAndEndTimeFromFacilityLibrary(ddlFacilityName.SelectedItem.Text);
                                    blockDayObj.From_Time = TimeList[0].ToString();
                                    blockDayObj.To_Time = TimeList[1].ToString();
                                }
                                blockDayObj.Blockdays_Group_ID = UGroupID;
                                blockDayObj.Day_Choosen = arrDay[a].ToString();
                                blockDayObj.Block_Type = cboTypeOfVisit.SelectedItem.Text;
                                blockDayObj.Created_By = ClientSession.UserName;
                                blockDayObj.Modified_By = ClientSession.UserName;
                                blockDayObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value));
                                blockDayObj.Created_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value));
                                saveList.Add(blockDayObj);
                            }


                        }
                        fromDate = fromDate.AddDays(1);
                    }
                    //Checkin for null value added by Janani on 2/8/10
                    if (saveList != null)
                    {
                        if (saveList.Count > 0)
                        {
                            days = (from d in saveList select d.Day_Choosen).ToList<string>();
                        }
                    }

                    for (int i = 0; i < arrDay.Count; i++)
                    {
                        if (!days.Contains(arrDay[i].ToString()))
                        {
                            errList = new ArrayList();
                            errList.Add(arrDay[i]);
                            string stxt = errList[0].ToString();
                            //ApplicationObject.erroHandler.DisplayErrorMessage("110109", "BlockDays", errList, this.Page);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110109','','" + stxt + "');", true);
                            btnSaveTOF.Enabled = true;
                            hdnSaveForDlc.Value = "true";
                            bChk = true;
                            return;
                        }
                    }


                }

            }
            if (btnSaveTOF.Text == "Save")
            {
                if (saveList != null)
                {
                    if (saveList.Count > 0)
                    {
                        if (Convert.ToUInt64(lblId.Text) == 0)
                        {
                            GroupID = blockDaysMngr.InsertBlockDaysList(saveList.ToArray<Blockdays>(), "");
                            saveCount++;
                            //ApplicationObject.erroHandler.DisplayErrorMessage("110106", "BlockDays", this.Page);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110106');", true);
                        }
                        else
                        {
                            if (saveList[0].Physician_ID!=0)
                            GroupID = blockDaysMngr.UpdateRecursiveBlockdays(updtGroupID, saveList[0].Physician_ID, saveList.ToArray<Blockdays>(), "");
                            else
                                GroupID = blockDaysMngr.UpdateRecursiveBlockdaysUsingTechID(updtGroupID, saveList[0].Machine_Technician_Library_ID, saveList.ToArray<Blockdays>(), "");
                            //blkDayProxy.DeleteUsingFromAndToDates(updtFacName, updtPhyId, "RECURSIVE", updtFromDt, updtToDt, updtDay, saveList.ToArray<Blockdays>());
                            // blkDayProxy.InsertBlockDaysList(saveList.ToArray<Blockdays>());
                            saveCount++;
                            //ApplicationObject.erroHandler.DisplayErrorMessage("110107", "BlockDays", this.Page);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "BlockDays", "DisplayErrorMessage('110107');", true);
                        }
                    }
                }
            }
            //else if (btnSaveTOF.Text == "Update")
            //{
            //    if (saveList != null)
            //    {
            //        if (saveList.Count > 0)
            //        {
            //            if (Convert.ToUInt64(lblId.Text) == 0)
            //            {
            //                GroupID = blockDaysMngr.UpdateBlockDays(blockDayObj, "");
            //                saveCount++;
            //                //ApplicationObject.erroHandler.DisplayErrorMessage("110106", "BlockDays", this.Page);
            //                ScriptManager.RegisterStartupScript(this,this.GetType (), "BlockDays", "DisplayErrorMessage('110106');", true);
            //            }
            //            else
            //            {
            //                GroupID = blockDaysMngr.UpdateRecursiveBlockdays(updtGroupID, saveList[0].Physician_ID, saveList.ToArray<Blockdays>(), "");
            //                //blkDayProxy.DeleteUsingFromAndToDates(updtFacName, updtPhyId, "RECURSIVE", updtFromDt, updtToDt, updtDay, saveList.ToArray<Blockdays>());
            //                // blkDayProxy.InsertBlockDaysList(saveList.ToArray<Blockdays>());
            //                saveCount++;
            //                //ApplicationObject.erroHandler.DisplayErrorMessage("110107", "BlockDays", this.Page);
            //                ScriptManager.RegisterStartupScript(this,this.GetType (), "BlockDays", "DisplayErrorMessage('110107');", true);
            //            }
            //        }
            //    }
            //}
            if ((YesForSaveTOF == "Yes") && (YesForSaveTab == ""))
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Demographics", "closeForYesClick();", true);
            }
            cleartxt();


        }

        protected void btnClearTOF_Click(object sender, EventArgs e)
        {
            cleartxt();

        }

        protected void ddlFacilityName_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ddlFacilityName.SelectedItem.Text select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            PhysicianList = new List<PhysicianLibrary>();
            PhysicianManager physicianMngr = new PhysicianManager();
            chklstboxProvider.Items.Clear();
            bool bSelectDefault = true;
            //PhysicianList = physicianMngr.GetPhysicianListbyFacility(ddlFacilityName.SelectedItem.Text, "Y");
            PhysicianList = UtilityManager.GetPhysicianList(ddlFacilityName.SelectedItem.Text.Trim(),ClientSession.LegalOrg);
            IList<string> PhyIDs = new List<string>();
            PhyIDs = PhysicianList.Select(a => Convert.ToString(a.Id)).ToList<string>();
            XmlDocument xmldoc = new XmlDocument();
            string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianFacilityMapping.xml");
            string sDefaultPhysicians = "";
            if (File.Exists(strXmlFilePath) == true)
            {
                xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianFacilityMapping" + ".xml");

                XmlNode nodeMatchingFacility = xmldoc.SelectSingleNode("/ROOT/PhyList/Facility[@name='" + ddlFacilityName.SelectedItem.Text.Trim() + "']");
                if (nodeMatchingFacility != null)
                    sDefaultPhysicians = nodeMatchingFacility.Attributes["default-physician-id"].Value.ToString();
            }
            if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" && PhyIDs.IndexOf(Convert.ToString(ClientSession.PhysicianId)) != -1)//BugID:45156
            {
                sDefaultPhysicians = Convert.ToString(ClientSession.PhysicianId);
            }
            string[] lstDefaultPhysicians = sDefaultPhysicians.Split(',');
           
            for (int i = 0; i < PhysicianList.Count; i++)
            {
                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
               // if (sAncillary != string.Empty && sAncillary == ddlFacilityName.SelectedItem.Text.Trim())
                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                {
                    pnlProvider.GroupingText = "Machine - Technician";
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
                            //item.Text = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
                            //item.Value = PhysicianList[i].PhyColor.ToString();//MachineTechnicianID

                            //Jira CAP-2777
                            MachinetechnicianList machinetechnicianList = new MachinetechnicianList();
                            machinetechnicianList = ConfigureBase<MachinetechnicianList>.ReadJson("machine_technician.json");
                            if (machinetechnicianList?.MachineTechnician != null)
                            {
                                List<Machinetechnician> machinetechnicians = new List<Machinetechnician>();
                                machinetechnicians = machinetechnicianList.MachineTechnician.Where(x => x.machine_technician_library_id == PhysicianList[i].PhyColor).ToList();
                                if (machinetechnicians.Count > 0)
                                {
                                    item.Text = machinetechnicians[0].machine_name + " - " + PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
                                    item.Value = PhysicianList[i].PhyColor.ToString();
                                }
                            }
                        }
                        else
                        {
                            item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
                            item.Value = PhysicianList[i].Id.ToString();
                        }
                    }
                }
                else
                {
                    pnlProvider.GroupingText = "Provider";
                    //old code
                    //item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
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
                
                chklstboxProvider.Items.Add(item);
                if (bSelectDefault)
                {
                    if (lstDefaultPhysicians.Any(curritem => curritem == PhysicianList[i].Id.ToString()))
                        chklstboxProvider.Items[i].Selected = true;
                    else
                        chklstboxProvider.Items[i].Selected = false;
                }
            }
            //Add By Manimaran 7/18/2014
            //ArrayList sPhyName = new ArrayList();
            //if (PhysicianList != null)
            //{
            //    if (PhysicianList.Count > 0)
            //    {
            //        for (int i = 0; i < PhysicianList.Count; i++)
            //        {
            //            string PhyName = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
            //            //string PhyName = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
            //            sPhyName.Add(PhyName);
            //            chklstboxProvider.Items.Add(sPhyName[i].ToString());
            //            chklstboxProvider.Items[i].Value = PhysicianList[i].Id.ToString();
            //            if (Request.QueryString["PhysicianSelected"] != null)
            //            {
            //                string[] sPhysicianChecked = null;
            //                sPhysicianChecked = Request.QueryString["PhysicianSelected"].ToString().Split('|');
            //                if (sPhysicianChecked != null)
            //                {
            //                    if (sPhysicianChecked.Contains(sPhyName[i].ToString().TrimEnd()))
            //                    {
            //                        chklstboxProvider.Items[i].Selected = true;
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                if (chklstboxProvider.Items.Count > 0)
            //                {
            //                    IList<AppointmentLookup> apptLookupList = new List<AppointmentLookup>();
            //                    apptLookupList = AppointmentMngr.GetAppointmentLookupList(ddlFacilityName.SelectedItem.Text);

            //                    for (int k = 0; k < apptLookupList.Count; k++)
            //                    {
            //                        for (int j = 0; j < chklstboxProvider.Items.Count; j++)
            //                        {
            //                            if (Convert.ToUInt64(chklstboxProvider.Items[j].Value) == apptLookupList[k].Physician_ID)
            //                            {
            //                                chklstboxProvider.Items[j].Selected = true;
            //                            }
            //                        }
            //                    }
            //                }
            //            }

            //        }
            //    }
            //}
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Facility", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void dtpRecurFromDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            btnSaveForRecurring.Enabled = true;
            btnClearForRecurring.Enabled = true;
            hdnSaveForDlc.Value = "true";
            hdnToFindSource.Value = "ToEnable";
        }

        protected void dtpRecurToDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            btnSaveForRecurring.Enabled = true;
            btnClearForRecurring.Enabled = true;
            hdnSaveForDlc.Value = "true";
            hdnToFindSource.Value = "ToEnable";
        }

        protected void dtpRecurFromTime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            btnSaveForRecurring.Enabled = true;
            btnClearForRecurring.Enabled = true;
            hdnSaveForDlc.Value = "true";
            hdnToFindSource.Value = "ToEnable";
        }

        protected void dtpRecurToTime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            btnSaveForRecurring.Enabled = true;
            btnClearForRecurring.Enabled = true;
            hdnSaveForDlc.Value = "true";
            hdnToFindSource.Value = "ToEnable";
        }

        protected void chkRecurSelecttime_CheckedChanged(object sender, EventArgs e)
        {
            DateTime dttime = DateTime.Now;
            if (chkRecurSelecttime.Checked)
            {
                dtpRecurFromTime.Enabled = true;
                dtpRecurToTime.Enabled = true;
                dtpRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                dtpRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                btnSaveForRecurring.Enabled = true;
                btnClearForRecurring.Enabled = true;
                hdnSaveForDlc.Value = "true";
                hdnToFindSource.Value = "ToEnable";
            }
            else
            {
                dtpRecurFromTime.Enabled = true;
                dtpRecurToTime.Enabled = true;
                //dtpRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                //dtpRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                //dtpRecurFromTime.Enabled = false;
                //dtpRecurToTime.Enabled = false;
                dtpRecurFromTime.SelectedTime = TimeSpan.Parse("00:01");
                dtpRecurToTime.SelectedTime = TimeSpan.Parse("23:59");

                //btnSaveForRecurring.Enabled = false;
            }
        }

        protected void dtpNonRecurDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            btnSaveForNonRecur.Enabled = true;
            btnCancelForNonRecur.Enabled = true;
            hdnSaveForDlc.Value = "true";
            hdnToFindSource.Value = "ToEnableNon";
        }

        protected void dtpNonRecurFromTime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            btnSaveForNonRecur.Enabled = true;
            btnCancelForNonRecur.Enabled = true;
            hdnSaveForDlc.Value = "true";
            hdnToFindSource.Value = "ToEnableNon";
        }

        protected void dtpNonRecurToTime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            btnSaveForNonRecur.Enabled = true;
            btnCancelForNonRecur.Enabled = true;
            hdnSaveForDlc.Value = "true";
            hdnToFindSource.Value = "ToEnableNon";
        }

        protected void chkNonRecurSelectTime_CheckedChanged(object sender, EventArgs e)
        {
            DateTime dttime = DateTime.Now;
            if (chkNonRecurSelectTime.Checked)
            {
                dtpNonRecurFromTime.Enabled = true;
                dtpNonRecurToTime.Enabled = true;
                dtpNonRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                dtpNonRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                btnSaveForNonRecur.Enabled = true;
                btnCancelForNonRecur.Enabled = true;
                hdnSaveForDlc.Value = "true";
                hdnToFindSource.Value = "ToEnableNon";
            }
            else
            {
                dtpNonRecurFromTime.Enabled = true;
                dtpNonRecurToTime.Enabled = true;
                //dtpNonRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                //dtpNonRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                //dtpNonRecurFromTime.Enabled = false;
                //dtpNonRecurToTime.Enabled = false;
                dtpNonRecurFromTime.SelectedTime = TimeSpan.Parse("00:01");
                dtpNonRecurToTime.SelectedTime = TimeSpan.Parse("23:59");
                //btnSaveForNonRecur.Enabled = false;
            }
        }

        protected void dtpFromdateTOF_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            btnSaveTOF.Enabled = true;
            hdnSaveForDlc.Value = "true";
        }

        protected void dtpTodateTOF_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            btnSaveTOF.Enabled = true;
            hdnSaveForDlc.Value = "true";
        }

        protected void chktypeofvisitSelectTime_CheckedChanged(object sender, EventArgs e)
        {
            DateTime dttime = DateTime.Now;
            if (chktypeofvisitSelectTime.Checked)
            {
                dtpFromTimeTOF.Enabled = true;
                dtpToTimeTOF.Enabled = true;
                dtpFromTimeTOF.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                dtpToTimeTOF.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                btnSaveTOF.Enabled = true;
                hdnSaveForDlc.Value = "true";
            }
            else
            {
                dtpFromTimeTOF.Enabled = false;
                dtpToTimeTOF.Enabled = false;
                dtpFromTimeTOF.SelectedTime = TimeSpan.Parse("00:01");
                dtpToTimeTOF.SelectedTime = TimeSpan.Parse("23:59");
                //btnSaveTOF.Enabled = false;
            }
        }

        protected void dtpFromTimeTOF_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            btnSaveTOF.Enabled = true;
            hdnSaveForDlc.Value = "true";
        }

        protected void dtpToTimeTOF_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            btnSaveTOF.Enabled = true;
            hdnSaveForDlc.Value = "true";
        }
        protected void btnInvisibleclear_Click(object sender, EventArgs e)
        {
           // cleartxt();
            DateTime dttime = DateTime.Now;
            //DateTime dttime = Convert.ToDateTime(hdnLocalTime.Value);
            //string sHour = string.Format("{0:HH:mm:ss tt}", Convert.ToDateTime(hdnLocalTime.Value).AddHours(1).ToShortTimeString());
            //string sHour1 = string.Format("{0:HH:mm:ss tt}", Convert.ToDateTime(hdnLocalTime.Value).ToShortTimeString());
            lblId.Text = "0";
            if (tabBlockDays.SelectedIndex == 0)
            {
                rdmpBlockDays.PageViews[0].Selected = true;
                btnSaveForRecurring.Text = "Save";
                btnClearForRecurring.Text = "Clear All";
                if (hdnLocalTime.Value != string.Empty)
                {
                    dtpRecurFromDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    dtpRecurToDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                }


                //dtpRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                //dtpRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                dtpRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                dtpRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                //chkRecurSelecttime.Checked = true;
                dtpRecurFromTime.Enabled = true;
                dtpRecurToTime.Enabled = true;
                chkRecurSelecttime.Checked = false;
                chkMonday.Checked = false;
                chkTuesday.Checked = false;
                chkWednesday.Checked = false;
                chkThursday.Checked = false;
                chkFriday.Checked = false;
                chkSaturday.Checked = false;
                chkSunday.Checked = false;
                chkAlternateWeeks.Checked = false;
                chkAlternateMonths.Checked = false;
                txtRecurringDescription.txtDLC.Text = string.Empty;
                btnSaveForRecurring.Enabled = false;
                btnClearForRecurring.Enabled = true;
                //ddlFacilityName.SelectedIndex = 0;
                hdnBlockDayType.Value = string.Empty;
                hdnBlockdaysId.Value = string.Empty;
                hdnNonRecBlockDaysId.Value = string.Empty;
                chkRecurSelecttime.Checked = true;
                hdnSaveForDlc.Value = "false";
            }
            else if (tabBlockDays.SelectedIndex == 1)
            {
                rdmpBlockDays.PageViews[1].Selected = true;
                btnSaveForNonRecur.Text = "Save";
                btnCancelForNonRecur.Text = "Clear All";
                dtpNonRecurDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                dtpNonRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);//Uncommented by Viji
                dtpNonRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);//Uncommented by Viji
                //dtpRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                //dtpRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
                dtpNonRecurFromTime.Enabled = true;
                dtpNonRecurToTime.Enabled = true;
                txtDescription.txtDLC.Text = string.Empty;
                btnSaveForNonRecur.Enabled = false;
                btnCancelForNonRecur.Enabled = true;
                chkNonRecurSelectTime.Checked = false;
                hdnBlockDayType.Value = string.Empty;
                hdnNonRecBlockDaysId.Value = string.Empty;
                hdnBlockdaysId.Value = string.Empty;
                chkNonRecurSelectTime.Checked = true;
                hdnSaveForDlc.Value = "false";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UnloadWaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        protected void btnInvisibleForBlockDays_Click(object sender, EventArgs e)
        {
            try
            {

                string s3 = hdnBlockDayType.Value;
                string[] sarry = hdnFromTime.Value.Split(':');
                int hour = Convert.ToInt32(sarry[0]);
                string[] sarryMin = sarry[1].Split(' ');
                int Minute = Convert.ToInt32(sarryMin[0]);
                string s = sarryMin[1].ToString();
                string[] sarryTo = hdnToTime.Value.Split(':');
                int hour1 = Convert.ToInt32(sarryTo[0]);
                string[] sarryMinTo = sarryTo[1].Split(' ');
                int Minute1 = Convert.ToInt32(sarryMinTo[0]);
                string s1 = sarryMinTo[1].ToString();
                string[] sDay = hdnDays.Value.Split(',');
                string sAlternateWeeks = hdnAlternateWeeks.Value;
                string sAlternateMonths = hdnAlternateMonths.Value;
                chkSaturday.Checked = false;
                chkSunday.Checked = false;
                if(sAlternateWeeks=="Y")
                {
                    chkAlternateWeeks.Checked = true;
                }
                if (sAlternateMonths == "Y")
                {
                    chkAlternateMonths.Checked = true;
                }
                for (int i = 0; i < sDay.Count(); i++)
                {
                    CheckBox chk = (CheckBox)pnlSelectDate.FindControl("chk" + sDay[i].Trim());
                    if (chk != null)
                    {
                        chk.Checked = true;
                    }
                    if (chkAlternateWeeks.Checked)
                        lstEditAlternateWeeks.Add(sDay[i].Trim());
                    else if (chkAlternateMonths.Checked)
                        lstEditAlternateMonths.Add(sDay[i].Trim());
                }

                if (hdnPhySelected.Value != string.Empty)
                {
                    CheckBoxList chkboxlst = (CheckBoxList)pnlProvider.FindControl("chklstboxProvider");
                    if (chkboxlst.Items.Count > 0)
                    {
                        for (int i = 0; i < chkboxlst.Items.Count; i++)
                        {
                            if (chkboxlst.Items[i].Text.Contains(hdnPhySelected.Value))
                            {
                                chkboxlst.Items[i].Selected = true;
                            }
                            else
                            {
                                chkboxlst.Items[i].Selected = false;
                            }
                        }
                    }
                }

                switch (hdnBlockDayType.Value)
                {
                    case "RECURSIVE":
                        txtRecurringDescription.txtDLC.Text = hdnRecDescription.Value;
                        chkRecurSelecttime.Enabled = true;
                        btnSaveForRecurring.Enabled = true;
                        btnClearForRecurring.Enabled = true;
                        hdnToFindSource.Value = "ToEnable";
                        btnSaveForRecurring.Text = "Update";
                        //btnSaveForRecurring.Style["width"] = "17%";
                        btnSaveForRecurring.Style["margin-right"] = "-2%";
                        btnClearForRecurring.Text = "Cancel";
                        chkRecurSelecttime.Enabled = true;
                        chkRecurSelecttime.Checked = true;
                        if (chkRecurSelecttime.Checked == true)
                        {
                            dtpRecurFromTime.Enabled = true;
                            dtpRecurToTime.Enabled = true;
                        }
                        tabBlockDays.SelectedIndex = 0;
                        rdmpBlockDays.PageViews[0].Selected = true;
                        //lblId.Text = hdnBlockdaysId.Value;
                        if(hdnFromDate.Value!="" && hdnFromDate.Value!= "&nbsp;")
                            dtpRecurFromDate.SelectedDate = Convert.ToDateTime(hdnFromDate.Value);
                        if (hdnToDate.Value != "" && hdnToDate.Value != "&nbsp;")
                            dtpRecurToDate.SelectedDate = Convert.ToDateTime(hdnToDate.Value);
                        DateTime dt = Convert.ToDateTime(hdnFromTime.Value);
                        dtpRecurFromTime.SelectedTime = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
                        DateTime dt1 = Convert.ToDateTime(hdnToTime.Value);
                        dtpRecurToTime.SelectedTime = new TimeSpan(dt1.Hour, dt1.Minute, dt1.Second);
                        break;
                    case "NON RECURSIVE":
                        btnSaveForNonRecur.Enabled = true;
                        btnCancelForNonRecur.Enabled = true;
                        btnSaveForNonRecur.Text = "Update";
                        btnSaveForNonRecur.Style["width"] = "13%";
                        btnSaveForNonRecur.Style["margin-left"] = "-16%";
                        btnCancelForNonRecur.Text = "Cancel";
                        hdnToFindSource.Value = "ToEnableNon";
                        chkNonRecurSelectTime.Enabled = true;
                        chkNonRecurSelectTime.Checked = true;
                        //lblId.Text = hdnBlockdaysId.Value;
                        if (chkNonRecurSelectTime.Checked == true)
                        {
                            dtpNonRecurFromTime.Enabled = true;
                            dtpNonRecurToTime.Enabled = true;
                        }
                        if (hdnFromDate.Value != string.Empty && hdnFromDate.Value != null && hdnFromDate.Value != "&nbsp;")
                        dtpNonRecurDate.SelectedDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnFromDate.Value)).Date;
                        txtDescription.txtDLC.Text = hdnNonRecurDescription.Value;
                        tabBlockDays.SelectedIndex = 1;
                        rdmpBlockDays.PageViews[1].Selected = true;
                        DateTime dtNonRec = Convert.ToDateTime(hdnFromTime.Value);
                        dtpNonRecurFromTime.SelectedTime = new TimeSpan(dtNonRec.Hour, dtNonRec.Minute, dtNonRec.Second);
                        DateTime dt1NonRec = Convert.ToDateTime(hdnToTime.Value);
                        dtpNonRecurToTime.SelectedTime = new TimeSpan(dt1NonRec.Hour, dt1NonRec.Minute, dt1NonRec.Second);
                        break;
                    default:
                        txtDescriptionTOF.txtDLC.Text = hdnRecDescription.Value;
                        //btnSaveTOF.Text = "Update";
                        btnSaveTOF.Enabled = true;
                        tabBlockDays.SelectedIndex = 2;
                        rdmpBlockDays.PageViews[2].Selected = true;
                        DateTime dtTOV = Convert.ToDateTime(hdnFromTime.Value);
                        dtpFromTimeTOF.SelectedTime = new TimeSpan(dtTOV.Hour, dtTOV.Minute, dtTOV.Second);
                        //dtpFromTimeTOF.Hour = hour;
                        //dtpFromTimeTOF.Minute = Minute;
                        //if (s.ToUpper() == "PM")
                        //{
                        //    dtpFromTimeTOF.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                        //}
                        //else
                        //{
                        //    dtpFromTimeTOF.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                        //}
                        DateTime dtTOV1 = Convert.ToDateTime(hdnFromTime.Value);
                        dtpToTimeTOF.SelectedTime = new TimeSpan(dtTOV1.Hour, dtTOV1.Minute, dtTOV1.Second);
                        //dtpToTimeTOF.Hour = hour1;
                        //dtpToTimeTOF.Minute = Minute1;
                        //if (s1.ToUpper() == "PM")
                        //{
                        //    dtpToTimeTOF.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                        //}
                        //else
                        //{
                        //    dtpToTimeTOF.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                        //}
                        break;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnHiddenForBlockDays_Click(object sender, EventArgs e)
        {
            try
            {
                cleartxt();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void tabGeneralQ_TabClick(object sender, RadTabStripEventArgs e)
        {
            DateTime dttime = DateTime.Now;
            if (tabBlockDays.SelectedIndex == 2)
            {
                StaticLookupManager objStaticlookupMngr = new StaticLookupManager();
                IList<StaticLookup> ObjLookup = new List<StaticLookup>();
                ObjLookup = objStaticlookupMngr.getStaticLookupByFieldName("CATEGORY OF BLOCK", "Sort_Order");
                cboTypeOfVisit.Items.Clear();
                cboTypeOfVisit.Items.Add(new RadComboBoxItem(""));
                for (int i = 0; i < ObjLookup.Count; i++)
                {
                    cboTypeOfVisit.Items.Add(new RadComboBoxItem(ObjLookup[i].Value));
                }
            }
            else if (tabBlockDays.SelectedIndex == 0)
            {
                chkSaturday.Checked = true;
                chkSunday.Checked = true;
                chkRecurSelecttime.Checked= true;
                dtpRecurFromTime.Enabled = true;
                dtpRecurToTime.Enabled = true;
                dtpRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                dtpRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
            }
            else if (tabBlockDays.SelectedIndex == 1)
            {
                chkNonRecurSelectTime.Checked = true;
                dtpNonRecurFromTime.Enabled = true;
                dtpNonRecurToTime.Enabled = true;
                dtpNonRecurFromTime.SelectedTime = new TimeSpan(dttime.Hour, dttime.Minute, dttime.Second);
                dtpNonRecurToTime.SelectedTime = new TimeSpan(dttime.AddHours(1).Hour, dttime.Minute, dttime.Second);
            }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "UnloadWaitCursor", "BlockDays_Load(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnClearForRecurring_Click(object sender, EventArgs e)
        {
            btnSaveForRecurring.Enabled = false;
        }


    }
}
