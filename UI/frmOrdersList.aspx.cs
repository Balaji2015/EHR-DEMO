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
using System.Runtime.Serialization;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Text;
using System.Drawing;
using System.IO;

using Telerik.Web.UI;
using Acurus.Capella.DataAccess.com.labcorp.www4;
using System.Diagnostics;
using Acurus.Capella.UI;
//using Acurus.Capella.LabAgent;

namespace Acurus.Capella.UI
{
    public partial class frmOrdersList : System.Web.UI.Page
    {
        PrintOrders print = new PrintOrders();
        OrdersManager objOrdersManager = new OrdersManager();
        FillHumanDTO humanRecord;
        string sHumanName = string.Empty;
        string sIdentificationNumber = string.Empty;
        string humanName = string.Empty;
        ulong OrderSubmitID = 0;
        string OrderType = "DIAGNOSTIC ORDER";
        public string Edited_Order_Submit_ID
        {
            get
            {
                return Edited_Order_Submit_ID;
            }
            set
            {
                Edited_Order_Submit_ID = value;
            }
        }
        public ulong HumanID
        {
            get
            {
                return ViewState["HumanID"] == null ? 0 : Convert.ToUInt32(ViewState["HumanID"]);
            }
            set
            {
                ViewState["HumanID"] = value;
            }
        }
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
        public ulong PhysicianID
        {
            get
            {
                return ViewState["PhysicianID"] == null ? 0 : Convert.ToUInt32(ViewState["PhysicianID"]);
            }
            set
            {
                ViewState["PhysicianID"] = value;
            }
        }
        public void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Diagnostic Order" + " - " + ClientSession.UserName;
            btnMoveToNextProcess.Visible = false;
            btnPlan.Visible = false;
            if (!IsPostBack)
            {
                SecurityServiceUtility obj = new SecurityServiceUtility();
                //Cap - 942
                ClientSession.processCheck = true;
                obj.ApplyUserPermissions(this.Page);
                if (Request["HumanID"] != null)
                {
                    HumanID = Convert.ToUInt32(Request["HumanID"]);
                    ClientSession.HumanId = HumanID;
                }
                else
                {
                    HumanID = ClientSession.HumanId;
                }
                if (Request["EncounterID"] != null)
                {
                    EncounterID = Convert.ToUInt32(Request["EncounterID"]);
                    ClientSession.EncounterId = EncounterID;
                }
                else
                {
                    if (Request["ScreenMode"] != null && Request["ScreenMode"].Trim() != string.Empty && Request["ScreenMode"].ToString().ToUpper() == "MENU")
                        EncounterID = 0;
                    else
                        EncounterID = ClientSession.EncounterId;
                }
                if (Request["PhysicianID"] != null)
                {
                    PhysicianID = Convert.ToUInt32(Request["PhysicianID"]);
                    ClientSession.PhysicianId = PhysicianID;
                }
                else
                {
                    PhysicianID = ClientSession.PhysicianId;
                }
                if (Request["OrderSubmitId"] != null)
                {
                    if (ClientSession.UserCurrentProcess == "MA_REVIEW")
                        Session["OrderSubmitId"] = Request["OrderSubmitId"].ToString();
                }

                lnkDiagnosticOrder.Attributes.Add("onClick", "WaitCursor();");
                lnkOrderList.Attributes.Add("onClick", "WaitCursor();");
                if (Request["IsFrontScreen"] != null)
                {
                    lnkDiagnosticOrder.NavigateUrl = "~/frmImageAndLabOrder.aspx?HumanID=" + HumanID.ToString() + "&EncounterID=" + EncounterID.ToString() + "&PhysicianID=" + PhysicianID.ToString() + "&EditedOrderSubmitID=" + Request["EditedOrderSubmitID"].ToString() + "&IsFrontScreen=Y";
                }
                //}
                else if (Request["ScreenMode"] != null)
                {
                    lnkDiagnosticOrder.NavigateUrl = "~/frmImageAndLabOrder.aspx?ScreenMode=" + Request["ScreenMode"].ToString();
                    lnkOrderList.NavigateUrl = "~/frmOrdersList.aspx?ScreenMode=" + Request["ScreenMode"].ToString();
                }
                else
                {
                    lnkDiagnosticOrder.NavigateUrl = "~/frmImageAndLabOrder.aspx";
                }

                LoadorderList();
            }

            //grdOrders.MasterGridViewTemplate.EnableSorting = true;
            //grdOrders.Columns["CreatedOrModifiedDateAndTime1"].SortOrder = RadSortOrder.Descending;

            //grdOrders.Rebind();
        }
       
        public void LoadorderList()
        {
            string procedureType = "LAB PROCEDURE";
            string OrderType = "DIAGNOSTIC ORDER";
            OrdersDTO objOrderDTO = null;
            //if (OrdersForm.btnPlan.Visible == false)
            //{
            //    btnPlan.Visible = false;
            //}
            grdOrders.DataSource = null;
            IList<OrderLabDetailsDTO> ilstOrderLabDetailsDTO = new List<OrderLabDetailsDTO>();
            IList<OrdersAssessment> ilstOrdersAssessment = new List<OrdersAssessment>();
            var serializer = new NetDataContractSerializer();
            object objDTO;
            objDTO = (object)serializer.ReadObject(objOrdersManager.LoadOrders(EncounterID, PhysicianID, HumanID, OrderType, procedureType, UtilityManager.ConvertToUniversal(), false));
            objOrderDTO = (OrdersDTO)objDTO;
            Session["objOrderDTO"] = objOrderDTO;
            humanRecord = objOrderDTO.objHuman;

            sHumanName = objOrderDTO.objHuman.Last_Name + "," + objOrderDTO.objHuman.First_Name;
            sIdentificationNumber = objOrderDTO.objHuman.Human_ID.ToString();
            humanName = objOrderDTO.objHuman.Last_Name + "," + objOrderDTO.objHuman.First_Name + " " + objOrderDTO.objHuman.MI;
            if (OrderSubmitID > 0)
            {

            }
            DataTable dt = new DataTable();
            dt.Columns.Add("Edit", typeof(Bitmap));
            dt.Columns.Add("Del", typeof(Bitmap));
            dt.Columns.Add("Procedure", typeof(string));
            dt.Columns.Add("Assessment", typeof(string));
            dt.Columns.Add("AuthReq", typeof(string));
            dt.Columns.Add("Stat", typeof(string));
            dt.Columns.Add("Test Date", typeof(string));
            dt.Columns.Add("Fasting", typeof(string));
            dt.Columns.Add("Lab", typeof(string));
            dt.Columns.Add("Location", typeof(string));
            dt.Columns.Add("Specimen", typeof(string));
            dt.Columns.Add("Specimen Transport Temperature", typeof(string));
            dt.Columns.Add("Unit", typeof(string));
            dt.Columns.Add("Clinical Information", typeof(string));
            dt.Columns.Add("SpecimenInHouse", typeof(string));
            dt.Columns.Add("Lab ID", typeof(string));
            dt.Columns.Add("Submit_ID", typeof(string));
            dt.Columns.Add("Current Process", typeof(string));
            dt.Columns.Add("Test Codes", typeof(string));
            dt.Columns.Add("ICD", typeof(string));
            dt.Columns.Add("View", typeof(string));
            dt.Columns.Add("Print", typeof(string));
            dt.Columns.Add("FilePath", typeof(string));
            dt.Columns.Add("CreatedOrModifiedDateAndTime1", typeof(string));
            dt.Columns.Add("Tag", typeof(object));
            dt.Columns.Add("FillQuestionSetsValue", typeof(string));
            ilstOrdersAssessment = objOrderDTO.OrderAssList;
            ilstOrderLabDetailsDTO = objOrderDTO.ilstOrderLabDetailsDTO;
            var TotalSubmitedOrders = (from rec in ilstOrderLabDetailsDTO select rec.OrdersSubmit.Id).Distinct();
            foreach (var submitID in TotalSubmitedOrders)
            {
                OrdersDTO tagObj = new OrdersDTO();
                IList<OrderLabDetailsDTO> OrderLabDetailsDTOBag = (from rec3 in ilstOrderLabDetailsDTO where rec3.OrdersSubmit.Id == submitID select rec3).ToList<OrderLabDetailsDTO>();
                IList<ulong> OrdersIDCorrespondToSub = (from rec3 in ilstOrderLabDetailsDTO where rec3.OrdersSubmit.Id == submitID select rec3.ObjOrder.Id).Distinct().ToList();
                //IList<OrdersAssessment> OrdersAssessmentBag = (from rec3 in ilstOrdersAssessment where OrdersIDCorrespondToSub.Contains(rec3.Order_ID) select rec3).ToList<OrdersAssessment>();
                IList<OrdersAssessment> OrdersAssessmentBag = (from rec3 in ilstOrdersAssessment where rec3.Order_Submit_ID == submitID select rec3).ToList<OrdersAssessment>();
                tagObj.OrderAssList = OrdersAssessmentBag;
                tagObj.ilstOrderLabDetailsDTO = OrderLabDetailsDTOBag;
                StringBuilder ICD = new StringBuilder();
                string LabProcedure = string.Empty;
                string DiagnosisCodes = string.Empty;
                string SpecimenInHouse = string.Empty;
                IList<OrderLabDetailsDTO> ForCorrespondingSubID = (from rec2 in ilstOrderLabDetailsDTO where rec2.OrdersSubmit.Id == submitID select rec2).ToList<OrderLabDetailsDTO>();
                if (!ForCorrespondingSubID[0].ObjOrder.Internal_Property_Current_Process.StartsWith("DELETED_OR"))
                {
                    DateTime CreatedOrModifiedDateAndTime = new DateTime();
                    string SpecimenType = string.Empty;
                    string SpecimenQty = string.Empty;
                    string stat = ForCorrespondingSubID[0].OrdersSubmit.Stat;
                    string fasting = string.Empty;
                    if (ForCorrespondingSubID[0].OrdersSubmit.Modified_Date_And_Time.ToString("dd-MM-yyyy").StartsWith("01-01-0001"))
                        CreatedOrModifiedDateAndTime = UtilityManager.ConvertToLocal(ForCorrespondingSubID[0].OrdersSubmit.Created_Date_And_Time);
                    else
                        CreatedOrModifiedDateAndTime = ForCorrespondingSubID[0].OrdersSubmit.Modified_Date_And_Time;
                    SpecimenType = ForCorrespondingSubID[0].OrdersSubmit.Specimen_Type;
                    SpecimenQty = ForCorrespondingSubID[0].OrdersSubmit.Quantity.ToString();
                    fasting = ForCorrespondingSubID[0].OrdersSubmit.Fasting;
                    string AuthReq = ForCorrespondingSubID[0].OrdersSubmit.Authorization_Required;
                    string testdate = ForCorrespondingSubID[0].OrdersSubmit.Test_Date;
                    string Notes = ForCorrespondingSubID[0].OrdersSubmit.Order_Notes;
                    string File_Path = string.Empty;
                    File_Path = ForCorrespondingSubID[0].OrdersSubmit.Internal_Property_File_Path;
                    IList<string> dupcheck = new List<string>();
                    string sTestCode = string.Empty;
                    string sICD = string.Empty;
                    foreach (OrderLabDetailsDTO obj1 in ForCorrespondingSubID)
                    {
                        LabProcedure += "  ,  " + obj1.ObjOrder.Lab_Procedure.Trim() + "-" + obj1.ObjOrder.Lab_Procedure_Description.Trim();
                        sTestCode += obj1.ObjOrder.Lab_Procedure.Trim() + ",";
                        //IList<OrdersAssessment> ForThisOrder = (from rec2 in ilstOrdersAssessment where rec2.Order_ID == obj1.ObjOrder.Id select rec2).ToList<OrdersAssessment>();
                        IList<OrdersAssessment> ForThisOrder = (from rec2 in ilstOrdersAssessment where rec2.Order_Submit_ID == obj1.ObjOrder.Order_Submit_ID select rec2).ToList<OrdersAssessment>();
                        foreach (OrdersAssessment ordAss in ForThisOrder)
                        {
                            if (!dupcheck.Contains(ordAss.ICD.Trim() + "-" + ordAss.ICD_Description.Trim()))
                            {
                                dupcheck.Add(ordAss.ICD.Trim() + "-" + ordAss.ICD_Description.Trim());
                                ICD.Append("  ,  " + ordAss.ICD.Trim() + "-" + ordAss.ICD_Description.Trim());
                                sICD += ordAss.ICD.Trim() + ",";
                            }
                        }
                    }
                    SpecimenInHouse = ForCorrespondingSubID[0].OrdersSubmit.Specimen_In_House;
                    DataRow dr = dt.NewRow();
                    //dr["Edit"]= global::Acurus.Capella.UI.Resources.edit;
                    //dr["Del"]= global::Acurus.Capella.UI.Properties.Resources.close_small_pressed;
                    dr["Procedure"] = LabProcedure.Substring(4).Trim();
                    if (ForCorrespondingSubID[0].LabName != "In-House Procedures" && ICD != null && ICD.Length > 3)
                        dr["Assessment"] = ICD.ToString().Substring(4).Trim();
                    dr["AuthReq"] = AuthReq;
                    dr["Stat"] = stat;
                    if (testdate != "0001-01-01")
                        dr["Test Date"] = testdate;
                    else
                        dr["Test Date"] = "";
                    dr["Fasting"] = fasting;
                    dr["Lab"] = ForCorrespondingSubID[0].LabName;
                    dr["Location"] = ForCorrespondingSubID[0].LabLocName;
                    dr["Specimen"] = SpecimenType;
                    dr["Specimen Transport Temperature"] = ForCorrespondingSubID[0].OrdersSubmit.Temperature;
                    if (SpecimenQty.Trim() != string.Empty && SpecimenQty.Trim() != string.Empty && SpecimenQty != "0")
                        dr["Unit"] = SpecimenQty + " " + ForCorrespondingSubID[0].OrdersSubmit.Specimen_Unit;
                    dr["Clinical Information"] = ForCorrespondingSubID[0].OrdersSubmit.Order_Notes;
                    dr["SpecimenInHouse"] = SpecimenInHouse;
                    dr["Lab ID"] = ForCorrespondingSubID[0].OrdersSubmit.Lab_ID.ToString();
                    dr["Submit_ID"] = ForCorrespondingSubID[0].OrdersSubmit.Id.ToString();
                    dr["Current Process"] = ForCorrespondingSubID[0].ObjOrder.Internal_Property_Current_Process;
                    dr["Test Codes"] = sTestCode;
                    dr["ICD"] = sICD;
                    //if (File_Path == "")
                    //    dr["View"] = global::Acurus.Capella.UI.Properties.Resources.Down_Disabled;
                    //else
                    //    dr["View"] = global::Acurus.Capella.UI.Properties.Resources.Down;
                    dr["FilePath"] = File_Path;
                    dr["CreatedOrModifiedDateAndTime1"] = CreatedOrModifiedDateAndTime;

                    dr["Tag"] = tagObj;
                    IList<string> ilstQuestionSet = new List<string>();

                    var faltList = tagObj.ilstOrderLabDetailsDTO.SelectMany(a => a.lstQuestionSetNames);
                    foreach (string strInList in faltList)
                    {
                        ilstQuestionSet.Add(strInList);
                    }
                    ilstQuestionSet = ilstQuestionSet.Distinct().ToList<string>();
                    //ilstQuestionSet.Add("AOE");
                    dr["FillQuestionSetsValue"] = string.Join(",", ilstQuestionSet.ToArray<string>());
                    dt.Rows.Add(dr);
                }
            }
            foreach (OrdersSubmit obj2 in objOrderDTO.ilstOrdersSubmitForPartialOrders)
            {
                DataRow dr = dt.NewRow();
                //GridViewDataRowInfo row = this.grdOrders.Rows.AddNew();
                //dr["Edit"] =new Bitmap("\\Resources\\action_add2.gif");
                //dr["Del"] = "test2";
                dr["Procedure"] = string.Empty;
                dr["AuthReq"] = obj2.Authorization_Required;
                dr["Stat"] = obj2.Stat;
                dr["Test Date"] = obj2.Test_Date;
                dr["Fasting"] = obj2.Fasting;
                dr["Lab"] = obj2.Lab_Name;
                dr["Location"] = obj2.Lab_Location_Name;
                dr["Specimen"] = obj2.Specimen_Type;
                dr["Specimen Transport Temperature"] = obj2.Temperature;
                if (obj2.Quantity.ToString().Trim() != string.Empty && obj2.Quantity.ToString().Trim() != string.Empty && obj2.Quantity.ToString().Trim() != "0")
                    dr["Unit"] = obj2.Quantity.ToString().Trim() + " " + obj2.Specimen_Unit;
                dr["Clinical Information"] = obj2.Order_Notes;
                dr["SpecimenInHouse"] = obj2.Specimen_In_House;
                dr["Lab ID"] = obj2.Lab_ID.ToString();
                dr["Submit_ID"] = obj2.Id.ToString();
                //dr["View"] = global::Acurus.Capella.UI.Properties.Resources.Down_Disabled;
                if (obj2.Modified_Date_And_Time.ToString("dd-MM-yyyy").StartsWith("01-01-0001"))
                    dr["CreatedOrModifiedDateAndTime1"] = UtilityManager.ConvertToLocal(obj2.Created_Date_And_Time);
                else
                    dr["CreatedOrModifiedDateAndTime1"] = obj2.Modified_Date_And_Time;
                dr["Current Process"] = obj2.Culture_Location;
                dr["Tag"] = obj2;
                //IList<string> ilstQuestionSet = new List<string>();
                //if (obj.ilstOrderLabDetailsDTO.Any(a => a.objAFP != null))
                //    ilstQuestionSet.Add("AFP");
                //if (obj.ilstOrderLabDetailsDTO.Any(a => a.objBloodLead != null))
                //    ilstQuestionSet.Add("CYT");
                //if (obj.ilstOrderLabDetailsDTO.Any(a => a.objCytology != null))
                //    ilstQuestionSet.Add("ZCY");
                //dr["QuestionSetsToBePopUp"] = string.Join(",", ilstQuestionSet.ToArray<string>());
                dt.Rows.Add(dr);
            }
            grdOrders.DataSource = dt;
            this.grdOrders.MasterTableView.SortExpressions.Clear();

            // Create "Date" sorting
            GridSortExpression expression = new GridSortExpression();
            expression.FieldName = "CreatedOrModifiedDateAndTime1";
            expression.SortOrder = GridSortOrder.Descending;

            // Set initial sortexpression to the [Date] column
            this.grdOrders.MasterTableView.SortExpressions.AddSortExpression(expression);
            grdOrders.DataBind();




            foreach (GridDataItem item in grdOrders.Items)
            {


                // IList<InHouseProcedure> lstImage = objOtherProDTO.OtherProcedure.Where(a => a.In_House_Procedure_Group_ID == Convert.ToUInt64(item["GroupKey"].Text)).ToList<InHouseProcedure>();
                // if (lstImage.Count > 0 && lstImage[0].File_Management_Index_ID == string.Empty)
                // {
                TableCell selectCel0 = item["FilePath"];

                if (selectCel0.Text == string.Empty)
                {
                    TableCell selectCell = item["View"];
                    ImageButton gd = (ImageButton)selectCell.Controls[0];
                    gd.ImageUrl = "~/Resources/Down_Disabled.bmp";
                }
                //}

            }
        }
        protected void btnrefreshgrid_Click(object sender, EventArgs e)
        {
            RadResultWindow.Visible = false;
            RadResultWindow.VisibleOnPageLoad = false;
            LoadorderList();
        }


        private void GoToNextTab()
        {

            //Telerik.Web.UI.RadTabStrip tabStrip = (Telerik.Web.UI.RadTabStrip)this.Master.FindControl("tabOrdersTab");
            //RadTab OrderDetailTab = tabStrip.FindTabByText("Order Details");
            //OrderDetailTab.Enabled = true;
            //OrderDetailTab.Selected = true;
        }
        private void GoToNextPageView()
        {
            RadMultiPage multiPage = (RadMultiPage)this.NamingContainer.FindControl("tabOrdersTab");
            RadPageView ViewDetailOrder = multiPage.FindPageViewByID("pgViewDetailOrder");
            //if (ViewDetailOrder == null)
            //{
            //    ViewDetailOrder = new RadPageView();
            //    ViewDetailOrder.ID = "Flight";
            //    multiPage.PageViews.Add(ViewDetailOrder);
            //}
            ViewDetailOrder.Selected = true;
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)grdOrders.DataSource;
            //            if (e.CommandArgument != null && Convert.ToInt32(e.CommandArgument) != -1)
            //          {
            ulong OrderSubmitID = Convert.ToUInt32(grdOrders.Items[Convert.ToInt32(hdnRowIndex.Value)].Cells[19].Text);
            //                    ulong OrderSubmitID = Convert.ToUInt64(dt.Rows[Convert.ToInt32(e.CommandArgument)]["Submit_ID"].ToString());
            bool temp = objOrdersManager.DeletedOrders(OrderSubmitID, "DIAGNOSTIC ORDER", string.Empty);
            if (temp)
            {
                // ApplicationObject.erroHandler.DisplayErrorMessage("230137", "DIAGNOSTIC ORDER", this);
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('230138');", true);
                return;
                // ApplicationObject.erroHandler.DisplayErrorMessage("230138", "DIAGNOSTIC ORDER", this);
            }
            // Page_Load(sender, e);
            LoadorderList();
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();};RefreshNotification('Orders');", true);
            //        }
        }
        protected void grdOrders_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {

                GoToNextPageView();
                //e.Item.Cells[
                //OrdersForm.OrderSubmitID_for_importResult = Convert.ToUInt64(grdOrders.SelectedRows[0].Cells["Submit_ID"].Value);
                //if (grdOrders.Rows[e.RowIndex].Tag is OrdersDTO)
                //{
                //    string[] seprator = new string[1] { "  ,  " };
                //    OrdersDTO temp = (OrdersDTO)grdOrders.Rows[e.RowIndex].Tag;
                //    OrdersForm.IsNormalMode = false;
                //    OrdersForm.NewOrderClick();
                //    OrdersForm.CellClick = false;
                //    OrdersForm.UpdatedOrders = temp.ilstOrderLabDetailsDTO.Select(a => a.ObjOrder).ToList<Orders>();
                //    OrdersForm.ProceduresViewList = ((grdOrders.Rows[e.RowIndex].Cells["Procedure"].Value).ToString().Split(seprator, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
                //    OrdersForm.ICDViewList = ((grdOrders.Rows[e.RowIndex].Cells["Assessment"].Value).ToString().Split(seprator, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
                //    OrdersForm.ilstOrdersRequiredForms = new List<OrdersRequiredForms>();
                //    foreach (IList<OrdersRequiredForms> obj in temp.ilstOrderLabDetailsDTO.Select(a => a.ilstOrdersRequiredForms))
                //    {
                //        OrdersForm.ilstOrdersRequiredForms = OrdersForm.ilstOrdersRequiredForms.Concat(obj).ToList<OrdersRequiredForms>();
                //    }
                //    OrdersForm.OrderViewObject = temp;
                //    OrdersForm.IsNormalMode = false;
                //    OrdersForm.LoadScreenFromOrderList();
                //    OrdersForm.btnClearAll.Text = "&Cancel";
                //    if (sProcess.ToUpper() == "CHECK OUT")
                //        OrdersForm.gbProcedures.Enabled = false;
                //    if (sProcess.ToUpper() == "MA_REVIEW" && OrdersForm.IsEditable)
                //    {
                //        if (!OrdersForm.OrderViewObject.ilstOrderLabDetailsDTO[0].OrdersSubmit.Lab_Name.StartsWith(OrdersForm.InHouseLabNameFromLookUp))
                //        {
                //            OrdersForm.gbSelectICD.Enabled = true;
                //        }

                //        OrdersForm.gbProcedures.Enabled = true;
                //        OrdersForm.gbOrderDetails.Enabled = true;
                //        OrdersForm.gbSpecimenDetails.Enabled = true;
                //    }
                //    if (sProcess.ToUpper() == "MA_REVIEW" && !OrdersForm.IsEditable)
                //    {
                //        OrdersForm.btnClearAll.Enabled = false;
                //        OrdersForm.ControlsDisabledForEditingPurpose = false;
                //    }
                //    //OrdersForm.cboLab.Enabled = false;
                //}
                //else
                //{
                //    //OrdersForm.NewOrderClick();
                //    //OrdersForm.CellClick = false;
                //    //OrdersForm.UpdatedOrders = new List<Orders>();
                //    //OrdersForm.ProceduresViewList = new List<string>();
                //    //OrdersForm.ICDViewList = new List<string>();
                //    //OrdersForm.ilstOrdersRequiredForms = new List<OrdersRequiredForms>();
                //    //OrdersForm.OrderViewObject = new OrdersDTO();
                //    //OrderLabDetailsDTO tempOrderLabDetailsDTO = new OrderLabDetailsDTO();
                //    //tempOrderLabDetailsDTO.OrdersSubmit = (OrdersSubmit)grdOrders.Rows[e.RowIndex].Tag;
                //    //OrdersForm.OrderViewObject.ilstOrderLabDetailsDTO.Add(tempOrderLabDetailsDTO);
                //    //OrdersForm.IsNormalMode = false;
                //    //OrdersForm.LoadScreenFromOrderList();
                //    //OrdersForm.btnClearAll.Text = "&Cancel";
                //    //if (!OrdersForm.OrderViewObject.ilstOrderLabDetailsDTO[0].OrdersSubmit.Lab_Name.StartsWith(OrdersForm.InHouseLabNameFromLookUp))
                //    //{
                //    //    OrdersForm.gbSelectICD.Enabled = true;
                //    //}
                //    ////else if (OrdersForm.OrderViewObject.ilstOrderLabDetailsDTO[0].OrdersSubmit.Lab_Name.Trim() == string.Empty)
                //    ////{
                //    ////    OrdersForm.gbCenterDetail.Enabled = true;
                //    ////}
                //    //if (sProcess.ToUpper() == "MA_REVIEW" && OrdersForm.OrderViewObject.ilstOrderLabDetailsDTO[0].OrdersSubmit.Lab_Name.Trim() == string.Empty)
                //    //    OrdersForm.gbCenterDetail.Enabled = true;
                //    //OrdersForm.gbProcedures.Enabled = true;
                //    //OrdersForm.gbOrderDetails.Enabled = true;
                //    //OrdersForm.gbSpecimenDetails.Enabled = true;
                //}
                //DetailedOrderTab.Select();
            }
            else if (e.CommandName == "Del")
            {
                //int del = ApplicationObject.erroHandler.DisplayErrorMessage("230105", "DIAGNOSTIC ORDER", this);
                //if (del == 1)
                //{


                DataTable dt = (DataTable)grdOrders.DataSource;
                if (e.CommandArgument != null && Convert.ToInt32(e.CommandArgument) != -1)
                {
                    ulong OrderSubmitID = Convert.ToUInt32(grdOrders.Items[Convert.ToInt32(e.CommandArgument)].Cells[19].Text);
                    //                    ulong OrderSubmitID = Convert.ToUInt64(dt.Rows[Convert.ToInt32(e.CommandArgument)]["Submit_ID"].ToString());
                    bool temp = objOrdersManager.DeletedOrders(OrderSubmitID, "DIAGNOSTIC ORDER", string.Empty);
                    if (temp)
                    {
                        // ApplicationObject.erroHandler.DisplayErrorMessage("230137", "DIAGNOSTIC ORDER", this);
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('230138');", true);
                        return;
                        // ApplicationObject.erroHandler.DisplayErrorMessage("230138", "DIAGNOSTIC ORDER", this);
                    }
                    // Page_Load(sender, e);
                    LoadorderList();
                }

                //DataTable dt = (DataTable)grdOrders.DataSource;
                //if (dt != null)
                //{
                //    ulong OrderSubmitID = Convert.ToUInt64(dt.Rows[Convert.ToInt32(e.CommandArgument)]["Submit_ID"].ToString());
                //    bool temp = objOrdersManager.DeletedOrders(OrderSubmitID, "DIAGNOSTIC ORDER", string.Empty);
                //    if (temp)
                //    {
                //       // ApplicationObject.erroHandler.DisplayErrorMessage("230137", "DIAGNOSTIC ORDER", this);
                //    }
                //    else
                //    {
                //       // ApplicationObject.erroHandler.DisplayErrorMessage("230138", "DIAGNOSTIC ORDER", this);
                //    }
                //    Page_Load(sender, e);
                //}
                //frmOrdersList_Load(new object(), new EventArgs());
                //OrdersForm.NewOrderClick();
                //}
            }
            else if (e.CommandName == "EditC")
            {
                DataTable dt = (DataTable)grdOrders.DataSource;

                if (e.CommandArgument != null && Convert.ToInt32(e.CommandArgument) != -1)
                {
                    hdnPaperForm.Value = "true";
                    //string OrderSubmitID = dt.Rows[Convert.ToInt32(e.CommandArgument)]["Submit_ID"].ToString();
                    string OrderSubmitID = grdOrders.Items[Convert.ToInt32(e.CommandArgument)].Cells[19].Text;
                    InsertOrderSubmitIDInURL(OrderSubmitID);
                    if (grdOrders.Items[Convert.ToInt32(e.CommandArgument)].Cells[26].Text != string.Empty)
                    {
                        Session["ResultExist"] = true;
                    }
                    ScriptManager.RegisterStartupScript(this, typeof(frmOrdersList), "SwitchScreen", "var lnkDetailedOrder= document.getElementById('lnkDiagnosticOrder');lnkDetailedOrder.click();", true);
                    //ScriptManager.RegisterStartupScript(this, typeof(frmOrdersList), "SwitchScreen", "var lnkDetailedOrder= document.getElementById('lnkDiagnosticOrder');lnkDetailedOrder.click();window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = 'true';", true);

                }
            }
            else if (e.CommandName == "View")
            {
                DataTable dt = (DataTable)grdOrders.DataSource;
                if (e.CommandArgument != null && Convert.ToInt32(e.CommandArgument) != -1)
                {
                    //if (dt.Rows[Convert.ToInt32(e.CommandArgument)]["FilePath"].ToString() != string.Empty)
                    if (grdOrders.Items[Convert.ToInt32(e.CommandArgument)].Cells[26].Text != string.Empty)
                    {
                        //string OrderSubmitID = dt.Rows[Convert.ToInt32(e.CommandArgument)]["Submit_ID"].ToString();
                        string OrderSubmitID = grdOrders.Items[Convert.ToInt32(e.CommandArgument)].Cells[19].Text;
                        RadResultWindow.Visible = true;
                        RadResultWindow.VisibleOnPageLoad = true;
                        RadResultWindow.Width = 1199;
                        RadResultWindow.Height = 588;
                        RadResultWindow.Left=37;
                        RadResultWindow.Top=32;
                        RadResultWindow.VisibleStatusbar = false;
                        RadResultWindow.OnClientBeforeClose = "CloseResultPage";
                        RadResultWindow.OnClientClose = "RefreshCloseResultPage";
                        RadResultWindow.NavigateUrl = "frmViewResult.aspx?HumanID=" + HumanID.ToString() + "&OrderSubmitId=" + OrderSubmitID + "&ResultId=" + "0" + "&CurrentProcess=" + grdOrders.Items[Convert.ToInt32(e.CommandArgument)].Cells[17].Text + "&Type=Results&Opening_from=OrdersList";//BugID:43099
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    }
                }
            }
            else if (e.CommandName == "Print")
            {
                DataTable dt = (DataTable)grdOrders.DataSource;
                string OrderIDList = "";
                //string Loinc = "";
                string urls = "";
                //if (dt != null)
                //{

                //    OrdersManager objOrdersManager = new OrdersManager();
                //    IList<string> ordersForRecommendedMaterialsList = objOrdersManager.GetOrdersForRecommendedMaterials(ClientSession.EncounterId, ClientSession.HumanId);
                //    if (ordersForRecommendedMaterialsList.Count > 0)
                //    {
                //        foreach (var item in ordersForRecommendedMaterialsList)
                //        {
                //            Loinc = item.Split('|')[1].ToString();
                //            urls +=Loinc + ";";                           

                //        }

                //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty,"openLink('"+urls+"');",  true); 

                //    }
                //}


                //string proc = dt.Rows[Convert.ToInt32(e.CommandArgument)]["Procedure"].ToString();
                if (e.CommandArgument != "")
                {
                    //Added the below condition by srividhya. Refer bug id: 31500.
                    if (grdOrders.Items[Convert.ToInt32(e.CommandArgument)].Cells[5].Text == "" || grdOrders.Items[Convert.ToInt32(e.CommandArgument)].Cells[5].Text == "&nbsp;")
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('230152');", true);
                        //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230152');", true); 
                        return;
                    }
                    string proc = grdOrders.Items[Convert.ToInt32(e.CommandArgument)].Cells[5].Text;

                    string[] strvalue = proc.ToString().Split(',');
                    string[] cpt_code = new string[5];

                    for (int i = 0; i < strvalue.Count(); i++)
                    {
                        if (strvalue[i].Contains('-'))
                        {
                            cpt_code = strvalue[i].ToString().Split('-');
                            //if (cpt_code[0].All(Char.IsDigit))
                            //{
                            if (i != (strvalue.Count() - 1))
                            {
                                OrderIDList += cpt_code[0].ToString() + ",";
                            }
                            else
                            {
                                OrderIDList += cpt_code[0].ToString();
                            }
                            //}
                        }

                    }
                    if (OrderIDList.EndsWith(","))
                    {
                        OrderIDList = OrderIDList.TrimEnd(',');
                    }
                    IList<string> ionic = objOrdersManager.GetOrdersForPrint(OrderIDList);
                    for (int i = 0; i < ionic.Count; i++)
                    {
                        if (ionic[i] != "")
                        {
                            urls += ionic[i] + ";";
                            // string pageurl = "http://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.1&mainSearchCriteria.v.c=" + ionic[i].ToString() + "&informationRecipient.languageCode.c = en";
                            // Response.Write(string.Format("<script>window.open('" + pageurl + "','_new');</script>"));
                        }

                    }
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "openLink('" + urls + "');", true);

                }
            }
            else if (e.CommandName == "FillQuestionSet")
            {
                OrderCodeLibrary objSelectedOrderCode = null;
                OrderCodeLibraryManager objOrderCodeLibraryManager = new OrderCodeLibraryManager();
                bool zblFlag = false, zcyFlag = false, zsaFlag = false;
                ulong OrderSubmitID = Convert.ToUInt32(grdOrders.Items[Convert.ToInt32(e.CommandArgument)].Cells[19].Text);
                string CPT = grdOrders.Items[Convert.ToInt32(e.CommandArgument)].Cells[5].Text.Split('-')[0];
                string Lab = grdOrders.Items[Convert.ToInt32(e.CommandArgument)].Cells[14].Text;
                objSelectedOrderCode = objOrderCodeLibraryManager.GetOrderCodeDetailsForSelectedOrderCode(CPT);
                if (objSelectedOrderCode.Order_Code_Question_Set_Segment != string.Empty)
                {
                    switch (objSelectedOrderCode.Order_Code_Question_Set_Segment.ToUpper())
                    {
                        case "ZBL":
                            zblFlag = true;
                            break;
                        case "ZCY":
                            zcyFlag = true;
                            break;
                        case "ZSA":
                            zsaFlag = true;
                            break;
                    }
                }
                if (Lab == "LabCorp")
                {
                    if (zblFlag)
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Diagnositic Order", "OpenBloodLead('" + OrderSubmitID + "');", true);
                        //return;
                        RadResultWindow.NavigateUrl = "frmOrdersQuestionSetsBloodLead.aspx?OrderSubmitID=" + OrderSubmitID;
                        RadResultWindow.OnClientClose = "CloseQuestionSetWindow";
                        RadResultWindow.Width = 630;
                        RadResultWindow.Height = 120;
                        RadResultWindow.VisibleOnPageLoad = true;
                        RadResultWindow.VisibleTitlebar = true;
                        RadResultWindow.VisibleStatusbar = false;
                        RadResultWindow.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move;
                    }
                    if (zcyFlag)
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Diagnositic Order", "OpenCytology('" + OrderSubmitID + "');", true);
                        //return;
                        RadResultWindow.NavigateUrl = "frmOrdersQuestionSetsCytology.aspx?OrderSubmitID=" + OrderSubmitID;
                        RadResultWindow.OnClientClose = "CloseQuestionSetWindow";
                        RadResultWindow.Width = 730;
                        RadResultWindow.Height = 550;
                        RadResultWindow.VisibleOnPageLoad = true;
                        RadResultWindow.VisibleTitlebar = true;
                        RadResultWindow.VisibleStatusbar = false;
                        RadResultWindow.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move;
                    }
                    if (zsaFlag)
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Diagnositic Order", "OpenAFP('" + OrderSubmitID + "');", true);
                        //return;
                        RadResultWindow.NavigateUrl = "frmOrdersQuestionSetsAFP.aspx?OrderSubmitID=" + OrderSubmitID;
                        RadResultWindow.OnClientClose = "CloseQuestionSetWindow";
                        //RadResultWindow.Width = 1070;
                        //RadResultWindow.Height = 740;
                        RadResultWindow.Width = 830;
                        RadResultWindow.Height = 500;
                        RadResultWindow.VisibleOnPageLoad = true;
                        RadResultWindow.VisibleTitlebar = true;
                        RadResultWindow.VisibleStatusbar = false;
                        RadResultWindow.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move;
                    }
                }
                else if (Lab == "Quest Diagnostics")
                {
                    AOELookUpManager AOEMngr = new AOELookUpManager();
                    IList<AOELookUp> ilstAOELookUp = new List<AOELookUp>();
                    ilstAOELookUp = AOEMngr.GetAOELookUpList(CPT);
                    ///frmOrderQuestionSetAOE objfrmOrderQuestionSetAOE;
                    if ((ilstAOELookUp != null && ilstAOELookUp.Count > 0))
                    {
                        RadResultWindow.NavigateUrl = "frmOrderQuestionSetAOE.aspx?OrderSubmitID=" + OrderSubmitID;
                        RadResultWindow.OnClientClose = "CloseQuestionSetWindow";
                        RadResultWindow.Width = 555;
                        RadResultWindow.Height = 500;
                        RadResultWindow.VisibleOnPageLoad = true;
                        RadResultWindow.VisibleTitlebar = true;
                        RadResultWindow.VisibleStatusbar = false;
                        RadResultWindow.Behaviors = WindowBehaviors.Close;
                    }
                }
                else
                {
                    RadResultWindow.VisibleOnPageLoad = false;
                }

            }
            if (e.CommandName != "EditC" && e.CommandName != "View")
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }


        void InsertOrderSubmitIDInURL(string OrderSubmitID)
        {
            string sEnable = "true";
            if (lnkDiagnosticOrder.NavigateUrl.Contains("EditedOrderSubmitID"))
            {
                if (lnkDiagnosticOrder.NavigateUrl.Contains("IsFrontScreen"))
                    lnkDiagnosticOrder.NavigateUrl = "~/frmImageAndLabOrder.aspx?HumanID=" + HumanID.ToString() + "&EncounterID=" + EncounterID.ToString() + "&PhysicianID=" + PhysicianID.ToString() + "&EditedOrderSubmitID=" + OrderSubmitID + "&ScreenMode=" + Request["ScreenMode"] + "&IsFrontScreen=Y" + "&Enable=" + sEnable + "&hdnForEditErrorMsg=Edit";
                else
                    lnkDiagnosticOrder.NavigateUrl = "~/frmImageAndLabOrder.aspx?HumanID=" + HumanID.ToString() + "&EncounterID=" + EncounterID.ToString() + "&PhysicianID=" + PhysicianID.ToString() + "&EditedOrderSubmitID=" + OrderSubmitID + "&ScreenMode=" + Request["ScreenMode"] + "&Enable=" + sEnable + "&hdnForEditErrorMsg=Edit";
            }
            else
            {
                if (lnkDiagnosticOrder.NavigateUrl != null && lnkDiagnosticOrder.NavigateUrl.Contains("?"))
                    lnkDiagnosticOrder.NavigateUrl = lnkDiagnosticOrder.NavigateUrl + "&EditedOrderSubmitID=" + OrderSubmitID + "&FilledInPaperForm=" + hdnPaperForm.Value + "&ScreenMode=" + Request["ScreenMode"] + "&Enable=" + sEnable + "&hdnForEditErrorMsg=Edit";
                else
                    lnkDiagnosticOrder.NavigateUrl = lnkDiagnosticOrder.NavigateUrl + "?EditedOrderSubmitID=" + OrderSubmitID + "&ScreenMode=" + Request["ScreenMode"] + "&Enable=" + sEnable + "&hdnForEditErrorMsg=Edit";
            }
        }
        protected void btnPrintRequsition_Click(object sender, EventArgs e)
        {
            hdnSelectedItem.Value = string.Empty;
            //string path = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["CapellaConfigurationSetttings"] + "\\" + System.Configuration.ConfigurationSettings.AppSettings["LabCorpRequesitionFilePathName"]);
            if (grdOrders.MasterTableView.Items.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Order List", "DisplayErrorMessage('230115'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            string path = Server.MapPath("Documents/" + Session.SessionID);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string FileLocation = string.Empty;
            PrintOrders print = new PrintOrders();
            OrdersDTO objOrderDTO = new OrdersDTO();
            FillHumanDTO objHuman = new FillHumanDTO();
            if (Session["objOrderDTO"] != null)
                objOrderDTO = (OrdersDTO)Session["objOrderDTO"];
            if (Session["objFillHumanDTO"] != null)
                objHuman = (FillHumanDTO)Session["objFillHumanDTO"];
            else
                objHuman = objOrderDTO.objHuman;
            //IList<string> LabAndOrderSubmitID = objOrdersManager.GetOrderSubmitIDForPrintRequestion(EncounterID, PhysicianID, HumanID);
            IList<string> LabAndOrderSubmits = new List<string>();
            LabAndOrderSubmits = objOrdersManager.GetOrderSubmitIDForPrintRequestion(EncounterID, PhysicianID, HumanID,OrderType);


            IList<string> LabAndOrderSubmitID = LabAndOrderSubmits.Distinct().ToList<string>();

            foreach (string str in LabAndOrderSubmitID)
            {
                if (str.StartsWith("1"))
                    FileLocation = print.PrintRequisitionUsingDatafromDB(Convert.ToUInt32(str.Split('|')[1]), path, "ORDERS", EncounterID, OrderType);
                else
                    FileLocation = print.PrintSplitRequisitionUsingDatafromDBQuest(Convert.ToUInt32(str.Split('|')[1]), path, "ORDERS", false, OrderType);

                string[] Split1 = new string[] { Server.MapPath("") };
                string[] FileName1 = FileLocation.Split(Split1, StringSplitOptions.RemoveEmptyEntries);
                if (hdnSelectedItem.Value == string.Empty)
                {
                    hdnSelectedItem.Value = FileName1[0].ToString();
                }
                else
                {
                    hdnSelectedItem.Value += "|" + FileName1[0].ToString();
                }
            }

            FileLocation = print.CallPrintLabAndImageOrders(path, EncounterID, objOrderDTO, objHuman,OrderType);
            if (FileLocation != string.Empty)
            {

                string[] Split = new string[] { Server.MapPath("") };
                string[] FileName = FileLocation.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < FileName.Length; i++)
                {
                    if (hdnSelectedItem.Value == string.Empty)
                    {
                        hdnSelectedItem.Value = FileName[i].ToString();
                    }
                    else
                    {
                        hdnSelectedItem.Value += "|" + FileName[i].ToString();
                    }
                }
            }


            if (hdnSelectedItem.Value != string.Empty)
            {
                string FaxSubject = string.Empty;
                string sLABNAME = string.Empty;
                if (objOrderDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit != null)
                {
                    if (objOrderDTO.ilstOrderLabDetailsDTO.Count > 0)
                    {
                        for (int y = 0; y < objOrderDTO.ilstOrderLabDetailsDTO.Count; y++)
                        {
                            if (y == 0)
                                sLABNAME = objOrderDTO.ilstOrderLabDetailsDTO[y].OrdersSubmit.Id + "_" + objOrderDTO.ilstOrderLabDetailsDTO[y].OrdersSubmit.Lab_Name + "_" + objOrderDTO.ilstOrderLabDetailsDTO[y].OrdersSubmit.Created_Date_And_Time.ToString("dd-MMM-yyyy");
                            else
                                sLABNAME += "|" + objOrderDTO.ilstOrderLabDetailsDTO[y].OrdersSubmit.Id + "_" + objOrderDTO.ilstOrderLabDetailsDTO[y].OrdersSubmit.Lab_Name + "_" + objOrderDTO.ilstOrderLabDetailsDTO[y].OrdersSubmit.Created_Date_And_Time.ToString("dd-MMM-yyyy");
                        }
                        //
                    }
                }
                //FaxSubject = "_" + objFillHumnaDTO.First_Name + " " + objFillHumnaDTO.Last_Name + "_" + objOrderDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Lab_Name + "_" + objOrderDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Created_Date_And_Time;    
                FaxSubject = "_" + objHuman.First_Name + " " + objHuman.Last_Name + "$" + sLABNAME;
                //MessageWindow.Width = Unit.Pixel(750);
                //MessageWindow.Height = Unit.Pixel(600);
                //MessageWindow.OnClientClose = "CloseQuestionSetWindow";
                //MessageWindow.NavigateUrl = "frmPrintPDF.aspx?Location=DYNAMIC&SI=" + hdnSelectedItem.Value.ToString();
                //MessageWindow.VisibleStatusbar = false;
                //MessageWindow.VisibleOnPageLoad = true;
                ScriptManager.RegisterStartupScript(this, typeof(frmImageAndLabOrder), string.Empty, "OpenPDFImage('" + FaxSubject + "');", true);
                //ScriptManager.RegisterStartupScript(this, typeof(frmImageAndLabOrder), string.Empty, "OpenPDFImage();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }

            //hdnSelectedItem.Value = string.Empty;



            //ScriptManager.RegisterStartupScript(this, typeof(frmImageAndLabOrder), string.Empty, "OpenPDF();", true);

        }

        protected void btnGenerateABN_Click(object sender, EventArgs e)
        {
            hdnSelectedItem.Value = string.Empty;
            //string path = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["CapellaConfigurationSetttings"] + "\\" + System.Configuration.ConfigurationSettings.AppSettings["LabCorpRequesitionFilePathName"]);
            string path = Server.MapPath("Documents/" + Session.SessionID);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string FileLocation = string.Empty;
            FillHumanDTO humanRecord;
            var serializer = new NetDataContractSerializer();
            OrdersDTO objOrderDTO = null;
            if (Session["objOrderDTO"] != null)
            {
                objOrderDTO = (OrdersDTO)Session["objOrderDTO"];
            }
            humanRecord = objOrderDTO.objHuman;
            string sHumanName = objOrderDTO.objHuman.Last_Name + "," + objOrderDTO.objHuman.First_Name;
            string sIdentificationNumber = objOrderDTO.objHuman.Human_ID.ToString();
            string humanName = objOrderDTO.objHuman.Last_Name + "," + objOrderDTO.objHuman.First_Name + " " + objOrderDTO.objHuman.MI;
            PrintOrders print = new PrintOrders();
            bool IsABNNeed = false;
            ResultMasterManager objResultMasterManager = new ResultMasterManager();
            IList<string> LabCorpCPT = new List<string>();
            IList<string> LabCorpICD = new List<string>();
            IList<string> QuestOrderSubmitID = new List<string>();
            IList<Orders> QuestORdersList = new List<Orders>();
            IList<OrdersAssessment> OrdersAssessmentList = new List<OrdersAssessment>();
            objResultMasterManager.GenerateABNDatas(EncounterID, PhysicianID, HumanID, out LabCorpCPT, out LabCorpICD, out QuestOrderSubmitID, out OrdersAssessmentList, out QuestORdersList, objOrderDTO.OrderAssList, objOrderDTO.Lists);
            AbnResponseType objAbnResponseType = new AbnResponseType();
            Acurus.Capella.DataAccess.com.labcorp.www4.Message[] objMessage;
            LabCarrierLookUpManager objLabCarrierLookUpManager = new LabCarrierLookUpManager();
            var ilstorder_submit_id = (from ordsubmitID in QuestOrderSubmitID where ordsubmitID != "0" select ordsubmitID).Distinct().ToList();
            InsurancePlanManager objInsurancePlanManager = new InsurancePlanManager();
            string FilePath = string.Empty;
            string RetrunFilePath = string.Empty;
            if (QuestOrderSubmitID != null && QuestOrderSubmitID.Count > 0)
            {
                IList<LabSettings> LabList = new List<LabSettings>();
                LabcorpSettingsManager objLabcorpSettingsManager = new LabcorpSettingsManager();
                LabList = objLabcorpSettingsManager.GetLabcorpSettings();
                LabList = LabList.Where(a => a.Lab_ID == 2).ToList<LabSettings>();

                InsurancePlan objInsurancePlan = null;
                if (humanRecord.PatientInsuredBag != null && humanRecord.PatientInsuredBag.Count > 0)
                    objInsurancePlan = objInsurancePlanManager.GetInsurancebyID(humanRecord.PatientInsuredBag[0].Insurance_Plan_ID)[0];
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230125');", true);
                    return;
                }

                LabCarrierLookUp objLabCarrierLookUp = objLabCarrierLookUpManager.GetLabCarrierDetailsForInsPlanID(objInsurancePlan.Id, 2);
                foreach (string ul in QuestOrderSubmitID)
                {
                    IList<Orders> tempOrders = QuestORdersList.Where(a => a.Order_Submit_ID == Convert.ToUInt64(ul)).ToList<Orders>();
                    string OrdeCode = string.Empty;
                    foreach (Orders obj in tempOrders)
                    {
                        OrdeCode += obj.Lab_Procedure + "\t" + obj.Lab_Procedure_Description + "\n";
                    }

                    IsABNNeed = print.GenerateABN(LabList[0].Lab_Client_Account_No, LabList[0].Receiving_Facility, humanRecord.Human_ID.ToString(), (humanRecord.Last_Name + "," + humanRecord.First_Name + "," + humanRecord.MI), objLabCarrierLookUp.LCA_Carrier_Code, objInsurancePlan.Ins_Plan_Name, objInsurancePlan.Payer_Addrress1, "ACUR" + ul.ToString(), OrdersAssessmentList, OrdeCode, LabList[0].User_Name, LabList[0].Password, objInsurancePlan.Payer_City, objInsurancePlan.Payer_State, objInsurancePlan.Payer_Zip, true, LabList[0].URL, path, out RetrunFilePath); //objOrdersUtilityManager.GenerateABNForQuest(ilstQuestOrderSubmitIDs, true);
                    FilePath = RetrunFilePath;
                    hdnSelectedItem.Value = FilePath;
                }
            }
            FacilityManager objFacilityMgr = new FacilityManager();
            //IList<FacilityLibrary> ilstFacility = objFacilityMgr.GetFacilityByFacilityname(objOrderDTO.ilstOrdersSubmitForPartialOrders[0].Facility_Name);
            IList<FacilityLibrary> ilstFacility = new List<FacilityLibrary>();
            if (objOrderDTO.ilstOrdersSubmitForPartialOrders.Count > 0)
                ilstFacility = objFacilityMgr.GetFacilityByFacilityname(objOrderDTO.ilstOrdersSubmitForPartialOrders[0].Facility_Name);
            string AccountNumberForLabCorp = string.Empty;
            if (ilstFacility.Count > 0)
            {
                AccountNumberForLabCorp = ilstFacility[0].LabCorp_Account_Number;
            }
            else
                AccountNumberForLabCorp = "04281070";
            objAbnResponseType = (AbnResponseType)objResultMasterManager.GenerateABNForm(AccountNumberForLabCorp,sHumanName, sIdentificationNumber, LabCorpCPT.Distinct().ToArray<string>(), LabCorpICD.Distinct().ToArray<string>());
            if (objAbnResponseType.outputType == "Success")
            {
                if ((bool)objAbnResponseType.isABNRequired)
                {
                    if (objAbnResponseType.contentType.ToUpper().Contains("PDF") && objAbnResponseType.abnContents.Length > 0)
                    {
                        RetrunFilePath = string.Empty;
                        RetrunFilePath = path + "\\" + sHumanName + "_" + sIdentificationNumber + "_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf";
                        using (var f = System.IO.File.Create(RetrunFilePath))
                        {
                            f.Write(objAbnResponseType.abnContents, 0, objAbnResponseType.abnContents.Length);
                            hdnSelectedItem.Value = "|" + RetrunFilePath.Replace(Server.MapPath(""), string.Empty);
                            f.Close();
                        }
                        try
                        {

                            IsABNNeed = true;
                            //System.Diagnostics.Process.Start(path);
                            IsABNNeed = true;
                        }
                        catch (Exception k)
                        {
                        }
                    }
                }
                else
                {
                    IsABNNeed = false;
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('230125');", true);
                }
            }
            else if (objAbnResponseType.outputType == "Failure")
            {
                objMessage = new Acurus.Capella.DataAccess.com.labcorp.www4.Message[objAbnResponseType.messages.Count()];
                string err = string.Empty;
                objMessage = objAbnResponseType.messages;
                foreach (Acurus.Capella.DataAccess.com.labcorp.www4.Message msg in objMessage)
                {
                    err += msg.text + System.Environment.NewLine;
                }
                if (err != string.Empty)
                {
                    if (IsABNNeed)
                    {
                        string[] sErr = err.ToString().Split('.');
                        if (sErr[0].ToString() == "Test Code was not found" && !IsABNNeed)
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('230132');", true);
                        }
                        else
                        {
                            //RadMessageBox.Show("LabCorp Response:" + err);
                        }
                    }
                }
            }
            if (!IsABNNeed)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('230132');", true);
            }
            if (hdnSelectedItem.Value != string.Empty)
            {
                if (hdnSelectedItem.Value.ToString().StartsWith("|"))
                {
                    hdnSelectedItem.Value = hdnSelectedItem.Value.ToString().Substring(1);
                }
                //MessageWindow.Width = Unit.Pixel(900);
                //MessageWindow.Height = Unit.Pixel(600);
                //MessageWindow.OnClientClose = "CloseQuestionSetWindow";
                //MessageWindow.NavigateUrl = "frmPrintPDF.aspx?Location=DYNAMIC&SI=" + hdnSelectedItem.Value.ToString();
                //MessageWindow.VisibleStatusbar = false;
                //MessageWindow.VisibleOnPageLoad = true;
                ScriptManager.RegisterStartupScript(this, typeof(frmImageAndLabOrder), string.Empty, "OpenPDFImage();", true);
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }

        protected void btnPlan_Click(object sender, EventArgs e)
        {

        }

        protected void grdOrders_ItemCreated(object sender, GridItemEventArgs e)
        {
            e.Item.ToolTip = "";
            if (e.Item is GridDataItem)
            {
                GridDataItem gridItem = e.Item as GridDataItem;
                foreach (GridColumn column in grdOrders.MasterTableView.RenderColumns)
                {
                    if (column.UniqueName == "FillQuestionSet")
                    {

                    }
                    else if (column.UniqueName == "Edit")
                    {
                        gridItem[column.UniqueName].ToolTip = "Edit";
                    }
                    else if (column.UniqueName == "Del")
                    {
                        gridItem[column.UniqueName].ToolTip = "Delete";
                    }

                }
            }

        }

        protected void btnPrintlabel_Click(object sender, EventArgs e)
        {
            hdnSelectedItem.Value = string.Empty;
            IList<ulong> ilstQuestOrderSubmitIDs = new List<ulong>();
            IList<ulong> ilstQuestOrders = new List<ulong>();
            if (grdOrders.MasterTableView.Items.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Order List", "DisplayErrorMessage('230156'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            else
            {
                for (int i = 0; i < grdOrders.MasterTableView.Items.Count; i++)
                {
                    if (grdOrders.MasterTableView.Items[i].Cells[14].Text.ToString().ToUpper() == "QUEST DIAGNOSTICS" && grdOrders.MasterTableView.Items[i].Cells[7].Text.ToString().ToUpper() == "Y")
                    {
                        ilstQuestOrderSubmitIDs.Add(Convert.ToUInt64(grdOrders.MasterTableView.Items[i].Cells[19].Text));
                    }
                    else if (grdOrders.MasterTableView.Items[i].Cells[14].Text.ToString().ToUpper() == "QUEST DIAGNOSTICS" && grdOrders.MasterTableView.Items[i].Cells[7].Text.ToString().ToUpper() == "N")
                    {
                        ilstQuestOrders.Add(Convert.ToUInt64(grdOrders.MasterTableView.Items[i].Cells[19].Text));
                    }
                }

                if (ilstQuestOrderSubmitIDs.Count > 0)
                {
                    //AgentManager objAgentaManger = new AgentManager();
                    //if (objAgentaManger.GetPrinterStatus())
                    //{
                    string spath = string.Empty;
                    string path = Server.MapPath("Documents/" + Session.SessionID);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    PrintOrders objPrint = new PrintOrders();
                    HumanManager HumanMngr = new HumanManager();
                    string PatientName = HumanMngr.GetPatientNameByHumanID(HumanID);
                    FacilityManager objfacilityProxy = new FacilityManager();
                    string AccountNumberForQuest = string.Empty;
                    IList<FacilityLibrary> ilstFacility = objfacilityProxy.GetFacilityByFacilityname(ClientSession.FacilityName);
                    if (ilstFacility.Count > 0)
                    {
                        AccountNumberForQuest = ilstFacility[0].Quest_Account_Number;
                    }
                    foreach (ulong ul in ilstQuestOrderSubmitIDs)
                    {
                        if (spath == string.Empty)
                        {
                            spath = objPrint.PrintLabelfromOrders(AccountNumberForQuest, ul, PatientName, path, "QUEST DIAGNOSTICS");
                        }
                        else
                        {
                            spath += "|" + objPrint.PrintLabelfromOrders(AccountNumberForQuest, ul, PatientName, path, "QUEST DIAGNOSTICS");
                        }
                        //objAgentaManger.PrintQuestLabel("Client #: " + AccountNumberForQuest + System.Environment.NewLine + "Lab Ref #: " + "ACUR" + ul.ToString() + System.Environment.NewLine + "Patient Name: " + PatientName);
                    }
                    if (spath != string.Empty)
                    {

                        string[] Split = new string[] { Server.MapPath("") };
                        string[] FileName = spath.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < FileName.Length; i++)
                        {
                            if (hdnSelectedItem.Value == string.Empty)
                            {
                                hdnSelectedItem.Value = FileName[i].ToString();
                            }
                            else
                            {
                                hdnSelectedItem.Value += "|" + FileName[i].ToString();
                            }
                        }
                    }
                    if (hdnSelectedItem.Value != string.Empty)
                    {

                        ScriptManager.RegisterStartupScript(this, typeof(frmImageAndLabOrder), string.Empty, "OpenPDFImage();", true);
                    }
                    //}
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                }

                else if (ilstQuestOrders.Count <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Order List", "DisplayErrorMessage('230115'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230141'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
            }

        }
        protected void btnDisableLoad_Click(object sender, EventArgs e)
        {
            RadResultWindow.VisibleOnPageLoad = false;
            MessageWindow.VisibleOnPageLoad = false;
        }
    }
}

