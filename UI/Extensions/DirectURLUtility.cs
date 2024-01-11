using Acurus.Capella.DataAccess.ManagerObjects;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Acurus.Capella.UI.Extensions
{
    public class DirectURLUtility
    {
        public string GetServerRedirectURLByDirectURL(string redirecturl, string Default_Server)
        {

            //CAP-1167
            var serverRedirectUrl = Default_Server;
            if (!string.IsNullOrEmpty(redirecturl))
            {
                var returnURL = HttpUtility.UrlDecode(redirecturl);
                var defaultServerHost = new Uri(Default_Server);
                var returnUrlHost = new Uri(returnURL);
                //CAP - 1306 & 1311
                if (defaultServerHost.Authority == returnUrlHost.Authority)
                {
                    serverRedirectUrl = Default_Server + "?redirecturl=" + HttpUtility.UrlEncode(returnURL);
                }
                else
                {
                    var modifiedRedirectUrl = HttpUtility.UrlEncode(defaultServerHost.Scheme + "://" + defaultServerHost.Authority + returnUrlHost.PathAndQuery);
                    serverRedirectUrl = Default_Server + "?redirecturl=" + modifiedRedirectUrl;
                }
            }

            return serverRedirectUrl;
        }

        public static bool IsValidRedirectUrlForLogin(string currentURL)
        {
            var encounterUrlPattern = @"^https?://[^/]+/frmPatientChart\.aspx\?EncounterID=\d+$";
            var humanUrlPattern = @"^https?://[^/]+/frmPatientChart\.aspx\?(?:HumanID=\d+)?(&ScreenMode=Menu)?(&openingfrom=Menu)?(&ScreenName=ERX)?(&ScreenName=CreateOrder)?(&ScreenName=OrderManagement)?(&IsDirectURL=Y)?$";

            if (Regex.IsMatch(currentURL, humanUrlPattern) || Regex.IsMatch(currentURL, encounterUrlPattern))
            {
                return true;
            }

            return false;
        }

        public static bool IsValidEncounterUrl(string currentURL)
        {
            var urlPattern = @"^https?://[^/]+/frmPatientChart\.aspx\?EncounterID=\d+$";

            if (Regex.IsMatch(currentURL, urlPattern))
            {
                return true;
            }

            return false;
        }
        //CAP-1511
        public static bool IsValidEncounterTabUrl(string currentURL)
        {
            var urlPattern = @"^https?://[^/]+/frmPatientChart\.aspx\?(EncounterID=\d+)?(&Screen=\w+)$";

            if (Regex.IsMatch(currentURL, urlPattern))
            {
                return true;
            }

            return false;
        }
        public static bool IsValidLegalOrg(string user_name, ulong humanId)
        {
            HumanManager humanManager = new HumanManager();
            UserManager userManager = new UserManager();
            var humanDetails = humanManager.GetPatientDetailsUsingPatientInformattion(humanId);
            var user = userManager.GetUser(user_name);

            var humanLegalOrg = humanDetails?.FirstOrDefault()?.Legal_Org ?? string.Empty;
            var userLegalOrg = user?.FirstOrDefault()?.Legal_Org ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(humanLegalOrg) && !string.IsNullOrWhiteSpace(userLegalOrg) && humanLegalOrg == userLegalOrg)
            {
                return true;
            }

            return false;
        }

        public static bool IsScnMenuEnabled(string user_name, string scn_name)
        {
            UserScnTabManager userScnTabManager = new UserScnTabManager();

            IDictionary<string, ulong> scn_tab_dict = new Dictionary<string, ulong>()
            {
                {"ERX", 5400 },
                {"CREATEORDER", 8507},
                {"ORDERMANAGEMENT", 70900}
            };

            var currentScreen = scn_tab_dict.FirstOrDefault(x => x.Key.Equals(scn_name, StringComparison.InvariantCultureIgnoreCase));

            var scnTab_Id = currentScreen.Key != null ? currentScreen.Value : 0;

            var user_Scn_Tabs = userScnTabManager.GetUserScreenTabByID(scnTab_Id, user_name);

            if ((user_Scn_Tabs != null && user_Scn_Tabs.Count > 0) || scnTab_Id == 0)
            {
                return true;
            }

            return false;
        }

        //CAP-1511
        public static (string,string) GetCurrentTabIdByTabName(string tabName)
        {
            var currentTabId = string.Empty;
            var currentChildTabId = string.Empty;
            switch (tabName)
            {
                //case "SCREENING":
                //    currentTabId = "tabStripEncounter_tbQuestion";
                //    break;
                //case "PFSH":
                //    currentTabId = "tabStripEncounter_tbPFSH";
                //    break;
                //case "ROS":
                //    currentTabId = "tabStripEncounter_tbROS";
                //    break;
                case "VITALS":
                    currentTabId = "tabStripEncounter_tbVitals";
                    break;
                //case "EXAM":
                //    currentTabId = "tabStripEncounter_tbExam";
                //    break;
                //case "TEST":
                //    currentTabId = "tabStripEncounter_tabTest";
                //    break;
                case "ASSESSMENT":
                    currentTabId = "tabStripEncounter_tbAssessment";
                    break;
                //case "ORDERS":
                //    currentTabId = "tabStripEncounter_tbOrders";
                //    break;
                case "ERX":
                    currentTabId = "tabStripEncounter_tbEPrescription";
                    break;
                //case "PLAN":
                //    currentTabId = "tabStripEncounter_tbPlan";
                //    break;
                case "EANDM":
                    currentTabId = "tabStripEncounter_tbEandM";
                    break;
                //case "GENERALPLAN":
                //    currentTabId = "tabStripEncounter_tbPlan";
                //    currentChildTabId = "tabPlan_tbGeneralPlan";
                //    break;
                //case "INDIVIDUALCAREPLAN":
                //    currentTabId = "tabStripEncounter_tbPlan";
                //    currentChildTabId = "tabPlan_tbIndividualCarePlan";
                //    break;
                //case "PREVENTIVESCREENPLAN":
                //    currentTabId = "tabStripEncounter_tbPlan";
                //    currentChildTabId = "tabPlan_tbPreventiveScreen";
                //    break;
                case "DIAGNOSTICORDER":
                    currentTabId = "tabStripEncounter_tbOrders";
                    currentChildTabId = "tabOrder_tbDiagnosticOrder";
                    break;
                case "REFERRALORDER":
                    currentTabId = "tabStripEncounter_tbOrders";
                    currentChildTabId = "tabOrder_tbReferralOrder";
                    break;
                case "IMMUNIZATION":
                    currentTabId = "tabStripEncounter_tbOrders";
                    currentChildTabId = "tabOrder_tbImmunization";
                    break;
                case "PROCEDURE":
                    currentTabId = "tabStripEncounter_tbOrders";
                    currentChildTabId = "tabOrder_tbProcedure";
                    break;

                default:
                    currentTabId = "tabStripEncounter_tbCCHPI";
                    break;
            }

            return (currentTabId, currentChildTabId);
        }
    }
}