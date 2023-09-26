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
using System.IO;
using System.Net;
using Telerik.Web.UI;
/*added*/
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using Acurus.Capella.Core.DTO;
using System.Threading;
using MySql.Data.MySqlClient;

namespace Acurus.Capella.UI
{
    public partial class frmEligibilityVerification : System.Web.UI.Page
    {
        // Eligibility_VerificationProxy EligibilityProxy = new Eligibility_VerificationProxy();
        ulong myHumanID, ulMyLookupPhyID = 0;
        string myPatientName;
        string myGroupNumber;
        string myPolicyHolderId;
        string myInsPlanId;
        string myCarrierName;
        string myInsPlanName;
        string myClaimCity;
        string myState;
        string myZipCode;
        string myClaimAddress;
        //string myPolicyHolderID;
        bool saveCheckingFlag = false, bSaveCompleted = true;
        DateTime dtMyEffectiveDate;
        DateTime dtMyTerminationDate;
        IList<Eligibility_Verification> loadEligibilityList = null;
        Eligibility_VerficationManager EligibilityMngr = new Eligibility_VerficationManager();
        StaticLookupManager LookUpMngr = new StaticLookupManager();
        StateManager StateMngr = new StateManager();
        InsurancePlanManager InsMngr = new InsurancePlanManager();
        FillQuickPatient objCheckOut;
        IList<InsurancePlan> inslist;
        HumanManager HumanMngr = new HumanManager();/*added*/
        IList<FileManagementIndex> fileManagementIndexList = new List<FileManagementIndex>();
        IList<FileManagementIndex> fileExistList = new List<FileManagementIndex>();
        FileManagementIndexManager FleManagementMngr = new FileManagementIndexManager();
        // InsurancePlanManager insMngr = new InsurancePlanManager();
        //FileManagementIndexManager objFilemngtIndexMngr = new FileManagementIndexManager();
        int prevNum = 0;
        string lastNumToAdd = string.Empty;
        string claimaddreplace = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            //dtpEffectiveStartDate.ClientEvents.OnBlur = "CallMe";
            //dtpTerminationDate.ClientEvents.OnBlur = "CallMe";
            this.Page.Title = "Eligibility Verification" + "-" + ClientSession.UserName;
            if (!IsPostBack)
            {
                // ClientSession.FlushSession();
                //srividhya added the below code.
                loadEVTypes();
                //  Eligibility_VerificationProxy.EligibilityVerificationDetails = null;



                if (Request["humanID"] != null && Request["patientName"] != null && Request["groupNumber"] != null && Request["policyHolderId"] != null && Request["insPlanId"] != null && Request["carrierName"] != null && Request["dtEffectiveDate"] != null && Request["dtTerminationDate"] != null && Request["insplanname"] != null && Request["ClaimCity"] != null && Request["ClaimState"] != null && Request["zipcode"] != null && Request["ClaimAddress"] != null)
                {
                    myCarrierName = Request["carrierName"];
                    //srividhya
                    txtCarrierName.Text = myCarrierName;/*command*/
                    txtCarrierName.Attributes.Add("title", myCarrierName);
                    hdnmyCarrierName.Value = myCarrierName;

                    //added
                    myInsPlanName = Request["insplanname"];
                    txtInsPlanName.Text = myInsPlanName;
                    txtInsPlanName.Attributes.Add("title", myInsPlanName);
                    hdnmyInsPlanName.Value = myInsPlanName;

                    myClaimCity = Request["ClaimCity"];
                    txtClaimCity.Text = myClaimCity;
                    hdnmyClaimCity.Value = myClaimCity;

                    myState = Request["ClaimState"];
                    ddlState.Text = myState;
                    hdnmymyState.Value = myState;

                    myZipCode = Request["zipcode"];
                    txtZipCode.Text = myZipCode;
                    hdnmyZipCode.Value = myZipCode;

                    myPolicyHolderId = Request["policyHolderId"];
                    txtPolicyHolderId.Text = myPolicyHolderId;
                    txtPolicyHolderId.Attributes.Add("title", myPolicyHolderId);
                    hdnmyPolicyHolderId.Value = myPolicyHolderId;

                    myClaimAddress = Request["ClaimAddress"].Replace("!@", "#");
                    txtClaimAddress.Text = myClaimAddress;
                    hdnmyClaimAddress.Value = myClaimAddress;


                    if (Request["humanID"] != null && Request["humanID"]!="")
                    myHumanID = Convert.ToUInt64(Request["humanID"]);
                    //srividhya
                    txtPatientAccountNo.Text = myHumanID.ToString();
                    hdnmyHumanID.Value = myHumanID.ToString();
                    myPatientName = Request["patientName"];
                    //srividhya
                    txtPatientName.Text = myPatientName;
                    hdnmyPatientName.Value = myPatientName;
                    myGroupNumber = Request["groupNumber"];
                    //srividhya
                    //txtGroupNumber.Text = myGroupNumber.ToString();
                    //hdnmyGroupNumber.Value = myGroupNumber;
                    //myPolicyHolderId = Request["policyHolderId"];
                    //srividhya
                    //txtPolicyHolderID.Text = myPolicyHolderId.ToString();/*command*/
                    hdnmyPolicyHolderId.Value = myPolicyHolderId;
                    myInsPlanId = Request["insPlanId"];
                    txtGroupNumber.Text = Request["groupNumber"];
                    //srividhya
                    //txtInsPlanID.Text = myInsPlanId;/*command*/
                    hdnInsPlanId.Value = myInsPlanId;
                    saveCheckingFlag = false;
                    dtpEffectiveStartDate.Focus();

                    LoadComboBox();
                    if (Request["dtEffectiveDate"] != string.Empty)
                    {
                        dtMyEffectiveDate = Convert.ToDateTime(Request["dtEffectiveDate"]);
                        hdndtMyEffectiveDate.Value = dtMyEffectiveDate.ToString();
                        //srividhya
                        if (dtMyEffectiveDate != null && dtMyEffectiveDate.ToString("yyyy-MM-dd") != "0001-01-01")
                        {
                            //dtpEffectiveStartDate.SelectedDate = dtMyEffectiveDate;//.ToString("dd-MMM-yyyy");
                            dtpEffectiveStartDate.Text = dtMyEffectiveDate.ToString("dd-MMM-yyyy");//.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            if (Request["UTCTime"] != string.Empty)
                            {
                                //dtpEffectiveStartDate.SelectedDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(Request["UTCTime"]));//.ToString("dd-MMM-yyyy");// DateTime.Now.ToString("dd-MMM-yyyy");
                                dtpEffectiveStartDate.Text = UtilityManager.ConvertToLocal(Convert.ToDateTime(Request["UTCTime"])).ToString("dd-MMM-yyyy");//.ToString("dd-MMM-yyyy");// DateTime.Now.ToString("dd-MMM-yyyy");
                            }
                        }
                    }
                    if (Request["dtTerminationDate"] != string.Empty)
                    {
                        dtMyTerminationDate = Convert.ToDateTime(Request["dtTerminationDate"]);
                        hdndtMyTerminationDate.Value = dtMyTerminationDate.ToString();
                        //srividhya
                        if (dtMyEffectiveDate != null)
                        {
                            //dtpEffectiveStartDate.SelectedDate = dtMyEffectiveDate;//.ToString("dd-MMM-yyyy");
                            dtpEffectiveStartDate.Text = dtMyEffectiveDate.ToString("dd-MMM-yyyy");//.ToString("dd-MMM-yyyy");
                        }
                    }
                    //srividhya added the below code on 06-feb-2014
                    if (Request["PCPCopay"] != null)
                    {
                        txtPCPCopay.Text = Request["PCPCopay"].ToString();
                    }
                    if (Request["SPCCopay"] != null)
                    {
                        txtSPCCopay.Text = Request["SPCCopay"].ToString();
                    }
                    if (Request["Deductible"] != null)
                    {
                        txtDeductibleforThePlan.Text = Request["Deductible"].ToString();
                    }
                    if (Request["CoInsurance"] != null)
                    {
                        txtCoInsurance.Text = Request["CoInsurance"].ToString();
                    }
                    if (Request["EVMode"] != null)
                    {
                        for (int i = 0; i < ddlEVType.Items.Count; i++)
                        {
                            if (ddlEVType.Items[i].Text == Request["EVMode"].ToString())
                            {
                                ddlEVType.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    //if (Request["EVStatus"] != null)/*command*/
                    //{
                    //    if (ddlEligibilityStatus.Items.Count > 0)
                    //        for (int i = 0; i < ddlEligibilityStatus.Items.Count; i++)
                    //        {
                    //            if (ddlEligibilityStatus.Items[i].Text == Request["EVStatus"].ToString())
                    //            {
                    //                ddlEligibilityStatus.SelectedIndex = i;
                    //                break;
                    //            }
                    //        }
                    //}
                    if (Request["CallRepName"] != null)
                    {
                        txtCallRepresentative.Text = Request["CallRepName"].ToString();
                        txtCallRepresentative.Attributes.Add("title", Request["CallRepName"].ToString());
                    }
                    if (Request["CallRefNo"] != null)
                    {
                        txtCallReference.Text = Request["CallRefNo"].ToString();
                        txtCallReference.Attributes.Add("title", Request["CallRefNo"].ToString());
                    }
                    if (Request["Comments"] != null)
                    {
                        txtComments.Text = Request["Comments"].ToString();
                    }
                }
                if (Request["PatientDOB"] != null)
                {
                    hdnDOB.Value = Request["PatientDOB"].ToString();
                }
                if (Request["PatientType"] != null)
                {
                    txtPatientType.Text = Request["PatientType"];
                }
                loadEligibilityList = EligibilityMngr.GetEligDetailsUsingHumanandPolicyHolderID(myHumanID, myPolicyHolderId);
                if (txtPatientAccountNo.Text != string.Empty)
                {
                    IList<FileManagementIndex> ilistFile = FleManagementMngr.GetListbySourceAndHumanId(Convert.ToUInt64(txtPatientAccountNo.Text), "ELIGIBILITY VERIFICATION");
                    FindFileIndex(ilistFile);
                }

                //srividhya commented the below code on 06-Feb-2014                
                //dtpEffectiveStartDate.Text = string.Empty;                
                //dtpTerminationDate.Text = string.Empty;
                //txtPatientName.Text = myPatientName;
                //txtPolicyHolderID.Text = myPolicyHolderId.ToString();
                //txtInsPlanID.Text = myInsPlanId;
                //txtPatientAccountNo.Text = myHumanID.ToString();
                //txtGroupNumber.Text = myGroupNumber.ToString();
                txtCarrierName.Text = myCarrierName;
                if (txtCarrierName.Text != null)
                {
                    txtCarrierName.Attributes.Add("title", myCarrierName);
                }


                if (myPolicyHolderId != null)
                {
                    txtPolicyHolderId.Text = myPolicyHolderId.ToString();
                }
                txtPolicyHolderId.Attributes.Add("title", myPolicyHolderId);
                txtInsPlanName.Text = myInsPlanName;
                txtInsPlanName.Attributes.Add("title", myInsPlanName);
                txtClaimCity.Text = myClaimCity;
                ddlState.Text = myState;
                txtZipCode.Text = myZipCode;
                txtClaimAddress.Text = myClaimAddress;
                txtGroupNumber.Text = Request["groupNumber"];

                //dtpTerminationDate.Value = DateTime.MinValue;
                //code added by balaji.TJ 2015-12-18
                IList<InsurancePlan> InsPlanList = new List<InsurancePlan>();
                //if (txtInsPlanID.Text != "")/*command*/
                //    InsPlanList = InsMngr.GetInsurancebyID(Convert.ToUInt64(txtInsPlanID.Text));
                if (InsPlanList != null && InsPlanList.Count > 0) //code added by balaji.TJ 2015-12-18
                {
                    txtClaimAddress.Text = InsPlanList[0].Claim_Address;
                    txtClaimCity.Text = InsPlanList[0].Claim_City;
                    txtZipCode.Text = InsPlanList[0].Claim_ZipCode;
                    for (int i = 0; i < ddlState.Items.Count; i++)
                    {
                        if (ddlState.Items[i].Text == InsPlanList[0].Claim_State)
                        {
                            ddlState.SelectedIndex = i;
                            break;
                        }
                    }
                }
                //srividhya commented the below code, instead put it as top.
                //loadEVTypes();
                //srividhya added the below code 
                //   ddlEVType_SelectedIndexChanged(sender, e); bugid= 625625
                if (loadEligibilityList != null && loadEligibilityList.Count > 0)
                {
                    txtSPCCopay.Text = loadEligibilityList[0].SPC_Copay.ToString();
                    //dtpEffectiveStartDate.SelectedDate = loadEligibilityList[0].Effective_Date;//.ToString("dd-MMM-yyyy");
                    dtpEffectiveStartDate.Text = loadEligibilityList[0].Effective_Date.ToString("dd-MMM-yyyy");//.ToString("dd-MMM-yyyy");
                    for (int i = 0; i < ddlEVType.Items.Count; i++)
                    {
                        if (ddlEVType.Items[i].Text == loadEligibilityList[0].Eligibility_Type)
                        {
                            ddlEVType.SelectedIndex = i;
                            break;
                        }
                    }
                    // ddlEVType.Text = loadEligibilityList[0].Eligibility_Type;
                    txtPCPCopay.Text = loadEligibilityList[0].PCP_Copay.ToString();
                    txtDeductibleforThePlan.Text = loadEligibilityList[0].Deductible_For_Plan.ToString();
                    txtDeductibleMet.Text = loadEligibilityList[0].Deductible_Met_So_Far.ToString();
                    txtCoInsurance.Text = loadEligibilityList[0].Coinsurance.ToString();
                    txtCallRepresentative.Text = loadEligibilityList[0].Call_Rep_Name;
                    txtCallRepresentative.Attributes.Add("title", loadEligibilityList[0].Call_Rep_Name);
                    txtCallReference.Text = loadEligibilityList[0].Call_Ref_Number;

                    txtCallReference.Attributes.Add("title", loadEligibilityList[0].Call_Ref_Number);
                    if (loadEligibilityList[0].Termination_Date != Convert.ToDateTime("1/1/0001"))
                    {
                        //dtpTerminationDate.SelectedDate = loadEligibilityList[0].Termination_Date;//.ToString("dd-MMM-yyyy");
                        dtpTerminationDate.Text = loadEligibilityList[0].Termination_Date.ToString("dd-MMM-yyyy");//.ToString("dd-MMM-yyyy");
                    }
                    //srividhya commented the code
                    //ddlEVType_SelectedIndexChanged(sender, e);

                    //srividhya added the code
                    txtComments.Text = loadEligibilityList[0].Comments; //code added by balaji.TJ 2015-12-18
                    //if (ddlEligibilityStatus.Items.Count > 0)/*command*/
                    //    for (int i = 0; i < ddlEligibilityStatus.Items.Count; i++)
                    //    {
                    //        if (ddlEligibilityStatus.Items[i].Text.ToUpper() == loadEligibilityList[0].Eligibility_Status.ToUpper())
                    //        {
                    //            ddlEligibilityStatus.SelectedIndex = i;
                    //            break;
                    //        }
                    //    }
                }
                else //No need
                {
                    //Modified by Priyangha Bugid:20665
                    //dtpEffectiveStartDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    //*****//
                    //srividhya
                    if (dtMyEffectiveDate != null && dtMyEffectiveDate.ToString("yyyy-MM-dd") != "0001-01-01")
                    {
                    }
                    else
                    {
                        if (Request["UTCTime"] != string.Empty)
                        {
                            //code committed by balaji.TJ
                            //dtpEffectiveStartDate.SelectedDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(Request["UTCTime"]));//.ToString("dd-MMM-yyyy");// DateTime.Now.ToString("dd-MMM-yyyy");
                        }
                    }
                }
                //dtpEffectiveStartDate.Focus();
                //dtpEffectiveStartDate.Format = DateTimePickerFormat.Custom;
                //dtpEffectiveStartDate.CustomFormat = " ";
                //srividhya
                if (Request.QueryString["EncounterId"] != null)
                {
                    hdnEncounterID.Value = Request.QueryString["EncounterId"].ToString();
                }
                else
                {
                    hdnEncounterID.Value = "0";
                }

                saveCheckingFlag = false;

                btnAdd.Enabled = false;
                btnAdd.Attributes.Add("Onclick", "javascript:return ValidateSAve();");
            }
            FillPatientStrip(Convert.ToUInt64(Request["humanID"]));
        }

        /*added stripe*/

        public void FillPatientStrip(ulong humanID)
        {

            string sPatientstrip = UtilityManager.FillPatientStrip(humanID);
            if (sPatientstrip != null)
            {
                divPatientstrip.InnerText = sPatientstrip;
            }

            // Assign PatientStrip Values
            //string FileName = "Human" + "_" + humanID + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlNodeList xmlhumanList = itemDoc.GetElementsByTagName("Human");
            //        Human objFillHuman = new Human();
            //        IList<Human> lstHuman = new List<Human>();
            //        if (xmlhumanList != null && xmlhumanList.Count > 0)
            //        {
            //            objFillHuman.Id = Convert.ToUInt64(xmlhumanList[0].Attributes.GetNamedItem("Id").Value);
            //            objFillHuman.Birth_Date = Convert.ToDateTime(xmlhumanList[0].Attributes.GetNamedItem("Birth_Date").Value);
            //            objFillHuman.First_Name = xmlhumanList[0].Attributes.GetNamedItem("First_Name").Value;
            //            objFillHuman.Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Last_Name").Value;
            //            objFillHuman.MI = xmlhumanList[0].Attributes.GetNamedItem("MI").Value;
            //            objFillHuman.Sex = xmlhumanList[0].Attributes.GetNamedItem("Sex").Value;
            //            objFillHuman.Suffix = xmlhumanList[0].Attributes.GetNamedItem("Suffix").Value;
            //            objFillHuman.Medical_Record_Number = xmlhumanList[0].Attributes.GetNamedItem("Medical_Record_Number").Value;
            //            objFillHuman.Home_Phone_No = xmlhumanList[0].Attributes.GetNamedItem("Home_Phone_No").Value;
            //            objFillHuman.Human_Type = xmlhumanList[0].Attributes.GetNamedItem("Human_Type").Value;
            //            objFillHuman.Patient_Account_External = xmlhumanList[0].Attributes.GetNamedItem("Patient_Account_External").Value;
            //            objFillHuman.Cell_Phone_Number = xmlhumanList[0].Attributes.GetNamedItem("Cell_Phone_Number").Value;
            //            lstHuman.Add(objFillHuman);
            //        }


            //        string phoneno = "";

            //        if (lstHuman != null && lstHuman.Count > 0)
            //        {

            //            if (objFillHuman.Home_Phone_No.Length == 14)
            //            {
            //                phoneno = objFillHuman.Home_Phone_No;
            //            }
            //            else
            //            {
            //                phoneno = objFillHuman.Cell_Phone_Number;
            //            }

            //        }

            //        string sPatientSex = string.Empty;
            //        if (objFillHuman.Sex != string.Empty)
            //        {
            //            if (objFillHuman.Sex.Substring(0, 1).ToUpper() == "U")
            //            {
            //                sPatientSex = "UNK";
            //            }
            //            else
            //            {
            //                sPatientSex = objFillHuman.Sex.Substring(0, 1);
            //            }
            //        }
            //        else
            //        {
            //            sPatientSex = "";
            //        }

            //        if (lstHuman != null && lstHuman.Count > 0)
            //        {
            //            divPatientstrip.InnerText = objFillHuman.Last_Name + "," + objFillHuman.First_Name +
            //       "  " + objFillHuman.MI + "  " + objFillHuman.Suffix + "   |   " +
            //        objFillHuman.Birth_Date.ToString("dd-MMM-yyyy") + "   |   " +
            //       (CalculateAge(objFillHuman.Birth_Date)).ToString() +
            //       "  year(s)    |   " + sPatientSex + "   |   Acc #:" + humanID +
            //       "   |   " + "Med Rec #:" + objFillHuman.Medical_Record_Number + "   |   " +
            //       "Phone #:" + phoneno + "   |   Patient Type:" + objFillHuman.Human_Type;
            //        }
            //        else
            //        {
            //            divPatientstrip.InnerText = " " + "   |" + "|" + "|" + "|" + "|";
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}
            //else
            //{
            //    divPatientstrip.InnerText = " " + "   |" + "|" + "|" + "|" + "|";
            //}
        }


        public int CalculateAge(DateTime birthDate)
        {
            // cache the current time
            DateTime now = DateTime.Today; // today is fine, don't need the timestamp from now
            // get the difference in years
            int years = 0;
            if (birthDate != null)
                years = now.Year - birthDate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;

            return years;
        }

        //method not in use
        //public string FillPatientSummaryBarforPatientChart(string LastName, string FirstName, string MI, string Suffix, DateTime DOB, ulong ulhumanID, string MedRecNo, string HomePhoneNo, string Sex, string PatientStatus, string SSN, string PatientType, string sPriPlan, string sPriCarrier, string sSecPlan, string sSecCarrier)
        //{
        //    string sMySummary;
        //    string sPatientSex = string.Empty;
        //    if (Sex != string.Empty) //For Bug Id 48937 
        //    {
        //        if (Sex.Substring(0, 1).ToUpper() == "U")
        //        {
        //            sPatientSex = "UNK";
        //        }
        //        else
        //        {
        //            sPatientSex = Sex.Substring(0, 1);
        //        }

        //    }
        //    else
        //    {
        //        sPatientSex = " ";
        //    }
        //    if (PatientStatus == "DECEASED")
        //    {
        //        sMySummary = LastName + "," + FirstName +
        //           "  " + MI + "  " + Suffix + "   |   " +
        //           DOB.ToString("dd-MMM-yyyy") + "   |   " +
        //           (CalculateAge(DOB)).ToString() +
        //           "  year(s)    |   " + sPatientSex + "   |   " + PatientStatus + "   |   Acct #:" + ulhumanID +
        //           "   |   " + "Med Rec #:" + MedRecNo + "   |   " +
        //           "Phone #:" + HomePhoneNo + "   |   ";
        //    }
        //    else
        //    {
        //        sMySummary = LastName + "," + FirstName +
        //       "  " + MI + "  " + Suffix + "   |   " +
        //       DOB.ToString("dd-MMM-yyyy") + "   |   " +
        //       (CalculateAge(DOB)).ToString() +
        //       "  year(s)    |   " + sPatientSex + "   |   Acct #:" + ulhumanID +
        //       "   |   " + "Med Rec #:" + MedRecNo + "   |   " +
        //       "Phone #:" + HomePhoneNo + "   |   ";

        //    }

        //    ClientSession.SummaryList = "Name: " + LastName + " " + FirstName + " " + MI + " " + Suffix + " | DOB:  " + DOB.ToString("dd-MMM-yyyy") + " | Acc #:" + ulhumanID;
        //    if (PatientType != string.Empty)
        //    {
        //        sMySummary += "Patient Type:" + PatientType + "   |   ";
        //    }
        //    if (sPriPlan != string.Empty)
        //    {
        //        sMySummary += "$" + "Pri Plan:" + sPriCarrier + " - " + sPriPlan + "   |   ";
        //    }
        //    if (sSecPlan != string.Empty)
        //    {
        //        sMySummary += "$" + "Sec Plan:" + sSecCarrier + " - " + sSecPlan + "   |   ";
        //    }
        //    if (SSN != string.Empty)
        //    {
        //        sMySummary += "$" + "SSN:" + SSN + "   |   ";
        //    }

        //    string HumanFile = string.Empty;
        //    if (ClientSession.HumanId != null)
        //        HumanFile = "Human" + "_" + ClientSession.HumanId + ".xml";
        //    string sHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFile);

        //    if (File.Exists(sHumanFilePath) == true)
        //    {
        //        XmlDocument itemDoc = new XmlDocument();
        //        XmlTextReader XmlText = new XmlTextReader(sHumanFilePath);
        //        itemDoc.Load(XmlText);
        //        XmlText.Close();
        //        XmlNodeList xmlAgenode = itemDoc.GetElementsByTagName("Age");
        //        if (xmlAgenode != null && xmlAgenode.Count > 0)
        //            xmlAgenode[0].Attributes[0].Value = CalculateAge(DOB).ToString();
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
        //        }
        //        //itemDoc.Save(sHumanFilePath);

        //        int trycount = 0;
        //    trytosaveagain:
        //        try
        //        {
        //            itemDoc.Save(sHumanFilePath);
        //        }
        //        catch (Exception xmlexcep)
        //        {
        //            trycount++;
        //            if (trycount <= 3)
        //            {
        //                int TimeMilliseconds = 0;
        //                if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
        //                    TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

        //                Thread.Sleep(TimeMilliseconds);
        //                string sMsg = string.Empty;
        //                string sExStackTrace = string.Empty;

        //                string version = "";
        //                if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
        //                    version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

        //                string[] server = version.Split('|');
        //                string serverno = "";
        //                if (server.Length > 1)
        //                    serverno = server[1].Trim();

        //                if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
        //                    sMsg = xmlexcep.InnerException.Message;
        //                else
        //                    sMsg = xmlexcep.Message;

        //                if (xmlexcep != null && xmlexcep.StackTrace != null)
        //                    sExStackTrace = xmlexcep.StackTrace;

        //                string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
        //                string ConnectionData;
        //                ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        //                using (MySqlConnection con = new MySqlConnection(ConnectionData))
        //                {
        //                    using (MySqlCommand cmd = new MySqlCommand(insertQuery))
        //                    {
        //                        cmd.Connection = con;
        //                        try
        //                        {
        //                            con.Open();
        //                            cmd.ExecuteNonQuery();
        //                            con.Close();
        //                        }
        //                        catch
        //                        {
        //                        }
        //                    }
        //                }
        //                goto trytosaveagain;
        //            }
        //        }
        //    }

        //    ClientSession.PatientPane = sMySummary;
        //    return sMySummary;
        //}
        //End






        private void loadEVTypes()
        {
            IList<StaticLookup> StaticLookuplist = LookUpMngr.getStaticLookupByFieldName("VERIFICATIONTYPE");
            ddlEVType.Items.Clear();
            if (StaticLookuplist != null)
            {
                foreach (StaticLookup j in StaticLookuplist)
                {
                    ddlEVType.Items.Add(j.Value);
                }
            }
            ddlEVType.SelectedIndex = 0;

            IList<StaticLookup> EligibilityStatus = LookUpMngr.getStaticLookupByFieldName("ELIGIBILITY STATUS");
            //ddlEligibilityStatus.Items.Clear();
            //ddlEligibilityStatus.Items.Add("");
            //if (EligibilityStatus != null)
            //{
            //    foreach (StaticLookup j in EligibilityStatus)
            //    {
            //        ddlEligibilityStatus.Items.Add(j.Value);
            //    }
            //    ddlEligibilityStatus.SelectedIndex = 1;
            //}

            ddlState.Items.Clear();
            IList<State> StateList = StateMngr.Getstate();
            ddlState.Items.Add("");
            if (StateList != null)
            {
                foreach (State i in StateList)
                {
                    ddlState.Items.Add(i.State_Code);
                }
                ddlState.SelectedIndex = 0;
            }
        }
        private bool Validation()
        {
            //if (dtpEffectiveStartDate.SelectedDate == null)// "__-___-____")
            if (dtpEffectiveStartDate.Text == string.Empty)// "__-___-____")
            {
                // ApplicationObject.erroHandler.DisplayErrorMessage("350007", "Eligibility Verification", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Eligibility Verification", "DisplayErrorMessage('350007');", true);
                dtpEffectiveStartDate.Focus();
                return false;
            }
            //Commented By Suresh on !8-June-2011
            //if (Convert.ToBoolean( dtpTerminationDate.CustomFormat ==" ") && dtpEffectiveStartDate.Value > dtpTerminationDate.Value)
            //{
            //    ApplicationObject.erroHandler.DisplayErrorMessage("350008", this.Text);
            //    dtpTerminationDate.Focus();
            //    return false;
            //}
            //Commented By Suresh on 18-June-2011

            //Added by Suresh on  18-June-2011
            //if ((dtpTerminationDate.SelectedDate != null) && dtpEffectiveStartDate.SelectedDate != null)// "__-___-____")
            if ((dtpTerminationDate.Text != string.Empty) && dtpEffectiveStartDate.Text != string.Empty)// "__-___-____")
            {
                //if (Convert.ToDateTime(dtpEffectiveStartDate.SelectedDate.Value) > Convert.ToDateTime(dtpTerminationDate.SelectedDate.Value))
                if (Convert.ToDateTime(dtpEffectiveStartDate.Text) > Convert.ToDateTime(dtpTerminationDate.Text))
                {
                    //ApplicationObject.erroHandler.DisplayErrorMessage("350008", "Eligibility Verification", this.Page);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Eligibility Verification", "DisplayErrorMessage('350008');", true);
                    dtpTerminationDate.Focus();
                    return false;
                }
            }
            //Added by Suresh on  18-June-2011
            if (txtCoInsurance.Text.ToString() != string.Empty)
            {
                if ((Convert.ToDecimal(txtCoInsurance.Text) < 0) || (Convert.ToDecimal(txtCoInsurance.Text) > 100))
                {
                    //ApplicationObject.erroHandler.DisplayErrorMessage("350002", "Eligibility Verification", this.Page);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Eligibility Verification", "DisplayErrorMessage('350002');", true);
                    txtCoInsurance.Focus();
                    return false;
                }
            }
            return true;
        }
        public Boolean AmountValidation(string Amount)
        {
            double dvalue = Convert.ToDouble(Amount);
            if (dvalue > 9999999.99)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("350001", "Eligibility Verification", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Eligibility Verification", "DisplayErrorMessage('350001');", true);
                return false;
            }
            return true;
        }

        //protected void ddlEVType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlEVType.SelectedItem.Text.ToUpper() == "VOICE")
        //    {
        //        TextBoxColorChange(txtCallReference, true);
        //        TextBoxColorChange(txtCallRepresentative, true);
        //        //TextBoxColorChange(txtHyperlinkToWeb, false);
        //    }
        //    else if (ddlEVType.SelectedItem.Text.ToUpper() == "WEB")
        //    {
        //        //TextBoxColorChange(txtHyperlinkToWeb, true);
        //        TextBoxColorChange(txtCallReference, false);
        //        TextBoxColorChange(txtCallRepresentative, false);
        //    }
        //    else
        //    {
        //        TextBoxColorChange(txtCallReference, false);
        //        TextBoxColorChange(txtCallRepresentative, false);
        //        //TextBoxColorChange(txtHyperlinkToWeb, false);
        //    }
        //}
        public void TextBoxColorChange(TextBox txtbox, Boolean bToNormal)
        {
            if (bToNormal == false)
            {
                txtbox.ReadOnly = true;
                txtbox.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                txtbox.Text = string.Empty;
                //((RadTextBoxItem)(txtbox.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //((Telerik.WinControls.Primitives.FillPrimitive)(txtbox.GetChildAt(0).GetChildAt(1))).BackColor = System.Drawing.Color.FromArgb(191, 219, 255);
                //((Telerik.WinControls.Primitives.BorderPrimitive)(txtbox.GetChildAt(0).GetChildAt(2))).ForeColor = System.Drawing.Color.Black;
                //((Telerik.WinControls.Primitives.BorderPrimitive)(txtbox.GetChildAt(0).GetChildAt(2))).BackColor = System.Drawing.Color.Black;
            }
            else
            {
                txtbox.ReadOnly = false;
                txtbox.BackColor = System.Drawing.Color.White;
                //((RadTextBoxElement)(txtbox.GetChildAt(0))).BackColor = System.Drawing.Color.White;
                //((RadTextBoxItem)(txtbox.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.White;
                //((Telerik.WinControls.Primitives.FillPrimitive)(txtbox.GetChildAt(0).GetChildAt(1))).BackColor = System.Drawing.Color.White;
                //((Telerik.WinControls.Primitives.BorderPrimitive)(txtbox.GetChildAt(0).GetChildAt(2))).ForeColor = System.Drawing.Color.FromArgb(173, 195, 222);
                //((Telerik.WinControls.Primitives.BorderPrimitive)(txtbox.GetChildAt(0).GetChildAt(2))).BackColor = System.Drawing.Color.White;
            }
        }

        protected void txtClaimAddress_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }
        protected void txtClaimCity_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }
        protected void txtCallRepresentative_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }

        protected void ddlEVType_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
            //ddlEligibilityStatus.Focus();
        }

        protected void txtPCPCopay_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }

        protected void txtSPCCopay_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }

        protected void txtDeductibleforThePlan_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }

        protected void txtCoInsurance_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }

        protected void txtCallReference_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }

        protected void txtHyperlinkToWeb_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }

        protected void dtpEffectiveStartDate_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }

        protected void txtDeductibleMet_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }

        protected void ddlState_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }

        protected void txtZipCode_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }

        protected void dtpTerminationDate_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }

        protected void txtComments_TextChanged(object sender, EventArgs e)
        {
            saveCheckingFlag = true;
            btnAdd.Enabled = true;
        }



        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    btnAdd.Enabled = false;
        //    if (!Validation())
        //    {
        //        btnAdd.Enabled = true;
        //        bSaveCompleted = false;
        //        saveCheckingFlag = true;
        //        return;
        //    }
        //    if (hdnSaveFlag.Value.ToUpper() == "TRUE")
        //    {
        //        Eligibility_Verification objEligibile = new Eligibility_Verification();
        //        IList EligibilityDetails = new List<string>();
        //        //if (dtpEffectiveStartDate.SelectedDate != null)// "__-___-____")
        //        //{
        //        //    EligibilityDetails.Add("Effective Start Date!" + dtpEffectiveStartDate.SelectedDate.Value);
        //        //    objEligibile.Effective_Date = Convert.ToDateTime(dtpEffectiveStartDate.SelectedDate.Value);
        //        //}

        //        if (dtpEffectiveStartDate.Text != "")// "__-___-____")
        //        {
        //            EligibilityDetails.Add("Effective Start Date!" + dtpEffectiveStartDate.Text);
        //            objEligibile.Effective_Date = Convert.ToDateTime(dtpEffectiveStartDate.Text);
        //        }

        //        //if (dtpTerminationDate.SelectedDate != null)// "__-___-____")
        //        //{
        //        //    EligibilityDetails.Add("Termination Date!" + dtpTerminationDate.SelectedDate.Value);
        //        //    objEligibile.Termination_Date = Convert.ToDateTime(dtpTerminationDate.SelectedDate.Value);
        //        //}

        //        if (dtpTerminationDate.Text != null)// "__-___-____")
        //        {
        //            EligibilityDetails.Add("Termination Date!" + dtpTerminationDate.Text);
        //            objEligibile.Termination_Date = Convert.ToDateTime(dtpTerminationDate.Text);
        //        }

        //        if (txtPCPCopay.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("PCP Copay!" + txtPCPCopay.Text);
        //            objEligibile.PCP_Copay = Convert.ToDouble(txtPCPCopay.Text);
        //        }
        //        if (txtSPCCopay.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("SPC Copay!" + txtSPCCopay.Text);
        //            objEligibile.SPC_Copay = Convert.ToDouble(txtSPCCopay.Text);
        //        }
        //        if (txtDeductibleforThePlan.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("Deductible for the Plan!" + txtDeductibleforThePlan.Text);
        //            objEligibile.Deductible_For_Plan = Convert.ToDouble(txtDeductibleforThePlan.Text);
        //        }
        //        if (txtCoInsurance.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("Co Insurance!" + txtCoInsurance.Text);
        //            objEligibile.Coinsurance = Convert.ToDouble(txtCoInsurance.Text);
        //        }
        //        if (ddlEVType.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("EV Type!" + ddlEVType.Text);
        //            objEligibile.Eligibility_Type = ddlEVType.Text;
        //        }
        //        if (txtCallRepresentative.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("Call Representative Name!" + txtCallRepresentative.Text);
        //            objEligibile.Call_Rep_Name = txtCallRepresentative.Text;
        //        }
        //        if (txtCallReference.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("Call Reference Number!" + txtCallReference.Text);
        //            objEligibile.Call_Ref_Number = txtCallReference.Text;
        //        }
        //        if (txtHyperlinkToWeb.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("Hyperlink to web image!" + txtHyperlinkToWeb.Text);
        //            objEligibile.Hyperlink = txtHyperlinkToWeb.Text;
        //        }
        //        if (txtCarrierName.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("Carrier From EV!" + txtCarrierName.Text);
        //        }
        //        objEligibile.Human_ID = Convert.ToUInt64(hdnmyHumanID.Value.ToString());
        //        if (hdnInsPlanId.Value.ToString() != string.Empty)
        //        {
        //            objEligibile.Insurance_Plan_ID = Convert.ToUInt64(hdnInsPlanId.Value);
        //        }

        //        objEligibile.Policy_Holder_ID = hdnmyPolicyHolderId.Value.ToString();
        //        objEligibile.Group_Number = hdnmyGroupNumber.Value.ToString();
        //        objEligibile.Eligibility_Verified_By = ClientSession.UserName;
        //        if (hdnLocalTime.Value != string.Empty)
        //        {
        //            objEligibile.Eligibility_Verified_Date = Convert.ToDateTime(hdnLocalTime.Value);
        //        }
        //        objEligibile.Created_By = ClientSession.UserName;
        //        if (hdnLocalTime.Value != string.Empty)
        //        {

        //            objEligibile.Created_DateAndTime = Convert.ToDateTime(hdnLocalTime.Value);
        //        }
        //        if (txtComments.Text != string.Empty)
        //        {
        //            objEligibile.Comments = txtComments.Text;
        //            EligibilityDetails.Add("Comments!" + txtComments.Text);
        //        }
        //        InsurancePlanManager insMngr = new InsurancePlanManager();
        //        InsurancePlan objInsPlan = null;
        //        IList<InsurancePlan> InsList = insMngr.GetInsurancebyID(Convert.ToUInt64(txtInsPlanID.Text));
        //        if (InsList.Count > 0)
        //        {
        //            objInsPlan = InsList[0];
        //            objInsPlan.Claim_State = ddlState.Text;
        //            objInsPlan.Claim_ZipCode = txtZipCode.Text;
        //            objInsPlan.Claim_City = txtClaimCity.Text;
        //            objInsPlan.Claim_Address = txtClaimAddress.Text;
        //            objInsPlan.Modified_By = ClientSession.UserName;
        //            if (hdnLocalTime.Value != string.Empty)
        //            {
        //                objInsPlan.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
        //            }
        //        }

        //        //srividhya added on 03-Feb-2014
        //        PatientNotes PatNotesRecord = new PatientNotes();
        //        if (txtComments.Text != "")
        //        {
        //            PatNotesRecord.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
        //            PatNotesRecord.Message_Description = "Eligibility Verification";
        //            PatNotesRecord.Notes = txtComments.Text;
        //            PatNotesRecord.Created_By = ClientSession.UserName;
        //            //PatNotesRecord.Created_Date_And_Time = DateTime.Now;
        //            if (hdnLocalTime.Value != string.Empty)
        //            {
        //                PatNotesRecord.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
        //            }

        //            PatNotesRecord.Is_PatientChart = "N";
        //            PatNotesRecord.Type = "MESSAGE";

        //            if (hdnEncounterID.Value != "")
        //            {
        //                PatNotesRecord.Encounter_ID = Convert.ToInt32(hdnEncounterID.Value);
        //                PatNotesRecord.SourceID = Convert.ToInt32(hdnEncounterID.Value);
        //            }

        //            PatNotesRecord.Source = "EV";
        //            PatNotesRecord.IsDelete = "N";
        //            PatNotesRecord.Is_PopupEnable = "N";
        //            PatNotesRecord.Priority = "NORMAL";                    
        //           }

        //        EligibilityMngr.SaveEligibilityVerificationAndInsuranceRCM(objEligibile, objInsPlan, string.Empty, PatNotesRecord);
        //        Eligibility_VerficationManager.EligibilityVerificationDetails = EligibilityDetails;
        //        //ApplicationObject.erroHandler.DisplayErrorMessage("350006", "Eligibility Verification", this.Page);

        //        if (UploadEV.UploadedFiles.Count > 0)
        //        {
        //            foreach (UploadedFile file in UploadEV.UploadedFiles)
        //            {
        //                if (file.FileName.ToUpper().Contains(".TIF") || file.FileName.ToUpper().Contains(".PNG") || file.FileName.ToUpper().Contains(".BMP") || file.FileName.ToUpper().Contains(".GIF") || file.FileName.ToUpper().Contains(".JPG") || file.FileName.ToUpper().Contains(".JPEG") || file.FileName.ToUpper().Contains(".DOCX") || file.FileName.ToUpper().Contains(".PDF") || file.FileName.ToUpper().Contains(".XLS"))
        //                {
        //                    string server_path = string.Empty;
        //                    string SelectedFilePath = Server.MapPath("~/atala-capture-upload/" + Session.SessionID + "/EV/" + txtPatientAccountNo.Text + "/" + Path.GetFileName(file.FileName));
        //                    file.SaveAs(SelectedFilePath);
        //                    FTPImageProcess ftpImageProcess = new FTPImageProcess();
        //                    string ftpUserID = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
        //                    string ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];
        //                    string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];

        //                    if (ftpImageProcess.CreateDirectory(txtPatientAccountNo.Text, ftpServerIP, ftpUserID, ftpPassword))
        //                    {
        //                        if (MyCreateDirectory(txtPatientAccountNo.Text, ftpServerIP, ftpUserID, ftpPassword, string.Empty))
        //                        {
        //                            string serverPath = string.Empty;
        //                            serverPath = ftpImageProcess.UploadToImageServer(txtPatientAccountNo.Text, ftpServerIP, ftpUserID, ftpPassword, SelectedFilePath, string.Empty);
        //                            if (fileExistList.Count == 0)
        //                            {
        //                                if (serverPath != string.Empty)
        //                                {
        //                                    FileManagementIndex filemanagementIndex = new FileManagementIndex();
        //                                    fileManagementIndexList = new List<FileManagementIndex>();
        //                                    filemanagementIndex.Created_By = ClientSession.UserName;
        //                                    //srividhya
        //                                    if (hdnLocalTime.Value != string.Empty)
        //                                    {
        //                                        filemanagementIndex.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);// UtilityManager.ConvertToUniversal();
        //                                    }
        //                                    filemanagementIndex.Source = "DEMOGRAPHICS";
        //                                    filemanagementIndex.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
        //                                    //filemanagementIndex.Order_ID = ulEncounterId;
        //                                    filemanagementIndex.Scan_Index_Conversion_ID = 0;
        //                                    filemanagementIndex.File_Path = serverPath;
        //                                    fileManagementIndexList.Add(filemanagementIndex);
        //                                    FleManagementMngr.SaveUpdateDeleteFileManagementIndexForOnline(fileManagementIndexList.ToArray(), null, null, string.Empty);
        //                                }
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "showVal", "SaveClose();", true);
        //        saveCheckingFlag = false;
        //        bSaveCompleted = true;
        //    }
        //    else
        //    {
        //        Eligibility_VerficationManager.EligibilityVerificationDetails = null;
        //    }
        //    // this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "showVal", "CloseWindow();", true);

        //}

        protected void btnBrowse_Click(object sender, EventArgs e)
        {

        }


        public void LoadComboBox()
        {

            string[] FieldName = { "VERIFICATIONTYPE", "METHOD OF PAYMENT", "SEX", "PREFERRED CONFIDENTIAL CORRESPONDENCE MODE", "HUMAN TYPE", "RELATIONSHIP_FOR_PAYMENT" };


            objCheckOut = HumanMngr.GetStaticLookupandCarrier(FieldName);
            //            <<<<<<< frmEligibilityVerification.aspx.cs
            //            IList<Carrier> carrierLst = objCheckOut.CarrierList;
            //=======
            //            objCheckOut = HumanMngr.GetStaticLookupandCarrier(FieldName);
            //            //IList<Carrier> carrierLst = objCheckOut.CarrierList;
            //>>>>>>> 1.30.16.3.274.1.4.4

            //if (carrierLst != null && carrierLst.Count > 0)
            //{
            //    Session["CarrierList"] = objCheckOut.CarrierList;
            //    ddlPayerName.Items.Add("");/*01/02*/
            //    ddlPayerName.Items.Add("OTHER");

            //    for (int i = 0; i < carrierLst.Count; i++)
            //    {
            //        ListItem lstCarrier = new ListItem();
            //        lstCarrier.Text = carrierLst[i].Carrier_Name;
            //        lstCarrier.Value = carrierLst[i].Id.ToString();
            //        //ddlPayerName.Items.Add(lstCarrier);/*01/02*/


            //    }
            //}
        }



        //protected void ddlPayerName_SelectedIndexChanged(object sender, EventArgs e)/*01/02*/
        //{

        //    //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);

        //    if (ddlPayerName.SelectedIndex != 0)
        //    {
        //        //bClose = true;
        //        if (ddlPayerName.SelectedItem.Text == "OTHER")
        //        {
        //            ddlPlanName.Items.Clear();
        //            ddlPlanName.Items.Add("OTHER");
        //            //TextBoxColorChange(txtOtherPayerName, true);
        //            //TextBoxColorChange(txtOtherPlan, true);
        //            //ChangeLabelStyle(lblOtherPayer, false); /*have to change*/
        //            //ChangeLabelStyle(lblOtherPlanName, false);
        //            txtClaimAddress.Text = string.Empty;
        //            txtClaimCity.Text = string.Empty;
        //            //msktxtClaimZipCode.Text = string.Empty;

        //            //txtClaimState.Text = string.Empty;


        //            //spanPayer.Attributes.Remove("class"); /*added*/

        //            //spanPayer.Attributes.Add("class", "MandLabelstyle");
        //            //spanOtherPayer.Visible = true;

        //            //spanPlan.Attributes.Remove("class");

        //            //spanPlan.Attributes.Add("class", "MandLabelstyle");
        //            //spanOtherPlanStar.Visible = true;
        //        }
        //        else
        //        {

        //            //  ddlPlanName.Text = "";
        //            inslist = ((InsMngr.GetInsurancebyCarrierID(Convert.ToUInt64(ddlPayerName.Items[ddlPayerName.SelectedIndex].Value))).OrderBy(s => s.Ins_Plan_Name)).ToList<InsurancePlan>();

        //            ddlPlanName.Items.Clear();
        //            ddlPlanName.Items.Add("");

        //            if (inslist != null)
        //            {
        //                for (int i = 0; i < inslist.Count; i++)
        //                {
        //                    ListItem ddlItem = new ListItem();
        //                    ddlItem.Text = inslist[i].Ins_Plan_Name;
        //                    ddlItem.Value = inslist[i].Id.ToString();
        //                    ddlPlanName.Items.Add(ddlItem);


        //                }
        //                //CboPlanName.Items[i].ToolTipText = inslist[i].Ins_Plan_Name;
        //                ddlPlanName.SelectedIndex = 0;
        //            }
        //            //TextBoxColorChange(txtOtherPayerName, false);
        //            //TextBoxColorChange(txtOtherPlan, false);
        //            //txtOtherPayerName.Text = string.Empty;
        //            //txtOtherPlan.Text = string.Empty;
        //            //ChangeLabelStyle(lblOtherPayer, true);     /* have to change*/
        //            //ChangeLabelStyle(lblOtherPlanName, true);
        //        }


        //        //btnSave.Enabled = true;
        //    }
        //    else
        //    {
        //        ddlPlanName.Items.Clear();
        //        //txtOtherPayerName.Text = string.Empty;
        //        //txtOtherPlan.Text = string.Empty;
        //        //ChangeLabelStyle(lblOtherPayer, true);  /*have to change*/
        //        //ChangeLabelStyle(lblOtherPlanName, true);

        //        //spanPayer.Attributes.Remove("class"); /*added*/
        //        //spanPayer.Attributes.Add("class", "spanstyle");
        //        //spanOtherPayer.Visible = true;
        //        //spanOtherPayer.Visible = false;

        //        //spanPlan.Attributes.Remove("class");
        //        //spanPlan.Attributes.Add("class", "spanstyle");
        //        //spanOtherPlanStar.Visible = true;
        //        //spanOtherPlanStar.Visible = false;
        //    }

        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "closeload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();loadAuthafterEV()}", true);

        //}



        //protected void ddlPlanName_SelectedIndexChanged(object sender, EventArgs e)/*01/02*/
        //{
        //    if (ddlPlanName.SelectedIndex != 0)
        //    {


        //        if (ddlPayerName.SelectedItem.Text == "OTHER" && ddlPlanName.SelectedItem.Text == "OTHER")
        //        {
        //            //TextBoxColorChange(txtOtherPayerName, true);
        //            //TextBoxColorChange(txtOtherPlan, true);
        //            txtClaimAddress.Text = string.Empty;
        //            txtClaimCity.Text = string.Empty;
        //            //msktxtClaimZipCode.Text = string.Empty;

        //            //txtClaimState.Text = string.Empty;
        //        }
        //        else if (ddlPayerName.SelectedItem.Text != "OTHER")
        //        {
        //            IList<InsurancePlan> insplanlist = ((InsMngr.GetInsurancebyID(Convert.ToUInt64(ddlPlanName.Items[ddlPlanName.SelectedIndex].Value))).OrderBy(s => s.Ins_Plan_Name)).ToList<InsurancePlan>();
        //            if (insplanlist != null && insplanlist.Count > 0)
        //            {
        //                txtClaimAddress.Text = insplanlist[0].Claim_Address;
        //                txtClaimCity.Text = insplanlist[0].Claim_City;
        //                //msktxtClaimZipCode.Text = insplanlist[0].Claim_ZipCode;

        //                //txtClaimState.Text = insplanlist[0].Claim_State;



        //            }
        //            //TextBoxColorChange(txtOtherPayerName, false);
        //            //TextBoxColorChange(txtOtherPlan, false);
        //        }
        //    }
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ChangeLabel", "loadAuthafterEV();", true);
        //}



        //public void FillPlanDetails()/*01/02*/
        //{
        //    if (ddlPayerName.SelectedIndex != 0)
        //    {
        //        //bClose = true;
        //        if (ddlPayerName.SelectedItem.Text == "OTHER")
        //        {
        //            ddlPlanName.Items.Clear();
        //            ddlPlanName.Items.Add("OTHER");

        //        }
        //        else
        //        {
        //            // IList<InsurancePlan> inslist;
        //            //  ddlPlanName.Text = "";
        //            inslist = ((InsMngr.GetInsurancebyCarrierID(Convert.ToUInt64(ddlPayerName.Items[ddlPayerName.SelectedIndex].Value))).OrderBy(s => s.Ins_Plan_Name)).ToList<InsurancePlan>();

        //            ddlPlanName.Items.Clear();
        //            ddlPlanName.Items.Add("");

        //            if (inslist != null)
        //            {
        //                for (int i = 0; i < inslist.Count; i++)
        //                {
        //                    ListItem ddlItem = new ListItem();
        //                    ddlItem.Text = inslist[i].Ins_Plan_Name;
        //                    ddlItem.Value = inslist[i].Id.ToString();
        //                    ddlPlanName.Items.Add(ddlItem);


        //                }
        //                //CboPlanName.Items[i].ToolTipText = inslist[i].Ins_Plan_Name;
        //                ddlPlanName.SelectedIndex = 0;
        //            }
        //            //TextBoxColorChange(txtOtherPayerName, false);
        //            //TextBoxColorChange(txtOtherPlan, false);
        //            //txtOtherPayerName.Text = string.Empty;
        //            //txtOtherPlan.Text = string.Empty;
        //        }


        //        //btnSave.Enabled = true;
        //    }
        //    else
        //    {
        //        ddlPlanName.Items.Clear();
        //        //TextBoxColorChange(txtOtherPayerName, false);
        //        //TextBoxColorChange(txtOtherPlan, false);
        //        //txtOtherPayerName.Text = string.Empty;
        //        //txtOtherPlan.Text = string.Empty;
        //    }
        //}



        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    if (UploadEV.UploadedFiles.Count > 0)
        //    {
        //        FTPImageProcess ftpImageProcess = new FTPImageProcess();
        //        string ftpUserID = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
        //        string ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];
        //        string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];

        //        if (ftpImageProcess.CreateDirectory(txtPatientAccountNo.Text, ftpServerIP, ftpUserID, ftpPassword))
        //        {
        //            if (MyCreateDirectory(txtPatientAccountNo.Text, ftpServerIP, ftpUserID, ftpPassword, string.Empty))
        //            {
        //                string serverPath = string.Empty;
        //                serverPath = ftpImageProcess.UploadToImageServer(txtPatientAccountNo.Text, ftpServerIP, ftpUserID, ftpPassword, , string.Empty);
        //                if (fileExistList.Count == 0)
        //                {
        //                    if (serverPath != string.Empty)
        //                    {
        //                        FileManagementIndex filemanagementIndex = new FileManagementIndex();
        //                        fileManagementIndexList = new List<FileManagementIndex>();
        //                        filemanagementIndex.Created_By = ClientSession.UserName;
        //                        //srividhya
        //                        if (hdnLocalTime.Value != string.Empty)
        //                        {
        //                            filemanagementIndex.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);// UtilityManager.ConvertToUniversal();
        //                        }
        //                        //filemanagementIndex.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
        //                        filemanagementIndex.Source = "DEMOGRAPHICS";
        //                        filemanagementIndex.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
        //                        //filemanagementIndex.Order_ID = ulEncounterId;
        //                        filemanagementIndex.Scan_Index_Conversion_ID = 0;
        //                        filemanagementIndex.File_Path = serverPath;
        //                        fileManagementIndexList.Add(filemanagementIndex);
        //                        FleManagementMngr.SaveUpdateDeleteFileManagementIndexForOnline(fileManagementIndexList.ToArray(), null, null, string.Empty);
        //                    }
        //                }

        //            }
        //        }
        //    }
        //}
        public bool MyCreateDirectory(string HumanID, string serverIP, string UserName, string Password, string EncounterID)
        {
            string uri = string.Empty;
            FtpWebRequest reqFTP;
            FtpWebResponse responseFTP;
            uri = serverIP + HumanID + "/";
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(UserName, Password);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                responseFTP = (FtpWebResponse)reqFTP.GetResponse();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                }
            }
            return true;
        }

        //protected void btnSave_Click1(object sender, EventArgs e)
        //{
        //    btnAdd.Enabled = false;
        //    if (!Validation())
        //    {
        //        btnAdd.Enabled = true;
        //        bSaveCompleted = false;
        //        saveCheckingFlag = true;
        //        return;
        //    }
        //    if (hdnSaveFlag.Value.ToUpper() == "TRUE")
        //    {
        //        Eligibility_Verification objEligibile = new Eligibility_Verification();
        //        IList EligibilityDetails = new List<string>();
        //        //if (dtpEffectiveStartDate.SelectedDate != null)//"__-___-____")
        //        //{
        //        //    EligibilityDetails.Add("Effective Start Date!" + dtpEffectiveStartDate.SelectedDate.Value);
        //        //    objEligibile.Effective_Date = Convert.ToDateTime(dtpEffectiveStartDate.SelectedDate.Value);
        //        //}

        //        if (dtpEffectiveStartDate.Text != "")//"__-___-____")
        //        {
        //            EligibilityDetails.Add("Effective Start Date!" + dtpEffectiveStartDate.Text);
        //            objEligibile.Effective_Date = Convert.ToDateTime(dtpEffectiveStartDate.Text);
        //        }

        //        //if (dtpTerminationDate.SelectedDate != null)// "__-___-____")
        //        //{
        //        //    EligibilityDetails.Add("Termination Date!" + dtpTerminationDate.SelectedDate.Value);
        //        //    objEligibile.Termination_Date = Convert.ToDateTime(dtpTerminationDate.SelectedDate.Value);
        //        //}

        //        if (dtpTerminationDate.Text != "")// "__-___-____")
        //        {
        //            EligibilityDetails.Add("Termination Date!" + dtpTerminationDate.Text);
        //            objEligibile.Termination_Date = Convert.ToDateTime(dtpTerminationDate.Text);
        //        }

        //        if (txtPCPCopay.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("PCP Copay!" + txtPCPCopay.Text);
        //            objEligibile.PCP_Copay = Convert.ToDouble(txtPCPCopay.Text);
        //        }
        //        if (txtSPCCopay.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("SPC Copay!" + txtSPCCopay.Text);
        //            objEligibile.SPC_Copay = Convert.ToDouble(txtSPCCopay.Text);
        //        }
        //        if (txtDeductibleforThePlan.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("Deductible for the Plan!" + txtDeductibleforThePlan.Text);
        //            objEligibile.Deductible_For_Plan = Convert.ToDouble(txtDeductibleforThePlan.Text);
        //        }
        //        if (txtCoInsurance.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("Co Insurance!" + txtCoInsurance.Text);
        //            objEligibile.Coinsurance = Convert.ToDouble(txtCoInsurance.Text);
        //        }
        //        if (ddlEVType.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("EV Type!" + ddlEVType.Text);
        //            objEligibile.Eligibility_Type = ddlEVType.Text;
        //        }
        //        if (txtCallRepresentative.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("Call Representative Name!" + txtCallRepresentative.Text);
        //            objEligibile.Call_Rep_Name = txtCallRepresentative.Text;
        //        }
        //        if (txtCallReference.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("Call Reference Number!" + txtCallReference.Text);
        //            objEligibile.Call_Ref_Number = txtCallReference.Text;
        //        }
        //        if (txtHyperlinkToWeb.Text != string.Empty)
        //        {
        //            EligibilityDetails.Add("Hyperlink to web image!" + txtHyperlinkToWeb.Text);
        //            objEligibile.Hyperlink = txtHyperlinkToWeb.Text;
        //        }
        //        objEligibile.Human_ID = Convert.ToUInt64(hdnmyHumanID.Value.ToString());
        //        if (hdnInsPlanId.Value.ToString() != string.Empty)
        //        {
        //            objEligibile.Insurance_Plan_ID = Convert.ToUInt64(hdnInsPlanId.Value);
        //        }

        //        objEligibile.Policy_Holder_ID = hdnmyPolicyHolderId.Value.ToString();
        //        objEligibile.Group_Number = hdnmyGroupNumber.Value.ToString();
        //        objEligibile.Eligibility_Verified_By = ClientSession.UserName;
        //        if (hdnLocalTime.Value != string.Empty)
        //        {
        //            objEligibile.Eligibility_Verified_Date = Convert.ToDateTime(hdnLocalTime.Value);
        //        }
        //        objEligibile.Created_By = ClientSession.UserName;
        //        if (hdnLocalTime.Value != string.Empty)
        //        {

        //            objEligibile.Created_DateAndTime = Convert.ToDateTime(hdnLocalTime.Value);
        //        }
        //        if (txtComments.Text != string.Empty)
        //        {
        //            objEligibile.Comments = txtComments.Text;
        //            EligibilityDetails.Add("Comments!" + txtComments.Text);
        //        }
        //        InsurancePlanManager insMngr = new InsurancePlanManager();
        //        InsurancePlan objInsPlan = null;
        //        IList<InsurancePlan> InsList = insMngr.GetInsurancebyID(Convert.ToUInt64(txtInsPlanID.Text));
        //        if (InsList.Count > 0)
        //        {
        //            objInsPlan = InsList[0];
        //            objInsPlan.Claim_State = ddlState.Text;
        //            objInsPlan.Claim_ZipCode = txtZipCode.Text;
        //            objInsPlan.Claim_City = txtClaimCity.Text;
        //            objInsPlan.Claim_Address = txtClaimAddress.Text;
        //            objInsPlan.Modified_By = ClientSession.UserName;
        //            if (hdnLocalTime.Value != string.Empty)
        //            {
        //                objInsPlan.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
        //            }
        //        }

        //        //srividhya added on 03-Feb-2014
        //        PatientNotes PatNotesRecord = new PatientNotes();
        //        if (txtComments.Text != "")
        //        {
        //            PatNotesRecord.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
        //            PatNotesRecord.Message_Description = "Eligibility Verification";
        //            PatNotesRecord.Notes = txtComments.Text;
        //            PatNotesRecord.Created_By = ClientSession.UserName;
        //            if (hdnLocalTime.Value != string.Empty)
        //            {
        //                PatNotesRecord.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
        //            }
        //            //PatNotesRecord.Created_Date_And_Time = DateTime.Now;
        //            PatNotesRecord.Is_PatientChart = "N";
        //            PatNotesRecord.Type = "MESSAGE";

        //            if (hdnEncounterID.Value != "")
        //            {
        //                PatNotesRecord.Encounter_ID = Convert.ToInt32(hdnEncounterID.Value);
        //                PatNotesRecord.SourceID = Convert.ToInt32(hdnEncounterID.Value);
        //            }

        //            PatNotesRecord.Source = "EV";
        //            PatNotesRecord.IsDelete = "N";
        //            PatNotesRecord.Is_PopupEnable = "N";
        //            PatNotesRecord.Priority = "NORMAL";
        //             }

        //        EligibilityMngr.SaveEligibilityVerificationAndInsuranceRCM(objEligibile, objInsPlan, string.Empty, PatNotesRecord);
        //        Eligibility_VerficationManager.EligibilityVerificationDetails = EligibilityDetails;
        //        //ApplicationObject.erroHandler.DisplayErrorMessage("350006", "Eligibility Verification", this.Page);

        //        if (UploadEV.UploadedFiles.Count > 0)
        //        {
        //            FTPImageProcess ftpImageProcess = new FTPImageProcess();
        //            string ftpUserID = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
        //            string ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];
        //            string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];

        //            if (ftpImageProcess.CreateDirectory(txtPatientAccountNo.Text, ftpServerIP, ftpUserID, ftpPassword))
        //            {
        //                if (MyCreateDirectory(txtPatientAccountNo.Text, ftpServerIP, ftpUserID, ftpPassword, string.Empty))
        //                {
        //                    string serverPath = string.Empty;
        //                    serverPath = ftpImageProcess.UploadToImageServer(txtPatientAccountNo.Text, ftpServerIP, ftpUserID, ftpPassword, UploadEVSheet.PostedFile.FileName, string.Empty);
        //                    if (fileExistList.Count == 0)
        //                    {
        //                        if (serverPath != string.Empty)
        //                        {
        //                            FileManagementIndex filemanagementIndex = new FileManagementIndex();
        //                            fileManagementIndexList = new List<FileManagementIndex>();
        //                            filemanagementIndex.Created_By = ClientSession.UserName;
        //                            //srividhya
        //                            if (hdnLocalTime.Value != string.Empty)
        //                            {
        //                                filemanagementIndex.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);// UtilityManager.ConvertToUniversal();
        //                            }
        //                            //filemanagementIndex.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
        //                            filemanagementIndex.Source = "DEMOGRAPHICS";
        //                            filemanagementIndex.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
        //                            //filemanagementIndex.Order_ID = ulEncounterId;
        //                            filemanagementIndex.Scan_Index_Conversion_ID = 0;
        //                            filemanagementIndex.File_Path = serverPath;
        //                            fileManagementIndexList.Add(filemanagementIndex);
        //                            FleManagementMngr.SaveUpdateDeleteFileManagementIndexForOnline(fileManagementIndexList.ToArray(), null, null, string.Empty);
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "showVal", "SaveClose();", true);
        //        saveCheckingFlag = false;
        //        bSaveCompleted = true;
        //    }
        //    else
        //    {
        //        Eligibility_VerficationManager.EligibilityVerificationDetails = null;
        //    }
        //    // this.Pag
        //}

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Startloadcursor", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("atala-capture-upload/" + Session.SessionID + "/EV/" + txtPatientAccountNo.Text + "/"));
            if (!dir.Exists)
            {
                dir.Create();
            }
            FileInfo[] files = dir.GetFiles();
            for (int j = 0; j < files.Length; j++)
            {
                File.Delete(files[j].FullName);
            }
            string sValueForClose = hdnMessageType.Value;
            hdnMessageType.Value = string.Empty;
            btnAdd.Enabled = false;
            if (!Validation())
            {
                btnAdd.Enabled = true;
                bSaveCompleted = false;
                saveCheckingFlag = true;
                return;
            }
            prevNum = (int)Session["NumberCount"];
            if (hdnSaveFlag.Value.ToUpper() == "TRUE")
            {
                Eligibility_Verification objEligibile = new Eligibility_Verification();
                if (UploadEV.UploadedFiles.Count > 0)
                {
                    foreach (UploadedFile file in UploadEV.UploadedFiles)
                    {
                        if (file.FileName.ToUpper().Contains(".TIF") || file.FileName.ToUpper().Contains(".PNG") || file.FileName.ToUpper().Contains(".BMP") || file.FileName.ToUpper().Contains(".GIF") || file.FileName.ToUpper().Contains(".JPG") || file.FileName.ToUpper().Contains(".JPEG") || file.FileName.ToUpper().Contains(".DOCX") || file.FileName.ToUpper().Contains(".PDF") || file.FileName.ToUpper().Contains(".XLS"))
                        {
                            string server_path = string.Empty;
                            string SelectedFilePath = Server.MapPath("~/atala-capture-upload/" + Session.SessionID + "/EV/" + txtPatientAccountNo.Text + "/" + Path.GetFileName(file.FileName));
                            file.SaveAs(SelectedFilePath);

                            FTPImageProcess ftpImageProcess = new FTPImageProcess();
                            string ftpUserID = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
                            string ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];
                            string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];

                            //if (ftpImageProcess.CreateDirectory(txtPatientAccountNo.Text, ftpServerIP, ftpUserID, ftpPassword))
                            bool bCreateDirectory = ftpImageProcess.CreateDirectory(txtPatientAccountNo.Text, ftpServerIP, ftpUserID, ftpPassword, out string sCheckFileNotFoundException);
                            if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                                return;
                            }
                            if (bCreateDirectory)
                            {
                                if (MyCreateDirectory(txtPatientAccountNo.Text, ftpServerIP, ftpUserID, ftpPassword, string.Empty))
                                {
                                    string serverPath = string.Empty;
                                    lastNumToAdd = Convert.ToString(prevNum + 1);
                                    if (lastNumToAdd.Length == 1)
                                        lastNumToAdd = "0" + lastNumToAdd;
                                    string sStoringFormat = ClientSession.FacilityName.Replace("#", "") + "_EV_" + DateTime.Now.ToString("yyyyMMdd") + "_" + ClientSession.HumanId.ToString() + "_" + lastNumToAdd + Path.GetExtension(file.FileName);
                                    serverPath = ftpImageProcess.UploadToImageServer(txtPatientAccountNo.Text, ftpServerIP, ftpUserID, ftpPassword, SelectedFilePath, sStoringFormat, out string sCheckFileNotFoundExceptions);
                                    if (sCheckFileNotFoundExceptions != "" && sCheckFileNotFoundExceptions.Contains("CheckFileNotFoundException"))
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExceptions.Split('~')[1] + "\");", true);
                                        return;
                                    }
                                    if (serverPath != string.Empty)
                                    {
                                        FileManagementIndex filemanagementIndex = new FileManagementIndex();
                                        fileManagementIndexList = new List<FileManagementIndex>();
                                        filemanagementIndex.Created_By = ClientSession.UserName;

                                        filemanagementIndex.Source = "ELIGIBILITY VERIFICATION";
                                        if (txtPatientAccountNo.Text != "") //code added by balaji.TJ 2015-12-18
                                            filemanagementIndex.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                                        if (hdnLocalTime.Value != string.Empty)
                                        {
                                            filemanagementIndex.Document_Date = Convert.ToDateTime(hdnLocalTime.Value);
                                            //srividhya                                        
                                            filemanagementIndex.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);// UtilityManager.ConvertToUniversal();                                        
                                            //filemanagementIndex.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                        }
                                        //filemanagementIndex.Order_ID = ulEncounterId;
                                        filemanagementIndex.Scan_Index_Conversion_ID = 0;
                                        filemanagementIndex.Document_Type = "Insurance";
                                        filemanagementIndex.Document_Sub_Type = "Insurance eligibility sheet";
                                        filemanagementIndex.File_Path = serverPath;
                                        fileManagementIndexList.Add(filemanagementIndex);
                                        IList<FileManagementIndex> FileList = FleManagementMngr.SaveUpdateDeleteFileManagementIndexForRCM(fileManagementIndexList.ToArray(), null, null, string.Empty);
                                        if (FileList != null && FileList.Count > 0) //code added by balaji.TJ 2015-12-18
                                        {
                                            objEligibile.File_Management_Index_ID = Convert.ToInt32(FileList[0].Id);
                                        }
                                        prevNum++;
                                        Session["NumberCount"] = prevNum;
                                    }
                                }
                            }
                        }
                    }
                }
                IList EligibilityDetails = new List<string>();
                //if (dtpEffectiveStartDate.SelectedDate != null)//"__-___-____")
                //{
                //    EligibilityDetails.Add("Effective Start Date!" + dtpEffectiveStartDate.SelectedDate.Value);
                //    objEligibile.Effective_Date = Convert.ToDateTime(dtpEffectiveStartDate.SelectedDate.Value);
                //}

                if (dtpEffectiveStartDate.Text != "")//"__-___-____")
                {
                    EligibilityDetails.Add("Effective Start Date!" + dtpEffectiveStartDate.Text);
                    objEligibile.Effective_Date = Convert.ToDateTime(dtpEffectiveStartDate.Text);
                }

                //if (dtpTerminationDate.SelectedDate != null)//"__-___-____")
                //{
                //    EligibilityDetails.Add("Termination Date!" + dtpTerminationDate.SelectedDate.Value);
                //    objEligibile.Termination_Date = Convert.ToDateTime(dtpTerminationDate.SelectedDate.Value);
                //}

                if (dtpTerminationDate.Text != "")//"__-___-____")
                {
                    EligibilityDetails.Add("Termination Date!" + dtpTerminationDate.Text);
                    objEligibile.Termination_Date = Convert.ToDateTime(dtpTerminationDate.Text);
                }

                if (txtPCPCopay.Text != string.Empty && System.Text.RegularExpressions.Regex.IsMatch(txtPCPCopay.Text, "^[0-9]*$")==true)
                {
                    EligibilityDetails.Add("PCP Copay!" + txtPCPCopay.Text);
                    objEligibile.PCP_Copay = Convert.ToDouble(txtPCPCopay.Text);
                }
                if (txtSPCCopay.Text != string.Empty && System.Text.RegularExpressions.Regex.IsMatch(txtSPCCopay.Text, "^[0-9]*$") == true)
                {
                    EligibilityDetails.Add("SPC Copay!" + txtSPCCopay.Text);
                    objEligibile.SPC_Copay = Convert.ToDouble(txtSPCCopay.Text);
                }
                if (txtDeductibleforThePlan.Text != string.Empty && System.Text.RegularExpressions.Regex.IsMatch(txtDeductibleforThePlan.Text, "^[0-9]*$") == true)
                {
                    EligibilityDetails.Add("Deductible for the Plan!" + txtDeductibleforThePlan.Text);
                    objEligibile.Deductible_For_Plan = Convert.ToDouble(txtDeductibleforThePlan.Text);
                }
                if (txtCoInsurance.Text != string.Empty && System.Text.RegularExpressions.Regex.IsMatch(txtCoInsurance.Text, "^[0-9]*$") == true)
                {
                    EligibilityDetails.Add("Co Insurance!" + txtCoInsurance.Text);
                    objEligibile.Coinsurance = Convert.ToDouble(txtCoInsurance.Text);
                }
                if (ddlEVType.Text != string.Empty)
                {
                    EligibilityDetails.Add("EV Type!" + ddlEVType.Text);
                    objEligibile.Eligibility_Type = ddlEVType.Text;
                }
                if (txtCallRepresentative.Text != string.Empty)
                {
                    EligibilityDetails.Add("Call Representative Name!" + txtCallRepresentative.Text);
                    objEligibile.Call_Rep_Name = txtCallRepresentative.Text;
                }
                if (txtCallReference.Text != string.Empty)
                {
                    EligibilityDetails.Add("Call Reference Number!" + txtCallReference.Text);
                    objEligibile.Call_Ref_Number = txtCallReference.Text;
                }
                //if (txtHyperlinkToWeb.Text != string.Empty)
                //{
                //    EligibilityDetails.Add("Hyperlink to web image!" + txtHyperlinkToWeb.Text);
                //    objEligibile.Hyperlink = txtHyperlinkToWeb.Text;
                //}
                if (hdnmyHumanID.Value!=null&&hdnmyHumanID.Value.Trim() != "") //code added by balaji.TJ 2015-12-18
                    objEligibile.Human_ID = Convert.ToUInt64(hdnmyHumanID.Value.ToString());
                if (hdnInsPlanId.Value!=null&&hdnInsPlanId.Value.ToString().Trim() != string.Empty)
                {
                    objEligibile.Insurance_Plan_ID = Convert.ToUInt64(hdnInsPlanId.Value);
                }

                objEligibile.Policy_Holder_ID = hdnmyPolicyHolderId.Value.ToString();
                objEligibile.Group_Number = hdnmyGroupNumber.Value.ToString();
                objEligibile.Eligibility_Verified_By = ClientSession.UserName;
                if (hdnLocalTime.Value != string.Empty)
                {
                    objEligibile.Eligibility_Verified_Date = Convert.ToDateTime(hdnLocalTime.Value);
                }
                objEligibile.Created_By = ClientSession.UserName;
                if (hdnLocalTime.Value != string.Empty)
                {

                    objEligibile.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                }
                if (txtComments.Text != string.Empty)
                {
                    objEligibile.Comments = txtComments.Text;
                    EligibilityDetails.Add("Comments!" + txtComments.Text);
                }
                if (txtComments.Text != string.Empty)
                {
                    objEligibile.Comments = txtComments.Text;
                    EligibilityDetails.Add("Comments!" + txtComments.Text);
                }
                //if (ddlEligibilityStatus.Text != string.Empty)
                //{
                //    objEligibile.Comments = txtComments.Text;
                //    EligibilityDetails.Add("ELIGIBILITYSTATUS!" + ddlEligibilityStatus.Text);
                //}
                if (txtCarrierName.Text != string.Empty)
                {
                    EligibilityDetails.Add("Carrier From EV!" + txtCarrierName.Text);
                }
                if (txtDeductibleMet.Text.Trim() != string.Empty && System.Text.RegularExpressions.Regex.IsMatch(txtDeductibleMet.Text, "^[0-9]*$") == true)
                {
                    EligibilityDetails.Add("Deductible Met!" + txtDeductibleMet.Text);
                    objEligibile.Deductible_Met_So_Far = Convert.ToDouble(txtDeductibleMet.Text);
                }
                //objEligibile.Eligibility_Status = ddlEligibilityStatus.Text;
                objEligibile.Eligibility_Check_Mode = "MANUAL";
                objEligibile.Eligibility_Status = "EV-PERFORMED MANUALLY";// bug Id:67077 
                objEligibile.Payer_Name = txtCarrierName.Text;
                objEligibile.Insurance_Plan_Name = txtInsPlanName.Text;
                PatientInsuredPlanManager objpat = new PatientInsuredPlanManager();
                IList<PatientInsuredPlan> lstpat = new List<PatientInsuredPlan>();
                if (hdnmyHumanID.Value != null && hdnmyHumanID.Value.Trim() != "")
                {
                    lstpat = objpat.getInsurancePoliciesByHumanId(Convert.ToUInt64(hdnmyHumanID.Value));
                    if (hdnInsPlanId.Value != null && hdnInsPlanId.Value.Trim() != "")
                    {

                        lstpat = (from m in lstpat where m.Insurance_Plan_ID == (Convert.ToUInt64(hdnInsPlanId.Value)) && m.Active.ToUpper() == "YES" select m).ToList<PatientInsuredPlan>();
                    }
                }
                if (lstpat.Count > 0)
                    objEligibile.Insurance_Type = lstpat[0].Insurance_Type;
                InsurancePlan objInsPlan = null;
                if (ddlState.Text.Trim() != string.Empty || txtZipCode.TextWithLiterals.Trim() != "-" || txtClaimCity.Text.Trim() != string.Empty || txtClaimAddress.Text.Trim() != string.Empty)
                {
                    IList<InsurancePlan> InsList = new List<InsurancePlan>(); //code added by balaji.TJ 2015-12-18
                    //if (txtInsPlanID.Text != "")
                    //    InsList = InsMngr.GetInsurancebyID(Convert.ToUInt64(txtInsPlanID.Text));
                    if (InsList != null && InsList.Count > 0)
                    {
                        objInsPlan = InsList[0];
                        if (ddlState.Text.Trim() != string.Empty)
                        {
                            objInsPlan.Claim_State = ddlState.Text;
                        }
                        if (txtZipCode.TextWithLiterals.Trim() != "-")
                            objInsPlan.Claim_ZipCode = txtZipCode.Text;
                        if (txtClaimCity.Text.Trim() != string.Empty)
                            objInsPlan.Claim_City = txtClaimCity.Text;
                        if (txtClaimAddress.Text.Trim() != string.Empty)
                            objInsPlan.Claim_Address = txtClaimAddress.Text;
                        objInsPlan.Modified_By = ClientSession.UserName;
                        if (hdnLocalTime.Value != string.Empty)
                        {
                            objInsPlan.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                    }
                }
                //srividhya added on 03-Feb-2014
                PatientNotes PatNotesRecord = new PatientNotes();
                if (txtComments.Text != "")
                {
                    if (txtPatientAccountNo.Text != null && txtPatientAccountNo.Text.Trim()!="")
                        PatNotesRecord.Human_ID = Convert.ToUInt64(txtPatientAccountNo.Text);
                    PatNotesRecord.Message_Description = "Eligibility Verification";
                    PatNotesRecord.Notes = txtComments.Text;
                    PatNotesRecord.Created_By = ClientSession.UserName;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        PatNotesRecord.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    }
                    //PatNotesRecord.Created_Date_And_Time = DateTime.Now;
                    PatNotesRecord.Is_PatientChart = "N";
                    PatNotesRecord.Type = "MESSAGE";

                    if (hdnEncounterID.Value != "")
                    {
                        PatNotesRecord.Encounter_ID = Convert.ToInt32(hdnEncounterID.Value);
                        PatNotesRecord.SourceID = Convert.ToInt32(hdnEncounterID.Value);
                    }

                    PatNotesRecord.Source = "EV";
                    PatNotesRecord.IsDelete = "N";
                    PatNotesRecord.Is_PopupEnable = "N";
                    PatNotesRecord.Priority = "NORMAL";
                }

                EligibilityMngr.SaveEligibilityVerificationAndInsuranceRCM(objEligibile, objInsPlan, string.Empty, PatNotesRecord);
                Eligibility_VerficationManager.EligibilityVerificationDetails = EligibilityDetails;
                //ApplicationObject.erroHandler.DisplayErrorMessage("350006", "EV", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "showVal", "SaveClose();", true);
                saveCheckingFlag = false;
                bSaveCompleted = true;
            }
            else
            {
                Eligibility_VerficationManager.EligibilityVerificationDetails = null;
            }
            if (sValueForClose == "Yes")
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "showVal", "window.close();", true);
            }
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Stoploadcursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void txtFileName_TextChanged(object sender, EventArgs e)
        {

        }

        void FindFileIndex(IList<FileManagementIndex> file_exam_lst)
        {
            if (file_exam_lst != null) //code added by balaji.TJ 2015-12-18
            {
                int[] sortIndexNum = new int[file_exam_lst.Count];
                for (int i = 0; i < file_exam_lst.Count; i++)
                {
                    if (file_exam_lst[i].File_Path != string.Empty)
                    {
                        try
                        {
                            string fi = file_exam_lst[i].File_Path.Substring(file_exam_lst[i].File_Path.LastIndexOf("/") + 1);
                            prevNum = Convert.ToInt32(fi.Substring(fi.LastIndexOf("_") + 1, (fi.LastIndexOf(".") - 1) - fi.LastIndexOf("_")));
                            sortIndexNum[i] = prevNum;
                        }
                        catch
                        {

                        }
                    }
                }
                if (file_exam_lst.Count > 0)
                    prevNum = sortIndexNum.Max();
                Session["NumberCount"] = prevNum;
            }
        }


    }
}
