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
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using Telerik.Web.UI;
using System.Runtime.Serialization;
using Acurus.Capella.Core.DTO;
using System.Text;
using System.Drawing;
using System.Globalization;
using System.Xml;


namespace Acurus.Capella.UI
{
    public partial class frmOrder : System.Web.UI.Page
    {
        OrdersManager objOrderManagementMngr = new OrdersManager();
        InHouseProcedureManager objProcedureMngr = new InHouseProcedureManager();
        LabManager objLabproxy = new LabManager();
        LabLocationManager objLabLocProxy = new LabLocationManager();
        IList<Lab> labList = new List<Lab>();
        UtilityManager utilityMgr = new UtilityManager();
        
        string OrderType = "DIAGNOSTIC ORDER";
        string screenName = string.Empty;
        object misValue = System.Reflection.Missing.Value;
        RadComboBoxItem labItem;
        WorkFlowManager workFlowMngr = new WorkFlowManager();
        IList<PhysicianLibrary> phList = new List<PhysicianLibrary>();
        string _OpeningFrom = string.Empty;
        StaticLookupManager objStaticLookupMngr = new StaticLookupManager();
        ImmunizationManager objImmunizationMngr = new ImmunizationManager();
        InHouseProcedureManager objInhouseProcedureMngr = new InHouseProcedureManager();
        ReferralOrderManager objReferralOrder = new ReferralOrderManager();
        DataSet dsreport = new DataSet();
        DataView dv = new DataView();

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                ClientSession.processCheck = false;
                SecurityServiceUtility objSecurityServiceUtility = new SecurityServiceUtility();
                objSecurityServiceUtility.ApplyUserPermissions(this);
                ClientSession.FlushSession();
                PhysicianManager objPhysicianMngr = new PhysicianManager();
                dtpFromDate.Enabled = false;
                dtpToDate.Enabled = false;
                FillOrderType();
                btnAddResult.Enabled = false;
                FacilityManager objFacilityMngr = new FacilityManager();
                //IList<FacilityLibrary> FacilityList = objFacilityMngr.GetFacilityList();


                XmlDocument xmldoc = new XmlDocument();

                string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\Facility_Library.xml");
                if (File.Exists(strXmlFilePath) == true)
                {
                    xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "Facility_Library" + ".xml");
                    XmlNodeList xmlFacilityList = xmldoc.GetElementsByTagName("Facility");
                    cboFacilityName.Items.Add(new RadComboBoxItem("ALL"));
                    if (xmlFacilityList.Count > 0)
                    {
                        foreach (XmlNode item in xmlFacilityList)
                        {
                            if (item != null && item.Attributes.GetNamedItem("Legal_Org").Value == ClientSession.LegalOrg)
                                cboFacilityName.Items.Add(new RadComboBoxItem(item.Attributes[0].Value));
                            //Cap - 534
                            //if (item != null && ClientSession.FacilityName == item.Attributes[0].Value)                              
                            //cboFacilityName.SelectedIndex = cboFacilityName.Items.FindItemIndexByText(item.Attributes[0].Value);                           
                        }                      
                    }
                }


                if (ClientSession.HumanId != 0)
                {
                    if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
                    {
                            txtPatientName.Text = ClientSession.PatientPaneList[0].Last_Name + "," + ClientSession.PatientPaneList[0].First_Name + ClientSession.PatientPaneList[0].MI + ClientSession.PatientPaneList[0].Suffix;
                    }
                    else
                    {
                        HumanManager objHumanMngr = new HumanManager();
                        Human objHuman = new Human();
                        objHuman = objHumanMngr.GetById(ClientSession.HumanId);
                        if (objHuman != null)
                            txtPatientName.Text = objHuman.Last_Name + "," + objHuman.First_Name + "," + objHuman.MI + objHuman.Suffix;
                    }
                    txtPatientName.Attributes["tagPatientName"] = ClientSession.HumanId.ToString();
                    if (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant")
                    {
                        if (ClientSession.FacilityName != string.Empty)
                        {

                            string PersonName = string.Empty;
                            XDocument xmluser = XDocument.Load(HttpContext.Current.Server.MapPath(@"ConfigXML\User.xml"));
                            IEnumerable<XElement> xmluserid = xmluser.Element("UserList")
                                .Elements("User").Where(aa => aa.Attribute("User_Name").Value.ToString() == ClientSession.UserName);
                            if (xmluserid != null && xmluserid.Count() > 0)
                            {
                                if (xmluserid.Attributes("person_name") != null)
                                    PersonName = xmluserid.Attributes("person_name").First().Value.ToString();
                            }

                            txtProviderName.Text = PersonName;
                            txtProviderName.Attributes["tagProviderName"] = ClientSession.PhysicianId.ToString();
                        }
                    }
                }


                lblResultsFound.Text = string.Empty;
                mpnOrderManagement.Reset();
                grdReport.DataSource = new string[] { };
                grdReport.DataBind();
                //Cap - 1007
                RadComboBoxSelectedIndexChangedEventArgs e1 = new RadComboBoxSelectedIndexChangedEventArgs("DIAGNOSTIC ORDER", string.Empty, "DIAGNOSTIC ORDER", string.Empty);
                cboOrderType_SelectedIndexChanged(sender, e1);
            }

            txtPatientName.ReadOnly = true;
            txtProviderName.ReadOnly = true;
            if (hdnResultsLabel.Value != null && hdnResultsLabel.Value != string.Empty)
            {
                lblResultsFound.Text = hdnResultsLabel.Value;
            }
            else
                lblResultsFound.Text = string.Empty;


        }

        private void FillOrderType()
        {
            IList<string> sOrderList = new List<string>();
            //Cap - 1007
            //sOrderList.Add("");
            sOrderList.Add("DIAGNOSTIC ORDER");
            sOrderList.Add("REFERRAL ORDER");
            sOrderList.Add("IMMUNIZATION ORDER");
            sOrderList.Add("PROCEDURES");
            sOrderList.Add("DME ORDER");
            if (sOrderList != null)
            {
                for (int j = 0; j < sOrderList.Count; j++)
                {
                    cboOrderType.Items.Add(new RadComboBoxItem(sOrderList[j]));
                }
                // Cap - 1136
                cboOrderType.Text = "DIAGNOSTIC ORDER";
            }
        }

        private void FillLabCombo()
        {
            cboLabCenter.Items.Clear();
            cboLabCenter.Enabled = true;
            string labType = string.Empty;
            //Cap - 1007
            //if (cboOrderType.Text != string.Empty)
            if (cboOrderType.Items[cboOrderType.SelectedIndex].Text != string.Empty)
            {
                //Cap - 1007
                //labType = cboOrderType.Text.Split(' ')[0];
                labType = cboOrderType.Items[cboOrderType.SelectedIndex].Text.Split(' ')[0];
                if (labType == "DIAGNOSTIC")
                    labType = "LAB";
                //labList = objLabproxy.GetLabDetails(labType);
                //if (labList != null)
                //{
                //    if (labList.Count > 0)
                //    {
                //        cboLabCenter.Items.Add(new RadComboBoxItem(""));
                //        for (int i = 0; i < labList.Count; i++)
                //        {
                //            RadComboBoxItem Item = new RadComboBoxItem();
                //            Item.Text = labList[i].Lab_Name;
                //            Item.Value = labList[i].Id.ToString();
                //            cboLabCenter.Items.Add(Item);
                //        }
                //    }
                //}

                //Get Lab from xml
                XDocument xmlLab = XDocument.Load(Server.MapPath(@"ConfigXML\LabList.xml"));

                //IEnumerable<XElement> xml = xmlLab.Element("LabList")
                //    .Elements("Lab")
                //    .OrderBy(s => (string)s.Attribute("name"));
                IEnumerable<XElement> xml = xmlLab.Element("LabList")
                   .Elements("Lab").Where(a => a.Attribute("type").Value.ToString() == labType)
                   .OrderBy(s => (int)s.Attribute("sort_order"));
                if (xml != null)
                {
                    cboLabCenter.Items.Add(new RadComboBoxItem(""));
                    foreach (XElement LabElement in xml)
                    {
                        RadComboBoxItem Item = new RadComboBoxItem();
                        Item.Text = LabElement.Attribute("name").Value;
                        Item.Value = LabElement.Attribute("id").Value;
                        cboLabCenter.Items.Add(Item);
                    }
                }
            }
        }

        protected void cboOrderType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
           // DataRow row = null;
            grdReport.DataSource = null;
            grdReport.DataBind();
            mpnOrderManagement.Reset();
            if (cboOrderType.SelectedIndex != null)
            {
                if (cboOrderType.Items[cboOrderType.SelectedIndex].Text.ToUpper() == "DIAGNOSTIC ORDER")
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Columns.Add("Control");
                    dt.Columns.Add("Arrival Date");
                    dt.Columns.Add("Order Type");
                    dt.Columns.Add("Order Status");
                    dt.Columns.Add("Patient Acc");
                    dt.Columns.Add("Patient Name");
                    dt.Columns.Add("Patient DOB");
                    dt.Columns.Add("Procedure/Rx/Reason for referral");
                    dt.Columns.Add("ICD");
                    dt.Columns.Add("Ordering Provider");
                    dt.Columns.Add("Attending Provider");
                    dt.Columns.Add("Ordering Facility");
                    dt.Columns.Add("Ref to Facility/Lab/Pharmacy");
                    dt.Columns.Add("Specimen");
                    dt.Columns.Add("LabId");
                    dt.Columns.Add("Print Req.");
                    dt.Columns.Add("Physician ID");
                    dt.Columns.Add("Encounter ID");
                    dt.Columns.Add("Specimen Collected Date Time");
                    dt.Columns.Add("View Result");
                    dt.Columns.Add("GroupID");
                    dt.Columns.Add("Index Order ID");
                    dt.Columns.Add("IsElectronic Signature");
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    grdReport.DataSource = dt;
                    grdReport.DataBind();
                    grdReport.Items[0].Visible = false;
                }
                else if (cboOrderType.Items[cboOrderType.SelectedIndex].Text.ToUpper() == "IMMUNIZATION ORDER")
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Columns.Add("Control");
                    dt.Columns.Add("Arrival Date");
                    dt.Columns.Add("Order Type");
                    dt.Columns.Add("Order Status");
                    dt.Columns.Add("Patient Acc");
                    dt.Columns.Add("Patient Name");
                    dt.Columns.Add("Patient DOB");
                    dt.Columns.Add("Procedure/Rx/Reason for referral");
                    dt.Columns.Add("ICD");
                    dt.Columns.Add("Ordering Provider");
                    dt.Columns.Add("Attending Provider");
                    dt.Columns.Add("Ordering Facility");
                    dt.Columns.Add("Ref to Facility/Lab/Pharmacy");
                    dt.Columns.Add("Specimen");
                    dt.Columns.Add("LabId");
                    dt.Columns.Add("Print Req.");
                    dt.Columns.Add("Physician ID");
                    dt.Columns.Add("Encounter ID");
                    dt.Columns.Add("Specimen Collected Date Time");
                    dt.Columns.Add("View Result");
                    dt.Columns.Add("GroupID");
                    dt.Columns.Add("Temp_Property");
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    grdReport.DataSource = dt;
                    grdReport.DataBind();
                    grdReport.Items[0].Visible = false;
                }
                else if (cboOrderType.Items[cboOrderType.SelectedIndex].Text.ToUpper() == "REFERRAL ORDER")
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Columns.Add("Control");
                    dt.Columns.Add("Arrival Date");
                    dt.Columns.Add("Order Type");
                    dt.Columns.Add("Order Status");
                    dt.Columns.Add("Patient Acc");
                    dt.Columns.Add("Patient Name");
                    dt.Columns.Add("Patient DOB");
                    dt.Columns.Add("Procedure/Rx/Reason for referral");
                    dt.Columns.Add("ICD");
                    dt.Columns.Add("Ordering Provider");
                    dt.Columns.Add("Attending Provider");
                    dt.Columns.Add("Ordering Facility");
                    dt.Columns.Add("Ref to Facility/Lab/Pharmacy");
                    dt.Columns.Add("Specimen");
                    dt.Columns.Add("LabId");
                    dt.Columns.Add("Print Req.");
                    dt.Columns.Add("Physician ID");
                    dt.Columns.Add("Encounter ID");
                    dt.Columns.Add("Specimen Collected Date Time");
                    dt.Columns.Add("View Result");
                    dt.Columns.Add("GroupID");
                    dt.Columns.Add("Temp_Property");
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    grdReport.DataSource = dt;
                    grdReport.DataBind();
                    grdReport.Items[0].Visible = false;
                }
                else if (cboOrderType.Items[cboOrderType.SelectedIndex].Text.ToUpper() == "PROCEDURES")
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Columns.Add("Arrival Date");
                    dt.Columns.Add("Order Type");
                    dt.Columns.Add("Order Status");
                    dt.Columns.Add("Patient Acc");
                    dt.Columns.Add("Patient Name");
                    dt.Columns.Add("Patient DOB");
                    dt.Columns.Add("Procedure/Rx/Reason for referral");
                    dt.Columns.Add("Ordering Provider");
                    dt.Columns.Add("Ordering Facility");
                    dt.Columns.Add("GroupID");
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    grdReport.DataSource = dt;
                    grdReport.DataBind();
                    grdReport.Items[0].Visible = false;
                }
            }
            //IList<string> workflow;
            //workflow = workFlowMngr.GetWorkFlowMapListByLab(cboOrderType.Items[cboOrderType.SelectedIndex].Text, ClientSession.FacilityName);

            cboOrderStatus.Items.Clear();
            lblResultsFound.Text = string.Empty;

            IList<string> ProcessList = new List<string>();
            ProcessList = workFlowMngr.GetWorkFlowMapListByLab(cboOrderType.Items[cboOrderType.SelectedIndex].Text, ClientSession.FacilityName);
            //if (workflow != null)
            //    ProcessList = (from w in workflow select w.To_Process).Distinct().ToList();
            if (ProcessList != null)
            {
                if (ProcessList.Count > 0)
                {
                    cboOrderStatus.Items.Add(new RadComboBoxItem(""));
                }

                for (int i = 0; i < ProcessList.Count; i++)
                {
                    RadComboBoxItem Item = new RadComboBoxItem();
                    Item.Text = ProcessList[i];
                    cboOrderStatus.Items.Add(Item);
                }
            }
            if (cboOrderType.SelectedIndex != null)
            {
                //if (cboOrderType.Items[cboOrderType.SelectedIndex].Text.ToUpper() == "DIAGNOSTIC ORDER" || cboOrderType.Items[cboOrderType.SelectedIndex].Text.ToUpper() == "IMAGE ORDER" || cboOrderType.Items[cboOrderType.SelectedIndex].Text.ToUpper() == "INTERNAL DIAGNOSTIC ORDER" || cboOrderType.Items[cboOrderType.SelectedIndex].Text.ToUpper() == "INTERNAL IMAGE ORDER")
                if (cboOrderType.Items[cboOrderType.SelectedIndex].Text.ToUpper() == "DIAGNOSTIC ORDER" || cboOrderType.Items[cboOrderType.SelectedIndex].Text.ToUpper() == "DME ORDER")
                {
                    FillLabCombo();
                }
                else
                {
                    cboLabCenter.Items.Clear();
                    cboLabCenter.Enabled = false;
                }
            }

            if (cboOrderType.Text.ToUpper() == "DIAGNOSTIC ORDER")
            {
                grdReport.Columns[0].Display = true;
                grdReport.Columns[15].Display = true;
                grdReport.Columns[19].Display = true;
            }
            else if (cboOrderType.Text.ToUpper() == "REFERRAL ORDER" || cboOrderType.Text.ToUpper() == "IMMUNIZATION ORDER")
            {
                grdReport.Columns[0].Display = false;
                grdReport.Columns[15].Display = true;
                grdReport.Columns[19].Display = false;
            }
            else
            {
                grdReport.Columns[0].Display = false;
                grdReport.Columns[15].Display = false;
                grdReport.Columns[19].Display = false;
            }
            if (chkDate.Checked == true)
            {
                dtpFromDate.Enabled = true;
                dtpToDate.Enabled = true;
            }
            else
            {
                dtpFromDate.Enabled = false;
                dtpToDate.Enabled = false;
            }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "OrderType", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        IList<FillOrdersManagementDTO> SearchFillOrderManagement = new List<FillOrdersManagementDTO>();
        IList<FillOrdersManagementDTO> SearchImmunizationList;
        IList<FillOrdersManagementDTO> SearchOtherProcedureList;
        IList<FillOrdersManagementDTO> SearchReferralOrderList;
        DateTime fromdate = new DateTime();
        DateTime todate = new DateTime();
        void GetDateRange(out DateTime fromdate, out DateTime todate)
        {
            if (chkDate.Checked == true)
            {
                fromdate = UtilityManager.ConvertToUniversal(dtpFromDate.SelectedDate.Value);
                todate = UtilityManager.ConvertToUniversal(dtpToDate.SelectedDate.Value);

                if (dtpToDate.SelectedDate.Value.ToString("dd-MMM-yyyy") == Convert.ToDateTime(hdnLocalTime.Value).ToString("dd-MMM-yyyy"))
                    todate = todate.AddDays(2);
                else
                    todate = todate.AddDays(1);
            }
            else
            {
                fromdate = UtilityManager.ConvertToUniversal(dtpFromDate.MinDate);
                todate = Convert.ToDateTime(hdnLocalTime.Value); //UtilityManager.ConvertToUniversal();
                todate = todate.AddDays(2);
            }
        }

        private void Clear()
        {
            chkDate.Checked = false;
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
            mpnOrderManagement.Reset();
            if (_OpeningFrom != "Default to human and physician")
            {
                txtPatientName.Text = string.Empty;
                txtProviderName.Text = string.Empty;
            }

            cboLabCenter.SelectedIndex = 0;
            grdReport.DataSource = new string[] { };
            grdReport.DataBind();
            //grdReport.DataSource = null;
            //grdReport.DataBind();
            lblResultsFound.Text = string.Empty;
            //cboFacilityName.SelectedIndex = cboFacilityName.Items.FindItemIndexByText(ClientSession.FacilityName);
            //cboFacilityName.Text = ClientSession.FacilityName;
            //Cap-534
            cboFacilityName.SelectedIndex = cboFacilityName.Items.FindItemIndexByText("ALL");
            cboFacilityName.Text = "ALL";
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Closepage", "RadWindowClose();", true);
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dtResult = new System.Data.DataTable();
            //dtResult = (System.Data.DataTable)Session["dt"];
            if (Session["SearchOrder"] != null)
            {
                SearchFillOrderManagement = (IList<FillOrdersManagementDTO>)Session["SearchOrder"];
                if (SearchFillOrderManagement.Count > 0)
                {
                    dtResult = PrintOrder(SearchFillOrderManagement, cboOrderType.Text);
                    lblResultsFound.Text = SearchFillOrderManagement.Count.ToString() + " Result(s) found";
                }
            }

            if (dtResult == null)
            {
                return;
            }
            dsreport = new DataSet();

            System.Data.DataTable dtNew = new System.Data.DataTable();


            dtNew = dtResult.Copy();

            dsreport.Tables.Add(dtNew);

            dv = new DataView(dsreport.Tables[0]);
            string filename = "Order_Search_Report_" + UtilityManager.ConvertToLocal(Convert.ToDateTime(hdnLocalTime.Value)).ToString("yyyyMMdd hh mm ss tt") + ".xls";
            if (dsreport.Tables[0].Rows.Count > 0)
            {
                Response.Charset = "UTF-8";
                Response.ContentType = "application/x-msexcel";
                Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                StringBuilder sResponseMessage = new StringBuilder();
                sResponseMessage.Append("<table  border=\"1\" width=\"50%\">");
                int col;
                sResponseMessage.Append("<tr align=\"center\" >");
                if (cboOrderType.Text.ToUpper() == "DIAGNOSTIC ORDER")
                {
                    sResponseMessage.Append("<td colspan=\"12\" align=\"center\"><strong>" + cboOrderType.Text + "</strong></td>");
                    sResponseMessage.Append("<tr align=\"center\" >");
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Order Type : " + "</strong></td>");
                    sResponseMessage.Append("<td colspan=\"4\" align=\"left\">" + cboOrderType.Text);
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Order Status  : " + "</strong></td>");
                    sResponseMessage.Append("<td colspan=\"4\" align=\"left\">" + cboOrderStatus.Text);
                    sResponseMessage.Append("<tr align=\"center\" >");
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Arrival Date  : " + "</strong></td>");
                    if (chkDate.Checked == true)
                    {
                        sResponseMessage.Append("<td colspan=\"4\" align=\"left\">" + dtpFromDate.SelectedDate.Value.ToString("dd - MMM - yyyy") + "  TO  " + dtpToDate.SelectedDate.Value.ToString("dd - MMM - yyyy"));
                    }
                    else
                    {
                        sResponseMessage.Append("<td colspan=\"4\" align=\"left\">");
                    }
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Facility Name  : " + "</strong></td>");
                    sResponseMessage.Append("<td colspan=\"4\" align=\"left\">" + cboFacilityName.Text);
                    if (txtPatientName.Text != string.Empty || txtProviderName.Text != string.Empty)
                    {
                        sResponseMessage.Append("<tr align=\"center\" >");
                        if (txtPatientName.Text != string.Empty)
                        {
                            sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Patient Name  : " + "</strong></td>");
                            sResponseMessage.Append("<td colspan=\"4\" align=\"left\">" + txtPatientName.Text);
                        }
                        if (txtProviderName.Text != string.Empty)
                        {
                            sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Provider  : " + "</strong></td>");
                            sResponseMessage.Append("<td colspan=\"4\" align=\"left\">" + txtProviderName.Text);
                        }
                    }
                    sResponseMessage.Append("<tr align=\"center\" >");
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Lab/Imaging Center  : " + "</strong></td>");
                    sResponseMessage.Append("<td colspan=\"4\" align=\"left\">" + cboLabCenter.Text);
                }
                else if (cboOrderType.Text.ToUpper() == "PROCEDURES")
                {
                    sResponseMessage.Append("<td colspan=\"8\" align=\"center\"><strong>" + cboOrderType.Text + "</strong></td>");
                    sResponseMessage.Append("<tr align=\"center\" >");
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Order Type : " + "</strong></td>");
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + cboOrderType.Text);
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Order Status  : " + "</strong></td>");
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + cboOrderStatus.Text);
                    sResponseMessage.Append("<tr align=\"center\" >");
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Arrival Date  : " + "</strong></td>");
                    if (chkDate.Checked == true)
                    {
                        sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + dtpFromDate.SelectedDate.Value.ToString("dd - MMM - yyyy") + "  TO  " + dtpToDate.SelectedDate.Value.ToString("dd - MMM - yyyy"));
                    }
                    else
                    {
                        sResponseMessage.Append("<td colspan=\"2\" align=\"left\">");
                    }
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Facility Name  : " + "</strong></td>");
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + cboFacilityName.Text);
                    if (txtPatientName.Text != string.Empty || txtProviderName.Text != string.Empty)
                    {
                        sResponseMessage.Append("<tr align=\"center\" >");
                        if (txtPatientName.Text != string.Empty)
                        {
                            sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Patient Name  : " + "</strong></td>");
                            sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + txtPatientName.Text);
                        }
                        if (txtProviderName.Text != string.Empty)
                        {
                            if (txtPatientName.Text != string.Empty)
                            {
                                sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Provider  : " + "</strong></td>");
                                sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + txtProviderName.Text);
                            }
                            else
                            {
                                sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Provider  : " + "</strong></td>");
                                sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + txtProviderName.Text);
                            }
                        }
                    }
                    sResponseMessage.Append("<tr align=\"center\" >");
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Lab/Imaging Center  : " + "</strong></td>");
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\">" + cboLabCenter.Text);
                }
                else
                {
                    sResponseMessage.Append("<td colspan=\"11\" align=\"center\"><strong>" + cboOrderType.Text + "</strong></td>");
                    sResponseMessage.Append("<tr align=\"center\" >");
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Order Type : " + "</strong></td>");
                    sResponseMessage.Append("<td colspan=\"4\" align=\"left\">" + cboOrderType.Text);
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Order Status  : " + "</strong></td>");
                    sResponseMessage.Append("<td colspan=\"3\" align=\"left\">" + cboOrderStatus.Text);
                    sResponseMessage.Append("<tr align=\"center\" >");
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Arrival Date  : " + "</strong></td>");
                    if (chkDate.Checked == true)
                    {
                        sResponseMessage.Append("<td colspan=\"4\" align=\"left\">" + dtpFromDate.SelectedDate.Value.ToString("dd - MMM - yyyy") + "  TO  " + dtpToDate.SelectedDate.Value.ToString("dd - MMM - yyyy"));
                    }
                    else
                    {
                        sResponseMessage.Append("<td colspan=\"4\" align=\"left\">");
                    }
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Facility Name  : " + "</strong></td>");
                    sResponseMessage.Append("<td colspan=\"3\" align=\"left\">" + cboFacilityName.Text);
                    if (txtPatientName.Text != string.Empty || txtProviderName.Text != string.Empty)
                    {
                        sResponseMessage.Append("<tr align=\"center\" >");
                        if (txtPatientName.Text != string.Empty)
                        {
                            sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Patient Name  : " + "</strong></td>");
                            sResponseMessage.Append("<td colspan=\"4\" align=\"left\">" + txtPatientName.Text);
                        }
                        if (txtProviderName.Text != string.Empty)
                        {
                            if (txtPatientName.Text != string.Empty)
                            {
                                sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Provider  : " + "</strong></td>");
                                sResponseMessage.Append("<td colspan=\"3\" align=\"left\">" + txtProviderName.Text);
                            }
                            else
                            {
                                sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Provider  : " + "</strong></td>");
                                sResponseMessage.Append("<td colspan=\"4\" align=\"left\">" + txtProviderName.Text);
                            }
                        }
                    }
                    sResponseMessage.Append("<tr align=\"center\" >");
                    sResponseMessage.Append("<td colspan=\"2\" align=\"left\"><strong>" + " Lab/Imaging Center  : " + "</strong></td>");
                    sResponseMessage.Append("<td colspan=\"4\" align=\"left\">" + cboLabCenter.Text);
                }
                sResponseMessage.Append("<tr align=\"center\" >");
                for (col = 0; col < dv.Table.Columns.Count - 1; col++)
                {
                    if (grdReport.Columns[col].Display == true)
                    {
                        if (dv.Table.Columns[col].ColumnName == "Print Req." || dv.Table.Columns[col].ColumnName == "View Result" || dv.Table.Columns[col].ColumnName == "Order Status")
                        {
                            continue;
                        }
                        else
                        {
                            if (dv.Table.Columns[col].ColumnName == "Control" || dv.Table.Columns[col].ColumnName == "Patient Acc")
                            {
                                dv.Table.Columns[col].ColumnName = dv.Table.Columns[col].ColumnName + " #";
                            }
                            if (cboOrderType.Text.ToUpper() == "PROCEDURES")
                            {
                                if (col == dv.Table.Columns.Count - 2)
                                    sResponseMessage.Append("<td colspan=\"2\" width=\"20%\"><strong>" + dv.Table.Columns[col].ColumnName + "</strong></td>");
                                else
                                    sResponseMessage.Append("<td width=\"20%\"><strong>" + dv.Table.Columns[col].ColumnName + "</strong></td>");
                            }
                            else
                                sResponseMessage.Append("<td width=\"20%\"><strong>" + dv.Table.Columns[col].ColumnName + "</strong></td>");
                        }
                    }
                }
                sResponseMessage.Append("</tr>");
                foreach (DataRowView drv in dv)
                {

                    sResponseMessage.Append("<tr>");
                    for (col = 0; col < dv.Table.Columns.Count - 1; col++)
                    {
                        if (grdReport.Columns[col].Display == true)
                        {
                            if (dv.Table.Columns[col].ColumnName == "Print Req." || dv.Table.Columns[col].ColumnName == "View Result" || dv.Table.Columns[col].ColumnName == "Order Status")
                            {
                                continue;
                            }
                            else
                            {
                                if (dtResult.Columns[col].ToString().Trim() == "Arrival Date" || dtResult.Columns[col].ToString().Trim() == "Patient DOB")
                                {
                                    if (cboOrderType.Text.ToUpper() == "PROCEDURES")
                                    {
                                        if (col == dv.Table.Columns.Count - 2)
                                            sResponseMessage.Append("<td colspan=\"2\" width='50%' align=\"Left\" style='mso-number-format:dd\\/mmm\\/yyyy'>" + drv[col] + "</td>");
                                        else
                                            sResponseMessage.Append("<td width='50%' align=\"Left\" style='mso-number-format:dd\\/mmm\\/yyyy'>" + drv[col] + "</td>");
                                    }
                                    else
                                        sResponseMessage.Append("<td width='50%' align=\"Left\" style='mso-number-format:dd\\/mmm\\/yyyy'>" + drv[col] + "</td>");
                                }
                                else
                                {
                                    if (cboOrderType.Text.ToUpper() == "PROCEDURES")
                                    {
                                        if (col == dv.Table.Columns.Count - 2)
                                            sResponseMessage.Append("<td colspan=\"2\" width='50%' align=\"Left\">" + drv[col].ToString() + "</td>");
                                        else
                                            sResponseMessage.Append("<td width='50%' align=\"Left\">" + drv[col].ToString() + "</td>");
                                    }
                                    else
                                        sResponseMessage.Append("<td width='50%' align=\"Left\">" + drv[col].ToString() + "</td>");
                                }
                            }
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

        protected void btnAddResult_Click(object sender, EventArgs e)
        {
            if (grdReport.SelectedItems != null)
            {
                if (grdReport.SelectedItems.Count > 0)
                {
                    hdnOrder.Value = "false";
                    string OrderSubId = grdReport.SelectedItems[0].Cells[2].Text.Replace("ACUR", "");
                    string CurrentProc = grdReport.SelectedItems[0].Cells[5].Text;
                    string sOrderIDSign = "&IndexOrderID=" + ((GridDataItem)(grdReport.SelectedItems[0]))["IndexOrderID"].Text + "&ElectronicSign=" + ((GridDataItem)(grdReport.SelectedItems[0]))["IsElectronicSignature"].Text;
                    OrderManagementWindow.NavigateUrl = "frmExamPhotos.aspx?type=Result Upload" + "&CurrentProcess=" + CurrentProc + "&OrderSubmit_ID=" + OrderSubId + "&hdnOrder=" + hdnOrder.Value + "&IsCmg=IsCmg" + sOrderIDSign;
                    OrderManagementWindow.OnClientClose = "CloseImportResult";
                    OrderManagementWindow.Width = 850;
                    OrderManagementWindow.Height = 400;
                    OrderManagementWindow.VisibleOnPageLoad = true;
                    OrderManagementWindow.VisibleTitlebar = true;
                    OrderManagementWindow.VisibleStatusbar = false;
                    OrderManagementWindow.Behaviors = WindowBehaviors.Close;
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (chkDate.Checked == true)
            {
                dtpFromDate.Enabled = true;
                dtpToDate.Enabled = true;
            }
            btnAddResult.Enabled = false;
            string sHumanID = string.Empty;
            // Cap - 1007 - Set default order.
            //if (cboOrderType.SelectedIndex == 0 || cboOrderType.SelectedIndex < 0)
            //{
            //    grdReport.DataSource = new string[] { };
            //    grdReport.DataBind();
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('7090003');", true);
            //    lblResultsFound.Text = "No Result(s) found";
            //    hdnResultsLabel.Value = "No Result(s) found";
            //    return;
            //}
            //else if (cboOrderType.SelectedIndex != 0)
            //{
                if (chkDate.Checked == true && dtpToDate.SelectedDate < dtpFromDate.SelectedDate)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('7090008');", true);
                    lblResultsFound.Text = "No Result(s) found";
                    hdnResultsLabel.Value = "No Result(s) found";
                    dtpToDate.Focus();
                    return;
                }
            // Cap - 1007 - Set default order.
            //if (cboOrderType.SelectedIndex != 0 && cboLabCenter.Text.Trim() != string.Empty)
            if(cboLabCenter.Text.Trim() != string.Empty)
            {
                labItem = cboLabCenter.SelectedItem as RadComboBoxItem;
            }
            else
            {
                labItem = new RadComboBoxItem();
                int i = 0;
                labItem.Value = i.ToString();
            }

            if (txtPatientName.Text == string.Empty)
                {
                    int i = 0;
                    txtPatientName.Attributes["tagPatientName"] = i.ToString();
                    sHumanID = i.ToString();
                }
                else
                {
                    if (hdnPatientValues != null && hdnPatientValues.Value.Trim() != "")
                    {
                        txtPatientName.Attributes["tagPatientName"] = hdnPatientValues.Value;
                        sHumanID = hdnPatientValues.Value;
                    }
                    else
                    {
                        sHumanID = ClientSession.HumanId.ToString();
                    }

                }
                if (txtProviderName.Text == string.Empty)
                {
                    int i = 0;
                    txtProviderName.Attributes["tagProviderName"] = i.ToString();
                }
                else
                {
                    if (hdnTransferVaraible != null && hdnTransferVaraible.Value.Trim() != "")
                    {
                        txtProviderName.Attributes["tagProviderName"] = hdnTransferVaraible.Value;
                    }
                    else
                    {

                        txtProviderName.Attributes["tagProviderName"] = Convert.ToString(ClientSession.CurrentPhysicianId);
                    }
                }
                if (chkDate.Checked == true)
                {
                    if (dtpFromDate.SelectedDate.ToString() == "")
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('7090011');", true);
                        lblResultsFound.Text = "No Result(s) found";
                        hdnResultsLabel.Value = "No Result(s) found";
                        return;
                    }
                    if (dtpToDate.SelectedDate.ToString() == "")
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('7090012');", true);
                        lblResultsFound.Text = "No Result(s) found";
                        hdnResultsLabel.Value = "No Result(s) found";
                        return;
                    }
                }
                GetDateRange(out fromdate, out todate);
                Session.Remove("SearchOrder");
                mpnOrderManagement.PageNumber = 1;
                //Cap - 534
                String sFacilityName = string.Empty;
                if(cboFacilityName.Text != "ALL")
                {
                    sFacilityName = cboFacilityName.Text;
                }
                else
                {
                    sFacilityName = "%";
                }

                //if (cboOrderType.Text.ToUpper() == "DIAGNOSTIC ORDER" || cboOrderType.Text.ToUpper() == "IMAGE ORDER" || cboOrderType.Text.ToUpper() == "INTERNAL DIAGNOSTIC ORDER" || cboOrderType.Text.ToUpper() == "INTERNAL IMAGE ORDER")
                if (cboOrderType.Text.ToUpper() == "DIAGNOSTIC ORDER" || cboOrderType.Text.ToUpper() == "DME ORDER")
                {
                    //SearchFillOrderManagement = objOrderManagementMngr.SearchOrder(cboOrderType.Text, cboOrderStatus.Text, cboFacilityName.Text, 0, fromdate, todate, Convert.ToUInt64(sHumanID), Convert.ToUInt64(txtProviderName.Attributes["tagProviderName"]), Convert.ToUInt64(labItem.Value), Convert.ToUInt64(0), mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
                    //FillSearchResultLabAndImage(SearchFillOrderManagement);
                    //Cap - 534
                    //SearchFillOrderManagement = objOrderManagementMngr.SearchOrder(cboOrderType.Text, cboOrderStatus.Text, cboFacilityName.Text, 0, fromdate, todate, Convert.ToUInt64(sHumanID), Convert.ToUInt64(txtProviderName.Attributes["tagProviderName"]), Convert.ToUInt64(labItem.Value), Convert.ToUInt64(0), mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
                    SearchFillOrderManagement = objOrderManagementMngr.SearchOrder(cboOrderType.Text, cboOrderStatus.Text, sFacilityName, 0, fromdate, todate, Convert.ToUInt64(sHumanID), Convert.ToUInt64(txtProviderName.Attributes["tagProviderName"]), Convert.ToUInt64(labItem.Value), Convert.ToUInt64(0), mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
                    if (SearchFillOrderManagement != null)
                    {
                        FillSearchResultLabAndImage(SearchFillOrderManagement, mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
                        Session["SearchOrder"] = SearchFillOrderManagement;
                    }
                }
                else if (cboOrderType.Text.ToUpper() == "IMMUNIZATION ORDER")
                {
                    //SearchImmunizationList = objImmunizationMngr.SearchImmunizationOrder(cboOrderType.Text, cboOrderStatus.Text, cboFacilityName.Text, 0, fromdate, todate, Convert.ToUInt64(sHumanID), Convert.ToUInt64(txtProviderName.Attributes["tagProviderName"]), mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
                    //FillSearchResultImmunization(SearchImmunizationList);
                    //Cap - 534
                    //SearchImmunizationList = objImmunizationMngr.SearchImmunizationOrder(cboOrderType.Text, cboOrderStatus.Text, cboFacilityName.Text, 0, fromdate, todate, Convert.ToUInt64(sHumanID), Convert.ToUInt64(txtProviderName.Attributes["tagProviderName"]), mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
                    SearchImmunizationList = objImmunizationMngr.SearchImmunizationOrder(cboOrderType.Text, cboOrderStatus.Text, sFacilityName, 0, fromdate, todate, Convert.ToUInt64(sHumanID), Convert.ToUInt64(txtProviderName.Attributes["tagProviderName"]), mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
                    if (SearchImmunizationList != null)
                    {
                        FillSearchResultImmunization(SearchImmunizationList, mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
                        Session["SearchOrder"] = SearchImmunizationList;
                    }
                }
                else if (cboOrderType.Text.ToUpper() == "REFERRAL ORDER")
                {
                    //SearchReferralOrderList = objReferralOrder.SearchReferralOrder(0, fromdate, todate, Convert.ToUInt64(sHumanID), Convert.ToUInt64(txtProviderName.Attributes["tagProviderName"]), mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage, cboOrderType.Text, cboOrderStatus.Text, cboFacilityName.Text);
                    //FillSearchResultReferral(SearchReferralOrderList);
                    //Cap - 534
                    //SearchReferralOrderList = objReferralOrder.SearchReferralOrder(0, fromdate, todate, Convert.ToUInt64(sHumanID), Convert.ToUInt64(txtProviderName.Attributes["tagProviderName"]), mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage, cboOrderType.Text, cboOrderStatus.Text, cboFacilityName.Text);
                    SearchReferralOrderList = objReferralOrder.SearchReferralOrder(0, fromdate, todate, Convert.ToUInt64(sHumanID), Convert.ToUInt64(txtProviderName.Attributes["tagProviderName"]), mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage, cboOrderType.Text, cboOrderStatus.Text, sFacilityName);
                    if (SearchReferralOrderList != null)
                    {
                        FillSearchResultReferral(SearchReferralOrderList, mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
                        Session["SearchOrder"] = SearchReferralOrderList;
                    }
                }
                else if (cboOrderType.Text.ToUpper() == "PROCEDURES")
                {
                    //SearchOtherProcedureList = objProcedureMngr.SearchProcedureOrder(cboOrderType.Text, cboOrderStatus.Text, cboFacilityName.Text, 0, fromdate, todate, Convert.ToUInt64(sHumanID), Convert.ToUInt64(txtProviderName.Attributes["tagProviderName"]), mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
                    //FillSearchResultOtherProcedure(SearchOtherProcedureList);
                    //Cap - 534
                    //SearchOtherProcedureList = objProcedureMngr.SearchProcedureOrder(cboOrderType.Text, cboOrderStatus.Text, cboFacilityName.Text, 0, fromdate, todate, Convert.ToUInt64(sHumanID), Convert.ToUInt64(txtProviderName.Attributes["tagProviderName"]), mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
                    SearchOtherProcedureList = objProcedureMngr.SearchProcedureOrder(cboOrderType.Text, cboOrderStatus.Text, sFacilityName, 0, fromdate, todate, Convert.ToUInt64(sHumanID), Convert.ToUInt64(txtProviderName.Attributes["tagProviderName"]), mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
                    if (SearchOtherProcedureList != null)
                    {
                        FillSearchResultOtherProcedure(SearchOtherProcedureList, mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
                        Session["SearchOrder"] = SearchOtherProcedureList;
                    }
                }
                if ((SearchFillOrderManagement != null && SearchFillOrderManagement.Count > 0) || (SearchReferralOrderList != null && SearchReferralOrderList.Count > 0) || (SearchImmunizationList != null && SearchImmunizationList.Count > 0) || (SearchOtherProcedureList != null && SearchOtherProcedureList.Count > 0))
                {
                    lblResultsFound.Text = mpnOrderManagement.TotalNoofDBRecords.ToString() + " Result(s) found";
                    hdnResultsLabel.Value = mpnOrderManagement.TotalNoofDBRecords.ToString() + " Result(s) found";
                }
                else
                {
                    lblResultsFound.Text = "No Result(s) found";
                    hdnResultsLabel.Value = "No Result(s) found";
                    mpnOrderManagement.Reset();
                }
            //}

            ScriptManager.RegisterStartupScript(this, this.GetType(), "SearchOrders", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //divLoading.Style.Add("display", "none");
            if (chkDate.Checked == false)
            {
                dtpFromDate.Clear();
                dtpToDate.Clear();
            }
        }
        public System.Data.DataTable PrintOrder(IList<FillOrdersManagementDTO> FillOrderManagementSearch, string orderType)
        {
            System.Data.DataTable dt = null;
            if (orderType.ToUpper() == "DIAGNOSTIC ORDER" || orderType.ToUpper() == "DME ORDER")
            {
                dt = new System.Data.DataTable();
                dt.Columns.Add("Control");
                dt.Columns.Add("Arrival Date");
                dt.Columns.Add("Order Type");
                dt.Columns.Add("Order Status");
                dt.Columns.Add("Patient Acc");
                dt.Columns.Add("Patient Name");
                dt.Columns.Add("Patient DOB");
                dt.Columns.Add("Procedure/Rx/Reason for referral");
                dt.Columns.Add("ICD");
                dt.Columns.Add("Ordering Provider");
                dt.Columns.Add("Attending Provider");
                dt.Columns.Add("Ordering Facility");
                dt.Columns.Add("Ref to Facility/Lab/Pharmacy");
                dt.Columns.Add("Specimen");
                dt.Columns.Add("LabId");
                dt.Columns.Add("Print Req.");
                dt.Columns.Add("Physician ID");
                dt.Columns.Add("Encounter ID");
                dt.Columns.Add("Specimen Collected Date Time");
                dt.Columns.Add("View Result");
                dt.Columns.Add("GroupID");
                dt.Columns.Add("Index Order ID");
                dt.Columns.Add("IsElectronic Signature");

                DataRow row = null;
                if (FillOrderManagementSearch != null)
                {
                    for (int i = 0; i < FillOrderManagementSearch.Count; i++)
                    {

                        row = dt.NewRow();
                        if (row != null)
                        {
                            row["Ref to Facility/Lab/Pharmacy"] = FillOrderManagementSearch[i].Lab_Name;
                            if (cboOrderType.Text.Trim().ToUpper().Contains("INTERNAL") == false)
                            {
                                row["GroupID"] = FillOrderManagementSearch[i].Group_ID;
                                row["Order Status"] = FillOrderManagementSearch[i].Current_Process;
                                row["Control"] = "ACUR" + FillOrderManagementSearch[i].Group_ID.ToString();
                            }

                            row["Arrival Date"] = UtilityManager.ConvertToLocal(FillOrderManagementSearch[i].Ordered_Date_And_Time).ToString("dd-MMM-yyyy");
                            row["Order Type"] = cboOrderType.Text;
                            row["Patient Acc"] = FillOrderManagementSearch[i].Human_Id;
                            row["Patient Name"] = FillOrderManagementSearch[i].Human_Name;
                            row["Patient DOB"] = FillOrderManagementSearch[i].Date_Of_Birth.ToString("dd-MMM-yyyy");
                            row["Procedure/Rx/Reason for referral"] = FillOrderManagementSearch[i].Procedures;
                            row["ICD"] = FillOrderManagementSearch[i].Assessment;
                            row["Ordering Provider"] = FillOrderManagementSearch[i].Ordering_Provider;
                            row["Ordering Facility"] = FillOrderManagementSearch[i].Facility_Name;
                            row["LabId"] = FillOrderManagementSearch[i].Lab_ID;
                            row["Encounter ID"] = FillOrderManagementSearch[i].Encounter_ID;
                            row["Physician ID"] = FillOrderManagementSearch[i].Physician_ID;
                            row["Specimen Collected Date Time"] = FillOrderManagementSearch[i].Specimen_Collected_Date_Time;
                            row["Index Order ID"] = FillOrderManagementSearch[i].File_Management_Index_Order_ID;
                            row["IsElectronic Signature"] = FillOrderManagementSearch[i].Is_Electronic_Result_Available;
                        }
                        dt.Rows.Add(row);
                    }
                }
            }
            else if (orderType.ToUpper() == "IMMUNIZATION ORDER")
            {
                dt = new System.Data.DataTable();
                dt.Columns.Add("Control");
                dt.Columns.Add("Arrival Date");
                dt.Columns.Add("Order Type");
                dt.Columns.Add("Order Status");
                dt.Columns.Add("Patient Acc");
                dt.Columns.Add("Patient Name");
                dt.Columns.Add("Patient DOB");
                dt.Columns.Add("Procedure/Rx/Reason for referral");
                dt.Columns.Add("ICD");
                dt.Columns.Add("Ordering Provider");
                dt.Columns.Add("Attending Provider");
                dt.Columns.Add("Ordering Facility");
                dt.Columns.Add("Ref to Facility/Lab/Pharmacy");
                dt.Columns.Add("Specimen");
                dt.Columns.Add("LabId");
                dt.Columns.Add("Print Req.");
                dt.Columns.Add("Physician ID");
                dt.Columns.Add("Encounter ID");
                dt.Columns.Add("Specimen Collected Date Time");
                dt.Columns.Add("View Result");
                dt.Columns.Add("GroupID");
                dt.Columns.Add("Temp_Property");

                DataRow row = null;
                if (FillOrderManagementSearch != null)
                {
                    for (int i = 0; i < FillOrderManagementSearch.Count; i++)
                    {

                        row = dt.NewRow();
                        row["Temp_Property"] = FillOrderManagementSearch[i].Temp_Property;
                        //CAP-2318
                        string ordered_Date_And_Time = UtilityManager.ConvertToLocal(FillOrderManagementSearch[i].Ordered_Date_And_Time).ToString("dd-MMM-yyyy");
                        row["Arrival Date"] = ordered_Date_And_Time == "01-Jan-0001" ? "" : ordered_Date_And_Time;
                        row["Order Type"] = cboOrderType.Text;
                        row["Order Status"] = FillOrderManagementSearch[i].Current_Process;
                        row["Patient Acc"] = FillOrderManagementSearch[i].Human_Id;
                        row["Patient Name"] = FillOrderManagementSearch[i].Human_Name;
                        row["Patient DOB"] = FillOrderManagementSearch[i].Date_Of_Birth.ToString("dd-MMM-yyyy");
                        row["Procedure/Rx/Reason for referral"] = FillOrderManagementSearch[i].Procedures;
                        row["ICD"] = string.Empty;
                        row["Ordering Provider"] = FillOrderManagementSearch[i].Ordering_Provider;
                        row["Ordering Facility"] = FillOrderManagementSearch[i].Facility_Name;
                        row["Ref to Facility/Lab/Pharmacy"] = string.Empty;
                        dt.Rows.Add(row);

                    }
                }
            }
            else if (orderType.ToUpper() == "REFERRAL ORDER")
            {
                dt = new System.Data.DataTable();
                dt.Columns.Add("Control");
                dt.Columns.Add("Arrival Date");
                dt.Columns.Add("Order Type");
                dt.Columns.Add("Order Status");
                dt.Columns.Add("Patient Acc");
                dt.Columns.Add("Patient Name");
                dt.Columns.Add("Patient DOB");
                dt.Columns.Add("Procedure/Rx/Reason for referral");
                dt.Columns.Add("ICD");
                dt.Columns.Add("Ordering Provider");
                dt.Columns.Add("Attending Provider");
                dt.Columns.Add("Ordering Facility");
                dt.Columns.Add("Ref to Facility/Lab/Pharmacy");
                dt.Columns.Add("Specimen");
                dt.Columns.Add("LabId");
                dt.Columns.Add("Print Req.");
                dt.Columns.Add("Physician ID");
                dt.Columns.Add("Encounter ID");
                dt.Columns.Add("Specimen Collected Date Time");
                dt.Columns.Add("View Result");
                dt.Columns.Add("GroupID");
                dt.Columns.Add("Temp_Property");
                DataRow row = null;
                if (FillOrderManagementSearch != null)
                {
                    for (int i = 0; i < FillOrderManagementSearch.Count; i++)
                    {
                        row = dt.NewRow();

                        row["Temp_Property"] = FillOrderManagementSearch[i].Temp_Property;
                        //CAP-2318
                        string ordered_Date_And_Time = UtilityManager.ConvertToLocal(FillOrderManagementSearch[i].Ordered_Date_And_Time).ToString("dd-MMM-yyyy");
                        row["Arrival Date"] = ordered_Date_And_Time == "01-Jan-0001" ? "" : ordered_Date_And_Time;
                        row["Order Type"] = cboOrderType.Text;
                        row["Patient Acc"] = FillOrderManagementSearch[i].Human_Id;
                        row["Patient Name"] = FillOrderManagementSearch[i].Human_Name;
                        row["Patient DOB"] = FillOrderManagementSearch[i].Date_Of_Birth.ToString("dd-MMM-yyyy");
                        row["Procedure/Rx/Reason for referral"] = FillOrderManagementSearch[i].Procedures;
                        row["ICD"] = FillOrderManagementSearch[i].Assessment;
                        row["Ordering Provider"] = FillOrderManagementSearch[i].Ordering_Provider;
                        row["Attending Provider"] = FillOrderManagementSearch[i].To_Physician_Name;
                        row["Ref to Facility/Lab/Pharmacy"] = FillOrderManagementSearch[i].To_Facility_Name;
                        row["Order Status"] = FillOrderManagementSearch[i].Current_Process;
                        dt.Rows.Add(row);
                    }
                }
            }
            else if (orderType.ToUpper() == "PROCEDURES")
            {
                dt = new System.Data.DataTable();
                dt.Columns.Add("Arrival Date");
                dt.Columns.Add("Order Type");
                dt.Columns.Add("Patient Acc");
                dt.Columns.Add("Patient Name");
                dt.Columns.Add("Patient DOB");
                dt.Columns.Add("Procedure/Rx/Reason for referral");
                dt.Columns.Add("Ordering Provider");
                dt.Columns.Add("Ordering Facility");
                dt.Columns.Add("GroupID");

                DataRow row = null;
                if (FillOrderManagementSearch != null)
                {
                    for (int i = 0; i < FillOrderManagementSearch.Count; i++)
                    {
                        row = dt.NewRow();

                        row["Arrival Date"] = UtilityManager.ConvertToLocal(FillOrderManagementSearch[i].Ordered_Date_And_Time).ToString("dd-MMM-yyyy");
                        row["Order Type"] = cboOrderType.Text;
                        row["Patient Acc"] = FillOrderManagementSearch[i].Human_Id;
                        row["Patient Name"] = FillOrderManagementSearch[i].Human_Name;
                        row["Patient DOB"] = FillOrderManagementSearch[i].Date_Of_Birth.ToString("dd-MMM-yyyy");
                        row["Procedure/Rx/Reason for referral"] = FillOrderManagementSearch[i].Procedures;
                        row["Ordering Provider"] = FillOrderManagementSearch[i].To_Physician_Name;
                        row["Ordering Facility"] = FillOrderManagementSearch[i].Facility_Name;
                        row["GroupID"] = FillOrderManagementSearch[i].Group_ID;
                        dt.Rows.Add(row);
                    }
                }
            }

            return dt;
        }

        private void FillSearchResultLabAndImage(IList<FillOrdersManagementDTO> FillOrderManagementSearch, int pageNumber, int maxResults)
        {

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Control");
            dt.Columns.Add("Arrival Date");
            dt.Columns.Add("Order Type");
            dt.Columns.Add("Order Status");
            dt.Columns.Add("Patient Acc");
            dt.Columns.Add("Patient Name");
            dt.Columns.Add("Patient DOB");
            dt.Columns.Add("Procedure/Rx/Reason for referral");
            dt.Columns.Add("ICD");
            dt.Columns.Add("Ordering Provider");
            dt.Columns.Add("Attending Provider");
            dt.Columns.Add("Ordering Facility");
            dt.Columns.Add("Ref to Facility/Lab/Pharmacy");
            dt.Columns.Add("Specimen");
            dt.Columns.Add("LabId");
            dt.Columns.Add("Print Req.");
            dt.Columns.Add("Physician ID");
            dt.Columns.Add("Encounter ID");
            dt.Columns.Add("Specimen Collected Date Time");
            dt.Columns.Add("View Result");
            dt.Columns.Add("GroupID");
            dt.Columns.Add("Index Order ID");
            dt.Columns.Add("IsElectronic Signature");

            grdReport.DataSource = null;
            grdReport.DataBind();
            DataRow row = null;
            if (FillOrderManagementSearch != null)
            {
                if (FillOrderManagementSearch.Count != 0)
                    mpnOrderManagement.TotalNoofDBRecords = Convert.ToInt32(FillOrderManagementSearch[0].Total_Record_Found);


                //for (int i = 0; i < FillOrderManagementSearch.Count; i++)
                for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > FillOrderManagementSearch.Count ? FillOrderManagementSearch.Count : (maxResults * pageNumber)); i++)
                {

                    row = dt.NewRow();
                    if (row != null)
                    {
                        row["Ref to Facility/Lab/Pharmacy"] = FillOrderManagementSearch[i].Lab_Name;
                        if (cboOrderType.Text.Trim().ToUpper().Contains("INTERNAL") == false)
                        {
                            row["GroupID"] = FillOrderManagementSearch[i].Group_ID;
                            row["Order Status"] = FillOrderManagementSearch[i].Current_Process;
                            row["Control"] = "ACUR" + FillOrderManagementSearch[i].Group_ID.ToString();
                        }

                        row["Arrival Date"] = UtilityManager.ConvertToLocal(FillOrderManagementSearch[i].Ordered_Date_And_Time).ToString("dd-MMM-yyyy");
                        row["Order Type"] = cboOrderType.Text;
                        row["Patient Acc"] = FillOrderManagementSearch[i].Human_Id;
                        row["Patient Name"] = FillOrderManagementSearch[i].Human_Name;
                        row["Patient DOB"] = FillOrderManagementSearch[i].Date_Of_Birth.ToString("dd-MMM-yyyy");
                        row["Procedure/Rx/Reason for referral"] = FillOrderManagementSearch[i].Procedures;
                        row["ICD"] = FillOrderManagementSearch[i].Assessment;
                        row["Ordering Provider"] = FillOrderManagementSearch[i].Ordering_Provider;
                        row["Ordering Facility"] = FillOrderManagementSearch[i].Facility_Name;
                        row["LabId"] = FillOrderManagementSearch[i].Lab_ID;
                        row["Encounter ID"] = FillOrderManagementSearch[i].Encounter_ID;
                        row["Physician ID"] = FillOrderManagementSearch[i].Physician_ID;
                        row["Specimen Collected Date Time"] = FillOrderManagementSearch[i].Specimen_Collected_Date_Time;
                        row["Index Order ID"] = FillOrderManagementSearch[i].File_Management_Index_Order_ID;
                        row["IsElectronic Signature"] = FillOrderManagementSearch[i].Is_Electronic_Result_Available;
                    }
                    dt.Rows.Add(row);
                }
            }
            //ViewState["dt"] = dt;
            Session["dt"] = dt;

            grdReport.DataSource = dt;
            grdReport.DataBind();
            //foreach (GridDataItem item in grdReport.Items)
            //{
            //    if (item["ReftoFacility/Pharmacy"].Text.ToString().Contains("LabCorp") || item["ReftoFacility/Pharmacy"].Text.ToString().Contains("Quest"))
            //    {
            //        TableCell selectCell = item["PrintEReq"];
            //        ImageButton gd = (ImageButton)selectCell.Controls[0];
            //        gd.ID = "PrintReq";
            //        gd.ImageUrl = "~/Resources/PrintReq.png";
            //    }
            //    else
            //    {
            //        TableCell selectCell = item["PrintEReq"];
            //        ImageButton gd = (ImageButton)selectCell.Controls[0];
            //        gd.ID = "PrintReqDis";
            //        gd.ImageUrl = "~/Resources/PrintReq_Disabled.png";
            //    }
            //    if (item["IndexOrderID"].Text == string.Empty || (Convert.ToInt32(item["IndexOrderID"].Text) == 0 && item["IsElectronicSignature"].Text == "False"))
            //    {
            //        TableCell selectCell = item["ViewResult"];
            //        ImageButton gd = (ImageButton)selectCell.Controls[0];
            //        gd.ImageUrl = "~/Resources/Down_Disabled.bmp";
            //    }
            //    else
            //    {

            //        TableCell selectCell = item["ViewResult"];
            //        ImageButton gd = (ImageButton)selectCell.Controls[0];
            //        gd.ImageUrl = "~/Resources/Down.bmp";
            //    }
            //    if (item["Assessment"].Text.Trim() == string.Empty)
            //    {
            //        TableCell selectCell = item["PrintEReq"];
            //        ImageButton gd = (ImageButton)selectCell.Controls[0];
            //        gd.ID = "PrintReqDis";
            //        gd.ImageUrl = "~/Resources/PrintReq_Disabled.png";
            //    }
            //}

        }

        private void FillSearchResultOtherProcedure(IList<FillOrdersManagementDTO> FillOrderManagementSearch, int pageNumber, int maxResults)
        {

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Arrival Date");
            dt.Columns.Add("Order Type");
            //dt.Columns.Add("Order Status");
            dt.Columns.Add("Patient Acc");
            dt.Columns.Add("Patient Name");
            dt.Columns.Add("Patient DOB");
            dt.Columns.Add("Procedure/Rx/Reason for referral");
            dt.Columns.Add("Ordering Provider");
            dt.Columns.Add("Ordering Facility");
            dt.Columns.Add("GroupID");

            grdReport.DataSource = null;
            grdReport.DataBind();
            DataRow row = null;



            //Changed On 23-11-2012 by Saravanakumar
            if (FillOrderManagementSearch != null)
            {
                if (mpnOrderManagement.PageNumber == 1 && FillOrderManagementSearch.Count > 0)
                    mpnOrderManagement.TotalNoofDBRecords = Convert.ToInt32(FillOrderManagementSearch[0].Total_Record_Found);
                //if (phList.Count == 0)
                //phList = Phyproxy.GetPhysicianList();
                //for (int i = 0; i < FillOrderManagementSearch.Count; i++)
                for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > FillOrderManagementSearch.Count ? FillOrderManagementSearch.Count : (maxResults * pageNumber)); i++)
                {
                    row = dt.NewRow();

                    row["Arrival Date"] = UtilityManager.ConvertToLocal(FillOrderManagementSearch[i].Ordered_Date_And_Time).ToString("dd-MMM-yyyy");
                    row["Order Type"] = cboOrderType.Text;

                    //row["Order Status"] = FillOrderManagementSearch[i].Current_Process;
                    row["Patient Acc"] = FillOrderManagementSearch[i].Human_Id;
                    row["Patient Name"] = FillOrderManagementSearch[i].Human_Name;
                    row["Patient DOB"] = FillOrderManagementSearch[i].Date_Of_Birth.ToString("dd-MMM-yyyy");
                    //Latha - Main - 30 Jul 2011 - End
                    row["Procedure/Rx/Reason for referral"] = FillOrderManagementSearch[i].Procedures;
                    // row["Assessment"] = string.Empty;
                    //IList<PhysicianLibrary> ph = (from p in phList
                    //                              where p.Id == FillOrderManagementSearch[i].Physician_ID
                    //                              select p).ToList();
                    //if (ph != null)
                    //{
                    //row["Ordering Provider"] = ph[0].PhyPrefix + " " + ph[0].PhyFirstName + " " + ph[0].PhyMiddleName + " " + ph[0].PhyLastName + " " + ph[0].PhySuffix;
                    //}
                    row["Ordering Provider"] = FillOrderManagementSearch[i].To_Physician_Name;
                    row["Ordering Facility"] = FillOrderManagementSearch[i].Facility_Name;
                    row["GroupID"] = FillOrderManagementSearch[i].Group_ID;
                    dt.Rows.Add(row);
                }
            }
            Session["dt"] = dt;

            grdReport.DataSource = dt;
            grdReport.DataBind();
        }

        private void FillSearchResultImmunization(IList<FillOrdersManagementDTO> FillOrderManagementSearch, int pageNumber, int maxResults)
        {

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Control");
            dt.Columns.Add("Arrival Date");
            dt.Columns.Add("Order Type");
            dt.Columns.Add("Order Status");
            dt.Columns.Add("Patient Acc");
            dt.Columns.Add("Patient Name");
            dt.Columns.Add("Patient DOB");
            dt.Columns.Add("Procedure/Rx/Reason for referral");
            dt.Columns.Add("ICD");
            dt.Columns.Add("Ordering Provider");
            dt.Columns.Add("Attending Provider");
            dt.Columns.Add("Ordering Facility");
            dt.Columns.Add("Ref to Facility/Lab/Pharmacy");
            dt.Columns.Add("Specimen");
            dt.Columns.Add("LabId");
            dt.Columns.Add("Print Req.");
            dt.Columns.Add("Physician ID");
            dt.Columns.Add("Encounter ID");
            dt.Columns.Add("Specimen Collected Date Time");
            dt.Columns.Add("View Result");
            dt.Columns.Add("GroupID");
            dt.Columns.Add("Temp_Property");

            DataRow row = null;
            grdReport.DataSource = null;
            grdReport.DataBind();
            //Changed On 23-11-2012 by Saravanakumar
            if (FillOrderManagementSearch != null)
            {
                if (mpnOrderManagement.PageNumber == 1 && FillOrderManagementSearch.Count > 0)
                    mpnOrderManagement.TotalNoofDBRecords = Convert.ToInt32(FillOrderManagementSearch[0].Total_Record_Found);

                //for (int i = 0; i < FillOrderManagementSearch.Count; i++)
                for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > FillOrderManagementSearch.Count ? FillOrderManagementSearch.Count : (maxResults * pageNumber)); i++)
                {

                    row = dt.NewRow();
                    row["Temp_Property"] = FillOrderManagementSearch[i].Temp_Property;
                    //CAP-2299
                    string ordered_Date_And_Time = UtilityManager.ConvertToLocal(FillOrderManagementSearch[i].Ordered_Date_And_Time).ToString("dd-MMM-yyyy");
                    row["Arrival Date"] = ordered_Date_And_Time == "01-Jan-0001" ? "" : ordered_Date_And_Time;
                    row["Order Type"] = cboOrderType.Text;
                    row["Order Status"] = FillOrderManagementSearch[i].Current_Process;
                    row["Patient Acc"] = FillOrderManagementSearch[i].Human_Id;
                    row["Patient Name"] = FillOrderManagementSearch[i].Human_Name;
                    row["Patient DOB"] = FillOrderManagementSearch[i].Date_Of_Birth.ToString("dd-MMM-yyyy");
                    row["Procedure/Rx/Reason for referral"] = FillOrderManagementSearch[i].Procedures;
                    row["ICD"] = string.Empty;
                    row["Ordering Provider"] = FillOrderManagementSearch[i].Ordering_Provider;
                    row["Ordering Facility"] = FillOrderManagementSearch[i].Facility_Name;
                    row["Ref to Facility/Lab/Pharmacy"] = string.Empty;
                    row["Encounter ID"] = FillOrderManagementSearch[i].Group_ID;
                    dt.Rows.Add(row);

                }
            }
            //ViewState["dt"] = dt;
            Session["dt"] = dt;

            grdReport.DataSource = dt;
            grdReport.DataBind();
        }


        private void FillSearchResultReferral(IList<FillOrdersManagementDTO> FillOrderManagementObj, int pageNumber, int maxResults)
        {

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Control");
            dt.Columns.Add("Arrival Date");
            dt.Columns.Add("Order Type");
            dt.Columns.Add("Order Status");
            dt.Columns.Add("Patient Acc");
            dt.Columns.Add("Patient Name");
            dt.Columns.Add("Patient DOB");
            dt.Columns.Add("Procedure/Rx/Reason for referral");
            dt.Columns.Add("ICD");
            dt.Columns.Add("Ordering Provider");
            dt.Columns.Add("Attending Provider");
            dt.Columns.Add("Ordering Facility");
            dt.Columns.Add("Ref to Facility/Lab/Pharmacy");
            dt.Columns.Add("Specimen");
            dt.Columns.Add("LabId");
            dt.Columns.Add("Print Req.");
            dt.Columns.Add("Physician ID");
            dt.Columns.Add("Encounter ID");
            dt.Columns.Add("Specimen Collected Date Time");
            dt.Columns.Add("View Result");
            dt.Columns.Add("GroupID");
            dt.Columns.Add("Temp_Property");

            DataRow row = null;
            grdReport.DataSource = null;
            grdReport.DataBind();
            if (FillOrderManagementObj != null)
            {
                if (mpnOrderManagement.PageNumber == 1 && FillOrderManagementObj.Count > 0)
                    mpnOrderManagement.TotalNoofDBRecords = Convert.ToInt32(FillOrderManagementObj[0].Total_Record_Found);

                //for (int i = 0; i < FillOrderManagementObj.Count; i++)
                for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > FillOrderManagementObj.Count ? FillOrderManagementObj.Count : (maxResults * pageNumber)); i++)
                {
                    row = dt.NewRow();

                    row["Temp_Property"] = FillOrderManagementObj[i].Temp_Property;
                    //CAP-2299
                    string ordered_Date_And_Time = UtilityManager.ConvertToLocal(FillOrderManagementObj[i].Ordered_Date_And_Time).ToString("dd-MMM-yyyy");
                    row["Arrival Date"] = ordered_Date_And_Time == "01-Jan-0001" ? "" : ordered_Date_And_Time;
                    row["Order Type"] = cboOrderType.Text;
                    row["Patient Acc"] = FillOrderManagementObj[i].Human_Id;
                    row["Patient Name"] = FillOrderManagementObj[i].Human_Name;
                    row["Patient DOB"] = FillOrderManagementObj[i].Date_Of_Birth.ToString("dd-MMM-yyyy");
                    row["Procedure/Rx/Reason for referral"] = FillOrderManagementObj[i].Procedures;
                    row["ICD"] = FillOrderManagementObj[i].Assessment;
                    row["Ordering Provider"] = FillOrderManagementObj[i].Ordering_Provider;
                    row["Attending Provider"] = FillOrderManagementObj[i].To_Physician_Name;
                    row["Ref to Facility/Lab/Pharmacy"] = FillOrderManagementObj[i].To_Facility_Name;
                    row["Encounter ID"] = FillOrderManagementObj[i].Group_ID;
                    row["Order Status"] = FillOrderManagementObj[i].Current_Process;
                    dt.Rows.Add(row);
                }
            }
            //ViewState["dt"] = dt;
            Session["dt"] = dt;

            grdReport.DataSource = dt;
            grdReport.DataBind();
        }

        protected void btnPrintToPDF_Click(object sender, EventArgs e)
        {
            //if (grdReport.Items.Count > 0)
            //{
            //    PrintResult();
            //}
            if (Session["SearchOrder"] != null)
            {
                SearchFillOrderManagement = (IList<FillOrdersManagementDTO>)Session["SearchOrder"];
                if (SearchFillOrderManagement != null)
                {
                    if (SearchFillOrderManagement.Count > 0)
                    {
                        PrintResult(SearchFillOrderManagement);
                    }
                }
            }
        }


        private void PrintResult(IList<FillOrdersManagementDTO> SearchFillOrderManagement)
        {
            SelectedItem.Value = string.Empty;
            System.Data.DataTable dtResult = new System.Data.DataTable();
            //dtResult = (System.Data.DataTable)Session["dt"];
            if (SearchFillOrderManagement != null)
            {
                if (SearchFillOrderManagement.Count > 0)
                {
                    dtResult = PrintOrder(SearchFillOrderManagement, cboOrderType.Text);
                    lblResultsFound.Text = SearchFillOrderManagement.Count.ToString() + " Result(s) found";
                }
            }

            if (dtResult == null)
            {
                return;
            }
            string sDirPath = Server.MapPath("Documents/" + Session.SessionID);

            DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }

            string TargetFileDirectory = Server.MapPath("Documents\\" + Session.SessionID);

            iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            string sReportFilePathName = System.Configuration.ConfigurationSettings.AppSettings["OrderManagementSearchPathName"];
            string folderPath = string.Empty;
            if (hdnLocalTime.Value != string.Empty)
            {
                folderPath = TargetFileDirectory + sReportFilePathName + "\\" + Convert.ToDateTime(hdnLocalTime.Value).ToString("yyyyMMdd") + "\\Pdf";
            }
            string sPrintPathName = folderPath + "\\" + "Order_Search_Report_" + cboOrderType.Text + UtilityManager.ConvertToLocal(Convert.ToDateTime(hdnLocalTime.Value)).ToString("yyyyMMdd hh mm ss tt") + ".pdf";
            iTextSharp.text.Font HeaderFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 20, 20, 20, 20);
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            doc.Open();
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Order Management Report for:\n"));
            doc.Add(new Paragraph("\n"));
            PdfPTable OrdDetailsTable = new PdfPTable(new float[] { 40, 40 });
            OrdDetailsTable.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase("Order Type  :  " + cboOrderType.Text, HeaderFont));
            cell.Colspan = 1;
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            OrdDetailsTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Order Status  :  " + cboOrderStatus.Text, HeaderFont));
            cell.Colspan = 1;
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            OrdDetailsTable.AddCell(cell);
            if (chkDate.Checked)
                //Changed On 23-11-2012 by Saravanakumar
                cell = new PdfPCell(new Phrase("Arrival Date  :  " + dtpFromDate.SelectedDate.Value.ToShortDateString() + " to " + dtpToDate.SelectedDate.Value.ToShortDateString(), HeaderFont));

            else
                cell = new PdfPCell(new Phrase("Arrival Date  :  ", HeaderFont));
            cell.Colspan = 1;
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            OrdDetailsTable.AddCell(cell);

            //Added On 14-12-2012 by Saravanakumar Bugid:12808
            cell = new PdfPCell(new Phrase("Facility Name  :  " + cboFacilityName.Text, HeaderFont));
            cell.Colspan = 1;
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            OrdDetailsTable.AddCell(cell);

            if (txtPatientName.Text != string.Empty)
            {
                cell = new PdfPCell(new Phrase("Patient Name  :  " + txtPatientName.Text, HeaderFont));
                cell.Colspan = 1;
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
                OrdDetailsTable.AddCell(cell);
            }

            if (txtProviderName.Text != string.Empty)
            {
                cell = new PdfPCell(new Phrase("Provider  :  " + txtProviderName.Text, HeaderFont));
                cell.Colspan = 1;
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
                OrdDetailsTable.AddCell(cell);
            }

            cell = new PdfPCell(new Phrase("Lab/Imaging Center  :  " + cboLabCenter.Text, HeaderFont));
            cell.Colspan = 1;
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            OrdDetailsTable.AddCell(cell);

            doc.Add(OrdDetailsTable);
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));
            //Changed On 26-11-2012 by Saravanakumar
            PdfPTable ResultTable;
            if (cboOrderType.Text == "DIAGNOSTIC ORDER")
            {
                ResultTable = new PdfPTable(new float[] { 15, 10, 10, 10, 15, 15, 20, 15, 15, 10, 10, 10 });
            }
            else if (cboOrderType.Text == "PROCEDURES")
            {
                ResultTable = new PdfPTable(new float[] { 10, 10, 10, 15, 15, 20, 15 });
            }
            else
            {
                ResultTable = new PdfPTable(new float[] { 10, 10, 10, 15, 15, 20, 15, 15, 10, 10, 10 });
            }
            ResultTable.WidthPercentage = 100;

            for (int i = 0; i < dtResult.Columns.Count - 1; i++)
            {
                if (grdReport.Columns[i].Display == true)
                {
                    if (dtResult.Columns[i].ToString().Trim() == "Print Req." || dtResult.Columns[i].ToString().Trim() == "View Result" || dtResult.Columns[i].ToString().Trim() == "Order Status")
                    {
                        continue;
                    }
                    else
                    {
                        if (dtResult.Columns[i].ToString() == "Control" || dtResult.Columns[i].ToString() == "Patient Acc")
                        {
                            string sControl;
                            sControl = dtResult.Columns[i].ToString() + " #";
                            cell = new PdfPCell(new Phrase(sControl, reducedFont));
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(dtResult.Columns[i].ToString(), reducedFont));
                        }
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        ResultTable.AddCell(cell);
                    }
                }
            }



            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                for (int j = 0; j < dtResult.Columns.Count - 1; j++)
                {
                    if (grdReport.Columns[j].Display == true)
                    {
                        if (dtResult.Columns[j].ToString().Trim() == "Print Req." || dtResult.Columns[j].ToString().Trim() == "View Result" || dtResult.Columns[j].ToString().Trim() == "Order Status")
                        {
                            continue;
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(Convert.ToString(dtResult.Rows[i][j].ToString()), normalFont));
                            cell.Colspan = 1;
                            ResultTable.AddCell(cell);
                        }
                    }
                }
            }

            doc.Add(ResultTable);
            doc.Close();
            string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
            string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
            if (SelectedItem.Value == string.Empty)
            {
                SelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
            }
            else
            {
                SelectedItem.Value += "|" + FileName[0].ToString();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenPDF();", true);

        }

        protected void grdReport_ItemCommand(object sender, GridCommandEventArgs e)
        {
            
            hdnSelectedItem.Value = string.Empty;

            // ulong ID = Convert.ToUInt64(grdReport.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)].Cells[8].Text);
            if (e.Item.ItemIndex != -1)
            {
                
                GridDataItem GridSelectItem = (GridDataItem)e.Item;
                //Srividhya - 19-dec-2014 - bugid:28811 & 28806==start
                ClientSession.HumanId = Convert.ToUInt32(GridSelectItem["PatientAcc"].Text);
                if (GridSelectItem["PhysicianID"].Text != "&nbsp;")
                    ClientSession.PhysicianId = Convert.ToUInt32(GridSelectItem["PhysicianID"].Text);
                //srividhya--end
                if (GridSelectItem["LabId"].Text != null && GridSelectItem["LabId"].Text.ToString() == "32" && GridSelectItem["OrderStatus"].Text.ToString().ToUpper() != "BILLING_WAIT" && GridSelectItem["OrderStatus"].Text.ToString().ToUpper() != "RESULT_REVIEW")
                {

                    Session["ResultExist"] = true;
                }
                else
                {
                    btnAddResult.Enabled = false;
                    Session.Add("ResultExist", null);
                }

                if (e.CommandName == "RowClick" && GridSelectItem["LabId"].Text != null && GridSelectItem["LabId"].Text.ToString() == "32" && GridSelectItem["OrderStatus"].Text.ToString().ToUpper() != "BILLING_WAIT" && GridSelectItem["OrderStatus"].Text.ToString().ToUpper() != "RESULT_REVIEW")
                    btnAddResult.Enabled = true;
            }


            if (grdReport.SelectedItems.Count == 0)
                btnAddResult.Enabled = false;
            if (e.CommandName == "PrintEReq")
            {
                GridDataItem GridSelectItem = (GridDataItem)e.Item;
                string FileLocation = string.Empty;
                PrintOrders print = new PrintOrders();
                if (GridSelectItem["ReftoFacility/Pharmacy"].Text.ToString().Contains("LabCorp") || GridSelectItem["ReftoFacility/Pharmacy"].Text.ToString().Contains("Quest"))
                {
                    if (cboOrderType.Text == "DIAGNOSTIC ORDER" && GridSelectItem["LabId"].Text != string.Empty)
                    {
                        string LabName = GridSelectItem["ReftoFacility/Pharmacy"].Text;

                        string sDirPath = Server.MapPath("Documents/" + Session.SessionID);

                        DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }

                        string TargetFileDirectory = Server.MapPath("Documents\\" + Session.SessionID);

                        string sReportFilePathName = System.Configuration.ConfigurationSettings.AppSettings["OrderManagementSearchPathName"];
                        string folderPath = TargetFileDirectory + sReportFilePathName + "\\" + Convert.ToDateTime(hdnLocalTime.Value).ToString("yyyyMMdd") + "\\Pdf";
                        string sPrintPathName = folderPath + "\\" + "Order_Search_Report_" + UtilityManager.ConvertToLocal(Convert.ToDateTime(hdnLocalTime.Value)).ToString("yyyyMMdd hh mm ss tt");
                        iTextSharp.text.Font HeaderFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);




                        if (LabName == "LabCorp")
                        {
                            if (GridSelectItem["ICD"].Text.ToString() != string.Empty && GridSelectItem["ICD"].Text != "&nbsp;")
                            {


                                FileLocation = print.PrintRequisitionUsingDatafromDB(Convert.ToUInt64(GridSelectItem["Control"].Text.ToString().Replace("ACUR", "").Trim()), folderPath, "ORDERS", Convert.ToUInt64(GridSelectItem["EncounterID"].Text), OrderType);
                                //string[] Split = new string[] { Server.MapPath("") };
                                string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                                string[] FileName = FileLocation.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                                if (hdnSelectedItem.Value == string.Empty)
                                {
                                    // hdnSelectedItem.Value = FileName[0].ToString();
                                    hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                                }
                                else
                                {
                                    hdnSelectedItem.Value += "|" + FileName[0].ToString();
                                }

                                ScriptManager.RegisterStartupScript(this, typeof(frmPrintPDF), string.Empty, "OpenPDFImage();", true);

                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", "DisplayErrorMessage('7090013');", true);
                                return;
                            }
                        }
                        else if (LabName == "Quest Diagnostics")
                        {
                            FileLocation = print.PrintSplitRequisitionUsingDatafromDBQuest(Convert.ToUInt64(GridSelectItem["Control"].Text.ToString().Replace("ACUR", "").Trim()), folderPath, "ORDERS", false, OrderType);
                            //string[] Split = new string[] { Server.MapPath("") };
                            string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                            string[] FileName = FileLocation.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                            if (hdnSelectedItem.Value == string.Empty)
                            {
                                //hdnSelectedItem.Value = FileName[0].ToString();
                                hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                            }
                            else
                            {
                                hdnSelectedItem.Value += "|" + FileName[0].ToString();
                            }

                            ScriptManager.RegisterStartupScript(this, typeof(frmImageAndLabOrder), string.Empty, "OpenPDFImage();", true);
                        }
                    }
                }
                else if (GridSelectItem["Procedure/Rx/Reasonforreferral"].Text.ToString().Contains("Paper Order"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", "DisplayErrorMessage('7090014');", true);
                    return;
                }
                else
                {
                    if (cboOrderType.Text == "REFERRAL ORDER")
                    {
                        SelectedItem.Value = string.Empty;

                        ReferralOrderManager refOrderMngr = new ReferralOrderManager();
                        ReferralOrderDTO objRefOrdDTO = new ReferralOrderDTO();
                        objRefOrdDTO = refOrderMngr.GetReferralOrderUsingReferralOrderID(Convert.ToUInt64(GridSelectItem["EncounterID"].Text));

                        string sDirPath = Server.MapPath("Documents/" + Session.SessionID);

                        DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }

                        string TargetFileDirectory = Server.MapPath("Documents\\" + Session.SessionID);

                        OrdersManager objOrdersManager = new OrdersManager();
                        FillHumanDTO objFillHumnaDTO = new FillHumanDTO();
                        objFillHumnaDTO = objOrdersManager.PatientInsuredBag(Convert.ToUInt32(GridSelectItem["PatientAcc"].Text));

                        if (objRefOrdDTO.RefOrdList.Count > 0)
                        {
                            FileLocation = print.PrintInReferralOrder(objRefOrdDTO.RefOrdList, objRefOrdDTO.RefOrdList[0].From_Physician_ID, objRefOrdDTO, objFillHumnaDTO, objRefOrdDTO.RefOrdList[0].Referral_Specialty, TargetFileDirectory);

                            string[] Split = new string[] { Server.MapPath("") };
                            string[] FileName = FileLocation.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                            if (FileName.Count() > 0)
                            {
                                for (int i = 0; i < FileName.Count(); i++)
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

                            ScriptManager.RegisterStartupScript(this, typeof(frmImageAndLabOrder), string.Empty, "OpenPDFImage();", true);
                        }
                    }
                    else if (cboOrderType.Text == "IMMUNIZATION ORDER")
                    {
                        SelectedItem.Value = string.Empty;

                        ImmunizationManager immunMngr = new ImmunizationManager();
                        ImmunizationDTO objImmunizationFill = new ImmunizationDTO();
                        //CAP-2310
                        objImmunizationFill.Immunization = immunMngr.GetImmunizationUsingGroupID(Convert.ToUInt64(GridSelectItem["EncounterID"].Text), Convert.ToUInt64(GridSelectItem["PatientAcc"].Text));

                        string sDirPath = Server.MapPath("Documents/" + Session.SessionID);

                        DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }

                        string TargetFileDirectory = Server.MapPath("Documents\\" + Session.SessionID);

                        HumanManager humanMngr = new HumanManager();
                        Human objHuman= humanMngr.GetHumanFromHumanID(Convert.ToUInt32(GridSelectItem["PatientAcc"].Text));

                        PhysicianLibrary objPhyLib = new PhysicianLibrary();
                        XmlDocument xmldoc = new XmlDocument();

                        string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
                        if (File.Exists(strXmlFilePath) == true)
                        {
                            //logger.Debug("Reading PhysicianAddressDetails.xml");
                            xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                            XmlNode nodeMatchingPhysicianAddress = xmldoc.SelectSingleNode("/PhysicianAddress/p" + objImmunizationFill.Immunization[0].Physician_Id);
                            if (nodeMatchingPhysicianAddress != null)
                            {
                                objPhyLib.PhyAddress1 = nodeMatchingPhysicianAddress.Attributes["Physician_Address1"].Value.ToString();
                                objPhyLib.PhyAddress2 = nodeMatchingPhysicianAddress.Attributes["Physician_Address2"].Value.ToString();
                                objPhyLib.PhyCity = nodeMatchingPhysicianAddress.Attributes["Physician_City"].Value.ToString();
                                objPhyLib.PhyState = nodeMatchingPhysicianAddress.Attributes["Physician_State"].Value.ToString();
                                objPhyLib.PhyZip = nodeMatchingPhysicianAddress.Attributes["Physician_Zip"].Value.ToString();
                                objPhyLib.PhyTelephone = nodeMatchingPhysicianAddress.Attributes["Physician_Telephone"].Value.ToString();
                                objPhyLib.PhyFax = nodeMatchingPhysicianAddress.Attributes["Physician_Fax"].Value.ToString();
                                objPhyLib.PhyNPI = nodeMatchingPhysicianAddress.Attributes["Physician_NPI"].Value.ToString();
                                //logger.Debug("XML tag '/PhysicianAddress/p" + physician_id + "' found");
                            }
                            //else
                            //logger.Debug("XML tag '/PhysicianAddress/p" + physician_id + "' not found");
                        }

                        if (objImmunizationFill.Immunization.Count > 0)
                        {
                            FileLocation = print.PrintImmunizationOrders(objImmunizationFill, objPhyLib, objHuman, sDirPath);

                            string[] Split = new string[] { Server.MapPath("") };
                            string[] FileName = FileLocation.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                            if (FileName.Count() > 0)
                            {
                                for (int i = 0; i < FileName.Count(); i++)
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

                            ScriptManager.RegisterStartupScript(this, typeof(frmImageAndLabOrder), string.Empty, "OpenPDFImage();", true);
                        }
                    }
                    else
                    {
                        string path = Server.MapPath("Documents/" + Session.SessionID);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        OrdersManager objOrdersManager = new OrdersManager();
                        OrdersDTO objOrderDTO = null;
                        object objDTO;
                        var serializer = new NetDataContractSerializer();
                        ulong order_submit_id = Convert.ToUInt64(GridSelectItem["Control"].Text.ToString().Replace("ACUR", "").Trim());
                        objDTO = (object)serializer.ReadObject(objOrdersManager.LoadOrdersByOrdersSubmitedId(order_submit_id,Convert.ToUInt64(GridSelectItem["EncounterID"].Text), Convert.ToUInt32(GridSelectItem["PhysicianID"].Text), Convert.ToUInt32(GridSelectItem["PatientAcc"].Text), "DIAGNOSTIC ORDER", string.Empty, UtilityManager.ConvertToUniversal(DateTime.Now), false));
                        objOrderDTO = (OrdersDTO)objDTO;
                        FillHumanDTO objFillHumnaDTO = new FillHumanDTO();
                        //FileLocation = print.CallPrintLabAndImageOrders(path, Convert.ToUInt64(GridSelectItem["EncounterID"].Text), objOrderDTO, objOrderDTO.objHuman);
                        
                        PhysicianManager objPhysicianManager = new PhysicianManager();
                        PhysicianLibrary objPhysician = new PhysicianLibrary();
                        if (GridSelectItem["PhysicianID"].Text != "0")
                            objPhysician = objPhysicianManager.GetphysiciannameByPhyID(Convert.ToUInt64(GridSelectItem["PhysicianID"].Text.ToString()))[0];
                        else
                        {
                            EncounterManager objEncMngr = new EncounterManager();
                            IList<Encounter> objEnc = new List<Encounter>();
                            objEnc = objEncMngr.GetEncounterByOrderSubmitID(Convert.ToInt32(GridSelectItem["Control"].Text.ToString().Replace("ACUR", "").Trim()));
                            if (objEnc.Count > 0)
                            {
                                objPhysician.PhyFirstName = objEnc[0].Referring_Physician;
                                objPhysician.PhyAddress1 = objEnc[0].Referring_Address;
                                objPhysician.PhyTelephone = objEnc[0].Referring_Phone_No;
                                objPhysician.PhyFax = objEnc[0].Referring_Fax_No;
                                objPhysician.PhyNPI = objEnc[0].Referring_Provider_NPI;
                            }
                        }
                        IList<OrderLabDetailsDTO> ordList = (from obj in objOrderDTO.ilstOrderLabDetailsDTO where obj.OrdersSubmit.Id == order_submit_id select obj).ToList<OrderLabDetailsDTO>();
                        string LabName = GridSelectItem["ReftoFacility/Pharmacy"].Text;

                        if (LabName.ToUpper() == "OUTPATIENT ORDER")//For bug Id 65806
                        {
                            FileLocation = print.PrintOrdersForOutpatientOrder(ordList, objPhysician, objOrderDTO.objHuman, objOrderDTO, path, "DIAGNOSTIC ORDER", ordList);
                        }
                        else
                        {
                            FileLocation = print.PrintOrdersForSelectedLab(ordList, objPhysician, objOrderDTO.objHuman, "DIAGNOSTIC ORDER", objOrderDTO, path);
                        }
                        string[] Split = new string[] { Server.MapPath("") };
                        string[] FileName = FileLocation.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                        if (FileName.Count() > 0)
                        {
                            for (int i = 0; i < FileName.Count(); i++)
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

                        ScriptManager.RegisterStartupScript(this, typeof(frmImageAndLabOrder), string.Empty, "OpenPDFImage();", true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7090013');", true);
                        //return;
                    }
                }
            }
            else if (e.CommandName == "ViewResults")
            {
                GridDataItem GridSelectItem = (GridDataItem)e.Item;
                if (GridSelectItem["IndexOrderID"].Text == string.Empty || (Convert.ToInt32(GridSelectItem["IndexOrderID"].Text) == 0 && GridSelectItem["IsElectronicSignature"].Text == "False"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", "DisplayErrorMessage('7090010');", true);
                    Session.Add("ResultExist", null);
                    return;
                }
                else
                {
                    ulong Human_ID = 0;
                    Session["ResultExist"] = true;
                    ulong order_submit_id = Convert.ToUInt64(GridSelectItem["Control"].Text.ToString().Replace("ACUR", "").Trim());
                    string currentProcess = Convert.ToString(GridSelectItem["OrderStatus"].Text.ToString());
                    if (GridSelectItem["PatientAcc"].Text != null && GridSelectItem["PatientAcc"].Text != string.Empty)
                    {
                        Human_ID = Convert.ToUInt64(GridSelectItem["PatientAcc"].Text.ToString());
                    }
                    ulong Physician_ID = Convert.ToUInt64(GridSelectItem["PhysicianID"].Text.ToString());
                    int lab_id = Convert.ToInt32(GridSelectItem["LabId"].Text);
                    ulong enc_id = Convert.ToUInt64(GridSelectItem["EncounterID"].Text);
                    hdnCurrentProcess.Value = currentProcess;
                    string ScriptContent = "ViewResults(" + Human_ID + "," + order_submit_id + "," + enc_id + "," + Physician_ID + "," + lab_id + ");";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Open", ScriptContent, true);

                }
            }
        }

        protected void pbClearPatientName_Click(object sender, ImageClickEventArgs e)
        {
            txtPatientName.Text = "";
            DateEnable();
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void pbClearProviderName_Click(object sender, ImageClickEventArgs e)
        {
            txtProviderName.Text = "";
            DateEnable();
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        public void FirstPageNavigator(object sender, EventArgs e)
        {
            if (Session["SearchOrder"] != null)
            {
                IList<FillOrdersManagementDTO> pageNavigatorMngt = (IList<FillOrdersManagementDTO>)Session["SearchOrder"];
                FillSearchResultLabAndImage(pageNavigatorMngt, mpnOrderManagement.PageNumber, mpnOrderManagement.MaxResultPerPage);
            }
            // btnSearch_Click(sender, e);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            cboOrderType.SelectedIndex = 0;
            cboOrderStatus.SelectedIndex = 0;
            cboLabCenter.SelectedIndex = 0;
            Session.Remove("dt");
            Clear();
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        public void DateEnable()
        {
            if (chkDate.Checked == true)
            {
                dtpFromDate.Enabled = true;
                dtpToDate.Enabled = true;
            }
            else
            {
                dtpFromDate.Enabled = false;
                dtpToDate.Enabled = false;
            }
        }

        protected void grdReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {

        }
    }
}
