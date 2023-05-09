using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.Reflection;
using System.Data;
using System.IO;
using System.Net;
using System.Xml;
using System.Diagnostics;
using System.Web.Services.Protocols;
using System.Web;
using System.Threading;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Xml.Serialization;
namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public interface IManagerBase<T, TKey> : IDisposable
    {
        // Get Methods
        T GetById(TKey Id);
        IList<T> GetAll();
        //IList<T> GetAll(int maxResults);
        IList<T> GetByCriteria(params ICriterion[] criterionList);
        //IList<T> GetByCriteria(int maxResults, params ICriterion[] criterionList);
        T GetUniqueByCriteria(params ICriterion[] criterionList);
        IList<T> GetByExample(T exampleObject, params string[] excludePropertyList);
        IList<T> GetByQuery(string query);
        //IList<T> GetByQuery(string query);
        T GetUniqueByQuery(string query);

        // Misc Methods
        void SetFetchMode(string associationPath, FetchMode mode);
        ICriteria CreateCriteria();

        // CRUD Methods
        //object Save(T entity);
        //void SaveOrUpdate(T entity);
        //void Delete(T entity);
        //void Update(T entity);

        //IList<T> Save(IList<T> entity);
        //void SaveOrUpdate(IList<T> entity);
        //void Delete(IList<T> entity);
        //void Update(IList<T> entity);

        //        void Refresh(T entity);
        //        void Evict(T entity);
        //        IList<object> Save(IList<T> entity);
        //        void SaveOrUpdate(IList<T> entity);
        //        void Delete(IList<T> entity);
        //        void Update(IList<T> entity);

        //        void Delete(IList<T> entityList, ISession session);
        //void Update(IList<T> entityList, ISession session);
        //IList<object> Save(IList<T> entityList, ISession session);

        //void SaveUpdateDelete(IList<T> saveList, IList<T> updateList, IList<T> deleteList);
        //void SaveUpdateDeleteWithTransaction(ref IList<T> saveList, ref IList<T> updateList, IList<T> deleteList, string MACAddress);
        //int SaveUpdateDeleteWithoutTransaction(ref IList<T> saveList, ref IList<T> updateList, IList<T> deleteList, ISession MySession, string MACAddress);
        //void SaveUpdateDelete(T saveObj, T updateObj, T delObj);

        // Properties
        System.Type Type { get; }
        INHibernateSession Session { get; }
    }

    public abstract partial class ManagerBase<T, TKey> : IManagerBase<T, TKey>
    {
        #region Declarations
        IList<string> ilstHumanXml = new List<string>();

        protected INHibernateSession session;
        //protected int defaultMaxResults = 25;// Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["NHibernate_defaultMaxResults"].ToString());

        private bool _disposed = false;
        private Dictionary<string, FetchMode> _fetchModeMap = new Dictionary<string, FetchMode>();

        #endregion

        #region Constructors

        public ManagerBase()
            : this(NHibernateSessionManager.Instance.Session) { }
        public ManagerBase(INHibernateSession session)
        {
            this.session = session;
            this.session.IncrementRefCount();
        }

        ~ManagerBase()
        {
            Dispose(true);
        }

        #endregion

        #region Get Methods

        public virtual T GetById(TKey id)
        {
            return (T)Session.GetISession().Get(typeof(T), id);
        }

        public IList<T> GetAll()
        {
            return GetByCriteria();
        }
        //public IList<T> GetAll(int maxResults)
        //{
        //    return GetByCriteria(maxResults);
        //}

        public int GetCount(string entityName)
        {
            int rowCount = 0;
            ICriteria criteria = Session.GetISession().CreateCriteria(entityName);
            criteria.SetProjection(Projections.RowCount());
            IList result = criteria.List();
            if (result.Count > 0)
                rowCount = (int)result[0];
            return rowCount;
        }

        public IList<T> GetLimit(int startIndex, int maxSize, string entityName)
        {
            ICriteria criteria = Session.GetISession().CreateCriteria(entityName);
            criteria.SetFirstResult(startIndex).SetMaxResults(maxSize);
            return criteria.List<T>();
        }

        //public IList<T> GetByCriteria(params ICriterion[] criterionList)
        //{
        //    return GetByCriteria( criterionList);
        //}
        public IList<T> GetByCriteria(params ICriterion[] criterionList)
        {

            ICriteria criteria = CreateCriteria();  //.SetMaxResults(maxResults);

            foreach (ICriterion criterion in criterionList)
                criteria.Add(criterion);

            IList<T> list = criteria.List<T>();


            return list;
        }
        public IList<T> GetByCriteria(int maxResults, int pageNumber, ICriteria criteria)
        {
            if (criteria.List<T>().Count >= maxResults)
            {
                criteria.SetMaxResults(maxResults);
                criteria.SetFirstResult((pageNumber - 1) * maxResults);
            }

            IList<T> list = criteria.List<T>();

            return list;
        }

        public T GetUniqueByCriteria(params ICriterion[] criterionList)
        {
            ICriteria criteria = CreateCriteria();

            foreach (ICriterion criterion in criterionList)
                criteria.Add(criterion);

            return criteria.UniqueResult<T>();
        }

        public IList<T> GetByExample(T exampleObject, params string[] excludePropertyList)
        {
            ICriteria criteria = CreateCriteria();
            Example example = Example.Create(exampleObject);

            foreach (string excludeProperty in excludePropertyList)
                example.ExcludeProperty(excludeProperty);

            criteria.Add(example);

            return criteria.List<T>();
        }

        //public IList<T> GetByQuery(string query)
        //{
        //    return GetByQuery( query);
        //}
        public IList<T> GetByQuery(string query)
        {
            IQuery iQuery = Session.GetISession().CreateQuery(query); // .SetMaxResults(maxResults);
            return iQuery.List<T>();
        }

        public IList<T> GetByQuery(int maxResults, int pageNumber, string query)
        {
            IQuery iQuery = Session.GetISession().CreateQuery(query).SetMaxResults(maxResults);
            iQuery.SetFirstResult(maxResults * pageNumber);
            return iQuery.List<T>();
        }

        public T GetUniqueByQuery(string query)
        {
            IQuery iQuery = Session.GetISession().CreateQuery(query);
            return iQuery.UniqueResult<T>();

        }

        #endregion

        #region Misc Methods

        public void SetFetchMode(string associationPath, FetchMode mode)
        {
            if (!_fetchModeMap.ContainsKey(associationPath))
                _fetchModeMap.Add(associationPath, mode);
        }

        public ICriteria CreateCriteria()
        {
            ICriteria criteria = Session.GetISession().CreateCriteria(typeof(T));

            foreach (var pair in _fetchModeMap)
                criteria = criteria.SetFetchMode(pair.Key, pair.Value);

            return criteria;
        }

        #endregion

        #region CRUD Methods

        private object Save(T entity)
        {
            return Session.GetISession().Save(entity);
        }
        private void SaveOrUpdate(T entity)
        {
            Session.GetISession().SaveOrUpdate(entity);
        }
        private IList<object> Save(IList<T> entityList)
        {
            IList<object> savedObjects = new List<object>();
            ISession session = Session.GetISession();
            ITransaction trans = null;
            try
            {
                trans = session.BeginTransaction();
                foreach (T entity in entityList)
                {
                    object obj = session.Save(entity);
                    savedObjects.Add(obj);
                }
                session.Flush();
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
                session.Close();
            }
            return savedObjects;
        }

        private void SaveOrUpdate(IList<T> entityList)
        {
            ISession session = Session.GetISession();
            ITransaction trans = null;
            try
            {
                trans = session.BeginTransaction();
                foreach (T entity in entityList)
                {
                    session.SaveOrUpdate(entity);
                }
                session.Flush();
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
                session.Close();
            }
        }

        private void Delete(IList<T> entityList)
        {
            ISession session = Session.GetISession();
            ITransaction trans = null;
            try
            {
                trans = session.BeginTransaction();
                foreach (T entity in entityList)
                {
                    session.Delete(entity);
                }
                session.Flush();
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
                session.Close();
            }
        }

        private void Update(IList<T> entityList)
        {
            ISession session = Session.GetISession();
            ITransaction trans = null;
            try
            {
                trans = session.BeginTransaction();
                foreach (T entity in entityList)
                {
                    session.Update(entity);
                }
                session.Flush();
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
                session.Close();
            }
        }

        private void Delete(T entity)
        {
            Session.GetISession().Delete(entity);
        }
        private void Update(T entity)
        {
            Session.GetISession().Update(entity);
        }

        private void Refresh(T entity)
        {
            Session.GetISession().Refresh(entity);
        }
        private void Evict(T entity)
        {
            Session.GetISession().Evict(entity);
        }

        private void Delete(IList<T> entityList, ISession session)
        {
            foreach (T entity in entityList)
            {
                session.Delete(entity);
            }
        }
        private void Update(IList<T> entityList, ISession session)
        {
            for (int i = 0; i < entityList.Count; i++)
            {
                entityList[i] = (T)session.Merge(entityList[i]);
            }
        }

        private IList<object> Save(IList<T> entityList, ISession session)
        {
            IList<object> savedObjects = new List<object>();
            foreach (T entity in entityList)
            {
                object obj = session.Save(entity);
                savedObjects.Add(obj);
            }
            return savedObjects;
        }

        int iTryCount = 0;

        public void SaveUpdateDeleteWithTransaction(ref IList<T> saveList, IList<T> updateList, IList<T> deleteList, string MACAddress)
        {
            iTryCount = 0;

        TryAgain:
            ISession session = Session.GetISession();

            NHibernateSessionUtility.Instance.MySession = session;
            NHibernateSessionUtility.Instance.MACAddress = MACAddress;
            try
            {
                using (ITransaction trans = session.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        //trans = session.BeginTransaction(IsolationLevel.ReadUncommitted);

                        if (deleteList != null)
                        {
                            Delete(deleteList, session);
                        }

                        if (saveList != null)
                        {
                            Save(saveList, session);
                        }

                        if (updateList != null)
                        {
                            Update(updateList, session);
                        }

                        session.Flush();
                        //Added by Jisha for Delete AuditLog 
                        if (deleteList != null)
                        {
                            if (NHibernateSessionUtility.Instance.MyAuditLogList.Count > 0)
                            {
                                if (typeof(T).AssemblyQualifiedName.ToString().Contains("Acurus.Capella.Core.DomainObjects.AuditLog") == false)
                                {
                                    AuditLogManager auditLogManager = new AuditLogManager();

                                    // auditLogManager.AppendToAuditLog(NHibernateSessionUtility.Instance.MyAuditLogList, session, NHibernateSessionUtility.Instance.MACAddress);

                                    IDbCommand command = session.Connection.CreateCommand();
                                    command.Connection = session.Connection;

                                    trans.Enlist(command);

                                    for (int k = 0; k < NHibernateSessionUtility.Instance.MyAuditLogList.Count; k++)
                                    {
                                        AuditLog AuditLogRecord = NHibernateSessionUtility.Instance.MyAuditLogList[k];
                                        command.CommandText = "insert into audit_log values (0,'" + AuditLogRecord.Human_ID.ToString() + "','" + AuditLogRecord.Entity_Name + "','" +
                                            AuditLogRecord.Transaction_Type + "','" + AuditLogRecord.Transaction_By + "','" +
                                            AuditLogRecord.Transaction_Date_And_Time.ToString("yyyy-MM-dd hh:mm:ss") + "','" + AuditLogRecord.Entity_Id.ToString() + "','" +
                                            AuditLogRecord.Old_Value + "','" + AuditLogRecord.New_Value + "','" + AuditLogRecord.Attribute + "')";
                                        command.ExecuteNonQuery();
                                    }

                                    NHibernateSessionUtility.Instance.MyAuditLogList.Clear();
                                }
                            }

                        }
                        //Added by Selvaraman for AuditLog - 5 Jun
                        //if (NHibernateSessionUtility.Instance.MyAuditLogList.Count > 0)
                        //{
                        //    if (typeof(T).AssemblyQualifiedName.ToString().Contains("Acurus.Capella.Core.DomainObjects.AuditLog") == false)
                        //    {
                        //        AuditLogManager auditLogManager = new AuditLogManager();

                        //        // auditLogManager.AppendToAuditLog(NHibernateSessionUtility.Instance.MyAuditLogList, session, NHibernateSessionUtility.Instance.MACAddress);

                        //        IDbCommand command = session.Connection.CreateCommand();
                        //        command.Connection = session.Connection;

                        //        trans.Enlist(command);

                        //        for (int k = 0; k < NHibernateSessionUtility.Instance.MyAuditLogList.Count; k++)
                        //        {
                        //            AuditLog AuditLogRecord = NHibernateSessionUtility.Instance.MyAuditLogList[k];
                        //            command.CommandText = "insert into audit_log values (0,'" + AuditLogRecord.Human_ID.ToString() + "','" + AuditLogRecord.Entity_Name + "','" +
                        //                AuditLogRecord.Transaction_Type + "','" + AuditLogRecord.Transaction_By + "','" +
                        //                AuditLogRecord.Transaction_Date_And_Time.ToString("yyyy-MM-dd hh:mm:ss") + "','" + AuditLogRecord.Entity_Id.ToString() + "','" +
                        //                AuditLogRecord.MAC_Address + "')";
                        //            command.ExecuteNonQuery();
                        //        }

                        //        NHibernateSessionUtility.Instance.MyAuditLogList.Clear();
                        //    }
                        //}


                        trans.Commit();
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        if (ex.InnerException.Source.ToString().Contains("MySQL") == true)
                        {
                            MySql.Data.MySqlClient.MySqlException excep = (MySql.Data.MySqlClient.MySqlException)ex.InnerException;

                            //Deadlock Error
                            if (excep.Number == 1213 && iTryCount < 5)
                            {
                                iTryCount++;
                                goto TryAgain;
                            }
                            else
                            {
                                trans.Rollback();
                                //session.Close();
                                throw new Exception(ex.Message);
                            }
                        }

                        else
                        {
                            //if (ex.InnerException.Source.ToString().Contains("MySql.Data"))
                            //{
                            //    MySql.Data.MySqlClient.MySqlException excep = (MySql.Data.MySqlClient.MySqlException)ex.InnerException;
                            //    if (excep.Number == 1451)
                            //    {
                            //        trans.Rollback();
                            //        throw new Exception("Referrential Intergrity Exception Occured");
                            //    }
                            //}
                            trans.Rollback();
                            //session.Close();
                            throw new Exception(ex.Message);
                        }
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        session.Close();
                    }
                }
            }
            catch (Exception ex1)
            {
                //session.Close();
                throw new Exception(ex1.Message);
            }
        }

        public int SaveUpdateDeleteWithoutTransaction(ref IList<T> saveList, IList<T> updateList, IList<T> deleteList, ISession MySession, string MACAddress)
        {
            int iResult = -1;

            //ISession session = Session.GetISession();
            NHibernateSessionUtility.Instance.MySession = MySession;
            NHibernateSessionUtility.Instance.MACAddress = MACAddress;
            ITransaction trans = null;
            try
            {

                if (deleteList != null)
                {
                    Delete(deleteList, MySession);
                }

                if (saveList != null)
                {
                    Save(saveList, MySession);
                }

                if (updateList != null)
                {
                    Update(updateList, MySession);
                }

                iResult = 0;

                //Added by Jisha for Delete AuditLog 
                if (deleteList != null)
                {

                    if (NHibernateSessionUtility.Instance.MyAuditLogList.Count > 0)
                    {
                        if (typeof(T).AssemblyQualifiedName.ToString().Contains("Acurus.Capella.Core.DomainObjects.AuditLog") == false)
                        {
                            AuditLogManager auditLogManager = new AuditLogManager();

                            // auditLogManager.AppendToAuditLog(NHibernateSessionUtility.Instance.MyAuditLogList, session, NHibernateSessionUtility.Instance.MACAddress);

                            IDbCommand command = MySession.Connection.CreateCommand();
                            command.Connection = MySession.Connection;

                            //trans.Enlist(command);
                            AuditLog AuditLogRecord;//= NHibernateSessionUtility.Instance.MyAuditLogList[k];
                            for (int k = 0; k < NHibernateSessionUtility.Instance.MyAuditLogList.Count; k++)
                            {
                                AuditLogRecord = NHibernateSessionUtility.Instance.MyAuditLogList[k];
                                command.CommandText = "insert into audit_log values (0,'" + AuditLogRecord.Human_ID.ToString() + "','" + AuditLogRecord.Entity_Name + "','" +
                                    AuditLogRecord.Transaction_Type + "','" + AuditLogRecord.Transaction_By + "','" +
                                    AuditLogRecord.Transaction_Date_And_Time.ToString("yyyy-MM-dd hh:mm:ss") + "','" + AuditLogRecord.Entity_Id.ToString() + "','" +
                                    AuditLogRecord.Old_Value + "','" + AuditLogRecord.New_Value + "','" + AuditLogRecord.Attribute + "')";
                                command.ExecuteNonQuery();
                            }

                            NHibernateSessionUtility.Instance.MyAuditLogList.Clear();
                        }
                    }
                }



            }



            catch (NHibernate.Exceptions.GenericADOException ex)
            {


                if (ex.InnerException.Source.ToString().Contains("MySQL") == true)
                {
                    MySql.Data.MySqlClient.MySqlException excep = (MySql.Data.MySqlClient.MySqlException)ex.InnerException;

                    //Deadlock Error
                    if (excep.Number == 1213)
                    {
                        iResult = 2;
                        throw new Exception(ex.Message);
                    }
                    else
                    {
                        iResult = 1;
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    ////if (ex.InnerException.Source.ToString().Contains("MySql.Data"))
                    ////{
                    ////    MySql.Data.MySqlClient.MySqlException excep = (MySql.Data.MySqlClient.MySqlException)ex.InnerException;
                    ////    if (excep.Number == 1451)
                    ////    {
                    ////        trans.Rollback();
                    ////        throw new Exception("Referrential Intergrity Exception Occured");
                    ////    }
                    ////}
                    //trans.Rollback();
                    //session.Close();
                    //throw;
                    iResult = 1;
                    if (trans != null)
                        trans.Rollback();
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ep)
            {
                iResult = 1;
                if (trans != null)
                    trans.Rollback();
                throw new Exception(ep.Message);
            }
            finally
            {

            }
            return iResult;
        }

        /* Added for SaveUpdateDelete_DBAndXML_WithTransaction and SaveUpdateDelete_DBAndXML_WithoutTransaction
         * bool Is_EncRecord -to check if the Record is Encounter Record
         * ulong HumanID_EncSave -Human ID for the corresponding Encounter Record
         * An new Parameter (bool bSave_In_Human) to the each of GenerateXML- Save,SaveStatic,Update functions 
         * bSave_In_Human - "true" :If Record-Encounter record, an Save/Update is sent for saving in HUmanXML along with EncounterXML update else bSave_In_Human-"false"
        */

        //public void SaveUpdateDelete_DBAndXML_WithTransaction(ref IList<T> saveList, ref IList<T> updateList, IList<T> deleteList, string MACAddress, bool bSaveInXML, bool bSaveStatic, ulong EncounterOrHumanId, string sGeneralNotesText)
        //{
        //    // string Is_Audit_log = "N";
        //    bool bIsRCopia = false;
        //    bool bResetLocalTime = false;
        //    IList<string> lstSaveLocalTimes = new List<string>();
        //    IList<string> lstUpdateLocalTimes = new List<string>();
        //    iTryCount = 0;
        //    IList<object> lstobjxml = new List<object>();
        //    bool bsavehit = false;
        //    bool bXmlFound = true;
        //    bool Is_EncRecord = false;
        //    ulong HumanID_EncSave = 0;
        //    string IsRxHistory = "N";
        //    IList<ulong> lstDataConsistentHumanIDs = new List<ulong>();
        //    if (System.Configuration.ConfigurationManager.AppSettings["IsRxHistory"] != null)
        //        IsRxHistory = System.Configuration.ConfigurationManager.AppSettings["IsRxHistory"].Trim().ToUpper();

        //    TryAgain:
        //    ISession session = Session.GetISession();

        //    NHibernateSessionUtility.Instance.MySession = session;
        //    NHibernateSessionUtility.Instance.MACAddress = MACAddress;
        //    try
        //    {
        //        using (ITransaction trans = session.BeginTransaction(IsolationLevel.ReadUncommitted))
        //        {
        //            try
        //            {
        //                #region DB Transaction
        //                if (deleteList != null)
        //                {
        //                    Delete(deleteList, session);
        //                    if (deleteList.Count > 0 && (deleteList[0].GetType().Name.ToUpper() == "RCOPIA_ALLERGY" || deleteList[0].GetType().Name.ToUpper() == "RCOPIA_MEDICATION" || deleteList[0].GetType().Name.ToUpper() == "RCOPIA_PRESCRIPTION_LIST"))
        //                        bIsRCopia = true;
        //                    if (deleteList.Count > 0 && (deleteList[0].GetType().Name.ToUpper() == "PATIENTRESULTS" || deleteList[0].GetType().Name.ToUpper() == "ADDENDUMNOTES"))
        //                        bResetLocalTime = true;
        //                }

        //                if (saveList != null)
        //                {
        //                    if (saveList.Count > 0 && (saveList[0].GetType().Name.ToUpper() == "PATIENTRESULTS" || saveList[0].GetType().Name.ToUpper() == "ADDENDUMNOTES" || saveList[0].GetType().Name.ToUpper() == "ENCOUNTER"))
        //                    {
        //                        bResetLocalTime = true;
        //                        for (int iCount = 0; iCount < saveList.Count; iCount++)
        //                            lstSaveLocalTimes.Add(saveList[iCount].GetType().GetProperty("Local_Time").GetValue(saveList[iCount], null).ToString());
        //                        if (saveList[0].GetType().Name.ToUpper() == "ENCOUNTER")
        //                        {
        //                            Is_EncRecord = true;
        //                            IList<Encounter> enc = (IList<Encounter>)saveList;
        //                            HumanID_EncSave = enc[0].Human_ID;
        //                        }
        //                    }
        //                    Save(saveList, session);
        //                    if (saveList.Count > 0 && (saveList[0].GetType().Name.ToUpper() == "RCOPIA_ALLERGY" || saveList[0].GetType().Name.ToUpper() == "RCOPIA_MEDICATION" || saveList[0].GetType().Name.ToUpper() == "RCOPIA_PRESCRIPTION_LIST"))
        //                        bIsRCopia = true;
        //                }

        //                if (updateList != null)
        //                {
        //                    if (updateList.Count > 0 && (updateList[0].GetType().Name.ToUpper() == "PATIENTRESULTS" || updateList[0].GetType().Name.ToUpper() == "ADDENDUMNOTES" || updateList[0].GetType().Name.ToUpper() == "ENCOUNTER"))
        //                    {
        //                        bResetLocalTime = true;
        //                        for (int iCount = 0; iCount < updateList.Count; iCount++)
        //                            lstUpdateLocalTimes.Add(updateList[iCount].GetType().GetProperty("Local_Time").GetValue(updateList[iCount], null).ToString());
        //                        if (updateList[0].GetType().Name.ToUpper() == "ENCOUNTER")
        //                        {
        //                            Is_EncRecord = true;
        //                            IList<Encounter> enc = (IList<Encounter>)updateList;
        //                            HumanID_EncSave = enc[0].Human_ID;
        //                        }
        //                    }
        //                    Update(updateList, session);
        //                    if (updateList.Count > 0 && (updateList[0].GetType().Name.ToUpper() == "RCOPIA_ALLERGY" || updateList[0].GetType().Name.ToUpper() == "RCOPIA_MEDICATION" || updateList[0].GetType().Name.ToUpper() == "RCOPIA_PRESCRIPTION_LIST"))
        //                        bIsRCopia = true;
        //                }
        //                session.Flush();
        //                if (bSaveInXML && EncounterOrHumanId == 0 && saveList != null && saveList.Count > 0 && saveList[0].GetType().Name.ToUpper() == "HUMAN")
        //                {
        //                    if (saveList != null && saveList.Count > 0)
        //                    {
        //                        EncounterOrHumanId = Convert.ToUInt32(saveList[0].GetType().GetProperty("Id").GetValue(saveList[0], null));
        //                        string HumanFileName = "Human" + "_" + EncounterOrHumanId + ".xml";
        //                        string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);

        //                        string sDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("Template_XML");
        //                        string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
        //                        XmlDocument itemDoc = new XmlDocument();
        //                        XmlTextReader XmlText = new XmlTextReader(sXmlPath);
        //                        itemDoc.Load(XmlText);
        //                        XmlNodeList xmlnode = itemDoc.GetElementsByTagName("EncounterDetails");
        //                        xmlnode[0].ParentNode.RemoveChild(xmlnode[0]);
        //                    }
        //                }
        //                #endregion

        //                if (IsRxHistory.ToUpper() == "Y")
        //                    bIsRCopia = false;
        //                #region XML Transaction And Transaction Commit
        //                if (bSaveInXML)
        //                {
        //                    if (bResetLocalTime)
        //                    {
        //                        if (saveList != null && saveList.Count > 0)
        //                        {
        //                            for (int iCount = 0; iCount < saveList.Count; iCount++)
        //                            {
        //                                saveList[iCount].GetType().GetProperty("Local_Time").SetValue(saveList[iCount], lstSaveLocalTimes[iCount], null);
        //                                if (saveList[iCount].GetType().Name.ToUpper() == "ENCOUNTER")
        //                                    saveList[iCount].GetType().GetProperty("Local_Time").SetValue(saveList[iCount], lstSaveLocalTimes[iCount], null);
        //                            }
        //                        }
        //                        if (updateList != null && updateList.Count > 0)
        //                        {
        //                            for (int iCount = 0; iCount < updateList.Count; iCount++)
        //                            {
        //                                updateList[iCount].GetType().GetProperty("Local_Time").SetValue(updateList[iCount], lstUpdateLocalTimes[iCount], null);
        //                                if (updateList[iCount].GetType().Name.ToUpper() == "ENCOUNTER")
        //                                    updateList[iCount].GetType().GetProperty("Local_Time").SetValue(updateList[iCount], lstUpdateLocalTimes[iCount], null);
        //                            }
        //                        }
        //                    }
        //                    GenerateXml XMLObj = new GenerateXml();
        //                    IList<GenerateXml> XMLObjList = new List<GenerateXml>();
        //                    IList<object> lstCheckDataConsistency = new List<object>();
        //                    bool bIsDataConsistent = true;

        //                    GenerateXml XmlObjHuman = null;
        //                    if (Is_EncRecord)
        //                    {
        //                        XmlObjHuman = new GenerateXml();
        //                    }

        //                    if (!bIsRCopia)
        //                    {
        //                        if (!bSaveStatic)
        //                        {
        //                            IList<T> SaveUpdateList = new List<T>();
        //                            if (saveList != null && saveList.Count > 0)
        //                                SaveUpdateList = saveList;
        //                            if (updateList != null && updateList.Count > 0)
        //                                SaveUpdateList = SaveUpdateList.Concat(updateList).ToList();
        //                            if (SaveUpdateList.Count > 0)
        //                            {
        //                                IList<object> lstObj = SaveUpdateList.Cast<object>().ToList();
        //                                lstobjxml = lstObj;
        //                                XMLObj.GenerateXmlSave(lstObj, EncounterOrHumanId, sGeneralNotesText, false, false, false, false);
        //                                if (XmlObjHuman != null)
        //                                {
        //                                    XmlObjHuman.GenerateXmlSave(lstObj, HumanID_EncSave, sGeneralNotesText, true, false, false, false);
        //                                }

        //                                bsavehit = true;
        //                                lstCheckDataConsistency = lstObj;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (saveList != null && saveList.Count > 0)
        //                            {
        //                                IList<object> lstObj = saveList.Cast<object>().ToList();
        //                                lstobjxml = lstObj;
        //                                XMLObj.GenerateXmlSaveStatic(lstObj, EncounterOrHumanId, sGeneralNotesText, false);
        //                                if (XmlObjHuman != null)
        //                                    XmlObjHuman.GenerateXmlSaveStatic(lstObj, HumanID_EncSave, sGeneralNotesText, true);
        //                                bsavehit = true;
        //                                lstCheckDataConsistency = lstObj;
        //                            }
        //                            if (updateList != null && updateList.Count > 0)
        //                            {
        //                                IList<object> lstObj = updateList.Cast<object>().ToList();
        //                                lstobjxml = lstObj;
        //                                XMLObj.GenerateXmlUpdate(lstObj, EncounterOrHumanId, sGeneralNotesText, false);
        //                                if (XmlObjHuman != null)
        //                                    XmlObjHuman.GenerateXmlUpdate(lstObj, HumanID_EncSave, sGeneralNotesText, true);
        //                                bsavehit = true;
        //                                lstCheckDataConsistency = lstCheckDataConsistency.Concat(lstObj).ToList<object>();
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {

        //                        IList<ulong> lstPrevHumanIDs = new List<ulong>();
        //                        lstDataConsistentHumanIDs = new List<ulong>();
        //                        GenerateXml tempXMLObj = new GenerateXml();

        //                        if (saveList != null && saveList.Count > 0)
        //                        {
        //                            for (int i = 0; i < saveList.Count && bIsDataConsistent; i++)
        //                            {
        //                                List<object> lstObj = new List<object>();
        //                                lstObj.Add(saveList[i]);
        //                                lstObj = lstObj.Cast<object>().ToList();
        //                                ulong uHuman_id = Convert.ToUInt32(lstObj[0].GetType().GetProperty("Human_ID").GetValue(lstObj[0], null).ToString());
        //                                if (!lstPrevHumanIDs.Any(item => item == uHuman_id))
        //                                {
        //                                    tempXMLObj = new GenerateXml();
        //                                    lstobjxml = lstObj;
        //                                    tempXMLObj.GenerateXmlSaveStatic(lstObj, uHuman_id, string.Empty, false);
        //                                    bsavehit = true;
        //                                    XMLObjList.Add(tempXMLObj);
        //                                    lstPrevHumanIDs.Add(uHuman_id);
        //                                }
        //                                else
        //                                {
        //                                    tempXMLObj = new GenerateXml();
        //                                    tempXMLObj = XMLObjList.Where(item => item.itemDoc.BaseURI.Contains("_" + uHuman_id + ".xml")).FirstOrDefault();
        //                                    lstobjxml = lstObj;
        //                                    if (tempXMLObj == null)
        //                                    {
        //                                        EncounterOrHumanId = uHuman_id;
        //                                        goto ln;
        //                                    }
        //                                    else
        //                                    {
        //                                        tempXMLObj.GenerateXmlSaveStatic(lstObj, uHuman_id, string.Empty, false);
        //                                    }
        //                                    bsavehit = true;

        //                                }
        //                                lstCheckDataConsistency = lstCheckDataConsistency.Concat(lstObj).ToList<object>();
        //                                bIsDataConsistent = tempXMLObj.CheckDataConsistency(lstObj, true, sGeneralNotesText);

        //                                //Rcopia_medication data conssitancy
        //                                if (!bIsDataConsistent)
        //                                {
        //                                    lstDataConsistentHumanIDs.Add(uHuman_id);
        //                                    XMLObjList.Remove(tempXMLObj);

        //                                }
        //                            }
        //                        }
        //                        if (updateList != null && updateList.Count > 0)
        //                        {
        //                            for (int i = 0; i < updateList.Count && bIsDataConsistent; i++)
        //                            {
        //                                List<object> lstObj = new List<object>();
        //                                lstObj.Add(updateList[i]);
        //                                lstObj = lstObj.Cast<object>().ToList();
        //                                ulong uHuman_id = Convert.ToUInt32(lstObj[0].GetType().GetProperty("Human_ID").GetValue(lstObj[0], null).ToString());
        //                                if (!lstPrevHumanIDs.Any(item => item == uHuman_id))
        //                                {
        //                                    tempXMLObj = new GenerateXml();
        //                                    tempXMLObj.GenerateXmlUpdate(lstObj, uHuman_id, string.Empty, false);
        //                                    bsavehit = true;
        //                                    lstobjxml = lstObj;
        //                                    XMLObjList.Add(tempXMLObj);
        //                                    lstPrevHumanIDs.Add(uHuman_id);
        //                                }
        //                                else
        //                                {
        //                                    tempXMLObj = new GenerateXml();
        //                                    tempXMLObj = XMLObjList.Where(item => item.itemDoc.BaseURI.Contains("_" + uHuman_id + ".xml")).FirstOrDefault();
        //                                    lstobjxml = lstObj;
        //                                    if (tempXMLObj == null)
        //                                    {
        //                                        EncounterOrHumanId = uHuman_id;
        //                                        goto ln;
        //                                    }
        //                                    else
        //                                        tempXMLObj.GenerateXmlUpdate(lstObj, uHuman_id, string.Empty, false);
        //                                    bsavehit = true;
        //                                }
        //                                lstCheckDataConsistency = lstCheckDataConsistency.Concat(lstObj).ToList<object>();
        //                                bIsDataConsistent = tempXMLObj.CheckDataConsistency(lstObj, true, sGeneralNotesText);

        //                                //If any data consistancy in RCOPIA_MEDICATION we need to freshly update the all list from DB
        //                                if (!bIsDataConsistent)
        //                                {
        //                                    lstDataConsistentHumanIDs.Add(uHuman_id);
        //                                    XMLObjList.Remove(tempXMLObj);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (deleteList != null && deleteList.Count > 0)
        //                    {
        //                        IList<object> lstObj = deleteList.Cast<object>().ToList();
        //                        lstobjxml = lstObj;
        //                        XMLObj.GenerateXmlDelete(lstObj, EncounterOrHumanId, sGeneralNotesText, false);
        //                        bsavehit = true;
        //                    }
        //                #region XML type
        //                ln: string FileName = "Encounter" + "_" + EncounterOrHumanId + ".xml";
        //                    if (lstobjxml.Count > 0)
        //                    {
        //                        if (ilstHumanXml == null || ilstHumanXml.Count == 0)
        //                        {
        //                            SetHumanXmlList();
        //                        }

        //                        string SourceName = lstobjxml[0].GetType().Name;
        //                        string GeneralNotesList = SourceName + sGeneralNotesText + "List";
        //                        if (ilstHumanXml.Contains(GeneralNotesList) || ilstHumanXml.Contains(SourceName))
        //                        {
        //                            FileName = FileName.Replace("Encounter", "Human");
        //                        }
        //                    }
        //                    #endregion
        //                    if (!bIsRCopia)
        //                        bIsDataConsistent = XMLObj.CheckDataConsistency(lstCheckDataConsistency, false, sGeneralNotesText);
        //                    if (bIsRCopia)
        //                    {

        //                        for (int i = 0; i < XMLObjList.Count; i++)
        //                        {
        //                            if (bsavehit)
        //                            {
        //                                if (XMLObjList[i].itemDoc.BaseURI == "")
        //                                {
        //                                    bXmlFound = false;
        //                                    throw new Exception("$Transaction XML: '" + XMLObjList[i].strXmlFilePath + "' not found. Please contact support team to generate the XML. ");
        //                                }

        //                            }
        //                        }

        //                        if (bXmlFound)
        //                        {
        //                            for (int i = 0; i < XMLObjList.Count; i++)
        //                            {
        //                                try
        //                                {
        //                                    //XMLObjList[i].itemDoc.Save(XMLObjList[i].strXmlFilePath);

        //                                    int trycount = 0;
        //                                trytosaveagain:
        //                                    try
        //                                    {
        //                                        XMLObjList[i].itemDoc.Save(XMLObjList[i].strXmlFilePath);
        //                                    }
        //                                    catch (Exception xmlexcep)
        //                                    {
        //                                        trycount++;
        //                                        if (trycount <= 3)
        //                                        {
        //                                            int TimeMilliseconds = 0;
        //                                            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
        //                                                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

        //                                            Thread.Sleep(TimeMilliseconds);
        //                                            string sMsg = string.Empty;
        //                                            string sExStackTrace = string.Empty;

        //                                            string version = "";
        //                                            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
        //                                                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

        //                                            string[] server = version.Split('|');
        //                                            string serverno = "";
        //                                            if (server.Length > 1)
        //                                                serverno = server[1].Trim();

        //                                            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
        //                                                sMsg = xmlexcep.InnerException.Message;
        //                                            else
        //                                                sMsg = xmlexcep.Message;

        //                                            if (xmlexcep != null && xmlexcep.StackTrace != null)
        //                                                sExStackTrace = xmlexcep.StackTrace;

        //                                            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','','','','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
        //                                            string ConnectionData;
        //                                            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        //                                            using (MySqlConnection con = new MySqlConnection(ConnectionData))
        //                                            {
        //                                                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
        //                                                {
        //                                                    cmd.Connection = con;
        //                                                    try
        //                                                    {
        //                                                        con.Open();
        //                                                        cmd.ExecuteNonQuery();
        //                                                        con.Close();
        //                                                    }
        //                                                    catch
        //                                                    {
        //                                                    }
        //                                                }
        //                                            }
        //                                            goto trytosaveagain;
        //                                        }
        //                                    }

        //                                }
        //                                catch (XmlException xmlexcep)
        //                                {
        //                                    throw new Exception(xmlexcep.Message);
        //                                }
        //                                catch (IOException ex)
        //                                {
        //                                    throw new Exception(ex.Message);
        //                                }
        //                            }
        //                            trans.Commit();
        //                            //Rcopia medicaiton 
        //                            if (lstDataConsistentHumanIDs != null && lstDataConsistentHumanIDs.Count > 0)
        //                            {

        //                                Rcopia_MedicationManager objMedmngr = new Rcopia_MedicationManager();
        //                                IList<Rcopia_Medication> lstMed = new List<Rcopia_Medication>();
        //                                GenerateXml objxml = new GenerateXml();
        //                                foreach (ulong uHumanId in lstDataConsistentHumanIDs)
        //                                {
        //                                    objxml = new GenerateXml();
        //                                    lstMed = objMedmngr.GetMedicationByHumanID(uHumanId);
        //                                    IList<object> lstObj = lstMed.Cast<object>().ToList();
        //                                    objxml.GenerateXmlSave(lstObj, uHumanId, string.Empty, true, false, false, true);

        //                                }
        //                            }
        //                        }
        //                    }
        //                    else

        //                        if (bsavehit)
        //                    {
        //                        if (XMLObj.itemDoc.BaseURI != "")
        //                        {
        //                            if (bIsDataConsistent)
        //                            {
        //                                trans.Commit();
        //                                if (!bIsRCopia)
        //                                {
        //                                    // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
        //                                    int trycount = 0;
        //                                trytosaveagain:
        //                                    try
        //                                    {
        //                                        XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
        //                                    }
        //                                    catch (Exception xmlexcep)
        //                                    {
        //                                        trycount++;
        //                                        if (trycount <= 3)
        //                                        {
        //                                            int TimeMilliseconds = 0;
        //                                            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
        //                                                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

        //                                            Thread.Sleep(TimeMilliseconds);
        //                                            string sMsg = string.Empty;
        //                                            string sExStackTrace = string.Empty;

        //                                            string version = "";
        //                                            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
        //                                                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

        //                                            string[] server = version.Split('|');
        //                                            string serverno = "";
        //                                            if (server.Length > 1)
        //                                                serverno = server[1].Trim();

        //                                            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
        //                                                sMsg = xmlexcep.InnerException.Message;
        //                                            else
        //                                                sMsg = xmlexcep.Message;

        //                                            if (xmlexcep != null && xmlexcep.StackTrace != null)
        //                                                sExStackTrace = xmlexcep.StackTrace;

        //                                            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','','','','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
        //                                            string ConnectionData;
        //                                            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        //                                            using (MySqlConnection con = new MySqlConnection(ConnectionData))
        //                                            {
        //                                                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
        //                                                {
        //                                                    cmd.Connection = con;
        //                                                    try
        //                                                    {
        //                                                        con.Open();
        //                                                        cmd.ExecuteNonQuery();
        //                                                        con.Close();
        //                                                    }
        //                                                    catch
        //                                                    {
        //                                                    }
        //                                                }
        //                                            }
        //                                            goto trytosaveagain;
        //                                        }
        //                                    }
        //                                }
        //                                if (Is_EncRecord && XmlObjHuman != null && XmlObjHuman.itemDoc.BaseURI != "")
        //                                {
        //                                    // XmlObjHuman.itemDoc.Save(XmlObjHuman.strXmlFilePath);

        //                                    int trycount = 0;
        //                                trytosaveagain:
        //                                    try
        //                                    {
        //                                        XmlObjHuman.itemDoc.Save(XmlObjHuman.strXmlFilePath);
        //                                    }
        //                                    catch (Exception xmlexcep)
        //                                    {
        //                                        trycount++;
        //                                        if (trycount <= 3)
        //                                        {
        //                                            int TimeMilliseconds = 0;
        //                                            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
        //                                                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

        //                                            Thread.Sleep(TimeMilliseconds);
        //                                            string sMsg = string.Empty;
        //                                            string sExStackTrace = string.Empty;

        //                                            string version = "";
        //                                            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
        //                                                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

        //                                            string[] server = version.Split('|');
        //                                            string serverno = "";
        //                                            if (server.Length > 1)
        //                                                serverno = server[1].Trim();

        //                                            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
        //                                                sMsg = xmlexcep.InnerException.Message;
        //                                            else
        //                                                sMsg = xmlexcep.Message;

        //                                            if (xmlexcep != null && xmlexcep.StackTrace != null)
        //                                                sExStackTrace = xmlexcep.StackTrace;

        //                                            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','','','','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
        //                                            string ConnectionData;
        //                                            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        //                                            using (MySqlConnection con = new MySqlConnection(ConnectionData))
        //                                            {
        //                                                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
        //                                                {
        //                                                    cmd.Connection = con;
        //                                                    try
        //                                                    {
        //                                                        con.Open();
        //                                                        cmd.ExecuteNonQuery();
        //                                                        con.Close();
        //                                                    }
        //                                                    catch
        //                                                    {
        //                                                    }
        //                                                }
        //                                            }
        //                                            goto trytosaveagain;
        //                                        }
        //                                    }

        //                                }
        //                            }
        //                            else
        //                            {
        //                                throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");



        //                            }
        //                        }
        //                        else
        //                        {
        //                            throw new Exception("$Transaction XML: '" + FileName + "' not found. Please contact support team to generate the XML. ");
        //                        }
        //                    }
        //                }
        //                else
        //                    trans.Commit();
        //                #endregion
        //            }
        //            catch (NHibernate.Exceptions.GenericADOException ex)
        //            {
        //                if (ex.InnerException.Source.ToString().Contains("MySQL") == true)
        //                {
        //                    MySql.Data.MySqlClient.MySqlException excep = (MySql.Data.MySqlClient.MySqlException)ex.InnerException;

        //                    //Deadlock Error
        //                    if (excep.Number == 1213 && iTryCount < 5)
        //                    {
        //                        iTryCount++;
        //                        goto TryAgain;
        //                    }
        //                    else
        //                    {
        //                        trans.Rollback();
        //                        throw new Exception(ex.Message);
        //                    }
        //                }

        //                else
        //                {
        //                    trans.Rollback();
        //                    throw new Exception(ex.Message);
        //                }
        //            }
        //            catch (SoapException e)
        //            {
        //                trans.Rollback();
        //                throw new Exception(e.Message);
        //            }
        //            catch (Exception e)
        //            {
        //                trans.Rollback();
        //                throw new Exception(e.Message);
        //            }
        //            finally
        //            {
        //                session.Close();
        //            }
        //        }
        //        Webservicetrigger(ref saveList, ref updateList, deleteList);
        //    }
        //    catch (Exception ex1)
        //    {
        //        throw new Exception(ex1.Message);
        //    }
        //}

        public void SaveUpdateDelete_DBAndXML_WithTransaction(ref IList<T> saveList, ref IList<T> updateList, IList<T> deleteList, string MACAddress, bool bSaveInXML, bool bSaveStatic, ulong EncounterOrHumanId, string sGeneralNotesText)
        {
            // string Is_Audit_log = "N";
            bool bIsRCopia = false;
            bool bResetLocalTime = false;
            IList<string> lstSaveLocalTimes = new List<string>();
            IList<string> lstUpdateLocalTimes = new List<string>();
            iTryCount = 0;
            IList<object> lstobjxml = new List<object>();
            bool bsavehit = false;
            bool bXmlFound = true;
            bool Is_EncRecord = false;
            ulong HumanID_EncSave = 0;
            string IsRxHistory = "N";
            IList<ulong> lstDataConsistentHumanIDs = new List<ulong>();
            if (System.Configuration.ConfigurationManager.AppSettings["IsRxHistory"] != null)
                IsRxHistory = System.Configuration.ConfigurationManager.AppSettings["IsRxHistory"].Trim().ToUpper();

            TryAgain:
            ISession session = Session.GetISession();

            NHibernateSessionUtility.Instance.MySession = session;
            NHibernateSessionUtility.Instance.MACAddress = MACAddress;
            try
            {
                using (ITransaction trans = session.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        #region DB Transaction
                        if (deleteList != null)
                        {
                            Delete(deleteList, session);
                            if (deleteList.Count > 0 && (deleteList[0].GetType().Name.ToUpper() == "RCOPIA_ALLERGY" || deleteList[0].GetType().Name.ToUpper() == "RCOPIA_MEDICATION" || deleteList[0].GetType().Name.ToUpper() == "RCOPIA_PRESCRIPTION_LIST"))
                                bIsRCopia = true;
                            if (deleteList.Count > 0 && (deleteList[0].GetType().Name.ToUpper() == "PATIENTRESULTS" || deleteList[0].GetType().Name.ToUpper() == "ADDENDUMNOTES"))
                                bResetLocalTime = true;
                        }

                        if (saveList != null)
                        {
                            if (saveList.Count > 0 && (saveList[0].GetType().Name.ToUpper() == "PATIENTRESULTS" || saveList[0].GetType().Name.ToUpper() == "ADDENDUMNOTES" || saveList[0].GetType().Name.ToUpper() == "ENCOUNTER"))
                            {
                                bResetLocalTime = true;
                                for (int iCount = 0; iCount < saveList.Count; iCount++)
                                    lstSaveLocalTimes.Add(saveList[iCount].GetType().GetProperty("Local_Time").GetValue(saveList[iCount], null).ToString());
                                if (saveList[0].GetType().Name.ToUpper() == "ENCOUNTER")
                                {
                                    Is_EncRecord = true;
                                    IList<Encounter> enc = (IList<Encounter>)saveList;
                                    HumanID_EncSave = enc[0].Human_ID;
                                }
                            }
                            Save(saveList, session);
                            if (saveList.Count > 0 && (saveList[0].GetType().Name.ToUpper() == "RCOPIA_ALLERGY" || saveList[0].GetType().Name.ToUpper() == "RCOPIA_MEDICATION" || saveList[0].GetType().Name.ToUpper() == "RCOPIA_PRESCRIPTION_LIST"))
                                bIsRCopia = true;
                        }

                        if (updateList != null)
                        {
                            if (updateList.Count > 0 && (updateList[0].GetType().Name.ToUpper() == "PATIENTRESULTS" || updateList[0].GetType().Name.ToUpper() == "ADDENDUMNOTES" || updateList[0].GetType().Name.ToUpper() == "ENCOUNTER"))
                            {
                                bResetLocalTime = true;
                                for (int iCount = 0; iCount < updateList.Count; iCount++)
                                    lstUpdateLocalTimes.Add(updateList[iCount].GetType().GetProperty("Local_Time").GetValue(updateList[iCount], null).ToString());
                                if (updateList[0].GetType().Name.ToUpper() == "ENCOUNTER")
                                {
                                    Is_EncRecord = true;
                                    IList<Encounter> enc = (IList<Encounter>)updateList;
                                    HumanID_EncSave = enc[0].Human_ID;
                                }
                            }
                            Update(updateList, session);
                            if (updateList.Count > 0 && (updateList[0].GetType().Name.ToUpper() == "RCOPIA_ALLERGY" || updateList[0].GetType().Name.ToUpper() == "RCOPIA_MEDICATION" || updateList[0].GetType().Name.ToUpper() == "RCOPIA_PRESCRIPTION_LIST"))
                                bIsRCopia = true;
                        }
                        session.Flush();
                        if (bSaveInXML && EncounterOrHumanId == 0 && saveList != null && saveList.Count > 0 && saveList[0].GetType().Name.ToUpper() == "HUMAN")
                        {
                            if (saveList != null && saveList.Count > 0)
                            {
                                EncounterOrHumanId = Convert.ToUInt32(saveList[0].GetType().GetProperty("Id").GetValue(saveList[0], null));
                               // string HumanFileName = "Human" + "_" + EncounterOrHumanId + ".xml";
                               // string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);

                                string sDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("Template_XML");
                                string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                                XmlDocument itemDoc = new XmlDocument();
                                XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                                itemDoc.Load(XmlText);
                                XmlNodeList xmlnode = itemDoc.GetElementsByTagName("EncounterDetails");
                                xmlnode[0].ParentNode.RemoveChild(xmlnode[0]);
                            }
                        }
                        #endregion

                        if (IsRxHistory.ToUpper() == "Y")
                            bIsRCopia = false;
                        #region XML Transaction And Transaction Commit
                        if (bSaveInXML)
                        {
                            if (bResetLocalTime)
                            {
                                if (saveList != null && saveList.Count > 0)
                                {
                                    for (int iCount = 0; iCount < saveList.Count; iCount++)
                                    {
                                        saveList[iCount].GetType().GetProperty("Local_Time").SetValue(saveList[iCount], lstSaveLocalTimes[iCount], null);
                                        if (saveList[iCount].GetType().Name.ToUpper() == "ENCOUNTER")
                                            saveList[iCount].GetType().GetProperty("Local_Time").SetValue(saveList[iCount], lstSaveLocalTimes[iCount], null);
                                    }
                                }
                                if (updateList != null && updateList.Count > 0)
                                {
                                    for (int iCount = 0; iCount < updateList.Count; iCount++)
                                    {
                                        updateList[iCount].GetType().GetProperty("Local_Time").SetValue(updateList[iCount], lstUpdateLocalTimes[iCount], null);
                                        if (updateList[iCount].GetType().Name.ToUpper() == "ENCOUNTER")
                                            updateList[iCount].GetType().GetProperty("Local_Time").SetValue(updateList[iCount], lstUpdateLocalTimes[iCount], null);
                                    }
                                }
                            }
                            GenerateXml XMLObj = new GenerateXml();
                            IList<GenerateXml> XMLObjList = new List<GenerateXml>();
                            IList<object> lstCheckDataConsistency = new List<object>();
                            bool bIsDataConsistent = true;

                            GenerateXml XmlObjHuman = null;
                            if (Is_EncRecord)
                            {
                                XmlObjHuman = new GenerateXml();
                            }

                            if (!bIsRCopia)
                            {
                                if (!bSaveStatic)
                                {
                                    IList<T> SaveUpdateList = new List<T>();
                                    if (saveList != null && saveList.Count > 0)
                                        SaveUpdateList = saveList;
                                    if (updateList != null && updateList.Count > 0)
                                        SaveUpdateList = SaveUpdateList.Concat(updateList).ToList();
                                    if (SaveUpdateList.Count > 0)
                                    {
                                        IList<object> lstObj = SaveUpdateList.Cast<object>().ToList();
                                        lstobjxml = lstObj;
                                        GenerateXml objtempxml = new GenerateXml();
                                        objtempxml.itemDoc = null;
                                        XMLObj.GenerateXmlSave(lstObj, EncounterOrHumanId, sGeneralNotesText, false, false, false, false,ref objtempxml);
                                        if (XmlObjHuman != null)
                                        {
                                            XmlObjHuman.GenerateXmlSave(lstObj, HumanID_EncSave, sGeneralNotesText, true, false, false, false,ref objtempxml);
                                        }

                                        bsavehit = true;
                                        lstCheckDataConsistency = lstObj;
                                    }
                                }
                                else
                                {
                                    if (saveList != null && saveList.Count > 0)
                                    {
                                        IList<object> lstObj = saveList.Cast<object>().ToList();
                                        lstobjxml = lstObj;
                                        GenerateXml xmlobjtemp = new GenerateXml();
                                        xmlobjtemp.itemDoc = null;

                                        XMLObj.GenerateXmlSaveStatic(lstObj, EncounterOrHumanId, sGeneralNotesText, false, xmlobjtemp);
                                        if (XmlObjHuman != null)
                                            XmlObjHuman.GenerateXmlSaveStatic(lstObj, HumanID_EncSave, sGeneralNotesText, true, xmlobjtemp);
                                        bsavehit = true;
                                        lstCheckDataConsistency = lstObj;
                                    }
                                    if (updateList != null && updateList.Count > 0)
                                    {
                                        GenerateXml xmlobjtemp = new GenerateXml();
                                        xmlobjtemp.itemDoc = null;
                                        IList<object> lstObj = updateList.Cast<object>().ToList();
                                        lstobjxml = lstObj;
                                        if(saveList != null && saveList.Count > 0)
                                            XMLObj.GenerateXmlUpdate(lstObj, EncounterOrHumanId, sGeneralNotesText, false, XMLObj);
                                        else
                                            XMLObj.GenerateXmlUpdate(lstObj, EncounterOrHumanId, sGeneralNotesText, false, xmlobjtemp);
                                        if (XmlObjHuman != null)
                                            XmlObjHuman.GenerateXmlUpdate(lstObj, HumanID_EncSave, sGeneralNotesText, true, xmlobjtemp);
                                        bsavehit = true;
                                        lstCheckDataConsistency = lstCheckDataConsistency.Concat(lstObj).ToList<object>();
                                    }
                                }
                            }
                            else
                            {

                                IList<ulong> lstPrevHumanIDs = new List<ulong>();
                                lstDataConsistentHumanIDs = new List<ulong>();
                                GenerateXml tempXMLObj = new GenerateXml();

                                if (saveList != null && saveList.Count > 0)
                                {
                                    for (int i = 0; i < saveList.Count && bIsDataConsistent; i++)
                                    {
                                        List<object> lstObj = new List<object>();
                                        lstObj.Add(saveList[i]);
                                        lstObj = lstObj.Cast<object>().ToList();
                                        ulong uHuman_id = Convert.ToUInt32(lstObj[0].GetType().GetProperty("Human_ID").GetValue(lstObj[0], null).ToString());
                                        if (!lstPrevHumanIDs.Any(item => item == uHuman_id))
                                        {
                                            tempXMLObj = new GenerateXml();
                                            lstobjxml = lstObj;
                                            GenerateXml xmlobjtemp = new GenerateXml();
                                            xmlobjtemp.itemDoc = null;
                                            tempXMLObj.GenerateXmlSaveStatic(lstObj, uHuman_id, string.Empty, false, xmlobjtemp);
                                            bsavehit = true;
                                            XMLObjList.Add(tempXMLObj);
                                            lstPrevHumanIDs.Add(uHuman_id);
                                        }
                                        else
                                        {
                                            tempXMLObj = new GenerateXml();
                                            //Jira #CAP-128
                                            //tempXMLObj = XMLObjList.Where(item => item.itemDoc.BaseURI.Contains("_" + uHuman_id + ".xml")).FirstOrDefault();
                                            tempXMLObj = XMLObjList.Where(item => item.itemDoc.InnerXml.Contains(uHuman_id.ToString())).FirstOrDefault();
                                            lstobjxml = lstObj;
                                            if (tempXMLObj == null)
                                            {
                                                EncounterOrHumanId = uHuman_id;
                                                goto ln;
                                            }
                                            else
                                            {
                                                GenerateXml xmlobjtemp = new GenerateXml();
                                                xmlobjtemp.itemDoc = null;
                                                //Jira #CAP-128
                                                //tempXMLObj.GenerateXmlSaveStatic(lstObj, uHuman_id, string.Empty, false, xmlobjtemp);
                                                tempXMLObj.GenerateXmlSaveStatic(lstObj, uHuman_id, string.Empty, false, tempXMLObj);
                                            }
                                            bsavehit = true;

                                        }
                                        lstCheckDataConsistency = lstCheckDataConsistency.Concat(lstObj).ToList<object>();
                                        bIsDataConsistent = tempXMLObj.CheckDataConsistency(lstObj, true, sGeneralNotesText);

                                        //Rcopia_medication data conssitancy
                                        if (!bIsDataConsistent)
                                        {
                                            lstDataConsistentHumanIDs.Add(uHuman_id);
                                            XMLObjList.Remove(tempXMLObj);

                                        }
                                    }
                                }
                                if (updateList != null && updateList.Count > 0)
                                {
                                    for (int i = 0; i < updateList.Count && bIsDataConsistent; i++)
                                    {
                                        List<object> lstObj = new List<object>();
                                        lstObj.Add(updateList[i]);
                                        lstObj = lstObj.Cast<object>().ToList();
                                        ulong uHuman_id = Convert.ToUInt32(lstObj[0].GetType().GetProperty("Human_ID").GetValue(lstObj[0], null).ToString());
                                        if (!lstPrevHumanIDs.Any(item => item == uHuman_id))
                                        {
                                            GenerateXml xmlobjtemp = new GenerateXml();
                                            xmlobjtemp.itemDoc = null;
                                            tempXMLObj = new GenerateXml();
                                            tempXMLObj.GenerateXmlUpdate(lstObj, uHuman_id, string.Empty, false, xmlobjtemp);
                                            bsavehit = true;
                                            lstobjxml = lstObj;
                                            XMLObjList.Add(tempXMLObj);
                                            lstPrevHumanIDs.Add(uHuman_id);
                                        }
                                        else
                                        {
                                            tempXMLObj = new GenerateXml();
                                            //Jira #CAP-128
                                            //tempXMLObj = XMLObjList.Where(item => item.itemDoc.BaseURI.Contains("_" + uHuman_id + ".xml")).FirstOrDefault();
                                            tempXMLObj = XMLObjList.Where(item => item.itemDoc.InnerXml.Contains(uHuman_id.ToString())).FirstOrDefault();
                                            lstobjxml = lstObj;
                                            GenerateXml XMLObjtemp = new GenerateXml();
                                            XMLObjtemp.itemDoc = null;
                                            if (tempXMLObj == null)
                                            {
                                                EncounterOrHumanId = uHuman_id;
                                                goto ln;
                                            }
                                            else
                                            {
                                                //Jira #CAP-128
                                                //tempXMLObj.GenerateXmlUpdate(lstObj, uHuman_id, string.Empty, false, XMLObjtemp);
                                                tempXMLObj.GenerateXmlUpdate(lstObj, uHuman_id, string.Empty, false, tempXMLObj);
                                            }
                                            bsavehit = true;
                                        }
                                        lstCheckDataConsistency = lstCheckDataConsistency.Concat(lstObj).ToList<object>();
                                        bIsDataConsistent = tempXMLObj.CheckDataConsistency(lstObj, true, sGeneralNotesText);

                                        //If any data consistancy in RCOPIA_MEDICATION we need to freshly update the all list from DB
                                        if (!bIsDataConsistent)
                                        {
                                            lstDataConsistentHumanIDs.Add(uHuman_id);
                                            XMLObjList.Remove(tempXMLObj);
                                        }
                                    }
                                }
                            }
                            if (deleteList != null && deleteList.Count > 0)
                            {
                                IList<object> lstObj = deleteList.Cast<object>().ToList();
                                lstobjxml = lstObj;
                                GenerateXml objtempxml = new GenerateXml();
                                objtempxml.itemDoc = null;  
                                XMLObj.GenerateXmlDelete(lstObj, EncounterOrHumanId, sGeneralNotesText, false, objtempxml);
                                bsavehit = true;
                            }
                        #region XML type
                        ln: string FileName = "Encounter" + "_" + EncounterOrHumanId + ".xml";
                            if (lstobjxml.Count > 0)
                            {
                                if (ilstHumanXml == null || ilstHumanXml.Count == 0)
                                {
                                    SetHumanXmlList();
                                }

                                string SourceName = lstobjxml[0].GetType().Name;
                                string GeneralNotesList = SourceName + sGeneralNotesText + "List";
                                if (ilstHumanXml.Contains(GeneralNotesList) || ilstHumanXml.Contains(SourceName))
                                {
                                    FileName = FileName.Replace("Encounter", "Human");
                                }
                            }
                            #endregion
                            if (!bIsRCopia)
                                bIsDataConsistent = XMLObj.CheckDataConsistency(lstCheckDataConsistency, false, sGeneralNotesText);
                            if (bIsRCopia)
                            {

                                for (int i = 0; i < XMLObjList.Count; i++)
                                {
                                    if (bsavehit)
                                    {
                                        //Commented By Deepak
                                        //if (XMLObjList[i].itemDoc.BaseURI == "")
                                        if (XMLObjList[i].itemDoc.InnerXml == "")
                                        {
                                            bXmlFound = false;
                                            throw new Exception("$Transaction XML: '" + "' not found. Please contact support team to generate the XML. ");
                                        }

                                    }
                                }

                                if (bXmlFound)
                                {
                                    for (int i = 0; i < XMLObjList.Count; i++)
                                    {
                                        try
                                        {
                                            //XMLObjList[i].itemDoc.Save(XMLObjList[i].strXmlFilePath);

                                            int trycount = 0;
                                        trytosaveagain:
                                            try
                                            {
                                                //XMLObjList[i].itemDoc.Save(XMLObjList[i].strXmlFilePath);

                                                if (FileName.Contains("Human"))
                                                {
                                                    WriteBlob(EncounterOrHumanId, XMLObjList[i].itemDoc, session, saveList, updateList, deleteList, XMLObjList[i], false);
                                                }
                                                else if (FileName.Contains("Encounter"))
                                                {
                                                    WriteBlob(EncounterOrHumanId, XMLObjList[i].itemDoc, session, saveList, updateList, deleteList, XMLObjList[i], false);
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

                                                    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','','','','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
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
                                        catch (XmlException xmlexcep)
                                        {
                                            throw new Exception(xmlexcep.Message);
                                        }
                                        catch (IOException ex)
                                        {
                                            throw new Exception(ex.Message);
                                        }
                                    }
                                    trans.Commit();
                                    //Rcopia medicaiton 
                                    if (lstDataConsistentHumanIDs != null && lstDataConsistentHumanIDs.Count > 0)
                                    {

                                        Rcopia_MedicationManager objMedmngr = new Rcopia_MedicationManager();
                                        IList<Rcopia_Medication> lstMed = new List<Rcopia_Medication>();
                                        GenerateXml objxml = new GenerateXml();
                                        foreach (ulong uHumanId in lstDataConsistentHumanIDs)
                                        {
                                            objxml = new GenerateXml();
                                            lstMed = objMedmngr.GetMedicationByHumanID(uHumanId);
                                            IList<object> lstObj = lstMed.Cast<object>().ToList();
                                            GenerateXml Xmltemp = new GenerateXml();
                                                Xmltemp.itemDoc=null;
                                            objxml.GenerateXmlSave(lstObj, uHumanId, string.Empty, true, false, false, true,ref Xmltemp);

                                        }
                                    }
                                }
                            }
                            else

                                if (bsavehit)
                            {
                                if (XMLObj.itemDoc.InnerXml != "")
                                {
                                    if (bIsDataConsistent)
                                    {

                                        if (!bIsRCopia)
                                        {
                                            // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                            int trycount = 0;
                                        trytosaveagain:
                                            try
                                            {
                                                //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);

                                                if (FileName.Contains("Human"))
                                                {
                                                    WriteBlob(EncounterOrHumanId, XMLObj.itemDoc, session, saveList, updateList, deleteList, XMLObj, false);
                                                }
                                                else if (FileName.Contains("Encounter"))
                                                {
                                                    WriteBlob(EncounterOrHumanId, XMLObj.itemDoc, session, saveList, updateList, deleteList, XMLObj, false);
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

                                                    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','','','','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
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
                                        if (Is_EncRecord && XmlObjHuman != null && XmlObjHuman.itemDoc.InnerXml != "")
                                        {
                                            // XmlObjHuman.itemDoc.Save(XmlObjHuman.strXmlFilePath);

                                            int trycount = 0;
                                        trytosaveagain:
                                            try
                                            {
                                                //XmlObjHuman.itemDoc.Save(XmlObjHuman.strXmlFilePath);

                                                //if (FileName.Contains("Human"))
                                                //{
                                                WriteBlob(XmlObjHuman.ulHumanID, XmlObjHuman.itemDoc, session, saveList, updateList, deleteList, XmlObjHuman, false);
                                                //}
                                                //else if (FileName.Contains("Encounter"))
                                                //{
                                                //    WriteBlob("Encounter", EncounterOrHumanId, XMLObjList[i].itemDoc, session, saveList, updateList, deleteList);
                                                //}
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

                                                    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','','','','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
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
                                    {
                                        throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");



                                    }
                                }
                                else
                                {
                                    throw new Exception("$Transaction XML: '" + FileName + "' not found. Please contact support team to generate the XML. ");
                                }
                            }
                        }
                        else
                            trans.Commit();
                        #endregion
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        if (ex.InnerException.Source.ToString().Contains("MySQL") == true)
                        {
                            MySql.Data.MySqlClient.MySqlException excep = (MySql.Data.MySqlClient.MySqlException)ex.InnerException;

                            //Deadlock Error
                            if (excep.Number == 1213 && iTryCount < 5)
                            {
                                iTryCount++;
                                goto TryAgain;
                            }
                            else
                            {
                                trans.Rollback();
                                throw new Exception(ex.Message);
                            }
                        }

                        else
                        {
                            trans.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                    catch (SoapException e)
                    {
                        trans.Rollback();
                        throw new Exception(e.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        session.Close();
                    }
                }
                Webservicetrigger(ref saveList, ref updateList, deleteList);
            }
            catch (Exception ex1)
            {
                throw new Exception(ex1.Message);
            }
        }



        public int SaveUpdateDelete_DBAndXML_WithoutTransaction(ref IList<T> saveList, ref IList<T> updateList, IList<T> deleteList, ISession MySession, string MACAddress, bool bSaveInXML, bool bSaveStatic, ulong EncounterOrHumanId, string sGeneralNotesText, ref GenerateXml XMLObj)
        {

            bool bResetLocalTime = false;
            IList<string> lstSaveLocalTimes = new List<string>();
            IList<string> lstUpdateLocalTimes = new List<string>();
            int iResult = -1;
            bool Is_EncRecord = false;
            ulong HumanID_EncSave = 0;

            NHibernateSessionUtility.Instance.MySession = MySession;
            NHibernateSessionUtility.Instance.MACAddress = MACAddress;
            ITransaction trans = null;
            bool bsavehit = false;
            IList<object> lstobjxml = new List<object>();
            try
            {
                #region DB Transaction
                if (deleteList != null)
                {
                    Delete(deleteList, MySession);
                    if (deleteList.Count > 0 && (deleteList[0].GetType().Name.ToUpper() == "PATIENTRESULTS" || deleteList[0].GetType().Name.ToUpper() == "ADDENDUMNOTES" || deleteList[0].GetType().Name.ToUpper() == "ENCOUNTER"))
                        bResetLocalTime = true;
                }

                if (saveList != null)
                {
                    if (saveList.Count > 0 && (saveList[0].GetType().Name.ToUpper() == "PATIENTRESULTS" || saveList[0].GetType().Name.ToUpper() == "ADDENDUMNOTES" || saveList[0].GetType().Name.ToUpper() == "ENCOUNTER"))
                    {
                        bResetLocalTime = true;
                        for (int iCount = 0; iCount < saveList.Count; iCount++)
                            lstSaveLocalTimes.Add(saveList[iCount].GetType().GetProperty("Local_Time").GetValue(saveList[iCount], null).ToString());
                        if (saveList[0].GetType().Name.ToUpper() == "ENCOUNTER")
                        {
                            Is_EncRecord = true;
                            IList<Encounter> enc = (IList<Encounter>)saveList;
                            HumanID_EncSave = enc[0].Human_ID;
                        }
                    }
                    Save(saveList, MySession);
                }

                if (updateList != null)
                {
                    if (updateList.Count > 0 && (updateList[0].GetType().Name.ToUpper() == "PATIENTRESULTS" || updateList[0].GetType().Name.ToUpper() == "ADDENDUMNOTES" || updateList[0].GetType().Name.ToUpper() == "ENCOUNTER"))
                    {
                        bResetLocalTime = true;
                        for (int iCount = 0; iCount < updateList.Count; iCount++)
                            lstUpdateLocalTimes.Add(updateList[iCount].GetType().GetProperty("Local_Time").GetValue(updateList[iCount], null).ToString());
                        if (updateList[0].GetType().Name.ToUpper() == "ENCOUNTER")
                        {
                            Is_EncRecord = true;
                            IList<Encounter> enc = (IList<Encounter>)updateList;
                            HumanID_EncSave = enc[0].Human_ID;
                        }
                    }
                    Update(updateList, MySession);
                }
                iResult = 0;
                MySession.Flush();
                if (bSaveInXML && EncounterOrHumanId == 0 && saveList != null && saveList.Count > 0 && saveList[0].GetType().Name.ToUpper() == "HUMAN")
                {
                    if (saveList != null && saveList.Count > 0)
                    {
                        EncounterOrHumanId = Convert.ToUInt32(saveList[0].GetType().GetProperty("Id").GetValue(saveList[0], null));
                       // string HumanFileName = "Human" + "_" + EncounterOrHumanId + ".xml";
                       // string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);

                        string sDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("Template_XML");
                        string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                        XmlDocument itemDoc = new XmlDocument();
                        XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                        itemDoc.Load(XmlText);
                        XmlNodeList xmlnode = itemDoc.GetElementsByTagName("EncounterDetails");
                        xmlnode[0].ParentNode.RemoveChild(xmlnode[0]);
                        XmlText.Close();
                        //itemDoc.Save(strXmlHumanFilePath);


                        int trycount = 0;
                    trytosaveagain:
                        try
                        {
                            //itemDoc.Save(strXmlHumanFilePath);
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

                                string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','','','','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
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
                #endregion

                #region XML Transaction

                GenerateXml XmlObjHuman = null;
                if (Is_EncRecord)
                {
                    XmlObjHuman = new GenerateXml();
                }
                if (bSaveInXML)
                {
                    if (bResetLocalTime)
                    {
                        if (saveList != null && saveList.Count > 0)
                        {
                            for (int iCount = 0; iCount < saveList.Count; iCount++)
                                saveList[iCount].GetType().GetProperty("Local_Time").SetValue(saveList[iCount], lstSaveLocalTimes[iCount], null);
                        }
                        if (updateList != null && updateList.Count > 0)
                        {
                            for (int iCount = 0; iCount < updateList.Count; iCount++)
                                updateList[iCount].GetType().GetProperty("Local_Time").SetValue(updateList[iCount], lstUpdateLocalTimes[iCount], null);
                        }
                    }
                    if (deleteList != null && deleteList.Count > 0)
                    {
                        IList<object> lstObj = deleteList.Cast<object>().ToList();
                        lstobjxml = lstObj;
                        XMLObj.GenerateXmlDelete(lstObj, EncounterOrHumanId, sGeneralNotesText, false, XMLObj);
                        bsavehit = true;
                    }
                    if (!bSaveStatic)
                    {
                        IList<T> SaveUpdateList = new List<T>();
                        if (saveList != null && saveList.Count > 0)
                            SaveUpdateList = saveList;
                        if (updateList != null && updateList.Count > 0)
                            SaveUpdateList = SaveUpdateList.Concat(updateList).ToList();
                        if (SaveUpdateList.Count > 0)
                        {
                            IList<object> lstObj = SaveUpdateList.Cast<object>().ToList();
                            lstobjxml = lstObj;
                            XMLObj.GenerateXmlSave(lstObj, EncounterOrHumanId, sGeneralNotesText, false, false, false, false,ref XMLObj);
                            if (XmlObjHuman != null)
                            {
                                XmlObjHuman.GenerateXmlSave(lstObj, HumanID_EncSave, sGeneralNotesText, true, false, false, false,ref XmlObjHuman);
                            }
                            bsavehit = true;
                        }
                    }
                    else
                    {
                        if (saveList != null && saveList.Count > 0)
                        {
                            IList<object> lstObj = saveList.Cast<object>().ToList();
                            lstobjxml = lstObj;
                            XMLObj.GenerateXmlSaveStatic(lstObj, EncounterOrHumanId, sGeneralNotesText, false, XMLObj);
                            if (XmlObjHuman != null)
                                XmlObjHuman.GenerateXmlSaveStatic(lstObj, HumanID_EncSave, sGeneralNotesText, true, XmlObjHuman);
                            bsavehit = true;
                        }
                        if (updateList != null && updateList.Count > 0)
                        {
                            IList<object> lstObj = updateList.Cast<object>().ToList();
                            lstobjxml = lstObj;
                            XMLObj.GenerateXmlUpdate(lstObj, EncounterOrHumanId, sGeneralNotesText, false, XMLObj);
                            if (XmlObjHuman != null)
                                XmlObjHuman.GenerateXmlUpdate(lstObj, HumanID_EncSave, sGeneralNotesText, true, XmlObjHuman);
                            bsavehit = true;
                        }
                    }
                    #region XML type
                    string FileName = "Encounter" + "_" + EncounterOrHumanId + ".xml";
                    if (lstobjxml.Count > 0)
                    {
                        if (ilstHumanXml == null || ilstHumanXml.Count == 0)
                        {
                            SetHumanXmlList();
                        }

                        string SourceName = lstobjxml[0].GetType().Name;
                        string GeneralNotesList = SourceName + sGeneralNotesText + "List";
                        if (ilstHumanXml.Contains(GeneralNotesList) || ilstHumanXml.Contains(SourceName))
                        {
                            FileName = FileName.Replace("Encounter", "Human");
                        }
                        if (XmlObjHuman != null && XmlObjHuman.itemDoc.InnerXml != "")
                        {
                            //XmlObjHuman.itemDoc.Save(XmlObjHuman.strXmlFilePath);
                            int trycount = 0;
                        trytosaveagain:
                            try
                            {
                                //XmlObjHuman.itemDoc.Save(XmlObjHuman.strXmlFilePath);

                                WriteBlob(HumanID_EncSave, XmlObjHuman.itemDoc, MySession, saveList, updateList, deleteList, XmlObjHuman, false);
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
                    #endregion
                    if (bsavehit && XMLObj.itemDoc.InnerXml == "") //.itemDoc.BaseURI == "")
                    {
                        throw new Exception("$Transaction XML: '" + FileName + "' not found. Please contact support team to generate the XML. ");
                    }
                }

                #endregion
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException.Source.ToString().Contains("MySQL") == true)
                {
                    MySql.Data.MySqlClient.MySqlException excep = (MySql.Data.MySqlClient.MySqlException)ex.InnerException;

                    //Deadlock Error
                    if (excep.Number == 1213)
                    {
                        iResult = 2;
                        throw new Exception(ex.Message);
                    }
                    else
                    {
                        iResult = 1;
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    iResult = 1;
                    if (trans != null)
                        trans.Rollback();
                    throw new Exception(ex.Message);
                }
            }
            catch (SoapException e)
            {
                trans.Rollback();
                throw new Exception(e.Message);
            }
            catch (Exception ep)
            {
                iResult = 1;
                if (trans != null)
                    trans.Rollback();
                throw new Exception(ep.Message);
            }
            finally
            {
            }
            Webservicetrigger(ref saveList, ref updateList, deleteList);
            return iResult;
        }

        public void Webservicetrigger(ref IList<T> saveList, ref IList<T> updateList, IList<T> deleteList)
        {
            #region AuditLogEntry
            string Is_Audit_log = "N";
            //Added by Jisha for Delete AuditLog 
            if (System.Configuration.ConfigurationManager.AppSettings["Is_Audit_Log"] != null)
                Is_Audit_log = System.Configuration.ConfigurationManager.AppSettings["Is_Audit_Log"];

            if (Is_Audit_log == "Y")
            {
                if (deleteList != null || saveList != null || updateList != null)
                {
                    if (saveList != null && saveList.Count > 0)
                        CreateAuditLogList(saveList);
                    if (NHibernateSessionUtility.Instance.MyAuditLogList.Count > 0)
                    {
                        string Project_Name = string.Empty;
                        if (System.Configuration.ConfigurationManager.AppSettings["ProjectName"] != null)
                            Project_Name = System.Configuration.ConfigurationManager.AppSettings["ProjectName"];
                        string HumanID;
                        string Entity_Name;
                        string Entity_ID;
                        string Attribute_Name;
                        string OldValue;
                        string NewValue;
                        string TransactionType;
                        string TransactionBy;
                        string Transaction_Date_Time;
                        if (NHibernateSessionUtility.Instance.MyAuditLogList != null && NHibernateSessionUtility.Instance.MyAuditLogList.Count > 0)
                        {
                            string[] lstHumanID = NHibernateSessionUtility.Instance.MyAuditLogList.Select(a => a.Human_ID.ToString()).ToArray();
                            HumanID = string.Join("|~|", lstHumanID.Select(x => x.ToString()).ToArray());
                            string[] lstEntity_Name = NHibernateSessionUtility.Instance.MyAuditLogList.Select(a => a.Entity_Name.ToString()).ToArray();
                            Entity_Name = string.Join("|~|", lstEntity_Name);
                            string[] lstEntity_ID = NHibernateSessionUtility.Instance.MyAuditLogList.Select(a => a.Entity_Id.ToString()).ToArray();
                            Entity_ID = string.Join("|~|", lstEntity_ID);
                            string[] lstAttribute_Name = NHibernateSessionUtility.Instance.MyAuditLogList.Select(a => a.Attribute.ToString()).ToArray();
                            Attribute_Name = string.Join("|~|", lstAttribute_Name);
                            string[] lstOldValue = NHibernateSessionUtility.Instance.MyAuditLogList.Select(a => a.Old_Value.ToString()).ToArray();
                            OldValue = string.Join("|~|", lstOldValue);
                            string[] lstNewValue = NHibernateSessionUtility.Instance.MyAuditLogList.Select(a => a.New_Value.ToString()).ToArray();
                            NewValue = string.Join("|~|", lstNewValue);
                            string[] lstTransactionType = NHibernateSessionUtility.Instance.MyAuditLogList.Select(a => a.Transaction_Type.ToString()).ToArray();
                            TransactionType = string.Join("|~|", lstTransactionType);
                            string[] lstTransactionBy = NHibernateSessionUtility.Instance.MyAuditLogList.Select(a => a.Transaction_By.ToString()).ToArray();
                            TransactionBy = string.Join("|~|", lstTransactionBy);
                            string[] lstTransaction_Date_Time = NHibernateSessionUtility.Instance.MyAuditLogList.Select(a => a.Transaction_Date_And_Time.ToString()).ToArray();
                            Transaction_Date_Time = string.Join("|~|", lstTransaction_Date_Time);
                            TriggerService(Project_Name, HumanID, Entity_Name, Entity_ID
                                           , Attribute_Name, OldValue, NewValue, TransactionType, TransactionBy, Transaction_Date_Time);
                        }
                        NHibernateSessionUtility.Instance.MyAuditLogList.Clear();
                    }
                }
            }

            #endregion
        }

        public void WriteBlob(ulong EntityID, XmlDocument xmlDoc, ISession MySession, IList<T> saveList, IList<T> updateList, IList<T> deleteList, GenerateXml objGenerateXml, Boolean bIsEncounterXMLCreate)
        {
            if (EntityID == 0)
            {
                return;
            }

            string sXMLType = "";
            string tagname = "";
            if (saveList != null && saveList.Count > 0)
            {
                tagname = saveList[0].GetType().Name;
            }
            else if (updateList != null && updateList.Count > 0)
            {
                tagname = updateList[0].GetType().Name;
            }
            else if (deleteList != null && deleteList.Count > 0)
            {
                tagname = deleteList[0].GetType().Name;

            }

            //if (!tagname.Contains("List"))
                tagname = tagname + "List";

            IList<MapXMLBlob> ilstXMLBlob = new List<MapXMLBlob>();
            ilstXMLBlob = NHibernateSessionUtility.Instance.MyMapXMLBlobList.Where(a => a.XML_Tag_Name == tagname).ToList();
            if (ilstXMLBlob.Count > 0)
            {
                if (ilstXMLBlob.Count == 2)
                {
                    if (objGenerateXml!=null && EntityID == objGenerateXml.ulEncounterID)
                    {
                        sXMLType = "Blob_Encounter";
                    }
                   else if (objGenerateXml != null && EntityID == objGenerateXml.ulHumanID)
                    {
                        sXMLType = "Blob_Human";
                    }
                    else
                    {
                        sXMLType = ilstXMLBlob[0].Table_Name;
                    }
                }
                else
                {
                    sXMLType = ilstXMLBlob[0].Table_Name;
                }
                string sXMLContent = String.Empty;
                if (sXMLType == "Blob_Human")
                {
                    Human_Blob objhumanblob = new Human_Blob();
                    IList<Human_Blob> ilstHumanBlob = new List<Human_Blob>();
                    HumanBlobManager HumanBlobMngr = new HumanBlobManager();
                    objhumanblob.Human_ID = EntityID;
                    objhumanblob.Id = EntityID;
                    if (objGenerateXml != null)
                    {
                        objhumanblob.Version = objGenerateXml.iHumanBlobVersion;
                        objhumanblob.Created_By = objGenerateXml.sCreatedBy;
                        objhumanblob.Created_Date_And_Time = objGenerateXml.dtCreatedDateandTime;
                    }
                    byte[] bytes = null;
                    try
                    {
                        bytes = System.Text.Encoding.Default.GetBytes(xmlDoc.OuterXml);
                    }
                    catch (Exception ex)
                    {

                    }
                    objhumanblob.Human_XML = bytes;
                    if (saveList != null && saveList.Count > 0 && saveList[0].GetType().Name.ToUpper() == "HUMAN")
                    {
                        objhumanblob.Created_By = saveList[0].GetType().GetProperty("Created_By").GetValue(saveList[0], null) as string;
                        objhumanblob.Created_Date_And_Time = Convert.ToDateTime(saveList[0].GetType().GetProperty("Created_Date_And_Time").GetValue(saveList[0], null));
                        ilstHumanBlob.Add(objhumanblob);

                        HumanBlobMngr.SaveHumanBlobWithoutTransaction(ilstHumanBlob, null, MySession, string.Empty);
                    }
                    else
                    {
                        IList<Human_Blob> ilstInsertHumanBlob = null;
                        if (updateList != null && updateList.Count > 0)
                        {
                            objhumanblob.Modified_By = updateList[0].GetType().GetProperty("Modified_By").GetValue(updateList[0], null) as string;
                            //objhumanblob.Modified_Date_And_Time = Convert.ToDateTime(updateList[0].GetType().GetProperty("Created_Date_And_Time").GetValue(saveList[0], null));
                            if (updateList[0].GetType().GetProperty("Modified_Date_and_Time") != null)
                            {
                                objhumanblob.Modified_Date_And_Time = Convert.ToDateTime(updateList[0].GetType().GetProperty("Modified_Date_and_Time").GetValue(updateList[0], null));
                            }
                            else if (updateList[0].GetType().GetProperty("Modified_Date_And_Time") != null)
                            {
                                objhumanblob.Modified_Date_And_Time = Convert.ToDateTime(updateList[0].GetType().GetProperty("Modified_Date_And_Time").GetValue(updateList[0], null));
                            }
                        }
                        else if (deleteList != null && deleteList.Count > 0)
                        {
                            objhumanblob.Modified_By = deleteList[0].GetType().GetProperty("Modified_By").GetValue(deleteList[0], null) as string;
                            //objhumanblob.Modified_Date_And_Time = Convert.ToDateTime(deleteList[0].GetType().GetProperty("Created_Date_And_Time").GetValue(saveList[0], null));
                            if (deleteList[0].GetType().GetProperty("Modified_Date_and_Time") != null)
                            {
                                objhumanblob.Modified_Date_And_Time = Convert.ToDateTime(deleteList[0].GetType().GetProperty("Modified_Date_and_Time").GetValue(deleteList[0], null));
                            }
                            else if (deleteList[0].GetType().GetProperty("Modified_Date_And_Time") != null)
                            {
                                objhumanblob.Modified_Date_And_Time = Convert.ToDateTime(deleteList[0].GetType().GetProperty("Modified_Date_And_Time").GetValue(deleteList[0], null));
                            }
                        }
                        ilstHumanBlob.Add(objhumanblob);

                        HumanBlobMngr.SaveHumanBlobWithoutTransaction(ilstInsertHumanBlob, ilstHumanBlob, MySession, string.Empty);
                    }

                }
                else if (sXMLType == "Blob_Encounter")
                {
                    Encounter_Blob objEncounterblob = new Encounter_Blob();
                    IList<Encounter_Blob> ilstEncounterBlob = new List<Encounter_Blob>();
                    EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
                    objEncounterblob.Encounter_ID = EntityID;
                    objEncounterblob.Id = EntityID;
                    if (objGenerateXml != null)
                    {
                        objEncounterblob.Version = objGenerateXml.iEncounterBlobVersion;
                        objEncounterblob.Created_By = objGenerateXml.sCreatedBy;
                        objEncounterblob.Created_Date_And_Time = objGenerateXml.dtCreatedDateandTime;
                    }

                    byte[] bytes = null;
                    try
                    {
                        bytes = System.Text.Encoding.Default.GetBytes(xmlDoc.OuterXml);
                    }
                    catch (Exception ex)
                    {

                    }
                    objEncounterblob.Encounter_XML = bytes;
                    //if (saveList != null && saveList.Count > 0 && saveList[0].GetType().Name.ToUpper() == "ENCOUNTER") //To be changed for Process_Encounter
                    if (bIsEncounterXMLCreate == true)
                    {
                        if (updateList != null && updateList.Count > 0)
                        {
                            objEncounterblob.Created_By = updateList[0].GetType().GetProperty("Modified_By").GetValue(updateList[0], null) as string;
                            objEncounterblob.Created_Date_And_Time = Convert.ToDateTime(updateList[0].GetType().GetProperty("Modified_Date_and_Time").GetValue(updateList[0], null));
                            ilstEncounterBlob.Add(objEncounterblob);

                            EncounterBlobMngr.SaveEncounterBlobWithoutTransaction(ilstEncounterBlob, null, MySession, string.Empty);
                        }
                        else if (saveList != null && saveList.Count > 0)
                        {
                            objEncounterblob.Created_By = saveList[0].GetType().GetProperty("Created_By").GetValue(saveList[0], null) as string;
                            if (saveList[0].GetType().GetProperty("Created_Date_And_Time") != null)
                            {
                                objEncounterblob.Created_Date_And_Time = Convert.ToDateTime(saveList[0].GetType().GetProperty("Created_Date_And_Time").GetValue(saveList[0], null));
                            }
                            else
                            {
                                objEncounterblob.Created_Date_And_Time = Convert.ToDateTime(DateTime.Now);
                            }
                            ilstEncounterBlob.Add(objEncounterblob);

                            EncounterBlobMngr.SaveEncounterBlobWithoutTransaction(ilstEncounterBlob, null, MySession, string.Empty);
                        }
                    }
                    else
                    {
                        IList<Encounter_Blob> ilstInsertEncounterBlob = null;
                        if (updateList != null && updateList.Count > 0)
                        {
                            objEncounterblob.Modified_By = updateList[0].GetType().GetProperty("Modified_By").GetValue(updateList[0], null) as string;
                            if (updateList[0].GetType().GetProperty("Modified_Date_and_Time") != null)
                            {
                                objEncounterblob.Modified_Date_And_Time = Convert.ToDateTime(updateList[0].GetType().GetProperty("Modified_Date_and_Time").GetValue(updateList[0], null));
                            }
                            else if (updateList[0].GetType().GetProperty("Modified_Date_And_Time") != null)
                            {
                                objEncounterblob.Modified_Date_And_Time = Convert.ToDateTime(updateList[0].GetType().GetProperty("Modified_Date_And_Time").GetValue(updateList[0], null));
                            }

                        }
                        else if (deleteList != null && deleteList.Count > 0)
                        {
                            objEncounterblob.Modified_By = deleteList[0].GetType().GetProperty("Modified_By").GetValue(deleteList[0], null) as string;
                            if (deleteList[0].GetType().GetProperty("Modified_Date_and_Time") != null)
                            {
                                objEncounterblob.Modified_Date_And_Time = Convert.ToDateTime(deleteList[0].GetType().GetProperty("Modified_Date_and_Time").GetValue(deleteList[0], null));
                            }
                            else if (deleteList[0].GetType().GetProperty("Modified_Date_And_Time") != null)
                            {
                                objEncounterblob.Modified_Date_And_Time = Convert.ToDateTime(deleteList[0].GetType().GetProperty("Modified_Date_And_Time").GetValue(deleteList[0], null));
                            }
                        }
                        ilstEncounterBlob.Add(objEncounterblob);

                        EncounterBlobMngr.SaveEncounterBlobWithoutTransaction(ilstInsertEncounterBlob, ilstEncounterBlob, MySession, string.Empty);
                    }
                }

            }
            else
            {

            }
        }
        public static IList<object> ReadBlob(ulong EntityID, IList<string> ilstTagName)
        {
            string sXMLType = "";

            IList<object> ilstResult = new List<object>();
            if (EntityID == 0)
            {
                return ilstResult;
            }
            IList<object> ilstEntity = new List<object>();
            XmlDocument xmlDoc = new XmlDocument();
            string sXMLContent = String.Empty;
            IList<MapXMLBlob> ilstXMLBlob = new List<MapXMLBlob>();
            ilstXMLBlob = NHibernateSessionUtility.Instance.MyMapXMLBlobList.Where(a => a.XML_Tag_Name == ilstTagName[0].ToString()).ToList();
            if (ilstXMLBlob.Count > 0)
            {
                sXMLType = ilstXMLBlob[0].Table_Name;
                if (sXMLType == "Blob_Human")
                {
                    HumanBlobManager HumanBlobMngr = new HumanBlobManager();
                    IList<Human_Blob> ilstHumanBlob = HumanBlobMngr.GetHumanBlob(EntityID);
                    if (ilstHumanBlob.Count > 0)
                    {
                        sXMLContent = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                        if (sXMLContent.Substring(0, 1) != "<")
                            sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                        xmlDoc.LoadXml(sXMLContent);
                    }
                    else
                    {
                        throw new Exception("Human XML is not found");
                    }
                }
                else if (sXMLType == "Blob_Encounter")
                {
                    EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
                    IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(EntityID);
                    if (ilstEncounterBlob.Count > 0)
                    {
                        try
                        {
                            sXMLContent = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                            if (sXMLContent.Substring(0, 1) != "<")
                                sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                            xmlDoc.LoadXml(sXMLContent);
                        }
                        catch
                        {
                            throw new Exception("Encounter XML is invalid");
                        }
                    }
                    else
                    {
                        throw new Exception("Encounter XML is not found");
                    }
                }


                try
                {
                    XmlNodeList xmlTagName = null;

                    for (int iInputTagCount = 0; iInputTagCount < ilstTagName.Count; iInputTagCount++)
                    {
                        ilstEntity = new List<object>();
                        if (xmlDoc.GetElementsByTagName(ilstTagName[iInputTagCount]) != null && xmlDoc.GetElementsByTagName(ilstTagName[iInputTagCount]).Count > 0)
                        {
                            xmlTagName = xmlDoc.GetElementsByTagName(ilstTagName[iInputTagCount])[0].ChildNodes;
                            if (xmlTagName.Count > 0)
                            {
                                ilstEntity = new List<object>();
                                for (int iXMLTagCount = 0; iXMLTagCount < xmlTagName.Count; iXMLTagCount++)
                                {
                                    string TagName = xmlTagName[iXMLTagCount].Name;
                                    IEnumerable<PropertyInfo> propInfo = null;
                                    object objEntity = null;

                                    if (TagName == "Human")
                                    {
                                        Human objHuman = new Human();
                                        objEntity = (object)objHuman;
                                    }
                                    else if (TagName == "Orders")
                                    {
                                        Orders objOrders = new Orders();
                                        objEntity = (object)objOrders;
                                    }
                                    else
                                    {
                                        XmlSerializer xmlserializer = FillSerializer(TagName);//new XmlSerializer(typeof(ImmunizationHistory));
                                        objEntity = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[iXMLTagCount])) as object;
                                    }


                                    if (objEntity != null)
                                    {
                                        propInfo = from obji in ((object)objEntity).GetType().GetProperties() select obji;

                                        for (int iAttributeCount = 0; iAttributeCount < xmlTagName[iXMLTagCount].Attributes.Count; iAttributeCount++)
                                        {
                                            XmlNode nodevalue = xmlTagName[iXMLTagCount].Attributes[iAttributeCount];
                                            {
                                                foreach (PropertyInfo property in propInfo)
                                                {
                                                    if (property.Name == nodevalue.Name)
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
                                                            property.SetValue(objEntity, Convert.ToUInt64(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
                                                            property.SetValue(objEntity, Convert.ToString(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            property.SetValue(objEntity, Convert.ToDateTime(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
                                                            property.SetValue(objEntity, Convert.ToInt32(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                                                            property.SetValue(objEntity, Convert.ToDecimal(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "DOUBLE")
                                                            property.SetValue(objEntity, Convert.ToDouble(nodevalue.Value), null);
                                                        else
                                                            property.SetValue(objEntity, nodevalue.Value, null);
                                                    }
                                                }
                                            }

                                        }
                                        ilstEntity.Add(objEntity);
                                    }

                                }

                            }
                            else
                            {
                                ilstResult.Add(null);
                            }
                            ilstResult.Add((object)ilstEntity);
                        }
                        else
                        {
                            ilstResult.Add(null);
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " " + ex.StackTrace);
                }
            }
            else
            {
                throw new Exception("XML Tag Name is not found in the lookup table");
            }

            return ilstResult;
        }

        public static XmlSerializer FillSerializer(string sEntityName)
        {
            XmlSerializer xmlserializer = null;

            switch (sEntityName)
            {
                case "Encounter":
                    {
                        xmlserializer = new XmlSerializer(typeof(Encounter));
                        break;
                    }
                case "PatientInsuredPlan":
                    {
                        xmlserializer = new XmlSerializer(typeof(PatientInsuredPlan));
                        break;
                    }
                case "ProblemList":
                    {
                        xmlserializer = new XmlSerializer(typeof(ProblemList));
                        break;
                    }
                case "ChiefComplaints":
                    {
                        xmlserializer = new XmlSerializer(typeof(ChiefComplaints));
                        break;
                    }
                case "Healthcare_Questionnaire":
                    {
                        xmlserializer = new XmlSerializer(typeof(Healthcare_Questionnaire));
                        break;
                    }
                case "Test":
                    {
                        xmlserializer = new XmlSerializer(typeof(Test));
                        break;
                    }
                case "PastMedicalHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(PastMedicalHistory));
                        break;
                    }
                case "PastMedicalHistoryMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(PastMedicalHistoryMaster));
                        break;
                    }
                case "SocialHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(SocialHistory));
                        break;
                    }
                case "SocialHistoryMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(SocialHistoryMaster));
                        break;
                    }

                case "SurgicalHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(SurgicalHistory));
                        break;
                    }
                case "SurgicalHistoryMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(SurgicalHistoryMaster));
                        break;
                    }
                case "FamilyHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(FamilyHistory));
                        break;
                    }
                case "FamilyDisease":
                    {
                        xmlserializer = new XmlSerializer(typeof(FamilyDisease));
                        break;
                    }
                case "FamilyHistoryMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(FamilyHistoryMaster));
                        break;
                    }
                case "FamilyDiseaseMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(FamilyDiseaseMaster));
                        break;
                    }
                case "FileManagementIndex":
                    {
                        xmlserializer = new XmlSerializer(typeof(FileManagementIndex));
                        break;
                    }
                case "ImmunizationHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(ImmunizationHistory));
                        break;
                    }
                case "ImmunizationMasterHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(ImmunizationMasterHistory));
                        break;
                    }
                case "NonDrugAllergy":
                    {
                        xmlserializer = new XmlSerializer(typeof(NonDrugAllergy));
                        break;
                    }
                case "NonDrugAllergyMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(NonDrugAllergyMaster));
                        break;
                    }
                case "AdvanceDirective":
                    {
                        xmlserializer = new XmlSerializer(typeof(AdvanceDirective));
                        break;
                    }
                case "AdvanceDirectiveMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(AdvanceDirectiveMaster));
                        break;
                    }
                case "PhysicianPatient":
                    {
                        xmlserializer = new XmlSerializer(typeof(PhysicianPatient));
                        break;
                    }
                case "PhysicianPatientMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(PhysicianPatientMaster));
                        break;
                    }

                case "HospitalizationHistory":
                    {
                        xmlserializer = new XmlSerializer(typeof(HospitalizationHistory));
                        break;
                    }
                case "HospitalizationHistoryMaster":
                    {
                        xmlserializer = new XmlSerializer(typeof(HospitalizationHistoryMaster));
                        break;
                    }
                case "ROS":
                    {
                        xmlserializer = new XmlSerializer(typeof(ROS));
                        break;
                    }
                case "PatientResults":
                    {
                        xmlserializer = new XmlSerializer(typeof(PatientResults));
                        break;
                    }
                case "Examination":
                    {
                        xmlserializer = new XmlSerializer(typeof(Examination));
                        break;
                    }
                case "Assessment":
                    {
                        xmlserializer = new XmlSerializer(typeof(Assessment));
                        break;
                    }
                case "OrdersSubmit":
                    {
                        xmlserializer = new XmlSerializer(typeof(OrdersSubmit));
                        break;
                    }
                case "Orders":
                    {
                        xmlserializer = new XmlSerializer(typeof(Orders));
                        break;
                    }
                case "OrdersAssessment":
                    {
                        xmlserializer = new XmlSerializer(typeof(OrdersAssessment));
                        break;
                    }
                case "ReferralOrder":
                    {
                        xmlserializer = new XmlSerializer(typeof(ReferralOrder));
                        break;
                    }
                case "ReferralOrdersAssessment":
                    {
                        xmlserializer = new XmlSerializer(typeof(ReferralOrdersAssessment));
                        break;
                    }
                case "Immunization":
                    {
                        xmlserializer = new XmlSerializer(typeof(Immunization));
                        break;
                    }
                case "InHouseProcedure":
                    {
                        xmlserializer = new XmlSerializer(typeof(InHouseProcedure));
                        break;
                    }
                case "EAndMCoding":
                    {
                        xmlserializer = new XmlSerializer(typeof(EAndMCoding));
                        break;
                    }
                case "EandMCodingICD":
                    {
                        xmlserializer = new XmlSerializer(typeof(EandMCodingICD));
                        break;
                    }
                case "TreatmentPlan":
                    {
                        xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
                        break;
                    }
                case "CarePlan":
                    {
                        xmlserializer = new XmlSerializer(typeof(CarePlan));
                        break;
                    }
                case "Documents":
                    {
                        xmlserializer = new XmlSerializer(typeof(Documents));
                        break;
                    }
                case "PreventiveScreen":
                    {
                        xmlserializer = new XmlSerializer(typeof(PreventiveScreen));
                        break;
                    }
                case "GeneralNotes":
                    {
                        xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                        break;
                    }
                case "GeneralNotesROS":
                    {
                        xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                        break;
                    }
                case "GeneralNotesROSGeneralNotes":
                    {
                        xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                        break;
                    }
                case "Rcopia_Allergy":
                    {
                        xmlserializer = new XmlSerializer(typeof(Rcopia_Allergy));
                        break;
                    }
                case "Rcopia_Medication":
                    {
                        xmlserializer = new XmlSerializer(typeof(Rcopia_Medication));
                        break;
                    }
                case "Rcopia_Prescription_List":
                    {
                        xmlserializer = new XmlSerializer(typeof(Rcopia_Prescription_List));
                        break;
                    }
                case "AddendumNotes":
                    {
                        xmlserializer = new XmlSerializer(typeof(AddendumNotes));
                        break;
                    }
                case "Human":
                    {
                        xmlserializer = new XmlSerializer(typeof(Human));
                        break;
                    }
                case "PotentialDiagnosis":
                    {
                        xmlserializer = new XmlSerializer(typeof(PotentialDiagnosis));
                        break;
                    }
                case "GeneralNotesSelectedAssessment":
                    {
                        xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                        break;
                    }

            }
            return xmlserializer;
        }
        private void SetHumanXmlList()
        {
            ilstHumanXml = new List<string>();
            ilstHumanXml.Add("GeneralNotesFamilyHistoryList");
            ilstHumanXml.Add("ProblemList");
            ilstHumanXml.Add("PastMedicalHistory");
            ilstHumanXml.Add("PastMedicalHistoryMaster");
            ilstHumanXml.Add("SocialHistory");
            ilstHumanXml.Add("SocialHistoryMaster");
            ilstHumanXml.Add("SurgicalHistory");
            ilstHumanXml.Add("SurgicalHistoryMaster");
            ilstHumanXml.Add("FamilyHistory");
            ilstHumanXml.Add("FamilyDisease");
            ilstHumanXml.Add("FamilyHistoryMaster");
            ilstHumanXml.Add("FamilyDiseaseMaster");
            ilstHumanXml.Add("ImmunizationHistory");
            ilstHumanXml.Add("NonDrugAllergy");
            ilstHumanXml.Add("AdvanceDirective");
            ilstHumanXml.Add("AdvanceDirectiveMaster");
            ilstHumanXml.Add("HospitalizationHistory");
            ilstHumanXml.Add("HospitalizationHistoryMaster");
            ilstHumanXml.Add("GeneralNotesPastMedicalHistoryList");
            ilstHumanXml.Add("Immunization");
            ilstHumanXml.Add("GeneralNotesNonDrugAllergyList");
            ilstHumanXml.Add("Rcopia_Allergy");
            ilstHumanXml.Add("Rcopia_Prescription_List");
            ilstHumanXml.Add("Rcopia_Medication");
            ilstHumanXml.Add("PatientResults");
            ilstHumanXml.Add("InHouseProcedure");
            ilstHumanXml.Add("GeneralNotesSocialHistoryList");
            ilstHumanXml.Add("PhysicianPatient");
            ilstHumanXml.Add("PhysicianPatientMaster");
            ilstHumanXml.Add("PatientResultsList");
            ilstHumanXml.Add("PatientInsuredPlan");
            ilstHumanXml.Add("ReferralOrder");
            ilstHumanXml.Add("ReferralOrdersAssessment");
            ilstHumanXml.Add("Orders");
            ilstHumanXml.Add("OrdersSubmit");
            ilstHumanXml.Add("OrdersAssessment");
            ilstHumanXml.Add("FileManagementIndex");
            ilstHumanXml.Add("Human");
            ilstHumanXml.Add("PotentialDiagnosis");
        }


        public void SaveDeleteForBuckets(IList<T> saveList, string tableName)
        {
            iTryCount = 0;

        TryAgain:
            ISession session = Session.GetISession();

            NHibernateSessionUtility.Instance.MySession = session;
            ITransaction trans = null;

            try
            {
                trans = session.BeginTransaction();
                //ICriteria crt = session.CreateCriteria(typeof(T));
                //IList<T> deleteList = crt.List<T>();
                //if (deleteList != null)
                {
                    //Delete(deleteList, session);
                    ISQLQuery sql = session.CreateSQLQuery("Delete from " + tableName);
                    sql.ExecuteUpdate();
                }

                if (saveList != null)
                {
                    session.Clear();
                    Save(saveList, session);
                }

                session.Flush();
                trans.Commit();
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException.Source.ToString().Contains("MySQL") == true)
                {
                    MySql.Data.MySqlClient.MySqlException excep = (MySql.Data.MySqlClient.MySqlException)ex.InnerException;

                    //Deadlock Error
                    if (excep.Number == 1213 && iTryCount < 5)
                    {
                        iTryCount++;
                        goto TryAgain;
                    }
                    else
                    {
                        trans.Rollback();
                        //session.Close();
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    trans.Rollback();
                    //session.Close();
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception e)
            {
                trans.Rollback();
                //MySession.Close();
                throw new Exception(e.Message);
            }
            finally
            {
                session.Close();
            }
        }

        IList<string> IgnorePropList = new List<string> { "VERSION", "CREATED_BY", "CREATED_DATE_AND_TIME", "MODIFIED_BY", "MODIFIED_DATE_AND_TIME", "ID", "HUMAN_ID", "PHYSICIAN_ID", "ENCOUNTER_ID" };
        IList<string> IgnoreValList = new List<string> { "0", "0.0", "0.00", DateTime.MinValue.ToString(), string.Empty };

        public void CreateAuditLogList(IList<T> saveList)
        {
            string table_name = (typeof(T).AssemblyQualifiedName.ToString() != string.Empty && typeof(T).AssemblyQualifiedName.ToString().IndexOf(',') != -1) ? typeof(T).AssemblyQualifiedName.ToString().Split(',')[0] : string.Empty;
            if (NHibernateSessionUtility.Instance.AuditTrailTableList.Contains(table_name) == true)
            {
                if (saveList != null && saveList.Count > 0)
                {
                    for (int i = 0; i < saveList.Count; i++)
                    {
                        foreach (var prop in saveList[i].GetType().GetProperties())
                        {
                            if (prop.GetValue(saveList[i], null) != null && !IgnorePropList.Contains(prop.Name.ToString().ToUpper()))
                            {
                                if (!IgnoreValList.Contains(prop.GetValue(saveList[i], null).ToString().Trim()) && !prop.GetValue(saveList[i], null).ToString().Trim().Contains("System.Collections.Generic.List"))//BugID:50203
                                {
                                    AuditLog newaudit = new AuditLog();
                                    newaudit.Transaction_Type = "INSERT";
                                    newaudit.Attribute = prop.Name;
                                    newaudit.Old_Value = prop.GetValue(saveList[i], null).ToString();
                                    newaudit.New_Value = string.Empty;
                                    newaudit.Entity_Name = table_name;
                                    newaudit.Entity_Id = Convert.ToUInt64(saveList[i].GetType().GetProperty("Id").GetValue(saveList[i], null).ToString());
                                    if ((saveList[i].GetType().GetProperty("Human_ID") != null || saveList[i].GetType().GetProperty("Human_Id") != null))
                                        newaudit.Human_ID = saveList[i].GetType().GetProperty("Human_ID") == null ? Convert.ToInt32(saveList[i].GetType().GetProperty("Human_Id").GetValue(saveList[i], null).ToString()) : Convert.ToInt32(saveList[i].GetType().GetProperty("Human_ID").GetValue(saveList[i], null).ToString());
                                    if (table_name.Equals("Acurus.Capella.Core.DomainObjects.Human"))
                                        newaudit.Human_ID = Convert.ToInt32(saveList[i].GetType().GetProperty("Id").GetValue(saveList[i], null).ToString());
                                    newaudit.Transaction_By = saveList[i].GetType().GetProperty("Created_By").GetValue(saveList[i], null).ToString();
                                    newaudit.Transaction_Date_And_Time = saveList[i].GetType().GetProperty("Created_Date_And_Time") == null ? Convert.ToDateTime(saveList[i].GetType().GetProperty("Created_Date_and_Time").GetValue(saveList[i], null).ToString()) : Convert.ToDateTime(saveList[i].GetType().GetProperty("Created_Date_And_Time").GetValue(saveList[i], null).ToString());
                                    NHibernateSessionUtility.Instance.MyAuditLogList.Add(newaudit);
                                }
                            }
                        }
                    }
                }
            }

            /* if (DeleteList != null && DeleteList.Count > 0)
             {
                 for (int i = 0; i < DeleteList.Count; i++)
                 {
                     foreach (var prop in DeleteList[i].GetType().GetProperties())
                     {
                         if (prop.Name.ToUpper() == "ID")
                         {
                             AuditLog newaudit = new AuditLog();
                             newaudit.Transaction_Type = "DELETE";
                             newaudit.Attribute = prop.Name;
                             newaudit.Old_Value = prop.GetValue(DeleteList[i], null).ToString();
                             newaudit.New_Value = string.Empty;
                             newaudit.Entity_Name = DeleteList[i].GetType().Name;
                             newaudit.Entity_Id = Convert.ToInt32(DeleteList[i].GetType().GetProperty("Id").GetValue(saveList[0], null).ToString());
                             newaudit.Human_ID = Convert.ToInt32(DeleteList[i].GetType().GetProperty("Human_Id").GetValue(saveList[0], null).ToString());
                             newaudit.MAC_Address = string.Empty;
                             newaudit.Transaction_By = DeleteList[i].GetType().GetProperty("Modified_By").GetValue(saveList[0], null).ToString();
                             newaudit.Transaction_Date_And_Time = Convert.ToDateTime(DeleteList[i].GetType().GetProperty("Modified_Date_And_Time").GetValue(saveList[0], null).ToString());
                             NHibernateSessionUtility.Instance.MyAuditLogList.Add(newaudit);
                         }
                     }
                 }
             }
             if (UpdateList != null && UpdateList.Count > 0)
             {
                 for (int i = 0; i < UpdateList.Count; i++)
                 {
                     foreach (var prop in UpdateList[i].GetType().GetProperties())
                     {
                         AuditLog newaudit = new AuditLog();
                         newaudit.Transaction_Type = "UPDATE";
                         newaudit.Attribute = prop.Name;
                         newaudit.Old_Value = prop.GetValue(DeleteList[i], null).ToString();
                         newaudit.New_Value = string.Empty;
                         newaudit.Entity_Name = DeleteList[i].GetType().Name;
                         newaudit.Entity_Id = Convert.ToInt32(DeleteList[i].GetType().GetProperty("Id").GetValue(saveList[0], null).ToString());
                         newaudit.Human_ID = Convert.ToInt32(DeleteList[i].GetType().GetProperty("Human_Id").GetValue(saveList[0], null).ToString());
                         newaudit.MAC_Address = string.Empty;
                         newaudit.Transaction_By = DeleteList[i].GetType().GetProperty("Modified_By").GetValue(saveList[0], null).ToString();
                         newaudit.Transaction_Date_And_Time = Convert.ToDateTime(DeleteList[i].GetType().GetProperty("Modified_Date_And_Time").GetValue(saveList[0], null).ToString());
                         NHibernateSessionUtility.Instance.MyAuditLogList.Add(newaudit);
                     }
                 }
             }
             */
        }

        private ManualResetEvent allDone = new ManualResetEvent(false);
        HttpWebRequest request;
        public void TriggerService(string ProjectName, string _HumanID, string _EntityName, string _EntityID,
                                       string _AttributeName, string _OldValue, string _NewValue,
                                       string _TransactionType, string _Transaction_By, string _Transaction_Date_Time)
        {
            string ServiceURl = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["Audit_LoggingService"] != null)
                ServiceURl = System.Configuration.ConfigurationManager.AppSettings["Audit_LoggingService"];

            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(ServiceURl);
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.Method = "POST";
            int trycount = 0;

        callrequest:
            try
            {


                myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
                using (StreamWriter requestWriter = new StreamWriter(myRequest.GetRequestStream()))
                {

                    requestWriter.Write("&{0}={1}", HttpUtility.UrlEncode("ProjectName"), HttpUtility.UrlEncode(ProjectName.ToString()));
                    requestWriter.Write("&{0}={1}", HttpUtility.UrlEncode("_HumanID"), HttpUtility.UrlEncode(_HumanID));
                    requestWriter.Write("&{0}={1}", HttpUtility.UrlEncode("_EntityName"), HttpUtility.UrlEncode(_EntityName));
                    requestWriter.Write("&{0}={1}", HttpUtility.UrlEncode("_EntityID"), HttpUtility.UrlEncode(_EntityID));
                    requestWriter.Write("&{0}={1}", HttpUtility.UrlEncode("_AttributeName"), HttpUtility.UrlEncode(_AttributeName));
                    requestWriter.Write("&{0}={1}", HttpUtility.UrlEncode("_OldValue"), HttpUtility.UrlEncode(_OldValue));
                    requestWriter.Write("&{0}={1}", HttpUtility.UrlEncode("_NewValue"), HttpUtility.UrlEncode(_NewValue));
                    requestWriter.Write("&{0}={1}", HttpUtility.UrlEncode("_TransactionType"), HttpUtility.UrlEncode(_TransactionType));
                    requestWriter.Write("&{0}={1}", HttpUtility.UrlEncode("_Transaction_By"), HttpUtility.UrlEncode(_Transaction_By));
                    requestWriter.Write("&{0}={1}", HttpUtility.UrlEncode("_Transaction_Date_Time"), HttpUtility.UrlEncode(_Transaction_Date_Time));

                }
                // start the asynchronous operation

                request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
            }
            catch (Exception ex)
            {
                trycount++;
                if (trycount < 3)
                {

                    Thread.Sleep(1000);

                    goto callrequest;
                }



            }
            // Keep the main thread from continuing while the asynchronous 
            // operation completes. A real world application 
            //// could do something useful such as updating its user interface. 
            //allDone.WaitOne();
        }
        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            int streamcheck = 0;
        callrequeststream:
            try
            {
                request = (HttpWebRequest)asynchronousResult.AsyncState;



            }
            catch
            {
                streamcheck++;
                if (streamcheck < 3)
                {

                    Thread.Sleep(1000);

                    goto callrequeststream;
                }

            }

            //// End the operation
            //Stream postStream = request.EndGetRequestStream(asynchronousResult);

            //Console.WriteLine("Please enter the input data to be posted:");
            //string postData = Console.ReadLine();

            //// Convert the string into a byte array. 
            //byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //// Write to the request stream.
            //postStream.Write(byteArray, 0, postData.Length);
            //postStream.Close();

            //// Start the asynchronous operation to get the response

        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            int streamcheck = 0;
        callresponsestream:
            try
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

                // End the operation
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                string responseString = streamRead.ReadToEnd();
                Console.WriteLine(responseString);
                // Close the stream object
                streamResponse.Close();
                streamRead.Close();

                // Release the HttpWebResponse
                response.Close();
                allDone.Set();
            }
            catch
            {
                streamcheck++;
                if (streamcheck < 3)
                {

                    Thread.Sleep(1000);

                    goto callresponsestream;
                }

            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// The NHibernate Session object is exposed only to the Manager class.
        /// It is recommended that you...
        /// ...use the the NHibernateSession methods to control Transactions (unless you specifically want nested transactions).
        /// ...do not directly expose the Flush method (to prevent open transactions from locking your DB).
        /// </summary>
        public System.Type Type
        {
            get { return typeof(T); }
        }
        public INHibernateSession Session
        {
            get { return session; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(false);
        }
        private void Dispose(bool finalizing)
        {
            if (!_disposed)
            {
                if (session != null)
                    session.DecrementRefCount();

                if (!finalizing)
                    GC.SuppressFinalize(this);

                _disposed = true;
            }
        }

        #endregion


    }
}



////Old Code - Start
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using NHibernate;
//using NHibernate.Criterion;

//namespace Acurus.Capella.DataAccess.ManagerObjects
//{
//    public interface IManagerBase<T, TKey> : IDisposable
//    {
//        // Get Methods
//        T GetById(TKey Id);
//        IList<T> GetAll();
//        IList<T> GetAll(int maxResults);
//        IList<T> GetByCriteria(params ICriterion[] criterionList);
//        IList<T> GetByCriteria(int maxResults, params ICriterion[] criterionList);
//        T GetUniqueByCriteria(params ICriterion[] criterionList);
//        IList<T> GetByExample(T exampleObject, params string[] excludePropertyList);
//        IList<T> GetByQuery(string query);
//        IList<T> GetByQuery(int maxResults, string query);
//        T GetUniqueByQuery(string query);

//        // Misc Methods
//        void SetFetchMode(string associationPath, FetchMode mode);
//        ICriteria CreateCriteria();

//        // CRUD Methods
//        //object Save(T entity);
//        //void SaveOrUpdate(T entity);
//        //void Delete(T entity);
//        //void Update(T entity);

//        //IList<T> Save(IList<T> entity);
//        //void SaveOrUpdate(IList<T> entity);
//        //void Delete(IList<T> entity);
//        //void Update(IList<T> entity);

////        void Refresh(T entity);
////        void Evict(T entity);
////        IList<object> Save(IList<T> entity);
////        void SaveOrUpdate(IList<T> entity);
////        void Delete(IList<T> entity);
////        void Update(IList<T> entity);

////        void Delete(IList<T> entityList, ISession session);
////void Update(IList<T> entityList, ISession session);
////IList<object> Save(IList<T> entityList, ISession session);

////void SaveUpdateDelete(IList<T> saveList, IList<T> updateList, IList<T> deleteList);
//void SaveUpdateDeleteWithTransaction(ref IList<T> saveList, IList<T> updateList, IList<T> deleteList);
//Boolean SaveUpdateDeleteWithoutTransaction(ref IList<T> saveList, IList<T> updateList, IList<T> deleteList, ISession MySession);
////void SaveUpdateDelete(T saveObj, T updateObj, T delObj);

//        // Properties
//        System.Type Type { get; }
//        INHibernateSession Session { get; }
//    }

//    public abstract partial class ManagerBase<T, TKey> : IManagerBase<T, TKey>
//    {
//        #region Declarations


//        protected INHibernateSession session;
//        protected int defaultMaxResults = 25;// Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["NHibernate_defaultMaxResults"].ToString());

//        private bool _disposed = false;
//        private Dictionary<string, FetchMode> _fetchModeMap = new Dictionary<string, FetchMode>();

//        #endregion

//        #region Constructors

//        public ManagerBase()
//            : this(NHibernateSessionManager.Instance.Session) { }
//        public ManagerBase(INHibernateSession session)
//        {
//            this.session = session;
//            this.session.IncrementRefCount();
//        }

//        ~ManagerBase()
//        {
//            Dispose(true);
//        }

//        #endregion

//        #region Get Methods

//        public virtual T GetById(TKey id)
//        {
//            return (T)Session.GetISession().Get(typeof(T), id);
//        }

//        public IList<T> GetAll()
//        {
//            return GetByCriteria(defaultMaxResults);
//        }
//        public IList<T> GetAll(int maxResults)
//        {
//            return GetByCriteria(maxResults);
//        }

//        public int GetCount(string entityName)
//        {
//            int rowCount = 0;
//            ICriteria criteria = Session.GetISession().CreateCriteria(entityName);
//            criteria.SetProjection(Projections.RowCount());
//            IList result = criteria.List();
//            if (result.Count > 0)
//                rowCount = (int)result[0];
//            return rowCount;
//        }

//        public IList<T> GetLimit(int startIndex, int maxSize, string entityName)
//        {
//            ICriteria criteria = Session.GetISession().CreateCriteria(entityName);
//            criteria.SetFirstResult(startIndex).SetMaxResults(maxSize);
//            return criteria.List<T>();
//        }

//        public IList<T> GetByCriteria(params ICriterion[] criterionList)
//        {
//            return GetByCriteria(defaultMaxResults, criterionList);
//        }
//        public IList<T> GetByCriteria(int maxResults, params ICriterion[] criterionList)
//        {

//            ICriteria criteria = CreateCriteria().SetMaxResults(maxResults);

//            foreach (ICriterion criterion in criterionList)
//                criteria.Add(criterion);

//            IList<T> list = criteria.List<T>();


//            return list;
//        }
//        public IList<T> GetByCriteria(int maxResults, int pageNumber, ICriteria criteria)
//        {
//            if (criteria.List<T>().Count >= maxResults)
//            {
//                criteria.SetMaxResults(maxResults);
//                criteria.SetFirstResult((pageNumber - 1) * maxResults);
//            }

//            IList<T> list = criteria.List<T>();

//            return list;
//        }

//        public T GetUniqueByCriteria(params ICriterion[] criterionList)
//        {
//            ICriteria criteria = CreateCriteria();

//            foreach (ICriterion criterion in criterionList)
//                criteria.Add(criterion);

//            return criteria.UniqueResult<T>();
//        }

//        public IList<T> GetByExample(T exampleObject, params string[] excludePropertyList)
//        {
//            ICriteria criteria = CreateCriteria();
//            Example example = Example.Create(exampleObject);

//            foreach (string excludeProperty in excludePropertyList)
//                example.ExcludeProperty(excludeProperty);

//            criteria.Add(example);

//            return criteria.List<T>();
//        }

//        public IList<T> GetByQuery(string query)
//        {
//            return GetByQuery(defaultMaxResults, query);
//        }
//        public IList<T> GetByQuery(int maxResults, string query)
//        {
//            IQuery iQuery = Session.GetISession().CreateQuery(query).SetMaxResults(maxResults);
//            return iQuery.List<T>();
//        }

//        public IList<T> GetByQuery(int maxResults, int pageNumber, string query)
//        {
//            IQuery iQuery = Session.GetISession().CreateQuery(query).SetMaxResults(maxResults);
//            iQuery.SetFirstResult(maxResults * pageNumber);
//            return iQuery.List<T>();
//        }

//        public T GetUniqueByQuery(string query)
//        {
//            IQuery iQuery = Session.GetISession().CreateQuery(query);
//            return iQuery.UniqueResult<T>();

//        }

//        #endregion

//        #region Misc Methods

//        public void SetFetchMode(string associationPath, FetchMode mode)
//        {
//            if (!_fetchModeMap.ContainsKey(associationPath))
//                _fetchModeMap.Add(associationPath, mode);
//        }

//        public ICriteria CreateCriteria()
//        {
//            ICriteria criteria = Session.GetISession().CreateCriteria(typeof(T));

//            foreach (var pair in _fetchModeMap)
//                criteria = criteria.SetFetchMode(pair.Key, pair.Value);

//            return criteria;
//        }

//        #endregion

//        #region CRUD Methods

//        private object Save(T entity)
//        {
//            return Session.GetISession().Save(entity);
//        }
//        private void SaveOrUpdate(T entity)
//        {
//            Session.GetISession().SaveOrUpdate(entity);
//        }
//        private IList<object> Save(IList<T> entityList)
//        {
//            IList<object> savedObjects = new List<object>();
//            ISession session = Session.GetISession();
//            ITransaction trans = null;
//            try
//            {
//                trans = session.BeginTransaction();
//                foreach (T entity in entityList)
//                {
//                    object obj = session.Save(entity);
//                    savedObjects.Add(obj);
//                }
//                trans.Commit();
//            }
//            catch (Exception)
//            {
//                trans.Rollback();
//                throw;
//            }
//            finally
//            {
//                session.Close();
//            }
//            return savedObjects;
//        }

//        private void SaveOrUpdate(IList<T> entityList)
//        {
//            ISession session = Session.GetISession();
//            ITransaction trans = null;
//            try
//            {
//                trans = session.BeginTransaction();
//                foreach (T entity in entityList)
//                {
//                    session.SaveOrUpdate(entity);
//                }
//                trans.Commit();
//            }
//            catch (Exception)
//            {
//                trans.Rollback();
//                throw;
//            }
//            finally
//            {
//                session.Close();
//            }
//        }

//        private void Delete(IList<T> entityList)
//        {
//            ISession session = Session.GetISession();
//            ITransaction trans = null;
//            try
//            {
//                trans = session.BeginTransaction();
//                foreach (T entity in entityList)
//                {
//                    session.Delete(entity);
//                }
//                trans.Commit();
//            }
//            catch (Exception)
//            {
//                trans.Rollback();
//                throw;
//            }
//            finally
//            {
//                session.Close();
//            }
//        }

//        private void Update(IList<T> entityList)
//        {
//            ISession session = Session.GetISession();
//            ITransaction trans = null;
//            try
//            {
//                trans = session.BeginTransaction();
//                foreach (T entity in entityList)
//                {
//                    session.Update(entity);
//                }
//                trans.Commit();
//            }
//            catch (Exception)
//            {
//                trans.Rollback();
//                throw;
//            }
//            finally
//            {
//                session.Close();
//            }
//        }

//        private void Delete(T entity)
//        {
//            Session.GetISession().Delete(entity);
//        }
//        private void Update(T entity)
//        {
//            Session.GetISession().Update(entity);
//        }

//        private void Refresh(T entity)
//        {
//            Session.GetISession().Refresh(entity);
//        }
//        private void Evict(T entity)
//        {
//            Session.GetISession().Evict(entity);
//        }

//        private void Delete(IList<T> entityList, ISession session)
//        {
//            foreach (T entity in entityList)
//            {
//                session.Delete(entity);
//            }
//        }
//        private void Update(IList<T> entityList, ISession session)
//        {
//            foreach (T entity in entityList)
//            {
//                session.Update(entity);
//            }
//        }

//        private IList<object> Save(IList<T> entityList, ISession session)
//        {
//            IList<object> savedObjects = new List<object>();
//            foreach (T entity in entityList)
//            {
//                object obj = session.Save(entity);
//                savedObjects.Add(obj);
//            }
//            return savedObjects;
//        }

//        //public void SaveUpdateDelete(IList<T> saveList, IList<T> updateList, IList<T> deleteList)
//        //{
//        //    ISession session = Session.GetISession();
//        //    ITransaction trans = null;
//        //    try
//        //    {

//        //        trans = session.BeginTransaction();
//        //        if (deleteList != null)
//        //        {
//        //            Delete(deleteList, session);
//        //        }
//        //        if (saveList != null)
//        //        {
//        //            Save(saveList, session);
//        //        }
//        //        if (updateList!=null)
//        //        {
//        //            Update(updateList, session);
//        //        }
//        //        trans.Commit();
//        //    }
//        //    catch (Exception)
//        //    {
//        //        trans.Rollback();
//        //        throw;
//        //    }
//        //    finally
//        //    {
//        //        session.Close();
//        //    }
//        //}

//          int iTryCount = 0;


//        public void SaveUpdateDeleteWithTransaction( ref IList<T> saveList, IList<T> updateList, IList<T> deleteList)
//        {
//            iTryCount = 0;

//        TryAgain:
//            ISession session = Session.GetISession();

//            ITransaction trans = null;
//            try
//            {
//                trans = session.BeginTransaction();

//                if (deleteList != null)
//                {
//                    Delete(deleteList, session);
//                }

//                if (saveList != null)
//                {
//                    Save(saveList,session);
//                }

//                if (updateList != null)
//                {
//                    Update(updateList,session);
//                }

//                session.Flush();

//                trans.Commit();
//            }
//            catch (NHibernate.Exceptions.GenericADOException ex)
//            {
//              MySql.Data.MySqlClient.MySqlException excep = (MySql.Data.MySqlClient.MySqlException)ex.InnerException;

//                //Deadlock Error
//                if (excep.Number == 1213 && iTryCount<5)
//                {
//                    iTryCount++;
//                    goto TryAgain;
//                }
//                else
//                {
//                    trans.Rollback();
//                    throw;
//                }
//            }
//            catch (Exception ep)
//            {
//                trans.Rollback();
//                throw;
//            }
//            finally
//            {
//                session.Close();
//            }
//        }

//        public Boolean SaveUpdateDeleteWithoutTransaction(ref IList<T> saveList, IList<T> updateList, IList<T> deleteList,ISession MySession)
//        {
//            Boolean bResult = false;

//            try
//            {
//                if (deleteList != null)
//                {
//                    Delete(deleteList, MySession);
//                }

//                if (saveList != null)
//                {
//                    Save(saveList, MySession);
//                }

//                if (updateList != null)
//                {
//                    Update(updateList, MySession);
//                }

//                bResult = true;
//            }
//            catch (NHibernate.Exceptions.GenericADOException ex)
//            {
//                bResult = false;

//                MySql.Data.MySqlClient.MySqlException excep = (MySql.Data.MySqlClient.MySqlException)ex.InnerException;

//                //Deadlock Error
//                if (excep.Number == 1213)
//                {
//                    return bResult;
//                }

//                throw;
//            }
//            catch (Exception ep)
//            {
//                bResult = false;
//                throw;
//            }
//            finally
//            {
//                //MySession.Close();
//            }
//            return bResult;
//        }

//        #endregion


//        #region Properties

//        /// <summary>
//        /// The NHibernate Session object is exposed only to the Manager class.
//        /// It is recommended that you...
//        /// ...use the the NHibernateSession methods to control Transactions (unless you specifically want nested transactions).
//        /// ...do not directly expose the Flush method (to prevent open transactions from locking your DB).
//        /// </summary>
//        public System.Type Type
//        {
//            get { return typeof(T); }
//        }
//        public INHibernateSession Session
//        {
//            get { return session; }
//        }

//        #endregion

//        #region IDisposable Members

//        public void Dispose()
//        {
//            Dispose(false);
//        }
//        private void Dispose(bool finalizing)
//        {
//            if (!_disposed)
//            {
//                session.DecrementRefCount();

//                if (!finalizing)
//                    GC.SuppressFinalize(this);

//                _disposed = true;
//            }
//        }

//        #endregion







//    }
//}
////Old Code - End
