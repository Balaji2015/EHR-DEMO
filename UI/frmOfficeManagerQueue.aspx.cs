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
using Acurus.Capella.Core.DTO;
using Telerik.Web.UI;
using System.Collections.Generic;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Xml;
using System.IO;

namespace Acurus.Capella.UI
{
    public partial class frmOfficeManagerQueue : System.Web.UI.Page
    {
        WFObjectManager objWFObjectMngr = new WFObjectManager();
        WorkFlowManager objWorkFlowMngr = new WorkFlowManager();
        FacilityManager FacProxy = new FacilityManager();
        IList<WorkFlow> WFList = null;
        UtilityManager utilityMngr = new UtilityManager();
        DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sClientIPAddress = string.Empty;
                sClientIPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (sClientIPAddress == null || sClientIPAddress == "")
                {
                    sClientIPAddress = Request.ServerVariables["REMOTE_ADDR"];
                }

                ClientSession.processCheck = false;
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);
                //IList<FacilityLibrary> FacList = null;
                //FacList = FacProxy.GetFacilityList();

                //-----Added By nijanthan(19-11-15)
                OfficeManagerDTO OfficeManagerDTO =FacProxy.GetFacilityListAndWorkflowList(ClientSession.FacilityName);
                //if (OfficeManagerDTO.lstFacilityLibrary.Count>0)
                //{
                //    for (int i = 0; i < OfficeManagerDTO.lstFacilityLibrary.Count; i++)
                //    {
                //        cboFacilityName.Items.Add(new RadComboBoxItem(OfficeManagerDTO.lstFacilityLibrary[i].Fac_Name.ToString()));
                //        if (OfficeManagerDTO.lstFacilityLibrary[i].Fac_Name.ToString() == ClientSession.FacilityName)
                //        {
                //            cboFacilityName.SelectedIndex = i;
                //        }
                //    }
                //}
                XmlDocument xmldoc = new XmlDocument();
                string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\Facility_Library.xml");
                if (File.Exists(strXmlFilePath) == true)
                {
                    xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "Facility_Library" + ".xml");
                    XmlNodeList xmlFacilityList = xmldoc.GetElementsByTagName("Facility");
                 
                    if (xmlFacilityList.Count > 0)
                    {
                        foreach (XmlNode item in xmlFacilityList)
                        {
                            if (item != null && item.Attributes.GetNamedItem("Legal_Org").Value == ClientSession.LegalOrg)
                                cboFacilityName.Items.Add(new RadComboBoxItem(item.Attributes[0].Value));

                            if (item != null && item.Attributes[0].Value==ClientSession.FacilityName)
                                cboFacilityName.SelectedIndex = cboFacilityName.Items.IndexOf(cboFacilityName.Items.FindItemByText(item.Attributes[0].Value.ToString()));
                        }
                    }
                }

                //commented by nijanthan
                //if (FacList.Count > 0)
                //{
                //    for (int i = 0; i < FacList.Count; i++)
                //    {
                //        cboFacilityName.Items.Add(new RadComboBoxItem(FacList[i].Fac_Name.ToString()));
                //        if (FacList[i].Fac_Name.ToString() == ClientSession.FacilityName)
                //        {
                //            cboFacilityName.SelectedIndex = i;
                //        }
                //    }
                //}

                //WFList = objWorkFlowMngr.GetWorkFlowMapListbyFacilityName(ClientSession.FacilityName);


                //-----Added By nijanthan(19-11-15)
                if (OfficeManagerDTO.listWorkflow.Count>0)
                {
                    WFList = OfficeManagerDTO.listWorkflow;
                }

                ViewState["WFList"] = WFList;


                //-----Added By nijanthan(18-11-15)
                var objTypeList = (from obj in WFList select new { obj.Obj_Type }).Distinct().OrderBy(o=>o.Obj_Type);
                cboObjectType.Items.Add(new RadComboBoxItem(""));
                //-----Added By nijanthan(17-11-15)
                if (objTypeList.Count() > 0)
                {
                    foreach (var objType in objTypeList)
                    {
                        cboObjectType.Items.Add(new RadComboBoxItem(objType.Obj_Type.ToString()));
                    }
                    if (cboObjectType.Items.Count > 0)
                    {
                        cboObjectType.SelectedIndex = 0;
                    }
                }
                //-----Added By nijanthan(17-11-15)
                if (cboObjectType.Text != string.Empty || cboObjectType.Text != "")
                {
                    FillCurrentProcess(cboObjectType.Text);
                }


                lblResult.Text = string.Empty;
                //mpnOfficeManagerQueue.Reset();
            }

        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            //grdWFObject.DataSource = null;
           // grdWFObject.DataBind();
            lblResult.Text = string.Empty;
           // mpnOfficeManagerQueue.Reset();
            cboCurrentProcess.SelectedIndex = 0;
            cboObjectType.SelectedIndex = 0;
            iFrame.Attributes.Add("src", "");
        }


        protected void btnGetObjects_Click(object sender, EventArgs e)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "Open", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);
            string strCurrentProcess = string.Empty;
           
            if(cboObjectType.SelectedItem.Text=="")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Test", "DisplayErrorMessage('980010'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            if (cboCurrentProcess.SelectedItem.Text == "")
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Test", "DisplayErrorMessage('980011'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //return;
                strCurrentProcess = "%25";
            }
            else
            {
                strCurrentProcess = cboCurrentProcess.SelectedItem.Text;
            }

            //string sBIRTReportUrl = System.Configuration.ConfigurationManager.AppSettings["BIRTReportUrl"].ToString();
            string sPath = string.Empty;
            NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration().Configure();
            //string[] conString = cfg.GetProperty(NHibernate.Cfg.Environment.ConnectionString).ToString().Split(';');
            string[] conString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ToString().Split(';');
            string sDataBase = string.Empty;
            string sDataSource = string.Empty;
            string sUserId = string.Empty;
            string sPassword = string.Empty;
            string sPort = "3306";
            for (int i = 0; i < conString.Length; i++)
            {
                if (conString[i].ToString().ToUpper().Contains("DATABASE=") == true)
                {
                    sDataBase = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("DATA SOURCE") == true)
                {
                    sDataSource = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("USER ID") == true)
                {
                    sUserId = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("PASSWORD") == true)
                {
                    sPassword = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("PORT") == true)
                {
                    sPort = conString[i].ToString().Split('=')[1];
                }
            }
            string sodaURL = string.Empty;
            string sAzure = System.Configuration.ConfigurationManager.AppSettings["Azure"].ToString();
            if (sAzure == "Y")
                sodaURL = "jdbc:mysql://" + sDataSource + ":" + sPort + "/" + sDataBase + "?useSSL=true&requireSSL=false";
            else
                sodaURL = "jdbc:mysql://" + sDataSource + ":" + sPort + "/" + sDataBase;

            string sodaUser = sUserId;
            string sodaPassword = sPassword;
            string sDBConnection = "&odaURL=" + sodaURL + "&odaUser=" + sodaUser + "&odaPassword=" + sodaPassword;
            string sBIRTReportUrl = System.Configuration.ConfigurationManager.AppSettings["BIRTReportUrl_" + ClientSession.LegalOrg].ToString();
            //strPath = sBIRTReportUrl + "Human_Details.rptdesign" + "&strDenID=" + strDenID + "&strNumId=" + strNumId + "&odaURL=" + sodaURL + "&odaUser=" + sodaUser + "&odaPassword=" + sodaPassword;
            /*Modified by balaji.TJ*/
            if (cboObjectType.Text == "ENCOUNTER" || cboObjectType.Text == "DOCUMENTATION" || cboObjectType.Text == "DOCUMENT REVIEW" || cboObjectType.Text == "PHONE ENCOUNTER" || cboObjectType.Text == "BILLING")
            {
                sPath = sBIRTReportUrl + "CAPELLA_" +ClientSession.LegalOrg + "_ENCOUNTER_REPORT.rptdesign" + sDBConnection + "&strObjType=" + cboObjectType.Text + "&strFacilityName=" + cboFacilityName.Text.Replace("#", "%23") + "&strCurrentProcess=" + strCurrentProcess + "&legal_org=" + ClientSession.LegalOrg + "&__title=";
            }
            else if (cboObjectType.Text == "SCAN")
            {
                sPath = sBIRTReportUrl + "CAPELLA_" + ClientSession.LegalOrg + "_SCAN_REPORT.rptdesign" + sDBConnection + "&strObjType=" + cboObjectType.Text + "&strFacilityName=" + cboFacilityName.Text.Replace("#", "%23") + "&strCurrentProcess=" + strCurrentProcess + "&legal_org=" + ClientSession.LegalOrg + "&__title=";
            }
            else if (cboObjectType.Text == "ADDENDUM")
            {
                sPath = sBIRTReportUrl + "CAPELLA_" + ClientSession.LegalOrg + "_ADDENDUM_REPORT.rptdesign" + sDBConnection + "&strObjType=" + cboObjectType.Text + "&strFacilityName=" + cboFacilityName.Text.Replace("#", "%23") + "&strCurrentProcess=" + strCurrentProcess + "&legal_org=" + ClientSession.LegalOrg + "&__title=";
            }
            else if (cboObjectType.Text == "DIAGNOSTIC ORDER" || cboObjectType.Text == "IMAGE ORDER")
            {
                sPath = sBIRTReportUrl + "CAPELLA_" + ClientSession.LegalOrg + "_DIAGNOSTIC ORDER_REPORT.rptdesign" + sDBConnection + "&strObjType=" + cboObjectType.Text + "&strFacilityName=" + cboFacilityName.Text.Replace("#", "%23") + "&strCurrentProcess=" + strCurrentProcess + "&legal_org=" + ClientSession.LegalOrg + "&__title="; 
            }
            /*Modified by balaji.TJ*/
            else if (cboObjectType.Text == "DIAGNOSTIC_RESULT")
            {
                sPath = sBIRTReportUrl + "CAPELLA_" + ClientSession.LegalOrg + "_DIAGNOSTIC_RESULT_REPORT.rptdesign" + sDBConnection + "&strObjType=" + cboObjectType.Text + "&strFacilityName=" + cboFacilityName.Text.Replace("#", "%23") + "&strCurrentProcess=" + strCurrentProcess + "&legal_org=" + ClientSession.LegalOrg + "&__title=";
            }
            else if (cboObjectType.Text == "IMMUNIZATION ORDER")
            {
                sPath = sBIRTReportUrl + "CAPELLA_" + ClientSession.LegalOrg + "_IMMUNIZATION ORDER_REPORT.rptdesign" + sDBConnection + "&strObjType=" + cboObjectType.Text + "&strFacilityName=" + cboFacilityName.Text.Replace("#", "%23") + "&strCurrentProcess=" + strCurrentProcess + "&legal_org=" + ClientSession.LegalOrg + "&__title="; 
            }
            else if (cboObjectType.Text == "REFERRAL ORDER")
            {
                sPath = sBIRTReportUrl + "CAPELLA_" + ClientSession.LegalOrg + "_REFERRAL ORDER_REPORT.rptdesign" + sDBConnection + "&strObjType=" + cboObjectType.Text + "&strFacilityName=" + cboFacilityName.Text.Replace("#", "%23") + "&strCurrentProcess=" + strCurrentProcess + "&legal_org=" + ClientSession.LegalOrg + "&__title="; 
            }
            else if (cboObjectType.Text == "TASK")
            {
                sPath = sBIRTReportUrl + "CAPELLA_" + ClientSession.LegalOrg + "_TASK_REPORT.rptdesign" + sDBConnection + "&strObjType=" + cboObjectType.Text + "&strFacilityName=" + cboFacilityName.Text.Replace("#", "%23") + "&strCurrentProcess=" + strCurrentProcess + "&legal_org=" + ClientSession.LegalOrg + "&__title="; 
            }
            else if (cboObjectType.Text == "E-PRESCRIBE")
            {
                sPath = sBIRTReportUrl + "CAPELLA_" + ClientSession.LegalOrg + "_E-PRESCRIPTION_REPORT.rptdesign" + sDBConnection + "&strObjType=" + cboObjectType.Text + "&strFacilityName=" + cboFacilityName.Text.Replace("#", "%23") + "&strCurrentProcess=" + strCurrentProcess + "&legal_org=" + ClientSession.LegalOrg + "&__title="; 
            }
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            iFrame.Attributes.Add("src", sPath);
           // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Close", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

            #region commented
            //grdWFObject.DataSource = null;
            //if(hdnTableName.Value=="")
            //{
            //    hdnTableName.Value = "0";
            //}
            //DataSet ds = new DataSet();
            //if (hdnNavigator.Value == "" || hdnNavigator.Value==string.Empty)
            //{
            //    mpnOfficeManagerQueue.Reset();
            //    ds = objWFObjectMngr.GetListOfObjectsByFaciltyObjTypeCurrentProcess(cboFacilityName.Text, cboObjectType.Text, cboCurrentProcess.Text, mpnOfficeManagerQueue.PageNumber, mpnOfficeManagerQueue.MaxResultPerPage, Convert.ToInt32(hdnTableName.Value), hdnEncouterAndArcCount.Value);
            //}
            //else
            //{
            //    if (hdnFacilityObjTypeCurProc.Value!=string.Empty)
            //    {
            //        ds = objWFObjectMngr.GetListOfObjectsByFaciltyObjTypeCurrentProcess(hdnFacilityObjTypeCurProc.Value.Split('|')[0], hdnFacilityObjTypeCurProc.Value.Split('|')[1].Split('$')[0], hdnFacilityObjTypeCurProc.Value.Split('$')[1], mpnOfficeManagerQueue.PageNumber, mpnOfficeManagerQueue.MaxResultPerPage, Convert.ToInt32(hdnTableName.Value), hdnEncouterAndArcCount.Value);
            //    }
                
            //}
            
            //foreach (DataRow row in ds.Tables[0].Rows)
            //{
            //    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            //    {

            //        if (ds.Tables[0].Columns[i].DataType.Name == "DateTime")//if (i == 1 || i==7)
            //        {
            //            //if (ds.Tables[0].Columns[3].ToString() == "Patient DOB")
            //            //{
            //            // row[ds.Tables[0].Columns[i]] = UtilityManager.ConvertToLocal(Convert.ToDateTime(row[ds.Tables[0].Columns[i]])).ToString("dd-MMM-yyyy hh:mm:ss tt");
            //            //DateTime localdate = Convert.ToDateTime(row[ds.Tables[0].Columns[3]]);
            //            //row[ds.Tables[0].Columns[3]] = UtilityManager.ConvertToLocal(localdate).ToString("dd-MMM-yyyy hh:mm tt");
            //            //}
            //            DateTime localdate = Convert.ToDateTime(row[ds.Tables[0].Columns[i]]);
            //            //row[ds.Tables[0].Columns[i]] = UtilityManager.ConvertToLocal(localdate).ToString("dd-MMM-yyyy hh:mm tt");
            //            row[ds.Tables[0].Columns[i]] = localdate.ToString("dd-MMM-yyyy hh:mm tt");
            //        }
            //    }
            //}

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    if (hdnNavigator.Value == "")
            //    {
            //        hdnFacilityObjTypeCurProc.Value = cboFacilityName.Text + '|' + cboObjectType.Text + '$' + cboCurrentProcess.Text;
            //    }
                
            //    grdWFObject.DataSource = ds.Tables[0];
            //    grdWFObject.DataBind();
            //    int grdcolumn = grdWFObject.MasterTableView.AutoGeneratedColumns.Count();
            //    if (grdWFObject.MasterTableView.AutoGeneratedColumns[grdcolumn - 1].HeaderText=="Limit")
            //    {
            //        hdnTableName.Value = ds.Tables[0].Rows[0].ItemArray[grdcolumn - 1].ToString();
            //        grdWFObject.MasterTableView.AutoGeneratedColumns[grdcolumn - 1].Visible = false;
            //        grdWFObject.MasterTableView.AutoGeneratedColumns[grdcolumn - 2].Visible = false;
            //    }
            //    else if (grdWFObject.MasterTableView.AutoGeneratedColumns[grdcolumn - 1].HeaderText == "Total No of Record")
            //    {
            //        grdWFObject.MasterTableView.AutoGeneratedColumns[grdcolumn - 1].Visible = false;
            //    }
                

            //    if (mpnOfficeManagerQueue.PageNumber == 1 && ds.Tables[0].Rows.Count > 0)
            //        if (grdWFObject.MasterTableView.AutoGeneratedColumns[grdcolumn - 1].HeaderText == "Limit")
            //        {
            //            //String TotalRecordsCount = ds.Tables[0].Rows[0].ItemArray[grdcolumn - 2].ToString();
            //            hdnEncouterAndArcCount.Value = ds.Tables[0].Rows[0].ItemArray[grdcolumn - 2].ToString();
            //            mpnOfficeManagerQueue.TotalNoofDBRecords = Convert.ToInt32(hdnEncouterAndArcCount.Value.Split('|')[1]);
            //        }
            //        else
            //        {
            //            //String TotalRecordsCount = ds.Tables[0].Rows[0].ItemArray[grdcolumn - 1].ToString();
            //            //hdnEncouterAndArcCount.Value = ds.Tables[0].Rows[0].ItemArray[grdcolumn - 2].ToString();
            //            //mpnOfficeManagerQueue.TotalNoofDBRecords = Convert.ToInt32(hdnEncouterAndArcCount.Value.Split('|')[1]);
            //            mpnOfficeManagerQueue.TotalNoofDBRecords = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[grdcolumn - 1]);
            //        }

                   
            //    //mpnOfficeManagerQueue.TotalNoofDBRecords = ds.Tables[0].Rows.Count;

            //    ViewState["officeds"] = ds;
            //}
            //else
            //{
            //    hdnFacilityObjTypeCurProc.Value = string.Empty;
            //    lblResult.Text = "No Result(s) Found ";
            //    mpnOfficeManagerQueue.Reset();
            //}

            //if (grdWFObject.MasterTableView.AutoGeneratedColumns.Count() > 0)
            //{
            //    foreach (GridColumn col in grdWFObject.MasterTableView.AutoGeneratedColumns)
            //    {
            //        if (col.HeaderText.StartsWith("Date") == true || col.HeaderText.EndsWith("Date") == true)
            //        {
            //            GridDateTimeColumn dv = (GridDateTimeColumn)col;
            //            dv.DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}";
            //        }
            //        if (col.HeaderText.EndsWith("DOB") == true)
            //        {
            //            GridDateTimeColumn dv = (GridDateTimeColumn)col;
            //            dv.DataFormatString = "{0:dd-MMM-yyyy}";
            //        }
            //        col.ItemStyle.Wrap = true;
            //    }
            //}
            //hdnNavigator.Value = string.Empty;


            //grdWFObject.DataBind();
            #endregion
            
            //ClientScript.RegisterStartupScript(this.GetType(), "Close", "CloseObjects();", true);
            //divLoading.Style.Add("display", "none");
        }
        private void FillCurrentProcess(string objType)
        {
            if (ViewState["WFList"] != null)
            {
                WFList = (IList<WorkFlow>)ViewState["WFList"];
            }

            cboCurrentProcess.Items.Clear();
            // var wf = WFList.Where(a => a.Obj_Type == objType).OrderBy(k => k.To_Process).GroupBy(s => s.To_Process).ToList();//.OrderBy(p=>p.)
            //-----Added By nijanthan(18-11-15)
            var wf = from w in WFList
                     where w.Obj_Type == objType
                     orderby w.To_Process ascending
                     group w by new { w.To_Process } into g
                     select new { g.Key.To_Process };
            cboCurrentProcess.Items.Add(new RadComboBoxItem(""));
            foreach (var wflow in wf)
            {
                cboCurrentProcess.Items.Add(new RadComboBoxItem(wflow.To_Process));
            }
            if (cboCurrentProcess.Items.Count > 0)
            {
                cboCurrentProcess.SelectedIndex = 0;
            }
        }

        protected void cboObjectType_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            FillCurrentProcess(cboObjectType.Text);
            iFrame.Attributes.Add("src", "");
            //grdWFObject.DataSource = null;
          //  grdWFObject.DataBind();
           // mpnOfficeManagerQueue.Reset();
            //divLoading.Style.Add("display", "none");
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Close", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        public void FirstPageNavigator(object sender, EventArgs e)
        {
            hdnNavigator.Value = "True";
            btnGetObjects_Click(sender, e);
        }

        //protected void grdWFObject_PreRender(object sender, EventArgs e)
        //{
        //    foreach (GridColumn column in grdWFObject.MasterTableView.AutoGeneratedColumns)
        //    {
        //        if (column.UniqueName == "Patient Acc#")
        //        {
        //            column.HeaderStyle.Width = Unit.Pixel(150);
        //        }
        //        if (column.UniqueName == "Encounter ID")
        //        {
        //            column.HeaderStyle.Width = Unit.Pixel(150);
        //        }
        //        if (column.UniqueName == "Patient Name")
        //        {
        //            column.HeaderStyle.Width = Unit.Pixel(160);
        //        }

        //    }
        //}

        //protected void grdWFObject_SortCommand(object sender, GridSortCommandEventArgs e)
        //{
        //    ds = (DataSet)ViewState["officeds"];

        //    if (e.NewSortOrder.ToString() == "Ascending")
        //    {
        //        ds.Tables[0].DefaultView.Sort = "Date of Service";
        //    }
        //    else
        //    {
        //        ds.Tables[0].DefaultView.Sort = "Date of Service DESC";
        //    }
        //    ds.AcceptChanges();

        //   // grdWFObject.DataSource = ds.Tables[0];
        //  //  grdWFObject.DataBind();
        //}

        protected void btnHdnClearall_Click(object sender, EventArgs e)
        {
          
            btnClearAll_Click(sender, e);
            
        }

    }
}
