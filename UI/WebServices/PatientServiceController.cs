using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.Core.DTOJson;
using Acurus.Capella.DataAccess;
using Acurus.Capella.DataAccess.ManagerObjects;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.UI.MobileControls;
using static QRCoder.PayloadGenerator;

namespace Acurus.Capella.UI.WebServices.API
{
    public class PatientServiceController : ApiController
    {
        [HttpPost]
        public IHttpActionResult SavePatientData(Human_Akido objHuman)
        {
            ulong uHumanId = 0;
            string created_By = "Acurus API";
            DateTime localTime = DateTime.Now;
            try
            {
                if (!VerifyToken())
                {
                    return Json(new { status = "Unauthorized", ErrorDescription = "The remote server returned an error: (403) Forbidden." });
                }

                string errorMsg = Validation(objHuman);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return Json(new { HumanID = 0, status = "ValidationError", ErrorDescription = errorMsg });
                }

                HumanManager humanManager = new HumanManager();
                var CheckHuman = humanManager.GetPatientDetailsUsingPatientDetails(objHuman.Last_Name, objHuman.First_Name, objHuman.Birth_Date, objHuman.Sex, objHuman.Medical_Record_Number, objHuman.Patient_Account_External, objHuman.Legal_Org);

                if (CheckHuman?.HumanDetails?.Id != null && CheckHuman?.HumanDetails?.Id > 0)
                {
                    List<CDCDuplicateHumanTracker> listCDCDuplicateHumanTracker = new List<CDCDuplicateHumanTracker>();
                    listCDCDuplicateHumanTracker.Add(new CDCDuplicateHumanTracker()
                    {
                        Human_ID = 0,
                        Error_Description = "Duplicate patient exists.",
                        Status = "Error",
                        Created_By = created_By,
                        Created_Date_And_Time = localTime
                    });
                    CDCDuplicateHumanTrackerManager cDCDuplicateHumanTrackerManager = new CDCDuplicateHumanTrackerManager();
                    cDCDuplicateHumanTrackerManager.SaveCDCDuplicateHumanTrackerWithTransaction(listCDCDuplicateHumanTracker, string.Empty);

                    return Json(new { HumanID = 0, status = "ValidationError", ErrorDescription = "Duplicate patient exists." });
                }

                PatGuarantor objPatguarantor = new PatGuarantor();
                objPatguarantor.Active = "YES";
                objPatguarantor.Created_By = created_By;
                objPatguarantor.Created_Date_And_Time = localTime;
                objPatguarantor.From_Date = localTime;
                objPatguarantor.Relationship = objHuman.Guarantor_Relationship;
                if (!string.IsNullOrEmpty(objHuman.Guarantor_Relationship))
                {
                    StaticLookupManager staticLookUpMngr = new StaticLookupManager();
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RELATIONSHIP");
                    var filterData = staticlookuplist.FirstOrDefault(a => a.Value.ToLower().Trim() == objHuman.Guarantor_Relationship.ToLower().Trim());
                    if (filterData != null && !string.IsNullOrEmpty(filterData.Description))
                    {
                        objPatguarantor.Relationship_No = Convert.ToInt16(filterData.Description);
                    }
                }

                if (objHuman.ZipCode.Length == 9)
                {
                    objHuman.ZipCode = $"{objHuman.ZipCode.Substring(0, 5)}-{objHuman.ZipCode.Substring(5, 4)}";
                }

                objHuman.Cell_Phone_Number = FormatPhone(objHuman.Cell_Phone_Number);

                if (objHuman.Care_Giver_Phone_Number?.Length == 10)
                {
                    objHuman.Care_Giver_Phone_Number = FormatPhone(objHuman.Care_Giver_Phone_Number);
                }
                if (objHuman.Guarantor_CellPhone_Number?.Length == 10)
                {
                    objHuman.Guarantor_CellPhone_Number = FormatPhone(objHuman.Guarantor_CellPhone_Number);
                }
                if (objHuman.Guarantor_Home_Phone_Number?.Length == 10)
                {
                    objHuman.Guarantor_Home_Phone_Number = FormatPhone(objHuman.Guarantor_Home_Phone_Number);
                }
                if (objHuman.Home_Phone_No?.Length == 10)
                {
                    objHuman.Home_Phone_No = FormatPhone(objHuman.Home_Phone_No);
                }
                if (objHuman.Work_Phone_Ext?.Length == 10)
                {
                    objHuman.Work_Phone_Ext = FormatPhone(objHuman.Work_Phone_Ext);
                }
                if (objHuman.Work_Phone_No?.Length == 10)
                {
                    objHuman.Work_Phone_No = FormatPhone(objHuman.Work_Phone_No);
                }

                Human human = new Human()
                {
                    Legal_Org = objHuman.Legal_Org,
                    Previous_Name = objHuman.Previous_Name,
                    Prefix = objHuman.Prefix,
                    Last_Name = objHuman.Last_Name,
                    First_Name = objHuman.First_Name,
                    MI = objHuman.MI,
                    Suffix = objHuman.Suffix,
                    Birth_Date = objHuman.Birth_Date,
                    Sex = objHuman.Sex.ToUpper(),
                    Street_Address1 = objHuman.Street_Address1,
                    Street_Address2 = objHuman.Street_Address2,
                    City = objHuman.City,
                    State = objHuman.State,
                    ZipCode = objHuman.ZipCode,
                    Cell_Phone_Number = objHuman.Cell_Phone_Number,
                    Home_Phone_No = objHuman.Home_Phone_No,
                    Work_Phone_No = objHuman.Work_Phone_No,
                    Work_Phone_Ext = objHuman.Work_Phone_Ext,
                    SSN = objHuman.SSN,
                    Medical_Record_Number = objHuman.Medical_Record_Number,
                    Account_Status = objHuman.Account_Status,
                    EMail = objHuman.EMail,
                    Marital_Status = objHuman.Marital_Status,
                    Employer_Name = objHuman.Employer_Name,
                    Patient_Account_External = objHuman.Patient_Account_External,
                    Fax_Number = objHuman.Fax_Number,
                    Patient_Status = objHuman.Patient_Status,
                    Date_Of_Death = objHuman.Date_Of_Death,
                    Reason_For_Death = objHuman.Reason_For_Death,
                    Guarantor_Relationship = objHuman.Guarantor_Relationship,
                    Guarantor_Is_Patient = objHuman.Guarantor_Is_Patient.ToUpper(),
                    Care_Giver_Relation = objHuman.Care_Giver_Relation,
                    Care_Giver_First_Name = objHuman.Care_Giver_First_Name,
                    Care_Giver_Last_Name = objHuman.Care_Giver_Last_Name,
                    Care_Giver_Phone_Number = objHuman.Care_Giver_Phone_Number,
                    Created_By = created_By,
                    Created_Date_And_Time = localTime,
                    Gender_Identity = objHuman.Sex.ToUpper() //TODO: When Gender_Identity is provided as input, a priority should be set.
                };

                if (human.Guarantor_Is_Patient.ToUpper() == "Y")
                {
                    human.Guarantor_Last_Name = human.Last_Name;
                    human.Guarantor_First_Name = human.First_Name;
                    human.Guarantor_MI = human.MI;
                    human.Guarantor_Birth_Date = human.Birth_Date;
                    human.Guarantor_Street_Address1 = human.Street_Address1;
                    human.Guarantor_Street_Address2 = human.Street_Address2;
                    human.Guarantor_City = human.City;
                    human.Guarantor_Sex = human.Sex;
                    human.Guarantor_State = human.State;
                    human.Guarantor_Zip_Code = human.ZipCode;
                    human.Guarantor_Home_Phone_Number = human.Home_Phone_No;
                    human.Gaurantor_Email = human.EMail;
                    human.Guarantor_CellPhone_Number = human.Cell_Phone_Number;
                }
                else
                {
                    human.Guarantor_Last_Name = objHuman.Guarantor_Last_Name;
                    human.Guarantor_First_Name = objHuman.Guarantor_First_Name;
                    human.Guarantor_MI = objHuman.Guarantor_MI;
                    human.Guarantor_Birth_Date = objHuman.Guarantor_Birth_Date;
                    human.Guarantor_Street_Address1 = objHuman.Guarantor_Street_Address1;
                    human.Guarantor_Street_Address2 = objHuman.Guarantor_Street_Address2;
                    human.Guarantor_City = objHuman.Guarantor_City;
                    human.Guarantor_Sex = objHuman.Guarantor_Sex?.ToUpper();
                    human.Guarantor_State = objHuman.Guarantor_State;
                    human.Guarantor_Zip_Code = objHuman.Guarantor_Zip_Code;
                    human.Guarantor_Home_Phone_Number = objHuman.Guarantor_Home_Phone_Number;
                    human.Gaurantor_Email = objHuman.Gaurantor_Email;
                    human.Guarantor_CellPhone_Number = objHuman.Guarantor_CellPhone_Number;
                }

                string sCarrier = string.Empty;
                var humanObj = humanManager.AppendBatchToHuman(human, objPatguarantor, sCarrier);

                uHumanId = humanObj.Id;
            }
            catch (Exception ex)
            {
                LogError(ex, "0");
                return Json(new { HumanID = 0, status = "Error", ErrorDescription = "Error in processing the request. " + ex.Message });
            }
            return Json(new { HumanID = uHumanId, status = "Success" });
        }

        [HttpPut]
        public IHttpActionResult UpdatePatientData(UpdateHuman_Akido objUpdateHuman)
        {
            ulong uHumanId = 0;
            DateTime localTime = DateTime.Now;
            string modified_By = "Acurus API";
            try
            {
                if (!VerifyToken())
                {
                    return Json(new { status = "Unauthorized", ErrorDescription = "The remote server returned an error: (403) Forbidden." });
                }

                if (objUpdateHuman.humanID == 0)
                {
                    return Json(new { HumanID = 0, status = "ValidationError", ErrorDescription = "HumanID is not present in the request." });
                }

                string errorMsg = ValidationForUpdate(objUpdateHuman.human_data);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return Json(new { HumanID = objUpdateHuman.humanID, status = "ValidationError", ErrorDescription = errorMsg });
                }

                HumanManager humanManager = new HumanManager();
                Human humanData = humanManager.GetById(objUpdateHuman.humanID);

                if (humanData == null || humanData.Id == 0)
                {
                    return Json(new { HumanID = objUpdateHuman.humanID, status = "ValidationError", ErrorDescription = "HumanID is invalid." });
                }

                if (objUpdateHuman.human_data.ContainsKey("Medical_Record_Number")
                    && objUpdateHuman.human_data.ContainsKey("Patient_Account_External"))
                {
                    string medical_Record_Number = objUpdateHuman.human_data["Medical_Record_Number"]?.ToString();
                    string patient_Account_External = objUpdateHuman.human_data["Patient_Account_External"]?.ToString();

                    HumanDTO CheckHuman = new HumanDTO();
                    if (medical_Record_Number.ToUpper() != humanData.Medical_Record_Number.ToUpper()
                        && patient_Account_External.ToUpper() != humanData.Patient_Account_External.ToUpper())
                    {
                        CheckHuman = humanManager.GetPatientDetailsUsingPatientDetails(string.Empty, string.Empty, DateTime.MinValue, string.Empty, medical_Record_Number, patient_Account_External, ClientSession.LegalOrg);
                    }
                    if (medical_Record_Number.ToUpper() != humanData.Medical_Record_Number.ToUpper() && CheckHuman.MedicalRecordNoList == true)
                    {
                        return Json(new { HumanID = humanData.Id, status = "ValidationError", ErrorDescription = "Medical Record # already exists." });
                    }
                    if (patient_Account_External.ToUpper() != humanData.Patient_Account_External.ToUpper() && CheckHuman.Patient_Account_External == true)
                    {
                        return Json(new { HumanID = humanData.Id, status = "ValidationError", ErrorDescription = "External Account # already exists." });
                    }
                }

                foreach (var item in objUpdateHuman.human_data)
                {
                    UpdateProperty(humanData, item.Key, item.Value);
                }

                PatGuarantor objPatguarantor = new PatGuarantor();
                int iUpdateGuarantorID = Convert.ToInt32(objUpdateHuman.humanID);
                if (iUpdateGuarantorID != 0)
                {
                    PatGuarantorManager patGuarantorMngr = new PatGuarantorManager();
                    IList<PatGuarantor> patguarantorlist = new List<PatGuarantor>();
                    patguarantorlist = patGuarantorMngr.GetPatGuarantorDetails(Convert.ToInt32(objUpdateHuman.humanID), iUpdateGuarantorID);
                    if (patguarantorlist != null && patguarantorlist.Count > 0) //code added by balaji
                    {
                        objPatguarantor = patguarantorlist[0];
                        objPatguarantor.Human_ID = Convert.ToInt32(objUpdateHuman.humanID);
                        objPatguarantor.Active = "YES";
                        objPatguarantor.Modified_By = modified_By;
                        objPatguarantor.Modified_Date_And_Time = localTime;
                        objPatguarantor.From_Date = localTime.Date;
                        objPatguarantor.Guarantor_Human_ID = iUpdateGuarantorID;
                    }
                    else
                    {
                        objPatguarantor.Active = "YES";
                        objPatguarantor.Created_By = modified_By;
                        objPatguarantor.Created_Date_And_Time = localTime;
                        objPatguarantor.From_Date = localTime;
                    }
                }

                if (objUpdateHuman.human_data.ContainsKey("Guarantor_Relationship"))
                {
                    objPatguarantor.Relationship = humanData.Guarantor_Relationship;
                    if (!string.IsNullOrEmpty(humanData.Guarantor_Relationship))
                    {
                        StaticLookupManager staticLookUpMngr = new StaticLookupManager();
                        var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RELATIONSHIP");
                        var filterData = staticlookuplist.FirstOrDefault(a => a.Value.ToLower().Trim() == humanData.Guarantor_Relationship.ToLower().Trim());
                        if (filterData != null && !string.IsNullOrEmpty(filterData.Description))
                        {
                            objPatguarantor.Relationship_No = Convert.ToInt16(filterData.Description);
                        }
                    }
                }

                if (objUpdateHuman.human_data.ContainsKey("ZipCode") && humanData.ZipCode.Length == 9)
                {
                    humanData.ZipCode = $"{humanData.ZipCode.Substring(0, 5)}-{humanData.ZipCode.Substring(5, 4)}";
                }

                if (objUpdateHuman.human_data.ContainsKey("Cell_Phone_Number"))
                {
                    humanData.Cell_Phone_Number = FormatPhone(humanData.Cell_Phone_Number);
                }

                if (objUpdateHuman.human_data.ContainsKey("Care_Giver_Phone_Number") && humanData.Care_Giver_Phone_Number?.Length == 10)
                {
                    humanData.Care_Giver_Phone_Number = FormatPhone(humanData.Care_Giver_Phone_Number);
                }
                if (objUpdateHuman.human_data.ContainsKey("Guarantor_CellPhone_Number") && humanData.Guarantor_CellPhone_Number?.Length == 10)
                {
                    humanData.Guarantor_CellPhone_Number = FormatPhone(humanData.Guarantor_CellPhone_Number);
                }
                if (objUpdateHuman.human_data.ContainsKey("Guarantor_Home_Phone_Number") && humanData.Guarantor_Home_Phone_Number?.Length == 10)
                {
                    humanData.Guarantor_Home_Phone_Number = FormatPhone(humanData.Guarantor_Home_Phone_Number);
                }
                if (objUpdateHuman.human_data.ContainsKey("Home_Phone_No") && humanData.Home_Phone_No?.Length == 10)
                {
                    humanData.Home_Phone_No = FormatPhone(humanData.Home_Phone_No);
                }
                if (objUpdateHuman.human_data.ContainsKey("Work_Phone_Ext") && humanData.Work_Phone_Ext?.Length == 10)
                {
                    humanData.Work_Phone_Ext = FormatPhone(humanData.Work_Phone_Ext);
                }
                if (objUpdateHuman.human_data.ContainsKey("Work_Phone_No") && humanData.Work_Phone_No?.Length == 10)
                {
                    humanData.Work_Phone_No = FormatPhone(humanData.Work_Phone_No);
                }

                humanData.Modified_By = modified_By;
                humanData.Modified_Date_And_Time = localTime;

                string sCarrier = string.Empty;
                PatientInsuredPlan objPatInsPLan = null;
                var humanObj = humanManager.UpdateBatchToHuman(humanData, objPatguarantor, objPatInsPLan, sCarrier);

                uHumanId = humanObj.Id;
            }
            catch (Exception ex)
            {
                LogError(ex, "0");
                return Json(new { HumanID = 0, status = "Error", ErrorDescription = "Error in processing the request. " + ex.Message });
            }
            return Json(new { HumanID = uHumanId, status = "Success" });
        }

        public string Validation(Human_Akido objHuman)
        {
            DateTime localTime = DateTime.Now;
            StaticLookupManager staticLookUpMngr = new StaticLookupManager();
            if (string.IsNullOrEmpty(objHuman.Legal_Org))
            {
                return "Legal_Org is not present in the request.";
            }
            else
            {
                //var stringList = new List<string>() { "CMG", "WC", "RJ" };
                var stringList = new List<string>() { "CMG" };
                if (!stringList.Any(a => a.Equals(objHuman.Legal_Org.Trim().ToUpper())))
                    return "Legal_Org is invalid in the request.";
            }
            if (string.IsNullOrEmpty(objHuman.Last_Name))
            {
                return "Last_Name is not present in the request.";
            }
            if (string.IsNullOrEmpty(objHuman.First_Name))
            {
                return "First_Name is not present in the request.";
            }
            if (objHuman.Birth_Date == null || objHuman.Birth_Date == DateTime.MinValue)
            {
                return "Birth_Date is not present in the request.";
            }
            else if (ValidateDateFormate(objHuman.Birth_Date.ToString()))
            {
                return "Birth_Date is invalid in the request.";
            }
            else if (objHuman.Birth_Date > localTime)
            {
                return "Birth_Date can not be a future date.";
            }
            if (string.IsNullOrEmpty(objHuman.Sex))
            {
                return "Sex is not present in the request.";
            }
            else
            {
                var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("SEX");
                if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Sex.ToLower().Trim()))
                {
                    return "Sex is invalid in the request.";
                }
            }
            if (string.IsNullOrEmpty(objHuman.Street_Address1))
            {
                return "Street_Address1 is not present in the request.";
            }
            if (string.IsNullOrEmpty(objHuman.City))
            {
                return "City is not present in the request.";
            }
            if (string.IsNullOrEmpty(objHuman.State))
            {
                return "State is not present in the request.";
            }
            else
            {
                StateManager StateMngr = new StateManager();
                var statelist = StateMngr.Getstate();
                if (!statelist.Any(a => a.State_Code.ToLower().Trim() == objHuman.State.ToLower().Trim()))
                {
                    return "State is invalid in the request.";
                }
            }
            if (string.IsNullOrEmpty(objHuman.ZipCode))
            {
                return "ZipCode is not present in the request.";
            }
            if (objHuman.ZipCode.Length != 5 && objHuman.ZipCode.Length != 9 || !Regex.IsMatch(objHuman.ZipCode, @"^\d+$"))
            {
                return "ZipCode is invalid in the request.";
            }
            if (string.IsNullOrEmpty(objHuman.Cell_Phone_Number))
            {
                return "Cell_Phone_Number is not present in the request.";
            }
            if (objHuman.Cell_Phone_Number.Length != 10 || !Regex.IsMatch(objHuman.Cell_Phone_Number, @"^\d+$"))
            {
                return "Cell_Phone_Number is invalid in the request.";
            }

            if (!string.IsNullOrEmpty(objHuman.Home_Phone_No)
                && (objHuman.Home_Phone_No.Length != 10
                || !Regex.IsMatch(objHuman.Home_Phone_No, @"^\d+$")))
            {
                return "Home_Phone_No is invalid in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Work_Phone_No)
                && (objHuman.Work_Phone_No.Length != 10
                || !Regex.IsMatch(objHuman.Work_Phone_No, @"^\d+$")))
            {
                return "Work_Phone_No is invalid in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Guarantor_Home_Phone_Number)
                && (objHuman.Guarantor_Home_Phone_Number.Length != 10
                || !Regex.IsMatch(objHuman.Guarantor_Home_Phone_Number, @"^\d+$")))
            {
                return "Guarantor_Home_Phone_Number is invalid in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Care_Giver_Phone_Number)
                && (objHuman.Care_Giver_Phone_Number.Length != 10
                || !Regex.IsMatch(objHuman.Care_Giver_Phone_Number, @"^\d+$")))
            {
                return "Care_Giver_Phone_Number is invalid in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Fax_Number)
                && !Regex.IsMatch(objHuman.Fax_Number, @"^\d+$"))
            {
                return "Fax_Number is invalid in the request.";
            }

            if (!string.IsNullOrEmpty(objHuman.Suffix))
            {
                var allowedSuffixList = new List<string>() { "JR", "SR" };
                if (!allowedSuffixList.Any(a => a.Equals(objHuman.Suffix.Trim().ToUpper())))
                    return "Suffix is invalid in the request.";
            }

            if (string.IsNullOrEmpty(objHuman.Account_Status))
            {
                return "Account_Status is not present in the request.";
            }
            else
            {
                var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("ACCOUNT STATUS");
                if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Account_Status.ToLower().Trim()))
                {
                    return "Account_Status is invalid in the request.";
                }
            }
            if (!string.IsNullOrEmpty(objHuman.Marital_Status))
            {
                var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("MARITAL STATUS DEMOGRAPHICS");
                if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Marital_Status.ToLower().Trim()))
                {
                    return "Marital_Status is invalid in the request.";
                }
            }
            if (string.IsNullOrEmpty(objHuman.Patient_Status))
            {
                return "Patient_Status is not present in the request.";
            }
            else if (!string.IsNullOrEmpty(objHuman.Patient_Status))
            {
                var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PATIENT STATUS DEMOGRAPHICS");
                if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Patient_Status.ToLower().Trim()))
                {
                    return "Patient_Status is invalid in the request.";
                }
            }
            if (!string.IsNullOrEmpty(objHuman.Reason_For_Death))
            {
                var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("REASON FOR DEATH DEMOGRAPHICS");
                if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Reason_For_Death.ToLower().Trim()))
                {
                    return "Reason_For_Death is invalid in the request.";
                }
            }
            if (string.IsNullOrEmpty(objHuman.Guarantor_Is_Patient))
            {
                return "Guarantor_Is_Patient is not present in the request.";
            }
            else if (objHuman.Guarantor_Is_Patient != "Y" && objHuman.Guarantor_Is_Patient != "N")
            {
                return "Guarantor_Is_Patient is invalid in the request.";
            }

            if (objHuman.Guarantor_Is_Patient.ToUpper() == "N")
            {
                if (string.IsNullOrEmpty(objHuman.Guarantor_Last_Name))
                {
                    return "Guarantor_Last_Name is not present in the request.";
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_First_Name))
                {
                    return "Guarantor_First_Name is not present in the request.";
                }
                if (objHuman.Guarantor_Birth_Date == null || objHuman.Guarantor_Birth_Date == DateTime.MinValue)
                {
                    return "Guarantor_Birth_Date is not present in the request.";
                }
                else if (ValidateDateFormate(objHuman.Guarantor_Birth_Date.ToString()))
                {
                    return "Guarantor_Birth_Date is invalid in the request.";
                }
                else if (objHuman.Guarantor_Birth_Date > localTime)
                {
                    return "Guarantor_Birth_Date can not be a future date.";
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_Street_Address1))
                {
                    return "Guarantor_Street_Address1 is not present in the request.";
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_City))
                {
                    return "Guarantor_City is not present in the request.";
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_Sex))
                {
                    return "Guarantor_Sex is not present in the request.";
                }
                else
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("SEX");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Guarantor_Sex.ToLower().Trim()))
                    {
                        return "Guarantor_Sex is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_State))
                {
                    return "Guarantor_State is not present in the request.";
                }
                else
                {
                    StateManager StateMngr = new StateManager();
                    var statelist = StateMngr.Getstate();
                    if (!statelist.Any(a => a.State_Code.ToLower().Trim() == objHuman.Guarantor_State.ToLower().Trim()))
                    {
                        return "Guarantor_State is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_Zip_Code))
                {
                    return "Guarantor_Zip_Code is not present in the request.";
                }
                if (objHuman.Guarantor_Zip_Code.Length != 5 && objHuman.Guarantor_Zip_Code.Length != 9 || !Regex.IsMatch(objHuman.Guarantor_Zip_Code, @"^\d+$"))
                {
                    return "Guarantor_Zip_Code is invalid in the request.";
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_CellPhone_Number))
                {
                    return "Guarantor_CellPhone_Number is not present in the request.";
                }
                if (objHuman.Guarantor_CellPhone_Number.Length != 10 || !Regex.IsMatch(objHuman.Guarantor_CellPhone_Number, @"^\d+$"))
                {
                    return "Guarantor_CellPhone_Number is invalid in the request.";
                }
            }

            #region LengthValidation
            if (!string.IsNullOrEmpty(objHuman.Last_Name) && objHuman.Last_Name.Length > 35)
            {
                return "Last_Name exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.First_Name) && objHuman.First_Name.Length > 25)
            {
                return "First_Name exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.MI) && objHuman.MI.Length > 25)
            {
                return "MI exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Street_Address1) && objHuman.Street_Address1.Length > 55)
            {
                return "Street_Address1 exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Street_Address2) && objHuman.Street_Address2.Length > 55)
            {
                return "Street_Address2 exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.City) && objHuman.City.Length > 35)
            {
                return "City exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Work_Phone_Ext) && objHuman.Work_Phone_Ext.Length > 15)
            {
                return "Work_Phone_Ext exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.SSN) && objHuman.SSN.Length > 11)
            {
                return "SSN exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Medical_Record_Number) && objHuman.Medical_Record_Number.Length > 25)
            {
                return "Medical_Record_Number exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.EMail) && objHuman.EMail.Length > 100)
            {
                return "EMail exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Employer_Name) && objHuman.Employer_Name.Length > 100)
            {
                return "Employer_Name exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Patient_Account_External) && objHuman.Patient_Account_External.Length > 50)
            {
                return "Patient_Account_External exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Guarantor_Last_Name) && objHuman.Guarantor_Last_Name.Length > 35)
            {
                return "Guarantor_Last_Name exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Guarantor_First_Name) && objHuman.Guarantor_First_Name.Length > 25)
            {
                return "Guarantor_First_Name exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Guarantor_MI) && objHuman.Guarantor_MI.Length > 25)
            {
                return "Guarantor_MI exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Guarantor_Street_Address1) && objHuman.Guarantor_Street_Address1.Length > 55)
            {
                return "Guarantor_Street_Address1 exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Guarantor_Street_Address2) && objHuman.Guarantor_Street_Address2.Length > 55)
            {
                return "Guarantor_Street_Address2 exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Guarantor_City) && objHuman.Guarantor_City.Length > 35)
            {
                return "Guarantor_City exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Gaurantor_Email) && objHuman.Gaurantor_Email.Length > 50)
            {
                return "Gaurantor_Email exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Care_Giver_First_Name) && objHuman.Care_Giver_First_Name.Length > 35)
            {
                return "Care_Giver_First_Name exceeds the maximum length in the request.";
            }
            if (!string.IsNullOrEmpty(objHuman.Care_Giver_Last_Name) && objHuman.Care_Giver_Last_Name.Length > 35)
            {
                return "Care_Giver_Last_Name exceeds the maximum length in the request.";
            }
            #endregion

            if (string.IsNullOrEmpty(objHuman.Guarantor_Relationship))
            {
                return "Guarantor_Relationship is not present in the request.";
            }
            else
            {
                var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RELATIONSHIP");
                if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Guarantor_Relationship.ToLower().Trim()))
                {
                    return "Guarantor_Relationship is invalid in the request.";
                }
            }

            return "";
        }

        public string ValidationForUpdate(Dictionary<string, object> human_data)
        {
            DateTime localTime = DateTime.Now;
            Human_Akido objHuman = new Human_Akido();

            // 1. Map dynamic human_data into objHuman
            foreach (var item in human_data)
            {
                var prop = objHuman.GetType()
                    .GetProperties()
                    .FirstOrDefault(p => p.Name.Equals(item.Key, StringComparison.OrdinalIgnoreCase));

                if (prop != null)
                {
                    object convertedValue = null;

                    try
                    {
                        if (prop.PropertyType == typeof(DateTime))
                        {
                            convertedValue = Convert.ToDateTime(item.Value);
                        }
                        else
                        {
                            convertedValue = Convert.ChangeType(item.Value, prop.PropertyType);
                        }

                        prop.SetValue(objHuman, convertedValue);
                    }
                    catch
                    {
                        return $"{prop.Name} has invalid data format.";
                    }
                }
            }

            StaticLookupManager staticLookUpMngr = new StaticLookupManager();
            // 2. Validation only for fields included in human_data
            if (human_data.ContainsKey("Legal_Org") && string.IsNullOrEmpty(objHuman.Legal_Org))
                return "Legal_Org is not present in the request.";
            else if (human_data.ContainsKey("Legal_Org") && !string.IsNullOrEmpty(objHuman.Legal_Org))
            {
                //var stringList = new List<string>() { "CMG", "WC", "RJ" };
                var stringList = new List<string>() { "CMG" };
                if (!stringList.Any(a => a.Equals(objHuman.Legal_Org.Trim().ToUpper())))
                    return "Legal_Org is invalid in the request.";
            }

            if (human_data.ContainsKey("Last_Name") && string.IsNullOrEmpty(objHuman.Last_Name))
                return "Last_Name is not present in the request.";

            if (human_data.ContainsKey("First_Name") && string.IsNullOrEmpty(objHuman.First_Name))
                return "First_Name is not present in the request.";

            if (human_data.ContainsKey("Birth_Date"))
            {
                if (objHuman.Birth_Date == DateTime.MinValue)
                    return "Birth_Date is not present in the request.";
                if (ValidateDateFormate(objHuman.Birth_Date.ToString()))
                    return "Birth_Date is invalid in the request.";
                if (objHuman.Birth_Date > localTime)
                    return "Birth_Date can not be future date.";
            }

            if (human_data.ContainsKey("Sex"))
            {
                if (string.IsNullOrEmpty(objHuman.Sex))
                    return "Sex is not present in the request.";

                var sexList = staticLookUpMngr.getStaticLookupByFieldName("SEX");
                if (!sexList.Any(a => a.Value.Equals(objHuman.Sex, StringComparison.OrdinalIgnoreCase)))
                    return "Sex is invalid in the request.";
            }

            if (human_data.ContainsKey("Street_Address1") && string.IsNullOrEmpty(objHuman.Street_Address1))
                return "Street_Address1 is not present in the request.";

            if (human_data.ContainsKey("City") && string.IsNullOrEmpty(objHuman.City))
                return "City is not present in the request.";

            if (human_data.ContainsKey("State"))
            {
                if (string.IsNullOrEmpty(objHuman.State))
                    return "State is not present in the request.";

                var stateList = new StateManager().Getstate();
                if (!stateList.Any(a => a.State_Code.Equals(objHuman.State, StringComparison.OrdinalIgnoreCase)))
                    return "State is invalid in the request.";
            }

            if (human_data.ContainsKey("ZipCode"))
            {
                if (string.IsNullOrEmpty(objHuman.ZipCode))
                    return "ZipCode is not present in the request.";

                if (objHuman.ZipCode.Length != 5 && objHuman.ZipCode.Length != 9 || !Regex.IsMatch(objHuman.ZipCode, @"^\d+$"))
                    return "ZipCode is invalid in the request.";
            }

            if (human_data.ContainsKey("Suffix") && !string.IsNullOrEmpty(objHuman.Suffix))
            {
                var stringList = new List<string>() { "JR", "SR" };
                if (!stringList.Any(a => a.Equals(objHuman.Suffix.Trim().ToUpper())))
                    return "Suffix is invalid in the request.";
            }

            if (human_data.ContainsKey("Cell_Phone_Number"))
            {
                if (string.IsNullOrEmpty(objHuman.Cell_Phone_Number))
                    return "Cell_Phone_Number is not present in the request.";

                if (objHuman.Cell_Phone_Number.Length != 10 || !Regex.IsMatch(objHuman.Cell_Phone_Number, @"^\d+$"))
                    return "Cell_Phone_Number is invalid in the request.";
            }

            if (human_data.ContainsKey("Home_Phone_No")
                && !string.IsNullOrEmpty(objHuman.Home_Phone_No)
                && (objHuman.Home_Phone_No.Length != 10
                || !Regex.IsMatch(objHuman.Home_Phone_No, @"^\d+$")))
            {
                return "Home_Phone_No is invalid in the request.";
            }
            if (human_data.ContainsKey("Work_Phone_No")
                && !string.IsNullOrEmpty(objHuman.Work_Phone_No)
                && (objHuman.Work_Phone_No.Length != 10
                || !Regex.IsMatch(objHuman.Work_Phone_No, @"^\d+$")))
            {
                return "Work_Phone_No is invalid in the request.";
            }
            if (human_data.ContainsKey("Guarantor_Home_Phone_Number")
                && !string.IsNullOrEmpty(objHuman.Guarantor_Home_Phone_Number)
                && (objHuman.Guarantor_Home_Phone_Number.Length != 10
                || !Regex.IsMatch(objHuman.Guarantor_Home_Phone_Number, @"^\d+$")))
            {
                return "Guarantor_Home_Phone_Number is invalid in the request.";
            }
            if (human_data.ContainsKey("Care_Giver_Phone_Number")
                && !string.IsNullOrEmpty(objHuman.Care_Giver_Phone_Number)
                && (objHuman.Care_Giver_Phone_Number.Length != 10
                || !Regex.IsMatch(objHuman.Care_Giver_Phone_Number, @"^\d+$")))
            {
                return "Care_Giver_Phone_Number is invalid in the request.";
            }
            if (human_data.ContainsKey("Fax_Number")
                && !string.IsNullOrEmpty(objHuman.Fax_Number)
                && !Regex.IsMatch(objHuman.Fax_Number, @"^\d+$"))
            {
                return "Fax_Number is invalid in the request.";
            }

            if (human_data.ContainsKey("Account_Status"))
            {
                if (string.IsNullOrEmpty(objHuman.Account_Status))
                    return "Account_Status is not present in the request.";

                var lookup = staticLookUpMngr.getStaticLookupByFieldName("ACCOUNT STATUS");
                if (!lookup.Any(a => a.Value.Equals(objHuman.Account_Status, StringComparison.OrdinalIgnoreCase)))
                    return "Account_Status is invalid in the request.";
            }

            if (human_data.ContainsKey("Patient_Status"))
            {
                if (string.IsNullOrEmpty(objHuman.Patient_Status))
                    return "Patient_Status is not present in the request.";

                var lookup = staticLookUpMngr.getStaticLookupByFieldName("PATIENT STATUS DEMOGRAPHICS");
                if (!lookup.Any(a => a.Value.Equals(objHuman.Patient_Status, StringComparison.OrdinalIgnoreCase)))
                    return "Patient_Status is invalid in the request.";
            }

            if (human_data.ContainsKey("Marital_Status") && !string.IsNullOrEmpty(objHuman.Marital_Status))
            {
                var lookup = staticLookUpMngr.getStaticLookupByFieldName("MARITAL STATUS DEMOGRAPHICS");
                if (!lookup.Any(a => a.Value.Equals(objHuman.Marital_Status, StringComparison.OrdinalIgnoreCase)))
                    return "Marital_Status is invalid in the request.";
            }

            if (human_data.ContainsKey("Reason_For_Death") && !string.IsNullOrEmpty(objHuman.Reason_For_Death))
            {
                var lookup = staticLookUpMngr.getStaticLookupByFieldName("REASON FOR DEATH DEMOGRAPHICS");
                if (!lookup.Any(a => a.Value.Equals(objHuman.Reason_For_Death, StringComparison.OrdinalIgnoreCase)))
                    return "Reason_For_Death is invalid in the request.";
            }

            if (human_data.ContainsKey("Guarantor_Is_Patient") && string.IsNullOrEmpty(objHuman.Guarantor_Is_Patient))
                return "Guarantor_Is_Patient is not present in the request.";

            if (human_data.ContainsKey("Guarantor_Is_Patient") && (objHuman.Guarantor_Is_Patient.ToUpper() != "Y" && objHuman.Guarantor_Is_Patient.ToUpper() != "N"))
            {
                return "Guarantor_Is_Patient is invalid in the request.";
            }

            if (human_data.ContainsKey("Guarantor_Is_Patient") && objHuman.Guarantor_Is_Patient.ToUpper() == "N")
            {
                if (string.IsNullOrEmpty(objHuman.Guarantor_Last_Name))
                {
                    return "Guarantor_Last_Name is not present in the request.";
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_First_Name))
                {
                    return "Guarantor_First_Name is not present in the request.";
                }
                if (objHuman.Guarantor_Birth_Date == null || objHuman.Guarantor_Birth_Date == DateTime.MinValue)
                {
                    return "Guarantor_Birth_Date is not present in the request.";
                }
                else if (ValidateDateFormate(objHuman.Guarantor_Birth_Date.ToString()))
                {
                    return "Guarantor_Birth_Date is invalid in the request.";
                }
                else if (objHuman.Guarantor_Birth_Date > localTime)
                {
                    return "Guarantor_Birth_Date can not be a future date.";
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_Street_Address1))
                {
                    return "Guarantor_Street_Address1 is not present in the request.";
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_City))
                {
                    return "Guarantor_City is not present in the request.";
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_Sex))
                {
                    return "Guarantor_Sex is not present in the request.";
                }
                else
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("SEX");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Guarantor_Sex.ToLower().Trim()))
                    {
                        return "Guarantor_Sex is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_State))
                {
                    return "Guarantor_State is not present in the request.";
                }
                else
                {
                    StateManager StateMngr = new StateManager();
                    var statelist = StateMngr.Getstate();
                    if (!statelist.Any(a => a.State_Code.ToLower().Trim() == objHuman.Guarantor_State.ToLower().Trim()))
                    {
                        return "Guarantor_State is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_Zip_Code))
                {
                    return "Guarantor_Zip_Code is not present in the request.";
                }
                if (objHuman.Guarantor_Zip_Code.Length != 5 && objHuman.Guarantor_Zip_Code.Length != 9 || !Regex.IsMatch(objHuman.Guarantor_Zip_Code, @"^\d+$"))
                {
                    return "Guarantor_Zip_Code is invalid in the request.";
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_CellPhone_Number))
                {
                    return "Guarantor_CellPhone_Number is not present in the request.";
                }
                if (objHuman.Guarantor_CellPhone_Number.Length != 10 || !Regex.IsMatch(objHuman.Guarantor_CellPhone_Number, @"^\d+$"))
                {
                    return "Guarantor_CellPhone_Number is invalid in the request.";
                }
                if (string.IsNullOrEmpty(objHuman.Guarantor_Relationship))
                {
                    return "Guarantor_Relationship is not present in the request.";
                }
                else
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RELATIONSHIP");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Guarantor_Relationship.ToLower().Trim()))
                    {
                        return "Guarantor_Relationship is invalid in the request.";
                    }
                }
            }

            #region LengthValidation
            if (human_data.ContainsKey("Last_Name") && !string.IsNullOrEmpty(objHuman.Last_Name) && objHuman.Last_Name.Length > 35)
            {
                return "Last_Name exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("First_Name") && !string.IsNullOrEmpty(objHuman.First_Name) && objHuman.First_Name.Length > 25)
            {
                return "First_Name exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("MI") && !string.IsNullOrEmpty(objHuman.MI) && objHuman.MI.Length > 25)
            {
                return "MI exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Street_Address1") && !string.IsNullOrEmpty(objHuman.Street_Address1) && objHuman.Street_Address1.Length > 55)
            {
                return "Street_Address1 exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Street_Address2") && !string.IsNullOrEmpty(objHuman.Street_Address2) && objHuman.Street_Address2.Length > 55)
            {
                return "Street_Address2 exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("City") && !string.IsNullOrEmpty(objHuman.City) && objHuman.City.Length > 35)
            {
                return "City exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Work_Phone_Ext") && !string.IsNullOrEmpty(objHuman.Work_Phone_Ext) && objHuman.Work_Phone_Ext.Length > 15)
            {
                return "Work_Phone_Ext exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("SSN") && !string.IsNullOrEmpty(objHuman.SSN) && objHuman.SSN.Length > 11)
            {
                return "SSN exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Medical_Record_Number") && !string.IsNullOrEmpty(objHuman.Medical_Record_Number) && objHuman.Medical_Record_Number.Length > 25)
            {
                return "Medical_Record_Number exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("EMail") && !string.IsNullOrEmpty(objHuman.EMail) && objHuman.EMail.Length > 100)
            {
                return "EMail exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Employer_Name") && !string.IsNullOrEmpty(objHuman.Employer_Name) && objHuman.Employer_Name.Length > 100)
            {
                return "Employer_Name exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Patient_Account_External") && !string.IsNullOrEmpty(objHuman.Patient_Account_External) && objHuman.Patient_Account_External.Length > 50)
            {
                return "Patient_Account_External exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Guarantor_Last_Name") && !string.IsNullOrEmpty(objHuman.Guarantor_Last_Name) && objHuman.Guarantor_Last_Name.Length > 35)
            {
                return "Guarantor_Last_Name exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Guarantor_First_Name") && !string.IsNullOrEmpty(objHuman.Guarantor_First_Name) && objHuman.Guarantor_First_Name.Length > 25)
            {
                return "Guarantor_First_Name exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Guarantor_MI") && !string.IsNullOrEmpty(objHuman.Guarantor_MI) && objHuman.Guarantor_MI.Length > 25)
            {
                return "Guarantor_MI exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Guarantor_Street_Address1") && !string.IsNullOrEmpty(objHuman.Guarantor_Street_Address1) && objHuman.Guarantor_Street_Address1.Length > 55)
            {
                return "Guarantor_Street_Address1 exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Guarantor_Street_Address2") && !string.IsNullOrEmpty(objHuman.Guarantor_Street_Address2) && objHuman.Guarantor_Street_Address2.Length > 55)
            {
                return "Guarantor_Street_Address2 exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Guarantor_City") && !string.IsNullOrEmpty(objHuman.Guarantor_City) && objHuman.Guarantor_City.Length > 35)
            {
                return "Guarantor_City exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Gaurantor_Email") && !string.IsNullOrEmpty(objHuman.Gaurantor_Email) && objHuman.Gaurantor_Email.Length > 50)
            {
                return "Gaurantor_Email exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Care_Giver_First_Name") && !string.IsNullOrEmpty(objHuman.Care_Giver_First_Name) && objHuman.Care_Giver_First_Name.Length > 35)
            {
                return "Care_Giver_First_Name exceeds the maximum length in the request.";
            }
            if (human_data.ContainsKey("Care_Giver_Last_Name") && !string.IsNullOrEmpty(objHuman.Care_Giver_Last_Name) && objHuman.Care_Giver_Last_Name.Length > 35)
            {
                return "Care_Giver_Last_Name exceeds the maximum length in the request.";
            }
            #endregion

            return "";
        }

        public bool ValidateDateFormate(string stringDate)
        {
            return DateTime.TryParseExact(stringDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _);
        }

        private bool VerifyToken()
        {
            var authorization = Request.Headers.GetValues("Authorization");
            string token = authorization.Any() ? authorization.FirstOrDefault() : "";
            token = token.Replace("Bearer ", "");
            var endPointToken = ConfigurationSettings.AppSettings["EndPointToken"] ?? "";
            if (token == null || string.IsNullOrEmpty(token.ToString()) || string.IsNullOrEmpty(endPointToken) || token.ToString() != endPointToken)
            {
                return false;
            }
            return true;
        }

        private void LogError(Exception exc, string sHumanID)
        {
            string version = "";
            if (ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
            {
                version = ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();
            }

            string[] server = version.Split('|');
            string serverno = "";
            if (server.Length > 1)
                serverno = server[1].Trim();

            string sMessage = "";
            string statserrorlogstacktrace = "";

            if (exc != null && exc.Message != null)
                sMessage = exc.Message;

            if (exc != null && exc.StackTrace != null)
                statserrorlogstacktrace = exc.StackTrace;
            if (exc != null && exc.InnerException != null && exc.InnerException.Message != null && sMessage == string.Empty)
            {
                sMessage += exc.InnerException.Message;
            }
            if (exc != null && exc.InnerException != null && exc.InnerException.StackTrace != null && sMessage == string.Empty)
            {
                statserrorlogstacktrace += exc.InnerException.StackTrace;
            }

            if (exc != null && exc.Message != null)
            {
                string userName = string.Empty;
                ulong physicianId = 0;
                string insertQuery = "insert into stats_apperrorlog values(0,'" + sMessage.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + "', '" + serverno + "','" + DateTime.Now + "','" + userName + "','" + 0 + "','" + sHumanID + "','" + physicianId + "','" + statserrorlogstacktrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                DBConnector.WriteData(insertQuery);
            }
        }

        private string FormatPhone(string Phone_Number)
        {
            return $"({Phone_Number.Substring(0, 3)}) {Phone_Number.Substring(3, 3)}-{Phone_Number.Substring(6, 4)}";
        }

        private void UpdateProperty(object target, string propertyName, object value)
        {
            var prop = target.GetType()
                .GetProperties()
                .FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            if (prop != null && prop.CanWrite)
            {
                object convertedValue = Convert.ChangeType(value, prop.PropertyType);
                prop.SetValue(target, convertedValue);
            }
        }
    }

    public class Human_Akido
    {
        public virtual string Legal_Org { get; set; } = string.Empty;
        public virtual string Previous_Name { get; set; } = string.Empty;
        public virtual string Prefix { get; set; } = string.Empty;
        public virtual string Last_Name { get; set; } = string.Empty;
        public virtual string First_Name { get; set; } = string.Empty;
        public virtual string MI { get; set; } = string.Empty;
        public virtual string Suffix { get; set; } = string.Empty;
        public virtual DateTime Birth_Date { get; set; } = DateTime.MinValue;
        public virtual string Sex { get; set; } = string.Empty;
        public virtual string Street_Address1 { get; set; } = string.Empty;
        public virtual string Street_Address2 { get; set; } = string.Empty;
        public virtual string City { get; set; } = string.Empty;
        public virtual string State { get; set; } = string.Empty;
        public virtual string ZipCode { get; set; } = string.Empty;
        public virtual string Cell_Phone_Number { get; set; } = string.Empty;
        public virtual string Home_Phone_No { get; set; } = string.Empty;
        public virtual string Work_Phone_No { get; set; } = string.Empty;
        public virtual string Work_Phone_Ext { get; set; } = string.Empty;
        public virtual string SSN { get; set; } = string.Empty;
        public virtual string Medical_Record_Number { get; set; } = string.Empty;
        public virtual string Account_Status { get; set; } = string.Empty;
        public virtual string EMail { get; set; } = string.Empty;
        public virtual string Marital_Status { get; set; } = string.Empty;
        public virtual string Employer_Name { get; set; } = string.Empty;
        public virtual string Patient_Account_External { get; set; } = string.Empty;
        public virtual string Fax_Number { get; set; } = string.Empty;
        public virtual string Patient_Status { get; set; } = string.Empty;
        public virtual DateTime Date_Of_Death { get; set; } = DateTime.MinValue;
        public virtual string Reason_For_Death { get; set; } = string.Empty;
        public virtual string Guarantor_Last_Name { get; set; } = string.Empty;
        public virtual string Guarantor_First_Name { get; set; } = string.Empty;
        public virtual string Guarantor_MI { get; set; } = string.Empty;
        public virtual DateTime Guarantor_Birth_Date { get; set; } = DateTime.MinValue;
        public virtual string Guarantor_Street_Address1 { get; set; } = string.Empty;
        public virtual string Guarantor_Street_Address2 { get; set; } = string.Empty;
        public virtual string Guarantor_City { get; set; } = string.Empty;
        public virtual string Guarantor_Sex { get; set; } = string.Empty;
        public virtual string Guarantor_State { get; set; } = string.Empty;
        public virtual string Guarantor_Zip_Code { get; set; } = string.Empty;
        public virtual string Guarantor_Home_Phone_Number { get; set; } = string.Empty;
        public virtual string Guarantor_Is_Patient { get; set; } = string.Empty;
        public virtual string Gaurantor_Email { get; set; } = string.Empty;
        public virtual string Guarantor_CellPhone_Number { get; set; } = string.Empty;
        public virtual string Guarantor_Relationship { get; set; } = string.Empty;
        public virtual string Care_Giver_Relation { get; set; } = string.Empty;
        public virtual string Care_Giver_First_Name { get; set; } = string.Empty;
        public virtual string Care_Giver_Last_Name { get; set; } = string.Empty;
        public virtual string Care_Giver_Phone_Number { get; set; } = string.Empty;
    }

    public class UpdateHuman_Akido
    {
        public ulong humanID { get; set; }
        public Dictionary<string, object> human_data { get; set; }
    }
}