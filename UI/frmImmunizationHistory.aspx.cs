using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Acurus.Capella.Core.DomainObjects;
using System.Net;
using System.IO;
using Acurus.Capella.Core.DTO;
using System.Collections;
using System.Runtime.Serialization;
using System.Data;
using System.Drawing;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Text;
using Acurus.Capella.UI;
using Telerik.Web.UI;
using Acurus.Capella.UI.UserControls;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;


namespace Acurus.Capella.UI
{
    public partial class frmImmunizationHistory : SessionExpired
    {
        IList<ImmunizationHistory> ImmList = new List<ImmunizationHistory>();
        ImmunizationHistoryManager immunizationMng = new ImmunizationHistoryManager();
        IList<ImmunizationMasterHistory> ImmMasterList = new List<ImmunizationMasterHistory>();
        ImmunizationMasterHistoryManager immunizationMasterManager = new ImmunizationMasterHistoryManager();
        ImmunizationHistoryDTO LoadPhyProcedAndVaccAndPhyCodeLib = new ImmunizationHistoryDTO();
        string ScreenMode = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
          

            frmImmunizationHistory _source = (frmImmunizationHistory)sender;
            ScreenMode = _source.Page.Request.Params[0];
            try
            {
                if (!IsPostBack)
                {
                    grdImmunization.DataSource = new string[] { };
                    grdImmunization.DataBind();

                    ClientSession.FlushSession();
                    Hiddenupdate.Value = "ADD";
                    DLC.DName = "pbDropdown";
                    btnSave.Enabled = false;
                    PhysicianProcedureManager phyProcedureManager = new PhysicianProcedureManager();
                    StaticLookupManager LookUpMngr = new StaticLookupManager();
                    LoadPhyProcedAndVaccAndPhyCodeLib = immunizationMng.GetLoadPhyProcedAndVaccAndPhyCodeLib(ClientSession.PhysicianId, "IMMUNIZATION PROCEDURE", 0, ClientSession.LegalOrg);
                    ViewState["LoadPhyProcedAndVaccAndPhyCodeLib"] = LoadPhyProcedAndVaccAndPhyCodeLib;
                    if (LoadPhyProcedAndVaccAndPhyCodeLib.PhysicianProcedure != null && LoadPhyProcedAndVaccAndPhyCodeLib.PhysicianProcedure.Count > 0)
                    {
                        //Cap - 3256
                        //IList<PhysicianProcedure> Procedure = (from p in LoadPhyProcedAndVaccAndPhyCodeLib.PhysicianProcedure where p.Physician_Procedure_Code.StartsWith("J") == false select p).ToList<PhysicianProcedure>();
                        IList<PhysicianProcedure> Procedure = (from p in LoadPhyProcedAndVaccAndPhyCodeLib.PhysicianProcedure  select p).ToList<PhysicianProcedure>();
                        fillImmunizationHistory(Procedure);
                    }

                    DLC.txtDLC.Attributes.Add("onkeypress", "EnableSave();");
                    DLC.txtDLC.Attributes.Add("onchange", "EnableSave();");
                    LoadGridPageNavigator(false);
                    string[] FieldName = { "ROUTE OF ADMINISTRATION", "IMMUNIZATIONLOCATION", "IMMUNIZATION SOURCE", "DOSE", "IMMUNIZATION PROTECTION TYPE", "IMMUNIZATION ADMINISTRATION UNITS" };
                    IList<StaticLookup> StaticList = LookUpMngr.getStaticLookupByFieldName(FieldName);
                    IList<StaticLookup> LookupList = null;
                    btnManageFrequentlyUsedImmunProc.Attributes.Add("onClick", "return clickManageFrequentlyUser()");
                    LookupList = StaticList.Where(l => l.Field_Name == "ROUTE OF ADMINISTRATION").ToList<StaticLookup>();
                    if (LookupList != null && LookupList.Count > 0)
                    {
                        cboRouteOfAdminstration.Items.Add(new RadComboBoxItem(""));
                        for (int i = 0; i < LookupList.Count; i++)
                            cboRouteOfAdminstration.Items.Add(new RadComboBoxItem(LookupList[i].Value.ToString()));
                    }
                    LookupList.Clear();
                    LookupList = StaticList.Where(l => l.Field_Name == "IMMUNIZATIONLOCATION").ToList<StaticLookup>();
                    if (LookupList != null && LookupList.Count > 0)
                    {
                        cboLocation.Items.Add(new RadComboBoxItem(""));
                        for (int i = 0; i < LookupList.Count; i++)
                            cboLocation.Items.Add(new RadComboBoxItem(LookupList[i].Value.ToString()));
                    }
                    LookupList.Clear();
                    LookupList = StaticList.Where(l => l.Field_Name == "IMMUNIZATION SOURCE").ToList<StaticLookup>();
                    if (LookupList != null && LookupList.Count > 0)
                    {
                        cboImmunizationSource.Items.Add(new RadComboBoxItem(""));
                        for (int i = 0; i < LookupList.Count; i++)
                        {
                            cboImmunizationSource.Items.Add(new RadComboBoxItem(LookupList[i].Value));
                        }
                        cboImmunizationSource.Items.Add(new RadComboBoxItem("Refused Administration"));
                    }
                    LookupList.Clear();
                    LookupList = StaticList.Where(l => l.Field_Name == "DOSE").ToList<StaticLookup>();
                    if (LookupList != null && LookupList.Count > 0)
                    {
                        cboDosetotal.Items.Add(new RadComboBoxItem(""));
                        for (int i = 0; i < LookupList.Count; i++)
                            cboDosetotal.Items.Add(new RadComboBoxItem(LookupList[i].Value));
                    }
                    LookupList.Clear();
                    LookupList = StaticList.Where(l => l.Field_Name == "IMMUNIZATION PROTECTION TYPE").ToList<StaticLookup>();
                    if (LookupList != null && LookupList.Count > 0)
                    {
                        cboProtectionState.Items.Add(new RadComboBoxItem(""));
                        for (int i = 0; i < LookupList.Count; i++)
                            cboProtectionState.Items.Add(new RadComboBoxItem(LookupList[i].Value));
                    }
                    LookupList.Clear();
                    LookupList = StaticList.Where(l => l.Field_Name == "IMMUNIZATION ADMINISTRATION UNITS").ToList<StaticLookup>();
                    if (LookupList != null && LookupList.Count > 0)
                    {
                        cboAdminUnit.Items.Add(new RadComboBoxItem(""));
                        for (int i = 0; i < LookupList.Count; i++)
                            cboAdminUnit.Items.Add(new RadComboBoxItem(LookupList[i].Value));
                    }
                    this.cboManufacturer.Items.Add(new RadComboBoxItem(""));
                    if (LoadPhyProcedAndVaccAndPhyCodeLib.VaccineManufacturerCodes != null && LoadPhyProcedAndVaccAndPhyCodeLib.VaccineManufacturerCodes.Count != 0)
                    {
                        foreach (VaccineManufacturerCodes obj1 in LoadPhyProcedAndVaccAndPhyCodeLib.VaccineManufacturerCodes)
                        {
                            RadComboBoxItem item = new RadComboBoxItem();
                            item.Text = obj1.Manufacturer_Name;
                            item.ToolTip = obj1.Manufacturer_Name;
                            item.Value = obj1.MVX_Code;
                            cboManufacturer.Items.Add(item);
                        }
                    }

                    dpDateOnVIS.MaxDate = DateTime.Now;
                    dpVisgiven.MaxDate = DateTime.Now;
                    DLC.txtDLC.Attributes.Add("onkeypress", "return keyValues();");

                    ClientSession.processCheck = true;
                    if (UIManager.is_Menu_Level_PFSH)//(UIManager.PFSH_OpeingFrom == "Menu")
                    {
                        this.Page.Items.Add("Title", "frmImmunizationHistoryMenu");
                    }
                    ClientSession.processCheck = true;

                }

                if (btnSave.Text == "Add")
                {
                    Hiddenupdate.Value = "ADD";
                }
                else
                {
                    Hiddenupdate.Value = "UPDATE";
                    btnClearAll.Text = "Cancel";
                    System.Web.UI.HtmlControls.HtmlGenericControl textClear = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                    textClear.InnerText = "C";
                    System.Web.UI.HtmlControls.HtmlGenericControl textClearAdd = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                    textClearAdd.InnerText = "ancel";
                }
                if (HiddenField2.Value == "true")
                {
                    string[] split = txtImmunizationProcedure.Text.Split('-');
                    if (txtImmunizationProcedure.Text != "")
                    {
                        LoadPhyProcedAndVaccAndPhyCodeLib = (ImmunizationHistoryDTO)ViewState["LoadPhyProcedAndVaccAndPhyCodeLib"];
                        if (LoadPhyProcedAndVaccAndPhyCodeLib.ProcedureCodeLibrary != null && LoadPhyProcedAndVaccAndPhyCodeLib.ProcedureCodeLibrary.Count > 0)
                        {
                            IList<ProcedureCodeLibrary> proList = (from p in LoadPhyProcedAndVaccAndPhyCodeLib.ProcedureCodeLibrary where p.Procedure_Code == split[0] select p).ToList<ProcedureCodeLibrary>(); //(split[0]);
                            if (proList != null)
                            {
                                if (proList.Count > 0)
                                    txtCVXcode.Text = proList[0].CVX_Code;
                            }
                        }
                    }
                    else
                    {
                        txtCVXcode.Text = "";
                    }
                    HiddenField2.Value = "";
                    btnSave.Enabled = true;
                    dpVisgiven.Enabled = false;
                    dpDateOnVIS.Enabled = false;
                }
                else if (HiddenField2.Value == "false")
                {
                    btnSave.Enabled = false;
                }

                if (!chkVisgiven.Checked)
                {
                    dpVisgiven.Enabled = false;
                    dpDateOnVIS.Enabled = false;
                }
                else
                {
                    dpVisgiven.Enabled = true;
                    dpDateOnVIS.Enabled = true;
                    btnSave.Enabled = true;
                }
                if (!IsPostBack)
                {
                    SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                    objSecurity.ApplyUserPermissions(this.Page);


                    if (ClientSession.UserCurrentOwner.Trim() == "UNKNOWN" || (!ClientSession.CheckUser && ClientSession.UserPermission == "R"))//if(UIManager.PFSH_OpeingFrom=="Menu")
                    {
                        dpVisgiven.Enabled = false;
                        dpDateOnVIS.Enabled = false;
                        chklstImmunizationHistory.Enabled = false;
                        cdtAdministeredDate.Enable = false;
                        cdtAdministeredDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                        btnSave.Enabled = false;
                        pnlImmunizationHistory.Enabled = false;
                        chkVisgiven.Enabled = false;
                        pnlDLC.Enabled = false;
                        DLC.Enable = false;
                    }
                    if (cboLocation.Enabled == false)
                    {
                        chklstImmunizationHistory.Enabled = false;
                        cdtAdministeredDate.Enable = false;
                        cdtAdministeredDate.RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                    }
                    // Moved the below statement to initially disable the button-By Pujhitha
                    btnSave.Enabled = false;
                    dpVisgiven.Enabled = false;
                    dpDateOnVIS.Enabled = false;
                }
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            catch (Exception ExLoad)
            {
                throw ExLoad;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Immunization Load", "alert('" + ExLoad.Message.Replace("'", " ") + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true); 
            }
        }
        public void fillImmunizationHistory(IList<PhysicianProcedure> immProList)
        {
            chklstImmunizationHistory.Items.Clear();
            IList<PhysicianProcedure> listProcedure = immProList;
            if (listProcedure != null)
            {
                if (listProcedure.Count > 0)
                {
                    for (int i = 0; i < listProcedure.Count; i++)
                        chklstImmunizationHistory.Items.Add(new RadListBoxItem(listProcedure[i].Physician_Procedure_Code + "-" + listProcedure[i].Procedure_Description));
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                DateTime utc = Convert.ToDateTime(strtime);
                ImmunizationHistoryDTO LoadImmunHistoryGrid;
                ImmunizationHistoryManager immunizationMngr = new ImmunizationHistoryManager();
                ImmunizationHistory immunizationHistory;
                ImmunizationMasterHistoryManager immunizationMasterMngr = new ImmunizationMasterHistoryManager();
                ImmunizationMasterHistory immunizationMasterHistory;

                DateTime Birth_Date = utc; //Convert.ToDateTime(hdnLocalTime.Value);
                if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
                    Birth_Date = ClientSession.PatientPaneList[0].Birth_Date;
                else
                    Birth_Date = Convert.ToDateTime(ClientSession.PatientPane.Split('|')[1].Trim());

                if (txtImmunizationProcedure.Text == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Immunization Save", "DisplayErrorMessage('295004'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();", true);
                    return;
                }
                if (txtDose.Text != string.Empty)
                {

                    int f = 0;
                    if (Convert.ToInt16(txtDose.Text) > 20)
                        f = 1;
                    if (cboDosetotal.Text != string.Empty)
                    {
                        if (Convert.ToInt32(txtDose.Text) > Convert.ToInt32(cboDosetotal.Text))
                            f = 1;
                    }
                    if (f == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Immunization Save", "DisplayErrorMessage('295008'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();", true);
                        return;
                    }
                }
                string ComboBoxDate = string.Empty;
                if (cdtAdministeredDate.cboDate.Text == "" && cdtAdministeredDate.cboMonth.Text != "" && cdtAdministeredDate.cboYear.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Immunization Save", "DisplayErrorMessage('295010'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();", true);

                    return;
                }
                string sDate = string.Empty;
                if (cdtAdministeredDate.cboDate.Text != "" && cdtAdministeredDate.cboMonth.Text != "" && cdtAdministeredDate.cboYear.Text != "")
                    sDate = cdtAdministeredDate.cboDate.Text + "-" + cdtAdministeredDate.cboMonth.Text + "-" + cdtAdministeredDate.cboYear.Text;
                else if (cdtAdministeredDate.cboYear.Text != "" && cdtAdministeredDate.cboMonth.Text != "" && cdtAdministeredDate.cboDate.Text == "")
                    sDate = "01-" + cdtAdministeredDate.cboMonth.Text + "-" + cdtAdministeredDate.cboYear.Text;
                else if (cdtAdministeredDate.cboYear.Text != "" && cdtAdministeredDate.cboMonth.Text == "" && cdtAdministeredDate.cboDate.Text == "")
                    sDate = "01-" + "Jan-" + cdtAdministeredDate.cboYear.Text;
                else
                    sDate = "";
                DateTime dt = new DateTime();
                if (sDate.Trim() != string.Empty)
                    dt = UtilityManager.ConvertToLocal(Convert.ToDateTime(sDate));
                if (dt.Year != 1)
                {
                    int h = dt.Date.CompareTo(utc.Date);// (UtilityManager.ConvertToLocal(Convert.ToDateTime(hdnLocalTime.Value)).Date);
                    if (h > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Immunization Save", "DisplayErrorMessage('295011'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();", true);
                        return;
                    }

                    if (Birth_Date.CompareTo(dt) > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Immunization Save", "DisplayErrorMessage('180613'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();", true);
                        return;
                    }
                }


                if (string.Compare(Convert.ToString(Hiddenupdate.Value), "ADD", true) == 0)
                {
                    if (ScreenMode == "Queue")
                    {
                        immunizationHistory = new ImmunizationHistory();
                        AddorUpdateImmunizationHistory(immunizationHistory);
                        immunizationHistory.Created_By = ClientSession.UserName;
                        immunizationHistory.Is_Deleted = "N";
                        immunizationHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                        IList<ImmunizationHistory> lstImmunizationHistory = new List<ImmunizationHistory>();
                        lstImmunizationHistory = (IList<ImmunizationHistory>)Session["ImmunizationHistory"];
                        IList<ImmunizationHistory> SaveImmunizationHistory = new List<ImmunizationHistory>();
                        if (lstImmunizationHistory != null && lstImmunizationHistory.Count > 0)
                        {
                            foreach (ImmunizationHistory item in lstImmunizationHistory)
                            {
                                if (item.Encounter_ID != ClientSession.EncounterId)
                                {
                                    item.Encounter_ID = ClientSession.EncounterId;
                                    item.Id = 0;
                                    item.Version = 0;
                                    if (item.Procedure_Code.Trim() == "90732" || item.Procedure_Code.Trim() == "90670")
                                    {
                                        item.Snomed_Code = "473165003";
                                    }
                                    else
                                    {
                                        item.Snomed_Code = "";
                                    }
                                    SaveImmunizationHistory.Add(item);
                                }
                            }
                        }
                        immunizationHistory.Encounter_ID = ClientSession.EncounterId;
                        SaveImmunizationHistory.Add(immunizationHistory);

                        if (Session["LoadImmunizationMasterList"] != null)
                        {
                            IList<ImmunizationMasterHistory> _loadMasterList = new List<ImmunizationMasterHistory>();
                            _loadMasterList = (IList<ImmunizationMasterHistory>)Session["LoadImmunizationMasterList"];
                            if (_loadMasterList.Count > 0)
                            {
                                foreach (ImmunizationMasterHistory item in _loadMasterList)
                                {
                                    if (ScreenMode == "Queue")
                                    {
                                        ImmunizationHistory objHistory = new ImmunizationHistory();
                                        //bugId:61103 
                                        //AddorUpdateImmunizationHistory(objHistory);

                                        objHistory.Immunization_Description = item.Immunization_Description;
                                        objHistory.CVX_Code = item.CVX_Code;
                                        objHistory.Route_Of_Administration = item.Route_Of_Administration;

                                        objHistory.Is_VIS_Given = item.Is_VIS_Given;
                                        objHistory.Date_On_Vis = item.Date_On_Vis;
                                        objHistory.Vis_Given_Date = item.Vis_Given_Date;
                                        objHistory.Procedure_Code = item.Procedure_Code;
                                        objHistory.Human_ID = ClientSession.HumanId;
                                        objHistory.Physician_ID = ClientSession.PhysicianId;

                                        objHistory.Lot_Number = item.Lot_Number;

                                        objHistory.Administered_Amount = item.Administered_Amount;

                                        objHistory.Administered_Date = item.Administered_Date;

                                        objHistory.Manufacturer = item.Manufacturer;
                                        objHistory.Administered_Unit = item.Administered_Unit;

                                        objHistory.Expiry_Date = item.Expiry_Date;
                                        objHistory.Immunization_Source = item.Immunization_Source;
                                        objHistory.Location = item.Location;
                                        objHistory.Protection_State = item.Protection_State;

                                        objHistory.Dose = item.Dose;

                                        objHistory.Dose_No = item.Dose_No;
                                        objHistory.Notes = item.Notes;

                                        objHistory.Snomed_Code = item.Snomed_Code;
                                        objHistory.Immunization_History_Master_ID = item.Id;
                                        objHistory.Created_By = ClientSession.UserName;
                                        objHistory.Is_Deleted = "N";
                                        objHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                        objHistory.Encounter_ID = ClientSession.EncounterId;
                                        SaveImmunizationHistory.Add(objHistory);
                                    }

                                }
                            }
                            Session["LoadImmunizationMasterList"] = null;
                        }
                        ImmList = (IList<ImmunizationHistory>)Session["ImmunizationHistory"];
                        LoadImmunHistoryGrid = immunizationMngr.InsertIntoImmunizationHistory(ImmList, SaveImmunizationHistory, ClientSession.HumanId, 0, 0, string.Empty, ClientSession.EncounterId, false);
                    }
                    else if (ScreenMode == "Menu")
                    {
                        immunizationMasterHistory = new ImmunizationMasterHistory();
                        AddorUpdateImmunizationMasterHistory(immunizationMasterHistory);
                        immunizationMasterHistory.Created_By = ClientSession.UserName;
                        immunizationMasterHistory.Is_Deleted = "N";
                        immunizationMasterHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                        IList<ImmunizationMasterHistory> objImmunizationMasterHistoryDTO = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
                        IList<ImmunizationMasterHistory> SaveImmunizationMasterHistory = new List<ImmunizationMasterHistory>();
                        SaveImmunizationMasterHistory.Add(immunizationMasterHistory);
                        ImmMasterList = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
                        LoadImmunHistoryGrid = immunizationMasterMngr.InsertIntoImmunizationHistory(ImmMasterList, SaveImmunizationMasterHistory, ClientSession.HumanId, 0, 0, string.Empty, ClientSession.EncounterId, false);
                    }


                    btnSave.Enabled = false;

                }
                else if (string.Compare(Convert.ToString(Hiddenupdate.Value), "UPDATE", true) == 0)
                {
                    ulong ulUpdateDelId = 0;
                    if (Session["UpdateDeleteID"] != null)
                        ulUpdateDelId = (ulong)Session["UpdateDeleteID"];
                    if ((IList<ImmunizationMasterHistory>)Session["LoadImmunizationMasterList"] == null)
                    {
                        if (ScreenMode == "Queue")
                        {
                            IList<ImmunizationHistory> ResultList = new List<ImmunizationHistory>();
                            if (Session["ImmunizationHistory"] != null)
                                ResultList = (IList<ImmunizationHistory>)Session["ImmunizationHistory"];
                            IList<ImmunizationHistory> temp = (from obj in ResultList where obj.Id == ulUpdateDelId select obj).ToList().ToList<ImmunizationHistory>();
                            if (temp.Count > 0)
                            {
                                immunizationHistory = temp[0];
                                AddorUpdateImmunizationHistory(immunizationHistory);
                                immunizationHistory.Modified_By = ClientSession.UserName;
                                immunizationHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                immunizationHistory.Is_Deleted = "N";
                                IList<ImmunizationHistory> objImmunizationHistoryDTO = (IList<ImmunizationHistory>)Session["ImmunizationHistory"];
                                IList<ImmunizationHistory> SaveImmunizationHistory = new List<ImmunizationHistory>();
                                IList<ImmunizationHistory> UpdateImmunizationHistory = new List<ImmunizationHistory>();
                                IList<ulong> lstId = new List<ulong>();
                                foreach (ImmunizationHistory item in objImmunizationHistoryDTO)
                                {
                                    if (item.Procedure_Code.Trim() == "90732" || item.Procedure_Code.Trim() == "90670")
                                    {
                                        item.Snomed_Code = "473165003";
                                    }
                                    else
                                    {
                                        item.Snomed_Code = "";
                                    }

                                    if (item.Encounter_ID != ClientSession.EncounterId)
                                    {
                                        if (ulUpdateDelId != item.Id)
                                        {
                                            lstId.Add(item.Id);
                                            item.Encounter_ID = ClientSession.EncounterId;
                                            item.Id = 0;
                                            item.Version = 0;
                                            SaveImmunizationHistory.Add(item);
                                        }
                                        else
                                        {
                                            lstId.Add(ulUpdateDelId);
                                            immunizationHistory.Id = 0;
                                            immunizationHistory.Encounter_ID = ClientSession.EncounterId;
                                            immunizationHistory.Version = 0;
                                            SaveImmunizationHistory.Add(immunizationHistory);
                                        }
                                    }
                                    else
                                    {
                                        lstId.Add(item.Encounter_ID);
                                    }

                                }

                                if (lstId.Count > 0 && !lstId.Any(a => a == ulUpdateDelId))
                                    UpdateImmunizationHistory.Add(immunizationHistory);

                                ImmList = (IList<ImmunizationHistory>)Session["ImmunizationHistory"];
                                LoadImmunHistoryGrid = immunizationMngr.UpdateImmunizationHistoryDetails(ImmList, SaveImmunizationHistory, UpdateImmunizationHistory, ClientSession.HumanId, 0, 0, string.Empty, ClientSession.EncounterId, false);
                            }

                        }
                        else if (ScreenMode == "Menu")
                        {
                            IList<ImmunizationMasterHistory> ResultList = new List<ImmunizationMasterHistory>();
                            if (Session["ImmunizationMasterHistory"] != null)
                                ResultList = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
                            IList<ImmunizationMasterHistory> temp = (from obj in ResultList where obj.Id == ulUpdateDelId select obj).ToList().ToList<ImmunizationMasterHistory>();
                            if (temp.Count > 0)
                            {
                                immunizationMasterHistory = temp[0];
                                AddorUpdateImmunizationMasterHistory(immunizationMasterHistory);
                                immunizationMasterHistory.Modified_By = ClientSession.UserName;
                                immunizationMasterHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                immunizationMasterHistory.Is_Deleted = "N";
                                IList<ImmunizationMasterHistory> objImmunizationHistoryDTO = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
                                IList<ImmunizationMasterHistory> SaveImmunizationHistory = new List<ImmunizationMasterHistory>();
                                IList<ImmunizationMasterHistory> UpdateImmunizationHistory = new List<ImmunizationMasterHistory>();
                                IList<ulong> lstId = new List<ulong>();
                                foreach (ImmunizationMasterHistory item in objImmunizationHistoryDTO)
                                {
                                    if (item.Procedure_Code.Trim() == "90732" || item.Procedure_Code.Trim() == "90670")
                                        item.Snomed_Code = "473165003";
                                    else
                                        item.Snomed_Code = "";
                                }

                                if (lstId.Count > 0 && !lstId.Any(a => a == ulUpdateDelId))
                                    UpdateImmunizationHistory.Add(immunizationMasterHistory);

                                // ImmMasterList = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
                                UpdateImmunizationHistory = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
                                LoadImmunHistoryGrid = immunizationMasterManager.UpdateImmunizationHistoryDetails(ImmMasterList, SaveImmunizationHistory, UpdateImmunizationHistory, ClientSession.HumanId, 0, 0, string.Empty, 0, true);
                            }
                        }
                    }
                    else if (((IList<ImmunizationMasterHistory>)Session["LoadImmunizationMasterList"]).Count > 0)
                    {
                        if (ScreenMode == "Queue")
                        {
                            IList<ImmunizationMasterHistory> ResultList = new List<ImmunizationMasterHistory>();
                            if (Session["ImmunizationMasterHistory"] != null)
                                ResultList = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
                            IList<ImmunizationMasterHistory> temp = (from obj in ResultList where obj.Id == ulUpdateDelId select obj).ToList().ToList<ImmunizationMasterHistory>();
                            if (temp.Count > 0)
                            {
                                immunizationMasterHistory = temp[0];
                                AddorUpdateImmunizationMasterHistory(immunizationMasterHistory);
                                immunizationMasterHistory.Modified_By = ClientSession.UserName;
                                immunizationMasterHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                immunizationMasterHistory.Is_Deleted = "N";
                                IList<ImmunizationMasterHistory> objImmunizationHistoryDTO = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
                                IList<ImmunizationMasterHistory> SaveImmunizationHistory = new List<ImmunizationMasterHistory>();
                                IList<ImmunizationMasterHistory> UpdateImmunizationHistory = new List<ImmunizationMasterHistory>();
                                IList<ImmunizationHistory> SaveImmunHistory = new List<ImmunizationHistory>();
                                IList<ImmunizationHistory> UpdateImmunHistory = new List<ImmunizationHistory>();
                                IList<ulong> lstId = new List<ulong>();
                                foreach (ImmunizationMasterHistory item in objImmunizationHistoryDTO)
                                {
                                    if (item.Procedure_Code.Trim() == "90732" || item.Procedure_Code.Trim() == "90670")
                                        item.Snomed_Code = "473165003";
                                    else
                                        item.Snomed_Code = "";
                                }

                                if (lstId.Count > 0 && !lstId.Any(a => a == ulUpdateDelId))
                                    UpdateImmunizationHistory.Add(immunizationMasterHistory);
                                //Jira #CAP-187 - update in immunization_history
                                if (Session["ImmunizationHistory"] == null)
                                {
                                    foreach (ImmunizationMasterHistory item in objImmunizationHistoryDTO)
                                    {
                                        
                                            ImmunizationHistory objHistory = new ImmunizationHistory();
                                            //bugId:61103 
                                            //AddorUpdateImmunizationHistory(objHistory);

                                            objHistory.Immunization_Description = item.Immunization_Description;
                                            objHistory.CVX_Code = item.CVX_Code;
                                            objHistory.Route_Of_Administration = item.Route_Of_Administration;

                                            objHistory.Is_VIS_Given = item.Is_VIS_Given;
                                            objHistory.Date_On_Vis = item.Date_On_Vis;
                                            objHistory.Vis_Given_Date = item.Vis_Given_Date;
                                            objHistory.Procedure_Code = item.Procedure_Code;
                                            objHistory.Human_ID = ClientSession.HumanId;
                                            objHistory.Physician_ID = ClientSession.PhysicianId;

                                            objHistory.Lot_Number = item.Lot_Number;

                                            objHistory.Administered_Amount = item.Administered_Amount;

                                            objHistory.Administered_Date = item.Administered_Date;

                                            objHistory.Manufacturer = item.Manufacturer;
                                            objHistory.Administered_Unit = item.Administered_Unit;

                                            objHistory.Expiry_Date = item.Expiry_Date;
                                            objHistory.Immunization_Source = item.Immunization_Source;
                                            objHistory.Location = item.Location;
                                            objHistory.Protection_State = item.Protection_State;

                                            objHistory.Dose = item.Dose;

                                            objHistory.Dose_No = item.Dose_No;
                                            objHistory.Notes = item.Notes;

                                            objHistory.Snomed_Code = item.Snomed_Code;
                                            objHistory.Immunization_History_Master_ID = item.Id;
                                            objHistory.Created_By = ClientSession.UserName;
                                            objHistory.Is_Deleted = "N";
                                            objHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                            objHistory.Encounter_ID = ClientSession.EncounterId;
                                            SaveImmunHistory.Add(objHistory);
                                        

                                    }
                                }
                                else
                                {
                                    IList<ImmunizationHistory> ResultListimmu = (IList<ImmunizationHistory>)Session["ImmunizationHistory"];
                                    IList<ImmunizationHistory> tempimmu = (from obj in ResultListimmu where obj.Immunization_History_Master_ID == ulUpdateDelId select obj).ToList().ToList<ImmunizationHistory>();
                                    ImmunizationHistory immuobj = new ImmunizationHistory();
                                    if (tempimmu.Count > 0)
                                    {
                                        immuobj = tempimmu[0];
                                        AddorUpdateImmunizationHistory(immuobj);
                                        immuobj.Modified_By = ClientSession.UserName;
                                        immuobj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                        immuobj.Is_Deleted = "N";
                                        UpdateImmunHistory.Add(immuobj);
                                    }
                                }

                                //ImmMasterList = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
                                UpdateImmunizationHistory = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
                                //Jira #CAP-187 - update in immunization_history
                                //LoadImmunHistoryGrid = immunizationMasterManager.UpdateImmunizationHistoryDetails(ImmMasterList, SaveImmunizationHistory, UpdateImmunizationHistory, ClientSession.HumanId, 0, 0, string.Empty, 0, true);
                                LoadImmunHistoryGrid = immunizationMngr.UpdateImmunizationHistoryAndMasterDetails(UpdateImmunizationHistory, SaveImmunHistory, UpdateImmunHistory, ClientSession.HumanId, 0, 0, string.Empty, ClientSession.EncounterId, false);
                            }

                        }

                    }
                    btnSave.Text = "Add";
                    btnClearAll.Text = "Clear All";
                    btnSave.Enabled = false;
                    System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanAdd");
                    if (text1 != null)
                        text1.InnerText = "A";
                    System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanAdditionalword");
                    if (text2 != null)
                        text2.InnerText = "dd";
                    System.Web.UI.HtmlControls.HtmlGenericControl textClear = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                    if (textClear != null)
                        textClear.InnerText = "C";
                    System.Web.UI.HtmlControls.HtmlGenericControl textClearAdd = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                    if (textClearAdd != null)
                        textClearAdd.InnerText = "lear All";
                    btnSave.AccessKey = "A";
                    Hiddenupdate.Value = "ADD";

                }
                LoadGridPageNavigator(false);
                Clear();
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "SavedSuccessfully();EnablePFSH(" + ClientSession.EncounterId + "); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ClientSession.bPFSHVerified = false;
                UIManager.IsPFSHVerified = true;
            }
            catch (Exception ExSave) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Immunization Load", "alert('" + ExSave.Message.Replace("'", " ") + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true); }
        }
        protected void InvisibleClearAllButton_Click(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanAdd");
                if (text1 != null)
                    text1.InnerText = "A";
                System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanAdditionalword");
                if (text2 != null)
                    text2.InnerText = "dd";
                System.Web.UI.HtmlControls.HtmlGenericControl textClear = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                if (textClear != null)
                    textClear.InnerText = "C";
                System.Web.UI.HtmlControls.HtmlGenericControl textClearAdd = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                if (textClearAdd != null)
                    textClearAdd.InnerText = "lear All";
                btnSave.AccessKey = "A";
                //Hiddenupdate.Value = "ADD";
                btnSave.Text = "Add";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Immunization Clear All", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            catch (Exception ExSave) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Immunization Load", "alert('" + ExSave.Message.Replace("'", " ") + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true); }
        }
        public void Clear()
        {
            txtImmunizationProcedure.Text = string.Empty;
            txtDose.Text = string.Empty;
            cboDosetotal.SelectedIndex = 0;
            cboLocation.SelectedIndex = 0;
            cboProtectionState.SelectedIndex = 0;
            txtLotNumber.Text = string.Empty;
            txtAdminAmt.Text = string.Empty;
            cboAdminUnit.SelectedIndex = 0;
            cboManufacturer.SelectedIndex = 0;
            cboRouteOfAdminstration.SelectedIndex = 0;
            cdtAdministeredDate.AssignDate(null, null, null);
            cdtAdministeredDate.cboYear.Text = "";
            cdtAdministeredDate.cboMonth.Text = "";
            cdtAdministeredDate.cboDate.Text = "";
            cdtAdministeredDate.cboDate.SelectedIndex = 0;
            DLC.txtDLC.Text = string.Empty;
            dpDateOnVIS.SelectedDate = null;
            dpVisgiven.SelectedDate = null;
            cboImmunizationSource.SelectedIndex = 0;
            chkVisgiven.Checked = false;
            txtCVXcode.Text = string.Empty;
            dpExpiryDate.SelectedDate = null;
            chklstImmunizationHistory.ClearSelection();
            btnSave.Enabled = false;

        }


        public void AddorUpdateImmunizationHistory(ImmunizationHistory immunizationHistory)
        {
            if (txtImmunizationProcedure.Text != string.Empty)
            {
                string[] split = txtImmunizationProcedure.Text.Split('-');
                if (split.Length > 0)
                {
                    immunizationHistory.Procedure_Code = split[0];
                    for (int i = 1; i < split.Length; i++)
                    {
                        if (i == 1)
                            immunizationHistory.Immunization_Description = split[i];
                        else
                            immunizationHistory.Immunization_Description += "-" + split[i];
                    }
                }

            }
            immunizationHistory.CVX_Code = txtCVXcode.Text;
            immunizationHistory.Route_Of_Administration = cboRouteOfAdminstration.Text;
            if (chkVisgiven.Checked)
            {
                immunizationHistory.Is_VIS_Given = "Y";
                immunizationHistory.Date_On_Vis = dpDateOnVIS.SelectedDate.Value;
                immunizationHistory.Vis_Given_Date = dpVisgiven.SelectedDate.Value;
                dpDateOnVIS.Enabled = true;
                dpVisgiven.Enabled = true;
            }
            else
            {
                immunizationHistory.Is_VIS_Given = "N";
            }
            immunizationHistory.Human_ID = ClientSession.HumanId;
            immunizationHistory.Physician_ID = ClientSession.PhysicianId;

            immunizationHistory.Lot_Number = txtLotNumber.Text;
            if (txtAdminAmt.Text != string.Empty)
                immunizationHistory.Administered_Amount = Convert.ToDecimal(txtAdminAmt.Text);
            else
                immunizationHistory.Administered_Amount = Convert.ToDecimal("0");
            if (cdtAdministeredDate.cboDate.Text != "" && cdtAdministeredDate.cboMonth.Text != "" && cdtAdministeredDate.cboYear.Text != "")
            {
                DateTime dtpAdminDate = DateTime.Now;
                string strAdminDate = cdtAdministeredDate.cboDate.Text + "-" + cdtAdministeredDate.cboMonth.Text + "-" + cdtAdministeredDate.cboYear.Text;
                dtpAdminDate = Convert.ToDateTime(strAdminDate);

                immunizationHistory.Administered_Date = dtpAdminDate.ToString("dd-MMM-yyyy");
            }
            else if (cdtAdministeredDate.cboYear.Text != "" && cdtAdministeredDate.cboMonth.Text != "" && cdtAdministeredDate.cboDate.Text == "")
                immunizationHistory.Administered_Date = cdtAdministeredDate.cboMonth.Text + "-" + cdtAdministeredDate.cboYear.Text;
            else if (cdtAdministeredDate.cboYear.Text != "" && cdtAdministeredDate.cboMonth.Text == "" && cdtAdministeredDate.cboDate.Text == "")
                immunizationHistory.Administered_Date = cdtAdministeredDate.cboYear.Text;
            else
                immunizationHistory.Administered_Date = "";

            immunizationHistory.Manufacturer = cboManufacturer.Text;
            immunizationHistory.Administered_Unit = cboAdminUnit.Text;
            if (dpExpiryDate.SelectedDate != null)
                immunizationHistory.Expiry_Date = dpExpiryDate.SelectedDate.Value;
            immunizationHistory.Immunization_Source = cboImmunizationSource.Text;
            immunizationHistory.Location = cboLocation.Text;
            immunizationHistory.Protection_State = cboProtectionState.Text;
            if (txtDose.Text != string.Empty)
                immunizationHistory.Dose = Convert.ToUInt32(txtDose.Text);
            if (cboDosetotal.Text != string.Empty)
                immunizationHistory.Dose_No = Convert.ToUInt32(cboDosetotal.Text);
            immunizationHistory.Notes = DLC.txtDLC.Text;
            if (immunizationHistory.Procedure_Code.Trim() == "90732" || immunizationHistory.Procedure_Code.Trim() == "90670")
            {
                immunizationHistory.Snomed_Code = "473165003";
            }
            else
            {
                immunizationHistory.Snomed_Code = "";
            }
        }

        public void AddorUpdateImmunizationMasterHistory(ImmunizationMasterHistory immunizationMasterHistory)
        {
            if (txtImmunizationProcedure.Text != string.Empty)
            {
                string[] split = txtImmunizationProcedure.Text.Split('-');
                if (split.Length > 0)
                {
                    immunizationMasterHistory.Procedure_Code = split[0];
                    for (int i = 1; i < split.Length; i++)
                    {
                        if (i == 1)
                            immunizationMasterHistory.Immunization_Description = split[i];
                        else
                            immunizationMasterHistory.Immunization_Description += "-" + split[i];
                    }
                }

            }
            immunizationMasterHistory.CVX_Code = txtCVXcode.Text;
            immunizationMasterHistory.Route_Of_Administration = cboRouteOfAdminstration.Text;
            if (chkVisgiven.Checked)
            {
                immunizationMasterHistory.Is_VIS_Given = "Y";
                immunizationMasterHistory.Date_On_Vis = dpDateOnVIS.SelectedDate.Value;
                immunizationMasterHistory.Vis_Given_Date = dpVisgiven.SelectedDate.Value;
                dpDateOnVIS.Enabled = true;
                dpVisgiven.Enabled = true;
            }
            else
            {
                immunizationMasterHistory.Is_VIS_Given = "N";
            }
            immunizationMasterHistory.Human_ID = ClientSession.HumanId;
            immunizationMasterHistory.Physician_ID = ClientSession.PhysicianId;

            immunizationMasterHistory.Lot_Number = txtLotNumber.Text;
            if (txtAdminAmt.Text != string.Empty)
                immunizationMasterHistory.Administered_Amount = Convert.ToDecimal(txtAdminAmt.Text);
            else
                immunizationMasterHistory.Administered_Amount = Convert.ToDecimal("0");
            if (cdtAdministeredDate.cboDate.Text != "" && cdtAdministeredDate.cboMonth.Text != "" && cdtAdministeredDate.cboYear.Text != "")
            {
                DateTime dtpAdminDate = DateTime.Now;
                string strAdminDate = cdtAdministeredDate.cboDate.Text + "-" + cdtAdministeredDate.cboMonth.Text + "-" + cdtAdministeredDate.cboYear.Text;
                dtpAdminDate = Convert.ToDateTime(strAdminDate);

                immunizationMasterHistory.Administered_Date = dtpAdminDate.ToString("dd-MMM-yyyy");
            }
            else if (cdtAdministeredDate.cboYear.Text != "" && cdtAdministeredDate.cboMonth.Text != "" && cdtAdministeredDate.cboDate.Text == "")
                immunizationMasterHistory.Administered_Date = cdtAdministeredDate.cboMonth.Text + "-" + cdtAdministeredDate.cboYear.Text;
            else if (cdtAdministeredDate.cboYear.Text != "" && cdtAdministeredDate.cboMonth.Text == "" && cdtAdministeredDate.cboDate.Text == "")
                immunizationMasterHistory.Administered_Date = cdtAdministeredDate.cboYear.Text;
            else
                immunizationMasterHistory.Administered_Date = "";

            immunizationMasterHistory.Manufacturer = cboManufacturer.Text;
            immunizationMasterHistory.Administered_Unit = cboAdminUnit.Text;
            if (dpExpiryDate.SelectedDate != null)
                immunizationMasterHistory.Expiry_Date = dpExpiryDate.SelectedDate.Value;
            immunizationMasterHistory.Immunization_Source = cboImmunizationSource.Text;
            immunizationMasterHistory.Location = cboLocation.Text;
            immunizationMasterHistory.Protection_State = cboProtectionState.Text;
            if (txtDose.Text != string.Empty)
                immunizationMasterHistory.Dose = Convert.ToUInt32(txtDose.Text);
            if (cboDosetotal.Text != string.Empty)
                immunizationMasterHistory.Dose_No = Convert.ToUInt32(cboDosetotal.Text);
            immunizationMasterHistory.Notes = DLC.txtDLC.Text;
            if (immunizationMasterHistory.Procedure_Code.Trim() == "90732" || immunizationMasterHistory.Procedure_Code.Trim() == "90670")
            {
                immunizationMasterHistory.Snomed_Code = "473165003";
            }
            else
            {
                immunizationMasterHistory.Snomed_Code = "";
            }
        }


        protected void grdImmunization_ItemCommand(object sender, GridCommandEventArgs e)
        {
            DateTime utc = DateTime.MinValue;
            string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
            if (strtime != string.Empty)
                utc = Convert.ToDateTime(strtime);
            if (e.CommandName == "EditRows")
            {
                if (grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[10].Text!="0")
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Autosave", "alert('This was created from Order.Cannot Edit/Delete this Procedure.'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                    return;
                }
                Hiddenupdate.Value = "UPDATE";
                btnSave.Text = "Update";
                btnClearAll.Text = "Cancel";
                btnSave.Enabled = true;
                if (ScreenMode == "Menu")
                {
                    IList<ImmunizationMasterHistory> ResultList = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
                    if (grdImmunization.MasterTableView.Items.Count > 0)
                    {
                        ulong ID = 0;
                        if (System.Text.RegularExpressions.Regex.IsMatch(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[9].Text, "^[0-9]*$") == true)
                        {
                            ID = Convert.ToUInt64(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[9].Text);
                        }
                        ulong ulUpdateDelId = ID;
                        Session["UpdateDeleteID"] = ulUpdateDelId;
                        IList<ImmunizationMasterHistory> ImmuMasterHistoryList = (from I in ResultList where I.Id == Convert.ToUInt64(ID) select I).ToList<ImmunizationMasterHistory>();
                        if (ImmuMasterHistoryList != null && ImmuMasterHistoryList.Count > 0)
                            ImmunizationMasterUpdate(ImmuMasterHistoryList[0]);
                    }
                }
                else if (ScreenMode == "Queue")
                {
                    if ((IList<ImmunizationMasterHistory>)Session["LoadImmunizationMasterList"] == null)
                    {
                        IList<ImmunizationHistory> ResultList = (IList<ImmunizationHistory>)Session["ImmunizationHistory"];
                        if (grdImmunization.MasterTableView.Items.Count > 0)
                        {
                            ulong ID = 0;
                            if (System.Text.RegularExpressions.Regex.IsMatch(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[9].Text, "^[0-9]*$") == true)
                            {
                                ID = Convert.ToUInt64(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[9].Text);
                            }
                            ulong ulUpdateDelId = ID;
                            Session["UpdateDeleteID"] = ulUpdateDelId;
                            IList<ImmunizationHistory> ImmuHistoryList = (from I in ResultList where I.Id == Convert.ToUInt64(ID) select I).ToList<ImmunizationHistory>();
                            if (ImmuHistoryList != null && ImmuHistoryList.Count > 0)
                                ImmunizationUpdate(ImmuHistoryList[0]);
                        }
                    }
                    else if ((IList<ImmunizationMasterHistory>)Session["LoadImmunizationMasterList"] != null && ((IList<ImmunizationMasterHistory>)Session["LoadImmunizationMasterList"]).Count > 0)
                    {
                        IList<ImmunizationMasterHistory> ResultList = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
                        if (grdImmunization.MasterTableView.Items.Count > 0)
                        {
                            ulong ID = 0;
                            if (System.Text.RegularExpressions.Regex.IsMatch(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[9].Text, "^[0-9]*$") == true)
                            {
                                ID = Convert.ToUInt64(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[9].Text);
                            }
                            ulong ulUpdateDelId = ID;
                            Session["UpdateDeleteID"] = ulUpdateDelId;
                            IList<ImmunizationMasterHistory> ImmuMasterHistoryList = (from I in ResultList where I.Id == Convert.ToUInt64(ID) select I).ToList<ImmunizationMasterHistory>();
                            if (ImmuMasterHistoryList != null && ImmuMasterHistoryList.Count > 0)
                                ImmunizationMasterUpdate(ImmuMasterHistoryList[0]);
                        }
                    }
                }

                System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanAdd");
                if (text1 != null)
                    text1.InnerText = "U";
                System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanAdditionalword");
                if (text2 != null)
                    text2.InnerText = "pdate";
                btnSave.AccessKey = "U";
                System.Web.UI.HtmlControls.HtmlGenericControl textClear = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                if (textClear != null)
                    textClear.InnerText = "C";
                System.Web.UI.HtmlControls.HtmlGenericControl textClearAdd = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                if (textClearAdd != null)
                    textClearAdd.InnerText = "ancel";
                btnSave.Enabled = true;
                btnClearAll.Text = "Cancel";
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Autosave", "GridEditAutoSaveEnable(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

            }
            if (e.CommandName == "DeleteRows")
            {
                if (grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[10].Text != "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Autosave", "alert('This was created from Order.Cannot Edit/Delete this Procedure.'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                    return;
                }
                if (ScreenMode == "Queue")
                {
                    if ((IList<ImmunizationMasterHistory>)Session["LoadImmunizationMasterList"] != null && ((IList<ImmunizationMasterHistory>)Session["LoadImmunizationMasterList"]).Count > 0)
                    {
                        //Jira #CAP-610
                        //if (grdImmunization.MasterTableView.Items.Count > 0 && System.Text.RegularExpressions.Regex.IsMatch(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[8].Text, "^[0-9]*$") == true)
                        //{
                        //    DeleteFromMaster(Convert.ToUInt64(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[8].Text));
                        //}
                        if (grdImmunization.MasterTableView.Items.Count > 0 && System.Text.RegularExpressions.Regex.IsMatch(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[9].Text, "^[0-9]*$") == true)
                        {
                            DeleteFromMaster(Convert.ToUInt64(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[9].Text));
                        }
                    }
                    else
                    {
                        ImmunizationHistory immunizationHistory = new ImmunizationHistory();
                        ImmunizationHistoryManager immunizationMngr = new ImmunizationHistoryManager();
                        IList<ImmunizationHistory> DeleteList = (IList<ImmunizationHistory>)Session["ImmunizationHistory"];
                        ulong ImmunizationID = 0;
                        if (grdImmunization.MasterTableView.Items.Count > 0 && System.Text.RegularExpressions.Regex.IsMatch(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[9].Text, "^[0-9]*$") == true)
                        {
                            ImmunizationID = Convert.ToUInt64(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[9].Text);
                        }
                        immunizationHistory = (from I in DeleteList where I.Id == ImmunizationID select I).ToList<ImmunizationHistory>()[0];
                        immunizationHistory.Modified_By = ClientSession.UserName;
                        immunizationHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        immunizationHistory.Is_Deleted = "Y";
                        IList<ImmunizationHistory> DelList = new List<ImmunizationHistory>();
                        DelList.Add(immunizationHistory);
                        IList<ImmunizationHistory> SaveList = new List<ImmunizationHistory>();
                        IList<ImmunizationHistory> AddImmunization = new List<ImmunizationHistory>();
                        IList<ImmunizationHistory> DelImmunization = new List<ImmunizationHistory>();

                        if (DelList[0].Encounter_ID == ClientSession.EncounterId)
                            DelImmunization.Add(DelList[0]);
                        ImmList = (IList<ImmunizationHistory>)Session["ImmunizationHistory"];
                        if (DelImmunization.Count > 0)
                        {
                            ImmunizationHistoryDTO LoadImmunHistoryGrid = immunizationMngr.DeleteImmunizationHistoryDetails(ImmList, AddImmunization, DelImmunization, String.Empty);
                            LoadGridPageNavigator(true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ImmunHistory", "DisplayErrorMessage('380053'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        }
                    }
                }
                else if (ScreenMode == "Menu")
                {
                    if (grdImmunization.MasterTableView.Items.Count > 0 && System.Text.RegularExpressions.Regex.IsMatch(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[9].Text, "^[0-9]*$") == true)
                    {
                        DeleteFromMaster(Convert.ToUInt64(grdImmunization.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[9].Text));
                    }
                }

                Clear();
                btnSave.Text = "Add";
                btnClearAll.Text = "Clear All";
                System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanAdd");
                if (text1 != null)
                    text1.InnerText = "A";
                System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanAdditionalword");
                if (text2 != null)
                    text2.InnerText = "dd";
                btnSave.AccessKey = "A";
                System.Web.UI.HtmlControls.HtmlGenericControl textClear = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                if (textClear != null)
                    textClear.InnerText = "C";
                System.Web.UI.HtmlControls.HtmlGenericControl textClearAdd = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                if (textClearAdd != null)
                    textClearAdd.InnerText = "lear All";
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}RefreshNotification('ImmunizationHistory');", true);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "notification", "DeleteSuccessfully();RefreshNotification('ImmunizationHistory');", true);



            }
        }

        public void ImmunizationUpdate(ImmunizationHistory objImmunizationHistory)
        {
            chklstImmunizationHistory.ClearSelection();
            txtImmunizationProcedure.Text = objImmunizationHistory.Procedure_Code + "-" + objImmunizationHistory.Immunization_Description;
            int result = chklstImmunizationHistory.FindItemIndexByValue(objImmunizationHistory.Procedure_Code + "-" + objImmunizationHistory.Immunization_Description);
            if (result != -1)
            {
                chklstImmunizationHistory.Items[result].Selected = true;
            }
            if (objImmunizationHistory.Location != string.Empty)
            {
                for (int k = 0; k < cboLocation.Items.Count(); k++)
                {
                    if (cboLocation.Items[k].Text == objImmunizationHistory.Location)
                        cboLocation.SelectedIndex = k;
                }
            }
            else { cboLocation.ClearSelection(); }


            if (objImmunizationHistory.Is_VIS_Given == "Y")
            {
                chkVisgiven.Checked = true;
                if (objImmunizationHistory.Date_On_Vis != DateTime.MinValue)
                    dpDateOnVIS.SelectedDate = objImmunizationHistory.Date_On_Vis;
                if (objImmunizationHistory.Vis_Given_Date != DateTime.MinValue)
                    dpVisgiven.SelectedDate = objImmunizationHistory.Vis_Given_Date;
                dpDateOnVIS.Enabled = true;
                dpVisgiven.Enabled = true;
            }
            else
            {
                chkVisgiven.Checked = false;
                dpDateOnVIS.Enabled = false;
                dpVisgiven.Enabled = false;
                dpVisgiven.Clear();
                dpDateOnVIS.Clear();
            }

            if (objImmunizationHistory.Expiry_Date != DateTime.MinValue)
                dpExpiryDate.SelectedDate = objImmunizationHistory.Expiry_Date;
            else
                dpExpiryDate.SelectedDate = null;
            txtLotNumber.Text = objImmunizationHistory.Lot_Number;
            txtAdminAmt.Text = objImmunizationHistory.Administered_Amount.ToString();
            if (objImmunizationHistory.Route_Of_Administration != string.Empty)
            {
                for (int k = 0; k < cboRouteOfAdminstration.Items.Count(); k++)
                {
                    if (cboRouteOfAdminstration.Items[k].Text == objImmunizationHistory.Route_Of_Administration)
                        cboRouteOfAdminstration.SelectedIndex = k;
                }
            }
            else { cboRouteOfAdminstration.ClearSelection(); }
            string[] Date = objImmunizationHistory.Administered_Date.Split('-');


            if (Date.Length == 3)
            {
                cdtAdministeredDate.AssignDate(Date[2], Date[1], Date[0]);
                if (cdtAdministeredDate.cboMonth.Items.FindItemByText(Date[1]) != null)
                {
                    cdtAdministeredDate.cboMonth.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText(Date[1]).Index;
                }
                cdtAdministeredDate.cboYear.SelectedIndex = cdtAdministeredDate.cboYear.Items.FindItemByText(Date[2]).Index;
                if (Date[0].StartsWith("0"))
                {
                    Date[0] = Date[0].Substring(1, 1);
                }
                cdtAdministeredDate.cboDate.SelectedIndex = cdtAdministeredDate.cboDate.Items.FindItemByText(Date[0]).Index;
            }
            else if (Date.Length == 2)
            {

                cdtAdministeredDate.AssignDate(Date[1], Date[0], null);
                cdtAdministeredDate.cboYear.SelectedIndex = cdtAdministeredDate.cboYear.Items.FindItemByText(Date[1]).Index;
                if (cdtAdministeredDate.cboMonth.Items.FindItemByText(Date[0]) != null)
                {
                    cdtAdministeredDate.cboMonth.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText(Date[0]).Index;
                }
                cdtAdministeredDate.cboDate.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText("").Index;
            }
            else if (Date.Length == 1 && Date[0] != "")
            {
                cdtAdministeredDate.AssignDate(Date[0], null, null);
                cdtAdministeredDate.cboYear.SelectedIndex = cdtAdministeredDate.cboYear.Items.FindItemByText(Date[0]).Index;
                cdtAdministeredDate.cboMonth.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText("").Index;
                cdtAdministeredDate.cboDate.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText("").Index;
            }
            else if (Date != null && Date[0] != null &&
                Date[0] == "")
            {
                cdtAdministeredDate.AssignDate(null, null, null);
                cdtAdministeredDate.cboMonth.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText("").Index;
                cdtAdministeredDate.cboYear.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText("").Index;
                cdtAdministeredDate.cboDate.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText("").Index;

            }
            DLC.txtDLC.Text = objImmunizationHistory.Notes;

            if (objImmunizationHistory.Manufacturer != string.Empty)
            {
                for (int k = 0; k < cboManufacturer.Items.Count(); k++)
                {
                    if (cboManufacturer.Items[k].Text == objImmunizationHistory.Manufacturer)
                        cboManufacturer.SelectedIndex = k;
                }
            }
            else
            {
                if (cboManufacturer.Items.Count > 0)
                    cboManufacturer.SelectedIndex = 0;
            }
            if (objImmunizationHistory.Administered_Unit != string.Empty)
            {
                for (int k = 0; k < cboAdminUnit.Items.Count(); k++)
                {
                    if (cboAdminUnit.Items[k].Text == objImmunizationHistory.Administered_Unit)
                        cboAdminUnit.SelectedIndex = k;
                }
            }
            else
            {
                if (cboAdminUnit.Items.Count > 0)
                    cboAdminUnit.SelectedIndex = 0;
            }

            if (objImmunizationHistory.Immunization_Source != string.Empty)
            {
                for (int k = 0; k < cboImmunizationSource.Items.Count(); k++)
                {
                    if (cboImmunizationSource.Items[k].Text == objImmunizationHistory.Immunization_Source)
                        cboImmunizationSource.SelectedIndex = k;
                }
            }
            else { cboImmunizationSource.ClearSelection(); }
            if (objImmunizationHistory.Protection_State != string.Empty)
            {
                for (int k = 0; k < cboProtectionState.Items.Count(); k++)
                {
                    if (cboProtectionState.Items[k].Text == objImmunizationHistory.Protection_State)
                        cboProtectionState.SelectedIndex = k;
                }
            }
            else
            {
                if (cboProtectionState.Items.Count > 0)
                    cboProtectionState.SelectedIndex = 0;
            }


            txtCVXcode.Text = objImmunizationHistory.CVX_Code;
            if (objImmunizationHistory.Dose != 0)
                txtDose.Text = Convert.ToString(objImmunizationHistory.Dose);
            else { txtDose.Text = string.Empty; }
            if (objImmunizationHistory.Dose_No != 0)
            {
                for (int k = 0; k < cboDosetotal.Items.Count(); k++)
                {
                    if (cboDosetotal.Items[k].Text == objImmunizationHistory.Dose_No.ToString())
                        cboDosetotal.SelectedIndex = k;
                }
            }
            else { cboDosetotal.ClearSelection(); }

        }

        public void ImmunizationMasterUpdate(ImmunizationMasterHistory objImmunizationMasterHistory)
        {
            chklstImmunizationHistory.ClearSelection();
            txtImmunizationProcedure.Text = objImmunizationMasterHistory.Procedure_Code + "-" + objImmunizationMasterHistory.Immunization_Description;
            int result = chklstImmunizationHistory.FindItemIndexByValue(objImmunizationMasterHistory.Procedure_Code + "-" + objImmunizationMasterHistory.Immunization_Description);
            if (result != -1)
            {
                chklstImmunizationHistory.Items[result].Selected = true;
            }
            if (objImmunizationMasterHistory.Location != string.Empty)
            {
                for (int k = 0; k < cboLocation.Items.Count(); k++)
                {
                    if (cboLocation.Items[k].Text == objImmunizationMasterHistory.Location)
                        cboLocation.SelectedIndex = k;
                }
            }
            else { cboLocation.ClearSelection(); }


            if (objImmunizationMasterHistory.Is_VIS_Given == "Y")
            {
                chkVisgiven.Checked = true;
                if (objImmunizationMasterHistory.Date_On_Vis != DateTime.MinValue)
                    dpDateOnVIS.SelectedDate = objImmunizationMasterHistory.Date_On_Vis;
                if (objImmunizationMasterHistory.Vis_Given_Date != DateTime.MinValue)
                    dpVisgiven.SelectedDate = objImmunizationMasterHistory.Vis_Given_Date;
                dpDateOnVIS.Enabled = true;
                dpVisgiven.Enabled = true;
            }
            else
            {
                chkVisgiven.Checked = false;
                dpDateOnVIS.Enabled = false;
                dpVisgiven.Enabled = false;
                dpVisgiven.Clear();
                dpDateOnVIS.Clear();
            }

            if (objImmunizationMasterHistory.Expiry_Date != DateTime.MinValue)
                dpExpiryDate.SelectedDate = objImmunizationMasterHistory.Expiry_Date;
            else
                dpExpiryDate.SelectedDate = null;
            txtLotNumber.Text = objImmunizationMasterHistory.Lot_Number;
            txtAdminAmt.Text = objImmunizationMasterHistory.Administered_Amount.ToString();
            if (objImmunizationMasterHistory.Route_Of_Administration != string.Empty)
            {
                for (int k = 0; k < cboRouteOfAdminstration.Items.Count(); k++)
                {
                    if (cboRouteOfAdminstration.Items[k].Text == objImmunizationMasterHistory.Route_Of_Administration)
                        cboRouteOfAdminstration.SelectedIndex = k;
                }
            }
            else { cboRouteOfAdminstration.ClearSelection(); }
            string[] Date = objImmunizationMasterHistory.Administered_Date.Split('-');


            if (Date.Length == 3)
            {
                cdtAdministeredDate.AssignDate(Date[2], Date[1], Date[0]);
                cdtAdministeredDate.cboMonth.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText(Date[1]).Index;
                cdtAdministeredDate.cboYear.SelectedIndex = cdtAdministeredDate.cboYear.Items.FindItemByText(Date[2]).Index;
                if (Date[0].StartsWith("0"))
                {
                    Date[0] = Date[0].Substring(1, 1);
                }
                cdtAdministeredDate.cboDate.SelectedIndex = cdtAdministeredDate.cboDate.Items.FindItemByText(Date[0]).Index;
            }
            else if (Date.Length == 2)
            {

                cdtAdministeredDate.AssignDate(Date[1], Date[0], null);
                cdtAdministeredDate.cboYear.SelectedIndex = cdtAdministeredDate.cboYear.Items.FindItemByText(Date[1]).Index;
                cdtAdministeredDate.cboMonth.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText(Date[0]).Index;
                cdtAdministeredDate.cboDate.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText("").Index;
            }
            else if (Date.Length == 1 && Date[0] != "")
            {

                cdtAdministeredDate.AssignDate(Date[0], null, null);
                cdtAdministeredDate.cboYear.SelectedIndex = cdtAdministeredDate.cboYear.Items.FindItemByText(Date[0]).Index;
                cdtAdministeredDate.cboMonth.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText("").Index;
                cdtAdministeredDate.cboDate.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText("").Index;
            }
            else if (Date != null && Date[0] != null &&
                Date[0] == "")
            {
                cdtAdministeredDate.AssignDate(null, null, null);
                cdtAdministeredDate.cboMonth.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText("").Index;
                cdtAdministeredDate.cboYear.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText("").Index;
                cdtAdministeredDate.cboDate.SelectedIndex = cdtAdministeredDate.cboMonth.Items.FindItemByText("").Index;

            }
            DLC.txtDLC.Text = objImmunizationMasterHistory.Notes;

            if (objImmunizationMasterHistory.Manufacturer != string.Empty)
            {
                for (int k = 0; k < cboManufacturer.Items.Count(); k++)
                {
                    if (cboManufacturer.Items[k].Text == objImmunizationMasterHistory.Manufacturer)
                        cboManufacturer.SelectedIndex = k;
                }
            }
            else
            {
                if (cboManufacturer.Items.Count > 0)
                    cboManufacturer.SelectedIndex = 0;
            }
            if (objImmunizationMasterHistory.Administered_Unit != string.Empty)
            {
                for (int k = 0; k < cboAdminUnit.Items.Count(); k++)
                {
                    if (cboAdminUnit.Items[k].Text == objImmunizationMasterHistory.Administered_Unit)
                        cboAdminUnit.SelectedIndex = k;
                }
            }
            else
            {
                if (cboAdminUnit.Items.Count > 0)
                    cboAdminUnit.SelectedIndex = 0;
            }

            if (objImmunizationMasterHistory.Immunization_Source != string.Empty)
            {
                for (int k = 0; k < cboImmunizationSource.Items.Count(); k++)
                {
                    if (cboImmunizationSource.Items[k].Text == objImmunizationMasterHistory.Immunization_Source)
                        cboImmunizationSource.SelectedIndex = k;
                }
            }
            else { cboImmunizationSource.ClearSelection(); }
            if (objImmunizationMasterHistory.Protection_State != string.Empty)
            {
                for (int k = 0; k < cboProtectionState.Items.Count(); k++)
                {
                    if (cboProtectionState.Items[k].Text == objImmunizationMasterHistory.Protection_State)
                        cboProtectionState.SelectedIndex = k;
                }
            }
            else
            {
                if (cboProtectionState.Items.Count > 0)
                    cboProtectionState.SelectedIndex = 0;
            }


            txtCVXcode.Text = objImmunizationMasterHistory.CVX_Code;
            if (objImmunizationMasterHistory.Dose != 0)
                txtDose.Text = Convert.ToString(objImmunizationMasterHistory.Dose);
            else { txtDose.Text = string.Empty; }
            if (objImmunizationMasterHistory.Dose_No != 0)
            {
                for (int k = 0; k < cboDosetotal.Items.Count(); k++)
                {
                    if (cboDosetotal.Items[k].Text == objImmunizationMasterHistory.Dose_No.ToString())
                        cboDosetotal.SelectedIndex = k;
                }
            }
            else { cboDosetotal.ClearSelection(); }

        }

        public void DeleteFromMaster(ulong DeleteMasterID)
        {
            ImmunizationMasterHistory immunizationMasterHistory = new ImmunizationMasterHistory();
            ImmunizationMasterHistoryManager immunizationMasterMngr = new ImmunizationMasterHistoryManager();
            IList<ImmunizationMasterHistory> DeleteList = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
            ulong ImmunizationID = 0;
            ImmunizationID = DeleteMasterID;
            immunizationMasterHistory = (from I in DeleteList where I.Id == ImmunizationID select I).ToList<ImmunizationMasterHistory>()[0];
            immunizationMasterHistory.Modified_By = ClientSession.UserName;
            immunizationMasterHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            immunizationMasterHistory.Is_Deleted = "Y";
            IList<ImmunizationMasterHistory> DelList = new List<ImmunizationMasterHistory>();
            DelList.Add(immunizationMasterHistory);
            IList<ImmunizationMasterHistory> SaveList = new List<ImmunizationMasterHistory>();
            IList<ImmunizationMasterHistory> AddImmunization = new List<ImmunizationMasterHistory>();
            IList<ImmunizationMasterHistory> DelImmunization = new List<ImmunizationMasterHistory>();

            DelImmunization.Add(DelList[0]);
            ImmMasterList = (IList<ImmunizationMasterHistory>)Session["ImmunizationMasterHistory"];
            if (DelImmunization.Count > 0)
            {
                ImmunizationHistoryDTO LoadImmunHistoryGrid = immunizationMasterMngr.DeleteImmunizationHistoryDetails(ImmMasterList, AddImmunization, DelImmunization, String.Empty);
                LoadGridPageNavigator(true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ImmunHistory", "DisplayErrorMessage('380053'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
        }

        protected void chkVisgiven_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkVisgiven.Checked == true)
                {
                    dpVisgiven.SelectedDate = DateTime.Now;
                    dpVisgiven.Enabled = true;
                    dpDateOnVIS.SelectedDate = DateTime.Now;
                    dpDateOnVIS.Enabled = true;
                }
                else
                {
                    dpVisgiven.SelectedDate = null;
                    dpVisgiven.Enabled = false;
                    dpDateOnVIS.SelectedDate = null;
                    dpDateOnVIS.Enabled = false;
                }

                btnSave.Enabled = true;
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            catch (Exception ExVisGiven) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Immunization Load", "alert('" + ExVisGiven.Message.Replace("'", " ") + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true); }
        }


        public void LoadGridPageNavigator(bool Is_Delete)
        {
            ImmunizationHistoryDTO ResultList = new ImmunizationHistoryDTO();
            string FileName = String.Empty;// "Human" + "_" + ClientSession.HumanId + ".xml";
            string strXmlFilePath = String.Empty;// Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            if (ScreenMode == "Menu")
            {
                LoadFromMaster(ResultList, FileName, strXmlFilePath);
            }
            else if (ScreenMode == "Queue")
            {
                ImmunizationHistoryManager immunizationMngr = new ImmunizationHistoryManager();
                IList<ImmunizationHistory> ImmHislst = new List<ImmunizationHistory>();
                IList<ImmunizationHistory> ilstImmBlob = new List<ImmunizationHistory>();
                IList<object> ilstImmBlobFinal = new List<object>();

                IList<string> sTagName = new List<string>();
                sTagName.Add("ImmunizationHistoryList");

                //ilstImmBlob = (IList<ImmunizationHistory>)UtilityManager.ReadBlob("Human", ClientSession.HumanId, sTagName);

                ilstImmBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, sTagName);
                //for (int iCount =0; iCount< ilstImmBlobFinal.Count; iCount++)
                //{
                //    ilstImmBlob.Add((ImmunizationHistory)ilstImmBlobFinal[iCount]);
                //}
                if (ilstImmBlobFinal != null && ilstImmBlobFinal.Count > 0)
                {
                    if (ilstImmBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstImmBlobFinal[0]).Count; iCount++)
                        {
                            ilstImmBlob.Add((ImmunizationHistory)((IList<object>)ilstImmBlobFinal[0])[iCount]);
                        }
                    }
                }

                var imm = from immRecord in ilstImmBlob where immRecord.Is_Deleted != "Y" select immRecord;
                ImmHislst = imm.ToList<ImmunizationHistory>();

                bool _is_from_current_encounter_data = false;
                //bool _is_entries_deleted = true;

                var immEnc = from immEncRecord in ilstImmBlob where immEncRecord.Encounter_ID == ClientSession.EncounterId select immEncRecord;
                IList<ImmunizationHistory> ilstTempimmEncRecord = immEnc.ToList<ImmunizationHistory>();

                if (ilstTempimmEncRecord.Count > 0)
                {
                    _is_from_current_encounter_data = true;
                }

                //if (File.Exists(strXmlFilePath) == true)
                //{
                //XmlDocument itemDoc = new XmlDocument();
                //XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                //XmlNodeList xmlTagName = null;
                //using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //{
                //    itemDoc.Load(fs);

                //    XmlText.Close();
                ////BugId:61234
                //if (itemDoc.GetElementsByTagName("ImmunizationHistoryList") != null && itemDoc.GetElementsByTagName("ImmunizationHistoryList").Count>0)
                //{
                //    XmlNodeList xmlDeleteCheckTagName = itemDoc.GetElementsByTagName("ImmunizationHistoryList")[0].ChildNodes;
                //   _is_entries_deleted = true;

                //    for (int j = 0; j < xmlDeleteCheckTagName.Count; j++)
                //    {
                //        for (int i = 0; i < xmlDeleteCheckTagName[j].Attributes.Count; i++)
                //        {
                //            if (xmlDeleteCheckTagName[j].Attributes[i].Name == "Is_Deleted")
                //            {
                //                if (xmlDeleteCheckTagName[j].Attributes[i].InnerText == "N")
                //                {
                //                    _is_entries_deleted = false;
                //                    break;
                //                }
                //            }

                //        }
                //    }

                //}
                //if (!_is_entries_deleted && itemDoc.GetElementsByTagName("ImmunizationHistoryList") != null && itemDoc.GetElementsByTagName("ImmunizationHistoryList").Count > 0)
                //if (ImmHislst.Count > 0)
               // {
                    //xmlTagName = itemDoc.GetElementsByTagName("ImmunizationHistoryList")[0].ChildNodes;

                    //if (xmlTagName != null && xmlTagName.Count > 0)
                    //{
                    //    for (int j = 0; j < xmlTagName.Count; j++)
                    //    {
                    //        string TagName = xmlTagName[j].Name;
                    //        XmlSerializer xmlserializer = new XmlSerializer(typeof(ImmunizationHistory));
                    //        ImmunizationHistory ImmunizationHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ImmunizationHistory;
                    //        IEnumerable<PropertyInfo> propInfo = null;
                    //        propInfo = from obji in ((ImmunizationHistory)ImmunizationHistory).GetType().GetProperties() select obji;

                    //        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                    //        {
                    //            XmlNode nodevalue = xmlTagName[j].Attributes[i];
                    //            {
                    //                if (propInfo != null)
                    //                {
                    //                    foreach (PropertyInfo property in propInfo)
                    //                    {
                    //                        if (property.Name.ToUpper() == nodevalue.Name.ToUpper())
                    //                        {
                    //                            if (property.PropertyType.Name.ToUpper() == "UINT64")
                    //                                property.SetValue(ImmunizationHistory, Convert.ToUInt64(nodevalue.Value), null);
                    //                            else if (property.PropertyType.Name.ToUpper() == "STRING")
                    //                                property.SetValue(ImmunizationHistory, Convert.ToString(nodevalue.Value), null);
                    //                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                    //                                property.SetValue(ImmunizationHistory, Convert.ToDateTime(nodevalue.Value), null);
                    //                            else if (property.PropertyType.Name.ToUpper() == "INT32")
                    //                                property.SetValue(ImmunizationHistory, Convert.ToInt32(nodevalue.Value), null);
                    //                            else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                    //                                property.SetValue(ImmunizationHistory, Convert.ToDecimal(nodevalue.Value), null);
                    //                            else
                    //                                property.SetValue(ImmunizationHistory, nodevalue.Value, null);
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }

                    //        ImmHislst.Add(ImmunizationHistory);
                    //        if (ImmunizationHistory.Encounter_ID == ClientSession.EncounterId)
                    //            _is_from_current_encounter_data = true;
                    //    }
                    if (_is_from_current_encounter_data)
                    {
                        if (ImmHislst != null && ImmHislst.Count > 0)
                        {
                            IList<ImmunizationHistory> lstImmuHisCurrEnc = new List<ImmunizationHistory>();
                            lstImmuHisCurrEnc = (from item in ImmHislst where item.Encounter_ID == ClientSession.EncounterId select item).ToList<ImmunizationHistory>();
                            if (lstImmuHisCurrEnc != null && lstImmuHisCurrEnc.Count > 0)
                            {
                                ResultList.Immunization = lstImmuHisCurrEnc;
                            }
                            else if (!Is_Delete)
                            {
                                ulong maxEncId = 0;
                                IList<ulong> lstEncId = (from item in ImmHislst select item.Encounter_ID).Distinct().ToList<ulong>();
                                if (lstEncId != null && lstEncId.Count > 0)
                                    maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                                foreach (ulong item in lstEncId)
                                    if (item > maxEncId && item < ClientSession.EncounterId)
                                        maxEncId = item;
                                lstImmuHisCurrEnc = (from item in ImmHislst where item.Encounter_ID == maxEncId select item).ToList<ImmunizationHistory>();
                                ResultList.Immunization = lstImmuHisCurrEnc;
                            }
                        }
                        else
                        {
                            ResultList.Immunization = ImmHislst;
                        }
                        Session["ImmunizationHistory"] = ResultList.Immunization;
                        if (ResultList.Immunization != null && ResultList.Immunization.Count > 0)
                        {
                            IList<ImmunizationHistory> ilstImmuGrid = new List<ImmunizationHistory>();
                            ilstImmuGrid = ResultList.Immunization;
                            grdImmunization.DataSource = null;
                            DataTable dt = new DataTable();
                            dt.Columns.Add("Immunization Procedure", typeof(string));
                            dt.Columns.Add("Administered Date", typeof(string));
                            dt.Columns.Add("Dose", typeof(string));
                            dt.Columns.Add("Route of Administration", typeof(string));
                            dt.Columns.Add("Immunization Source", typeof(string));
                            dt.Columns.Add("Id", typeof(ulong));
                            dt.Columns.Add("Immunization_Order_ID", typeof(string));

                            for (int i = 0; i < ilstImmuGrid.Count; i++)
                            {
                                if (ilstImmuGrid[i].Is_Deleted != "Y")
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["Immunization Procedure"] = ilstImmuGrid[i].Procedure_Code + "-" + ilstImmuGrid[i].Immunization_Description;
                                    dr["Administered Date"] = ilstImmuGrid[i].Administered_Date;
                                    if (ilstImmuGrid[i].Dose != 0)
                                        dr["Dose"] = ilstImmuGrid[i].Dose;
                                    dr["Route of Administration"] = ilstImmuGrid[i].Route_Of_Administration;
                                    dr["Immunization Source"] = ilstImmuGrid[i].Immunization_Source;
                                    dr["Id"] = ilstImmuGrid[i].Id;
                                    dr["Immunization_Order_ID"] = ilstImmuGrid[i].Immunization_Order_ID;

                                    dt.Rows.Add(dr);
                                }
                            }
                            grdImmunization.DataSource = dt;
                            grdImmunization.DataBind();

                            foreach (GridDataItem item in grdImmunization.Items)
                            {
                                TableCell selectCell = item["EditRows"];
                                ImageButton gd = (ImageButton)selectCell.Controls[0];
                                gd.ToolTip = "Edit";
                                TableCell selectCell1 = item["DeleteRows"];
                                ImageButton gdDel = (ImageButton)selectCell1.Controls[0];
                                gdDel.ToolTip = "Delete";
                            }
                        }
                        else
                        {
                            grdImmunization.DataSource = new string[] { };
                            grdImmunization.DataBind();

                        }
                    }
                    if (!_is_from_current_encounter_data)
                    {
                        ImmHislst.Clear();
                        LoadFromMaster(ResultList, FileName, strXmlFilePath);
                    }
                //}


            }
            else
            {
                LoadFromMaster(ResultList, FileName, strXmlFilePath);
            }
                        //fs.Close();
                        //fs.Dispose();
                    //}
            //    }





            //}
        }

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            try
            {
                IList<PhysicianProcedure> phyProcedureList = null;
                PhysicianProcedureManager phyProcedureManager = new PhysicianProcedureManager();
                phyProcedureList = phyProcedureManager.GetProceduresUsingPhysicianIDAndLabID(ClientSession.PhysicianId, "IMMUNIZATION PROCEDURE", 0, ClientSession.LegalOrg);
                //Cap - 3256
                //IList<PhysicianProcedure> Procedure = (from p in phyProcedureList where p.Physician_Procedure_Code.StartsWith("J") == false select p).ToList<PhysicianProcedure>();
                IList<PhysicianProcedure> Procedure = (from p in phyProcedureList select p).ToList<PhysicianProcedure>();
                fillImmunizationHistory(Procedure);
            }
            catch (Exception ExInvisible) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Immunization Load", "alert('" + ExInvisible.Message.Replace("'", " ") + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true); }
        }


        public void LoadFromMaster(ImmunizationHistoryDTO ResultList, string FileName, string strXmlFilePath)
        {
            ImmunizationMasterHistoryManager immunizationMasterMngr = new ImmunizationMasterHistoryManager();
            IList<ImmunizationMasterHistory> ImmMasterHislst = new List<ImmunizationMasterHistory>();
            IList<ImmunizationMasterHistory> ilstImmMasterHisBlob = new List<ImmunizationMasterHistory>();
            IList<object> ilstImmMasterHisBlobFinal = new List<object>();

            IList<string> sTagName = new List<string>();
            sTagName.Add("ImmunizationMasterHistoryList");

            //ilstImmMasterHisBlob = (IList<ImmunizationMasterHistory>)UtilityManager.ReadBlob( ClientSession.HumanId, sTagName);
            ilstImmMasterHisBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, sTagName);
            if (ilstImmMasterHisBlobFinal != null && ilstImmMasterHisBlobFinal.Count > 0)
            {
                if (ilstImmMasterHisBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstImmMasterHisBlobFinal[0]).Count; iCount++)
                    {
                        ilstImmMasterHisBlob.Add((ImmunizationMasterHistory)((IList<object>)ilstImmMasterHisBlobFinal[0])[iCount]);
                    }
                }
            }

            var imm = from immRecord in ilstImmMasterHisBlob where immRecord.Is_Deleted != "Y" select immRecord;
            ImmMasterHislst = imm.ToList<ImmunizationMasterHistory>();

                //if (File.Exists(strXmlFilePath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                //    XmlNodeList xmlTagName = null;
                //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);

                //        XmlText.Close();

                //        if (itemDoc.GetElementsByTagName("ImmunizationMasterHistoryList") != null && itemDoc.GetElementsByTagName("ImmunizationMasterHistoryList").Count > 0)
                //        {
                //            xmlTagName = itemDoc.GetElementsByTagName("ImmunizationMasterHistoryList")[0].ChildNodes;

                //            if (xmlTagName != null && xmlTagName.Count > 0)
                //            {
                //                for (int j = 0; j < xmlTagName.Count; j++)
                //                {
                //                    string TagName = xmlTagName[j].Name;
                //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(ImmunizationMasterHistory));
                //                    ImmunizationMasterHistory ImmunizationMasterHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ImmunizationMasterHistory;
                //                    IEnumerable<PropertyInfo> propInfo = null;
                //                    propInfo = from obji in ((ImmunizationMasterHistory)ImmunizationMasterHistory).GetType().GetProperties() select obji;

                //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                    {
                //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                        {
                //                            if (propInfo != null)
                //                            {
                //                                foreach (PropertyInfo property in propInfo)
                //                                {
                //                                    if (property.Name.ToUpper() == nodevalue.Name.ToUpper())
                //                                    {
                //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                            property.SetValue(ImmunizationMasterHistory, Convert.ToUInt64(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                            property.SetValue(ImmunizationMasterHistory, Convert.ToString(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                            property.SetValue(ImmunizationMasterHistory, Convert.ToDateTime(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                            property.SetValue(ImmunizationMasterHistory, Convert.ToInt32(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                //                                            property.SetValue(ImmunizationMasterHistory, Convert.ToDecimal(nodevalue.Value), null);
                //                                        else
                //                                            property.SetValue(ImmunizationMasterHistory, nodevalue.Value, null);
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                    if (ImmunizationMasterHistory.Is_Deleted != "Y")
                //                        ImmMasterHislst.Add(ImmunizationMasterHistory);
                //                }
                //            }
                //        }
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}

                if (ImmMasterHislst != null && ImmMasterHislst.Count > 0)
            {
                ResultList.ImmunizationMasterList = ImmMasterHislst;
                Session["ImmunizationMasterHistory"] = ResultList.ImmunizationMasterList;
                if (ScreenMode == "Queue")
                    Session["LoadImmunizationMasterList"] = ImmMasterHislst;
            }
            else
            {
                if (ScreenMode == "Queue")
                    Session["LoadImmunizationMasterList"] = null;
            }



            if (ResultList.ImmunizationMasterList != null)
            {
                IList<ImmunizationMasterHistory> ilstImmuGrid = new List<ImmunizationMasterHistory>();
                ilstImmuGrid = ResultList.ImmunizationMasterList;
                grdImmunization.DataSource = null;
                DataTable dt = new DataTable();
                dt.Columns.Add("Immunization Procedure", typeof(string));
                dt.Columns.Add("Administered Date", typeof(string));

                dt.Columns.Add("Dose", typeof(string));
               
                dt.Columns.Add("Route of Administration", typeof(string));
                dt.Columns.Add("Immunization Source", typeof(string));
                dt.Columns.Add("Id", typeof(ulong));
                dt.Columns.Add("Immunization_Order_ID", typeof(string));
                for (int i = 0; i < ilstImmuGrid.Count; i++)
                {
                    if (ilstImmuGrid[i].Is_Deleted != "Y")
                    {
                        DataRow dr = dt.NewRow();
                        dr["Immunization Procedure"] = ilstImmuGrid[i].Procedure_Code + "-" + ilstImmuGrid[i].Immunization_Description;
                        dr["Administered Date"] = ilstImmuGrid[i].Administered_Date;
                          
                        if (ilstImmuGrid[i].Dose != 0)
                            dr["Dose"] = ilstImmuGrid[i].Dose;
                        dr["Route of Administration"] = ilstImmuGrid[i].Route_Of_Administration;
                        dr["Immunization Source"] = ilstImmuGrid[i].Immunization_Source;
                        dr["Id"] = ilstImmuGrid[i].Id;
                        dr["Immunization_Order_ID"] = ilstImmuGrid[i].Immunization_Order_ID;
                        dt.Rows.Add(dr);
                    }
                }
                grdImmunization.DataSource = dt;
                grdImmunization.DataBind();

                foreach (GridDataItem item in grdImmunization.Items)
                {
                    TableCell selectCell = item["EditRows"];
                    ImageButton gd = (ImageButton)selectCell.Controls[0];
                    gd.ToolTip = "Edit";
                    TableCell selectCell1 = item["DeleteRows"];
                    ImageButton gdDel = (ImageButton)selectCell1.Controls[0];
                    gdDel.ToolTip = "Delete";
                }
            }
            else
            {
                grdImmunization.DataSource = new string[] { };
                grdImmunization.DataBind();

            }
        }

    }
}
