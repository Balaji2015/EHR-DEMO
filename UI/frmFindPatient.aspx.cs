using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Acurus.Capella.Core.DomainObjects;
using System.Diagnostics;
using System.Net;
using System.IO;
using Acurus.Capella.Core.DTO;
using System.Collections;
using System.Runtime.Serialization;
using System.Data;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Text;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;


namespace Acurus.Capella.UI
{
    public partial class frmFindPatient : System.Web.UI.Page
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
           
            this.Page.Title = "Capella Find Patient";

            if (!IsPostBack)
            {
                if (Request["ScreenName"] != null)
                {
                    hdnFromScreen.Value = Request["ScreenName"].ToString();
                }
                else if (Session["FromScreen"] != null)
                {
                    hdnFromScreen.Value = Session["FromScreen"].ToString();
                }

                ClientSession.processCheck = false;
                SecurityServiceUtility objSecurityServiceUtility = new SecurityServiceUtility();
                objSecurityServiceUtility.ApplyUserPermissions(this);
                if (hdnFromScreen.Value != "" && hdnFromScreen.Value == "Indexing")
                {
                    btnAddpatient.Disabled = true;
                }
            }
        }

        #endregion

        FillQuickPatient objCheckOutLoad = null;
        HumanManager HumanMngr = new HumanManager();

        #region Webmethods
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetPatientDetailsByTokens(string text_searched, string account_status, string patient_status, string human_type)
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
                    var lstResult = JsonConvert.DeserializeObject(HttpContext.Current.Session["PreviousPatientList"].ToString());
                    //HttpContext.Current.Session["PreviousPatientList"];
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
                    text_searched=text_searched.Replace("'", "''");
                    lstHumans = objHumanManager.GetHumanFromTokens(text_searched, account_status, patient_status, human_type, out DB_Time,ClientSession.LegalOrg,ClientSession.UserCarrier);

                    var lstResult = (from Hum in lstHumans
                                     select new
                                     {
                                         label = Hum.Result.ToUpper(),
                                         value = new
                                         {
                                             HumanId = Hum.Human_ID.ToString(),
                                             Status = Hum.Patient_Status,
                                             Account_Status = Hum.Account_Status,
                                             sPhyFax = Hum.Result.ToUpper().Contains("FAX:") ? Hum.Result.Substring(Hum.Result.ToUpper().IndexOf("FAX") + 5, 14) : string.Empty
                                         }
                                     });

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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetPlanDetailsByTokens(string text_searched)
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
            string CurrentKeywordCriteria = text_searched;
            try
            {
                if (HttpContext.Current.Session["PreviousPlanKeywordCriteria"] != null
                    && HttpContext.Current.Session["PreviousPlanList"] != null
                    && HttpContext.Current.Session["PreviousPlanKeywordCriteria"].ToString().Trim().ToLower() == CurrentKeywordCriteria.ToLower())
                {
                    var lstResult = JsonConvert.DeserializeObject(HttpContext.Current.Session["PreviousPlanList"].ToString());
                    //HttpContext.Current.Session["PreviousPatientList"];
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
                    IList<InsurancePlan> lstHumans = new List<InsurancePlan>();
                    InsurancePlanManager objPlanManager = new InsurancePlanManager();
                    text_searched = text_searched.Replace("'", "''");
                    lstHumans = objPlanManager.GetPlanFromTokens(text_searched);

                    var lstResult = (from Hum in lstHumans
                                     select new
                                     {
                                         label = Hum.Ins_Plan_Name.ToUpper(),
                                         value = new
                                         {
                                             PlanId = Hum.Id.ToString() + "|" + Hum.Carrier_ID.ToString()
                                         }
                                     }) ;

                   
                    if (lstResult.Count() == 0)
                    {
                        string sPlanvalue = System.Configuration.ConfigurationManager.AppSettings["OtherPlanName"].ToString();
                        lstHumans = objPlanManager.GetPlanFromTokens(sPlanvalue);

                        var lstResultNoMatch = (from Hum in lstHumans
                                         select new
                                         {
                                             label = Hum.Ins_Plan_Name.ToUpper(),
                                             value = new
                                             {
                                                 PlanId = Hum.Id.ToString()
                                             }
                                         });


                        HttpContext.Current.Session.Add("PreviousPlanKeywordCriteria", CurrentKeywordCriteria);
                        HttpContext.Current.Session.Add("PreviousPlanList", JsonConvert.SerializeObject(lstResult));
                        var lstFinalResult = new
                        {
                            Matching_Result = lstResultNoMatch,

                        };
                        //var lstFinalResult = new
                        //{
                        //    Result = "No matches found.",

                        //};
                        return JsonConvert.SerializeObject(lstFinalResult);
                    }
                    else
                    {
                        HttpContext.Current.Session.Add("PreviousPlanKeywordCriteria", CurrentKeywordCriteria);
                        HttpContext.Current.Session.Add("PreviousPlanList", JsonConvert.SerializeObject(lstResult));
                        var lstFinalResult = new
                        {
                            Matching_Result = lstResult,
                            
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
        public static void CreateAuditLogEntry(int HumanID, string startTime)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return;
            }

            string Is_Audit_log = "N";

            if (System.Configuration.ConfigurationManager.AppSettings["Is_Audit_Log"] != null)
                Is_Audit_log = System.Configuration.ConfigurationManager.AppSettings["Is_Audit_Log"];

            if (Is_Audit_log == "Y")
            {
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

                string Entity_Name = "Human";
                string Transaction_Type = "QUERY";
                int Human_ID = 0;
                if (HumanID != 0)
                    Human_ID = Convert.ToInt32(HumanID);
                else
                    Human_ID = Convert.ToInt32(ClientSession.HumanId);
                AuditLogManager objAuditLogManager = new AuditLogManager();
                objAuditLogManager.InsertIntoAuditLog(Entity_Name, Transaction_Type, Human_ID, ClientSession.UserName);
            }
        }

        [WebMethod(EnableSession = true)]
        public static string GetHumanDetails(ulong HumanID, string FullDetails)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sPriCarrier = string.Empty;
            if (FullDetails != string.Empty )
            {
                if (FullDetails.Split('|')[4].ToString().Contains("PRI.CAR"))
                {
                    sPriCarrier = FullDetails.Split('|')[4].ToString().TrimEnd();
                }
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
                    PolicyHolderID = policy_holder_id,
                    Address = selectedPatient.Street_Address1 + " , " + selectedPatient.City + "," + selectedPatient.State,
                    ZipCode = selectedPatient.ZipCode,
                    EMail = selectedPatient.EMail,
                  PriCarrier=  sPriCarrier,
                   

                },
                DisplayString = (selectedPatient.Last_Name + "," + selectedPatient.First_Name + " " + selectedPatient.MI
                              + " | DOB: " + selectedPatient.Birth_Date.ToString("dd-MMM-yyyy")
                              + " | " + selectedPatient.Sex
                              + " | ACC#: " + selectedPatient.Id.ToString()
                              + (sPriCarrier != string.Empty ? " |" + sPriCarrier:"")
                              + (selectedPatient.Patient_Account_External != string.Empty && selectedPatient.Patient_Account_External != " " ? " | EX.ACC#: " + selectedPatient.Patient_Account_External : "")
                              + (selectedPatient.Medical_Record_Number != string.Empty && selectedPatient.Medical_Record_Number != " " ? " | MR#: " + selectedPatient.Medical_Record_Number : "")
                              + (selectedPatient.Street_Address1 != string.Empty && selectedPatient.City != string.Empty ? " | ADDR: " + selectedPatient.Street_Address1 + " , " + selectedPatient.City + " " + selectedPatient.ZipCode : " | ZipCode: " + selectedPatient.ZipCode)
                              + (selectedPatient.Home_Phone_No != string.Empty ? " | Ph: " + selectedPatient.Home_Phone_No : "")
                              + " | PATIENT TYPE: " + selectedPatient.Human_Type + " | EMail: " + selectedPatient.EMail).ToUpper()
            });
            return return_value;
        }

        [WebMethod(EnableSession = true)]
        public static void RemoveSessions()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return;
            }
            HttpContext.Current.Session.Remove("PreviousPatientList");
            HttpContext.Current.Session.Remove("PreviousPatientKeyword");
        }

        //BugID:49685
        [WebMethod(EnableSession = true)]
        public static void InsertIntoAuditLog(string Transaction_Type, string Entity_Name, string Human_ID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return;
            }
            string Is_Audit_log = "N";
              if (System.Configuration.ConfigurationManager.AppSettings["Is_Audit_Log"] != null)
                Is_Audit_log = System.Configuration.ConfigurationManager.AppSettings["Is_Audit_Log"];

              if (Is_Audit_log == "Y")
              {
                  if (ClientSession.UserName.EndsWith("_E"))
                      Transaction_Type = "EMERGENCY ACCESS";
                  int HumanID = 0;
                  if (Human_ID != string.Empty)
                      HumanID = Convert.ToInt32(Human_ID);
                  else
                      HumanID = Convert.ToInt32(ClientSession.HumanId);
                  AuditLogManager objAuditManager = new AuditLogManager();
                  objAuditManager.InsertIntoAuditLog(Entity_Name, Transaction_Type, HumanID, ClientSession.UserName);
              }
        }
        #endregion

        protected void btnPrintFaceSheet_ServerClick(object sender, EventArgs e)
        {
            SelectedItem.Value = string.Empty;

            Human humanLoadRecord = null;
         //   txtPatientSearch.attributes['data-human-id'].value 
            if (hdnHumanID.Value != string.Empty && hdnHumanID.Value != "0")
            {
                objCheckOutLoad = HumanMngr.LoadQuickPatient(0,Convert.ToUInt64(hdnHumanID.Value));
            }
            else
            {
                return;
            }
            if (objCheckOutLoad != null && objCheckOutLoad.HumanObj != null)
            {
                humanLoadRecord = objCheckOutLoad.HumanObj;
            }

            string path = Server.MapPath("Documents/" + Session.SessionID);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            PrintOrders printOrders = new PrintOrders();
            string FileLocation = printOrders.PrintFaceSheet(objCheckOutLoad, path);

            string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
            string[] FileName = FileLocation.Split(Split, StringSplitOptions.RemoveEmptyEntries);
            if (SelectedItem.Value == string.Empty)
            {
                SelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
            }
            else
            {
                SelectedItem.Value += "|" + FileName[0].ToString();
            }
            string sFaxSubject = string.Empty;
            if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y")
            {
                string sFaxFirstname = string.Empty;
                string sFaxLastName = string.Empty;

                IList<string> ilstHumanTag = new List<string>();
                ilstHumanTag.Add("HumanList");

                IList<object> ilstHumanBlobList = new List<object>();
                ilstHumanBlobList = UtilityManager.ReadBlob(Convert.ToUInt64(hdnHumanID.Value), ilstHumanTag);

                Human objFillHuman = new Human();

                if (ilstHumanBlobList != null && ilstHumanBlobList.Count > 0)
                {
                    if (ilstHumanBlobList[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobList[0]).Count; iCount++)
                        {
                            objFillHuman = ((Human)((IList<object>)ilstHumanBlobList[0])[iCount]);
                            sFaxFirstname = objFillHuman.First_Name;
                            sFaxLastName = objFillHuman.Last_Name;
                        }
                    }
                }

                //string human_id = "Human" + "_" + hdnHumanID.Value.ToString() + ".xml";
                //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);
                //if (File.Exists(strXmlHumanPath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                //    using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);

                //        XmlText.Close();
                //        if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                //        {
                //            if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes.Count > 0)
                //            {
                //                if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != null)
                //                    sFaxFirstname = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                //                if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != null)
                //                    sFaxLastName = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                //            }
                //        }
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}
                sFaxSubject = "Face_Sheet" + sFaxLastName + sFaxFirstname + "_" + DateTime.Now.ToString("dd-MMM-yyyy");
                sFaxSubject = sFaxSubject.Replace("'", "%27"); //For Bug id 72678

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Print FaceSheet", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PrintPDF('" + sFaxSubject + "');", true);

        }
    }
}
