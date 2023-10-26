using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DTO;
using System.Collections;
using Acurus.Capella.Core.DomainObjects;
using System.Web;
using Telerik.Web;
using Telerik.Web.UI.Grid;
using Telerik.Web.UI;
using System.Data;


namespace Acurus.Capella.UI
{

    public class ClientSession
    {
        # region Private Constants

        private const string userPermissionDTO = "ClientUserPermissionDTO";
        private const string userName = "ClientUserName";
        //private const string personName = "ClientPersonName";
        private const string userRole = "ClientUserRole";
        private const string facilityName = "ClientFacilityName";
        private const string userCurrentProcess = "ClientUserCurrentProcess";
        private const string userCurrentOwner = "ClientUserCurrentOwner";
        private const string userCurrentList = "ClientUserCurrentList";
        private const string listProcEncounter = "ClientListProcEncounter";
        private const string userPermission = "ClientUserPermission";
        private const string checkUser = "ClientCheckUser";
        //private const string defaultNoofDays = "ClientDefaultNoofDays";
        private const string currentObjectType = "ClientCurrentObjectType";
        private const string encounterId = "ClientEncounterId";
        private const string physicianId = "ClientPhysicianId";
        private const string humanId = "ClientHumanId";
        private const string fillEncounterandWFObject = "ClientFillEncounterandWFObject";
        private const string patientPaneList = "ClientPatientPaneList";
        private const string physiciandetails = "Clientphysiciandetails";
        private const string rCopiaUserName = "ClientRCopiaUserName";
        private const string is_RCopia_Notification_Required = "ClientIs_RCopia_Notification_Required";
        private const string fillPatientChart = "ClientFillPatientChart";
        private const string selectedencounterid = "ClientSelectedencounterid";
        private const string physicianUserName = "ClientPhysicianUserName";
        private const string pFSHVerified = "ClientbPFSHVerified";
        private const string isDirtySocialHistory = "ClientIsDirtySocialHistory";
        public static bool processCheck = false;
        private const string currentPhysicianId = "ClientCurrentPhysicianId";
        private const string sWindowlst = "ClientWindowList";
        private const string selectedfrom = "ClientSelectedFrom";
        private const string sSelectedTab = "ClientSelectedTab";
        // private const string _currentAddendumID = "ClientCurrentAddendumId";
        private const string _UniversalTime = "ClientUniversalTime";
        private const string _LocalOffSetTime = "ClientLocalOffSetTime";
        private const string _LocalDate = "ClientLocalDate";
        private const string _LocalTime = "ClientLocalTime";
        private const string _RequiredForms_ReturnList = "RequiredForms_ReturnList";

        //FrmSummaryofCAre
        private const string _Save_Summary = "Save_Summary";
        //private const string _Load_Summary_PDF = "Load_Summary_PDF";
        public static bool bAsyncError = false;
        private const string summaryList = "ClientSummaryList";
        private const string _PatientPane = "ClientPatientPane";
        private const string _SavedSession = "ClientSavedSession";
        private const string _CDSNotificationRule = "ClientCDSNotificationRule";
        private const string _NotificationUserLookup = "ClientNotificationUserLookup";
        private const string _LegalOrg = "ClientLegalOrg";
        private const string _UserCarrier = "ClientUserCarrier";
        private const string _bFollows_DST = "ClientbFollows_DST";
        private const string _Is_All_Facilities = "ClientIs_All_Facilities";

        // private const string _NotificationCount = "ClientNotificationCount";//BugID:47780
        // public static bool bIsMandatoryNotifPresent = false;
        # endregion


        #region Public Methods
        public static void FlushSession()
        {




            for (int i = 0; i < HttpContext.Current.Session.Count; i++)
            {
                if (HttpContext.Current.Session.Keys[i].ToString().StartsWith("Client") == false)
                {
                    HttpContext.Current.Session[i] = null;
                    HttpContext.Current.Session.Remove(HttpContext.Current.Session.Keys[i]);
                }
            }
        }
        #endregion

        //>>>>>>> 1.27.4.4
        # region Public Properties

        public static UserPermissionDTO UserPermissionDTO
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session!=null ? (HttpContext.Current.Session[userPermissionDTO] != null ? (UserPermissionDTO)HttpContext.Current.Session[userPermissionDTO] : new UserPermissionDTO()) : new UserPermissionDTO()) : new UserPermissionDTO();
            }

            set
            {
                HttpContext.Current.Session[userPermissionDTO] = value;
            }
        }


        public static IList<CDSRuleMaster> CDSNotificationRule
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[_CDSNotificationRule] != null ? (IList<CDSRuleMaster>)HttpContext.Current.Session[_CDSNotificationRule] : new List<CDSRuleMaster>()) : new List<CDSRuleMaster>()) : new List<CDSRuleMaster>();
            }

            set
            {
                HttpContext.Current.Session[_CDSNotificationRule] = value;
            }
        }

        public static IList<UserLookup> NotificationUserLookup
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[_NotificationUserLookup] != null ? (IList<UserLookup>)HttpContext.Current.Session[_NotificationUserLookup] : new List<UserLookup>()) : new List<UserLookup>() ) : new List<UserLookup>();
            }

            set
            {
                HttpContext.Current.Session[_NotificationUserLookup] = value;
            }
        }

        public static string UserName
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[userName] != null ? Convert.ToString(HttpContext.Current.Session[userName]) : string.Empty) : string.Empty) : string.Empty;

            }

            set
            {
                HttpContext.Current.Session[userName] = value;
            }
        }

        public static string PhysicianUserName
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[physicianUserName] != null ? (string)HttpContext.Current.Session[physicianUserName] : string.Empty) : string.Empty ) : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[physicianUserName] = value;
            }
        }

        //public static string PersonName
        //{
        //    get
        //    {
        //        return (string)HttpContext.Current.Session[personName] : string.Empty;
        //    }

        //    set
        //    {
        //        HttpContext.Current.Session[personName] = value;
        //    }
        //}

        public static string UserRole
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[userRole] != null ? (string)HttpContext.Current.Session[userRole] : string.Empty) : string.Empty ) : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[userRole] = value;
            }
        }

        public static string FacilityName
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[facilityName] != null ? (string)HttpContext.Current.Session[facilityName] : string.Empty) : string.Empty) : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[facilityName] = value;
            }
        }

        public static string UserCurrentProcess
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[userCurrentProcess] != null ? (string)HttpContext.Current.Session[userCurrentProcess] : string.Empty) : string.Empty ) : string.Empty;

            }

            set
            {
                HttpContext.Current.Session[userCurrentProcess] = value;
            }
        }

        public static string UserCurrentOwner
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[userCurrentOwner] != null ? (string)HttpContext.Current.Session[userCurrentOwner] : string.Empty) : string.Empty) : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[userCurrentOwner] = value;
            }
        }

        public static ArrayList UserCurrentList
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[userCurrentList] != null ? (ArrayList)HttpContext.Current.Session[userCurrentList] : new ArrayList()) : new ArrayList()) : new ArrayList();
            }

            set
            {
                HttpContext.Current.Session[userCurrentList] = value;
            }
        }



        //public static ArrayList ListProcEncounter
        //{
        //    get
        //    {
        //        return (ArrayList)HttpContext.Current.Session[listProcEncounter] : new ArrayList();
        //    }

        //    set
        //    {
        //        HttpContext.Current.Session[listProcEncounter] = value;
        //    }
        //}

        public static string UserPermission
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[userPermission] != null ? (string)HttpContext.Current.Session[userPermission] : string.Empty) : string.Empty) : string.Empty; 

            }

            set
            {
                HttpContext.Current.Session[userPermission] = value;
            }
        }

        public static bool CheckUser
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[checkUser] != null ? (bool)HttpContext.Current.Session[checkUser] : false) : false) : false;
            }

            set
            {
                HttpContext.Current.Session[checkUser] = value;
            }
        }

        //public static int DefaultNoofDays
        //{
        //    get
        //    {
        //        return (int?)HttpContext.Current.Session[defaultNoofDays] : 0;
        //    }

        //    set
        //    {
        //        HttpContext.Current.Session[defaultNoofDays] = value;
        //    }
        //}

        public static string CurrentObjectType
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[currentObjectType] != null ? (string)HttpContext.Current.Session[currentObjectType] : string.Empty) : string.Empty) : string.Empty;

            }

            set
            {
                HttpContext.Current.Session[currentObjectType] = value;
            }
        }

        public static ulong EncounterId
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[encounterId] != null ? (ulong)HttpContext.Current.Session[encounterId] : 0) : 0) : 0;
            }

            set
            {
                HttpContext.Current.Session[encounterId] = value;
            }
        }

        public static ulong Selectedencounterid
        {
            get
            {

                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[selectedencounterid] != null ? (ulong)HttpContext.Current.Session[selectedencounterid] : 0) : 0) : 0;
            }

            set
            {
                HttpContext.Current.Session[selectedencounterid] = value;
            }
        }

        public static string SelectedFrom
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[selectedfrom] != null ? (string)HttpContext.Current.Session[selectedfrom] : string.Empty) : string.Empty) : string.Empty;

            }

            set
            {
                HttpContext.Current.Session[selectedfrom] = value;
            }
        }



        public static ulong PhysicianId
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[physicianId] != null ? (ulong)HttpContext.Current.Session[physicianId] : 0) : 0) : 0;

            }
            set
            {
                HttpContext.Current.Session[physicianId] = value;
            }
        }

        public static ulong HumanId
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[humanId] != null ? (ulong)HttpContext.Current.Session[humanId] : 0) : 0) : 0;
            }

            set
            {
                HttpContext.Current.Session[humanId] = value;
            }
        }

        public static FillEncounterandWFObject FillEncounterandWFObject
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[fillEncounterandWFObject] != null ? (FillEncounterandWFObject)HttpContext.Current.Session[fillEncounterandWFObject] : new FillEncounterandWFObject()) : new FillEncounterandWFObject()) : new FillEncounterandWFObject();
            }

            set
            {
                HttpContext.Current.Session[fillEncounterandWFObject] = value;
            }
        }

        public static IList<PatientPane> PatientPaneList
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[patientPaneList] != null ? (IList<PatientPane>)HttpContext.Current.Session[patientPaneList] : new List<PatientPane>()) : new List<PatientPane>()) : new List<PatientPane>();
            }

            set
            {
                HttpContext.Current.Session[patientPaneList] = value;
            }
        }

        public static FillPatientChart FillPatientChart
        {
            get
            { 
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[fillPatientChart] != null ? (FillPatientChart)HttpContext.Current.Session[fillPatientChart] : new FillPatientChart()) : new FillPatientChart()) : new FillPatientChart();
            }

            set
            {
                HttpContext.Current.Session[fillPatientChart] = value;
            }
        }

        public static string RCopiaUserName
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[rCopiaUserName] != null ? (string)HttpContext.Current.Session[rCopiaUserName] : string.Empty) : string.Empty) : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[rCopiaUserName] = value;
            }
        }

        public static string Is_RCopia_Notification_Required
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[is_RCopia_Notification_Required] != null ? (string)HttpContext.Current.Session[is_RCopia_Notification_Required] : string.Empty) : string.Empty) : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[is_RCopia_Notification_Required] = value;
            }
        }

        public static bool bPFSHVerified
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ?  (HttpContext.Current.Session[pFSHVerified] != null ? (bool)HttpContext.Current.Session[pFSHVerified] : false) : false ) :false;
            }

            set
            {
                HttpContext.Current.Session[pFSHVerified] = value;
            }
        }

        public static bool IsDirtySocialHistory
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[isDirtySocialHistory] != null ? (bool)HttpContext.Current.Session[isDirtySocialHistory] : false) : false ) :false;
            }

            set
            {
                HttpContext.Current.Session[isDirtySocialHistory] = value;
            }
        }

        public static string UniversalTime
        {

            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[_UniversalTime] != null ? (string)HttpContext.Current.Session[_UniversalTime] : string.Empty) : string.Empty) : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[_UniversalTime] = value;
            }
        }

        public static string LocalOffSetTime
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[_LocalOffSetTime] != null ? (string)HttpContext.Current.Session[_LocalOffSetTime] : string.Empty) : string.Empty) : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[_LocalOffSetTime] = value;
            }
        }

        public static string LocalDate
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[_LocalDate] != null ? (string)HttpContext.Current.Session[_LocalDate] : string.Empty) : string.Empty) : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[_LocalDate] = value;
            }

        }


        public static string LocalTime
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[_LocalTime] != null ? (string)HttpContext.Current.Session[_LocalTime] : string.Empty) : string.Empty) : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[_LocalTime] = value;
            }
        }

        //public static IList<OrdersRequiredForms> RequiredForms_ReturnList
        //{
        //    get
        //    {
        //        return (IList<OrdersRequiredForms>)HttpContext.Current.Session[_RequiredForms_ReturnList] : new List<OrdersRequiredForms>();
        //    }

        //    set
        //    {
        //        HttpContext.Current.Session[_RequiredForms_ReturnList] = value;
        //    }
        //}

        public static bool Save_Summary
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[_Save_Summary] != null ? (bool)HttpContext.Current.Session[_Save_Summary] : false) : false) : false;
            }

            set
            {
                HttpContext.Current.Session[_Save_Summary] = value;
            }
        }
        //public static bool Load_Summary_PDF
        //{
        //    get
        //    {
        //        return (bool?)HttpContext.Current.Session[_Load_Summary_PDF] : false;
        //    }

        //    set
        //    {
        //        HttpContext.Current.Session[_Load_Summary_PDF] = value;
        //    }
        //}
        public static ArrayList WindowList
        {
            get
            {

                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[sWindowlst] != null ? (ArrayList)HttpContext.Current.Session[sWindowlst] : new ArrayList()) : new ArrayList()) : new ArrayList();
            }

            set
            {
                HttpContext.Current.Session[sWindowlst] = value;
            }
        }

        public static ulong CurrentPhysicianId
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[currentPhysicianId] != null ? (ulong)HttpContext.Current.Session[currentPhysicianId] : 0) : 0 ) : 0;
            }

            set
            {
                HttpContext.Current.Session[currentPhysicianId] = value;
            }
        }

        public static string SetSelectedTab
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[sSelectedTab] != null ? (string)HttpContext.Current.Session[sSelectedTab] : string.Empty) : string.Empty) : string.Empty;

            }

            set
            {
                HttpContext.Current.Session[sSelectedTab] = value;
            }
        }

        public static string SummaryList
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[summaryList] != null ? (string)HttpContext.Current.Session[summaryList] : string.Empty) : string.Empty) : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[summaryList] = value;
            }
        }
        public static string SavedSession
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[_SavedSession] != null ? (string)HttpContext.Current.Session[_SavedSession] : string.Empty) : string.Empty) : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[_SavedSession] = value;
            }
        }
        public static string PatientPane
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[_PatientPane] != null ? (string)HttpContext.Current.Session[_PatientPane] : string.Empty) : string.Empty) : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[_PatientPane] = value;
            }
        }

        //public static ulong currAddendumID
        //{
        //    get
        //    {
        //        return (ulong?)HttpContext.Current.Session[_currentAddendumID] : 0;
        //    }

        //    set
        //    {
        //        HttpContext.Current.Session[_currentAddendumID] = value;
        //    }
        //}
        //public static string NotificationCount
        //{
        //    get
        //    {
        //        return (string)HttpContext.Current.Session[_NotificationCount] : string.Empty;
        //    }

        //    set
        //    {
        //        HttpContext.Current.Session[_NotificationCount] = value;
        //    }
        //}

        public static IList<PhysicianLibrary> PhysicainDetails
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[physiciandetails] != null ? (IList<PhysicianLibrary>)HttpContext.Current.Session[physiciandetails] : new List<PhysicianLibrary>()) : new List<PhysicianLibrary>()) : new List<PhysicianLibrary>();

            }

            set
            {
                HttpContext.Current.Session[physiciandetails] = value;
            }
        }

        public static string LegalOrg
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[_LegalOrg] != null ? (string)HttpContext.Current.Session[_LegalOrg] : string.Empty) : string.Empty ) : string.Empty;

            }

            set
            {
                HttpContext.Current.Session[_LegalOrg] = value;
            }
        }

        public static string UserCarrier
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[_UserCarrier] != null ? (string)HttpContext.Current.Session[_UserCarrier] : string.Empty) : string.Empty ) : string.Empty;

            }

            set
            {
                HttpContext.Current.Session[_UserCarrier] = value;
            }
        }

        public static bool bFollows_DST
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[_bFollows_DST] != null ? (bool)HttpContext.Current.Session[_bFollows_DST] : false) : false ) : false;

            }

            set
            {
                HttpContext.Current.Session[_bFollows_DST] = value;
            }
        }

        public static string Is_All_Facilities
        {
            get
            {
                return HttpContext.Current != null ? (HttpContext.Current.Session != null ? (HttpContext.Current.Session[_Is_All_Facilities] != null ? (string)HttpContext.Current.Session[_Is_All_Facilities] : string.Empty) : string.Empty) : string.Empty;

            }

            set
            {
                HttpContext.Current.Session[_Is_All_Facilities] = value;
            }
        }
        # endregion
    }
}
