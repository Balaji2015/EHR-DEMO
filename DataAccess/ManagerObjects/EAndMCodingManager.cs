using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;
using System.Xml.XPath;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Threading;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IEAndMCodingManager : IManagerBase<EAndMCoding, ulong>
    {
        //FillEandMCoding LoadEandMCoding(ulong ulEncID, ulong ulHumanID, ulong ulPhyID, string ProcedureType, DateTime date_of_service, bool bFlag, string sPOS);
        IList<EAndMCoding> GetEMCodeList(ulong ulEncID);
        int GetEMCount(ulong ulEncID);
        Boolean IsPrimaryFilled(ulong ulEncID, string UserRole);
        IList<PhysicianProcedure> GetPhysicianProcedure(ulong Physician_Id, string Procedure_Type, ulong Lab_Id, string sLegalOrg);
        int SaveUpdateEandMCoding(IList<EAndMCoding> eandm, IList<EAndMCoding> eandmICD, string MACAddress);
        int SaveUpdateEandMCodingICD(IList<EandMCodingICD> SaveList, IList<EandMCodingICD> UpdateList, string MACAddress);
        void SaveBillingInstruction(IList<PatientNotes> MsgDetails, IList<Encounter> EncountList, string MACAddress);
        IList<PatientNotes> GetBillingInstructions(ulong EncounterID);
        void UpdateBillingInstructions(IList<PatientNotes> MsgDet, string MACAddress);
        //IList<ModifierLibrary> GetModifierForValue(string Modfier_Code);
        IList<EAndMCoding> GetEandMcodByEandMcodingId(ulong EandMCodingId);
        void DeleteEandMcodAndEandMcodICD(IList<EAndMCoding> DeletEandMcod, IList<EandMCodingICD> DeleteEandMcodICD, string MACAddress);
        FillEandMCoding LoadEandMCodingForDuplicateCheck(ulong ulEncID);
        //IList<ModifierLibrary> GetShortDescForModValue(string Modfier_Code);
        IList<AllICD> GetShortDescFromAllIcd(string All_ICD);
        IList<InsurancePlan> GetInsuPlanNameAndExternalPlanNum(string InsurPlanId);
        FillEandMCoding LoadEandMCodingNew(ulong ulEncID, ulong ulHumanID, DateTime date_of_service, out bool bSaveEnable, bool Is_CMG_Ancillary);
        FillEandMCoding SaveUpdateEandMCoding(IList<EAndMCoding> SaveListCPT, IList<EAndMCoding> UpdateListCPT, IList<EandMCodingICD> eandmICDList, string UserName, DateTime DateAndTime, IList<EandMCodingICD> UpdateICDList, Encounter EnRecord, string sBillingInstruction, IList<Immunization> ImmDelList, IList<ImmunizationHistory> ImmHisDelList, CarePlan CareplanUpdate, IList<EAndMCoding> lstEandDeleteList);
        IList<EAndMCoding> GetDetailsByEncounterID(ulong encid);
        FillEandMCoding SaveUpdateEandMCodingForPhoneEncounter(IList<EAndMCoding> SaveListCPT, IList<EAndMCoding> UpdateListCPT, IList<EandMCodingICD> eandmICDList, string UserName, DateTime DateAndTime, IList<EandMCodingICD> UpdateICDList);
    }
    public partial class EAndMCodingManager : ManagerBase<EAndMCoding, ulong>, IEAndMCodingManager
    {

        #region Constructors

        public EAndMCodingManager()
            : base()
        {

        }
        public EAndMCodingManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region PublicMethods

        //public FillEandMCoding LoadEandMCoding(ulong ulEncID, ulong ulHumanID, ulong ulPhyID, string ProcedureType)
        //{
        //    FillEandMCoding eandmCode = new FillEandMCoding();
        //    IList<ProblemList> probList;

        //    //ICriteria criteria1 = session.GetISession().CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Physician_ID", ulPhyID)).Add(Expression.Eq("Procedure_Type", ProcedureType));
        //    //eandmCode.PhyProcList = criteria1.List<PhysicianProcedure>();

        //    //ICriteria criteria2 = session.GetISession().CreateCriteria(typeof(Orders)).Add(Expression.Eq("Encounter_ID", ulEncID)).Add(Expression.Eq("Is_In_House", "Y"));
        //    //eandmCode.OrdersList = criteria2.List<Orders>();

        //    ICriteria criteria10 = session.GetISession().CreateCriteria(typeof(Immunization)).Add(Expression.Eq("Encounter_Id", ulEncID)).Add(Expression.Eq("Vaccine_In_House", "Y"));
        //    eandmCode.ImmunizationList = criteria10.List<Immunization>();

        //    ICriteria criteria9 = session.GetISession().CreateCriteria(typeof(InHouseProcedure)).Add(Expression.Eq("Encounter_ID", ulEncID));
        //    eandmCode.InHouseProcList = criteria9.List<InHouseProcedure>();

        //    ICriteria criteria3 = session.GetISession().CreateCriteria(typeof(ProblemList)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Status", "Active")).Add(Expression.Eq("Is_Active", "Y"));
        //    probList = criteria3.List<ProblemList>();

        //    ISQLQuery sql = session.GetISession().CreateSQLQuery("Select a.* from assessment a where a.Encounter_ID = '" + ulEncID.ToString() + "' and a.Assessment_Type ='Selected' and a.Assessment_Status <> 'Suspected'").AddEntity("a", typeof(Assessment));
        //    eandmCode.AssessmentList = sql.List<Assessment>();

        //    //ICriteria criteria4 = session.GetISession().CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", ulEncID)).Add(Expression.Eq("Assessment_Type","Selected")).Add(Expression.NotEqProperty("Status","Suspected"));
        //    //eandmCode.AssessmentList = criteria4.List<Assessment>();

        //    var DistProblem = from d in probList group d by d.ICD_Code;
        //    eandmCode.ProblemList = new List<ProblemList>();
        //    foreach (var r in DistProblem)
        //    {
        //        eandmCode.ProblemList.Add(r.ToList<ProblemList>()[0]);
        //    }

        //    ICriteria criteria5 = session.GetISession().CreateCriteria(typeof(EAndMCoding)).Add(Expression.Eq("Encounter_ID", ulEncID)).AddOrder(Order.Desc("Created_Date_And_Time"));
        //    eandmCode.EandMCodingList = criteria5.List<EAndMCoding>();

        //    ICriteria criteria6 = session.GetISession().CreateCriteria(typeof(EandMCodingICD)).Add(Expression.Eq("Encounter_ID", ulEncID));
        //    eandmCode.EandMCodingICDList = criteria6.List<EandMCodingICD>();

        //    ICriteria criteria7 = session.GetISession().CreateCriteria(typeof(OrdersAssessment)).Add(Expression.Eq("Encounter_ID", ulEncID));
        //    eandmCode.OrdersAssessmentList = criteria7.List<OrdersAssessment>();

        //    ICriteria criteria8 = session.GetISession().CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Insurance_Type", "PRIMARY"));
        //    eandmCode.PatInsList = criteria8.List<PatientInsuredPlan>();

        //    InsurancePlanManager insMngr = new InsurancePlanManager();
        //    if (eandmCode.PatInsList.Count > 0)
        //    {
        //        eandmCode.PatInsPlanListDetail = insMngr.GetInsurancebyID(eandmCode.PatInsList[0].Insurance_Plan_ID);
        //    }

        //    EncounterManager encMngr = new EncounterManager();
        //    eandmCode.EncRecord = encMngr.GetEncounterByEncounterID(ulEncID)[0];
        //    IList<InsurancePlan> tempList = insMngr.GetInsurancebyID(Convert.ToUInt64(eandmCode.EncRecord.Insurance_Plan_ID));
        //    if (tempList.Count > 0)
        //    {
        //        eandmCode.EncInsPlanName = tempList[0].Ins_Plan_Name;
        //    }

        //    //ICriteria criteria8 = session.GetISession().CreateCriteria(typeof(OrdersProblemList)).Add(Expression.Eq("Encounter_ID", ulEncID));
        //    //eandmCode.OrderProblemList = criteria8.List<OrdersProblemList>();

        //    return eandmCode;
        //}
        const string CMGStaticLookupValue = "CMG Anc.-In House";
        //public FillEandMCoding LoadEandMCoding(ulong ulEncID, ulong ulHumanID, ulong ulPhyID, string ProcedureType, DateTime date_of_service, bool bFlag, string sPOS)
        //{
        //    FillEandMCoding eandmCode = new FillEandMCoding();
        //    IList<EAndMCoding> EandMCodeList = new List<EAndMCoding>();
        //    IList<EandMCodingICD> EandMCodingICDList = new List<EandMCodingICD>();
        //    IList<OrdersAssessment> OrdersAssessmentList = new List<OrdersAssessment>();
        //    IList<Immunization> ImmunizationList = new List<Immunization>();
        //    IList<InHouseProcedure> InHouseProcedureList = new List<InHouseProcedure>();
        //    IList<Orders> OrdersProcedureList = new List<Orders>();
        //    IList<ProblemList> probList = new List<ProblemList>();
        //    IList<PatientInsuredPlan> PatientInsuredPlanList = new List<PatientInsuredPlan>();
        //    IList<Encounter> encList = new List<Encounter>();
        //    //IList<FacilityLibrary> FacList = new List<FacilityLibrary>();
        //    //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        //Added by bala
        //        //IList<string> DOSList = new List<string>();
        //        string sDOSYear = string.Empty;
        //        IList<ProcedureCodeRuleMaster> ProcedureList = new List<ProcedureCodeRuleMaster>();
        //        int iResultCount = 0;
        //        StaticLookupManager objstaticlookupManager = new StaticLookupManager();
        //        //string CMGStaticLookupValue = objstaticlookupManager.getStaticLookupByFieldName("IN-HOUSE PROCEDURES LAB NAME").Select(a => a.Value).SingleOrDefault();
        //        IQuery query1 = iMySession.GetNamedQuery("Fill.IN-HOUSE PROCEDURES.List");
        //        query1.SetParameter(0, ulEncID);
        //        query1.SetParameter(1, CMGStaticLookupValue);
        //        ArrayList arrList = new ArrayList(query1.List());
        //        for (int i = 0; i < arrList.Count; i++)
        //        {
        //            object[] ccObject = (object[])arrList[i];
        //            if (ccObject[0] != null)
        //                eandmCode.OrderProcedureID.Add(Convert.ToUInt64(ccObject[0].ToString()));
        //            if (ccObject[1] != null)
        //                eandmCode.OrderProcedure.Add(ccObject[1].ToString());
        //            if (ccObject[2] != null)
        //                eandmCode.OrderProcedureDesc.Add(ccObject[2].ToString());
        //        }
        //        //ICriteria criteria2 = session.GetISession().CreateCriteria(typeof(Orders)).Add(Expression.Eq("Encounter_ID", ulEncID)).Add(Expression.Eq("Order_Code_Type", CMGStaticLookupValue));
        //        //OrdersProcedureList = criteria2.List<Orders>();
        //        //ISQLQuery sql1 = session.GetISession().CreateSQLQuery("SELECT a.* FROM orders_assessment a , orders b where a.order_id=b.order_id and a.encounter_id='" + ulEncID.ToString() + "' and b.Is_In_House='Y' group by icd").AddEntity("a", typeof(OrdersAssessment));
        //        //OrdersAssessmentList = sql1.List<OrdersAssessment>();

        //        //for (int i = 0; i < OrdersAssessmentList.Count; i++)
        //        //{
        //        //    eandmCode.OrdersAssessmentOrderID.Add(OrdersAssessmentList[i].Id);
        //        //    eandmCode.OrdersAssessmentICD.Add(OrdersAssessmentList[i].ICD);
        //        //    eandmCode.OrdersAssessmentICDDesc.Add(OrdersAssessmentList[i].ICD_Description);
        //        //}

        //        //ICriteria criteria11 = session.GetISession().CreateCriteria(typeof(EandMCodingICD)).Add(Expression.Eq("Encounter_ID", ulEncID));
        //        //eandmCode.EandMCodingICDList = criteria11.List<EandMCodingICD>();


        //        ICriteria criteria10 = iMySession.CreateCriteria(typeof(Immunization)).Add(Expression.Eq("Encounter_Id", ulEncID)).Add(Expression.Eq("Vaccine_In_House", "Y"));
        //        ImmunizationList = criteria10.List<Immunization>();
        //        for (int i = 0; i < ImmunizationList.Count; i++)
        //        {
        //            eandmCode.ImmunProcedureID.Add(ImmunizationList[i].Id);
        //            eandmCode.ImmunProcedure.Add(ImmunizationList[i].Procedure_Code);
        //            eandmCode.ImmunProcedureDesc.Add(ImmunizationList[i].Immunization_Description);
        //        }

        //        ICriteria criteria9 = iMySession.CreateCriteria(typeof(InHouseProcedure)).Add(Expression.Eq("Encounter_ID", ulEncID));
        //        InHouseProcedureList = criteria9.List<InHouseProcedure>();
        //        for (int i = 0; i < InHouseProcedureList.Count; i++)
        //        {
        //            eandmCode.InhouseProcedureID.Add(InHouseProcedureList[i].Id);
        //            eandmCode.InhouseProcedure.Add(InHouseProcedureList[i].Procedure_Code);
        //            eandmCode.InhouseProcedureDesc.Add(InHouseProcedureList[i].Procedure_Code_Description);
        //        }

        //        ICriteria criteria3 = iMySession.CreateCriteria(typeof(ProblemList)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Status", "Active")).Add(Expression.Eq("Is_Active", "Y"));
        //        probList = criteria3.List<ProblemList>();

        //        ISQLQuery sql = iMySession.CreateSQLQuery("Select a.* from assessment a where a.Encounter_ID = '" + ulEncID.ToString() + "' and a.Assessment_Type ='Selected' and a.Assessment_Status <> 'Suspected'").AddEntity("a", typeof(Assessment));
        //        eandmCode.AssessmentList = sql.List<Assessment>();

        //        var DistProblem = from d in probList group d by d.ICD;
        //        foreach (var r in DistProblem)
        //        {
        //            eandmCode.ProblemList.Add(r.ToList<ProblemList>()[0]);
        //        }

        //        ICriteria criteria5 = iMySession.CreateCriteria(typeof(EAndMCoding)).Add(Expression.Eq("Encounter_ID", ulEncID)).Add(Restrictions.Or(Restrictions.Eq("Is_Delete", "N"), Restrictions.Eq("Is_Delete", ""))).AddOrder(Order.Desc("Created_Date_And_Time"));
        //        eandmCode.EandMCodingList = criteria5.List<EAndMCoding>();

        //        ICriteria criteria6 = iMySession.CreateCriteria(typeof(EandMCodingICD)).Add(Expression.Eq("Encounter_ID", ulEncID)).Add(Restrictions.Or(Restrictions.Eq("Is_Delete", "N"), Restrictions.Eq("Is_Delete", "")));
        //        eandmCode.EandMCodingICDList = criteria6.List<EandMCodingICD>();
        //        //ICriteria criteria8 = iMySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Insurance_Type", "PRIMARY"));
        //        //PatientInsuredPlanList = criteria8.List<PatientInsuredPlan>();
        //        //IQuery HQLquery = iMySession.GetNamedQuery("Get.PatInsuredAndPlan.EandMCoding");
        //        //HQLquery.SetParameter(0, ulHumanID);
        //        //IList ilstObj = HQLquery.List();
        //        //if (ilstObj.Count > 0)
        //        //{
        //        //    object[] Obj = (object[])ilstObj[0];
        //        //    eandmCode.PatInsuredInsPlanID = Convert.ToUInt32(Obj[0].ToString());
        //        //    eandmCode.PatInsuredPolicyHolderID = Obj[1].ToString();
        //        //    eandmCode.PatInsuredHumanID = Convert.ToUInt32(Obj[2].ToString());
        //        //    eandmCode.PatInsuredInsPlanName = Obj[3].ToString();
        //        //    eandmCode.PatInsuredExtPlanNumber = Obj[4].ToString();
        //        //}

        //        //EncounterManager encMngr = new EncounterManager();

        //        //encList = encMngr.GetEncounterByEncounterID(ulEncID);

        //        //for (int i = 0; i < encList.Count; i++)
        //        //{

        //        //    eandmCode.EncPlaceofService = encList[i].Place_Of_Service;
        //        //    //Added by bala
        //        //    DOSList.Add(Convert.ToString(encList[i].Date_of_Service));
        //        //    //string[] sTxt = DOSList[i].Split('/');
        //        //    sDOSYear = Convert.ToString(encList[i].Date_of_Service.Year); //sTxt[2].Substring(0,4);
        //        //}
        //        //if (bFlag == false)
        //        //{
        //        //    EncounterManager encMngr = new EncounterManager();
        //        //    encList = encMngr.GetEncounterByEncounterID(ulEncID);
        //        //    for (int i = 0; i < encList.Count; i++)
        //        //    {
        //        //        eandmCode.EncPlaceofService = encList[i].Place_Of_Service;
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    eandmCode.EncPlaceofService = sPOS;
        //        //}

        //        sDOSYear = Convert.ToString(date_of_service.Year);
        //        //FacilityManager FacMngr = new FacilityManager();
        //        //FacList = FacMngr.GetFacilityListByFacilityName(encList[0].Facility_Name);
        //        //if (FacList.Count > 0)
        //        //{
        //        //    eandmCode.EncPlaceofService = FacList[0].POS;
        //        //}

        //        //Added by bala

        //        // var list1= from a in dt.Rows where a.Domain_Object.contains("vitals") select a;
        //        //int iPrevGroupNumber = 0;
        //        bool bPrevConditionSatisfied = false;
        //        string sPrevRuleName = string.Empty;
        //        string sSubRuleName = string.Empty;

        //        for (int i = 0; i < ProcedureList.Count; i++)
        //        {
        //            if (bPrevConditionSatisfied == true)
        //            {
        //                if (sPrevRuleName == ProcedureList[i].Rule_Name)
        //                {
        //                    //if (ProcedureList[i].Is_Mandatory == "N")
        //                    //{
        //                    //    continue;
        //                    //}

        //                    if (sSubRuleName != ProcedureList[i].Sub_Rule_Name)
        //                    {
        //                        continue;
        //                    }
        //                }
        //            }

        //            //if (bPrevConditionSatisfied == true)
        //            //{
        //            //    if (iPrevGroupNumber == ProcedureList[i].Group_Number)
        //            //    {
        //            //        continue;
        //            //    }
        //            //}

        //            //bPrevConditionSatisfied = false;
        //            if (ProcedureList[i].WhereCriteria == string.Empty && ProcedureList[i].DOS_Year != DateTime.Now.Year.ToString())
        //            {
        //                eandmCode.ProcedureMasterList.Add(ProcedureList[i].Procedure_Code + "-" + ProcedureList[i].Procedure_Code_Description);
        //            }
        //            else if (ProcedureList[i].WhereCriteria != string.Empty)
        //            {
        //                string sWherecriteria = ProcedureList[i].WhereCriteria.Replace(":EncID", ulEncID.ToString()).Replace(":HumanID", ulHumanID.ToString());
        //                ISQLQuery procsql1 = iMySession.CreateSQLQuery(sWherecriteria);
        //                ArrayList arrayList = new ArrayList(procsql1.List());
        //                object ires = (object)arrayList[0];
        //                iResultCount = Convert.ToInt32(ires);
        //                if (ProcedureList[i].Is_Result_Expected == "Y" && iResultCount >= 1)
        //                {
        //                    eandmCode.ProcedureMasterList.Add(ProcedureList[i].Procedure_Code + "-" + ProcedureList[i].Procedure_Code_Description);
        //                    bPrevConditionSatisfied = true;
        //                    sSubRuleName = ProcedureList[i].Sub_Rule_Name;

        //                }
        //                else if (ProcedureList[i].Is_Result_Expected == "N" && iResultCount == 0)
        //                {
        //                    eandmCode.ProcedureMasterList.Add(ProcedureList[i].Procedure_Code + "-" + ProcedureList[i].Procedure_Code_Description);
        //                    bPrevConditionSatisfied = true;
        //                    sSubRuleName = ProcedureList[i].Sub_Rule_Name;
        //                }
        //            }
        //            //iPrevGroupNumber = ProcedureList[i].Group_Number;
        //            sPrevRuleName = ProcedureList[i].Rule_Name;

        //        }
        //        //InsurancePlanManager insMngr = new InsurancePlanManager();
        //        //IList<InsurancePlan> tempList = insMngr.GetInsurancebyID(Convert.ToUInt64(eandmCode.PatInsuredInsPlanID));
        //        //if (tempList.Count > 0)
        //        //{
        //        //    eandmCode.PatInsuredInsPlanName = tempList[0].Ins_Plan_Name;
        //        //    eandmCode.PatInsuredExtPlanNumber = tempList[0].External_Plan_Number;
        //        //}
        //        iMySession.Close();
        //    }
        //    return eandmCode;
        //}

        int iTryCount = 0; //kumar


        public IList<EAndMCoding> GetEMCodeList(ulong ulEncID)
        {
            IList<EAndMCoding> eandmList = new List<EAndMCoding>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ICriteria crit = iMySession.CreateCriteria(typeof(EAndMCoding)).Add(Expression.Eq("Encounter_ID", ulEncID)).Add(Expression.Not(Expression.Eq("Is_Delete", "Y")));
                eandmList = crit.List<EAndMCoding>();
                iMySession.Close();
            }
            return eandmList;
        }

        public int GetEMCount(ulong ulEncID)
        {
            IList<EAndMCoding> eandmList = new List<EAndMCoding>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(EAndMCoding)).Add(Expression.Eq("Encounter_ID", ulEncID)).Add(Expression.Not(Expression.Eq("Is_Delete", "Y")));
                eandmList = crit.List<EAndMCoding>();
                iMySession.Close();
            }
            return eandmList.Count;
        }

        public Boolean IsPrimaryFilled(ulong ulEncID, string UserRole)
        {
            IList<EandMCodingICD> eandmList = new List<EandMCodingICD>();
            IList<EandMCodingICD> eandmPrimaryList = new List<EandMCodingICD>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            // Added by Manimozhi on 30th May 2012 - Bug id 9842
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select a.* from e_m_coding_icd a where a.Encounter_ID = '" + ulEncID.ToString() + "' and a.Is_Delete<>'Y'").AddEntity("a", typeof(EandMCodingICD));
                eandmList = sql.List<EandMCodingICD>();


                ISQLQuery sql1 = iMySession.CreateSQLQuery("Select a.* from e_m_coding_icd a where a.Encounter_ID = '" + ulEncID.ToString() + "' and ICD_Category = 'Primary' and a.Is_Delete<>'Y'").AddEntity("a", typeof(EandMCodingICD));
                eandmPrimaryList = sql1.List<EandMCodingICD>();


                IList<EAndMCoding> eandmCodingList = new List<EAndMCoding>();

                ISQLQuery sql2 = iMySession.CreateSQLQuery("Select a.* from e_m_coding a where a.Encounter_ID = '" + ulEncID.ToString() + "' and a.Is_Delete<>'Y'").AddEntity("a", typeof(EAndMCoding));
                eandmCodingList = sql2.List<EAndMCoding>();

                iMySession.Close();
                //if (UserRole.ToUpper() == "CODER")
                //{
                //    if ((eandmCodingList.Count > 0 && eandmList.Count == 0) || (eandmList.Count != eandmPrimaryList.Count))
                //    {
                //        return false;
                //    }

                //}
            }
            //if (eandmList.Count == eandmPrimaryList.Count && eandmList.Count > 0)
            if (eandmPrimaryList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


            // Commented by Manimozhi on  30th May 2012 - Bug id 9842

            //ICriteria crit = session.GetISession().CreateCriteria(typeof(EandMCodingICD)).Add(Expression.Eq("Encounter_ID", ulEncID)).Add(Expression.Eq("ICD_Category", "Primary"));
            //eandmList = crit.List<EandMCodingICD>();


        }

        public IList<PhysicianProcedure> GetPhysicianProcedure(ulong Physician_Id, string Procedure_Type, ulong Lab_Id, string sLegalOrg)
        {
            IPhysicianProcedureManager PhysicianProcedureMgr = new ManagerFactory().GetPhysicianProcedureManager();
            IList<PhysicianProcedure> temp = PhysicianProcedureMgr.GetProceduresUsingPhysicianIDAndLabID(Physician_Id, Procedure_Type, Lab_Id, sLegalOrg);
            if (temp != null && temp.Count > 0)
            {
                if (Procedure_Type != "OTHER PROCEDURE")
                    return (temp).Any(a => a.Sort_Order == 0) ? (temp).Where(a => a.Sort_Order != 0).ToList().Concat(temp.Where(a => a.Sort_Order == 0).OrderBy(b => b.Procedure_Description)).ToList() : temp;
            }
            return temp;

        }
        public int SaveUpdateEandMCoding(IList<EAndMCoding> SaveList, IList<EAndMCoding> UpdateList, string MACAddress)
        {
            iTryCount = 0;
        TryAgain1:
            int iResult = 0;
            ISession MySession = Session.GetISession();
            // ITransaction trans = null;
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {

                        if (SaveList.Count > 0)
                        {
                            //   iResult = SaveUpdateDeleteWithoutTransaction(ref SaveList, null, null, MySession, MACAddress);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain1;
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
                        if (UpdateList.Count > 0)
                        {
                            //EandMCodingICDManager ICDMngr = new EandMCodingICDManager();
                            ////iResult = ICDMngr.SaveUpdateEandMICD(eandmICD, MACAddress, MySession);
                            SaveList = null;
                            //iResult = SaveUpdateDeleteWithoutTransaction(ref SaveList, UpdateList, null, MySession, MACAddress);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain1;
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
                        MySession.Flush();
                        trans.Commit();
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
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
            return iResult;
        }

        public void SaveBillingInstruction(IList<PatientNotes> MsgDetails, IList<Encounter> EncountList, string MACAddress)
        {
            //PatientNotesManager PatNotMgr = new PatientNotesManager();
            //ISession MySession = Session.GetISession();
            //PatNotMgr.SaveUpdateDeleteWithTransaction(ref MsgDetails, null, null, MACAddress);


            //commented by srividhya - batch list is saved twice - so i will comment the below line
            //SaveUpdateDeleteWithTransaction(ref DummyList, BatchList, null, MacAddress);
            //ScanManager objScanMngr = new ScanManager();
            //IList<Encounter> SaveList = null;
            EncounterManager objEncountMngr = new EncounterManager();
            PatientNotesManager PatNtMngr = new PatientNotesManager();
            iTryCount = 0;
        TryAgain:
            int iResult = 0;

            Boolean bResult = false;

            ISession MySession = Session.GetISession();
            // ITransaction trans = null;
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {





                        if (MsgDetails != null && MsgDetails.Count > 0)
                        {
                            // iResult = PatNtMngr.SaveUpdateDeleteWithoutTransaction(ref MsgDetails, null, null, MySession, "MACAddress");
                        }
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
                                throw new Exception("Deadlock is occured. Transaction failed");

                            }
                        }
                        else if (iResult == 1)
                        {

                            trans.Rollback();
                            // MySession.Close();
                            throw new Exception("Exception is occured. Transaction failed");

                        }
                        if (MsgDetails != null)
                        {
                            if (EncountList.Count > 0)
                            {
                                EncountList[0].Message_ID = (MsgDetails[0].Id);
                            }

                        }
                        //Commented on 23-03-2016 as it is not in use

                        // iResult = objEncountMngr.SaveUpdateDeleteWithoutTransaction(ref SaveList, EncountList, null, MySession, MACAddress);


                        //objWfMgr.UpdateOwner(objWfObject.Obj_System_Id, objWfObject.Obj_Type, Username);

                        //MySession.Flush();
                        trans.Commit();
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        throw new Exception(ex.Message);
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
        public int SaveUpdateEandMCodingICD(IList<EandMCodingICD> SaveList, IList<EandMCodingICD> UpdateList, string MACAddress)
        {
            EandMCodingICDManager ICDMngr = new EandMCodingICDManager();
            iTryCount = 0;
        TryAgain1:
            int iResult = 0;
            ISession MySession = Session.GetISession();
            // ITransaction trans = null;
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {

                        if (SaveList.Count > 0)
                        {
                            //EandMCodingICDManager ICDMngr = new EandMCodingICDManager();
                            //iResult = ICDMngr.SaveUpdateDeleteWithoutTransaction(ref SaveList, null, null, MySession, MACAddress);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain1;
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
                        if (UpdateList.Count > 0)
                        {

                            // iResult = ICDMngr.SaveUpdateEandMICD(eandmICD, MACAddress, MySession);
                            // iResult = ICDMngr.SaveUpdateDeleteWithoutTransaction(ref eandmICD, null, null, MySession, MACAddress);
                            SaveList = null;
                            //iResult = ICDMngr.SaveUpdateDeleteWithoutTransaction(ref SaveList, UpdateList, null, MySession, MACAddress);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain1;
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
                        MySession.Flush();
                        trans.Commit();
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
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
            return iResult;
        }

        //public IList<StaticLookup> GetModifierForValue(string Field_Name, string Value)
        //{
        //    IList<StaticLookup> ilistModifier = new List<StaticLookup>();
        //    StaticLookup objstat = new StaticLookup();
        //    IQuery query = session.GetISession().GetNamedQuery("GetModifierDescription");
        //    //query.SetString(0, Field_Name);
        //    query.SetParameter(0, Value);
        //    ArrayList aryModifier = new ArrayList();
        //    aryModifier = new ArrayList(query.List());
        //    if (aryModifier.Count != 0)
        //    {
        //        //object[] obj = (object[])aryModifier[0];
        //        //objstat.Field_Name = obj[0].ToString();
        //        objstat.Description = aryModifier[0].ToString();
        //    }
        //    ilistModifier.Add(objstat);
        //    return ilistModifier;
        //}

        //public IList<ModifierLibrary> GetModifierForValue(string Modfier_Code)
        //{
        //    IList<ModifierLibrary> ilistModifier = new List<ModifierLibrary>();
        //    ModifierLibrary objstat = new ModifierLibrary();
        //    //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query = iMySession.GetNamedQuery("GetModifierDescriptionFrmModLib");
        //        //query.SetString(0, Field_Name);
        //        query.SetParameter(0, Modfier_Code);
        //        ArrayList aryModifier = new ArrayList();
        //        aryModifier = new ArrayList(query.List());
        //        if (aryModifier.Count != 0)
        //        {
        //            //object[] obj = (object[])aryModifier[0];
        //            //objstat.Field_Name = obj[0].ToString();
        //            objstat.Modifier_Code_Description = aryModifier[0].ToString();
        //        }
        //        ilistModifier.Add(objstat);
        //        iMySession.Close();
        //    }
        //    return ilistModifier;
        //}

        public IList<PatientNotes> GetBillingInstructions(ulong EncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<PatientNotes> PatNotesList = new List<PatientNotes>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("SELECT b.* FROM encounter a inner join patient_notes b on a.Message_ID=b.Message_ID WHERE a.Encounter_ID=" + EncounterID + "")
                  .AddEntity("a", typeof(Encounter)).AddEntity("b", typeof(PatientNotes));
                //IList<Encounter> EncuntList = new List<Encounter>();
                PatientNotes objPatNot = new PatientNotes();
                //Encounter objencnt = new Encounter();
                if (sql.List().Count > 0)
                {
                    foreach (IList<Object> l in sql.List())
                    {
                        PatientNotes Patrecrd = (PatientNotes)l[1];
                        PatNotesList.Add(Patrecrd);

                    }
                }
                iMySession.Close();
            }
            return PatNotesList;
        }
        public void UpdateBillingInstructions(IList<PatientNotes> MsgDet, string MACAddress)
        {
            // IList<PatientNotes> dummylist = null;
            //EAndMCodingManager objEndMngr = new EAndMCodingManager();
            PatientNotesManager objPatMngr = new PatientNotesManager();


            //objPatMngr.SaveUpdateDeleteWithTransaction(ref dummylist, MsgDet, null, MACAddress);

        }

        public void SaveElectronicSuperBill(IList<Encounter> updatelist, string MACAddress)
        {
            // IList<Encounter> dummylist = null;
            EncounterManager entmngr = new EncounterManager();
            //entmngr.SaveUpdateDeleteWithTransaction(ref dummylist, updatelist, null, MACAddress);
        }
        public IList<EAndMCoding> GetEandMcodByEandMcodingId(ulong EandMCodingId)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<EAndMCoding> listEandMCoding = new List<EAndMCoding>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit;
                crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", EandMCodingId));
                listEandMCoding = crit.List<EAndMCoding>();
                iMySession.Close();
            }
            return listEandMCoding;
        }
        public void DeleteEandMcodAndEandMcodICD(IList<EAndMCoding> DeletEandMcod, IList<EandMCodingICD> DeleteEandMcodICD, string MACAddress)
        {
            EAndMCodingManager Eandmcodmgr = new EAndMCodingManager();
            EandMCodingICDManager ICDMngr = new EandMCodingICDManager();
            iTryCount = 0;
        TryAgain1:
            int iResult = 0;
            ISession MySession = Session.GetISession();
            //ITransaction trans = null;
            //IList<EAndMCoding> dummylist = null;
            //IList<EandMCodingICD> dummylist1 = null;
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        //trans = MySession.BeginTransaction();
                        if (DeletEandMcod != null && DeletEandMcod.Count > 0)
                        {
                            //EandMCodingICDManager ICDMngr = new EandMCodingICDManager();
                            //iResult = Eandmcodmgr.SaveUpdateDeleteWithoutTransaction(ref dummylist, DeletEandMcod, null, MySession, MACAddress);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain1;
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
                        //if (DeletEandMcod != null)
                        //{
                        //    if (DeleteEandMcodICD.Count > 0)
                        //    {
                        //        DeleteEandMcodICD[0].Is_Delete = (DeletEandMcod[0].Is_Delete);
                        //    }

                        //}
                        //iResult = ICDMngr.SaveUpdateDeleteWithoutTransaction(ref dummylist1, DeleteEandMcodICD, null, MySession, MACAddress);
                        if (iResult == 2)
                        {
                            if (iTryCount < 5)
                            {
                                iTryCount++;
                                goto TryAgain1;
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

                        //objWfMgr.UpdateOwner(objWfObject.Obj_System_Id, objWfObject.Obj_Type, Username);

                        //MySession.Flush();
                        trans.Commit();
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        MySession.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //MySession.Close();
                throw new Exception(ex.Message);
            }
        }
        public FillEandMCoding LoadEandMCodingForDuplicateCheck(ulong ulEncID)
        {
            FillEandMCoding eandmCode = new FillEandMCoding();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit1 = iMySession.CreateCriteria(typeof(EAndMCoding)).Add(Expression.Eq("Encounter_ID", ulEncID)).Add(Expression.Eq("Is_Delete", "N")).AddOrder(Order.Desc("Procedure_Code"));
                eandmCode.EandMCodingList = crit1.List<EAndMCoding>();

                ICriteria crit2 = iMySession.CreateCriteria(typeof(EandMCodingICD)).Add(Expression.Eq("Encounter_ID", ulEncID)).Add(Expression.Eq("Is_Delete", "N"));
                eandmCode.EandMCodingICDList = crit2.List<EandMCodingICD>();

                iMySession.Close();
            }
            return eandmCode;
        }
        //public IList<ModifierLibrary> GetShortDescForModValue(string Modfier_Code)
        //{
        //    IList<ModifierLibrary> ilistModifier = new List<ModifierLibrary>();
        //    ModifierLibrary objstat = new ModifierLibrary();
        //    //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query = iMySession.GetNamedQuery("GetShortDescFromModifLibry");
        //        //query.SetString(0, Field_Name);
        //        query.SetParameter(0, Modfier_Code);
        //        ArrayList aryModifier = new ArrayList();
        //        aryModifier = new ArrayList(query.List());
        //        if (aryModifier.Count != 0)
        //        {
        //            //object[] obj = (object[])aryModifier[0];
        //            //objstat.Field_Name = obj[0].ToString();
        //            objstat.Short_Description = aryModifier[0].ToString();
        //        }
        //        ilistModifier.Add(objstat);
        //        iMySession.Close();
        //    }
        //    return ilistModifier;
        //}
        public IList<AllICD> GetShortDescFromAllIcd(string All_ICD)
        {
            IList<AllICD> ilistAllIcd = new List<AllICD>();
            AllICD objstat = new AllICD();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("GetShortDescFromAllICD");
                //query.SetString(0, Field_Name);
                query.SetParameter(0, All_ICD);
                ArrayList aryAllIcdDesc = new ArrayList();
                aryAllIcdDesc = new ArrayList(query.List());
                if (aryAllIcdDesc.Count != 0)
                {
                    //object[] obj = (object[])aryModifier[0];
                    //objstat.Field_Name = obj[0].ToString();
                    objstat.Short_Description = aryAllIcdDesc[0].ToString();
                }
                ilistAllIcd.Add(objstat);
                iMySession.Close();
            }
            return ilistAllIcd;
        }
        public IList<InsurancePlan> GetInsuPlanNameAndExternalPlanNum(string InsurPlanId)
        {
            IList<InsurancePlan> listInsurPlan = new List<InsurancePlan>();
            InsurancePlan objInsuPlan = new InsurancePlan();

            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("GetInsuPlanNameAndExtPlanNumb");
                //query.SetString(0, Field_Name);
                query.SetParameter(0, InsurPlanId);
                ArrayList aryInsurPlan = new ArrayList();
                aryInsurPlan = new ArrayList(query.List());
                if (aryInsurPlan.Count != 0)
                {
                    object[] obj = (object[])aryInsurPlan[0];
                    //objstat.Field_Name = obj[0].ToString();
                    objInsuPlan.Ins_Plan_Name = obj[0].ToString();
                    objInsuPlan.External_Plan_Number = obj[1].ToString();
                }
                listInsurPlan.Add(objInsuPlan);
                iMySession.Close();
            }
            return listInsurPlan;
        }

        public class CustomDataAdapter : DbDataAdapter
        {
            public int FillFromReader(DataTable dataTable, IDataReader dataReader)
            {
                return this.Fill(dataTable, dataReader);
            }
        }

        //BugID:48668 -- ServProc REVAMP -- LoadEandMCodingNew Code Changed
        class OrdersAssIcd
        {
            public string ICD { get; set; }
            public string ICD_Description { get; set; }
            public ulong Order_ID { get; set; }
        };
        public FillEandMCoding LoadEandMCodingNew(ulong ulEncID, ulong ulHumanID, DateTime date_of_service, out bool bSaveEnable, bool Is_CMG_Ancillary)
        {
            FillEandMCoding eandmCode = new FillEandMCoding();
            IList<EandMCodingICD> EandMCodingICDList = new List<EandMCodingICD>();
            IList<Immunization> ImmunizationList = new List<Immunization>();
            IList<InHouseProcedure> InHouseProcedureList = new List<InHouseProcedure>();
            //IList<ProblemList> probList = new List<ProblemList>();
            IList<string> ProcList = new List<string>();
            IList<string> ICDList = new List<string>();
            //ArrayList aryAccessCPT = new ArrayList();
            IList<string> TempProcedureList = new List<string>();
            IList<string> TempProcedureListImmunization = new List<string>();
            IList<string> TempProcedureListInhousePro = new List<string>();
            IList<string> TempProcedureListOrdersList = new List<string>();
            IList<string> TempProcedureListOrdersList1 = new List<string>();
            IList<string> ICdlist = new List<string>();
            ArrayList aryAccessICD = new ArrayList();
            // bool IsPrimaryChecked = false;
            string sAssesmentPrimary = string.Empty;
            EncounterManager EM = new EncounterManager();
            bSaveEnable = false;
            IList<EandMCodingICD> EMCodingAssessmentSequence = new List<EandMCodingICD>();


            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                string ICDType = string.Empty;
                if (date_of_service >= DateTime.Parse("01-Oct-2015"))
                {
                    ICDType = "ICD_10";
                }
                else
                {
                    ICDType = "ICD_9";
                }
                IList<string> AssessmentICDs = new List<string>();
                IList<string> EandMAssICDs = new List<string>();
                IList<string> EandMOrdersAssICDs = new List<string>();
                IList<string> OrdersAssessmentICDs = new List<string>();
                IList<string> EandMCPTs = new List<string>();
                List<string> ImmCPT = new List<string>();
                IList<string> ImmDose = new List<string>();
                IList<OrdersAssIcd> OrdersAssICDList = new List<OrdersAssIcd>();
                bool EMPriICDSet = false;
                IList<string> Immcptunit = new List<string>();
                IList<ProcedureModifierLookup> lstprocedure = new List<ProcedureModifierLookup>();
                ProcedureModifierLookupManager obj = new ProcedureModifierLookupManager();
                IList<ProcedureCodeLibrary> lstprocedurelib = new List<ProcedureCodeLibrary>();
                ProcedureCodeLibraryManager objlib = new ProcedureCodeLibraryManager();

                List<string> InProcCPT = new List<string>();
                List<string> OrderCPT = new List<string>();
                List<string> OrderCPT1 = new List<string>();

                //BugID:46231
                IList<string> lstStatus_Assessment = new List<string>();
                if (System.Configuration.ConfigurationSettings.AppSettings["ServProcCodeExclutionStatus"] != null)
                    lstStatus_Assessment = System.Configuration.ConfigurationSettings.AppSettings["ServProcCodeExclutionStatus"].ToString().Split(',');

                #region HumanXML
                XmlDocument xmlHumandoc = new XmlDocument();
                XmlDocument xmlEncounterdoc = new XmlDocument();

                #region EncounterXML
                IList<string> ilstEandMTagList = new List<string>();
                ilstEandMTagList.Add("EandMCodingICDList");
                ilstEandMTagList.Add("AssessmentList");
                ilstEandMTagList.Add("EAndMCodingList");
                ilstEandMTagList.Add("EncounterList");


                eandmCode.EandMCodingICDList = new List<EandMCodingICD>();
                IList<EandMCodingICD> lst = new List<EandMCodingICD>();
                IList<EAndMCoding> lsteandm = new List<EAndMCoding>();
                IList<Assessment> lstass = new List<Assessment>();
                IList<object> ilstEandMBlobFinal = new List<object>();
                eandmCode.EandMCodingList = new List<EAndMCoding>();
                ilstEandMBlobFinal = ReadBlob(ulEncID, ilstEandMTagList);

                if (ilstEandMBlobFinal != null && ilstEandMBlobFinal.Count > 0)
                {
                    if (ilstEandMBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstEandMBlobFinal[0]).Count; iCount++)
                        {
                            lst.Add((EandMCodingICD)((IList<object>)ilstEandMBlobFinal[0])[iCount]);
                        }
                        eandmCode.EandMCodingICDList = (from m in lst where m.Encounter_ID == ulEncID && (m.Is_Delete == "N" || m.Is_Delete == "") select m).ToList<EandMCodingICD>();

                    }
                    if (ilstEandMBlobFinal[3] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstEandMBlobFinal[3]).Count; iCount++)
                        {
                            eandmCode.EncounterList.Add((Encounter)((IList<object>)ilstEandMBlobFinal[3])[iCount]);
                        }
                        // eandmCode.EandMCodingICDList = (from m in lst where m.Encounter_ID == ulEncID && (m.Is_Delete == "N" || m.Is_Delete == "") select m).ToList<EandMCodingICD>();

                    }

                    if (!Is_CMG_Ancillary)//BugID:52857
                    {
                        if (ilstEandMBlobFinal[1] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstEandMBlobFinal[1]).Count; iCount++)
                            {
                                lstass.Add((Assessment)((IList<object>)ilstEandMBlobFinal[1])[iCount]);
                            }
                            for (int i = 0; i < lstass.Count; i++)
                            {
                                if (lstass[i].Encounter_ID == ulEncID && !(lstStatus_Assessment.Contains(lstass[i].Assessment_Status)) && lstass[i].Version_Year == ICDType && lstass[i].Diagnosis_Source != "VITALS|DELETED")
                                {


                                    if (AssessmentICDs.Contains(lstass[i].ICD) == false)
                                    {
                                        AssessmentICDs.Add(lstass[i].ICD.Trim());
                                        eandmCode.AssessmentList.Add(lstass[i]);
                                    }

                                }

                            }

                        }
                    }
                    if (ilstEandMBlobFinal[2] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstEandMBlobFinal[2]).Count; iCount++)
                        {
                            lsteandm.Add((EAndMCoding)((IList<object>)ilstEandMBlobFinal[2])[iCount]);
                        }

                        List<string> lsteandmcpt = new List<string>();
                        for (int i = 0; i < lsteandm.Count; i++)
                        {
                            if (lsteandm[i].Encounter_ID == ulEncID)
                            {
                                if (EandMCPTs.IndexOf(lsteandm[i].Procedure_Code) == -1)
                                    EandMCPTs.Add(lsteandm[i].Procedure_Code);//BugID:49118
                                if (lsteandm[i].Is_Delete == "N" || lsteandm[i].Is_Delete == "")
                                {
                                    lsteandmcpt.Add(lsteandm[i].Procedure_Code);
                                }
                            }
                        }
                        IList<ProcedureModifierLookup> lstcptlib = new List<ProcedureModifierLookup>();
                        ProcedureModifierLookupManager objmanager = new ProcedureModifierLookupManager();

                        lstcptlib = objmanager.GetProcedureList(lsteandmcpt);

                        for (int i = 0; i < lsteandm.Count; i++)
                        {
                            if (lsteandm[i].Encounter_ID == ulEncID)
                            {
                                if (EandMCPTs.IndexOf(lsteandm[i].Procedure_Code) == -1)
                                    EandMCPTs.Add(lsteandm[i].Procedure_Code);//BugID:49118
                                if (lsteandm[i].Is_Delete == "N" || lsteandm[i].Is_Delete == "")
                                {
                                    //if (aryAccessCPT.Contains(EAndMCoding.Procedure_Code) == false)
                                    //{
                                    eandmCode.EandMCodingList.Add(lsteandm[i]);
                                    IList<ProcedureModifierLookup> lsttempCPT = new List<ProcedureModifierLookup>();
                                    lsttempCPT = (from m in lstcptlib where m.Procedure_Code == lsteandm[i].Procedure_Code && m.Modifier == lsteandm[i].Modifier1 select m).ToList<ProcedureModifierLookup>();
                                    if (lsttempCPT.Count > 0)
                                        ProcList.Add(lsteandm[i].Procedure_Code + "~" + lsteandm[i].Procedure_Code_Description + "~" + lsteandm[i].Id + "~" + lsteandm[i].Units + "~" + lsteandm[i].Modifier1 + "~" + lsteandm[i].Modifier2 + "~" + lsteandm[i].Modifier3 + "~" + lsteandm[i].Modifier4 + "~" + string.Empty + "~" + lsteandm[i].Version + "~" + lsteandm[i].Diagnosis_Pointer_1 + "~" + lsteandm[i].Diagnosis_Pointer_2 + "~" + lsteandm[i].Diagnosis_Pointer_3 + "~" + lsteandm[i].Diagnosis_Pointer_4 + "~" + lsteandm[i].Diagnosis_Pointer_5 + "~" + lsteandm[i].Diagnosis_Pointer_6 + "~" + lsttempCPT[0].Sort_Order + "~" + lsttempCPT[0].RVU);

                                    else
                                    {

                                        lsttempCPT = (from m in lstcptlib where m.Procedure_Code == lsteandm[i].Procedure_Code && m.Modifier == String.Empty select m).ToList<ProcedureModifierLookup>();
                                        if (lsttempCPT.Count > 0)
                                            ProcList.Add(lsteandm[i].Procedure_Code + "~" + lsteandm[i].Procedure_Code_Description + "~" + lsteandm[i].Id + "~" + lsteandm[i].Units + "~" + lsteandm[i].Modifier1 + "~" + lsteandm[i].Modifier2 + "~" + lsteandm[i].Modifier3 + "~" + lsteandm[i].Modifier4 + "~" + string.Empty + "~" + lsteandm[i].Version + "~" + lsteandm[i].Diagnosis_Pointer_1 + "~" + lsteandm[i].Diagnosis_Pointer_2 + "~" + lsteandm[i].Diagnosis_Pointer_3 + "~" + lsteandm[i].Diagnosis_Pointer_4 + "~" + lsteandm[i].Diagnosis_Pointer_5 + "~" + lsteandm[i].Diagnosis_Pointer_6 + "~" + lsttempCPT[0].Sort_Order + "~" + lsttempCPT[0].RVU);
                                        else
                                            ProcList.Add(lsteandm[i].Procedure_Code + "~" + lsteandm[i].Procedure_Code_Description + "~" + lsteandm[i].Id + "~" + lsteandm[i].Units + "~" + lsteandm[i].Modifier1 + "~" + lsteandm[i].Modifier2 + "~" + lsteandm[i].Modifier3 + "~" + lsteandm[i].Modifier4 + "~" + string.Empty + "~" + lsteandm[i].Version + "~" + lsteandm[i].Diagnosis_Pointer_1 + "~" + lsteandm[i].Diagnosis_Pointer_2 + "~" + lsteandm[i].Diagnosis_Pointer_3 + "~" + lsteandm[i].Diagnosis_Pointer_4 + "~" + lsteandm[i].Diagnosis_Pointer_5 + "~" + lsteandm[i].Diagnosis_Pointer_6 + "~" + 9 + "~" + 0);

                                    }
                                    //    aryAccessCPT.Add(EAndMCoding.Procedure_Code);
                                    //}
                                }
                            }

                        }

                    }




                }
                GenerateXml objxml = new GenerateXml();

                xmlHumandoc = objxml.ReadBlob("Human", ulHumanID);
                xmlEncounterdoc = objxml.ReadBlob("Encounter", ulEncID);

                string sDOSYear = string.Empty;
                sDOSYear = Convert.ToString(date_of_service.Year);
                XmlNodeList xmlTypeofVisit = xmlEncounterdoc.SelectNodes("/notes/Modules/EncounterList/Encounter[@Visit_Type!='RETINAL EYE SCAN']");
                //if (eandmCode.EandMCodingList.Count == 0 && xmlTypeofVisit.Count > 0)
                if (xmlTypeofVisit.Count > 0)
                {
                    IList<ProcedureCodeRuleMaster> ProcedureList = new List<ProcedureCodeRuleMaster>();
                    ISQLQuery procsql = iMySession.CreateSQLQuery("SELECT p.* FROM procedure_code_rule_master p where  p.DOS_Year='" + sDOSYear + "' or p.DOS_Year=' ' order by p.Sort_Order,p.XML_Type").AddEntity("p", typeof(ProcedureCodeRuleMaster));
                    ProcedureList = procsql.List<ProcedureCodeRuleMaster>();
                    ArrayList arrayList1 = new ArrayList();

                    Hashtable result = new Hashtable();
                    Hashtable hashResultList = new Hashtable();
                    string sTemp1 = string.Empty;
                    string sTemp2 = string.Empty;
                    ArrayList arrayList = new ArrayList();

                    //var HumanBasedProcedureList = from p in ProcedureList where p.XML_Type == "HUMAN" select p;
                    //IList<ProcedureCodeRuleMaster> ilstHumanBasedProcedureList = HumanBasedProcedureList.ToList<ProcedureCodeRuleMaster>();

                    bool bMatchingFailed = false;
                    bool bDoNotAdd = false;
                    XmlNodeList xmlPhysicianList = null;
                    IList<ProcedureModifierLookup> lstProcedurerulemasterCPT = new List<ProcedureModifierLookup>();
                    ProcedureModifierLookupManager objmanager = new ProcedureModifierLookupManager();
                    List<string> lstCPTrulemaster = ProcedureList.Select(a => a.Procedure_Code).ToList<string>();
                    lstProcedurerulemasterCPT = objmanager.GetProcedureList(lstCPTrulemaster);


                    for (int iCount = 0; iCount < ProcedureList.Count; iCount++)
                    {
                        var proc = from p in ProcedureList where p.Procedure_Code == ProcedureList[iCount].Procedure_Code select p;
                        IList<ProcedureCodeRuleMaster> tempList = proc.ToList<ProcedureCodeRuleMaster>();

                        bDoNotAdd = false;

                        if (tempList.Count > 1) //If one CPT has more than 1 condition
                        {
                            if (tempList[tempList.Count - 1].Id == ProcedureList[iCount].Id) //If the current iteration is the last condition for the CPT then, we can suggest the CPT
                                bDoNotAdd = false; //False for last iteration for that CPT, True for other than last iteration for the CPT
                            else
                                bDoNotAdd = true; //False for last iteration for that CPT, True for other than last iteration for the CPT

                            if (tempList[0].Id == ProcedureList[iCount].Id) //If the current iteration is the first condition for the CPT then, we reset the Matching failed variable
                                bMatchingFailed = false;  //False for no restriction, True in case if one of the previous rule is failed for the CPT then, prevent from suggesting the CPT
                        }
                        else
                        {
                            bDoNotAdd = false;
                            bMatchingFailed = false;
                        }

                        if (ProcedureList[iCount].XML_Type == "HUMAN")
                        {
                            xmlPhysicianList = xmlHumandoc.SelectNodes(ProcedureList[iCount].XML_Query.Replace(":EncID", ulEncID.ToString()).Replace(":From12MonthDate", DateTime.Now.AddDays(-365).ToString("yyyy-MM-dd")).Replace(":From24MonthDate", DateTime.Now.AddDays(-730).ToString("yyyyMMdd")).Replace(":ToDate", DateTime.Now.ToString("yyyy-MM-dd")));
                        }
                        else
                        {
                            xmlPhysicianList = xmlEncounterdoc.SelectNodes(ProcedureList[iCount].XML_Query);
                        }

                        if (xmlPhysicianList.Count > 0 && bDoNotAdd == false && bMatchingFailed == false)
                        {
                            IList<ProcedureModifierLookup> lsttempCPT = new List<ProcedureModifierLookup>();
                            lsttempCPT = (from m in lstProcedurerulemasterCPT where m.Procedure_Code == ProcedureList[iCount].Procedure_Code && m.Modifier == String.Empty select m).ToList<ProcedureModifierLookup>();

                            if (lsttempCPT.Count > 0)
                            {

                                eandmCode.ProcedureMasterList.Add(ProcedureList[iCount].Procedure_Code + "~" + ProcedureList[iCount].Procedure_Code_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "" + "~" + lsttempCPT[0].Sort_Order + "~" + lsttempCPT[0].RVU);
                                TempProcedureList.Add(ProcedureList[iCount].Procedure_Code + "~" + ProcedureList[iCount].Procedure_Code_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "" + "~" + lsttempCPT[0].Sort_Order + "~" + lsttempCPT[0].RVU);
                            }
                            else
                            {

                                eandmCode.ProcedureMasterList.Add(ProcedureList[iCount].Procedure_Code + "~" + ProcedureList[iCount].Procedure_Code_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "" + "~" + 9 + "~" + 0);
                                TempProcedureList.Add(ProcedureList[iCount].Procedure_Code + "~" + ProcedureList[iCount].Procedure_Code_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "" + "~" + 9 + "~" + 0);

                            }
                        }

                        if (bDoNotAdd == true)
                        {
                            if (xmlPhysicianList.Count == 0)
                            {
                                bMatchingFailed = true;
                            }
                        }
                    }
                }
                //string FileNames = "Encounter" + "_" + ulEncID + ".xml";
                //string strXmlFilePaths = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileNames);
                //if (File.Exists(strXmlFilePaths) == true)
                //{
                //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePaths);
                //    XmlNodeList xmlTagName = null;
                //    // itemDoc.Load(XmlText);
                //    using (FileStream fs = new FileStream(strXmlFilePaths
                //        , FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        xmlEncounterdoc.Load(fs);

                //        XmlText.Close();

                //if (xmlEncounterdoc.GetElementsByTagName("EandMCodingICDList") != null)
                //{
                //    if (xmlEncounterdoc.GetElementsByTagName("EandMCodingICDList").Count > 0)
                //    {
                //        xmlTagName = xmlEncounterdoc.GetElementsByTagName("EandMCodingICDList")[0].ChildNodes;
                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                string TagName = xmlTagName[j].Name;
                //                XmlSerializer xmlserializer = new XmlSerializer(typeof(EandMCodingICD));
                //                EandMCodingICD EandMCodingICD = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as EandMCodingICD;
                //                IEnumerable<PropertyInfo> propInfo = null;
                //                if (EandMCodingICD != null)
                //                {
                //                    propInfo = from obji in ((EandMCodingICD)EandMCodingICD).GetType().GetProperties() select obji;

                //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                    {
                //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                        {
                //                            foreach (PropertyInfo property in propInfo)
                //                            {
                //                                if (property.Name == nodevalue.Name)
                //                                {
                //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                        property.SetValue(EandMCodingICD, Convert.ToUInt64(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                        property.SetValue(EandMCodingICD, Convert.ToString(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                        property.SetValue(EandMCodingICD, Convert.ToDateTime(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                        property.SetValue(EandMCodingICD, Convert.ToInt32(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                //                                        property.SetValue(EandMCodingICD, Convert.ToDecimal(nodevalue.Value), null);
                //                                    else
                //                                        property.SetValue(EandMCodingICD, nodevalue.Value, null);
                //                                }
                //                            }
                //                        }
                //                    }
                //                    //if (EandMCodingICD.Source.ToUpper() == "ASSESSMENT" || EandMCodingICD.Source.ToUpper() == "ORDERS_ASSESSMENT")
                //                    //    EMCodingAssessmentSequence.Add(EandMCodingICD);
                //                    if (EandMCodingICD.Encounter_ID == ulEncID && (EandMCodingICD.Is_Delete == "N" || EandMCodingICD.Is_Delete == ""))
                //                    {
                //                        eandmCode.EandMCodingICDList.Add(EandMCodingICD);
                //                        //if (EandMCodingICD.Source.ToUpper() == "ASSESSMENT" && !EandMAssICDs.Contains(EandMCodingICD.ICD.Trim()))
                //                        //    EandMAssICDs.Add(EandMCodingICD.ICD.Trim());
                //                        //if (EandMCodingICD.Source.ToUpper() == "ORDERS_ASSESSMENT" && !EandMOrdersAssICDs.Contains(EandMCodingICD.ICD.Trim()))
                //                        //    EandMOrdersAssICDs.Add(EandMCodingICD.ICD.Trim());

                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                //if (!Is_CMG_Ancillary)//BugID:52857
                //{
                //    #region AssessmentList
                //    if (xmlEncounterdoc.GetElementsByTagName("AssessmentList") != null)
                //    {
                //        if (xmlEncounterdoc.GetElementsByTagName("AssessmentList").Count > 0)
                //        {
                //            xmlTagName = xmlEncounterdoc.GetElementsByTagName("AssessmentList")[0].ChildNodes;
                //            eandmCode.AssessmentList = new List<Assessment>();
                //            if (xmlTagName.Count > 0)
                //            {
                //                for (int j = 0; j < xmlTagName.Count; j++)
                //                {
                //                    string TagName = xmlTagName[j].Name;
                //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(Assessment));
                //                    Assessment Assessment = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Assessment;
                //                    IEnumerable<PropertyInfo> propInfo = null;
                //                    if (Assessment != null)
                //                    {
                //                        propInfo = from obji in ((Assessment)Assessment).GetType().GetProperties() select obji;

                //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                        {
                //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                            {
                //                                foreach (PropertyInfo property in propInfo)
                //                                {
                //                                    if (property.Name == nodevalue.Name)
                //                                    {
                //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                            property.SetValue(Assessment, Convert.ToUInt64(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                            property.SetValue(Assessment, Convert.ToString(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                            property.SetValue(Assessment, Convert.ToDateTime(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                            property.SetValue(Assessment, Convert.ToInt32(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                //                                            property.SetValue(Assessment, Convert.ToDecimal(nodevalue.Value), null);
                //                                        else
                //                                            property.SetValue(Assessment, nodevalue.Value, null);
                //                                    }
                //                                }
                //                            }
                //                        }
                //                        if (Assessment.Encounter_ID == ulEncID && !(lstStatus_Assessment.Contains(Assessment.Assessment_Status)) && Assessment.Version_Year == ICDType && Assessment.Diagnosis_Source != "VITALS|DELETED")
                //                        {
                //                            //if (aryAccessICD.Contains(Assessment.ICD) == false)
                //                            //{
                //                            //    eandmCode.AssessmentList.Add(Assessment);
                //                            //    if (IsPrimaryChecked == false)
                //                            //    {
                //                            //        sAssesmentPrimary = Assessment.Primary_Diagnosis;
                //                            //    }
                //                            //    ICDList.Add("ASSESSMENT" + "~" + Assessment.ICD + "~" + Assessment.ICD_Description + "~" + 0 + "~" + sAssesmentPrimary + "~" + "0" + "~" + "6" + "~" + "");
                //                            //    aryAccessICD.Add(Assessment.ICD);
                //                            //}

                //                            if (AssessmentICDs.Contains(Assessment.ICD) == false)
                //                            {
                //                                AssessmentICDs.Add(Assessment.ICD.Trim());
                //                                eandmCode.AssessmentList.Add(Assessment);
                //                            }

                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    #endregion
                //}
                #region EandMCodingICD


                //BugID:48192 
                IList<Assessment> lstAssment = new List<Assessment>();
                IList<EandMCodingICD> lstEmICD = new List<EandMCodingICD>();
                string AssPriICD = string.Empty, EMPriICD = string.Empty;
                if (eandmCode.AssessmentList != null && eandmCode.AssessmentList.Count > 0)
                    lstAssment = eandmCode.AssessmentList.Where(a => a.Primary_Diagnosis.ToUpper() == "Y").ToList<Assessment>();
                if (lstAssment != null && lstAssment.Count > 0)
                {
                    AssPriICD = lstAssment[0].ICD.Trim();
                }
                if (eandmCode.EandMCodingICDList != null && eandmCode.EandMCodingICDList.Count > 0)
                    lstEmICD = eandmCode.EandMCodingICDList.Where(a => a.Is_Delete.ToUpper() == "N" && a.ICD_Category.ToUpper() == "PRIMARY").ToList<EandMCodingICD>();
                if (lstEmICD != null && lstEmICD.Count > 0)
                {
                    EMPriICD = lstEmICD[0].ICD.Trim();
                }

                if (AssPriICD.Trim().Equals(EMPriICD.Trim()))
                    EMPriICDSet = true;

                if (eandmCode.EandMCodingICDList != null)
                {
                    eandmCode.EandMCodingICDList = eandmCode.EandMCodingICDList.OrderBy(a => a.Sequence).ToList();
                    for (int j = 0; j < eandmCode.EandMCodingICDList.Count; j++)
                    {
                        string sPrimary = string.Empty;

                        var eandMICDList = from eandmICD in eandmCode.EandMCodingICDList where eandmICD.ICD.Trim() == eandmCode.EandMCodingICDList[j].ICD.Trim() select eandmICD;
                        IList<EandMCodingICD> templist = eandMICDList.ToList<EandMCodingICD>();
                        string sICDID = eandmCode.EandMCodingICDList[j].Id.ToString();
                        string sICDVersion = eandmCode.EandMCodingICDList[j].Version.ToString();
                        string Source = string.Empty;
                        if (eandmCode.EandMCodingICDList[j].Source.ToUpper() == "EMICD")
                        {
                            Source = "EMICD";
                            if (eandmCode.EandMCodingICDList[j].ICD_Category.ToUpper() == "PRIMARY")
                                sPrimary = ((AssPriICD == string.Empty) ? "Pri" : "");
                        }
                        //else if (eandmCode.EandMCodingICDList[j].Source.ToUpper() == "ASSESSMENT" && AssessmentICDs.Contains(eandmCode.EandMCodingICDList[j].ICD.Trim()))
                        ////else if (eandmCode.EandMCodingICDList[j].Source.ToUpper() == "ASSESSMENT")
                        //{
                        //    if (AssessmentICDs != null && AssessmentICDs.Count == 0)
                        //    {
                        //        if (eandmCode.EandMCodingICDList[j].ICD_Category.ToUpper() == "PRIMARY")
                        //            sPrimary = "Pri";
                        //    }
                        //    else if (AssessmentICDs.Contains(eandmCode.EandMCodingICDList[j].ICD.Trim()))
                        //    {
                        //        if (eandmCode.EandMCodingICDList[j].ICD_Category.ToUpper() == "PRIMARY")
                        //            sPrimary = ((EMPriICDSet == true) ? "Pri" : "");
                        //        else if (eandmCode.EandMCodingICDList[j].ICD.Trim().Equals(AssPriICD))
                        //            sPrimary = "Pri";
                        //    }
                        //    Source = "ASSESSMENT";

                        //}
                        //else if (eandmCode.EandMCodingICDList[j].Source.ToUpper() == "ORDERS_ASSESSMENT")
                        //{
                        //    Source = "ORDERS_ASSESSMENT";
                        //}
                        if (Source.Trim() != string.Empty && Source == "EMICD")
                        {
                            if (!ICDList.Any(a => a.Split('~')[1] == eandmCode.EandMCodingICDList[j].ICD))
                                ICDList.Add(Source + "~" + eandmCode.EandMCodingICDList[j].ICD + "~" + eandmCode.EandMCodingICDList[j].ICD_Description + "~" + sICDVersion + "~" + sPrimary + "~" + sICDID + "~" + eandmCode.EandMCodingICDList[j].Sequence + "~" + ",");
                            else
                            {
                                IList<string> sReomveICDDesc = ICDList.Select(a => a.Split('~')[1]).ToList();
                                int iIndex = sReomveICDDesc.IndexOf(eandmCode.EandMCodingICDList[j].ICD);
                                //if (iIndex > -1)
                                //    if (ICDList[iIndex].Split('~').Length == 8)
                                //    {
                                //        //added for 6th ICD Column Marking
                                //        if (ICDList[iIndex].Split('~')[ICDList[iIndex].Split('~').Length - 1].Split(',').Length < 6)
                                //        {
                                //            for (int i = 0; i < eandmCode.EandMCodingICDList[j].Sequence; i++)
                                //            {
                                //                if (eandmCode.EandMCodingICDList[j].Sequence - 1 == i)
                                //                    ICDList[iIndex] += eandmCode.EandMCodingICDList[j].Sequence;
                                //                else if (ICDList[iIndex].Split('~')[ICDList[iIndex].Split('~').Length - 1].Split(',').Length != eandmCode.EandMCodingICDList[j].Sequence)
                                //                    ICDList[iIndex] += ",";
                                //            }
                                //        }
                                //    }
                            }
                        }

                        aryAccessICD.Add(eandmCode.EandMCodingICDList[j].ICD);
                    }
                }
                int iAssessmentSequence = 1;
                //if (eandmCode.EandMCodingICDList.Count != 0)
                if (EMCodingAssessmentSequence.Count != 0)
                {
                    IList<EandMCodingICD> lstEmcoding = EMCodingAssessmentSequence.OrderBy(a => Convert.ToUInt32(a.Sequence.Replace("A", "").Replace("B", ""))).ToList();
                    iAssessmentSequence = Convert.ToInt32(lstEmcoding[EMCodingAssessmentSequence.Count - 1].Sequence.Replace("A", "").Replace("B", "")) + 1;
                    //iAssessmentSequence = Convert.ToInt32(eandmCode.EandMCodingICDList[eandmCode.EandMCodingICDList.Count - 1].Sequence.Replace("A", "").Replace("B", "")) + 1;
                }

                if (eandmCode.AssessmentList != null && eandmCode.AssessmentList.Count > 0)
                {
                    IList<EandMCodingICD> lstemp = new List<EandMCodingICD>();
                    lstemp = (from m in eandmCode.EandMCodingICDList where m.Source == "ASSESSMENT" select m).ToList<EandMCodingICD>();
                    IList<Assessment> ICDListinAss_notinEMICD = new List<Assessment>();
                    ICDListinAss_notinEMICD = (from a in eandmCode.AssessmentList where !(EandMAssICDs.Contains(a.ICD.Trim())) select a).ToList<Assessment>();


                    if (ICDListinAss_notinEMICD != null && ICDListinAss_notinEMICD.Count > 0)
                        foreach (Assessment assICD in ICDListinAss_notinEMICD)
                        {
                            sAssesmentPrimary = string.Empty;
                            if (assICD.Primary_Diagnosis.ToUpper() == "Y")
                                sAssesmentPrimary = "Pri";
                            string sequencrtemp = "";
                            if (lstemp.Count > 0)
                            {
                                IList<EandMCodingICD> temp = (from a in lstemp where a.ICD == assICD.ICD select a).ToList<EandMCodingICD>();
                                if (temp.Count > 0)
                                {
                                    sequencrtemp = temp[0].Sequence;
                                }
                                else
                                {
                                    sequencrtemp = "A" + iAssessmentSequence.ToString();
                                }

                            }
                            else
                            {
                                sequencrtemp = "A" + iAssessmentSequence.ToString();
                            }
                            ICDList.Add("ASSESSMENT" + "~" + assICD.ICD + "~" + assICD.ICD_Description + "~" + 0 + "~" + sAssesmentPrimary + "~" + "0" + "~" + sequencrtemp + "~" + "");
                            //ICDList.Add("ASSESSMENT" + "~" + assICD.ICD + "~" + assICD.ICD_Description + "~" + 0 + "~" + sAssesmentPrimary + "~" + "0" + "~" + "6" + "~" + "" + "~" + "A" + iAssessmentSequence.ToString());
                            iAssessmentSequence += 1;
                        }
                }
                if (OrdersAssICDList != null && OrdersAssICDList.Count > 0)
                {
                    IList<OrdersAssIcd> ICDListinOrders_notinEMICD = new List<OrdersAssIcd>();
                    ICDListinOrders_notinEMICD = (from a in OrdersAssICDList where !(EandMOrdersAssICDs.Contains(a.ICD.Trim())) select a).ToList<OrdersAssIcd>();
                    if (ICDListinOrders_notinEMICD != null && ICDListinOrders_notinEMICD.Count > 0)
                        foreach (OrdersAssIcd assOrderICD in ICDListinOrders_notinEMICD)
                        {
                            sAssesmentPrimary = string.Empty;
                            //ICDList.Add("ORDERS_ASSESSMENT" + "~" + assOrderICD.ICD + "~" + assOrderICD.ICD_Description + "~" + 0 + "~" + sAssesmentPrimary + "~" + "0" + "~" + "6" + "~" + "" + "~" + "A" + iAssessmentSequence.ToString());
                            ICDList.Add("ORDERS_ASSESSMENT" + "~" + assOrderICD.ICD + "~" + assOrderICD.ICD_Description + "~" + 0 + "~" + sAssesmentPrimary + "~" + "0" + "~" + "A" + iAssessmentSequence.ToString() + "~" + "");
                            iAssessmentSequence += 1;
                        }
                }

                #endregion

                #region EAndMCodingList

                //if (xmlEncounterdoc.GetElementsByTagName("EAndMCodingList") != null)
                //{
                //    if (xmlEncounterdoc.GetElementsByTagName("EAndMCodingList").Count > 0)
                //    {
                //        xmlTagName = xmlEncounterdoc.GetElementsByTagName("EAndMCodingList")[0].ChildNodes;
                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                string TagName = xmlTagName[j].Name;
                //                XmlSerializer xmlserializer = new XmlSerializer(typeof(EAndMCoding));
                //                EAndMCoding EAndMCoding = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as EAndMCoding;
                //                IEnumerable<PropertyInfo> propInfo = null;
                //                if (EAndMCoding != null)
                //                {
                //                    propInfo = from obji in ((EAndMCoding)EAndMCoding).GetType().GetProperties() select obji;

                //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                    {
                //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                        {
                //                            foreach (PropertyInfo property in propInfo)
                //                            {
                //                                if (property.Name == nodevalue.Name)
                //                                {
                //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                        property.SetValue(EAndMCoding, Convert.ToUInt64(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                        property.SetValue(EAndMCoding, Convert.ToString(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                        property.SetValue(EAndMCoding, Convert.ToDateTime(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                        property.SetValue(EAndMCoding, Convert.ToInt32(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                //                                        property.SetValue(EAndMCoding, Convert.ToDecimal(nodevalue.Value), null);
                //                                    else
                //                                        property.SetValue(EAndMCoding, nodevalue.Value, null);
                //                                }
                //                            }
                //                        }
                //                    }
                //                    if (EAndMCoding.Encounter_ID == ulEncID)
                //                    {
                //                        if (EandMCPTs.IndexOf(EAndMCoding.Procedure_Code) == -1)
                //                            EandMCPTs.Add(EAndMCoding.Procedure_Code);//BugID:49118
                //                        if (EAndMCoding.Is_Delete == "N" || EAndMCoding.Is_Delete == "")
                //                        {
                //                            //if (aryAccessCPT.Contains(EAndMCoding.Procedure_Code) == false)
                //                            //{
                //                            eandmCode.EandMCodingList.Add(EAndMCoding);
                //                            ProcList.Add(EAndMCoding.Procedure_Code + "~" + EAndMCoding.Procedure_Code_Description + "~" + EAndMCoding.Id + "~" + EAndMCoding.Units + "~" + EAndMCoding.Modifier1 + "~" + EAndMCoding.Modifier2 + "~" + EAndMCoding.Modifier3 + "~" + EAndMCoding.Modifier4 + "~" + string.Empty + "~" + EAndMCoding.Version + "~" + EAndMCoding.Diagnosis_Pointer_1 + "~" + EAndMCoding.Diagnosis_Pointer_2 + "~" + EAndMCoding.Diagnosis_Pointer_3 + "~" + EAndMCoding.Diagnosis_Pointer_4 + "~" + EAndMCoding.Diagnosis_Pointer_5 + "~" + EAndMCoding.Diagnosis_Pointer_6 + "~" + EAndMCoding.Sort_Order);
                //                            //    aryAccessCPT.Add(EAndMCoding.Procedure_Code);
                //                            //}
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion
                //fs.Close();
                //fs.Dispose();
                // }
                //  }
                IList<string> ilstEandMHumanTagList = new List<string>();
                ilstEandMHumanTagList.Add("ImmunizationList");
                ilstEandMHumanTagList.Add("InHouseProcedureList");
                ilstEandMHumanTagList.Add("OrdersSubmitList");
                ilstEandMHumanTagList.Add("OrdersList");
                ilstEandMHumanTagList.Add("OrdersAssessmentList");

                IList<string> OrdersSubmitIDLst = new List<string>();
                IList<string> OrdersIDLst = new List<string>();

                //eandmCode.EandMCodingICDList = new List<EandMCodingICD>();

                IList<object> ilstEandMHumanBlobFinal = new List<object>();
                IList<Immunization> lstimm = new List<Immunization>();
                IList<InHouseProcedure> lstinhouse = new List<InHouseProcedure>();
                IList<OrdersSubmit> lstordersub = new List<OrdersSubmit>();
                IList<Orders> lstorder = new List<Orders>();
                IList<OrdersAssessment> lstorderass = new List<OrdersAssessment>();
                ilstEandMHumanBlobFinal = ReadBlob(ulHumanID, ilstEandMHumanTagList);

                if (ilstEandMHumanBlobFinal != null && ilstEandMHumanBlobFinal.Count > 0)
                {
                    if (ilstEandMHumanBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstEandMHumanBlobFinal[0]).Count; iCount++)
                        {
                            lstimm.Add((Immunization)((IList<object>)ilstEandMHumanBlobFinal[0])[iCount]);
                        }
                        for (int i = 0; i < lstimm.Count; i++)
                        {
                            if (lstimm[i].Encounter_Id == ulEncID && lstimm[i].Vaccine_In_House == "Y" && lstimm[i].Is_Administration_Refused.ToUpper() != "Y" && lstimm[i].Is_Deleted.ToUpper() == "N")//if Administration Refused Do not suggest in EandMCoding Screen
                            {
                                // TempProcedureList.Add(Immunization.Procedure_Code + "~" + Immunization.Immunization_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                                TempProcedureListImmunization.Add(lstimm[i].Procedure_Code + "~" + lstimm[i].Immunization_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                                //if (aryAccessCPT.Contains(Immunization.Procedure_Code) == false)
                                //{
                                //    ProcList.Add(Immunization.Procedure_Code + "~" + Immunization.Immunization_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                                //    aryAccessCPT.Add(Immunization.Procedure_Code);
                                //}
                                ImmCPT.Add(lstimm[i].Procedure_Code);
                                ImmDose.Add(lstimm[i].Administered_Amount.ToString());
                            }
                        }
                        if (ImmCPT.Count > 0)
                            lstprocedure = obj.GetProcedureList(ImmCPT);
                        if (ImmCPT.Count > 0)
                            lstprocedurelib = objlib.GetProcedureList(ImmCPT);
                        for (int i = 0; i < ImmCPT.Count; i++)
                        {

                            IList<ProcedureCodeLibrary> temp = (from m in lstprocedurelib
                                                                where
                                                                    m.Procedure_Code == ImmCPT[i].ToString() && m.Default_Dose != ""
                                                                select m).ToList<ProcedureCodeLibrary>();
                            if (temp.Count > 0)
                            {
                                if (ImmDose[i] != "" && ImmDose[i] != "0")
                                {
                                    var dose = Math.Ceiling(Convert.ToDouble(ImmDose[i]) / Convert.ToDouble(temp[0].Default_Dose.Split(' ')[0]));
                                    Immcptunit.Add(ImmCPT[i] + "~" + dose.ToString());
                                }
                                else
                                {
                                    Immcptunit.Add(ImmCPT[i] + "~" + "1");
                                }


                            }
                            else
                            {
                                Immcptunit.Add(ImmCPT[i] + "~" + "1");
                            }

                            IList<string> FinalProcedurelist = (from m in TempProcedureListImmunization where m.Split('~')[0] == ImmCPT[i].ToString() select m).ToList<string>();


                            if (FinalProcedurelist.Count > 0)
                            {
                                IList<ProcedureModifierLookup> tempSort = (from m in lstprocedure where m.Procedure_Code == ImmCPT[i].ToString() && m.Modifier == String.Empty select m).ToList<ProcedureModifierLookup>();
                                if (tempSort.Count > 0)
                                {
                                    TempProcedureList.Add(FinalProcedurelist[0] + "~" + tempSort[0].Sort_Order + "~" + tempSort[0].RVU);
                                }
                                else
                                {
                                    TempProcedureList.Add(FinalProcedurelist[0] + "~" + 9 + "~" + 0);
                                }
                            }
                        }
                    }
                    if (ilstEandMHumanBlobFinal[1] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstEandMHumanBlobFinal[1]).Count; iCount++)
                        {
                            lstinhouse.Add((InHouseProcedure)((IList<object>)ilstEandMHumanBlobFinal[1])[iCount]);
                        }
                        for (int i = 0; i < lstinhouse.Count; i++)
                        {
                            if (lstinhouse[i].Encounter_ID == ulEncID)
                            {
                                if (lstinhouse[i].Procedure_Code != "99999")
                                {

                                    InProcCPT.Add(lstinhouse[i].Procedure_Code);
                                    TempProcedureListInhousePro.Add(lstinhouse[i].Procedure_Code + "~" + lstinhouse[i].Procedure_Code_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                                }
                            }
                        }


                        //For SortOrder
                        ProcedureModifierLookupManager objcpt = new ProcedureModifierLookupManager();
                        if (InProcCPT.Count > 0)
                            lstprocedure = objcpt.GetProcedureList(InProcCPT);
                        for (int i = 0; i < InProcCPT.Count; i++)
                        {
                            IList<string> FinalProcedurelist = (from m in TempProcedureListInhousePro where m.Split('~')[0] == InProcCPT[i].ToString() select m).ToList<string>();


                            if (FinalProcedurelist.Count > 0)
                            {
                                IList<ProcedureModifierLookup> tempSort = (from m in lstprocedure where m.Procedure_Code == InProcCPT[i].ToString() && m.Modifier == string.Empty select m).ToList<ProcedureModifierLookup>();
                                if (tempSort.Count > 0)
                                {
                                    TempProcedureList.Add(FinalProcedurelist[0] + "~" + tempSort[0].Sort_Order + "~" + tempSort[0].RVU);
                                }
                                else
                                {
                                    TempProcedureList.Add(FinalProcedurelist[0] + "~" + 9 + "~" + 0);
                                }
                            }
                        }

                    }
                    if (Is_CMG_Ancillary)
                    {

                        if (ilstEandMHumanBlobFinal[2] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstEandMHumanBlobFinal[2]).Count; iCount++)
                            {
                                lstordersub.Add((OrdersSubmit)((IList<object>)ilstEandMHumanBlobFinal[2])[iCount]);
                            }
                            for (int i = 0; i < lstordersub.Count; i++)
                            {
                                if (lstordersub[i].Is_Deleted == "N" && lstordersub[i].Encounter_ID == ulEncID)
                                {
                                    OrdersSubmitIDLst.Add(lstordersub[i].Id.ToString().Trim());
                                }
                            }
                        }
                        if (ilstEandMHumanBlobFinal[3] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstEandMHumanBlobFinal[3]).Count; iCount++)
                            {
                                lstorder.Add((Orders)((IList<object>)ilstEandMHumanBlobFinal[3])[iCount]);
                            }
                            for (int i = 0; i < lstorder.Count; i++)
                            {
                                if (OrdersSubmitIDLst.Contains(lstorder[i].Order_Submit_ID.ToString().Trim()) && lstorder[i].Encounter_ID == ulEncID)
                                {
                                    string Lab_Procedure = lstorder[i].Lab_Procedure.ToString().Trim();
                                    string Lab_Procedure_Description = lstorder[i].Lab_Procedure_Description.ToString().Trim();

                                    OrderCPT.Add(Lab_Procedure);
                                    TempProcedureListOrdersList.Add(Lab_Procedure + "~" + Lab_Procedure_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                                    OrdersIDLst.Add(lstorder[i].Id.ToString().Trim());
                                }
                            }
                            //For SortOrder
                            if (OrderCPT.Count > 0)
                                lstprocedure = obj.GetProcedureList(OrderCPT);
                            for (int i = 0; i < OrderCPT.Count; i++)
                            {
                                IList<string> FinalProcedurelist = (from m in TempProcedureListOrdersList where m.Split('~')[0] == OrderCPT[i].ToString() select m).ToList<string>();


                                if (FinalProcedurelist.Count > 0)
                                {
                                    IList<ProcedureModifierLookup> tempSort = (from m in lstprocedure where m.Procedure_Code == OrderCPT[i].ToString() && m.Modifier == String.Empty select m).ToList<ProcedureModifierLookup>();
                                    if (tempSort.Count > 0)
                                    {
                                        TempProcedureList.Add(FinalProcedurelist[0] + "~" + tempSort[0].Sort_Order + tempSort[0].RVU);
                                    }
                                    else
                                    {
                                        TempProcedureList.Add(FinalProcedurelist[0] + "~" + 9 + 0);
                                    }

                                }
                            }
                        }
                        if (ilstEandMHumanBlobFinal[4] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstEandMHumanBlobFinal[4]).Count; iCount++)
                            {
                                lstorderass.Add((OrdersAssessment)((IList<object>)ilstEandMHumanBlobFinal[4])[iCount]);
                            }
                            for (int i = 0; i < lstorderass.Count; i++)
                            {
                                if (OrdersSubmitIDLst.Contains(lstorderass[i].Order_Submit_ID.ToString().Trim()) && lstorderass[i].Encounter_ID == ulEncID)
                                {
                                    string Ass_ICD = lstorderass[i].ICD.ToString();
                                    string Ass_ICDDesc = lstorderass[i].ICD_Description.ToString();
                                    ulong AssOrder_ID = Convert.ToUInt32(lstorderass[i].Order_Submit_ID.ToString());

                                    if (OrdersAssessmentICDs.Contains(Ass_ICD) == false)
                                    {
                                        OrdersAssessmentICDs.Add(Ass_ICD.Trim());
                                        OrdersAssIcd objOrderAssIcd = new OrdersAssIcd();
                                        objOrderAssIcd.ICD = Ass_ICD;
                                        objOrderAssIcd.ICD_Description = Ass_ICDDesc;
                                        objOrderAssIcd.Order_ID = AssOrder_ID;
                                        OrdersAssICDList.Add(objOrderAssIcd);
                                    }
                                }
                            }
                            iAssessmentSequence = 1;
                            if (OrdersAssICDList != null && OrdersAssICDList.Count > 0)
                            {
                                IList<OrdersAssIcd> ICDListinOrders_notinEMICD = new List<OrdersAssIcd>();
                                ICDListinOrders_notinEMICD = (from a in OrdersAssICDList where !(EandMOrdersAssICDs.Contains(a.ICD.Trim())) select a).ToList<OrdersAssIcd>();
                                if (ICDListinOrders_notinEMICD != null && ICDListinOrders_notinEMICD.Count > 0)
                                    foreach (OrdersAssIcd assOrderICD in ICDListinOrders_notinEMICD)
                                    {
                                        sAssesmentPrimary = string.Empty;
                                        //ICDList.Add("ORDERS_ASSESSMENT" + "~" + assOrderICD.ICD + "~" + assOrderICD.ICD_Description + "~" + 0 + "~" + sAssesmentPrimary + "~" + "0" + "~" + "6" + "~" + "" + "~" + "A" + iAssessmentSequence.ToString());
                                        ICDList.Add("ORDERS_ASSESSMENT" + "~" + assOrderICD.ICD + "~" + assOrderICD.ICD_Description + "~" + 0 + "~" + sAssesmentPrimary + "~" + "0" + "~" + "A" + iAssessmentSequence.ToString() + "~" + "");
                                        iAssessmentSequence += 1;
                                    }
                            }
                        }
                    }
                    if (!Is_CMG_Ancillary)
                    {

                        if (ilstEandMHumanBlobFinal[2] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstEandMHumanBlobFinal[2]).Count; iCount++)
                            {
                                lstordersub.Add((OrdersSubmit)((IList<object>)ilstEandMHumanBlobFinal[2])[iCount]);
                            }
                            for (int i = 0; i < lstordersub.Count; i++)
                            {
                                if (lstordersub[i].Is_Deleted == "N" && lstordersub[i].Encounter_ID == ulEncID && lstordersub[i].Order_Code_Type == "CMG ANC.-IN HOUSE")
                                {
                                    OrdersSubmitIDLst.Add(lstordersub[i].Id.ToString().Trim());
                                }
                            }
                        }

                        if (ilstEandMHumanBlobFinal[3] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstEandMHumanBlobFinal[3]).Count; iCount++)
                            {
                                lstorder.Add((Orders)((IList<object>)ilstEandMHumanBlobFinal[3])[iCount]);
                            }
                            for (int i = 0; i < lstorder.Count; i++)
                            {


                                if (OrdersSubmitIDLst.Contains(lstorder[i].Order_Submit_ID.ToString().Trim()) && lstorder[i].Encounter_ID == ulEncID)
                                {
                                    string Lab_Procedure = lstorder[i].Lab_Procedure.ToString().Trim();
                                    string Lab_Procedure_Description = lstorder[i].Lab_Procedure_Description.ToString().Trim();


                                    TempProcedureListOrdersList1.Add(Lab_Procedure + "~" + Lab_Procedure_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                                    OrderCPT1.Add(Lab_Procedure);
                                    OrdersIDLst.Add(lstorder[i].Id.ToString().Trim());
                                }
                            }

                            //For SortOrder
                            if (OrderCPT1.Count > 0)
                                lstprocedure = obj.GetProcedureList(OrderCPT1);
                            for (int i = 0; i < OrderCPT1.Count; i++)
                            {
                                IList<string> FinalProcedurelist = (from m in TempProcedureListOrdersList1 where m.Split('~')[0] == OrderCPT1[i].ToString() select m).ToList<string>();


                                if (FinalProcedurelist.Count > 0)
                                {
                                    IList<ProcedureModifierLookup> tempSort = (from m in lstprocedure where m.Procedure_Code == OrderCPT1[i].ToString() && m.Modifier == string.Empty select m).ToList<ProcedureModifierLookup>();
                                    if (tempSort.Count > 0)
                                    {
                                        TempProcedureList.Add(FinalProcedurelist[0] + "~" + tempSort[0].Sort_Order + "~" + tempSort[0].RVU);
                                    }
                                    else
                                    {
                                        TempProcedureList.Add(FinalProcedurelist[0] + "~" + 9 + "~" + 0);
                                    }
                                }
                            }

                        }
                    }
                }
                // string FileName = "Human" + "_" + ulHumanID + ".xml";
                // string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                //if (File.Exists(strXmlFilePath) == true)
                //{
                //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                //    XmlNodeList xmlTagName = null;
                //    // itemDoc.Load(XmlText);
                //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        xmlHumandoc.Load(fs);

                //        XmlText.Close();

                //BugID:46231
                #region ProblemList
                //if (itemDoc.GetElementsByTagName("ProblemListList") != null)
                //{
                //    if (itemDoc.GetElementsByTagName("ProblemListList").Count > 0)
                //    {
                //        xmlTagName = itemDoc.GetElementsByTagName("ProblemListList")[0].ChildNodes;

                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                string TagName = xmlTagName[j].Name;
                //                XmlSerializer xmlserializer = new XmlSerializer(typeof(ProblemList));
                //                ProblemList ProblemList = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ProblemList;
                //                IEnumerable<PropertyInfo> propInfo = null;
                //                if (ProblemList != null)
                //                {
                //                    propInfo = from obji in ((ProblemList)ProblemList).GetType().GetProperties() select obji;

                //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                    {
                //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                        {
                //                            foreach (PropertyInfo property in propInfo)
                //                            {
                //                                if (property.Name == nodevalue.Name)
                //                                {
                //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                        property.SetValue(ProblemList, Convert.ToUInt64(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                        property.SetValue(ProblemList, Convert.ToString(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                        property.SetValue(ProblemList, Convert.ToDateTime(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                        property.SetValue(ProblemList, Convert.ToInt32(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                //                                        property.SetValue(ProblemList, Convert.ToDecimal(nodevalue.Value), null);
                //                                    else
                //                                        property.SetValue(ProblemList, nodevalue.Value, null);
                //                                }
                //                            }
                //                        }
                //                    }
                //                    if (ProblemList.Human_ID == ulHumanID && ProblemList.Status == "Active" && ProblemList.Is_Active == "Y" && ProblemList.Version_Year == ICDType)
                //                    {
                //                        if (aryAccessICD.Contains(ProblemList.ICD) == false && ProblemList.ICD != string.Empty && ProblemList.ICD != "0000")
                //                        {
                //                            ICDList.Add("PROBLEMLIST" + "~" + ProblemList.ICD + "~" + ProblemList.Problem_Description + "~" + 0 + "~" + "N" + "~" + "0" + "~" + "6" + "~" + "");
                //                            probList.Add(ProblemList);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion
                #region ImmunizationTab
                //if (eandmCode.EandMCodingList.Count == 0 && xmlHumandoc.GetElementsByTagName("ImmunizationList") != null)
                //if (xmlHumandoc.GetElementsByTagName("ImmunizationList") != null)
                //{
                //    if (xmlHumandoc.GetElementsByTagName("ImmunizationList").Count > 0)
                //    {
                //        xmlTagName = xmlHumandoc.GetElementsByTagName("ImmunizationList")[0].ChildNodes;

                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                string TagName = xmlTagName[j].Name;
                //                XmlSerializer xmlserializer = new XmlSerializer(typeof(Immunization));
                //                Immunization Immunization = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Immunization;
                //                IEnumerable<PropertyInfo> propInfo = null;
                //                if (Immunization != null)
                //                {
                //                    propInfo = from obji in ((Immunization)Immunization).GetType().GetProperties() select obji;

                //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                    {
                //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                        {
                //                            foreach (PropertyInfo property in propInfo)
                //                            {
                //                                if (property.Name == nodevalue.Name)
                //                                {
                //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                        property.SetValue(Immunization, Convert.ToUInt64(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                        property.SetValue(Immunization, Convert.ToString(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                        property.SetValue(Immunization, Convert.ToDateTime(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                        property.SetValue(Immunization, Convert.ToInt32(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                //                                        property.SetValue(Immunization, Convert.ToDecimal(nodevalue.Value), null);
                //                                    else
                //                                        property.SetValue(Immunization, nodevalue.Value, null);
                //                                }
                //                            }
                //                        }
                //                    }
                //                    if (Immunization.Encounter_Id == ulEncID && Immunization.Vaccine_In_House == "Y" && Immunization.Is_Administration_Refused.ToUpper() != "Y" && Immunization.Is_Deleted.ToUpper() == "N")//if Administration Refused Do not suggest in EandMCoding Screen
                //                    {
                //                        // TempProcedureList.Add(Immunization.Procedure_Code + "~" + Immunization.Immunization_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                //                        TempProcedureListImmunization.Add(Immunization.Procedure_Code + "~" + Immunization.Immunization_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                //                        //if (aryAccessCPT.Contains(Immunization.Procedure_Code) == false)
                //                        //{
                //                        //    ProcList.Add(Immunization.Procedure_Code + "~" + Immunization.Immunization_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                //                        //    aryAccessCPT.Add(Immunization.Procedure_Code);
                //                        //}
                //                        ImmCPT.Add(Immunization.Procedure_Code);
                //                        ImmDose.Add(Immunization.Administered_Amount.ToString());
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                //if (ImmCPT.Count > 0)
                //    lstprocedure = obj.GetProcedureList(ImmCPT);
                //for (int i = 0; i < ImmCPT.Count; i++)
                //{

                //    IList<ProcedureCodeLibrary> temp = (from m in lstprocedure
                //                                        where
                //                                            m.Procedure_Code == ImmCPT[i].ToString() && m.Default_Dose != ""
                //                                        select m).ToList<ProcedureCodeLibrary>();
                //    if (temp.Count > 0)
                //    {
                //        if (ImmDose[i] != "" && ImmDose[i] != "0")
                //        {
                //            var dose = Math.Ceiling(Convert.ToDouble(ImmDose[i]) / Convert.ToDouble(temp[0].Default_Dose.Split(' ')[0]));
                //            Immcptunit.Add(ImmCPT[i] + "~" + dose.ToString());
                //        }
                //        else
                //        {
                //            Immcptunit.Add(ImmCPT[i] + "~" + "1");
                //        }


                //    }
                //    else
                //    {
                //        Immcptunit.Add(ImmCPT[i] + "~" + "1");
                //    }

                //    IList<string> FinalProcedurelist = (from m in TempProcedureListImmunization where m.Split('~')[0] == ImmCPT[i].ToString() select m).ToList<string>();


                //    if (FinalProcedurelist.Count > 0)
                //    {
                //        IList<ProcedureCodeLibrary> tempSort = (from m in lstprocedure where m.Procedure_Code == ImmCPT[i].ToString() select m).ToList<ProcedureCodeLibrary>();
                //        if (tempSort.Count > 0)
                //        {
                //            TempProcedureList.Add(FinalProcedurelist[0] + "~" + tempSort[0].Sort_Order);
                //        }
                //    }
                //}
                //  var temp = from m in lstprocedure where m.Default_Dose != "" && (from n in ImmDose select n.Split('~')).Contains(m.Procedure_Code) select m;

                #endregion
                #region ProceduresTab
                //if (eandmCode.EandMCodingList.Count == 0 && xmlHumandoc.GetElementsByTagName("InHouseProcedureList") != null)
                //if (xmlHumandoc.GetElementsByTagName("InHouseProcedureList") != null)
                //{
                //    if (xmlHumandoc.GetElementsByTagName("InHouseProcedureList").Count > 0)
                //    {
                //        xmlTagName = xmlHumandoc.GetElementsByTagName("InHouseProcedureList")[0].ChildNodes;

                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                string TagName = xmlTagName[j].Name;
                //                XmlSerializer xmlserializer = new XmlSerializer(typeof(InHouseProcedure));
                //                InHouseProcedure InHouseProcedure = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as InHouseProcedure;
                //                IEnumerable<PropertyInfo> propInfo = null;
                //                if (InHouseProcedure != null)
                //                {
                //                    propInfo = from obji in ((InHouseProcedure)InHouseProcedure).GetType().GetProperties() select obji;

                //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                    {
                //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                        {
                //                            foreach (PropertyInfo property in propInfo)
                //                            {
                //                                if (property.Name == nodevalue.Name)
                //                                {
                //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                        property.SetValue(InHouseProcedure, Convert.ToUInt64(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                        property.SetValue(InHouseProcedure, Convert.ToString(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                        property.SetValue(InHouseProcedure, Convert.ToDateTime(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                        property.SetValue(InHouseProcedure, Convert.ToInt32(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                //                                        property.SetValue(InHouseProcedure, Convert.ToDecimal(nodevalue.Value), null);
                //                                    else
                //                                        property.SetValue(InHouseProcedure, nodevalue.Value, null);
                //                                }
                //                            }
                //                        }
                //                    }
                //                    if (InHouseProcedure.Encounter_ID == ulEncID)
                //                    {
                //                        if (InHouseProcedure.Procedure_Code != "99999")
                //                        {
                //                            /*
                //                             int sSort_Order = 0;
                //                               if (lstprocedure != null && lstprocedure.Count > 0)
                //                               {
                //                                   IList<ProcedureCodeLibrary> temp1 = (from m in lstprocedure where m.Procedure_Code == InHouseProcedure.Procedure_Code select m).ToList<ProcedureCodeLibrary>();
                //                                   if (temp1.Count > 0)
                //                                   {
                //                                       sSort_Order = temp1[0].Sort_Order;
                //                                   }
                //                               }
                //                               else
                //                               {
                //                                   lstprocedure = obj.GetProcedureList(ImmCPT);
                //                                   IList<ProcedureCodeLibrary> temp1 = (from m in lstprocedure where m.Procedure_Code == InHouseProcedure.Procedure_Code select m).ToList<ProcedureCodeLibrary>();
                //                                   if (temp1.Count > 0)
                //                                   {
                //                                       sSort_Order = temp1[0].Sort_Order;
                //                                   }
                //                               }
                //                               TempProcedureList.Add(InHouseProcedure.Procedure_Code + "~" + InHouseProcedure.Procedure_Code_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "" + "~" + sSort_Order);*/
                //                            InProcCPT.Add(InHouseProcedure.Procedure_Code);
                //                            TempProcedureListInhousePro.Add(InHouseProcedure.Procedure_Code + "~" + InHouseProcedure.Procedure_Code_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                //                        }
                //                        //if (aryAccessCPT.Contains(InHouseProcedure.Procedure_Code) == false)
                //                        //{
                //                        //    ProcList.Add(InHouseProcedure.Procedure_Code + "~" + InHouseProcedure.Procedure_Code_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                //                        //    aryAccessCPT.Add(InHouseProcedure.Procedure_Code);
                //                        //}
                //                    }
                //                }
                //            }

                //           //For SortOrder 
                //            if (InProcCPT.Count > 0)
                //                lstprocedure = obj.GetProcedureList(InProcCPT);
                //            for (int i = 0; i < InProcCPT.Count; i++)
                //            {
                //                IList<string> FinalProcedurelist = (from m in TempProcedureListInhousePro where m.Split('~')[0] == InProcCPT[i].ToString() select m).ToList<string>();


                //                if (FinalProcedurelist.Count > 0)
                //                {
                //                    IList<ProcedureCodeLibrary> tempSort = (from m in lstprocedure where m.Procedure_Code == InProcCPT[i].ToString() select m).ToList<ProcedureCodeLibrary>();
                //                    if (tempSort.Count > 0)
                //                    {
                //                        TempProcedureList.Add(FinalProcedurelist[0] + "~" + tempSort[0].Sort_Order);
                //                    }
                //                }
                //            }


                //        }
                //    }
                //}
                #endregion

                //CMG Ancilliary
                #region OrdersSubmitTab
                //if (Is_CMG_Ancillary)
                //{

                //if (eandmCode.EandMCodingList.Count == 0 && xmlHumandoc.GetElementsByTagName("OrdersSubmitList") != null)
                //if (xmlHumandoc.GetElementsByTagName("OrdersSubmitList") != null)
                //{
                //    if (xmlHumandoc.GetElementsByTagName("OrdersSubmitList").Count > 0)
                //    {
                //        xmlTagName = xmlHumandoc.GetElementsByTagName("OrdersSubmitList")[0].ChildNodes;

                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                if (xmlTagName[j].Attributes.GetNamedItem("Is_Deleted").Value.Equals("N") && xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value.Equals(ulEncID.ToString()))
                //                {
                //                    OrdersSubmitIDLst.Add(xmlTagName[j].Attributes.GetNamedItem("Id").Value.ToString().Trim());
                //                }
                //            }
                //        }
                //    }
                //}
                //BugID:52776
                // if (eandmCode.EandMCodingList.Count == 0 && xmlHumandoc.GetElementsByTagName("OrdersList") != null)
                //if (xmlHumandoc.GetElementsByTagName("OrdersList") != null)
                //{
                //    if (xmlHumandoc.GetElementsByTagName("OrdersList").Count > 0 && OrdersSubmitIDLst.Count > 0)
                //    {
                //        xmlTagName = xmlHumandoc.GetElementsByTagName("OrdersList")[0].ChildNodes;

                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                if (OrdersSubmitIDLst.Contains(xmlTagName[j].Attributes.GetNamedItem("Order_Submit_ID").Value.ToString().Trim()) && xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value.Equals(ulEncID.ToString()))
                //                {
                //                    string Lab_Procedure = xmlTagName[j].Attributes.GetNamedItem("Lab_Procedure").Value.ToString().Trim();
                //                    string Lab_Procedure_Description = xmlTagName[j].Attributes.GetNamedItem("Lab_Procedure_Description").Value.ToString().Trim();
                //                    /*
                //                    int sSort_Order = 0;
                //                    if (lstprocedure != null && lstprocedure.Count > 0)
                //                    {
                //                        IList<ProcedureCodeLibrary> temp1 = (from m in lstprocedure where m.Procedure_Code == Lab_Procedure select m).ToList<ProcedureCodeLibrary>();
                //                        if (temp1.Count > 0)
                //                        {
                //                            sSort_Order = temp1[0].Sort_Order;
                //                        }
                //                    }
                //                    else
                //                    {
                //                        lstprocedure = obj.GetProcedureList(ImmCPT);
                //                        IList<ProcedureCodeLibrary> temp1 = (from m in lstprocedure where m.Procedure_Code == Lab_Procedure select m).ToList<ProcedureCodeLibrary>();
                //                        if (temp1.Count > 0)
                //                        {
                //                            sSort_Order = temp1[0].Sort_Order;
                //                        }
                //                    }
                //                    TempProcedureList.Add(Lab_Procedure + "~" + Lab_Procedure_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "" + "~" + sSort_Order);
                //                     */
                //                    OrderCPT.Add(Lab_Procedure);
                //                    TempProcedureListOrdersList.Add(Lab_Procedure + "~" + Lab_Procedure_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                //                    OrdersIDLst.Add(xmlTagName[j].Attributes.GetNamedItem("Id").Value.ToString().Trim());
                //                }
                //            }

                //            //For SortOrder
                //            if (OrderCPT.Count > 0)
                //                lstprocedure = obj.GetProcedureList(OrderCPT);
                //            for (int i = 0; i < OrderCPT.Count; i++)
                //            {
                //                IList<string> FinalProcedurelist = (from m in TempProcedureListOrdersList where m.Split('~')[0] == OrderCPT[i].ToString() select m).ToList<string>();


                //                if (FinalProcedurelist.Count > 0)
                //                {
                //                    IList<ProcedureCodeLibrary> tempSort = (from m in lstprocedure where m.Procedure_Code == OrderCPT[i].ToString() select m).ToList<ProcedureCodeLibrary>();
                //                    if (tempSort.Count > 0)
                //                    {
                //                        TempProcedureList.Add(FinalProcedurelist[0] + "~" + tempSort[0].Sort_Order);
                //                    }
                //                }
                //            }

                //        }
                //    }
                //}
                //    if (xmlHumandoc.GetElementsByTagName("OrdersAssessmentList") != null)
                //    {
                //        if (xmlHumandoc.GetElementsByTagName("OrdersAssessmentList").Count > 0)
                //        {
                //            xmlTagName = xmlHumandoc.GetElementsByTagName("OrdersAssessmentList")[0].ChildNodes;

                //            if (xmlTagName.Count > 0)
                //            {
                //                for (int j = 0; j < xmlTagName.Count; j++)
                //                {
                //                    //if (OrdersIDLst.Contains(xmlTagName[j].Attributes.GetNamedItem("Order_Submit_ID").Value.ToString().Trim()) && xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value.Equals(ulEncID.ToString()))
                //                    if (OrdersSubmitIDLst.Contains(xmlTagName[j].Attributes.GetNamedItem("Order_Submit_ID").Value.ToString().Trim()) && xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value.Equals(ulEncID.ToString()))
                //                    {
                //                        string Ass_ICD = xmlTagName[j].Attributes.GetNamedItem("ICD").Value.ToString();
                //                        string Ass_ICDDesc = xmlTagName[j].Attributes.GetNamedItem("ICD_Description").Value.ToString();
                //                        ulong AssOrder_ID = Convert.ToUInt32(xmlTagName[j].Attributes.GetNamedItem("Order_Submit_ID").Value.ToString());

                //                        if (OrdersAssessmentICDs.Contains(Ass_ICD) == false)
                //                        {
                //                            OrdersAssessmentICDs.Add(Ass_ICD.Trim());
                //                            OrdersAssIcd objOrderAssIcd = new OrdersAssIcd();
                //                            objOrderAssIcd.ICD = Ass_ICD;
                //                            objOrderAssIcd.ICD_Description = Ass_ICDDesc;
                //                            objOrderAssIcd.Order_ID = AssOrder_ID;
                //                            OrdersAssICDList.Add(objOrderAssIcd);
                //                        }
                //                    }
                //                }
                //            }

                //             iAssessmentSequence = 1;
                //            if (OrdersAssICDList != null && OrdersAssICDList.Count > 0)
                //            {
                //                IList<OrdersAssIcd> ICDListinOrders_notinEMICD = new List<OrdersAssIcd>();
                //                ICDListinOrders_notinEMICD = (from a in OrdersAssICDList where !(EandMOrdersAssICDs.Contains(a.ICD.Trim())) select a).ToList<OrdersAssIcd>();
                //                if (ICDListinOrders_notinEMICD != null && ICDListinOrders_notinEMICD.Count > 0)
                //                    foreach (OrdersAssIcd assOrderICD in ICDListinOrders_notinEMICD)
                //                    {
                //                        sAssesmentPrimary = string.Empty;
                //                        //ICDList.Add("ORDERS_ASSESSMENT" + "~" + assOrderICD.ICD + "~" + assOrderICD.ICD_Description + "~" + 0 + "~" + sAssesmentPrimary + "~" + "0" + "~" + "6" + "~" + "" + "~" + "A" + iAssessmentSequence.ToString());
                //                        ICDList.Add("ORDERS_ASSESSMENT" + "~" + assOrderICD.ICD + "~" + assOrderICD.ICD_Description + "~" + 0 + "~" + sAssesmentPrimary + "~" + "0" + "~" + "A" + iAssessmentSequence.ToString() + "~" + "");
                //                        iAssessmentSequence += 1;
                //                    }
                //            }
                //        }
                //    }
                //}
                #endregion
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}

                #region CMG In House Lab
                //IQuery query1 = iMySession.GetNamedQuery("Fill.IN-HOUSE PROCEDURES.List");
                //query1.SetParameter(0, ulEncID);
                //query1.SetParameter(1, "CMG Anc.-In House");
                //ArrayList arrList = new ArrayList(query1.List());
                //for (int i = 0; i < arrList.Count; i++)
                //{
                //    object[] ccObject = (object[])arrList[i];
                //eandmCode.OrderProcedureID.Add(Convert.ToUInt64(ccObject[0].ToString()));
                //eandmCode.OrderProcedure.Add(ccObject[1].ToString());
                //eandmCode.OrderProcedureDesc.Add(ccObject[2].ToString());
                //    TempProcedureList.Add(eandmCode.OrderProcedure[i] + "~" + eandmCode.OrderProcedureDesc[i] + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                //}
                //if (!Is_CMG_Ancillary)
                //{
                //    IList<string> OrdersSubmitIDLst = new List<string>();
                //    IList<string> OrdersIDLst = new List<string>();
                //    XmlNodeList xmlTagName = null;

                //if (xmlHumandoc.GetElementsByTagName("OrdersSubmitList") != null)
                //{
                //    if (xmlHumandoc.GetElementsByTagName("OrdersSubmitList").Count > 0)
                //    {
                //        xmlTagName = xmlHumandoc.GetElementsByTagName("OrdersSubmitList")[0].ChildNodes;

                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                if (xmlTagName[j].Attributes.GetNamedItem("Is_Deleted").Value.Equals("N") && xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value.Equals(ulEncID.ToString()) && xmlTagName[j].Attributes.GetNamedItem("Order_Code_Type").Value.Equals("CMG ANC.-IN HOUSE"))
                //                {
                //                    OrdersSubmitIDLst.Add(xmlTagName[j].Attributes.GetNamedItem("Id").Value.ToString().Trim());
                //                }
                //            }
                //        }
                //    }
                //}
                //BugID:52776
                //    if (xmlHumandoc.GetElementsByTagName("OrdersList") != null)
                //    {
                //        if (xmlHumandoc.GetElementsByTagName("OrdersList").Count > 0 && OrdersSubmitIDLst.Count > 0)
                //        {
                //            xmlTagName = xmlHumandoc.GetElementsByTagName("OrdersList")[0].ChildNodes;

                //            if (xmlTagName.Count > 0)
                //            {
                //                for (int j = 0; j < xmlTagName.Count; j++)
                //                {
                //                    if (OrdersSubmitIDLst.Contains(xmlTagName[j].Attributes.GetNamedItem("Order_Submit_ID").Value.ToString().Trim()) && xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value.Equals(ulEncID.ToString()))
                //                    {
                //                        string Lab_Procedure = xmlTagName[j].Attributes.GetNamedItem("Lab_Procedure").Value.ToString().Trim();
                //                        string Lab_Procedure_Description = xmlTagName[j].Attributes.GetNamedItem("Lab_Procedure_Description").Value.ToString().Trim();

                //                        /*
                //                        int sSort_Order = 0;
                //                        if (lstprocedure != null && lstprocedure.Count > 0)
                //                        {
                //                            IList<ProcedureCodeLibrary> temp1 = (from m in lstprocedure where m.Procedure_Code == Lab_Procedure select m).ToList<ProcedureCodeLibrary>();
                //                            if (temp1.Count > 0)
                //                            {
                //                                sSort_Order = temp1[0].Sort_Order;
                //                            }
                //                        }
                //                        else
                //                        {
                //                            lstprocedure = obj.GetProcedureList(ImmCPT);
                //                            IList<ProcedureCodeLibrary> temp1 = (from m in lstprocedure where m.Procedure_Code == Lab_Procedure select m).ToList<ProcedureCodeLibrary>();
                //                            if (temp1.Count > 0)
                //                            {
                //                                sSort_Order = temp1[0].Sort_Order;
                //                            }
                //                        }

                //                        TempProcedureList.Add(Lab_Procedure + "~" + Lab_Procedure_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "" + "~" + sSort_Order);
                //                        */
                //                        TempProcedureListOrdersList1.Add(Lab_Procedure + "~" + Lab_Procedure_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "");
                //                        OrderCPT1.Add(Lab_Procedure);
                //                        OrdersIDLst.Add(xmlTagName[j].Attributes.GetNamedItem("Id").Value.ToString().Trim());
                //                    }
                //                }

                //                //For SortOrder
                //                if (OrderCPT1.Count > 0)
                //                    lstprocedure = obj.GetProcedureList(OrderCPT1);
                //                for (int i = 0; i < OrderCPT1.Count; i++)
                //                {
                //                    IList<string> FinalProcedurelist = (from m in TempProcedureListOrdersList1 where m.Split('~')[0] == OrderCPT1[i].ToString() select m).ToList<string>();


                //                    if (FinalProcedurelist.Count > 0)
                //                    {
                //                        IList<ProcedureCodeLibrary> tempSort = (from m in lstprocedure where m.Procedure_Code == OrderCPT1[i].ToString() select m).ToList<ProcedureCodeLibrary>();
                //                        if (tempSort.Count > 0)
                //                        {
                //                            TempProcedureList.Add(FinalProcedurelist[0] + "~" + tempSort[0].Sort_Order);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion

                #endregion

                #endregion




                //Encounter enc = EM.GetById(ulEncID);
                //eandmCode.EncounterList.Add(enc);
                #region EncountersTab
                //if (xmlEncounterdoc.GetElementsByTagName("EncounterList") != null)
                //{
                //    if (xmlEncounterdoc.GetElementsByTagName("EncounterList").Count > 0)
                //    {
                //        XmlNodeList xmlTagName = null;
                //        xmlTagName = xmlEncounterdoc.GetElementsByTagName("EncounterList")[0].ChildNodes;

                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                string TagName = xmlTagName[j].Name;
                //                XmlSerializer xmlserializer = new XmlSerializer(typeof(Encounter));
                //                Encounter enc = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Encounter;
                //                IEnumerable<PropertyInfo> propInfo = null;
                //                if (enc != null)
                //                {
                //                    propInfo = from obji in ((Encounter)enc).GetType().GetProperties() select obji;

                //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                    {
                //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                        {
                //                            foreach (PropertyInfo property in propInfo)
                //                            {
                //                                if (property.Name == nodevalue.Name)
                //                                {
                //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                        property.SetValue(enc, Convert.ToUInt64(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                        property.SetValue(enc, Convert.ToString(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                        property.SetValue(enc, Convert.ToDateTime(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                        property.SetValue(enc, Convert.ToInt32(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                //                                        property.SetValue(enc, Convert.ToDecimal(nodevalue.Value), null);
                //                                    else
                //                                        property.SetValue(enc, nodevalue.Value, null);
                //                                }
                //                            }
                //                        }
                //                    }
                //                    eandmCode.EncounterList.Add(enc);
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion

                #region ProcedureCodeRuleMaster
                //string sDOSYear = string.Empty;
                //sDOSYear = Convert.ToString(date_of_service.Year);
                //XmlNodeList xmlTypeofVisit = xmlEncounterdoc.SelectNodes("/notes/Modules/EncounterList/Encounter[@Visit_Type!='RETINAL EYE SCAN']");
                ////if (eandmCode.EandMCodingList.Count == 0 && xmlTypeofVisit.Count > 0)
                //if (xmlTypeofVisit.Count > 0)
                //{
                //    IList<ProcedureCodeRuleMaster> ProcedureList = new List<ProcedureCodeRuleMaster>();
                //    ISQLQuery procsql = iMySession.CreateSQLQuery("SELECT p.* FROM procedure_code_rule_master p where  p.DOS_Year='" + sDOSYear + "' or p.DOS_Year=' ' order by p.Sort_Order,p.XML_Type").AddEntity("p", typeof(ProcedureCodeRuleMaster));
                //    ProcedureList = procsql.List<ProcedureCodeRuleMaster>();
                //    ArrayList arrayList1 = new ArrayList();

                //    Hashtable result = new Hashtable();
                //    Hashtable hashResultList = new Hashtable();
                //    string sTemp1 = string.Empty;
                //    string sTemp2 = string.Empty;
                //    ArrayList arrayList = new ArrayList();

                //    //var HumanBasedProcedureList = from p in ProcedureList where p.XML_Type == "HUMAN" select p;
                //    //IList<ProcedureCodeRuleMaster> ilstHumanBasedProcedureList = HumanBasedProcedureList.ToList<ProcedureCodeRuleMaster>();

                //    bool bMatchingFailed = false;
                //    bool bDoNotAdd = false;
                //    XmlNodeList xmlPhysicianList = null;


                //    for (int iCount = 0; iCount < ProcedureList.Count; iCount++)
                //    {
                //        var proc = from p in ProcedureList where p.Procedure_Code == ProcedureList[iCount].Procedure_Code select p;
                //        IList<ProcedureCodeRuleMaster> tempList = proc.ToList<ProcedureCodeRuleMaster>();

                //        bDoNotAdd = false;

                //        if (tempList.Count > 1) //If one CPT has more than 1 condition
                //        {
                //            if (tempList[tempList.Count - 1].Id == ProcedureList[iCount].Id) //If the current iteration is the last condition for the CPT then, we can suggest the CPT
                //                bDoNotAdd = false; //False for last iteration for that CPT, True for other than last iteration for the CPT
                //            else
                //                bDoNotAdd = true; //False for last iteration for that CPT, True for other than last iteration for the CPT

                //            if (tempList[0].Id == ProcedureList[iCount].Id) //If the current iteration is the first condition for the CPT then, we reset the Matching failed variable
                //                bMatchingFailed = false;  //False for no restriction, True in case if one of the previous rule is failed for the CPT then, prevent from suggesting the CPT
                //        }
                //        else
                //        {
                //            bDoNotAdd = false;
                //            bMatchingFailed = false;
                //        }

                //        if (ProcedureList[iCount].XML_Type == "HUMAN")
                //        {
                //            xmlPhysicianList = xmlHumandoc.SelectNodes(ProcedureList[iCount].XML_Query.Replace(":EncID", ulEncID.ToString()).Replace(":From12MonthDate", DateTime.Now.AddDays(-365).ToString("yyyy-MM-dd")).Replace(":From24MonthDate", DateTime.Now.AddDays(-730).ToString("yyyyMMdd")).Replace(":ToDate", DateTime.Now.ToString("yyyy-MM-dd")));
                //        }
                //        else
                //        {
                //            xmlPhysicianList = xmlEncounterdoc.SelectNodes(ProcedureList[iCount].XML_Query);
                //        }

                //        if (xmlPhysicianList.Count > 0 && bDoNotAdd == false && bMatchingFailed == false)
                //        {
                //            eandmCode.ProcedureMasterList.Add(ProcedureList[iCount].Procedure_Code + "~" + ProcedureList[iCount].Procedure_Code_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "" + "~" + ProcedureList[iCount].Sort_Order);
                //            TempProcedureList.Add(ProcedureList[iCount].Procedure_Code + "~" + ProcedureList[iCount].Procedure_Code_Description + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "" + "~" + ProcedureList[iCount].Sort_Order);
                //        }

                //        if (bDoNotAdd == true)
                //        {
                //            if (xmlPhysicianList.Count == 0)
                //            {
                //                bMatchingFailed = true;
                //            }
                //        }
                //    }
                //}

                #endregion
                IList<EAndMCoding> EMList = eandmCode.EandMCodingList;
                IList<string> FinalProcedureList = TempProcedureList;
                IList<string> DistinctProcedureList = FinalProcedureList.Select(a => (a.ToString().Split('~')[0] + "~" + a.ToString().Split('~')[1] + "~" + a.ToString().Split('~')[10] + "~" + a.ToString().Split('~')[11])).Distinct().ToList();


                for (int iDis = 0; iDis < DistinctProcedureList.Count; iDis++)
                {
                    int count1 = EMList.Where(a => (a.Procedure_Code + "~" + a.Procedure_Code_Description) == DistinctProcedureList[iDis].ToString().Split('~')[0] + "~" + DistinctProcedureList[iDis].ToString().Split('~')[1]).Count();
                    int count2 = FinalProcedureList.Where(a => (a.ToString().Split('~')[0] + "~" + a.ToString().Split('~')[1]) == DistinctProcedureList[iDis].ToString().Split('~')[0] + "~" + DistinctProcedureList[iDis].ToString().Split('~')[1]).Count();
                    if (count2 > count1)
                    {
                        for (int iDiff = 0; iDiff < (count2 - count1); iDiff++)
                        {

                            if (EandMCPTs.IndexOf(DistinctProcedureList[iDis].ToString().Split('~')[0]) == -1)
                            {
                                //BugID:48904 - A CPT which is present in E_M_Coding table will not be autosuggested anymore for the reasons that (Is_delete='Y' - user has deleted it, Is_delete='N' - already present in CPT list)
                                var check = (from m in Immcptunit where m.Split('~').Contains(DistinctProcedureList[iDis].ToString().Split('~')[0]) select m).ToArray();
                                if (check.Count() > 0)
                                    ProcList.Add(DistinctProcedureList[iDis].ToString().Split('~')[0] + "~" + DistinctProcedureList[iDis].ToString().Split('~')[1] + "~" + "" + "~" + check[0].Split('~')[1].ToString() + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + DistinctProcedureList[iDis].ToString().Split('~')[2] + "~" + DistinctProcedureList[iDis].ToString().Split('~')[3]);
                                else

                                    ProcList.Add(DistinctProcedureList[iDis].ToString().Split('~')[0] + "~" + DistinctProcedureList[iDis].ToString().Split('~')[1] + "~" + "" + "~" + "1" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "6" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + "" + "~" + DistinctProcedureList[iDis].ToString().Split('~')[2] + "~" + DistinctProcedureList[iDis].ToString().Split('~')[3]);
                            }
                        }
                    }
                }
                //string[] lstcpt = new string[] { };
                //lstcpt = ProcList.Select(a => a.Split('~')[0]).ToArray();
                //IList<string> mpdoproc = new List<string>();
                //mpdoproc = CPTModifier(lstcpt);

                //for (int y = 0; y < mpdoproc.Count; y++)
                //{
                //    for (int z = 0; z < ProcList.Count; z++)
                //    {
                //        if (ProcList[z].Split('~')[0] == mpdoproc[y].Split('~')[0])
                //        {
                //            string[] splitCPT = ProcList[z].Split('~');
                //            if (!ProcList[z].Contains(mpdoproc[y].Split('~')[1]))
                //            {
                //                if (splitCPT.Length > 0)
                //                {
                //                    if (splitCPT[4] == "")
                //                    {
                //                        splitCPT[4] = mpdoproc[y].Split('~')[1];
                //                    }
                //                    else if (splitCPT[5] == "")
                //                    {
                //                        splitCPT[5] = mpdoproc[y].Split('~')[1];
                //                    }
                //                    else if (splitCPT[6] == "")
                //                    {
                //                        splitCPT[6] = mpdoproc[y].Split('~')[1];
                //                    }
                //                    else if (splitCPT[7] == "")
                //                    {
                //                        splitCPT[7] = mpdoproc[y].Split('~')[1];
                //                    }
                //                }

                //                string cptwithmodi = "";
                //                for (int g = 0; g < splitCPT.Length; g++)
                //                {

                //                    if (g == 0)
                //                    {
                //                        cptwithmodi = splitCPT[g];
                //                    }
                //                    else
                //                    {
                //                        cptwithmodi = cptwithmodi + "~" + splitCPT[g];
                //                    }
                //                }
                //                ProcList.RemoveAt(z);
                //                ProcList.Add(cptwithmodi);
                //            }
                //        }
                //    }
                //}


                //  string ModifierLookup = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\", "Vaccine_Procedure_Mapping.xml");

                eandmCode.ProcList = new List<string>();
                for (int i = 0; i < ProcList.Count; i++)
                {
                    eandmCode.ProcList.Add(ProcList[i]);
                }
                eandmCode.ICDList = new List<string>();
                for (int j = 0; j < ICDList.Count; j++)
                {
                    ICdlist.Add(ICDList[j].Split('~')[1]);
                    eandmCode.ICDList.Add(ICDList[j]);
                }
                if (!EMPriICDSet)
                    bSaveEnable = true;

                WFObjectManager WFObjMngr = new WFObjectManager();
                WFObject WFObjBilling = WFObjMngr.GetByObjectSystemIdAnfObjType(ulEncID, "BILLING");
                eandmCode.BillingWFObjCurrentProcess = WFObjBilling.Current_Process;

                iMySession.Close();
            }
            return eandmCode;
        }

        //public IList<string> CPTModifier(string[] lstcpt)
        //{
        //    IList<string> CPTAdminVaccList = new List<string>();
        //    XmlDocument xmldocModifier = new XmlDocument();
        //    xmldocModifier.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "Vaccine_Procedure_Mapping" + ".xml");
        //    XmlNodeList xmlNodeModifierList = xmldocModifier.GetElementsByTagName("Vaccine_Procedure_Mapping");
        //    CPTAdminVaccList = xmlNodeModifierList.Cast<XmlNode>().Select(a => a.Attributes["Admin_Procedure_Code"].Value.ToString() + "|" + a.Attributes["Vaccine_Procedure_Code"].Value.ToString() + "|" + a.Attributes["Modifier"].Value.ToString()).ToList<string>();

        //    IList<string> mpdoproc = new List<string>();
        //    IList<string> lstAdmincpt = new List<string>();

        //    foreach (string AdminCPT in lstcpt)
        //    {
        //        if (CPTAdminVaccList.Select(a => a.Split('|')[0]).Contains(AdminCPT))
        //        {

        //            lstAdmincpt.Add(AdminCPT);
        //        }
        //        //number is the current item in the loop
        //    }
        //    foreach (string temp in lstAdmincpt)
        //    {
        //        for (int k = 0; k < CPTAdminVaccList.Count; k++)
        //        {
        //            if (CPTAdminVaccList[k].IndexOf(temp) >= 0)
        //            {
        //                string vacc = CPTAdminVaccList[k].Split('|')[1];
        //                string[] vacccpt = vacc.Split(',');
        //                foreach (string tempnew in vacccpt)
        //                {
        //                    if (lstcpt.Contains(tempnew))
        //                    {
        //                        mpdoproc.Add(temp + "~" + CPTAdminVaccList[k].Split('|')[2]);
        //                    }
        //                }

        //            }
        //        }
        //        //number is the current item in the loop
        //    }

        //    return mpdoproc;
        //}
        //public IList<string> CPTModifierDelete(string[] lstcpt)
        //{
        //    IList<string> CPTAdminVaccList = new List<string>();
        //    XmlDocument xmldocModifier = new XmlDocument();
        //    xmldocModifier.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "Vaccine_Procedure_Mapping" + ".xml");
        //    XmlNodeList xmlNodeModifierList = xmldocModifier.GetElementsByTagName("Vaccine_Procedure_Mapping");
        //    CPTAdminVaccList = xmlNodeModifierList.Cast<XmlNode>().Select(a => a.Attributes["Admin_Procedure_Code"].Value.ToString() + "|" + a.Attributes["Vaccine_Procedure_Code"].Value.ToString() + "|" + a.Attributes["Modifier"].Value.ToString()).ToList<string>();

        //    IList<string> mpdoproc = new List<string>();
        //    IList<string> lstAdmincpt = new List<string>();

        //    foreach (string AdminCPT in lstcpt)
        //    {
        //        if (CPTAdminVaccList.Select(a => a.Split('|')[0]).Contains(AdminCPT))
        //        {

        //            lstAdmincpt.Add(AdminCPT);
        //        }
        //        //number is the current item in the loop
        //    }
        //    foreach (string temp in lstAdmincpt)
        //    {
        //       IList<string> mpdoproctemp = new List<string>();
        //       string modifier = "";
        //        for (int k = 0; k < CPTAdminVaccList.Count; k++)
        //        {
        //            modifier = CPTAdminVaccList[k].Split('|')[2];
        //            if (CPTAdminVaccList[k].IndexOf(temp) >= 0)
        //            {
        //                string vacc = CPTAdminVaccList[k].Split('|')[1];
        //                string[] vacccpt = vacc.Split(',');
        //                foreach (string tempnew in vacccpt)
        //                {
        //                    if (lstcpt.Contains(tempnew))
        //                    {
        //                        mpdoproctemp.Add(temp + "~" + CPTAdminVaccList[k].Split('|')[2]);

        //                    }
        //                }

        //            }
        //        }
        //        if (mpdoproctemp.Count == 0)
        //        {
        //            mpdoproc.Add(temp + "~" + modifier);
        //        }
        //        //number is the current item in the loop
        //    }


        //    return mpdoproc;
        //}
        public FillEandMCoding SaveUpdateEandMCoding(IList<EAndMCoding> SaveListCPT, IList<EAndMCoding> UpdateListCPT, IList<EandMCodingICD> SaveListICD, string UserName, DateTime DateAndTime, IList<EandMCodingICD> UpdateListICD, Encounter EnRecord, string sBillingInstruction, IList<Immunization> ImmDelList, IList<ImmunizationHistory> ImmHisDelList, CarePlan CareplanUpdate, IList<EAndMCoding> lsteandmDeletelist)
        {
            iTryCount = 0;
            FillEandMCoding DTO = new FillEandMCoding();
            IList<EAndMCoding> CombinedSaveUpdateList = new List<EAndMCoding>();
            //IList<EandMCodingICD> SaveEMICDList = new List<EandMCodingICD>();
            //IList<EandMCodingICD> UpdateEMICDList = new List<EandMCodingICD>();
            //IList<EandMCodingICD> DeleteEMICDList = new List<EandMCodingICD>();
            IList<Encounter> ilstUpdateList = new List<Encounter>();
            IList<int> lstSaveOrder = new List<int>();
            IList<int> lstUpdateOrder = new List<int>();
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XMLObjHuman = new GenerateXml();

            ulong EncounterOrHumanId = 0;
            ulong humanid = 0;
            bool bEandMCodingConsistent = true, bEandMCodingICDConsistent = true, bEandMCodingICDafterDeleteConsistent = true, bEncounterConsistent = true;
            string FileNames;
            string strXmlFilePaths;

            //Jira #CAP-107
            //New code - start
            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            IList<TreatmentPlan> delTplanlst = new List<TreatmentPlan>();

            if (ImmDelList != null && ImmDelList.Count > 0)
            {
                ulong Enc_ID = ImmDelList[0].Encounter_Id;

                //BugID:46789
                #region TPlanGET
                //string FileName = "Encounter" + "_" + Enc_ID + ".xml";
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                IList<string> ilstGeneralPlanTagList = new List<string>();
                ilstGeneralPlanTagList.Add("TreatmentPlanList");


                IList<object> ilstGeneralPlanBlobFinal = new List<object>();
                ilstGeneralPlanBlobFinal = ReadBlob(Enc_ID, ilstGeneralPlanTagList);
                if (ilstGeneralPlanBlobFinal != null && ilstGeneralPlanBlobFinal.Count > 0)
                {
                    if (ilstGeneralPlanBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstGeneralPlanBlobFinal[0]).Count; iCount++)
                        {
                            objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount]);
                        }
                    }
                }
            }
        //New code - End

        TryAgain1:
            int iResult = 0;

            ISession MySession = Session.GetISession();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        if (SaveListCPT.Count == 0)
                        {
                            SaveListCPT = null;
                        }
                        //if (SaveListCPT != null)
                        //{
                        //    for (int i = 0; i < SaveListCPT.Count; i++)
                        //    {
                        //        lstSaveOrder.Add(SaveListCPT[i].CPT_Order);
                        //    }
                        //}
                        //if (UpdateListCPT != null)
                        //{
                        //    for (int i = 0; i < UpdateListCPT.Count; i++)
                        //    {
                        //        lstUpdateOrder.Add(UpdateListCPT[i].CPT_Order);
                        //    }
                        //}
                        //iResult = SaveUpdateDeleteWithoutTransaction(ref SaveListCPT, UpdateListCPT, DeleteCPTList, MySession, string.Empty);
                        if (SaveListCPT != null && SaveListCPT.Count > 0)
                        {
                            EncounterOrHumanId = SaveListCPT[0].Encounter_ID;
                            humanid = SaveListCPT[0].Human_ID;
                        }
                        else
                            if (UpdateListCPT != null && UpdateListCPT.Count > 0)
                        {
                            EncounterOrHumanId = UpdateListCPT[0].Encounter_ID;
                            humanid = UpdateListCPT[0].Human_ID;
                        }
                        iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveListCPT, ref UpdateListCPT, lsteandmDeletelist, MySession, string.Empty, true, true, EncounterOrHumanId, string.Empty, ref XMLObj);

                        if (iResult == 2)
                        {
                            if (iTryCount < 5)
                            {
                                iTryCount++;
                            }
                            else
                            {
                                trans.Rollback();
                                throw new Exception("Deadlock is occured. Transaction failed");
                            }
                        }
                        else if (iResult == 1)
                        {
                            trans.Rollback();
                            throw new Exception("Exception is occured. Transaction failed");
                        }
                        IList<EAndMCoding> ComblstConsisChk = new List<EAndMCoding>();
                        if (SaveListCPT != null)
                        {
                            foreach (EAndMCoding obj in SaveListCPT)
                            {
                                ComblstConsisChk.Add(obj);
                            }
                        }
                        if (UpdateListCPT != null)
                        {
                            foreach (EAndMCoding objUpd in UpdateListCPT)
                            {
                                ComblstConsisChk.Add(objUpd);
                            }
                        }
                        bEandMCodingConsistent = XMLObj.CheckDataConsistency(ComblstConsisChk.Cast<object>().ToList(), true, string.Empty);




                        //if (SaveListCPT != null)
                        //{
                        //    for (int i = 0; i < SaveListCPT.Count; i++)
                        //    {
                        //        if (SaveListCPT[i].Is_Delete == "N")
                        //        {
                        //            SaveListCPT[i].CPT_Order = lstSaveOrder[i];
                        //            CombinedSaveUpdateList.Add(SaveListCPT[i]);
                        //        }
                        //    }
                        //}
                        //if (UpdateListCPT != null)
                        //{
                        //    for (int i = 0; i < UpdateListCPT.Count; i++)
                        //    {
                        //        if (UpdateListCPT[i].Is_Delete == "N")
                        //        {
                        //            UpdateListCPT[i].CPT_Order = lstUpdateOrder[i];
                        //            CombinedSaveUpdateList.Add(UpdateListCPT[i]);
                        //        }
                        //    }
                        //}



                        EandMCodingICDManager eandmicdMngr = new EandMCodingICDManager();
                        EandMCodingICD objEandMCodingICD = null;

                        //if (CombinedSaveUpdateList.Count > 0)
                        //{
                        //    for (int iCPT = 0; iCPT < CombinedSaveUpdateList.Count; iCPT++) //Combined Save/Upadate CPT List
                        //    {
                        //        if (arylstICD.Length > 0)
                        //        {
                        //            for (int iICD = 0; iICD < arylstICD.Length; iICD++) // Checked ICD List from Grid
                        //            {
                        //                string[] aryICD = arylstICD[iICD].ToString().Split('~');
                        //                for (int iSelect = 3; iSelect < 9; iSelect++)
                        //                {
                        //                    if (aryICD[iSelect].ToString() != string.Empty && aryICD[iSelect].ToString() == CombinedSaveUpdateList[iCPT].Sequence.ToString()) //Check Whether Save ICD List
                        //                    {
                        //                        string[] sEMICDIds = new string[aryICD[9].ToString().Split(',').Length];
                        //                        for (int iLen = 0; iLen < aryICD[9].ToString().Split(',').Length; iLen++)
                        //                        {
                        //                            sEMICDIds[iLen] = aryICD[9].Split(',')[iLen];
                        //                        }

                        //                        if (sEMICDIds.Contains(CombinedSaveUpdateList[iCPT].Id.ToString()) == false)
                        //                        {
                        //                            objEandMCodingICD = new EandMCodingICD();
                        //                            objEandMCodingICD.E_M_Coding_ID = CombinedSaveUpdateList[iCPT].Id;
                        //                            objEandMCodingICD.Encounter_ID = CombinedSaveUpdateList[iCPT].Encounter_ID;
                        //                            objEandMCodingICD.Human_ID = CombinedSaveUpdateList[iCPT].Human_ID;
                        //                            objEandMCodingICD.ICD = aryICD[0];
                        //                            objEandMCodingICD.ICD_Description = aryICD[1];
                        //                            objEandMCodingICD.Sequence = CombinedSaveUpdateList[iCPT].Sequence;
                        //                            objEandMCodingICD.Source = aryICD[11];//BugID:48668 -- ServProc REVAMP
                        //                            if (aryICD[2] == "Y")
                        //                            {
                        //                                objEandMCodingICD.ICD_Category = "Primary";
                        //                            }
                        //                            else
                        //                            {
                        //                                objEandMCodingICD.ICD_Category = "None";
                        //                            }
                        //                            objEandMCodingICD.ICD_Type = "";
                        //                            objEandMCodingICD.Is_Delete = "N";
                        //                            objEandMCodingICD.Created_By = CombinedSaveUpdateList[iCPT].Created_By;
                        //                            objEandMCodingICD.Created_Date_And_Time = CombinedSaveUpdateList[iCPT].Created_Date_And_Time;
                        //                            SaveEMICDList.Add(objEandMCodingICD); // SaveICD List
                        //                        }
                        //                        else
                        //                        {
                        //                            var eandmICDList1 = from eandmICD in eandmICDList where eandmICD.E_M_Coding_ID == CombinedSaveUpdateList[iCPT].Id && eandmICD.ICD == aryICD[0].ToString() select eandmICD;
                        //                            IList<EandMCodingICD> tempICDlist = eandmICDList1.ToList<EandMCodingICD>();
                        //                            objEandMCodingICD = new EandMCodingICD();
                        //                            if (tempICDlist.Count > 0)
                        //                            {
                        //                                objEandMCodingICD = tempICDlist[0];
                        //                                if (aryICD[2] == "Y")
                        //                                {
                        //                                    objEandMCodingICD.ICD_Category = "Primary";
                        //                                }
                        //                                else
                        //                                {
                        //                                    objEandMCodingICD.ICD_Category = "None";
                        //                                }
                        //                                objEandMCodingICD.Source = aryICD[11];//BugID:48668 -- ServProc REVAMP
                        //                                objEandMCodingICD.Sequence = CombinedSaveUpdateList[iCPT].Sequence;
                        //                                objEandMCodingICD.Human_ID = CombinedSaveUpdateList[iCPT].Human_ID;
                        //                                objEandMCodingICD.Modified_By = UserName;
                        //                                objEandMCodingICD.Modified_Date_And_Time = DateAndTime;
                        //                                UpdateEMICDList.Add(objEandMCodingICD); //UpdateICD List
                        //                            }
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        //IList<EandMCodingICD> updatedEMICDBeforeSaveUpdate = new List<EandMCodingICD>();
                        //if (SaveEMICDList.Count == 0)
                        //{
                        //    SaveEMICDList = null;
                        //}
                        //if ((SaveEMICDList != null && SaveEMICDList.Count > 0) || (UpdateEMICDList != null && UpdateEMICDList.Count > 0))
                        //{
                        //    if (SaveEMICDList != null && SaveEMICDList.Count > 0)
                        //        EncounterOrHumanId = SaveEMICDList[0].Encounter_ID;
                        //    else
                        //        if (UpdateEMICDList != null && UpdateEMICDList.Count > 0)
                        //            EncounterOrHumanId = UpdateEMICDList[0].Encounter_ID;


                        iResult = eandmicdMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveListICD, ref UpdateListICD, null, MySession, string.Empty, true, true, EncounterOrHumanId, string.Empty, ref XMLObj);
                        //iResult = eandmicdMngr.SaveUpdateDeleteWithoutTransaction(ref SaveEMICDList, UpdateEMICDList, null, MySession, string.Empty);

                        if (iResult == 2)
                        {
                            if (iTryCount < 5)
                            {
                                iTryCount++;
                            }
                            else
                            {
                                trans.Rollback();
                                throw new Exception("Deadlock is occured. Transaction failed");
                            }
                        }
                        else if (iResult == 1)
                        {
                            trans.Rollback();
                            throw new Exception("Exception is occured. Transaction failed");
                        }
                        IList<EandMCodingICD> CombICDConsischk = new List<EandMCodingICD>();
                        if (SaveListICD != null)
                        {
                            foreach (EandMCodingICD obj in SaveListICD)
                            {
                                CombICDConsischk.Add(obj);
                            }
                        }
                        if (UpdateListICD != null)
                        {
                            foreach (EandMCodingICD objUpd in UpdateListICD)
                            {
                                CombICDConsischk.Add(objUpd);
                            }
                        }

                        bEandMCodingICDConsistent = XMLObj.CheckDataConsistency(CombICDConsischk.Cast<object>().ToList(), true, string.Empty);
                        //}


                        //Inorder to find DeleteICD List(Comparing Existing List with Previous List which is saved in Session)
                        //IList<EandMCodingICD> CombinedEMICDList = new List<EandMCodingICD>();
                        //if (UpdateListICD != null)
                        //{
                        //    foreach (EandMCodingICD emicd in UpdateListICD)
                        //    {
                        //        updatedEMICDBeforeSaveUpdate.Add(emicd);
                        //    }
                        //}

                        //if (SaveListICD != null)
                        //{
                        //    for (int i = 0; i < SaveListICD.Count; i++)
                        //    {
                        //        CombinedEMICDList.Add(SaveListICD[i]);
                        //    }
                        //}
                        //if (UpdateListICD != null)
                        //{
                        //    for (int i = 0; i < UpdateListICD.Count; i++)
                        //    {
                        //        CombinedEMICDList.Add(UpdateListICD[i]);
                        //    }
                        //}
                        //if (updatedEMICDBeforeSaveUpdate != null)
                        //{
                        //    for (int i = 0; i < updatedEMICDBeforeSaveUpdate.Count; i++)
                        //    {
                        //        updatedEMICDBeforeSaveUpdate[i].Version = updatedEMICDBeforeSaveUpdate[i].Version - 1;
                        //        CombinedEMICDList.Add(updatedEMICDBeforeSaveUpdate[i]);
                        //    }
                        //}

                        //foreach (EandMCodingICD eandmobj in eandmICDList)
                        //{
                        //    eandmobj.Version = eandmobj.Version + 1;
                        //}

                        //if (eandmICDList.Count > 0)
                        //{
                        //    DeleteEMICDList = eandmICDList.Except(CombinedEMICDList).ToList<EandMCodingICD>();

                        //}
                        //foreach (EandMCodingICD obj in DeleteEMICDList)
                        //{
                        //    obj.ICD_Category = "None";
                        //    obj.Is_Delete = "Y";
                        //    obj.Version = obj.Version - 1;
                        //}


                        //if (DeleteEMICDList.Count > 0)
                        //{
                        //    IList<EandMCodingICD> SaveEandMICDList = new List<EandMCodingICD>();

                        //    IList<EandMCodingICD> UpdateEandMICDList = DeleteEMICDList;

                        //    EandMCodingICDManager eandmcodingmanager = new EandMCodingICDManager();
                        //    iResult = eandmcodingmanager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveEandMICDList, ref UpdateEandMICDList, null, MySession, string.Empty, true, true, DeleteEMICDList[0].Encounter_ID, string.Empty, ref XMLObj);
                        //    //iResult = eandmicdMngr.SaveUpdateDeleteWithoutTransaction(ref SaveEandMICDList, null, DeleteEMICDList, MySession, string.Empty);
                        //    if (iResult == 2)
                        //    {
                        //        if (iTryCount < 5)
                        //        {
                        //            iTryCount++;
                        //        }
                        //        else
                        //        {
                        //            trans.Rollback();
                        //            throw new Exception("Deadlock is occured. Transaction failed");
                        //        }
                        //    }
                        //    else if (iResult == 1)
                        //    {
                        //        trans.Rollback();
                        //        throw new Exception("Exception is occured. Transaction failed");
                        //    }
                        //    IList<EandMCodingICD> CombICDConsisDelchk = new List<EandMCodingICD>();
                        //    if (SaveEandMICDList != null)
                        //    {
                        //        foreach (EandMCodingICD obj in SaveEandMICDList)
                        //        {
                        //            CombICDConsisDelchk.Add(obj);
                        //        }
                        //    }
                        //    if (UpdateEandMICDList != null)
                        //    {
                        //        foreach (EandMCodingICD objUpd in UpdateEandMICDList)
                        //        {
                        //            CombICDConsisDelchk.Add(objUpd);
                        //        }
                        //    }

                        //    bEandMCodingICDafterDeleteConsistent = XMLObj.CheckDataConsistency(CombICDConsisDelchk.Cast<object>().ToList(), true, string.Empty);

                        //}
                        if (ImmDelList != null && ImmDelList.Count > 0)
                        {
                            ulong Enc_ID = ImmDelList[0].Encounter_Id;
                            //Jira #CAP-107

                            //Old-Code - Start

                            //IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
                            //IList<TreatmentPlan> delTplanlst = new List<TreatmentPlan>();
                            ////BugID:46789
                            //#region TPlanGET
                            ////string FileName = "Encounter" + "_" + Enc_ID + ".xml";
                            ////string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                            //IList<string> ilstGeneralPlanTagList = new List<string>();
                            //ilstGeneralPlanTagList.Add("TreatmentPlanList");


                            //IList<object> ilstGeneralPlanBlobFinal = new List<object>();
                            //ilstGeneralPlanBlobFinal = ReadBlob(Enc_ID, ilstGeneralPlanTagList);
                            //if (ilstGeneralPlanBlobFinal != null && ilstGeneralPlanBlobFinal.Count > 0)
                            //{
                            //    if (ilstGeneralPlanBlobFinal[0] != null)
                            //    {
                            //        for (int iCount = 0; iCount < ((IList<object>)ilstGeneralPlanBlobFinal[0]).Count; iCount++)
                            //        {
                            //            objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount]);
                            //        }
                            //    }
                            //}
                            //Old - Code - Start

                            //  XmlTextReader XmlText = null;
                            //try
                            //{
                            //    if (File.Exists(strXmlFilePath) == true)
                            //    {
                            //        XmlDocument itemDoc = new XmlDocument();
                            //        XmlText = new XmlTextReader(strXmlFilePath);
                            //        XmlNodeList xmlTagName = null;
                            //        // itemDoc.Load(XmlText);
                            //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //        {
                            //            itemDoc.Load(fs);

                            //            XmlText.Close();
                            //            #region Treatment_plan
                            //            if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
                            //            {
                            //                xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

                            //                if (xmlTagName.Count > 0)
                            //                {
                            //                    for (int j = 0; j < xmlTagName.Count; j++)
                            //                    {
                            //                        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == Enc_ID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("IMMUNIZATION/INJECTION"))
                            //                        {

                            //                            string TagName = xmlTagName[j].Name;
                            //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
                            //                            TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
                            //                            IEnumerable<PropertyInfo> propInfo = null;
                            //                            propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

                            //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                            //                            {

                            //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
                            //                                {
                            //                                    foreach (PropertyInfo property in propInfo)
                            //                                    {
                            //                                        if (property.Name == nodevalue.Name)
                            //                                        {
                            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
                            //                                                property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
                            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
                            //                                                property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
                            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                            //                                                property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
                            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
                            //                                                property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
                            //                                            else
                            //                                                property.SetValue(TreatmentPlan, nodevalue.Value, null);
                            //                                        }
                            //                                    }
                            //                                }

                            //                            }
                            //                            objTreatmentPlan.Add(TreatmentPlan);
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //            #endregion
                            //            fs.Close();
                            //            fs.Dispose();
                            //        }
                            //    }
                            //}
                            //catch (Exception Ex)
                            //{
                            //    if (XmlText != null)
                            //        XmlText.Close();
                            //    throw Ex;

                            //}
                            if (Enc_ID != 0)
                            {
                                delTplanlst = (from oj in objTreatmentPlan where ImmDelList.Any(a => a.Id == oj.Source_ID) select oj).ToList<TreatmentPlan>();
                            }
                            #endregion

                            IList<Immunization> ImmSavelst = new List<Immunization>();
                            IList<Immunization> ImmUpdatelst = new List<Immunization>();
                            IList<Immunization> ImmDeletelst = ImmDelList;
                            ImmunizationManager ImmManager = new ImmunizationManager();
                            iResult = ImmManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ImmSavelst, ref ImmUpdatelst, ImmDeletelst, MySession, string.Empty, true, false, ImmDeletelst[0].Human_ID, string.Empty, ref XMLObjHuman);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                }
                                else
                                {
                                    trans.Rollback();
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception is occured. Transaction failed");
                            }

                            //BugID:46789
                            if (delTplanlst != null && delTplanlst.Count > 0)
                            {
                                IList<TreatmentPlan> TplanSavelst = new List<TreatmentPlan>();
                                IList<TreatmentPlan> TplanUpdatelst = new List<TreatmentPlan>();
                                IList<TreatmentPlan> TplanDeletelst = delTplanlst;
                                TreatmentPlanManager TplanManager = new TreatmentPlanManager();
                                iResult = TplanManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref TplanSavelst, ref TplanUpdatelst, TplanDeletelst, MySession, string.Empty, true, true, Enc_ID, string.Empty, ref XMLObj);
                                if (iResult == 2)
                                {
                                    if (iTryCount < 5)
                                    {
                                        iTryCount++;
                                    }
                                    else
                                    {
                                        trans.Rollback();
                                        throw new Exception("Deadlock is occured. Transaction failed");
                                    }
                                }
                                else if (iResult == 1)
                                {
                                    trans.Rollback();
                                    throw new Exception("Exception is occured. Transaction failed");
                                }
                            }

                        }
                        if (ImmHisDelList != null && ImmHisDelList.Count > 0 && ImmHisDelList[0].Immunization_History_Master_ID != 0)
                        {
                            IList<ImmunizationHistory> ImmHisSavelst = new List<ImmunizationHistory>();
                            IList<ImmunizationHistory> ImmHisUpdatelst = new List<ImmunizationHistory>();
                            IList<ImmunizationHistory> ImmHisDeletelst = ImmHisDelList;

                            ImmunizationHistoryManager ImmManager = new ImmunizationHistoryManager();
                            ImmunizationMasterHistoryManager obj = new ImmunizationMasterHistoryManager();

                            IList<ImmunizationMasterHistory> lst = new List<ImmunizationMasterHistory>();
                            IList<ImmunizationMasterHistory> lstsave = new List<ImmunizationMasterHistory>();

                            lst = obj.GetImmunizationDeteailsbyID(ImmHisDelList[0].Immunization_History_Master_ID);
                            if (lst != null && lst.Count > 0)
                            {
                                lst[0].Is_Deleted = "Y";
                                lst[0].Modified_Date_And_Time = System.DateTime.Now;
                                lst[0].Modified_Date_And_Time = System.DateTime.Now;

                            }


                            iResult = obj.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref lstsave, ref lst, null, MySession, string.Empty, true, true, lst[0].Human_ID, string.Empty, ref XMLObjHuman);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                }
                                else
                                {
                                    trans.Rollback();
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception is occured. Transaction failed");
                            }

                            iResult = ImmManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ImmHisSavelst, ref ImmHisUpdatelst, ImmHisDelList, MySession, string.Empty, true, false, ImmHisDeletelst[0].Human_ID, string.Empty, ref XMLObjHuman);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                }
                                else
                                {
                                    trans.Rollback();
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception is occured. Transaction failed");
                            }
                        }
                        if (CareplanUpdate != null && CareplanUpdate.Id.ToString() != "0")//Updation of Care_plan on deletion of CPTs "99406","99407" - BugID:47386
                        {
                            IList<CarePlan> CplanSavelst = new List<CarePlan>();
                            IList<CarePlan> CplanUpdatelst = new List<CarePlan>();
                            IList<CarePlan> CplanDeletelst = new List<CarePlan>();
                            CplanUpdatelst.Add(CareplanUpdate);
                            CarePlanManager cplanmanager = new CarePlanManager();
                            iResult = cplanmanager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref CplanSavelst, ref CplanUpdatelst, CplanDeletelst, MySession, string.Empty, true, true, CplanUpdatelst[0].Encounter_ID, string.Empty, ref XMLObj);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                }
                                else
                                {
                                    trans.Rollback();
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception is occured. Transaction failed");
                            }
                        }
                        #region OLD ENCOUNTER UPDATE CODE
                        /*DTO.EncounterList = new List<Encounter>();
                        EncounterManager EncountMngr = new EncounterManager();
                        EnRecord.Billing_Instruction = sBillingInstruction;
                        ilstUpdateList.Add(EnRecord);
                        if (ilstUpdateList.Count > 0)
                        {
                            IList<Encounter> SaveList = null;
                            iResult = EncountMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveList, ref ilstUpdateList, null, MySession, string.Empty, true, true, ilstUpdateList[0].Id, string.Empty, ref XMLObj);
                            //  iResult = EncountMngr.SaveUpdateDeleteWithoutTransaction(ref SaveList, ilstUpdateList, null, MySession, string.Empty);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                }
                                else
                                {
                                    trans.Rollback();
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception is occured. Transaction failed");
                            }
                            IList<Encounter> CombEncConsischk = new List<Encounter>();
                            if (SaveList != null)
                            {
                                foreach (Encounter obj in SaveList)
                                {
                                    CombEncConsischk.Add(obj);
                                }
                            }
                            if (ilstUpdateList != null)
                            {
                                foreach (Encounter objUpd in ilstUpdateList)
                                {
                                    CombEncConsischk.Add(objUpd);
                                }
                            }

                            bEncounterConsistent = XMLObj.CheckDataConsistency(CombEncConsischk.Cast<object>().ToList(), true, string.Empty);
                        }*/
                        #endregion
                        #region ENCOUNTER UPDATE CODE

                        ilstUpdateList = new List<Encounter>();
                        ilstUpdateList.Add(EnRecord);//BugID:51613
                        if (ilstUpdateList.Count > 0)
                        {
                            EncounterManager EncountMngr = new EncounterManager();
                            ilstUpdateList[0].Billing_Instruction = sBillingInstruction;
                            IList<Encounter> SaveList = null;
                            iResult = EncountMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveList, ref ilstUpdateList, null, MySession, string.Empty, true, false, ilstUpdateList[0].Id, string.Empty, ref XMLObj);//BugID:44337

                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                }
                                else
                                {
                                    trans.Rollback();
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception is occured. Transaction failed");
                            }
                            IList<Encounter> CombEncConsischk = new List<Encounter>();
                            if (SaveList != null)
                            {
                                foreach (Encounter obj in SaveList)
                                {
                                    CombEncConsischk.Add(obj);
                                }
                            }
                            if (ilstUpdateList != null)
                            {
                                foreach (Encounter objUpd in ilstUpdateList)
                                {
                                    CombEncConsischk.Add(objUpd);
                                }
                            }

                            bEncounterConsistent = XMLObj.CheckDataConsistency(CombEncConsischk.Cast<object>().ToList(), true, string.Empty);
                        }
                        #endregion

                        if (bEandMCodingConsistent && bEandMCodingICDConsistent && bEandMCodingICDafterDeleteConsistent && bEncounterConsistent)
                        {
                            //if (XMLObj != null && XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                            if (XMLObj != null)
                            {
                                // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                int trycount = 0;
                            trytosaveagain:
                                try
                                {
                                    // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    WriteBlob(EncounterOrHumanId, XMLObj.itemDoc, MySession, SaveListCPT, UpdateListCPT, null, XMLObj, false);
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
                            // if (XMLObjHuman != null && XMLObjHuman.strXmlFilePath != null && XMLObjHuman.strXmlFilePath != "")
                            if (XMLObjHuman != null)
                            {
                                // XMLObjHuman.itemDoc.Save(XMLObjHuman.strXmlFilePath);
                                int trycount = 0;
                            trytosaveagain:
                                try
                                {
                                    ImmunizationHistoryManager objman = new ImmunizationHistoryManager();
                                    ImmunizationManager objmanimm = new ImmunizationManager();
                                    if ((ImmHisDelList != null && ImmHisDelList.Count > 0 && ImmHisDelList[0].Immunization_History_Master_ID != 0))
                                    {
                                        objman.WriteBlob(humanid, XMLObjHuman.itemDoc, MySession, null, null, ImmHisDelList, XMLObjHuman, false);
                                    }
                                    if (ImmDelList != null && ImmDelList.Count > 0)
                                    {
                                        objmanimm.WriteBlob(humanid, XMLObjHuman.itemDoc, MySession, null, null, ImmDelList, XMLObjHuman, false);
                                    }

                                    //XMLObjHuman.itemDoc.Save(XMLObjHuman.strXmlFilePath);
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
                            trans.Commit();
                        }
                        else
                            throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");

                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        MySession.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //bugid:x6modifier//WISH FIX .. BugID:45392
            //Load Data from XML
            #region
            //DTO.EandMCodingList = new List<EAndMCoding>();
            //DTO.EandMCodingICDList = new List<EandMCodingICD>();
            //if (SaveListCPT != null)
            //{
            //    for (int i = 0; i < SaveListCPT.Count; i++)
            //    {
            //        DTO.EandMCodingList.Add(SaveListCPT[i]);
            //    }
            //}
            //if (UpdateListCPT != null)
            //{
            //    for (int i = 0; i < UpdateListCPT.Count; i++)
            //    {
            //        //  UpdateListCPT[i].Version = UpdateListCPT[i].Version + 1;
            //        DTO.EandMCodingList.Add(UpdateListCPT[i]);
            //    }
            //}
            //if (SaveEMICDList != null)
            //{
            //    for (int i = 0; i < SaveEMICDList.Count; i++)
            //    {
            //        DTO.EandMCodingICDList.Add(SaveEMICDList[i]);
            //    }
            //}
            //if (UpdateEMICDList != null)
            //{
            //    for (int i = 0; i < UpdateEMICDList.Count; i++)
            //    {
            //        //  UpdateEMICDList[i].Version = UpdateEMICDList[i].Version + 1;
            //        DTO.EandMCodingICDList.Add(UpdateEMICDList[i]);
            //    }
            //}
            #endregion
            #region data from XML
            IList<string> ICDList = new List<string>();
            //ArrayList aryAccessCPT = new ArrayList();
            IList<string> TempProcedureList = new List<string>();
            IList<string> ICdlist = new List<string>();
            ArrayList aryAccessICD = new ArrayList();
            bool IsPrimaryChecked = false;
            string sAssesmentPrimary = string.Empty;


            IList<string> ilstEandMTagList = new List<string>();
            ilstEandMTagList.Add("EandMCodingICDList");

            ilstEandMTagList.Add("EAndMCodingList");
            ilstEandMTagList.Add("EncounterList");



            IList<EandMCodingICD> lsticd = new List<EandMCodingICD>();
            IList<EAndMCoding> lsteandm = new List<EAndMCoding>();
            IList<Assessment> lstass = new List<Assessment>();
            IList<object> ilstEandMBlobFinal = new List<object>();

            ilstEandMBlobFinal = ReadBlob(EncounterOrHumanId, ilstEandMTagList);

            if (ilstEandMBlobFinal != null && ilstEandMBlobFinal.Count > 0)
            {
                if (ilstEandMBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstEandMBlobFinal[0]).Count; iCount++)
                    {
                        lsticd.Add((EandMCodingICD)((IList<object>)ilstEandMBlobFinal[0])[iCount]);
                    }
                    for (int i = 0; i < lsticd.Count; i++)
                    {
                        if (lsticd[i].Encounter_ID == EncounterOrHumanId && (lsticd[i].Is_Delete == "N" || lsticd[i].Is_Delete == ""))
                        {
                            DTO.EandMCodingICDList.Add(lsticd[i]);
                        }
                    }
                    if (DTO.EandMCodingICDList != null)
                    {
                        DTO.EandMCodingICDList = DTO.EandMCodingICDList.OrderBy(a => a.Sequence).ToList();
                        for (int j = 0; j < DTO.EandMCodingICDList.Count; j++)
                        {
                            string sPrimary = string.Empty;
                            if (DTO.EandMCodingICDList[j].ICD_Category == "Primary")
                            {
                                sPrimary = "Y";
                                IsPrimaryChecked = true;
                            }

                            var eandMICDList = from eandmICD in DTO.EandMCodingICDList where eandmICD.ICD == DTO.EandMCodingICDList[j].ICD select eandmICD;
                            IList<EandMCodingICD> templist = eandMICDList.ToList<EandMCodingICD>();
                            string sIDList = string.Empty;
                            string sICDVersion = string.Empty;
                            for (int k = 0; k < templist.Count; k++)
                            {
                                if (k == 0)
                                {
                                    sIDList = templist[k].Id.ToString();
                                    sICDVersion = templist[k].Version.ToString();
                                }
                                else
                                {
                                    sIDList += "," + templist[k].Id.ToString();
                                    sICDVersion = "," + templist[k].Version.ToString();
                                }
                            }

                            if (!ICDList.Any(a => a.Split('~')[1] == DTO.EandMCodingICDList[j].ICD))
                                ICDList.Add("ASSESSMENT" + "~" + DTO.EandMCodingICDList[j].ICD + "~" + DTO.EandMCodingICDList[j].ICD_Description + "~" + sICDVersion + "~" + sPrimary + "~" + sIDList + "~" + DTO.EandMCodingICDList[j].Sequence + "~" + ",");
                            else
                            {
                                IList<string> sReomveICDDesc = ICDList.Select(a => a.Split('~')[1]).ToList();
                                int iIndex = sReomveICDDesc.IndexOf(DTO.EandMCodingICDList[j].ICD);

                            }
                            aryAccessICD.Add(DTO.EandMCodingICDList[j].ICD);
                        }
                    }
                }



                if (ilstEandMBlobFinal[1] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstEandMBlobFinal[1]).Count; iCount++)
                    {
                        lsteandm.Add((EAndMCoding)((IList<object>)ilstEandMBlobFinal[1])[iCount]);
                    }
                    for (int i = 0; i < lsteandm.Count; i++)
                    {
                        IList<string> ProcList = new List<string>();
                        if (lsteandm[i].Encounter_ID == EncounterOrHumanId && (lsteandm[i].Is_Delete == "N" || lsteandm[i].Is_Delete == ""))
                        {
                            //#region setCPTOrder
                            //if (CombinedSaveUpdateList != null && CombinedSaveUpdateList.Count > 0)
                            //{
                            //    IList<EAndMCoding> cptlst = CombinedSaveUpdateList.Where(a => a.Id == EAndMCoding.Id).ToList<EAndMCoding>();
                            //    if (cptlst != null && cptlst.Count > 0)
                            //        EAndMCoding.CPT_Order = cptlst[0].CPT_Order;
                            //}
                            //#endregion

                            DTO.EandMCodingList.Add(lsteandm[i]);
                            ProcList.Add(lsteandm[i].Procedure_Code + "~" + lsteandm[i].Procedure_Code_Description + "~" + lsteandm[i].Id + "~" + lsteandm[i].Units + "~" + lsteandm[i].Modifier1 + "~" + lsteandm[i].Modifier2 + "~" + lsteandm[i].Modifier3 + "~" + lsteandm[i].Modifier4 + "~" + string.Empty + "~" + lsteandm[i].Version + "~" + lsteandm[i].Diagnosis_Pointer_1 + "~" + lsteandm[i].Diagnosis_Pointer_2 + "~" + lsteandm[i].Diagnosis_Pointer_3 + "~" + lsteandm[i].Diagnosis_Pointer_4 + "~" + lsteandm[i].Diagnosis_Pointer_5 + "~" + lsteandm[i].Diagnosis_Pointer_6);

                        }

                    }

                }




            }





            //FileNames = "Encounter" + "_" + EncounterOrHumanId + ".xml";
            //strXmlFilePaths = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileNames);
            //XmlTextReader XmlText1 = null;
            //try
            //{
            //    if (File.Exists(strXmlFilePaths) == true)
            //    {
            //        XmlDocument itemDoc = new XmlDocument();
            //        XmlText1 = new XmlTextReader(strXmlFilePaths);
            //        XmlNodeList xmlTagName = null;
            //        //  itemDoc.Load(XmlText);
            //        using (FileStream fs = new FileStream(strXmlFilePaths, FileMode.Open, FileAccess.Read, FileShare.Read))
            //        {
            //            itemDoc.Load(fs);

            //            XmlText1.Close();

            //            #region EandMCodingICD
            //            DTO.EandMCodingICDList = new List<EandMCodingICD>();
            //            if (itemDoc.GetElementsByTagName("EandMCodingICDList") != null)
            //            {
            //                if (itemDoc.GetElementsByTagName("EandMCodingICDList").Count > 0)
            //                {
            //                    xmlTagName = itemDoc.GetElementsByTagName("EandMCodingICDList")[0].ChildNodes;
            //                    if (xmlTagName.Count > 0)
            //                    {
            //                        for (int j = 0; j < xmlTagName.Count; j++)
            //                        {
            //                            string TagName = xmlTagName[j].Name;
            //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(EandMCodingICD));
            //                            EandMCodingICD EandMCodingICD = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as EandMCodingICD;
            //                            IEnumerable<PropertyInfo> propInfo = null;
            //                            if (EandMCodingICD != null)
            //                            {
            //                                propInfo = from obji in ((EandMCodingICD)EandMCodingICD).GetType().GetProperties() select obji;

            //                                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                                {
            //                                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                                    {
            //                                        foreach (PropertyInfo property in propInfo)
            //                                        {
            //                                            if (property.Name == nodevalue.Name)
            //                                            {
            //                                                if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                    property.SetValue(EandMCodingICD, Convert.ToUInt64(nodevalue.Value), null);
            //                                                else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                    property.SetValue(EandMCodingICD, Convert.ToString(nodevalue.Value), null);
            //                                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                    property.SetValue(EandMCodingICD, Convert.ToDateTime(nodevalue.Value), null);
            //                                                else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                    property.SetValue(EandMCodingICD, Convert.ToInt32(nodevalue.Value), null);
            //                                                else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
            //                                                    property.SetValue(EandMCodingICD, Convert.ToDecimal(nodevalue.Value), null);
            //                                                else
            //                                                    property.SetValue(EandMCodingICD, nodevalue.Value, null);
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                                if (EandMCodingICD.Encounter_ID == EncounterOrHumanId && (EandMCodingICD.Is_Delete == "N" || EandMCodingICD.Is_Delete == ""))
            //                                {
            //                                    DTO.EandMCodingICDList.Add(EandMCodingICD);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            if (DTO.EandMCodingICDList != null)
            //            {
            //                DTO.EandMCodingICDList = DTO.EandMCodingICDList.OrderBy(a => a.Sequence).ToList();
            //                for (int j = 0; j < DTO.EandMCodingICDList.Count; j++)
            //                {
            //                    string sPrimary = string.Empty;
            //                    if (DTO.EandMCodingICDList[j].ICD_Category == "Primary")
            //                    {
            //                        sPrimary = "Y";
            //                        IsPrimaryChecked = true;
            //                    }

            //                    var eandMICDList = from eandmICD in DTO.EandMCodingICDList where eandmICD.ICD == DTO.EandMCodingICDList[j].ICD select eandmICD;
            //                    IList<EandMCodingICD> templist = eandMICDList.ToList<EandMCodingICD>();
            //                    string sIDList = string.Empty;
            //                    string sICDVersion = string.Empty;
            //                    for (int k = 0; k < templist.Count; k++)
            //                    {
            //                        if (k == 0)
            //                        {
            //                            sIDList = templist[k].Id.ToString();
            //                            sICDVersion = templist[k].Version.ToString();
            //                        }
            //                        else
            //                        {
            //                            sIDList += "," + templist[k].Id.ToString();
            //                            sICDVersion = "," + templist[k].Version.ToString();
            //                        }
            //                    }

            //                    if (!ICDList.Any(a => a.Split('~')[1] == DTO.EandMCodingICDList[j].ICD))
            //                        ICDList.Add("ASSESSMENT" + "~" + DTO.EandMCodingICDList[j].ICD + "~" + DTO.EandMCodingICDList[j].ICD_Description + "~" + sICDVersion + "~" + sPrimary + "~" + sIDList + "~" + DTO.EandMCodingICDList[j].Sequence + "~" + ",");
            //                    else
            //                    {
            //                        IList<string> sReomveICDDesc = ICDList.Select(a => a.Split('~')[1]).ToList();
            //                        int iIndex = sReomveICDDesc.IndexOf(DTO.EandMCodingICDList[j].ICD);
            //                        //if (iIndex > -1)
            //                        //    if (ICDList[iIndex].Split('~').Length == 8)
            //                        //    {
            //                        //        //added for 6th ICD Column Marking
            //                        //        if (ICDList[iIndex].Split('~')[ICDList[iIndex].Split('~').Length - 1].Split(',').Length < 6)
            //                        //        {
            //                        //            for (int i = 0; i < DTO.EandMCodingICDList[j].Sequence; i++)
            //                        //            {
            //                        //                if (DTO.EandMCodingICDList[j].Sequence - 1 == i)
            //                        //                    ICDList[iIndex] += DTO.EandMCodingICDList[j].Sequence;
            //                        //                else if (ICDList[iIndex].Split('~')[ICDList[iIndex].Split('~').Length - 1].Split(',').Length != DTO.EandMCodingICDList[j].Sequence)
            //                        //                    ICDList[iIndex] += ",";
            //                        //            }
            //                        //        }
            //                        //    }
            //                    }
            //                    aryAccessICD.Add(DTO.EandMCodingICDList[j].ICD);
            //                }
            //            }
            //            #endregion

            //            #region EAndMCodingList
            //            DTO.EandMCodingList = new List<EAndMCoding>();
            //            if (itemDoc.GetElementsByTagName("EAndMCodingList") != null)
            //            {
            //                if (itemDoc.GetElementsByTagName("EAndMCodingList").Count > 0)
            //                {
            //                    xmlTagName = itemDoc.GetElementsByTagName("EAndMCodingList")[0].ChildNodes;
            //                    if (xmlTagName.Count > 0)
            //                    {
            //                        for (int j = 0; j < xmlTagName.Count; j++)
            //                        {
            //                            string TagName = xmlTagName[j].Name;
            //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(EAndMCoding));
            //                            EAndMCoding EAndMCoding = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as EAndMCoding;
            //                            IEnumerable<PropertyInfo> propInfo = null;
            //                            if (EAndMCoding != null)
            //                            {
            //                                propInfo = from obji in ((EAndMCoding)EAndMCoding).GetType().GetProperties() select obji;

            //                                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                                {
            //                                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                                    {
            //                                        foreach (PropertyInfo property in propInfo)
            //                                        {
            //                                            if (property.Name == nodevalue.Name)
            //                                            {
            //                                                if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                    property.SetValue(EAndMCoding, Convert.ToUInt64(nodevalue.Value), null);
            //                                                else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                    property.SetValue(EAndMCoding, Convert.ToString(nodevalue.Value), null);
            //                                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                    property.SetValue(EAndMCoding, Convert.ToDateTime(nodevalue.Value), null);
            //                                                else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                    property.SetValue(EAndMCoding, Convert.ToInt32(nodevalue.Value), null);
            //                                                else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
            //                                                    property.SetValue(EAndMCoding, Convert.ToDecimal(nodevalue.Value), null);
            //                                                else
            //                                                    property.SetValue(EAndMCoding, nodevalue.Value, null);
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                                IList<string> ProcList = new List<string>();
            //                                if (EAndMCoding.Encounter_ID == EncounterOrHumanId && (EAndMCoding.Is_Delete == "N" || EAndMCoding.Is_Delete == ""))
            //                                {
            //                                    //#region setCPTOrder
            //                                    //if (CombinedSaveUpdateList != null && CombinedSaveUpdateList.Count > 0)
            //                                    //{
            //                                    //    IList<EAndMCoding> cptlst = CombinedSaveUpdateList.Where(a => a.Id == EAndMCoding.Id).ToList<EAndMCoding>();
            //                                    //    if (cptlst != null && cptlst.Count > 0)
            //                                    //        EAndMCoding.CPT_Order = cptlst[0].CPT_Order;
            //                                    //}
            //                                    //#endregion

            //                                    DTO.EandMCodingList.Add(EAndMCoding);
            //                                    ProcList.Add(EAndMCoding.Procedure_Code + "~" + EAndMCoding.Procedure_Code_Description + "~" + EAndMCoding.Id + "~" + EAndMCoding.Units + "~" + EAndMCoding.Modifier1 + "~" + EAndMCoding.Modifier2 + "~" + EAndMCoding.Modifier3 + "~" + EAndMCoding.Modifier4 + "~" + string.Empty + "~" + EAndMCoding.Version + "~" + EAndMCoding.Diagnosis_Pointer_1 + "~" + EAndMCoding.Diagnosis_Pointer_2 + "~" + EAndMCoding.Diagnosis_Pointer_3 + "~" + EAndMCoding.Diagnosis_Pointer_4 + "~" + EAndMCoding.Diagnosis_Pointer_5 + "~" + EAndMCoding.Diagnosis_Pointer_6);

            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            #endregion
            //            fs.Close();
            //            fs.Dispose();

            //        }
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    if (XmlText1 != null)
            //        XmlText1.Close();

            //    throw Ex;
            //}

            #endregion
            if (ilstUpdateList != null)
            {
                for (int i = 0; i < ilstUpdateList.Count; i++)
                {
                    DTO.EncounterList.Add(ilstUpdateList[i]);
                }
            }

            if (DTO.EncounterList != null)
            {
                if (DTO.EncounterList.Count == 0)
                    DTO.EncounterList.Add(EnRecord);
            }
            return DTO;
        }

        public IList<EAndMCoding> GetDetailsByEncounterID(ulong encid)
        {
            IList<EAndMCoding> listGetPQRI_Lookup = new List<EAndMCoding>();
            //using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            //{
            //    ICriteria critGetPQRI_LookupDetails = iMySession.CreateCriteria(typeof(PQRI_Data));//.Add(Expression.NotEqProperty("PQRI_Selection_XML", ""));
            //    listGetPQRI_Lookup = critGetPQRI_LookupDetails.List<PQRI_Data>().Where(a => a.PQRI_Selection_XML.Trim() != string.Empty && a.Sort_Order.ToString()=="3").ToList<PQRI_Data>();
            //    iMySession.Close();
            //}
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            ISQLQuery sqlquery = iMySession.CreateSQLQuery("select e.* from e_m_coding e where e.encounter_id ='" + encid + "'").AddEntity("e", typeof(EAndMCoding));
            listGetPQRI_Lookup = sqlquery.List<EAndMCoding>();
            return listGetPQRI_Lookup;
        }

        public FillEandMCoding SaveUpdateEandMCodingForPhoneEncounter(IList<EAndMCoding> SaveListCPT, IList<EAndMCoding> UpdateListCPT, IList<EandMCodingICD> eandmICDList, string UserName, DateTime DateAndTime, IList<EandMCodingICD> UpdateICDList)
        {
            iTryCount = 0;
            FillEandMCoding DTO = new FillEandMCoding();
            IList<EAndMCoding> CombinedSaveUpdateList = new List<EAndMCoding>();
            IList<EandMCodingICD> SaveEMICDList = new List<EandMCodingICD>();
            IList<EandMCodingICD> UpdateEMICDList = new List<EandMCodingICD>();
            IList<EandMCodingICD> DeleteEMICDList = new List<EandMCodingICD>();
            IList<Encounter> ilstUpdateList = new List<Encounter>();
            IList<int> lstSaveOrder = new List<int>();
            IList<int> lstUpdateOrder = new List<int>();
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XMLObjHuman = new GenerateXml();

            ulong EncounterOrHumanId = 0;
            bool bEandMCodingConsistent = true, bEandMCodingICDConsistent = true, bEandMCodingICDafterDeleteConsistent = true, bEncounterConsistent = true;
            int iResult = 0;

            ISession MySession = Session.GetISession();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        if (SaveListCPT.Count == 0)
                        {
                            SaveListCPT = null;
                        }
                        if (SaveListCPT != null && SaveListCPT.Count > 0)
                            EncounterOrHumanId = SaveListCPT[0].Encounter_ID;
                        else
                            if (UpdateListCPT != null && UpdateListCPT.Count > 0)
                            EncounterOrHumanId = UpdateListCPT[0].Encounter_ID;
                        iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveListCPT, ref UpdateListCPT, null, MySession, string.Empty, true, true, EncounterOrHumanId, string.Empty, ref XMLObj);

                        if (iResult == 2)
                        {
                            if (iTryCount < 5)
                            {
                                iTryCount++;
                            }
                            else
                            {
                                trans.Rollback();
                                throw new Exception("Deadlock is occured. Transaction failed");
                            }
                        }
                        else if (iResult == 1)
                        {
                            trans.Rollback();
                            throw new Exception("Exception is occured. Transaction failed");
                        }
                        IList<EAndMCoding> ComblstConsisChk = new List<EAndMCoding>();
                        if (SaveListCPT != null)
                        {
                            foreach (EAndMCoding obj in SaveListCPT)
                            {
                                ComblstConsisChk.Add(obj);
                            }
                        }
                        if (UpdateListCPT != null)
                        {
                            foreach (EAndMCoding objUpd in UpdateListCPT)
                            {
                                ComblstConsisChk.Add(objUpd);
                            }
                        }
                        bEandMCodingConsistent = XMLObj.CheckDataConsistency(ComblstConsisChk.Cast<object>().ToList(), true, string.Empty);

                        EandMCodingICDManager eandmicdMngr = new EandMCodingICDManager();
                        IList<EandMCodingICD> updatedEMICDBeforeSaveUpdate = new List<EandMCodingICD>();
                        if (SaveEMICDList.Count == 0)
                        {
                            SaveEMICDList = null;
                        }
                        if ((eandmICDList != null && eandmICDList.Count > 0) || (UpdateEMICDList != null && UpdateEMICDList.Count > 0))
                        {
                            if (eandmICDList != null && eandmICDList.Count > 0)
                                EncounterOrHumanId = eandmICDList[0].Encounter_ID;
                            else
                                if (UpdateEMICDList != null && UpdateEMICDList.Count > 0)
                                EncounterOrHumanId = UpdateEMICDList[0].Encounter_ID;

                            iResult = eandmicdMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref eandmICDList, ref UpdateEMICDList, null, MySession, string.Empty, true, true, EncounterOrHumanId, string.Empty, ref XMLObj);
                            // iResult = eandmicdMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveEMICDList, ref UpdateEMICDList, null, MySession, string.Empty, true, true, EncounterOrHumanId, string.Empty, ref XMLObj);
                            //iResult = eandmicdMngr.SaveUpdateDeleteWithoutTransaction(ref SaveEMICDList, UpdateEMICDList, null, MySession, string.Empty);

                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                }
                                else
                                {
                                    trans.Rollback();
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception is occured. Transaction failed");
                            }
                            IList<EandMCodingICD> CombICDConsischk = new List<EandMCodingICD>();
                            if (SaveEMICDList != null)
                            {
                                foreach (EandMCodingICD obj in SaveEMICDList)
                                {
                                    CombICDConsischk.Add(obj);
                                }
                            }
                            if (UpdateEMICDList != null)
                            {
                                foreach (EandMCodingICD objUpd in UpdateEMICDList)
                                {
                                    CombICDConsischk.Add(objUpd);
                                }
                            }

                            bEandMCodingICDConsistent = XMLObj.CheckDataConsistency(CombICDConsischk.Cast<object>().ToList(), true, string.Empty);
                        }


                        //Inorder to find DeleteICD List(Comparing Existing List with Previous List which is saved in Session)
                        IList<EandMCodingICD> CombinedEMICDList = new List<EandMCodingICD>();
                        if (UpdateEMICDList != null)
                        {
                            foreach (EandMCodingICD emicd in UpdateEMICDList)
                            {
                                updatedEMICDBeforeSaveUpdate.Add(emicd);
                            }
                        }

                        if (SaveEMICDList != null)
                        {
                            for (int i = 0; i < SaveEMICDList.Count; i++)
                            {
                                CombinedEMICDList.Add(SaveEMICDList[i]);
                            }
                        }
                        if (UpdateEMICDList != null)
                        {
                            for (int i = 0; i < UpdateEMICDList.Count; i++)
                            {
                                CombinedEMICDList.Add(UpdateEMICDList[i]);
                            }
                        }
                        //if (updatedEMICDBeforeSaveUpdate != null)
                        //{
                        //    for (int i = 0; i < updatedEMICDBeforeSaveUpdate.Count; i++)
                        //    {
                        //        updatedEMICDBeforeSaveUpdate[i].Version = updatedEMICDBeforeSaveUpdate[i].Version - 1;
                        //        CombinedEMICDList.Add(updatedEMICDBeforeSaveUpdate[i]);
                        //    }
                        //}

                        foreach (EandMCodingICD eandmobj in eandmICDList)
                        {
                            eandmobj.Version = eandmobj.Version + 1;
                        }

                        if (eandmICDList.Count > 0 && CombinedEMICDList.Count > 0)
                        {
                            DeleteEMICDList = eandmICDList.Except(CombinedEMICDList).ToList<EandMCodingICD>();

                        }
                        foreach (EandMCodingICD obj in DeleteEMICDList)
                        {
                            obj.ICD_Category = "None";
                            obj.Is_Delete = "Y";
                            obj.Version = obj.Version - 1;
                        }


                        if (DeleteEMICDList.Count > 0)
                        {
                            IList<EandMCodingICD> SaveEandMICDList = new List<EandMCodingICD>();

                            IList<EandMCodingICD> UpdateEandMICDList = DeleteEMICDList;

                            EandMCodingICDManager eandmcodingmanager = new EandMCodingICDManager();
                            iResult = eandmcodingmanager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveEandMICDList, ref UpdateEandMICDList, null, MySession, string.Empty, true, true, DeleteEMICDList[0].Encounter_ID, string.Empty, ref XMLObj);
                            //iResult = eandmicdMngr.SaveUpdateDeleteWithoutTransaction(ref SaveEandMICDList, null, DeleteEMICDList, MySession, string.Empty);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                }
                                else
                                {
                                    trans.Rollback();
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception is occured. Transaction failed");
                            }
                            IList<EandMCodingICD> CombICDConsisDelchk = new List<EandMCodingICD>();
                            if (SaveEandMICDList != null)
                            {
                                foreach (EandMCodingICD obj in SaveEandMICDList)
                                {
                                    CombICDConsisDelchk.Add(obj);
                                }
                            }
                            if (UpdateEandMICDList != null)
                            {
                                foreach (EandMCodingICD objUpd in UpdateEandMICDList)
                                {
                                    CombICDConsisDelchk.Add(objUpd);
                                }
                            }

                            bEandMCodingICDafterDeleteConsistent = XMLObj.CheckDataConsistency(CombICDConsisDelchk.Cast<object>().ToList(), true, string.Empty);

                        }











                        //if (ImmDelList != null && ImmDelList.Count > 0)
                        //{
                        //    ulong Enc_ID = ImmDelList[0].Encounter_Id;
                        //    IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
                        //    IList<TreatmentPlan> delTplanlst = new List<TreatmentPlan>();
                        //    //BugID:46789
                        //    #region TPlanGET
                        //    string FileName = "Encounter" + "_" + Enc_ID + ".xml";
                        //    string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                        //    if (File.Exists(strXmlFilePath) == true)
                        //    {
                        //        XmlDocument itemDoc = new XmlDocument();
                        //        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                        //        XmlNodeList xmlTagName = null;
                        //        // itemDoc.Load(XmlText);
                        //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        //        {
                        //            itemDoc.Load(fs);

                        //            XmlText.Close();
                        //            #region Treatment_plan
                        //            if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
                        //            {
                        //                xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

                        //                if (xmlTagName.Count > 0)
                        //                {
                        //                    for (int j = 0; j < xmlTagName.Count; j++)
                        //                    {
                        //                        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == Enc_ID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("IMMUNIZATION/INJECTION"))
                        //                        {

                        //                            string TagName = xmlTagName[j].Name;
                        //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
                        //                            TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
                        //                            IEnumerable<PropertyInfo> propInfo = null;
                        //                            propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

                        //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                        //                            {

                        //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
                        //                                {
                        //                                    foreach (PropertyInfo property in propInfo)
                        //                                    {
                        //                                        if (property.Name == nodevalue.Name)
                        //                                        {
                        //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
                        //                                                property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
                        //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
                        //                                                property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
                        //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                        //                                                property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
                        //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
                        //                                                property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
                        //                                            else
                        //                                                property.SetValue(TreatmentPlan, nodevalue.Value, null);
                        //                                        }
                        //                                    }
                        //                                }

                        //                            }
                        //                            objTreatmentPlan.Add(TreatmentPlan);
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //            #endregion
                        //        }
                        //    }
                        //    if (Enc_ID != 0)
                        //    {
                        //        delTplanlst = (from oj in objTreatmentPlan where ImmDelList.Any(a => a.Id == oj.Source_ID) select oj).ToList<TreatmentPlan>();
                        //    }
                        //    #endregion

                        //    IList<Immunization> ImmSavelst = new List<Immunization>();
                        //    IList<Immunization> ImmUpdatelst = new List<Immunization>();
                        //    IList<Immunization> ImmDeletelst = ImmDelList;
                        //    ImmunizationManager ImmManager = new ImmunizationManager();
                        //    iResult = ImmManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ImmSavelst, ref ImmUpdatelst, ImmDeletelst, MySession, string.Empty, true, false, ImmDeletelst[0].Human_ID, string.Empty, ref XMLObjHuman);
                        //    if (iResult == 2)
                        //    {
                        //        if (iTryCount < 5)
                        //        {
                        //            iTryCount++;
                        //        }
                        //        else
                        //        {
                        //            trans.Rollback();
                        //            throw new Exception("Deadlock is occured. Transaction failed");
                        //        }
                        //    }
                        //    else if (iResult == 1)
                        //    {
                        //        trans.Rollback();
                        //        throw new Exception("Exception is occured. Transaction failed");
                        //    }
                        //    //BugID:46789
                        //    if (delTplanlst != null && delTplanlst.Count > 0)
                        //    {
                        //        IList<TreatmentPlan> TplanSavelst = new List<TreatmentPlan>();
                        //        IList<TreatmentPlan> TplanUpdatelst = new List<TreatmentPlan>();
                        //        IList<TreatmentPlan> TplanDeletelst = delTplanlst;
                        //        TreatmentPlanManager TplanManager = new TreatmentPlanManager();
                        //        iResult = TplanManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref TplanSavelst, ref TplanUpdatelst, TplanDeletelst, MySession, string.Empty, true, true, Enc_ID, string.Empty, ref XMLObj);
                        //        if (iResult == 2)
                        //        {
                        //            if (iTryCount < 5)
                        //            {
                        //                iTryCount++;
                        //            }
                        //            else
                        //            {
                        //                trans.Rollback();
                        //                throw new Exception("Deadlock is occured. Transaction failed");
                        //            }
                        //        }
                        //        else if (iResult == 1)
                        //        {
                        //            trans.Rollback();
                        //            throw new Exception("Exception is occured. Transaction failed");
                        //        }
                        //    }

                        //}
                        //if (ImmHisDelList != null && ImmHisDelList.Count > 0)
                        //{
                        //    IList<ImmunizationHistory> ImmHisSavelst = new List<ImmunizationHistory>();
                        //    IList<ImmunizationHistory> ImmHisUpdatelst = new List<ImmunizationHistory>();
                        //    IList<ImmunizationHistory> ImmHisDeletelst = ImmHisDelList;

                        //    ImmunizationHistoryManager ImmManager = new ImmunizationHistoryManager();
                        //    ImmunizationMasterHistoryManager obj = new ImmunizationMasterHistoryManager();

                        //    IList<ImmunizationMasterHistory> lst = new List<ImmunizationMasterHistory>();
                        //    IList<ImmunizationMasterHistory> lstsave = new List<ImmunizationMasterHistory>();

                        //    lst = obj.GetImmunizationDeteailsbyID(ImmHisDelList[0].Immunization_History_Master_ID);
                        //    if (lst.Count > 0)
                        //    {
                        //        lst[0].Is_Deleted = "Y";
                        //        lst[0].Modified_Date_And_Time = System.DateTime.Now;
                        //        lst[0].Modified_Date_And_Time = System.DateTime.Now;

                        //    }


                        //    iResult = obj.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref lstsave, ref lst, null, MySession, string.Empty, true, true, lst[0].Human_ID, string.Empty, ref XMLObjHuman);
                        //    if (iResult == 2)
                        //    {
                        //        if (iTryCount < 5)
                        //        {
                        //            iTryCount++;
                        //        }
                        //        else
                        //        {
                        //            trans.Rollback();
                        //            throw new Exception("Deadlock is occured. Transaction failed");
                        //        }
                        //    }
                        //    else if (iResult == 1)
                        //    {
                        //        trans.Rollback();
                        //        throw new Exception("Exception is occured. Transaction failed");
                        //    }

                        //    iResult = ImmManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ImmHisSavelst, ref ImmHisUpdatelst, ImmHisDelList, MySession, string.Empty, true, false, ImmHisDeletelst[0].Human_ID, string.Empty, ref XMLObjHuman);
                        //    if (iResult == 2)
                        //    {
                        //        if (iTryCount < 5)
                        //        {
                        //            iTryCount++;
                        //        }
                        //        else
                        //        {
                        //            trans.Rollback();
                        //            throw new Exception("Deadlock is occured. Transaction failed");
                        //        }
                        //    }
                        //    else if (iResult == 1)
                        //    {
                        //        trans.Rollback();
                        //        throw new Exception("Exception is occured. Transaction failed");
                        //    }
                        //}
                        //if (CareplanUpdate != null && CareplanUpdate.Id.ToString() != "0")//Updation of Care_plan on deletion of CPTs "99406","99407" - BugID:47386
                        //{
                        //    IList<CarePlan> CplanSavelst = new List<CarePlan>();
                        //    IList<CarePlan> CplanUpdatelst = new List<CarePlan>();
                        //    IList<CarePlan> CplanDeletelst = new List<CarePlan>();
                        //    CplanUpdatelst.Add(CareplanUpdate);
                        //    CarePlanManager cplanmanager = new CarePlanManager();
                        //    iResult = cplanmanager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref CplanSavelst, ref CplanUpdatelst, CplanDeletelst, MySession, string.Empty, true, true, CplanUpdatelst[0].Encounter_ID, string.Empty, ref XMLObj);
                        //    if (iResult == 2)
                        //    {
                        //        if (iTryCount < 5)
                        //        {
                        //            iTryCount++;
                        //        }
                        //        else
                        //        {
                        //            trans.Rollback();
                        //            throw new Exception("Deadlock is occured. Transaction failed");
                        //        }
                        //    }
                        //    else if (iResult == 1)
                        //    {
                        //        trans.Rollback();
                        //        throw new Exception("Exception is occured. Transaction failed");
                        //    }
                        //}

                        //if (bEandMCodingConsistent && bEandMCodingICDConsistent && bEandMCodingICDafterDeleteConsistent && bEncounterConsistent)
                        //{
                        if (XMLObj != null)
                        {
                            //  XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                            int trycount = 0;
                        trytosaveagain:
                            try
                            {
                                //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                WriteBlob(EncounterOrHumanId, XMLObj.itemDoc, MySession, SaveListCPT, UpdateListCPT, null, XMLObj, false);
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
                        if (XMLObjHuman != null)
                        {
                            //  XMLObjHuman.itemDoc.Save(XMLObjHuman.strXmlFilePath);
                            int trycount = 0;
                        trytosaveagain:
                            try
                            {
                                //XMLObjHuman.itemDoc.Save(XMLObjHuman.strXmlFilePath);
                                //WriteBlob(EncounterOrHumanId, XMLObjHuman.itemDoc, MySession, SaveListCPT, UpdateListCPT, null, XMLObjHuman, false);
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
                        trans.Commit();
                        //}
                        //else
                        //    throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");

                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        MySession.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //bugid:x6modifier//WISH FIX .. BugID:45392
            //Load Data from XML

            //#region data from XML
            //IList<string> ICDList = new List<string>();
            ////ArrayList aryAccessCPT = new ArrayList();
            //IList<string> TempProcedureList = new List<string>();
            //IList<string> ICdlist = new List<string>();
            //ArrayList aryAccessICD = new ArrayList();
            //bool IsPrimaryChecked = false;
            //string sAssesmentPrimary = string.Empty;
            //FileNames = "Encounter" + "_" + EncounterOrHumanId + ".xml";
            //strXmlFilePaths = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileNames);
            //if (File.Exists(strXmlFilePaths) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePaths);
            //    XmlNodeList xmlTagName = null;
            //    //  itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(strXmlFilePaths, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();

            //        #region EandMCodingICD
            //        DTO.EandMCodingICDList = new List<EandMCodingICD>();
            //        if (itemDoc.GetElementsByTagName("EandMCodingICDList") != null)
            //        {
            //            if (itemDoc.GetElementsByTagName("EandMCodingICDList").Count > 0)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("EandMCodingICDList")[0].ChildNodes;
            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {
            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(EandMCodingICD));
            //                        EandMCodingICD EandMCodingICD = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as EandMCodingICD;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        if (EandMCodingICD != null)
            //                        {
            //                            propInfo = from obji in ((EandMCodingICD)EandMCodingICD).GetType().GetProperties() select obji;

            //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                            {
            //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                                {
            //                                    foreach (PropertyInfo property in propInfo)
            //                                    {
            //                                        if (property.Name == nodevalue.Name)
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(EandMCodingICD, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(EandMCodingICD, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(EandMCodingICD, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(EandMCodingICD, Convert.ToInt32(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
            //                                                property.SetValue(EandMCodingICD, Convert.ToDecimal(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(EandMCodingICD, nodevalue.Value, null);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                            if (EandMCodingICD.Encounter_ID == EncounterOrHumanId && (EandMCodingICD.Is_Delete == "N" || EandMCodingICD.Is_Delete == ""))
            //                            {
            //                                DTO.EandMCodingICDList.Add(EandMCodingICD);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        if (DTO.EandMCodingICDList != null)
            //        {
            //            DTO.EandMCodingICDList = DTO.EandMCodingICDList.OrderBy(a => a.Sequence).ToList();
            //            for (int j = 0; j < DTO.EandMCodingICDList.Count; j++)
            //            {
            //                string sPrimary = string.Empty;
            //                if (DTO.EandMCodingICDList[j].ICD_Category == "Primary")
            //                {
            //                    sPrimary = "Y";
            //                    IsPrimaryChecked = true;
            //                }

            //                var eandMICDList = from eandmICD in DTO.EandMCodingICDList where eandmICD.ICD == DTO.EandMCodingICDList[j].ICD select eandmICD;
            //                IList<EandMCodingICD> templist = eandMICDList.ToList<EandMCodingICD>();
            //                string sIDList = string.Empty;
            //                string sICDVersion = string.Empty;
            //                for (int k = 0; k < templist.Count; k++)
            //                {
            //                    if (k == 0)
            //                    {
            //                        sIDList = templist[k].E_M_Coding_ID.ToString();
            //                        sICDVersion = templist[k].Version.ToString();
            //                    }
            //                    else
            //                    {
            //                        sIDList += "," + templist[k].E_M_Coding_ID.ToString();
            //                        sICDVersion = "," + templist[k].Version.ToString();
            //                    }
            //                }

            //                if (!ICDList.Any(a => a.Split('~')[1] == DTO.EandMCodingICDList[j].ICD))
            //                    ICDList.Add("ASSESSMENT" + "~" + DTO.EandMCodingICDList[j].ICD + "~" + DTO.EandMCodingICDList[j].ICD_Description + "~" + sICDVersion + "~" + sPrimary + "~" + sIDList + "~" + DTO.EandMCodingICDList[j].Sequence + "~" + ",");
            //                else
            //                {
            //                    IList<string> sReomveICDDesc = ICDList.Select(a => a.Split('~')[1]).ToList();
            //                    int iIndex = sReomveICDDesc.IndexOf(DTO.EandMCodingICDList[j].ICD);
            //                    if (iIndex > -1)
            //                        if (ICDList[iIndex].Split('~').Length == 8)
            //                        {
            //                            //added for 6th ICD Column Marking
            //                            if (ICDList[iIndex].Split('~')[ICDList[iIndex].Split('~').Length - 1].Split(',').Length < 6)
            //                            {
            //                                for (int i = 0; i < DTO.EandMCodingICDList[j].Sequence; i++)
            //                                {
            //                                    if (DTO.EandMCodingICDList[j].Sequence - 1 == i)
            //                                        ICDList[iIndex] += DTO.EandMCodingICDList[j].Sequence;
            //                                    else if (ICDList[iIndex].Split('~')[ICDList[iIndex].Split('~').Length - 1].Split(',').Length != DTO.EandMCodingICDList[j].Sequence)
            //                                        ICDList[iIndex] += ",";
            //                                }
            //                            }
            //                        }
            //                }
            //                aryAccessICD.Add(DTO.EandMCodingICDList[j].ICD);
            //            }
            //        }
            //        #endregion

            //        #region EAndMCodingList
            //        DTO.EandMCodingList = new List<EAndMCoding>();
            //        if (itemDoc.GetElementsByTagName("EAndMCodingList") != null)
            //        {
            //            if (itemDoc.GetElementsByTagName("EAndMCodingList").Count > 0)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("EAndMCodingList")[0].ChildNodes;
            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {
            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(EAndMCoding));
            //                        EAndMCoding EAndMCoding = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as EAndMCoding;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        if (EAndMCoding != null)
            //                        {
            //                            propInfo = from obji in ((EAndMCoding)EAndMCoding).GetType().GetProperties() select obji;

            //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                            {
            //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                                {
            //                                    foreach (PropertyInfo property in propInfo)
            //                                    {
            //                                        if (property.Name == nodevalue.Name)
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(EAndMCoding, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(EAndMCoding, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(EAndMCoding, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(EAndMCoding, Convert.ToInt32(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
            //                                                property.SetValue(EAndMCoding, Convert.ToDecimal(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(EAndMCoding, nodevalue.Value, null);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                            IList<string> ProcList = new List<string>();
            //                            if (EAndMCoding.Encounter_ID == EncounterOrHumanId && (EAndMCoding.Is_Delete == "N" || EAndMCoding.Is_Delete == ""))
            //                            {
            //                                #region setCPTOrder
            //                                if (CombinedSaveUpdateList != null && CombinedSaveUpdateList.Count > 0)
            //                                {
            //                                    IList<EAndMCoding> cptlst = CombinedSaveUpdateList.Where(a => a.Id == EAndMCoding.Id).ToList<EAndMCoding>();
            //                                    if (cptlst != null && cptlst.Count > 0)
            //                                        EAndMCoding.CPT_Order = cptlst[0].CPT_Order;
            //                                }
            //                                #endregion

            //                                DTO.EandMCodingList.Add(EAndMCoding);
            //                                ProcList.Add(EAndMCoding.Procedure_Code + "~" + EAndMCoding.Procedure_Code_Description + "~" + EAndMCoding.Id + "~" + EAndMCoding.Units + "~" + EAndMCoding.Modifier1 + "~" + EAndMCoding.Modifier2 + "~" + EAndMCoding.Modifier3 + "~" + EAndMCoding.Modifier4 + "~" + EAndMCoding.Sequence + "~" + EAndMCoding.Version);

            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        #endregion

            //    }
            //}
            //#endregion
            //if (ilstUpdateList != null)
            //{
            //    for (int i = 0; i < ilstUpdateList.Count; i++)
            //    {
            //        DTO.EncounterList.Add(ilstUpdateList[i]);
            //    }
            //}

            //if (DTO.EncounterList != null)
            //{
            //    if (DTO.EncounterList.Count == 0)
            //        DTO.EncounterList.Add(SaveEncounter[0]);
            //}
            return DTO;
        }

        #endregion
    }
}
