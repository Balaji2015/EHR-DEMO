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
    public partial class frmHistorySurgical : System.Web.UI.Page
    {
        #region Declaration

        UserLookupManager objUserLookupManager = new UserLookupManager();
        SurgicalHistoryManager objSurgicalHistoryManager = new SurgicalHistoryManager();
        SurgicalHistory objSurgicalHistory = new SurgicalHistory();
        IList<SurgicalHistory> SaveLst = new List<SurgicalHistory>();
        IList<SurgicalHistory> UpdateLst = new List<SurgicalHistory>();
        IList<SurgicalHistory> DeleteLst = new List<SurgicalHistory>();
        IList<SurgicalHistory> SurgclList = new List<SurgicalHistory>();

        SurgicalHistoryMasterManager objSurgicalHistoryMasterManager = new SurgicalHistoryMasterManager();
        SurgicalHistoryMaster objSurgicalMasterHistory = new SurgicalHistoryMaster();
        IList<SurgicalHistoryMaster> SaveMasterLst = new List<SurgicalHistoryMaster>();
        IList<SurgicalHistoryMaster> UpdateMasterLst = new List<SurgicalHistoryMaster>();
        IList<SurgicalHistoryMaster> DeleteMasterLst = new List<SurgicalHistoryMaster>();
        IList<SurgicalHistoryMaster> SurgicalMasterList = new List<SurgicalHistoryMaster>();



        string Space_Data = "&nbsp;";
        SurgicalHistoryDTO objSurgicalHistoryDTO = null;
        string FromComboBoxDate = string.Empty;
        ulong EncounterId = 0;
        ulong HumanId = 0;
        ulong PhysicianId = 0;
        string ScreenMode = string.Empty;

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            frmHistorySurgical _source = (frmHistorySurgical)sender;
            ScreenMode = _source.Page.Request.Params[0];
            EncounterId = ClientSession.EncounterId;
            HumanId = ClientSession.HumanId;
            PhysicianId = ClientSession.PhysicianId;
            if (!IsPostBack)
            {
                ClientSession.FlushSession();
                Hiddenupdate.Value = "ADD";
                dtpDateOfSurgery.AssignDate(null, null, null);
                DLC.txtDLC.Attributes.Add("onkeypress", "EnableSave(event);");
                txtSurgeryName.Attributes.Add("onkeypress", "EnableSave(event);");
                txtSurgeryName.Attributes.Add("onchange", "EnableSave(event);");
                dtpDateOfSurgery.clbCalendar.ClientEvents.OnDateSelected = "Enable_OR_Disable";//SelectionChanged += new Telerik.Web.UI.Calendar.SelectedDatesEventHandler(clb_SelectionChanged);
                LoadSurgeryName();
                LoadGridWithPageNavigator(true, null, false);
                btnAdd.Enabled = false;
            }
            DLC.txtDLC.Attributes.Add("onChange", "CCTextChanged()");
            dtpDateOfSurgery.cboDate.OnClientSelectedIndexChanged = "DateSelecting";
            dtpDateOfSurgery.cboMonth.OnClientSelectedIndexChanged = "DateSelecting";
            dtpDateOfSurgery.cboYear.OnClientSelectedIndexChanged = "DateSelecting";
            dtpDateOfSurgery.clbCalendar.ClientEvents.OnDateSelected = "DateSelecting";

            dtpDateOfSurgery.cboDate.AutoPostBack = false;
            dtpDateOfSurgery.cboMonth.AutoPostBack = false;
            dtpDateOfSurgery.cboYear.AutoPostBack = false;
            dtpDateOfSurgery.clbCalendar.AutoPostBack = false;


            if (Session["SurgicalHistoryDTO"] != null)
                objSurgicalHistoryDTO = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];

            if (!IsPostBack)
            {
                if (UIManager.is_Menu_Level_PFSH)
                {
                    this.Page.Items.Add("Title", "frmHistorySurgicalMenu");
                }
                ClientSession.processCheck = true;
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);
                btnAdd.Enabled = false;
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "EndWaitCursor", "EndWaitCursor();", true);
            if (ClientSession.UserCurrentOwner.Trim() == "UNKNOWN" || (!ClientSession.CheckUser && ClientSession.UserPermission == "R"))
            {
                btnAdd.Enabled = false;
                btnClearAll.Enabled = false;
                lstSurgeryName.Enabled = false;
                dtpDateOfSurgery.Enable = false;
                dtpDateOfSurgery.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                if (grdSurgeryHistoryDetails != null && grdSurgeryHistoryDetails.Items.Count > 0)
                {
                    grdSurgeryHistoryDetails.Columns[0].Visible = false;
                    grdSurgeryHistoryDetails.Columns[1].Visible = false;
                }
            }

            if (ClientSession.UserRole.ToUpper() != "PHYSICIAN" && ClientSession.UserRole.ToUpper() != "PHYSICIAN ASSISTANT")
            {
                pbDatabase.ImageUrl = "~/Resources/Database Disable.png";
                pbDatabase.Enabled = false;
            }

        }


        void cboDate_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            btnAdd.Enabled = true;
        }

        void pbDropdown_Click(object sender, ImageClickEventArgs e)
        {
            if (Hidden1.Value == "True")
                btnAdd.Enabled = true;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            #region Validations
            string strtime = string.Empty;
            DateTime utc = DateTime.UtcNow;


            if (hdnLocalTime.Value != string.Empty)
            {
                strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                utc = Convert.ToDateTime(strtime);
            }

            string sDate = string.Empty;

            if (dtpDateOfSurgery.cboDate.Text != "" && dtpDateOfSurgery.cboMonth.Text != "" && dtpDateOfSurgery.cboYear.Text != "")
            {
                FromComboBoxDate = dtpDateOfSurgery.cboDate.Text + "-" + dtpDateOfSurgery.cboMonth.Text + "-" + dtpDateOfSurgery.cboYear.Text;
                sDate = dtpDateOfSurgery.cboDate.Text + "-" + dtpDateOfSurgery.cboMonth.Text + "-" + dtpDateOfSurgery.cboYear.Text;
            }
            else if (dtpDateOfSurgery.cboYear.Text != "" && dtpDateOfSurgery.cboMonth.Text != "" && dtpDateOfSurgery.cboDate.Text == "")
            {
                FromComboBoxDate = dtpDateOfSurgery.cboMonth.Text + "-" + dtpDateOfSurgery.cboYear.Text;
                sDate = "01-" + dtpDateOfSurgery.cboMonth.Text + "-" + dtpDateOfSurgery.cboYear.Text;
            }
            else if (dtpDateOfSurgery.cboYear.Text != "" && dtpDateOfSurgery.cboMonth.Text == "" && dtpDateOfSurgery.cboDate.Text == "")
            {
                FromComboBoxDate = dtpDateOfSurgery.cboYear.Text;
                sDate = "01-" + "Jan-" + dtpDateOfSurgery.cboYear.Text;
            }
            else if (dtpDateOfSurgery.cboYear.Text == string.Empty && dtpDateOfSurgery.cboMonth.Text != string.Empty && dtpDateOfSurgery.cboDate.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FromDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none'; {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();DisplayErrorMessage('180611');", true);
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
                string[] aryDate = FromComboBoxDate.TrimStart('-').TrimEnd('-').Split('-');

                if (aryDate != null && aryDate.Length == 1)
                {
                    if (aryDate[0] != string.Empty && aryDate[0].Length < 4)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "DateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none'; {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();DisplayErrorMessage('180006');", true);
                        return;
                    }
                }
            }
            if (dt.Year != 1)
            {
                int h = dt.Date.CompareTo(utc.Date);
                if (h > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NowDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none'; {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();DisplayErrorMessage('180009');", true);
                    return;
                }

                if (objSurgicalHistoryDTO != null)
                {
                    if (ClientSession.PatientPaneList.Count > 0)
                    {
                        if (ClientSession.PatientPaneList.Where(a => a.Human_Id == ClientSession.HumanId).ToList<PatientPane>()[0].Birth_Date != null)
                        {
                            h = dt.Date.CompareTo(ClientSession.PatientPaneList.Where(a => a.Human_Id == ClientSession.HumanId).ToList<PatientPane>()[0].Birth_Date.Date);
                            if (h < 0)
                            {
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "BirthDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none'; {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();DisplayErrorMessage('180012');", true);
                                return;
                            }
                        }
                    }
                    else
                    {
                        h = dt.Date.CompareTo(objSurgicalHistoryDTO.PatientDOB);
                        if (h < 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "BirthDateValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none'; {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();DisplayErrorMessage('180012');", true);
                            return;
                        }
                    }
                }
            }

            #endregion

            if (string.Compare(Convert.ToString(Hiddenupdate.Value), "ADD", true) == 0)
            {
                LoadObject();
                if (ScreenMode == "Menu")
                {
                    objSurgicalMasterHistory.Human_ID = HumanId;
                    objSurgicalMasterHistory.Created_By = ClientSession.UserName;
                    objSurgicalMasterHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    SaveMasterLst.Add(objSurgicalMasterHistory);
                }
                else
                {
                    objSurgicalHistory.Human_ID = HumanId;
                    objSurgicalHistory.Encounter_Id = ClientSession.EncounterId;
                    objSurgicalHistory.Created_By = ClientSession.UserName;
                    objSurgicalHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    SaveLst.Add(objSurgicalHistory);
                    if (objSurgicalHistoryDTO != null && objSurgicalHistoryDTO.SurgicalList != null && objSurgicalHistoryDTO.SurgicalList.Count > 0)
                    {
                        foreach (SurgicalHistory item in objSurgicalHistoryDTO.SurgicalList)
                        {
                            if (item.Encounter_Id != ClientSession.EncounterId)
                            {
                                SurgicalHistory obj = new SurgicalHistory();
                                obj.Date_Of_Surgery = item.Date_Of_Surgery;
                                obj.Description = item.Description;
                                obj.Human_ID = item.Human_ID;
                                obj.Surgery_Name = item.Surgery_Name;
                                obj.Encounter_Id = ClientSession.EncounterId;
                                obj.Created_By = ClientSession.UserName;
                                obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                obj.Id = 0;
                                SaveLst.Add(obj);
                            }
                        }
                    }
                }
                if (Session["LoadSurgicalMasterList"] != null)
                {
                    IList<SurgicalHistoryMaster> _loadMasterList = new List<SurgicalHistoryMaster>();
                    _loadMasterList = (IList<SurgicalHistoryMaster>)Session["LoadSurgicalMasterList"];
                    if (_loadMasterList.Count > 0)
                    {
                        foreach (SurgicalHistoryMaster item in _loadMasterList)
                        {
                            if (ScreenMode == "Queue")
                            {
                                SurgicalHistory obj = new SurgicalHistory();
                                obj.Date_Of_Surgery = item.Date_Of_Surgery;
                                obj.Description = item.Description;
                                obj.Human_ID = item.Human_ID;
                                obj.Surgery_Name = item.Surgery_Name;
                                obj.Encounter_Id = ClientSession.EncounterId;
                                obj.Created_By = ClientSession.UserName;
                                obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                obj.Surgical_History_Master_ID = item.Id;
                                //CAP-1844
                                obj.Version = item.Version;
                                SaveLst.Add(obj);
                            }

                        }
                    }

                }
            }
            else
            {
                ulong ulSurgeryId = Convert.ToUInt32(Session["UpdateId"]);
                IList<ulong> lstId = new List<ulong>();
                if ((IList<SurgicalHistoryMaster>)Session["LoadSurgicalMasterList"] == null)
                {
                    if (ScreenMode == "Menu")
                    {
                        if (!lstId.Any(a => a.ToString() == ulSurgeryId.ToString()))
                        {
                            objSurgicalMasterHistory = objSurgicalHistoryDTO.SurgicalMasterList.Where(a => a.Id == ulSurgeryId).ToList<SurgicalHistoryMaster>()[0];
                            LoadObject();
                            objSurgicalMasterHistory.Modified_By = ClientSession.UserName;
                            objSurgicalMasterHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            UpdateMasterLst.Add(objSurgicalMasterHistory);
                        }
                    }
                    else
                    {
                        //CAP-790
                        if (objSurgicalHistoryDTO?.SurgicalList != null && objSurgicalHistoryDTO.SurgicalList.Count > 0)
                        {
                            foreach (SurgicalHistory item in objSurgicalHistoryDTO.SurgicalList)
                            {
                                if (item.Encounter_Id != ClientSession.EncounterId)
                                {
                                    SurgicalHistory obj = new SurgicalHistory();
                                    if (item.Id == ulSurgeryId)
                                    {
                                        obj.Date_Of_Surgery = FromComboBoxDate != string.Empty ? FromComboBoxDate : string.Empty;
                                        obj.Surgery_Name = txtSurgeryName.Text;
                                        obj.Description = DLC.txtDLC.Text;
                                    }
                                    else
                                    {
                                        obj.Description = item.Description;
                                        obj.Surgery_Name = item.Surgery_Name;
                                        obj.Date_Of_Surgery = item.Date_Of_Surgery;
                                    }

                                    lstId.Add(item.Id);
                                    obj.Encounter_Id = ClientSession.EncounterId;
                                    obj.Human_ID = item.Human_ID;
                                    obj.Created_By = ClientSession.UserName;
                                    obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                    obj.Modified_By = ClientSession.UserName;
                                    obj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                    SaveLst.Add(obj);
                                }
                            }
                        }
                        if (!lstId.Any(a => a.ToString() == ulSurgeryId.ToString()))
                        {
                            objSurgicalHistory = objSurgicalHistoryDTO.SurgicalList.Where(a => a.Id == ulSurgeryId).ToList<SurgicalHistory>()[0];
                            LoadObject();
                            objSurgicalHistory.Modified_By = ClientSession.UserName;
                            objSurgicalHistory.Encounter_Id = ClientSession.EncounterId;
                            objSurgicalHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            UpdateLst.Add(objSurgicalHistory);
                        }
                    }
                }
                else
                {
                    if ((ScreenMode == "Queue") && ((IList<SurgicalHistoryMaster>)Session["LoadSurgicalMasterList"]).Count > 0)
                    {
                        if (!lstId.Any(a => a.ToString() == ulSurgeryId.ToString()))
                        {
                            objSurgicalMasterHistory = ((IList<SurgicalHistoryMaster>)Session["LoadSurgicalMasterList"]).Where(a => a.Id == ulSurgeryId).ToList<SurgicalHistoryMaster>()[0];
                            LoadObject();
                            objSurgicalMasterHistory.Modified_By = ClientSession.UserName;
                            objSurgicalMasterHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            UpdateMasterLst.Add(objSurgicalMasterHistory);
                        }

                    }

                }
            }
            if ((IList<SurgicalHistoryMaster>)Session["LoadSurgicalMasterList"] == null)
            {
                if (ScreenMode == "Menu")
                {
                    SurgicalHistoryDTO surgilLst = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];
                    if (surgilLst != null)
                    {
                        SurgicalMasterList = surgilLst.SurgicalMasterList;
                    }
                    Session["SurgicalHistoryDTO"] = objSurgicalHistoryMasterManager.SurgicalSaveUpdateDelete(SurgicalMasterList, SaveMasterLst, UpdateMasterLst, DeleteMasterLst, HumanId, string.Empty);
                }
                else
                {
                    SurgicalHistoryDTO surgilLst = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];
                    if (surgilLst != null)
                    {
                        SurgclList = surgilLst.SurgicalList;
                    }
                    Session["SurgicalHistoryDTO"] = objSurgicalHistoryManager.SurgicalSaveUpdateDelete(SurgclList, SaveLst, UpdateLst, DeleteLst, HumanId, string.Empty, ClientSession.EncounterId);
                }
            }
            else if (((IList<SurgicalHistoryMaster>)Session["LoadSurgicalMasterList"]).Count > 0)
            {
                if (ScreenMode == "Queue")
                {
                    SurgicalHistoryDTO surgilLst = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];
                    if (surgilLst != null)
                    {
                        if (SaveLst.Count > 0 || UpdateLst.Count > 0 || DeleteLst.Count > 0)
                        {
                            SurgclList = surgilLst.SurgicalList;
                            Session["SurgicalHistoryDTO"] = objSurgicalHistoryManager.SurgicalSaveUpdateDelete(SurgclList, SaveLst, UpdateLst, DeleteLst, HumanId, string.Empty, ClientSession.EncounterId);
                            Session["LoadSurgicalMasterList"] = null;
                        }
                        else if (SaveMasterLst.Count > 0 || UpdateMasterLst.Count > 0 || DeleteMasterLst.Count > 0)
                        {
                            SurgicalMasterList = surgilLst.SurgicalMasterList;
                            Session["SurgicalHistoryDTO"] = objSurgicalHistoryMasterManager.SurgicalSaveUpdateDelete(SurgicalMasterList, SaveMasterLst, UpdateMasterLst, DeleteMasterLst, HumanId, string.Empty);
                        }
                    }

                }

            }

            System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
            if (text1 != null)
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
            LoadGridWithPageNavigator(true, (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"], false);
            ClearFields();
            btnAdd.Enabled = false;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PFSH", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}EnablePFSH(" + ClientSession.EncounterId + ");SavedSuccessfully();", true);
            ClientSession.bPFSHVerified = false;
            UIManager.IsPFSHVerified = true;
            dtpDateOfSurgery.clbCalendar.SelectedDate = DateTime.Now;


        }

        protected void pbDatabase_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "openAddorUpdate();", true);
        }

        protected void grdSurgeryHistoryDetails_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandArgument != null && e.CommandArgument != string.Empty)
            {
                GridItem Item = grdSurgeryHistoryDetails.Items[Convert.ToUInt16(e.CommandArgument)];
                if (e.CommandName.Trim() == "Edt")
                {
                    Hiddenupdate.Value = "UPDATE";

                    if (Item.Cells[4].Text.Trim() != string.Empty && Item.Cells[4].Text.Trim() != Space_Data)
                    {
                        string[] Date = Item.Cells[4].Text.Split('-');
                        if (Date != null)
                        {
                            dtpDateOfSurgery.cboDate.ClearSelection();
                            dtpDateOfSurgery.cboMonth.ClearSelection();
                            dtpDateOfSurgery.cboYear.ClearSelection();
                            if (Date.Length == 3)
                            {
                                dtpDateOfSurgery.AssignDate(Date[2], Date[1], Date[0]);
                                dtpDateOfSurgery.cboYear.SelectedIndex = dtpDateOfSurgery.cboYear.Items.FindItemIndexByText(Date[2]);
                                dtpDateOfSurgery.cboMonth.SelectedIndex = dtpDateOfSurgery.cboMonth.Items.FindItemIndexByText(Date[1]);
                                dtpDateOfSurgery.cboDate.SelectedIndex = dtpDateOfSurgery.cboDate.Items.FindItemIndexByText(Date[0]);
                            }
                            else if (Date.Length == 2)
                            {
                                dtpDateOfSurgery.AssignDate(Date[1], Date[0], null);
                                dtpDateOfSurgery.cboYear.SelectedIndex = dtpDateOfSurgery.cboYear.Items.FindItemIndexByText(Date[1]);
                                dtpDateOfSurgery.cboMonth.SelectedIndex = dtpDateOfSurgery.cboMonth.Items.FindItemIndexByText(Date[0]);
                            }
                            else
                            {
                                dtpDateOfSurgery.AssignDate(Date[0], null, null);
                                dtpDateOfSurgery.cboYear.SelectedIndex = dtpDateOfSurgery.cboYear.Items.FindItemIndexByText(Date[0]);
                            }
                        }
                    }
                    else
                    {
                        dtpDateOfSurgery.AssignDate(null, null, null);
                        dtpDateOfSurgery.cboDate.SelectedIndex = 0;
                    }

                    txtSurgeryName.Text = (Item.Cells[5].Text.Trim() != Space_Data) ? Item.Cells[5].Text : string.Empty;
                    DLC.txtDLC.Text = (Item.Cells[6].Text.Trim() != Space_Data) ? Item.Cells[6].Text : string.Empty;
                    Session["UpdateId"] = Item.Cells[7].Text;

                    System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
                    if (text1 != null)
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
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Autosave", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}GridEditAutoSaveEnable();", true);
                }
                else if (e.CommandName.Trim() == "DeleteRows")
                {
                    string strtime = string.Empty;
                    DateTime utc = DateTime.UtcNow;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                        utc = Convert.ToDateTime(strtime);
                    }
                    Session["UTCAuditDelete"] = utc;
                    btnAdd.Enabled = false;
                    hdnDelSurgicalId.Value = Item.Cells[7].Text;

                    if (ScreenMode == "Menu")
                    {
                        SurgicalHistoryDTO surglLst = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];
                        SurgicalMasterList = surglLst.SurgicalMasterList;
                        DeleteFromMaster(Convert.ToUInt64(grdSurgeryHistoryDetails.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[7].Text), SurgicalMasterList);
                    }
                    else
                    {
                        if ((IList<SurgicalHistoryMaster>)Session["LoadSurgicalMasterList"] != null && ((IList<SurgicalHistoryMaster>)Session["LoadSurgicalMasterList"]).Count > 0)
                        {
                            IList<SurgicalHistoryMaster> surglMasterLstToDelete = (IList<SurgicalHistoryMaster>)Session["LoadSurgicalMasterList"];
                            DeleteFromMaster(Convert.ToUInt64(grdSurgeryHistoryDetails.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[7].Text), surglMasterLstToDelete);
                        }
                        else
                        {
                            SurgicalHistoryDTO surglLst = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];
                            SurgclList = surglLst.SurgicalList;
                            IList<SurgicalHistory> DelList = new List<SurgicalHistory>();
                            IList<SurgicalHistory> DeleteSurgicalList = new List<SurgicalHistory>();
                            SurgicalHistory objSurgicalHistory = new SurgicalHistory();
                            ulong SurgicalID = 0;
                            if (grdSurgeryHistoryDetails.MasterTableView.Items.Count > 0)
                            {
                                SurgicalID = Convert.ToUInt64(grdSurgeryHistoryDetails.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[7].Text);
                            }
                            objSurgicalHistory = (from I in SurgclList where I.Id == SurgicalID select I).ToList<SurgicalHistory>()[0];
                            objSurgicalHistory.Modified_By = ClientSession.UserName;

                            objSurgicalHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            DelList.Add(objSurgicalHistory);
                            IList<SurgicalHistory> SaveList = new List<SurgicalHistory>();

                            if (DelList[0].Encounter_Id == ClientSession.EncounterId)
                                DeleteSurgicalList.Add(DelList[0]);
                            if (DeleteSurgicalList.Count > 0)
                            {
                                Session["SurgicalHistoryDTO"] = objSurgicalHistoryManager.SurgicalSaveUpdateDelete(SurgclList, SaveLst, UpdateLst, DeleteSurgicalList, HumanId, string.Empty, ClientSession.EncounterId);
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SurgicalPFSH", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('180037');EnablePFSH();", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ImmunHistory", "DisplayErrorMessage('380053'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            }
                        }

                    }
                    System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
                    if (text1 != null)
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
                    LoadGridWithPageNavigator(true, (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"], true);
                    ClearFields();

                }
            }
        }

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            if (ScreenMode == "Menu")
            {
                if (objSurgicalHistoryDTO != null && objSurgicalHistoryDTO.SurgicalMasterList.Count > 0)
                {
                    if (objSurgicalHistoryDTO.SurgicalMasterList.Where(a => a.Id == Convert.ToUInt32(hdnDelSurgicalId.Value)).ToList<SurgicalHistoryMaster>().Count > 0)
                    {
                        SurgicalHistoryMaster objLst = objSurgicalHistoryDTO.SurgicalMasterList.Where(a => a.Id == Convert.ToUInt32(hdnDelSurgicalId.Value)).ToList<SurgicalHistoryMaster>()[0];
                        if (objLst != null && DeleteMasterLst != null)
                            DeleteMasterLst.Add(objLst);
                    }
                }
                SurgicalHistoryDTO surglLst = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];
                SurgicalMasterList = surglLst.SurgicalMasterList;
                Session["SurgicalHistoryDTO"] = objSurgicalHistoryMasterManager.SurgicalSaveUpdateDelete(SurgicalMasterList, SaveMasterLst, UpdateMasterLst, DeleteMasterLst, HumanId, string.Empty);
            }
            else
            {
                if (objSurgicalHistoryDTO != null && objSurgicalHistoryDTO.SurgicalList.Count > 0)
                {
                    if (objSurgicalHistoryDTO.SurgicalList.Where(a => a.Id == Convert.ToUInt32(hdnDelSurgicalId.Value)).ToList<SurgicalHistory>().Count > 0)
                    {
                        SurgicalHistory objLst = objSurgicalHistoryDTO.SurgicalList.Where(a => a.Id == Convert.ToUInt32(hdnDelSurgicalId.Value)).ToList<SurgicalHistory>()[0];
                        if (objLst != null && DeleteLst != null)
                            DeleteLst.Add(objLst);
                    }
                }
                SurgicalHistoryDTO surglLst = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];
                SurgclList = surglLst.SurgicalList;
                Session["SurgicalHistoryDTO"] = objSurgicalHistoryManager.SurgicalSaveUpdateDelete(SurgclList, SaveLst, UpdateLst, DeleteLst, HumanId, string.Empty, ClientSession.EncounterId);
            }

            LoadGridWithPageNavigator(true, (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"], false);
            ClearFields();
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SurgicalPFSH", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('180037');EnablePFSH();", true);
        }
        protected void LibraryButton_Click(object sender, EventArgs e)
        {
            LoadSurgeryName();
        }
        protected void InvisibleClearAllButton_Click(object sender, EventArgs e)
        {

            btnAdd.Enabled = false;
            txtSurgeryName.Text = string.Empty;
            DLC.txtDLC.Text = string.Empty;
            btnClearAll.Text = "Clear All";
            dtpDateOfSurgery.AssignDate(null, null, null);
            dtpDateOfSurgery.cboDate.SelectedIndex = 0;
            dtpDateOfSurgery.cboMonth.SelectedIndex = 0;
            dtpDateOfSurgery.cboYear.SelectedIndex = 0;

            System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
            if (text1 != null)
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
        }
        public void FirstPageNavigator(object sender, EventArgs e)
        {
            LoadGridWithPageNavigator(true, null, false);
        }

        #endregion

        #region Methods

        public void LoadGridWithPageNavigator(bool Is_Load, SurgicalHistoryDTO objSurgicalHistoryDTOs, bool is_delete)
        {
            if (ScreenMode == "Menu")
            {
                LoadFromMaster(Is_Load, objSurgicalHistoryDTO, is_delete);
            }
            else
            {
                if (Is_Load)
                {
                    IList<object> ilstInSurgicalHistoryBlobFinal = new List<object>();
                    IList<string> ilstSurgicalHistoryTagList = new List<string>();
                    bool _is_from_current_encounter_data = false;
                    SurgicalHistoryDTO SurgHistDTO = new SurgicalHistoryDTO();
                    IList<SurgicalHistory> SurgHislst = new List<SurgicalHistory>();

                    #region Commented By Deepak


                    //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
                    //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                    //try
                    //{
                    //    if (File.Exists(strXmlFilePath) == true)
                    //    {
                    //        XmlDocument itemDoc = new XmlDocument();
                    //        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                    //        XmlNodeList xmlTagName = null;
                    //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    //        {
                    //            itemDoc.Load(fs);
                    //            XmlText.Close();
                    //            if (itemDoc.GetElementsByTagName("SurgicalHistoryList")[0] != null)
                    //            {
                    //                xmlTagName = itemDoc.GetElementsByTagName("SurgicalHistoryList")[0].ChildNodes;
                    //                if (xmlTagName != null && xmlTagName.Count > 0)
                    //                {
                    //                    for (int j = 0; j < xmlTagName.Count; j++)
                    //                    {
                    //                        string TagName = xmlTagName[j].Name;
                    //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(SurgicalHistory));
                    //                        SurgicalHistory SurgicalHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as SurgicalHistory;
                    //                        IEnumerable<PropertyInfo> propInfo = null;
                    //                        propInfo = from obji in ((SurgicalHistory)SurgicalHistory).GetType().GetProperties() select obji;
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
                    //                                                property.SetValue(SurgicalHistory, Convert.ToUInt64(nodevalue.Value), null);
                    //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
                    //                                                property.SetValue(SurgicalHistory, Convert.ToString(nodevalue.Value), null);
                    //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                    //                                                property.SetValue(SurgicalHistory, Convert.ToDateTime(nodevalue.Value), null);
                    //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
                    //                                                property.SetValue(SurgicalHistory, Convert.ToInt32(nodevalue.Value), null);
                    //                                            else
                    //                                                property.SetValue(SurgicalHistory, nodevalue.Value, null);
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }
                    //                        }

                    //                        SurgHislst.Add(SurgicalHistory);
                    //                        if (SurgicalHistory.Encounter_Id == ClientSession.EncounterId)
                    //                            _is_from_current_encounter_data = true;
                    //                    }
                    //                    if (!_is_from_current_encounter_data)
                    //                    {
                    //                        SurgHislst.Clear();
                    //                        LoadFromMaster(Is_Load, objSurgicalHistoryDTO, is_delete);
                    //                        if (Session["SurgicalHistoryDTO"] != null)
                    //                            objSurgicalHistoryDTO = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];
                    //                    }
                    //                }
                    //            }
                    //            else
                    //            {
                    //                LoadFromMaster(Is_Load, objSurgicalHistoryDTO, is_delete);
                    //                if (Session["SurgicalHistoryDTO"] != null)
                    //                    objSurgicalHistoryDTO = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];
                    //            }

                    //            if (itemDoc.GetElementsByTagName("dob")[0] != null)
                    //            {
                    //                SurgHistDTO.PatientDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("dob")[0].InnerText);
                    //            }
                    //            fs.Close();
                    //            fs.Dispose();
                    //        }
                    //    }


                    //}
                    //catch (Exception ex)
                    //{
                    //    throw new Exception(ex.Message + " - " + strXmlFilePath);
                    //}
                    #endregion

                    ilstSurgicalHistoryTagList.Add("SurgicalHistoryList");
                    ilstInSurgicalHistoryBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstSurgicalHistoryTagList);

                    if (ilstInSurgicalHistoryBlobFinal != null && ilstInSurgicalHistoryBlobFinal.Count > 0)
                    {
                        if (ilstInSurgicalHistoryBlobFinal[0] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstInSurgicalHistoryBlobFinal[0]).Count; iCount++)
                            {
                                SurgHislst.Add((SurgicalHistory)((IList<object>)ilstInSurgicalHistoryBlobFinal[0])[iCount]);

                                if (((SurgicalHistory)((IList<object>)ilstInSurgicalHistoryBlobFinal[0])[iCount]).Encounter_Id == ClientSession.EncounterId)
                                    _is_from_current_encounter_data = true;
                            }
                            if (!_is_from_current_encounter_data)
                            {
                                SurgHislst.Clear();
                                LoadFromMaster(Is_Load, objSurgicalHistoryDTO, is_delete);
                                if (Session["SurgicalHistoryDTO"] != null)
                                    objSurgicalHistoryDTO = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];
                            }
                        }
                        else
                        {
                            LoadFromMaster(Is_Load, objSurgicalHistoryDTO, is_delete);
                            if (Session["SurgicalHistoryDTO"] != null)
                                objSurgicalHistoryDTO = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];
                        }
                    }
                        if (objSurgicalHistoryDTO != null && ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0 && (ClientSession.PatientPaneList[0]).Birth_Date != null)
                        {
                            objSurgicalHistoryDTO.PatientDOB = ClientSession.PatientPaneList[0].Birth_Date;
                        }




                    if (SurgHislst != null && SurgHislst.Count > 0)
                    {

                        IList<SurgicalHistory> lstSurgCurrEnc = new List<SurgicalHistory>();
                        lstSurgCurrEnc = (from item in SurgHislst where item.Encounter_Id == ClientSession.EncounterId select item).ToList<SurgicalHistory>();
                        if (lstSurgCurrEnc != null && lstSurgCurrEnc.Count > 0)
                        {
                            SurgHistDTO.SurgicalList = lstSurgCurrEnc;
                            SurgHistDTO.SurgicalCount = lstSurgCurrEnc.Count;
                        }
                        else if (!is_delete)
                        {
                            ulong maxEncId = 0;
                            IList<ulong> lstEncId = (from item in SurgHislst select item.Encounter_Id).Distinct().ToList<ulong>();
                            if (lstEncId != null && lstEncId.Count > 0)
                                maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                            foreach (ulong item in lstEncId)
                                if (item > maxEncId && item < ClientSession.EncounterId)
                                    maxEncId = item;
                            lstSurgCurrEnc = (from item in SurgHislst where item.Encounter_Id == maxEncId select item).ToList<SurgicalHistory>();
                            SurgHistDTO.SurgicalList = lstSurgCurrEnc;
                            SurgHistDTO.SurgicalCount = lstSurgCurrEnc.Count;
                        }
                    }
                    else
                    {
                        SurgHistDTO.SurgicalList = SurgHislst;
                        SurgHistDTO.SurgicalCount = 0;
                    }
                    Session["SurgicalHistoryDTO"] = SurgHistDTO;
                    objSurgicalHistoryDTO = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];

                }
                else
                {
                    Session["SurgicalHistoryDTO"] = objSurgicalHistoryDTOs;
                    objSurgicalHistoryDTO = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];
                }
                if (objSurgicalHistoryDTO != null)
                {
                    if (objSurgicalHistoryDTO != null && objSurgicalHistoryDTO.SurgicalList != null && objSurgicalHistoryDTO.SurgicalList.Count > 0)
                    {
                        grdSurgeryHistoryDetails.DataSource = new string[] { };
                        grdSurgeryHistoryDetails.DataBind();
                        IList<SurgicalHistory> SurgicalList = new List<SurgicalHistory>();
                        SurgicalList = objSurgicalHistoryDTO.SurgicalList;
                        foreach (SurgicalHistory obj in SurgicalList)
                        {
                            if (obj.Date_Of_Surgery.Trim() != string.Empty)
                                switch (obj.Date_Of_Surgery.Split('-').Count())
                                {
                                    case 1:
                                        objSurgicalHistoryDTO.dictSurgeryDate.Add(obj.Id, Convert.ToDateTime("01-Jan-" + obj.Date_Of_Surgery));
                                        break;
                                    case 2:
                                        objSurgicalHistoryDTO.dictSurgeryDate.Add(obj.Id, Convert.ToDateTime("01-" + obj.Date_Of_Surgery));
                                        break;
                                    case 3:
                                        objSurgicalHistoryDTO.dictSurgeryDate.Add(obj.Id, Convert.ToDateTime(obj.Date_Of_Surgery));
                                        break;
                                    default:
                                        break;
                                }
                            else
                                objSurgicalHistoryDTO.dictSurgeryDate.Add(obj.Id, DateTime.MinValue);
                        }
                        objSurgicalHistoryDTO.dictSurgeryDate = (from keyval in objSurgicalHistoryDTO.dictSurgeryDate orderby keyval.Value descending select keyval).ToDictionary(keyval => keyval.Key, keyval => keyval.Value);
                        IList<SurgicalHistory> tempSurgicalList = new List<SurgicalHistory>();
                        foreach (KeyValuePair<ulong, DateTime> item in objSurgicalHistoryDTO.dictSurgeryDate)
                        {
                            if (item.Value != DateTime.MinValue)
                            {
                                tempSurgicalList.Add((from ob in SurgicalList where ob.Id == item.Key select ob).ToList<SurgicalHistory>()[0]);
                            }
                        }
                        objSurgicalHistoryDTO.dictSurgeryDate = (from keyval in objSurgicalHistoryDTO.dictSurgeryDate where keyval.Value == DateTime.MinValue select keyval).ToDictionary(keyval => keyval.Key, keyval => keyval.Value);
                        if (objSurgicalHistoryDTO.dictSurgeryDate.Count > 0)
                        {
                            for (int i = 0; i < objSurgicalHistoryDTO.dictSurgeryDate.Count; i++)
                            {
                                SurgicalHistory objSurDate = (from objSurg in SurgicalList where objSurgicalHistoryDTO.dictSurgeryDate.Keys.ElementAt(i) == objSurg.Id select objSurg).ToList<SurgicalHistory>()[0];
                                if (DateTime.Compare(objSurDate.Modified_Date_And_Time, objSurDate.Created_Date_And_Time) >= 0)
                                    objSurgicalHistoryDTO.dictSurgeryDate[objSurgicalHistoryDTO.dictSurgeryDate.Keys.ElementAt(i)] = objSurDate.Modified_Date_And_Time;
                                else
                                    objSurgicalHistoryDTO.dictSurgeryDate[objSurgicalHistoryDTO.dictSurgeryDate.Keys.ElementAt(i)] = objSurDate.Created_Date_And_Time;
                            }
                        }

                        objSurgicalHistoryDTO.dictSurgeryDate = (from keyval in objSurgicalHistoryDTO.dictSurgeryDate orderby keyval.Value descending select keyval).ToDictionary(keyval => keyval.Key, keyval => keyval.Value);

                        foreach (KeyValuePair<ulong, DateTime> item in objSurgicalHistoryDTO.dictSurgeryDate)
                            tempSurgicalList.Add((from ob in SurgicalList where ob.Id == item.Key select ob).ToList<SurgicalHistory>()[0]);

                        grdSurgeryHistoryDetails.DataSource = null;
                        DataTable objDataTable = new DataTable();
                        DataColumn objDataColumn = new DataColumn();
                        objDataColumn = new DataColumn("DateOfSurgery", typeof(string));
                        objDataTable.Columns.Add(objDataColumn);
                        objDataColumn = new DataColumn("SurgeryName", typeof(string));
                        objDataTable.Columns.Add(objDataColumn);
                        objDataColumn = new DataColumn("SurgeryNotes", typeof(string));
                        objDataTable.Columns.Add(objDataColumn);
                        objDataColumn = new DataColumn("ID", typeof(string));
                        objDataTable.Columns.Add(objDataColumn);

                        for (int i = 0; i < tempSurgicalList.Count; i++)
                        {
                            DataRow objDataRow = objDataTable.NewRow();
                            objDataRow["DateOfSurgery"] = tempSurgicalList[i].Date_Of_Surgery;
                            objDataRow["SurgeryName"] = tempSurgicalList[i].Surgery_Name;
                            objDataRow["SurgeryNotes"] = tempSurgicalList[i].Description;
                            objDataRow["ID"] = tempSurgicalList[i].Id.ToString();
                            objDataTable.Rows.Add(objDataRow);
                        }

                        grdSurgeryHistoryDetails.DataSource = objDataTable;
                        grdSurgeryHistoryDetails.DataBind();
                    }
                    else
                    {
                        if (grdSurgeryHistoryDetails.DataSource == null)
                        {
                            grdSurgeryHistoryDetails.DataSource = new string[] { };
                            grdSurgeryHistoryDetails.DataBind();
                        }
                    }
                }
                else
                {
                    grdSurgeryHistoryDetails.DataSource = new string[] { };
                    grdSurgeryHistoryDetails.DataBind();
                }

            }

        }

        public bool SurgeryNotesDuplicate()
        {
            string[] aryValue = DLC.txtDLC.Text.Split(',');
            for (int i = 0; i < aryValue.Count(); i++)
                aryValue[i] = aryValue[i].Trim();
            if (aryValue != null && aryValue.Length > 0)
            {
                HashSet<string> hashSet = new HashSet<string>(aryValue);
                if (hashSet.Count != aryValue.Length)
                    return true;
            }
            return false;
        }

        public void ClearFields()
        {
            btnAdd.Text = "Add";
            btnClearAll.Text = "Clear All";
            txtSurgeryName.Text = string.Empty;
            DLC.txtDLC.Text = string.Empty;

            dtpDateOfSurgery.AssignDate(null, null, null);
            dtpDateOfSurgery.cboDate.SelectedIndex = 0;
            dtpDateOfSurgery.cboMonth.SelectedIndex = 0;
            dtpDateOfSurgery.cboYear.SelectedIndex = 0;


            dtpDateOfSurgery.cboDate.ClearSelection();
            dtpDateOfSurgery.cboMonth.ClearSelection();
            dtpDateOfSurgery.cboYear.ClearSelection();
        }

        public void LoadSurgeryName()
        {
            lstSurgeryName.Items.Clear();
            Session["FieldLookupList"] = objUserLookupManager.GetFieldLookupList(PhysicianId, "SURGERY NAME", "Value").ToArray();
            IList<FieldLookup> lstFieldLookup = (IList<FieldLookup>)Session["FieldLookupList"];
            if (lstFieldLookup != null)
            {
                for (int i = 0; i < lstFieldLookup.Count; i++)
                    lstSurgeryName.Items.Add(new RadListBoxItem(lstFieldLookup[i].Value));
            }
        }

        public void LoadObject()
        {
            if ((IList<SurgicalHistoryMaster>)Session["LoadSurgicalMasterList"] == null)
            {
                if (ScreenMode == "Menu")
                {
                    objSurgicalMasterHistory.Date_Of_Surgery = FromComboBoxDate != string.Empty ? FromComboBoxDate : string.Empty;
                    objSurgicalMasterHistory.Surgery_Name = txtSurgeryName.Text;
                    objSurgicalMasterHistory.Description = DLC.txtDLC.Text;
                    objSurgicalMasterHistory.Is_Deleted = "N";
                }
                else
                {
                    objSurgicalHistory.Date_Of_Surgery = FromComboBoxDate != string.Empty ? FromComboBoxDate : string.Empty;
                    objSurgicalHistory.Surgery_Name = txtSurgeryName.Text;
                    objSurgicalHistory.Description = DLC.txtDLC.Text;
                }
            }
            else if (((IList<SurgicalHistoryMaster>)Session["LoadSurgicalMasterList"]).Count > 0)
            {
                if (ScreenMode == "Queue")
                {
                    if (string.Compare(Convert.ToString(Hiddenupdate.Value), "ADD", true) == 0)
                    {
                        objSurgicalHistory.Date_Of_Surgery = FromComboBoxDate != string.Empty ? FromComboBoxDate : string.Empty;
                        objSurgicalHistory.Surgery_Name = txtSurgeryName.Text;
                        objSurgicalHistory.Description = DLC.txtDLC.Text;
                    }
                    else
                    {
                        objSurgicalMasterHistory.Date_Of_Surgery = FromComboBoxDate != string.Empty ? FromComboBoxDate : string.Empty;
                        objSurgicalMasterHistory.Surgery_Name = txtSurgeryName.Text;
                        objSurgicalMasterHistory.Description = DLC.txtDLC.Text;
                        objSurgicalMasterHistory.Is_Deleted = "N";
                    }
                }

            }
        }


        public void LoadFromMaster(bool Is_Load, SurgicalHistoryDTO objSurgicalHistoryDTOs, bool is_delete)
        {
            if (Is_Load)
            {
                SurgicalHistoryDTO SurgHistDTO = new SurgicalHistoryDTO();
                IList<SurgicalHistoryMaster> SurgicalLoadMasterlst = new List<SurgicalHistoryMaster>();
                IList<object> ilstInSurgicalLoadMasterlstBlobFinal = new List<object>();
                IList<string> ilstSurgicalLoadMasterlstTagList = new List<string>();

                #region Commented By Deepak
                
                //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                //try
                //{
                //    if (File.Exists(strXmlFilePath) == true)
                //    {
                //        XmlDocument itemDoc = new XmlDocument();
                //        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                //        XmlNodeList xmlTagName = null;
                //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //        {
                //            itemDoc.Load(fs);
                //            XmlText.Close();
                //            if (itemDoc.GetElementsByTagName("SurgicalHistoryMasterList")[0] != null)
                //            {
                //                xmlTagName = itemDoc.GetElementsByTagName("SurgicalHistoryMasterList")[0].ChildNodes;
                //                if (xmlTagName != null && xmlTagName.Count > 0)
                //                {
                //                    for (int j = 0; j < xmlTagName.Count; j++)
                //                    {
                //                        string TagName = xmlTagName[j].Name;
                //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(SurgicalHistoryMaster));
                //                        SurgicalHistoryMaster objMasterSurgicalHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as SurgicalHistoryMaster;
                //                        IEnumerable<PropertyInfo> propInfo = null;
                //                        propInfo = from obji in ((SurgicalHistoryMaster)objMasterSurgicalHistory).GetType().GetProperties() select obji;
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
                //                                                property.SetValue(objMasterSurgicalHistory, Convert.ToUInt64(nodevalue.Value), null);
                //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                                property.SetValue(objMasterSurgicalHistory, Convert.ToString(nodevalue.Value), null);
                //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                                property.SetValue(objMasterSurgicalHistory, Convert.ToDateTime(nodevalue.Value), null);
                //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                                property.SetValue(objMasterSurgicalHistory, Convert.ToInt32(nodevalue.Value), null);
                //                                            else
                //                                                property.SetValue(objMasterSurgicalHistory, nodevalue.Value, null);
                //                                        }
                //                                    }
                //                                }
                //                            }
                //                        }
                //                        if (objMasterSurgicalHistory.Is_Deleted != "Y")
                //                            SurgicalLoadMasterlst.Add(objMasterSurgicalHistory);
                //                    }
                //                }
                //            }

                //            if (itemDoc.GetElementsByTagName("dob")[0] != null)
                //            {
                //                SurgHistDTO.PatientDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("dob")[0].InnerText);
                //            }
                //            fs.Close();
                //            fs.Dispose();
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception(ex.Message + " - " + strXmlFilePath);
                //}
                #endregion
                ilstSurgicalLoadMasterlstTagList.Add("SurgicalHistoryMasterList");
                ilstInSurgicalLoadMasterlstBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstSurgicalLoadMasterlstTagList);

                if (ilstInSurgicalLoadMasterlstBlobFinal != null && ilstInSurgicalLoadMasterlstBlobFinal.Count > 0)
                {
                    if (ilstInSurgicalLoadMasterlstBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstInSurgicalLoadMasterlstBlobFinal[0]).Count; iCount++)
                        {
                            if (((SurgicalHistoryMaster)((IList<object>)ilstInSurgicalLoadMasterlstBlobFinal[0])[iCount]).Is_Deleted != "Y")
                            {
                                SurgicalLoadMasterlst.Add((SurgicalHistoryMaster)((IList<object>)ilstInSurgicalLoadMasterlstBlobFinal[0])[iCount]);
                            }
                        }
                    }
                }

                if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0 && (ClientSession.PatientPaneList[0]).Birth_Date != null)
                {
                    SurgHistDTO.PatientDOB = (ClientSession.PatientPaneList[0]).Birth_Date;
                }


                if (SurgicalLoadMasterlst != null && SurgicalLoadMasterlst.Count > 0)
                {
                    if ((ScreenMode == "Queue") || (ScreenMode == "Portal"))
                        Session["LoadSurgicalMasterList"] = SurgicalLoadMasterlst;
                    SurgHistDTO.SurgicalMasterList = SurgicalLoadMasterlst;
                    SurgHistDTO.SurgicalCount = SurgicalLoadMasterlst.Count;
                    if (!is_delete)
                    {
                        //ulong maxEncId = 0;
                        //IList<ulong> lstEncId = (from item in SurgHislst select item.Encounter_Id).Distinct().ToList<ulong>();
                        //if (lstEncId != null && lstEncId.Count > 0)
                        //    maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                        //foreach (ulong item in lstEncId)
                        //    if (item > maxEncId && item < ClientSession.EncounterId)
                        //        maxEncId = item;
                        //lstSurgCurrEnc = (from item in SurgHislst where item.Encounter_Id == maxEncId select item).ToList<SurgicalHistory>();
                        //SurgHistDTO.SurgicalList = lstSurgCurrEnc;
                        //SurgHistDTO.SurgicalCount = lstSurgCurrEnc.Count;
                    }
                }
                else
                {
                    SurgHistDTO.SurgicalMasterList = SurgicalLoadMasterlst;
                    SurgHistDTO.SurgicalCount = SurgicalLoadMasterlst.Count;
                    if (ScreenMode == "Queue")
                        Session["LoadSurgicalMasterList"] = null;
                }
                Session["SurgicalHistoryDTO"] = SurgHistDTO;
                objSurgicalHistoryDTO = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];

            }
            else
            {
                Session["SurgicalHistoryDTO"] = objSurgicalHistoryDTO;
                objSurgicalHistoryDTO = (SurgicalHistoryDTO)Session["SurgicalHistoryDTO"];
            }
            if (objSurgicalHistoryDTO != null)
            {
                if (objSurgicalHistoryDTO != null && objSurgicalHistoryDTO.SurgicalMasterList != null && objSurgicalHistoryDTO.SurgicalMasterList.Count > 0)
                {
                    grdSurgeryHistoryDetails.DataSource = new string[] { };
                    grdSurgeryHistoryDetails.DataBind();
                    IList<SurgicalHistoryMaster> SurgicalList = new List<SurgicalHistoryMaster>();
                    SurgicalList = objSurgicalHistoryDTO.SurgicalMasterList;
                    foreach (SurgicalHistoryMaster obj in SurgicalList)
                    {
                        if (obj.Date_Of_Surgery.Trim() != string.Empty)
                            switch (obj.Date_Of_Surgery.Split('-').Count())
                            {
                                case 1:
                                    objSurgicalHistoryDTO.dictSurgeryDate.Add(obj.Id, Convert.ToDateTime("01-Jan-" + obj.Date_Of_Surgery));
                                    break;
                                case 2:
                                    objSurgicalHistoryDTO.dictSurgeryDate.Add(obj.Id, Convert.ToDateTime("01-" + obj.Date_Of_Surgery));
                                    break;
                                case 3:
                                    objSurgicalHistoryDTO.dictSurgeryDate.Add(obj.Id, Convert.ToDateTime(obj.Date_Of_Surgery));
                                    break;
                                default:
                                    break;
                            }
                        else
                            objSurgicalHistoryDTO.dictSurgeryDate.Add(obj.Id, DateTime.MinValue);
                    }
                    objSurgicalHistoryDTO.dictSurgeryDate = (from keyval in objSurgicalHistoryDTO.dictSurgeryDate orderby keyval.Value descending select keyval).ToDictionary(keyval => keyval.Key, keyval => keyval.Value);
                    IList<SurgicalHistoryMaster> tempSurgicalList = new List<SurgicalHistoryMaster>();
                    foreach (KeyValuePair<ulong, DateTime> item in objSurgicalHistoryDTO.dictSurgeryDate)
                    {
                        if (item.Value != DateTime.MinValue)
                        {
                            tempSurgicalList.Add((from ob in SurgicalList where ob.Id == item.Key select ob).ToList<SurgicalHistoryMaster>()[0]);
                        }
                    }
                    objSurgicalHistoryDTO.dictSurgeryDate = (from keyval in objSurgicalHistoryDTO.dictSurgeryDate where keyval.Value == DateTime.MinValue select keyval).ToDictionary(keyval => keyval.Key, keyval => keyval.Value);
                    if (objSurgicalHistoryDTO.dictSurgeryDate.Count > 0)
                    {
                        for (int i = 0; i < objSurgicalHistoryDTO.dictSurgeryDate.Count; i++)
                        {
                            SurgicalHistoryMaster objSurDate = (from objSurg in SurgicalList where objSurgicalHistoryDTO.dictSurgeryDate.Keys.ElementAt(i) == objSurg.Id select objSurg).ToList<SurgicalHistoryMaster>()[0];
                            if (DateTime.Compare(objSurDate.Modified_Date_And_Time, objSurDate.Created_Date_And_Time) >= 0)
                                objSurgicalHistoryDTO.dictSurgeryDate[objSurgicalHistoryDTO.dictSurgeryDate.Keys.ElementAt(i)] = objSurDate.Modified_Date_And_Time;
                            else
                                objSurgicalHistoryDTO.dictSurgeryDate[objSurgicalHistoryDTO.dictSurgeryDate.Keys.ElementAt(i)] = objSurDate.Created_Date_And_Time;
                        }
                    }

                    objSurgicalHistoryDTO.dictSurgeryDate = (from keyval in objSurgicalHistoryDTO.dictSurgeryDate orderby keyval.Value descending select keyval).ToDictionary(keyval => keyval.Key, keyval => keyval.Value);

                    foreach (KeyValuePair<ulong, DateTime> item in objSurgicalHistoryDTO.dictSurgeryDate)
                        tempSurgicalList.Add((from ob in SurgicalList where ob.Id == item.Key select ob).ToList<SurgicalHistoryMaster>()[0]);

                    grdSurgeryHistoryDetails.DataSource = null;
                    DataTable objDataTable = new DataTable();
                    DataColumn objDataColumn = new DataColumn();
                    objDataColumn = new DataColumn("DateOfSurgery", typeof(string));
                    objDataTable.Columns.Add(objDataColumn);
                    objDataColumn = new DataColumn("SurgeryName", typeof(string));
                    objDataTable.Columns.Add(objDataColumn);
                    objDataColumn = new DataColumn("SurgeryNotes", typeof(string));
                    objDataTable.Columns.Add(objDataColumn);
                    objDataColumn = new DataColumn("ID", typeof(string));
                    objDataTable.Columns.Add(objDataColumn);

                    for (int i = 0; i < tempSurgicalList.Count; i++)
                    {
                        DataRow objDataRow = objDataTable.NewRow();
                        objDataRow["DateOfSurgery"] = tempSurgicalList[i].Date_Of_Surgery;
                        objDataRow["SurgeryName"] = tempSurgicalList[i].Surgery_Name;
                        objDataRow["SurgeryNotes"] = tempSurgicalList[i].Description;
                        objDataRow["ID"] = tempSurgicalList[i].Id.ToString();
                        objDataTable.Rows.Add(objDataRow);
                    }

                    grdSurgeryHistoryDetails.DataSource = objDataTable;
                    grdSurgeryHistoryDetails.DataBind();
                }
                else
                {
                    if (grdSurgeryHistoryDetails.DataSource == null)
                    {
                        grdSurgeryHistoryDetails.DataSource = new string[] { };
                        grdSurgeryHistoryDetails.DataBind();
                    }
                }
            }
            else
            {
                grdSurgeryHistoryDetails.DataSource = new string[] { };
                grdSurgeryHistoryDetails.DataBind();
            }
        }

        public void DeleteFromMaster(ulong updatedMasterID, IList<SurgicalHistoryMaster> SurgicalMasterListToBeDeleted)
        {
            IList<SurgicalHistoryMaster> DelList = new List<SurgicalHistoryMaster>();
            IList<SurgicalHistoryMaster> updateDeleteSurgicalMasterList = new List<SurgicalHistoryMaster>();
            SurgicalHistoryMaster objSurgicalHistoryMaster = new SurgicalHistoryMaster();
            objSurgicalHistoryMaster = (from I in SurgicalMasterListToBeDeleted where I.Id == updatedMasterID select I).ToList<SurgicalHistoryMaster>()[0];
            objSurgicalHistoryMaster.Is_Deleted = "Y";
            objSurgicalHistoryMaster.Modified_By = ClientSession.UserName;
            objSurgicalHistoryMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            DelList.Add(objSurgicalHistoryMaster);
            IList<SurgicalHistoryMaster> SaveListMaster = new List<SurgicalHistoryMaster>();

            if (DelList.Count > 0)
                updateDeleteSurgicalMasterList.Add(DelList[0]);
            if (updateDeleteSurgicalMasterList.Count > 0)
            {
                Session["SurgicalHistoryDTO"] = objSurgicalHistoryMasterManager.SurgicalSaveUpdateDelete(SurgicalMasterList, SaveListMaster, updateDeleteSurgicalMasterList, null, HumanId, string.Empty);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SurgicalPFSH", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('180037');EnablePFSH();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ImmunHistory", "DisplayErrorMessage('380053'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }

        }


        #endregion
    }
}
