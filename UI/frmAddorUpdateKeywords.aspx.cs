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
using Acurus.Capella.UI;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Drawing;

namespace Acurus.Capella.UI
{
    public partial class frmAddorUpdateKeywords : SessionExpired
    {
        #region Declaration

        string fieldname = string.Empty;
        IList<StaticLookup> fieldLookupList;
        UserLookupManager objUserLookupManager = new UserLookupManager();

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                btnAdd.Enabled = false;
                hdnEnableYesNo.Value = "false";
                fieldname = Request["FieldName"];
                if (fieldname == "DOCUMENTS TO PRINT")             
                    fieldname = "PATIENT EDUCATION DOCUMENTS";
                else if (fieldname == "CHIEF_COMPLAINTS")
                {
                    lblDesc.Visible = true;
                    txtDesc.Visible = true;
                    lblDesc.Text = "HPI";
                    lblDesc.ForeColor = Color.Black;
                    tddesc.Style.Add("height", "60px");
                   grdAddOrUpdateKeyword.Columns[4].Visible = true;
                }              
                Session["CurrentPhysicianID"] = Request["PhyID"];
                string userName = string.Empty;
                if (Request["userName"] != null)
                    userName = Request["userName"].ToString();
                IList<UserLookup> userLookup = objUserLookupManager.GetFieldLookupList(string.IsNullOrEmpty(userName) ? ClientSession.UserName : userName, fieldname, string.Empty);
                FillGrid(userLookup);
                lblKeyword.Text = fieldname.Replace("_"," ").ToString();
                grdAddOrUpdateKeyword.HeaderRow.Cells[3].Text = fieldname.Split('-')[0].ToString();
                txtKeyword.Attributes.Add("onkeypress", "btnAddEnabled()");
               
            }
        }
        private void Page_PreRender(object sender, System.EventArgs e)
        {
            grdAddOrUpdateKeyword.HeaderRow.Cells[3].Text = Request["FieldName"].Split('-')[0].ToString();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string YesNoCancel = hdnMessageType.Value;
            hdnMessageType.Value = string.Empty;
            if (txtKeyword.Text.Trim() == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Status", "DisplayErrorMessage('020002'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                txtKeyword.Focus();
                return;
            }
            fieldname = Request["FieldName"];
            if (fieldname == "DOCUMENTS TO PRINT")           
                fieldname = "PATIENT EDUCATION DOCUMENTS";           

            grdAddOrUpdateKeyword.HeaderRow.Cells[3].Text = fieldname.Split('-')[0].ToString();
            string userName = string.Empty;
            if (Request["userName"] != null)
                userName = Request["userName"].ToString();
            UserLookup objUserLook = new UserLookup();
            if (btnAdd.Text == "Add")
            {
                hdnMaxSortOrder.Value = objUserLookupManager.GetMaxValue(fieldname, string.IsNullOrEmpty(userName) ? ClientSession.UserName : userName);
                if (hdnMaxSortOrder.Value == "" || hdnMaxSortOrder.Value == string.Empty)
                    hdnMaxSortOrder.Value = "9999";  
                IList<UserLookup> SaveList = new List<UserLookup>();
                objUserLook.Field_Name = fieldname.ToUpper();
                objUserLook.Value = txtKeyword.Text.Trim();
                if (fieldname.ToString().Replace("*", String.Empty).Trim() == "CHIEF_COMPLAINTS")
                {
                    objUserLook.Description = txtDesc.Text.Trim();
                }
                objUserLook.User_Name = string.IsNullOrEmpty(userName) ? ClientSession.UserName : userName;
                objUserLook.Sort_Order = 0;
                if(fieldname.ToUpper()=="AMENDMENT NOTES")
                    objUserLook.Physician_ID = Convert.ToUInt32(ClientSession.CurrentPhysicianId);
                else
                    objUserLook.Physician_ID = Convert.ToUInt32(ClientSession.PhysicianId);
                objUserLook.Created_By = ClientSession.UserName;
                objUserLook.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                SaveList.Add(objUserLook);
                bool bDuplicateCheck = CheckDuplication();
                if (bDuplicateCheck.ToString().ToUpper() == "TRUE")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Status", "DisplayErrorMessage('020005'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    btnAdd.Enabled = true;
                    return;
                }
                if (objUserLook.Value != string.Empty && bDuplicateCheck.ToString().ToUpper() == "FALSE")
                {
                    objUserLookupManager.SaveUpdateDeleteWithTransaction(ref SaveList, null, null, string.Empty);
                    IList<UserLookup> userLookup = objUserLookupManager.GetFieldLookupList(string.IsNullOrEmpty(userName) ? ClientSession.UserName : userName, fieldname, string.Empty);
                    if (userLookup != null && userLookup.Count > 0) //modified by balaji.tj 2015-11-28
                    FillGrid(userLookup);
                    txtKeyword.Text = string.Empty;
                    txtDesc.Text = string.Empty;
                   if(YesNoCancel!="Yes")
                       ScriptManager.RegisterStartupScript(this, this.GetType(), "Status", "DisplayErrorMessage('020003');bSaveAddorUpdate(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);   
                   else
                       ScriptManager.RegisterStartupScript(this, this.GetType(), "Status", "DisplayErrorMessage('020003');YesNoUpdateSaveAddorUpdate(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);   
                }
                hdnEnableYesNo.Value = "false";
                btnAdd.Enabled = false;
            }
            else if (btnAdd.Text == "Update")
            {
                bool bDuplicateCheck = CheckDuplication();
                if (bDuplicateCheck.ToString().ToUpper() == "TRUE")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Status", "DisplayErrorMessage('020005'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    btnAdd.Enabled = true;
                    return;
                }
                else
                {
                    objUserLook = new UserLookup();
                    objUserLook = objUserLookupManager.GetFieldLookupList(string.IsNullOrEmpty(userName) ? ClientSession.UserName : userName, fieldname, string.Empty).Where(d => d.Id == Convert.ToUInt32(upadeID.Value)).FirstOrDefault();
                    IList<UserLookup> SaveList = new List<UserLookup>();
                    IList<UserLookup> UpdateList = new List<UserLookup>();
                    objUserLook.Value = txtKeyword.Text.Trim();
                    if (fieldname != "" && fieldname.ToString().Replace("*", String.Empty).Trim() == "CHIEF_COMPLAINTS") //modified by balaji.tj 2015-11-28                  
                        objUserLook.Description = txtDesc.Text.Trim();                   
                    objUserLook.Modified_By = ClientSession.UserName;
                    objUserLook.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    UpdateList.Add(objUserLook);
                    objUserLookupManager.SaveUpdateDeleteWithTransaction(ref SaveList, UpdateList, null, string.Empty);
                    IList<UserLookup> userLookup = objUserLookupManager.GetFieldLookupList(string.IsNullOrEmpty(userName) ? ClientSession.UserName : userName, fieldname, string.Empty);
                    FillGrid(userLookup);                   
                    ClearAll();
                    if(YesNoCancel!="Yes")
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Status", "DisplayErrorMessage('020004');bSaveAddorUpdate(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                     else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Status", "DisplayErrorMessage('020003');YesNoUpdateSaveAddorUpdate(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);   
                }
            }
        }

        protected void grdAddOrUpdateKeyword_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditC")
            {
                btnAdd.Enabled = true;
                hdnEnableYesNo.Value = "true";
               // int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = gvr.RowIndex;
                GridViewRow gridRow = grdAddOrUpdateKeyword.Rows[index];
                txtKeyword.Text = HttpUtility.HtmlDecode(grdAddOrUpdateKeyword.Rows[gridRow.RowIndex].Cells[3].Text.ToString()); 
                if (Request["FieldName"] != null && Request["FieldName"] != "" && Request["FieldName"].Split('-')[0].ToString().Replace("*", String.Empty).Trim() == "CHIEF_COMPLAINTS") //modified by balaji.tj 2015-11-28               
                    txtDesc.Text = HttpUtility.HtmlDecode(grdAddOrUpdateKeyword.Rows[gridRow.RowIndex].Cells[4].Text.Replace("&nbsp;", "").ToString());                
                upadeID.Value = grdAddOrUpdateKeyword.Rows[gridRow.RowIndex].Cells[5].Text;
                btnAdd.Text = "Update";
                btnClearAll.Text = "Cancel";
                fieldname = Request["FieldName"];
                if (fieldname != "") //Added by balaji.tj 2015-11-28
                grdAddOrUpdateKeyword.HeaderRow.Cells[3].Text = fieldname.Split('-')[0].ToString();
               
            }
            if (e.CommandName == "CopyC")
            {
                btnAdd.Enabled = true;
                hdnEnableYesNo.Value = "true";
                btnAdd.Text = "Add";
                btnClearAll.Text = "Clear All";
                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = gvr.RowIndex;
                GridViewRow gridRow = grdAddOrUpdateKeyword.Rows[index];
                txtKeyword.Text = grdAddOrUpdateKeyword.Rows[gridRow.RowIndex].Cells[3].Text; //modified by balaji.tj 2015-11-28
                if (Request["FieldName"] != null && Request["FieldName"] != "" && Request["FieldName"].Split('-')[0].ToString().Replace("*", String.Empty).Trim() == "CHIEF_COMPLAINTS")               
                    txtDesc.Text = grdAddOrUpdateKeyword.Rows[gridRow.RowIndex].Cells[4].Text.Replace("&nbsp;", "");               
                fieldname = Request["FieldName"];
                if (fieldname != "")  //Added by balaji.tj 2015-11-28
                grdAddOrUpdateKeyword.HeaderRow.Cells[3].Text = fieldname.Split('-')[0].ToString();
            }
            else if (e.CommandName == "Delc")
            {
               
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (hdmRowIndex.Value != null && hdmRowIndex.Value != "")
            {
                int index = Convert.ToInt32(hdmRowIndex.Value);
                GridViewRow gridRow = grdAddOrUpdateKeyword.Rows[index];

                ulong deleteId = 0;
                if (System.Text.RegularExpressions.Regex.IsMatch(grdAddOrUpdateKeyword.Rows[gridRow.RowIndex].Cells[5].Text, "^[0-9]*$") == true)
                {
                    deleteId = Convert.ToUInt32(grdAddOrUpdateKeyword.Rows[gridRow.RowIndex].Cells[5].Text);
                }
                UserLookup objUserLookup = new UserLookup();
                objUserLookup.Id = deleteId;
                IList<UserLookup> SaveList = new List<UserLookup>();

                IList<UserLookup> DeleteList = new List<UserLookup>();
                DeleteList.Add(objUserLookup);
                objUserLookupManager.SaveUpdateDeleteWithTransaction(ref SaveList, null, DeleteList, string.Empty);
                string userName = string.Empty;
                if (Request["userName"] != null)
                    userName = Request["userName"].ToString();
                fieldname = Request["FieldName"];
                if (fieldname != "" && fieldname == "DOCUMENTS TO PRINT")  //modified by balaji.tj 2015-11-28                   
                    fieldname = "PATIENT EDUCATION DOCUMENTS";
                IList<UserLookup> userLookup = objUserLookupManager.GetFieldLookupList(string.IsNullOrEmpty(userName) ? ClientSession.UserName : userName, fieldname, string.Empty);
                FillGrid(userLookup);
                ClearAll();
                fieldname = Request["FieldName"];
                if (fieldname != "")  //Added by balaji.tj 2015-11-28
                    grdAddOrUpdateKeyword.HeaderRow.Cells[3].Text = fieldname.Split('-')[0].ToString();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            fieldname = Request["FieldName"];
            if (fieldname != "")  //Added by balaji.tj 2015-11-28
                grdAddOrUpdateKeyword.HeaderRow.Cells[3].Text = fieldname.Split('-')[0].ToString();
            if (isClearAll.Value == "true")
                ClearAll();

        }
      

        protected void grdAddOrUpdateKeyword_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnDel = (ImageButton)e.Row.Cells[2].FindControl("btnDel");
                fieldname = Request["FieldName"];
                if (fieldname != "")  //Added by balaji.tj 2015-11-28
                grdAddOrUpdateKeyword.HeaderRow.Cells[3].Text = fieldname.Split('-')[0].ToString();
            }
        }

        #endregion

        #region Methods

        public void FillGrid(IList<UserLookup> UserLookupLst)
        {
            if (UserLookupLst.Count == 0)
            {
                DataTable dtAddUpdateKeyword = new DataTable();
                DataColumn dtCol = new DataColumn("Edit", typeof(LinkButton));
                dtAddUpdateKeyword.Columns.Add(dtCol);
                dtCol = new DataColumn("Del", typeof(LinkButton));
                dtAddUpdateKeyword.Columns.Add(dtCol);
                dtCol = new DataColumn("Keyword", typeof(string));
                dtAddUpdateKeyword.Columns.Add(dtCol);
                dtCol = new DataColumn("Description", typeof(string));
                dtAddUpdateKeyword.Columns.Add(dtCol);
                dtCol = new DataColumn("UserLookupID", typeof(string));
                dtAddUpdateKeyword.Columns.Add(dtCol);
                DataRow dr = dtAddUpdateKeyword.NewRow();
                dtAddUpdateKeyword.Rows.Add(dr);
                grdAddOrUpdateKeyword.DataSource = dtAddUpdateKeyword;
                grdAddOrUpdateKeyword.DataBind();
                grdAddOrUpdateKeyword.Columns[3].HeaderText = Request["FieldName"];
                grdAddOrUpdateKeyword.Rows[0].Cells[0].CssClass = "displayNone";
                grdAddOrUpdateKeyword.Rows[0].Cells[1].CssClass = "displayNone";
                grdAddOrUpdateKeyword.Rows[0].Cells[2].CssClass = "displayNone";
                grdAddOrUpdateKeyword.Rows[0].Cells[3].CssClass = "displayNone";
            }
            else
            {
                DataTable dtAddUpdateKeyword = new DataTable();
                DataColumn dtCol = new DataColumn("Edit", typeof(LinkButton));
                dtAddUpdateKeyword.Columns.Add(dtCol);
                dtCol = new DataColumn("Del", typeof(LinkButton));
                dtAddUpdateKeyword.Columns.Add(dtCol);
                dtCol = new DataColumn("Keyword", typeof(string));
                dtAddUpdateKeyword.Columns.Add(dtCol);
                dtCol = new DataColumn("Description", typeof(string));
                dtAddUpdateKeyword.Columns.Add(dtCol);
                dtCol = new DataColumn("UserLookupID", typeof(string));
                dtAddUpdateKeyword.Columns.Add(dtCol);
                for (int i = 0; i < UserLookupLst.Count; i++)
                {
                    DataRow dr = dtAddUpdateKeyword.NewRow();
                    dr["Keyword"] = UserLookupLst[i].Value;
                    dr["Description"] = UserLookupLst[i].Description;
                    dr["UserLookupID"] = UserLookupLst[i].Id;
                    dtAddUpdateKeyword.Rows.Add(dr);
                }
                grdAddOrUpdateKeyword.DataSource = dtAddUpdateKeyword;
                grdAddOrUpdateKeyword.DataBind();
                foreach (GridViewRow row in grdAddOrUpdateKeyword.Rows)
                {
                    ImageButton EditImgBtn = row.Cells[0].Controls[0] as ImageButton;
                    ImageButton CopyImgBtn = row.Cells[1].Controls[0] as ImageButton;
                    ImageButton DelImgBtn = row.Cells[2].Controls[0] as ImageButton;
                    if (EditImgBtn != null)                  
                        EditImgBtn.ToolTip = "Edit";                    
                    if (CopyImgBtn != null)                   
                        CopyImgBtn.ToolTip = "Copy";                   
                    if (DelImgBtn != null)                  
                        DelImgBtn.ToolTip = "Delete";                    
                }
                //grdAddOrUpdateKeyword.Columns[3].HeaderText = Request["FieldName"];
            }
            if (UserLookupLst.Count > 0)
                lbltotal.Text = Convert.ToString(UserLookupLst.Count) + " " + "Result(s) Found";
            else
                lbltotal.Text = UserLookupLst.Count + " " + "Result(s) Found";
        }

        //code comment by balaji.Tj 2015-11-30
        //private bool CheckDuplication(string NewValue)
        //{
        //    bool value = false;
        //    if (fieldLookupList != null)
        //    {
        //        for (int i = 0; i < fieldLookupList.Count; i++)
        //        {
        //            if (NewValue.ToLower() == fieldLookupList[i].Value.Trim().ToLower())
        //            {
        //                value = true;
        //                break;
        //            }
        //            else                  
        //                value = false;                   
        //        }
        //    }
        //    return value;
        //}

        private bool CheckDuplication()
        {            
            string userName = string.Empty;
            if (Request["userName"] != null)
                userName = Request["userName"].ToString();
            fieldname = Request["FieldName"];
            if (fieldname != "" && fieldname == "DOCUMENTS TO PRINT") //modified by balaji.tj 2015-11-28           
                fieldname = "PATIENT EDUCATION DOCUMENTS";           
            bool bchk = false;
            //int idescRepeat = 0;
            IList<UserLookup> userLookup = new List<UserLookup>();
            //IList<UserLookup> userLookup = objUserLookupManager.GetFieldLookupList(string.IsNullOrEmpty(userName) ? ClientSession.UserName : userName, fieldname, string.Empty);
            if (btnAdd.Text == "Add" || Request["FieldName"] != "" && Request["FieldName"].Split('-')[0].ToString().Replace("*", String.Empty).Trim() != "CHIEF_COMPLAINTS") //modified by balaji.tj 2015-11-28
            {
                #region
                //code comment by balaji.Tj 2015-11-30
                //if (userLookup!= null && userLookup.Count > 0) //modified by balaji.tj 2015-11-28
                //{
                //    for (int i = 0; i < userLookup.Count; i++)
                //    {
                //        string KeyValue = userLookup[i].Value.ToUpper().Trim();
                //        if (txtKeyword.Text.ToUpper().Trim() == KeyValue)                       
                //            bchk = true;                       
                //    }
                //}
                #endregion
                //code added by balaji.Tj 2015-11-30
                if (grdAddOrUpdateKeyword.Rows.Count > 0)
                {
                    for (int i = 0; i < grdAddOrUpdateKeyword.Rows.Count; i++)
                    {
                        if (upadeID.Value != grdAddOrUpdateKeyword.Rows[i].Cells[5].Text)
                        {
                            if (upadeID.Value != "0" || upadeID.Value != "")
                            {
                                if (txtKeyword.Text.ToUpper().Trim() == grdAddOrUpdateKeyword.Rows[i].Cells[3].Text.ToUpper().Trim())
                                {
                                    bchk = true;
                                    break;
                                }
                            }
                        }
                        else
                            bchk = false;
                    }
                }
            }
            else
            {
                #region
                ////code comment by balaji.Tj 2015-11-30
                //if (userLookup != null && userLookup.Count > 0) //modified by balaji.tj 2015-11-28
                //{
                //    for (int i = 0; i < userLookup.Count; i++)
                //    {
                //        string KeyValue = userLookup[i].Value.ToUpper().Trim();
                //        if (Request["FieldName"] != null && Request["FieldName"] != "" && Request["FieldName"].Split('-')[0].ToString() == "CHIEF_COMPLAINTS") //modified by balaji.tj 2015-11-28
                //        {
                //            if (txtKeyword.Text.ToUpper().Trim() == KeyValue)
                //            {
                //                bchk = true;
                //                idescRepeat++;
                //            }
                //        }
                //    }
                //    if (idescRepeat < 2)
                //        bchk = false;
                //}
                #endregion
                //code added by balaji.Tj 2015-11-30
                if (grdAddOrUpdateKeyword.Rows.Count > 0)
                {
                    for (int i = 0; i < grdAddOrUpdateKeyword.Rows.Count; i++)
                    {
                        if (Request["FieldName"] != null && Request["FieldName"] != "" && Request["FieldName"].Split('-')[0].ToString().Replace("*", String.Empty).Trim() == "CHIEF_COMPLAINTS") //modified by balaji.tj 2015-11-28
                        {                            
                            if (upadeID.Value != grdAddOrUpdateKeyword.Rows[i].Cells[5].Text)
                            {
                                if (upadeID.Value != "0" || upadeID.Value != "")
                                {
                                    if (txtKeyword.Text.ToUpper().Trim() == grdAddOrUpdateKeyword.Rows[i].Cells[3].Text.ToUpper().Trim())
                                    {
                                        bchk = true;
                                        break;
                                    }
                                }
                            }
                            else
                                bchk = false;
                        }                       
                    }                   
                }
            }
            return bchk;
        }

        private void ClearAll()
        {
            btnAdd.Enabled = false;
            hdnEnableYesNo.Value = "false";
            txtKeyword.Text = string.Empty;
            txtDesc.Text = string.Empty;
            btnAdd.Text = "Add";
            btnClearAll.Text = "Clear All";
            upadeID.Value = "0";
        }

        #endregion

        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}
