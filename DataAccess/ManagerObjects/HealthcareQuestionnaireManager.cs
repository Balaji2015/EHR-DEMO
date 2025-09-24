using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.Linq;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IHealthcareQuestionnaireManager : IManagerBase<Healthcare_Questionnaire, ulong>
    {
        IList<FillHealthcareQuestionnaire> AppendHealthQuestion(IList<Healthcare_Questionnaire> HealthQuestionList, DateTime PatientDOB, DateTime dtpAppointmentDate, string MACAddress, bool sStaus, IList<FillHealthcareQuestionnaire> HealthQuestionnaireList);
        IList<Healthcare_Questionnaire> GetHealthQuestionList(ulong ulEncID, out IList<string> CategoryList);
        int GetHealthQuestionEntries(ulong ulEncID);
        IList<FillHealthcareQuestionnaire> UpdateHealthQuestion(IList<Healthcare_Questionnaire> HealthQuestionList, DateTime PatientDOB, DateTime dtpAppointmentDate, string MACAddress, bool sStaus, IList<FillHealthcareQuestionnaire> HealthQuestionnaireList);

    }

    public partial class HealthcareQuestionnaireManager : ManagerBase<Healthcare_Questionnaire, ulong>, IHealthcareQuestionnaireManager
    {
        #region Constructors

        public HealthcareQuestionnaireManager()
            : base()
        {

        }
        public HealthcareQuestionnaireManager(INHibernateSession session)
            : base(session)
        {

        }
        #endregion
        #region Get Methods
        public IList<FillHealthcareQuestionnaire> GetFillHealthQuestionnaire(IList<FillHealthcareQuestionnaire> HealthQuestionnaireList, IList<Healthcare_Questionnaire> HealthQuestionList, string sInsertOrUpdate)
        {
            if (HealthQuestionnaireList != null && HealthQuestionnaireList.Count > 0)
            {
                for (int i = 0; i < HealthQuestionList.Count; i++)
                {
                    HealthQuestionnaireList[i].HealthCare_Questionnaire_ID = HealthQuestionList[i].Id;
                    HealthQuestionnaireList[i].Version = HealthQuestionList[i].Version;
                    if (HealthQuestionnaireList[i].Question.ToUpper() == "TOTAL SCORE")
                        HealthQuestionnaireList[i].Selected_Option = HealthQuestionList[i].Selected_Option;
                }
            }
            return HealthQuestionnaireList;
        }
        public IList<FillHealthcareQuestionnaire> AppendHealthQuestion(IList<Healthcare_Questionnaire> HealthQuestionList, DateTime PatientDOB, DateTime dtpAppointmentDate, string MACAddress, bool sStaus, IList<FillHealthcareQuestionnaire> HealthQuestionnaireList)
        {
            IList<FillHealthcareQuestionnaire> HealthQuestionScreenList = new List<FillHealthcareQuestionnaire>();

            //SaveUpdateDeleteWithTransaction(ref HealthQuestionList, null, null, MACAddress);
            IList<Healthcare_Questionnaire> HealthQuestionUpdateListnull = null;
            string sQuestionnaireTabName = string.Empty;
            sQuestionnaireTabName = HealthQuestionList[0].Questionnaire_Category.Replace(" ", "").Replace("/", "");
            SaveUpdateDelete_DBAndXML_WithTransaction(ref HealthQuestionList, ref HealthQuestionUpdateListnull, null, MACAddress, true, false, HealthQuestionList[0].Encounter_ID, sQuestionnaireTabName);
            QuestionnaireLookupManager QuestionMngr = new QuestionnaireLookupManager();
            if (HealthQuestionList != null && HealthQuestionList.Count > 0)
            {
                HealthQuestionScreenList = GetFillHealthQuestionnaire(HealthQuestionnaireList, HealthQuestionList, "INSERT");

                //HealthQuestionScreenList = QuestionMngr.GetHealthQuestionLookupListFromServer(HealthQuestionList[0].Questionnaire_Category, PatientDOB, dtpAppointmentDate, HealthQuestionList[0].Created_By, HealthQuestionList[0].Encounter_ID, sStaus);
            }

            //comment by balaji.T
            //GenerateXml XMLObj = new GenerateXml();
            //if (HealthQuestionList != null && HealthQuestionList.Count > 0)
            //{
            //    string sQuestionnaireTabName = string.Empty;
            //    ulong encounterid = HealthQuestionList[0].Encounter_ID;
            //    sQuestionnaireTabName = HealthQuestionList[0].Questionnaire_Category.Replace(" ", "").Replace("/","");
            //    List<object> lstObj = HealthQuestionList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, encounterid, sQuestionnaireTabName);
            //}
            return HealthQuestionScreenList;
        }

        public IList<FillHealthcareQuestionnaire> UpdateHealthQuestion(IList<Healthcare_Questionnaire> HealthQuestionList, DateTime PatientDOB, DateTime dtpAppointmentDate, string MACAddress, bool sStaus, IList<FillHealthcareQuestionnaire> HealthQuestionnaireList)
        {
            IList<FillHealthcareQuestionnaire> HealthQuestionScreenList = new List<FillHealthcareQuestionnaire>();

            IList<Healthcare_Questionnaire> addList = null;

            string sQuestionnaireTabName = string.Empty;
            ulong encounterid = 0;
            if (HealthQuestionList != null && HealthQuestionList.Count > 0)
            {
                encounterid = HealthQuestionList[0].Encounter_ID;
                sQuestionnaireTabName = HealthQuestionList[0].Questionnaire_Category.Replace(" ", "").Replace("/", "");
            }
            //SaveUpdateDeleteWithTransaction(ref addList, HealthQuestionList, null, MACAddress);

            SaveUpdateDelete_DBAndXML_WithTransaction(ref addList, ref HealthQuestionList, null, MACAddress, true, false, encounterid, sQuestionnaireTabName);
            QuestionnaireLookupManager QuestionMngr = new QuestionnaireLookupManager();
            if (HealthQuestionList != null && HealthQuestionList.Count > 0)
            {
                HealthQuestionScreenList = GetFillHealthQuestionnaire(HealthQuestionnaireList, HealthQuestionList, "UPDATE");

                //HealthQuestionScreenList = QuestionMngr.GetHealthQuestionLookupListFromServer(HealthQuestionList[0].Questionnaire_Category, PatientDOB, dtpAppointmentDate, HealthQuestionList[0].Created_By, HealthQuestionList[0].Encounter_ID, sStaus);
            }
            //code comment by balaji.T
            //GenerateXml XMLObj = new GenerateXml();
            //if (HealthQuestionList != null && HealthQuestionList.Count > 0)
            //{
            //    string sQuestionnaireTabName = string.Empty;
            //    ulong encounterid = HealthQuestionList[0].Encounter_ID;
            //    sQuestionnaireTabName = HealthQuestionList[0].Questionnaire_Category.Replace(" ", "").Replace("/", "");
            //    List<object> lstObj = HealthQuestionList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, encounterid, sQuestionnaireTabName);
            //}

            return HealthQuestionScreenList;
        }


        public IList<FillHealthcareQuestionnaire> UpdateHealthQuestionforRos(IList<Healthcare_Questionnaire> HealthQuestionList, DateTime PatientDOB, DateTime dtpAppointmentDate, string MACAddress, bool sStaus, IList<FillHealthcareQuestionnaire> HealthQuestionnaireList)
        {
            IList<FillHealthcareQuestionnaire> HealthQuestionScreenList = new List<FillHealthcareQuestionnaire>();

            IList<Healthcare_Questionnaire> addList = null;

            //SaveUpdateDeleteWithTransaction(ref addList, HealthQuestionList, null, MACAddress);
            string sQuestionnaireTabName = string.Empty;
            ulong encounterid = 0;
            if (HealthQuestionList != null && HealthQuestionList.Count > 0)
            {
                encounterid = HealthQuestionList[0].Encounter_ID;
                sQuestionnaireTabName = HealthQuestionList[0].Questionnaire_Category.Replace(" ", "").Replace("/", "");
            }
            SaveUpdateDelete_DBAndXML_WithTransaction(ref addList, ref HealthQuestionList, null, MACAddress, true, false, encounterid, sQuestionnaireTabName);
            QuestionnaireLookupManager QuestionMngr = new QuestionnaireLookupManager();
            if (HealthQuestionList != null && HealthQuestionList.Count > 0)
            {
                //HealthQuestionScreenList = GetFillHealthQuestionnaire(HealthQuestionnaireList, HealthQuestionList, "UPDATE");
                //CAP-3628
                //HealthQuestionScreenList = QuestionMngr.GetHealthQuestionLookupListFromServer(HealthQuestionList[0].Questionnaire_Category, PatientDOB, dtpAppointmentDate, HealthQuestionList[0].Created_By, HealthQuestionList[0].Encounter_ID, sStaus);
                HealthQuestionScreenList = QuestionMngr.GetHealthQuestionLookupListFromServer(HealthQuestionList[0].Questionnaire_Category, PatientDOB, dtpAppointmentDate, HealthQuestionList[0].Created_By, HealthQuestionList[0].Encounter_ID, sStaus, HealthQuestionList[0].Human_ID);
            }
            //GenerateXml XMLObj = new GenerateXml(); //code comment by balaji.T
            //if (HealthQuestionList != null && HealthQuestionList.Count > 0)
            //{
            //    string sQuestionnaireTabName = string.Empty;
            //    ulong encounterid = HealthQuestionList[0].Encounter_ID;
            //    sQuestionnaireTabName = HealthQuestionList[0].Questionnaire_Category.Replace(" ", "").Replace("/", "");
            //    List<object> lstObj = HealthQuestionList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, encounterid, sQuestionnaireTabName);
            //}

            return HealthQuestionScreenList;
        }


        public IList<Healthcare_Questionnaire> GetHealthQuestionList(ulong ulEncID, out IList<string> CategoryList)
        {
            IList<Healthcare_Questionnaire> Questionary = new List<Healthcare_Questionnaire>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Healthcare_Questionnaire)).Add(Expression.Eq("Encounter_ID", ulEncID));
                Questionary = crit.List<Healthcare_Questionnaire>();
                iMySession.Close();
                CategoryList = Questionary.Select(a => a.Questionnaire_Category).Distinct().ToList<string>();
            }
            return Questionary;
        }

        public int GetHealthQuestionEntries(ulong ulEncID)
        {
            int iCount = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Healthcare_Questionnaire)).Add(Expression.Eq("Encounter_ID", ulEncID));
                iCount = crit.List<Healthcare_Questionnaire>().Count;
                iMySession.Close();
            }
            return iCount;
        }

        //Added CopyPrev for Questionnaire for BugID:44324 - COPD Screening Feature
        public ulong GetQuestionnaireCount(ulong encounterId, string type, bool isFromArchive)
        {
            ulong countQuestionnaire = 0;
            IList<Healthcare_Questionnaire> lstHealthquestionnaire = new List<Healthcare_Questionnaire>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                var querySQL = string.Format(@"SELECT E.* 
                                               FROM   {0} E 
                                               WHERE  E.ENCOUNTER_ID = :ENCOUNTER_ID 
                                               AND E.QUESTIONNAIRE_CATEGORY = :TYPE",
                                               isFromArchive ? "HEALTHCARE_QUESTIONNAIRE_ARC" : "HEALTHCARE_QUESTIONNAIRE");

                var SQLQuery = iMySession.CreateSQLQuery(querySQL)
                       .AddEntity("E", typeof(Healthcare_Questionnaire));

                SQLQuery.SetParameter("ENCOUNTER_ID", encounterId);
                SQLQuery.SetParameter("TYPE", type);

                lstHealthquestionnaire = SQLQuery.List<Healthcare_Questionnaire>();
                if(lstHealthquestionnaire!=null && lstHealthquestionnaire.Count>0)
                countQuestionnaire = Convert.ToUInt64(lstHealthquestionnaire.Count);

                iMySession.Close();
            }
            return countQuestionnaire;
        }
        //Added CopyPrev for Questionnaire for BugID:44324 - COPD Screening Feature
        public void GetQuestionnaireforPastEncounter(ulong humanId, ulong encounterId, ulong physicianId, string questionnaireType, out bool isPrevEncPresent, out bool bPhysicianProcess, out bool isPrevQuestionnairePresent, out ulong prevEncID, out bool isFromArchive)
        {
            EncounterManager objEncounterManager = new EncounterManager();

            prevEncID = 0;
            bPhysicianProcess = false;
            isPrevQuestionnairePresent = false;
            isPrevEncPresent = false;
            isFromArchive = false;
            ulong questionnaireCount = 0;

            var lstEncounter = objEncounterManager.GetPreviousEncounterDetails(encounterId, humanId, physicianId, out bPhysicianProcess, out isFromArchive);

            if (lstEncounter.Count > 0)
            {
                prevEncID = lstEncounter[0].Id;
                isPrevEncPresent = true;
                if (bPhysicianProcess)
                {
                    questionnaireCount = GetQuestionnaireCount(prevEncID, questionnaireType, isFromArchive);
                    if (questionnaireCount > 0)
                        isPrevQuestionnairePresent = true;
                }

            }
        }
        #endregion

    }
}
