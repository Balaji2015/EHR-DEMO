using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.Criterion;
using System;
using Acurus.Capella.Core.DTO;
using System.Linq;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public interface IUserLookupManager : IManagerBase<UserLookup, uint>
    {

        IList<UserLookup> GetFieldLookupList(string userName, string Field_Name);
        IList<UserLookup> GetFieldLookupListforPartialField(string userName, string PartialField_Name);
        IList<UserLookup> GetFieldLookupList(string userName, string Field_Name, string sOrder);
        void UpdateLookups(UserLookup lookups, string sMacAddress);
        void DeleteLookups(UserLookup lookups, string sMacAddress);
        ulong SaveLookups(UserLookup lookups); //prabu 14/04/10
        IList<UserLookup> GetFieldLookupListInfoForAll(string userName, ulong Physician_Id, string[] Field_Name);
        void DeleteLookupsQuery(string userName, string[] Field_Lookup);
        void SaveUserLookup(UserLookup UserLookup);
       // int SaveUpdateDelete(IList<UserLookup> SaveList, IList<UserLookup> UpdateList, IList<UserLookup> DeleteList, ISession Mysess);
        //int SaveintoUserLookup(IList<UserLookup> SaveUserLookup, IList<UserLookup> UpdateUserLookup, IList<UserLookup> DeleteUserLookup, ISession Mysession);
        //IList<PendingDownlaods> SaveUpdateLocallookup(IList<UserRelatedModification> objUserRelated, IList<LookupLastModified> objLookLast);
        FillLookupDetails SaveUpdateDeleteUserLookup(IList<UserLookup> SaveUserLookup, IList<UserLookup> UpdateUserLookup, IList<UserLookup> DeleteUserLookup, string sLastModified, string sMacAddress);

        IList<UserLookup> SaveUpdateDeleteManageCDSS(IList<UserLookup> SaveUserLookup, IList<UserLookup> UpdateUserLookup, IList<UserLookup> DeleteUserLookup, string UserName, string FiledName);
        IList<UserLookup> GetFieldLookupListForRCM(string userName, string Field_Name, string sOrder, string IsActive);
        IList<UserLookup> GetUserLookupById(ulong Id);
        IList<UserLookup> GetFieldLookupListRCM(string UserName, string Field_Name, string IsActive);
        string GetMaxValue(string FieldName, string UserName);
        IList<UserLookup> GetFieldLookupListforPartialField(ulong Physician_ID, string PartialField_Name, string sex);
        IList<UserLookup> GetFieldLookupListByFieldNameandDescription(string userName, string Field_Name, string Description);
    }
    public partial class UserLookupManager : ManagerBase<UserLookup, uint>, IUserLookupManager
    {

        #region Constructors

        public UserLookupManager()
            : base()
        {

        }
        public UserLookupManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Implementation

        public void DeleteLookupsQuery(string userName, string[] Field_Name)
        {
            IList<UserLookup> UserLookupList = new List<UserLookup>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.In("Field_Name", Field_Name)).Add(Expression.Eq("User_Name", userName));
                UserLookupList = criteria.List<UserLookup>();
                iMySession.Close();
            }
            //IList<UserLookup> nulllist = null;
           // IList<UserLookup> UpdateList = null;
            //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
            //SaveUpdateDeleteWithTransaction(ref nulllist, null, UserLookupList, "");


        }

        public void SaveUserLookup(UserLookup UserLookup)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Insert into User_lookup select max(user_lookup_id)+1,'" + UserLookup.Field_Name.Replace("'", "''") + "','" + UserLookup.Value + "','" + UserLookup.Description + "','" + UserLookup.Sort_Order + "','" + UserLookup.Default_Value + "','" + UserLookup.User_Name + "'");
                sql.ExecuteUpdate();
                iMySession.Close();
            }
        }

        public void SaveFieldLookupList(IList<UserLookup> UserLookup)
        {
            string insertdata = string.Empty;
           // IList<UserLookup> UpdateList = null;
            //IList<UserLookup> DeleteList = null;
            if (UserLookup != null)
            {
                //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
                //SaveUpdateDeleteWithTransaction(ref UserLookup, null, null, "");

            }
        }

        public void UpdateLookups(UserLookup lookups, string sMacAddress)
        {
            IList<UserLookup> objUserLookup = new List<UserLookup>();
            //IList<UserLookup> objUserLookupSave = null;
            //IList<UserLookup> DeleteList = null;
            objUserLookup.Add(lookups);
            //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
            //SaveUpdateDeleteWithTransaction(ref objUserLookupSave, objUserLookup, null, sMacAddress);   
        }

        public void DeleteLookups(UserLookup lookups, string sMacAddress)
        {
            IList<UserLookup> objUserLookup = new List<UserLookup>();
            //IList<UserLookup> objUserLookupSave = null;
            //IList<UserLookup> UpdateList = null;
            objUserLookup.Add(lookups);
            //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
            //SaveUpdateDeleteWithTransaction(ref objUserLookupSave, null, objUserLookup, sMacAddress);

        }

        public ulong SaveLookups(UserLookup lookups)
        {
            IList<UserLookup> objUserLookup = new List<UserLookup>();
            //IList<UserLookup> UpdateList = null;
            //IList<UserLookup> DeleteList = null;
            objUserLookup.Add(lookups);
            //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
            //SaveUpdateDeleteWithTransaction(ref objUserLookup, null, null,"");
            return 0;
        }
        public IList<UserLookup> GetFieldLookupListInfoForAll(string userName, ulong Physician_Id, string[] Field_Name)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<UserLookup> UserLookup = new List<UserLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (userName == string.Empty)
                {
                    ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("Physician_ID", Physician_Id)).Add(Expression.In("Field_Name", Field_Name));
                    UserLookup = criteria.List<UserLookup>();
                }
                else
                {
                    ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("User_Name", userName)).Add(Expression.In("Field_Name", Field_Name));
                    UserLookup = criteria.List<UserLookup>();
                }
                iMySession.Close();
            }
            return UserLookup;
        }

        public IList<UserLookup> GetFieldLookupList(string userName, string Field_Name)
        {
            //  ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<UserLookup> userList = new List<UserLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("User_Name", userName)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper()));
                //return criteria.List<UserLookup>();
                userList = criteria.List<UserLookup>();
                iMySession.Close();
            }
            return GetResultsBySpecificOrder(userList);
        }
        public IList<UserLookup> GetFieldLookupListNotes(string userName, string Field_Name, string value)
        {
            //  ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<UserLookup> userList = new List<UserLookup>();
            try
            {

                string _value = "%" + value.Trim() + "%";
                //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

                //using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                //{
                ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("User_Name", userName)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Like("Value", _value.ToUpper())).AddOrder(Order.Asc("Sort_Order"));

                //return criteria.List<UserLookup>();
                //userList =
                userList = criteria.List<UserLookup>();
                iMySession.Close();

            }

            catch 
            {

            }

            return userList;
        }


        public IList<UserLookup> GetFieldLookupList(string Field_Name)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<UserLookup> userList = new List<UserLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).AddOrder(Order.Asc("Sort_Order"));
                userList = criteria.List<UserLookup>();
                iMySession.Close();
            }
            return userList;
        }

        public IList<UserLookup> GetFieldLookupListforPartialField(string userName, string PartialField_Name)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<UserLookup> userList = new List<UserLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("User_Name", userName)).Add(Expression.Like("Field_Name", PartialField_Name.ToUpper(), MatchMode.Start)).AddOrder(Order.Asc("Sort_Order"));
                userList = criteria.List<UserLookup>();
                iMySession.Close();
            }
            return userList;
        }
        //BugID:54702
        public IList<UserLookup> GetFieldLookupListforPartialField(ulong Physician_ID, string PartialField_Name, string sex)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<UserLookup> userList = new List<UserLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Restrictions.Eq("Physician_ID", Physician_ID)).Add(Restrictions.Like("Field_Name", PartialField_Name.ToUpper(), MatchMode.Start)).Add(!Restrictions.Eq("Description", sex.ToUpper())).AddOrder(Order.Asc("Sort_Order"));
                userList = criteria.List<UserLookup>();
                iMySession.Close();
            }
            return userList;
        }
        public IList<UserLookup> GetFieldLookupList(string userName, string Field_Name, string sOrder)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<UserLookup> userList = new List<UserLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("User_Name", userName)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).AddOrder(Order.Asc("Sort_Order")).AddOrder(Order.Asc("Value"));
                userList = criteria.List<UserLookup>();
                iMySession.Close();
            }
            return GetResultsBySpecificOrder(userList);//Selvamani - 26/03/2012            
        }
        //not in use
        //public int SaveUpdateDelete(IList<UserLookup> SaveList, IList<UserLookup> UpdateList, IList<UserLookup> DeleteList, ISession Mysess)
        //{
        //    int iResult = 0;
        //    GenerateXml XMLObj = new GenerateXml();
        //    //iResult = SaveUpdateDeleteWithoutTransaction(ref SaveList, UpdateList, DeleteList, Mysess, "");
        //    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveList, ref UpdateList, DeleteList, Mysess, string.Empty, false, false, 0, string.Empty, ref XMLObj);
        //    return iResult;
        //}

        public IList<UserLookup> GetFieldLookup(ulong Physician_ID, string Field_Name, string value)
        {
            IList<UserLookup> userList = new List<UserLookup>();
            try
            {

                string _value = "%" + value.Trim() + "%";
                //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

                //using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                //{
                ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("Physician_ID", Physician_ID)).Add(Expression.Eq("Field_Name", Field_Name)).Add(Expression.Like("Value", _value.ToUpper())).AddOrder(Order.Asc("Sort_Order"));
                //return criteria.List<UserLookup>();
                //userList =
                userList = criteria.List<UserLookup>();
                iMySession.Close();

            }

            catch 
            {

            }

            return userList;
            //}
            //return GetResultsBySpecificOrder(userList);//Selvamani - 23/05/2012      
        }


        public IList<UserLookup> GetFieldLookupList(ulong Physician_ID, string Field_Name)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<UserLookup> userList = new List<UserLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("Physician_ID", Physician_ID)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).AddOrder(Order.Asc("Sort_Order"));
                //return criteria.List<UserLookup>();
                userList = criteria.List<UserLookup>();
                iMySession.Close();
            }
            return GetResultsBySpecificOrder(userList);//Selvamani - 23/05/2012      
        }

        public IList<UserLookup> GetFieldLookupList(ulong Physician_ID, string Field_Name, string sOrder)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<UserLookup> userList = new List<UserLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("Physician_ID", Physician_ID)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).AddOrder(Order.Asc(sOrder));
                userList = criteria.List<UserLookup>();
                iMySession.Close();
            }
            return GetResultsBySpecificOrder(userList);//Selvamani - 26/03/2012            
        }

        public IList<UserLookup> GetFieldLookupList(ulong Physician_ID, string Field_Name, string Sex, string sOrder)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<UserLookup> UserLookupList = new List<UserLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Restrictions.Eq("Physician_ID", Physician_ID)).Add(Restrictions.Eq("Field_Name", Field_Name.ToUpper())).Add(!Restrictions.Eq("Description", Sex.ToUpper())).AddOrder(Order.Asc(sOrder));
                UserLookupList = criteria.List<UserLookup>();
                iMySession.Close();
            }
            return UserLookupList;
        }


        //Selvamani - 26/03/2012
        private IList<UserLookup> GetResultsBySpecificOrder(IList<UserLookup> userLookup)
        {
            //return criteria.List<UserLookup>().Any(a => a.Sort_Order == 0) ? criteria.List<UserLookup>().Where(u => u.Sort_Order != 0).ToList().Concat(criteria.List<UserLookup>().Where(u => u.Sort_Order == 0).OrderBy(b => b.Value)).ToList() : criteria.List<UserLookup>();
            return userLookup.Any(a => a.Sort_Order == 0) ? userLookup.Where(u => u.Sort_Order != 0).ToList().Concat(userLookup.Where(u => u.Sort_Order == 0).OrderBy(b => b.Value).ToList()).ToList() : userLookup.ToList();
        }
        //$

        #endregion
        public IList<UserLookup> GetFieldLookupListByFieldNameandDescription(string userName, string Field_Name, string Description)
        {
            //  ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<UserLookup> userList = new List<UserLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("User_Name", userName)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Description", Description.ToUpper()));
                //return criteria.List<UserLookup>();
                userList = criteria.List<UserLookup>();
                iMySession.Close();
            }
            return GetResultsBySpecificOrder(userList);
        }
        #region IUserLookupManager Members

        //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
        //public int SaveintoUserLookup(IList<UserLookup> SaveUserLookup, IList<UserLookup> UpdateUserLookup, IList<UserLookup> DeleteUserLookup, ISession Mysession)
        //{
        //    return SaveUpdateDeleteWithoutTransaction(ref SaveUserLookup, UpdateUserLookup, DeleteUserLookup, Mysession, "");

        //}

        #endregion

        #region IUserLookupManager Members


        //public IList<PendingDownlaods> SaveUpdateLocallookup(IList<UserRelatedModification> objUserRelated, IList<LookupLastModified> objLookLast)
        //{
        //    IList<PendingDownlaods> PendingLst = new List<PendingDownlaods>();
        //    PendingDownlaods objPending = null;
        //    IList<UserLookup> SaveUserLookup = new List<UserLookup>();
        //    IList<UserLookup> UpdateUserLookup = new List<UserLookup>();
        //    IList<UserLookup> DeleteUserLookup = new List<UserLookup>();

        //    IList<PhysicianICD_9> SavePhyICD = new List<PhysicianICD_9>();
        //    IList<PhysicianICD_9> UpdatePhyICD = new List<PhysicianICD_9>();
        //    IList<PhysicianICD_9> DeletePhyICD = new List<PhysicianICD_9>();

        //    IList<PhysicianProcedure> SavePhyProcedure = new List<PhysicianProcedure>();
        //    IList<PhysicianProcedure> updatePhyProcedure = new List<PhysicianProcedure>();
        //    IList<PhysicianProcedure> DeletePhyProcedure = new List<PhysicianProcedure>();

        //    IList<TemplateMaster> saveTemplateMaster = new List<TemplateMaster>();
        //    IList<TemplateMaster> updateTemplateMaster = new List<TemplateMaster>();
        //    IList<TemplateMaster> deleteTemplateMaster = new List<TemplateMaster>();

        //    IList<TemplatePhrases> saveTemplatePhrases = new List<TemplatePhrases>();
        //    IList<TemplatePhrases> updateTemplatePhrases = new List<TemplatePhrases>();
        //    IList<TemplatePhrases> deleteTemplatePhrases = new List<TemplatePhrases>();

        //    var UsrLst = (from l in objUserRelated group l by new { l.Table_Name, l.Transaction_Type } into TableType select new { TableName = TableType.Key.Table_Name, TransactionType = TableType.Key.Transaction_Type }).ToArray();

        //    for (int i = 0; i < UsrLst.Count(); i++)
        //    {
        //        switch (UsrLst[i].TableName.ToUpper())
        //        {
        //            case "USER_LOOKUP":
        //                {
        //                    switch (UsrLst[i].TransactionType.ToUpper())
        //                    {
        //                        case "INSERT":
        //                            {
        //                                SaveUserLookup = FillUserLookup((from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>());
        //                                break;
        //                            }
        //                        case "UPDATE":
        //                            {
        //                                UpdateUserLookup = FillUserLookup((from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>());
        //                                break;
        //                            }
        //                        case "DELETE":
        //                            {
        //                                IList<UserRelatedModification> DeleteLookup = (from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>();
        //                                if (DeleteLookup != null && DeleteLookup.Count > 0)
        //                                {
        //                                    UserLookup objUserLookup = null;
        //                                    foreach (UserRelatedModification u in DeleteLookup)
        //                                    {
        //                                        IList<UserLookup> LookUpList = new List<UserLookup>();
        //                                        //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //                                        using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //                                        {
        //                                            ICriteria crt = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("Id", u.User_Lookup_ID));
        //                                            LookUpList = crt.List<UserLookup>();
        //                                            iMySession.Close();
        //                                        }
        //                                        if (LookUpList.Count > 0)
        //                                        {
        //                                            objUserLookup = new UserLookup();
        //                                            objUserLookup.Id = u.User_Lookup_ID;
        //                                            objUserLookup.Field_Name = u.Field_Name;
        //                                            objUserLookup.User_Name = u.User_Name;
        //                                            objUserLookup.Value = u.Value;
        //                                            objUserLookup.Description = u.Description;
        //                                            objUserLookup.Sort_Order = u.Sort_Order;
        //                                            objUserLookup.Default_Value = u.Default_Value;
        //                                            objUserLookup.Is_Physician = u.Is_Physician;
        //                                            objUserLookup.Doc_Type = u.Doc_Type;
        //                                            objUserLookup.Doc_Sub_Type = u.Doc_Sub_Type;
        //                                            objUserLookup.Physician_ID = u.Physician_ID;
        //                                            objUserLookup.Created_By = u.Created_By;
        //                                            objUserLookup.Created_Date_And_Time = u.Created_Date_And_Time;
        //                                            objUserLookup.Modified_By = u.Modified_By;
        //                                            objUserLookup.Modified_Date_And_Time = u.Modified_Date_And_Time;
        //                                            DeleteUserLookup.Add(objUserLookup);
        //                                        }
        //                                    }
        //                                }
        //                                break;
        //                            }
        //                        default:
        //                            break;
        //                    }
        //                    break;

        //                }
        //            case "PHYSICIAN_PROCEDURE":
        //                {
        //                    switch (UsrLst[i].TransactionType.ToUpper())
        //                    {
        //                        case "INSERT":
        //                            {
        //                                SavePhyProcedure = FillPhysicianProcedureLst((from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>());
        //                                break;
        //                            }
        //                        case "UPDATE":
        //                            {
        //                                updatePhyProcedure = FillPhysicianProcedureLst((from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>());
        //                                break;
        //                            }
        //                        case "DELETE":
        //                            {
        //                                IList<UserRelatedModification> DeleteProcedure = (from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>();
        //                                if (DeleteProcedure != null && DeleteProcedure.Count > 0)
        //                                {
        //                                    PhysicianProcedure objPhyProcedure = null;
        //                                    foreach (UserRelatedModification u in DeleteProcedure)
        //                                    {
        //                                        IList<PhysicianProcedure> PhyProcList = new List<PhysicianProcedure>();
        //                                        //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //                                        using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //                                        {
        //                                            ICriteria crt = iMySession.CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Id", u.Physician_Procedure_ID));
        //                                            PhyProcList = crt.List<PhysicianProcedure>();
        //                                            iMySession.Close();
        //                                        }
        //                                        if (PhyProcList.Count > 0)
        //                                        {
        //                                            objPhyProcedure = new PhysicianProcedure();
        //                                            objPhyProcedure.Id = u.Physician_Procedure_ID;
        //                                            objPhyProcedure.Physician_ID = u.Physician_ID;
        //                                            objPhyProcedure.Physician_Procedure_Code = u.Physician_Procedure_Code;
        //                                            objPhyProcedure.Procedure_Description = u.Procedure_Code_Description;
        //                                            objPhyProcedure.Procedure_Type = u.Procedure_Type;
        //                                            objPhyProcedure.Lab_ID = u.Lab_ID;
        //                                            objPhyProcedure.Created_By = u.Created_By;
        //                                            objPhyProcedure.Created_Date_And_Time = u.Created_Date_And_Time;
        //                                            objPhyProcedure.Modified_By = u.Modified_By;
        //                                            objPhyProcedure.Modified_Date_And_Time = u.Modified_Date_And_Time;
        //                                            objPhyProcedure.Version = u.Version;
        //                                            objPhyProcedure.Sort_Order = Convert.ToUInt32(u.Sort_Order);
        //                                            DeletePhyProcedure.Add(objPhyProcedure);
        //                                        }
        //                                    }

        //                                }
        //                                break;
        //                            }
        //                        default:
        //                            break;
        //                    }
        //                    break;
        //                }
        //            case "PHYSICIAN_ICD":
        //                {
        //                    switch (UsrLst[i].TransactionType.ToUpper())
        //                    {
        //                        case "INSERT":
        //                            {
        //                                SavePhyICD = FillPhysicianICDLst((from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>());
        //                                break;
        //                            }
        //                        case "UPDATE":
        //                            {
        //                                UpdatePhyICD = FillPhysicianICDLst((from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>());
        //                                break;
        //                            }
        //                        case "DELETE":
        //                            {
        //                                IList<UserRelatedModification> DeleteICD = (from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>();
        //                                if (DeleteICD != null && DeleteICD.Count > 0)
        //                                {
        //                                    PhysicianICD_9 objPhyICD = null;
        //                                    foreach (UserRelatedModification u in DeleteICD)
        //                                    {
        //                                        objPhyICD = new PhysicianICD_9();
        //                                        IList<PhysicianICD_9> PhyICDList = new List<PhysicianICD_9>();
        //                                        //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //                                        using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //                                        {
        //                                            ICriteria crt = iMySession.CreateCriteria(typeof(PhysicianICD_9)).Add(Expression.Eq("ICD_9", u.Physician_ICD)).Add(Expression.Eq("Physician_ID", u.Physician_ID)).Add(Expression.Eq("Parent_ICD_9", u.Parent_ICD)).Add(Expression.Eq("Is_Ruled_Out", u.Is_Ruled_Out));
        //                                            PhyICDList = crt.List<PhysicianICD_9>();
        //                                            iMySession.Close();
        //                                        }
        //                                        if (PhyICDList.Count > 0)
        //                                        {
        //                                            objPhyICD.Physician_ID = u.Physician_ID;
        //                                            objPhyICD.ICD_9 = u.Physician_ICD;
        //                                            objPhyICD.Parent_ICD_9 = u.Parent_ICD;
        //                                            objPhyICD.ICD_9_Description = u.ICD_Description;
        //                                            objPhyICD.Leaf_Node = u.Leaf_Node;
        //                                            objPhyICD.HCC_Category = u.HCC_Category;
        //                                            objPhyICD.Is_Delete = u.Is_Delete;
        //                                            objPhyICD.Version_Year = u.Version_Year;
        //                                            objPhyICD.Is_Ruled_Out = u.Is_Ruled_Out;
        //                                            objPhyICD.ICD_Category = u.ICD_Category;
        //                                            objPhyICD.Created_By = u.Created_By;
        //                                            objPhyICD.Created_Date_And_Time = u.Created_Date_And_Time;
        //                                            objPhyICD.Modified_By = u.Modified_By;
        //                                            objPhyICD.Modified_Date_And_Time = u.Modified_Date_And_Time;
        //                                            objPhyICD.Version = u.Version;
        //                                            objPhyICD.Physician_ICD_Sort_Order = u.Physician_ICD_Sort_Order;
        //                                            DeletePhyICD.Add(objPhyICD);
        //                                        }
        //                                    }
        //                                }
        //                                break;
        //                            }
        //                        default:
        //                            break;
        //                    }
        //                    break;
        //                }
        //            case "TEMPLATE_MASTER":
        //                {
        //                    switch (UsrLst[i].TransactionType.ToUpper())
        //                    {
        //                        case "INSERT":
        //                            {
        //                                saveTemplateMaster = FillTemplateMasterLst((from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>());
        //                                break;
        //                            }
        //                        case "UPDATE":
        //                            {
        //                                updateTemplateMaster = FillTemplateMasterLst((from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>());
        //                                break;
        //                            }
        //                        case "DELETE":
        //                            {
        //                                IList<UserRelatedModification> deleteMaster = (from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>();
        //                                if (deleteMaster != null && deleteMaster.Count > 0)
        //                                {
        //                                    TemplateMaster objTemplateMaster = null;
        //                                    foreach (UserRelatedModification u in deleteMaster)
        //                                    {
        //                                        IList<TemplateMaster> PhyICDList = new List<TemplateMaster>();
        //                                        //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //                                        using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //                                        {
        //                                            ICriteria crt = iMySession.CreateCriteria(typeof(TemplateMaster)).Add(Expression.Eq("Id", u.Template_Master_ID));
        //                                            PhyICDList = crt.List<TemplateMaster>();
        //                                            iMySession.Close();
        //                                        }
        //                                        if (PhyICDList.Count > 0)
        //                                        {
        //                                            objTemplateMaster = new TemplateMaster();
        //                                            objTemplateMaster.Id = u.Template_Master_ID;
        //                                            objTemplateMaster.Template_Name = u.Template_Name;
        //                                            objTemplateMaster.User_Name = u.User_Name;
        //                                            objTemplateMaster.Created_By = u.Created_By;
        //                                            objTemplateMaster.Created_Date_And_Time = u.Created_Date_And_Time;
        //                                            objTemplateMaster.Modified_By = u.Modified_By;
        //                                            objTemplateMaster.Modified_Date_And_Time = u.Modified_Date_And_Time;
        //                                            //objTemplateMaster.Version = u.Version;
        //                                            deleteTemplateMaster.Add(objTemplateMaster);
        //                                        }
        //                                    }
        //                                }
        //                                break;
        //                            }
        //                        default:
        //                            break;
        //                    }
        //                    break;
        //                }
        //            case "TEMPLATE_PHRASES":
        //                {
        //                    switch (UsrLst[i].TransactionType.ToUpper())
        //                    {
        //                        case "INSERT":
        //                            {
        //                                saveTemplatePhrases = FillTemplatePhrasesLst((from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>());
        //                                break;
        //                            }
        //                        case "UPDATE":
        //                            {
        //                                updateTemplatePhrases = FillTemplatePhrasesLst((from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>());
        //                                break;
        //                            }
        //                        case "DELETE":
        //                            {
        //                                IList<UserRelatedModification> deletePhrases = (from us in objUserRelated where us.Transaction_Type.ToUpper() == UsrLst[i].TransactionType.ToUpper() && us.Table_Name.ToUpper() == UsrLst[i].TableName.ToUpper() select us).ToList<UserRelatedModification>();
        //                                if (deletePhrases != null && deletePhrases.Count > 0)
        //                                {
        //                                    TemplatePhrases objTemplatePhrases = null;
        //                                    foreach (UserRelatedModification u in deletePhrases)
        //                                    {
        //                                        IList<TemplatePhrases> TemplatePhrasesList = new List<TemplatePhrases>();
        //                                        //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //                                        using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //                                        {
        //                                            ICriteria crt = iMySession.CreateCriteria(typeof(TemplatePhrases)).Add(Expression.Eq("Id", u.Template_Phrases_ID));
        //                                            TemplatePhrasesList = crt.List<TemplatePhrases>();
        //                                            iMySession.Close();
        //                                        }
        //                                        if (TemplatePhrasesList.Count > 0)
        //                                        {
        //                                            objTemplatePhrases = new TemplatePhrases();
        //                                            objTemplatePhrases.Id = u.Template_Phrases_ID;
        //                                            objTemplatePhrases.Template_Master_ID = u.Template_Phrases_ID;
        //                                            objTemplatePhrases.Context = u.Context;
        //                                            objTemplatePhrases.Phrases = u.Phrases;
        //                                            objTemplatePhrases.User_Name = u.User_Name;
        //                                            objTemplatePhrases.Created_By = u.Created_By;
        //                                            objTemplatePhrases.Created_Date_And_Time = u.Created_Date_And_Time;
        //                                            objTemplatePhrases.Modified_By = u.Modified_By;
        //                                            objTemplatePhrases.Modified_Date_And_Time = u.Modified_Date_And_Time;
        //                                            // objTemplatePhrases.Version = u.Version;
        //                                            deleteTemplatePhrases.Add(objTemplatePhrases);
        //                                        }
        //                                    }
        //                                }
        //                                break;
        //                            }
        //                        default:
        //                            break;
        //                    }
        //                    break;
        //                }
        //            default:
        //                break;
        //        }
        //    }
        //    int iTryCount = 0;
        //TryAgain:
        //    int iResult = 0;
        //    Session.GetISession().Clear();
        //    ISession MySession = Session.GetISession();
        //    //ITransaction trans = null;
        //    try
        //    {
        //        using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
        //        {
        //            try
        //            {
        //                //trans = MySession.BeginTransaction();
        //                if ((SaveUserLookup != null && SaveUserLookup.Count != 0) || (UpdateUserLookup != null && UpdateUserLookup.Count != 0) || (DeleteUserLookup != null && DeleteUserLookup.Count > 0))
        //                {
        //                    GenerateXml XMLObj = new GenerateXml();
        //                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveUserLookup, ref UpdateUserLookup, DeleteUserLookup, MySession, string.Empty, false, false, 0, string.Empty, ref XMLObj);

        //                    //iResult = SaveUpdateDeleteWithoutTransaction(ref SaveUserLookup, UpdateUserLookup, DeleteUserLookup, MySession, "");
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
        //                        // MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                    objPending = new PendingDownlaods();
        //                    // objPending.Computer_Name = sComputer_Name;
        //                    objPending.List_of_Tables = "user_lookup";
        //                    PendingLst.Add(objPending);
        //                }


        //                if ((SavePhyICD != null && SavePhyICD.Count != 0) || (UpdatePhyICD != null && UpdatePhyICD.Count != 0) || (DeletePhyICD != null && DeletePhyICD.Count > 0))
        //                {
        //                    PhysicianICD_9Manager objPhysicianICDMgr = new PhysicianICD_9Manager();
        //                    iResult = objPhysicianICDMgr.SaveintoPhysicianICD(SavePhyICD, UpdatePhyICD, DeletePhyICD, MySession);
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
        //                        // MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                    objPending = new PendingDownlaods();
        //                    // objPending.Computer_Name = sComputer_Name;
        //                    objPending.List_of_Tables = "physician_icd";
        //                    PendingLst.Add(objPending);
        //                }

        //                if ((SavePhyProcedure != null && SavePhyProcedure.Count != 0) || (updatePhyProcedure != null && updatePhyProcedure.Count != 0) || (DeletePhyProcedure != null && DeletePhyProcedure.Count > 0))
        //                {
        //                    PhysicianProcedureManager objPhysicianProcedureMgr = new PhysicianProcedureManager();
        //                    iResult = objPhysicianProcedureMgr.SaveintoPhysicianProcedure(SavePhyProcedure, updatePhyProcedure, DeletePhyProcedure, MySession);
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
        //                        // MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                    objPending = new PendingDownlaods();
        //                    // objPending.Computer_Name = sComputer_Name;
        //                    objPending.List_of_Tables = "physician_procedure";
        //                    PendingLst.Add(objPending);
        //                }

        //                if ((saveTemplateMaster != null && saveTemplateMaster.Count != 0) || (updateTemplateMaster != null && updateTemplateMaster.Count != 0) || (deleteTemplateMaster != null && deleteTemplateMaster.Count > 0))
        //                {
        //                    TemplateMasterManager objTemplateMasterMgr = new TemplateMasterManager();
        //                    iResult = objTemplateMasterMgr.SaveintoTemplateMaster(saveTemplateMaster, updateTemplateMaster, deleteTemplateMaster, MySession);

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
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                    objPending = new PendingDownlaods();
        //                    objPending.List_of_Tables = "template_master";
        //                    PendingLst.Add(objPending);
        //                }

        //                if ((saveTemplatePhrases != null && saveTemplatePhrases.Count != 0) || (updateTemplatePhrases != null && updateTemplatePhrases.Count != 0) || (deleteTemplatePhrases != null && deleteTemplatePhrases.Count > 0))
        //                {
        //                    TemplatePhrasesManager objTemplatePhrasesMgr = new TemplatePhrasesManager();
        //                    iResult = objTemplatePhrasesMgr.SaveintoTemplatePhrases(saveTemplatePhrases, updateTemplatePhrases, deleteTemplatePhrases, MySession);

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
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                    objPending = new PendingDownlaods();
        //                    objPending.List_of_Tables = "template_phrases";
        //                    PendingLst.Add(objPending);
        //                }

        //                if (objLookLast != null && objLookLast.Count != 0)
        //                {
        //                    LookupLastModifiedManager objLookupModifiedMgr = new LookupLastModifiedManager();
        //                    iResult = objLookupModifiedMgr.UpdateintoLookupLastModidied(objLookLast, MySession);
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
        //                        // MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                }

        //                MySession.Flush();
        //                trans.Commit();
        //            }
        //            catch (NHibernate.Exceptions.GenericADOException ex)
        //            {
        //                trans.Rollback();
        //                MySession.Close();
        //                throw new Exception(ex.Message);
        //            }
        //            catch (Exception e)
        //            {
        //                trans.Rollback();
        //                MySession.Close();
        //                throw new Exception(e.Message);
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        //MySession.Close();
        //        throw new Exception(Ex.Message);
        //    }
        //    return PendingLst;
        //}

        //Local Function Call

        //IList<UserLookup> FillUserLookup(IList<UserRelatedModification> UserRelatedLst)
        //{
        //    IList<UserLookup> UserLookLst = new List<UserLookup>();
        //    UserLookup objUserLookup = null;
        //    foreach (UserRelatedModification u in UserRelatedLst)
        //    {
        //        objUserLookup = new UserLookup();
        //        objUserLookup.Id = u.User_Lookup_ID;
        //        objUserLookup.Field_Name = u.Field_Name;
        //        objUserLookup.User_Name = u.User_Name;
        //        objUserLookup.Value = u.Value;
        //        objUserLookup.Description = u.Description;
        //        objUserLookup.Sort_Order = u.Sort_Order;
        //        objUserLookup.Default_Value = u.Default_Value;
        //        objUserLookup.Is_Physician = u.Is_Physician;
        //        objUserLookup.Doc_Type = u.Doc_Type;
        //        objUserLookup.Doc_Sub_Type = u.Doc_Sub_Type;
        //        objUserLookup.Physician_ID = u.Physician_ID;
        //        objUserLookup.Created_By = u.Created_By;
        //        objUserLookup.Created_Date_And_Time = u.Created_Date_And_Time;
        //        objUserLookup.Modified_By = u.Modified_By;
        //        objUserLookup.Modified_Date_And_Time = u.Modified_Date_And_Time;
        //        UserLookLst.Add(objUserLookup);
        //    }
        //    return UserLookLst;
        //}

        //IList<PhysicianProcedure> FillPhysicianProcedureLst(IList<UserRelatedModification> UserRelatedLst)
        //{
        //    IList<PhysicianProcedure> PhyProcedureLst = new List<PhysicianProcedure>();
        //    PhysicianProcedure objPhyProcedure = null;
        //    foreach (UserRelatedModification u in UserRelatedLst)
        //    {
        //        objPhyProcedure = new PhysicianProcedure();
        //        objPhyProcedure.Id = u.Physician_Procedure_ID;
        //        objPhyProcedure.Physician_ID = u.Physician_ID;
        //        objPhyProcedure.Physician_Procedure_Code = u.Physician_Procedure_Code;
        //        objPhyProcedure.Procedure_Description = u.Procedure_Code_Description;
        //        objPhyProcedure.Procedure_Type = u.Procedure_Type;
        //        objPhyProcedure.Lab_ID = u.Lab_ID;
        //        objPhyProcedure.Created_By = u.Created_By;
        //        objPhyProcedure.Created_Date_And_Time = u.Created_Date_And_Time;
        //        objPhyProcedure.Modified_By = u.Modified_By;
        //        objPhyProcedure.Modified_Date_And_Time = u.Modified_Date_And_Time;
        //        objPhyProcedure.Order_Group_Name = u.Order_Group_Name;
        //        objPhyProcedure.Version = u.Version;
        //        objPhyProcedure.Sort_Order = Convert.ToUInt32(u.Sort_Order);
        //        PhyProcedureLst.Add(objPhyProcedure);
        //    }
        //    return PhyProcedureLst;
        //}

        //IList<PhysicianICD_9> FillPhysicianICDLst(IList<UserRelatedModification> UserRelatedLst)
        //{
        //    IList<PhysicianICD_9> PhyICDLst = new List<PhysicianICD_9>();
        //    PhysicianICD_9 objPhyICD = null;
        //    foreach (UserRelatedModification u in UserRelatedLst)
        //    {
        //        objPhyICD = new PhysicianICD_9();
        //        objPhyICD.Physician_ID = u.Physician_ID;
        //        objPhyICD.ICD_9 = u.Physician_ICD;
        //        objPhyICD.Parent_ICD_9 = u.Parent_ICD;
        //        objPhyICD.ICD_9_Description = u.ICD_Description;
        //        objPhyICD.Leaf_Node = u.Leaf_Node;
        //        objPhyICD.HCC_Category = u.HCC_Category;
        //        objPhyICD.Is_Delete = u.Is_Delete;
        //        objPhyICD.ICD_Category = u.ICD_Category;
        //        objPhyICD.Version_Year = u.Version_Year;
        //        objPhyICD.Is_Ruled_Out = u.Is_Ruled_Out;
        //        objPhyICD.Created_By = u.Created_By;
        //        objPhyICD.Created_Date_And_Time = u.Created_Date_And_Time;
        //        objPhyICD.Modified_By = u.Modified_By;
        //        objPhyICD.Modified_Date_And_Time = u.Modified_Date_And_Time;
        //        objPhyICD.Version = u.Version;
        //        objPhyICD.Physician_ICD_Sort_Order = u.Physician_ICD_Sort_Order;
        //        PhyICDLst.Add(objPhyICD);
        //    }
        //    return PhyICDLst;
        //}

        //IList<TemplateMaster> FillTemplateMasterLst(IList<UserRelatedModification> UserRelatedLst)
        //{
        //    IList<TemplateMaster> templateMasterLst = new List<TemplateMaster>();
        //    TemplateMaster objTemplateMaster = null;
        //    foreach (UserRelatedModification u in UserRelatedLst)
        //    {
        //        objTemplateMaster = new TemplateMaster();
        //        objTemplateMaster.Id = u.Template_Master_ID;
        //        objTemplateMaster.Template_Name = u.Template_Name;
        //        objTemplateMaster.User_Name = u.User_Name;
        //        objTemplateMaster.Created_By = u.Created_By;
        //        objTemplateMaster.Created_Date_And_Time = u.Created_Date_And_Time;
        //        objTemplateMaster.Modified_By = u.Modified_By;
        //        objTemplateMaster.Modified_Date_And_Time = u.Modified_Date_And_Time;
        //        //objTemplateMaster.Version = u.Version;
        //        templateMasterLst.Add(objTemplateMaster);
        //    }
        //    return templateMasterLst;
        //}

        //IList<TemplatePhrases> FillTemplatePhrasesLst(IList<UserRelatedModification> UserRelatedLst)
        //{
        //    IList<TemplatePhrases> templatePhrasesLst = new List<TemplatePhrases>();
        //    TemplatePhrases objTemplatePhrases = null;
        //    foreach (UserRelatedModification u in UserRelatedLst)
        //    {
        //        objTemplatePhrases = new TemplatePhrases();
        //        objTemplatePhrases.Id = u.Template_Phrases_ID;
        //        objTemplatePhrases.Template_Master_ID = u.Template_Master_ID;
        //        objTemplatePhrases.Context = u.Context;
        //        objTemplatePhrases.Phrases = u.Phrases;
        //        objTemplatePhrases.User_Name = u.User_Name;
        //        objTemplatePhrases.Created_By = u.Created_By;
        //        objTemplatePhrases.Created_Date_And_Time = u.Created_Date_And_Time;
        //        objTemplatePhrases.Modified_By = u.Modified_By;
        //        objTemplatePhrases.Modified_Date_And_Time = u.Modified_Date_And_Time;
        //        //objTemplatePhrases.Version = u.Version;
        //        templatePhrasesLst.Add(objTemplatePhrases);
        //    }
        //    return templatePhrasesLst;
        //}

        #endregion

        #region IUserLookupManager Members

        public FillLookupDetails SaveUpdateDeleteUserLookup(IList<UserLookup> SaveUserLookup, IList<UserLookup> UpdateUserLookup, IList<UserLookup> DeleteUserLookup, string sLastModified, string sMacAddress)
        {
            FillLookupDetails objFillLookup = new FillLookupDetails();
            //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
            // SaveUpdateDeleteWithTransaction(ref SaveUserLookup, UpdateUserLookup, DeleteUserLookup, "");
            //UserRelatedModificationManager objUserRelatedModificationMgr = new UserRelatedModificationManager();
            //objFillLookup.User_Related = objUserRelatedModificationMgr.GetAllLookupTablesFromServer(sLastModified, sMacAddress);
            objFillLookup.UTC_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");
            // objFillLookup.UTC_Time = objFillLookup.User_Related.Count > 0 ? objFillLookup.User_Related.Max(s => s.Last_Modified_Time).ToString("yyyy-MM-dd HH:mm:ss") : System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");

            return objFillLookup;
        }


        public IList<UserLookup> SaveUpdateDeleteManageCDSS(IList<UserLookup> SaveUserLookup, IList<UserLookup> UpdateUserLookup, IList<UserLookup> DeleteUserLookup, string UserName, string FiledName)
        {
            //SaveUpdateDeleteWithTransaction(ref SaveUserLookup, UpdateUserLookup, DeleteUserLookup, string.Empty);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref SaveUserLookup, ref UpdateUserLookup, DeleteUserLookup, string.Empty, false, false, 0, string.Empty);
            IList<UserLookup> UserLookupList = new List<UserLookup>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("Field_Name", FiledName)).Add(Expression.Eq("User_Name", UserName));
                UserLookupList = crit.List<UserLookup>();
                iMySession.Close();
            }
            return UserLookupList;
        }

        public IList<UserLookup> GetFieldLookupListForRCM(string userName, string Field_Name, string sOrder, string IsActive)
        {
            IList<UserLookup> UserLookupList = new List<UserLookup>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("User_Name", userName)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).AddOrder(Order.Asc("Sort_Order")).Add(Expression.Eq("Is_Active", "Y"));
                UserLookupList = criteria.List<UserLookup>();
                iMySession.Close();
            }
            return GetResultsBySpecificOrder(UserLookupList);
        }
        public IList<UserLookup> GetUserLookupById(ulong Id)
        {
            IList<UserLookup> UserLookupList = new List<UserLookup>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("Id", Id));
                UserLookupList = criteria.List<UserLookup>();
                iMySession.Close();
            }
            return GetResultsBySpecificOrder(UserLookupList);
        }


        public IList<UserLookup> GetFieldLookupListRCM(string UserName, string Field_Name, string IsActive)
        {
            IList<UserLookup> UserLookupList = new List<UserLookup>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("User_Name", UserName)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Is_Active", "Y")).AddOrder(Order.Asc("Sort_Order"));
                UserLookupList = criteria.List<UserLookup>();
                iMySession.Close();
            }
            //return criteria.List<UserLookup>();
            return GetResultsBySpecificOrder(UserLookupList);//Selvamani - 23/05/2012      
        }
        public string GetMaxValue(string FieldName, string UserName)
        {
            string SortOrder = string.Empty;
            ArrayList arySortOrder = new ArrayList();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("GetMaxSortordFromUserLookup");
                query.SetParameter(0, FieldName);
                query.SetParameter(1, UserName);
                arySortOrder = new ArrayList(query.List());
                iMySession.Close();
            }
            if (arySortOrder.Count != 0)
            {
                SortOrder = arySortOrder[0].ToString();
            }
            return SortOrder;
        }
        #endregion
    }
}
