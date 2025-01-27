using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.Data;
using System.IO;

namespace Acurus.Capella.DataAccess.ManagerObjects
{

    public partial interface IActivityLogManager : IManagerBase<ActivityLog, uint>
    {
        void SaveActivityLogManager(IList<ActivityLog> SaveActivity, string sMacAddress);
        IList<ActivityLog> GetLogUsingHumanandEncounterID(ulong ulHumanID);
        IList<ActivityLog> GetInboxEntries(string EmailID);
        IList<ActivityLog> GetSentBoxEntries(string EmailID);
        IList<ActivityLog> GetActivityType(List<string> ActivityType);
        IList<ActivityLog> GetActivityByActivityTypeandSubject(string ActivityType, string Subject);
        IList<ActivityLog> GetFaxActivityTypeByStatus();
        IList<ActivityLog> GetFaxActivity(string ActivityLogId);
        IList<ActivityLog> GetActivityLogForEFaxManagement(List<string> ActivityType, ulong uHumanID, string sFaxStatus, string sRecipiantName, string sSenderName, string sFromDate, string sToDate);
    }


    public partial class ActivityLogManager : ManagerBase<ActivityLog, uint>, IActivityLogManager
    {
        #region Constructors

        public ActivityLogManager()
            : base()
        {

        }
        public ActivityLogManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion


        #region Get Methods

        public ArrayList sha2message(string message)
        {
            ArrayList arrList2 = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.sha2.message");
                query.SetString(0, message);
                ArrayList arrList1 = new ArrayList(query.List());
                 arrList2 = arrList1;

                iMySession.Close();
            }

            return arrList2;
        }
        public void SaveActivityLogManager(IList<ActivityLog> SaveActivity, string sMacAddress)
        {
          //  SaveUpdateDeleteWithTransaction(ref SaveActivity, null, null, sMacAddress);
            IList<ActivityLog> updateList = null;
            IList<ActivityLog> deleteList = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref SaveActivity, ref updateList, deleteList, sMacAddress, false, false, SaveActivity[0].Encounter_ID, string.Empty);
        }

        public void UpdateActivityLogManager(IList<ActivityLog> updateList)
        {
            //  SaveUpdateDeleteWithTransaction(ref SaveActivity, null, null, sMacAddress);
            IList<ActivityLog> SaveActivity = null;
            IList<ActivityLog> deleteList = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref SaveActivity, ref updateList, deleteList, string.Empty, false, false, updateList[0].Encounter_ID, string.Empty);
        }
        public IList<ActivityLog> GetLogUsingHumanandEncounterID(ulong ulHumanID)
        {
            IList<ActivityLog> ilstActivitylog = new List<ActivityLog>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(ActivityLog)).Add(Expression.Eq("Human_ID", ulHumanID));
                ilstActivitylog = criteria.List<ActivityLog>();
                iMySession.Close();
            }
            return ilstActivitylog;
        }


        public IList<ActivityLog> GetInboxEntries(string MailID)
        {
            IList<ActivityLog> ActivityList = new List<ActivityLog>();
            //using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            //{
            //    ActivityLog resultList;
            //    IQuery query;
            //    ArrayList arylstActivityLog = null;
            //    object[] obj = null;
            
            //    query = iMySession.GetNamedQuery("Get.InboxEntriesInActivtiLog");
            //    query.SetParameter(0, MailID);

            //    arylstActivityLog = new ArrayList(query.List());
            //    for (int i = 0; i < arylstActivityLog.Count; i++)
            //    {
            //        obj = (object[])arylstActivityLog[i];
            //        resultList = new ActivityLog();
            //        resultList.Sent_To = obj[0].ToString();
            //        resultList.Subject = obj[1].ToString();
            //        resultList.Activity_Date_And_Time = Convert.ToDateTime(obj[2].ToString());
            //        resultList.From_Address = obj[3].ToString();
            //        resultList.Message = obj[4].ToString();
            //        ActivityList.Add(resultList);
            //    }
            //    iMySession.Close();
            //}

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //SELECT Sent_To,Subject,Cast(Activity_Date_And_Time as char(100)),From_Address,Message FROM activity_log where (Activity_Type='Send' or Activity_Type='Send Message' or Activity_Type like '%transmitted%') and Sent_To=?;
                ISQLQuery sql = iMySession.CreateSQLQuery("Select p.* from activity_log p where (p.Activity_Type='Send' or p.Activity_Type='Send Message' or p.Activity_Type like '%transmitted%') and p.Sent_To in ('" + MailID + "') order by Activity_Date_And_Time desc").AddEntity("p", typeof(ActivityLog));
                ActivityList = sql.List<ActivityLog>();
                iMySession.Close();
            }

            return ActivityList;

        }

        public IList<ActivityLog> GetSentBoxEntries(string EmailID)
        {
            IList<ActivityLog> ActivityList = new List<ActivityLog>();
            //using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            //{
            //    ActivityLog resultList;
            //    IQuery query;
            //    ArrayList arylstActivityLog = null;
            //    object[] obj = null;
            
            //    query = iMySession.GetNamedQuery("Get.SentBoxEntriesInActivtiLog");
            //    query.SetParameter(0, EmailID);

            //    arylstActivityLog = new ArrayList(query.List());
            //    for (int i = 0; i < arylstActivityLog.Count; i++)
            //    {
            //        obj = (object[])arylstActivityLog[i];
            //        resultList = new ActivityLog();
            //        resultList.Sent_To = obj[0].ToString();
            //        resultList.Subject = obj[1].ToString();
            //        resultList.Activity_Date_And_Time = Convert.ToDateTime(obj[2].ToString());
            //        resultList.From_Address = obj[3].ToString();
            //        resultList.Message = obj[4].ToString();
            //        ActivityList.Add(resultList);
            //    }
            //    iMySession.Close();
            //}

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select p.* from activity_log p where (p.Activity_Type='Send' or p.Activity_Type='Send Message' or p.Activity_Type like '%transmitted%') and p.From_Address in ('" + EmailID + "') order by Activity_Date_And_Time desc").AddEntity("p", typeof(ActivityLog));
                ActivityList = sql.List<ActivityLog>();
                iMySession.Close();
            }
            return ActivityList;

        }
        public int GetGroupID()
        {
            int iGroupID = 0; 
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select Max(a.Group_ID) from activity_log a");
                ArrayList arr = new ArrayList(sql.List());
                if(arr.Count>0)
                {
                    iGroupID = Convert.ToInt32(arr[0]);
                    iGroupID = iGroupID + 1;
                }
                iMySession.Close();
            }
            return iGroupID;
        }
        public IList<PhysicianLibrary> ilstPhysicianLibraryforGetMailID(ulong uPhysicianID)
        {
            IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select p.* from physician_library p where p.Physician_Library_ID='" + uPhysicianID + "'").AddEntity("p", typeof(PhysicianLibrary));
                ilstPhysicianLibrary = sql.List<PhysicianLibrary>();
                iMySession.Close();
            }
            return ilstPhysicianLibrary;
        }

        public IList<Human> ilstHumanMail(ulong uHumanID)
        {
            IList<Human> ilstHuman = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select h.* from human h where h.Human_ID='" + uHumanID + "'").AddEntity("h", typeof(Human));
                ilstHuman = sql.List<Human>();
                iMySession.Close();
            }
            return ilstHuman;
        }

        public IList<ActivityLog> GetActivityType(List<string> ActivityType)
        {
            IList<ActivityLog> ilstActivitylog = new List<ActivityLog>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(ActivityLog)).Add(Expression.In("Activity_Type", ActivityType));
                ilstActivitylog = criteria.List<ActivityLog>();
                iMySession.Close();
            }
            return ilstActivitylog;
        }
        public IList<ActivityLog> GetActivityByActivityTypeandSubject(string ActivityType, string Subject)
        {
            IList<ActivityLog> ilstActivitylog = new List<ActivityLog>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(ActivityLog)).Add(Expression.Eq("Activity_Type", ActivityType)).Add(Expression.Eq("Subject", Subject));
                ilstActivitylog = criteria.List<ActivityLog>();
                iMySession.Close();
            }
            return ilstActivitylog;
        }
        public IList<ActivityLog> GetActivityTypeByusername(List<string> ActivityType,string username, DateTime? activityDateTime = null)
        {
            IList<ActivityLog> ilstActivitylog = new List<ActivityLog>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(ActivityLog)).Add(Expression.In("Activity_Type", ActivityType)).Add(Expression.Eq("Activity_By",username));
                //CAP-1831 - eFax Outbox - Introduce filter 
                if (activityDateTime != null)
                {
                    criteria.Add(Expression.Ge("Activity_Date_And_Time", activityDateTime));
                }
                ilstActivitylog = criteria.List<ActivityLog>();
                iMySession.Close();
            }
            return ilstActivitylog;
        }
        public IList<ActivityLog> GetActivityLogForEFaxManagement(List<string> ActivityType ,ulong uHumanID,string sFaxStatus, string sRecipiantName, string sSenderName, string sFromDate, string sToDate)
        {
            IList<ActivityLog> ilstActivitylog = new List<ActivityLog>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                string query = "Select a.* from activity_log a where a.Activity_Type in (" + string.Join(",", ActivityType.ToArray()) + ") and date(a.Activity_Date_And_Time) between'" + sFromDate + "' and '" + sToDate + "'" + ((sFaxStatus == "ALL") ? "" : "and a.Fax_Status='" + sFaxStatus + "'");
                if (uHumanID != 0)
                {
                    query = query + "and a.Human_ID = '" + uHumanID + "'";
                }

                if (sRecipiantName != "" && sRecipiantName.Split(',').Length <= 1)
                {
                    query = query + "and a.Fax_Recipient_Name like'%"+ sRecipiantName.Split(',')[0] + "%'";
                }
                if (sRecipiantName != "" && sRecipiantName.Split(',').Length > 1)
                {
                    query = query + "and a.Fax_Recipient_Name like'%" + sRecipiantName.Split(',')[0] + "%' and a.Fax_Recipient_Name like'%" + sRecipiantName.Split(',')[1].Replace(".","") + "%'";
                }

                //SenderName
                if (sSenderName != "" && sSenderName.Split(',').Length <= 1)
                {
                    query = query + "and a.Fax_Sender_Name like'%" + sSenderName.Split(',')[0] + "%'";
                }
                if (sSenderName != "" && sSenderName.Split(',').Length > 1)
                {
                    query = query + "and a.Fax_Sender_Name like'%" + sSenderName.Split(',')[0] + "%' and a.Fax_Sender_Name like'%" + sSenderName.Split(',')[1].Replace(".", "") + "%'";
                }

                ISQLQuery sql = iMySession.CreateSQLQuery(query).AddEntity("a", typeof(ActivityLog));
                ilstActivitylog = sql.List<ActivityLog>();
                iMySession.Close();
            }
            return ilstActivitylog;
        }
        public IList<ActivityLog> CheckActivityExists(List<string> ActivityType,List<ulong> Encounter)
        {
            IList<ActivityLog> ilstActivitylog = new List<ActivityLog>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(ActivityLog)).Add(Expression.In("Activity_Type", ActivityType)).Add(Expression.In("Encounter_ID", Encounter));
                ilstActivitylog = criteria.List<ActivityLog>();
                iMySession.Close();
            }
            return ilstActivitylog;
        }
        public IList<ActivityLog> GetFaxActivityTypeByStatus()
        {
            IList<ActivityLog> ilstActivitylog = new List<ActivityLog>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(ActivityLog)).Add(Expression.Eq("Activity_Type", "EFAX")).Add(Expression.Eq("Fax_Status", "READY TO SEND"));
                ilstActivitylog = criteria.List<ActivityLog>();
                iMySession.Close();
            }
            return ilstActivitylog;
        }
        public IList<ActivityLog> GetFaxActivity(string ActivityLogId)
        {
            IList<ActivityLog> ilstActivitylog = new List<ActivityLog>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(ActivityLog)).Add(Expression.Eq("Activity_Type", "EFAX")).Add(Expression.Eq("Id",Convert.ToUInt64(ActivityLogId)));
                ilstActivitylog = criteria.List<ActivityLog>();
                iMySession.Close();
            }
            return ilstActivitylog;
        }
        #endregion
    }
}
