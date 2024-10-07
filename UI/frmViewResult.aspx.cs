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
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DTO;
using Telerik.Web.UI;
using System.IO;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Xml;
using System.Text;
using System.Threading;
using System.Web.Services;
using Newtonsoft.Json;


namespace Acurus.Capella.UI
{
    public partial class frmViewResult : System.Web.UI.Page
    {
        //BugID:43099 --Revamp--
        #region  Server.MapPath


        public RadTab tb = null;
        ResultMaster objresultmaster = null;
        RadListBox lsttype = new RadListBox();
        FillPhysicianUser PhyUserList;
        string[] sNotes = null;

        [Serializable]
        public class Result_Files_List
        {
            public DateTime _Res_Date = DateTime.MinValue;
            public ulong _Ord_Id = 0;
            public string _Doc_Type = string.Empty;
            public string _Sub_Doc_Type = string.Empty;
            public string _Img_path = string.Empty;
            public ulong _Res_Id = 0;
            public string _Order_Description = string.Empty;
            public ulong _IndexId = 0;

            public string Doc_Type
            {
                get { return _Doc_Type; }
                set { _Doc_Type = value; }
            }
            public string Sub_Doc_Type
            {
                get { return _Sub_Doc_Type; }
                set { _Sub_Doc_Type = value; }
            }
            public ulong Order_Submit_Id
            {
                get { return _Ord_Id; }
                set { _Ord_Id = value; }
            }
            public DateTime Result_Date
            {
                get { return _Res_Date; }
                set { _Res_Date = value; }
            }
            public string Ord_Type
            {
                get { return _Doc_Type; }
                set { _Doc_Type = value; }
            }
            public ulong Result_Id
            {
                get { return _Res_Id; }
                set { _Res_Id = value; }
            }
            public string Img_Path
            {
                get { return _Img_path; }
                set { _Img_path = value; }
            }

            public string Order_Description
            {
                get { return _Order_Description; }
                set { _Order_Description = value; }
            }
            public ulong IndexId
            {
                get { return _IndexId; }
                set { _IndexId = value; }

            }
        }
        string sResult_obr_date = string.Empty;
        string sProcedureDescription = string.Empty;
        ulong uOrderID = 0;



        protected void Page_Load(object sender, EventArgs e)
        {
            ulong human_id = 0;
            string Document_type = string.Empty;
            ulong Key_id = 0;
            if (ConfigurationManager.AppSettings["TemplateNotes"] != null)
                sNotes = ConfigurationManager.AppSettings["TemplateNotes"].ToString().Split('|');            
            if (Request["HumanID"] != null && Request["HumanID"] != "")
            {
                human_id = Convert.ToUInt64(Request["HumanID"].ToString());
            }


            if (!IsPostBack)
            {
               
                if (Request["Openingfrom"] != null && Request["Openingfrom"] == "MyorderQueue")
                {
                    btnpatientChart1.Visible = true;
                    btnFindAppointments.Visible = true;
                    btnePrescribe.Visible = true;
                    btnTask.Visible = true;
                    btnDeleteIndexing.Visible = false;
                    DelIndexDiv.Attributes.Remove("width");
                    
                    //btnpatientChart1.Style["display"] = none;

                    // btnTask.Style["margin-left"] = "-114px";

                    //btnSave.Style["margin-top"] = "-3px";
                    //btnSave.Style["margin-left"] = "29px";
                    //btnSave.Style["margin-right"] = "-49px";
                }

                if (Request["Result_OBR_Date"] != null && Request["Result_OBR_Date"] != "")
                {
                    sResult_obr_date = Request["Result_OBR_Date"].ToString();
                }

                if (Request["Description"] != null && Request["Description"] != "")
                {
                    sProcedureDescription = Request["Description"].ToString();
                }
                if (Request["OrderSubmitId"] != null && Request["OrderSubmitId"] != "")
                    uOrderID = Convert.ToUInt64(Request["OrderSubmitId"].ToString());
                

                DLC.DName = "pbDropDown";
                //DLC.txtDLC.Attributes.Add("onkeypress", "txtProviderNotes_OnKeyPress(event);");
                //DLC.txtDLC.Attributes.Add("onchange", "txtProviderNotes_OnKeyPress(event);");
                DLC.txtDLC.Attributes.Add("onkeypress", "txtProviderNotes_OnValueChanged(event);");
                DLC.txtDLC.Attributes.Add("onchange", "txtProviderNotes_OnValueChanged(event);");
                // btnSave.Attributes.Add("OnClientClick", "return btnSave_ClientClicked();");
                imgCopyPrevious.Attributes.Add("onclick", "OpenResultInterpretation();");
                Session["Status_Flag"] = null;
                if (Request["HumanID"] != null && Request["HumanID"] != "")
                {
                    human_id = Convert.ToUInt64(Request["HumanID"].ToString());
                }
                Session["human_id"] = human_id;





                Session["Result_Master_Id"] = 0;
                //Cap - 1704
                Session["Order_Id"] = 0;
                if (Request["OrderSubmitId"] != null && Request["OrderSubmitId"] != "")
                {
                    Session["Order_Id"] = Request["OrderSubmitId"];
                }                   

                #region Opening from Patient_Pane
                if (Request["Opening_from"] != null && Request["Opening_from"] == "Patient_Pane") //Opening from Left side Patient Pane
                {
                    if (Request["Doc_type"] != null)
                    {
                        Document_type = Request["Doc_type"].ToString();
                    }
                    if (Request["Key_id"] != null && Request["Key_id"] != "")
                    {
                        Key_id = Convert.ToUInt64(Request["Key_id"].ToString());
                    }
                    Session["Doc_type"] = Document_type;
                    Session["Key_id"] = Key_id;

                    btnPrintEducatnMaterial.Visible = true;

                    //btnSave.Style["margin-left"] = "308px";  // "444px";
                    //btnSave.Style["margin-top"] = "-56px";

                    //btnSave.Style["position"] = "absolute";
                    btnMoveToMa.Style["margin-left"] = "77px";
                    // btnMoveToMa.Style["margin-top"] = "-10px";
                    btnMoveToNextProcess.Style["margin-left"] = "66px";
                    // btnMoveToNextProcess.Style["margin-top"] = "-13px";
                    // btnTask.Style["margin-left"] = "418px"; //change
                    //btnEfax.Style["margin-left"] = "5px";
                    btnSave.Disabled = true;
                    //WFObjectManager WFObjMngr = new WFObjectManager();
                    //WFObject wfObj;

                    //if (Request["OrderSubmitId"] != null)
                    //{
                    //    wfObj = WFObjMngr.GetByObjectSystemIdIncludeArchive(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER");
                    //    hdnLeftPaneOrderSubmitID.Value = wfObj.Obj_System_Id.ToString();

                    //}
                    //else
                    //{
                    //    wfObj = WFObjMngr.GetByObjectSystemIdIncludeArchive(Key_id, "DIAGNOSTIC_RESULT");
                    //    hdnLeftPaneResultMasterID.Value = wfObj.Obj_System_Id.ToString();
                    //}

                    //if (wfObj.Current_Process == "RESULT_REVIEW" && wfObj.Current_Owner == ClientSession.UserName)
                    //{
                    //    hdnLeftPaneObjType.Value = wfObj.Obj_Type;
                    //    hdnLeftPaneCurrentProcess.Value = wfObj.Current_Process;

                    //    btnMoveToNextProcess.Visible = true;
                    //    btnMoveToMa.Visible = true;
                    //    chkShowAll.Visible = true;
                    //    cboMoveToMA.Visible = true;
                    //    Label1.Visible = true;
                    //    chkPhyName.Visible = true;
                    //    FillCombobox();
                    //    rdbMA.Checked = true;
                    //    string sTemp = string.Empty;
                    //    string[] sPhyname = null;
                    //    StaticLookupManager objstaticlookup = new StaticLookupManager();
                    //    IList<StaticLookup> StaticLst = objstaticlookup.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES");
                    //    IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
                    //    PhysicianManager objphysican = new PhysicianManager();

                    //    if (ClientSession.PhysicianId != 0)
                    //    {
                    //        PhysicianList = objphysican.GetphysiciannameByPhyID(Convert.ToUInt32(ClientSession.PhysicianId));
                    //        if (PhysicianList != null && PhysicianList.Count > 0)
                    //        {
                    //            if (StaticLst != null && StaticLst.Count > 0)
                    //            {
                    //                sPhyname = StaticLst[0].Value.Split('|');
                    //                if (sPhyname != null && sPhyname.Length > 1 && sPhyname[1].Trim() != string.Empty)
                    //                {
                    //                    sTemp = sPhyname[1].Replace("<Physician>", PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + " " + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix);
                    //                }
                    //            }

                    //        }
                    //        sTemp = sTemp.Replace("<Date>", "");
                    //        sTemp = sTemp.Replace(" on ", "");
                    //        chkPhyName.Text = sTemp;
                    //    }
                    //}
                    //else
                    //{
                    //    btnMoveToNextProcess.Visible = false;
                    //    btnMoveToMa.Visible = false;
                    //    chkShowAll.Visible = false;
                    //    cboMoveToMA.Visible = false;
                    //    Label1.Visible = false;
                    //    chkPhyName.Visible = false;
                    //    rdbMA.Visible = false;
                    //    rdbProvider.Visible = false;
                    //    btnSave.Style["margin-left"] = "370px";
                    //}

                    //btnSave.Visible = false;
                    //DLC.txtDLC.Enabled = false;
                    //txtMedicalAssistantNotes.Enabled = false;
                    //DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    //DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    //DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    //DLC.pbDropdown.Attributes.Remove("class");
                    //DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");

                    //txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    //txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    //txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");

                    btnSave.Visible = true;
                    if ((ClientSession.UserRole != null && ClientSession.UserRole.ToUpper().Trim() == "PHYSICIAN" || ClientSession.UserRole == "Physician Assistant"))
                    {
                        txtMedicalAssistantNotes.Enabled = false;
                        txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                        txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    }

                    if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper().Trim() == "MEDICAL ASSISTANT")
                    {
                        DLC.txtDLC.Enabled = false;
                        DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                        DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        DLC.pbDropdown.Attributes.Remove("class");
                        DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");
                    }

                    trPatInfo.Visible = false;//Added for BugID:45808
                    btnClose.Visible = false;
                    ConstructTreeView(human_id, Document_type, Key_id);
                }
                #endregion
                #region Opening from OrdersQ
                else if (Request["Opening_from"] != null && Request["Opening_from"] == "OrdersQ") //Opening from My Orders Q
                {
                    Document_type = "Results";

                    if (Request["ResultMasterID"] != null && Request["ResultMasterID"] != "")
                    {
                        Key_id = Convert.ToUInt64(Request["ResultMasterID"].ToString());
                    }
                    else if (Request["File_Ref_ID"] != null && Request["File_Ref_ID"] != "")
                    {
                        Key_id = Convert.ToUInt64(Request["File_Ref_ID"].ToString());
                    }
                    Session["Doc_type"] = Document_type;
                    Session["Key_id"] = Key_id;
                    //Cap - 686
                    if (Request["CurrentProcess"] != null)
                    {
                        hdncurrentProcess.Value = Request["CurrentProcess"].ToUpper().Trim();
                    }
                    if ((ClientSession.UserRole != null && ClientSession.UserRole.ToUpper().Trim() == "PHYSICIAN" || ClientSession.UserRole == "Physician Assistant") && Request["CurrentProcess"] != null && Request["CurrentProcess"].ToUpper().Trim() == "RESULT_REVIEW")
                    {
                        string sTemp = string.Empty;
                        //string[] sPhyname = null;
                        //StaticLookupManager objstaticlookup = new StaticLookupManager();
                        //IList<StaticLookup> StaticLst = objstaticlookup.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES");
                        IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
                        PhysicianManager objphysican = new PhysicianManager();
                        //if (Request["PhysicianId"] != null && Convert.ToUInt32(Request["PhysicianId"]) != 0)
                        if (ClientSession.PhysicianId != 0)
                        {
                            //Cap - 1169
                            //PhysicianList = objphysican.GetphysiciannameByPhyID(ClientSession.PhysicianId);
                            PhysicianList = objphysican.GetphysiciannameByUserName(ClientSession.UserName);
                            {
                                //if (StaticLst != null && StaticLst.Count > 0)
                                //{
                                //    sPhyname = StaticLst[0].Value.Split('|');
                                //    if (sPhyname != null && sPhyname.Length > 1 && sPhyname[1].Trim() != string.Empty)
                                //    {
                                //        sTemp = sPhyname[1].Replace("<Physician>", PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + " " + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix);
                                //    }
                                //}
                                sTemp = "Electronically Signed by " + PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + (string.IsNullOrEmpty(PhysicianList[0].PhyMiddleName) ? "" : " ") + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix;

                            }
                            //sTemp = sTemp.Replace("<Date>", "");
                            //sTemp = sTemp.Replace(" on ", "");
                            chkPhyName.Text = sTemp;
                        }
                        txtMedicalAssistantNotes.Enabled = false;
                        txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                        txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");

                        rdbMA.Checked = true;
                        btnClose.Style["margin-left"] = "15px";

                        FillCombobox();

                    }
                    if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper().Trim() == "MEDICAL ASSISTANT" && Request["CurrentProcess"] != null && Request["CurrentProcess"].ToUpper().Trim() == "MA_RESULTS")
                    {

                        btnMoveToMa.Text = "Save & Move To Provider";
                        chkShowAll.Visible = true;
                        //Jira CAP-2153
                        //cboMoveToMA.Visible = true;
                        txtAssignedTo.Visible = true;
                        imgclearAssignTo.Visible = true;
                        //Jira CAP-2153 - End
                        Label1.Visible = true;
                        rdbMA.Visible = false;
                        rdbProvider.Checked = true;
                        FillCombobox();
                        chkPhyName.Visible = false;
                        DLC.txtDLC.Enabled = false;
                        DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                        DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        DLC.pbDropdown.Attributes.Remove("class");
                        DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");

                    }

                    ConstructTreeView(human_id, Document_type, Key_id);
                    ResultMasterManager masterProxy = new ResultMasterManager();
                    if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")//to handle Diagnostic_results which do not have OrderID.//BugID:45807
                    {
                        PageViewResult.Height = 480;
                        tvViewIndex.Height = 432;
                    }
                    else
                    {
                        tvViewIndex.Height = 540;
                        PageViewResult.Height = 555;
                    }

                }
                #endregion
                #region Opening from Order Management Screen(CPOE-Order)
                if (Request["Opening_from"] != null && Request["Opening_from"] == "OrderManagementScreen") //Opening from Order Management Screen (Menu Level)
                {
                    Document_type = "Results";
                    if (Request["File_Ref_ID"] != null && Request["File_Ref_ID"] != null)
                        Key_id = Convert.ToUInt64(Request["File_Ref_ID"].ToString());

                    Session["Key_id"] = Key_id;
                    Session["Doc_type"] = Document_type;

                    string sTemp = string.Empty;
                    //string[] sPhyname = null;
                    //StaticLookupManager objstaticlookup = new StaticLookupManager();
                    //IList<StaticLookup> StaticLst = objstaticlookup.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES");
                    IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
                    PhysicianManager objphysican = new PhysicianManager();
                    //Cap - 1169
                    //if (Request["PhysicianId"] != null && Convert.ToUInt32(Request["PhysicianId"]) != 0)
                    //{                        
                    //PhysicianList = objphysican.GetphysiciannameByPhyID(Convert.ToUInt32(Request["PhysicianId"]));
                    PhysicianList = objphysican.GetphysiciannameByUserName(ClientSession.UserName);
                    if (PhysicianList != null && PhysicianList.Count > 0)
                        {
                            //if (StaticLst != null && StaticLst.Count > 0)
                            //{
                            //    sPhyname = StaticLst[0].Value.Split('|');
                            //    if (sPhyname != null && sPhyname.Length > 1 && sPhyname[1].Trim() != string.Empty)
                            //    {
                            //        sTemp = sPhyname[1].Replace("<Physician>", PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + " " + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix);
                            //    }
                            //}
                            sTemp = "Electronically Signed by " + PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + (string.IsNullOrEmpty(PhysicianList[0].PhyMiddleName) ? "" : " ") + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix;

                        }
                        //sTemp = sTemp.Replace("<Date>", "");
                        //sTemp = sTemp.Replace(" on ", "");
                        chkPhyName.Text = sTemp;
                    //}

                    ConstructTreeView(human_id, Document_type, Key_id);
                    btnPrintEducatnMaterial.Visible = true;
                    DLC.txtDLC.Enabled = false;
                    txtMedicalAssistantNotes.Enabled = false;
                    btnMoveToMa.Visible = false;
                    chkShowAll.Visible = false;
                    //Jira CAP-2153
                    //cboMoveToMA.Visible = false;
                    txtAssignedTo.Visible = false;
                    imgclearAssignTo.Visible = false;
                    //Jira CAP-2153 - End
                    Label1.Visible = false;
                    btnMoveToNextProcess.Visible = false;
                    btnSave.Visible = false;
                    chkPhyName.Visible = false;
                    rdbMA.Visible = false;
                    rdbProvider.Visible = false;
                    btnPrintEducatnMaterial.Visible = false;
                    btnEfax.Enabled = false;
                    btnClose.Style["margin-left"] = "41px";
                    DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    DLC.pbDropdown.Attributes.Remove("class");
                    DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");
                    txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    btnDeleteIndexing.Visible = false;
                    DelIndexDiv.Attributes.Remove("width");
                    //Jira CAP-2228
                    imgCopyPrevious.Visible = false;
                }
                #endregion
                #region Opening from OrdersList
                if (Request["Opening_from"] != null && Request["Opening_from"] == "OrdersList") //Opening from Diagnostic Order - Orders List
                {
                    Document_type = "Results";
                    Session["Doc_type"] = Document_type;
                    if (Request["OrderSubmitId"] != null && Request["OrderSubmitId"] != "")
                        Key_id = Convert.ToUInt64(Request["OrderSubmitId"].ToString());
                    Session["Key_id"] = Key_id;

                    DLC.txtDLC.Enabled = false;
                    txtMedicalAssistantNotes.Enabled = false;
                    btnMoveToMa.Visible = false;
                    chkShowAll.Visible = false;
                    //Jira CAP-2153
                    //cboMoveToMA.Visible = false;
                    txtAssignedTo.Visible = false;
                    imgclearAssignTo.Visible = false;
                    //Jira CAP-2153 - End
                    Label1.Visible = false;
                    btnMoveToNextProcess.Visible = false;
                    btnSave.Visible = true;//change save
                    btnSave.Disabled = true;
                    chkPhyName.Visible = false;
                    btnClose.Style["margin-left"] = "41px";
                    btnClose.Style["width"] = "60%";
                    //btnEfax.Style["margin-top"] = "-3px";
                    //btnEfax.Style["margin-left"] = "2px";
                    //btnTask.Style["margin-left"] = "143px";
                    //btnSave.Style["margin-top"] = "-46px";
                    //btnSave.Style["margin-left"] = "488px";
                    //btnClose.Style["margin-left"] = "-22px";
                    //btnClose.Style["margin-top"] = "-8px";
                    rdbMA.Visible = false;
                    rdbProvider.Visible = false;
                    // btnTask.Style["margin-left"] = "42%";
                    //btnSave.Style["margin-top"] = "-64%";  //"-90%";   //"-90%";
                    //btnSave.Style["margin-left"] = "321%";        //"387%";   //"652%";   // // "339%";

                    //btnTask.Style["margin-left"] = "2px";
                    //btnClose.Style["margin-left"] = "29px";

                    DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    DLC.pbDropdown.Attributes.Remove("class");
                    DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");
                    txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    ConstructTreeView(human_id, Document_type, Key_id);
                }
                #endregion
                //Cap - 1696
                btnSave.Disabled = true;
            }
            else
            {
                if (hdnSave.Value == "true")
                {
                    // btnSave.Enabled = true;
                    btnSave.Disabled = false;
                }
            }
            // btnSave.Click += new EventHandler(btnSave_Click);
            //btnSave.Attributes.Add("onclick", "btnSave_ClientClick();");
            #region Populate Patient Info
            try
            {
                string sPatientstrip = UtilityManager.FillPatientStrip(human_id);
                if (sPatientstrip != null)
                {
                    txtPatientInformation.Value = sPatientstrip;
                    //Cap - 1193
                    hdnHumanText.Value = sPatientstrip;
                }
                //Cap - 2289
                if (txtMedicalAssistantNotes.Enabled == true)
                {
                    imgCopyPrevious.Visible = false;
                }
            }
            catch (Exception Ex)
            {
                //XmlText.Close();
                // ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Regeneratexml", "RegenerateXML('" + human_id.ToString() + "','Human','result');", true);
                return;

                // UtilityManager.GenerateXML(human_id.ToString(), "Human");

                //goto retry;
            }
        ln:
            try
            {
                IList<string> ilstHumanTag = new List<string>();
                ilstHumanTag.Add("HumanList");

                IList<object> ilstHumanBlobList = new List<object>();
                ilstHumanBlobList = UtilityManager.ReadBlob(human_id, ilstHumanTag);

                Human objFillHuman = new Human();

                if (ilstHumanBlobList != null && ilstHumanBlobList.Count > 0)
                {
                    if (ilstHumanBlobList[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobList[0]).Count; iCount++)
                        {
                            objFillHuman = ((Human)((IList<object>)ilstHumanBlobList[0])[iCount]);
                            hdnHumanId.Value = Convert.ToString(objFillHuman.Id);
                            hdnDOB.Value = Convert.ToDateTime(objFillHuman.Birth_Date).ToString();
                            hdnPatientfirstname.Value = objFillHuman.First_Name;
                            hdnPatientlastname.Value = objFillHuman.Last_Name;
                            hdnPatientmiddlename.Value = objFillHuman.MI;
                            hdnPatientType.Value = objFillHuman.Human_Type;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //XmlText.Close();
                //Thread.Sleep(5000);
                UtilityManager.GenerateXML(human_id.ToString(), "Human");

                goto ln;
            }

            //string FileName = "Human" + "_" + human_id + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{

            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    try
            //    {
            //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //        {
            //            itemDoc.Load(fs);

            //            XmlNodeList xmlhumanList = itemDoc.GetElementsByTagName("Human");
            //            Human objFillHuman = new Human();
            //            IList<Human> lstHuman = new List<Human>();
            //            if (xmlhumanList != null && xmlhumanList.Count > 0)
            //            {
            //                objFillHuman.Id = Convert.ToUInt64(xmlhumanList[0].Attributes.GetNamedItem("Id").Value);
            //                hdnHumanId.Value = Convert.ToString(objFillHuman.Id);
            //                objFillHuman.Birth_Date = Convert.ToDateTime(xmlhumanList[0].Attributes.GetNamedItem("Birth_Date").Value);
            //                hdnDOB.Value = Convert.ToDateTime(objFillHuman.Birth_Date).ToString();
            //                objFillHuman.First_Name = xmlhumanList[0].Attributes.GetNamedItem("First_Name").Value;
            //                hdnPatientfirstname.Value = objFillHuman.First_Name;
            //                objFillHuman.Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Last_Name").Value;
            //                hdnPatientlastname.Value = objFillHuman.Last_Name;
            //                objFillHuman.MI = xmlhumanList[0].Attributes.GetNamedItem("MI").Value;
            //                hdnPatientmiddlename.Value = objFillHuman.MI;
            //                objFillHuman.Sex = xmlhumanList[0].Attributes.GetNamedItem("Sex").Value;
            //                objFillHuman.Suffix = xmlhumanList[0].Attributes.GetNamedItem("Suffix").Value;
            //                objFillHuman.Medical_Record_Number = xmlhumanList[0].Attributes.GetNamedItem("Medical_Record_Number").Value;
            //                objFillHuman.Home_Phone_No = xmlhumanList[0].Attributes.GetNamedItem("Home_Phone_No").Value;
            //                objFillHuman.Human_Type = xmlhumanList[0].Attributes.GetNamedItem("Human_Type").Value;
            //                hdnPatientType.Value = objFillHuman.Human_Type;
            //                objFillHuman.Patient_Account_External = xmlhumanList[0].Attributes.GetNamedItem("Patient_Account_External").Value;
            //                //objFillHuman.Home_Phone_No = xmlhumanList[0].Attributes.GetNamedItem("Cell_Phone_Number").Value;
            //                objFillHuman.Cell_Phone_Number = xmlhumanList[0].Attributes.GetNamedItem("Cell_Phone_Number").Value;

            //                lstHuman.Add(objFillHuman);
            //            }

            //            string phoneno = "";
            //            if (lstHuman != null && lstHuman.Count > 0)
            //            {
            //                if (objFillHuman.Home_Phone_No.Length == 14)
            //                {
            //                    phoneno = objFillHuman.Home_Phone_No;
            //                }
            //                else
            //                {
            //                    phoneno = objFillHuman.Cell_Phone_Number;
            //                }

            //            }

            //            if (lstHuman != null && lstHuman.Count > 0)
            //            {
            //                string sSex = string.Empty;
            //                if (objFillHuman.Sex != null && objFillHuman.Sex.Trim() != "")
            //                    sSex = objFillHuman.Sex.Substring(0, 1);

            //                txtPatientInformation.Value = objFillHuman.Last_Name + "," + objFillHuman.First_Name +
            //           " " + objFillHuman.MI + " " + objFillHuman.Suffix + " | " +
            //            objFillHuman.Birth_Date.ToString("dd-MMM-yyyy") + " | " +
            //           (CalculateAge(objFillHuman.Birth_Date)).ToString() +
            //           " year(s) | " + sSex + " | Acc #:" + objFillHuman.Id +
            //           " | " + "Med Rec #:" + objFillHuman.Medical_Record_Number + " | " +
            //           "Phone #:" + phoneno + " | Patient Type:" + objFillHuman.Human_Type;
            //            }
            //            fs.Close();
            //            fs.Dispose();
            //        }
            //    }
            //    catch (Exception Ex)
            //    {
            //        XmlText.Close();
            //        // ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
            //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Regeneratexml", "RegenerateXML('" + human_id.ToString() + "','Human','result');", true);
            //        return;

            //        // UtilityManager.GenerateXML(human_id.ToString(), "Human");

            //        //goto retry;
            //    }
            //}
            #endregion
            if (DLC.txtDLC != null && DLC.txtDLC.Text != string.Empty && DLC.txtDLC.Text.Contains("Test Reviewed:"))
            {
                DLC.txtDLC.Enabled = false;
                DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                DLC.pbDropdown.Attributes.Remove("class");
                DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");
            }



            if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y" && ClientSession.UserPermissionDTO != null && ClientSession.UserPermissionDTO.Userscntab != null)
            {
                var idname = from u in ClientSession.UserPermissionDTO.Userscntab where u.scn_id == Convert.ToUInt64("101113") select u;
                if (idname.ToList().Count > 0 && idname.ToList()[0].Permission == "R")
                    btnEfax.Enabled = false;
                else
                    btnEfax.Enabled = true;
            }
            else
                btnEfax.Enabled = false;

            FillAkidoInterpretationNoteButton(uOrderID.ToString());
        }
        private string ConstructTreeView(ulong humanId, string Doc_type, ulong KeyID)
        {
            loadlistforResult();
            IList<Result_Files_List> result_files = new List<Result_Files_List>();
            LoadTreeViewData(out result_files);
            LoadTreeView(result_files);
            Session["Scan_Index"] = null;
            Session["Scan_Index"] = "INDEX_SCREEN";
            if (Session["Key_id"] != null)
                HighlightSelectedItem(Convert.ToUInt64(Session["Key_id"]));
            return string.Empty;
        }

        public int CalculateAge(DateTime birthDate)
        {
            DateTime now = DateTime.Today;
            int years = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;
            return years;
        }
        public void hdnbtngeneratexml_Click(object sender, EventArgs e)
        {

            //  Patientchartload();
        }
        public void loadlistforResult()
        {
            FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
            FileManagementDTO objdto = new FileManagementDTO();
            //objdto = objfileproxy.GetResultandExamListusing(Convert.ToUInt64(Session["human_id"]));
            //For Bug Id:53929 If matching patient id mismatch also v get the order using order id.
            objdto = objfileproxy.GetResultandExamListusing(Convert.ToUInt64(Session["human_id"]), uOrderID);
            IList<FileManagementIndex> ilstscaninindex = new List<FileManagementIndex>();
            IList<ResultMaster> ilstresultmaster = new List<ResultMaster>();
            IList<FileManagementIndex> filelist = new List<FileManagementIndex>();
            IList<ResultMaster> result_master_list = new List<ResultMaster>();
            filelist = objdto.FileManagementList;
            result_master_list = objdto.ResultMasterList;
            Session["fileList"] = filelist;
            Session["result_master_list"] = result_master_list;

            //For Bug ID : 65667 
            //Check if the selected line item procedure description = Paper Order [QueryString]
            //If so
            //{
            //Check if objdto.file management index does not have the line item for the selected orders submit id [Session["OrdersSubmitID"]
            //If not present
            //{
            //Move the file from scan_index_conversion to FTP
            //Add a line item in the memory variable objdto.FileManagementIndex
            //}
            //}

            if (sProcedureDescription != null && sProcedureDescription != string.Empty && sProcedureDescription.ToLower().Replace(" ", "").Contains("paperorder"))
            {
                IList<FileManagementIndex> IndexList = new List<FileManagementIndex>();
                IndexList = objdto.FileManagementList.Where(f => f.Order_ID == Convert.ToUInt32(uOrderID)).ToList<FileManagementIndex>();
                if (IndexList == null || IndexList.Count == 0)
                {
                    Scan_IndexManager objscanmngr = new Scan_IndexManager();
                    IList<scan_index> lstScanIndex = new List<scan_index>();
                    lstScanIndex = objscanmngr.GetScanIndexDetailsForOrderID(uOrderID);
                    if (lstScanIndex != null && lstScanIndex.Count() > 0)
                    {
                        string ftpServerIP = ConfigurationManager.AppSettings["ftpServerIP"];
                        string ftpUserName = ConfigurationManager.AppSettings["ftpUserID"];
                        string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
                        FTPImageProcess _ftpImageProcess = new FTPImageProcess();
                        string serverPath = string.Empty;

                        //if (_ftpImageProcess.CreateDirectory(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserName, ftpPassword))
                        bool bCreateDirectory = _ftpImageProcess.CreateDirectory(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserName, ftpPassword,out string sCheckFileNotFoundException);
                        if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                            return;
                        }
                        if (bCreateDirectory)
                        {
                            for (int i = 0; i < lstScanIndex.Count; i++)
                            {
                                serverPath = _ftpImageProcess.UploadToImageServer(lstScanIndex[i].Human_ID.ToString(), ftpServerIP, ftpUserName, ftpPassword, Server.MapPath(lstScanIndex[i].Indexed_File_Path), string.Empty, out string sCheckFileNotFoundExceptionses);
                                if (sCheckFileNotFoundExceptionses != "" && sCheckFileNotFoundExceptionses.Contains("CheckFileNotFoundException"))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExceptionses.Split('~')[1] + "\");", true);
                                    return;
                                }
                                if (serverPath != string.Empty)
                                {
                                    FileManagementIndex filemanagementIndex = new FileManagementIndex();
                                    filemanagementIndex.Created_By = lstScanIndex[i].Created_By;
                                    filemanagementIndex.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                    filemanagementIndex.Document_Date = lstScanIndex[i].Document_Date;
                                    filemanagementIndex.Document_Type = lstScanIndex[i].Document_Type;
                                    filemanagementIndex.Document_Sub_Type = lstScanIndex[i].Document_Sub_Type;
                                    filemanagementIndex.Source = "SCAN";
                                    filemanagementIndex.Human_ID = lstScanIndex[i].Human_ID;
                                    filemanagementIndex.Order_ID = lstScanIndex[i].Order_ID;
                                    filemanagementIndex.Scan_Index_Conversion_ID = lstScanIndex[i].Id;
                                    filemanagementIndex.File_Path = serverPath;
                                    filemanagementIndex.Encounter_ID = lstScanIndex[i].Encounter_ID;
                                    objdto.FileManagementList.Add(filemanagementIndex);

                                }
                            }
                        }
                    }
                }
            }

            //End Bug ID : 65667 


            if (result_master_list != null && result_master_list.Count > 0)
            {
                ulong order_submit_id = 0;
                for (int i = 0; i < result_master_list.Count; i++)
                {
                    if (result_master_list[i].Order_ID != 0)
                    {
                        if (result_master_list[i].Order_ID != order_submit_id)
                        {
                            ilstresultmaster.Add(result_master_list[i]);
                            order_submit_id = result_master_list[i].Order_ID;
                        }
                    }
                    else
                    {
                        ilstresultmaster.Add(result_master_list[i]);
                        order_submit_id = result_master_list[i].Order_ID;
                    }

                }
            }


            if (ilstresultmaster.Count > 0 && filelist != null && filelist.Count > 0)
            {
                ilstscaninindex = new List<FileManagementIndex>();
                for (int i = 0; i < filelist.Count; i++)
                {
                    bool blScan = false;
                    for (int j = 0; j < ilstresultmaster.Count; j++)
                    {
                        if (ilstresultmaster[j].Order_ID != 0)
                        {
                            if (filelist[i].Order_ID == ilstresultmaster[j].Order_ID)
                            {
                                ilstresultmaster[j].Is_Scan_order = "Y";
                                blScan = true;
                                break;
                            }
                        }
                    }
                    //if (blScan == false)//BugID:49161
                    //{
                    if (filelist[i].Order_ID == 0 && filelist[i].Document_Sub_Type != "Lab Result Form")
                    {
                        ilstscaninindex.Add(filelist[i]);
                    }
                    else if (filelist[i].Order_ID != 0)
                    {
                        ilstscaninindex.Add(filelist[i]);
                    }

                    //}
                }
            }

            else if ((ilstresultmaster.Count == 0 && filelist != null && filelist.Count > 0))
            {
                for (int i = 0; i < filelist.Count; i++)
                {
                    ilstscaninindex.Add(filelist[i]);
                }
            }

            if (ilstresultmaster != null && ilstresultmaster.Count > 0)
            {
                ilstresultmaster = ilstresultmaster.OrderByDescending(a => a.Created_Date_And_Time).ToList<ResultMaster>();
            }
            if (ilstscaninindex != null && ilstscaninindex.Count > 0)
            {
                ilstscaninindex = ilstscaninindex.OrderByDescending(a => a.Order_ID).ToList<FileManagementIndex>();
            }
            Session["ilstscanindex"] = ilstscaninindex;
            Session["ilstresultmaster"] = ilstresultmaster;
        }
        private IList<Result_Files_List> LoadTreeViewData(out IList<Result_Files_List> ResultLists)
        {
            IList<FileManagementIndex> ilstscaninindex = new List<FileManagementIndex>();
            IList<ResultMaster> ilstresultmaster = new List<ResultMaster>();
            ilstscaninindex = (IList<FileManagementIndex>)Session["ilstscanindex"];
            ilstresultmaster = (IList<ResultMaster>)Session["ilstresultmaster"];
            ResultLists = new List<Result_Files_List>();

            IList<string> DocTypes = new List<string>();
            if (ilstscaninindex != null)
            {
                for (int i = 0; i < ilstscaninindex.Count; i++)
                {
                    Result_Files_List objresult = new Result_Files_List();
                    if (ilstscaninindex[i].Order_ID != 0)
                    {
                        //OrdersSubmit ordersList = new OrdersSubmit();
                        //OrdersSubmitManager OsManager = new OrdersSubmitManager();
                        //ordersList = OsManager.GetById(ilstscaninindex[i].Order_ID);
                        //if (ordersList != null && ordersList.Id != 0)
                        //{
                        //    if (ordersList.Bill_Type.Trim() != string.Empty) //BugID:42001 ,41884
                        //    {
                        //        objresult.Result_Date = ordersList.Created_Date_And_Time;//Results entered from diagnostic_order screen
                        //    }
                        //    else
                        //    {
                        //        objresult.Result_Date = ilstscaninindex[i].Document_Date;//Results uploaded from upload scan documents screen
                        //    }
                        //}
                        //else
                        //{
                        objresult.Result_Date = ilstscaninindex[i].Document_Date;
                        //}
                    }
                    else
                    {
                        objresult.Result_Date = ilstscaninindex[i].Document_Date;
                    }
                    objresult.Result_Id = ilstscaninindex[i].Result_Master_ID;
                    objresult.Img_Path = ilstscaninindex[i].File_Path;
                    objresult.Doc_Type = ilstscaninindex[i].Document_Type;
                    objresult.Sub_Doc_Type = ilstscaninindex[i].Document_Sub_Type;
                    objresult.Order_Submit_Id = ilstscaninindex[i].Order_ID;
                    objresult.Ord_Type = ilstscaninindex[i].Document_Type;
                    objresult.Order_Description = ilstscaninindex[i].Orders_Description;
                    objresult.IndexId = ilstscaninindex[i].Id;
                    //if (Session["Doc_type"] != null && (ilstscaninindex[i].Document_Type.ToUpper().Trim() == Session["Doc_type"].ToString().ToUpper().Trim())) //For Bug ID 70801,70802
                    if (Session["Doc_type"] != null && (ilstscaninindex[i].Document_Type.ToUpper().Trim() == Session["Doc_type"].ToString().ToUpper().Trim()) && (ilstscaninindex[i].Source.ToUpper().Trim() != "EXAM"))
                    {
                        ResultLists.Add(objresult);
                    }
                    //if ((DocTypes.IndexOf(ilstscaninindex[i].Document_Type) == -1) && (ilstscaninindex[i].Document_Type.Trim() != string.Empty)) //For Bug ID 70801,70802
                    if ((DocTypes.IndexOf(ilstscaninindex[i].Document_Type) == -1) && (ilstscaninindex[i].Document_Type.Trim() != string.Empty) && (ilstscaninindex[i].Source.ToUpper().Trim() != "EXAM"))
                    {
                        DocTypes.Add(ilstscaninindex[i].Document_Type);
                    }
                }
            }


            if (Session["Doc_type"] != null && Session["Doc_type"].ToString().ToUpper().Trim() == "RESULTS")
            {
                if (ilstresultmaster != null)
                {
                    for (int i = 0; i < ilstresultmaster.Count; i++)
                    {
                        if (ilstresultmaster[i].OBR_Specimen_Collected_Date_And_Time != string.Empty)
                        {
                            Result_Files_List objresult = new Result_Files_List();
                            string formatString = "yyyyMMddHHmmss";
                            string sample = ilstresultmaster[i].OBR_Specimen_Collected_Date_And_Time;
                            if (sample.Contains('-'))
                            {
                                sample = sample.Split('-')[0].ToString();
                            }
                            if (sample.Length == 14)
                                formatString = "yyyyMMddHHmmss";
                            else if (sample.Length == 12)
                                formatString = "yyyyMMddHHmm";

                            DateTime dt = DateTime.ParseExact(sample, formatString, null);

                            objresult.Result_Date = dt;
                            objresult.Result_Id = ilstresultmaster[i].Id;
                            objresult.IndexId = 0;
                            objresult.Doc_Type = "Results";
                            string sub_doc = "Laboratory";
                            if (ilstresultmaster[i].Orders_Description.Split('-')[0].Trim().ToUpper() == "Patient Discharge Summary".Trim().ToUpper())
                                sub_doc = "Patient Discharge Summary";
                            else if (ilstresultmaster[i].Orders_Description.Split('-')[0].Trim().ToUpper() == "Patient Clinical Summary".Trim().ToUpper())
                                sub_doc = "Patient Clinical Summary";
                            else if (ilstresultmaster[i].Orders_Description.Split('-')[0].Trim().ToUpper() == "Pat Edu".Trim().ToUpper())
                                sub_doc = "Patient Education";
                            objresult.Sub_Doc_Type = sub_doc;
                            objresult.Img_Path = string.Empty;
                            objresult.Order_Submit_Id = ilstresultmaster[i].Order_ID;
                            objresult.Ord_Type = ilstresultmaster[i].Order_Type;
                            objresult.Order_Description = ilstresultmaster[i].Orders_Description;
                            ResultLists.Add(objresult);
                            if (DocTypes.IndexOf("Results") == -1)
                            {
                                DocTypes.Add("Results");
                            }
                        }
                    }
                }
            }
            if (DocTypes != null)
            {
                for (int i = 0; i < DocTypes.Count; i++)
                {
                    RadComboBoxItem tempItem = new RadComboBoxItem();
                    tempItem.Text = DocTypes[i];
                    tempItem.ToolTip = DocTypes[i];
                    if (cboDocumentType.Items.FindItemByText(DocTypes[i]) == null)
                        cboDocumentType.Items.Add(tempItem);
                }
            }

            if (Session["Doc_type"] != null && cboDocumentType.Items.Count > 0 && Session["Doc_type"].ToString() != "")
            {
                if (cboDocumentType.FindItemByText((string)Session["Doc_type"].ToString()) != null)//BugID:48326
                    cboDocumentType.FindItemByText((string)Session["Doc_type"].ToString()).Selected = true;
            }
            return ResultLists;
        }
        private void LoadTreeView(IList<Result_Files_List> Res_files)
        {
            tvViewIndex.Nodes.Clear();
            RadTreeNode tnMainNode = new RadTreeNode();
            tnMainNode.Text = Session["Doc_type"].ToString();
            tvViewIndex.Nodes.Add(tnMainNode);
            IList<string> SubNodelst = new List<string>();
            Res_files = Res_files.OrderByDescending(a => a._Res_Date).ToList<Result_Files_List>();
            if (Session["Doc_type"] != null && Session["Doc_type"].ToString().ToUpper().Trim() == "RESULTS")
            {
                SubNodelst = (from obj in Res_files select obj.Sub_Doc_Type.ToString()).Distinct().ToList<string>();
                foreach (string s in SubNodelst)
                {
                    RadTreeNode tnSubNode = new RadTreeNode();
                    tnSubNode.Text = s;
                    tnMainNode.Nodes.Add(tnSubNode);
                    foreach (Result_Files_List rs in Res_files.Where(a => a.Sub_Doc_Type == s))
                    {
                        RadTreeNode tnChildNode = new RadTreeNode();
                        if (rs.Sub_Doc_Type == "Laboratory")
                        {
                            tnChildNode.Text = rs.Result_Date.ToString("dd-MMM-yyyy hh:mm tt");
                            tnChildNode.Text = tnChildNode.Text.Replace("07:30 AM", "00:00 AM");//For Bug Id: 59576
                        }
                        else
                        {
                            tnChildNode.Text = UtilityManager.ConvertToLocal(rs.Result_Date).ToString("dd-MMM-yyyy hh:mm tt");
                            tnChildNode.Text = tnChildNode.Text.Replace("07:30 AM", "00:00 AM"); //For Bug Id: 59576
                        }

                        if (rs.Img_Path == string.Empty)
                            tnChildNode.Target = rs.Order_Submit_Id.ToString();
                        else
                            tnChildNode.Target = rs.Img_Path;
                        if (rs.IndexId == 0)
                            tnChildNode.Value = rs.Result_Id.ToString();
                        else
                            tnChildNode.Value = rs.IndexId.ToString();
                        if (rs.Order_Submit_Id != 0)
                        {
                            tnChildNode.Attributes.Add("OrderSubmitID", rs.Order_Submit_Id.ToString());
                        }
                        //CAP-2154
                        if (rs.Result_Id != 0)
                        {
                            tnChildNode.Attributes.Add("ResultMasterID", rs.Result_Id.ToString());
                        }
                        else
                        {
                            tnChildNode.Attributes.Add("OrderSubmitID", rs.Order_Submit_Id.ToString());
                        }
                        tnSubNode.Nodes.Add(tnChildNode);
                    }
                }
            }
            else
            {
                SubNodelst = (from obj in Res_files where obj.Img_Path != string.Empty select obj.Result_Date.ToString("dd-MMM-yyyy").ToString()).Distinct().ToList<string>();
                foreach (string s in SubNodelst)
                {
                    RadTreeNode tnSubNode = new RadTreeNode();
                    tnSubNode.Text = s;
                    tnMainNode.Nodes.Add(tnSubNode);
                    foreach (Result_Files_List rs in Res_files.Where(a => a.Result_Date.ToString("dd-MMM-yyyy") == s))
                    {
                        string File_Name = rs.Img_Path.Substring(rs.Img_Path.LastIndexOf("/") + 1);
                        string file = File_Name.Substring(File_Name.LastIndexOf("_") + 1);
                        string[] temp_name = file.Split('.');
                        string filename = rs.Sub_Doc_Type + "_" + temp_name[0];

                        RadTreeNode tnChildNode = new RadTreeNode();
                        tnChildNode.Text = filename;
                        tnChildNode.Target = rs.Img_Path;
                        tnChildNode.Value = rs.IndexId.ToString();
                        if (rs.Order_Submit_Id != 0)
                        {
                            tnChildNode.Attributes.Add("OrderSubmitID", rs.Order_Submit_Id.ToString());
                        }
                        //CAP-2154
                        if (rs.Result_Id != 0)
                        {
                            tnChildNode.Attributes.Add("ResultMasterID", rs.Result_Id.ToString());
                        }
                        else
                        {
                            tnChildNode.Attributes.Add("OrderSubmitID", rs.Order_Submit_Id.ToString());
                        }
                        tnSubNode.Nodes.Add(tnChildNode);
                    }
                }
            }

            tvViewIndex.ExpandAllNodes();

        }
        private void HighlightSelectedItem(ulong keyID)
        {
            string Matching_ItemID = string.Empty;
            bool node_found = false;
            if (!IsPostBack && Request["Opening_from"] != null && ((Request["Opening_from"] == "OrdersQ" && Request["ObjType"] != "DIAGNOSTIC_RESULT" && keyID == 0) || (Request["Opening_from"] == "OrderManagementScreen") || (Request["Opening_from"] == "OrdersList")))
            {
                if (Request["OrderSubmitId"] != null)
                    Matching_ItemID = Request["OrderSubmitId"];
                Session["OpenedItemID"] = Matching_ItemID;// BugID:45807
                for (int i = 0; i < tvViewIndex.Nodes[0].Nodes.Count; i++)
                {
                    for (int j = 0; j < tvViewIndex.Nodes[0].Nodes[i].Nodes.Count; j++)
                    {
                        foreach (string key in tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes.Keys)
                        {
                            if (tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes[key] == Matching_ItemID)
                            {
                                tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Selected = true;
                                if (DLC.txtDLC.Enabled == true)
                                {
                                    if (sNotes != null && sNotes.Count() > 0 && (sNotes.Contains(tvViewIndex.Nodes[0].Nodes[i].Nodes[j].ParentNode.Text.ToUpper())))
                                    {
                                        hdnSubDocumentType.Value = tvViewIndex.Nodes[0].Nodes[i].Nodes[j].ParentNode.Text.ToUpper();
                                        //Jira Cap - 2228
                                        if (Request["Opening_from"] != null && Request["Opening_from"] != "OrderManagementScreen")
                                        {
                                            imgCopyPrevious.Visible = true;
                                        }
                                        DLC.txtDLC.Enabled = true;
                                        //DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                                        //DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                                        //DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                                        //DLC.pbDropdown.Attributes.Remove("class");
                                        //DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");
                                        DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("White");
                                        DLC.pbDropdown.Attributes.Remove("class");
                                        DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackground");
                                        imgPrintNotes.Visible = true;
                                    }
                                    //else
                                    //{
                                    //    imgCopyPrevious.Visible = false;
                                    //    //DLC.txtDLC.Enabled = true;
                                    //    //DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("White");
                                    //    //DLC.pbDropdown.Attributes.Remove("class");
                                    //    //DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackground");
                                    //    imgPrintNotes.Visible = false;
                                    //}
                                }
                                else
                                {
                                    hdnSubDocumentType.Value = "";
                                    imgCopyPrevious.Visible = false;
                                    //MA process 
                                    if (sNotes != null && sNotes.Count() > 0 && (sNotes.Contains(tvViewIndex.Nodes[0].Nodes[i].Nodes[j].ParentNode.Text.ToUpper())))
                                    {
                                        imgPrintNotes.Visible = true;
                                    }
                                    else
                                        imgPrintNotes.Visible = false;

                                }
                                TabLoad(tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Target, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes["OrderSubmitID"], "OrderSubmitID");//added OrderSubmitID in list for BugID:45708



                                node_found = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                Matching_ItemID = keyID.ToString();
                Session["OpenedItemID"] = Matching_ItemID; // BugID:45807
                for (int i = 0; i < tvViewIndex.Nodes[0].Nodes.Count; i++)
                {
                    for (int j = 0; j < tvViewIndex.Nodes[0].Nodes[i].Nodes.Count; j++)
                    {
                        if (tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Value == Matching_ItemID)
                        {
                            tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Selected = true;
                            if (DLC.txtDLC.Enabled == true)
                            {
                                if (sNotes != null && sNotes.Count() > 0 && (sNotes.Contains(tvViewIndex.Nodes[0].Nodes[i].Nodes[j].ParentNode.Text.ToUpper())))
                                {
                                    hdnSubDocumentType.Value = tvViewIndex.Nodes[0].Nodes[i].Nodes[j].ParentNode.Text.ToUpper();
                                    //Jira Cap - 2228
                                    if (Request["Opening_from"] != null && Request["Opening_from"] != "OrderManagementScreen")
                                    {
                                        imgCopyPrevious.Visible = true;
                                    }
                                    //DLC.txtDLC.Enabled = false;
                                    //DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                                    //DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                                    //DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                                    //DLC.pbDropdown.Attributes.Remove("class");
                                    //DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");
                                    DLC.txtDLC.Enabled = true;
                                    DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("White");
                                    DLC.pbDropdown.Attributes.Remove("class");
                                    DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackground");
                                    imgPrintNotes.Visible = true;
                                }
                                //else
                                //{
                                //    imgCopyPrevious.Visible = false;
                                //    //DLC.txtDLC.Enabled = true;
                                //    //DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("White");
                                //    //DLC.pbDropdown.Attributes.Remove("class");
                                //    //DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackground");
                                //    imgPrintNotes.Visible = false;
                                //}
                            }
                            else
                            {
                                hdnSubDocumentType.Value = tvViewIndex.Nodes[0].Nodes[i].Nodes[j].ParentNode.Text.ToUpper();
                                imgCopyPrevious.Visible = false;
                                if (sNotes != null && sNotes.Count() > 0 && (sNotes.Contains(tvViewIndex.Nodes[0].Nodes[i].Nodes[j].ParentNode.Text.ToUpper())))
                                    imgPrintNotes.Visible = true;
                                else
                                    imgPrintNotes.Visible = false;
                            }
                            if (Convert.ToUInt64(tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes["OrderSubmitID"]) != 0)
                            {
                                TabLoad(tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Target, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes["OrderSubmitID"], "OrderSubmitID");//added OrderSubmitID in list for BugID:45708
                            }
                            else if (Convert.ToUInt64(tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes["ResultMasterID"]) != 0)
                            {
                                TabLoad(tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Target, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes["ResultMasterID"], "ResultMasterID");//added OrderSubmitID in list for BugID:45708
                                Session["Result_Master_Id"] = tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes["ResultMasterID"].ToString();
                            }
                            else
                            {
                                TabLoad(tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Target, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes["ResultMasterID"], string.Empty);//added OrderSubmitID in list for BugID:45708
                            }

                            //TabLoad(tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Target, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes["OrderSubmitID"]);//added OrderSubmitID in list for BugID:45708
                            node_found = true;
                            break;
                        }
                    }
                }
            }
            if (!node_found)
            {
                tabView.Tabs.Clear();
                PageViewScan.ContentUrl = string.Empty;
                PageViewResult.ContentUrl = string.Empty;
                PageViewResultFiles.ContentUrl = string.Empty;
                PageViewSpirometryResults.ContentUrl = string.Empty;
                PageViewABIResults.ContentUrl = string.Empty;
                PageViewMessageLog.ContentUrl = string.Empty;
                txtFileInformation.Value = "";
                //BugID:46221
                DLC.txtDLC.Enabled = false;
                DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                DLC.pbDropdown.Attributes.Remove("class");
                DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");
                txtMedicalAssistantNotes.Enabled = false;
                txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                txtProvNoteshistory.Enabled = false;
                txtMedNoteshistory.Enabled = false;
                btnSave.Visible = true;
                btnMoveToMa.Enabled = false;
                btnMoveToNextProcess.Enabled = false;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "wrong patient order mapping", @"<script type='text/javascript'>alert('Result is matched to the wrong patient.Please contact support.');</script>");//BugID:48326
            }

        }

        public void TabLoad(string KeyValue, string TargetValue, string OrderSubID, string IDName)//added OrderSubmitID in list for BugID:45708
        {
            //CAP-2478, CAP-2485
            string NewOrderSubmitID = OrderSubID;
            //Session["Order_Id"] = 0;
            Session["Notes"] = null;//BugID:46316- by default its given a value null.
            ulong order_id = 0;
            string sfileExtension = (Path.GetExtension(TargetValue.ToString())).ToString();
            hdnpath.Value = TargetValue + "~";
            // hdnfileindexid.Value = KeyValue;
            txtMedicalAssistantNotes.Text = string.Empty;
            DLC.txtDLC.Text = string.Empty;
            IList<FileManagementIndex> filelist = new List<FileManagementIndex>();
            IList<ResultMaster> result_master_list = new List<ResultMaster>();
            if (Session["filelist"] != null)
                filelist = (IList<FileManagementIndex>)Session["filelist"];

            if (Session["filelist"] != null)
                result_master_list = (IList<ResultMaster>)Session["result_master_list"];

            //Added for Enabling controls only for that particular item opened from Q.Start BugID:45807
            if (Request["Opening_from"] != null && Request["Opening_from"] == "OrdersQ")
            {
                if (Session["OpenedItemID"] != null && (Session["OpenedItemID"].ToString() == KeyValue || Session["OpenedItemID"].ToString() == OrderSubID))
                {
                    if ((ClientSession.UserRole != null && ClientSession.UserRole.ToUpper().Trim() == "PHYSICIAN" || ClientSession.UserRole == "Physician Assistant") && Request["CurrentProcess"] != null && Request["CurrentProcess"].ToUpper().Trim() == "RESULT_REVIEW")
                    {
                        txtMedicalAssistantNotes.Enabled = false;
                        txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                        txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        DLC.txtDLC.Enabled = true;
                        DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("White");
                        DLC.pbDropdown.Attributes.Remove("class");
                        DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackground");
                        chkPhyName.Visible = true;
                    }
                    if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper().Trim() == "MEDICAL ASSISTANT" && Request["CurrentProcess"] != null && Request["CurrentProcess"].ToUpper().Trim() == "MA_RESULTS")
                    {
                        DLC.txtDLC.Enabled = false;
                        DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                        DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        DLC.pbDropdown.Attributes.Remove("class");
                        DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");
                        txtMedicalAssistantNotes.Enabled = true;
                        txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("White");
                        chkPhyName.Visible = false;
                    }
                    btnMoveToNextProcess.Visible = true;
                    btnMoveToMa.Visible = true;
                    chkShowAll.Visible = true;
                    //Jira CAP-2153
                    //cboMoveToMA.Visible = true;
                    txtAssignedTo.Visible = true;
                    imgclearAssignTo.Visible = true;
                    //Jira CAP-2153 - End
                    Label1.Visible = true;
                    //chkPhyName.Visible = true;
                    rdbMA.Visible = true;
                    rdbProvider.Visible = true;
                    chkPhyName.Enabled = true;
                    chkShowAll.Enabled = true;
                    //Jira CAP-2153
                    //cboMoveToMA.Enabled = true;
                    txtAssignedTo.Disabled = false;
                    imgclearAssignTo.Disabled = false;
                    //Jira CAP-2153 - End
                    btnMoveToMa.Enabled = true;
                    btnMoveToNextProcess.Enabled = true;
                    //Jira CAP-2228
                    if (sNotes != null && sNotes.Count() > 0 &&(sNotes.Contains(hdnSubDocumentType.Value.ToUpper())))
                    {
                        //Jira CAP-2369
                        //if (Request["Opening_from"] != null && Request["Opening_from"] != "OrderManagementScreen")
                        if (Request["Opening_from"] != null && Request["Opening_from"] != "OrderManagementScreen" && txtMedicalAssistantNotes.Enabled != true)
                        {
                            imgCopyPrevious.Visible = true;
                        }
                    }
                    //btnSave.Visible = false;
                }
                else
                {
                    //chkPhyName.Enabled = false;
                    //chkShowAll.Enabled = false;
                    //cboMoveToMA.Enabled = false;
                    //btnMoveToMa.Enabled = false;
                    //btnMoveToNextProcess.Enabled = false;
                    btnMoveToNextProcess.Visible = false;
                    btnMoveToMa.Visible = false;
                    chkShowAll.Visible = false;
                    //Jira CAP-2153
                    //cboMoveToMA.Visible = false;
                    txtAssignedTo.Visible = false;
                    imgclearAssignTo.Visible= false;
                    //Jira CAP-2153 -End
                    Label1.Visible = false;
                    chkPhyName.Visible = false;
                    rdbMA.Visible = false;
                    rdbProvider.Visible = false;
                    DLC.txtDLC.Enabled = false;
                    DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    DLC.pbDropdown.Attributes.Remove("class");
                    DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");
                    txtMedicalAssistantNotes.Enabled = false;
                    txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    btnSave.Visible = true;
                }
            }

            if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper().Trim() == "PHYSICIAN" || ClientSession.UserRole == "Physician Assistant")
            {
                txtMedicalAssistantNotes.Enabled = false;
                txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                DLC.txtDLC.Enabled = true;
                DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("White");
                DLC.pbDropdown.Attributes.Remove("class");
                DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackground");
            }
            if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper().Trim() == "MEDICAL ASSISTANT")
            {
                DLC.txtDLC.Enabled = false;
                DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                DLC.pbDropdown.Attributes.Remove("class");
                DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");
                txtMedicalAssistantNotes.Enabled = true;
                txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("White");
            }

            //End BugID:45807
            //Srividhya added the file types other than tif on 2-jan-2015-bug id:29130
            if (sfileExtension.ToUpper() == ".TIF" || sfileExtension.ToUpper() == ".PDF" || sfileExtension.ToUpper() == ".PNG" || sfileExtension.ToUpper() == ".GIF" || sfileExtension.ToUpper() == ".BMP" || sfileExtension.ToUpper() == ".JPG" || sfileExtension.ToUpper() == ".JPEG" || sfileExtension.ToUpper() == ".JPG" || sfileExtension.ToUpper() == ".DCM")
            {
                //BEGIN:---To change the title whenever a subnode is clicked BugID:43099
                UInt64 file_id = 0;
                if (UInt64.TryParse(KeyValue, out file_id))
                {
                    Session["Key_id"] = file_id;
                    IList<FileManagementIndex> lstScanFiles = (from doc in filelist where doc.Id == file_id select doc).ToList<FileManagementIndex>();
                    if (lstScanFiles != null && lstScanFiles.Count > 0)
                    {
                        Session["Result_Master_Id"] = lstScanFiles[0].Result_Master_ID.ToString();
                        if (lstScanFiles[0].Result_Master_ID != 0)
                        {
                            // hdnFaxpath.Value = lstScanFiles[0].Order_ID.ToString() + " $ " + UtilityManager.ConvertToLocal(lstScanFiles[0].Document_Date).ToString("dd-MMM-yyyy h:mm:ss tt") + "$" + lstScanFiles[0].Result_Master_ID;
                            hdnFaxpath.Value = lstScanFiles[0].File_Path.ToString();
                        }
                        else
                        {
                            //hdnFaxpath.Value = lstScanFiles[0].Order_ID.ToString() + " $ " + UtilityManager.ConvertToLocal(lstScanFiles[0].Document_Date).ToString("dd-MMM-yyyy h:mm:ss tt") + " $ " + lstScanFiles[0].Id;
                            hdnFaxpath.Value = lstScanFiles[0].File_Path.ToString();
                        }

                        //hdnFaxpath.Value = lstScanFiles[0].Order_ID.ToString() + " $ " + UtilityManager.ConvertToLocal(lstScanFiles[0].Document_Date).ToString("dd-MMM-yyyy h:mm:ss tt");
                        hdnpath.Value = hdnpath.Value + lstScanFiles[0].Generate_Link_File_Path.ToString();
                        //modified for integrum
                        txtFileInformation.Value = lstScanFiles[0].File_Path.Substring(lstScanFiles[0].File_Path.LastIndexOf("/") + 1).Replace("//", "").Replace("\\", "") + " | " + lstScanFiles[0].Document_Type.ToString() + " | " + lstScanFiles[0].Document_Sub_Type.ToString() + " | " + UtilityManager.ConvertToLocal(lstScanFiles[0].Document_Date).ToString("dd-MMM-yyyy");
                        Session["Scan_Index"] = null;
                        if (lstScanFiles[0].Scan_Index_Conversion_ID != 0 && lstScanFiles[0].Source.ToUpper() == "SCAN")
                        {
                            Session["Scan_Index"] = "INDEX_SCREEN";
                            if (ConfigurationSettings.AppSettings["VisibleDeleteIndexingViewResult"] != null && ConfigurationSettings.AppSettings["VisibleDeleteIndexingViewResult"].ToString().ToUpper().Contains(ClientSession.UserRole.ToUpper()) == true)//&& lstScanFiles[0].Document_Type.ToUpper() != "RESULTS")
                            {
                                btnDeleteIndexing.Visible = true;
                                hdnfileindexid.Value = lstScanFiles[0].Scan_Index_Conversion_ID.ToString();
                                //Cap - 1199
                                //DelIndexDiv.Attributes.Add("width", "40% !important");
                                //CAP-1287
                                DelIndexDiv.Attributes.Add("width", "15% !important");
                            }
                            else
                            {
                                btnDeleteIndexing.Visible = false;
                                DelIndexDiv.Attributes.Remove("width");
                            }
                        }
                        else
                        {
                            btnDeleteIndexing.Visible = false;
                            DelIndexDiv.Attributes.Remove("width");
                        }
                        if ((Request["Openingfrom"] != null && Request["Openingfrom"] == "MyorderQueue") || (Request["Opening_from"] != null && Request["Opening_from"] == "OrdersList") || (Request["Opening_from"] != null && Request["Opening_from"] == "OrderManagementScreen"))
                        {
                            btnDeleteIndexing.Visible = false;
                            DelIndexDiv.Attributes.Remove("width");
                        }
                        if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y")
                        {
                            //sFaxSubject
                            string sFaxFirstname = string.Empty;
                            string sFaxLastName = string.Empty;

                            IList<Human> lsthuman = new List<Human>();
                            IList<string> ilstGeneralPlanTagList = new List<string>();
                            ilstGeneralPlanTagList.Add("HumanList");
                        retry:
                            try
                            {
                                IList<object> ilstGeneralPlanBlobFinal = new List<object>();
                                ilstGeneralPlanBlobFinal = UtilityManager.ReadBlob(Convert.ToUInt64(lstScanFiles[0].Human_ID.ToString()), ilstGeneralPlanTagList);
                                if (ilstGeneralPlanBlobFinal != null && ilstGeneralPlanBlobFinal.Count > 0)
                                {
                                    if (ilstGeneralPlanBlobFinal[0] != null)
                                    {
                                        for (int iCount = 0; iCount < ((IList<object>)ilstGeneralPlanBlobFinal[0]).Count; iCount++)
                                        {
                                            //objFillHuman = (Human)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount];
                                            lsthuman.Add((Human)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount]);
                                        }
                                        sFaxFirstname = "_" + lsthuman[0].First_Name;
                                        sFaxLastName = "_" + lsthuman[0].Last_Name;

                                        hdnFaxSubject.Value = "Result" + sFaxLastName + sFaxFirstname + "_" + UtilityManager.ConvertToLocal(lstScanFiles[0].Document_Date).ToString("dd-MMM-yyyy");

                                    }
                                }
                            }

                            catch (Exception ex)
                            {
                                //if (ex.Message.ToLower().Contains("there is an unclosed literal string") == true || ex.Message.ToLower().Contains("root element is missing") == true || ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                                //{
                                // XmlText.Close();
                                UtilityManager.GenerateXML(lstScanFiles[0].Human_ID.ToString().ToString(), "Human");
                                //return;
                                goto retry;
                                //}
                                //else
                                //{
                                //    throw new Exception(ex.Message + " - " + strXmlHumanPath);
                                //}
                            }
                            //    string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "Human_" + lstScanFiles[0].Human_ID.ToString() + ".xml");
                            //    XmlTextReader XmlText = null;
                            //retry:
                            //    //try
                            //{
                            //    if (File.Exists(strXmlHumanPath) == true)
                            //    {
                            //        XmlDocument itemDoc = new XmlDocument();
                            //        XmlText = new XmlTextReader(strXmlHumanPath);
                            //        using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //        {
                            //            itemDoc.Load(fs);

                            //            XmlText.Close();
                            //            if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                            //            {
                            //                if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes.Count > 0)
                            //                {
                            //                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes.Count > 0)
                            //                    {
                            //                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != null)
                            //                            sFaxFirstname = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                            //                        if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != null)
                            //                            sFaxLastName = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();

                            //                    }
                            //                }
                            //            }
                            //            fs.Close();
                            //            fs.Dispose();
                            //        }

                            //        hdnFaxSubject.Value = "Result" + sFaxLastName + sFaxFirstname + "_" + UtilityManager.ConvertToLocal(lstScanFiles[0].Document_Date).ToString("dd-MMM-yyyy");
                            //        //
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    //if (ex.Message.ToLower().Contains("there is an unclosed literal string") == true || ex.Message.ToLower().Contains("root element is missing") == true || ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                            //    //{
                            //    XmlText.Close();
                            //    UtilityManager.GenerateXML(lstScanFiles[0].Human_ID.ToString().ToString(), "Human");
                            //    //return;
                            //    goto retry;
                            //    //}
                            //    //else
                            //    //{
                            //    //    throw new Exception(ex.Message + " - " + strXmlHumanPath);
                            //    //}
                            //}


                        }
                    }

                }
                //END:---
                tabView.Tabs.Clear();
                PageViewScan.ContentUrl = string.Empty;
                PageViewResult.ContentUrl = string.Empty;
                PageViewResultFiles.ContentUrl = string.Empty;
                PageViewSpirometryResults.ContentUrl = string.Empty;
                PageViewABIResults.ContentUrl = string.Empty;
                PageViewMessageLog.ContentUrl = string.Empty;
                //CAP-790
                if (Session["human_id"] != null)
                {
                    PageViewScan.ContentUrl = "frmImageViewer.aspx?FilePath=" + TargetValue.ToString().Replace("#", "HASHSYMBOL") + "&Source=RESULT" + "&HumanId=" + Session["human_id"].ToString();
                }
                PageViewScan.Selected = true;
                if (OrderSubID != null && IDName != string.Empty)// IDName=="ResultMasterID")//added  for BugID:66433
                {
                    ResultMaster objresultmaster = new ResultMaster();
                    ResultMasterManager masterProxy = new ResultMasterManager();

                    objresultmaster = masterProxy.GetReviewCommentsForViewIndexedImages(Convert.ToUInt64(Session["human_id"]), (IDName + OrderSubID));
                    if (objresultmaster != null && objresultmaster.Id != 0)
                    {
                        //CAP-2478, CAP-2485
                        NewOrderSubmitID = IDName == "ResultMasterID" ? objresultmaster.Order_ID.ToString() : NewOrderSubmitID;
                        //BugID:46221 -- Prov Notes History and Med Notes History
                        txtMedicalAssistantNotes.Text = string.Empty;
                        DLC.txtDLC.Text = string.Empty;
                        txtMedNoteshistory.Text = (objresultmaster.MA_Notes.Trim() != string.Empty && (objresultmaster.MA_Notes.IndexOf("):") == -1)) ?
                            "@" + (objresultmaster.Modified_By.Trim() != string.Empty ? objresultmaster.Modified_By : objresultmaster.Created_By) +
                            "(" + (objresultmaster.Modified_Date_And_Time != DateTime.MinValue ? UtilityManager.ConvertToLocal(objresultmaster.Modified_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") : UtilityManager.ConvertToLocal(objresultmaster.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")) + "): " + objresultmaster.MA_Notes : objresultmaster.MA_Notes;
                        txtMedNoteshistory.Text = txtMedNoteshistory.Text.Replace("<br/>", "\n");
                        if (hdnSubDocumentType.Value != string.Empty && sNotes != null && sNotes.Count() > 0 && (sNotes.Contains(hdnSubDocumentType.Value.ToString()))) // && objresultmaster.Result_Review_Comments.Trim().Contains("Test Reviewed: ") == true)
                        {
                            //string sInterpretationTest = objresultmaster.Result_Review_Comments.Substring(0, objresultmaster.Result_Review_Comments.IndexOf(";")).Replace("[[[Test Reviewed: ", "");

                            //txtProvNoteshistory.Text = (objresultmaster.Result_Review_Comments.Trim() != string.Empty && (objresultmaster.Result_Review_Comments.IndexOf("):") == -1)) ?
                            //"@" + (objresultmaster.Modified_By.Trim() != string.Empty ? objresultmaster.Modified_By : objresultmaster.Created_By) +
                            //"(" + (objresultmaster.Modified_Date_And_Time != DateTime.MinValue ? UtilityManager.ConvertToLocal(objresultmaster.Modified_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") : UtilityManager.ConvertToLocal(objresultmaster.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")) + "): " + sInterpretationTest : sInterpretationTest;
                            string[] reviewcomments = objresultmaster.Result_Review_Comments.Split(new string[] { "]]]" }, StringSplitOptions.None);
                            string NotesHistory = "";
                            string notesattribute = "";
                            for (int i = 0; i < reviewcomments.Length; i++)
                            {
                                if (reviewcomments[i].Trim() != string.Empty)
                                {
                                    if (reviewcomments[i].Contains("Test Reviewed: ") == true)
                                    {

                                        NotesHistory = NotesHistory + reviewcomments[i].Substring(0, reviewcomments[i].IndexOf(";")).Replace("[[[Test Reviewed: ", "");
                                        //Cap - 686
                                        if (notesattribute == string.Empty)
                                        {
                                            notesattribute = reviewcomments[i].Substring(reviewcomments[i].IndexOf("[[[") + 3, reviewcomments[i].Length - reviewcomments[i].IndexOf("[[[") - 3);
                                        }
                                        else
                                        {
                                            notesattribute = notesattribute + "<br/>" + reviewcomments[i].Substring(reviewcomments[i].IndexOf("[[[") + 3, reviewcomments[i].Length - reviewcomments[i].IndexOf("[[[") - 3);
                                        }

                                    }
                                    else
                                    {
                                        NotesHistory = NotesHistory + reviewcomments[i];

                                    }
                                }

                            }
                            txtProvNoteshistory.Text = NotesHistory.Replace("\n\n\n", "\n");// objResultMaster.Result_Review_Comments.Substring(0, objResultMaster.Result_Review_Comments.IndexOf(";")).Replace("[[[Test Reviewed: ", "");

                            txtProvNoteshistory.Attributes.Add("InterpretationText", notesattribute.Replace("\n\n\n", "\n"));
                        }
                        else
                        {
                            txtProvNoteshistory.Text = (objresultmaster.Result_Review_Comments.Trim() != string.Empty && (objresultmaster.Result_Review_Comments.IndexOf("):") == -1)) ?
                               "@" + (objresultmaster.Modified_By.Trim() != string.Empty ? objresultmaster.Modified_By : objresultmaster.Created_By) +
                               "(" + (objresultmaster.Modified_Date_And_Time != DateTime.MinValue ? UtilityManager.ConvertToLocal(objresultmaster.Modified_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") : UtilityManager.ConvertToLocal(objresultmaster.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")) + "): " + objresultmaster.Result_Review_Comments : objresultmaster.Result_Review_Comments;
                        }
                        txtProvNoteshistory.Text = txtProvNoteshistory.Text.Replace("<br/>", "\n");
                        Session["Notes"] = objresultmaster;
                        if (objresultmaster.Reviewed_Provider_Sign_ID != 0)
                        {
                            chkPhyName.Checked = true;
                        }
                        else
                        {
                            chkPhyName.Checked = false;
                        }
                    }
                    else
                    {
                        txtMedicalAssistantNotes.Text = string.Empty;
                        DLC.txtDLC.Text = string.Empty;
                        txtMedNoteshistory.Text = string.Empty;
                        txtProvNoteshistory.Text = string.Empty;
                        txtProvNoteshistory.Attributes.Remove("InterpretationText");
                        chkPhyName.Checked = false;
                    }
                }
                else
                {
                    txtMedicalAssistantNotes.Text = string.Empty;
                    DLC.txtDLC.Text = string.Empty;
                    txtMedNoteshistory.Text = string.Empty;
                    txtProvNoteshistory.Text = string.Empty;
                    txtProvNoteshistory.Attributes.Remove("InterpretationText");
                    chkPhyName.Checked = false;
                }
            }
            else
            {
                tabView.Tabs.Clear();
                PageViewScan.ContentUrl = string.Empty;
                PageViewResult.ContentUrl = string.Empty;
                PageViewResultFiles.ContentUrl = string.Empty;
                PageViewSpirometryResults.ContentUrl = string.Empty;
                PageViewABIResults.ContentUrl = string.Empty;
                PageViewMessageLog.ContentUrl = string.Empty;

                if (cboDocumentType.SelectedItem.Text == "Results")
                {
                    try
                    {
                        if (UInt64.TryParse(TargetValue, out order_id))
                        {
                            Session["Order_Id"] = order_id;
                        }
                        ulong Result_Master_Id = 0;
                        IList<string> Tab_Name = new List<string>();
                        if (order_id != 0)
                        {
                            if (result_master_list != null && result_master_list.Count > 0)
                            {
                                IList<ResultMaster> lstresult = new List<ResultMaster>();
                                lstresult = ((from doc in result_master_list
                                              where doc.Order_ID == order_id
                                              orderby doc.MSH_Date_And_Time_Of_Message descending
                                              select doc)).ToList<ResultMaster>();
                                //BugID:48413 Tabs are sorted by MSH_Date_And_Time_Of_Message descending
                                if (lstresult != null && lstresult.Count > 0)
                                {
                                    Result_Master_Id = lstresult[0].Id;
                                    Session["Result_Master_Id"] = Result_Master_Id;
                                    Tab_Name.Add("Result Master");
                                    //BugID:47602
                                    foreach (ResultMaster rm in lstresult)
                                    {
                                        tb = new RadTab();
                                        tb.Target = "Result Master";
                                        PDFGenerator pdfGenertor = new PDFGenerator();
                                        string dateTime = pdfGenertor.SetDateTimeToControl(rm.MSH_Date_And_Time_Of_Message, "DateAndTime");
                                        if (dateTime != string.Empty)
                                            tb.Text = dateTime.Substring(0, dateTime.LastIndexOf(':')) + " " + dateTime.Substring(dateTime.LastIndexOf(':')).Split(' ')[1];
                                        tb.Value = rm.Id.ToString();
                                        tabView.Tabs.Add(tb);
                                    }
                                    if (tabView.Tabs.Count > 0)
                                        tabView.SelectedIndex = 0;
                                }

                            }
                        }
                        else
                        {
                            UInt64.TryParse(KeyValue, out Result_Master_Id);
                            Tab_Name.Add("Result Master");

                        }
                        ResultMaster objresultmaster = new ResultMaster();
                        ResultMasterManager masterProxy = new ResultMasterManager();
                        if (TargetValue == "0")//to handle Diagnostic_results which do not have OrderID.//BugID:45807
                        {
                            objresultmaster = masterProxy.GetById(Convert.ToUInt64(KeyValue));
                        }
                        else
                        {
                            objresultmaster = masterProxy.GetById(Convert.ToUInt64(Session["Result_Master_Id"]));//BugID:47602
                        }
                        if (objresultmaster != null && objresultmaster.Id != 0)
                        {
                            //BugID:46221 -- Prov Notes History and Med Notes History
                            txtMedicalAssistantNotes.Text = string.Empty;
                            DLC.txtDLC.Text = string.Empty;
                            txtMedNoteshistory.Text = (objresultmaster.MA_Notes.Trim() != string.Empty && (objresultmaster.MA_Notes.IndexOf("):") == -1)) ?
                            "@" + (objresultmaster.Modified_By.Trim() != string.Empty ? objresultmaster.Modified_By : objresultmaster.Created_By) +
                            "(" + (objresultmaster.Modified_Date_And_Time != DateTime.MinValue ? UtilityManager.ConvertToLocal(objresultmaster.Modified_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") : UtilityManager.ConvertToLocal(objresultmaster.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")) + "): " + objresultmaster.MA_Notes : objresultmaster.MA_Notes;
                            txtMedNoteshistory.Text = txtMedNoteshistory.Text.Replace("<br/>", "\n");
                            //if (hdnSubDocumentType.Value != string.Empty && sNotes != null && sNotes.Count() > 0 && (sNotes.Contains(hdnSubDocumentType.Value.ToString())) && objresultmaster.Result_Review_Comments.Trim().Contains("Test Reviewed: ") == true)
                            //{
                            //    //txtProvNoteshistory.Text = objresultmaster.Result_Review_Comments.Trim();
                            //    string sInterpretationTest = objresultmaster.Result_Review_Comments.Substring(0, objresultmaster.Result_Review_Comments.IndexOf(";")).Replace("[[[Test Reviewed: ", "");

                            //    txtProvNoteshistory.Text = (objresultmaster.Result_Review_Comments.Trim() != string.Empty && (objresultmaster.Result_Review_Comments.IndexOf("):") == -1)) ?
                            //    "@" + (objresultmaster.Modified_By.Trim() != string.Empty ? objresultmaster.Modified_By : objresultmaster.Created_By) +
                            //    "(" + (objresultmaster.Modified_Date_And_Time != DateTime.MinValue ? UtilityManager.ConvertToLocal(objresultmaster.Modified_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") : UtilityManager.ConvertToLocal(objresultmaster.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")) + "): " + sInterpretationTest : sInterpretationTest;

                            //    txtProvNoteshistory.Attributes.Add("InterpretationText", objresultmaster.Result_Review_Comments.Substring(objresultmaster.Result_Review_Comments.IndexOf("[[[") + 3, objresultmaster.Result_Review_Comments.IndexOf("]]]") - objresultmaster.Result_Review_Comments.IndexOf("[[[") - 3));
                            //}
                            if (hdnSubDocumentType.Value != string.Empty && sNotes != null && sNotes.Count() > 0 && (sNotes.Contains(hdnSubDocumentType.Value.ToString()))) // && objresultmaster.Result_Review_Comments.Trim().Contains("Test Reviewed: ") == true)
                            {
                                //string sInterpretationTest = objresultmaster.Result_Review_Comments.Substring(0, objresultmaster.Result_Review_Comments.IndexOf(";")).Replace("[[[Test Reviewed: ", "");

                                //txtProvNoteshistory.Text = (objresultmaster.Result_Review_Comments.Trim() != string.Empty && (objresultmaster.Result_Review_Comments.IndexOf("):") == -1)) ?
                                //"@" + (objresultmaster.Modified_By.Trim() != string.Empty ? objresultmaster.Modified_By : objresultmaster.Created_By) +
                                //"(" + (objresultmaster.Modified_Date_And_Time != DateTime.MinValue ? UtilityManager.ConvertToLocal(objresultmaster.Modified_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") : UtilityManager.ConvertToLocal(objresultmaster.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")) + "): " + sInterpretationTest : sInterpretationTest;
                                string[] reviewcomments = objresultmaster.Result_Review_Comments.Split(new string[] { "]]]" }, StringSplitOptions.None);
                                string NotesHistory = "";
                                string notesattribute = "";
                                for (int i = 0; i < reviewcomments.Length; i++)
                                {
                                    if (reviewcomments[i].Trim() != string.Empty)
                                    {
                                        if (reviewcomments[i].Contains("Test Reviewed: ") == true)
                                        {

                                            NotesHistory = NotesHistory + reviewcomments[i].Substring(0, reviewcomments[i].IndexOf(";")).Replace("[[[Test Reviewed: ", ""); ;
                                            //Cap - 686
                                            if (notesattribute == string.Empty)
                                            {
                                                notesattribute = reviewcomments[i].Substring(reviewcomments[i].IndexOf("[[[") + 3, reviewcomments[i].Length - reviewcomments[i].IndexOf("[[[") - 3);
                                            }
                                            else
                                            {
                                                notesattribute = notesattribute + "<br/>" + reviewcomments[i].Substring(reviewcomments[i].IndexOf("[[[") + 3, reviewcomments[i].Length - reviewcomments[i].IndexOf("[[[") - 3);
                                            }

                                        }
                                        else
                                        {
                                            NotesHistory = NotesHistory + reviewcomments[i];

                                        }
                                    }

                                }
                                txtProvNoteshistory.Text = NotesHistory.Replace("\n\n\n", "\n");// objResultMaster.Result_Review_Comments.Substring(0, objResultMaster.Result_Review_Comments.IndexOf(";")).Replace("[[[Test Reviewed: ", "");

                                txtProvNoteshistory.Attributes.Add("InterpretationText", notesattribute.Replace("\n\n\n", "\n"));
                            }
                            else
                            {
                                txtProvNoteshistory.Text = (objresultmaster.Result_Review_Comments.Trim() != string.Empty && (objresultmaster.Result_Review_Comments.IndexOf("):") == -1)) ?
                                    "@" + (objresultmaster.Modified_By.Trim() != string.Empty ? objresultmaster.Modified_By : objresultmaster.Created_By) +
                                    "(" + (objresultmaster.Modified_Date_And_Time != DateTime.MinValue ? UtilityManager.ConvertToLocal(objresultmaster.Modified_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") : UtilityManager.ConvertToLocal(objresultmaster.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")) + "): " + objresultmaster.Result_Review_Comments : objresultmaster.Result_Review_Comments;
                            }
                            txtProvNoteshistory.Text = txtProvNoteshistory.Text.Replace("<br/>", "\n");
                            Session["Notes"] = objresultmaster;
                            if (objresultmaster.Reviewed_Provider_Sign_ID != 0)
                            {
                                chkPhyName.Checked = true;
                            }
                            else
                            {
                                chkPhyName.Checked = false;
                            }
                        }
                        else
                        {
                            txtMedicalAssistantNotes.Text = string.Empty;
                            DLC.txtDLC.Text = string.Empty;
                            txtMedNoteshistory.Text = string.Empty;
                            txtProvNoteshistory.Text = string.Empty;
                            txtProvNoteshistory.Attributes.Remove("InterpretationText");
                            chkPhyName.Checked = false;
                        }

                        for (int i = 0; i < Tab_Name.Count; i++)
                        {
                            tb = new RadTab();
                            tb.Target = "tb" + Tab_Name[i].ToLower();
                            if (Tab_Name[i].ToLower() == "scan")
                            {
                                btnPrintEducatnMaterial.Visible = false;
                                IList<string> lstScanId = ((from doc in filelist
                                                            where doc.Order_ID == order_id
                                                            select doc.File_Path).Distinct()).ToList<string>();
                                tb.Text = "Scanned Results";
                                /* File Header */
                                IList<FileManagementIndex> lstScanIds = (from doc in filelist where doc.Order_ID == order_id select doc).ToList<FileManagementIndex>();
                                if (lstScanIds != null && lstScanIds.Count > 0)
                                    //modified for integrum
                                    txtFileInformation.Value = lstScanIds[0].File_Path.Substring(lstScanIds[0].File_Path.LastIndexOf("/") + 1).Replace("//", "").Replace("\\", "") + " | " + "Results" + " | " + lstScanIds[0].Document_Sub_Type.ToString() + " | " + UtilityManager.ConvertToLocal(lstScanIds[0].Document_Date).ToString("dd-MMM-yyyy");//BugID:42809
                            }

                            else if (Tab_Name[i].ToLower() == "order")
                            {
                                tb.Text = "Result Files";
                            }
                            else if (Tab_Name[i].ToLower() == "result master")
                            {
                                PDFGenerator objPDFGenerator = new PDFGenerator();
                                string filepath = string.Empty;
                                if (Directory.Exists(Server.MapPath("Documents/" + Session.SessionID)))
                                    filepath = Server.MapPath("Documents/" + Session.SessionID) + "\\";
                                else
                                {
                                    Directory.CreateDirectory(Server.MapPath("Documents/" + Session.SessionID));
                                    filepath = Server.MapPath("Documents/" + Session.SessionID) + "\\";
                                }

                                hdnSelectedItem.Value = String.Empty;
                                string file = string.Empty;
                                string filename = objPDFGenerator.GenerateRequestionForLabcorp(Result_Master_Id, filepath);
                                hdnFaxpath.Value = filename + "|" + objresultmaster.Order_ID;
                                XmlTextReader XmlText = null;
                                if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y")
                                {
                                    //sFaxSubject


                                    string sFaxFirstname = string.Empty;
                                    string sFaxLastName = string.Empty;
                                retry:
                                    try
                                    {
                                        IList<Human> lsthuman = new List<Human>();
                                        IList<string> ilstGeneralPlanTagList = new List<string>();
                                        ilstGeneralPlanTagList.Add("HumanList");
                                        IList<object> ilstGeneralPlanBlobFinal = new List<object>();
                                        ilstGeneralPlanBlobFinal = UtilityManager.ReadBlob(Convert.ToUInt64(objresultmaster.Matching_Patient_Id.ToString()), ilstGeneralPlanTagList);
                                        if (ilstGeneralPlanBlobFinal != null && ilstGeneralPlanBlobFinal.Count > 0)
                                        {
                                            if (ilstGeneralPlanBlobFinal[0] != null)
                                            {
                                                for (int iCount = 0; iCount < ((IList<object>)ilstGeneralPlanBlobFinal[0]).Count; iCount++)
                                                {
                                                    //objFillHuman = (Human)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount];
                                                    lsthuman.Add((Human)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount]);
                                                }
                                                sFaxFirstname = "_" + lsthuman[0].First_Name;
                                                sFaxLastName = "_" + lsthuman[0].Last_Name;


                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //if (ex.Message.ToLower().Contains("there is an unclosed literal string") == true || ex.Message.ToLower().Contains("root element is missing") == true || ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                                        //{
                                        XmlText.Close();
                                        UtilityManager.GenerateXML(objresultmaster.Matching_Patient_Id.ToString().ToString(), "Human");
                                        //return;
                                        goto retry;
                                        //}
                                        //else
                                        //{
                                        //    throw new Exception(ex.Message + " - " + strXmlHumanPath);
                                        //}
                                    }
                                    //retry:
                                    //    string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "Human_" + objresultmaster.Matching_Patient_Id.ToString() + ".xml");
                                    //try
                                    //{
                                    //    if (File.Exists(strXmlHumanPath) == true)
                                    //    {
                                    //        XmlDocument itemDoc = new XmlDocument();
                                    //        XmlText = new XmlTextReader(strXmlHumanPath);
                                    //        using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                                    //        {
                                    //            itemDoc.Load(fs);

                                    //            XmlText.Close();
                                    //            if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                                    //            {
                                    //                if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes.Count > 0)
                                    //                {
                                    //                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != null)
                                    //                        sFaxFirstname = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                    //                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != null)
                                    //                        sFaxLastName = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();

                                    //                }
                                    //            }
                                    //            fs.Close();
                                    //            fs.Dispose();
                                    //        }
                                    //    }
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    //if (ex.Message.ToLower().Contains("there is an unclosed literal string") == true || ex.Message.ToLower().Contains("root element is missing") == true || ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                                    //    //{
                                    //    XmlText.Close();
                                    //    UtilityManager.GenerateXML(objresultmaster.Matching_Patient_Id.ToString().ToString(), "Human");
                                    //    //return;
                                    //    goto retry;
                                    //    //}
                                    //    //else
                                    //    //{
                                    //    //    throw new Exception(ex.Message + " - " + strXmlHumanPath);
                                    //    //}
                                    //}

                                    hdnFaxSubject.Value = "Result" + sFaxLastName + sFaxFirstname + "_" + sResult_obr_date;
                                }

                                string[] Split = null;
                                //if(Directory.Exists(Server.MapPath("Documents\\" + Session.SessionID)))

                                if (!Directory.Exists(Server.MapPath("Documents/" + Session.SessionID)))
                                    Directory.CreateDirectory(Server.MapPath("Documents/" + Session.SessionID));



                                Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                                string[] FileName = filename.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                                if (FileName != null && FileName.Length > 0)
                                    file = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                                //string loc = "DYNAMIC";
                                if (hdnSelectedItem.Value == string.Empty)
                                {
                                    if (FileName != null && FileName.Length > 0)
                                    {
                                        hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                                        txtFileInformation.Value = FileName[0].ToString().Replace("//", "").Replace("\\", "") + " | " + "Results";
                                    }

                                    tb.Value = order_id.ToString();
                                    tabView.Tabs.Add(tb);
                                }
                                else
                                {
                                    if (FileName != null && FileName.Length > 0)
                                    {
                                        hdnSelectedItem.Value += "|" + FileName[0].ToString();
                                    }
                                }
                                PageViewResult.ContentUrl = hdnSelectedItem.Value.ToString();
                                PageViewResult.Selected = true;

                            }
                            else if (Tab_Name[i].ToLower() == "abi")
                            {
                                tb.Text = "ABI Results";
                                tb.Value = order_id.ToString();
                                tabView.Tabs.Add(tb);

                            }
                            else if (Tab_Name[i].ToLower() == "spirometry")
                            {
                                tb.Text = "Spirometry Results";
                                tb.Value = order_id.ToString();
                                tabView.Tabs.Add(tb);

                            }
                            else if (Tab_Name[i].ToLower() == "message")
                            {
                                tb.Text = "Message Log";
                                tb.Value = order_id.ToString();
                                tabView.Tabs.Add(tb);
                            }



                        }

                        for (int i = 0; i < Tab_Name.Count; i++)
                        {
                            if (Tab_Name[i].ToLower() == "scan")
                            {
                                IList<string> lstScanId = ((from doc in filelist
                                                            where doc.Order_ID == order_id
                                                            select doc.File_Path).Distinct()).ToList<string>();
                                if (filelist.Count == 1 && filelist.Count > 0)
                                {
                                    /* Changing The Image Viewer To View the Multiple Results Attached To Same Order  */
                                    if (lstScanId != null && lstScanId.Count > 0)
                                        PageViewScan.ContentUrl = "frmImageViewer.aspx?FilePath=" + lstScanId[0].ToString().Replace("#", "HASHSYMBOL") + "&Source=RESULT" + "&HumanId=" + Session["human_id"].ToString();
                                }
                                else
                                {
                                    /* To Avoid Alteration Of Existing Working , Left The Code Block For Single Result To Order Mapping */
                                    if (lstScanId != null && lstScanId.Count > 0)
                                        PageViewScan.ContentUrl = "frmImageViewer.aspx?FilePath=" + lstScanId[0].ToString().Replace("#", "HASHSYMBOL") + "&Source=RESULT" + "&HumanId=" + Session["human_id"].ToString();
                                }
                                PageViewScan.Selected = true;
                                break;
                            }

                            else if (Tab_Name[i].ToLower() == "order")
                            {
                                FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
                                // IList<FileManagementIndex> filelist_Orders = objfileproxy.GetImagesforAnnotations(Convert.ToUInt32(Session["HumanId"]), Convert.ToUInt64(TargetValue), "ORDER");
                                IList<FileManagementIndex> lstfile = new List<FileManagementIndex>();
                                lstfile = (from doc in filelist
                                           where doc.Source == "ORDER"
                                           select doc).ToList<FileManagementIndex>();
                                if (lstfile != null && lstfile.Count > 0)
                                {
                                    if (lstfile[0].Source == "ORDER")
                                    {
                                        string concatenatedFiles = string.Empty;

                                        string filename = string.Empty;
                                        lstfile.OrderBy(a => a.Id).FirstOrDefault();
                                        tb = new RadTab();
                                        tb.Target = "tbOrder";
                                        tb.Text = "Result Files";
                                        tabView.Tabs.Add(tb);
                                        if (Request["Type"] == "Results")
                                        {
                                            IList<FileManagementIndex> IndexList = new List<FileManagementIndex>();
                                            IndexList = lstfile.Where(f => f.Id == Convert.ToUInt32(KeyValue)).ToList<FileManagementIndex>();
                                            if (IndexList != null && IndexList.Count > 0)
                                            {
                                                PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + IndexList[0].File_Path;
                                            }
                                            else if (lstfile != null && lstfile.Count > 0)
                                            {
                                                PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + lstfile[lstfile.Count - 1].File_Path;
                                            }
                                        }
                                        else if (Request.QueryString["Screen"] == "ResultView" || Request.QueryString["Screen"] == "OrderManagement")
                                        {
                                            PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + TargetValue;
                                        }
                                        else if (lstfile != null && lstfile.Count > 0)
                                        {
                                            PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + lstfile[lstfile.Count - 1].File_Path;
                                        }
                                        else
                                        {
                                            PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + lstfile[01].File_Path;
                                        }
                                        PageViewResult.Selected = true;
                                        //modified for Integrum
                                        txtFileInformation.Value = lstfile[0].File_Path.Split('/')[5].Replace("//", "").Replace("\\", "") + "|" + lstfile[0].Document_Type.ToString() + " | " + lstfile[0].Document_Sub_Type.ToString() + " | " + UtilityManager.ConvertToLocal(lstfile[0].Document_Date).ToString("dd-MMM-yyyy");//BugID:42809
                                    }
                                }

                                //PageViewResultFiles.ContentUrl = "frmImageViewer.aspx?Screen=ResultView" + "&OrderSubmitId=" + Request["OrderSubmitId"];
                                //PageViewResultFiles.Selected = true;
                                break;
                            }
                            else if (Tab_Name[i].ToLower() == "result master")
                            {
                                //PageViewResult.ContentUrl = "frmResult.aspx?Result_Master_ID=" + Result_Master_Id + "&strScreenName=PATIENT CHART";
                                //PageViewResult.Selected = true;
                                //break;
                            }
                            else if (Tab_Name[i].ToLower() == "abi")
                            {
                                PageViewABIResults.ContentUrl = "frmABIResult.aspx?HumanId=" + Session["human_id"].ToString();
                                PageViewABIResults.Selected = true;
                                break;
                            }
                            else if (Tab_Name[i].ToLower() == "spirometry")
                            {
                                PageViewSpirometryResults.ContentUrl = "frmSpirometry.aspx?HumanId=" + Session["human_id"].ToString();
                                PageViewSpirometryResults.Selected = true;
                                break;
                            }
                            else if (Tab_Name[i].ToLower() == "message")
                            {
                                break;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        //do nothing
                        throw e;

                    }
                }

            }


            if (Request["Opening_from"] != null && Request["Opening_from"] == "Patient_Pane") //Opening from Left side Patient Pane
            {
                WFObjectManager WFObjMngr = new WFObjectManager();
                WFObject wfObj = new WFObject();

                if (IDName == "OrderSubmitID")
                {
                    wfObj = WFObjMngr.GetByObjectSystemIdIncludeArchive(Convert.ToUInt64(OrderSubID), "DIAGNOSTIC ORDER");
                    hdnLeftPaneOrderSubmitID.Value = wfObj.Obj_System_Id.ToString();

                }
                else if (IDName == "ResultMasterID")
                {
                    wfObj = WFObjMngr.GetByObjectSystemIdIncludeArchive(Convert.ToUInt64(OrderSubID), "DIAGNOSTIC_RESULT");
                    hdnLeftPaneResultMasterID.Value = wfObj.Obj_System_Id.ToString();
                }
                //Cap - 686
                hdncurrentProcess.Value = wfObj.Current_Process;
                if (wfObj.Current_Process == "RESULT_REVIEW" && wfObj.Current_Owner == ClientSession.UserName)
                {
                    hdnLeftPaneObjType.Value = wfObj.Obj_Type;
                    hdnLeftPaneCurrentProcess.Value = wfObj.Current_Process;

                    btnMoveToNextProcess.Visible = true;
                    //Jira CAP-2228
                    if (sNotes != null && sNotes.Count() > 0 && (sNotes.Contains(hdnSubDocumentType.Value.ToUpper())))
                    {
                        if (Request["Opening_from"] != null && Request["Opening_from"] != "OrderManagementScreen")
                        {
                            imgCopyPrevious.Visible = true;
                        }
                    }
                    btnMoveToMa.Visible = true;
                    chkShowAll.Visible = true;
                    //Jira CAP-2153
                    //cboMoveToMA.Visible = true;
                    txtAssignedTo.Visible = true;
                    imgclearAssignTo.Visible = true;
                    //Jira CAP-2153 - End
                    Label1.Visible = true;
                    chkPhyName.Visible = true;
                    rdbMA.Checked = true;
                    FillCombobox();
                    //btnSave.Style["margin-left"] = "308px"; //"306px";   //"352px";  //"348px"; // "440px";
                    //btnSave.Style["margin-top"] = "-56px";
                    rdbMA.Visible = true;
                    rdbProvider.Visible = true;
                    string sTemp = string.Empty;
                    //string[] sPhyname = null;
                    //StaticLookupManager objstaticlookup = new StaticLookupManager();
                    //IList<StaticLookup> StaticLst = objstaticlookup.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES");
                    IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
                    PhysicianManager objphysican = new PhysicianManager();

                    //Cap - 1169
                    //if (ClientSession.PhysicianId != 0)
                    //{
                    //PhysicianList = objphysican.GetphysiciannameByPhyID(Convert.ToUInt32(ClientSession.PhysicianId));
                    PhysicianList = objphysican.GetphysiciannameByUserName(ClientSession.UserName);
                    if (PhysicianList != null && PhysicianList.Count > 0)
                        {
                            //if (StaticLst != null && StaticLst.Count > 0)
                            //{
                            //    sPhyname = StaticLst[0].Value.Split('|');
                            //    if (sPhyname != null && sPhyname.Length > 1 && sPhyname[1].Trim() != string.Empty)
                            //    {
                            //        sTemp = sPhyname[1].Replace("<Physician>", PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + " " + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix);
                            //    }
                            //}
                            sTemp = "Electronically Signed by " + PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + (string.IsNullOrEmpty(PhysicianList[0].PhyMiddleName) ? "" : " ") + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix;

                        }
                        //sTemp = sTemp.Replace("<Date>", "");
                        //sTemp = sTemp.Replace(" on ", "");
                        chkPhyName.Text = sTemp;
                    //}
                }
                else
                {
                    btnMoveToNextProcess.Visible = false;
                    btnMoveToMa.Visible = false;
                    chkShowAll.Visible = false;
                    //Jira CAP-2153
                    //cboMoveToMA.Visible = false;
                    txtAssignedTo.Visible = false;
                    imgclearAssignTo.Visible = false;
                    //Jira CAP-2153 - End
                    Label1.Visible = false;
                    chkPhyName.Visible = false;
                    rdbMA.Visible = false;
                    rdbProvider.Visible = false;
                    //btnSave.Style["margin-left"] = "20.5%";    //"278px";  //"278px";  //"370px";
                    //btnSave.Style["margin-top"] = "-4.2%";

                    //btnSave.Style["margin-left"] = "455px";
                    // btnEfax.Enabled = false;
                }
            }
            
            FillAkidoInterpretationNoteButton(NewOrderSubmitID);
        }

        protected void tvViewIndex_NodeClick1(object sender, RadTreeNodeEventArgs e)
        {
            if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper().Trim() == "PHYSICIAN" || ClientSession.UserRole == "Physician Assistant")
            {
                if (sNotes != null && sNotes.Count() > 0 && e.Node.ParentNode != null && (sNotes.Contains(e.Node.ParentNode.Text.ToUpper())))
                {
                    hdnSubDocumentType.Value = e.Node.ParentNode.Text.ToUpper();
                    if (Request["Opening_from"] != null && Request["Opening_from"] != "OrderManagementScreen")
                    {
                        imgCopyPrevious.Visible = true;
                    }
                    //DLC.txtDLC.Enabled = false;
                    //DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    //DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    //DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    //DLC.pbDropdown.Attributes.Remove("class");
                    //DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");
                    DLC.txtDLC.Enabled = true;
                    DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("White");
                    DLC.pbDropdown.Attributes.Remove("class");
                    DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackground");
                    imgPrintNotes.Visible = true;
                }
                else
                {
                    hdnSubDocumentType.Value = "";
                    imgCopyPrevious.Visible = false;
                    imgPrintNotes.Visible = false;
                }
            }
            if (sNotes != null && sNotes.Count() > 0 && e.Node.ParentNode != null && (sNotes.Contains(e.Node.ParentNode.Text.ToUpper())))
                imgPrintNotes.Visible = true;

            if (e.Node.Nodes.Count == 0 && e.Node.Attributes["ResultMasterID"] != null)
            {
                TabLoad(e.Node.Value.ToString(), e.Node.Target.ToString(), e.Node.Attributes["ResultMasterID"].ToString(), "ResultMasterID");
                Session["Result_Master_Id"] = e.Node.Attributes["ResultMasterID"].ToString();
            }
            else if (e.Node.Nodes.Count == 0 && e.Node.Attributes["OrderSubmitId"] != null)//BugID:48326
            {
                TabLoad(e.Node.Value.ToString(), e.Node.Target.ToString(), e.Node.Attributes["OrderSubmitId"].ToString(), "OrderSubmitID");
                Session["Order_Id"] = e.Node.Attributes["OrderSubmitId"].ToString();
                //Session["Order_Id"] = 0;
            }
            if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y")
            {
                try
                {
                    hdnFaxSubject.Value = "Result_" + hdnPatientlastname.Value + hdnPatientfirstname.Value + "_" + Convert.ToDateTime(e.Node.Text).ToString("yyyy-MMM-dd");
                }
                catch
                {
                    hdnFaxSubject.Value = "Result_" + hdnPatientlastname.Value + hdnPatientfirstname.Value + "_" + e.Node.Text;
                }
            }
            //e.Node.FullPath.Split('/')[0];

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }
        #endregion
        #region itemEvents
        protected void pbFilter_Click(object sender, ImageClickEventArgs e)
        {
            //pbPlus.ID = "pbPlus";
            //if (cboDocumentType.Text == "Results")
            //{
            //    loadTreeView(cboDocumentType.Text, "");
            //}
            //else
            //{
            //    loadTreeView(cboDocumentType.Text, txtDocumentSubType.Text);
            //}
        }
        protected void cboDocumentType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboDocumentType != null && cboDocumentType.Items.Count > 0 && cboDocumentType.SelectedItem.Text != string.Empty)
            {
                Session["Notes"] = null;//BugID:46316 -- by default value is null.
                Session["Doc_type"] = cboDocumentType.SelectedItem.Text;
                DLC.txtDLC.Text = string.Empty;
                txtMedicalAssistantNotes.Text = string.Empty;
                txtProvNoteshistory.Text = string.Empty;
                txtProvNoteshistory.Attributes.Remove("InterpretationText");
                txtMedNoteshistory.Text = string.Empty;
                //DLC.txtDLC.Enabled = false;
                //txtMedicalAssistantNotes.Enabled = false;
                //DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                //DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                //DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                //DLC.pbDropdown.Attributes.Remove("class");
                //DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");
                //txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                //txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                //txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                ConstructTreeView(Convert.ToUInt64(Session["human_id"]), cboDocumentType.SelectedItem.Text, 0);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);//to stopload after documenttype is loaded.
                pbPlus.ID = "pbPlus";
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }
        protected void pbPlus_Click(object sender, ImageClickEventArgs e)
        {
            //if (pbPlus.ID == "pbPlus")
            //{
            //    HtmlTable ctrl = (HtmlTable)Page.FindControl("tblTree");
            //    lsttype = new RadListBox();
            //    lsttype.ID = "lstDynamic";
            //    lsttype.Height = 100;
            //    lsttype.Width = txtDocumentSubType.Width;
            //    lsttype.OnClientSelectedIndexChanged = "SelectedIndexChanged";
            //    ctrl.Rows[3].Cells[0].Controls.Add(lsttype);
            //    pbPlus.ImageUrl = "~/Resources/minus_new.gif";
            //    pbPlus.ID = "pbMinus";
            //    LoadlstNotes();
            //    lsttype.Visible = true;
            //}
            //else
            //{
            //    lsttype.Visible = false;
            //    pbPlus.ImageUrl = "~/Resources/plus_new.gif";
            //    pbPlus.ID = "pbPlus";
            //}


        }

        protected void btnMoveToNextProcess_Click(object sender, EventArgs e)
        {


            WFObjectManager obj_workFlow = new WFObjectManager();
            ulong LabId = 0;
            if (Request["LabId"] != null)
                LabId = Convert.ToUInt32(Request["LabId"]);
            else if (Request["Screen"] != null && Request["Screen"] == "ResultViewOrder")
                LabId = 32;
            //if (btnSave.Enabled == true)//bug Id:56084 
            // {
            SaveNotes();
            //  }
            if (LabId == 32)
            {
                FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
                IList<FileManagementIndex> lstfilemanage = new List<FileManagementIndex>();
                lstfilemanage = objfileproxy.GetImagesforAnnotations(Convert.ToUInt64(Request["HumanID"]), Convert.ToUInt64(Request["OrderSubmitId"]), "ORDER,ABI,SPIROMETRY,SCAN");
                if (lstfilemanage != null && lstfilemanage.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Test", "DisplayErrorMessage('115039'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                    return;

                }

                OrdersSubmitManager objordeproxy = new OrdersSubmitManager();
                IList<OrdersSubmit> lstordersubmit = new List<OrdersSubmit>();
                lstordersubmit = objordeproxy.GetOrdersSubmitListbyID(Convert.ToUInt64(Request["OrderSubmitId"]));

                if (lstordersubmit != null && lstordersubmit.Count > 0)
                {
                    if (lstordersubmit[0].Specimen_Collection_Date_And_Time == DateTime.MinValue)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Test", "DisplayErrorMessage('115040'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                        return;
                    }

                }
            }

            if (Request["Opening_from"] != null && Request["Opening_from"] != "Patient_Pane")
            {
                string Current_Process = string.Empty;
                int close_type = 0;
                string User_name = string.Empty;
                string[] current_process = new string[] { Request["CurrentProcess"].Replace(" _", "_").ToString() };

                if (Request["CurrentProcess"] != null)
                    Current_Process = Request["CurrentProcess"].Replace(" _", "_").ToString();

                UserManager objPhy = new UserManager();
                string sPhysicianName = string.Empty;

                IList<User> PhyUserList = objPhy.getUserByPHYID(ClientSession.PhysicianId);
                if (PhyUserList != null && PhyUserList.Count > 0)
                {
                    sPhysicianName = PhyUserList[0].user_name;
                    User_name = PhyUserList[0].user_name;
                }

                if (Current_Process == "ORDER_GENERATE" && ClientSession.UserRole == "Medical Assistant")
                {
                    close_type = 8;

                }
                else if (Current_Process == "MA_REVIEW" && ClientSession.UserRole == "Medical Assistant")
                {
                    close_type = 1;
                    User_name = ClientSession.UserName;

                }
                else if (Current_Process == "ORDER_GENERATE" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
                {
                    close_type = 9;
                    User_name = "UNKNOWN";
                }
                else if (Current_Process == "RESULT_REVIEW" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
                {
                    close_type = 1;
                    User_name = "UNKNOWN";
                }
                else if (Current_Process == "MA_RESULTS")
                {
                    close_type = 3;
                    User_name = "UNKNOWN";
                }








                if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                {

                    if (chkPhyName.Checked == true)
                    {
                        //if (btnSave.Enabled == true)
                        if (btnSave.Disabled == false)
                        {
                            SaveNotes();
                        }

                        IList<string> _Status_Flag = new List<string>();
                        if (Session["Status_Flag"] != null)
                        {
                            _Status_Flag = (IList<string>)Session["Status_Flag"];
                        }
                        // string[] current_process = new string[] { "RESULT_REVIEW" };
                        if (_Status_Flag != null)
                        {
                            if (_Status_Flag.Count == 0)
                            {
                                //$muthusamy on 23-12-2014 for LabResult Changes
                                if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
                                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", close_type, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                                else
                                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", close_type, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                                //$muthusamy 
                            }
                            else
                            {
                                if ((_Status_Flag.Contains("P") && _Status_Flag.Contains("F")) || _Status_Flag.Contains("F"))
                                {
                                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 1, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                                }
                                else if (_Status_Flag.Contains("P") && !_Status_Flag.Contains("F"))
                                {
                                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 2, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                                }
                            }
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Test", "DisplayErrorMessage('115034'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                        return;
                    }
                }

                else
                {
                    //$muthusamy on 23-12-2014 for LabResult Changes
                    if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
                        obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 1, User_name, UtilityManager.ConvertToUniversal(), null, current_process, null);
                    else
                        obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(), null, current_process, null);
                    //$muthusamy
                }
                //Added for moving order from MA_REVIEW to RESULT_REVIEW (Bug Id: 29134)
                //Begin
                if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
                {
                    if (Current_Process == "MA_REVIEW")
                    {

                        string sPhyName = string.Empty;
                        UserManager objPhyMngr = new UserManager();

                        IList<User> PhyList = objPhyMngr.getUserByPHYID(ClientSession.PhysicianId);
                        if (PhyList != null && PhyList.Count > 0)
                            sPhyName = PhyList[0].user_name;
                        string[] current_proc = new string[] { "ORDER_GENERATE" };
                        obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 8, sPhyName, UtilityManager.ConvertToUniversal(), null, current_proc, null);


                    }
                }
            }
            else if (Request["Opening_from"] != null && Request["Opening_from"] == "Patient_Pane")
            {
                string Current_Process = string.Empty;
                int close_type = 0;
                string User_name = string.Empty;
                string[] current_process = new string[] { hdnLeftPaneCurrentProcess.Value };

                Current_Process = hdnLeftPaneCurrentProcess.Value;

                UserManager objPhy = new UserManager();
                string sPhysicianName = string.Empty;

                IList<User> PhyUserList = objPhy.getUserByPHYID(ClientSession.PhysicianId);
                if (PhyUserList != null && PhyUserList.Count > 0)
                {
                    sPhysicianName = PhyUserList[0].user_name;
                    User_name = PhyUserList[0].user_name;
                }

                if (Current_Process == "RESULT_REVIEW" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
                {
                    close_type = 1;
                    User_name = "UNKNOWN";
                }
                //else if (Current_Process == "MA_RESULTS")
                //{
                //    close_type = 3;
                //    User_name = "UNKNOWN";
                //}

                if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                {

                    if (chkPhyName.Checked == true)
                    {
                        //if (btnSave.Enabled == true)
                        if (btnSave.Disabled == false)
                        {
                            SaveNotes();
                        }

                        IList<string> _Status_Flag = new List<string>();
                        if (Session["Status_Flag"] != null)
                        {
                            _Status_Flag = (IList<string>)Session["Status_Flag"];
                        }
                        // string[] current_process = new string[] { "RESULT_REVIEW" };
                        if (_Status_Flag != null)
                        {
                            if (_Status_Flag.Count == 0)
                            {
                                //$muthusamy on 23-12-2014 for LabResult Changes
                                if (hdnLeftPaneObjType.Value == "DIAGNOSTIC_RESULT")
                                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(hdnLeftPaneResultMasterID.Value), "DIAGNOSTIC_RESULT", close_type, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                                else
                                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(hdnLeftPaneOrderSubmitID.Value), "DIAGNOSTIC ORDER", close_type, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                                //$muthusamy 
                            }
                            else
                            {
                                if ((_Status_Flag.Contains("P") && _Status_Flag.Contains("F")) || _Status_Flag.Contains("F"))
                                {
                                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(hdnLeftPaneOrderSubmitID.Value), "DIAGNOSTIC ORDER", 1, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                                }
                                else if (_Status_Flag.Contains("P") && !_Status_Flag.Contains("F"))
                                {
                                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(hdnLeftPaneOrderSubmitID.Value), "DIAGNOSTIC ORDER", 2, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                                }
                            }
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Test", "DisplayErrorMessage('115034'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                        return;
                    }
                }

                //else
                //{
                //    //$muthusamy on 23-12-2014 for LabResult Changes
                //    if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
                //        obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 1, User_name, UtilityManager.ConvertToUniversal(), null, current_process, null);
                //    else
                //        obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(), null, current_process, null);
                //    //$muthusamy
                //}
                //Added for moving order from MA_REVIEW to RESULT_REVIEW (Bug Id: 29134)
                //Begin
                //if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
                //{
                //    if (Current_Process == "MA_REVIEW")
                //    {

                //        string sPhyName = string.Empty;
                //        UserManager objPhyMngr = new UserManager();

                //        IList<User> PhyList = objPhyMngr.getUserByPHYID(ClientSession.PhysicianId);
                //        if (PhyList != null && PhyList.Count > 0)
                //            sPhyName = PhyList[0].user_name;
                //        string[] current_proc = new string[] { "ORDER_GENERATE" };
                //        obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 8, sPhyName, UtilityManager.ConvertToUniversal(), null, current_proc, null);


                //    }
                //}
                btnMoveToNextProcess.Visible = false;
                btnMoveToMa.Visible = false;
                chkShowAll.Visible = false;
                //Jira CAP-2153
                //cboMoveToMA.Visible = false;
                txtAssignedTo.Visible = false;
                imgclearAssignTo.Visible= false;
                //Jira CAP-2153 - End
                Label1.Visible = false;
                chkPhyName.Visible = false;
                rdbMA.Visible = false;
                rdbProvider.Visible = false;
                //btnSave.Style["margin-left"] = "281px";//"348px";  //"440px";
                // btnSave.Style["margin-left"] = "455px";
            }           
            //End
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MovedSuccessfully", "DisplayErrorMessage('050002');", true);
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Close", "End(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NextResult", "ViewNextResult();", true);//BugID:41027 -- move to next result         
        }
        protected void btnMoveToMa_Click(object sender, EventArgs e)
        {
            //  if (btnSave.Enabled == true)//bug Id:56084 
            // {
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

            SaveNotes();
            // }
            WFObjectManager obj_workFlow = new WFObjectManager();

            if (Request["Opening_from"] != null && Request["Opening_from"] != "Patient_Pane")
            {
                if (btnMoveToMa.Text == "Save & Move To Provider")
                {
                    if (Request["CurrentProcess"] != null && Request["CurrentProcess"].ToUpper().Trim() == "RESULT_REVIEW")
                    {
                        if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
                        {
                            //Old Code
                            //obj_workFlow.UpdateOwner(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, string.Empty);
                            //Gitlab# 2485 - Physician Name Display Change
                            //Jira CAP-2153
                            //obj_workFlow.UpdateOwner(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", cboMoveToMA.SelectedItem.Value, string.Empty);
                            obj_workFlow.UpdateOwner(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", hdnAssignTo.Value, string.Empty);
                        }
                        else
                        {
                            //Old Code
                            //obj_workFlow.UpdateOwner(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, string.Empty);
                            //Gitlab# 2485 - Physician Name Display Change
                            //Jira CAP-2153
                            //obj_workFlow.UpdateOwner(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", cboMoveToMA.SelectedItem.Value, string.Empty);
                            obj_workFlow.UpdateOwner(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", hdnAssignTo.Value, string.Empty);
                        }
                    }
                    else
                    {


                        //PhysicianManager objphymanager = new PhysicianManager();
                        //PhyUserList = objphymanager.GetPhysicianandUser(false, string.Empty);
                        //string sPhysicianName = string.Empty;
                        string[] current_process = new string[] { "MA_RESULTS" };
                        //if (PhyUserList != null && PhyUserList.UserList != null && PhyUserList.UserList.Count > 0)
                        //{
                        //    for (int i = 0; i < PhyUserList.UserList.Count; i++)
                        //    {
                        //        if (Convert.ToUInt64(PhyUserList.UserList[i].Physician_Library_ID) == Convert.ToUInt64(Request.QueryString["PhysicianId"]))
                        //        {
                        //            sPhysicianName = PhyUserList.UserList[i].user_name;
                        //            break;
                        //        }
                        //    }
                        //}

                        if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
                        {
                            //if (sPhysicianName == "")
                            //{
                            //    WFObject objWorkflowList = new WFObject();
                            //    objWorkflowList = obj_workFlow.GetByObjectSystemId(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT");
                            //    if (objWorkflowList != null)
                            //    {
                            //        sPhysicianName = objWorkflowList.Process_Allocation.Split('|')[1].Split('-')[1];
                            //    }

                            //}
                            //Old Code
                            //obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 2, cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
                            //Gitlab# 2485 - Physician Name Display Change
                            //Jira CAP-2153
                            //obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 2, cboMoveToMA.SelectedItem.Value, UtilityManager.ConvertToUniversal(), null, current_process, null);
                            obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 2, hdnAssignTo.Value, UtilityManager.ConvertToUniversal(), null, current_process, null);
                        }
                        else
                        {
                            //Old Code
                            //obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 2, cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
                            //Gitlab# 2485 - Physician Name Display Change
                            //Jira CAP-2153
                            //obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 2, cboMoveToMA.SelectedItem.Value, UtilityManager.ConvertToUniversal(), null, current_process, null);
                            obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 2, hdnAssignTo.Value, UtilityManager.ConvertToUniversal(), null, current_process, null);
                        }

                        //$muthusamy
                    }
                }
                else
                {
                    //Cap - 1247
                    if (Request["CurrentProcess"] != null && Request["CurrentProcess"].ToUpper().Trim() == "MA_RESULTS")
                    {
                        if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
                        {
                            //Old Code
                            //obj_workFlow.UpdateOwner(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, string.Empty);
                            //Gitlab# 2485 - Physician Name Display Change
                            //Jira CAP-2153
                            //obj_workFlow.UpdateOwner(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", cboMoveToMA.SelectedItem.Value, string.Empty);
                            obj_workFlow.UpdateOwner(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", hdnAssignTo.Value, string.Empty);
                        }
                        else
                        {
                            //Old Code
                            //obj_workFlow.UpdateOwner(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, string.Empty);
                            //Gitlab# 2485 - Physician Name Display Change
                            //Jira CAP-2153
                            //obj_workFlow.UpdateOwner(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", cboMoveToMA.SelectedItem.Value, string.Empty);
                            obj_workFlow.UpdateOwner(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", hdnAssignTo.Value, string.Empty);
                        }
                    }
                    else
                    {

                        //For Bug Id 56084-4.9.18
                        //if (cboMoveToMA.Text == string.Empty && cboMoveToMA.Visible == true)
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DisplayErrorMessage('115046'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                        //    return;
                        //}
                        string[] current_process = new string[] { "RESULT_REVIEW" };
                    IList<Encounter> lstenc = new List<Encounter>();
                    EncounterManager objencmanager = new EncounterManager();
                    if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
                    {
                            //Old Code
                            //obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 2, cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
                            //Gitlab# 2485 - Physician Name Display Change
                            //Jira CAP-2153
                            //obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 2, cboMoveToMA.SelectedItem.Value, UtilityManager.ConvertToUniversal(), null, current_process, null);
                            obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 2, hdnAssignTo.Value, UtilityManager.ConvertToUniversal(), null, current_process, null);
                        }
                    //commented for BugID:45893
                    //else if (Convert.ToUInt64(Request["EncounterId"]) != 0)
                    //{
                    //    lstenc = objencmanager.GetEncounterByEncounterID(Convert.ToUInt64(Request["EncounterId"]));
                    //    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 7, lstenc.Count > 0 ? lstenc[0].Assigned_Med_Asst_User_Name : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
                    //}
                    //else 
                    //{
                    //    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 7, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                    //}
                    //added for BgID:45893 to move item to respective MA's Queue.
                    else
                    {
                            //Old Code
                            //obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 7, cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
                            //Gitlab# 2485 - Physician Name Display Change
                            //Jira CAP-2153
                            //obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 7, cboMoveToMA.SelectedItem.Value, UtilityManager.ConvertToUniversal(), null, current_process, null);
                            obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 7, hdnAssignTo.Value, UtilityManager.ConvertToUniversal(), null, current_process, null);
                        }
                }
                }
            }
            else if (Request["Opening_from"] != null && Request["Opening_from"] == "Patient_Pane") //Opening from Left side Patient Pane
            {
                if (btnMoveToMa.Text == "Save & Move To Provider")
                {
                    if (hdnLeftPaneCurrentProcess.Value == "RESULT_REVIEW")
                    {
                        if (hdnLeftPaneObjType.Value == "DIAGNOSTIC_RESULT")
                        {
                            //Old Code
                            //obj_workFlow.UpdateOwner(Convert.ToUInt64(hdnLeftPaneResultMasterID.Value), "DIAGNOSTIC_RESULT", cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, string.Empty);
                            //Gitlab# 2485 - Physician Name Display Change
                            //Jira CAP-2153
                            //obj_workFlow.UpdateOwner(Convert.ToUInt64(hdnLeftPaneResultMasterID.Value), "DIAGNOSTIC_RESULT", cboMoveToMA.SelectedItem.Value, string.Empty);
                            obj_workFlow.UpdateOwner(Convert.ToUInt64(hdnLeftPaneResultMasterID.Value), "DIAGNOSTIC_RESULT", hdnAssignTo.Value, string.Empty);
                        }
                        else
                        {
                            //Old Code
                            //obj_workFlow.UpdateOwner(Convert.ToUInt64(hdnLeftPaneOrderSubmitID.Value), "DIAGNOSTIC ORDER", cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, string.Empty);
                            //Gitlab# 2485 - Physician Name Display Change
                            //Jira CAP-2153
                            //obj_workFlow.UpdateOwner(Convert.ToUInt64(hdnLeftPaneOrderSubmitID.Value), "DIAGNOSTIC ORDER", cboMoveToMA.SelectedItem.Value, string.Empty);
                            obj_workFlow.UpdateOwner(Convert.ToUInt64(hdnLeftPaneOrderSubmitID.Value), "DIAGNOSTIC ORDER", hdnAssignTo.Value, string.Empty);
                        }
                    }
                    //else
                    //{
                    //    string[] current_process = new string[] { "MA_RESULTS" };

                    //    if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
                    //    {
                    //        obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 2, cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
                    //    }
                    //    else
                    //        obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 2, cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
                    //}
                }
                else
                {

                    string[] current_process = new string[] { "RESULT_REVIEW" };
                    IList<Encounter> lstenc = new List<Encounter>();
                    EncounterManager objencmanager = new EncounterManager();
                    if (hdnLeftPaneObjType.Value == "DIAGNOSTIC_RESULT")
                    {
                        //Old Code
                        //obj_workFlow.MoveToNextProcess(Convert.ToUInt64(hdnLeftPaneResultMasterID.Value), "DIAGNOSTIC_RESULT", 2, cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
                        //Gitlab# 2485 - Physician Name Display Change
                        //Jira CAP-2153
                        //obj_workFlow.MoveToNextProcess(Convert.ToUInt64(hdnLeftPaneResultMasterID.Value), "DIAGNOSTIC_RESULT", 2, cboMoveToMA.SelectedItem.Value, UtilityManager.ConvertToUniversal(), null, current_process, null);
                        obj_workFlow.MoveToNextProcess(Convert.ToUInt64(hdnLeftPaneResultMasterID.Value), "DIAGNOSTIC_RESULT", 2, hdnAssignTo.Value, UtilityManager.ConvertToUniversal(), null, current_process, null);
                    }

                    else
                    {
                        //Old Code
                        //obj_workFlow.MoveToNextProcess(Convert.ToUInt64(hdnLeftPaneOrderSubmitID.Value), "DIAGNOSTIC ORDER", 7, cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
                        //Gitlab# 2485 - Physician Name Display Change
                        //Jira CAP-2153
                        //obj_workFlow.MoveToNextProcess(Convert.ToUInt64(hdnLeftPaneOrderSubmitID.Value), "DIAGNOSTIC ORDER", 7, cboMoveToMA.SelectedItem.Value, UtilityManager.ConvertToUniversal(), null, current_process, null);
                        obj_workFlow.MoveToNextProcess(Convert.ToUInt64(hdnLeftPaneOrderSubmitID.Value), "DIAGNOSTIC ORDER", 7, hdnAssignTo.Value, UtilityManager.ConvertToUniversal(), null, current_process, null);
                    }
                }
                btnMoveToNextProcess.Visible = false;
                btnMoveToMa.Visible = false;
                chkShowAll.Visible = false;
                //Jira CAP-2153
                //cboMoveToMA.Visible = false;
                txtAssignedTo.Visible = false;
                imgclearAssignTo.Visible = false;
                //Jira CAP-2153 - End
                Label1.Visible = false;
                chkPhyName.Visible = false;
                rdbMA.Visible = false;
                rdbProvider.Visible = false;
                //btnSave.Style["margin-left"] = "440px";
                // btnSave.Style["margin-left"] = "455px";
                //btnSave.Style["margin-left"] = "20.5%";    //"278px"; //"278px";   //"370px";
                //btnSave.Style["margin-top"] = "-4.2%";
                // btnEfax.Enabled = false;

            }

            
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Close", "End(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NextResult", "ViewNextResult(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);//BugID:41027 -- move to next result           
        }

        public void SaveNotes()
        {
            ResultMasterManager masterProxy = new ResultMasterManager();
            IList<ResultMaster> lstResultMaster = new List<ResultMaster>();
            ResultMaster objResultMaster = new ResultMaster();
            ResultMasterManager rsManager = new ResultMasterManager();
            ulong resMasID = 0;

            ulong ulFileManagementIndexID = 0;

            //For Bug Id 56084-4.9.18
            //if (ClientSession.UserRole.ToUpper().Trim() == "MEDICAL ASSISTANT" && txtMedicalAssistantNotes.Text=="")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "alert('Please Enter Medical Assistant Notes'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
            //    return;
            //}
            //if ((ClientSession.UserRole.ToUpper().Trim() == "PHYSICIAN" ||  ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT") && DLC.txtDLC.Text == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "alert('Please Enter Provider  Notes'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
            //    return;
            //}

            //CAP-2154
            ulong orderid = 0;
            ulong resultMasterId = 0;
            if(tvViewIndex?.SelectedNode != null)
            {
                ulong.TryParse(tvViewIndex.SelectedNode.Attributes["ResultMasterID"], out resultMasterId);
                ulong.TryParse(tvViewIndex.SelectedNode.Attributes["OrderSubmitId"], out orderid);
            }


            //if ((Session["Order_Id"] != null && Session["Order_Id"] != string.Empty && Convert.ToUInt32(Session["Order_Id"]) != 0))
            //if (Request["OrderSubmitId"] != null && Request["OrderSubmitId"] != string.Empty && Convert.ToUInt32(Request["OrderSubmitId"]) != 0)
            //CAP-2154
            if (orderid > 0)
            {
                lstResultMaster = rsManager.GetResultReviewNotesBasedOnOrderSubmitId(orderid);
                //lstResultMaster = rsManager.GetResultReviewNotesBasedOnOrderSubmitId(Convert.ToUInt32(Request["OrderSubmitId"]));
                if (lstResultMaster != null && lstResultMaster.Count > 0)
                {
                    if (lstResultMaster.Count == 1)
                    {
                        objResultMaster = lstResultMaster[0];
                    }
                    else if (lstResultMaster.Count > 1)
                    {
                        //CAP-2154
                        if (resultMasterId > 0)
                        {
                            //    IList<ResultMaster> lstResMaster = new List<ResultMaster>();
                            //    lstResMaster = lstResultMaster.Where(a => a.Id == resMasID).ToList<ResultMaster>();
                            //    if (lstResMaster != null && lstResMaster.Count > 0)
                            //        objResultMaster = lstResMaster[0];
                            //    else
                            //    {
                            //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DisplayErrorMessage('115058'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                            //        return;
                            //    }
                            objResultMaster = rsManager.GetById(resultMasterId);
                        }
                        else if (Request["ResultMasterID"] != null && UInt64.TryParse(Request["ResultMasterID"], out resMasID))
                        {
                            //IList<ResultMaster> lstResMaster = new List<ResultMaster>();
                            //lstResMaster = lstResultMaster.Where(a => a.Id == resMasID).ToList<ResultMaster>();
                            //if (lstResMaster != null && lstResMaster.Count > 0)
                            //    objResultMaster = lstResMaster[0];
                            //else
                            //{
                            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DisplayErrorMessage('115058'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                            //    return;
                            //}
                            objResultMaster = rsManager.GetById(resMasID);
                        }
                    }
                    if (objResultMaster != null && objResultMaster.Id != 0)
                    {
                        objResultMaster.Modified_By = ClientSession.UserName;
                        objResultMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    }
                }
                else
                {
                    objResultMaster = new ResultMaster();
                    objResultMaster.PID_External_Patient_ID = Convert.ToString(Request["HumanId"]);
                    objResultMaster.PID_Alternate_Patient_ID = objResultMaster.PID_External_Patient_ID;
                    objResultMaster.Created_By = ClientSession.UserName;
                    objResultMaster.Order_ID = Convert.ToUInt32(Session["Order_Id"]);  //Request["OrderSubmitId"]);
                    objResultMaster.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    //jira cap-498
                    objResultMaster.Matching_Patient_Id = Convert.ToUInt32(Request["HumanId"]);
                }
            }
            //CAP-2154
            else if (resultMasterId > 0)
            {
                IList<ResultMaster> lstResMaster = new List<ResultMaster>();
                objResultMaster = rsManager.GetById(resultMasterId);
                if (objResultMaster != null && objResultMaster.Id != 0)
                {
                    objResultMaster.Modified_By = ClientSession.UserName;
                    objResultMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
                else
                {
                    objResultMaster = new ResultMaster();
                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DisplayErrorMessage('115058'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                    //return;
                }
            }
            else if (Request["ResultMasterID"] != null && UInt64.TryParse(Request["ResultMasterID"], out resMasID) && resMasID != 0)
            {
                IList<ResultMaster> lstResMaster = new List<ResultMaster>();
                objResultMaster = rsManager.GetById(resMasID);
                if (objResultMaster != null && objResultMaster.Id != 0)
                {
                    objResultMaster.Modified_By = ClientSession.UserName;
                    objResultMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DisplayErrorMessage('115057'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                    return;
                }
            }

            if (objResultMaster != null)
            {
                string sNote = (DLC.txtDLC.Text.Trim() != string.Empty ? DLC.txtDLC.Text : txtProvNoteshistory.Text);
                if (DLC.txtDLC.Text.Trim() != string.Empty)
                {
                    if (objResultMaster.Id == 0)
                    {
                        objResultMaster = new ResultMaster();
                        objResultMaster.PID_External_Patient_ID = Convert.ToString(Request["HumanId"]);
                        objResultMaster.PID_Alternate_Patient_ID = objResultMaster.PID_External_Patient_ID;
                        objResultMaster.Created_By = ClientSession.UserName;
                        if (tvViewIndex.SelectedNode.Attributes["OrderSubmitId"] != null)
                            objResultMaster.Order_ID = Convert.ToUInt32(tvViewIndex.SelectedNode.Attributes["OrderSubmitId"]); //Convert.ToUInt32(Request["OrderSubmitId"]);
                        objResultMaster.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        objResultMaster.Matching_Patient_Id = Convert.ToUInt64(objResultMaster.PID_External_Patient_ID);
                        ulFileManagementIndexID = Convert.ToUInt64(Session["Key_id"]);
                    }
                    //jira cap-498
                    else
                    {
                        ulFileManagementIndexID = Convert.ToUInt64(Session["Key_id"]);
                    }
                    if (hdnSubDocumentType.Value != string.Empty && sNotes != null && sNotes.Count() > 0 && (sNotes.Contains(hdnSubDocumentType.Value.ToString()))) // && sNote.Contains("Test Reviewed: ") == true)
                    {
                        // objResultMaster.Result_Review_Comments = sNote;
                        //objResultMaster.Result_Review_Comments = (txtProvNoteshistory.Text.Trim() != string.Empty ? txtProvNoteshistory.Text + "<br/>" : string.Empty) + "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + "[[[" + DLC.txtDLC.Text + "]]]";

                        if (sNote.Contains("Test Reviewed: ") == true)
                        {
                            string notes = "";

                            if (txtProvNoteshistory.Attributes["InterpretationText"] != null)
                                notes = txtProvNoteshistory.Attributes["InterpretationText"].Trim();
                            string textboxnotes = DLC.txtDLC.Text;
                            string testname = textboxnotes.Split(new string[] { "Test Reviewed:" }, StringSplitOptions.None)[1].Split(';')[0];
                            string NotesHistory = "";
                            //Cap - 686
                            Boolean DlcUpdate = false;
                            //Cap - 1099
                            //string[] Result_Review = objResultMaster.Result_Review_Comments.Split(new string[] { "]]]" }, StringSplitOptions.None);
                            string[] Result_Review = objResultMaster.Result_Review_Comments.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                            for (int i = 0; i < Result_Review.Length; i++)
                            {
                                //Cap - 907
                                //if (Result_Review[i].Trim() != string.Empty && Result_Review[i].Split(';')[0].Split(':')[3].Trim() == testname.Trim())
                                if (Result_Review[i].Trim() != string.Empty && Result_Review[i].Contains("Test Reviewed: ") == true && Result_Review[i].Split(';')[0].Length > 0 && Result_Review[i].Split(';')[0].Split(':').Length > 3 && Result_Review[i].Split(';')[0].Split(':')[3].Trim() == testname.Trim())
                                {
                                    DlcUpdate = true;
                                    break;
                                }

                            }
                            //Cap - 686
                            //if (notes.IndexOf(testname) >= 0)
                            if (DlcUpdate == true)
                            {
                                //Cap - 1099
                                //string[] Result_Review_Comments = objResultMaster.Result_Review_Comments.Split(new string[] { "]]]" }, StringSplitOptions.None);
                                string[] Result_Review_Comments = objResultMaster.Result_Review_Comments.Split(new string[] { "<br/>" }, StringSplitOptions.None);

                                for (int i = 0; i < Result_Review_Comments.Length; i++)
                                {
                                    //Cap - 686
                                    //if (Result_Review_Comments[i].Trim() != string.Empty && Result_Review_Comments[i].IndexOf(testname) >= 0)
                                    //CAP-1042
                                    //if (Result_Review_Comments[i].Trim() != string.Empty && Result_Review_Comments[i].Split(';')[0].Split(':')[3].Trim() == testname.Trim())
                                    if (Result_Review_Comments[i].Trim() != string.Empty && Result_Review_Comments[i].Split(';')[0].Split(':').Length > 3 && Result_Review_Comments[i].Split(';')[0].Split(':')[3].Trim() == testname.Trim())
                                    {
                                        objResultMaster.Result_Review_Comments = objResultMaster.Result_Review_Comments.Replace(Result_Review_Comments[i].Substring(Result_Review_Comments[i].IndexOf("[[[") + 3, Result_Review_Comments[i].Length - Result_Review_Comments[i].IndexOf("[[[") - 3), DLC.txtDLC.Text + "]]]");
                                        //Cap - 1099
                                        if (objResultMaster.Result_Review_Comments.EndsWith("]]]") == false)
                                        {
                                            objResultMaster.Result_Review_Comments = objResultMaster.Result_Review_Comments + "]]]";
                                        }
                                        txtProvNoteshistory.Attributes.Add("InterpretationText", txtProvNoteshistory.Attributes["InterpretationText"].Trim().Replace(Result_Review_Comments[i].Substring(Result_Review_Comments[i].IndexOf("[[[") + 3, Result_Review_Comments[i].Length - Result_Review_Comments[i].IndexOf("[[[") - 3).Replace("]]]", ""), DLC.txtDLC.Text));

                                        if (Result_Review_Comments[i].Trim() != string.Empty && Result_Review_Comments[i].Contains("Test Reviewed: ") == true)
                                            NotesHistory = NotesHistory + Result_Review_Comments[i].Substring(0, Result_Review_Comments[i].IndexOf(";")).Replace("[[[Test Reviewed: ", "");
                                        else if (Result_Review_Comments[i].Trim() != string.Empty)
                                        {
                                            NotesHistory = NotesHistory + Result_Review_Comments[i];
                                        }
                                    }

                                }

                                // objResultMaster.Result_Review_Comments.Split(new string[] { "]]]" }, StringSplitOptions.None)[0].Substring(objResultMaster.Result_Review_Comments.Split(new string[] { "]]]" }, StringSplitOptions.None)[0].IndexOf("[[[")+3, objResultMaster.Result_Review_Comments.Split(new string[] { "]]]" }, StringSplitOptions.None)[0].Length-objResultMaster.Result_Review_Comments.Split(new string[] { "]]]" }, StringSplitOptions.None)[0].IndexOf("[[[")-3)
                            }
                            else
                            {


                                objResultMaster.Result_Review_Comments = (objResultMaster.Result_Review_Comments.Trim() != string.Empty ? objResultMaster.Result_Review_Comments + "<br/>" : string.Empty) + "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + "[[[" + DLC.txtDLC.Text + "]]]";



                                string[] reviewcomments = objResultMaster.Result_Review_Comments.Split(new string[] { "]]]" }, StringSplitOptions.None);

                                for (int i = 0; i < reviewcomments.Length; i++)
                                {
                                    if (reviewcomments[i].Trim() != string.Empty)
                                    {
                                        if (reviewcomments[i].Contains("Test Reviewed: ") == true)

                                            NotesHistory = NotesHistory + reviewcomments[i].Substring(0, reviewcomments[i].IndexOf(";")).Replace("[[[Test Reviewed: ", "");
                                        else
                                            NotesHistory = NotesHistory + reviewcomments[i];
                                    }
                                }
                                txtProvNoteshistory.Text = NotesHistory.Replace("<br/>", "\n"); ;// objResultMaster.Result_Review_Comments.Substring(0, objResultMaster.Result_Review_Comments.IndexOf(";")).Replace("[[[Test Reviewed: ", "");
                                if (txtProvNoteshistory.Attributes["InterpretationText"] != null)
                                    txtProvNoteshistory.Attributes.Add("InterpretationText", txtProvNoteshistory.Attributes["InterpretationText"].Trim() + "<br/>" + DLC.txtDLC.Text);
                                // txtProvNoteshistory.Attributes.Add("InterpretationText", objResultMaster.Result_Review_Comments.Substring(objResultMaster.Result_Review_Comments.IndexOf("[[[") + 3, objResultMaster.Result_Review_Comments.IndexOf("]]]") - objResultMaster.Result_Review_Comments.IndexOf("[[[") - 3));
                                else
                                    txtProvNoteshistory.Attributes.Add("InterpretationText", DLC.txtDLC.Text);

                            }
                        }
                        else
                        {
                            string NotesHistory = "";
                            objResultMaster.Result_Review_Comments = (objResultMaster.Result_Review_Comments.Trim() != string.Empty ? objResultMaster.Result_Review_Comments + "<br/>" : string.Empty) + "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + DLC.txtDLC.Text;

                            string[] reviewcomments = objResultMaster.Result_Review_Comments.Split(new string[] { "]]]" }, StringSplitOptions.None);

                            for (int i = 0; i < reviewcomments.Length; i++)
                            {
                                if (reviewcomments[i].Trim() != string.Empty)
                                {
                                    if (reviewcomments[i].Contains("Test Reviewed: ") == true)

                                        NotesHistory = NotesHistory + reviewcomments[i].Substring(0, reviewcomments[i].IndexOf(";")).Replace("[[[Test Reviewed: ", "");
                                    else
                                        NotesHistory = NotesHistory + reviewcomments[i];
                                }
                            }
                            txtProvNoteshistory.Text = NotesHistory.Replace("<br/>", "\n"); ;
                            // txtProvNoteshistory.Text = objResultMaster.Result_Review_Comments.Replace("<br/>", "\n");

                            //if (txtProvNoteshistory.Attributes["InterpretationText"] != null)
                            //    txtProvNoteshistory.Attributes.Add("InterpretationText", txtProvNoteshistory.Attributes["InterpretationText"].Trim() + "<br/>" + DLC.txtDLC.Text);
                            //// txtProvNoteshistory.Attributes.Add("InterpretationText", objResultMaster.Result_Review_Comments.Substring(objResultMaster.Result_Review_Comments.IndexOf("[[[") + 3, objResultMaster.Result_Review_Comments.IndexOf("]]]") - objResultMaster.Result_Review_Comments.IndexOf("[[[") - 3));
                            //else
                            //    txtProvNoteshistory.Attributes.Add("InterpretationText", DLC.txtDLC.Text);
                        }
                    }
                    else
                    {
                        objResultMaster.Result_Review_Comments = (txtProvNoteshistory.Text.Trim() != string.Empty ? txtProvNoteshistory.Text + "<br/>" : string.Empty) + "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + DLC.txtDLC.Text;
                        txtProvNoteshistory.Text = objResultMaster.Result_Review_Comments.Replace("<br/>", "\n");
                    }

                    DLC.txtDLC.Text = string.Empty;
                    DLC.txtDLC.Enabled = true;

                    DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("White");
                    DLC.pbDropdown.Attributes.Remove("class");
                    DLC.pbDropdown.Attributes.Add("class", "pbDropdownBackground");
                }
                else
                {
                    //For Bug Id 56084-4.9.18
                    if (DLC.txtDLC.Enabled == true)
                    {
                        //if (hdnSubDocumentType.Value != string.Empty && sNotes != null && sNotes.Count() > 0 && (sNotes.Contains(hdnSubDocumentType.Value.ToString())) && sNote.Contains("Test Reviewed: ") == true)
                        //{
                        //    objResultMaster.Result_Review_Comments = sNote;
                        //}
                        //else
                        if (!objResultMaster.Result_Review_Comments.Contains("@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + "[No Comments]"))
                        {
                            objResultMaster.Result_Review_Comments = (objResultMaster.Result_Review_Comments.Trim() != string.Empty ? objResultMaster.Result_Review_Comments + "<br/>" : string.Empty) + "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + "[No Comments]";
                            //Cap - 1054
                            //txtProvNoteshistory.Text = objResultMaster.Result_Review_Comments.Replace("<br/>", "\n");
                            if (txtProvNoteshistory.Text != "")
                            {
                                txtProvNoteshistory.Text = txtProvNoteshistory.Text + "\n@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + "[No Comments]";
                            }
                            else
                            {
                                txtProvNoteshistory.Text = "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + "[No Comments]";
                            }

                            DLC.txtDLC.Text = string.Empty;
                        }
                    }

                }
                if (txtMedicalAssistantNotes.Text.Trim() != string.Empty)
                {
                    if (objResultMaster.Id == 0)
                    {
                        objResultMaster = new ResultMaster();
                        if (Request["HumanId"] != null)
                            objResultMaster.PID_External_Patient_ID = Convert.ToString(Request["HumanId"]);
                        objResultMaster.PID_Alternate_Patient_ID = objResultMaster.PID_External_Patient_ID;
                        objResultMaster.Created_By = ClientSession.UserName;
                        //if (Request["OrderSubmitId"] != null)
                        //    objResultMaster.Order_ID = Convert.ToUInt32(Request["OrderSubmitId"]);
                        if (tvViewIndex.SelectedNode.Attributes["OrderSubmitId"] != null)
                            objResultMaster.Order_ID = Convert.ToUInt32(tvViewIndex.SelectedNode.Attributes["OrderSubmitId"]);
                        objResultMaster.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        objResultMaster.Matching_Patient_Id = Convert.ToUInt64(objResultMaster.PID_External_Patient_ID);
                        ulFileManagementIndexID = Convert.ToUInt64(Session["Key_id"]);
                    }
                    //jira cap-498
                    else
                    {
                        ulFileManagementIndexID = Convert.ToUInt64(Session["Key_id"]);
                    }
                    objResultMaster.MA_Notes = (txtMedNoteshistory.Text.Trim() != string.Empty ? txtMedNoteshistory.Text + "<br/>" : string.Empty) + "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + txtMedicalAssistantNotes.Text;
                    txtMedNoteshistory.Text = objResultMaster.MA_Notes.Replace("<br/>", "\n");
                    txtMedicalAssistantNotes.Text = string.Empty;
                }
                else
                {
                    //For Bug Id 56084-4.9.18
                    if (txtMedicalAssistantNotes.Enabled == true)
                    {
                        objResultMaster.MA_Notes = (txtMedNoteshistory.Text.Trim() != string.Empty ? txtMedNoteshistory.Text + "<br/>" : string.Empty) + "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + "[No Comments]";
                        txtMedNoteshistory.Text = objResultMaster.MA_Notes.Replace("<br/>", "\n");
                        txtMedicalAssistantNotes.Text = string.Empty;
                    }
                }
                if (chkPhyName.Checked == true)
                {
                    objResultMaster.Reviewed_Provider_Sign_ID = Convert.ToInt32(ClientSession.PhysicianId);
                }
                objResultMaster = rsManager.SaveResultMasterItem(objResultMaster, ulFileManagementIndexID);
                //tvViewIndex.SelectedNode.Attributes.Add("ResultMasterID", objResultMaster.Id.ToString());
                Session["Notes"] = objResultMaster;
            }
            //Jira CAP-2153
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "AutoSave", "SaveViewResults();", true);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "AutoSave", "SaveViewResults();OnPageLoad(true);", true);
            //Jira CAP-2153 - End
            btnSave.Disabled = true;
            hdnSave.Value = "false";
        }       

        protected void tabView_TabClick(object sender, RadTabStripEventArgs e)
        {
            IList<FileManagementIndex> filelist = new List<FileManagementIndex>();
            filelist = (IList<FileManagementIndex>)Session["filelist"];

            if (e.Tab.Text == "Scanned Results")
            {
                ulong order_id = (ulong)Session["Order_Id"];

                IList<string> lstScanId = ((from doc in filelist
                                            where doc.Order_ID == order_id
                                            select doc.File_Path).Distinct()).ToList<string>();
                if (lstScanId != null && lstScanId.Count > 0)
                {
                    PageViewScan.ContentUrl = "frmImageViewer.aspx?FilePath=" + lstScanId[0].ToString().Replace("#", "HASHSYMBOL") + "&Source=RESULT" + "&HumanId=" + Request["HumanId"].ToString() + "&Doc=" + cboDocumentType.Text + "&DDate=" + tvViewIndex.SelectedNode.Text.ToString();
                    PageViewScan.Selected = true;
                }


            }

            else if (e.Tab.Text == "Result Files")
            {

                IList<string> lstScanId = ((from doc in filelist
                                            where doc.Order_ID == Convert.ToUInt32(Request["OrderSubmitId"])
                                            select doc.File_Path).Distinct()).ToList<string>();
                if (lstScanId != null && lstScanId.Count > 0)
                {
                    PageViewResultFiles.ContentUrl = "frmImageViewer.aspx?Source=RESULT&FilePath=" + lstScanId[0].ToString();
                    PageViewResultFiles.Selected = true;
                }


            }
            else if (e.Tab.Text == "Results")
            {
                ulong result_id = (ulong)Session["Result_Master_Id"];
                PageViewResult.ContentUrl = "frmResult.aspx?Result_Master_ID=" + result_id.ToString() + "&strScreenName=PATIENT CHART";
                PageViewResult.Selected = true;

            }
            else if (e.Tab.Text == "ABI Results")
            {
                PageViewABIResults.ContentUrl = "frmABIResult.aspx?HumanId=" + Request["HumanId"].ToString();
                PageViewABIResults.Selected = true;

            }
            else if (e.Tab.Text == "Spirometry Results")
            {
                PageViewSpirometryResults.ContentUrl = "frmSpirometry.aspx?HumanId=" + Request["HumanId"].ToString();
                PageViewSpirometryResults.Selected = true;

            }
            else if (e.Tab.Text == "Scanned Results")
            {
                tb.Text = "Message Log";

            }
            //BugID:47602
            else if (e.Tab.Target == "Result Master")
            {
                PDFGenerator objPDFGenerator = new PDFGenerator();
                if (!Directory.Exists(Server.MapPath("Documents/" + Session.SessionID)))
                    Directory.CreateDirectory(Server.MapPath("Documents/" + Session.SessionID));

                string filepath = Server.MapPath("Documents/" + Session.SessionID) + "\\";
                hdnSelectedItem.Value = String.Empty;
                string file = string.Empty;
                string filename = objPDFGenerator.GenerateRequestionForLabcorp(Convert.ToUInt32(e.Tab.Value), filepath);

                string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                string[] FileName = filename.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                if (FileName != null && FileName.Length > 0)
                    file = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                if (hdnSelectedItem.Value == string.Empty)
                {
                    if (FileName != null && FileName.Length > 0)
                    {
                        hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                        txtFileInformation.Value = FileName[0].ToString().Replace("//", "").Replace("\\", "") + " | " + "Results";
                    }

                    //tb.Value = order_id.ToString();
                    //tabView.Tabs.Add(tb);
                }
                else
                {
                    if (FileName != null && FileName.Length > 0)
                    {
                        hdnSelectedItem.Value += "|" + FileName[0].ToString();
                    }
                }
                PageViewResult.ContentUrl = hdnSelectedItem.Value.ToString();
                PageViewResult.Selected = true;

                ResultMaster objresultmaster = new ResultMaster();
                ResultMasterManager masterProxy = new ResultMasterManager();
                objresultmaster = masterProxy.GetById(Convert.ToUInt64(e.Tab.Value));
                if (objresultmaster != null && objresultmaster.Id != 0)
                {
                    hdnFaxpath.Value = filename + "|" + objresultmaster.Order_ID;
                    txtProvNoteshistory.Text = objresultmaster.Result_Review_Comments;
                    txtProvNoteshistory.Text = txtProvNoteshistory.Text.Replace("<br/>", "\n");
                    txtMedNoteshistory.Text = objresultmaster.MA_Notes;
                    txtMedNoteshistory.Text = txtMedNoteshistory.Text.Replace("<br/>", "\n");
                    Session["Notes"] = objresultmaster;
                    Session["Result_Master_Id"] = objresultmaster.Id;
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ulong Result_Master_ID;
            Result_Master_ID = (ulong)Session["Result_Master_Id"];
            PDFGenerator objPDFGenerator = new PDFGenerator();
            string filepath = string.Empty;
            if (!Directory.Exists(Server.MapPath("Documents/" + Session.SessionID)))
                Directory.CreateDirectory(Server.MapPath("Documents/" + Session.SessionID));
            if (Directory.Exists(Server.MapPath("Documents/" + Session.SessionID)))
                filepath = Server.MapPath("Documents/" + Session.SessionID) + "\\";
            hdnSelectedItem.Value = String.Empty;
            string file = string.Empty;
            string filename = objPDFGenerator.GenerateRequestionForLabcorp(Result_Master_ID, filepath);
            string[] Split = null;
            if (Directory.Exists(Server.MapPath("Documents/" + Session.SessionID)))
                Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
            string[] FileName = filename.Split(Split, StringSplitOptions.RemoveEmptyEntries);
            if (FileName != null && FileName.Length > 0)
                file = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
            // string loc = "DYNAMIC";
            if (hdnSelectedItem.Value == string.Empty)
            {
                if (FileName != null && FileName.Length > 0)
                    hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
            }
            else
            {
                if (FileName != null && FileName.Length > 0)
                    hdnSelectedItem.Value += "|" + FileName[0].ToString();
            }
            RadScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenPDF();", true);

        }

        protected void btnPrintEducatnMaterial_Click(object sender, EventArgs e)
        {
            ulong Result_Master_ID;
            string urls = "";
            string value = string.Empty;
            bool ShowAlert = true;
            //BugID:48680
            ResultMaster objresMaster = new ResultMaster();
            if (Session["Notes"] != null)
            {
                objresMaster = (ResultMaster)Session["Notes"];
                Result_Master_ID = objresMaster.Id;
                ResultOBXManager obxMgr = new ResultOBXManager();
                IList<ResultOBX> ResultOBXList = new List<ResultOBX>();
                ResultOBXList = obxMgr.GetResultByMasterID(Result_Master_ID);
                if (ResultOBXList != null && ResultOBXList.Count > 0)
                {
                    for (int i = 0; i < ResultOBXList.Count; i++)
                    {
                        value = ResultOBXList[i].OBX_Loinc_Identifier;
                        if (value != "")
                        {
                            urls += value + ";";
                        }
                    }
                }

                if (urls.Trim() != String.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "openLink('" + urls + "');", true);
                    ShowAlert = false;
                }
                else
                {
                    ShowAlert = true;
                }
            }
            if (ShowAlert)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "No Valid Loinc Observation", "DisplayErrorMessage('115058'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
            }

            #region OLD CODE
            /* 
            ResultMasterManager objResultMasterManager = new ResultMasterManager();
            IList<ResultMaster> result_master_list = new List<ResultMaster>();
            result_master_list = (IList<ResultMaster>)Session["result_master_list"];
            Result_Master_ID = result_master_list[0].Id;
            FillResultDTO objFillResultDTO = new FillResultDTO();
            Stream strm = null;
            var serializer = new NetDataContractSerializer();
            strm = objResultMasterManager.GetResultByResultMasterID(Result_Master_ID);
            object ol = (object)serializer.ReadObject(strm);
            objFillResultDTO = ol as FillResultDTO;

            for (int i = 0; i < objFillResultDTO.ResultOBXList.Count; i++)
            {
                value = objFillResultDTO.ResultOBXList[i].OBX_Loinc_Identifier;
                //if (value != "")
                //{
                //    string pageurl = "http://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.1&mainSearchCriteria.v.c=" + value + "&informationRecipient.languageCode.c = en";
                //    // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "window.open('" + pageurl + "','_blank');", true);
                //    urls += pageurl + ";";

                //}          
                if (value != "")
                {
                    urls += value + ";";
                }

            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "openLink('" + urls + "');", true);
         */
            #endregion
        }

        protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            //Cap - 1374
            btnSave.Disabled = false;
            //Cap - 1696
            hdnSave.Value = "true";

            FillCombobox();
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    SaveNotes();
        //}
        #endregion

        protected void btnSave_Click1(object sender, EventArgs e)
        {
            SaveNotes();
        }

        protected void rdbMA_CheckedChanged(object sender, EventArgs e)
        {
            //Cap - 1696
            btnSave.Disabled=false;
            hdnSave.Value = "true";
            //Jira CAP-2153
            //FillCombobox();
            btnMoveToMa.Text = "Save & Move To MA";
            //btnSave.Style["margin-left"] = "308px"; //"293px"; //"349px";  //"443px";
            //btnSave.Style["margin-top"] = "-56px";
            //Jira CAP-2153
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();OnPageLoad(true);}", true);

        }

        protected void rdbProvider_CheckedChanged(object sender, EventArgs e)
        {
            //Cap - 1696
            btnSave.Disabled = false;
            hdnSave.Value = "true";
            //Jira CAP-2153
            //FillCombobox();
            btnMoveToMa.Text = "Save & Move To Provider";
            // btnSave.Style["margin-left"] = "329px"; //"370px"; //"459px";
            //Jira CAP-2153
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();OnPageLoad(true);}", true);

        }

        void FillCombobox()
        {
            string sRole = string.Empty;
            if (rdbMA.Checked == true)
            {
                sRole = "MEDICAL ASSISTANT";
            }
            else if (rdbProvider.Checked == true)
            {
                sRole = "PHYSICIAN";
            }


            if (chkShowAll.Visible == true)
            {
                //Jira CAP-2153
                //XDocument xmlUser = null;
                //XDocument xmlPhysician = null;
                //SortedDictionary<string, string> hashphyList = new SortedDictionary<string, string>();
                //if (File.Exists(Server.MapPath(@"ConfigXML\User.xml")))
                //    xmlUser = XDocument.Load(Server.MapPath(@"ConfigXML\User.xml"));
                //if (File.Exists(Server.MapPath(@"ConfigXML\PhysicianAddressDetails.xml")))
                //    xmlPhysician = XDocument.Load(Server.MapPath(@"ConfigXML\PhysicianAddressDetails.xml"));
                //cboMoveToMA.Items.Clear();
                //cboMoveToMA.Items.Add(new RadComboBoxItem(""));
                //if (xmlUser != null)
                //{
                //    if (chkShowAll.Checked)
                //    {
                //        foreach (XElement elements in xmlUser.Descendants("UserList"))
                //        {
                //            foreach (XElement UserElement in elements.Elements())
                //            {
                //                if (UserElement.Attribute("Role").Value.ToUpper().Contains(sRole) == true && UserElement.Attribute("User_Name").Value.ToUpper() != ClientSession.UserName && UserElement.Attribute("Legal_Org").Value.ToUpper() == ClientSession.LegalOrg)
                //                {
                //                    //Old Code
                //                    //string xmlValue = UserElement.Attribute("User_Name").Value + "-" + UserElement.Attribute("person_name").Value;
                //                    //cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
                //                    //Gitlab# 2485 - Physician Name Display Change
                //                    if (sRole == "MEDICAL ASSISTANT")
                //                    {
                //                        //string xmlValue = UserElement.Attribute("person_name").Value;
                //                        if (hashphyList.ContainsKey(UserElement.Attribute("person_name").Value) == false)
                //                        {
                //                            hashphyList.Add(UserElement.Attribute("person_name").Value, UserElement.Attribute("User_Name").Value);
                //                        }
                //                    }
                //                    else
                //                    {
                //                        foreach (XElement element in xmlPhysician.Descendants("p" + UserElement.Attribute("Physician_Library_ID").Value))
                //                        {
                //                            string phyName = string.Empty;
                //                            string username = string.Empty;
                //                            string prefix = string.Empty;
                //                            string firstname = string.Empty;
                //                            string middlename = string.Empty;
                //                            string lastname = string.Empty;
                //                            string suffix = string.Empty;
                //                            //string phyID = string.Empty;

                //                            if (element.Attribute("Physician_prefix").Value != null)
                //                                prefix = element.Attribute("Physician_prefix").Value;
                //                            if (element.Attribute("Physician_First_Name").Value != null)
                //                                firstname = element.Attribute("Physician_First_Name").Value;
                //                            if (element.Attribute("Physician_Middle_Name").Value != null)
                //                                middlename = element.Attribute("Physician_Middle_Name").Value;
                //                            if (element.Attribute("Physician_Last_Name").Value != null)
                //                                lastname = element.Attribute("Physician_Last_Name").Value;
                //                            if (element.Attribute("Physician_Suffix").Value != null)
                //                                suffix = element.Attribute("Physician_Suffix").Value;
                //                            //if (element.Attribute("ID").Value != null)
                //                            //    phyID = element.Attribute("ID").Value;

                //                            if (lastname != String.Empty)
                //                                phyName += lastname;
                //                            if (firstname != String.Empty)
                //                            {
                //                                if (phyName != String.Empty)
                //                                    phyName += "," + firstname;
                //                                else
                //                                    phyName += firstname;
                //                            }
                //                            if (middlename != String.Empty)
                //                                phyName += " " + middlename;
                //                            if (suffix != String.Empty)
                //                                phyName += "," + suffix;

                //                            if (hashphyList.ContainsKey(phyName) == false)
                //                            {
                //                                hashphyList.Add(phyName, UserElement.Attribute("User_Name").Value);
                //                            }
                //                        }
                //                    }


                //                    //if (Request["PhysicianId"] != null && Convert.ToUInt32(Request["PhysicianId"]) != 0 && UserElement.Attribute("Physician_Library_ID").Value == Request["PhysicianId"].ToString())
                //                    //{
                //                    //    cboMoveToMA.SelectedValue = xmlValue;
                //                    //}
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        foreach (XElement elements in xmlUser.Descendants("UserList"))
                //        {
                //            foreach (XElement UserElement in elements.Elements())
                //            {
                //                if (UserElement.Attribute("Default_Facility").Value.ToUpper() == ClientSession.FacilityName.ToUpper() && UserElement.Attribute("Role").Value.ToUpper().Contains(sRole) == true && UserElement.Attribute("User_Name").Value.ToUpper() != ClientSession.UserName && UserElement.Attribute("Legal_Org").Value.ToUpper() == ClientSession.LegalOrg)
                //                {
                //                    //Old Code
                //                    //string xmlValue = UserElement.Attribute("User_Name").Value + "-" + UserElement.Attribute("person_name").Value;
                //                    //cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
                //                    //Gitlab# 2485 - Physician Name Display Change
                //                    if (sRole == "MEDICAL ASSISTANT")
                //                    {
                //                        //string xmlValue = UserElement.Attribute("person_name").Value;
                //                        if (hashphyList.ContainsKey(UserElement.Attribute("person_name").Value) == false)
                //                        {
                //                            hashphyList.Add(UserElement.Attribute("person_name").Value, UserElement.Attribute("User_Name").Value);
                //                        }
                //                    }
                //                    else
                //                    {
                //                        foreach (XElement element in xmlPhysician.Descendants("p" + UserElement.Attribute("Physician_Library_ID").Value))
                //                        {
                //                            string phyName = string.Empty;
                //                            string prefix = string.Empty;
                //                            string firstname = string.Empty;
                //                            string middlename = string.Empty;
                //                            string lastname = string.Empty;
                //                            string suffix = string.Empty;
                //                            //string phyID = string.Empty;

                //                            if (element.Attribute("Physician_prefix").Value != null)
                //                                prefix = element.Attribute("Physician_prefix").Value;
                //                            if (element.Attribute("Physician_First_Name").Value != null)
                //                                firstname = element.Attribute("Physician_First_Name").Value;
                //                            if (element.Attribute("Physician_Middle_Name").Value != null)
                //                                middlename = element.Attribute("Physician_Middle_Name").Value;
                //                            if (element.Attribute("Physician_Last_Name").Value != null)
                //                                lastname = element.Attribute("Physician_Last_Name").Value;
                //                            if (element.Attribute("Physician_Suffix").Value != null)
                //                                suffix = element.Attribute("Physician_Suffix").Value;
                //                            //if (element.Attribute("ID").Value != null)
                //                            //    phyID = element.Attribute("ID").Value;

                //                            if (lastname != String.Empty)
                //                                phyName += lastname;
                //                            if (firstname != String.Empty)
                //                            {
                //                                if (phyName != String.Empty)
                //                                    phyName += "," + firstname;
                //                                else
                //                                    phyName += firstname;
                //                            }
                //                            if (middlename != String.Empty)
                //                                phyName += " " + middlename;
                //                            if (suffix != String.Empty)
                //                                phyName += "," + suffix;

                //                            if (hashphyList.ContainsKey(phyName) == false)
                //                            {
                //                                hashphyList.Add(phyName, UserElement.Attribute("User_Name").Value);
                //                            }
                //                        }
                //                    }

                //                    //if (Request["PhysicianId"] != null && Convert.ToUInt32(Request["PhysicianId"]) != 0 && UserElement.Attribute("Physician_Library_ID").Value == Request["PhysicianId"].ToString())
                //                    //{
                //                    //    cboMoveToMA.SelectedValue = xmlValue;
                //                    //}
                //                }
                //            }
                //        }
                //    }
                //}

                //foreach (var item in hashphyList)
                //{
                //    cboMoveToMA.Items.Add(new Telerik.Web.UI.RadComboBoxItem(item.Key, item.Value));
                //}
                //Jira CAP-2153 -End

                btnPrintEducatnMaterial.Visible = true;

            }
        }

        //protected void hdnSaveEnable_Click(object sender, EventArgs e)
        //{
        //    btnSave.Enabled = true;
        //}

        string ExtractBetween(string text, string start, string end)
        {
            int iStart = text.IndexOf(start);
            iStart = (iStart == -1) ? 0 : iStart + start.Length;
            int iEnd = text.LastIndexOf(end);
            if (iEnd == -1)
            {
                iEnd = text.Length;
            }
            int len = iEnd - iStart;

            return text.Substring(iStart, len);
        }
        protected void btnPrintInt_ServerClick(object sender, EventArgs e)
        {
            if (DLC.txtDLC.Text != "")
            {
                //Cap - 1537
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "PrintError", "DisplayErrorMessage('115062');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PrintError", "DisplayErrorMessage('115062');{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OnPageLoad", "OnPageLoad(false);", true);
                btnSave.Disabled = false;
                return;
            }

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Startloadinggrid", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);

            PrintOrders print = new PrintOrders();
            string sOutput = string.Empty;

            string sDirPath = Server.MapPath("Documents/" + Session.SessionID);

            DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            string TargetFileDirectory = Server.MapPath("Documents\\" + Session.SessionID);
            ResultMaster objresMaster = new ResultMaster();
            int iSignedPhysicianId = 0;
            string sSignDate = string.Empty;
            string sFacilityAddress = string.Empty; //ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper(); //ClientSession.FacilityName 

            if (Session["Notes"] != null)
            {
                objresMaster = (ResultMaster)Session["Notes"];
                iSignedPhysicianId = objresMaster.Reviewed_Provider_Sign_ID;

                OrdersSubmitManager OrdersSubmitMngr = new OrdersSubmitManager();
                sFacilityAddress = OrdersSubmitMngr.GetLabLocationAddressbyOrderSubmitID(objresMaster.Order_ID);

                if (iSignedPhysicianId != 0)
                {
                    if (objresMaster.Modified_Date_And_Time.ToString("yyyy-MM-dd") != "0001-01-01")
                    {
                        sSignDate = UtilityManager.ConvertToLocal(objresMaster.Modified_Date_And_Time).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        sSignDate = UtilityManager.ConvertToLocal(objresMaster.Created_Date_And_Time).ToString("dd-MMM-yyyy");
                    }
                }
            }

            string sPhysicianName = string.Empty;
            XmlDocument xmldoc1 = new XmlDocument();
            string sSignedPhysicianName = string.Empty;
            string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
            //if (File.Exists(strXmlFilePath1) == true && ClientSession.CurrentPhysicianId != 0)
            if (File.Exists(strXmlFilePath1) == true && iSignedPhysicianId != 0)
            {
                xmldoc1.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                //XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/PhysicianAddress/p" + ClientSession.CurrentPhysicianId);
                XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/PhysicianAddress/p" + iSignedPhysicianId);
                if (nodeMatchingPhysicianAddress != null)
                {
                    //sPhysicianName = nodeMatchingPhysicianAddress.Attributes["Physician_prefix"].Value.ToString() + " " +
                    //nodeMatchingPhysicianAddress.Attributes["Physician_First_Name"].Value.ToString() + " " +
                    //nodeMatchingPhysicianAddress.Attributes["Physician_Middle_Name"].Value.ToString() + " " +
                    //nodeMatchingPhysicianAddress.Attributes["Physician_Last_Name"].Value.ToString() + " " +
                    //nodeMatchingPhysicianAddress.Attributes["Physician_Suffix"].Value.ToString();
                    sSignedPhysicianName = nodeMatchingPhysicianAddress.Attributes["Physician_prefix"].Value.ToString() + " " +
                    nodeMatchingPhysicianAddress.Attributes["Physician_First_Name"].Value.ToString() + " " +
                    nodeMatchingPhysicianAddress.Attributes["Physician_Middle_Name"].Value.ToString() + " " +
                    nodeMatchingPhysicianAddress.Attributes["Physician_Last_Name"].Value.ToString() + " " +
                    nodeMatchingPhysicianAddress.Attributes["Physician_Suffix"].Value.ToString();
                }
            }

            //if (ClientSession.CurrentPhysicianId == 0)
            //{
            //    xmldoc1 = new XmlDocument();
            //    strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
            //    if (File.Exists(strXmlFilePath1) == true)
            //    {
            //        xmldoc1.Load(strXmlFilePath1);
            //        if (txtProvNoteshistory.Text.StartsWith("@") == true && txtProvNoteshistory.Text.Contains("(") == true)
            //        {
            //            XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/UserList/User[@User_Name='" + txtProvNoteshistory.Text.Substring(1, txtProvNoteshistory.Text.IndexOf("(") - 1) + "']"); // + ClientSession.CurrentPhysicianId);
            //            if (nodeMatchingPhysicianAddress != null)
            //            {
            //                sPhysicianName = nodeMatchingPhysicianAddress.Attributes["person_name"].Value.ToString();
            //            }
            //        }
            //    }
            //}


            string sPatientInfo = string.Empty;

            if (txtPatientInformation.Value != string.Empty && txtPatientInformation.Value.Split('|').Length > 5)
            {
                sPatientInfo = "Patient Name: " + txtPatientInformation.Value.Split('|')[0] + Environment.NewLine +
                "Date of Birth: " + txtPatientInformation.Value.Split('|')[1] + Environment.NewLine +
                "MRN: " + txtPatientInformation.Value.Split('|')[5].Split(':')[1] + Environment.NewLine;
            }

            string sHumanID = string.Empty;
            if (Session["human_id"] != null)
                sHumanID = Session["human_id"].ToString();

            //string sNotes = (DLC.txtDLC.Text.Trim() != string.Empty ? DLC.txtDLC.Text : txtProvNoteshistory.Attributes["InterpretationText"]);

            string sNotes = txtProvNoteshistory.Attributes["InterpretationText"];

            if (sNotes != null && sNotes.Contains("Test Reviewed: ") == true)
            {
                string[] sTemplate = sNotes.Split(new string[] { "Test Reviewed: " }, StringSplitOptions.None);
                if (sTemplate.Count() > 1)
                {
                    for (int i = 0; i < sTemplate.Length; i++)
                    {
                        if (sTemplate[i].Trim() != string.Empty && sTemplate[i].Contains(';'))
                        {
                            string sInterpretationTitle = sTemplate[i].Split(';')[0];
                            string Notes = "Test Reviewed: " + sInterpretationTitle + ";" + sTemplate[i].Split(';')[1].Replace("\\n", "\n").Replace("\\t", "\t").Replace("\\r", "\r").Replace("\"", "").Replace("<br/>", "");

                            //                            string sSignDate = string.Empty;
                            //if (txtProvNoteshistory.Text.Contains('\n') == true && txtProvNoteshistory.Text.Split('\n').Length > (i - 1))
                            //{
                            //    if (txtProvNoteshistory.Text.Split('\n')[i - 1].Contains("(") == true && txtProvNoteshistory.Text.Split('\n')[i - 1].Contains(")") == true)
                            //    {
                            //        sSignDate = ExtractBetween(txtProvNoteshistory.Text.Split('\n')[i - 1], "(", ")");
                            //    }
                            //}

                            xmldoc1 = new XmlDocument();
                            strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");

                            try
                            {
                                if (File.Exists(strXmlFilePath1) == true)
                                {
                                    xmldoc1.Load(strXmlFilePath1);
                                    if (txtProvNoteshistory.Text.StartsWith("@") == true && txtProvNoteshistory.Text.Contains("(") == true)
                                    {
                                        string[] sPhyName = txtProvNoteshistory.Text.Split(new string[] { "\n" }, StringSplitOptions.None);
                                        for (int j = 0; j < sPhyName.Length; j++)
                                        {
                                            if (sPhyName[j].Contains(sInterpretationTitle) == true)
                                            {
                                                XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/UserList/User[@User_Name='" + sPhyName[j].Substring(1, sPhyName[j].IndexOf(sInterpretationTitle) - 25) + "']"); // + ClientSession.CurrentPhysicianId);
                                                if (nodeMatchingPhysicianAddress != null)
                                                {
                                                    sPhysicianName = nodeMatchingPhysicianAddress.Attributes["person_name"].Value.ToString();
                                                    // sSignDate = sPhyName[j].Substring(sPhyName[j].IndexOf(sInterpretationTitle) - 23, sPhyName[j].IndexOf(sInterpretationTitle) - 12);
                                                    //sSignDate = ExtractBetween(sPhyName[j], "(", ")");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message + " - " + strXmlFilePath1, ex);
                            }


                            if (sOutput == string.Empty)
                                sOutput = print.PrintInterpretationNotes(Notes, sPhysicianName, sFacilityAddress, sPatientInfo, sHumanID, TargetFileDirectory, sInterpretationTitle, sSignDate, sSignedPhysicianName);
                            else
                                sOutput += "|" + print.PrintInterpretationNotes(Notes, sPhysicianName, sFacilityAddress, sPatientInfo, sHumanID, TargetFileDirectory, sInterpretationTitle, sSignDate, sSignedPhysicianName);
                        }
                    }
                }
            }

            //sOutput = print.PrintInterpretationNotes(sNotes, sPhysicianName, ClientSession.FacilityName, sPatientInfo, sHumanID, TargetFileDirectory);

            string[] sPath = sOutput.Split('|');
            string sPrintPathName = string.Empty;
            hdnFileName.Value = string.Empty;

            for (int iCount = 0; iCount < sPath.Length; iCount++)
            {
                sPrintPathName = sPath[iCount];
                string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                //if (hdnFileName.Value == string.Empty)
                //{
                if (FileName.Length > 0)
                {
                    if (hdnFileName.Value == string.Empty)
                        hdnFileName.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                    else
                        hdnFileName.Value += "|" + "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                }
            }


            //}
            //string FaxSubject = sOutput.Split('|')[1];

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Open Print Receipt-Window", "OpenPrintRecipt_Window('" + FaxSubject + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PrintInterpretation", "PrintInterpretation();", true);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Test", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
        }





        [WebMethod(EnableSession = true)]
        public static string DeleteIndexImage(string ScanIndexConversionID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string status = string.Empty;
            IList<FileManagementIndex> filemanagementIndexList = new List<FileManagementIndex>();
            FileManagementIndexManager objFMImngr = new FileManagementIndexManager();
            // filemanagementIndexList = objFMImngr.GetFileListUsingScanIndexID(Convert.ToUInt64(ScanIndexConversionID));
            filemanagementIndexList = objFMImngr.GetListbySourceAndHumanId(ClientSession.HumanId, "SCAN");
            if (filemanagementIndexList.Count > 0)
            {
                var deletelist = (from u in filemanagementIndexList where u.Scan_Index_Conversion_ID == Convert.ToUInt64(ScanIndexConversionID) select u);
                if (deletelist.ToList().Count > 0)
                {
                    FileManagementIndex objFMI = new FileManagementIndex();
                    objFMI = deletelist.ToList()[0];
                    if (deletelist.ToList()[0].Document_Type == "Results")
                    {
                        var MultipleOrderCheck = (from u in filemanagementIndexList where u.Order_ID == deletelist.ToList()[0].Order_ID select u);
                        if (MultipleOrderCheck.ToList().Count > 1)
                        {
                            status = "115068";//There are multiple results indexed to this order, please contact support for assistance.
                        }
                        else
                        {
                            WFObjectManager obj_workFlow = new WFObjectManager();
                            WFObject DiaOrderWfObject = null;
                            DiaOrderWfObject = obj_workFlow.GetByObjectSystemId(deletelist.ToList()[0].Order_ID, "DIAGNOSTIC ORDER");
                            //CAP-1946
                            if (DiaOrderWfObject != null && DiaOrderWfObject.Id > 0)
                            {
                                if (DiaOrderWfObject.Current_Process == "BILLING_WAIT")
                                {
                                    //iCloseType = 9;
                                    status = "115067";//This result has been signed and can not be deleted.
                                }
                                else
                                {
                                    string OrderID = UpdateFileManagementAndScanindexconversion(objFMI);
                                    int iCloseType = 0;
                                    if (DiaOrderWfObject.Current_Process == "MA_RESULTS")
                                    {
                                        iCloseType = 5;
                                    }
                                    else if (DiaOrderWfObject.Current_Process == "RESULT_REVIEW")
                                    {
                                        iCloseType = 8;
                                    }
                                    obj_workFlow.MoveToPreviousProcess(Convert.ToUInt64(OrderID), "DIAGNOSTIC ORDER", iCloseType, "UNKNOWN", UtilityManager.ConvertToUniversal(DateTime.Now), null);
                                    status = "Success";
                                }
                            }
                            else
                            {
                                status = "115066";//Result is already archived. So, It can not be deleted. Please contact support.
                            }

                        }
                    }
                    else
                    {
                        status = UpdateFileManagementAndScanindexconversion(deletelist.ToList()[0]);
                    }
                }
            }
            return status;
        }


        public static string UpdateFileManagementAndScanindexconversion(FileManagementIndex objFMI)
        {
            //Update file_management_index
            string sResult = string.Empty;
            FileManagementIndexManager objFMImngr = new FileManagementIndexManager();
            IList<FileManagementIndex> filemanagementIndexupdate = new List<FileManagementIndex>();
            if (objFMI.Document_Type == "Results")
            {
                sResult = objFMI.Order_ID.ToString();
                ResultMasterManager objResultMaster = new ResultMasterManager();
                IList<ResultMaster> lstResultMaster = new List<ResultMaster>();
                lstResultMaster = objResultMaster.GetResultMasterListByResultmasterIDForMRE(objFMI.Result_Master_ID);
                ResultMaster objResMaster = new ResultMaster();
                if (lstResultMaster.Count() > 0)
                {
                    IList<ResultMaster> lstUpdateResultMaster = new List<ResultMaster>();
                    objResMaster = lstResultMaster[0];
                    objResMaster.PID_Alternate_Patient_ID = "0";
                    objResMaster.Order_ID = 0;
                    lstUpdateResultMaster.Add(objResMaster);
                    objResultMaster.SaveResultMasterforDeleteFiles(lstUpdateResultMaster);
                }
                objFMI.Workset_ID = objFMI.Order_ID;  //assigning order id for checking purpose only.
                objFMI.Order_ID = 0;
            }
            else
            {
                sResult = "Success";
            }
            objFMI.Modified_By = ClientSession.UserName;
            objFMI.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            objFMI.Is_Delete = "Y";
            filemanagementIndexupdate.Add(objFMI);
            objFMImngr.SaveFileManagementIndexforDeleteFiles(filemanagementIndexupdate);

            //Update scan_index_conversion
            Scan_IndexManager objSICMngr = new Scan_IndexManager();
            IList<scan_index> lstScanIndex = new List<scan_index>();

            lstScanIndex = objSICMngr.GetScanIndexDetailsForScanIndexConversionID(objFMI.Scan_Index_Conversion_ID);
            if (lstScanIndex.Count() > 0)
            {
                objSICMngr = new Scan_IndexManager();
                scan_index objScanIndex = new scan_index();
                objScanIndex = lstScanIndex[0];
                objScanIndex.Modified_By = ClientSession.UserName;
                objScanIndex.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                objScanIndex.Workset_ID = objScanIndex.Order_ID;
                objScanIndex.Order_ID = 0;
                lstScanIndex.Add(objScanIndex);
                objSICMngr.SaveUpdateScanIndexforDeleteFiles(lstScanIndex);
            }
            //File moved to Delete folder.
            StringBuilder filePath = new StringBuilder();
            string UNCPath = ConfigurationSettings.AppSettings["UNCPath"];
            string ftpIP = ConfigurationSettings.AppSettings["ftpServerIP"];
            filePath.Append(objFMI.File_Path.Replace(ftpIP, UNCPath));
            StringBuilder ImagePathDeleteFolder = new StringBuilder();
            ImagePathDeleteFolder.Append(ConfigurationManager.AppSettings["ScanningPath_Local"].ToString() + ConfigurationManager.AppSettings["IndexingDeleteFolder"].ToString() + Path.GetFileName(objFMI.File_Path.ToString()));
            if (filePath.Length != 0 && File.Exists(filePath.ToString()) == true)
            {
                try
                {
                    DirectoryInfo DirDelFolder = new DirectoryInfo(Path.GetDirectoryName(ImagePathDeleteFolder.ToString()));
                    if (!DirDelFolder.Exists)
                        Directory.CreateDirectory(DirDelFolder.FullName);


                    File.Move(filePath.ToString(), ImagePathDeleteFolder.ToString());
                }
                catch
                {
                }
            }
            return sResult;
        }

        //Cap - 686
        protected void btn_SetValue(object sender, EventArgs e)
        {
            txtProvNoteshistory.Attributes.Remove("InterpretationText");
            txtProvNoteshistory.Text = hdnNewProviderNotesHistory.Value.Replace("<br/>", "\n");
            txtProvNoteshistory.Attributes.Add("InterpretationText", hdnNewProviderhistoryattribute.Value);
            //DLC.txtDLC.Text = hdnNewProviderNotes.Value;
            if (DLC.txtDLC.Text != string.Empty)
            {
                btnSave.Disabled = false;
                //Cap - 1696
                hdnSave.Value = "true";
            }
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Test", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);

        }

        private void FillAkidoInterpretationNoteButton(string OrderSubID)
        {   
            //CAP-2478, CAP-2485
            hdnAkidoOrderSubmitID.Value = OrderSubID;
            //CAP-1987
            string sIsAkidoInterpretationNote = "false";
            string sExMessage = "";
            string sStatus = "";
            sIsAkidoInterpretationNote = UtilityManager.IsAkidoInterpretationNote(OrderSubID, out sExMessage, out sStatus);
            if (Request.QueryString["Menu"] == null && ConfigurationSettings.AppSettings["IsAkidoInterpretationNote"] == "Y" && sIsAkidoInterpretationNote == "true")
            {
                btnAkidoInterpretationNote.InnerHtml = "Signed Interpretation Report <i class=\"bi bi-box-arrow-up-right\" aria-hidden=\"true\" style=\"margin-left: 3px;\"></i>";
                btnAkidoInterpretationNote.Visible = true;
                hdnAkidoInterpretationNote.Value = ConfigurationSettings.AppSettings["AkidoInterpretationNoteURL"];
            }
            else
            {
                if (btnMoveToMa.Visible && imgCopyPrevious.Visible)
                {
                    var userAkidoNote = from u in ClientSession.UserPermissionDTO.Userscntab where u.scn_id == 101140 && u.user_name == ClientSession.UserName select u;
                    //CAP-2170 - Disable Akido Note button where the primary physician of encounter is not supervising provider
                    if (userAkidoNote.ToList().Count > 0)
                    {
                        btnAkidoInterpretationNote.InnerHtml = "Akido Interpretation Report <i class=\"bi bi-box-arrow-up-right\" aria-hidden=\"true\" style=\"margin-left: 3px;\"></i>";
                        btnAkidoInterpretationNote.Visible = true;
                        hdnAkidoInterpretationNote.Value = ConfigurationSettings.AppSettings["AkidoInterpretationNoteURL"];
                    }
                    else
                    {
                        btnAkidoInterpretationNote.Visible = false;
                    }
                }
                else
                {
                    btnAkidoInterpretationNote.Visible = false;
                }
            }
        }
    }
}
