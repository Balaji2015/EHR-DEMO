using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTOJson;
using Acurus.Capella.DataAccess.ManagerObjects;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace Acurus.Capella.UI.WebServices.API
{
    public class ProgressNotesServiceController : ApiController
    {
        [HttpGet]
        public IHttpActionResult LoadCapellaHistoryData(string sHumanID, string sCategory = "", bool bIsForce = false)
        {
            HumanManager humanManager = new HumanManager();
            Human objHuman = new Human();
            string sAuthorizedLegalOrg = ConfigurationSettings.AppSettings["LegalOrgForHydration"]?.ToString() ?? "";
            CDC_Audit_LogManager cDC_Audit_LogManager = new CDC_Audit_LogManager();
            IList<CDC_Audit_Log> ilstCDC_Audit_Log = new List<CDC_Audit_Log>();
            ilstCDC_Audit_Log.Add(new CDC_Audit_Log());
            ilstCDC_Audit_Log[0].Human_ID = Convert.ToUInt64(sHumanID ?? "0");
            ilstCDC_Audit_Log[0].API_Name = "LoadCapellaHistoryData";
            ilstCDC_Audit_Log[0].Request = Request.RequestUri.ToString();
            ilstCDC_Audit_Log[0].Created_By = "Acurus";
            ilstCDC_Audit_Log[0].Created_Date_And_Time = DateTime.UtcNow;
            cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);

            try
            {
                if (!VerifyToken())
                {
                    ilstCDC_Audit_Log[0].Response = @"status = ""Unauthorized"", ErrorDescription = ""The remote server returned an error: (403) Forbidden.""";
                    ilstCDC_Audit_Log[0].Modified_By = "Acurus";
                    ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
                    cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
                    return Json(new { status = "Unauthorized", ErrorDescription = "The remote server returned an error: (403) Forbidden." });
                }
                //Jira CAP-3519
                //if (sHumanID == "")
                if (sHumanID == "" || sHumanID == "0")
                {
                    ilstCDC_Audit_Log[0].Response = @"HumanID = " + sHumanID + @", status = ""ValidationError"", ErrorDescription = ""HumanID is not valid. Cannot load Capella history data.""";
                    ilstCDC_Audit_Log[0].Modified_By = "Acurus";
                    ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
                    cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
                    return Json(new { HumanID = sHumanID, status = "ValidationError", ErrorDescription = "HumanID is not valid. Cannot load Capella history data." });
                }
                sCategory = sCategory == null ? "" : sCategory;
                if (sCategory.ToUpper() != "" && sCategory.ToUpper() != "ENCOUNTERS" && sCategory.ToUpper() != "FILES" && sCategory.ToUpper() != "LABRESULTS")
                {
                    ilstCDC_Audit_Log[0].Response = @"HumanID = " + sHumanID + @", status = ""ValidationError"", ErrorDescription = ""Category is not valid. Cannot load Capella history data.""";
                    ilstCDC_Audit_Log[0].Modified_By = "Acurus";
                    ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
                    cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
                    return Json(new { HumanID = sHumanID, status = "ValidationError", ErrorDescription = "Category is not valid. Cannot load Capella history data." });
                }
                //Jira CAP-3519
                objHuman = humanManager.GetHumanFromHumanID(Convert.ToUInt64(sHumanID));
                if (!(sAuthorizedLegalOrg.Split(',')).Contains(objHuman.Legal_Org))
                {
                    ilstCDC_Audit_Log[0].Response = @"HumanID = " + sHumanID + @", status = ""ValidationError"", ErrorDescription = ""This patient is not part of " + sAuthorizedLegalOrg + @" Legal Org. Cannot load Capella history data.""";
                    ilstCDC_Audit_Log[0].Modified_By = "Acurus";
                    ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
                    cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
                    return Json(new { HumanID = sHumanID, status = "ValidationError", ErrorDescription = "This patient is not part of " + sAuthorizedLegalOrg + " Legal Org. Cannot load Capella history data." });
                }
                //CAP-3112
                //if (!CapellaTaskManager.TryStartTask(sHumanID, ""))
                //{
                //    return Json(new { HumanID = sHumanID, status = "InProgress", ErrorDescription = "Request is already InProgress." });
                //}

                Task.Run(() =>
                {
                    //try
                    //{
                        //GenerateCapellaHistoryData(sHumanID,sCategory);
                        GenerateCapellaHistoryData(sHumanID, bIsForce, sCategory);
                    //}
                    //finally
                    //{
                        //CapellaTaskManager.EndTask(sHumanID, "");
                    //}
                });
            }
            catch (Exception ex)
            {
                ilstCDC_Audit_Log[0].Response = @"HumanID = " + sHumanID + @", status = ""Error"", ErrorDescription = ""Error in processing the request. """ + ex.Message;
                ilstCDC_Audit_Log[0].Modified_By = "Acurus";
                ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
                cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
                return Json(new { HumanID = sHumanID, status = "Error", ErrorDescription = "Error in processing the request. " + ex.Message });
            }
            ilstCDC_Audit_Log[0].Response = @"HumanID = " + sHumanID + @", status = ""Acknowledged""";
            ilstCDC_Audit_Log[0].Modified_By = "Acurus";
            ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
            cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
            return Json(new { HumanID = sHumanID, status = "Acknowledged" });
        }

        private void GenerateCapellaHistoryData(string sHumanID, bool bIsForce, string sCategory = "")
        {
            try
            {
                var releaseDate = ConfigurationSettings.AppSettings["ReleaseDate"] ?? "";
                var TriggerReleaseDate = ConfigurationSettings.AppSettings["TriggerReleaseDate"] ?? "";
                var thresholdDate = ConfigurationSettings.AppSettings["ThresholdDate"] ?? "";
                //Jira CAP-3577
                string sAllocatedProcessForHydration = ConfigurationSettings.AppSettings["AllocatedProcessForHydration"]?.ToString() ?? "";
                //Jira CAP-3112 - Start
                //if (string.IsNullOrEmpty(sCategory) || sCategory.ToUpper() == "ENCOUNTERS")
                //{
                //    string encounterByHumanIDQury = "SELECT enc.Encounter_ID FROM encounter enc JOIN wf_object wf ON enc.Encounter_ID = wf.Obj_System_Id  WHERE enc.Human_ID = {0} AND  DATE(enc.Date_of_Service) >= '{1}' AND DATE(enc.Date_of_Service) <= '{2}' and wf.Obj_Type = 'DOCUMENTATION' and wf.Current_Process = 'DOCUMENT_COMPLETE' and date(enc.Encounter_Provider_Signed_Date) <> '0001-01-01' UNION ALL SELECT enc.Encounter_ID FROM encounter_arc enc JOIN wf_object_arc wf ON enc.Encounter_ID = wf.Obj_System_Id WHERE enc.Human_ID = {0} AND  DATE(enc.Date_of_Service) >= '{1}' AND DATE(enc.Date_of_Service) <= '{2}' and wf.Obj_Type = 'DOCUMENTATION' and wf.Current_Process = 'DOCUMENT_COMPLETE' and date(enc.Encounter_Provider_Signed_Date) <> '0001-01-01';";

                //    DataSet result = DBConnector.ReadData(string.Format(encounterByHumanIDQury, sHumanID, thresholdDate, releaseDate));

                //    if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                //    {
                //        IList<string> lstEncounterIDs = new List<string>();
                //        if (bIsForce == false)
                //        {
                //            string CheckCDCTabel = "select group_concat(Encounter_id) as Encounter_IDs from cdc_progress_note where Human_id = {0};";
                //            DataSet Checkresult = DBConnector.ReadData(string.Format(CheckCDCTabel, sHumanID));
                //            string sEncounterIDs = (Checkresult.Tables[0].Rows.Count > 0) ? Checkresult.Tables[0].Rows[0]["Encounter_IDs"].ToString() : "";
                //            lstEncounterIDs = sEncounterIDs.Split(',').ToList();
                //        }

                //        foreach (DataRow row in result.Tables[0].Rows)
                //        {
                //            string encounter_ID = row["Encounter_ID"].ToString();
                //            if (lstEncounterIDs.Contains(encounter_ID))
                //            {
                //                continue;
                //            }
                //            Task.Run(() => { GenerateJsonNotes(sHumanID, encounter_ID, "Acurus", DateTime.UtcNow); });
                //            //GenerateJsonNotes(sHumanID, encounter_ID, "Acurus", DateTime.UtcNow);
                //        }
                //    }
                //    //else
                //    //{
                //    //    return Json(new { HumanID = sHumanID, status = "ValidationError", ErrorDescription = "Encounter is not present in DB. Cannot generate progress note." });
                //    //}
                //}
                //Jira CAP-3112 - End
                if (string.IsNullOrEmpty(sCategory) || sCategory.ToUpper() == "FILES")
                {
                    //Jira CAP-3057
                    //string fileManagementIndexQury = "SELECT File_Management_Index_ID FROM file_management_index WHERE Human_ID = {0} AND Is_Delete != 'Y' AND DATE(Created_Date_And_Time) >= '{1}' AND DATE(Created_Date_And_Time) <= '{2}';";

                    //DataSet result = DBConnector.ReadData(string.Format(fileManagementIndexQury, sHumanID, thresholdDate, releaseDate));

                    //string fileManagementIndexQury = "SELECT File_Management_Index_ID FROM file_management_index WHERE Human_ID = {0} AND Is_Delete != 'Y';";

                    //DataSet result = DBConnector.ReadData(string.Format(fileManagementIndexQury, sHumanID));

                    string fileManagementIndexQury = "SELECT File_Management_Index_ID FROM file_management_index WHERE Human_ID = {0} AND Is_Delete != 'Y' AND DATE(Created_Date_And_Time) >= '{1}' AND DATE(Created_Date_And_Time) <= '{2}';";

                    DataSet result = DBConnector.ReadData(string.Format(fileManagementIndexQury, sHumanID, thresholdDate, TriggerReleaseDate));


                    if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                    {
                        IList<string> lstEntityIDs = new List<string>();
                        if (bIsForce == false)
                        {
                            string CheckCDCTabel = "select group_concat(Entity_ID) as EntityIDs  from cdc_entity_tracker where Human_ID = {0} and Entity_Name = 'Files';";
                            DataSet Checkresult = DBConnector.ReadData(string.Format(CheckCDCTabel, sHumanID));
                            string sEntityIDs = (Checkresult.Tables[0].Rows.Count > 0) ? Checkresult.Tables[0].Rows[0]["EntityIDs"].ToString() : "";
                            lstEntityIDs = sEntityIDs.Split(',').ToList();
                        }

                        foreach (DataRow row in result.Tables[0].Rows)
                        {
                            string file_Management_Index_ID = row["File_Management_Index_ID"].ToString();

                            if (lstEntityIDs.Contains(file_Management_Index_ID))
                            {
                                continue;
                            }
                            //Task.Run(() => { GenerateCDCEntity(sHumanID, file_Management_Index_ID, "Files", "Acurus", DateTime.UtcNow); });
                            GenerateCDCEntity(sHumanID, file_Management_Index_ID, "Files", "Acurus", DateTime.UtcNow);
                        }
                    }
                    //else
                    //{
                    //    return Json(new { HumanID = sHumanID, status = "ValidationError", ErrorDescription = "Image is not present in DB. Cannot generate progress note." });
                    //}
                }

                if (string.IsNullOrEmpty(sCategory) || sCategory.ToUpper() == "LABRESULTS")
                {
                    //Jira CAP-3057
                    //string resultMasterQury = "SELECT Result_Master_ID FROM result_master WHERE Matching_Patient_ID = {0} and File_Name != '' AND DATE(Created_Date_And_Time) >= '{1}' AND DATE(Created_Date_And_Time) <= '{2}';";

                    //DataSet result = DBConnector.ReadData(string.Format(resultMasterQury, sHumanID, thresholdDate, releaseDate));

                    //string resultMasterQury = "SELECT Result_Master_ID FROM result_master WHERE Matching_Patient_ID = {0} and File_Name != '';";

                    //DataSet result = DBConnector.ReadData(string.Format(resultMasterQury, sHumanID));

                    string resultMasterQury = "SELECT Result_Master_ID FROM result_master WHERE Matching_Patient_ID = {0} and File_Name != '' AND DATE(Created_Date_And_Time) >= '{1}' AND DATE(Created_Date_And_Time) <= '{2}';";

                    DataSet result = DBConnector.ReadData(string.Format(resultMasterQury, sHumanID, thresholdDate, TriggerReleaseDate));

                    if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                    {
                        IList<string> lstEntityIDs = new List<string>();
                        if (bIsForce == false)
                        {
                            string CheckCDCTabel = "select group_concat(Entity_ID) as EntityIDs from cdc_entity_tracker where Human_ID = {0} and Entity_Name = 'LabResults';";
                            DataSet Checkresult = DBConnector.ReadData(string.Format(CheckCDCTabel, sHumanID));
                            string sEntityIDs = (Checkresult.Tables[0].Rows.Count > 0) ? Checkresult.Tables[0].Rows[0]["EntityIDs"].ToString() : "";
                            lstEntityIDs = sEntityIDs.Split(',').ToList();
                        }

                        foreach (DataRow row in result.Tables[0].Rows)
                        {
                            string result_Master_Id = row["Result_Master_ID"].ToString();

                            if (lstEntityIDs.Contains(result_Master_Id))
                            {
                                continue;
                            }

                            //Task.Run(() => { GenerateCDCEntity(sHumanID, result_Master_Id, "LabResults", "Acurus", DateTime.UtcNow); });
                            GenerateCDCEntity(sHumanID, result_Master_Id, "LabResults", "Acurus", DateTime.UtcNow);
                        }
                    }
                    //else
                    //{
                    //    return Json(new { HumanID = sHumanID, status = "ValidationError", ErrorDescription = "Lab result is not present in DB. Cannot generate progress note." });
                    //}
                }

                //Jira CAP-3112
                if (string.IsNullOrEmpty(sCategory) || sCategory.ToUpper() == "ENCOUNTERS")
                {
                    //Jira CAP-3577
                    //string encounterByHumanIDQury = "SELECT enc.Encounter_ID FROM encounter enc JOIN wf_object wf ON enc.Encounter_ID = wf.Obj_System_Id  WHERE enc.Human_ID = {0} AND  DATE(enc.Date_of_Service) >= '{1}' AND DATE(enc.Date_of_Service) <= '{2}' and wf.Obj_Type = 'DOCUMENTATION' and wf.Current_Process = 'DOCUMENT_COMPLETE' and date(enc.Encounter_Provider_Signed_Date) <> '0001-01-01' UNION ALL SELECT enc.Encounter_ID FROM encounter_arc enc JOIN wf_object_arc wf ON enc.Encounter_ID = wf.Obj_System_Id WHERE enc.Human_ID = {0} AND  DATE(enc.Date_of_Service) >= '{1}' AND DATE(enc.Date_of_Service) <= '{2}' and wf.Obj_Type = 'DOCUMENTATION' and wf.Current_Process = 'DOCUMENT_COMPLETE' and date(enc.Encounter_Provider_Signed_Date) <> '0001-01-01';";
                    string encounterByHumanIDQury = "SELECT enc.Encounter_ID FROM encounter enc JOIN wf_object wf ON enc.Encounter_ID = wf.Obj_System_Id  WHERE enc.Human_ID = {0} AND  DATE(enc.Date_of_Service) >= '{1}' AND DATE(enc.Date_of_Service) <= '{2}' and wf.Obj_Type = 'DOCUMENTATION' and wf.Current_Process in ('"+ sAllocatedProcessForHydration.Replace("|","','") + "') and date(enc.Encounter_Provider_Signed_Date) <> '0001-01-01' UNION ALL SELECT enc.Encounter_ID FROM encounter_arc enc JOIN wf_object_arc wf ON enc.Encounter_ID = wf.Obj_System_Id WHERE enc.Human_ID = {0} AND  DATE(enc.Date_of_Service) >= '{1}' AND DATE(enc.Date_of_Service) <= '{2}' and wf.Obj_Type = 'DOCUMENTATION' and wf.Current_Process in ('"+ sAllocatedProcessForHydration.Replace("|", "','") + "') and date(enc.Encounter_Provider_Signed_Date) <> '0001-01-01';";

                    DataSet result = DBConnector.ReadData(string.Format(encounterByHumanIDQury, sHumanID, thresholdDate, releaseDate));

                    if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                    {
                        IList<string> lstEncounterIDs = new List<string>();
                        if (bIsForce == false)
                        {
                            string CheckCDCTabel = "select group_concat(Encounter_id) as Encounter_IDs from cdc_progress_note where Human_id = {0};";
                            DataSet Checkresult = DBConnector.ReadData(string.Format(CheckCDCTabel, sHumanID));
                            string sEncounterIDs = (Checkresult.Tables[0].Rows.Count > 0) ? Checkresult.Tables[0].Rows[0]["Encounter_IDs"].ToString() : "";
                            lstEncounterIDs = sEncounterIDs.Split(',').ToList();
                        }

                        foreach (DataRow row in result.Tables[0].Rows)
                        {
                            string encounter_ID = row["Encounter_ID"].ToString();
                            if (lstEncounterIDs.Contains(encounter_ID))
                            {
                                continue;
                            }
                            Task.Run(() => { GenerateJsonNotes(sHumanID, encounter_ID, "Acurus", DateTime.UtcNow); });
                            //GenerateJsonNotes(sHumanID, encounter_ID, "Acurus", DateTime.UtcNow);
                        }
                    }
                    //else
                    //{
                    //    return Json(new { HumanID = sHumanID, status = "ValidationError", ErrorDescription = "Encounter is not present in DB. Cannot generate progress note." });
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in processing the request. " + ex.Message.ToString());
                //return Json(new { HumanID = sHumanID, status = "Error", ErrorDescription = "Error in processing the request. " + ex.Message });
            }
        }

        [HttpGet]
        public IHttpActionResult LoadProgressNotes(string sHumanID, string sEncounterID, string transactionBy, DateTime transactionDateTime)
        {
            HumanManager humanManager = new HumanManager();
            Human objHuman = new Human();
            string sAuthorizedLegalOrg = ConfigurationSettings.AppSettings["LegalOrgForHydration"]?.ToString() ?? "";
            //Jira CAP-3577
            string sAllocatedProcessForHydration = ConfigurationSettings.AppSettings["AllocatedProcessForHydration"]?.ToString() ?? "";
            
            CDC_Audit_LogManager cDC_Audit_LogManager = new CDC_Audit_LogManager();
            IList<CDC_Audit_Log> ilstCDC_Audit_Log = new List<CDC_Audit_Log>();
            ilstCDC_Audit_Log.Add(new CDC_Audit_Log());
            ilstCDC_Audit_Log[0].Human_ID = Convert.ToUInt64(sHumanID ?? "0");
            ilstCDC_Audit_Log[0].Encounter_ID = Convert.ToUInt64(sEncounterID ?? "0");
            ilstCDC_Audit_Log[0].API_Name = "LoadProgressNotes";
            ilstCDC_Audit_Log[0].Request = Request.RequestUri.ToString();
            ilstCDC_Audit_Log[0].Created_By = "Acurus";
            ilstCDC_Audit_Log[0].Created_Date_And_Time = DateTime.UtcNow;
            cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);

            try
            {
                if (!VerifyToken())
                {
                    ilstCDC_Audit_Log[0].Response = @"HumanID=" + sHumanID + @",EncounterID=" + sEncounterID + ",Status=Unauthorized";
                    ilstCDC_Audit_Log[0].Modified_By = "Acurus";
                    ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
                    cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
                    return Unauthorized();
                }

                if (sEncounterID == "")
                {
                    ilstCDC_Audit_Log[0].Response = @"HumanID = " + sHumanID + @", EncounterID = " + sEncounterID + @", status = ""ValidationError"", ErrorDescription = ""EncounterID is not valid. Cannot generate progress note.""";
                    ilstCDC_Audit_Log[0].Modified_By = "Acurus";
                    ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
                    cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
                    return Json(new { HumanID = sHumanID, EncounterID = sEncounterID, status = "ValidationError", ErrorDescription = "EncounterID is not valid. Cannot generate progress note." });
                }
                //Jira CAP-3519
                if (sHumanID == "" || sHumanID == "0")
                {
                    ilstCDC_Audit_Log[0].Response = @"HumanID = " + sHumanID + @", EncounterID = " + sEncounterID + @", status = ""ValidationError"", ErrorDescription = ""HumanID is not valid. Cannot generate progress note.""";
                    ilstCDC_Audit_Log[0].Modified_By = "Acurus";
                    ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
                    cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
                    return Json(new { HumanID = sHumanID, EncounterID = sEncounterID, status = "ValidationError", ErrorDescription = "HumanID is not valid. Cannot generate progress note." });
                }
                objHuman = humanManager.GetHumanFromHumanID(Convert.ToUInt64(sHumanID));
                if (!(sAuthorizedLegalOrg.Split(',')).Contains(objHuman.Legal_Org))
                {
                    ilstCDC_Audit_Log[0].Response = @"HumanID = " + sHumanID + @", status = ""ValidationError"", ErrorDescription = ""This patient is not part of " + sAuthorizedLegalOrg + @" Legal Org. Cannot generate progress note.""";
                    ilstCDC_Audit_Log[0].Modified_By = "Acurus";
                    ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
                    cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
                    return Json(new { HumanID = sHumanID, status = "ValidationError", ErrorDescription = "This patient is not part of " + sAuthorizedLegalOrg + " Legal Org. Cannot generate progress note." });
                }
                //CAP-3112
                //if (!CapellaTaskManager.TryStartTask("", sEncounterID))
                //{
                //    return Json(new { HumanID = sHumanID, EncounterID = sEncounterID, status = "InProgress", ErrorDescription = "Request is already InProgress." });
                //}

                //string encounterByHumanIDQury = "SELECT enc.Encounter_ID, wf.Current_Process FROM encounter enc JOIN wf_object wf ON enc.Encounter_ID = wf.Obj_System_Id WHERE enc.Encounter_ID = " + sEncounterID + " AND wf.Obj_Type = 'DOCUMENTATION' UNION ALL SELECT enc.Encounter_ID, wf.Current_Process FROM encounter_arc enc JOIN wf_object_arc wf ON enc.Encounter_ID = wf.Obj_System_Id WHERE enc.Encounter_ID = " + sEncounterID + " AND wf.Obj_Type = 'DOCUMENTATION';";

                string encounterByHumanIDQury = "SELECT enc.Encounter_ID, wf.Current_Process,enc.Encounter_Provider_Signed_Date FROM encounter enc JOIN wf_object wf ON enc.Encounter_ID = wf.Obj_System_Id WHERE enc.Encounter_ID = " + sEncounterID + " AND wf.Obj_Type = 'DOCUMENTATION';";

                DataSet result = DBConnector.ReadData(encounterByHumanIDQury);

                if (result?.Tables != null && result.Tables[0].Rows.Count == 0)
                {
                    //For Archive encounter
                    string encounterByHumanIDQury_arc = "SELECT enc.Encounter_ID, wf.Current_Process, enc.Encounter_Provider_Signed_Date FROM encounter_arc enc JOIN wf_object_arc wf ON enc.Encounter_ID = wf.Obj_System_Id WHERE enc.Encounter_ID = " + sEncounterID + " AND wf.Obj_Type = 'DOCUMENTATION';";

                    result = DBConnector.ReadData(encounterByHumanIDQury_arc);
                }


                if (result?.Tables == null || result.Tables[0].Rows.Count == 0)
                {
                    ilstCDC_Audit_Log[0].Response = @"HumanID = " + sHumanID + @", EncounterID = " + sEncounterID + @", status = ""ValidationError"", ErrorDescription = ""EncounterID is not present in DB. Cannot generate progress note.""";
                    ilstCDC_Audit_Log[0].Modified_By = "Acurus";
                    ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
                    cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
                    return Json(new { HumanID = sHumanID, EncounterID = sEncounterID, status = "ValidationError", ErrorDescription = "EncounterID is not present in DB. Cannot generate progress note." });
                }

                string current_Process = result.Tables[0].Rows[0]["Current_Process"].ToString();
                string Encounter_Provider_SignDate = Convert.ToDateTime(result.Tables[0].Rows[0]["Encounter_Provider_Signed_Date"]).ToString("yyyy-MM-dd");
                //Jira CAP-3577
                //if (current_Process != "DOCUMENT_COMPLETE")
                if (!sAllocatedProcessForHydration.Split('|').Contains(current_Process))
                {
                    ilstCDC_Audit_Log[0].Response = @"HumanID = " + sHumanID + @", EncounterID = " + sEncounterID + @", status = ""ValidationError"", ErrorDescription = ""Encounter is not in DOCUMENT_COMPLETE. Cannot generate progress note.""";
                    ilstCDC_Audit_Log[0].Modified_By = "Acurus";
                    ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
                    cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
                    return Json(new { HumanID = sHumanID, EncounterID = sEncounterID, status = "ValidationError", ErrorDescription = "Encounter is not in DOCUMENT_COMPLETE. Cannot generate progress note." });
                }
                else if (Encounter_Provider_SignDate.Contains("0001-01-01"))
                {
                    ilstCDC_Audit_Log[0].Response = @"HumanID = " + sHumanID + @", EncounterID = " + sEncounterID + @", status = ""ValidationError"", ErrorDescription = ""Encounter is not Signed in Capella. Cannot generate progress note.""";
                    ilstCDC_Audit_Log[0].Modified_By = "Acurus";
                    ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
                    cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
                    return Json(new { HumanID = sHumanID, EncounterID = sEncounterID, status = "ValidationError", ErrorDescription = "Encounter is not Signed in Capella. Cannot generate progress note." });
                }
                //CAP-3112
                Task.Run(() =>
                {
                    //try
                    //{
                        GenerateJsonNotes(sHumanID, sEncounterID, transactionBy, transactionDateTime);
                    //}
                    //finally
                    //{
                        //CapellaTaskManager.EndTask("", sEncounterID);
                    //}
                });
            }
            catch (Exception ex)
            {
                ilstCDC_Audit_Log[0].Response = @"HumanID = " + sHumanID + @", EncounterID = " + sEncounterID + @", status = ""Acknowledged"", ErrorDescription = ""Error in processing the request. " + ex.Message + @"""";
                ilstCDC_Audit_Log[0].Modified_By = "Acurus";
                ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
                cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
                return Json(new { HumanID = sHumanID, EncounterID = sEncounterID, status = "Error", ErrorDescription = "Error in processing the request. " + ex.Message });
            }
            ilstCDC_Audit_Log[0].Response = @"HumanID = " + sHumanID + @", EncounterID = " + sEncounterID + @", status = ""Acknowledged""";
            ilstCDC_Audit_Log[0].Modified_By = "Acurus";
            ilstCDC_Audit_Log[0].Modified_Date_And_Time = DateTime.UtcNow;
            cDC_Audit_LogManager.SaveCDC_Audit_LogWithTransaction(ilstCDC_Audit_Log, string.Empty);
            return Json(new { HumanID = sHumanID, EncounterID = sEncounterID, status = "Acknowledged" });
        }

        private void GenerateJsonNotes(string sHumanID, string sEncounterID, string transactionBy, DateTime transactionDateTime)
        {
        lnGenerateJsonNotes:

            IList<Blob_Progress_Note> ilstBlob_Progress_Note = new List<Blob_Progress_Note>();
            BlobProgressNoteManager BlobProgressNoteMngr = new BlobProgressNoteManager();
            EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
            IList<Encounter_Blob> ilstEncounterBlob = new List<Encounter_Blob>();

            HumanBlobManager HumanBlobMngr = new HumanBlobManager();
            IList<Human_Blob> ilstHumanBlob = new List<Human_Blob>();
            Blob_Progress_Note objBlobProgressNotesInitiated = new Blob_Progress_Note();
            //ilstBlob_Progress_Note = BlobProgressNoteMngr.GetBlobProgressNotes(Convert.ToUInt64(sEncounterID));
            string blobProgressNotesQry = "SELECT Encounter_ID AS Id, Human_ID, Progress_Note_Json, Status, Error_Description, Created_By, Created_Date_And_Time, Modified_By, Modified_Date_And_Time, Version FROM cdc_progress_note WHERE Encounter_ID = {0};";
            DataSet BlobProgressNotesResult = DBConnector.ReadData(string.Format(blobProgressNotesQry, sEncounterID));
            ilstBlob_Progress_Note = DBConnector.DataTableToList<Blob_Progress_Note>(BlobProgressNotesResult.Tables[0]) ?? new List<Blob_Progress_Note>();
            bool isModified = false;
            try
            {
                //Initiate save call
                if (ilstBlob_Progress_Note.Count == 0)
                {
                    ilstBlob_Progress_Note.Add(objBlobProgressNotesInitiated);
                    ilstBlob_Progress_Note[0].Created_By = transactionBy;
                    ilstBlob_Progress_Note[0].Created_Date_And_Time = transactionDateTime;
                    isModified = false;
                }
                else
                {
                    ilstBlob_Progress_Note[0].Modified_By = transactionBy;
                    ilstBlob_Progress_Note[0].Modified_Date_And_Time = transactionDateTime;
                    isModified = true;
                }

                ilstBlob_Progress_Note[0].Id = Convert.ToUInt64(sEncounterID);
                ilstBlob_Progress_Note[0].Human_ID = Convert.ToUInt64(sHumanID);
                ilstBlob_Progress_Note[0].Progress_Note_Json = null;
                ilstBlob_Progress_Note[0].Status = "Initiated";
                ilstBlob_Progress_Note[0].Error_Description = string.Empty;

                BlobProgressNoteMngr.SaveBlobProgressNotesWithTransaction(ilstBlob_Progress_Note, string.Empty);
                //End

                if (sEncounterID != "")
                {
                    //ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(Convert.ToUInt64(sEncounterID));
                    string encounterBlobQry = "SELECT Encounter_XML,Human_XML FROM blob_encounter WHERE Encounter_ID = {0};";
                    DataSet encounterBlobResult = DBConnector.ReadData(string.Format(encounterBlobQry, sEncounterID));
                    ilstEncounterBlob = DBConnector.DataTableToList<Encounter_Blob>(encounterBlobResult.Tables[0]);
                }
                //if (sHumanID != "")
                //{
                //    ilstHumanBlob = HumanBlobMngr.GetHumanBlob(Convert.ToUInt64(sHumanID));
                //}
                string sTabMode = "false";
                string sIsPhoneEncounter = "N";
                string sXMLEncounterDoc = string.Empty;
                string sXMLHumanDoc = string.Empty;
                string sFinalOutPut = string.Empty;
                XmlDocument xmlEncounterDoc = new XmlDocument();
                XmlDocument xmlHumanDoc = new XmlDocument();
                UtilityManager utilitymngr = new UtilityManager();

                if (ilstEncounterBlob.Count > 0)
                {
                    sXMLEncounterDoc = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                    //Jira CAP-3393
                    sXMLEncounterDoc = UtilityManager.ReplaceHexadecimal(sXMLEncounterDoc);
                    if (sXMLEncounterDoc.Substring(0, 1) != "<")
                        sXMLEncounterDoc = sXMLEncounterDoc.Substring(1, sXMLEncounterDoc.Length - 1);
                    //Jira #CAP-115
                    sXMLEncounterDoc = UtilityManager.ReplaceSpecialCharaters(sXMLEncounterDoc);
                    xmlEncounterDoc.LoadXml(sXMLEncounterDoc);
                    sIsPhoneEncounter = (xmlEncounterDoc.SelectSingleNode("notes/Modules/EncounterList/Encounter")?.Attributes.GetNamedItem("Is_Phone_Encounter")?.Value.ToUpper()) ?? "N";
                }

                //sIsPhoneEncounter = xmlEncounterDoc.SelectSingleNode("notes/Modules/EncounterList/Encounter").Attributes.GetNamedItem("Is_Phone_Encounter").Value.ToUpper();

                //string objectSystemIdQry = "SELECT Current_Process FROM WF_Object WHERE Obj_System_Id = {0} AND Obj_Type = 'DOCUMENTATION' UNION ALL SELECT Current_Process FROM WF_Object_arc WHERE Obj_System_Id = {0} AND Obj_Type = 'DOCUMENTATION';";
                //DataSet ObjectSystemResult = DBConnector.ReadData(string.Format(objectSystemIdQry, sEncounterID));
                //WFObject DocumentationWfObject = new WFObject();
                //DocumentationWfObject = DBConnector.DataTableToList<WFObject>(ObjectSystemResult.Tables[0]).FirstOrDefault() ?? new WFObject();

                //if (DocumentationWfObject.Current_Process == "DOCUMENT_COMPLETE" || sIsPhoneEncounter.ToUpper() == "Y")
                //{
                if (ilstEncounterBlob != null && ilstEncounterBlob.Count > 0 && ilstEncounterBlob[0].Human_XML != null)
                {
                    sXMLHumanDoc = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Human_XML);
                    //Jira CAP-3393
                    sXMLHumanDoc = UtilityManager.ReplaceHexadecimal(sXMLHumanDoc);
                }
                else
                {
                    string qryHumanBlob = "SELECT * FROM blob_human where Human_ID = {0};";
                    DataSet HumanBlobResult = DBConnector.ReadData(string.Format(qryHumanBlob, sHumanID));
                    ilstHumanBlob = DBConnector.DataTableToList<Human_Blob>(HumanBlobResult.Tables[0]);
                    //ilstHumanBlob = HumanBlobMngr.GetHumanBlob(Convert.ToUInt64(sHumanID));
                    if (ilstHumanBlob != null && ilstHumanBlob.Count > 0 && ilstHumanBlob[0].Human_XML != null)
                    {
                        sXMLHumanDoc = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                        //Jira CAP-3393
                        sXMLHumanDoc = UtilityManager.ReplaceHexadecimal(sXMLHumanDoc);
                    }
                }




                if (sXMLHumanDoc != null && sXMLHumanDoc != "" && sXMLHumanDoc != string.Empty)
                {
                    //sXMLHumanDoc = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                    if (sXMLHumanDoc.Substring(0, 1) != "<")
                        sXMLHumanDoc = sXMLHumanDoc.Substring(1, sXMLHumanDoc.Length - 1);
                    //Jira #CAP-115
                    sXMLHumanDoc = UtilityManager.ReplaceSpecialCharaters(sXMLHumanDoc);
                    xmlHumanDoc.LoadXml(sXMLHumanDoc);
                }

                string xsltFile = string.Empty;
                if (sIsPhoneEncounter != null && sIsPhoneEncounter == "Y")
                {// jira cap-499
                    sIsPhoneEncounter = "Y";
                    xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Phone_Encounter_Notes.xsl");
                }
                else
                {
                    xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Progress_Notes.xsl");
                }

                string WordOutputName = sEncounterID + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".html";
                string outputDocument = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], WordOutputName);

                string htmlString = string.Empty;
                htmlString = UtilityManager.PrintPDFUsingXSLT(sXMLEncounterDoc, sXMLHumanDoc, xsltFile, outputDocument, "");
                System.IO.FileInfo file = new System.IO.FileInfo(outputDocument);



                string Encounter_signedDate = "";
                string Encounter_signed_UserId = "";
                string Encounter_Provider_Name = "";
                string Encounter_Reviewed_signedDate = "";
                string Encounter_Reviewed_Name = "";
                string Encounter_Reviewed_Id = "";
                string sIsphoneEncounter = "";
                string sCreatedBy = "";

                TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
                XDocument xmlDocumentType = XDocument.Load(EncXMLContent);
                foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
                {
                    foreach (XElement Encounter in elements.Elements())
                    {
                        Encounter_Reviewed_signedDate = Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value;
                        if (Encounter_Reviewed_signedDate != "0001-01-01 12:00:00 AM")
                        {
                            Encounter_Reviewed_signedDate = ConvertToLocal(Encounter_Reviewed_signedDate);
                        }
                        Encounter_signedDate = Encounter.Attribute("Encounter_Provider_Signed_Date").Value;
                        if (Encounter_signedDate != "0001-01-01 12:00:00 AM")
                        {
                            Encounter_signedDate = ConvertToLocal(Encounter_signedDate);
                        }

                        Encounter_Reviewed_Id = Encounter.Attribute("Encounter_Provider_Review_ID").Value;
                        sIsphoneEncounter = Encounter.Attribute("Is_Phone_Encounter").Value;
                        sCreatedBy = Encounter.Attribute("Created_By").Value;
                        Encounter_signed_UserId = Encounter.Attribute("Encounter_Provider_ID").Value;
                    }

                    //if (Encounter_signedDate == "" || Encounter_signedDate == "01-Jan-0001 12:00:00 AM")
                    //{
                    //    foreach (XElement Encounter in elements.Elements())
                    //    {
                    //        Encounter_signedDate = ConvertToLocal(Encounter.Attribute("Encounter_Provider_Signed_Date").Value).ToString("dd-MMM-yyyy hh:mm:ss tt");
                    //    }
                    //}
                }
                //Provider Name 
                foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
                {
                    foreach (XElement Encounter in elements.Elements())
                    {
                        Encounter_Provider_Name = Encounter.Value;
                        break;
                    }
                    break;
                }
                //Provider Reviewed Name 
                if (Encounter_Reviewed_Id != "")
                {
                    //CAP-2788
                    UserList ilstUserList = ConfigureBase<UserList>.ReadJson("User.json");
                    if (ilstUserList?.User != null)
                    {
                        var filteredData = ilstUserList?.User.FirstOrDefault(a => a.Physician_Library_ID.ToString() != "0" && a.Physician_Library_ID.ToString() == Encounter_Reviewed_Id);
                        if (filteredData != null)
                        {
                            Encounter_Reviewed_Name = filteredData.person_name;
                        }
                    }
                }

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(sXMLEncounterDoc);

                string sPhysicianID = xmldoc.SelectSingleNode("notes/Modules/EncounterList/Encounter").Attributes.GetNamedItem("Encounter_Provider_ID").Value;
                string sEncounterIDForPatientDetails = (xmldoc.SelectSingleNode("notes/Modules/EncounterList/Encounter")?.Attributes?.GetNamedItem("Id")?.Value != null) ?
                    xmldoc.SelectSingleNode("notes/Modules/EncounterList/Encounter")?.Attributes?.GetNamedItem("Id")?.Value :
                    (xmldoc.SelectSingleNode("notes/Modules/EncounterList/Encounter")?.Attributes?.GetNamedItem("Encounter_ID")?.Value != null) ?
                    xmldoc.SelectSingleNode("notes/Modules/EncounterList/Encounter")?.Attributes?.GetNamedItem("Encounter_ID")?.Value : "";
                UserManager Usermngr = new UserManager();
                IList<User> ilstUser = new List<User>();

                ilstUser = getUserByPHYID(sPhysicianID);
                //ilstUser = Usermngr.getUserByPHYID(Convert.ToUInt64(sPhysicianID));
                string sUserEmailAddr = (ilstUser.Count > 0) ? ilstUser[0].EMail_Address : "";

                //Generate Json
                //string htmlString = System.IO.File.ReadAllText(outputDocument);
                if (System.Configuration.ConfigurationSettings.AppSettings["XsltTransformVersion"] != null
                    && System.Configuration.ConfigurationSettings.AppSettings["XsltTransformVersion"].ToString().ToUpper() == "V1")
                {
                    htmlString = System.IO.File.ReadAllText(outputDocument);
                }
                string xmls = htmlString.Replace("&nbsp;", "").Replace("&bull;", "").Replace("&amp;", "");
                xmls = "<?xml version=\"1.0\" encoding=\"utf-8\"?> <content>" + xmls + "</content>";
                htmlString = htmlString.Replace("<subtab>", "").Replace("</subtab>", "").Replace("<plan>", "").Replace("</plan>", "");
                //Cap - 2508
                while (htmlString.Contains("<AddendumProviderID>"))
                {
                    string NewInput = htmlString.Substring(htmlString.IndexOf("<AddendumProviderID>"), (htmlString.IndexOf("</AddendumProviderID>") - htmlString.IndexOf("<AddendumProviderID>") + 21));
                    string NewInput2 = htmlString.Substring(htmlString.IndexOf("<AddendumPhysicianEMailAddress>"), (htmlString.IndexOf("</AddendumPhysicianEMailAddress>") - htmlString.IndexOf("<AddendumPhysicianEMailAddress>") + 32));
                    //htmlString = htmlString.Replace(NewInput, "").Replace(NewInput2, "");
                    htmlString = ReplaceFirst(htmlString, NewInput, "");
                    htmlString = ReplaceFirst(htmlString, NewInput2, "");
                }
                while (htmlString.Contains("<AddendumReviewProviderID>"))
                {
                    string NewInput = htmlString.Substring(htmlString.IndexOf("<AddendumReviewProviderID>"), (htmlString.IndexOf("</AddendumReviewProviderID>") - htmlString.IndexOf("<AddendumReviewProviderID>") + 27));
                    string NewInput2 = htmlString.Substring(htmlString.IndexOf("<AddendumReviewPhysicianEMailAddress>"), (htmlString.IndexOf("</AddendumReviewPhysicianEMailAddress>") - htmlString.IndexOf("<AddendumReviewPhysicianEMailAddress>") + 38));
                    //htmlString = htmlString.Replace(NewInput, "").Replace(NewInput2, "");
                    htmlString = ReplaceFirst(htmlString, NewInput, "");
                    htmlString = ReplaceFirst(htmlString, NewInput2, "");
                }
                //Cap - 2508 End

                //Jira CAP-3134
                ////Jira CAP-1015
                //xmls = xmls.Replace("<HPINotes>", "</p><p><b>").Replace("</HPINotes>", "</b><br />");
                xmls = xmls.Replace("<HPINotes>", "</p><p><b>").Replace("</HPINotes>", "</b><br />").Replace("<br>", "<br/>");
                htmlString = htmlString.Replace("amp;", "");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmls);
                string sTyepeOfPattern = string.Empty;
                string sSection = string.Empty;
                sFinalOutPut = "{";
                for (int i = 0; i < doc.SelectNodes("content/p").Count; i++)
                {
                    sTyepeOfPattern = string.Empty;
                    sSection = string.Empty;
                    if (doc.SelectSingleNode("content/p[" + i + "]")?.InnerXml != null && doc.SelectSingleNode("content/p[" + i + "]").InnerXml != "")
                    {
                        // This patern for normal screen like CC - Content is present under Secion Name directly
                        if (doc.SelectSingleNode("content/p[" + i + "]/b")?.InnerXml != null
                            && doc.SelectSingleNode("content/p[" + i + "]/i")?.InnerXml == null
                            && doc.SelectSingleNode("content/p[" + i + "]/b/i")?.InnerXml == null
                            && doc.SelectSingleNode("content/p[" + i + "]/table")?.InnerXml == null
                            && doc.SelectSingleNode("content/p[" + i + "]/font/b/i")?.InnerXml == null)
                        {

                            sTyepeOfPattern = "1";
                            sSection = doc.SelectSingleNode("content/p[" + i + "]").InnerXml;
                        }
                        //// This patern Only for Screening and prev screening - Content is present under Section and multiple level sub sections & heading
                        else if (doc.SelectSingleNode("content/p[" + i + "]/i")?.InnerXml != null || doc.SelectSingleNode("content/p[" + i + "]/b/i")?.InnerXml != null)
                        {
                            sTyepeOfPattern = "2";
                            sSection = doc.SelectSingleNode("content/p[" + i + "]").InnerXml;
                        }
                        // This patern Only for Individualized Care Plan - Content is present under Section and multiple level sub sections & heading. But content has exta tags like font
                        else if (doc.SelectSingleNode("content/p[" + i + "]/font/b/i")?.InnerXml != null)
                        {
                            sTyepeOfPattern = "3";
                            sSection = doc.SelectSingleNode("content/p[" + i + "]").InnerXml;
                        }
                        // This pattern for table formate in the progress note - Content is in Table format
                        else if (i != 1 && doc.SelectSingleNode("content/p[" + i + "]/table")?.InnerXml != null)
                        {
                            sTyepeOfPattern = "4";
                            sSection = doc.SelectSingleNode("content/p[" + i + "]").InnerXml;
                        }
                        // This patern for Patient Details - Content is Progress Note header
                        else if (i == 1
                            && doc.SelectSingleNode("content/p[" + i + "]/table/thead/tr/td")?.InnerXml != null
                            && doc.SelectSingleNode("content/p[" + i + "]/table/thead/tr/td").InnerText.Contains("Patient Name"))
                        {

                            XmlNode AdditionalParentElelemnt = doc.CreateElement("tr");
                            XmlNode AdditionalElement = null;
                            IList<string> AdditionalValues = new List<string>();
                            AdditionalValues.Add("EncounterID:" + sEncounterIDForPatientDetails);
                            AdditionalValues.Add("PhysicianID:" + sPhysicianID);
                            AdditionalValues.Add("Physician EMail Address:" + sUserEmailAddr);
                            for (int iCount = 0; iCount < AdditionalValues.Count; iCount++)
                            {
                                AdditionalElement = doc.CreateElement("td");
                                AdditionalElement.InnerText = AdditionalValues[iCount];
                                AdditionalParentElelemnt.AppendChild(AdditionalElement);
                            }

                            sTyepeOfPattern = "5";
                            doc.SelectSingleNode("content/p[" + i + "]/table/thead").AppendChild(AdditionalParentElelemnt);
                            sSection = doc.SelectSingleNode("content/p[" + i + "]").InnerXml;
                        }

                        //Final Json formation
                        string sSectionOutPut = string.Empty;
                        if (sSection != string.Empty)
                        {
                            sSectionOutPut = GenerateJson(sSection, sTyepeOfPattern);
                        }
                        if (sSectionOutPut != string.Empty)
                        {
                            sFinalOutPut = sFinalOutPut + ((sFinalOutPut.Length > 1) ? "," : "") + sSectionOutPut;
                        }
                    }
                }

                string strfooterProvider = "";
                string strSignedBy = "";
                string strSignedAt = "";
                string strSignedUserId = Encounter_signed_UserId;
                string strReviewedBy = "";
                string strReviewedAt = "";
                string strProviderUserId = Encounter_Reviewed_Id;
                string strSignedUserEmail = "";
                string strReviewedUserEmail = "";

                if (!string.IsNullOrWhiteSpace(strSignedUserId))
                {
                    strSignedUserEmail = getUserByPHYID(strSignedUserId).FirstOrDefault()?.EMail_Address ?? "";
                }

                if (!string.IsNullOrWhiteSpace(strProviderUserId) && strProviderUserId != "0")
                {
                    strReviewedUserEmail = getUserByPHYID(strProviderUserId).FirstOrDefault()?.EMail_Address ?? "";
                }

                strProviderUserId = !string.IsNullOrWhiteSpace(strReviewedUserEmail) ? strProviderUserId : "";

                if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && sIsphoneEncounter != "Y")
                {
                    strSignedAt = Encounter_signedDate;
                    strSignedBy = Encounter_Provider_Name;
                    strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
                }
                else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && sIsphoneEncounter == "Y")
                {
                    strSignedAt = Encounter_signedDate;
                    if (Encounter_Provider_Name != "")
                    {
                        strSignedBy = Encounter_Provider_Name;
                        strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
                    }
                    else
                    {
                        strSignedBy = sCreatedBy;
                        strfooterProvider = "Electronically Signed by " + sCreatedBy + " at " + Encounter_signedDate;
                    }
                }
                //string strfooterProviderReviewed = "I " + Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate +
                //     " have reviewed the chart and agree with the management plan with the changes to the plan as indicated.";

                //string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
                //StaticLookupManager staticMngr = new StaticLookupManager();
                string strfooterProviderReviewed = string.Empty;

                string qryStaticLookupByFieldName = "SELECT * FROM static_lookup WHERE Field_Name = 'WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES';";
                DataSet StaticLookupByFieldNameResult = DBConnector.ReadData(qryStaticLookupByFieldName);
                IList<StaticLookup> CommonList = DBConnector.DataTableToList<StaticLookup>(StaticLookupByFieldNameResult.Tables[0]) ?? new List<StaticLookup>();
                //IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
                if (CommonList.Count > 0)
                {
                    strfooterProviderReviewed = CommonList[0].Value.Replace("<Physician>", Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate).Replace("|", "");
                }


                if (file.Exists)
                {
                    File.Delete(outputDocument);
                }
                var strBody = new StringBuilder();

                string sbTop = "";

                sbTop = sbTop + htmlString;



                string strfooterF = "";

                string strfooterPA = "";
                string strfooterP = "";
                string strFooterJson = "";

                if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && Encounter_Reviewed_signedDate != "" && Encounter_Reviewed_signedDate != "01-Jan-0001 12:00 AM" && Encounter_Reviewed_signedDate != "0001-01-01 12:00:00 AM")
                {
                    strReviewedAt = Encounter_Reviewed_signedDate;
                    strReviewedBy = Encounter_Reviewed_Name;

                    strfooterPA = strfooterProvider;

                    strfooterP = strfooterProviderReviewed;
                    strfooterF = " ";
                    //  strfooterProvider = strfooterProvider + "<br/>" + strfooterProviderReviewed;
                }
                else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && Encounter_signedDate != "0001-01-01 12:00:00 AM")
                {
                    strfooterF = strfooterProvider;
                }
                else
                {


                    strfooterF = "";

                }
                string sFooter = string.Empty;
                string sFooterNode = ",\"Footer" + "\":[";
                if (strfooterF == "")
                {
                    sFooter = "";
                    //sFinalOutPut = sFinalOutPut + sFooter;
                }
                else if (strfooterPA != "" && strfooterP != "")
                {
                    sFooter = strfooterPA + "  " + strfooterP;
                    //sFinalOutPut = sFinalOutPut + sFooter;
                }
                else
                {
                    sFooter = strfooterF;
                    //sFinalOutPut = sFinalOutPut + sFooter;
                }

                if (strSignedAt != "")
                {
                    strSignedAt = Convert.ToDateTime(strSignedAt).ToString("o");
                }

                sFinalOutPut = sFinalOutPut + sFooterNode
                                    + "{\"" + "text" + "\":\"" + (sFooter?.Trim() ?? "") + "\"," +
                                    "\"" + "signedBy" + "\":\"" + (strSignedBy?.Trim() ?? "") + "\"," +
                                    "\"" + "UserID" + "\":\"" + (strSignedUserEmail ?? "") + "\"," +
                                    "\"" + "ProviderID" + "\":\"" + (strSignedUserId ?? "") + "\"," +
                                    "\"" + "ReviewedBy" + "\":\"" + (strReviewedBy ?? "") + "\"," +
                                    "\"" + "ReviewedUserID" + "\":\"" + (strReviewedUserEmail ?? "") + "\"," +
                                    "\"" + "ReviewedProviderID" + "\":\"" + (strProviderUserId ?? "") + "\"," +
                                    "\"" + "signedAt" + "\":\"" + (strSignedAt?.Trim() ?? "") + "\"}]";

                sFinalOutPut = sFinalOutPut.Replace("<plan />", "").Replace("</plan>", "").Replace("<br />", "").Replace("<br/>", "").Replace("</subtab>", "").Replace("<subtab />", "") + "}";
                //}



                if (sFinalOutPut != string.Empty)
                {
                    ilstBlob_Progress_Note[0].Id = Convert.ToUInt64(sEncounterID);
                    ilstBlob_Progress_Note[0].Human_ID = Convert.ToUInt64(sHumanID);
                    byte[] bytesKeep = null;
                    try
                    {
                        bytesKeep = System.Text.Encoding.Default.GetBytes(sFinalOutPut);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error : " + ex?.Message);
                    }
                    ilstBlob_Progress_Note[0].Status = "Completed";
                    ilstBlob_Progress_Note[0].Progress_Note_Json = bytesKeep;
                    ilstBlob_Progress_Note[0].Error_Description = string.Empty;
                    if (isModified)
                    {
                        ilstBlob_Progress_Note[0].Modified_By = transactionBy;
                        ilstBlob_Progress_Note[0].Modified_Date_And_Time = transactionDateTime;
                    }
                    else
                    {
                        ilstBlob_Progress_Note[0].Created_By = transactionBy;
                        ilstBlob_Progress_Note[0].Created_Date_And_Time = transactionDateTime;
                    }
                    BlobProgressNoteMngr.SaveBlobProgressNotesWithTransaction(ilstBlob_Progress_Note, string.Empty);
                }
                //return "{\"EncounterID\":" + sEncounterID + ",\"status\":\"Completed\",\"Description\":\"Request is available in Blob Progress Note table\"}";
            }
            catch (Exception eex)
            {
                string sStatus = string.Empty;
                sStatus = RegenerateXML(eex, sHumanID, sEncounterID);
                if (sStatus == "Success")
                {
                    goto lnGenerateJsonNotes;
                }
                else if (sStatus != "Time Out")
                {
                    try
                    {
                        ilstBlob_Progress_Note = BlobProgressNoteMngr.GetBlobProgressNotes(Convert.ToUInt64(sEncounterID));
                        ilstBlob_Progress_Note[0].Id = Convert.ToUInt64(sEncounterID);
                        ilstBlob_Progress_Note[0].Human_ID = Convert.ToUInt64(sHumanID);
                        ilstBlob_Progress_Note[0].Progress_Note_Json = null;
                        ilstBlob_Progress_Note[0].Status = "Error";
                        ilstBlob_Progress_Note[0].Error_Description = (sStatus != "") ? "Error : " + sStatus : "Message : " + eex?.Message + "Stack Trace : " + eex?.StackTrace;
                        ilstBlob_Progress_Note[0].Modified_By = "Acurus";
                        ilstBlob_Progress_Note[0].Modified_Date_And_Time = DateTime.UtcNow;
                        BlobProgressNoteMngr.SaveBlobProgressNotesWithTransaction(ilstBlob_Progress_Note, string.Empty);
                    }
                    catch
                    {
                        throw new Exception("Error : " + eex?.Message);
                    }
                }
            }
        }
        public string RegenerateXML(Exception ex, string sHumanID, string sEncounterID)
        {
            string sStatus = string.Empty;
            string sEncounterStatus = string.Empty;
            if ((ex.Message.ToLower().Contains("there is an unclosed literal string") == true || ex.Message.ToLower().Contains("root element is missing") == true || ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true))
            {

                string sResultHuman = string.Empty;
                string sResultEncounter = string.Empty;
                EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();

                IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(Convert.ToUInt64(sEncounterID));

                if (ilstEncounterBlob.Count > 0)
                {
                    //HumanXML
                    string sHumanXMLContent = string.Empty;
                    XmlDocument xmlDoc = new XmlDocument();
                    try
                    {
                        sHumanXMLContent = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Human_XML);
                        if (sHumanXMLContent.Substring(0, 1) != "<")
                            sHumanXMLContent = sHumanXMLContent.Substring(1, sHumanXMLContent.Length - 1);
                        xmlDoc.LoadXml(sHumanXMLContent);
                        sResultHuman = "Success";
                        sStatus = "Success";
                    }
                    catch
                    {
                        sResultHuman = "Failure";
                        sStatus = UtilityManager.GenerateXMLForCDC(sHumanID, "HUMAN", sHumanID, sEncounterID);
                    }

                    if (sStatus == "Success")
                    {
                        //EncounterXML
                        string sXMLContent = string.Empty;
                        xmlDoc = new XmlDocument();
                        try
                        {
                            sXMLContent = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                            if (sXMLContent.Substring(0, 1) != "<")
                                sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                            xmlDoc.LoadXml(sXMLContent);
                            sResultEncounter = "Success";
                        }
                        catch
                        {
                            sResultEncounter = "Failure";
                            sStatus = UtilityManager.GenerateXMLForCDC(sEncounterID, "ENCOUNTER", sHumanID, sEncounterID);
                        }
                    }
                }
                
            }
            return sStatus;

        }
        public string ReplaceFirst(string input, string search, string replacement)
        {
            int pos = input.IndexOf(search);
            if (pos < 0)
                return input;
            return input.Substring(0, pos) + replacement + input.Substring(pos + search.Length);
        }
        private string GenerateJson(string sSection, string sType)
        {
            string[] heading = { "</b><br />" };
            string[] Case2heading = { "<br />" };
            string[] PlanTag = { "<plan>" };
            string[] subtab = { "<subtab>" };
            string[] section = { "<i>" };
            string[] AmendmentSplit = { "<AddendumDelimiter />" };
            string[] AmendmentFillTagSplit = { "<AddendumDelimiter></AddendumDelimiter>" };
            switch (sType)
            {
                case "1":
                    {
                        //Direct page
                        UserManager usermngr = new UserManager();
                        IList<User> ilstUser = new List<User>();
                        string[] PlanTagcontent = sSection.Split(PlanTag, System.StringSplitOptions.RemoveEmptyEntries);

                        IList<string> ilstsection = new List<string>();
                        if (PlanTagcontent[0].Contains("Amendment Notes"))
                        {
                            ilstsection = PlanTagcontent[0].Split(AmendmentSplit, System.StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (ilstsection.Count == 1)
                            {
                                ilstsection = PlanTagcontent[0].Split(AmendmentFillTagSplit, System.StringSplitOptions.RemoveEmptyEntries).ToList();
                            }
                        }
                        else
                        {
                            ilstsection = PlanTagcontent[0].Split(heading, System.StringSplitOptions.RemoveEmptyEntries).ToList();
                        }

                        if (PlanTagcontent.Length > 1)
                        {
                            //Jira CAP-3108
                            //ilstsection.Add(PlanTagcontent[1].Replace("</plan>", "").Replace("<br />", @"\n").Replace("<br/>", @"\n").Replace("\t", ""));
                            ilstsection.Add(PlanTagcontent[1].Replace("</plan>", "").Replace("<br />", @"\n").Replace("<br/>", @"\n").Replace("\t", "").Replace("\"", "'").Replace('"', '\"').Replace("\r\n", @"\n").Replace("\n", @"\n"));
                        }
                        if (ilstsection[0].Contains("Amendment Notes"))
                        {
                            ilstsection.Insert(0, ilstsection[0].Substring(ilstsection[0].IndexOf("<b>"), ilstsection[0].IndexOf("</b>")));
                        }
                        if (ilstsection.Count >= 2)
                        {
                            ilstsection[1] = ilstsection[1].Replace(ilstsection[0]+ "</b>", "");
                        }

                        string[] sectopns = ilstsection.ToArray();
                        string sFormationJson = string.Empty;

                        

                        for (int i = 0; i < sectopns.Length; i++)
                        {
                            if (i == 0 && sectopns[i].Contains("<b>"))
                            {
                                //Key
                                sFormationJson = "\"" + sectopns[i].Replace("<b>", "").Replace("</b>", "").Replace(":", "").Replace("<br />", "") + "\"" + ":[";
                            }
                            //Below condition for Amendment
                            else if (sectopns[0].Replace("<b>", "").Replace("</b>", "").Replace(":", "") == "Amendment Notes")
                            {
                                if (sectopns[i].IndexOf("AddendumReviewProviderID") > -1)
                                {
                                    //Get Notes
                                    string[] sTempNotesName = sectopns[i].Split(new string[] { " AM: ", " PM: " }, System.StringSplitOptions.RemoveEmptyEntries);
                                    string sNotesName = sTempNotesName.Length > 1 ? sTempNotesName[1] : "";
                                    sectopns[i] = sectopns[i].Replace(":" + sNotesName, "");

                                    //Get CreatedAt
                                    string[] sTempCreatedAt = sectopns[i].Split(new string[] { " on " }, System.StringSplitOptions.RemoveEmptyEntries);
                                    string sCreatedAt = sTempCreatedAt.Length > 1 ? sTempCreatedAt[1].Replace(":" + sNotesName, "").Replace(": " + sNotesName, "") : "";
                                    sCreatedAt = (sCreatedAt.IndexOf(" PM: ") > -1) ? sCreatedAt.Replace(sCreatedAt.Substring(sCreatedAt.IndexOf(" PM: ")), "") : sCreatedAt;
                                    sCreatedAt = (sCreatedAt.IndexOf(" AM: ") > -1) ? sCreatedAt.Replace(sCreatedAt.Substring(sCreatedAt.IndexOf(" AM: ")), "") : sCreatedAt;
                                    //Jira CAP-3617
                                    sCreatedAt = (sCreatedAt.IndexOf("PM:") > -1) ? sCreatedAt.Replace(sCreatedAt.Substring(sCreatedAt.IndexOf("PM:")), "PM") : sCreatedAt;
                                    sCreatedAt = (sCreatedAt.IndexOf("AM:") > -1) ? sCreatedAt.Replace(sCreatedAt.Substring(sCreatedAt.IndexOf("AM:")), "AM") : sCreatedAt;
                                    //Jira CAP-3617 - End
                                    //Get ProviderUserID
                                    string sProviderUserID = string.Empty;
                                    if (sectopns[i].IndexOf("<AddendumProviderID>") > -1)
                                    {
                                        Match matchProviderID = Regex.Match(sectopns[i], @"<AddendumProviderID>(\d+)</AddendumProviderID>");
                                        if (matchProviderID.Success)
                                        {
                                            sProviderUserID = matchProviderID.Groups[1].Value;
                                            ilstUser = getUserByPHYID(sProviderUserID);
                                        }
                                    }

                                    string UserID = ilstUser[0].EMail_Address;
                                    string sReviewProviderUserID = string.Empty;
                                    string sReviewPhysicianEmailID = string.Empty;
                                    if (sectopns[i].IndexOf("<AddendumReviewProviderID>") > -1 && sectopns[i].IndexOf("</AddendumReviewProviderID>") > -1)
                                    {
                                        Match matchProviderID = Regex.Match(sectopns[i], @"<AddendumReviewProviderID>(\d+)</AddendumReviewProviderID>");
                                        if (matchProviderID.Success)
                                        {
                                            sReviewProviderUserID = matchProviderID.Groups[1].Value;
                                            ilstUser = getUserByPHYID(sReviewProviderUserID);
                                            sReviewPhysicianEmailID = ilstUser[0].EMail_Address;
                                        }
                                    }


                                    string[] sTempOtherDetails = sectopns[i].Split(new string[] { "Signed by" }, System.StringSplitOptions.RemoveEmptyEntries);
                                    string sOtherDetails = sTempOtherDetails.Length > 0 ? sTempOtherDetails[1] : "";
                                    sTempOtherDetails = sOtherDetails.Split(new string[] { " on " }, System.StringSplitOptions.RemoveEmptyEntries);

                                    string sPAname = string.Empty;
                                    string sPhysician = string.Empty;
                                    if (sOtherDetails.IndexOf("and reviewed by") > 0)
                                    {
                                        sTempOtherDetails = sTempOtherDetails[0].Split(new string[] { "and reviewed by" }, System.StringSplitOptions.RemoveEmptyEntries);
                                        sPAname = sTempOtherDetails[0];
                                        sPhysician = sTempOtherDetails[1];
                                    }
                                    else
                                    {
                                        sPAname = sOtherDetails.Split(new string[] { " on " }, System.StringSplitOptions.RemoveEmptyEntries)[0];
                                    }

                                    if (sCreatedAt != "")
                                    {
                                        sCreatedAt = Convert.ToDateTime(sCreatedAt).ToString("o");
                                    }
                                    sNotesName = sNotesName.Replace("<br />", @"\n").Replace("<br/>", @"\n").Replace("\t", "").Replace('"', '\"');
                                    sNotesName = sNotesName.Substring(0, sNotesName.Length - @"\n".Length);

                                    sFormationJson = sFormationJson + ((sFormationJson[sFormationJson.Length - 1] == '}') ? "," : "")
                                        //Jira CAP-2608
                                        //+ "{\"" + "text" + "\":\"" + sNotesName + "\"," +
                                        + "{\"" + "text" + "\":\"" + sNotesName.Replace("\r\n", @"\n").Replace("\n", @"\n") + "\"," +
                                        "\"" + "createdBy" + "\":\"" + sPAname.TrimEnd() + "\"," +
                                        "\"" + "UserID" + "\":\"" + UserID + "\"," +
                                        "\"" + "ProviderID" + "\":\"" + sProviderUserID + "\"," +
                                        "\"" + "ReviewedBy" + "\":\"" + sPhysician + "\"," +
                                        "\"" + "ReviewedUserID" + "\":\"" + sReviewPhysicianEmailID + "\"," +
                                        "\"" + "ReviewedProviderID" + "\":\"" + sReviewProviderUserID + "\"," +
                                        "\"" + "createdAt" + "\":\"" + sCreatedAt.Trim() + "\"}";
                                }
                                else if (sectopns[i].IndexOf("<AddendumProviderID>") > -1)
                                {
                                    string sProviderEmailID = string.Empty;
                                    string sProviderUserID = string.Empty;
                                    //if (sectopns[i].IndexOf("<AddendumProviderID>") > -1)
                                    {
                                        Match matchProviderID = Regex.Match(sectopns[i], @"<AddendumProviderID>(\d+)</AddendumProviderID>");
                                        if (matchProviderID.Success)
                                        {
                                            sProviderUserID = matchProviderID.Groups[1].Value;
                                            ilstUser = getUserByPHYID(sProviderUserID);
                                        }

                                        //sProviderUserID = sectopns[i].Substring(sectopns[i].IndexOf("<AddendumProviderID>"), sectopns[i].IndexOf("</AddendumProviderID>") + "</AddendumProviderID>".Length);
                                        //sectopns[i] = sectopns[i].Replace(sProviderUserID, "");
                                        //sProviderUserID = sProviderUserID.Replace("<AddendumProviderID>", "").Replace("</AddendumProviderID>", "");
                                        //ilstUser = usermngr.getUserByPHYID(Convert.ToUInt64(sProviderUserID));
                                        //ilstUser = getUserByPHYID(sProviderUserID);
                                    }
                                    sProviderEmailID = ilstUser[0].EMail_Address;

                                    string[] sTempNotesName = sectopns[i].Split(new string[] { " AM: ", " PM: " }, System.StringSplitOptions.RemoveEmptyEntries);
                                    string sNotesName = sTempNotesName.Length > 1 ? sTempNotesName[1] : "";
                                    string[] sTempCreatedAt = sectopns[i].Split(new string[] { " on " }, System.StringSplitOptions.RemoveEmptyEntries);
                                    string sCreatedAt = sTempCreatedAt.Length > 1 ? sTempCreatedAt[1].Replace(":" + sNotesName, "").Replace(": " + sNotesName, "") : "";
                                    sCreatedAt = (sCreatedAt.IndexOf(" PM: ") > -1)? sCreatedAt.Replace(sCreatedAt.Substring(sCreatedAt.IndexOf(" PM: ")), ""): sCreatedAt;
                                    sCreatedAt = (sCreatedAt.IndexOf(" AM: ") > -1) ? sCreatedAt.Replace(sCreatedAt.Substring(sCreatedAt.IndexOf(" AM: ")), "") : sCreatedAt;
                                    //Jira CAP-3617
                                    sCreatedAt = (sCreatedAt.IndexOf("PM:") > -1) ? sCreatedAt.Replace(sCreatedAt.Substring(sCreatedAt.IndexOf("PM:")), "PM") : sCreatedAt;
                                    sCreatedAt = (sCreatedAt.IndexOf("AM:") > -1) ? sCreatedAt.Replace(sCreatedAt.Substring(sCreatedAt.IndexOf("AM:")), "AM") : sCreatedAt;
                                    //Jira CAP-3617 - End
                                    string[] tempCreatedByFinal = sectopns[i].Split(new string[] { " Signed by " }, System.StringSplitOptions.RemoveEmptyEntries);

                                    string tempCreatedBy = tempCreatedByFinal.Length > 1 ? tempCreatedByFinal[1].Split(new string[] { " on " }, System.StringSplitOptions.RemoveEmptyEntries)[0] : "";


                                    string createdBy = tempCreatedBy ?? string.Empty;
                                    if (sCreatedAt != "")
                                    {
                                        sCreatedAt = Convert.ToDateTime(sCreatedAt).ToString("o");
                                    }

                                    sNotesName = sNotesName.Replace("<br />", @"\n").Replace("<br/>", @"\n").Replace("\t", "").Replace('"', '\"');
                                    sNotesName = sNotesName.Substring(0, sNotesName.Length - @"\n".Length);
                                    sFormationJson = sFormationJson + ((sFormationJson[sFormationJson.Length - 1] == '}') ? "," : "")
                                        //Jira CAP-2608
                                        //+ "{\"" + "text" + "\":\"" + sNotesName + "\"," +
                                        + "{\"" + "text" + "\":\"" + sNotesName.Replace("\r\n", @"\n").Replace("\n", @"\n") + "\"," +
                                        "\"" + "createdBy" + "\":\"" + createdBy.TrimEnd() + "\"," +
                                        "\"" + "UserID" + "\":\"" + sProviderEmailID + "\"," +
                                        "\"" + "ProviderID" + "\":\"" + sProviderUserID + "\"," +
                                        "\"" + "ReviewedBy" + "\":\"" + string.Empty + "\"," +
                                        "\"" + "ReviewedUserID" + "\":\"" + string.Empty + "\"," +
                                        "\"" + "ReviewedProviderID" + "\":\"" + string.Empty + "\"," +
                                        "\"" + "createdAt" + "\":\"" + sCreatedAt + "\"}";
                                }
                            }
                            else
                            {
                                //value
                                sectopns[i] = sectopns[i].Replace("\"", "'").Replace("</b>", "").TrimStart().TrimEnd().Replace("<plan />", "").Replace("</plan>", "").Replace("<br />", @"\n").Replace("<br/>", @"\n").Replace("\r\n", @"\n").Replace("\n", @"\n").Replace("\t", "").Replace('"', '\"');
                                if (sectopns[i] != string.Empty && sectopns[i] != "")
                                {
                                    sFormationJson = sFormationJson + "\"" + sectopns[i] + "\"" + ((sectopns.Length - 1 == i) ? string.Empty : ",");
                                }
                            }
                        }
                        sFormationJson = sFormationJson + "]";
                        return sFormationJson;
                        break;
                    }
                case "2":
                    {
                        //Screening and pre screening
                        string sXmlSection = sSection;
                        string sScreenName = string.Empty;
                        int count = 1;
                        sXmlSection = "<?xml version=\"1.0\" encoding=\"utf-8\"?> <content>" + sXmlSection + "</content>";
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(sXmlSection);

                        if (doc.SelectSingleNode("content/b")?.InnerText != null)
                        {
                            sScreenName = doc.SelectSingleNode("content/b")?.InnerText.Replace(":", "");
                            sSection = sSection.Replace(doc.SelectSingleNode("content/b").OuterXml + "\"<br />", "").Replace(doc.SelectSingleNode("content/b").OuterXml, "").Replace(doc.SelectSingleNode("content/b").OuterXml + "\"<br/>", "");
                        }

                        string[] sContent = sSection.Split(subtab, System.StringSplitOptions.RemoveEmptyEntries);
                        if (sContent.Length > 0)
                        {
                            count = sContent.Length;
                        }
                        string sSectioncontent = string.Empty;
                        string sSubtabcontent = string.Empty;
                        for (int iSubtab = 0; iSubtab < count; iSubtab++)
                        {
                            string sSubtabName = string.Empty;
                            if (sContent[iSubtab] != "<br />" && sContent[iSubtab] != "<br /><b>" && sContent[iSubtab] != "<b>")
                            {
                                if (sContent[iSubtab].Contains("</subtab>"))
                                {
                                    sSubtabName = sContent[iSubtab].Substring(0, sContent[iSubtab].IndexOf("</subtab>")).Replace(":", "");
                                    sContent[iSubtab] = sContent[iSubtab].Replace(sSubtabName + ":</subtab>", "");
                                }
                                string[] sSectioniteams = sContent[iSubtab].Split(section, System.StringSplitOptions.RemoveEmptyEntries);
                                sSectioncontent = string.Empty;
                                for (int iSectionCount = 0; iSectionCount < sSectioniteams.Length; iSectionCount++)
                                {
                                    if (sSectioniteams[iSectionCount] != "<br />" && sSectioniteams[iSectionCount] != "<br /><b>" && sSectioniteams[iSectionCount] != "<b>")
                                    {
                                        string[] iSectionValuesplit = sSectioniteams[iSectionCount].Split(Case2heading, System.StringSplitOptions.RemoveEmptyEntries);
                                        sSectioncontent = sSectioncontent + ((sSectioncontent != string.Empty) ? "," : "");
                                        for (int iSectionValueCount = 0; iSectionValueCount < iSectionValuesplit.Length; iSectionValueCount++)
                                        {
                                            if (iSectionValuesplit[iSectionValueCount] != "<br />" && iSectionValuesplit[iSectionValueCount] != "<br /><b>" && iSectionValuesplit[iSectionValueCount] != "<b>")
                                            {
                                                if (iSectionValuesplit[iSectionValueCount].Contains("</i>"))
                                                {
                                                    sSectioncontent = sSectioncontent + ((sSectioncontent != string.Empty && sSectioncontent.Substring(sSectioncontent.LastIndexOf("],")) != "],") ? "]," : "");
                                                    sSectioncontent = sSectioncontent + "\"" + iSectionValuesplit[iSectionValueCount].Replace("\"", "'").Replace("</i>", "").Replace("</b>", "").Replace(":", "").Replace('"', '\"') + "\"" + ":["; ;
                                                }
                                                else
                                                {
                                                    //Jira CAP-3421
                                                    if (iSectionValuesplit[iSectionValueCount].Contains("<table"))
                                                    {
                                                        iSectionValuesplit[iSectionValueCount] = iSectionValuesplit[iSectionValueCount].Replace("\"", "'").Replace("<b>", "").Replace("</b>", "").TrimStart().TrimEnd().Replace("<br />", "").Replace("<br/>", "").Replace("\r\n", "").Replace("\n", "").Replace("\t", "").Replace('"', '\"');
                                                        XmlDocument xmlDocumentForTable = new XmlDocument();
                                                        xmlDocumentForTable.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?> <content>" + iSectionValuesplit[iSectionValueCount] + "</content>");
                                                        string sRowContent = string.Empty;
                                                        string sInnerContent = string.Empty;
                                                        string sTableContent = string.Empty;

                                                        for (int iCountX = 1; iCountX < xmlDocumentForTable.SelectSingleNode("content/table").ChildNodes.Count; iCountX++)
                                                        {
                                                            //Row
                                                            int iTagCount = xmlDocumentForTable.SelectSingleNode("content/table").ChildNodes[iCountX].ChildNodes.Count;
                                                            sRowContent = string.Empty;
                                                            sInnerContent = string.Empty;
                                                            for (int iCountY = 1; iCountY < iTagCount - 2; iCountY++)
                                                            {
                                                                sInnerContent = sInnerContent + xmlDocumentForTable.SelectSingleNode("content/table").ChildNodes[iCountX].ChildNodes[iCountY].InnerText.TrimStart().TrimEnd() + " ";
                                                            }
                                                            if ((xmlDocumentForTable.SelectSingleNode("content/table").ChildNodes.Count - 1) == iCountX)
                                                            {
                                                                sRowContent = "{\"Total Score\":\"" + xmlDocumentForTable.SelectSingleNode("content/table").ChildNodes[iCountX].ChildNodes[iTagCount - 1].InnerText + "\"}";
                                                            }
                                                            else
                                                            {
                                                                sRowContent = "{\"MinLabel\":\"" + xmlDocumentForTable.SelectSingleNode("content/table").ChildNodes[iCountX].ChildNodes[0].InnerText +
                                                                "\",\"Range\":\"" + sInnerContent.TrimEnd() +
                                                                "\",\"MaxLabel\":\"" + xmlDocumentForTable.SelectSingleNode("content/table").ChildNodes[iCountX].ChildNodes[iTagCount - 2].InnerText +
                                                                "\",\"Score\":\"" + xmlDocumentForTable.SelectSingleNode("content/table").ChildNodes[iCountX].ChildNodes[iTagCount - 1].InnerText + "\"}";
                                                            }
                                                            sTableContent = sTableContent + ((sTableContent != string.Empty) ? "," + sRowContent : sRowContent);
                                                        }
                                                        sSectioncontent = sSectioncontent + ((sSectioncontent != string.Empty && sSectioncontent.Substring(sSectioncontent.LastIndexOf(":[")) == ":[") ? "" : ",") + sTableContent;
                                                    }
                                                    else
                                                    {
                                                        //Jira CAP-2608
                                                        //iSectionValuesplit[iSectionValueCount] = iSectionValuesplit[iSectionValueCount].Replace("\"", "'").Replace("</b>", "").TrimStart().TrimEnd().Replace("<br />", "").Replace("<br/>", "");
                                                        iSectionValuesplit[iSectionValueCount] = iSectionValuesplit[iSectionValueCount].Replace("\"", "'").Replace("</b>", "").TrimStart().TrimEnd().Replace("<br />", @"\n").Replace("<br/>", @"\n").Replace("\r\n", @"\n").Replace("\n", @"\n").Replace("\t", "").Replace('"', '\"');
                                                        sSectioncontent = sSectioncontent + ((sSectioncontent != string.Empty && sSectioncontent.Substring(sSectioncontent.LastIndexOf(":[")) == ":[") ? "" : ",") + "\"" + iSectionValuesplit[iSectionValueCount] + "\"";
                                                    }
                                                }

                                            }
                                            if (iSectionValueCount == iSectionValuesplit.Length - 1)
                                            {
                                                sSectioncontent = sSectioncontent + ((sSectioncontent != string.Empty) ? "]" : "");
                                            }
                                        }




                                    }


                                }

                            }
                            if (sSubtabName != string.Empty)
                            {
                                string subtabtemp = string.Empty;
                                subtabtemp = "\"" + sSubtabName + "\":{" + sSectioncontent + "}";
                                sSubtabcontent = sSubtabcontent + ((sSubtabcontent != string.Empty) ? "," : "") + subtabtemp;
                            }
                        }

                        if (sSubtabcontent != string.Empty)
                        {
                            sScreenName = "\"" + sScreenName + "\":{" + sSubtabcontent + "}";
                        }
                        else
                        {
                            sScreenName = "\"" + sScreenName + "\":{" + sSectioncontent + "}";
                        }

                        return sScreenName;
                        break;
                    }
                case "3":
                    {
                        //Individualized Care Plan
                        sSection = "<?xml version=\"1.0\" encoding=\"utf-8\"?> <content>" + sSection + "</content>";
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(sSection);
                        int iChildTagCount = 0;
                        string sHeaderName = string.Empty;
                        if (doc.SelectSingleNode("content/font[1]/b")?.InnerText != null)
                        {
                            sHeaderName = doc.SelectSingleNode("content/font[1]/b").InnerText.Replace(":", "");
                        }

                        if (doc.SelectSingleNode("content/font[2]")?.ChildNodes != null)
                        {
                            iChildTagCount = (int)doc.SelectSingleNode("content/font[2]")?.ChildNodes.Count;
                        }
                        string sBody = string.Empty;
                        for (int iCount = 0; iCount < iChildTagCount; iCount++)
                        {

                            if (doc.SelectSingleNode("content/font[2]")?.ChildNodes[iCount].Name == "b")
                            {
                                sBody = sBody + ((sBody != string.Empty) ? "]," : "");
                                string header = doc.SelectSingleNode("content/font[2]")?.ChildNodes[iCount].InnerText.Replace(":", "");
                                sBody = sBody + "\"" + header + "\"" + ":[";
                            }
                            else if (doc.SelectSingleNode("content/font[2]")?.ChildNodes[iCount].Name == "ul")
                            {
                                //Jira CAP-3108
                                //string value = doc.SelectSingleNode("content/font[2]")?.ChildNodes[iCount].InnerText.Replace("\"", "'").Replace('"', '\"');
                                string value = doc.SelectSingleNode("content/font[2]")?.ChildNodes[iCount].InnerText.Replace("\"", "'").Replace('"', '\"').Replace("<br />", @"\n").Replace("<br/>", @"\n").Replace("\r\n", @"\n").Replace("\n", @"\n").Replace("\t", "");
                                sBody = sBody + ((sBody.Substring(sBody.LastIndexOf(":[")) == ":[") ? "" : ",") + "\"" + value + "\"";
                            }

                            if (iCount == iChildTagCount - 1)
                            {
                                sBody = sBody + ((sBody != string.Empty) ? "]" : "");
                            }

                        }
                        sHeaderName = "\"" + sHeaderName + "\":{" + sBody + "}";
                        return sHeaderName;

                        break;
                    }
                case "4":
                    {
                        //table formate Pages
                        sSection = "<?xml version=\"1.0\" encoding=\"utf-8\"?> <content>" + sSection + "</content>";
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(sSection);
                        string sTableRow = string.Empty;
                        int iSubtabCount = 1;
                        bool isSubtabOccure = false;
                        string sScubtabHeader = string.Empty;
                        string sSubtabContent = string.Empty;
                        string sScreenName = string.Empty;
                        if (doc.SelectNodes("content/subtab")?.Count != null && doc.SelectNodes("content/subtab").Count > 0) // if this screen have subtab this codition is applayed
                        {
                            iSubtabCount = doc.SelectNodes("content/subtab").Count;
                            isSubtabOccure = true;
                        }
                        for (int iCountSubtab = 1; iCountSubtab <= iSubtabCount; iCountSubtab++)
                        {
                            sSubtabContent = string.Empty;
                            sTableRow = string.Empty;
                            if (isSubtabOccure)
                            {
                                sSubtabContent = doc.SelectSingleNode("content/subtab[" + iCountSubtab + "]").InnerText.Replace(":", "").Replace("\"", "'").Replace('"', '\"');
                            }
                            else
                            {
                                sSubtabContent = doc.SelectSingleNode("content/b").InnerText.Replace(":", "");
                            }
                            for (int iCountRow = 1; iCountRow <= doc.SelectNodes("content/table[" + iCountSubtab + "]/tr").Count; iCountRow++)
                            {
                                string sTablecolumn = string.Empty;
                                int rowcount = doc.SelectNodes("content/table[" + iCountSubtab + "]/tr[" + iCountRow + "]/td").Count;
                                for (int iCounttd = 1; iCounttd <= rowcount; iCounttd++)
                                {
                                    //Jira CAP-3108
                                    //sTablecolumn = sTablecolumn + ((sTablecolumn != string.Empty) ? "," : "") + "\"" + doc.SelectSingleNode("content/table[" + iCountSubtab + "]/thead/tr/td[" + iCounttd + "]").InnerText.Replace("\"", "'").Replace('"', '\"') + "\"" + ":" + "\"" + doc.SelectSingleNode("content/table[" + iCountSubtab + "]/tr[" + iCountRow + "]/td[" + iCounttd + "]").InnerText.Replace("\"", "'").Replace('"', '\"') + "\"";
                                    sTablecolumn = sTablecolumn + ((sTablecolumn != string.Empty) ? "," : "") + "\"" + doc.SelectSingleNode("content/table[" + iCountSubtab + "]/thead/tr/td[" + iCounttd + "]").InnerText.Replace("\"", "'").Replace('"', '\"') + "\"" + ":" + "\"" + doc.SelectSingleNode("content/table[" + iCountSubtab + "]/tr[" + iCountRow + "]/td[" + iCounttd + "]").InnerText.Replace("\"", "'").Replace('"', '\"').Replace("<br />", @"\n").Replace("<br/>", @"\n").Replace("\r\n", @"\n").Replace("\n", @"\n").Replace("\t", "") + "\"";
                                }

                                sTableRow = sTableRow + ((sTableRow != string.Empty) ? "," : "") + "{" + sTablecolumn + "}";
                            }
                            sSubtabContent = "\"" + sSubtabContent + "\":[" + sTableRow + "]";
                            sScubtabHeader = sScubtabHeader + ((sScubtabHeader != string.Empty) ? "," : "") + sSubtabContent;
                        }
                        if (isSubtabOccure)
                        {
                            sScreenName = "\"" + doc.SelectSingleNode("content/b").InnerText.Replace(":","") + "\":";
                            sScreenName = sScreenName + "{" + sScubtabHeader + "}";
                            return sScreenName;
                        }


                        return sSubtabContent;
                        break;
                    }
                case "5":
                    {
                        //Patient Details - header
                        sSection = "<?xml version=\"1.0\" encoding=\"utf-8\"?> <content>" + sSection + "</content>";
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(sSection);
                        int iRowLength = doc.SelectNodes("content/table/thead/tr").Count;
                        string value = string.Empty;
                        string sPatientDetails = string.Empty;
                        for (int iCount = 1; iCount <= iRowLength; iCount++)
                        {
                            if (doc.SelectNodes("content/table/thead/tr[" + iCount + "]/td")?.Count != null)
                            {
                                int iColumnLength = doc.SelectNodes("content/table/thead/tr[" + iCount + "]/td").Count;
                                for (int iCounttd = 1; iCounttd <= iColumnLength; iCounttd++)
                                {
                                    value = string.Empty;
                                    if (doc.SelectSingleNode("content/table/thead/tr[" + iCount + "]/td[" + iCounttd + "]")?.InnerText != null)
                                    {
                                        value = doc.SelectSingleNode("content/table/thead/tr[" + iCount + "]/td[" + iCounttd + "]").InnerText.Replace("#$%", "");
                                        if (!value.EndsWith(": "))
                                        {
                                            //Jira CAP-2608
                                            //value = value.Replace(": ", ":");
                                            value = value.Replace(": ", ":").Replace("\r\n", @"\n").Replace("\n", @"\n").Replace("\t", "").Replace('"', '\"'); 
                                        }
                                    }
                                    if (value != "")
                                    {
                                        //Jira CAP-2638
                                        //sPatientDetails = sPatientDetails + ((sPatientDetails != string.Empty) ? "," : "") + "\"" + value.Substring(0, value.IndexOf(":")) + "\":\"" + value.Substring(((value.IndexOf(":") != value.Length - 1) ? value.IndexOf(":") + 1 : 0)) + "\"";
                                        sPatientDetails = sPatientDetails + ((sPatientDetails != string.Empty) ? "," : "") + "\"" + value.Substring(0, value.IndexOf(":")) + "\":\"" + value.Substring(((value.IndexOf(":") != value.Length - 1) ? value.IndexOf(":") + 1 : value.Length)).TrimEnd() + "\"";
                                    }
                                }
                            }
                        }
                        sPatientDetails = "\"" + "Patient Details" + "\":{" + sPatientDetails + "}";
                        return sPatientDetails;
                    }
            }
            return string.Empty;
        }

        private void GenerateCDCEntity(string sHumanID, string sEntityID, string sEntityName, string transactionBy, DateTime transactionDateTime)
        {
            IList<CDCEventTracker> ilstCDCEvent = new List<CDCEventTracker>();
            CDCEventTrackerManager CDCEventMngr = new CDCEventTrackerManager();
            CDCEventTracker objCDCEventInitiated = new CDCEventTracker();

            string sCDCEntityQry = "SELECT CDC_Entity_Tracker_ID as Id, Entity_ID, Human_ID, Entity_Name, Status, Error_Description, Created_By, Created_Date_and_Time, Modified_By, Modified_Date_and_Time, Version FROM cdc_entity_tracker WHERE Human_ID = {0} and Entity_ID= {1} and Entity_Name= '{2}';";
            DataSet CDCEventResult = DBConnector.ReadData(string.Format(sCDCEntityQry, sHumanID, sEntityID, sEntityName));
            ilstCDCEvent = DBConnector.DataTableToList<CDCEventTracker>(CDCEventResult.Tables[0]) ?? new List<CDCEventTracker>();
            bool isModified = false;
            try
            {
                //Initiate save call
                if (ilstCDCEvent.Count == 0)
                {
                    ilstCDCEvent.Add(objCDCEventInitiated);
                    ilstCDCEvent[0].Id = 0;
                    ilstCDCEvent[0].Created_By = transactionBy;
                    ilstCDCEvent[0].Created_Date_And_Time = transactionDateTime;
                    isModified = false;
                }
                else
                {
                    ilstCDCEvent[0].Modified_By = transactionBy;
                    ilstCDCEvent[0].Modified_By = transactionBy;
                    ilstCDCEvent[0].Modified_Date_And_Time = transactionDateTime;
                    isModified = true;
                }

                ilstCDCEvent[0].Human_ID = Convert.ToUInt64(sHumanID);
                ilstCDCEvent[0].Entity_ID = Convert.ToUInt64(sEntityID);
                ilstCDCEvent[0].Entity_Name = sEntityName;
                ilstCDCEvent[0].Status = "Completed";
                ilstCDCEvent[0].Error_Description = string.Empty;

                CDCEventMngr.SaveCDCEventTrackerWithTransaction(ilstCDCEvent, string.Empty);
            }
            catch (Exception eex)
            {
                try
                {
                    ilstCDCEvent[0].Human_ID = Convert.ToUInt64(sHumanID);
                    ilstCDCEvent[0].Entity_ID = Convert.ToUInt64(sEntityID);
                    ilstCDCEvent[0].Entity_Name = sEntityName;
                    ilstCDCEvent[0].Status = "Error";
                    ilstCDCEvent[0].Error_Description = "Message : " + eex?.Message + "Stack Trace : " + eex?.StackTrace;
                    if (isModified)
                    {
                        ilstCDCEvent[0].Modified_By = "";
                        ilstCDCEvent[0].Modified_Date_And_Time = DateTime.UtcNow;
                    }
                    else
                    {
                        ilstCDCEvent[0].Created_By = "";
                        ilstCDCEvent[0].Created_Date_And_Time = DateTime.UtcNow;
                    }
                    CDCEventMngr.SaveCDCEventTrackerWithTransaction(ilstCDCEvent, string.Empty);
                    throw new Exception("Error : " + eex?.Message);
                }
                catch { throw new Exception("Error : " + eex?.Message); }
            }
        }

        private string ConvertToLocal(string review_Signed_Date)
        {
            //Jira CAP-2711
            review_Signed_Date = Convert.ToDateTime(review_Signed_Date).ToString("yyyy-MM-dd HH:mm");
            string convertToLocalQry = "SELECT convert_tz('{0}','Gmt','Us/Pacific') AS PSTTime;";
            DataSet convertToLocalResult = DBConnector.ReadData(string.Format(convertToLocalQry, review_Signed_Date));
            if (convertToLocalResult != null && convertToLocalResult.Tables != null && convertToLocalResult.Tables.Count > 0)
            {
                return Convert.ToDateTime(convertToLocalResult.Tables[0].Rows[0]["PSTTime"]).ToString("dd-MMM-yyyy hh:mm tt");
            }
            return DateTime.MinValue.ToString("dd-MMM-yyyy hh:mm tt");
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

        private IList<User> getUserByPHYID(string sProviderUserID)
        {
            string qryUserByPHYID = "SELECT EMail_Address FROM User WHERE Physician_Library_ID = {0};";
            DataSet UserByPHYIDResult = DBConnector.ReadData(string.Format(qryUserByPHYID, sProviderUserID));
            return DBConnector.DataTableToList<User>(UserByPHYIDResult.Tables[0]) ?? new List<User>();
        }
        ////Jira CAP-3393
        //public string ReplaceHexadecimal(string sInputString)
        //{
        //    string sHtmlEntitiesRegx = @"&#x?[0-9a-fA-F]+;?";
        //    sInputString = Regex.Replace(sInputString, sHtmlEntitiesRegx, " ");

        //    string sHexadecimalRegx = @"^0x[0-9a-fA-F]+$|^[0-9a-fA-F]{2,}$";
        //    sInputString = Regex.Replace(sInputString, sHexadecimalRegx, " ");
        //    return sInputString;
        //}
    }

    public static class DBConnector
    {
        static MySqlDataAdapter MyDataAdap = null;

        private static string ReadConnection()
        {
            string ConnectionData;
            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            return ConnectionData;
        }

        public static DataSet ReadData(string Query)
        {
            DataSet dsReturn = new DataSet();
            MyDataAdap = new MySqlDataAdapter(Query, ReadConnection());
            MyDataAdap.Fill(dsReturn);
            return dsReturn;
        }

        public static int WriteData(string Query)
        {
            int iReturn = 0;
            using (MySqlConnection con = new MySqlConnection(ReadConnection()))
            {
                using (MySqlCommand cmd = new MySqlCommand(Query))
                {
                    cmd.Connection = con;
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        iReturn = 1;
                    }
                    catch
                    {
                        iReturn = 2;
                    }
                }
            }
            return iReturn;
        }

        public static List<T> DataTableToList<T>(this DataTable table) where T : new()
        {
            List<T> result = new List<T>();
            try
            {
                if (table != null
                    && table.Rows.Count > 0)
                {
                    IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
                    foreach (DataRow row in table.Rows)
                    {
                        T item = new T();
                        if (row != null && properties != null && properties.Count > 0)
                        {
                            foreach (var property in properties)
                            {
                                if (table.Columns.Contains(property.Name))
                                {
                                    if (row[property.Name] == DBNull.Value)
                                        property.SetValue(item, null, null);
                                    else
                                    {
                                        var value = row[property.Name];
                                        if (value is MySql.Data.Types.MySqlDateTime mysqlDateTime)
                                        {
                                            if (!mysqlDateTime.IsValidDateTime)
                                                property.SetValue(item, null, null);
                                            else
                                                property.SetValue(item, mysqlDateTime.GetDateTime(), null);
                                        }
                                        else
                                        {
                                            property.SetValue(item, value, null);
                                        }
                                    }
                                }
                            }
                        }
                        result.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
    //CAP-3112
    //public static class CapellaTaskManager
    //{
    //    private static readonly ConcurrentDictionary<string, object> _runningTasks = new ConcurrentDictionary<string, object>();
    //    private static readonly ConcurrentDictionary<string, object> _runningTasksForEncounter = new ConcurrentDictionary<string, object>();

    //    public static bool TryStartTask(string humanId, string encounterId)
    //    {
    //        if (!string.IsNullOrEmpty(humanId))
    //        {
    //            return _runningTasks.TryAdd(humanId, null);
    //        }
    //        else if (!string.IsNullOrEmpty(encounterId))
    //        {
    //            return _runningTasksForEncounter.TryAdd(encounterId, null);
    //        }
    //        return true;
    //    }

    //    public static void EndTask(string humanId, string encounterId)
    //    {
    //        if (!string.IsNullOrEmpty(humanId))
    //        {
    //            _runningTasks.TryRemove(humanId, out _);
    //        }
    //        else if (!string.IsNullOrEmpty(encounterId))
    //        {
    //            _runningTasksForEncounter.TryRemove(encounterId, out _);
    //        }
    //    }
    //}
}
