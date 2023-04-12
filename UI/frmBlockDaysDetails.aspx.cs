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
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Telerik.Web.UI;
using System.IO;
using System.Xml;
using DocumentFormat.OpenXml.Presentation;

namespace Acurus.Capella.UI
{
    public partial class frmBlockDaysDetails : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        ulong PhyID = 0;
        ulong MecTechID = 0;
        string FacilityName = string.Empty;
        string[] sp;
        int TotalCount = 0;
        protected int pageIndex = 1;
        protected int pageCount = 0;
        protected int rowCount = 0;
        private const int CONST_PAGE_SIZE = 5;
        int PageNumber = 1;
        int MaxResultPerPage = 25;
        Double myPageNumber;
        int iMyLastPageNo;
       // string sAncillary = string.Empty;
         

        public double GetTotalNoofDBRecords()
        {
            if (TotalCount != 0)
            {
                myPageNumber = (double)(TotalCount) / (double)(MaxResultPerPage);
                iMyLastPageNo = Convert.ToInt32(Math.Ceiling(myPageNumber));
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
                {
                    iStartPageNo = ((Convert.ToInt32(hdnLastPageNo.Value) - 1) * MaxResultPerPage) + 1;
                }
            }
            if (hdnLastPageNo.Value != string.Empty)
            {
                iEndPageNo = ((Convert.ToInt32(hdnLastPageNo.Value)) * MaxResultPerPage);
            }

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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                this.Page.Title = "Block Days Details" + "-" + ClientSession.UserName;
                IList<FacilityLibrary> FacilityList = new List<FacilityLibrary>();
                hdnFacilityName.Value = Request.QueryString["Facility_Name"].Replace("_", "#");
                FacilityList = new List<FacilityLibrary>();
                ddlFacilityName.Items.Add("");
                //Gitlab #4133
                //FacilityList = ApplicationObject.facilityLibraryList;//Codereview FacilityMngr.GetFacilityList();
                var faclist = from f in ApplicationObject.facilityLibraryList where f.Legal_Org == ClientSession.LegalOrg select f;
                FacilityList = faclist.ToList<FacilityLibrary>();
                if (FacilityList != null)
                {
                    if (FacilityList.Count > 0)
                    {
                        for (int i = 0; i < FacilityList.Count; i++)
                        {
                            ListItem item = new ListItem();
                            item.Value = i.ToString();
                            item.Text = FacilityList[i].Fac_Name.ToString();
                            ddlFacilityName.Items.Add(item);
                            if (hdnFacilityName.Value == FacilityList[i].Fac_Name.ToString())
                            {
                                //ddlFacilityName.SelectedItem.Text = FacilityList[i].Fac_Name.ToString();
                                ddlFacilityName.SelectedIndex = i + 1;
                            }
                        }
                    }
                }
                if (Request.QueryString["hdnForMedicalAssistant"] == "MEDICAL ASSISTANT")
                {
                    ddlFacilityName.Enabled = false;
                    ddlProvider.Enabled = false;
                    btnCancel.Enabled = false;
                    btnEdit.Enabled = false;
                    btnGetBlockDetails.Enabled = false;
                    CreateDataTable();
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    grdBlockDays.DataSource = dt;
                    grdBlockDays.DataBind();
                    //grdBlockDays.Rows[0].CssClass = "displayNone";
                    grdBlockDays.Enabled = false;
                    chkShowAll.Enabled = false;
                }
                else
                {
                    //ddlFacilityName.Text = FacilityName;
                    string PhysicianName = string.Empty;
                    if (Request.QueryString["Physician_Name"] != null)
                    {
                        PhysicianName = Request.QueryString["Physician_Name"].ToString();
                        hdnPhysician.Value = PhysicianName;
                    }
                    ddlFacilityName_SelectedIndexChanged(sender, e);
                    if (ddlProvider.Items.Count > 0)
                    {
                        ddlProvider.Text = PhysicianName;
                    }
                    SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                    objSecurity.ApplyUserPermissions(this.Page);
                    btnEdit.Enabled = false;
                    if (ClientSession.UserRole.ToUpper() == "FRONT OFFICE")
                    {
                        ddlFacilityName.Enabled = false;
                        ddlProvider.Enabled = false;
                    }
                    CreateDataTable();
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    grdBlockDays.DataSource = dt;
                    grdBlockDays.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        if (grdBlockDays.MasterTableView.Items[0].Cells[2].Text != string.Empty)
                        {
                            grdBlockDays.DataSource = new string[] { };
                            grdBlockDays.DataBind();
                        }
                    }
                    //grdBlockDays.Rows[0].CssClass = "displayNone";
                }
            }
            //string[] Values;
           // if (ClientSession.BlockDays != null && ClientSession.BlockDays == "true")
            //if (hdnGetClick.Value != null && hdnGetClick.Value.ToUpper() == "TRUE")
            //{
            //    btnGetBlockDetails_Click(sender, e);
            //}
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int iIndex = 0;
            foreach (Telerik.Web.UI.GridDataItem dataItem in grdBlockDays.MasterTableView.Items)
            {
                if (dataItem.Selected == true)
                {
                    int index = dataItem.ItemIndex;
                    hdnSelectedIndex.Value = index.ToString();
                }
            }
            if (grdBlockDays.MasterTableView.Items.Count == 0)
            {
                return;
            }
            if (hdnSelectedIndex.Value != string.Empty)
            {
                iIndex = Convert.ToInt32(hdnSelectedIndex.Value);
            }
            if (hdnSelectedIndex.Value == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110202","Block Days Details", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "BlockDays", "DisplayErrorMessage('110202');", true);
                return;
            }
            else
            {
                hdnDaySelected.Value = grdBlockDays.Items[iIndex].Cells[16].Text;
                hdnFromDate.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[8].Text;
                hdnToDate.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[9].Text;
                hdnFromTime.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[10].Text;
                hdnToTime.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[11].Text;
                hdnIndex.Value = hdnSelectedIndex.Value;
                hdnDescription.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[7].Text;
                hdnBlockId.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[17].Text;
                hdnBlockType.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[6].Text;
                hdnPhysician.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[4].Text;
                hdnAlternateWeeks.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[19].Text;
                hdnAlternateMonths.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[20].Text;

            }
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "showVal", "EditBlockDays();", true);
        }
        protected void ddlFacilityName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ddlFacilityName.SelectedItem.Text select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
            FacilityName = ddlFacilityName.SelectedItem.Text;
            PhysicianList = new List<PhysicianLibrary>();
            //BlockDaysProxy.blockStaticList.Clear();
            PhysicianManager physicianMngr = new PhysicianManager();
            ddlProvider.Items.Clear();
            ddlProvider.Items.Add("");
           // if (sAncillary != string.Empty && sAncillary == ddlFacilityName.SelectedItem.Text.Trim())
            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
            {
                lblProvider.Text = "Machine - Technician*";
                PhysicianList = new List<PhysicianLibrary>();
                PhysicianList = UtilityManager.GetPhysicianList(ddlFacilityName.SelectedItem.Text.Trim(),ClientSession.LegalOrg);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc = new XmlDocument();
                xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
                for (int i = 0; i < PhysicianList.Count; i++)
                {
                    ListItem item = new ListItem();
                    if (File.Exists(strXmlFilePathTech) == true)
                    {
                        if (PhysicianList[i].PhyColor != "0")
                        {
                            XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + PhysicianList[i].PhyColor);
                            item.Text = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
                            item.Value = PhysicianList[i].PhyColor.ToString();//MachineTechnicianID
                        }
                        else
                        {
                            item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
                            item.Value = PhysicianList[i].Id.ToString();
                        }
                        ddlProvider.Items.Add(item);
                        if (Request.QueryString["Physician_Name"] != null && item.Text.Trim() == Request.QueryString["Physician_Name"].ToString().Trim())
                        {
                            ddlProvider.Items[i + 1].Selected = true;
                        }
                    }
                }
            }
            else
            {
                lblProvider.Text = "Provider*";
                PhysicianList = new List<PhysicianLibrary>();
                PhysicianList = physicianMngr.GetPhysicianListbyFacility(ddlFacilityName.SelectedItem.Text, "Y");
                if (PhysicianList != null)
                {
                    if (PhysicianList.Count > 0)
                    {
                        for (int i = 0; i < PhysicianList.Count; i++)
                        {
                            ListItem item = new ListItem();
                            //old code
                            //item.Value = PhysicianList[i].Id.ToString();
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
                            ddlProvider.Items.Add(item);
                            if (Request.QueryString["Physician_Name"] != null && item.Text == Request.QueryString["Physician_Name"].ToString())
                            {
                                ddlProvider.Items[i + 1].Selected = true;
                            }
                        }
                    }
                }
            }


        }
        protected void btnGetBlockDetails_Click(object sender, EventArgs e)
        {
            //ClientSession.BlockDays = "";
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ddlFacilityName.SelectedItem.Text select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();

            BlockdaysManager blockDaysMngr = new BlockdaysManager();
            FillBlockDays objFillBlkDay = new FillBlockDays();
            btnEdit.Enabled = false;
            if (ddlFacilityName.SelectedItem.Text == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110203", "Block Days Details", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "BlockDays", "DisplayErrorMessage('110203');", true);
                return;
            }
            if (ddlProvider.SelectedItem.Text == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110204", "Block Days Details", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "BlockDays", "DisplayErrorMessage('110204');", true);
                return;
            }
            DateTime dt;
            if (chkShowAll.Checked == true)
            {
                dt = DateTime.MinValue;
            }
            else
                dt = DateTime.Now;

            if (ddlProvider.SelectedItem.Text == string.Empty)
            {
                PhyID = 0;
            }
            else
            {
                for (int i = 0; i < ddlProvider.Items.Count; i++)
                {
                    if (ddlProvider.SelectedItem.Text == ddlProvider.Items[i].Text)
                    {
                        //Checking for null value added by Janani on 2/8/10
                        //if (sAncillary != string.Empty && sAncillary == ddlFacilityName.SelectedItem.Text.Trim() && ddlProvider.Items[i].Value != string.Empty)
                        if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y" && ddlProvider.Items[i].Value != string.Empty)
                        {
                            MecTechID = Convert.ToUInt64(ddlProvider.Items[i].Value);
                        }
                        else
                        {
                            PhyID = Convert.ToUInt64(ddlProvider.Items[i].Value);
                        }
                        break;
                    }
                }
            }

            //mpnBlockDays.PageNumber = 1;
            if (PhyID != 0)
                objFillBlkDay = blockDaysMngr.GetBlockDaysDetails(PhyID, ddlFacilityName.SelectedItem.Text, dt, PageNumber, 25);// mpnBlockDays.PageNumber, mpnBlockDays.MaxResultPerPage);
            else
            {
                objFillBlkDay = blockDaysMngr.GetBlockDaysDetailsUsingMechID(MecTechID, ddlFacilityName.SelectedItem.Text, dt, PageNumber, 25);
            }
            if (objFillBlkDay.BlockDays.Count > 0)
            {
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
                btnNext.Enabled = true;
                btnLast.Enabled = true;
            }
            else
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
            }
            FillBlockDaysDetails(objFillBlkDay.BlockDays);
            lblResult.Text = objFillBlkDay.BlockDaysCount.ToString() + " Result(s) Found";
            hdnTotalCount.Value = objFillBlkDay.BlockDaysCount.ToString();
            TotalCount = objFillBlkDay.BlockDaysCount;
            pageCount = Convert.ToInt32(GetTotalNoofDBRecords());
            Session["PageCount"] = pageCount;
            RefreshPageButtons();
        }
        protected void CreateDataTable()
        {
            dt.Columns.Add("Del", typeof(string));
            dt.Columns.Add("Facility Name", typeof(string));
            dt.Columns.Add("Provider Name", typeof(string));
            dt.Columns.Add("Block Day", typeof(string));
            dt.Columns.Add("Block Type", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("From Date", typeof(string));
            dt.Columns.Add("To Date", typeof(string));
            dt.Columns.Add("From Time", typeof(string));
            dt.Columns.Add("To Time", typeof(string));
            dt.Columns.Add("Block Id", typeof(string));
            dt.Columns.Add("Group Id", typeof(string));
            dt.Columns.Add("Facility Id", typeof(string));
            dt.Columns.Add("Block Date", typeof(string));
            dt.Columns.Add("Day Choosan", typeof(string));
            dt.Columns.Add("BlockDaysId", typeof(string));
            dt.Columns.Add("Physician Id", typeof(string));
            dt.Columns.Add("AlternateWeeks", typeof(string));
            dt.Columns.Add("AlternateMonths", typeof(string));
            grdBlockDays.DataSource = dt;
            grdBlockDays.DataBind();
            if (grdBlockDays.DataSource == null)
            {
                grdBlockDays.DataSource = new string[] { };
                grdBlockDays.DataBind();
            }
        }
        private void FillBlockDaysDetails(IList<Blockdays> blockList)
        {

            string time = string.Empty;
            grdBlockDays.DataSource = null;
            grdBlockDays.DataBind();
            //Checking for null value added by Janani on 2/8/10
            if (blockList != null)
            {
                if (blockList.Count == 0)
                {
                    btnEdit.Enabled = false;
                    grdBlockDays.DataSource = new string[] { };
                    grdBlockDays.DataBind();
                    return;
                }
                CreateDataTable();
                DataRow dr = null;
                foreach (Blockdays obj in blockList.OrderBy(a => a.From_Date_Choosen))
                {
                    dr = dt.NewRow();
                    for (int i = 0; i < ddlProvider.Items.Count; i++)
                    {
                        if (ddlProvider.Items[i].Value != null && ddlProvider.Items[i].Value != string.Empty)
                        {
                            if (obj.Physician_ID != 0)
                            {
                                if (obj.Physician_ID == Convert.ToUInt64(ddlProvider.Items[i].Value))
                                    dr["Provider Name"] = ddlProvider.Items[i].Text;

                            }
                            else
                            {
                                if (obj.Machine_Technician_Library_ID == Convert.ToUInt64(ddlProvider.Items[i].Value))
                                    dr["Provider Name"] = ddlProvider.Items[i].Text;
                            }
                        }
                    }
                    dr["Facility Name"] = obj.Facility_Name;
                    dr["Block Day"] = obj.Day_Choosen;
                    dr["Block Type"] = obj.Block_Type;
                    dr["Description"] = obj.Reason;
                    if (obj.From_Date_Choosen != DateTime.MinValue)
                        dr["From Date"] = obj.From_Date_Choosen.ToString("dd-MMM-yyyy");
                    if (obj.From_Date_Choosen != DateTime.MinValue)
                        dr["To Date"] = obj.To_Date_Choosen.ToString("dd-MMM-yyyy");
                    dr["From Time"] = obj.From_Time;
                    dr["To Time"] = obj.To_Time;
                    dr["Block Id"] = obj.Id;
                    dr["Group Id"] = obj.Blockdays_Group_ID;
                    dr["Facility Id"] = obj.Facility_Name;
                    dr["Block Date"] = obj.Block_Date;
                    dr["Day Choosan"] = obj.Day_Choosen;
                    dr["BlockDaysId"] = obj.Created_By;
                    dr["AlternateWeeks"] = obj.Is_Alternate_Weeks;
                    dr["AlternateMonths"] = obj.Is_Alternate_Months;
                    if (obj.To_Time != string.Empty)
                    {
                        sp = obj.To_Time.Split(':');
                        if (Convert.ToInt32(sp[0]) == 12)
                        {
                            time = "PM";
                        }
                        else if (Convert.ToInt32(sp[0]) > 12)
                        {
                            sp[0] = (Convert.ToInt32(sp[0]) - 12).ToString();
                            time = "PM";
                        }
                        else
                        {
                            time = "AM";
                        }
                        dr["To Time"] = sp[0] + ":" + sp[1] + " " + time;
                    }
                    else
                        dr["To Time"] = obj.To_Time;

                    if (obj.From_Time != string.Empty)
                    {
                        sp = obj.From_Time.Split(':');

                        if (Convert.ToInt32(sp[0]) == 12)
                        {
                            time = "PM";
                        }
                        else if (Convert.ToInt32(sp[0]) > 12)
                        {
                            sp[0] = (Convert.ToInt32(sp[0]) - 12).ToString();
                            time = "PM";
                        }
                        else
                        {
                            time = "AM";
                        }
                        dr["From Time"] = sp[0] + ":" + sp[1] + " " + time;
                    }
                    else
                        dr["From Time"] = obj.From_Time;

                    dr["Physician Id"] = obj.Physician_ID;


                    dt.Rows.Add(dr);
                }
                grdBlockDays.DataSource = dt;
                grdBlockDays.DataBind();
                //if (grdBlockDays.MasterTableView.Items.Count > 0)
                //{
                //    grdBlockDays.MasterTableView.Items[0].Selected = true;
                //    //grdBlockDays.MasterTableView.Items[0].Focus();
                //    //btnEdit.Enabled = true;
                //}
            }
        }
        //protected void grdBlockDays_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    BlockdaysManager blockDaysMngr = new BlockdaysManager();
        //    FillBlockDays objFillBlkDay = new FillBlockDays();
        //    if (e.CommandName == "Del")
        //    //if (e.CommandName == hdnTrue.Value)
        //    {
        //        GridViewRow gridRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
        //        ulong groupID = Convert.ToUInt64(grdBlockDays.Rows[gridRow.RowIndex].Cells[12].Text);

        //        DateTime dt;
        //        if (chkShowAll.Checked)
        //        {
        //            dt = DateTime.MinValue;
        //        }
        //        else
        //            dt = DateTime.Now;
        //        if (ddlProvider.SelectedItem != null)
        //        {
        //            PhyID = Convert.ToUInt64(ddlProvider.SelectedItem.Value);
        //        }
        //        else
        //        {
        //            PhyID = 0;
        //        }

        //        objFillBlkDay = blockDaysMngr.DeleteUsingGroupID(groupID, ClientSession.UserName, UtilityManager.ConvertToUniversal(), ApplicationObject.macAddress, PhyID, ddlFacilityName.SelectedItem.Text, dt, 1, 25);
        //        //FillBlockDaysDetails(objFillBlkDay.BlockDays);
        //        //mpnBlockDays.TotalNoofDBRecords = objFillBlkDay.BlockDaysCount;
        //        btnGetBlockDetails_Click(sender, e);
        //        lblResult.Text = objFillBlkDay.BlockDaysCount.ToString() + " Result(s) Found";
        //    }
        //    else if (e.CommandName == "Select")
        //    {
        //        GridView _gridView = (GridView)sender;
        //        int _selectedIndex = 0;
        //        // Get the selected index and the command name
        //        if (e.CommandArgument != string.Empty)
        //        {
        //            _selectedIndex = int.Parse(e.CommandArgument.ToString());
        //        }
        //        string _commandName = e.CommandName;
        //        switch (_commandName)
        //        {
        //            case ("SingleClick"):
        //                //Do what ever functions as to be performed in Single click
        //                hdnSelectedIndex.Value = _selectedIndex.ToString();
        //                btnEdit.Enabled = true;
        //                break;
        //        }
        //        btnEdit.Enabled = true;
        //        hdnSelectedIndex.Value = e.CommandArgument.ToString();
        //    }
        //    else
        //    {
        //        btnEdit.Enabled = false;
        //    }
        //}
        protected void grdBlockDays_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    e.Row.Cells[0].Attributes.Add("style", "display:none");//to hide the Select column Header
            //}
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    //For Single click functionality .cells[15] is the position of the Link button
            //    e.Row.Cells[0].Attributes.Add("style", "display:none");
            //    LinkButton _singleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            //    string _jsSingle =
            //    ClientScript.GetPostBackClientHyperlink(_singleClickButton, "");
            //    _jsSingle = _jsSingle.Insert(11, "setTimeout(\"");
            //    _jsSingle += "\", 300)";
            //    e.Row.Attributes["onclick"] = _jsSingle;
            //}
        }
        //protected override void Render(HtmlTextWriter writer)
        //{
        //    foreach (GridViewRow r in grdBlockDays.Rows)
        //    {
        //        if (r.RowType == DataControlRowType.DataRow)
        //        {
        //            Page.ClientScript.RegisterForEventValidation
        //            (r.UniqueID + "$ctl00");//Unique Id for Select Column
        //            Page.ClientScript.RegisterForEventValidation
        //            (r.UniqueID + "$ctl01");//Unique id for Single click Link Button
        //        }
        //    }
        //    base.Render(writer);
        //}
        protected void grdBlockDays_ItemCommand(object sender, GridCommandEventArgs e)
        {
            BlockdaysManager blockDaysMngr = new BlockdaysManager();
            FillBlockDays objFillBlkDay = new FillBlockDays();
            if (e.CommandName == "DeleteRow")
            {
                if (hdnIs_Single_Occurence.Value=="true")
                {
                    ulong blockID = 0;
                    ulong GroupID = 0;
                    blockID = Convert.ToUInt32(e.Item.Cells[12].Text);
                    GroupID = Convert.ToUInt32(e.Item.Cells[13].Text);
                    IList<Blockdays> ilst = blockDaysMngr.DeleteUsingBlockID(blockID, GroupID);
                    ddlFacilityName.Enabled = true;
                    ddlProvider.Enabled = true;
                    btnCancel.Enabled = true;
                    btnEdit.Text = "Edit All Occurrence";
                    btnAll.Visible = true;
                    btnGetBlockDetails.Visible = true;
                    chkShowAll.Visible = true;
                    btnFirst.Visible = true;
                    btnPrevious.Visible = true;
                    btnNext.Visible = true;
                    btnLast.Visible = true;
                    lblShowing.Visible = true;
                    lblResult.Visible = true;
                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "cancel", "CancelClick();", true);
                    btnGetBlockDetails_Click(sender, e);
                    hdnIs_Single_Occurence.Value = "false";
                }
                else
                {
                    ulong groupID = Convert.ToUInt32(e.Item.Cells[13].Text);
                    DateTime dt;
                    if (chkShowAll.Checked)
                    {
                        dt = DateTime.MinValue;
                    }
                    else
                    {
                        dt = DateTime.Now;
                    }
                    //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
                    //{
                    //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
                    //}

                   // if (sAncillary != string.Empty && sAncillary == ddlFacilityName.SelectedItem.Text.Trim())
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ddlFacilityName.SelectedItem.Text select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                    {
                        if (ddlProvider.SelectedItem != null)
                        {
                            MecTechID = Convert.ToUInt64(ddlProvider.SelectedItem.Value);
                        }
                        else
                        {
                            MecTechID = 0;
                        }
                        objFillBlkDay = blockDaysMngr.DeleteUsingGroupIDMechID(groupID, ClientSession.UserName, UtilityManager.ConvertToUniversal(), string.Empty, MecTechID, ddlFacilityName.SelectedItem.Text, dt, 1, 25);
                    }
                    else
                    {
                        if (ddlProvider.SelectedItem != null)
                        {
                            PhyID = Convert.ToUInt64(ddlProvider.SelectedItem.Value);
                        }
                        else
                        {
                            PhyID = 0;
                        }

                        objFillBlkDay = blockDaysMngr.DeleteUsingGroupID(groupID, ClientSession.UserName, UtilityManager.ConvertToUniversal(), string.Empty, PhyID, ddlFacilityName.SelectedItem.Text, dt, 1, 25);

                    }
                    btnGetBlockDetails_Click(sender, e);
                    lblResult.Text = objFillBlkDay.BlockDaysCount.ToString() + " Result(s) Found";
                }
            }
            else
            {
                //if (grdBlockDays.SelectedItems.Count > 0)
                //{
                //    btnEdit.Enabled = true;
                //}
                //else
                //{
                //    btnEdit.Enabled = false;
                //}

                hdnSelectedIndex.Value = e.Item.ItemIndex.ToString();
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
                    if (Session["PageNumber"] != null && hdnLastPageNo.Value.Trim() != string.Empty && Convert.ToInt32(Session["PageNumber"]) < Convert.ToInt32(hdnLastPageNo.Value))
                    {
                        PageNumber = Convert.ToInt32(Session["PageNumber"]) + 1;
                        Session["PageNumber"] = PageNumber;
                    }

                    break;

                case "Last":
                    if (hdnLastPageNo.Value != string.Empty)
                    {
                        PageNumber = Convert.ToInt32(hdnLastPageNo.Value);
                    }
                    break;

            }
            Session["PageNumber"] = PageNumber;

            this.btnGetBlockDetails_Click(sender, e);

            RefreshPageButtons();

        }
        private void RefreshPageButtons()
        {

            int iStartPageNo = 0;
            int iEndPageNo = 0;
            if (hdnLastPageNo.Value != string.Empty)
            {

                if (Convert.ToInt32(hdnLastPageNo.Value) == 0)
                {
                    iStartPageNo = 0;
                }
                else
                {
                    iStartPageNo = ((PageNumber - 1) * MaxResultPerPage) + 1;
                }
            }

            iEndPageNo = (PageNumber * MaxResultPerPage);
            lblShowing.Text = "Showing " + iStartPageNo.ToString() + " - " + (iEndPageNo).ToString() + " of " + hdnTotalCount.Value.ToString();


            if (iEndPageNo == 0)
            {

                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                return;
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
            if (iEndPageNo >= Convert.ToInt32(hdnTotalCount.Value) && iEndPageNo != null)
            {
                iEndPageNo = Convert.ToInt32(hdnTotalCount.Value);

                if (iStartPageNo == 0 && iEndPageNo != 0)
                {
                    iStartPageNo = 1;
                }
                if (hdnTotalCount.Value == "0")
                {
                    iStartPageNo = 0;
                }
                lblShowing.Text = "Showing " + iStartPageNo.ToString() + " - " + (iEndPageNo).ToString() + " of " + hdnTotalCount.Value.ToString();

                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
            else
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }
        }

        protected void grdBlockDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sFromDate = string.Empty;
            string sToDate = string.Empty;
            sFromDate = grdBlockDays.SelectedItems[0].Cells[8].Text;
            sToDate = grdBlockDays.SelectedItems[0].Cells[9].Text;
            if (sFromDate == sToDate)
            {
                btnEdit.Visible = true;
                btnEdit.Enabled = true;
                btnEdit.Text = "Edit Occurrence";
                btnAll.Visible = false;
                btnAll.Enabled = false;

            }
            else
            {
                btnEdit.Visible = true;
                btnEdit.Enabled = true;
                btnEdit.Text = "Edit All Occurrence";
                btnAll.Visible = true;
                btnAll.Enabled = true;
                int iIndex = 0;
                foreach (Telerik.Web.UI.GridDataItem dataItem in grdBlockDays.MasterTableView.Items)
                {
                    if (dataItem.Selected == true)
                    {
                        int index = dataItem.ItemIndex;
                        hdnSelectedIndex.Value = index.ToString();
                    }
                }
                if (grdBlockDays.MasterTableView.Items.Count == 0)
                {
                    return;
                }
                if (hdnSelectedIndex.Value != string.Empty)
                {
                    iIndex = Convert.ToInt32(hdnSelectedIndex.Value);
                }
                hdnDaySelected.Value = grdBlockDays.Items[iIndex].Cells[16].Text;
                hdnFromDate.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[8].Text;
                hdnToDate.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[9].Text;
                hdnFromTime.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[10].Text;
                hdnToTime.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[11].Text;
                hdnGroupID.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[13].Text;
                hdnIndex.Value = hdnSelectedIndex.Value;
                hdnDescription.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[7].Text;
                hdnBlockId.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[17].Text;
                hdnBlockType.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[6].Text;
                hdnPhysician.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[4].Text;
                hdnPhysicianID.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[18].Text;
                hdnUniqueBLockID.Value = grdBlockDays.MasterTableView.Items[iIndex].Cells[12].Text;
            }
        }

        protected void btnAll_Click(object sender, EventArgs e)
        {
            ddlFacilityName.Enabled = false;
            ddlProvider.Enabled = false;
            btnCancel.Enabled = true;
            btnEdit.Text = "Edit";
            btnAll.Visible = false;
            btnEdit.Enabled = false;
            btnGetBlockDetails.Visible = false;
            chkShowAll.Visible = false;
            btnFirst.Visible = false;
            btnPrevious.Visible = false;
            btnNext.Visible = false;
            btnLast.Visible = false;
            lblShowing.Visible = false;
            lblResult.Visible = false;
            IList<Blockdays> lstDispaly = new List<Blockdays>();
            BlockdaysManager objBlockdays = new BlockdaysManager();
            lstDispaly = objBlockdays.GetBlockDaysByGroupID(Convert.ToUInt32(hdnGroupID.Value));
            IList<Blockdays> newBlockDays = new List<Blockdays>();
            hdnIs_Single_Occurence.Value = "true";
            foreach (Blockdays obj in lstDispaly)
            {
                obj.To_Date_Choosen = obj.From_Date_Choosen;
                obj.Block_Type = "NON RECURSIVE";
                obj.Created_By = obj.Id.ToString();
                newBlockDays.Add(obj);
            }            
            FillBlockDaysDetails(newBlockDays);          
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Edit")
            {
                ddlFacilityName.Enabled = true;
                ddlProvider.Enabled = true;
                btnCancel.Enabled = true;
                btnAll.Visible = true;
                btnAll.Text = "Edit Single Occurrence";
                btnEdit.Text = "Edit All Occurrence";
                btnGetBlockDetails.Visible = true;
                chkShowAll.Visible = true;
                btnFirst.Visible = true;
                btnPrevious.Visible = true;
                btnNext.Visible = true;
                btnLast.Visible = true;
                lblShowing.Visible = true;
                lblResult.Visible = true;
                btnGetBlockDetails_Click(sender, e);
                btnAll.Enabled = true;
                btnEdit.Enabled = true;
            }
        }
    }
}
