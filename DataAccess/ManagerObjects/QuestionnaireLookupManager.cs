using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;

namespace Acurus.Capella.DataAccess.ManagerObjects
{

    public interface IQuestionnaireLookupManager : IManagerBase<Questionnaire_Lookup, ulong>
    {
        IList<FillHealthcareQuestionnaire> GetHealthQuestionLookupListFromServer(string sCategory, DateTime PatientDoB, DateTime AppintmentDate, string UserName, ulong ulEncID, bool sStatus, ulong ulHumanID);
        IList<FillHealthcareQuestionnaire> GetHealthQuestionLookupListFromLocal(string sCategory, DateTime PatientDoB, DateTime AppintmentDate, string UserName, ulong ulEncID, bool sStatus, ulong ulHumanID);
        IList<Questionnaire_Lookup> BatchOperationsToQuestionnaireLookup(IList<Questionnaire_Lookup> addList, IList<Questionnaire_Lookup> updateList, IList<Questionnaire_Lookup> deleteList, string sMACAddress, string sUserName, string sQuestion);
        IList<Questionnaire_Lookup> GetQuestionnaireLookup(string UserName);
        IList<Questionnaire_Lookup> GetHealthQuestionCategoryList(string UserName);
        IList<FillHealthcareQuestionnaire> GetQuestionTypeList(string UserName);
        FillHealthcareQuestionnaire GetReviewOfQuestionnarieByEncounterId(ulong encounter_ID, ulong human_ID);
        IList<string> GetHealthQuestionRosType(string UserName);

    }

    public partial class QuestionnaireLookupManager : ManagerBase<Questionnaire_Lookup, ulong>, IQuestionnaireLookupManager
    {
        #region Constructors

        public QuestionnaireLookupManager()
            : base()
        {

        }
        public QuestionnaireLookupManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Implementation


        public IList<FillHealthcareQuestionnaire> GetHealthQuestionLookupListFromServer(string sCategory, DateTime PatientDoB, DateTime AppintmentDate, string UserName, ulong ulEncID, bool sStatus, ulong ulHumanID)
        {

            ArrayList arylstQuestionnaireLookup = null;
            object[] objArrQuestionnaireLookup = null;
            IQuery query;

            IList<FillHealthcareQuestionnaire> fillHealthScreenList = new List<FillHealthcareQuestionnaire>();
            FillHealthcareQuestionnaire fillHealthScreen;
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                query = mySession.GetNamedQuery("Get.Questionnaire.Lookup.Old");
                query.SetParameter(0, ulEncID);
                query.SetParameter(1, sCategory);
                arylstQuestionnaireLookup = new ArrayList(query.List());
                mySession.Close();

            }
            if (arylstQuestionnaireLookup != null)
            {
                for (int i = 0; i < arylstQuestionnaireLookup.Count; i++)
                {
                    objArrQuestionnaireLookup = (object[])arylstQuestionnaireLookup[i];

                    fillHealthScreen = new FillHealthcareQuestionnaire();
                    if(objArrQuestionnaireLookup[0]!= null)
                    fillHealthScreen.HealthCare_Questionnaire_ID = Convert.ToUInt64(objArrQuestionnaireLookup[0].ToString());
                    fillHealthScreen.Encounter_ID = ulEncID; //Added CopyPrev for Questionnaire for BugID:44324 - COPD Screening Feature
                    //fillHealthScreen.Physician_ID = Convert.ToUInt64(objArrQuestionnaireLookup[2].ToString());
                    //fillHealthScreen.Human_ID = Convert.ToUInt64(objArrQuestionnaireLookup[3].ToString());
                    if (objArrQuestionnaireLookup[1] != null)
                    fillHealthScreen.Questionnaire_Lookup_ID = Convert.ToUInt64(objArrQuestionnaireLookup[1].ToString());
                    if (objArrQuestionnaireLookup[2] != null)
                    fillHealthScreen.Questionnaire_Category = objArrQuestionnaireLookup[2].ToString();
                    if (objArrQuestionnaireLookup[3] != null)
                    fillHealthScreen.Questionnaire_Type = objArrQuestionnaireLookup[3].ToString();
                    if (objArrQuestionnaireLookup[4] != null)
                    fillHealthScreen.Question = objArrQuestionnaireLookup[4].ToString();
                    if (objArrQuestionnaireLookup[5] != null)
                    fillHealthScreen.Selected_Option = objArrQuestionnaireLookup[5].ToString();
                    if (objArrQuestionnaireLookup[6] != null)
                    fillHealthScreen.Possible_Options = objArrQuestionnaireLookup[6].ToString();
                    if (objArrQuestionnaireLookup[7] != null)
                    fillHealthScreen.Notes = objArrQuestionnaireLookup[7].ToString();
                    if (objArrQuestionnaireLookup[8] != null)
                    fillHealthScreen.Created_By = objArrQuestionnaireLookup[8].ToString();
                    if (objArrQuestionnaireLookup[9] != null)
                    fillHealthScreen.Created_Date_And_Time = Convert.ToDateTime(objArrQuestionnaireLookup[9].ToString());
                    if (objArrQuestionnaireLookup[10] != null)
                    fillHealthScreen.Modified_By = objArrQuestionnaireLookup[10].ToString();
                    if (objArrQuestionnaireLookup[11] != null)
                    fillHealthScreen.Modified_Date_And_Time = Convert.ToDateTime(objArrQuestionnaireLookup[11].ToString());
                    if (objArrQuestionnaireLookup[12] != null)
                    fillHealthScreen.Version = Convert.ToInt32(objArrQuestionnaireLookup[12].ToString());
                    //fillHealthScreen.UpdateFlag = Convert.ToInt32(objArrQuestionnaireLookup[13].ToString());
                    if (objArrQuestionnaireLookup[14] != null)
                    fillHealthScreen.Normal_Question_Status = objArrQuestionnaireLookup[14].ToString();
                    //fillHealthScreen.Is_Calculation_Required = objArrQuestionnaireLookup[15].ToString();
                    if (objArrQuestionnaireLookup[16] != null)
                    fillHealthScreen.Is_Notes = objArrQuestionnaireLookup[16].ToString();
                    //CAP-3628
                    if (objArrQuestionnaireLookup[17] != null)
                        fillHealthScreen.Controls = objArrQuestionnaireLookup[17].ToString();
                    if (objArrQuestionnaireLookup[18] != null)
                        fillHealthScreen.Question_Loinc_Code = objArrQuestionnaireLookup[18].ToString();
                    if (objArrQuestionnaireLookup[19] != null)
                        fillHealthScreen.Options_Loinc_Code = objArrQuestionnaireLookup[19].ToString();
                    if (objArrQuestionnaireLookup[20] != null
                        && !string.IsNullOrEmpty(objArrQuestionnaireLookup[20].ToString()))
                    {
                        using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                        {
                            string where_Criteria = objArrQuestionnaireLookup[20].ToString();
                            ISQLQuery QuerySQL = iMySession.CreateSQLQuery(where_Criteria.Replace(":Human_ID", ulHumanID.ToString()));
                            ArrayList ArysqlList = new ArrayList(QuerySQL.List());

                            if (ArysqlList != null && ArysqlList.Count > 0)
                            {
                                fillHealthScreen.Where_Criteria = ArysqlList[0].ToString();
                            }
                        }
                    }
                    fillHealthScreenList.Add(fillHealthScreen);

                }
                if (arylstQuestionnaireLookup.Count == 0)
                {
                    QuestionnaireLookupManager questionnaireMngr = new QuestionnaireLookupManager();
                    //CAP-3628
                    //fillHealthScreenList = questionnaireMngr.GetHealthQuestionLookupListFromLocal(sCategory, PatientDoB, AppintmentDate, UserName, ulEncID, sStatus);
                    fillHealthScreenList = questionnaireMngr.GetHealthQuestionLookupListFromLocal(sCategory, PatientDoB, AppintmentDate, UserName, ulEncID, sStatus, ulHumanID);
                }
            }
            return fillHealthScreenList;
        }

        public IList<FillHealthcareQuestionnaire> GetHealthQuestionLookupListFromServercopyprevious(string sCategory, DateTime PatientDoB, DateTime AppintmentDate, string UserName, ulong ulEncID, bool sStatus, ulong ulHumanID)
        {

            ArrayList arylstQuestionnaireLookup = null;
            object[] objArrQuestionnaireLookup = null;
            IQuery query;

            IList<FillHealthcareQuestionnaire> fillHealthScreenList = new List<FillHealthcareQuestionnaire>();
            FillHealthcareQuestionnaire fillHealthScreen;
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                query = mySession.GetNamedQuery("Get.Questionnaire.Lookup.Old.Copyprevious");
                query.SetParameter(0, ulEncID);
                query.SetParameter(1, sCategory);
                query.SetParameter(2, ulEncID);
                query.SetParameter(3, sCategory);
                arylstQuestionnaireLookup = new ArrayList(query.List());
                mySession.Close();

            }
            if (arylstQuestionnaireLookup != null)
            {
                for (int i = 0; i < arylstQuestionnaireLookup.Count; i++)
                {
                    objArrQuestionnaireLookup = (object[])arylstQuestionnaireLookup[i];

                    fillHealthScreen = new FillHealthcareQuestionnaire();
                    if (objArrQuestionnaireLookup[0] != null)
                        fillHealthScreen.HealthCare_Questionnaire_ID = Convert.ToUInt64(objArrQuestionnaireLookup[0].ToString());
                    fillHealthScreen.Encounter_ID = ulEncID; //Added CopyPrev for Questionnaire for BugID:44324 - COPD Screening Feature
                    //fillHealthScreen.Physician_ID = Convert.ToUInt64(objArrQuestionnaireLookup[2].ToString());
                    //fillHealthScreen.Human_ID = Convert.ToUInt64(objArrQuestionnaireLookup[3].ToString());
                    if (objArrQuestionnaireLookup[1] != null)
                        fillHealthScreen.Questionnaire_Lookup_ID = Convert.ToUInt64(objArrQuestionnaireLookup[1].ToString());
                    if (objArrQuestionnaireLookup[2] != null)
                        fillHealthScreen.Questionnaire_Category = objArrQuestionnaireLookup[2].ToString();
                    if (objArrQuestionnaireLookup[3] != null)
                        fillHealthScreen.Questionnaire_Type = objArrQuestionnaireLookup[3].ToString();
                    if (objArrQuestionnaireLookup[4] != null)
                        fillHealthScreen.Question = objArrQuestionnaireLookup[4].ToString();
                    if (objArrQuestionnaireLookup[5] != null)
                        fillHealthScreen.Selected_Option = objArrQuestionnaireLookup[5].ToString();
                    if (objArrQuestionnaireLookup[6] != null)
                        fillHealthScreen.Possible_Options = objArrQuestionnaireLookup[6].ToString();
                    if (objArrQuestionnaireLookup[7] != null)
                        fillHealthScreen.Notes = objArrQuestionnaireLookup[7].ToString();
                    if (objArrQuestionnaireLookup[8] != null)
                        fillHealthScreen.Created_By = objArrQuestionnaireLookup[8].ToString();
                    if (objArrQuestionnaireLookup[9] != null)
                        fillHealthScreen.Created_Date_And_Time = Convert.ToDateTime(objArrQuestionnaireLookup[9].ToString());
                    if (objArrQuestionnaireLookup[10] != null)
                        fillHealthScreen.Modified_By = objArrQuestionnaireLookup[10].ToString();
                    if (objArrQuestionnaireLookup[11] != null)
                        fillHealthScreen.Modified_Date_And_Time = Convert.ToDateTime(objArrQuestionnaireLookup[11].ToString());
                    if (objArrQuestionnaireLookup[12] != null)
                        fillHealthScreen.Version = Convert.ToInt32(objArrQuestionnaireLookup[12].ToString());
                    //fillHealthScreen.UpdateFlag = Convert.ToInt32(objArrQuestionnaireLookup[13].ToString());
                    if (objArrQuestionnaireLookup[14] != null)
                        fillHealthScreen.Normal_Question_Status = objArrQuestionnaireLookup[14].ToString();
                    //fillHealthScreen.Is_Calculation_Required = objArrQuestionnaireLookup[15].ToString();
                    if (objArrQuestionnaireLookup[16] != null)
                        fillHealthScreen.Is_Notes = objArrQuestionnaireLookup[16].ToString();
                    //CAP-3628
                    if (objArrQuestionnaireLookup[17] != null)
                        fillHealthScreen.Controls = objArrQuestionnaireLookup[17].ToString();
                    if (objArrQuestionnaireLookup[18] != null)
                        fillHealthScreen.Question_Loinc_Code = objArrQuestionnaireLookup[18].ToString();
                    if (objArrQuestionnaireLookup[19] != null)
                        fillHealthScreen.Options_Loinc_Code = objArrQuestionnaireLookup[19].ToString();
                    if (objArrQuestionnaireLookup[20] != null
                        && !string.IsNullOrEmpty(objArrQuestionnaireLookup[20].ToString()))
                    {
                        using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                        {
                            string where_Criteria = objArrQuestionnaireLookup[20].ToString();
                            ISQLQuery QuerySQL = iMySession.CreateSQLQuery(where_Criteria.Replace(":Human_ID", ulHumanID.ToString()));
                            ArrayList ArysqlList = new ArrayList(QuerySQL.List());

                            if (ArysqlList != null && ArysqlList.Count > 0)
                            {
                                fillHealthScreen.Where_Criteria = ArysqlList[0].ToString();
                            }
                        }
                    }
                    fillHealthScreenList.Add(fillHealthScreen);

                }
                if (arylstQuestionnaireLookup.Count == 0)
                {
                    QuestionnaireLookupManager questionnaireMngr = new QuestionnaireLookupManager();
                    //CAP-3628
                    //fillHealthScreenList = questionnaireMngr.GetHealthQuestionLookupListFromLocal(sCategory, PatientDoB, AppintmentDate, UserName, ulEncID, sStatus);
                    fillHealthScreenList = questionnaireMngr.GetHealthQuestionLookupListFromLocal(sCategory, PatientDoB, AppintmentDate, UserName, ulEncID, sStatus, ulHumanID);
                }
            }
            return fillHealthScreenList;
        }


        public IList<FillHealthcareQuestionnaire> GetHealthQuestionLookupListFromLocal(string sCategory, DateTime PatientDoB, DateTime AppintmentDate, string UserName, ulong ulEncID, bool sStatus, ulong ulHumanID)
        {

            ArrayList arylstQuestionnaireLookup = null;
            object[] objArrQuestionnaireLookup = null;
            decimal ageInDays;
            string sQuestionType = string.Empty;
            IQuery query;
            IQuery query1;
            IList<FillHealthcareQuestionnaire> fillHealthScreenList = new List<FillHealthcareQuestionnaire>();
            FillHealthcareQuestionnaire fillHealthScreen;
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (PatientDoB != DateTime.MinValue && AppintmentDate != DateTime.MinValue)
                {
                    ageInDays = CalculateAgeInMonths(PatientDoB, AppintmentDate);
                    sQuestionType = ageInDays.ToString();
                }
                if (sStatus == false)
                {

                    query = mySession.GetNamedQuery("Get.Questionnaire.Lookup.New");
                    query.SetString(0, UserName);
                    query.SetString(1, sCategory);
                    arylstQuestionnaireLookup = new ArrayList(query.List());
                }
                else if (sStatus == true)
                {
                    query1 = mySession.GetNamedQuery("Get.Questionnaire.Lookup.model");
                    query1.SetParameter(0, UserName);
                    query1.SetParameter(1, sCategory);
                    query1.SetParameter(2, sQuestionType);
                    //query1.SetParameter(3, sQuestionType);
                    arylstQuestionnaireLookup = new ArrayList(query1.List());

                }

                mySession.Close();

            }
            //string[] Split = sQuestionType.Split(' ');
            //if (sQuestionType!="0")
            //if(sStatus==true)
            //{
            //    query = session.GetISession().GetNamedQuery("Get.Questionnaire.Lookup.New");
            //    if (sQuestionType.Trim() != string.Empty)
            //    {

            //        query.SetParameter(0, UserName);
            //        query.SetParameter(1, sCategory);
            //        query.SetParameter(2, sQuestionType);

            //        arylstQuestionnaireLookup = new ArrayList(query.List());
            //    }
            //}
            //else
            //{
            //    query1 = session.GetISession().GetNamedQuery("Get.Questionnaire.Lookup.model");
            //    //if (sQuestionType.Trim() != string.Empty)
            //    //{
            //        query1.SetParameter(0, UserName);
            //        query1.SetParameter(1, sCategory);
            //        query1.SetParameter(2, sQuestionType + "%");
            //        arylstQuestionnaireLookup = new ArrayList(query1.List());
            //    //}


            //}



            if (arylstQuestionnaireLookup != null)
            {
                for (int i = 0; i < arylstQuestionnaireLookup.Count; i++)
                {
                    objArrQuestionnaireLookup = (object[])arylstQuestionnaireLookup[i];

                    fillHealthScreen = new FillHealthcareQuestionnaire();
                    if (objArrQuestionnaireLookup[0] != null)
                    fillHealthScreen.HealthCare_Questionnaire_ID = Convert.ToUInt64(objArrQuestionnaireLookup[0].ToString());
                    //fillHealthScreen.Encounter_ID = Convert.ToUInt64(objArrQuestionnaireLookup[1].ToString());
                    //fillHealthScreen.Physician_ID = Convert.ToUInt64(objArrQuestionnaireLookup[2].ToString());
                    //fillHealthScreen.Human_ID = Convert.ToUInt64(objArrQuestionnaireLookup[3].ToString());
                    if (objArrQuestionnaireLookup[1] != null)
                    fillHealthScreen.Questionnaire_Lookup_ID = Convert.ToUInt64(objArrQuestionnaireLookup[1].ToString());
                    if (objArrQuestionnaireLookup[2] != null)
                    fillHealthScreen.Questionnaire_Category = objArrQuestionnaireLookup[2].ToString();
                    if (objArrQuestionnaireLookup[3] != null)
                    fillHealthScreen.Questionnaire_Type = objArrQuestionnaireLookup[3].ToString();
                    if (objArrQuestionnaireLookup[4] != null)
                    fillHealthScreen.Question = objArrQuestionnaireLookup[4].ToString();
                    if (objArrQuestionnaireLookup[5] != null)
                    fillHealthScreen.Selected_Option = objArrQuestionnaireLookup[5].ToString();
                    if (objArrQuestionnaireLookup[6] != null)
                    fillHealthScreen.Possible_Options = objArrQuestionnaireLookup[6].ToString();
                    if (objArrQuestionnaireLookup[7] != null)
                    fillHealthScreen.Notes = objArrQuestionnaireLookup[7].ToString();
                    if (objArrQuestionnaireLookup[8] != null)
                    fillHealthScreen.Created_By = objArrQuestionnaireLookup[8].ToString();
                    if (objArrQuestionnaireLookup[9] != null)
                    fillHealthScreen.Created_Date_And_Time = Convert.ToDateTime(objArrQuestionnaireLookup[9].ToString());
                    if (objArrQuestionnaireLookup[10] != null)
                    fillHealthScreen.Modified_By = objArrQuestionnaireLookup[10].ToString();
                    if (objArrQuestionnaireLookup[11] != null)
                    fillHealthScreen.Modified_Date_And_Time = Convert.ToDateTime(objArrQuestionnaireLookup[11].ToString());
                    if (objArrQuestionnaireLookup[12] != null)
                    fillHealthScreen.Version = Convert.ToInt32(objArrQuestionnaireLookup[12].ToString());
                    //fillHealthScreen.UpdateFlag = Convert.ToInt32(objArrQuestionnaireLookup[13].ToString());
                    if (objArrQuestionnaireLookup[14] != null)
                    fillHealthScreen.Normal_Question_Status = objArrQuestionnaireLookup[14].ToString();
                    //fillHealthScreen.Is_Calculation_Required = objArrQuestionnaireLookup[15].ToString();
                    if (objArrQuestionnaireLookup[16] != null)
                    fillHealthScreen.Is_Notes = objArrQuestionnaireLookup[16].ToString();
                    //CAP-3628
                    if (objArrQuestionnaireLookup[17] != null)
                        fillHealthScreen.Controls = objArrQuestionnaireLookup[17].ToString();
                    if (objArrQuestionnaireLookup[18] != null)
                        fillHealthScreen.Question_Loinc_Code = objArrQuestionnaireLookup[18].ToString();
                    if (objArrQuestionnaireLookup[19] != null)
                        fillHealthScreen.Options_Loinc_Code = objArrQuestionnaireLookup[19].ToString();
                    if (objArrQuestionnaireLookup[20] != null
                        && !string.IsNullOrEmpty(objArrQuestionnaireLookup[20].ToString()))
                    {
                        using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                        {
                            string where_Criteria = objArrQuestionnaireLookup[20].ToString();
                            ISQLQuery QuerySQL = iMySession.CreateSQLQuery(where_Criteria.Replace(":Human_ID", ulHumanID.ToString()));
                            ArrayList ArysqlList = new ArrayList(QuerySQL.List());

                            if (ArysqlList != null && ArysqlList.Count > 0)
                            {
                                fillHealthScreen.Where_Criteria = ArysqlList[0].ToString();
                            }
                        }
                    }
                    fillHealthScreenList.Add(fillHealthScreen);

                }
            }
            var finalList = from f in fillHealthScreenList select f;
            return finalList.ToList<FillHealthcareQuestionnaire>();



        }
        public IList<Questionnaire_Lookup> BatchOperationsToQuestionnaireLookup(IList<Questionnaire_Lookup> addList, IList<Questionnaire_Lookup> updateList, IList<Questionnaire_Lookup> deleteList, string sMACAddress, string sUserName, string sQuestion)
        {
            //SaveUpdateDeleteWithTransaction(ref addList, updateList, deleteList, sMACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref addList, ref updateList, deleteList, sMACAddress, false, false, 0, string.Empty);
            return GetQuestionnaireLookup(sUserName);
        }

        public IList<Questionnaire_Lookup> GetQuestionnaireLookup(string UserName)
        {
            IList<Questionnaire_Lookup> HealthQuestionsList = new List<Questionnaire_Lookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Questionnaire_Lookup)).Add(Expression.Eq("User_Name", UserName));
                HealthQuestionsList = crit.List<Questionnaire_Lookup>();
                iMySession.Close();
            }
            return HealthQuestionsList;
        }

        public IList<Questionnaire_Lookup> GetHealthQuestionCategoryList(string UserName)
        {
            IList<Questionnaire_Lookup> healthList = new List<Questionnaire_Lookup>();
            IList<Questionnaire_Lookup> QuestList = new List<Questionnaire_Lookup>();
            Questionnaire_Lookup resultList;
            IList<string> tempList = new List<string>();
            //IList<object> crit = session.GetISession().CreateSQLQuery("Select distinct(q.Questionnaire_Category),q.Is_Ros_Type,q.Sort_Order from questionnaire_lookup q where q.user_name='" + UserName + "' order by Sort_Order desc").List<object>();
            // ISession mySession = NHibernateSessionManager.Instance.CreateISession();

            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IList<object> crit = session.GetISession().CreateSQLQuery("Select distinct(q.Questionnaire_Category),q.Is_Ros_Type from questionnaire_lookup q where q.user_name='" + UserName + "' order by Sort_Order desc").List<object>();
                foreach (object[] obj in crit)
                {
                    resultList = new Questionnaire_Lookup();
                    resultList.Questionnaire_Category = obj[0].ToString();
                    resultList.Is_Ros_Type = obj[1].ToString();
                    healthList.Add(resultList);
                }
                mySession.Close();
            }
            return healthList;
        }

        public IList<FillHealthcareQuestionnaire> GetQuestionTypeList(string UserName)
        {
            FillHealthcareQuestionnaire healthList = new FillHealthcareQuestionnaire();
            IList<FillHealthcareQuestionnaire> healthQuestion = new List<FillHealthcareQuestionnaire>();
            ArrayList arrList = null;
            IQuery iquery = null;
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (UserName != string.Empty)
                {
                    iquery = mySession.GetNamedQuery("Fill.Questionnaire.UserName");
                    iquery.SetParameter(0, UserName);
                }
                arrList = new ArrayList(iquery.List());
                mySession.Close();
            }
            if (arrList.Count > 0)
            {
                foreach (object[] obj in arrList)
                {
                    FillHealthcareQuestionnaire FillHealth = new FillHealthcareQuestionnaire();
                    if (obj[0]!= null)
                    FillHealth.Questionnaire_Type = obj[0].ToString();
                    if (obj[1] != null)
                    FillHealth.Is_Calculation_Required = obj[1].ToString();
                    healthQuestion.Add(FillHealth);
                }
            }

            return healthQuestion;
        }
        public FillHealthcareQuestionnaire GetReviewOfQuestionnarieByEncounterId(ulong encounter_ID, ulong human_ID)
        {
            FillHealthcareQuestionnaire objROQ = new FillHealthcareQuestionnaire();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = mySession.CreateCriteria(typeof(Healthcare_Questionnaire)).Add(Expression.Eq("Encounter_ID", encounter_ID));
                //objROQ.Question_List = criteria.List<Healthcare_Questionnaire>();
                mySession.Close();
            }
            return objROQ;
        }
        public IList<string> GetHealthQuestionRosType(string UserName)
        {
            IList<string> healthList = new List<string>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery crit = mySession.CreateSQLQuery("Select distinct(q.Is_Ros_Type) from questionnaire_lookup q where q.user_name='" + UserName + "'");
                healthList = crit.List<string>();
                mySession.Close();
            }
            return healthList;

        }


        public int CalculateAgeInMonths(DateTime birthDate, DateTime now)
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


            //else
            //{
            //    leap=3
            //}

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




        #endregion
    }
}