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
using System.Xml;
using Telerik.Web.UI;
using Acurus.Capella.Core.DTOJson;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Web.Services;
using System.IO.Compression;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Vml;

namespace Acurus.Capella.UI
{
    public partial class frmLabException : System.Web.UI.Page
    {
        FillPhysicianUser PhyUserList;
        FillPhysicianUser phyUnmatchedList;
        PhysicianManager objPhysicianManager = new PhysicianManager();
        OrdersManager orderProxy = new OrdersManager();
        ResultMasterManager resultmasterMngr = new ResultMasterManager();
        // bool bChkClick = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Jira Cap - 3206
                //cboUnmatchProvider.Items.Add(new RadComboBoxItem(""));
                //cboUnmatchProvider.Items[0].Value = "";
                //CAP-3208
                //cboProviderName.Items.Add(new RadComboBoxItem("ALL"));
                cboProviderName.Items.Add(new ListItem("ALL"));

                //Jira Cap - 3206
                //if (grdUnassignedResults.DataSource == null)
                //{
                //    grdUnassignedResults.DataSource = new string[] { };
                //    grdUnassignedResults.DataBind();

                //}
                //if (grdOutstandingOrders.DataSource == null)
                //{
                //    grdOutstandingOrders.DataSource = new string[] { };
                //    grdOutstandingOrders.DataBind();

                //}
                PhyUserList = objPhysicianManager.GetPhysicianandUser(true, ClientSession.FacilityName, ClientSession.LegalOrg);

                phyUnmatchedList = objPhysicianManager.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);

                if (PhyUserList.UserList != null)
                {
                    IList<User> ResultUserList = PhyUserList.UserList.Where(a => a.user_name == ClientSession.UserName).ToList<User>();

                    if (ResultUserList != null)
                    {
                        if (ResultUserList.Count == 0)
                        {
                            if (phyUnmatchedList != null)
                                PhyUserList = phyUnmatchedList;
                            chkProviderName.Checked = true;

                        }
                    }


                }

                if (PhyUserList.PhyList != null)
                {
                    //CAP-3208
                    //for (int i = 0; i < PhyUserList.PhyList.Count; i++)
                    //{
                    //    string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;
                    //    cboProviderName.Items.Add(new RadComboBoxItem(sPhyName.Trim()));
                    //    cboProviderName.Items[i + 1].Value = PhyUserList.PhyList[i].PhyNPI;
                    //    if (PhyUserList.UserList[i].user_name.ToString() == ClientSession.UserName)
                    //        cboProviderName.SelectedIndex = i + 1;
                    //}

                    for (int i = 0; i < PhyUserList.PhyList.Count; i++)
                    {
                        string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;
                        ListItem listItem = new ListItem(sPhyName.Trim(), PhyUserList.PhyList[i].PhyNPI);
                        cboProviderName.Items.Add(listItem);
                        if (PhyUserList.UserList[i].user_name.ToString() == ClientSession.UserName)
                        {
                            cboProviderName.SelectedIndex = i + 1;
                        }
                    }
                }

                //Jira Cap - 3206
                //if (phyUnmatchedList.PhyList != null)
                //{
                //    for (int i = 0; i < phyUnmatchedList.PhyList.Count; i++)
                //    {
                //        string sPhyName = phyUnmatchedList.PhyList[i].PhyPrefix + " " + phyUnmatchedList.PhyList[i].PhyFirstName + " " + phyUnmatchedList.PhyList[i].PhyMiddleName + " " + phyUnmatchedList.PhyList[i].PhyLastName + " " + phyUnmatchedList.PhyList[i].PhySuffix;
                //        cboUnmatchProvider.Items.Add(new RadComboBoxItem(sPhyName.Trim()));
                //        cboUnmatchProvider.Items[i + 1].Value = phyUnmatchedList.PhyList[i].PhyNPI;
                //    }
                //}

                //pageNavigator1.lbtnLast.Attributes.Add("onclick", "check();");
                //pageNavigator1.lbtnNext.Attributes.Add("onclick", "check();");
                //pageNavigator1.lbtnFirst.Attributes.Add("onclick", "check();");
                //pageNavigator1.lbtnPrevious.Attributes.Add("onclick", "check();");
                //CAP-3208
                //cboProviderName.Items.Add(new RadComboBoxItem("PROVIDER NOT ASSIGNED"));
                cboProviderName.Items.Add(new ListItem("PROVIDER NOT ASSIGNED"));
                //pageNavigator1.TotalNoofDBRecords = 0;
                //rbtnAllResults.Checked = true;
                //Jira Cap - 3206
                //cboUnmatchProvider.Enabled = false;
                //chkUnmatchedProvider.Enabled = false;
                //CAP - 3207
                //btnFindPatient.Enabled = false;
                //Jira Cap - 3206
                //btnMatchOrders.Enabled = false;
                string labIdList = string.Empty;

                //BugID:46054
                //XmlDocument xmldocUser = new XmlDocument();
                //xmldocUser.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "LabList" + ".xml");
                //XmlNodeList xmlLabList = xmldocUser.GetElementsByTagName("Lab");
                //int count = 0;
                //cboLabName.Items.Add(new RadComboBoxItem(""));
                //cboLabName.Items[count].Value = "0";
                //foreach (XmlNode item in xmlLabList)
                //{
                //    cboLabName.Items.Add(new RadComboBoxItem(item.Attributes.GetNamedItem("name").Value.ToString()));
                //    count++;
                //    cboLabName.Items[count].Value = item.Attributes.GetNamedItem("id").Value;
                //}

                //CAP-2862
                //CAP-3182
                //Lablist objlablist = new Lablist();
                //objlablist = ConfigureBase<Lablist>.ReadJson("LabList.json");
                //List<Labs> listLabList = new List<Labs>();
                //listLabList = objlablist.Lab.ToList();
                //int count = 0;
                //cboLabName.Items.Add(new RadComboBoxItem(""));
                //cboLabName.Items[count].Value = "0";
                ////CAP-2923
                //count++;
                //if (listLabList != null && listLabList.Count > 0)
                //{
                //    foreach (Labs objlab in listLabList)
                //    {
                //        cboLabName.Items.Add(new RadComboBoxItem(objlab.name.ToString()));
                //        cboLabName.Items[count].Value = objlab.id;
                //        count++;
                //    }
                //}

                //CAP-3208
                //LabManager objmgr = new LabManager();
                //IList<Lab> listLabList = new List<Lab>();
                //listLabList = objmgr.GetlabName();
                //int count = 0;
                //cboLabName.Items.Add(new RadComboBoxItem(""));
                //cboLabName.Items[count].Value = "0";
                ////CAP-2923
                //count++;
                //if (listLabList != null && listLabList.Count > 0)
                //{
                //    foreach (Lab objlab in listLabList)
                //    {
                //        cboLabName.Items.Add(new RadComboBoxItem(objlab.Lab_Name.ToString()));
                //        cboLabName.Items[count].Value = objlab.Id.ToString();
                //        count++;
                //    }
                //}

                LabManager objmgr = new LabManager();
                var listLabList = objmgr.GetlabName();

                cboLabName.Items.Clear();
                cboLabName.Items.Add(new ListItem("", "0"));

                if (listLabList != null && listLabList.Any())
                {
                    foreach (var lab in listLabList)
                    {
                        cboLabName.Items.Add(new ListItem(lab.Lab_Name, lab.Id.ToString()));
                    }
                }

                //CAP-3208
                //StaticLookupManager sm = new StaticLookupManager();
                //IList<StaticLookup> Errorlst = sm.getStaticLookupByFieldName("ERROR_CODE");
                //cboErrorReason.Items.Add(new RadComboBoxItem(""));
                //foreach (StaticLookup sl in Errorlst)
                //{
                //    // if (sl.Value != "ACUR_LAB_03" && sl.Value != "ACUR_LAB_04") //For Bug Id : 68705
                //   if (sl.Value != "ACUR_LAB_04")
                //        cboErrorReason.Items.Add(new RadComboBoxItem(sl.Value + "-" + sl.Description));
                //}

                StaticLookupManager sm = new StaticLookupManager();
                IList<StaticLookup> Errorlst = sm.getStaticLookupByFieldName("ERROR_CODE");
                cboErrorReason.Items.Add(new ListItem(""));

                foreach (StaticLookup sl in Errorlst)
                {
                    if (sl.Value != "ACUR_LAB_04")
                        cboErrorReason.Items.Add(new ListItem(sl.Value + "-" + sl.Description));
                }

            }
            else
            {
                //CAP - 3207
                //if (Request.Form["__EVENTTARGET"] != null)
                //{
                //    if (Request.Form["__EVENTTARGET"] == "grdUnassignedResults")
                //        FillPatientandOutstandingorders();
                //    else if (Request.Form["__EVENTTARGET"].Contains("chk"))
                //    {
                //        btnFindPatient.Enabled = false;
                //        btnMatchOrders.Enabled = false;
                //        if (cboUnmatchProvider.Enabled == false)
                //        {
                //            cboUnmatchProvider.Enabled = false;
                //            chkUnmatchedProvider.Enabled = false;
                //            cboUnmatchProvider.SelectedIndex = 0;
                //        }
                //        grdUnassignedResults.DataSource = null;
                //        chkNoOrders.Checked = false;
                //    }
                //}

                //Jira Cap - 3206
                //if (Request.Form["__EVENTTARGET"] != null)
                //{
                //    string sAutosearch = string.Empty;
                //    //if (Request.Form["__EVENTTARGET"] == "grdUnassignedResults")
                //    //{
                //    //    FillPatientandOutstandingorders();
                //    //}
                //    //else if (Request.Form["__EVENTTARGET"].Contains("chk"))
                //    //{
                //    //    //CAP - 3207
                //    //    //btnFindPatient.Enabled = false;
                //    //    btnMatchOrders.Enabled = false;
                //    //    if (cboUnmatchProvider.Enabled == false)
                //    //    {
                //    //        cboUnmatchProvider.Enabled = false;
                //    //        chkUnmatchedProvider.Enabled = false;
                //    //        cboUnmatchProvider.SelectedIndex = 0;
                //    //    }
                //    //    grdUnassignedResults.DataSource = null;
                //    //    chkNoOrders.Checked = false;
                //    //}

                //    //string eventTarget = Request["__EVENTTARGET"];
                //    //if (eventTarget == "ctl00$ContentPlaceHolder1$InvisibleButton")
                //    //{
                //    //    sAutosearch = "Y";
                //    //   // InvisibleButton_Click(this, EventArgs.Empty);
                //    //}
                //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "labpatientsearch", "setpatientsearch('" + sAutosearch + "');", true);
                //}
            }


        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            IList<FillOrderException> ilstFillOrderException = new List<FillOrderException>();
            ResultMasterManager objresultMasterMngr = new ResultMasterManager();
            IList<string> FieldName = new List<string>();
            IList<string> FieldValue = new List<string>();
            string sCriteria = string.Empty;
            string NPINumbers = string.Empty;
            //CAP - 3207
            //btnFindPatient.Enabled = false;
            //Jira Cap - 3206
            //btnMatchOrders.Enabled = false;
            // cboUnmatchProvider.Enabled = false;
            //chkUnmatchedProvider.Enabled = false;
            // cboUnmatchProvider.SelectedIndex = 0;
            //grdUnassignedResults.DataSource = null;
            //chkNoOrders.Checked = false;
            Clear();
            //CAP-3208
            //if (cboProviderName.SelectedIndex != null)
            //{
            //    if (cboProviderName.Items[cboProviderName.SelectedIndex].Value != null)
            //        NPINumbers = cboProviderName.Items[cboProviderName.SelectedIndex].Value.ToString();
            //    else
            //        NPINumbers = "";
            //}
            if (cboProviderName.SelectedIndex >= 0)
            {
                if (!string.IsNullOrEmpty(cboProviderName.SelectedValue))
                    NPINumbers = cboProviderName.SelectedValue.ToString();
                else
                    NPINumbers = "";
            }
            //CAP-3208
            //if (cboCategory.Items[cboCategory.SelectedIndex].Value != null)
            //    sCriteria = cboCategory.Items[cboCategory.SelectedIndex].Text.ToString();

            if (!string.IsNullOrEmpty(cboCategory.SelectedValue))
                sCriteria = cboCategory.SelectedItem.Text;

            //CAP-3208
            //if (cboErrorReason.SelectedIndex != 0)
            //{
            //    FieldName.Add("Reason_Code");
            //    if (cboErrorReason.Items[cboErrorReason.SelectedIndex].Value != null)
            //        FieldValue.Add(cboErrorReason.Items[cboErrorReason.SelectedIndex].Text.ToString());
            //}

            if (cboErrorReason.SelectedIndex != 0)
            {
                FieldName.Add("Reason_Code");
                if (!string.IsNullOrEmpty(cboErrorReason.SelectedValue))
                    FieldValue.Add(cboErrorReason.SelectedItem.Text);
            }

            //CAP-3208
            //if (cboLabName.SelectedIndex != 0)
            //{
            //    FieldName.Add("Lab_Id");
            //    if (cboLabName.Items[cboLabName.SelectedIndex].Value != null)
            //        FieldValue.Add(cboLabName.Items[cboLabName.SelectedIndex].Value.ToString());               
            //}

            if (cboLabName.SelectedIndex != 0)
            {
                FieldName.Add("Lab_Id");
                if (!string.IsNullOrEmpty(cboLabName.SelectedValue))
                    FieldValue.Add(cboLabName.SelectedValue);
            }

            if (frmDate.SelectedDate.ToString() != "" && toDate.SelectedDate.ToString() != "")
            {
                FieldName.Add("MSH_Date_And_Time_Of_Message");
                String[] dateSplit = frmDate.SelectedDate.ToString().Split('/');
                string dateval = dateSplit[2].Split(' ')[0] + (dateSplit[0].Length == 1 ? "0" + dateSplit[0] : dateSplit[0]) + (dateSplit[1].Length == 1 ? "0" + dateSplit[1] : dateSplit[1]);
                String[] dateSplit_toDt = toDate.SelectedDate.ToString().Split('/');
                string dateval_toDt = dateSplit_toDt[2].Split(' ')[0] + (dateSplit_toDt[0].Length == 1 ? "0" + dateSplit_toDt[0] : dateSplit_toDt[0]) + (dateSplit_toDt[1].Length == 1 ? "0" + dateSplit_toDt[1] : dateSplit_toDt[1]);
                FieldValue.Add(dateval + "-" + dateval_toDt);
            }
            //if (rbtnAllResults.Checked)
            //    sCriteria = rbtnAllResults.Text;
            //else if (rbtnResultAttachedtoPatientChart.Checked)
            //    sCriteria = rbtnResultAttachedtoPatientChart.Text;
            //else if (rbtnResultNottoPatientChart.Checked)
            //    sCriteria = rbtnResultNottoPatientChart.Text;

            //CAP - 3208
            //if (cboProviderName.Text.ToUpper() == "ALL")
            //{
            //    sCriteria = cboProviderName.Text.ToUpper();
            //}
            //else if (cboProviderName.Text.ToUpper() == "PROVIDER NOT ASSIGNED")
            //{
            //    sCriteria = cboProviderName.Text.ToUpper();
            //}
            //else if (cboProviderName.SelectedIndex == null)
            //{
            //    // ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7100011');", true);
            //    return;
            //}
            //else if (cboProviderName.Items[cboProviderName.SelectedIndex].Value.ToString() == null)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('7100011');", true);
            //    return;
            //}
            //else if (cboProviderName.Items[cboProviderName.SelectedIndex].Value.ToString() == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('7100011');", true);
            //    return;
            //}

            if (cboProviderName.SelectedItem.Text.ToUpper() == "ALL")
            {
                sCriteria = cboProviderName.SelectedItem.Text.ToUpper();
            }
            else if (cboProviderName.SelectedItem.Text.ToUpper() == "PROVIDER NOT ASSIGNED")
            {
                sCriteria = cboProviderName.SelectedItem.Text.ToUpper();
            }
            else if (cboProviderName.SelectedIndex == null && cboProviderName.SelectedIndex < 0)
            {
                // ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7100011');", true);
                return;
            }
            else if (string.IsNullOrEmpty(cboProviderName.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('7100011');", true);
                return;
            }

            //if (txtResultDt.Text != "____-___-__" && txtResultDt.Text != "")
            //{
            //    FieldName.Add("MSH_Date_And_Time_Of_Message");
            //    String[] dateSplit = txtResultDt.Text.Split('-');
            //    string dateval = String.Join("", dateSplit);
            //    FieldValue.Add(dateval);
            //}
            // if (txtReasonNtMatching.Text != "")
            //{
            //    FieldName.Add("Reason_Code");
            //    FieldValue.Add(txtReasonNtMatching.Text.Trim());
            //}
            // if (txtLabName.Text != "")
            //{
            //     string labIdList = string.Empty;
            //    XmlDocument xmldocUser = new XmlDocument();
            //    xmldocUser.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "LabList" + ".xml");
            //    XmlNodeList xmlLabList = xmldocUser.GetElementsByTagName("Lab");
            //    foreach (XmlNode item in xmlLabList)
            //    {
            //        if(item.Attributes.GetNamedItem("name").Value.ToString().ToLower().Contains(txtLabName.Text.Trim().ToString().ToLower())){
            //            if(labIdList==string.Empty){
            //                labIdList+=Convert.ToInt16(item.Attributes.GetNamedItem("id").Value.ToString());
            //            }
            //            else{
            //                labIdList+=","+Convert.ToInt16(item.Attributes.GetNamedItem("id").Value.ToString());
            //            }
            //        }
            //    }
            //    FieldValue.Add(labIdList);
            //    FieldName.Add("Lab_Id");
            //}
            //Jira Cap - 3206
            //ilstFillOrderException = objresultMasterMngr.GetOrderExceptionItems(pageNavigator1.PageNumber, pageNavigator1.MaxResultPerPage, sCriteria, NPINumbers, FieldName, FieldValue);
            ilstFillOrderException = objresultMasterMngr.GetOrderExceptionItems(sCriteria, NPINumbers, FieldName, FieldValue);
            Session["FillOrderException"] = ilstFillOrderException;
            if (ilstFillOrderException != null)
            {
                if (ilstFillOrderException.Count > 0)
                {
                    //Jira Cap - 3206
                    //pageNavigator1.TotalNoofDBRecords = ilstFillOrderException[0].TotalCount;
                    //lblSearch.Text = pageNavigator1.TotalNoofDBRecords.ToString() + " Result(s) Found ";
                }
            }
            else
            {
                //Jira Cap - 3206
                //pageNavigator1.TotalNoofDBRecords = 0;
                //lblSearch.Text = "No Result Found ";
            }
            if (ilstFillOrderException != null)
                //Jira Cap - 3206
                //FillOrderExceptionGrid(ilstFillOrderException);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SearchLab", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }
        private void Clear()
        {
            //Jira Cap - 3206
            //DataTable dtClear = new DataTable();
            //grdOutstandingOrders.DataSource = dtClear;
            //grdOutstandingOrders.DataBind();
            //CAP - 3207
            //txtPatientName.Text = string.Empty;
            //txtAccountNumber.Text = string.Empty;
            //txtDOB.Text = string.Empty;
            //txtGender.Text = string.Empty;

        }

        private void FillOutstandingOrdersGrid(IList<Orders> FillOutstandingOrders)
        {
            ulong iOrderSubmitId = 0;
            if (FillOutstandingOrders != null)
                FillOutstandingOrders = FillOutstandingOrders.OrderByDescending(a => a.Order_Submit_ID).ToList<Orders>();
            DataRow dr = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("Order#", typeof(string));
            dt.Columns.Add("Order Description(Procedures)", typeof(string));
            dt.Columns.Add("Order Date", typeof(string));
            dt.Columns.Add("Lab Name", typeof(string));
            dt.Columns.Add("Order_Submit_ID", typeof(string));
            bool colAdded = false;
            if (FillOutstandingOrders != null)
            {

                foreach (Orders obj in FillOutstandingOrders)
                {
                    if (obj.Lab_Procedure != "")
                    {
                        if (iOrderSubmitId == obj.Order_Submit_ID)
                        {
                            dr["Order Description(Procedures)"] += obj.Lab_Procedure + "-" + obj.Lab_Procedure_Description + ";";
                            colAdded = true;
                        }
                        else
                        {
                            if (dr != null)
                            {
                                dt.Rows.Add(dr);
                                colAdded = false;
                            }

                            dr = dt.NewRow();
                            dr["Order#"] = "ACUR" + obj.Order_Submit_ID.ToString();
                            dr["Order Description(Procedures)"] += obj.Lab_Procedure + "-" + obj.Lab_Procedure_Description + ";";
                            dr["Order Date"] = obj.Created_Date_And_Time.ToString("dd-MMM-yyyy");
                            dr["Lab Name"] = obj.Internal_Property_Lab_Name;
                            dr["Order_Submit_ID"] = obj.Order_Submit_ID.ToString();
                            iOrderSubmitId = obj.Order_Submit_ID;
                        }
                    }
                }
                if (colAdded == true)
                {
                    dt.Rows.Add(dr);
                    colAdded = false;
                }
            }
            //Jira Cap - 3206
            //grdOutstandingOrders.DataSource = dt;
            //grdOutstandingOrders.DataBind();

        }
        //Jira Cap - 3206
        //private void FillOrderExceptionGrid(IList<FillOrderException> FillOrderException)
        //{

        //    grdUnassignedResults.DataSource = null;
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Patient Acc # in Result", typeof(string));
        //    dt.Columns.Add("Patient Name in Result", typeof(string));
        //    dt.Columns.Add("Patient DOB In Result", typeof(string));
        //    dt.Columns.Add("Patient Gender In Result", typeof(string));
        //    dt.Columns.Add("Order ID", typeof(string));
        //    dt.Columns.Add("Specimen", typeof(string));
        //    dt.Columns.Add("Lab Procedure", typeof(string));
        //    dt.Columns.Add("Reason for not matching", typeof(string));
        //    dt.Columns.Add("Result Message Date And Time", typeof(string));
        //    dt.Columns.Add("Provider Name", typeof(string));
        //    dt.Columns.Add("Lab Name", typeof(string));

        //    dt.Columns.Add("Order Type", typeof(string));
        //    dt.Columns.Add("OrderID", typeof(string));
        //    dt.Columns.Add("Lab ID", typeof(string));
        //    dt.Columns.Add("Matching Patient ID", typeof(string));
        //    dt.Columns.Add("Reason Code", typeof(string));
        //    dt.Columns.Add("OrderingNPI", typeof(string));
        //    dt.Columns.Add("ResultMasterID", typeof(string));
        //    dt.Columns.Add("Abnormal", typeof(string));

        //    if (FillOrderException != null)
        //    {
        //        FillOrderException = FillOrderException.OrderByDescending(x => x.Is_Abnormal).ToList<FillOrderException>(); ;
        //        foreach (FillOrderException obj in FillOrderException)
        //        {
        //            DataRow dr = dt.NewRow();
        //            if (obj.HumanId != 0)
        //                dr["Patient Acc # in Result"] = obj.HumanId.ToString();
        //            dr["Patient Name in Result"] = obj.Last_Name_In_Result + " " + obj.First_Name_In_Result + " " + obj.MI_In_Result;
        //            if (obj.DOB_In_Result!=null)
        //             dr["Patient DOB In Result"] = obj.DOB_In_Result.ToString("dd-MMM-yyyy");
        //            else
        //             dr["Patient DOB In Result"] ="";

        //            dr["Patient Gender In Result"] = obj.Sex_In_Result;
        //            dr["Order ID"] = obj.Order_ID.ToString();
        //            dr["Specimen"] = obj.Specimen;
        //            dr["Lab Procedure"] = obj.Lab_Procedure;
        //            dr["Reason for not matching"] = obj.Reason_Code + "-" + obj.Reason_Description;
        //            if (obj.Result_Received_Date_And_Time.Trim() != "")
        //                dr["Result Message Date And Time"] = DateTime.ParseExact(obj.Result_Received_Date_And_Time.Substring(0, 12), "yyyyMMddHHmm", null).ToString("yyyy-MM-dd hh:mm tt");
        //            else
        //                dr["Result Message Date And Time"] = "";
        //            dr["Provider Name"] = obj.Provider_Name;
        //            dr["Lab Name"] = obj.Lab_Name;
        //            dr["OrderID"] = obj.Order_ID.ToString();
        //            dr["Lab ID"] = obj.Lab_ID;
        //            dr["Matching Patient ID"] = obj.Matching_Patient_ID;
        //            dr["Reason Code"] = obj.Reason_Code;
        //            dr["OrderingNPI"] = obj.OrderingProviderNPI;
        //            dr["ResultMasterID"] = obj.Result_Master_ID;
        //            dr["Abnormal"] = obj.Is_Abnormal; //For bug ID 45804
        //            dt.Rows.Add(dr);
        //        }
        //    }
        //    grdUnassignedResults.DataSource = dt;
        //    grdUnassignedResults.DataBind();
        //}
        public void FirstPageNavigator(object sender, EventArgs e)
        {
            btnSearch_Click(new object(), new EventArgs());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MatchOrder", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }




        //Jira Cap - 3206
        //public void FillPatientandOutstandingorders()
        //{
        //    bool cboCheck = false;
        //    //CAP - 3207
        //    //btnFindPatient.Enabled = true;
        //    bool vFindPatientenabled =true;
        //    string sPatientname = string.Empty;
        //    //vFindPatientenabled = true;
        //    btnMatchOrders.Enabled = true;
        //    if (Session["FillOrderException"] != null)
        //    {
        //        IList<FillOrderException> FillOrderExceptionList = (IList<FillOrderException>)Session["FillOrderException"];
        //        IList<FillOrderException> ResultLabExecptionList = new List<FillOrderException>();
        //        if (grdUnassignedResults.SelectedItems.Count > 0)
        //        {
        //            if (FillOrderExceptionList != null)
        //            {
        //                ResultLabExecptionList = FillOrderExceptionList.Where(a => a.Result_Master_ID == Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[21].Text)).ToList<FillOrderException>();
        //                if (ResultLabExecptionList != null)
        //                {
        //                    if (ResultLabExecptionList.Count > 0)
        //                    {
        //                        if (ResultLabExecptionList[0].Reason_Code != "ACUR_LAB_05" || ResultLabExecptionList[0].Reason_Code != "ACUR_LAB_06")
        //                        {
        //                            //CAP-3208
        //                            if (cboProviderName.Text.ToUpper() == "ALL")
        //                            {

        //                                for (int i = 0; i < cboUnmatchProvider.Items.Count; i++)
        //                                {
        //                                    if (grdUnassignedResults.SelectedItems[0].Cells[20].Text.ToString().Trim() == "")
        //                                    {
        //                                        break;
        //                                    }
        //                                    if (cboUnmatchProvider.Items[i].Value.ToString() == grdUnassignedResults.SelectedItems[0].Cells[20].Text.ToString())
        //                                    {
        //                                        cboUnmatchProvider.Enabled = false;
        //                                        chkUnmatchedProvider.Enabled = false;
        //                                        cboUnmatchProvider.SelectedIndex = i;
        //                                        cboCheck = true;
        //                                        break;
        //                                    }
        //                                }
        //                                if (!cboCheck)
        //                                {
        //                                    cboUnmatchProvider.SelectedIndex = 0;
        //                                    cboUnmatchProvider.Enabled = true;
        //                                    chkUnmatchedProvider.Enabled = true;

        //                                }

        //                            }
        //                            else if (ResultLabExecptionList[0].Reason_Code == "ACUR_LAB_05" || ResultLabExecptionList[0].Reason_Code == "ACUR_LAB_06")
        //                            {
        //                                cboUnmatchProvider.Enabled = true;
        //                                chkUnmatchedProvider.Enabled = true;
        //                            }
        //                            else
        //                            {
        //                                cboUnmatchProvider.Enabled = false;
        //                                chkUnmatchedProvider.Enabled = false;

        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            if (Convert.ToUInt32(grdUnassignedResults.SelectedItems[0].Cells[18].Text.ToString()) != 0)
        //            {
        //                Clear();
        //                if (ResultLabExecptionList != null)
        //                {
        //                    if (ResultLabExecptionList.Count > 0)
        //                    {
        //                        //CAP - 3207
        //                        //txtPatientName.Text = ResultLabExecptionList[0].Last_Name_In_Capella + " " + ResultLabExecptionList[0].First_Name_In_Capella + " " + ResultLabExecptionList[0].MI_In_Capella;
        //                        //txtAccountNumber.Text = ResultLabExecptionList[0].Human_Id_In_Capella.ToString();
        //                        //txtDOB.Text = ResultLabExecptionList[0].DOB_In_Capella.ToString("dd-MMM-yyyy");
        //                        //txtGender.Text = ResultLabExecptionList[0].Sex_In_Capella;
        //                        //sPatientname = (ResultLabExecptionList[0].Last_Name_In_Capella + " " + ResultLabExecptionList[0].First_Name_In_Capella + " " + ResultLabExecptionList[0].MI_In_Capella + " | "+ ResultLabExecptionList[0].Human_Id_In_Capella.ToString() + " | " + ResultLabExecptionList[0].DOB_In_Capella.ToString("dd-MMM-yyyy") + " | "+ ResultLabExecptionList[0].Sex_In_Capella);
        //                        sPatientname = (ResultLabExecptionList[0].Last_Name_In_Capella + "," + ResultLabExecptionList[0].First_Name_In_Capella + " " + ResultLabExecptionList[0].MI_In_Capella + " | DOB: " + ResultLabExecptionList[0].DOB_In_Capella.ToString("dd-MMM-yyyy") + " | " + ResultLabExecptionList[0].Sex_In_Capella + " | ACC#: " + ResultLabExecptionList[0].Human_Id_In_Capella.ToString() + " | EX.ACC#: " + ResultLabExecptionList[0].Patient_Account_External.ToString() + " | ADDR: " + ResultLabExecptionList[0].Street_Address1.ToString() + " | Ph: " + ResultLabExecptionList[0].Home_Phone_No.ToString() + " | PATIENT TYPE: " + ResultLabExecptionList[0].Human_Type.ToString());
        //                        FillOutstandingOrdersGrid(ResultLabExecptionList[0].OrdersList);
        //                        if (Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[6].Text.ToString()) != 0)
        //                        {
        //                            grdOutstandingOrders.ClientSettings.Selecting.AllowRowSelect = false;
        //                        }
        //                        else
        //                        {
        //                            grdOutstandingOrders.ClientSettings.Selecting.AllowRowSelect = true;
        //                        }
        //                        //CAP - 3207
        //                        //btnFindPatient.Enabled = false;
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                Clear();
        //                vFindPatientenabled = false;
        //            }
        //        }
        //    }
        //    //CAP - 3207
        //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "SearchLab", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "SearchLab", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}FindPatientenabled('" + vFindPatientenabled + "','" + sPatientname + "')", true);

        //}
        protected void SearchClick_Click(object sender, EventArgs e)
        {
            btnSearch_Click(new Object(), new EventArgs());
        }

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            ulong HumanID = 0;
            if (hdnHumanID.Value != null && hdnHumanID.Value != string.Empty)
                HumanID = Convert.ToUInt64(hdnHumanID.Value);

            if (HumanID > 0)
            {
                HumanManager objHumanProxy = new HumanManager();
                var serializer = new NetDataContractSerializer();
                IList<Human> objHuman = objHumanProxy.GetPatientDetailsUsingPatientInformattion(HumanID);
                if (objHuman != null && objHuman.Count > 0)
                {
                    //CAP - 3207
                    //txtPatientName.Text = objHuman[0].Last_Name + "," + objHuman[0].First_Name + "  " + objHuman[0].MI;
                    //txtAccountNumber.Text = objHuman[0].Id.ToString();
                    //txtDOB.Text = objHuman[0].Birth_Date.ToString("dd-MMM-yyyy");
                    //txtGender.Text = objHuman[0].Sex;
                }
                //CAP - 3207
                //IList<Orders> listOrders = orderProxy.GetLabProcedureBy_ObjectType_And_CurrentProcess_And_HumanId("DIAGNOSTIC ORDER", "RESULT_PROCESS", Convert.ToUInt64(txtAccountNumber.Text));
                IList<Orders> listOrders = orderProxy.GetLabProcedureBy_ObjectType_And_CurrentProcess_And_HumanId("DIAGNOSTIC ORDER", "RESULT_PROCESS", Convert.ToUInt64(HumanID));
                if (listOrders != null)
                    FillOutstandingOrdersGrid(listOrders);
                //Jira Cap - 3206
                //if (grdUnassignedResults.SelectedItems.Count > 0)
                //{
                //    if (Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[6].Text.ToString()) != 0)
                //        grdOutstandingOrders.ClientSettings.Selecting.AllowRowSelect = false;
                //    //CAP-3225
                //    else
                //        grdOutstandingOrders.ClientSettings.Selecting.AllowRowSelect = true;
                //}
                //else
                //    grdOutstandingOrders.ClientSettings.Selecting.AllowRowSelect = true;
            }
        }





        protected void chkProviderName_CheckedChanged(object sender, EventArgs e)
        {
            //CAP-3208
            //cboProviderName.Items.Clear();
            //if (chkProviderName.Checked == true)
            //{
            //    PhyUserList = objPhysicianManager.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);
            //    cboProviderName.Items.Add(new RadComboBoxItem("ALL"));
            //    if (PhyUserList.PhyList != null)
            //    {
            //        for (int i = 0; i < PhyUserList.PhyList.Count; i++)
            //        {
            //            string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;
            //            cboProviderName.Items.Add(new RadComboBoxItem(sPhyName));
            //            cboProviderName.Items[i + 1].Value = PhyUserList.PhyList[i].PhyNPI;
            //        }
            //    }
            //    cboProviderName.Items.Add(new RadComboBoxItem("PROVIDER NOT ASSIGNED"));
            //}
            //else
            //{
            //    PhyUserList = objPhysicianManager.GetPhysicianandUser(true, ClientSession.FacilityName, ClientSession.LegalOrg);
            //    cboProviderName.Items.Add(new RadComboBoxItem("ALL"));
            //    if (PhyUserList.PhyList != null)
            //    {
            //        for (int i = 0; i < PhyUserList.PhyList.Count; i++)
            //        {
            //            string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;
            //            cboProviderName.Items.Add(new RadComboBoxItem(sPhyName));
            //            cboProviderName.Items[i + 1].Value = PhyUserList.PhyList[i].PhyNPI;
            //            if (PhyUserList.UserList[i].user_name.ToString() == ClientSession.UserName)
            //            {
            //                cboProviderName.SelectedIndex = i + 1;
            //            }
            //        }
            //    }
            //    cboProviderName.Items.Add(new RadComboBoxItem("PROVIDER NOT ASSIGNED"));
            //}

            cboProviderName.Items.Clear();

            if (chkProviderName.Checked)
            {
                PhyUserList = objPhysicianManager.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);

                cboProviderName.Items.Add(new ListItem("ALL"));

                if (PhyUserList.PhyList != null)
                {
                    for (int i = 0; i < PhyUserList.PhyList.Count; i++)
                    {
                        var phy = PhyUserList.PhyList[i];
                        string sPhyName = $"{phy.PhyPrefix} {phy.PhyFirstName} {phy.PhyMiddleName} {phy.PhyLastName} {phy.PhySuffix}".Trim();
                        cboProviderName.Items.Add(new ListItem(sPhyName, phy.PhyNPI));
                    }
                }

                cboProviderName.Items.Add(new ListItem("PROVIDER NOT ASSIGNED"));
            }
            else
            {
                PhyUserList = objPhysicianManager.GetPhysicianandUser(true, ClientSession.FacilityName, ClientSession.LegalOrg);

                cboProviderName.Items.Add(new ListItem("ALL"));

                if (PhyUserList.PhyList != null)
                {
                    for (int i = 0; i < PhyUserList.PhyList.Count; i++)
                    {
                        var phy = PhyUserList.PhyList[i];
                        string sPhyName = $"{phy.PhyPrefix} {phy.PhyFirstName} {phy.PhyMiddleName} {phy.PhyLastName} {phy.PhySuffix}".Trim();
                        var item = new ListItem(sPhyName, phy.PhyNPI);
                        cboProviderName.Items.Add(item);

                        if (PhyUserList.UserList[i].user_name.Equals(ClientSession.UserName, StringComparison.OrdinalIgnoreCase))
                        {
                            item.Selected = true;
                        }
                    }
                }

                cboProviderName.Items.Add(new ListItem("PROVIDER NOT ASSIGNED", "PROVIDER NOT ASSIGNED"));
            }
        }
        //Jira Cap - 3206
        //protected void btnMatchOrders_Click(object sender, EventArgs e)
        //{
        //    ulong HumanID = 0;
        //    if (hdnHumanID.Value != null && hdnHumanID.Value != string.Empty)
        //        HumanID = Convert.ToUInt64(hdnHumanID.Value);
        //    bool bCheck = false;
        //    string NPINumbers = string.Empty;

        //    ulong ulAutoPhysID = Convert.ToUInt64(System.Configuration.ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"]);
        //    //CAP - 3207
        //    //if (txtPatientName.Text == "" && txtAccountNumber.Text == "")
        //    //{
        //    //    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('7100013');", true);
        //    //    return;

        //    //}
        //    if (grdUnassignedResults.SelectedItems.Count > 0)
        //    {
        //        if (cboUnmatchProvider.Text != "")
        //        {

        //            if (grdUnassignedResults.SelectedItems[0].Cells[19].Text.ToString() == "ACUR_LAB_05" || grdUnassignedResults.SelectedItems[0].Cells[19].Text.ToString() == "ACUR_LAB_06")
        //                bCheck = true;
        //            //}
        //            if (cboUnmatchProvider.SelectedIndex != null)
        //                NPINumbers = cboUnmatchProvider.Items[cboUnmatchProvider.SelectedIndex].Value.ToString();
        //            if (NPINumbers == "")
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('7100011');", true);
        //                return;

        //            }
        //        }
        //        else if (cboUnmatchProvider.Enabled == true && (grdUnassignedResults.SelectedItems[0].Cells[19].Text.ToString() == "ACUR_LAB_05" || grdUnassignedResults.SelectedItems[0].Cells[19].Text.ToString() == "ACUR_LAB_06"))
        //        {
        //            if (cboUnmatchProvider.Text == "")
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('7100012');", true);
        //                return;

        //            }
        //            if (cboUnmatchProvider.SelectedIndex != null)
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('7100011');", true);
        //                return;
        //            }
        //            if (cboUnmatchProvider.Items[cboUnmatchProvider.SelectedIndex].Value.ToString() == "")
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('7100011');", true);
        //                return;
        //            }
        //            NPINumbers = cboUnmatchProvider.Items[cboUnmatchProvider.SelectedIndex].Value.ToString();
        //            bCheck = true;
        //        }
        //    }

        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('7100001');", true);
        //        return;
        //    }

        //    if (NPINumbers.Trim() == "")
        //    {
        //        NPINumbers = cboProviderName.SelectedValue;
        //    }

        //    //if (!chkNoOrders.Checked && grdOutstandingOrders.SelectedItems.Count > 0 || grdOutstandingOrders.ClientSettings.Selecting.AllowRowSelect == false)
        //    //{
        //    //    ulong Order_Submit_ID = 0;
        //    //    if (grdOutstandingOrders.Items.Count > 0 && grdOutstandingOrders.ClientSettings.Selecting.AllowRowSelect == false)
        //    //        Order_Submit_ID = Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[6].Text.ToString());
        //    //    else if (grdOutstandingOrders.Items.Count == 0 && (grdUnassignedResults.SelectedItems.Count > 0 && grdUnassignedResults.SelectedItems[0].Cells[6].Text.ToString() != "0"))
        //    //        Order_Submit_ID = Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[6].Text.ToString());
        //    //    else
        //    //        Order_Submit_ID = Convert.ToUInt64(grdOutstandingOrders.SelectedItems[0].Cells[6].Text.ToString());


        //    //    resultmasterMngr.UpdateResultMasterAndWf_Object(Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[21].Text.ToString()), Order_Submit_ID, Convert.ToUInt64(txtAccountNumber.Text), NPINumbers, string.Empty);
        //    //    btnSearch_Click(new object(), new EventArgs());
        //    //    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7100010');", true);

        //    //}
        //    //else if (chkNoOrders.Checked && grdOutstandingOrders.SelectedItems.Count == 0 || !chkNoOrders.Checked && grdOutstandingOrders.SelectedItems.Count == 0)
        //    //{
        //    //    if (grdUnassignedResults.SelectedItems.Count > 0)
        //    //    {
        //    //        resultmasterMngr.UpdateResultMasterListForLab(Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[21].Text.ToString()), Convert.ToUInt64(txtAccountNumber.Text), Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[16].Text.ToString()), Convert.ToUInt32(grdUnassignedResults.SelectedItems[0].Cells[18].Text.ToString()), NPINumbers, bCheck, string.Empty);
        //    //        btnSearch_Click(new object(), new EventArgs());
        //    //        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7100010');", true);
        //    //    }

        //    //}

        //    if (!chkNoOrders.Checked)
        //    {
        //        if (grdOutstandingOrders.SelectedItems.Count > 0 || grdOutstandingOrders.ClientSettings.Selecting.AllowRowSelect == false)
        //        {
        //            ulong Order_Submit_ID = 0;
        //            if (grdOutstandingOrders.Items.Count > 0 && grdOutstandingOrders.ClientSettings.Selecting.AllowRowSelect == false)
        //                Order_Submit_ID = Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[6].Text.ToString());
        //            else if (grdOutstandingOrders.Items.Count == 0 && (grdUnassignedResults.SelectedItems.Count > 0 && grdUnassignedResults.SelectedItems[0].Cells[6].Text.ToString() != "0"))
        //                Order_Submit_ID = Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[6].Text.ToString());
        //            else
        //                Order_Submit_ID = Convert.ToUInt64(grdOutstandingOrders.SelectedItems[0].Cells[6].Text.ToString());

        //            //CAP-3207
        //            //resultmasterMngr.UpdateResultMasterAndWf_Object(Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[21].Text.ToString()), Order_Submit_ID, Convert.ToUInt64(txtAccountNumber.Text), NPINumbers, string.Empty);
        //            resultmasterMngr.UpdateResultMasterAndWf_Object(Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[21].Text.ToString()), Order_Submit_ID, Convert.ToUInt64(HumanID), NPINumbers, string.Empty);
        //            btnSearch_Click(new object(), new EventArgs());
        //            //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7100010');", true);                    
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7100010'); FindPatientenabled('Success','');", true);
        //        }
        //        else
        //        {
        //            //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('7100016');", true);
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();DisplayErrorMessage('7100016');} ", true);

        //            return;
        //        }
        //    }
        //    else if (chkNoOrders.Checked)
        //    {
        //        //if (grdOutstandingOrders.SelectedItems.Count == 0 || !chkNoOrders.Checked && grdOutstandingOrders.SelectedItems.Count == 0)
        //        //{
        //        //if (grdUnassignedResults.SelectedItems.Count > 0)
        //        //{
        //        //CAP-3207
        //        //resultmasterMngr.UpdateResultMasterListForLab(Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[21].Text.ToString()), Convert.ToUInt64(txtAccountNumber.Text), Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[16].Text.ToString()), Convert.ToUInt32(grdUnassignedResults.SelectedItems[0].Cells[18].Text.ToString()), NPINumbers, bCheck, string.Empty, ulAutoPhysID);
        //        resultmasterMngr.UpdateResultMasterListForLab(Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[21].Text.ToString()), Convert.ToUInt64(HumanID), Convert.ToUInt64(grdUnassignedResults.SelectedItems[0].Cells[16].Text.ToString()), Convert.ToUInt32(grdUnassignedResults.SelectedItems[0].Cells[18].Text.ToString()), NPINumbers, bCheck, string.Empty, ulAutoPhysID);
        //        btnSearch_Click(new object(), new EventArgs());
        //        //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7100010');", true);
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7100010'); FindPatientenabled('Success','');", true);
        //        //}
        //        //}
        //    }

        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "MatchOrder", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //}

        //Jira Cap - 3206
        //protected void chkUnmatchedProvider_CheckedChanged(object sender, EventArgs e)
        //{
        //    cboUnmatchProvider.Items.Clear();
        //    cboUnmatchProvider.Items.Add(new RadComboBoxItem(""));
        //    cboUnmatchProvider.Items[0].Value = "";
        //    if (chkUnmatchedProvider.Checked == true)
        //    {
        //        phyUnmatchedList = objPhysicianManager.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);

        //        if (phyUnmatchedList.PhyList != null)
        //        {
        //            for (int i = 0; i < phyUnmatchedList.PhyList.Count; i++)
        //            {
        //                string sPhyName = phyUnmatchedList.PhyList[i].PhyPrefix + " " + phyUnmatchedList.PhyList[i].PhyFirstName + " " + phyUnmatchedList.PhyList[i].PhyMiddleName + " " + phyUnmatchedList.PhyList[i].PhyLastName + " " + phyUnmatchedList.PhyList[i].PhySuffix;
        //                cboUnmatchProvider.Items.Add(new RadComboBoxItem(sPhyName.Trim()));
        //                cboUnmatchProvider.Items[i + 1].Value = phyUnmatchedList.PhyList[i].PhyNPI;


        //            }
        //        }
        //    }
        //    else
        //    {
        //        phyUnmatchedList = objPhysicianManager.GetPhysicianandUser(true, ClientSession.FacilityName, ClientSession.LegalOrg);

        //        if (phyUnmatchedList.PhyList != null)
        //        {
        //            for (int i = 0; i < phyUnmatchedList.PhyList.Count; i++)
        //            {
        //                string sPhyName = phyUnmatchedList.PhyList[i].PhyPrefix + " " + phyUnmatchedList.PhyList[i].PhyFirstName + " " + phyUnmatchedList.PhyList[i].PhyMiddleName + " " + phyUnmatchedList.PhyList[i].PhyLastName + " " + phyUnmatchedList.PhyList[i].PhySuffix;
        //                cboUnmatchProvider.Items.Add(new RadComboBoxItem(sPhyName.Trim()));
        //                cboUnmatchProvider.Items[i + 1].Value = phyUnmatchedList.PhyList[i].PhyNPI;


        //            }
        //        }

        //    }
        //}
        protected void grdUnassignedResults_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataBoundItem = e.Item as GridDataItem;
                if (dataBoundItem["Abnormal"].Text == "Yes")
                {
                    //dataBoundItem["Abnormal"].ForeColor = Color.Red; // change color for particuler cell
                    e.Item.ForeColor = System.Drawing.Color.Red; // for whole row
                }
            }
        }

        protected void grdUnassignedResults_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {

                //Jira Cap - 3206
                // if (grdUnassignedResults.SelectedItems.Count == 0)
                //{
                // btnFindPatient.Enabled = false;
                //btnMatchOrders.Enabled = false;
                //}
                ulong resultID = Convert.ToUInt64(e.Item.Cells[21].Text);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Open", "Result('" + resultID + "');", true);
            }
        }


        //Jira Cap - 3206
        private static string Compress(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(inputBytes, 0, inputBytes.Length);
                }
                return Convert.ToBase64String(outputStream.ToArray());
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object LoadResults()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            HttpContext.Current.Session.Remove("SearchOrder");

            string extra_search = HttpContext.Current.Request.Params["extra_search"];
            var searchData = JsonConvert.DeserializeObject<Dictionary<string, string>>(extra_search);
            string sCriteria = searchData["sCriteria"];
            string NPINumbers = searchData["NPINumbers"];


            IList<string> FieldName = new List<string>();
            IList<string> FieldValue = new List<string>();
            for (int i = 0; i < searchData["FieldName"].Split(',').Length; i++)
            {
                FieldName.Add(searchData["FieldName"].Split(',')[i]);
            }

            for (int j = 0; j < searchData["FieldValue"].Split(',').Length; j++)
            {
                FieldValue.Add(searchData["FieldValue"].Split(',')[j]);
            }



            IList<FillOrderException> ilstFillOrderException = new List<FillOrderException>();
            ResultMasterManager objresultMasterMngr = new ResultMasterManager();
            ilstFillOrderException = objresultMasterMngr.GetOrderExceptionItems(sCriteria, NPINumbers, FieldName, FieldValue);
            HttpContext.Current.Session["FillOrderException"] = ilstFillOrderException;
            if (ilstFillOrderException != null)
            {
                ilstFillOrderException = ilstFillOrderException.OrderByDescending(a => a.Is_Abnormal).ToList();
            }
            var resultNew = new
            {
                data = Compress(JsonConvert.SerializeObject(ilstFillOrderException.ToList<FillOrderException>())),
            };
            return resultNew;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static string FillOutstandingorders(string ResultMasterID)
        {

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            //string extra_search = HttpContext.Current.Request.Params["extra_search"];
            //var searchData = JsonConvert.DeserializeObject<Dictionary<string, string>>(extra_search);
            //string Result_Master_ID = searchData["Result_Master_ID"];


            IList<FillOrderException> ResultLabExecptionList = new List<FillOrderException>();
            if (HttpContext.Current.Session["FillOrderException"] != null)
            {
                IList<FillOrderException> FillOrderExceptionList = (IList<FillOrderException>)HttpContext.Current.Session["FillOrderException"];


                if (FillOrderExceptionList != null)
                {
                    ResultLabExecptionList = FillOrderExceptionList.Where(a => a.Result_Master_ID == Convert.ToUInt64(ResultMasterID)).ToList<FillOrderException>();
                }
            }

            return JsonConvert.SerializeObject(ResultLabExecptionList);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object GetOutstandingOrdersList(string HumanId)
        {

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            OrdersManager orderProxy = new OrdersManager();
            IList<Orders> listOrders = orderProxy.GetLabProcedureBy_ObjectType_And_CurrentProcess_And_HumanId("DIAGNOSTIC ORDER", "RESULT_PROCESS", Convert.ToUInt64(HumanId));



            return JsonConvert.SerializeObject(listOrders);
        }


        [WebMethod(EnableSession = true)]
        public static string MatchButtonClick(string Result_Master_ID, string Order_Submit_ID, bool chkNoOrders, string HumanID, string NPINumbers, string Matching_Patient_ID)
        {

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            if (HumanID == "")
            {
                HumanID = "0";
            }

            bool bCheck = true;
            ulong ulAutoPhysID = Convert.ToUInt64(System.Configuration.ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"]);
            ResultMasterManager resultmasterMngr = new ResultMasterManager();

            string Val = string.Empty;
            if (!chkNoOrders)
            {
                resultmasterMngr.UpdateResultMasterAndWf_Object(Convert.ToUInt64(Result_Master_ID), Convert.ToUInt64(Order_Submit_ID), Convert.ToUInt64(HumanID), NPINumbers, string.Empty);
                Val = "7100010";
            }
            else if (chkNoOrders)
            {
                resultmasterMngr.UpdateResultMasterListForLab(Convert.ToUInt64(Result_Master_ID), Convert.ToUInt64(HumanID), Convert.ToUInt64(Order_Submit_ID), Convert.ToUInt32(Matching_Patient_ID), NPINumbers, bCheck, string.Empty, ulAutoPhysID);
                Val = "7100010";
            }
            return JsonConvert.SerializeObject(Val);
        }
        [WebMethod(EnableSession = true)]
        public static string chkUnmatchedProvidercheck(bool check)
        {
            FillPhysicianUser phyUnmatchedList;
            PhysicianManager objPhysicianManager = new PhysicianManager();
            if (check == true)
            {
                phyUnmatchedList = objPhysicianManager.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);
            }
            else
            {
                phyUnmatchedList = objPhysicianManager.GetPhysicianandUser(true, ClientSession.FacilityName, ClientSession.LegalOrg);

            }

            return JsonConvert.SerializeObject(phyUnmatchedList);
        }


        [WebMethod(EnableSession = true)]
        public static string chkProviderNameCheckedChanged(bool check)
        {

            FillPhysicianUser PhyUserList;
            PhysicianManager objPhysicianManager = new PhysicianManager();

            if (check)
            {
                PhyUserList = objPhysicianManager.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);

            }
            else
            {
                PhyUserList = objPhysicianManager.GetPhysicianandUser(true, ClientSession.FacilityName, ClientSession.LegalOrg);

            }
            return JsonConvert.SerializeObject(PhyUserList);
        }
        //CAP-3362
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GenerateHumanDetails(string HumanID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            OrdersManager orderProxy = new OrdersManager();
            IList<Orders> listOrders = orderProxy.GetLabProcedureBy_ObjectType_And_CurrentProcess_And_HumanId("DIAGNOSTIC ORDER", "RESULT_PROCESS", Convert.ToUInt64(HumanID));           

            HumanManager HumanMngr = new HumanManager();
            Human objhm = HumanMngr.GetHumanFromHumanID(Convert.ToUInt32(HumanID));
            string sPatientname = (objhm.Last_Name + "," + objhm.First_Name + " " + objhm.MI + " | DOB: " + objhm.Date_Of_Death.ToString("dd-MMM-yyyy") + " | " + objhm.Sex + " | ACC#: " + objhm.Id.ToString() + " | EX.ACC#: " + objhm.Patient_Account_External.ToString() + " | ADDR: " + objhm.Street_Address1.ToString() + " | Ph: " + objhm.Home_Phone_No.ToString() + " | PATIENT TYPE: " + objhm.Human_Type.ToString());

            var result = new { HumanDetails = sPatientname, listOrders = listOrders };
            
            return JsonConvert.SerializeObject(result);
        }

    }
}
