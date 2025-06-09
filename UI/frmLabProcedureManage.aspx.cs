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
using Acurus.Capella.Core.DTO;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Drawing;
using System.Text;

namespace Acurus.Capella.UI
{
    public partial class frmLabProcedureManage : System.Web.UI.Page
    {
        #region Declaration
        IList<PhysicianProcedure> phyProcedureslist
        {
            get
            {
                return (IList<PhysicianProcedure>)ViewState["phyProcedureslist"];
            }
            set
            {
                ViewState["phyProcedureslist"] = value;
            }
        }
        IList<string> panelName = new List<string>();

        string procedureType = string.Empty;
        string SelectedLabID = string.Empty;
        string IsImmunizationHistory = string.Empty;
        bool sAllProcedures = false;

        EAndMCodingManager objEAndMCodingManager = new EAndMCodingManager();

        IList<PhysicianProcedure> delList = new List<PhysicianProcedure>();
        Dictionary<string, string> dictOrderCodeAndPanel = new Dictionary<string, string>();
        Dictionary<string, string> warningmsg = new Dictionary<string, string>();

        #endregion

        #region Methods

        void formload()
        {
            if (Request["ScreenMode"] == "ESuperbill")
            {
                sAllProcedures = false;
                hdnEandMCode.Value = "EandMCode";
            }
            hdnIsDirty.Value = string.Empty;
            hdnSelectedCPT.Value = string.Empty;
            if (!(procedureType.StartsWith("LAB") || procedureType.StartsWith("IMAGING") || procedureType.StartsWith("DME")))
            {
                pnlGroupDetail.Visible = false;
            }
            bool space = false;
            char[] str = procedureType.ToCharArray();
            StringBuilder retVal = new StringBuilder(32);
            if (str.Length > 0)
            {
                retVal.Append(char.ToUpper(str[0]));
                for (int i = 1; i < str.Length; i++)
                {
                    if (char.IsSeparator(str[i]))
                    {
                        space = true;
                        retVal.Append(str[i]);
                    }
                    else
                    {
                        if (space == true)
                        {
                            retVal.Append(char.ToUpper(str[i]));
                            space = false;
                        }
                        else
                        {
                            retVal.Append(char.ToLower(str[i]));
                        }

                    }
                }
            }
            lblEnterDescription.Text = "Enter " + retVal.ToString().Replace("E And M", "") + " Description";
            lblEnterCPTCode.Text = "Enter " + retVal.ToString().Replace("E And M", "") + " Code";
            if (!(procedureType.StartsWith("LAB") || procedureType.StartsWith("IMAGING") || procedureType.StartsWith("DME")))
            {
                phyProcedureslist = objEAndMCodingManager.GetPhysicianProcedure(ClientSession.PhysicianId, procedureType.ToUpper(), Convert.ToUInt32(SelectedLabID), ClientSession.LegalOrg);
                if (procedureType.ToUpper() == "IMMUNIZATION PROCEDURE" && IsImmunizationHistory == "Y")
                {
                    //Cap - 3256
                    //IList<PhysicianProcedure> Procedure = (from p in phyProcedureslist where p.Physician_Procedure_Code.StartsWith("J") == false select p).ToList<PhysicianProcedure>();
                    IList<PhysicianProcedure> Procedure = (from p in phyProcedureslist select p).ToList<PhysicianProcedure>();
                    FillPhysicianLabProcedure(Procedure, string.Empty);
                }
                else
                {
                    FillPhysicianLabProcedure(phyProcedureslist, string.Empty);
                }
            }
            else
            {
                phyProcedureslist = objEAndMCodingManager.GetPhysicianProcedure(ClientSession.PhysicianId, procedureType.ToUpper(), Convert.ToUInt32(SelectedLabID),ClientSession.LegalOrg);
                LoadCombo();
                FillPhysicianLabProcedure(phyProcedureslist, cboPanelName.SelectedValue.ToString());
            }
            if (IsImmunizationHistory == "Y" && UIManager.is_Menu_Level_PFSH)
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = false;
            }
            if (sAllProcedures)
            {
                cboPanelName.Text = "OTHER";
                cboPanelName.Enabled = false;
                btnSave.Text = "OK";
            }
            else
            {
                btnSave.Text = "Save";
                cboPanelName.Focus();
            }
        }

        void CheckPanel()
        {
            warningmsg = new Dictionary<string, string>();
            string OCType = string.Empty;
            var totalcount = lstLabProcedureAll.Items.Cast<ListItem>().Where(item => item.Selected);
            dictOrderCodeAndPanel = (Dictionary<string, string>)ViewState["dictOrderCodeAndPanel"];
            for (int i = 0; i < totalcount.Count(); i++)
            {
                if (dictOrderCodeAndPanel.Count > 0)
                    OCType = dictOrderCodeAndPanel[(totalcount.ElementAt(i).Text)];
                if (OCType != null && OCType.Trim() != string.Empty && OCType.Trim() != cboPanelName.Text.Trim())
                    warningmsg.Add(totalcount.ElementAt(i).Text, OCType);
            }
        }

        void LoadCombo()
        {
            panelName = new List<string>();
            RadComboBoxItem cmbItem = new RadComboBoxItem();
            IList<string> CboValues = new List<string>();

            CboValues = (from rec in phyProcedureslist select rec.Order_Group_Name).Distinct().ToList<string>();
            cboPanelName.DataSource = CboValues;
            cboPanelName.DataBind();
            cboPanelName.SelectedIndex = 0;

        }

        void AttachOnClickEvent()
        {
            if (lstLabProcudurePhysician.Items.Count > 0)
            {
                for (int i = 0; i < lstLabProcudurePhysician.Items.Count; i++)
                {
                    lstLabProcudurePhysician.Items[i].Attributes.Add("onClick", "EnableSaveButton()");
                }
            }
        }

        void DeleteSelectedProcedure()
        {
            if (hdnDeletedCPT.Value != string.Empty)
            {
                string[] DeletedCPT = hdnDeletedCPT.Value.Split('|');
                ArrayList existingList = new ArrayList();
                if (phyProcedureslist.Count > 0)
                {
                    existingList = new ArrayList();
                    foreach (PhysicianProcedure obj in phyProcedureslist)
                        existingList.Add(obj.Physician_Procedure_Code + "-" + obj.Procedure_Description);
                }

                for (int i = 0; i < DeletedCPT.Count(); i++)
                {
                    if (existingList.Contains(DeletedCPT[i].ToString()))
                    {
                        string[] split = DeletedCPT[i].ToString().Split('-');
                        if ((from obj in this.phyProcedureslist where obj.Physician_Procedure_Code == split[0] select obj).ToList<PhysicianProcedure>().Count > 0)
                        {
                            PhysicianProcedure objPhyProcedure = (from obj in this.phyProcedureslist where obj.Physician_Procedure_Code == split[0] select obj).ToList<PhysicianProcedure>()[0];
                            lstLabProcudurePhysician.Items.Remove(DeletedCPT[i].ToString());
                            if (delList.Contains(objPhyProcedure) == false)
                            {
                                delList.Add(objPhyProcedure);
                            }
                        }
                    }
                    else
                    {
                        lstLabProcudurePhysician.Items.Remove(DeletedCPT[i].ToString());
                    }
                }
                btnSave.Enabled = true;

            }

        }

        private void FillPhysicianLabProcedure(IList<PhysicianProcedure> listPro, string PanelName)
        {

            if ((procedureType.StartsWith("LAB") || procedureType.StartsWith("IMAGING")) || procedureType.StartsWith("DME"))
            {
                if (lstLabProcudurePhysician.Items.Count > 0)
                    lstLabProcudurePhysician.Items.Clear();
                foreach (PhysicianProcedure obj in listPro.Where(a => a.Order_Group_Name == PanelName.Trim()).ToList<PhysicianProcedure>())
                {
                    lstLabProcudurePhysician.Items.Add((obj.Physician_Procedure_Code + "-" + obj.Procedure_Description).ToString());
                }
                return;
            }

            if (listPro != null)
            {
                if (listPro.Count > 0)
                {
                    if (lstLabProcudurePhysician.Items.Count > 0)
                        lstLabProcudurePhysician.Items.Clear();

                    for (int i = 0; i < listPro.Count; i++)
                    {
                        if (IsImmunizationHistory == "Y")
                        {
                            bool IsInjection = false;

                            if (IsInjection == false)
                            {
                                lstLabProcudurePhysician.Items.Add((listPro[i].Physician_Procedure_Code + "-" + listPro[i].Procedure_Description).ToString());
                            }
                        }
                        else
                        {
                            lstLabProcudurePhysician.Items.Add((listPro[i].Physician_Procedure_Code + "-" + listPro[i].Procedure_Description).ToString());
                        }

                    }

                }
            }
            AttachOnClickEvent();
        }

        private void FillLabProcedureAll(IList<ProcedureCodeLibrary> listPro)
        {
            dictOrderCodeAndPanel = new Dictionary<string, string>();
            if (listPro != null)
            {
                if (listPro.Count > 0)
                {
                    if (lstLabProcedureAll.Items.Count > 0)
                        lstLabProcedureAll.Items.Clear();

                    for (int i = 0; i < listPro.Count; i++)
                    {
                        if (IsImmunizationHistory == "Y")
                        {
                            bool IsInjection = false;
                            //Cap - 3302
                            ////if (listPro[i].Procedure_Code.ToUpper().StartsWith("J"))
                            //{
                            //    IsInjection = true;
                            //    break;
                            //}
                            if (IsInjection == false)
                            {
                                lstLabProcedureAll.Items.Add((listPro[i].Procedure_Code + "-" + listPro[i].Procedure_Description).ToString());
                            }
                        }
                        else
                        {
                            lstLabProcedureAll.Items.Add((listPro[i].Procedure_Code + "-" + listPro[i].Procedure_Description).ToString());
                            dictOrderCodeAndPanel.Add(listPro[i].Procedure_Code + "-" + listPro[i].Procedure_Description, listPro[i].Order_Group_Name);
                        }

                    }
                    ViewState["dictOrderCodeAndPanel"] = dictOrderCodeAndPanel;
                }
            }

        }

        private bool CheckProcedureSelection(CheckBoxList lvw, string text)
        {
            ArrayList errList = null;
            if (lvw.Items.Count == 0)
            {
                errList = new ArrayList();
                errList.Add(text);

                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessageList('230005','" + text + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return false;
            }
            var totalcount = lvw.Items.Cast<ListItem>().Where(item => item.Selected);
            if (totalcount.Count() == 0)
            {
                errList = new ArrayList();
                errList.Add(text);

                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessageList('230003','" + text + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                return false;
            }

            return true;
        }

        private void FillOrderCodes(IList<OrderCodeLibrary> orderCodeList)
        {
            dictOrderCodeAndPanel = new Dictionary<string, string>();
            if (orderCodeList != null)
            {
                if (orderCodeList.Count > 0)
                {
                    if (lstLabProcedureAll.Items.Count > 0)
                        lstLabProcedureAll.Items.Clear();

                    for (int i = 0; i < orderCodeList.Count; i++)
                    {
                        lstLabProcedureAll.Items.Add((orderCodeList[i].Order_Code + "-" + orderCodeList[i].Order_Code_Name).ToString());
                        dictOrderCodeAndPanel.Add(orderCodeList[i].Order_Code + "-" + orderCodeList[i].Order_Code_Name, orderCodeList[i].Order_Group_Name);
                    }
                    ViewState["dictOrderCodeAndPanel"] = dictOrderCodeAndPanel;
                }
            }
            AttachOnClickEvent();
        }

        private void InsertLabProcedure(ArrayList listLabNames)
        {
            IList<PhysicianProcedure> saveList = new List<PhysicianProcedure>();
            PhysicianProcedure objProcedure = new PhysicianProcedure();
            if (listLabNames != null)
            {
                if (listLabNames.Count > 0)
                {

                    for (int i = 0; i < listLabNames.Count; i++)
                    {
                        objProcedure = new PhysicianProcedure();
                        string labProcedure = listLabNames[i].ToString();
                        string[] arr = labProcedure.Split('-'); ;
                        objProcedure.Created_By = ClientSession.UserName;
                        objProcedure.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        if (arr.Length > 0)
                        {
                            if (arr.Length > 2)
                            {
                                for (int k = 1; k < arr.Length; k++)
                                {
                                    if (k == 1)
                                        objProcedure.Procedure_Description = arr[k].ToString();
                                    else
                                        objProcedure.Procedure_Description += "-" + arr[k].ToString();
                                }
                            }
                            else
                                objProcedure.Procedure_Description = arr[1].ToString();
                            objProcedure.Physician_Procedure_Code = arr[0].ToString();
                        }
                        objProcedure.Procedure_Type = procedureType;
                        objProcedure.Order_Group_Name = cboPanelName.Text.Trim();
                        objProcedure.Modified_By = ClientSession.UserName;
                        objProcedure.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        objProcedure.Physician_ID = ClientSession.PhysicianId;
                        objProcedure.Lab_ID = Convert.ToUInt32(SelectedLabID);
                        objProcedure.Legal_Org = ClientSession.LegalOrg;

                        saveList.Add(objProcedure);
                        if ((procedureType.StartsWith("LAB") || procedureType.StartsWith("IMAGING")) || procedureType.StartsWith("DME") && !panelName.Contains(objProcedure.Order_Group_Name))
                        {
                            panelName.Add(objProcedure.Order_Group_Name);
                            cboPanelName.Items.Add(new RadComboBoxItem(objProcedure.Order_Group_Name));
                        }
                    }
                }
            }
            if (delList.Count > 0)
            {
                foreach (PhysicianProcedure obj in delList)
                {
                    obj.Modified_By = ClientSession.UserName;
                    obj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
            }
            if (saveList.Count > 0 || delList.Count > 0)
            {
                hdnIsDirty.Value = "true";
                PhysicianProcedureManager objPhysicianProcedureManager = new PhysicianProcedureManager();
                objPhysicianProcedureManager.SaveUpdateDeleteLabProcedure(saveList, delList);
                this.phyProcedureslist = objEAndMCodingManager.GetPhysicianProcedure(ClientSession.PhysicianId, procedureType.ToUpper(), Convert.ToUInt32(SelectedLabID), ClientSession.LegalOrg);
            }

        }


        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            IsImmunizationHistory = Convert.ToString(Request.QueryString["IsImmunizationHistory"]) == "Y" ? "Y" : "N";
            SelectedLabID = Convert.ToString(Request["selectedLabID"]);
            procedureType = Convert.ToString(Request.QueryString["procedureType"]);
            int FrequProcHgt = 0;
            int ProcedureCodeHgt = 0;
            if (Request.Cookies["FrequProcHgt"] != null && int.TryParse(Request.Cookies["FrequProcHgt"].Value, out FrequProcHgt))
                gbFrequentlyUsedProcedures.Height = Unit.Pixel(FrequProcHgt);
            if (Request.Cookies["ProcedureCodeHgt"] != null && int.TryParse(Request.Cookies["ProcedureCodeHgt"].Value, out ProcedureCodeHgt))
                gbProcedureCodeLibrary.Height = Unit.Pixel(ProcedureCodeHgt);

            if (!IsPostBack)
            {
                formload();
                SecurityServiceUtility obj = new SecurityServiceUtility();
                obj.ApplyUserPermissions(this.Page);
                btnSave.Enabled = false;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RadButton btnSender = (RadButton)sender;
            lstLabProcedureAll.Items.Clear();
            lblResult.Text = string.Empty;
            string sDate = string.Empty;
            if (ClientSession.EncounterId != 0)
            {
                sDate = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.ToString("yyyy-MM-dd");
            }
            else
                sDate = DateTime.Now.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(txtEnterCPTCode.Text) && string.IsNullOrEmpty(txtEnterDescription.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            else
            {
                if ((SelectedLabID == "1" || SelectedLabID == "2") && procedureType == "LAB PROCEDURE")
                {
                    OrderCodeLibraryManager OrderCodeLibraryMngr = new OrderCodeLibraryManager();
                    IList<OrderCodeLibrary> orderCodeList = OrderCodeLibraryMngr.SearchOrderCodeForCodeAndDesc(txtEnterCPTCode.Text.Trim(), txtEnterDescription.Text.Trim().Replace(' ', '%'), Convert.ToUInt32(SelectedLabID));
                    FillOrderCodes(orderCodeList);
                    if (orderCodeList != null)
                    {
                        if (orderCodeList.Count > 0)
                            lblResult.Text = orderCodeList.Count.ToString() + " Result(s) Found";
                        else
                            lblResult.Text = "No Result(s) Found";
                    }
                    else
                        lblResult.Text = "No Result(s) Found";
                }
                else
                {
                    ProcedureCodeLibraryManager objOrderProxy = new ProcedureCodeLibraryManager();
                    IList<ProcedureCodeLibrary> list = null;

                    if ((Request.Form["btnSearchAll"] == null && Request.Form["btnSearch"] == null
                        && Request.Form["__EVENTTARGET"] == "btnSearch") || Request.Form["btnSearchAll"] == "Search All"
                        && Request.Form["btnSearch"] == null)
                    {
                        list = objOrderProxy.SearchProcedureCodeBasedOnCPTAndDesc(txtEnterCPTCode.Text.Trim(), txtEnterDescription.Text.Trim().Replace(' ', '%'), procedureType,sDate);
                    }
                    else
                    {
                        list = objOrderProxy.SearchProcedureCodeBasedOnCPTAndDesc(txtEnterCPTCode.Text.Trim(), txtEnterDescription.Text.Trim().Replace(' ', '%'), string.Empty,sDate);
                    }

                    FillLabProcedureAll(list);

                    if (list != null)
                    {
                        if (list.Count > 0)
                            lblResult.Text = lstLabProcedureAll.Items.Count.ToString() + " Result(s) Found";
                        else
                            lblResult.Text = "No Result(s) Found";
                    }
                    else
                        lblResult.Text = "No Result(s) Found";
                }
                lblResult.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            AttachOnClickEvent();

        }

        protected void btnInvisibleBack_Click(object sender, EventArgs e)
        {
            DeleteSelectedProcedure();
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnMove_Click(object sender, EventArgs e)
        {
            if ((CheckProcedureSelection(lstLabProcedureAll, "to move to Frequently Used Procedures")))
            {
                int count = 0;
                bool flag = false;

                IList<PhysicianProcedure> list = delList;
                if (!(lblEnterDescription.Text.Contains("Lab") || lblEnterDescription.Text.Contains("Imaging")))
                    goto label1;
                CheckPanel();
                if (warningmsg.Count > 0)
                {
                    StringBuilder sbErrorMsg = new StringBuilder();
                    foreach (var dt in warningmsg)
                    {
                        sbErrorMsg.Append(dt.Key.ToString() + " belongs to " + dt.Value.ToString() + "\n");
                    }
                    if (hdnMoveClick.Value == "0")
                    {
                        hdnMoveClick.Value = sbErrorMsg.ToString().Replace('-', ':');
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "CheckIfOTHERTEST();", true);
                        return;
                    }
                    if (hdnMoveClick.Value == "1,true")
                    {
                        hdnMoveClick.Value = "0";
                        goto label1;
                    }
                    else if (hdnMoveClick.Value == "1,false")
                    {
                        hdnMoveClick.Value = "0";
                        return;
                    }
                }
            label1:
                var totalcount = lstLabProcedureAll.Items.Cast<ListItem>().Where(item => item.Selected);
                if (lstLabProcedureAll.Items.Count > 0 && totalcount.Count() > 0)
                {
                    flag = true;
                    for (int i = 0; i < totalcount.Count(); i++)
                    {
                        count = 0;
                        if (lstLabProcudurePhysician.Items.Count > 0)
                        {
                            for (int j = 0; j < phyProcedureslist.Count; j++)
                            {
                                if (totalcount.ElementAt(i).Text.ToString().ToUpper().Split('-')[0] == (phyProcedureslist[j].Physician_Procedure_Code + "-" + phyProcedureslist[j].Procedure_Description).ToUpper().Split('-')[0])
                                {
                                    flag = false;
                                    count++;
                                }

                            }
                            for (int j = 0; j < lstLabProcudurePhysician.Items.Count; j++)
                            {
                                if (totalcount.ElementAt(i).Text.ToString().ToUpper().Split('-')[0] == lstLabProcudurePhysician.Items[j].ToString().ToUpper().Split('-')[0])
                                {
                                    flag = false;
                                    count++;
                                }
                            }

                        }
                        if (count == 0)
                        {
                            bool ThrowWarningMsgForDuplicateEntry = true;
                            for (int j = 0; j < phyProcedureslist.Count; j++)
                            {
                                if (totalcount.ElementAt(i).Text.ToString().ToUpper().Split('-')[0] == (phyProcedureslist[j].Physician_Procedure_Code + "-" + phyProcedureslist[j].Procedure_Description).ToUpper().Split('-')[0])
                                {
                                    ThrowWarningMsgForDuplicateEntry = false;
                                }

                            }

                            if (ThrowWarningMsgForDuplicateEntry)
                            {
                                ListItem tempListItem = new ListItem();
                                tempListItem.Text = totalcount.ElementAt(i).Text.ToString();
                                tempListItem.Selected = true;

                                lstLabProcudurePhysician.Items.Add(tempListItem);
                                flag = true;
                                btnSave.Enabled = true;
                            }
                        }

                        if (list != null)
                        {
                            if (list.Count > 0)
                            {
                                foreach (PhysicianProcedure obj in list)
                                {
                                    if (obj.Physician_Procedure_Code == totalcount.ElementAt(i).Text.ToString().ToUpper().Split('-')[0])
                                    {
                                        delList.Remove(obj);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (flag == true)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230001'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230002'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    }

                    for (int i = 0; i < lstLabProcedureAll.Items.Count; i++)
                    {
                        lstLabProcedureAll.Items[i].Selected = false;
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void cboPanelName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            FillPhysicianLabProcedure(phyProcedureslist, cboPanelName.Text.ToString());
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (hdnMessageType.Value == "Yes")
            {
                hdnRefresh.Value = "true";
            }
            if (btnSave.Text == "Save")
            {
                if ((procedureType.StartsWith("LAB") || procedureType.StartsWith("IMAGING") || procedureType.StartsWith("DME")))
                {
                    if (cboPanelName.Text == string.Empty)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Manage Frequently Used Procedure", "DisplayErrorMessage('220112'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;
                    }
                }

                if ((lblEnterDescription.Text.Contains("Lab") || lblEnterDescription.Text.Contains("Imaging")) && cboPanelName.Text.Trim() == string.Empty)
                {
                    return;
                }
                btnSave.Enabled = false;
                DeleteSelectedProcedure();
                ArrayList labName = new ArrayList();

                if (lstLabProcudurePhysician.Items.Count > 0)
                {

                    for (int i = 0; i < lstLabProcudurePhysician.Items.Count; i++)
                    {
                        labName.Add(lstLabProcudurePhysician.Items[i].ToString());
                    }
                }
                if (phyProcedureslist.Count > 0)
                {
                    for (int i = 0; i < labName.Count; i++)
                    {
                        if (labName.Count > 0)
                        {

                            for (int j = 0; j < phyProcedureslist.Count; j++)
                            {
                                if (labName[i].ToString() == (phyProcedureslist[j].Physician_Procedure_Code + "-" + phyProcedureslist[j].Procedure_Description).ToString())
                                {

                                    labName.Remove((phyProcedureslist[j].Physician_Procedure_Code + "-" + phyProcedureslist[j].Procedure_Description).ToString());
                                    i = i - 1;
                                    break;
                                }
                            }
                        }
                    }

                    InsertLabProcedure(labName);
                }
                else
                {
                    InsertLabProcedure(labName);
                }

                delList.Clear();
                if ((lblEnterDescription.Text.Contains("Lab") || lblEnterDescription.Text.Contains("Imaging")))
                {
                    cboPanelName.SelectedIndex = panelName.IndexOf(cboPanelName.Text);
                }
                if (Request["ScreenMode"] == "ESuperbill" || hdnEandMCode.Value == "EandMCode")
                {
                    for (int i = 0; i < labName.Count; i++)
                    {
                        if (hdnSelectedCPT.Value == string.Empty)
                        {
                            hdnSelectedCPT.Value += labName[i];
                        }
                        else
                        {
                            hdnSelectedCPT.Value += "|" + labName[i];
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, typeof(frmLabProcedureManage), "CancelKeyFalse", "oncloseclick();", true);
                }
                else
                {
                    for (int i = 0; i < phyProcedureslist.Count; i++)
                    {

                        if (hdnSelectedCPT.Value == string.Empty)
                        {
                            hdnSelectedCPT.Value += phyProcedureslist[i].ToString();
                        }
                        else
                        {
                            hdnSelectedCPT.Value += "|" + phyProcedureslist[i].ToString();
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, typeof(frmLabProcedureManage), string.Empty, "EnableSave();", true);

                }
                btnSave.Enabled = false;
            }
            else if (btnSave.Text == "OK")
            {
                ArrayList labName = new ArrayList();
                PhysicianProcedure objProcedure = null;
                if (lstLabProcudurePhysician.Items.Count > 0)
                {
                    for (int i = 0; i < lstLabProcudurePhysician.Items.Count; i++)
                    {
                        labName.Add(lstLabProcudurePhysician.Items[i].ToString());
                    }
                }
                btnSave.Enabled = false;
                if (phyProcedureslist.Count > 0)
                {
                    for (int i = 0; i < labName.Count; i++)
                    {
                        if (labName.Count > 0)
                        {

                            for (int j = 0; j < phyProcedureslist.Count; j++)
                            {
                                if (labName[i].ToString() == (phyProcedureslist[j].Physician_Procedure_Code + "-" + phyProcedureslist[j].Procedure_Description).ToString())
                                {

                                    labName.Remove((phyProcedureslist[j].Physician_Procedure_Code + "-" + phyProcedureslist[j].Procedure_Description).ToString());
                                    i = i - 1;
                                    break;
                                }
                            }
                        }
                    }
                }
                for (int j = 0; j < labName.Count; j++)
                {

                    objProcedure = new PhysicianProcedure();
                    objProcedure.Order_Group_Name = cboPanelName.Text.Trim();
                    string labProcedure = labName[j].ToString();
                    string[] arr = labProcedure.Split('-'); ;
                    if (arr.Length > 0)
                    {
                        if (arr.Length > 2)
                        {
                            for (int k = 1; k < arr.Length; k++)
                            {
                                if (k == 1)
                                    objProcedure.Procedure_Description = arr[k].ToString();
                                else
                                    objProcedure.Procedure_Description += "-" + arr[k].ToString();
                            }
                        }
                        else
                            objProcedure.Procedure_Description = arr[1].ToString();
                        objProcedure.Physician_Procedure_Code = arr[0].ToString();
                    }
                    phyProcedureslist.Add(objProcedure);
                }
                phyProcedureslist = phyProcedureslist.Except(delList).ToList();

                if (Request["ScreenMode"] == "ESuperbill")
                {
                    for (int i = 0; i < labName.Count; i++)
                    {
                        if (hdnSelectedCPT.Value == string.Empty)
                        {
                            hdnSelectedCPT.Value += labName[i];
                        }
                        else
                        {
                            hdnSelectedCPT.Value += "|" + labName[i];
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < phyProcedureslist.Count; i++)
                    {

                        if (hdnSelectedCPT.Value == string.Empty)
                        {
                            hdnSelectedCPT.Value += phyProcedureslist[i].ToString();
                        }
                        else
                        {
                            hdnSelectedCPT.Value += "|" + phyProcedureslist[i].ToString();
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, typeof(frmLabProcedureManage), "CancelKeyFalse", "oncloseclick();", true);
            }

        }

        protected void btnClearAll_Clicked(object sender, EventArgs e)
        {
            lstLabProcedureAll.Items.Clear();
            lblResult.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        #endregion
    }
}
