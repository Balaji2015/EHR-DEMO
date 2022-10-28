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
using Telerik.Web;
using Telerik.Web.UI;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;

namespace Acurus.Capella.UI
{
    public partial class frmInhouseProcedure : System.Web.UI.Page
    {
        InHouseProcedureManager ObjInHouse_Mgr = new InHouseProcedureManager();
        public InHouseProcedureDTO objOtherProDTO = null;
        InHouseProcedure objOtherProc;

        public IList<InHouseProcedure> lstUpdate;
        public ulong EncounterID
        {
            get
            {

                return ViewState["EncounterID"] == null ? 0 : Convert.ToUInt32(ViewState["EncounterID"]);
            }
            set
            {
                ViewState["EncounterID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            pbProcedure.MyTextBox = ctmDLC_procedure.txtDLC;
            pbProcedure.MyTextBox.Height = 50;
            pbProcedure.MyPlaceHolder = phlProce;
            pbProcedure.MyGridHeight = 210;

            if (!IsPostBack)
            {
                ctmDLC_procedure.txtDLC.TextMode = TextBoxMode.MultiLine;
                PhysicianProcedureManager objPhysicianProcedureManager = new PhysicianProcedureManager();
                if (Request["EncounterID"] != null && Request["EncounterID"].Trim() != string.Empty)
                {
                    EncounterID = Convert.ToUInt32(Request["EncounterID"]);
                }
                else
                {
                    EncounterID = ClientSession.EncounterId;
                }
                hEncID.Value = EncounterID.ToString();

                //Gitlab #2729 - Load data from DB
                //Old Code
                //var lstOtherProcedure = GetFromXML(EncounterID);
                InHouseProcedureDTO lstOtherProcedure = ObjInHouse_Mgr.LoadInHouseProcedure(EncounterID, ClientSession.PhysicianId, ClientSession.HumanId, ClientSession.LegalOrg);

                var lstProcedureList = objPhysicianProcedureManager.GetProceduresUsingPhysicianIDAndLabID(ClientSession.PhysicianId, "OTHER PROCEDURE", 0,ClientSession.LegalOrg);
                objOtherProDTO = new InHouseProcedureDTO();

                objOtherProDTO.OtherProcedure = lstOtherProcedure.OtherProcedure;
                objOtherProDTO.lstprocedureList = lstProcedureList;

                //objOtherProDTO = ObjInHouse_Mgr.LoadInHouseProcedure(ClientSession.EncounterId, ClientSession.PhysicianId, ClientSession.HumanId);
                hdnManageFreqProcedureCount.Value = lstProcedureList.Count.ToString();
                LoadGrid(objOtherProDTO.OtherProcedure);
                LoadProcedure();

                ViewState["Procedure"] = objOtherProDTO;

                SecurityServiceUtility obj = new SecurityServiceUtility();
                obj.ApplyUserPermissions(this.Page);

                btnAdd.Enabled = false;
                btnAdd.Text = "Add";
                IsSaveOrUpdate.Value = "Add";
                pnlCheckedList.Attributes.Add("onscroll", "Scrolling();");
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "AutoSave", "EnableSaveDiagnosticOrder('false');", true);
            }

            ctmDLC_procedure.txtDLC.Attributes.Add("onChange", "CCTextChanged()");

            ctmDLC_procedure.txtDLC.Attributes.Add("OnKeyPress", "GetKeyPress()");

            pbProcedure.pbCustomPhrases.Style.Add(HtmlTextWriterStyle.Display, "block");
        }

        public void LoadProcedure()
        {
            chklstOtherProcedures.Items.Clear();

            for (int i = 0; i < objOtherProDTO.lstprocedureList.Count; i++)
            {
                ListItem lvItem = new ListItem();

                lvItem.Text = objOtherProDTO.lstprocedureList[i].Physician_Procedure_Code + "-" + objOtherProDTO.lstprocedureList[i].Procedure_Description;
                lvItem.Value = objOtherProDTO.lstprocedureList[i].Physician_Procedure_Code;
                lvItem.Attributes.Add("Title", lvItem.Text);
                chklstOtherProcedures.Items.Add(lvItem);
                chklstOtherProcedures.RepeatDirection = RepeatDirection.Vertical;
                chklstOtherProcedures.RepeatLayout = RepeatLayout.Flow;
            }
        }

        public void btnAdd_Click(object sender, EventArgs e)
        {
            RadWindow1.VisibleOnPageLoad = false;
            RadWindow2.VisibleOnPageLoad = false;

            IList<InHouseProcedure> lstSave = new List<InHouseProcedure>();
            IList<InHouseProcedure> UpdateList = new List<InHouseProcedure>();
            IList<InHouseProcedure> lstDelete = new List<InHouseProcedure>();
            InHouseProcedure objUpdate;

            IList<ListItem> lstChecked = chklstOtherProcedures.Items.Cast<ListItem>().Where(a => a.Selected).ToList<ListItem>();

            string sLocalTime = string.Empty;

            if (chklstOtherProcedures.Items.Cast<ListItem>().Where(a => a.Selected).ToList<ListItem>().Count > 0)
            {
                if (btnAdd.Text == "Add")
                {
                    for (int x = 0; x < lstChecked.Count; x++)
                    {
                        objOtherProc = new InHouseProcedure();

                        objOtherProc.Human_ID = ClientSession.HumanId;
                        objOtherProc.Encounter_ID = EncounterID;//ClientSession.EncounterId;
                        objOtherProc.Physician_ID = ClientSession.PhysicianId;
                        objOtherProc.Notes = ctmDLC_procedure.txtDLC.Text.Trim();
                        //objOtherProc.Internal_Property_File_Name = UIManager.DB_Filepath;
                        objOtherProc.Internal_Property_File_Name = string.Empty;
                        objOtherProc.Created_By = ClientSession.UserName;
                        string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                        objOtherProc.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        //Gitlab #2844 - Fill the Created_Date_and_Time and Modified_Date_and_Time properly
                        //objOtherProc.Modified_By = ClientSession.UserName;
                        //objOtherProc.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();

                        objOtherProc.Procedure_Code_Description = lstChecked[x].Text.ToString().Substring(lstChecked[x].Text.ToString().IndexOf('-') + 1).Trim();
                        objOtherProc.Procedure_Code = lstChecked[x].Value.ToString().Split('-')[0].Trim();

                        sLocalTime = UtilityManager.ConvertToLocal(objOtherProc.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");

                        lstSave.Add(objOtherProc);
                    }
                    objOtherProDTO = ObjInHouse_Mgr.InsertInHouseProcedure(lstSave, null, "", "", sLocalTime, ClientSession.LegalOrg);
                }
                else
                {
                    if (ViewState["lstUpdate"] != null)
                        lstUpdate = (IList<InHouseProcedure>)ViewState["lstUpdate"];
                    string FileName = string.Empty;
                    bool IsDelete = false;

                    if (UIManager.IsAnnotation)
                    {
                        if (lstUpdate[0].Internal_Property_File_Name != "")
                            FileName = lstUpdate[0].Internal_Property_File_Name;
                        else
                            FileName = string.Empty;
                            //FileName = UIManager.DB_Filepath;
                    }
                    else
                    {
                        if (lstUpdate[0].Internal_Property_File_Name != "")
                            FileName = lstUpdate[0].Internal_Property_File_Name;
                        else
                            IsDelete = true;
                    }

                    for (int x = 0; x < lstChecked.Count; x++)
                    {
                        if (lstUpdate.Any(A => A.Procedure_Code == lstChecked[x].Value.ToString().Split('-')[0].Trim()))
                        {
                            InHouseProcedure objSave = new InHouseProcedure();

                            objSave = lstUpdate.Where(A => A.Procedure_Code == lstChecked[x].Text.ToString().Split('-')[0].Trim()).ToList<InHouseProcedure>()[0];
                            objSave.Internal_Property_File_Name = FileName;
                            objSave.Notes = ctmDLC_procedure.txtDLC.Text.Trim();
                            objSave.Modified_By = ClientSession.UserName;
                            string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                            objSave.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            UpdateList.Add(objSave);
                        }
                        else
                        {
                            string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();

                            objUpdate = new InHouseProcedure();
                            objUpdate.Human_ID = ClientSession.HumanId;
                            objUpdate.Encounter_ID = EncounterID;//ClientSession.EncounterId;
                            objUpdate.Physician_ID = ClientSession.PhysicianId;
                            objUpdate.Notes = ctmDLC_procedure.txtDLC.Text.Trim();
                            objUpdate.File_Management_Index_ID = lstUpdate[0].File_Management_Index_ID;
                            objUpdate.In_House_Procedure_Group_ID = lstUpdate[0].In_House_Procedure_Group_ID;
                            objUpdate.Internal_Property_File_Name = FileName;
                            objUpdate.Created_By = ClientSession.UserName;
                            objUpdate.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            //Gitlab #2844 - Fill the Created_Date_and_Time and Modified_Date_and_Time properly
                            //objUpdate.Modified_By = ClientSession.UserName;
                            //objUpdate.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            objUpdate.Procedure_Code_Description = lstChecked[x].Text.ToString().Trim().Substring(lstChecked[x].Text.ToString().Trim().IndexOf('-') + 1);
                            objUpdate.Procedure_Code = lstChecked[x].Text.ToString().Split('-')[0].Trim();

                            lstSave.Add(objUpdate);
                        }
                    }

                    foreach (InHouseProcedure objDelete in lstUpdate)
                    {
                        if (UpdateList.Where(A => A.Id == objDelete.Id).Count() == 0)
                            lstDelete.Add(objDelete);
                    }

                    objOtherProDTO = ObjInHouse_Mgr.UpdateAndDeleteAndSaveInHouseProcedure(lstSave, UpdateList, lstDelete, IsDelete, "", sLocalTime, ClientSession.LegalOrg);

                    btnClearAll.Text = "Clear All";
                    btnClearAll.ToolTip = "Clear All";
                    System.Web.UI.HtmlControls.HtmlGenericControl text3 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                    text3.InnerText = "C";
                    System.Web.UI.HtmlControls.HtmlGenericControl text4 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                    text4.InnerText = "lear All";
                }

                LoadGrid(objOtherProDTO.OtherProcedure);

                ViewState["Procedure"] = objOtherProDTO;
                //UIManager.DB_Filepath = string.Empty;

                Fieldclear();
                btnAdd.Enabled = false;
                chklstOtherProcedures.Enabled = true;
                btnManageFrequentlyUsed.Enabled = true;
                if (ClientSession.EncounterId != 0)
                {
                    if (ClientSession.FillEncounterandWFObject != null)
                    {
                        if (ClientSession.FillEncounterandWFObject.EncRecord != null)
                        {

                            ClientSession.FillEncounterandWFObject.EncRecord.is_serviceprocedure_saved = "N";
                            IList<Encounter> lst = new List<Encounter>();
                            IList<Encounter> lsttemp = new List<Encounter>();
                            lst.Add(ClientSession.FillEncounterandWFObject.EncRecord);
                            EncounterManager obj = new EncounterManager();
                            obj.SaveUpdateDelete_DBAndXML_WithTransaction(ref lsttemp, ref lst, null, string.Empty, true, false, ClientSession.FillEncounterandWFObject.EncRecord.Id, string.Empty);
                            ClientSession.FillEncounterandWFObject.EncRecord = lst[0];
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully",
                    " EnableSaveDiagnosticOrder('false');Savedsuccessfully();RefreshNotification('InHouseProcedure');", true);


               
            }
            else
            {
                if (btnAdd.Text.ToUpper() == "UPDATE")
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty,
                   "Order_SaveUnsuccessful();DisplayErrorMessage('280018');top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty,
                  "Order_SaveUnsuccessful();DisplayErrorMessage('280002');top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
                    chklstOtherProcedures.Enabled = true;
                    btnManageFrequentlyUsed.Enabled = true;
                }
                txtProcedure.Focus();
                btnAdd.Enabled = true;
                return;
            }

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "top.window.document.getElementById('ctl00_Loading').style.display = 'none'; {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void grdProcedure_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem && !e.Item.IsInEditMode)
            {
                TableCell cell = (e.Item as GridDataItem)["Procedure"];
                cell.Text = cell.Text.Replace("\r\n", "<br />");

                TableCell celudi = (e.Item as GridDataItem)["UniqueDeviceIdentifier"];
                celudi.ToolTip = celudi.Text.Replace("&nbsp;", "");
            }
        }

        public void LoadGrid(IList<InHouseProcedure> lstOrderProcedures)
        {
            grdOtherProcedure.DataSource = null;
            grdOtherProcedure.DataBind();

            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Procedure", typeof(string)));
            dt.Columns.Add(new DataColumn("Notes", typeof(string)));
            dt.Columns.Add(new DataColumn("GroupKey", typeof(string)));
            dt.Columns.Add(new DataColumn("UniqueDeviceIdentifier", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));

            if (lstOrderProcedures != null)
            {
               var groupByCriteria = lstOrderProcedures.GroupBy(D => D.In_House_Procedure_Group_ID)
                                                       .Select(a => new
                                                       {
                                                           Notes = a.FirstOrDefault().Notes,
                                                           Procedure_Code = string.Join(",\r\n", a.Select(m => m.Procedure_Code + "-" + m.Procedure_Code_Description).ToArray()),
                                                           In_House_Procedure_Group_ID = a.FirstOrDefault().In_House_Procedure_Group_ID,
                                                           Is_Active = a.FirstOrDefault().Is_Active,
                                                           Device_Identifier_UDI = a.FirstOrDefault().Device_Identifier_UDI,
                                                           Device_Identifier_DI = a.FirstOrDefault().Device_Identifier_DI

                                                       }).ToList();
                if (chkActive.Checked == false)
                            groupByCriteria = groupByCriteria.Where(a => a.Is_Active == "Y" || a.Is_Active == "").ToList();

                for (int i = 0; i < groupByCriteria.Count; i++)
                {
                    dr = dt.NewRow();

                    dr["Procedure"] = groupByCriteria[i].Procedure_Code;
                    dr["Notes"] = groupByCriteria[i].Notes;
                    dr["GroupKey"] = groupByCriteria[i].In_House_Procedure_Group_ID;
                    dr["UniqueDeviceIdentifier"] = groupByCriteria[i].Device_Identifier_UDI + groupByCriteria[i].Device_Identifier_DI;
                    if (groupByCriteria[i].Is_Active.Trim() != null && groupByCriteria[i].Is_Active.ToUpper().Trim() == "N")
                        dr["Status"] = "Inactive";
                    else if (groupByCriteria[i].Is_Active.Trim() != null && groupByCriteria[i].Is_Active.ToUpper().Trim() == "Y")
                        dr["Status"] = "Active";
                    else
                        dr["Status"] = "";
                    dt.Rows.Add(dr);
                
                }

                grdOtherProcedure.DataSource = dt;
                grdOtherProcedure.DataBind();
              
                foreach (GridDataItem item in grdOtherProcedure.Items)
                {
                    IList<InHouseProcedure> lstImage = objOtherProDTO.OtherProcedure
                                             .Where(a => a.In_House_Procedure_Group_ID == Convert.ToUInt64(item["GroupKey"].Text))
                                             .ToList<InHouseProcedure>();

                    if (lstImage.Count > 0 && lstImage[0].File_Management_Index_ID == string.Empty)
                    {
                        TableCell selectCell = item["View"];
                        ImageButton gd = (ImageButton)selectCell.Controls[0];
                        gd.ImageUrl = "~/Resources/Down_Disabled.bmp";
                    }

                }
            }
        }

        protected void grdProcedure_ItemCommand(object sender, GridCommandEventArgs e)
        {
            chklstOtherProcedures.Enabled = true;
            btnManageFrequentlyUsed.Enabled = true;
            RadWindow1.VisibleOnPageLoad = false;
            RadWindow2.VisibleOnPageLoad = false;

            txtProcedure.Text = string.Empty;
            ctmDLC_procedure.txtDLC.Text = string.Empty;
           
            var groupKey = string.Empty;

            if (!string.IsNullOrEmpty(hdnSelectedIndex.Value))
            {
                groupKey = grdOtherProcedure.MasterTableView.Items[Convert.ToInt32(hdnSelectedIndex.Value)]["GroupKey"].Text;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();} {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

            ViewState["GroupKey"] = groupKey;

            hdngroupkey.Value = groupKey;
            if (ViewState["Procedure"] != null)
            {
                objOtherProDTO = (InHouseProcedureDTO)ViewState["Procedure"];
            }

            lstUpdate = objOtherProDTO.OtherProcedure.Where(a => a.In_House_Procedure_Group_ID == Convert.ToUInt64(groupKey)).ToList<InHouseProcedure>();

            foreach (InHouseProcedure obj in lstUpdate)
            {
                if (obj.File_Management_Index_ID != string.Empty)
                {
                    IList<string> lstFle = objOtherProDTO.lstFileManagementIndex.Where(a => a.Id == Convert.ToUInt64(obj.File_Management_Index_ID)).Select(b => b.File_Path).ToList();
                    if (lstFle.Count > 0)
                        lstUpdate[lstUpdate.IndexOf(obj)].Internal_Property_File_Name = lstFle[0];
                }
            }

            ViewState["lstUpdate"] = lstUpdate;

            if (e.CommandName == "PDelete")
            {
                hdnDelImmuniztionId.Value = groupKey;
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "CellSelected(" + hdnDelImmuniztionId.Value + ");", true);
            }
            else if (e.CommandName == "PEdit")
            {
              
                if (lstUpdate.Count > 0 && (lstUpdate[0].Device_Identifier_DI.Trim() == "" && lstUpdate[0].Device_Identifier_UDI.Trim() == ""))
                {
                foreach (ListItem li in chklstOtherProcedures.Items)
                {
                    li.Selected = false;
                }

                for (int i = 0; i < lstUpdate.Count; i++)
                {
                    if (chklstOtherProcedures.Items.FindByText(lstUpdate[i].Procedure_Code + "-" + lstUpdate[i].Procedure_Code_Description) != null)
                        chklstOtherProcedures.Items.FindByText(lstUpdate[i].Procedure_Code + "-" + lstUpdate[i].Procedure_Code_Description).Selected = true;
                }

                chklstOtherProcedures.Enabled = false;
                btnManageFrequentlyUsed.Enabled = false;

              
                    txtProcedure.Text = grdOtherProcedure.MasterTableView.Items[Convert.ToInt32(hdnSelectedIndex.Value)]["Procedure"].Text.Replace("<br />", "\r\n");
                    ctmDLC_procedure.txtDLC.Text = grdOtherProcedure.MasterTableView.Items[Convert.ToInt32(hdnSelectedIndex.Value)]["Notes"].Text;

                    btnAdd.Text = "Update";
                    btnAdd.ToolTip = "Update";                   
                    btnAdd.AccessKey = "u";
                    btnClearAll.Text = "Cancel";
                    System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
                    text1.InnerText = "U";
                    System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdditionalword");
                    text2.InnerText = "pdate";
                    System.Web.UI.HtmlControls.HtmlGenericControl text3 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                    text3.InnerText = "C";
                    System.Web.UI.HtmlControls.HtmlGenericControl text4 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                    text4.InnerText = "ancel";
                    btnClearAll.ToolTip = "Cancel";
                    btnAdd.Enabled = true;
                    IsSaveOrUpdate.Value = "Update";
                    hProcedure.Value = grdOtherProcedure.MasterTableView.Items[Convert.ToInt32(hdnSelectedIndex.Value)]["Procedure"].Text.Replace("<br />", "\r\n");
                    hNotes.Value = grdOtherProcedure.MasterTableView.Items[Convert.ToInt32(hdnSelectedIndex.Value)]["Notes"].Text;
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "AutoSave", "EnableSaveDiagnosticOrder('true');", true);
                }
                else
                {
                IsSaveOrUpdate.Value = "Update";
                hProcedure.Value= grdOtherProcedure.MasterTableView.Items[Convert.ToInt32(hdnSelectedIndex.Value)]["Procedure"].Text.Replace("<br />", "\r\n");
                hNotes.Value = grdOtherProcedure.MasterTableView.Items[Convert.ToInt32(hdnSelectedIndex.Value)]["Notes"].Text;
               
                foreach (ListItem li in chklstOtherProcedures.Items)
                {
                    li.Selected = false;
                }
                txtProcedure.Text = string.Empty;
                ctmDLC_procedure.txtDLC.Text = string.Empty;
                btnAdd.Text = "Add";
                btnAdd.ToolTip = "Add";
                btnAdd.AccessKey = "a";
                System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
                text1.InnerText = "A";
                System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdditionalword");
                text2.InnerText = "dd";
                btnAdd.Enabled = false;
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "AutoSave", "ImplantableScreenOpen('true');EnableSaveDiagnosticOrder('false');", true);
                }
            }
            else if (e.CommandName == "View")
            {
                if (lstUpdate[0].File_Management_Index_ID != string.Empty)
                {
                    IList<FileManagementIndex> lstFilemgnt = new List<FileManagementIndex>();
                    lstFilemgnt = objOtherProDTO.lstFileManagementIndex.Where(F => F.Id == Convert.ToUInt64(lstUpdate[0].File_Management_Index_ID)).ToList<FileManagementIndex>();
                    RadWindow2.VisibleOnPageLoad = true;
                    RadWindow2.Title = "View Problem Area";
                    RadWindow2.VisibleStatusbar = false;
                    RadWindow2.NavigateUrl = "frmViewProcedure.aspx?procedureType=" + "procedure" + "&lstFile=" + lstFilemgnt[0].File_Path;
                }
            }
            hdnSelectedIndex.Value = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        void Fieldclear()
        {
            foreach (ListItem li in chklstOtherProcedures.Items)
            {
                li.Selected = false;
            }
            txtProcedure.Text = string.Empty;
            ctmDLC_procedure.txtDLC.Text = string.Empty;
            btnAdd.Text = "Add";
            IsSaveOrUpdate.Value = "Add";
            btnAdd.ToolTip = "Add";
            btnAdd.AccessKey = "a";
            System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
            text1.InnerText = "A";
            System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdditionalword");
            text2.InnerText = "dd";
            hProcedure.Value = "";
            hNotes.Value = "";
        }

        #region Test Area
        /*
        protected void btnTestArea_Click(object sender, EventArgs e)
        {
            if (chklstOtherProcedures.Items.Cast<ListItem>().Where(a => a.Selected).ToList<ListItem>().Count > 0)
            {
                if (ViewState["Procedure"] == null)
                {
                    return;
                }
                objOtherProDTO = (InHouseProcedureDTO)ViewState["Procedure"];
                UIManager.DB_Filepath = string.Empty;
                btnAdd.Enabled = true;

                if (btnAdd.Text == "Add")
                {
                    RadWindow1.Title = "Problem Area";
                    //RadWindow1.NavigateUrl = "frmTestArea.aspx?procedureType=" + "Procedure" + "&lstFile=" + string.Empty + "&bSave=" + "true&FileCount=" + objOtherProDTO.File_Count + "";
                    RadWindow1.NavigateUrl = "frmTestArea.aspx?procedureType=" + "Procedure" + "&lstFile=" + string.Empty + "&bSave=" + "true&FileCount=0";
                    RadWindow1.VisibleOnPageLoad = true;
                    RadWindow1.OnClientBeforeClose = "BeforeCloseForTestArea";
                }

                else
                {
                    ulong GroupKey = 0;

                    if (ViewState["GroupKey"] != null)
                    {
                        GroupKey = Convert.ToUInt64(ViewState["GroupKey"]);
                    }

                    string lstFile = string.Join("~", objOtherProDTO.lstFileManagementIndex.Where(r => r.Order_ID == GroupKey).Select(a => a.File_Path).ToArray());
                    RadWindow1.VisibleOnPageLoad = true;
                    RadWindow1.Title = "Problem Area";
                    RadWindow1.Visible = true;
                    //RadWindow1.NavigateUrl = "frmTestArea.aspx?procedureType=" + "Procedure" + "&lstFile=" + lstFile + "&bSave=" + "false&FileCount=" + objOtherProDTO.File_Count + "";
                    RadWindow1.NavigateUrl = "frmTestArea.aspx?procedureType=" + "Procedure" + "&lstFile=" + lstFile + "&bSave=" + "false&FileCount=0";
                    RadWindow1.VisibleOnPageLoad = true;
                    RadWindow1.Visible = true;
                    RadWindow1.OnClientBeforeClose = "BeforeCloseForTestArea";
                }
            }
            else
            {
                txtProcedure.Text = string.Empty;
            }

            btnAdd.Enabled = true;

            RadWindow2.VisibleOnPageLoad = false;

        }
        */

        #endregion

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            if (ViewState["Procedure"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

            objOtherProDTO = (InHouseProcedureDTO)ViewState["Procedure"];

            lstUpdate = objOtherProDTO.OtherProcedure.Where(a => a.In_House_Procedure_Group_ID == Convert.ToUInt64(hdnDelImmuniztionId.Value)).ToList<InHouseProcedure>();
            ViewState["lstUpdate"] = lstUpdate;

            objOtherProDTO = ObjInHouse_Mgr.DeleteInHouseProcedureBYList(lstUpdate, "", "", ClientSession.LegalOrg);
            ViewState["Procedure"] = objOtherProDTO;
            if (lstUpdate.Count > 0)
            {
                if (lstUpdate[0].Encounter_ID != 0)
                {
                    if (ClientSession.FillEncounterandWFObject != null)
                    {
                        if (ClientSession.FillEncounterandWFObject.EncRecord != null)
                        {

                            ClientSession.FillEncounterandWFObject.EncRecord.is_serviceprocedure_saved = "N";
                            IList<Encounter> lst = new List<Encounter>();
                            IList<Encounter> lsttemp = new List<Encounter>();
                            lst.Add(ClientSession.FillEncounterandWFObject.EncRecord);
                            EncounterManager obj = new EncounterManager();
                            obj.SaveUpdateDelete_DBAndXML_WithTransaction(ref lsttemp, ref lst, null, string.Empty, true, false, ClientSession.FillEncounterandWFObject.EncRecord.Id, string.Empty);
                            ClientSession.FillEncounterandWFObject.EncRecord = lst[0];
                        }
                    }
                }
            }
            LoadGrid(objOtherProDTO.OtherProcedure);

            Fieldclear();
            txtProcedure.Focus();
            btnAdd.Enabled = false;

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();RefreshNotification('InHouseProcedure');}", true);
        }

        protected void btnRefreshProce_Click(object sender, EventArgs e)
        {
            if (ViewState["Procedure"] == null)
            {
                return;
            }

            objOtherProDTO = (InHouseProcedureDTO)ViewState["Procedure"];
            PhysicianProcedureManager obj = new PhysicianProcedureManager();
            objOtherProDTO.lstprocedureList = obj.GetProceduresUsingPhysicianIDAndLabID(ClientSession.PhysicianId, "OTHER PROCEDURE", 0, ClientSession.LegalOrg);
            Boolean bManageFreqCount = false;
            int iCount=0;
            if(hdnManageFreqProcedureCount.Value!=string.Empty)
                iCount=Convert.ToInt32(hdnManageFreqProcedureCount.Value);
            if (iCount < objOtherProDTO.lstprocedureList.Count)
                bManageFreqCount = true;
            LoadProcedure();
            ViewState["Procedure"] = objOtherProDTO;
            RadWindow3.VisibleOnPageLoad = false;
            RadWindow3.Visible = false;

            RadWindow1.Visible = false;
            RadWindow1.VisibleOnPageLoad = false;

            if(bManageFreqCount)
            {
                btnAdd.Enabled = true;
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "EnableSaveDiagnosticOrder('true');  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            RadWindow1.VisibleOnPageLoad = false;
            RadWindow1.Visible = false;

            btnAdd.Text = "Add";
            IsSaveOrUpdate.Value = "Add";
            btnAdd.ToolTip = "Add";
            btnAdd.AccessKey = "a";
            System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
            text1.InnerText = "A";
            System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdditionalword");
            text2.InnerText = "dd";
            btnClearAll.ToolTip = "Clear All";
            btnClearAll.Text = "Clear All";
            System.Web.UI.HtmlControls.HtmlGenericControl text3 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
            text3.InnerText = "C";
            System.Web.UI.HtmlControls.HtmlGenericControl text4 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
            text4.InnerText = "lear All";

            if (txtProcedure.Text == string.Empty)
            {
                //btnTestArea.Enabled = false;
                btnAdd.Enabled = false;
            }
            chklstOtherProcedures.ClearSelection();
            chklstOtherProcedures.Enabled = true;
            btnManageFrequentlyUsed.Enabled = true;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }


        protected void chklstOtherProcedures_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadWindow2.VisibleOnPageLoad = false;
            IList<string> lst = chklstOtherProcedures.Items.Cast<ListItem>().Where(a => a.Selected).Select(b => b.Text).ToList<string>();
            txtProcedure.Text = string.Join(",\r\n", lst.ToArray());
            btnAdd.Enabled = true;
            btnAdd.AccessKey = "a";
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "AutoSave", "EnableSaveDiagnosticOrder('true'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        //IList<InHouseProcedure> GetFromXML(ulong EncounterID)
        //{
        //    string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
        //    string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

        //    var _IsAvailable = File.Exists(strXmlFilePath);

        //    IList<InHouseProcedure> ilstInHouseProcedure = new List<InHouseProcedure>();

        //    if (_IsAvailable)
        //    {
        //        XmlDocument itemDoc = new XmlDocument();
        //        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
        //        XmlNodeList xmlTagName = null;
        //        //itemDoc.Load(XmlText);
        //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //        {
        //            itemDoc.Load(fs);

        //            XmlText.Close();
        //            if (itemDoc.GetElementsByTagName("InHouseProcedureList")[0] != null)
        //            {
        //                xmlTagName = itemDoc.GetElementsByTagName("InHouseProcedureList")[0].ChildNodes;
        //                IList<InHouseProcedure> ilst = new List<InHouseProcedure>();
        //                if (xmlTagName.Count > 0)
        //                {
        //                    for (int j = 0; j < xmlTagName.Count; j++)
        //                    {
        //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(InHouseProcedure));
        //                        InHouseProcedure objInHouseProcedure = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as InHouseProcedure;

        //                        IEnumerable<PropertyInfo> propInfo = null;

        //                        propInfo = from obji in ((InHouseProcedure)objInHouseProcedure).GetType().GetProperties() select obji;

        //                        //if (xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value == Convert.ToString(ClientSession.EncounterId))
        //                        if (xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value == Convert.ToString(EncounterID))
        //                        {
        //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
        //                            {
        //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
        //                                {
        //                                    foreach (PropertyInfo property in propInfo)
        //                                    {
        //                                        if (property.Name == nodevalue.Name)
        //                                        {
        //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
        //                                                property.SetValue(objInHouseProcedure, Convert.ToUInt64(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
        //                                                property.SetValue(objInHouseProcedure, Convert.ToString(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
        //                                                property.SetValue(objInHouseProcedure, Convert.ToDateTime(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
        //                                                property.SetValue(objInHouseProcedure, Convert.ToInt32(nodevalue.Value), null);
        //                                            else
        //                                                property.SetValue(objInHouseProcedure, nodevalue.Value, null);
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            ilst.Add(objInHouseProcedure);
        //                        }
        //                    }
        //                }
        //                ilstInHouseProcedure = ilst.OrderByDescending(a => a.Modified_Date_And_Time).ToList();
        //            }
        //            fs.Close();
        //            fs.Dispose();
        //        }
        //    }
        //    return ilstInHouseProcedure;
        //}
        protected void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "activeload", "{sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);
            IList<InHouseProcedure> ilstProcedure = new List<InHouseProcedure>();
            //Gitlab #2729 - Load data from DB
            //Old Code
            //ilstProcedure = GetFromXML(EncounterID);
            InHouseProcedureDTO lstOtherProcedure = ObjInHouse_Mgr.LoadInHouseProcedure(EncounterID, ClientSession.PhysicianId, ClientSession.HumanId, ClientSession.LegalOrg);
            ilstProcedure = lstOtherProcedure.OtherProcedure;

            if (chkActive.Checked == false)
                ilstProcedure = ilstProcedure.Where(a => a.Is_Active == "Y" || a.Is_Active == "").ToList<InHouseProcedure>();
            if (objOtherProDTO== null)
            {
                objOtherProDTO = new InHouseProcedureDTO();
                objOtherProDTO.OtherProcedure = ilstProcedure;
            }
            LoadGrid(objOtherProDTO.OtherProcedure);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "activeStop", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

       

        protected void btnInvisibleImplant_Click(object sender, EventArgs e)
        {           
            Fieldclear();
            btnAdd.Enabled = false;
            IList<InHouseProcedure> ilstProcedure = new List<InHouseProcedure>();
            //ilstProcedure = GetFromXML(EncounterID);
            //Gitlab #2729 - Load data from DB
            //Old Code
            //ilstProcedure = GetFromXML(EncounterID);
            InHouseProcedureDTO lstOtherProcedure = ObjInHouse_Mgr.LoadInHouseProcedure(EncounterID, ClientSession.PhysicianId, ClientSession.HumanId, ClientSession.LegalOrg);
            ilstProcedure = lstOtherProcedure.OtherProcedure;

            if (objOtherProDTO == null)
            {
                objOtherProDTO = new InHouseProcedureDTO();
                objOtherProDTO.OtherProcedure = ilstProcedure;
            }
            LoadGrid(objOtherProDTO.OtherProcedure);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "activeStop", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
    }
}
