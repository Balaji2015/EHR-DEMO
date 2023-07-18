using System.Web.Services;
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
using System.Xml;
using System.IO;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.UI;
using Acurus.Capella.Core.DTO;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Drawing;
using Acurus.Capella.UI.Extensions;
using Telerik.Web.UI;
using System.Text;



namespace Acurus.Capella.UI
{
    public partial class frmPatientList : System.Web.UI.Page
    {
        DataTable dtEncounter;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Page.ClientScript.GetPostBackEventReference(grdEncounters, "");
                //lblLogged.Text = "You are logged in as" + " " + ClientSession.UserName + "  " + "in" + "  " + ClientSession.FacilityName;
                var client = from c in ApplicationObject.ClientList where c.Legal_Org == ClientSession.LegalOrg select c;
                IList<Client> currentClientList = client.ToList<Client>();

                if (currentClientList.Count > 0)
                {
                    lblLogged.Text = currentClientList[0].Client_Name + " - " + ClientSession.UserName + "  " + "in" + "  " + ClientSession.FacilityName;
                }


                string[] ListofDays = new string[12]{"31", "28", "31", "30", "31", "30", "31", "31", "30", "31", "30", "31"};
        string[] ListofMonth = new string[12]{"JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"};



                if (DateTime.Now.Month.ToString() == "1")
                {
                    dtFromDOS.Text = "01" + "-" + ListofMonth[DateTime.Now.AddMonths(-2).Month] + "-" + DateTime.Now.AddYears(-1).Year.ToString();
                }
                else if (DateTime.Now.Month.ToString() == "2")
                {
                    dtFromDOS.Text = "01" + "-" + "JAN" + "-" + DateTime.Now.Year.ToString();
                }
                else
                {
                    dtFromDOS.Text = "01" + "-" + ListofMonth[DateTime.Now.AddMonths(-2).Month] + "-" + DateTime.Now.Year.ToString();
                }
                
                //if (DateTime.Now.Day < 10)
                //    dtToDOS.Text = "0" + DateTime.Now.Day.ToString() + "-" + ListofMonth[DateTime.Now.AddMonths(-1).Month] + "-" + DateTime.Now.Year.ToString();
                //else
                //    dtToDOS.Text = DateTime.Now.Day.ToString() + "-" + ListofMonth[DateTime.Now.AddMonths(-1).Month] + "-" + DateTime.Now.Year.ToString(); CreateEmptyGrid();


                if (DateTime.Now.Day < 10)
                {
                    if (DateTime.Now.Month.ToString() == "1")
                    {
                        dtToDOS.Text = "0" + DateTime.Now.Day.ToString() + "-" + "JAN" + "-" + DateTime.Now.Year.ToString();
                    }
                    else
                    {
                        dtToDOS.Text = "0" + DateTime.Now.Day.ToString() + "-" + ListofMonth[DateTime.Now.AddMonths(-1).Month] + "-" + DateTime.Now.Year.ToString();
                    }
                }
                else
                {
                    if (DateTime.Now.Month.ToString() == "1")
                    {
                        dtToDOS.Text = DateTime.Now.Day.ToString() + "-" + "JAN" + "-" + DateTime.Now.Year.ToString();
                        CreateEmptyGrid();
                    }
                    else
                    {
                        dtToDOS.Text = DateTime.Now.Day.ToString() + "-" + ListofMonth[DateTime.Now.AddMonths(-1).Month] + "-" + DateTime.Now.Year.ToString(); CreateEmptyGrid();
                    }

                }
                lblNoofResults.Text = "No record(s) found.";

                ListItem lst = new ListItem();
                UserLookupManager userLookupMngr = new UserLookupManager();
                IList<UserLookup> ilstUserLookup = userLookupMngr.GetFieldLookupList(ClientSession.UserName, "PATIENT LIST PAYOR");


                //if (ilstUserLookup.Count == 0)
                if (ilstUserLookup.Count > 0 && ilstUserLookup.Count == 1 && ilstUserLookup[0].Value.ToUpper() == "ALL")
                {
                    CarrierManager carrierMngr = new CarrierManager();
                    IList<Carrier> ilstCarrier = carrierMngr.GetAll();

                    for (int iCount = 0; iCount < ilstCarrier.Count; iCount++)
                    {
                        lst = new ListItem();
                        lst.Text = ilstCarrier[iCount].Carrier_Name;
                        lst.Value = ilstCarrier[iCount].Id.ToString();
                        ddlPayerName.Items.Add(lst);
                    }
                }
                else if (ilstUserLookup.Count > 0)
                {
                    CarrierManager carrierMngr = new CarrierManager();
                    IList<Carrier> ilstCarrier = carrierMngr.GetAll();

                    for (int iCount = 0; iCount < ilstUserLookup.Count; iCount++)
                    {
                        var carrier = from c in ilstCarrier where c.Id == Convert.ToUInt64(ilstUserLookup[iCount].Value) select c;
                        IList<Carrier> lstcarrier = carrier.ToList<Carrier>();

                        if (lstcarrier.Count > 0)
                        {
                            lst = new ListItem();
                            lst.Text = lstcarrier[0].Carrier_Name;
                            lst.Value = lstcarrier[0].Id.ToString();
                            ddlPayerName.Items.Add(lst);
                        }
                    }
                }
                if (ddlPayerName.Text != "")
                {
                    SelectPlan();
                }
            }
        }

        public void loadGrid(DataTable dtEncounter)
        {

            for (int iCount = 0; iCount < dtEncounter.Rows.Count; iCount++)
            {
                DateTime dtTime = Convert.ToDateTime(dtEncounter.Rows[iCount]["DOS"].ToString());
                string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("yyyy-MM-dd hh:mm tt"); //dd-MMM-yyyy hh:mm tt");
                dtEncounter.Rows[iCount]["DOS"] = strDate;
            }
            grdEncounters.DataSource = dtEncounter;
            grdEncounters.DataBind();

            if (dtEncounter.Rows.Count == 0)
            {
                lblNoofResults.Text = "No record(s) found.";
                CreateEmptyGrid();
            }
            else
            {
                lblNoofResults.Text = dtEncounter.Rows.Count.ToString() + " record(s) found.";

                //GridSortExpression sort = new GridSortExpression();
                //sort.FieldName = "Date of Service";
                //sort.SortOrder = GridSortOrder.Ascending;
                //grdEncounters.MasterTableView.SortExpressions.AddSortExpression(sort);
                //grdEncounters.MasterTableView.Rebind();
            }
        }

        private void CreateEmptyGrid()
        {
            DataTable dtHuman = new DataTable();
            dtHuman.Columns.Add("Patient Name", typeof(string));
            dtHuman.Columns.Add("DOB", typeof(string));
            dtHuman.Columns.Add("Pt. Acc. #", typeof(string));
            dtHuman.Columns.Add("Member ID", typeof(string));
            dtHuman.Columns.Add("DOS", typeof(DateTime));
            dtHuman.Columns.Add("Provider Name", typeof(string));
            dtHuman.Columns.Add("Pri. Carrier", typeof(string));
            dtHuman.Columns.Add("Pri. Plan", typeof(string));
            dtHuman.Columns.Add("Type of Visit", typeof(string));
            dtHuman.Columns.Add("Facility", typeof(string));
            dtHuman.Columns.Add("Encounter ID", typeof(string));
            DataRow dr = dtHuman.NewRow();
            dtHuman.Rows.Add(dr);
            grdEncounters.DataSource = dtHuman;
            grdEncounters.DataBind();
            GridDataItem item = (GridDataItem)grdEncounters.MasterTableView.Items[0];
            item["View"].Visible = false;

            dtEncounter = new DataTable();

            Session["PatientListGrid"] = null;
        }

        protected void btnlogout_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.top.location.href=\" frmLogin.aspx\"; </script>");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            //if (dtpPatientDOB.DateInput.SelectedDate == null)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('103012'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    dtToDOS.Focus();
            //    return;
            //}
            if (dtFromDOS.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('103010'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                dtFromDOS.Focus();
                lblNoofResults.Text = "No record(s) found.";
                CreateEmptyGrid();
                return;
            }
            if (dtToDOS.Text== string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('103011'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                dtToDOS.Focus();
                lblNoofResults.Text = "No record(s) found.";
                CreateEmptyGrid();
                return;
            }
            if (Convert.ToDateTime(dtFromDOS.Text) > Convert.ToDateTime(dtToDOS.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('103005'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                dtFromDOS.Focus();
                lblNoofResults.Text = "No record(s) found.";
                CreateEmptyGrid();
                return;
            }
            if (Convert.ToDateTime(dtFromDOS.Text) > DateTime.Now)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('103008'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                dtFromDOS.Focus();
                lblNoofResults.Text = "No record(s) found.";
                CreateEmptyGrid();
                return;
            }
            if (Convert.ToDateTime(dtToDOS.Text) > DateTime.Now)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('103009'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                dtToDOS.Focus();
                lblNoofResults.Text = "No record(s) found.";
                CreateEmptyGrid();
                return;
            }
            if (dtpPatientDOB.Text!=string.Empty && Convert.ToDateTime(dtpPatientDOB.Text) > DateTime.Now)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('103013'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                dtpPatientDOB.Focus();
                lblNoofResults.Text = "No record(s) found.";
                CreateEmptyGrid();
                return;
            }


            EncounterManager encMngr = new EncounterManager();
            DataTable dtEncounter = null;
            string sPlanId = string.Empty;
            if (ddlPlan.SelectedItem.Text == "ALL")
            {
                for (int iCount = 0; iCount < ddlPlan.Items.Count; iCount++)
                {
                    if (ddlPlan.Items[iCount].Text != "ALL")
                    {
                        if (sPlanId == string.Empty)
                            sPlanId = "'" + ddlPlan.Items[iCount].Value + "'";
                        else
                            sPlanId += ",'" + ddlPlan.Items[iCount].Value + "'";
                    }

                }
            }
            else
            {
                if (ddlPlan.SelectedItem.Value.Trim() != "ALL")
                    sPlanId = "'" + ddlPlan.SelectedItem.Value + "'";
            }

            string dob = dtpPatientDOB.Text;
            if (dob == string.Empty)
                dob = DateTime.MinValue.ToString();
            if (txtPatientAccNo.Value == string.Empty)
                dtEncounter = encMngr.GetEncountersbyDOSRange(0, Convert.ToDateTime(dob), txtMemberId.Value, Convert.ToUInt64(ddlPayerName.SelectedValue), Convert.ToDateTime(dtFromDOS.Text), Convert.ToDateTime(dtToDOS.Text), txtPatientLastName.Value, txtPatientFirstName.Value, sPlanId, ClientSession.LegalOrg);
            else
                dtEncounter = encMngr.GetEncountersbyDOSRange(Convert.ToUInt64(txtPatientAccNo.Value), Convert.ToDateTime(dob), txtMemberId.Value, Convert.ToUInt64(ddlPayerName.SelectedValue), Convert.ToDateTime(dtFromDOS.Text), Convert.ToDateTime(dtToDOS.Text), txtPatientLastName.Value, txtPatientFirstName.Value, sPlanId, ClientSession.LegalOrg);
            Session["PatientListGrid"] = dtEncounter;

            loadGrid(dtEncounter);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlPayerName.SelectedIndex = 0;
            ddlPlan.SelectedIndex = 0;
            txtPatientAccNo.Value = string.Empty;
            txtPatientFirstName.Value = string.Empty;
            txtPatientLastName.Value = string.Empty;
            txtMemberId.Value = string.Empty;
            string[] ListofDays = new string[12] { "31", "28", "31", "30", "31", "30", "31", "31", "30", "31", "30", "31" };
            string[] ListofMonth = new string[12] { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };



            dtFromDOS.Text = "01" + "-" + ListofMonth[DateTime.Now.AddMonths(-2).Month] + "-" + DateTime.Now.Year.ToString();
            if(DateTime.Now.Day<10)
            dtToDOS.Text = "0"+ DateTime.Now.Day.ToString() + "-" + ListofMonth[DateTime.Now.AddMonths(-1).Month] + "-" + DateTime.Now.Year.ToString();
            else
                dtToDOS.Text = DateTime.Now.Day.ToString() + "-" + ListofMonth[DateTime.Now.AddMonths(-1).Month] + "-" + DateTime.Now.Year.ToString();
            dtpPatientDOB.Text = string.Empty;
            CreateEmptyGrid();
            lblNoofResults.Text = "No record(s) found.";
        }

        protected void grdEncounters_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "ViewSummary")
            {
                hdnEncID.Value = grdEncounters.MasterTableView.Items[0].Cells[9].Text;

                GridDataItem GridSelectItem = (GridDataItem)e.Item;
                if (GridSelectItem["PatientAcc"].Text != null && GridSelectItem["PatientAcc"].Text != string.Empty)
                {
                    ClientSession.HumanId = Convert.ToUInt64(GridSelectItem["PatientAcc"].Text.ToString());
                }
                ClientSession.EncounterId = Convert.ToUInt64(GridSelectItem["EncID"].Text);

                PatListModalWindow.Visible = true;
                PatListModalWindow.VisibleOnPageLoad = true;
                PatListModalWindow.Width = 1018;
                PatListModalWindow.Height = 600;
                PatListModalWindow.VisibleStatusbar = false;
                PatListModalWindow.OnClientBeforeClose = "CloseResultPage";
                PatListModalWindow.OnClientClose = "RefreshCloseResultPage";
                //PatListModalWindow.NavigateUrl = "frmSummaryNew.aspx?IsPatientList=Y" + "&EncounterID=" + ClientSession.EncounterId.ToString() + "&HumanID=" + ClientSession.HumanId.ToString();
                PatListModalWindow.NavigateUrl = "frmSummaryNew.aspx?IsPatientList=Y" + "&EncounterID=" + ClientSession.EncounterId.ToString() + "&HumanID=" + ClientSession.HumanId.ToString()+"&TabMode=true";

            }
        }

        protected void ddlPayerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectPlan();
        }

        void SelectPlan()
        {
            ddlPlan.Items.Clear();
            if (ddlPayerName.SelectedItem.Text != null)
            {
                ListItem lst = new ListItem();
                UserLookupManager userLookupMngr = new UserLookupManager();
                IList<UserLookup> ilstUserLookup = userLookupMngr.GetFieldLookupListByFieldNameandDescription(ClientSession.UserName, "PLAN LIST", ddlPayerName.SelectedItem.Value);
                //CAP-362 : Add All option for primary plan
                ddlPlan.Items.Add("ALL");

                if (ilstUserLookup.Count > 0 && ilstUserLookup.Count == 1 && ilstUserLookup[0].Value.ToUpper() == "ALL")
                {
                    //Load from insurance plan table based on selected payer carrier id
                    InsurancePlanManager insMngr = new InsurancePlanManager();
                    if (ddlPayerName.SelectedItem.Value.Trim() != "")
                    {
                        IList<InsurancePlan> ilsIns = insMngr.GetInsurancebyCarrierID(Convert.ToUInt64(ddlPayerName.SelectedItem.Value));
                        for (int iCount = 0; iCount < ilsIns.Count; iCount++)
                        {
                                lst = new ListItem();
                                lst.Text = ilsIns[iCount].Ins_Plan_Name;
                                lst.Value = ilsIns[iCount].Id.ToString();
                                ddlPlan.Items.Add(lst);
                        }
                    }
                }
                else if (ilstUserLookup.Count > 0)
                {
                    InsurancePlanManager insMngr = new InsurancePlanManager();

                    //Jira #CAP-516
                    //IList<InsurancePlan> ilstInsPlan = insMngr.GetAll();
                    IList<InsurancePlan> ilstInsPlan = insMngr.GetInsurancebyIDAll();

                    for (int iCount = 0; iCount < ilstUserLookup.Count; iCount++)
                    {
                        var insplan = from c in ilstInsPlan where c.Id == Convert.ToUInt64(ilstUserLookup[iCount].Value) select c;
                        IList<InsurancePlan> lstInsPlan = insplan.ToList<InsurancePlan>();

                        if (lstInsPlan.Count > 0)
                        {
                            lst = new ListItem();
                            lst.Text = lstInsPlan[0].Ins_Plan_Name;
                            lst.Value = lstInsPlan[0].Id.ToString();
                            ddlPlan.Items.Add(lst);
                        }
                    }
                }
            }
        }

        protected void grdEncounters_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            dtEncounter = (DataTable)Session["PatientListGrid"];

            //for (int iCount = 0; iCount < dtEncounter.Rows.Count; iCount++)
            //{
            //    DateTime dtTime = Convert.ToDateTime(dtEncounter.Rows[iCount]["DOS"].ToString());
            //    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("yyyy-MM-dd hh:mm tt"); //"dd-MMM-yyyy hh:mm tt");
            //    dtEncounter.Rows[iCount]["DOS"] = strDate;
            //}
            grdEncounters.DataSource = dtEncounter;
        }

        protected void btnExporttoExcel_Click(object sender, EventArgs e)
        {
            DataSet dsreport = new DataSet();

            System.Data.DataTable dtNew = new System.Data.DataTable();

            if (Session["PatientListGrid"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('750003'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

            dtNew = ((DataTable)Session["PatientListGrid"]).Copy();

            dsreport.Tables.Add(dtNew);
            
            DataView dv = new DataView(dsreport.Tables[0]);
           // string filename = "Wellness_PatientList_" + UtilityManager.ConvertToLocal(Convert.ToDateTime(hdnLocalTime.Value)).ToString("yyyyMMdd hh mm ss tt") + ".xls";
            string filename = "Wellness_PatientList_" + DateTime.Now.ToString("yyyyMMdd hh mm ss tt") + ".xls";
            if (dsreport.Tables[0].Rows.Count > 0)
            {
                Response.Charset = "UTF-8";
                Response.ContentType = "application/x-msexcel";
                Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                StringBuilder sResponseMessage = new StringBuilder();
                sResponseMessage.Append("<table  border=\"1\" width=\"50%\">");
                int col;
                sResponseMessage.Append("<tr align=\"center\" >");
                //if (cboOrderType.Text.ToUpper() == "PROCEDURES")
                //{
                sResponseMessage.Append("<td colspan=\"10\" align=\"center\"><strong>" + "WELLNESS PATIENT LIST" + "</strong></td>");
                //sResponseMessage.Append("<tr align=\"center\" >");
                //sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Payor : " + "</strong></td>");
                //sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + cboOrderType.Text);
                //sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Order Status  : " + "</strong></td>");
                //sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + cboOrderStatus.Text);
                //sResponseMessage.Append("<tr align=\"center\" >");
                //sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Arrival Date  : " + "</strong></td>");
                //if (chkDate.Checked == true)
                //{
                //    sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + dtpFromDate.SelectedDate.Value.ToString("dd - MMM - yyyy") + "  TO  " + dtpToDate.SelectedDate.Value.ToString("dd - MMM - yyyy"));
                //}
                //else
                //{
                //    sResponseMessage.Append("<td colspan=\"2\" align=\"left\">");
                //}
                //sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Facility Name  : " + "</strong></td>");
                //sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + cboFacilityName.Text);
                //if (txtPatientName.Text != string.Empty || txtProviderName.Text != string.Empty)
                //{
                //    sResponseMessage.Append("<tr align=\"center\" >");
                //    if (txtPatientName.Text != string.Empty)
                //    {
                //        sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Patient Name  : " + "</strong></td>");
                //        sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + txtPatientName.Text);
                //    }
                //    if (txtProviderName.Text != string.Empty)
                //    {
                //        if (txtPatientName.Text != string.Empty)
                //        {
                //            sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Provider  : " + "</strong></td>");
                //            sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + txtProviderName.Text);
                //        }
                //        else
                //        {
                //            sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Provider  : " + "</strong></td>");
                //            sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + txtProviderName.Text);
                //        }
                //    }
                //}
                //sResponseMessage.Append("<tr align=\"center\" >");
                //sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Lab/Imaging Center  : " + "</strong></td>");
                //sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + cboLabCenter.Text);
                //}

                sResponseMessage.Append("<tr align=\"center\" >");
                for (col = 0; col < dv.Table.Columns.Count - 1; col++)
                {
                    if (grdEncounters.Columns[col].Display == true)
                    {

                        sResponseMessage.Append("<td width=\"20%\"><strong>" + dv.Table.Columns[col].ColumnName + "</strong></td>");
                    }
                }
                sResponseMessage.Append("</tr>");
                foreach (DataRowView drv in dv)
                {

                    sResponseMessage.Append("<tr>");
                    for (col = 0; col < dv.Table.Columns.Count - 1; col++)
                    {
                        if (grdEncounters.Columns[col].Display == true)
                        {
                            sResponseMessage.Append("<td width='50%' align=\"Left\">" + drv[col].ToString() + "</td>");
                        }
                    }
                    sResponseMessage.Append("</tr>");
                }
                sResponseMessage.Append("</table>");
                Response.Write(sResponseMessage);
                Response.Flush();
                Response.End();
            }
        }
    }
}