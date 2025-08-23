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
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Telerik.Web.UI;
using System.Xml.XPath;
using Acurus.Capella.UI;
using System.Xml;
using System.IO;
using System.Threading;
using MySql.Data.MySqlClient;
using CrystalDecisions.Web;
using DocumentFormat.OpenXml.Tools.ClassExplorer;
using System.Net;
using System.Text;
using Acurus.Capella.Core.DTOJson;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Text.Json;

namespace Acurus.Capella.UI
{
    public partial class frmWFObjectManager : System.Web.UI.Page
    {
        WFObjectManager objWfObjectMngr = new WFObjectManager();
        WorkFlowManager WorkFlowMngr = new WorkFlowManager();
        ProcUserManager ProcUserMngr = new ProcUserManager();
        ProcessMasterManager ProcessMasterMngr = new ProcessMasterManager();
        WFObjectManager WfObjectMngr = new WFObjectManager();
        CorrectedDBEntriesManager CorrDBEntryProxy = new CorrectedDBEntriesManager();
        EncounterManager EncProxy = new EncounterManager();
        DataSet ds = new DataSet();
        DataTable dtTable = new DataTable();
        //IList<ProcessMaster> proclist;
        //IList<WorkFlow> worflowList;
        //IList<ProcUser> ProcUser = new List<ProcUser>();
        //IList<User> userMyList = new List<User>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["HumanID"] != null)
                    txtAccountNo.Text = Request["HumanID"].ToString();
                else
                    txtAccountNo.Text = ClientSession.HumanId.ToString();
                string sName = string.Empty;
                string sDOB = string.Empty;

                if (Request["PatientName"] == null)
                {
                    IList<string> ilstAdminTagList = new List<string>();
                    ilstAdminTagList.Add("HumanList");

                    IList<object> ilstEditAppFinal = new List<object>();
                    ilstEditAppFinal = UtilityManager.ReadBlob(Convert.ToUInt64(txtAccountNo.Text), ilstAdminTagList);

                    if (ilstEditAppFinal.Count > 0 && ilstEditAppFinal != null)
                    {
                        if (ilstEditAppFinal[0] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstEditAppFinal[0]).Count; iCount++)
                            {


                                if (((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).Birth_Date != null)
                                {
                                    sDOB = Convert.ToDateTime(((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).Birth_Date).ToString("dd-MMM-yyyy");
                                }
                                if (((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).Last_Name != null)
                                {
                                    if (sName == string.Empty)
                                        sName = ((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).Last_Name.ToString();
                                    else
                                        sName += "," + ((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).Last_Name.ToString();
                                }
                                if (((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).First_Name != null)
                                {
                                    if (sName == string.Empty)
                                        sName = ((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).First_Name.ToString();
                                    else
                                        sName += "," + ((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).First_Name.ToString();
                                }
                                if (((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).MI != null)
                                {
                                    if (sName == string.Empty)
                                        sName = ((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).MI.ToString();
                                    else
                                        sName += " " + ((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).MI.ToString();
                                }
                                if (((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).Suffix != null)
                                {
                                    if (sName == string.Empty)
                                        sName = ((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).Suffix.ToString();
                                    else
                                        sName += " " + ((Human)((IList<object>)ilstEditAppFinal[0])[iCount]).Suffix.ToString();
                                }

                            }
                        }

                    }

                    //string FileName = "Human" + "_" + txtAccountNo.Text + ".xml";
                    //string strXmlFilePathHuman = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                    //if (File.Exists(strXmlFilePathHuman) == true)
                    //{
                    //    XmlDocument itemDocHuman = new XmlDocument();
                    //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePathHuman);
                    //    //itemDocHuman.Load(XmlText);
                    //    using (FileStream fs = new FileStream(strXmlFilePathHuman, FileMode.Open, FileAccess.Read, FileShare.Read))
                    //    {
                    //        itemDocHuman.Load(fs);

                    //        XmlText.Close();
                    //        string xmlContent = null;
                    //        XDocument documentnodeHuman = XDocument.Load(strXmlFilePathHuman);

                    //        if (itemDocHuman != null && itemDocHuman.GetElementsByTagName("Modules")[0].ChildNodes.Count > 0)
                    //        {
                    //            for (int i = 0; i < itemDocHuman.GetElementsByTagName("Modules")[0].ChildNodes.Count; i++)
                    //            {
                    //                if (itemDocHuman.GetElementsByTagName("Modules")[0].ChildNodes[i].Name == "HumanList")
                    //                {
                    //                    xmlContent = itemDocHuman.GetElementsByTagName("Modules")[0].ChildNodes[i].OuterXml;
                    //                    XmlDocument documentnode = new XmlDocument();
                    //                    documentnode.LoadXml(xmlContent);
                    //                    XmlNode node = documentnode.DocumentElement;
                    //                    IEnumerator list = node.GetEnumerator();
                    //                    XmlNode tempNode;
                    //                    while (list.MoveNext())
                    //                    {
                    //                        tempNode = (XmlNode)list.Current;

                    //                        if (tempNode.Attributes["Birth_Date"] != null)
                    //                        {
                    //                            if (tempNode.Attributes["Birth_Date"].Value.ToString() != "")
                    //                                sDOB = Convert.ToDateTime(tempNode.Attributes["Birth_Date"].Value).ToString("dd-MMM-yyyy");
                    //                        }
                    //                        if (tempNode.Attributes["Last_Name"] != null)
                    //                        {
                    //                            if (tempNode.Attributes["Last_Name"].Value.ToString() != "")
                    //                                if (sName == string.Empty)
                    //                                    sName = tempNode.Attributes["Last_Name"].Value.ToString();
                    //                                else
                    //                                    sName += "," + tempNode.Attributes["Last_Name"].Value.ToString();
                    //                        }
                    //                        if (tempNode.Attributes["First_Name"] != null)
                    //                        {
                    //                            if (tempNode.Attributes["First_Name"].Value.ToString() != "")
                    //                                if (sName == string.Empty)
                    //                                    sName = tempNode.Attributes["First_Name"].Value.ToString();
                    //                                else
                    //                                    sName += "," + tempNode.Attributes["First_Name"].Value.ToString();
                    //                        }
                    //                        if (tempNode.Attributes["MI"] != null)
                    //                        {
                    //                            if (tempNode.Attributes["MI"].Value.ToString() != "")
                    //                                if (sName == string.Empty)
                    //                                    sName = tempNode.Attributes["MI"].Value.ToString();
                    //                                else
                    //                                    sName += " " + tempNode.Attributes["MI"].Value.ToString();
                    //                        }
                    //                        if (tempNode.Attributes["Suffix"] != null)
                    //                        {
                    //                            if (tempNode.Attributes["Suffix"].Value.ToString() != "")
                    //                                if (sName == string.Empty)
                    //                                    sName = tempNode.Attributes["Suffix"].Value.ToString();
                    //                                else
                    //                                    sName += " " + tempNode.Attributes["Suffix"].Value.ToString();
                    //                        }
                    //                    }
                    //                    // break;
                    //                }
                    //            }
                    //        }
                    //        fs.Close();
                    //        fs.Dispose();
                    //    }
                    //}
                }

                if (Request["PatientDOB"] != null)
                    txtDOB.Text = Request["PatientDOB"].ToString();
                else
                    txtDOB.Text = sDOB;//ClientSession.PatientPaneList[0].Birth_Date.ToString("dd-MMM-yyyy");
                if (Request["PatientName"] != null)
                    txtPatientName.Text = Request["PatientName"].ToString();
                else
                    txtPatientName.Text = sName;//ClientSession.PatientPaneList[0].Last_Name + "," + ClientSession.PatientPaneList[0].First_Name + ClientSession.PatientPaneList[0].MI + ClientSession.PatientPaneList[0].Suffix;

                rbPanel.Enabled = false;

                if (txtPatientName.Text != string.Empty)
                {
                    //btnGetObjects_Click(sender, e);
                }

                //ClientSession.processCheck = false;
                //SecurityServiceUtility objSecurityServiceUtility = new SecurityServiceUtility();
                //objSecurityServiceUtility.ApplyUserPermissions(this);

                //ViewState["workflowList"] = WorkFlowMngr.GetWorkFlowMapListbyFacilityName(ClientSession.FacilityName); // GetWorkFlowAll();
                ViewState["ProcUser"] = ProcUserMngr.GetProcUserList();
                //ViewState["proclist"] = ApplicationObject.processMasterList;  //ProcessMasterMngr.GetAllProcessList();

                //UserManager login = new UserManager();
                //ViewState["userMyList"] = login.GetPhysicianUserList();
            }

        }

        //protected void btnGetObjects_Click(object sender, EventArgs e)
        //{
        //    FillGrid();
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //}

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object LoadGrid()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string extra_search = HttpContext.Current.Request.Params["extra_search"];
            var searchData = JsonConvert.DeserializeObject<Dictionary<string, string>>(extra_search);
            string sAccountNo = searchData["AccountNo"];
            string sDOB = searchData["DOB"];
            string sPatientName = searchData["PatientName"];
            WFObjectManager objWfObjectMngr = new WFObjectManager();
            ArrayList arryList = objWfObjectMngr.GetPatientByCurrentProcess(Convert.ToUInt32(sAccountNo), sDOB, sPatientName);
            if (arryList.Count > 0)
            {
                
            }
            else
            {
                return  new{data = Compress(JsonConvert.SerializeObject("DisplayErrorMessage-700010"))};
            }


                var resultNew = new
            {
                data = Compress(JsonConvert.SerializeObject(arryList[0]))
            };

            return resultNew;
        }
        //private void FillGrid()
        //{
        //    IList<WFObject> ilstWfObject = new List<WFObject>();
        //    ds = objWfObjectMngr.GetPatientCurrentProcess(Convert.ToUInt32(txtAccountNo.Text), txtDOB.Text, txtPatientName.Text);
        //    //if (ds.Tables.Count > 0)
        //    //{
        //    //    ViewState["AdminModule"] = ds;
        //    //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //    //    {
        //    //        DateTime dtTime = Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[1]);
        //    //        DateTime dtApptDateTime = Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[9]);

        //    //        ds.Tables[0].Rows[i]["Date of Service"] = UtilityManager.ConvertToLocal(dtTime);
        //    //        ds.Tables[0].Rows[i]["Appointment Date"] = UtilityManager.ConvertToLocal(dtApptDateTime);
        //    //        ds.Tables[0].Rows[i].AcceptChanges();
        //    //    }
        //    //    if (ds.Tables[0].Rows.Count > 0)
        //    //    {
        //    //        grdAdminModule.DataSource = ds.Tables[0];
        //    //        grdAdminModule.DataBind();
        //    //        grdAdminModule.MasterTableView.AutoGeneratedColumns[2].Visible = false;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    dtTable.Columns.Add("Encounter ID", typeof(int));
        //    //    dtTable.Columns.Add("Date of Service", typeof(DateTime));
        //    //    dtTable.Columns.Add("Patient Acc#", typeof(int));
        //    //    dtTable.Columns.Add("Patient Name");
        //    //    dtTable.Columns.Add("Patient DOB", typeof(DateTime));
        //    //    //dtTable.Columns.Add("Physician Name");
        //    //    dtTable.Columns.Add("Appointment Provider Name");
        //    //    dtTable.Columns.Add("Encounter Provider Name");
        //    //    dtTable.Columns.Add("Current Process");
        //    //    dtTable.Columns.Add("Current Owner");
        //    //    dtTable.Columns.Add("Appointment Date", typeof(DateTime));
        //    //    dtTable.Columns.Add("Object Type");
        //    //    dtTable.Columns.Add("MA Name");
        //    //    dtTable.Columns.Add("Batch Status");
        //    //    dtTable.Columns.Add("Facility Name");

        //    //    grdAdminModule.DataSource = dtTable;
        //    //    grdAdminModule.DataBind();


        //    //    this.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700010');", true);
        //    //    return;
        //    //}
        //    //if (grdAdminModule.MasterTableView.AutoGeneratedColumns.Count() > 0)
        //    //{

        //    //    foreach (GridColumn col in grdAdminModule.MasterTableView.AutoGeneratedColumns)
        //    //    {
        //    //        //CAP-1082
        //    //        if (col.HeaderText.StartsWith("Date") == true || col.HeaderText.EndsWith("Date") == true)
        //    //        {
        //    //            GridDateTimeColumn dv = (GridDateTimeColumn)col;
        //    //            dv.DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}";


        //    //        }
        //    //        else if (col.HeaderText.EndsWith("DOB") == true)
        //    //        {
        //    //            GridDateTimeColumn dv = (GridDateTimeColumn)col;
        //    //            dv.DataFormatString = "{0:dd-MMM-yyyy}";

        //    //        }
        //    //        col.ItemStyle.Wrap = true;
        //    //    }
        //    //}

        //    //grdAdminModule.DataBind();
        //}
        protected void grdAdminModule_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "RowClick")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
        }
        protected void grdAdminModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string[] str = new string[] { };
            //string[] sprocess = new string[] { };
            //string[] str2 = new string[] { };
            //txtReasonForChange.Text = string.Empty;
            //cboPreviousProcess.DataSource = new string[] { };
            //cboPreviousProcess.DataBind();
            //cboUpdateOwner.DataSource = new string[] { };
            //cboUpdateOwner.DataBind();
            //rbPanel.Enabled = false;
            //pnlUpdateOwner.Enabled = false;
            //pnlUpdateProcess.Enabled = false;
            //rbUpdateOwner.Enabled = true;
            //rbUpdateProcess.Enabled = true;
            //rbUpdateOwner.Checked = false;
            //rbUpdateProcess.Checked = false;
            //GridDataItem grdSelectedItem = (GridDataItem)grdAdminModule.SelectedItems[0];

            //IList<ProcessMaster> proclist = ApplicationObject.processMasterList; // (IList<ProcessMaster>)ViewState["proclist"];
            //IList<WorkFlow> worflowList = (IList<WorkFlow>)ViewState["workflowList"];
            ////IList<User> userMyList = (IList<User>)ViewState["userMyList"];
            //IList<ProcUser> ProcUser = (IList<ProcUser>)ViewState["ProcUser"];
            ////worflowList = WorkFlowMngr.GetWorkFlowMapList(grdSelectedItem["Object Type"].Text);
            ////ProcUser = ProcUserMngr.GetProcUserList();
            ////proclist = ApplicationObject.processMasterList;  //ProcessMasterMngr.GetAllProcessList();

            //if (worflowList.Count > 0)
            //{
            //    IList<string> wf1 = ((from w in worflowList where w.Obj_Type == grdSelectedItem["Object Type"].Text select w.To_Process).Distinct()).ToList<string>();
            //    str = wf1.ToArray();
            //}


            //if (proclist.Count > 0)
            //{
            //    //ViewState["proclist"] = proclist;
            //    IList<string> BackPushAllowedProcess = (from p in proclist where p.Is_Back_Push_Allowed == "Y" && str.Contains(p.Process_Name) select p.Process_Name).ToList<string>();
            //    sprocess = BackPushAllowedProcess.ToArray();
            //}

            //if (sprocess.Count() > 0)
            //{
            //    if (grdSelectedItem["Current Process"].Text != string.Empty)
            //    {
            //        if (sprocess.Contains(grdSelectedItem["Current Process"].Text))
            //        {


            //            //var wfProcess = (from wfToprocess in worflowList
            //            //                 where wfToprocess.To_Process == grdSelectedItem["Current Process"].Text && wfToprocess.Fac_Name == ClientSession.FacilityName && wfToprocess.Obj_Type == grdSelectedItem["Object Type"].Text
            //            //                 select new {wfToprocess.From_Process, wfToprocess.Close_Type }).Distinct();
            //            var wfProcess = (from wfToprocess in worflowList
            //                             where wfToprocess.To_Process == grdSelectedItem["Current Process"].Text && wfToprocess.Fac_Name == ClientSession.FacilityName && wfToprocess.Obj_Type == grdSelectedItem["Object Type"].Text
            //                             select new { wfToprocess.From_Process, wfToprocess.To_Process }).Distinct();


            //            foreach (var Toprocess in wfProcess)
            //            {
            //                if (Toprocess.From_Process != "CHECK_OUT_WAIT" && Toprocess.From_Process != "BILLING_WAIT")
            //                {
            //                    if (grdSelectedItem["Current Process"].Text != Toprocess.From_Process)//Added by Naveena for bug_id 26501,26498
            //                    {
            //                        if (grdSelectedItem["Current Process"].Text == "CANCELLED")
            //                        {
            //                            if (Toprocess.From_Process.ToString() != "MA_PROCESS")
            //                            {
            //                                cboPreviousProcess.Items.Add(new RadComboBoxItem(Toprocess.From_Process));
            //                            }
            //                        }
            //                        else
            //                        {
            //                            cboPreviousProcess.Items.Add(new RadComboBoxItem(Toprocess.From_Process));
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700007');", true);
            //            //return;
            //        }
            //    }

            //}

            ////Update Owner
            //if (worflowList.Count > 0)
            //{
            //    IList<string> wf2 = ((from w in worflowList where w.Obj_Type == grdSelectedItem["Object Type"].Text select w.To_Process).Distinct()).ToList<string>();
            //    str2 = wf2.ToArray();

            //}


            //var process = from p in proclist where p.Process_Type == "ASSIGNED" && str2.Contains(p.Process_Name) select p;

            //if (process.Count() > 0)
            //{
            //    UserManager lo = new UserManager();
            //    IList<string> userList = new List<string>();
            //    if (grdSelectedItem["Current Process"].Text != string.Empty)
            //    {
            //        var user = from u in ProcUser where u.Process_Name == grdSelectedItem["Current Process"].Text && u.Status == "A" orderby u.User_Name select u.User_Name;
            //        userList = user.ToList<string>();
            //    }

            //    cboUpdateOwner.DataSource = userList;
            //    cboUpdateOwner.DataBind();
            //}
            //else
            //{

            //}
            //if (grdAdminModule.SelectedItems.Count > 0)
            //{
            //    rbPanel.Enabled = true;
            //    rbUpdateOwner.Checked = false;
            //    rbUpdateOwner.Checked = false;
            //    //pnlUpdateProcess.Enabled = false;
            //    //pnlUpdateOwner.Enabled = false;
            //}
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }

        //protected void btnUpdateProcess_Click(object sender, EventArgs e)
        //{


        //}
        private static void PhyAsstProcess(ulong ulEncounterID, string sIs_Physician_Asst_Process)
        {
            IList<Encounter> encList = new List<Encounter>();
            Encounter objEncounter = null;
            EncounterManager EncProxyMngr = new EncounterManager();
            encList = EncProxyMngr.GetEncounterByEncounterID(ulEncounterID);
            if (encList != null && encList.Count > 0)
            {
                objEncounter = new Encounter();
                objEncounter = encList[0];
                objEncounter.Is_Physician_Asst_Process = sIs_Physician_Asst_Process;
                EncProxyMngr.UpdateEncounter(objEncounter, string.Empty, new object[] { "false" });
            }

        }
        //protected void btnUpdateOwner_Click(object sender, EventArgs e)
        //{
        //    
        //    string sJoinedSelectedRows = string.Join(",", hdnSelectedRow.Value);
        //    using (JsonDocument doc = JsonDocument.Parse(sJoinedSelectedRows))
        //    {
        //        JsonElement root = doc.RootElement;

        //        //if (grdAdminModule.SelectedItems.Count == 0)
        //        //{
        //        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700001'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //        //    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700001');", true);
        //        //    return;
        //        //}

        //        if (cboUpdateOwner.Value == string.Empty)
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700011'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700005');", true);
        //            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700011');", true);
        //            cboUpdateOwner.Focus();
        //            return;
        //        }
        //        //For Bug ID :67379
        //        string[] sprocess = new string[] { };
        //        IList<ProcessMaster> proclist = ApplicationObject.processMasterList;
        //        if (proclist != null && proclist.Count > 0)
        //        {
        //            IList<string> unassignedProcess = (from p in proclist where p.Process_Type == "UNASSIGNED" select p.Process_Name).ToList<string>();
        //            sprocess = unassignedProcess.ToArray();
        //        }
        //        else if (ApplicationObject.processMasterList != null)
        //        {
        //            IList<string> unassignedProcess = (from p in ApplicationObject.processMasterList where p.Process_Type == "UNASSIGNED" select p.Process_Name).ToList<string>();
        //            sprocess = unassignedProcess.ToArray();
        //        }
        //        else
        //        {

        //            proclist = ProcessMasterMngr.GetAllProcessList();
        //            IList<string> unassignedProcess = (from p in proclist where p.Process_Type == "UNASSIGNED" select p.Process_Name).ToList<string>();
        //            sprocess = unassignedProcess.ToArray();
        //        }

        //        //GridDataItem grdSelectedItem = (GridDataItem)grdAdminModule.SelectedItems[0];
        //        if (sprocess.Count() > 0)
        //        {
        //            if (root.GetProperty("Current Process").GetString() != string.Empty)
        //            {
        //                if (sprocess.Contains(root.GetProperty("Current Process").GetString()) && root.GetProperty("Current Owner").GetString() == "UNKNOWN")
        //                {
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700015'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //                    cboUpdateOwner.Focus();
        //                    return;
        //                }
        //            }
        //        }
        //        //End For Bug ID :67379

        //        //GridDataItem selectedItem = (GridDataItem)grdAdminModule.SelectedItems[0];
        //        if (root.GetProperty("Current Process").GetString() != string.Empty)
        //        {
        //            if (root.GetProperty("Current Process").GetString() == "MA_PROCESS")
        //            {
        //                EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
        //                IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()));

        //                if (ilstEncounterBlob == null && ilstEncounterBlob.Count == 0)
        //                {
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700013'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //                    return;
        //                }

        //                //    string FileName = "Encounter" + "_" + grdAdminModule.SelectedItems[0].Cells[2].Text + ".xml";
        //                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
        //                //if (File.Exists(strXmlFilePath) == false)
        //                //{
        //                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700013'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //                //    return;
        //                //}
        //            }
        //        }
        //        //IList<User> userMyList = new List<User>();
        //        //if (ViewState["userMyList"] != null)
        //        //{
        //        //    userMyList = (IList<User>)ViewState["userMyList"];
        //        //}

        //        // btnUpdateOwner.Enabled = false;

        //        //var logg = from l in userMyList where l.Physician_Library_ID == Convert.ToUInt64(cboUpdateOwner.Items[cboUpdateOwner.SelectedIndex].Value) select l;

        //        string sRole = string.Empty;
        //        int Physician_Library_ID = 0;

        //        XmlDocument xmldocUser = new XmlDocument();
        //        if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "User" + ".xml"))
        //            xmldocUser.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "User" + ".xml");
        //        XmlNodeList xmlUserList = xmldocUser.GetElementsByTagName("User");
        //        string sFacilityName = string.Empty;
        //        if (xmlUserList != null)
        //        {
        //            foreach (XmlNode item in xmlUserList)
        //                if (cboUpdateOwner.Value.ToString().Trim().ToUpper() == item.Attributes.GetNamedItem("User_Name").Value.ToUpper().Trim())
        //                {
        //                    if (cboUpdateOwner.Value.ToString() == item.Attributes[0].Value.ToUpper())
        //                    {
        //                        if (item.Attributes.GetNamedItem("Physician_Library_ID").Value != null)
        //                            Physician_Library_ID = Convert.ToInt32(item.Attributes.GetNamedItem("Physician_Library_ID").Value);
        //                        if (item.Attributes.GetNamedItem("Role").Value != null)
        //                            sRole = item.Attributes.GetNamedItem("Role").Value;
        //                        break;
        //                    }
        //                }
        //        }

        //        if (root.GetProperty("Current Process").GetString() != string.Empty)
        //        {
        //            if (root.GetProperty("Current Process").GetString() == "PROVIDER_PROCESS")
        //            {
        //                if (sRole.ToUpper() == "PHYSICIAN ASSISTANT")
        //                {
        //                    PhyAsstProcess(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), "Y");
        //                }
        //                else if (sRole.ToUpper() == "PHYSICIAN")
        //                {
        //                    PhyAsstProcess(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), "N");
        //                }
        //            }
        //        }


        //        CorrectedDBEntries CorrDBEntry = new CorrectedDBEntries();
        //        CorrDBEntry.Corrected_By = ClientSession.UserName;
        //        CorrDBEntry.Corrected_Date = UtilityManager.ConvertToLocal(DateTime.Now);
        //        WFObject WFRecord = null;
        //        if (root.GetProperty("Encounter ID").GetString() != string.Empty && root.GetProperty("Object Type").GetString() != string.Empty)
        //        {
        //            WFRecord = WfObjectMngr.GetByObjectSystemId(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), root.GetProperty("Object Type").GetString());
        //        }

        //        CorrDBEntry.Old_Owner = WFRecord.Current_Owner;
        //        if (cboUpdateOwner.Items[cboUpdateOwner.SelectedIndex].Value != string.Empty)
        //        {
        //            CorrDBEntry.New_Owner = cboUpdateOwner.Items[cboUpdateOwner.SelectedIndex].Value;
        //        }
        //        if (root.GetProperty("Current Process").GetString() != string.Empty)
        //        {
        //            CorrDBEntry.Old_Process = root.GetProperty("Current Process").GetString();
        //        }
        //        CorrDBEntry.Reason_for_Changes = txtReasonForChange.Value;
        //        CorrDBEntry.Type_of_Change = "UPDATE_OWNER";
        //        CorrDBEntry.WF_Object_ID = Convert.ToInt32(WFRecord.Id);
        //        CorrDBEntryProxy.AppendToCorrectedDBEntries(CorrDBEntry, string.Empty);

        //        if (root.GetProperty("Encounter ID").GetString() != string.Empty && root.GetProperty("Object Type").GetString() != string.Empty && cboUpdateOwner.Value != string.Empty)
        //        {
        //            WfObjectMngr.UpdateOwner(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), root.GetProperty("Object Type").GetString(), cboUpdateOwner.Value, string.Empty);
        //        }

        //        if (root.GetProperty("Current Process").GetString() != string.Empty)
        //        {
        //            if (root.GetProperty("Current Process").GetString() == "PROVIDER_PROCESS" || root.GetProperty("Current Process").GetString() == "TECHNICIAN_PROCESS")
        //            {
        //                EncounterManager encProxy = new EncounterManager();
        //                Encounter enc = new Encounter();
        //                IList<Encounter> encList = new List<Encounter>();
        //                if (root.GetProperty("Encounter ID").GetString() != string.Empty)
        //                {
        //                    encList = encProxy.GetEncounterByEncounterID(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()));
        //                }
        //                if (encList.Count == 0)
        //                {
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('700014'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //                    return;
        //                }
        //                enc = encList[0];

        //                if (Physician_Library_ID != 0) // (cboUpdateOwner.Items[cboUpdateOwner.SelectedIndex].Value != string.Empty)
        //                {
        //                    enc.Encounter_Provider_ID = Physician_Library_ID; // Convert.ToInt32(cboUpdateOwner.Items[cboUpdateOwner.SelectedIndex].Value);
        //                }

        //                //string FileName = "Encounter" + "_" + enc.Id + ".xml";
        //                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

        //                //if (File.Exists(strXmlFilePath) == false)
        //                //{
        //                //    string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
        //                //    string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");

        //                //    XmlDocument itemDoc = new XmlDocument();
        //                //    XmlTextReader XmlText = new XmlTextReader(sXmlPath);
        //                //    itemDoc.Load(XmlText);
        //                //    XmlText.Close();

        //                //    XmlNodeList xmlAgenode = itemDoc.GetElementsByTagName("Age");
        //                //    if (xmlAgenode != null && xmlAgenode.Count > 0)
        //                //        xmlAgenode[0].ParentNode.RemoveChild(xmlAgenode[0]);

        //                //  //  itemDoc.Save(strXmlFilePath);
        //                //    int trycount = 0;
        //                //trytosaveagain:
        //                //    try
        //                //    {
        //                //        itemDoc.Save(strXmlFilePath);
        //                //    }
        //                //    catch (Exception xmlexcep)
        //                //    {
        //                //        trycount++;
        //                //        if (trycount <= 3)
        //                //        {
        //                //            int TimeMilliseconds = 0;
        //                //            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
        //                //                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

        //                //            Thread.Sleep(TimeMilliseconds);
        //                //            string sMsg = string.Empty;
        //                //            string sExStackTrace = string.Empty;

        //                //            string version = "";
        //                //            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
        //                //                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

        //                //            string[] server = version.Split('|');
        //                //            string serverno = "";
        //                //            if (server.Length > 1)
        //                //                serverno = server[1].Trim();

        //                //            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
        //                //                sMsg = xmlexcep.InnerException.Message;
        //                //            else
        //                //                sMsg = xmlexcep.Message;

        //                //            if (xmlexcep != null && xmlexcep.StackTrace != null)
        //                //                sExStackTrace = xmlexcep.StackTrace;

        //                //            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
        //                //            string ConnectionData;
        //                //            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        //                //            using (MySqlConnection con = new MySqlConnection(ConnectionData))
        //                //            {
        //                //                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
        //                //                {
        //                //                    cmd.Connection = con;
        //                //                    try
        //                //                    {
        //                //                        con.Open();
        //                //                        cmd.ExecuteNonQuery();
        //                //                        con.Close();
        //                //                    }
        //                //                    catch
        //                //                    {
        //                //                    }
        //                //                }
        //                //            }
        //                //            goto trytosaveagain;
        //                //        }
        //                //    }
        //                //}

        //                //Jira CAP-2887
        //                //encProxy.UpdateEncounter(enc, string.Empty, new object[] { "false" });
        //                XmlDocument itemDoc = new XmlDocument();
        //                string sXMLContent = String.Empty;
        //                EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
        //                Encounter_Blob objEncounterblob = null;
        //                bool bIsUpdateinBlob = true;
        //                IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(enc.Id);
        //                if (ilstEncounterBlob.Count == 0)
        //                {
        //                    bIsUpdateinBlob = false;
        //                }
        //                encProxy.UpdateEncounter(enc, string.Empty, new object[] { "false" }, bIsUpdateinBlob);

        //                //Jira CAP-2887
        //                //XmlDocument itemDoc = new XmlDocument();
        //                //string sXMLContent = String.Empty;
        //                //EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
        //                //Encounter_Blob objEncounterblob = null;
        //                //IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(enc.Id);
        //                ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(enc.Id);
        //                if (ilstEncounterBlob.Count > 0)
        //                {
        //                    objEncounterblob = ilstEncounterBlob[0];
        //                    sXMLContent = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
        //                    if (sXMLContent.Substring(0, 1) != "<")
        //                        sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
        //                    itemDoc.LoadXml(sXMLContent);


        //                    //if (File.Exists(strXmlFilePath) == true)
        //                    //{
        //                    //    XmlDocument itemDoc = new XmlDocument();
        //                    //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
        //                    //    itemDoc.Load(XmlText);
        //                    //    XmlText.Close();
        //                    //IEnumerable<XElement> ilstPhysician = null;
        //                    XmlNodeList xmlMember_ID = itemDoc.GetElementsByTagName("Encounter_Provider_Name");

        //                    //string sPhysicianFacilityXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\
        //                    //PhysicianFacilityMapping.xml";
        //                    //XDocument xmlPhysician = XDocument.Load(sPhysicianFacilityXmlPath);
        //                    //ilstPhysician = xmlPhysician.Element("ROOT").Element("PhyList").Elements("Facility").Elements("Physician").Where(aa => aa.Attribute("ID").Value.ToString() == enc.Encounter_Provider_ID.ToString());
        //                    //CAP-2781
        //                    PhysicianFacilityMappingList physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");
        //                    if (physicianFacilityMappingList != null)
        //                    {
        //                        var lstPhysician = physicianFacilityMappingList.PhysicianFacility.SelectMany(x => x.Physician).FirstOrDefault(y => y.ID == enc.Encounter_Provider_ID.ToString());
        //                        if (lstPhysician != null)
        //                        {
        //                            xmlMember_ID[0].InnerText = lstPhysician.prefix + " " + lstPhysician.firstname + " " + lstPhysician.middlename + " " + lstPhysician.lastname;
        //                        }
        //                    }

        //                    string sPhysicianid = enc.Encounter_Provider_ID.ToString();
        //                    //XmlNodeList xmlPhysicianAddress = itemDoc.GetElementsByTagName("Physician_Address");
        //                    //if (xmlPhysicianAddress != null)
        //                    //{
        //                    //    string sPhysicianXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\PhysicianAddressDetails.xml";
        //                    //    XmlDocument itemPhysiciandoc = new XmlDocument();
        //                    //    XmlTextReader XmlPhysicianText = new XmlTextReader(sPhysicianXmlPath);
        //                    //    itemPhysiciandoc.Load(XmlPhysicianText);

        //                    //    XmlNodeList xmlphy = itemPhysiciandoc.GetElementsByTagName("p" + sPhysicianid);
        //                    //    if (xmlphy.Count > 0)
        //                    //    {
        //                    //        xmlPhysicianAddress[0].Attributes[0].Value = xmlphy[0].Attributes[0].Value;
        //                    //        xmlPhysicianAddress[0].Attributes[1].Value = xmlphy[0].Attributes[1].Value;
        //                    //        xmlPhysicianAddress[0].Attributes[2].Value = xmlphy[0].Attributes[2].Value;
        //                    //        xmlPhysicianAddress[0].Attributes[3].Value = xmlphy[0].Attributes[3].Value;
        //                    //        xmlPhysicianAddress[0].Attributes[4].Value = xmlphy[0].Attributes[4].Value;
        //                    //        xmlPhysicianAddress[0].Attributes[5].Value = xmlphy[0].Attributes[5].Value;
        //                    //        xmlPhysicianAddress[0].Attributes[6].Value = xmlphy[0].Attributes[6].Value;
        //                    //        xmlPhysicianAddress[0].Attributes[7].Value = xmlphy[0].Attributes[7].Value;
        //                    //    }
        //                    //}
        //                    //CAP-2780
        //                    XmlNodeList xmlPhysicianAddress = itemDoc.GetElementsByTagName("Physician_Address");
        //                    if (xmlPhysicianAddress != null)
        //                    {
        //                        PhysicianAddressDetailsList physicianAddressDetailsList = ConfigureBase<PhysicianAddressDetailsList>.ReadJson("PhysicianAddressDetails.json");
        //                        if (physicianAddressDetailsList != null)
        //                        {
        //                            var matchingAddress = physicianAddressDetailsList.PhysicianAddress
        //                            .FirstOrDefault(address => address.Physician_Library_ID == sPhysicianid.ToString());

        //                            if (matchingAddress != null)
        //                            {
        //                                xmlPhysicianAddress[0].Attributes[0].Value = matchingAddress?.Physician_Address1 ?? "";
        //                                xmlPhysicianAddress[0].Attributes[1].Value = matchingAddress?.Physician_Address2 ?? "";
        //                                xmlPhysicianAddress[0].Attributes[2].Value = matchingAddress?.Physician_City ?? "";
        //                                xmlPhysicianAddress[0].Attributes[3].Value = matchingAddress?.Physician_State ?? "";
        //                                xmlPhysicianAddress[0].Attributes[4].Value = matchingAddress?.Physician_Zip ?? "";
        //                                xmlPhysicianAddress[0].Attributes[5].Value = matchingAddress?.Physician_Telephone ?? "";
        //                                xmlPhysicianAddress[0].Attributes[6].Value = matchingAddress?.Physician_Fax ?? "";
        //                                xmlPhysicianAddress[0].Attributes[7].Value = matchingAddress?.Specialties ?? "";
        //                            }
        //                        }
        //                    }
        //                    //itemDoc.Save(strXmlFilePath);
        //                    IList<Encounter_Blob> ilstUpdateBlob = new List<Encounter_Blob>();
        //                    byte[] bytes = null;
        //                    try
        //                    {
        //                        bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                    }
        //                    objEncounterblob.Encounter_XML = bytes;
        //                    ilstUpdateBlob.Add(objEncounterblob);
        //                    EncounterBlobManager EncBlobManager = new EncounterBlobManager();
        //                    EncBlobManager.SaveEncounterBlobWithTransaction(ilstUpdateBlob, string.Empty);
        //                }


        //            }
        //            if (root.GetProperty("Current Process").GetString() == "MA_PROCESS")
        //            {
        //                EncounterManager encProxy = new EncounterManager();
        //                Encounter enc = new Encounter();
        //                IList<Encounter> encList = new List<Encounter>();
        //                if (root.GetProperty("Encounter ID").GetString() != string.Empty)
        //                {
        //                    encList = encProxy.GetEncounterByEncounterID(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()));
        //                }


        //                if (encList.Count == 0)
        //                {
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('700014'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //                    return;
        //                }
        //                else
        //                {
        //                    enc = encList[0];
        //                    if (cboUpdateOwner.Items[cboUpdateOwner.SelectedIndex].Value != string.Empty)
        //                    {
        //                        enc.Assigned_Med_Asst_User_Name = cboUpdateOwner.Items[cboUpdateOwner.SelectedIndex].Value;
        //                    }
        //                    encProxy.UpdateEncounter(enc, string.Empty, new object[] { "false" });
        //                }
        //            }
        //            if (root.GetProperty("Current Process").GetString() == "SCRIBE_PROCESS" || root.GetProperty("Current Process").GetString() == "AKIDO_SCRIBE_PROCESS" || root.GetProperty("Current Process").GetString() == "SCRIBE_CORRECTION" || root.GetProperty("Current Process").GetString() == "SCRIBE_REVIEW_CORRECTION")
        //            {
        //                EncounterManager encProxy = new EncounterManager();
        //                Encounter enc = new Encounter();
        //                IList<Encounter> encList = new List<Encounter>();
        //                if (root.GetProperty("Encounter ID").GetString() != string.Empty)
        //                {
        //                    encList = encProxy.GetEncounterByEncounterID(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()));
        //                }


        //                if (encList.Count == 0)
        //                {
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('700014'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //                    return;
        //                }
        //                else
        //                {
        //                    enc = encList[0];
        //                    if (cboUpdateOwner.Items[cboUpdateOwner.SelectedIndex].Value != string.Empty)
        //                    {
        //                        enc.Assigned_Scribe_User_Name = cboUpdateOwner.Items[cboUpdateOwner.SelectedIndex].Value;
        //                    }
        //                    encProxy.UpdateEncounter(enc, string.Empty, new object[] { "false" });
        //                }
        //            }
        //        }



        //        //btnGetObjects_Click(sender, e);
        //        //FillGrid();
        //        //clearall();
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "FillGrid(); ClearAll(); DisplayErrorMessage('700004'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //        //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //    }
        //}
        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object btnUpdateOwner_Click(string[] sSelectedRow, string[] sCboUpdateOwner, string stxtReasonForChange)
        {
            WFObjectManager WfObjectMngr = new WFObjectManager();
            var sUpdateOwner = (FullName : sCboUpdateOwner[0], User_Name : sCboUpdateOwner[1]) ;
            string sJoinedSelectedRows = "{" + string.Join(",", sSelectedRow) + "}";
            using (JsonDocument doc = JsonDocument.Parse(sJoinedSelectedRows))
            {
                JsonElement root = doc.RootElement;

                //if (grdAdminModule.SelectedItems.Count == 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700001'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700001');", true);
                //    return;
                //}

                //if (cboUpdateOwner.Value == string.Empty)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700011'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700005');", true);
                //    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700011');", true);
                //    cboUpdateOwner.Focus();
                //    return;
                //}
                //For Bug ID :67379
                string[] sprocess = new string[] { };
                IList<ProcessMaster> proclist = ApplicationObject.processMasterList;
                if (proclist != null && proclist.Count > 0)
                {
                    IList<string> unassignedProcess = (from p in proclist where p.Process_Type == "UNASSIGNED" select p.Process_Name).ToList<string>();
                    sprocess = unassignedProcess.ToArray();
                }
                else if (ApplicationObject.processMasterList != null)
                {
                    IList<string> unassignedProcess = (from p in ApplicationObject.processMasterList where p.Process_Type == "UNASSIGNED" select p.Process_Name).ToList<string>();
                    sprocess = unassignedProcess.ToArray();
                }
                else
                {
                    ProcessMasterManager ProcessMasterMngr = new ProcessMasterManager();
                    proclist = ProcessMasterMngr.GetAllProcessList();
                    IList<string> unassignedProcess = (from p in proclist where p.Process_Type == "UNASSIGNED" select p.Process_Name).ToList<string>();
                    sprocess = unassignedProcess.ToArray();
                }

                //GridDataItem grdSelectedItem = (GridDataItem)grdAdminModule.SelectedItems[0];
                if (sprocess.Count() > 0)
                {
                    if (root.GetProperty("Current Process").GetString() != string.Empty)
                    {
                        if (sprocess.Contains(root.GetProperty("Current Process").GetString()) && root.GetProperty("Current Owner").GetString() == "UNKNOWN")
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700015'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            //cboUpdateOwner.Focus();
                            return "DisplayErrorMessage-700015-return";
                        }
                    }
                }
                //End For Bug ID :67379

                //GridDataItem selectedItem = (GridDataItem)grdAdminModule.SelectedItems[0];
                if (root.GetProperty("Current Process").GetString() != string.Empty)
                {
                    if (root.GetProperty("Current Process").GetString() == "MA_PROCESS")
                    {
                        EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
                        IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()));

                        //Jira CAP-3587
                        //if (ilstEncounterBlob == null && ilstEncounterBlob.Count == 0)
                        if (ilstEncounterBlob == null || ilstEncounterBlob.Count == 0)
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700013'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            //Jira CAP-3587
                            //return "DisplayErrorMessage-700013-return";
                            return "DisplayErrorMessage-700016-return";
                        }

                        //    string FileName = "Encounter" + "_" + grdAdminModule.SelectedItems[0].Cells[2].Text + ".xml";
                        //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                        //if (File.Exists(strXmlFilePath) == false)
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700013'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        //    return;
                        //}
                    }
                }
                //IList<User> userMyList = new List<User>();
                //if (ViewState["userMyList"] != null)
                //{
                //    userMyList = (IList<User>)ViewState["userMyList"];
                //}

                // btnUpdateOwner.Enabled = false;

                //var logg = from l in userMyList where l.Physician_Library_ID == Convert.ToUInt64(cboUpdateOwner.Items[cboUpdateOwner.SelectedIndex].Value) select l;

                string sRole = string.Empty;
                int Physician_Library_ID = 0;

                XmlDocument xmldocUser = new XmlDocument();
                if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "User" + ".xml"))
                    xmldocUser.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "User" + ".xml");
                XmlNodeList xmlUserList = xmldocUser.GetElementsByTagName("User");
                string sFacilityName = string.Empty;
                if (xmlUserList != null)
                {
                    foreach (XmlNode item in xmlUserList)
                        if (sUpdateOwner.User_Name.ToString().Trim().ToUpper() == item.Attributes.GetNamedItem("User_Name").Value.ToUpper().Trim())
                        {
                            if (sUpdateOwner.User_Name.ToString() == item.Attributes[0].Value.ToUpper())
                            {
                                if (item.Attributes.GetNamedItem("Physician_Library_ID").Value != null)
                                    Physician_Library_ID = Convert.ToInt32(item.Attributes.GetNamedItem("Physician_Library_ID").Value);
                                if (item.Attributes.GetNamedItem("Role").Value != null)
                                    sRole = item.Attributes.GetNamedItem("Role").Value;
                                break;
                            }
                        }
                }

                if (root.GetProperty("Current Process").GetString() != string.Empty)
                {
                    if (root.GetProperty("Current Process").GetString() == "PROVIDER_PROCESS")
                    {
                        if (sRole.ToUpper() == "PHYSICIAN ASSISTANT")
                        {
                            PhyAsstProcess(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), "Y");
                        }
                        else if (sRole.ToUpper() == "PHYSICIAN")
                        {
                            PhyAsstProcess(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), "N");
                        }
                    }
                }


                CorrectedDBEntries CorrDBEntry = new CorrectedDBEntries();
                CorrDBEntry.Corrected_By = ClientSession.UserName;
                CorrDBEntry.Corrected_Date = UtilityManager.ConvertToLocal(DateTime.Now);
                WFObject WFRecord = null;
                if (root.GetProperty("Encounter ID").GetString() != string.Empty && root.GetProperty("Object Type").GetString() != string.Empty)
                {
                    WFRecord = WfObjectMngr.GetByObjectSystemId(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), root.GetProperty("Object Type").GetString());
                }

                CorrDBEntry.Old_Owner = WFRecord.Current_Owner;
                if (sUpdateOwner.User_Name != string.Empty)
                {
                    CorrDBEntry.New_Owner = sUpdateOwner.User_Name;
                }
                if (root.GetProperty("Current Process").GetString() != string.Empty)
                {
                    CorrDBEntry.Old_Process = root.GetProperty("Current Process").GetString();
                }
                //CorrDBEntry.Reason_for_Changes = txtReasonForChange.Text;
                CorrDBEntry.Reason_for_Changes = stxtReasonForChange;
                CorrDBEntry.Type_of_Change = "UPDATE_OWNER";
                CorrDBEntry.WF_Object_ID = Convert.ToInt32(WFRecord.Id);
                CorrectedDBEntriesManager CorrDBEntryProxy = new CorrectedDBEntriesManager();
                CorrDBEntryProxy.AppendToCorrectedDBEntries(CorrDBEntry, string.Empty);

                if (root.GetProperty("Encounter ID").GetString() != string.Empty && root.GetProperty("Object Type").GetString() != string.Empty && sUpdateOwner.User_Name != string.Empty)
                {
                    WfObjectMngr.UpdateOwner(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), root.GetProperty("Object Type").GetString(), sUpdateOwner.User_Name, string.Empty);
                }

                if (root.GetProperty("Current Process").GetString() != string.Empty)
                {
                    if (root.GetProperty("Current Process").GetString() == "PROVIDER_PROCESS" || root.GetProperty("Current Process").GetString() == "TECHNICIAN_PROCESS")
                    {
                        EncounterManager encProxy = new EncounterManager();
                        Encounter enc = new Encounter();
                        IList<Encounter> encList = new List<Encounter>();
                        if (root.GetProperty("Encounter ID").GetString() != string.Empty)
                        {
                            encList = encProxy.GetEncounterByEncounterID(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()));
                        }
                        if (encList.Count == 0)
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('700014'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            return "DisplayErrorMessage-700014-return";
                        }
                        enc = encList[0];

                        if (Physician_Library_ID != 0) // (cboUpdateOwner.Items[cboUpdateOwner.SelectedIndex].Value != string.Empty)
                        {
                            enc.Encounter_Provider_ID = Physician_Library_ID; // Convert.ToInt32(cboUpdateOwner.Items[cboUpdateOwner.SelectedIndex].Value);
                        }

                        //string FileName = "Encounter" + "_" + enc.Id + ".xml";
                        //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

                        //if (File.Exists(strXmlFilePath) == false)
                        //{
                        //    string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                        //    string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");

                        //    XmlDocument itemDoc = new XmlDocument();
                        //    XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                        //    itemDoc.Load(XmlText);
                        //    XmlText.Close();

                        //    XmlNodeList xmlAgenode = itemDoc.GetElementsByTagName("Age");
                        //    if (xmlAgenode != null && xmlAgenode.Count > 0)
                        //        xmlAgenode[0].ParentNode.RemoveChild(xmlAgenode[0]);

                        //  //  itemDoc.Save(strXmlFilePath);
                        //    int trycount = 0;
                        //trytosaveagain:
                        //    try
                        //    {
                        //        itemDoc.Save(strXmlFilePath);
                        //    }
                        //    catch (Exception xmlexcep)
                        //    {
                        //        trycount++;
                        //        if (trycount <= 3)
                        //        {
                        //            int TimeMilliseconds = 0;
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                        //                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                        //            Thread.Sleep(TimeMilliseconds);
                        //            string sMsg = string.Empty;
                        //            string sExStackTrace = string.Empty;

                        //            string version = "";
                        //            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        //                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                        //            string[] server = version.Split('|');
                        //            string serverno = "";
                        //            if (server.Length > 1)
                        //                serverno = server[1].Trim();

                        //            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                        //                sMsg = xmlexcep.InnerException.Message;
                        //            else
                        //                sMsg = xmlexcep.Message;

                        //            if (xmlexcep != null && xmlexcep.StackTrace != null)
                        //                sExStackTrace = xmlexcep.StackTrace;

                        //            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        //            string ConnectionData;
                        //            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        //            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                        //            {
                        //                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                        //                {
                        //                    cmd.Connection = con;
                        //                    try
                        //                    {
                        //                        con.Open();
                        //                        cmd.ExecuteNonQuery();
                        //                        con.Close();
                        //                    }
                        //                    catch
                        //                    {
                        //                    }
                        //                }
                        //            }
                        //            goto trytosaveagain;
                        //        }
                        //    }
                        //}

                        //Jira CAP-2887
                        //encProxy.UpdateEncounter(enc, string.Empty, new object[] { "false" });
                        XmlDocument itemDoc = new XmlDocument();
                        string sXMLContent = String.Empty;
                        EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
                        Encounter_Blob objEncounterblob = null;
                        bool bIsUpdateinBlob = true;
                        IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(enc.Id);
                        if (ilstEncounterBlob.Count == 0)
                        {
                            bIsUpdateinBlob = false;
                        }
                        encProxy.UpdateEncounter(enc, string.Empty, new object[] { "false" }, bIsUpdateinBlob);

                        //Jira CAP-2887
                        //XmlDocument itemDoc = new XmlDocument();
                        //string sXMLContent = String.Empty;
                        //EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
                        //Encounter_Blob objEncounterblob = null;
                        //IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(enc.Id);
                        ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(enc.Id);
                        if (ilstEncounterBlob.Count > 0)
                        {
                            objEncounterblob = ilstEncounterBlob[0];
                            sXMLContent = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                            sXMLContent = UtilityManager.ReplaceHexadecimal(sXMLContent);
                            if (sXMLContent.Substring(0, 1) != "<")
                                sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                            itemDoc.LoadXml(sXMLContent);


                            //if (File.Exists(strXmlFilePath) == true)
                            //{
                            //    XmlDocument itemDoc = new XmlDocument();
                            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                            //    itemDoc.Load(XmlText);
                            //    XmlText.Close();
                            //IEnumerable<XElement> ilstPhysician = null;
                            XmlNodeList xmlMember_ID = itemDoc.GetElementsByTagName("Encounter_Provider_Name");

                            //string sPhysicianFacilityXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\
                            //PhysicianFacilityMapping.xml";
                            //XDocument xmlPhysician = XDocument.Load(sPhysicianFacilityXmlPath);
                            //ilstPhysician = xmlPhysician.Element("ROOT").Element("PhyList").Elements("Facility").Elements("Physician").Where(aa => aa.Attribute("ID").Value.ToString() == enc.Encounter_Provider_ID.ToString());
                            //CAP-2781
                            PhysicianFacilityMappingList physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");
                            if (physicianFacilityMappingList != null)
                            {
                                var lstPhysician = physicianFacilityMappingList.PhysicianFacility.SelectMany(x => x.Physician).FirstOrDefault(y => y.ID == enc.Encounter_Provider_ID.ToString());
                                if (lstPhysician != null)
                                {
                                    xmlMember_ID[0].InnerText = lstPhysician.prefix + " " + lstPhysician.firstname + " " + lstPhysician.middlename + " " + lstPhysician.lastname;
                                }
                            }

                            string sPhysicianid = enc.Encounter_Provider_ID.ToString();
                            //XmlNodeList xmlPhysicianAddress = itemDoc.GetElementsByTagName("Physician_Address");
                            //if (xmlPhysicianAddress != null)
                            //{
                            //    string sPhysicianXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\PhysicianAddressDetails.xml";
                            //    XmlDocument itemPhysiciandoc = new XmlDocument();
                            //    XmlTextReader XmlPhysicianText = new XmlTextReader(sPhysicianXmlPath);
                            //    itemPhysiciandoc.Load(XmlPhysicianText);

                            //    XmlNodeList xmlphy = itemPhysiciandoc.GetElementsByTagName("p" + sPhysicianid);
                            //    if (xmlphy.Count > 0)
                            //    {
                            //        xmlPhysicianAddress[0].Attributes[0].Value = xmlphy[0].Attributes[0].Value;
                            //        xmlPhysicianAddress[0].Attributes[1].Value = xmlphy[0].Attributes[1].Value;
                            //        xmlPhysicianAddress[0].Attributes[2].Value = xmlphy[0].Attributes[2].Value;
                            //        xmlPhysicianAddress[0].Attributes[3].Value = xmlphy[0].Attributes[3].Value;
                            //        xmlPhysicianAddress[0].Attributes[4].Value = xmlphy[0].Attributes[4].Value;
                            //        xmlPhysicianAddress[0].Attributes[5].Value = xmlphy[0].Attributes[5].Value;
                            //        xmlPhysicianAddress[0].Attributes[6].Value = xmlphy[0].Attributes[6].Value;
                            //        xmlPhysicianAddress[0].Attributes[7].Value = xmlphy[0].Attributes[7].Value;
                            //    }
                            //}
                            //CAP-2780
                            XmlNodeList xmlPhysicianAddress = itemDoc.GetElementsByTagName("Physician_Address");
                            if (xmlPhysicianAddress != null)
                            {
                                PhysicianAddressDetailsList physicianAddressDetailsList = ConfigureBase<PhysicianAddressDetailsList>.ReadJson("PhysicianAddressDetails.json");
                                if (physicianAddressDetailsList != null)
                                {
                                    var matchingAddress = physicianAddressDetailsList.PhysicianAddress
                                    .FirstOrDefault(address => address.Physician_Library_ID == sPhysicianid.ToString());

                                    if (matchingAddress != null)
                                    {
                                        xmlPhysicianAddress[0].Attributes[0].Value = matchingAddress?.Physician_Address1 ?? "";
                                        xmlPhysicianAddress[0].Attributes[1].Value = matchingAddress?.Physician_Address2 ?? "";
                                        xmlPhysicianAddress[0].Attributes[2].Value = matchingAddress?.Physician_City ?? "";
                                        xmlPhysicianAddress[0].Attributes[3].Value = matchingAddress?.Physician_State ?? "";
                                        xmlPhysicianAddress[0].Attributes[4].Value = matchingAddress?.Physician_Zip ?? "";
                                        xmlPhysicianAddress[0].Attributes[5].Value = matchingAddress?.Physician_Telephone ?? "";
                                        xmlPhysicianAddress[0].Attributes[6].Value = matchingAddress?.Physician_Fax ?? "";
                                        xmlPhysicianAddress[0].Attributes[7].Value = matchingAddress?.Specialties ?? "";
                                    }
                                }
                            }
                            //itemDoc.Save(strXmlFilePath);
                            IList<Encounter_Blob> ilstUpdateBlob = new List<Encounter_Blob>();
                            byte[] bytes = null;
                            try
                            {
                                bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);
                            }
                            catch (Exception ex)
                            {

                            }
                            objEncounterblob.Encounter_XML = bytes;
                            ilstUpdateBlob.Add(objEncounterblob);
                            EncounterBlobManager EncBlobManager = new EncounterBlobManager();
                            EncBlobManager.SaveEncounterBlobWithTransaction(ilstUpdateBlob, string.Empty);
                        }


                    }
                    if (root.GetProperty("Current Process").GetString() == "MA_PROCESS")
                    {
                        EncounterManager encProxy = new EncounterManager();
                        Encounter enc = new Encounter();
                        IList<Encounter> encList = new List<Encounter>();
                        if (root.GetProperty("Encounter ID").GetString() != string.Empty)
                        {
                            encList = encProxy.GetEncounterByEncounterID(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()));
                        }


                        if (encList.Count == 0)
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('700014'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            return "DisplayErrorMessage-700014-return";
                        }
                        else
                        {
                            enc = encList[0];
                            if (sUpdateOwner.User_Name != string.Empty)
                            {
                                enc.Assigned_Med_Asst_User_Name = sUpdateOwner.User_Name;
                            }
                            encProxy.UpdateEncounter(enc, string.Empty, new object[] { "false" });
                        }
                    }
                    if (root.GetProperty("Current Process").GetString() == "SCRIBE_PROCESS" || root.GetProperty("Current Process").GetString() == "AKIDO_SCRIBE_PROCESS" || root.GetProperty("Current Process").GetString() == "SCRIBE_CORRECTION" || root.GetProperty("Current Process").GetString() == "SCRIBE_REVIEW_CORRECTION")
                    {
                        EncounterManager encProxy = new EncounterManager();
                        Encounter enc = new Encounter();
                        IList<Encounter> encList = new List<Encounter>();
                        if (root.GetProperty("Encounter ID").GetString() != string.Empty)
                        {
                            encList = encProxy.GetEncounterByEncounterID(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()));
                        }


                        if (encList.Count == 0)
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('700014'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            return "DisplayErrorMessage-700014-return";
                        }
                        else
                        {
                            enc = encList[0];
                            if (sUpdateOwner.User_Name != string.Empty)
                            {
                                enc.Assigned_Scribe_User_Name = sUpdateOwner.User_Name;
                            }
                            encProxy.UpdateEncounter(enc, string.Empty, new object[] { "false" });
                        }
                    }
                }



                //btnGetObjects_Click(sender, e);
                //FillGrid();
                //clearall();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "FillGrid(); ClearAll(); DisplayErrorMessage('700004'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return "DisplayErrorMessage-700004";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            //clearall();
            //grdAdminModule.DataSource = null;
            //grdAdminModule.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }
        //private void clearall()
        //{
        //    txtReasonForChange.Value = string.Empty;
        //    cboPreviousProcess.DataSource = new string[] { };
        //    cboPreviousProcess.DataBind();
        //    cboUpdateOwner.DataSource = new string[] { };
        //    cboUpdateOwner.DataBind();
        //    rbUpdateOwner.Checked = false;
        //    rbUpdateProcess.Checked = false;
        //    rbPanel.Enabled = false;
        //}


        //protected void rbUpdateOwner_CheckedChanged(object sender, EventArgs e)
        //{
        //    pnlUpdateProcess.Enabled = false;
        //    pnlUpdateOwner.Enabled = true;

        //    //Jira #CAP-195 - Old code 
        //    ////Update Owner
        //    //IList<ProcessMaster> proclist = ApplicationObject.processMasterList; // (IList<ProcessMaster>)ViewState["proclist"];
        //    //IList<ProcUser> ProcUser = (IList<ProcUser>)ViewState["ProcUser"];
        //    //GridDataItem grdSelectedItem = (GridDataItem)grdAdminModule.SelectedItems[0];

        //    //if (proclist.Count > 0)
        //    //{
        //    //    //ViewState["proclist"] = proclist;
        //    //    IList<ProcessMaster> BackPushAllowedProcess = (from p in proclist where p.Process_Name == grdSelectedItem["Current Process"].Text select p).ToList<ProcessMaster>();
        //    //    if (BackPushAllowedProcess.Count > 0)
        //    //    {
        //    //        if (BackPushAllowedProcess[0].Process_Type == "ASSIGNED")
        //    //        {
        //    //            UserManager lo = new UserManager();
        //    //            IList<string> userOldList = new List<string>();
        //    //            //IList<string> userList = new List<string>();
        //    //            if (grdSelectedItem["Current Process"].Text != string.Empty)
        //    //            {
        //    //                var user = from u in ProcUser where u.Process_Name == grdSelectedItem["Current Process"].Text && u.Status == "A" orderby u.User_Name select u.User_Name;
        //    //                userOldList = user.ToList<string>();
        //    //            }

        //    //            XDocument xmlUser = null;
        //    //            XDocument xmlPhysician = null;
        //    //            SortedDictionary<string, string> hashUserList = new SortedDictionary<string, string>();
        //    //            if (File.Exists(Server.MapPath(@"ConfigXML\User.xml")))
        //    //                xmlUser = XDocument.Load(Server.MapPath(@"ConfigXML\User.xml"));
        //    //            if (File.Exists(Server.MapPath(@"ConfigXML\PhysicianAddressDetails.xml")))
        //    //                xmlPhysician = XDocument.Load(Server.MapPath(@"ConfigXML\PhysicianAddressDetails.xml"));

        //    //            for (int iCount =0;iCount< userOldList.Count;iCount++)
        //    //            {
        //    //                if (xmlUser != null)
        //    //                {
        //    //                    foreach (XElement elements in xmlUser.Descendants("UserList"))
        //    //                    {
        //    //                        foreach (XElement UserElement in elements.Elements())
        //    //                        {
        //    //                            if (UserElement.Attribute("User_Name").Value.ToUpper() == userOldList[iCount].ToString() && UserElement.Attribute("Legal_Org").Value.ToUpper() == ClientSession.LegalOrg)
        //    //                            {
        //    //                                if (UserElement.Attribute("Physician_Library_ID").Value.ToUpper() == "0")
        //    //                                {
        //    //                                    //userList.Add(UserElement.Attribute("person_name").Value);

        //    //                                    //cboUpdateOwner.Items.Add(new RadComboBoxItem(UserElement.Attribute("person_name").Value, UserElement.Attribute("person_name").Value));

        //    //                                    if (hashUserList.ContainsKey(UserElement.Attribute("person_name").Value) == false)
        //    //                                    {
        //    //                                        hashUserList.Add(UserElement.Attribute("person_name").Value, UserElement.Attribute("User_Name").Value);
        //    //                                    }
        //    //                                }
        //    //                                else
        //    //                                {
        //    //                                    foreach (XElement element in xmlPhysician.Descendants("p" + UserElement.Attribute("Physician_Library_ID").Value))
        //    //                                    {
        //    //                                        string phyName = string.Empty;
        //    //                                        string prefix = string.Empty;
        //    //                                        string firstname = string.Empty;
        //    //                                        string middlename = string.Empty;
        //    //                                        string lastname = string.Empty;
        //    //                                        string suffix = string.Empty;

        //    //                                        if (element.Attribute("Physician_prefix").Value != null)
        //    //                                            prefix = element.Attribute("Physician_prefix").Value;
        //    //                                        if (element.Attribute("Physician_First_Name").Value != null)
        //    //                                            firstname = element.Attribute("Physician_First_Name").Value;
        //    //                                        if (element.Attribute("Physician_Middle_Name").Value != null)
        //    //                                            middlename = element.Attribute("Physician_Middle_Name").Value;
        //    //                                        if (element.Attribute("Physician_Last_Name").Value != null)
        //    //                                            lastname = element.Attribute("Physician_Last_Name").Value;
        //    //                                        if (element.Attribute("Physician_Suffix").Value != null)
        //    //                                            suffix = element.Attribute("Physician_Suffix").Value;

        //    //                                        //Gitlab# 2485 - Physician Name Display Change
        //    //                                        if (lastname != String.Empty)
        //    //                                            phyName += lastname;
        //    //                                        if (firstname != String.Empty)
        //    //                                        {
        //    //                                            if (phyName != String.Empty)
        //    //                                                phyName += "," + firstname;
        //    //                                            else
        //    //                                                phyName += firstname;
        //    //                                        }
        //    //                                        if (middlename != String.Empty)
        //    //                                            phyName += " " + middlename;
        //    //                                        if (suffix != String.Empty)
        //    //                                            phyName += "," + suffix;


        //    //                                        //userList.Add(phyName);
        //    //                                        //cboUpdateOwner.Items.Add(new RadComboBoxItem(phyName, UserElement.Attribute("User_Name").Value));
        //    //                                        if (hashUserList.ContainsKey(phyName) == false)
        //    //                                        {
        //    //                                            hashUserList.Add(phyName, UserElement.Attribute("User_Name").Value);
        //    //                                        }
        //    //                                    }
        //    //                                }
        //    //                            }
        //    //                        }
        //    //                    }
        //    //                }
        //    //            }

        //    //            foreach (var item in hashUserList)
        //    //            {
        //    //                cboUpdateOwner.Items.Add(new Telerik.Web.UI.RadComboBoxItem(item.Key, item.Value));
        //    //            }


        //    //            //cboUpdateOwner.DataSource = userList;
        //    //            //cboUpdateOwner.DataBind();
        //    //        }
        //    //    }
        //    //}
        //    //Jira #CAP-195 - new code
        //    FillCboOwner();
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //}
        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object rbUpdateOwner_CheckedChanged(string[] sSelectedRow, bool checkkShowAllOwner)
        {
            //Update Owner
            string sJoinedSelectedRows = "{" + string.Join(",", sSelectedRow) + "}";
            using (JsonDocument doc = JsonDocument.Parse(sJoinedSelectedRows))
            {
                JsonElement root = doc.RootElement;

                IList<ProcessMaster> proclist = ApplicationObject.processMasterList;
                ProcUserManager ProcUserMngr = new ProcUserManager();
                WorkFlowManager WorkFlowMngr = new WorkFlowManager();
                IList<ProcUser> ProcUser = ProcUserMngr.GetProcUserList();
                IList<(string value, string key)> strings = new List<(string value, string key)>();
                //SortedDictionary<string, string> hashUserList = new SortedDictionary<string, string>();
                if (proclist.Count > 0)
                {
                    IList<ProcessMaster> BackPushAllowedProcess = (from p in proclist where p.Process_Name == root.GetProperty("Current Process").GetString() select p).ToList<ProcessMaster>();
                    if (BackPushAllowedProcess.Count > 0)
                    {
                        if (BackPushAllowedProcess[0].Process_Type == "ASSIGNED")
                        {
                            UserManager lo = new UserManager();
                            IList<string> userOldList = new List<string>();
                            if (root.GetProperty("Current Process").GetString() != string.Empty)
                            {
                                var user = from u in ProcUser where u.Process_Name == root.GetProperty("Current Process").GetString() && u.Status == "A" orderby u.User_Name select u.User_Name;
                                userOldList = user.ToList<string>();
                            }

                            XDocument xmlPhysician = null;

                            MapFacilityPhysicianManager mapfacphymnge = new MapFacilityPhysicianManager();
                            IList<string> NonPhysician = new List<string>();
                            IList<MapFacilityPhysician> Physician = new List<MapFacilityPhysician>();
                            //CAP-2788
                            UserList ilstUserList = ConfigureBase<UserList>.ReadJson("User.json");
                            if (ilstUserList?.User != null)
                            {
                                var filteredData = ilstUserList?.User.FirstOrDefault(a => a.Physician_Library_ID.ToString() != "0" && a.User_Name == userOldList[0].ToString());
                                if (filteredData != null)
                                {
                                    Physician = mapfacphymnge.GetPhyisician_ListbyFacilityName(root.GetProperty("Facility Name").GetString(), ClientSession.LegalOrg);
                                }
                                else
                                {
                                    NonPhysician = mapfacphymnge.GetUser_ListbyFacilityName(root.GetProperty("Facility Name").GetString());
                                }
                            }
                            for (int iCount = 0; iCount < userOldList.Count; iCount++)
                            {
                                var filteredData = ilstUserList?.User.FirstOrDefault(a => a.User_Name.ToUpper() == userOldList[iCount].ToString() && a.Legal_Org.ToUpper() == ClientSession.LegalOrg);
                                if (ilstUserList?.User != null && filteredData != null)
                                {
                                    if (filteredData.Physician_Library_ID.ToUpper() == "0")
                                    {

                                        //if (hashUserList.ContainsKey(filteredData.person_name) == false)
                                        if ((strings.Where(x => x.key == filteredData.person_name).Count() > 0) == false)
                                        {
                                            if (checkkShowAllOwner == false)
                                            {
                                                //CAP -195 - no users are listed when show all is in unchecked state
                                                //if (NonPhysician != null && NonPhysician.Count > 0 && NonPhysician.Contains(UserElement.Attribute("person_name").Value.ToString()) == true)
                                                if (NonPhysician != null && NonPhysician.Count > 0 && NonPhysician.Contains(filteredData.User_Name) == true)
                                                {
                                                    //hashUserList.Add(filteredData.person_name, filteredData.User_Name);
                                                    strings.Add((filteredData.person_name, filteredData.User_Name));
                                                }
                                            }
                                            else
                                            {
                                                //hashUserList.Add(filteredData.person_name, filteredData.User_Name);
                                                strings.Add((filteredData.person_name, filteredData.User_Name));
                                            }
                                        }

                                    }
                                    else
                                    {
                                        //CAP-2780
                                        PhysicianAddressDetailsList physicianAddressDetailsList = ConfigureBase<PhysicianAddressDetailsList>.ReadJson("PhysicianAddressDetails.json");
                                        if (physicianAddressDetailsList != null)

                                        {
                                            foreach (var physician in physicianAddressDetailsList.PhysicianAddress.Where(x => x.Physician_Library_ID == filteredData.Physician_Library_ID))
                                            {
                                                string phyName = string.Empty;
                                                string prefix = physician.Physician_prefix ?? string.Empty;
                                                string firstname = physician.Physician_First_Name ?? string.Empty;
                                                string middlename = physician.Physician_Middle_Name ?? string.Empty;
                                                string lastname = physician.Physician_Last_Name ?? string.Empty;
                                                string suffix = physician.Physician_Suffix ?? string.Empty;

                                                if (!string.IsNullOrEmpty(lastname))
                                                    phyName += lastname;

                                                if (!string.IsNullOrEmpty(firstname))
                                                {
                                                    if (!string.IsNullOrEmpty(phyName))
                                                        phyName += ", " + firstname;
                                                    else
                                                        phyName += firstname;
                                                }

                                                if (!string.IsNullOrEmpty(middlename))
                                                    phyName += " " + middlename;

                                                if (!string.IsNullOrEmpty(suffix))
                                                    phyName += ", " + suffix;

                                                //if (!hashUserList.ContainsKey(phyName))
                                                if (!(strings.Where(x => x.key == phyName).Count() > 0))
                                                {
                                                    //if (!checkkShowAllOwner.Checked)
                                                    if (!(checkkShowAllOwner == true))
                                                    {
                                                        if (Physician != null && Physician.Count > 0)
                                                        {
                                                            var user = from u in Physician where u.Phy_Rec_ID == Convert.ToUInt64(physician.Physician_Library_ID) select u.Phy_Rec_ID;
                                                            IList<ulong> selectedUsers = user.ToList<ulong>();
                                                            if (selectedUsers.Count > 0)
                                                            {
                                                                //hashUserList.Add(phyName, filteredData.User_Name);
                                                                strings.Add((phyName, filteredData.User_Name));
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //hashUserList.Add(phyName, filteredData.User_Name);
                                                        strings.Add((phyName, filteredData.User_Name));
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            strings = strings.OrderBy(a => a.value).ToList();
                            var cboOwner = JsonConvert.SerializeObject(strings);
                            return cboOwner;
                        }


                    }
                }

                return JsonConvert.SerializeObject(strings);
            }
        }
        //protected void rbUpdateProcess_CheckedChanged(object sender, EventArgs e)
        //{
        //    pnlUpdateProcess.Enabled = true;
        //    pnlUpdateOwner.Enabled = false;

        //    //Jira CAP-2660
        //    cboPreviousProcess.Items.Clear();
        //    divWfObject.FindControl("grdAdminModuleWfobjwct");
        //    GridDataItem grdSelectedItem = (GridDataItem)grdAdminModule.SelectedItems[0];

        //    IList<ProcessMaster> proclist = ApplicationObject.processMasterList; // (IList<ProcessMaster>)ViewState["proclist"];
        //    IList<ProcUser> ProcUser = (IList<ProcUser>)ViewState["ProcUser"];

        //    if (proclist.Count > 0)
        //    {
        //        //ViewState["proclist"] = proclist;
        //        IList<ProcessMaster> BackPushAllowedProcess = (from p in proclist where p.Process_Name == grdSelectedItem["Current Process"].Text select p).ToList<ProcessMaster>();
        //        if (BackPushAllowedProcess.Count > 0)
        //        {
        //            if (BackPushAllowedProcess[0].Is_Back_Push_Allowed == "Y")
        //            {
        //                if (grdSelectedItem["Current Process"].Text == "MA_PROCESS")
        //                {
        //                    //cboPreviousProcess.Items.Add(new RadComboBoxItem("SCHEDULED"));
        //                    cboPreviousProcess.Items.Add("SCHEDULED");

        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //                    return;
        //                }
        //                //Jira #CAP-706-start
        //                if (grdSelectedItem["Current Process"].Text == "PROVIDER_PROCESS")
        //                {
        //                    //cboPreviousProcess.Items.Add(new RadComboBoxItem("AKIDO_SCRIBE_PROCESS"));
        //                    //cboPreviousProcess.Items.Add(new RadComboBoxItem("TRANSCRIPT_PROCESS"));
        //                    cboPreviousProcess.Items.Add("AKIDO_SCRIBE_PROCESS");
        //                    cboPreviousProcess.Items.Add("TRANSCRIPT_PROCESS");
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //                    return;
        //                }
        //                //Jira #CAP-706-end
        //                IList<WorkFlow> worflowList = WorkFlowMngr.GetWorkFlowMapListbyFacilityNameandObjType(grdSelectedItem["Facility Name"].Text, grdSelectedItem["Object Type"].Text, grdSelectedItem["Current Process"].Text);

        //                var wfProcess = (from wfToprocess in worflowList
        //                                 select new { wfToprocess.From_Process, wfToprocess.To_Process }).Distinct();

        //                foreach (var Toprocess in wfProcess)
        //                {
        //                    if (Toprocess.From_Process != "CHECK_OUT_WAIT" && Toprocess.From_Process != "CHECK_OUT_COMPLETE")
        //                    {
        //                        if (grdSelectedItem["Current Process"].Text != Toprocess.From_Process)//Added by Naveena for bug_id 26501,26498
        //                        {
        //                            if (grdSelectedItem["Current Process"].Text == "CANCELLED")
        //                            {
        //                                if (Toprocess.From_Process.ToString() != "MA_PROCESS")
        //                                {
        //                                    //cboPreviousProcess.Items.Add(new RadComboBoxItem(Toprocess.From_Process));
        //                                    cboPreviousProcess.Items.Add(Toprocess.From_Process);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                //cboPreviousProcess.Items.Add(new RadComboBoxItem(Toprocess.From_Process));
        //                                cboPreviousProcess.Items.Add(Toprocess.From_Process);
        //                            }
        //                        }
        //                    }


        //                }
        //            }
        //        }
        //    }

        //    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //}
        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object rbUpdateProcess_CheckedChanged(string[] sSelectedRow)
        {
            string sJoinedSelectedRows = "{" + string.Join(",", sSelectedRow) + "}";
            using (JsonDocument doc = JsonDocument.Parse(sJoinedSelectedRows))
            {
                JsonElement root = doc.RootElement;
                IList<ProcessMaster> proclist = ApplicationObject.processMasterList;
                ProcUserManager ProcUserMngr = new ProcUserManager();
                WorkFlowManager WorkFlowMngr = new WorkFlowManager();
                IList<ProcUser> ProcUser = ProcUserMngr.GetProcUserList();
                IList<string> strings = new List<string>();
                //string extra_search = HttpContext.Current.Request.Params["extra_search"];
                //var searchData = JsonConvert.DeserializeObject<Dictionary<string, string>>(extra_search);
                //string sAccountNo = searchData["AccountNo"];
                if (proclist.Count > 0)
                {
                    //ViewState["proclist"] = proclist;
                    IList<ProcessMaster> BackPushAllowedProcess = (from p in proclist where p.Process_Name == root.GetProperty("Current Process").GetString() select p).ToList<ProcessMaster>();
                    if (BackPushAllowedProcess.Count > 0)
                    {
                        if (BackPushAllowedProcess[0].Is_Back_Push_Allowed == "Y")
                        {
                            if (root.GetProperty("Current Process").GetString() == "MA_PROCESS")
                            {
                                //cboPreviousProcess.Items.Add(new RadComboBoxItem("SCHEDULED"));
                                //cboPreviousProcess.Items.Add("SCHEDULED");
                                strings.Add("SCHEDULED");

                                //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                return JsonConvert.SerializeObject(strings);
                            }
                            //Jira #CAP-706-start
                            if (root.GetProperty("Current Process").GetString() == "PROVIDER_PROCESS")
                            {
                                //cboPreviousProcess.Items.Add(new RadComboBoxItem("AKIDO_SCRIBE_PROCESS"));
                                //cboPreviousProcess.Items.Add(new RadComboBoxItem("TRANSCRIPT_PROCESS"));
                                //cboPreviousProcess.Items.Add("AKIDO_SCRIBE_PROCESS");
                                //cboPreviousProcess.Items.Add("TRANSCRIPT_PROCESS");
                                strings.Add("AKIDO_SCRIBE_PROCESS");
                                strings.Add("TRANSCRIPT_PROCESS");
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                return JsonConvert.SerializeObject(strings);
                            }
                            //Jira #CAP-706-end
                            IList<WorkFlow> worflowList = WorkFlowMngr.GetWorkFlowMapListbyFacilityNameandObjType(root.GetProperty("Facility Name").GetString(), root.GetProperty("Object Type").GetString(), root.GetProperty("Current Process").GetString());

                            var wfProcess = (from wfToprocess in worflowList
                                             select new { wfToprocess.From_Process, wfToprocess.To_Process }).Distinct();

                            foreach (var Toprocess in wfProcess)
                            {
                                if (Toprocess.From_Process != "CHECK_OUT_WAIT" && Toprocess.From_Process != "CHECK_OUT_COMPLETE")
                                {
                                    if (root.GetProperty("Current Process").GetString() != Toprocess.From_Process)//Added by Naveena for bug_id 26501,26498
                                    {
                                        if (root.GetProperty("Current Process").GetString() == "CANCELLED")
                                        {
                                            if (Toprocess.From_Process.ToString() != "MA_PROCESS")
                                            {
                                                //cboPreviousProcess.Items.Add(new RadComboBoxItem(Toprocess.From_Process));
                                                strings.Add(Toprocess.From_Process);
                                            }
                                        }
                                        else
                                        {
                                            //cboPreviousProcess.Items.Add(new RadComboBoxItem(Toprocess.From_Process));
                                            strings.Add(Toprocess.From_Process);
                                        }
                                    }
                                }


                            }
                        }
                    }
                }

                return JsonConvert.SerializeObject(strings);
            }
        }
        protected void grdAdminModule_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            //if (ViewState["AdminModule"] != null)
            //{
            //    ds = (DataSet)ViewState["AdminModule"];
            //}
            //if (e.NewSortOrder.ToString() == "Ascending")
            //{
            //    ds.Tables[0].DefaultView.Sort = "Date of Service";
            //}
            //else
            //{
            //    ds.Tables[0].DefaultView.Sort = "Date of Service DESC";
            //}
            //ds.AcceptChanges();

            //grdAdminModule.DataSource = ds.Tables[0];
            //grdAdminModule.DataBind();

        }

        //protected void grdAdminModule_PreRender(object sender, EventArgs e)
        //{
        //    foreach (GridColumn column in grdAdminModule.MasterTableView.AutoGeneratedColumns)
        //    {
        //        if (column.UniqueName == "Current Process")
        //        {
        //            column.HeaderStyle.Width = Unit.Pixel(150);
        //        }
        //        if (column.UniqueName == "Encounter ID")
        //        {
        //            column.HeaderStyle.Width = Unit.Pixel(60);
        //        }
        //        if (column.UniqueName == "Patient Name")
        //        {
        //            column.HeaderStyle.Width = Unit.Pixel(120);
        //        }
        //        if (column.UniqueName == "Object Type")
        //        {
        //            column.HeaderStyle.Width = Unit.Pixel(125);
        //        }
        //    }
        //}

        //protected void btnUpdateProcess_Click(object sender, EventArgs e)
        //{
        //    if (hdnSelectedRow.Value == "")
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700001');NotSaved();", true);
        //        return;
        //    }
        //    string sJoinedSelectedRows = string.Join(",", hdnSelectedRow.Value);
        //    using (JsonDocument doc = JsonDocument.Parse(sJoinedSelectedRows))
        //    {
        //        JsonElement root = doc.RootElement;
        //        string YesNoCancel = hdnMessageType.Value;
        //        hdnMessageType.Value = string.Empty;

        //        //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "btnUpdateProcessClick();", true);
        //        //if (grdAdminModule.SelectedItems.Count == 0)
        //        //{
        //        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700001');NotSaved();", true);
        //        //    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700001'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //        //    return;
        //        //}

        //        if (txtReasonForChange.Value == string.Empty)
        //        {

        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700003');NotSaved();", true);
        //            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700003'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //            txtReasonForChange.Focus();
        //            return;
        //        }

        //        //if (cboPreviousProcess.Text == "START")
        //        if (cboPreviousProcess.Value == "START")
        //        {

        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700007');NotSaved();", true);
        //            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700007'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //            return;
        //        }
        //        if (cboPreviousProcess.Value == string.Empty)
        //        //if (cboPreviousProcess.Text == string.Empty)
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700007');NotSaved();", true);
        //            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700009');", true);
        //            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700007'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //            cboPreviousProcess.Focus();
        //            return;
        //        }
        //        IList<ProcessMaster> proclist;
        //        if (ApplicationObject.processMasterList != null)
        //        {
        //            proclist = ApplicationObject.processMasterList;
        //        }

        //        //GridDataItem grdSelectedItem = (GridDataItem)grdAdminModule.SelectedItems[0];
        //        //GridDataItem selectedItem = (GridDataItem)grdAdminModule.SelectedItems[0];

        //        if (root.GetProperty("Current Process").GetString() != string.Empty && root.GetProperty("Current Process").GetString().ToUpper() == "MA_PROCESS" && root.GetProperty("Date of Service").GetString() != "01-Jan-0001 12:00 AM")
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700012');NotSaved();", true);
        //            return;
        //        }

        //        WFObject WFRecord = null;
        //        if (root.GetProperty("Encounter ID").GetString() != string.Empty && root.GetProperty("Object Type").GetString() != string.Empty)
        //        {
        //            if (root.GetProperty("Current Process").GetString() == "MA_PROCESS")
        //            {
        //                WFRecord = WfObjectMngr.GetByObjectSystemId(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), "ENCOUNTER");
        //            }
        //            else
        //            {
        //                WFRecord = WfObjectMngr.GetByObjectSystemId(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), root.GetProperty("Object Type").GetString());
        //            }
        //            if (WFRecord.Id == 0)
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('700014'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //                return;
        //            }
        //        }

        //        if (root.GetProperty("Current Process").GetString() == "MA_PROCESS")
        //        {
        //            if (root.GetProperty("Encounter ID").GetString() != string.Empty && root.GetProperty("Object Type").GetString() != string.Empty)
        //            {
        //                WfObjectMngr.MoveToPreviousProcessForAdmin(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), root.GetProperty("Object Type").GetString(), "UNKNOWN", cboPreviousProcess.Value, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty);
        //            }
        //            WfObjectMngr.MoveToPreviousProcessForAdmin(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), "ENCOUNTER", "UNKNOWN", cboPreviousProcess.Value, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty);

        //            WFObjectManager wfObjMngr = new WFObjectManager();
        //            WFObject DocumentationWfObject = null;
        //            DocumentationWfObject = wfObjMngr.GetByObjectSystemId(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), "DOCUMENTATION");
        //            wfObjMngr.DeleteDocumentationObject(DocumentationWfObject);

        //            WFObject BillingWfObject = null;
        //            BillingWfObject = wfObjMngr.GetByObjectSystemId(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), "BILLING");
        //            wfObjMngr.DeleteDocumentationObject(BillingWfObject);
        //        }
        //        //Jira #CAP-706-start
        //        else if (root.GetProperty("Current Process").GetString() == "PROVIDER_PROCESS")
        //        {

        //            if (root.GetProperty("Encounter ID").GetString() != string.Empty && root.GetProperty("Object Type").GetString() != string.Empty)
        //            {
        //                //Jira #CAP-724 -start
        //                //Jira #CAP-855
        //                string sExMessage = "";
        //                string sStatus = "";
        //                string sIsAkidoEncounter = "false";
        //                string sIsCapellaEncounter = string.Empty;
        //                //Jira CAP-1990
        //                //sIsAkidoEncounter = UtilityManager.IsAkidoEncounter(grdAdminModule.SelectedItems[0].Cells[2].Text.ToString(), out sExMessage);
        //                sIsCapellaEncounter = UtilityManager.IsCapellaEncounter(string.Empty, Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()));
        //                if (sIsCapellaEncounter != "Y")
        //                {
        //                    sIsAkidoEncounter = UtilityManager.IsAkidoEncounter(root.GetProperty("Encounter ID").GetString().ToString(), out sExMessage, out sStatus);
        //                }
        //                //Jira CAP-1990
        //                //if (System.Configuration.ConfigurationSettings.AppSettings["IsAkidoEncounterCheck"] == "Y" && cboPreviousProcess.SelectedItem.Text == "AKIDO_SCRIBE_PROCESS" && sIsAkidoEncounter == "false")
        //                if (System.Configuration.ConfigurationSettings.AppSettings["IsAkidoEncounterCheck"] == "Y" && cboPreviousProcess.Value == "AKIDO_SCRIBE_PROCESS" && sStatus != System.Configuration.ConfigurationSettings.AppSettings["AkidoSignedStatus"].ToString())
        //                {
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('1011196'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //                    return;
        //                }
        //                else if (System.Configuration.ConfigurationSettings.AppSettings["IsAkidoNoteSummary"] == "Y" && sIsAkidoEncounter == "Exception")
        //                {
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('1011199'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //                    return;
        //                }
        //                //Jira #CAP-724 -end
        //                WfObjectMngr.MoveToPreviousProcessForAdmin(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), root.GetProperty("Object Type").GetString(), "UNKNOWN", cboPreviousProcess.Value, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty);

        //                //WfObjectMngr.MoveToNextProcess(Convert.ToUInt64(grdAdminModule.SelectedItems[0].Cells[2].Text),"ENCOUNTER",1,"UNKNOWN",)
        //                //    (Convert.ToUInt64(grdAdminModule.SelectedItems[0].Cells[2].Text), "ENCOUNTER", "UNKNOWN", cboPreviousProcess.SelectedItem.Text, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty);

        //                if (sStatus == System.Configuration.ConfigurationSettings.AppSettings["AkidoSignedStatus"].ToString())
        //                {
        //                    EncounterManager objEncounter = new EncounterManager();
        //                    IList<Encounter> ilstUpdateEncounter = new List<Encounter>();

        //                    ilstUpdateEncounter = objEncounter.GetEncounterByEncounterID(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()));
        //                    if (ilstUpdateEncounter.Count > 0)
        //                    {
        //                        ilstUpdateEncounter[0].Is_Signed_in_Akido_Note = "Y";
        //                        ilstUpdateEncounter[0].Modified_By = ClientSession.UserName;
        //                        ilstUpdateEncounter[0].Modified_Date_and_Time = UtilityManager.ConvertToUniversal(); ;

        //                        //Jira CAP-2887
        //                        EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
        //                        bool bIsUpdateinBlob = true;
        //                        IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(ilstUpdateEncounter.FirstOrDefault().Id);
        //                        if (ilstEncounterBlob.Count == 0)
        //                        {
        //                            bIsUpdateinBlob = false;
        //                        }
        //                        //Jira CAP-2887
        //                        // objEncounter.SaveandMoveAkidoEncounter(ilstUpdateEncounter);
        //                        objEncounter.SaveandMoveAkidoEncounter(ilstUpdateEncounter, bIsUpdateinBlob);
        //                    }
        //                }
        //            }
        //        }
        //        //Jira #CAP-706-end
        //        else
        //        {
        //            //WfObjectMngr.MoveToPreviousProcessForAdmin(Convert.ToUInt64(grdAdminModule.SelectedItems[0].Cells[2].Text), selectedItem["Object Type"].Text, "UNKNOWN", cboPreviousProcess.SelectedItem.Text, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty);
        //            WfObjectMngr.MoveToPreviousProcessForAdmin(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), root.GetProperty("Object Type").GetString(), "UNKNOWN", cboPreviousProcess.Value, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty);
        //        }

        //        CorrectedDBEntries CorrDBEntry = new CorrectedDBEntries();
        //        CorrDBEntry.Corrected_By = ClientSession.UserName;
        //        CorrDBEntry.Corrected_Date = UtilityManager.ConvertToLocal(DateTime.Now);
        //        //WFObject WFRecord = null;
        //        //if (grdAdminModule.SelectedItems[0].Cells[2].Text != string.Empty && selectedItem["Object Type"].Text!=string.Empty)
        //        //{
        //        //    WFRecord = WfObjectMngr.GetByObjectSystemId(Convert.ToUInt64(grdAdminModule.SelectedItems[0].Cells[2].Text), selectedItem["Object Type"].Text);
        //        //}

        //        CorrDBEntry.New_Owner = WFRecord.Current_Owner;
        //        CorrDBEntry.New_Process = WFRecord.Current_Process;
        //        if (root.GetProperty("Current Owner").GetString() != string.Empty)
        //        {
        //            CorrDBEntry.Old_Owner = root.GetProperty("Current Owner").GetString();
        //        }
        //        if (root.GetProperty("Current Process").GetString() != string.Empty)
        //        {
        //            CorrDBEntry.Old_Process = root.GetProperty("Current Process").GetString();
        //        }

        //        CorrDBEntry.Reason_for_Changes = txtReasonForChange.Value;
        //        CorrDBEntry.Type_of_Change = "UPDATE_PROCESS";
        //        CorrDBEntry.WF_Object_ID = Convert.ToInt32(WFRecord.Id);
        //        CorrDBEntryProxy.AppendToCorrectedDBEntries(CorrDBEntry, string.Empty);

        //        if (cboPreviousProcess.Value != string.Empty)
        //        {
        //            if (cboPreviousProcess.Value.ToUpper() == "PHY_ASST_PROCESS")
        //            {
        //                if (root.GetProperty("Encounter ID").GetString() != string.Empty)
        //                {
        //                    PhyAsstProcess(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString().ToString()), "Y");
        //                }

        //            }
        //        }
        //        //clearall();
        //        //btnGetObjects_Click(sender, e);
        //        //FillGrid();
        //        if (YesNoCancel == string.Empty)
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "ClearAll(); FillGrid(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('700002'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "ClearAll(); FillGrid(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('700002');SaveSuccess();", true);
        //        }


        //    }
        //}
        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object btnUpdateProcess_Click(string[] sSelectedRow, string shdnMessageType,string stxtReasonForChange,string scboPreviousProcess)
        {
            
            string sJoinedSelectedRows = "{" + string.Join(",", sSelectedRow) + "}";
            using (JsonDocument doc = JsonDocument.Parse(sJoinedSelectedRows))
            {
                JsonElement root = doc.RootElement;
                string YesNoCancel = shdnMessageType;
                //hdnMessageType.Value = string.Empty;

                //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "btnUpdateProcessClick();", true);
                //if (grdAdminModule.SelectedItems.Count == 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700001');NotSaved();", true);
                //    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700001'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //    return;
                //}

                //if (txtReasonForChange.Value == string.Empty)
                //{

                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700003');NotSaved();", true);
                //    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700003'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //    txtReasonForChange.Focus();
                //    return;
                //}

                //if (cboPreviousProcess.Text == "START")
                //if (cboPreviousProcess.Value == "START")
                //{

                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700007');NotSaved();", true);
                //    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700007'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //    return;
                //}
                //if (cboPreviousProcess.Value == string.Empty)
                ////if (cboPreviousProcess.Text == string.Empty)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700007');NotSaved();", true);
                //    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700009');", true);
                //    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('700007'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //    cboPreviousProcess.Focus();
                //    return;
                //}
                IList<ProcessMaster> proclist;
                if (ApplicationObject.processMasterList != null)
                {
                    proclist = ApplicationObject.processMasterList;
                }

                //GridDataItem grdSelectedItem = (GridDataItem)grdAdminModule.SelectedItems[0];
                //GridDataItem selectedItem = (GridDataItem)grdAdminModule.SelectedItems[0];

                if (root.GetProperty("Current Process").GetString() != string.Empty && root.GetProperty("Current Process").GetString().ToUpper() == "MA_PROCESS" && root.GetProperty("Date of Service").GetString() != "01-Jan-0001 12:00 AM")
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "string.Empty", "DisplayErrorMessage('700012');NotSaved();", true);
                    return "DisplayErrorMessage-700012-NotSaved-return";
                }

                WFObject WFRecord = null;
                WFObjectManager WfObjectMngr = new WFObjectManager();
                if (root.GetProperty("Encounter ID").GetString() != string.Empty && root.GetProperty("Object Type").GetString() != string.Empty)
                {
                    if (root.GetProperty("Current Process").GetString() == "MA_PROCESS")
                    {
                        WFRecord = WfObjectMngr.GetByObjectSystemId(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), "ENCOUNTER");
                    }
                    else
                    {
                        WFRecord = WfObjectMngr.GetByObjectSystemId(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), root.GetProperty("Object Type").GetString());
                    }
                    if (WFRecord.Id == 0)
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('700014'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return "DisplayErrorMessage-700014-return";
                    }
                }

                if (root.GetProperty("Current Process").GetString() == "MA_PROCESS")
                {
                    if (root.GetProperty("Encounter ID").GetString() != string.Empty && root.GetProperty("Object Type").GetString() != string.Empty)
                    {
                        WfObjectMngr.MoveToPreviousProcessForAdmin(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), root.GetProperty("Object Type").GetString(), "UNKNOWN", scboPreviousProcess, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty);
                    }
                    WfObjectMngr.MoveToPreviousProcessForAdmin(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), "ENCOUNTER", "UNKNOWN", scboPreviousProcess, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty);

                    WFObjectManager wfObjMngr = new WFObjectManager();
                    WFObject DocumentationWfObject = null;
                    DocumentationWfObject = wfObjMngr.GetByObjectSystemId(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), "DOCUMENTATION");
                    wfObjMngr.DeleteDocumentationObject(DocumentationWfObject);

                    WFObject BillingWfObject = null;
                    BillingWfObject = wfObjMngr.GetByObjectSystemId(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), "BILLING");
                    wfObjMngr.DeleteDocumentationObject(BillingWfObject);
                }
                //Jira #CAP-706-start
                else if (root.GetProperty("Current Process").GetString() == "PROVIDER_PROCESS")
                {

                    if (root.GetProperty("Encounter ID").GetString() != string.Empty && root.GetProperty("Object Type").GetString() != string.Empty)
                    {
                        //Jira #CAP-724 -start
                        //Jira #CAP-855
                        string sExMessage = "";
                        string sStatus = "";
                        string sIsAkidoEncounter = "false";
                        string sIsCapellaEncounter = string.Empty;
                        //Jira CAP-1990
                        //sIsAkidoEncounter = UtilityManager.IsAkidoEncounter(grdAdminModule.SelectedItems[0].Cells[2].Text.ToString(), out sExMessage);
                        sIsCapellaEncounter = UtilityManager.IsCapellaEncounter(string.Empty, Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()));
                        if (sIsCapellaEncounter != "Y")
                        {
                            sIsAkidoEncounter = UtilityManager.IsAkidoEncounter(root.GetProperty("Encounter ID").GetString().ToString(), out sExMessage, out sStatus);
                        }
                        //Jira CAP-1990
                        //if (System.Configuration.ConfigurationSettings.AppSettings["IsAkidoEncounterCheck"] == "Y" && cboPreviousProcess.SelectedItem.Text == "AKIDO_SCRIBE_PROCESS" && sIsAkidoEncounter == "false")
                        if (System.Configuration.ConfigurationSettings.AppSettings["IsAkidoEncounterCheck"] == "Y" && scboPreviousProcess == "AKIDO_SCRIBE_PROCESS" && sStatus != System.Configuration.ConfigurationSettings.AppSettings["AkidoSignedStatus"].ToString())
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('1011196'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            return "DisplayErrorMessage-1011196-return";
                        }
                        else if (System.Configuration.ConfigurationSettings.AppSettings["IsAkidoNoteSummary"] == "Y" && sIsAkidoEncounter == "Exception")
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('1011199'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            return "DisplayErrorMessage-1011199-return";
                        }
                        //Jira #CAP-724 -end
                        WfObjectMngr.MoveToPreviousProcessForAdmin(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), root.GetProperty("Object Type").GetString(), "UNKNOWN", scboPreviousProcess, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty);

                        //WfObjectMngr.MoveToNextProcess(Convert.ToUInt64(grdAdminModule.SelectedItems[0].Cells[2].Text),"ENCOUNTER",1,"UNKNOWN",)
                        //    (Convert.ToUInt64(grdAdminModule.SelectedItems[0].Cells[2].Text), "ENCOUNTER", "UNKNOWN", cboPreviousProcess.SelectedItem.Text, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty);

                        if (sStatus == System.Configuration.ConfigurationSettings.AppSettings["AkidoSignedStatus"].ToString())
                        {
                            EncounterManager objEncounter = new EncounterManager();
                            IList<Encounter> ilstUpdateEncounter = new List<Encounter>();

                            ilstUpdateEncounter = objEncounter.GetEncounterByEncounterID(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()));
                            if (ilstUpdateEncounter.Count > 0)
                            {
                                ilstUpdateEncounter[0].Is_Signed_in_Akido_Note = "Y";
                                ilstUpdateEncounter[0].Modified_By = ClientSession.UserName;
                                ilstUpdateEncounter[0].Modified_Date_and_Time = UtilityManager.ConvertToUniversal(); ;

                                //Jira CAP-2887
                                EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
                                bool bIsUpdateinBlob = true;
                                IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(ilstUpdateEncounter.FirstOrDefault().Id);
                                if (ilstEncounterBlob.Count == 0)
                                {
                                    bIsUpdateinBlob = false;
                                }
                                //Jira CAP-2887
                                // objEncounter.SaveandMoveAkidoEncounter(ilstUpdateEncounter);
                                objEncounter.SaveandMoveAkidoEncounter(ilstUpdateEncounter, bIsUpdateinBlob);
                            }
                        }
                    }
                }
                //Jira #CAP-706-end
                else
                {
                    //WfObjectMngr.MoveToPreviousProcessForAdmin(Convert.ToUInt64(grdAdminModule.SelectedItems[0].Cells[2].Text), selectedItem["Object Type"].Text, "UNKNOWN", cboPreviousProcess.SelectedItem.Text, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty);
                    WfObjectMngr.MoveToPreviousProcessForAdmin(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString()), root.GetProperty("Object Type").GetString(), "UNKNOWN", scboPreviousProcess, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty);
                }

                CorrectedDBEntries CorrDBEntry = new CorrectedDBEntries();
                CorrDBEntry.Corrected_By = ClientSession.UserName;
                CorrDBEntry.Corrected_Date = UtilityManager.ConvertToLocal(DateTime.Now);
                //WFObject WFRecord = null;
                //if (grdAdminModule.SelectedItems[0].Cells[2].Text != string.Empty && selectedItem["Object Type"].Text!=string.Empty)
                //{
                //    WFRecord = WfObjectMngr.GetByObjectSystemId(Convert.ToUInt64(grdAdminModule.SelectedItems[0].Cells[2].Text), selectedItem["Object Type"].Text);
                //}

                CorrDBEntry.New_Owner = WFRecord.Current_Owner;
                CorrDBEntry.New_Process = WFRecord.Current_Process;
                if (root.GetProperty("Current Owner").GetString() != string.Empty)
                {
                    CorrDBEntry.Old_Owner = root.GetProperty("Current Owner").GetString();
                }
                if (root.GetProperty("Current Process").GetString() != string.Empty)
                {
                    CorrDBEntry.Old_Process = root.GetProperty("Current Process").GetString();
                }

                CorrDBEntry.Reason_for_Changes = stxtReasonForChange;
                CorrDBEntry.Type_of_Change = "UPDATE_PROCESS";
                CorrDBEntry.WF_Object_ID = Convert.ToInt32(WFRecord.Id);
                CorrectedDBEntriesManager CorrDBEntryProxy = new CorrectedDBEntriesManager();
                CorrDBEntryProxy.AppendToCorrectedDBEntries(CorrDBEntry, string.Empty);

                if (scboPreviousProcess != string.Empty)
                {
                    if (scboPreviousProcess.ToUpper() == "PHY_ASST_PROCESS")
                    {
                        if (root.GetProperty("Encounter ID").GetString() != string.Empty)
                        {
                            PhyAsstProcess(Convert.ToUInt64(root.GetProperty("Encounter ID").GetString().ToString()), "Y");
                        }

                    }
                }
                //clearall();
                //btnGetObjects_Click(sender, e);
                //FillGrid();
                if (YesNoCancel == string.Empty)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "ClearAll(); FillGrid(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('700002'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return "DisplayErrorMessage-700002-ClearAll-FillGrid";
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "ClearAll(); FillGrid(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('700002');SaveSuccess();", true);
                    return "DisplayErrorMessage-700002-SaveSuccess-ClearAll-FillGrid";
                }


            }
        }
        //Jira #CAP-195 - new event (checkkShowAllOwner_CheckedChanged)
        //protected void checkkShowAllOwner_CheckedChanged(object sender, EventArgs e)
        //{
        //    FillCboOwner();
        //}
        //Jira #CAP-195 - new method (FillCboOwner)
        //private void FillCboOwner()
        //{

        //    cboUpdateOwner.Items.Clear();
        //    //Update Owner
        //    IList<ProcessMaster> proclist = ApplicationObject.processMasterList; // (IList<ProcessMaster>)ViewState["proclist"];
        //    IList<ProcUser> ProcUser = (IList<ProcUser>)ViewState["ProcUser"];
        //    GridDataItem grdSelectedItem = (GridDataItem)grdAdminModule.SelectedItems[0];

        //    if (proclist.Count > 0)
        //    {
        //        //ViewState["proclist"] = proclist;
        //        IList<ProcessMaster> BackPushAllowedProcess = (from p in proclist where p.Process_Name == grdSelectedItem["Current Process"].Text select p).ToList<ProcessMaster>();
        //        if (BackPushAllowedProcess.Count > 0)
        //        {
        //            if (BackPushAllowedProcess[0].Process_Type == "ASSIGNED")
        //            {
        //                UserManager lo = new UserManager();
        //                IList<string> userOldList = new List<string>();
        //                //IList<string> userList = new List<string>();
        //                if (grdSelectedItem["Current Process"].Text != string.Empty)
        //                {
        //                    var user = from u in ProcUser where u.Process_Name == grdSelectedItem["Current Process"].Text && u.Status == "A" orderby u.User_Name select u.User_Name;
        //                    userOldList = user.ToList<string>();
        //                }

        //                XDocument xmlPhysician = null;
        //                SortedDictionary<string, string> hashUserList = new SortedDictionary<string, string>();
        //                //if (File.Exists(Server.MapPath(@"ConfigXML\PhysicianAddressDetails.xml")))
        //                //    xmlPhysician = XDocument.Load(Server.MapPath(@"ConfigXML\PhysicianAddressDetails.xml"));
        //                MapFacilityPhysicianManager mapfacphymnge = new MapFacilityPhysicianManager();
        //                IList<string> NonPhysician = new List<string>();
        //                IList<MapFacilityPhysician> Physician = new List<MapFacilityPhysician>();
        //                //CAP-2788
        //                UserList ilstUserList = ConfigureBase<UserList>.ReadJson("User.json");
        //                if (ilstUserList?.User != null)
        //                {
        //                    var filteredData = ilstUserList?.User.FirstOrDefault(a => a.Physician_Library_ID.ToString() != "0" && a.User_Name == userOldList[0].ToString());
        //                    if (filteredData != null)
        //                    {
        //                        Physician = mapfacphymnge.GetPhyisician_ListbyFacilityName(grdSelectedItem["Facility Name"].Text, ClientSession.LegalOrg);
        //                    }
        //                    else
        //                    {
        //                        NonPhysician = mapfacphymnge.GetUser_ListbyFacilityName(grdSelectedItem["Facility Name"].Text);
        //                    }
        //                }
        //                for (int iCount = 0; iCount < userOldList.Count; iCount++)
        //                {
        //                    var filteredData = ilstUserList?.User.FirstOrDefault(a => a.User_Name.ToUpper() == userOldList[iCount].ToString() && a.Legal_Org.ToUpper() == ClientSession.LegalOrg);
        //                    if (ilstUserList?.User != null && filteredData != null)
        //                    {
        //                        if (filteredData.Physician_Library_ID.ToUpper() == "0")
        //                        {
        //                            //userList.Add(UserElement.Attribute("person_name").Value);

        //                            //cboUpdateOwner.Items.Add(new RadComboBoxItem(UserElement.Attribute("person_name").Value, UserElement.Attribute("person_name").Value));


        //                            //if (hashUserList.ContainsKey(UserElement.Attribute("person_name").Value) == false)
        //                            //{
        //                            //    hashUserList.Add(UserElement.Attribute("person_name").Value, UserElement.Attribute("User_Name").Value);
        //                            //}
        //                            if (hashUserList.ContainsKey(filteredData.person_name) == false)
        //                            {
        //                                if (checkkShowAllOwner.Checked == false)
        //                                {
        //                                    //CAP -195 - no users are listed when show all is in unchecked state
        //                                    //if (NonPhysician != null && NonPhysician.Count > 0 && NonPhysician.Contains(UserElement.Attribute("person_name").Value.ToString()) == true)
        //                                    if (NonPhysician != null && NonPhysician.Count > 0 && NonPhysician.Contains(filteredData.User_Name) == true)
        //                                    {
        //                                        hashUserList.Add(filteredData.person_name, filteredData.User_Name);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    hashUserList.Add(filteredData.person_name, filteredData.User_Name);
        //                                }
        //                            }

        //                        }
        //                        else
        //                        {
        //                            //CAP-2780
        //                            PhysicianAddressDetailsList physicianAddressDetailsList = ConfigureBase<PhysicianAddressDetailsList>.ReadJson("PhysicianAddressDetails.json");
        //                            if (physicianAddressDetailsList != null)

        //                            {
        //                                foreach (var physician in physicianAddressDetailsList.PhysicianAddress.Where(x=> x.Physician_Library_ID == filteredData.Physician_Library_ID))
        //                                {
        //                                    string phyName = string.Empty;
        //                                    string prefix = physician.Physician_prefix ?? string.Empty;
        //                                    string firstname = physician.Physician_First_Name ?? string.Empty;
        //                                    string middlename = physician.Physician_Middle_Name ?? string.Empty;
        //                                    string lastname = physician.Physician_Last_Name ?? string.Empty;
        //                                    string suffix = physician.Physician_Suffix ?? string.Empty;

        //                                    if (!string.IsNullOrEmpty(lastname))
        //                                        phyName += lastname;

        //                                    if (!string.IsNullOrEmpty(firstname))
        //                                    {
        //                                        if (!string.IsNullOrEmpty(phyName))
        //                                            phyName += ", " + firstname;
        //                                        else
        //                                            phyName += firstname;
        //                                    }

        //                                    if (!string.IsNullOrEmpty(middlename))
        //                                        phyName += " " + middlename;

        //                                    if (!string.IsNullOrEmpty(suffix))
        //                                        phyName += ", " + suffix;

        //                                    if (!hashUserList.ContainsKey(phyName))
        //                                    {
        //                                        if (!checkkShowAllOwner.Checked)
        //                                        {
        //                                            if (Physician != null && Physician.Count > 0)
        //                                            {
        //                                                var user = from u in Physician where u.Phy_Rec_ID == Convert.ToUInt64(physician.Physician_Library_ID) select u.Phy_Rec_ID;
        //                                                IList<ulong> selectedUsers = user.ToList<ulong>();
        //                                                if (selectedUsers.Count > 0)
        //                                                {
        //                                                    hashUserList.Add(phyName, filteredData.User_Name);
        //                                                }
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            hashUserList.Add(phyName, filteredData.User_Name);
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            //foreach (XElement element in xmlPhysician.Descendants("p" + UserElement.Attribute("Physician_Library_ID").Value))
        //                            //{
        //                            //    string phyName = string.Empty;
        //                            //    string prefix = string.Empty;
        //                            //    string firstname = string.Empty;
        //                            //    string middlename = string.Empty;
        //                            //    string lastname = string.Empty;
        //                            //    string suffix = string.Empty;

        //                            //    if (element.Attribute("Physician_prefix").Value != null)
        //                            //        prefix = element.Attribute("Physician_prefix").Value;
        //                            //    if (element.Attribute("Physician_First_Name").Value != null)
        //                            //        firstname = element.Attribute("Physician_First_Name").Value;
        //                            //    if (element.Attribute("Physician_Middle_Name").Value != null)
        //                            //        middlename = element.Attribute("Physician_Middle_Name").Value;
        //                            //    if (element.Attribute("Physician_Last_Name").Value != null)
        //                            //        lastname = element.Attribute("Physician_Last_Name").Value;
        //                            //    if (element.Attribute("Physician_Suffix").Value != null)
        //                            //        suffix = element.Attribute("Physician_Suffix").Value;

        //                            //    //Gitlab# 2485 - Physician Name Display Change
        //                            //    if (lastname != String.Empty)
        //                            //        phyName += lastname;
        //                            //    if (firstname != String.Empty)
        //                            //    {
        //                            //        if (phyName != String.Empty)
        //                            //            phyName += "," + firstname;
        //                            //        else
        //                            //            phyName += firstname;
        //                            //    }
        //                            //    if (middlename != String.Empty)
        //                            //        phyName += " " + middlename;
        //                            //    if (suffix != String.Empty)
        //                            //        phyName += "," + suffix;


        //                            //    //userList.Add(phyName);
        //                            //    //cboUpdateOwner.Items.Add(new RadComboBoxItem(phyName, UserElement.Attribute("User_Name").Value));

        //                            //    //if (hashUserList.ContainsKey(phyName) == false)
        //                            //    //{
        //                            //    //    hashUserList.Add(phyName, UserElement.Attribute("User_Name").Value);
        //                            //    //}
        //                            //    if (hashUserList.ContainsKey(phyName) == false)
        //                            //    {
        //                            //        if (checkkShowAllOwner.Checked == false)
        //                            //        {
        //                            //            if (Physician != null && Physician.Count > 0)
        //                            //            {
        //                            //                var user = from u in Physician where u.Phy_Rec_ID == Convert.ToUInt64(element.Attribute("Physician_Library_ID").Value) select u.Phy_Rec_ID;
        //                            //                IList<ulong> selectedUsers = user.ToList<ulong>();
        //                            //                if (selectedUsers.Count > 0)
        //                            //                {
        //                            //                    hashUserList.Add(phyName, UserElement.Attribute("User_Name").Value);
        //                            //                }
        //                            //            }
        //                            //        }
        //                            //        else
        //                            //        {
        //                            //            hashUserList.Add(phyName, UserElement.Attribute("User_Name").Value);
        //                            //        }
        //                            //    }

        //                            //}
        //                        }
        //                    }
        //                }   


        //                foreach (var item in hashUserList)
        //                {
        //                    //cboUpdateOwner.Items.Add(new Telerik.Web.UI.RadComboBoxItem(item.Key, item.Value));
        //                    cboUpdateOwner.Items.Add(new ListItem() { Text = item.Key, Value = item.Value });
        //                }
        //            }

        //            //cboUpdateOwner.DataSource = userList;
        //            //cboUpdateOwner.DataBind();
        //        }
        //    }

        //    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //}
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

        
    }
}
