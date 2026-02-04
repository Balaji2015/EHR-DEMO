using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Linq;


namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IPatientInsuredPlanManager : IManagerBase<PatientInsuredPlan, ulong>
    {


        PatientInsuredPlan getInsuranceDetailsUsingPatInsuredId(ulong patInsuredId);
        IList<PatientInsuredPlan> getInsurancePoliciesByHumanId(ulong patHumanId);
        void addPatInsured(PatientInsuredPlan patInsured, string MACAddress);
        void UpdatePatInsured(PatientInsuredPlan patinsuredPlan, string MACAddress);
        void BatchUpdatePatInsured(IList<PatientInsuredPlan> patinsuredPlan, ulong ulCarrier, string MACAddress);
        int addPatInsuredPlan(PatientInsuredPlan patInsured, ISession MySession, string MACAddress);
        IList<PatientInsuredPlan> GetActiveInsurancePoliciesByHumanId(ulong ulHumanID);
        IList<PatientInsuredPlan> GetActiveInsurancePoliciesByHumanId(ulong ulHumanID,string sInsuranceType);
        IList<PatientInsuredPlan> getInsuranceDetailsByPatInsuredId(ulong patInsuredId);
        IList<PatientInsuredPlan> GetPolicyHolderId(ulong InsuransePlanId, string Humanid);
        IList<PatientInsuredPlan> GetPlanbyHumanID(ulong[] human_ID, out IList<InsurancePlan> lstInsPlan);
        //Added for ACO by srividhya on 9-Jul-2014
        IList<PatientInsuredPlan> GetPatInsPlanDetailsbyPolicyHolderID(string HICNumber);
        IList<PatientInsuredPlan> GetPatInsPlanDetails(IList<ulong> Human_ID);
        //Added by srividhya on 9-Sep-2014
        IList<PatientInsuredPlan> GetEligDetailsBetweenEffectiveAndTerminationDate(ulong ulHumanId, ulong ulInsPlanId, string sDOS);
        ulong GetHumanCountbycarrierIDMedicaid(IList<ulong> humanid);
        ulong GetHumanCountbycarrierIDOther(IList<ulong> humanid);
        IList<PatientInsuredPlan> getInsurancePoliciesByPolicyHolderId(ulong patHumanId, string PolicyHolderId);
        Carrier GetActivePrimaryInsuranceNameByHumanId(ulong ulHumanID);
    }
    public partial class  PatientInsuredPlanManager: ManagerBase<PatientInsuredPlan, ulong>, IPatientInsuredPlanManager
    {
        #region Constructors

        public PatientInsuredPlanManager()
            : base()
        {

        }
        public PatientInsuredPlanManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion


        #region Get Methods


        public PatientInsuredPlan getInsuranceDetailsUsingPatInsuredId(ulong patInsuredId)
        {
            IList<PatientInsuredPlan> objPatInsPlan = new List<PatientInsuredPlan>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = mySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Id", patInsuredId));
                objPatInsPlan = criteria.List<PatientInsuredPlan>();
                mySession.Close();
            }
            return objPatInsPlan[0];
        }


        public IList<PatientInsuredPlan> getInsurancePoliciesByHumanId(ulong patHumanId)
        {
            IList<PatientInsuredPlan> objPatInsurPlan = new List<PatientInsuredPlan>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = mySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Human_ID", patHumanId));
                 objPatInsurPlan = criteria.List<PatientInsuredPlan>();
                mySession.Close();
            }
            return objPatInsurPlan;
        }

        public IList<PatientInsuredPlan> getInsurancePoliciesByPolicyHolderId(ulong patHumanId,string PolicyHolderId)
        {
            IList<PatientInsuredPlan> objPatInsurPlan = new List<PatientInsuredPlan>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = mySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Human_ID", patHumanId)).Add(Expression.Eq("Policy_Holder_ID", PolicyHolderId));
                objPatInsurPlan = criteria.List<PatientInsuredPlan>();
                mySession.Close();
            }
            return objPatInsurPlan;
        }

        public void addPatInsured(PatientInsuredPlan patInsured, string MACAddress)
        {
            IList<PatientInsuredPlan> patientInsuredPlanList = getInsurancePoliciesByHumanId(patInsured.Human_ID);

            var patins = from f in patientInsuredPlanList where f.Insurance_Type == "PRIMARY" select f;
            if (patins.ToList<PatientInsuredPlan>().Count > 0 && patInsured.Insurance_Type=="PRIMARY")
            {
                patInsured.Insurance_Type = string.Empty;
            }

            IList<PatientInsuredPlan> Patinslist = new List<PatientInsuredPlan>();
            Patinslist.Add(patInsured);
            GenerateXml ObjXml = new GenerateXml();
            ISession MySession = Session.GetISession();
            //bool bPhysicianPatient = true, bHuman = true;
            IList<PatientInsuredPlan> PatInsPlanTemp = null;
            if (Patinslist != null && Patinslist.Count > 0)
            {
                SaveUpdateDelete_DBAndXML_WithTransaction(ref Patinslist, ref PatInsPlanTemp, null, MACAddress, true, false, Patinslist[0].Human_ID, string.Empty);
                //int Result = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Patinslist, ref PatInsPlanTemp, null, MySession, "", true, true, Patinslist[0].Human_ID, string.Empty, ref ObjXml);
                //bPhysicianPatient = ObjXml.CheckDataConsistency(Patinslist.Cast<object>().ToList(), true, "");
            }
            IList<Human> patient = new List<Human>();
            HumanManager humanMngr = new HumanManager();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = mySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", patInsured.Human_ID));
                patient = criteria.List<Human>();
                mySession.Close();
            }
            if (patient != null && patient.Count > 0)
            {
                IList<Human> humanLst = null;
                patient[0].Encounter_Provider_ID = Convert.ToInt32(patInsured.PCP_ID);
                patient[0].PCP_Name = patInsured.PCP_Name;
                patient[0].PCP_NPI = patInsured.PCP_NPI;
                ulong humanId = patient[0].Id;
                humanMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref humanLst, ref patient, null, MACAddress, true, false, Patinslist[0].Human_ID, string.Empty);
            }
          
          //  int Result1 = humanMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref humanLst, ref patient, null, MySession, "", true, false, Patinslist[0].Human_ID, string.Empty, ref ObjXml);
            //bHuman = ObjXml.CheckDataConsistency(Patinslist.Cast<object>().ToList(), true, "");
            //if (bPhysicianPatient && bHuman)
            //    ObjXml.itemDoc.Save(ObjXml.strXmlFilePath);
            //GenerateXml XMLObj = new GenerateXml();
            //if (Patinslist.Count > 0)
            //{

            //    ulong humanid = Patinslist[0].Human_ID;
            //    List<object> lstObj = Patinslist.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, humanid, string.Empty);
            //}
        }

        //public void addPatInsuredPlan(PatientInsuredPlan patInsured, string MACAddress)
        //{
        //    IList<PatientInsuredPlan> Patinslist = new List<PatientInsuredPlan>();
        //    Patinslist.Add(patInsured);
        //    GenerateXml ObjXml = new GenerateXml();
        //    ISession MySession = Session.GetISession();
        //    //bool bPhysicianPatient = true, bHuman = true;
        //    IList<PatientInsuredPlan> PatInsPlanTemp = null;
        //    if (Patinslist != null && Patinslist.Count > 0)
        //    {
        //        //SaveUpdateDelete_DBAndXML_WithTransaction(ref Patinslist, ref PatInsPlanTemp, null, MACAddress, true, false, Patinslist[0].Human_ID, string.Empty);
        //        int Result = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Patinslist, ref PatInsPlanTemp, null, MySession, "", true, true, Patinslist[0].Human_ID, string.Empty, ref ObjXml);
        //        //bPhysicianPatient = ObjXml.CheckDataConsistency(Patinslist.Cast<object>().ToList(), true, "");
        //    }
        //}

        public int addPatInsuredPlan(PatientInsuredPlan patInsured, ISession MySession, string MACAddress)
        {
            IList<PatientInsuredPlan> patientInsuredPlanList = getInsurancePoliciesByHumanId(patInsured.Human_ID);

            var patins = from f in patientInsuredPlanList where f.Insurance_Type == "PRIMARY" select f;
            if (patins.ToList<PatientInsuredPlan>().Count > 0 && patInsured.Insurance_Type == "PRIMARY")
            {
                patInsured.Insurance_Type = string.Empty;
            }

            IList<PatientInsuredPlan> Patinslist = new List<PatientInsuredPlan>();
            Patinslist.Add(patInsured);
            int iResult = 0;
            GenerateXml ObjXML = new GenerateXml();
            IList<PatientInsuredPlan> UpdatePatInsList = null;
            if ((Patinslist != null))
            {
                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Patinslist, ref UpdatePatInsList, null, MySession, "", true, false, Patinslist[0].Human_ID, "", ref ObjXML);
            }
            return iResult;
        }


        public ulong GetHumanCountbycarrierIDMedicaid(IList<ulong> humanid)
        {
            ulong count = 0;
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

            IQuery EncounterExclusionquery22 = iMySession.GetNamedQuery("Get.HumanCountbyCarrierNameMedicaid");
            EncounterExclusionquery22.SetParameterList("human_id", humanid.ToArray());
            ArrayList Enc_Exclusion_lst22 = new ArrayList(EncounterExclusionquery22.List());
            if (Enc_Exclusion_lst22.Count > 0)
                count = Convert.ToUInt32(Enc_Exclusion_lst22.Count);
            return count;
        }
        public void UpdatePatInsured(PatientInsuredPlan patinsuredPlan, string MACAddress)
        {
            IList<PatientInsuredPlan> patientInsuredPlanList = getInsurancePoliciesByHumanId(patinsuredPlan.Human_ID);

            var patins = from f in patientInsuredPlanList where f.Insurance_Type == "PRIMARY" select f;
            if (patins.ToList<PatientInsuredPlan>().Count > 0 && patinsuredPlan.Insurance_Type == "PRIMARY" && patins.ToList<PatientInsuredPlan>()[0].Id!= patinsuredPlan.Id)
            {
                patinsuredPlan.Insurance_Type = string.Empty;
            }

            IList<PatientInsuredPlan> patInsuredPlanList = new List<PatientInsuredPlan>();
            patInsuredPlanList.Add(patinsuredPlan);
            IList<PatientInsuredPlan> insPlanInsertList = null;
            GenerateXml ObjXml = new GenerateXml();
            ISession MySession = Session.GetISession();
          //  bool bPhysicianPatient = true, bHuman = true;
            if (patInsuredPlanList.Count > 0)
            {
                SaveUpdateDelete_DBAndXML_WithTransaction(ref insPlanInsertList, ref patInsuredPlanList, null, MACAddress, true, false, patInsuredPlanList[0].Human_ID, string.Empty);
                //int Result = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref insPlanInsertList, ref patInsuredPlanList, null, MySession, "", true, true, patInsuredPlanList[0].Human_ID, string.Empty, ref ObjXml);
                //bPhysicianPatient = ObjXml.CheckDataConsistency(patInsuredPlanList.Cast<object>().ToList(), true, "");            
                IList<Human> patient = new List<Human>();
                HumanManager humanMngr = new HumanManager();
                using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
                {
                    ICriteria criteria = mySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", patInsuredPlanList[0].Human_ID));
                    patient = criteria.List<Human>();
                    mySession.Close();
                }
                if (patient != null && patient.Count > 0)
                {
                    IList<Human> humanLst = null;
                    patient[0].Encounter_Provider_ID = Convert.ToInt32(patInsuredPlanList[0].PCP_ID);
                    patient[0].PCP_Name = patInsuredPlanList[0].PCP_Name;
                    patient[0].PCP_NPI = patInsuredPlanList[0].PCP_NPI;
                    humanMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref humanLst, ref patient, null, MACAddress, true, false, patient[0].Id, string.Empty);
                }
               
                //int Result1 = humanMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref humanLst, ref patient, null, MySession, "", true, false, patient[0].Id, string.Empty, ref ObjXml);
                //bHuman = ObjXml.CheckDataConsistency(patient.Cast<object>().ToList(), true, "");
                //if (bPhysicianPatient && bHuman)
                //    ObjXml.itemDoc.Save(ObjXml.strXmlFilePath);   
                //GenerateXml XMLObj = new GenerateXml();
                //ulong humanid = patInsuredPlanList[0].Human_ID;
                //List<object> lstObj = patInsuredPlanList.Cast<object>().ToList();
                //XMLObj.GenerateXmlSave(lstObj, humanid, string.Empty);
            }
        }

        public void BatchUpdatePatInsured(IList<PatientInsuredPlan> patinsuredPlan, ulong ulCarrier, string MACAddress)
        {
            IList<PatientInsuredPlan> Patinslistadd = null;
            //GenerateXml ObjXml = new GenerateXml();
            //ISession MySession = Session.GetISession();
            //bool bPhysicianPatient = true;
            if (patinsuredPlan.Count > 0)
            {
                SaveUpdateDelete_DBAndXML_WithTransaction(ref Patinslistadd, ref patinsuredPlan, null, MACAddress, true, false, patinsuredPlan[0].Human_ID, string.Empty);
                //int Result = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Patinslistadd, ref patinsuredPlan, null, MySession, "", true, false, patinsuredPlan[0].Human_ID, string.Empty, ref ObjXml);
                //bPhysicianPatient = ObjXml.CheckDataConsistency(patinsuredPlan.Cast<object>().ToList(), true, "");
                //if (bPhysicianPatient)
                //    ObjXml.itemDoc.Save(ObjXml.strXmlFilePath);
            }
        }
        public ulong BatchAddUpdatePatInsured(IList<PatientInsuredPlan> patinsuredPlan, IList<PatientInsuredPlan> updatepatinsuredPlan, string MACAddress)
        {
            ulong uPat_Insured_Plan_ID = 0;
            IList<PatientInsuredPlan> Patinslistadd = null;
            //GenerateXml ObjXml = new GenerateXml();
            //ISession MySession = Session.GetISession();
            //bool bPhysicianPatient = true;
            if (patinsuredPlan.Count > 0)
            {
                SaveUpdateDelete_DBAndXML_WithTransaction(ref patinsuredPlan, ref updatepatinsuredPlan, null, MACAddress, true, false, patinsuredPlan[0].Human_ID, string.Empty);
                uPat_Insured_Plan_ID = patinsuredPlan[0].Id;
                //int Result = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Patinslistadd, ref patinsuredPlan, null, MySession, "", true, false, patinsuredPlan[0].Human_ID, string.Empty, ref ObjXml);
                //bPhysicianPatient = ObjXml.CheckDataConsistency(patinsuredPlan.Cast<object>().ToList(), true, "");
                //if (bPhysicianPatient)
                //    ObjXml.itemDoc.Save(ObjXml.strXmlFilePath);
            }

           else  if (updatepatinsuredPlan.Count > 0)
            {
                SaveUpdateDelete_DBAndXML_WithTransaction(ref patinsuredPlan, ref updatepatinsuredPlan, null, MACAddress, true, false, updatepatinsuredPlan[0].Human_ID, string.Empty);
                //int Result = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Patinslistadd, ref patinsuredPlan, null, MySession, "", true, false, patinsuredPlan[0].Human_ID, string.Empty, ref ObjXml);
                //bPhysicianPatient = ObjXml.CheckDataConsistency(patinsuredPlan.Cast<object>().ToList(), true, "");
                //if (bPhysicianPatient)
                //    ObjXml.itemDoc.Save(ObjXml.strXmlFilePath);
            }
            return uPat_Insured_Plan_ID;
        }
        //Added by srividhya on 09-Nov-2013
        public IList<PatientInsuredPlan> GetActiveInsurancePoliciesByHumanId(ulong ulHumanID)
        {
            IList<PatientInsuredPlan> Patinslistadd = new List<PatientInsuredPlan>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = mySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Active", "YES"));
                Patinslistadd = criteria.List<PatientInsuredPlan>();
                mySession.Close();
            }
            return Patinslistadd;
        }

        public IList<PatientInsuredPlan> GetActiveInsurancePoliciesByHumanId(ulong ulHumanID,string sInsuranceType)
        {
            IList<PatientInsuredPlan> objPatInsPlan = new List<PatientInsuredPlan>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = mySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Insurance_Type", sInsuranceType));
                objPatInsPlan = criteria.List<PatientInsuredPlan>();
                mySession.Close();
            }
            return objPatInsPlan;
        }
        public IList<PatientInsuredPlan> getInsuranceDetailsByPatInsuredId(ulong patInsuredId)
        {
            IList<PatientInsuredPlan> objPatInsPlan = new List<PatientInsuredPlan>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = mySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Id", patInsuredId));
                objPatInsPlan = criteria.List<PatientInsuredPlan>();
                mySession.Close();
            }
            return objPatInsPlan;
        }
        public IList<PatientInsuredPlan> GetPolicyHolderId(ulong InsuransePlanId,string Humanid)
        {

           
            ArrayList arrList = null;
            IList<PatientInsuredPlan> assign = new List<PatientInsuredPlan>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = mySession.GetNamedQuery("Get.PolicyHolderId");
                query.SetString(0, InsuransePlanId.ToString());
                query.SetAnsiString(1, Humanid);


                //User objuser = new User();
                arrList = new ArrayList(query.List());
                if (arrList != null && arrList.Count != 0)
                {
                    PatientInsuredPlan obj = new PatientInsuredPlan();
                    //StaticLookup=new StaticLookup();

                    obj.Policy_Holder_ID = arrList[0].ToString();
                    assign.Add(obj);


                }
                mySession.Close();
            }
            return assign;


        }
        public string GetPolicyHolderIdFromHumanId(ulong Humanid)
        {
            ArrayList arrList = null;
            string policy_holder_id = "0";
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery query = mySession.CreateSQLQuery("SELECT distinct  p.Policy_Holder_Id FROM `pat_insured_plan` p where p.human_id=" + Humanid);

                arrList = new ArrayList(query.List());
                if (arrList != null && arrList.Count != 0)
                    policy_holder_id = arrList[0].ToString();
               
                mySession.Close();
            }
            return policy_holder_id;
        }
        public IList<PatientInsuredPlan> GetPlanbyHumanID(ulong[] human_ID,out IList<InsurancePlan> lstInsPlan)
        {
           
            IList<PatientInsuredPlan> lstPatInsPlan = new List<PatientInsuredPlan>();
            lstInsPlan = new List<InsurancePlan>();
            ArrayList arryChkList = null;
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = mySession.GetNamedQuery("Get.InsurancePlanAndPatientPlanByID");
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
                        lstPatInsPlan.Add(objPlan);

                        InsurancePlan objInsPlan = new InsurancePlan();
                        objInsPlan.Id = Convert.ToUInt64(oj[1].ToString());
                        objInsPlan.Carrier_ID = Convert.ToInt32(oj[0].ToString());
                        lstInsPlan.Add(objInsPlan);
                    }
                }
                mySession.Close();
            }
            return lstPatInsPlan;
        }

        //Added for ACO by srividhya on 9-Jul-2014
        public IList<PatientInsuredPlan> GetPatInsPlanDetailsbyPolicyHolderID(string HICNumber)
        {
           
            IList<PatientInsuredPlan> lstGetList = new List<PatientInsuredPlan>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery query1 = mySession.CreateSQLQuery("select u.* FROM pat_insured_plan u where u.policy_Holder_Id in (:PolicyHolderID)").AddEntity("u", typeof(PatientInsuredPlan));
                query1.SetParameter("PolicyHolderID", HICNumber);
                lstGetList = query1.List<PatientInsuredPlan>();
                mySession.Close();
            }
            return lstGetList;
        }

        public IList<PatientInsuredPlan> GetPatInsPlanDetails(IList<ulong> Human_ID)
        {
            IList<PatientInsuredPlan> objPatInsPlan = new List<PatientInsuredPlan>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.In("Insured_Human_ID", Human_ID.ToArray<ulong>()));
                objPatInsPlan = crit.List<PatientInsuredPlan>();
                mySession.Close();
            }
            return objPatInsPlan;
        }

        public IList<PatientInsuredPlan> GetEligDetailsBetweenEffectiveAndTerminationDate(ulong ulHumanId, ulong ulInsPlanId, string sDOS)
        {
            IList<PatientInsuredPlan> PatInsPlanList = new List<PatientInsuredPlan>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = mySession.CreateSQLQuery("SELECT * FROM `pat_insured_plan` where human_id='" + ulHumanId + "' and insurance_plan_id='" + ulInsPlanId + "' and active='YES' and (('" + sDOS + "' between Effective_start_date and termination_date) or Effective_start_date='" + sDOS + "' or termination_date='" + sDOS + "');").AddEntity("a", typeof(PatientInsuredPlan));
                PatInsPlanList = sql.List<PatientInsuredPlan>();
                mySession.Close();
            }
            return PatInsPlanList;
        }

        public ulong GetHumanCountbycarrierID(IList<ulong> humanid)
        {
            ulong count = 0;
           ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            
            IQuery EncounterExclusionquery22 = iMySession.GetNamedQuery("Get.HumanCountbyCarrierName");
            EncounterExclusionquery22.SetParameterList("human_id", humanid.ToArray());
            ArrayList Enc_Exclusion_lst22 = new ArrayList(EncounterExclusionquery22.List());
            if (Enc_Exclusion_lst22.Count > 0)
                count = Convert.ToUInt32(Enc_Exclusion_lst22.Count);
            return count;
        }

        public ulong GetHumanCountbycarrierIDOther(IList<ulong> humanid)
        {
            ulong count = 0;
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

            IQuery EncounterExclusionquery22 = iMySession.GetNamedQuery("Get.HumanCountbyCarrierNameOther");
            EncounterExclusionquery22.SetParameterList("human_id", humanid.ToArray());
            ArrayList Enc_Exclusion_lst22 = new ArrayList(EncounterExclusionquery22.List());
            if (Enc_Exclusion_lst22.Count > 0)
                count = Convert.ToUInt32(Enc_Exclusion_lst22.Count);
            return count;
        }
        public Carrier GetActivePrimaryInsuranceNameByHumanId(ulong ulHumanID)
        {
            IList<PatientInsuredPlan> objPatInsPlan = new List<PatientInsuredPlan>();
            Carrier objCarrier = new Carrier();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = mySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Insurance_Type", "PRIMARY")).Add(Expression.Eq("Active", "YES"));
                objPatInsPlan = criteria.List<PatientInsuredPlan>();
                mySession.Close();
                if (objPatInsPlan.Count > 0)
                {
                    ulong uInsured_Id = objPatInsPlan[0].Insurance_Plan_ID;
                    if (uInsured_Id != 0)
                    {
                        InsurancePlanManager InsPlanMngr = new InsurancePlanManager();
                        IList<InsurancePlan> objInsPlan = new List<InsurancePlan>();
                        objInsPlan = InsPlanMngr.GetInsurancebyID(uInsured_Id);

                        CarrierManager carrierMngr = new CarrierManager();
                        objCarrier = carrierMngr.GetCarrierUsingId(Convert.ToUInt64(objInsPlan[0].Carrier_ID));
                    }
                }
            }
            return objCarrier;
        }

        public FindPhysican FindPhysicianByInsureList(ulong ulHumanID)
        {
            IList<PhysicianFacilityDTO> phylst = new List<PhysicianFacilityDTO>();
            FindPhysican objFindPhy = new FindPhysican();
            ArrayList resultList = null;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Find.Physician.by.Human.ID");
                query.SetParameter(0, ulHumanID);

                //query.SetParameter(3, "%" + token + "%");
                //query.SetParameter(4, "%" + token + "%");
                resultList = new ArrayList(query.List());
                if (resultList.Count > 0)
                {
                    foreach (object[] obj in resultList)
                    {
                        PhysicianFacilityDTO objPhy = new PhysicianFacilityDTO();
                        objPhy.PhyId = Convert.ToUInt64(obj[0]);
                        objPhy.PhyPrefix = obj[1].ToString();
                        objPhy.PhyFirstName = obj[2].ToString();
                        objPhy.PhyMiddleName = obj[3].ToString();
                        objPhy.PhyLastName = obj[4].ToString();
                        objPhy.PhySuffix = obj[5].ToString();
                        objPhy.PhySpecialtyCode = Convert.ToString(obj[6]);
                        //objPhy.PhyCity = obj[7].ToString();
                        //objPhy.PhyState = obj[8].ToString();
                        //objPhy.PhyZip = obj[9].ToString();
                        objPhy.PhyNPI = obj[10].ToString();
                        //objPhy.PhySpecialtyID = Convert.ToUInt64(obj[11]);7/2/
                        objPhy.PhySpecialtyID = Convert.ToString(obj[11]);
                        //objPhy.PhyFax = obj[13].ToString();
                        objPhy.PhyAddrs = obj[14].ToString();
                        //objPhy.PhyPhone = obj[15].ToString();
                        if (obj[12] != null)
                        {
                            objPhy.PhyFacility = obj[12].ToString();
                            FacilityManager Fmgr = new FacilityManager();
                            IList<FacilityLibrary> ilstFacilityLibrary = new List<FacilityLibrary>();
                            ilstFacilityLibrary = Fmgr.GetFacilityByFacilityname(obj[12].ToString());
                            if (ilstFacilityLibrary != null && ilstFacilityLibrary.Count > 0)
                            {
                                objPhy.PhyCity = ilstFacilityLibrary[0].Fac_City.ToString();
                                objPhy.PhyState = ilstFacilityLibrary[0].Fac_State.ToString();
                                objPhy.PhyZip = ilstFacilityLibrary[0].Fac_Zip.ToString();
                                objPhy.PhyFax = ilstFacilityLibrary[0].Fac_Fax.ToString();
                                objPhy.PhyPhone = ilstFacilityLibrary[0].Fac_Telephone.ToString();
                            }
                        }
                        else
                        {
                            if (obj[12] != null)
                                objPhy.PhyFacility = obj[12].ToString();
                            if (obj[7] != null)
                                objPhy.PhyCity = obj[7].ToString();
                            if (obj[8] != null)
                                objPhy.PhyState = obj[8].ToString();
                            if (obj[9] != null)
                                objPhy.PhyZip = obj[9].ToString();
                            if (obj[13] != null)
                                objPhy.PhyFax = obj[13].ToString();
                            if (obj[15] != null)
                                objPhy.PhyPhone = obj[15].ToString();
                        }

                        phylst.Add(objPhy);
                    }
                }

                objFindPhy.PhyList = phylst;

                iMySession.Close();
            }
            return objFindPhy;
        }

        #endregion
    }
}
