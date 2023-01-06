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
using System.IO;
using System.Xml;
using MySql.Data.MySqlClient;
using System.Threading;

namespace Acurus.Capella.UI
{
    public partial class frmAddInsurancePolicies : System.Web.UI.Page
    {

        IList<PhysicianLibrary> physicianList;
        InsurancePlan objInsPlan = new InsurancePlan();
        PhysicianManager PhyMngr = new PhysicianManager();
        PatientInsuredPlan patInsured;
        ulong ulId, ulMyHumanID = 0;
        IList eligibilityDetails;
        bool bEnable = false;
        public static ulong ulInsuredHumanId;
        Human humanObj = new Human();
        PatientInsuredPlan patInsPlan = new PatientInsuredPlan();
        Boolean bUpdatePolicy, bInsuranceType, bFormCloseCheck;
        ulong PatInusredPlanId;

        ulong iLoadInsPlanId;
        IList<Eligibility_Verification> EligList;

        PatientInsuredPlanManager PatinsMngr = new PatientInsuredPlanManager();
        Eligibility_VerficationManager EligibilityMngr = new Eligibility_VerficationManager();
        InsurancePlanManager InsMngr = new InsurancePlanManager();
        HumanManager HumanMngr = new HumanManager();
        StaticLookupManager LookUpMngr = new StaticLookupManager();
        PatGuarantorManager patGuarantorMngr = new PatGuarantorManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Add Insurance Policy" + "-" + ClientSession.UserName;
            bool isNotSelf = false;
            if (!IsPostBack)
            {

                // ClientSession.FlushSession();
                if (Request["sSelf"] != null)
                {
                    isNotSelf = true;
                }
                if (Request["PatientType"] != null)
                {
                    hdnPatientType.Value = Request["PatientType"];
                }
                if (Request["HumanId"] != null && Request["InsuranceType"] != null && Request["LastName"] != null && Request["FirstName"] != null && Request["ExAccountNo"] != null)
                {
                    ulMyHumanID = Convert.ToUInt64(Request["HumanId"]);
                    // Session["ulMyHumanID"] = ulMyHumanID;
                    hdnMyHumanID.Value = ulMyHumanID.ToString();
                    bUpdatePolicy = false;
                    // Session["bUpdatePolicy"] = bUpdatePolicy;
                    hdnbUpdatePolicy.Value = bUpdatePolicy.ToString();
                    bInsuranceType = true;
                    txtPatientLastName.Text = Request["LastName"].ToString();
                    txtPatientFirstName.Text = Request["FirstName"].ToString();
                    txtPolicyHolderAccountNo.Text = ulMyHumanID.ToString();
                    txtExternalAccountNo.Text = Request["ExAccountNo"].ToString();
                    if (Request["HumanId"] != null)
                    {
                        txtAccNo.Text = Request["HumanId"].ToString();
                    }

                }
                else if (Request["HumanId"] != null && Request["LastName"] != null && Request["FirstName"] != null && Request["ExAccountNo"] != null)
                {
                    ulMyHumanID = Convert.ToUInt64(Request["HumanId"]);
                    // Session["ulMyHumanID"] = ulMyHumanID;
                    hdnMyHumanID.Value = ulMyHumanID.ToString();
                    bUpdatePolicy = false;
                    //  Session["bUpdatePolicy"] = bUpdatePolicy;
                    hdnbUpdatePolicy.Value = bUpdatePolicy.ToString();
                    txtPatientLastName.Text = Request["LastName"].ToString(); ;
                    txtPatientFirstName.Text = Request["FirstName"].ToString(); ;
                    txtPolicyHolderAccountNo.Text = ulMyHumanID.ToString();
                    txtExternalAccountNo.Text = Request["ExAccountNo"].ToString();
                    if (Request["HumanId"] != null)
                    {
                        txtAccNo.Text = Request["HumanId"].ToString();
                    }
                }
                else if (Request["HumanId"] != null && Request["InsHumanId"] != null && Request["InsPlan"] != null)
                {
                    btnSelectPlan.Enabled = false;
                    PatInusredPlanId = Convert.ToUInt64(Request["InsPlan"].ToString());
                    hdnPatInusredPlanId.Value = PatInusredPlanId.ToString();
                    ulId = Convert.ToUInt64(Request["InsHumanId"].ToString());
                    hdnInsuredHumanID.Value = ulId.ToString();
                    ulMyHumanID = Convert.ToUInt64(Request["HumanId"].ToString());
                    //   Session["ulMyHumanID"] = ulMyHumanID;
                    hdnMyHumanID.Value = ulMyHumanID.ToString();
                    //variable indicates whether this form is called as new form or as update form from the patient insurance policy 
                    //maintenance form.set as true if its as view or update insurance policy form.
                    bUpdatePolicy = true;
                    // Session["bUpdatePolicy"] = bUpdatePolicy;
                    hdnbUpdatePolicy.Value = bUpdatePolicy.ToString();
                    //for update the patient relation combo cannot be changed and add new insured and select existing insured button
                    //should be disabled.similarly select plan should be disabled.perform eligibility button to be enabled.
                    //ComboBoxColorChange(ddlPatientRelation);
                    btnAddNewInsured.Enabled = false;
                    btnSelectExistingInsured.Enabled = false;
                    btnPerformEligibilityVerification.Enabled = true;
                    //Commented on 12/11/2013
                    // ComboBoxColorChangeWhite(cboInsAssignment);
                    //method to fill the human details.
                    //TextBoxColorChangeWhite(txtGroupNumber);
                    //ComboBoxColorChangeWhite(cboPhysicianId);
                    //ComboBoxColorChangeWhite(cboPhysicianName);
                    btnFindPCP.Enabled = true;
                    getFromHumanId(ulId);
                    if (Request["InsHumanId"] != null)
                    {
                        txtAccNo.Text = Request["InsHumanId"].ToString();
                    }

                }
                dtpTerminationDate.Text = string.Empty;
                dtpEffectiveStartDate.Text = string.Empty;

                loadCombo();
                if (Convert.ToBoolean(hdnbUpdatePolicy.Value) == true)
                {
                    Human objhuman = new Human();
                    IList<Human> HumanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(ulMyHumanID);
                    if (HumanList != null && HumanList.Count > 0) //added by balaji.tj
                    {
                        objhuman = HumanList[0];
                    }
                    if (objhuman != null)
                    {
                        txtPatientFirstName.Text = objhuman.First_Name;
                        txtPatientLastName.Text = objhuman.Last_Name;
                        txtPolicyHolderAccountNo.Text = objhuman.Id.ToString();
                        txtExternalAccountNo.Text = objhuman.Patient_Account_External;
                        mskInsPhone.ReadOnly = true;
                        msktxtInsFax.ReadOnly = true;
                        msktxtPatWorkPhone.ReadOnly = true;
                        msktxtPolicyHolderCellPhno.ReadOnly = true;
                        msktxtPolicyHolderHomePhno.ReadOnly = true;
                        msktxtPolicyHolderSSN.ReadOnly = true;

                        //get the particular pat_insured table entry for the selected row in prev form.
                        foreach (PatientInsuredPlan patIns in objhuman.PatientInsuredBag)
                        {
                            if (PatInusredPlanId == patIns.Id)
                            {
                                patInsPlan = patIns;
                                Session["patInsPlan"] = patInsPlan;
                                for (int i = 0; i < ddlPatientRelation.Items.Count; i++)
                                {
                                    if (ddlPatientRelation.Items[i].Text == patIns.Relationship.ToString())
                                    {
                                        ddlPatientRelation.SelectedIndex = i;
                                    }
                                }
                                ComboBoxSelectedIndexTrigger(ddlPatientRelation);
                                loadPlanGroup(patIns.Insurance_Plan_ID, false);
                                iLoadInsPlanId = patIns.Insurance_Plan_ID;
                                bEnable = false;

                                txtPolicyHolderID.Text = patIns.Policy_Holder_ID;
                                LoadEligibiltyDetails();
                                txtGroupNumber.Text = patIns.Group_Number;
                                cboInsAssignment.Text = patIns.Assignment;

                                physicianList = PhyMngr.LoadPhysicianList();
                                if (physicianList != null)
                                {
                                    if (patIns.PCP_ID != 0)
                                    {
                                        var FldList = from h in physicianList where h.Id == patIns.PCP_ID select h;
                                        txtPCP.Text = FldList.ToList()[0].PhyLastName + "," + FldList.ToList()[0].PhyFirstName + " " + FldList.ToList()[0].PhyMiddleName + " " + FldList.ToList()[0].PhySuffix;
                                        txtPcpTag.Value = patIns.PCP_ID.ToString();

                                    }
                                    else
                                    {
                                        txtPCP.Text = patIns.PCP_Name;
                                        txtPcpTag.Value = "0";
                                    }
                                }
                                txtPCPNPI.Text = patIns.PCP_NPI;

                                // cboPhysicianId.Text = patIns.PCP_ID.ToString();

                                if (patIns.Effective_Start_Date == Convert.ToDateTime("1/1/0001"))
                                {
                                    dtpEffectiveStartDate.Text = string.Empty;
                                }
                                else
                                {
                                    //dtpEffectiveStartDate.CustomFormat = "dd-MMM-yyyy ";
                                    dtpEffectiveStartDate.Text = patIns.Effective_Start_Date.ToString("dd-MMM-yyyy");
                                }

                                if (patIns.Termination_Date == Convert.ToDateTime("1/1/0001"))
                                {
                                    dtpTerminationDate.Text = string.Empty;
                                }
                                else
                                {
                                    // dtpTerminationDate.CustomFormat = "dd-MMM-yyyy ";
                                    dtpTerminationDate.Text = patIns.Termination_Date.ToString("dd-MMM-yyyy");
                                }      //dtpTerminationDate.Value=patIns.Termination_Date;

                                break;
                            }
                        }
                    }
                    if (txtPolicyHolderID.Text != string.Empty)
                    {
                        btnPerformEligibilityVerification.Enabled = true;
                    }
                    else
                    {
                        btnPerformEligibilityVerification.Enabled = false;
                    }

                }
                else
                {
                    ddlPatientRelation.SelectedIndex = 0;
                    ddlPatientRelation_SelectedIndexChanged(sender, e);
                }
                bFormCloseCheck = false;
                btnSave.Enabled = false;
                txtInsPlanID.Attributes.Add("readonly", "readonly");
                //txtPCP.Attributes.Add("readonly", "readonly");
                if (isNotSelf == true)
                {
                    //Response.Write("<script>Open();</script>");
                    btnSelectPlan_Click(sender, e);
                    AddInsuredModalWindow.NavigateUrl = "frmSelectPayer.aspx";
                    AddInsuredModalWindow.Height = 470;
                    AddInsuredModalWindow.VisibleOnPageLoad = true;
                    AddInsuredModalWindow.Width = 830;
                    AddInsuredModalWindow.CenterIfModal = true;
                    AddInsuredModalWindow.VisibleTitlebar = true;
                    AddInsuredModalWindow.VisibleStatusbar = false;
                    AddInsuredModalWindow.OnClientClose = "SelectPlanClick";
                    AddInsuredModalWindow.KeepInScreenBounds = true;
                    AddInsuredModalWindow.Behaviors = Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move;
                    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "OpenSelectPayer", "Open();", true);
                    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Self", "SelectPlanClickForSelf();", true);

                    hdnSelfPlan.Value = true.ToString();
                    ulong MyPlanId = 0;
                    //btnSelectPlan_Click(sender, e); ulong MyPlanId = 0;
                    string CarrierName = string.Empty;
                    //select payer form will be opened.
                    //frmSelectPayer selectPayer = new frmSelectPayer();
                    //selectPayer.View(ref MyPlanId, ref CarrierName);
                    //  string link = "frmSelectPayer.aspx?MyPlanId=" + MyPlanId + "&CarrierName=" + CarrierName;
                    // MessageBox.Open1(link);
                    //myplanid is the variable set in select payer form indicating that the particular plan is selected and the details will be filled here from insurance_plan table.
                    if (txtInsPlanID.Text != string.Empty)
                    {
                        MyPlanId = Convert.ToUInt64(txtInsPlanID.Text);
                    }
                    if (MyPlanId > 0)
                    {
                        btnSave.Enabled = true;
                        loadPlanGroup(MyPlanId, true);
                    }
                    if (bEnable == true)
                    {
                        bFormCloseCheck = true;
                        TextBoxColorChangeWhite(txtPolicyHolderID);
                        TextBoxColorChangeWhite(txtGroupNumber);
                        //ComboBoxColorChangeWhite(cboPhysicianId);
                        //ComboBoxColorChangeWhite(cboPhysicianName);
                        btnFindPCP.Enabled = true;
                        btnPerformEligibilityVerification.Enabled = true;
                        btnSave.Enabled = true;
                        // ComboBoxColorChangeWhite(cboInsAssignment);
                        bEnable = false;
                    }
                }
                if (Request["CurrentProcess"] != null)
                {
                    hdnCurrentProcess.Value = Request["CurrentProcess"];
                    if (hdnCurrentProcess.Value.Contains("QC"))
                    {
                        DisableTableLayout(pnlInsuranceDetails);
                        DisableTableLayout(pnlPatientInformation);
                        DisableTableLayout(pnlPolicyHolderInfo);
                        btnSave.Enabled = false;
                        btnAddNewInsured.Enabled = false;
                        btnFindPCP.Enabled = false;
                        btnPerformEligibilityVerification.Enabled = false;
                        btnSelectExistingInsured.Enabled = false;
                        btnSelectPlan.Enabled = false;
                    }
                }

                //srividhya added on 03-feb-2014               
                if (Request.QueryString["EncounterId"] != null)
                {
                    hdnEncounterID.Value = Request.QueryString["EncounterId"].ToString();
                }
                else
                {
                    hdnEncounterID.Value = "0";
                }
                txtPolicyHolderID.ReadOnly = true;
                if (txtInsCarrierName.Text != "" && txtInsCarrierName.Text == "MEDICARE" && Request["HumanId"] != null && Request["InsHumanId"] != null && Request["InsPlan"] != null)
                {
                    //txtPolicyHolderID.Enabled = true;
                    TextBoxColorChangeWhite(txtPolicyHolderID);
                    txtPolicyHolderID.ReadOnly = false;
                }

                btnSave.Attributes.Add("Onclick", "javascript:return Validation();");

            }
        }
        public void DisableTableLayout(Panel tablelayout)
        {
            for (int i = 0; i < tablelayout.Controls.Count; i++)
            {
                if (tablelayout.Controls[i].GetType().ToString().Contains("TextBox"))
                {
                    TextBox txtBox = (TextBox)tablelayout.Controls[i];
                    txtBox.ReadOnly = true;
                    txtBox.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("DropDownList"))
                {
                    DropDownList combobox = (DropDownList)tablelayout.Controls[i];
                    combobox.Enabled = false;
                    combobox.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("CheckBox"))
                {
                    CheckBox checkbox = (CheckBox)tablelayout.Controls[i];
                    checkbox.Enabled = false;

                }
            }
            //else if (tablelayout.Controls[i].GetType().ToString().Contains("DateTimePicker"))
            //{
            //    DateTimePicker datetimepicker = (DateTimePicker)tablelayout.Controls[i];
            //    DateTimePickerColorChange(datetimepicker);
            //}
            //else if (tablelayout.Controls[i].GetType().ToString().Contains("MaskedTextBox"))
            //{
            //    MaskedTextBox MskTxtBox = (MaskedTextBox)tablelayout.Controls[i];
            //    MaskedTextBoxColorChange(MskTxtBox, false);
            //}
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            string YesNoCancel = hdnMessageType.Value;
            hdnMessageType.Value = string.Empty;
            //save details of first tab in human table except the patient relation combo.patient relation combo value has to be saved in                pat_insured table.in human table update operation and in pat_insured table save operation to be performed.
            if (Convert.ToBoolean(hdnbUpdatePolicy.Value) == false)
            {
                if (Validation() == false)
                {
                    btnSave.Enabled = true;
                    return;
                }
                patInsured = new PatientInsuredPlan();
                patInsured = fillPatientInsuredTableDetails(patInsured);
                patInsured.Created_By = ClientSession.UserName;
                if (hdnLocalTime.Value != string.Empty)
                    patInsured.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);

                if (chkAddAsGuarantor.Checked == true)
                {
                    PatGuarantor objPatguarantor = null;
                    IList<PatGuarantor> patGuarantorLIst = null;
                    int GuarantorID = 0;
                    if (ddlPatientRelation.SelectedItem.Text.ToUpper() == "SELF")
                    {
                        if (txtPolicyHolderAccountNo.Text != string.Empty)
                        {
                            GuarantorID = Convert.ToInt32(txtPolicyHolderAccountNo.Text);
                        }
                    }
                    else
                    {
                        if (hdnFindPatientID.Value != string.Empty)
                        {
                            GuarantorID = Convert.ToInt32(hdnFindPatientID.Value);
                        }
                        else if (txtPolicyHolderLastName.Text != string.Empty)
                        {
                            if (hdnInsuredHumanID.Value != string.Empty)
                            {
                                GuarantorID = Convert.ToInt32(hdnInsuredHumanID.Value);
                            }
                        }
                    }
                    if (GuarantorID != 0)
                    {
                        IList<PatGuarantor> patguarantorlist = patGuarantorMngr.GetPatGuarantorDetails(Convert.ToInt32(txtPolicyHolderAccountNo.Text), GuarantorID);
                        if (patguarantorlist != null && patguarantorlist.Count > 0) //added by balaji.TJ
                        {
                            objPatguarantor = patguarantorlist[0];
                            if (txtPolicyHolderAccountNo.Text != "") //added by balaji.TJ
                                objPatguarantor.Human_ID = Convert.ToInt32(txtPolicyHolderAccountNo.Text);
                            objPatguarantor.Active = "YES";
                            objPatguarantor.Modified_By = ClientSession.UserName;
                            if (hdnLocalTime.Value != string.Empty)
                            {
                                objPatguarantor.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                                objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                            }
                            objPatguarantor.Guarantor_Human_ID = GuarantorID;
                            objPatguarantor.Relationship = ddlPatientRelation.SelectedItem.Text;
                            if (ddlPatientRelation.SelectedItem.Text != string.Empty && ddlPatientRelation.SelectedIndex != -1)
                            {
                                string temp = ddlPatientRelation.Items[ddlPatientRelation.SelectedIndex].Value;
                                string[] Relation = temp.Split('-');
                                if (Relation.Length == 2)
                                {
                                    objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);

                                }

                            }
                        }
                        else
                        {
                            objPatguarantor = new PatGuarantor();
                            if (txtPolicyHolderAccountNo.Text != "") //added by balaji.TJ
                                objPatguarantor.Human_ID = Convert.ToInt32(txtPolicyHolderAccountNo.Text);
                            objPatguarantor.Active = "YES";
                            objPatguarantor.Created_By = ClientSession.UserName;
                            if (hdnLocalTime.Value != string.Empty)
                                objPatguarantor.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                            objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                            objPatguarantor.Guarantor_Human_ID = GuarantorID;
                            objPatguarantor.Relationship = ddlPatientRelation.SelectedItem.Text;
                            if (ddlPatientRelation.SelectedItem.Text != string.Empty && ddlPatientRelation.SelectedIndex != -1)
                            {
                                string temp = ddlPatientRelation.Items[ddlPatientRelation.SelectedIndex].Value;
                                string[] Relation = temp.Split('-');
                                if (Relation.Length == 2)
                                {
                                    objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);

                                }
                            }
                        }
                        patGuarantorLIst = new List<PatGuarantor>();
                        patGuarantorLIst.Add(objPatguarantor);
                        patGuarantorMngr.SaveBatchWithTransaction(patGuarantorLIst, null, string.Empty);
                    }
                }

                #region Commented By Deepak

                ////For xml To save Pat_insurance details
                //string HumanFileName = "Human" + "_" + ClientSession.HumanId + ".xml";
                //string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
                //if (File.Exists(strXmlHumanFilePath) == false && ClientSession.HumanId > 0)
                //{
                //    string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                //    string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                //    itemDoc.Load(XmlText);
                //    XmlText.Close();
                //    //itemDoc.Save(strXmlHumanFilePath);
                //    int trycount = 0;
                //trytosaveagain:
                //    try
                //    {
                //        itemDoc.Save(strXmlHumanFilePath);
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
                #endregion
                //UpdateAgeinBlob(ClientSession.HumanId, ClientSession.PatientPaneList[0].Birth_Date);

                // save in pat_insured table through the object.
                PatinsMngr.addPatInsured(patInsured, string.Empty);
                if (EligList != null && EligList.Count > 0)
                {
                    if (txtInsPlanID.Text != "")
                        EligList[0].Insurance_Plan_ID = Convert.ToUInt64(txtInsPlanID.Text);
                    EligibilityMngr.UpdateEligibilityVerificationInformatnion(EligList[0], string.Empty);
                }
                if (YesNoCancel != "Yes")
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "showVal", "SaveCloseWindow();", true);
                }
                // this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "showVal", "SaveCloseWindow();", true);
                // bFormCloseCheck = false;

            }
            else
            {
                if ((PatientInsuredPlan)Session["patInsPlan"] != null)
                {
                    patInsPlan = fillPatientInsuredTableDetails((PatientInsuredPlan)Session["patInsPlan"]);
                    Session["patInsPlan"] = patInsPlan;
                    patInsPlan.Modified_By = ClientSession.UserName;
                    if (hdnLocalTime.Value != string.Empty)
                        patInsPlan.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    // update in pat_insured table.
                    PatinsMngr.UpdatePatInsured(patInsPlan, string.Empty);
                    if (chkAddAsGuarantor.Checked == true)
                    {
                        PatGuarantor objPatguarantor = null;
                        IList<PatGuarantor> patGuarantorLIst = null;
                        int GuarantorID = 0;
                        if (ddlPatientRelation.SelectedItem.Text.ToUpper() == "SELF")
                        {
                            if (txtPolicyHolderAccountNo.Text != string.Empty)
                            {
                                GuarantorID = Convert.ToInt32(txtPolicyHolderAccountNo.Text);
                            }
                        }
                        else
                        {
                            if (hdnFindPatientID.Value != string.Empty)
                            {
                                GuarantorID = Convert.ToInt32(hdnFindPatientID.Value);
                            }
                            else if (txtPolicyHolderLastName.Text != string.Empty)
                            {
                                if (hdnInsuredHumanID.Value != string.Empty)
                                {
                                    GuarantorID = Convert.ToInt32(hdnInsuredHumanID.Value);
                                }
                            }
                        }
                        if (GuarantorID != 0)
                        {
                            IList<PatGuarantor> patguarantorlist = patGuarantorMngr.GetPatGuarantorDetails(Convert.ToInt32(txtPolicyHolderAccountNo.Text), GuarantorID);
                            if (patguarantorlist != null && patguarantorlist.Count > 0) //added by balaji.TJ
                            {
                                objPatguarantor = patguarantorlist[0];
                                if (txtPolicyHolderAccountNo.Text != string.Empty) //added by balaji.TJ
                                    objPatguarantor.Human_ID = Convert.ToInt32(txtPolicyHolderAccountNo.Text);
                                objPatguarantor.Active = "YES";
                                objPatguarantor.Modified_By = ClientSession.UserName;
                                if (hdnLocalTime.Value != string.Empty)
                                    objPatguarantor.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                                if (hdnLocalTime.Value != string.Empty)
                                    objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                                objPatguarantor.Guarantor_Human_ID = GuarantorID;
                                objPatguarantor.Relationship = ddlPatientRelation.SelectedItem.Text;
                                if (ddlPatientRelation.SelectedItem.Text != string.Empty && ddlPatientRelation.SelectedIndex != -1)
                                {
                                    string temp = ddlPatientRelation.Items[ddlPatientRelation.SelectedIndex].Value;
                                    string[] Relation = temp.Split('-');
                                    if (Relation.Length == 2)
                                        objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);
                                }
                            }
                            else
                            {
                                objPatguarantor = new PatGuarantor();
                                if (txtPolicyHolderAccountNo.Text != "") //added by balaji.TJ
                                    objPatguarantor.Human_ID = Convert.ToInt32(txtPolicyHolderAccountNo.Text);
                                objPatguarantor.Active = "YES";
                                objPatguarantor.Created_By = ClientSession.UserName;
                                if (hdnLocalTime.Value != string.Empty)
                                {
                                    objPatguarantor.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                                    objPatguarantor.From_Date = Convert.ToDateTime(hdnLocalTime.Value).Date;
                                }
                                objPatguarantor.Guarantor_Human_ID = GuarantorID;
                                objPatguarantor.Relationship = ddlPatientRelation.SelectedItem.Text;
                                if (ddlPatientRelation.SelectedItem.Text != string.Empty && ddlPatientRelation.SelectedIndex != -1)
                                {
                                    string temp = ddlPatientRelation.Items[ddlPatientRelation.SelectedIndex].Value;
                                    string[] Relation = temp.Split('-');
                                    if (Relation.Length == 2)
                                    {
                                        objPatguarantor.Relationship_No = Convert.ToInt16(Relation[1]);

                                    }

                                }
                            }
                            patGuarantorLIst = new List<PatGuarantor>();
                            patGuarantorLIst.Add(objPatguarantor);
                            patGuarantorMngr.SaveBatchWithTransaction(patGuarantorLIst, null, string.Empty);
                        }
                    }
                    patInsPlan = PatinsMngr.getInsuranceDetailsUsingPatInsuredId(((PatientInsuredPlan)Session["patInsPlan"]).Id);
                    Session["patInsPlan"] = patInsPlan;
                    if (EligList != null && EligList.Count > 0)
                    {
                        if (txtInsPlanID.Text != "") //added by balaji.TJ 
                            EligList[0].Insurance_Plan_ID = Convert.ToUInt64(txtInsPlanID.Text);
                        EligList[0].Group_Number = txtGroupNumber.Text;
                        EligibilityMngr.UpdateEligibilityVerificationInformatnion(EligList[0], string.Empty);
                        EligList = EligibilityMngr.GetEligDetailsUsingHumanandPolicyHolderIDandInsPlanID(Convert.ToUInt64(hdnMyHumanID.Value), txtPolicyHolderID.Text, iLoadInsPlanId);
                    }
                    // ApplicationObject.erroHandler.DisplayErrorMessage("420020", "Add Insurance Policy", this.Page);//0.erroHandler.DisplayErrorMessage("420020", this.Text);
                    if (YesNoCancel != "Yes")
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "showVal", "SaveCloseWindow();", true);
                    }
                }
            }
            //CloseWindow
            bFormCloseCheck = false;
            if (hdnCurrentProcess.Value == string.Empty)
            {
                hdnCurrentProcess.Value = string.Empty;
            }
            //Response.Write("<script> alert('Saved Successfully'); self.close(); if (window.opener && !window.opener.closed) { window.opener.location.reload(true);} </script>");
            // Response.Write("<script> alert('Saved Successfully'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();} self.close(); </script>");

            hdnCarrierID.Value = "1234";
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveIns", "SaveCloseWindow();", true);
        }
        public void loadCombo()
        {
            IList<StaticLookup> StaticLookUpList = null;
            //patient relation combo.
            StaticLookUpList = LookUpMngr.getStaticLookupByFieldName("PATIENT RELATIONSHIP");
            if (StaticLookUpList != null)
            {
                for (int i = 0; i < StaticLookUpList.Count; i++)
                {
                    ddlPatientRelation.Items.Add(StaticLookUpList[i].Value);
                    ddlPatientRelation.Items[i].Value = StaticLookUpList[i].Sort_Order + "-" + StaticLookUpList[i].Description;
                }
            }
            StaticLookUpList = LookUpMngr.getStaticLookupByFieldName("ASSIGNMENT");
            if (StaticLookUpList != null)
            {
                foreach (StaticLookup i in StaticLookUpList)
                {
                    cboInsAssignment.Items.Add(i.Value);
                }
            }


        }

        //event when patient relation combo is changed. 
        private void ddlPatientRelation_SelectedIndexChanged(object sender, EventArgs e)
        {
            bFormCloseCheck = true;
            btnSave.Enabled = true;
            ClearAll(pnlPolicyDemoInfo);
            ClearAll(pnlPolicyHolderInfo);
            ClearAll(pnlInsuranceDetails);
            //if relation is self then load that patient's human details.
            sPolicyHolderId.Attributes.Add("class", "MandLabelstyle");
            sPolicyMan.Attributes.Add("class", "manredforstar");
            if (ddlPatientRelation.SelectedItem.Text.ToUpper() == "SELF")
            {
                //disable addnewinsured and selectexistinginsured buttons.
                btnSelectExistingInsured.Enabled = false;
                btnAddNewInsured.Enabled = false;
                //patient's first and last name are updated.
                getFromHumanId(Convert.ToUInt64(hdnMyHumanID.Value));
                txtAccNo.Text = hdnMyHumanID.Value;
                return;
            }
            btnSelectExistingInsured.Enabled = true;
            btnAddNewInsured.Enabled = true;

        }
        private void ComboBoxSelectedIndexTrigger(DropDownList ComboBox)
        {
            // DropDownListIte Item = ComboBox.FindItem(ComboBox.Text);
            //if (Item != null)
            //{
            //}//  ComboBox.SelectedIndex = Item.Index;
        }
        public void ClearAll(Panel tablelayout)
        {
            dtpTerminationDate.Text = string.Empty;
            dtpEffectiveStartDate.Text = string.Empty;
            for (int i = 0; i < tablelayout.Controls.Count; i++)
            {
                if (tablelayout.Controls[i].GetType().ToString().Contains("TextBox"))
                {
                    TextBox txtBox = (TextBox)tablelayout.Controls[i];
                    txtBox.Text = string.Empty;
                }
                else if (tablelayout.Controls[i].GetType().ToString().Contains("DropDownList"))
                {
                    DropDownList combobox = (DropDownList)tablelayout.Controls[i];
                    combobox.SelectedIndex = 0;
                }

            }
        }

        public void getFromHumanId(ulong Id)
        {
            IList<Human> HumanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(Id);
            if (HumanList != null && HumanList.Count > 0) //added by balaji.TJ 
            {
                humanObj = HumanList[0];
            }
            if (hdnPatInusredPlanId.Value != string.Empty)
            {
                PatInusredPlanId = Convert.ToUInt64(hdnPatInusredPlanId.Value);
            }
            if (humanObj != null)
            {
                txtPolicyHolderLastName.Text = humanObj.Last_Name;
                txtHolderTag.Value = humanObj.Id.ToString();
                txtPolicyHolderFirstName.Text = humanObj.First_Name;
                txtPolicyHolderMiddleName.Text = humanObj.MI;
                txtPolicyHolderAddress.Text = humanObj.Street_Address1;
                txtPolicyHolderZipCode.Text = humanObj.ZipCode;
                txtPolicyHolderCity.Text = humanObj.City;
                txtPolicyHolderState.Text = humanObj.State;
                msktxtPolicyHolderSSN.Text = humanObj.SSN;
                txtPolicyHolderDateOfBirth.Text = humanObj.Birth_Date.ToString("dd-MMM-yyyy");
                txtPolicyHolderSex.Text = humanObj.Sex;
                txtPolicyHolderMaritalStatus.Text = humanObj.Marital_Status;
                txtPatEmploymentStatus.Text = humanObj.Employment_Status;
                txtPatEmployerName.Text = humanObj.Employer_Name;
                msktxtPatWorkPhone.Text = humanObj.Work_Phone_No;
                txtPatWorkExtn.Text = humanObj.Work_Phone_Ext;
                txtPolicyHolderEmail.Text = humanObj.EMail;
                msktxtPolicyHolderHomePhno.Text = humanObj.Home_Phone_No;
                msktxtPolicyHolderCellPhno.Text = humanObj.Cell_Phone_Number;
                txtPolicyHolderDriverLicenseno.Text = humanObj.Driver_License_Num;
                txtPolicyHolderLicenseState.Text = humanObj.Driver_State;
                txtPolicyHolderMedicalRecordNo.Text = humanObj.Medical_Record_Number;

                if(humanObj.PatientInsuredBag != null)
                foreach (PatientInsuredPlan patIns in humanObj.PatientInsuredBag)
                {
                    if (PatInusredPlanId == patIns.Id)
                    {
                        patInsPlan = patIns;
                        Session["patInsPlan"] = patInsPlan;
                        for (int i = 0; i < ddlPatientRelation.Items.Count; i++)
                        {
                            if (ddlPatientRelation.Items[i].Text == patIns.Relationship.ToString())
                            {
                                ddlPatientRelation.SelectedIndex = i;
                            }
                        }
                        // ddlPatientRelation.Text = patIns.Relationship.ToString();
                        ComboBoxSelectedIndexTrigger(ddlPatientRelation);
                        loadPlanGroup(patIns.Insurance_Plan_ID, false);
                        iLoadInsPlanId = patIns.Insurance_Plan_ID;
                        bEnable = false;
                        txtGroupNumber.Text = patIns.Group_Number;
                        txtPolicyHolderID.Text = patIns.Policy_Holder_ID;
                        LoadEligibiltyDetails();
                        // cboInsAssignment.Text = patIns.Assignment;
                        for (int i = 0; i < cboInsAssignment.Items.Count; i++)
                        {
                            if (cboInsAssignment.Items[i].Text == patIns.Assignment)
                            {
                                cboInsAssignment.SelectedIndex = i;
                            }
                        }

                        physicianList = PhyMngr.LoadPhysicianList();
                        if (physicianList != null)
                        {
                            if (patIns.PCP_ID != 0)
                            {
                                var FldList = from h in physicianList where h.Id == patIns.PCP_ID select h;
                                txtPCP.Text = FldList.ToList()[0].PhyLastName + "," + FldList.ToList()[0].PhyFirstName + " " + FldList.ToList()[0].PhyMiddleName + " " + FldList.ToList()[0].PhySuffix;
                                txtPCPNPI.Text = patIns.PCP_NPI;
                                txtPcpTag.Value = patIns.PCP_ID.ToString();
                            }
                            else
                            {
                                txtPCP.Text = "";
                                txtPcpTag.Value = "0";
                            }
                        }
                        // cboPhysicianId.Text = patIns.PCP_ID.ToString();

                        if (patIns.Effective_Start_Date == Convert.ToDateTime("1/1/0001"))
                        {
                            dtpEffectiveStartDate.Text = string.Empty;
                        }
                        else
                        {
                            //dtpEffectiveStartDate.CustomFormat = "dd-MMM-yyyy ";
                            dtpEffectiveStartDate.Text = patIns.Effective_Start_Date.ToString("dd-MMM-yyyy");
                        }

                        if (patIns.Termination_Date == Convert.ToDateTime("1/1/0001"))
                        {
                            dtpTerminationDate.Text = string.Empty;
                        }
                        else
                        {
                            // dtpTerminationDate.CustomFormat = "dd-MMM-yyyy ";
                            dtpTerminationDate.Text = patIns.Termination_Date.ToString("dd-MMM-yyyy");
                        }      //dtpTerminationDate.Value=patIns.Termination_Date;

                        break;
                    }
                }
            }
        }
        public void loadPlanGroup(ulong ulInsPlanID, bool reLoad)
        {
            //get insurance details from insurance_plan table.
            txtInsPlanID.Text = ulInsPlanID.ToString();
            IList<InsurancePlan> insList = InsMngr.GetInsurancebyID(ulInsPlanID);
            if (insList != null && insList.Count > 0) //code added by balaji.TJ
            {
                objInsPlan = insList[0];
            }
            if (objInsPlan != null)
            {
                //set the fields in the form with the values from insurance_plan table.
                txtInsPlanName.Text = objInsPlan.Ins_Plan_Name;
                IList<Carrier> CarrierList = InsMngr.GetCarrierList();
                Carrier objCarrier = null;
                if (CarrierList != null && CarrierList.Count > 0)
                {
                    objCarrier = (from h in CarrierList where h.Id == Convert.ToUInt64(objInsPlan.Carrier_ID) select h).ToList()[0];
                }
                if (objCarrier != null)
                {
                    txtInsCarrierName.Text = objCarrier.Carrier_Name;
                }


                txtInsFinancialClass.Text = objInsPlan.Financial_Class_Name;
                txtInsAddress.Text = objInsPlan.Payer_Addrress1;
                txtInsZipCode.Text = objInsPlan.Payer_Zip;
                txtInsCity.Text = objInsPlan.Payer_City;
                txtInsState.Text = objInsPlan.Payer_State;
                txtInsGovtType.Text = objInsPlan.Govt_Type;
                mskInsPhone.Text = objInsPlan.Payer_Phone_Number;
                msktxtInsFax.Text = objInsPlan.Fax_Number;
                if (objInsPlan.Active.ToUpper() == "Y")
                {
                    cboInsAssignment.Text = "Yes";
                }
                else
                {
                    cboInsAssignment.Text = "No";
                }
                txtClaimAddress.Text = objInsPlan.Claim_Address;
                txtClaimCity.Text = objInsPlan.Claim_City;
                txtClaimState.Text = objInsPlan.Claim_State;
                txtZipCode.Text = objInsPlan.Claim_ZipCode;
                bEnable = true;
            }
        }
        public void LoadEligibiltyDetails()
        {
            if (Convert.ToBoolean(hdnbUpdatePolicy.Value) == false)
            {
                if (hdnMyHumanID.Value != string.Empty)
                {
                    EligList = EligibilityMngr.GetEligDetailsUsingHumanandPolicyHolderID(Convert.ToUInt64(hdnMyHumanID.Value), txtPolicyHolderID.Text);
                }
            }
            else
            {
                if (hdnMyHumanID.Value != string.Empty)
                {
                    EligList = EligibilityMngr.GetEligDetailsUsingHumanandPolicyHolderIDandInsPlanID(Convert.ToUInt64(hdnMyHumanID.Value), txtPolicyHolderID.Text, iLoadInsPlanId);
                }
            }

            if (EligList != null && EligList.Count > 0)
            {
                txtEligibilityVerifiedBy.Text = EligList[0].Eligibility_Verified_By;
                txtEligibilityVerificationDate.Text = EligList[0].Eligibility_Verified_Date.ToString("dd-MMM-yyyy");
                //dtpEffectiveStartDate.Format = DateTimePickerFormat.Custom;
                //dtpEffectiveStartDate.CustomFormat = "dd-MMM-yyyy ";
                dtpEffectiveStartDate.Text = EligList[0].Effective_Date.ToString("dd-MMM-yyyy");
                if (EligList[0].Termination_Date == Convert.ToDateTime("1/1/0001"))
                {
                    dtpTerminationDate.Text = string.Empty;
                }
                else
                {
                    // dtpTerminationDate.CustomFormat = "dd-MMM-yyyy ";
                    dtpTerminationDate.Text = EligList[0].Termination_Date.ToString("dd-MMM-yyyy");
                }

                txtPCPCopay.Text = EligList[0].PCP_Copay.ToString();
                txtSPCCopay.Text = EligList[0].SPC_Copay.ToString();
                txtDeductible.Text = EligList[0].Deductible_For_Plan.ToString();
                txtCoInsurance.Text = EligList[0].Coinsurance.ToString();
                txtCallRepName.Text = EligList[0].Call_Rep_Name;
                txtCallRefNumber.Text = EligList[0].Call_Ref_Number;
                txtCarrierNameEligVerification.Text = EligList[0].Payer_Name;
                txtGroupNumber.Text = EligList[0].Group_Number;
                txtVerificationMode.Text = EligList[0].Eligibility_Type;
                txtEligibilityStatus.Text = EligList[0].Eligibility_Status;
                txtDeductibleMet.Text = Convert.ToString(EligList[0].Deductible_Met_So_Far);
                //Added by priyangh
                txtComments.Text = EligList[0].Comments;
                ////
                if (txtInsPlanID.Text != string.Empty)
                {
                    if (EligList[0].Insurance_Plan_ID != Convert.ToUInt64(txtInsPlanID.Text))
                    {
                        if (EligList[0].Insurance_Plan_ID != 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Verify", "verifiyPlanId();", true);
                            //if (MessageBox.DisplayErrorMessage("420048") == 2) // if (ApplicationObject.erroHandler.DisplayErrorMessage("420048", this.Text) == 2)
                            //{
                            //    txtPolicyHolderID.Text = string.Empty;
                            //    txtPolicyHolderID.Focus();
                            //}
                            ClearEligibilityDetails();
                        }
                    }
                }
            }
        }
        public void ClearEligibilityDetails()
        {
            txtEligibilityVerifiedBy.Text = string.Empty;
            txtEligibilityVerificationDate.Text = string.Empty;
            //dtpEffectiveStartDate.Value = DateTime.MinValue;
            dtpEffectiveStartDate.Text = string.Empty;
            dtpTerminationDate.Text = string.Empty;
            txtPCPCopay.Text = string.Empty;
            txtSPCCopay.Text = string.Empty;
            txtDeductible.Text = string.Empty;
            txtCoInsurance.Text = string.Empty;
            txtCallRepName.Text = string.Empty;
            txtCallRefNumber.Text = string.Empty;
            txtCarrierNameEligVerification.Text = string.Empty;
        }
        public Boolean Validation()
        {
            if (txtInsPlanID.Text.Trim() == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("420042", "Add Insurance POlicy", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Insurance POlicy", "DisplayErrorMessage('420042'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                btnSelectPlan.Focus();
                return false;
            }
            if (txtPolicyHolderID.Text.Trim() == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("420026", "Add Insurance Policy", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Insurance POlicy", "DisplayErrorMessage('420026'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                txtPolicyHolderID.Focus();
                return false;
            }
            if (txtPolicyHolderID.Text != string.Empty)
            {
                string sCarrierIDList = System.Configuration.ConfigurationSettings.AppSettings["MedicareCarrierIDList"];
                string[] CarrierIDList = sCarrierIDList.Split(',');

                IList<InsurancePlan> insList = InsMngr.GetInsurancebyID(Convert.ToUInt64(txtInsPlanID.Text));
                if (insList != null && insList.Count > 0) //code added by balaji.TJ
                {
                    objInsPlan = insList[0];
                }
                if (objInsPlan != null)
                {
                    IList<Carrier> CarrierList = InsMngr.GetCarrierList();

                    if (CarrierIDList.Contains<String>(objInsPlan.Carrier_ID.ToString()) == true)
                    {
                        string sResult = UtilityManager.ValidatePolicyHolderID(txtPolicyHolderID.Text);
                        if (sResult.StartsWith("Fail") == true)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380057','','" + sResult.Split('|')[1] + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            txtPolicyHolderID.Focus();
                            return false;
                        }
                    }
                }
            }
            if (ddlPatientRelation.Text.Trim() == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("420025", "Add Insurance Policy", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Insurance POlicy", "DisplayErrorMessage('420025'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ddlPatientRelation.Focus();
                return false;
            }
            //if (dtpEffectiveStartDate.Text != string.Empty)  //commented Bugid:77947 
            //{
            //    if (Convert.ToDateTime(dtpEffectiveStartDate.Text) == DateTime.MinValue)
            //    {
            //        //ApplicationObject.erroHandler.DisplayErrorMessage("420041", "Add Insurance Policy", this.Page);
            //        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Insurance POlicy", "DisplayErrorMessage('420041'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //        return false;
            //    }
            //}
            if (txtPolicyHolderFirstName.Text.Trim() == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("420043", "Add Insurance Policy", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Insurance POlicy", "DisplayErrorMessage('420043'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                btnAddNewInsured.Focus();
                return false;
            }
            return true;
        }
        public PatientInsuredPlan fillPatientInsuredTableDetails(PatientInsuredPlan patInsured)
        {
            if (txtHolderTag.Value.Trim() != string.Empty)
            {
                patInsured.Insured_Human_ID = Convert.ToUInt64(txtHolderTag.Value);//txtPolicyHolderAccountNo.Text
            }

            if (txtPolicyHolderAccountNo.Text != string.Empty)
            {
                patInsured.Human_ID = Convert.ToUInt64(txtPolicyHolderAccountNo.Text);
            }

            if (ddlPatientRelation.SelectedItem.Text != string.Empty && ddlPatientRelation.SelectedIndex != -1)
            {
                patInsured.Relationship = ddlPatientRelation.SelectedItem.Text.ToUpper();

                string temp = ddlPatientRelation.Items[ddlPatientRelation.SelectedIndex].Value;
                string[] Relation = temp.Split('-');
                if (Relation.Length == 2)
                {
                    patInsured.Relationship_No = Convert.ToInt16(Relation[1]);

                }
                // patInsured.Relationship_No = Convert.ToInt32(ddlPatientRelation.Items[ddlPatientRelation.SelectedIndex].Value);
            }
            //patInsured.Created_By = ClientSession.UserName;
            //if (hdnLocalTime.Value != string.Empty)
            //    patInsured.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);

            if (txtInsPlanID.Text != string.Empty)
            {
                patInsured.Insurance_Plan_ID = Convert.ToUInt64(txtInsPlanID.Text);
            }
            patInsured.Group_Number = txtGroupNumber.Text;
            patInsured.Policy_Holder_ID = txtPolicyHolderID.Text;
            patInsured.Active = "Yes";
            if (dtpEffectiveStartDate.Text != string.Empty)
            {
                patInsured.Effective_Start_Date = Convert.ToDateTime(dtpEffectiveStartDate.Text);
            }
            if (dtpTerminationDate.Text == string.Empty)
            {
                patInsured.Termination_Date = Convert.ToDateTime("1/1/0001");
            }
            else
            {
                patInsured.Termination_Date = Convert.ToDateTime(dtpTerminationDate.Text);
            }
            if (bInsuranceType == true)
            {
                Boolean bFillPrimary = true;

                if (txtAccNo.Text != "") //code added by balaji.TJ 2015-12-17
                {
                    IList<PatientInsuredPlan> patientInsuredPlanList = PatinsMngr.getInsurancePoliciesByHumanId((Convert.ToUInt64(txtAccNo.Text)));

                    var patins = from f in patientInsuredPlanList where f.Insurance_Type == "PRIMARY" select f;
                    if (patins.ToList<PatientInsuredPlan>().Count>0)
                    {
                        bFillPrimary = false;
                    }
                }

                if (bFillPrimary==true)
                    patInsured.Insurance_Type = "PRIMARY";
            }
            if (txtPCPCopay.Text != string.Empty)
            {
                patInsured.PCP_Copay = Convert.ToDouble(txtPCPCopay.Text);
            }
            if (txtSPCCopay.Text != string.Empty)
            {
                patInsured.Specialist_Copay = Convert.ToDouble(txtSPCCopay.Text);
            }
            if (txtDeductible.Text != string.Empty)
            {
                patInsured.Deductible = Convert.ToDouble(txtDeductible.Text);
            }
            if (txtCoInsurance.Text != string.Empty)
            {
                patInsured.Co_Insurance = Convert.ToDouble(txtCoInsurance.Text);
            }
            patInsured.Assignment = cboInsAssignment.Text;
            if (ddlPatientRelation.SelectedItem != null)
            {
                patInsured.Relationship = ddlPatientRelation.SelectedItem.Text;
            }
            if (txtPcpTag.Value != string.Empty)
            {
                patInsured.PCP_ID = Convert.ToUInt64(txtPcpTag.Value);
            }
            else
            {
                patInsured.PCP_ID = 0;
            }
            //patInsured.PCP_Name = txtPCP.Text;
            if (hdnTxtPCP.Value != "")//For bug Id 60036//Control not retaining the assinged value from Script
                patInsured.PCP_Name = hdnTxtPCP.Value;
            else
                patInsured.PCP_Name = txtPCP.Text;
            patInsured.PCP_NPI = txtPCPNPI.Text;
            if (txtDeductibleMet.Text != string.Empty)
            {
                patInsured.Deductible_Met_So_Far = Convert.ToDouble(txtDeductibleMet.Text);
            }
            return patInsured;
        }

        protected void ddlPatientRelation_SelectedIndexChanged1(object sender, EventArgs e)
        {
            bFormCloseCheck = true;
            btnSave.Enabled = true;
            ClearAll(pnlPolicyDemoInfo);
            ClearAll(pnlPolicyHolderInfo);
            ClearAll(pnlInsuranceDetails);
            //if relation is self then load that patient's human details.
            if (ddlPatientRelation.SelectedItem.Text.ToUpper() == "SELF")
            {
                //disable addnewinsured and selectexistinginsured buttons.
                btnSelectExistingInsured.Enabled = false;
                btnAddNewInsured.Enabled = false;
                //patient's first and last name are updated.
                getFromHumanId(Convert.ToUInt64(hdnMyHumanID.Value));

                return;
            }
            btnSelectExistingInsured.Enabled = true;
            btnAddNewInsured.Enabled = true;
        }

        protected void btnAddNewInsured_Click(object sender, EventArgs e)
        {
            AddInsuredRefresh();
        }
        public void ComboBoxColorChangeWhite(DropDownList combobox)
        {
            combobox.Enabled = true;
            combobox.BackColor = System.Drawing.Color.White;
            //combobox.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
            //((Telerik.WinControls.Primitives.FillPrimitive)(combobox.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.White;
            //((RadTextBoxItem)(combobox.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.White;
            //((Telerik.WinControls.Primitives.BorderPrimitive)(combobox.GetChildAt(0).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(200, 224, 255);
        }
        public void TextBoxColorChangeWhite(TextBox txtbox)
        {
            txtbox.ReadOnly = false;
            txtbox.BackColor = System.Drawing.Color.White;
            //((RadTextBoxElement)(txtbox.GetChildAt(0))).BackColor = System.Drawing.Color.White;
            //((RadTextBoxItem)(txtbox.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.White;
            //((Telerik.WinControls.Primitives.FillPrimitive)(txtbox.GetChildAt(0).GetChildAt(1))).BackColor = System.Drawing.Color.White;
            //((Telerik.WinControls.Primitives.BorderPrimitive)(txtbox.GetChildAt(0).GetChildAt(2))).ForeColor = System.Drawing.Color.FromArgb(173, 195, 222);
            //((Telerik.WinControls.Primitives.BorderPrimitive)(txtbox.GetChildAt(0).GetChildAt(2))).BackColor = System.Drawing.Color.White;
        }

        protected void btnPerformEligibilityVerification_Click(object sender, EventArgs e)
        {
            PerformEligibilityVerification();
        }
        public void loadEligibleGroup()
        {
            //EligibilityVerificationDetails is assgined in eligibility form.the values are obtained through eligibilityDetails ilist.
            eligibilityDetails = Eligibility_VerficationManager.EligibilityVerificationDetails;
            if (eligibilityDetails != null && eligibilityDetails.Count > 0)
            {
                ClearEligibilityDetails();
                btnSave.Enabled = true;
                txtEligibilityVerifiedBy.Text = ClientSession.UserName;
                //srividhya commented the below code
                //txtEligibilityVerificationDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                if (hdnLocalTime.Value != "")
                    txtEligibilityVerificationDate.Text = UtilityManager.ConvertToLocal(Convert.ToDateTime(hdnLocalTime.Value)).ToString("dd-MMM-yyyy");

                //for each details in eligibility ilist the values are splitted and assigned.split[0] contains the field name and split[1] contains the value.
                foreach (string s in eligibilityDetails)
                {
                    string[] split = s.Split('!');
                    if (split.Length > 0)
                    {
                        if (split[0].ToUpper() == "EFFECTIVE START DATE")
                        {

                            dtpEffectiveStartDate.Text = Convert.ToDateTime(split[1]).ToString("dd-MMM-yyyy");
                        }
                        else if (split[0].ToUpper() == "TERMINATION DATE")
                        {

                            dtpTerminationDate.Text = Convert.ToDateTime(split[1]).ToString("dd-MMM-yyyy");
                        }
                        else if (split[0].ToUpper() == "PCP COPAY")
                        {
                            txtPCPCopay.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "SPC COPAY")
                        {
                            txtSPCCopay.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "DEDUCTIBLE FOR THE PLAN")
                        {
                            txtDeductible.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "CO INSURANCE")
                        {
                            txtCoInsurance.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "CALL REPRESENTATIVE NAME")
                        {
                            txtCallRepName.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "CALL REFERENCE NUMBER")
                        {
                            txtCallRefNumber.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "COMMENTS")
                        {
                            txtComments.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "FILENAME")
                        {
                            txtFileName.Text = split[1];
                        }

                        else if (split[0].ToUpper() == "CLAIMADDRESS")
                        {
                            txtClaimAddress.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "CLAIMCITY")
                        {
                            txtClaimCity.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "STATE")
                        {
                            txtClaimState.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "ZIPCODE")
                        {
                            txtZipCode.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "ATTENTION")
                        {
                            txtAttention.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "EV TYPE")
                        {
                            txtVerificationMode.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "ELIGIBILITYSTATUS")
                        {
                            txtEligibilityStatus.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "CARRIER FROM EV")
                        {
                            txtCarrierNameEligVerification.Text = split[1];
                        }
                        else if (split[0].ToUpper() == "DEDUCTIBLE MET")
                        {
                            txtDeductibleMet.Text = split[1];
                        }


                    }
                }
            }
        }

        protected void cboInsAssignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            bFormCloseCheck = true;
            if (Convert.ToBoolean(hdnbUpdatePolicy.Value) == true)
            {
                btnSave.Enabled = true;
            }
        }

        protected void btnSelectPlan_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
        }

        protected void btnSelectPlanForSelf_Click(object sender, EventArgs e)
        {
            if (ddlPatientRelation.SelectedItem.Text.Trim() == string.Empty)
            {
                // ApplicationObject.erroHandler.DisplayErrorMessage("420025", "Add insurance Policy", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Insurance POlicy", "DisplayErrorMessage('420025'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ddlPatientRelation.Focus();
                return;

            }
            ulong MyPlanId = 0;
            string CarrierName = string.Empty;
            //select payer form will be opened.
            //frmSelectPayer selectPayer = new frmSelectPayer();
            //selectPayer.View(ref MyPlanId, ref CarrierName);
            //  string link = "frmSelectPayer.aspx?MyPlanId=" + MyPlanId + "&CarrierName=" + CarrierName;
            // MessageBox.Open1(link);
            //myplanid is the variable set in select payer form indicating that the particular plan is selected and the details will be filled here from insurance_plan table.
            if (txtInsPlanID.Text != string.Empty)
            {
                MyPlanId = Convert.ToUInt64(txtInsPlanID.Text);
            }
            if (MyPlanId > 0)
            {
                btnSave.Enabled = true;
                loadPlanGroup(MyPlanId, true);
            }
            if (bEnable == true)
            {
                bFormCloseCheck = true;
                TextBoxColorChangeWhite(txtPolicyHolderID);
                TextBoxColorChangeWhite(txtGroupNumber);
                //ComboBoxColorChangeWhite(cboPhysicianId);
                //ComboBoxColorChangeWhite(cboPhysicianName);
                btnFindPCP.Enabled = true;
                btnPerformEligibilityVerification.Enabled = true;
                btnSave.Enabled = true;
                //commented on 12/11/2013
                // ComboBoxColorChangeWhite(cboInsAssignment);
                bEnable = false;
            }
        }

        protected void btnSelectExistingInsured_Click(object sender, EventArgs e)
        {
            SelectExistingPatient();
        }

        protected void txtPolicyHolderID_TextChanged(object sender, EventArgs e)
        {
            if (txtPolicyHolderID.ReadOnly == true)
            {
                btnSave.Enabled = false;
            }
            if (txtPolicyHolderID.ReadOnly == false)
            {
                btnSave.Enabled = true;
            }

            if (txtPolicyHolderID.Text != string.Empty && txtPolicyHolderID.ReadOnly == false)
            {
                LoadEligibiltyDetails();
            }
            txtGroupNumber.Focus();
        }
        protected void txtGroupNumber_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }


        protected void btnAddNewInsuredRefresh_Click(object sender, EventArgs e)
        {
            AddInsuredRefresh();
        }

        public void AddInsuredRefresh()
        {
            bFormCloseCheck = true;
            btnSave.Enabled = true;
            //if patient relation combo is empty error message is shown to select some relation.
            if (ddlPatientRelation.Text.Trim() == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("420025", "Add Insurance Policy", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Insurance POlicy", "DisplayErrorMessage('420025'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ddlPatientRelation.Focus();
            }
            //if some relation is choosed then addnewinsured button is clicked. when clicked patient information is opened to a new human.
            else
            {
                //0 is passed to intimate the patient info form that its a new entry.
                //frmPatientDemographics patientInfo = new frmPatientDemographics(0, true);
                ////patientInfo.ShowDialog();
                //ulinsuredhumanid is assigned a value from the patient info form so that its corresponding details from human table can be                 loaded.

                if (hdnFindPatientID.Value != string.Empty)
                {

                    getFromHumanId(Convert.ToUInt64(hdnFindPatientID.Value));
                    txtAccNo.Text = hdnFindPatientID.Value;
                }
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Insurance POlicy", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
        }
        public void SelectExistingPatient()
        {
            if (ddlPatientRelation.Text.Trim() == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("420025", "Add Insurance Policy", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Insurance POlicy", "DisplayErrorMessage('420025'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ddlPatientRelation.Focus();

                return;
            }
            //find patient form is opened.the row selected there will be set with the human id in id from select existing variable.with this the human details will be loaded.

            if (hdnFindPatientID.Value != string.Empty)
            {
                getFromHumanId(Convert.ToUInt64(hdnFindPatientID.Value));
                txtAccNo.Text = hdnFindPatientID.Value;
            }
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Insurance POlicy", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        protected void btnSelectPatientRefresh_Click(object sender, EventArgs e)
        {
            SelectExistingPatient();
        }
        public void LoadPlanDetails()
        {
            txtPolicyHolderID.ReadOnly = false;
            txtPolicyHolderID.CssClass = "Editabletxtbox";
            txtPCP.ReadOnly = false;
            txtPCP.CssClass = "Editabletxtbox";
            txtPCPNPI.ReadOnly = false;
            txtPCPNPI.CssClass = "Editabletxtbox";
            txtGroupNumber.ReadOnly = false;
            txtGroupNumber.CssClass = "Editabletxtbox";
            if (ddlPatientRelation.SelectedItem.Text.Trim() == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("420025", "Add insurance Policy", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Insurance POlicy", "DisplayErrorMessage('420025'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ddlPatientRelation.Focus();
                return;

            }
            ulong MyPlanId = 0;
            string CarrierName = string.Empty;
            //select payer form will be opened.
            //frmSelectPayer selectPayer = new frmSelectPayer();
            //selectPayer.View(ref MyPlanId, ref CarrierName);
            //  string link = "frmSelectPayer.aspx?MyPlanId=" + MyPlanId + "&CarrierName=" + CarrierName;
            // MessageBox.Open1(link);
            //myplanid is the variable set in select payer form indicating that the particular plan is selected and the details will be filled here from insurance_plan table.
            if (txtInsPlanID.Text != string.Empty)
            {
                MyPlanId = Convert.ToUInt64(txtInsPlanID.Text);
            }
            if (MyPlanId > 0)
            {
                btnSave.Enabled = true;
                loadPlanGroup(MyPlanId, true);
            }
            if (bEnable == true)
            {
                bFormCloseCheck = true;
                TextBoxColorChangeWhite(txtPolicyHolderID);
                TextBoxColorChangeWhite(txtGroupNumber);
                //ComboBoxColorChangeWhite(cboPhysicianId);
                //ComboBoxColorChangeWhite(cboPhysicianName);
                btnFindPCP.Enabled = true;
                btnPerformEligibilityVerification.Enabled = true;
                btnSave.Enabled = true;
                //btnSave.Enabled = false;
                //commented on 12/11/2013
                //ComboBoxColorChangeWhite(cboInsAssignment);
                bEnable = false;
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MandLabel", "changelabelstyle();", true);

        }

        protected void btnLoadPlan_Click(object sender, EventArgs e)
        {
            LoadPlanDetails();
        }

        protected void btnPerformEligibility_Click(object sender, EventArgs e)
        {
            PerformEligibilityVerification();
        }
        public void PerformEligibilityVerification()
        {
            bFormCloseCheck = true;
            btnSave.Enabled = true;
            //only after the plan is selected the eligibility verification can be performed and also only after the policy holder id is                 given the eligibility verification group box can be performed.so planid textbox and policyholder textbox is checked.
            if (txtPolicyHolderID.Text != string.Empty)
            {
                //eligibility verification form is opened.humanid,patient first name,last name,group number,policyholderid,insurance planid and carrier name is passed in constructor.The values entered in eligibility verification form is not saved in any table                  instead its just passed to the add insurance policy form thru an ilist. from this form it will be saved in pat_insured table.

                // string link = "frmEligibilityVerification.aspx?humanID=" + Convert.ToUInt64(txtPolicyHolderAccountNo.Text) + "&patientName=" + txtPatientFirstName.Text + " " + txtPatientLastName.Text + "&groupNumber=" + txtGroupNumber.Text + "&policyHolderId=" + txtPolicyHolderID.Text + "&insPlanId=" + txtInsPlanID.Text + "&carrierName=" + txtInsCarrierName.Text + "&dtEffectiveDate=" + dtpEffectiveStartDate.SelectedDate.Value + "&dtTerminationDate=" + dtpTerminationDate.SelectedDate;
                // MessageBox.Open(link);
                //method to fill the eligibility verification group box with the entries filled in eligibility verification form. 
                loadEligibleGroup();
                ulong MyPlanId = 0;
                if (txtInsPlanID.Text != string.Empty)
                {
                    MyPlanId = Convert.ToUInt64(txtInsPlanID.Text);
                }
                if (MyPlanId > 0)
                {
                    btnSave.Enabled = true;
                    loadPlanGroup(MyPlanId, true);
                }

                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Insurance POlicy", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            //error message to finish selecting plan and policy holder id to be filled.
            else
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("420026", "Add Insurance Policy", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Insurance POlicy", "DisplayErrorMessage('420026'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                txtPolicyHolderID.Focus();
            }
        }

        public void UpdateAgeinBlob(ulong ulHumanID, DateTime BirthDate)
        {
            XmlDocument itemDoc = new XmlDocument();
            string sXMLContent = String.Empty;
            HumanBlobManager HumanBlobMngr = new HumanBlobManager();
            Human_Blob objHumanblob = null;
            IList<Human_Blob> ilstHumanBlob = HumanBlobMngr.GetHumanBlob(ulHumanID);
            if (ilstHumanBlob.Count > 0)
            {
                objHumanblob = ilstHumanBlob[0];
                sXMLContent = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                itemDoc.LoadXml(sXMLContent);
            }


            // XmlText = new XmlTextReader(strXmlHumanFilePath);
            //itemDoc.Load(XmlText);

            int iAge = 0;
            iAge = UtilityManager.CalculateAge(BirthDate);

            XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
            if (xmlAge != null && xmlAge.Count > 0)
                xmlAge[0].Attributes[0].Value = iAge.ToString();
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
            }

            //XmlText.Close();
            // itemDoc.Save(strXmlHumanFilePath);
            //int trycount = 0;
            //trytosaveagain:
            try
            {
                //itemDoc.Save(strXmlHumanFilePath);
                IList<Human_Blob> ilstUpdateBlob = new List<Human_Blob>();
                byte[] bytes = null;
                try
                {
                    bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);
                }
                catch (Exception ex)
                {

                }
                objHumanblob.Human_XML = bytes;
                ilstUpdateBlob.Add(objHumanblob);
                HumanBlobMngr.SaveHumanBlobWithTransaction(ilstUpdateBlob, string.Empty);
            }
            catch (Exception xmlexcep)
            {
                throw new Exception(xmlexcep.Message.ToString());

                //trycount++;
                //if (trycount <= 3)
                //{
                //    int TimeMilliseconds = 0;
                //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                //    Thread.Sleep(TimeMilliseconds);
                //    string sMsg = string.Empty;
                //    string sExStackTrace = string.Empty;

                //    string version = "";
                //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                //    string[] server = version.Split('|');
                //    string serverno = "";
                //    if (server.Length > 1)
                //        serverno = server[1].Trim();

                //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                //        sMsg = xmlexcep.InnerException.Message;
                //    else
                //        sMsg = xmlexcep.Message;

                //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                //        sExStackTrace = xmlexcep.StackTrace;

                //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                //    string ConnectionData;
                //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                //    {
                //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                //        {
                //            cmd.Connection = con;
                //            try
                //            {
                //                con.Open();
                //                cmd.ExecuteNonQuery();
                //                con.Close();
                //            }
                //            catch
                //            {
                //            }
                //        }
                //    }
                //    goto trytosaveagain;
                //}
            }
        }

    }
}

