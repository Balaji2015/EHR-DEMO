using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;

namespace Acurus.Capella.UI
{
    public partial class frmPerformEV : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtServieType.Value = ConfigurationManager.AppSettings["EVServiceType"].ToString().Split('|')[0];

                ServiceTypeCode.Value = ConfigurationManager.AppSettings["EVServiceType"].ToString().Split('|')[1];
                ServiceTypeCodeDesc.Value = txtServieType.Value;
            }
            ulong humanID = 0;
            if (Request["HumanId"] != null && Request["HumanId"] != null)
            {
                humanID = Convert.ToUInt64(Request["HumanId"]);
            }
            else
            {
                humanID = ClientSession.HumanId;

            }
            if (Request["InsPlanId"] != null && Request["InsPlanId"] != null)
            {
                hdnInsPlan_Id.Value = Request["InsPlanId"];
            }

             if (Request["IsPatInsurance"] != null && Request["IsPatInsurance"].ToString() == "true")
             {
                 btnSelectPlan.Visible = false;
             }
             else
             {
                 btnSelectPlan.Visible = true;
             }


            string sdivPatientstrip = UtilityManager.FillPatientStrip(humanID);
            if (sdivPatientstrip != null)
            {
                divPatientstrip.InnerText = sdivPatientstrip;
            }


            //string FileName = "Human" + "_" + humanID + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    try
            //    {
            //        XmlDocument itemDoc = new XmlDocument();
            //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //        {
            //            itemDoc.Load(fs);

            //            XmlNodeList xmlhumanList = itemDoc.GetElementsByTagName("Human");
            //            Human objFillHuman = new Human();
            //            IList<Human> lstHuman = new List<Human>();
            //            if (xmlhumanList != null && xmlhumanList.Count > 0)
            //            {
            //                objFillHuman.Id = Convert.ToUInt64(xmlhumanList[0].Attributes.GetNamedItem("Id").Value);
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
            //                objFillHuman.Guarantor_Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Last_Name").Value;

            //                objFillHuman.Guarantor_Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Last_Name").Value;
            //                objFillHuman.Guarantor_MI = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_MI").Value;
            //                objFillHuman.Guarantor_Sex = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Sex").Value;

            //                objFillHuman.Guarantor_Street_Address1 = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Street_Address1").Value;
            //                objFillHuman.Street_Address1 = xmlhumanList[0].Attributes.GetNamedItem("Street_Address1").Value;
            //                objFillHuman.Guarantor_Birth_Date = Convert.ToDateTime(xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Birth_Date").Value);
            //                objFillHuman.Cell_Phone_Number = xmlhumanList[0].Attributes.GetNamedItem("Cell_Phone_Number").Value;
            //                // objFillHuman.Policy_Holder_Id = xmlhumanList[0].Attributes.GetNamedItem("Policy_Holder_Id").Value;
            //                //objFillHuman.Plan_Id = xmlhumanList[0].Attributes.GetNamedItem("Plan_Id");

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

            //            string sPatientSex = string.Empty;


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

            //            if (lstHuman != null && lstHuman.Count > 0)
            //            {

            //                //     txtpatientName.Value = objFillHuman.Last_Name + "," + objFillHuman.First_Name +
            //                //"  " + objFillHuman.MI + "  " + objFillHuman.Suffix + "|" + objFillHuman.Birth_Date.ToString("dd-MMM-yyyy") + " | " +
            //                // objFillHuman.Sex + " | " + humanID;
            //                if (objFillHuman.Patient_Account_External == "")
            //                {
            //                    hdnHumanDetails.Value = humanID + "~" + objFillHuman.Last_Name + "~" + objFillHuman.First_Name + "~0~" + objFillHuman.Human_Type + "~"
            //                        + ClientSession.EncounterId + "~" + ClientSession.UserCurrentProcess +
            //                        "~" + sPatientSex + "~" + objFillHuman.Street_Address1 + "~" + objFillHuman.Birth_Date
            //                        + "~" + objFillHuman.Guarantor_Birth_Date + "~" + objFillHuman.Guarantor_First_Name + "~" + objFillHuman.Guarantor_Last_Name + "~" +
            //                        objFillHuman.Guarantor_MI + "~" + objFillHuman.Guarantor_Sex + "~" + objFillHuman.MI + "~" + objFillHuman.Suffix;
            //                }
            //                else
            //                {
            //                    hdnHumanDetails.Value = humanID + "~" + objFillHuman.Last_Name + "~" + objFillHuman.First_Name + "~"
            //                        + objFillHuman.Patient_Account_External + "~" + objFillHuman.Human_Type + "~" +
            //                        ClientSession.EncounterId + "~" + ClientSession.UserCurrentProcess +
            //                        "~" + sPatientSex + "~" + objFillHuman.Street_Address1 + "~" + objFillHuman.Birth_Date
            //                        + "~" + objFillHuman.Guarantor_Birth_Date + "~" + objFillHuman.Guarantor_First_Name + "~" + objFillHuman.Guarantor_Last_Name + "~" +
            //                        objFillHuman.Guarantor_MI + "~" + objFillHuman.Guarantor_Sex + "~" + objFillHuman.MI + "~" + objFillHuman.Suffix;
            //                    ;
            //                }

            //                divPatientstrip.InnerText = " " + objFillHuman.Last_Name + "," + objFillHuman.First_Name +
            //   "  " + objFillHuman.MI + "  " + objFillHuman.Suffix + " | " +
            //    objFillHuman.Birth_Date.ToString("dd-MMM-yyyy") + " | " +
            //   (CalculateAge(objFillHuman.Birth_Date)).ToString() +
            //   "  year(s) | " + sPatientSex + " | Acc #:" + humanID +
            //   " | " + "Med Rec #:" + objFillHuman.Medical_Record_Number + " | " +
            //   "Phone #:" + phoneno + " | Patient Type:" + objFillHuman.Human_Type;
            //            }
            //            fs.Close();
            //            fs.Dispose();
            //        }
            //    }
            //    catch (Exception Ex)
            //    {
            //        //XmlText.Close();
            //        // ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
            //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + humanID.ToString() + "','Human','ev');", true);


            //        //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");

            //        return;
            //    }
            //}


            EncounterManager obj = new EncounterManager();
            IList<Encounter> lstencounter = new List<Encounter>();

            UtilityManager utl = new UtilityManager();
            DateTime currentdate = UtilityManager.ConvertToUniversal();
            //For Bug ID 61396
            //lstencounter = obj.GetEncounterListForEV(humanID, Convert.ToDateTime(currentdate).ToString("yyyy-MM-dd hh:mm:ss tt"));
            lstencounter = obj.GetEncounterListForEV(humanID, Convert.ToDateTime(currentdate).ToString("yyyy-MM-dd 00:00:00"));
            seldateNextApp.Items.Clear();
            seldateNextApp.Items.Add("---Select---");
            if (lstencounter.Count > 0)
            {
                for (int i = 0; i < lstencounter.Count; i++)
                {
                    seldateNextApp.Items.Add(UtilityManager.ConvertToLocal(Convert.ToDateTime(lstencounter[i].Appointment_Date)).ToString("dd-MMM-yyyy hh:mm tt"));

                }

            }

            if (Request["ScreenMode"] != null)
                if (Request["ScreenMode"] == "ManualEv")
                    //btnmEV.Enabled = false;
                   btnmEV.Style.Add("display", "none !important");
                       

            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoadEV", "bindEVTable();", true);
            
            // }
            if (ClientSession.UserPermissionDTO.Scntab != null)
            {
                var scn_id = (from p in ClientSession.UserPermissionDTO.Scntab where p.SCN_Name == "frmPerformEV" select p).ToList();
                if (scn_id.Count() > 0)
                {
                    var SendEV = from p in ClientSession.UserPermissionDTO.Screens where p.SCN_ID == Convert.ToInt32(scn_id[0].SCN_ID) && p.Permission == "U" select p;
                    if (SendEV.Count() > 0)
                        hdnIsSendEVEnable.Value = "false";
                    else
                        hdnIsSendEVEnable.Value = "true";
                }
                else
                    hdnIsSendEVEnable.Value = "true";
            }

        }
        public void hdnbtngeneratexml_Click(object sender, EventArgs e)
        {

            //  Patientchartload();
        }
        public int CalculateAge(DateTime birthDate)
        {
            DateTime now = DateTime.Today;
            int years = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;
            return years;
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string LoadEV(string HumanID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            try
            {


                Eligibility_VerficationManager objEV = new Eligibility_VerficationManager();

                ArrayList AuthDetails = new ArrayList();
                IList<AuthorizationEncounter> lstauthenc = new List<AuthorizationEncounter>();

                ClientSession.HumanId = Convert.ToUInt64(HumanID);

                AuthDetails = objEV.GetEvDetails(Convert.ToUInt64(HumanID));


                var result = new { EV = AuthDetails };
                return JsonConvert.SerializeObject(result);

            }
            catch (Exception exception)
            {
                var lstFinalResult = new
                {
                    Error = "The following error occurred :" + exception.Message + ". Please contact support."
                };
                return JsonConvert.SerializeObject(lstFinalResult);
            }
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
       
        public static string SaveEVDetails(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            try
            {

                IList<Eligibility_Verification> lstEv = new List<Eligibility_Verification>();
                Eligibility_Verification objev = new Eligibility_Verification();
                Eligibility_VerficationManager objevman = new Eligibility_VerficationManager();
                objev.Human_ID = Convert.ToUInt64(data[0]);
                objev.Payer_Name = data[1];
                objev.Insurance_Plan_ID = Convert.ToUInt64(data[2]);
                objev.Insurance_Plan_Name = data[3];
                objev.Policy_Holder_ID = data[4];
                objev.TRN02TraceNumber1 = data[5];
                objev.Service_Type_Code = data[6];
                objev.Service_Type = data[7];
                objev.Insurance_Type = data[8];
                if (data[9] != null && data[9] != "")
                    objev.Requested_For_From_Date = Convert.ToDateTime(data[9]);
                if (data[10] != null && data[10] != "")
                    objev.Requested_For_To_Date = Convert.ToDateTime(data[10]);
                objev.Eligibility_Status = data[11];
                objev.Requested_By = ClientSession.UserName;
                objev.Eligibility_Verified_By = ClientSession.UserName;
                objev.Eligibility_Check_Mode = data[12];
                objev.Created_By = ClientSession.UserName;
                objev.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
              //objev.Eligibility_Type = "Electronic";
                lstEv.Add(objev);

                objevman.SaveUpdateDeleteWithTransaction(ref lstEv, null, null, string.Empty);
                return "Success";

            }
            catch (Exception exception)
            {
                var lstFinalResult = new
                {
                    Error = "The following error occurred :" + exception.Message + ". Please contact support."
                };
                return JsonConvert.SerializeObject(lstFinalResult);
            }
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
      
        public static string LoadEVshowall(string HumanID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            try
            {


                Eligibility_VerficationManager objEV = new Eligibility_VerficationManager();

                ArrayList AuthDetails = new ArrayList();
                IList<AuthorizationEncounter> lstauthenc = new List<AuthorizationEncounter>();


                AuthDetails = objEV.GetEvDetailsshowall(Convert.ToUInt64(HumanID));


                var result = new { EV = AuthDetails };
                return JsonConvert.SerializeObject(result);

            }
            catch (Exception exception)
            {
                var lstFinalResult = new
                {
                    Error = "The following error occurred :" + exception.Message + ". Please contact support."
                };
                return JsonConvert.SerializeObject(lstFinalResult);
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        //data : HumanID, CarrierID, InsurancePlanID,PolicyHolderID
      
            public static string GetPerformEVDetails(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            try
            {
                ArrayList EvDetails = new ArrayList();

                IList<Human> lstHuman = new List<Human>();
                IList<Human> lstInsHuman = new List<Human>(); IList<string> ilstGeneralPlanTagList = new List<string>();
                ilstGeneralPlanTagList.Add("HumanList");
                IList<Human> lsthuman = new List<Human>();
                Human objFillHuman = new Human();
                IList<object> ilstGeneralPlanBlobFinal = new List<object>();
                ilstGeneralPlanBlobFinal = UtilityManager.ReadBlob(Convert.ToUInt64(data[0]), ilstGeneralPlanTagList);
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
                //Get Details from Xml
                //string FileName = "Human" + "_" + data[0] + ".xml";
               // string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                //if (File.Exists(strXmlFilePath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);

                //        XmlNodeList xmlhumanList = itemDoc.GetElementsByTagName("Human");
                //        Human objFillHuman = new Human();
                //        if (xmlhumanList != null && xmlhumanList.Count > 0)
                //        {
                //            objFillHuman.Id = Convert.ToUInt64(xmlhumanList[0].Attributes.GetNamedItem("Id").Value);
                //            objFillHuman.Birth_Date = Convert.ToDateTime(xmlhumanList[0].Attributes.GetNamedItem("Birth_Date").Value);
                //            objFillHuman.First_Name = xmlhumanList[0].Attributes.GetNamedItem("First_Name").Value;
                //            objFillHuman.Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Last_Name").Value;
                //            objFillHuman.MI = xmlhumanList[0].Attributes.GetNamedItem("MI").Value;
                //            objFillHuman.Sex = xmlhumanList[0].Attributes.GetNamedItem("Sex").Value;
                //            objFillHuman.Suffix = xmlhumanList[0].Attributes.GetNamedItem("Suffix").Value;
                //            objFillHuman.Street_Address1 = xmlhumanList[0].Attributes.GetNamedItem("Street_Address1").Value;
                //            objFillHuman.City = xmlhumanList[0].Attributes.GetNamedItem("City").Value;
                //            objFillHuman.State = xmlhumanList[0].Attributes.GetNamedItem("State").Value;
                //            objFillHuman.ZipCode = xmlhumanList[0].Attributes.GetNamedItem("ZipCode").Value;
                //            if (xmlhumanList[0].Attributes.GetNamedItem("Birth_Order") != null)
                //                objFillHuman.Birth_Order = xmlhumanList[0].Attributes.GetNamedItem("Birth_Order").Value;
                //            lstHuman.Add(objFillHuman);
                //        }
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}

                Carrier carrierList = new Carrier();
                CarrierManager carrierMngr = new CarrierManager();
                //Carrier ID to be passed
                if (data[1] != null && data[1] != "")
                    carrierList = carrierMngr.GetCarrierUsingId(Convert.ToUInt64(data[1]));

                IList<PatientInsuredPlan> patInsList = new List<PatientInsuredPlan>();
                PatientInsuredPlanManager patInsMngr = new PatientInsuredPlanManager();
                //For Bug ID 61241
                // patInsList = patInsMngr.getInsurancePoliciesByHumanId(Convert.ToUInt64(data[0]));
                if (data[3] != null && data[3] != "")
                    patInsList = patInsMngr.getInsurancePoliciesByPolicyHolderId(Convert.ToUInt64(data[0]), data[3].Trim());

                //Selected Insruance ID to be passed
                string Insured_human_id = "";
                if (data[2] != null && data[2] != "" && patInsList.Count()>0)
                {
                    var pat = from p in patInsList where p.Insurance_Plan_ID == Convert.ToUInt64(data[2]) select p;
                    Insured_human_id = pat.ToList<PatientInsuredPlan>()[0].Insured_Human_ID.ToString();
                }
              
               lsthuman = new List<Human>();
                 objFillHuman = new Human();
                ilstGeneralPlanBlobFinal = new List<object>();
                ilstGeneralPlanBlobFinal = UtilityManager.ReadBlob(Convert.ToUInt64(Insured_human_id), ilstGeneralPlanTagList);
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
                //Get insured human details
                //string FileNameIns = "Human" + "_" + Insured_human_id + ".xml";
                //string strXmlFilePathIns = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileNameIns);
                //if (File.Exists(strXmlFilePathIns) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    using (FileStream fs = new FileStream(strXmlFilePathIns, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);
                //        XmlNodeList xmlhumanList = itemDoc.GetElementsByTagName("Human");
                //        Human objInsFillHuman = new Human();
                //        if (xmlhumanList != null && xmlhumanList.Count > 0)
                //        {
                //            objInsFillHuman.Id = Convert.ToUInt64(xmlhumanList[0].Attributes.GetNamedItem("Id").Value);
                //            objInsFillHuman.Birth_Date = Convert.ToDateTime(xmlhumanList[0].Attributes.GetNamedItem("Birth_Date").Value);
                //            objInsFillHuman.First_Name = xmlhumanList[0].Attributes.GetNamedItem("First_Name").Value;
                //            objInsFillHuman.Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Last_Name").Value;
                //            objInsFillHuman.MI = xmlhumanList[0].Attributes.GetNamedItem("MI").Value;
                //            objInsFillHuman.Sex = xmlhumanList[0].Attributes.GetNamedItem("Sex").Value;
                //            objInsFillHuman.Suffix = xmlhumanList[0].Attributes.GetNamedItem("Suffix").Value;
                //            objInsFillHuman.Street_Address1 = xmlhumanList[0].Attributes.GetNamedItem("Street_Address1").Value;
                //            objInsFillHuman.City = xmlhumanList[0].Attributes.GetNamedItem("City").Value;
                //            objInsFillHuman.State = xmlhumanList[0].Attributes.GetNamedItem("State").Value;
                //            objInsFillHuman.ZipCode = xmlhumanList[0].Attributes.GetNamedItem("ZipCode").Value;
                //            lstInsHuman.Add(objInsFillHuman);
                //        }
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}

                IList<InsurancePlan> InsurancePlanList = new List<InsurancePlan>();
                InsurancePlanManager InsurancePlanMngr = new InsurancePlanManager();
                //Ins ID to be passed
                if (data[2] != null && data[2] != "")
                    InsurancePlanList = InsurancePlanMngr.GetInsurancebyID(Convert.ToUInt64(data[2]));
                //string FileName = "Human" + "_" + data[0] + ".xml";
                 //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

                //ProviderAddressDetails ConfigXML
                string sProjectName = System.Configuration.ConfigurationSettings.AppSettings["EVProjectName"].ToString();
                PhysicianLibrary objPhyLib = new PhysicianLibrary();
                if (sProjectName != null && sProjectName.ToUpper() == "CMG")
                {
                    objPhyLib = new PhysicianLibrary();
                }
                else if (sProjectName != null && sProjectName.ToUpper() == "NSI")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
                    //if (File.Exists(strXmlFilePath) == true)
                    {
                        xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                        XmlNode nodeMatchingPhysicianAddress = xmldoc.SelectSingleNode("/PhysicianAddress/p" + "230");//Dr mesiwala ID
                        if (nodeMatchingPhysicianAddress != null)
                        {
                            objPhyLib.PhyFirstName = nodeMatchingPhysicianAddress.Attributes["Physician_First_Name"].Value.ToString();
                            objPhyLib.PhyMiddleName = nodeMatchingPhysicianAddress.Attributes["Physician_Middle_Name"].Value.ToString();
                            objPhyLib.PhySuffix = nodeMatchingPhysicianAddress.Attributes["Physician_Suffix"].Value.ToString();
                        }

                    }
                } string sUserName = string.Empty;
                sUserName = ClientSession.UserName;
                var result = new { HumanDetails = lstHuman, InsuredHumanDetails = lstInsHuman, InsDetails = InsurancePlanList, PhysicianDetails = objPhyLib, Payerid_NAIC_ID = carrierList.NAIC_ID.ToString(), InsuredRelationship = patInsList[0].Relationship, ProjectName = sProjectName, PolicyHolderID = patInsList[0].Policy_Holder_ID, CurrentUserName = sUserName };
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception exception)
            {
                var lstFinalResult = new
                {
                    Error = "The following error occurred :" + exception.Message + ". Please contact support."
                };
                return JsonConvert.SerializeObject(lstFinalResult);
            }
        }
    }
}
