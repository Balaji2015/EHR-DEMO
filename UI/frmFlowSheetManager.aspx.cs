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
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Text;
using System.Drawing;

namespace Acurus.Capella.UI
{
    public partial class frmFlowSheetManager : System.Web.UI.Page
    {
        FlowSheetTemplateManager FlowsheetMngr = new FlowSheetTemplateManager();
        VitalsManager vitalMngr = new VitalsManager();
        PhysicianResultsManager PhysicianResultMngr = new PhysicianResultsManager();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (chklstResultItems.CheckedItems.Count > 0 ||
                chklstVitalsDataItems.CheckedItems.Count > 0)
            {
                btnAdd.Enabled = true;
                btnAddandGenerate.Enabled = true;
            }

            if (!IsPostBack)
            {
                btnAdd.Attributes.Add("disabled", "disabled");
                btnAddandGenerate.Attributes.Add("disabled", "disabled");

                LoadGrid();

                LoadPhysicianResults();

                LoadVitalsResults();

                txtTemplate.Focus();
            }
        }

        void LoadVitalsResults()
        {
            IList<DynamicScreen> dynamicScreenList = vitalMngr.GetAcurusCodesListforFlowsheet(ClientSession.PhysicianId,ClientSession.LegalOrg);

            foreach (DynamicScreen item in dynamicScreenList)
            {
                RadListBoxItem lstItem = new RadListBoxItem();
                lstItem.Text = item.Control_Name;
                lstItem.Value = item.Acurus_Result_Code;
                chklstVitalsDataItems.Items.Add(lstItem);
            }
        }

        void LoadPhysicianResults()
        {
            IList<PhysicianResults> PhysicianResultList = PhysicianResultMngr.GetPhysicianResults(ClientSession.PhysicianId,ClientSession.LegalOrg);

            foreach (PhysicianResults obj in PhysicianResultList)
            {
                RadListBoxItem lstItem = new RadListBoxItem();
                lstItem.Text = obj.Acurus_Result_Description;
                lstItem.Value = obj.Acurus_Result_Code;
                chklstResultItems.Items.Add(lstItem);
            }
        }

        public void LoadGrid()
        {
            IList<FlowSheetTemplate> FlowSheetList = FlowsheetMngr.GetFlowSheetTemplate(ClientSession.PhysicianId);

            if (FlowSheetList != null)
            {
                grdFlowSheet.DataSource = null;

                DataTable dtFlowSheet = new DataTable();

                dtFlowSheet.Columns.Add("Edit", typeof(Bitmap));
                dtFlowSheet.Columns.Add("Copy", typeof(Bitmap));
                dtFlowSheet.Columns.Add("Del", typeof(Bitmap));
                dtFlowSheet.Columns.Add("Flow SheetTemplateName", typeof(string));
                dtFlowSheet.Columns.Add("Data Items", typeof(string));
                dtFlowSheet.Columns.Add("FlowID", typeof(string));
                dtFlowSheet.Columns.Add("Version", typeof(string));
                dtFlowSheet.Columns.Add("Unit", typeof(string));
                dtFlowSheet.Columns.Add("Phy ID", typeof(string));

                dtFlowSheet.Columns.Add("Result Code", typeof(string));

                for (int i = 0; i < FlowSheetList.Count; i++)
                {
                    DataRow dr = dtFlowSheet.NewRow();

                    dr["Phy ID"] = FlowSheetList[i].Physician_ID;
                    dr["Flow SheetTemplateName"] = FlowSheetList[i].Template_Name;
                    dr["Data Items"] = FlowSheetList[i].Acurus_Result_Description;
                    dr["FlowID"] = FlowSheetList[i].Id;
                    dr["Version"] = FlowSheetList[i].Version;
                    dr["Unit"] = FlowSheetList[i].Unit;
                    dr["Result Code"] = FlowSheetList[i].Acurus_Result_Code;

                    dtFlowSheet.Rows.Add(dr);
                }

                grdFlowSheet.DataSource = dtFlowSheet;
                grdFlowSheet.DataBind();
            }
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                                "FlowSheetManagerClearAll();", true);
        }

        protected void InvisibleButtonClear_Click(object sender, EventArgs e)
        {
            ClearAllFields();
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                string.Empty,
                                                " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        string GetValuesFromVitalsResults(out string description)
        {
           // Dictionary<string, string> VitalsLoincList = new Dictionary<string, string>();
            IList<String> lstCodes = new List<String>();
            IList<String> lstDesc = new List<String>();

            for (int i = 0; i < chklstVitalsDataItems.CheckedItems.Count; i++)
            {
                var obj = chklstVitalsDataItems.CheckedItems[i];
                //VitalsLoincList.Add(obj.Value, obj.Text);
                lstCodes.Add(obj.Value);
                lstDesc.Add(obj.Text);
            }

            for (int i = 0; i < chklstResultItems.CheckedItems.Count; i++)
            {
                var obj = chklstResultItems.CheckedItems[i];
                lstCodes.Add(obj.Value);
                lstDesc.Add(obj.Text);
                //VitalsLoincList.Add(obj.Value, obj.Text);
            }

            var vitalDataItems = string.Join("|", lstCodes.ToArray());
            description = string.Join("|",lstDesc.ToArray());

            return vitalDataItems;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            IList<FlowSheetTemplate> UniqueList = new List<FlowSheetTemplate>();

            IList<FlowSheetTemplate> lstFlowSheetTemplate = FlowsheetMngr.GetFlowSheetTemplate(ClientSession.PhysicianId);

            bool IsUpdate = btnAdd.Text == "Add" ? false : true;

            UniqueList = lstFlowSheetTemplate
                                .Where(a => string.Compare(a.Template_Name, txtTemplate.Text.Trim(), true) == 0)
                                .ToList<FlowSheetTemplate>();

            ulong flowSheetTemplateId = 0;

            if (IsUpdate)
            {
                if (ulong.TryParse(DelID.Value, out flowSheetTemplateId))
                {
                    UniqueList = UniqueList.Where(a => a.Id != flowSheetTemplateId)
                                          .ToList<FlowSheetTemplate>();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                   string.Empty,
                                                   " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}",
                                                   true);
                    return;
                }
            }

            if (UniqueList.Count > 0)
            {
                if (UniqueList[0].Physician_ID != 0 || IsUpdate)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                   string.Empty,
                                                   "DisplayErrorMessage('102007'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}",
                                                   true);
                    txtTemplate.Focus();
                    return;
                }
            }

            IList<FlowSheetTemplate> lstAppendFlowSheet = new List<FlowSheetTemplate>();
            IList<FlowSheetTemplate> lstUpdateFlowSheet = new List<FlowSheetTemplate>();

            var Acurus_Result_Description = "";
            var Acurus_Result_Code = GetValuesFromVitalsResults(out Acurus_Result_Description);

            if (IsUpdate)
            {
                var queryresultList = from s in lstFlowSheetTemplate
                                      where s.Id == flowSheetTemplateId
                                      select s;

                var objUpdateFS = queryresultList.ToList<FlowSheetTemplate>().FirstOrDefault();

                if (objUpdateFS != null)
                {
                    objUpdateFS.Modified_By = ClientSession.UserName;
                    objUpdateFS.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();

                    objUpdateFS.Acurus_Result_Description = Acurus_Result_Description;
                    objUpdateFS.Acurus_Result_Code = Acurus_Result_Code;

                    lstUpdateFlowSheet.Add(objUpdateFS);
                }
                else
                {
                    return;
                    // Log info....
                }
            }
            else
            {
                //Changed by vaishali on 05-01-2015 for bug ID 36857
                if (txtTemplate.Text.Trim() != "")
                {
                    FlowSheetTemplate objFlowSheet = new FlowSheetTemplate();
                    objFlowSheet.Created_By = ClientSession.UserName;
                    objFlowSheet.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objFlowSheet.Modified_By = string.Empty;
                    objFlowSheet.Modified_Date_And_Time = DateTime.MinValue;
                    objFlowSheet.Sort_Order = 9999;

                    objFlowSheet.Acurus_Result_Description = Acurus_Result_Description;
                    objFlowSheet.Acurus_Result_Code = Acurus_Result_Code;
                    objFlowSheet.Template_Name = txtTemplate.Text.ToUpper().Trim();
                    objFlowSheet.Physician_ID = ClientSession.PhysicianId;

                    lstAppendFlowSheet.Add(objFlowSheet);
                }
            }
            //Changed by vaishali on 05-01-2015 for bug ID 36857
            if (lstAppendFlowSheet.Count > 0 || lstUpdateFlowSheet.Count > 0)
            {
                FlowsheetMngr.SaveUpdateDeleteWithTransaction(ref lstAppendFlowSheet, lstUpdateFlowSheet, null, string.Empty);

                Button ctrl = (Button)sender;

                if (ctrl.Text == "Update" || ctrl.Text == "Add")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                                        "DisplayErrorMessage('200028'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                }

                LoadGrid();
                ClearAllFields();
            }
        }

        private void ClearAllFields()
        {
            txtTemplate.Text = string.Empty;
            btnAdd.Text = "Add";
            btnClearAll.Text = "Clear All";
            btnAddandGenerate.Text = "Add and Generate ";

            txtTemplate.Focus();

            ResetVitalsResult();
            ResetLabResult();
        }

        void ResetVitalsResult()
        {
            for (int i = chklstVitalsDataItems.CheckedItems.Count - 1; i >= 0; i--)
            {
                chklstVitalsDataItems.CheckedItems[i].Checked = false; ;
            }
        }
        void ResetLabResult()
        {
            for (int i = chklstResultItems.CheckedItems.Count - 1; i >= 0; i--)
            {
                chklstResultItems.CheckedItems[i].Checked = false;
            }
        }

        void SetResultsDetails(string resultsValueItems)
        {
            var arrVitalsDataItems = resultsValueItems.Split('|');

            int iItems = -1;
            int iItemResult = -1;

            for (int i = 0; i < arrVitalsDataItems.Length; i++)
            {
                var d = arrVitalsDataItems[i];

                //iItems = chklstVitalsDataItems.FindItemIndexByValue(d);
                if (chklstVitalsDataItems.FindItemByText(d) != null)
                    iItems = chklstVitalsDataItems.FindItemByText(d).Index;
                else
                    iItems = -1;
                if (iItems >= 0)
                {
                    if (chklstVitalsDataItems.Items[iItems] != null)
                    {
                        chklstVitalsDataItems.Items[iItems].Checked = true;
                        continue;
                    }
                   
                }

                //iItemResult = chklstResultItems.FindItemIndexByValue(d);
                if (chklstResultItems.FindItemByText(d) != null)
                    iItemResult = chklstResultItems.FindItemByText(d).Index;
                else
                    iItemResult = -1;

                if (iItemResult >= 0)
                {
                    if (chklstResultItems.Items[iItemResult] != null)
                    {
                        chklstResultItems.Items[iItemResult].Checked = true;
                        continue;
                    }
                   
                }

                if (iItemResult < 0 && iItems < 0)
                {
                    RadListBoxItem lstItem = new RadListBoxItem();
                    lstItem.Text = d.ToString();
                    int iNewItems=-1;
                    chklstResultItems.Items.Add(lstItem);

                      if(chklstResultItems.FindItemByText(d)!=null)
                            iNewItems = chklstResultItems.FindItemByText(d).Index;

                    if (iNewItems > 0 || iNewItems == 0)
                    {
                        if (chklstResultItems.Items[iNewItems] != null)
                        {
                            chklstResultItems.Items[iNewItems].Checked = true;
                        }
                    }
                }
            }
        }

        protected void btnAddandGenerate_Click(object sender, EventArgs e)
        {
            string sValue = string.Empty;
            string sTemplateName = txtTemplate.Text;

            if (!string.IsNullOrEmpty(txtTemplate.Text.Trim()))
            {
                var functionJS = sTemplateName.Replace("'", "\\'").Trim(); ;
                btnAdd_Click(sender, e);
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                                    "OpenFlowSheet('" + functionJS + "');", true);
            }
            else
            {
            }
        }

        protected void grdFlowSheet_ItemCommand(object sender, GridCommandEventArgs e)
        {
            IList<FlowSheetTemplate> flowList = FlowsheetMngr.GetFlowSheetTemplate(ClientSession.PhysicianId);
            IList<string> ResultsFromFlowsheet = new List<string>();

            Dictionary<string, string> ResultCodeList = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(hdnSelectedIndex.Value))
            {
                DelID.Value = grdFlowSheet.MasterTableView.Items[Convert.ToInt32(hdnSelectedIndex.Value)]["FlowID"].Text;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();} {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

            #region delete
            if (e.CommandName == "Del")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                                    "DeleteItem('" + DelID.Value + "');", true);
                return;
            }
            #endregion
            #region Edit Copy
            else
            {
                ClearAllFields();

                txtTemplate.Text = grdFlowSheet.MasterTableView.Items[Convert.ToInt32(hdnSelectedIndex.Value)]["FlowSheetTemplateName"].Text;

                var resultValues = grdFlowSheet.MasterTableView.Items[Convert.ToInt32(hdnSelectedIndex.Value)]["DataItems"].Text;

                SetResultsDetails(resultValues);

                if (e.CommandName == "EditDel")
                {
                    btnAddandGenerate.Text = "Update and Generate";
                    btnAdd.Text = "Update";
                    btnClearAll.Text = "Cancel";
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "EnableSave(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            #endregion

            txtTemplate.Focus();
            hdnSelectedIndex.Value = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void grdFlowSheet_ItemCreated(object sender, GridItemEventArgs e)
        {
            e.Item.ToolTip = "";

            if (e.Item is GridDataItem)
            {
                GridDataItem gridItem = e.Item as GridDataItem;

                foreach (GridColumn column in grdFlowSheet.MasterTableView.RenderColumns)
                {
                    if (column.UniqueName == "Edit")
                    {
                        gridItem[column.UniqueName].ToolTip = "Edit";
                    }
                    if (column.UniqueName == "Copy")
                    {
                        gridItem[column.UniqueName].ToolTip = "Copy";
                    }
                    if (column.UniqueName == "Del")
                    {
                        gridItem[column.UniqueName].ToolTip = "Delete";
                    }
                }
            }
        }

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            IList<FlowSheetTemplate> flowList = FlowsheetMngr.GetFlowSheetTemplate(ClientSession.PhysicianId);

            FlowSheetTemplate objDel = (from c in flowList
                                        where c.Id == Convert.ToUInt64(DelID.Value)
                                        select c).FirstOrDefault();
            if (objDel != null)
                flowList = FlowsheetMngr.DeleteFlowSheetTemplate(objDel, string.Empty);

            LoadGrid();
            ClearAllFields();

            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                                        " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            var DuplicateList = hdnAddResults.Value.ToString().Split('|');

            var SearchList = hdnSearchResults.Value.ToString().Split('|');

            IList<string> finallist = SearchList.ToList<string>();

            foreach (var val in DuplicateList)
            {
                var RemoveList = from a in SearchList where (a == val) select a;

                if (RemoveList.Count() > 0)
                {
                    finallist.Remove(val.ToString());
                }
            }

            IList<PhysicianResults> PhysicianResultList = PhysicianResultMngr.GetPhysicianResults(ClientSession.PhysicianId,ClientSession.LegalOrg);
            if (finallist.Count > 0)
            {
                foreach (var value in finallist)
                {
                    string[] values = value.Split('_');
                    PhysicianResults obj = new PhysicianResults();
                    obj.Acurus_Result_Code = values[0].ToString();
                    obj.Acurus_Result_Description = values[1].ToString();

                    PhysicianResultList.Add(obj);
                    RadListBoxItem lstItem = new RadListBoxItem();
                    lstItem.Text = values[1].ToString();
                    chklstResultItems.Items.Add(lstItem);
                }
            }

            foreach (string value in SearchList)
            {
                string[] values = value.Split('_');

                if (values[0].ToString() != string.Empty)
                {
                    int iItemResult = chklstResultItems.FindItemIndexByValue(values[0].ToString());

                    if (iItemResult >= 0)
                    {
                        chklstResultItems.Items[iItemResult].Checked = true;
                    }
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "EnableSave();", true);
        }
    }
}