using System;
using System.Collections;
using System.Collections.Generic;
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
using AjaxControlToolkit.Design;
using AjaxControlToolkit;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Telerik.Web.Design;
using Telerik.Web.UI;
using Acurus.Capella.UI.UserControls;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;


namespace Acurus.Capella.UI
{
    public partial class frmHistoryHospitalization : System.Web.UI.Page
    {
        #region Declaration

       
        UserLookupManager objUserLookupManager = new UserLookupManager();
        IList<Encounter> ilstEncounter = new List<Encounter>();
        EncounterManager objEncounterManager = new EncounterManager();
        HospitalizationDTO HospitalDTO = null;
        HospitalizationHistoryManager objHospitalizationHistoryManager = new HospitalizationHistoryManager();
        HospitalizationHistory objHospitalizationHistory = new HospitalizationHistory();
        IList<HospitalizationHistory> HosplList = new List<HospitalizationHistory>();
        IList<HospitalizationHistory> SaveLst = new List<HospitalizationHistory>();
        IList<HospitalizationHistory> UpdateLst = new List<HospitalizationHistory>();
        IList<HospitalizationHistory> DeleteLst = new List<HospitalizationHistory>();

        HospitalizationHistoryMasterManager objHospitalizationHistoryMasterManager = new HospitalizationHistoryMasterManager();
        HospitalizationHistoryMaster objHospitalizationMasterHistory = new HospitalizationHistoryMaster();
        IList<HospitalizationHistoryMaster> HospMasterlList = new List<HospitalizationHistoryMaster>();
        IList<HospitalizationHistoryMaster> SaveMasterLst = new List<HospitalizationHistoryMaster>();
        IList<HospitalizationHistoryMaster> UpdateMasterLst = new List<HospitalizationHistoryMaster>();
        IList<HospitalizationHistoryMaster> DeleteMasterLst = new List<HospitalizationHistoryMaster>();

        

        string Space_Data = "&nbsp;";
        ulong EncounterId = 0;
        ulong HumanId = 0;
        ulong PhysicianId = 0;
        string ScreenMode = string.Empty;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            frmHistoryHospitalization _source = (frmHistoryHospitalization)sender;
            ScreenMode = _source.Page.Request.Params[0];
            EncounterId = ClientSession.EncounterId;
            HumanId = ClientSession.HumanId;
            PhysicianId = ClientSession.PhysicianId;
            if (ScreenMode == "Menu")
                chkPatientDeniesHospitalization.Visible = false;
            if (HospitalDTO != null)
                HospitalDTO.Encount = ClientSession.FillEncounterandWFObject.EncRecord;
            if (!IsPostBack)
            {
                Hiddenupdate.Value = "ADD";
                ClientSession.FlushSession();
                dtpFromDate.AssignDate(null, null, null);
                dtpToDate.AssignDate(null, null, null);
                DLC.txtDLC.Attributes.Add("onkeypress", "EnableSave(event);");
                LoadReasonforHospitalization();
                FillHospitalizationGridWithPageNavigator(true, null,false);
                dtpReadmissionDate.AssignDate(null, null, null);
                if (grdHospitalizationHistory.DataSource == null)
                {
                    grdHospitalizationHistory.DataSource = new string[] { };
                    grdHospitalizationHistory.DataBind();
                    chkPatientDeniesHospitalization.Enabled = true;
                }

                if (Session["HospitalDTO"] != null)
                {
                    HospitalDTO = (HospitalizationDTO)Session["HospitalDTO"];
                    if (HospitalDTO != null)
                    {
                        HospitalDTO.Encount = ClientSession.FillEncounterandWFObject.EncRecord;
                        Encounter objEncounter = ClientSession.FillEncounterandWFObject.EncRecord; //HospitalDTO.Encount;
                        if (objEncounter != null && objEncounter.Is_Hospitalization_Denied == "Y" )
                            chkPatientDeniesHospitalization.Checked = true;
                    }
                }
                else
                    chkPatientDeniesHospitalization.Enabled = true;

            }
            //Cap - 1437 - Code position changed
            if (chkPatientDeniesHospitalization.Checked)
            {
                DLC.Enable = false;
                pnlHospitalization.Enabled = false;
                pnlGrid.Enabled = false;
                btnAdd.Enabled = false;
                btnClearAll.Enabled = false;
                dtpToDate.Enable = false;
                dtpFromDate.Enable = false;
                txtReasonForHospitalization.Enabled = false;
                txtDischargePhysician.Enabled = false;
                lstReasonForHospitalization.Enabled = false;
                ddlReadmitted.Disabled = true;
                pbDatabase.ImageUrl = "~/Resources/Database Disable.png";
                dtpFromDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                dtpToDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                dtpReadmissionDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";

                pbClear.Enabled = false;
                pbClear.ImageUrl = "~/Resources/close_disabled.png";
            }
            else
            {
                pnlHospitalization.Enabled = true;
                pnlGrid.Enabled = true;
                lstReasonForHospitalization.Enabled = true;
                txtReasonForHospitalization.Enabled = true;
                ddlReadmitted.Disabled = false;
                btnClearAll.Enabled = true;
                txtDischargePhysician.Enabled = true;
                if (chkCurrentDate.Checked)
                {
                    dtpToDate.Enable = false;
                    dtpToDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                }
                else
                {
                    dtpToDate.Enable = true;
                }
                if (ddlReadmitted.SelectedIndex != 1)
                {
                    dtpReadmissionDate.Enable = false;
                    dtpReadmissionDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                }
                else
                {
                    dtpReadmissionDate.Enable = true;
                    dtpReadmissionDate.RadButton1.ImageUrl = "~/Resources/calenda2.bmp";
                }

                dtpFromDate.Enable = true;
            }


            DLC.txtDLC.Attributes.Add("onChange", "CCTextChanged()");
            dtpFromDate.cboDate.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboDate_SelectedIndexChanged);
            dtpFromDate.cboMonth.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboDate_SelectedIndexChanged);
            dtpFromDate.cboYear.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboDate_SelectedIndexChanged);
            dtpToDate.cboDate.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboDate_SelectedIndexChanged);
            dtpToDate.cboMonth.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboDate_SelectedIndexChanged);
            dtpToDate.cboYear.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboDate_SelectedIndexChanged);
            dtpToDate.cboDate.OnClientSelectedIndexChanged = "clbCalendar_OnDateSelecting";
            dtpToDate.cboMonth.OnClientSelectedIndexChanged = "clbCalendar_OnDateSelecting";
            dtpToDate.cboYear.OnClientSelectedIndexChanged = "clbCalendar_OnDateSelecting";
            dtpFromDate.cboDate.OnClientSelectedIndexChanged = "clbCalendar_OnDateSelecting";
            dtpFromDate.cboMonth.OnClientSelectedIndexChanged = "clbCalendar_OnDateSelecting";
            dtpFromDate.cboYear.OnClientSelectedIndexChanged = "clbCalendar_OnDateSelecting";
            dtpReadmissionDate.cboDate.OnClientSelectedIndexChanged = "clbCalendar_OnDateSelecting";
            dtpReadmissionDate.cboMonth.OnClientSelectedIndexChanged = "clbCalendar_OnDateSelecting";
            dtpReadmissionDate.cboYear.OnClientSelectedIndexChanged = "clbCalendar_OnDateSelecting";
            dtpReadmissionDate.cboDate.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboDate_SelectedIndexChanged);
            dtpReadmissionDate.cboMonth.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboDate_SelectedIndexChanged);
            dtpReadmissionDate.cboYear.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboDate_SelectedIndexChanged);

            if (Request.Form["chkCurrentDate"] != null && Request.Form["chkCurrentDate"].ToString() == "on")
                dtpToDate.Enable = false;
            else
                dtpToDate.Enable = true;

            if (ddlReadmitted.SelectedIndex != 1)
	                dtpReadmissionDate.Enable = false;
	            else
	                dtpReadmissionDate.Enable = true;

            if (!IsPostBack)
            {
                if (UIManager.is_Menu_Level_PFSH)
                {
                    this.Page.Items.Add("Title", "frmHistoryHospitalizationMenu");
                }

                if (UIManager.PFSH_OpeingFrom != "Menu")
                {
                   if (!chkPatientDeniesHospitalization.Checked && ClientSession.UserRole != "CODER" && ClientSession.UserCurrentProcess.Trim() != string.Empty)
                    {
                        pbClear.Enabled = true;
                        pbClear.ImageUrl = "~/Resources/close_small_pressed.png";

                    }
                    if (ClientSession.UserCurrentProcess.Trim().ToUpper() == "CHECK_OUT")
                    {
                        pbClear.Enabled = false;
                        pbClear.ImageUrl = "~/Resources/close_disabled.png";
                    }
                    ClientSession.processCheck = true;
                    SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                    objSecurity.ApplyUserPermissions(this.Page);
                }
                btnAdd.Enabled = false;
            }
            if (Session["HospitalDTO"] != null)
            {
                HospitalDTO = (HospitalizationDTO)Session["HospitalDTO"];

              if(HospitalDTO.HospList != null && HospitalDTO.HospList.Count > 0)
                    chkPatientDeniesHospitalization.Enabled = false;
            }
            else
                chkPatientDeniesHospitalization.Enabled = true;

            

            if (ClientSession.UserRole.ToUpper() != "PHYSICIAN" && ClientSession.UserRole.ToUpper() != "PHYSICIAN ASSISTANT")
            {
                pbDatabase.ImageUrl = "~/Resources/Database Disable.png";
                pbDatabase.Enabled = false;
            }
            if (ClientSession.UserCurrentOwner.Trim() == "UNKNOWN" || (!ClientSession.CheckUser && ClientSession.UserPermission == "R"))//if (ClientSession.UserCurrentOwner.Trim() == "UNKNOWN" || (!ClientSession.CheckUser&&ClientSession.UserPermission=="R"))
            {
                btnAdd.Enabled = false;
                btnClearAll.Enabled = false;
                lstReasonForHospitalization.Enabled = false;
                pbDatabase.Enabled = false;
                pbDatabase.ImageUrl = "~/Resources/Database Disable.png";
                dtpFromDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                dtpToDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                dtpReadmissionDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                pnlHistoryControls.Enabled = false;
                pbClear.Enabled = false;
                pbClear.ImageUrl = "~/Resources/close_disabled.png";
                DLC.Enable = false;
                if (grdHospitalizationHistory != null && grdHospitalizationHistory.Items.Count > 0)
                {
                   grdHospitalizationHistory.Columns[0].Visible = false;
                   grdHospitalizationHistory.Columns[1].Visible = false;
                }
            }
            if (ClientSession.UserCurrentProcess == "DISABLE")
            {
                ddlReadmitted.Disabled = true;
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "EndWaitCursor", "EndWaitCursor();", true);           
        }


        void clb_SelectionChanged(object sender, Telerik.Web.UI.Calendar.SelectedDatesEventArgs e)
        {
            btnAdd.Enabled = true;
        }

        void cboDate_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            btnAdd.Enabled = true;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "EnableSave", "DateSelecting();", true);
        }
        void pbDropdown_Click(object sender, ImageClickEventArgs e)
        {
            if (Hidden1.Value == "True")
                btnAdd.Enabled = true;
        }

        protected void pbDatabase_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "openAddorUpdate();", true);
            LoadReasonforHospitalization();
        }

        private void LoadReasonforHospitalization()
        {
            lstReasonForHospitalization.Items.Clear();
            string gender = string.Empty;
            if (ClientSession.PatientPaneList.Count > 0)
            {
                gender = ClientSession.PatientPaneList.Where(a => a.Human_Id == ClientSession.HumanId).ToList<PatientPane>()[0].Sex;
            }
            else
            {
                gender = Convert.ToString(ClientSession.PatientPane.Split('|')[3].Trim());
                if (gender == "M")
                {
                    gender = "MALE";
                }
                else if (gender == "F")
                {
                    gender = "FEMALE";
                }
                else 
                {
                    gender = "MALE";
                }
            }

            if (gender == "MALE")
            {
                gender = "FEMALE";
            }
            else if (gender == "FEMALE")
            {
                gender = "MALE";
            }
            else
            {
                gender = "MALE";
            }

            IList<FieldLookup> FldLook = new List<FieldLookup>();
            FldLook = objUserLookupManager.GetFieldLookupList(PhysicianId, "Reason For Hospitalization", gender, "Value").ToArray();
            if (FldLook != null)
            {
                for (int i = 0; i < FldLook.Count; i++)
                    lstReasonForHospitalization.Items.Add(new RadListBoxItem(FldLook[i].Value));
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string strtime = string.Empty;
            DateTime utc = new DateTime();
            if (hdnLocalTime.Value != string.Empty)
            {
                strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                utc = Convert.ToDateTime(strtime);
            }
            
            DateTime Birth_Date = DateTime.Now; 
            if (ClientSession.PatientPaneList.Count > 0)
            {
                Birth_Date = ClientSession.PatientPaneList.Where(a => a.Human_Id == ClientSession.HumanId).ToList<PatientPane>()[0].Birth_Date;
            }
            else
            {
                Birth_Date = Convert.ToDateTime(ClientSession.PatientPane.Split('|')[1].Trim());
            }

            if (txtReasonForHospitalization.Text.Trim() == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "txtBoxValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180606'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                txtReasonForHospitalization.Focus();
                return;
            }

            if (NotesDuplicate(DLC.txtDLC.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NotesValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180615'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

            string FromComboBoxDate = string.Empty;
            string sDate = string.Empty;
            if (dtpFromDate.cboDate.Text.Trim() != string.Empty && dtpFromDate.cboMonth.Text.Trim() == string.Empty && dtpFromDate.cboYear.Text.Trim() != string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FromDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180611'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

            else if (dtpFromDate.cboDate.Text != "" && dtpFromDate.cboMonth.Text != "" && dtpFromDate.cboYear.Text != "")
            {
                FromComboBoxDate = dtpFromDate.cboDate.Text + "-" + dtpFromDate.cboMonth.Text + "-" + dtpFromDate.cboYear.Text;
                sDate = dtpFromDate.cboDate.Text + "-" + dtpFromDate.cboMonth.Text + "-" + dtpFromDate.cboYear.Text;
            }
            else if (dtpFromDate.cboYear.Text != "" && dtpFromDate.cboMonth.Text != "" && dtpFromDate.cboDate.Text == "")
            {
                FromComboBoxDate = dtpFromDate.cboMonth.Text + "-" + dtpFromDate.cboYear.Text;
                sDate = "01-" + dtpFromDate.cboMonth.Text + "-" + dtpFromDate.cboYear.Text;
            }
            else if (dtpFromDate.cboYear.Text != "" && dtpFromDate.cboMonth.Text == "" && dtpFromDate.cboDate.Text == "")
            {
                FromComboBoxDate = dtpFromDate.cboYear.Text;
                sDate = "01-" + "Jan-" + dtpFromDate.cboYear.Text;
            }
            else if (dtpFromDate.cboYear.Text == string.Empty && dtpFromDate.cboMonth.Text != string.Empty && dtpFromDate.cboDate.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FromDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180611'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            else
            {
                FromComboBoxDate = "";
                sDate = "";
            }

            DateTime dt = new DateTime();
            if (sDate.Trim() != string.Empty)
                dt = UtilityManager.ConvertToLocal(Convert.ToDateTime(sDate));//dt = Convert.ToDateTime(sDate);

            if (sDate.Contains("-"))
            {
                string[] aryDate = sDate.TrimStart(new char[] { '-' }).TrimEnd(new char[] { '-' }).Split('-');

                if (aryDate!= null && aryDate.Length == 1)
                {
                    if (aryDate[0] != string.Empty && aryDate[0].Length < 4)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FromDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180611'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;
                    }
                }
                else if ((aryDate != null && aryDate.Length == 3 && aryDate[2] != null && aryDate[2].Length < 4) || (aryDate != null && aryDate.Length == 3 && (aryDate[0].Trim() == string.Empty || aryDate[1].Trim() == string.Empty || aryDate[2].Trim() == string.Empty)) || (aryDate != null && aryDate.Length == 2 && (aryDate[0].Trim() == string.Empty || aryDate[1].Trim() == string.Empty || aryDate[1].Length < 4)))
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FromDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180611'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
            }

            if (dt.Year != 1)
            {
                int h = dt.Date.CompareTo(utc.Date);// (UtilityManager.ConvertToLocal(Convert.ToDateTime(hdnLocalTime.Value)).Date);

                if (h > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FromDateCompareNowDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180604'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }

                if (Birth_Date.CompareTo(dt) > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FromDAteComparwBirthDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180613'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
            }

            string ToComboBoxDate = string.Empty;
            string sToDate = string.Empty;

            if (!chkCurrentDate.Checked)
            {
                if (dtpToDate.cboDate.Text.Trim() != string.Empty && dtpToDate.cboMonth.Text.Trim() == string.Empty && dtpToDate.cboYear.Text.Trim() != string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ToDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180612'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
                else if (dtpToDate.cboDate.Text != "" && dtpToDate.cboMonth.Text != "" && dtpToDate.cboYear.Text != "")
                {
                    ToComboBoxDate = dtpToDate.cboDate.Text + "-" + dtpToDate.cboMonth.Text + "-" + dtpToDate.cboYear.Text;
                    sToDate = dtpToDate.cboDate.Text + "-" + dtpToDate.cboMonth.Text + "-" + dtpToDate.cboYear.Text;
                }
                else if (dtpToDate.cboYear.Text != "" && dtpToDate.cboMonth.Text != "" && dtpToDate.cboDate.Text == "")
                {
                    ToComboBoxDate = dtpToDate.cboMonth.Text + "-" + dtpToDate.cboYear.Text;
                    sToDate = "01-" + dtpToDate.cboMonth.Text + "-" + dtpToDate.cboYear.Text;
                }
                else if (dtpToDate.cboYear.Text != "" && dtpToDate.cboMonth.Text == "" && dtpToDate.cboDate.Text == "")
                {
                    ToComboBoxDate = dtpToDate.cboYear.Text;
                    sToDate = "01-" + "Jan-" + dtpToDate.cboYear.Text;
                }

                else if (dtpToDate.cboYear.Text == string.Empty && dtpToDate.cboMonth.Text != string.Empty && dtpToDate.cboDate.Text == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ToDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180612'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
                else
                {
                    ToComboBoxDate = "";
                    sToDate = "";
                }
            }

            if (!chkCurrentDate.Checked && sToDate.Trim() != string.Empty)
            {

                DateTime dtToDate = new DateTime();
                if (sToDate.Trim() != string.Empty)
                    dtToDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(sToDate));//dtToDate = Convert.ToDateTime(sToDate);

                string[] aryToDate = sToDate.TrimStart(new char[] { '-' }).TrimEnd(new char[] { '-' }).Split('-');

                if (aryToDate.Length == 1)
                {
                    if (aryToDate != null && aryToDate[0] != string.Empty && aryToDate[0].Length < 4)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ToDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180612'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;
                    }
                }
                else if ((aryToDate != null && aryToDate.Length == 3 && aryToDate[2] != null && aryToDate[2].Length < 4) || (aryToDate != null && aryToDate.Length == 3 && (aryToDate[0].Trim() == string.Empty || aryToDate[1].Trim() == string.Empty || aryToDate[2].Trim() == string.Empty)) || (aryToDate != null && aryToDate.Length == 2 && (aryToDate[0].Trim() == string.Empty || aryToDate[1].Trim() == string.Empty || aryToDate[1].Length < 4)))
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ToDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180612'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }


                if (dtToDate.Year != 1)
                {
                   
                    int h1 = dtToDate.Date.CompareTo(utc.Date);
                    if (h1 > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ToDateCompareNowDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180605'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;
                    }

                    if (Birth_Date.CompareTo(dtToDate.Date) > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ToDateCompareBirthDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180614'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;
                    }

                    if (dt.Date.CompareTo(dtToDate.Date) > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FromCompareToDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180609'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;
                    }
                }
            }

            string ReadmissionDate = string.Empty;
            string sPreviousFromDate = sDate;
	            sDate = string.Empty;
	            if (dtpReadmissionDate.cboDate.Text.Trim() != string.Empty && dtpReadmissionDate.cboMonth.Text.Trim() == string.Empty && dtpReadmissionDate.cboYear.Text.Trim() != string.Empty)
	            {
	                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ReadmissionDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180616'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
	                return;
	            }
			
	            else if (dtpReadmissionDate.cboDate.Text != "" && dtpReadmissionDate.cboMonth.Text != "" && dtpReadmissionDate.cboYear.Text != "")
	            {
	                ReadmissionDate = dtpReadmissionDate.cboDate.Text + "-" + dtpReadmissionDate.cboMonth.Text + "-" + dtpReadmissionDate.cboYear.Text;
	                sDate = dtpReadmissionDate.cboDate.Text + "-" + dtpReadmissionDate.cboMonth.Text + "-" + dtpReadmissionDate.cboYear.Text;
	            }
	            else if (dtpReadmissionDate.cboYear.Text != "" && dtpReadmissionDate.cboMonth.Text != "" && dtpReadmissionDate.cboDate.Text == "")
	            {
	                ReadmissionDate = dtpReadmissionDate.cboMonth.Text + "-" + dtpReadmissionDate.cboYear.Text;
	                sDate = "01-" + dtpReadmissionDate.cboMonth.Text + "-" + dtpReadmissionDate.cboYear.Text;
	            }
	            else if (dtpReadmissionDate.cboYear.Text != "" && dtpReadmissionDate.cboMonth.Text == "" && dtpReadmissionDate.cboDate.Text == "")
	            {
	                ReadmissionDate = dtpReadmissionDate.cboYear.Text;
	                sDate = "01-" + "Jan-" + dtpReadmissionDate.cboYear.Text;
	            }
	            else if (dtpReadmissionDate.cboYear.Text == string.Empty && dtpReadmissionDate.cboMonth.Text != string.Empty && dtpReadmissionDate.cboDate.Text == string.Empty)
	            {
	                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ReadmissionDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180616'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
	                return;
	            }
	            else
	            {
	                ReadmissionDate = "";
	                sDate = "";
	            }
	
	            dt = new DateTime();
	            if (sDate.Trim() != string.Empty)
	                dt = UtilityManager.ConvertToLocal(Convert.ToDateTime(sDate));//dt = Convert.ToDateTime(sDate);

	            if (sDate.Contains("-"))
	            {
	                string[] aryDate = sDate.TrimStart(new char[] { '-' }).TrimEnd(new char[] { '-' }).Split('-');
	
	                if (aryDate.Length == 1)
	                {
                        if (aryDate != null && aryDate[0] != string.Empty && aryDate[0].Length < 4)
	                    {
	                       
	                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ReadmissionDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180616'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
	                        return;
	                    }
	                }
	                else if ((aryDate != null && aryDate.Length == 3 && aryDate[2] != null && aryDate[2].Length < 4) || (aryDate != null && aryDate.Length == 3 && (aryDate[0].Trim() == string.Empty || aryDate[1].Trim() == string.Empty || aryDate[2].Trim() == string.Empty)) || (aryDate != null && aryDate.Length == 2 && (aryDate[0].Trim() == string.Empty || aryDate[1].Trim() == string.Empty || aryDate[1].Length < 4)))
	                {
	                   
	                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ReadmissionDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180616'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
	                    return;
	                }
	            }
	
	            if (dt.Year != 1)
	            {
	             
	                int h = dt.Date.CompareTo(utc.Date);
	                if (h > 0)
	                {
	                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ReadmissionCompareNowDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180618'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
	                    return;
	                }
                    if (sPreviousFromDate != string.Empty)
                    {
                        DateTime dtFromDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(sPreviousFromDate));
                        h = dt.Date.CompareTo(dtFromDate);
                        if (h < 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ReadmissionCompareNowDateValidation", "PFSH_SaveUnsuccessful();DisplayErrorMessage('180620'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            return;
                        }
                    }
	                if (sToDate != string.Empty)
	                {
	                    DateTime dtToDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(sToDate));
	                    h = dt.Date.CompareTo(dtToDate.Date);
	                    if (h < 0)
	                    {
	                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ReadmissionCompareNowDateValidation", "PFSH_SaveUnsuccessful();DisplayErrorMessage('180619'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
	                        return;
	                    }
	                }
	                else if (chkCurrentDate.Checked)
	                {
	                    h = DateTime.UtcNow.Date.CompareTo(dt);
	                    if (h < 0)
	                    {
	                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ReadmissionCompareNowDateValidation", "PFSH_SaveUnsuccessful();DisplayErrorMessage('180619'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
	                        return;
	                    }
	                }
	                if (Birth_Date.CompareTo(dt) > 0)
	                {
	                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ReadmissionComparwBirthDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none';PFSH_SaveUnsuccessful();DisplayErrorMessage('180617'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;
	                }
	            }

        

            if (string.Compare(Convert.ToString(Hiddenupdate.Value), "ADD", true) == 0)
            {
                LoadObject();

                if (ScreenMode == "Menu")
                {
                    objHospitalizationMasterHistory.From_Date = FromComboBoxDate;
                    if (chkCurrentDate.Checked)
                        objHospitalizationMasterHistory.To_Date = "Current";
                    else
                        objHospitalizationMasterHistory.To_Date = ToComboBoxDate;

                    objHospitalizationMasterHistory.Readmission_Date = ReadmissionDate;
                    if (ddlReadmitted.Value == "Yes")
                        objHospitalizationMasterHistory.Is_Readmitted = "Y";
                    else if (ddlReadmitted.Value == "No")
                        objHospitalizationMasterHistory.Is_Readmitted = "N";
                    objHospitalizationMasterHistory.Created_By = ClientSession.UserName;
                    objHospitalizationMasterHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    SaveMasterLst.Add(objHospitalizationMasterHistory);
                    if (HospitalDTO != null)
                    {
                        if (HospitalDTO.HospList != null && HospitalDTO.HospList.Count > 0)
                        {
                        foreach (HospitalizationHistory item in HospitalDTO.HospList)
                        {
                            if (item.Encounter_Id != ClientSession.EncounterId)
                            {
                                HospitalizationHistory obj = new HospitalizationHistory();
                                item.Id = 0;
                                obj.Human_ID = ClientSession.HumanId;
                                obj.Encounter_Id = ClientSession.EncounterId;
                                obj.From_Date = item.From_Date;
                                obj.To_Date = item.To_Date;
                                obj.Reason_For_Hospitalization = item.Reason_For_Hospitalization;
                                obj.Hospitalization_Notes = item.Hospitalization_Notes;
                                obj.Created_By = ClientSession.UserName;
                                obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                SaveLst.Add(obj);
                            }
                        }
                        }

                    }
                }
                else if (ScreenMode == "Queue")
                {
                    objHospitalizationHistory.From_Date = FromComboBoxDate;
                    if (chkCurrentDate.Checked)
                        objHospitalizationHistory.To_Date = "Current";
                    else
                        objHospitalizationHistory.To_Date = ToComboBoxDate;

                    objHospitalizationHistory.Readmission_Date = ReadmissionDate;
                    if (ddlReadmitted.Value == "Yes")
                        objHospitalizationHistory.Is_Readmitted = "Y";
                    else if (ddlReadmitted.Value == "No")
                        objHospitalizationHistory.Is_Readmitted = "N";
                    objHospitalizationHistory.Encounter_Id = ClientSession.EncounterId;
                    objHospitalizationHistory.Created_By = ClientSession.UserName;
                    objHospitalizationHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    SaveLst.Add(objHospitalizationHistory);
                    if (HospitalDTO.HospList != null && HospitalDTO.HospList.Count > 0)
                    {
                        foreach (HospitalizationHistory item in HospitalDTO.HospList)
                        {
                            if (item.Encounter_Id != ClientSession.EncounterId)
                            {
                                HospitalizationHistory obj = new HospitalizationHistory();
                                item.Id = 0;
                                obj.Human_ID = ClientSession.HumanId;
                                obj.Encounter_Id = ClientSession.EncounterId;
                                obj.From_Date = item.From_Date;
                                obj.To_Date = item.To_Date;
                                obj.Reason_For_Hospitalization = item.Reason_For_Hospitalization;
                                obj.Hospitalization_Notes = item.Hospitalization_Notes;
                                obj.Created_By = ClientSession.UserName;
                                obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                SaveLst.Add(obj);
                            }
                        }
                    }
                }
                if (Session["LoadHospitalizationMasterList"]!= null)
                {
                    IList<HospitalizationHistoryMaster> _loadHospMasterList = new List<HospitalizationHistoryMaster>();
                    _loadHospMasterList = (IList<HospitalizationHistoryMaster>)Session["LoadHospitalizationMasterList"];
                    if (_loadHospMasterList.Count > 0)
                    {
                        foreach (HospitalizationHistoryMaster item in _loadHospMasterList)
                        {
                            if (ScreenMode == "Queue")
                            {
                                HospitalizationHistory obj = new HospitalizationHistory();
                                obj.Reason_For_Hospitalization = item.Reason_For_Hospitalization;
                                obj.Hospitalization_Notes = item.Hospitalization_Notes;
                                obj.Discharge_Physician = item.Discharge_Physician;
                                obj.Version = item.Version;
                                obj.From_Date = item.From_Date; ;
                                obj.To_Date = item.To_Date;
                                obj.Readmission_Date = item.Readmission_Date;
                                obj.Is_Readmitted = item.Is_Readmitted;
                                obj.Encounter_Id = ClientSession.EncounterId;
                                obj.Human_ID = item.Human_ID;
                                obj.Hospitalization_History_Master_ID = item.Id;
                                obj.Created_By = ClientSession.UserName;
                                obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                SaveLst.Add(obj);
                            }

                        }
                    }

                }
                
            }
            else
            {
                IList<ulong> lstId = new List<ulong>();
                ulong UpdateId = 0;
                if (Session["UpdateId"] != null)
                {
                    UpdateId = Convert.ToUInt32(Session["UpdateId"]);
                }
                if ((IList<HospitalizationHistoryMaster>)Session["LoadHospitalizationMasterList"] == null)
                {

                    if (ScreenMode == "Menu")
                {
                    if (!lstId.Any(a => a.ToString() == UpdateId.ToString()))
                    {
                        objHospitalizationMasterHistory = HospitalDTO.HospMasterList.Where(a => a.Id == UpdateId).ToList<HospitalizationHistoryMaster>()[0];
                        LoadObject();
                        objHospitalizationMasterHistory.From_Date = FromComboBoxDate;
                        if (chkCurrentDate.Checked)
                            objHospitalizationMasterHistory.To_Date = "Current";
                        else
                            objHospitalizationMasterHistory.To_Date = ToComboBoxDate;

                        objHospitalizationMasterHistory.Readmission_Date = ReadmissionDate;
                        if (ddlReadmitted.Value == "Yes")
                            objHospitalizationMasterHistory.Is_Readmitted = "Y";
                        else if (ddlReadmitted.Value == "No")
                            objHospitalizationMasterHistory.Is_Readmitted = "N";
                        objHospitalizationMasterHistory.Version = HospitalDTO.HospMasterList.Where(a => a.Id == UpdateId).ToList<HospitalizationHistoryMaster>()[0].Version;
                        objHospitalizationMasterHistory.Created_By = HospitalDTO.HospMasterList.Where(a => a.Id == UpdateId).ToList<HospitalizationHistoryMaster>()[0].Created_By;
                        objHospitalizationMasterHistory.Created_Date_And_Time = HospitalDTO.HospMasterList.Where(a => a.Id == UpdateId).ToList<HospitalizationHistoryMaster>()[0].Created_Date_And_Time;
                        objHospitalizationMasterHistory.Modified_By = ClientSession.UserName;
                        objHospitalizationMasterHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        UpdateMasterLst.Add(objHospitalizationMasterHistory);
                    }
                }
                    else if (ScreenMode == "Queue")
                {
                    if (HospitalDTO.HospList != null && HospitalDTO.HospList.Count > 0)
                    {
                        foreach (HospitalizationHistory item in HospitalDTO.HospList)
                        {
                            HospitalizationHistory obj = new HospitalizationHistory();
                            if (item.Encounter_Id != ClientSession.EncounterId)
                            {
                                lstId.Add(item.Id);
                                if (item.Id == UpdateId)
                                {
                                    obj.Hospitalization_Notes = DLC.txtDLC.Text;
                                    obj.From_Date = objHospitalizationHistory.From_Date;
                                    obj.To_Date = objHospitalizationHistory.To_Date;
                                    obj.Reason_For_Hospitalization = txtReasonForHospitalization.Text;
                                    obj.Discharge_Physician = txtDischargePhysician.Text;
                                    obj.Is_Readmitted = objHospitalizationHistory.Is_Readmitted;
                                    obj.Readmission_Date = objHospitalizationHistory.Readmission_Date;
                                }
                                else
                                {
                                    obj.Hospitalization_Notes = item.Hospitalization_Notes;
                                    obj.From_Date = item.From_Date;
                                    obj.To_Date = item.To_Date;
                                    obj.Reason_For_Hospitalization = item.Reason_For_Hospitalization;
                                    obj.Discharge_Physician = item.Discharge_Physician;
                                    obj.Is_Readmitted = item.Is_Readmitted;
                                    obj.Readmission_Date = item.Readmission_Date;
                                }
                                obj.Human_ID = ClientSession.HumanId;
                                obj.Encounter_Id = ClientSession.EncounterId;
                                obj.Created_By = ClientSession.UserName;
                                obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                SaveLst.Add(obj);
                            }
                        }
                    }
                    if (!lstId.Any(a => a.ToString() == UpdateId.ToString()))
                    {
                        objHospitalizationHistory = HospitalDTO.HospList.Where(a => a.Id == UpdateId).ToList<HospitalizationHistory>()[0];
                        LoadObject();
                        objHospitalizationHistory.From_Date = FromComboBoxDate;
                        if (chkCurrentDate.Checked)
                            objHospitalizationHistory.To_Date = "Current";
                        else
                            objHospitalizationHistory.To_Date = ToComboBoxDate;

                        objHospitalizationHistory.Readmission_Date = ReadmissionDate;
                        if (ddlReadmitted.Value == "Yes")
                            objHospitalizationHistory.Is_Readmitted = "Y";
                        else if (ddlReadmitted.Value == "No")
                            objHospitalizationHistory.Is_Readmitted = "N";
                        objHospitalizationHistory.Version = HospitalDTO.HospList.Where(a => a.Id == UpdateId).ToList<HospitalizationHistory>()[0].Version;
                        objHospitalizationHistory.Encounter_Id = ClientSession.EncounterId;
                        objHospitalizationHistory.Created_By = HospitalDTO.HospList.Where(a => a.Id == UpdateId).ToList<HospitalizationHistory>()[0].Created_By;
                        objHospitalizationHistory.Created_Date_And_Time = HospitalDTO.HospList.Where(a => a.Id == UpdateId).ToList<HospitalizationHistory>()[0].Created_Date_And_Time;
                        objHospitalizationHistory.Modified_By = ClientSession.UserName;
                        objHospitalizationHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        UpdateLst.Add(objHospitalizationHistory);
                    }
                }
            }
                else
                {
                    if ((ScreenMode == "Queue") && ((IList<HospitalizationHistoryMaster>)Session["LoadHospitalizationMasterList"]).Count > 0)
                    {
                        if (!lstId.Any(a => a.ToString() == UpdateId.ToString()))
                        {
                            objHospitalizationMasterHistory = HospitalDTO.HospMasterList.Where(a => a.Id == UpdateId).ToList<HospitalizationHistoryMaster>()[0]; 
                            LoadObject();
                            objHospitalizationMasterHistory.From_Date = FromComboBoxDate;
                            if (chkCurrentDate.Checked)
                                objHospitalizationMasterHistory.To_Date = "Current";
                            else
                                objHospitalizationMasterHistory.To_Date = ToComboBoxDate;

                            objHospitalizationMasterHistory.Readmission_Date = ReadmissionDate;
                            if (ddlReadmitted.Value == "Yes")
                                objHospitalizationMasterHistory.Is_Readmitted = "Y";
                            else if (ddlReadmitted.Value == "No")
                                objHospitalizationMasterHistory.Is_Readmitted = "N";
                            objHospitalizationMasterHistory.Version = HospitalDTO.HospMasterList.Where(a => a.Id == UpdateId).ToList<HospitalizationHistoryMaster>()[0].Version;
                            objHospitalizationMasterHistory.Created_By = HospitalDTO.HospMasterList.Where(a => a.Id == UpdateId).ToList<HospitalizationHistoryMaster>()[0].Created_By;
                            objHospitalizationMasterHistory.Created_Date_And_Time = HospitalDTO.HospMasterList.Where(a => a.Id == UpdateId).ToList<HospitalizationHistoryMaster>()[0].Created_Date_And_Time;
                            objHospitalizationMasterHistory.Modified_By = ClientSession.UserName;
                            objHospitalizationMasterHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            UpdateMasterLst.Add(objHospitalizationMasterHistory);
                        }
                    }

                }

               System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
                if(text1!= null)
                text1.InnerText = "A";
                System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdditionalword");
                if (text2 != null)
                text2.InnerText = "dd";
                System.Web.UI.HtmlControls.HtmlGenericControl textClear = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                if (textClear != null)
                textClear.InnerText = "C";
                System.Web.UI.HtmlControls.HtmlGenericControl textClearAdd = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                if (textClearAdd != null)
                textClearAdd.InnerText = "lear All";
                btnAdd.AccessKey = "A";
                Hiddenupdate.Value = "ADD";
            }

            HospitalizationDTO HosplLst = (HospitalizationDTO)Session["HospitalDTO"];
            
                if ((IList<HospitalizationHistoryMaster>)Session["LoadHospitalizationMasterList"] == null)
                {
                    if (ScreenMode == "Menu")
                    {
                        if (HosplLst != null)
                        HospMasterlList = HosplLst.HospMasterList;
                        Session["HospitalDTO"] = objHospitalizationHistoryMasterManager.HospitalHistorySaveUpdateDelete(HospMasterlList, SaveMasterLst, UpdateMasterLst, DeleteMasterLst, HumanId, string.Empty);
                    }
                    else if (ScreenMode == "Queue")
                    {
                        if (HosplLst != null)
                        HosplList = HosplLst.HospList;
                        Session["HospitalDTO"] = objHospitalizationHistoryManager.HospitalHistorySaveUpdateDelete(HosplList, SaveLst, UpdateLst, DeleteLst, HumanId, string.Empty, ClientSession.EncounterId);
                    }
                }
                else if (((IList<HospitalizationHistoryMaster>)Session["LoadHospitalizationMasterList"]).Count > 0)
                {

                    if (ScreenMode == "Queue")
                    {
                        HospitalizationDTO hospTempList = (HospitalizationDTO)Session["HospitalDTO"];
                        if (hospTempList != null)
                        {
                            if (SaveLst.Count > 0 || UpdateLst.Count > 0 || DeleteLst.Count > 0)
                            {
                                HosplList = hospTempList.HospList;
                                Session["HospitalDTO"] = objHospitalizationHistoryManager.HospitalHistorySaveUpdateDelete(HosplList, SaveLst, UpdateLst, DeleteLst, HumanId, string.Empty, ClientSession.EncounterId);
                                Session["LoadHospitalizationMasterList"] = null;
                            }
                            else if (SaveMasterLst.Count > 0 || UpdateMasterLst.Count > 0 || DeleteMasterLst.Count > 0)
                            {
                                HospMasterlList = hospTempList.HospMasterList;
                                Session["HospitalDTO"] = objHospitalizationHistoryMasterManager.HospitalHistorySaveUpdateDelete(HospMasterlList, SaveMasterLst, UpdateMasterLst, DeleteMasterLst, HumanId, string.Empty);
                            }
                        }
                        
                    }

                }

            FillHospitalizationGridWithPageNavigator(true, (HospitalizationDTO)Session["HospitalDTO"], false);
            ClearFields();
            btnAdd.Enabled = false;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "SavedSuccessfully(); DisplayErrorMessage('180601');EnablePFSH(" + ClientSession.EncounterId + "); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            ClientSession.bPFSHVerified = false;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            dtpFromDate.clbCalendar.SelectedDate = DateTime.Now;
            dtpToDate.clbCalendar.SelectedDate = DateTime.Now;
            dtpReadmissionDate.clbCalendar.SelectedDate = DateTime.Now;
            UIManager.IsPFSHVerified = true;
        }

        public void LoadObject()
        {
            if ((IList<HospitalizationHistoryMaster>)Session["LoadHospitalizationMasterList"] == null)
            {
                if (ScreenMode == "Menu")
                {
                    objHospitalizationMasterHistory.Human_ID = ClientSession.HumanId;
                    objHospitalizationMasterHistory.Reason_For_Hospitalization = txtReasonForHospitalization.Text;
                    objHospitalizationMasterHistory.Hospitalization_Notes = DLC.txtDLC.Text;
                    objHospitalizationMasterHistory.Discharge_Physician = txtDischargePhysician.Text;
                    objHospitalizationMasterHistory.Is_Deleted = "N";
                }
                else
                {
                    objHospitalizationHistory.Human_ID = ClientSession.HumanId;
                    objHospitalizationHistory.Reason_For_Hospitalization = txtReasonForHospitalization.Text;
                    objHospitalizationHistory.Hospitalization_Notes = DLC.txtDLC.Text;
                    objHospitalizationHistory.Discharge_Physician = txtDischargePhysician.Text;
                }
            }
             else if (((IList<HospitalizationHistoryMaster>)Session["LoadHospitalizationMasterList"]).Count > 0)
            {
                if (ScreenMode == "Queue")
                {
                    if (string.Compare(Convert.ToString(Hiddenupdate.Value), "ADD", true) == 0)
                    {
                        objHospitalizationHistory.Human_ID = ClientSession.HumanId;
                        objHospitalizationHistory.Reason_For_Hospitalization = txtReasonForHospitalization.Text;
                        objHospitalizationHistory.Hospitalization_Notes = DLC.txtDLC.Text;
                        objHospitalizationHistory.Discharge_Physician = txtDischargePhysician.Text;
                    }
                    else
                    {
                        objHospitalizationMasterHistory.Human_ID = ClientSession.HumanId;
                        objHospitalizationMasterHistory.Reason_For_Hospitalization = txtReasonForHospitalization.Text;
                        objHospitalizationMasterHistory.Hospitalization_Notes = DLC.txtDLC.Text;
                        objHospitalizationMasterHistory.Discharge_Physician = txtDischargePhysician.Text;
                        objHospitalizationMasterHistory.Is_Deleted = "N";
                    }
                    
                }
                
            }

        }

        public void FillHospitalizationGrid(HospitalizationDTO HospitalDTO)
        {

            if (HospitalDTO != null && HospitalDTO.Encount != null && HospitalDTO.Encount.Is_Hospitalization_Denied == "Y")
            {
                chkPatientDeniesHospitalization.Checked = true;
                chkPatientDeniesHospitalization.Enabled = false;
            }
            else
                chkPatientDeniesHospitalization.Checked = false;


            if (HospitalDTO != null)
            {
                chkPatientDeniesHospitalization.Enabled = false;
                HospitalizationDTO ObjHospDto = new HospitalizationDTO();
                grdHospitalizationHistory.DataSource = null;
                DataTable objDataTable = new DataTable();
                DataColumn objDataColumn = new DataColumn();
                objDataColumn = new DataColumn("ReasonForHospitalization", typeof(string));
                objDataTable.Columns.Add(objDataColumn);
                objDataColumn = new DataColumn("FromDate", typeof(string));
                objDataTable.Columns.Add(objDataColumn);
                objDataColumn = new DataColumn("ToDate", typeof(string));
                objDataTable.Columns.Add(objDataColumn);
                objDataColumn = new DataColumn("HospitalizationNotes", typeof(string));
                objDataTable.Columns.Add(objDataColumn);
                objDataColumn = new DataColumn("Id", typeof(string));
                objDataTable.Columns.Add(objDataColumn);
                objDataColumn = new DataColumn("DischargePhysician", typeof(string));
                objDataTable.Columns.Add(objDataColumn);
                objDataColumn = new DataColumn("Readmitted", typeof(string));
                objDataTable.Columns.Add(objDataColumn);
                objDataColumn = new DataColumn("Readmitted_Date", typeof(string));
                objDataTable.Columns.Add(objDataColumn);
                if (ScreenMode == "Menu")
                {
                    if (HospitalDTO.HospMasterList != null && HospitalDTO.HospMasterList.Count > 0)
                    {
                        FillGridFromMaster(HospitalDTO);
                    }
                    else
                    {
                        chkPatientDeniesHospitalization.Enabled = true;
                        if (grdHospitalizationHistory.DataSource == null)
                        {
                            grdHospitalizationHistory.DataSource = new string[] { };
                            grdHospitalizationHistory.DataBind();
                            chkPatientDeniesHospitalization.Enabled = true;
                        }
                    }
                }
                else if (ScreenMode == "Queue")
                {
                    if (HospitalDTO.HospList != null && HospitalDTO.HospList.Count > 0)
                    {
                        ObjHospDto.HospList = new List<HospitalizationHistory>();
                        foreach (HospitalizationHistory obj in HospitalDTO.HospList)
                        {
                            if (obj.From_Date.Trim() != string.Empty)
                                switch (obj.From_Date.Split('-').Count())
                                {
                                    case 1:
                                        ObjHospDto.dictHospFromDate.Add(obj.Id, Convert.ToDateTime("01-Jan-" + obj.From_Date));
                                        break;
                                    case 2:
                                        ObjHospDto.dictHospFromDate.Add(obj.Id, Convert.ToDateTime("01-" + obj.From_Date));
                                        break;
                                    case 3:
                                        ObjHospDto.dictHospFromDate.Add(obj.Id, Convert.ToDateTime(obj.From_Date));
                                        break;
                                    default:
                                        break;
                                }
                            else
                                ObjHospDto.dictHospFromDate.Add(obj.Id, DateTime.MinValue);
                        }
                        ObjHospDto.dictHospFromDate = (from keyval in ObjHospDto.dictHospFromDate orderby keyval.Value descending select keyval).ToDictionary(keyval => keyval.Key, keyval => keyval.Value);

                        foreach (KeyValuePair<ulong, DateTime> item in ObjHospDto.dictHospFromDate)
                        {
                            if (item.Value != DateTime.MinValue)
                            {
                                ObjHospDto.HospList.Add((from ob in HospitalDTO.HospList where ob.Id == item.Key select ob).ToList<HospitalizationHistory>()[0]);
                            }
                        }
                        ObjHospDto.dictHospFromDate = (from keyval in ObjHospDto.dictHospFromDate where keyval.Value == DateTime.MinValue select keyval).ToDictionary(keyval => keyval.Key, keyval => keyval.Value);
                        if (ObjHospDto.dictHospFromDate.Count > 0)
                        {
                            for (int i = 0; i < ObjHospDto.dictHospFromDate.Count; i++)
                            {
                                HospitalizationHistory objSurDate = (from objSurg in HospitalDTO.HospList where ObjHospDto.dictHospFromDate.Keys.ElementAt(i) == objSurg.Id select objSurg).ToList<HospitalizationHistory>()[0];
                                if (DateTime.Compare(objSurDate.Modified_Date_And_Time, objSurDate.Created_Date_And_Time) >= 0)
                                    ObjHospDto.dictHospFromDate[ObjHospDto.dictHospFromDate.Keys.ElementAt(i)] = objSurDate.Modified_Date_And_Time;
                                else
                                    ObjHospDto.dictHospFromDate[ObjHospDto.dictHospFromDate.Keys.ElementAt(i)] = objSurDate.Created_Date_And_Time;
                            }
                        }

                        ObjHospDto.dictHospFromDate = (from keyval in ObjHospDto.dictHospFromDate orderby keyval.Value descending select keyval).ToDictionary(keyval => keyval.Key, keyval => keyval.Value);

                        foreach (KeyValuePair<ulong, DateTime> item in ObjHospDto.dictHospFromDate)
                            ObjHospDto.HospList.Add((from ob in HospitalDTO.HospList where ob.Id == item.Key select ob).ToList<HospitalizationHistory>()[0]);

                        for (int i = 0; i < ObjHospDto.HospList.Count; i++)
                        {
                            DataRow objDataRow = objDataTable.NewRow();
                            objDataRow["ReasonForHospitalization"] = ObjHospDto.HospList[i].Reason_For_Hospitalization;
                            objDataRow["FromDate"] = ObjHospDto.HospList[i].From_Date;
                            objDataRow["ToDate"] = ObjHospDto.HospList[i].To_Date;
                            objDataRow["HospitalizationNotes"] = ObjHospDto.HospList[i].Hospitalization_Notes;
                            objDataRow["Id"] = ObjHospDto.HospList[i].Id;
                            objDataRow["DischargePhysician"] = ObjHospDto.HospList[i].Discharge_Physician;
                            objDataRow["Readmitted"] = ObjHospDto.HospList[i].Is_Readmitted;
                            objDataRow["Readmitted_Date"] = ObjHospDto.HospList[i].Readmission_Date;
                            objDataTable.Rows.Add(objDataRow);
                        }
                        grdHospitalizationHistory.DataSource = objDataTable;
                        grdHospitalizationHistory.DataBind();
                    }
                    else if (HospitalDTO.HospMasterList != null && HospitalDTO.HospMasterList.Count > 0)
                    {
                        FillGridFromMaster(HospitalDTO);
                    }
                    else
                    {
                        chkPatientDeniesHospitalization.Enabled = true;
                        if (grdHospitalizationHistory.DataSource == null)
                        {
                            grdHospitalizationHistory.DataSource = new string[] { };
                            grdHospitalizationHistory.DataBind();
                            chkPatientDeniesHospitalization.Enabled = true;
                        }
                    }
                }
               
            }
            
        }

        public void ClearFields()
        {
            txtReasonForHospitalization.Text = string.Empty;
            txtDischargePhysician.Text = string.Empty;
            DLC.txtDLC.Text = string.Empty;
            chkCurrentDate.Checked = false;
            btnAdd.Text = "Add";
            btnClearAll.Text = "Clear All";
            dtpFromDate.Enable = true;
            dtpToDate.Enable = true;
            dtpFromDate.AssignDate(null, null, null);
            dtpFromDate.cboDate.SelectedIndex = 0;
            dtpFromDate.cboMonth.SelectedIndex = 0;
            dtpFromDate.cboYear.SelectedIndex = 0;
            dtpToDate.AssignDate(null, null, null);
            dtpToDate.cboDate.SelectedIndex = 0;
            dtpToDate.cboMonth.SelectedIndex = 0;
            dtpToDate.cboYear.SelectedIndex = 0;
            dtpReadmissionDate.AssignDate(null, null, null);
            dtpReadmissionDate.cboDate.SelectedIndex = 0;
            dtpReadmissionDate.cboMonth.SelectedIndex = 0;
            dtpReadmissionDate.cboYear.SelectedIndex = 0;
            ddlReadmitted.SelectedIndex = 0;
        }

        protected void grdHospitalizationHistory_ItemCommand(object sender, GridCommandEventArgs e)
        {
            GridItem Item = grdHospitalizationHistory.Items[Convert.ToUInt16(e.CommandArgument)];
            if (e.CommandName.Trim() == "Edt")
            {
                txtReasonForHospitalization.Text = string.Empty;
                chkCurrentDate.Checked = false;
                txtReasonForHospitalization.Text = (Item.Cells[4].Text.Trim() == Space_Data) ? string.Empty : Item.Cells[4].Text;
                txtDischargePhysician.Text = string.Empty;
	                txtDischargePhysician.Text = (Item.Cells[9].Text.Trim() == Space_Data) ? string.Empty : Item.Cells[9].Text;
	                if (Item.Cells[10].Text.Trim() == "Y")
                {
                    ddlReadmitted.SelectedIndex = 1;
                    //CAP - 1500
                    dtpReadmissionDate.Enable = true;
                    dtpReadmissionDate.RadButton1.ImageUrl = "~/Resources/calenda2.bmp";
                }	                    
	                else if (Item.Cells[10].Text.Trim() == "N")
	                    ddlReadmitted.SelectedIndex = 2;
	                else
	                    ddlReadmitted.SelectedIndex = 0;
                dtpToDate.Enable = true;

                dtpFromDate.cboDate.ClearSelection();
                dtpFromDate.cboMonth.ClearSelection();
                dtpFromDate.cboYear.ClearSelection();

                dtpToDate.cboDate.ClearSelection();
                dtpToDate.cboMonth.ClearSelection();
                dtpToDate.cboYear.ClearSelection();

                dtpReadmissionDate.cboDate.ClearSelection();
                dtpReadmissionDate.cboMonth.ClearSelection();
                dtpReadmissionDate.cboYear.ClearSelection();

                if (Item.Cells[5].Text.Trim() != string.Empty && Item.Cells[5].Text.Trim() != Space_Data)
                {

                    string[] Date = Item.Cells[5].Text.Split('-');
                    if (Date != null)
                    {
                        if (Date.Length == 3)
                        {
                            dtpFromDate.AssignDate(Date[2], Date[1], Date[0]);
                            dtpFromDate.cboYear.SelectedIndex = dtpFromDate.cboYear.Items.FindItemIndexByText(Date[2]);
                            dtpFromDate.cboMonth.SelectedIndex = dtpFromDate.cboMonth.Items.FindItemIndexByText(Date[1]);
                            dtpFromDate.cboDate.SelectedIndex = dtpFromDate.cboDate.Items.FindItemIndexByText(Date[0]);
                        }
                        else if (Date.Length == 2)
                        {

                            dtpFromDate.AssignDate(Date[1], Date[0], null);
                            dtpFromDate.cboYear.SelectedIndex = dtpFromDate.cboYear.Items.FindItemIndexByText(Date[1]);
                            dtpFromDate.cboMonth.SelectedIndex = dtpFromDate.cboMonth.Items.FindItemIndexByText(Date[0]);
                        }
                        else
                        {
                            dtpFromDate.AssignDate(Date[0], null, null);
                            dtpFromDate.cboYear.SelectedIndex = dtpFromDate.cboYear.Items.FindItemIndexByText(Date[0]);
                        }
                    }
                }

                if (Item.Cells[6].Text.Trim().ToUpper() == "CURRENT")
                {
                    chkCurrentDate.Checked = true;
                    dtpToDate.Enable = false;
                    dtpToDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                    ddlReadmitted.Disabled = true;
                    ddlReadmitted.SelectedIndex = 0;
                    dtpReadmissionDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                    dtpReadmissionDate.Enable = false;
                }
                else if (Item.Cells[6].Text.Trim() != string.Empty && Item.Cells[6].Text.Trim() != Space_Data)
                {
                    dtpToDate.Enable = true;
                    string[] Date = Item.Cells[6].Text.Split('-');
                    if (Date != null)
                    {
                        if (Date.Length == 3)
                        {
                            dtpToDate.AssignDate(Date[2], Date[1], Date[0]);
                            dtpToDate.cboYear.SelectedIndex = dtpToDate.cboYear.Items.FindItemIndexByText(Date[2]);
                            dtpToDate.cboMonth.SelectedIndex = dtpToDate.cboMonth.Items.FindItemIndexByText(Date[1]);
                            dtpToDate.cboDate.SelectedIndex = dtpToDate.cboDate.Items.FindItemIndexByText(Date[0]);
                        }
                        else if (Date.Length == 2)
                        {
                            dtpToDate.AssignDate(Date[1], Date[0], null);
                            dtpToDate.cboYear.SelectedIndex = dtpToDate.cboYear.Items.FindItemIndexByText(Date[1]);
                            dtpToDate.cboMonth.SelectedIndex = dtpToDate.cboMonth.Items.FindItemIndexByText(Date[0]);
                        }
                        else
                        {
                            dtpToDate.AssignDate(Date[0], null, null);
                            dtpToDate.cboYear.SelectedIndex = dtpToDate.cboYear.Items.FindItemIndexByText(Date[0]);
                        }
                    }
                }

                if (ddlReadmitted.SelectedIndex != 1)
	                {
	                    dtpReadmissionDate.Enable = false;
	                    dtpReadmissionDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
	                }
	                else if (Item.Cells[11].Text.Trim() != string.Empty && Item.Cells[11].Text.Trim() != Space_Data)
	                {
	                    dtpReadmissionDate.Enable = true;
	                    string[] Date = Item.Cells[11].Text.Split('-');
                        if (Date != null)
                        {
                            if (Date.Length == 3)
                            {
                                dtpReadmissionDate.AssignDate(Date[2], Date[1], Date[0]);
                                dtpReadmissionDate.cboYear.SelectedIndex = dtpFromDate.cboYear.Items.FindItemIndexByText(Date[2]);
                                dtpReadmissionDate.cboMonth.SelectedIndex = dtpFromDate.cboMonth.Items.FindItemIndexByText(Date[1]);
                                dtpReadmissionDate.cboDate.SelectedIndex = dtpFromDate.cboDate.Items.FindItemIndexByText(Date[0]);
                            }
                            else if (Date.Length == 2)
                            {

                                dtpReadmissionDate.AssignDate(Date[1], Date[0], null);
                                dtpReadmissionDate.cboYear.SelectedIndex = dtpFromDate.cboYear.Items.FindItemIndexByText(Date[1]);
                                dtpReadmissionDate.cboMonth.SelectedIndex = dtpFromDate.cboMonth.Items.FindItemIndexByText(Date[0]);
                            }
                            else
                            {
                                dtpReadmissionDate.AssignDate(Date[0], null, null);
                                dtpReadmissionDate.cboYear.SelectedIndex = dtpFromDate.cboYear.Items.FindItemIndexByText(Date[0]);
                            }
                        }
	                }

                DLC.txtDLC.Text = (Item.Cells[7].Text.Trim() == Space_Data) ? string.Empty : Item.Cells[7].Text;
                Session["UpdateId"] = Item.Cells[8].Text;

                btnAdd.Text = "Update";
                btnAdd.Enabled = true;
                btnClearAll.Text = "Cancel";

                System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
                if(text1!= null)
                text1.InnerText = "U";
                System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdditionalword");
                if (text2 != null)
                text2.InnerText = "pdate";
                btnAdd.AccessKey = "U";
                System.Web.UI.HtmlControls.HtmlGenericControl textClear = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                if (textClear != null)
                textClear.InnerText = "C";
                System.Web.UI.HtmlControls.HtmlGenericControl textClearAdd = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                if (textClearAdd != null)
                textClearAdd.InnerText = "ancel";
                btnAdd.Enabled = true;
                btnClearAll.Text = "Cancel";
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Autosave", "GridEditAutoSaveEnable();", true);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            else if (e.CommandName.Trim() == "DeleteRows")//Delete
            {
                string strtime = string.Empty;
                DateTime utc = DateTime.UtcNow;
                if (hdnLocalTime.Value != string.Empty)
                {
                    strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                    utc = Convert.ToDateTime(strtime);
                }
                IList<HospitalizationHistory> listOfHospitalizationToDelete = new List<HospitalizationHistory>();
                Session["UTCAuditDelete"] = utc;

                btnAdd.Enabled = false;
                hdnDelHospitalizationId.Value = Item.Cells[8].Text;
                ulong hospitalizationId = 0;
                if (grdHospitalizationHistory.MasterTableView.Items.Count > 0)
                {
                    hospitalizationId = Convert.ToUInt64(grdHospitalizationHistory.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[8].Text);
                }

                HospitalizationDTO HosptList = (HospitalizationDTO)Session["HospitalDTO"];
                if (ScreenMode == "Menu")
                {
                    DeleteFromMaster(hospitalizationId, HosptList);
                }
                else if (ScreenMode == "Queue")
                {
                    if ((IList<HospitalizationHistoryMaster>)Session["LoadHospitalizationMasterList"] != null && ((IList<HospitalizationHistoryMaster>)Session["LoadHospitalizationMasterList"]).Count > 0)
                    {
                        HosptList.HospMasterList= (IList<HospitalizationHistoryMaster>)Session["LoadHospitalizationMasterList"];
                        DeleteFromMaster(hospitalizationId, HosptList);
                    }
                    else
                    {
                    HosplList = HosptList.HospList;
                    IList<HospitalizationHistory> DelList = new List<HospitalizationHistory>();
                    IList<HospitalizationHistory> DeleteHospitalizationList = new List<HospitalizationHistory>();
                    HospitalizationHistory objHosptalizationHistory = new HospitalizationHistory();
                    objHosptalizationHistory = (from I in HosplList where I.Id == hospitalizationId select I).ToList<HospitalizationHistory>()[0];
                    objHosptalizationHistory.Modified_By = ClientSession.UserName;
                    objHosptalizationHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    DelList.Add(objHosptalizationHistory);
                    IList<HospitalizationHistory> SaveList = new List<HospitalizationHistory>();
                    if (DelList[0].Encounter_Id == ClientSession.EncounterId)
                        DeleteHospitalizationList.Add(DelList[0]);
                    foreach (HospitalizationHistory item in HosplList)
                    {
                        if (item.Id != DelList[0].Id && item.Encounter_Id != ClientSession.EncounterId)
                        {
                            item.Version = 0;
                            item.Id = 0;
                            item.Encounter_Id = ClientSession.EncounterId;
                            item.Created_By = ClientSession.UserName;
                            item.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            SaveList.Add(item);
                        }
                    }
                    Session["HospitalDTO"] = objHospitalizationHistoryManager.HospitalHistorySaveUpdateDelete(HosplList, SaveList, UpdateLst, DeleteHospitalizationList, HumanId, string.Empty, ClientSession.EncounterId);
                }
                }

                FillHospitalizationGridWithPageNavigator(true, (HospitalizationDTO)Session["HospitalDTO"], true);
                ClearFields();
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "SavedSuccessfully(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
        }
        public bool NotesDuplicate(string value)
        {
            string[] aryValue = value.Split(',');
            if (aryValue != null && aryValue.Length > 0)
            {
                HashSet<string> hashSet = new HashSet<string>(aryValue);
                if (hashSet.Count != aryValue.Length)
                    return true;
            }
            return false;
        }

        protected void chkPatientDeniesHospitalization_CheckedChanged(object sender, EventArgs e)
        {
            if (EncounterId != 0)
            {
                Encounter objEncounter = new Encounter();
                if (ClientSession.FillEncounterandWFObject.EncRecord.Id>0)
                {
                    objEncounter = ClientSession.FillEncounterandWFObject.EncRecord; 
                }
                else
                {
                    ilstEncounter=objEncounterManager.GetEncounterByEncounterID(EncounterId);
                    if (ilstEncounter!= null && ilstEncounter.Count > 0)
                    {
                        objEncounter = ilstEncounter[0]; 
                    }
               }
                if (objEncounter != null)
                {
                    if (chkPatientDeniesHospitalization.Checked)
                    {
                        chkCurrentDate.Checked = false;
                        txtReasonForHospitalization.Text = string.Empty;
                        txtDischargePhysician.Text = string.Empty;
                        dtpFromDate.AssignDate(null, null, null);
                        dtpFromDate.cboDate.ClearSelection();
                        dtpToDate.AssignDate(null, null, null);
                        dtpToDate.cboDate.ClearSelection();
                        dtpReadmissionDate.AssignDate(null, null, null);
                        dtpReadmissionDate.cboDate.ClearSelection();
                        DLC.txtDLC.Text = string.Empty;
                        DLC.Enable = false;
                        objEncounter.Is_Hospitalization_Denied = "Y";
                        pnlHospitalization.Enabled = false;
                        pnlGrid.Enabled = false;
                        btnAdd.Enabled = false;
                        btnClearAll.Enabled = false;
                        pbClear.Enabled = false;
                        pbClear.ImageUrl = "~/Resources/close_disabled.png";
                        pbDatabase.ImageUrl = "~/Resources/Database Disable.png";
                        dtpFromDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                        dtpToDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                        dtpReadmissionDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                        ddlReadmitted.SelectedIndex = 0;
                    }
                    else
                    {
                        objEncounter.Is_Hospitalization_Denied = "N";
                        pnlHospitalization.Enabled = true;
                        pnlGrid.Enabled = true;
                        lstReasonForHospitalization.Enabled = true;
                        btnAdd.Enabled = false;
                        btnClearAll.Enabled = true;
                        DLC.Enable = true;
                        pbClear.Enabled = true;
                        dtpFromDate.RadButton1.ImageUrl = "~/Resources/calenda2.bmp";
                        dtpFromDate.Enable = true;
                        dtpToDate.RadButton1.ImageUrl = "~/Resources/calenda2.bmp";
                        dtpReadmissionDate.RadButton1.ImageUrl = "~/Resources/calenda2.bmp";
	                        if (ddlReadmitted.SelectedIndex != 2)
	                            dtpReadmissionDate.Enable = true;
                        pbClear.ImageUrl = "~/Resources/close_small_pressed.png";
                        if (ClientSession.UserRole == "Medical Assistant" || ClientSession.UserRole.Trim().ToUpper() == "CODER")
                        {
                            pbDatabase.ImageUrl = "~/Resources/Database Disable.png";
                            pbDatabase.Enabled = false;
                        }
                        else
                        {
                            pbDatabase.Enabled = true;
                        }
                    }

                    objEncounter.Modified_By = ClientSession.UserName;
                    objEncounter.Modified_Date_and_Time = UtilityManager.ConvertToUniversal();
                    objEncounter.Local_Time = UtilityManager.ConvertToLocal(objEncounter.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
                    IList<Encounter> enclst = new List<Encounter>();
                    enclst = objEncounterManager.UpdateEncounterHospitalization(objEncounter, string.Empty);
                    ClientSession.FillEncounterandWFObject.EncRecord = (Encounter)enclst[0];

                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            HospitalizationHistory objHospitalizationHistory = null;
            if (HospitalDTO != null && HospitalDTO.HospList.Count > 0)
            {
                objHospitalizationHistory = HospitalDTO.HospList.Where(a => a.Id == Convert.ToUInt32(hdnDelHospitalizationId.Value)).ToList<HospitalizationHistory>()[0];
            }
            if (Session["HospitalDTO"] != null)
            {
                if (HospitalDTO.HospList.Count > 0)
                    chkPatientDeniesHospitalization.Enabled = false;
                else
                    chkPatientDeniesHospitalization.Enabled = true;
            }
            else
            {
                chkPatientDeniesHospitalization.Enabled = true;
                if (grdHospitalizationHistory.DataSource == null)
                {
                    grdHospitalizationHistory.DataSource = new string[] { };
                    grdHospitalizationHistory.DataBind();
                }
            }
            ClearFields();
        }
        protected void LibraryIconButton_Click(object sender, EventArgs e)
        {
            LoadReasonforHospitalization();
        }

        protected void InvisibleClearAllButton_Click(object sender, EventArgs e)
        {
            btnAdd.Text = "Add";
            btnClearAll.Text = "Clear All";
            btnAdd.Enabled = false;
            txtReasonForHospitalization.Text = string.Empty;
            txtDischargePhysician.Text = string.Empty;
            chkCurrentDate.Checked = false;
            dtpFromDate.Enable = true;
            dtpToDate.Enable = true;
            dtpFromDate.AssignDate(null, null, null);
            dtpFromDate.cboDate.SelectedIndex = 0;
            dtpFromDate.cboMonth.SelectedIndex = 0;
            dtpFromDate.cboYear.SelectedIndex = 0;

            dtpToDate.AssignDate(null, null, null);
            dtpToDate.cboDate.SelectedIndex = 0;
            dtpToDate.cboMonth.SelectedIndex = 0;
            dtpToDate.cboYear.SelectedIndex = 0;

            dtpReadmissionDate.AssignDate(null, null, null);
            dtpReadmissionDate.cboDate.SelectedIndex = 0;
            dtpReadmissionDate.cboMonth.SelectedIndex = 0;
            dtpReadmissionDate.cboYear.SelectedIndex = 0;
            ddlReadmitted.SelectedIndex = 0;

            DLC.txtDLC.Text = string.Empty;
            System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
            if(text1!= null)
            text1.InnerText = "A";
            System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdditionalword");
            if (text2 != null)
            text2.InnerText = "dd";
            System.Web.UI.HtmlControls.HtmlGenericControl textClear = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
            if (textClear != null)
            textClear.InnerText = "C";
            System.Web.UI.HtmlControls.HtmlGenericControl textClearAdd = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
            if (textClearAdd != null)
            textClearAdd.InnerText = "lear All";
            btnAdd.AccessKey = "A";
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        public void FirstPageNavigator(object sender, EventArgs e)
        {
            FillHospitalizationGridWithPageNavigator(true, null,false);
        }

        public void FillHospitalizationGridWithPageNavigator(bool Is_Load, HospitalizationDTO HospitalDTOs,bool is_Delete)
        {
            HospitalizationDTO HospHistDTO = new HospitalizationDTO();
            if (ScreenMode == "Menu")
             {
                 if (Is_Load)
                 LoadFromMaster(Is_Load, HospHistDTO, is_Delete);
             }
            else if (ScreenMode == "Queue")
             {
                 if (Is_Load)
                 {
                     bool _is_from_current_encounter_data = false;
                     IList<HospitalizationHistory> HospHislst = new List<HospitalizationHistory>();
                    IList<object> ilstInHospitalizationHistoryBlobFinal = new List<object>();
                    IList<string> ilstHospitalizationHistoryTagList = new List<string>();
                    #region Commented By Deepak

                    //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
                    //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                    //if (File.Exists(strXmlFilePath) == true)
                    //{
                    //    XmlDocument itemDoc = new XmlDocument();
                    //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                    //    XmlNodeList xmlTagName = null;
                    //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    //    {
                    //        itemDoc.Load(fs);
                    //        XmlText.Close();
                    //        if (itemDoc.GetElementsByTagName("HospitalizationHistoryList") != null && itemDoc.GetElementsByTagName("HospitalizationHistoryList").Count > 0)
                    //        {
                    //            xmlTagName = itemDoc.GetElementsByTagName("HospitalizationHistoryList")[0].ChildNodes;
                    //            if (xmlTagName != null && xmlTagName.Count > 0)
                    //            {
                    //                for (int j = 0; j < xmlTagName.Count; j++)
                    //                {
                    //                    string TagName = xmlTagName[j].Name;
                    //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(HospitalizationHistory));
                    //                    HospitalizationHistory HospitalizationHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as HospitalizationHistory;
                    //                    IEnumerable<PropertyInfo> propInfo = null;
                    //                    propInfo = from obji in ((HospitalizationHistory)HospitalizationHistory).GetType().GetProperties() select obji;
                    //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                    //                    {
                    //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                    //                        {
                    //                            if (propInfo != null)
                    //                            {
                    //                                foreach (PropertyInfo property in propInfo)
                    //                                {
                    //                                    if (property.Name == nodevalue.Name)
                    //                                    {
                    //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
                    //                                            property.SetValue(HospitalizationHistory, Convert.ToUInt64(nodevalue.Value), null);
                    //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
                    //                                            property.SetValue(HospitalizationHistory, Convert.ToString(nodevalue.Value), null);
                    //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                    //                                            property.SetValue(HospitalizationHistory, Convert.ToDateTime(nodevalue.Value), null);
                    //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
                    //                                            property.SetValue(HospitalizationHistory, Convert.ToInt32(nodevalue.Value), null);
                    //                                        else
                    //                                            property.SetValue(HospitalizationHistory, nodevalue.Value, null);
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }

                    //                    HospHislst.Add(HospitalizationHistory);
                    //                    if (HospitalizationHistory.Encounter_Id == ClientSession.EncounterId)
                    //                        _is_from_current_encounter_data = true;
                    //                }
                    //                if (!_is_from_current_encounter_data)
                    //                {
                    //                    HospHislst.Clear();
                    //                    LoadFromMaster(Is_Load, HospHistDTO, is_Delete);
                    //                    if (Session["HospitalDTO"] != null)
                    //                        HospHistDTO = (HospitalizationDTO)Session["HospitalDTO"];
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            LoadFromMaster(Is_Load, HospHistDTO, is_Delete);
                    //        }
                    //        if (itemDoc.GetElementsByTagName("EncounterList") != null && itemDoc.GetElementsByTagName("EncounterList").Count > 0)
                    //        {
                    //            xmlTagName = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes;

                    //            if (xmlTagName != null && xmlTagName.Count > 0)
                    //            {
                    //                for (int j = 0; j < xmlTagName.Count; j++)
                    //                {
                    //                    if (xmlTagName[j].Attributes[73].Value == ClientSession.EncounterId.ToString())
                    //                    {
                    //                        string TagName = xmlTagName[j].Name;
                    //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(Encounter));
                    //                        Encounter Encounter = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Encounter;
                    //                        IEnumerable<PropertyInfo> propInfo = null;
                    //                        propInfo = from obji in ((Encounter)Encounter).GetType().GetProperties() select obji;

                    //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                    //                        {
                    //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
                    //                            {
                    //                                if (propInfo != null)
                    //                                {
                    //                                    foreach (PropertyInfo property in propInfo)
                    //                                    {
                    //                                        if (property.Name == nodevalue.Name)
                    //                                        {
                    //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
                    //                                                property.SetValue(Encounter, Convert.ToUInt64(nodevalue.Value), null);
                    //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
                    //                                                property.SetValue(Encounter, Convert.ToString(nodevalue.Value), null);
                    //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                    //                                                property.SetValue(Encounter, Convert.ToDateTime(nodevalue.Value), null);
                    //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
                    //                                                property.SetValue(Encounter, Convert.ToInt32(nodevalue.Value), null);
                    //                                            else
                    //                                                property.SetValue(Encounter, nodevalue.Value, null);
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                        HospHistDTO.Encount = Encounter;
                    //                    }
                    //                }
                    //            }

                    //        }
                    //        if (itemDoc.GetElementsByTagName("dob") != null && itemDoc.GetElementsByTagName("dob").Count > 0)
                    //        {
                    //            HospHistDTO.DateofBirth = Convert.ToDateTime(itemDoc.GetElementsByTagName("dob")[0].InnerText);
                    //        }
                    //        fs.Close();
                    //        fs.Dispose();
                    //    }
                    //}
                    #endregion

                    ilstHospitalizationHistoryTagList.Add("HospitalizationHistoryList");
                    ilstHospitalizationHistoryTagList.Add("EncounterList");

                   

                    ilstInHospitalizationHistoryBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstHospitalizationHistoryTagList);

                    if (ilstInHospitalizationHistoryBlobFinal != null && ilstInHospitalizationHistoryBlobFinal.Count > 0)
                    {
                        if (ilstInHospitalizationHistoryBlobFinal[0] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstInHospitalizationHistoryBlobFinal[0]).Count; iCount++)
                            {
                                HospHislst.Add((HospitalizationHistory)((IList<object>)ilstInHospitalizationHistoryBlobFinal[0])[iCount]);
                                if (((HospitalizationHistory)((IList<object>)ilstInHospitalizationHistoryBlobFinal[0])[iCount]).Encounter_Id == ClientSession.EncounterId)
                                    _is_from_current_encounter_data = true;
                            }
                            if (!_is_from_current_encounter_data)
                            {
                                HospHislst.Clear();
                                LoadFromMaster(Is_Load, HospHistDTO, is_Delete);
                                if (Session["HospitalDTO"] != null)
                                    HospHistDTO = (HospitalizationDTO)Session["HospitalDTO"];
                            }
                        }
                        else
                        {
                            LoadFromMaster(Is_Load, HospHistDTO, is_Delete);
                        }

                        if (ilstInHospitalizationHistoryBlobFinal[1] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstInHospitalizationHistoryBlobFinal[1]).Count; iCount++)
                            {
                                if (((Encounter)((IList<object>)ilstInHospitalizationHistoryBlobFinal[1])[iCount]).Encounter_ID == ClientSession.EncounterId)
                                {
                                    //GitLab #3940
                                    HospHistDTO.Encount = (Encounter)(((IList<object>)ilstInHospitalizationHistoryBlobFinal[1])[iCount]);
                                }
                            }
                        }

                    }
                    if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0 && (ClientSession.PatientPaneList[0]).Birth_Date != null)
                    {
                        HospHistDTO.DateofBirth = (ClientSession.PatientPaneList[0]).Birth_Date;
                    }
                   



                    if (HospHislst != null && HospHislst.Count > 0)
                     {
                         IList<HospitalizationHistory> lstHospCurrEnc = new List<HospitalizationHistory>();
                         lstHospCurrEnc = (from item in HospHislst where item.Encounter_Id == ClientSession.EncounterId select item).ToList<HospitalizationHistory>();
                         if (lstHospCurrEnc != null && lstHospCurrEnc.Count > 0)
                         {
                             HospHistDTO.HospList = lstHospCurrEnc;
                         }
                         else if (!is_Delete)
                         {
                             ulong maxEncId = 0;
                             IList<ulong> lstEncId = (from item in HospHislst select item.Encounter_Id).Distinct().ToList<ulong>();
                             if (lstEncId != null && lstEncId.Count > 0)
                                 maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                             foreach (ulong item in lstEncId)
                                 if (item > maxEncId && item < ClientSession.EncounterId)
                                     maxEncId = item;
                             lstHospCurrEnc = (from item in HospHislst where item.Encounter_Id == maxEncId select item).ToList<HospitalizationHistory>();
                             HospHistDTO.HospList = lstHospCurrEnc;
                         }
                     }
                     else
                     {
                         HospHistDTO.HospList = HospHislst;
                     }
                     Session["HospitalDTO"] = HospHistDTO;
                     HospitalDTO = (HospitalizationDTO)Session["HospitalDTO"];

                 }
                 else
                 {
                     Session["HospitalDTO"] = HospitalDTOs;
                     HospitalDTO = (HospitalizationDTO)Session["HospitalDTO"];
                 }

                 if (HospitalDTO != null)
                 {
                     FillHospitalizationGrid(HospitalDTO);
                 }
                 else
                 {
                     grdHospitalizationHistory.DataSource = new string[] { };
                     grdHospitalizationHistory.DataBind();
                 }
             }

        }

        public void LoadFromMaster(bool Is_Load, HospitalizationDTO HospitalDTO, bool is_Delete)
        {
            if (Is_Load)
            {
                IList<HospitalizationHistoryMaster> HospLoadMasterLst = new List<HospitalizationHistoryMaster>();
                IList<object> ilstInHospitalizationHistoryBlobFinal = new List<object>();
                IList<string> ilstHospitalizationHistoryTagList = new List<string>();

                #region Commented By Deepak
                
                //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                //if (File.Exists(strXmlFilePath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                //    XmlNodeList xmlTagName = null;
                //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);
                //        XmlText.Close();
                //        if (itemDoc.GetElementsByTagName("HospitalizationHistoryMasterList") != null && itemDoc.GetElementsByTagName("HospitalizationHistoryMasterList").Count > 0)
                //        {
                //            xmlTagName = itemDoc.GetElementsByTagName("HospitalizationHistoryMasterList")[0].ChildNodes;
                //            if (xmlTagName != null && xmlTagName.Count > 0)
                //            {
                //                for (int j = 0; j < xmlTagName.Count; j++)
                //                {
                //                    string TagName = xmlTagName[j].Name;
                //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(HospitalizationHistoryMaster));
                //                    HospitalizationHistoryMaster HospitalizationHistoryMaster = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as HospitalizationHistoryMaster;
                //                    IEnumerable<PropertyInfo> propInfo = null;
                //                    propInfo = from obji in ((HospitalizationHistoryMaster)HospitalizationHistoryMaster).GetType().GetProperties() select obji;
                //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                    {
                //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                        {
                //                            if (propInfo != null)
                //                            {
                //                                foreach (PropertyInfo property in propInfo)
                //                                {
                //                                    if (property.Name == nodevalue.Name)
                //                                    {
                //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                            property.SetValue(HospitalizationHistoryMaster, Convert.ToUInt64(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                            property.SetValue(HospitalizationHistoryMaster, Convert.ToString(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                            property.SetValue(HospitalizationHistoryMaster, Convert.ToDateTime(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                            property.SetValue(HospitalizationHistoryMaster, Convert.ToInt32(nodevalue.Value), null);
                //                                        else
                //                                            property.SetValue(HospitalizationHistoryMaster, nodevalue.Value, null);
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                    if (HospitalizationHistoryMaster.Is_Deleted != "Y")
                //                    HospLoadMasterLst.Add(HospitalizationHistoryMaster);
                //                }
                //            }
                //        }
                //        if (itemDoc.GetElementsByTagName("EncounterList") != null && itemDoc.GetElementsByTagName("EncounterList").Count > 0)
                //        {
                //            xmlTagName = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes;

                //            if (xmlTagName != null && xmlTagName.Count > 0)
                //            {
                //                for (int j = 0; j < xmlTagName.Count; j++)
                //                {
                //                    if (xmlTagName[j].Attributes[73].Value == ClientSession.EncounterId.ToString())
                //                    {
                //                        string TagName = xmlTagName[j].Name;
                //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(Encounter));
                //                        Encounter Encounter = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Encounter;
                //                        IEnumerable<PropertyInfo> propInfo = null;
                //                        propInfo = from obji in ((Encounter)Encounter).GetType().GetProperties() select obji;

                //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                        {
                //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                            {
                //                                if (propInfo != null)
                //                                {
                //                                    foreach (PropertyInfo property in propInfo)
                //                                    {
                //                                        if (property.Name == nodevalue.Name)
                //                                        {
                //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                                property.SetValue(Encounter, Convert.ToUInt64(nodevalue.Value), null);
                //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                                property.SetValue(Encounter, Convert.ToString(nodevalue.Value), null);
                //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                                property.SetValue(Encounter, Convert.ToDateTime(nodevalue.Value), null);
                //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                                property.SetValue(Encounter, Convert.ToInt32(nodevalue.Value), null);
                //                                            else
                //                                                property.SetValue(Encounter, nodevalue.Value, null);
                //                                        }
                //                                    }
                //                                }
                //                            }
                //                        }
                //                        HospitalDTO.Encount = Encounter;
                //                    }
                //                }
                //            }

                //        }
                //        if (itemDoc.GetElementsByTagName("dob") != null && itemDoc.GetElementsByTagName("dob").Count > 0)
                //        {
                //            HospitalDTO.DateofBirth = Convert.ToDateTime(itemDoc.GetElementsByTagName("dob")[0].InnerText);
                //        }
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}

                #endregion

                ilstHospitalizationHistoryTagList.Add("HospitalizationHistoryMasterList");
                ilstHospitalizationHistoryTagList.Add("EncounterList");
                IList<Encounter> lstencounter = new List<Encounter>();
                ilstInHospitalizationHistoryBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstHospitalizationHistoryTagList);

                if (ilstInHospitalizationHistoryBlobFinal != null && ilstInHospitalizationHistoryBlobFinal.Count > 0)
                {
                    if (ilstInHospitalizationHistoryBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstInHospitalizationHistoryBlobFinal[0]).Count; iCount++)
                        {
                            if (((HospitalizationHistoryMaster)((IList<object>)ilstInHospitalizationHistoryBlobFinal[0])[iCount]).Is_Deleted != "Y")
                            {
                                HospLoadMasterLst.Add((HospitalizationHistoryMaster)((IList<object>)ilstInHospitalizationHistoryBlobFinal[0])[iCount]);
                            }
                        }
                    }

                    if (ilstInHospitalizationHistoryBlobFinal[1] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstInHospitalizationHistoryBlobFinal[1]).Count; iCount++)
                        {
                            if (((Encounter)((IList<object>)ilstInHospitalizationHistoryBlobFinal[1])[iCount]).Encounter_ID == ClientSession.EncounterId)
                            {

                                HospitalDTO.Encount = (Encounter)((IList<object>)ilstInHospitalizationHistoryBlobFinal[1])[iCount];
                            }
                        }
                    }
                }

                if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0 && (ClientSession.PatientPaneList[0]).Birth_Date != null)
                {
                    HospitalDTO.DateofBirth = (ClientSession.PatientPaneList[0]).Birth_Date;
                }

                if (HospLoadMasterLst != null && HospLoadMasterLst.Count > 0)
                {
                    if ((ScreenMode == "Queue") || (ScreenMode == "Portal"))
                        Session["LoadHospitalizationMasterList"] = HospLoadMasterLst;
                    HospitalDTO.HospMasterList = HospLoadMasterLst;
                }
                else
                {
                    if (ScreenMode == "Queue")
                        Session["LoadHospitalizationMasterList"] = null;
                }
                Session["HospitalDTO"] = HospitalDTO;
                HospitalDTO = (HospitalizationDTO)Session["HospitalDTO"];

            }
            else
            {
                Session["HospitalDTO"] = HospitalDTO;
                HospitalDTO = (HospitalizationDTO)Session["HospitalDTO"];
            }

            if (HospitalDTO != null)
            {
                FillHospitalizationGrid(HospitalDTO);
            }
            else
            {
                grdHospitalizationHistory.DataSource = new string[] { };
                grdHospitalizationHistory.DataBind();
            }

        }
        public void DeleteFromMaster(ulong hospitalizationID, HospitalizationDTO HospitalDTO)
        {
            HospMasterlList = HospitalDTO.HospMasterList;
            IList<HospitalizationHistoryMaster> DelList = new List<HospitalizationHistoryMaster>();
            IList<HospitalizationHistoryMaster> UpdateDeleteHospMasterList = new List<HospitalizationHistoryMaster>();
            HospitalizationHistoryMaster objHosptalizationMasterHistory = new HospitalizationHistoryMaster();
            objHosptalizationMasterHistory = (from I in HospMasterlList where I.Id == hospitalizationID select I).ToList<HospitalizationHistoryMaster>()[0];
            objHosptalizationMasterHistory.Is_Deleted = "Y";
            objHosptalizationMasterHistory.Modified_By = ClientSession.UserName;
            objHosptalizationMasterHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            DelList.Add(objHosptalizationMasterHistory);
            IList<HospitalizationHistoryMaster> SaveMasterList = new List<HospitalizationHistoryMaster>();
            if (DelList.Count > 0)
            UpdateDeleteHospMasterList.Add(DelList[0]);
            if (UpdateDeleteHospMasterList.Count>0)
                Session["HospitalDTO"] = objHospitalizationHistoryMasterManager.HospitalHistorySaveUpdateDelete(HospMasterlList, SaveMasterLst, UpdateDeleteHospMasterList, null, HumanId, string.Empty);
        }

        public void FillGridFromMaster(HospitalizationDTO HospitalDTO)
        {
            HospitalizationDTO ObjHospDto = new HospitalizationDTO();
            DataTable objDataTable = new DataTable();
            DataColumn objDataColumn = new DataColumn();
            objDataColumn = new DataColumn("ReasonForHospitalization", typeof(string));
            objDataTable.Columns.Add(objDataColumn);
            objDataColumn = new DataColumn("FromDate", typeof(string));
            objDataTable.Columns.Add(objDataColumn);
            objDataColumn = new DataColumn("ToDate", typeof(string));
            objDataTable.Columns.Add(objDataColumn);
            objDataColumn = new DataColumn("HospitalizationNotes", typeof(string));
            objDataTable.Columns.Add(objDataColumn);
            objDataColumn = new DataColumn("Id", typeof(string));
            objDataTable.Columns.Add(objDataColumn);
            objDataColumn = new DataColumn("DischargePhysician", typeof(string));
            objDataTable.Columns.Add(objDataColumn);
            objDataColumn = new DataColumn("Readmitted", typeof(string));
            objDataTable.Columns.Add(objDataColumn);
            objDataColumn = new DataColumn("Readmitted_Date", typeof(string));
            objDataTable.Columns.Add(objDataColumn);
            ObjHospDto.HospMasterList = new List<HospitalizationHistoryMaster>();
            foreach (HospitalizationHistoryMaster obj in HospitalDTO.HospMasterList)
            {
                if (obj.From_Date.Trim() != string.Empty)
                    switch (obj.From_Date.Split('-').Count())
                    {
                        case 1:
                            ObjHospDto.dictHospFromDate.Add(obj.Id, Convert.ToDateTime("01-Jan-" + obj.From_Date));
                            break;
                        case 2:
                            ObjHospDto.dictHospFromDate.Add(obj.Id, Convert.ToDateTime("01-" + obj.From_Date));
                            break;
                        case 3:
                            ObjHospDto.dictHospFromDate.Add(obj.Id, Convert.ToDateTime(obj.From_Date));
                            break;
                        default:
                            break;
                    }
                else
                    ObjHospDto.dictHospFromDate.Add(obj.Id, DateTime.MinValue);
            }
            ObjHospDto.dictHospFromDate = (from keyval in ObjHospDto.dictHospFromDate orderby keyval.Value descending select keyval).ToDictionary(keyval => keyval.Key, keyval => keyval.Value);

            foreach (KeyValuePair<ulong, DateTime> item in ObjHospDto.dictHospFromDate)
            {
                if (item.Value != DateTime.MinValue)
                {
                    ObjHospDto.HospMasterList.Add((from ob in HospitalDTO.HospMasterList where ob.Id == item.Key select ob).ToList<HospitalizationHistoryMaster>()[0]);
                }
            }
            ObjHospDto.dictHospFromDate = (from keyval in ObjHospDto.dictHospFromDate where keyval.Value == DateTime.MinValue select keyval).ToDictionary(keyval => keyval.Key, keyval => keyval.Value);
            if (ObjHospDto.dictHospFromDate.Count > 0)
            {
                for (int i = 0; i < ObjHospDto.dictHospFromDate.Count; i++)
                {
                    HospitalizationHistoryMaster objSurDate = (from objSurg in HospitalDTO.HospMasterList where ObjHospDto.dictHospFromDate.Keys.ElementAt(i) == objSurg.Id select objSurg).ToList<HospitalizationHistoryMaster>()[0];
                    if (DateTime.Compare(objSurDate.Modified_Date_And_Time, objSurDate.Created_Date_And_Time) >= 0)
                        ObjHospDto.dictHospFromDate[ObjHospDto.dictHospFromDate.Keys.ElementAt(i)] = objSurDate.Modified_Date_And_Time;
                    else
                        ObjHospDto.dictHospFromDate[ObjHospDto.dictHospFromDate.Keys.ElementAt(i)] = objSurDate.Created_Date_And_Time;
                }
            }

            ObjHospDto.dictHospFromDate = (from keyval in ObjHospDto.dictHospFromDate orderby keyval.Value descending select keyval).ToDictionary(keyval => keyval.Key, keyval => keyval.Value);

            foreach (KeyValuePair<ulong, DateTime> item in ObjHospDto.dictHospFromDate)
                ObjHospDto.HospMasterList.Add((from ob in HospitalDTO.HospMasterList where ob.Id == item.Key select ob).ToList<HospitalizationHistoryMaster>()[0]);

            for (int i = 0; i < ObjHospDto.HospMasterList.Count; i++)
            {
                DataRow objDataRow = objDataTable.NewRow();
                objDataRow["ReasonForHospitalization"] = ObjHospDto.HospMasterList[i].Reason_For_Hospitalization;
                objDataRow["FromDate"] = ObjHospDto.HospMasterList[i].From_Date;
                objDataRow["ToDate"] = ObjHospDto.HospMasterList[i].To_Date;
                objDataRow["HospitalizationNotes"] = ObjHospDto.HospMasterList[i].Hospitalization_Notes;
                objDataRow["Id"] = ObjHospDto.HospMasterList[i].Id;
                objDataRow["DischargePhysician"] = ObjHospDto.HospMasterList[i].Discharge_Physician;
                objDataRow["Readmitted"] = ObjHospDto.HospMasterList[i].Is_Readmitted;
                objDataRow["Readmitted_Date"] = ObjHospDto.HospMasterList[i].Readmission_Date;
                objDataTable.Rows.Add(objDataRow);
            }
            grdHospitalizationHistory.DataSource = objDataTable;
            grdHospitalizationHistory.DataBind();
        }
       
    }
}
