using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using Acurus.Capella.Core;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Web;
using System.Diagnostics;
using System.Xml.Linq;
using System.Threading;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Acurus.Capella.DataAccess.ManagerObjects
{

    public partial interface IEncounterManager : IManagerBase<Encounter, ulong>
    {
        IList<Encounter> GetEncounterByEncounterID(ulong EncounterID);
        IList<Encounter> GetEncounterByEncounterIDIncludeArchive(ulong EncounterID);
        IList<Encounter> GetAllEncounterList();
        //ulong CreateEncounter(Encounter EncounterRecord, WFObject wfObjectRecord, string MACAddress, object[] IsCMGOrders);
        IList<Encounter> UpdateEncounter(Encounter EncounterRecord, string MACAddress, object[] IsCMGOrders);
        FillPatientChart LoadPatientChart(ulong ulHumanID, ulong ulEncounterId, DateTime LocalDate, string MACAddress, string UserName, bool bLoad, ulong ulAddendumId, string strObjTpe, bool bIncludeArchive);
        IList<PatientPane> FillPatientPane(ulong Humanid, ulong Encounterid, string MACAddress, string UserName, bool bLoad);
        //Added by Chithra on 11-Mar-2015 to include Archived Encounter table in query.
        IList<PatientPane> FillPatientPaneIncludeArchive(ulong Humanid, string MACAddress, string UserName, bool bLoad);
        IList<Encounter> GetEncounterUsingHumanID(ulong ulHumanID);
        IList<Encounter> GetEncountersforFacilityDOS(ulong ulHumanID, DateTime sCurrentDate, string sFacilityName);
        // void BatchInsertUpdateCheckOut(IList<Eligibility_Verification> InsertEligList, IList<Encounter> UpdateEncList, IList<VisitPayment> InsertVisitList, IList<Check> SaveCheckList, IList<PPHeader> SavePPHeaderList, IList<PPLineItem> SavePPLineItemList, IList<AccountTransaction> SaveAccountList, string MACAddress);
        DataTable ToDataTable(ArrayList arylstColName, ArrayList arylstValues, ArrayList arlstEncwithM2, ArrayList arlstEncWithAnnualProcedure, string sAnnualProcedure);
        // int CreateUpdateEncounterWithoutTransaction(ref IList<Encounter> ListToInsert, IList<Encounter> ListToUpdate, ISession MySession, string MACAddress);
        DataSet DtblHccReport(DateTime From_Date, DateTime To_Date, string StatementforProcess, string sAnnualProcedure);
        //FillAppointment GetAppointmentByDateandPhy(DateTime DOS, ulong[] PhyID, string[] FacName);
        IList<FillWillingonCancel> GetAppointmentsforPatientwithStatus(ulong ulHumanID, Boolean bShowPastAlso, int PageNumber, int MaxResultSet);
        AppointmentPreChecks CheckDuplicateAppointment(ulong PatientId, ulong PhysicianId, DateTime DOS, string FacilityName, DateTime Blockdate, string Time, int Duration, ulong EncID);
        FillPatientSummaryBarDTO LoadPatientSummaryBar(ulong EncounterId, ulong HumanId, DateTime LocalDate, string UserName, string sLegalorg);
        //ChargePostingDTO CreateEncounterforCoding(IList<Encounter> EncList, IList<WFObjectBilling> WfList, string MacAddress);
        //IList<AppointmentReport> GetAppointmentReport(DateTime AppointmentDateFrom, DateTime AppointmentDateTo);
        DataSet dsPendingHccReport(string sAnnualProcedure, string strCarrierName);
        DataSet DtblProductivityReport(DateTime dtFromDate, DateTime dtToDate);
        void OrderByPRocess(string process, ArrayList arylstProductivityReport, ArrayList aryreportlist);
        void FillDataTableRws(ArrayList listDatas, DataTable dtProductivityReport, int count, string process_name);
        DataSet DtblAssessmentDIFFReport(DateTime From_date, DateTime To_Date, int Tagvalue);
        IList<Encounter> UpdateEncounterHospitalization(Encounter EncounterRecord, string MACAddress);
        DataSet DtblCPBatchReport(DateTime From_date, DateTime To_Date);
        DataSet DtblPPBatchReport(DateTime From_Date, DateTime To_Date);
        //Ginu
        IList<Encounter> GetAppointmentByHumanIDFacilityDate(ulong PatientId, DateTime DOS, string facilityName);
        //Ginu
        IList<Encounter> GetEncounterUsingHumanIDandDOS(ulong Human_ID, DateTime DOS);
        //ulong CreateVirtualEncounter(Encounter encounterobj, WFObjectBilling objWf_obj, string MacAddress);
        //ChargePostingDTO LoadEncounterList(IList<DateTime> DateofService, ulong HumanId);
        //void UpdateEncounterWithEncounterChild(Encounter encounterRecord, WFObject wfObjectRecord, string macAddress);
        DataSet DtblTransportationReport(DateTime From_date, DateTime To_Date);
        DataSet DtblAppointmentSummaryReport(DateTime From_date, DateTime To_Date, string PlanName);
        void MoveToNextProcess(IList<object> arlstProcess, string sOwner, string sMacAddress);
        void UpdateEncounterWithEncounterChild(Encounter encounterRecord, WFObject wfObjectRecord, WFObject wfEncObject, string macAddress, int iCloseType);
        DataSet DtblAppointmentDetailsReport(DateTime From_Date, DateTime To_Date);
        DataSet GetEHRMeasureCalculation(IList<StaticLookup> FieldLookupLst, string FromDate, string ToDate, ulong PhysicianID, int vitalAge, int SmokingAge, string RcopiaUserName);
        // int UpdateEncounterWithSession(Encounter objEnc, ISession Mysession, string MacAddress);
        //muthu on 9-jun-2011
        DataSet DtblAppointmentsReport(DateTime From_date, DateTime To_Date, ulong Physician_Name, ulong Human_Name, string[] ObjType, string Facility_Name, string[] currentProcess, string PlanName, ulong EncPhysicianName, string CarrierName, ulong PcpID, string strCreatedBy);
        //UserDTO FindUser(string UserName, int iPageNumber, int iMaxResult);
        //muthu on 9-jun-2011
        //Added  by Janani on 3-09-11
        void SubmitOrdersAndPrescriptions(ulong ulMyEncounterID, ulong ulMyHumanID, ulong ulMyPhysicianID, string UserName, string MACAddress, string FacilityName, DateTime currentDateTime);
        //Added  by Janani on 8-09-11
        // MoveVerificationDTO PerformMoveVerification(ulong ulMyEncounterID, ulong selectedPhysicianID, ulong ulMyHumanID, DateTime currentDate, string FacilityName, string UserName, bool VerifyPFSH, string Source, string SourceOtherInfo, string userCurrentProcess, string MACAddress, string btnPhyCorrectionText, bool bDuplicateCheck, string UserRole);
        MoveVerificationDTO PerformMoveVerification(ulong ulMyEncounterID, ulong selectedPhysicianID, ulong ulMyHumanID, DateTime currentDate, string FacilityName, string UserName, bool VerifyPFSH, string Source, string SourceOtherInfo, string userCurrentProcess, string MACAddress, string btnPhyCorrectionText, bool bDuplicateCheck, string UserRole, string btnID, bool breview, string IsACOValid, out string sAlert);
        //Added by Latha on 8-sep-2011 - performance tuning - Start
        void MoveToNextProcessFromPrintDocuments(Encounter encounterRecord, ulong ulEncounterID, ulong ulHumanID, ulong ulSelectedPhyID, string sUserName, string sButtonName, string sFacilityName, string sMacAddress, DateTime dtCurrentDateTime, bool bMovetoReview, string UserRole, int CloseType);
        //Added by Latha on 8-sep-2011 - performance tuning - End
        FillEncounterandWFObject GetEncounterandWFObject(ulong ulEncID, ulong ulAddendumId, string strObjTpe);
        //Added by Chithra on 10-Mar_2016 - To include archive tables in query
        FillEncounterandWFObject GetEncounterandWFObjectIncludeArchive(ulong ulEncID, ulong ulAddendumId, string strObjTpe, bool bIncludeArchive);
        //Added by Suresh on 12-sep-2011 - Coders Production Report
        DataSet DtblCodersReport(DateTime From_date, DateTime To_Date, string sFacilityName);
        //Added by Suresh on 12-sep-2011 - Coders Production Report

        CheckOutDTO CheckOutLoad(ulong humanID, ulong encounterID, string MACAddress, string UserName);

        void BatchOperationsOnCheckOut(IList<Documents> SaveDocuList, IList<Documents> UpdateDocuList, IList<Documents> DeleteDocuList, Encounter objEncounter, ulong HumanID, ulong EncounterID, string sMacAddress, string UserName, DateTime currentDate, bool Is_FormClosed);
        void SaveUpdatePhoneEncounter(IList<Encounter> SaveEncounter, IList<TreatmentPlan> SavePlan, string strHumanDetails, string sMacAddress);
        // Added by Manimozhi on jan 20th 2012- Start
        FillNewEditAppointment GetEncounterAndHumanRecord(ulong EncID, ulong HumanID);
        //void UpdateEncounterForRoom(ulong EncID, string ExamRoom, string ModifiedBy, DateTime ModifiedDateandTime, string MACAddress);
        void UpdateEncounterForRoom(ulong EncID, string ExamRoom, string ModifiedBy, string ObjType, DateTime ModifiedDateandTime, string MACAddress);
        void UpdateEncounterforMedAsst(ulong ulEncID, string sMedAsstName, string sModifiedBy, DateTime dtModifiedDateandTime, string MACAddress);
        void UpdateDateOfService(ulong ulEncID, DateTime dtDateOfService, string MACAddress, string sLocal_Time);
        //void MoveToNextProcessFromAfterPlan(Encounter encounterRecord, ulong ulEncounterID, ulong ulHumanID, ulong ulSelectedPhyID, string sUserName, string sButtonName, string sFacilityName, string sMacAddress, DateTime dtCurrentDateTime, bool bMovetoReview);
        void MoveToNextProcessFromAfterPlan(Encounter encounterRecord, ulong ulEncounterID, ulong ulHumanID, ulong ulSelectedPhyID, string sUserName, string sButtonName, string sFacilityName, string sMacAddress, DateTime dtCurrentDateTime, bool bMovetoReview, WFObject objEncWfObj, WFObject objDocWfObject, WFObject objDocReviewWfObj, string UserRole);
        //Added by manimozhi -End

        DataSet DtblOfficeManagerSummaryReport(DateTime From_date, DateTime To_Date, string CurrentProcess, string CurrentOwner);
        DataSet DtblPatientSeenReport(DateTime From_date, DateTime To_Date, string ReportFor, bool IncludeHouseCallVisit, string Active);
        IList<WorkFlow> GetWorkFlowMapListForReports();
        DataSet DtblRefillReport(int phyid, DateTime From_date, DateTime To_Date);
        //FillAppointment GetAppointmentByDateandPhyRCM(DateTime DOS, ulong[] PhyID, string[] FacName, string sView, out string time_taken);
        //Added by Bala for Appointment Facility Screen on 27 sep 2013
        //FillAppointment GetAppointmentByDateandPhyRCMFacility(DateTime DOS, ulong[] PhyID, string[] FacName, string sView, out string time_taken);
        //Added by Bala for Electronic Super Bill Q
        //IList<ElectronicsuperbillQDTO> GetElectronicSuperBillQ(string sUserName);

        //Added by srividhya on 06-May-2013
        IList<Encounter> GetEncounterByEncAndHumanID(ulong ulEncounterID, ulong ulHumanID);
        IList<User> GetUsers();

        //Added by Bala for Print Recipt
        IList<FillPrintRecipt> FillPrintRecipt(ulong ulEncounterID, ulong ulHumanID, Boolean bIsEncounterBased);

        IList<Encounter> GetEncounterByEncounterIDforPaperSuperBill(ulong ulhuman_id);
        void InsertToWfobjectforPrintsuperbill(ulong ulMyEncounterID, DateTime currentDate, string FacilityName, string UserName, ulong selectedPhysicianID);
        //Added by Srividhya on 13-12-2013 for Print SuperBill
        SuperBillDTO GetEncounterDetailsforPaperSuperBill(ulong ulEncounterID);
        IList<SuperBillDTO> GetMultipleEncounterDetailsforPaperSuperBill(string sFacilityName, string sApptDate, ulong ulApptProvID);
        IList<Encounter> GetEncounterByEncounterIDforPS(ulong ulhuman_id, ulong ulEncounterID);
        IList<FillAppointmentSlot> GetEncounterForAppointmentSlot(string sFacilityname, ulong ulPhysicianID, DateTime sDateofService, string sNoofDays);
        //Added by bala for Willing on cancellation
        IList<FillWillingonCancel> GetWillingCancellationList();
        string GetNotification(ulong ulEncounterId, ulong ulHumanID, string UserName, String ScreenName, IList<CDSRuleMaster> ClinicalList, IList<Notification> NotificationList, IList<UserLookup> UserLookupList, DateTime Current_dt, string sUserRole, string sPhysicianSpecialty);//, string sEncounteProviderName);
        IList<Encounter> GetEncounterUsingHumanIDlist(IList<ulong> lstHumanID);
        //IList<Encounter> GetEncounterUsingHumanIDlist(IList<ulong> lstHumanID);
        IList<Encounter> GetEncoutnerListByPhyID(int Phy_ID);
        IList<Encounter> GetEncounterList(IList<ulong> EncLst);
        void UpdateEncounterList(Encounter EncRecord, string MacAddress);
        string SaveSummaryOfCare(SummaryOfCareDTO SummaryDTOList, string UserName, string FacilityName, ulong EncID, string sLegalOrg);
        HumanDTO GetHumanDetailsbyApptDate(string FacilityName, string EncounterProviderId, string ApptDate);
        IList<string> GetEncounterListArray(ulong ulHuman_ID, string sCategory, string sInterpretationSubDocType);
        void UpdateEncounterforMyQueue(ulong ulEncID, string sMedAsstName, string sModifiedBy, DateTime dtModifiedDateandTime, DateTime dtDateOfService, string ObjType, string ExamRoom, string MACAddress, string sLocal_Time);
        IList<MyQ> UpdateEncounterMoveTo(ulong ulEncID, string sMedAsstName, string sModifiedBy, DateTime dtModifiedDateandTime, string CurrentObjType,
        string ExamRoom, string FacName, string objtype, string[] processtype, Boolean bShowall, int DefaultNoofDays, string MACAddress);

        //MoveVerificationDTO PerformMovetoProvider(ulong ulMyEncounterID, ulong selectedPhysicianID, ulong ulMyHumanID, DateTime currentDate, string FacilityName, string UserName, bool VerifyPFSH, string Source, string SourceOtherInfo, string userCurrentProcess, string MACAddress, string btnPhyCorrectionText, bool bDuplicateCheck, string UserRole, string btnID, bool breview, string IsACOValid, WFObject objEncWfObj, Encounter EncRecord, string IsPatientDiscussed, ulong IsDiscussedBy, bool bAcoCheck, string sRoleName, string sUsername);
        MoveVerificationDTO PerformMovetoNextProcess(ulong ulMyEncounterID, ulong selectedPhysicianID, ulong ulMyHumanID, DateTime currentDate, string FacilityName, string UserName, bool VerifyPFSH, string Source, string SourceOtherInfo, string userCurrentProcess, string MACAddress, string btnPhyCorrectionText, bool bDuplicateCheck, string UserRole, string btnID, bool breview, string IsACOValid, WFObject objEncWfObj, Encounter EncRecord, WFObject objDocWfobj, WFObject objDocReviewWfobj, string IsPatientDiscussed, ulong IsDiscussedBy, out string sAlert);
        IList<Encounter> GetBillingInstructions(ulong EncounterID, string Keyword);

        void UpdateEncounterList(IList<Encounter> EncounterList, string MACAddress, object[] IsCMGOrders);
        IList<Encounter> GetEncounterUsingHumanIDOrderByEncID(ulong ulHumanID);

        string CopyPreviousEncounter(ulong encounterId, ulong humanId, ulong physicianId,
                                           string currentUserName, ulong previousEncounterId, string userRole, DateTime dtCurrentDOS);
        void UpdateDocumentationEncounterWithEncounterChild(Encounter EncounterRecord, WFObject wfEncObject, string macAddress, int iCloseType);
        void GetEandMCount(ulong EncounterID, string UserRole, out bool isEandMCPTPresent, out bool isEandMICDPresent, out bool isEandMPriICDPresent);
        ArrayList GetPatientListBulkAccess(string fromdate, string todate, string PhyId);
        bool GetEligibleEncCaseReporting(out IList<string> EligibleEncList);
        IList<Encounter> GetOrderingPhysicianByOrderSubmitID(ulong iOrderSubmit);
        IList<Encounter> GetAppointmentForBulkEncounterTemplate(string ProviderId, DateTime DOS, string facilityName);
        IList<Encounter> GetPhoneEncounter(ulong ulHumanID);
        ulong SaveUpdatePhoneEncounter(IList<Encounter> SaveEncounter, IList<TreatmentPlan> SavePlan, string sMacAddress);

        DataTable GetEncountersbyDOSRange(ulong ulHumanID, DateTime dtPatientDOB, string sMemberID, ulong ulCarrierID, DateTime dtFromDate, DateTime dtToDate, string sLegalOrg);
        IList<string> GetEncounterListForIndexing(ulong ulHuman_ID, string sMonths);
        DataTable GetEncountersbyDOSRange(ulong ulHumanID, DateTime dtPatientDOB, string sMemberID, ulong ulCarrierID, DateTime dtFromDate, DateTime dtToDate, string sLastName, string sFirstName, string sPlanId, string sLegalOrg);
    }
    public partial class EncounterManager : ManagerBase<Encounter, ulong>, IEncounterManager
    {


        #region Constructors

        public EncounterManager()
            : base()
        {

        }
        public EncounterManager(INHibernateSession session)
            : base(session)
        {

        }
        #endregion
        string strLineitem;
        Hashtable result = new Hashtable();
        #region Get Methods


        public IList<Encounter> GetEncounterByEncounterID(ulong EncounterID)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", EncounterID));
                ilstEncounter = crit.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            // return crit.List<Encounter>();
        }

        public IList<Encounter> GetEncounterByOrderSubmitID(int ulOrderSubmitID)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Order_Submit_ID", ulOrderSubmitID));
                ilstEncounter = crit.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            // return crit.List<Encounter>();
        }

        public IList<Encounter> GetEncounterByEncounterIDIncludeArchive(ulong EncounterID)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sqlquery = iMySession.CreateSQLQuery("SELECT enc.* FROM encounter enc where enc.encounter_id=" + EncounterID.ToString() +
                    " union all SELECT arc.* FROM encounter_arc arc  where arc.encounter_id=" + EncounterID.ToString() + ";").AddEntity("e", typeof(Encounter));
                ilstEncounter = sqlquery.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            // return crit.List<Encounter>();
        }
        public IList<Encounter> GetrecentEncounterByhumanandProcess(ulong HumanID, string obj, string process)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sqlquery = iMySession.CreateSQLQuery("select a.* from encounter as a join wf_object as b on a.encounter_id=b.obj_system_id where human_id=" + HumanID + " and obj_type='" + obj + "'and current_Process='" + process + "' order by encounter_id desc limit 1").AddEntity("a", typeof(Encounter));
                ilstEncounter = sqlquery.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            // return crit.List<Encounter>();
        }
        public IList<Encounter> GetEncounterByApptID(ulong ApptID)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Encounter_ID", ApptID));
                ilstEncounter = crit.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            //return crit.List<Encounter>();
        }
        public IList<Encounter> GetAllEncounterList()
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).AddOrder(Order.Desc("Date_of_Service"));
                ilstEncounter = crit.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            //return crit.List<Encounter>();
            //return GetAll(GetCount("Acurus.Capella.Core.DomainObjects.Encounter"))

        }
        public IList<Encounter> GetEncounterListForBulkExport(int provider_id, string fromDate, string toDate)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sqlquery = iMySession.CreateSQLQuery("SELECT e.* FROM encounter e where encounter_provider_id=" + provider_id + " and date_of_service >='" + fromDate + "' and date_of_service <='" + toDate + "' group by human_id;").AddEntity("e", typeof(Encounter));
                ilstEncounter = sqlquery.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            // return sqlquery.List<Encounter>();
        }
        public IList<Encounter> GetEncounterUsingHumanIDandDOS(ulong Human_ID, DateTime DOS)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", Human_ID)).Add(Expression.Eq("Date_of_Service", DOS));
                ilstEncounter = crit.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            //return crit.List<Encounter>();
        }
        string strNotifiaction = string.Empty;
        string NotificationSummaryText = string.Empty;

        public FillPatientChart LoadPatientChart(ulong ulHumanID, ulong ulEncounterId, DateTime LocalDate, string MACAddress, string UserName, bool bLoad, ulong ulAddendumId, string strObjType, bool bIncludeArchive)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            FillPatientChart objFillChart = new FillPatientChart();
            ProblemListManager objProbListMgr = new ProblemListManager();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                // objFillChart.PblmMedList = objProbListMgr.GetProblemDescriptionList(ulHumanID);             
                if (strObjType.ToUpper() == "ADDENDUM" && bIncludeArchive)
                    objFillChart.PatChartList = FillPatientPaneIncludeArchive(ulHumanID, MACAddress, UserName, bLoad);
                else
                    objFillChart.PatChartList = FillPatientPane(ulHumanID, ulEncounterId, MACAddress, UserName, bLoad);
                //srividhya added on 04-Jun-2015 
                if (bLoad == false)
                {
                    return objFillChart;
                }
                if (strObjType.ToUpper() == "ADDENDUM" && bIncludeArchive)
                    objFillChart.Fill_Encounter_and_WFObject = GetEncounterandWFObjectIncludeArchive(ulEncounterId, ulAddendumId, strObjType, bIncludeArchive);
                else
                    objFillChart.Fill_Encounter_and_WFObject = GetEncounterandWFObject(ulEncounterId, ulAddendumId, strObjType);

                iMySession.Close();
            }


            return objFillChart;
        }


        //vinoth branch 3_0_43
        int NotificationCount = 0;
        private void FillText(string Heading, string strRule, string Message, string Attributes, string Developer, string FundingSource, string link, string ReleaseDate, string Classification, string ResolveScreen)
        {
            if (Heading != string.Empty && Heading.Trim() != "")
            {
                NotificationCount = NotificationCount + 1;
                if (Classification != string.Empty && Classification.Trim() != "")
                    strNotifiaction += "<u>[#" + Heading + " - " + Classification + "]</u>" + Environment.NewLine;
                else
                    strNotifiaction += "<u>[#" + Heading + "]</u>" + Environment.NewLine;
            }
            if (strRule != string.Empty && strRule.Trim() != "")
            {
                strNotifiaction += "[ Rule: ]" + strRule;
            }
            if (Message != string.Empty && Message.Trim() != "")
            {
                strNotifiaction += "[ Message: ]" + Message + Environment.NewLine;
            }
            if (Attributes != string.Empty && Attributes.Trim() != "")
            {
                strNotifiaction += Environment.NewLine + "<p style='font-size: 12.5px;font-family: Microsoft Sans Serif;'>###" + link + "  target='_blank' ><b> Citation:</b>!" + Attributes + "</p>" + Environment.NewLine;
            }
            if (Developer != string.Empty && Developer.Trim() != "")
            {
                strNotifiaction += "[# Developer: ]" + Developer + "  |  ";
            }
            if (FundingSource != string.Empty && FundingSource.Trim() != "")
            {
                strNotifiaction += "[# Funding Source: ]" + FundingSource + "  |  ";
            }
            if (ReleaseDate != string.Empty && ReleaseDate.Trim() != "")
            {
                strNotifiaction += "[# Release Date: ]" + ReleaseDate + Environment.NewLine;
            }
            if (ResolveScreen != string.Empty && ResolveScreen.Trim() != "")
            {
                strNotifiaction += "~~RESOLVE~~" + ResolveScreen;
            }

            strNotifiaction += "~@ ";
            if (Classification == "MACRA MEASURE")
            {
                if (NotificationSummaryText == string.Empty)
                {
                    NotificationSummaryText = "<ul><li>" + Heading + "</li>";
                }
                else
                {
                    NotificationSummaryText += "<li>" + Heading + "</li>";
                }
            }
        }
        private bool AgeAndSex(string sFromAge, string sToAge, string sSex, ulong HumanID)
        {
            bool AryAgeSex = false;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ISQLQuery query;
                if (sSex.ToUpper() == "ALL")
                    query = iMySession.CreateSQLQuery("Select * from human where DATE_FORMAT(current_date, '%Y') - DATE_FORMAT(birth_date, '%Y') - (DATE_FORMAT(current_date, '00-%m-%d') < DATE_FORMAT(birth_date, '00-%m-%d')) between" + "'" + sFromAge + "'" + "and" + "'" + sToAge + "'" + "and Human_ID=" + "'" + HumanID + "'").AddEntity(typeof(Human));
                else
                    query = iMySession.CreateSQLQuery("Select * from human where DATE_FORMAT(current_date, '%Y') - DATE_FORMAT(birth_date, '%Y') - (DATE_FORMAT(current_date, '00-%m-%d') < DATE_FORMAT(birth_date, '00-%m-%d')) between" + "'" + sFromAge + "'" + "and" + "'" + sToAge + "'" + "and sex =" + "'" + sSex + "'" + "and Human_ID=" + "'" + HumanID + "'").AddEntity(typeof(Human));
                if (query.List<Human>().Count > 0)
                    AryAgeSex = true;
                iMySession.Close();
            }
            return AryAgeSex;

        }
        //Vinoth
        public IList<Encounter> UpdateEncounterHospitalization(Encounter EncounterRecord, string MACAddress)
        {
            IList<Encounter> EncList = null;
            IList<Encounter> EncListupdate = new List<Encounter>();
            IList<Encounter> EncVersionList = GetEncounterByEncounterID(EncounterRecord.Id);//Added by thiyagarajan.M 20-04-2012
            EncounterRecord.Version = EncVersionList[0].Version;
            EncListupdate.Add(EncounterRecord);
            GenerateXml XMLObj = new GenerateXml();
            // SaveUpdateDeleteWithTransaction(ref EncList, EncListupdate, null, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref EncList, ref EncListupdate, null, MACAddress, true, false, EncounterRecord.Id, string.Empty);

            //if (EncListupdate.Count > 0)
            //{
            //    GenerateXml XMLObj = new GenerateXml();
            //    List<object> lstObjEncHosHis = new List<object>();
            //    lstObjEncHosHis = EncListupdate.Cast<object>().ToList();
            //    ulong encounterid = EncListupdate[0].Id;
            //    XMLObj.GenerateXmlSave(lstObjEncHosHis, encounterid, string.Empty);
            //}

            return GetEncounterByEncounterID(EncListupdate[0].Id);
        }
        //Vinoth

        //prabu 14/04/10
        //public ulong CreateEncounter(Encounter EncounterRecord, WFObject wfObjectRecord, string MACAddress, object[] IsCMGLabObject)
        //{
        //    iTryCount = 0;

        //TryAgain:
        //    int iResult = 0;
        //    ISession MySession = Session.GetISession();
        //    ITransaction trans = null;
        //    IList<Encounter> EncList = new List<Encounter>();
        //    try
        //    {
        //        trans = MySession.BeginTransaction();

        //        EncList.Add(EncounterRecord);
        //        iResult = SaveUpdateDeleteWithoutTransaction(ref EncList, null, null, MySession, MACAddress);
        //        //Added for CMG Lab
        //        if (Convert.ToBoolean(IsCMGLabObject[0]))
        //        {
        //            if (IsCMGLabObject.Count() == 3)
        //            {
        //                ISession temp = session.GetISession();
        //                OrdersManager objOrdersManager = new OrdersManager();
        //                objOrdersManager.UpdateCMGEncounterID(EncList[0].Id, Convert.ToUInt32(IsCMGLabObject[1]), IsCMGLabObject[2].ToString(), MACAddress, temp);
        //            }
        //            else if (IsCMGLabObject.Count() == 4)
        //            {
        //                ISession temp = session.GetISession();
        //                OrdersManager objOrdersManager = new OrdersManager();
        //                objOrdersManager.UpdateCMGEncounterID(Convert.ToUInt32(IsCMGLabObject[3]), EncList[0].Id, Convert.ToUInt32(IsCMGLabObject[1]), IsCMGLabObject[2].ToString(), MACAddress, temp);
        //            }

        //        }
        //        //if bResult = false then, the deadlock is occured 
        //        if (iResult == 2)
        //        {
        //            if (iTryCount < 5)
        //            {
        //                iTryCount++;
        //                goto TryAgain;
        //            }
        //            else
        //            {
        //                trans.Rollback();
        //                //  MySession.Close();
        //                throw new Exception("Deadlock occurred. Transaction failed.");
        //            }
        //        }
        //        else if (iResult == 1)
        //        {
        //            trans.Rollback();
        //            //  MySession.Close();
        //            throw new Exception("Exception occurred. Transaction failed.");
        //        }
        //        if (wfObjectRecord != null)
        //        {
        //            wfObjectRecord.Obj_System_Id = EncList[0].Id;
        //            WFObjectManager wfObjectManager = new WFObjectManager();
        //            iResult = wfObjectManager.InsertToWorkFlowObject(wfObjectRecord, 1, MACAddress, MySession);

        //            if (iResult == 2)
        //            {
        //                if (iTryCount < 5)
        //                {
        //                    iTryCount++;
        //                    goto TryAgain;
        //                }
        //                else
        //                {
        //                    trans.Rollback();
        //                    // MySession.Close();
        //                    throw new Exception("Deadlock occurred. Transaction failed.");
        //                }
        //            }
        //            else if (iResult == 1)
        //            {
        //                trans.Rollback();
        //                // MySession.Close();
        //                throw new Exception("Exception occurred. Transaction failed.");
        //            }
        //        }

        //        //To check MapFacilityPhysician
        //        ICriteria crit = MySession.CreateCriteria(typeof(MapFacilityPhysician)).Add(Expression.Eq("Facility_Name", EncounterRecord.Facility_Name)).Add(Expression.Eq("Phy_Rec_ID", Convert.ToUInt64(EncounterRecord.Appointment_Provider_ID)));
        //        if (crit.List<MapFacilityPhysician>().Count == 0)
        //        {
        //            MapFacilityPhysician map = new MapFacilityPhysician();
        //            map.Facility_Name = EncounterRecord.Facility_Name;
        //            map.Phy_Rec_ID = Convert.ToUInt64(EncounterRecord.Appointment_Provider_ID);
        //            map.Status = "Y";
        //            MapFacilityPhysicianManager mapMngr = new MapFacilityPhysicianManager();
        //            iResult = mapMngr.SaveMapFacilityPhysician(map, MACAddress, MySession);
        //            if (iResult == 2)
        //            {
        //                if (iTryCount < 5)
        //                {
        //                    iTryCount++;
        //                    goto TryAgain;
        //                }
        //                else
        //                {
        //                    trans.Rollback();
        //                    //  MySession.Close();
        //                    throw new Exception("Deadlock occurred. Transaction failed.");
        //                }
        //            }
        //            else if (iResult == 1)
        //            {
        //                trans.Rollback();
        //                //  MySession.Close();
        //                throw new Exception("Exception occurred. Transaction failed.");
        //            }
        //        }

        //        MySession.Flush();
        //        trans.Commit();
        //    }
        //    catch (NHibernate.Exceptions.GenericADOException ex)
        //    {
        //        trans.Rollback();
        //        // MySession.Close();
        //        throw new Exception(ex.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        trans.Rollback();
        //        //MySession.Close();
        //        throw new Exception(e.Message);
        //    }

        //    finally
        //    {
        //        MySession.Close();
        //    }
        //    return EncList[0].Id;
        //}
        //prabu 14/04/10

        public IList<Encounter> UpdateEncounter(Encounter EncounterRecord, string MACAddress, object[] IsCMGOrders)
        {
            IList<Encounter> EncList = null;
            IList<Encounter> EncListupdate = new List<Encounter>();
            EncListupdate.Add(EncounterRecord);
            //SaveUpdateDeleteWithTransaction(ref EncList, EncListupdate, null, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref EncList, ref EncListupdate, null, MACAddress, true, true, EncounterRecord.Id, string.Empty);
            //Added for CMG Lab
            if (Convert.ToBoolean(IsCMGOrders[0]))
            {
                ISession MySecondSession = session.GetISession();
                ITransaction trans = MySecondSession.BeginTransaction();
                OrdersManager objOrdersManager = new OrdersManager();
                objOrdersManager.UpdateCMGEncounterID(EncListupdate[0].Id, Convert.ToUInt32(IsCMGOrders[1]), IsCMGOrders[2].ToString(), MACAddress, MySecondSession);
                MySecondSession.Flush();
                trans.Commit();
            }
            // Encounter UpdateEncounterRecord = Encounter(EncListupdate);
            return EncListupdate;
        }
        public void UpdateEncounterList(IList<Encounter> EncounterList, string MACAddress, object[] IsCMGOrders)
        {
            IList<Encounter> EncList = null;
            // SaveUpdateDeleteWithTransaction(ref EncList, EncounterList, null, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref EncList, ref EncounterList, null, MACAddress, true, false, EncounterList[0].Id, string.Empty);
            //Added for CMG Lab
            if (Convert.ToBoolean(IsCMGOrders[0]))
            {
                ISession MySecondSession = session.GetISession();
                ITransaction trans = MySecondSession.BeginTransaction();
                OrdersManager objOrdersManager = new OrdersManager();
                objOrdersManager.UpdateCMGEncounterID(EncounterList[0].Id, Convert.ToUInt32(IsCMGOrders[1]), IsCMGOrders[2].ToString(), MACAddress, MySecondSession);
                MySecondSession.Flush();
                trans.Commit();
            }

        }
        public void UpdateEncounterWithEncounterChild(Encounter EncounterRecord, WFObject wfObjectRecord, WFObject wfEncObject, string macAddress, int iCloseType)
        {
            iTryCount = 0;
            bool bEncounter = true;
            GenerateXml XMLObj = new GenerateXml();
        TryAgain:
            int iResult = 0;

            ISession MySession = Session.GetISession();
            ITransaction trans = null;
            IList<Encounter> EncList = null;
            IList<Encounter> EncListUpdate = new List<Encounter>();
            WFObjectManager wfObjectManager = new WFObjectManager();
            try
            {
                trans = MySession.BeginTransaction();

                EncListUpdate.Add(EncounterRecord);
                //iResult = SaveUpdateDeleteWithoutTransaction(ref EncList, EncListUpdate, null, MySession, macAddress);
                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref EncList, ref EncListUpdate, null, MySession, macAddress, true, false, EncListUpdate[0].Id, string.Empty, ref XMLObj);
                bEncounter = XMLObj.CheckDataConsistency(EncListUpdate.Cast<object>().ToList(), false, "");
                wfObjectManager.MoveToNextProcess(wfEncObject.Obj_System_Id, wfEncObject.Obj_Type, 4, "UNKNOWN", System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), macAddress, null, MySession);
                //if bResult = false then, the deadlock is occured 
                if (iResult == 2)
                {
                    if (iTryCount < 5)
                    {
                        iTryCount++;
                        goto TryAgain;
                    }
                }
                if (wfObjectRecord != null)
                {
                    iResult = wfObjectManager.InsertToWorkFlowObject(wfObjectRecord, iCloseType, macAddress, MySession);
                    if (iResult == 2)
                    {
                        if (iTryCount < 5)
                        {
                            iTryCount++;
                            goto TryAgain;
                        }
                        else
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                if (bEncounter)
                {
                    trans.Commit();
                    // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                    int trycount = 0;
                trytosaveagain:
                    try
                    {
                        // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                    }
                    catch (Exception xmlexcep)
                    {
                        trycount++;
                        if (trycount <= 3)
                        {
                            int TimeMilliseconds = 0;
                            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                            Thread.Sleep(TimeMilliseconds);
                            string sMsg = string.Empty;
                            string sExStackTrace = string.Empty;

                            string version = "";
                            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                            string[] server = version.Split('|');
                            string serverno = "";
                            if (server.Length > 1)
                                serverno = server[1].Trim();

                            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                sMsg = xmlexcep.InnerException.Message;
                            else
                                sMsg = xmlexcep.Message;

                            if (xmlexcep != null && xmlexcep.StackTrace != null)
                                sExStackTrace = xmlexcep.StackTrace;

                            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                            string ConnectionData;
                            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                            {
                                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                {
                                    cmd.Connection = con;
                                    try
                                    {
                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            goto trytosaveagain;
                        }
                    }
                }
                else
                    throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                trans.Rollback();
                // MySession.Close();
                throw new Exception(ex.Message);
            }
            catch (Exception e)
            {
                trans.Rollback();
                //MySession.Close();
                throw new Exception(e.Message);
            }

            finally
            {
                MySession.Close();
            }
        }
        public void UpdateDocumentationEncounterWithEncounterChild(Encounter EncounterRecord, WFObject wfEncObject, string macAddress, int iCloseType)
        {
            {
                iTryCount = 0;
                bool bEncounter = true;
                GenerateXml XMLObj = new GenerateXml();
            TryAgain:
                int iResult = 0;

                ISession MySession = Session.GetISession();
                ITransaction trans = null;
                IList<Encounter> EncList = null;
                IList<Encounter> EncListUpdate = new List<Encounter>();
                WFObjectManager wfObjectManager = new WFObjectManager();
                try
                {
                    trans = MySession.BeginTransaction();

                    EncListUpdate.Add(EncounterRecord);
                    //iResult = SaveUpdateDeleteWithoutTransaction(ref EncList, EncListUpdate, null, MySession, macAddress);
                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref EncList, ref EncListUpdate, null, MySession, macAddress, true, false, EncListUpdate[0].Id, string.Empty, ref XMLObj);
                    bEncounter = XMLObj.CheckDataConsistency(EncListUpdate.Cast<object>().ToList(), false, "");
                    wfObjectManager.MoveToNextProcess(wfEncObject.Obj_System_Id, wfEncObject.Obj_Type, 4, "UNKNOWN", System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), macAddress, null, MySession);
                    //if bResult = false then, the deadlock is occured 
                    if (iResult == 2)
                    {
                        if (iTryCount < 5)
                        {
                            iTryCount++;
                            goto TryAgain;
                        }
                    }
                    //if (wfObjectRecord != null)
                    //{
                    //    iResult = wfObjectManager.InsertToWorkFlowObject(wfObjectRecord, iCloseType, macAddress, MySession);
                    //    if (iResult == 2)
                    //    {
                    //        if (iTryCount < 5)
                    //        {
                    //            iTryCount++;
                    //            goto TryAgain;
                    //        }
                    //        else
                    //        {
                    //            trans.Rollback();
                    //            //MySession.Close();
                    //            throw new Exception("Deadlock occurred. Transaction failed.");
                    //        }
                    //    }
                    //    else if (iResult == 1)
                    //    {
                    //        trans.Rollback();
                    //        // MySession.Close();
                    //        throw new Exception("Exception occurred. Transaction failed.");
                    //    }
                    //}
                    if (bEncounter)
                    {
                        trans.Commit();
                        // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);

                        int trycount = 0;
                    trytosaveagain:
                        try
                        {
                            //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                        }
                        catch (Exception xmlexcep)
                        {
                            trycount++;
                            if (trycount <= 3)
                            {
                                int TimeMilliseconds = 0;
                                if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                    TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                Thread.Sleep(TimeMilliseconds);
                                string sMsg = string.Empty;
                                string sExStackTrace = string.Empty;

                                string version = "";
                                if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                    version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                string[] server = version.Split('|');
                                string serverno = "";
                                if (server.Length > 1)
                                    serverno = server[1].Trim();

                                if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                    sMsg = xmlexcep.InnerException.Message;
                                else
                                    sMsg = xmlexcep.Message;

                                if (xmlexcep != null && xmlexcep.StackTrace != null)
                                    sExStackTrace = xmlexcep.StackTrace;

                                string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                string ConnectionData;
                                ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                {
                                    using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                    {
                                        cmd.Connection = con;
                                        try
                                        {
                                            con.Open();
                                            cmd.ExecuteNonQuery();
                                            con.Close();
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                                goto trytosaveagain;
                            }
                        }
                    }
                    else
                        throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
                }
                catch (NHibernate.Exceptions.GenericADOException ex)
                {
                    trans.Rollback();
                    // MySession.Close();
                    throw new Exception(ex.Message);
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    //MySession.Close();
                    throw new Exception(e.Message);
                }

                finally
                {
                    MySession.Close();
                }
            }
        }

        //public ulong CreateVirtualEncounter(Encounter encounterobj, WFObjectBilling objWf_obj, string MacAddress)
        //{
        //    iTryCount = 0;
        //    ulong wf_obj_id;
        //    IList<Encounter> EncList = new List<Encounter>();
        //    EncList.Add(encounterobj);
        //TryAgain:
        //    Boolean bResult = false;

        //    ISession MySession = Session.GetISession();
        //    ITransaction trans = null;
        //    try
        //    {
        //        trans = MySession.BeginTransaction();


        //        SaveUpdateDeleteWithoutTransaction(ref EncList, null, null, MySession, MacAddress);

        //        WFObjectBillingManager objWfobjMngr = new WFObjectBillingManager();
        //        wf_obj_id = objWfobjMngr.InsertToWorkFlowObject(objWf_obj, MySession, MacAddress);

        //        MySession.Flush();
        //        trans.Commit();
        //    }
        //    catch (NHibernate.Exceptions.GenericADOException ex)
        //    {
        //        trans.Rollback();
        //        // MySession.Close();
        //        throw new Exception(ex.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        trans.Rollback();
        //        //MySession.Close();
        //        throw new Exception(e.Message);
        //    }
        //    finally
        //    {
        //        MySession.Close();
        //    }

        //    return wf_obj_id;
        //}

        //public ChargePostingDTO CreateEncounterforCoding(IList<Encounter> EncList, IList<WFObjectBilling> WfList, string MacAddress)
        //{
        //    ChargePostingDTO objChargePostingDTO = new ChargePostingDTO();
        //    iTryCount = 0;

        //TryAgain:
        //    int iResult = 0;
        //    Boolean bResult = false;

        //    ISession MySession = Session.GetISession();
        //    ITransaction trans = null;
        //    try
        //    {
        //        trans = MySession.BeginTransaction();
        //        if (EncList != null && EncList.Count > 0)
        //        {
        //            iResult = SaveUpdateDeleteWithoutTransaction(ref EncList, null, null, MySession, MacAddress);
        //            if (iResult == 2)
        //            {
        //                if (iTryCount < 5)
        //                {
        //                    iTryCount++;
        //                    goto TryAgain;
        //                }
        //                else
        //                {

        //                    trans.Rollback();
        //                    //  MySession.Close();
        //                    throw new Exception("Deadlock is occured. Transaction failed");

        //                }
        //            }
        //            else if (iResult == 1)
        //            {

        //                trans.Rollback();
        //                // MySession.Close();
        //                throw new Exception("Exception is occured. Transaction failed");

        //            }
        //            if (WfList != null && WfList.Count > 0)
        //            {
        //                for (int i = 0; i < EncList.Count; i++)
        //                {
        //                    WfList[i].Obj_System_Id = EncList[i].Id;
        //                }
        //            }
        //        }

        //        if (WfList != null && WfList.Count > 0)
        //        {
        //            for (int i = 0; i < WfList.Count; i++)
        //            {
        //                WFObjectBillingManager objWfobjMngr = new WFObjectBillingManager();
        //                ulong wf_obj_id = objWfobjMngr.InsertToWorkFlowObject(WfList[i], MySession, MacAddress);
        //            }
        //        }

        //        MySession.Flush();
        //        trans.Commit();
        //    }
        //    catch (NHibernate.Exceptions.GenericADOException ex)
        //    {
        //        trans.Rollback();
        //        // MySession.Close();
        //        throw new Exception(ex.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        trans.Rollback();
        //        //MySession.Close();
        //        throw new Exception(e.Message);
        //    }
        //    finally
        //    {
        //        MySession.Close();
        //    }

        //    for (int i = 0; i < EncList.Count; i++)
        //    {
        //        objChargePostingDTO.EncounterList.Add(EncList[i]);
        //        objChargePostingDTO.WfObjectList.Add(WfList[i]);
        //    }

        //    return objChargePostingDTO;
        //}

        //public int CreateUpdateEncounterWithoutTransaction(ref IList<Encounter> ListToInsert, IList<Encounter> ListToUpdate, ISession MySession, string MACAddress)
        //{
        //    int iResult = 0;

        //    if ((ListToInsert != null) || (ListToUpdate != null))
        //    {
        //        iResult = SaveUpdateDeleteWithoutTransaction(ref ListToInsert, ListToUpdate, null, MySession, MACAddress);
        //    }
        //    return iResult;
        //}
        public string[] GetInusranceDetails(ulong Humanid)
        {
            ArrayList humanlist = null;
            string[] arry = new string[8];
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.PatientPaneForHuman");
                query2.SetString(0, Humanid.ToString());
                humanlist = new ArrayList(query2.List());
                for (int i = 0; i < humanlist.Count; i++)
                {
                    object[] obj = (object[])humanlist[i];
                    if (i == 0)
                    {
                        if (obj[0] != null) arry[0] = obj[0].ToString();
                        if (obj[1] != null) arry[1] = obj[1].ToString();
                        if (obj[2] != null) arry[2] = obj[2].ToString();
                        if (obj[3] != null) arry[3] = obj[3].ToString();
                    }
                    if (i == 1)
                    {
                        if (obj[0] != null) arry[4] = obj[0].ToString();
                        if (obj[1] != null) arry[5] = obj[1].ToString();
                        if (obj[2] != null) arry[6] = obj[2].ToString();
                        if (obj[3] != null) arry[7] = obj[3].ToString();
                    }
                }
                iMySession.Close();
            }
            return arry;
        }
        //public IList<PatientPane> FillPatientPane(ulong Humanid, string MACAddress, string UserName, bool bLoad)
        //{
        //    // PatientPane objPatientPane = new PatientPane();
        //    IList<PatientPane> objPatientPane = new List<PatientPane>();
        //    ArrayList PatientList = null;
        //    // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query1 = iMySession.GetNamedQuery("Fill.PatientPane");
        //        query1.SetString(0, Humanid.ToString());
        //        PatientList = new ArrayList(query1.List());
        //        //<<<<<<< EncounterManager.cs
        //        //            IQuery query2 = session.GetISession().GetNamedQuery("Fill.PatientPaneForHuman");
        //        //            query2.SetString(0, Humanid.ToString());
        //        //            ArrayList humanlist = null;
        //        //            humanlist = new ArrayList(query2.List());
        //        //||||||| 1.199

        //        //=======
        //        //IQuery query2 = iMySession.GetNamedQuery("Fill.PatientPaneForHuman");
        //        //query2.SetString(0, Humanid.ToString());
        //        //ArrayList humanlist = null;
        //        //humanlist = new ArrayList(query2.List());

        //        //>>>>>>> 1.199.2.3

        //        PatientPane p = null;
        //        PatientPane plan = null;
        //        System.Web.UI.Page page = new System.Web.UI.Page();
        //        //added by Ginu on 2-Aug-2010
        //        if (PatientList != null)
        //        {
        //            for (int i = 0; i < PatientList.Count; i++)
        //            {
        //                p = new PatientPane();
        //                object[] obj = (object[])PatientList[i];

        //                if (obj[0] != null) p.Human_Id = Convert.ToUInt64(obj[0]);
        //                if (obj[1] != null) p.Last_Name = obj[1].ToString();
        //                if (obj[2] != null) p.First_Name = obj[2].ToString();
        //                if (obj[3] != null) p.MI = obj[3].ToString();
        //                if (obj[4] != null) p.Birth_Date = Convert.ToDateTime(obj[4]);
        //                if (obj[5] != null) p.Sex = obj[5].ToString();
        //                if (obj[6] != null) p.Medical_Record_Number = obj[6].ToString();
        //                if (obj[7] != null) p.SSN = obj[7].ToString();
        //                if (obj[8] != null) p.Encounter_ID = Convert.ToUInt64(obj[8]);
        //                if (obj[9] != null) p.Date_of_Service = Convert.ToDateTime(obj[9]);
        //                if (obj[10] != null) p.Assigned_Physician = obj[10].ToString();
        //                if (obj[11] != null) p.Assigned_Medical_Asst = obj[11].ToString();
        //                if (obj[12] != null) p.Appointment_Date_Time = Convert.ToDateTime(obj[12]);
        //                if (obj[13] != null) p.Suffix = obj[13].ToString();
        //                if (obj[14] != null) p.HomePhoneNo = obj[14].ToString();
        //                if (obj[15] != null) p.Assigned_Physician_User_Name = obj[15].ToString();
        //                if (obj[16] != null) p.Is_Sent_To_RCopia = obj[16].ToString();
        //                if (obj[17] != null) p.Patient_Status = obj[17].ToString();
        //                if (obj[18] != null) p.Patient_Type = obj[18].ToString();
        //                if (obj[19] != null) p.ACO_Validation = obj[19].ToString();
        //                objPatientPane.Add(p);
        //            }
        //        }

        //        /*For Bug Id 54525
        //         if (obj[0] != null) p.Human_Id = Convert.ToUInt64(obj[0]);
        //                if (obj[1] != null) p.Last_Name = obj[1].ToString();
        //                if (obj[2] != null) p.First_Name = obj[2].ToString();
        //                if (obj[3] != null) p.MI = obj[3].ToString();
        //                if (obj[4] != null) p.Birth_Date = Convert.ToDateTime(obj[4]);
        //                if (obj[5] != null) p.Sex = obj[5].ToString();
        //                if (obj[6] != null) p.Medical_Record_Number = obj[6].ToString();
        //                if (obj[7] != null) p.SSN = obj[7].ToString();
        //               //if (obj[8] != null) p.Is_Previous = obj[8].ToString();
        //                if (obj[9] != null) p.Encounter_ID = Convert.ToUInt64(obj[9]);
        //                if (obj[10] != null) p.Date_of_Service = Convert.ToDateTime(obj[10]);
        //               //if (obj[11] != null) p.Notes = obj[11].ToString();
        //                if (obj[12] != null) p.Assigned_Physician = obj[12].ToString();
        //                if (obj[13] != null) p.Assigned_Medical_Asst = obj[13].ToString();
        //                //if (obj[14] != null) p.Physician_Last_Name = obj[14].ToString();
        //                //if (obj[15] != null) p.Physician_First_Name = obj[15].ToString();
        //                //if (obj[16] != null) p.Physician_Middle_Name = obj[16].ToString();
        //                //if (obj[17] != null) p.Physician_Suffix = obj[17].ToString();
        //                if (obj[18] != null) p.Appointment_Date_Time = Convert.ToDateTime(obj[18]);
        //                if (obj[19] != null) p.Suffix = obj[19].ToString();
        //                if (obj[20] != null) p.HomePhoneNo = obj[20].ToString();
        //                if (obj[21] != null) p.Assigned_Physician_User_Name = obj[21].ToString();
        //                if (obj[22] != null) p.Is_Sent_To_RCopia = obj[22].ToString();
        //                if (obj[23] != null) p.Patient_Status = obj[23].ToString();
        //                //if (obj[24] != null) p.Facility_Name = obj[24].ToString();
        //                if (obj[25] != null) p.Patient_Type = obj[25].ToString();
        //                if (obj[26] != null) p.ACO_Validation = obj[26].ToString(); 

        //         */


        //        // if (obj[25] != null) p.CarrierName = obj[25].ToString();
        //        //if (obj[8] != null) p.Insurance_Type = obj[8].ToString();
        //        //if (obj[9] != null) p.Insurance_Plan_ID = Convert.ToUInt64(obj[9]);
        //        //if (obj[10] != null) p.Ins_Plan_Name = obj[10].ToString();
        //        //if (obj[11] != null) p.Problem_Description = obj[11].ToString();
        //        //<<<<<<< EncounterManager.cs

        //        //                    if (obj[8] != null) p.Is_Previous = obj[8].ToString();
        //        //                    if (obj[9] != null) p.Encounter_ID = Convert.ToUInt64(obj[9]);
        //        //                    if (obj[10] != null) p.Date_of_Service = Convert.ToDateTime(obj[10]);
        //        //                    if (obj[11] != null) p.Notes = obj[11].ToString();
        //        //                    if (obj[12] != null) p.Assigned_Physician = obj[12].ToString();
        //        //                    if (obj[13] != null) p.Assigned_Medical_Asst = obj[13].ToString();
        //        //                    if (obj[14] != null) p.Physician_Last_Name = obj[14].ToString();
        //        //                    if (obj[15] != null) p.Physician_First_Name = obj[15].ToString();
        //        //                    if (obj[16] != null) p.Physician_Middle_Name = obj[16].ToString();
        //        //                    if (obj[17] != null) p.Physician_Suffix = obj[17].ToString();
        //        //                    if (obj[18] != null) p.Appointment_Date_Time = Convert.ToDateTime(obj[18]);
        //        //                    if (obj[19] != null) p.Suffix = obj[19].ToString();
        //        //                    if (obj[20] != null) p.HomePhoneNo = obj[20].ToString();
        //        //                    if (obj[21] != null) p.Assigned_Physician_User_Name = obj[21].ToString();
        //        //                    // if (obj[25] != null) p.CarrierName = obj[25].ToString();
        //        //                    if (obj[22] != null) p.Is_Sent_To_RCopia = obj[22].ToString();
        //        //                    if (obj[23] != null) p.Patient_Status = obj[23].ToString();

        //        //||||||| 1.199
        //        //                    if (obj[11] != null) p.Is_Previous = obj[11].ToString();
        //        //                    if (obj[12] != null) p.Encounter_ID = Convert.ToUInt64(obj[12]);
        //        //                    if (obj[13] != null) p.Date_of_Service = Convert.ToDateTime(obj[13]);
        //        //                    if (obj[14] != null) p.Notes = obj[14].ToString();
        //        //                    if (obj[15] != null) p.Assigned_Physician = obj[15].ToString();
        //        //                    if (obj[16] != null) p.Assigned_Medical_Asst = obj[16].ToString();
        //        //                    if (obj[17] != null) p.Physician_Last_Name = obj[17].ToString();
        //        //                    if (obj[18] != null) p.Physician_First_Name = obj[18].ToString();
        //        //                    if (obj[19] != null) p.Physician_Middle_Name = obj[19].ToString();
        //        //                    if (obj[20] != null) p.Physician_Suffix = obj[20].ToString();
        //        //                    if (obj[21] != null) p.Appointment_Date_Time = Convert.ToDateTime(obj[21]);
        //        //                    if (obj[22] != null) p.Suffix = obj[22].ToString();
        //        //                    if (obj[23] != null) p.HomePhoneNo = obj[23].ToString();
        //        //                    if (obj[24] != null) p.Assigned_Physician_User_Name = obj[24].ToString();
        //        //                    if (obj[25] != null) p.CarrierName = obj[25].ToString();
        //        //                    if (obj[26] != null) p.Is_Sent_To_RCopia = obj[26].ToString();
        //        //=======
        //        //<<<<<<< EncounterManager.cs
        //        //            if (humanlist != null && humanlist.Count > 0)
        //        //            {
        //        //                p.Insurance_Plan_ID = new List<ulong>();
        //        //                p.Insurance_Type = new List<string>();
        //        //                p.Ins_Plan_Name = new List<string>();
        //        //                p.CarrierName = new List<string>();
        //        //                for (int i = 0; i < humanlist.Count; i++)
        //        //                {
        //        //                    object[] obj = (object[])humanlist[i];
        //        //                    if (obj[0] != null) p.Insurance_Type.Add(obj[0].ToString());
        //        //                    if (obj[1] != null) p.Insurance_Plan_ID.Add(Convert.ToUInt64(obj[1]));
        //        //                    if (obj[2] != null) p.Ins_Plan_Name.Add(obj[2].ToString());
        //        //                    if (obj[3] != null) p.CarrierName.Add(obj[3].ToString());
        //        //                }
        //        //            }
        //        //||||||| 1.199

        //        //=======
        //        //if (humanlist != null && humanlist.Count > 0)
        //        //{
        //        //    p.Insurance_Plan_ID = new List<ulong>();
        //        //    p.Insurance_Type = new List<string>();
        //        //    p.Ins_Plan_Name = new List<string>();
        //        //    p.CarrierName = new List<string>();
        //        //    for (int i = 0; i < humanlist.Count; i++)
        //        //    {
        //        //        object[] obj = (object[])humanlist[i];
        //        //        if (obj[0] != null) p.Insurance_Type.Add(obj[0].ToString());
        //        //        if (obj[1] != null) p.Insurance_Plan_ID.Add(Convert.ToUInt64(obj[1]));
        //        //        if (obj[2] != null) p.Ins_Plan_Name.Add(obj[2].ToString());
        //        //        if (obj[3] != null) p.CarrierName.Add(obj[3].ToString());
        //        //    }
        //        //}

        //        //>>>>>>> 1.199.2.3
        //        //Srividhya dded on 04-Jun-2015
        //        if (bLoad == true) //Only if condition executes, if this method is called from UI not from console application
        //        {
        //            if (page.Session["CheckPatientPane"] != null && page.Session["CheckPatientPane"].ToString() == "true")
        //            {
        //                goto l;
        //            }
        //        }
        //        //audit log select entry only for find patient-mu ehr
        //        //if (NHibernateSessionUtility.Instance.AuditTrailTableList != null)
        //        //{
        //        //    DateTime dttansdatetime = DateTime.Now;
        //        //    if (page.Session["UTCAudittime"] != null)
        //        //    {
        //        //        dttansdatetime = Convert.ToDateTime(page.Session["UTCAudittime"].ToString());
        //        //        page.Session.Remove("UTCAudittime");
        //        //    }
        //        //    if (NHibernateSessionUtility.Instance.AuditTrailTableList.Contains("Acurus.Capella.Core.DomainObjects.Human") == true)
        //        //    {
        //        //        AuditLogManager auditLogMngr = new AuditLogManager();
        //        //        string sEntityName = "Acurus.Capella.Core.DomainObjects.Human";
        //        //        IList<AuditLog> SaveAuditLogList = new List<AuditLog>();
        //        //        AuditLog objAuditLog = new AuditLog();
        //        //        objAuditLog.Entity_Id = Convert.ToInt32(Humanid);
        //        //        objAuditLog.Human_ID = Convert.ToInt32(Humanid);
        //        //        objAuditLog.Transaction_By = UserName;
        //        //        objAuditLog.Transaction_Date_And_Time = dttansdatetime;
        //        //        objAuditLog.Transaction_Type = "SELECT";
        //        //        objAuditLog.MAC_Address = MACAddress;
        //        //        objAuditLog.Entity_Name = sEntityName;
        //        //        SaveAuditLogList.Add(objAuditLog);

        //        //        auditLogMngr.SaveUpdateDeleteWithTransaction(ref SaveAuditLogList, null, null, MACAddress);
        //        //    }
        //        //}
        //        iMySession.Close();
        //    }
        //l:
        //    return objPatientPane;
        //}
        //BugID:54697
        public IList<PatientPane> FillPatientPane(ulong Humanid, ulong Encounterid, string MACAddress, string UserName, bool bLoad)
        {
            IList<PatientPane> objPatientPane = new List<PatientPane>();
            PatientPane objPatPane = new PatientPane();
            ulong EncProviderId = 0;

            GenerateXml objGenerateXML = new GenerateXml();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc = objGenerateXML.ReadBlob("Human", Humanid);
            //if (xmlDoc != null)
            //{

            //}

            #region Human & Encounter Info from Human_XML
            //string HumanFileName = "Human" + "_" + Humanid + ".xml";
            //string strHuman_XmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);

            //if (File.Exists(strHuman_XmlFilePath))
            if (xmlDoc != null)
            {
                //XmlTextReader xmlReader = new XmlTextReader(strHuman_XmlFilePath);
                //try
                //{
                //    xmlDoc.Load(xmlReader);
                //}
                //catch (Exception ex)
                //{
                //    xmlReader.Close();
                //    throw ex;
                //}
                //xmlReader.Close();




                XmlNodeList xmlNodelst = xmlDoc.GetElementsByTagName("HumanList");
                if (xmlNodelst != null && xmlNodelst.Count > 0 && xmlNodelst[0].ChildNodes.Count > 0)
                {
                    if (xmlNodelst[0].ChildNodes[0].Attributes.GetNamedItem("Id").Value.ToString().Equals(Humanid.ToString()))
                    {
                        XmlNode xmlHumanNode = xmlNodelst[0].ChildNodes[0];
                        objPatPane.Human_Id = Convert.ToUInt32(xmlHumanNode.Attributes.GetNamedItem("Id").Value.ToString());
                        objPatPane.Last_Name = xmlHumanNode.Attributes.GetNamedItem("Last_Name").Value.ToString();
                        objPatPane.First_Name = xmlHumanNode.Attributes.GetNamedItem("First_Name").Value.ToString();
                        objPatPane.MI = xmlHumanNode.Attributes.GetNamedItem("MI").Value.ToString();
                        objPatPane.Birth_Date = Convert.ToDateTime(xmlHumanNode.Attributes.GetNamedItem("Birth_Date").Value.ToString());
                        objPatPane.Sex = xmlHumanNode.Attributes.GetNamedItem("Sex").Value.ToString();
                        objPatPane.Medical_Record_Number = xmlHumanNode.Attributes.GetNamedItem("Medical_Record_Number").Value.ToString();
                        objPatPane.SSN = xmlHumanNode.Attributes.GetNamedItem("SSN").Value.ToString();
                        objPatPane.Suffix = xmlHumanNode.Attributes.GetNamedItem("Suffix").Value.ToString();
                        objPatPane.HomePhoneNo = xmlHumanNode.Attributes.GetNamedItem("Home_Phone_No").Value.ToString();
                        //objPatPane.Is_Sent_To_RCopia = xmlHumanNode.Attributes.GetNamedItem("Is_Sent_To_Rcopia").Value.ToString();
                        objPatPane.Patient_Status = xmlHumanNode.Attributes.GetNamedItem("Patient_Status").Value.ToString();
                        objPatPane.Patient_Type = xmlHumanNode.Attributes.GetNamedItem("Human_Type").Value.ToString();
                        //objPatPane.ACO_Validation = xmlHumanNode.Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value.ToString();
                        objPatPane.CellPhoneNo = xmlHumanNode.Attributes.GetNamedItem("Cell_Phone_Number").Value.ToString();
                        objPatPane.PhotoPath = xmlHumanNode.Attributes.GetNamedItem("Photo_Path").Value.ToString();
                        objPatPane.PastDue = xmlHumanNode.Attributes.GetNamedItem("Past_Due").Value.ToString();
                        if (xmlHumanNode.Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value != null && xmlHumanNode.Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value != string.Empty)
                            objPatPane.ACO_Is_Eligible_Patient = xmlHumanNode.Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value.ToString();
                        else
                            objPatPane.ACO_Is_Eligible_Patient = "";
                        if (xmlHumanNode.Attributes.GetNamedItem("Is_Translator_Required") != null && xmlHumanNode.Attributes.GetNamedItem("Is_Translator_Required").Value != null && xmlHumanNode.Attributes.GetNamedItem("Is_Translator_Required").Value != string.Empty && xmlHumanNode.Attributes.GetNamedItem("Is_Translator_Required").Value == "Y")
                        {
                            objPatPane.Preferred_Language = xmlHumanNode.Attributes.GetNamedItem("Preferred_Language").Value.ToString();
                        }
                        else
                        {
                            objPatPane.Preferred_Language = "";
                        }
                        //For bud Id 70617 
                        XmlNodeList xmlAgenode = xmlDoc.GetElementsByTagName("Age");
                        if (xmlAgenode != null && xmlAgenode.Count > 0)
                        {
                            if (xmlAgenode[0].Attributes[0].Value.Trim() == "" || xmlAgenode[0].Attributes[0].Value.Trim() == "0")
                            {
                                xmlAgenode[0].Attributes[0].Value = CalculateAge(objPatPane.Birth_Date).ToString();
                                //xmlDoc.Save(strHuman_XmlFilePath);
                                int trycount = 0;
                            trytosaveagain:
                                try
                                {
                                    //xmlDoc.Save(strHuman_XmlFilePath);
                                    ISession session = Session.GetISession();
                                    try
                                    {
                                        using (ITransaction trans = session.BeginTransaction(IsolationLevel.ReadUncommitted))
                                        {
                                            WriteBlob(Humanid, xmlDoc, session, null, null, null, null, true);

                                            trans.Commit();
                                        }
                                    }
                                    catch (Exception ex1)
                                    {

                                        throw new Exception(ex1.Message);
                                    }
                                }
                                catch (Exception xmlexcep)
                                {
                                    trycount++;
                                    if (trycount <= 3)
                                    {
                                        int TimeMilliseconds = 0;
                                        if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                            TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                        Thread.Sleep(TimeMilliseconds);
                                        string sMsg = string.Empty;
                                        string sExStackTrace = string.Empty;

                                        string version = "";
                                        if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                            version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                        string[] server = version.Split('|');
                                        string serverno = "";
                                        if (server.Length > 1)
                                            serverno = server[1].Trim();

                                        if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                            sMsg = xmlexcep.InnerException.Message;
                                        else
                                            sMsg = xmlexcep.Message;

                                        if (xmlexcep != null && xmlexcep.StackTrace != null)
                                            sExStackTrace = xmlexcep.StackTrace;

                                        string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                        string ConnectionData;
                                        ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                        using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                        {
                                            using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                            {
                                                cmd.Connection = con;
                                                try
                                                {
                                                    con.Open();
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }
                                        goto trytosaveagain;
                                    }
                                }

                            }
                        }
                    }
                }
                XmlNodeList xmlEncNodelst = xmlDoc.GetElementsByTagName("EncounterList");
                if (xmlEncNodelst != null && xmlEncNodelst.Count > 0 && xmlEncNodelst[0].ChildNodes.Count > 0)
                {
                    //for (int i = 0; i < xmlEncNodelst[0].ChildNodes.Count; i++)
                    //{
                    //    if (xmlEncNodelst[0].ChildNodes[i].Attributes.GetNamedItem("Id").Value.ToString().Equals(Encounterid.ToString()) &&
                    //        xmlEncNodelst[0].ChildNodes[i].Attributes.GetNamedItem("Human_ID").Value.ToString().Equals(Humanid.ToString()) &&
                    //        xmlEncNodelst[0].ChildNodes[i].Attributes.GetNamedItem("Is_Phone_Encounter").Value.ToString() != "Y")
                    //    {
                    //        XmlNode xmlEncounterNode = xmlEncNodelst[0].ChildNodes[i];
                    //        objPatPane.Encounter_ID = Convert.ToUInt32(xmlEncounterNode.Attributes.GetNamedItem("Id").Value.ToString());
                    //        objPatPane.Date_of_Service = Convert.ToDateTime(xmlEncounterNode.Attributes.GetNamedItem("Date_of_Service").Value.ToString());
                    //        objPatPane.Assigned_Medical_Asst = xmlEncounterNode.Attributes.GetNamedItem("Assigned_Med_Asst_User_Name").Value.ToString();
                    //        objPatPane.Appointment_Date_Time = Convert.ToDateTime(xmlEncounterNode.Attributes.GetNamedItem("Appointment_Date").Value.ToString());
                    //        EncProviderId = Convert.ToUInt32(xmlEncounterNode.Attributes.GetNamedItem("Encounter_Provider_ID").Value.ToString());
                    //    }
                    //}

                    try
                    {
                        XmlElement xmlElemnt = xmlDoc.DocumentElement;
                        XmlNode xmlEncounterNode = null;
                        xmlEncounterNode = xmlElemnt.SelectSingleNode("/notes/Modules/EncounterList/Encounter[@Id=" + Encounterid.ToString() + "]");
                        if (xmlEncounterNode != null)
                        {
                            objPatPane.Encounter_ID = Convert.ToUInt32(xmlEncounterNode.Attributes.GetNamedItem("Id").Value.ToString());
                            objPatPane.Date_of_Service = Convert.ToDateTime(xmlEncounterNode.Attributes.GetNamedItem("Date_of_Service").Value.ToString());
                            objPatPane.Assigned_Medical_Asst = xmlEncounterNode.Attributes.GetNamedItem("Assigned_Med_Asst_User_Name").Value.ToString();
                            objPatPane.Appointment_Date_Time = Convert.ToDateTime(xmlEncounterNode.Attributes.GetNamedItem("Appointment_Date").Value.ToString());
                            EncProviderId = Convert.ToUInt32(xmlEncounterNode.Attributes.GetNamedItem("Encounter_Provider_ID").Value.ToString());
                        }
                    }
                    catch (Exception e)
                    {

                    }

                }
            }
            #endregion
            #region Physician Info from Config_XML
            string strPhysician_XmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianFacilityMapping.xml");
            if (EncProviderId != 0)
            {
                if (File.Exists(strPhysician_XmlFilePath))
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmlTextReader = new XmlTextReader(strPhysician_XmlFilePath);
                    xmldoc.Load(xmlTextReader);
                    xmlTextReader.Close();
                    try
                    {
                        XmlElement xmlElemnt = xmldoc.DocumentElement;
                        XmlNode xmlPhysicianNode = null;
                        xmlPhysicianNode = xmlElemnt.SelectSingleNode("/ROOT/PhyList/Facility/Physician[@ID=" + EncProviderId.ToString() + "]");
                        if (xmlPhysicianNode != null)
                        {
                            string Physician_Last_Name = xmlPhysicianNode.Attributes.GetNamedItem("lastname").Value.ToString();
                            string Physician_First_Name = xmlPhysicianNode.Attributes.GetNamedItem("firstname").Value.ToString();
                            string Physician_Middle_Name = xmlPhysicianNode.Attributes.GetNamedItem("middlename").Value.ToString();
                            string Physician_Suffix = xmlPhysicianNode.Attributes.GetNamedItem("suffix").Value.ToString();
                            objPatPane.Assigned_Physician_User_Name = xmlPhysicianNode.Attributes.GetNamedItem("username").Value.ToString();
                            objPatPane.Assigned_Physician = xmlPhysicianNode.Attributes.GetNamedItem("prefix").Value.ToString() + " " +
                                                            Physician_First_Name + " " + Physician_Middle_Name +
                                                            " " + Physician_Last_Name + " " + Physician_Suffix;
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            #endregion
            objPatientPane.Add(objPatPane);
            return objPatientPane;
        }
        public IList<PatientPane> FillPatientPaneIncludeArchive(ulong Humanid, string MACAddress, string UserName, bool bLoad)
        {
            IList<PatientPane> objPatientPane = new List<PatientPane>();
            ArrayList PatientList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PatientPane.Include.Encounter.Archive");
                query1.SetString(0, Humanid.ToString());
                query1.SetString(1, Humanid.ToString());
                PatientList = new ArrayList(query1.List());

                PatientPane p = null;
                // PatientPane plan = null;
                System.Web.UI.Page page = new System.Web.UI.Page();
                if (PatientList != null)
                {
                    for (int i = 0; i < PatientList.Count; i++)
                    {
                        p = new PatientPane();
                        object[] obj = (object[])PatientList[i];

                        if (obj[0] != null) p.Human_Id = Convert.ToUInt64(obj[0]);
                        if (obj[1] != null) p.Last_Name = obj[1].ToString();
                        if (obj[2] != null) p.First_Name = obj[2].ToString();
                        if (obj[3] != null) p.MI = obj[3].ToString();
                        if (obj[4] != null) p.Birth_Date = Convert.ToDateTime(obj[4]);
                        if (obj[5] != null) p.Sex = obj[5].ToString();
                        if (obj[6] != null) p.Medical_Record_Number = obj[6].ToString();
                        if (obj[7] != null) p.SSN = obj[7].ToString();

                        //For Bug Id 54525
                        // if (obj[8] != null) p.Is_Previous = obj[8].ToString();
                        if (obj[8] != null) p.Encounter_ID = Convert.ToUInt64(obj[8]);
                        if (obj[9] != null) p.Date_of_Service = Convert.ToDateTime(obj[9]);
                        //if (obj[11] != null) p.Notes = obj[11].ToString();
                        if (obj[10] != null) p.Assigned_Physician = obj[10].ToString();
                        //if (obj[13] != null) p.Assigned_Medical_Asst = obj[13].ToString();
                        //if (obj[14] != null) p.Physician_Last_Name = obj[14].ToString();
                        //if (obj[15] != null) p.Physician_First_Name = obj[15].ToString();
                        //if (obj[16] != null) p.Physician_Middle_Name = obj[16].ToString();
                        //if (obj[17] != null) p.Physician_Suffix = obj[17].ToString();
                        if (obj[12] != null) p.Appointment_Date_Time = Convert.ToDateTime(obj[12]);
                        if (obj[13] != null) p.Suffix = obj[13].ToString();
                        if (obj[14] != null) p.HomePhoneNo = obj[14].ToString();
                        if (obj[15] != null) p.Assigned_Physician_User_Name = obj[15].ToString();
                        //if (obj[16] != null) p.Is_Sent_To_RCopia = obj[16].ToString();
                        if (obj[17] != null) p.Patient_Status = obj[17].ToString();
                        //if (obj[24] != null) p.Facility_Name = obj[24].ToString();
                        if (obj[18] != null) p.Patient_Type = obj[18].ToString();
                        if (obj[19] != null) p.ACO_Is_Eligible_Patient = obj[19].ToString();
                        //if (obj[20] != null) p.Cell_Phone_Number = obj[19].ToString();
                        if (obj[20] != null) p.PhotoPath = obj[20].ToString();
                        if (obj[21] != null) p.PastDue = obj[21].ToString();
                        objPatientPane.Add(p);
                    }
                }
                if (bLoad == true)
                {
                    if (page.Session["CheckPatientPane"] != null && page.Session["CheckPatientPane"].ToString() == "true")
                    {
                        goto l;
                    }
                }
                iMySession.Close();
            }
        l:
            return objPatientPane;
        }
        public IList<Encounter> GetEncounterUsingHumanID(ulong ulHumanID)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", ulHumanID)).AddOrder(Order.Desc("Date_of_Service"));
                ilstEncounter = crit.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            //return crit.List<Encounter>();
        }

        public IList<Encounter> GetEncountersforFacilityDOS(ulong ulHumanID, DateTime sCurrentDate, string sFacilityName)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            ArrayList ilistObj = null; // new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISQLQuery sql = iMySession.CreateSQLQuery("(select e.Is_Phone_Encounter, e.Cancelation_Reason_Code, e.Facility_Name, cast(e.Appointment_Date as char(50)) as Appointment_Date, cast(e.Encounter_ID as char(50)), e.Batch_Status, 'Main' , e.Reschedule_Reason_Code from encounter e where human_id='" + ulHumanID + "' and appointment_date >= DATE_SUB('" + sCurrentDate.ToString("yyyy-MM-dd hh:mm:ss") + "',INTERVAL 60 day)) union all (select e.Is_Phone_Encounter, e.Cancelation_Reason_Code, e.Facility_Name,cast(e.Appointment_Date as char(50)) as Appointment_Date, cast(e.Encounter_ID as char(50)), e.Batch_Status, 'Arc', e.Reschedule_Reason_Code from encounter_arc e where human_id='" + ulHumanID + "' and appointment_date >= DATE_SUB('" + sCurrentDate.ToString("yyyy-MM-dd hh:mm:ss") + "',INTERVAL 60 day)) order by Appointment_Date desc"); //.AddEntity("e", typeof(Encounter));

                // ISQLQuery sql = iMySession.CreateSQLQuery("(select e.Is_Phone_Encounter, e.Cancelation_Reason_Code, e.Facility_Name, cast(e.Appointment_Date as char(50)) as Appointment_Date, cast(e.Encounter_ID as char(50)), e.Batch_Status, 'Main' , e.Reschedule_Reason_Code from encounter e where human_id='" + ulHumanID + "') union all (select e.Is_Phone_Encounter, e.Cancelation_Reason_Code, e.Facility_Name,cast(e.Appointment_Date as char(50)) as Appointment_Date, cast(e.Encounter_ID as char(50)), e.Batch_Status, 'Arc', e.Reschedule_Reason_Code from encounter_arc e where human_id='" + ulHumanID + "') order by Appointment_Date desc"); //.AddEntity("e", typeof(Encounter));


                ISQLQuery sql = iMySession.CreateSQLQuery("(select e.Is_Phone_Encounter, e.Cancelation_Reason_Code, e.Facility_Name, cast(e.Appointment_Date as char(50)) as Appointment_Date, cast(e.Encounter_ID as char(50)), e.Batch_Status, 'Main' , e.Reschedule_Reason_Code from encounter e where human_id='" + ulHumanID + "' and facility_name='" + sFacilityName + "' and batch_status != 'CLOSED')  order by Appointment_Date desc"); //.AddEntity("e", typeof(Encounter));

                ilistObj = new ArrayList(sql.List());

                if (ilistObj != null)
                {
                    foreach (object item in ilistObj)
                    {
                        Encounter encRecord = new Encounter();
                        object[] obj = (object[])item;
                        encRecord.Is_Phone_Encounter = obj[0].ToString();
                        encRecord.Cancelation_Reason_Code = obj[1].ToString();
                        encRecord.Facility_Name = obj[2].ToString();
                        encRecord.Appointment_Date = Convert.ToDateTime(obj[3]);
                        encRecord.Id = Convert.ToUInt64(obj[4]);
                        encRecord.Batch_Status = obj[5].ToString();
                        encRecord.Exam_Room = obj[6].ToString();
                        encRecord.Reschedule_Reason_Code = obj[7].ToString();
                        ilstEncounter.Add(encRecord);
                    }
                }

                iMySession.Close();
            }
            return ilstEncounter;
        }


        public IList<Encounter> GetEncounterUsingHumanIDlist(IList<ulong> lstHumanID)
        {
            IList<Encounter> Enclist = new List<Encounter>();
            IList<Encounter> EncIDlist = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                for (int i = 0; i < lstHumanID.Count; i++)
                {
                    ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", lstHumanID[i])).AddOrder(Order.Desc("Date_of_Service"));
                    Enclist = crit.List<Encounter>();
                    if (Enclist.Count > 0)
                    {
                        Encounter objEncounter = new Encounter();
                        objEncounter = Enclist[0];
                        EncIDlist.Add(objEncounter);
                    }
                    //for (int j = 0; j < Enclist.Count; j++)
                    //{
                    //    Encounter objEncounter = new Encounter();
                    //    objEncounter = Enclist[j];
                    //    EncIDlist.Add(objEncounter);
                    //}
                }
                iMySession.Close();
            }
            return EncIDlist;
        }

        //public IList<Encounter> GetEncounterUsingHumanIDlist(IList<ulong> lstHumanID)
        //{
        //    IList<Encounter> Enclist = new List<Encounter>();
        //    IList<Encounter> EncIDlist = new List<Encounter>();
        //    for (int i = 0; i < lstHumanID.Count; i++)
        //    {
        //        ICriteria crit = session.GetISession().CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", lstHumanID[i])).AddOrder(Order.Desc("Date_of_Service"));

        //        Enclist = crit.List<Encounter>();
        //        if (Enclist.Count > 0)
        //        {
        //            //EncIDlist.Add(Enclist[0].Id);
        //            for (int j = 0; j < Enclist.Count; j++)
        //            {
        //                Encounter objEncounter = new Encounter();
        //                objEncounter = Enclist[j];
        //                EncIDlist.Add(objEncounter);
        //            }
        //        }
        //    }
        //    return EncIDlist;
        //}

        public IList<FillWillingonCancel> GetAppointmentsforPatientwithStatus(ulong ulHumanID, Boolean bShowPastAlso, int PageNumber, int MaxResultSet)
        {
            FillWillingonCancel FillApptList = new FillWillingonCancel();
            IList<FillWillingonCancel> ilistApp = new List<FillWillingonCancel>();
            ArrayList aryAppointmentList = null;
            ArrayList aryArcApptList = null;
            int Count = 0;
            int CountPaging = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query3 = iMySession.GetNamedQuery("Get.Appointment.Patient.With.Status.Count");
                query3.SetString(0, ulHumanID.ToString());
                if (bShowPastAlso == true)
                {
                    query3.SetString(1, "1111-11-11");
                }
                else
                {
                    query3.SetString(1, DateTime.Now.Date.ToString("yyyy-MM-dd"));
                }
                aryAppointmentList = new ArrayList(query3.List());

                Count = Convert.ToInt16(aryAppointmentList[0]);
                CountPaging = Convert.ToInt16(aryAppointmentList[0]);
                aryAppointmentList.Clear();
                if (bShowPastAlso == true)
                {
                    IQuery query4 = iMySession.GetNamedQuery("Get.Appointment.Patient.With.Status.Arc.Count");
                    query4.SetString(0, ulHumanID.ToString());
                    query4.SetString(1, "1111-11-11");
                    aryAppointmentList = new ArrayList(query4.List());//Changed from query3 to query4 by srividhya

                    Count = Count + Convert.ToInt16(aryAppointmentList[0]);
                    IQuery query = iMySession.GetNamedQuery("Get.Appointment.Patient.With.Status");
                    query.SetString(0, ulHumanID.ToString());
                    query.SetString(1, "1111-11-11");
                    PageNumber = PageNumber - 1;
                    query.SetInt32(2, PageNumber * MaxResultSet);
                    //if (FillApptList.ApptCount < 25)
                    //    query.SetInt32(3, FillApptList.ApptCount);
                    //else
                    query.SetInt32(3, MaxResultSet);
                    aryAppointmentList = new ArrayList(query.List());
                    if (aryAppointmentList == null || aryAppointmentList.Count < 25)
                    {
                        IQuery query1 = iMySession.GetNamedQuery("Get.Appointment.Patient.With.Status.From.Arc");
                        query1.SetString(0, ulHumanID.ToString());
                        query1.SetString(1, "1111-11-11");
                        //if (aryAppointmentList == null || aryAppointmentList.Count == 0)
                        if (PageNumber * MaxResultSet != 0)
                        {
                            int total = (PageNumber * MaxResultSet) - CountPaging;
                            if (total < 0)
                                total = (-1) * total;
                            query1.SetInt32(2, total);
                        }
                        else
                            query1.SetInt32(2, PageNumber * MaxResultSet);
                        //else
                        //    query1.SetInt32(2, aryAppointmentList.Count);
                        query1.SetInt32(3, MaxResultSet);
                        aryArcApptList = new ArrayList(query1.List());
                        aryAppointmentList.AddRange(aryArcApptList);
                    }
                }
                else
                {
                    IQuery query = iMySession.GetNamedQuery("Get.Appointment.Patient.With.Status");
                    query.SetString(0, ulHumanID.ToString());
                    query.SetString(1, DateTime.Now.Date.ToString("yyyy-MM-dd"));
                    PageNumber = PageNumber - 1;
                    query.SetInt32(2, PageNumber * MaxResultSet);
                    query.SetInt32(3, MaxResultSet);
                    aryAppointmentList = new ArrayList(query.List());
                }

                // added by Ginu on 2-Aug-2010
                if (aryAppointmentList != null)
                {
                    for (int i = 0; i < aryAppointmentList.Count; i++)
                    {
                        object[] oj = (object[])aryAppointmentList[i];
                        FillApptList = new FillWillingonCancel();
                        FillApptList.Encounter_ID = Convert.ToUInt64(oj[0]);
                        FillApptList.Physician_ID = Convert.ToUInt32(oj[1]);
                        //FillApptList.Human_ID.Add(Convert.ToUInt64(oj[2]));
                        FillApptList.Appointment_Date_Time = Convert.ToDateTime(oj[2]);
                        //FillApptList.Duration_Minutes.Add(Convert.ToInt32(oj[4]));
                        FillApptList.Facility_Name = oj[4].ToString();
                        if (oj[3] == null)
                        {
                            FillApptList.Current_Process = "";
                        }
                        else
                        {
                            FillApptList.Current_Process = oj[3].ToString();
                        }
                        FillApptList.Physician_Name = oj[5].ToString();
                        FillApptList.Machine_Technician_Library_ID = Convert.ToUInt32(oj[6]);
                        if (i == 0)
                            FillApptList.Count = Count;
                        if (oj[7] != null)
                            FillApptList.Test_Ordered = oj[7].ToString();
                        if (oj[8] != null)
                            FillApptList.Rescheduled_Appointment_Date = Convert.ToDateTime(oj[8]);
                        if (oj[9] != null)
                            FillApptList.Reason_for_Cancelation = oj[9].ToString();
                        if (oj[10] != null)
                            FillApptList.Human_Type = oj[10].ToString();
                        ilistApp.Add(FillApptList);
                    }
                }
                iMySession.Close();
            }
            return ilistApp;
        }

        //public FillAppointment GetAppointmentByDateandPhy(DateTime DOS, ulong[] PhyID, string[] FacName)
        //{
        //    FillAppointment FillApptList = new FillAppointment();
        //    ArrayList aryAppointmentList = null;
        //    Encounter objAppointment;
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {

        //        string sView = string.Empty;
        //        //Because, of the universal time - we have to do the between in the where criteria. So, that, it 
        //        //will consider the past and next data also - and, in UI, it will fill up the calendar correctly
        //        //2 Days are added and subtracted - because, only then, the universal time issue is not coming - otherwise, some time appt is 
        //        //not displaying - example, when we click on cancel button in new appt screen.
        //        IQuery query = iMySession.GetNamedQuery("Get.Appointment.DateandPhy");
        //        //query.SetString(0, DOS.ToString("yyyy-MM-dd") + "%");
        //        query.SetString(0, DOS.AddDays(-2).ToString("yyyy-MM-dd"));
        //        query.SetString(1, DOS.AddDays(2).ToString("yyyy-MM-dd"));
        //        query.SetParameterList("FacList", FacName);
        //        query.SetParameterList("PhyList", PhyID);


        //        aryAppointmentList = new ArrayList(query.List());

        //        //added by Ginu on 2-Aug-2010
        //        if (aryAppointmentList != null)
        //        {
        //            for (int i = 0; i < aryAppointmentList.Count; i++)
        //            {
        //                object[] oj = (object[])aryAppointmentList[i];
        //                objAppointment = new Encounter();
        //                FillApptList.EncounterId.Add(Convert.ToUInt64(oj[0]));
        //                FillApptList.Appointment_Provider_ID.Add(Convert.ToInt32(oj[1]));
        //                FillApptList.Human_ID.Add(Convert.ToUInt64(oj[2]));
        //                FillApptList.Appointment_Date.Add(Convert.ToDateTime(oj[3]));
        //                FillApptList.Duration_Minutes.Add(Convert.ToInt32(oj[4]));
        //                FillApptList.ApptStatus.Add(oj[5].ToString());
        //                string sPatientName = oj[6].ToString() + "," + oj[7].ToString() +
        //           "  " + oj[8].ToString() + "  " + oj[10].ToString();
        //                FillApptList.PatientName.Add(sPatientName);
        //                if (oj[11] == null)
        //                {
        //                    oj[11] = string.Empty;
        //                }
        //                if (oj[12] == null)
        //                {
        //                    oj[12] = string.Empty;
        //                }
        //                if (oj[13] == null)
        //                {
        //                    oj[13] = string.Empty;
        //                }
        //                if (oj[14] == null)
        //                {
        //                    oj[14] = string.Empty;

        //                }
        //                if (oj[15] == null)
        //                {
        //                    oj[15] = string.Empty;
        //                }

        //                //FillApptList.PhysicianName.Add(oj[11].ToString() + " " + oj[12].ToString() + " " + oj[13].ToString() + " " + oj[14].ToString() + " " + oj[15].ToString());
        //                FillApptList.PhysicianName.Add(oj[11].ToString() + " " + oj[12].ToString() + " " + oj[13].ToString() + " " + oj[14].ToString());

        //            }
        //        }

        //        BlockdaysManager blockMngr = new BlockdaysManager();
        //        FillApptList.blockList = blockMngr.GetBlockDaysUsingPhyFacApptDt(PhyID, FacName, DOS, sView);
        //        iMySession.Close();
        //    }
        //    return FillApptList;

        //}
        public IList<Encounter> GetOLdAppointmentByDateandPatientID(DateTime DOS, ulong PatientId)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.Appointment.DateandPatient");
                query.SetString(0, DOS.ToString("yyyy-MM-dd") + "%");
                query.SetString(1, PatientId.ToString());
                ilstEncounter = query.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            //return query.List<Encounter>();
        }

        //Ginu
        public IList<Encounter> GetAppointmentByHumanIDFacilityDate(ulong PatientId, DateTime DOS, string facilityName)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.Appointment.HumanIDFacilityDate");
                query.SetString(0, DOS.ToString("yyyy-MM-dd") + "%");
                query.SetString(1, PatientId.ToString());
                query.SetString(2, facilityName);
                ilstEncounter = query.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            //return query.List<Encounter>();
        }
        //Ginu

        public AppointmentPreChecks CheckDuplicateAppointment(ulong PatientId, ulong PhysicianId, DateTime DOS, string FacilityName, DateTime Blockdate, string Time, int Duration, ulong EncID)
        {
            IQuery query = null;
            IList<Encounter> SamePatient = new List<Encounter>();
            IList<Encounter> DiffPatient = new List<Encounter>();
            AppointmentPreChecks apptPreCheck = new AppointmentPreChecks();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                //string[] sp = DOS
                //query = iMySession.GetNamedQuery("Get.Appointment.PatientPhysicianDate.ForPatient");
                //query.SetString(0, DOS.ToString("yyyy-MM-dd") + "%");
                //query.SetString(1, PatientId.ToString());
                //query.SetString(2, PhysicianId.ToString());
                //SamePatient = query.List<Encounter>();

                ISQLQuery sqlForPatient = iMySession.CreateSQLQuery("(select {e.*} from encounter e where e.Appointment_Date like'" + DOS.ToString("yyyy-MM-dd") + "%' and e.Human_ID=" + PatientId + " and e.Appointment_Provider_ID=" + PhysicianId + " and e.Cancelation_Reason_Code='' and e.Reschedule_Reason_Code='' order by e.Appointment_Date desc) union all (select {e.*} from encounter_arc e where e.Appointment_Date like'" + DOS.ToString("yyyy-MM-dd") + "%' and e.Human_ID=" + PatientId + " and e.Appointment_Provider_ID=" + PhysicianId + " and e.Cancelation_Reason_Code='' and e.Reschedule_Reason_Code='' order by e.Appointment_Date desc)").AddEntity("e", typeof(Encounter));
                SamePatient = sqlForPatient.List<Encounter>();


                //query = iMySession.GetNamedQuery("Get.Appointment.PatientPhysicianDate.NotForPatient");
                //query.SetString(0, DOS.ToString("yyyy-MM-dd") + "%");
                //query.SetString(1, PhysicianId.ToString());
                //DiffPatient = query.List<Encounter>();

                ISQLQuery sqlNotForPatient = iMySession.CreateSQLQuery("(select {e.*} from encounter e where e.Appointment_Date like'" + DOS.ToString("yyyy-MM-dd") + "%' and e.Appointment_Provider_ID=" + PhysicianId + " and e.Cancelation_Reason_Code='' and e.Reschedule_Reason_Code='' order by e.Appointment_Date desc) union all (select {e.*} from encounter_arc e where e.Appointment_Date like'" + DOS.ToString("yyyy-MM-dd") + "%' and e.Appointment_Provider_ID=" + PhysicianId + " and e.Cancelation_Reason_Code='' and e.Reschedule_Reason_Code='' order by e.Appointment_Date desc)").AddEntity("e", typeof(Encounter));
                DiffPatient = sqlNotForPatient.List<Encounter>();

                apptPreCheck.SamePatient = SamePatient;
                apptPreCheck.DifferentPatient = DiffPatient;

                BlockdaysManager blockMngr = new BlockdaysManager();

                //  apptPreCheck.bBlock = blockMngr.GetBlockStatus(PhysicianId, FacilityName, Blockdate, DOS.TimeOfDay.ToString(), Duration);
                //---start
                DateTime endTime;
                string sFinalEndTime = string.Empty;
                string[] split = Time.Split(':');
                Time = split[0] + ":" + split[1];
                if (split.Length > 0)
                {
                    endTime = new DateTime(0001, 1, 1, Convert.ToInt16(split[0]), Convert.ToInt16(split[1]), 0);
                    endTime = endTime.AddMinutes(Convert.ToDouble(Duration));
                    string[] strEndTime = endTime.TimeOfDay.ToString().Split(':');
                    if (strEndTime.Length > 0)
                    {
                        sFinalEndTime = strEndTime[0] + ":" + strEndTime[1];
                    }

                }
                ArrayList arrList = new ArrayList();

                query = iMySession.GetNamedQuery("Get.BlockDays.GetBlockStatus")
                                   .SetString(0, PhysicianId.ToString())
                                   .SetString(1, FacilityName)
                                   .SetString(2, Blockdate.ToString("yyyy-MM-dd"))
                                   .SetString(3, Time)
                                   .SetString(4, sFinalEndTime);

                arrList = new ArrayList(query.List());


                if (arrList.Count > 0)
                {
                    apptPreCheck.bBlock = true;
                }
                else
                    apptPreCheck.bBlock = false;
                //--end
                // apptPreCheck.Block_Type = blockMngr.GetBlockDays(PhysicianId, FacilityName, Blockdate, DOS.TimeOfDay.ToString(), Duration);
                //--start
                DateTime endTime1;
                string sFinalEndTime1 = string.Empty;
                string[] split1 = Time.Split(':');
                if (split1.Length > 0)
                {
                    endTime1 = new DateTime(0001, 1, 1, Convert.ToInt16(split1[0]), Convert.ToInt16(split1[1]), 0);
                    endTime1 = endTime1.AddMinutes(Convert.ToDouble(Duration));
                    string[] strEndTime1 = endTime1.TimeOfDay.ToString().Split(':');
                    if (strEndTime1.Length > 0)
                    {
                        sFinalEndTime1 = strEndTime1[0] + ":" + strEndTime1[1];
                    }

                }
                ArrayList arrList1 = new ArrayList();

                query = iMySession.GetNamedQuery("Get.BlockDays.GetBlockdays")
                                  .SetString(0, PhysicianId.ToString())
                                  .SetString(1, FacilityName)
                                  .SetString(2, Blockdate.ToString("yyyy-MM-dd"))
                                  .SetString(3, Time)
                                  .SetString(4, sFinalEndTime1);

                arrList1 = new ArrayList(query.List());


                string sBlockType = string.Empty;
                if (arrList1.Count > 0)
                {
                    for (int bDCount = 0; bDCount < arrList1.Count; bDCount++)
                    {
                        if (arrList1[bDCount].ToString().Trim() != "RECURSIVE" || arrList1[bDCount].ToString().Trim() != "NONRECURSIVE")
                        {
                            sBlockType = arrList1[bDCount].ToString();
                        }
                        else
                        {
                            sBlockType = arrList1[bDCount].ToString();
                        }
                    }


                }
                apptPreCheck.Block_Type = sBlockType;
                //--end
                //  IList<Encounter> EncList;
                // EncList = GetEncounterByEncounterID(EncID);                
                //apptPreCheck.CurrentEncounter = EncList;

                //=============================

                ISQLQuery sql = iMySession.CreateSQLQuery("(select {e.*} from encounter e where e.encounter_id=" + EncID + ") Union all (select {e.*} from encounter_arc e where  e.encounter_id=" + EncID + ")").AddEntity("e", typeof(Encounter));


                foreach (Object l in sql.List())
                {
                    Encounter TempEncounter = (Encounter)l;
                    apptPreCheck.CurrentEncounter.Add(TempEncounter);
                }

                //=============================

                iMySession.Close();
            }
            return apptPreCheck;
        }

        public DataSet DtblHccReport(DateTime From_Date, DateTime To_Date, string StatementforProcess, string sAnnualProcedure)
        {
            bool bProcess;
            ArrayList arylstPromedReport = null;
            ArrayList arylstPromedReportColumns = null;
            ArrayList arlstEncwithM2 = new ArrayList();
            DataSet ds = new DataSet();
            ArrayList arlstEncWithAnnualProcedure = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.Getpromed.ColumnHeadings");
                arylstPromedReportColumns = new ArrayList(query2.List());
                if (StatementforProcess == "Yes")
                {
                    IQuery query1 = iMySession.GetNamedQuery("GetWorkFlowObjects.BillingComplete");
                    query1.SetString(0, From_Date.ToString("yyyy-MM-dd"));
                    query1.SetString(1, To_Date.ToString("yyyy-MM-dd"));
                    arylstPromedReport = new ArrayList(query1.List());
                    bProcess = false;
                }
                else
                {
                    IQuery query1 = iMySession.GetNamedQuery("GetWorkFlowObjects");
                    query1.SetString(0, From_Date.ToString("yyyy-MM-dd"));
                    query1.SetString(1, To_Date.ToString("yyyy-MM-dd"));
                    arylstPromedReport = new ArrayList(query1.List());
                    bProcess = true;
                }

                DataTable Dtbl;
                Dtbl = ToDataTable(arylstPromedReportColumns, arylstPromedReport, arlstEncwithM2, arlstEncWithAnnualProcedure, sAnnualProcedure);

                ds.Tables.Add(Dtbl);
                DataTable dtblEncM2 = new DataTable();
                if (arlstEncwithM2.Count > 0 || arlstEncWithAnnualProcedure.Count > 0)
                {
                    ArrayList arlstExcepReportCoumns = null;
                    IQuery qury = iMySession.GetNamedQuery("FillColumns.EnCounterCPTwithM2");
                    arlstExcepReportCoumns = new ArrayList(qury.List());
                    if (arlstExcepReportCoumns.Count > 0)
                    {
                        object[] objlst = (object[])arlstExcepReportCoumns[0];
                        for (int y = 0; y < objlst.Length; y++)
                        {
                            if (y == 1 || y == 4)
                                dtblEncM2.Columns.Add(objlst[y].ToString(), typeof(System.DateTime));
                            else
                                dtblEncM2.Columns.Add(objlst[y].ToString(), typeof(System.String));
                        }
                    }
                }
                //To Create datatable for ExceptionReport(Cpt with Modifeier2); 
                if (arlstEncwithM2.Count > 0)
                {

                    ArrayList arlstEncwithMod2 = null;
                    IQuery qry1;
                    if (bProcess == false)
                    {
                        qry1 = iMySession.GetNamedQuery("CheckEncounterwithModifier2.withDateRange.BillingComplete");
                    }
                    else
                    {
                        qry1 = iMySession.GetNamedQuery("CheckEncounterwithModifier2.withDateRange.GetWorkFlowObjects");
                    }
                    qry1.SetString(0, From_Date.ToString("yyyy-MM-dd"));
                    qry1.SetString(1, To_Date.ToString("yyyy-MM-dd"));
                    arlstEncwithMod2 = new ArrayList(qry1.List());

                    if (arlstEncwithMod2.Count > 0)
                    {
                        for (int z = 0; z < arlstEncwithMod2.Count; z++)
                        {
                            DataRow dr = dtblEncM2.NewRow();
                            object[] objPromedList = (object[])arlstEncwithMod2[z];

                            for (int g = 0; g < objPromedList.Length; g++)
                            {
                                dr[g] = objPromedList[g].ToString();
                            }
                            dtblEncM2.Rows.Add(dr);
                        }
                    }
                }
                //To check Encounter with Annual Procedure code.
                ArrayList alstEncwithAnualProc = null;
                if (arlstEncWithAnnualProcedure.Count > 0)
                {
                    for (int r = 0; r < arlstEncWithAnnualProcedure.Count; r++)
                    {
                        IQuery qry01 = iMySession.GetNamedQuery("GetAnnualProcedureEnc");
                        qry01.SetString(0, arlstEncWithAnnualProcedure[r].ToString());
                        alstEncwithAnualProc = new ArrayList(qry01.List());
                        DataRow dr = dtblEncM2.NewRow();
                        object[] objProcList = (object[])alstEncwithAnualProc[0];
                        objProcList[10] = "Annual Wellness Procedure (G0438 or G0439) is missing";
                        for (int g = 0; g < objProcList.Length; g++)
                        {
                            try
                            {
                                dr[g] = objProcList[g].ToString();
                            }
                            catch
                            {
                                // string s;
                            }
                        }
                        dtblEncM2.Rows.Add(dr);
                    }
                }
                ds.Tables.Add(dtblEncM2);
                iMySession.Close();
            }
            return ds;
        }

        public DataSet dsPendingHccReport(string sAnnualProcedure, string strCarrierName)
        {
            sAnnualProcedure = "G0438,G0439";
            ArrayList arylstPromedReport = null;
            ArrayList arylstPromedReportColumns = null;
            ArrayList arlstEncwithM2 = new ArrayList();
            ArrayList arlstEncWithAnnualProcedure = new ArrayList();
            DataSet ds = new DataSet();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                IQuery query2 = iMySession.GetNamedQuery("Fill.Getpromed.ColumnHeadings");
                arylstPromedReportColumns = new ArrayList(query2.List());
                IQuery query1 = iMySession.GetNamedQuery("GetPendingHccReport");
                if (strCarrierName == "ALL")
                {
                    strCarrierName = "";
                }
                if (strCarrierName.Trim() != string.Empty)
                {
                    query1.SetString(0, strCarrierName.ToString());
                    //query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));

                }
                else
                {
                    query1.SetString(0, "%");

                }
                arylstPromedReport = new ArrayList(query1.List());
                DataTable Dtbl;
                Dtbl = ToDataTable(arylstPromedReportColumns, arylstPromedReport, arlstEncwithM2, arlstEncWithAnnualProcedure, sAnnualProcedure);

                ds.Tables.Add(Dtbl);
                DataTable dtblEncM2 = new DataTable();
                if (arlstEncwithM2.Count > 0 || arlstEncWithAnnualProcedure.Count > 0)
                {
                    ArrayList arlstExcepReportCoumns = null;
                    IQuery qury = iMySession.GetNamedQuery("FillColumns.EnCounterCPTwithM2");
                    arlstExcepReportCoumns = new ArrayList(qury.List());
                    if (arlstExcepReportCoumns.Count > 0)
                    {
                        object[] objlst = (object[])arlstExcepReportCoumns[0];
                        for (int y = 0; y < objlst.Length; y++)
                        {
                            if (y == 1 || y == 4)
                                dtblEncM2.Columns.Add(objlst[y].ToString(), typeof(System.DateTime));
                            else
                                dtblEncM2.Columns.Add(objlst[y].ToString(), typeof(System.String));
                        }
                    }
                }
                //To Create datatable for ExceptionReport(Cpt with Modifeier2); 
                if (arlstEncwithM2.Count > 0)
                {

                    ArrayList arlstEncwithMod2 = null;
                    IQuery qry1 = iMySession.GetNamedQuery("CheckEncounterwithModifier2");
                    arlstEncwithMod2 = new ArrayList(qry1.List());

                    if (arlstEncwithMod2.Count > 0)
                    {

                        for (int z = 0; z < arlstEncwithMod2.Count; z++)
                        {
                            DataRow dr = dtblEncM2.NewRow();
                            object[] objPromedList = (object[])arlstEncwithMod2[z];

                            for (int g = 0; g < objPromedList.Length; g++)
                            {
                                dr[g] = objPromedList[g].ToString();
                            }
                            dtblEncM2.Rows.Add(dr);
                        }
                    }
                }
                //To check Encounter with Annual Procedure code.
                ArrayList alstEncwithAnualProc = null;
                if (arlstEncWithAnnualProcedure.Count > 0)
                {
                    for (int r = 0; r < arlstEncWithAnnualProcedure.Count; r++)
                    {
                        IQuery qry01 = iMySession.GetNamedQuery("GetAnnualProcedureEnc");
                        qry01.SetString(0, arlstEncWithAnnualProcedure[r].ToString());
                        alstEncwithAnualProc = new ArrayList(qry01.List());
                        DataRow dr = dtblEncM2.NewRow();
                        object[] objProcList = (object[])alstEncwithAnualProc[0];
                        objProcList[10] = "Annual Wellness Procedure (G0438 or G0439) is Available but V70.0 is Missing";
                        for (int g = 0; g < objProcList.Length; g++)
                        {
                            try
                            {
                                dr[g] = objProcList[g].ToString();
                            }
                            catch
                            {
                                //string s;
                            }
                        }
                        dtblEncM2.Rows.Add(dr);
                    }

                }
                ds.Tables.Add(dtblEncM2);
                iMySession.Close();
            }
            return ds;


        }

        public DataTable ToDataTable(ArrayList arylstColName, ArrayList arylstValues, ArrayList arlstEncwithM2, ArrayList arlstEncWithAnnualProcedure, string sAnnualProcedure)
        {
            DataTable dtReport = new DataTable();
            string[] sAnnualPhysicalProcedure = new string[2];
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                //string sProcedure = System.Configuration.ConfigurationSettings.AppSettings.Get("AnnualPhysicalProcedure");
                //if(sProcedure!=string.Empty)
                sAnnualPhysicalProcedure = sAnnualProcedure.Split(',');

                object[] objlst = (object[])arylstColName[0];
                bool Annual = true;
                for (int i = 0; i < objlst.Length; i++)
                {
                    if (i == 2 || i == 10 || i == 11 || i == 15)
                    {
                        dtReport.Columns.Add(objlst[i].ToString(), typeof(System.DateTime));
                    }
                    else
                    {
                        dtReport.Columns.Add(objlst[i].ToString(), typeof(System.String));
                    }
                }
                DataRow dr = dtReport.NewRow();
                for (int i = 0; i < arylstValues.Count; i++)
                {
                Found:
                    string Icd9 = string.Empty;
                    string v70 = string.Empty;
                    if (arylstValues.Count > 0)
                    {
                        object[] objPromedList = (object[])arylstValues[i];
                        ulong Encounter_ID = Convert.ToUInt64(objPromedList[0].ToString());

                        //Fill Main Columns
                        IQuery que1 = iMySession.GetNamedQuery("CheckModifier");
                        que1.SetString(0, Encounter_ID.ToString());
                        int Count = Convert.ToInt32(que1.List()[0]);
                        if (Count > 0)
                        {
                            arlstEncwithM2.Add(Encounter_ID);
                            i++;
                            if (i < arylstValues.Count)
                                goto Found;
                            else
                                break;
                        }
                        dr = dtReport.NewRow();

                        //TO add CPT in Datatable;
                        ArrayList aryCPTList = new ArrayList();
                        ArrayList aryICdList = new ArrayList();
                        //bool Gcode = true;
                        IQuery query4 = iMySession.GetNamedQuery("GetProcedureCodeFromCpt2");
                        query4.SetString(0, Encounter_ID.ToString());
                        aryCPTList = new ArrayList(query4.List());
                        // int ucount = aryCPTList.Count;
                        string CPT1 = string.Empty;
                        if (aryCPTList.Count > 0)
                        {
                            // m:
                            for (int q = 0; q < aryCPTList.Count; q++)
                            {

                            m:

                                object[] obj = (object[])aryCPTList[q];
                                //for (int y = 0; y < obj.Length; y++)
                                //{
                                //    if (obj[y].ToString() == "G0438" || obj[y].ToString() == "G0439")
                                //    {
                                //        Gcode = false;
                                //    }
                                //    //else
                                //    //{

                                //    //}
                                //}

                                if (obj[0].ToString().Contains("G0438") || obj[0].ToString().Contains("G0439"))
                                {
                                    goto l;
                                    break;

                                }
                                else if (q == aryCPTList.Count - 1)
                                {
                                    goto l;
                                    break;
                                }
                                else
                                {

                                    q++;
                                    goto m;
                                }

                            //.Contains("G0438").ToString()||obj.Contains("G0439"))
                            //obj[q]=obj.Contains("")

                            //if (Gcode == false)
                            //{
                            l:
                                if (obj[0].ToString() == sAnnualPhysicalProcedure[0].ToString() || obj[0].ToString() == sAnnualPhysicalProcedure[1].ToString())
                                {

                                    CPT1 = Convert.ToString(obj[0]);
                                    dr["CPT1"] = CPT1;
                                    objPromedList[22] = CPT1;
                                    objPromedList[23] = Convert.ToString(obj[2]);
                                    objPromedList[25] = Convert.ToString(obj[1]);


                                    IQuery quey = iMySession.GetNamedQuery("GetIcd9");
                                    quey.SetString(0, Encounter_ID.ToString());
                                    quey.SetString(1, CPT1);
                                    if (quey.List().Count > 0)
                                    {
                                        Icd9 = Convert.ToString(quey.List()[0]);
                                        objPromedList[14] = Icd9;
                                    }
                                    else
                                        Icd9 = objPromedList[14].ToString();


                                    IQuery quer1 = iMySession.GetNamedQuery("GetV70");
                                    quer1.SetString(0, Encounter_ID.ToString());
                                    quer1.SetString(1, CPT1);
                                    aryICdList = new ArrayList(quer1.List());
                                    if (aryICdList.Count > 0)
                                    {
                                        for (int u = 0; u < aryICdList.Count; u++)
                                        {
                                            string obj1 = (string)aryICdList[u];
                                            if (obj1.ToString() == "V70.0")
                                            {
                                                v70 = obj1.ToString();
                                            }
                                        }
                                        //foreach (object[] oj in aryICdList)
                                        //{
                                        //    if(oj.Contains("V70.0").ToString)
                                        //        v70 = oj[0].ToString();


                                        //}
                                        //v70 = Convert.ToString(que1.List()[0]);
                                        //objPromedList[14] = Icd9;
                                    }

                                    aryCPTList.RemoveAt(q);
                                    Annual = true;
                                    break;
                                }
                                //}
                                else if (obj[0].ToString() != sAnnualPhysicalProcedure[0].ToString() || obj[0].ToString() == sAnnualPhysicalProcedure[1].ToString())
                                {
                                    CPT1 = Convert.ToString(obj[0]);
                                    IQuery quey = iMySession.GetNamedQuery("GetIcd9");
                                    quey.SetString(0, Encounter_ID.ToString());
                                    quey.SetString(1, CPT1);

                                    if (quey.List().Count > 0)
                                    {
                                        Icd9 = Convert.ToString(quey.List()[0]);
                                        objPromedList[14] = Icd9;
                                    }
                                    else
                                        Icd9 = objPromedList[14].ToString();

                                    Annual = false;
                                    //aryCPTList.RemoveAt(q);
                                    //  goto m;
                                    break;
                                }
                            }
                            if (v70 == string.Empty && Annual == true)
                            {
                                arlstEncWithAnnualProcedure.Add(Encounter_ID);
                                i++;
                                if (i < arylstValues.Count)
                                    goto Found;
                                else
                                    break;
                            }
                        }
                        // m:
                        if (aryCPTList.Count > 0)
                        {


                            for (int l = 0; l < aryCPTList.Count; l++)
                            {
                                //if (l == 0 || l == 1 || l == 2||l==3||l==4||l==5||l==6||l==7||l==8||l==9||l==10||l==11||l==12||l==13||l==14||l==15||
                                //    l==16||l==17||l==18||l==19||l==20||l==21||l==22||l==23||l==24||l==25||l==26||l==27||l==28||l==29||l==30||
                                //    l==31||l==32||l==33||l==34||l==35||l==36||l==37||l==38||l==39||l==40||l==41||l==42||l==43||l==44||l==45||
                                //    l==46||l==47||l==48)
                                if (l <= 48 && Annual == true)
                                {
                                    object[] obj = (object[])aryCPTList[l];
                                    dr["CPT" + (l + 2)] = Convert.ToString(obj[0]);
                                    dr["M" + (l + 2)] = Convert.ToString(obj[2]);
                                    dr["FEE" + (l + 2)] = Convert.ToString(obj[1]);
                                }
                                else if (l <= 49 && Annual == false)
                                {
                                    object[] obj = (object[])aryCPTList[l];
                                    dr["CPT" + (l + 1)] = Convert.ToString(obj[0]);
                                    dr["M" + (l + 1)] = Convert.ToString(obj[2]);
                                    dr["FEE" + (l + 1)] = Convert.ToString(obj[1]);

                                }



                                //else if (l == 3)
                                //{
                                //    goto m;
                                //    break;
                                //}
                            }
                        }
                        //m:
                        //To add All the Row values in Datatable
                        for (int j = 0; j < objPromedList.Length; j++)
                        {
                            try
                            {
                                switch (j)
                                {
                                    case 236:
                                        {
                                            dr["CURRENT PROCESS"] = objPromedList[j].ToString();
                                            break;
                                        }

                                    case 237:
                                        {
                                            dr["ENCOUNTER PROVIDER REVIEW NAME"] = objPromedList[j].ToString();
                                            break;
                                        }
                                    case 238:
                                        {
                                            dr["REVIEWLICENSE"] = objPromedList[j].ToString();
                                            break;
                                        }
                                    case 239:
                                        {
                                            dr["REVIEWNPI"] = objPromedList[j].ToString();
                                            break;
                                        }
                                    default:
                                        {
                                            dr[j] = objPromedList[j].ToString();
                                            break;
                                        }
                                }
                                //if (j == 236)

                                //else
                                //    dr[j] = objPromedList[j].ToString();
                                //if (j == 237)

                                //else
                                //    
                            }
                            catch
                            {
                                //string s;
                            }

                        }
                        int k = 0;
                        ArrayList aryDiagList = null;
                        IQuery que = iMySession.GetNamedQuery("GetICDOfAssessment");
                        que.SetString(0, Encounter_ID.ToString());
                        que.SetString(1, Icd9);
                        aryDiagList = new ArrayList(que.List());
                        ArrayList aLineItem = null;
                        IQuery query6 = iMySession.GetNamedQuery("GetHyperlinkLineItem");
                        query6.SetString(0, Encounter_ID.ToString());
                        aLineItem = new ArrayList(query6.List());
                        if (aLineItem.Count > 0)
                        {
                            object[] objLineList = (object[])aLineItem[0];
                            strLineitem = objLineList[0].ToString() + "_" + objLineList[1].ToString() + "_" + objLineList[2].ToString() + "_" + objLineList[3].ToString();
                        }
                        for (int l = 0; l < aryDiagList.Count; l++)
                        {
                            if (k == 11)
                            {
                                dr[233] = strLineitem;
                                dtReport.Rows.Add(dr);
                                dr = dtReport.NewRow();
                                for (int m = 0; m < objPromedList.Length; m++)
                                {
                                    try
                                    {
                                        switch (m)
                                        {
                                            case 236:
                                                {
                                                    dr["CURRENT PROCESS"] = objPromedList[m].ToString();
                                                    break;
                                                }

                                            case 237:
                                                {
                                                    dr["ENCOUNTER PROVIDER REVIEW NAME"] = objPromedList[m].ToString();
                                                    break;
                                                }
                                            case 238:
                                                {
                                                    dr["REVIEWLICENSE"] = objPromedList[m].ToString();
                                                    break;
                                                }
                                            case 239:
                                                {
                                                    dr["REVIEWNPI"] = objPromedList[m].ToString();
                                                    break;
                                                }

                                            default:
                                                {
                                                    dr[m] = objPromedList[m].ToString();
                                                    break;
                                                }
                                        }
                                        //if (m == 236)
                                        //    dr["CURRENT PROCESS"] = objPromedList[m].ToString();
                                        //else
                                        //    dr[m] = objPromedList[m].ToString();
                                        //if (m == 237)
                                        //    dr["ENCOUNTER_PROVIDER_REVIEW_ID"] = objPromedList[m].ToString();
                                        //else
                                        //    dr[m] = objPromedList[m].ToString();

                                    }
                                    catch
                                    {
                                        //string s;
                                    }

                                }
                                if (aryCPTList.Count > 0)
                                {
                                    for (int w = 0; w < aryCPTList.Count; w++)
                                    {
                                        //    if (w == 0 || w == 1 || w == 2 || w == 3 || w == 4 || w == 5 || w == 6 || w == 7 || w == 8 || w == 9 ||w == 10 || w == 11 || w == 12 || w == 13 || w == 14 || w == 15 ||
                                        //w == 16 || w == 17 || w == 18 || w == 19 || w == 20 || w == 21 || w == 22 || w == 23 || w == 24 || w == 25 || w == 26 || w == 27 || w == 28 || w == 29 || w == 30 ||
                                        //w == 31 || w == 32 || w == 33 || w == 34 || w == 35 || w == 36 || w == 37 || w == 38 || w == 39 || w == 40 || w == 41 || w == 42 || w == 43 || w == 44 || w == 45 ||
                                        //w == 46 || w == 47 || w == 48)
                                        if (w <= 48 && Annual == true)
                                        {
                                            object[] obj = (object[])aryCPTList[w];
                                            dr["CPT" + (w + 2)] = Convert.ToString(obj[0]);
                                            dr["M" + (w + 2)] = Convert.ToString(obj[2]);
                                            dr["FEE" + (w + 2)] = Convert.ToString(obj[1]);
                                        }
                                        else if (w <= 49 && Annual == true)
                                        {
                                            object[] obj = (object[])aryCPTList[w];
                                            dr["CPT" + (w + 1)] = Convert.ToString(obj[0]);
                                            dr["M" + (w + 1)] = Convert.ToString(obj[2]);
                                            dr["FEE" + (w + 1)] = Convert.ToString(obj[1]);

                                        }
                                    }
                                }
                                k = 0;
                            }
                            string ColumnName = "DIAG " + (k + 2).ToString();
                            try
                            {
                                dr[ColumnName] = aryDiagList[l].ToString();

                            }
                            catch
                            {
                                //string S;

                            }
                            k++;
                        }

                        dr[233] = strLineitem;
                        //dtReport.Rows.Add(dr);


                        if (aryCPTList.Count >= 49)
                        {
                            int p = 0;

                            dtReport.Rows.Add(dr);
                            dr = dtReport.NewRow();
                            //dr["CPT1"].ToString() = string.Empty;
                            if (Annual == true)
                            {
                                for (int l = 49; l < aryCPTList.Count; l++)
                                {

                                    //dtReport.Rows.Add(dr);
                                    //dr = dtReport.NewRow();
                                    //}

                                    object[] obj1 = (object[])aryCPTList[l];
                                    dr["CPT" + (p + 1)] = Convert.ToString(obj1[0]);
                                    dr["M" + (p + 1)] = Convert.ToString(obj1[2]);
                                    dr["FEE" + (p + 1)] = Convert.ToString(obj1[1]);
                                    p++;
                                }
                            }
                            else if (Annual == false)
                            {
                                for (int l = 50; l < aryCPTList.Count; l++)
                                {

                                    //dtReport.Rows.Add(dr);
                                    //dr = dtReport.NewRow();
                                    //}

                                    object[] obj1 = (object[])aryCPTList[l];
                                    dr["CPT" + (p + 1)] = Convert.ToString(obj1[0]);
                                    dr["M" + (p + 1)] = Convert.ToString(obj1[2]);
                                    dr["FEE" + (p + 1)] = Convert.ToString(obj1[1]);
                                    p++;
                                }

                            }
                            for (int j = 0; j < objPromedList.Length; j++)
                            {
                                try
                                {
                                    switch (j)
                                    {
                                        case 236:
                                            {
                                                dr["CURRENT PROCESS"] = objPromedList[j].ToString();
                                                break;
                                            }

                                        case 237:
                                            {
                                                dr["ENCOUNTER PROVIDER REVIEW NAME"] = objPromedList[j].ToString();
                                                break;
                                            }
                                        case 238:
                                            {
                                                dr["REVIEWLICENSE"] = objPromedList[j].ToString();
                                                break;
                                            }
                                        case 239:
                                            {
                                                dr["REVIEWNPI"] = objPromedList[j].ToString();
                                                break;
                                            }
                                        default:
                                            {
                                                dr[j] = objPromedList[j].ToString();
                                                break;
                                            }
                                    }
                                    //if (j == 236)
                                    //    dr["CURRENT PROCESS"] = objPromedList[j].ToString();
                                    //else
                                    //    dr[j] = objPromedList[j].ToString();
                                    //if (j == 237)
                                    //    dr["ENCOUNTER PROVIDER REVIEW ID"] = objPromedList[j].ToString();
                                    //else
                                    //    dr[j] = objPromedList[j].ToString();
                                }
                                catch
                                {
                                    //string s;
                                }

                            }
                            int u = 0;
                            ArrayList aryDiagList12 = null;
                            IQuery que12 = iMySession.GetNamedQuery("GetICDOfAssessment");
                            que12.SetString(0, Encounter_ID.ToString());
                            que12.SetString(1, Icd9);
                            aryDiagList12 = new ArrayList(que12.List());
                            ArrayList aLineItem12 = null;
                            IQuery query61 = iMySession.GetNamedQuery("GetHyperlinkLineItem");
                            query61.SetString(0, Encounter_ID.ToString());
                            aLineItem12 = new ArrayList(query61.List());
                            if (aLineItem12.Count > 0)
                            {
                                object[] objLineList = (object[])aLineItem12[0];
                                strLineitem = objLineList[0].ToString() + "_" + objLineList[1].ToString() + "_" + objLineList[2].ToString() + "_" + objLineList[3].ToString();
                            }
                            for (int l = 0; l < aryDiagList12.Count; l++)
                            {
                                if (u == 11)
                                {
                                    //dr[233] = strLineitem;
                                    //dtReport.Rows.Add(dr);
                                    //dr = dtReport.NewRow();
                                    //for (int m = 0; m < objPromedList.Length; m++)
                                    //{
                                    //    try
                                    //    {
                                    //        if (m == 236)
                                    //            dr["CURRENT PROCESS"] = objPromedList[m].ToString();
                                    //        else
                                    //            dr[m] = objPromedList[m].ToString();

                                    //    }
                                    //    catch
                                    //    {
                                    //        string s;
                                    //    }
                                    //}
                                    //if (aryCPTList.Count > 0)
                                    //{
                                    //    int r = 0;
                                    //    for (int w = 49; w < aryCPTList.Count; w++)
                                    //    {
                                    //        object[] obj = (object[])aryCPTList[w];
                                    //        dr["CPT" + (r + 2)] = Convert.ToString(obj[0]);
                                    //        dr["M" + (r + 2)] = Convert.ToString(obj[2]);
                                    //        dr["FEE" + (r + 2)] = Convert.ToString(obj[1]);
                                    //        r++;
                                    //    }
                                    //}
                                    //u = 0;
                                }
                                string ColumnName = "DIAG " + (u + 2).ToString();
                                try
                                {
                                    dr[ColumnName] = aryDiagList12[l].ToString();

                                }
                                catch
                                {
                                    //string S;

                                }
                                u++;
                            }



                        }
                        dr[233] = strLineitem;
                        dtReport.Rows.Add(dr);



                    }
                }
                iMySession.Close();
            }
            return dtReport;

        }


        //public IList<AppointmentReport> GetAppointmentReport(DateTime AppointmentDateFrom, DateTime AppointmentDateTo)
        //{
        //    ArrayList AppointmentList;
        //    //AppointmentDate = DateTime.Parse("2010-07-05 10:30:00");
        //    //durationMinutes = 20;
        //    IList<AppointmentReport> appointmentReportList = new List<AppointmentReport>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {

        //        IQuery query = iMySession.GetNamedQuery("Get.Appointment.Report");
        //        query.SetString(0, AppointmentDateFrom.ToString("yyyy-MM-dd hh:mm:ss"));
        //        query.SetString(1, AppointmentDateTo.ToString("yyyy-MM-dd hh:mm:ss"));
        //        AppointmentList = new ArrayList(query.List());
        //        AppointmentReport objAppointmentReport;

        //        //added by Ginu on 2-Aug-2010
        //        if (AppointmentList != null)
        //        {
        //            for (int i = 0; i < AppointmentList.Count; i++)
        //            {
        //                objAppointmentReport = new AppointmentReport();
        //                object[] obj = (object[])AppointmentList[i];
        //                objAppointmentReport.Encounter_Id = Convert.ToUInt64(obj[0]);
        //                objAppointmentReport.Appt_Date = Convert.ToDateTime(obj[1]);
        //                objAppointmentReport.Physician_Name = obj[2].ToString();
        //                objAppointmentReport.Patient_Name = obj[3].ToString();
        //                objAppointmentReport.Payment_Note = obj[4].ToString();
        //                objAppointmentReport.Patient_Payment = Convert.ToInt64(obj[5]);
        //                objAppointmentReport.Method_Of_Payment = obj[6].ToString();
        //                objAppointmentReport.Check_Card_Number = obj[7].ToString();
        //                objAppointmentReport.Current_process = obj[8].ToString();
        //                objAppointmentReport.Birth_Date = Convert.ToDateTime(obj[9]);


        //                //objAppointmentReport.Physician_first_name = obj[2].ToString();
        //                //objAppointmentReport.Reason_For_Cancellation = obj[4].ToString();
        //                //objAppointmentReport.Last_Name = obj[5].ToString();
        //                //objAppointmentReport.First_Name = obj[6].ToString();
        //                //objAppointmentReport.Birth_Date = Convert.ToDateTime(obj[7]);
        //                //objAppointmentReport.Purpose_Of_Visit = obj[8].ToString();




        //                appointmentReportList.Add(objAppointmentReport);
        //            }
        //        }
        //        iMySession.Close();
        //    }
        //    return appointmentReportList;
        //}


        //public FillAppointmentReport CensusReport(DateTime dtFromdate, DateTime dtTodate)
        //{
        //    FillAppointmentReport objAppointReport = new FillAppointmentReport();
        //    ArrayList aryCensusRepot = null;
        //    IList<AppointmentReport> objAppointmentList = new List<AppointmentReport>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {

        //        IQuery query = iMySession.GetNamedQuery("Get.GetCensusReport");
        //        query.SetDateTime(0, Convert.ToDateTime(dtFromdate.ToString("yyyy-MM-dd")));
        //        query.SetDateTime(1, Convert.ToDateTime(dtTodate.ToString("yyyy-MM-dd")));
        //        aryCensusRepot = new ArrayList(query.List());

        //        //added by Ginu on 2-Aug-2010

        //        if (aryCensusRepot != null)
        //        {
        //            for (int i = 0; i < aryCensusRepot.Count; i++)
        //            {
        //                object[] objCensus = (object[])(aryCensusRepot[i]);
        //                AppointmentReport objAppointmentReport = new AppointmentReport();
        //                objAppointmentReport.Encounter_Id = Convert.ToUInt64(objCensus[0]);
        //                objAppointmentReport.Appt_Date = Convert.ToDateTime(objCensus[1]);
        //                objAppointmentReport.Purpose_Of_Visit = objCensus[10].ToString();
        //                objAppointmentReport.Patient_Note = objCensus[11].ToString();
        //                objAppointmentReport.Current_process = objCensus[12].ToString();
        //                objAppointmentReport.Physician_ID = Convert.ToUInt64(objCensus[13]);
        //                objAppointmentList.Add(objAppointmentReport);

        //            }
        //        }
        //        iMySession.Close();
        //    }
        //    return objAppointReport;
        //}

        public DataSet DtblProductivityReport(DateTime From_Date, DateTime To_Date)
        {
            DataSet dsreport = new DataSet();
            DataTable dtreport = new DataTable();


            DataTable dtProductivityReport = new DataTable();
            ArrayList arylstProductivityReport = null;
            ArrayList arylstProductivityReportColumns = null;
            // ArrayList aryreportlistBYProcess = null;
            // ArrayList Rowcount = null;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.GetProductivityReport.ColumnHeadings");
                arylstProductivityReportColumns = new ArrayList(query2.List());
                //string  from_Date ="2010-11-04";
                //string to_Date = "2011-11-04";
                IQuery query1 = iMySession.GetNamedQuery("Reports.ProductivityReport");
                //query1.SetString(0, from_Date);
                //query1.SetString(1, to_Date);
                query1.SetString(0, From_Date.ToString("yyyy-MM-dd"));
                query1.SetString(1, To_Date.ToString("yyyy-MM-dd"));
                arylstProductivityReport = new ArrayList(query1.List());

                //Fill Main Columns
                object[] objlst = (object[])arylstProductivityReportColumns[0];
                for (int i = 0; i < objlst.Length; i++)
                {
                    if (i == 4 || i == 7)
                    {
                        dtProductivityReport.Columns.Add(objlst[i].ToString(), typeof(System.DateTime));
                    }
                    else
                    {
                        dtProductivityReport.Columns.Add(objlst[i].ToString(), typeof(System.String));
                    }
                }
                iMySession.Close();
            }
            ArrayList aryreportlist = new ArrayList();
            OrderByPRocess("PROVIDER_PROCESS", arylstProductivityReport, aryreportlist);
            int PROVIDER_PROCESS_COUNT = aryreportlist.Count;
            FillDataTableRws(aryreportlist, dtProductivityReport, PROVIDER_PROCESS_COUNT, "Provider_Process");
            aryreportlist.Clear();

            //changed physician_Correction to CODER_REVIEW_CORRECTION
            OrderByPRocess("CODER_REVIEW_CORRECTION", arylstProductivityReport, aryreportlist);
            int PHYSICIAN_CORRECTION_COUNT = aryreportlist.Count;
            //changed physician_Correction to CODER_REVIEW_CORRECTION
            FillDataTableRws(aryreportlist, dtProductivityReport, PHYSICIAN_CORRECTION_COUNT, "coder_Review_Correction");
            aryreportlist.Clear();

            OrderByPRocess("REVIEW_CODING", arylstProductivityReport, aryreportlist);
            int REVIEW_CODING_COUNT = aryreportlist.Count;
            FillDataTableRws(aryreportlist, dtProductivityReport, REVIEW_CODING_COUNT, "Review_Coding");
            aryreportlist.Clear();

            OrderByPRocess("REVIEW_CODING_2", arylstProductivityReport, aryreportlist);
            int REVIEW_CODING_2_COUNT = aryreportlist.Count;
            FillDataTableRws(aryreportlist, dtProductivityReport, REVIEW_CODING_2_COUNT, "Review_Coding_2");
            aryreportlist.Clear();

            OrderByPRocess("PROMED_REPORT", arylstProductivityReport, aryreportlist);
            int PROMED_REPORT_COUNT = aryreportlist.Count;
            FillDataTableRws(aryreportlist, dtProductivityReport, PROMED_REPORT_COUNT, "Promed_Report");
            aryreportlist.Clear();

            OrderByPRocess("BILLING_COMPLETE", arylstProductivityReport, aryreportlist);
            int BILLING_COMPLETE_COUNT = aryreportlist.Count;
            FillDataTableRws(aryreportlist, dtProductivityReport, BILLING_COMPLETE_COUNT, "Billing_Complete");
            aryreportlist.Clear();
            dsreport.Tables.Add(dtProductivityReport);

            return dsreport;


        }


        public void OrderByPRocess(string process, ArrayList arylstProductivityReport, ArrayList aryreportlist)
        {
            for (int i = 0; i < arylstProductivityReport.Count; i++)
            {
                object[] obj = (object[])arylstProductivityReport[i];
                if (obj[5].ToString() == process)
                {
                    aryreportlist.Add(obj.ToArray());
                }

            }
        }

        public void FillDataTableRws(ArrayList listDatas, DataTable dtProductivityReport, int count, string process_name)
        {
            int Duration;

            DataRow dr = dtProductivityReport.NewRow();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                for (int i = 0; i < listDatas.Count; i++)
                {
                    object[] objProductivity = (object[])listDatas[i];

                    IQuery query1 = iMySession.GetNamedQuery("Reports.ProductivityReport.GetDuration");
                    query1.SetString(0, objProductivity[8].ToString());
                    query1.SetString(1, objProductivity[5].ToString());
                    Duration = Convert.ToInt32(query1.List()[0]);
                    objProductivity[8] = Duration;

                    dr = dtProductivityReport.NewRow();
                    for (int j = 0; j < objProductivity.Length; j++)
                    {
                        try
                        {
                            dr[j] = objProductivity[j].ToString();
                        }
                        catch
                        {
                            //string s;
                        }
                    }
                    dtProductivityReport.Rows.Add(dr);
                }
                iMySession.Close();
                dr = dtProductivityReport.NewRow();
                dr[0] = "Total Number of " + process_name + "  =  " + count;
                dtProductivityReport.Rows.Add(dr);
                iMySession.Close();
            }
        }


        public DataSet DtblAssessmentDIFFReport(DateTime From_Date, DateTime To_Date, int Tagvalue)
        {
            string CurrentProcess1, CurrentProcess2, CurrentProcess3;
            DataSet DsDiffReport = new DataSet();

            DataTable DtblDiffreport = new DataTable();
            ArrayList arylstAssessmentDIFFColumns;
            ArrayList arylstAssessmentDIFF;
            ArrayList arylstICDCodes;
            ArrayList arylstProblemlistType;
            ArrayList arylstDateofService;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                IQuery query2 = iMySession.GetNamedQuery("Fill.GetAssessmentDiffReport.ColumnHeadings");
                arylstAssessmentDIFFColumns = new ArrayList(query2.List());
                if (arylstAssessmentDIFFColumns.Count > 0)
                {
                    object[] objlstColumns = (object[])arylstAssessmentDIFFColumns[0];

                    for (int i = 0; i < objlstColumns.Length; i++)
                    {
                        if (i == 3 || i == 6)
                        {
                            DtblDiffreport.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                        }
                        else
                        {
                            DtblDiffreport.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                        }
                    }
                }

                IQuery query1 = iMySession.GetNamedQuery("Reports.AssessmentDiffReport");
                if (Tagvalue == 1)
                {
                    CurrentProcess1 = "REVIEW_CODING";

                    query1.SetString(0, CurrentProcess1 + "%");
                    query1.SetString(1, CurrentProcess1 + "%");
                    query1.SetString(2, CurrentProcess1 + "%");
                    query1.SetString(3, From_Date.ToString("yyyy-MM-dd"));
                    query1.SetString(4, To_Date.ToString("yyyy-MM-dd"));
                }
                else if (Tagvalue == 2)
                {
                    CurrentProcess1 = "PROMED_REPORT";
                    CurrentProcess2 = "BILLING_COMPLETE";

                    query1.SetString(0, CurrentProcess1 + "%");
                    query1.SetString(1, CurrentProcess1 + "%");
                    query1.SetString(2, CurrentProcess2 + "%");
                    query1.SetString(3, From_Date.ToString("yyyy-MM-dd"));
                    query1.SetString(4, To_Date.ToString("yyyy-MM-dd"));
                }
                else if (Tagvalue == 3)
                {
                    CurrentProcess1 = "PROMED_REPORT";
                    CurrentProcess2 = "BILLING_COMPLETE";
                    CurrentProcess3 = "REVIEW_CODING";

                    query1.SetString(0, CurrentProcess1 + "%");
                    query1.SetString(1, CurrentProcess2 + "%");
                    query1.SetString(2, CurrentProcess3 + "%");
                    query1.SetString(3, From_Date.ToString("yyyy-MM-dd"));
                    query1.SetString(4, To_Date.ToString("yyyy-MM-dd"));

                }

                arylstAssessmentDIFF = new ArrayList(query1.List());
                for (int i = 0; i < arylstAssessmentDIFF.Count; i++)
                {
                    object[] objlst = (object[])arylstAssessmentDIFF[i];
                    ulong Encounter_ID = Convert.ToUInt64(objlst[0].ToString());
                    ulong Human_ID = Convert.ToUInt64(objlst[1].ToString());
                    IQuery query4 = iMySession.GetNamedQuery("Reports.AssessmentDiffReport.ICDCodes");
                    query4.SetString(0, Encounter_ID.ToString());
                    query4.SetString(1, Encounter_ID.ToString());
                    arylstICDCodes = new ArrayList(query4.List());
                    object[] objlst1 = new object[arylstICDCodes.Count];
                    for (int l = 0; l < arylstICDCodes.Count; l++)
                    {

                        objlst1 = (object[])arylstICDCodes[l];

                        if (objlst1[4].ToString() == "No")
                        {
                            IQuery query7 = iMySession.GetNamedQuery("Reports.AssessmentDiffReport.ICDCodes.GetReferenceSource.HumanID");
                            query7.SetString(0, objlst1[1].ToString());
                            query7.SetString(1, Human_ID.ToString());
                            arylstProblemlistType = new ArrayList(query7.List());
                        }
                        else
                        {
                            IQuery query7 = iMySession.GetNamedQuery("Reports.AssessmentDiffReport.ICDCodes.GetReferenceSource.EncounterID");
                            query7.SetString(0, objlst1[1].ToString());
                            query7.SetString(1, Encounter_ID.ToString());
                            arylstProblemlistType = new ArrayList(query7.List());
                        }
                        if (arylstProblemlistType.Count == 0)
                        {
                            objlst1[3] = "NONE";
                        }
                        else if (arylstProblemlistType[0].ToString() == "medadv_problem_list")
                        {
                            objlst1[3] = "Med_Adv_Problem_List";
                        }
                        else if (arylstProblemlistType[0].ToString() == "Assessment")
                        {
                            objlst1[3] = "Added_By_Physician";
                        }
                        if (objlst1[3].ToString() == "Med_Adv_Problem_List" && objlst1[4].ToString() == "No")
                        {
                            IQuery query9 = iMySession.GetNamedQuery("Reports.AssessmentDiffReport.ICDCodes.DateOfService.MedAdv");
                            query9.SetString(0, Human_ID.ToString());
                            query9.SetString(1, objlst1[1].ToString());
                            arylstDateofService = new ArrayList(query9.List());
                        }
                        else
                        {
                            IQuery query10 = iMySession.GetNamedQuery("Reports.AssessmentDiffReport.ICDCodes.DateOfService.Encounter");
                            query10.SetString(0, Encounter_ID.ToString());
                            arylstDateofService = new ArrayList(query10.List());

                        }
                        objlst1[0] = arylstDateofService[0];
                        DataRow dr = DtblDiffreport.NewRow();
                        if (objlst.Length > 0)
                        {
                            for (int k = 0; k < objlst.Length; k++)
                            {
                                dr[k] = objlst[k].ToString();
                            }
                        }
                        int m = 0;
                        if (objlst1.Length > 0)
                        {
                            for (int j = 6; j < 11; j++)
                            {
                                dr[j] = objlst1[m].ToString();
                                m++;
                            }
                        }
                        DtblDiffreport.Rows.Add(dr);
                    }
                    //DataRow drow = DtblDiffreport.NewRow();
                    //if (objlst.Length > 0)
                    //{
                    //    for (int k = 0; k < objlst.Length; k++)
                    //    {
                    //        drow[k] = objlst[k].ToString();
                    //    }
                    //}
                    //int h = 0;
                    //if (objlst1.Length > 0)
                    //{
                    //    for (int j = 6; j < 11; j++)
                    //    {
                    //        drow[j] = objlst1[h].ToString();
                    //        h++;
                    //    }
                    //}
                    //DtblDiffreport.Rows.Add(drow);
                }
                DsDiffReport.Tables.Add(DtblDiffreport);
                DtblDiffreport.Columns.Remove("Encounter ID");
                iMySession.Close();
            }
            return DsDiffReport;
        }

        int iTryCount = 0;
        //public void BatchInsertUpdateCheckOut(IList<Eligibility_Verification> InsertEligList, IList<Encounter> UpdateEncList, IList<VisitPayment> InsertVisitList, IList<Check> SaveCheckList, IList<PPHeader> SavePPHeaderList, IList<PPLineItem> SavePPLineItemList, IList<AccountTransaction> SaveAccountList, string MACAddress)
        //{
        //    iTryCount = 0;
        //    ulong ulObjSysId = 0;

        //TryAgain:
        //    int iResult = 0;

        //    ISession MySession = Session.GetISession();
        //    //ITransaction trans = null;
        //    try
        //    {
        //        using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
        //        {
        //            try
        //            {
        //                // trans = MySession.BeginTransaction();
        //                IList<Encounter> EncInsertList = null;
        //                if (UpdateEncList != null && UpdateEncList.Count > 0)
        //                {
        //                    iResult = SaveUpdateDeleteWithoutTransaction(ref EncInsertList, UpdateEncList, null, MySession, MACAddress);

        //                    if (iResult == 2)
        //                    {
        //                        if (iTryCount < 5)
        //                        {
        //                            iTryCount++;
        //                            goto TryAgain;
        //                        }
        //                        else
        //                        {
        //                            trans.Rollback();
        //                            //  MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        // MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                }

        //                if (InsertEligList != null && InsertEligList.Count > 0)
        //                {
        //                    Eligibility_VerficationManager ObjElig = new Eligibility_VerficationManager();
        //                    iResult = ObjElig.SaveEligWithoutTransaction(InsertEligList, MySession, MACAddress);

        //                    if (iResult == 2)
        //                    {
        //                        if (iTryCount < 5)
        //                        {
        //                            iTryCount++;
        //                            goto TryAgain;
        //                        }
        //                        else
        //                        {
        //                            trans.Rollback();
        //                            //  MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        //  MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                }

        //                if (InsertVisitList != null && InsertVisitList.Count > 0)
        //                {
        //                    VisitPaymentManager ObjVisit = new VisitPaymentManager();
        //                    iResult = ObjVisit.SaveVisitPaymentWithoutTransaction(InsertVisitList, MySession, MACAddress);

        //                    if (iResult == 2)
        //                    {
        //                        if (iTryCount < 5)
        //                        {
        //                            iTryCount++;
        //                            goto TryAgain;
        //                        }
        //                        else
        //                        {
        //                            trans.Rollback();
        //                            //  MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        //  MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }

        //                    CheckManager objCheck = new CheckManager();
        //                    iResult = objCheck.SaveCheckWithoutTransaction(SaveCheckList, MySession, MACAddress);

        //                    if (iResult == 2)
        //                    {
        //                        if (iTryCount < 5)
        //                        {
        //                            iTryCount++;
        //                            goto TryAgain;
        //                        }
        //                        else
        //                        {
        //                            trans.Rollback();
        //                            //MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        // MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }

        //                    SavePPHeaderList[0].Payment_ID = SaveCheckList[0].Payment_ID;
        //                    SavePPHeaderList[0].Check_Table_Int_ID = SaveCheckList[0].Id;

        //                    PPHeaderManager objPPHeader = new PPHeaderManager();
        //                    iResult = objPPHeader.SavePPHeaderWithoutTransaction(SavePPHeaderList, MySession, MACAddress);

        //                    if (iResult == 2)
        //                    {
        //                        if (iTryCount < 5)
        //                        {
        //                            iTryCount++;
        //                            goto TryAgain;
        //                        }
        //                        else
        //                        {
        //                            trans.Rollback();
        //                            //MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        // MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }

        //                    SavePPLineItemList[0].PP_Header_ID = SavePPHeaderList[0].Id;
        //                    SavePPLineItemList[0].Payment_ID = SaveCheckList[0].Payment_ID;
        //                    SavePPLineItemList[0].Check_Table_Int_ID = SaveCheckList[0].Id;

        //                    PPLineItemManager objPPLineItem = new PPLineItemManager();
        //                    iResult = objPPLineItem.SavePPLineItemWithoutTransaction(SavePPLineItemList, null, MySession, MACAddress);

        //                    if (iResult == 2)
        //                    {
        //                        if (iTryCount < 5)
        //                        {
        //                            iTryCount++;
        //                            goto TryAgain;
        //                        }
        //                        else
        //                        {
        //                            trans.Rollback();
        //                            // MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        //MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }

        //                    SaveAccountList[0].Bill_To_PP_Line_Item_ID = SavePPLineItemList[0].Id;
        //                    SaveAccountList[0].PP_Header_ID = SavePPHeaderList[0].Id;
        //                    SaveAccountList[0].Payment_ID = SaveCheckList[0].Payment_ID;
        //                    SaveAccountList[0].Check_Table_Int_ID = SaveCheckList[0].Id;

        //                    AccountTransactionManager objAccount = new AccountTransactionManager();
        //                    iResult = objAccount.SaveAccountWithoutTransaction(SaveAccountList, MySession, MACAddress);

        //                    if (iResult == 2)
        //                    {
        //                        if (iTryCount < 5)
        //                        {
        //                            iTryCount++;
        //                            goto TryAgain;
        //                        }
        //                        else
        //                        {
        //                            trans.Rollback();
        //                            // MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        //MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                }

        //                MySession.Flush();
        //                trans.Commit();
        //            }

        //            catch (NHibernate.Exceptions.GenericADOException ex)
        //            {
        //                trans.Rollback();
        //                // MySession.Close();
        //                throw new Exception(ex.Message);
        //            }
        //            catch (Exception e)
        //            {
        //                trans.Rollback();
        //                //MySession.Close();
        //                throw new Exception(e.Message);
        //            }

        //            finally
        //            {
        //                MySession.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex1)
        //    {
        //        //MySession.Close();
        //        throw new Exception(ex1.Message);
        //    }
        //}
        //vinoth branch 3_0_43

        public FillPatientSummaryBarDTO LoadPatientSummaryBar(ulong EncounterId, ulong HumanId, DateTime LocalDate, string UserName, string sLegalorg)
        {
            FillPatientSummaryBarDTO objPatientSummary = new FillPatientSummaryBarDTO();
            //  ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ProblemListManager objProbListMgr = new ProblemListManager();
                objPatientSummary.PblmMedList = objProbListMgr.GetProblemDescriptionList(HumanId);
                ulong Physician_ID = 0;
                if (EncounterId == 0)
                {
                    ICriteria critEncounter = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", EncounterId)).AddOrder(Order.Desc("Id"));
                    if (critEncounter.List<Encounter>().Count > 0)
                    {
                        EncounterId = critEncounter.List<Encounter>()[0].Id;
                    }

                }
                else
                {
                    ICriteria critEncounter = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", EncounterId)).AddOrder(Order.Desc("Id"));
                    if (critEncounter.List<Encounter>().Count > 0)
                    {
                        Physician_ID = (ulong)critEncounter.List<Encounter>()[0].Encounter_Provider_ID;
                    }
                }
                ICriteria crit = iMySession.CreateCriteria(typeof(ChiefComplaints)).Add(Expression.Eq("Encounter_ID", EncounterId)).Add(Expression.Eq("HPI_Element", "Chief Complaints"));
                objPatientSummary.ChiefComplaintList = crit.List<ChiefComplaints>();

                //object Vitals_Id = session.GetISession().CreateCriteria(typeof(Vitals)).Add(Expression.Eq("Human_ID", HumanId))
                //.SetProje3ction(Projections.ProjectionList().Add(Projections.Max("Vitals_Taken_Date"))).List<object>()[0];

                //crit = session.GetISession().CreateCriteria(typeof(Vitals)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Vitals_Taken_Date", Convert.ToDateTime(Vitals_Id)));
                //objPatientSummary.VitalsList = crit.List<Vitals>();
                //15-12-2011 by Vinoth
                //ISQLQuery sqlquery = session.GetISession().CreateSQLQuery("select * from vitals where Human_Id=" + HumanId + " group by Vital_Name having Max(Vitals_Taken_Date)").AddEntity(typeof(Vitals));
                //objPatientSummary.VitalsList = sqlquery.List<Vitals>();
                //IList<DynamicScreen> DynamicscreenList=new List<DynamicScreen>();
                //ICriteria crt1 = session.GetISession().CreateCriteria(typeof(DynamicScreen));
                //DynamicscreenList = crt1.List<DynamicScreen>();

                //IList<PatientResults> templist = new List<PatientResults>();
                //for (int k = 0; k < DynamicscreenList.Count; k++)
                //{
                //    if()
                //    ISQLQuery sqlquery = session.GetISession().CreateSQLQuery("select v.* from patient_results v inner join dynamic_screen s on (v.Loinc_Observation=s.control_Name )  where v.human_id='" + HumanId + "' and v.Results_Type='Vitals' and loinc_observation = '"+DynamicscreenList[k].Control_Name+"' and  v.Captured_date_and_time in(select max(Captured_date_and_time)   from patient_results where human_id=v.human_id and loinc_observation ='"+DynamicscreenList[k].Control_Name+"') group by v.Loinc_Observation order by s.sort_order").AddEntity("v", typeof(PatientResults));
                // //   objPatientSummary.VitalsList = sqlquery.List<PatientResults>();
                //    foreach (IList<object> l in sqlquery.List())
                //    {
                //        PatientResults objResult = (PatientResults)l[0];
                //        templist.Add(objResult);

                //    }

                //}
                //objPatientSummary.VitalsList = templist;


                //ISQLQuery sqlquery = session.GetISession().CreateSQLQuery("select v.* from patient_results v inner join dynamic_screen s on (v.Loinc_Observation=s.control_Name )  where v.human_id='"+HumanId+"' and v.Results_Type='Vitals' and loinc_observation = v.loinc_observation and  v.Captured_date_and_time in(select max(Captured_date_and_time)   from patient_results where human_id=v.human_id and loinc_observation = v.loinc_observation) group by v.Loinc_Observation order by s.sort_order").AddEntity("v", typeof(PatientResults));
                //objPatientSummary.VitalsList = sqlquery.List<PatientResults>();

                //pravin 2012-07-18
                IList<PatientResults> Duptemplist = new List<PatientResults>();
                IQuery query1 = iMySession.GetNamedQuery("Fill.Vitals");
                query1.SetString(0, Convert.ToString(HumanId));
                query1.SetString(1, Convert.ToString(EncounterId));
                if (query1.List().Count > 0)
                    objPatientSummary.Vitals = Convert.ToInt32(query1.List()[0]);

                if (objPatientSummary.Vitals == 1)
                {
                    ISQLQuery sqlquery = iMySession.CreateSQLQuery("select v.* from patient_results v inner join dynamic_screen s on (v.Loinc_Observation=s.control_Name) inner join map_vitals_physician m on (s.master_vitals_id=m.master_vitals_id)  where v.human_id='" + HumanId + "' and m.physician_id='" + Physician_ID + "' and v.Results_Type='Vitals' and v.encounter_id='" + EncounterId + "' and loinc_observation = v.loinc_observation and  v.Captured_date_and_time in(select max(Captured_date_and_time)   from patient_results where human_id=v.human_id and encounter_id=v.encounter_id and loinc_observation = v.loinc_observation) group by v.Loinc_Observation order by m.sort_order").AddEntity("v", typeof(PatientResults));
                    objPatientSummary.VitalsList = sqlquery.List<PatientResults>();
                }
                else if (objPatientSummary.Vitals > 1)
                {
                    IList<PatientResults> latestResults = new List<PatientResults>();


                    ISQLQuery sqlquery = iMySession.CreateSQLQuery("select v.* from patient_results v where v.human_id='" + HumanId + "' and v.encounter_id='" + EncounterId + "' and v.value <> '' and  v.Results_Type='Vitals' order by v.loinc_observation,v.Captured_date_and_time desc,v.vitals_group_id desc").AddEntity("v", typeof(PatientResults));
                    latestResults = sqlquery.List<PatientResults>();
                    IList<PatientResults> templist = new List<PatientResults>();
                    int k = 0;
                    for (int i = 0; i < latestResults.Count - 1; i++)
                    {
                        if (k == 0)
                        {
                            if (latestResults[i].Loinc_Observation == latestResults[i + 1].Loinc_Observation)
                            {
                                Duptemplist.Add(latestResults[i]);
                                k = 1;
                            }
                            else
                            {
                                Duptemplist.Add(latestResults[i]);
                                k = 0;
                            }
                        }
                        else if (latestResults[i].Loinc_Observation != latestResults[i + 1].Loinc_Observation)
                        {
                            if (!templist.Contains(latestResults[i]))
                                templist.Add(latestResults[i]);
                            k = 0;
                        }
                    }






                    VitalsManager objVitalsManger = new VitalsManager();


                    ISQLQuery sqlqueryHuman = iMySession.CreateSQLQuery("select h.* from human h   where h.Human_ID='" + HumanId + "'").AddEntity("h", typeof(Human));
                    IList<Human> lstHuman = sqlqueryHuman.List<Human>();

                    ISQLQuery sqlqueryUser = iMySession.CreateSQLQuery("select u.* from user u   where u.User_Name='" + UserName + "'").AddEntity("u", typeof(User));
                    IList<User> lstUser = sqlqueryUser.List<User>();

                    double humanAgeInMonths = 0;
                    humanAgeInMonths = ((DateTime.Now.Date - lstHuman[0].Birth_Date.Date).TotalDays) / 30.4375;

                    IList<object> objList = objVitalsManger.GetLocalData(lstUser[0].Physician_Library_ID, Convert.ToInt16(humanAgeInMonths.ToString().Split('.')[0]), lstHuman[0].Sex, "'BMI-AGE','HC-AGE'", 2000);
                    IList<DynamicScreen> dynamicScreenList = new List<DynamicScreen>();
                    if (objList != null && objList.Count > 0)
                    {
                        if (objList[1] != null && objList[1].GetType().ToString().ToUpper().Contains("DYNAMICSCREEN"))
                            dynamicScreenList = (IList<DynamicScreen>)objList[1];
                    }

                    var Result = from a in Duptemplist join b in dynamicScreenList on a.Loinc_Observation equals b.Control_Name orderby b.Sort_Order select a;



                    objPatientSummary.VitalsList = Result.ToList<PatientResults>();


                }




                //crit = session.GetISession().CreateCriteria(typeof(NonDrugAllergy)).Add(Expression.Eq("Encounter_Id",EncounterId)).Add(Expression.Eq("Is_Present", "Y"));
                //objPatientSummary.NonDrugAllergyList = crit.List<NonDrugAllergy>();


                //Added For bug id:27574
                crit = iMySession.CreateCriteria(typeof(NonDrugAllergy)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Encounter_Id", EncounterId)).Add(Expression.Eq("Is_Present", "Y"));
                if (crit.List<NonDrugAllergy>() != null && crit.List<NonDrugAllergy>().Count > 0)
                {
                    objPatientSummary.NonDrugAllergyList = crit.List<NonDrugAllergy>();
                }
                else
                {
                    crit = iMySession.CreateCriteria(typeof(NonDrugAllergy)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Is_Present", "Y"));
                    if (crit.List<NonDrugAllergy>() != null && crit.List<NonDrugAllergy>().Count > 0)
                    {
                        IList<NonDrugAllergy> tempNonDrugAllergy = new List<NonDrugAllergy>();
                        tempNonDrugAllergy = crit.List<NonDrugAllergy>();
                        tempNonDrugAllergy = tempNonDrugAllergy.Where(a => a.Encounter_Id < EncounterId).ToList<NonDrugAllergy>();
                        ulong maxEncounter = 0;
                        if (tempNonDrugAllergy != null && tempNonDrugAllergy.Count > 0)
                            maxEncounter = tempNonDrugAllergy.Max(b => b.Encounter_Id);
                        objPatientSummary.NonDrugAllergyList = tempNonDrugAllergy.Where(a => a.Encounter_Id == maxEncounter).ToList<NonDrugAllergy>();
                    }
                }


                //crit = session.GetISession().CreateCriteria(typeof(Rcopia_Medication)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Le("Stop_Date",Convert.ToDateTime(LocalDate.ToString("yyyy-MM-dd"))));//.AddOrder(Order.Desc("To_Date"));
                //objPatientSummary.MedicationList = crit.List<Rcopia_Medication>();
                //ISQLQuery querysql = session.GetISession().CreateSQLQuery("SELECT * FROM rcopia_medication  where human_id='" + HumanId + "' and (date_format(stop_date,'%Y-%m-%d')>'" + LocalDate.ToString("yyyy-MM-dd") + "' or stop_date='0001-01-01 00:00:00')").AddEntity(typeof(Rcopia_Medication));
                //objPatientSummary.MedicationList = querysql.List<Rcopia_Medication>();
                //Added By Mohan on 10-feb-2012
                //IList<string> MedLst = session.GetISession().CreateSQLQuery("SELECT Concat(if(r.Brand_Name!=r.Generic_Name, concat(r.Brand_Name,'( ',r.Generic_Name,' ) '),concat(r.Generic_Name,' ')),r.Strength,' ', r.Form,' : ', r.Dose,' ', r.Dose_Unit,' ', r.Route,' ', r.Dose_Timing,' ',m.Dosage,' ',m.Route_Of_Administration,' ',m.Frequency,' ',m.Drug_Name) a FROM rcopia_medication r join med_history m on (m.Human_ID = r.Human_ID) where r.Human_ID=" + HumanId + " and Deleted='N' group by a").List<string>();

                //pravin
                IList<Encounter> EncounterLst = new List<Encounter>();
                //IList<Encounter> resultList = new List<Encounter>();
                IList<DateTime> resultListDate = new List<DateTime>();
                IList<ulong> resultListID = new List<ulong>();
                ISQLQuery sqlquery1 = iMySession.CreateSQLQuery("select e.* from encounter e where   e.Human_ID=" + HumanId + " order by e.date_of_service desc ").AddEntity("e", typeof(Encounter));
                EncounterLst = sqlquery1.List<Encounter>();
                IList<string> EncounterID = new List<string>();
                IList<string> EncounterDate = new List<string>();
                DateTime CurrentDOS = DateTime.MinValue;
                if (EncounterId != 0)
                {
                    for (int i = 0; i < EncounterLst.Count; i++)
                    {

                        if (EncounterLst[i].Id == EncounterId)
                        {
                            CurrentDOS = EncounterLst[i].Date_of_Service;
                        }
                    }
                }

                for (int k = 0; k < EncounterLst.Count; k++)
                {
                    if (EncounterID.Count < 2)
                    {
                        if (EncounterLst[k].Date_of_Service <= CurrentDOS)
                        {
                            if (CurrentDOS.ToString() != DateTime.MinValue.ToString())
                            {
                                if (EncounterLst[k].Date_of_Service.ToString() != DateTime.MinValue.ToString())
                                {

                                    EncounterID.Add(EncounterLst[k].Id.ToString());
                                    //EncounterDate.Add(EncounterLst[k].Date_of_Service.ToString());
                                    //Encounter objEncounter = new Encounter();
                                    //objEncounter = EncounterLst[k];    
                                    resultListID.Add(EncounterLst[k].Id);
                                }
                                if (EncounterLst[k].Date_of_Service.ToString() != DateTime.MinValue.ToString())
                                {

                                    // EncounterID.Add(EncounterLst[k].Id.ToString());
                                    EncounterDate.Add(EncounterLst[k].Date_of_Service.ToString());
                                    //Encounter objEncounter = new Encounter();
                                    //objEncounter = EncounterLst[k];
                                    resultListDate.Add(EncounterLst[k].Date_of_Service);
                                }
                            }
                        }

                    }
                }

                if (resultListDate.Count != 0)
                {
                    objPatientSummary.EncounterDateList = resultListDate;
                    //objPatientSummary.EncounterIDList = resultList;
                }
                if (resultListID.Count != 0)
                {
                    //objPatientSummary.EncounterDateList = resultList;
                    objPatientSummary.EncounterIDList = resultListID;
                }

                //cast(r.stop_date as char(100)),cast(r.start_date as char(100)),Concat(if(r.Brand_Name!=r.Generic_Name, concat(r.Brand_Name,'( ',r.Generic_Name,' ) '),concat(r.Generic_Name,' ')),r.Strength,' ', r.Form,' : ', r.Dose,' ', r.Dose_Unit,' ', r.Route,' ', r.Dose_Timing) a

                ICriteria criteria = iMySession.CreateCriteria(typeof(Rcopia_Medication)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Deleted", "N"));

                objPatientSummary.MedicationList = criteria.List<Rcopia_Medication>();

                //  IList<Rcopia_Medication> MedLst = session.GetISession().CreateSQLQuery("SELECT *  FROM rcopia_medication  where Human_ID=" + HumanId + " and Deleted='N' and Stop_Reason ='' ").List<Rcopia_Medication>();
                IList<string> Medication_history = iMySession.CreateSQLQuery("Select Concat(m.Drug_Name,' ',m.Dosage,' ',m.Route_Of_Administration,' ',m.Frequency) from med_history m where  m.Human_ID=" + HumanId + "").List<string>();

                IList<string> MedHistoryLst = new List<string>();

                foreach (string item in Medication_history)
                    MedHistoryLst.Add(item);

                objPatientSummary.MedHistoryList = MedHistoryLst;

                crit = iMySession.CreateCriteria(typeof(Rcopia_Allergy)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Deleted", "N"));
                objPatientSummary.AllergyList = crit.List<Rcopia_Allergy>();
                iMySession.Close();

            }
            return objPatientSummary;
        }

        //Saravanan Loadpatientsummary
        public FillPatientSummaryBarDTO LoadPatientSummaryBarUsingList(ulong EncounterId, ulong HumanId, DateTime LocalDate, string UserName, IList<int> ilstChangeSummaryBar, string sLegalorg)
        {
            FillPatientSummaryBarDTO objPatientSummary = new FillPatientSummaryBarDTO();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {


                ulong Physician_ID = 0;
                if (EncounterId == 0)
                {
                    ICriteria critEncounter = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", EncounterId)).AddOrder(Order.Desc("Id"));
                    if (critEncounter.List<Encounter>().Count > 0)
                    {
                        EncounterId = critEncounter.List<Encounter>()[0].Id;
                    }

                }
                else
                {
                    ICriteria critEncounter = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", EncounterId)).AddOrder(Order.Desc("Id"));
                    if (critEncounter.List<Encounter>().Count > 0)
                    {
                        Physician_ID = (ulong)critEncounter.List<Encounter>()[0].Encounter_Provider_ID;
                    }
                }

                for (int iValue = 0; iValue < ilstChangeSummaryBar.Count; iValue++)
                {
                    switch (ilstChangeSummaryBar[iValue])
                    {
                        case 1:
                            ICriteria crit = iMySession.CreateCriteria(typeof(Rcopia_Allergy)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Deleted", "N"));
                            objPatientSummary.AllergyList = crit.List<Rcopia_Allergy>();
                            crit = iMySession.CreateCriteria(typeof(NonDrugAllergy)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Encounter_Id", EncounterId)).Add(Expression.Eq("Is_Present", "Y"));
                            if (crit.List<NonDrugAllergy>() != null && crit.List<NonDrugAllergy>().Count > 0)
                            {
                                objPatientSummary.NonDrugAllergyList = crit.List<NonDrugAllergy>();
                            }

                            break;
                        case 2:
                            IList<PatientResults> Duptemplist = new List<PatientResults>();
                            IQuery query1 = iMySession.GetNamedQuery("Fill.Vitals");
                            query1.SetString(0, Convert.ToString(HumanId));
                            query1.SetString(1, Convert.ToString(EncounterId));
                            if (query1.List().Count > 0)
                                objPatientSummary.Vitals = Convert.ToInt32(query1.List()[0]);

                            if (objPatientSummary.Vitals == 1)
                            {
                                ISQLQuery sqlquery = iMySession.CreateSQLQuery("select v.* from patient_results v inner join dynamic_screen s on (v.Loinc_Observation=s.control_Name) inner join map_vitals_physician m on (s.master_vitals_id=m.master_vitals_id)  where v.human_id='" + HumanId + "' and m.physician_id='" + Physician_ID + "' and v.Results_Type='Vitals' and v.encounter_id='" + EncounterId + "' and loinc_observation = v.loinc_observation and  v.Captured_date_and_time in(select max(Captured_date_and_time)   from patient_results where human_id=v.human_id and encounter_id=v.encounter_id and loinc_observation = v.loinc_observation) group by v.Loinc_Observation order by m.sort_order").AddEntity("v", typeof(PatientResults));
                                objPatientSummary.VitalsList = sqlquery.List<PatientResults>();
                            }
                            else if (objPatientSummary.Vitals > 1)
                            {
                                IList<PatientResults> latestResults = new List<PatientResults>();
                                ISQLQuery sqlquery = iMySession.CreateSQLQuery("select v.* from patient_results v where v.human_id='" + HumanId + "' and v.encounter_id='" + EncounterId + "' and v.value <> '' and  v.Results_Type='Vitals' order by v.loinc_observation,v.Captured_date_and_time desc,v.vitals_group_id desc").AddEntity("v", typeof(PatientResults));
                                latestResults = sqlquery.List<PatientResults>();
                                IList<PatientResults> templist = new List<PatientResults>();
                                int k = 0;
                                for (int i = 0; i < latestResults.Count - 1; i++)
                                {
                                    if (k == 0)
                                    {
                                        if (latestResults[i].Loinc_Observation == latestResults[i + 1].Loinc_Observation)
                                        {
                                            Duptemplist.Add(latestResults[i]);
                                            k = 1;
                                        }
                                        else
                                        {
                                            Duptemplist.Add(latestResults[i]);
                                            k = 0;
                                        }
                                    }
                                    else if (latestResults[i].Loinc_Observation != latestResults[i + 1].Loinc_Observation)
                                    {
                                        if (!templist.Contains(latestResults[i]))
                                            templist.Add(latestResults[i]);
                                        k = 0;
                                    }
                                }
                                VitalsManager objVitalsManger = new VitalsManager();
                                ISQLQuery sqlqueryHuman = iMySession.CreateSQLQuery("select h.* from human h   where h.Human_ID='" + HumanId + "'").AddEntity("h", typeof(Human));
                                IList<Human> lstHuman = sqlqueryHuman.List<Human>();
                                ISQLQuery sqlqueryUser = iMySession.CreateSQLQuery("select u.* from user u   where u.User_Name='" + UserName + "'").AddEntity("u", typeof(User));
                                IList<User> lstUser = sqlqueryUser.List<User>();
                                double humanAgeInMonths = 0;
                                humanAgeInMonths = ((DateTime.Now.Date - lstHuman[0].Birth_Date.Date).TotalDays) / 30.4375;
                                IList<object> objList = objVitalsManger.GetLocalData(lstUser[0].Physician_Library_ID, Convert.ToInt16(humanAgeInMonths.ToString().Split('.')[0]), lstHuman[0].Sex, "'BMI-AGE','HC-AGE'", 2000);
                                IList<DynamicScreen> dynamicScreenList = new List<DynamicScreen>();
                                if (objList != null && objList.Count > 0)
                                {
                                    if (objList[1] != null && objList[1].GetType().ToString().ToUpper().Contains("DYNAMICSCREEN"))
                                        dynamicScreenList = (IList<DynamicScreen>)objList[1];
                                }
                                var Result = from a in Duptemplist join b in dynamicScreenList on a.Loinc_Observation equals b.Control_Name orderby b.Sort_Order select a;
                                objPatientSummary.VitalsList = Result.ToList<PatientResults>();
                            }
                            break;
                        case 3:
                            ICriteria critChiefComplaints = iMySession.CreateCriteria(typeof(ChiefComplaints)).Add(Expression.Eq("Encounter_ID", EncounterId)).Add(Expression.Eq("HPI_Element", "Chief Complaints"));
                            objPatientSummary.ChiefComplaintList = critChiefComplaints.List<ChiefComplaints>();
                            break;
                        case 4:
                            ProblemListManager objProbListMgr = new ProblemListManager();
                            objPatientSummary.PblmMedList = objProbListMgr.GetProblemDescriptionList(HumanId);
                            break;
                        case 5:
                            ICriteria criteria = iMySession.CreateCriteria(typeof(Rcopia_Medication)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Deleted", "N"));
                            objPatientSummary.MedicationList = criteria.List<Rcopia_Medication>();
                            IList<string> Medication_history = iMySession.CreateSQLQuery("Select Concat(m.Drug_Name,' ',m.Dosage,' ',m.Route_Of_Administration,' ',m.Frequency) from med_history m where  m.Human_ID=" + HumanId + "").List<string>();
                            IList<string> MedHistoryLst = new List<string>();
                            foreach (string item in Medication_history)
                                MedHistoryLst.Add(item);
                            objPatientSummary.MedHistoryList = MedHistoryLst;
                            break;
                    }
                }

                IList<Encounter> EncounterLst = new List<Encounter>();
                IList<DateTime> resultListDate = new List<DateTime>();
                IList<ulong> resultListID = new List<ulong>();
                ISQLQuery sqlquery1 = iMySession.CreateSQLQuery("select e.* from encounter e where   e.Human_ID=" + HumanId + " order by e.date_of_service desc ").AddEntity("e", typeof(Encounter));
                EncounterLst = sqlquery1.List<Encounter>();
                IList<string> EncounterID = new List<string>();
                IList<string> EncounterDate = new List<string>();
                DateTime CurrentDOS = DateTime.MinValue;
                if (EncounterId != 0)
                {
                    for (int i = 0; i < EncounterLst.Count; i++)
                    {

                        if (EncounterLst[i].Id == EncounterId)
                        {
                            CurrentDOS = EncounterLst[i].Date_of_Service;
                        }
                    }
                }

                for (int k = 0; k < EncounterLst.Count; k++)
                {
                    if (EncounterID.Count < 2)
                    {
                        if (EncounterLst[k].Date_of_Service <= CurrentDOS)
                        {
                            if (CurrentDOS.ToString() != DateTime.MinValue.ToString())
                            {
                                if (EncounterLst[k].Date_of_Service.ToString() != DateTime.MinValue.ToString())
                                {

                                    EncounterID.Add(EncounterLst[k].Id.ToString());
                                    resultListID.Add(EncounterLst[k].Id);
                                }
                                if (EncounterLst[k].Date_of_Service.ToString() != DateTime.MinValue.ToString())
                                {
                                    EncounterDate.Add(EncounterLst[k].Date_of_Service.ToString());
                                    resultListDate.Add(EncounterLst[k].Date_of_Service);
                                }
                            }
                        }

                    }
                }
                if (resultListDate.Count != 0)
                {
                    objPatientSummary.EncounterDateList = resultListDate;
                }
                if (resultListID.Count != 0)
                {
                    objPatientSummary.EncounterIDList = resultListID;
                }
                iMySession.Close();
            }
            return objPatientSummary;
        }
        //End loadpatientsummary


        public DataSet DtblCPBatchReport(DateTime From_Date, DateTime To_Date)
        {
            ArrayList arylstColumnsOfCPBatchReport;
            DataSet CPBatchReport = new DataSet();

            ArrayList arylstCPBatch;
            DataTable dtblCPBatch = new DataTable();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.GetCPBatchReport.ColumnHeadings");
                arylstColumnsOfCPBatchReport = new ArrayList(query2.List());
                if (arylstColumnsOfCPBatchReport.Count > 0)
                {
                    object[] objlstColumns = (object[])arylstColumnsOfCPBatchReport[0];

                    for (int i = 0; i < objlstColumns.Length; i++)
                    {
                        if (i == 1 || i == 6 | i == 10 | i == 11)
                        {
                            dtblCPBatch.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                        }
                        else
                        {
                            dtblCPBatch.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                        }
                    }
                }

                IQuery query1 = iMySession.GetNamedQuery("Reports.CPBatchReport");
                query1.SetString(0, From_Date.ToString("yyyy-MM-dd"));
                query1.SetString(1, To_Date.ToString("yyyy-MM-dd"));
                arylstCPBatch = new ArrayList(query1.List());
                for (int h = 0; h < arylstCPBatch.Count; h++)
                {
                    object[] objlst = (object[])arylstCPBatch[h];
                    DataRow dr = dtblCPBatch.NewRow();
                    if (objlst.Length > 0)
                    {
                        for (int k = 0; k < objlst.Length; k++)
                        {
                            dr[k] = objlst[k].ToString();

                        }
                        dtblCPBatch.Rows.Add(dr);
                    }
                }


                CPBatchReport.Tables.Add(dtblCPBatch);
                iMySession.Close();
            }
            return CPBatchReport;
        }

        public DataSet DtblPPBatchReport(DateTime From_Date, DateTime To_Date)
        {
            DataSet PPBatchReport = new DataSet();
            ArrayList arylstColumnsOfPPBatchReport;
            ArrayList arylstPPBatch;
            DataTable dtblPPBatch = new DataTable();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.GetPPBatchReport.ColumnHeadings");
                arylstColumnsOfPPBatchReport = new ArrayList(query2.List());
                if (arylstColumnsOfPPBatchReport.Count > 0)
                {
                    object[] objlstColumns = (object[])arylstColumnsOfPPBatchReport[0];

                    for (int i = 0; i < objlstColumns.Length; i++)
                    {
                        if (i == 1 || i == 6 | i == 13 | i == 15)
                        {
                            dtblPPBatch.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                        }
                        else
                        {
                            dtblPPBatch.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                        }
                    }
                }
                IQuery query1 = iMySession.GetNamedQuery("Reports.PPBatchReport");
                query1.SetString(0, From_Date.ToString("yyyy-MM-dd"));
                query1.SetString(1, To_Date.ToString("yyyy-MM-dd"));
                arylstPPBatch = new ArrayList(query1.List());
                for (int h = 0; h < arylstPPBatch.Count; h++)
                {
                    object[] objlst = (object[])arylstPPBatch[h];
                    DataRow dr = dtblPPBatch.NewRow();
                    if (objlst.Length > 0)
                    {
                        for (int k = 0; k < objlst.Length; k++)
                        {
                            dr[k] = objlst[k].ToString();

                        }
                        dtblPPBatch.Rows.Add(dr);
                    }
                }


                PPBatchReport.Tables.Add(dtblPPBatch);
                iMySession.Close();
            }
            return PPBatchReport;
        }
        public DataSet DtblAppointmentDetailsReport(DateTime From_Date, DateTime To_Date)
        {
            ArrayList arylstColumnsOfAppointmentDetailsReport;
            DataSet AppointmentDetailsReport = new DataSet();
            ArrayList arylstAppointmentDetails;
            DataTable dtblAppointmentDetails = new DataTable();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.GetAppointmentDetailsReport.ColumnHeadings");
                arylstColumnsOfAppointmentDetailsReport = new ArrayList(query2.List());

                if (arylstColumnsOfAppointmentDetailsReport.Count > 0)
                {
                    object[] objlstColumns = (object[])arylstColumnsOfAppointmentDetailsReport[0];

                    for (int i = 0; i < objlstColumns.Length; i++)
                    {
                        if (i == 2 || i == 5 || i == 6)
                        {
                            dtblAppointmentDetails.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                        }
                        else
                        {
                            dtblAppointmentDetails.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                        }
                    }
                }

                IQuery query1 = iMySession.GetNamedQuery("Reports.AppointmentDetailsReport");
                query1.SetString(0, From_Date.ToString("yyyy-MM-dd hh:mm tt"));
                query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                arylstAppointmentDetails = new ArrayList(query1.List());
                if (arylstAppointmentDetails != null)
                {
                    for (int h = 0; h < arylstAppointmentDetails.Count; h++)
                    {

                        object[] objlst = (object[])arylstAppointmentDetails[h];
                        DataRow dr = dtblAppointmentDetails.NewRow();
                        if (objlst.Length > 0)
                        {
                            for (int k = 0; k < objlst.Length; k++)
                            {

                                try
                                {

                                    if (objlst[k].ToString().Split(',').Length > 0)
                                    {
                                        if (objlst[k].ToString().Split(',')[0] == string.Empty && objlst[k].ToString().Split(',')[1] == string.Empty)
                                            objlst[k] = string.Empty;
                                        else if (objlst[k].ToString().Split(',')[0] == string.Empty)
                                            objlst[k] = objlst[k].ToString().Split(',')[1];
                                    }

                                    dr[k] = objlst[k].ToString();
                                }
                                catch
                                {
                                    // string hl;
                                }

                            }
                            dtblAppointmentDetails.Rows.Add(dr);
                        }
                    }
                }


                AppointmentDetailsReport.Tables.Add(dtblAppointmentDetails);
                iMySession.Close();
            }
            return AppointmentDetailsReport;
        }


        public DataSet DtblTransportationReport(DateTime From_date, DateTime To_Date)
        {
            ArrayList arylstColumnsOfTransportationReport;
            DataSet TransportationReport = new DataSet();
            ArrayList arylstTransportation;
            DataTable dtblTransportation = new DataTable();
            //string Fromdate = "2011-02-17";
            // string ToDate = "2011-02-20";
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.GetTransportationReport.ColumnHeadings");
                arylstColumnsOfTransportationReport = new ArrayList(query2.List());
                if (arylstColumnsOfTransportationReport != null)
                {
                    if (arylstColumnsOfTransportationReport.Count > 0)
                    {
                        object[] objlstColumns = (object[])arylstColumnsOfTransportationReport[0];

                        for (int i = 0; i < objlstColumns.Length; i++)
                        {
                            if (i == 2 || i == 6)
                            {
                                dtblTransportation.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                            }
                            else
                            {
                                dtblTransportation.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                            }
                        }
                    }
                }

                IQuery query1 = iMySession.GetNamedQuery("Reports.TransportationReport");
                //query1.SetString(0, Fromdate);
                // query1.SetString(1, ToDate);
                query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                arylstTransportation = new ArrayList(query1.List());
                if (arylstTransportation != null)
                {
                    for (int h = 0; h < arylstTransportation.Count; h++)
                    {
                        object[] objlst = (object[])arylstTransportation[h];
                        DataRow dr = dtblTransportation.NewRow();
                        if (objlst.Length > 0)
                        {
                            for (int k = 0; k < objlst.Length; k++)
                            {
                                try
                                {
                                    if (objlst[k].ToString().Split(',').Length > 0)
                                    {
                                        if (objlst[k].ToString().Split(',')[0] == string.Empty && objlst[k].ToString().Split(',')[1] == string.Empty && objlst[k].ToString().Split(',')[2] == string.Empty && objlst[k].ToString().Split(',')[3] == string.Empty)
                                            objlst[k] = string.Empty;
                                        else if (objlst[k].ToString().Split(',')[0] == string.Empty)
                                            objlst[k] = objlst[k].ToString().Split(',')[1];
                                    }
                                    dr[k] = objlst[k].ToString();
                                }
                                catch
                                {
                                    // string sMessage;
                                }
                            }
                            dtblTransportation.Rows.Add(dr);
                        }
                    }
                }

                TransportationReport.Tables.Add(dtblTransportation);
                iMySession.Close();
            }
            return TransportationReport;
        }

        public DataSet DtblAppointmentSummaryReport(DateTime From_date, DateTime To_Date)
        {
            ArrayList arylstColumnsOfAppointmentSummaryReport;
            ArrayList arylstAppointmentSummary;

            DataSet AppointmentReport = new DataSet();
            DataTable dtblAppointmentSummary = new DataTable();
            //string Fromdate = "2011-02-17";
            // string ToDate = "2011-02-20";
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.GetAppointmentSummaryReport.ColumnHeadings");
                arylstColumnsOfAppointmentSummaryReport = new ArrayList(query2.List());
                if (arylstColumnsOfAppointmentSummaryReport != null)
                {
                    if (arylstColumnsOfAppointmentSummaryReport.Count > 0)
                    {
                        object[] objlstColumns = (object[])arylstColumnsOfAppointmentSummaryReport[0];

                        for (int i = 0; i < objlstColumns.Length; i++)
                        {
                            if (i == 0)
                            {
                                dtblAppointmentSummary.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                            }
                            else
                            {
                                dtblAppointmentSummary.Columns.Add(objlstColumns[i].ToString(), typeof(System.Int32));
                            }
                        }
                    }
                }
                IQuery query1 = iMySession.GetNamedQuery("ReportsAppointmentSummaryReport");
                //query1.SetString(0, Fromdate);
                // query1.SetString(1, ToDate);
                query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                arylstAppointmentSummary = new ArrayList(query1.List());
                if (arylstAppointmentSummary != null)
                {
                    for (int h = 0; h < arylstAppointmentSummary.Count; h++)
                    {
                        object[] objlst = (object[])arylstAppointmentSummary[h];
                        DataRow dr = dtblAppointmentSummary.NewRow();
                        if (objlst.Length > 0)
                        {
                            for (int k = 0; k < objlst.Length; k++)
                            {
                                try
                                {
                                    dr[k] = objlst[k].ToString();

                                }
                                catch
                                {
                                    //string sMessage;
                                }
                            }
                            dtblAppointmentSummary.Rows.Add(dr);
                        }
                    }
                }

                AppointmentReport.Tables.Add(dtblAppointmentSummary);
                iMySession.Close();
            }
            return AppointmentReport;
        }
        //public ChargePostingDTO LoadEncounterList(IList<DateTime> DateofService, ulong HumanId)
        //{

        //    ChargePostingDTO objChargePostingDTO = new ChargePostingDTO();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        for (int i = 0; i < DateofService.Count; i++)
        //        {
        //            string Date = DateofService[i].ToString("yyyy-MM-dd");
        //            ISQLQuery sql = iMySession.CreateSQLQuery("Select a.* from Encounter a where Human_id =" + HumanId + " and Date_of_Service like '%" + Date + "%'").AddEntity("a", typeof(Encounter));

        //            if (sql.List<Encounter>().Count != 0)
        //            {
        //                objChargePostingDTO.EncounterList.Add(sql.List<Encounter>()[0]);
        //                WFObjectBillingManager objWFObjectBillingManager = new WFObjectBillingManager();
        //                objChargePostingDTO.WfObjectList.Add(objWFObjectBillingManager.GetByObjectSystemIdAndObjType(sql.List<Encounter>()[0].Id, "BILLING_ENCOUNTER"));
        //            }
        //        }
        //        iMySession.Close();
        //    }
        //    return objChargePostingDTO;
        //}

        public void MoveToNextProcess(IList<object> arlstProcess, string sOwner, string sMacAddress)
        {
            ISession MySession = Session.GetISession();
        //ITransaction trans = null;

        TryAgain:
            int iResult = 0;
            // trans = MySession.BeginTransaction();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        for (int i = 0; i < arlstProcess.Count; i++)
                        {

                            ulong EncounterId = Convert.ToUInt64(arlstProcess[i]);
                            WFObjectManager objMngr = new WFObjectManager();
                            WFObject objwfObject = new WFObject();
                            objwfObject = objMngr.GetByObjectSystemId(EncounterId, "DOCUMENTATION");

                            iResult = objMngr.MoveToNextProcess(objwfObject.Obj_System_Id, objwfObject.Obj_Type, 1, objwfObject.Current_Owner, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), sMacAddress, null, MySession);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {
                                    trans.Rollback();
                                    //MySession.Close();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                //MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                        }
                        MySession.Flush();
                        trans.Commit();
                    }

                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        throw new Exception(e.Message);
                    }

                    finally
                    {
                        MySession.Close();
                    }
                }
            }
            catch (Exception ex1)
            {
                //MySession.Close();
                throw new Exception(ex1.Message);
            }
        }

        ArrayList tempDenominator = null;
        ArrayList tempsummarycaredenominator = null;
        ArrayList tempDenominatorotherencounter = null;
        ArrayList tempNumerator = null;
        string ResultDenominator = string.Empty;
        string Resultsummarycaredenominator = string.Empty;
        string ResultDenominatorotherencounter = string.Empty;
        public DataSet GetEHRMeasureCalculation(IList<StaticLookup> FieldLookupLst, string sFromDate, string sToDate, ulong PhysicianID, int vitalAge, int SmokingAge, string RcopiaUserName)
        {
            //int iCount1 = 0;
            //DateTime dFromDate = Convert.ToDateTime(sFromDate);
            //DateTime dToDate = Convert.ToDateTime(sToDate).AddDays(1).AddMilliseconds(-1);
            //string FromDate = dFromDate.ToString("yyyy-MM-dd HH:mm:ss");
            //string ToDate = dToDate.ToString("yyyy-MM-dd HH:mm:ss");
            string FromDate = sFromDate;
            string ToDate = sToDate;
            DataSet dsMeasure = new DataSet();
            result.Clear();
            DataTable dtMeasure = new DataTable();
            //ArrayList aryMeasure = null;
            // using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            // {
            //IQuery query = iMySession.GetNamedQuery("Fill.EHRMeasureCalCulation.ColumnHeadings");
            //  aryMeasure = new ArrayList(query.List());
            //  if (aryMeasure.Count > 0)
            //   {
            //object[] objMeasureColumns = (object[])aryMeasure[0];

            // for (int i = 0; i < objMeasureColumns.Length; i++)
            //  {
            dtMeasure.Columns.Add("Measure Name", typeof(System.String));
            dtMeasure.Columns.Add("Measure in Detail", typeof(System.String));
            dtMeasure.Columns.Add("Numerator", typeof(System.String));
            dtMeasure.Columns.Add("Denominator", typeof(System.String));
            dtMeasure.Columns.Add("Percentage", typeof(System.String));
            dtMeasure.Columns.Add("Percentage Required", typeof(System.String));
            dtMeasure.Columns.Add("Cleared", typeof(System.String));
            // }
            dtMeasure.Columns.Add("Denominator Human Id", typeof(System.String));
            dtMeasure.Columns.Add("Numerator Human Id", typeof(System.String));

            // }

            DataRow drMeasure = null;

            IQuery query1 = session.GetISession().GetNamedQuery("NumeratorCommon");
            query1.SetString(0, Convert.ToString(PhysicianID));
            query1.SetString(1, FromDate);
            query1.SetString(2, ToDate);
            query1.SetString(3, Convert.ToString(PhysicianID));
            query1.SetString(4, FromDate);
            query1.SetString(5, ToDate);
            tempNumerator = new ArrayList(query1.List());
            // UserManager objPhysicianManager = new UserManager();
            //IList<User>  lstphy=objPhysicianManager.getUserByPHYID(PhysicianID);
            //lstphy[0].user_name;
            query1 = session.GetISession().GetNamedQuery("VitalsSignDenominator");
            query1.SetString(0, Convert.ToString(PhysicianID));
            query1.SetString(1, FromDate);
            query1.SetString(2, ToDate);
            query1.SetString(3, Convert.ToString(PhysicianID));
            query1.SetString(4, FromDate);
            query1.SetString(5, ToDate);
            tempDenominator = new ArrayList(query1.List());
            if (tempDenominator != null && tempDenominator.Count > 0)
            {
                ResultDenominator = tempDenominator.Count.ToString();
                for (int k = 0; k < tempDenominator.Count; k++)
                {
                    if (k == 0)
                        ResultDenominator = ResultDenominator + "|" + (tempDenominator[k].ToString());
                    else
                        ResultDenominator = ResultDenominator + "," + (tempDenominator[k].ToString());
                }
            }
            query1 = session.GetISession().GetNamedQuery("PatientEducationDenominator");
            query1.SetString(0, Convert.ToString(PhysicianID));
            query1.SetString(1, FromDate);
            query1.SetString(2, ToDate);
            query1.SetString(3, Convert.ToString(PhysicianID));
            query1.SetString(4, FromDate);
            query1.SetString(5, ToDate);
            tempDenominatorotherencounter = new ArrayList(query1.List());
            if (tempDenominatorotherencounter != null && tempDenominatorotherencounter.Count > 0)
            {
                ResultDenominatorotherencounter = tempDenominatorotherencounter.Count.ToString();
                for (int k = 0; k < tempDenominatorotherencounter.Count; k++)
                {
                    if (k == 0)
                        ResultDenominatorotherencounter = ResultDenominatorotherencounter + "|" + (tempDenominatorotherencounter[k].ToString());
                    else
                        ResultDenominatorotherencounter = ResultDenominatorotherencounter + "," + (tempDenominatorotherencounter[k].ToString());
                }
            }
            IQuery query3 = session.GetISession().GetNamedQuery("SummaryofCareMeasure1Denominator");
            query3.SetString(0, Convert.ToString(PhysicianID));
            query3.SetString(1, FromDate);
            query3.SetString(2, ToDate);
            query3.SetString(3, Convert.ToString(PhysicianID));
            query3.SetString(4, FromDate);
            query3.SetString(5, ToDate);

            tempsummarycaredenominator = new ArrayList(query3.List());

            if (tempsummarycaredenominator != null && tempsummarycaredenominator.Count > 0)
            {
                Resultsummarycaredenominator = tempsummarycaredenominator.Count.ToString();
                for (int k = 0; k < tempsummarycaredenominator.Count; k++)
                {
                    if (k == 0)
                        Resultsummarycaredenominator = Resultsummarycaredenominator + "|" + (tempsummarycaredenominator[k].ToString());
                    else
                        Resultsummarycaredenominator = Resultsummarycaredenominator + "," + (tempsummarycaredenominator[k].ToString());
                }
            }
            for (int i = 0; i < FieldLookupLst.Count; i++)
            {
                drMeasure = dtMeasure.NewRow();
                string[] split_numerator = null;
                string[] split_denominator = null;
                for (int j = 0; j < 7; j++)
                {
                    switch (j.ToString())
                    {
                        case "0":
                            drMeasure[j] = FieldLookupLst[i].Value;
                            break;
                        case "1":
                            drMeasure[j] = FieldLookupLst[i].Description;
                            break;
                        case "2":

                            string count = ReturnCountandHumanId(Convert.ToString(FieldLookupLst[i].Value) + "-n-" + FieldLookupLst[i].Doc_Type, FromDate, ToDate, PhysicianID, vitalAge, SmokingAge, RcopiaUserName);
                            split_numerator = count.Split('|');
                            drMeasure[j] = split_numerator[0].ToString();
                            if (split_numerator[0].ToString() == "")
                                drMeasure[j] = "0";
                            break;
                        case "3":
                            // if (FieldLookupLst[i].Value.ToUpper() == "CPOE FOR RADIOLOGY ORDERS" || FieldLookupLst[i].Value.ToUpper() == "CPOE FOR LABORATORY ORDERS")
                            //  drMeasure[j] = drMeasure[2];
                            // else
                            // {
                            count = ReturnCountandHumanId(Convert.ToString(FieldLookupLst[i].Value) + "-d-" + FieldLookupLst[i].Doc_Type, FromDate, ToDate, PhysicianID, vitalAge, SmokingAge, RcopiaUserName);
                            split_denominator = count.Split('|');
                            drMeasure[j] = split_denominator[0].ToString();
                            if (split_denominator[0].ToString() == "")
                                drMeasure[j] = "0";
                            //}
                            break;
                        case "4":
                            try
                            {
                                drMeasure[j] = Convert.ToString(Decimal.Round(((Convert.ToDecimal(drMeasure[2]) / Convert.ToDecimal(drMeasure[3])) * 100), 2) + "%");
                            }
                            catch
                            {
                                drMeasure[j] = "0%";
                            }
                            break;
                        case "5":
                            drMeasure[j] = FieldLookupLst[i].Default_Value + "%";
                            break;
                        case "6":
                            decimal dMeasure;
                            drMeasure[j] = FieldLookupLst[i].Default_Value;
                            if (drMeasure[4] != "0%")
                            {
                                string strMeasure = drMeasure[j].ToString();
                                dMeasure = Convert.ToDecimal(strMeasure);
                                string[] strDec = drMeasure[4].ToString().Split('%');
                                decimal dDec = Convert.ToDecimal(strDec[0]);
                                if (dDec >= dMeasure)
                                {
                                    drMeasure[j] = "Yes";
                                }
                                else if (dDec < dMeasure)
                                {
                                    drMeasure[j] = "No";

                                }
                            }
                            else
                            {
                                drMeasure[j] = "No";
                            }
                            break;
                    }
                }
                if (split_denominator != null && split_denominator.Length > 1)
                    drMeasure["Denominator Human Id"] = split_denominator[1].ToString();
                else
                    drMeasure["Denominator Human Id"] = "0";
                if (split_numerator != null && split_numerator.Length > 1)
                    drMeasure["Numerator Human Id"] = split_numerator[1].ToString();
                else
                    drMeasure["Numerator Human Id"] = "0";
                dtMeasure.Rows.Add(drMeasure);

            }

            dsMeasure.Tables.Add(dtMeasure);
            //  }
            return dsMeasure;


        }
        ArrayList tempRcopia = null;
        private string ReturnCountandHumanId(string strType, string FromDate, string ToDate, ulong PhysicianID, int vitalAge, int SmokingAge, string sRcopiaUserName)
        {
            //int iCount = 0;
            string Result = string.Empty;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {


                switch (strType)
                {
                    case "Maintain up-to-date problem list-n-Stage 2":
                        {

                            IQuery query = iMySession.GetNamedQuery("MaintainProblemListNumerator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            // query.SetString(5, ToDate);
                            if (tempDenominator.Count > 0)
                            {
                                query.SetParameterList("HumanID", tempDenominator);
                                ArrayList temp = null;
                                temp = new ArrayList(query.List());
                                if (temp != null && temp.Count > 0)
                                {
                                    Result = temp.Count.ToString();
                                    for (int k = 0; k < temp.Count; k++)
                                    {
                                        if (k == 0)
                                            Result = Result + "|" + (temp[k].ToString());
                                        else
                                            Result = Result + "," + (temp[k].ToString());
                                    }
                                }
                            }
                            break;
                        }
                    case "Maintain up-to-date problem list-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("MaintainProblemListDenominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //if (temp != null && temp.Count > 0)
                            //{
                            //    Result = temp.Count.ToString();
                            //    for (int k = 0; k < temp.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (temp[k].ToString());
                            //        else
                            //            Result = Result + "," + (temp[k].ToString());
                            //    }
                            //}
                            Result = Result + ResultDenominator;
                            break;
                        }


                    case "Record Demographics-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("RecordDemographicsNumerator");
                            //query.setstring(0, convert.tostring(physicianid));
                            //query.setstring(1, fromdate);
                            //query.setstring(2, todate);
                            //query.setstring(3, convert.tostring(physicianid));
                            //query.setstring(4, fromdate);
                            //query.setstring(5, todate);
                            if (tempDenominator.Count > 0)
                            {
                                query.SetParameterList("HumanID", tempDenominator);
                                ArrayList temp = null;


                                temp = new ArrayList(query.List());

                                if (temp != null && temp.Count > 0)
                                {
                                    Result = temp.Count.ToString();
                                    for (int k = 0; k < temp.Count; k++)
                                    {
                                        if (k == 0)
                                            Result = Result + "|" + (temp[k].ToString());
                                        else
                                            Result = Result + "," + (temp[k].ToString());
                                    }
                                }
                            }
                            break;
                        }
                    case "Record Demographics-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("RecordDemographicsDenominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //if (temp != null && temp.Count > 0)
                            //{
                            //    Result = temp.Count.ToString();
                            //    for (int k = 0; k < temp.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (temp[k].ToString());
                            //        else
                            //            Result = Result + "," + (temp[k].ToString());
                            //    }
                            //}
                            Result = Result + ResultDenominator;
                            break;
                        }
                    case "Record Vitals-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("RecordVitalsAllWithinScopeNumerator");
                            //query.SetString(0, FromDate);
                            //query.SetString(1, FromDate);
                            //query.SetString(2, Convert.ToString(PhysicianID));
                            //query.SetString(3, FromDate);
                            //query.SetString(4, ToDate);
                            //query.SetString(5, Convert.ToString(vitalAge));
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            if (tempDenominator.Count > 0)
                            {
                                query.SetParameterList("HumanID", tempDenominator);
                                ArrayList temp = null;
                                temp = new ArrayList(query.List());
                                if (temp != null && temp.Count > 0)
                                {
                                    Result = temp.Count.ToString();
                                    for (int k = 0; k < temp.Count; k++)
                                    {
                                        if (k == 0)
                                            Result = Result + "|" + (temp[k].ToString());
                                        else
                                            Result = Result + "," + (temp[k].ToString());
                                    }
                                }
                            }
                            break;
                        }
                    case "Record Vitals-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("VitalsSignDenominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //if (tempDenominator != null && tempDenominator.Count > 0)
                            //{
                            //    Result = tempDenominator.Count.ToString();
                            //    for (int k = 0; k < tempDenominator.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempDenominator[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempDenominator[k].ToString());
                            //    }
                            //}

                            Result = Result + ResultDenominator;
                            break;
                        }
                    case "Record Smoking Status-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("SmokingStatusNumerator");
                            //query.SetString(0, FromDate);
                            //query.SetString(1, FromDate);
                            //query.SetString(2, Convert.ToString(PhysicianID));
                            //query.SetString(3, FromDate);
                            //query.SetString(4, ToDate);
                            //query.SetString(5, Convert.ToString(SmokingAge));

                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(SmokingAge));
                            //query.SetString(4, Convert.ToString(PhysicianID));
                            //query.SetString(5, FromDate);
                            //query.SetString(6, ToDate);
                            query.SetString(0, Convert.ToString(SmokingAge));
                            if (tempDenominator.Count > 0)
                            {
                                query.SetParameterList("HumanID", tempDenominator);
                                ArrayList temp = null;
                                temp = new ArrayList(query.List());
                                if (temp != null && temp.Count > 0)
                                {
                                    Result = temp.Count.ToString();
                                    for (int k = 0; k < temp.Count; k++)
                                    {
                                        if (k == 0)
                                            Result = Result + "|" + (temp[k].ToString());
                                        else
                                            Result = Result + "," + (temp[k].ToString());
                                    }
                                }
                            }
                            break;
                        }
                    case "Record Smoking Status-d-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("SmokingStatusDenominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(SmokingAge));
                            query.SetString(4, Convert.ToString(PhysicianID));
                            query.SetString(5, FromDate);
                            query.SetString(6, ToDate);
                            query.SetString(7, Convert.ToString(SmokingAge));
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Patient Specific Education Resources-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientEducationNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Patient Specific Education Resources-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("PatientEducationDenominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //if (tempDenominatorotherencounter != null && tempDenominatorotherencounter.Count > 0)
                            //{
                            //    Result = tempDenominatorotherencounter.Count.ToString();
                            //    for (int k = 0; k < tempDenominatorotherencounter.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempDenominatorotherencounter[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempDenominatorotherencounter[k].ToString());
                            //    }
                            //}
                            Result = Result + ResultDenominatorotherencounter;
                            break;
                        }
                    case "Clinical Lab Test Results-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("ClinicalLabTestResultsNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Clinical Lab Test Results-d-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("ClinicalLabTestResultsDenominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Active Medication List-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("ActiveMedicationListNumerator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            if (tempDenominator.Count > 0)
                            {
                                query.SetParameterList("HumanID", tempDenominator);
                                ArrayList temp = null;
                                temp = new ArrayList(query.List());
                                if (temp != null && temp.Count > 0)
                                {
                                    Result = temp.Count.ToString();
                                    for (int k = 0; k < temp.Count; k++)
                                    {
                                        if (k == 0)
                                            Result = Result + "|" + (temp[k].ToString());
                                        else
                                            Result = Result + "," + (temp[k].ToString());
                                    }
                                }
                            }
                            break;
                        }
                    case "Active Medication List-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("ActiveMedicationListDenominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //if (tempDenominator != null && tempDenominator.Count > 0)
                            //{
                            //    Result = tempDenominator.Count.ToString();
                            //    for (int k = 0; k < tempDenominator.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempDenominator[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempDenominator[k].ToString());
                            //    }
                            //}
                            Result = Result + ResultDenominator;
                            break;
                        }
                    case "Maintain active medication allergy list.-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("MaintainActiveMedicationAllergyListNumerator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            if (tempDenominator.Count > 0)
                            {
                                query.SetParameterList("HumanID", tempDenominator);
                                ArrayList temp = null;
                                temp = new ArrayList(query.List());
                                if (temp != null && temp.Count > 0)
                                {
                                    Result = temp.Count.ToString();
                                    for (int k = 0; k < temp.Count; k++)
                                    {
                                        if (k == 0)
                                            Result = Result + "|" + (temp[k].ToString());
                                        else
                                            Result = Result + "," + (temp[k].ToString());
                                    }
                                }
                            }
                            break;
                        }
                    case "Maintain active medication allergy list.-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("MaintainActiveMedicationAllergyListDenominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //if (tempDenominator != null && tempDenominator.Count > 0)
                            //{
                            //    Result = tempDenominator.Count.ToString();
                            //    for (int k = 0; k < tempDenominator.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempDenominator[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempDenominator[k].ToString());
                            //    }
                            //}
                            Result = Result + ResultDenominator;
                            break;
                        }
                    case "CPOE for Medication Orders-n-Stage 2":
                        {

                            UserManager objPhysicianManager = new UserManager();
                            IList<User> lstphy = objPhysicianManager.getUserByPHYID(PhysicianID);



                            IQuery query = iMySession.GetNamedQuery("CPOEMedicationOrdersListNumerator");
                            query.SetString(0, lstphy[0].user_name);

                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            //ArrayList temp = null;
                            tempRcopia = new ArrayList(query.List());
                            if (tempRcopia != null && tempRcopia.Count > 0)
                            {
                                Result = tempRcopia.Count.ToString();
                                for (int k = 0; k < tempRcopia.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (tempRcopia[k].ToString());
                                    else
                                        Result = Result + "," + (tempRcopia[k].ToString());
                                }
                            }
                            break;
                        }
                    case "CPOE for Medication Orders-d-Stage 2":
                        {
                            //UserManager objPhysicianManager = new UserManager();
                            //IList<User> lstphy = objPhysicianManager.getUserByPHYID(PhysicianID);

                            //IQuery query = iMySession.GetNamedQuery("CPOEMedicationOrdersListDenominator");
                            //query.SetString(0, lstphy[0].user_name);

                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //ArrayList temp = null;
                            //tempRcopia = new ArrayList(query.List());
                            if (tempRcopia != null && tempRcopia.Count > 0)
                            {
                                Result = tempRcopia.Count.ToString();
                                for (int k = 0; k < tempRcopia.Count; k++)
                                {
                                    if (k == 0)
                                        Result = tempRcopia[k].ToString();
                                    else
                                        Result = Result + "," + (tempRcopia[k].ToString());
                                }
                            }


                            //

                            IQuery query1 = iMySession.GetNamedQuery("CPOEMedicationOrdersListDenominatorPart-stage2");
                            query1.SetString(0, Convert.ToString(PhysicianID));
                            query1.SetString(1, FromDate);
                            query1.SetString(2, ToDate);
                            query1.SetString(3, Convert.ToString(PhysicianID));
                            query1.SetString(4, FromDate);
                            query1.SetString(5, ToDate);


                            ArrayList temp1 = null;
                            temp1 = new ArrayList(query1.List());

                            ulong count = 0;

                            count = Convert.ToUInt32(tempRcopia.Count);
                            if (temp1 != null && temp1.Count > 0)
                            {
                                for (int x = 0; x < temp1.Count; x++)
                                {

                                    IList<object> lstObje = (IList<object>)temp1[x];
                                    for (int y = 0; y < lstObje.Count; y++)
                                    {
                                        if (y == 1)
                                            count += Convert.ToUInt32(lstObje[y]);
                                        else
                                            Result = Result + "," + (lstObje[y].ToString());

                                    }


                                    //if (x == 0)
                                    //    Result = Result + "|" + (temp[k].ToString());
                                    //else
                                    //    Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            Result = count.ToString() + "|" + Result;
                            //

                            break;
                        }
                    case "Clinical Summaries-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("ClinicalSummariesNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Clinical Summaries-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("ClinicalSummariesDenominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //  if (tempDenominatorotherencounter != null && tempDenominatorotherencounter.Count > 0)
                            //{
                            //    Result = tempDenominatorotherencounter.Count.ToString();
                            //    for (int k = 0; k < tempDenominatorotherencounter.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempDenominatorotherencounter[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempDenominatorotherencounter[k].ToString());
                            //    }
                            //}
                            Result = Result + ResultDenominatorotherencounter;
                            break;
                        }
                    case "Transition of Care Summary-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("TransitionCareSummaryNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Transition of Care Summary-d-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("TransitionCareSummaryDenominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }

                    case "E-Prescribing (eRx)-n-Stage 2":
                        {


                            UserManager objPhysicianManager = new UserManager();
                            IList<User> lstphy = objPhysicianManager.getUserByPHYID(PhysicianID);


                            IQuery query = iMySession.GetNamedQuery("EPrescribingNumerator");
                            query.SetString(0, lstphy[0].user_name);
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());


                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString()) + "~Y";
                                    else
                                        Result = Result + "," + (temp[k].ToString()) + "~Y";
                                }
                            }

                            //result.Add("12n", iCount);
                            break;
                        }
                    case "E-Prescribing (eRx)-d-Stage 2":
                        {

                            UserManager objPhysicianManager = new UserManager();
                            IList<User> lstphy = objPhysicianManager.getUserByPHYID(PhysicianID);


                            IQuery query = iMySession.GetNamedQuery("EPrescribingDenominator");
                            query.SetString(0, lstphy[0].user_name);
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }

                    case "E-Prescribing (eRx)-Excludes Controlled Substances-n-Stage 2":
                        {

                            UserManager objPhysicianManager = new UserManager();
                            IList<User> lstphy = objPhysicianManager.getUserByPHYID(PhysicianID);

                            IQuery query = iMySession.GetNamedQuery("EPrescribingExcludesControlledSubstancesNU");
                            query.SetString(0, lstphy[0].user_name);
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            //result.Add("12n", iCount);
                            break;
                        }

                    case "E-Prescribing (eRx)-Excludes Controlled Substances-d-Stage 2":
                        {

                            UserManager objPhysicianManager = new UserManager();
                            IList<User> lstphy = objPhysicianManager.getUserByPHYID(PhysicianID);


                            IQuery query = iMySession.GetNamedQuery("EPrescribingExcludesControlledSubstancesDE");
                            query.SetString(0, lstphy[0].user_name);
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            //result.Add("12n", iCount);
                            break;
                        }
                    case "Electronic Copy of Health Information-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("ElectronicCopyHealthInformationNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Electronic Copy of Health Information-d-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("ElectronicCopyHealthInformationDenominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Patient Electronic Access-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientElectronicAccessNumerator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            if (tempDenominator.Count > 0)
                            {
                                query.SetParameterList("HumanID", tempDenominator);
                                ArrayList temp = null;
                                temp = new ArrayList(query.List());
                                if (temp != null && temp.Count > 0)
                                {
                                    Result = temp.Count.ToString();
                                    for (int k = 0; k < temp.Count; k++)
                                    {
                                        if (k == 0)
                                            Result = Result + "|" + (temp[k].ToString());
                                        else
                                            Result = Result + "," + (temp[k].ToString());
                                    }
                                }
                            }
                            break;
                        }
                    case "Patient Electronic Access-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("PatientElectronicAccessDenominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //if (tempDenominator != null && tempDenominator.Count > 0)
                            //{
                            //    Result = tempDenominator.Count.ToString();
                            //    for (int k = 0; k < tempDenominator.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempDenominator[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempDenominator[k].ToString());
                            //    }
                            //}

                            Result = Result + ResultDenominator;
                            break;
                        }
                    case "Patient Reminders-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientReminderNumerator");
                            //query.SetString(0, FromDate);
                            //query.SetString(1, FromDate);
                            //query.SetString(2, Convert.ToString(PhysicianID));
                            //query.SetString(3, FromDate);
                            //query.SetString(4, ToDate);
                            //query.SetString(5, FromDate);
                            //query.SetString(6, FromDate);
                            //query.SetString(7, Convert.ToString(PhysicianID));
                            //query.SetString(8, FromDate);
                            //query.SetString(9, ToDate);
                            //query.SetString(5, Convert.ToString(SmokingAge));
                            if (tempDenominator.Count > 0)
                            {
                                query.SetParameterList("HumanID", tempDenominator);
                                ArrayList temp = null;
                                temp = new ArrayList(query.List());
                                if (temp != null && temp.Count > 0)
                                {
                                    Result = temp.Count.ToString();
                                    for (int k = 0; k < temp.Count; k++)
                                    {
                                        if (k == 0)
                                            Result = Result + "|" + (temp[k].ToString());
                                        else
                                            Result = Result + "," + (temp[k].ToString());
                                    }
                                }
                            }
                            break;
                        }
                    case "Patient Reminders-d-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientReminderDenominator");
                            query.SetString(0, FromDate);
                            query.SetString(1, FromDate);
                            query.SetString(2, Convert.ToString(PhysicianID));
                            query.SetString(3, FromDate);
                            query.SetString(4, ToDate);
                            //query.SetString(5, Convert.ToString(SmokingAge));
                            query.SetString(5, FromDate);
                            query.SetString(6, FromDate);
                            query.SetString(7, Convert.ToString(PhysicianID));
                            query.SetString(8, FromDate);
                            query.SetString(9, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }

                    case "CPOE for Radiology Orders-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("CPOERadiologyOrdersListNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "CPOE for Radiology Orders-d-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("CPOERadiologyOrdersListDenominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "CPOE for Laboratory Orders-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("CPOELaboratoryOrdersListNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "CPOE for Laboratory Orders-d-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("CPOELaboratoryOrdersListDenominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Imaging Results-n-Stage 2":
                        {
                            ArrayList strList = new ArrayList();
                            IQuery query = iMySession.GetNamedQuery("ImagingResultListNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            //strList = new ArrayList(query.List());
                            //IList<string> sProcess = new List<string>();
                            //sProcess.Add("RESULT_REVIEW");
                            //sProcess.Add("BILLING_WAIT");
                            //sProcess.Add("MA_REVIEW");
                            //sProcess.Add("MA_RESULTS");
                            //ICriteria crit = session.GetISession().CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", "DIAGNOSTIC ORDER")).Add(Expression.In("Current_Process", sProcess.ToArray())).Add(Expression.In("Obj_System_Id", strList.ToArray()));
                            //IList<WFObject> objList = crit.List<WFObject>();
                            //if (strList.Count > 0)
                            //    result.Add("18n", strList);
                            //if (objList != null)
                            //    iCount = objList.Count();
                            break;
                        }
                    case "Imaging Results-d-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("ImagingResultListDenominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            //if (result.ContainsKey("18n"))
                            //{
                            //    ArrayList strList = new ArrayList();
                            //    strList = (ArrayList)result["18n"];
                            //    ICriteria crit = session.GetISession().CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", "DIAGNOSTIC ORDER")).Add(Expression.Not(Expression.Eq("Current_Process", "DELETED_ORDER"))).Add(Expression.In("Obj_System_Id", strList.ToArray()));
                            //    IList<WFObject> objList = crit.List<WFObject>();
                            //    result.Clear();
                            //    if (objList != null)
                            //        iCount = objList.Count();
                            //}
                            break;
                        }
                    case "Family Health History-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("Get.FamilyHealthHistoryNumerator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            if (tempDenominator.Count > 0)
                            {
                                query.SetParameterList("HumanID", tempDenominator);
                                ArrayList temp = null;
                                temp = new ArrayList(query.List());
                                if (temp != null && temp.Count > 0)
                                {
                                    Result = temp.Count.ToString();
                                    for (int k = 0; k < temp.Count; k++)
                                    {
                                        if (k == 0)
                                            Result = Result + "|" + (temp[k].ToString());
                                        else
                                            Result = Result + "," + (temp[k].ToString());
                                    }
                                }
                            }
                            break;
                        }

                    case "Family Health History-d-Stage 2":
                        {
                            // IQuery query = iMySession.GetNamedQuery("Get.FamilyHealthHistoryDenominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //if (tempDenominator != null && tempDenominator.Count > 0)
                            //{
                            //    Result = tempDenominator.Count.ToString();
                            //    for (int k = 0; k < tempDenominator.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempDenominator[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempDenominator[k].ToString());
                            //    }
                            //}
                            Result = Result + ResultDenominator;
                            break;
                        }
                    case "Electronic Notes-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("ElectronicsNotesNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }

                    case "Electronic Notes-d-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("ElectronicsNotesDenominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            // Result = Result + ResultDenominatorotherencounter;
                            break;
                        }

                    case "Patient Electronic Access Measure1-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientElectronicAccessMeasure1Numerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Patient Electronic Access Measure1-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("PatientElectronicAccessMeasure1Denominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //  temp = new ArrayList(query.List());
                            //if (tempDenominator != null && tempDenominator.Count > 0)
                            //{
                            //   Result = tempDenominator.Count.ToString();
                            //    for (int k = 0; k < tempDenominator.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempDenominator[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempDenominator[k].ToString());
                            //    }
                            //}
                            Result = Result + ResultDenominator;
                            break;
                        }
                    case "Patient Electronic Access Measure2-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientElectronicAccessMeasure2Numerator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            if (tempDenominator.Count > 0)
                            {
                                query.SetParameterList("HumanID", tempDenominator);
                                ArrayList temp = null;
                                temp = new ArrayList(query.List());
                                if (temp != null && temp.Count > 0)
                                {
                                    Result = temp.Count.ToString();
                                    for (int k = 0; k < temp.Count; k++)
                                    {
                                        if (k == 0)
                                            Result = Result + "|" + (temp[k].ToString());
                                        else
                                            Result = Result + "," + (temp[k].ToString());
                                    }
                                }
                            }
                            break;
                        }
                    case "Patient Electronic Access Measure2-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("PatientElectronicAccessMeasure2Denominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //if (tempDenominator != null && tempDenominator.Count > 0)
                            //{
                            //    Result = tempDenominator.Count.ToString();
                            //    for (int k = 0; k < tempDenominator.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempDenominator[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempDenominator[k].ToString());
                            //    }
                            //}
                            Result = Result + ResultDenominator;
                            break;
                        }
                    case "Preventive Care-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("PreventivecareNumerator");

                            string From_Date = Convert.ToDateTime(FromDate).AddYears(-2).ToString("yyyy-MM-dd");
                            string To_Date = FromDate;
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);

                            query.SetString(3, From_Date);
                            query.SetString(4, To_Date);
                            query.SetString(5, Convert.ToString(PhysicianID));
                            query.SetString(6, FromDate);
                            query.SetString(7, ToDate);
                            query.SetString(8, From_Date);
                            query.SetString(9, To_Date);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Preventive Care-d-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("PreventivecareDenominator");
                            string From_Date = Convert.ToDateTime(FromDate).AddYears(-2).ToString("yyyy-MM-dd");
                            string To_Date = FromDate;
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, From_Date);
                            query.SetString(2, To_Date);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, From_Date);
                            query.SetString(5, To_Date);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            //Result = Result + ResultDenominatorotherencounter;
                            break;
                        }
                    case "Use Secure Electronic Messaging-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("SecureelectronicmessagingNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Use Secure Electronic Messaging-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("SecureelectronicmessagingDenominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //if (tempDenominator != null && tempDenominator.Count > 0)
                            //{
                            //    Result = tempDenominator.Count.ToString();
                            //    for (int k = 0; k < tempDenominator.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempDenominator[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempDenominator[k].ToString());
                            //    }
                            //}
                            Result = Result + ResultDenominator;
                            break;
                        }
                    case "Summary of Care Measure1-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("SummaryofCareMeasure1Numerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }

                    case "Summary of Care Measure2-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("SummaryofCareMeasure2Numerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Summary of Care Measure1-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("SummaryofCareMeasure1Denominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //if (tempsummarycaredenominator != null && tempsummarycaredenominator.Count > 0)
                            //{
                            //    Result = tempsummarycaredenominator.Count.ToString();
                            //    for (int k = 0; k < tempsummarycaredenominator.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempsummarycaredenominator[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempsummarycaredenominator[k].ToString());
                            //    }
                            //}
                            Result = Result + Resultsummarycaredenominator;
                            break;
                        }
                    case "Summary of Care Measure2-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("SummaryofCareMeasure2Denominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //if (tempsummarycaredenominator != null && tempsummarycaredenominator.Count > 0)
                            //{
                            //    Result = tempsummarycaredenominator.Count.ToString();
                            //    for (int k = 0; k < tempsummarycaredenominator.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempsummarycaredenominator[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempsummarycaredenominator[k].ToString());
                            //    }
                            //}
                            Result = Result + Resultsummarycaredenominator;
                            break;
                        }


                    // By Mohan

                    //1.All Within Scope
                    case "Record Vitals-All Within Scope-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("RecordVitalsAllWithinScopeDenominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            // temp = new ArrayList(query.List());
                            //if (tempDenominator != null && tempDenominator.Count > 0)
                            //{
                            //    Result = tempDenominator.Count.ToString();
                            //    for (int k = 0; k < tempDenominator.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempDenominator[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempDenominator[k].ToString());
                            //    }
                            //}
                            Result = Result + ResultDenominator;
                            break;
                        }

                    case "Record Vitals-All Within Scope-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("RecordVitalsAllWithinScopeNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            //query.SetParameterList("HumanID", tempDenominator);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            break;
                        }

                    case "Record Vitals-BP Out of Scope-d-Stage 1":
                        {
                            //IQuery query = iMySession.GetNamedQuery("RecordVitalsBPOutofScopeDenominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            //if (tempDenominator != null && tempDenominator.Count > 0)
                            //{
                            //    Result = tempDenominator.Count.ToString();
                            //    for (int k = 0; k < tempDenominator.Count; k++)
                            //    {
                            //        if (k == 0)
                            //            Result = Result + "|" + (tempDenominator[k].ToString());
                            //        else
                            //            Result = Result + "," + (tempDenominator[k].ToString());
                            //    }
                            //}
                            Result = Result + ResultDenominator;
                            break;
                        }

                    case "Record Vitals-BP Out of Scope-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("RecordVitalsBPOutofScopeNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            break;
                        }

                    //2.BP Out of Scope


                    case "Record Vitals-BP Out of Scope-d-Stage 2":
                        {
                            //IQuery query = iMySession.GetNamedQuery("RecordVitalsBPOutofScopeDenominator");
                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //ArrayList temp = null;
                            //temp = new ArrayList(query.List());
                            if (tempDenominator != null && tempDenominator.Count > 0)
                            {
                                Result = tempDenominator.Count.ToString();
                                for (int k = 0; k < tempDenominator.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (tempDenominator[k].ToString());
                                    else
                                        Result = Result + "," + (tempDenominator[k].ToString());
                                }
                            }

                            break;
                        }

                    case "Record Vitals-BP Out of Scope-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("RecordVitalsBPOutofScopeNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            break;
                        }




                    case "Record Vitals-Ht/Wt Out of Scope-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("RecordVitalsHtWtOutofScopeDenominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            break;
                        }

                    case "Record Vitals-Ht/Wt Out of Scope-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("RecordVitalsHtWtOutofScopeNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            break;
                        }
                    //3.Ht/Wt Out of Scope


                    case "Record Vitals-Ht/Wt Out of Scope -d-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("RecordVitalsHtWtOutofScopeDenominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            break;
                        }

                    case "Record Vitals-Ht/Wt Out of Scope -n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("RecordVitalsHtWtOutofScopeNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            break;
                        }



                    //End Mohan

                    case "Medication Reconciliation-n-Stage 2":
                        {
                            IQuery query = iMySession.GetNamedQuery("MedicationReconciliationNUMERStage2");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            break;

                            //    string sInputXML = string.Empty;
                            //    string sOutputXML = string.Empty;
                            //    string sUserName = string.Empty;
                            //    string sMACAddress = string.Empty;
                            //    ulong phyid = 0;
                            //    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
                            //    RCopiaGenerateXML rcopiaXML = new RCopiaGenerateXML();
                            //    RCopiaXMLResponseProcess rcopiaResponseXML = new RCopiaXMLResponseProcess();

                            //    DateTime LstDate = Convert.ToDateTime(FromDate);
                            //    sInputXML = rcopiaXML.CreateGetReviewStatusXML("get_review_status", LstDate);
                            //    if (rcopiaSessionMngr.UploadAddress != null)
                            //    {
                            //        sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.UploadAddress + sInputXML, 1);
                            //    }

                            //    XmlDocument XMLDoc = new XmlDocument();

                            //    if (sOutputXML != null && sOutputXML != string.Empty)
                            //    {

                            //        DataTable dt = new DataTable();
                            //        dt.Columns.Add("Patient Account", typeof(string));
                            //        dt.Columns.Add("Patient Name", typeof(string));
                            //        dt.Columns.Add("Medication Review Status ", typeof(string));
                            //        dt.Columns.Add("Allergy Review Status", typeof(string));
                            //        dt.Columns.Add("Problem Review Status", typeof(string));

                            //        XMLDoc.LoadXml(sOutputXML);
                            //        XmlNodeList XMLList;
                            //        bool IsEligible = false;
                            //        //Get the number of nodes in the xml document  
                            //        string elementValue = "PatientReviewStatus";
                            //        XMLList = XMLDoc.GetElementsByTagName(elementValue);

                            //        foreach (XmlElement elem in XMLList)
                            //        {

                            //            string MedReviewBy = string.Empty;
                            //            string AllergyReviewBy = string.Empty;
                            //            string ProblemReviewBy = string.Empty;
                            //            //string RCopiaUserName = Session["ClientRCopiaUserName"].ToString();
                            //            string RCopiaUserName = sRcopiaUserName;
                            //            string MedRvwHasRecords = elem["MedicationReviewStatus"].ChildNodes[1].InnerText;

                            //            string AllergyRvwHasRecords = elem["AllergyReviewStatus"].ChildNodes[1].InnerText;
                            //            string ProblemRvwHasRecords = elem["ProblemReviewStatus"].ChildNodes[0].InnerText;
                            //            if (MedRvwHasRecords.ToUpper() == "Y")
                            //            {
                            //                if (elem["MedicationReviewStatus"].ChildNodes[2] != null)
                            //                {
                            //                    MedReviewBy = elem["MedicationReviewStatus"].ChildNodes[2].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText;
                            //                }
                            //            }
                            //            if (AllergyRvwHasRecords.ToUpper() == "Y")
                            //            {
                            //                if (elem["AllergyReviewStatus"].ChildNodes[2] != null)
                            //                {
                            //                    AllergyReviewBy = elem["AllergyReviewStatus"].ChildNodes[2].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText;
                            //                }
                            //            }

                            //            if (ProblemRvwHasRecords.ToUpper() == "Y")
                            //            {
                            //                if (elem["ProblemReviewStatus"].ChildNodes[1] != null)
                            //                {
                            //                    ProblemReviewBy = elem["ProblemReviewStatus"].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText;
                            //                }
                            //            }

                            //            if (MedRvwHasRecords.ToUpper() == "Y" && MedReviewBy == RCopiaUserName)
                            //            {
                            //                DataRow dr = dt.NewRow();
                            //                dr["Patient Account"] = elem["Patient"].ChildNodes[1].InnerText;
                            //                dr["Patient Name"] = elem["Patient"].ChildNodes[3].InnerText + "," + " " + elem["Patient"].ChildNodes[2].InnerText;
                            //                if (MedRvwHasRecords.ToUpper() == "Y")
                            //                {
                            //                    dr["Medication Review Status "] = "YES";
                            //                }
                            //                //else
                            //                //{
                            //                //    dr["Medication Review Status "] = "NO";
                            //                //}
                            //                if (AllergyRvwHasRecords.ToUpper() == "Y")
                            //                {
                            //                    dr["Allergy Review Status"] = "YES";
                            //                }
                            //                else
                            //                {
                            //                    dr["Allergy Review Status"] = "NO";
                            //                }
                            //                if (ProblemRvwHasRecords.ToUpper() == "Y")
                            //                {
                            //                    dr["Problem Review Status"] = "YES";
                            //                }
                            //                else
                            //                {
                            //                    dr["Problem Review Status"] = "NO";
                            //                }

                            //                dt.Rows.Add(dr);
                            //                IsEligible = false;
                            //            }
                            //        }
                            //        DataSet ds = new DataSet();
                            //        ds.Tables.Add(dt);
                            //        //if (ds.Tables[0].Rows.Count > 0)
                            //        //    iCount = ds.Tables[0].Rows.Count;

                            //        if (ds.Tables[0].Rows.Count > 0)
                            //        {
                            //            Result = ds.Tables[0].Rows.Count.ToString();
                            //            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            //            {
                            //                if (k == 0)
                            //                    Result = Result + "|" + ds.Tables[0].Rows[k].Field<string>("Patient Account");
                            //                else
                            //                    Result = Result + "," + ds.Tables[0].Rows[k].Field<string>("Patient Account");
                            //            }
                            //        }
                            //    }

                            //    break;
                        }
                    case "Medication Reconciliation-d-Stage 2":
                        {

                            IQuery query = iMySession.GetNamedQuery("MedicationReconciliationDENOMStage2");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            break;

                            //    string sInputXML = string.Empty;
                            //    string sOutputXML = string.Empty;
                            //    string sUserName = string.Empty;
                            //    string sMACAddress = string.Empty;
                            //    ulong phyid = 0;
                            //    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
                            //    RCopiaGenerateXML rcopiaXML = new RCopiaGenerateXML();
                            //    RCopiaXMLResponseProcess rcopiaResponseXML = new RCopiaXMLResponseProcess();

                            //    DateTime LstDate = Convert.ToDateTime(FromDate);
                            //    sInputXML = rcopiaXML.CreateGetReviewStatusXML("update_patient_office_visits", LstDate);
                            //    if (rcopiaSessionMngr.UploadAddress != null)
                            //    {
                            //        sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.UploadAddress + sInputXML, 1);
                            //    }

                            //    XmlDocument XMLDoc = new XmlDocument();

                            //    if (sOutputXML != null && sOutputXML != string.Empty)
                            //    {

                            //        XMLDoc.LoadXml(sOutputXML);
                            //        XmlNodeList XMLList;
                            //        string elementValue = "OfficeVisit";
                            //        XMLList = XMLDoc.GetElementsByTagName(elementValue);


                            //        if (XMLList.Count > 0)
                            //            Result = XMLList.Count + "|0";
                            //    }

                            //    break;
                        }






                    //saravanan




                    case "Medication Reconciliation-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("MedicationReconciliationNUMERStage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            break;


                            //string sInputXML = string.Empty;
                            //string sOutputXML = string.Empty;
                            //string sUserName = string.Empty;
                            //string sMACAddress = string.Empty;
                            //ulong phyid = 0;
                            //RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
                            //RCopiaGenerateXML rcopiaXML = new RCopiaGenerateXML();
                            //RCopiaXMLResponseProcess rcopiaResponseXML = new RCopiaXMLResponseProcess();

                            //DateTime LstDate = Convert.ToDateTime(FromDate);
                            //sInputXML = rcopiaXML.CreateGetReviewStatusXML("get_review_status", LstDate);
                            //if (rcopiaSessionMngr.UploadAddress != null)
                            //{
                            //    sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.UploadAddress + sInputXML, 1);
                            //}

                            //XmlDocument XMLDoc = new XmlDocument();

                            //if (sOutputXML != null && sOutputXML != string.Empty)
                            //{

                            //    DataTable dt = new DataTable();
                            //    dt.Columns.Add("Patient Account", typeof(string));
                            //    dt.Columns.Add("Patient Name", typeof(string));
                            //    dt.Columns.Add("Medication Review Status ", typeof(string));
                            //    dt.Columns.Add("Allergy Review Status", typeof(string));
                            //    dt.Columns.Add("Problem Review Status", typeof(string));

                            //    XMLDoc.LoadXml(sOutputXML);
                            //    XmlNodeList XMLList;
                            //    bool IsEligible = false;
                            //    //Get the number of nodes in the xml document  
                            //    string elementValue = "PatientReviewStatus";
                            //    XMLList = XMLDoc.GetElementsByTagName(elementValue);

                            //    foreach (XmlElement elem in XMLList)
                            //    {

                            //        string MedReviewBy = string.Empty;
                            //        string AllergyReviewBy = string.Empty;
                            //        string ProblemReviewBy = string.Empty;
                            //        //string RCopiaUserName = Session["ClientRCopiaUserName"].ToString();
                            //        string RCopiaUserName = sRcopiaUserName;
                            //        string MedRvwHasRecords = elem["MedicationReviewStatus"].ChildNodes[1].InnerText;

                            //        string AllergyRvwHasRecords = elem["AllergyReviewStatus"].ChildNodes[1].InnerText;
                            //        string ProblemRvwHasRecords = elem["ProblemReviewStatus"].ChildNodes[0].InnerText;
                            //        if (MedRvwHasRecords.ToUpper() == "Y")
                            //        {
                            //            if (elem["MedicationReviewStatus"].ChildNodes[2] != null)
                            //            {
                            //                MedReviewBy = elem["MedicationReviewStatus"].ChildNodes[2].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText;
                            //            }
                            //        }
                            //        if (AllergyRvwHasRecords.ToUpper() == "Y")
                            //        {
                            //            if (elem["AllergyReviewStatus"].ChildNodes[2] != null)
                            //            {
                            //                AllergyReviewBy = elem["AllergyReviewStatus"].ChildNodes[2].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText;
                            //            }
                            //        }

                            //        if (ProblemRvwHasRecords.ToUpper() == "Y")
                            //        {
                            //            if (elem["ProblemReviewStatus"].ChildNodes[1] != null)
                            //            {
                            //                ProblemReviewBy = elem["ProblemReviewStatus"].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText;
                            //            }
                            //        }

                            //        if (MedRvwHasRecords.ToUpper() == "Y" && MedReviewBy == RCopiaUserName)
                            //        {
                            //            DataRow dr = dt.NewRow();
                            //            dr["Patient Account"] = elem["Patient"].ChildNodes[1].InnerText;
                            //            dr["Patient Name"] = elem["Patient"].ChildNodes[3].InnerText + "," + " " + elem["Patient"].ChildNodes[2].InnerText;
                            //            if (MedRvwHasRecords.ToUpper() == "Y")
                            //            {
                            //                dr["Medication Review Status "] = "YES";
                            //            }
                            //            //else
                            //            //{
                            //            //    dr["Medication Review Status "] = "NO";
                            //            //}
                            //            if (AllergyRvwHasRecords.ToUpper() == "Y")
                            //            {
                            //                dr["Allergy Review Status"] = "YES";
                            //            }
                            //            else
                            //            {
                            //                dr["Allergy Review Status"] = "NO";
                            //            }
                            //            if (ProblemRvwHasRecords.ToUpper() == "Y")
                            //            {
                            //                dr["Problem Review Status"] = "YES";
                            //            }
                            //            else
                            //            {
                            //                dr["Problem Review Status"] = "NO";
                            //            }

                            //            dt.Rows.Add(dr);
                            //            IsEligible = false;
                            //        }
                            //    }
                            //    DataSet ds = new DataSet();
                            //    ds.Tables.Add(dt);
                            //    //if (ds.Tables[0].Rows.Count > 0)
                            //    //    iCount = ds.Tables[0].Rows.Count;

                            //    if (ds.Tables[0].Rows.Count > 0)
                            //    {
                            //        Result = ds.Tables[0].Rows.Count.ToString();
                            //        for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            //        {
                            //            if (k == 0)
                            //                Result = Result + "|" + ds.Tables[0].Rows[k].Field<string>("Patient Account");
                            //            else
                            //                Result = Result + "," + ds.Tables[0].Rows[k].Field<string>("Patient Account");
                            //        }
                            //    }
                            //}

                            //break;
                        }
                    case "Medication Reconciliation-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("MedicationReconciliationDENOMStage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            break;
                            //string sInputXML = string.Empty;
                            //string sOutputXML = string.Empty;
                            //string sUserName = string.Empty;
                            //string sMACAddress = string.Empty;
                            //ulong phyid = 0;
                            //RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
                            //RCopiaGenerateXML rcopiaXML = new RCopiaGenerateXML();
                            //RCopiaXMLResponseProcess rcopiaResponseXML = new RCopiaXMLResponseProcess();

                            //DateTime LstDate = Convert.ToDateTime(FromDate);
                            //sInputXML = rcopiaXML.CreateGetReviewStatusXML("update_patient_office_visits", LstDate);
                            //if (rcopiaSessionMngr.UploadAddress != null)
                            //{
                            //    sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.UploadAddress + sInputXML, 1);
                            //}

                            //XmlDocument XMLDoc = new XmlDocument();

                            //if (sOutputXML != null && sOutputXML != string.Empty)
                            //{

                            //    XMLDoc.LoadXml(sOutputXML);
                            //    XmlNodeList XMLList;
                            //    string elementValue = "OfficeVisit";
                            //    XMLList = XMLDoc.GetElementsByTagName(elementValue);


                            //    if (XMLList.Count > 0)
                            //        Result = XMLList.Count + "|0";
                            //}

                            //break;
                        }
                    case "Maintain up-to-date problem list-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("MaintainProblemListNumerator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);

                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Maintain up-to-date problem list-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("MaintainProblemListDenominator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Record demographics-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("RecordDemographicsNumerator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Record demographics-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("RecordDemographicsDenominator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Vital signs-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("RecordVitalsAllWithinScopeNumerator");

                            //query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            //query.SetString(3, Convert.ToString(PhysicianID));
                            //query.SetString(4, FromDate);
                            //query.SetString(5, ToDate);
                            //query.SetString(3, Convert.ToString(vitalAge));
                            if (tempDenominator.Count > 0)
                            {
                                query.SetParameterList("HumanID", tempDenominator);
                                ArrayList temp = null;
                                temp = new ArrayList(query.List());
                                if (temp != null && temp.Count > 0)
                                {
                                    Result = temp.Count.ToString();
                                    for (int k = 0; k < temp.Count; k++)
                                    {
                                        if (k == 0)
                                            Result = Result + "|" + (temp[k].ToString());
                                        else
                                            Result = Result + "," + (temp[k].ToString());
                                    }
                                }
                            }
                            break;
                        }
                    case "Vital signs-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("RecordVitalsAllWithinScopeDenominator");

                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            //query.SetString(3, Convert.ToString(vitalAge));
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Smoking status-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("SmokingStatusNumerator.Stage1");

                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(SmokingAge));
                            query.SetString(4, Convert.ToString(PhysicianID));
                            query.SetString(5, FromDate);
                            query.SetString(6, ToDate);
                            query.SetString(7, Convert.ToString(SmokingAge));
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Smoking status-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("SmokingStatusDenominator.Stage1");

                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(SmokingAge));
                            query.SetString(4, Convert.ToString(PhysicianID));
                            query.SetString(5, FromDate);
                            query.SetString(6, ToDate);
                            query.SetString(7, Convert.ToString(SmokingAge));
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Patient specific education resources-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientEducationNumerator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Patient specific education resources-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientEducationDenominator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Clinical Lab Test Results-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("ClinicalLabTestResultsNumerator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Clinical Lab Test Results-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("ClinicalLabTestResultsDenominator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Active Medication List-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("ActiveMedicationListNumerator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Active Medication List-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("ActiveMedicationListDenominator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Maintain active medication allergy list-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("MaintainActiveMedicationAllergyListNumerator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Maintain active medication allergy list-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("MaintainActiveMedicationAllergyListDenominator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "CPOE for Medication Orders-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("CPOEMedicationOrdersListNumerator.Stage1");
                            UserManager objPhysicianManager = new UserManager();
                            IList<User> lstphy = objPhysicianManager.getUserByPHYID(PhysicianID);

                            query.SetString(0, lstphy[0].user_name);
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "CPOE for Medication Orders-d-Stage 1":
                        {

                            UserManager objPhysicianManager = new UserManager();
                            IList<User> lstphy = objPhysicianManager.getUserByPHYID(PhysicianID);
                            IQuery query = iMySession.GetNamedQuery("CPOEMedicationOrdersListDenominator.Stage1");
                            query.SetString(0, lstphy[0].user_name);

                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = temp[k].ToString();
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }


                            //

                            IQuery query1 = iMySession.GetNamedQuery("CPOEMedicationOrdersListDenominatorPart-stage2");
                            query1.SetString(0, Convert.ToString(PhysicianID));
                            query1.SetString(1, FromDate);
                            query1.SetString(2, ToDate);
                            query1.SetString(3, Convert.ToString(PhysicianID));
                            query1.SetString(4, FromDate);
                            query1.SetString(5, ToDate);


                            ArrayList temp1 = null;
                            temp1 = new ArrayList(query1.List());

                            ulong count = 0;
                            count = Convert.ToUInt32(temp.Count);

                            if (temp1 != null && temp1.Count > 0)
                            {
                                for (int x = 0; x < temp1.Count; x++)
                                {

                                    IList<object> lstObje = (IList<object>)temp1[x];
                                    for (int y = 0; y < lstObje.Count; y++)
                                    {
                                        if (y == 1)
                                            count += Convert.ToUInt32(lstObje[y]);
                                        else
                                            Result = Result + "," + (lstObje[y].ToString());

                                    }


                                    //if (x == 0)
                                    //    Result = Result + "|" + (temp[k].ToString());
                                    //else
                                    //    Result = Result + "," + (temp[k].ToString());
                                }
                            }

                            Result = count.ToString() + "|" + Result;
                            break;
                        }
                    case "Clinical Summaries-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("ClinicalSummariesNumerator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Clinical Summaries-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("ClinicalSummariesDenominator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Transition of Care Summary-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("SummaryofCareMeasure1Numerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Transition of Care Summary-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("SummaryofCareMeasure1Denominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }

                    case "E-Prescribing (eRx)-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("EPrescribingNumerator.Stage1");
                            UserManager objPhysicianManager = new UserManager();
                            IList<User> lstphy = objPhysicianManager.getUserByPHYID(PhysicianID);
                            query.SetString(0, lstphy[0].user_name);
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "E-Prescribing (eRx)-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("EPrescribingDenominator.Stage1");
                            UserManager objPhysicianManager = new UserManager();
                            IList<User> lstphy = objPhysicianManager.getUserByPHYID(PhysicianID);
                            query.SetString(0, lstphy[0].user_name);
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Electronic Copy of Health Information-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("ElectronicCopyHealthInformationNumerator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Electronic Copy of Health Information-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("ElectronicCopyHealthInformationDenominator.Stage1");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Patient Electronic Access-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientElectronicAccessMeasure1Numerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Patient Electronic Access-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientElectronicAccessMeasure1Denominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }


                    case "Patient Electronic Access MeasureB-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientElectronicAccessMeasure2Numerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Patient Electronic Access MeasureB-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientElectronicAccessMeasure2Denominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Patient Reminders-n-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientReminderNumerator.Stage1");

                            query.SetString(0, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            query.SetString(1, FromDate.Split(' ')[0]);
                            query.SetString(2, FromDate.Split(' ')[0]);
                            query.SetString(3, FromDate);
                            query.SetString(4, ToDate);
                            query.SetString(5, Convert.ToString(PhysicianID));
                            //query.SetString(1, FromDate);
                            //query.SetString(2, ToDate);
                            query.SetString(6, FromDate.Split(' ')[0]);
                            query.SetString(7, FromDate.Split(' ')[0]);
                            query.SetString(8, FromDate);
                            query.SetString(9, ToDate);
                            //query.SetString(5, Convert.ToString(SmokingAge));
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "Patient Reminders-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("PatientReminderDenominator.Stage1");
                            query.SetString(0, FromDate.Split(' ')[0]);
                            query.SetString(1, FromDate.Split(' ')[0]);
                            query.SetString(2, Convert.ToString(PhysicianID));
                            query.SetString(3, FromDate.Split(' ')[0]);
                            query.SetString(4, FromDate.Split(' ')[0]);
                            query.SetString(5, Convert.ToString(PhysicianID));
                            //query.SetString(3, FromDate);
                            //query.SetString(4, ToDate);
                            //query.SetString(5, Convert.ToString(SmokingAge));
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    //added by naveena
                    case "CPOE for Medication Orders - core-n-Stage 1":
                        {
                            UserManager objPhysicianManager = new UserManager();
                            IList<User> lstphy = objPhysicianManager.getUserByPHYID(PhysicianID);

                            IQuery query = iMySession.GetNamedQuery("CPOEMedicationOrders1ListNumerator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate.Split(' ')[0]);
                            query.SetString(2, ToDate.Split(' ')[0]);
                            query.SetString(3, lstphy[0].user_name);
                            query.SetString(4, Convert.ToString(PhysicianID));
                            query.SetString(5, FromDate.Split(' ')[0]);
                            query.SetString(6, ToDate.Split(' ')[0]);
                            query.SetString(7, lstphy[0].user_name);
                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        }
                    case "CPOE for Medication Orders - core-d-Stage 1":
                        {
                            IQuery query = iMySession.GetNamedQuery("CPOEMedicationOrders1ListDenominator");
                            query.SetString(0, Convert.ToString(PhysicianID));
                            query.SetString(1, FromDate);
                            query.SetString(2, ToDate);
                            query.SetString(3, Convert.ToString(PhysicianID));
                            query.SetString(4, FromDate);
                            query.SetString(5, ToDate);

                            ArrayList temp = null;
                            temp = new ArrayList(query.List());
                            if (temp != null && temp.Count > 0)
                            {
                                Result = temp.Count.ToString();
                                for (int k = 0; k < temp.Count; k++)
                                {
                                    if (k == 0)
                                        Result = Result + "|" + (temp[k].ToString());
                                    else
                                        Result = Result + "," + (temp[k].ToString());
                                }
                            }
                            break;
                        } //

                    default:
                        break;
                }
                iMySession.Close();
            }
            return Result;
        }
        //public int UpdateEncounterWithSession(Encounter objEnc, ISession Mysession, string MacAddress)
        //{
        //    IList<Encounter> SaveList = null;
        //    IList<Encounter> UpdateList = new List<Encounter>();
        //    UpdateList.Add(objEnc);
        //    return SaveUpdateDeleteWithoutTransaction(ref SaveList, UpdateList, null, Mysession, MacAddress);
        //}

        //muthu on 9-jun-2011
        public DataSet DtblAppointmentsReport(DateTime From_date, DateTime To_Date, ulong Physician_Name, ulong Human_Name, string[] ObjType, string Facility_Name, string[] currentProcess, string PlanName, ulong EncPhysicianName, string CarrierName, ulong PcpID, string strCreatedBy)
        {
            ArrayList arylstColumnsOfAppointmentsReport;
            ArrayList arylstAppointment;
            DataTable dtblAppointments = new DataTable();
            DataSet dsAppointmentResult = new DataSet();
            DataSet AppointmentDetailsReport = new DataSet();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (CarrierName != string.Empty)
                {
                    goto l;
                }

                if (PlanName == string.Empty)
                {
                    IQuery query2 = iMySession.GetNamedQuery("Fill.GetAppointmentsReport.ColumnHeadings");
                    arylstColumnsOfAppointmentsReport = new ArrayList(query2.List());

                    if (arylstColumnsOfAppointmentsReport.Count > 0)
                    {
                        object[] objlstColumns = (object[])arylstColumnsOfAppointmentsReport[0];
                        dtblAppointments.Columns.Clear();
                        for (int i = 0; i < objlstColumns.Length; i++)
                        {
                            if (i == 4 || i == 12 || i == 13)
                            {
                                dtblAppointments.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                            }
                            else
                            {
                                dtblAppointments.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                            }
                        }
                    }

                    IQuery query1 = iMySession.GetNamedQuery("Reports.AppointmentsReport");
                    if (Facility_Name.Trim() == string.Empty && Physician_Name == 0 && Human_Name == 0 && PlanName.Trim() == string.Empty && EncPhysicianName == 0 && CarrierName.Trim() == string.Empty && PcpID == 0 && strCreatedBy == string.Empty)
                    {
                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));

                    }
                    if (ObjType[0].Contains(","))
                    {
                        string[] ss = ObjType[0].Split(',');
                        query1.SetParameterList("objlist", ss);
                    }
                    else
                    {
                        query1.SetParameterList("objlist", ObjType);
                    }
                    query1.SetParameterList("Currentlist", currentProcess);
                    if (Facility_Name.Trim() != string.Empty)
                    {
                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));

                        query1.SetString(2, Facility_Name);
                    }
                    else
                    {
                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(2, "%");
                    }
                    if (Physician_Name != 0)
                    {
                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(3, Physician_Name.ToString());
                    }
                    else
                    {
                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(3, "%");
                    }
                    if (Human_Name != 0)
                    {
                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(4, Human_Name.ToString());
                    }
                    else
                    {
                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(4, "%");
                    }
                    //if (PcpID != 0)
                    //{
                    //    query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                    //    query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                    //    query1.SetString(5, PcpID.ToString());


                    //}
                    //else
                    //{
                    //    query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                    //    query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                    //    query1.SetString(5, "%");
                    //}
                    if (EncPhysicianName != 0)
                    {
                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(5, EncPhysicianName.ToString());


                    }
                    else
                    {
                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(5, "%");

                    }
                    if (strCreatedBy != string.Empty)
                    {
                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(6, strCreatedBy.ToString());


                    }
                    else
                    {
                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(6, "%");

                    }


                    arylstAppointment = new ArrayList(query1.List());

                    if (arylstAppointment != null)
                    {
                        for (int h = 0; h < arylstAppointment.Count; h++)
                        {

                            object[] objlst = (object[])arylstAppointment[h];
                            DataRow dr = dtblAppointments.NewRow();
                            if (objlst.Length > 0)
                            {
                                for (int k = 0; k < objlst.Length; k++)
                                {

                                    try
                                    {

                                        //if (objlst[k].ToString().Split(',').Length > 0)
                                        //{
                                        //    if (objlst[k].ToString().Split(',')[0] == string.Empty && objlst[k].ToString().Split(',')[1] == string.Empty)
                                        //        objlst[k] = string.Empty;
                                        //    else if (objlst[k].ToString().Split(',')[0] == string.Empty)
                                        //        objlst[k] = objlst[k].ToString().Split(',')[1];
                                        //}

                                        dr[k] = objlst[k].ToString();
                                    }
                                    catch
                                    {
                                        //string hl;
                                    }

                                }
                                dtblAppointments.Rows.Add(dr);
                            }
                        }
                    }


                    //   DataSet AppointmentDetailsReport = new DataSet();
                    AppointmentDetailsReport.Tables.Add(dtblAppointments);
                    dsAppointmentResult = AppointmentDetailsReport;
                    //return AppointmentDetailsReport;
                }
            l:
                if (PlanName != string.Empty || CarrierName != string.Empty)
                {
                    IQuery query2 = iMySession.GetNamedQuery("Fill.GetAppointmentsReport.ColumnHeadings.withplan");
                    arylstColumnsOfAppointmentsReport = new ArrayList(query2.List());
                    if (arylstColumnsOfAppointmentsReport.Count > 0)
                    {
                        object[] objlstColumns = (object[])arylstColumnsOfAppointmentsReport[0];
                        dtblAppointments.Columns.Clear();
                        for (int i = 0; i < objlstColumns.Length; i++)
                        {
                            if (i == 4 || i == 14 || i == 15)
                            {
                                dtblAppointments.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                            }
                            else
                            {
                                dtblAppointments.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                            }
                        }
                    }
                    if (PlanName == "ALL" || CarrierName == "ALL" || PlanName != string.Empty || CarrierName != string.Empty)
                    {
                        IQuery query1 = iMySession.GetNamedQuery("ReportsAppointmentsReport.WithPlan_Name");
                        if (PlanName == "ALL")
                        {
                            PlanName = string.Empty;
                        }
                        if (CarrierName == "ALL")
                        {
                            CarrierName = string.Empty;
                        }

                        if (Facility_Name.Trim() == string.Empty && Physician_Name == 0 && Human_Name == 0 && PlanName.Trim() == string.Empty && EncPhysicianName == 0 && CarrierName.Trim() == string.Empty && PcpID == 0 && strCreatedBy == string.Empty)
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));

                        }
                        if (ObjType[0].Contains(","))
                        {
                            string[] ss = ObjType[0].Split(',');
                            query1.SetParameterList("objList", ss);
                        }
                        else
                        {
                            query1.SetParameterList("objList", ObjType);
                        }
                        query1.SetParameterList("CurrentList", currentProcess);

                        if (PlanName.Trim() != string.Empty)
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));

                            query1.SetString(2, PlanName);
                        }
                        else
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(2, "%");
                        }
                        if (Facility_Name.Trim() != string.Empty)
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));

                            query1.SetString(3, Facility_Name);
                        }
                        else
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(3, "%");
                        }
                        //if (currentProcess.Trim() != string.Empty)
                        //{
                        //    query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                        //    query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                        //    query1.SetString(4, currentProcess);
                        //}
                        //else
                        //{
                        //    query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                        //    query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                        //    query1.SetString(4, "%");
                        //}
                        if (Physician_Name != 0)
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(4, Physician_Name.ToString());
                        }
                        else
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(4, "%");
                        }
                        if (Human_Name != 0)
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(5, Human_Name.ToString());
                        }
                        else
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(5, "%");
                        }
                        if (PcpID != 0)
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(6, PcpID.ToString());


                        }
                        else
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(6, "%");
                        }
                        if (EncPhysicianName != 0)
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(7, EncPhysicianName.ToString());


                        }
                        else
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(7, "%");

                        }
                        if (CarrierName != string.Empty)
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(8, CarrierName.ToString());


                        }
                        else
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(8, "%");

                        }
                        if (strCreatedBy != string.Empty)
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(9, strCreatedBy.ToString());


                        }
                        else
                        {
                            query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm tt"));
                            query1.SetString(9, "%");

                        }
                        arylstAppointment = new ArrayList(query1.List());

                        if (arylstAppointment != null)
                        {
                            for (int h = 0; h < arylstAppointment.Count; h++)
                            {

                                object[] objlst = (object[])arylstAppointment[h];
                                DataRow dr = dtblAppointments.NewRow();
                                if (objlst.Length > 0)
                                {
                                    for (int k = 0; k < objlst.Length; k++)
                                    {

                                        try
                                        {

                                            //if (objlst[k].ToString().Split(',').Length > 0)
                                            //{
                                            //    if (objlst[k].ToString().Split(',')[0] == string.Empty && objlst[k].ToString().Split(',')[1] == string.Empty)
                                            //        objlst[k] = string.Empty;
                                            //    else if (objlst[k].ToString().Split(',')[0] == string.Empty)
                                            //        objlst[k] = objlst[k].ToString().Split(',')[1];
                                            //}

                                            dr[k] = objlst[k].ToString();
                                        }
                                        catch
                                        {

                                        }

                                    }
                                    dtblAppointments.Rows.Add(dr);
                                }
                            }
                        }



                        AppointmentDetailsReport.Tables.Add(dtblAppointments);
                        dsAppointmentResult = AppointmentDetailsReport;
                        //return AppointmentDetailsReport;
                    }
                    iMySession.Close();

                }

            }
            return dsAppointmentResult;
        }
        //muthu on 9-jun-2011

        //muthu on 9-jun-2011
        //public UserDTO FindUser(string UserName, int iPageNumber, int iMaxResult)
        //{
        //    UserDTO objuser = new UserDTO();
        //    IList<User> userList = new List<User>();
        //    ArrayList arrList = null;
        //    IQuery iquery1 = null;
        //    using (
        //    ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        if (UserName != null)
        //        {
        //            IQuery query1 = iMySession.GetNamedQuery("Fill.UserName");
        //            query1.SetString(0, Convert.ToString(UserName + "%"));
        //            if (query1.List().Count > 0)
        //                objuser.userCount = Convert.ToInt32(query1.List()[0]);


        //            iquery1 = iMySession.GetNamedQuery("Fill.FindUserName");
        //            iquery1.SetString(0, UserName + "%");
        //            int PageNumber = iPageNumber - 1;
        //            iquery1.SetInt32(1, PageNumber * iMaxResult);
        //            iquery1.SetInt32(2, iMaxResult);
        //        }
        //        arrList = new ArrayList(iquery1.List());
        //        if (arrList.Count > 0)
        //        {
        //            foreach (object[] obj in arrList)
        //            {
        //                User user = new User();
        //                user.user_name = obj[0].ToString();
        //                user.Physician_Library_ID = Convert.ToUInt32(obj[1]);
        //                user.role = obj[2].ToString();
        //                user.person_name = obj[3].ToString();
        //                userList.Add(user);
        //            }
        //        }
        //        objuser.user = userList;
        //        iMySession.Close();
        //    }
        //    return objuser;
        //}
        //muthu on 9-jun-2011

        #endregion

        public FillEncounterandWFObject GetEncounterandWFObject(ulong ulEncID, ulong ulAddendumId, string strObjTpe)
        {
            FillEncounterandWFObject fillEncWFObj = new FillEncounterandWFObject();
            WFObjectManager wfobjMngr = new WFObjectManager();
            if (ulEncID == 0 && ulAddendumId != 0)
            {
                fillEncWFObj.AddendumWFRecord = wfobjMngr.GetByObjectSystemId(ulAddendumId, "ADDENDUM");
                return fillEncWFObj;
            }
            else if (ulEncID == 0 && ulAddendumId == 0)
            {
                return fillEncWFObj;
            }
            else
            {
                IList<Encounter> lstEnc = GetEncounterByEncounterID(ulEncID);
                if (lstEnc.Count > 0)
                    fillEncWFObj.EncRecord = lstEnc[0];

                if (strObjTpe == "ADDENDUM")
                {
                    fillEncWFObj.AddendumWFRecord = wfobjMngr.GetByObjectSystemId(ulAddendumId, "ADDENDUM");
                    //fillEncWFObj.EncounterWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "ENCOUNTER");
                    //fillEncWFObj.DocumentationWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "DOCUMENTATION");
                    IList<WFObject> WFObjList = wfobjMngr.GetByObjectSystemIdForEncounterandDocumentation(ulEncID);
                    if (WFObjList.Count > 0)
                    {
                        var wfobjEnc = from w in WFObjList where w.Obj_Type == "ENCOUNTER" select w;
                        IList<WFObject> wfobjEncList = wfobjEnc.ToList<WFObject>();
                        if (wfobjEncList.Count > 0)
                            fillEncWFObj.EncounterWFRecord = wfobjEncList[0];

                        var wfobjDoc = from w in WFObjList where w.Obj_Type == "DOCUMENTATION" select w;
                        IList<WFObject> wfobjDocList = wfobjDoc.ToList<WFObject>();
                        if (wfobjDocList.Count > 0)
                            fillEncWFObj.DocumentationWFRecord = wfobjDocList[0];
                    }
                }
                if (strObjTpe == "ENCOUNTER")
                {
                    fillEncWFObj.EncounterWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "ENCOUNTER");


                    IList<WFObject> WFObjList = wfobjMngr.GetByObjectSystemIdForEncounterandDocumentation(ulEncID);
                    //if (WFObjList.Count > 0)
                    //{


                    //    var wfobjDoc = from w in WFObjList where w.Obj_Type == "DOCUMENTATION" select w;
                    //    IList<WFObject> wfobjDocList = wfobjDoc.ToList<WFObject>();
                    //    if (wfobjDocList.Count > 0)
                    //        fillEncWFObj.DocumentationWFRecord = wfobjDocList[0];
                    //}

                }
                if (strObjTpe == "DOCUMENTATION")
                {
                    //fillEncWFObj.EncounterWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "ENCOUNTER");
                    //fillEncWFObj.DocumentationWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "DOCUMENTATION");
                    IList<WFObject> WFObjList = wfobjMngr.GetByObjectSystemIdForEncounterandDocumentation(ulEncID);
                    if (WFObjList.Count > 0)
                    {
                        var wfobjEnc = from w in WFObjList where w.Obj_Type == "ENCOUNTER" select w;
                        IList<WFObject> wfobjEncList = wfobjEnc.ToList<WFObject>();
                        if (wfobjEncList.Count > 0)
                            fillEncWFObj.EncounterWFRecord = wfobjEncList[0];

                        var wfobjDoc = from w in WFObjList where w.Obj_Type == "DOCUMENTATION" select w;
                        IList<WFObject> wfobjDocList = wfobjDoc.ToList<WFObject>();
                        if (wfobjDocList.Count > 0)
                            fillEncWFObj.DocumentationWFRecord = wfobjDocList[0];
                    }
                }
                if (strObjTpe == "DOCUMENT REVIEW")
                {
                    fillEncWFObj.EncounterWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "ENCOUNTER");
                    fillEncWFObj.DocumentationWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "DOCUMENTATION");
                    //fillEncWFObj.DocReviewWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "DOCUMENT REVIEW");
                }
                //fillEncWFObj.AddendumWFRecord = wfobjMngr.GetByObjectSystemId(ulAddendumId, "ADDENDUM");
                //fillEncWFObj.EncounterWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "ENCOUNTER");
                //fillEncWFObj.DocumentationWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "DOCUMENTATION");
                //fillEncWFObj.DocReviewWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "DOCUMENT REVIEW");


                //ChiefComplaintsManager ccMngr = new ChiefComplaintsManager();
                //fillEncWFObj.CC = ccMngr.FillCC(ulEncID);


                return fillEncWFObj;
            }
        }

        public FillEncounterandWFObject GetEncounterandWFObjectIncludeArchive(ulong ulEncID, ulong ulAddendumId, string strObjTpe, bool bIncludeArchive)
        {
            FillEncounterandWFObject fillEncWFObj = new FillEncounterandWFObject();
            WFObjectManager wfobjMngr = new WFObjectManager();
            if (ulEncID == 0)
            {
                fillEncWFObj.AddendumWFRecord = wfobjMngr.GetByObjectSystemIdIncludeArchive(ulAddendumId, "ADDENDUM");
                return fillEncWFObj;
            }
            else
            {
                IList<Encounter> lstEnc = GetEncounterByEncounterIDIncludeArchive(ulEncID);
                if (lstEnc.Count > 0)
                    fillEncWFObj.EncRecord = lstEnc[0];

                //if (strObjTpe == "ADDENDUM")
                //{
                fillEncWFObj.AddendumWFRecord = wfobjMngr.GetByObjectSystemIdIncludeArchive(ulAddendumId, "ADDENDUM");
                fillEncWFObj.EncounterWFRecord = wfobjMngr.GetByObjectSystemIdIncludeArchive(ulEncID, "ENCOUNTER");
                fillEncWFObj.DocumentationWFRecord = wfobjMngr.GetByObjectSystemIdIncludeArchive(ulEncID, "DOCUMENTATION");
                //}
                //if (strObjTpe == "ENCOUNTER")
                //{
                //    fillEncWFObj.EncounterWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "ENCOUNTER");
                //}
                //if (strObjTpe == "DOCUMENTATION")
                //{
                //    fillEncWFObj.EncounterWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "ENCOUNTER");
                //    fillEncWFObj.DocumentationWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "DOCUMENTATION");
                //}
                //if (strObjTpe == "DOCUMENT REVIEW")
                //{
                //    fillEncWFObj.EncounterWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "ENCOUNTER");
                //    fillEncWFObj.DocumentationWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "DOCUMENTATION");
                //    fillEncWFObj.DocReviewWFRecord = wfobjMngr.GetByObjectSystemId(ulEncID, "DOCUMENT REVIEW");
                //}
                return fillEncWFObj;
            }
        }

        #region SubmitOrdersAndPrescriptions


        public void SubmitOrdersAndPrescriptions(ulong ulMyEncounterID, ulong ulMyHumanID, ulong ulMyPhysicianID, string UserName, string MACAddress, string FacilityName, DateTime currentDateTime)
        {
            WFObjectManager objWfManager = new WFObjectManager();
            Encounter EncRecord = GetById(ulMyEncounterID);

            #region InHouseSubmit
            InHouseProcedureManager objInHouseMngr = new InHouseProcedureManager();
            InHouseProcedureDTO otherDTO = objInHouseMngr.FillInHouseProcedure(ulMyEncounterID, ulMyPhysicianID, ulMyHumanID);
            IList<InHouseProcedure> otherProSubmitList = new List<InHouseProcedure>();
            if (otherDTO != null && otherDTO.OtherProcedure.Count > 0)
            {
                foreach (InHouseProcedure obj in otherDTO.OtherProcedure)
                {
                    if (obj.In_House_Procedure_Group_ID == 0)
                    {
                        obj.In_House_Procedure_Group_ID = otherDTO.MaxGroupId + 1;
                        obj.Modified_Date_And_Time = currentDateTime;
                        otherProSubmitList.Add(obj);
                    }
                }
                if (otherProSubmitList != null && otherProSubmitList.Count > 0)
                {
                    ulong GroupId = (ulong)objInHouseMngr.SubmitInHouseProcedures(otherProSubmitList, MACAddress);
                    if (GroupId != 0)
                    {
                        WFObject TempWF = new WFObject();
                        TempWF = objWfManager.GetByObjectSystemId(GroupId, "INTERNAL ORDER");
                        if (EncRecord != null)
                        {
                            objWfManager.MoveToNextProcess(TempWF.Obj_System_Id, TempWF.Obj_Type, 2, EncRecord.Assigned_Med_Asst_User_Name, currentDateTime, MACAddress, null, null);
                        }
                    }
                }
            }
            #endregion

            #region SubmitImmunization

            ImmunizationManager objImmunMngr = new ImmunizationManager();
            ImmunizationDTO immunDTO = objImmunMngr.FillImmunization(ulMyHumanID, ulMyEncounterID);
            IList<Immunization> immunSubmitList = new List<Immunization>();
            if (immunDTO != null && immunDTO.Immunization.Count > 0)
            {
                foreach (Immunization obj in immunDTO.Immunization)
                {
                    if (obj.Immunization_Group_ID == 0 && obj.Vaccine_In_House == "Y")
                    {
                        obj.Immunization_Group_ID = immunDTO.MaxGroupId + 1;
                        obj.Modified_Date_And_Time = currentDateTime;
                        immunSubmitList.Add(obj);
                    }
                }
                if (immunSubmitList != null && immunSubmitList.Count > 0)
                {
                    ulong immunsubmitId = (ulong)objImmunMngr.SubmitImmunization(immunSubmitList, MACAddress);
                    if (immunsubmitId != 0)
                    {
                        WFObject TempWF = new WFObject();
                        TempWF = objWfManager.GetByObjectSystemId(immunsubmitId, "IMMUNIZATION ORDER");
                        // objWfManager.MoveToNextProcess(TempWF.Obj_System_Id, TempWF.Obj_Type, 2, EncRecord.Assigned_Med_Asst_User_Name, currentDateTime, MACAddress, null, null);//For bug ID 48532
                        if (EncRecord != null)
                        {
                            //Cap - 712
                            //objWfManager.MoveToNextProcess(TempWF.Obj_System_Id, TempWF.Obj_Type, 5, EncRecord.Assigned_Med_Asst_User_Name, currentDateTime, MACAddress, null, null);
                            objWfManager.MoveToNextProcess(TempWF.Obj_System_Id, TempWF.Obj_Type, 5, "UNKNOWN", currentDateTime, MACAddress, null, null);
                        }
                    }
                }
            }

            #endregion

            #region Submit LabAndImageOrders

            OrdersManager objOrdMngr = new OrdersManager();
            //objOrdMngr.SubmitLabImageOrders(ulMyEncounterID, ulMyHumanID, ulMyPhysicianID, UserName, MACAddress, currentDateTime, "DIAGNOSTIC ORDER", "INTERNAL DIAGNOSTIC ORDER", EncRecord.Assigned_Med_Asst_User_Name);
            //objOrdMngr.SubmitLabImageOrders(ulMyEncounterID, ulMyHumanID, ulMyPhysicianID, UserName, MACAddress, currentDateTime, "IMAGE ORDER", "INTERNAL IMAGE ORDER", EncRecord.Assigned_Med_Asst_User_Name);
            //Comment by bala for order submit
            // objOrdMngr.SubmitOrdersToLab(ulMyHumanID, ulMyEncounterID, "DIAGNOSTIC ORDER", MACAddress, UserName, currentDateTime, EncRecord.Assigned_Med_Asst_User_Name);
            //Added by bala

            /***  commented for perfomance tuning
             * no significance for objOrdMngr.SubmitLogic,objOrdMngr.SubmitOrdersToLab
             *  by Jisha 
          ***/
            //  objOrdMngr.SubmitLogic(ulMyHumanID, ulMyEncounterID, ulMyPhysicianID, "DIAGNOSTIC ORDER", UserName, currentDateTime, FacilityName);
            //  objOrdMngr.SubmitOrdersToLab(ulMyHumanID, ulMyEncounterID, "IMAGE ORDER", MACAddress, UserName, currentDateTime, EncRecord.Assigned_Med_Asst_User_Name);


            #endregion

            #region Submit Prescription
            if (EncRecord != null)
            {
                if (EncRecord.Is_Prescription_To_Be_Moved != null && EncRecord.Is_Prescription_To_Be_Moved == "Y")
                {
                    ulong ulMyPrescriptionId = 0;
                    PrescriptionManager objPrescMngr = new PrescriptionManager();
                    Prescription prescRecord = new Prescription();
                    prescRecord.Encounter_ID = Convert.ToInt32(ulMyEncounterID);
                    prescRecord.Created_By = UserName;
                    prescRecord.Created_Date_And_Time = currentDateTime;
                    prescRecord.Facility_Name = FacilityName;
                    prescRecord.Human_ID = Convert.ToInt32(ulMyHumanID);
                    prescRecord.Modified_By = string.Empty;
                    prescRecord.Modified_Date_And_Time = DateTime.MinValue;
                    prescRecord.Physician_ID = Convert.ToInt32(ulMyPhysicianID);
                    prescRecord.Prescription_Date = currentDateTime;
                    prescRecord.Version = 0;
                    IList<Prescription> prescription = new List<Prescription>();
                    prescription.Add(prescRecord);
                    WFObject WFObj = new WFObject();
                    WFObj.Obj_Type = "E-PRESCRIBE";
                    WFObj.Current_Arrival_Time = currentDateTime;
                    WFObj.Current_Owner = EncRecord.Assigned_Med_Asst_User_Name;
                    WFObj.Fac_Name = FacilityName;
                    WFObj.Current_Process = "START";
                    ulMyPrescriptionId = objPrescMngr.SavePrescription(prescription.ToArray<Prescription>(), null, null, MACAddress, WFObj);

                }
            }
            #endregion

            #region Submit Referral Order
            // Added by Manimozhi on June 19th 2012 
            ReferralOrderManager objReferralMngr = new ReferralOrderManager();
            ReferralOrderDTO ReferralDTO = objReferralMngr.FillReferralOrder(ulMyHumanID, ulMyPhysicianID, ulMyEncounterID);
            IList<ReferralOrder> ReferralorderSubmitList = new List<ReferralOrder>();
            if (ReferralDTO != null && ReferralDTO.RefOrdList.Count > 0)
            {

                foreach (ReferralOrder obj in ReferralDTO.RefOrdList)
                {
                    if (obj.Referral_Order_Group_ID == 0)
                    {
                        if (obj.Move_To_MA == "Y")
                        {
                            obj.Referral_Order_Group_ID = ReferralDTO.MaxGroupId + 1;
                            obj.Modified_Date_And_Time = currentDateTime;
                            ReferralorderSubmitList.Add(obj);
                        }
                    }

                }

                if (ReferralorderSubmitList != null && ReferralorderSubmitList.Count > 0)
                {
                    if (EncRecord != null)
                    {
                        ulong referralsubmitId = (ulong)objReferralMngr.SubmitReferralOrder(ReferralorderSubmitList, FacilityName, EncRecord.Assigned_Med_Asst_User_Name, MACAddress);
                    }
                }

                ReferralDTO = objReferralMngr.FillReferralOrder(ulMyHumanID, ulMyPhysicianID, ulMyEncounterID);
                IList<ReferralOrder> ReferralorderSubmitListWithNoMA = new List<ReferralOrder>();
                foreach (ReferralOrder obj in ReferralDTO.RefOrdList)
                {

                    if (obj.Referral_Order_Group_ID == 0)
                    {
                        if (obj.Move_To_MA == "N")
                        {
                            obj.Referral_Order_Group_ID = ReferralDTO.MaxGroupId + 1;
                            obj.Modified_Date_And_Time = currentDateTime;
                            ReferralorderSubmitListWithNoMA.Add(obj);
                        }
                    }
                }
                if (ReferralorderSubmitListWithNoMA != null && ReferralorderSubmitListWithNoMA.Count > 0)
                {
                    if (EncRecord != null)
                    {
                        ulong referralsubmitId = (ulong)objReferralMngr.SubmitReferralOrder(ReferralorderSubmitListWithNoMA, FacilityName, EncRecord.Assigned_Med_Asst_User_Name, MACAddress);
                    }
                }
            }
            #endregion
        }

        #endregion
        public IList<Encounter> GetEncounterListForEV(ulong Human_id, string fromDate)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sqlquery = iMySession.CreateSQLQuery("SELECT e.* FROM encounter e where human_id=" + Human_id + " and appointment_date >='" + fromDate + "'").AddEntity("e", typeof(Encounter));
                ilstEncounter = sqlquery.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            // return sqlquery.List<Encounter>();
        }
        public MoveVerificationDTO PerformMoveVerification(ulong ulMyEncounterID, ulong selectedPhysicianID, ulong ulMyHumanID, DateTime currentDate, string FacilityName, string UserName, bool VerifyPFSH, string Source, string SourceOtherInfo, string userCurrentProcess, string MACAddress, string btnPhyCorrectionText, bool bDuplicateCheck, string UserRole, string btnID, bool breview, string IsACOValid, out string sAlert)
        {
            sAlert = string.Empty;
            // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

            MoveVerificationDTO objMoveVerifyDTO = new MoveVerificationDTO();
            WFObjectManager objWfMngr = new WFObjectManager();
            WFObject objDocWfObject = new WFObject();
            WFObject objEncWfObj = new WFObject();
            WFObject objDocReviewWfObj = new WFObject();
            WFObject objBillingWfObj = new WFObject();

            /***    added for perfomance tuning
             *       For MA_Process,CHECK_OUT,CHECK_OUT_WAIT,obj_type= DOCUMENTATION is not required
             *       by Jisha
             *       
             * **/
            //  WFObject objDocWfObject = objWfMngr.GetByObjectSystemId(ulMyEncounterID, "DOCUMENTATION");
            if (userCurrentProcess != "MA_PROCESS" || userCurrentProcess != "CHECK_OUT" || userCurrentProcess != "CHECK_OUT_WAIT")
            {
                objDocWfObject = objWfMngr.GetByObjectSystemId(ulMyEncounterID, "DOCUMENTATION");
            }


            objEncWfObj = objWfMngr.GetByObjectSystemId(ulMyEncounterID, "ENCOUNTER");

            //if ((objDocWfObject.Current_Process == "PROVIDER_PROCESS"))
            //{
            //    objDocReviewWfObj = objWfMngr.GetByObjectSystemId(ulMyEncounterID, "DOCUMENT REVIEW");

            //}
            Encounter EncRecord = GetById(ulMyEncounterID);
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //if (objDocWfObject != null)
                //{
                if ((objEncWfObj != null && objEncWfObj.Current_Process == "MA_PROCESS"))
                {
                    if (EncRecord.Is_PFSH_Verified != "Y")
                    {
                        if (VerifyPFSH == false)
                        {
                            objMoveVerifyDTO.IsPFSHVerified = EncRecord.Is_PFSH_Verified;
                            return objMoveVerifyDTO;
                        }
                        else
                        {
                            EncRecord.Source_Of_Information = Source;
                            EncRecord.If_Source_Of_Information_Others = SourceOtherInfo;
                            EncRecord.Is_PFSH_Verified = "Y";
                        }
                    }
                    else
                    {
                        objMoveVerifyDTO.IsPFSHVerified = EncRecord.Is_PFSH_Verified;

                    }

                    HumanManager objHumanManger = new HumanManager();
                    objMoveVerifyDTO.IsACOValid = objHumanManger.IsACOValid(ulMyHumanID);

                    if (objMoveVerifyDTO.IsACOValid && IsACOValid == "False")
                    {
                        objMoveVerifyDTO.IsWorkflowPushed = false;
                        return objMoveVerifyDTO;
                    }
                    //Move EncWfObj to 'CHECK_OUT_WAIT'

                    // Modified by Manimozhi on jan 19th 2012
                    objWfMngr = new WFObjectManager();
                    //objWfMngr.MoveToNextProcess(objEncWfObj.Obj_System_Id, objEncWfObj.Obj_Type, 4, "UNKNOWN", currentDate, MACAddress, null, null);


                    //  objWfMngr.MoveToNextProcess(objEncWfObj, 4, "UNKNOWN", currentDate, MACAddress, null);

                    // added by manimozhi on jan 18th 2012

                    if (btnPhyCorrectionText.ToUpper() != "MOVE TO CHECKOUT")
                    {
                        //Create Documentation WfObj
                        WFObject WFObj = new WFObject();
                        WFObj.Fac_Name = FacilityName;
                        WFObj.Obj_Type = "DOCUMENTATION";
                        WFObj.Parent_Obj_Type = "ENCOUNTER";
                        WFObj.Parent_Obj_System_Id = ulMyEncounterID;
                        WFObj.Obj_System_Id = ulMyEncounterID;
                        WFObj.Current_Process = "START";
                        WFObj.Current_Arrival_Time = currentDate;
                        string sOwner = string.Empty, sRole = string.Empty;
                        ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", selectedPhysicianID));
                        if (criteria.List<User>() != null && criteria.List<User>().Count > 0)
                        {
                            sRole = criteria.List<User>()[0].role;
                            sOwner = criteria.List<User>()[0].user_name;
                        }

                        WFObj.Current_Owner = sOwner;

                        EncRecord.Encounter_Provider_ID = Convert.ToInt32(selectedPhysicianID);
                        EncRecord.Modified_By = UserName;
                        EncRecord.Modified_Date_and_Time = currentDate;
                        if (sRole == "Physician")
                        {
                            EncRecord.Is_Physician_Asst_Process = "N";
                        }
                        else if (sRole == "Physician Assistant")
                        {
                            EncRecord.Is_Physician_Asst_Process = "Y";

                        }
                        EncounterManager EncMngr = new EncounterManager();
                        EncMngr.UpdateEncounterWithEncounterChild(EncRecord, WFObj, objEncWfObj, MACAddress, 1);
                        /***  commented for perfomance tuning
                          * EncRecord.Is_PFSH_Verified is already in update list.
                          *   by Jisha  ***/

                        //  EncRecord = GetById(ulMyEncounterID);
                        objMoveVerifyDTO.IsPFSHVerified = EncRecord.Is_PFSH_Verified;
                        objMoveVerifyDTO.IsWorkflowPushed = true;
                    }
                }
                else if (objDocWfObject != null)
                {
                    EAndMCodingManager emMnger = new EAndMCodingManager();
                    EandMCodingICDManager emICDMngr = new EandMCodingICDManager();
                    IList<EandMCodingICD> eandmICDList = new List<EandMCodingICD>();
                    IList<string> GcodeCheckList = new List<string>();
                    bool bGCodeCheck = false;
                    //Get E & M count

                    //Added by Bala for Duplicate Check and G Codes Check


                    /***  commented for perfomance tuning
                       *  EAndMCount is not using anywhere
                       *  by Jisha 
                     ***/


                    //  objMoveVerifyDTO.EAndMCount = emMnger.GetEMCount(ulMyEncounterID);

                    objMoveVerifyDTO.EandMCodingList = emMnger.GetEMCodeList(ulMyEncounterID);


                    /***  commented for perfomance tuning
                       *  EandMdtoList is not using anywhere
                       *  by Jisha
                     ***/
                    // objMoveVerifyDTO.EandMdtoList = emMnger.LoadEandMCoding(ulMyEncounterID, ulMyHumanID, selectedPhysicianID, "E AND M PROCEDURE", EncRecord.Date_of_Service);

                    /*Commented by valli 
                    //For G-code Check
                    //var GcodeList = from g in objMoveVerifyDTO.EandMCodingList where (g.Procedure_Code == "G0438" || g.Procedure_Code == "G0439") select g.Id;

                    //if (GcodeList.Count() > 0)
                    //{
                    //    objMoveVerifyDTO.IsGcodePresent = true;
                    //    for (int i = 0; i < GcodeList.Count(); i++)
                    //    {
                    //        eandmICDList = emICDMngr.EandMcodingList(Convert.ToUInt32(GcodeList.ElementAt(i)));
                    //        if (eandmICDList.Count > 0)
                    //        {
                    //            GcodeCheckList.Add(eandmICDList.Any(a => a.ICD.Contains("Z00.00") || a.ICD.Contains("Z00.01")).ToString());


                    //        }

                    //    }
                    //    objMoveVerifyDTO.IsICDPresent = GcodeCheckList.Contains("False") == true ? false : true;
                    //    if (Convert.ToBoolean(objMoveVerifyDTO.IsGcodePresent) == true && Convert.ToBoolean(objMoveVerifyDTO.IsICDPresent) == false)
                    //    {
                    //        bGCodeCheck = true;
                    //    }
                    //    else
                    //    {
                    //        bGCodeCheck = false;
                    //    }
                    //}
                    */
                    // Added by Valli
                    IDictionary<string, IList<string>> CPTICD = new Dictionary<string, IList<string>>();
                    //Jira CAP-998
                    //string sE_And_M_CPT_And_ICD = System.Configuration.ConfigurationSettings.AppSettings["E_And_M_CPT_And_ICD"].ToString();
                    string sE_And_M_CPT_And_ICD_Validation = string.Empty;
                    HumanManager HumanMngr = new HumanManager();
                    Human objHuman = new Human();
                    objHuman = HumanMngr.GetHumanFromHumanID(EncRecord.Human_ID);
                    int iAge = CalculateAgeByDOS(objHuman.Birth_Date, EncRecord.Date_of_Service);
                    if (iAge > 18)
                    {
                        sAlert = "180045";
                        sE_And_M_CPT_And_ICD_Validation = "E_And_M_CPT_And_ICD";
                    }
                    else
                    {
                        sAlert = "180057";
                        sE_And_M_CPT_And_ICD_Validation = "E_And_M_CPT_And_ICD_LessThenOrEqual18Age";
                    }
                    string sE_And_M_CPT_And_ICD = System.Configuration.ConfigurationSettings.AppSettings[sE_And_M_CPT_And_ICD_Validation].ToString();
                    string sPlan = "";// System.Configuration.ConfigurationSettings.AppSettings["Primary_Plan"].ToString();
                    if (sE_And_M_CPT_And_ICD != "")
                    {
                        string[] sVal = sE_And_M_CPT_And_ICD.Split('$');
                        if (sVal.Count() > 0)
                        {
                            for (int i = 0; i < sVal.Count(); i++)
                            {
                                CPTICD.Add(sVal[i].Split('|')[0].ToString(), sVal[i].Split('|').Skip(1).ToList());
                            }
                        }
                    }
                    var GcodeList = from g in objMoveVerifyDTO.EandMCodingList where (CPTICD.Any(c => g.Procedure_Code.Contains(c.Key))) select new { ID = g.Id, Procedure_Code = g.Procedure_Code };

                    var GcodeList_New = from g in objMoveVerifyDTO.EandMCodingList where (CPTICD.Any(c => g.Procedure_Code.Contains(c.Key))) select g.Procedure_Code;

                    var Procedure_code = CPTICD.Where(a => GcodeList_New.Any(b => b.ToString() == a.Key)).Select(r => r);
                    //End

                    //var GcodeList = from g in objMoveVerifyDTO.EandMCodingList where (g.Procedure_Code == "G0438" || g.Procedure_Code == "G0439") select g.Id;
                    //if (GcodeList_New.Count()==0)
                    //{
                    //    objMoveVerifyDTO.CPT = CPTICD.Select(p => p.Key).ToList<string>();
                    //}
                    IList<string> ilstICD = new List<string>();
                    if (GcodeList.Count() > 0)
                    {
                        objMoveVerifyDTO.IsGcodePresent = true;
                        for (int i = 0; i < GcodeList.Count(); i++)
                        {
                            //eandmICDList = emICDMngr.EandMcodingList(Convert.ToUInt32(GcodeList.ElementAt(i)));
                            IList<EandMCodingICD> eandmICDList1 = new List<EandMCodingICD>();
                            eandmICDList1 = emICDMngr.EandMcodingList(Convert.ToUInt32(ulMyEncounterID));
                            eandmICDList = (from p in eandmICDList1 where p.Is_Delete == "N" select p).ToList<EandMCodingICD>();
                            if (eandmICDList.Count > 0)
                            {
                                // GcodeCheckList.Add(eandmICDList.Any(a => a.ICD == "V70.0").ToString());
                                //GcodeCheckList.Add(eandmICDList.Any(a => a.ICD == "Z00.00").ToString());
                                //GcodeCheckList.Add(eandmICDList.Any(a => CPTICD.Any(z => z.Value.Contains(a.ICD))).ToString());
                                GcodeCheckList.Add(eandmICDList.Any(a => Procedure_code.Any(z => z.Key == (GcodeList.ElementAt(i).Procedure_Code) && z.Value.Contains(a.ICD))).ToString());
                                if (GcodeCheckList[i].ToString().ToUpper() == "FALSE")
                                {
                                    //var icd = string.Join(" or ", (Procedure_code.ElementAt(i).Value).Select(g => g.ToString()).ToArray());
                                    var test = (from r in Procedure_code
                                                where r.Key == GcodeList.ElementAt(i).Procedure_Code

                                                select r.Value).ToArray();


                                    if (test.Count() > 0)
                                    {
                                        var icd = string.Join(" or ", test[0].Select(g => g.ToString()).ToArray());
                                        ilstICD.Add(icd.ToString() + '$' + GcodeList.ElementAt(i).Procedure_Code);
                                    }
                                }
                            }

                        }

                        objMoveVerifyDTO.IsICDPresent = GcodeCheckList.Contains("False") == true ? false : true;

                        if (Convert.ToBoolean(objMoveVerifyDTO.IsGcodePresent) == true && Convert.ToBoolean(objMoveVerifyDTO.IsICDPresent) == false)
                        {
                            bGCodeCheck = true;
                        }
                        else
                        {
                            bGCodeCheck = false;
                        }
                    }


                    //For Duplicate Check
                    if (Convert.ToBoolean(bDuplicateCheck) == false)
                    {
                        objMoveVerifyDTO.IsDuplicatePresent = false;
                        bGCodeCheck = false;
                    }
                    else
                    {
                        int EandMListCount = objMoveVerifyDTO.EandMCodingList.Count;
                        var EandMDistinctListCount = (from d in objMoveVerifyDTO.EandMCodingList select d.Procedure_Code).Distinct();
                        if (EandMListCount != EandMDistinctListCount.Count())
                        {
                            objMoveVerifyDTO.IsDuplicatePresent = true;
                        }
                    }

                    //Get E & M primary filled
                    // Modified by Manimozhi - 31st May 2012 - Bug id 9842

                    /***  commented for perfomance tuning and added into lineNo 7148
                     * objMoveVerifyDTO.EAndMIsPrimaryFilled  is required only for REVIEW_CODING process
                     *  by Jisha 
                   ***/
                    //Boolean PrimaryCount = emMnger.IsPrimaryFilled(ulMyEncounterID, UserRole);
                    //if (PrimaryCount == true)
                    //{
                    //    objMoveVerifyDTO.EAndMIsPrimaryFilled = true;
                    //}
                    //else
                    //{
                    //    objMoveVerifyDTO.EAndMIsPrimaryFilled = false;
                    //}


                    /***  added the condition for perfomance tuning and added into lineNo 7148
                       * Exception and FeedBack is not required for PROVIDER_PROCESS
                       *  by Jisha 
                     ***/
                    if ((objDocWfObject.Current_Process == "CODER_REVIEW_CORRECTION" || objDocWfObject.Current_Process == "REVIEW_CODING" || objDocWfObject.Current_Process == "REVIEW_CODING_2") && (btnID == "btnPhysiciancorrection"))
                    {
                        CreateExceptionManager objExceptionMnger = new CreateExceptionManager();
                        //Get Exception Created By
                        ICriteria criteria = iMySession.CreateCriteria(typeof(CreateException)).Add(Expression.Eq("Enounter_ID", ulMyEncounterID));
                        IList<CreateException> list = criteria.List<CreateException>();
                        if (list != null && list.Count > 0)
                        {
                            objMoveVerifyDTO.ExceptionCreatedBy = list[0].Created_By;
                            objMoveVerifyDTO.ExceptionCount = list.Count;
                        }
                        else
                        {
                            objMoveVerifyDTO.ExceptionCreatedBy = string.Empty;
                        }
                        //Get FeedBackProvided
                        objMoveVerifyDTO.IsFeedBackProvided = objExceptionMnger.CheckFeedBack(ulMyEncounterID);
                        objMoveVerifyDTO.IsWorkflowPushed = false;
                    }
                    // && condition added by manimozhi on 29th dec 2011

                    //Jira #CAP-707
                    //if ((objDocWfObject.Current_Process == "REVIEW_CODING" || objDocWfObject.Current_Process == "REVIEW_CODING_2") && userCurrentProcess == objDocWfObject.Current_Process)
                    //Jira #CAP-740
                    //if ((objDocWfObject.Current_Process == "REVIEW_CODING" || objDocWfObject.Current_Process == "REVIEW_CODING_2") && userCurrentProcess == objDocWfObject.Current_Process && (btnID == "btnPhysiciancorrection"))
                    if ((objDocWfObject.Current_Process == "REVIEW_CODING" || objDocWfObject.Current_Process == "REVIEW_CODING_2" || objDocWfObject.Current_Process == "AKIDO_REVIEW_CODING") && userCurrentProcess == objDocWfObject.Current_Process && (btnID == "btnPhysiciancorrection" || objDocWfObject.Current_Process == "REVIEW_CODING" || objDocWfObject.Current_Process == "REVIEW_CODING_2" || objDocWfObject.Current_Process == "AKIDO_REVIEW_CODING"))
                    {
                        /***  added for perfomance tuning 
                         * objMoveVerifyDTO.EAndMIsPrimaryFilled  is required only for REVIEW_CODING process
                         *  by Jisha 
                       ***/

                        Boolean PrimaryCount = emMnger.IsPrimaryFilled(ulMyEncounterID, UserRole);
                        if (PrimaryCount == true)
                        {
                            objMoveVerifyDTO.EAndMIsPrimaryFilled = true;
                        }
                        else
                        {
                            objMoveVerifyDTO.EAndMIsPrimaryFilled = false;
                        }

                        if (objMoveVerifyDTO.EAndMIsPrimaryFilled && (objMoveVerifyDTO.ExceptionCount == 0 || (objMoveVerifyDTO.ExceptionCount > 0 && objMoveVerifyDTO.IsFeedBackProvided)) && Convert.ToBoolean(bGCodeCheck) == false && Convert.ToBoolean(objMoveVerifyDTO.IsDuplicatePresent) == false)
                        {
                            if (EncRecord.Is_Physician_Asst_Process == "Y")
                            {
                                string sOwner = string.Empty, sRole = string.Empty;
                                ICriteria criteria1 = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", Convert.ToUInt64(EncRecord.Encounter_Provider_Review_ID))); //selectedPhyID
                                if (criteria1.List<User>() != null && criteria1.List<User>().Count > 0)
                                {
                                    sRole = criteria1.List<User>()[0].role;
                                    sOwner = criteria1.List<User>()[0].user_name;
                                }
                                if (objDocWfObject.Current_Process == "REVIEW_CODING")
                                {
                                    ICriteria criteriaPlan = iMySession.CreateCriteria(typeof(TreatmentPlan)).Add(Expression.Eq("Encounter_Id", EncRecord.Id)).Add(Expression.Eq("Amendment_Type", "Corrections to be Made")); //selectedPhyID
                                    if (EncRecord.Encounter_Provider_Review_ID == 0)
                                    {
                                        objWfMngr.MoveToNextProcess(objDocWfObject.Obj_System_Id, objDocWfObject.Obj_Type, 1, "UNKNOWN", currentDate, MACAddress, null, null);
                                    }
                                    else if (criteriaPlan.List<TreatmentPlan>() != null && criteriaPlan.List<TreatmentPlan>().Count > 0)
                                    {
                                        objWfMngr.MoveToNextProcess(objDocWfObject.Obj_System_Id, objDocWfObject.Obj_Type, 6, sOwner, currentDate, MACAddress, null, null);
                                    }
                                    else
                                    {
                                        objWfMngr.MoveToNextProcess(objDocWfObject.Obj_System_Id, objDocWfObject.Obj_Type, 5, sOwner, currentDate, MACAddress, null, null);
                                    }
                                    objMoveVerifyDTO.IsWorkflowPushed = true;
                                }
                                else
                                {
                                    //int closeType = 3;
                                    //if (objDocWfObject.Current_Process == "REVIEW_CODING_2" && (objMoveVerifyDTO.ExceptionCount > 0 && objMoveVerifyDTO.IsFeedBackProvided))
                                    //{
                                    //    closeType = 4;
                                    //    sOwner = "UNKNOWN";
                                    //}
                                    //objWfMngr.MoveToNextProcess(objDocWfObject.Obj_System_Id, objDocWfObject.Obj_Type, closeType, sOwner, currentDate, MACAddress, null, null);
                                    //objMoveVerifyDTO.IsWorkflowPushed = true;

                                    int closeType = 3;
                                    if (objDocWfObject.Current_Process == "REVIEW_CODING_2" && EncRecord.Encounter_Provider_Review_ID != 0 && (objMoveVerifyDTO.ExceptionCount > 0 && objMoveVerifyDTO.IsFeedBackProvided))
                                    {
                                        closeType = 4;
                                        //sOwner = "UNKNOWN";
                                    }
                                    else if (objDocWfObject.Current_Process == "REVIEW_CODING_2" && EncRecord.Encounter_Provider_Review_ID == 0 && (objMoveVerifyDTO.ExceptionCount > 0 && objMoveVerifyDTO.IsFeedBackProvided))
                                    {
                                        closeType = 1;
                                        sOwner = "UNKNOWN";
                                    }

                                    objWfMngr.MoveToNextProcess(objDocWfObject.Obj_System_Id, objDocWfObject.Obj_Type, closeType, sOwner, currentDate, MACAddress, null, null);
                                    objMoveVerifyDTO.IsWorkflowPushed = true;
                                }
                                objDocWfObject = objWfMngr.GetByObjectSystemId(objDocWfObject.Obj_System_Id, "DOCUMENTATION");
                                if (objDocWfObject.Current_Process == "DOCUMENT_COMPLETE")
                                {
                                    objBillingWfObj = objWfMngr.GetByObjectSystemId(objDocWfObject.Obj_System_Id, "BILLING");
                                    if (objBillingWfObj.Current_Process == "BATCHING_WAIT")
                                        objWfMngr.MoveToNextProcess(objBillingWfObj.Obj_System_Id, objBillingWfObj.Obj_Type, 1, "UNKNOWN", currentDate, MACAddress, null, null);

                                    //Jira CAP-340
                                    EncounterBlobManager objEncblobmngr = new EncounterBlobManager();
                                    objEncblobmngr.LockEncounter(ulMyEncounterID, ulMyHumanID, UserName, currentDate);
                                }

                            }
                            else
                            {
                                //int closeType = 1;
                                ////Commented for this Bug ID 56954 --Start--
                                ////if (objDocWfObject.Current_Process == "REVIEW_CODING_2" && (objMoveVerifyDTO.ExceptionCount > 0 && objMoveVerifyDTO.IsFeedBackProvided))
                                ////    closeType = 4;
                                ////--End--
                                //objWfMngr.MoveToNextProcess(objDocWfObject.Obj_System_Id, objDocWfObject.Obj_Type, closeType, "UNKNOWN", currentDate, MACAddress, null, null);
                                //objMoveVerifyDTO.IsWorkflowPushed = true;

                                int closeType = 1;
                                string currentOwner = "UNKNOWN";
                                if (objDocWfObject.Current_Process == "REVIEW_CODING_2" && EncRecord.Encounter_Provider_Review_ID != 0 && (objMoveVerifyDTO.ExceptionCount > 0 && objMoveVerifyDTO.IsFeedBackProvided))
                                    closeType = 4;
                                if (EncRecord.Encounter_Provider_Review_ID != 0)
                                {
                                    closeType = 5;
                                    ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", Convert.ToUInt64(EncRecord.Encounter_Provider_Review_ID)));
                                    if (criteria.List<User>() != null && criteria.List<User>().Count > 0)
                                    {
                                        currentOwner = criteria.List<User>()[0].user_name;
                                    }
                                }
                                if(objDocWfObject.Current_Process=="AKIDO_REVIEW_CODING")
                                {
                                    EncRecord.Is_EandM_Submitted = "Y";
                                    EncRecord.E_M_Submitted_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                    EncRecord.Modified_By = UserName;
                                    EncRecord.Modified_Date_and_Time= System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                    UpdateEncounter(EncRecord, string.Empty, new object[] { "false" });
                                }
                                objWfMngr.MoveToNextProcess(objDocWfObject.Obj_System_Id, objDocWfObject.Obj_Type, closeType, currentOwner, currentDate, MACAddress, null, null);
                                objMoveVerifyDTO.IsWorkflowPushed = true;
                                objDocWfObject = objWfMngr.GetByObjectSystemId(objDocWfObject.Obj_System_Id, "DOCUMENTATION");
                                if (objDocWfObject.Current_Process == "DOCUMENT_COMPLETE")
                                {
                                    objBillingWfObj = objWfMngr.GetByObjectSystemId(objDocWfObject.Obj_System_Id, "BILLING");
                                    if (objBillingWfObj.Current_Process == "BATCHING_WAIT")
                                        objWfMngr.MoveToNextProcess(objBillingWfObj.Obj_System_Id, objBillingWfObj.Obj_Type, 1, "UNKNOWN", currentDate, MACAddress, null, null);

                                    //Jira CAP-340
                                    EncounterBlobManager objEncblobmngr = new EncounterBlobManager();
                                    objEncblobmngr.LockEncounter(ulMyEncounterID, ulMyHumanID, UserName, currentDate);
                                }
                            }

                        }

                    }
                    else if (objDocWfObject.Current_Process == "CODER_REVIEW_CORRECTION")
                    {
                        SubmitOrdersAndPrescriptions(ulMyEncounterID, selectedPhysicianID, ulMyHumanID, UserName, MACAddress, FacilityName, currentDate);
                        if (objMoveVerifyDTO.ExceptionCreatedBy == string.Empty)
                        {
                            objMoveVerifyDTO.IsWorkflowPushed = false;
                        }
                        else if (objMoveVerifyDTO.ExceptionCount == 0 || (objMoveVerifyDTO.ExceptionCount > 0 && objMoveVerifyDTO.IsFeedBackProvided))
                        {
                            objWfMngr.MoveToNextProcess(objDocWfObject.Obj_System_Id, objDocWfObject.Obj_Type, 1, objMoveVerifyDTO.ExceptionCreatedBy, currentDate, MACAddress, null, null);
                            objMoveVerifyDTO.IsWorkflowPushed = true;
                        }
                    }
                    else if (objDocWfObject.Current_Process == "PROVIDER_REVIEW_CORRECTION")
                    {
                        string sEquivalantOwner = string.Empty;

                        //To get the equivalant allocation process to get the owner
                        if (objDocWfObject.Process_Allocation != string.Empty)
                        {
                            if (objDocWfObject.Process_Allocation.Contains("REVIEW_CODING") == true)
                            {
                                string[] sAlloc = objDocWfObject.Process_Allocation.Split('|');
                                for (int i = 0; i < sAlloc.Length; i++)
                                {
                                    if (sAlloc[i].StartsWith("REVIEW_CODING" + "-") == true|| sAlloc[i].StartsWith("AKIDO_REVIEW_CODING" + "-") == true)
                                    {
                                        string[] sString = sAlloc[i].Split('-');
                                        sEquivalantOwner = sString[1];
                                        break;
                                    }
                                }
                            }
                            else if (objDocWfObject.Process_Allocation.Contains("PROVIDER_REVIEW") == true)
                            {
                                string[] sAlloc = objDocWfObject.Process_Allocation.Split('|');
                                for (int i = 0; i < sAlloc.Length; i++)
                                {
                                    if (sAlloc[i].StartsWith("PROVIDER_REVIEW" + "-") == true)
                                    {
                                        string[] sString = sAlloc[i].Split('-');
                                        sEquivalantOwner = sString[1];
                                        break;
                                    }
                                }
                            }
                        }
                        objWfMngr.MoveToNextProcess(objDocWfObject.Obj_System_Id, objDocWfObject.Obj_Type, 1, sEquivalantOwner, currentDate, MACAddress, null, null);
                        objMoveVerifyDTO.IsWorkflowPushed = true;
                    }
                    else if (objDocWfObject.Current_Process == "PROVIDER_PROCESS")
                    {
                        if (UserRole == "Physician")
                        {
                            HumanManager objHumanManger = new HumanManager();
                            objMoveVerifyDTO.IsACOValid = objHumanManger.IsACOValid(ulMyHumanID);

                            if (objMoveVerifyDTO.EandMCodingList.Count == 0 || (objMoveVerifyDTO.IsACOValid && IsACOValid == "False"))
                            {
                                objMoveVerifyDTO.IsWorkflowPushed = false;
                            }
                            else
                            {
                                MoveToNextProcessFromAfterPlan(EncRecord, ulMyEncounterID, ulMyHumanID, selectedPhysicianID, UserName, btnPhyCorrectionText, FacilityName, string.Empty, Convert.ToDateTime(currentDate), breview, objEncWfObj, objDocWfObject, objDocReviewWfObj, UserRole);
                                objMoveVerifyDTO.IsWorkflowPushed = true;
                            }
                        }
                        else
                        {
                            if (objMoveVerifyDTO.EandMCodingList.Count == 0)
                            {
                                objMoveVerifyDTO.IsWorkflowPushed = false;
                                objMoveVerifyDTO.IsPFSHVerified = EncRecord.Is_PFSH_Verified;
                            }
                            else
                            {
                                MoveToNextProcessFromAfterPlan(EncRecord, ulMyEncounterID, ulMyHumanID, selectedPhysicianID, UserName, btnPhyCorrectionText, FacilityName, string.Empty, Convert.ToDateTime(currentDate), breview, objEncWfObj, objDocWfObject, objDocReviewWfObj, UserRole);
                                objMoveVerifyDTO.IsWorkflowPushed = true;
                            }
                        }
                    }
                }
                //}
                iMySession.Close();
            }
            return objMoveVerifyDTO;
        }


        //Added by Latha on 8-sep-2011 - performance tuning - Start
        #region MoveToNextProcessFromPrintDocuments
        public void MoveToNextProcessFromPrintDocuments(Encounter encounterRecord, ulong ulEncounterID, ulong ulHumanID, ulong ulSelectedPhyID, string sUserName, string sButtonName, string sFacilityName, string sMacAddress, DateTime dtCurrentDateTime, bool bMovetoReview, string UserRole, int CloseType)
        {
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

            WFObject EncounterWfObject = null;
            WFObject DocumentationWfObject = null;
            WFObject BillingWfObject = null;
            WFObjectManager wfObjMngr = new WFObjectManager();
            IList<Encounter> lstencountertemp = new List<Encounter>();
            string sRoleSelectedPhyID = string.Empty;
            string sRoleLoginUser = string.Empty;
            string sOwner = string.Empty;

            #region GetUserRoleAndOwner
            if (ulSelectedPhyID != 0)
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", ulSelectedPhyID));
                if (criteria.List<User>() != null && criteria.List<User>().Count > 0)
                {
                    sRoleSelectedPhyID = criteria.List<User>()[0].role;
                }
            }
            ICriteria crit = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("user_name", sUserName));
            if (crit.List<User>() != null && crit.List<User>().Count > 0)
            {
                sRoleLoginUser = crit.List<User>()[0].role;
            }

            sOwner = sUserName;
            #endregion

            #region GetWfObject
            EncounterWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "ENCOUNTER");
            DocumentationWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "DOCUMENTATION");
            BillingWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "BILLING");
            #endregion

            #region MoveToCheckOut
            if (sButtonName.ToUpper() == "MOVE TO CHECKOUT" || sButtonName.ToUpper() == "MOVE TO NEXT PROCESS")
            {
                if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "MA_PROCESS")
                {
                    encounterRecord.Is_EandM_Submitted = "Y";
                    encounterRecord.E_M_Submitted_Date_And_Time = DateTime.Now;
                    lstencountertemp = UpdateEncounter(encounterRecord, sMacAddress, new object[] { "false" });
                    if (lstencountertemp.Count > 0)
                        encounterRecord = lstencountertemp[0];

                    if (DocumentationWfObject.Current_Process == "PROVIDER_PROCESS_WAIT")
                    {
                        //wfObjMngr.DeleteDocumentationObject(DocumentationWfObject);
                        wfObjMngr.MoveToNextProcess(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, 8, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                    }

                    //Move to check out wait
                    wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);

                    #region For RCM - MA Only Visit
                    // Added By Manimozhi For RCM - MA Only Visit - When MA clicks Move to Checkout Create Documentation Object with Process as REVIEW_CODING.. //
                    //****** START**********//
                    //if (DocumentationWfObject.Obj_System_Id == 0)
                    //{
                    //    WFObject objWfObject = new WFObject();
                    //    objWfObject.Fac_Name = EncounterWfObject.Fac_Name;
                    //    objWfObject.Obj_Type = "DOCUMENTATION";
                    //    objWfObject.Parent_Obj_Type = EncounterWfObject.Obj_Type;
                    //    objWfObject.Parent_Obj_System_Id = ulEncounterID;
                    //    objWfObject.Obj_System_Id = ulEncounterID;
                    //    objWfObject.Current_Process = "START";
                    //    objWfObject.Current_Arrival_Time = dtCurrentDateTime;
                    //    objWfObject.Current_Owner = "UNKNOWN";
                    //TryAgain:
                    //    int iResult = 0;
                    //    ISession MySession = Session.GetISession();
                    //    ITransaction trans = null;
                    //    IList<Encounter> EncList = new List<Encounter>();
                    //    try
                    //    {
                    //        trans = MySession.BeginTransaction();
                    //        if (objWfObject != null)
                    //        {
                    //            iResult = wfObjMngr.InsertToWorkFlowObject(objWfObject, 2, sMacAddress, MySession);

                    //            if (iResult == 2)
                    //            {
                    //                if (iTryCount < 5)
                    //                {
                    //                    iTryCount++;
                    //                    goto TryAgain;
                    //                }
                    //                else
                    //                {
                    //                    trans.Rollback();
                    //                    // MySession.Close();
                    //                    throw new Exception("Deadlock occurred. Transaction failed.");
                    //                }
                    //            }
                    //            else if (iResult == 1)
                    //            {
                    //                trans.Rollback();
                    //                // MySession.Close();
                    //                throw new Exception("Exception occurred. Transaction failed.");
                    //            }
                    //        }


                    //        MySession.Flush();
                    //        trans.Commit();
                    //    }
                    //    catch (NHibernate.Exceptions.GenericADOException ex)
                    //    {
                    //        trans.Rollback();
                    //        // MySession.Close();
                    //        throw new Exception(ex.Message);
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        trans.Rollback();
                    //        //MySession.Close();
                    //        throw new Exception(e.Message);
                    //    }
                    //}
                    //****** END**********//
                    #endregion
                    OrdersManager ordMnger = new OrdersManager();
                    //comment by bala
                    //ordMnger.SubmitOrdersToLab(ulHumanID, ulEncounterID, "DIAGNOSTIC ORDER", sMacAddress, sUserName, dtCurrentDateTime, encounterRecord.Assigned_Med_Asst_User_Name);
                    ordMnger.SubmitLogic(ulHumanID, ulEncounterID, ulSelectedPhyID, "DIAGNOSTIC ORDER", sUserName, dtCurrentDateTime, sFacilityName);
                    ordMnger.SubmitOrdersToLab(ulHumanID, ulEncounterID, "IMAGE ORDER", sMacAddress, sUserName, dtCurrentDateTime, encounterRecord.Assigned_Med_Asst_User_Name);
                }
                else if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT" && (encounterRecord.Assigned_Scribe_User_Name == string.Empty || sButtonName.ToUpper() == "MOVE TO CHECKOUT"))
                {
                    //Move to check out
                    wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);

                    // Move to Checkout from physician GITLAB #3084
                    if (UserRole.ToUpper() != "MEDICAL ASSISTANT" && encounterRecord.Is_EandM_Submitted != "Y")
                        encounterRecord.Is_EandM_Submitted = "N";
                    else
                        encounterRecord.Is_EandM_Submitted = "Y";

                    encounterRecord.E_M_Submitted_Date_And_Time = DateTime.Now;
                    //Added by Janani to update Follow up details in encounter
                    lstencountertemp = UpdateEncounter(encounterRecord, sMacAddress, new object[] { "false" });
                    if (lstencountertemp.Count > 0)
                        encounterRecord = lstencountertemp[0];

                    if (DocumentationWfObject.Current_Process == "MA_PROCESS")
                    {
                        wfObjMngr.MoveToNextProcess(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, 2, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                        WFObject WFDocObj = wfObjMngr.GetByObjectSystemId(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type);
                        if (BillingWfObject.Current_Process == "BATCHING_WAIT")
                            wfObjMngr.MoveToNextProcess(BillingWfObject.Obj_System_Id, BillingWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);

                        //Jira CAP-340
                        if (WFDocObj.Current_Process == "DOCUMENT_COMPLETE")
                        {
                            EncounterBlobManager objEncblobmngr = new EncounterBlobManager();
                            objEncblobmngr.LockEncounter(ulEncounterID, ulHumanID, sUserName, dtCurrentDateTime);
                        }
                    }
                }
            }
            #endregion

            #region MoveToNextProcess from PhysicianAssistant
            if (sButtonName.ToUpper() == "MOVE TO NEXT PROCESS" && UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
            {
                if (DocumentationWfObject != null && DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_PROCESS")
                {
                    if (encounterRecord != null)
                    {
                        lstencountertemp = UpdateEncounter(encounterRecord, sMacAddress, new object[] { "false" });
                        if (lstencountertemp.Count > 0)
                            encounterRecord = lstencountertemp[0];
                        string sCurrentOwner = string.Empty;
                        ICriteria crite = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", ulSelectedPhyID));
                        if (crite.List<User>() != null && crite.List<User>().Count > 0)
                        {
                            sCurrentOwner = crite.List<User>()[0].user_name;
                        }
                        SubmitOrdersAndPrescriptions(ulEncounterID, ulHumanID, Convert.ToUInt64(encounterRecord.Encounter_Provider_ID), sUserName, sMacAddress, sFacilityName, dtCurrentDateTime);
                        /* if (bMovetoReview)
                         {
                             WFObject ehrNewWFObj = new WFObject();
                             ehrNewWFObj.Fac_Name = EncounterWfObject.Fac_Name;
                             ehrNewWFObj.Obj_Type = "DOCUMENT REVIEW";
                             ehrNewWFObj.Parent_Obj_Type = EncounterWfObject.Obj_Type;
                             ehrNewWFObj.Parent_Obj_System_Id = ulEncounterID;
                             ehrNewWFObj.Obj_System_Id = ulEncounterID;
                             ehrNewWFObj.Current_Process = "START";
                             ehrNewWFObj.Current_Arrival_Time = dtCurrentDateTime;
                             ehrNewWFObj.Current_Owner = sCurrentOwner;
                             wfObjMngr.InsertToRCopiaWorkFlowObject(ehrNewWFObj, 1, sMacAddress);

                             if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT")
                             {
                                 wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                             }
                         }
                         // Added on 26th dec 2011 by manimozhi 
                         // Directly move from check_out_wait to checkout in PA login.
                         else
                         {
                             if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT")
                             {
                                 wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                             }
                         }
                         wfObjMngr.MoveToNextProcess(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                        */
                        //Changed for Provider_Review PhysicianAssistant WorkFlow Change.
                        if (bMovetoReview)
                        {
                            wfObjMngr.MoveToNextProcess(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, 5, sCurrentOwner, dtCurrentDateTime, sMacAddress, null, null);
                        }
                        else
                        {
                            wfObjMngr.MoveToNextProcess(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                        }
                        if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT")
                        {
                            wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                        }
                    }
                }
            }
            #endregion

            #region MoveToNextProcess from Provider
            if ((sButtonName.ToUpper() == "MOVE TO NEXT PROCESS" || sButtonName.ToUpper() == "MOVE TO PHYSICIAN ASSISTANT") && (UserRole.ToUpper() == "PHYSICIAN"))
            {
                //Selvaraman - 5-Apr-12
                //For Wellness - the documentwfobject will be in provider_review
                if (DocumentationWfObject != null && (DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_PROCESS" || DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW" || DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW_2"))
                {
                    if (DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_PROCESS" && CloseType != 6)
                    {
                        CloseType = 1;
                    }
                    if (encounterRecord != null)
                    {
                        //Provider review Move to next process  GITLAB #3084
                        if (sButtonName.ToUpper() != "MOVE TO PHYSICIAN ASSISTANT")
                            encounterRecord.Is_EandM_Submitted = "Y";

                        if (DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW_2" && DocumentationWfObject.Process_Allocation.IndexOf("REVIEW_CODING") > -1)
                        {
                            if (sButtonName.ToUpper() == "MOVE TO PHYSICIAN ASSISTANT")
                                CloseType = 1;
                            else
                                CloseType = 3;
                        }
                        //Commented as not required
                        /* if ((DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW" || DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW_2") && CloseType == 2)//set e_m_submitted to "Y" when moved to REVIEW_CODING process.
                             encounterRecord.Is_EandM_Submitted = "Y"; */
                        lstencountertemp = UpdateEncounter(encounterRecord, sMacAddress, new object[] { "false" });
                        if (lstencountertemp.Count > 0)
                            encounterRecord = lstencountertemp[0];
                        SubmitOrdersAndPrescriptions(ulEncounterID, ulHumanID, Convert.ToUInt64(encounterRecord.Encounter_Provider_ID), sUserName, sMacAddress, sFacilityName, dtCurrentDateTime);
                        if (DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_PROCESS" && encounterRecord.Assigned_Scribe_User_Name != string.Empty && CloseType == 6)
                        {
                            wfObjMngr.MoveToNextProcess(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, CloseType, encounterRecord.Assigned_Scribe_User_Name, dtCurrentDateTime, sMacAddress, null, null);

                        }
                        else
                        {
                            wfObjMngr.MoveToNextProcess(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, CloseType, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);

                            if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT")
                            {
                                wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                            }

                            WFObject WFDocObj = wfObjMngr.GetByObjectSystemId(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type);
                            if (WFDocObj.Current_Process == "DOCUMENT_COMPLETE")
                            {
                                BillingWfObject = wfObjMngr.GetByObjectSystemId(DocumentationWfObject.Obj_System_Id, "BILLING");
                                if (BillingWfObject.Current_Process == "BATCHING_WAIT")
                                    wfObjMngr.MoveToNextProcess(BillingWfObject.Obj_System_Id, BillingWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);

                                //Jira CAP-340
                                EncounterBlobManager objEncblobmngr = new EncounterBlobManager();
                                objEncblobmngr.LockEncounter(ulEncounterID, ulHumanID, sUserName, dtCurrentDateTime);
                            }
                        }
                    }
                }

                /*else if (DocumentReviewWfObject != null && DocumentReviewWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW")
                {
                    //if (sRoleLoginUser.ToUpper() == "PHYSICIAN")
                    //{
                    wfObjMngr.MoveToNextProcess(DocumentReviewWfObject.Obj_System_Id, DocumentReviewWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                    if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT")
                    {
                        wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                    }
                    //}
                }*/
            }
            // iMySession.Close();
            #endregion

            #region MoveToNextProcess from Provider
            if (sButtonName.ToUpper() == "MOVE TO PHYSICIAN ASSISTANT" && (UserRole.ToUpper() == "PHYSICIAN ASSISTANT"))
            {
                //Selvaraman - 5-Apr-12
                //For Wellness - the documentwfobject will be in provider_review
                if (DocumentationWfObject != null && (DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_PROCESS" || DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW" || DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW_2"))
                {
                    if (DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_PROCESS" && CloseType != 6)
                    {
                        CloseType = 1;
                    }
                    if (encounterRecord != null)
                    {
                        if (DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW_2" && DocumentationWfObject.Process_Allocation.IndexOf("REVIEW_CODING") > -1)
                        {
                            if (sButtonName.ToUpper() == "MOVE TO PHYSICIAN ASSISTANT")
                                CloseType = 1;
                            else
                                CloseType = 3;
                        }
                        //Commented as not required
                        /* if ((DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW" || DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW_2") && CloseType == 2)//set e_m_submitted to "Y" when moved to REVIEW_CODING process.
                             encounterRecord.Is_EandM_Submitted = "Y"; */
                        lstencountertemp = UpdateEncounter(encounterRecord, sMacAddress, new object[] { "false" });
                        if (lstencountertemp.Count > 0)
                            encounterRecord = lstencountertemp[0];
                        SubmitOrdersAndPrescriptions(ulEncounterID, ulHumanID, Convert.ToUInt64(encounterRecord.Encounter_Provider_ID), sUserName, sMacAddress, sFacilityName, dtCurrentDateTime);
                        if (DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_PROCESS" && encounterRecord.Assigned_Scribe_User_Name != string.Empty && CloseType == 6)
                        {
                            wfObjMngr.MoveToNextProcess(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, CloseType, encounterRecord.Assigned_Scribe_User_Name, dtCurrentDateTime, sMacAddress, null, null);

                        }
                        else
                        {
                            wfObjMngr.MoveToNextProcess(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, CloseType, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);

                            if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT")
                            {
                                wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                            }
                        }
                    }
                }

                /*else if (DocumentReviewWfObject != null && DocumentReviewWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW")
                {
                    //if (sRoleLoginUser.ToUpper() == "PHYSICIAN")
                    //{
                    wfObjMngr.MoveToNextProcess(DocumentReviewWfObject.Obj_System_Id, DocumentReviewWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                    if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT")
                    {
                        wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                    }
                    //}
                }*/
            }
            iMySession.Close();
            #endregion

        }
        #endregion
        //Added by Latha on 8-sep-2011 - performance tuning - End

        //Added by Suresh on 12-sep-2011 - Coders Production Report
        public DataSet DtblCodersReport(DateTime From_date, DateTime To_Date, string sFacilityName)
        {

            DataSet CodersReport = new DataSet();
            string[] Split = sFacilityName.Split(',');
            ArrayList arylstColumnsOfCodersReport;
            ArrayList arylstCodersReport;
            DataTable dtblCodersReport = new DataTable();
            //string Fromdate = "2011-02-17";
            // string ToDate = "2011-02-20";
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.GetCodersReport.ColumnHeadings");
                arylstColumnsOfCodersReport = new ArrayList(query2.List());
                if (arylstColumnsOfCodersReport != null)
                {
                    if (arylstColumnsOfCodersReport.Count > 0)
                    {
                        object[] objlstColumns = (object[])arylstColumnsOfCodersReport[0];

                        for (int i = 0; i < objlstColumns.Length; i++)
                        {
                            if (i == 3)
                            {
                                dtblCodersReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                            }
                            else
                            {

                                dtblCodersReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                            }
                        }
                    }
                }

                IQuery query1 = iMySession.GetNamedQuery("Reports.CodersReport");
                //query1.SetString(0, Fromdate);
                // query1.SetString(2, sFacilityName);

                query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                query1.SetParameterList("Faclist", Split.ToList());
                arylstCodersReport = new ArrayList(query1.List());


                //Added for BugID:36931 
                IQuery query = iMySession.GetNamedQuery("Reports.CodersReportArchive");
                //query1.SetString(0, Fromdate);
                // query1.SetString(2, sFacilityName);

                query.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                query.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                query.SetParameterList("Faclist", Split.ToList());
                arylstCodersReport.AddRange(query.List());
                //End


                if (arylstCodersReport != null)
                {
                    for (int h = 0; h < arylstCodersReport.Count; h++)
                    {
                        object[] objlst = (object[])arylstCodersReport[h];
                        DataRow dr = dtblCodersReport.NewRow();
                        if (objlst.Length > 0)
                        {
                            for (int k = 0; k < objlst.Length; k++)
                            {

                                try
                                {
                                    if (objlst[k].ToString().Split(',').Length > 0)
                                    {
                                        if (objlst[k].ToString().Split(',')[0] == string.Empty && objlst[k].ToString().Split(',')[1] == string.Empty && objlst[k].ToString().Split(',')[2] == string.Empty && objlst[k].ToString().Split(',')[3] == string.Empty)
                                            objlst[k] = string.Empty;
                                        else if (objlst[k].ToString().Split(',')[0] == string.Empty)
                                            objlst[k] = objlst[k].ToString().Split(',')[1];
                                    }
                                    dr[k] = objlst[k].ToString();
                                }
                                catch
                                {
                                    // string sMessage;
                                }
                            }
                            dtblCodersReport.Rows.Add(dr);
                        }
                    }
                }


                CodersReport.Tables.Add(dtblCodersReport);
                iMySession.Close();
            }
            return CodersReport;
        }
        //pravin 11-01-2012
        public CheckOutDTO CheckOutLoad(ulong humanID, ulong encounterID, string MACAddress, string UserName)
        {
            CheckOutDTO checkOutDtoObj = new CheckOutDTO();
            EncounterManager objEncMngr = new EncounterManager();
            OrdersManager objOrdManager = new OrdersManager();

            // checkOutDtoObj.PatientPaneList = objEncMngr.FillPatientPane(humanID, MACAddress, UserName);
            DocumentManager objDocMngr = new DocumentManager();
            //checkOutDtoObj.DocumentList = objDocMngr.GetDocumentsList(encounterID); //for bugid 37282
            //checkOutDtoObj.objhuman = objOrdManager.GetHumanById(humanID); //for bugid 37281 

            checkOutDtoObj.DocumentList = objDocMngr.GetDocumentsListForCheckout(encounterID);
            //checkOutDtoObj.objhuman = objOrdManager.GetHumanByIdForCheckout(humanID);//for bugid 37280

            checkOutDtoObj.CheckoutOrdersList = objOrdManager.GetOrdersByEncounterID(encounterID);

            return checkOutDtoObj;

        }
        public void BatchOperationsOnCheckOut(IList<Documents> SaveDocuList, IList<Documents> UpdateDocuList, IList<Documents> DeleteDocuList, Encounter objEncounter, ulong HumanID, ulong EncounterID, string sMacAddress, string UserName, DateTime currentDate, bool Is_FormClosed)
        {
            WFObject objWF = new WFObject();
            WFObjectManager TempWF = new WFObjectManager();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            GenerateXml XMLObj = new GenerateXml();
            if (Is_FormClosed == true)
            {
                objWF = TempWF.GetByObjectSystemId(EncounterID, "ENCOUNTER");
            }
            else
            {
                objWF = null;
            }
            int iTryCount = 0;
        TryAgain:
            int iResult = 0;

            ISession MySession = Session.GetISession();
            //ITransaction trans = null;
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        // trans = MySession.BeginTransaction();
                        if (SaveDocuList != null && SaveDocuList.Count > 0 || UpdateDocuList != null && UpdateDocuList.Count > 0 || DeleteDocuList != null && DeleteDocuList.Count > 0)
                        {
                            DocumentManager dcmntMngr = new DocumentManager();
                            iResult = dcmntMngr.BatchOperationsToDocuments(SaveDocuList, UpdateDocuList, DeleteDocuList, MySession, sMacAddress);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {
                                    trans.Rollback();
                                    // MySession.Close();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                //MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                        }
                        IList<Encounter> saveList = new List<Encounter>();
                        IList<Encounter> encList = new List<Encounter>();
                        if (objEncounter != null)
                        {
                            //IList<Encounter> saveList = new List<Encounter>();
                            //IList<Encounter> encList = new List<Encounter>();
                            encList.Add(objEncounter);

                            // iResult = SaveUpdateDeleteWithoutTransaction(ref saveList, encList, null, MySession, sMacAddress);
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref saveList, ref encList, null, MySession, sMacAddress, true, false, EncounterID, string.Empty, ref XMLObj);

                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {
                                    trans.Rollback();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                        }
                        #region Comment
                        //for bugid 27286
                        if (objWF != null && objWF.Obj_System_Id != 0)
                        {

                            //IList<AuthorizationEncounter> AuthEncList = new List<AuthorizationEncounter>();
                            //ICriteria criteria;

                            //criteria = iMySession.CreateCriteria(typeof(AuthorizationEncounter)).Add(Expression.Eq("Encounter_ID", EncounterID));
                            //AuthEncList = criteria.List<AuthorizationEncounter>();

                            //if (AuthEncList != null && AuthEncList.Count > 0)
                            //{
                            //    ulong Authorization_ID = AuthEncList[0].Authorization_ID;

                            //    IList<Authorization> AuthList = new List<Authorization>();
                            //    ICriteria icriteria;

                            //    icriteria = iMySession.CreateCriteria(typeof(Authorization)).Add(Expression.Eq("Id", Authorization_ID));
                            //    AuthList = icriteria.List<Authorization>();
                            //    if (AuthList != null && AuthList.Count > 0)
                            //    {
                            //        IList<Authorization> newauthlst = null;
                            //        //Authorization auth = new Authorization();
                            //        //auth.Id = AuthList[0].Id;
                            //        //auth.Number_Of_Visits_Used_Temp = AuthList[0].Number_Of_Visits_Used_Temp + 1;
                            //        //auth.Version = AuthList[0].Version;
                            //        //AuthList.Add(auth);
                            //        AuthList[0].Number_Of_Visits_Used_Temp = AuthList[0].Number_Of_Visits_Used_Temp + 1;
                            //        AuthorizationManager authmngr = new AuthorizationManager();
                            //        //authmngr.UpdateIsActive(AuthList, sMacAddress);
                            //        iResult = authmngr.SaveUpdateDeleteWithoutTransaction(ref newauthlst, AuthList, null, MySession, sMacAddress);
                            //        if (iResult == 2)
                            //        {
                            //            if (iTryCount < 5)
                            //            {
                            //                iTryCount++;
                            //                goto TryAgain;
                            //            }
                            //            else
                            //            {
                            //                trans.Rollback();
                            //                throw new Exception("Deadlock occurred. Transaction failed.");
                            //            }
                            //        }
                            //        else if (iResult == 1)
                            //        {
                            //            trans.Rollback();
                            //            throw new Exception("Exception occurred. Transaction failed.");
                            //        }
                            //    }

                            //}
                            iResult = TempWF.MoveToNextProcess(objWF.Obj_System_Id, objWF.Obj_Type, 1, UserName, currentDate, sMacAddress, null, MySession);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {
                                    trans.Rollback();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                        }
                        #endregion

                        iMySession.Close();
                        MySession.Flush();
                        //trans.Commit();
                        if (XMLObj.itemDoc.InnerXml != null && XMLObj.itemDoc.InnerXml != "")
                        {
                            try
                            {
                                #region "code Modified by Balaji.TJ 2023-01-08"
                                WriteBlob(EncounterID, XMLObj.itemDoc, MySession, saveList, encList, null, XMLObj, false);
                                #endregion
                            }
                            catch (Exception xmlexcep)
                            {

                            }
                        }
                        trans.Commit();
                        //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);


                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        MySession.Close();
                    }
                }
            }

            catch (Exception ex1)
            {
                //MySession.Close();
                throw new Exception(ex1.Message);
            }


        }


        //Added by Suresh on 12-sep-2011 - Coders Production Report


        #region IEncounterManager Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SaveEncounter"></param>
        /// <param name="SavePlan"></param>
        /// <param name="strHumanDetails"></param>
        /// <param name="sMacAddress"></param>
        /// changed the method as similar to acs for integrum ptoject.to reate xml for phone encounter
        public void SaveUpdatePhoneEncounter(IList<Encounter> SaveEncounter, IList<TreatmentPlan> SavePlan, string strHumanDetails, string sMacAddress)
        {
            TreatmentPlanManager objTreatmentMgr = null;
            IList<Encounter> EncListUpdate = null;
            bool bEncounter = true;
            GenerateXml XMLObj = new GenerateXml();
            int iTryCount = 0;
        TryAgain:
            int iResult = 0;
            ulong ulEncId = 0;

            ISession MySession = Session.GetISession();
            ITransaction trans = null;
            try
            {
                trans = MySession.BeginTransaction();
                if (SaveEncounter != null && SaveEncounter.Count > 0)
                {
                    //iResult = SaveUpdateDeleteWithoutTransaction(ref SaveEncounter, null, null, MySession, sMacAddress);
                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveEncounter, ref EncListUpdate, null, MySession, sMacAddress, false, false, SaveEncounter[0].Id, string.Empty, ref XMLObj);

                    if (iResult == 2)
                    {
                        if (iTryCount < 5)
                        {
                            iTryCount++;
                            goto TryAgain;
                        }
                        else
                        {
                            trans.Rollback();
                            // MySession.Close();
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                    ulEncId = SaveEncounter[0].Id;
                    GenerateXml objXml = new GenerateXml();
                    EncounterBlobManager blobmanager = new EncounterBlobManager();
                    IList<Encounter_Blob> obj = new List<Encounter_Blob>();
                    obj = blobmanager.GetEncounterBlob(ulEncId);

                    //Crerate Encounter xml when save Phone Encounter 
                    // string EncounterFileName = "Encounter" + "_" + ulEncId + ".xml";
                    // string strXmlEncounterFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], EncounterFileName);
                    // if (File.Exists(strXmlEncounterFilePath) == false)
                    if (obj.Count == 0)
                    {
                        string sDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("Template_XML");
                        string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                        XmlDocument itemDoc = new XmlDocument();
                        XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                        objXml.itemDoc.Load(XmlText);
                        XmlText.Close();

                        XmlNodeList xmlAgenode = itemDoc.GetElementsByTagName("Age");
                        if (xmlAgenode != null)
                            xmlAgenode[0].ParentNode.RemoveChild(xmlAgenode[0]);
                        // itemDoc.Save(strXmlEncounterFilePath);
                    }
                    List<object> lstObj = SaveEncounter.Cast<object>().ToList();
                    // objXml.GenerateXmlSave(lstObj, ulEncId, string.Empty, true);
                    // objXml.itemDoc = null;
                    objXml.GenerateXmlSave(lstObj, ulEncId, string.Empty, false, true, false, false, ref objXml);
                    WriteBlob(ulEncId, objXml.itemDoc, MySession, SaveEncounter, null, null, objXml, true);
                    trans.Commit();
                    trans = MySession.BeginTransaction();
                    // List<object> lstObj = SaveEncounter.Cast<object>().ToList();
                    //objXml.GenerateXmlSave(lstObj, ulEncId, string.Empty, true);
                    //objXml.itemDoc = null;
                    //  objXml.GenerateXmlSave(lstObj, ulEncId, string.Empty, false, true, false, false,ref objXml);
                }

                if (SavePlan != null && SavePlan.Count > 0)
                {
                    if (SaveEncounter != null && SaveEncounter.Count > 0)
                    {
                        SavePlan[0].Encounter_Id = SaveEncounter[0].Id;
                    }


                    //if (updateList.Count > 0)
                    //{

                    //    encounterid = updateList[0].Encounter_ID;
                    //    foreach (var obj in updateList)
                    //        obj.Version += 1;
                    //    foreach (ChiefComplaints obj in updateList)
                    //        filllst.Add(obj);

                    //}

                    objTreatmentMgr = new TreatmentPlanManager();
                    bool isPhone_Encounter = true;
                    iResult = objTreatmentMgr.BatchOperationsToTreatmentPlan(SavePlan, null, null, MySession, sMacAddress, isPhone_Encounter);

                    if (iResult == 2)
                    {
                        if (iTryCount < 5)
                        {
                            iTryCount++;
                            goto TryAgain;
                        }
                        else
                        {
                            trans.Rollback();
                            //  MySession.Close();
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                MySession.Flush();
                trans.Commit();

            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                trans.Rollback();
                // MySession.Close();
                throw new Exception(ex.Message);
            }
            catch (Exception e)
            {
                trans.Rollback();
                //MySession.Close();
                throw new Exception(e.Message);
            }
            finally
            {
                MySession.Close();
            }

            //GenerateXml XMLObj = new GenerateXml();
            //ulong encounterid = 0;

            //string FileName = "Encounter" + "_" + ulEncId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

            //if (SaveEncounter != null && SaveEncounter.Count > 0)
            //{
            //    if (File.Exists(strXmlFilePath) == false)
            //    {
            //        string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
            //        string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");

            //        XmlDocument itemDoc = new XmlDocument();
            //        XmlTextReader XmlText = new XmlTextReader(sXmlPath);
            //        itemDoc.Load(XmlText);
            //        XmlText.Close();
            //        itemDoc.Save(strXmlFilePath);
            //    }

            //    encounterid = SaveEncounter[0].Id;
            //    List<object> lstObj = SaveEncounter.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //}


            //if (SavePlan.Count > 0)
            //{
            //    List<object> lstObjTrtPlan = new List<object>();
            //    lstObjTrtPlan = SavePlan.Cast<object>().ToList();
            //    encounterid = SavePlan[0].Encounter_Id;
            //    XMLObj.GenerateXmlSave(lstObjTrtPlan, encounterid, string.Empty);
            //}

            //Stream stream = new MemoryStream();
            //var stream = new MemoryStream();
            //var serializer = new NetDataContractSerializer();
            //Hashtable htStream = new Hashtable();
            //ICriteria crt = session.GetISession().CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id",ulEncounterID));
            //if (crt.List<Encounter>().Count > 0)
            //{
            //    htStream.Add("ENCOUNTER", crt.List<Encounter>());
            //}
            //objTreatmentMgr = new TreatmentPlanManager();
            //IList<TreatmentPlan> TreatmentLst = objTreatmentMgr.GetTreatmentPlanUsingEncounterId(ulEncounterID);
            //if (TreatmentLst != null && TreatmentLst.Count > 0)
            //{
            //    htStream.Add("PLAN", TreatmentLst);
            //}
            //serializer.WriteObject(stream, htStream);
            //stream.Seek(0L, SeekOrigin.Begin);
            //return stream;
        }
        public ulong SaveUpdatePhoneEncounter(IList<Encounter> SaveEncounter, IList<TreatmentPlan> SavePlan, string sMacAddress)
        {
            TreatmentPlanManager objTreatmentMgr = null;
            IList<Encounter> EncListUpdate = null;
            GenerateXml XMLObj = new GenerateXml();
            int iTryCount = 0;
        TryAgain:
            int iResult = 0;
            ulong ulEncId = 0;

            ISession MySession = Session.GetISession();
            ITransaction trans = null;
            try
            {
                trans = MySession.BeginTransaction();
                if (SaveEncounter != null && SaveEncounter.Count > 0)
                {
                    //iResult = SaveUpdateDeleteWithoutTransaction(ref SaveEncounter, null, null, MySession, sMacAddress);
                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveEncounter, ref EncListUpdate, null, MySession, sMacAddress, false, false, SaveEncounter[0].Id, string.Empty, ref XMLObj);

                    if (iResult == 2)
                    {
                        if (iTryCount < 5)
                        {
                            iTryCount++;
                            goto TryAgain;
                        }
                        else
                        {
                            trans.Rollback();
                            // MySession.Close();
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                    ulEncId = SaveEncounter[0].Id;
                    //Crerate Encounter xml when save Phone Encounter 

                    EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
                    //IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(ulEncId);
                    //if (ilstEncounterBlob.Count < 0)
                    //{ 
                    //    string EncounterFileName = "Encounter" + "_" + ulEncId + ".xml";
                    //string strXmlEncounterFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], EncounterFileName);
                    //if (File.Exists(strXmlEncounterFilePath) == false)
                    //{
                    // string sDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("Template_XML");
                    //string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                    string sXmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template_XML\\Base_XML.xml");
                    XmlDocument itemDoc = new XmlDocument();
                    XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                    XMLObj.itemDoc.Load(XmlText);
                    XmlText.Close();

                    XmlNodeList xmlAgenode = XMLObj.itemDoc.GetElementsByTagName("Age");
                    if (xmlAgenode != null)
                        xmlAgenode[0].ParentNode.RemoveChild(xmlAgenode[0]);

                    IEnumerable<XElement> ilstPhysician = null;
                    XmlNodeList xmlMember_ID = XMLObj.itemDoc.GetElementsByTagName("Encounter_Provider_Name");

                    string sPhysicianFacilityXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\PhysicianFacilityMapping.xml";
                    XDocument xmlPhysician = XDocument.Load(sPhysicianFacilityXmlPath);
                    ilstPhysician = xmlPhysician.Element("ROOT").Element("PhyList").Elements("Facility").Elements("Physician").Where(aa => aa.Attribute("ID").Value.ToString() == SaveEncounter[0].Encounter_Provider_ID.ToString());
                    if (ilstPhysician != null && ilstPhysician.Count() > 0)
                    {
                        xmlMember_ID[0].InnerText = ilstPhysician.Attributes("prefix").First().Value.ToString() + " " + ilstPhysician.Attributes("firstname").First().Value.ToString() + " " + ilstPhysician.Attributes("middlename").First().Value.ToString() + " " + ilstPhysician.Attributes("lastname").First().Value.ToString();
                    }

                    string sPhysicianid = SaveEncounter[0].Encounter_Provider_ID.ToString();
                    XmlNodeList xmlPhysicianAddress = XMLObj.itemDoc.GetElementsByTagName("Physician_Address");
                    if (xmlPhysicianAddress != null)
                    {
                        string sPhysicianXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\PhysicianAddressDetails.xml";
                        XmlDocument itemPhysiciandoc = new XmlDocument();
                        XmlTextReader XmlPhysicianText = new XmlTextReader(sPhysicianXmlPath);
                        itemPhysiciandoc.Load(XmlPhysicianText);

                        XmlNodeList xmlphy = itemPhysiciandoc.GetElementsByTagName("p" + sPhysicianid);
                        if (xmlphy.Count > 0)
                        {
                            xmlPhysicianAddress[0].Attributes[0].Value = xmlphy[0].Attributes[0].Value;
                            xmlPhysicianAddress[0].Attributes[1].Value = xmlphy[0].Attributes[1].Value;
                            xmlPhysicianAddress[0].Attributes[2].Value = xmlphy[0].Attributes[2].Value;
                            xmlPhysicianAddress[0].Attributes[3].Value = xmlphy[0].Attributes[3].Value;
                            xmlPhysicianAddress[0].Attributes[4].Value = xmlphy[0].Attributes[4].Value;
                            xmlPhysicianAddress[0].Attributes[5].Value = xmlphy[0].Attributes[5].Value;
                            xmlPhysicianAddress[0].Attributes[6].Value = xmlphy[0].Attributes[6].Value;
                            xmlPhysicianAddress[0].Attributes[7].Value = xmlphy[0].Attributes[7].Value;
                        }
                    }

                    //itemDoc.Save(strXmlEncounterFilePath);


                    GenerateXml objXml = new GenerateXml();
                    List<object> lstObj = SaveEncounter.Cast<object>().ToList();
                    // objXml.GenerateXmlSave(lstObj, ulEncId, string.Empty, true);
                    // objXml.itemDoc = null;
                    XMLObj.GenerateXmlSave(lstObj, ulEncId, string.Empty, false, true, false, false, ref XMLObj);
                    WriteBlob(ulEncId, XMLObj.itemDoc, MySession, SaveEncounter, null, null, XMLObj, true);
                    trans.Commit();
                    trans = MySession.BeginTransaction();


                }

                if (SavePlan != null && SavePlan.Count > 0)
                {
                    if (SaveEncounter != null && SaveEncounter.Count > 0)
                    {
                        SavePlan[0].Encounter_Id = SaveEncounter[0].Id;
                    }

                    objTreatmentMgr = new TreatmentPlanManager();
                    bool isPhone_Encounter = true;
                    iResult = objTreatmentMgr.BatchOperationsToTreatmentPlan(SavePlan, null, null, MySession, sMacAddress, isPhone_Encounter);

                    if (iResult == 2)
                    {
                        if (iTryCount < 5)
                        {
                            iTryCount++;
                            goto TryAgain;
                        }
                        else
                        {
                            trans.Rollback();
                            //  MySession.Close();
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                MySession.Flush();
                trans.Commit();

            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                trans.Rollback();
                // MySession.Close();
                throw new Exception(ex.Message);
            }
            catch (Exception e)
            {
                trans.Rollback();
                //MySession.Close();
                throw new Exception(e.Message);
            }
            finally
            {
                MySession.Close();
            }
            return ulEncId;
        }

        #endregion
        // Added By Manimozhi - Performance Tuning - Start
        public FillNewEditAppointment GetEncounterAndHumanRecord(ulong EncID, ulong HumanID)
        {
            FillNewEditAppointment fillneweditappt = new FillNewEditAppointment();

            IList<Human> objHuman = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteriaByHumanId = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanID));
                objHuman = criteriaByHumanId.List<Human>();

                if (objHuman.Count > 0)
                {
                    foreach (Human obj in objHuman)
                    {
                        fillneweditappt.Human_ID = obj.Id;
                        fillneweditappt.First_Name = obj.First_Name;
                        fillneweditappt.Last_Name = obj.Last_Name;
                        fillneweditappt.Home_Phone_No = obj.Home_Phone_No;
                        fillneweditappt.Cell_Phone_No = obj.Cell_Phone_Number;
                        fillneweditappt.Birth_Date = obj.Birth_Date;
                        fillneweditappt.MI = obj.MI;
                        fillneweditappt.Suffix = obj.Suffix;
                        fillneweditappt.HumanType = obj.Human_Type;
                        fillneweditappt.Encounter_Provider_ID = obj.Encounter_Provider_ID;
                    }
                }
                IList<Encounter> objEncounter = new List<Encounter>();
                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", EncID));
                objEncounter = crit.List<Encounter>();
                if (objEncounter.Count > 0)
                {
                    fillneweditappt.EncounterRecord = objEncounter[0];
                }
                fillneweditappt.bAuthcount = false;

                // fillneweditappt.Test = GetTestForEncounterID(EncID, HumanID);//not been used right now 12-01-2016 by vasanth

                //Commented by bala for create auth using new logic
                //After the Kumar mail, merge the below function. Added by srividhya on 15-Oct-2013
                //ICriteria critAuth = session.GetISession().CreateCriteria(typeof(AuthorizationEncounter)).Add(Expression.Eq("Encounter_ID", EncID)).Add(Expression.Eq("Human_ID", HumanID));
                //IList<AuthorizationEncounter> AuthLIst = critAuth.List<AuthorizationEncounter>();
                //if (AuthLIst.Count > 0 && AuthLIst != null)
                //{
                //    //Added by srividhya on 15-Oct-2013
                //    ICriteria critAuth1 = session.GetISession().CreateCriteria(typeof(Authorization)).Add(Expression.Eq("Id", AuthLIst[0]));
                //    IList<Authorization> AuthLIst1 = critAuth1.List<Authorization>();

                //    if (AuthLIst1.Count > 0)
                //    {
                //        fillneweditappt.AuthNo = AuthLIst1[0].Prior_Auth_No;
                //    }
                //}

                //if (objEncounter.Count > 0)//not been used right now 12-01-2016 by vasanth
                //{
                //    ICriteria critAuth1 = iMySession.CreateCriteria(typeof(Authorization)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Referred_To_Provider_ID", Convert.ToUInt64(objEncounter[0].Appointment_Provider_ID)));
                //    IList<Authorization> AuthLIst1 = critAuth1.List<Authorization>();
                //    if (AuthLIst1.Count > 0)
                //    {
                //        if (AuthLIst1.Count > 1)
                //        {
                //            fillneweditappt.bAuthcount = true;

                //        }
                //        else
                //        {
                //            fillneweditappt.bAuthcount = false;
                //            //if (AuthLIst1.All(a => Convert.ToDateTime(a.From_Date) >= Convert.ToDateTime(objEncounter[0].Appointment_Date) && Convert.ToDateTime(a.To_Date) <= Convert.ToDateTime(objEncounter[0].Appointment_Date)) == true)
                //            if (Convert.ToDateTime(AuthLIst1[0].From_Date).Date.CompareTo(Convert.ToDateTime(objEncounter[0].Appointment_Date.Date)) < 0 && Convert.ToDateTime(AuthLIst1[0].To_Date).Date.CompareTo(Convert.ToDateTime(objEncounter[0].Appointment_Date.Date)) > 0)
                //            {
                //                fillneweditappt.AuthNo = AuthLIst1[0].Auth_No;
                //                fillneweditappt.AuthorizationId = AuthLIst1[0].Id;
                //            }
                //        }
                //    }
                //}
                iMySession.Close();
            }
            return fillneweditappt;
        }


        //public IList<object> GetTestForEncounterID(ulong EncounterId, ulong HumanID)
        //{
        //    IList<object> obj = new List<object>();
        //    string TestOrdered = string.Empty;
        //    IList<string> temp = new List<string>();
        //    ISQLQuery sq = session.GetISession().CreateSQLQuery("SELECT O.* FROM orders O,wf_object W where W.obj_system_id=O.Order_Submit_ID and O.Human_ID=" + HumanID.ToString() + " and w.obj_type=\"DIAGNOSTIC ORDER\" and w.current_process=\"RESULT_PROCESS\" and O.CMG_Encounter_ID=" + EncounterId + ";")
        //           .AddEntity("O", typeof(Orders));
        //    //.AddEntity("W", typeof(WFObject));
        //    IList<Orders> SqResult = new List<Orders>();
        //    SqResult = sq.List<Orders>();
        //    temp = SqResult.Select(a => a.Lab_Procedure + "-" + a.Lab_Procedure_Description).ToList();
        //    IList<string> CptAndOrderID = new List<string>();
        //    foreach (Orders obj1 in SqResult)
        //    {
        //        CptAndOrderID.Add(obj1.Id.ToString());
        //    }


        //    foreach (string str in temp.Distinct())
        //    {
        //        if (TestOrdered.Trim() == string.Empty)
        //            TestOrdered = str;
        //        else
        //            TestOrdered += " , " + str;
        //    }
        //    obj.Add(TestOrdered);
        //    obj.Add(CptAndOrderID);

        //    return obj;

        //}

        public string GetTestForEncounterID(ulong EncounterId, ulong HumanID)
        {
            string TestOrdered = string.Empty;
            IList<string> temp = new List<string>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ISQLQuery sq = iMySession.CreateSQLQuery("SELECT O.* FROM orders O,wf_object W where W.obj_system_id=O.Order_Submit_ID and O.Human_ID=" + HumanID.ToString() + " and w.obj_type=\"LAB ORDER\" and w.current_process=\"ORDER_GENERATE\" and O.CMG_Encounter_ID=" + EncounterId + ";")
                       .AddEntity("O", typeof(Orders));
                //.AddEntity("W", typeof(WFObject));
                temp = sq.List<Orders>().Select(a => a.Lab_Procedure + "-" + a.Lab_Procedure_Description).ToList();
                foreach (string str in temp.Distinct())
                {
                    if (TestOrdered.Trim() == string.Empty)
                        TestOrdered = str;
                    else
                        TestOrdered += " , " + str;
                }
                iMySession.Close();
            }
            return TestOrdered;

        }

        //public void UpdateEncounterForRoom(ulong EncID, string ExamRoom, string ModifiedBy, DateTime ModifiedDateandTime, string MACAddress)
        public void UpdateEncounterForRoom(ulong EncID, string ExamRoom, string ModifiedBy, string ObjType, DateTime ModifiedDateandTime, string MACAddress)
        {
            IList<Encounter> encupdateList = GetEncounterByEncounterID(EncID);
            encupdateList[0].Encounter_ID = EncID;
            encupdateList[0].Exam_Room = ExamRoom;
            encupdateList[0].Modified_By = ModifiedBy;
            encupdateList[0].Modified_Date_and_Time = ModifiedDateandTime;
            IList<Encounter> encAddList = null;
            //SaveUpdateDeleteWithTransaction(ref encAddList, encupdateList, null, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref encAddList, ref encupdateList, null, MACAddress, true, false, EncID, string.Empty);
            //Added By Saravanakumar
            //WFObjectManager objWFObjectManager = new WFObjectManager();
            //objWFObjectManager.UpdateOwner(Convert.ToUInt64(EncID), ObjType, ModifiedBy, string.Empty);

        }

        public void UpdateEncounterforMedAsst(ulong ulEncID, string sMedAsstName, string sModifiedBy, DateTime dtModifiedDateandTime, string MACAddress)
        {
            ulong ID = 0;
            IList<Encounter> updateEncounterList = GetEncounterByEncounterID(ulEncID);
            updateEncounterList[0].Encounter_ID = ulEncID;
            updateEncounterList[0].Modified_By = sModifiedBy;
            updateEncounterList[0].Modified_Date_and_Time = dtModifiedDateandTime;
            if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "TECHNICIAN")
                updateEncounterList[0].Technician_ID = ID;
            else if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "PHYSICIAN")
                updateEncounterList[0].Reading_Provider_ID = ID;
            else if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "MEDICAL ASSISTANT")
                updateEncounterList[0].Assigned_Med_Asst_User_Name = sMedAsstName;
            else if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "SCRIBE")
                updateEncounterList[0].Assigned_Scribe_User_Name = sModifiedBy;
            IList<Encounter> addEncounterList = null;
            // SaveUpdateDeleteWithTransaction(ref addEncounterList, updateEncounterList, null, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref addEncounterList, ref updateEncounterList, null, MACAddress, true, false, ulEncID, string.Empty);
        }

        public void UpdateDateOfService(ulong ulEncID, DateTime dtDateOfService, string MACAddress, string sLocal_Time)
        {




            IList<Encounter> encupdateList = GetEncounterByEncounterID(ulEncID);
            encupdateList[0].Encounter_ID = ulEncID;
            encupdateList[0].Date_of_Service = dtDateOfService;
            encupdateList[0].Local_Time = sLocal_Time;
            IList<Encounter> addEncounterList = null;


            //string EncounterFileName = "Encounter" + "_" + ulEncID + ".xml";
            //string strXmlEncounterFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], EncounterFileName);
            //if (File.Exists(strXmlEncounterFilePath) == false)
            //{
            //    string sDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("Template_XML");
            //    string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(sXmlPath);
            //    itemDoc.Load(XmlText);
            //    XmlText.Close();

            //    XmlNodeList xmlAgenode = itemDoc.GetElementsByTagName("Age");
            //    if (xmlAgenode != null && xmlAgenode.Count > 0)
            //        xmlAgenode[0].ParentNode.RemoveChild(xmlAgenode[0]);
            //    itemDoc.Save(strXmlEncounterFilePath);
            //}
            //IList<object> ilstEncBlob = new List<object>();
            //IList<string> ilstEncounterTagList = new List<string>();
            //ilstEncounterTagList.Add("EncounterList");
            //ilstEncBlob = ReadBlob( ulEncID, ilstEncounterTagList);

            //if (File.Exists(strXmlEncounterFilePath) == false)
            //{

            //Jira CAP-1408
            EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
            IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(ulEncID);
            //Jira CAP-1408 checked if condion
            if (ilstEncounterBlob.Count == 0)
            {
                string sDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("Template_XML");
                string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                XmlDocument itemDoc = new XmlDocument();
                XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                itemDoc.Load(XmlText);
                XmlText.Close();

                XmlNodeList xmlAgenode = itemDoc.GetElementsByTagName("Age");
                if (xmlAgenode != null && xmlAgenode.Count > 0)
                    xmlAgenode[0].ParentNode.RemoveChild(xmlAgenode[0]);
                //itemDoc.Save(strXmlEncounterFilePath);

                ISession session = Session.GetISession();
                try
                {
                    using (ITransaction trans = session.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        WriteBlob(ulEncID, itemDoc, session, null, encupdateList, null, null, true);

                        trans.Commit();
                    }
                }
                catch (Exception ex1)
                {

                    throw new Exception(ex1.Message);
                }
            }

            //}
            // SaveUpdateDeleteWithTransaction(ref addEncounterList, encupdateList, null, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref addEncounterList, ref encupdateList, null, MACAddress, true, false, ulEncID, string.Empty);
        }
        // Added By Manimozhi - jan 25th 2012 - Performance Tuning- End

        //added By suresh on 31-jan-2012 for officeManager Report
        public DataSet DtblOfficeManagerSummaryReport(DateTime From_date, DateTime To_Date, string CurrentProcess, string CurrentOwner)
        {
            DataSet OffieceManagerSummaryReport = new DataSet();
            ArrayList arylstColumnsOfOfficeMangerReport;
            ArrayList arylstOfficeManger;
            DataTable dtblOfficceManagerReport = new DataTable();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                IQuery query2 = iMySession.GetNamedQuery("Fill.GetOfficeManagerSummaryReport.ColumnHeadings");
                arylstColumnsOfOfficeMangerReport = new ArrayList(query2.List());
                if (arylstColumnsOfOfficeMangerReport.Count > 0)
                {
                    object[] objlstColumns = (object[])arylstColumnsOfOfficeMangerReport[0];

                    for (int i = 0; i < objlstColumns.Length; i++)
                    {
                        if (i == 2 || i == 6 || i == 5 || i == 9)
                        {
                            dtblOfficceManagerReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                        }
                        else
                        {
                            dtblOfficceManagerReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                        }
                    }
                }
                IQuery query1 = iMySession.GetNamedQuery("Fill.GetOfficeManagerSummaryReport");
                IQuery query = iMySession.GetNamedQuery("Fill.GetOfficeManagerSummaryReportArchive");
                if (CurrentProcess.Trim() == string.Empty && CurrentOwner.Trim() == string.Empty)
                {
                    query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                    query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));

                    query.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                    query.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                }
                if (CurrentProcess.Trim() != string.Empty)
                {
                    query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                    query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));

                    query1.SetString(2, CurrentProcess);

                    query.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                    query.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));

                    query.SetString(2, CurrentProcess);
                }
                else
                {
                    query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                    query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                    query1.SetString(2, "%");

                    query.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                    query.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                    query.SetString(2, "%");
                }
                if (CurrentOwner.Trim() != string.Empty)
                {
                    query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                    query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                    query1.SetString(3, CurrentOwner);

                    query.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                    query.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                    query.SetString(3, CurrentOwner);
                }
                else
                {
                    query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                    query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                    query1.SetString(3, "%");

                    query.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                    query.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                    query.SetString(3, "%");
                }
                arylstOfficeManger = new ArrayList(query1.List());

                arylstOfficeManger.AddRange(query.List());

                if (arylstOfficeManger != null)
                {
                    for (int h = 0; h < arylstOfficeManger.Count; h++)
                    {

                        object[] objlst = (object[])arylstOfficeManger[h];
                        DataRow dr = dtblOfficceManagerReport.NewRow();
                        if (objlst.Length > 0)
                        {
                            for (int k = 0; k < objlst.Length; k++)
                            {

                                try
                                {

                                    dr[k] = objlst[k].ToString();
                                }
                                catch
                                {

                                }

                            }
                            dtblOfficceManagerReport.Rows.Add(dr);
                        }
                    }
                }



                OffieceManagerSummaryReport.Tables.Add(dtblOfficceManagerReport);
                iMySession.Close();
            }
            return OffieceManagerSummaryReport;
        }
        //added By suresh on 31-jan-2012 for officeManager Report
        //added By suresh on 11-April-2012 for Summary Report 
        public DataSet DtblAppointmentSummaryReport(DateTime From_date, DateTime To_Date, string PlanName)
        {
            ArrayList arylstColumnsOfAppointmentSummaryReport;
            ArrayList arylstAppointmentSummary = new ArrayList();
            DataTable dtblAppointmentSummary = new DataTable();
            DataSet dsResultSummary = new DataSet();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (PlanName == string.Empty)
                {
                    IQuery query2 = iMySession.GetNamedQuery("Fill.GetAppointmentSummaryReport.ColumnHeadings");
                    arylstColumnsOfAppointmentSummaryReport = new ArrayList(query2.List());
                    if (arylstColumnsOfAppointmentSummaryReport.Count > 0)
                    {
                        object[] objlstColumns = (object[])arylstColumnsOfAppointmentSummaryReport[0];

                        for (int i = 0; i < objlstColumns.Length; i++)
                        {
                            if (i == 0)
                            {
                                dtblAppointmentSummary.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                            }
                            else if (i == 1)
                            {
                                dtblAppointmentSummary.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                            }
                            else
                            {
                                dtblAppointmentSummary.Columns.Add(objlstColumns[i].ToString(), typeof(System.Int32));
                            }
                        }
                    }

                    IQuery query1 = iMySession.GetNamedQuery("ReportsAppointmentSummaryReport");
                    //query1.SetString(0, Fromdate);
                    // query1.SetString(1, ToDate);
                    query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                    query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                    arylstAppointmentSummary = new ArrayList(query1.List());
                    for (int h = 0; h < arylstAppointmentSummary.Count; h++)
                    {
                        object[] objlst = (object[])arylstAppointmentSummary[h];
                        DataRow dr = dtblAppointmentSummary.NewRow();
                        if (objlst.Length > 0)
                        {
                            for (int k = 0; k < objlst.Length; k++)
                            {
                                try
                                {
                                    dr[k] = objlst[k].ToString();

                                }
                                catch
                                {
                                    // string sMessage;
                                }
                            }
                            dtblAppointmentSummary.Rows.Add(dr);
                        }
                    }
                    DataSet AppointmentReport = new DataSet();
                    AppointmentReport.Tables.Add(dtblAppointmentSummary);
                    dsResultSummary = AppointmentReport;
                    //return AppointmentReport;
                }
                else if (PlanName != string.Empty)
                {
                    IQuery query2 = iMySession.GetNamedQuery("Fill.GetAppointmentSummaryReport.ColumnHeadings.withplan");
                    arylstColumnsOfAppointmentSummaryReport = new ArrayList(query2.List());
                    if (arylstColumnsOfAppointmentSummaryReport.Count > 0)
                    {
                        object[] objlstColumns = (object[])arylstColumnsOfAppointmentSummaryReport[0];

                        for (int i = 0; i < objlstColumns.Length; i++)
                        {
                            if (i == 0)
                            {
                                dtblAppointmentSummary.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                            }
                            else if (i == 1 || i == 2 || i == 3)
                            {
                                dtblAppointmentSummary.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                            }

                            else
                            {
                                dtblAppointmentSummary.Columns.Add(objlstColumns[i].ToString(), typeof(System.Int32));
                            }
                        }
                    }

                    if (PlanName == "ALL")
                    {
                        IQuery query1 = iMySession.GetNamedQuery("ReportsAppointmentSummaryReport.WithOut.Plan_Name");
                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                        arylstAppointmentSummary = new ArrayList(query1.List());
                    }
                    else if (PlanName != string.Empty)
                    {
                        IQuery query1 = iMySession.GetNamedQuery("ReportsAppointmentSummaryReport.WithPlan_Name");
                        query1.SetString(2, PlanName.ToString());
                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm"));
                        arylstAppointmentSummary = new ArrayList(query1.List());

                    }

                    for (int h = 0; h < arylstAppointmentSummary.Count; h++)
                    {
                        object[] objlst = (object[])arylstAppointmentSummary[h];
                        DataRow dr = dtblAppointmentSummary.NewRow();
                        if (objlst.Length > 0)
                        {
                            for (int k = 0; k < objlst.Length; k++)
                            {
                                try
                                {
                                    dr[k] = objlst[k].ToString();

                                }
                                catch
                                {
                                    //string sMessage;
                                }
                            }
                            dtblAppointmentSummary.Rows.Add(dr);
                        }
                    }
                    DataSet AppointmentReport = new DataSet();
                    AppointmentReport.Tables.Add(dtblAppointmentSummary);
                    dsResultSummary = AppointmentReport;
                    //return AppointmentReport;
                }
                iMySession.Close();
            }
            return dsResultSummary;
        }

        public void MoveToNextProcessFromAfterPlan(Encounter encounterRecord, ulong ulEncounterID, ulong ulHumanID, ulong ulSelectedPhyID, string sUserName, string sButtonName, string sFacilityName, string sMacAddress, DateTime dtCurrentDateTime, bool bMovetoReview, WFObject objEncWfObj, WFObject objDocWfObject, WFObject objDocReviewWfObj, string UserRole)
        {
            WFObject EncounterWfObject = null;
            WFObject DocumentationWfObject = null;
            WFObject BillingWfObject = null;
            WFObjectManager wfObjMngr = new WFObjectManager();
            EncounterManager encMngr = new EncounterManager();
            //string sRoleSelectedPhyID = string.Empty;
            //string sRoleLoginUser = string.Empty;
            string sOwner = string.Empty;

            /***  commented for perfomance tuning
              * encounter is passing in argument
              *  by Jisha    
           ***/

            //Encounter objEncounter = encMngr.GetEncounterByEncounterID(ulEncounterID)[0];

            Encounter objEncounter = encounterRecord;
            //objEncounter.Is_Document_Given = encounterRecord.Is_Document_Given;
            //objEncounter.Due_On = encounterRecord.Due_On;
            //objEncounter.Follow_Reason_Notes = encounterRecord.Follow_Reason_Notes;
            //objEncounter.Is_PFSH_Verified = encounterRecord.Is_PFSH_Verified;
            //objEncounter.Source_Of_Information = encounterRecord.Source_Of_Information;
            //objEncounter.If_Source_Of_Information_Others = encounterRecord.If_Source_Of_Information_Others;
            objEncounter.Encounter_Provider_ID = encounterRecord.Encounter_Provider_ID;
            objEncounter.Encounter_Provider_Signed_Date = encounterRecord.Encounter_Provider_Signed_Date;
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //#region GetUserRoleAndOwner
                //if (ulSelectedPhyID != 0)
                //{
                //    ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", ulSelectedPhyID));
                //    if (criteria.List<User>() != null && criteria.List<User>().Count > 0)
                //    {
                //        sRoleSelectedPhyID = criteria.List<User>()[0].role;
                //    }
                //}
                //ICriteria crit = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("user_name", sUserName));
                //if (crit.List<User>() != null && crit.List<User>().Count > 0)
                //{
                //    sRoleLoginUser = crit.List<User>()[0].role;
                //}

                //sOwner = sUserName;
                //#endregion

                /***  commented for perfomance tuning
               * EncounterWfObject,DocumentationWfObject  is passing in argument
               *  by Jisha 
            ***/


                //#region GetWfObject
                //EncounterWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "ENCOUNTER");
                //DocumentationWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "DOCUMENTATION");
                //DocumentReviewWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "DOCUMENT REVIEW");
                //#endregion

                #region GetWfObject
                if (sButtonName.ToUpper() == "MOVE TO NEXT PROCESS")
                {
                    EncounterWfObject = objEncWfObj;
                    DocumentationWfObject = objDocWfObject;
                }
                else
                {
                    EncounterWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "ENCOUNTER");
                    DocumentationWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "DOCUMENTATION");
                }


                #endregion



                #region MoveToCheckOut
                if (sButtonName.ToUpper() == "MOVE TO CHECKOUT")
                {
                    if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "MA_PROCESS")
                    {
                        UpdateEncounter(objEncounter, sMacAddress, new object[] { "false" });
                        //Move to check out wait
                        wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                        OrdersManager ordMnger = new OrdersManager();
                        //comment by bala
                        //ordMnger.SubmitOrdersToLab(ulHumanID, ulEncounterID, "DIAGNOSTIC ORDER", sMacAddress, sUserName, dtCurrentDateTime, encounterRecord.Assigned_Med_Asst_User_Name);
                        ordMnger.SubmitLogic(ulHumanID, ulEncounterID, ulSelectedPhyID, "DIAGNOSTIC ORDER", sUserName, dtCurrentDateTime, sFacilityName);
                        ordMnger.SubmitOrdersToLab(ulHumanID, ulEncounterID, "IMAGE ORDER", sMacAddress, sUserName, dtCurrentDateTime, objEncounter.Assigned_Med_Asst_User_Name);
                    }
                    else if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT")
                    {
                        //Added by Janani to update Follow up details in encounter
                        UpdateEncounter(objEncounter, sMacAddress, new object[] { "false" });
                        //Move to check out
                        wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                    }
                }
                #endregion

                #region MoveToProvider
                else if (sButtonName.ToUpper() == "MOVE TO NEXT PROCESS" && UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                {
                    if (DocumentationWfObject != null && DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_PROCESS")
                    {
                        if (objEncounter != null)
                        {
                            ////Commented out as Is_EandM_Submitted should always be "Y" whenever PhyAssistant moves the encounter to next Process

                            ////uncommented for GITLAB #3084
                            //if (!bMovetoReview)//if the encounter is moved to PROVIDER for PROVIDER_REVIEW -- do not set e_m_submitted to "Y", to prevent batch creation when Documentation object is in correction/review cycle.
                            //objEncounter.Is_EandM_Submitted = "Y";
                            //else
                            //    objEncounter.Is_EandM_Submitted = "N";

                            //GitLab #3240
                            if (!bMovetoReview)
                            {
                                objEncounter.Is_EandM_Submitted = "Y";
                                objEncounter.E_M_Submitted_Date_And_Time = DateTime.Now;
                            }
                            if (bMovetoReview == true)
                                objEncounter.Encounter_Provider_Review_ID = Convert.ToInt32(ulSelectedPhyID);
                            else
                                objEncounter.Encounter_Provider_Review_ID = 0;
                            UpdateEncounter(objEncounter, sMacAddress, new object[] { "false" });
                            //string sCurrentOwner = string.Empty;
                            //ICriteria crite = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", ulSelectedPhyID));
                            //if (crite.List<User>() != null && crite.List<User>().Count > 0)
                            //{
                            //    sCurrentOwner = crite.List<User>()[0].user_name;
                            //}
                            string sCurrentOwner = "UNKNOWN";
                            SubmitOrdersAndPrescriptions(ulEncounterID, ulHumanID, Convert.ToUInt64(objEncounter.Encounter_Provider_ID), sUserName, sMacAddress, sFacilityName, dtCurrentDateTime);
                            /* if (bMovetoReview)
                             {
                                 WFObject ehrNewWFObj = new WFObject();
                                 ehrNewWFObj.Fac_Name = EncounterWfObject.Fac_Name;
                                 ehrNewWFObj.Obj_Type = "DOCUMENT REVIEW";
                                 ehrNewWFObj.Parent_Obj_Type = EncounterWfObject.Obj_Type;
                                 ehrNewWFObj.Parent_Obj_System_Id = ulEncounterID;
                                 ehrNewWFObj.Obj_System_Id = ulEncounterID;
                                 ehrNewWFObj.Current_Process = "START";
                                 ehrNewWFObj.Current_Arrival_Time = dtCurrentDateTime;
                                 ehrNewWFObj.Current_Owner = sCurrentOwner;
                                 wfObjMngr.InsertToRCopiaWorkFlowObject(ehrNewWFObj, 1, sMacAddress);

                                 if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT")
                                 {
                                     wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                                 }
                             }
                             // Added on 26th dec 2011 by manimozhi 
                             // Directly move from check_out_wait to checkout in PA login.
                             else
                             {
                                 if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT")
                                 {
                                     wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                                 }
                             }
                             wfObjMngr.MoveToNextProcess(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                             */
                            if (bMovetoReview)
                            {
                                wfObjMngr.MoveToNextProcess(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, 5, sCurrentOwner, dtCurrentDateTime, sMacAddress, null, null);

                                DocumentationWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "DOCUMENTATION");
                                if (DocumentationWfObject.Current_Process == "PROVIDER_REVIEW")
                                {
                                    if (ulSelectedPhyID != 0)
                                    {
                                        ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", ulSelectedPhyID));
                                        if (criteria.List<User>() != null && criteria.List<User>().Count > 0)
                                        {
                                            sCurrentOwner = criteria.List<User>()[0].user_name;
                                            wfObjMngr.UpdateOwner(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, sCurrentOwner, string.Empty);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                wfObjMngr.MoveToNextProcess(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);

                                DocumentationWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "DOCUMENTATION");
                                if (DocumentationWfObject.Current_Process == "DOCUMENT_COMPLETE")
                                {
                                    BillingWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "BILLING");
                                    if (BillingWfObject.Current_Process == "BATCHING_WAIT")
                                        wfObjMngr.MoveToNextProcess(BillingWfObject.Obj_System_Id, BillingWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);

                                    //Jira CAP-340
                                    EncounterBlobManager objEncblobmngr = new EncounterBlobManager();
                                    objEncblobmngr.LockEncounter(ulEncounterID, ulHumanID, sUserName, dtCurrentDateTime);
                                }
                            }
                            if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT")
                            {
                                wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                            }
                        }
                    }
                }
                #endregion

                #region MoveToNextProcess
                else if (sButtonName.ToUpper() == "MOVE TO NEXT PROCESS")
                {
                    int CloseType = 1;
                    //Selvaraman - 5-Apr-12
                    //For Wellness - the documentwfobject will be in provider_review
                    if (DocumentationWfObject != null && (DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_PROCESS" || DocumentationWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW" || DocumentationWfObject.Current_Process.ToUpper() == "TECHNICIAN_PROCESS"))//CMG Ancilliary
                    {
                        if (objEncounter != null)
                        {
                            IList<object> query1 = new List<object>();
                            //Jira #CAP-9
                            //query1 = iMySession.CreateSQLQuery("Update encounter set Encounter_Provider_ID='" + objEncounter.Encounter_Provider_ID + "',Encounter_Provider_Signed_Date='" + objEncounter.Encounter_Provider_Signed_Date.ToString("yyyy-MM-dd hh:mm:ss") + "',Is_EandM_Submitted = 'Y', E_M_Submitted_Date_And_Time='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' " + " where Encounter_ID='" + ulEncounterID + "'").List<object>();
                            query1 = iMySession.CreateSQLQuery("Update encounter set Encounter_Provider_ID='" + objEncounter.Encounter_Provider_ID + "',Encounter_Provider_Signed_Date='" + objEncounter.Encounter_Provider_Signed_Date.ToString("yyyy-MM-dd HH:mm:ss") + "',Is_EandM_Submitted = 'Y', E_M_Submitted_Date_And_Time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " + " where Encounter_ID='" + ulEncounterID + "'").List<object>();
                            // UpdateEncounter(objEncounter, sMacAddress, new object[] { "false" });
                            SubmitOrdersAndPrescriptions(ulEncounterID, ulHumanID, Convert.ToUInt64(objEncounter.Encounter_Provider_ID), sUserName, sMacAddress, sFacilityName, dtCurrentDateTime);
                            wfObjMngr.MoveToNextProcess(DocumentationWfObject.Obj_System_Id, DocumentationWfObject.Obj_Type, CloseType, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                            if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT")
                            {
                                wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                            }

                            DocumentationWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "DOCUMENTATION");
                            if (DocumentationWfObject.Current_Process == "DOCUMENT_COMPLETE")
                            {
                                BillingWfObject = wfObjMngr.GetByObjectSystemId(ulEncounterID, "BILLING");
                                if (BillingWfObject.Current_Process == "BATCHING_WAIT")
                                    wfObjMngr.MoveToNextProcess(BillingWfObject.Obj_System_Id, BillingWfObject.Obj_Type, CloseType, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);

                                //Jira CAP-340
                                EncounterBlobManager objEncblobmngr = new EncounterBlobManager();
                                objEncblobmngr.LockEncounter(ulEncounterID, ulHumanID, sUserName, dtCurrentDateTime);
                            }
                        }
                    }

                    //else if (DocumentReviewWfObject != null && DocumentReviewWfObject.Current_Process.ToUpper() == "PROVIDER_REVIEW")
                    //{
                    //    //if (sRoleLoginUser.ToUpper() == "PHYSICIAN")
                    //    //{
                    //    wfObjMngr.MoveToNextProcess(DocumentReviewWfObject.Obj_System_Id, DocumentReviewWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                    //    if (EncounterWfObject != null && EncounterWfObject.Current_Process.ToUpper() == "CHECK_OUT_WAIT")
                    //    {
                    //        wfObjMngr.MoveToNextProcess(EncounterWfObject.Obj_System_Id, EncounterWfObject.Obj_Type, 1, "UNKNOWN", dtCurrentDateTime, sMacAddress, null, null);
                    //    }
                    //    //}
                    //}
                }
                iMySession.Close();
            }
            #endregion
        }

        //added By suresh on 11-April-2012 for Summary Report 
        public DataSet DtblPatientSeenReport(DateTime From_date, DateTime To_Date, string ReportFor, bool IncludeHouseCallVisit, string Active)
        {

            DataSet ResultDataSet = new DataSet();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (ReportFor == "PATIENT SEEN")
                {
                    ArrayList arylstColumnsOfPatientSeenReport;
                    ArrayList arylstPatientSeenReport;
                    DataTable dtblPatientSeenReport = new DataTable();
                    //string Fromdate = "2011-02-17";
                    // string ToDate = "2011-02-20";


                    IQuery query2 = iMySession.GetNamedQuery("Fill.PateitSeenReport.ColumnHeadings");
                    arylstColumnsOfPatientSeenReport = new ArrayList(query2.List());
                    if (arylstColumnsOfPatientSeenReport != null)
                    {
                        if (arylstColumnsOfPatientSeenReport.Count > 0)
                        {
                            object[] objlstColumns = (object[])arylstColumnsOfPatientSeenReport[0];

                            for (int i = 0; i < objlstColumns.Length; i++)
                            {
                                if (i == 11 || i == 12 || i == 3)
                                {
                                    dtblPatientSeenReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                                }
                                else
                                {

                                    dtblPatientSeenReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                                }
                            }
                        }
                    }
                    if (IncludeHouseCallVisit == true)
                    {
                        IQuery query1 = iMySession.GetNamedQuery("Reports.PateitSeenReport");
                        //query1.SetString(0, Fromdate);
                        // query1.SetString(2, sFacilityName);

                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm:ss"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm:ss"));

                        if (Active != string.Empty)
                        {
                            query1.SetString(2, Active);
                        }
                        else
                        {
                            query1.SetString(2, "%");
                        }
                        query1.SetString(3, From_date.ToString("yyyy-MM-dd hh:mm:ss"));
                        query1.SetString(4, To_Date.ToString("yyyy-MM-dd hh:mm:ss"));
                        arylstPatientSeenReport = new ArrayList(query1.List());
                    }
                    else
                    {
                        IQuery query1 = iMySession.GetNamedQuery("Reports.PateitSeenReport.ExcludingHouseCalls");
                        //query1.SetString(0, Fromdate);
                        // query1.SetString(2, sFacilityName);

                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm:ss"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm:ss"));
                        if (Active != string.Empty)
                        {
                            query1.SetString(2, Active);
                        }
                        else
                        {
                            query1.SetString(2, "%");
                        }
                        arylstPatientSeenReport = new ArrayList(query1.List());
                    }
                    if (arylstPatientSeenReport != null)
                    {
                        for (int h = 0; h < arylstPatientSeenReport.Count; h++)
                        {
                            object[] objlst = (object[])arylstPatientSeenReport[h];
                            DataRow dr = dtblPatientSeenReport.NewRow();
                            if (objlst.Length > 0)
                            {
                                for (int k = 0; k < objlst.Length; k++)
                                {

                                    try
                                    {
                                        if (objlst[k].ToString().Split(',').Length > 0)
                                        {
                                            if (objlst[k].ToString().Split(',')[0] == string.Empty && objlst[k].ToString().Split(',')[1] == string.Empty && objlst[k].ToString().Split(',')[2] == string.Empty && objlst[k].ToString().Split(',')[3] == string.Empty)
                                                objlst[k] = string.Empty;
                                            else if (objlst[k].ToString().Split(',')[0] == string.Empty)
                                                objlst[k] = objlst[k].ToString().Split(',')[1];
                                        }
                                        dr[k] = objlst[k].ToString();
                                    }
                                    catch
                                    {
                                        //  string sMessage;
                                    }
                                }
                                dtblPatientSeenReport.Rows.Add(dr);
                            }
                        }
                    }
                    DataSet PatientSeenReport = new DataSet();
                    PatientSeenReport.Tables.Add(dtblPatientSeenReport);
                    ResultDataSet = PatientSeenReport;
                }
                else if (ReportFor == "PATIENT NOT SEEN AND SCHEDULED")
                {
                    ArrayList arylstColumnsOfPatientSeenReport;
                    ArrayList arylstPatientSeenReport;
                    DataTable dtblPatientSeenReport = new DataTable();
                    //string Fromdate = "2011-02-17";
                    // string ToDate = "2011-02-20";
                    IQuery query2 = iMySession.GetNamedQuery("Fill.PateitNotSeenReport.ColumnHeadings");
                    arylstColumnsOfPatientSeenReport = new ArrayList(query2.List());
                    if (arylstColumnsOfPatientSeenReport != null)
                    {
                        if (arylstColumnsOfPatientSeenReport.Count > 0)
                        {
                            object[] objlstColumns = (object[])arylstColumnsOfPatientSeenReport[0];

                            for (int i = 0; i < objlstColumns.Length; i++)
                            {
                                if (i == 2)
                                {
                                    dtblPatientSeenReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                                }
                                else
                                {

                                    dtblPatientSeenReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                                }
                            }
                        }
                    }
                    if (IncludeHouseCallVisit == true)
                    {
                        IQuery query1 = iMySession.GetNamedQuery("Reports.NotSeenReport.And.Scheduled");
                        //query1.SetString(0, Fromdate);
                        // query1.SetString(2, sFacilityName);

                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm:ss"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm:ss"));

                        if (Active != string.Empty)
                        {
                            query1.SetString(2, Active);
                        }
                        else
                        {
                            query1.SetString(2, "%");
                        }
                        arylstPatientSeenReport = new ArrayList(query1.List());
                    }
                    else
                    {
                        IQuery query1 = iMySession.GetNamedQuery("Reports.NotSeenReport.ExcludingHouseCalls.And.Scheduled");
                        //query1.SetString(0, Fromdate);
                        // query1.SetString(2, sFacilityName);

                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm:ss"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm:ss"));
                        if (Active != string.Empty)
                        {
                            query1.SetString(2, Active);
                        }
                        else
                        {
                            query1.SetString(2, "%");
                        }
                        arylstPatientSeenReport = new ArrayList(query1.List());
                    }
                    if (arylstPatientSeenReport != null)
                    {
                        for (int h = 0; h < arylstPatientSeenReport.Count; h++)
                        {
                            object[] objlst = (object[])arylstPatientSeenReport[h];
                            DataRow dr = dtblPatientSeenReport.NewRow();
                            if (objlst.Length > 0)
                            {
                                for (int k = 0; k < objlst.Length; k++)
                                {

                                    try
                                    {
                                        if (objlst[k].ToString().Split(',').Length > 0)
                                        {
                                            if (objlst[k].ToString().Split(',')[0] == string.Empty && objlst[k].ToString().Split(',')[1] == string.Empty && objlst[k].ToString().Split(',')[2] == string.Empty && objlst[k].ToString().Split(',')[3] == string.Empty)
                                                objlst[k] = string.Empty;
                                            else if (objlst[k].ToString().Split(',')[0] == string.Empty)
                                                objlst[k] = objlst[k].ToString().Split(',')[1];
                                        }
                                        dr[k] = objlst[k].ToString();
                                    }
                                    catch
                                    {
                                        // string sMessage;
                                    }
                                }
                                dtblPatientSeenReport.Rows.Add(dr);
                            }
                        }
                    }
                    DataSet PatientSeenReport = new DataSet();
                    PatientSeenReport.Tables.Add(dtblPatientSeenReport);
                    ResultDataSet = PatientSeenReport;
                }
                else if (ReportFor == "PATIENT NOT SEEN")
                {
                    ArrayList arylstColumnsOfPatientNotSeenReport;
                    ArrayList arylstPatientNotSeenReport;
                    DataTable dtblPatientNotSeenReport = new DataTable();
                    //string Fromdate = "2011-02-17";
                    // string ToDate = "2011-02-20";
                    IQuery query2 = iMySession.GetNamedQuery("Fill.PateitNotSeenReport.ColumnHeadings");
                    arylstColumnsOfPatientNotSeenReport = new ArrayList(query2.List());
                    if (arylstColumnsOfPatientNotSeenReport != null)
                    {
                        if (arylstColumnsOfPatientNotSeenReport.Count > 0)
                        {
                            object[] objlstColumns = (object[])arylstColumnsOfPatientNotSeenReport[0];

                            for (int i = 0; i < objlstColumns.Length; i++)
                            {
                                if (i == 2)
                                {
                                    dtblPatientNotSeenReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                                }
                                else
                                {

                                    dtblPatientNotSeenReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                                }
                            }
                        }
                    }
                    if (IncludeHouseCallVisit == true)
                    {
                        IQuery query1 = iMySession.GetNamedQuery("Reports.PateitNotSeenReport");
                        //query1.SetString(0, Fromdate);
                        // query1.SetString(2, sFacilityName);

                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm:ss"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm:ss"));
                        query1.SetString(2, From_date.ToString("yyyy-MM-dd hh:mm:ss"));
                        query1.SetString(3, To_Date.ToString("yyyy-MM-dd hh:mm:ss"));
                        if (Active != string.Empty)
                        {
                            query1.SetString(4, Active);
                        }
                        else
                        {
                            query1.SetString(4, "%");
                        }
                        arylstPatientNotSeenReport = new ArrayList(query1.List());
                    }
                    else
                    {
                        IQuery query1 = iMySession.GetNamedQuery("Reports.PateitNotSeenReport.ExcludingHouseCalls");
                        //query1.SetString(0, Fromdate);
                        // query1.SetString(2, sFacilityName);

                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm:ss"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm:ss"));
                        if (Active != string.Empty)
                        {
                            query1.SetString(2, Active);
                        }
                        else
                        {
                            query1.SetString(2, "%");
                        }
                        arylstPatientNotSeenReport = new ArrayList(query1.List());
                    }
                    if (arylstPatientNotSeenReport != null)
                    {
                        for (int h = 0; h < arylstPatientNotSeenReport.Count; h++)
                        {
                            object[] objlst = (object[])arylstPatientNotSeenReport[h];
                            DataRow dr = dtblPatientNotSeenReport.NewRow();
                            if (objlst.Length > 0)
                            {
                                for (int k = 0; k < objlst.Length; k++)
                                {

                                    try
                                    {
                                        if (objlst[k].ToString().Split(',').Length > 0)
                                        {
                                            if (objlst[k].ToString().Split(',')[0] == string.Empty && objlst[k].ToString().Split(',')[1] == string.Empty && objlst[k].ToString().Split(',')[2] == string.Empty && objlst[k].ToString().Split(',')[3] == string.Empty)
                                                objlst[k] = string.Empty;
                                            else if (objlst[k].ToString().Split(',')[0] == string.Empty)
                                                objlst[k] = objlst[k].ToString().Split(',')[1];
                                        }
                                        dr[k] = objlst[k].ToString();
                                    }
                                    catch
                                    {
                                        //string sMessage;
                                    }
                                }
                                dtblPatientNotSeenReport.Rows.Add(dr);
                            }
                        }
                    }
                    DataSet PatientSeenReport = new DataSet();
                    PatientSeenReport.Tables.Add(dtblPatientNotSeenReport);
                    ResultDataSet = PatientSeenReport;
                }
                else if (ReportFor == "PATIENT NOT SEEN AND NOT SCHEDULED")
                {
                    ArrayList arylstColumnsOfPatientNotSeenReport;
                    ArrayList arylstPatientNotSeenReport;
                    DataTable dtblPatientNotSeenReport = new DataTable();
                    //string Fromdate = "2011-02-17";
                    // string ToDate = "2011-02-20";
                    IQuery query2 = iMySession.GetNamedQuery("Fill.PateitNotSeenReport.ColumnHeadings");
                    arylstColumnsOfPatientNotSeenReport = new ArrayList(query2.List());
                    if (arylstColumnsOfPatientNotSeenReport != null)
                    {
                        if (arylstColumnsOfPatientNotSeenReport.Count > 0)
                        {
                            object[] objlstColumns = (object[])arylstColumnsOfPatientNotSeenReport[0];

                            for (int i = 0; i < objlstColumns.Length; i++)
                            {
                                if (i == 2)
                                {
                                    dtblPatientNotSeenReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                                }
                                else
                                {

                                    dtblPatientNotSeenReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                                }
                            }
                        }
                    }
                    if (IncludeHouseCallVisit == true)
                    {
                        IQuery query1 = iMySession.GetNamedQuery("Reports.PateitNotSeenReport.NotScheduled");
                        //query1.SetString(0, Fromdate);
                        // query1.SetString(2, sFacilityName);

                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm:ss"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm:ss"));
                        query1.SetString(2, From_date.ToString("yyyy-MM-dd hh:mm:ss"));
                        query1.SetString(3, To_Date.ToString("yyyy-MM-dd hh:mm:ss"));
                        if (Active != string.Empty)
                        {
                            query1.SetString(4, Active);
                        }
                        else
                        {
                            query1.SetString(4, "%");
                        }
                        arylstPatientNotSeenReport = new ArrayList(query1.List());
                    }
                    else
                    {
                        IQuery query1 = iMySession.GetNamedQuery("Reports.PateitNotSeenReport.NotScheduled.ExcludingHouseCalls");
                        //query1.SetString(0, Fromdate);
                        // query1.SetString(2, sFacilityName);

                        query1.SetString(0, From_date.ToString("yyyy-MM-dd hh:mm:ss"));
                        query1.SetString(1, To_Date.ToString("yyyy-MM-dd hh:mm:ss"));
                        if (Active != string.Empty)
                        {
                            query1.SetString(2, Active);
                        }
                        else
                        {
                            query1.SetString(2, "%");
                        }
                        arylstPatientNotSeenReport = new ArrayList(query1.List());
                    }
                    if (arylstPatientNotSeenReport != null)
                    {
                        for (int h = 0; h < arylstPatientNotSeenReport.Count; h++)
                        {
                            object[] objlst = (object[])arylstPatientNotSeenReport[h];
                            DataRow dr = dtblPatientNotSeenReport.NewRow();
                            if (objlst.Length > 0)
                            {
                                for (int k = 0; k < objlst.Length; k++)
                                {

                                    try
                                    {
                                        if (objlst[k].ToString().Split(',').Length > 0)
                                        {
                                            if (objlst[k].ToString().Split(',')[0] == string.Empty && objlst[k].ToString().Split(',')[1] == string.Empty && objlst[k].ToString().Split(',')[2] == string.Empty && objlst[k].ToString().Split(',')[3] == string.Empty)
                                                objlst[k] = string.Empty;
                                            else if (objlst[k].ToString().Split(',')[0] == string.Empty)
                                                objlst[k] = objlst[k].ToString().Split(',')[1];
                                        }
                                        dr[k] = objlst[k].ToString();
                                    }
                                    catch
                                    {
                                        //string sMessage;
                                    }
                                }
                                dtblPatientNotSeenReport.Rows.Add(dr);
                            }
                        }
                    }
                    DataSet PatientSeenReport = new DataSet();
                    PatientSeenReport.Tables.Add(dtblPatientNotSeenReport);
                    ResultDataSet = PatientSeenReport;
                }
                iMySession.Close();
            }
            return ResultDataSet;
        }

        public IList<WorkFlow> GetWorkFlowMapListForReports()
        {

            IList<WorkFlow> WFMapList = new List<WorkFlow>();
            WorkFlow objwfobject;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("GetWorflowProcessForReports");
                ArrayList aryWFList = new ArrayList(query.List());


                if (aryWFList != null)
                {
                    for (int i = 0; i < aryWFList.Count; i++)
                    {
                        objwfobject = new WorkFlow();
                        object oj = (object)aryWFList[i];
                        objwfobject.To_Process = oj.ToString();
                        WFMapList.Add(objwfobject);
                    }
                }
                iMySession.Close();
            }
            return WFMapList;
        }

        //Added On 11-12-2012 by Saravanakumar
        public DataSet DtblRefillReport(int phyid, DateTime From_date, DateTime To_Date)
        {
            ArrayList arylstRefillReport;
            ArrayList arylstRefill;
            DataTable dtblRefillReport = new DataTable();
            DataSet RefillReport = new DataSet();
            //string Fromdate = "2011-02-17";
            // string ToDate = "2011-02-20";
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                IQuery query2 = iMySession.GetNamedQuery("Fill.GetRefillReport.ColumnHeadings");
                arylstRefillReport = new ArrayList(query2.List());
                if (arylstRefillReport != null)
                {
                    if (arylstRefillReport.Count > 0)
                    {
                        object[] objlstColumns = (object[])arylstRefillReport[0];

                        for (int i = 0; i < objlstColumns.Length; i++)
                        {
                            if (i == 4)
                            {
                                dtblRefillReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                            }
                            else
                            {
                                dtblRefillReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                            }
                        }
                    }
                }

                IQuery query1 = iMySession.GetNamedQuery("Reports.GetRefillReport");
                //query1.SetString(0, Fromdate);
                // query1.SetString(1, ToDate);
                query1.SetString(0, phyid.ToString());
                //Modified by priyangha on 26/12/2012 
                query1.SetString(1, From_date.ToString("yyyy-MM-dd"));
                query1.SetString(2, To_Date.ToString("yyyy-MM-dd"));

                arylstRefill = new ArrayList(query1.List());
                if (arylstRefill != null)
                {
                    for (int h = 0; h < arylstRefill.Count; h++)
                    {
                        object[] objlst = (object[])arylstRefill[h];
                        DataRow dr = dtblRefillReport.NewRow();
                        if (objlst.Length > 0)
                        {
                            for (int k = 0; k < objlst.Length; k++)
                            {
                                try
                                {
                                    if (objlst[k].ToString().Split(',').Length > 0)
                                    {
                                        if (objlst[k].ToString().Split(',')[0] == string.Empty && objlst[k].ToString().Split(',')[1] == string.Empty && objlst[k].ToString().Split(',')[2] == string.Empty && objlst[k].ToString().Split(',')[3] == string.Empty)
                                            objlst[k] = string.Empty;
                                        else if (objlst[k].ToString().Split(',')[0] == string.Empty)
                                            objlst[k] = objlst[k].ToString().Split(',')[1];
                                    }
                                    dr[k] = objlst[k].ToString();
                                }
                                catch
                                {
                                    //string sMessage;
                                }
                            }
                            dtblRefillReport.Rows.Add(dr);
                        }
                    }
                }

                RefillReport.Tables.Add(dtblRefillReport);
                iMySession.Close();
            }
            return RefillReport;
        }

        public string GetTechnicianIDOrReadingProviderID(string Name, ref ulong ID)
        {
            //ulong ID = 0;
            string Role = string.Empty;
            UserManager objUserManager = new UserManager();
            IList<User> UserList = objUserManager.GetUser(Name);
            if (UserList != null && UserList.Count > 0)
            {
                if (UserList[0].role.ToUpper().Trim() == "TECHNICIAN")
                {
                    Role = "TECHNICIAN";
                    ID = UserList[0].Physician_Library_ID;
                }
                else if (UserList[0].role.ToUpper().Trim() == "PHYSICIAN")
                {
                    Role = "PHYSICIAN";
                    ID = UserList[0].Physician_Library_ID;
                }
                else if (UserList[0].role.ToUpper().Trim() == "MEDICAL ASSISTANT")
                {
                    Role = "MEDICAL ASSISTANT";
                    ID = UserList[0].Physician_Library_ID;
                }
                else if (UserList[0].role.ToUpper().Trim() == "OFFICE MANAGER")
                {
                    Role = "OFFICE MANAGER";
                    ID = UserList[0].Physician_Library_ID;
                }
                else if (UserList[0].role.ToUpper().Trim() == "SCRIBE")
                {
                    Role = "SCRIBE";
                    ID = UserList[0].Physician_Library_ID;
                }
            }

            return Role;
        }
        public FillNewEditAppointment GetEncounterAndHumanRecordForRCM(ulong EncID, ulong HumanID)
        {
            FillNewEditAppointment fillneweditappt = new FillNewEditAppointment();

            IList<Human> objHuman = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteriaByHumanId = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanID));
                objHuman = criteriaByHumanId.List<Human>();

                if (objHuman.Count > 0)
                {
                    foreach (Human obj in objHuman)
                    {
                        fillneweditappt.Human_ID = obj.Id;
                        fillneweditappt.First_Name = obj.First_Name;
                        fillneweditappt.Last_Name = obj.Last_Name;
                        fillneweditappt.Home_Phone_No = obj.Home_Phone_No;
                        fillneweditappt.Cell_Phone_No = obj.Cell_Phone_Number;
                        fillneweditappt.Birth_Date = obj.Birth_Date;
                        fillneweditappt.MI = obj.MI;
                        fillneweditappt.Suffix = obj.Suffix;
                        fillneweditappt.HumanType = obj.Human_Type;

                    }
                }
                IList<Encounter> objEncounter = new List<Encounter>();
                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", EncID));
                objEncounter = crit.List<Encounter>();
                if (objEncounter.Count > 0)
                {
                    fillneweditappt.EncounterRecord = objEncounter[0];
                }

                fillneweditappt.Test = GetTestForEncounterID(EncID, HumanID);


                ICriteria critAuth = iMySession.CreateCriteria(typeof(AuthorizationEncounter)).Add(Expression.Eq("Encounter_ID", EncID)).Add(Expression.Eq("Human_ID", HumanID));
                IList<AuthorizationEncounter> AuthLIst = critAuth.List<AuthorizationEncounter>();
                if (AuthLIst.Count > 0 && AuthLIst != null)
                {
                    //Added by srividhya on 15-Oct-2013
                    ICriteria critAuth1 = iMySession.CreateCriteria(typeof(Authorization)).Add(Expression.Eq("Id", AuthLIst[0]));
                    IList<Authorization> AuthLIst1 = critAuth1.List<Authorization>();

                    if (AuthLIst1.Count > 0)
                    {
                        //fillneweditappt.AuthNo = AuthLIst1[0].Auth_No;
                    }
                }
                iMySession.Close();
            }
            return fillneweditappt;
        }
        public ulong CreateEncounterForRCM(Encounter EncounterRecord, WFObject wfObjectRecord, string MACAddress, string AuthorizationNo, object[] IsCMGLabObject)
        {
            iTryCount = 0;

        TryAgain:
            int iResult = 0;
            ISession MySession = Session.GetISession();
            IList<Encounter> EncList = new List<Encounter>();
            IList<Encounter> EncListUpdate = null;
            GenerateXml XMLObj = new GenerateXml();
            //ITransaction trans = null;
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {

                    try
                    {
                        //trans = MySession.BeginTransaction();

                        EncList.Add(EncounterRecord);
                        // iResult = SaveUpdateDeleteWithoutTransaction(ref EncList, null, null, MySession, MACAddress);
                        iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref EncList, ref EncListUpdate, null, MySession, MACAddress, false, false, EncList[0].Id, string.Empty, ref XMLObj);
                        //Added for CMG Lab
                        if (Convert.ToBoolean(IsCMGLabObject[0]))
                        {
                            ISession temp = session.GetISession();
                            OrdersManager objOrdersManager = new OrdersManager();
                            objOrdersManager.UpdateCMGEncounterID(EncList[0].Id, Convert.ToUInt32(IsCMGLabObject[1]), IsCMGLabObject[2].ToString(), MACAddress, temp);

                        }
                        //if bResult = false then, the deadlock is occured 
                        if (iResult == 2)
                        {
                            if (iTryCount < 5)
                            {
                                iTryCount++;
                                goto TryAgain;
                            }
                            else
                            {
                                trans.Rollback();
                                //  MySession.Close();
                                throw new Exception("Deadlock occurred. Transaction failed.");
                            }
                        }
                        else if (iResult == 1)
                        {
                            trans.Rollback();
                            //  MySession.Close();
                            throw new Exception("Exception occurred. Transaction failed.");
                        }
                        if (wfObjectRecord != null)
                        {
                            wfObjectRecord.Obj_System_Id = EncList[0].Id;
                            WFObjectManager wfObjectManager = new WFObjectManager();
                            iResult = wfObjectManager.InsertToWorkFlowObject(wfObjectRecord, 1, MACAddress, MySession);

                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {
                                    trans.Rollback();
                                    // MySession.Close();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                // MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                        }
                        if (AuthorizationNo != string.Empty && EncList.Count > 0 && EncList != null)
                        {
                            AuthorizationEncounter objAuthEncounter = new AuthorizationEncounter();
                            objAuthEncounter.Authorization_ID = Convert.ToUInt64(AuthorizationNo);
                            objAuthEncounter.Encounter_ID = EncList[0].Id;
                            objAuthEncounter.Human_ID = EncList[0].Human_ID;
                            objAuthEncounter.Created_By = EncList[0].Created_By;
                            objAuthEncounter.Created_Date_And_Time = EncList[0].Created_Date_and_Time;

                            IList<AuthorizationEncounter> AuthList = new List<AuthorizationEncounter>();
                            AuthList.Add(objAuthEncounter);
                            AuthorizationEncounterManager objAuthEncManager = new AuthorizationEncounterManager();
                            iResult = objAuthEncManager.SaveUpdateDeleteWithoutTransaction(ref AuthList, null, null, MySession, string.Empty);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {

                                    trans.Rollback();
                                    // MySession.Close();
                                    throw new Exception("Deadlock is occured. Transaction failed");

                                }
                            }
                            else if (iResult == 1)
                            {

                                trans.Rollback();
                                //MySession.Close();
                                throw new Exception("Exception is occured. Transaction failed");

                            }
                        }


                        #region Changed to Xml if need to map a physician it to done by creating a physician with that facility_name in find Physician screen
                        //To check MapFacilityPhysician
                        //ICriteria crit = MySession.CreateCriteria(typeof(MapFacilityPhysician)).Add(Expression.Eq("Facility_Name", EncounterRecord.Facility_Name)).Add(Expression.Eq("Phy_Rec_ID", Convert.ToUInt64(EncounterRecord.Appointment_Provider_ID)));
                        //if (crit.List<MapFacilityPhysician>().Count == 0)
                        //{
                        //    MapFacilityPhysician map = new MapFacilityPhysician();
                        //    map.Facility_Name = EncounterRecord.Facility_Name;
                        //    map.Phy_Rec_ID = Convert.ToUInt64(EncounterRecord.Appointment_Provider_ID);
                        //    map.Status = "Y";
                        //    MapFacilityPhysicianManager mapMngr = new MapFacilityPhysicianManager();
                        //    iResult = mapMngr.SaveMapFacilityPhysician(map, MACAddress, MySession);
                        //    if (iResult == 2)
                        //    {
                        //        if (iTryCount < 5)
                        //        {
                        //            iTryCount++;
                        //            goto TryAgain;
                        //        }
                        //        else
                        //        {
                        //            trans.Rollback();
                        //            //  MySession.Close();
                        //            throw new Exception("Deadlock occurred. Transaction failed.");
                        //        }
                        //    }
                        //    else if (iResult == 1)
                        //    {
                        //        trans.Rollback();
                        //        //  MySession.Close();
                        //        throw new Exception("Exception occurred. Transaction failed.");
                        //    }
                        //}
                        #endregion
                        MySession.Flush();
                        trans.Commit();
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        throw new Exception(e.Message);
                    }

                    finally
                    {
                        MySession.Close();
                    }
                }
            }

            catch (Exception ex1)
            {
                //MySession.Close();
                throw new Exception(ex1.Message);
            }
            return EncList[0].Id;
        }
        //public FillAppointment GetAppointmentByDateandPhyRCM(DateTime DOS, ulong[] PhyID, string[] FacName, string sView,out string time_taken)
        //{
        //    FillAppointment FillApptList = new FillAppointment();
        //    ArrayList aryAppointmentList = null;
        //    Encounter objAppointment;
        //    time_taken = "";
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        bool bInvalidDOS = false;
        //        try
        //        {
        //            string tempDate = DOS.AddDays(-2).ToString("yyyy-MM-dd");
        //        }
        //        catch
        //        {
        //            bInvalidDOS = true;
        //        }
        //        if (!bInvalidDOS)
        //        {
        //            //Because, of the universal time - we have to do the between in the where criteria. So, that, it 
        //            //will consider the past and next data also - and, in UI, it will fill up the calendar correctly
        //            //2 Days are added and subtracted - because, only then, the universal time issue is not coming - otherwise, some time appt is 
        //            //not displaying - example, when we click on cancel button in new appt screen.
        //            IQuery query = iMySession.GetNamedQuery("Get.Appointment.DateandPhyName");
        //            //query.SetString(0, DOS.ToString("yyyy-MM-dd") + "%");

        //            if (sView == "SwitchToWeekView")
        //            {
        //                query.SetString(0, DOS.AddDays(-8).ToString("yyyy-MM-dd"));
        //                query.SetString(1, DOS.AddDays(8).ToString("yyyy-MM-dd"));
        //            }
        //            else if (sView == "SwitchToMonthView")
        //            {
        //                query.SetString(0, DOS.AddDays(-31).ToString("yyyy-MM-dd"));
        //                query.SetString(1, DOS.AddDays(31).ToString("yyyy-MM-dd"));
        //            }
        //            else
        //            {
        //                query.SetString(0, DOS.AddDays(-2).ToString("yyyy-MM-dd"));
        //                query.SetString(1, DOS.AddDays(2).ToString("yyyy-MM-dd"));
        //            }
        //            query.SetParameterList("FacList", FacName);
        //            query.SetParameterList("PhyList", PhyID);

        //            Stopwatch LoadApptsDBCall = new Stopwatch();
        //            LoadApptsDBCall.Start();
        //            aryAppointmentList = new ArrayList(query.List());
        //            LoadApptsDBCall.Stop();
        //            time_taken = "LoadAppts DB Call Time : " + LoadApptsDBCall.Elapsed.Seconds + "." + LoadApptsDBCall.Elapsed.Milliseconds + "s. ";
        //        }

        //        //added by Ginu on 2-Aug-2010
        //        if (aryAppointmentList != null)
        //        {
        //            for (int i = 0; i < aryAppointmentList.Count; i++)
        //            {
        //                object[] oj = (object[])aryAppointmentList[i];
        //                objAppointment = new Encounter();
        //                FillApptList.EncounterId.Add(Convert.ToUInt64(oj[0]));
        //                FillApptList.Appointment_Provider_ID.Add(Convert.ToInt32(oj[1]));
        //                FillApptList.Human_ID.Add(Convert.ToUInt64(oj[2]));
        //                FillApptList.Appointment_Date.Add(Convert.ToDateTime(oj[3]));
        //                FillApptList.Duration_Minutes.Add(Convert.ToInt32(oj[4]));
        //                FillApptList.ApptStatus.Add(Convert.ToString(oj[5]));
        //                string sPatientName = oj[6].ToString() + "," + oj[7].ToString() +
        //           "  " + oj[8].ToString() + "  " + oj[10].ToString();
        //                if (oj[11] == null)
        //                {
        //                    oj[11] = string.Empty;
        //                }
        //                if (oj[12] == null)
        //                {
        //                    oj[12] = string.Empty;
        //                }
        //                if (oj[13] == null)
        //                {
        //                    oj[13] = string.Empty;
        //                }
        //                if (oj[14] == null)
        //                {
        //                    oj[14] = string.Empty;

        //                }
        //                if (oj[15] == null)
        //                {
        //                    oj[15] = string.Empty;
        //                }
        //                if (oj[16] != null)
        //                {
        //                    FillApptList.E_Super_Bill.Add(oj[16].ToString());
        //                }
        //                if (oj[17] != null)
        //                {
        //                    FillApptList.Birth_Date.Add(Convert.ToDateTime(oj[17].ToString()));
        //                }
        //                if (oj[18] != null)
        //                {
        //                    FillApptList.Human_Type.Add(oj[18].ToString());
        //                }
        //                //FillApptList.PhysicianName.Add(oj[11].ToString() + " " + oj[12].ToString() + " " + oj[13].ToString() + " " + oj[14].ToString() + " " + oj[15].ToString());
        //                FillApptList.PhysicianName.Add(oj[11].ToString() + " " + oj[12].ToString() + " " + oj[13].ToString() + " " + oj[14].ToString());

        //                FillApptList.PatientName.Add(sPatientName);
        //            }
        //        }

        //        if (FillApptList.EncounterId.Count > 0)
        //        {
        //            CheckManager chkMngr = new CheckManager();
        //            IList<Check> objCheck = new List<Check>();
        //            Stopwatch ChecksDBCall = new Stopwatch();
        //            ChecksDBCall.Start();
        //            objCheck = chkMngr.GetPaymentDetails(FillApptList.EncounterId.ToArray<ulong>());
        //            ChecksDBCall.Stop();
        //            time_taken += "Checks DB Call Time : " + ChecksDBCall.Elapsed.Seconds + "." + ChecksDBCall.Elapsed.Milliseconds + "s. ";



        //            for (int k = 0; k < FillApptList.EncounterId.Count; k++)
        //            {

        //                if (objCheck.Any(b => b.Encounter_ID.ToString() == FillApptList.EncounterId[k].ToString()) == true)
        //                {
        //                    FillApptList.Payment_Paid.Add("Y");
        //                }
        //                else
        //                {
        //                    FillApptList.Payment_Paid.Add("N");
        //                }

        //            }

        //            PatientInsuredPlanManager patMngr = new PatientInsuredPlanManager();
        //            InsurancePlanManager ipMngr = new InsurancePlanManager();
        //            IList<InsurancePlan> objIPList = new List<InsurancePlan>();
        //            IList<InsurancePlan> objIP = new List<InsurancePlan>();
        //            IList<PatientInsuredPlan> objPlanIns = new List<PatientInsuredPlan>();
        //            Stopwatch InsPlanDBCall = new Stopwatch();
        //            InsPlanDBCall.Start();
        //            objPlanIns = patMngr.GetPlanbyHumanID(FillApptList.Human_ID.ToArray<ulong>(), out objIPList);
        //            InsPlanDBCall.Stop();
        //            time_taken += "InsPlan DB Call Time : " + InsPlanDBCall.Elapsed.Seconds + "." + InsPlanDBCall.Elapsed.Milliseconds + "s. "; 
        //            //ulong[] plan_id = null;
        //            //if (objPlanIns.Count > 0)
        //            //    plan_id = objPlanIns.Select(e => e.Insurance_Plan_ID).ToArray<ulong>();
        //            //if (plan_id != null && plan_id.Count() > 0)
        //            //    objIPList = ipMngr.GetInsuranceListbyIDList(plan_id);
        //            string[] sPlanid = { "21", "73", "90", "93", "115" };
        //            for (int l = 0; l < FillApptList.Human_ID.Count; l++)
        //            {
        //                if (objPlanIns.Any(b => b.Human_ID.ToString() == FillApptList.Human_ID[l].ToString()) == true)
        //                {
        //                    ulong ulplanID = objPlanIns.Where(d => d.Human_ID == FillApptList.Human_ID[l]).Select(e => e.Insurance_Plan_ID).ToList<ulong>()[0];
        //                    objIP = objIPList.Where(item => item.Id == ulplanID).ToList<InsurancePlan>();//ipMngr.GetInsurancebyID(ulplanID);
        //                    if (objIP.Count > 0)
        //                    {
        //                        if (sPlanid.Any(f => f.ToString() == objIP[0].Carrier_ID.ToString()) == true)
        //                        {
        //                            FillApptList.Is_Medicare_Plan.Add("Y");
        //                        }
        //                        else
        //                        {
        //                            FillApptList.Is_Medicare_Plan.Add("N");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        FillApptList.Is_Medicare_Plan.Add("N");
        //                    }
        //                }
        //                else
        //                {
        //                    FillApptList.Is_Medicare_Plan.Add("N");
        //                }
        //            }

        //            WFObjectManager wfObjMngr = new WFObjectManager();
        //            IList<WFObject> objWf = new List<WFObject>();
        //            Stopwatch WFObjectDBCall = new Stopwatch();
        //            WFObjectDBCall.Start();
        //            objWf = wfObjMngr.GetDocumentationObject(FillApptList.EncounterId.ToArray<ulong>());
        //            WFObjectDBCall.Stop();
        //            time_taken += "WFObject DB Call Time : " + WFObjectDBCall.Elapsed.Seconds + "." + WFObjectDBCall.Elapsed.Milliseconds + "s. "; 

        //            for (int m = 0; m < FillApptList.EncounterId.Count; m++)
        //            {
        //                if (objWf.Any(w => w.Obj_System_Id.ToString() == FillApptList.EncounterId[m].ToString()) == true)
        //                {
        //                    FillApptList.Document_Type.Add("Y");
        //                }
        //                else
        //                {
        //                    FillApptList.Document_Type.Add("N");
        //                }
        //            }

        //        }




        //        BlockdaysManager blockMngr = new BlockdaysManager();
        //        Stopwatch BlockDaysDBCall = new Stopwatch();
        //        BlockDaysDBCall.Start();
        //        FillApptList.blockList = blockMngr.GetBlockDaysUsingPhyFacApptDt(PhyID, FacName, DOS, sView);
        //        BlockDaysDBCall.Stop();
        //        time_taken += "WFObject DB Call Time : " + BlockDaysDBCall.Elapsed.Seconds + "." + BlockDaysDBCall.Elapsed.Milliseconds + "s. "; 
        //        iMySession.Close();
        //    }
        //    return FillApptList;

        //}

        //public FillAppointment GetAppointmentByDateandPhyRCMFacility(DateTime DOS, ulong[] PhyID, string[] FacName, string sView,out string time_taken)
        //{
        //    FillAppointment FillApptList = new FillAppointment();
        //    ArrayList aryAppointmentList = null;
        //    Encounter objAppointment;
        //    time_taken = "";
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {

        //        //Because, of the universal time - we have to do the between in the where criteria. So, that, it 
        //        //will consider the past and next data also - and, in UI, it will fill up the calendar correctly
        //        //2 Days are added and subtracted - because, only then, the universal time issue is not coming - otherwise, some time appt is 
        //        //not displaying - example, when we click on cancel button in new appt screen.
        //        IQuery query = iMySession.GetNamedQuery("Get.Appointment.DateandPhyNameforFacility");
        //        //query.SetString(0, DOS.ToString("yyyy-MM-dd") + "%");
        //        if (sView == "SwitchToWeekView")
        //        {
        //            query.SetString(0, DOS.AddDays(-8).ToString("yyyy-MM-dd"));
        //            query.SetString(1, DOS.AddDays(8).ToString("yyyy-MM-dd"));
        //        }
        //        else if (sView == "SwitchToMonthView")
        //        {
        //            query.SetString(0, DOS.AddDays(-31).ToString("yyyy-MM-dd"));
        //            query.SetString(1, DOS.AddDays(31).ToString("yyyy-MM-dd"));
        //        }
        //        else
        //        {
        //            query.SetString(0, DOS.AddDays(-2).ToString("yyyy-MM-dd"));
        //            query.SetString(1, DOS.AddDays(2).ToString("yyyy-MM-dd"));
        //        }
        //        query.SetParameterList("PhyList", PhyID);
        //        query.SetParameterList("FacList", FacName);

        //        Stopwatch LoadApptsDBCall = new Stopwatch();
        //        LoadApptsDBCall.Start();
        //        aryAppointmentList = new ArrayList(query.List());
        //        LoadApptsDBCall.Stop();
        //        time_taken = "LoadAppts DB Call Time : " + LoadApptsDBCall.Elapsed.Seconds + "." + LoadApptsDBCall.Elapsed.Milliseconds + "s. ";
        //        //added by Ginu on 2-Aug-2010
        //        if (aryAppointmentList != null)
        //        {
        //            for (int i = 0; i < aryAppointmentList.Count; i++)
        //            {
        //                object[] oj = (object[])aryAppointmentList[i];
        //                objAppointment = new Encounter();
        //                FillApptList.EncounterId.Add(Convert.ToUInt64(oj[0]));
        //                FillApptList.Appointment_Provider_ID.Add(Convert.ToInt32(oj[1]));
        //                FillApptList.Human_ID.Add(Convert.ToUInt64(oj[2]));
        //                FillApptList.Appointment_Date.Add(Convert.ToDateTime(oj[3]));
        //                FillApptList.Duration_Minutes.Add(Convert.ToInt32(oj[4]));
        //                FillApptList.ApptStatus.Add(oj[5].ToString());
        //                string sPatientName = oj[6].ToString() + "," + oj[7].ToString() +
        //           "  " + oj[8].ToString() + "  " + oj[10].ToString();
        //                if (oj[11] == null)
        //                {
        //                    oj[11] = string.Empty;
        //                }
        //                if (oj[12] == null)
        //                {
        //                    oj[12] = string.Empty;
        //                }
        //                if (oj[13] == null)
        //                {
        //                    oj[13] = string.Empty;
        //                }
        //                if (oj[14] == null)
        //                {
        //                    oj[14] = string.Empty;

        //                }
        //                if (oj[15] == null)
        //                {
        //                    oj[15] = string.Empty;
        //                }
        //                if (oj[16] == null)
        //                {
        //                    oj[16] = string.Empty;
        //                }
        //                if (oj[17] != null)
        //                {
        //                    FillApptList.E_Super_Bill.Add(oj[17].ToString());
        //                }
        //                if (oj[18] != null)
        //                {
        //                    FillApptList.Birth_Date.Add(Convert.ToDateTime(oj[18].ToString()));
        //                }
        //                if (oj[19] != null)
        //                {
        //                    FillApptList.Human_Type.Add(oj[19].ToString());
        //                }
        //                //FillApptList.PhysicianName.Add(oj[11].ToString() + " " + oj[12].ToString() + " " + oj[13].ToString() + " " + oj[14].ToString() + " " + oj[15].ToString());
        //                FillApptList.PhysicianName.Add(oj[11].ToString() + " " + oj[12].ToString() + " " + oj[13].ToString() + " " + oj[14].ToString());
        //                FillApptList.FacilityName.Add(oj[16].ToString());
        //                FillApptList.PatientName.Add(sPatientName);
        //            }
        //        }

        //        if (FillApptList.EncounterId.Count > 0)
        //        {
        //            CheckManager chkMngr = new CheckManager();
        //            IList<Check> objCheck = new List<Check>();
        //            Stopwatch ChecksDBCall = new Stopwatch();
        //            ChecksDBCall.Start();
        //            objCheck = chkMngr.GetPaymentDetails(FillApptList.EncounterId.ToArray<ulong>());
        //            ChecksDBCall.Stop();
        //            time_taken += "Checks DB Call Time : " + ChecksDBCall.Elapsed.Seconds + "." + ChecksDBCall.Elapsed.Milliseconds + "s. ";

        //            for (int k = 0; k < FillApptList.EncounterId.Count; k++)
        //            {
        //                if (objCheck.Any(b => b.Encounter_ID.ToString() == FillApptList.EncounterId[k].ToString()) == true)
        //                {
        //                    FillApptList.Payment_Paid.Add("Y");
        //                }
        //                else
        //                {
        //                    FillApptList.Payment_Paid.Add("N");
        //                }
        //            }

        //            WFObjectManager wfObjMngr = new WFObjectManager();
        //            IList<WFObject> objWf = new List<WFObject>();
        //            Stopwatch WFObjectDBCall = new Stopwatch();
        //            WFObjectDBCall.Start();
        //            objWf = wfObjMngr.GetDocumentationObject(FillApptList.EncounterId.ToArray<ulong>());
        //            WFObjectDBCall.Stop();
        //            time_taken += "WFObject DB Call Time : " + WFObjectDBCall.Elapsed.Seconds + "." + WFObjectDBCall.Elapsed.Milliseconds + "s. ";


        //            for (int m = 0; m < FillApptList.EncounterId.Count; m++)
        //            {
        //                if (objWf.Any(w => w.Obj_System_Id.ToString() == FillApptList.EncounterId[m].ToString()) == true)
        //                {
        //                    FillApptList.Document_Type.Add("Y");
        //                }
        //                else
        //                {
        //                    FillApptList.Document_Type.Add("N");
        //                }
        //            }
        //        }

        //        BlockdaysManager blockMngr = new BlockdaysManager();
        //        //bug id:27594
        //        //FillApptList.blockList = blockMngr.GetBlockDaysUsingPhyFacApptDtbyFacility(PhyID, FacName, DOS);
        //        Stopwatch BlockDaysDBCall = new Stopwatch();
        //        BlockDaysDBCall.Start();
        //        FillApptList.blockList = blockMngr.GetBlockDaysUsingPhyFacApptDt(PhyID, FacName, DOS, sView);
        //        BlockDaysDBCall.Stop();
        //        time_taken += "WFObject DB Call Time : " + BlockDaysDBCall.Elapsed.Seconds + "." + BlockDaysDBCall.Elapsed.Milliseconds + "s. ";
        //        iMySession.Close();
        //    }
        //    return FillApptList;

        //}
        public void InsertToWfobjectforElectronicsuperbill(ulong ulMyEncounterID, DateTime currentDate, string FacilityName, string UserName, ulong selectedPhysicianID)
        {
            WFObjectManager objWfMngr = new WFObjectManager();
            WFObject objEncWfObj = objWfMngr.GetByObjectSystemId(ulMyEncounterID, "ENCOUNTER");
            Encounter EncRecord = GetById(ulMyEncounterID);
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {


                if ((objEncWfObj != null && objEncWfObj.Current_Process == "MA_PROCESS"))
                {

                    //Move EncWfObj to 'CHECK_OUT_WAIT'


                    // objWfMngr.MoveToNextProcess(objEncWfObj.Obj_System_Id, objEncWfObj.Obj_Type, 4, "UNKNOWN", currentDate, string.Empty, null, null); bug-41466



                    WFObject WFObj = new WFObject();
                    WFObj.Fac_Name = FacilityName;
                    WFObj.Obj_Type = "DOCUMENTATION";
                    WFObj.Parent_Obj_Type = "ENCOUNTER";
                    WFObj.Parent_Obj_System_Id = ulMyEncounterID;
                    WFObj.Obj_System_Id = ulMyEncounterID;
                    WFObj.Current_Process = "START";
                    WFObj.Current_Arrival_Time = currentDate;
                    string sOwner = string.Empty, sRole = string.Empty;
                    ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", selectedPhysicianID));
                    if (criteria.List<User>() != null && criteria.List<User>().Count > 0)
                    {
                        sRole = criteria.List<User>()[0].role;
                        sOwner = criteria.List<User>()[0].user_name;
                    }
                    iMySession.Close();
                    WFObj.Current_Owner = sOwner;

                    EncRecord.Encounter_Provider_ID = Convert.ToInt32(selectedPhysicianID);
                    EncRecord.Modified_By = UserName;
                    EncRecord.Modified_Date_and_Time = currentDate;
                    if (sRole == "Physician")
                    {
                        EncRecord.Is_Physician_Asst_Process = "N";
                    }
                    else if (sRole == "Physician Assistant")
                    {
                        EncRecord.Is_Physician_Asst_Process = "Y";

                    }
                    UpdateEncounterWithEncounterChild(EncRecord, WFObj, objEncWfObj, string.Empty, 4);
                    EncRecord = GetById(ulMyEncounterID);


                }
                iMySession.Close();
            }


        }
        public void UpdateEncounterForRCM(Encounter EncounterRecord, IList<WFObject> lstWFobj, bool WFFacCheck, string MACAddress, string AuthorizationNo, object[] IsCMGOrders)
        {
            IList<Encounter> EncList = null;
            IList<Encounter> EncListupdate = new List<Encounter>();
            //    IList<AuthorizationEncounter> AuthList = null;
            EncListupdate.Add(EncounterRecord);
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                #region New Added
                iTryCount = 0;

            TryAgain:
                int iResult = 0;
                ISession MySession = Session.GetISession();
                // IList<Encounter> EncList = new List<Encounter>();
                //ITransaction trans = null;
                try
                {
                    using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {

                        try
                        {
                            //trans = MySession.BeginTransaction();
                            GenerateXml XMLObj = null;
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref EncList, ref EncListupdate, null, MySession, MACAddress, false, false, 0, "", ref XMLObj);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {
                                    trans.Rollback();
                                    //  MySession.Close();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                //  MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                            if (WFFacCheck)
                            {
                                IList<WFObject> lstWFobjdum = null;
                                WFObjectManager wfObjectManager = new WFObjectManager();
                                iResult = wfObjectManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref lstWFobjdum, ref lstWFobj, null, MySession, MACAddress, false, false, 0, "", ref XMLObj);

                                if (iResult == 2)
                                {
                                    if (iTryCount < 5)
                                    {
                                        iTryCount++;
                                        goto TryAgain;
                                    }
                                    else
                                    {
                                        trans.Rollback();
                                        // MySession.Close();
                                        throw new Exception("Deadlock occurred. Transaction failed.");
                                    }
                                }
                                else if (iResult == 1)
                                {
                                    trans.Rollback();
                                    // MySession.Close();
                                    throw new Exception("Exception occurred. Transaction failed.");
                                }
                            }

                            //MySession.Flush();
                            trans.Commit();
                        }
                        catch (NHibernate.Exceptions.GenericADOException ex)
                        {
                            trans.Rollback();
                            // MySession.Close();
                            throw new Exception(ex.Message);
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception(e.Message);
                        }

                        finally
                        {
                            MySession.Close();
                        }
                    }
                }

                catch (Exception ex1)
                {
                    //MySession.Close();
                    throw new Exception(ex1.Message);
                }
                //return EncList[0].Id;
                #endregion
                #region Authorization and CMG Lab not been used right now
                //SaveUpdateDeleteWithTransaction(ref EncList, EncListupdate, null, MACAddress);
                //if (AuthorizationNo != string.Empty && EncListupdate != null)
                //{
                //    ICriteria critAuth = iMySession.CreateCriteria(typeof(AuthorizationEncounter)).Add(Expression.Eq("Encounter_ID", EncounterRecord.Id)).Add(Expression.Eq("Human_ID", EncounterRecord.Human_ID)).Add(Expression.Eq("Authorization_ID", Convert.ToUInt64(AuthorizationNo)));
                //    AuthList = critAuth.List<AuthorizationEncounter>();
                //    AuthorizationEncounter objAuthEncounter = new AuthorizationEncounter();
                //    if (AuthList == null && AuthList.Count == 0)
                //    {
                //        objAuthEncounter.Authorization_ID = Convert.ToUInt64(AuthorizationNo);
                //        objAuthEncounter.Encounter_ID = EncListupdate[0].Id;
                //        objAuthEncounter.Human_ID = EncListupdate[0].Human_ID;
                //        objAuthEncounter.Created_By = EncListupdate[0].Created_By;
                //        objAuthEncounter.Created_Date_And_Time = EncListupdate[0].Created_Date_and_Time;
                //        IList<AuthorizationEncounter> AuthSaveList = new List<AuthorizationEncounter>();
                //        AuthSaveList.Add(objAuthEncounter);
                //        AuthorizationEncounterManager objAuthEncManager = new AuthorizationEncounterManager();
                //        objAuthEncManager.SaveUpdateDeleteWithTransaction(ref AuthSaveList, null, null, string.Empty);
                //    }
                //}


                ////Added for CMG Lab
                //if (Convert.ToBoolean(IsCMGOrders[0]))
                //{
                //    ISession MySecondSession = session.GetISession();
                //    ITransaction trans = MySecondSession.BeginTransaction();
                //    OrdersManager objOrdersManager = new OrdersManager();
                //    objOrdersManager.UpdateCMGEncounterID(EncListupdate[0].Id, Convert.ToUInt32(IsCMGOrders[1]), IsCMGOrders[2].ToString(), MACAddress, MySecondSession);
                //    MySecondSession.Flush();
                //    trans.Commit();
                //}
                #endregion
                #region Changed to Xml if need to map a physician it to done by creating a physician with that facility_name in find Physician screen

                //ICriteria crit = iMySession.CreateCriteria(typeof(MapFacilityPhysician)).Add(Expression.Eq("Facility_Name", EncounterRecord.Facility_Name)).Add(Expression.Eq("Phy_Rec_ID", Convert.ToUInt64(EncounterRecord.Appointment_Provider_ID)));
                //if (crit.List<MapFacilityPhysician>().Count == 0)
                //{
                //    ISession MySession = Session.GetISession();
                //    ITransaction trans = MySession.BeginTransaction();
                //    MapFacilityPhysician map = new MapFacilityPhysician();
                //    map.Facility_Name = EncounterRecord.Facility_Name;
                //    map.Phy_Rec_ID = Convert.ToUInt64(EncounterRecord.Appointment_Provider_ID);
                //    map.Status = "Y";
                //    MapFacilityPhysicianManager mapMngr = new MapFacilityPhysicianManager();
                //    mapMngr.SaveMapFacilityPhysician(map, MACAddress, MySession);
                //    MySession.Flush();
                //    trans.Commit();
                //}
                #endregion
                iMySession.Close();
            }
        }
        //public IList<ElectronicsuperbillQDTO> GetElectronicSuperBillQ(string sUserName)
        //{


        //    IList<ElectronicsuperbillQDTO> objESuperBillQ = new List<ElectronicsuperbillQDTO>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query1 = iMySession.GetNamedQuery("Get.ElectronicSuperBillQ");
        //        query1.SetString(0, sUserName);
        //        ArrayList FavoriteList = new ArrayList(query1.List());

        //        for (int i = 0; i < FavoriteList.Count; i++)
        //        {
        //            ElectronicsuperbillQDTO obj = new ElectronicsuperbillQDTO();
        //            object[] Fillreport = (object[])FavoriteList[i];
        //            obj.Encounter_ID = Convert.ToUInt64(Fillreport[0].ToString());
        //            obj.Appointment_Date = Convert.ToDateTime(Fillreport[1].ToString());
        //            obj.Medical_Record_Number = Fillreport[2].ToString();
        //            obj.Human_Id = Convert.ToUInt64(Fillreport[3].ToString());
        //            obj.ExternalaccountNumber = Fillreport[4].ToString();
        //            obj.Patient_name = Fillreport[5].ToString();
        //            obj.Birth_Date = Convert.ToDateTime(Fillreport[6].ToString());
        //            obj.Type_Of_Visit = Fillreport[7].ToString();
        //            obj.Facility_Name = Fillreport[8].ToString();
        //            obj.Appointment_Physician_Name = Fillreport[9].ToString();
        //            obj.Current_Process = Fillreport[10].ToString();
        //            obj.Physician_ID = Fillreport[11].ToString();
        //            objESuperBillQ.Add(obj);
        //        }
        //        iMySession.Close();
        //    }
        //    return objESuperBillQ;
        //}

        public IList<Encounter> GetEncounterByEncAndHumanID(ulong ulEncounterID, ulong ulHumanID)
        {

            IList<Encounter> EncounterList = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", ulEncounterID)).Add(Expression.Eq("Human_ID", ulHumanID));
                EncounterList = crit.List<Encounter>();
                iMySession.Close();
            }
            return EncounterList;
        }
        public IList<User> GetUsers()
        {
            IList<User> ilstUser = new List<User>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ISQLQuery sql = iMySession.CreateSQLQuery("Select u.* from user u where u.status='A'").AddEntity("u", typeof(User));
                ilstUser = sql.List<User>();
                iMySession.Close();
            }
            return ilstUser;
        }
        public IList<FillPrintRecipt> FillPrintRecipt(ulong ulEncounterID, ulong ulHumanID, Boolean bIsEncounterBased)
        {
            IList<FillPrintRecipt> lstFillPrintRecipt = new List<FillPrintRecipt>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                string sQuery = string.Empty;
                if (bIsEncounterBased == true)
                    sQuery = "Get.PrintReceiptDetails.EncounterBased";
                else
                    sQuery = "Get.PrintReceiptDetails.VoucherBased";
                IQuery query1 = iMySession.GetNamedQuery(sQuery);
                query1.SetString(0, ulHumanID.ToString());
                query1.SetString(1, ulEncounterID.ToString());
                query1.SetString(2, ulHumanID.ToString());
                query1.SetString(3, ulEncounterID.ToString());
                ArrayList FavoriteList = new ArrayList(query1.List());
                for (int i = 0; i < FavoriteList.Count; i++)
                {
                    FillPrintRecipt obj = new FillPrintRecipt();
                    object[] Fillreport = (object[])FavoriteList[i];
                    if (Fillreport[0] != null)
                    {
                        obj.Provider_Name = Fillreport[0].ToString();
                    }
                    if (Fillreport[1] != null)
                    {
                        obj.Facility_Name = Fillreport[1].ToString();
                    }
                    if (Fillreport[2].ToString() != null && Fillreport[2].ToString() != string.Empty)
                    {
                        obj.Appointment_Date = Convert.ToDateTime(Fillreport[2].ToString());
                    }
                    if (Fillreport[3] != null)
                    {
                        obj.Last_Name = Fillreport[3].ToString();
                    }
                    if (Fillreport[4] != null)
                    {
                        obj.Suffix = Fillreport[4].ToString();
                    }
                    if (Fillreport[5] != null)
                    {
                        obj.First_Name = Fillreport[5].ToString();
                    }
                    if (Fillreport[6] != null)
                    {
                        obj.MI = Fillreport[6].ToString();
                    }
                    if (Fillreport[7] != null)
                    {
                        obj.Birth_Date = Convert.ToDateTime(Fillreport[7].ToString());
                    }
                    if (Fillreport[8] != null)
                    {
                        obj.Street_Address = Fillreport[8].ToString();
                    }
                    if (Fillreport[9] != null)
                    {
                        obj.City = Fillreport[9].ToString();
                    }
                    if (Fillreport[10] != null)
                    {
                        obj.State = Fillreport[10].ToString();
                    }
                    if (Fillreport[11] != null)
                    {
                        obj.Zipcode = Fillreport[11].ToString();
                    }
                    if (Fillreport[12] != null)
                    {
                        obj.Human_ID = Convert.ToUInt64(Fillreport[12].ToString());
                    }
                    //if (Fillreport[13] != null)
                    //    obj.Insurance_Plan_Name = Fillreport[13].ToString();
                    //if (Fillreport[14] != null)
                    //    obj.Policy_Holder_ID = Fillreport[14].ToString();
                    obj.Encounter_ID = Convert.ToUInt64(Fillreport[13].ToString());
                    //if (Fillreport[14] != null)
                    //{
                    //    obj.PCP_Copay = Fillreport[14].ToString();
                    //}
                    //if (Fillreport[15] != null)
                    //    obj.SPC_Copay = Fillreport[15].ToString();
                    //if (Fillreport[16] != null)
                    //    obj.Total_Deduction = Fillreport[16].ToString();
                    //if (Fillreport[17] != null)
                    //    obj.Deduction_Met ="0";
                    //if (Fillreport[17] != null)
                    //    obj.Coinsurance = Fillreport[17].ToString();
                    if (Fillreport[14] != null)
                        obj.PastDue = Fillreport[14].ToString();
                    if (Fillreport[15] != null)
                        obj.Payment_Method = Fillreport[15].ToString();
                    if (Fillreport[16] != null)
                        obj.Check_Card_No = Fillreport[16].ToString();
                    if (Fillreport[17] != null)
                        obj.Payment_Amount = Fillreport[17].ToString();
                    if (Fillreport[18] != null)
                        obj.Payment_Note = Fillreport[18].ToString();
                    if (Fillreport[19] != null)
                        obj.Payment_Date = Convert.ToDateTime(Fillreport[19].ToString());
                    if (Fillreport[20] != null)
                        obj.Refund_Amount = Fillreport[20].ToString();
                    if (Fillreport[21] != null)
                        obj.Rec_On_Acc = Fillreport[21].ToString();
                    if (Fillreport[22] != null)
                    {
                        obj.Amount_Paid_By = Fillreport[22].ToString();
                    }
                    if (Fillreport[23] != null)
                    {
                        obj.Created_By = Fillreport[23].ToString();
                    }
                    if (Fillreport[24] != null && Fillreport[24].ToString() != string.Empty)
                    {
                        obj.Date_of_service = Convert.ToDateTime(Fillreport[24].ToString());
                    }
                    if (Fillreport[26] != null)
                    {
                        obj.Patient_Account_External = Fillreport[26].ToString();
                    }


                    lstFillPrintRecipt.Add(obj);
                }
                HumanDTO objhuman = new HumanDTO();
                HumanManager humanMngr = new HumanManager();
                objhuman = humanMngr.GetHumanInuranceAndCarrierDetails(ulHumanID);
                if (objhuman != null)
                {
                    if (lstFillPrintRecipt.Count > 0)
                    {
                        for (int j = 0; j < lstFillPrintRecipt.Count; j++)
                        {
                            if (objhuman.InsuranceDetails.Ins_Plan_Name != string.Empty)
                            {
                                lstFillPrintRecipt[j].Insurance_Plan_Name = objhuman.InsuranceDetails.Ins_Plan_Name;
                                lstFillPrintRecipt[j].Policy_Holder_ID = objhuman.HumanDetails.PatientInsuredBag.Where(a => a.Insurance_Type == "PRIMARY").ToList<PatientInsuredPlan>()[0].Policy_Holder_ID;
                            }
                        }
                    }
                }
                PatientInsuredPlanManager patinsMngr = new PatientInsuredPlanManager();
                IList<PatientInsuredPlan> objPatInsPlan = new List<PatientInsuredPlan>();
                objPatInsPlan = patinsMngr.GetActiveInsurancePoliciesByHumanId(ulHumanID, "PRIMARY");
                if (objPatInsPlan.Count > 0)
                {
                    for (int k = 0; k < lstFillPrintRecipt.Count; k++)
                    {
                        lstFillPrintRecipt[k].PCP_Copay = objPatInsPlan[0].PCP_Copay.ToString();
                        lstFillPrintRecipt[k].SPC_Copay = objPatInsPlan[0].Specialist_Copay.ToString();
                        lstFillPrintRecipt[k].Total_Deduction = objPatInsPlan[0].Deductible.ToString();
                        lstFillPrintRecipt[k].Deduction_Met = "";
                        lstFillPrintRecipt[k].Coinsurance = objPatInsPlan[0].Co_Insurance.ToString();

                    }

                }
                else
                {
                    for (int k = 0; k < lstFillPrintRecipt.Count; k++)
                    {
                        lstFillPrintRecipt[k].PCP_Copay = "0.00";
                        lstFillPrintRecipt[k].SPC_Copay = "0.00";
                        lstFillPrintRecipt[k].Total_Deduction = "0.00";
                        lstFillPrintRecipt[k].Deduction_Met = "";
                        lstFillPrintRecipt[k].Coinsurance = "0.00";

                    }
                }
                iMySession.Close();
            }
            return lstFillPrintRecipt;
        }
        public IList<Encounter> GetEncounterByEncounterIDforPaperSuperBill(ulong ulhuman_id)
        {
            IList<Encounter> finalEnclst = new List<Encounter>();
            IList<Encounter> lstenc = new List<Encounter>();
            IList<WFObject> lstwfobj = new List<WFObject>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", ulhuman_id)).Add(Expression.Eq("Encounter_Mode", "PAPER SUPERBILL"));
                lstenc = crit.List<Encounter>();

                if (lstenc.Count > 0)
                {
                    for (int i = 0; i < lstenc.Count; i++)
                    {
                        ICriteria crite = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", lstenc[i].Id)).Add(Expression.Eq("Obj_Type", "DOCUMENTATION")).Add(Expression.Eq("Current_Process", "ATTACH_SUPERBILL"));
                        lstwfobj = crite.List<WFObject>();
                        if (lstwfobj.Count > 0)
                            finalEnclst.Add(lstenc[i]);
                    }
                }
                iMySession.Close();
            }
            return finalEnclst;
        }
        public void InsertToWfobjectforPrintsuperbill(ulong ulMyEncounterID, DateTime currentDate, string FacilityName, string UserName, ulong selectedPhysicianID)
        {
            WFObjectManager objWfMngr = new WFObjectManager();
            WFObject objEncWfObj = objWfMngr.GetByObjectSystemId(ulMyEncounterID, "ENCOUNTER");
            Encounter EncRecord = GetById(ulMyEncounterID);
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {


                if ((objEncWfObj != null))
                {

                    //Move EncWfObj to 'CHECK_OUT_WAIT'


                    //objWfMngr.MoveToNextProcess(objEncWfObj.Obj_System_Id, objEncWfObj.Obj_Type, 5, "UNKNOWN", currentDate, string.Empty, null, null);



                    WFObject WFObj = new WFObject();
                    WFObj.Fac_Name = FacilityName;
                    WFObj.Obj_Type = "DOCUMENTATION";
                    WFObj.Parent_Obj_Type = "ENCOUNTER";
                    WFObj.Parent_Obj_System_Id = ulMyEncounterID;
                    WFObj.Obj_System_Id = ulMyEncounterID;
                    WFObj.Current_Process = "START";
                    WFObj.Current_Arrival_Time = currentDate;
                    string sOwner = string.Empty, sRole = string.Empty;
                    ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", selectedPhysicianID));
                    if (criteria.List<User>() != null && criteria.List<User>().Count > 0)
                    {
                        sRole = criteria.List<User>()[0].role;
                        sOwner = criteria.List<User>()[0].user_name;
                    }
                    iMySession.Close();
                    WFObj.Current_Owner = sOwner;

                    EncRecord.Encounter_Provider_ID = Convert.ToInt32(selectedPhysicianID);
                    EncRecord.Modified_By = UserName;
                    EncRecord.Modified_Date_and_Time = currentDate;
                    if (sRole == "Physician")
                    {
                        EncRecord.Is_Physician_Asst_Process = "N";
                    }
                    else if (sRole == "Physician Assistant")
                    {
                        EncRecord.Is_Physician_Asst_Process = "Y";

                    }
                    UpdateEncounterWithEncounterChild(EncRecord, WFObj, objEncWfObj, string.Empty, 5);
                    EncRecord = GetById(ulMyEncounterID);


                }
                iMySession.Close();
            }

        }

        public SuperBillDTO GetEncounterDetailsforPaperSuperBill(ulong ulEncounterID)
        {
            SuperBillDTO superbillDTO = new SuperBillDTO();
            IList<PatientInsuredPlan> PrimPatInsPlanList = new List<PatientInsuredPlan>();
            IList<PatientInsuredPlan> SecPatInsPlanList = new List<PatientInsuredPlan>();
            IList<InsurancePlan> PriInsPlanList = new List<InsurancePlan>();
            IList<InsurancePlan> SecInsPlanList = new List<InsurancePlan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ISQLQuery sql = iMySession.CreateSQLQuery("select {e.*},{h.*} from encounter e,human h where h.human_id=e.human_id and e.encounter_id=" + ulEncounterID).AddEntity("e", typeof(Encounter)).AddEntity("h", typeof(Human));

                foreach (IList<Object> l in sql.List())
                {
                    Encounter TempEncounter = (Encounter)l[0];
                    Human TempHuman = (Human)l[1];

                    superbillDTO.Human_ID = TempEncounter.Human_ID;
                    superbillDTO.Patient_Name = TempHuman.Last_Name + " ," + TempHuman.First_Name + " ," + TempHuman.MI;
                    superbillDTO.Patient_DOB = TempHuman.Birth_Date.ToString("dd-MMM-yyyy");
                    superbillDTO.Patient_Sex = TempHuman.Sex;
                    superbillDTO.Patient_Address = TempHuman.Street_Address1;
                    superbillDTO.Patient_City = TempHuman.City;
                    superbillDTO.Patient_State = TempHuman.State;
                    superbillDTO.Patient_ZipCode = TempHuman.ZipCode;
                    superbillDTO.Patient_HomePhoneNo = TempHuman.Home_Phone_No;
                    superbillDTO.SSN = TempHuman.SSN;
                    superbillDTO.Referring_Prov_Name = TempEncounter.Referring_Physician;
                    superbillDTO.Account_Type = TempHuman.Human_Type;
                    superbillDTO.Appt_Prov_ID = TempEncounter.Appointment_Provider_ID.ToString();

                    IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();

                    ICriteria criteria3 = iMySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("Id", Convert.ToUInt64(TempEncounter.Appointment_Provider_ID)));
                    PhyList = criteria3.List<PhysicianLibrary>();

                    if (PhyList.Count > 0)
                    {
                        superbillDTO.Appt_Prov_Name = PhyList[0].PhyPrefix + " " + PhyList[0].PhyFirstName + " " + PhyList[0].PhyMiddleName + " " + PhyList[0].PhyLastName + " " + PhyList[0].PhySuffix;
                    }

                    superbillDTO.Appt_Date = TempEncounter.Appointment_Date.ToString("dd-MMM-yyyy");
                    superbillDTO.Appt_Time = TempEncounter.Appointment_Date.ToLocalTime().ToString("hh:mm tt");
                    superbillDTO.Appt_Facility = TempEncounter.Facility_Name;
                    superbillDTO.App_Length = TempEncounter.Duration_Minutes.ToString();
                    superbillDTO.Reason = TempEncounter.Visit_Type;
                    superbillDTO.Voucher_No = TempEncounter.Id;

                    ICriteria criteria = iMySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Human_ID", TempEncounter.Human_ID)).Add(Expression.Eq("Insurance_Type", "PRIMARY"));
                    PrimPatInsPlanList = criteria.List<PatientInsuredPlan>();

                    if (PrimPatInsPlanList.Count > 0)
                    {
                        ICriteria crit = iMySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Id", PrimPatInsPlanList[0].Insurance_Plan_ID));
                        PriInsPlanList = crit.List<InsurancePlan>();
                        if (PriInsPlanList.Count > 0)
                        {
                            superbillDTO.Pri_Ins_Name = PriInsPlanList[0].Ins_Plan_Name;
                            superbillDTO.Pri_Copay = Convert.ToDecimal(PrimPatInsPlanList[0].PCP_Copay);
                            superbillDTO.Pri_Member_ID = PrimPatInsPlanList[0].Policy_Holder_ID;
                        }
                    }

                    ICriteria criteria1 = iMySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Human_ID", TempEncounter.Human_ID)).Add(Expression.Eq("Insurance_Type", "SECONDARY"));
                    SecPatInsPlanList = criteria1.List<PatientInsuredPlan>();

                    if (SecPatInsPlanList.Count > 0)
                    {
                        ICriteria crit1 = iMySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Id", SecPatInsPlanList[0].Insurance_Plan_ID));
                        SecInsPlanList = crit1.List<InsurancePlan>();
                        if (SecInsPlanList.Count > 0)
                        {
                            superbillDTO.Sec_Ins_Name = SecInsPlanList[0].Ins_Plan_Name;
                            superbillDTO.Sec_Copay = Convert.ToDecimal(SecPatInsPlanList[0].PCP_Copay);
                            superbillDTO.Sec_Member_ID = SecPatInsPlanList[0].Policy_Holder_ID;
                        }
                    }

                    ArrayList aryCharge = null;
                    IQuery query1 = iMySession.GetNamedQuery("Get.PatientBalance");
                    query1.SetString(0, TempHuman.Id.ToString());
                    aryCharge = new ArrayList(query1.List());
                    superbillDTO.Acc_Balance = Convert.ToInt32(aryCharge[0]);

                    aryCharge.Clear();
                    query1 = iMySession.GetNamedQuery("Get.PatientDue");
                    query1.SetString(0, TempHuman.Id.ToString());
                    aryCharge = new ArrayList(query1.List());
                    superbillDTO.Patient_Due = Convert.ToInt32(aryCharge[0]);

                    ISQLQuery sql1 = iMySession.CreateSQLQuery("SELECT a.*,b.* FROM authorization_encounter a,authorization b where a.encounter_id=" + TempEncounter.Id + " and a.human_id=" + TempEncounter.Human_ID + " and a.is_active='Y' and a.authorization_id=b.authorization_id and a.human_id=b.human_id;")
                  .AddEntity("a", typeof(AuthorizationEncounter)).AddEntity("b", typeof(Authorization));

                    if (sql1.List().Count > 0)
                    {
                        foreach (IList<Object> g in sql1.List())
                        {
                            //superbillDTO.Auth_No = ((Authorization)g[1]).Auth_No;
                        }
                    }
                }
                iMySession.Close();
            }
            return superbillDTO;
        }

        public IList<SuperBillDTO> GetMultipleEncounterDetailsforPaperSuperBill(string sFacilityName, string sApptDate, ulong ulApptProvID)
        {
            IList<SuperBillDTO> SuperBillDTOList = new List<SuperBillDTO>();
            SuperBillDTO superbillDTO = new SuperBillDTO();
            IList<PatientInsuredPlan> PrimPatInsPlanList = new List<PatientInsuredPlan>();
            IList<PatientInsuredPlan> SecPatInsPlanList = new List<PatientInsuredPlan>();
            IList<InsurancePlan> PriInsPlanList = new List<InsurancePlan>();
            IList<InsurancePlan> SecInsPlanList = new List<InsurancePlan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ISQLQuery sql = iMySession.CreateSQLQuery("select {e.*},{h.*} from encounter e,human h where h.human_id=e.human_id and e.Facility_Name= '" + sFacilityName + "' and e.Appointment_Provider_ID='" + ulApptProvID + "' and e.Appointment_Date like '" + sApptDate + "%'").AddEntity("e", typeof(Encounter)).AddEntity("h", typeof(Human));

                foreach (IList<Object> l in sql.List())
                {
                    superbillDTO = new SuperBillDTO();
                    Encounter TempEncounter = (Encounter)l[0];
                    Human TempHuman = (Human)l[1];

                    superbillDTO.Human_ID = TempEncounter.Human_ID;
                    superbillDTO.Patient_Name = TempHuman.Last_Name + " ," + TempHuman.First_Name + " ," + TempHuman.MI;
                    superbillDTO.Patient_DOB = TempHuman.Birth_Date.ToString("dd-MMM-yyyy");
                    superbillDTO.Patient_Sex = TempHuman.Sex;
                    superbillDTO.Patient_Address = TempHuman.Street_Address1;
                    superbillDTO.Patient_City = TempHuman.City;
                    superbillDTO.Patient_State = TempHuman.State;
                    superbillDTO.Patient_ZipCode = TempHuman.ZipCode;
                    superbillDTO.Patient_HomePhoneNo = TempHuman.Home_Phone_No;
                    superbillDTO.SSN = TempHuman.SSN;
                    superbillDTO.Referring_Prov_Name = TempEncounter.Referring_Physician;
                    superbillDTO.Account_Type = TempHuman.Human_Type;
                    superbillDTO.Appt_Prov_ID = TempEncounter.Appointment_Provider_ID.ToString();

                    IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();

                    ICriteria criteria3 = iMySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("Id", Convert.ToUInt64(TempEncounter.Appointment_Provider_ID)));
                    PhyList = criteria3.List<PhysicianLibrary>();

                    if (PhyList.Count > 0)
                    {
                        superbillDTO.Appt_Prov_Name = PhyList[0].PhyPrefix + " " + PhyList[0].PhyFirstName + " " + PhyList[0].PhyMiddleName + " " + PhyList[0].PhyLastName + " " + PhyList[0].PhySuffix;
                    }

                    superbillDTO.Appt_Date = TempEncounter.Appointment_Date.ToString("dd-MMM-yyyy");
                    superbillDTO.Appt_Time = TempEncounter.Appointment_Date.ToLocalTime().ToString("hh:mm tt");
                    superbillDTO.Appt_Facility = TempEncounter.Facility_Name;
                    superbillDTO.App_Length = TempEncounter.Duration_Minutes.ToString();
                    superbillDTO.Reason = TempEncounter.Visit_Type;
                    superbillDTO.Voucher_No = TempEncounter.Id;

                    ICriteria criteria = iMySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Human_ID", TempEncounter.Human_ID)).Add(Expression.Eq("Insurance_Type", "PRIMARY"));
                    PrimPatInsPlanList = criteria.List<PatientInsuredPlan>();

                    if (PrimPatInsPlanList.Count > 0)
                    {
                        ICriteria crit = iMySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Id", PrimPatInsPlanList[0].Insurance_Plan_ID));
                        PriInsPlanList = crit.List<InsurancePlan>();
                        if (PriInsPlanList.Count > 0)
                        {
                            superbillDTO.Pri_Ins_Name = PriInsPlanList[0].Ins_Plan_Name;
                            superbillDTO.Pri_Copay = Convert.ToDecimal(PrimPatInsPlanList[0].PCP_Copay);
                            superbillDTO.Pri_Member_ID = PrimPatInsPlanList[0].Policy_Holder_ID;
                        }
                    }

                    ICriteria criteria1 = iMySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Human_ID", TempEncounter.Human_ID)).Add(Expression.Eq("Insurance_Type", "SECONDARY"));
                    SecPatInsPlanList = criteria1.List<PatientInsuredPlan>();

                    if (SecPatInsPlanList.Count > 0)
                    {
                        ICriteria crit1 = iMySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Id", SecPatInsPlanList[0].Insurance_Plan_ID));
                        SecInsPlanList = crit1.List<InsurancePlan>();
                        if (SecInsPlanList.Count > 0)
                        {
                            superbillDTO.Sec_Ins_Name = SecInsPlanList[0].Ins_Plan_Name;
                            superbillDTO.Sec_Copay = Convert.ToDecimal(SecPatInsPlanList[0].PCP_Copay);
                            superbillDTO.Sec_Member_ID = SecPatInsPlanList[0].Policy_Holder_ID;
                        }
                    }

                    ArrayList aryCharge = null;
                    IQuery query1 = iMySession.GetNamedQuery("Get.PatientBalance");
                    query1.SetString(0, TempHuman.Id.ToString());
                    aryCharge = new ArrayList(query1.List());
                    superbillDTO.Acc_Balance = Convert.ToInt32(aryCharge[0]);

                    aryCharge.Clear();
                    query1 = iMySession.GetNamedQuery("Get.PatientDue");
                    query1.SetString(0, TempHuman.Id.ToString());
                    aryCharge = new ArrayList(query1.List());
                    superbillDTO.Patient_Due = Convert.ToInt32(aryCharge[0]);

                    ISQLQuery sql1 = iMySession.CreateSQLQuery("SELECT a.*,b.* FROM authorization_encounter a,authorization b where a.encounter_id=" + TempEncounter.Id + " and a.human_id=" + TempEncounter.Human_ID + " and a.is_active='Y' and a.authorization_id=b.authorization_id and a.human_id=b.human_id;")
                  .AddEntity("a", typeof(AuthorizationEncounter)).AddEntity("b", typeof(Authorization));

                    if (sql1.List().Count > 0)
                    {
                        foreach (IList<Object> g in sql1.List())
                        {
                            //superbillDTO.Auth_No = ((Authorization)g[1]).Auth_No;
                        }
                    }

                    SuperBillDTOList.Add(superbillDTO);
                }
                iMySession.Close();
            }
            return SuperBillDTOList;
        }

        public IList<Encounter> GetEncounterByEncounterIDforPS(ulong ulhuman_id, ulong ulEncounterID)
        {
            IList<Encounter> finalEnclst = new List<Encounter>();
            IList<Encounter> lstenc = new List<Encounter>();
            IList<WFObject> lstwfobj = new List<WFObject>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", ulhuman_id)).Add(Expression.Eq("Id", ulEncounterID)).Add(Expression.Eq("Encounter_Mode", "PAPER SUPERBILL"));
                lstenc = crit.List<Encounter>();

                if (lstenc.Count > 0)
                {
                    for (int i = 0; i < lstenc.Count; i++)
                    {
                        ICriteria crite = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", lstenc[i].Id)).Add(Expression.Eq("Obj_Type", "DOCUMENTATION")).Add(Expression.Eq("Current_Process", "ATTACH_SUPERBILL"));
                        lstwfobj = crite.List<WFObject>();
                        if (lstwfobj.Count > 0)
                            finalEnclst.Add(lstenc[i]);
                    }
                }
                iMySession.Close();
            }
            return finalEnclst;
        }
        public IList<FillAppointmentSlot> GetEncounterForAppointmentSlot(string sFacilityname, ulong ulPhysicianID, DateTime sDateofService, string sNoofDays)
        {
            IList<FillAppointmentSlot> objAppSlot = new List<FillAppointmentSlot>();
            ArrayList AppointmentList;
            IList<WFObject> Wfobj = new List<WFObject>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {


                ICriteria crit = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Fac_Name", sFacilityname)).Add(Expression.Eq("Obj_Type", "ENCOUNTER"));
                Wfobj = crit.List<WFObject>();

                if (sNoofDays == "1")
                {
                    IQuery query1 = iMySession.GetNamedQuery("Get.AppointmentSlotDetailsday");
                    query1.SetString(0, sFacilityname);
                    query1.SetString(1, ulPhysicianID.ToString());
                    query1.SetString(2, "%" + sDateofService.ToString("yyyy-MM-dd") + "%");
                    AppointmentList = new ArrayList(query1.List());
                    for (int i = 0; i < AppointmentList.Count; i++)
                    {
                        FillAppointmentSlot obj = new FillAppointmentSlot();
                        object[] FillAppointment = (object[])AppointmentList[i];
                        string scurrentprocess = Wfobj.Where(a => a.Obj_System_Id == Convert.ToUInt64(FillAppointment[3].ToString())).Select(b => b.Current_Process).ToList<string>()[0];
                        if (scurrentprocess != "RESCHEDULED")
                        {
                            obj.HumanId.Add(Convert.ToUInt64(FillAppointment[0].ToString()));
                            obj.AppointmentDate.Add(Convert.ToDateTime(FillAppointment[1].ToString()));
                            obj.AppointmentTime.Add(FillAppointment[2].ToString());
                            objAppSlot.Add(obj);
                        }
                    }

                }
                else
                {
                    IQuery query1 = iMySession.GetNamedQuery("Get.AppointmentSlotDetails");
                    query1.SetString(0, sFacilityname);
                    query1.SetString(1, ulPhysicianID.ToString());
                    query1.SetString(2, sDateofService.ToString("yyyy-MM-dd"));
                    query1.SetString(3, sDateofService.AddDays(Convert.ToDouble(sNoofDays) - 1).ToString("yyyy-MM-dd"));
                    AppointmentList = new ArrayList(query1.List());
                    for (int i = 0; i < AppointmentList.Count; i++)
                    {
                        FillAppointmentSlot obj = new FillAppointmentSlot();
                        object[] FillAppointment = (object[])AppointmentList[i];
                        if (Wfobj.Any(a => a.Obj_System_Id == Convert.ToUInt64(FillAppointment[3].ToString())))
                        {
                            string scurrentprocess = Wfobj.Where(a => a.Obj_System_Id == Convert.ToUInt64(FillAppointment[3].ToString())).Select(b => b.Current_Process).ToList<string>()[0];
                            if (scurrentprocess != "RESCHEDULED")
                            {
                                obj.HumanId.Add(Convert.ToUInt64(FillAppointment[0].ToString()));
                                obj.AppointmentDate.Add(Convert.ToDateTime(FillAppointment[1].ToString()));
                                obj.AppointmentTime.Add(FillAppointment[2].ToString());
                                objAppSlot.Add(obj);
                            }
                        }
                    }
                }
                iMySession.Close();
            }
            return objAppSlot;
        }
        public IList<FillWillingonCancel> GetWillingCancellationList()
        {
            IList<FillWillingonCancel> objFillWillingonCancel = new List<FillWillingonCancel>();
            ArrayList WillingCancellationList;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                IQuery query1 = iMySession.GetNamedQuery("Get.WillingoncancellationDetails");
                WillingCancellationList = new ArrayList(query1.List());
                for (int i = 0; i < WillingCancellationList.Count; i++)
                {
                    FillWillingonCancel obj = new FillWillingonCancel();
                    object[] FillWillingonCancel = (object[])WillingCancellationList[i];
                    obj.Appointment_Date_Time = Convert.ToDateTime(FillWillingonCancel[0].ToString());
                    obj.Facility_Name = FillWillingonCancel[1].ToString();
                    obj.Physician_Name = FillWillingonCancel[2].ToString();
                    obj.Human_ID = Convert.ToUInt64(FillWillingonCancel[3].ToString());
                    obj.Visit_Type = FillWillingonCancel[4].ToString();
                    obj.Last_Name = FillWillingonCancel[5].ToString();
                    obj.First_Name = FillWillingonCancel[6].ToString();
                    obj.Mi = FillWillingonCancel[7].ToString();
                    obj.Suffix = FillWillingonCancel[8].ToString();
                    obj.Sex = FillWillingonCancel[9].ToString();
                    obj.Human_Type = FillWillingonCancel[10].ToString();
                    obj.Current_Process = FillWillingonCancel[11].ToString();
                    obj.Encounter_ID = Convert.ToUInt64(FillWillingonCancel[12].ToString());
                    obj.Physician_ID = Convert.ToUInt64(FillWillingonCancel[13].ToString());
                    obj.Duration_Time = FillWillingonCancel[14].ToString();
                    objFillWillingonCancel.Add(obj);
                }
                iMySession.Close();
            }
            return objFillWillingonCancel;
        }




        public string GetNotification(ulong ulEncounterId, ulong ulHumanID, string UserName, String ScreenName, IList<CDSRuleMaster> ClinicalList, IList<Notification> NotificationList, IList<UserLookup> UserLookupList, DateTime Current_dt, string sUserRole, string sPhysicianSpecialty)//, string sEncounteProviderName)
        {
            NotificationCount = 0;
            Notification saveNotify = new Notification();
            IList<Notification> ilstSaveNotify = new List<Notification>();
            Notification updateNotify = new Notification();
            IList<Notification> ilstUpdateNotify = new List<Notification>();
            Notification deleteNotify = new Notification();
            IList<Notification> ilstdeleteNotify = new List<Notification>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (ScreenName.Trim().ToUpper() != "NOTIFY")
                {
                    foreach (CDSRuleMaster lstCDS in ClinicalList)
                    {


                        saveNotify = new Notification();
                        updateNotify = new Notification();
                        deleteNotify = new Notification();
                        string query = string.Empty;
                        ISQLQuery QuerySQL = null;
                        query = lstCDS.Where_Criteria;
                        Boolean bCheck = false;

                        //For Exception provider  //Bug ID 51397 removed temporary...
                        #region ExceptionProvider
                        //IList<CDSRuleMaster> lst = ClinicalList.Where(a => a.Clinincal_Decision_Name.Trim().ToUpper() == lstCDS.Clinincal_Decision_Name.Trim().ToUpper() && a.Exception_Provider.ToUpper().Contains(sEncounteProviderName.ToUpper())).ToList<CDSRuleMaster>();
                        //if (lst != null && lst.Count > 0)
                        //{
                        //    if (lst[0].Exception_Provider.Contains(','))
                        //    {
                        //        string[] sArry = lst[0].Exception_Provider.Split(',');
                        //        foreach (string sRole in sArry)
                        //        {
                        //            if (sRole.ToUpper() == sEncounteProviderName.ToUpper())
                        //            {
                        //                goto l;
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        continue;
                        //    }
                        //l:
                        //    continue;
                        //}
                        //
                        #endregion

                        if (ScreenName.Trim().ToUpper() == "ALL" || ScreenName.Trim().ToUpper() == "CALCULATEALL")
                        {
                            if (lstCDS.Is_Manage_CDS_Allowed == "N")
                            {
                                if (ScreenName.Trim().ToUpper() == "CALCULATEALL" || ScreenName.Trim().ToUpper() == "ALL" || lstCDS.Role_Privilege.ToUpper().Contains(sUserRole.ToUpper()))
                                {
                                    //Calulate all Macra measures 
                                    ArrayList ArysqlList = null;
                                    string[] sQuery = query.Split('&');
                                    string[] sEntity = lstCDS.Entity_Name.Split('&');
                                    try
                                    {
                                        for (int i = 0; i < sQuery.Count(); i++)
                                        {
                                            ArysqlList = null;
                                            if (bCheck == false)
                                            {
                                                if (ulEncounterId != 0)
                                                {
                                                    QuerySQL = iMySession.CreateSQLQuery(sQuery[i].Replace(":HumanID", ulHumanID.ToString()).Replace(":Enc", ulEncounterId.ToString())).AddEntity(sEntity[i]);
                                                    ArysqlList = new ArrayList(QuerySQL.List());
                                                }
                                                else
                                                {
                                                    if (sQuery[i].Contains(":Enc") == false)
                                                    {
                                                        QuerySQL = iMySession.CreateSQLQuery(sQuery[i].Replace(":HumanID", ulHumanID.ToString())).AddEntity(sEntity[i]);
                                                        ArysqlList = new ArrayList(QuerySQL.List());
                                                    }
                                                }



                                                if (ArysqlList != null && ArysqlList.Count > 0)
                                                {
                                                    bCheck = false;
                                                }
                                                else
                                                {
                                                    bCheck = true;
                                                }
                                            }
                                        }
                                        if (bCheck == false)
                                        {
                                            IList<Notification> ilstNotify = NotificationList.Where(a => a.CDS_Rule_Master_Name == lstCDS.Clinincal_Decision_Name).ToList<Notification>();
                                            if (ilstNotify.Count > 0)
                                            {
                                                foreach (Notification item in ilstNotify)
                                                {
                                                    updateNotify = item;
                                                    updateNotify.Modified_By = UserName;
                                                    updateNotify.Modified_Date_And_Time = Current_dt;
                                                    updateNotify.Status = "Active";
                                                    ilstUpdateNotify.Add(updateNotify);
                                                }
                                            }
                                            else
                                            {
                                                saveNotify.CDS_Rule_Master_Name = lstCDS.Clinincal_Decision_Name;
                                                saveNotify.Human_ID = ulHumanID.ToString();
                                                saveNotify.Encounter_ID = ulEncounterId.ToString();
                                                saveNotify.Created_By = UserName;
                                                saveNotify.Created_Date_And_Time = Current_dt;
                                                saveNotify.Modified_By = "";
                                                saveNotify.Modified_Date_And_Time = DateTime.MinValue;
                                                saveNotify.Status = "Active";
                                                ilstSaveNotify.Add(saveNotify);
                                            }
                                        }
                                        else
                                        {
                                            //For Not satisfied rule v need to delete but here v update
                                            IList<Notification> ilstNotify = NotificationList.Where(a => a.CDS_Rule_Master_Name == lstCDS.Clinincal_Decision_Name).ToList<Notification>();
                                            if (ilstNotify.Count > 0)
                                            {
                                                foreach (Notification item in ilstNotify)
                                                {
                                                    updateNotify = item;
                                                    updateNotify.Modified_By = UserName;
                                                    updateNotify.Modified_Date_And_Time = Current_dt;
                                                    updateNotify.Status = "InActive";
                                                    ilstUpdateNotify.Add(updateNotify);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw (ex);
                                    }
                                }

                            }
                            else
                            {
                                //Compare all cds rule with userlookup
                                IList<UserLookup> ilstLookup = UserLookupList.Where(a => a.Value == lstCDS.Clinincal_Decision_Name).ToList<UserLookup>();
                                if (ilstLookup.Count > 0)
                                {
                                    ArrayList ArysqlList = null;
                                    string[] sQuery = query.Split('&');
                                    string[] sEntity = lstCDS.Entity_Name.Split('&');
                                    try
                                    {
                                        for (int i = 0; i < sQuery.Count(); i++)
                                        {
                                            ArysqlList = null;
                                            if (bCheck == false)
                                            {
                                                if (ulEncounterId != 0)
                                                {
                                                    QuerySQL = iMySession.CreateSQLQuery(sQuery[i].Replace(":HumanID", ulHumanID.ToString()).Replace(":Enc", ulEncounterId.ToString())).AddEntity(sEntity[i]);
                                                    ArysqlList = new ArrayList(QuerySQL.List());
                                                }
                                                else
                                                {
                                                    if (sQuery[i].Contains(":Enc") == false)
                                                    {
                                                        QuerySQL = iMySession.CreateSQLQuery(sQuery[i].Replace(":HumanID", ulHumanID.ToString())).AddEntity(sEntity[i]);
                                                        ArysqlList = new ArrayList(QuerySQL.List());
                                                    }
                                                }

                                                if (ArysqlList != null && ArysqlList.Count > 0)
                                                {
                                                    bCheck = false;
                                                }
                                                else
                                                {
                                                    bCheck = true;
                                                }
                                            }
                                        }
                                        if (bCheck == false)
                                        {
                                            IList<Notification> ilstNotify = NotificationList.Where(a => a.CDS_Rule_Master_Name == lstCDS.Clinincal_Decision_Name).ToList<Notification>();
                                            if (ilstNotify.Count > 0)
                                            {
                                                foreach (Notification item in ilstNotify)
                                                {
                                                    updateNotify = item;
                                                    updateNotify.Modified_By = UserName;
                                                    updateNotify.Modified_Date_And_Time = Current_dt;
                                                    updateNotify.Status = "Active";
                                                    ilstUpdateNotify.Add(updateNotify);
                                                }
                                            }
                                            else
                                            {
                                                saveNotify.CDS_Rule_Master_Name = lstCDS.Clinincal_Decision_Name;
                                                saveNotify.Human_ID = ulHumanID.ToString();
                                                saveNotify.Encounter_ID = ulEncounterId.ToString();
                                                saveNotify.Created_By = UserName;
                                                saveNotify.Created_Date_And_Time = Current_dt;
                                                saveNotify.Modified_By = "";
                                                saveNotify.Modified_Date_And_Time = DateTime.MinValue;
                                                saveNotify.Status = "Active";
                                                ilstSaveNotify.Add(saveNotify);
                                            }
                                        }
                                        else
                                        {
                                            //For Not satisfied rule v need to delete but here v update
                                            IList<Notification> ilstNotify = NotificationList.Where(a => a.CDS_Rule_Master_Name == lstCDS.Clinincal_Decision_Name).ToList<Notification>();
                                            if (ilstNotify.Count > 0)
                                            {
                                                foreach (Notification item in ilstNotify)
                                                {
                                                    updateNotify = item;
                                                    updateNotify.Modified_By = UserName;
                                                    updateNotify.Modified_Date_And_Time = Current_dt;
                                                    updateNotify.Status = "InActive";
                                                    ilstUpdateNotify.Add(updateNotify);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw (ex);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Calulate for cds Rule for specify screen
                            ArrayList ArysqlList = null;
                            string[] sQuery = query.Split('&');
                            string[] sEntity = lstCDS.Entity_Name.Split('&');
                            string[] sScreenName = lstCDS.SCN_Name.Split('&');
                            string[] ScreenNametemp = ScreenName.Split('|');
                            for (int k = 0; k < ScreenNametemp.Length; k++)
                            {
                                if (sScreenName.Contains(ScreenNametemp[k]) == true)
                                {
                                    for (int i = 0; i < sQuery.Count(); i++)
                                    {
                                        ArysqlList = null;
                                        if (bCheck == false)
                                        {
                                            if (ulEncounterId != 0)
                                            {
                                                QuerySQL = iMySession.CreateSQLQuery(sQuery[i].Replace(":HumanID", ulHumanID.ToString()).Replace(":Enc", ulEncounterId.ToString())).AddEntity(sEntity[i]);
                                                ArysqlList = new ArrayList(QuerySQL.List());
                                            }
                                            else
                                            {
                                                if (sQuery[i].Contains(":Enc") == false)
                                                {
                                                    QuerySQL = iMySession.CreateSQLQuery(sQuery[i].Replace(":HumanID", ulHumanID.ToString())).AddEntity(sEntity[i]);
                                                    ArysqlList = new ArrayList(QuerySQL.List());
                                                }
                                            }

                                            if (ArysqlList != null && ArysqlList.Count > 0)
                                            {
                                                bCheck = false;
                                            }
                                            else
                                            {
                                                bCheck = true;
                                            }
                                        }
                                    }
                                    if (bCheck == false)
                                    {
                                        //save
                                        IList<Notification> lstNotification = NotificationList.Where(a => a.CDS_Rule_Master_Name.Trim().ToUpper() == lstCDS.Clinincal_Decision_Name.Trim().ToUpper()).ToList<Notification>();
                                        if (lstNotification.Count == 0)
                                        {
                                            saveNotify.CDS_Rule_Master_Name = lstCDS.Clinincal_Decision_Name;
                                            saveNotify.Human_ID = ulHumanID.ToString();
                                            saveNotify.Encounter_ID = ulEncounterId.ToString();
                                            saveNotify.Created_By = UserName;
                                            saveNotify.Created_Date_And_Time = Current_dt;
                                            saveNotify.Modified_By = "";
                                            saveNotify.Modified_Date_And_Time = DateTime.MinValue;
                                            saveNotify.Status = "Active";
                                            ilstSaveNotify.Add(saveNotify);
                                        }
                                        else
                                        {
                                            foreach (Notification item in lstNotification)
                                            {
                                                updateNotify = item;
                                                updateNotify.Modified_By = UserName;
                                                updateNotify.Modified_Date_And_Time = Current_dt;
                                                updateNotify.Status = "Active";
                                                ilstUpdateNotify.Add(updateNotify);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        IList<Notification> lstNotification = NotificationList.Where(a => a.CDS_Rule_Master_Name.Trim().ToUpper() == lstCDS.Clinincal_Decision_Name.Trim().ToUpper()).ToList<Notification>();
                                        foreach (Notification item in lstNotification)
                                        {
                                            updateNotify = item;
                                            updateNotify.Modified_By = UserName;
                                            updateNotify.Modified_Date_And_Time = Current_dt;
                                            updateNotify.Status = "Inactive";
                                            ilstUpdateNotify.Add(updateNotify);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (ilstSaveNotify.Count > 0 || ilstUpdateNotify.Count > 0 || ilstdeleteNotify.Count > 0)
                    {
                        NotificationManager objNotificationMngr = new NotificationManager();
                        NotificationList = objNotificationMngr.SaveUpdateDeleteNotification(ilstSaveNotify, ilstUpdateNotify, ilstdeleteNotify, ulHumanID, ulEncounterId);
                        NotificationList = (from cds in ClinicalList
                                            join notify in NotificationList
                                             on cds.Clinincal_Decision_Name equals notify.CDS_Rule_Master_Name
                                            select notify).ToList<Notification>();
                        foreach (Notification item in NotificationList)
                        {
                            IList<UserLookup> lstLookupList = UserLookupList.Where(a => a.Value.Trim().ToUpper() == item.CDS_Rule_Master_Name.Trim().ToUpper()).ToList<UserLookup>();
                            if (lstLookupList != null && lstLookupList.Count > 0)
                            {
                                if (item.Status.Trim().ToUpper() == "ACTIVE")
                                {
                                    IList<CDSRuleMaster> lst = ClinicalList.Where(a => a.Clinincal_Decision_Name.Trim().ToUpper() == item.CDS_Rule_Master_Name.Trim().ToUpper() && a.Role_Privilege.ToUpper().Contains(sUserRole.ToUpper())).ToList<CDSRuleMaster>();
                                    if (lst != null && lst.Count > 0)
                                    {
                                        if (lst[0].Role_Privilege.Contains(','))
                                        {
                                            string[] sArry = lst[0].Role_Privilege.Split(',');
                                            foreach (string sRole in sArry)
                                            {
                                                if (sRole.ToUpper() == sUserRole.ToUpper())
                                                {
                                                    // FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                    if (lst[0].Physician_Specialty.Trim() != string.Empty)
                                                    {
                                                        string[] arySpeciality = sPhysicianSpecialty.Split(',');
                                                        var ary = lst[0].Physician_Specialty.Split(',').Intersect(arySpeciality);
                                                        Boolean bCommon = false;

                                                        foreach (string s in ary)
                                                        {
                                                            bCommon = true;
                                                        }
                                                        if (bCommon == true)
                                                        {
                                                            FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (item.Status.Trim().ToUpper() == "ACTIVE")
                                {
                                    IList<CDSRuleMaster> lst = ClinicalList.Where(a => a.Clinincal_Decision_Name.Trim().ToUpper() == item.CDS_Rule_Master_Name.Trim().ToUpper() && a.Is_Manage_CDS_Allowed == "N" && a.Role_Privilege.ToUpper().Contains(sUserRole.ToUpper())).ToList<CDSRuleMaster>();
                                    if (lst != null && lst.Count > 0)
                                    {
                                        if (lst[0].Role_Privilege.Contains(','))
                                        {
                                            string[] sArry = lst[0].Role_Privilege.Split(',');
                                            foreach (string sRole in sArry)
                                            {
                                                if (sRole.ToUpper() == sUserRole.ToUpper())
                                                {
                                                    // FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                    if (lst[0].Physician_Specialty.Trim() != string.Empty)
                                                    {
                                                        string[] arySpeciality = sPhysicianSpecialty.Split(',');
                                                        var ary = lst[0].Physician_Specialty.Split(',').Intersect(arySpeciality);
                                                        Boolean bCommon = false;

                                                        foreach (string s in ary)
                                                        {
                                                            bCommon = true;
                                                        }
                                                        if (bCommon == true)
                                                        {
                                                            FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        NotificationManager objNotificationMngr = new NotificationManager();
                        NotificationList = objNotificationMngr.GetNotificationForHumanID(ulHumanID, ulEncounterId);
                        NotificationList = (from cds in ClinicalList
                                            join notify in NotificationList
                                             on cds.Clinincal_Decision_Name equals notify.CDS_Rule_Master_Name
                                            select notify).ToList<Notification>();
                        if (NotificationList.Count > 0)
                        {
                            foreach (Notification item in NotificationList)
                            {
                                IList<UserLookup> lstLookupList = UserLookupList.Where(a => a.Value.Trim().ToUpper() == item.CDS_Rule_Master_Name.Trim().ToUpper()).ToList<UserLookup>();
                                if (lstLookupList != null && lstLookupList.Count > 0)
                                {
                                    if (item.Status.Trim().ToUpper() == "ACTIVE")
                                    {
                                        IList<CDSRuleMaster> lst = ClinicalList.Where(a => a.Clinincal_Decision_Name.Trim().ToUpper() == item.CDS_Rule_Master_Name.Trim().ToUpper() && a.Role_Privilege.ToUpper().Contains(sUserRole.ToUpper())).ToList<CDSRuleMaster>();
                                        if (lst != null && lst.Count > 0)
                                        {
                                            if (lst[0].Role_Privilege.Contains(','))
                                            {
                                                string[] sArry = lst[0].Role_Privilege.Split(',');
                                                foreach (string sRole in sArry)
                                                {
                                                    if (sRole.ToUpper() == sUserRole.ToUpper())
                                                    {
                                                        // FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                        if (lst[0].Physician_Specialty.Trim() != string.Empty)
                                                        {
                                                            string[] arySpeciality = sPhysicianSpecialty.Split(',');
                                                            var ary = lst[0].Physician_Specialty.Split(',').Intersect(arySpeciality);
                                                            Boolean bCommon = false;

                                                            foreach (string s in ary)
                                                            {
                                                                bCommon = true;
                                                            }
                                                            if (bCommon == true)
                                                            {
                                                                FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                        }
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    if (item.Status.Trim().ToUpper() == "ACTIVE")
                                    {
                                        IList<CDSRuleMaster> lst = ClinicalList.Where(a => a.Clinincal_Decision_Name.Trim().ToUpper() == item.CDS_Rule_Master_Name.Trim().ToUpper() && a.Is_Manage_CDS_Allowed == "N" && a.Role_Privilege.ToUpper().Contains(sUserRole.ToUpper())).ToList<CDSRuleMaster>();
                                        if (lst != null && lst.Count > 0)
                                        {
                                            if (lst[0].Role_Privilege.Contains(','))
                                            {
                                                string[] sArry = lst[0].Role_Privilege.Split(',');
                                                foreach (string sRole in sArry)
                                                {
                                                    if (sRole.ToUpper() == sUserRole.ToUpper())
                                                    {
                                                        // FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                        if (lst[0].Physician_Specialty.Trim() != string.Empty)
                                                        {
                                                            string[] arySpeciality = sPhysicianSpecialty.Split(',');
                                                            var ary = lst[0].Physician_Specialty.Split(',').Intersect(arySpeciality);
                                                            Boolean bCommon = false;

                                                            foreach (string s in ary)
                                                            {
                                                                bCommon = true;
                                                            }
                                                            if (bCommon == true)
                                                            {
                                                                FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                                        }
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                FillText(lst[0].Clinincal_Decision_Name, lst[0].Rules, lst[0].Message, lst[0].Source_Attributes, lst[0].Developer, lst[0].Funding_Source, lst[0].Link_Data, lst[0].Release_Date, lst[0].Classification, lst[0].Resolve_Screen);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                iMySession.Close();
            }
            strNotifiaction = strNotifiaction + "~~~" + NotificationCount.ToString() + "~*~" + NotificationSummaryText;
            return strNotifiaction;
        }

        public IList<Encounter> GetEncoutnerListByPhyID(int Phy_ID)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Encounter_Provider_ID", Phy_ID));
                ilstEncounter = crit.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            //return crit.List<Encounter>();
        }


        public IList<Encounter> GetEncounterList(IList<ulong> EncLst)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.In("Id", EncLst.ToArray()));
                //return crit.List<Encounter>();
                ilstEncounter = crit.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
        }
        public IList<Encounter> GetEncounterListArchive(IList<string> EncLst)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            string svalue = string.Join(",", EncLst.Cast<string>().ToArray());
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sqlquery = iMySession.CreateSQLQuery("SELECT enc.* FROM encounter enc where enc.encounter_id in(" + svalue +
                    ") and date_of_service!='0001-01-01 00:00:00' union all SELECT arc.* FROM encounter_arc arc  where arc.encounter_id in(" + svalue + ") and date_of_service!='0001-01-01 00:00:00';").AddEntity("e", typeof(Encounter));
                ilstEncounter = sqlquery.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
        }
        //public bool SaveSummaryOfCare(SummaryOfCareDTO SummaryDTOList, string UserName)
        //{
        //    ulong Human_Id = 0;
        //    ulong Physicican_Id = 0;
        //    ulong Encounter_Id = 0;
        //    ulong Result_Master_Id = 0;
        //    ulong Result_OBR_Id = 0;
        //    string macAddress = string.Empty;
        //    DateTime Activity_log_time = DateTime.MinValue;
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {

        //        #region Human

        //        IList<Human> lsthuman = new List<Human>();
        //        lsthuman = SummaryDTOList.HumanList;
        //        if (lsthuman.Count > 0)
        //        {
        //            IList<Human> temp_lst = new List<Human>();
        //            ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Last_Name", lsthuman[0].Last_Name)).Add(Expression.Eq("First_Name", lsthuman[0].First_Name)).Add(Expression.Eq("Sex", lsthuman[0].Sex)).Add(Expression.Eq("Birth_Date", lsthuman[0].Birth_Date));
        //            temp_lst = crit.List<Human>();
        //            if (temp_lst.Count > 0)
        //            {
        //                Human_Id = temp_lst[0].Id;

        //                if (temp_lst[0].Is_Sent_To_Rcopia == "N")
        //                {
        //                    temp_lst[0].Is_Sent_To_Rcopia = "Y";
        //                    IList<Human> humanListadd = null;
        //                    HumanManager objhumanmanager = new HumanManager();
        //                    objhumanmanager.SaveUpdateDeleteWithTransaction(ref humanListadd, temp_lst, null, macAddress);
        //                    RCopiaTransactionManager objRcopiaMngr = new RCopiaTransactionManager();
        //                    objRcopiaMngr.SendPatientToRCopia(Human_Id, macAddress);
        //                }
        //            }
        //            else
        //            {
        //                HumanManager objhumanmanager = new HumanManager();
        //                lsthuman[0].Patient_Status = "ALIVE";
        //                lsthuman[0].Human_Type = "REGULAR";
        //                lsthuman[0].Account_Status = "Active";
        //                Human_Id = objhumanmanager.SaveHumanforSummary(lsthuman);

        //            }
        //        }

        //        #endregion

        //        #region PhysicianLibrary

        //        IList<PhysicianLibrary> lstphysician = new List<PhysicianLibrary>();
        //        lstphysician = SummaryDTOList.PhysicianList;
        //        if (lstphysician.Count > 0)
        //        {
        //            IList<PhysicianLibrary> temp_lst = new List<PhysicianLibrary>();
        //            ICriteria crit = iMySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("PhyLastName", lstphysician[0].PhyLastName)).Add(Expression.Eq("PhyFirstName", lstphysician[0].PhyFirstName));
        //            temp_lst = crit.List<PhysicianLibrary>();
        //            if (temp_lst.Count > 0)
        //            {
        //                Physicican_Id = temp_lst[0].Id;
        //            }
        //            else
        //            {

        //                PhysicianManager objmanager = new PhysicianManager();
        //                Physicican_Id = objmanager.SavePhysicanforSummary(lstphysician);
        //            }
        //        }
        //        #endregion

        //        #region FacilityLibrary
        //        IList<FacilityLibrary> lstfacility = new List<FacilityLibrary>();
        //        lstfacility = SummaryDTOList.FacilityList;
        //        if (lstfacility.Count > 0)
        //        {
        //            IList<FacilityLibrary> temp_lst = new List<FacilityLibrary>();
        //            ICriteria crit = iMySession.CreateCriteria(typeof(FacilityLibrary)).Add(Expression.Eq("Fac_Name", lstfacility[0].Fac_Name));
        //            temp_lst = crit.List<FacilityLibrary>();
        //            if (temp_lst.Count == 0)
        //            {
        //                FacilityManager objmanager = new FacilityManager();
        //                objmanager.SaveFacilityforSummary(lstfacility);
        //            }
        //        }
        //        #endregion

        //        #region Encounter
        //        IList<Encounter> lstencounter = new List<Encounter>();
        //        IList<Encounter> encupdateList = null;
        //        lstencounter = SummaryDTOList.EncounterList;
        //        if (lstencounter.Count > 0)
        //        {
        //            lstencounter[0].Human_ID = Human_Id;
        //            string id = Physicican_Id.ToString();
        //            lstencounter[0].Appointment_Provider_ID = Convert.ToInt32(id);
        //            lstencounter[0].Encounter_Provider_ID = Convert.ToInt32(id);
        //            //  SaveUpdateDeleteWithTransaction(ref lstencounter, null, null, macAddress);
        //            SaveUpdateDelete_DBAndXML_WithTransaction(ref lstencounter, ref encupdateList, null, macAddress, true, false, lstencounter[0].Id, string.Empty);
        //            Encounter_Id = Convert.ToUInt64(lstencounter[0].Id);
        //            Activity_log_time = lstencounter[0].Created_Date_and_Time;
        //        }
        //        #endregion

        //        #region ActivityLog

        //        IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
        //        ActivityLogManager ActivitylogMngr = new ActivityLogManager();
        //        ActivityLog activity = new ActivityLog();
        //        activity.Activity_Type = "CCD Incorporated";
        //        activity.Activity_Date_And_Time = Activity_log_time;
        //        activity.Encounter_ID = Encounter_Id;
        //        activity.Human_ID = Human_Id;
        //        ActivityLogList.Add(activity);
        //        ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);

        //        #endregion

        //        #region SocialHistory
        //        IList<SocialHistory> lstsocial = new List<SocialHistory>();
        //        lstsocial = SummaryDTOList.SocialHistoryList;
        //        if (lstsocial.Count > 0)
        //        {
        //            for (int i = 0; i < lstsocial.Count; i++)
        //            {
        //                lstsocial[i].Human_ID = Human_Id;
        //                lstsocial[i].Encounter_ID = Encounter_Id;
        //            }
        //            SocialHistoryManager objmanager = new SocialHistoryManager();
        //            objmanager.SaveSocialHistoryforSummary(lstsocial);
        //        }
        //        #endregion

        //        #region Vitals
        //        IList<PatientResults> lstvitals = new List<PatientResults>();
        //        lstvitals = SummaryDTOList.VitalList;
        //        if (lstvitals.Count > 0)
        //        {
        //            for (int i = 0; i < lstvitals.Count; i++)
        //            {
        //                lstvitals[i].Human_ID = Human_Id;
        //                lstvitals[i].Encounter_ID = Encounter_Id;
        //                lstvitals[i].Physician_ID = Physicican_Id;
        //            }
        //            VitalsManager objmanager = new VitalsManager();
        //            objmanager.SaveVitalsforSummary(lstvitals);
        //        }
        //        #endregion

        //        #region TreatmentPlan
        //        IList<TreatmentPlan> lsttreatment = new List<TreatmentPlan>();
        //        lsttreatment = SummaryDTOList.TreatmentPlanList;
        //        if (lsttreatment.Count > 0)
        //        {
        //            for (int i = 0; i < lsttreatment.Count; i++)
        //            {
        //                lsttreatment[i].Human_ID = Human_Id;
        //                lsttreatment[i].Encounter_Id = Encounter_Id;
        //                lsttreatment[i].Physician_Id = Physicican_Id;
        //            }
        //            TreatmentPlanManager objmanager = new TreatmentPlanManager();
        //            objmanager.SaveTreatmentforSummary(lsttreatment);
        //        }
        //        #endregion

        //        #region CarePlan
        //        IList<CarePlan> lstcare = new List<CarePlan>();
        //        lstcare = SummaryDTOList.CareplanList;
        //        if (lstcare.Count > 0)
        //        {
        //            for (int i = 0; i < lstcare.Count; i++)
        //            {
        //                lstcare[i].Human_ID = Human_Id;
        //                lstcare[i].Encounter_ID = Encounter_Id;
        //                lstcare[i].Physician_ID = Physicican_Id;
        //            }
        //            CarePlanManager objmanager = new CarePlanManager();
        //            objmanager.SaveCarePlanforSummary(lstcare);
        //        }
        //        #endregion

        //        #region Immunization
        //        IList<Immunization> lstimmunization = new List<Immunization>();
        //        lstimmunization = SummaryDTOList.ImmunizationList;
        //        if (lstimmunization.Count > 0)
        //        {
        //            for (int i = 0; i < lstimmunization.Count; i++)
        //            {
        //                lstimmunization[i].Human_ID = Human_Id;
        //                lstimmunization[i].Encounter_Id = Encounter_Id;
        //                lstimmunization[i].Physician_Id = Physicican_Id;
        //            }
        //            ImmunizationManager objmanager = new ImmunizationManager();
        //            objmanager.SaveImmunizationforSummary(lstimmunization);
        //        }
        //        #endregion

        //        #region ProblemList
        //        //IList<ProblemList> lstproblem = new List<ProblemList>();
        //        //lstproblem = SummaryDTOList.ProblemList;
        //        //if (lstproblem.Count > 0)
        //        //{
        //        //    for (int i = 0; i < lstproblem.Count; i++)
        //        //    {
        //        //        lstproblem[i].Human_ID = Human_Id;
        //        //        lstproblem[i].Encounter_ID = Encounter_Id;
        //        //        lstproblem[i].Physician_ID = Physicican_Id;
        //        //    }
        //        //    ProblemListManager objmanager = new ProblemListManager();
        //        //    objmanager.SaveProblemListforSummary(lstproblem);
        //        //}
        //        #endregion

        //        #region ReferalOrder
        //        IList<ReferralOrder> lstreferalorder = new List<ReferralOrder>();
        //        lstreferalorder = SummaryDTOList.ReferalOrderList;
        //        if (lstreferalorder.Count > 0)
        //        {
        //            for (int i = 0; i < lstreferalorder.Count; i++)
        //            {
        //                lstreferalorder[i].Human_ID = Human_Id;
        //                lstreferalorder[i].Encounter_ID = Encounter_Id;

        //            }
        //            ReferralOrderManager objmanager = new ReferralOrderManager();
        //            objmanager.SaveReferalOrderforSummary(lstreferalorder);
        //        }
        //        #endregion

        //        #region Procedures

        //        IList<InHouseProcedure> lstprocedure = new List<InHouseProcedure>();
        //        lstprocedure = SummaryDTOList.ProcedureList;
        //        if (lstprocedure.Count > 0)
        //        {
        //            for (int i = 0; i < lstprocedure.Count; i++)
        //            {
        //                lstprocedure[i].Human_ID = Human_Id;
        //                lstprocedure[i].Encounter_ID = Encounter_Id;
        //                lstprocedure[i].Physician_ID = Physicican_Id;
        //            }
        //            InHouseProcedureManager objmanager = new InHouseProcedureManager();
        //            objmanager.SaveProceduresforSummary(lstprocedure);
        //        }

        //        #endregion

        //        #region ResultMaster

        //        IList<ResultMaster> lstresultmaster = new List<ResultMaster>();
        //        lstresultmaster = SummaryDTOList.ResultMasterList;
        //        if (lstresultmaster.Count > 0)
        //        {
        //            for (int i = 0; i < lstresultmaster.Count; i++)
        //            {
        //                lstresultmaster[i].PID_External_Patient_ID = Human_Id.ToString();
        //                lstresultmaster[i].Matching_Patient_Id = Human_Id;
        //                lstresultmaster[i].PID_Alternate_Patient_ID = Human_Id.ToString();
        //            }
        //            ResultMasterManager objmanager = new ResultMasterManager();
        //            Result_Master_Id = objmanager.SaveResultMasterforSummary(lstresultmaster);
        //        }

        //        #endregion

        //        #region ResultOBR

        //        IList<ResultOBR> lstresultobr = new List<ResultOBR>();
        //        lstresultobr = SummaryDTOList.ResultObrList;
        //        if (lstresultobr.Count > 0)
        //        {
        //            for (int i = 0; i < lstresultobr.Count; i++)
        //            {
        //                lstresultobr[i].Result_Master_ID = Result_Master_Id;
        //            }
        //            ResultOBRManager objmanager = new ResultOBRManager();
        //            Result_OBR_Id = objmanager.SaveResultOBRforSummary(lstresultobr);
        //        }

        //        #endregion

        //        #region ResultOBX

        //        IList<ResultOBX> lstresultobx = new List<ResultOBX>();
        //        lstresultobx = SummaryDTOList.ResultObxList;
        //        if (lstresultobx.Count > 0)
        //        {
        //            for (int i = 0; i < lstresultobx.Count; i++)
        //            {
        //                lstresultobx[i].Result_Master_ID = Result_Master_Id;
        //                lstresultobx[i].Result_OBR_ID = Result_OBR_Id;
        //            }
        //            ResultOBXManager objmanager = new ResultOBXManager();
        //            objmanager.SaveResultOBXforSummary(lstresultobx);
        //        }

        //        #endregion

        //        #region ResultORC

        //        IList<ResultORC> lstresultorc = new List<ResultORC>();
        //        lstresultorc = SummaryDTOList.ResultOrcList;
        //        if (lstresultorc.Count > 0)
        //        {
        //            for (int i = 0; i < lstresultorc.Count; i++)
        //            {
        //                lstresultorc[i].Result_Master_ID = Result_Master_Id;
        //            }
        //            ResultORCManager objmanager = new ResultORCManager();
        //            objmanager.SaveResultORCforSummary(lstresultorc);
        //        }

        //        #endregion

        //        #region Assesment

        //        IList<Assessment> lstassesment = new List<Assessment>();
        //        lstassesment = SummaryDTOList.AssesmentList;
        //        if (lstassesment.Count > 0)
        //        {
        //            for (int i = 0; i < lstassesment.Count; i++)
        //            {
        //                lstassesment[i].Human_ID = Human_Id;
        //                lstassesment[i].Encounter_ID = Encounter_Id;
        //                lstassesment[i].Physician_ID = Physicican_Id;
        //            }
        //            AssessmentManager objmanager = new AssessmentManager();
        //            objmanager.SaveAssesmentforSummary(lstassesment);
        //        }

        //        #endregion

        //        #region Allergy

        //        IList<Rcopia_Allergy> lstallergy = new List<Rcopia_Allergy>();
        //        lstallergy = SummaryDTOList.AllergyList;
        //        if (lstallergy.Count > 0)
        //        {
        //            for (int i = 0; i < lstallergy.Count; i++)
        //            {
        //                lstallergy[i].Human_ID = Human_Id;
        //            }
        //        }
        //        RCopiaGenerateXML objMedicationmanager = new RCopiaGenerateXML();
        //        objMedicationmanager.creteSendAllergyXml(lstallergy.ToArray(), null, null);

        //        #endregion

        //        #region Medication

        //        IList<Rcopia_Medication> lstmedication = new List<Rcopia_Medication>();
        //        lstmedication = SummaryDTOList.MedicationList;
        //        if (lstmedication.Count > 0)
        //        {
        //            for (int i = 0; i < lstmedication.Count; i++)
        //            {
        //                lstmedication[i].Human_ID = Human_Id;
        //            }
        //        }
        //        RCopiaGenerateXML objMedicationmanagerobjMedicationmanager = new RCopiaGenerateXML();
        //        objMedicationmanagerobjMedicationmanager.creteSendMedicationXml(lstmedication, null, null);
        //        RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
        //        Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
        //        objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.UploadAddress, UserName, macAddress, DateTime.UtcNow);
        //        #endregion
        //        iMySession.Close();
        //    }

        //    return true;
        //}

        public string SaveSummaryOfCare(SummaryOfCareDTO SummaryDTOList, string UserName, string FacilityName, ulong EncID, string sLegalOrg)
        {
            ulong Human_Id = 0;
            ulong Physicican_Id = 0;
            ulong Encounter_Id = 0;
            ulong Result_Master_Id = 0;
            ulong Result_OBR_Id = 0;
            string macAddress = string.Empty;
            DateTime Activity_log_time = DateTime.MinValue;
            GenerateXml objXML = new GenerateXml();
            string sreturn = string.Empty;


            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                #region Human

                IList<Human> lsthuman = new List<Human>();
                lsthuman = SummaryDTOList.HumanList;
                Human objHuman = new Human();
                if (lsthuman.Count > 0)
                {
                    objHuman = lsthuman[0];
                    IList<Human> temp_lst = new List<Human>();
                    ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Last_Name", lsthuman[0].Last_Name)).Add(Expression.Eq("First_Name", lsthuman[0].First_Name)).Add(Expression.Eq("Sex", lsthuman[0].Sex)).Add(Expression.Eq("Birth_Date", lsthuman[0].Birth_Date));
                    temp_lst = crit.List<Human>();
                    if (temp_lst.Count > 0)
                    {
                        Human_Id = temp_lst[0].Id;

                        if (temp_lst[0].Is_Sent_To_Rcopia == "N")
                        {
                            temp_lst[0].Is_Sent_To_Rcopia = "Y";
                            IList<Human> humanListadd = null;
                            HumanManager objhumanmanager = new HumanManager();
                            //objhumanmanager.SaveUpdateDeleteWithTransaction(ref humanListadd, temp_lst, null, macAddress);
                            objhumanmanager.SaveUpdateDelete_DBAndXML_WithTransaction(ref humanListadd, ref temp_lst, null, macAddress, true, false, temp_lst[0].Id, string.Empty);
                            RCopiaTransactionManager objRcopiaMngr = new RCopiaTransactionManager();
                            objRcopiaMngr.SendPatientToRCopia(Human_Id, macAddress, sLegalOrg);
                        }
                    }
                    else
                    {
                        HumanManager objhumanmanager = new HumanManager();
                        //lsthuman[0].Patient_Status = "ALIVE";
                        //lsthuman[0].Human_Type = "REGULAR";
                        //lsthuman[0].Account_Status = "Active";
                        objHuman.Patient_Status = "ALIVE";
                        objHuman.Human_Type = "REGULAR";
                        objHuman.Account_Status = "Active";
                        IList<Human> ilstHuman = new List<Human>();
                        Human_Id = objhumanmanager.AppendHumanFromImport(objHuman, sLegalOrg);
                        //ilstHuman = objhumanmanager.SaveHumanforSummary(lsthuman);
                        // Human_Id = ilstHuman[0].Id;

                        /*Generating Human XML*/
                        //if (ilstHuman[0].Id != 0)
                        //{                         
                        //    string HumanFileName = "Human" + "_" + ilstHuman[0].Id + ".xml";
                        //    string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
                        //    if (File.Exists(strXmlHumanFilePath) == false && ilstHuman[0].Id > 0)
                        //    {
                        //        string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                        //        string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                        //        XmlDocument itemDoc = new XmlDocument();
                        //        XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                        //        itemDoc.Load(XmlText);
                        //        XmlNodeList xmlnode = itemDoc.GetElementsByTagName("EncounterDetails");
                        //        xmlnode[0].ParentNode.RemoveChild(xmlnode[0]);

                        //        int iAge = 0;

                        //        DateTime now = DateTime.Today;
                        //        int years = now.Year - ilstHuman[0].Birth_Date.Year;
                        //        if (now.Month < ilstHuman[0].Birth_Date.Month || (now.Month == ilstHuman[0].Birth_Date.Month && now.Day < ilstHuman[0].Birth_Date.Day))
                        //            --years;
                        //        iAge = years;

                        //        XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
                        //        xmlAge[0].Attributes[0].Value = iAge.ToString();

                        //        XmlText.Close();
                        //        itemDoc.Save(strXmlHumanFilePath);

                        //        List<object> lsthumanObj = ilstHuman.Cast<object>().ToList();
                        //        objXML.Copy_Previous_GenerateXmlSave(lsthumanObj, Human_Id, string.Empty);
                        //    }                          
                        //}

                    }
                }

                #endregion

                #region PhysicianLibrary

                IList<PhysicianLibrary> lstphysician = new List<PhysicianLibrary>();
                lstphysician = SummaryDTOList.PhysicianList;
                if (lstphysician != null && lstphysician.Count > 0)
                {
                    IList<PhysicianLibrary> temp_lst = new List<PhysicianLibrary>();
                    ICriteria crit = iMySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("PhyLastName", lstphysician[0].PhyLastName)).Add(Expression.Eq("PhyFirstName", lstphysician[0].PhyFirstName));
                    temp_lst = crit.List<PhysicianLibrary>();
                    if (temp_lst.Count > 0)
                    {
                        Physicican_Id = temp_lst[0].Id;
                    }
                    else
                    {

                        PhysicianManager objmanager = new PhysicianManager();
                        if (lstphysician != null && lstphysician.Count > 0)
                        {
                            lstphysician[0].Taxonomy_Code = "208D00000X";
                            lstphysician[0].Taxonomy_Description = "General Practice";

                            Physicican_Id = objmanager.SavePhysicanforSummary(lstphysician);
                        }
                    }
                }
                #endregion

                #region FacilityLibrary
                IList<FacilityLibrary> lstfacility = new List<FacilityLibrary>();
                lstfacility = SummaryDTOList.FacilityList;
                if (lstfacility != null && lstfacility.Count > 0)
                {
                    IList<FacilityLibrary> temp_lst = new List<FacilityLibrary>();
                    ICriteria crit = iMySession.CreateCriteria(typeof(FacilityLibrary)).Add(Expression.Eq("Fac_Name", lstfacility[0].Fac_Name));
                    temp_lst = crit.List<FacilityLibrary>();
                    if (temp_lst.Count == 0)
                    {
                        FacilityManager objmanager = new FacilityManager();
                        objmanager.SaveFacilityforSummary(lstfacility);
                    }
                }
                #endregion

                #region Encounter
                IList<Encounter> lstencounter = new List<Encounter>();
                // ulong uMaxEncounterID = 0;
                IList<Encounter> encupdateList = null;
                lstencounter = SummaryDTOList.EncounterList;
                if (lstencounter != null && lstencounter.Count > 0)
                {
                    lstencounter[0].Human_ID = Human_Id;
                    string id = Physicican_Id.ToString();
                    lstencounter[0].Appointment_Provider_ID = Convert.ToInt32(id);
                    lstencounter[0].Encounter_Provider_ID = Convert.ToInt32(id);
                    //bugid:45349
                    SaveUpdateDelete_DBAndXML_WithTransaction(ref lstencounter, ref encupdateList, null, macAddress, false, false, 0, string.Empty);
                    ////Commented For Blob
                    //Encounter_Id = lstencounter[0].Id;
                    //string FileName = "Encounter" + "_" + Encounter_Id + ".xml";
                    //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

                    //if (File.Exists(strXmlFilePath) == false && Encounter_Id > 0)
                    //{
                    //    string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                    //    string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                    //    XmlDocument itemDoc = new XmlDocument();
                    //    XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                    //    itemDoc.Load(XmlText);
                    //    XmlText.Close();

                    //    XmlNodeList xmlAgenode = itemDoc.GetElementsByTagName("Age");
                    //    if (xmlAgenode != null && xmlAgenode.Count > 0)
                    //        xmlAgenode[0].ParentNode.RemoveChild(xmlAgenode[0]);

                    //    //itemDoc.Save(strXmlFilePath);
                    //    int trycount = 0;
                    //trytosaveagain:
                    //    try
                    //    {
                    //        //itemDoc.Save(strXmlFilePath);
                    //    }
                    //    catch (Exception xmlexcep)
                    //    {
                    //        //trycount++;
                    //        //if (trycount <= 3)
                    //        //{
                    //        //    int TimeMilliseconds = 0;
                    //        //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                    //        //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                    //        //    Thread.Sleep(TimeMilliseconds);
                    //        //    string sMsg = string.Empty;
                    //        //    string sExStackTrace = string.Empty;

                    //        //    string version = "";
                    //        //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                    //        //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                    //        //    string[] server = version.Split('|');
                    //        //    string serverno = "";
                    //        //    if (server.Length > 1)
                    //        //        serverno = server[1].Trim();

                    //        //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                    //        //        sMsg = xmlexcep.InnerException.Message;
                    //        //    else
                    //        //        sMsg = xmlexcep.Message;

                    //        //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                    //        //        sExStackTrace = xmlexcep.StackTrace;

                    //        //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    //        //    string ConnectionData;
                    //        //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    //        //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                    //        //    {
                    //        //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                    //        //        {
                    //        //            cmd.Connection = con;
                    //        //            try
                    //        //            {
                    //        //                con.Open();
                    //        //                cmd.ExecuteNonQuery();
                    //        //                con.Close();
                    //        //            }
                    //        //            catch
                    //        //            {
                    //        //            }
                    //        //        }
                    //        //    }
                    //        //    goto trytosaveagain;
                    //        //}
                    //    }

                    //    List<object> lstEncObj = lstencounter.Cast<object>().ToList();
                    //    objXML.Copy_Previous_GenerateXmlSave(lstEncObj, lstencounter[0].Id, string.Empty, false);
                    //    GenerateXml objxmlHuman = new GenerateXml();
                    //    objxmlHuman.Copy_Previous_GenerateXmlSave(lstEncObj, lstencounter[0].Human_ID, string.Empty, true);
                    //}


                    Activity_log_time = lstencounter[0].Created_Date_and_Time;
                }
                #endregion



                #region SocialHistory
                IList<SocialHistory> lstsocial = new List<SocialHistory>();
                lstsocial = SummaryDTOList.SocialHistoryList;
                if (lstsocial != null && lstsocial.Count > 0)
                {
                    for (int i = 0; i < lstsocial.Count; i++)
                    {
                        lstsocial[i].Human_ID = Human_Id;
                        lstsocial[i].Encounter_ID = Encounter_Id;
                    }
                    SocialHistoryManager objmanager = new SocialHistoryManager();
                    objmanager.SaveSocialHistoryforSummary(lstsocial);
                }
                #endregion

                #region Vitals
                IList<PatientResults> lstvitals = new List<PatientResults>();
                lstvitals = SummaryDTOList.VitalList;
                if (lstvitals != null && lstvitals.Count > 0)
                {
                    for (int i = 0; i < lstvitals.Count; i++)
                    {
                        lstvitals[i].Human_ID = Human_Id;
                        lstvitals[i].Encounter_ID = Encounter_Id;
                        lstvitals[i].Physician_ID = Physicican_Id;
                    }
                    VitalsManager objmanager = new VitalsManager();
                    objmanager.SaveVitalsforSummary(lstvitals);
                }
                #endregion

                #region TreatmentPlan
                IList<TreatmentPlan> lsttreatment = new List<TreatmentPlan>();
                lsttreatment = SummaryDTOList.TreatmentPlanList;
                if (lsttreatment != null && lsttreatment.Count > 0)
                {
                    for (int i = 0; i < lsttreatment.Count; i++)
                    {
                        lsttreatment[i].Human_ID = Human_Id;
                        lsttreatment[i].Encounter_Id = Encounter_Id;
                        lsttreatment[i].Physician_Id = Physicican_Id;
                    }
                    TreatmentPlanManager objmanager = new TreatmentPlanManager();
                    objmanager.SaveTreatmentforSummary(lsttreatment);
                }
                #endregion

                #region CarePlan
                IList<CarePlan> lstcare = new List<CarePlan>();
                lstcare = SummaryDTOList.CareplanList;
                if (lstcare != null && lstcare.Count > 0)
                {
                    for (int i = 0; i < lstcare.Count; i++)
                    {
                        lstcare[i].Human_ID = Human_Id;
                        lstcare[i].Encounter_ID = Encounter_Id;
                        lstcare[i].Physician_ID = Physicican_Id;
                    }
                    CarePlanManager objmanager = new CarePlanManager();
                    objmanager.SaveCarePlanforSummary(lstcare);
                }
                #endregion

                #region Immunization
                IList<Immunization> lstimmunization = new List<Immunization>();
                lstimmunization = SummaryDTOList.ImmunizationList;
                if (lstimmunization != null && lstimmunization.Count > 0)
                {
                    for (int i = 0; i < lstimmunization.Count; i++)
                    {
                        lstimmunization[i].Human_ID = Human_Id;
                        lstimmunization[i].Encounter_Id = Encounter_Id;
                        lstimmunization[i].Physician_Id = Physicican_Id;
                    }
                    ImmunizationManager objmanager = new ImmunizationManager();
                    objmanager.SaveImmunizationforSummary(lstimmunization);
                }
                #endregion

                #region ProblemList
                //IList<ProblemList> lstproblem = new List<ProblemList>();
                //lstproblem = SummaryDTOList.ProblemList;
                //if (lstproblem.Count > 0)
                //{
                //    for (int i = 0; i < lstproblem.Count; i++)
                //    {
                //        lstproblem[i].Human_ID = Human_Id;
                //        lstproblem[i].Encounter_ID = Encounter_Id;
                //        lstproblem[i].Physician_ID = Physicican_Id;
                //    }
                //    ProblemListManager objmanager = new ProblemListManager();
                //    objmanager.SaveProblemListforSummary(lstproblem);
                //}
                #endregion

                #region ReferalOrder
                IList<ReferralOrder> lstreferalorder = new List<ReferralOrder>();
                lstreferalorder = SummaryDTOList.ReferalOrderList;
                if (lstreferalorder != null && lstreferalorder.Count > 0)
                {
                    for (int i = 0; i < lstreferalorder.Count; i++)
                    {
                        lstreferalorder[i].Human_ID = Human_Id;
                        lstreferalorder[i].Encounter_ID = Encounter_Id;

                    }
                    ReferralOrderManager objmanager = new ReferralOrderManager();
                    objmanager.SaveReferalOrderforSummary(lstreferalorder);
                }
                #endregion

                #region Procedures

                IList<InHouseProcedure> lstprocedure = new List<InHouseProcedure>();
                lstprocedure = SummaryDTOList.ProcedureList;
                if (lstprocedure != null && lstprocedure.Count > 0)
                {
                    for (int i = 0; i < lstprocedure.Count; i++)
                    {
                        lstprocedure[i].Human_ID = Human_Id;
                        lstprocedure[i].Encounter_ID = Encounter_Id;
                        lstprocedure[i].Physician_ID = Physicican_Id;
                    }
                    InHouseProcedureManager objmanager = new InHouseProcedureManager();
                    objmanager.SaveProceduresforSummary(lstprocedure);
                }

                #endregion

                #region ResultMaster

                IList<ResultMaster> lstresultmaster = new List<ResultMaster>();
                lstresultmaster = SummaryDTOList.ResultMasterList;
                if (lstresultmaster != null && lstresultmaster.Count > 0)
                {
                    for (int i = 0; i < lstresultmaster.Count; i++)
                    {
                        lstresultmaster[i].PID_External_Patient_ID = Human_Id.ToString();
                        lstresultmaster[i].Matching_Patient_Id = Human_Id;
                        lstresultmaster[i].PID_Alternate_Patient_ID = Human_Id.ToString();
                    }
                    ResultMasterManager objmanager = new ResultMasterManager();
                    Result_Master_Id = objmanager.SaveResultMasterforSummary(lstresultmaster);
                }

                #endregion

                #region ResultOBR

                IList<ResultOBR> lstresultobr = new List<ResultOBR>();
                lstresultobr = SummaryDTOList.ResultObrList;
                if (lstresultobr != null && lstresultobr.Count > 0)
                {
                    for (int i = 0; i < lstresultobr.Count; i++)
                    {
                        lstresultobr[i].Result_Master_ID = Result_Master_Id;
                    }
                    ResultOBRManager objmanager = new ResultOBRManager();
                    Result_OBR_Id = objmanager.SaveResultOBRforSummary(lstresultobr);
                }

                #endregion

                #region ResultOBX

                IList<ResultOBX> lstresultobx = new List<ResultOBX>();
                lstresultobx = SummaryDTOList.ResultObxList;
                if (lstresultobx != null && lstresultobx.Count > 0)
                {
                    for (int i = 0; i < lstresultobx.Count; i++)
                    {
                        lstresultobx[i].Result_Master_ID = Result_Master_Id;
                        lstresultobx[i].Result_OBR_ID = Result_OBR_Id;
                    }
                    ResultOBXManager objmanager = new ResultOBXManager();
                    objmanager.SaveResultOBXforSummary(lstresultobx);
                }

                #endregion

                #region ResultORC

                IList<ResultORC> lstresultorc = new List<ResultORC>();
                lstresultorc = SummaryDTOList.ResultOrcList;
                if (lstresultorc != null && lstresultorc.Count > 0)
                {
                    for (int i = 0; i < lstresultorc.Count; i++)
                    {
                        lstresultorc[i].Result_Master_ID = Result_Master_Id;
                    }
                    ResultORCManager objmanager = new ResultORCManager();
                    objmanager.SaveResultORCforSummary(lstresultorc);
                }

                #endregion

                #region Assesment

                //IList<Assessment> lstassesment = new List<Assessment>();
                //lstassesment = SummaryDTOList.AssesmentList;
                //if (lstassesment.Count > 0)
                //{
                //    for (int i = 0; i < lstassesment.Count; i++)
                //    {
                //        lstassesment[i].Human_ID = Human_Id;
                //        lstassesment[i].Encounter_ID = Encounter_Id;
                //        lstassesment[i].Physician_ID = Physicican_Id;
                //    }
                //    AssessmentManager objmanager = new AssessmentManager();
                //    objmanager.SaveAssesmentforSummary(lstassesment);
                //}

                #endregion

                #region Allergy

                IList<Rcopia_Allergy> lstallergy = new List<Rcopia_Allergy>();
                lstallergy = SummaryDTOList.AllergyList;
                if (lstallergy != null && lstallergy.Count > 0)
                {
                    for (int i = 0; i < lstallergy.Count; i++)
                    {
                        lstallergy[i].Human_ID = Human_Id;
                    }
                }
                RCopiaGenerateXML objMedicationmanager = new RCopiaGenerateXML();
                objMedicationmanager.creteSendAllergyXml(lstallergy.ToArray(), null, null, sLegalOrg);

                #endregion

                #region Medication

                IList<Rcopia_Medication> lstmedication = new List<Rcopia_Medication>();
                lstmedication = SummaryDTOList.MedicationList;
                if (lstmedication != null && lstmedication.Count > 0)
                {
                    for (int i = 0; i < lstmedication.Count; i++)
                    {
                        lstmedication[i].Human_ID = Human_Id;
                    }
                }
                RCopiaGenerateXML objMedicationmanagerobjMedicationmanager = new RCopiaGenerateXML();
                objMedicationmanagerobjMedicationmanager.creteSendMedicationXml(lstmedication, null, null, sLegalOrg);
                RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
                Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();


                //objUpdateInfoMngr.DownloadRCopiaInfo(UserName, macAddress, DateTime.UtcNow, FacilityName, EncID);
                //objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.UploadAddress, UserName, macAddress, DateTime.UtcNow, FacilityName, EncID);

                //Patient Level RCopia Download - Commented
                objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.UploadAddress, UserName, macAddress, DateTime.UtcNow, FacilityName, EncID, Human_Id, sLegalOrg);
                #endregion



                #region GoalSection
                IList<TreatmentPlan> lsttreatmentgoal = new List<TreatmentPlan>();
                lsttreatmentgoal = SummaryDTOList.goalList;
                if (lsttreatmentgoal != null && lsttreatmentgoal.Count > 0)
                {
                    for (int i = 0; i < lsttreatmentgoal.Count; i++)
                    {
                        lsttreatmentgoal[i].Human_ID = Human_Id;
                        lsttreatmentgoal[i].Encounter_Id = Encounter_Id;
                        lsttreatmentgoal[i].Physician_Id = Physicican_Id;
                    }
                    TreatmentPlanManager objmanager = new TreatmentPlanManager();
                    objmanager.SaveTreatmentforSummary(lsttreatmentgoal);
                }
                #endregion


                #region Mentalstatus
                IList<CarePlan> lstcareplanmental = new List<CarePlan>();
                lstcareplanmental = SummaryDTOList.mentalstatuslist;
                if (lstcareplanmental != null && lstcareplanmental.Count > 0)
                {
                    for (int i = 0; i < lstcareplanmental.Count; i++)
                    {
                        lstcareplanmental[i].Human_ID = Human_Id;
                        lstcareplanmental[i].Encounter_ID = Encounter_Id;
                        lstcareplanmental[i].Physician_ID = Physicican_Id;
                    }
                    CarePlanManager objmanager = new CarePlanManager();
                    objmanager.SaveCarePlanforSummary(lstcareplanmental);
                }
                #endregion


                #region ImplantList
                IList<InHouseProcedure> lstcImplantList = new List<InHouseProcedure>();
                lstcImplantList = SummaryDTOList.ImplantList;
                if (lstcImplantList != null && lstcImplantList.Count > 0)
                {
                    for (int i = 0; i < lstcImplantList.Count; i++)
                    {
                        lstcImplantList[i].Human_ID = Human_Id;
                        lstcImplantList[i].Encounter_ID = Encounter_Id;
                        lstcImplantList[i].Physician_ID = Physicican_Id;
                    }
                    InHouseProcedureManager objmanager = new InHouseProcedureManager();
                    objmanager.SaveProceduresforSummary(lstcImplantList);
                }
                #endregion

                #region HealthCOncern
                IList<ProblemList> lstproblemlist = new List<ProblemList>();
                lstproblemlist = SummaryDTOList.healthconcernlist;
                if (lstproblemlist != null && lstproblemlist.Count > 0)
                {
                    for (int i = 0; i < lstproblemlist.Count; i++)
                    {
                        lstproblemlist[i].Human_ID = Human_Id;
                        lstproblemlist[i].Encounter_ID = Encounter_Id;
                        lstproblemlist[i].Physician_ID = Physicican_Id;
                    }
                    ProblemListManager objmanager = new ProblemListManager();
                    objmanager.SaveProblemListforSummary(lstproblemlist);
                }
                #endregion

                #region ActivityLog

                //IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                //ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                //ActivityLog activity = new ActivityLog();
                //activity.Activity_Type = "CCD Incorporated";
                //activity.Activity_Date_And_Time = Activity_log_time;
                //activity.Encounter_ID = Encounter_Id;
                //activity.Human_ID = Human_Id;
                //activity.Activity_By = UserName;
                //ActivityLogList.Add(activity);
                //ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);

                #endregion
                sreturn = Human_Id.ToString() + '_' + Encounter_Id.ToString();

                iMySession.Close();
            }

            return sreturn;
        }

        public void UpdateEncounterList(Encounter EncRecord, string MacAddress)
        {
            IList<Encounter> EncSaveList = null;
            IList<Encounter> EncUpdateList = new List<Encounter>();
            if (EncRecord.Id != 0)
            {
                EncUpdateList.Add(EncRecord);
                // SaveUpdateDeleteWithTransaction(ref EncSaveList, EncUpdateList, null, MacAddress);
                SaveUpdateDelete_DBAndXML_WithTransaction(ref EncSaveList, ref EncUpdateList, null, MacAddress, true, false, EncRecord.Id, string.Empty);
            }
        }

        public HumanDTO GetHumanDetailsbyApptDate(string FacilityName, string EncounterProviderId, string ApptDate)
        {
            HumanDTO Humandto = new HumanDTO();
            IList<Human> HumanList = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ISQLQuery sql = iMySession.CreateSQLQuery("SELECT {e.*},{w.*},{h.*} FROM encounter e, wf_object w,human h where e.facility_name='" + FacilityName + "' and e.encounter_provider_id='" + EncounterProviderId + "' and e.appointment_date like '" + ApptDate + "%' and w.current_process<>'READY_FOR_BILLING' AND e.encounter_id=w.obj_system_id and w.obj_type='ENCOUNTER' and h.human_id=e.human_id").AddEntity("e", typeof(Encounter)).AddEntity("w", typeof(WFObject)).AddEntity("h", typeof(Human));

                foreach (IList<Object> obj in sql.List())
                {
                    Humandto.HumanList.Add((Human)obj[2]);
                    Humandto.EncList.Add((Encounter)obj[0]);
                    Humandto.WFObjectList.Add((WFObject)obj[1]);
                }
                iMySession.Close();
            }
            return Humandto;
        }
        /// <summary>
        /// added for muehr 
        /// </summary>
        /// <param name="EncounterRecord"></param>
        /// <param name="MACAddress"></param>
        /// 


        //public void UpdateEncounterPaperOrder(IList<Encounter> EncounterRecord, string MACAddress)

        public IList<Encounter> UpdateEncounterPaperOrder(IList<Encounter> EncounterRecord, string MACAddress)
        {
            //<<<<<<< EncounterManager.cs
            //            IList<Encounter> EncList = null;            
            //            SaveUpdateDeleteWithTransaction(ref EncList, EncounterRecord, null, MACAddress);

            //||||||| 1.95.2.13
            //IList<Encounter> EncList = null;
            //IList<Encounter> EncListupdate = new List<Encounter>();        
            // EncListupdate.Add(EncounterRecord);
            // SaveUpdateDeleteWithTransaction(ref EncList, EncListupdate, null, MACAddress);

            //=======
            IList<Encounter> EncList = null;
            // SaveUpdateDeleteWithTransaction(ref EncList, EncounterRecord, null, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref EncList, ref EncounterRecord, null, MACAddress, true, false, EncounterRecord[0].Id, string.Empty);
            EncList = GetEncounterByEncounterID(EncounterRecord[0].Id);  //added for bug id 30643
            return EncList;
            //>>>>>>> 1.95.2.15
        }

        #region oldmethod
        //public IList<string> GetEncounterListArray(ulong ulHuman_ID)
        //{
        //    ISession iMySessiondesc = NHibernateSessionManager.Instance.CreateISession();
        //    IQuery query = iMySessiondesc.GetNamedQuery("GetEncounterandAllDetails");
        //    query.SetString(0, ulHuman_ID.ToString());
        //    ArrayList arrList = new ArrayList(query.List());
        //    IList<string> sDescList = new List<string>();
        //    foreach (object[] oj in arrList)
        //        sDescList.Add("Encounters" + "^" + oj[0].ToString() + "^" + oj[1].ToString() + ";" + oj[2].ToString());
        //    IQuery queryarc = iMySessiondesc.GetNamedQuery("GetEncounterandAllDetailsFromArchive");
        //    queryarc.SetString(0, ulHuman_ID.ToString());
        //    ArrayList arrList_arc = new ArrayList(queryarc.List());
        //    foreach (object[] oj in arrList_arc)
        //        sDescList.Add("Encounters" + "^" + oj[0].ToString() + "^" + oj[1].ToString() + ";" + oj[2].ToString());

        //    IList<PatientNotes> lstpatientnotes = new List<PatientNotes>();
        //    ISQLQuery sq = iMySessiondesc.CreateSQLQuery("SELECT p.*,w.* FROM patient_notes p left join wf_object w on (p.message_id=w.obj_system_id and w.obj_type='TASK') where w.current_process = 'CLOSE_TASK' and p.Human_Id='" + ulHuman_ID + "' and p.Is_PatientChart='Y' order by p.Modified_Date_And_Time Desc").AddEntity("p", typeof(PatientNotes)).AddEntity("w", typeof(WFObject));
        //    int k = 0;
        //    foreach (IList<object> l in sq.List())
        //    {
        //        lstpatientnotes.Add((PatientNotes)l[0]);
        //        sDescList.Add("Patient Task" + "^" + lstpatientnotes[k].Modified_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt") + "^" + lstpatientnotes[k].Id);
        //        k++;
        //    }
        //    IList<TreatmentPlan> TreatLst = new List<TreatmentPlan>();
        //    IList<Encounter> EncList = new List<Encounter>();
        //    Encounter objEnc = null;
        //    TreatmentPlan objPlan = null;
        //    ISQLQuery sqlquery = iMySessiondesc.CreateSQLQuery("select e.*,p.* from encounter e left join treatment_plan p on (e.encounter_id=p.encounter_id) where e.human_id='" + ulHuman_ID + "' and e.Is_phone_Encounter='Y' order by e.Date_of_service Desc").AddEntity("e", typeof(Encounter)).AddEntity("p", typeof(TreatmentPlan));
        //    foreach (IList<object> l in sqlquery.List())
        //    {
        //        objEnc = (Encounter)l[0];
        //        EncList.Add(objEnc);
        //        objPlan = (TreatmentPlan)l[1];
        //        TreatLst.Add(objPlan);
        //    }

        //    if (EncList != null && EncList.Count > 0 && TreatLst != null && TreatLst.Count > 0)
        //    {

        //        for (int n = 0; n < EncList.Count; n++)
        //        {
        //            TreatmentPlan objTrtPlan = new TreatmentPlan();
        //            if (TreatLst[0] != null)
        //            {
        //                objTrtPlan = (from p in TreatLst where p.Encounter_Id == EncList[n].Id select p).ToList<TreatmentPlan>()[0];
        //            }
        //            sDescList.Add("Phone Encounter" + "^" + objTrtPlan.Created_Date_And_Time.ToString() + "^" + objTrtPlan.Encounter_Id);
        //        }


        //    }
        //    IQuery querySummary = iMySessiondesc.GetNamedQuery("GetSummaryofCareDetails");
        //    querySummary.SetString(0, ulHuman_ID.ToString());
        //    ArrayList arrListSummary = new ArrayList(querySummary.List());
        //    foreach (object[] oj in arrListSummary)
        //        sDescList.Add("Summary of Care" + "^" + oj[0].ToString() + "^" + oj[1].ToString() + ";" + oj[2].ToString());
        //    FillResultMasterAndEntry ResultEntryDTO = new FillResultMasterAndEntry();
        //    ResultMasterManager resultMngr = new ResultMasterManager();
        //    ResultEntryDTO = resultMngr.GetResultUsingHumanId(ulHuman_ID);
        //    FileManagementIndexManager scanMngr = new FileManagementIndexManager();
        //    IList<FileManagementIndex> filelist = scanMngr.GetFileList(ulHuman_ID);
        //    IList<ResultMaster> result_master_list = new List<ResultMaster>();
        //    IList<FileManagementIndex> ilstscaninindex = new List<FileManagementIndex>();
        //    IList<ResultMaster> ilstresultmaster = new List<ResultMaster>();
        //    FillResultMasterAndEntry objResultMasterAndEntry = ResultEntryDTO;
        //    if (objResultMasterAndEntry.Result_Master_List.Count > 0)
        //        result_master_list = objResultMasterAndEntry.Result_Master_List;
        //    if (result_master_list.Count > 0)
        //    {
        //        ulong order_submit_id = 0;
        //        for (int i = 0; i < result_master_list.Count; i++)
        //        {
        //            if (result_master_list[i].Order_ID != 0)
        //            {
        //                if (result_master_list[i].Order_ID != order_submit_id)
        //                {
        //                    ilstresultmaster.Add(result_master_list[i]);
        //                    order_submit_id = result_master_list[i].Order_ID;
        //                }
        //            }
        //            else
        //            {
        //                ilstresultmaster.Add(result_master_list[i]);
        //                order_submit_id = result_master_list[i].Order_ID;
        //            }
        //        }
        //    }
        //    if (ilstresultmaster.Count > 0 && filelist.Count > 0)
        //    {
        //        ilstscaninindex = new List<FileManagementIndex>();
        //        for (int i = 0; i < filelist.Count; i++)
        //        {
        //            bool blScan = false;
        //            for (int j = 0; j < ilstresultmaster.Count; j++)
        //            {
        //                if (ilstresultmaster[j].Order_ID != 0)
        //                {
        //                    if (filelist[i].Order_ID == ilstresultmaster[j].Order_ID)
        //                    {
        //                        ilstresultmaster[j].Is_Scan_order = "Y";
        //                        blScan = true;
        //                        break;
        //                    }
        //                }
        //            }
        //            if (blScan == false)
        //            {
        //                if (filelist[i].Order_ID == 0 && filelist[i].Document_Sub_Type != "Lab Result Form")
        //                {
        //                    ilstscaninindex.Add(filelist[i]);
        //                }
        //                else if (filelist[i].Order_ID != 0)
        //                {
        //                    ilstscaninindex.Add(filelist[i]);
        //                }

        //            }
        //        }
        //    }
        //    else if ((ilstresultmaster.Count == 0 && filelist.Count > 0))
        //    {
        //        for (int i = 0; i < filelist.Count; i++)
        //        {
        //            ilstscaninindex.Add(filelist[i]);
        //        }
        //    }

        //    if (ilstresultmaster.Count > 0)
        //        ilstresultmaster = ilstresultmaster.OrderByDescending(a => a.Created_Date_And_Time).ToList<ResultMaster>();
        //    if (ilstscaninindex.Count > 0)
        //        ilstscaninindex = ilstscaninindex.OrderByDescending(a => a.Order_ID).ToList<FileManagementIndex>();

        //    for (int i = 0; i < ilstresultmaster.Count; i++)
        //    {
        //        if (ilstresultmaster[i].MSH_Date_And_Time_Of_Message != string.Empty)
        //        {
        //            string formatString = "yyyyMMddHHmmss";
        //            string sample = ilstresultmaster[i].MSH_Date_And_Time_Of_Message;
        //            if (sample.Contains('-'))
        //            {
        //                sample = sample.Split('-')[0].ToString();
        //            }
        //            if (sample.Length == 14)
        //                formatString = "yyyyMMddHHmmss";
        //            else if (sample.Length == 12)
        //                formatString = "yyyyMMddHHmm";

        //            DateTime dt = DateTime.ParseExact(sample, formatString, null);
        //            sDescList.Add("Others" + "^" + ilstresultmaster[i].Id + "^" + dt + "^Laboratory^" + ilstresultmaster[i].Orders_Description + "^Results^" + ilstresultmaster[i].Order_ID);
        //        }
        //    }
        //    //ulong ord_sb_id = 0;
        //    for (int i = 0; i < ilstscaninindex.Count; i++)
        //    {
        //        if (ilstscaninindex[i].Order_ID != 0)
        //        {

        //            IList<OrdersSubmit> ordersList = new List<OrdersSubmit>();
        //            ICriteria crit = iMySessiondesc.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", ilstscaninindex[i].Order_ID));
        //            ordersList = crit.List<OrdersSubmit>();
        //            if (ordersList.Count > 0)
        //            {
        //                if (ordersList[0].Specimen_Collection_Date_And_Time == DateTime.MinValue)
        //                {
        //                    ilstscaninindex[i].Created_Date_And_Time = ordersList[0].Created_Date_And_Time;
        //                }
        //                else
        //                {
        //                    ilstscaninindex[i].Created_Date_And_Time = ordersList[0].Specimen_Collection_Date_And_Time;
        //                }
        //            }
        //          //  sDescList.Add("Results" + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Created_Date_And_Time + "|" + ilstscaninindex[i].Id + "|" + ilstscaninindex[i].Orders_Description); // ilstresultmaster[i].Created_Date_And_Time + "^" + ilstresultmaster[i].Orders_Description + "^" + ilstresultmaster[i].Order_ID);
        //            //ord_sb_id = ilstscaninindex[i].Order_ID;
        //        }
        //        //if (ilstscaninindex[i].Order_ID != 0)
        //        //{
        //        //    //if (ord_sb_id != ilstscaninindex[i].Order_ID)
        //        //    //{
        //        //    sDescList.Add("Results" + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Document_Date + "|" + ilstscaninindex[i].Id); // ilstresultmaster[i].Created_Date_And_Time + "^" + ilstresultmaster[i].Orders_Description + "^" + ilstresultmaster[i].Order_ID);
        //        //    ord_sb_id = ilstscaninindex[i].Order_ID;
        //        //    //}
        //        //}

        //    }

        //    for (int i = 0; i < ilstscaninindex.Count; i++)
        //    {
        //        //if (ilstscaninindex[i].Order_ID == 0)
        //        //{
        //            if (ilstscaninindex[i].Source == "SCAN")
        //            {
        //                if (ilstscaninindex[i].Document_Type.ToUpper() == "ENCOUNTERS")
        //                {
        //                    if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
        //                    {
        //                        string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
        //                        string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
        //                        string[] name = temp_file_name.Split('.');
        //                        string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
        //                        sDescList.Add("EncountersSub" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path);
        //                    }
        //                }
        //                else
        //                {
        //                    if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
        //                    {
        //                        string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
        //                        string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
        //                        string[] name = temp_file_name.Split('.');
        //                        string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
        //                        if(ilstscaninindex[i].Order_ID==0)
        //                        sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type);
        //                        else
        //                            sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type+"^"+ilstscaninindex[i].Order_ID+"^"+ilstscaninindex[i].Orders_Description);
        //                    }

        //                }
        //            }
        //        //}
        //    }

        //    iMySessiondesc.Close();
        //    return sDescList;
        //}
        #endregion

        //public IList<string> GetEncounterListArray(ulong ulHuman_ID)
        //{
        //    ISession iMySessiondesc = NHibernateSessionManager.Instance.CreateISession();
        //    IQuery query = iMySessiondesc.GetNamedQuery("GetEncounterandAllDetails");
        //    query.SetString(0, ulHuman_ID.ToString());
        //    query.SetString(1, ulHuman_ID.ToString());
        //    ArrayList arrList = new ArrayList(query.List());
        //    IList<string> sDescList = new List<string>();
        //    foreach (object[] oj in arrList)
        //        sDescList.Add("Encounters" + "^" + oj[0].ToString() + "^" + oj[1].ToString() + ";" + oj[2].ToString());
        //    //IQuery queryarc = iMySessiondesc.GetNamedQuery("GetEncounterandAllDetailsFromArchive");
        //    //queryarc.SetString(0, ulHuman_ID.ToString());
        //    //ArrayList arrList_arc = new ArrayList(queryarc.List());
        //    //foreach (object[] oj in arrList_arc)
        //    //    sDescList.Add("Encounters" + "^" + oj[0].ToString() + "^" + oj[1].ToString() + ";" + oj[2].ToString());

        //    //bug Id:67990 
        //    //  ISQLQuery sq = iMySessiondesc.CreateSQLQuery("SELECT p.*,w.* FROM patient_notes p left join wf_object w on (p.message_id=w.obj_system_id and w.obj_type='TASK') where w.current_process = 'CLOSE_TASK' and p.Human_Id='" + ulHuman_ID + "' and p.Is_PatientChart='Y' order by p.Modified_Date_And_Time Desc").AddEntity("p", typeof(PatientNotes)).AddEntity("w", typeof(WFObject));
        //   // IList<PatientNotes> lst = new List<PatientNotes>();

        //  //  ISQLQuery sq = iMySessiondesc.CreateSQLQuery("SELECT p.* FROM patient_notes p where p.Human_Id='" + ulHuman_ID + "' and p.Is_PatientChart='Y' order by p.Modified_Date_And_Time Desc").AddEntity("p", typeof(PatientNotes));
        //    ArrayList lst = new ArrayList();
        //    ISQLQuery sq = iMySessiondesc.CreateSQLQuery("SELECT p.Message_ID,cast(p.Modified_Date_And_Time as char(50)) FROM patient_notes p where p.Human_Id='" + ulHuman_ID + "' and p.Is_PatientChart='Y' order by p.Modified_Date_And_Time Desc");
        //   // lst = sq.List<PatientNotes>();
        //     arrList = new ArrayList(sq.List());
        //     for (int i = 0; i < arrList.Count; i++)
        //     {
        //         object[] oj = (object[])arrList[i];
        //         sDescList.Add("Patient Task" + "^" + Convert.ToDateTime(oj[1]).ToString("dd-MMM-yyyy hh:mm tt") + "^" + oj[0]);

        //     }
        //     //ISQLQuery sql;
        //     //sql = iMySession.CreateSQLQuery("select os.Lab_Name,concat (h.First_Name,' ',h.Last_Name) as human_name from orders_submit os left join human h on h.human_id = os.human_id where os.order_submit_id ='" + ordersubmitid + "'");
        //     //ArrayList arr = new ArrayList(sql.List());
        //     //if (arr.Count > 0)
        //     //{
        //     //    for (int i = 0; i < arr.Count; i++)
        //     //    {
        //     //        object[] obj = (object[])arr[i];
        //     //        result = obj[0].ToString() + "|" + obj[1].ToString();
        //     //    }
        //     //}
        //    //  int k = 0;
        //    // foreach (IList<object> l in sq.List())
        //    //for (int k = 0; k < lst.Count; k++)
        //    //{
        //    //    //lstpatientnotes.Add((PatientNotes)l[0]);
        //    //   // sDescList.Add("Patient Task" + "^" + lst[k].Modified_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt") + "^" + lst[k].Id);
        //    //    // k++;
        //    //}
        //    IList<TreatmentPlan> TreatLst = new List<TreatmentPlan>();
        //    IList<Encounter> EncList = new List<Encounter>();
        //    Encounter objEnc = null;
        //    TreatmentPlan objPlan = null;
        //    ISQLQuery sqlquery = iMySessiondesc.CreateSQLQuery("select e.*,p.* from encounter e left join treatment_plan p on (e.encounter_id=p.encounter_id) where e.human_id='" + ulHuman_ID + "' and e.Is_phone_Encounter='Y' order by e.Date_of_service Desc").AddEntity("e", typeof(Encounter)).AddEntity("p", typeof(TreatmentPlan));
        //    foreach (IList<object> l in sqlquery.List())
        //    {
        //        objEnc = (Encounter)l[0];
        //        EncList.Add(objEnc);
        //        objPlan = (TreatmentPlan)l[1];
        //        TreatLst.Add(objPlan);
        //    }

        //    if (EncList != null && EncList.Count > 0 && TreatLst != null && TreatLst.Count > 0)
        //    {

        //        for (int n = 0; n < EncList.Count; n++)
        //        {
        //            //Commented For Bug ID 55373
        //            //TreatmentPlan objTrtPlan = new TreatmentPlan();
        //            //if (TreatLst[0] != null)
        //            //{
        //            //    objTrtPlan = (from p in TreatLst where p.Encounter_Id == EncList[n].Id select p).ToList<TreatmentPlan>()[0];
        //            //}
        //            //sDescList.Add("Phone Encounter" + "^" + objTrtPlan.Created_Date_And_Time.ToString() + "^" + objTrtPlan.Encounter_Id);

        //            sDescList.Add("Phone Encounter" + "^" + EncList[n].Date_of_Service.ToString() + "^" + EncList[n].Id);
        //        }
        //    }
        //    IQuery querySummary = iMySessiondesc.GetNamedQuery("GetSummaryofCareDetails");
        //    querySummary.SetString(0, ulHuman_ID.ToString());
        //    ArrayList arrListSummary = new ArrayList(querySummary.List());
        //    foreach (object[] oj in arrListSummary)
        //        sDescList.Add("Summary of Care" + "^" + oj[0].ToString() + "^" + oj[1].ToString() + ";" + oj[2].ToString());
        //    FillResultMasterAndEntry ResultEntryDTO = new FillResultMasterAndEntry();
        //    ResultMasterManager resultMngr = new ResultMasterManager();
        //    ResultEntryDTO = resultMngr.GetResultUsingHumanId(ulHuman_ID);
        //    FileManagementIndexManager scanMngr = new FileManagementIndexManager();
        //    IList<FileManagementIndex> filelist = scanMngr.GetFileList(ulHuman_ID);
        //    IList<ResultMaster> result_master_list = new List<ResultMaster>();
        //    IList<FileManagementIndex> ilstscaninindex = new List<FileManagementIndex>();
        //    IList<ResultMaster> ilstresultmaster = new List<ResultMaster>();
        //    FillResultMasterAndEntry objResultMasterAndEntry = ResultEntryDTO;
        //    int cntDR_EncounterList = 1;
        //    if (objResultMasterAndEntry.Result_Master_List.Count > 0)
        //        result_master_list = objResultMasterAndEntry.Result_Master_List;
        //    if (result_master_list.Count > 0)
        //    {
        //        ulong order_submit_id = 0;
        //        for (int i = 0; i < result_master_list.Count; i++)
        //        {
        //            if (result_master_list[i].Order_ID != 0)
        //            {
        //                if (result_master_list[i].Order_ID != order_submit_id)
        //                {
        //                    ilstresultmaster.Add(result_master_list[i]);
        //                    order_submit_id = result_master_list[i].Order_ID;
        //                }
        //            }
        //            else
        //            {
        //                ilstresultmaster.Add(result_master_list[i]);
        //                order_submit_id = result_master_list[i].Order_ID;
        //            }
        //        }
        //    }
        //    if (ilstresultmaster.Count > 0 && filelist.Count > 0)
        //    {
        //        ilstscaninindex = new List<FileManagementIndex>();
        //        for (int i = 0; i < filelist.Count; i++)
        //        {
        //            bool blScan = false;
        //            for (int j = 0; j < ilstresultmaster.Count; j++)
        //            {
        //                if (ilstresultmaster[j].Order_ID != 0)
        //                {
        //                    if (filelist[i].Order_ID == ilstresultmaster[j].Order_ID)
        //                    {
        //                        ilstresultmaster[j].Is_Scan_order = "Y";
        //                        blScan = true;
        //                        break;
        //                    }
        //                }
        //            }
        //            //BugID:49662
        //            //if (blScan == false)
        //            //{
        //            if (filelist[i].Order_ID == 0 && filelist[i].Document_Sub_Type != "Lab Result Form")
        //            {
        //                ilstscaninindex.Add(filelist[i]);
        //            }
        //            else if (filelist[i].Order_ID != 0)
        //            {
        //                ilstscaninindex.Add(filelist[i]);
        //            }

        //            //}
        //        }
        //    }
        //    else if ((ilstresultmaster.Count == 0 && filelist.Count > 0))
        //    {
        //        for (int i = 0; i < filelist.Count; i++)
        //        {
        //            ilstscaninindex.Add(filelist[i]);
        //        }
        //    }

        //    if (ilstresultmaster.Count > 0)
        //        ilstresultmaster = ilstresultmaster.OrderByDescending(a => a.Created_Date_And_Time).ToList<ResultMaster>();
        //    if (ilstscaninindex.Count > 0)
        //        ilstscaninindex = ilstscaninindex.OrderByDescending(a => a.Order_ID).ToList<FileManagementIndex>();

        //    for (int i = 0; i < ilstresultmaster.Count; i++)
        //    {
        //        if (ilstresultmaster[i].OBR_Specimen_Collected_Date_And_Time != string.Empty)//BugID:47602
        //        {
        //            string formatString = "yyyyMMddHHmmss";
        //            string sample = ilstresultmaster[i].OBR_Specimen_Collected_Date_And_Time;//BugID:47602
        //            if (sample.Contains('-'))
        //            {
        //                sample = sample.Split('-')[0].ToString();
        //            }
        //            if (sample.Length == 14)
        //                formatString = "yyyyMMddHHmmss";
        //            else if (sample.Length == 12)
        //                formatString = "yyyyMMddHHmm";

        //            DateTime dt = DateTime.ParseExact(sample, formatString, null);
        //            sDescList.Add("Others" + "^" + ilstresultmaster[i].Id + "^" + dt + "^Laboratory^" + ilstresultmaster[i].Orders_Description + "^Results^" + ilstresultmaster[i].Order_ID);
        //        }
        //    }
        //    //ulong ord_sb_id = 0;
        //    for (int i = 0; i < ilstscaninindex.Count; i++)
        //    {
        //        if (ilstscaninindex[i].Order_ID != 0)
        //        {  //BugID: 62511

        //            //IList<OrdersSubmit> ordersList = new List<OrdersSubmit>();
        //            //ICriteria crit = iMySessiondesc.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", ilstscaninindex[i].Order_ID));
        //            //ordersList = crit.List<OrdersSubmit>();
        //            //if (ordersList.Count > 0)
        //            //{
        //            //if (ordersList[0].Bill_Type.Trim() != string.Empty) //BugID:42001 ,41884
        //            //{
        //            //    ilstscaninindex[i].Created_Date_And_Time = ordersList[0].Created_Date_And_Time;//Results entered from diagnostic_order screen
        //            //}
        //            //else
        //            //{
        //            ilstscaninindex[i].Created_Date_And_Time = ilstscaninindex[i].Document_Date;//Results uploaded from upload scan documents screen
        //            //}
        //            //if (ordersList[0].Specimen_Collection_Date_And_Time == DateTime.MinValue)
        //            //{
        //            //    ilstscaninindex[i].Created_Date_And_Time = ordersList[0].Created_Date_And_Time;
        //            //}
        //            //else
        //            //{
        //            //    ilstscaninindex[i].Created_Date_And_Time = ordersList[0].Specimen_Collection_Date_And_Time;
        //            //}
        //            //}
        //            //  sDescList.Add("Results" + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Created_Date_And_Time + "|" + ilstscaninindex[i].Id + "|" + ilstscaninindex[i].Orders_Description); // ilstresultmaster[i].Created_Date_And_Time + "^" + ilstresultmaster[i].Orders_Description + "^" + ilstresultmaster[i].Order_ID);
        //            //ord_sb_id = ilstscaninindex[i].Order_ID;
        //        }
        //        //if (ilstscaninindex[i].Order_ID != 0)
        //        //{
        //        //    //if (ord_sb_id != ilstscaninindex[i].Order_ID)
        //        //    //{
        //        //    sDescList.Add("Results" + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Document_Date + "|" + ilstscaninindex[i].Id); // ilstresultmaster[i].Created_Date_And_Time + "^" + ilstresultmaster[i].Orders_Description + "^" + ilstresultmaster[i].Order_ID);
        //        //    ord_sb_id = ilstscaninindex[i].Order_ID;
        //        //    //}
        //        //}

        //    }

        //    for (int i = 0; i < ilstscaninindex.Count; i++)
        //    {
        //        //if (ilstscaninindex[i].Order_ID == 0)
        //        //{
        //        ilstscaninindex = ilstscaninindex.OrderBy(a => a.Appointment_Date).ToList<FileManagementIndex>();
        //        if (ilstscaninindex[i].Source.ToUpper() == "ENCOUNTER_XML_DR")
        //        {
        //            if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
        //            {
        //                string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + (cntDR_EncounterList < 10 ? "0" + cntDR_EncounterList.ToString() : cntDR_EncounterList.ToString());
        //                sDescList.Add("Others" + "^" + ilstscaninindex[i].Encounter_ID + "^" + ilstscaninindex[i].Appointment_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Result_Master_ID);
        //                cntDR_EncounterList++;
        //            }
        //        }
        //        else if (ilstscaninindex[i].Source == "SCAN" || ilstscaninindex[i].Source == "ELIGIBILITY VERIFICATION")//BugID:55156
        //        {
        //            if (ilstscaninindex[i].Document_Type.ToUpper() == "ENCOUNTERS")
        //            {
        //                if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
        //                {
        //                    string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
        //                    string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
        //                    string[] name = temp_file_name.Split('.');
        //                    string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
        //                    sDescList.Add("EncountersSub" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Result_Master_ID);
        //                }
        //            }
        //            else
        //            {
        //                if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
        //                {
        //                    string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
        //                    string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
        //                    string[] name = temp_file_name.Split('.');
        //                    string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
        //                    if (ilstscaninindex[i].Order_ID == 0)
        //                        sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Result_Master_ID);
        //                    else //modified for integrum BugID:54261 & 55094
        //                        //sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Orders_Description + "$Dateonly" + "^" + ilstscaninindex[i].Result_Master_ID);//$Dateonly For showing document date only.
        //                        sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Result_Master_ID + "^" + ilstscaninindex[i].Orders_Description + "$Dateonly" + "^" + ilstscaninindex[i].Order_ID);//$Dateonly For showing document date only.

        //                }

        //            }
        //        }
        //        else if (ilstscaninindex[i].Source == "ORDER")
        //        {
        //            if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
        //            {
        //                string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
        //                string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
        //                string[] name = temp_file_name.Split('.');
        //                ilstscaninindex[i].Document_Sub_Type = "Laboratory";
        //                ilstscaninindex[i].Document_Type = "Results";
        //                string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
        //                if (ilstscaninindex[i].Order_ID == 0)
        //                    sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type);
        //                else
        //                    sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Orders_Description);
        //            }

        //        }
        //        else if (ilstscaninindex[i].Source == "EXAM")
        //        {
        //            if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
        //            {
        //                string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
        //                string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
        //                string[] name = temp_file_name.Split('.');
        //                //ilstscaninindex[i].Document_Sub_Type = "Laboratory";
        //                //   ilstscaninindex[i].Document_Type = "Results";
        //                string file_name = ilstscaninindex[i].Document_Type + "_" + name[0];
        //                if (ilstscaninindex[i].Order_ID == 0)
        //                    sDescList.Add("Exam" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type);
        //                else
        //                    sDescList.Add("Exam" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Orders_Description);
        //            }

        //        }

        //        //}
        //    }

        //    iMySessiondesc.Close();
        //    return sDescList;
        //}



        public IList<string> GetEncounterListArray(ulong ulHuman_ID, string sCategory, string sInterpretationSubDocType)
        {
            ISession iMySessiondesc = NHibernateSessionManager.Instance.CreateISession();
            ArrayList arrList;
            IList<string> sDescList = new List<string>();
            if (sCategory == "Encounters")
            {
                IQuery query = iMySessiondesc.GetNamedQuery("GetEncounterandAllDetails");
                query.SetString(0, ulHuman_ID.ToString());
                query.SetString(1, ulHuman_ID.ToString());
                arrList = new ArrayList(query.List());
                foreach (object[] oj in arrList)
                    sDescList.Add("Encounters" + "^" + oj[0].ToString() + "^" + oj[1].ToString() + ";" + oj[2].ToString() + "^" + oj[3].ToString());
            }

            if (sCategory == "Patient Task")
            {
                ArrayList lst = new ArrayList();
                //Old Code
                //ISQLQuery sq = iMySessiondesc.CreateSQLQuery("SELECT p.Message_ID,cast(p.Modified_Date_And_Time as char(50)) FROM patient_notes p where p.Human_Id='" + ulHuman_ID + "' and p.Is_PatientChart='Y' order by p.Modified_Date_And_Time Desc");
                //Gitlab# 2685 - Visible “add to patient chart” checkbox
                ISQLQuery sq = iMySessiondesc.CreateSQLQuery("SELECT p.Message_ID,cast(if(p.Modified_Date_And_Time='0001-01-01 00:00:00',p.created_date_and_time,p.Modified_Date_And_Time) as char(50)) FROM patient_notes p where p.Human_Id='" + ulHuman_ID + "' and p.Is_PatientChart='Y' order by p.Modified_Date_And_Time Desc");
                arrList = new ArrayList(sq.List());
                for (int i = 0; i < arrList.Count; i++)
                {
                    object[] oj = (object[])arrList[i];
                    sDescList.Add("Patient Task" + "^" + Convert.ToDateTime(oj[1]).ToString("dd-MMM-yyyy hh:mm tt") + "^" + oj[0]);

                }
            }

            if (sCategory == "Encounters")
            {
                IList<TreatmentPlan> TreatLst = new List<TreatmentPlan>();
                IList<Encounter> EncList = new List<Encounter>();
                Encounter objEnc = null;
                TreatmentPlan objPlan = null;
                ISQLQuery sqlquery = iMySessiondesc.CreateSQLQuery("select e.*,p.* from encounter e left join treatment_plan p on (e.encounter_id=p.encounter_id) where e.human_id='" + ulHuman_ID + "' and e.Is_phone_Encounter='Y' and e.Reason_for_Cancelation=''    union select e.*,p.* from encounter_arc e left join treatment_plan_arc p on (e.encounter_id=p.encounter_id) where e.human_id='" + ulHuman_ID + "' and e.Is_phone_Encounter='Y' and e.Reason_for_Cancelation='' ").AddEntity("e", typeof(Encounter)).AddEntity("p", typeof(TreatmentPlan));
                var queryoutput = sqlquery.List();

                foreach (IList<object> l in sqlquery.List())
                {
                    objEnc = (Encounter)l[0];
                    EncList.Add(objEnc);

                    EncList = EncList.OrderByDescending(o => o.Date_of_Service).ToList<Encounter>();
                    objPlan = (TreatmentPlan)l[1];
                    TreatLst.Add(objPlan);
                }

                if (EncList != null && EncList.Count > 0 && TreatLst != null && TreatLst.Count > 0)
                {
                    for (int n = 0; n < EncList.Count; n++)
                    {
                        sDescList.Add("Phone Encounter" + "^" + EncList[n].Date_of_Service.ToString() + "^" + EncList[n].Id);
                    }
                }
            }

            if (sCategory == "Summary of Care")
            {
                IQuery querySummary = iMySessiondesc.GetNamedQuery("GetSummaryofCareDetails");
                querySummary.SetString(0, ulHuman_ID.ToString());
                ArrayList arrListSummary = new ArrayList(querySummary.List());
                foreach (object[] oj in arrListSummary)
                    sDescList.Add("Summary of Care" + "^" + oj[0].ToString() + "^" + oj[1].ToString() + ";" + oj[2].ToString());
            }

            if (sCategory == "Results")
            {
                FillResultMasterAndEntry ResultEntryDTO = new FillResultMasterAndEntry();
                ResultMasterManager resultMngr = new ResultMasterManager();
                ResultEntryDTO = resultMngr.GetResultUsingHumanId(ulHuman_ID);
                FillResultMasterAndEntry objResultMasterAndEntry = ResultEntryDTO;
                IList<ResultMaster> ilstresultmaster = new List<ResultMaster>();
                IList<ResultMaster> result_master_list = new List<ResultMaster>();
                if (objResultMasterAndEntry.Result_Master_List.Count > 0)
                    result_master_list = objResultMasterAndEntry.Result_Master_List;
                if (result_master_list.Count > 0)
                {
                    ulong order_submit_id = 0;
                    for (int i = 0; i < result_master_list.Count; i++)
                    {
                        if (result_master_list[i].Order_ID != 0)
                        {
                            if (result_master_list[i].Order_ID != order_submit_id)
                            {
                                ilstresultmaster.Add(result_master_list[i]);
                                order_submit_id = result_master_list[i].Order_ID;
                            }
                        }
                        else
                        {
                            ilstresultmaster.Add(result_master_list[i]);
                            order_submit_id = result_master_list[i].Order_ID;
                        }
                    }
                }

                if (ilstresultmaster.Count > 0)
                    ilstresultmaster = ilstresultmaster.OrderByDescending(a => a.Created_Date_And_Time).ToList<ResultMaster>();

                for (int i = 0; i < ilstresultmaster.Count; i++)
                {
                    if (ilstresultmaster[i].OBR_Specimen_Collected_Date_And_Time != string.Empty)//BugID:47602
                    {
                        string formatString = "yyyyMMddHHmmss";
                        string sample = ilstresultmaster[i].OBR_Specimen_Collected_Date_And_Time;//BugID:47602
                        if (sample.Contains('-'))
                        {
                            sample = sample.Split('-')[0].ToString();
                        }
                        if (sample.Length == 14)
                            formatString = "yyyyMMddHHmmss";
                        else if (sample.Length == 12)
                            formatString = "yyyyMMddHHmm";

                        DateTime dt = DateTime.ParseExact(sample, formatString, null);
                        sDescList.Add("Others" + "^" + ilstresultmaster[i].Id + "^" + dt + "^Laboratory^" + ilstresultmaster[i].Orders_Description + "^Results^" + ilstresultmaster[i].Order_ID);
                    }
                }
            }

            if (sCategory == "Document" || sCategory == "Exam" || sCategory == "Results" || sCategory == "Encounters")
            {
                FileManagementIndexManager scanMngr = new FileManagementIndexManager();

                IList<FileManagementIndex> filelist = null;

                if (sCategory == "Encounters")
                    filelist = scanMngr.GetEncountersFileList(ulHuman_ID);
                else
                    filelist = scanMngr.GetFileList(ulHuman_ID);

                IList<FileManagementIndex> ilstscaninindex = new List<FileManagementIndex>();

                int cntDR_EncounterList = 1;

                if (filelist.Count > 0)
                {
                    ilstscaninindex = new List<FileManagementIndex>();
                    for (int i = 0; i < filelist.Count; i++)
                    {
                        if (filelist[i].Order_ID == 0 && filelist[i].Document_Sub_Type != "Lab Result Form")
                        {
                            ilstscaninindex.Add(filelist[i]);
                        }
                        else if (filelist[i].Order_ID != 0)
                        {
                            ilstscaninindex.Add(filelist[i]);
                        }
                    }

                    //Interpretation Notes
                    string[] sTempNew = sInterpretationSubDocType.Split('|').ToArray();
                    IList<FileManagementIndex> InterpretationNotes = (from intnotes in filelist where sTempNew.Contains(intnotes.Document_Sub_Type.ToUpper()) select intnotes).ToList<FileManagementIndex>();
                    string newList = string.Join(",", sInterpretationSubDocType.Split('|').Select(x => string.Format("'{0}'", x)).ToArray());

                    // string svalue = string.Join(",", EncLst.Cast<string>().ToArray());

                    if (InterpretationNotes.Count() > 0)//Check if filelist contains any one of the item in sInterpretationSubDocType
                    {

                        // ISQLQuery sqlquery = iMySessiondesc.CreateSQLQuery("select * from file_management_index f, wf_object w where f.human_id='" + ulHuman_ID + "' and f.doc_sub_type in (" + newList + ") and f.order_id=w.obj_system_id and w.obj_type='DIAGNOSTIC ORDER' and (w.current_process='BILLING_WAIT' OR w.current_process='BILLING_COMPLETE') and f.result_master_id<>0").AddEntity("f", typeof(FileManagementIndex));

                        //ISQLQuery sqlquery = iMySessiondesc.CreateSQLQuery("select * from file_management_index f, wf_object w where f.human_id='" + ulHuman_ID + "' and f.Is_Delete<>'Y'  and f.doc_sub_type in (" + newList + ")  and f.order_id=w.obj_system_id and w.obj_type='DIAGNOSTIC ORDER' and (w.current_process='BILLING_WAIT' OR w.current_process='BILLING_COMPLETE') and f.result_master_id<>0 "
                        //+ "union all  select * from file_management_index f, wf_object_arc wa where f.human_id='" + ulHuman_ID + "' and f.Is_Delete<>'Y'  and f.doc_sub_type in (" + newList + ")  and f.order_id=wa.obj_system_id and wa.obj_type='DIAGNOSTIC ORDER' and (wa.current_process='BILLING_WAIT' OR wa.current_process='BILLING_COMPLETE') and f.result_master_id<>0").AddEntity("f", typeof(FileManagementIndex));
                        ISQLQuery sqlquery = iMySessiondesc.CreateSQLQuery("select * from file_management_index f left join wf_object w on (f.order_id=w.obj_system_id and w.obj_type='DIAGNOSTIC ORDER') "
+ "left join result_master m on (f.result_master_id=m.result_master_id) where f.human_id='" + ulHuman_ID + "' and f.Is_Delete<>'Y'  and f.doc_sub_type in (" + newList + ") and  (w.current_process='BILLING_WAIT' OR w.current_process='BILLING_COMPLETE')  and f.result_master_id<>0 "
 + "and m.Result_Review_Comments like '%Test Reviewed: %' union all  select * from file_management_index f left join wf_object_arc w on (f.order_id=w.obj_system_id and w.obj_type='DIAGNOSTIC ORDER') "
+ "left join result_master m on (f.result_master_id=m.result_master_id) where f.human_id='" + ulHuman_ID + "' and f.Is_Delete<>'Y' "
 + "and f.doc_sub_type in (" + newList + ")  and  (w.current_process='BILLING_WAIT' OR w.current_process='BILLING_COMPLETE') and f.result_master_id<>0  and m.Result_Review_Comments like '%Test Reviewed: %'").AddEntity("f", typeof(FileManagementIndex));

                        IList<FileManagementIndex> queryoutput = sqlquery.List<FileManagementIndex>();

                        for (int i = 0; i < queryoutput.Count; i++)
                        {
                            //sDescList.Add("StressTestReport" + "^" + queryoutput[i].Id + "^" + queryoutput[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + "Stress Test Signed Report" + "^" + queryoutput[i].File_Path + "^" + queryoutput[i].Document_Type + "^" + queryoutput[i].Order_ID + "^" + queryoutput[i].Result_Master_ID);
                            sDescList.Add("StressTestReport" + "^" + queryoutput[i].Id + "^" + queryoutput[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + queryoutput[i].Document_Sub_Type + " Signed Report" + "^" + queryoutput[i].File_Path + "^" + queryoutput[i].Document_Type + "^" + queryoutput[i].Order_ID + "^" + queryoutput[i].Result_Master_ID);

                        }
                    }
                }

                if (ilstscaninindex.Count > 0)
                    ilstscaninindex = ilstscaninindex.OrderByDescending(a => a.Order_ID).ToList<FileManagementIndex>();


                //ulong ord_sb_id = 0;
                for (int i = 0; i < ilstscaninindex.Count; i++)
                {
                    if (ilstscaninindex[i].Order_ID != 0)
                    {
                        ilstscaninindex[i].Created_Date_And_Time = ilstscaninindex[i].Document_Date;//Results uploaded from upload scan documents screen
                    }
                }

                for (int i = 0; i < ilstscaninindex.Count; i++)
                {
                    ilstscaninindex = ilstscaninindex.OrderBy(a => a.Appointment_Date).ToList<FileManagementIndex>();
                    if (ilstscaninindex[i].Source.ToUpper() == "ENCOUNTER_XML_DR")
                    {
                        if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
                        {
                            string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + (cntDR_EncounterList < 10 ? "0" + cntDR_EncounterList.ToString() : cntDR_EncounterList.ToString());
                            sDescList.Add("Others" + "^" + ilstscaninindex[i].Encounter_ID + "^" + ilstscaninindex[i].Appointment_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Result_Master_ID);
                            cntDR_EncounterList++;
                        }
                    }
                    else if (ilstscaninindex[i].Source == "SCAN" || ilstscaninindex[i].Source == "ELIGIBILITY VERIFICATION")//BugID:55156
                    {
                        if (ilstscaninindex[i].Document_Type.ToUpper() == "ENCOUNTERS")
                        {
                            if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
                            {
                                string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
                                string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
                                string[] name = temp_file_name.Split('.');
                                string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
                                sDescList.Add("EncountersSub" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Result_Master_ID + "^" + ilstscaninindex[i].Encounter_ID);
                            }
                        }
                        else
                        {
                            if (ilstscaninindex[i].Document_Type.ToUpper() == "RESULTS" && sCategory == "Document")
                            {
                                //Do Nothing
                            }
                            else
                            {
                                if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
                                {
                                    string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
                                    string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
                                    string[] name = temp_file_name.Split('.');
                                    string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
                                    if (ilstscaninindex[i].Order_ID == 0)
                                        sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Result_Master_ID);
                                    else //modified for integrum BugID:54261 & 55094
                                        //sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Orders_Description + "$Dateonly" + "^" + ilstscaninindex[i].Result_Master_ID);//$Dateonly For showing document date only.
                                        sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Result_Master_ID + "^" + ilstscaninindex[i].Orders_Description + "$Dateonly" + "^" + ilstscaninindex[i].Order_ID);//$Dateonly For showing document date only.

                                }
                            }
                        }
                    }
                    else if (ilstscaninindex[i].Source == "ORDER")
                    {
                        if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
                        {
                            string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
                            string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
                            string[] name = temp_file_name.Split('.');
                            ilstscaninindex[i].Document_Sub_Type = "Laboratory";
                            ilstscaninindex[i].Document_Type = "Results";
                            string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
                            if (ilstscaninindex[i].Order_ID == 0)
                                sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type);
                            else
                                sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Orders_Description);
                        }

                    }
                    else if (ilstscaninindex[i].Source == "EXAM")
                    {
                        if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
                        {
                            string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
                            string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
                            string[] name = temp_file_name.Split('.');
                            //ilstscaninindex[i].Document_Sub_Type = "Laboratory";
                            //   ilstscaninindex[i].Document_Type = "Results";
                            string file_name = ilstscaninindex[i].Document_Type + "_" + name[0];
                            if (ilstscaninindex[i].Order_ID == 0)
                                sDescList.Add("Exam" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type);
                            else
                                sDescList.Add("Exam" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Orders_Description);
                        }

                    }
                }
            }

            iMySessiondesc.Close();
            return sDescList;
        }

        public IList<string> GetEncounterListArrayPatientPortal(ulong ulHuman_ID)
        {
            ISession iMySessiondesc = NHibernateSessionManager.Instance.CreateISession();
            IQuery query = iMySessiondesc.GetNamedQuery("GetEncounterandAllDetails");
            query.SetString(0, ulHuman_ID.ToString());
            query.SetString(1, ulHuman_ID.ToString());
            ArrayList arrList = new ArrayList(query.List());
            IList<string> sDescList = new List<string>();
            foreach (object[] oj in arrList)
                sDescList.Add("Encounters" + "^" + oj[0].ToString() + "^" + oj[1].ToString() + ";" + oj[2].ToString());
            //IQuery queryarc = iMySessiondesc.GetNamedQuery("GetEncounterandAllDetailsFromArchive");
            //queryarc.SetString(0, ulHuman_ID.ToString());
            //ArrayList arrList_arc = new ArrayList(queryarc.List());
            //foreach (object[] oj in arrList_arc)
            //    sDescList.Add("Encounters" + "^" + oj[0].ToString() + "^" + oj[1].ToString() + ";" + oj[2].ToString());

            //IList<PatientNotes> lstpatientnotes = new List<PatientNotes>();
            //ISQLQuery sq = iMySessiondesc.CreateSQLQuery("SELECT p.*,w.* FROM patient_notes p left join wf_object w on (p.message_id=w.obj_system_id and w.obj_type='TASK') where w.current_process = 'CLOSE_TASK' and p.Human_Id='" + ulHuman_ID + "' and p.Is_PatientChart='Y' order by p.Modified_Date_And_Time Desc").AddEntity("p", typeof(PatientNotes)).AddEntity("w", typeof(WFObject));
            //int k = 0;
            //foreach (IList<object> l in sq.List())
            //{
            //    lstpatientnotes.Add((PatientNotes)l[0]);
            //    sDescList.Add("Patient Task" + "^" + lstpatientnotes[k].Modified_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt") + "^" + lstpatientnotes[k].Id);
            //    k++;
            //}
            //IList<TreatmentPlan> TreatLst = new List<TreatmentPlan>();
            //IList<Encounter> EncList = new List<Encounter>();
            //Encounter objEnc = null;
            //TreatmentPlan objPlan = null;
            //ISQLQuery sqlquery = iMySessiondesc.CreateSQLQuery("select e.*,p.* from encounter e left join treatment_plan p on (e.encounter_id=p.encounter_id) where e.human_id='" + ulHuman_ID + "' and e.Is_phone_Encounter='Y' order by e.Date_of_service Desc").AddEntity("e", typeof(Encounter)).AddEntity("p", typeof(TreatmentPlan));
            //foreach (IList<object> l in sqlquery.List())
            //{
            //    objEnc = (Encounter)l[0];
            //    EncList.Add(objEnc);
            //    objPlan = (TreatmentPlan)l[1];
            //    TreatLst.Add(objPlan);
            //}

            //if (EncList != null && EncList.Count > 0 && TreatLst != null && TreatLst.Count > 0)
            //{

            //    for (int n = 0; n < EncList.Count; n++)
            //    {
            //        TreatmentPlan objTrtPlan = new TreatmentPlan();
            //        if (TreatLst[0] != null)
            //        {
            //            objTrtPlan = (from p in TreatLst where p.Encounter_Id == EncList[n].Id select p).ToList<TreatmentPlan>()[0];
            //        }
            //        sDescList.Add("Phone Encounter" + "^" + objTrtPlan.Created_Date_And_Time.ToString() + "^" + objTrtPlan.Encounter_Id);
            //    }


            //}
            IQuery querySummary = iMySessiondesc.GetNamedQuery("GetSummaryofCareDetails");
            querySummary.SetString(0, ulHuman_ID.ToString());
            ArrayList arrListSummary = new ArrayList(querySummary.List());
            foreach (object[] oj in arrListSummary)
                sDescList.Add("Summary of Care" + "^" + oj[0].ToString() + "^" + oj[1].ToString() + ";" + oj[2].ToString());

            FillResultMasterAndEntry ResultEntryDTO = new FillResultMasterAndEntry();
            ResultMasterManager resultMngr = new ResultMasterManager();
            ResultEntryDTO = resultMngr.GetResultUsingHumanId(ulHuman_ID);
            FillResultMasterAndEntry objResultMasterAndEntry = ResultEntryDTO;
            IList<ResultMaster> ilstresultmaster = new List<ResultMaster>();
            IList<ResultMaster> result_master_list = new List<ResultMaster>();
            if (objResultMasterAndEntry.Result_Master_List.Count > 0)
                result_master_list = objResultMasterAndEntry.Result_Master_List;
            if (result_master_list.Count > 0)
            {
                ulong order_submit_id = 0;
                for (int i = 0; i < result_master_list.Count; i++)
                {
                    if (result_master_list[i].Order_ID != 0)
                    {
                        if (result_master_list[i].Order_ID != order_submit_id)
                        {
                            ilstresultmaster.Add(result_master_list[i]);
                            order_submit_id = result_master_list[i].Order_ID;
                        }
                    }
                    else
                    {
                        ilstresultmaster.Add(result_master_list[i]);
                        order_submit_id = result_master_list[i].Order_ID;
                    }
                }
            }

            if (ilstresultmaster.Count > 0)
                ilstresultmaster = ilstresultmaster.OrderByDescending(a => a.Created_Date_And_Time).ToList<ResultMaster>();

            for (int i = 0; i < ilstresultmaster.Count; i++)
            {
                if (ilstresultmaster[i].OBR_Specimen_Collected_Date_And_Time != string.Empty)//BugID:47602
                {
                    string formatString = "yyyyMMddHHmmss";
                    string sample = ilstresultmaster[i].OBR_Specimen_Collected_Date_And_Time;//BugID:47602
                    if (sample.Contains('-'))
                    {
                        sample = sample.Split('-')[0].ToString();
                    }
                    if (sample.Length == 14)
                        formatString = "yyyyMMddHHmmss";
                    else if (sample.Length == 12)
                        formatString = "yyyyMMddHHmm";

                    DateTime dt = DateTime.ParseExact(sample, formatString, null);
                    sDescList.Add("Others" + "^" + ilstresultmaster[i].Id + "^" + dt + "^Laboratory^" + ilstresultmaster[i].Orders_Description + "^Results^" + ilstresultmaster[i].Order_ID);
                }
            }



            FileManagementIndexManager scanMngr = new FileManagementIndexManager();

            IList<FileManagementIndex> filelist = new List<FileManagementIndex>();

            //if (sCategory == "Encounters")
            IList<FileManagementIndex> filelist_enc = new List<FileManagementIndex>();
            filelist_enc = scanMngr.GetEncountersFileList(ulHuman_ID);
            // else
            IList<FileManagementIndex> filelist_new = new List<FileManagementIndex>();
            filelist = scanMngr.GetFileList(ulHuman_ID);

            //filelist.Concat(filelist_enc);
            //filelist.Concat(filelist_new);

            IList<FileManagementIndex> ilstscaninindex = new List<FileManagementIndex>();

            int cntDR_EncounterList = 1;

            if (filelist.Count > 0)
            {
                ilstscaninindex = new List<FileManagementIndex>();
                for (int i = 0; i < filelist.Count; i++)
                {
                    if (filelist[i].Order_ID == 0 && filelist[i].Document_Sub_Type != "Lab Result Form")
                    {
                        ilstscaninindex.Add(filelist[i]);
                    }
                    else if (filelist[i].Order_ID != 0)
                    {
                        ilstscaninindex.Add(filelist[i]);
                    }
                }

                //Interpretation Notes
                //string[] sTempNew = sInterpretationSubDocType.Split('|').ToArray();
                //IList<FileManagementIndex> InterpretationNotes = (from intnotes in filelist where sTempNew.Contains(intnotes.Document_Sub_Type.ToUpper()) select intnotes).ToList<FileManagementIndex>();
                //string newList = string.Join(",", sInterpretationSubDocType.Split('|').Select(x => string.Format("'{0}'", x)).ToArray());

                //// string svalue = string.Join(",", EncLst.Cast<string>().ToArray());

                //if (InterpretationNotes.Count() > 0)//Check if filelist contains any one of the item in sInterpretationSubDocType
                //{

                //    // ISQLQuery sqlquery = iMySessiondesc.CreateSQLQuery("select * from file_management_index f, wf_object w where f.human_id='" + ulHuman_ID + "' and f.doc_sub_type in (" + newList + ") and f.order_id=w.obj_system_id and w.obj_type='DIAGNOSTIC ORDER' and (w.current_process='BILLING_WAIT' OR w.current_process='BILLING_COMPLETE') and f.result_master_id<>0").AddEntity("f", typeof(FileManagementIndex));

                //    ISQLQuery sqlquery = iMySessiondesc.CreateSQLQuery("select * from file_management_index f, wf_object w where f.human_id='" + ulHuman_ID + "' and f.doc_sub_type in (" + newList + ")  and f.order_id=w.obj_system_id and w.obj_type='DIAGNOSTIC ORDER' and (w.current_process='BILLING_WAIT' OR w.current_process='BILLING_COMPLETE') and f.result_master_id<>0 "
                //    + "union all  select * from file_management_index f, wf_object_arc wa where f.human_id='" + ulHuman_ID + "' and f.doc_sub_type in (" + newList + ")  and f.order_id=wa.obj_system_id and wa.obj_type='DIAGNOSTIC ORDER' and (wa.current_process='BILLING_WAIT' OR wa.current_process='BILLING_COMPLETE') and f.result_master_id<>0").AddEntity("f", typeof(FileManagementIndex));
                //    IList<FileManagementIndex> queryoutput = sqlquery.List<FileManagementIndex>();
                //    for (int i = 0; i < queryoutput.Count; i++)
                //    {
                //        //sDescList.Add("StressTestReport" + "^" + queryoutput[i].Id + "^" + queryoutput[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + "Stress Test Signed Report" + "^" + queryoutput[i].File_Path + "^" + queryoutput[i].Document_Type + "^" + queryoutput[i].Order_ID + "^" + queryoutput[i].Result_Master_ID);
                //        sDescList.Add("StressTestReport" + "^" + queryoutput[i].Id + "^" + queryoutput[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + queryoutput[i].Document_Sub_Type + " Signed Report" + "^" + queryoutput[i].File_Path + "^" + queryoutput[i].Document_Type + "^" + queryoutput[i].Order_ID + "^" + queryoutput[i].Result_Master_ID);

                //    }
                //}
            }

            if (ilstscaninindex.Count > 0)
                ilstscaninindex = ilstscaninindex.OrderByDescending(a => a.Order_ID).ToList<FileManagementIndex>();


            //ulong ord_sb_id = 0;
            for (int i = 0; i < ilstscaninindex.Count; i++)
            {
                if (ilstscaninindex[i].Order_ID != 0)
                {
                    ilstscaninindex[i].Created_Date_And_Time = ilstscaninindex[i].Document_Date;//Results uploaded from upload scan documents screen
                }
            }

            for (int i = 0; i < ilstscaninindex.Count; i++)
            {
                ilstscaninindex = ilstscaninindex.OrderBy(a => a.Appointment_Date).ToList<FileManagementIndex>();
                if (ilstscaninindex[i].Source.ToUpper() == "ENCOUNTER_XML_DR")
                {
                    if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
                    {
                        string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + (cntDR_EncounterList < 10 ? "0" + cntDR_EncounterList.ToString() : cntDR_EncounterList.ToString());
                        sDescList.Add("Others" + "^" + ilstscaninindex[i].Encounter_ID + "^" + ilstscaninindex[i].Appointment_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Result_Master_ID);
                        cntDR_EncounterList++;
                    }
                }
                else if (ilstscaninindex[i].Source == "SCAN" || ilstscaninindex[i].Source == "ELIGIBILITY VERIFICATION")//BugID:55156
                {
                    if (ilstscaninindex[i].Document_Type.ToUpper() == "ENCOUNTERS")
                    {
                        if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
                        {
                            string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
                            string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
                            string[] name = temp_file_name.Split('.');
                            string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
                            sDescList.Add("EncountersSub" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Result_Master_ID);
                        }
                    }
                    else
                    {
                        //if (ilstscaninindex[i].Document_Type.ToUpper() == "RESULTS" && sCategory == "Document")
                        //{
                        //    //Do Nothing
                        //}
                        //else
                        //{
                        if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
                        {
                            string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
                            string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
                            string[] name = temp_file_name.Split('.');
                            string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
                            if (ilstscaninindex[i].Order_ID == 0)
                                sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Result_Master_ID);
                            else //modified for integrum BugID:54261 & 55094
                                //sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Orders_Description + "$Dateonly" + "^" + ilstscaninindex[i].Result_Master_ID);//$Dateonly For showing document date only.
                                sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Result_Master_ID + "^" + ilstscaninindex[i].Orders_Description + "$Dateonly" + "^" + ilstscaninindex[i].Order_ID);//$Dateonly For showing document date only.

                        }
                        // }
                    }
                }
                else if (ilstscaninindex[i].Source == "ORDER")
                {
                    if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
                    {
                        string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
                        string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
                        string[] name = temp_file_name.Split('.');
                        ilstscaninindex[i].Document_Sub_Type = "Laboratory";
                        ilstscaninindex[i].Document_Type = "Results";
                        string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
                        if (ilstscaninindex[i].Order_ID == 0)
                            sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type);
                        else
                            sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Orders_Description);
                    }

                }
                else if (ilstscaninindex[i].Source == "EXAM")
                {
                    if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
                    {
                        string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
                        string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
                        string[] name = temp_file_name.Split('.');
                        //ilstscaninindex[i].Document_Sub_Type = "Laboratory";
                        //   ilstscaninindex[i].Document_Type = "Results";
                        string file_name = ilstscaninindex[i].Document_Type + "_" + name[0];
                        if (ilstscaninindex[i].Order_ID == 0)
                            sDescList.Add("Exam" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type);
                        else
                            sDescList.Add("Exam" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Orders_Description);
                    }

                }
            }


            /*
            FillResultMasterAndEntry ResultEntryDTO = new FillResultMasterAndEntry();
            ResultMasterManager resultMngr = new ResultMasterManager();
            ResultEntryDTO = resultMngr.GetResultUsingHumanId(ulHuman_ID);
            FileManagementIndexManager scanMngr = new FileManagementIndexManager();
            IList<FileManagementIndex> filelist = scanMngr.GetFileList(ulHuman_ID);
            IList<ResultMaster> result_master_list = new List<ResultMaster>();
            IList<FileManagementIndex> ilstscaninindex = new List<FileManagementIndex>();
            IList<ResultMaster> ilstresultmaster = new List<ResultMaster>();
            FillResultMasterAndEntry objResultMasterAndEntry = ResultEntryDTO;
            if (objResultMasterAndEntry.Result_Master_List.Count > 0)
                result_master_list = objResultMasterAndEntry.Result_Master_List;
            if (result_master_list.Count > 0)
            {
                ulong order_submit_id = 0;
                for (int i = 0; i < result_master_list.Count; i++)
                {
                    if (result_master_list[i].Order_ID != 0)
                    {
                        if (result_master_list[i].Order_ID != order_submit_id)
                        {
                            ilstresultmaster.Add(result_master_list[i]);
                            order_submit_id = result_master_list[i].Order_ID;
                        }
                    }
                    else
                    {
                        ilstresultmaster.Add(result_master_list[i]);
                        order_submit_id = result_master_list[i].Order_ID;
                    }
                }
            }
            if (ilstresultmaster.Count > 0 && filelist.Count > 0)
            {
                ilstscaninindex = new List<FileManagementIndex>();
                for (int i = 0; i < filelist.Count; i++)
                {
                    bool blScan = false;
                    for (int j = 0; j < ilstresultmaster.Count; j++)
                    {
                        if (ilstresultmaster[j].Order_ID != 0)
                        {
                            if (filelist[i].Order_ID == ilstresultmaster[j].Order_ID)
                            {
                                ilstresultmaster[j].Is_Scan_order = "Y";
                                blScan = true;
                                break;
                            }
                        }
                    }
                    if (blScan == false)
                    {
                        if (filelist[i].Order_ID == 0 && filelist[i].Document_Sub_Type != "Lab Result Form")
                        {
                            ilstscaninindex.Add(filelist[i]);
                        }
                        else if (filelist[i].Order_ID != 0)
                        {
                            ilstscaninindex.Add(filelist[i]);
                        }

                    }
                }
            }
            else if ((ilstresultmaster.Count == 0 && filelist.Count > 0))
            {
                for (int i = 0; i < filelist.Count; i++)
                {
                    ilstscaninindex.Add(filelist[i]);
                }
            }

            if (ilstresultmaster.Count > 0)
                ilstresultmaster = ilstresultmaster.OrderByDescending(a => a.Created_Date_And_Time).ToList<ResultMaster>();
            if (ilstscaninindex.Count > 0)
                ilstscaninindex = ilstscaninindex.OrderByDescending(a => a.Order_ID).ToList<FileManagementIndex>();

            for (int i = 0; i < ilstresultmaster.Count; i++)
            {
                if (ilstresultmaster[i].OBR_Specimen_Collected_Date_And_Time != string.Empty)//BugID:47602
                {
                    string formatString = "yyyyMMddHHmmss";
                    string sample = ilstresultmaster[i].OBR_Specimen_Collected_Date_And_Time;//BugID:47602
                    if (sample.Contains('-'))
                    {
                        sample = sample.Split('-')[0].ToString();
                    }
                    if (sample.Length == 14)
                        formatString = "yyyyMMddHHmmss";
                    else if (sample.Length == 12)
                        formatString = "yyyyMMddHHmm";

                    DateTime dt = DateTime.ParseExact(sample, formatString, null);
                    sDescList.Add("Others" + "^" + ilstresultmaster[i].Id + "^" + dt + "^Laboratory^" + ilstresultmaster[i].Orders_Description + "^Results^" + ilstresultmaster[i].Order_ID);
                }
            }
            //ulong ord_sb_id = 0;
            for (int i = 0; i < ilstscaninindex.Count; i++)
            {
                if (ilstscaninindex[i].Order_ID != 0)
                {  //BugID: 62511

                    //IList<OrdersSubmit> ordersList = new List<OrdersSubmit>();
                    //ICriteria crit = iMySessiondesc.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", ilstscaninindex[i].Order_ID));
                    //ordersList = crit.List<OrdersSubmit>();
                    //if (ordersList.Count > 0)
                    //{
                    //    if (ordersList[0].Bill_Type.Trim() != string.Empty) //BugID:42001 ,41884
                    //    {
                    //        ilstscaninindex[i].Created_Date_And_Time = ordersList[0].Created_Date_And_Time;//Results entered from diagnostic_order screen
                    //    }
                    //    else
                    //    {
                    ilstscaninindex[i].Created_Date_And_Time = ilstscaninindex[i].Document_Date;//Results uploaded from upload scan documents screen
                    //}
                    //if (ordersList[0].Specimen_Collection_Date_And_Time == DateTime.MinValue)
                    //{
                    //    ilstscaninindex[i].Created_Date_And_Time = ordersList[0].Created_Date_And_Time;
                    //}
                    //else
                    //{
                    //    ilstscaninindex[i].Created_Date_And_Time = ordersList[0].Specimen_Collection_Date_And_Time;
                    //}
                    //}
                    //  sDescList.Add("Results" + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Created_Date_And_Time + "|" + ilstscaninindex[i].Id + "|" + ilstscaninindex[i].Orders_Description); // ilstresultmaster[i].Created_Date_And_Time + "^" + ilstresultmaster[i].Orders_Description + "^" + ilstresultmaster[i].Order_ID);
                    //ord_sb_id = ilstscaninindex[i].Order_ID;
                }
                //if (ilstscaninindex[i].Order_ID != 0)
                //{
                //    //if (ord_sb_id != ilstscaninindex[i].Order_ID)
                //    //{
                //    sDescList.Add("Results" + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Document_Date + "|" + ilstscaninindex[i].Id); // ilstresultmaster[i].Created_Date_And_Time + "^" + ilstresultmaster[i].Orders_Description + "^" + ilstresultmaster[i].Order_ID);
                //    ord_sb_id = ilstscaninindex[i].Order_ID;
                //    //}
                //}

            }

            for (int i = 0; i < ilstscaninindex.Count; i++)
            {
                //if (ilstscaninindex[i].Order_ID == 0)
                //{
                if (ilstscaninindex[i].Source == "SCAN")
                {
                    if (ilstscaninindex[i].Document_Type.ToUpper() == "ENCOUNTERS")
                    {
                        if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
                        {
                            string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
                            string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
                            string[] name = temp_file_name.Split('.');
                            string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
                            sDescList.Add("EncountersSub" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path);
                        }
                    }
                    else
                    {
                        if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
                        {
                            string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
                            string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
                            string[] name = temp_file_name.Split('.');
                            string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
                            if (ilstscaninindex[i].Order_ID == 0)
                                sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type);
                            else
                                sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Orders_Description);
                        }

                    }
                }
                else if (ilstscaninindex[i].Source == "ORDER")
                {
                    if (ilstscaninindex[i].Document_Date.ToString() != string.Empty)
                    {
                        string Filename = ilstscaninindex[i].File_Path.Substring(ilstscaninindex[i].File_Path.LastIndexOf("/") + 1);
                        string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
                        string[] name = temp_file_name.Split('.');
                        ilstscaninindex[i].Document_Sub_Type = "Laboratory";
                        ilstscaninindex[i].Document_Type = "Results";
                        string file_name = ilstscaninindex[i].Document_Sub_Type + "_" + name[0];
                        if (ilstscaninindex[i].Order_ID == 0)
                            sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Document_Date.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type);
                        else
                            sDescList.Add("Others" + "^" + ilstscaninindex[i].Id + "^" + ilstscaninindex[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt") + "^" + file_name + "^" + ilstscaninindex[i].File_Path + "^" + ilstscaninindex[i].Document_Type + "^" + ilstscaninindex[i].Order_ID + "^" + ilstscaninindex[i].Orders_Description);
                    }

                }

                //}
            }
            */
            iMySessiondesc.Close();
            return sDescList;
        }
        public void UpdateEncounterforMyQueue(ulong ulEncID, string sMedAsstName, string sModifiedBy, DateTime dtModifiedDateandTime, DateTime dtDateOfService, string ObjType, string ExamRoom, string MACAddress, string sLocal_Time)
        {
            ulong ID = 0;
            IList<Encounter> updateEncounterList = GetEncounterByEncounterID(ulEncID);
            updateEncounterList[0].Encounter_ID = ulEncID;
            updateEncounterList[0].Modified_By = sModifiedBy;
            updateEncounterList[0].Modified_Date_and_Time = dtModifiedDateandTime;
            updateEncounterList[0].Date_of_Service = dtDateOfService;
            updateEncounterList[0].Local_Time = sLocal_Time;
            updateEncounterList[0].Exam_Room = ExamRoom;
            if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "TECHNICIAN")
                updateEncounterList[0].Technician_ID = ID;
            else if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "PHYSICIAN")
                updateEncounterList[0].Reading_Provider_ID = ID;
            //else if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "MEDICAL ASSISTANT")
            if (sMedAsstName != "")
                updateEncounterList[0].Assigned_Med_Asst_User_Name = sMedAsstName;
            else if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "SCRIBE")
                updateEncounterList[0].Assigned_Scribe_User_Name = sModifiedBy;
            IList<Encounter> addEncounterList = null;

            //string EncounterFileName = "Encounter" + "_" + ulEncID + ".xml";
            //string strXmlEncounterFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], EncounterFileName);
            //if (File.Exists(strXmlEncounterFilePath) == false || (File.Exists(strXmlEncounterFilePath)==true && File.ReadAllBytes(strXmlEncounterFilePath).Length ==0))
            //{

            //IList<object> ilstEncBlob = new List<object>();
            //IList<string> ilstEncounterTagList = new List<string>();
            //ilstEncounterTagList.Add("EncounterList");
            //ilstEncBlob = ReadBlob("Encounter", ulEncID, ilstEncounterTagList);

            //if (File.Exists(strXmlEncounterFilePath) == false)
            //{
            //itemDoc.Save(strXmlEncounterFilePath);

            //code modified by balaji.TJ 2023-01-31
            EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
            IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(ulEncID);
            if (ilstEncounterBlob.Count == 0)
            {

                string sDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("Template_XML");
                string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                XmlDocument itemDoc = new XmlDocument();
                XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                itemDoc.Load(XmlText);
                XmlText.Close();

                XmlNodeList xmlAgenode = itemDoc.GetElementsByTagName("Age");
                if (xmlAgenode != null && xmlAgenode.Count > 0)
                    xmlAgenode[0].ParentNode.RemoveChild(xmlAgenode[0]);

                // itemDoc.Save(strXmlEncounterFilePath);

                //int trycount = 0;
                //trytosaveagain:
                try
                {
                    // itemDoc.Save(strXmlEncounterFilePath);
                    ISession session = Session.GetISession();
                    try
                    {
                        using (ITransaction trans = session.BeginTransaction(IsolationLevel.ReadUncommitted))
                        {
                            WriteBlob(ulEncID, itemDoc, session, null, updateEncounterList, null, null, true);

                            trans.Commit();
                        }
                    }
                    catch (Exception ex1)
                    {

                        throw new Exception(ex1.Message);
                    }

                }
                catch (Exception xmlexcep)
                {
                    //trycount++;
                    //if (trycount <= 3)
                    //{
                    //    int TimeMilliseconds = 0;
                    //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                    //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                    //    Thread.Sleep(TimeMilliseconds);
                    //    string sMsg = string.Empty;
                    //    string sExStackTrace = string.Empty;

                    //    string version = "";
                    //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                    //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                    //    string[] server = version.Split('|');
                    //    string serverno = "";
                    //    if (server.Length > 1)
                    //        serverno = server[1].Trim();

                    //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                    //        sMsg = xmlexcep.InnerException.Message;
                    //    else
                    //        sMsg = xmlexcep.Message;

                    //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                    //        sExStackTrace = xmlexcep.StackTrace;

                    //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    //    string ConnectionData;
                    //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                    //    {
                    //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                    //        {
                    //            cmd.Connection = con;
                    //            try
                    //            {
                    //                con.Open();
                    //                cmd.ExecuteNonQuery();
                    //                con.Close();
                    //            }
                    //            catch
                    //            {
                    //            }
                    //        }
                    //    }
                    //    goto trytosaveagain;
                    //}
                }
                //}
            }
            //SaveUpdateDeleteWithTransaction(ref addEncounterList, updateEncounterList, null, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref addEncounterList, ref updateEncounterList, null, MACAddress, true, false, ulEncID, string.Empty);
            WFObjectManager objWFObjectManager = new WFObjectManager();
            objWFObjectManager.UpdateOwner(Convert.ToUInt64(ulEncID), ObjType, sModifiedBy, string.Empty);
        }

        // This method not in use
        //public WFObject UpdateEncounterforCheckin(ulong 
        //    ulEncID, string sMedAsstName, string sModifiedBy, DateTime dtModifiedDateandTime,
        //    DateTime dtDateOfService, string ObjType, string ExamRoom, string MACAddress, string sLocal_Time, ref ulong EncProviderID, ref string FacilityName)
        //{
        //    ulong ID = 0;
        //    IList<Encounter> updateEncounterList = GetEncounterByEncounterID(ulEncID);
        //    updateEncounterList[0].Encounter_ID = ulEncID;
        //    updateEncounterList[0].Modified_By = sModifiedBy;
        //    updateEncounterList[0].Modified_Date_and_Time = dtModifiedDateandTime;
        //    updateEncounterList[0].Date_of_Service = dtDateOfService;
        //    updateEncounterList[0].Local_Time = sLocal_Time;
        //    updateEncounterList[0].Exam_Room = ExamRoom;
        //    if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "TECHNICIAN")
        //        updateEncounterList[0].Technician_ID = ID;
        //    else if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "PHYSICIAN")
        //        updateEncounterList[0].Reading_Provider_ID = ID;
        //    //else if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "MEDICAL ASSISTANT")  //Commented for avoid Assigned_Med_Asst_User_Name in encounter while check in to provider by MA.
        //    //    updateEncounterList[0].Assigned_Med_Asst_User_Name = sMedAsstName;

        //    IList<Encounter> addEncounterList = null;
        //    EncProviderID = Convert.ToUInt64(updateEncounterList[0].Encounter_Provider_ID);
        //    FacilityName = updateEncounterList[0].Facility_Name;
        //    string EncounterFileName = "Encounter" + "_" + ulEncID + ".xml";
        //    string strXmlEncounterFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], EncounterFileName);
        //    if (File.Exists(strXmlEncounterFilePath) == false)
        //    {
        //        string sDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("Template_XML");
        //        string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
        //        XmlDocument itemDoc = new XmlDocument();
        //        XmlTextReader XmlText = new XmlTextReader(sXmlPath);
        //        itemDoc.Load(XmlText);
        //        XmlText.Close();

        //        XmlNodeList xmlAgenode = itemDoc.GetElementsByTagName("Age");
        //        if (xmlAgenode != null && xmlAgenode.Count > 0)
        //            xmlAgenode[0].ParentNode.RemoveChild(xmlAgenode[0]);
        //       // itemDoc.Save(strXmlEncounterFilePath);

        //        int trycount = 0;
        //    trytosaveagain:
        //        try
        //        {
        //            itemDoc.Save(strXmlEncounterFilePath);
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

        //    //SaveUpdateDeleteWithTransaction(ref addEncounterList, updateEncounterList, null, MACAddress);
        //    SaveUpdateDelete_DBAndXML_WithTransaction(ref addEncounterList, ref updateEncounterList, null, MACAddress, true, false, ulEncID, string.Empty);
        //    //WFObjectManager objWFObjectManager = new WFObjectManager();
        //    //objWFObjectManager.UpdateOwner(Convert.ToUInt64(ulEncID), ObjType, sModifiedBy, string.Empty);


        //    WFObject lstwfobject = new WFObject();
        //    WFObjectManager objwfobject = new WFObjectManager();
        //    lstwfobject = objwfobject.GetByObjectSystemId(Convert.ToUInt64(ulEncID), "ENCOUNTER");

        //    bool VerifyPFSH = false;
        //    string Source = string.Empty, If_Source_Of_Information_Others = string.Empty;



        //    MoveVerificationDTO objMoveVerifyDTO;
        //    string username = "";
        //    string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
        //    if (File.Exists(strXmlFilePath1) == true)
        //    {
        //        XmlDocument xmldocUser = new XmlDocument();
        //        xmldocUser.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "User" + ".xml");

        //        XmlNodeList xmlUserList = xmldocUser.GetElementsByTagName("User");


        //        if (xmlUserList.Count > 0)
        //        {
        //            foreach (XmlNode item in xmlUserList)
        //            {
        //                if (EncProviderID.ToString() == item.Attributes["Physician_Library_ID"].Value)
        //                {
        //                    username = item.Attributes["User_Name"].Value;

        //                    break;
        //                }
        //            }

        //        }
        //    }

        //    if (lstwfobject != null)
        //        objMoveVerifyDTO = PerformMovetoProvider(Convert.ToUInt64(ulEncID),
        //           EncProviderID,
        //       updateEncounterList[0].Human_ID, System.DateTime.Now, FacilityName, username, VerifyPFSH, Source, If_Source_Of_Information_Others,
        //       "", string.Empty, "", false, "", "btnMove", false, "",
        //      lstwfobject,
        //      updateEncounterList[0], "", 0, false, "", username, string.Empty);
        //    return lstwfobject;

        //}
        public IList<MyQ> UpdateEncounterMoveTo(ulong ulEncID, string sMedAsstName, string sModifiedBy, DateTime dtModifiedDateandTime, string CurrentObjType, string ExamRoom, string FacName, string objtype, string[] processtype, Boolean bShowall, int DefaultNoofDays, string MACAddress)
        {
            ulong ID = 0;
            IList<Encounter> updateEncounterList = GetEncounterByEncounterID(ulEncID);
            updateEncounterList[0].Encounter_ID = ulEncID;
            updateEncounterList[0].Modified_By = sModifiedBy;
            updateEncounterList[0].Modified_Date_and_Time = dtModifiedDateandTime;
            updateEncounterList[0].Exam_Room = ExamRoom;
            if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "TECHNICIAN")
                updateEncounterList[0].Technician_ID = ID;
            else if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "PHYSICIAN")
                updateEncounterList[0].Reading_Provider_ID = ID;
            else if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "MEDICAL ASSISTANT")
                updateEncounterList[0].Assigned_Med_Asst_User_Name = sMedAsstName;
            else if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "OFFICE MANAGER")
                updateEncounterList[0].Assigned_Med_Asst_User_Name = sMedAsstName;
            else if (GetTechnicianIDOrReadingProviderID(sModifiedBy, ref ID) == "SCRIBE")
                updateEncounterList[0].Assigned_Scribe_User_Name = sModifiedBy;

            IList<Encounter> addEncounterList = null;
            //SaveUpdateDeleteWithTransaction(ref addEncounterList, updateEncounterList, null, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref addEncounterList, ref updateEncounterList, null, MACAddress, false, false, ulEncID, string.Empty);
            WFObjectManager objWFObjectManager = new WFObjectManager();
            objWFObjectManager.UpdateOwner(Convert.ToUInt64(ulEncID), objtype, sModifiedBy, string.Empty);
            string[] ObjType = new string[4];
            ObjType[0] = "ENCOUNTER";
            ObjType[1] = "DOCUMENTATION";
            ObjType[2] = "PHONE ENCOUNTER";
            ObjType[3] = "DOCUMENT REVIEW";
            IList<MyQ> MyqList = objWFObjectManager.GetListObjects(FacName, ObjType, processtype, sMedAsstName, bShowall, DefaultNoofDays, string.Empty);
            return MyqList;
        }


        public MoveVerificationDTO PerformMovetoProvider(ulong ulMyEncounterID, ulong selectedPhysicianID, ulong ulMyHumanID, DateTime currentDate, string FacilityName, string UserName, bool VerifyPFSH, string Source, string SourceOtherInfo, string userCurrentProcess, string MACAddress, string btnPhyCorrectionText, bool bDuplicateCheck, string UserRole, string btnID, bool breview, string IsACOValid, WFObject objEncWfObj, Encounter EncRecord, string IsPatientDiscussed, ulong IsDiscussedBy, bool bAcoCheck, string sRoleName, string sUsername, string scribeprocess)
        {
            MoveVerificationDTO objMoveVerifyDTO = new MoveVerificationDTO();
            WFObjectManager objWfMngr = new WFObjectManager();
            IList<object> query1 = new List<object>();


            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                // objWfMngr.MoveToNextProcess(objEncWfObj.Obj_System_Id, objEncWfObj.Obj_Type, 4, "UNKNOWN", currentDate, MACAddress, null, null);bugid-41466

                if (IsPatientDiscussed != "" && IsDiscussedBy != 0 && bAcoCheck)
                {
                    query1 = iMySession.CreateSQLQuery("Update human set ACO_Is_Eligible_Patient='" + IsPatientDiscussed + "',ACO_Patient_Discussed_By='" + IsDiscussedBy + "' where Human_ID='" + ulMyHumanID + "'").List<object>();
                }
                EncRecord.Encounter_Provider_ID = Convert.ToInt32(selectedPhysicianID);
                EncRecord.Modified_By = UserName;
                EncRecord.Modified_Date_and_Time = currentDate;
                if (sRoleName.Trim().ToUpper() == "PHYSICIAN")
                {
                    EncRecord.Is_Physician_Asst_Process = "N";
                }
                else if (sRoleName == "Physician Assistant")
                {
                    EncRecord.Is_Physician_Asst_Process = "Y";
                }
                if (UserRole.Trim().ToUpper() == "TECHNICIAN" && userCurrentProcess.ToUpper() == "TECHNICIAN_PROCESS")//BugID:53885 -- Added for CMG LAB AND ANCILLARY-- When moved to Coder EandMSubmitted is set to 'Y'
                {
                    EncRecord.Is_EandM_Submitted = "Y";
                    EncRecord.E_M_Submitted_Date_And_Time = DateTime.Now;
                }
                EncRecord.Is_PFSH_Verified = "Y";
                EncRecord.Source_Of_Information = "Self";

                if (UserRole.Trim().ToUpper() == "MEDICAL ASSISTANT" && userCurrentProcess.ToUpper().Trim() == "MA_PROCESS" && btnPhyCorrectionText.ToUpper().Trim() == "MOVE TO PROVIDER" && scribeprocess == string.Empty)
                {
                    EncRecord.Assigned_Scribe_User_Name = string.Empty;
                }
                EncounterManager EncMngr = new EncounterManager();

                IList<WFObject> iWfObject = new List<WFObject>();
                iWfObject = objWfMngr.GetByDocumentObjectSystemId(ulMyEncounterID, "DOCUMENTATION");
                //if (iWfObject != null && iWfObject.Count == 0)
                //{
                //    WFObject WFObj = new WFObject();

                //    WFObj.Fac_Name = FacilityName;
                //    WFObj.Obj_Type = "DOCUMENTATION";
                //    WFObj.Parent_Obj_Type = "ENCOUNTER";
                //    WFObj.Parent_Obj_System_Id = ulMyEncounterID;
                //    WFObj.Obj_System_Id = ulMyEncounterID;
                //    WFObj.Current_Process = "START";
                //    WFObj.Current_Arrival_Time = currentDate;
                //    string sOwner = string.Empty, sRole = string.Empty;

                //    WFObj.Current_Owner = sUsername;
                //    if (UserRole.ToUpper().Trim() == "TECHNICIAN")//CMG Ancilliary
                //        WFObj.Current_Owner = "UNKNOWN";
                //    if (scribeprocess.ToUpper() == "SCRIBE")
                //    {
                //        WFObj.Current_Owner = "UNKNOWN";
                //        EncMngr.UpdateEncounterWithEncounterChild(EncRecord, WFObj, objEncWfObj, MACAddress, 6);

                //    }
                //    else
                //        EncMngr.UpdateEncounterWithEncounterChild(EncRecord, WFObj, objEncWfObj, MACAddress, 1);
                //}
                //check for current_process to prevent DOCUMENTATION Object from Moving to REVIEW_CODING from PROVIDER_PROCESS on click of Move to Provider from MA
                if (iWfObject != null && iWfObject.Count > 0)
                {
                    if (iWfObject != null && iWfObject.Count > 0 && (iWfObject[0].Current_Process.ToUpper() == "PROVIDER_PROCESS_WAIT" || iWfObject[0].Current_Process.ToUpper() == "SCRIBE_PROCESS_WAIT" || iWfObject[0].Current_Process.ToUpper() == "MA_PROCESS"))
                    {




                        if (scribeprocess.ToUpper() == "SCRIBE" && (iWfObject[0].Current_Process.ToUpper() == "SCRIBE_PROCESS_WAIT" || iWfObject[0].Current_Process.ToUpper() == "PROVIDER_PROCESS_WAIT"))
                        {
                            if (EncRecord.Assigned_Scribe_User_Name != string.Empty)
                            {
                                sUsername = EncRecord.Assigned_Scribe_User_Name;
                                objWfMngr.MoveToNextProcess(iWfObject[0].Obj_System_Id, iWfObject[0].Obj_Type, 6, sUsername, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), iWfObject[0].Current_Process, null, null);
                                EncMngr.UpdateDocumentationEncounterWithEncounterChild(EncRecord, objEncWfObj, MACAddress, 1);
                            }


                            else
                            {
                                sUsername = "UNKNOWN";
                                objWfMngr.MoveToNextProcessscribe(iWfObject[0].Obj_System_Id, iWfObject[0].Obj_Type, 6, sUsername, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), iWfObject[0].Current_Process, null, null);
                                EncMngr.UpdateDocumentationEncounterWithEncounterChild(EncRecord, objEncWfObj, MACAddress, 1);
                            }
                        }
                        else if (iWfObject[0].Current_Process.ToUpper() == "SCRIBE_PROCESS_WAIT")
                        {
                            //sUsername = EncRecord.Assigned_Scribe_User_Name;

                            objWfMngr.MoveToNextProcess(iWfObject[0].Obj_System_Id, iWfObject[0].Obj_Type, 1, sUsername, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), iWfObject[0].Current_Process, null, null);
                            EncMngr.UpdateDocumentationEncounterWithEncounterChild(EncRecord, objEncWfObj, MACAddress, 1);
                        }
                        else if (iWfObject[0].Current_Process.ToUpper() == "MA_PROCESS")
                        {
                            //sUsername = EncRecord.Assigned_Scribe_User_Name;
                            if (scribeprocess.ToUpper() == "SCRIBE")
                            {
                                sUsername = "UNKNOWN";
                                objWfMngr.MoveToNextProcess(iWfObject[0].Obj_System_Id, iWfObject[0].Obj_Type, 6, sUsername, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), iWfObject[0].Current_Process, null, null);
                            }
                            else
                            {
                                objWfMngr.MoveToNextProcess(iWfObject[0].Obj_System_Id, iWfObject[0].Obj_Type, 1, sUsername, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), iWfObject[0].Current_Process, null, null);

                            }
                            IList<Encounter> lstAddEncounter = null;
                            IList<Encounter> lstUpdateEncounter = new List<Encounter>();
                            lstUpdateEncounter.Add(EncRecord);
                            EncMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref lstAddEncounter, ref lstUpdateEncounter, null, MACAddress, true, true, EncRecord.Id, string.Empty);
                        }
                        else
                        {
                            objWfMngr.MoveToNextProcess(iWfObject[0].Obj_System_Id, iWfObject[0].Obj_Type, 1, sUsername, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), iWfObject[0].Current_Process, null, null);
                            EncMngr.UpdateDocumentationEncounterWithEncounterChild(EncRecord, objEncWfObj, MACAddress, 1);
                        }
                    }
                    if (iWfObject != null && iWfObject.Count > 0 && (iWfObject[0].Current_Process.ToUpper() == "SCRIBE_PROCESS" || iWfObject[0].Current_Process.ToUpper() == "SCRIBE_CORRECTION" || iWfObject[0].Current_Process.ToUpper() == "SCRIBE_REVIEW_CORRECTION"))
                    {
                        objWfMngr.MoveToNextProcess(iWfObject[0].Obj_System_Id, iWfObject[0].Obj_Type, 1, sUsername, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), iWfObject[0].Current_Process, null, null);

                    }
                    if (iWfObject != null && iWfObject.Count > 0 && iWfObject[0].Current_Process.ToUpper() == "TECHNICIAN_PROCESS")
                    {
                        objWfMngr.MoveToNextProcess(iWfObject[0].Obj_System_Id, iWfObject[0].Obj_Type, 1, "UNKNOWN", System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), iWfObject[0].Current_Process, null, null);

                        objWfMngr.MoveToNextProcess(objEncWfObj.Obj_System_Id, objEncWfObj.Obj_Type, 1, "UNKNOWN", currentDate, MACAddress, null, null);

                        WFObject objBillingWfObj = objWfMngr.GetByObjectSystemId(iWfObject[0].Obj_System_Id, "BILLING");
                        if (objBillingWfObj.Current_Process == "BATCHING_WAIT")
                            objWfMngr.MoveToNextProcess(objBillingWfObj.Obj_System_Id, objBillingWfObj.Obj_Type, 1, "UNKNOWN", currentDate, MACAddress, null, null);

                        EncMngr.UpdateEncounterList(EncRecord, string.Empty);

                        WFObject objDocWfObject = objWfMngr.GetByObjectSystemId(iWfObject[0].Obj_System_Id, "DOCUMENTATION");
                        if (objDocWfObject.Current_Process == "DOCUMENT_COMPLETE")
                        {
                            //Jira CAP-340
                            EncounterBlobManager objEncblobmngr = new EncounterBlobManager();
                            objEncblobmngr.LockEncounter(ulMyEncounterID, ulMyHumanID, UserName, currentDate);
                        }
                    }
                }

                objMoveVerifyDTO.IsPFSHVerified = EncRecord.Is_PFSH_Verified;
                objMoveVerifyDTO.IsWorkflowPushed = true;

                iMySession.Close();
            }

            return objMoveVerifyDTO;
        }

        public MoveVerificationDTO PerformMovetoNextProcess(ulong ulMyEncounterID, ulong selectedPhysicianID, ulong ulMyHumanID, DateTime currentDate, string FacilityName, string UserName, bool VerifyPFSH, string Source, string SourceOtherInfo, string userCurrentProcess, string MACAddress, string btnPhyCorrectionText, bool bDuplicateCheck, string UserRole, string btnID, bool breview, string IsACOValid, WFObject objEncWfObj, Encounter EncRecord, WFObject objDocWfobj, WFObject objDocReviewWfobj, string IsPatientDiscussed, ulong IsDiscussedBy, out string sAlert)
        {
            sAlert = string.Empty;
            MoveVerificationDTO objMoveVerifyDTO = new MoveVerificationDTO();
            int EMICDCount = 0;

            EAndMCodingManager emMnger = new EAndMCodingManager();
            EandMCodingICDManager emICDMngr = new EandMCodingICDManager();
            IList<EandMCodingICD> eandmICDList = new List<EandMCodingICD>();
            IList<string> GcodeCheckList = new List<string>();
            bool bGCodeCheck = false;

            objMoveVerifyDTO.EandMCodingList = emMnger.GetEMCodeList(ulMyEncounterID);
            EMICDCount = emICDMngr.GetEMICDCount(ulMyEncounterID);


            //  var GcodeList = from g in objMoveVerifyDTO.EandMCodingList where (g.Procedure_Code == "G0438" || g.Procedure_Code == "G0439") select g.Id;
            //CAP-975
            //var GcodeList = from g in objMoveVerifyDTO.EandMCodingList where (g.Procedure_Code == "G0438" || g.Procedure_Code == "G0439") select g.Encounter_ID;

            //if (GcodeList != null && GcodeList.Count() > 0)
            //{
            //    objMoveVerifyDTO.IsGcodePresent = true;
            //    for (int i = 0; i < GcodeList.Count(); i++)
            //    {
            //        eandmICDList = emICDMngr.EandMcodingList(Convert.ToUInt32(GcodeList.ElementAt(i)));
            //        if (eandmICDList.Count > 0)
            //        {
            //            GcodeCheckList.Add(eandmICDList.Any(a => a.ICD == "V70.0").ToString());


            //        }

            //    }
            //    objMoveVerifyDTO.IsICDPresent = GcodeCheckList.Contains("False") == true ? false : true;
            //    if (Convert.ToBoolean(objMoveVerifyDTO.IsGcodePresent) == true && Convert.ToBoolean(objMoveVerifyDTO.IsICDPresent) == false)
            //    {
            //        bGCodeCheck = true;
            //        return objMoveVerifyDTO;
            //    }
            //    else
            //    {
            //        bGCodeCheck = false;
            //    }
            //}

            IDictionary<string, IList<string>> CPTICD = new Dictionary<string, IList<string>>();
            //Jira CAP-998
            //string sE_And_M_CPT_And_ICD = System.Configuration.ConfigurationSettings.AppSettings["E_And_M_CPT_And_ICD"].ToString();
            string sE_And_M_CPT_And_ICD_Validation = string.Empty;
            HumanManager HumanMngr = new HumanManager();
            Human objHuman = new Human();
            objHuman = HumanMngr.GetHumanFromHumanID(EncRecord.Human_ID);
            int iAge = CalculateAgeByDOS(objHuman.Birth_Date, EncRecord.Date_of_Service);
            if (iAge > 18)
            {
                sAlert = "180045";
                sE_And_M_CPT_And_ICD_Validation = "E_And_M_CPT_And_ICD";
            }
            else
            {
                sAlert = "180057";
                sE_And_M_CPT_And_ICD_Validation = "E_And_M_CPT_And_ICD_LessThenOrEqual18Age";
            }
            string sE_And_M_CPT_And_ICD = System.Configuration.ConfigurationSettings.AppSettings[sE_And_M_CPT_And_ICD_Validation].ToString();
            string sPlan = "";// System.Configuration.ConfigurationSettings.AppSettings["Primary_Plan"].ToString();
            if (sE_And_M_CPT_And_ICD != "")
            {
                string[] sVal = sE_And_M_CPT_And_ICD.Split('$');
                if (sVal.Count() > 0)
                {
                    for (int i = 0; i < sVal.Count(); i++)
                    {
                        CPTICD.Add(sVal[i].Split('|')[0].ToString(), sVal[i].Split('|').Skip(1).ToList());
                    }
                }
            }
            var GcodeList = from g in objMoveVerifyDTO.EandMCodingList where (CPTICD.Any(c => g.Procedure_Code.Contains(c.Key))) select new { ID = g.Id, Procedure_Code = g.Procedure_Code };

            var GcodeList_New = from g in objMoveVerifyDTO.EandMCodingList where (CPTICD.Any(c => g.Procedure_Code.Contains(c.Key))) select g.Procedure_Code;

            var Procedure_code = CPTICD.Where(a => GcodeList_New.Any(b => b.ToString() == a.Key)).Select(r => r);

            IList<string> ilstICD = new List<string>();
            if (GcodeList.Count() > 0)
            {
                objMoveVerifyDTO.IsGcodePresent = true;
                for (int i = 0; i < GcodeList.Count(); i++)
                {
                    //eandmICDList = emICDMngr.EandMcodingList(Convert.ToUInt32(GcodeList.ElementAt(i)));
                    IList<EandMCodingICD> eandmICDList1 = new List<EandMCodingICD>();
                    eandmICDList1 = emICDMngr.EandMcodingList(Convert.ToUInt32(ulMyEncounterID));
                    eandmICDList = (from p in eandmICDList1 where p.Is_Delete == "N" select p).ToList<EandMCodingICD>();
                    if (eandmICDList.Count > 0)
                    {
                        // GcodeCheckList.Add(eandmICDList.Any(a => a.ICD == "V70.0").ToString());
                        //GcodeCheckList.Add(eandmICDList.Any(a => a.ICD == "Z00.00").ToString());
                        //GcodeCheckList.Add(eandmICDList.Any(a => CPTICD.Any(z => z.Value.Contains(a.ICD))).ToString());
                        GcodeCheckList.Add(eandmICDList.Any(a => Procedure_code.Any(z => z.Key == (GcodeList.ElementAt(i).Procedure_Code) && z.Value.Contains(a.ICD))).ToString());
                        if (GcodeCheckList[i].ToString().ToUpper() == "FALSE")
                        {
                            //var icd = string.Join(" or ", (Procedure_code.ElementAt(i).Value).Select(g => g.ToString()).ToArray());
                            var test = (from r in Procedure_code
                                        where r.Key == GcodeList.ElementAt(i).Procedure_Code

                                        select r.Value).ToArray();


                            if (test.Count() > 0)
                            {
                                var icd = string.Join(" or ", test[0].Select(g => g.ToString()).ToArray());
                                ilstICD.Add(icd.ToString() + '$' + GcodeList.ElementAt(i).Procedure_Code);
                            }
                        }
                    }

                }

                objMoveVerifyDTO.IsICDPresent = GcodeCheckList.Contains("False") == true ? false : true;

                if (Convert.ToBoolean(objMoveVerifyDTO.IsGcodePresent) == true && Convert.ToBoolean(objMoveVerifyDTO.IsICDPresent) == false)
                {
                    bGCodeCheck = true;
                    return objMoveVerifyDTO;
                }
                else
                {
                    bGCodeCheck = false;
                }
            }


            if (Convert.ToBoolean(bDuplicateCheck) == false)
            {
                objMoveVerifyDTO.IsDuplicatePresent = false;
                bGCodeCheck = false;
            }
            else
            {
                int EandMListCount = objMoveVerifyDTO.EandMCodingList.Count;
                var EandMDistinctListCount = (from d in objMoveVerifyDTO.EandMCodingList select d.Procedure_Code).Distinct();
                if (EandMListCount != EandMDistinctListCount.Count())
                {
                    objMoveVerifyDTO.IsDuplicatePresent = true;
                }
            }


            if (objDocWfobj.Current_Process == "PROVIDER_PROCESS")
            {
                if (UserRole == "Physician")
                {

                    //HumanManager objHumanManger = new HumanManager();
                    //objMoveVerifyDTO.IsACOValid = objHumanManger.IsACOValid(ulMyHumanID);

                    if (objMoveVerifyDTO.EandMCodingList.Count == 0)
                    {
                        objMoveVerifyDTO.IsWorkflowPushed = false;
                    }
                    else if (EMICDCount == 0)
                    {
                        objMoveVerifyDTO.IsICDFilled = false;
                        objMoveVerifyDTO.IsWorkflowPushed = false;
                    }
                    else
                    {

                        MoveToNextProcessFromAfterPlan(EncRecord, ulMyEncounterID, ulMyHumanID, selectedPhysicianID, UserName, btnPhyCorrectionText, FacilityName, string.Empty, Convert.ToDateTime(currentDate), breview, objEncWfObj, objDocWfobj, objDocReviewWfobj, UserRole);
                        objMoveVerifyDTO.IsWorkflowPushed = true;
                    }
                }
                else
                {
                    if (objMoveVerifyDTO.EandMCodingList.Count == 0)
                    {
                        objMoveVerifyDTO.IsWorkflowPushed = false;
                        objMoveVerifyDTO.IsPFSHVerified = EncRecord.Is_PFSH_Verified;
                    }
                    else
                    {
                        MoveToNextProcessFromAfterPlan(EncRecord, ulMyEncounterID, ulMyHumanID, selectedPhysicianID, UserName, btnPhyCorrectionText, FacilityName, string.Empty, Convert.ToDateTime(currentDate), breview, objEncWfObj, objDocWfobj, objDocReviewWfobj, UserRole);
                        objMoveVerifyDTO.IsWorkflowPushed = true;
                    }
                }
            }

            //Jira #CAP-707
            if (objDocWfobj.Current_Process == "AKIDO_SCRIBE_PROCESS")
            {
                WFObjectManager objenco = new WFObjectManager();
                objenco.MoveToNextProcess(ulMyEncounterID, "DOCUMENTATION", 1, "UNKNOWN", Convert.ToDateTime(currentDate), string.Empty, null, null);
            }

            return objMoveVerifyDTO;

        }
        public Boolean CheckBillableCodes(ulong ulEncID)
        {
            Boolean bCheck = false;

            ISQLQuery sql = session.GetISession().CreateSQLQuery("select count(*) from procedure_code_library where procedure_code in (select procedure_code from e_m_coding where encounter_id='" + ulEncID + "' and is_delete!='Y' and (procedure_code in ('99080','99024','MAVST','NCVST','NO CHARGE','NOBILL','NURSE','TB','WT','BP','A9270','99421','99422','99423') or procedure_charge >=1.00));");

            ArrayList ilistObj = new ArrayList(sql.List());
            if (ilistObj != null && ilistObj.Count > 0)
            {
                if (Convert.ToUInt64(ilistObj[0]) >= 1)
                    bCheck = true;
                else
                    bCheck = false;
            }

            return bCheck;
        }

        public Encounter UpdateE_SuperBill(IList<Encounter> EncListupdate, string MACAddress)
        {
            IList<Encounter> EncSaveList = null;
            //SaveUpdateDeleteWithTransaction(ref EncSaveList, EncListupdate, null, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref EncSaveList, ref EncListupdate, null, MACAddress, true, false, EncListupdate[0].Id, string.Empty);

            WFObjectManager objWfMngr = new WFObjectManager();
            IList<WFObject> iWfObject = new List<WFObject>();
            iWfObject = objWfMngr.GetByDocumentObjectSystemId(EncListupdate[0].Id, "BILLING");

            if (iWfObject[0].Current_Process == "BATCHING_WAIT")
                objWfMngr.MoveToNextProcess(iWfObject[0].Obj_System_Id, iWfObject[0].Obj_Type, 1, "UNKNOWN", System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), iWfObject[0].Current_Process, null, null);

            //if (EncListupdate != null)
            //{
            //    if (EncListupdate.Count > 0)
            //    {
            //        for (int i = 0; i < EncListupdate.Count; i++)
            //        {
            //            EncListupdate[i].Version = EncListupdate[i].Version + 1;
            //        }
            //        GenerateXml XMLObj = new GenerateXml();
            //        ulong encounterid = EncListupdate[0].Id;
            //        List<object> lstObj = EncListupdate.Cast<object>().ToList();
            //        XMLObj.GenerateXmlUpdate(lstObj, encounterid, string.Empty);
            //    }
            //}
            return GetById(EncListupdate[0].Id);
        }

        public string GetPreviousEncounterID(ulong HumanID, DateTime DateofService)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            ISQLQuery sqlquery = iMySession.CreateSQLQuery("select e.* from encounter e where e.human_id='" + HumanID + "' and e.date_of_service<'" + DateofService.ToString("yyyy-MM-dd HH:mm:ss") + "' order by e.date_of_service desc limit 1").AddEntity("e", typeof(Encounter));
            ilstEncounter = sqlquery.List<Encounter>();
            string PrevEncouterID = string.Empty;
            if (ilstEncounter.Count > 0 && ilstEncounter[0].Id != 0)
            {
                PrevEncouterID = ilstEncounter[0].Id.ToString();
            }
            iMySession.Close();
            return PrevEncouterID;
        }

        public IList<Encounter> GetBillingInstructions(ulong EncounterID, string Keyword)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            ISQLQuery sqlquery = iMySession.CreateSQLQuery("select e.* from encounter e where e.Billing_Instruction like '%" + Keyword + "%' order by e.Billing_Instruction").AddEntity("e", typeof(Encounter));
            ilstEncounter = sqlquery.List<Encounter>();
            return ilstEncounter;
        }

        public IList<Encounter> GetPreviousEncounter(ulong EncounterId, ulong humanId, ulong physicianId)
        {
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            var query = @"SELECT E.* 
                          FROM   ENCOUNTER E 
                          WHERE  E.HUMAN_ID = :HUMANID 
                                 AND E.ENCOUNTER_PROVIDER_ID = :PHYSICIANID 
                                 AND E.DATE_OF_SERVICE <> '0001-01-01 00:00:00' 
                                 AND E.DATE_OF_SERVICE < (SELECT E.DATE_OF_SERVICE 
                                                          FROM   ENCOUNTER E 
                                                          WHERE  E.ENCOUNTER_ID = :ENCOUNTERID) 
                                 AND E.ENCOUNTER_ID <> :ENCOUNTERID 
                                 AND E.IS_PHONE_ENCOUNTER <> 'Y' 
                          ORDER  BY E.DATE_OF_SERVICE DESC ";

            ISQLQuery ResultList = iMySession.CreateSQLQuery(query).AddEntity("e", typeof(Encounter));

            ResultList.SetParameter("HUMANID", humanId);
            ResultList.SetParameter("PHYSICIANID", Convert.ToInt32(physicianId));
            ResultList.SetParameter("ENCOUNTERID", EncounterId);

            var ilstEncounter = ResultList.List<Encounter>();

            iMySession.Close();

            return ilstEncounter;
        }

        public IList<Encounter> GetEncounterUsingHumanIDOrderByEncID(ulong ulHumanID)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            ISQLQuery sqlquery = iMySession.CreateSQLQuery("select e.* from encounter e where e.human_id='" + ulHumanID + "'  and e.date_of_service='0001-01-01 00:00:00' and date(e.Appointment_Date)>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' order by e.Appointment_Date asc ").AddEntity("e", typeof(Encounter));
            ilstEncounter = sqlquery.List<Encounter>();

            iMySession.Close();
            return ilstEncounter;


        }

        // Copy Previous Encounter

        public string CopyPreviousEncounter(ulong encounterId, ulong humanId, ulong physicianId,
                                           string currentUserName, ulong previousEncounterId, string userRole, DateTime dtCurrentDOS)
        {
            try
            {
                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                {
                    IQuery iQuery = iMySession.GetNamedQuery("SP.CopyPrevious.Encounter");

                    iQuery.SetParameter("HUMANID", humanId);
                    iQuery.SetParameter("PREVIOUSENCOUNTER", previousEncounterId);
                    iQuery.SetParameter("CURRENTENCOUNTER", encounterId);
                    iQuery.SetParameter("CURRENTPHYSICIAN", physicianId);
                    iQuery.SetParameter("ROLE", userRole);
                    iQuery.SetParameter("CURRENTUSER", currentUserName);
                    iQuery.SetParameter("CURRENTDOS", dtCurrentDOS.ToString("yyyy-MM-dd"));

                    ArrayList arlstReturn = new ArrayList(iQuery.List());

                    iMySession.Close();

                    if (arlstReturn.Count > 0)
                    {
                        var returnValue = Convert.ToString(arlstReturn[0]);
                        return returnValue;
                    }

                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return "ERROR~" + Convert.ToString(ex.InnerException.Message).Replace(Environment.NewLine, string.Empty);
                }

                return "ERROR~" + Convert.ToString(ex.Message).Replace(Environment.NewLine, string.Empty);
            }
        }

        //        public IList<Encounter> GetPreviousEncounterDetails(ulong ulEncounterID, ulong uHumanID,
        //                                                       ulong uPhysicianID, out bool isPhysicianProcess, out bool isFromArchive)
        //        {
        //            IList<Encounter> lstPreviousEncounter = new List<Encounter>();
        //            WFObjectManager objWFObjectManager = new WFObjectManager();

        //            isPhysicianProcess = false;
        //            isFromArchive = false;

        //            if (uHumanID == 0)
        //            {
        //                return lstPreviousEncounter;
        //            }

        //            ulong previousEncounterId = 0;

        //            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //            {
        //                try
        //                {
        //                    var querySQL = @"SELECT E.* 
        //                                     FROM   ENCOUNTER E 
        //                                     WHERE  E.HUMAN_ID=:HUMAN_ID 
        //                                     AND    E.ENCOUNTER_PROVIDER_ID=:PHYSICIAN_ID 
        //                                     AND    E.DATE_OF_SERVICE <>'0001-01-01 00:00:00'
        //                                     AND    E.ENCOUNTER_ID <> :ENCOUNTER_ID 
        //                                     AND    E.IS_PHONE_ENCOUNTER <> 'Y' 
        //                                     AND    E.DATE_OF_SERVICE < 
        //                                            ( SELECT   E.DATE_OF_SERVICE 
        //                                              FROM     ENCOUNTER E 
        //                                              WHERE    E.ENCOUNTER_ID=:ENCOUNTER_ID ) 
        //                                     ORDER BY E.DATE_OF_SERVICE DESC";


        //                    var SQLQuery = iMySession.CreateSQLQuery(querySQL)
        //                           .AddEntity("E", typeof(Encounter));

        //                    SQLQuery.SetParameter("ENCOUNTER_ID", ulEncounterID);
        //                    SQLQuery.SetParameter("HUMAN_ID", uHumanID);
        //                    SQLQuery.SetParameter("PHYSICIAN_ID", uPhysicianID);

        //                    var lstEncounter = SQLQuery.List<Encounter>();

        //                    if (lstEncounter.Count == 0)
        //                    {
        //                        querySQL = @"SELECT E.* 
        //                                     FROM   ENCOUNTER_ARC E 
        //                                     WHERE  E.HUMAN_ID=:HUMAN_ID 
        //                                     AND    E.ENCOUNTER_PROVIDER_ID=:PHYSICIAN_ID 
        //                                     AND    E.DATE_OF_SERVICE <>'0001-01-01 00:00:00'
        //                                     AND    E.ENCOUNTER_ID <> :ENCOUNTER_ID 
        //                                     AND    E.IS_PHONE_ENCOUNTER <> 'Y'                                      
        //                                     ORDER BY E.DATE_OF_SERVICE DESC";

        //                        SQLQuery = iMySession.CreateSQLQuery(querySQL)
        //                             .AddEntity("E", typeof(Encounter));

        //                        SQLQuery.SetParameter("ENCOUNTER_ID", ulEncounterID);
        //                        SQLQuery.SetParameter("HUMAN_ID", uHumanID);
        //                        SQLQuery.SetParameter("PHYSICIAN_ID", uPhysicianID);

        //                        lstEncounter = SQLQuery.List<Encounter>();
        //                        isFromArchive = !(lstEncounter.Count == 0);
        //                    }

        //                    if (lstEncounter.Count == 0)
        //                    {
        //                        previousEncounterId = 0;
        //                    }
        //                    //else  // added to avoid the check for the current_process="Provider Process" for the previous encounter BugID:43191,41676 
        //                    //{
        //                    //    previousEncounterId = lstEncounter[0].Id;
        //                    //    isPhysicianProcess = true;

        //                    //    lstPreviousEncounter = lstEncounter.Where(a => a.Id == previousEncounterId)
        //                    //                                       .ToList<Encounter>();
        //                    //}
        //                    // commented to avoid the check for the current_process="Provider Process" for the previous encounter BugID:43191,41676 
        //                    else if (lstEncounter.Count == 1)
        //                    {
        //                        previousEncounterId = lstEncounter[0].Id;
        //                        isPhysicianProcess = objWFObjectManager.IsPreviousEncounterPhysicianProcess(previousEncounterId, isFromArchive);

        //                        lstPreviousEncounter = lstEncounter.Where(a => a.Id == previousEncounterId)
        //                                                           .ToList<Encounter>();
        //                        if (!isPhysicianProcess)
        //                        {
        //                            return lstPreviousEncounter;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        for (int i = 0; i < lstEncounter.Count; i++)
        //                        {
        //                            isPhysicianProcess = objWFObjectManager.IsPreviousEncounterPhysicianProcess(lstEncounter[i].Id, isFromArchive);

        //                            if (isPhysicianProcess)
        //                            {
        //                                previousEncounterId = lstEncounter[i].Id;
        //                                lstPreviousEncounter = lstEncounter.Where(a => a.Id == previousEncounterId)
        //                                                                   .ToList<Encounter>();
        //                                break;
        //                            }
        //                        }
        //                        if (!isPhysicianProcess)
        //                        {
        //                            lstPreviousEncounter = lstEncounter.Where(a => a.Id == lstEncounter[0].Id)
        //                                                                   .ToList<Encounter>();
        //                            return lstPreviousEncounter;
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    previousEncounterId = 0;
        //                }

        //                iMySession.Close();
        //            }
        //            return lstPreviousEncounter;
        //        }




        public IList<Encounter> GetPreviousEncounterDetails(ulong ulEncounterID, ulong uHumanID,
                                                    ulong uPhysicianID, out bool isPhysicianProcess, out bool isFromArchive)
        {
            IList<Encounter> lstPreviousEncounter = new List<Encounter>();
            WFObjectManager objWFObjectManager = new WFObjectManager();

            isPhysicianProcess = false;
            isFromArchive = false;

            if (uHumanID == 0)
            {
                return lstPreviousEncounter;
            }

            ulong previousEncounterId = 0;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    //Jira CAP-1665 - Old Code
                    //                    var querySQL = @"SELECT E.*
                    //                                     FROM   ENCOUNTER E
                    //                                     WHERE  E.HUMAN_ID=:HUMAN_ID
                    //                                     AND    E.ENCOUNTER_PROVIDER_ID=:PHYSICIAN_ID
                    //                                     AND    E.DATE_OF_SERVICE <>'0001-01-01 00:00:00'
                    //                                     AND    E.ENCOUNTER_ID <> :ENCOUNTER_ID
                    //                                     AND    E.IS_PHONE_ENCOUNTER <> 'Y'
                    //AND E.ENCOUNTER_PROVIDER_SIGNED_DATE<>'0001-01-01 00:00:00'
                    //                                     AND    E.DATE_OF_SERVICE <
                    //                                            ( SELECT   E.DATE_OF_SERVICE
                    //                                              FROM     ENCOUNTER E 
                    //                                              WHERE    E.ENCOUNTER_ID=:ENCOUNTER_ID )";




                    //                    var queryarc = @"SELECT E.*
                    //                                     FROM   ENCOUNTER_ARC E 
                    //                                     WHERE  E.HUMAN_ID=:HUMAN_ID
                    //                                     AND    E.ENCOUNTER_PROVIDER_ID=:PHYSICIAN_ID
                    //                                     AND    E.DATE_OF_SERVICE <>'0001-01-01 00:00:00'
                    //                                     AND    E.ENCOUNTER_ID <> :ENCOUNTER_ID
                    //                                     AND    E.IS_PHONE_ENCOUNTER <> 'Y'
                    //                                    AND E.ENCOUNTER_PROVIDER_SIGNED_DATE<>'0001-01-01 00:00:00'";

                    //Jira CAP-1665 - New Code
                    var querySQL = @"SELECT E.*
                                     FROM   ENCOUNTER E
                                     WHERE  E.HUMAN_ID=:HUMAN_ID
                                     AND    E.ENCOUNTER_PROVIDER_ID=:PHYSICIAN_ID
                                     AND    E.DATE_OF_SERVICE <>'0001-01-01 00:00:00'
                                     AND    E.ENCOUNTER_ID <> :ENCOUNTER_ID
                                     AND    E.IS_PHONE_ENCOUNTER <> 'Y'
                                     AND    E.Is_Signed_in_Akido_Note <> 'Y'
AND E.ENCOUNTER_PROVIDER_SIGNED_DATE<>'0001-01-01 00:00:00'
                                     AND    E.DATE_OF_SERVICE <
                                            ( SELECT   E.DATE_OF_SERVICE
                                              FROM     ENCOUNTER E 
                                              WHERE    E.ENCOUNTER_ID=:ENCOUNTER_ID )";




                    var queryarc = @"SELECT E.*
                                     FROM   ENCOUNTER_ARC E 
                                     WHERE  E.HUMAN_ID=:HUMAN_ID
                                     AND    E.ENCOUNTER_PROVIDER_ID=:PHYSICIAN_ID
                                     AND    E.DATE_OF_SERVICE <>'0001-01-01 00:00:00'
                                     AND    E.ENCOUNTER_ID <> :ENCOUNTER_ID
                                     AND    E.IS_PHONE_ENCOUNTER <> 'Y'
                                     AND    E.Is_Signed_in_Akido_Note <> 'Y'
                                    AND E.ENCOUNTER_PROVIDER_SIGNED_DATE<>'0001-01-01 00:00:00'
                                    AND    E.DATE_OF_SERVICE <
                                            ( SELECT   E.DATE_OF_SERVICE
                                              FROM     ENCOUNTER_ARC E 
                                              WHERE    E.ENCOUNTER_ID=:ENCOUNTER_ID )";



                    var querySQLAkidoEnc = @"SELECT E.*
                                     FROM   ENCOUNTER E , WF_OBJECT WF
                                     WHERE  E.HUMAN_ID=:HUMAN_ID
                                     AND    E.ENCOUNTER_PROVIDER_ID=:PHYSICIAN_ID
                                     AND    E.DATE_OF_SERVICE <>'0001-01-01 00:00:00'
                                     AND    E.ENCOUNTER_ID <> :ENCOUNTER_ID
                                     AND    E.IS_PHONE_ENCOUNTER <> 'Y'
                                     AND    E.IS_SIGNED_IN_AKIDO_NOTE = 'Y'
									 AND (WF.OBJ_TYPE ='DOCUMENTATION' AND WF.OBJ_SYSTEM_ID = E.ENCOUNTER_ID)
									 AND WF.CURRENT_PROCESS = 'DOCUMENT_COMPLETE'
                                     AND    E.DATE_OF_SERVICE <
                                            ( SELECT   E.DATE_OF_SERVICE
                                              FROM     ENCOUNTER E 
                                              WHERE    E.ENCOUNTER_ID=:ENCOUNTER_ID )";




                    var queryarcSQLAkidoEnc = @"SELECT E.*
                                     FROM   ENCOUNTER_ARC E , WF_OBJECT_ARC WF
                                     WHERE  E.HUMAN_ID=:HUMAN_ID
                                     AND    E.ENCOUNTER_PROVIDER_ID=:PHYSICIAN_ID
                                     AND    E.DATE_OF_SERVICE <>'0001-01-01 00:00:00'
                                     AND    E.ENCOUNTER_ID <> :ENCOUNTER_ID
                                     AND    E.IS_PHONE_ENCOUNTER <> 'Y'
                                     AND    E.IS_SIGNED_IN_AKIDO_NOTE = 'Y'
									 AND (WF.OBJ_TYPE ='DOCUMENTATION' AND WF.OBJ_SYSTEM_ID = E.ENCOUNTER_ID)
									 AND WF.CURRENT_PROCESS = 'DOCUMENT_COMPLETE'
                                     AND    E.DATE_OF_SERVICE <
                                            ( SELECT   E.DATE_OF_SERVICE
                                              FROM     ENCOUNTER_ARC E 
                                              WHERE    E.ENCOUNTER_ID=:ENCOUNTER_ID )";


                    var SQLQuery = iMySession.CreateSQLQuery(querySQL)
                           .AddEntity("E", typeof(Encounter));

                    SQLQuery.SetParameter("ENCOUNTER_ID", ulEncounterID);
                    SQLQuery.SetParameter("HUMAN_ID", uHumanID);
                    SQLQuery.SetParameter("PHYSICIAN_ID", uPhysicianID);


                    var SQLQueryarc = iMySession.CreateSQLQuery(queryarc)
                           .AddEntity("E", typeof(Encounter));

                    SQLQueryarc.SetParameter("ENCOUNTER_ID", ulEncounterID);
                    SQLQueryarc.SetParameter("HUMAN_ID", uHumanID);
                    SQLQueryarc.SetParameter("PHYSICIAN_ID", uPhysicianID);


                    var SQLQueryAkidoEnc = iMySession.CreateSQLQuery(querySQLAkidoEnc)
                           .AddEntity("E", typeof(Encounter));

                    SQLQueryAkidoEnc.SetParameter("ENCOUNTER_ID", ulEncounterID);
                    SQLQueryAkidoEnc.SetParameter("HUMAN_ID", uHumanID);
                    SQLQueryAkidoEnc.SetParameter("PHYSICIAN_ID", uPhysicianID);


                    var SQLQueryarcAkidoEnc = iMySession.CreateSQLQuery(queryarcSQLAkidoEnc)
                           .AddEntity("E", typeof(Encounter));

                    SQLQueryarcAkidoEnc.SetParameter("ENCOUNTER_ID", ulEncounterID);
                    SQLQueryarcAkidoEnc.SetParameter("HUMAN_ID", uHumanID);
                    SQLQueryarcAkidoEnc.SetParameter("PHYSICIAN_ID", uPhysicianID);



                    IList<Encounter> lstEncounter = new List<Encounter>();
                    IList<Encounter> lstEncounterarc = new List<Encounter>();
                    IList<Encounter> lstAkidoEncounter = new List<Encounter>();
                    IList<Encounter> lstAkidoEncounterarc = new List<Encounter>();

                    lstEncounter = SQLQuery.List<Encounter>();
                    lstEncounterarc = SQLQueryarc.List<Encounter>();
                    lstAkidoEncounter = SQLQueryAkidoEnc.List<Encounter>();
                    lstAkidoEncounterarc = SQLQueryarcAkidoEnc.List<Encounter>();

                    if (lstEncounter.Count > 0)
                    {
                        for (int i = 0; i < lstEncounter.Count; i++)
                        {
                            lstEncounter[i].Notes = "ENCOUNTER";
                        }
                    }
                    if (lstEncounterarc.Count > 0)
                    {
                        for (int i = 0; i < lstEncounterarc.Count; i++)
                        {
                            lstEncounterarc[i].Notes = "ENCOUNTER_ARC";
                        }
                    }
                    if (lstAkidoEncounter.Count > 0)
                    {
                        for (int i = 0; i < lstAkidoEncounter.Count; i++)
                        {
                            lstAkidoEncounter[i].Notes = "ENCOUNTER";
                        }
                    }
                    if (lstAkidoEncounterarc.Count > 0)
                    {
                        for (int i = 0; i < lstAkidoEncounterarc.Count; i++)
                        {
                            lstAkidoEncounterarc[i].Notes = "ENCOUNTER_ARC";
                        }
                    }

                    lstEncounter = lstEncounter.Concat(lstEncounterarc).ToList<Encounter>();
                    lstEncounter = lstEncounter.Concat(lstAkidoEncounter).ToList<Encounter>();
                    lstEncounter = lstEncounter.Concat(lstAkidoEncounterarc).ToList<Encounter>();
                    lstEncounter = (from s in lstEncounter
                                    orderby s.Date_of_Service descending
                                    select s).Take(1).ToList<Encounter>();


                    if (lstEncounter.Count == 0)
                    {
                        previousEncounterId = 0;
                    }
                    else
                    {
                        isPhysicianProcess = true;
                        if (lstEncounter[0].Notes.ToUpper() == "ENCOUNTER")
                        {
                            isFromArchive = false;
                        }
                        if (lstEncounter[0].Notes.ToUpper() == "ENCOUNTER_ARC")
                        {
                            isFromArchive = true;
                        }
                        previousEncounterId = lstEncounter[0].Id;
                        lstPreviousEncounter = lstEncounter;

                    }

                }
                catch
                {
                    previousEncounterId = 0;
                }

                iMySession.Close();
            }
            return lstPreviousEncounter;
        }


        public FillAppointment GetAppointmentsByDatePhysicianFacility(DateTime DOS_Start, DateTime DOS_End, ulong[] PhyID, string[] FacName, string sView, out string time_taken, bool isFO_MASetup, bool IsAncillary)
        {

            FillAppointment FillApptList = new FillAppointment();
            time_taken = "";
            if ((DOS_Start.ToShortDateString() != DateTime.MinValue.ToShortDateString()) && DOS_End.ToShortDateString() != DateTime.MinValue.ToShortDateString())
            {
                try
                {
                    string tempDate = DOS_Start.AddDays(-2).ToString("yyyy-MM-dd");
                }
                catch (Exception e)
                {
                    throw (e);
                }

                ArrayList aryAppointmentList = null;
                Encounter objAppointment;

                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                {
                    IQuery query = null;
                    //string sFacility = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"];
                    //if (sFacility != null && sFacility.ToUpper() == FacName.FirstOrDefault().ToUpper())
                    var facAncillary = from f in NHibernateSessionUtility.Instance.MyAncillaryFacilityList where f.Fac_Name == FacName.FirstOrDefault().ToUpper() select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0)
                        query = iMySession.GetNamedQuery("GetAppointmentsByDatePhysicianFacilityCMGLab");
                    else
                        query = iMySession.GetNamedQuery("GetAppointmentsByDatePhysicianFacility");

                    if (sView == "SwitchToWeekView")
                    {
                        //query.SetString(0, DOS_Start.AddDays(((int)DayOfWeek.Sunday) - ((int)DOS_Start.DayOfWeek)).ToString("yyyy-MM-dd HH:mm:ss"));
                        //query.SetString(1, DOS_End.AddDays(((int)DayOfWeek.Saturday) - ((int)DOS_Start.DayOfWeek)).ToString("yyyy-MM-dd HH:mm:ss"));
                        query.SetString("fromdate", DOS_Start.AddDays(((int)DayOfWeek.Sunday) - ((int)DOS_Start.DayOfWeek)).ToString("yyyy-MM-dd HH:mm:ss"));
                        query.SetString("todate", DOS_End.AddDays(((int)DayOfWeek.Saturday) - ((int)DOS_Start.DayOfWeek)).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else if (sView == "SwitchToMonthView")
                    {
                        DateTime StartOfMonth = new DateTime(DOS_Start.Year, DOS_Start.Month, 1, DOS_Start.Hour, DOS_Start.Minute, DOS_Start.Second);
                        DateTime EndOfMonth = StartOfMonth.AddMonths(1).AddDays(-1);
                        //query.SetString(0, StartOfMonth.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss"));
                        //query.SetString(1, EndOfMonth.AddDays(7).ToString("yyyy-MM-dd HH:mm:ss"));
                        query.SetString("fromdate", StartOfMonth.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss"));
                        query.SetString("todate", EndOfMonth.AddDays(7).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        //query.SetString(0, DOS_Start.ToString("yyyy-MM-dd HH:mm:ss"));
                        //query.SetString(1, DOS_End.ToString("yyyy-MM-dd HH:mm:ss"));
                        query.SetString("fromdate", DOS_Start.ToString("yyyy-MM-dd HH:mm:ss"));
                        query.SetString("todate", DOS_End.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    query.SetParameterList("PhyList", PhyID);
                    query.SetParameterList("FacList", FacName);

                    Stopwatch LoadApptsDBCall = new Stopwatch();
                    LoadApptsDBCall.Start();
                    aryAppointmentList = new ArrayList(query.List());
                    LoadApptsDBCall.Stop();
                    time_taken = "LoadAppts DB Call Time : " + LoadApptsDBCall.Elapsed.Seconds + "." + LoadApptsDBCall.Elapsed.Milliseconds + "s. ";
                    OrdersManager OrderMngr = new OrdersManager();
                    Orders objOrder = new Orders();
                    IList<string> ilstOrder = new List<string>();
                    string sPhyName = string.Empty;
                    if (aryAppointmentList != null)
                    {
                        for (int i = 0; i < aryAppointmentList.Count; i++)
                        {
                            object[] oj = (object[])aryAppointmentList[i];
                            objAppointment = new Encounter();
                            FillApptList.EncounterId.Add(Convert.ToUInt64(oj[0]));
                            FillApptList.Appointment_Provider_ID.Add(Convert.ToInt32(oj[1]));
                            FillApptList.Human_ID.Add(Convert.ToUInt64(oj[2]));
                            FillApptList.Appointment_Date.Add(Convert.ToDateTime(oj[3]));
                            FillApptList.Duration_Minutes.Add(Convert.ToInt32(oj[4]));
                            FillApptList.ApptStatus.Add(oj[5].ToString());
                            string sPatientName = Convert.ToString(oj[6]) + "," + Convert.ToString(oj[7]) + "  " + Convert.ToString(oj[8]) + "  " + Convert.ToString(oj[10]);

                            if (oj[11] == null)
                                oj[11] = string.Empty;
                            if (oj[12] == null)
                                oj[12] = string.Empty;
                            if (oj[13] == null)
                                oj[13] = string.Empty;
                            if (oj[14] == null)
                                oj[14] = string.Empty;
                            if (oj[15] == null)
                                oj[15] = string.Empty;
                            if (oj[16] == null)
                                oj[16] = string.Empty;

                            if (oj[17] != null)
                                FillApptList.E_Super_Bill.Add(oj[17].ToString());
                            else
                                FillApptList.E_Super_Bill.Add(string.Empty);

                            if (oj[18] != null)
                                FillApptList.Birth_Date.Add(Convert.ToDateTime(oj[18].ToString()));
                            if (oj[19] != null)
                                FillApptList.Human_Type.Add(oj[19].ToString());
                            // if (sFacility != null && sFacility.ToUpper() == FacName.FirstOrDefault().ToUpper())
                            if (ilstFacAncillary.Count > 0)
                                FillApptList.PhysicianName.Add(oj[12].ToString() + " - " + oj[13].ToString() + " " + oj[14].ToString());
                            else
                            {
                                //Old Code
                                //FillApptList.PhysicianName.Add(oj[11].ToString() + " " + oj[12].ToString() + " " + oj[13].ToString() + " " + oj[14].ToString());
                                //Gitlab# 2485 - Physician Name Display Change
                                sPhyName = string.Empty;
                                if (oj[14].ToString() != String.Empty)
                                    sPhyName += oj[14].ToString();
                                if (oj[12].ToString() != String.Empty)
                                {
                                    if (sPhyName != String.Empty)
                                        sPhyName += "," + oj[12].ToString();
                                    else
                                        sPhyName += oj[12].ToString();
                                }
                                if (oj[13].ToString() != String.Empty)
                                    sPhyName += " " + oj[13].ToString();
                                if (oj[15].ToString() != String.Empty)
                                    sPhyName += "," + oj[15].ToString();
                                FillApptList.PhysicianName.Add(sPhyName);
                            }


                            FillApptList.FacilityName.Add(oj[16].ToString());
                            FillApptList.PatientName.Add(sPatientName);
                            FillApptList.Is_Batch_Created.Add(oj[21].ToString());
                            if (oj[22] != null)
                            {
                                if (oj[22] != "" && Convert.ToInt32(oj[22]) != 0)
                                {
                                    FillApptList.Outstanding_Orders.Add(OrderMngr.GetOrdersByOrderSubmitID(Convert.ToInt32(oj[22])));
                                }
                                else
                                {
                                    FillApptList.Outstanding_Orders.Add("");
                                }
                            }
                            else
                            {
                                FillApptList.Outstanding_Orders.Add("");
                            }
                            //if (oj[23] != null)
                            //FillApptList.Payment_Paid.Add(oj[23].ToString());
                            //if (oj[23] != null)
                            //FillApptList.Payment_Paid.Add(oj[23].ToString());

                            //Gitlab #3552
                            //if (oj[23] != null)
                            //{
                            //    if (oj[23].ToString().Replace(" ", "") == "()-")
                            //    {
                            //        FillApptList.Perform_EV_Status.Add("");
                            //    }
                            //    else
                            //    {
                            //        FillApptList.Perform_EV_Status.Add(oj[23].ToString());
                            //    }
                            //}
                            //else
                            //    FillApptList.Perform_EV_Status.Add("");

                            //if (oj[24] != null)
                            //{


                            //    FillApptList.EVMode.Add(oj[24].ToString());
                            //}
                            //else
                            //    FillApptList.EVMode.Add("");

                            //if (oj[26] != null)
                            //    FillApptList.Payment_Paid.Add(oj[26].ToString());

                            //if (oj[25] != null)
                            //    FillApptList.TypeofVisit.Add(oj[25].ToString());

                            //if (oj[26] != null)
                            //    FillApptList.Is_ACO_Eligible.Add(oj[26].ToString());


                            if (oj[24] != null)
                                FillApptList.Payment_Paid.Add(oj[24].ToString());

                            if (oj[23] != null)
                                FillApptList.TypeofVisit.Add(oj[23].ToString());

                            if (oj[24] != null)
                                FillApptList.Is_ACO_Eligible.Add(oj[24].ToString());

                            if (oj[25] != null && oj[26]!=null && oj[26].ToString().ToUpper() == "Y")
                            {
                                FillApptList.Preferred_Language.Add(oj[25].ToString()+" req.");
                            }
                            else
                            {
                                FillApptList.Preferred_Language.Add("");
                            }

                            //FillApptList.Document_Type.Add(oj[22].ToString());

                        }
                    }

                    if (isFO_MASetup && FillApptList.Human_ID.ToArray<ulong>().Count() > 0)
                    {
                        PatientInsuredPlanManager patMngr = new PatientInsuredPlanManager();
                        IList<InsurancePlan> objIPList = new List<InsurancePlan>();
                        IList<InsurancePlan> objIP = new List<InsurancePlan>();
                        IList<PatientInsuredPlan> objPlanIns = new List<PatientInsuredPlan>();

                        Stopwatch InsPlanDBCall = new Stopwatch();
                        InsPlanDBCall.Start();
                        //  objPlanIns = patMngr.GetPlanbyHumanID(FillApptList.Human_ID.ToArray<ulong>(), out objIPList);//remove1
                        #region New Code
                        ulong[] human_ID = FillApptList.Human_ID.ToArray<ulong>();
                        ArrayList arryChkList = null;
                        IQuery query1 = iMySession.GetNamedQuery("Get.InsurancePlanAndPatientPlanByID");
                        query1.SetParameterList("HumanID", human_ID);
                        arryChkList = new ArrayList(query1.List());

                        if (arryChkList != null)
                        {
                            for (int i = 0; i < arryChkList.Count; i++)
                            {
                                PatientInsuredPlan objPlan = new PatientInsuredPlan();
                                object[] oj = (object[])arryChkList[i];
                                objPlan.Insurance_Plan_ID = Convert.ToUInt64(oj[1].ToString());
                                objPlan.Human_ID = Convert.ToUInt64(oj[2].ToString());
                                objPlanIns.Add(objPlan);

                                InsurancePlan objInsPlan = new InsurancePlan();
                                objInsPlan.Id = Convert.ToUInt64(oj[1].ToString());
                                objInsPlan.Carrier_ID = Convert.ToInt32(oj[0].ToString());
                                objIPList.Add(objInsPlan);
                            }
                        }
                        #endregion
                        InsPlanDBCall.Stop();
                        time_taken += "InsPlan DB Call Time : " + InsPlanDBCall.Elapsed.Seconds + "." + InsPlanDBCall.Elapsed.Milliseconds + "s. ";

                        string[] sPlanid = { "21", "73", "90", "93", "115" };
                        for (int l = 0; l < FillApptList.Human_ID.Count; l++)
                        {
                            if (objPlanIns.Any(b => b.Human_ID.ToString() == FillApptList.Human_ID[l].ToString()) == true)
                            {
                                ulong ulplanID = objPlanIns.Where(d => d.Human_ID == FillApptList.Human_ID[l]).Select(e => e.Insurance_Plan_ID).ToList<ulong>()[0];
                                objIP = objIPList.Where(item => item.Id == ulplanID).ToList<InsurancePlan>();
                                if (objIP.Count > 0)
                                {
                                    if (sPlanid.Any(f => f.ToString() == objIP[0].Carrier_ID.ToString()) == true)
                                        FillApptList.Is_Medicare_Plan.Add("Y");
                                    else
                                        FillApptList.Is_Medicare_Plan.Add("N");
                                }
                                else
                                    FillApptList.Is_Medicare_Plan.Add("N");
                            }
                            else
                                FillApptList.Is_Medicare_Plan.Add("N");
                        }
                    }

                    BlockdaysManager blockMngr = new BlockdaysManager();
                    Stopwatch BlockDaysDBCall = new Stopwatch();
                    ArrayList aryBlockList = null;
                    IList<Blockdays> blockList = new List<Blockdays>();
                    BlockDaysDBCall.Start();
                    // FillApptList.blockList = blockMngr.GetBlockDaysUsingPhyFacApptDt(PhyID, FacName, DOS_Start, DOS_End, sView);//remove2
                    # region New Code
                    if (sView == "SwitchToWeekView")
                    {
                        IQuery query1 = null;
                        if (IsAncillary)
                            query1 = iMySession.GetNamedQuery("Get.BlockDays.DateandTechByWeekandMonth");
                        else
                            query1 = iMySession.GetNamedQuery("Get.BlockDays.DateandPhyByWeekandMonth");
                        query1.SetString(0, DOS_Start.AddDays(((int)DayOfWeek.Sunday) - ((int)DOS_Start.DayOfWeek)).ToString("yyyy-MM-dd HH:mm:ss"));
                        query1.SetString(1, DOS_End.AddDays(((int)DayOfWeek.Saturday) - ((int)DOS_Start.DayOfWeek)).ToString("yyyy-MM-dd HH:mm:ss"));
                        query1.SetParameterList("FacList", FacName);
                        query1.SetParameterList("PhyList", PhyID);
                        aryBlockList = new ArrayList(query1.List());
                    }
                    else if (sView == "SwitchToMonthView")
                    {
                        IQuery query1 = null;
                        if (IsAncillary)
                            query1 = iMySession.GetNamedQuery("Get.BlockDays.DateandTechByWeekandMonth");
                        else
                            query1 = iMySession.GetNamedQuery("Get.BlockDays.DateandPhyByWeekandMonth");
                        DateTime StartOfMonth = new DateTime(DOS_Start.Year, DOS_Start.Month, 1, DOS_Start.Hour, DOS_Start.Minute, DOS_Start.Second);
                        DateTime EndOfMonth = StartOfMonth.AddMonths(1).AddDays(-1);
                        query1.SetString(0, StartOfMonth.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss"));
                        query1.SetString(1, EndOfMonth.AddDays(7).ToString("yyyy-MM-dd HH:mm:ss"));
                        query1.SetParameterList("FacList", FacName);
                        query1.SetParameterList("PhyList", PhyID);
                        aryBlockList = new ArrayList(query1.List());
                    }
                    else
                    {
                        IQuery query2 = null;
                        if (IsAncillary)
                            query2 = iMySession.GetNamedQuery("Get.BlockDays.DateandTech");
                        else
                            query2 = iMySession.GetNamedQuery("Get.BlockDays.DateandPhy");
                        query2.SetString(0, DOS_Start.ToString("yyyy-MM-dd"));
                        query2.SetParameterList("FacList", FacName);
                        query2.SetParameterList("PhyList", PhyID);
                        aryBlockList = new ArrayList(query2.List());
                    }

                    Blockdays block = null;
                    if (aryBlockList != null)
                    {
                        for (int i = 0; i < aryBlockList.Count; i++)
                        {
                            object[] oj = (object[])aryBlockList[i];
                            block = new Blockdays();
                            block.Id = Convert.ToUInt64(oj[0]);
                            if (IsAncillary)
                                block.Machine_Technician_Library_ID = Convert.ToUInt64(oj[14]);
                            else
                                block.Physician_ID = Convert.ToUInt64(oj[1]);
                            block.Facility_Name = oj[2].ToString();
                            block.Block_Date = Convert.ToDateTime(oj[3]);
                            block.From_Time = oj[4].ToString();
                            block.To_Time = oj[5].ToString();
                            block.From_Date_Choosen = Convert.ToDateTime(oj[6].ToString());
                            block.To_Date_Choosen = Convert.ToDateTime(oj[7].ToString());
                            block.Reason = oj[8].ToString();
                            block.Day_Choosen = oj[9].ToString();
                            block.Block_Type = oj[10].ToString();


                            blockList.Add(block);
                        }
                    }
                    FillApptList.blockList = blockList;

                    #endregion
                    BlockDaysDBCall.Stop();
                    time_taken += "Block Days DB Call Time : " + BlockDaysDBCall.Elapsed.Seconds + "." + BlockDaysDBCall.Elapsed.Milliseconds + "s. ";
                    iMySession.Close();
                }
            }
            return FillApptList;
        }

        public ulong GetPreviousEncounterID(ulong uPhysicianID, ulong uHumanID, ulong uCurrentEncounterID)
        {
            ulong uPreviousEncounterID = 0;
            IList<FileManagementIndex> list_image = new List<FileManagementIndex>();
            int phy_id = Convert.ToInt32(uPhysicianID);
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    uPreviousEncounterID = mySession.CreateCriteria(typeof(Encounter))
                   .SetProjection(Projections.Max("Id"))
                   .Add(Expression.Eq("Appointment_Provider_ID", phy_id))
                   .Add(Expression.Between("Id", 0, (uCurrentEncounterID - 1))).Add(!Expression.Eq("Date_of_Service", DateTime.MinValue))
                   .Add(Expression.Eq("Human_ID", uHumanID)).Add(!Expression.Eq("Id", uCurrentEncounterID)).List<ulong>()[0];
                }
                catch
                {
                    uPreviousEncounterID = 0;
                }
            }
            return uPreviousEncounterID;
        }

        public Encounter GetEncounterByEncounterIDArchive(ulong EncounterID)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            Encounter objencounter = new Encounter();
            // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sqlquery = iMySession.CreateSQLQuery("SELECT enc.* FROM encounter enc where enc.encounter_id=" + EncounterID.ToString() +
                    " union all SELECT arc.* FROM encounter_arc arc  where arc.encounter_id=" + EncounterID.ToString() + ";").AddEntity("e", typeof(Encounter));
                ilstEncounter = sqlquery.List<Encounter>();
                iMySession.Close();

                if (ilstEncounter.Count > 0)
                {
                    objencounter = ilstEncounter[0];
                }
            }
            return objencounter;
            // return crit.List<Encounter>();
        }
        //BugID:47782 START
        public void GetEandMCount(ulong EncounterID, string UserRole, out bool isEandMCPTPresent, out bool isEandMICDPresent, out bool isEandMPriICDPresent)
        {
            EAndMCodingManager emMnger = new EAndMCodingManager();
            EandMCodingICDManager emICDMngr = new EandMCodingICDManager();
            int EMCPT_count = 0, EMICD_count = 0;

            EMCPT_count = emMnger.GetEMCodeList(EncounterID).Count();
            if (EMCPT_count > 0)
            {
                isEandMCPTPresent = true;
            }
            else
                isEandMCPTPresent = false;
            EMICD_count = emICDMngr.GetEMICDCount(EncounterID);
            if (EMICD_count > 0)
            {
                isEandMICDPresent = true;
            }
            else
                isEandMICDPresent = false;
            isEandMPriICDPresent = emMnger.IsPrimaryFilled(EncounterID, UserRole);
        }
        //BugID:47782 END

        public IList<ImmunizationHistory> GetEncounterListForBulkExportInImmunizationRegistry(int provider_id, string fromDate, string toDate)
        {
            IList<ImmunizationHistory> ilstImmunization = new List<ImmunizationHistory>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                // ISQLQuery sqlquery = iMySession.CreateSQLQuery("SELECT e.* FROM encounter e where encounter_provider_id=" + provider_id + " and date_of_service >='" + fromDate + "' and date_of_service <='" + toDate + "' group by human_id;").AddEntity("e", typeof(Encounter));
                // ISQLQuery sqlquery = iMySession.CreateSQLQuery("SELECT e.* FROM encounter e,immunization_history i where e.human_id=i.human_id and e.encounter_provider_id=" + provider_id + " and e.date_of_service >='" + fromDate + "' and e.date_of_service <='" + toDate + "'  group by e.human_id,e.encounter_id").AddEntity("e", typeof(Encounter));
                //ilstEncounter = sqlquery.List<Encounter>();
                ISQLQuery sqlquery = iMySession.CreateSQLQuery("SELECT i.* FROM immunization_history i where i.physician_id=" + provider_id + " and date_format(STR_TO_DATE(i.administered_date, '%d-%M-%Y'),'%Y-%m-%d') >='" + fromDate + "' and date_format(STR_TO_DATE(i.administered_date, '%d-%M-%Y'),'%Y-%m-%d') <='" + toDate + "'").AddEntity("i", typeof(ImmunizationHistory));
                ilstImmunization = sqlquery.List<ImmunizationHistory>();
                iMySession.Close();
            }
            return ilstImmunization;
            // return sqlquery.List<Encounter>();
        }
        //BugID:49297 
        //BugID:49585
        public ArrayList GetEncounterListByDOSRange(ulong humanId, string fromDate, string toDate)
        {
            ArrayList EncIDs = new ArrayList();
            fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
            toDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
            using (ISession imysession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = imysession.GetNamedQuery("Get.EncounterIDListByDOS");
                query.SetString(0, humanId.ToString());
                query.SetString(1, fromDate);
                query.SetString(2, toDate);
                query.SetString(3, humanId.ToString());
                query.SetString(4, fromDate);
                query.SetString(5, toDate);
                EncIDs = new ArrayList(query.List());
            }
            return EncIDs;
        }

        public ArrayList GetPatientListBulkAccess(string fromdate, string todate, string PhyId)
        {
            string Fromdate1 = fromdate;
            string todate1 = todate;
            string PhyId1 = PhyId;
            ArrayList objPatientLstOutput = new ArrayList();

            using (ISession imysession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = session.GetISession().GetNamedQuery("Get.Demographics.PatientDetailsBulkAccess");
                query.SetParameter(0, Fromdate1);
                query.SetParameter(1, todate1);
                query.SetParameter(2, PhyId1);
                objPatientLstOutput = new ArrayList(query.List());
            }
            return objPatientLstOutput;
        }

        public bool GetEligibleEncCaseReporting(out IList<string> EligibleEncList)
        {
            bool Is_Eligible = false;
            EligibleEncList = new List<string>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = session.GetISession().GetNamedQuery("Get.CaseReporting.EligibleEncList");
                EligibleEncList = query.List<string>();
            }
            if (EligibleEncList.Count > 0)
            {
                Is_Eligible = true;
            }

            return Is_Eligible;
        }
        //Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
        public string TriggerSPforProvReviewStatusTracker(string mode, ulong encounter_id)
        {
            return string.Empty;

            ArrayList objProvRevTracker = new ArrayList();
            try
            {
                using (ISession iSession = NHibernateSessionManager.Instance.CreateISession())
                {
                    IQuery iMyQuery = iSession.GetNamedQuery("SP.ReCalculate.ProviderReviewStatus");
                    iMyQuery.SetParameter("input_mode", mode);
                    iMyQuery.SetParameter("enc_id", encounter_id);
                    objProvRevTracker = new ArrayList(iMyQuery.List());
                    iSession.Close();
                    return string.Empty;
                }
            }

            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return "ERROR~" + Convert.ToString(ex.InnerException.Message).Replace(Environment.NewLine, string.Empty);
                }

                return "ERROR~" + Convert.ToString(ex.Message).Replace(Environment.NewLine, string.Empty);
            }
        }

        public IList<Encounter> GetOrderingPhysicianByOrderSubmitID(ulong iOrderSubmit)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sqlquery = iMySession.CreateSQLQuery("SELECT i.* FROM encounter i where i.order_submit_id=" + iOrderSubmit + "").AddEntity("i", typeof(Encounter));
                ilstEncounter = sqlquery.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
        }


        public IList<Encounter> GetAppointmentForBulkEncounterTemplate(string ProviderId, DateTime DOS, string facilityName)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            ArrayList EncounterList = null;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("GetAppointmentForBulkExport");
                query.SetString(0, DOS.ToString("yyyy-MM-dd") + "%");
                query.SetString(1, facilityName);
                query.SetParameterList("ProviderId", ProviderId.Split(',').ToArray<string>());
                EncounterList = new ArrayList(query.List());

                Encounter ObjEnc = null;
                if (EncounterList != null)
                {
                    for (int i = 0; i < EncounterList.Count; i++)
                    {
                        ObjEnc = new Encounter();
                        object[] obj = (object[])EncounterList[i];

                        if (obj[0] != null) ObjEnc.Encounter_ID = Convert.ToUInt64(obj[0]);
                        if (obj[1] != null) ObjEnc.Human_ID = Convert.ToUInt64(obj[1].ToString());
                        if (obj[2] != null) ObjEnc.Encounter_Provider_ID = Convert.ToInt32(obj[2].ToString());
                        if (obj[3] != null) ObjEnc.Appointment_Date = Convert.ToDateTime(obj[3].ToString());
                        ilstEncounter.Add(ObjEnc);
                    }
                }
                iMySession.Close();
            }
            return ilstEncounter;
        }
        //sFacility, sSelectedDate, sPhyName
        public IList<Encounter> GetAppointmentForBulkEncounterTemplateFacility(string Facility, DateTime DOS, string sPhyName)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            ArrayList EncounterList = null;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("GetAppointmentForBulkExportFacilty");
                query.SetString(0, DOS.ToString("yyyy-MM-dd") + "%");
                query.SetString(1, sPhyName);
                query.SetParameterList("FacilityName", Facility.Split('|').ToArray<string>());
                EncounterList = new ArrayList(query.List());

                Encounter ObjEnc = null;
                if (EncounterList != null)
                {
                    for (int i = 0; i < EncounterList.Count; i++)
                    {
                        ObjEnc = new Encounter();
                        object[] obj = (object[])EncounterList[i];

                        if (obj[0] != null) ObjEnc.Encounter_ID = Convert.ToUInt64(obj[0]);
                        if (obj[1] != null) ObjEnc.Human_ID = Convert.ToUInt64(obj[1].ToString());
                        if (obj[2] != null) ObjEnc.Encounter_Provider_ID = Convert.ToInt32(obj[2].ToString());
                        if (obj[3] != null) ObjEnc.Facility_Name = obj[3].ToString();
                        if (obj[4] != null) ObjEnc.Appointment_Date = Convert.ToDateTime(obj[4].ToString());
                        ilstEncounter.Add(ObjEnc);
                    }
                }
                iMySession.Close();
            }
            return ilstEncounter;
        }


        public void UpdateDeleteDocumentation(Encounter EncRecord, string MacAddress, WFObject ehrwfobj, DateTime StartTime, string sOwner)
        {
            UpdateEncounterList(EncRecord, string.Empty);
            WFObjectManager objWfObjectMngr = new WFObjectManager();
            //objWfObjectMngr.DeleteDocumentation(ehrwfobj.Obj_System_Id, ehrwfobj.Obj_Type, sOwner, string.Empty);
            objWfObjectMngr.MoveToNextProcess(ehrwfobj.Obj_System_Id, ehrwfobj.Obj_Type, 4, sOwner, StartTime, ehrwfobj.Current_Process, null, null);
        }


        public IList<Encounter> GetEncounterUsingHumanIDandFacility(ulong ulHumanID, string facility)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Facility_Name", facility)).AddOrder(Order.Desc("Date_of_Service"));
                ilstEncounter = crit.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
            //return crit.List<Encounter>();
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
        public int CalculateAgeByDOS(DateTime birthDate, DateTime DOS)
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


        public IList<Encounter> GetPhoneEncounter(ulong ulHumanID)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Is_Phone_Encounter", "Y")).Add(Expression.Eq("Is_EandM_Submitted", "N")).AddOrder(Order.Desc("Id")).SetMaxResults(1);
                ilstEncounter = crit.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
        }

        public DataTable GetEncountersbyDOSRange(ulong ulHumanID, DateTime dtPatientDOB, string sMemberID, ulong ulCarrierID, DateTime dtFromDate, DateTime dtToDate, string sLegalOrg)
        {
            DataTable dtEncounter = new DataTable();
            ArrayList arylstEncounter = null;
            ArrayList arylstColumnHeadings = null;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.GetEncountersByRange.ColumnHeadings");
                arylstColumnHeadings = new ArrayList(query2.List());

                string sQuery = "SELECT cast(enc.Date_of_Service as char(100)) as DOS, enc.Human_ID as Patient_Account_Number,concat(Last_Name, ',',First_Name,'  ' ,MI,'  ' ,Suffix) as Patient_Name,  " +
    "cast(date_format(h.Birth_Date, '%d-%b-%Y') as char(100)) as Patient_DOB,enc.Facility_Name,concat(ifnull(p.Physician_Prefix,''),' ',ifnull(p.Physician_First_Name,''),' '," +
    "ifnull(p.Physician_Middle_Name,''),' ',ifnull(p.Physician_Last_Name,''),' ',ifnull(p.Physician_Suffix,'')) as Provider_Name," +
"ifnull(pat.policy_holder_id,'') as Member_ID,ifnull(ins.insurance_plan_name,'') as Plan_Name ,enc.Encounter_ID as Encounter_ID, ins.Carrier_ID " +
 "FROM encounter enc left join human h on (enc.human_id=h.human_id) left join physician_library p on (enc.Encounter_Provider_ID = p.physician_library_id)" +
 "left join pat_insured_plan pat on (h.human_id=pat.human_id) left join insurance_plan ins on (pat.insurance_plan_id= ins.insurance_plan_id) where :WhereCondition" +
 "union all " +
 "SELECT cast(enc.Date_of_Service as char(100)) as DOS, enc.Human_ID as Patient_Account_Number,concat(Last_Name, ',',First_Name,'  ' ,MI,'  ' ,Suffix) as Patient_Name,  " +
    "cast(date_format(h.Birth_Date, '%d-%b-%Y') as char(100)) as Patient_DOB,enc.Facility_Name,concat(ifnull(p.Physician_Prefix,''),' ',ifnull(p.Physician_First_Name,''),' '," +
    "ifnull(p.Physician_Middle_Name,''),' ',ifnull(p.Physician_Last_Name,''),' ',ifnull(p.Physician_Suffix,'')) as Provider_Name," +
"ifnull(pat.policy_holder_id,'') as Member_ID,ifnull(ins.insurance_plan_name,'') as Plan_Name ,enc.Encounter_ID as Encounter_ID, ins.Carrier_ID " +
 "FROM encounter_arc enc left join human h on (enc.human_id=h.human_id) left join physician_library p on (enc.Encounter_Provider_ID = p.physician_library_id)" +
 "left join pat_insured_plan pat on (h.human_id=pat.human_id) left join insurance_plan ins on (pat.insurance_plan_id= ins.insurance_plan_id) where :WhereCondition";

                string sWhereCondition = "date(enc.Date_of_Service) between '" + dtFromDate.ToString("yyyy-MM-dd") + "' and '" + dtToDate.ToString("yyyy-MM-dd") + "' h.Legal_Org ='" + sLegalOrg + "'";

                if (ulHumanID != 0)
                    sWhereCondition += "and h.Human_ID = " + ulHumanID.ToString() + " ";
                if (dtPatientDOB != DateTime.MinValue)
                    sWhereCondition += "and h.Birth_Date = '" + dtPatientDOB.ToString("yyyy-MM-dd") + "' ";
                sWhereCondition += "having Carrier_ID =" + ulCarrierID + " ";
                if (sMemberID != string.Empty)
                    sWhereCondition += "and member_id =" + sMemberID + " ";

                sQuery = sQuery.Replace(":WhereCondition", sWhereCondition);

                ISQLQuery sql = iMySession.CreateSQLQuery(sQuery);
                arylstEncounter = new ArrayList(sql.List());

                object[] objlst = (object[])arylstColumnHeadings[0];
                for (int iCount = 0; iCount < objlst.Length; iCount++)
                {
                    dtEncounter.Columns.Add(objlst[iCount].ToString(), typeof(System.String));
                }

                for (int iCount = 0; iCount < arylstEncounter.Count; iCount++)
                {
                    DataRow dr = dtEncounter.NewRow();

                    object[] objColumnlst = (object[])arylstEncounter[iCount];

                    for (int iMyCount = 0; iMyCount < objColumnlst.Length; iMyCount++)
                    {
                        dr[iMyCount] = objColumnlst[iMyCount].ToString();
                    }
                    dtEncounter.Rows.Add(dr);
                }
                iMySession.Close();
            }

            return dtEncounter;
        }

        public DataTable GetEncountersbyDOSRange(ulong ulHumanID, DateTime dtPatientDOB, string sMemberID, ulong ulCarrierID, DateTime dtFromDate, DateTime dtToDate, string sLastName, string sFirstName, string sPlanId, string sLegalOrg)
        {
            DataTable dtEncounter = new DataTable();
            ArrayList arylstEncounter = null;
            ArrayList arylstColumnHeadings = null;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.GetEncountersByRange.ColumnHeadings");
                arylstColumnHeadings = new ArrayList(query2.List());

                //                string sQuery = "select * from (SELECT cast(enc.Date_of_Service as char(100)) as DOS, enc.Human_ID as Patient_Account_Number,concat(Last_Name, ',',First_Name,'  ' ,MI,'  ' ,Suffix) as Patient_Name,  " +
                //    "cast(h.Birth_Date as char(100)) as Patient_DOB,enc.Facility_Name,concat(ifnull(p.Physician_Prefix,''),' ',ifnull(p.Physician_First_Name,''),' '," +
                //    "ifnull(p.Physician_Middle_Name,''),' ',ifnull(p.Physician_Last_Name,''),' ',ifnull(p.Physician_Suffix,'')) as Provider_Name," +
                //"ifnull(pat.policy_holder_id,'') as Member_ID,ifnull(ins.insurance_plan_name,'') as Plan_Name ,enc.Encounter_ID as Encounter_ID, ins.Carrier_ID " +
                // "FROM encounter enc left join human h on (enc.human_id=h.human_id) left join physician_library p on (enc.Encounter_Provider_ID = p.physician_library_id)" +
                // "left join pat_insured_plan pat on (h.human_id=pat.human_id) left join insurance_plan ins on (pat.insurance_plan_id= ins.insurance_plan_id) where :WhereCondition" +
                // "union all " +
                // "SELECT cast(enc.Date_of_Service as char(100)) as DOS, enc.Human_ID as Patient_Account_Number,concat(Last_Name, ',',First_Name,'  ' ,MI,'  ' ,Suffix) as Patient_Name,  " +
                //    "cast(h.Birth_Date as char(100)) as Patient_DOB,enc.Facility_Name,concat(ifnull(p.Physician_Prefix,''),' ',ifnull(p.Physician_First_Name,''),' '," +
                //    "ifnull(p.Physician_Middle_Name,''),' ',ifnull(p.Physician_Last_Name,''),' ',ifnull(p.Physician_Suffix,'')) as Provider_Name," +
                //"ifnull(pat.policy_holder_id,'') as Member_ID,ifnull(ins.insurance_plan_name,'') as Plan_Name ,enc.Encounter_ID as Encounter_ID, ins.Carrier_ID " +
                // "FROM encounter_arc enc left join human h on (enc.human_id=h.human_id) left join physician_library p on (enc.Encounter_Provider_ID = p.physician_library_id)" +
                // "left join pat_insured_plan pat on (h.human_id=pat.human_id) left join insurance_plan ins on (pat.insurance_plan_id= ins.insurance_plan_id) where :WhereCondition ) as d order by DOS";

                string sQuery = "select * from (SELECT concat(Last_Name, ',',First_Name,'  ' ,MI,'  ' ,Suffix) as Patient_Name,cast(h.Birth_Date as char(100)) as Patient_DOB, enc.Human_ID as Patient_Account_Number, ifnull(pat.policy_holder_id,'') as Member_ID,  cast(enc.Date_of_Service as char(100)) as DOS, " +
    "concat(ifnull(p.Physician_Prefix,''),' ',ifnull(p.Physician_First_Name,''),' '," +
    "ifnull(p.Physician_Middle_Name,''),' ',ifnull(p.Physician_Last_Name,''),' ',ifnull(p.Physician_Suffix,'')) as Provider_Name, c.Carrier_Name as Payer, ifnull(ins.insurance_plan_name,'') as Plan_Name , enc.Visit_Type as Type_of_Visit, enc.Facility_Name,  " +
"enc.Encounter_ID as Encounter_ID, ins.Carrier_ID " +
 "FROM encounter enc left join human h on (enc.human_id=h.human_id) left join physician_library p on (enc.Encounter_Provider_ID = p.physician_library_id)" +
 "left join pat_insured_plan pat on (h.human_id=pat.human_id) left join insurance_plan ins on (pat.insurance_plan_id= ins.insurance_plan_id) " +
 "left join carrier c on (c.carrier_id=ins.carrier_id) where :WhereCondition" +
 "union all " +
 "SELECT concat(Last_Name, ',',First_Name,'  ' ,MI,'  ' ,Suffix) as Patient_Name,cast(h.Birth_Date as char(100)) as Patient_DOB, enc.Human_ID as Patient_Account_Number, ifnull(pat.policy_holder_id,'') as Member_ID, cast(enc.Date_of_Service as char(100)) as DOS,   " +
    "concat(ifnull(p.Physician_Prefix,''),' ',ifnull(p.Physician_First_Name,''),' '," +
    "ifnull(p.Physician_Middle_Name,''),' ',ifnull(p.Physician_Last_Name,''),' ',ifnull(p.Physician_Suffix,'')) as Provider_Name,c.Carrier_Name as Payer, ifnull(ins.insurance_plan_name,'') as Plan_Name ,  enc.Visit_Type as Type_of_Visit, enc.Facility_Name,  " +
"enc.Encounter_ID as Encounter_ID, ins.Carrier_ID " +
 "FROM encounter_arc enc left join human h on (enc.human_id=h.human_id) left join physician_library p on (enc.Encounter_Provider_ID = p.physician_library_id)" +
 "left join pat_insured_plan pat on (h.human_id=pat.human_id) left join insurance_plan ins on (pat.insurance_plan_id= ins.insurance_plan_id) " +
 "left join carrier c on (c.carrier_id=ins.carrier_id) where :WhereCondition ) as d order by Patient_Name,Patient_DOB,Patient_Account_Number,Member_ID,DOS,Provider_Name,Payer,Plan_Name,Facility_Name";

                string sWhereCondition = "date(enc.Date_of_Service) between '" + dtFromDate.ToString("yyyy-MM-dd") + "' and '" + dtToDate.ToString("yyyy-MM-dd") + "' and h.Legal_Org ='" + sLegalOrg + "' and pat.Insurance_Type='PRIMARY'";

                if (ulHumanID != 0)
                    sWhereCondition += "and h.Human_ID = " + ulHumanID.ToString() + " ";
                if (sLastName != string.Empty)
                    sWhereCondition += "and h.Last_Name ='" + sLastName + "'  ";
                if (sFirstName != string.Empty)
                    sWhereCondition += "and h.First_Name ='" + sFirstName + "'  ";
                if (sPlanId != string.Empty)
                    sWhereCondition += "and pat.insurance_plan_id in (" + sPlanId + ")  ";
                if (sMemberID != string.Empty)
                    sWhereCondition += "and pat.policy_holder_id like '" + sMemberID + "%' ";
                if (dtPatientDOB != DateTime.MinValue)
                    sWhereCondition += "and h.Birth_Date = '" + dtPatientDOB.ToString("yyyy-MM-dd") + "' ";
                sWhereCondition += "having Carrier_ID =" + ulCarrierID + " ";


                sQuery = sQuery.Replace(":WhereCondition", sWhereCondition);

                ISQLQuery sql = iMySession.CreateSQLQuery(sQuery);
                arylstEncounter = new ArrayList(sql.List());

                object[] objlst = (object[])arylstColumnHeadings[0];
                for (int iCount = 0; iCount < objlst.Length; iCount++)
                {
                    dtEncounter.Columns.Add(objlst[iCount].ToString(), typeof(System.String));
                }

                for (int iCount = 0; iCount < arylstEncounter.Count; iCount++)
                {
                    DataRow dr = dtEncounter.NewRow();

                    object[] objColumnlst = (object[])arylstEncounter[iCount];

                    for (int iMyCount = 0; iMyCount < objColumnlst.Length; iMyCount++)
                    {
                        dr[iMyCount] = objColumnlst[iMyCount].ToString();
                    }
                    dtEncounter.Rows.Add(dr);
                }

                iMySession.Close();
            }

            return dtEncounter;
        }
        public IList<string> GetEncounterListForIndexing(ulong ulHuman_ID, string sMonths)
        {
            ISession iMySessiondesc = NHibernateSessionManager.Instance.CreateISession();
            ArrayList arrList;
            IList<string> sDescList = new List<string>();
            IQuery query = iMySessiondesc.GetNamedQuery("GetEncounterDetailsForIndexing");
            query.SetString(0, ulHuman_ID.ToString());
            query.SetString(1, sMonths);
            query.SetString(2, ulHuman_ID.ToString());
            query.SetString(3, sMonths);
            arrList = new ArrayList(query.List());
            foreach (object[] oj in arrList)
                sDescList.Add(oj[1].ToString() + " ~ " + oj[2].ToString() + "^" + oj[0].ToString());

            return sDescList;
        }
        public void writeCopypreviousencounterblobtable(ulong encounterid, GenerateXml objxml, IList<Encounter> lst)
        {
            ISession MySession = Session.GetISession();
            // ITransaction trans = null;


            using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                if (lst.Count > 0)
                {
                    WriteBlob(encounterid, objxml.itemDoc, MySession, null, lst, null, objxml, false);


                    trans.Commit();
                }
            }


        }
        public void writeCopyprevioushumanblobtable(ulong humanid, IList<Encounter> lst, GenerateXml objhumanxml)
        {
            ISession MySession = Session.GetISession();
            // ITransaction trans = null;


            using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {



                WriteBlob(humanid, objhumanxml.itemDoc, MySession, null, lst, null, objhumanxml, false);
                trans.Commit();

            }


        }

        //Jira #CAP-706 - new Method-SaveandMoveAkidoEncounter
        public void SaveandMoveAkidoEncounter(IList<Encounter> ilstUpdateEncounter)
        {
            ISession MySession = Session.GetISession();
            ITransaction trans = null;
            GenerateXml XMLObj = new GenerateXml();
            IList<Encounter> lstsaveencounter = new List<Encounter>();
            int iResult = 0;
            WFObjectManager wfObjectManager = new WFObjectManager();

        TryAgain:
            try
            {
                trans = MySession.BeginTransaction();

                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref lstsaveencounter, ref ilstUpdateEncounter, null, MySession, String.Empty, true, false, ilstUpdateEncounter[0].Id, string.Empty, ref XMLObj);
                wfObjectManager.MoveToNextProcess(ilstUpdateEncounter[0].Id, "ENCOUNTER", 1, "UNKNOWN", System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), string.Empty, null, MySession);

                //if bResult = false then, the deadlock is occured 
                if (iResult == 2)
                {
                    if (iTryCount < 5)
                    {
                        iTryCount++;
                        goto TryAgain;
                    }
                }


                int trycount = 0;
            trytosaveagain:
                try
                {
                    if (XMLObj.itemDoc.InnerXml != null && XMLObj.itemDoc.InnerXml != "")
                    {
                        try
                        {

                            WriteBlob(ilstUpdateEncounter[0].Id, XMLObj.itemDoc, MySession, lstsaveencounter, ilstUpdateEncounter, null, XMLObj, false);

                        }
                        catch (Exception xmlexcep)
                        {
                            throw xmlexcep;

                        }
                    }

                    trans.Commit();
                }
                catch (Exception xmlexcep)
                {
                    trycount++;
                    if (trycount <= 3)
                    {
                        int TimeMilliseconds = 0;
                        if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                            TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                        Thread.Sleep(TimeMilliseconds);
                        string sMsg = string.Empty;
                        string sExStackTrace = string.Empty;

                        string version = "";
                        if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                            version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                        string[] server = version.Split('|');
                        string serverno = "";
                        if (server.Length > 1)
                            serverno = server[1].Trim();

                        if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                            sMsg = xmlexcep.InnerException.Message;
                        else
                            sMsg = xmlexcep.Message;

                        if (xmlexcep != null && xmlexcep.StackTrace != null)
                            sExStackTrace = xmlexcep.StackTrace;

                        string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        string ConnectionData;
                        ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        using (MySqlConnection con = new MySqlConnection(ConnectionData))
                        {
                            using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                            {
                                cmd.Connection = con;
                                try
                                {
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                catch
                                {
                                }
                            }
                        }
                        goto trytosaveagain;
                    }
                }
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                trans.Rollback();
                // MySession.Close();
                throw new Exception(ex.Message);
            }
            catch (Exception e)
            {
                trans.Rollback();
                //MySession.Close();
                throw new Exception(e.Message);
            }

            finally
            {
                MySession.Close();
            }


        }

    }
}
