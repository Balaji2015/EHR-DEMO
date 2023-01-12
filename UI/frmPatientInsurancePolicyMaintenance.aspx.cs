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
using Acurus.Capella.DataAccess.ManagerObjects;
using NHibernate.Criterion;
using System.Xml;
using System.IO;
using System.Drawing;



namespace Acurus.Capella.UI
{
    public partial class frmPatientInsurancePolicyMaintenance : System.Web.UI.Page
    {
        PatientInsuredPlanManager PatInsuredMngr = new PatientInsuredPlanManager();
        CarrierManager CarrierMngr = new CarrierManager();
        IList<PatientInsuredPlan> patientInsuredPlanList;
        Human objHumanList = new Human();
        PatientInsuredPlan patInsured = new PatientInsuredPlan();
        ulong ulMyHumanID;
        DataTable dt = null;
        Human InsuredHumanList = new Human();
        string sMyPayerName = string.Empty, sMyRelationship = string.Empty, sMyInsuranceType = string.Empty;
        DataTable dtSettype = null;
        InsurancePlanManager InsMngr = new InsurancePlanManager();
        HumanManager HumanMngr = new HumanManager();
        StaticLookupManager StaticLookupMngr = new StaticLookupManager();
        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow r in grdPolicyInformation.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    Page.ClientScript.RegisterForEventValidation
                            (r.UniqueID + "$ctl00");
                    Page.ClientScript.RegisterForEventValidation
                            (r.UniqueID + "$ctl01");
                    //Page.ClientScript.RegisterForEventValidation
                    //       (r.UniqueID + "$ctl02");
                }
            }

            base.Render(writer);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Patient Insurance Policy Maintenance" + "-" + ClientSession.UserName;
            if (!IsPostBack)
            {
                btnMakeActive.Enabled = false;
                btnOK.Enabled = false;
                btnMakeInactive.Enabled = false;
                if (System.Text.RegularExpressions.Regex.IsMatch(Request["HumanId"], "^[0-9]*$") == true)
                {
                    ulMyHumanID = Convert.ToUInt64(Request["HumanId"]);
                }
                hdnHumanID.Value = ulMyHumanID.ToString();
                //to enter first name,last name and account number field in the form.
                //IList<Human> humanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(ulMyHumanID);
                IList<Human> humanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(ulMyHumanID);
                // Assign PatientStrip Values
                string phoneno = "";
                string FileName = "Human" + "_" + ulMyHumanID + ".xml";
                string sPatientSex = string.Empty;
                Human objFillHuman = new Human();
                IList<string> ilstGeneralPlanTagList = new List<string>();
                ilstGeneralPlanTagList.Add("HumanList");
                IList<Human> lsthuman = new List<Human>();

                IList<object> ilstGeneralPlanBlobFinal = new List<object>();
                ilstGeneralPlanBlobFinal = UtilityManager.ReadBlob(ulMyHumanID, ilstGeneralPlanTagList);
                if (ilstGeneralPlanBlobFinal != null && ilstGeneralPlanBlobFinal.Count > 0)
                {
                    if (ilstGeneralPlanBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstGeneralPlanBlobFinal[0]).Count; iCount++)
                        {
                            objFillHuman = (Human)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount];
                            lsthuman.Add(objFillHuman);
                        }
                    }
                }
               
          //  retry:
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                //if (File.Exists(strXmlFilePath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    try
                //    {
                //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //        {
                //            itemDoc.Load(fs);

                //            XmlNodeList xmlhumanList = itemDoc.GetElementsByTagName("Human");
                //            Human objFillHuman = new Human();

                //            if (xmlhumanList != null && xmlhumanList.Count > 0)
                //            {
                //                if (System.Text.RegularExpressions.Regex.IsMatch(xmlhumanList[0].Attributes.GetNamedItem("Id").Value, "^[0-9]*$") == true)
                //                {
                //                    objFillHuman.Id = Convert.ToUInt64(xmlhumanList[0].Attributes.GetNamedItem("Id").Value);
                //                }
                //                objFillHuman.Birth_Date = Convert.ToDateTime(xmlhumanList[0].Attributes.GetNamedItem("Birth_Date").Value);
                //                objFillHuman.First_Name = xmlhumanList[0].Attributes.GetNamedItem("First_Name").Value;
                //                objFillHuman.Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Last_Name").Value;
                //                objFillHuman.MI = xmlhumanList[0].Attributes.GetNamedItem("MI").Value;
                //                objFillHuman.Sex = xmlhumanList[0].Attributes.GetNamedItem("Sex").Value;
                //                objFillHuman.Suffix = xmlhumanList[0].Attributes.GetNamedItem("Suffix").Value;
                //                objFillHuman.Medical_Record_Number = xmlhumanList[0].Attributes.GetNamedItem("Medical_Record_Number").Value;
                //                objFillHuman.Home_Phone_No = xmlhumanList[0].Attributes.GetNamedItem("Home_Phone_No").Value;
                //                objFillHuman.Human_Type = xmlhumanList[0].Attributes.GetNamedItem("Human_Type").Value;
                //                objFillHuman.Patient_Account_External = xmlhumanList[0].Attributes.GetNamedItem("Patient_Account_External").Value;
                //                objFillHuman.Cell_Phone_Number = xmlhumanList[0].Attributes.GetNamedItem("Cell_Phone_Number").Value;
                //                humanList.Add(objFillHuman);
                //            }



                //            if (humanList != null && humanList.Count > 0)
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
                //            if (objFillHuman.Sex != string.Empty)
                //            {
                //                if (objFillHuman.Sex.Substring(0, 1).ToUpper() == "U")
                //                {
                //                    sPatientSex = "UNK";
                //                }
                //                else
                //                {
                //                    sPatientSex = objFillHuman.Sex.Substring(0, 1);
                //                }
                //            }
                //            else
                //            {
                //                sPatientSex = "";
                //            }
                //            fs.Close();
                //            fs.Dispose();
                //        }
                //    }
                //    catch(Exception ex)
                //    {
                //        UtilityManager.GenerateXML(ulMyHumanID.ToString(), "Human");
                //        //return;
                //        goto retry;
                //    }
                //}

                if (humanList != null && humanList.Count > 0)     //code added by balaji.TJ 2015-12-17          
                    objHumanList = humanList[0];
                if (objHumanList != null)
                {
                    txtPatientLastName.Text = objHumanList.Last_Name;
                    txtPatientFirstName.Text = objHumanList.First_Name;
                    txtAccountNo.Text = ulMyHumanID.ToString();
                    txtPatientExternalAccNo.Text = objHumanList.Patient_Account_External.ToString();
                    txtPatientDOB.Text = objHumanList.Birth_Date.ToString("dd-MMM-yyyy");
                    txtPatientSex.Text = objHumanList.Sex;
                    txtPateintType.Text = objHumanList.Human_Type;
                }
                chkShowActiveOnly.Checked = true;
                loadGrid();
                //if (grdPolicyInformation.RowCount > 0)
                //{
                //    grdPolicyInformation.CurrentRow = grdPolicyInformation.Rows[0];
                //    grdPolicyInformation.CurrentRow.VisualElement.Focus();
                //}
                //DynamicControlSettingsUtility objDynamicControlSettingsUtility = new DynamicControlSettingsUtility();
                //objDynamicControlSettingsUtility.GetDynamicControlsStyle(this);

                if (Request["CurrentProcess"] != null)
                {
                    hdnCurrentProcess.Value = Request["CurrentProcess"];
                    if (hdnCurrentProcess.Value.Contains("QC"))
                    {
                        btnAddInsuranceForSelf.Enabled = false;
                        btnAddInsurancePolicy.Enabled = false;
                        btnMakeActive.Enabled = false;
                        btnMakeInactive.Enabled = false;
                        btnSetPriorityAsPrimary.Enabled = false;
                        btnSetPriorityAsSecondary.Enabled = false;
                        btnSetPriorityAsTertiary.Enabled = false;
                        btnUpdateInsuredDemographics.Enabled = false;

                    }
                }
                //Added by priyangha//
                if (Request["ParentScreen"] != null)
                    if (Request["ParentScreen"] == "CaptureAuthorization")
                        btnOK.Enabled = true;
                //srividhya added on 03-feb-2014
                if (Request.QueryString["EncounterId"] != null)
                    hdnEncounterID.Value = Request.QueryString["EncounterId"].ToString();
                else if(ClientSession.EncounterId!=null&&ClientSession.EncounterId!=0)
                    hdnEncounterID.Value = ClientSession.EncounterId.ToString();
                else
                    hdnEncounterID.Value = "0";

                if (Request["ScreenMode"] != null)
                    if (Request["ScreenMode"] == "PerformEv")
                        btnPerformEV.Enabled = false;


                divPatientstrip.InnerText = objHumanList.Last_Name + "," + objHumanList.First_Name +"  " + objHumanList.MI + "  " + objHumanList.Suffix + "   |   " +
                       objHumanList.Birth_Date.ToString("dd-MMM-yyyy") + "   |   " + sPatientSex + "   |   Acc #:" + objHumanList.Id +
                      "   |   " + "Med Rec #:" + objHumanList.Medical_Record_Number + "   |   " + "Phone #:" + phoneno + "   |   Patient Type:" + objHumanList.Human_Type;
                     
            }
        }

        protected void chkShowActiveOnly_CheckedChanged(object sender, EventArgs e)
        {
            loadGrid();
        }
        public void loadGrid()
        {
            //clear the grid rows.
            grdPolicyInformation.DataSource = null;
            grdPolicyInformation.DataBind();
            // objHumanList = EncounterManager.Instance.GetHumanByHumanID( Convert.ToUInt64(Session["ulMyHumanID"]));
            IList<Human> humanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(hdnHumanID.Value));
            if (humanList != null && humanList.Count > 0) //code added by balaji.TJ 2015-12-17         
                objHumanList = humanList[0];
            if (objHumanList != null)
            {
                if (objHumanList.PatientInsuredBag != null && objHumanList.PatientInsuredBag.Count > 0)
                {
                    //IList<PatientInsuredPlan> PatInsOrderedList = objHumanList.PatientInsuredBag.OrderBy(x => x.Sort_Order).ToList<PatientInsuredPlan>();
                    IList<PatientInsuredPlan> PatInsOrderedList = objHumanList.PatientInsuredBag.OrderBy(x => x.Insurance_Type).ToList<PatientInsuredPlan>();
                    dt = new DataTable();
                    Session["dt"] = dt;
                    dt.Columns.Add("Policy Holder ID", typeof(string));
                    dt.Columns.Add("Plan ID", typeof(string));
                    dt.Columns.Add("Plan Name", typeof(string));
                    dt.Columns.Add("Group Number", typeof(string));
                    dt.Columns.Add("Insurance Type", typeof(string));
                    dt.Columns.Add("Patient Name", typeof(string));
                    dt.Columns.Add("Relationship", typeof(string));
                    dt.Columns.Add("Active", typeof(string));
                    dt.Columns.Add("Effective Start Date", typeof(string));
                    dt.Columns.Add("Termination Date", typeof(string));
                    dt.Columns.Add("Insured Name", typeof(string));
                    dt.Columns.Add("Insured Human ID", typeof(string));
                    dt.Columns.Add("Insured DOB", typeof(string));
                    dt.Columns.Add("Insured Sex", typeof(string));
                    dt.Columns.Add("Id", typeof(string));
                    dt.Columns.Add("Carrier Name", typeof(string));
                    dt.Columns.Add("CarrierID", typeof(string));
                    dt.Columns.Add("PlanType", typeof(string));
                    foreach (PatientInsuredPlan obj in PatInsOrderedList)//objHumanList.PatientInsuredBag)
                    {
                        if (((chkShowActiveOnly.Checked == true) && (obj.Active.ToUpper() == "YES")) || ((chkShowActiveOnly.Checked == false)))// && ((obj.Active == "No") || (obj.Active == null))))                    
                            loadGridRows(obj);
                    }
                    if (grdPolicyInformation.Rows.Count == 0)
                        CreateEmptyHeader();
                }
                else
                    CreateEmptyHeader();
            }
        }
        public void loadGridRows(PatientInsuredPlan obj)
        {
            ulong insPlanId;
            //store the insurance plan id in a variable.
            //get the insurance plan details from insurance_plan table using insurance plan id.
            IList<Human> humanInsList = HumanMngr.GetPatientDetailsUsingPatientInformattion(obj.Insured_Human_ID);
            if (humanInsList != null && humanInsList.Count > 0) //code added by balaji.tj 2015-12-17        
                InsuredHumanList = humanInsList[0];
            ulong InsID=0;
            if (Request["Insurance_Plan_ID"] != null && Request["Insurance_Plan_ID"] != "undefined")
                InsID = Convert.ToUInt64(Request["Insurance_Plan_ID"]);
            InsurancePlan objInsurancePlan = new InsurancePlan();
            if (dt != null)
            {
                DataRow dr = dt.NewRow();
                dr["Policy Holder ID"] = obj.Policy_Holder_ID;
                dr["Plan ID"] = obj.Insurance_Plan_ID;
                insPlanId = Convert.ToUInt64(dr[1]);
                IList<InsurancePlan> insList = InsMngr.GetInsurancebyID(insPlanId);
                if (insList != null && insList.Count > 0) //code added by balaji.TJ 2015-12-17               
                    objInsurancePlan = insList[0];
                if (objInsurancePlan != null)
                {
                    dr["Plan Name"] = objInsurancePlan.Ins_Plan_Name;
                    Carrier objcarrierName = CarrierMngr.GetCarrierUsingId(Convert.ToUInt64(objInsurancePlan.Carrier_ID));
                    if (objcarrierName != null)
                        dr["Carrier Name"] = objcarrierName.Carrier_Name;
                    //Added by srividhya on 6-Aug-2014
                    dr["CarrierID"] = objInsurancePlan.Carrier_ID.ToString();
                    dr["PlanType"] = objInsurancePlan.Financial_Class_Name;
                }
                dr["Group Number"] = obj.Group_Number;
                dr["Insurance Type"] = obj.Insurance_Type;
                dr["Patient Name"] = txtPatientLastName.Text + " " + txtPatientFirstName.Text;
                dr["Relationship"] = obj.Relationship;
                dr["Active"] = obj.Active;
                if (obj.Effective_Start_Date != DateTime.MinValue)
                    dr["Effective Start Date"] = obj.Effective_Start_Date.ToString("dd-MMM-yyyy");
                if (obj.Termination_Date != DateTime.MinValue)
                    dr["Termination Date"] = obj.Termination_Date.ToString("dd-MMM-yyyy");
                if (InsuredHumanList != null)
                {
                    dr["Insured Name"] = InsuredHumanList.Last_Name + " " + InsuredHumanList.First_Name;
                    dr["Insured DOB"] = InsuredHumanList.Birth_Date.ToString("dd-MMM-yyyy");
                    dr["Insured Sex"] = InsuredHumanList.Sex;
                }
                dr["Insured Human ID"] = obj.Insured_Human_ID;
                dr["Id"] = obj.Id;

              

                dt.Rows.Add(dr);
               
            }
            grdPolicyInformation.DataSource = dt;
            grdPolicyInformation.DataBind();
            for(int i=0;i<grdPolicyInformation.Rows.Count;i++) /*bug id: 61619	*/
            {
                string sValue = grdPolicyInformation.Rows[i].Cells[15].Text.ToString();
                if (InsID != null && InsID != 0 && InsID.ToString() == sValue)
                {
                    grdPolicyInformation.SelectedIndex = i;
                    hdnSelectedIndex.Value = i.ToString();
                    return;
                  
                }
            }
        }

        protected void btnAddInsurancePolicy_Click(object sender, EventArgs e)
        {
            //frmAddInsurancePolicies add = new frmAddInsurancePolicies(ulMyHumanID, txtPatientLastName.Text, txtPatientFirstName.Text, txtPatientExternalAccNo.Text);
            //add.ShowDialog();
            //  Server.Transfer("frmAddInsurancePolicies.aspx?HumanId=" + Session["ulMyHumanID"].ToString() + "&LastName=" + txtPatientLastName.Text + "&FirstName=" + txtPatientFirstName.Text + "&ExAccountNo=" + txtAccountNo.Text);
            if (chkShowActiveOnly.Checked == false)
                chkShowActiveOnly.Checked = true;
            loadGrid();
            // grdPolicyInformation.SelectedRos;
        }

        protected void btnSetPriorityAsPrimary_Click(object sender, EventArgs e)
        {
            setInsuranceType("PRIMARY");
        }
        public void setInsuranceType(string insuranceType)
        {
            IList<PatientInsuredPlan> PatientinsuredList = new List<PatientInsuredPlan>();
            string sOriginalInsType = string.Empty;
            //error message displayed if none of the rows are selected in the grid.
            if (grdPolicyInformation.SelectedRow == null)
            {
                // ApplicationObject.erroHandler.DisplayErrorMessage("420021", "Patient Insurance Policy Maintanence", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Insurance Policy Maintanence", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420021');", true);
            }
            //when the insurance type value is not primary.
            else
            {
                IList<StaticLookup> Staticlist = StaticLookupMngr.getStaticLookupByFieldName("INSURANCE TYPE");
                //insuranceCheck = true;
                //get the patient insurance details using human id from pat_insured table. number of policies for the human will be loaded                  in the list.
                if (hdnHumanID.Value != "") //code added by balaji.TJ 2015-12-17
                    patientInsuredPlanList = PatInsuredMngr.getInsurancePoliciesByHumanId((Convert.ToUInt64(hdnHumanID.Value)));
                if (patientInsuredPlanList != null && patientInsuredPlanList.Count > 0) //code added by balaji.TJ 2015-12-17
                {
                    for (int i = 0; i < patientInsuredPlanList.Count; i++)
                    {
                        patInsured = patientInsuredPlanList[i];
                        if (patientInsuredPlanList[i].Id == Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[14].Text))
                        {
                            sOriginalInsType = patientInsuredPlanList[i].Insurance_Type;
                            patientInsuredPlanList[i].Insurance_Type = insuranceType;
                            patInsured.Insurance_Type = insuranceType;
                            if (Staticlist != null && Staticlist.Count > 0)
                            {
                                for (int j = 0; j < Staticlist.Count; j++)
                                {
                                    if (Staticlist[j].Value == insuranceType)
                                    {
                                        patInsured.Sort_Order = Staticlist[j].Sort_Order;
                                        patientInsuredPlanList[i].Sort_Order = Staticlist[j].Sort_Order;
                                        break;
                                    }
                                }
                            }
                        }
                        //if the insurance type for the any row is primary then it is changed as old primary and the new one is primary.
                        if ((patientInsuredPlanList[i].Insurance_Type.ToUpper() == insuranceType.ToUpper()) && (patientInsuredPlanList[i].Id != Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[14].Text)))
                        {
                            patInsured.Insurance_Type = "OLD " + insuranceType;
                            patientInsuredPlanList[i].Insurance_Type = "OLD " + insuranceType;
                            if (Staticlist != null && Staticlist.Count > 0)
                            {
                                for (int j = 0; j < Staticlist.Count; j++)
                                {
                                    if (Staticlist[j].Value == "OLD " + insuranceType)
                                    {
                                        patInsured.Sort_Order = Staticlist[j].Sort_Order;
                                        patientInsuredPlanList[i].Sort_Order = Staticlist[j].Sort_Order;
                                        break;
                                    }
                                }
                            }
                        }
                        if (patInsured.Id != 0)
                        {
                            PatientinsuredList.Add(patInsured);
                        }
                    }

                    if (insuranceType == "PRIMARY")
                    {
                        PatInsuredMngr.BatchUpdatePatInsured(PatientinsuredList.ToArray<PatientInsuredPlan>(), Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[17].Text), string.Empty);

                        if (Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[17].Text) != 0 && PatientinsuredList.Count > 0)
                        {
                            IList<Human> lstUpdateHuman = new List<Human>();

                            lstUpdateHuman = HumanMngr.GetPatientDetailsUsingPatientInformattion(PatientinsuredList[0].Human_ID);
                            if (lstUpdateHuman.Count() > 0)
                            {
                                lstUpdateHuman[0].Primary_Carrier_ID = Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[17].Text);
                                HumanMngr.UpdateBatchToHuman(lstUpdateHuman[0], null, null, grdPolicyInformation.SelectedRow.Cells[3].Text);
                            }
                        }
                    }
                    else
                    {
                        PatInsuredMngr.BatchUpdatePatInsured(PatientinsuredList.ToArray<PatientInsuredPlan>(), 0, string.Empty);

                        if (PatientinsuredList.Count > 0 && sOriginalInsType == "PRIMARY")
                        {
                            IList<Human> lstUpdateHuman = new List<Human>();

                            lstUpdateHuman = HumanMngr.GetPatientDetailsUsingPatientInformattion(PatientinsuredList[0].Human_ID);
                            if (lstUpdateHuman.Count() > 0)
                            {
                                lstUpdateHuman[0].Primary_Carrier_ID = 0;
                                HumanMngr.UpdateBatchToHuman(lstUpdateHuman[0], null, null, string.Empty);
                            }
                        }
                    }
                    
                    grdPolicyInformation.DataSource = null;
                    grdPolicyInformation.DataBind();
                    dtSettype = new DataTable();
                    Session["dt"] = dtSettype;
                    dtSettype.Columns.Add("Policy Holder ID", typeof(string));
                    dtSettype.Columns.Add("Plan ID", typeof(string));
                    dtSettype.Columns.Add("Plan Name", typeof(string));
                    dtSettype.Columns.Add("Group Number", typeof(string));
                    dtSettype.Columns.Add("Insurance Type", typeof(string));
                    dtSettype.Columns.Add("Patient Name", typeof(string));
                    dtSettype.Columns.Add("Relationship", typeof(string));
                    dtSettype.Columns.Add("Active", typeof(string));
                    dtSettype.Columns.Add("Effective Start Date", typeof(string));
                    dtSettype.Columns.Add("Termination Date", typeof(string));
                    dtSettype.Columns.Add("Insured Name", typeof(string));
                    dtSettype.Columns.Add("Insured Human ID", typeof(string));
                    dtSettype.Columns.Add("Insured DOB", typeof(string));
                    dtSettype.Columns.Add("Insured Sex", typeof(string));
                    dtSettype.Columns.Add("Id", typeof(string));
                    dtSettype.Columns.Add("Carrier Name", typeof(string));
                    dtSettype.Columns.Add("CarrierID", typeof(string));
                    dtSettype.Columns.Add("PlanType", typeof(string));
                    IList<PatientInsuredPlan> PatInsOrderedList = patientInsuredPlanList.OrderBy(x => x.Sort_Order).ToList<PatientInsuredPlan>();
                    foreach (PatientInsuredPlan p in PatInsOrderedList) //patientInsuredPlanList)
                    {
                        if ((chkShowActiveOnly.Checked == true && p.Active.ToUpper() == "YES") || (chkShowActiveOnly.Checked == false))//&& p.Active.ToUpper() == "NO"))
                        {
                            ulong insPlanId;
                            //store the insurance plan id in a variable.
                            //get the insurance plan details from insurance_plan table using insurance plan id.
                            IList<Human> HumanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(p.Insured_Human_ID);
                            if (HumanList != null && HumanList.Count > 0)  //code added by balaji.tj 2015-12-17                          
                                InsuredHumanList = HumanList[0];
                            InsurancePlan objInsurancePlan = new InsurancePlan();
                            if (dtSettype != null)
                            {
                                DataRow dr = dtSettype.NewRow();
                                dr["Policy Holder ID"] = p.Policy_Holder_ID;
                                dr["Plan ID"] = p.Insurance_Plan_ID;
                                insPlanId = Convert.ToUInt64(dr[1]);
                                IList<InsurancePlan> insList = InsMngr.GetInsurancebyID(insPlanId);
                                if (insList != null && insList.Count > 0)
                                    objInsurancePlan = insList[0];
                                if (objInsurancePlan != null)
                                {
                                    dr["Plan Name"] = objInsurancePlan.Ins_Plan_Name;
                                    Carrier objcarrierName = CarrierMngr.GetCarrierUsingId(Convert.ToUInt64(objInsurancePlan.Carrier_ID));
                                    if (objcarrierName != null)
                                        dr["Carrier Name"] = objcarrierName.Carrier_Name;
                                    //Added by srividhya on 6-Aug-2014
                                    dr["CarrierID"] = objInsurancePlan.Carrier_ID.ToString();
                                    dr["PlanType"] = objInsurancePlan.Financial_Class_Name;
                                }
                                dr["Group Number"] = p.Group_Number;
                                dr["Insurance Type"] = p.Insurance_Type;
                                dr["Patient Name"] = txtPatientLastName.Text + " " + txtPatientFirstName.Text;
                                dr["Relationship"] = p.Relationship;
                                dr["Active"] = p.Active;
                                if (p.Effective_Start_Date != DateTime.MinValue)
                                    dr["Effective Start Date"] = p.Effective_Start_Date.ToString("dd-MMM-yyyy");
                                if (p.Termination_Date != DateTime.MinValue)
                                    dr["Termination Date"] = p.Termination_Date.ToString("dd-MMM-yyyy");
                                if (InsuredHumanList != null)
                                    dr["Insured Name"] = InsuredHumanList.Last_Name + " " + InsuredHumanList.First_Name;
                                dr["Insured Human ID"] = p.Insured_Human_ID;
                                dr["Insured DOB"] = InsuredHumanList.Birth_Date.ToString("dd-MMM-yyyy");
                                dr["Insured Sex"] = InsuredHumanList.Sex;
                                dr["Id"] = p.Id;
                                dtSettype.Rows.Add(dr);
                                // ((DataTable)Session["dt"]).Rows.Add(dr);
                            }
                        }
                    }
                    grdPolicyInformation.DataSource = dtSettype;
                    grdPolicyInformation.DataBind();
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "patient Insu", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnSetPriorityAsSecondary_Click(object sender, EventArgs e)
        {
            setInsuranceType("SECONDARY");
        }

        protected void btnSetPriorityAsTertiary_Click(object sender, EventArgs e)
        {
            setInsuranceType("TERTIARY");
        }

        protected void btnMakeActive_Click(object sender, EventArgs e)
        {
            if (grdPolicyInformation.SelectedRow == null)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("420023", "PatientInsurancePolicyMaintenance", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Insurance Policy Maintanence", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420023');", true);
            }
            else
            {
                activeStatus("Yes");
                btnMakeActive.Enabled = false;
                btnMakeInactive.Enabled = true;
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "patient Insu", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnMakeInactive_Click(object sender, EventArgs e)
        {

          
            if (grdPolicyInformation.SelectedRow == null)
            {
                //  ApplicationObject.erroHandler.DisplayErrorMessage("420024", "PatientInsurancePolicyMaintenance", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Insurance Policy Maintanence", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('420024');", true);
            }
            else
            {
                if (grdPolicyInformation.SelectedRow.Cells[4].Text.ToString().ToUpper() == "YES")
                {
                    activeStatus("No");
                    chkShowActiveOnly_CheckedChanged(sender, e);
                    //btnMakeActive.Enabled = true;
                    //btnMakeInactive.Text = "Make Active";
                    // btnMakeInactive.Enabled = false;
                    grdPolicyInformation_SelectedIndexChanged(sender, e);
                    grdPolicyInformation.SelectedIndex = -1;
                    btnMakeInactive.Enabled = false;
                }
                else
                {
                    activeStatus("Yes");
                    //btnMakeActive.Enabled = false;
                    // btnMakeInactive.Text = "Make Inactive";
                    grdPolicyInformation_SelectedIndexChanged(sender, e);
                    btnMakeInactive.Enabled = false;
                    grdPolicyInformation.SelectedIndex = -1;
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "patient Insu", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        public void activeStatus(string status)
        {
            patInsured = PatInsuredMngr.getInsuranceDetailsUsingPatInsuredId(Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[14].Text));
            if (patInsured != null)
                patInsured.Active = status;
            PatInsuredMngr.UpdatePatInsured(patInsured, string.Empty);
            if (chkShowActiveOnly.Checked == true && status == "No")
            {
                grdPolicyInformation.SelectedRow.Cells[4].Text = status;
                grdPolicyInformation.DeleteRow(grdPolicyInformation.SelectedIndex);
            }
            else
                grdPolicyInformation.SelectedRow.Cells[4].Text = status;
            //grdPolicyInformation.           
        }

        protected void btnViewUpdatePolicyInformation_Click(object sender, EventArgs e)
        {
            if (grdPolicyInformation.SelectedRow == null)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("420021", "PatientInsurancePolicyMaintenance", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Insurance Policy Maintanence", "DisplayErrorMessage('420021');", true);
                grdPolicyInformation.Focus();
            }
            else
            {
                //code comment by balaji.TJ 2015-12-17
                // ulong insplan = 0, inshumanid = 0;              
                //if (grdPolicyInformation.SelectedRow.Cells[14].Text != string.Empty)
                //{
                //    //insplan = Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[13].Text);
                //}
                //if (grdPolicyInformation.SelectedRow.Cells[13].Text != string.Empty)
                //{
                //    // inshumanid = Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[12].Text);
                //}
                //===========================
                //frmAddInsurancePolicies addInsPolicy = new frmAddInsurancePolicies(insplan, inshumanid, ulMyHumanID);
                //addInsPolicy.ShowDialog();
                //btnViewUpdatePolicyInformation.Attributes.Add("onClick","openwindow('"++'")"
                //string link = "frmAddInsurancePolicies.aspx?HumanId=" + Session["ulMyHumanID"] + "&InsPlan=" + insplan + "&InsHumanId=" + inshumanid;
                //MessageBox.Open(link);
                //Server.Transfer("frmAddInsurancePolicies.aspx?HumanId=" + Session["ulMyHumanID"] + "&InsPlan=" + insplan + "&InsHumanId=" + inshumanid);
                loadGrid();
            }
        }

        protected void btnUpdateInsuredDemographics_Click(object sender, EventArgs e)
        {
            //if (grdPolicyInformation.SelectedRow == null)
            //{
            //    MessageBox.DisplayErrorMessage("420021");//ApplicationObject.erroHandler.DisplayErrorMessage("420021", this.Text);
            //    grdPolicyInformation.Focus();
            //}
            //else
            //{
            //
            //    ulong ulhumanid = 0;
            //    if (grdPolicyInformation.SelectedRow.Cells[13].Text != null)
            //    {
            //        ulhumanid = Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[12].Text);
            //    }
            //    Server.Transfer("frmPatientDemographics.aspx?HumanId=" + Session["ulMyHumanID"] + "&bInsurance=" + false);
            //}
        }

        protected void grdPolicyInformation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdPolicyInformation.Rows.Count > 0)
            {
                try
                {
                   
                    if (grdPolicyInformation.SelectedRow != null)
                    {
                        //code comment by balaji.TJ 2015-12-17
                        //if (grdPolicyInformation.SelectedRow.Cells[4].Text.ToString().ToUpper() == "NO")
                        //{
                        //    // btnMakeInactive.Enabled = false;
                        //    //btnMakeActive.Enabled = true;
                        //    // btnMakeInactive.Text = "Make Inactive";
                        //}
                        //else if (grdPolicyInformation.SelectedRow.Cells[4].Text.ToString().ToUpper() == "YES")
                        //{
                        //    //btnMakeInactive.Enabled = true;
                        //    //btnMakeInactive.Text = "Make Active";
                        //    //btnMakeActive.Enabled = false;
                        //}
                        hdnSelectedIndex.Value = grdPolicyInformation.SelectedIndex.ToString();
                        hdnInsuredHumanID.Value = grdPolicyInformation.SelectedRow.Cells[11].Text;
                        if (hdnCurrentProcess.Value.Contains("QC"))
                        {
                            btnAddInsuranceForSelf.Enabled = false;
                            btnAddInsurancePolicy.Enabled = false;
                            btnMakeActive.Enabled = false;
                            btnMakeInactive.Enabled = false;
                            btnSetPriorityAsPrimary.Enabled = false;
                            btnSetPriorityAsSecondary.Enabled = false;
                            btnSetPriorityAsTertiary.Enabled = false;
                            btnUpdateInsuredDemographics.Enabled = false;
                        }
                    }
                }
                catch
                {

                }
            }
        }

        protected void grdPolicyInformation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (TableCell myCell in e.Row.Cells)
            {
                myCell.Style.Add("word-break", "break-all");
                myCell.Width = 100;
            }
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Attributes.Add("style", "display:none");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes.Add("style", "display:none");
                LinkButton _singleClickButton = (LinkButton)e.Row.Cells[16].Controls[0];
                string _jsSingle =
                ClientScript.GetPostBackClientHyperlink(_singleClickButton, "");
                _jsSingle = _jsSingle.Insert(11, "setTimeout(\"");
                _jsSingle += "\", 300)";
                e.Row.Attributes["onclick"] = _jsSingle;
                //LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[16].Controls[0];
                //string _jsDouble =
                //ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
                //e.Row.Attributes["ondblclick"] = _jsDouble;
            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';";
            //    e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
            //    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.grdPolicyInformation, "Select$" + e.Row.RowIndex);
            //}
        }

        protected void btnAddInsuranceForSelf_Click(object sender, EventArgs e)
        {
            //frmAddInsurancePolicies add = new frmAddInsurancePolicies(ulMyHumanID, txtPatientLastName.Text, txtPatientFirstName.Text, txtPatientExternalAccNo.Text, "Self");
            //add.ShowDialog();
            if (chkShowActiveOnly.Checked == false)
                chkShowActiveOnly.Checked = true;
            loadGrid();
        }
        public void loadGridRowsForActive(PatientInsuredPlan obj)
        {
            ulong insPlanId;
            //store the insurance plan id in a variable.
            //get the insurance plan details from insurance_plan table using insurance plan id.
            IList<Human> humanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(obj.Insured_Human_ID);
            if (humanList != null && humanList.Count > 0)   //added by balaji.TJ 2015-12-17         
                InsuredHumanList = humanList[0];
            InsurancePlan objInsurancePlan = new InsurancePlan();
            if (dtSettype != null)
            {
                DataRow dr = dtSettype.NewRow();
                dr["Policy Holder ID"] = obj.Policy_Holder_ID;
                dr["Plan ID"] = obj.Insurance_Plan_ID;
                insPlanId = Convert.ToUInt64(dr[1]);
                objInsurancePlan = InsMngr.GetInsurancebyID(insPlanId)[0];
                if (objInsurancePlan != null)
                {
                    dr["Plan Name"] = objInsurancePlan.Ins_Plan_Name;
                    Carrier objcarrierName = CarrierMngr.GetCarrierUsingId(Convert.ToUInt64(objInsurancePlan.Carrier_ID));
                    if (objcarrierName != null)
                        dr["Carrier Name"] = objcarrierName.Carrier_Name;
                    //Added by srividhya on 6-Aug-2014
                    dr["CarrierID"] = objInsurancePlan.Carrier_ID.ToString();
                    dr["PlanType"] = objInsurancePlan.Financial_Class_Name;
                }
                dr["Group Number"] = obj.Group_Number;
                dr["Insurance Type"] = obj.Insurance_Type;
                dr["Patient Name"] = txtPatientLastName.Text + " " + txtPatientFirstName.Text;
                dr["Relationship"] = obj.Relationship;
                dr["Active"] = obj.Active;
                if (obj.Effective_Start_Date != DateTime.MinValue)
                    dr["Effective Start Date"] = obj.Effective_Start_Date.ToString("dd-MMM-yyyy");
                if (obj.Termination_Date != DateTime.MinValue)
                    dr["Termination Date"] = obj.Termination_Date.ToString("dd-MMM-yyyy");
                if (InsuredHumanList != null)
                    dr["Insured Name"] = InsuredHumanList.Last_Name + " " + InsuredHumanList.First_Name;
                dr["Insured Human ID"] = obj.Insured_Human_ID;
                dr["Insured DOB"] = objHumanList.Birth_Date.ToString("dd-MMM-yyyy");
                dr["Insured Sex"] = objHumanList.Sex;
                dr["Id"] = obj.Id;
                dtSettype.Rows.Add(dr);
                // ((DataTable)Session["dt"]).Rows.Add(dr);
            }
            
            grdPolicyInformation.DataSource = dtSettype;
            grdPolicyInformation.DataBind();
        }

        protected void grdPolicyInformation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void txtPatientSex_TextChanged(object sender, EventArgs e)
        {

        }
       protected void  grdPolicyInformation_SelectedIndexChanging(object sender, GridViewCommandEventArgs e)
        {
            foreach (GridViewRow row in grdPolicyInformation.Rows)
            {
                if (row.RowIndex == grdPolicyInformation.SelectedIndex)
                {
                    row.CssClass = "highlight"; ;
                }
                else
                {
                    row.Style.Add("background-color", "#FFFFFF");
                }
            }
        }
        protected void grdPolicyInformation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView _gridView = (GridView)sender;
            // Get the selected index and the command name
            int _selectedIndex = int.Parse(e.CommandArgument.ToString());
            string _commandName = e.CommandName;

            switch (_commandName)
            {
                case ("SingleClick"):
                    _gridView.SelectedIndex = _selectedIndex;
                    hdnSelectedIndex.Value = _selectedIndex.ToString();
                    btnMakeInactive.Enabled = true;
                    //code comment by bbalaji.TJ 2015-12-17
                    //if (grdPolicyInformation.Rows[_selectedIndex].Cells[4].Text.ToString().ToUpper() == "NO")
                    //{
                    //    //btnMakeInactive.Enabled = false;
                    //    // btnMakeInactive.Text ="Make Active";
                    //    //btnMakeActive.Enabled = true;
                    //}
                    //else if (grdPolicyInformation.Rows[_selectedIndex].Cells[4].Text.ToString().ToUpper() == "YES")
                    //{
                    //    //btnMakeInactive.Enabled = true;
                    //    // btnMakeActive.Enabled = false;
                    //    //btnMakeInactive.Text = "Make Inactive";
                    //}
                    hdnSelectedIndex.Value = _selectedIndex.ToString();
                    hdnInsuredHumanID.Value = grdPolicyInformation.Rows[_selectedIndex].Cells[11].Text;
                    if (hdnCurrentProcess.Value.Contains("QC"))
                    {
                        btnAddInsuranceForSelf.Enabled = false;
                        btnAddInsurancePolicy.Enabled = false;
                        // btnMakeActive.Enabled = false;
                        btnMakeInactive.Enabled = false;
                        btnSetPriorityAsPrimary.Enabled = false;
                        btnSetPriorityAsSecondary.Enabled = false;
                        btnSetPriorityAsTertiary.Enabled = false;
                        btnUpdateInsuredDemographics.Enabled = false;

                    }
                    break;
                //case ("DoubleClick"):
                //    _gridView.SelectedIndex = _selectedIndex;
                //    hdnSelectedIndex.Value = _selectedIndex.ToString();
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "CloseFindPatient();", true);
                //    break;
            }
        }
        public void CreateEmptyHeader()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Policy Holder ID", typeof(string));
            dt.Columns.Add("Plan ID", typeof(string));
            dt.Columns.Add("Plan Name", typeof(string));
            dt.Columns.Add("Group Number", typeof(string));
            dt.Columns.Add("Insurance Type", typeof(string));
            dt.Columns.Add("Patient Name", typeof(string));
            dt.Columns.Add("Relationship", typeof(string));
            dt.Columns.Add("Active", typeof(string));
            dt.Columns.Add("Effective Start Date", typeof(string));
            dt.Columns.Add("Termination Date", typeof(string));
            dt.Columns.Add("Insured Name", typeof(string));
            dt.Columns.Add("Insured Human ID", typeof(string));
            dt.Columns.Add("Insured DOB", typeof(string));
            dt.Columns.Add("Insured Sex", typeof(string));
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Carrier Name", typeof(string));
            dt.Columns.Add("CarrierID", typeof(string));
            dt.Columns.Add("PlanType", typeof(string));
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            grdPolicyInformation.DataSource = dt;
            grdPolicyInformation.DataBind();
            grdPolicyInformation.Rows[0].Visible = false;
        }

        protected void btnAddInsSelfRefresh_Click(object sender, EventArgs e)
        {
            if (chkShowActiveOnly.Checked == false)
                chkShowActiveOnly.Checked = true;
            loadGrid();
        }

        protected void btnUpdateInformationRefresh_Click(object sender, EventArgs e)
        {
            if (grdPolicyInformation.SelectedRow == null)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("420021", "PatientInsurancePolicyMaintenance", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Patient Insurance Policy Maintanence", "DisplayErrorMessage('420021');", true);
                grdPolicyInformation.Focus();
            }
            else
            {
                //code comment by balaji.TJ 2015-12-17
                //ulong insplan = 0, inshumanid = 0;
                if (grdPolicyInformation.SelectedRow.Cells[14].Text != string.Empty)
                    //insplan = Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[13].Text);               
                    if (grdPolicyInformation.SelectedRow.Cells[13].Text != string.Empty)
                        // inshumanid = Convert.ToUInt64(grdPolicyInformation.SelectedRow.Cells[12].Text);             
                        //frmAddInsurancePolicies addInsPolicy = new frmAddInsurancePolicies(insplan, inshumanid, ulMyHumanID);
                        //addInsPolicy.ShowDialog();
                        //btnViewUpdatePolicyInformation.Attributes.Add("onClick","openwindow('"++'")"
                        //string link = "frmAddInsurancePolicies.aspx?HumanId=" + Session["ulMyHumanID"] + "&InsPlan=" + insplan + "&InsHumanId=" + inshumanid;
                        //MessageBox.Open(link);
                        //Server.Transfer("frmAddInsurancePolicies.aspx?HumanId=" + Session["ulMyHumanID"] + "&InsPlan=" + insplan + "&InsHumanId=" + inshumanid);
                        loadGrid();
            }
        }
    }
}
