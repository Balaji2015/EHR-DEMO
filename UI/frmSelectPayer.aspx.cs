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
using System.Collections.Generic;

using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;


namespace Acurus.Capella.UI
{
    public partial class frmSelectPayer : System.Web.UI.Page
    {
        //  SelectPayerProxy selectPayerProxy = new SelectPayerProxy();
        IList<SelectPayerDTO> objectList = new List<SelectPayerDTO>();
        //UtilityManager utilityMngr = new UtilityManager();
        ulong planId;
        string CarrierName = string.Empty;
        int PageNumber = 1;
        int MaxResultPerPage = 25;
        int TotalNoofDBRecords;
        string humanid = string.Empty;
        Double myPageNumber;
        int iMyLastPageNo;
        protected int pageCount = 0;
        int TotalCount = 0;
        InsurancePlanManager InsMngr = new InsurancePlanManager();

        //protected override void Render(HtmlTextWriter writer)
        //{
        //    //foreach (GridViewRow r in grdPayerInformation.Rows)
        //    //{
        //    //    if (r.RowType == DataControlRowType.DataRow)
        //    //    {
        //    //        Page.ClientScript.RegisterForEventValidation
        //    //                (r.UniqueID + "$ctl00");
        //    //        Page.ClientScript.RegisterForEventValidation
        //    //                (r.UniqueID + "$ctl01");
        //    //        Page.ClientScript.RegisterForEventValidation
        //    //               (r.UniqueID + "$ctl02");
        //    //    }
        //    //}

        //    base.Render(writer);
        //}
        protected void Page_Load(object sender, EventArgs e)
        {

            btnOk.Attributes.Add("onclick", "javascript:return closeWindowRadGrid();");

            if (!IsPostBack)
            {
                //ClientSession.FlushSession();
                grdPayerInformation.DataSource = new string[] { };

               
                lblError.Visible = false;
                btnOk.Enabled = false;
                txtCarrierName.Focus();
                btnFirst.Enabled = false;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                btnPrevious.Enabled = false;
                CreateGridHeader();
                if (Request["ScreenName"] != null)
                {
                    if (Request["CarrierName"] != null)
                    {
                        txtCarrierName.Text = Request["CarrierName"];
                        if (txtCarrierName.Text != string.Empty)
                        {
                            btnSearch_Click(sender, e);
                        }

                    }
                }

            }

            if (ClientSession.UserPermissionDTO != null && ClientSession.UserPermissionDTO.Userscntab != null)
            {
                var userQRCode = from u in ClientSession.UserPermissionDTO.Userscntab where u.scn_id == 6600 && u.user_name == ClientSession.UserName select u;
                if (userQRCode.ToList().Count > 0)
                {
                    btnPlanLibrary.Enabled = true;
                }
                else
                {
                    btnPlanLibrary.Enabled = false;
                }
            }

            }

        protected void btnOk_Click(object sender, EventArgs e)
        {

            if (grdPayerInformation.SelectedItems == null)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("420047", "Select Payer", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Select Payer", "DisplayErrorMessage('420047');", true);
                return;
            }

            IList<InsurancePlan> InsList = InsMngr.GetInsurancebyID(Convert.ToUInt64(grdPayerInformation.SelectedItems[0].Cells[2].Text));
            if (InsList != null)
                hdnCarrierID.Value = InsList[0].Carrier_ID.ToString();
            Response.Write("<script>window.close();</script>");
            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Select Payer", "Close();", true);
            //frmAddInsurancePolicies.MyPlanId = planId;
            //btnOk.Attributes.Add("onClick", "javascript:return closeWindow('" + grdPayerInformation.SelectedRow.Cells[1].Text+"','"+grdPayerInformation.SelectedRow.Cells[2].Text + "');");

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            grdPayerInformation.DataSource = null;
            grdPayerInformation.DataBind();
            CreateGridHeader();
            //grdPayerInformation.SelectedIndex = -1;
            btnOk.Enabled = false;
            string sZipcode = string.Empty;
            if (txtPlanID.Text.Trim() == string.Empty && txtCarrierName.Text.Trim() == string.Empty && txtZipCode.Text.Trim() == string.Empty && txtCityName.Text.Trim() == string.Empty && txtPlanName.Text.Trim() == string.Empty)
            {
                // ApplicationObject.erroHandler.DisplayErrorMessage("420037", "Select Payer", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Select Payer", "DisplayErrorMessage('420037');", true);
                PageNumber = 1;
                TotalNoofDBRecords = 0;
                txtCarrierName.Focus();
                return;
            }
            PageNumber = 1;
            this.FillPlanInfo();
            //objectList = selectPayerProxy.getInsurancePlanAndCarrierByPlanIDandCarrierNameDTO(txtPlanID.Text, txtCarrierName.Text + "%", PageNumber, 25, txtCityName.Text.Trim() + "%", sZipcode.Trim() + "%");
            //if (objectList.Count > 0 && objectList != null)
            //    TotalCount = objectList[0].InsuranceCount;
            //Session["TotalCount"] = TotalCount;
            //pageCount = Convert.ToInt32(GetTotalNoofDBRecords());
            //Session["PageCount"] = pageCount;
            //RefreshPageButtons();

            //GridFill(objectList);

            //if (objectList != null)
            //{
            //    if (objectList.Count > 0)
            //    {
            //        lblError.Text = objectList[0].InsuranceCount.ToString() + " Record(s) found.";
            //    }
            //    else
            //    {
            //        lblError.Text = "No Records found.";
            //    }
            //}
            //lblError.Visible = true;
        }
        //public void Fill()
        //{
        //    objectList = selectPayerProxy.getInsurancePlanAndCarrierByPlanIDandCarrierNameDTO(txtPlanID.Text, txtCarrierName.Text + "%", PageNumber, 25);
        //    if (objectList.Count > 0 && objectList != null)
        //        TotalCount = objectList[0].InsuranceCount;
        //    Session["TotalCount"] = TotalCount;
        //    pageCount = Convert.ToInt32(GetTotalNoofDBRecords());
        //    Session["PageCount"] = pageCount;
        //    GridFill(objectList);
        //}
        protected void btnClearAll_Click(object sender, EventArgs e)
        {

            lblError.Visible = false;
            txtPlanID.Text = string.Empty;
            txtCarrierName.Text = string.Empty;
            txtPlanName.Text = string.Empty;
            txtCityName.Text = string.Empty;
            txtZipCode.Text = string.Empty;
            grdPayerInformation.DataSource = null;
            grdPayerInformation.DataBind();
            txtCarrierName.Focus();
            PageNumber = 1;
            lblShowing.Text = string.Empty;
            btnFirst.Enabled = false;
            btnLast.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = false;
            TotalNoofDBRecords = 0;
            btnOk.Enabled = false;
            CreateGridHeader();

            //grdPayerInformation.SelectedIndex = -1;
        }

        public void GridFill(IList<SelectPayerDTO> InsList)
        {
            grdPayerInformation.DataSource = null;
            grdPayerInformation.DataBind();
            if (InsList != null && InsList.Count > 0)
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("Carrier ID", typeof(string));
                dt.Columns.Add("Carrier Name", typeof(string));
                dt.Columns.Add("Plan ID", typeof(string));
                dt.Columns.Add("Ins Plan Name", typeof(string));
                dt.Columns.Add("Financial Class", typeof(string));
                dt.Columns.Add("Address", typeof(string));
                dt.Columns.Add("City", typeof(string));
                dt.Columns.Add("State", typeof(string));
                dt.Columns.Add("Zip Code", typeof(string));
                dt.Columns.Add("Phone No", typeof(string));
                for (int i = 0; i < InsList.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Carrier ID"] = InsList[i].Carrier_ID.ToString();
                    dr["Carrier Name"] = InsList[i].Carrier_Name.ToString();
                    dr["Plan ID"] = InsList[i].Insurance_Plan_ID.ToString();
                    dr["Ins Plan Name"] = InsList[i].Ins_Plan_Name.ToString();
                    dr["Financial Class"] = InsList[i].Financial_Class_ID.ToString();
                    dr["Address"] = InsList[i].Payer_Addrress1.ToString();
                    dr["City"] = InsList[i].Payer_City.ToString();
                    dr["State"] = InsList[i].Payer_State.ToString();
                    dr["Zip Code"] = InsList[i].Payer_Zip.ToString();
                    if (InsList[i].Payer_Ph_No != "(   )    -")
                    {
                        dr["Phone No"] = InsList[i].Payer_Ph_No;
                    }
                    else
                    {
                        dr["Phone No"] = string.Empty;
                    }   
                    dt.Rows.Add(dr);
                }
                grdPayerInformation.DataSource = dt;
                grdPayerInformation.DataBind();
            }
            else
            {
                CreateGridHeader();
            }
        }

        protected void PageChangeEventHandler(object sender, CommandEventArgs e)
        {


            switch (e.CommandArgument.ToString())
            {
                case "First":

                    Session["PageNumber"] = 1;

                    break;
                case "Previous":

                    if (Convert.ToInt32(Session["PageNumber"]) > 1)
                    {
                        PageNumber = Convert.ToInt32(Session["PageNumber"]) - 1;
                        Session["PageNumber"] = PageNumber;
                    }

                    break;
                case "Next":
                    if (btnFirst.Enabled == false && btnPrevious.Enabled == false)
                    {
                        Session["PageNumber"] = 1;
                    }
                    if (Convert.ToInt32(Session["PageNumber"]) < Convert.ToInt32((hdnLastPageNo.Value)))
                    {
                        PageNumber = Convert.ToInt32(Session["PageNumber"]) + 1;
                        Session["PageNumber"] = PageNumber;
                    }

                    break;

                case "Last":

                    PageNumber = Convert.ToInt32(hdnLastPageNo.Value);
                    break;
            }
            Session["PageNumber"] = PageNumber;

            this.FillPlanInfo();

            RefreshPageButtons();
        }
        public double GetTotalNoofDBRecords()
        {


            if (TotalCount != 0)
            {
                myPageNumber = (double)(TotalCount) / (double)(MaxResultPerPage);
                iMyLastPageNo = Convert.ToInt32(Math.Ceiling(myPageNumber));
                // Session["iMyLastPageNo"] = iMyLastPageNo;
                hdnLastPageNo.Value = iMyLastPageNo.ToString();
            }
            LinkButtonLoad();
            return myPageNumber;
        }


        private void LinkButtonLoad()
        {
            int iStartPageNo = 0;
            int iEndPageNo = 0;

            if (TotalCount == 0)
            {
                iStartPageNo = 0;
            }
            else
            {
                if (hdnLastPageNo.Value != string.Empty)
                    iStartPageNo = ((Convert.ToInt32(hdnLastPageNo.Value) - 1) * MaxResultPerPage) + 1;
            }
            if (hdnLastPageNo.Value != string.Empty)
                iEndPageNo = (Convert.ToInt32((hdnLastPageNo.Value)) * MaxResultPerPage);


            if (iEndPageNo == 0)
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                return;
            }
            else
            {

            }

            if (PageNumber == 1)
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
            }
            else
            {
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
            }
            if (iEndPageNo >= TotalCount)
            {
                iEndPageNo = TotalCount;

                if (iStartPageNo == 0 && iEndPageNo != 0)
                {
                    iStartPageNo = 1;
                }


                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
            else
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }
        }
        private void RefreshPageButtons()
        {

            int iStartPageNo;
            int iEndPageNo;

            if (Convert.ToInt32(hdnTotalCount.Value) == 0)
            {
                iStartPageNo = 0;
            }
            else
            {
                iStartPageNo = ((PageNumber - 1) * MaxResultPerPage) + 1;
            }

            iEndPageNo = (PageNumber * MaxResultPerPage);
            lblShowing.Text = "Showing " + iStartPageNo.ToString() + " - " + (iEndPageNo).ToString() + " of " + TotalCount.ToString();

            if (iEndPageNo == 0)
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                return;
            }
            else
            {
                // lblShowing.Show();
            }

            if (PageNumber == 1)
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
            }
            else
            {
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
            }
            if (iEndPageNo >= Convert.ToInt32(hdnTotalCount.Value))
            {
                iEndPageNo = Convert.ToInt32(hdnTotalCount.Value);

                if (iStartPageNo == 0 && iEndPageNo != 0)
                {
                    iStartPageNo = 1;
                }

                lblShowing.Text = "Showing " + iStartPageNo.ToString() + " - " + (iEndPageNo).ToString() + " of " + TotalCount.ToString();
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
            else
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }
        }

        //protected void grdPayerInformation_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    hdnSelectedIndex.Value = grdPayerInformation.SelectedIndex.ToString();
        //    btnOk.Enabled = true;
        //}
        public void FillPlanInfo()
        {
            string sZipcode = string.Empty;
            if (txtZipCode.Text != "     -")
            {
                if (txtZipCode.Text.Length == 6 && txtZipCode.Text.Length < 10)
                {
                    string[] Split = Convert.ToString(txtZipCode.Text).Split('-');
                    if (Split.Length == 2 && Split[1] == string.Empty)
                    {
                        sZipcode = Split[0].ToString();
                    }
                }
                else
                {

                    sZipcode = txtZipCode.Text;
                }
            }
            else
            {
                sZipcode = string.Empty;
            }
            objectList = InsMngr.getInsurancePlanAndCarrierByPlanIDandCarrierNameDTO(txtPlanID.Text, txtCarrierName.Text + "%", PageNumber, 25, txtCityName.Text.Trim() + "%", sZipcode.Trim() + "%", txtPlanName.Text.Trim() + "%");
            if (objectList.Count == 0)
            {
                TotalNoofDBRecords = 0;
            }
            else
            {
                TotalNoofDBRecords = objectList[0].InsuranceCount;
            }
            GridFill(objectList);


            lblError.Text = TotalNoofDBRecords.ToString() + " Record(s) found.";
            if (objectList.Count > 0 && objectList != null)
                TotalCount = objectList[0].InsuranceCount;
            hdnTotalCount.Value = TotalCount.ToString();
            pageCount = Convert.ToInt32(GetTotalNoofDBRecords());
            Session["PageCount"] = pageCount;

            lblError.Visible = true;
            RefreshPageButtons();
        }

        //protected void grdPayerInformation_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.Header)
        //    {

        //        e.Row.Cells[0].Attributes.Add("style", "display:none");

        //    }
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Cells[0].Attributes.Add("style", "display:none");
        //        LinkButton _singleClickButton = (LinkButton)e.Row.Cells[10].Controls[0];

        //        string _jsSingle =
        //        ClientScript.GetPostBackClientHyperlink(_singleClickButton, "");

        //        _jsSingle = _jsSingle.Insert(11, "setTimeout(\"");
        //        _jsSingle += "\", 300)";

        //        e.Row.Attributes["onclick"] = _jsSingle;


        //        LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[11].Controls[0];
        //        string _jsDouble =
        //        ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
        //        e.Row.Attributes["ondblclick"] = _jsDouble;
        //    }

        //}

        //protected void grdPayerInformation_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    GridView _gridView = (GridView)sender;
        //    // Get the selected index and the command name
        //    int _selectedIndex = int.Parse(e.CommandArgument.ToString());
        //    string _commandName = e.CommandName;

        //    switch (_commandName)
        //    {
        //        case ("SingleClick"):
        //            _gridView.SelectedIndex = _selectedIndex;
        //            hdnSelectedIndex.Value = _selectedIndex.ToString();
        //            btnOk.Enabled = true;
        //            break;
        //        case ("DoubleClick"):
        //            _gridView.SelectedIndex = _selectedIndex;
        //            hdnSelectedIndex.Value = _selectedIndex.ToString();
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "closeWindow();", true);
        //            break;
        //    }

        //}
        public void CreateGridHeader()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Carrier ID", typeof(string));
            dt.Columns.Add("Carrier Name", typeof(string));
            dt.Columns.Add("Plan ID", typeof(string));
            dt.Columns.Add("Ins Plan Name", typeof(string));
            dt.Columns.Add("Financial Class", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("City", typeof(string));
            dt.Columns.Add("State", typeof(string));
            dt.Columns.Add("Zip Code", typeof(string));
            dt.Columns.Add("Phone No", typeof(string));
           // DataRow dr = dt.NewRow();
           // dt.Rows.Add(dr);
            grdPayerInformation.DataSource = dt;
            grdPayerInformation.DataBind();
            //grdPayerInformation.SelectedItems[0].Visible = false;
        }

    }
}
