using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Configuration;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Diagnostics;
using Acurus.Capella.DataAccess;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Security.RightsManagement;
using System.Net;
using Newtonsoft.Json.Linq;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI
{
    public partial class UtilityManager
    {
        public static ulong ulMyFindPatientID;
        public const ulong CASH_CARRIER_ID = 41;

        //BugID:53765
        //*Commented for UtilityManager CleanUp*
        /*public static ulong ulAppointmentID;
        public static ulong ulAppointmentProviderID;
        public static Hashtable userList = new Hashtable();
         * */

        //*Commented for UtilityManager CleanUp*
        /*public const string SECONDARY_INSURANCE = "SECONDARY_INSURANCE";
        public const string TERTIARY_INSURANCE = "TERTIARY_INSURANCE";
        public const string INSURANCE = "PRIMARY_INSURANCE";
        public const string BILLED_AMOUNT = "BILLED_AMOUNT";
        public const string BILL_TO = "BILL_TO";
        public const string MSGTYPE_CHARGEPOSTING = "CHARGE_POSTING_MESSAGE";
        public const string SPATTERN = "*.tif";
        public const string NEWPAYMENT = "NEWPAYMENT";
        public const string PATIENT_TYPE = "PATIENT";
        public const string TRANSFER = "TRANSFER";
        public const string COPAY = "COPAY";
        public const string COINSURANCE = "COINSURANCE";
        public const string DEDUCTIBLE = "DEDUCTIBLE";
        public const string PP_LINE_ITEM = "PP_LINE_ITEM";
        public const string REFUND = "REFUND";
        public const string OFFSET = "OFFSET";
        public const string OPENSTATUS = "OPEN";
        public const string WRITEOFF = "WRITEOFF";
        public const string UNAPPLIED = "UNAPPLIED";

        public const ulong CASH_INS_PLAN_ID = 68;
        public static bool bWorkset;
        public static bool bHold;
        public static string SCREEN_NAME;
        public const ulong SE_ERR_FNF = 2L;
        public static int iTotDamage = 0;
        public static DateTime APPOINMENT_DATE = DateTime.MinValue;

        //added by srividhya on 01-mar-2012
        public static bool bCancel = false;
        public static bool bCurrentScreen = false;
        public static decimal deExcCallQCAmt = 0;
        public static int iTotalLineItem = 0;
        public static DataSet ds = new DataSet();*/

        [DllImport("shell32.dll", EntryPoint = "ShellExecuteA")]
        public static extern ulong ShellExecute(int hWnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        public enum eTaskManager
        {
            eTaskManager_GeneralTask = 0,
            eTaskManager_PatientTask = 1
        }

        public static DateTime ConvertToUniversal()
        {

            DateTime inputDateTime = DateTime.UtcNow;

            //var request = ClientSession.UniversalTime.ToString();
            //string[] Hours = ClientSession.UniversalTime.ToString().Split('.');

            //DateTime dt = DateTime.MinValue;

            //double offset = (double.Parse(Hours[0]));
            //if (UIManager.bFollows_DST)
            //{
            //    DateTime DayLight_StartDateTime;
            //    DateTime DayLight_EndDateTime;
            //    int iDSTStartDay = UtilityManager.FindDate(inputDateTime.Year, 3, DayOfWeek.Sunday, 2);
            //    int iDSTEndDay = UtilityManager.FindDate(inputDateTime.Year, 11, DayOfWeek.Sunday, 1);
            //    DayLight_StartDateTime = new DateTime(inputDateTime.Year, 3, iDSTStartDay, 2, 0, 0);
            //    DayLight_EndDateTime = new DateTime(inputDateTime.Year, 11, iDSTEndDay, 2, 0, 0);
            //    if ((inputDateTime.Ticks > DayLight_StartDateTime.Ticks) && (inputDateTime.Ticks < DayLight_EndDateTime.Ticks))
            //    {
            //        if (offset < 0)
            //            offset += 1;
            //        else
            //            offset -= 1;
            //    }
            //}

            //if (inputDateTime.ToString() != "01-01-0001 12:00:00 AM")
            //{
            //    if (Hours.Length != 0 && Hours.Length == 2)
            //    {
            //        dt = inputDateTime.AddHours(-offset).AddMinutes(-double.Parse(Hours[1]));
            //    }
            //}
            //else
            //{
            //    dt = inputDateTime;
            //}

            return inputDateTime;

        }

        public static void inserttologgingtable(string EncounterID, string HumanID, string user_Name, string ProviderID, string Message_log, DateTime dtLogCurTime, string sGroupID, string sFormName)
        {
            string logflag = "N";
            if (ConfigurationSettings.AppSettings["Is_Logging"] != null)
                logflag = ConfigurationSettings.AppSettings["Is_Logging"].ToString().ToUpper();
            if (logflag == "Y" && HttpContext.Current.Session.SessionID != "")
            {
                string sModule = string.Empty;
                switch (sFormName)
                {
                    case "frmSummaryNew":
                        {
                            if (ConfigurationSettings.AppSettings["Is_Logging_frmSummaryNew"] != null && ConfigurationSettings.AppSettings["Is_Logging_frmSummaryNew"].ToString().ToUpper() == "Y")
                                sModule = "Yes";
                            break;
                        }
                    case "frmPatientChart":
                        {
                            if (ConfigurationSettings.AppSettings["Is_Logging_frmPatientChart"] != null && ConfigurationSettings.AppSettings["Is_Logging_frmPatientChart"].ToString().ToUpper() == "Y")
                                sModule = "Yes";
                            break;
                        }
                    case "frmRCopiaToolbar":
                        {
                            if (ConfigurationSettings.AppSettings["Is_Logging_frmRCopiaToolbar"] != null && ConfigurationSettings.AppSettings["Is_Logging_frmRCopiaToolbar"].ToString().ToUpper() == "Y")
                                sModule = "Yes";
                            break;
                        }
                    case "frmPatientCommunication":
                        {
                            if (ConfigurationSettings.AppSettings["Is_Logging_frmPatientCommunication"] != null && ConfigurationSettings.AppSettings["Is_Logging_frmPatientCommunication"].ToString().ToUpper() == "Y")
                                sModule = "Yes";
                            break;
                        }
                    case "ViewImg":
                        {

                            if (ConfigurationSettings.AppSettings["Is_Logging_ViewImg"] != null && ConfigurationSettings.AppSettings["Is_Logging_ViewImg"].ToString().ToUpper() == "Y")
                                sModule = "Yes";
                            break;
                        }
                    case "frmMyQueueNew":
                        {
                            if (ConfigurationSettings.AppSettings["Is_Logging_frmMyQueueNew"] != null && ConfigurationSettings.AppSettings["Is_Logging_frmMyQueueNew"].ToString().ToUpper() == "Y")
                                sModule = "Yes";
                            break;
                        }
                    case "frmLogin":
                        {

                            if (ConfigurationSettings.AppSettings["Is_Logging_frmLogin"] != null && ConfigurationSettings.AppSettings["Is_Logging_frmLogin"].ToString().ToUpper() == "Y")
                                sModule = "Yes";
                            break;
                        }
                    case "frmimageviewer":
                        {

                            if (ConfigurationSettings.AppSettings["Is_Logging_frmImageviewer"] != null && ConfigurationSettings.AppSettings["Is_Logging_frmImageviewer"].ToString().ToUpper() == "Y")
                                sModule = "Yes";
                            break;
                        }
                    default:
                        break;
                }
                if (sModule != string.Empty && sModule != "" && sModule == "Yes")
                {
                    if (!Directory.Exists(ConfigurationManager.AppSettings["UsageLogPath"]))
                        Directory.CreateDirectory(ConfigurationManager.AppSettings["UsageLogPath"]);
                    // string line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", EncounterID, "|" + HumanID, "|" + ProviderID, "|" + user_Name, "|" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:FFF"), "|" + Message_log);
                    // string line = EncounterID + "|" + HumanID + "|" + ProviderID + "|" + user_Name + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF") + "|" + Message_log + "|" + sGroupID.Replace(":","-");

                    string line = EncounterID + "|" + HumanID + "|" + ProviderID + "|" + user_Name + "|" + dtLogCurTime.ToString("yyyy-MM-dd HH:mm:ss:FFF") + "|" + Message_log + "|" + sGroupID.Replace(":", "-").Replace(" ", "-");

                    if (!File.Exists(Path.Combine(ConfigurationManager.AppSettings["UsageLogPath"], "UsageLog_" + HttpContext.Current.Session.SessionID + ".txt")))
                    {
                        using (StreamWriter sw = File.CreateText(Path.Combine(ConfigurationManager.AppSettings["UsageLogPath"], "UsageLog_" + HttpContext.Current.Session.SessionID + ".txt")))
                        {
                            sw.WriteLine(Environment.NewLine);
                            sw.WriteLine(line);
                        }
                    }

                    else
                    {
                        using (


                            StreamWriter outputFile = new StreamWriter(Path.Combine(ConfigurationManager.AppSettings["UsageLogPath"], "UsageLog_" + HttpContext.Current.Session.SessionID + ".txt"), true))
                        {

                            outputFile.WriteLine(Environment.NewLine);
                            outputFile.WriteLine(line);
                        }
                    }
                }

            }


        }

        public static void inserttologgingtableforSessionTimeout(string EventName, string RequestURL, string ExtraInfo)
        {
            string logflag = "N";
            if (ConfigurationSettings.AppSettings["Is_Logging_Globalasax"] != null)
                logflag = ConfigurationSettings.AppSettings["Is_Logging_Globalasax"].ToString().ToUpper();

            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session.SessionID != null && HttpContext.Current.Session.SessionID != "")
            {
            }
            else
            {
                if (logflag == "Y")
                {
                    try
                    {
                        if (!Directory.Exists(ConfigurationManager.AppSettings["UsageLogPath"]))
                            Directory.CreateDirectory(ConfigurationManager.AppSettings["UsageLogPath"]);

                        string line = "-----------------------------------------------" + Environment.NewLine;
                        line += "Event: " + EventName + Environment.NewLine;
                        line += "Nonsessionvariables: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF") + " | " + RequestURL + Environment.NewLine;
                        line += "HttpContext.Current.Session is null " + Environment.NewLine;
                        line += Environment.NewLine + "+++++++++++++++++++++++++++++++++++++++++++++++";

                        if (!File.Exists(Path.Combine(ConfigurationManager.AppSettings["UsageLogPath"], "UsageLog_SessionEnd.txt")))
                        {
                            using (StreamWriter sw = File.CreateText(Path.Combine(ConfigurationManager.AppSettings["UsageLogPath"], "UsageLog_SessionEnd.txt")))
                            {
                                sw.WriteLine(Environment.NewLine);
                                sw.WriteLine(line);
                            }
                        }

                        else
                        {
                            using (StreamWriter outputFile = new StreamWriter(Path.Combine(ConfigurationManager.AppSettings["UsageLogPath"], "UsageLog_SessionEnd.txt"), true))
                            {

                                outputFile.WriteLine(Environment.NewLine);
                                outputFile.WriteLine(line);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return;
            }

            if (logflag == "Y" && HttpContext.Current.Session.SessionID != "")
            {
                if (!Directory.Exists(ConfigurationManager.AppSettings["UsageLogPath"]))
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["UsageLogPath"]);

                string line = "-----------------------------------------------" + Environment.NewLine;
                line += "Event: " + EventName + Environment.NewLine;
                line += "Nonsessionvariables: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF") + " | " + RequestURL + Environment.NewLine;
                line += "Sessionvariables: " + HttpContext.Current.Session.SessionID + "| ";

                for (int iCount = 0; iCount < HttpContext.Current.Session.Keys.Count; iCount++)
                {
                    if (HttpContext.Current.Session[HttpContext.Current.Session.Keys[iCount]] != null && HttpContext.Current.Session[HttpContext.Current.Session.Keys[iCount]].ToString() != string.Empty)
                    {
                        line += HttpContext.Current.Session.Keys[iCount].ToString() + " - " + HttpContext.Current.Session[HttpContext.Current.Session.Keys[iCount]].ToString() + " | ";
                    }
                }

                if (ExtraInfo != string.Empty)
                {
                    line += Environment.NewLine + "Extravariables: " + ExtraInfo + Environment.NewLine;
                }

                line += Environment.NewLine + "+++++++++++++++++++++++++++++++++++++++++++++++";


                if (!File.Exists(Path.Combine(ConfigurationManager.AppSettings["UsageLogPath"], "UsageLog_" + HttpContext.Current.Session.SessionID + ".txt")))
                {
                    using (StreamWriter sw = File.CreateText(Path.Combine(ConfigurationManager.AppSettings["UsageLogPath"], "UsageLog_" + HttpContext.Current.Session.SessionID + ".txt")))
                    {
                        sw.WriteLine(Environment.NewLine);
                        sw.WriteLine(line);
                    }
                }

                else
                {
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(ConfigurationManager.AppSettings["UsageLogPath"], "UsageLog_" + HttpContext.Current.Session.SessionID + ".txt"), true))
                    {

                        outputFile.WriteLine(Environment.NewLine);
                        outputFile.WriteLine(line);
                    }
                }
            }
        }

        public static DateTime ConvertToUniversal(DateTime inputDateTime)
        {
            var request = ClientSession.UniversalTime.ToString();
            string[] Hours = ClientSession.UniversalTime.ToString().Split('.');

            DateTime dt = DateTime.MinValue;

            if (Hours != null && Hours[0] != string.Empty)
            {
                double offset = (double.Parse(Hours[0]));
                if (ClientSession.bFollows_DST)
                {
                    DateTime DayLight_StartDateTime;
                    DateTime DayLight_EndDateTime;
                    int iDSTStartDay = UtilityManager.FindDate(inputDateTime.Year, 3, DayOfWeek.Sunday, 2);
                    int iDSTEndDay = UtilityManager.FindDate(inputDateTime.Year, 11, DayOfWeek.Sunday, 1);
                    DayLight_StartDateTime = new DateTime(inputDateTime.Year, 3, iDSTStartDay, 2, 0, 0);
                    DayLight_EndDateTime = new DateTime(inputDateTime.Year, 11, iDSTEndDay, 2, 0, 0);
                    if ((inputDateTime.Ticks > DayLight_StartDateTime.Ticks) && (inputDateTime.Ticks < DayLight_EndDateTime.Ticks))
                    {
                        if (offset < 0)
                            offset += 1;
                        else
                            offset -= 1;
                    }
                }
                //Jira CAP-1185
                //if (inputDateTime.ToString() != "01-01-0001 12:00:00 AM")
                if (inputDateTime.ToString() != "01-01-0001 12:00:00 AM" && inputDateTime.ToString() != "1/1/0001 12:00:00 AM")
                {
                    if (Hours.Length != 0 && Hours.Length == 2)
                    {
                        dt = inputDateTime.AddHours(-offset).AddMinutes(-double.Parse(Hours[1]));
                    }
                }
                else
                {
                    dt = inputDateTime;
                }
            }

            return dt;

        }

        public static DateTime ConvertToLocal(DateTime inputDateTime)
        {
            //The following is the modified code
            DateTime dt = DateTime.MinValue;
            if (ClientSession.LocalOffSetTime != "")
            {
                double offset = -(double.Parse(ClientSession.LocalOffSetTime.ToString()));
                if (ClientSession.bFollows_DST)
                {
                    DateTime DayLight_StartDateTime;
                    DateTime DayLight_EndDateTime;
                    int iDSTStartDay = UtilityManager.FindDate(inputDateTime.Year, 3, DayOfWeek.Sunday, 2);
                    int iDSTEndDay = UtilityManager.FindDate(inputDateTime.Year, 11, DayOfWeek.Sunday, 1);
                    DayLight_StartDateTime = new DateTime(inputDateTime.Year, 3, iDSTStartDay, 2, 0, 0);
                    DayLight_EndDateTime = new DateTime(inputDateTime.Year, 11, iDSTEndDay, 2, 0, 0);
                    if ((inputDateTime.Ticks >= DayLight_StartDateTime.Ticks) && (inputDateTime.Ticks < DayLight_EndDateTime.Ticks))
                        offset += 60;
                }

                if (inputDateTime.ToString("dd-MM-yyyy hh:mm:ss tt") != "01-01-0001 12:00:00 AM")
                {
                    dt = inputDateTime.AddMinutes(offset);
                }
                else
                {
                    dt = inputDateTime;
                }
            }

            return dt;
        }

        public static int CalculateAge(DateTime birthDate)
        {
            // cache the current time
            DateTime now = DateTime.Today; // today is fine, don't need the timestamp from now
            // get the difference in years
            int years = now.Year - birthDate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;

            return years;
        }

        public static int CalculateAgeInDays(DateTime birthDate)
        {

            DateTime now = DateTime.Today;
            TimeSpan difference = now.Subtract(birthDate);
            return (int)difference.TotalDays;
        }
        public static int CalculateAgeByDOS(DateTime birthDate, DateTime DOS)
        {
            // cache the current time
            DateTime now = DOS; // today is fine, don't need the timestamp from now
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
        public static string FillPatientSummaryBar(string LastName, string FirstName, string MI, string Suffix, DateTime DOB, ulong ulHumanID, string MedRecNo, string HomePhoneNo, string Sex, string status, string SSN, string sPriPlan, string sPriCarrier, string sSecPlan)
        {

            string sMySummary;

            string sSex = string.Empty;
            if (Sex != null && Sex != "")
                sSex = Sex.Substring(0, 1);

            sMySummary = LastName + "," + FirstName +
               "  " + MI + "  " + Suffix + "   |   " +
               DOB.ToString("dd-MMM-yyyy") + "   |   " +
               (CalculateAge(DOB)).ToString() +
               "  year(s)    |   " + sSex + "   |   Acc #:" + ulHumanID +
               "   |   " + "Med Rec #:" + MedRecNo + "   |   " +
               "Phone #:" + HomePhoneNo + "   |   ";

            if (sPriPlan != string.Empty)
            {
                sMySummary += "Pri Plan:" + sPriCarrier + " - " + sPriPlan + "   |   ";
            }
            //if (sSecPlan != string.Empty)
            //{
            //    sMySummary += "Sec Plan:" + sSecCarrier+" - "+ sSecPlan + "   |   ";
            //}
            if (status != string.Empty)
            {
                sMySummary += "Patient Type:" + status + "   |   ";
            }
            if (SSN != string.Empty)
            {
                sMySummary += "SSN:" + SSN + "   |   ";
            }
            return sMySummary;
        }

        public static decimal MyConvertToDecimal(string MyString)
        {
            decimal deResult = 0;
            decimal.TryParse(MyString, out deResult);
            return (deResult);
        }

        private static Boolean mGrowing = false;

        public static int CalculateAgeInMonths(DateTime birthDate, DateTime now)
        {
            int leap = 0;
            int Months = 0;
            if (1 == now.Month || 3 == now.Month || 5 == now.Month || 7 == now.Month || 8 == now.Month ||
            10 == now.Month || 12 == now.Month)
            {
                leap = 31;
            }
            else if (2 == now.Month)
            {
                // Check for leap year
                if (0 == (now.Year % 4))
                {
                    // If date is divisible by 400, it's a leap year.
                    // Otherwise, if it's divisible by 100 it's not.
                    if (0 == (now.Year % 400))
                    {
                        leap = 29;
                    }
                    else if (0 == (now.Year % 100))
                    {
                        leap = 28;
                    }

                    // Divisible by 4 but not by 100 or 400
                    // so it leaps
                    leap = 29;
                }
                else
                {
                    leap = 28;
                }
                // Not a leap year

            }
            else
            {
                leap = 30;
            }

            if (leap == 28)
            {
                Months = Convert.ToInt16(now.Subtract(birthDate).Days / (365.25 / 12));
            }
            else if (leap == 31)
            {
                Months = Convert.ToInt16(now.Subtract(birthDate).Days / (365.25 / 12));
            }
            else if (leap == 29)
            {
                Months = Convert.ToInt16(now.Subtract(birthDate).Days / (366 / 12));
            }
            else if (leap == 30)
            {
                Months = Convert.ToInt16(now.Subtract(birthDate).Days / (365.25 / 12));
            }

            return Months;


        }

        public static int FindDate(int Year, int Month, DayOfWeek Day, int Occurrence)
        {
            if (Occurrence == 0 || Occurrence > 5)
                return 0;

            DateTime FirstDayOfMonth = new DateTime(Year, Month, 1);
            int days_needed = (int)Day - (int)FirstDayOfMonth.DayOfWeek;
            if (days_needed < 0)
                days_needed += 7;
            int resultDate = days_needed + 1 + (7 * (Occurrence - 1));
            return resultDate;
        }

        public IList<string> LoadPatientSummaryUsingList(IList<int> ilstChangeSummaryBar, out List<string> ilstToolTips)
        {
            IList<string> strSummary = new List<string>();
            EncounterManager objEncounterManager = new EncounterManager();

            ilstToolTips = new List<string>();

            var CCText = string.Empty;

            var VitalsText = string.Empty;
            var VitalsToolTip = string.Empty;

            var AlergicText = string.Empty;

            var ProblemListText = string.Empty;

            var MedicationText = string.Empty;

            var toolTipText = string.Empty;

            StringBuilder sbToolTip = new StringBuilder();


            Page page = new Page();
            page.Session["CheckPatientPane"] = "true";
            FillPatientSummaryBarDTO illstPatientSummary = FillPatientSummaryBar();
            //var illstPatientSummary = objEncounterManager.LoadPatientSummaryBarUsingList(ClientSession.Selectedencounterid, ClientSession.HumanId,
            //     UtilityManager.ConvertToLocal(DateTime.Now),
            //    ClientSession.UserName, ilstChangeSummaryBar);
            page.Session.Remove("CheckPatientPane");


            AlergicText = GetAllergyInfo(illstPatientSummary.NonDrugAllergyList,
                illstPatientSummary.AllergyList, out toolTipText);
            sbToolTip.Append(toolTipText);
            strSummary.Add("Allergy-" + AlergicText);


            CCText = GetChiefComplaintsInfo(illstPatientSummary.ChiefComplaintList, out toolTipText);
            sbToolTip.Append("<br/>");
            sbToolTip.Append(toolTipText);
            strSummary.Add("CC-" + CCText);

            ProblemListText = GetProblemList(illstPatientSummary.PblmMedList, out toolTipText);
            sbToolTip.Append("<br/>");
            sbToolTip.Append(toolTipText);
            strSummary.Add("ProblemList-" + ProblemListText);
            //ilstToolTips.Add(sbToolTip.ToString());


            VitalsText = GetVitalsInfo(illstPatientSummary.VitalsList, out toolTipText);
            VitalsToolTip = toolTipText;
            sbToolTip.Append("<br/>");
            sbToolTip.Append(toolTipText);
            strSummary.Add("Vitals-" + VitalsText);


            MedicationText = GetMedication(illstPatientSummary, out toolTipText);
            sbToolTip.Append("<br/>");
            sbToolTip.Append(toolTipText);
            strSummary.Add("Medication-" + VitalsText);

            ilstToolTips.Add(sbToolTip.ToString());
            if (ilstChangeSummaryBar.Contains(2))
                ilstToolTips.Add(VitalsToolTip);
            return strSummary;



        }

        public IList<string> LoadPatientSummaryList(out List<string> ilstToolTips)
        {
            IList<string> strSummary = new List<string>();
            EncounterManager objEncounterManager = new EncounterManager();

            ilstToolTips = new List<string>();

            var CCText = string.Empty;

            var VitalsText = string.Empty;
            var VitalsToolTip = string.Empty;

            var AlergicText = string.Empty;

            var ProblemListText = string.Empty;

            var MedicationText = string.Empty;

            var toolTipText = string.Empty;

            StringBuilder sbToolTip = new StringBuilder();

            Page page = new Page();
            page.Session["CheckPatientPane"] = "true";
            FillPatientSummaryBarDTO illstPatientSummary = FillPatientSummaryBar();
            //var illstPatientSummary = objEncounterManager.LoadPatientSummaryBar(ClientSession.Selectedencounterid, ClientSession.HumanId,
            //     UtilityManager.ConvertToLocal(DateTime.Now),
            //    ClientSession.UserName);
            page.Session.Remove("CheckPatientPane");


            AlergicText = GetAllergyInfo(illstPatientSummary.NonDrugAllergyList,
                illstPatientSummary.AllergyList, out toolTipText);
            sbToolTip.Append(toolTipText);
            strSummary.Add("Allergy-" + AlergicText);


            CCText = GetChiefComplaintsInfo(illstPatientSummary.ChiefComplaintList, out toolTipText);
            sbToolTip.Append("<br/>");
            sbToolTip.Append(toolTipText);
            strSummary.Add("CC-" + CCText);

            ProblemListText = GetProblemList(illstPatientSummary.PblmMedList, out toolTipText);
            sbToolTip.Append("<br/>");
            sbToolTip.Append(toolTipText);
            strSummary.Add("ProblemList-" + ProblemListText);
            ilstToolTips.Add(sbToolTip.ToString());


            VitalsText = GetVitalsInfo(illstPatientSummary.VitalsList, out toolTipText);
            VitalsToolTip = toolTipText;
            sbToolTip.Append("<br/>");
            sbToolTip.Append(toolTipText);
            strSummary.Add("Vitals-" + VitalsText);

            MedicationText = GetMedication(illstPatientSummary, out toolTipText);
            sbToolTip.Append("<br/>");
            sbToolTip.Append(toolTipText);
            strSummary.Add("Medication-" + VitalsText);

            return strSummary;

        }

        public FillPatientSummaryBarDTO FillPatientSummaryBar()
        {
            FillPatientSummaryBarDTO objFillPatientChart = new FillPatientSummaryBarDTO();
            DateTime CurrentDOS = DateTime.MinValue;
            if (ClientSession.EncounterId != 0)//BugID:52634,52632 
            {
                IList<string> ilstPatientSummaryBarTagEncounterList = new List<string>();
                ilstPatientSummaryBarTagEncounterList.Add("EncounterList");
                ilstPatientSummaryBarTagEncounterList.Add("ChiefComplaintsList");
            loop1:
                try
                {
                    IList<object> ilstEncBlobFinal = new List<object>();
                    ilstEncBlobFinal = UtilityManager.ReadBlob(ClientSession.EncounterId, ilstPatientSummaryBarTagEncounterList);

                    if (ilstEncBlobFinal != null && ilstEncBlobFinal.Count > 0)
                    {
                        if (ilstEncBlobFinal[0] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstEncBlobFinal[0]).Count; iCount++)
                            {
                                objFillPatientChart.EncounterIDList.Add(Convert.ToUInt32(((Encounter)((IList<object>)ilstEncBlobFinal[0])[iCount]).Id));

                                objFillPatientChart.EncounterDateList.Add(Convert.ToDateTime(((Encounter)((IList<object>)ilstEncBlobFinal[0])[iCount]).Date_of_Service));
                            }
                        }

                        if (ilstEncBlobFinal[1] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstEncBlobFinal[1]).Count; iCount++)
                            {
                                if (((ChiefComplaints)((IList<object>)ilstEncBlobFinal[1])[iCount]).HPI_Element == "Chief Complaints")
                                {
                                    objFillPatientChart.ChiefComplaintList.Add((ChiefComplaints)((IList<object>)ilstEncBlobFinal[1])[iCount]);
                                }
                            }
                        }
                    }

                }


                catch (Exception ex)
                {
                    // if (ex.Message.ToLower().Contains("input string was not") == true || ex.Message.ToLower().Contains("element") == true||ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('1011190');", true);

                        //XmlText.Close();
                        UtilityManager.GenerateXML(ClientSession.EncounterId.ToString(), "Encounter");
                        goto loop1;
                    }

                }
            }

            IList<string> ilstPatientSummaryBarTagHumanList = new List<string>();
            ilstPatientSummaryBarTagHumanList.Add("ProblemListList");
            ilstPatientSummaryBarTagHumanList.Add("PatientResultsList");
            ilstPatientSummaryBarTagHumanList.Add("Rcopia_MedicationList");
            ilstPatientSummaryBarTagHumanList.Add("Rcopia_AllergyList");
            ilstPatientSummaryBarTagHumanList.Add("NonDrugAllergyList");
        ln:
            try
            {
                IList<object> ilstHumanBlobFinal = new List<object>();
                ilstHumanBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstPatientSummaryBarTagHumanList);

                if (ilstHumanBlobFinal != null && ilstHumanBlobFinal.Count > 0)
                {
                    if (ilstHumanBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobFinal[0]).Count; iCount++)
                        {
                            objFillPatientChart.PblmMedList.Add((ProblemList)((IList<object>)ilstHumanBlobFinal[0])[iCount]);
                        }
                    }

                    if (ilstHumanBlobFinal[1] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobFinal[1]).Count; iCount++)
                        {
                            if (((PatientResults)((IList<object>)ilstHumanBlobFinal[1])[iCount]).Encounter_ID == ClientSession.EncounterId && ((PatientResults)((IList<object>)ilstHumanBlobFinal[1])[iCount]).Results_Type == "Vitals")
                            {
                                objFillPatientChart.VitalsList.Add((PatientResults)((IList<object>)ilstHumanBlobFinal[1])[iCount]);
                            }
                        }
                    }

                    if (ilstHumanBlobFinal[2] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobFinal[2]).Count; iCount++)
                        {
                            if (((Rcopia_Medication)((IList<object>)ilstHumanBlobFinal[2])[iCount]).Human_ID == ClientSession.HumanId && ((Rcopia_Medication)((IList<object>)ilstHumanBlobFinal[2])[iCount]).Deleted == "N")
                            {
                                objFillPatientChart.MedicationList.Add((Rcopia_Medication)((IList<object>)ilstHumanBlobFinal[2])[iCount]);
                            }
                        }
                    }

                    if (ilstHumanBlobFinal[3] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobFinal[3]).Count; iCount++)
                        {
                            if (((Rcopia_Allergy)((IList<object>)ilstHumanBlobFinal[3])[iCount]).Human_ID == ClientSession.HumanId && ((Rcopia_Allergy)((IList<object>)ilstHumanBlobFinal[3])[iCount]).Deleted == "N")
                            {
                                objFillPatientChart.AllergyList.Add((Rcopia_Allergy)((IList<object>)ilstHumanBlobFinal[3])[iCount]);
                            }
                        }
                    }

                    if (ilstHumanBlobFinal[4] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobFinal[4]).Count; iCount++)
                        {
                            if (((NonDrugAllergy)((IList<object>)ilstHumanBlobFinal[4])[iCount]).Is_Present == "Y")
                            {
                                objFillPatientChart.NonDrugAllergyList.Add((NonDrugAllergy)((IList<object>)ilstHumanBlobFinal[4])[iCount]);
                            }
                        }

                        if (objFillPatientChart.NonDrugAllergyList != null && objFillPatientChart.NonDrugAllergyList.Count > 0)
                        {
                            IList<NonDrugAllergy> lstNDACurrEnc = new List<NonDrugAllergy>();
                            lstNDACurrEnc = (from item in objFillPatientChart.NonDrugAllergyList where item.Encounter_Id == ClientSession.EncounterId select item).ToList<NonDrugAllergy>();
                            if (lstNDACurrEnc != null && lstNDACurrEnc.Count > 0)
                            {
                                objFillPatientChart.NonDrugAllergyList = lstNDACurrEnc;
                            }
                            else
                            {
                                IList<ulong> lstEncId = (from item in objFillPatientChart.NonDrugAllergyList select item.Encounter_Id).Distinct().ToList<ulong>();
                                ulong maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                                foreach (ulong item in lstEncId)
                                    if (item > maxEncId && item < ClientSession.EncounterId)
                                        maxEncId = item;
                                lstNDACurrEnc = (from item in objFillPatientChart.NonDrugAllergyList where item.Encounter_Id == maxEncId select item).ToList<NonDrugAllergy>();
                                objFillPatientChart.NonDrugAllergyList = lstNDACurrEnc;
                            }
                        }
                        else
                        {
                            objFillPatientChart.NonDrugAllergyList = objFillPatientChart.NonDrugAllergyList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // if (ex.Message.ToLower().Contains("input string was not") == true || ex.Message.ToLower().Contains("element") == true||ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('1011190');", true);

                    //XmlText.Close();
                    UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");
                    goto ln;
                }

            }

            //    string FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            //    string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //loop1:
            //    if (File.Exists(strXmlFilePath) == true)
            //    {
            //        XmlDocument itemDoc = new XmlDocument();
            //        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //        XmlNodeList xmlTagName = null;
            //        // itemDoc.Load(XmlText);
            //        try
            //        {
            //            using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //            {
            //                itemDoc.Load(fs);

            //                XmlText.Close();
            //                if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
            //                {
            //                    xmlTagName = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes;

            //                    if (xmlTagName.Count > 0)
            //                    {
            //                        for (int j = 0; j < xmlTagName.Count; j++)
            //                        {
            //                            objFillPatientChart.EncounterIDList.Add(Convert.ToUInt32(xmlTagName[j].Attributes.GetNamedItem("Id").Value));

            //                            objFillPatientChart.EncounterDateList.Add(Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Date_of_Service").Value));
            //                            //if (xmlTagName[j].Attributes[0].Value == ClientSession.EncounterId.ToString())
            //                            //    CurrentDOS = Convert.ToDateTime(xmlTagName[j].Attributes[4].Value);
            //                        }
            //                        //    for (int j = 0; j < xmlTagName.Count; j++)
            //                        //    {
            //                        //       if (EncounterID.Count < 2)
            //                        //{
            //                        //    if (EncounterLst[k].Date_of_Service <= CurrentDOS)
            //                        //    {
            //                        //        if (CurrentDOS.ToString() != DateTime.MinValue.ToString())
            //                    }
            //                }
            //                if (itemDoc.GetElementsByTagName("ChiefComplaintsList")[0] != null)
            //                {
            //                    xmlTagName = itemDoc.GetElementsByTagName("ChiefComplaintsList")[0].ChildNodes;

            //                    if (xmlTagName.Count > 0)
            //                    {
            //                        for (int j = 0; j < xmlTagName.Count; j++)
            //                        {
            //                            if (xmlTagName[j].Attributes.GetNamedItem("HPI_Element").Value == "Chief Complaints")
            //                            {
            //                                string TagName = xmlTagName[j].Name;
            //                                XmlSerializer xmlserializer = new XmlSerializer(typeof(ChiefComplaints));
            //                                ChiefComplaints ChiefComplaints = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ChiefComplaints;
            //                                IEnumerable<PropertyInfo> propInfo = null;
            //                                ChiefComplaints = (ChiefComplaints)ChiefComplaints;
            //                                propInfo = from obji in ((ChiefComplaints)ChiefComplaints).GetType().GetProperties() select obji;

            //                                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                                {
            //                                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                                    {
            //                                        foreach (PropertyInfo property in propInfo)
            //                                        {
            //                                            if (property.Name == nodevalue.Name)
            //                                            {
            //                                                if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                    property.SetValue(ChiefComplaints, Convert.ToUInt64(nodevalue.Value), null);
            //                                                else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                    property.SetValue(ChiefComplaints, Convert.ToString(nodevalue.Value), null);
            //                                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                    property.SetValue(ChiefComplaints, Convert.ToDateTime(nodevalue.Value), null);
            //                                                else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                    property.SetValue(ChiefComplaints, Convert.ToInt32(nodevalue.Value), null);
            //                                                else
            //                                                    property.SetValue(ChiefComplaints, nodevalue.Value, null);
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                                objFillPatientChart.ChiefComplaintList.Add(ChiefComplaints);
            //                            }
            //                        }

            //                    }
            //                }
            //                fs.Close();
            //                fs.Dispose();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            // if (ex.Message.ToLower().Contains("input string was not") == true || ex.Message.ToLower().Contains("element") == true||ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
            //            {
            //                //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('1011190');", true);

            //                XmlText.Close();
            //                UtilityManager.GenerateXML(ClientSession.EncounterId.ToString(), "Encounter");
            //                goto loop1;
            //            }

            //        }
            //    }
            //}


            //        string HumanFileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //    string HumanXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
            //ln:
            //    if (File.Exists(HumanXmlFilePath) == true)
            //{
            //    XmlTextReader XmlText = new XmlTextReader(HumanXmlFilePath);
            //    try
            //    {
            //        XmlDocument itemDoc = new XmlDocument();

            //        XmlNodeList xmlTagName = null;
            //        // itemDoc.Load(XmlText);
            //        using (FileStream fs = new FileStream(HumanXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //        {
            //            itemDoc.Load(fs);

            //            XmlText.Close();

            //            if (itemDoc.GetElementsByTagName("ProblemListList")[0] != null)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("ProblemListList")[0].ChildNodes;

            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {

            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(ProblemList));
            //                        ProblemList ProblemList = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ProblemList;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        ProblemList = (ProblemList)ProblemList;
            //                        propInfo = from obji in ((ProblemList)ProblemList).GetType().GetProperties() select obji;

            //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                        {
            //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                            {
            //                                foreach (PropertyInfo property in propInfo)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(ProblemList, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(ProblemList, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(ProblemList, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(ProblemList, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(ProblemList, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                        objFillPatientChart.PblmMedList.Add(ProblemList);
            //                    }
            //                }
            //            }

            //            if (itemDoc.GetElementsByTagName("PatientResultsList")[0] != null)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("PatientResultsList")[0].ChildNodes;

            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {

            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(PatientResults));
            //                        PatientResults PatientResults = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as PatientResults;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        PatientResults = (PatientResults)PatientResults;
            //                        propInfo = from obji in ((PatientResults)PatientResults).GetType().GetProperties() select obji;

            //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                        {
            //                            if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value) == ClientSession.EncounterId && (Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Results_Type").Value) == "Vitals"))
            //                            {
            //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                                {
            //                                    foreach (PropertyInfo property in propInfo)
            //                                    {
            //                                        if (property.Name == nodevalue.Name)
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(PatientResults, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(PatientResults, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(PatientResults, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(PatientResults, Convert.ToInt32(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(PatientResults, nodevalue.Value, null);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                        objFillPatientChart.VitalsList.Add(PatientResults);
            //                    }
            //                }
            //            }
            //            objFillPatientChart.Vitals = objFillPatientChart.VitalsList.Count;
            //            if (itemDoc.GetElementsByTagName("Rcopia_MedicationList")[0] != null)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("Rcopia_MedicationList")[0].ChildNodes;

            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {
            //                        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Human_ID").Value) == ClientSession.HumanId && xmlTagName[j].Attributes.GetNamedItem("Deleted").Value == "N")
            //                        {

            //                            //if (((DateTime.Compare(objFillPatientChart.EncounterDateList[0], Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Start_Date").Value)) >= 0)
            //                            //        && (Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Stop_Date").Value) != DateTime.MinValue) && (Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Start_Date").Value) != DateTime.MinValue)
            //                            //        && (DateTime.Compare(objFillPatientChart.EncounterDateList[0], Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Stop_Date").Value)) <= 0)) ||
            //                            //       ((DateTime.Compare(objFillPatientChart.EncounterDateList[0], Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Start_Date").Value)) >= 0)
            //                            //       && (Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Stop_Date").Value) == DateTime.MinValue) && (Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Start_Date").Value) != DateTime.MinValue)
            //                            //        ))
            //                            //{

            //                            string TagName = xmlTagName[j].Name;
            //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(Rcopia_Medication));
            //                            Rcopia_Medication Rcopia_Medication = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Rcopia_Medication;
            //                            IEnumerable<PropertyInfo> propInfo = null;
            //                            Rcopia_Medication = (Rcopia_Medication)Rcopia_Medication;
            //                            propInfo = from obji in ((Rcopia_Medication)Rcopia_Medication).GetType().GetProperties() select obji;

            //                            // if ((objFillPatientChart.EncounterDateList[0] > Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Start_Date").Value)) &&(objFillPatientChart.EncounterDateList[0] < Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Stop_Date").Value)))

            //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                            {
            //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                                {
            //                                    foreach (PropertyInfo property in propInfo)
            //                                    {
            //                                        if (property.Name == nodevalue.Name)
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(Rcopia_Medication, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(Rcopia_Medication, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(Rcopia_Medication, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(Rcopia_Medication, Convert.ToInt32(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(Rcopia_Medication, nodevalue.Value, null);
            //                                        }
            //                                    }
            //                                }

            //                            }



            //                            objFillPatientChart.MedicationList.Add(Rcopia_Medication);
            //                            //}
            //                        }
            //                    }
            //                }
            //            }

            //            if (itemDoc.GetElementsByTagName("Rcopia_AllergyList")[0] != null)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("Rcopia_AllergyList")[0].ChildNodes;

            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {
            //                        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Human_ID").Value) == ClientSession.HumanId && xmlTagName[j].Attributes.GetNamedItem("Deleted").Value.Equals("N"))
            //                        {
            //                            string TagName = xmlTagName[j].Name;
            //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(Rcopia_Allergy));
            //                            Rcopia_Allergy Rcopia_Allergy = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Rcopia_Allergy;
            //                            IEnumerable<PropertyInfo> propInfo = null;
            //                            Rcopia_Allergy = (Rcopia_Allergy)Rcopia_Allergy;
            //                            propInfo = from obji in ((Rcopia_Allergy)Rcopia_Allergy).GetType().GetProperties() select obji;

            //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                            {
            //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                                {
            //                                    foreach (PropertyInfo property in propInfo)
            //                                    {
            //                                        if (property.Name == nodevalue.Name)
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(Rcopia_Allergy, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(Rcopia_Allergy, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(Rcopia_Allergy, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(Rcopia_Allergy, Convert.ToInt32(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(Rcopia_Allergy, nodevalue.Value, null);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                            objFillPatientChart.AllergyList.Add(Rcopia_Allergy);
            //                        }
            //                    }
            //                }
            //            }
            //            if (itemDoc.GetElementsByTagName("NonDrugAllergyList")[0] != null)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("NonDrugAllergyList")[0].ChildNodes;

            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {
            //                        if ((xmlTagName[j].Attributes.GetNamedItem("Is_Present").Value == "Y"))
            //                        {
            //                            string TagName = xmlTagName[j].Name;
            //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(NonDrugAllergy));
            //                            NonDrugAllergy NonDrugAllergy = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as NonDrugAllergy;
            //                            IEnumerable<PropertyInfo> propInfo = null;
            //                            NonDrugAllergy = (NonDrugAllergy)NonDrugAllergy;
            //                            propInfo = from obji in ((NonDrugAllergy)NonDrugAllergy).GetType().GetProperties() select obji;

            //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                            {

            //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                                {
            //                                    foreach (PropertyInfo property in propInfo)
            //                                    {
            //                                        if (property.Name == nodevalue.Name)
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(NonDrugAllergy, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(NonDrugAllergy, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(NonDrugAllergy, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(NonDrugAllergy, Convert.ToInt32(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(NonDrugAllergy, nodevalue.Value, null);
            //                                        }
            //                                    }
            //                                }

            //                            }

            //                            objFillPatientChart.NonDrugAllergyList.Add(NonDrugAllergy);
            //                        }
            //                    }
            //                }
            //            }
            //            if (objFillPatientChart.NonDrugAllergyList != null && objFillPatientChart.NonDrugAllergyList.Count > 0)
            //            {
            //                IList<NonDrugAllergy> lstNDACurrEnc = new List<NonDrugAllergy>();
            //                lstNDACurrEnc = (from item in objFillPatientChart.NonDrugAllergyList where item.Encounter_Id == ClientSession.EncounterId select item).ToList<NonDrugAllergy>();
            //                if (lstNDACurrEnc != null && lstNDACurrEnc.Count > 0)
            //                {
            //                    objFillPatientChart.NonDrugAllergyList = lstNDACurrEnc;
            //                }
            //                else
            //                {
            //                    IList<ulong> lstEncId = (from item in objFillPatientChart.NonDrugAllergyList select item.Encounter_Id).Distinct().ToList<ulong>();
            //                    ulong maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
            //                    foreach (ulong item in lstEncId)
            //                        if (item > maxEncId && item < ClientSession.EncounterId)
            //                            maxEncId = item;
            //                    lstNDACurrEnc = (from item in objFillPatientChart.NonDrugAllergyList where item.Encounter_Id == maxEncId select item).ToList<NonDrugAllergy>();
            //                    objFillPatientChart.NonDrugAllergyList = lstNDACurrEnc;
            //                }
            //            }
            //            else
            //            {
            //                objFillPatientChart.NonDrugAllergyList = objFillPatientChart.NonDrugAllergyList;
            //            }
            //            fs.Close();
            //            fs.Dispose();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        // if (ex.Message.ToLower().Contains("input string was not") == true || ex.Message.ToLower().Contains("element") == true||ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
            //        {
            //            //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('1011190');", true);

            //            XmlText.Close();
            //            UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");
            //            goto ln;
            //        }

            //    }
            //}
            return objFillPatientChart;
        }

        public string AddVitalUnits(string Name)
        {
            switch (Name)
            {
                case "Height":
                    {
                        return " Feet'" + " inches";
                    }
                case "Weight":
                    {
                        return " lbs";
                    }
                case "Body Temperature":
                    {
                        return " F";
                    }
                case "BP-Sitting Sys/Dia":
                    {
                        return " mm/Hg";
                    }
                case "BP-Standing Sys/Dia":
                    {
                        return " mm/Hg";
                    }
                case "BP-Lying Sys/Dia":
                    {
                        return " mm/Hg";
                    }
                case "Respiratory Rate":
                    {
                        return " Breaths/min";
                    }
                case "Heart Rate":
                    {
                        return " Beats/min";
                    }
                case "Pulse Oximetry":
                    {
                        return " %";
                    }
                case "HbA1C":
                    {
                        return " %";
                    }
                case "eGFR":
                    {
                        return " ml/min/1.73m2";
                    }
                case "Blood Sugar-Fasting":
                    {
                        return " mg/dl";
                    }
                case "Blood Sugar-Post Prandial":
                    {
                        return " mg/dl";
                    }
                case "BASAL":
                    {
                        return " units/ hr";
                    }
                case "Head Circumference":
                    {
                        return " Inches";
                    }
                case "Oxygen Intake":
                    {
                        return " LPM";
                    }
                case "Oxygen Saturation":
                    {
                        return " %";
                    }
                case "Neck Size":
                    {
                        return " Inches";
                    }
                case "Pulse Rate":
                    {
                        return " Beats/min";
                    }
                case "Waist Circumference":
                    {
                        return " cm";
                    }
                case "LDL":
                    {
                        return " mg/dl";
                    }
                case "HDL":
                    {
                        return " mg/dl";
                    }
                case "Total Cholesterol":
                    {
                        return " mg/dl";
                    }
                case "Triglycerides":
                    {
                        return " mg/dl";
                    }
                case "Creatinine":
                    {
                        return " mg/dl";
                    }
                case "TSH":
                    {
                        return " mIU/L";
                    }
                case "Hgb":
                    {
                        return " g/dL";
                    }
                case "Calcium Score":
                    {
                        return " mg/dL";
                    }
                case "Lp(a)":
                    {
                        return " mg/dL";
                    }
                case "ISF": //added by naveena for bug_id=26435 on 31.10.2014
                    {
                        return " Mg/dL";
                    }
                case "ICR":
                    {
                        return " gms";
                    }
                case "PT":
                    {
                        return " sec.";
                    }
                case "Urine for Microalbumin":
                    {
                        return " mg/dL";
                    }
                case "Inhaled Oxygen Concentration":
                    {
                        return " %";
                    }
                case "Urine Microalbumin":
                    {
                        return " mg/L";
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        public string ConvertInchtoFeetInch(string s)
        {
            if (s == string.Empty)
            {
                return s;
            }
            decimal inch = Convert.ToDecimal(s);
            decimal feet = Math.Floor(inch / 12m);
            decimal remainInch = decimal.Round((inch % 12m), 2);
            return feet.ToString() + "'" + " " + remainInch.ToString() + "''";
        }

        public string ConversionOnRetrieval(string vitalName, string vitalValue)
        {
            int j = 0;
            string MethdName = ConvertInchtoFeetInch(vitalValue.ToString());
            string[] Splitter = { ".", "(", ",", ")" };
            string[] MthdInfo = MethdName.Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
            if (MethdName.Length > 0)
            {

                string[] Arguments = new string[MthdInfo.Length];
                string ClassName = string.Empty;
                //string MethodName = MthdInfo[1];
                Arguments[j] = vitalValue;
                j++;
                return MethdName;
            }
            else
                return string.Empty;
        }

        public string ValidateVitalUnits(string Name, string Value, string AbnormalValue)
        {
            if (Value.Trim() != string.Empty)
            {
                switch (Name)
                {
                    case "Body Temperature":
                        {
                            string[] Split = AbnormalValue.Split('-');
                            if (Convert.ToDecimal(Value) < Convert.ToDecimal(Split[0]) || Convert.ToDecimal(Value) > Convert.ToDecimal(Split[1]))
                                return Color.Red.Name;
                            else
                                return Color.Black.Name;
                        }
                    case "BP-Sitting Sys/Dia":
                        {
                            string[] SplitNormal = Value.Split('/');
                            string[] Split = AbnormalValue.Split('/');
                            if (Convert.ToInt16(SplitNormal[0]) < Convert.ToInt16(Split[0]) || Convert.ToInt16(SplitNormal[1]) > Convert.ToInt16(Split[1]))
                                return Color.Red.Name;
                            else
                                return Color.Black.Name;
                        }
                    case "BP-Standing Sys/Dia":
                        {
                            string[] SplitNormal = Value.Split('/');
                            string[] Split = AbnormalValue.Split('/');
                            if (Convert.ToInt16(SplitNormal[0]) < Convert.ToInt16(Split[0]) || Convert.ToInt16(SplitNormal[1]) > Convert.ToInt16(Split[1]))
                                return Color.Red.Name;
                            else
                                return Color.Black.Name;
                        }
                    case "BP-Lying Sys/Dia":
                        {
                            string[] SplitNormal = Value.Split('/');
                            string[] Split = AbnormalValue.Split('/');
                            if (Convert.ToInt16(SplitNormal[0]) < Convert.ToInt16(Split[0]) || Convert.ToInt16(SplitNormal[1]) > Convert.ToInt16(Split[1]))
                                return Color.Red.Name;
                            else
                                return Color.Black.Name;
                        }
                    case "Respiratory Rate":
                        {
                            string[] Split = AbnormalValue.Split('-');
                            if (Convert.ToDecimal(Value) < Convert.ToDecimal(Split[0]) || Convert.ToDecimal(Value) > Convert.ToDecimal(Split[1]))
                                return Color.Red.Name;
                            else
                                return Color.Black.Name;
                        }
                    case "Heart Rate":
                        {
                            string[] Split = AbnormalValue.Split('-');
                            if (Convert.ToDecimal(Value) < Convert.ToDecimal(Split[0]) || Convert.ToDecimal(Value) > Convert.ToDecimal(Split[1]))
                                return Color.Red.Name;
                            else
                                return Color.Black.Name;
                        }
                    case "Pulse Oximetry":
                        {
                            string[] Split = AbnormalValue.Split('-');
                            if (Convert.ToDecimal(Value) < Convert.ToDecimal(Split[0]) || Convert.ToDecimal(Value) > Convert.ToDecimal(Split[1]))
                                return Color.Red.Name;
                            else
                                return Color.Black.Name;
                        }
                    case "BMI Status":
                        {
                            if (Value.Contains(AbnormalValue) == false)
                                return Color.Red.Name;
                            else
                                return Color.Black.Name;
                        }
                    case "HbA1C Status":
                        {
                            if (Value != AbnormalValue)
                                return Color.Red.Name;
                            else
                                return Color.Black.Name;
                        }
                    case "eGFR Status":
                        {
                            if (Value != AbnormalValue)
                                return Color.Red.Name;
                            else
                                return Color.Black.Name;
                        }
                    case "Blood Sugar-Post Prandial Status":
                        {
                            if (Value.Contains(AbnormalValue) == false)
                                return Color.Red.Name;
                            else
                                return Color.Black.Name;
                        }
                    case "Blood Sugar-Fasting Status":
                        {
                            if (Value.Contains(AbnormalValue) == false)
                                return Color.Red.Name;
                            else
                                return Color.Black.Name;
                        }

                    default:
                        {
                            return Color.Black.Name;
                        }
                }
            }
            else
            {
                return Color.Black.Name;
            }
        }

        public string GetVitalsInfo(IList<PatientResults> ilstVitalsList, out string toolTipText)
        {
            toolTipText = string.Empty;

            IList<object> objList = new List<object>();

            var Text = string.Empty;
            var sVitalsText = string.Empty;
            StringBuilder sb_Vitals = new StringBuilder("<span class=\"BlockObjects\" style=\"color:Black;\">Vitals :</span><br/>");

            StringBuilder sb_VitalsToolTip = new StringBuilder("Vitals :<br/>");
            DateTime MaxCapturedDate = DateTime.MinValue;
            IList<PatientResults> OrderedVitalList = new List<PatientResults>();
            StaticLookupManager objStaticLookupManager = new StaticLookupManager();
            IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
            IList<String> vitalslist = new List<String>();
            StaticLookup objStaticLookup = new StaticLookup();


            //BugID:54697__ Removed Clientsession variable "VitalLimitLookup".Instead read from XML in Read Mode.
            //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        XmlDocument itemDoc = new XmlDocument();
            //        XmlTextReader xmltxtReader = new XmlTextReader(fs);
            //        itemDoc.Load(xmltxtReader);
            //        xmltxtReader.Close();
            //        XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("VitalList");
            //        if (xmlNodeList.Count > 0)
            //        {
            //            for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
            //            {
            //                objStaticLookup = new StaticLookup();
            //                objStaticLookup.Value = xmlNodeList[0].ChildNodes[j].Attributes[1].Value;
            //                objStaticLookup.Description = xmlNodeList[0].ChildNodes[j].Attributes[2].Value;
            //                iFieldLookupList.Add(objStaticLookup);
            //            }
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}

            //CAP-2787
            StaticLookupList staticLookupList = ConfigureBase<StaticLookupList>.ReadJson("staticlookup.json");
            if (staticLookupList != null)
            {
                var vitalList = staticLookupList.VitalList.ToList();
                if (vitalList != null)
                {
                    foreach (var vital in vitalList)
                    {
                        objStaticLookup = new StaticLookup();
                        objStaticLookup.Value = vital.value;
                        objStaticLookup.Description = vital.Description;
                        iFieldLookupList.Add(objStaticLookup);

                    }

                }
            }
            /*  if (ClientSession.VitalLimitLookup.Count > 0)
               iFieldLookupList = ClientSession.VitalLimitLookup;
           else
           {
               // iFieldLookupList = objStaticLookupManager.getStaticLookupByFieldName("VITALS LIMIT LEVEL");
               StaticLookup objStaticLookup = new StaticLookup();

               string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
               if (File.Exists(strXmlFilePath) == true)
               {
                   XmlDocument itemDoc = new XmlDocument();
                   XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                   itemDoc.Load(XmlText);
                   XmlText.Close();
                   XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("VitalList");
                   if (xmlNodeList.Count > 0)
                   {
                       for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                       {
                           objStaticLookup = new StaticLookup();
                           objStaticLookup.Value = xmlNodeList[0].ChildNodes[j].Attributes[1].Value;
                           objStaticLookup.Description = xmlNodeList[0].ChildNodes[j].Attributes[2].Value;
                           iFieldLookupList.Add(objStaticLookup);
                       }
                   }
               }
               ClientSession.VitalLimitLookup = iFieldLookupList;
           }
          * */
            //BugID:47874

            IList<MasterVitals> masterlist = new List<MasterVitals>();
            MapVitalsPhysicianManager objMapMngr = new MapVitalsPhysicianManager();
            IList<MapVitalsPhysician> mapVitalList = objMapMngr.GetVitalsForPhysician(ClientSession.PhysicianId);
            mapVitalList = mapVitalList.OrderBy(a => a.Sort_Order).ToList<MapVitalsPhysician>();
            VitalsManager objVMngr = new VitalsManager();
            IList<DynamicScreen> dynamicScreenlst = new List<DynamicScreen>();
            IList<DynamicScreen> dynList = objVMngr.LoadDynamicScreenXML();
            dynamicScreenlst = (from objMapVit in mapVitalList
                                join obj in dynList
                                    on objMapVit.Master_Vitals_ID equals obj.Master_Vitals_ID
                                select obj).ToList<DynamicScreen>();
            //dynamicScreenlst = (from obj in dynList where mapVitalList.Any(a => a.Master_Vitals_ID == obj.Master_Vitals_ID) select obj).ToList<DynamicScreen>();

            IList<string> Control_Types = new List<string>{"CustomDateTimePicker","TextBox","NumericUpDown","CheckBox","StatusLabel","ComboBox",
                                        "RadioButton","MaskTextBox"};

            foreach (DynamicScreen obj in dynamicScreenlst)
            {
                if (Control_Types.Contains(obj.Control_Type))//BugID:46764
                {
                    switch (obj.Control_Name)
                    {
                        case "INR": vitalslist.Add("PT/INR"); break;
                        case "Holter/Event": vitalslist.Add("HolterEvent"); break;
                        case "BP-Sitting Left": vitalslist.Add("BP-SittingLocation"); break;
                        case "BP-Standing Left": vitalslist.Add("BP-StandingLocation"); break;
                        case "BP-Lying Left": vitalslist.Add("BP-LyingLocation"); break;
                        //Jira Cap-448 - when added in vitals along with the Lt BP did not gets displayed in Summary bar
                        case "BP-Sitting$ Left": vitalslist.Add("BP-Sitting$Location"); break;
                        case "BP-Standing$ Left": vitalslist.Add("BP-Standing$Location"); break;
                        case "BP-Lying$ Left": vitalslist.Add("BP-Lying$Location"); break;
                        case "BP-Sitting$ Right": vitalslist.Add("BP-Sitting$Location"); break;
                        case "BP-Standing$ Right": vitalslist.Add("BP-Standing$Location"); break;
                        case "BP-Lying$ Right": vitalslist.Add("BP-Lying$Location"); break;
                        default:
                            {
                                string Vital_value = Regex.Replace(obj.Control_Name, @"\s", "");//BugID:46764
                                vitalslist.Add(Vital_value);
                                break;
                            }
                    }
                }
            }
            IList<string> LoincOBV = new List<string>();
            /*
            string strXmlFilePathVitals = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\Dynamic_Screen.xml");
            
            if (File.Exists(strXmlFilePathVitals) == true)
            {
                XDocument doc = XDocument.Load(strXmlFilePathVitals);
                if (doc.Root.Name.ToString() == "DynamicScreenList")
                {
                    var elements = doc.Root.Elements().ToList();
                    doc.Root.Elements().Remove();                         // Remove the elements from the document.
                    doc.Root.Add(elements.OrderBy(x => int.Parse(x.Attribute("Sort_Order").Value)));
                    var sortedElements = doc.Root.Elements().ToList();
                    foreach (XElement xe in sortedElements)
                    {
                        if (xe.Attribute("Control_Type").Value == "CustomDateTimePicker" || xe.Attribute("Control_Type").Value == "TextBox" || xe.Attribute("Control_Type").Value == "NumericUpDown" || xe.Attribute("Control_Type").Value == "CheckBox" || xe.Attribute("Control_Type").Value == "StatusLabel" || xe.Attribute("Control_Type").Value == "ComboBox" || xe.Attribute("Control_Type").Value == "RadioButton" || xe.Attribute("Control_Type").Value == "MaskTextBox")//BugID:46764
                        {
                            switch (xe.Attribute("Control_Name").Value)
                            {
                                case "INR": vitalslist.Add("PT/INR"); break;
                                case "Holter/Event": vitalslist.Add("HolterEvent"); break;
                                case "BP-Sitting Left": vitalslist.Add("BP-SittingLocation"); break;
                                case "BP-Standing Left": vitalslist.Add("BP-StandingLocation"); break;
                                case "BP-Lying Left": vitalslist.Add("BP-LyingLocation"); break;
                                default:
                                    {
                                        string Vital_value = Regex.Replace(xe.Attribute("Control_Name").Value, @"\s", "");//BugID:46764
                                        vitalslist.Add(Vital_value);
                                        break;
                                    }
                            }
                        }
                    }
                }

            }
            */
            for (int j = 0; j < vitalslist.Count; j++)
            {
                for (int k = 0; k < ilstVitalsList.Count; k++)
                {
                    string loinc_observation = Regex.Replace(ilstVitalsList[k].Loinc_Observation, @"\s", "");//BugID:46764
                    if (loinc_observation.Replace("Second", "$").Replace("CDP", "").ToUpper() == vitalslist[j].ToUpper())
                    {
                        if (LoincOBV.IndexOf(ilstVitalsList[k].Loinc_Observation) == -1)
                        {
                            LoincOBV.Add(ilstVitalsList[k].Loinc_Observation);
                            OrderedVitalList.Add(ilstVitalsList[k]);
                        }
                    }
                }
            }
            if (OrderedVitalList != null && OrderedVitalList.Count > 0)
                MaxCapturedDate = OrderedVitalList.Max(a => a.Captured_date_and_time);

            if (OrderedVitalList != null)
            {
                for (int i = 0; i < OrderedVitalList.Count; i++)
                {
                    if (OrderedVitalList[i].Loinc_Observation.Contains("Second"))
                    {
                        OrderedVitalList[i].Loinc_Observation = OrderedVitalList[i].Loinc_Observation.Remove(OrderedVitalList[i].Loinc_Observation.IndexOf("Second"), 6);
                    }

                    if (OrderedVitalList[i].Loinc_Observation.Contains("$"))
                    {
                        OrderedVitalList[i].Loinc_Observation = OrderedVitalList[i].Loinc_Observation.Remove(OrderedVitalList[i].Loinc_Observation.IndexOf("$"), 1);
                    }

                    if (i != OrderedVitalList.Count - 1)
                    {
                        if (OrderedVitalList[i + 1].Loinc_Observation.Contains("Second"))
                        {
                            OrderedVitalList[i + 1].Loinc_Observation =
                                OrderedVitalList[i + 1].Loinc_Observation.Remove(OrderedVitalList[i + 1].Loinc_Observation.IndexOf("Second"), 6);
                        }
                        if (OrderedVitalList[i + 1].Loinc_Observation.Contains("$"))
                        {
                            OrderedVitalList[i + 1].Loinc_Observation =
                                OrderedVitalList[i + 1].Loinc_Observation.Remove(OrderedVitalList[i + 1].Loinc_Observation.IndexOf("$"), 1);
                        }
                    }
                    if (i != OrderedVitalList.Count - 2 && i != OrderedVitalList.Count - 1)
                    {
                        if (OrderedVitalList[i + 2].Loinc_Observation.Contains("Second"))
                        {
                            OrderedVitalList[i + 2].Loinc_Observation =
                                OrderedVitalList[i + 2].Loinc_Observation.Remove(OrderedVitalList[i + 2].Loinc_Observation.IndexOf("Second"), 6);
                        }
                        if (OrderedVitalList[i + 2].Loinc_Observation.Contains("$"))
                        {
                            OrderedVitalList[i + 2].Loinc_Observation =
                                OrderedVitalList[i + 2].Loinc_Observation.Remove(OrderedVitalList[i + 2].Loinc_Observation.IndexOf("$"), 1);
                        }
                    }
                    if (OrderedVitalList[i].Value != string.Empty && OrderedVitalList[i].Value != " ")
                    {
                        if (OrderedVitalList[i].Loinc_Observation == "Height" && OrderedVitalList[i].Value != string.Empty)
                        {
                            if (OrderedVitalList[i].Value.Contains("'") == false)
                            {
                                string sValue = ConversionOnRetrieval(OrderedVitalList[i].Loinc_Observation, OrderedVitalList[i].Value);
                                OrderedVitalList[i].Value = sValue;
                            }
                            else
                            {
                                OrderedVitalList[i].Value = OrderedVitalList[i].Value;
                            }
                        }

                        IList<string> sList = (from h in iFieldLookupList
                                               where h.Value.ToUpper() ==

                                                   OrderedVitalList[i].Loinc_Observation.ToUpper()
                                               select h.Description).ToList<string>();

                        string sColor = Color.Black.Name;

                        if (sList.Count != 0)
                            sColor = ValidateVitalUnits(OrderedVitalList[i].Loinc_Observation, OrderedVitalList[i].Value, sList[0]);

                        if (OrderedVitalList[i].Loinc_Observation == "Height" && OrderedVitalList[i].Value != string.Empty)
                        {
                            string[] d = OrderedVitalList[i].Value.Split('\'');
                            Text = OrderedVitalList[i].Loinc_Observation + " - " + d[0] + " Feet" + d[1] + " Inches" + "\n";
                        }
                        else
                        {
                            if (i != OrderedVitalList.Count - 1)
                            {
                                if (OrderedVitalList[i].Loinc_Observation == OrderedVitalList[i + 1].Loinc_Observation)
                                {
                                    if (OrderedVitalList[i].Captured_date_and_time == OrderedVitalList[i + 1].Captured_date_and_time)
                                    {
                                        if (OrderedVitalList[i + 1].Value != string.Empty)
                                        {
                                            Text = OrderedVitalList[i].Loinc_Observation + " - " + OrderedVitalList[i].Value + AddVitalUnits(OrderedVitalList[i].Loinc_Observation) + " ," + OrderedVitalList[i + 1].Value + AddVitalUnits(OrderedVitalList[i + 1].Loinc_Observation) + "\n";
                                        }
                                        else
                                        {
                                            Text = OrderedVitalList[i].Loinc_Observation + " - " + OrderedVitalList[i].Value + AddVitalUnits(OrderedVitalList[i].Loinc_Observation);
                                        }
                                    }
                                    else if ((OrderedVitalList[i].Captured_date_and_time > OrderedVitalList[i + 1].Captured_date_and_time))
                                    {
                                        Text = OrderedVitalList[i].Loinc_Observation + " - " + OrderedVitalList[i].Value + AddVitalUnits(OrderedVitalList[i].Loinc_Observation) + "\n";
                                    }
                                    else if ((OrderedVitalList[i].Captured_date_and_time < OrderedVitalList[i + 1].Captured_date_and_time))
                                    {
                                        if (OrderedVitalList[i + 1].Value != string.Empty)
                                        {
                                            Text = OrderedVitalList[i].Loinc_Observation + " - " + OrderedVitalList[i + 1].Value + AddVitalUnits(OrderedVitalList[i + 1].Loinc_Observation) + "\n";
                                        }
                                    }
                                }
                                if (i != OrderedVitalList.Count - 2)
                                {
                                    if (OrderedVitalList[i].Loinc_Observation == OrderedVitalList[i + 2].Loinc_Observation)
                                    {
                                        if (OrderedVitalList[i].Captured_date_and_time == OrderedVitalList[i + 2].Captured_date_and_time)
                                        {
                                            if (OrderedVitalList[i + 2].Value != string.Empty)
                                            {
                                                Text = OrderedVitalList[i].Loinc_Observation + " - " + OrderedVitalList[i].Value + AddVitalUnits(OrderedVitalList[i].Loinc_Observation) + " ," + OrderedVitalList[i + 2].Value + AddVitalUnits(OrderedVitalList[i + 2].Loinc_Observation) + "\n";
                                            }
                                            else
                                            {
                                                Text = OrderedVitalList[i].Loinc_Observation + " - " + OrderedVitalList[i].Value + AddVitalUnits(OrderedVitalList[i].Loinc_Observation);
                                            }
                                        }
                                        else if ((OrderedVitalList[i].Captured_date_and_time > OrderedVitalList[i + 2].Captured_date_and_time))
                                        {
                                            Text = OrderedVitalList[i].Loinc_Observation + " - " + OrderedVitalList[i].Value + AddVitalUnits(OrderedVitalList[i].Loinc_Observation) + "\n";
                                        }
                                        else if ((OrderedVitalList[i].Captured_date_and_time < OrderedVitalList[i + 2].Captured_date_and_time))
                                        {
                                            if (OrderedVitalList[i + 2].Value != string.Empty)
                                            {
                                                Text = OrderedVitalList[i].Loinc_Observation + " - " + OrderedVitalList[i + 2].Value + AddVitalUnits(OrderedVitalList[i + 2].Loinc_Observation) + "\n";
                                            }
                                        }
                                    }
                                }
                            }
                            //Jira Cap-448 - when added in vitals along with the Lt BP did not gets displayed in Summary bar
                            // if (!sVitalsText.Contains(OrderedVitalList[i].Loinc_Observation) && !Text.Contains(OrderedVitalList[i].Loinc_Observation))
                            if (!Text.Contains(OrderedVitalList[i].Loinc_Observation))
                            {
                                Text = OrderedVitalList[i].Loinc_Observation + " - " + OrderedVitalList[i].Value + AddVitalUnits(OrderedVitalList[i].Loinc_Observation) + "\n";
                            }
                        }

                        if (Text != string.Empty)
                        {
                            sb_Vitals.Append("<span class=\"BlockObjects\" style=");

                            sb_Vitals.Append(sColor == Color.Black.Name ?
                                "\"color:Black;\">" : "\"color:Red;\">");

                            sb_VitalsToolTip.Append(Text.Trim().Replace("CDP", ""));//BugID:46764 -- Removed CDP from Loinc_Observation Text . CDP-Abbrv for Custom Date Time Picker
                            sb_VitalsToolTip.Append("<br/>");

                            sb_Vitals.Append(Text.Trim().Replace("CDP", ""));
                            sb_Vitals.Append("</span><br/>");
                            //SCS change For Bug Id:60984
                            if (Text.Contains("SCS - Yes") == true)
                            {
                                string sTemp = sb_VitalsToolTip.ToString().Replace("SCS - Yes", "").Replace("Vitals :<br/>", "Vitals :<br/>SCS - Yes<br/>");
                                sb_VitalsToolTip = new StringBuilder();
                                sb_VitalsToolTip.Append(sTemp);

                                string sTemp1 = sb_Vitals.ToString().Replace("<span class=\"BlockObjects\" style=\"color:Black;\">SCS - Yes</span><br/>", "").Replace("<span class=\"BlockObjects\" style=\"color:Black;\">Vitals :</span>", "Vitals :<br/><span class=\"BlockObjects\" style=\"color:Red;\">SCS - Yes</span>");
                                sb_Vitals = new StringBuilder();
                                sb_Vitals.Append(sTemp1);
                            }
                            sVitalsText += Text;
                            Text = string.Empty;
                        }
                    }
                }
            }

            toolTipText = sb_VitalsToolTip.ToString();
            return sb_Vitals.ToString();
        }


        public string GetChiefComplaintsInfo(IList<ChiefComplaints> ilstChiefComplaintsList, out string toolTipText)
        {
            StringBuilder sb_ChiefComplaints = new StringBuilder("Chief Complaints :<br/>");

            if (ilstChiefComplaintsList != null)
            {
                sb_ChiefComplaints.Append(string.Join("<br/>",
                    ilstChiefComplaintsList.Select(a => a.HPI_Value).Distinct().ToArray()));
            }

            sb_ChiefComplaints.Append("<br/>");
            toolTipText = sb_ChiefComplaints.ToString();
            return sb_ChiefComplaints.ToString();
        }

        public string GetAllergyInfo(IList<NonDrugAllergy> NonDrugAllergyList, IList<Rcopia_Allergy> DrugAllergyList, out string toolTipText)
        {
            //Cap - 2552
            NonDrugAllergyList = NonDrugAllergyList.OrderBy(a => a.Non_Drug_Allergy_History_Info).ThenBy(a => a.Description).ToList();
            DrugAllergyList = DrugAllergyList.OrderBy(a => a.Allergy_Name).ToList();

            StringBuilder sbAllergy = new StringBuilder("Allergies :<br/>Drug Allergy:<br/>");
            StringBuilder sbNDA = new StringBuilder("<br/>Non Drug Allergy:<br/>");
            string d = string.Empty;
            if (DrugAllergyList != null && DrugAllergyList.Count > 0)
            {
                for (int i = 0; i < DrugAllergyList.Count; i++)
                {
                    if (DrugAllergyList.Count > 1 && DrugAllergyList[i].Allergy_Name != "nkda")
                    {
                        //if (!(d.Contains(DrugAllergyList[i].Allergy_Name)))//For Bug ID : 72263 
                        //{
                        d += DrugAllergyList[i].Allergy_Name;
                        sbAllergy.Append(DrugAllergyList[i].Allergy_Name);
                        if (DrugAllergyList[i].Reaction != string.Empty)
                            sbAllergy.Append(" - ");
                        sbAllergy.Append(DrugAllergyList[i].Reaction);
                        sbAllergy.Append("<br/>");
                        //}
                    }
                    else if (DrugAllergyList.Count == 1)
                    {
                        sbAllergy.Append(DrugAllergyList[i].Allergy_Name);
                        if (DrugAllergyList[i].Reaction != string.Empty)
                            sbAllergy.Append(" - ");
                        sbAllergy.Append(DrugAllergyList[i].Reaction);
                        sbAllergy.Append("<br/>");
                    }

                }
                //sbAllergy.Append("<br/>");
            }
            else
            {
                sbAllergy = new StringBuilder("Allergies :<br/>");
            }
            string s = string.Empty;
            if (NonDrugAllergyList != null && NonDrugAllergyList.Count > 0)
            {
                for (int i = 0; i < NonDrugAllergyList.Count; i++)
                {
                    if (!(s.Contains(NonDrugAllergyList[i].Non_Drug_Allergy_History_Info)))
                    {
                        s += NonDrugAllergyList[i].Non_Drug_Allergy_History_Info;
                        sbNDA.Append(NonDrugAllergyList[i].Non_Drug_Allergy_History_Info);
                        if (NonDrugAllergyList[i].Description != string.Empty)
                            sbNDA.Append(" - ");
                        sbNDA.Append(NonDrugAllergyList[i].Description);
                        sbNDA.Append("<br/>");
                    }
                }
            }
            else
            {
                sbNDA = new StringBuilder("");
            }

            toolTipText = sbAllergy.Append(sbNDA.ToString()).ToString();
            return toolTipText;
        }

        public string GetProblemList(IList<ProblemList> ilstProblemList, out string toolTipText)
        {
            StringBuilder sb_ProblemList = new StringBuilder();

            StringBuilder sb_ProblemListToolTip = new StringBuilder();

            sb_ProblemList.Append("<span id=\"ctl00_C5POBody_lblProblemList :\" class=\"BlockObjects\" style=\"color:Black;\">Problem List :</span><br/>");
            sb_ProblemListToolTip.Append("Problem List :<br/>");

            IList<ProblemList> ilstProblemListActive = new List<ProblemList>();
            var act = ilstProblemList.Where(a => a.Status == "Active" && a.Date_Diagnosed != string.Empty && a.Is_Active == "Y")
                .OrderByDescending(a => a.Date_Diagnosed).ThenBy(a => a.Created_Date_And_Time).ToList()
                .Concat(ilstProblemList.Where(a => a.Status == "Active" && a.Date_Diagnosed == string.Empty && a.Is_Active == "Y")
                .OrderByDescending(a => a.Created_Date_And_Time).ToList());

            ilstProblemListActive = act.ToList<ProblemList>();

            IList<ProblemList> ilstProblemListInActive = new List<ProblemList>();
            var inact = ilstProblemList.Where(b => b.Status.ToUpper() == "INACTIVE" && b.Date_Diagnosed != string.Empty && b.Is_Active == "Y")
                .OrderByDescending(b => b.Date_Diagnosed).ThenBy(b => b.Created_Date_And_Time)
                .ToList().Concat(ilstProblemList.Where(b => b.Status.ToUpper() == "INACTIVE" && b.Date_Diagnosed == string.Empty && b.Is_Active == "Y")
                .OrderByDescending(b => b.Created_Date_And_Time).ToList());

            ilstProblemListInActive = inact.ToList<ProblemList>();
            var sprblmlst = string.Empty;

            if (ilstProblemList != null && ilstProblemList.Count > 0)
            {
                if (ilstProblemListActive.Count > 0 && ilstProblemListActive != null)
                {
                    for (int h = 0; h < ilstProblemListActive.Count; h++)
                    {
                        var strProblemList = string.Empty;
                        string sColor = Color.Black.Name;
                        string[] DateDiagnosed = null;
                        string diagnosed = string.Empty;
                        if (ilstProblemListActive[h].Date_Diagnosed != string.Empty)
                        {
                            DateDiagnosed = ilstProblemListActive[h].Date_Diagnosed.Split('-');

                            if (DateDiagnosed.Length == 2 || DateDiagnosed.Length == 1)
                            {
                                if (DateDiagnosed.Length == 1)
                                {
                                    diagnosed = " (" + ilstProblemListActive[h].Date_Diagnosed + ")";
                                }
                                else
                                {
                                    ilstProblemListActive[h].Date_Diagnosed = DateDiagnosed[1] + "-" + DateDiagnosed[0] + "-01";
                                    string smonth = Convert.ToDateTime(ilstProblemListActive[h].Date_Diagnosed).ToString("dd-MMM-yyyy");
                                    string[] sarrMonth = smonth.Split('-');


                                    diagnosed = " (" + sarrMonth[1] + "-" + DateDiagnosed[1] + ")";
                                }
                            }
                            else
                            {
                                ilstProblemListActive[h].Date_Diagnosed = Convert.ToDateTime(ilstProblemListActive[h].Date_Diagnosed).ToString("dd-MMM-yyyy");
                                diagnosed = " (" + ilstProblemListActive[h].Date_Diagnosed + ")";
                            }
                        }
                        if (ilstProblemListActive[h].Problem_Description != string.Empty && ilstProblemListActive[h].Problem_Description.ToUpper() != "OTHER1" && ilstProblemListActive[h].Problem_Description.ToUpper() != "OTHER2" && ilstProblemListActive[h].Problem_Description.ToUpper() != "OTHER3" && ilstProblemListActive[h].Problem_Description.ToUpper() != "OTHER4" && ilstProblemListActive[h].Problem_Description.ToUpper() != "OTHER5")
                        {
                            if (ilstProblemListActive[h].Status == "Active")
                            {
                                strProblemList += ilstProblemListActive[h].Problem_Description + diagnosed + "\n";

                                sColor = Color.Black.Name;

                            }
                        }
                        //if (ilstProblemListActive[h].ICD_Code != string.Empty)
                        //{

                        //    if (ilstProblemListActive[h].Status == "Active")
                        //    {
                        //        strProblemList += ilstProblemListActive[h].ICD_Code + "-" + ilstProblemListActive[h].Problem_Description + diagnosed + "\n";
                        //        sColor = Color.Black.Name;
                        //    }

                        //}
                        //else if (ilstProblemListActive[h].ICD_Code == string.Empty && ilstProblemListActive[h].Problem_Description != string.Empty)
                        //{
                        //    if (ilstProblemListActive[h].Status == "Active")
                        //    {
                        //        strProblemList += ilstProblemListActive[h].Problem_Description + diagnosed + "\n";
                        //        sColor = Color.Black.Name;

                        //    }
                        //}
                        if (strProblemList != string.Empty && !(sprblmlst.Contains(ilstProblemListActive[h].Problem_Description)))
                        {
                            sprblmlst += strProblemList;
                            sb_ProblemList.Append("<span class=\"BlockObjects\" style=");

                            sb_ProblemList.Append(sColor == Color.Black.Name ?
                                    "\"color:Black;\">" : "\"color:Red;\">");

                            sb_ProblemListToolTip.Append(strProblemList.Trim());
                            sb_ProblemListToolTip.Append("<br/>");

                            sb_ProblemList.Append(strProblemList.Trim());
                            sb_ProblemList.Append("</span><br/>");
                        }

                    }
                    sb_ProblemList.Append("<br/>");
                }

            }
            var sprblmlstinactve = string.Empty;

            if (ilstProblemList != null && ilstProblemList.Count > 0)
            {
                if (ilstProblemListInActive.Count > 0 && ilstProblemListInActive != null)
                {
                    sb_ProblemList.Append("<span class=\"BlockObjects\" style=\"color:Black;\">Inactive ProblemList :</span><br/>");

                    sb_ProblemListToolTip.Append("Inactive ProblemList :<br/>");

                    for (int h = 0; h < ilstProblemListInActive.Count; h++)
                    {
                        var strProblemList = string.Empty;
                        string sColor = Color.Black.Name;
                        string[] DateDiagnosed = null;
                        string diagnosed = string.Empty;
                        if (ilstProblemListInActive[h].Date_Diagnosed != string.Empty)
                        {
                            DateDiagnosed = ilstProblemListInActive[h].Date_Diagnosed.Split('-');

                            if (DateDiagnosed.Length == 2 || DateDiagnosed.Length == 1)
                            {
                                if (DateDiagnosed.Length == 1)
                                {
                                    diagnosed = " (" + ilstProblemListInActive[h].Date_Diagnosed + ")";
                                }
                                else
                                {
                                    ilstProblemListInActive[h].Date_Diagnosed = DateDiagnosed[1] + "-" + DateDiagnosed[0] + "-01";
                                    string smonth = Convert.ToDateTime(ilstProblemListInActive[h].Date_Diagnosed).ToString("dd-MMM-yyyy");
                                    string[] sarrMonth = smonth.Split('-');
                                    //CAP-1662
                                    diagnosed = " (" + sarrMonth[1] + "-" + DateDiagnosed[1] + ")";
                                }
                            }
                            else
                            {
                                ilstProblemListInActive[h].Date_Diagnosed = Convert.ToDateTime(ilstProblemListInActive[h].Date_Diagnosed).ToString("dd-MMM-yyyy");
                                diagnosed = " (" + ilstProblemListInActive[h].Date_Diagnosed + ")";
                            }
                        }
                        if (ilstProblemListInActive[h].Problem_Description != string.Empty && ilstProblemListInActive[h].Problem_Description.ToUpper() != "OTHER1" && ilstProblemListInActive[h].Problem_Description.ToUpper() != "OTHER2" && ilstProblemListInActive[h].Problem_Description.ToUpper() != "OTHER3" && ilstProblemListInActive[h].Problem_Description.ToUpper() != "OTHER4" && ilstProblemListInActive[h].Problem_Description.ToUpper() != "OTHER5")
                        {
                            if (ilstProblemListInActive[h].Status == "Inactive")
                            {
                                strProblemList += ilstProblemListInActive[h].Problem_Description + diagnosed + "\n";

                                sColor = Color.Black.Name;

                            }
                        }
                        //if (ilstProblemListInActive[h].ICD_Code != string.Empty)
                        //{
                        //    if (ilstProblemListInActive[h].Status == "Inactive")
                        //    {
                        //        strProblemList += ilstProblemListInActive[h].ICD_Code + "-" + ilstProblemListInActive[h].Problem_Description + diagnosed + "\n";
                        //        sColor = Color.Black.Name;
                        //    }
                        //}
                        //else if (ilstProblemListInActive[h].ICD_Code == string.Empty &&
                        //    ilstProblemListInActive[h].Problem_Description != string.Empty)
                        //{
                        //    if (ilstProblemListInActive[h].Status == "Inactive")
                        //    {
                        //        strProblemList += ilstProblemListInActive[h].Problem_Description + diagnosed + "\n";
                        //        sColor = Color.Black.Name;
                        //    }
                        //}
                        if (strProblemList != string.Empty && !(sprblmlstinactve.Contains(ilstProblemListInActive[h].Problem_Description)))
                        {
                            sprblmlstinactve += strProblemList;
                            sb_ProblemList.Append("<span class=\"BlockObjects\" style=");

                            sb_ProblemList.Append(sColor == Color.Black.Name ?
                                    "\"color:Black;\">" : "\"color:Red;\">");

                            sb_ProblemListToolTip.Append(strProblemList.Trim());
                            sb_ProblemListToolTip.Append("<br/>");

                            sb_ProblemList.Append(strProblemList.Trim());
                            sb_ProblemList.Append("</span><br/>");
                        }

                    }
                }
            }

            toolTipText = sb_ProblemListToolTip.ToString();

            return sb_ProblemList.ToString();
        }
        public string GetMedication(Acurus.Capella.Core.DTO.FillPatientSummaryBarDTO PatientSummary, out string toolTipText)
        {
            StringBuilder sb_Medication = new StringBuilder();

            StringBuilder sb_MedicationToolTip = new StringBuilder();

            IList<Rcopia_Medication> MedicationList = PatientSummary.MedicationList;

            IList<ulong> EncounterIDList = PatientSummary.EncounterIDList;

            IList<DateTime> EncounterDateList = PatientSummary.EncounterDateList;

            IList<string> MedicationHistoryList = PatientSummary.MedHistoryList;


            sb_Medication.Append("<span id=\"ctl00_C5POBody_lblMedication :\" class=\"BlockObjects\" style=\"color:Black;\">Medication :</span><br>");
            sb_MedicationToolTip.Append("Medication :<br/>");
            var strMedlst = string.Empty;

            #region Commented for Measures_Reconciliation Measure_Temp Fix

            //DateTime startdate = DateTime.MinValue;
            //DateTime stopdate = DateTime.MinValue;

            //if (MedicationList.Count != 0 && MedicationList != null)
            //{
            //    if (EncounterIDList.Count > 1 && EncounterDateList.Count > 1)
            //    {
            //        for (int i = 0; i < EncounterIDList.Count; i++)
            //        {
            //            for (int j = 0; j < EncounterDateList.Count; j++)
            //            {
            //                if (EncounterIDList[i].ToString() == ClientSession.EncounterId.ToString())
            //                {
            //                    stopdate = EncounterDateList[0].Date;

            //                    if (i == 0 && j == 0)
            //                    {
            //                        startdate = EncounterDateList[j + 1].Date;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (EncounterIDList.Count != 0 && EncounterDateList.Count != 0)
            //        {
            //            if (EncounterIDList[0].ToString() == ClientSession.EncounterId.ToString())
            //            {
            //                stopdate = EncounterDateList[0].Date;
            //                startdate = DateTime.MinValue;
            //            }
            //        }
            //        else if (EncounterIDList.Count == 0 && EncounterDateList.Count == 0)
            //        {
            //            stopdate = DateTime.Now;
            //            startdate = DateTime.Now;
            //        }
            //    }
            //}

            DateTime sEncdate = DateTime.MinValue;


            if (MedicationList.Count != 0 && MedicationList != null)
            {
                if (ClientSession.EncounterId == 0)//For open patient chart
                {
                    sEncdate = DateTime.Now.Date;
                }
                if (EncounterIDList.Count > 0)
                {
                    for (int i = 0; i < EncounterIDList.Count; i++)
                    {
                        for (int j = 0; j < EncounterDateList.Count; j++)
                        {
                            if (EncounterIDList[i].ToString() == ClientSession.EncounterId.ToString())
                            {
                                sEncdate = EncounterDateList[j].Date;
                            }
                        }
                    }
                }
                else
                {
                    sEncdate = DateTime.Now.Date;
                }

            }

            //if (EncounterIDList.Count != 0 && EncounterIDList != null)
            //{
            //var strMedlst = string.Empty;
            if (MedicationList.Count != 0 && MedicationList != null)
            {
                //Jira - Cap - 2351
                bool bMedicationFilled = false;
                //CAP-2420
                MedicationList = MedicationList.OrderBy(a => a.Brand_Name).ThenBy(a => a.Generic_Name).ToList();
                for (int i = 0; i < MedicationList.Count; i++)
                {
                    string sColor = Color.Black.Name;
                    var strMedicationlist = string.Empty;
                    //BugID:52944
                    //Commented to remove stopped medications from Medications List displayed in Patient Summary Bar
                    //                    if ((MedicationList[i].Start_Date.Date >= startdate.Date || MedicationList[i].Start_Date.Date <= startdate.Date) && MedicationList[i].Stop_Date.Date < stopdate.Date && MedicationList[i].Stop_Date.Date >= startdate.Date && MedicationList[i].Stop_Date.Date != DateTime.MinValue.Date)
                    //                    {
                    //                        //RED
                    //                        if (MedicationList[i].Brand_Name != MedicationList[i].Generic_Name)
                    //                        {
                    //                            strMedicationlist = MedicationList[i].Brand_Name + " " + MedicationList

                    //[i].Generic_Name + " " + MedicationList[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList

                    //[i].Dose + " " + MedicationList[i].Dose_Unit + " " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing

                    //+ "\n";

                    //                            sColor = Color.Red.Name;
                    //                        }
                    //                        else
                    //                        {
                    //                            strMedicationlist = MedicationList[i].Generic_Name + " " + MedicationList

                    //[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList[i].Dose + " " + MedicationList[i].Dose_Unit +

                    //" " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing + "\n";

                    //                            sColor = Color.Red.Name;
                    //                        }
                    //                    }
                    //                    else
                    //                    if ((MedicationList[i].Start_Date.Date >= startdate.Date || MedicationList[i].Start_Date.Date <= startdate.Date) &&
                    //                        (MedicationList[i].Stop_Date.Date >= stopdate.Date || MedicationList[i].Stop_Date.Date == DateTime.MinValue.Date) &&
                    //                        (MedicationList[i].Start_Date.Date <= stopdate.Date))
                    //                    {
                    //                        //BLACK
                    //                        if (MedicationList[i].Brand_Name != MedicationList[i].Generic_Name)
                    //                        {
                    //                            strMedicationlist = MedicationList[i].Brand_Name + " " + MedicationList

                    //[i].Generic_Name + " " + MedicationList[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList

                    //[i].Dose + " " + MedicationList[i].Dose_Unit + " " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing

                    //+ "\n";

                    //                            sColor = Color.Black.Name;
                    //                        }
                    //                        else if (MedicationList[i].Brand_Name != "" && MedicationList[i].Generic_Name != "")
                    //                        {
                    //                            strMedicationlist = MedicationList[i].Generic_Name + " " + MedicationList

                    //[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList[i].Dose + " " + MedicationList[i].Dose_Unit +

                    //" " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing + "\n";

                    //                            sColor = Color.Black.Name;
                    //                        }
                    //                    }

                    //                    else if (MedicationList[i].Start_Date.Date <= startdate.Date && (MedicationList[i].Stop_Date.Date > stopdate.Date || MedicationList[i].Stop_Date.Date < stopdate.Date))
                    //                    {
                    //                        strMedicationlist = string.Empty;
                    //                    }
                    //For Git Lab ID 1345
                    /* if ((MedicationList[i].Start_Date.Date == DateTime.MinValue.Date && MedicationList[i].Stop_Date.Date == DateTime.MinValue.Date) ||
                         (MedicationList[i].Start_Date.Date <= sEncdate.Date && MedicationList[i].Stop_Date.Date == DateTime.MinValue.Date) ||
                         (MedicationList[i].Start_Date.Date <= sEncdate.Date && MedicationList[i].Stop_Date.Date >= sEncdate.Date) ||
                         (MedicationList[i].Start_Date.Date == DateTime.MinValue.Date && MedicationList[i].Stop_Date.Date >= sEncdate.Date))*/
                    if ((MedicationList[i].Start_Date.Date == DateTime.MinValue.Date && MedicationList[i].Stop_Date.Date == DateTime.MinValue.Date) ||
                   (MedicationList[i].Start_Date.Date <= sEncdate.Date && MedicationList[i].Stop_Date.Date == DateTime.MinValue.Date) ||
                   (MedicationList[i].Start_Date.Date <= sEncdate.Date && MedicationList[i].Stop_Date.Date > sEncdate.Date) ||
                   (MedicationList[i].Start_Date.Date == DateTime.MinValue.Date && MedicationList[i].Stop_Date.Date > sEncdate.Date))
                    {
                        //BLACK
                        if (MedicationList[i].Brand_Name != MedicationList[i].Generic_Name)
                        {
                            strMedicationlist = MedicationList[i].Brand_Name + " " + MedicationList

[i].Generic_Name + " " + MedicationList[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList

[i].Dose + " " + MedicationList[i].Dose_Unit + " " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing

+ "\n";

                            sColor = Color.Black.Name;
                        }
                        else if (MedicationList[i].Brand_Name != "" && MedicationList[i].Generic_Name != "")
                        {
                            strMedicationlist = MedicationList[i].Generic_Name + " " + MedicationList

[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList[i].Dose + " " + MedicationList[i].Dose_Unit +

" " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing + "\n";

                            sColor = Color.Black.Name;
                        }
                    }
                    //if (strMedicationlist != string.Empty  && !(strMedlst.Contains(strMedicationlist)))
                    if (strMedicationlist != string.Empty)
                    {
                        //Jira - Cap - 2351
                        bMedicationFilled = true;
                        strMedlst += strMedicationlist;
                        sb_Medication.Append("<span class=\"BlockObjects\" style=");

                        sb_Medication.Append(sColor == Color.Black.Name ?
                                "\"color:Black;\">" : "\"color:Red;\">");

                        sb_MedicationToolTip.Append(strMedicationlist.Trim());
                        sb_MedicationToolTip.Append("<br/>");

                        sb_Medication.Append(strMedicationlist.Trim());
                        sb_Medication.Append("</span><br/>");
                    }

                }
                //Jira - Cap - 2351
                if (!bMedicationFilled)
                {
                    string sColor = Color.Black.Name;
                    sb_Medication.Append("<span class=\"BlockObjects\" style=");

                    sb_Medication.Append(sColor == Color.Black.Name ?
                            "\"color:Black;\">" : "\"color:Red;\">");

                    sb_Medication.Append("No Active Medication");

                    sb_Medication.Append("</span><br/>");
                }
            }
            //Jira - Cap - 2351
            else
            {
                string sColor = Color.Black.Name;
                sb_Medication.Append("<span class=\"BlockObjects\" style=");

                sb_Medication.Append(sColor == Color.Black.Name ?
                        "\"color:Black;\">" : "\"color:Red;\">");

                sb_Medication.Append("No Active Medication");

                sb_Medication.Append("</span><br/>");


            }
            //}

            #endregion
            //}
            #region Medication_New_TempFix
            //            if (MedicationList.Count != 0 && MedicationList != null)
            //            {
            //                for (int i = 0; i < MedicationList.Count; i++)
            //                {
            //                    string sColor = Color.Black.Name;
            //                    var strMedicationlist = string.Empty;

            //                    if (MedicationList[i].Brand_Name != MedicationList[i].Generic_Name)
            //                    {
            //                        strMedicationlist = MedicationList[i].Brand_Name + " " + MedicationList

            //[i].Generic_Name + " " + MedicationList[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList

            //[i].Dose + " " + MedicationList[i].Dose_Unit + " " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing

            //+ "\n";

            //                    }
            //                    else
            //                    {
            //                        strMedicationlist = MedicationList[i].Generic_Name + " " + MedicationList

            //[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList[i].Dose + " " + MedicationList[i].Dose_Unit +

            //" " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing + "\n";


            //                    }
            //                    if (strMedicationlist != string.Empty && !(strMedlst.Contains(strMedicationlist)))
            //                    {
            //                        strMedlst += strMedicationlist;
            //                        sb_Medication.Append("<span class=\"BlockObjects\" style=");

            //                        sb_Medication.Append(sColor == Color.Black.Name ?
            //                                "\"color:Black;\">" : "\"color:Red;\">");

            //                        sb_MedicationToolTip.Append(strMedicationlist.Trim());
            //                        sb_MedicationToolTip.Append("<br/>");

            //                        sb_Medication.Append(strMedicationlist.Trim());
            //                        sb_Medication.Append("</span><br/>");
            //                    }
            //                }

            //            }
            #endregion
            toolTipText = sb_MedicationToolTip.ToString();

            return sb_Medication.ToString();
        }
        //        public string GetMedication(Acurus.Capella.Core.DTO.FillPatientSummaryBarDTO PatientSummary, out string toolTipText)
        //        {
        //            StringBuilder sb_Medication = new StringBuilder();

        //            StringBuilder sb_MedicationToolTip = new StringBuilder();

        //            IList<Rcopia_Medication> MedicationList = PatientSummary.MedicationList;

        //            IList<ulong> EncounterIDList = PatientSummary.EncounterIDList;

        //            IList<DateTime> EncounterDateList = PatientSummary.EncounterDateList;

        //            IList<string> MedicationHistoryList = PatientSummary.MedHistoryList;


        //            sb_Medication.Append("<span id=\"ctl00_C5POBody_lblMedication :\" class=\"BlockObjects\" style=\"color:Black;\">Medication :</span><br>");
        //            sb_MedicationToolTip.Append("Medication :<br/>");
        //            var strMedlst = string.Empty;

        //            #region Commented for Measures_Reconciliation Measure_Temp Fix

        //            DateTime startdate = DateTime.MinValue;
        //            DateTime stopdate = DateTime.MinValue;

        //            if (MedicationList.Count != 0 && MedicationList != null)
        //            {
        //                if (EncounterIDList.Count > 1 && EncounterDateList.Count > 1)
        //                {
        //                    for (int i = 0; i < EncounterIDList.Count; i++)
        //                    {
        //                        for (int j = 0; j < EncounterDateList.Count; j++)
        //                        {
        //                            if (EncounterIDList[i].ToString() == ClientSession.EncounterId.ToString())
        //                            {
        //                                stopdate = EncounterDateList[0].Date;

        //                                if (i == 0 && j == 0)
        //                                {
        //                                    startdate = EncounterDateList[j + 1].Date;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (EncounterIDList.Count != 0 && EncounterDateList.Count != 0)
        //                    {
        //                        if (EncounterIDList[0].ToString() == ClientSession.EncounterId.ToString())
        //                        {
        //                            stopdate = EncounterDateList[0].Date;
        //                            startdate = DateTime.MinValue;
        //                        }
        //                    }
        //                    else if (EncounterIDList.Count == 0 && EncounterDateList.Count == 0)
        //                    {
        //                        stopdate = DateTime.Now;
        //                        startdate = DateTime.Now;
        //                    }
        //                }
        //            }

        //            //if (EncounterIDList.Count != 0 && EncounterIDList != null)
        //            //{
        //            //var strMedlst = string.Empty;
        //            if (MedicationList.Count != 0 && MedicationList != null)
        //            {

        //                for (int i = 0; i < MedicationList.Count; i++)
        //                {
        //                    string sColor = Color.Black.Name;
        //                    var strMedicationlist = string.Empty;
        //                    //BugID:52944
        //                    //Commented to remove stopped medications from Medications List displayed in Patient Summary Bar
        //                    //                    if ((MedicationList[i].Start_Date.Date >= startdate.Date || MedicationList[i].Start_Date.Date <= startdate.Date) && MedicationList[i].Stop_Date.Date < stopdate.Date && MedicationList[i].Stop_Date.Date >= startdate.Date && MedicationList[i].Stop_Date.Date != DateTime.MinValue.Date)
        //                    //                    {
        //                    //                        //RED
        //                    //                        if (MedicationList[i].Brand_Name != MedicationList[i].Generic_Name)
        //                    //                        {
        //                    //                            strMedicationlist = MedicationList[i].Brand_Name + " " + MedicationList

        //                    //[i].Generic_Name + " " + MedicationList[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList

        //                    //[i].Dose + " " + MedicationList[i].Dose_Unit + " " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing

        //                    //+ "\n";

        //                    //                            sColor = Color.Red.Name;
        //                    //                        }
        //                    //                        else
        //                    //                        {
        //                    //                            strMedicationlist = MedicationList[i].Generic_Name + " " + MedicationList

        //                    //[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList[i].Dose + " " + MedicationList[i].Dose_Unit +

        //                    //" " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing + "\n";

        //                    //                            sColor = Color.Red.Name;
        //                    //                        }
        //                    //                    }
        //                    //                    else
        //                    if ((MedicationList[i].Start_Date.Date >= startdate.Date || MedicationList[i].Start_Date.Date <= startdate.Date) && (MedicationList[i].Stop_Date.Date >= stopdate.Date || MedicationList[i].Stop_Date.Date == DateTime.MinValue.Date) && (MedicationList[i].Start_Date.Date <= stopdate.Date))
        //                    {
        //                        //BLACK
        //                        if (MedicationList[i].Brand_Name != MedicationList[i].Generic_Name)
        //                        {
        //                            strMedicationlist = MedicationList[i].Brand_Name + " " + MedicationList

        //[i].Generic_Name + " " + MedicationList[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList

        //[i].Dose + " " + MedicationList[i].Dose_Unit + " " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing

        //+ "\n";

        //                            sColor = Color.Black.Name;
        //                        }
        //                        else if (MedicationList[i].Brand_Name != "" && MedicationList[i].Generic_Name != "")
        //                        {
        //                            strMedicationlist = MedicationList[i].Generic_Name + " " + MedicationList

        //[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList[i].Dose + " " + MedicationList[i].Dose_Unit +

        //" " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing + "\n";

        //                            sColor = Color.Black.Name;
        //                        }
        //                    }

        //                    else if (MedicationList[i].Start_Date.Date <= startdate.Date && (MedicationList[i].Stop_Date.Date > stopdate.Date || MedicationList[i].Stop_Date.Date < stopdate.Date))
        //                    {
        //                        strMedicationlist = string.Empty;
        //                    }

        //                    if (strMedicationlist != string.Empty && !(strMedlst.Contains(strMedicationlist)))
        //                    {
        //                        strMedlst += strMedicationlist;
        //                        sb_Medication.Append("<span class=\"BlockObjects\" style=");

        //                        sb_Medication.Append(sColor == Color.Black.Name ?
        //                                "\"color:Black;\">" : "\"color:Red;\">");

        //                        sb_MedicationToolTip.Append(strMedicationlist.Trim());
        //                        sb_MedicationToolTip.Append("<br/>");

        //                        sb_Medication.Append(strMedicationlist.Trim());
        //                        sb_Medication.Append("</span><br/>");
        //                    }
        //                }
        //            }
        //            //}

        //            #endregion
        //            //}
        //            #region Medication_New_TempFix
        //            //            if (MedicationList.Count != 0 && MedicationList != null)
        //            //            {
        //            //                for (int i = 0; i < MedicationList.Count; i++)
        //            //                {
        //            //                    string sColor = Color.Black.Name;
        //            //                    var strMedicationlist = string.Empty;

        //            //                    if (MedicationList[i].Brand_Name != MedicationList[i].Generic_Name)
        //            //                    {
        //            //                        strMedicationlist = MedicationList[i].Brand_Name + " " + MedicationList

        //            //[i].Generic_Name + " " + MedicationList[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList

        //            //[i].Dose + " " + MedicationList[i].Dose_Unit + " " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing

        //            //+ "\n";

        //            //                    }
        //            //                    else
        //            //                    {
        //            //                        strMedicationlist = MedicationList[i].Generic_Name + " " + MedicationList

        //            //[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList[i].Dose + " " + MedicationList[i].Dose_Unit +

        //            //" " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing + "\n";


        //            //                    }
        //            //                    if (strMedicationlist != string.Empty && !(strMedlst.Contains(strMedicationlist)))
        //            //                    {
        //            //                        strMedlst += strMedicationlist;
        //            //                        sb_Medication.Append("<span class=\"BlockObjects\" style=");

        //            //                        sb_Medication.Append(sColor == Color.Black.Name ?
        //            //                                "\"color:Black;\">" : "\"color:Red;\">");

        //            //                        sb_MedicationToolTip.Append(strMedicationlist.Trim());
        //            //                        sb_MedicationToolTip.Append("<br/>");

        //            //                        sb_Medication.Append(strMedicationlist.Trim());
        //            //                        sb_Medication.Append("</span><br/>");
        //            //                    }
        //            //                }

        //            //            }
        //            #endregion
        //            toolTipText = sb_MedicationToolTip.ToString();

        //            return sb_Medication.ToString();
        //        }

        //Added by srividhya for console application
        public static DateTime ConvertToLocal(DateTime inputDateTime, string sOffsetTime)
        {
            //The following is the modified code
            DateTime dt = DateTime.MinValue;
            if (sOffsetTime != "")
            {
                double offset = -(double.Parse(sOffsetTime.ToString()));
                if (ClientSession.bFollows_DST)
                {
                    DateTime DayLight_StartDateTime;
                    DateTime DayLight_EndDateTime;
                    int iDSTStartDay = UtilityManager.FindDate(inputDateTime.Year, 3, DayOfWeek.Sunday, 2);
                    int iDSTEndDay = UtilityManager.FindDate(inputDateTime.Year, 11, DayOfWeek.Sunday, 1);
                    DayLight_StartDateTime = new DateTime(inputDateTime.Year, 3, iDSTStartDay, 2, 0, 0);
                    DayLight_EndDateTime = new DateTime(inputDateTime.Year, 11, iDSTEndDay, 2, 0, 0);
                    if ((inputDateTime.Ticks >= DayLight_StartDateTime.Ticks) && (inputDateTime.Ticks < DayLight_EndDateTime.Ticks))
                        offset += 60;
                }

                if (inputDateTime.ToString("dd-MM-yyyy hh:mm:ss tt") != "01-01-0001 12:00:00 AM")
                {
                    dt = inputDateTime.AddMinutes(offset);
                }
                else
                {
                    dt = inputDateTime;
                }
            }

            return dt;
        }

        public static IList<FacilityLibrary> GetFacilityList()
        {
            IList<FacilityLibrary> AllFacilities = new List<FacilityLibrary>();
            FacilityLibrary CurrentFacility = new FacilityLibrary();
            FacilityManager facMngr = new FacilityManager();
            AllFacilities = facMngr.GetAll();

            return AllFacilities.OrderBy(item => item.Fac_Name).ToList<FacilityLibrary>();

            //XmlDocument xmldoc = new XmlDocument();
            //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\Facility_Library.xml");
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "Facility_Library" + ".xml");
            //    XmlNodeList xmlFacilityList = xmldoc.GetElementsByTagName("Facility");
            //    if (xmlFacilityList.Count > 0)
            //    {
            //        foreach (XmlNode facility_details in xmlFacilityList)
            //        {
            //            if (facility_details != null)
            //            {
            //                CurrentFacility = new FacilityLibrary();
            //                CurrentFacility.Fac_Name = facility_details.Attributes["Name"].Value.ToString();
            //                CurrentFacility.Start_Time = facility_details.Attributes["Start_Time"].Value.ToString();
            //                CurrentFacility.End_Time = facility_details.Attributes["End_Time"].Value.ToString();
            //                CurrentFacility.Slot_Length = Convert.ToInt16(facility_details.Attributes["Slot_Length"].Value.ToString());
            //                CurrentFacility.Fac_Address1 = facility_details.Attributes["Primary_IP"].Value.ToString();
            //                CurrentFacility.Fac_Address2 = facility_details.Attributes["Secondary_IP"].Value.ToString();
            //                CurrentFacility.Fac_NPI = facility_details.Attributes["Facility_NPI"].Value.ToString();
            //                CurrentFacility.Taxonomy_Code = facility_details.Attributes["Taxonomy_Code"].Value.ToString();
            //                CurrentFacility.Taxonomy_Description = facility_details.Attributes["Taxonomy_Description"].Value.ToString();
            //                AllFacilities.Add(CurrentFacility);
            //            }
            //        }
            //    }
            //}
            //return AllFacilities.OrderBy(item => item.Fac_Name).ToList<FacilityLibrary>();
        }

        public static IList<PhysicianLibrary> GetPhysicianList(string facility_name, string sLegalOrg)
        {
            IList<PhysicianLibrary> AllPhysicians = new List<PhysicianLibrary>();

            PhysicianLibrary CurrentPhysician = new PhysicianLibrary();

            XmlDocument xmldoc = new XmlDocument();
            //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\
            //PhysicianFacilityMapping.xml");
            //CAP-2781
            PhysicianFacilityMappingList physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");
            if (physicianFacilityMappingList != null)
            {
                //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianFacilityMapping" + ".xml");
                if (facility_name.Trim() == string.Empty)
                {
                    //XmlNodeList xmlPhysicianList = xmldoc.SelectNodes("/ROOT/PhyList/Facility/Physician");
                    var physicianList = physicianFacilityMappingList.PhysicianFacility.SelectMany(x => x.Physician).ToList();
                    IList<PhysicianLibrary> lstMachineTech = new List<PhysicianLibrary>();
                    if (physicianList.Count > 0)
                    {
                        //CAP-1819
                        //Jira CAP-2777
                        //XmlDocument xmldocTech = new XmlDocument();
                        //string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
                        //if (File.Exists(strXmlFilePathTech))
                        //{
                        //    xmldocTech.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                        //}
                        //Jira CAP-2777
                        MachinetechnicianList machinetechnicianList = new MachinetechnicianList();
                        machinetechnicianList = ConfigureBase<MachinetechnicianList>.ReadJson("machine_technician.json");
                        List<Machinetechnician> machinetechnicians = new List<Machinetechnician>();
                        foreach (var Physician_details in physicianList)
                        {
                            if (Physician_details != null && Physician_details.Legal_Org.ToString() == sLegalOrg)
                            {
                                CurrentPhysician = new PhysicianLibrary();
                                CurrentPhysician.PhyPrefix = Physician_details.prefix;
                                CurrentPhysician.PhyFirstName = Physician_details.firstname;
                                CurrentPhysician.PhyMiddleName = Physician_details.middlename;
                                CurrentPhysician.PhyLastName = Physician_details.lastname;
                                CurrentPhysician.PhySuffix = Physician_details.suffix;
                                CurrentPhysician.PhyId = Convert.ToUInt32(Physician_details.ID);
                                CurrentPhysician.Id = Convert.ToUInt32(Physician_details.ID);
                                CurrentPhysician.Is_Active = Physician_details.status;
                                CurrentPhysician.PhyUserName = Physician_details.username;
                                CurrentPhysician.PhyColor = Physician_details.machine_technician_id;//assinged tech ID
                                //CAP-1819
                                //Jira CAP-2777
                                //XmlNode nodeMatchingFacility = xmldocTech.SelectSingleNode("/MachineTechnician/MachineTechnician" + CurrentPhysician.PhyColor);
                                //if (nodeMatchingFacility != null)
                                //{
                                //    CurrentPhysician.Company = nodeMatchingFacility.Attributes["machine_name"].Value.ToString();
                                //}
                                //else
                                //{
                                //    CurrentPhysician.Company = "";
                                //}

                                //Jira CAP-2777
                                machinetechnicians = new List<Machinetechnician>();
                                if (machinetechnicianList?.MachineTechnician != null)
                                {
                                    machinetechnicians = machinetechnicianList.MachineTechnician.Where(x => x.machine_technician_library_id == CurrentPhysician.PhyColor.ToString()).ToList();
                                }
                                if (machinetechnicians.Count > 0)
                                {
                                    CurrentPhysician.Company = machinetechnicians[0].machine_name.ToString();
                                }
                                else
                                {
                                    CurrentPhysician.Company = "";
                                }
                                if (CurrentPhysician.PhyColor != "0")
                                    lstMachineTech.Add(CurrentPhysician);
                                else
                                    AllPhysicians.Add(CurrentPhysician);
                            }
                        }
                    }
                    //CAP-1819
                    AllPhysicians = AllPhysicians.GroupBy(a => new { a.Id, a.Company }).Select(a => a.FirstOrDefault()).ToList<PhysicianLibrary>();
                    AllPhysicians = AllPhysicians.Concat(lstMachineTech).GroupBy(a => new { a.Id, a.Company }).Select(a => a.FirstOrDefault()).ToList<PhysicianLibrary>();//BugID:53256
                    return AllPhysicians.OrderBy(item => item.PhyLastName).ToList<PhysicianLibrary>();
                    //return AllPhysicians.Distinct().OrderBy(item => item.PhyFirstName).ToList<PhysicianLibrary>();
                }
                else
                {
                    //XmlNode nodeMatchingFacility = xmldoc.SelectSingleNode("/ROOT/PhyList/Facility[@name='" + facility_name.Trim() + "']");
                    PhysicianFacility phyFac = physicianFacilityMappingList.PhysicianFacility.Where(x => x.name == facility_name.Trim()).FirstOrDefault();
                    string sDefaultPhysicians = "";
                    //if (nodeMatchingFacility != null)
                    //{
                    //    sDefaultPhysicians = nodeMatchingFacility.Attributes["default-physician-id"].Value.ToString();
                    //}
                    if (phyFac != null)
                    {
                        sDefaultPhysicians = phyFac.defaultphysicianid.ToString();
                    }
                    string[] lstDefaultPhysicians = sDefaultPhysicians.Split(',');
                    //XmlNodeList xmlPhysicianList = nodeMatchingFacility != null ? nodeMatchingFacility.ChildNodes : (null);
                    //if (xmlPhysicianList != null && xmlPhysicianList.Count > 0)
                    if (phyFac?.Physician != null && phyFac.Physician.Count > 0)
                    { 
                        //foreach (XmlNode Physician_details in xmlPhysicianList)
                            foreach (var Physician_details in phyFac.Physician)
                            {
                            if (Physician_details != null)
                            {
                                CurrentPhysician = new PhysicianLibrary();
                                //CurrentPhysician.PhyPrefix = Physician_details.Attribut[es"prefix"].Value.ToString();
                                //CurrentPhysician.PhyFirstName = Physician_details.Attributes["firstname"].Value.ToString();
                                //CurrentPhysician.PhyMiddleName = Physician_details.Attributes["middlename"].Value.ToString();
                                //CurrentPhysician.PhyLastName = Physician_details.Attributes["lastname"].Value.ToString();
                                //CurrentPhysician.PhySuffix = Physician_details.Attributes["suffix"].Value.ToString();
                                //CurrentPhysician.PhyId = Convert.ToUInt32(Physician_details.Attributes["ID"].Value.ToString());
                                //CurrentPhysician.Id = Convert.ToUInt32(Physician_details.Attributes["ID"].Value.ToString());
                                //CurrentPhysician.Is_Active = Physician_details.Attributes["status"].Value.ToString();
                                //CurrentPhysician.PhyUserName = Physician_details.Attributes["username"].Value.ToString();
                                //CurrentPhysician.PhyColor = Physician_details.Attributes["machine_technician_id"].Value.ToString();//assinged tech ID

                                CurrentPhysician.PhyPrefix = Physician_details.prefix.ToString();
                                CurrentPhysician.PhyFirstName = Physician_details.firstname.ToString();
                                CurrentPhysician.PhyMiddleName = Physician_details.middlename.ToString();
                                CurrentPhysician.PhyLastName = Physician_details.lastname.ToString();
                                CurrentPhysician.PhySuffix = Physician_details.suffix.ToString();
                                CurrentPhysician.PhyId = Convert.ToUInt32(Physician_details.ID.ToString());
                                CurrentPhysician.Id = Convert.ToUInt32(Physician_details.ID.ToString());
                                CurrentPhysician.Is_Active = Physician_details.status.ToString();
                                CurrentPhysician.PhyUserName = Physician_details.username.ToString();
                                CurrentPhysician.PhyColor = Physician_details.machine_technician_id.ToString();

                                if (lstDefaultPhysicians.Count() > 0 && lstDefaultPhysicians[0].Trim() != string.Empty)
                                {
                                    if (CurrentPhysician.PhyId == Convert.ToUInt64(lstDefaultPhysicians[0]))
                                    {
                                        CurrentPhysician.PhyNotes = "Default";
                                    }
                                }
                                AllPhysicians.Add(CurrentPhysician);
                            }
                        }
                    }
                    return AllPhysicians.OrderBy(item => item.PhyLastName).ToList<PhysicianLibrary>();
                }
            }
            return AllPhysicians.Distinct().OrderBy(item => item.PhyLastName).ToList<PhysicianLibrary>();
        }

        public static IList<MapFacilityPhysician> GetFacilityListMappedToPhysician(string physician_id)
        {
            IList<MapFacilityPhysician> MappedFacilitiesForPhysician = new List<MapFacilityPhysician>();
            if (physician_id.Trim() != "")
            {
                MapFacilityPhysician CurrentFacilityForPhysician = new MapFacilityPhysician();

                XmlDocument xmldoc = new XmlDocument();
                //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianFacilityMapping.xml");
                //if (File.Exists(strXmlFilePath) == true)
                //CAP-2781
                PhysicianFacilityMappingList physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");
                if (physicianFacilityMappingList != null)
                {
                    //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianFacilityMapping" + ".xml");

                    //XmlNodeList nodeMatchingPhysicians = xmldoc.SelectNodes("/ROOT/PhyList/Facility/Physician[@ID='" + physician_id.Trim() + "']");
                    var facilityList = physicianFacilityMappingList.PhysicianFacility;
                    if (facilityList != null && facilityList.Count > 0)
                    {
                        foreach (var facility in facilityList)
                        {
                            foreach (var Physician_details in facility.Physician.Where(x => x.ID == physician_id.Trim()))
                            {
                                if (Physician_details != null)
                                {
                                    CurrentFacilityForPhysician = new MapFacilityPhysician();
                                    CurrentFacilityForPhysician.Facility_Name = facility.name;
                                    CurrentFacilityForPhysician.Phy_Rec_ID = Convert.ToUInt32(Physician_details.ID);
                                    string[] default_physician_ids = facility.defaultphysicianid.ToString().Split(',');
                                    CurrentFacilityForPhysician.Sort_Order = default_physician_ids.Any(item => item == Physician_details.ID) ? Convert.ToUInt32(1) : Convert.ToUInt32(0);
                                    MappedFacilitiesForPhysician.Add(CurrentFacilityForPhysician);
                                }
                            }
                        }
                    }

                    if (facilityList == null || facilityList.SelectMany(x => x.Physician.Where(y => y.ID == physician_id.Trim())).ToList().Count == 0)
                    {
                        //CAP-2781
                        //XmlNodeList nodeMatchingTechnicians = xmldoc.SelectNodes("/ROOT/PhyList/Facility/Physician[@machine_technician_id='" + physician_id.Trim() + "']");
                        var matchingTechnicians = physicianFacilityMappingList.PhysicianFacility.SelectMany(x => x.Physician).Where(x => x.machine_technician_id == physician_id.Trim()).ToList();
                        if (matchingTechnicians != null && matchingTechnicians.Count > 0)
                        {
                            foreach (var facility in facilityList)
                            {
                                foreach (var Technician_details in facility.Physician.Where(x => x.machine_technician_id == physician_id.Trim()))
                                {
                                    if (Technician_details != null)
                                    {
                                        CurrentFacilityForPhysician = new MapFacilityPhysician();
                                        CurrentFacilityForPhysician.Facility_Name = facility.name;
                                        CurrentFacilityForPhysician.Phy_Rec_ID = Convert.ToUInt32(Technician_details.ID);
                                        string[] default_physician_ids = facility.defaultphysicianid.ToString().Split(',');
                                        CurrentFacilityForPhysician.Sort_Order = default_physician_ids.Any(item => item == Technician_details.ID) ? Convert.ToUInt32(1) : Convert.ToUInt32(0);
                                        MappedFacilitiesForPhysician.Add(CurrentFacilityForPhysician);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return MappedFacilitiesForPhysician;
        }

        public static void CreateUserSessionFile(string sUserName, string sSessionID)
        {
            try
            {
                if (sUserName == string.Empty)
                {
                    UtilityManager.inserttologgingtableforSessionTimeout("Skipping theCreateUserSessionFile", string.Empty, string.Empty);
                    return;
                }

                UtilityManager.inserttologgingtableforSessionTimeout("Creating User Session File - " + sUserName + "_" + sSessionID, string.Empty, string.Empty);
                int trycount = 0;
            tryagain:
                try
                {
                    if (!Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]))
                        Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]);

                    File.Create(Path.Combine(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"], sUserName + "_" + sSessionID + ".txt")).Dispose();

                    UserManager userMngr = new UserManager();
                    userMngr.SaveLastSuccessfulyLoginDate(sUserName, ConvertToUniversal());
                }
                catch (Exception xmlexcep)
                {
                    trycount++;
                    if (trycount <= 3)
                    {
                        Thread.Sleep(1000);
                        goto tryagain;
                    }
                    else
                    {
                        //CAP-1942
                        throw new Exception(xmlexcep.Message, xmlexcep);
                    }
                }
            }
            catch (Exception ex)
            {
                //CAP-1942
                if (ex.InnerException != null && ex.InnerException.Message != null)
                    throw new Exception("The Network Path is not reachable. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString() + "." + Environment.NewLine + ex.InnerException.Message + ex + ".");
                else
                    throw new Exception("The Network Path is not reachable. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString() + "." + Environment.NewLine + ex.Message + ex + ".");

                //Ping pinger = null;
                //pinger = new Ping();
                //PingReply reply = null;
                //try
                //{
                //    reply = pinger.Send(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]);
                //}
                //catch (Exception exnew)
                //{
                //    if (exnew.InnerException != null && exnew.InnerException.Message != null)
                //        throw new Exception("The Network Path is not reachable. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString() + "." + Environment.NewLine + exnew.InnerException.Message + ".");
                //    else
                //        throw new Exception("The Network Path is not reachable. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString() + "." + Environment.NewLine + exnew.Message + ".");
                //}
                //if (reply.Status != IPStatus.Success)
                //{
                //    throw new Exception("The Ping test for the Network Path is Failed. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString());
                //}
                //else
                //{
                //    throw new Exception(ex.Message + "." + Environment.NewLine + "The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString());
                //}
            }
        }

        public static void DeleteUserSessionFile(string sUserName, string sSessionID)
        {
            try
            {
                sUserName = (sUserName != string.Empty) ? sUserName : "*";
                sSessionID = (sSessionID != string.Empty) ? sSessionID : "*";

                UtilityManager.inserttologgingtableforSessionTimeout("Deleting User Session File - " + sUserName + "_" + sSessionID, string.Empty, string.Empty);

                if (sUserName == "*" && sSessionID == "*")
                {
                    UtilityManager.inserttologgingtableforSessionTimeout("SkippingDeleteUserSessionFile as both both parameters are * - " + sUserName + "_" + sSessionID, string.Empty, string.Empty);
                    return;
                }
                int trycount = 0;
            tryagain:
                try
                {
                    if (!Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]))
                        Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]);

                    DirectoryInfo dirSession = new DirectoryInfo(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]);
                    FileInfo[] filesInDir = dirSession.GetFiles(sUserName + "_" + sSessionID + ".txt");

                    if (filesInDir.Count() > 0)
                        foreach (FileInfo userFile in filesInDir)
                            userFile.Delete();
                }
                catch (Exception xmlexcep)
                {
                    trycount++;
                    if (trycount <= 3)
                    {
                        Thread.Sleep(1000);
                        goto tryagain;
                    }
                    else
                    {
                        //CAP-1942
                        throw new Exception(xmlexcep.Message, xmlexcep);
                    }
                }
            }
            catch (Exception ex)
            {
                //CAP-1942
                if (ex.InnerException != null && ex.InnerException.Message != null)
                    throw new Exception("The Network Path is not reachable. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString() + "." + Environment.NewLine + ex.InnerException.Message + ex + ".");
                else
                    throw new Exception("The Network Path is not reachable. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString() + "." + Environment.NewLine + ex.Message + ex + ".");


                //Ping pinger = null;
                //pinger = new Ping();
                //PingReply reply = null;
                //try
                //{
                //    reply = pinger.Send(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]);
                //}
                //catch (Exception exnew)
                //{
                //    if (exnew.InnerException != null && exnew.InnerException.Message != null)
                //        throw new Exception("The Network Path is not reachable. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString() + "." + Environment.NewLine + exnew.InnerException.Message + ".");
                //    else
                //        throw new Exception("The Network Path is not reachable. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString() + "." + Environment.NewLine + exnew.Message + ".");
                //}
                //if (reply.Status != IPStatus.Success)
                //{
                //    throw new Exception("The Ping test for the Network Path is Failed. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString());
                //}
                //else
                //{
                //    throw new Exception(ex.Message + "." + Environment.NewLine + "The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString());
                //}
            }
        }

        //public static bool FindCurrentUserSessionFile(string sUserName, string sSessionID)
        //{
        //    bool bFound = false;

        //    if (!Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]))
        //        Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]);

        //    DirectoryInfo dirSession = new DirectoryInfo(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]);
        //    FileInfo[] filesInDir = dirSession.GetFiles(sUserName + "_" + sSessionID + ".txt");

        //    if (filesInDir.Count() > 0)
        //        bFound = true;

        //    return bFound;
        //}

        public static IList<string> FindUserSessionFiles(string sUserName, string sSessionID)
        {
            IList<string> lstFiles = new List<string>();

            sUserName = (sUserName != string.Empty) ? sUserName : "*";
            sSessionID = (sSessionID != string.Empty) ? sSessionID : "*";
            try
            {
                int trycount = 0;
            tryagain:
                try
                {
                    if (!Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]))
                        Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]);

                    DirectoryInfo dirSession = new DirectoryInfo(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]);
                    FileInfo[] filesInDir = dirSession.GetFiles(sUserName + "_" + sSessionID + ".txt");

                    if (filesInDir.Count() > 0)
                    {
                        foreach (FileInfo file in filesInDir)
                        {
                            //Jira CAP-2174
                            //var checkUsernameCriteria = (file.Name??string.Empty).Split('_');

                            //if(checkUsernameCriteria.Count() > 2 && !sUserName.Contains("_"))
                            if (file.Name.Substring(0, file.Name.LastIndexOf("_")) != sUserName)
                            {
                                continue;
                            }

                            lstFiles.Add(file.Name);
                        }
                    }
                }
                catch (Exception xmlexcep)
                {
                    trycount++;
                    if (trycount <= 3)
                    {
                        Thread.Sleep(1000);
                        goto tryagain;
                    }
                    else
                    {
                        //CAP-1942
                        throw new Exception(xmlexcep.Message, xmlexcep);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                    throw new Exception("The Network Path is not reachable. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString() + "." + Environment.NewLine + ex.InnerException.Message + ".");
                else
                    throw new Exception("The Network Path is not reachable. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString() + "." + Environment.NewLine + ex.Message + ".");

                //Ping pinger = null;
                //pinger = new Ping();
                //PingReply reply = null;
                //try
                //{
                //    reply = pinger.Send(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"]);
                //}
                //catch (Exception exnew)
                //{
                //    if (exnew.InnerException != null && exnew.InnerException.Message != null)
                //        throw new Exception("The Network Path is not reachable. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString() + "." + Environment.NewLine + exnew.InnerException.Message + ".");
                //    else
                //        throw new Exception("The Network Path is not reachable. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString() + "." + Environment.NewLine + exnew.Message + ".");
                //}
                //if (reply.Status != IPStatus.Success)
                //{
                //    throw new Exception("The Ping test for the Network Path is Failed. Please contact System Administrator. The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString());
                //}
                //else
                //{
                //    throw new Exception(ex.Message + "." + Environment.NewLine + "The Network Path is: " + System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"].ToString());
                //}
            }
            return lstFiles;
        }
        //Added by Saravanan
        public string GetSnomedfromStaticLookup(string sReasonOrFollowup, string sFieldName, string sEnteredItem)
        {
            string sSnomed = string.Empty;

            IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
            IList<string> ilstItem = sEnteredItem.Split(',').Select(i => i.TrimStart().ToString()).ToList<string>();

            #region XML code
            //    string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
            //    if (File.Exists(strXmlFilePath) == true)
            //    {
            //        XDocument xmlSnomed = XDocument.Load(strXmlFilePath);
            //        IEnumerable<XElement> xmlSnomedCode = null;
            //        try
            //        {
            //            if (ilstItem.Count > 0)
            //            {
            //                for (int i = 0; i < ilstItem.Count; i++)
            //                {
            //                    //                     xmlSnomedCode = xmlSnomed.Element("root").Element(sReasonOrFollowup)
            //                    //.Elements(sReasonOrFollowup.Replace("List"," ").Trim()).Where(xx => xx.Attribute("Field_Name").Value == sFieldName.ToUpper() && xx.Attribute("Value").Value.ToUpper() == ilstItem[i].ToString().ToUpper());
            //                    xmlSnomedCode = xmlSnomed.Element("root").Element(sReasonOrFollowup)
            //.Elements(sReasonOrFollowup.Replace("List", " ").Trim()).Where(xx => xx.Attribute("Field_Name").Value.ToUpper().Contains(sFieldName.ToUpper()) && xx.Attribute("Value").Value.ToUpper() == ilstItem[i].ToString().ToUpper());
            //                    if (xmlSnomedCode != null && xmlSnomedCode.Count() > 0)
            //                    {
            //                        if (sSnomed == string.Empty)
            //                            sSnomed = xmlSnomedCode.Attributes("Description").First().Value.ToString();
            //                        else
            //                            sSnomed += "," + xmlSnomedCode.Attributes("Description").First().Value.ToString();
            //                    }
            //                }
            //            }
            //        }
            //        catch (Exception)
            //        {
            //            sSnomed = string.Empty;
            //        }
            //    }
            #endregion

            //CAP-2787
            StaticLookupList staticLookupList = ConfigureBase<StaticLookupList>.ReadJson("staticlookup.json");
            if (staticLookupList != null)
            {
                try
                {
                    if (ilstItem.Count > 0)
                    {

                        for (int i = 0; i < ilstItem.Count; i++)
                        {
                            //CAP-2869
                            switch (sReasonOrFollowup){
                                case "MammogramTypeList":
                                    var MammogramType = staticLookupList.MammogramTypeList.FirstOrDefault(item => item.Field_Name.ToUpper().Contains(sFieldName.ToUpper()) && item.value.ToUpper() == ilstItem[i].ToString().ToUpper());

                                    if (MammogramType != null)
                                    {
                                        if (string.IsNullOrEmpty(sSnomed))
                                        {
                                            sSnomed = $"{MammogramType.Description}";
                                        }
                                        else
                                        {
                                            sSnomed += $",{MammogramType.Description}";
                                        }
                                    }

                                    break;
                                case "FoodAllergySnomedList":
                                    var FoodAllergySnomed = staticLookupList.FoodAllergySnomedList.FirstOrDefault(item => item.Field_Name.ToUpper().Contains(sFieldName.ToUpper()) && item.value.ToUpper() == ilstItem[i].ToString().ToUpper());

                                    if (FoodAllergySnomed != null)
                                    {
                                        if (string.IsNullOrEmpty(sSnomed))
                                        {
                                            sSnomed = $"{FoodAllergySnomed.Description}";
                                        }
                                        else
                                        {
                                            sSnomed += $",{FoodAllergySnomed.Description}";
                                        }
                                    }
                                    break;
                                case "ReasonNotPerformedList":
                                    var ReasonNotPerformed = staticLookupList.ReasonNotPerformedList.FirstOrDefault(item => item.Field_Name.ToUpper().Contains(sFieldName.ToUpper()) && item.value.ToUpper() == ilstItem[i].ToString().ToUpper());

                                    if (ReasonNotPerformed != null)
                                    {
                                        if (string.IsNullOrEmpty(sSnomed))
                                        {
                                            sSnomed = $"{ReasonNotPerformed.Description}";
                                        }
                                        else
                                        {
                                            sSnomed += $",{ReasonNotPerformed.Description}";
                                        }
                                    }
                                    break;
                                case "FollowupList":
                                    var Followup = staticLookupList.FollowupList.FirstOrDefault(item => item.Field_Name.ToUpper().Contains(sFieldName.ToUpper()) && item.value.ToUpper() == ilstItem[i].ToString().ToUpper());

                                    if (Followup != null)
                                    {
                                        if (string.IsNullOrEmpty(sSnomed))
                                        {
                                            sSnomed = $"{Followup.Description}";
                                        }
                                        else
                                        {
                                            sSnomed += $",{Followup.Description}";
                                        }
                                    }
                                    break;
                                case "FollowupReasonnotperformedList":
                                    var FollowupReasonnotperformed = staticLookupList.FollowupReasonnotperformedList.FirstOrDefault(item => item.Field_Name.ToUpper().Contains(sFieldName.ToUpper()) && item.Value.ToUpper() == ilstItem[i].ToString().ToUpper());

                                    if (FollowupReasonnotperformed != null)
                                    {
                                        if (string.IsNullOrEmpty(sSnomed))
                                        {
                                            sSnomed = $"{FollowupReasonnotperformed.Description}";
                                        }
                                        else
                                        {
                                            sSnomed += $",{FollowupReasonnotperformed.Description}";
                                        }
                                    }
                                    break;


                            }
                        }
                    }
                }
                catch (Exception)
                {
                    sSnomed = string.Empty;
                }
            }
            return sSnomed;
        }


        public string GetSnomedfromStaticLookupvitals(string sReasonOrFollowup, string sFieldName, string sEnteredItem)
        {
            string sSnomed = string.Empty;

            IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
            IList<string> ilstItem = sEnteredItem.Split(',').Select(i => i.TrimStart().ToString()).ToList<string>();


            //    string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
            //    if (File.Exists(strXmlFilePath) == true)
            //    {
            //        XDocument xmlSnomed = XDocument.Load(strXmlFilePath);
            //        IEnumerable<XElement> xmlSnomedCode = null;
            //        try
            //        {
            //            if (ilstItem.Count > 0)
            //            {
            //                for (int i = 0; i < ilstItem.Count; i++)
            //                {
            //                    //                     xmlSnomedCode = xmlSnomed.Element("root").Element(sReasonOrFollowup)
            //                    //.Elements(sReasonOrFollowup.Replace("List"," ").Trim()).Where(xx => xx.Attribute("Field_Name").Value == sFieldName.ToUpper() && xx.Attribute("Value").Value.ToUpper() == ilstItem[i].ToString().ToUpper());
            //                    xmlSnomedCode = xmlSnomed.Element("root").Element(sReasonOrFollowup)
            //.Elements(sReasonOrFollowup.Replace("List", " ").Trim()).Where(xx => xx.Attribute("Field_Name").Value.ToUpper().Contains(sFieldName.ToUpper()) && ilstItem[i].ToString().ToUpper().Contains(xx.Attribute("Value").Value.ToUpper()));
            //                    if (xmlSnomedCode != null && xmlSnomedCode.Count() > 0)
            //                    {
            //                        if (sSnomed == string.Empty)
            //                            sSnomed = xmlSnomedCode.Attributes("Description").First().Value.ToString();
            //                        else
            //                            sSnomed += "," + xmlSnomedCode.Attributes("Description").First().Value.ToString();
            //                    }
            //                }
            //            }
            //        }

            //        catch (Exception)
            //        {
            //            sSnomed = string.Empty;
            //        }
            // }
            //CAP-2787
            StaticLookupList staticLookupList = ConfigureBase<StaticLookupList>.ReadJson("staticlookup.json");
            if (staticLookupList != null)
            {
                try
                {
                    if (ilstItem.Count > 0)
                    {
                        for (int i = 0; i < ilstItem.Count; i++)
                        {
                            var matchingItems = staticLookupList.ReasonNotPerformedList
                            .Where(item => item.Field_Name.ToUpper().Contains(sFieldName.ToUpper()) && item.value.ToUpper() == ilstItem[i].ToString().ToUpper())
                            .ToList();

                            foreach (var match in matchingItems)
                            {
                                if (string.IsNullOrEmpty(sSnomed))
                                {
                                    sSnomed = $"{match.Description}";
                                }
                                else
                                {
                                    sSnomed += $",{match.Description}";
                                }
                            }

                        }
                    }
                }
                catch (Exception)
                {
                    sSnomed = string.Empty;
                }
            }
            return sSnomed;
        }

        public string GetFieldNameForSnomedCodefromStaticLookup(string sReasonOrFollowup, string sSnomedCode)
        {
            string sValue = string.Empty;

            IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
            IList<string> ilstItem = sSnomedCode.Split(',').Select(i => i.TrimStart().ToString()).ToList<string>();


            //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XDocument xmlSnomed = XDocument.Load(strXmlFilePath);
            //    IEnumerable<XElement> xmlSnomedCode = null;
            //    try
            //    {
            //        if (ilstItem.Count > 0)
            //        {
            //            for (int i = 0; i < ilstItem.Count; i++)
            //            {
            //                xmlSnomedCode = xmlSnomed.Element("root").Element(sReasonOrFollowup).Elements(sReasonOrFollowup.Replace("List", " ").Trim()).Where(xx => xx.Attribute("Description").Value.ToUpper() == ilstItem[i].ToString().ToUpper());
            //                if (xmlSnomedCode != null && xmlSnomedCode.Count() > 0)
            //                {
            //                    if (sValue == string.Empty)
            //                        sValue = xmlSnomedCode.Attributes("Description").First().Value.ToString() + "~" + xmlSnomedCode.Attributes("Value").First().Value.ToString();
            //                    else
            //                        sValue += "," + xmlSnomedCode.Attributes("Description").First().Value.ToString() + "~" + xmlSnomedCode.Attributes("Value").First().Value.ToString();
            //                }
            //            }
            //        }
            //    }

            //    catch (Exception)
            //    {
            //        sValue = string.Empty;
            //    }
            //}
            //CAP-2787
            StaticLookupList staticLookupList = ConfigureBase<StaticLookupList>.ReadJson("staticlookup.json");
            if (staticLookupList != null)
            {
                try
                {
                    if (ilstItem.Count > 0)
                    {
                        for (int i = 0; i < ilstItem.Count; i++)
                        {
                            var matchingItems = staticLookupList.FollowupList
                            .Where(item => item.Description.ToUpper() == ilstItem[i].ToString().ToUpper())
                            .ToList();

                            foreach (var match in matchingItems)
                            {
                                if (string.IsNullOrEmpty(sValue))
                                {
                                    sValue = $"{match.Description}~{match.value}";
                                }
                                else
                                {
                                    sValue += $",{match.Description}~{match.value}";
                                }
                            }

                        }
                    }
                }
                catch (Exception)
                {
                    sValue = string.Empty;
                }
            }
            return sValue;
        }


        public string GetSnomedForCPT(string sCPT)
        {

            string sSnomedCode = string.Empty;
            //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument xDoc = new XmlDocument();
            //    xDoc.Load(strXmlFilePath);
            //    XmlNodeList node = xDoc.GetElementsByTagName("procedurecodelist");
            //    foreach (XmlNode xNode in node[0].ChildNodes)
            //    {
            //        if (xNode.Attributes.GetNamedItem("code").Value == sCPT)
            //        {
            //            sSnomedCode = xNode.Attributes.GetNamedItem("snomedcode").Value;
            //            break;
            //        }
            //    }
            //}
            //CAP-2787
            StaticLookupList staticLookupList = ConfigureBase<StaticLookupList>.ReadJson("staticlookup.json");
            var procedurecodelist = staticLookupList.procedurecodelist.ToList();
            if (procedurecodelist != null)
            {
                foreach (var procedurecode in procedurecodelist)
                {
                    if (procedurecode != null && procedurecode.Code == sCPT)
                    {
                        sSnomedCode = procedurecode.snomedcode;
                        break;
                    }
                }
            }
            return sSnomedCode;

        }


        public string GetSnomedForCPTCATIMeasure(string sCPT)
        {

            string sSnomedCode = string.Empty;
            //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument xDoc = new XmlDocument();
            //    xDoc.Load(strXmlFilePath);
            //    XmlNodeList node = xDoc.GetElementsByTagName("cptsnomedList");
            //    foreach (XmlNode xNode in node[0].ChildNodes)
            //    {
            //        if (xNode.Attributes.GetNamedItem("cpt").Value == sCPT)
            //        {
            //            sSnomedCode = xNode.Attributes.GetNamedItem("snomedcode").Value;
            //            break;
            //        }
            //    }
            //}
            //CAP-2787
            StaticLookupList staticLookupList = ConfigureBase<StaticLookupList>.ReadJson("staticlookup.json");
            var procedurecodelist = staticLookupList.cptsnomedList.ToList();
            if (procedurecodelist != null)
            {
                Dictionary<string, string> dictBulkExport = new Dictionary<string, string>();
                foreach (var procedurecode in procedurecodelist)
                {
                    if (procedurecode.cpt == sCPT)
                    {
                        sSnomedCode = procedurecode.snomedcode;
                        break;
                    }
                }
            }
            return sSnomedCode;
        }


        class FileErrorList
        {
            public string FileName { get; set; }
            public int TotalErrorCount { get; set; }
            public int Missing_FN { get; set; }
            public int Missing_LN { get; set; }
            public int Invalid_CC { get; set; }
            public int Invalid_AC { get; set; }
            public int Invalid_RC { get; set; }
            public int Invalid_EGC { get; set; }
            public int Invalid_LC { get; set; }
            public int Invalid_Code { get; set; }
            public int Invalid_Unit { get; set; }
            public int Invalid_ValueCode { get; set; }
            public int Invalid_RouteCode { get; set; }
            public int Missing_EntrySection { get; set; }
            public int Missing_SectionTemplate { get; set; }
        }
        static IList<string> InValidCodeList = new List<string>();
        static IList<string> InValidValueCodeList = new List<string>();
        static IList<string> InValidRouteCodeList = new List<string>();
        static IList<string> InValidUnitList = new List<string>();
        static IList<string> ValidConfidentialityCodeList = new List<string>();
        static IList<string> ValidLanguageCodeList = new List<string>();
        static IList<string> ValidAdministrativeGenderCodeList = new List<string>();
        static IList<string> ValidRaceCodeList = new List<string>();
        static IList<string> ValidEthnicGroupCodeList = new List<string>();

        public bool ValidateCCD(string path, out string ErrorDetail)
        {
            ErrorDetail = string.Empty;
            FillLookupList();
            FileErrorList file_Error = new FileErrorList();
            file_Error.FileName = path;
            file_Error.TotalErrorCount = 0;
            file_Error.Missing_FN = 0;
            file_Error.Missing_LN = 0;
            file_Error.Invalid_CC = 0;
            file_Error.Invalid_AC = 0;
            file_Error.Invalid_RC = 0;
            file_Error.Invalid_EGC = 0;
            file_Error.Invalid_LC = 0;
            file_Error.Invalid_Code = 0;
            file_Error.Invalid_Unit = 0;
            file_Error.Invalid_ValueCode = 0;
            file_Error.Invalid_RouteCode = 0;
            file_Error.Missing_EntrySection = 0;
            file_Error.Missing_SectionTemplate = 0;


            XmlDocument xDoc = new XmlDocument();
            XmlTextReader XmlText = new XmlTextReader(path);
            xDoc.Load(XmlText);
            XmlText.Close();


            //Missing LastName
            XmlNodeList LN_NodeList = xDoc.GetElementsByTagName("family");
            foreach (XmlNode xnode in LN_NodeList)
            {
                if (xnode.InnerText.Trim() == string.Empty)
                    file_Error.Missing_LN++;
            }
            XmlNodeList Suffix_NodeList = xDoc.GetElementsByTagName("suffix");
            foreach (XmlNode xnode in Suffix_NodeList)
            {
                if (xnode.InnerText.Trim() == string.Empty)
                    file_Error.Missing_LN++;
            }
            //Missing FirstName
            XmlNodeList FN_NodeList = xDoc.GetElementsByTagName("given");
            foreach (XmlNode xnode in FN_NodeList)
            {
                if (xnode.InnerText.Trim() == string.Empty)
                    file_Error.Missing_FN++;
            }
            //Invalid ConfidentialityCode
            XmlNodeList confidentialityCode_NodeList = xDoc.GetElementsByTagName("confidentialityCode");
            foreach (XmlNode xnode in confidentialityCode_NodeList)
            {
                if (xnode.Attributes.GetNamedItem("code") != null && ValidConfidentialityCodeList.IndexOf(xnode.Attributes.GetNamedItem("code").Value) == -1)
                    file_Error.Invalid_CC++;
            }
            //Invalid LanguageCode
            XmlNodeList LanguageCode_NodeList = xDoc.GetElementsByTagName("languageCode");
            foreach (XmlNode xnode in LanguageCode_NodeList)
            {
                if (xnode.Attributes.GetNamedItem("code") != null && ValidLanguageCodeList.IndexOf(xnode.Attributes.GetNamedItem("code").Value) == -1)
                    file_Error.Invalid_LC++;
            }
            //Invalid AdministrativeGenderCode
            XmlNodeList GenderCode_NodeList = xDoc.GetElementsByTagName("administrativeGenderCode");
            foreach (XmlNode xnode in GenderCode_NodeList)
            {
                if (xnode.Attributes.GetNamedItem("code") != null && ValidAdministrativeGenderCodeList.IndexOf(xnode.Attributes.GetNamedItem("code").Value) == -1)
                    file_Error.Invalid_AC++;
            }
            //Invalid RaceCode
            XmlNodeList RaceCode_NodeList = xDoc.GetElementsByTagName("raceCode");
            foreach (XmlNode xnode in RaceCode_NodeList)
            {
                if (xnode.Attributes.GetNamedItem("code") != null && ValidRaceCodeList.IndexOf(xnode.Attributes.GetNamedItem("code").Value) == -1)
                    file_Error.Invalid_RC++;
            }
            //Invalid EthnicGroupCode
            XmlNodeList EthnicGroupCode_NodeList = xDoc.GetElementsByTagName("ethnicGroupCode");
            foreach (XmlNode xnode in EthnicGroupCode_NodeList)
            {
                if (xnode.Attributes.GetNamedItem("code") != null && ValidEthnicGroupCodeList.IndexOf(xnode.Attributes.GetNamedItem("code").Value) == -1)
                    file_Error.Invalid_EGC++;
            }
            //Invalid Code
            XmlNodeList Code_NodeList = xDoc.GetElementsByTagName("code");
            foreach (XmlNode xnode in Code_NodeList)
            {
                if (xnode.Attributes.GetNamedItem("code") != null && InValidCodeList.IndexOf(xnode.Attributes.GetNamedItem("code").Value) != -1)
                    file_Error.Invalid_Code++;
            }
            //Invalid Value Code & Unit
            XmlNodeList ValueCode_NodeList = xDoc.GetElementsByTagName("value");
            foreach (XmlNode xnode in ValueCode_NodeList)
            {
                if (xnode.Attributes.GetNamedItem("code") != null && InValidValueCodeList.IndexOf(xnode.Attributes.GetNamedItem("code").Value) != -1)
                    file_Error.Invalid_ValueCode++;
                if (xnode.Attributes.GetNamedItem("unit") != null && InValidUnitList.IndexOf(xnode.Attributes.GetNamedItem("unit").Value) != -1)
                    file_Error.Invalid_Unit++;
            }
            //Invalid Value Unit
            XmlNodeList RouteCode_NodeList = xDoc.GetElementsByTagName("routeCode");
            foreach (XmlNode xnode in RouteCode_NodeList)
            {
                if (xnode.Attributes.GetNamedItem("code") != null && InValidRouteCodeList.IndexOf(xnode.Attributes.GetNamedItem("code").Value) != -1)
                    file_Error.Invalid_RouteCode++;
            }
            //Missing Entry Section, TemplateID Section -- Component
            IList<string> ExcludeEntrySec = new List<string> { "HOSPITAL DISCHARGE INSTRUCTIONS" };
            XmlNodeList Section_NodeList = xDoc.GetElementsByTagName("section");
            foreach (XmlNode xnode in Section_NodeList)
            {
                XmlDocument xdocChild = new XmlDocument();
                xdocChild.InnerXml = xnode.ParentNode.InnerXml;
                bool is_CodeNode = false;
                int count_template = 0;
                //bool is_mentalStatus = false;
                XmlNodeList xCodeNode = xdocChild.GetElementsByTagName("code");
                XmlNodeList xTemplateNode = xdocChild.GetElementsByTagName("templateId");
                XmlNodeList xEntryNode = xdocChild.GetElementsByTagName("entry");
                XmlNodeList xTextNode = xdocChild.GetElementsByTagName("text");
                foreach (XmlNode xchNode in xCodeNode)
                {
                    if (xchNode.ParentNode.Name == "section" && xchNode.ParentNode.Attributes.GetNamedItem("nullFlavor") == null)
                    {
                        if (xchNode.Attributes.GetNamedItem("displayName") == null || ((xchNode.Attributes.GetNamedItem("displayName") != null && ExcludeEntrySec.IndexOf(xchNode.Attributes.GetNamedItem("displayName").Value) == -1)))
                        {
                            if (xTextNode != null && xTextNode[0].ChildNodes.Count > 1)
                                is_CodeNode = true;
                        }


                    }
                }
                if (is_CodeNode)
                {
                    if ((xEntryNode == null) || xEntryNode != null && xEntryNode.Count == 0)
                        file_Error.Missing_EntrySection++;
                    foreach (XmlNode xtNode in xTemplateNode)
                    {
                        if (xtNode.ParentNode.Name == "section")
                        {
                            count_template++;
                        }
                    }

                    if (count_template < 1)
                        file_Error.Missing_SectionTemplate++;

                }



            }
            XmlNodeList Observation_NodeList = xDoc.GetElementsByTagName("observation");
            foreach (XmlNode xnode in Observation_NodeList)
            {
                XmlDocument xdocChild = new XmlDocument();
                xdocChild.InnerXml = xnode.ParentNode.InnerXml;
                int count_template = 0;
                bool is_CodeNode = false;
                XmlNodeList xTemplateNode = xdocChild.GetElementsByTagName("templateId");
                XmlNodeList xCodeNode = xdocChild.GetElementsByTagName("code");
                XmlNodeList xIdNode = xdocChild.GetElementsByTagName("id");
                if (xIdNode != null && xIdNode.Count > 0)
                {
                    foreach (XmlNode xchNode in xCodeNode)
                    {
                        if (xchNode.ParentNode.Name == "observation")
                        {
                            if (xchNode.Attributes.GetNamedItem("nullFlavor") != null)
                            {
                                if (xchNode.Attributes.GetNamedItem("code") != null && xchNode.Attributes.GetNamedItem("code").Value != "PATOBJ")
                                    is_CodeNode = true;
                            }


                        }
                    }
                    if (is_CodeNode)
                    {
                        foreach (XmlNode xtNode in xTemplateNode)
                        {
                            if (xtNode.ParentNode.Name == "observation")
                            {
                                count_template++;
                            }
                        }
                        if (count_template < 1)
                            file_Error.Missing_SectionTemplate++;
                    }
                }

            }
            XmlNodeList act_NodeList = xDoc.GetElementsByTagName("act");
            foreach (XmlNode xnode in act_NodeList)
            {
                XmlDocument xdocChild = new XmlDocument();
                xdocChild.InnerXml = xnode.ParentNode.InnerXml;
                int count_template = 0;
                XmlNodeList xTemplateNode = xdocChild.GetElementsByTagName("templateId");
                XmlNodeList xIdNode = xdocChild.GetElementsByTagName("id");
                if (xIdNode != null && xIdNode.Count > 0)
                {
                    foreach (XmlNode xtNode in xTemplateNode)
                    {
                        if (xtNode.ParentNode.Name == "act")
                        {
                            count_template++;
                        }
                    }
                    if (count_template < 1)
                        file_Error.Missing_SectionTemplate++;
                }

            }
            XmlNodeList encounter_NodeList = xDoc.GetElementsByTagName("encounter");
            foreach (XmlNode xnode in encounter_NodeList)
            {
                XmlDocument xdocChild = new XmlDocument();
                xdocChild.InnerXml = xnode.ParentNode.InnerXml;
                int count_template = 0;
                XmlNodeList xTemplateNode = xdocChild.GetElementsByTagName("templateId");
                XmlNodeList xIdNode = xdocChild.GetElementsByTagName("id");
                if (xIdNode != null && xIdNode.Count > 0)
                {
                    foreach (XmlNode xtNode in xTemplateNode)
                    {
                        if (xtNode.ParentNode.Name == "encounter")
                        {
                            count_template++;
                        }
                    }
                    if (count_template < 1)
                        file_Error.Missing_SectionTemplate++;
                }
            }
            XmlNodeList procedure_NodeList = xDoc.GetElementsByTagName("procedure");
            foreach (XmlNode xnode in procedure_NodeList)
            {
                XmlDocument xdocChild = new XmlDocument();
                xdocChild.InnerXml = xnode.ParentNode.InnerXml;
                int count_template = 0;
                XmlNodeList xTemplateNode = xdocChild.GetElementsByTagName("templateId");
                XmlNodeList xIdNode = xdocChild.GetElementsByTagName("id");
                if (xIdNode != null && xIdNode.Count > 0)
                {
                    foreach (XmlNode xtNode in xTemplateNode)
                    {
                        if (xtNode.ParentNode.Name == "procedure")
                        {
                            count_template++;
                        }
                    }
                    if (count_template < 1)
                        file_Error.Missing_SectionTemplate++;
                }
            }
            XmlNodeList substanceAdministration_NodeList = xDoc.GetElementsByTagName("substanceAdministration");
            foreach (XmlNode xnode in substanceAdministration_NodeList)
            {
                XmlDocument xdocChild = new XmlDocument();
                xdocChild.InnerXml = xnode.ParentNode.InnerXml;
                int count_template = 0;
                XmlNodeList xTemplateNode = xdocChild.GetElementsByTagName("templateId");
                foreach (XmlNode xtNode in xTemplateNode)
                {
                    if (xtNode.ParentNode.Name == "substanceAdministration")
                    {
                        count_template++;
                    }
                }
                if (count_template < 1)
                    file_Error.Missing_SectionTemplate++;
            }
            XmlNodeList organizer_NodeList = xDoc.GetElementsByTagName("organizer");
            foreach (XmlNode xnode in organizer_NodeList)
            {
                XmlDocument xdocChild = new XmlDocument();
                xdocChild.InnerXml = xnode.ParentNode.InnerXml;
                int count_template = 0;
                XmlNodeList xTemplateNode = xdocChild.GetElementsByTagName("templateId");
                XmlNodeList xIdNode = xdocChild.GetElementsByTagName("id");
                if (xIdNode != null && xIdNode.Count > 0)
                {
                    foreach (XmlNode xtNode in xTemplateNode)
                    {
                        if (xtNode.ParentNode.Name == "organizer")
                        {
                            count_template++;
                        }
                    }
                    if (count_template < 1)
                        file_Error.Missing_SectionTemplate++;
                }
            }
            XmlNodeList manufacturedProduct_NodeList = xDoc.GetElementsByTagName("manufacturedProduct");
            foreach (XmlNode xnode in manufacturedProduct_NodeList)
            {
                XmlDocument xdocChild = new XmlDocument();
                xdocChild.InnerXml = xnode.ParentNode.InnerXml;
                int count_template = 0;
                XmlNodeList xTemplateNode = xdocChild.GetElementsByTagName("templateId");
                XmlNodeList xIdNode = xdocChild.GetElementsByTagName("id");
                if (xIdNode != null && xIdNode.Count > 0)
                {
                    foreach (XmlNode xtNode in xTemplateNode)
                    {
                        if (xtNode.ParentNode.Name == "manufacturedProduct")
                        {
                            count_template++;
                        }
                    }
                    if (count_template < 1)
                        file_Error.Missing_SectionTemplate++;
                }
            }


            file_Error.TotalErrorCount = file_Error.Invalid_AC + file_Error.Invalid_CC + file_Error.Invalid_EGC + file_Error.Invalid_LC + file_Error.Invalid_RC +
                                         file_Error.Invalid_Code + file_Error.Invalid_RouteCode + file_Error.Invalid_Unit + file_Error.Invalid_ValueCode +
                                         file_Error.Missing_FN + file_Error.Missing_LN + file_Error.Missing_EntrySection + file_Error.Missing_SectionTemplate;


            if (file_Error.TotalErrorCount == 0)
                return true;
            else
            {
                string[] FileName = path.Split(new string[] { "\\" }, StringSplitOptions.None);
                string File_Name = FileName[FileName.Length - 1];
                ErrorDetail = "FileName: " + File_Name + "--" + "Total Error_Count: " + file_Error.TotalErrorCount + "--";
                if (file_Error.Missing_LN > 0)
                    ErrorDetail += "Missing LastName: " + file_Error.Missing_LN + "--";
                if (file_Error.Missing_FN > 0)
                    ErrorDetail += "Missing LastName: " + file_Error.Missing_FN + "--";
                if (file_Error.Invalid_AC > 0)
                    ErrorDetail += "Invalid Administrative Gender Codes: " + file_Error.Invalid_AC + "--";
                if (file_Error.Invalid_CC > 0)
                    ErrorDetail += "Invalid Confidentiality Codes: " + file_Error.Invalid_CC + "--";
                if (file_Error.Invalid_EGC > 0)
                    ErrorDetail += "Invalid Ethnic Group Codes: " + file_Error.Invalid_EGC + "--";
                if (file_Error.Invalid_LC > 0)
                    ErrorDetail += "Invalid Language Codes: " + file_Error.Invalid_LC + "--";
                if (file_Error.Invalid_RC > 0)
                    ErrorDetail += "Invalid Race Codes: " + file_Error.Invalid_RC + "--";
                int code_count = file_Error.Invalid_Code + file_Error.Invalid_RouteCode + file_Error.Invalid_ValueCode + file_Error.Invalid_Unit;
                if (code_count > 0)
                    ErrorDetail += "Invalid Codes: " + code_count + "--";
                if (file_Error.Missing_EntrySection > 0)
                    ErrorDetail += "Missing/Invalid Entry Section : " + file_Error.Missing_EntrySection + "--";
                if (file_Error.Missing_SectionTemplate > 0)
                    ErrorDetail += "Missing/Invalid Section Template: " + file_Error.Missing_SectionTemplate + "--";
                return false;
            }

        }
        private static void FillLookupList()
        {

            // Cap - 2776 - XML to JSON
            //XmlDocument xmldoc = new XmlDocument();
            //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\LookupList_CCDAErrors.xml");

            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    xmldoc.Load(strXmlFilePath);
            //    XmlNodeList xmlCodesList = xmldoc.GetElementsByTagName("CodeList");
            //    if (xmlCodesList != null && xmlCodesList.Count > 0 && xmlCodesList[0].ChildNodes != null && xmlCodesList[0].ChildNodes.Count > 0)
            //    {
            //        foreach (XmlNode CodeNode in xmlCodesList[0].ChildNodes)
            //        {
            //            InValidCodeList.Add(CodeNode.Attributes.GetNamedItem("value").Value.ToString());
            //        }
            //    }
            //    XmlNodeList xmlValueCodesList = xmldoc.GetElementsByTagName("ValueCodeList");
            //    if (xmlValueCodesList != null && xmlValueCodesList.Count > 0 && xmlValueCodesList[0].ChildNodes != null && xmlValueCodesList[0].ChildNodes.Count > 0)
            //    {
            //        foreach (XmlNode CodeNode in xmlValueCodesList[0].ChildNodes)
            //        {
            //            InValidValueCodeList.Add(CodeNode.Attributes.GetNamedItem("value").Value.ToString());
            //        }
            //    }
            //    XmlNodeList xmlRouteCodesList = xmldoc.GetElementsByTagName("RouteCodeList");
            //    if (xmlRouteCodesList != null && xmlRouteCodesList.Count > 0 && xmlRouteCodesList[0].ChildNodes != null && xmlRouteCodesList[0].ChildNodes.Count > 0)
            //    {
            //        foreach (XmlNode CodeNode in xmlRouteCodesList[0].ChildNodes)
            //        {
            //            InValidRouteCodeList.Add(CodeNode.Attributes.GetNamedItem("value").Value.ToString());
            //        }
            //    }
            //    XmlNodeList xmlUnitList = xmldoc.GetElementsByTagName("UnitList");
            //    if (xmlUnitList != null && xmlUnitList.Count > 0 && xmlUnitList[0].ChildNodes != null && xmlUnitList[0].ChildNodes.Count > 0)
            //    {
            //        foreach (XmlNode CodeNode in xmlUnitList[0].ChildNodes)
            //        {
            //            InValidUnitList.Add(CodeNode.Attributes.GetNamedItem("value").Value.ToString());
            //        }
            //    }
            //    XmlNodeList xmlConfidCodesList = xmldoc.GetElementsByTagName("ConfidentialityCodeList");
            //    if (xmlConfidCodesList != null && xmlConfidCodesList.Count > 0 && xmlConfidCodesList[0].ChildNodes != null && xmlConfidCodesList[0].ChildNodes.Count > 0)
            //    {
            //        foreach (XmlNode CodeNode in xmlConfidCodesList[0].ChildNodes)
            //        {
            //            ValidConfidentialityCodeList.Add(CodeNode.Attributes.GetNamedItem("value").Value.ToString());
            //        }
            //    }
            //    XmlNodeList xmlLangCodesList = xmldoc.GetElementsByTagName("LanguageCodeList");
            //    if (xmlLangCodesList != null && xmlLangCodesList.Count > 0 && xmlLangCodesList[0].ChildNodes != null && xmlLangCodesList[0].ChildNodes.Count > 0)
            //    {
            //        foreach (XmlNode CodeNode in xmlLangCodesList[0].ChildNodes)
            //        {
            //            ValidLanguageCodeList.Add(CodeNode.Attributes.GetNamedItem("value").Value.ToString());
            //        }
            //    }
            //    XmlNodeList xmlAdminCodesList = xmldoc.GetElementsByTagName("AdministrativeGenderCodeList");
            //    if (xmlAdminCodesList != null && xmlAdminCodesList.Count > 0 && xmlAdminCodesList[0].ChildNodes != null && xmlAdminCodesList[0].ChildNodes.Count > 0)
            //    {
            //        foreach (XmlNode CodeNode in xmlAdminCodesList[0].ChildNodes)
            //        {
            //            ValidAdministrativeGenderCodeList.Add(CodeNode.Attributes.GetNamedItem("value").Value.ToString());
            //        }
            //    }
            //    XmlNodeList xmlRaceCodesList = xmldoc.GetElementsByTagName("RaceCodeList");
            //    if (xmlRaceCodesList != null && xmlRaceCodesList.Count > 0 && xmlRaceCodesList[0].ChildNodes != null && xmlRaceCodesList[0].ChildNodes.Count > 0)
            //    {
            //        foreach (XmlNode CodeNode in xmlRaceCodesList[0].ChildNodes)
            //        {
            //            ValidRaceCodeList.Add(CodeNode.Attributes.GetNamedItem("value").Value.ToString());
            //        }
            //    }
            //    XmlNodeList xmlEthnicGroupCodesList = xmldoc.GetElementsByTagName("EthnicGroupCodeList");
            //    if (xmlEthnicGroupCodesList != null && xmlEthnicGroupCodesList.Count > 0 && xmlEthnicGroupCodesList[0].ChildNodes != null && xmlEthnicGroupCodesList[0].ChildNodes.Count > 0)
            //    {
            //        foreach (XmlNode CodeNode in xmlEthnicGroupCodesList[0].ChildNodes)
            //        {
            //            ValidEthnicGroupCodeList.Add(CodeNode.Attributes.GetNamedItem("value").Value.ToString());
            //        }
            //    }
            //}


            LookupList_CCDAErrors ilistLookupList_CCDAErrors = ConfigureBase<LookupList_CCDAErrors>.ReadJson("LookupList_CCDAErrors.json");

            if (ilistLookupList_CCDAErrors != null && ilistLookupList_CCDAErrors.InvalidList != null)
            {
                if (ilistLookupList_CCDAErrors.InvalidList.CodeList != null)
                {
                    List<Code> Getcodelist = ilistLookupList_CCDAErrors.InvalidList.CodeList.Code;
                    if (Getcodelist.Count > 0)
                    {
                        for (int i = 0; i < Getcodelist.Count; i++)
                        {
                            InValidCodeList.Add(Getcodelist[i].value);
                        }
                    }
                }
                if (ilistLookupList_CCDAErrors.InvalidList.ValueCodeList != null)
                {
                    List<Code> Getcodelist = ilistLookupList_CCDAErrors.InvalidList.ValueCodeList.Code;
                    if (Getcodelist.Count > 0)
                    {
                        for (int i = 0; i < Getcodelist.Count; i++)
                        {
                            InValidValueCodeList.Add(Getcodelist[i].value);
                        }
                    }
                }
                if (ilistLookupList_CCDAErrors.InvalidList.RouteCodeList != null)
                {
                    string Getcodelist = ilistLookupList_CCDAErrors.InvalidList.RouteCodeList.Code.value;
                    InValidRouteCodeList.Add(Getcodelist);
                }
                if (ilistLookupList_CCDAErrors.InvalidList.ValueCodeList != null)
                {
                    List<Units> GetUnits = ilistLookupList_CCDAErrors.InvalidList.UnitList.Unit;
                    if (GetUnits.Count > 0)
                    {
                        for (int i = 0; i < GetUnits.Count; i++)
                        {
                            InValidUnitList.Add(GetUnits[i].value);
                        }
                    }
                }
            }
            if (ilistLookupList_CCDAErrors != null && ilistLookupList_CCDAErrors.ValidList != null)
            {
                if (ilistLookupList_CCDAErrors.ValidList.ConfidentialityCodeList != null)
                {
                    List<Code> Getcodelist = ilistLookupList_CCDAErrors.ValidList.ConfidentialityCodeList.Code;
                    if (Getcodelist.Count > 0)
                    {
                        for (int i = 0; i < Getcodelist.Count; i++)
                        {
                            ValidConfidentialityCodeList.Add(Getcodelist[i].value);
                        }
                    }
                }
                if (ilistLookupList_CCDAErrors.ValidList.LanguageCodeList != null)
                {
                    List<Code> Getcodelist = ilistLookupList_CCDAErrors.ValidList.LanguageCodeList.Code;
                    if (Getcodelist.Count > 0)
                    {
                        for (int i = 0; i < Getcodelist.Count; i++)
                        {
                            ValidLanguageCodeList.Add(Getcodelist[i].value);
                        }
                    }
                }
                if (ilistLookupList_CCDAErrors.ValidList.AdministrativeGenderCodeList != null)
                {
                    List<Code> Getcodelist = ilistLookupList_CCDAErrors.ValidList.AdministrativeGenderCodeList.Code;
                    if (Getcodelist.Count > 0)
                    {
                        for (int i = 0; i < Getcodelist.Count; i++)
                        {
                            ValidAdministrativeGenderCodeList.Add(Getcodelist[i].value);
                        }
                    }
                }
                if (ilistLookupList_CCDAErrors.ValidList.RaceCodeList != null)
                {
                    List<Code> Getcodelist = ilistLookupList_CCDAErrors.ValidList.RaceCodeList.Code;
                    if (Getcodelist.Count > 0)
                    {
                        for (int i = 0; i < Getcodelist.Count; i++)
                        {
                            ValidRaceCodeList.Add(Getcodelist[i].value);
                        }
                    }
                }

                if (ilistLookupList_CCDAErrors.ValidList.EthnicGroupCodeList != null)
                {
                    List<Code> Getcodelist = ilistLookupList_CCDAErrors.ValidList.EthnicGroupCodeList.Code;
                    if (Getcodelist.Count > 0)
                    {
                        for (int i = 0; i < Getcodelist.Count; i++)
                        {
                            ValidEthnicGroupCodeList.Add(Getcodelist[i].value);
                        }
                    }
                }
            }
        }

        /**To maintain the user access log**/
        public static void WriteApplicationAccessInfo(string sUserName, string sFacilityName)
        {
            //try
            //{
            //    string LoggedinDateandTime_PST = UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString();
            //    string sIPAddress = GetIPAddress();
            //    if (LoggedinDateandTime_PST != string.Empty)
            //    {
            //        LoggedinDateandTime_PST = LoggedinDateandTime_PST.Replace(":", "_").Replace("/", "_").Replace(" ", "_");
            //    }
            //    string FileName = LoggedinDateandTime_PST + "$" + sUserName + "$" + sIPAddress + "$" + sFacilityName + ".txt";
            //    if (System.Configuration.ConfigurationSettings.AppSettings["UserAccessFilePath"] != null && System.Configuration.ConfigurationSettings.AppSettings["UserAccessFilePath"].ToString() != string.Empty)
            //    {
            //        string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["UserAccessFilePath"], FileName);
            //        if (!File.Exists(strXmlFilePath) == true)
            //        {
            //            try
            //            {
            //                File.CreateText(strXmlFilePath);
            //            }
            //            catch
            //            {

            //            }
            //        }
            //    }

            //}
            //catch
            //{

            //}
        }
        public static string GetIPAddress()
        {
            string strIPPinged = string.Empty;
            try
            {
                string versionInfo = ConfigurationSettings.AppSettings["VersionConfiguration"] != null ? ConfigurationSettings.AppSettings["VersionConfiguration"].ToString() : string.Empty;
                if (versionInfo != string.Empty && versionInfo.IndexOf('|') > -1 && versionInfo.Split('|')[1] != null)
                    strIPPinged = versionInfo.Split('|')[1].ToString();
                return strIPPinged;
            }
            catch
            {
                return string.Empty;
            }

        }
        // Method not in use
        //public string NamingConventionGeneration(string OutputNamingConvention, string HumanID, string EncounterID, string FacilityName, string ProviderName)
        //{
        //    string[] OutputName = OutputNamingConvention.Split('~');
        //    string NotesName = OutputNamingConvention;
        //    string result = string.Empty;
        //    for (int i = 0; i < OutputName.Length; i++)
        //    {
        //        if (OutputName[i] != "")
        //        {
        //            result = ExtractBetween(OutputName[i], "[", "]");
        //            if (result.ToUpper().Contains("FACILITY"))
        //            {
        //                NotesName = NotesName.Replace("[" + result + "]", FacilityName.Replace(",", ""));
        //            }

        //            if (result.ToUpper().Contains("HUMAN"))
        //            {
        //                NotesName = NotesName.Replace("[" + result + "]", HumanID);
        //            }
        //            if (result.ToUpper().Contains("ENCOUNTER"))
        //            {
        //                NotesName = NotesName.Replace("[" + result + "]", EncounterID.ToString());

        //            }
        //            if (result.ToUpper().Contains("MEMBER") || result.ToUpper().Contains("LAST") || result.ToUpper().Contains("FIRST") || result.ToUpper().Contains("DOB"))
        //            {
        //                string sExternalAccNo = string.Empty;
        //                string sLastName = string.Empty;
        //                string sFirstName = string.Empty;
        //                string sDOB = string.Empty;
        //                string human_id = "Human" + "_" + HumanID + ".xml";

        //                string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);
        //                if (File.Exists(strXmlHumanPath) == true)
        //                {
        //                    XmlDocument itemDoc = new XmlDocument();
        //                    XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
        //                    // itemDoc.Load(XmlText);
        //                    using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //                    {
        //                        itemDoc.Load(fs);

        //                        XmlText.Close();
        //                        if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
        //                        {
        //                            if (result.ToUpper().Contains("MEMBER"))
        //                            {
        //                                sExternalAccNo = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value.ToString();
        //                                NotesName = NotesName.Replace("[" + result + "]", sExternalAccNo);
        //                            }
        //                            else if (result.ToUpper().Contains("LAST"))
        //                            {
        //                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
        //                                NotesName = NotesName.Replace("[" + result + "]", (sLastName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
        //                            }
        //                            else if (result.ToUpper().Contains("FIRST"))
        //                            {
        //                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
        //                                NotesName = NotesName.Replace("[" + result + "]", (sFirstName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
        //                            }
        //                            else if (result.ToUpper().Contains("DOB") || (OutputName[i].ToUpper().Contains("DATE") && OutputName[i].ToUpper().Contains("BIRTH")))
        //                            {
        //                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
        //                                NotesName = NotesName.Replace("[" + result + "]", sDOB);
        //                            }
        //                        }
        //                        fs.Close();
        //                        fs.Dispose();
        //                    }
        //                }

        //            }
        //            if ((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE")) || result.ToUpper().Contains("PROVIDER"))
        //            {
        //                string Encounterxml = "Encounter" + "_" + EncounterID + ".xml";
        //                string strXmlEncounterPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], Encounterxml);
        //                if (File.Exists(strXmlEncounterPath) == true)
        //                {
        //                    XmlDocument itemDoc = new XmlDocument();
        //                    XmlTextReader XmlText = new XmlTextReader(strXmlEncounterPath);
        //                    // itemDoc.Load(XmlText);
        //                    using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //                    {
        //                        itemDoc.Load(fs);

        //                        XmlText.Close();
        //                        if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
        //                        {
        //                            string sDOS = string.Empty;
        //                            if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value != "")
        //                            {
        //                                sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
        //                            }
        //                            NotesName = NotesName.Replace("[" + result + "]", sDOS);
        //                        }
        //                        fs.Close();
        //                        fs.Dispose();
        //                    }
        //                }
        //            }
        //            if (result.ToUpper().Contains("PROVIDER"))
        //            {
        //                NotesName = NotesName.Replace("[" + result + "]", ProviderName);
        //            }
        //        }
        //    }
        //    if (NotesName.Contains('['))
        //    {
        //        string[] sName = NotesName.Split('~');

        //        for (int i = 0; i < sName.Length; i++)
        //        {
        //            if (sName[i] != "")
        //            {
        //                if (sName[i].Contains("["))
        //                {
        //                    result = ExtractBetween(sName[i], "[", "]");
        //                    NotesName = NotesName.Replace("[" + result + "]", "");
        //                }
        //            }
        //        }
        //    }
        //    NotesName = NotesName.Replace("~", "").Replace("__", "_");
        //    return NotesName;
        //}

        string ExtractBetween(string text, string start, string end)
        {
            int iStart = text.IndexOf(start);
            iStart = (iStart == -1) ? 0 : iStart + start.Length;
            int iEnd = text.LastIndexOf(end);
            if (iEnd == -1)
            {
                iEnd = text.Length;
            }
            int len = iEnd - iStart;

            return text.Substring(iStart, len);
        }
        public static string ValidatePolicyHolderID(string sPolicyHolderID)
        {
            string sMyResult = string.Empty;

            //if (sPolicyHolderID.Length < 10 || sPolicyHolderID.Length > 12)
            //{
            //    sMyResult = "Fail" + "|1EG4TE5MK73";
            //    return sMyResult;
            //}

            int iPosition = 0;
            //CAP-1921
            if (Char.IsDigit(sPolicyHolderID[0]) && Char.IsLetter(sPolicyHolderID[1]) && Char.IsLetter(sPolicyHolderID[2]))
            {
                goto MBI;
            }
            //else if (Char.IsLetter(sPolicyHolderID[0]) == false && Char.IsLetter(sPolicyHolderID[1]) == false && Char.IsLetter(sPolicyHolderID[2]) == false)
            //{
            //    goto HCN1;
            //}
            //else if (Char.IsLetter(sPolicyHolderID[0]) == true && Char.IsLetter(sPolicyHolderID[1]) == false && Char.IsLetter(sPolicyHolderID[2]) == false)
            //{
            //    goto RRB1;
            //}
            //else if (Char.IsLetter(sPolicyHolderID[0]) == true && Char.IsLetter(sPolicyHolderID[1]) == true && Char.IsLetter(sPolicyHolderID[2]) == false)
            //{
            //    goto RRB2;
            //}
            //else if (Char.IsLetter(sPolicyHolderID[0]) == true && Char.IsLetter(sPolicyHolderID[1]) == true && Char.IsLetter(sPolicyHolderID[2]) == true)
            //{
            //    goto RRB3;
            //}
            else if (Char.IsDigit(sPolicyHolderID[0]) && Char.IsLetter(sPolicyHolderID[1]) && Char.IsDigit(sPolicyHolderID[2]))
            {
                goto MBI;
            }
            //else if (Char.IsLetter(sPolicyHolderID[0]) == true && Char.IsLetter(sPolicyHolderID[1]) == false && Char.IsLetter(sPolicyHolderID[2]) == true)
            //{
            //    goto RRB1;
            //}

            iPosition = 0;
        MBI: if (sPolicyHolderID.Length == 11)
            {
                foreach (char c in sPolicyHolderID)
                {
                    //CAP-1921 - For Medicare carrier - New Policy Holder_id Format to be included
                    //switch (iPosition)
                    //{
                    //    case 0:
                    //        if (Char.IsLetter(c) == true)
                    //        {
                    //            sMyResult = "Fail" + "|1EG4TE5MK73";
                    //            return sMyResult;
                    //        }
                    //        if (c == '0')
                    //        {
                    //            sMyResult = "Fail" + "|1EG4TE5MK73";
                    //            return sMyResult;
                    //        }
                    //        break;
                    //    case 1:
                    //    case 4:
                    //    case 7:
                    //    case 8:
                    //        if (Char.IsLetter(c) == false)
                    //        {
                    //            sMyResult = "Fail" + "|1EG4TE5MK73";
                    //            return sMyResult;
                    //        }
                    //        if (c == 'S' || c == 'L' || c == 'O' || c == 'I' || c == 'B' || c == 'Z')
                    //        {
                    //            sMyResult = "Fail" + "|1EG4TE5MK73";
                    //            return sMyResult;
                    //        }
                    //        break;
                    //    case 2:
                    //    case 5:
                    //        if (Char.IsLetterOrDigit(c) == false)
                    //        {
                    //            sMyResult = "Fail" + "|1EG4TE5MK73";
                    //            return sMyResult;
                    //        }
                    //        if (c == 'S' || c == 'L' || c == 'O' || c == 'I' || c == 'B' || c == 'Z')
                    //        {
                    //            sMyResult = "Fail" + "|1EG4TE5MK73";
                    //            return sMyResult;
                    //        }
                    //        break;
                    //    case 3:
                    //    case 6:
                    //    case 9:
                    //    case 10:
                    //        if (Char.IsLetter(c) == true)
                    //        {
                    //            sMyResult = "Fail" + "|1EG4TE5MK73";
                    //            return sMyResult;
                    //        }
                    //        break;

                    //}
                    switch (iPosition)
                    {
                        case 1:
                        case 4:
                        case 7:
                        case 8: //1, 4, 7, 8 = characters
                            if (Char.IsLetter(c) == false)
                            {
                                sMyResult = "Fail" + "|1AA0AA0AA00 or 1A00A00AA00";
                                return sMyResult;
                            }
                            break;
                        case 0:
                        case 3:
                        case 6:
                        case 9:
                        case 10: //0, 3, 6, 9, 10 = numbers
                            if (Char.IsDigit(c) == false)
                            {
                                sMyResult = "Fail" + "|1AA0AA0AA00 or 1A00A00AA00";
                                return sMyResult;
                            }
                            break;
                        case 2:
                        case 5: //2, 5 = letters or numbers
                            if (Char.IsLetterOrDigit(c) == false)
                            {
                                sMyResult = "Fail" + "|1AA0AA0AA00 or 1A00A00AA00";
                                return sMyResult;
                            }
                            break;
                    }
                    iPosition += 1;
                }
                sMyResult = "Success";
                return sMyResult;
            }
            else
            {
                sMyResult = "Fail" + "|1AA0AA0AA00 or 1A00A00AA00";
                return sMyResult;
            }


        RRB1: if (sPolicyHolderID.Length == 10)
            {
                iPosition = 0;
                foreach (char c in sPolicyHolderID)
                {
                    switch (iPosition)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            if (Char.IsLetter(c) == true)
                            {
                                sMyResult = "Fail" + "|A000000000";
                                return sMyResult;
                            }
                            break;
                        case 0:
                            if (Char.IsLetter(c) == false)
                            {
                                sMyResult = "Fail" + "|A000000000";
                                return sMyResult;
                            }
                            break;
                    }
                    iPosition += 1;
                }
                sMyResult = "Success";
                return sMyResult;
            }
            else
            {
                sMyResult = "Fail" + "|A000000000";
                return sMyResult;
            }

        HCN1: if (sPolicyHolderID.Length == 10)
            {
                iPosition = 0;
                foreach (char c in sPolicyHolderID)
                {
                    switch (iPosition)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                            if (Char.IsLetter(c) == true)
                            {
                                sMyResult = "Fail" + "|000000000A";
                                goto HCN2;
                            }
                            break;
                        case 9:
                            if (Char.IsLetter(c) == false)
                            {
                                sMyResult = "Fail" + "|000000000A";
                                goto HCN2;
                            }
                            break;
                    }
                    iPosition += 1;
                }
                sMyResult = "Success";
                return sMyResult;
            }
            else
            {
                sMyResult = "Fail" + "|000000000A";
                goto HCN2;
            }

        HCN2: if (sPolicyHolderID.Length == 11)
            {
                iPosition = 0;
                foreach (char c in sPolicyHolderID)
                {
                    switch (iPosition)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                            if (Char.IsLetter(c) == true)
                            {
                                sMyResult = "Fail" + "|000000000B1 or 000000000HA";
                                return sMyResult;
                            }
                            break;
                        case 9:
                            if (Char.IsLetter(c) == false)
                            {
                                sMyResult = "Fail" + "|000000000B1 or 000000000HA";
                                return sMyResult;
                            }
                            break;
                        case 10:
                            if (Char.IsLetterOrDigit(c) == false)
                            {
                                sMyResult = "Fail" + "|000000000B1 or 000000000HA";
                                return sMyResult;
                            }
                            break;
                    }
                    iPosition += 1;
                }
                sMyResult = "Success";
                return sMyResult;
            }
            else
            {
                sMyResult = "Fail" + "|000000000B1 or 000000000HA";
                return sMyResult;
            }

        RRB2: if (sPolicyHolderID.Length == 11)
            {
                iPosition = 0;
                foreach (char c in sPolicyHolderID)
                {
                    switch (iPosition)
                    {
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            if (Char.IsLetter(c) == true)
                            {
                                sMyResult = "Fail" + "|MA000000000";
                                return sMyResult;
                            }
                            break;
                        case 0:
                        case 1:
                            if (Char.IsLetter(c) == false)
                            {
                                sMyResult = "Fail" + "|MA000000000";
                                return sMyResult;
                            }
                            break;

                    }
                    iPosition += 1;
                }
                sMyResult = "Success";
                return sMyResult;
            }
            else
            {
                sMyResult = "Fail" + "|MA000000000";
                return sMyResult;
            }

        RRB3: if (sPolicyHolderID.Length == 12)
            {
                iPosition = 0;
                foreach (char c in sPolicyHolderID)
                {
                    switch (iPosition)
                    {
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            if (Char.IsLetter(c) == true)
                            {
                                sMyResult = "Fail" + "|WCA000000000";
                                return sMyResult;
                            }
                            break;
                        case 0:
                        case 1:
                        case 2:
                            if (Char.IsLetter(c) == false)
                            {
                                sMyResult = "Fail" + "|WCA000000000";
                                return sMyResult;
                            }
                            break;
                    }
                    iPosition += 1;
                }
                sMyResult = "Success";
                return sMyResult;
            }
            else
            {
                sMyResult = "Fail" + "|WCA000000000";
                return sMyResult;
            }

            return sMyResult;
        }
        public static string CreateXMLByBatchProcess(string sXmlType, string sXMLID)
        {
            string status = "";
            try
            {
                status = "First Block";
                //System.Diagnostics.Process proc = new System.Diagnostics.Process();
                string sXmlName = string.Empty;
                sXmlName = (sXmlType.ToUpper().Contains("HUMAN")) ? "Human_" : "Encounter_";
                int iTryCount = 0;
                status = "Second Block";
                string sSourceFile = System.Configuration.ConfigurationManager.AppSettings["CopyFromSource_" + sXmlType].ToString() + "\\" + sXmlName + sXMLID + ".xml";
                string sDestFile = System.Configuration.ConfigurationManager.AppSettings["CopyToDestination"].ToString() + "\\" + sXmlName + sXMLID + ".xml";
                if (File.Exists(sSourceFile))
                {
                    File.Delete(sSourceFile);
                }
                status = "Third Block Block";
                string batchfile = System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileFor" + sXmlType].ToString();
                if (File.Exists(batchfile))
                {
                    using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
                    {
                        status = "Third Block - Sub 1";
                        proc.StartInfo.FileName = System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileFor" + sXmlType].ToString();
                        status = "Third Block - Sub 2";
                        bool bStart = proc.Start();
                        status = bStart + " Third Block - Sub 3 " + System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileFor" + sXmlType].ToString();
                        proc.WaitForExit();
                        status = bStart + " Third Block - Sub 4 " + System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileFor" + sXmlType].ToString();
                    }
                    //status = "Third Block - Sub 1";
                    //proc.StartInfo.FileName = System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileFor" + sXmlType].ToString();
                    //status = "Third Block - Sub 2";
                    //proc.Start();
                    //status = "Third Block - Sub 3";
                    //proc.WaitForExit();
                    //status = "Third Block - Sub 4";
                    //retry:
                    //try
                    //{
                    //    System.IO.File.Copy(sSourceFile, sDestFile, true);
                    //}
                    //catch (Exception Ex)
                    //{
                    //    //if (Ex.Message.ToLower().Contains("used by another process"))
                    //    //{
                    //    //    if (iTryCount < 20)
                    //    //    {
                    //    //        iTryCount++;
                    //    //        Thread.Sleep(3000);
                    //    //        goto retry;
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        throw new Exception(status + " " + Ex.Message + "  " + Ex.InnerException);
                    //    //    }
                    //    //}
                    //    throw new Exception(status + " " + Ex.Message + "  " + Ex.InnerException);


                    //}
                    InsertIntostatserrorlog(sXmlName + sXMLID + ".xml");
                    status = string.Empty;
                }
                else
                {
                    status = "Batch File Not found-FileName:" + batchfile;
                }
                return status;

            }
            catch (Exception Ex)
            {
                //CAP-1942
                throw new Exception(status + " " + Ex.Message + "  " + Ex.InnerException, Ex);
            }

        }
        public static string GenerateXML(string sXMLID, string sXMLType)
        {

            string sResult = string.Empty;


            try
            {
                ulong XML_ID = Convert.ToUInt32(sXMLID);

                if (sXMLType.ToUpper().Contains("HUMAN"))
                {
                //Check if Human_ID exists
                //bool isPresent = CheckHumanIDValidity(XML_ID);
                //if (isPresent)
                //{
                ln:
                    bool ishumancount = GetListHuman();

                    if (ishumancount)
                    {
                        Thread.Sleep(3000);
                        goto ln;
                    }

                    bool isHumanDone = InsertIntoListHuman(XML_ID);


                    if (isHumanDone)
                    {
                        // CreateXMLByBackupProcess("Human", Application, XML_ID.ToString());
                        string status = CreateXMLByBatchProcess("Human", XML_ID.ToString());
                        if (status == string.Empty)
                        {
                            try
                            {
                                string sXMLHumanDoc = string.Empty;
                                HumanBlobManager HumanBlobMngr = new HumanBlobManager();
                                IList<Human_Blob> ilstHumanBlob = new List<Human_Blob>();
                                ilstHumanBlob = HumanBlobMngr.GetHumanBlob(XML_ID);
                                XmlDocument xmlHumanDoc = new XmlDocument();
                                if (ilstHumanBlob.Count > 0)
                                {
                                    sXMLHumanDoc = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                                    if (sXMLHumanDoc.Substring(0, 1) != "<")
                                        sXMLHumanDoc = sXMLHumanDoc.Substring(1, sXMLHumanDoc.Length - 1);
                                    xmlHumanDoc.LoadXml(sXMLHumanDoc);
                                    sResult = "Success";
                                }
                            }
                            catch (Exception ex)
                            {
                                sResult = "Failure";
                            }
                        }
                        else
                        {
                            sResult = status;
                        }
                        string sConnectionString = string.Empty;
                        sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        var builder = new MySqlConnectionStringBuilder(sConnectionString);

                        try
                        {
                            using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                            {
                                DBConnection.Open();
                                using (MySqlTransaction DBTransaction = DBConnection.BeginTransaction())
                                {
                                    string sQuery = "delete from list_human where human_id=" + XML_ID.ToString() + ";";
                                    using (MySqlCommand cmdInsert = new MySqlCommand(sQuery, DBConnection, DBTransaction))
                                    {
                                        cmdInsert.CommandText = sQuery;
                                        cmdInsert.CommandType = System.Data.CommandType.Text;
                                        try
                                        {
                                            cmdInsert.ExecuteNonQuery();
                                            DBTransaction.Commit();
                                        }
                                        catch (Exception e)
                                        {
                                            DBTransaction.Rollback();
                                            throw;
                                        }
                                        finally { }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            //CAP-1942
                            sResult = e.Message + e;
                        }

                    }
                    else
                        sResult = "ERROR: Unable to Generate XML for " + sXMLID + ". Please try again later.";
                    //}
                    //else
                    //    sResult = "The given Human_ID: " + sXMLID + " does not exist. Please enter a valid ID.";
                }
                else
                {
                    if (sXMLType.ToUpper().Contains("ENCOUNTER"))
                    {
                        //Check if Encounter_ID exists in Encounter/ Encounter_arc
                        string sTableName = string.Empty;
                        sTableName = CheckEncounterIDValidity(XML_ID);
                        if (sTableName.Contains("ENCOUNTER"))
                        {
                        ln:
                            bool ishumancount = GetListEncounter(sTableName, XML_ID);


                            if (ishumancount)
                            {
                                Thread.Sleep(3000);
                                goto ln;
                            }
                            bool isEncDone = InsertIntoListEncounterCurrOrArc(XML_ID, sTableName);
                            if (isEncDone)
                            {
                                string sXML = (sTableName.ToUpper() == "ENCOUNTER_CURRENT") ? "Encounter" : "EncounterArc";
                                // CreateXMLByBackupProcess(sXML, Application, XML_ID.ToString());
                                string status = CreateXMLByBatchProcess(sXML, XML_ID.ToString());
                                if (status == string.Empty)
                                {
                                    EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
                                    IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(XML_ID);
                                    if (ilstEncounterBlob.Count > 0)
                                    {
                                        string sXMLContent = string.Empty;
                                        XmlDocument xmlDoc = new XmlDocument();
                                        try
                                        {
                                            sXMLContent = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                                            if (sXMLContent.Substring(0, 1) != "<")
                                                sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                                            xmlDoc.LoadXml(sXMLContent);
                                            sResult = "Success";
                                        }
                                        catch
                                        {
                                            sResult = "Failure";
                                        }
                                    }
                                }
                                else
                                {
                                    sResult = status;
                                }
                                string sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                var builder = new MySqlConnectionStringBuilder(sConnectionString);

                                try
                                {
                                    using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                                    {
                                        DBConnection.Open();
                                        using (MySqlTransaction DBTransaction = DBConnection.BeginTransaction())
                                        {
                                            string sQuery = string.Empty;
                                            if (sTableName == "ENCOUNTER_CURRENT")
                                                sQuery = "delete from list_encounter where encounter_id=" + XML_ID.ToString() + ";";
                                            else
                                                if (sTableName == "ENCOUNTER_ARCHIVE")
                                                sQuery = "delete from list_encounter_arc where encounter_id=" + XML_ID.ToString() + ";";
                                            using (MySqlCommand cmdInsert = new MySqlCommand(sQuery, DBConnection, DBTransaction))
                                            {
                                                cmdInsert.CommandText = sQuery;
                                                cmdInsert.CommandType = System.Data.CommandType.Text;
                                                try
                                                {
                                                    cmdInsert.ExecuteNonQuery();
                                                    DBTransaction.Commit();
                                                }
                                                catch (Exception e)
                                                {
                                                    DBTransaction.Rollback();
                                                    throw;
                                                }
                                                finally { }
                                            }
                                        }
                                    }
                                }
                                catch (Exception e) { throw; }


                            }
                            else
                            {
                                sResult = "ERROR: Unable to generate XML for " + sXMLID + ". Please try again later.";
                            }
                        }
                        else
                        {
                            if (sTableName == "INVALID DOS")
                            {
                                sResult = "The Encounter of ID: " + sXMLID + " has not yet been processed. Hence XML could not be generated.";
                            }
                            else
                            {
                                sResult = "The given Encounter_ID: " + sXMLID + " does not exist. Please enter a valid ID.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //CAP-1942
                sResult = "ERROR: " + ex.Message + " STACKTRACE: " + ex.StackTrace + ex;

            }
            return sResult;
        }
        public static bool InsertIntoListEncounterCurrOrArc(ulong EncID, string sTable)
        {
            bool isInserted = false;
            string sConnectionString = string.Empty;
            sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            var builder = new MySqlConnectionStringBuilder(sConnectionString);

            try
            {
                using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                {
                    DBConnection.Open();
                    using (MySqlTransaction DBTransaction = DBConnection.BeginTransaction())
                    {
                        string sQuery = string.Empty;
                        if (sTable.ToUpper() == "ENCOUNTER_CURRENT")
                            sQuery = "insert into list_encounter values(" + EncID.ToString() + ", current_timestamp());";
                        else
                            if (sTable.ToUpper() == "ENCOUNTER_ARCHIVE")
                            sQuery = "insert into list_encounter_arc values(" + EncID.ToString() + ", current_timestamp());";
                        using (MySqlCommand cmdInsert = new MySqlCommand(sQuery, DBConnection, DBTransaction))
                        {
                            cmdInsert.CommandText = sQuery;
                            cmdInsert.CommandType = System.Data.CommandType.Text;
                            try
                            {
                                cmdInsert.ExecuteNonQuery();
                                DBTransaction.Commit();
                                isInserted = true;
                            }
                            catch (Exception e)
                            {
                                DBTransaction.Rollback();
                                isInserted = false;
                                throw;
                            }
                            finally { }
                        }
                    }
                }
            }
            catch (Exception e) { throw; }

            return isInserted;
        }

        public static bool GetListEncounter(string sTable, ulong deletionID)
        {
            string sConnectionString = string.Empty;
            bool bExists = false;
            sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            var builder = new MySqlConnectionStringBuilder(sConnectionString);

            try
            {
                using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                {
                    DBConnection.Open();
                    using (MySqlTransaction DBTransaction = DBConnection.BeginTransaction())
                    {
                        string sQuery = string.Empty;
                        if (sTable == "ENCOUNTER_CURRENT")
                            sQuery = "select * from   list_encounter ;";
                        else
                            if (sTable == "ENCOUNTER_ARCHIVE")
                            sQuery = "select *  from list_encounter_arc;";
                        using (MySqlCommand cmdCheck = new MySqlCommand(sQuery, DBConnection))
                        {
                            cmdCheck.CommandText = sQuery;
                            cmdCheck.CommandType = System.Data.CommandType.Text;
                            try
                            {
                                int iRows = 0;
                                string sResult = string.Empty;
                                iRows = Convert.ToInt32(cmdCheck.ExecuteScalar());
                                if (iRows > 0)
                                    bExists = true;
                            }
                            catch (Exception e) { throw; }
                            finally { }
                        }
                    }
                }
            }
            catch (Exception e) { throw; }
            return bExists;
        }

        public static string CheckEncounterIDValidity(ulong EncID)
        {
            ulong result_ID = 0;
            string result_Table = string.Empty;
            string DOS = string.Empty;
            string connectionString = string.Empty;
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            var builder = new MySqlConnectionStringBuilder(connectionString);

            try
            {
                using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                {
                    DBConnection.Open();
                    string sQuery = "select e.Encounter_ID, cast(e.Date_of_Service as char(100)) as DOS, 'ENCOUNTER_CURRENT' as Enc_Table from encounter e where encounter_id=" + EncID.ToString() +
                                    " union all select a.Encounter_ID, cast(a.Date_of_Service as char(100)) as DOS, 'ENCOUNTER_ARCHIVE' as Enc_Table from encounter_arc a where encounter_id=" + EncID.ToString() + ";";
                    using (MySqlCommand cmdCheck = new MySqlCommand(sQuery, DBConnection))
                    {
                        cmdCheck.CommandText = sQuery;
                        cmdCheck.CommandType = System.Data.CommandType.Text;
                        try
                        {
                            MySqlDataReader objReader = cmdCheck.ExecuteReader();
                            if (objReader == null)
                                objReader = cmdCheck.ExecuteReader();
                            while (objReader.Read())
                            {
                                result_ID = Convert.ToUInt32(objReader["Encounter_ID"]);
                                DOS = objReader["DOS"].ToString();
                                result_Table = Convert.ToString(objReader["Enc_Table"]);
                            }
                            objReader.Close();
                            //cmdCheck.Cancel();
                        }
                        catch (Exception e) { throw; }
                        finally { }
                    }
                }
            }
            catch (Exception e) { throw; }

            if (DOS != string.Empty && Convert.ToDateTime(DOS) == DateTime.MinValue)
                result_Table = "INVALID DOS";

            return result_Table;
        }

        public static bool InsertIntoListHuman(ulong humanID)
        {
            bool isInserted = false;
            string sConnectionString = string.Empty;
            sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            var builder = new MySqlConnectionStringBuilder(sConnectionString);

            try
            {
                using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                {
                    DBConnection.Open();
                    using (MySqlTransaction DBTransaction = DBConnection.BeginTransaction())
                    {
                        string sQuery = "insert into list_human values(" + humanID.ToString() + ");";
                        using (MySqlCommand cmdInsert = new MySqlCommand(sQuery, DBConnection, DBTransaction))
                        {
                            cmdInsert.CommandText = sQuery;
                            cmdInsert.CommandType = System.Data.CommandType.Text;
                            try
                            {
                                cmdInsert.ExecuteNonQuery();
                                DBTransaction.Commit();
                                isInserted = true;
                            }
                            catch (Exception e)
                            {
                                DBTransaction.Rollback();
                                isInserted = false;
                                //CAP-1942
                                throw new Exception(e.Message, e);
                            }
                            finally { }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //CAP-1942
                throw new Exception(e.Message, e);
            }

            return isInserted;
        }
        public static bool InsertIntostatserrorlog(string sxmlname)
        {
            bool isInserted = false;
            string sConnectionString = string.Empty;
            sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            var builder = new MySqlConnectionStringBuilder(sConnectionString);

            try
            {
                using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                {
                    DBConnection.Open();
                    string smessage = "XML is generated for " + sxmlname;
                    string version = "";
                    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                    string[] server = version.Split('|');
                    string serverno = "";
                    if (server.Length > 1)
                        serverno = server[1].Trim();
                    using (MySqlTransaction DBTransaction = DBConnection.BeginTransaction())
                    {
                        string sQuery = "insert into  stats_apperrorlog values(0,'" + smessage + "', '" + serverno + "','" + DateTime.Now + "','" + ClientSession.UserName + "','" + ClientSession.EncounterId + "','" + ClientSession.HumanId + "','" + ClientSession.PhysicianId + "',' ','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        using (MySqlCommand cmdInsert = new MySqlCommand(sQuery, DBConnection, DBTransaction))
                        {
                            cmdInsert.CommandText = sQuery;
                            cmdInsert.CommandType = System.Data.CommandType.Text;
                            try
                            {
                                cmdInsert.ExecuteNonQuery();
                                DBTransaction.Commit();
                                isInserted = true;
                            }
                            catch (Exception e)
                            {
                                DBTransaction.Rollback();
                                isInserted = false;
                                //CAP-1942
                                throw new Exception(e.Message, e);
                            }
                            finally { }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //CAP-1942
                throw new Exception(e.Message, e);
            }

            return isInserted;
        }

        public static bool InsertIntoListCCD(ulong humanID, ulong encounterID, string sInput, string sOutputLocation, string sDSN)
        {
            bool isInserted = false;
            string sConnectionString = string.Empty;
            sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            var builder = new MySqlConnectionStringBuilder(sConnectionString);

            try
            {
                using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                {
                    DBConnection.Open();
                    using (MySqlTransaction DBTransaction = DBConnection.BeginTransaction())
                    {
                        string sQuery = "insert into list_ccd values(" + humanID.ToString() + ", " + encounterID.ToString() + ", '" + sInput + "', '" + sOutputLocation.ToString() + "', '" + sDSN.ToString() + "');";
                        using (MySqlCommand cmdInsert = new MySqlCommand(sQuery, DBConnection, DBTransaction))
                        {
                            cmdInsert.CommandText = sQuery;
                            cmdInsert.CommandType = System.Data.CommandType.Text;
                            try
                            {
                                cmdInsert.ExecuteNonQuery();
                                DBTransaction.Commit();
                                isInserted = true;
                            }
                            catch (Exception e)
                            {
                                DBTransaction.Rollback();
                                isInserted = false;
                                //CAP-1942
                                throw new Exception(e.Message, e);
                            }
                            finally { }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //CAP-1942
                throw new Exception(e.Message, e);
            }

            return isInserted;
        }

        public static string CreateCCDXMLByBatchProcess(string sOutputLocation)
        {
            string status = "";

            try
            {
                status = "First Block";
                status = "Third Block Block";
                string batchfile = System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString();
                if (File.Exists(batchfile))
                {
                    try
                    {
                        status = "Third Block - Sub 1";
                        var proc1 = new Process();
                        proc1.StartInfo.WorkingDirectory = Path.GetDirectoryName(System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString());
                        proc1.StartInfo.FileName = System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString();
                        proc1.StartInfo.Arguments = "-v -s -a";
                        status = "Third Block - Sub 2";
                        bool bStart = proc1.Start();
                        status = bStart + " Third Block - Sub 3 ";
                        proc1.WaitForExit();
                        status = bStart + " Third Block - Sub 4 ";
                        var exitCode = proc1.ExitCode;
                        proc1.Close();

                        File.Copy(System.Configuration.ConfigurationManager.AppSettings["CCDOutputLocation"].ToString(), sOutputLocation);
                    }
                    catch (Exception ex)
                    {
                        //CAP-1942
                        throw new Exception(status + " " + ex.Message + "  " + ex.InnerException, ex);
                    }

                    //using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
                    //{
                    //    status = "Third Block - Sub 1";
                    //    proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString());
                    //    proc.StartInfo.FileName = "@" + System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString();
                    //    status = "Third Block - Sub 2" +" " + proc.StartInfo.WorkingDirectory + " " + proc.StartInfo.FileName;
                    //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "UtilityManager - 4522 - CreateCCDXMLByBatchProcess: status  - " + status + " : Start", DateTime.Now, "0", "frmimageviewer");
                    //    bool bStart = proc.Start();
                    //    status = bStart + " Third Block - Sub 3 " + System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString();
                    //    proc.WaitForExit();
                    //    status = bStart + " Third Block - Sub 4 " + System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString();
                    //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "UtilityManager - 4527 - CreateCCDXMLByBatchProcess: status  - " + status + " : End", DateTime.Now, "0", "frmimageviewer");
                    //}
                    status = string.Empty;
                }
                else
                {
                    status = "Batch File Not found-FileName:" + batchfile;
                }
                return status;

            }
            catch (Exception Ex)
            {
                //CAP-1942
                throw new Exception(status + " " + Ex.Message + "  " + Ex.InnerException, Ex);
            }

        }
        public static bool CheckHumanIDValidity(ulong HumanID)
        {
            bool bExists = false;
            string connectionString = string.Empty;
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            var builder = new MySqlConnectionStringBuilder(connectionString);

            try
            {
                using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                {
                    DBConnection.Open();
                    string sQuery = "select count(*) from human where human_id=" + HumanID.ToString() + ";";
                    using (MySqlCommand cmdCheck = new MySqlCommand(sQuery, DBConnection))
                    {
                        cmdCheck.CommandText = sQuery;
                        cmdCheck.CommandType = System.Data.CommandType.Text;
                        try
                        {
                            int iRows = 0;
                            string sResult = string.Empty;
                            iRows = Convert.ToInt32(cmdCheck.ExecuteScalar());
                            if (iRows > 0)
                                bExists = true;
                        }
                        catch (Exception e) { throw; }
                        finally { }
                    }
                }
            }
            catch (Exception e) { throw; }
            return bExists;
        }


        public static bool GetListHuman()
        {
            string sConnectionString = string.Empty;
            sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            var builder = new MySqlConnectionStringBuilder(sConnectionString);
            bool bExists = false;
            try
            {
                using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                {
                    DBConnection.Open();
                    using (MySqlTransaction DBTransaction = DBConnection.BeginTransaction())
                    {
                        string sQuery = "Select *  from list_human ; ";
                        using (MySqlCommand cmdCheck = new MySqlCommand(sQuery, DBConnection))
                        {
                            cmdCheck.CommandText = sQuery;
                            cmdCheck.CommandType = System.Data.CommandType.Text;
                            try
                            {
                                int iRows = 0;
                                string sResult = string.Empty;
                                iRows = Convert.ToInt32(cmdCheck.ExecuteScalar());
                                if (iRows > 0)
                                    bExists = true;
                            }
                            catch (Exception e) { throw; }
                            finally { }
                        }
                    }
                }
            }
            catch (Exception e) { throw; }
            return bExists;
        }

        public static bool GetListCCD()
        {
            string sConnectionString = string.Empty;
            sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            var builder = new MySqlConnectionStringBuilder(sConnectionString);
            bool bExists = false;
            try
            {
                using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                {
                    DBConnection.Open();
                    using (MySqlTransaction DBTransaction = DBConnection.BeginTransaction())
                    {
                        string sQuery = "Select *  from list_ccd ; ";
                        using (MySqlCommand cmdCheck = new MySqlCommand(sQuery, DBConnection))
                        {
                            cmdCheck.CommandText = sQuery;
                            cmdCheck.CommandType = System.Data.CommandType.Text;
                            try
                            {
                                int iRows = 0;
                                string sResult = string.Empty;
                                iRows = Convert.ToInt32(cmdCheck.ExecuteScalar());
                                if (iRows > 0)
                                    bExists = true;

                            }
                            catch (Exception e)
                            {
                                //CAP-1942
                                throw new Exception(e.Message, e);
                            }
                            finally { }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //CAP-1942
                throw new Exception(e.Message, e);
            }
            return bExists;
        }


        //  public static string GenerateCCD(string sXMLID, string sXMLType)
        public static string GenerateCCD(ulong ulHumanID, ulong ulEncounterID, string sCheckedItems, string sOutputLocation, string sDSN)
        {
            string sResult = string.Empty;

            try
            {
            ln:
                bool ishumancount = GetListCCD();
                if (ishumancount)
                {
                    //Thread.Sleep(3000);
                    //goto ln;
                    return "1011192";
                }
                bool isHumanDone = InsertIntoListCCD(ulHumanID, ulEncounterID, sCheckedItems, sOutputLocation, sDSN);

                if (isHumanDone)
                {
                    // CreateXMLByBackupProcess("Human", Application, XML_ID.ToString());
                    string status = CreateCCDXMLByBatchProcess(sOutputLocation);
                    if (status == string.Empty)

                        sResult = "Success";

                    else
                        sResult = status;

                    string sConnectionString = string.Empty;
                    sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    var builder = new MySqlConnectionStringBuilder(sConnectionString);
                    try
                    {
                        using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                        {
                            DBConnection.Open();
                            using (MySqlTransaction DBTransaction = DBConnection.BeginTransaction())
                            {
                                string sQuery = "delete from list_ccd where encounter_id=" + ulEncounterID.ToString() + ";";
                                using (MySqlCommand cmdInsert = new MySqlCommand(sQuery, DBConnection, DBTransaction))
                                {
                                    cmdInsert.CommandText = sQuery;
                                    cmdInsert.CommandType = System.Data.CommandType.Text;
                                    try
                                    {
                                        cmdInsert.ExecuteNonQuery();
                                        DBTransaction.Commit();
                                    }
                                    catch (Exception e)
                                    {
                                        DBTransaction.Rollback();
                                        throw;
                                    }
                                    finally { }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //CAP-1942
                        sResult = e.Message + e;
                    }
                }
            }

            catch (Exception ex)
            {
                //CAP-1942
                sResult = "ERROR: " + ex.Message + " STACKTRACE: " + ex.StackTrace + ex;
            }
            return sResult;
        }
        public static string ImportC2ByBatchProcess()
        {
            string status = "";

            try
            {
                status = "First Block";
                status = "Third Block Block";
                string batchfile = System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForC2"].ToString();
                if (File.Exists(batchfile))
                {
                    try
                    {
                        status = "Third Block - Sub 1";
                        var proc1 = new Process();
                        proc1.StartInfo.WorkingDirectory = Path.GetDirectoryName(System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForC2"].ToString());
                        proc1.StartInfo.FileName = System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForC2"].ToString();
                        proc1.StartInfo.Arguments = "-v -s -a";
                        status = "Third Block - Sub 2";
                        bool bStart = proc1.Start();
                        status = bStart + " Third Block - Sub 3 ";
                        proc1.WaitForExit();
                        status = bStart + " Third Block - Sub 4 ";
                        var exitCode = proc1.ExitCode;
                        proc1.Close();

                        //File.Copy(System.Configuration.ConfigurationManager.AppSettings["CCDOutputLocation"].ToString(), sOutputLocation);
                    }
                    catch (Exception ex)
                    {
                        //CAP-1942
                        throw new Exception(status + " " + ex.Message + "  " + ex.InnerException, ex);
                    }

                    //using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
                    //{
                    //    status = "Third Block - Sub 1";
                    //    proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString());
                    //    proc.StartInfo.FileName = "@" + System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString();
                    //    status = "Third Block - Sub 2" +" " + proc.StartInfo.WorkingDirectory + " " + proc.StartInfo.FileName;
                    //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "UtilityManager - 4522 - CreateCCDXMLByBatchProcess: status  - " + status + " : Start", DateTime.Now, "0", "frmimageviewer");
                    //    bool bStart = proc.Start();
                    //    status = bStart + " Third Block - Sub 3 " + System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString();
                    //    proc.WaitForExit();
                    //    status = bStart + " Third Block - Sub 4 " + System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString();
                    //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "UtilityManager - 4527 - CreateCCDXMLByBatchProcess: status  - " + status + " : End", DateTime.Now, "0", "frmimageviewer");
                    //}

                    status = "Eighth Block Block";
                    batchfile = System.Configuration.ConfigurationManager.AppSettings["CQMCalculatorExe"].ToString();
                    if (File.Exists(batchfile))
                    {
                        try
                        {
                            status = "Eighth Block - Sub 1";
                            var proc1 = new Process();
                            proc1.StartInfo.WorkingDirectory = Path.GetDirectoryName(System.Configuration.ConfigurationManager.AppSettings["CQMCalculatorExe"].ToString());
                            proc1.StartInfo.FileName = System.Configuration.ConfigurationManager.AppSettings["CQMCalculatorExe"].ToString();
                            proc1.StartInfo.Arguments = "-v -s -a";
                            status = "Eighth Block - Sub 2";
                            bool bStart = proc1.Start();
                            status = bStart + " Eighth Block - Sub 3 ";
                            proc1.WaitForExit();
                            status = bStart + " Eighth Block - Sub 4 ";
                            var exitCode = proc1.ExitCode;
                            proc1.Close();

                            //File.Copy(System.Configuration.ConfigurationManager.AppSettings["CCDOutputLocation"].ToString(), sOutputLocation);
                        }
                        catch (Exception ex)
                        {
                            //CAP-1942
                            throw new Exception(status + " " + ex.Message + "  " + ex.InnerException, ex);
                        }
                    }
                    else
                    {
                        status = "Batch File Not found-FileName:" + batchfile;
                    }
                    status = "Success";
                }
                else
                {
                    status = "Batch File Not found-FileName:" + batchfile;
                }
                return status;

            }
            catch (Exception Ex)
            {
                //CAP-1942
                throw new Exception(status + " " + Ex.Message + "  " + Ex.InnerException, Ex);
            }

        }

        public static IList<object> ReadBlob(ulong EntityID, IList<string> ilstTagName, string sMyXMLType = "")
        {


            string sXMLType = "";

            IList<object> ilstResult = new List<object>();

            if (EntityID == 0)
            {
                return ilstResult;
            }

            IList<object> ilstEntity = new List<object>();
            XmlDocument xmlDoc = new XmlDocument();
            string sXMLContent = string.Empty;
            IList<MapXMLBlob> ilstXMLBlob = new List<MapXMLBlob>();
            ilstXMLBlob = ApplicationObject.ilstMapXMLBlob.Where(a => a.XML_Tag_Name == ilstTagName[0].ToString()).ToList();
            if (ilstXMLBlob.Count > 0)
            {
                if (sMyXMLType != string.Empty)
                {
                    sXMLType = sMyXMLType;
                }
                else
                {
                    sXMLType = ilstXMLBlob[0].Table_Name;
                }
                if (sXMLType == "Blob_Human")
                {
                    HumanBlobManager HumanBlobMngr = new HumanBlobManager();
                    IList<Human_Blob> ilstHumanBlob = HumanBlobMngr.GetHumanBlob(EntityID);
                    if (ilstHumanBlob.Count > 0)
                    {
                        try
                        {
                            sXMLContent = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                            if (sXMLContent.Substring(0, 1) != "<")
                                sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                            xmlDoc.LoadXml(sXMLContent);
                        }
                        catch
                        {
                            throw new Exception("Human XML is invalid");
                        }
                    }
                    else
                    {
                        throw new Exception("Human XML is not found");
                    }

                }
                else if (sXMLType == "Blob_Encounter")
                {
                    EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
                    IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(EntityID);
                    if (ilstEncounterBlob.Count > 0)
                    {
                        try
                        {
                            sXMLContent = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                            if (sXMLContent.Substring(0, 1) != "<")
                                sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                            xmlDoc.LoadXml(sXMLContent);
                        }
                        catch
                        {
                            throw new Exception("Encounter XML is invalid");
                        }
                    }
                    else
                    {
                        throw new Exception("Encounter XML is not found");
                    }
                }


                try
                {
                    XmlNodeList xmlTagName = null;

                    for (int iInputTagCount = 0; iInputTagCount < ilstTagName.Count; iInputTagCount++)
                    {
                        ilstEntity = new List<object>();
                        if (xmlDoc.GetElementsByTagName(ilstTagName[iInputTagCount]) != null && xmlDoc.GetElementsByTagName(ilstTagName[iInputTagCount]).Count > 0)
                        {
                            xmlTagName = xmlDoc.GetElementsByTagName(ilstTagName[iInputTagCount])[0].ChildNodes;
                            if (xmlTagName.Count > 0)
                            {
                                ilstEntity = new List<object>();
                                for (int iXMLTagCount = 0; iXMLTagCount < xmlTagName.Count; iXMLTagCount++)
                                {
                                    string TagName = xmlTagName[iXMLTagCount].Name;
                                    IEnumerable<PropertyInfo> propInfo = null;
                                    object objEntity = null;

                                    if (TagName == "Human")
                                    {
                                        Human objHuman = new Human();
                                        objEntity = (object)objHuman;
                                    }
                                    else if (TagName == "Orders")
                                    {
                                        Orders objOrders = new Orders();
                                        objEntity = (object)objOrders;
                                    }
                                    else
                                    {
                                        XmlSerializer xmlserializer = FillSerializer(TagName);//new XmlSerializer(typeof(ImmunizationHistory));
                                        objEntity = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[iXMLTagCount])) as object;
                                    }

                                    if (objEntity != null)
                                    {
                                        propInfo = from obji in ((object)objEntity).GetType().GetProperties() select obji;

                                        for (int iAttributeCount = 0; iAttributeCount < xmlTagName[iXMLTagCount].Attributes.Count; iAttributeCount++)
                                        {
                                            XmlNode nodevalue = xmlTagName[iXMLTagCount].Attributes[iAttributeCount];
                                            {
                                                foreach (PropertyInfo property in propInfo)
                                                {
                                                    if (property.Name == nodevalue.Name)
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
                                                        {
                                                            ulong ulongValue = 0;
                                                            bool isNumber = ulong.TryParse(nodevalue.Value, out ulongValue);
                                                            if (isNumber == true)
                                                            {
                                                                property.SetValue(objEntity, Convert.ToUInt64(nodevalue.Value), null);
                                                            }
                                                        }
                                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
                                                            property.SetValue(objEntity, Convert.ToString(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            property.SetValue(objEntity, Convert.ToDateTime(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
                                                            property.SetValue(objEntity, Convert.ToInt32(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                                                            property.SetValue(objEntity, Convert.ToDecimal(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "DOUBLE")
                                                            property.SetValue(objEntity, Convert.ToDouble(nodevalue.Value), null);
                                                        else
                                                            property.SetValue(objEntity, nodevalue.Value, null);
                                                    }
                                                }
                                            }

                                        }
                                        ilstEntity.Add(objEntity);
                                    }

                                }

                            }
                            else
                            {
                                ilstResult.Add(null);
                            }
                            ilstResult.Add((object)ilstEntity);
                        }
                        else
                        {
                            ilstResult.Add(null);
                        }

                    }
                }
                catch (Exception ex)
                {
                    //CAP-1942
                    throw new Exception(ex.Message + " " + ex.StackTrace, ex);
                }
            }
            else
            {
                throw new Exception("XML Tag Name is not found in the lookup table");
            }

            return ilstResult;
        }

        public static XmlSerializer FillSerializer(string sEntityName)
        {
            XmlSerializer xmlserializer = null;

            switch (sEntityName)
            {
                case "Encounter":
                    {
                        xmlserializer = new XmlSerializer(typeof(Encounter));
                        break;
                    }
                case "PatientInsuredPlan":
                    {
                        xmlserializer = new XmlSerializer(typeof(PatientInsuredPlan));
                        break;
                    }
                case "ProblemList":
                    {
                        xmlserializer = new XmlSerializer(typeof(ProblemList));
                        break;
                    }
                case "ChiefComplaints":
                    {
                        xmlserializer = new XmlSerializer(typeof(ChiefComplaints));
                        break;
                    }
                case "Healthcare_Questionnaire":
                    {
                        xmlserializer = new XmlSerializer(typeof(Healthcare_Questionnaire));
                        break;
                    }
                case "Test":
                    {
                        xmlserializer = new XmlSerializer(typeof(Test));
                        break;
                    }
                case "PastMedicalHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(PastMedicalHistory));
                        break;
                    }
                case "PastMedicalHistoryMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(PastMedicalHistoryMaster));
                        break;
                    }
                case "SocialHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(SocialHistory));
                        break;
                    }
                case "SocialHistoryMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(SocialHistoryMaster));
                        break;
                    }

                case "SurgicalHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(SurgicalHistory));
                        break;
                    }
                case "SurgicalHistoryMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(SurgicalHistoryMaster));
                        break;
                    }
                case "FamilyHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(FamilyHistory));
                        break;
                    }
                case "FamilyDisease":
                    {
                        xmlserializer = new XmlSerializer(typeof(FamilyDisease));
                        break;
                    }
                case "FamilyHistoryMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(FamilyHistoryMaster));
                        break;
                    }
                case "FamilyDiseaseMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(FamilyDiseaseMaster));
                        break;
                    }
                case "FileManagementIndex":
                    {
                        xmlserializer = new XmlSerializer(typeof(FileManagementIndex));
                        break;
                    }
                case "ImmunizationHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(ImmunizationHistory));
                        break;
                    }
                case "ImmunizationMasterHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(ImmunizationMasterHistory));
                        break;
                    }
                case "NonDrugAllergy":
                    {
                        xmlserializer = new XmlSerializer(typeof(NonDrugAllergy));
                        break;
                    }
                case "NonDrugAllergyMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(NonDrugAllergyMaster));
                        break;
                    }
                case "AdvanceDirective":
                    {
                        xmlserializer = new XmlSerializer(typeof(AdvanceDirective));
                        break;
                    }
                case "AdvanceDirectiveMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(AdvanceDirectiveMaster));
                        break;
                    }
                case "PhysicianPatient":
                    {
                        xmlserializer = new XmlSerializer(typeof(PhysicianPatient));
                        break;
                    }
                case "PhysicianPatientMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(PhysicianPatientMaster));
                        break;
                    }

                case "HospitalizationHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(HospitalizationHistory));
                        break;
                    }
                case "HospitalizationHistoryMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(HospitalizationHistoryMaster));
                        break;
                    }
                case "ROS":
                    {
                        xmlserializer = new XmlSerializer(typeof(ROS));
                        break;
                    }
                case "PatientResults":
                    {
                        xmlserializer = new XmlSerializer(typeof(PatientResults));
                        break;
                    }
                case "Examination":
                    {
                        xmlserializer = new XmlSerializer(typeof(Examination));
                        break;
                    }
                case "Assessment":
                    {
                        xmlserializer = new XmlSerializer(typeof(Assessment));
                        break;
                    }
                case "OrdersSubmit":
                    {
                        xmlserializer = new XmlSerializer(typeof(OrdersSubmit));
                        break;
                    }
                case "Orders":
                    {
                        xmlserializer = new XmlSerializer(typeof(Orders));
                        break;
                    }
                case "OrdersAssessment":
                    {
                        xmlserializer = new XmlSerializer(typeof(OrdersAssessment));
                        break;
                    }
                case "ReferralOrder":
                    {
                        xmlserializer = new XmlSerializer(typeof(ReferralOrder));
                        break;
                    }
                case "ReferralOrdersAssessment":
                    {
                        xmlserializer = new XmlSerializer(typeof(ReferralOrdersAssessment));
                        break;
                    }
                case "Immunization":
                    {
                        xmlserializer = new XmlSerializer(typeof(Immunization));
                        break;
                    }
                case "InHouseProcedure":
                    {
                        xmlserializer = new XmlSerializer(typeof(InHouseProcedure));
                        break;
                    }
                case "EAndMCoding":
                    {
                        xmlserializer = new XmlSerializer(typeof(EAndMCoding));
                        break;
                    }
                case "EandMCodingICD":
                    {
                        xmlserializer = new XmlSerializer(typeof(EandMCodingICD));
                        break;
                    }
                case "TreatmentPlan":
                    {
                        xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
                        break;
                    }
                case "CarePlan":
                    {
                        xmlserializer = new XmlSerializer(typeof(CarePlan));
                        break;
                    }
                case "Documents":
                    {
                        xmlserializer = new XmlSerializer(typeof(Documents));
                        break;
                    }
                case "PreventiveScreen":
                    {
                        xmlserializer = new XmlSerializer(typeof(PreventiveScreen));
                        break;
                    }
                case "GeneralNotes":
                    {
                        xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                        break;
                    }
                case "GeneralNotesROS":
                    {
                        xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                        break;
                    }
                case "GeneralNotesROSGeneralNotes":
                    {
                        xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                        break;
                    }
                case "Rcopia_Allergy":
                    {
                        xmlserializer = new XmlSerializer(typeof(Rcopia_Allergy));
                        break;
                    }
                case "Rcopia_Medication":
                    {
                        xmlserializer = new XmlSerializer(typeof(Rcopia_Medication));
                        break;
                    }
                case "Rcopia_Prescription_List":
                    {
                        xmlserializer = new XmlSerializer(typeof(Rcopia_Prescription_List));
                        break;
                    }
                case "AddendumNotes":
                    {
                        xmlserializer = new XmlSerializer(typeof(AddendumNotes));
                        break;
                    }
                case "Human":
                    {
                        xmlserializer = new XmlSerializer(typeof(Human));
                        break;
                    }
                case "PotentialDiagnosis":
                    {
                        xmlserializer = new XmlSerializer(typeof(PotentialDiagnosis));
                        break;
                    }
                case "GeneralNotesSocialHistoryList":
                    {
                        xmlserializer = new XmlSerializer(typeof(SocialHistory));
                        break;
                    }
                case "SocialHistoryList":
                    {
                        xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                        break;
                    }
                case "SocialHistoryMasterList":
                    {
                        xmlserializer = new XmlSerializer(typeof(SocialHistoryMaster));
                        break;
                    }
            }
            return xmlserializer;
        }

        public static string FillPatientStrip(ulong humanID)
        {
            // Assign PatientStrip Values

            string sPatientStrip = string.Empty;

            IList<string> ilstHumanTag = new List<string>();
            ilstHumanTag.Add("HumanList");

            IList<object> ilstHumanBlobList = new List<object>();
            ilstHumanBlobList = UtilityManager.ReadBlob(humanID, ilstHumanTag);

            Human objFillHuman = new Human();

            if (ilstHumanBlobList != null && ilstHumanBlobList.Count > 0)
            {
                if (ilstHumanBlobList[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobList[0]).Count; iCount++)
                    {
                        objFillHuman = ((Human)((IList<object>)ilstHumanBlobList[0])[iCount]);
                    }
                }
            }
            else
            {
                return null;
            }
            //Jira #Cap123 - Cel phone num included
            //string phoneno = "";

            //if (objFillHuman != null)
            //{

            //    if (objFillHuman.Home_Phone_No.Length == 14)
            //    {
            //        phoneno = objFillHuman.Home_Phone_No;
            //    }
            //    else
            //    {
            //        phoneno = objFillHuman.Cell_Phone_Number;
            //    }

            //}

            string sPatientSex = string.Empty;

            if (objFillHuman != null && objFillHuman.Sex != string.Empty)
            {
                //Jira #CAP-857
                //if (objFillHuman.Sex.Substring(0, 1).ToUpper() == "U")
                //{
                //    //Cap - 596
                //    //sPatientSex = "UNK";
                //    sPatientSex = "UN";
                //}
                if (objFillHuman.Sex.ToUpper() == "UNKNOWN")
                {
                    sPatientSex = "UNK";
                }
                else if (objFillHuman.Sex.ToUpper() == "UNDIFFERENTIATED")
                {
                    sPatientSex = "UN";
                }
                else
                {
                    sPatientSex = objFillHuman.Sex.Substring(0, 1);
                }
            }
            else
            {
                sPatientSex = "";
            }

            string sAcoEligiblePatient = string.Empty;
            sAcoEligiblePatient = objFillHuman.ACO_Is_Eligible_Patient;

            if (objFillHuman != null)
            {
                //Jira #Cap123 - Cel phone num included
                //    sPatientStrip = objFillHuman.Last_Name + "," + objFillHuman.First_Name +
                //"  " + objFillHuman.MI + "  " + objFillHuman.Suffix + "   |   " +
                // objFillHuman.Birth_Date.ToString("dd-MMM-yyyy") + "   |   " +
                //(CalculateAge(objFillHuman.Birth_Date)).ToString() +
                //"  year(s)    |   " + sPatientSex + "   |   Acc #:" + humanID +
                //"   |   " + "Med Rec #:" + objFillHuman.Medical_Record_Number + "   |   " +
                //"Phone #:" + phoneno + "   |   Patient Type:" + objFillHuman.Human_Type + "   |   ";
                sPatientStrip = objFillHuman.Last_Name + "," + objFillHuman.First_Name +
                "  " + objFillHuman.MI + "  " + objFillHuman.Suffix + "   |   " +
                 objFillHuman.Birth_Date.ToString("dd-MMM-yyyy") + "   |   " +
                (CalculateAge(objFillHuman.Birth_Date)).ToString() +
                "  year(s)    |   " + sPatientSex + "   |   Acc #:" + humanID +
                "   |   " + "Med Rec #:" + objFillHuman.Medical_Record_Number + "   |   " +
                "Home Phone #:" + objFillHuman.Home_Phone_No + "  |  Cell Phone #:" + objFillHuman.Cell_Phone_Number + "   |   Patient Type:" + objFillHuman.Human_Type + "   |   ";
            }
            else
            {
                sPatientStrip = " " + "   |" + "|" + "|" + "|" + "|";
            }

            if (objFillHuman.Is_Translator_Required != null && objFillHuman.Is_Translator_Required.ToUpper() == "Y")
            {
                sPatientStrip += objFillHuman.Preferred_Language + " req." + "   |   ";
            }

            if (sAcoEligiblePatient != null && sAcoEligiblePatient != string.Empty && sAcoEligiblePatient != "N")
            {
                sPatientStrip += sAcoEligiblePatient + "   |   ";
            }

            return sPatientStrip;
        }
        //Jira #CAP-64,#CAP-67,#CAP-39 
        public static void RetryExecptionLog(Exception ex, int iTrycount, string sFileName = "")
        {
            string sMsg = string.Empty;
            string sExStackTrace = string.Empty;
            string insertQuery = string.Empty;
            string version = "";
            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

            string[] server = version.Split('|');
            string serverno = "";
            if (server.Length > 1)
                serverno = server[1].Trim();
            if (ex.InnerException != null && ex.InnerException.Message != null)
                sMsg = ex.InnerException.Message + "; FileName: " + sFileName;
            else
                sMsg = ex.Message + "; FileName: " + sFileName;

            if (ex != null && ex.StackTrace != null)
                sExStackTrace = ex.StackTrace;

            insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + iTrycount + " ', '" + serverno + "','" + DateTime.Now + "','" + ClientSession.UserName + "','" + ClientSession.EncounterId + "','" + ClientSession.HumanId + "','" + ClientSession.PhysicianId + "','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";

            if (insertQuery != string.Empty)
            {
                int iReturn = DBConnector.WriteData(insertQuery);
            }
        }

        public static string ReplaceSpecialCharaters(string sXMLContent)
        {

            sXMLContent = sXMLContent.Replace("&#x00;", " ").Replace("&#x01;", " ").Replace("&#x02;", " ").Replace("&#x03;", " ").Replace("&#x04;", " ").Replace("&#x05;", " ")
                .Replace("&#x06;", " ").Replace("&#x07;", " ").Replace("&#x08;", " ").Replace("&#x0B;", " ").Replace("&#x0C;", " ").Replace("&#x0E;", " ")
                .Replace("&#x0F;", " ").Replace("&#x10;", " ").Replace("&#x11;", " ").Replace("&#x12;", " ").Replace("&#x13;", " ").Replace("&#x14;", " ")
                .Replace("&#x15;", " ").Replace("&#x16;", " ").Replace("&#x17;", " ").Replace("&#x18;", " ").Replace("&#x19;", " ").Replace("&#x1A;", " ")
                .Replace("&#x1B;", " ").Replace("&#x1C;", " ").Replace("&#x1D;", " ").Replace("&#x1E;", " ").Replace("&#x1F;", " ").Replace("&#x7F;", " ");
            return sXMLContent;

        }
        //Jira #CAP-344
        public static string PrintSummaryUsingXSLT(string strTransformSource, XmlReader xmlr)
        {
            StringBuilder htmlOutput = new StringBuilder();
            using (TextWriter htmlWriter = new StringWriter(htmlOutput))
            {
                XslCompiledTransform objXSLTransform = new XslCompiledTransform();
                XsltSettings settingsxsl = new XsltSettings(true, false);
                objXSLTransform.Load(strTransformSource, settingsxsl, new XmlUrlResolver());
                objXSLTransform.Transform(xmlr, null, htmlWriter);

                return htmlWriter.ToString();
            }
        }
        //Jira #CAP-344
        public static void PrintPDFUsingXSLT(string sXMLEncounterDoc, string sXMLHumanDoc, string xsltFile, string outputDocument, string sGroup_ID_Log)
        {
            if (File.Exists(outputDocument))
            {
                File.Delete(outputDocument);
            }
            using (XmlTextWriter writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8))
            {
                XslCompiledTransform objXSLTransform = new XslCompiledTransform();
                DataSet ds;
                XmlDataDocument xmlDoc;
                XslCompiledTransform xslTran;
                XmlElement root;
                XPathNavigator nav;
                //XmlTextWriter writer;
                XsltSettings settings = new XsltSettings(true, false);

                ds = new DataSet();
                //ds.ReadXml(xmlDataFile);
                //ds.ReadXml(new XmlTextReader(new StringReader(sXMLEncounterDoc)));
                StringBuilder sb = new StringBuilder();
                sb.Append(sXMLEncounterDoc.ToString().Replace("</notes>", "").Replace("</Modules>", ""));
                if (sXMLHumanDoc != "" && sXMLHumanDoc != string.Empty)
                {
                    string SUB = sXMLHumanDoc.ToString().Substring(0, sXMLHumanDoc.LastIndexOf("?>") + 2);

                    sb.Append(sXMLHumanDoc.ToString().Replace(SUB, "").Replace("<notes>", "").Replace("<Modules>", ""));
                }
                ds.ReadXml(new XmlTextReader(new StringReader(sb.ToString())));

                xmlDoc = new XmlDataDocument(ds);
                // xslTran.Load(xsltFile);
                if (sGroup_ID_Log != null && sGroup_ID_Log != string.Empty)
                {
                    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF XSLT Load : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

                }
                objXSLTransform.Load(xsltFile, settings, new XmlUrlResolver());
                if (sGroup_ID_Log != null && sGroup_ID_Log != string.Empty)
                {
                    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF XSLT Load : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
                }
                root = xmlDoc.DocumentElement;


                nav = root.CreateNavigator();
                //writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8);
                objXSLTransform.Transform(nav, writer);
                writer.Close();
                //writer = null;
                nav = null;
                root = null;
                xmlDoc = null;
                ds = null;
            }
        }

        public Boolean LoadBlobHumanXML(ulong ulHumanID, ulong ulEncounterID, IList<Encounter_Blob> ilstEncounterBlob, string sTabMode, out string sXMLHumanDoc, string sIsPhone_Encounter = "N")
        {
            Boolean bAlert = false;
            sXMLHumanDoc = string.Empty;
            //if (sTabMode == "true")
            //{
            WFObjectManager wfObjMngr = new WFObjectManager();
            WFObject DocumentationWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "DOCUMENTATION");

            if (DocumentationWfObject != null && DocumentationWfObject.Current_Process == string.Empty)
            {
                DocumentationWfObject = wfObjMngr.GetWfObjArchiveByObjectSystemId(ulEncounterID, "DOCUMENTATION");
            }

            // jira cap-499
            if (DocumentationWfObject.Current_Process == "DOCUMENT_COMPLETE" || sIsPhone_Encounter.ToUpper() == "Y")
            {
                if (ilstEncounterBlob != null && ilstEncounterBlob.Count > 0 && ilstEncounterBlob[0].Human_XML != null)
                {
                    sXMLHumanDoc = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Human_XML);
                }
                else
                {
                    bAlert = true;
                }
            }
            else
            {
                IList<Human_Blob> ilstHumanBlob = new List<Human_Blob>();
                HumanBlobManager HumanBlobMngr = new HumanBlobManager();
                ilstHumanBlob = HumanBlobMngr.GetHumanBlob(Convert.ToUInt64(ulHumanID));
                if (ilstHumanBlob != null && ilstHumanBlob.Count > 0 && ilstHumanBlob[0].Human_XML != null)
                {
                    sXMLHumanDoc = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                }
            }
            //}
            //else
            //{
            //    IList<Human_Blob> ilstHumanBlob = new List<Human_Blob>();
            //    HumanBlobManager HumanBlobMngr = new HumanBlobManager();
            //    ilstHumanBlob = HumanBlobMngr.GetHumanBlob(Convert.ToUInt64(ulHumanID));
            //    if (ilstHumanBlob != null && ilstHumanBlob.Count > 0 && ilstHumanBlob[0].Human_XML != null)
            //    {
            //        sXMLHumanDoc = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
            //    }

            //}
            return bAlert;
        }

        //Jira CAP-1990
        //public static string IsAkidoEncounter(string sEncounterID, out string sExMessage)
        public static string IsAkidoEncounter(string sEncounterID, out string sExMessage, out string sStatus)
        {
            sStatus = "";
            sExMessage = "";
            string bIsAkidoEncounter = "false";
            //Jira CAP-1379
            int iRetryCount = 0;

        retry:
            try
            {
                iRetryCount = iRetryCount + 1;

                var myUri = new Uri(System.Configuration.ConfigurationSettings.AppSettings["AkidoNoteStatusURL"].ToString().Replace("[CapellaEncounterID]", sEncounterID));
                string AccessToken = System.Configuration.ConfigurationSettings.AppSettings["AkidoNoteStatusURLToken"].ToString();
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();

                var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                var json = myStreamReader.ReadToEnd();
                responseStream.Close();
                myWebResponse.Close();

                if (json.ToString() != "[]")
                {
                    bIsAkidoEncounter = "true";
                    //Jira CAP-1990
                    string sPJason = json.Substring(1, json.Length - 2);
                    var jsonObject = JObject.Parse(sPJason);
                    sStatus = (string)jsonObject["status"];

                }
            }
            catch (Exception ex)
            {
                //Jira CAP-1379
                //bIsAkidoEncounter = "Exception";
                //sExMessage = ex.Message;
                //Console.WriteLine(ex.ToString());

                //Jira CAP-1379
                if (iRetryCount < 3)
                {
                    Console.WriteLine("Retrying Count : " + iRetryCount + " -> " + ex.ToString());
                    System.Threading.Thread.Sleep(new TimeSpan(0, 0, 2));
                    goto retry;
                }
                else
                {
                    bIsAkidoEncounter = "Exception";
                    sExMessage = ex.Message;
                    Console.WriteLine(ex.ToString());
                }

            }

            return bIsAkidoEncounter;
        }
        public static string IsCapellaEncounter(string sEncounterXml, ulong ulEncounterID)
        {
            string sCapellaEncounter = string.Empty;
            if (sEncounterXml != string.Empty)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(sEncounterXml);


                sCapellaEncounter = xmlDoc.SelectSingleNode("notes/Modules/EncounterList/Encounter").Attributes.GetNamedItem("Encounter_Provider_Signed_Date").Value;
            }
            else
            {
                EncounterManager encounterManager = new EncounterManager();
                IList<Encounter> ilstEncounter = new List<Encounter>();
                ilstEncounter = encounterManager.GetEncounterByEncounterIDIncludeArc(ulEncounterID);
                if (ilstEncounter.Count > 0)
                {

                    sCapellaEncounter = ilstEncounter[0].Encounter_Provider_Signed_Date.ToString("yyyy-MM-dd hh:mm:ss");
                }
            }

            if (sCapellaEncounter != string.Empty && !sCapellaEncounter.Contains("0001-01-01"))
            {
                sCapellaEncounter = "Y";
            }
            return sCapellaEncounter;
        }
        //CAP-1987
        public static string IsAkidoInterpretationNote(string sOrderSubmitID, out string sExMessage, out string sStatus)
        {
            sStatus = "";
            sExMessage = "";
            string bIsAkidoInterpretationNote = "false";
            //Jira CAP-1379
            int iRetryCount = 0;

        retry:
            try
            {
                iRetryCount = iRetryCount + 1;

                var myUri = new Uri(System.Configuration.ConfigurationSettings.AppSettings["AkidoInterpretationNoteStatusURL"].ToString().Replace("[CapellaResourceID]", sOrderSubmitID).Replace("[CapellaResourceType]", "capella_order_submit_id"));
                string AccessToken = System.Configuration.ConfigurationSettings.AppSettings["AkidoInterpretationNoteStatusURLToken"].ToString();
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();

                var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                var json = myStreamReader.ReadToEnd();
                responseStream.Close();
                myWebResponse.Close();

                if (json.ToString() != "[]" && json.ToString().ToUpper().Contains("STATUS\":\"SIGNED"))
                {
                    bIsAkidoInterpretationNote = "true";
                    //Jira CAP-1990
                    string sPJason = json.Substring(1, json.Length - 2);
                    var jsonObject = JObject.Parse(sPJason);
                    sStatus = (string)jsonObject["status"];

                }
            }
            catch (Exception ex)
            {
                //Jira CAP-1379
                //bIsAkidoEncounter = "Exception";
                //sExMessage = ex.Message;
                //Console.WriteLine(ex.ToString());

                //Jira CAP-1379
                if (iRetryCount < 3)
                {
                    Console.WriteLine("Retrying Count : " + iRetryCount + " -> " + ex.ToString());
                    System.Threading.Thread.Sleep(new TimeSpan(0, 0, 2));
                    goto retry;
                }
                else
                {
                    bIsAkidoInterpretationNote = "Exception";
                    sExMessage = ex.Message;
                    Console.WriteLine(ex.ToString());
                }

            }

            return bIsAkidoInterpretationNote;
        }
        public static void IsAkidoCDC(string sHumanID, string sEncounterID, string sTransactionBy, string sTransactionDateTime)
        {
            string bIsAkidoEncounter = "false";
            //Jira CAP-1379
            int iRetryCount = 0;

        retry:
            try
            {
                iRetryCount = iRetryCount + 1;

                string akidoNoteCDCURL = System.Configuration.ConfigurationSettings.AppSettings["AkidoNoteCDCURL"].ToString();
                akidoNoteCDCURL = akidoNoteCDCURL.Replace("[CapellaHumanID]", sHumanID).Replace("[CapellaEncounterID]", sEncounterID).Replace("[CapellaTransactionBy]", sTransactionBy).Replace("[CapellaTransactionDateTime]", sTransactionDateTime);
                var myUri = new Uri(akidoNoteCDCURL);
                string AccessToken = System.Configuration.ConfigurationSettings.AppSettings["AkidoNoteCDCURLToken"].ToString();
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();

                var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                var json = myStreamReader.ReadToEnd();
                responseStream.Close();
                myWebResponse.Close();

                //if (json.ToString() != "[]")
                //{
                //    bIsAkidoEncounter = "true";
                //    //Jira CAP-1990
                //    string sPJason = json.Substring(1, json.Length - 2);
                //    var jsonObject = JObject.Parse(sPJason);
                //}
            }
            catch (Exception ex)
            {
                //Jira CAP-1379
                //bIsAkidoEncounter = "Exception";
                //sExMessage = ex.Message;
                //Console.WriteLine(ex.ToString());

                //Jira CAP-1379
                //    if (iRetryCount < 3)
                //    {
                //        Console.WriteLine("Retrying Count : " + iRetryCount + " -> " + ex.ToString());
                //        System.Threading.Thread.Sleep(new TimeSpan(0, 0, 2));
                //        goto retry;
                //    }
                //    else
                //    {
                //        bIsAkidoEncounter = "Exception";
                //        Console.WriteLine(ex.ToString());
                //    }

            }

            //return bIsAkidoEncounter;
        }
        public static bool CheckFileNotFoundException(Exception ex, out string sErrorMessage)
        {
            sErrorMessage = string.Empty;
            bool bCheckFileNotFoundException = false;
            if (ex.Message != null)
            {
                sErrorMessage = "MESSAGE: " + ex.Message + "\\n\\n";
            }
            if (ex.InnerException != null && ex.InnerException.Message != null)
            {
                sErrorMessage += "INNER EXCEPTION: " + ex.InnerException.Message + "\\n\\n";
            }
            if (ex.StackTrace != null)
            {
                sErrorMessage += "STACK TRACE: " + ex.StackTrace.ToString();
            }
            sErrorMessage = sErrorMessage.Replace("'", "");
            sErrorMessage = sErrorMessage.Replace(System.Environment.NewLine, "");
            bCheckFileNotFoundException = (sErrorMessage.Contains("Could not find file") ||
                                           sErrorMessage.Contains("Access to the path") ||
                                           sErrorMessage.Contains("Specific file is not present in the location") ||
                                           sErrorMessage.Contains("The process cannot access the file") ||
                                           sErrorMessage.Contains("Could not find a part of the path") ||
                                           sErrorMessage.Contains("pdf not found as file or resource") ||
                                           sErrorMessage.Contains("Invalid page number") ||
                                           sErrorMessage.Contains("trailer not found"));
            return bCheckFileNotFoundException;

        }
        public string ReplaceSpecialCharaterInFileName(string FileName)
        {
            string sFilename = string.Empty;
            sFilename = FileName.Replace("#", "").Replace("%", "").Replace("&", "").Replace("{", "").Replace("}", "")
                .Replace("<", "").Replace(">", "").Replace("*", "").Replace("?", "").Replace(" ", "").Replace("$", "")
                .Replace("!", "").Replace("'", "").Replace(":", "").Replace("@", "").Replace("+", "").Replace("`", "")
                .Replace("|", "").Replace("=", "").Replace("__", "_").Replace("~", "").Replace("^", "").Replace("\"", "");

            return sFilename;
        }
    }
}

