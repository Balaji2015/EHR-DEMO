
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
using System.Drawing;
using System.Reflection;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using AjaxControlToolkit;
using Telerik.Web.UI;
using System.Runtime.Serialization;
using Acurus.Capella.UI.UserControls;
using System.IO;

namespace Acurus.Capella.UI
{
    public partial class frmException : System.Web.UI.Page
    {
        UtilityManager objUtilityMngr = new UtilityManager();
        HumanManager humanproxy = new HumanManager();
        public static bool bExceptionRise = false;
        CreateException objCreateexception;
        CodingExceptionDTO exceptionList;
        CreateExceptionManager objCreateExceptionMngr = new CreateExceptionManager();
        PhysicianManager PhyMngr = new PhysicianManager();
        WFObjectManager WFMngr = new WFObjectManager();
        EncounterManager EncMngr = new EncounterManager();
        UserManager usermngr = new UserManager();
        WFObject ehrwfobj = new WFObject();
        bool bCheckEmptyIssues = false;
        ulong ulInvisibleId = 0;
        public ulong ulMyEncounterID = 0;
        ulong ulMyHumanID = 0;
        ulong ulMyPhysicianID = 0;
        string formName = string.Empty;
        ulong AddendumID = 0;
        DataSet ds = new DataSet();
        DataTable dtTable = new DataTable();
        DataRow dr = null;
        bool bCheckFeedback = false;
        Boolean bCheckSave;
        IList<CreateException> GridList = new List<CreateException>();
        DateTime utc = DateTime.MinValue;
        string hdnSucess;
        //code modified and  Added by balaji         
        protected void Page_Load(object sender, EventArgs e)
        {
            ulMyEncounterID = ClientSession.EncounterId;
            ulMyHumanID = ClientSession.HumanId;
            IList<Encounter> lstencounterPhysician = EncMngr.GetEncounterByEncounterID(ulMyEncounterID);
            //ulMyPhysicianID = ClientSession.PhysicianId;
            if (lstencounterPhysician.Count > 0)
            {
                ulMyPhysicianID = Convert.ToUInt32(lstencounterPhysician[0].Encounter_Provider_ID);
            }
            formName = Request["formName"].ToString();
            if (Request["AddendumID"]!=null)
                AddendumID =  Convert.ToUInt32(Request["AddendumID"]);
            hdnSourceScreen.Value = formName;
            if (!IsPostBack)
            {
                DLCfeedback.DName = "pbNotes";
                //ClientSession.FlushSession();
                //code modified by balaji                
                exceptionList = objCreateExceptionMngr.GetAllExceptionDetailsByDescAndGetEncounterDetails(ulMyEncounterID, mpnException.PageNumber, mpnException.MaxResultPerPage, ulMyHumanID, ulMyPhysicianID);
                GetEncounterDetails(exceptionList);
                if (exceptionList != null && exceptionList.ExceptionCount > 0)
                    mpnException.TotalNoofDBRecords = exceptionList.ExceptionCount;
                else
                {
                    mpnException.TotalNoofDBRecords = 0;
                    grdException.DataSource = new string[] { };
                    grdException.DataBind();
                }
                if (formName == "Create Coding Exception")
                {
                    this.Title = "Create Coding Exception -" + ClientSession.UserName;
                    pnlException.GroupingText = "Create Coding Exception";
                    //Code added By balaji.TJ                  
                    if (exceptionList != null && exceptionList.ExceptionCount > 0)
                    {
                        Session["Createcoding"] = exceptionList.Exception;
                        FillGrid(exceptionList);
                    }
                    txtIssues.Focus();
                    pnlReviewException.Visible = false;
                }
                else if (formName == "Feedback for Coding Exception")
                {
                    this.Title = "Feedback for Coding Exception -" + ClientSession.UserName;
                    pnlException.GroupingText = "Feedback for Coding Exception ";
                    pnlReviewException.Visible = true;
                    txtIssues.Enabled = false;
                    //Code added By balaji.TJ                  
                    if (exceptionList != null && exceptionList.Exception.Count > 0)
                    {
                        Session["CreateFeedback"] = exceptionList.Exception;
                        GetReviewExceptionDetails(exceptionList);
                    }
                    grdException.Columns[1].Visible = false;
                    txtIssues.BackColor = Color.FromArgb(191, 219, 255);
                    txtIssues.BorderColor = Color.Black;
                    txtIssues.ForeColor = Color.Black;
                    txtIssues.BorderWidth = 1;
                    lblIssues.ForeColor = Color.Black;
                    DLCfeedback.Value = "Feedback Notes";
                    btnMovetoProvider.Text = "Move to Coding Review";
                }
                btnAdd.Enabled = false;
            }
            //code Added by balaji           
            txtIssues.Attributes.Add("onkeyup", "EnableSavetxtIssues(event);");
            txtIssues.Attributes.Add("onkeypress", "EnableSavetxtIssues(event);");
            txtIssues.Attributes.Add("onchange", "EnableSavetxtIssues(event);");
            DLCfeedback.txtDLC.Attributes.Add("onkeyup", "EnableSavetxtFeedback(event);");
            DLCfeedback.txtDLC.Attributes.Add("onchange", "EnableSavetxtFeedback(event);");
        }
        public void FillGrid(CodingExceptionDTO exceptionlist)
        {
            if (dtTable.Rows.Count > 0)
                dtTable.Rows.Clear();
            else if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("Issues");
                dtTable.Columns.Add("Feedback");
                dtTable.Columns.Add("EncounterID");
                dtTable.Columns.Add("Version");
                grdException.DataSource = new string[] { };
                grdException.DataBind();
            }
            if (exceptionlist != null && exceptionlist.Exception != null && exceptionlist.Exception.Count > 0)//added by vijayan
            {
                for (int i = 0; i < exceptionlist.Exception.Count; i++)
                {
                    dr = dtTable.NewRow();
                    dr["Issues"] = exceptionlist.Exception[i].Issues;
                    dr["Feedback"] = exceptionlist.Exception[i].FeedBack;
                    dr["EncounterID"] = exceptionlist.Exception[i].Id;
                    dr["Version"] = exceptionlist.Exception[i].Version;
                    dtTable.Rows.Add(dr);
                }
                if (ds.Tables.Count == 0)
                    ds.Tables.Add(dtTable);
                grdException.DataSource = ds.Tables[0];
                grdException.DataBind();
            }
        }
        public void GetReviewExceptionDetails(CodingExceptionDTO ReviewDetails)
        {
            //code added by  blaji.tj  2015-12-14           
            if (ReviewDetails.Exception != null && ReviewDetails.Exception.Count > 0)
            {
                var exceptionlistWithoutFeedBack = ReviewDetails.Exception.Where(a => a.FeedBack == string.Empty);
                IList<CreateException> listwithoutFeedback = new List<CreateException>();
                listwithoutFeedback = exceptionlistWithoutFeedBack.ToList<CreateException>();
                var exceptionlistwithFeedBack = ReviewDetails.Exception.Where(a => a.FeedBack != string.Empty);
                IList<CreateException> listwithFeedback = new List<CreateException>();
                listwithFeedback = exceptionlistwithFeedBack.ToList<CreateException>();

                if (dtTable.Rows.Count > 0)
                    dtTable.Rows.Clear();
                else if (dtTable.Columns.Count == 0)
                {
                    dtTable.Columns.Add("Issues");
                    dtTable.Columns.Add("Feedback");
                    dtTable.Columns.Add("EncounterID");
                    dtTable.Columns.Add("Version");
                    dtTable.Columns.Add("CreatedBy");
                    dtTable.Columns.Add("CreatedDateTime");
                }
                if (listwithoutFeedback != null && listwithoutFeedback.Count > 0)
                {
                    for (int i = 0; i < listwithoutFeedback.Count; i++)
                    {
                        dr = dtTable.NewRow();
                        dr["Issues"] = listwithoutFeedback[i].Issues;
                        dr["Feedback"] = listwithoutFeedback[i].FeedBack;
                        dr["EncounterID"] = listwithoutFeedback[i].Id;
                        dr["CreatedBy"] = listwithoutFeedback[i].Created_By;
                        dr["CreatedDateTime"] = Convert.ToDateTime(listwithoutFeedback[i].Created_Date_And_Time);
                        dtTable.Rows.Add(dr);
                        ulInvisibleId = listwithoutFeedback[0].Id;
                        ViewState["ulInvisibleId"] = ulInvisibleId;
                        if (listwithoutFeedback[0].FeedBack == string.Empty)
                        {
                            txtIssues.Text = listwithoutFeedback[0].Issues.ToString();
                            DLCfeedback.txtDLC.Text = listwithoutFeedback[0].FeedBack.ToString();
                            btnAdd.Text = "Update";
                        }
                    }
                }
                if (listwithFeedback != null && listwithFeedback.Count > 0)
                {
                    for (int i = 0; i < listwithFeedback.Count; i++)
                    {
                        dr = dtTable.NewRow();
                        dr["Issues"] = listwithFeedback[i].Issues;
                        dr["Feedback"] = listwithFeedback[i].FeedBack;
                        dr["EncounterID"] = listwithFeedback[i].Id;
                        dr["CreatedBy"] = listwithFeedback[i].Created_By;
                        dr["CreatedDateTime"] = Convert.ToDateTime(listwithFeedback[i].Created_Date_And_Time);
                        dtTable.Rows.Add(dr);
                        if (listwithoutFeedback.Count == 0)
                        {
                            ulInvisibleId = listwithFeedback[0].Id;
                            ViewState["ulInvisibleId"] = ulInvisibleId;
                        }
                    }
                }
                bCheckFeedback = true;
                ViewState["bCheckFeedback"] = bCheckFeedback;
                if (ds.Tables.Count == 0)
                    ds.Tables.Add(dtTable);
                grdException.DataSource = ds.Tables[0];
                grdException.DataBind();
            }
        }
        public void GetEncounterDetails(CodingExceptionDTO exceptionList)
        {
            if (exceptionList != null)
            {
                txtEncounterID.Text = ulMyEncounterID.ToString();
                txtPatientAccount.Text = ulMyHumanID.ToString();
                txtProviderID.Text = ulMyPhysicianID.ToString();
                //code added by blaji.TJ 2015-12-14
                IList<PhysicianLibrary> objPhysician = new List<PhysicianLibrary>();
                if (exceptionList.PhysicianLibraryList != null && exceptionList.PhysicianLibraryList.Count > 0)
                {
                    objPhysician = exceptionList.PhysicianLibraryList;
                    txtProviderName.Text = objPhysician[0].PhyPrefix + " " + objPhysician[0].PhyFirstName + " " + objPhysician[0].PhyMiddleName + " " + objPhysician[0].PhyLastName + " " + objPhysician[0].PhySuffix;
                }
                // txtPatientName.Text = exceptionList.HumanName; 2016
                if (Request["PatientName"] != null)
                    txtPatientName.Text = Request["PatientName"].ToString();
            }
        }
        //code Modified by balaji.TJ 
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            hdnSucess = hdnsuccess.Value;
            string YesNoCancel = hdnMessageType.Value;
            hdnMessageType.Value = string.Empty;
            hdnsuccess.Value = string.Empty;
            string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
            if (strtime!="")
            utc = Convert.ToDateTime(strtime);
            if (formName == "Create Coding Exception")
            {
                if (btnAdd.Text == "Add")
                {
                    if (txtIssues.Text == string.Empty)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('450005');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        //divLoading.Style.Add("display", "none");
                        return;
                    }
                    else
                    {
                        objCreateexception = new CreateException();
                        InsertOrUpdate();
                        if (bCheckEmptyIssues == false)
                        {
                            //code modified by balaji.TJ                          
                            exceptionList = objCreateExceptionMngr.AddCreateExceptionDetails(objCreateexception, mpnException.PageNumber, mpnException.MaxResultPerPage, string.Empty);
                            Session["Createcoding"] = exceptionList.Exception;
                            // if (YesNoCancel != "Yes")
                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('450002');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            if (exceptionList.ExceptionCount > 0)
                            {
                                mpnException.TotalNoofDBRecords = exceptionList.ExceptionCount;
                                FillGrid(exceptionList);
                            }
                            else
                            {
                                mpnException.TotalNoofDBRecords = 0;
                                grdException.DataSource = new string[] { };
                                grdException.DataBind();
                            }
                            bExceptionRise = true;
                            txtIssues.Text = string.Empty;
                            //code added by balaji.TJ 
                            if (hdnSucess == "btnMovetoProvider")
                                MovetoPhysician();
                        }
                        else
                        {
                            bCheckEmptyIssues = false;
                            //divLoading.Style.Add("display", "none");
                            return;
                        }
                    }
                }
                else if (btnAdd.Text == "Update")
                {
                    IList<CreateException> exceptionUpdateList = new List<CreateException>(); ;
                    if (ViewState["ulInvisibleId"] != null)
                        ulInvisibleId = Convert.ToUInt64(ViewState["ulInvisibleId"]);

                    //code added by balaji.TJ 2015-12-12                    
                    IList<CreateException> lstobjCreateException = new List<CreateException>();
                    if (Session["Createcoding"] != null)
                    {
                        lstobjCreateException = Session["Createcoding"] as IList<CreateException>;
                        exceptionUpdateList = lstobjCreateException.Where(o => o.Id == ulInvisibleId).ToList<CreateException>();
                    }
                    objCreateexception = exceptionUpdateList[0];
                    InsertOrUpdate();
                    if (bCheckEmptyIssues == false)
                    {
                        //code modified by balaji.TJ                       
                        exceptionList = objCreateExceptionMngr.UpdateExceptionDetails(objCreateexception, mpnException.PageNumber, mpnException.MaxResultPerPage, string.Empty);
                        Session["Createcoding"] = exceptionList.Exception;
                        // if (YesNoCancel != "Yes")
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('450004');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        if (exceptionList.ExceptionCount > 0)
                        {
                            mpnException.TotalNoofDBRecords = exceptionList.ExceptionCount;
                            FillGrid(exceptionList);
                        }
                        else
                        {
                            mpnException.TotalNoofDBRecords = 0;
                            grdException.DataSource = new string[] { };
                            grdException.DataBind();
                        }
                        btnAdd.Text = "Add";
                        btnClear.Text = "Clear";
                        txtIssues.Text = "";
                        //code added by balaji.TJ 
                        if (hdnSucess == "btnMovetoProvider")
                            MovetoPhysician();
                    }
                    else
                    {
                        btnAdd.Text = "Add";
                        bCheckEmptyIssues = false;
                       // divLoading.Style.Add("display", "none");
                        return;
                    }
                }
                txtIssues.Focus();
            }
            else if (formName == "Feedback for Coding Exception")
            {
                CreateException objCreateexception = null;
                IList<CreateException> ExceplistTemp = new List<CreateException>();
                if (ViewState["bCheckFeedback"] != null)
                    bCheckFeedback = (bool)ViewState["bCheckFeedback"];
                if (bCheckFeedback == false)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "DisplayErrorMessage('440007');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    DLCfeedback.txtDLC.Text = string.Empty;
                    btnAdd.Enabled = false;
                    bCheckSave = true;
                    return;
                }
                else
                {
                    if (ViewState["ulInvisibleId"] != null)
                        ulInvisibleId = Convert.ToUInt64(ViewState["ulInvisibleId"]);
                    //code Added by balaji.TJ 2015-12-14
                    IList<CreateException> CreateFeedbacklist = new List<CreateException>();
                    if (Session["CreateFeedback"] != null)
                    {
                        CreateFeedbacklist = Session["CreateFeedback"] as IList<CreateException>;
                        ExceplistTemp = CreateFeedbacklist.Where(a => a.Id == ulInvisibleId).ToList<CreateException>();
                    }
                    if (ExceplistTemp != null && ExceplistTemp.Count > 0)//added by vijayan
                    {
                        objCreateexception = ExceplistTemp[0];
                        if (DLCfeedback.txtDLC.Text.Trim() == string.Empty)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "DisplayErrorMessage('440006');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            btnAdd.Enabled = true;
                            //divLoading.Style.Add("display", "none");
                            return;
                        }
                        else
                        {
                            objCreateexception.FeedBack = DLCfeedback.txtDLC.Text;
                            objCreateexception.Modified_By = ClientSession.UserName;
                            objCreateexception.Modified_Date_And_Time = utc;
                            objCreateexception.FeedBack_ProvidedBy = ClientSession.UserName;
                            //code Added by balaji.TJ 2015-12-15
                            exceptionList = objCreateExceptionMngr.UpdateExceptionDetails(objCreateexception, mpnException.PageNumber, mpnException.MaxResultPerPage, string.Empty);
                            DLCfeedback.txtDLC.Text = "";
                            txtIssues.Text = "";
                            if (btnAdd.Text == "Update")
                                btnAdd.Text = "Add";
                            if (exceptionList.ExceptionCount > 0)
                            {
                                Session["CreateFeedback"] = exceptionList.Exception;
                                mpnException.TotalNoofDBRecords = exceptionList.ExceptionCount;
                                GetReviewExceptionDetails(exceptionList);
                            }
                            else
                            {
                                mpnException.TotalNoofDBRecords = 0;
                                grdException.DataSource = new string[] { };
                                grdException.DataBind();
                            }
                            if (YesNoCancel != "Yes")
                                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('440002');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            if (hdnSucess == "btnMovetoProvider")
                                MovetoCodingReview();
                            else
                                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('440002');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        }
                    }
                    else
                    {
                        DLCfeedback.txtDLC.Text = "";
                        txtIssues.Text = "";
                        if (btnAdd.Text == "Update")
                            btnAdd.Text = "Add";
                    }
                    bCheckSave = false;
                }
                btnAdd.Focus();
            }
            btnAdd.Enabled = false;
           // divLoading.Style.Add("display", "none");
        }
        public void InsertOrUpdate()
        {

            if (!(txtIssues.Text.Trim() == string.Empty))
                objCreateexception.Issues = txtIssues.Text;
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('450010');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                bCheckEmptyIssues = true;
                txtIssues.Text = string.Empty;
                //divLoading.Style.Add("display", "none");
                return;

            }
            if (btnAdd.Text != "&Update")
            {
                objCreateexception.Enounter_ID = Convert.ToUInt64(txtEncounterID.Text);
                objCreateexception.Human_ID = Convert.ToUInt64(txtPatientAccount.Text);
                objCreateexception.Physician_ID = Convert.ToUInt64(txtProviderID.Text);

                objCreateexception.Created_By = ClientSession.UserName;
                objCreateexception.Created_Date_And_Time = utc;
                objCreateexception.Modified_Date_And_Time = utc;
                objCreateexception.Modified_By = ClientSession.UserName;

            }
            else
            {
                objCreateexception.Modified_By = ClientSession.UserName;
                objCreateexception.Modified_Date_And_Time = utc;
            }
        }
        protected void grdException_ItemCommand(object sender, GridCommandEventArgs e)
        {
            //Code added By balaji.TJ
            if (formName == "Create Coding Exception")
            {
                if (e.CommandName == "EditRow")
                {
                    IList<CreateException> ResultList = new List<CreateException>();
                    if (Session["Createcoding"] != null)
                        ResultList = (IList<CreateException>)Session["Createcoding"];
                    if (ResultList != null && ResultList.Count > 0)
                    {
                        this.btnAdd.Text = "Update";
                        this.btnClear.Text = "Cancel";
                        txtIssues.Focus();
                        btnAdd.Enabled = true;
                        ulong ID = Convert.ToUInt64(e.Item.Cells[6].Text);
                        ResultList = ResultList.Where(a => a.Id == ID).ToList<CreateException>();
                        ViewState["ulInvisibleId"] = ID;
                        if (ResultList.Count > 0)
                            txtIssues.Text = ResultList[0].Issues;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('450007');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        //divLoading.Style.Add("display", "none");
                        return;
                    }
                }
                else
                {
                    if (e.CommandName == "Del")
                    {
                        ulong ID = Convert.ToUInt32(e.Item.Cells[6].Text);
                        DateTime utc = DateTime.MinValue;
                        ViewState["ulInvisibleId"] = ID;
                        hdnDelExceptionId.Value = ID.ToString();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "CellSelected(" + hdnDelExceptionId.Value + ");", true);
                    }
                }
            }
            else
            {
                //Code added By balaji.TJ
                if (e.CommandName == "EditRow")
                {
                    IList<CreateException> ResultList = new List<CreateException>();
                    if (Session["CreateFeedback"] != null)
                        ResultList = (IList<CreateException>)Session["CreateFeedback"];
                    if (ResultList != null && ResultList.Count > 0)
                    {
                        this.btnAdd.Text = "Update";
                        this.btnClear.Text = "Cancel";
                        DLCfeedback.txtDLC.Focus();
                        bCheckFeedback = true;
                        ViewState["bCheckFeedback"] = bCheckFeedback;
                        ulong ID = Convert.ToUInt64(e.Item.Cells[6].Text);
                        ResultList = ResultList.Where(a => a.Id == ID).ToList<CreateException>();
                        btnAdd.Enabled = true;
                        ViewState["ulInvisibleId"] = ID;
                        if (ResultList.Count > 0)
                        {
                            txtIssues.Text = ResultList[0].Issues;
                            DLCfeedback.txtDLC.Text = ResultList[0].FeedBack;
                        }
                    }
                    else
                        btnAdd.Text = "Add";
                }
            }
        }
        protected void btnMovetoProvider_Click(object sender, EventArgs e)
        {
            if ((txtIssues.Text == "" || txtIssues.Text == string.Empty) && btnMovetoProvider.Text == "Move to Coding Review" && grdException.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('440007');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //divLoading.Style.Add("display", "none");
                return;
            }
            if ((DLCfeedback.txtDLC.Text == "" || DLCfeedback.txtDLC.Text == string.Empty) && btnMovetoProvider.Text == "Move to Coding Review" && grdException.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('440006');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //divLoading.Style.Add("display", "none");
                return;
            }
            if (txtIssues.Text != string.Empty && (DLCfeedback.txtDLC.Text == "" || DLCfeedback.txtDLC.Text == string.Empty) && btnMovetoProvider.Text == "Move to Coding Review")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('440006');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //divLoading.Style.Add("display", "none");
                return;
            }
            if (btnMovetoProvider.Text != "Move to Coding Review")
                MovetoPhysician();
            else
                MovetoCodingReview();
            //divLoading.Style.Add("display", "none");
        }
        public void MovetoPhysician()
        {
            DateTime FormLoadTime = DateTime.Now;
            //code Added by balaji.TJ 2015-12-14
            IList<CreateException> ResultList = new List<CreateException>();
            if (Session["Createcoding"] != null)
                ResultList = (IList<CreateException>)Session["Createcoding"];
            IList<string> feedback = ResultList.Select(o => o.FeedBack).ToList<string>();
            if (feedback != null)
            {
                if (feedback.Count == 0 || feedback.Contains(string.Empty) == false)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('450011');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    //divLoading.Style.Add("display", "none");
                    return;
                }
            }
            if (ResultList != null && ResultList.Count <= 0)//condition changed by vijayan [old condition  "if (!(exceptionList.Exception.Count > 0))"]
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('450008');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //divLoading.Style.Add("display", "none");
                return;
            }
            else
            {
                string[] strCurrentProcess = new string[] { "REVIEW_CODING", "REVIEW_CODING_2" };
                //Selvamani
                string ownerName = "UNKNOWN";
                if (ClientSession.CurrentObjectType == "ADDENDUM")
                {
                    strCurrentProcess = new string[] { "ADDENDUM_CODING", "ADDENDUM_CODING_2" };
                    IList<AddendumNotes> loadAddendumNotesForPhysicianObj = new List<AddendumNotes>();
                    AddendumNotesManager objAddendumNotesManager = new AddendumNotesManager();
                    //loadAddendumNotesForPhysicianObj = objAddendumNotesManager.getAddendumNotesForPhysician(ClientSession.currAddendumID, ClientSession.EncounterId);//For Bug Id 54759
                    loadAddendumNotesForPhysicianObj = objAddendumNotesManager.getAddendumNotesForPhysician(AddendumID, ClientSession.EncounterId);
                    if (loadAddendumNotesForPhysicianObj.Count > 0)
                    {
                        ulong phyId = (loadAddendumNotesForPhysicianObj[0].Provider_Review_Signed_ID > 0 && loadAddendumNotesForPhysicianObj[0].Provider_Signed_ID > 0) ? loadAddendumNotesForPhysicianObj[0].Provider_Signed_ID : (loadAddendumNotesForPhysicianObj[0].Provider_Review_Signed_ID > 0) ? loadAddendumNotesForPhysicianObj[0].Provider_Review_Signed_ID : loadAddendumNotesForPhysicianObj[0].Provider_Signed_ID;
                        IList<User> userList = usermngr.GetUserList(ClientSession.LegalOrg);
                        ownerName = userList.Any(a => a.Physician_Library_ID == phyId) ? userList.Where(d => d.Physician_Library_ID == phyId).Select(u => u.user_name.ToUpper()).ToList()[0] : string.Empty;
                    }
                    // WFMngr.MoveToNextProcess(ClientSession.currAddendumID, ClientSession.CurrentObjectType, 2, ownerName, UtilityManager.ConvertToUniversal(FormLoadTime), string.Empty, strCurrentProcess, null);//For Bug Id 54759
                    WFMngr.MoveToNextProcess(AddendumID, ClientSession.CurrentObjectType, 2, ownerName, UtilityManager.ConvertToUniversal(FormLoadTime), string.Empty, strCurrentProcess, null);
                }
                else
                 WFMngr.MoveToNextProcess(ulMyEncounterID, ClientSession.CurrentObjectType, 2, ownerName, UtilityManager.ConvertToUniversal(FormLoadTime), string.Empty, strCurrentProcess, null);
                Session["Moved"] = "True";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "DisplayErrorMessage('450009');LoadException();CloseException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
           // divLoading.Style.Add("display", "none");
        }
        public void MovetoCodingReview()
        {
            //Selvamani         
            if (ClientSession.CurrentObjectType == "ADDENDUM")
            {
                if (btnAdd.Enabled == true && DLCfeedback.txtDLC.Text.Trim() != string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('440010');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    DLCfeedback.txtDLC.Focus();
                   // divLoading.Style.Add("display", "none");
                    return;
                }
                else
                {
                    bool bDuplicateCheck = true;
                    string check = "True";
                    MoveVerificationDTO objMoveVerifyDTO = EncMngr.PerformMoveVerification(ulMyEncounterID, ulMyPhysicianID, ulMyHumanID, UtilityManager.ConvertToLocal(DateTime.Now), ClientSession.FacilityName, ClientSession.UserName, false, string.Empty, string.Empty, ClientSession.UserCurrentProcess, string.Empty, string.Empty, bDuplicateCheck, ClientSession.UserRole, "btnPhysiciancorrection", bDuplicateCheck, check, ClientSession.LegalOrg, out string sAlert);
                    if (objMoveVerifyDTO != null)
                    {
                        if (objMoveVerifyDTO.ExceptionCount > 0 && objMoveVerifyDTO.IsFeedBackProvided == false)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('440004');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            DLCfeedback.txtDLC.Focus();
                            //divLoading.Style.Add("display", "none");
                            return;
                        }
                    }
                }
                string ownerName = "UNKNOWN";
                string[] strCurrentProcess = new string[] { "ADDENDUM_CORRECTION" };
                WFObject wfObject = new WFObject();
                // wfObject = WFMngr.GetByObjectSystemId(ClientSession.currAddendumID, "ADDENDUM");//For Bug Id 54759
                wfObject = WFMngr.GetByObjectSystemId(AddendumID, "ADDENDUM");
                ownerName = wfObject.Process_Allocation.Split('|').Where(s => s.Split('-')[0] == "ADDENDUM_CODING").ToList().FirstOrDefault().Split('-')[1];
                //WFMngr.MoveToNextProcess(ClientSession.currAddendumID, ClientSession.CurrentObjectType, 1, ownerName, UtilityManager.ConvertToUniversal(DateTime.Now), string.Empty, strCurrentProcess, null);//For Bug Id 54759
                WFMngr.MoveToNextProcess(AddendumID, ClientSession.CurrentObjectType, 1, ownerName, UtilityManager.ConvertToUniversal(DateTime.Now), string.Empty, strCurrentProcess, null);
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('450009');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "CloseException();", true);
            }
            else
            {
                if (btnAdd.Enabled == true && DLCfeedback.txtDLC.Text.Trim() != string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('440010'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    DLCfeedback.txtDLC.Focus();
                    //divLoading.Style.Add("display", "none");
                    return;
                }
                try
                {
                    if (ClientSession.FillEncounterandWFObject.DocumentationWFRecord.Current_Process.ToUpper() == "PROVIDER_PROCESS" || ClientSession.FillEncounterandWFObject.DocumentationWFRecord.Current_Process.ToUpper() == "PHYSICIAN_CORRECTION" || ClientSession.FillEncounterandWFObject.DocumentationWFRecord.Current_Process.ToUpper() == "CODER_REVIEW_CORRECTION")
                    {
                        if (ClientSession.FillEncounterandWFObject != null && ClientSession.FillEncounterandWFObject.EncRecord != null)
                        {

                            //if (ClientSession.FillEncounterandWFObject.EncRecord.is_assessment_saved.ToUpper() == "N" && ClientSession.FillEncounterandWFObject.EncRecord.is_serviceprocedure_saved.ToUpper() == "N" && System.Configuration.ConfigurationSettings.AppSettings["IsReviewAlert"].ToString() == "Y")
                            //{
                            //    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            //    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "alert('Please add  atleast one ICD');CloseExceptionmovetab();", true);

                            //    return;
                            //}
                            if (ClientSession.FillEncounterandWFObject.EncRecord.is_serviceprocedure_saved.ToUpper() == "N" && System.Configuration.ConfigurationSettings.AppSettings["IsReviewAlert"].ToString().ToUpper() == "Y")
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "alert('Please review and save the changes in  the Service Procedure Tab');CloseExceptionmovetab();", true);

                                return;
                            }
                            //else if (ClientSession.FillEncounterandWFObject.EncRecord.is_assessment_saved.ToUpper() == "N" && ClientSession.FillEncounterandWFObject.EncRecord.is_serviceprocedure_saved.ToUpper() == "Y")
                            //{
                            //    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            //    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "alert('Please add  atleast one ICD in Assessment Tab');CloseExceptionmovetab();", true);


                            //    return;
                            //}
                        }
                    }
                }
                catch
                {

                }
                //Added by Bala for Duplicate Procedure Check in E&M code on 09-Nov-2012
                bool bDuplicateCheck = true;
                string check = "True";
                MoveVerificationDTO objMoveVerifyDTO = EncMngr.PerformMoveVerification(ulMyEncounterID, ulMyPhysicianID, ulMyHumanID, UtilityManager.ConvertToLocal(DateTime.Now), ClientSession.FacilityName, ClientSession.UserName, false, string.Empty, string.Empty, ClientSession.UserCurrentProcess, string.Empty, string.Empty, bDuplicateCheck, ClientSession.UserRole, "btnPhysiciancorrection", bDuplicateCheck, check, ClientSession.LegalOrg, out string sAlert);
                if (objMoveVerifyDTO != null)
                {
                    if (objMoveVerifyDTO.IsWorkflowPushed)
                    {
                        Session["Moved"] = "True";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "DisplayErrorMessage('450009');CloseException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    }
                    else
                    {
                        if (objMoveVerifyDTO.ExceptionCount > 0 && objMoveVerifyDTO.IsFeedBackProvided == false && hdnSucess == "btnMovetoProvider")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('450002');DisplayErrorMessage('440004');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            DLCfeedback.txtDLC.Focus();
                            //divLoading.Style.Add("display", "none");
                            return;
                        }
                        else if (objMoveVerifyDTO.ExceptionCount > 0 && objMoveVerifyDTO.IsFeedBackProvided == false)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('440004');LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            DLCfeedback.txtDLC.Focus();
                           // divLoading.Style.Add("display", "none");
                            return;
                        }

                    }
                }
            }
            //divLoading.Style.Add("display", "none");
        }
        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            if (ViewState["ulInvisibleId"] != null)
                ulInvisibleId = Convert.ToUInt64(ViewState["ulInvisibleId"]);
            //code modified by balaji
            exceptionList = objCreateExceptionMngr.DeleteException(ulInvisibleId, mpnException.PageNumber, mpnException.MaxResultPerPage, string.Empty, ClientSession.UserName, DateTime.Now.ToUniversalTime());
            dtTable.Rows.Clear();
            if (exceptionList != null)
            {
                mpnException.TotalNoofDBRecords = exceptionList.ExceptionCount;
                FillGrid(exceptionList);
            }
            btnAdd.Text = "Add";
            txtIssues.Text = "";
            txtIssues.Focus();
            //divLoading.Style.Add("display", "none");
            //Added for BugId:37163
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "LoadException(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }

        public void FirstPageNavigator(object sender, EventArgs e)
        {
            LoadGridPageNavigator();
        }
        #region LoadGridPageNavigator  added by balaji.TJ 2014-11-24
        public void LoadGridPageNavigator()
        {
            if (formName == "Create Coding Exception")
            {
                exceptionList = objCreateExceptionMngr.GetAllExceptionDetailsByDesc1(ulMyEncounterID, "Created_Date_And_Time", mpnException.PageNumber, mpnException.MaxResultPerPage, ulMyHumanID);
                Session["Createcoding"] = exceptionList.Exception;
                if (exceptionList != null)
                {
                    mpnException.TotalNoofDBRecords = exceptionList.ExceptionCount;
                    FillGrid(exceptionList);
                }
                else
                {
                    mpnException.TotalNoofDBRecords = 0;
                    grdException.DataSource = new string[] { };
                    grdException.DataBind();
                }
            }
            else
            {
                exceptionList = objCreateExceptionMngr.GetAllExceptionDetailsByDesc1(ulMyEncounterID, "Created_Date_And_Time", mpnException.PageNumber, mpnException.MaxResultPerPage, ulMyHumanID);
                Session["CreateFeedback"] = exceptionList.Exception;
                if (exceptionList != null)
                {
                    mpnException.TotalNoofDBRecords = exceptionList.ExceptionCount;
                    GetReviewExceptionDetails(exceptionList);
                    grdException.DataBind();
                }
                else
                {
                    mpnException.TotalNoofDBRecords = 0;
                    grdException.DataSource = new string[] { };
                    grdException.DataBind();
                }
            }
        }
        #endregion
    }
}
