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
using System.Web.Security;
using System.Xml.Linq;

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
        IList<object> GetActivityLogForEFaxManagement(List<string> ActivityType, ulong uHumanID, string sFaxStatus, string sRecipiantName, string sSenderName, string sFromDate, string sToDate, string sLegal_Org);
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
        public IList<object> GetActivityLogForEFaxManagement(List<string> ActivityType, ulong uHumanID, string sFaxStatus, string sRecipiantName, string sSenderName, string sFromDate, string sToDate, string sLegal_Org)
        {
            IList<object> objlistActivityManagementlst = new List<object>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //string query = "Select a.* from activity_log a where a.Activity_Type in (" + string.Join(",", ActivityType.ToArray()) + ") and date(a.Activity_Date_And_Time) between'" + sFromDate + "' and '" + sToDate + "'" + ((sFaxStatus == "ALL") ? "" : "and a.Fax_Status='" + sFaxStatus + "'");
                string query = "select a.Activity_Log_ID, a.Human_ID, a.Encounter_ID, a.Activity_Type, a.Activity_Date_And_Time, a.Sent_To, a.Subject, a.Message, a.From_Address, a.Role, a.Encrypted_Message, a.Activity_By, a.Fax_Sender_Name, a.Fax_Recipient_Name, a.Fax_Sender_Company, a.Fax_Recipient_Company, a.Fax_Sender_Number, a.Fax_Recipient_Number, a.Fax_File_Path, a.Fax_Status, a.Fax_Recipient_Category, a.Fax_Priority, a.Fax_Cover_Page_Template_Name, a.Error_Description, a.Is_Pdf_Moved, a.Group_ID, a.Fax_Sent_File_Path,ifnull(CONCAT(H.Prefix,' ',H.First_Name,' ',H.MI,' ',H.Last_Name,' ',H.Suffix),'') as Patient_Name,ifnull(H.Legal_Org,'') as Legal_Org from activity_log a  left join human H on (a.human_id= H.human_id) where a.Activity_Type in (" + string.Join(",", ActivityType.ToArray()) + ") and date(a.Activity_Date_And_Time) between'" + sFromDate + "' and '" + sToDate + "'" + ((sFaxStatus == "ALL") ? "" : "and a.Fax_Status='" + sFaxStatus + "'") + " and H.Legal_Org='" + sLegal_Org + "'";
                if (uHumanID != 0)
                {
                    query = query + "and a.Human_ID = '" + uHumanID + "'";
                }

                //if (sRecipiantName != "" && sRecipiantName.Split(',').Length <= 1)
                //{
                //    query = query + "and a.Fax_Recipient_Name like'%" + sRecipiantName.Split(',')[0] + "%'";
                //}
                //if (sRecipiantName != "" && sRecipiantName.Split(',').Length > 1)
                //{
                //    query = query + "and a.Fax_Recipient_Name like'%" + sRecipiantName.Split(',')[0] + "%' and a.Fax_Recipient_Name like'%" + sRecipiantName.Split(',')[1].Replace(".", "") + "%'";
                //}
                if (sRecipiantName != "")
                {
                    query = query + "and a.Fax_Recipient_Name = '" + sRecipiantName + "'";
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

                ISQLQuery sql1 = iMySession.CreateSQLQuery(query);
                var listActivityManagement = new
                {
                    Activity_Log_ID = string.Empty,
                    Human_ID = string.Empty,
                    Encounter_ID = string.Empty,
                    Activity_Type = string.Empty,
                    Activity_Date_And_Time = string.Empty,
                    Sent_To = string.Empty,
                    Subject = string.Empty,
                    Message = string.Empty,
                    From_Address = string.Empty,
                    Role = string.Empty,
                    Encrypted_Message = string.Empty,
                    Activity_By = string.Empty,
                    Fax_Sender_Name = string.Empty,
                    Fax_Recipient_Name = string.Empty,
                    Fax_Sender_Company = string.Empty,
                    Fax_Recipient_Company = string.Empty,
                    Fax_Sender_Number = string.Empty,
                    Fax_Recipient_Number = string.Empty,
                    Fax_File_Path = string.Empty,
                    Fax_Status = string.Empty,
                    Fax_Recipient_Category = string.Empty,
                    Fax_Priority = string.Empty,
                    Fax_Cover_Page_Template_Name = string.Empty,
                    Error_Description = string.Empty,
                    Is_Pdf_Moved = string.Empty,
                    Group_ID = string.Empty,
                    Fax_Sent_File_Path = string.Empty,
                    Patient_Name = string.Empty,
                    Legal_Org = string.Empty
                };


                foreach (object[] oj in sql1.List())
                {
                    listActivityManagement = new
                    {
                        Activity_Log_ID = oj[0].ToString(),
                        Human_ID = oj[1].ToString(),
                        Encounter_ID = oj[2].ToString(),
                        Activity_Type = oj[3].ToString(),
                        Activity_Date_And_Time = oj[4].ToString(),
                        Sent_To = oj[5].ToString(),
                        Subject = oj[6].ToString(),
                        Message = oj[7].ToString(),
                        From_Address = oj[8].ToString(),
                        Role = oj[9].ToString(),
                        Encrypted_Message = oj[10].ToString(),
                        Activity_By = oj[11].ToString(),
                        Fax_Sender_Name = oj[12].ToString(),
                        Fax_Recipient_Name = oj[13].ToString(),
                        Fax_Sender_Company = oj[14].ToString(),
                        Fax_Recipient_Company = oj[15].ToString(),
                        Fax_Sender_Number = oj[16].ToString(),
                        Fax_Recipient_Number = oj[17].ToString(),
                        Fax_File_Path = oj[18].ToString(),
                        Fax_Status = oj[19].ToString(),
                        Fax_Recipient_Category = oj[20].ToString(),
                        Fax_Priority = oj[21].ToString(),
                        Fax_Cover_Page_Template_Name = oj[22].ToString(),
                        Error_Description = oj[23].ToString(),
                        Is_Pdf_Moved = oj[24].ToString(),
                        Group_ID = oj[25].ToString(),
                        Fax_Sent_File_Path = oj[26].ToString(),
                        Patient_Name = oj[27].ToString(),
                        Legal_Org = oj[28].ToString()
                    };
                    objlistActivityManagementlst.Add(listActivityManagement);
                }
                iMySession.Close();


            }
            return objlistActivityManagementlst;
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
