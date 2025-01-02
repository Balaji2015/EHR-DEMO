using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Acurus.Capella.Core;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess;
using Acurus.Capella.DataAccess.ManagerObjects;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Web.Script.Serialization;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI.WebServices
{
    /// <summary>
    /// Summary description for ReportService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ReportService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string LoadReport()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            FacilityManager FacilityMngr = new FacilityManager();
            PhysicianManager PhysicianMngr = new PhysicianManager();
            EncounterManager EncounterMngr = new EncounterManager();
            StaticLookupManager StaticLookupMngr = new StaticLookupManager();

            IList<StaticLookup> ReportNameList = new List<StaticLookup>();
            IList<StaticLookup> ApptCurrentProcessList = new List<StaticLookup>();
            IList<FacilityLibrary> FacilityList = new List<FacilityLibrary>();
            IList<WorkFlow> CurrentProcessList = new List<WorkFlow>();
            IList<string> CurrentOwnerList = new List<string>();

            ReportNameList = StaticLookupMngr.getStaticLookupByFieldName("EHR REPORTS", "Sort_Order");
            ApptCurrentProcessList = StaticLookupMngr.getStaticLookupByFieldName("APPOINTMENTSREPORT");
            string sApptCPName = string.Empty;
            for (int iCurr = 0; iCurr < ApptCurrentProcessList.Count(); iCurr++)
            {
                sApptCPName += "'" + ApptCurrentProcessList[iCurr].Value + "',";
            }
            string sFacilityName = ClientSession.FacilityName;
            FacilityList = FacilityMngr.GetFacilityList();
            FillPhysicianUser PhyUserList = PhysicianMngr.GetPhysicianandUser(true, sFacilityName,ClientSession.LegalOrg);
            FillPhysicianUser AllPhyUserList = PhysicianMngr.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);
            //CAP-2788
            UserList ilstUserList = ConfigureBase<UserList>.ReadJson("User.json");
            if (ilstUserList?.User != null)
            {
                CurrentOwnerList = ilstUserList?.User.Select(a => a.User_Name).ToList();
            }
            CurrentProcessList = EncounterMngr.GetWorkFlowMapListForReports();

            IList<FacilityLibrary> PatientSeenFacilityName = FacilityMngr.GetFacilityNameByFacilityType();

            string sPSFacilityList = string.Empty;

            for (int iFac = 0; iFac < PatientSeenFacilityName.Count(); iFac++)
            {
                sPSFacilityList += "'" + PatientSeenFacilityName[iFac].Fac_Name + "',";
            }

            string sProjectType = System.Configuration.ConfigurationManager.AppSettings["ProjectType"].ToString();
            string sBIRTReportUrl = System.Configuration.ConfigurationManager.AppSettings["BIRTReportUrl"].ToString() + "CAPELLA_" + sProjectType;


            NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration().Configure();
            string[] conString = cfg.GetProperty(NHibernate.Cfg.Environment.ConnectionString).ToString().Split(';');

            string sDataBase = string.Empty;
            string sDataSource = string.Empty;
            string sUserId = string.Empty;
            string sPassword = string.Empty;
            string sPort = "3306";
            for (int i = 0; i < conString.Length; i++)
            {
                if (conString[i].ToString().ToUpper().Contains("DATABASE") == true)
                {
                    sDataBase = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("DATA SOURCE") == true)
                {
                    sDataSource = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("USER ID") == true)
                {
                    sUserId = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("PASSWORD") == true)
                {
                    sPassword = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("PORT") == true)
                {
                    sPort = conString[i].ToString().Split('=')[1];
                }
            }
            string sodaURL = "jdbc:mysql://" + sDataSource + ":" + sPort + "/" + sDataBase;
            string sodaUser = sUserId;
            string sodaPassword = sPassword;
            string sDBConnection = "&odaURL=" + sodaURL + "&odaUser=" + sodaUser + "&odaPassword=" + sodaPassword;

            var result = new { ReportNameList = ReportNameList, FacilityList = FacilityList, PhysicianList = PhyUserList.PhyList, AllPhysicianList = AllPhyUserList.PhyList, FacilityName = sFacilityName, UserRole = ClientSession.UserRole, UserID = ClientSession.PhysicianId, UserName = ClientSession.UserName, CurrentOwnerList = CurrentOwnerList, CurrentProcessList = CurrentProcessList, BIRTUrl = sBIRTReportUrl, DBConnection = sDBConnection, sPSFacilityList = sPSFacilityList.TrimEnd(','), ApptCurrentProcessList = sApptCPName.TrimEnd(',') };
            return JsonConvert.SerializeObject(result);
        }

        [WebMethod(EnableSession = true)]
        public string GetPatientDetailsByTokens(string text_searched, string account_status, string patient_status, string human_type)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            double WS_Time = 0;
            double DB_Time = 0;
            string CurrentKeywordCriteria = text_searched + "~" + account_status + "~" + patient_status + "~" + human_type;
            try
            {
                if (HttpContext.Current.Session["PreviousPatientKeywordCriteria"] != null
                    && HttpContext.Current.Session["PreviousPatientList"] != null
                    && HttpContext.Current.Session["PreviousPatientKeywordCriteria"].ToString().Trim().ToLower() == CurrentKeywordCriteria.ToLower())
                {
                    var lstResult = HttpContext.Current.Session["PreviousPatientList"];
                    watch.Stop();
                    WS_Time = watch.Elapsed.TotalSeconds;
                    string time_taken = "WS_Time : " + (WS_Time - DB_Time).ToString() + "s; DB_Time : " + (DB_Time).ToString() + "s;";
                    var lstFinalResult = new
                    {
                        Matching_Result = lstResult,
                        Time_Taken = time_taken
                    };
                    return JsonConvert.SerializeObject(lstFinalResult);
                }
                else
                {
                    IList<Human_Token> lstHumans = new List<Human_Token>();
                    HumanManager objHumanManager = new HumanManager();
                    lstHumans = objHumanManager.GetHumanFromTokens(text_searched, account_status, patient_status, human_type, out DB_Time,ClientSession.LegalOrg,ClientSession.UserCarrier);

                    var lstResult = (from Hum in lstHumans
                                     select new
                                     {
                                         label = Hum.Result.ToUpper(),
                                         value = new
                                         {
                                             HumanId = Hum.Human_ID.ToString(),
                                             Status = Hum.Patient_Status,
                                             Account_Status = Hum.Account_Status
                                         }
                                     }
                                     );

                    watch.Stop();
                    WS_Time = watch.Elapsed.TotalSeconds;
                    string time_taken = "WS_Time : " + (WS_Time - DB_Time).ToString() + "s; DB_Time : " + (DB_Time).ToString() + "s;";
                    if (lstResult.Count() == 0)
                    {
                        var lstFinalResult = new
                        {
                            Result = "No matches found.",
                            Time_Taken = time_taken
                        };
                        return JsonConvert.SerializeObject(lstFinalResult);
                    }
                    else
                    {
                        HttpContext.Current.Session.Add("PreviousPatientKeywordCriteria", CurrentKeywordCriteria);
                        HttpContext.Current.Session.Add("PreviousPatientList", JsonConvert.SerializeObject(lstResult));
                        var lstFinalResult = new
                        {
                            Matching_Result = lstResult,
                            Time_Taken = time_taken
                        };
                        return JsonConvert.SerializeObject(lstFinalResult);
                    }
                }
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

        [WebMethod(EnableSession = true)]
        public void CreateAuditLogEntry(int HumanID, string startTime)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return;
            }
            IList<AuditLog> Savelist = new List<AuditLog>();
            AuditLog objauditlog = new AuditLog();

            objauditlog.Human_ID = HumanID;
            objauditlog.Entity_Name = "Acurus.Capella.Core.DomainObjects.Human";
            objauditlog.Transaction_Type = "SELECT";
            objauditlog.Transaction_By = ClientSession.UserName;

            DateTime utc = Convert.ToDateTime(startTime);
            objauditlog.Transaction_Date_And_Time = utc;

            objauditlog.Entity_Id = Convert.ToUInt64(HumanID);
            objauditlog.Attribute = "Patient";
            Savelist.Add(objauditlog);

            objauditlog = new AuditLog();

            objauditlog.Human_ID = HumanID;
            objauditlog.Entity_Name = "Acurus.Capella.Core.DomainObjects.Human_Token";
            objauditlog.Transaction_Type = "SELECT";
            objauditlog.Transaction_By = ClientSession.UserName;

            objauditlog.Transaction_Date_And_Time = utc;

            objauditlog.Entity_Id = Convert.ToUInt64(HumanID);
            objauditlog.Attribute = "Patient";
            Savelist.Add(objauditlog);

            AuditLogManager objAuditManager = new AuditLogManager();
            objAuditManager.AppendSelectEntryToAuditLog(Savelist, string.Empty);
            HttpContext.Current.Session.Remove("PreviousPatientList");
            HttpContext.Current.Session.Remove("PreviousPatientKeyword");
        }

        [WebMethod(EnableSession = true)]
        public string GetHumanDetails(ulong HumanID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            HumanManager humanMngr = new HumanManager();
            Human selectedPatient = humanMngr.GetHumanFromHumanID(HumanID);
            //PatientInsuredPlanManager InsPlanMngr = new PatientInsuredPlanManager();
            string policy_holder_id = "";//InsPlanMngr.GetPolicyHolderIdFromHumanId(HumanID);
            string return_value = JsonConvert.SerializeObject(new
            {
                HumanDetails = new
                {
                    HumanId = selectedPatient.Id.ToString(),
                    PatientName = selectedPatient.Last_Name + ", " + selectedPatient.First_Name + " " + selectedPatient.MI,
                    PatientDOB = Convert.ToDateTime(selectedPatient.Birth_Date).ToString("dd-MMM-yyyy"),
                    Status = selectedPatient.Patient_Status,
                    PCP = selectedPatient.PCP_Name,
                    HumanType = selectedPatient.Human_Type,
                    PatientGender = selectedPatient.Sex,
                    Aco_Eligible = selectedPatient.ACO_Is_Eligible_Patient,
                    SSN = selectedPatient.SSN,
                    Account_Status = selectedPatient.Account_Status,
                    Home_Phone = selectedPatient.Home_Phone_No,
                    Cell_Phone = selectedPatient.Cell_Phone_Number,
                    Encounter_Provider_ID = selectedPatient.Encounter_Provider_ID,
                    PolicyHolderID = policy_holder_id
                },
                DisplayString = (selectedPatient.Last_Name + "," + selectedPatient.First_Name + " " + selectedPatient.MI
                              + " | DOB: " + selectedPatient.Birth_Date.ToString("dd-MMM-yyyy")
                              + " | " + selectedPatient.Sex
                              + " | ACC#: " + selectedPatient.Id.ToString()
                              + (selectedPatient.Patient_Account_External != string.Empty && selectedPatient.Patient_Account_External != " " ? " | EX.ACC#: " + selectedPatient.Patient_Account_External : "")
                              + (selectedPatient.Medical_Record_Number != string.Empty && selectedPatient.Medical_Record_Number != " " ? " | MR#: " + selectedPatient.Medical_Record_Number : "")
                              + (selectedPatient.Street_Address1 != string.Empty && selectedPatient.City != string.Empty ? " | ADDR: " + selectedPatient.Street_Address1 + " , " + selectedPatient.City + " " + selectedPatient.ZipCode : " | ZipCode: " + selectedPatient.ZipCode)
                              + (selectedPatient.Home_Phone_No != string.Empty ? " | Ph: " + selectedPatient.Home_Phone_No : "")
                              + " | PATIENT TYPE: " + selectedPatient.Human_Type).ToUpper()
            });
            return return_value;
        }
        [WebMethod(EnableSession = true)]
        public string GetCurrentOwner(string CurrentProcess)
        {
            string jsons = "";
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sCurrentProcess = Convert.ToString(CurrentProcess);
            ProcUserManager Prmgr = new ProcUserManager();
            IList<ProcUser> lstProcess = new List<ProcUser>();
            lstProcess = Prmgr.GetProcUserList(CurrentProcess);
            var vusername = lstProcess.Select(a => a.User_Name).ToList();
            string json = new JavaScriptSerializer().Serialize(vusername);
            jsons = "{\"CurrentOwner\" :" + json + " }";
            return jsons;
        }
        [WebMethod(EnableSession = true)]
        public string PhysicaianNameList(string sFacilityName)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            PhysicianManager PhysicianMngr = new PhysicianManager();
            FillPhysicianUser PhyUserList = PhysicianMngr.GetPhysicianandUser(true, sFacilityName, ClientSession.LegalOrg);
            var result = new { PhysicianList = PhyUserList.PhyList };
            return JsonConvert.SerializeObject(result);
        }
    }
}
