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
        public IHttpActionResult SavePatientData([FromBody] Human_Akido objHuman, [FromUri] string version)
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

                string errorMsg = Validation(objHuman, version);
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
                        Human_ID = CheckHuman?.HumanDetails?.Id ?? 0,
                        Error_Description = "Duplicate patient exists.",
                        Status = "Error",
                        Created_By = created_By,
                        Created_Date_And_Time = localTime
                    });
                    CDCDuplicateHumanTrackerManager cDCDuplicateHumanTrackerManager = new CDCDuplicateHumanTrackerManager();
                    cDCDuplicateHumanTrackerManager.SaveCDCDuplicateHumanTrackerWithTransaction(listCDCDuplicateHumanTracker, string.Empty);

                    return Json(new { HumanID = 0, DuplicatePatientID = CheckHuman?.HumanDetails?.Id, status = "ValidationError", ErrorDescription = "Duplicate patient exists." });
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

                #region V2
                if (version.ToUpper() == "V2")
                {
                    if (objHuman.Emergency_Cnt_ZipCode.Length == 9)
                    {
                        objHuman.Emergency_Cnt_ZipCode = $"{objHuman.Emergency_Cnt_ZipCode.Substring(0, 5)}-{objHuman.Emergency_Cnt_ZipCode.Substring(5, 4)}";
                    }
                    if (objHuman.Emergency_Cnt_Home_Phone_Number?.Length == 10)
                    {
                        objHuman.Emergency_Cnt_Home_Phone_Number = FormatPhone(objHuman.Emergency_Cnt_Home_Phone_Number);
                    }
                    if (objHuman.Emergency_Cnt_CellPhone_Number?.Length == 10)
                    {
                        objHuman.Emergency_Cnt_CellPhone_Number = FormatPhone(objHuman.Emergency_Cnt_CellPhone_Number);
                    }
                }
                #endregion

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
                };
                //CAP-4048
                if (string.IsNullOrEmpty(objHuman.Gender_Identity)
                    && !string.IsNullOrEmpty(objHuman.Sex)
                    && objHuman.Sex.ToUpper() != "UNKNOWN")
                {
                    human.Gender_Identity = objHuman.Sex;
                }
                else
                {
                    human.Gender_Identity = objHuman.Gender_Identity;
                }

                if (version.ToUpper() == "V2")
                {
                    human.Granularity = objHuman.Granularity;
                    human.Sexual_Orientation = objHuman.Sexual_Orientation;
                    human.Sexual_Orientation_Specify = objHuman.Sexual_Orientation_Specify;
                    human.Gender_Identity_Specify = objHuman.Gender_Identity_Specify;
                    human.SigOn_File = objHuman.SigOn_File;
                    human.Employment_Status = objHuman.Employment_Status;
                    human.Patient_Notes = objHuman.Patient_Notes;
                    human.Driver_State = objHuman.Driver_State;
                    human.Driver_License_Num = objHuman.Driver_License_Num;
                    human.Emergency_Cnt_Last_Name = objHuman.Emergency_Cnt_Last_Name;
                    human.Emergency_Cnt_First_Name = objHuman.Emergency_Cnt_First_Name;
                    human.Emergency_Cnt_MI = objHuman.Emergency_Cnt_MI;
                    human.Emergency_Cnt_StreetAddr1 = objHuman.Emergency_Cnt_StreetAddr1;
                    human.Emergency_Cnt_StreetAddr2 = objHuman.Emergency_Cnt_StreetAddr2;
                    human.Emergency_Cnt_City = objHuman.Emergency_Cnt_City;
                    human.Emergency_Cnt_Sex = objHuman.Emergency_Cnt_Sex;
                    human.Emergency_Cnt_State = objHuman.Emergency_Cnt_State;
                    human.Emergency_Cnt_ZipCode = objHuman.Emergency_Cnt_ZipCode;
                    human.Emergency_Cnt_Home_Phone_Number = objHuman.Emergency_Cnt_Home_Phone_Number;
                    human.Emergency_BirthDate = objHuman.Emergency_BirthDate;
                    human.Emergency_Cnt_CellPhone_Number = objHuman.Emergency_Cnt_CellPhone_Number;
                    human.Emer_Relation = objHuman.Emer_Relation;
                    human.Demo_Status = objHuman.Demo_Status;
                    human.People_In_Collection = objHuman.People_In_Collection;
                    human.Preferred_Language = objHuman.Preferred_Language;
                    human.Race = objHuman.Race;
                    human.Ethnicity = objHuman.Ethnicity;
                    human.Photo_Path = objHuman.Photo_Path;
                    human.Race_No = objHuman.Race_No;
                    human.Preferred_Confidential_Correspodence_Mode = objHuman.Preferred_Confidential_Correspodence_Mode;
                    human.Human_Type = objHuman.Human_Type;
                    human.Declared_Bankruptcy = objHuman.Declared_Bankruptcy;
                    human.PCP_Name = objHuman.PCP_Name;
                    human.PCP_NPI = objHuman.PCP_NPI;
                    human.Mothers_Maiden_Name = objHuman.Mothers_Maiden_Name;
                    human.Immunization_Registry_Status = objHuman.Immunization_Registry_Status;
                    human.Publicity_Code = objHuman.Publicity_Code;
                    human.Representative_Email = objHuman.Representative_Email;
                    human.Data_Sharing_Preference = objHuman.Data_Sharing_Preference;
                    human.Birth_Indicator = objHuman.Birth_Indicator;
                    human.Birth_Order = objHuman.Birth_Order;
                    human.Primary_Carrier_ID = objHuman.Primary_Carrier_ID;
                    human.Is_Translator_Required = objHuman.Is_Translator_Required;
                    human.Dynamics_Number = objHuman.Dynamics_Number;
                    human.Tribal_Affiliation = objHuman.Tribal_Affiliation;
                    human.Specific_Ethnicity = objHuman.Specific_Ethnicity;
                    human.Insurance_Status = objHuman.Insurance_Status;
                    human.Is_Sent_To_Rcopia = "Y";
                }

                if (!string.IsNullOrEmpty(human.Guarantor_Relationship))
                {
                    StaticLookupManager staticLookUpMngr = new StaticLookupManager();
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RELATIONSHIP");
                    var filterData = staticlookuplist.FirstOrDefault(a => a.Value.ToLower().Trim() == human.Guarantor_Relationship.ToLower().Trim());
                    if (filterData != null && !string.IsNullOrEmpty(filterData.Description))
                    {
                        human.Guarantor_Relationship_No = Convert.ToInt32(filterData.Description);
                    }
                }

                #region V2
                if (version.ToUpper() == "V2")
                {
                    if (!string.IsNullOrEmpty(objHuman.Ethnicity))
                    {
                        StaticLookupManager staticLookUpMngr = new StaticLookupManager();
                        var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("ETHNICITY");
                        var filterData = staticlookuplist.FirstOrDefault(a => a.Value.ToLower().Trim() == objHuman.Ethnicity.ToLower().Trim());
                        if (filterData != null && !string.IsNullOrEmpty(filterData.Description))
                        {
                            human.Ethnicity_No = Convert.ToInt32(filterData.Description);
                        }
                    }
                    if (!string.IsNullOrEmpty(objHuman.Race))
                    {
                        StaticLookupManager staticLookUpMngr = new StaticLookupManager();
                        var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RACE");
                        foreach (var item in objHuman.Race.Split(','))
                        {
                            var filterData = staticlookuplist.FirstOrDefault(a => a.Value.ToLower().Trim() == item.ToLower().Trim());
                            if (filterData != null && !string.IsNullOrEmpty(filterData.Description))
                            {
                                string[] Race = filterData.Description.Split('-');
                                if (Race.Count() == 2)
                                {
                                    human.Race_Alias += Race[0].ToString() + ",";
                                    human.Race_No = Race[1].ToString() + ",";
                                }
                            }
                        }
                        if (human.Race_Alias.Trim().EndsWith(","))
                        {
                            human.Race_Alias = human.Race_Alias.TrimEnd(',');
                        }
                        if (human.Race_No.Trim().EndsWith(","))
                        {
                            human.Race_No = human.Race_No.TrimEnd(',');
                        }
                    }
                }
                #endregion

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
        public IHttpActionResult UpdatePatientData([FromBody] UpdateHuman_Akido objUpdateHuman, [FromUri] string version)
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

                string errorMsg = ValidationForUpdate(objUpdateHuman.human_data, version);
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
                        CheckHuman = humanManager.GetPatientDetailsUsingPatientDetails(string.Empty, string.Empty, DateTime.MinValue, string.Empty, medical_Record_Number, patient_Account_External, humanData.Legal_Org);
                    }
                    if (medical_Record_Number.ToUpper() != humanData.Medical_Record_Number.ToUpper() && CheckHuman.MedicalRecordNoList == true)
                    {
                        return Json(new { HumanID = humanData.Id, DuplicatePatientID = CheckHuman?.ulMedicalRecordID, status = "ValidationError", ErrorDescription = "Medical Record # already exists." });
                    }
                    if (patient_Account_External.ToUpper() != humanData.Patient_Account_External.ToUpper() && CheckHuman.Patient_Account_External == true)
                    {
                        return Json(new { HumanID = humanData.Id, DuplicatePatientID = CheckHuman?.ulPatientAccountExternalID, status = "ValidationError", ErrorDescription = "External Account # already exists." });
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
                            humanData.Guarantor_Relationship_No = Convert.ToInt16(filterData.Description);
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

                #region V2
                if (version.ToUpper() == "V2")
                {
                    if (objUpdateHuman.human_data.ContainsKey("Emergency_Cnt_ZipCode") && humanData.Emergency_Cnt_ZipCode.Length == 9)
                    {
                        humanData.Emergency_Cnt_ZipCode = $"{humanData.Emergency_Cnt_ZipCode.Substring(0, 5)}-{humanData.Emergency_Cnt_ZipCode.Substring(5, 4)}";
                    }
                    if (objUpdateHuman.human_data.ContainsKey("Emergency_Cnt_Home_Phone_Number") && humanData.Emergency_Cnt_Home_Phone_Number?.Length == 10)
                    {
                        humanData.Emergency_Cnt_Home_Phone_Number = FormatPhone(humanData.Emergency_Cnt_Home_Phone_Number);
                    }
                    if (objUpdateHuman.human_data.ContainsKey("Emergency_Cnt_CellPhone_Number") && humanData.Emergency_Cnt_CellPhone_Number?.Length == 10)
                    {
                        humanData.Emergency_Cnt_CellPhone_Number = FormatPhone(humanData.Emergency_Cnt_CellPhone_Number);
                    }

                    if (objUpdateHuman.human_data.ContainsKey("Ethnicity") && !string.IsNullOrEmpty(humanData.Ethnicity))
                    {
                        StaticLookupManager staticLookUpMngr = new StaticLookupManager();
                        var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("ETHNICITY");
                        var filterData = staticlookuplist.FirstOrDefault(a => a.Value.ToLower().Trim() == humanData.Ethnicity.ToLower().Trim());
                        if (filterData != null && !string.IsNullOrEmpty(filterData.Description))
                        {
                            humanData.Ethnicity_No = Convert.ToInt32(filterData.Description);
                        }
                    }
                    if (objUpdateHuman.human_data.ContainsKey("Race") && !string.IsNullOrEmpty(humanData.Race))
                    {
                        StaticLookupManager staticLookUpMngr = new StaticLookupManager();
                        var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RACE");
                        foreach (var item in humanData.Race.Split(','))
                        {
                            var filterData = staticlookuplist.FirstOrDefault(a => a.Value.ToLower().Trim() == item.ToLower().Trim());
                            if (filterData != null && !string.IsNullOrEmpty(filterData.Description))
                            {
                                string[] Race = filterData.Description.Split('-');
                                if (Race.Count() == 2)
                                {
                                    humanData.Race_Alias += Race[0].ToString() + ",";
                                    humanData.Race_No = Race[1].ToString() + ",";
                                }
                            }
                        }
                        if (humanData.Race_Alias.Trim().EndsWith(","))
                        {
                            humanData.Race_Alias = humanData.Race_Alias.TrimEnd(',');
                        }
                        if (humanData.Race_No.Trim().EndsWith(","))
                        {
                            humanData.Race_No = humanData.Race_No.TrimEnd(',');
                        }
                    }
                }
                #endregion

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

        [HttpPost]
        public IHttpActionResult SaveInsuredPlanData([FromBody] PatientInsuredPlan_Akido insuredPlan_Akido)
        {
            ulong uPat_Insured_Plan_ID = 0;
            try
            {
                if (!VerifyToken())
                {
                    return Json(new { status = "Unauthorized", ErrorDescription = "The remote server returned an error: (403) Forbidden." });
                }

                string errorMsg = InsuredPlanValidation(insuredPlan_Akido);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return Json(new { Pat_Insured_Plan_ID = uPat_Insured_Plan_ID, status = "ValidationError", ErrorDescription = errorMsg });
                }

                PatientInsuredPlanManager objmanager = new PatientInsuredPlanManager();
                var updateInsurancePolicies = objmanager.getInsurancePoliciesByHumanId(insuredPlan_Akido.Human_ID);
                if (updateInsurancePolicies != null && updateInsurancePolicies.Any())
                {
                    updateInsurancePolicies.Where(a => a.Insurance_Type.ToUpper() == insuredPlan_Akido.Insurance_Type.ToUpper()).Select(a => a.Insurance_Type = string.Format("OLD {0}", a.Insurance_Type)).ToList();
                }

                PatientInsuredPlan patientInsuredPlan = new PatientInsuredPlan()
                {
                    Human_ID = insuredPlan_Akido.Human_ID,
                    Insured_Human_ID = insuredPlan_Akido.Insured_Human_ID,
                    Insurance_Plan_ID = insuredPlan_Akido.Insurance_Plan_ID,
                    Policy_Holder_ID = insuredPlan_Akido.Policy_Holder_ID,
                    Effective_Start_Date = insuredPlan_Akido.Effective_Start_Date,
                    Termination_Date = insuredPlan_Akido.Termination_Date,
                    Insurance_Type = insuredPlan_Akido.Insurance_Type,
                    Relationship = insuredPlan_Akido.Relationship,
                    Group_Number = insuredPlan_Akido.Group_Number,
                    Active = insuredPlan_Akido.Active,
                    Other_Insurance_Comments = insuredPlan_Akido.Other_Insurance_Comments,

                    PCP_ID = insuredPlan_Akido.PCP_ID,
                    PCP_Name = insuredPlan_Akido.PCP_Name,
                    PCP_NPI = insuredPlan_Akido.PCP_NPI,
                    PCP_Copay = insuredPlan_Akido.PCP_Copay,

                    Specialist_Copay = insuredPlan_Akido.Specialist_Copay,
                    Deductible = insuredPlan_Akido.Deductible,
                    Co_Insurance = insuredPlan_Akido.Co_Insurance,
                    Assignment = "Yes",
                    Sort_Order = insuredPlan_Akido.Sort_Order,
                    Deductible_Met_So_Far = insuredPlan_Akido.Deductible_Met_So_Far,
                    CCV_Name = insuredPlan_Akido.CCV_Name,
                    Created_By = "Acurus API",
                    Created_Date_And_Time = DateTime.Now,
                };

                if (!string.IsNullOrEmpty(patientInsuredPlan.Relationship))
                {
                    StaticLookupManager staticLookUpMngr = new StaticLookupManager();
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PATIENT RELATIONSHIP");
                    var filterData = staticlookuplist.FirstOrDefault(a => a.Value.ToLower().Trim() == patientInsuredPlan.Relationship.ToLower().Trim());
                    if (filterData != null && !string.IsNullOrEmpty(filterData.Description))
                    {
                        patientInsuredPlan.Relationship_No = Convert.ToInt32(filterData.Description);
                    }
                }

                List<PatientInsuredPlan> lstAddPatientInsuredPlan = new List<PatientInsuredPlan> { patientInsuredPlan };
                uPat_Insured_Plan_ID = objmanager.BatchAddUpdatePatInsured(lstAddPatientInsuredPlan, updateInsurancePolicies, String.Empty);

                HumanManager HumanMngr = new HumanManager();
                IList<Human> humanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(insuredPlan_Akido.Human_ID);
                Human objHumanList = new Human();
                if (humanList != null && humanList.Count > 0)
                {
                    objHumanList = humanList[0];
                }

                if (objHumanList != null)
                {
                    if (objHumanList.PatientInsuredBag != null && objHumanList.PatientInsuredBag.Count > 0)
                    {
                        IList<PatientInsuredPlan> PatInsOrderedList = (from m in objHumanList.PatientInsuredBag where m.Insurance_Type.ToUpper() == "PRIMARY" && m.Active.ToUpper() == "YES" select m).ToList<PatientInsuredPlan>();

                        string sCarrierName = "";
                        if (PatInsOrderedList.Count > 0)
                        {
                            var lstUpdateHuman = HumanMngr.GetPatientDetailsUsingPatientInformattion(insuredPlan_Akido.Human_ID);
                            if (lstUpdateHuman.Count() > 0)
                            {
                                InsurancePlanManager InsMngr = new InsurancePlanManager();
                                IList<InsurancePlan> insList = InsMngr.GetInsurancebyID(insuredPlan_Akido.Insurance_Plan_ID);
                                if (insList != null && insList.Any())
                                {
                                    lstUpdateHuman[0].Primary_Carrier_ID = Convert.ToUInt64(insList[0].Carrier_ID);
                                }
                                lstUpdateHuman[0].PCP_Name = insuredPlan_Akido.PCP_Name;
                                lstUpdateHuman[0].PCP_NPI = insuredPlan_Akido.PCP_NPI;
                                CarrierManager CarrierMngr = new CarrierManager();
                                Carrier objcarrierName = CarrierMngr.GetCarrierUsingId(Convert.ToUInt64(lstUpdateHuman[0].Primary_Carrier_ID));
                                if (objcarrierName != null)
                                    sCarrierName = objcarrierName.Carrier_Name;
                                HumanMngr.UpdateBatchToHuman(lstUpdateHuman[0], null, null, sCarrierName);
                            }
                        }
                        else
                        {
                            var lstUpdateHuman = HumanMngr.GetPatientDetailsUsingPatientInformattion(insuredPlan_Akido.Human_ID);
                            if (lstUpdateHuman.Count() > 0)
                            {
                                lstUpdateHuman[0].Primary_Carrier_ID = 0;
                                lstUpdateHuman[0].PCP_Name = string.Empty;
                                sCarrierName = "";
                                HumanMngr.UpdateBatchToHuman(lstUpdateHuman[0], null, null, sCarrierName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "0");
                return Json(new { Pat_Insured_Plan_ID = uPat_Insured_Plan_ID, status = "Error", ErrorDescription = "Error in processing the request. " + ex.Message });
            }
            return Json(new { Pat_Insured_Plan_ID = uPat_Insured_Plan_ID, status = "Success" });
        }

        [HttpPut]
        public IHttpActionResult UpdateInsuredPlanData([FromBody] UpdatePatientInsuredPlan_Akido updateInsuredPlan_Akido)
        {
            try
            {
                if (!VerifyToken())
                {
                    return Json(new { status = "Unauthorized", ErrorDescription = "The remote server returned an error: (403) Forbidden." });
                }

                if (updateInsuredPlan_Akido.Pat_Insured_Plan_ID == 0)
                {
                    return Json(new { Pat_Insured_Plan_ID = 0, status = "ValidationError", ErrorDescription = "Pat_Insured_Plan_ID is not present in the request." });
                }

                string errorMsg = InsuredPlanValidationForUpdate(updateInsuredPlan_Akido.Pat_Insured_Plan_Data);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return Json(new { updateInsuredPlan_Akido.Pat_Insured_Plan_ID, status = "ValidationError", ErrorDescription = errorMsg });
                }

                PatientInsuredPlanManager objmanager = new PatientInsuredPlanManager();
                PatientInsuredPlan patientInsuredPlan = objmanager.GetById(updateInsuredPlan_Akido.Pat_Insured_Plan_ID);

                if (patientInsuredPlan == null || patientInsuredPlan.Id == 0)
                {
                    return Json(new { Pat_Insured_Plan_ID = patientInsuredPlan.Id, status = "ValidationError", ErrorDescription = "Pat_Insured_Plan_ID is invalid." });
                }

                foreach (var item in updateInsuredPlan_Akido.Pat_Insured_Plan_Data)
                {
                    UpdateProperty(patientInsuredPlan, item.Key, item.Value);
                }

                var updateInsurancePolicies = objmanager.getInsurancePoliciesByHumanId(patientInsuredPlan.Human_ID);
                if (updateInsurancePolicies != null && updateInsurancePolicies.Any())
                {
                    updateInsurancePolicies
                        .Where(a => a.Insurance_Type == patientInsuredPlan.Insurance_Type && a.Id != patientInsuredPlan.Id)
                        .Select(a => a.Insurance_Type = string.Format("OLD {0}", a.Insurance_Type))
                        .ToList();
                }

                if (updateInsuredPlan_Akido.Pat_Insured_Plan_Data.ContainsKey("Relationship"))
                {
                    string relationship = updateInsuredPlan_Akido.Pat_Insured_Plan_Data["Relationship"]?.ToString();
                    if (!string.IsNullOrEmpty(relationship))
                    {
                        StaticLookupManager staticLookUpMngr = new StaticLookupManager();
                        var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PATIENT RELATIONSHIP");
                        var filterData = staticlookuplist.FirstOrDefault(a => a.Value.ToLower().Trim() == relationship.ToLower().Trim());
                        if (filterData != null && !string.IsNullOrEmpty(filterData.Description))
                        {
                            patientInsuredPlan.Relationship_No = Convert.ToInt32(filterData.Description);
                        }
                    }
                }

                patientInsuredPlan.Modified_By = "Acurus API";
                patientInsuredPlan.Modified_Date_And_Time = DateTime.Now;

                updateInsurancePolicies = updateInsurancePolicies.Where(a => a.Id != patientInsuredPlan.Id).ToList();
                updateInsurancePolicies.Add(patientInsuredPlan);

                objmanager.BatchAddUpdatePatInsured(new List<PatientInsuredPlan>(), updateInsurancePolicies, String.Empty);

                HumanManager HumanMngr = new HumanManager();
                IList<Human> humanList = HumanMngr.GetPatientDetailsUsingPatientInformattion(patientInsuredPlan.Human_ID);
                Human objHumanList = new Human();
                if (humanList != null && humanList.Count > 0)
                {
                    objHumanList = humanList[0];
                }

                if (objHumanList != null)
                {
                    if (objHumanList.PatientInsuredBag != null && objHumanList.PatientInsuredBag.Count > 0)
                    {
                        IList<PatientInsuredPlan> PatInsOrderedList = (from m in objHumanList.PatientInsuredBag where m.Insurance_Type.ToUpper() == "PRIMARY" && m.Active.ToUpper() == "YES" select m).ToList<PatientInsuredPlan>();

                        string sCarrierName = "";
                        if (PatInsOrderedList.Count > 0)
                        {
                            var lstUpdateHuman = HumanMngr.GetPatientDetailsUsingPatientInformattion(patientInsuredPlan.Human_ID);
                            if (lstUpdateHuman.Count() > 0)
                            {
                                InsurancePlanManager InsMngr = new InsurancePlanManager();
                                IList<InsurancePlan> insList = InsMngr.GetInsurancebyID(patientInsuredPlan.Insurance_Plan_ID);
                                if (insList != null && insList.Any())
                                {
                                    lstUpdateHuman[0].Primary_Carrier_ID = Convert.ToUInt64(insList[0].Carrier_ID);
                                }
                                lstUpdateHuman[0].PCP_Name = patientInsuredPlan.PCP_Name;
                                lstUpdateHuman[0].PCP_NPI = patientInsuredPlan.PCP_NPI;
                                CarrierManager CarrierMngr = new CarrierManager();
                                Carrier objcarrierName = CarrierMngr.GetCarrierUsingId(Convert.ToUInt64(lstUpdateHuman[0].Primary_Carrier_ID));
                                if (objcarrierName != null)
                                    sCarrierName = objcarrierName.Carrier_Name;
                                HumanMngr.UpdateBatchToHuman(lstUpdateHuman[0], null, null, sCarrierName);
                            }
                        }
                        else
                        {
                            var lstUpdateHuman = HumanMngr.GetPatientDetailsUsingPatientInformattion(patientInsuredPlan.Human_ID);
                            if (lstUpdateHuman.Count() > 0)
                            {
                                lstUpdateHuman[0].Primary_Carrier_ID = 0;
                                lstUpdateHuman[0].PCP_Name = string.Empty;
                                sCarrierName = "";
                                HumanMngr.UpdateBatchToHuman(lstUpdateHuman[0], null, null, sCarrierName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "0");
                return Json(new { updateInsuredPlan_Akido.Pat_Insured_Plan_ID, status = "Error", ErrorDescription = "Error in processing the request. " + ex.Message });
            }
            return Json(new { updateInsuredPlan_Akido.Pat_Insured_Plan_ID, status = "Success" });
        }

        public string Validation(Human_Akido objHuman, string version)
        {
            DateTime localTime = DateTime.Now;
            StaticLookupManager staticLookUpMngr = new StaticLookupManager();
            #region V1
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
            #endregion

            #region V2
            if (version.ToUpper() == "V2")
            {
                if (string.IsNullOrEmpty(objHuman.SigOn_File))
                {
                    return "SigOn_File is not present in the request.";
                }
                else
                {
                    var allowedDataList = new List<string>() { "YES", "NO" };
                    if (!allowedDataList.Any(a => a.Equals(objHuman.SigOn_File.Trim().ToUpper())))
                    {
                        return "SigOn_File is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.People_In_Collection))
                {
                    return "People_In_Collection is not present in the request.";
                }
                else
                {
                    var allowedDataList = new List<string>() { "Y", "N" };
                    if (!allowedDataList.Any(a => a.Equals(objHuman.People_In_Collection.Trim().ToUpper())))
                    {
                        return "People_In_Collection is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.Preferred_Language))
                {
                    return "Preferred_Language is not present in the request.";
                }
                else
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PREFERRED LANGUAGE");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Preferred_Language.ToLower().Trim()))
                    {
                        return "Preferred_Language is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.Ethnicity))
                {
                    return "Ethnicity is not present in the request.";
                }
                else
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("ETHNICITY");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Ethnicity.ToLower().Trim()))
                    {
                        return "Ethnicity is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.Human_Type))
                {
                    return "Human_Type is not present in the request.";
                }
                else
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("HUMAN TYPE");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Human_Type.ToLower().Trim()))
                    {
                        return "Human_Type is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.Immunization_Registry_Status))
                {
                    return "Immunization_Registry_Status is not present in the request.";
                }
                else
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("IMMUNZATION REGISTRY STATUS");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Immunization_Registry_Status.ToLower().Trim()))
                    {
                        return "Immunization_Registry_Status is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.Publicity_Code))
                {
                    return "Publicity_Code is not present in the request.";
                }
                else
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PUBLICITY CODE");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Publicity_Code.ToLower().Trim()))
                    {
                        return "Publicity_Code is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.Data_Sharing_Preference))
                {
                    return "Data_Sharing_Preference is not present in the request.";
                }
                else
                {
                    var allowedDataList = new List<string>() { "YES", "NO" };
                    if (!allowedDataList.Any(a => a.Equals(objHuman.Data_Sharing_Preference.Trim().ToUpper())))
                    {
                        return "Data_Sharing_Preference is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.Birth_Indicator))
                {
                    return "Birth_Indicator is not present in the request.";
                }
                else
                {
                    var allowedDataList = new List<string>() { "YES", "NO" };
                    if (!allowedDataList.Any(a => a.Equals(objHuman.Birth_Indicator.Trim().ToUpper())))
                    {
                        return "Birth_Indicator is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.Birth_Order))
                {
                    return "Birth_Order is not present in the request.";
                }
                else
                {
                    var allowedDataList = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                    if (!allowedDataList.Any(a => a.Equals(objHuman.Birth_Order.Trim().ToUpper())))
                    {
                        return "Birth_Order is invalid in the request.";
                    }
                }
                if (string.IsNullOrEmpty(objHuman.Is_Translator_Required))
                {
                    return "Is_Translator_Required is not present in the request.";
                }
                else
                {
                    var allowedDataList = new List<string>() { "Y", "N" };
                    if (!allowedDataList.Any(a => a.Equals(objHuman.Is_Translator_Required.Trim().ToUpper())))
                    {
                        return "Is_Translator_Required is invalid in the request.";
                    }
                }



                if (!string.IsNullOrEmpty(objHuman.Employment_Status))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("EMPLOYMENT STATUS");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Employment_Status.ToLower().Trim()))
                    {
                        return "Employment_Status is invalid in the request.";
                    }
                }
                if (!string.IsNullOrEmpty(objHuman.Driver_State))
                {
                    StateManager StateMngr = new StateManager();
                    var statelist = StateMngr.Getstate();
                    if (!statelist.Any(a => a.State_Code.ToLower().Trim() == objHuman.Driver_State.ToLower().Trim()))
                    {
                        return "Driver_State is invalid in the request.";
                    }
                }
                if (!string.IsNullOrEmpty(objHuman.Emergency_Cnt_Sex))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("SEX");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Emergency_Cnt_Sex.ToLower().Trim()))
                    {
                        return "Emergency_Cnt_Sex is invalid in the request.";
                    }
                }
                if (!string.IsNullOrEmpty(objHuman.Emergency_Cnt_State))
                {
                    StateManager StateMngr = new StateManager();
                    var statelist = StateMngr.Getstate();
                    if (!statelist.Any(a => a.State_Code.ToLower().Trim() == objHuman.Emergency_Cnt_State.ToLower().Trim()))
                    {
                        return "Emergency_Cnt_State is invalid in the request.";
                    }
                }
                if (!string.IsNullOrEmpty(objHuman.Emer_Relation))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RELATIONSHIP");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Emer_Relation.ToLower().Trim()))
                    {
                        return "Emer_Relation is invalid in the request.";
                    }
                }
                if (!string.IsNullOrEmpty(objHuman.Demo_Status))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("DEMO STATUS");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Demo_Status.ToLower().Trim()))
                    {
                        return "Demo_Status is invalid in the request.";
                    }
                }
                if (!string.IsNullOrEmpty(objHuman.Race))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RACE");
                    foreach (var item in objHuman.Race.Split(','))
                    {
                        if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == item.ToLower().Trim()))
                        {
                            return "Race is invalid in the request.";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(objHuman.Preferred_Confidential_Correspodence_Mode))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PREFERRED CONFIDENTIAL CO");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Preferred_Confidential_Correspodence_Mode.ToLower().Trim()))
                    {
                        return "Preferred_Confidential_Correspodence_Mode is invalid in the request.";
                    }
                }
                if (!string.IsNullOrEmpty(objHuman.Declared_Bankruptcy))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("DECLAREDBANKRUPTCY");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Declared_Bankruptcy.ToLower().Trim()))
                    {
                        return "Declared_Bankruptcy is invalid in the request.";
                    }
                }
                if (!string.IsNullOrEmpty(objHuman.Sexual_Orientation))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("SEXUAL ORIENTATION");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Sexual_Orientation.ToLower().Trim()))
                    {
                        return "Sexual_Orientation is invalid in the request.";
                    }
                }
                if (!string.IsNullOrEmpty(objHuman.Gender_Identity))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("GENDER IDENTITY");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Gender_Identity.ToLower().Trim()))
                    {
                        return "Gender_Identity is invalid in the request.";
                    }
                }
                if (!string.IsNullOrEmpty(objHuman.Granularity))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("GRANULARITY");
                    foreach (var item in objHuman.Granularity.Split(','))
                    {
                        if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == item.ToLower().Trim()))
                        {
                            return "Granularity is invalid in the request.";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(objHuman.Tribal_Affiliation))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("Tribal Affiliation");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Tribal_Affiliation.ToLower().Trim()))
                    {
                        return "Tribal_Affiliation is invalid in the request.";
                    }
                }
                if (!string.IsNullOrEmpty(objHuman.Specific_Ethnicity))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("SPECIFIC ETHINICITY");
                    foreach (var item in objHuman.Specific_Ethnicity.Split(','))
                    {
                        if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == item.ToLower().Trim()))
                        {
                            return "Specific_Ethnicity is invalid in the request.";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(objHuman.Emergency_Cnt_ZipCode)
                    && (objHuman.Emergency_Cnt_ZipCode.Length != 5
                    || objHuman.Emergency_Cnt_ZipCode.Length != 9
                    || !Regex.IsMatch(objHuman.Emergency_Cnt_ZipCode, @"^\d+$")))
                {
                    return "Emergency_Cnt_ZipCode is invalid in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Emergency_Cnt_Home_Phone_Number)
                    && (objHuman.Emergency_Cnt_Home_Phone_Number.Length != 10
                    || !Regex.IsMatch(objHuman.Emergency_Cnt_Home_Phone_Number, @"^\d+$")))
                {
                    return "Emergency_Cnt_Home_Phone_Number is invalid in the request.";
                }
                if (ValidateDateFormate(objHuman.Emergency_BirthDate.ToString()))
                {
                    return "Emergency_BirthDate is invalid in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Emergency_Cnt_CellPhone_Number)
                    && (objHuman.Emergency_Cnt_CellPhone_Number.Length != 10
                    || !Regex.IsMatch(objHuman.Emergency_Cnt_CellPhone_Number, @"^\d+$")))
                {
                    return "Emergency_Cnt_CellPhone_Number is invalid in the request.";
                }

                if (!string.IsNullOrEmpty(objHuman.Street_Address2) && objHuman.Street_Address2.Length > 55)
                {
                    return "Street_Address2 exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Driver_License_Num) && objHuman.Driver_License_Num.Length > 15)
                {
                    return "Driver_License_Num exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Emergency_Cnt_Last_Name) && objHuman.Emergency_Cnt_Last_Name.Length > 35)
                {
                    return "Emergency_Cnt_Last_Name exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Emergency_Cnt_First_Name) && objHuman.Emergency_Cnt_First_Name.Length > 25)
                {
                    return "Emergency_Cnt_First_Name exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Emergency_Cnt_MI) && objHuman.Emergency_Cnt_MI.Length > 55)
                {
                    return "Emergency_Cnt_MI exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Emergency_Cnt_StreetAddr1) && objHuman.Emergency_Cnt_StreetAddr1.Length > 55)
                {
                    return "Emergency_Cnt_StreetAddr1 exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Emergency_Cnt_StreetAddr2) && objHuman.Emergency_Cnt_StreetAddr2.Length > 55)
                {
                    return "Emergency_Cnt_StreetAddr2 exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Emergency_Cnt_City) && objHuman.Emergency_Cnt_City.Length > 35)
                {
                    return "Emergency_Cnt_City exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.PCP_Name) && objHuman.PCP_Name.Length > 100)
                {
                    return "PCP_Name exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.PCP_NPI) && objHuman.PCP_NPI.Length > 10)
                {
                    return "PCP_NPI exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Mothers_Maiden_Name) && objHuman.Mothers_Maiden_Name.Length > 50)
                {
                    return "Mothers_Maiden_Name exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Representative_Email) && objHuman.Representative_Email.Length > 100)
                {
                    return "Representative_Email exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Sexual_Orientation_Specify) && objHuman.Sexual_Orientation_Specify.Length > 100)
                {
                    return "Sexual_Orientation_Specify exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Gender_Identity_Specify) && objHuman.Gender_Identity_Specify.Length > 100)
                {
                    return "Gender_Identity_Specify exceeds the maximum length in the request.";
                }
                if (!string.IsNullOrEmpty(objHuman.Dynamics_Number) && objHuman.Dynamics_Number.Length > 25)
                {
                    return "Dynamics_Number exceeds the maximum length in the request.";
                }
            }
            #endregion

            return "";
        }

        public string ValidationForUpdate(Dictionary<string, object> human_data, string version)
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

            #region V1
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

            #region V2
            if (version.ToUpper() == "V2")
            {
                if (human_data.ContainsKey("SigOn_File"))
                {
                    if (string.IsNullOrEmpty(objHuman.SigOn_File))
                    {
                        return "SigOn_File is not present in the request.";
                    }
                    else
                    {
                        var allowedDataList = new List<string>() { "YES", "NO" };
                        if (!allowedDataList.Any(a => a.Equals(objHuman.SigOn_File.Trim().ToUpper())))
                        {
                            return "SigOn_File is invalid in the request.";
                        }
                    }
                }
                if (human_data.ContainsKey("People_In_Collection"))
                {
                    if (string.IsNullOrEmpty(objHuman.People_In_Collection))
                    {
                        return "People_In_Collection is not present in the request.";
                    }
                    else
                    {
                        var allowedDataList = new List<string>() { "Y", "N" };
                        if (!allowedDataList.Any(a => a.Equals(objHuman.People_In_Collection.Trim().ToUpper())))
                        {
                            return "People_In_Collection is invalid in the request.";
                        }
                    }
                }
                if (human_data.ContainsKey("Preferred_Language"))
                {
                    if (string.IsNullOrEmpty(objHuman.Preferred_Language))
                    {
                        return "Preferred_Language is not present in the request.";
                    }
                    else
                    {
                        var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PREFERRED LANGUAGE");
                        if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Preferred_Language.ToLower().Trim()))
                        {
                            return "Preferred_Language is invalid in the request.";
                        }
                    }
                }
                if (human_data.ContainsKey("Ethnicity"))
                {
                    if (string.IsNullOrEmpty(objHuman.Ethnicity))
                    {
                        return "Ethnicity is not present in the request.";
                    }
                    else
                    {
                        var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("ETHNICITY");
                        if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Ethnicity.ToLower().Trim()))
                        {
                            return "Ethnicity is invalid in the request.";
                        }
                    }
                }
                if (human_data.ContainsKey("Human_Type"))
                {
                    if (string.IsNullOrEmpty(objHuman.Human_Type))
                    {
                        return "Human_Type is not present in the request.";
                    }
                    else
                    {
                        var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("HUMAN TYPE");
                        if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Human_Type.ToLower().Trim()))
                        {
                            return "Human_Type is invalid in the request.";
                        }
                    }
                }
                if (human_data.ContainsKey("Immunization_Registry_Status"))
                {
                    if (string.IsNullOrEmpty(objHuman.Immunization_Registry_Status))
                    {
                        return "Immunization_Registry_Status is not present in the request.";
                    }
                    else
                    {
                        var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("IMMUNZATION REGISTRY STATUS");
                        if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Immunization_Registry_Status.ToLower().Trim()))
                        {
                            return "Immunization_Registry_Status is invalid in the request.";
                        }
                    }
                }
                if (human_data.ContainsKey("Publicity_Code"))
                {
                    if (string.IsNullOrEmpty(objHuman.Publicity_Code))
                    {
                        return "Publicity_Code is not present in the request.";
                    }
                    else
                    {
                        var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PUBLICITY CODE");
                        if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Publicity_Code.ToLower().Trim()))
                        {
                            return "Publicity_Code is invalid in the request.";
                        }
                    }
                }
                if (human_data.ContainsKey("Data_Sharing_Preference"))
                {
                    if (string.IsNullOrEmpty(objHuman.Data_Sharing_Preference))
                    {
                        return "Data_Sharing_Preference is not present in the request.";
                    }
                    else
                    {
                        var allowedDataList = new List<string>() { "YES", "NO" };
                        if (!allowedDataList.Any(a => a.Equals(objHuman.Data_Sharing_Preference.Trim().ToUpper())))
                        {
                            return "Data_Sharing_Preference is invalid in the request.";
                        }
                    }
                }
                if (human_data.ContainsKey("Birth_Indicator"))
                {
                    if (string.IsNullOrEmpty(objHuman.Birth_Indicator))
                    {
                        return "Birth_Indicator is not present in the request.";
                    }
                    else
                    {
                        var allowedDataList = new List<string>() { "YES", "NO" };
                        if (!allowedDataList.Any(a => a.Equals(objHuman.Birth_Indicator.Trim().ToUpper())))
                        {
                            return "Birth_Indicator is invalid in the request.";
                        }
                    }
                }
                if (human_data.ContainsKey("Birth_Order"))
                {
                    if (string.IsNullOrEmpty(objHuman.Birth_Order))
                    {
                        return "Birth_Order is not present in the request.";
                    }
                    else
                    {
                        var allowedDataList = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                        if (!allowedDataList.Any(a => a.Equals(objHuman.Birth_Order.Trim().ToUpper())))
                        {
                            return "Birth_Order is invalid in the request.";
                        }
                    }
                }
                if (human_data.ContainsKey("Is_Translator_Required"))
                {
                    if (string.IsNullOrEmpty(objHuman.Is_Translator_Required))
                    {
                        return "Is_Translator_Required is not present in the request.";
                    }
                    else
                    {
                        var allowedDataList = new List<string>() { "Y", "N" };
                        if (!allowedDataList.Any(a => a.Equals(objHuman.Is_Translator_Required.Trim().ToUpper())))
                        {
                            return "Is_Translator_Required is invalid in the request.";
                        }
                    }
                }



                if (human_data.ContainsKey("Employment_Status") && !string.IsNullOrEmpty(objHuman.Employment_Status))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("EMPLOYMENT STATUS");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Employment_Status.ToLower().Trim()))
                    {
                        return "Employment_Status is invalid in the request.";
                    }
                }
                if (human_data.ContainsKey("Driver_State") && !string.IsNullOrEmpty(objHuman.Driver_State))
                {
                    StateManager StateMngr = new StateManager();
                    var statelist = StateMngr.Getstate();
                    if (!statelist.Any(a => a.State_Code.ToLower().Trim() == objHuman.Driver_State.ToLower().Trim()))
                    {
                        return "Driver_State is invalid in the request.";
                    }
                }
                if (human_data.ContainsKey("Emergency_Cnt_Sex") && !string.IsNullOrEmpty(objHuman.Emergency_Cnt_Sex))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("SEX");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Emergency_Cnt_Sex.ToLower().Trim()))
                    {
                        return "Emergency_Cnt_Sex is invalid in the request.";
                    }
                }
                if (human_data.ContainsKey("Emergency_Cnt_State") && !string.IsNullOrEmpty(objHuman.Emergency_Cnt_State))
                {
                    StateManager StateMngr = new StateManager();
                    var statelist = StateMngr.Getstate();
                    if (!statelist.Any(a => a.State_Code.ToLower().Trim() == objHuman.Emergency_Cnt_State.ToLower().Trim()))
                    {
                        return "Emergency_Cnt_State is invalid in the request.";
                    }
                }
                if (human_data.ContainsKey("Emer_Relation") && !string.IsNullOrEmpty(objHuman.Emer_Relation))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RELATIONSHIP");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Emer_Relation.ToLower().Trim()))
                    {
                        return "Emer_Relation is invalid in the request.";
                    }
                }
                if (human_data.ContainsKey("Demo_Status") && !string.IsNullOrEmpty(objHuman.Demo_Status))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("DEMO STATUS");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Demo_Status.ToLower().Trim()))
                    {
                        return "Demo_Status is invalid in the request.";
                    }
                }
                if (human_data.ContainsKey("Race") && !string.IsNullOrEmpty(objHuman.Race))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("RACE");
                    foreach (var item in objHuman.Race.Split(','))
                    {
                        if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == item.ToLower().Trim()))
                        {
                            return "Race is invalid in the request.";
                        }
                    }
                }
                if (human_data.ContainsKey("Preferred_Confidential_Correspodence_Mode") && !string.IsNullOrEmpty(objHuman.Preferred_Confidential_Correspodence_Mode))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PREFERRED CONFIDENTIAL CO");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Preferred_Confidential_Correspodence_Mode.ToLower().Trim()))
                    {
                        return "Preferred_Confidential_Correspodence_Mode is invalid in the request.";
                    }
                }
                if (human_data.ContainsKey("Declared_Bankruptcy") && !string.IsNullOrEmpty(objHuman.Declared_Bankruptcy))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("DECLAREDBANKRUPTCY");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Declared_Bankruptcy.ToLower().Trim()))
                    {
                        return "Declared_Bankruptcy is invalid in the request.";
                    }
                }
                if (human_data.ContainsKey("Sexual_Orientation") && !string.IsNullOrEmpty(objHuman.Sexual_Orientation))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("SEXUAL ORIENTATION");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Sexual_Orientation.ToLower().Trim()))
                    {
                        return "Sexual_Orientation is invalid in the request.";
                    }
                }
                if (human_data.ContainsKey("Gender_Identity") && !string.IsNullOrEmpty(objHuman.Gender_Identity))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("GENDER IDENTITY");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Gender_Identity.ToLower().Trim()))
                    {
                        return "Gender_Identity is invalid in the request.";
                    }
                }
                if (human_data.ContainsKey("Granularity") && !string.IsNullOrEmpty(objHuman.Granularity))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("GRANULARITY");
                    foreach (var item in objHuman.Granularity.Split(','))
                    {
                        if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == item.ToLower().Trim()))
                        {
                            return "Granularity is invalid in the request.";
                        }
                    }
                }
                if (human_data.ContainsKey("Tribal_Affiliation") && !string.IsNullOrEmpty(objHuman.Tribal_Affiliation))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("Tribal Affiliation");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objHuman.Tribal_Affiliation.ToLower().Trim()))
                    {
                        return "Tribal_Affiliation is invalid in the request.";
                    }
                }
                if (human_data.ContainsKey("Specific_Ethnicity") && !string.IsNullOrEmpty(objHuman.Specific_Ethnicity))
                {
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("SPECIFIC ETHINICITY");
                    foreach (var item in objHuman.Specific_Ethnicity.Split(','))
                    {
                        if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == item.ToLower().Trim()))
                        {
                            return "Specific_Ethnicity is invalid in the request.";
                        }
                    }
                }

                if (human_data.ContainsKey("Emergency_Cnt_ZipCode")
                    && !string.IsNullOrEmpty(objHuman.Emergency_Cnt_ZipCode)
                    && (objHuman.Emergency_Cnt_ZipCode.Length != 5
                    || objHuman.Emergency_Cnt_ZipCode.Length != 9
                    || !Regex.IsMatch(objHuman.Emergency_Cnt_ZipCode, @"^\d+$")))
                {
                    return "Emergency_Cnt_ZipCode is invalid in the request.";
                }
                if (human_data.ContainsKey("Emergency_Cnt_Home_Phone_Number")
                    && !string.IsNullOrEmpty(objHuman.Emergency_Cnt_Home_Phone_Number)
                    && (objHuman.Emergency_Cnt_Home_Phone_Number.Length != 10
                    || !Regex.IsMatch(objHuman.Emergency_Cnt_Home_Phone_Number, @"^\d+$")))
                {
                    return "Emergency_Cnt_Home_Phone_Number is invalid in the request.";
                }
                if (human_data.ContainsKey("Emergency_BirthDate") && ValidateDateFormate(objHuman.Emergency_BirthDate.ToString()))
                {
                    return "Emergency_BirthDate is invalid in the request.";
                }
                if (human_data.ContainsKey("Emergency_Cnt_CellPhone_Number")
                    && !string.IsNullOrEmpty(objHuman.Emergency_Cnt_CellPhone_Number)
                    && (objHuman.Emergency_Cnt_CellPhone_Number.Length != 10
                    || !Regex.IsMatch(objHuman.Emergency_Cnt_CellPhone_Number, @"^\d+$")))
                {
                    return "Emergency_Cnt_CellPhone_Number is invalid in the request.";
                }

                if (human_data.ContainsKey("Street_Address2") && !string.IsNullOrEmpty(objHuman.Street_Address2) && objHuman.Street_Address2.Length > 55)
                {
                    return "Street_Address2 exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("Driver_License_Num") && !string.IsNullOrEmpty(objHuman.Driver_License_Num) && objHuman.Driver_License_Num.Length > 15)
                {
                    return "Driver_License_Num exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("Emergency_Cnt_Last_Name") && !string.IsNullOrEmpty(objHuman.Emergency_Cnt_Last_Name) && objHuman.Emergency_Cnt_Last_Name.Length > 35)
                {
                    return "Emergency_Cnt_Last_Name exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("Emergency_Cnt_First_Name") && !string.IsNullOrEmpty(objHuman.Emergency_Cnt_First_Name) && objHuman.Emergency_Cnt_First_Name.Length > 25)
                {
                    return "Emergency_Cnt_First_Name exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("Emergency_Cnt_MI") && !string.IsNullOrEmpty(objHuman.Emergency_Cnt_MI) && objHuman.Emergency_Cnt_MI.Length > 55)
                {
                    return "Emergency_Cnt_MI exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("Emergency_Cnt_StreetAddr1") && !string.IsNullOrEmpty(objHuman.Emergency_Cnt_StreetAddr1) && objHuman.Emergency_Cnt_StreetAddr1.Length > 55)
                {
                    return "Emergency_Cnt_StreetAddr1 exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("Emergency_Cnt_StreetAddr2") && !string.IsNullOrEmpty(objHuman.Emergency_Cnt_StreetAddr2) && objHuman.Emergency_Cnt_StreetAddr2.Length > 55)
                {
                    return "Emergency_Cnt_StreetAddr2 exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("Emergency_Cnt_City") && !string.IsNullOrEmpty(objHuman.Emergency_Cnt_City) && objHuman.Emergency_Cnt_City.Length > 35)
                {
                    return "Emergency_Cnt_City exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("PCP_Name") && !string.IsNullOrEmpty(objHuman.PCP_Name) && objHuman.PCP_Name.Length > 100)
                {
                    return "PCP_Name exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("Work_Phone_Ext") && !string.IsNullOrEmpty(objHuman.PCP_NPI) && objHuman.PCP_NPI.Length > 10)
                {
                    return "PCP_NPI exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("Mothers_Maiden_Name") && !string.IsNullOrEmpty(objHuman.Mothers_Maiden_Name) && objHuman.Mothers_Maiden_Name.Length > 50)
                {
                    return "Mothers_Maiden_Name exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("Representative_Email") && !string.IsNullOrEmpty(objHuman.Representative_Email) && objHuman.Representative_Email.Length > 100)
                {
                    return "Representative_Email exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("Sexual_Orientation_Specify") && !string.IsNullOrEmpty(objHuman.Sexual_Orientation_Specify) && objHuman.Sexual_Orientation_Specify.Length > 100)
                {
                    return "Sexual_Orientation_Specify exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("Gender_Identity_Specify") && !string.IsNullOrEmpty(objHuman.Gender_Identity_Specify) && objHuman.Gender_Identity_Specify.Length > 100)
                {
                    return "Gender_Identity_Specify exceeds the maximum length in the request.";
                }
                if (human_data.ContainsKey("Dynamics_Number") && !string.IsNullOrEmpty(objHuman.Dynamics_Number) && objHuman.Dynamics_Number.Length > 25)
                {
                    return "Dynamics_Number exceeds the maximum length in the request.";
                }
            }
            #endregion

            return "";
        }

        public string InsuredPlanValidation(PatientInsuredPlan_Akido objInsuredPlan_Akido)
        {
            if (objInsuredPlan_Akido.Human_ID == 0)
            {
                return "Human_ID is not present in the request.";
            }
            if (string.IsNullOrEmpty(objInsuredPlan_Akido.Insurance_Type))
            {
                return "Insurance_Type is not present in the request.";
            }
            else
            {
                var stringList1 = new List<string>() { "PRIMARY", "SECONDARY", "TERTIARY" };
                if (!stringList1.Any(a => a.Equals(objInsuredPlan_Akido.Insurance_Type.Trim().ToUpper())))
                    return "Insurance_Type is invalid in the request.";
            }
            if (objInsuredPlan_Akido.Insurance_Plan_ID == 0)
            {
                return "Insurance_Plan_ID is not present in the request.";
            }

            if (objInsuredPlan_Akido.Insurance_Plan_ID == 6930
                && string.IsNullOrEmpty(objInsuredPlan_Akido.Other_Insurance_Comments))
            {
                return "Other_Insurance_Comments is not present in the request.";
            }

            if (string.IsNullOrEmpty(objInsuredPlan_Akido.Policy_Holder_ID))
            {
                return "Policy_Holder_ID is not present in the request.";
            }
            if (string.IsNullOrEmpty(objInsuredPlan_Akido.Relationship))
            {
                return "Relationship is not present in the request.";
            }
            else
            {
                StaticLookupManager staticLookUpMngr = new StaticLookupManager();
                var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PATIENT RELATIONSHIP");
                if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objInsuredPlan_Akido.Relationship.ToLower().Trim()))
                {
                    return "Relationship is invalid in the request.";
                }
            }
            if (objInsuredPlan_Akido.Relationship.Trim().ToUpper() != "SELF"
                && objInsuredPlan_Akido.Insured_Human_ID == 0)
            {
                return "Insured_Human_ID is not present in the request.";
            }
            var stringList = new List<string>() { "YES", "NO" };
            if (!stringList.Any(a => a.Equals(objInsuredPlan_Akido.Active.Trim().ToUpper())))
                return "Active is invalid in the request.";

            if (objInsuredPlan_Akido.Effective_Start_Date > objInsuredPlan_Akido.Termination_Date)
            {
                return "Effective_Start_Date should be greater than Termination_Date. Please enter a valid date.";
            }

            return "";
        }

        public string InsuredPlanValidationForUpdate(Dictionary<string, object> insuredPlan_data)
        {
            PatientInsuredPlan_Akido objInsuredPlan = new PatientInsuredPlan_Akido();
            foreach (var item in insuredPlan_data)
            {
                var prop = objInsuredPlan.GetType()
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

                        prop.SetValue(objInsuredPlan, convertedValue);
                    }
                    catch
                    {
                        return $"{prop.Name} has invalid data format.";
                    }
                }
            }

            if (insuredPlan_data.ContainsKey("Insurance_Type"))
            {
                if (string.IsNullOrEmpty(objInsuredPlan.Insurance_Type))
                    return "Insurance_Type is not present in the request.";
                else
                {
                    var stringList1 = new List<string>() { "PRIMARY", "SECONDARY", "TERTIARY" };
                    if (!stringList1.Any(a => a.Equals(objInsuredPlan.Insurance_Type.Trim().ToUpper())))
                        return "Insurance_Type is invalid in the request.";
                }
            }

            if (insuredPlan_data.ContainsKey("Insurance_Plan_ID") && objInsuredPlan.Insurance_Plan_ID == 0)
                return "Insurance_Plan_ID is not present in the request.";

            if (insuredPlan_data.ContainsKey("Insurance_Plan_ID")
                && objInsuredPlan.Insurance_Plan_ID == 6930
                && string.IsNullOrEmpty(objInsuredPlan.Other_Insurance_Comments))
            {
                return "Other_Insurance_Comments is not present in the request.";
            }

            if (insuredPlan_data.ContainsKey("Policy_Holder_ID") && string.IsNullOrEmpty(objInsuredPlan.Policy_Holder_ID))
            {
                return "Policy_Holder_ID is not present in the request.";
            }

            if (insuredPlan_data.ContainsKey("Relationship"))
            {
                if (string.IsNullOrEmpty(objInsuredPlan.Relationship))
                {
                    return "Relationship is not present in the request.";
                }
                else
                {
                    StaticLookupManager staticLookUpMngr = new StaticLookupManager();
                    var staticlookuplist = staticLookUpMngr.getStaticLookupByFieldName("PATIENT RELATIONSHIP");
                    if (!staticlookuplist.Any(a => a.Value.ToLower().Trim() == objInsuredPlan.Relationship.ToLower().Trim()))
                    {
                        return "Relationship is invalid in the request.";
                    }
                }
            }

            if ((insuredPlan_data.ContainsKey("Insured_Human_ID")
                || insuredPlan_data.ContainsKey("Relationship"))
                && objInsuredPlan.Relationship.Trim().ToUpper() != "SELF"
                && objInsuredPlan.Insured_Human_ID == 0)
            {
                return "Insured_Human_ID is not present in the request.";
            }

            if (insuredPlan_data.ContainsKey("Active"))
            {
                var stringList = new List<string>() { "YES", "NO" };
                if (!stringList.Any(a => a.Equals(objInsuredPlan.Active.Trim().ToUpper())))
                    return "Active is invalid in the request.";
            }

            if ((insuredPlan_data.ContainsKey("Effective_Start_Date")
                || insuredPlan_data.ContainsKey("Termination_Date"))
                && objInsuredPlan.Effective_Start_Date > objInsuredPlan.Termination_Date)
            {
                return "Effective_Start_Date should be greater than Termination_Date. Please enter a valid date.";
            }

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

        //V2
        public virtual string Granularity { get; set; } = string.Empty;
        public virtual string Sexual_Orientation { get; set; } = string.Empty;
        public virtual string Sexual_Orientation_Specify { get; set; } = string.Empty;
        public virtual string Gender_Identity { get; set; } = string.Empty;
        public virtual string Gender_Identity_Specify { get; set; } = string.Empty;
        public virtual string SigOn_File { get; set; } = string.Empty;
        public virtual string Employment_Status { get; set; } = string.Empty;
        public virtual string Patient_Notes { get; set; } = string.Empty;
        public virtual string Driver_State { get; set; } = string.Empty;
        public virtual string Driver_License_Num { get; set; } = string.Empty;
        public virtual string Emergency_Cnt_Last_Name { get; set; } = string.Empty;
        public virtual string Emergency_Cnt_First_Name { get; set; } = string.Empty;
        public virtual string Emergency_Cnt_MI { get; set; } = string.Empty;
        public virtual string Emergency_Cnt_StreetAddr1 { get; set; } = string.Empty;
        public virtual string Emergency_Cnt_StreetAddr2 { get; set; } = string.Empty;
        public virtual string Emergency_Cnt_City { get; set; } = string.Empty;
        public virtual string Emergency_Cnt_Sex { get; set; } = string.Empty;
        public virtual string Emergency_Cnt_State { get; set; } = string.Empty;
        public virtual string Emergency_Cnt_ZipCode { get; set; } = string.Empty;
        public virtual string Emergency_Cnt_Home_Phone_Number { get; set; } = string.Empty;
        public virtual DateTime Emergency_BirthDate { get; set; } = DateTime.MinValue;
        public virtual string Emergency_Cnt_CellPhone_Number { get; set; } = string.Empty;
        public virtual string Emer_Relation { get; set; } = string.Empty;
        public virtual string Demo_Status { get; set; } = string.Empty;
        public virtual string People_In_Collection { get; set; } = string.Empty;
        public virtual string Preferred_Language { get; set; } = string.Empty;
        public virtual string Race { get; set; } = string.Empty;
        public virtual string Ethnicity { get; set; } = string.Empty;
        public virtual string Photo_Path { get; set; } = string.Empty;
        public virtual string Race_No { get; set; } = string.Empty;
        public virtual string Preferred_Confidential_Correspodence_Mode { get; set; } = string.Empty;
        public virtual string Human_Type { get; set; } = string.Empty;
        public virtual string Declared_Bankruptcy { get; set; } = string.Empty;
        public virtual string PCP_Name { get; set; } = string.Empty;
        public virtual string PCP_NPI { get; set; } = string.Empty;
        public virtual string Mothers_Maiden_Name { get; set; } = string.Empty;
        public virtual string Immunization_Registry_Status { get; set; } = string.Empty;
        public virtual string Publicity_Code { get; set; } = string.Empty;
        public virtual string Representative_Email { get; set; } = string.Empty;
        public virtual string Data_Sharing_Preference { get; set; } = string.Empty;
        public virtual string Birth_Indicator { get; set; } = string.Empty;
        public virtual string Birth_Order { get; set; } = string.Empty;
        public virtual ulong Primary_Carrier_ID { get; set; } = 0;
        public virtual string Is_Translator_Required { get; set; } = string.Empty;
        public virtual string Dynamics_Number { get; set; } = string.Empty;
        public virtual string Tribal_Affiliation { get; set; } = string.Empty;
        public virtual string Specific_Ethnicity { get; set; } = string.Empty;
        public virtual string Insurance_Status { get; set; } = string.Empty;
    }

    public class UpdateHuman_Akido
    {
        public ulong humanID { get; set; }
        public Dictionary<string, object> human_data { get; set; }
    }

    public class PatientInsuredPlan_Akido
    {
        public virtual ulong Human_ID { get; set; } = 0;
        public virtual ulong Insurance_Plan_ID { get; set; } = 0;
        public virtual ulong Insured_Human_ID { get; set; } = 0;
        public virtual string Group_Number { get; set; } = string.Empty;
        public virtual string Policy_Holder_ID { get; set; } = string.Empty;
        public virtual DateTime Effective_Start_Date { get; set; } = DateTime.MinValue;
        public virtual DateTime Termination_Date { get; set; } = DateTime.MinValue;
        public virtual double PCP_Copay { get; set; } = 0.0;
        public virtual double Specialist_Copay { get; set; } = 0.0;
        public virtual double Deductible { get; set; } = 0.0;
        public virtual double Co_Insurance { get; set; } = 0.0;
        public virtual string Insurance_Type { get; set; } = string.Empty;
        public virtual string Relationship { get; set; } = string.Empty;
        public virtual string Active { get; set; } = string.Empty;
        public virtual ulong PCP_ID { get; set; } = 0;
        public virtual int Sort_Order { get; set; } = 0;
        public virtual string PCP_Name { get; set; } = string.Empty;
        public virtual string PCP_NPI { get; set; } = string.Empty;
        public virtual double Deductible_Met_So_Far { get; set; } = 0;
        public virtual string Other_Insurance_Comments { get; set; } = string.Empty;
        public virtual string CCV_Name { get; set; } = string.Empty;
    }

    public class UpdatePatientInsuredPlan_Akido
    {
        public ulong Pat_Insured_Plan_ID { get; set; }
        public Dictionary<string, object> Pat_Insured_Plan_Data { get; set; }
    }
}