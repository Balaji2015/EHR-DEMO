using System;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.Criterion;
using System.Collections;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IIndexingExceptionLogManager : IManagerBase<IndexingExceptionLog,int>
    {
        void DeleteErrorLogByResultMasterID(int ResultMasterId, string MacAddress);
        void UpdateIndexingExceptionLog(IList<IndexingExceptionLog> UpdateList, ISession MySession);
        IList<IndexingExceptionLog> GetIndexingExceptionLogById(ulong ulIndexingExceptionLogId);
        IList<IndexingExceptionLog> GetAllActiveIndexingExceptionLog();
    }

    public partial class IndexingExceptionLogManager : ManagerBase<IndexingExceptionLog, int>, IIndexingExceptionLogManager
    {
        #region Constructors
        public IndexingExceptionLogManager()
            : base()
        {

        }
        public IndexingExceptionLogManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion
        
        public void DeleteErrorLogByResultMasterID(int ResultMasterId, string MacAddress)
        {

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("GetErrorLog.By.ResultMasterID");
                query.SetString(0, ResultMasterId.ToString());
                //ICriteria critOrders = session.GetISession().CreateCriteria(typeof(IndexingExceptionLog)).Add(Expression.Eq("Result_Master_ID", ResultMasterId));
                ArrayList ids = new ArrayList(query.List());
                if (ids != null && ids.Count > 0)
                {
                    IList<IndexingExceptionLog> ReturnList = new List<IndexingExceptionLog>();
                    for (int i = 0; i < ids.Count; i++)
                    {
                        ReturnList.Add(GetById(Convert.ToInt32(ids[i])));
                    }
                    IList<IndexingExceptionLog> DeleteList = new List<IndexingExceptionLog>();
                    IList<IndexingExceptionLog> SaveList = new List<IndexingExceptionLog>();
                    if (ReturnList != null && ReturnList.Count > 0)
                    {
                        DeleteList.Add(ReturnList[0]);

                    }
                    IList<IndexingExceptionLog> ErrorLogTemp = null;
                    SaveUpdateDelete_DBAndXML_WithTransaction(ref SaveList, ref ErrorLogTemp, DeleteList, MacAddress, false, false, 0, "");
                }
                iMySession.Close();
            }
        }

        public void UpdateIndexingExceptionLog(IList<IndexingExceptionLog> UpdateList, ISession MySession)
        {
            IList<IndexingExceptionLog> SaveList = new List<IndexingExceptionLog>();
            SaveUpdateDeleteWithoutTransaction(ref SaveList, UpdateList, null, MySession, string.Empty);
        }

        public IList<IndexingExceptionLog> GetIndexingExceptionLogById(ulong ulIndexingExceptionLogId)
        {
            IList<IndexingExceptionLog> ilstIndexingExceptionLog = new List<IndexingExceptionLog>();
            session.GetISession().Close();
            ICriteria crit = session.GetISession().CreateCriteria(typeof(IndexingExceptionLog)).Add(Expression.Eq("Id", ulIndexingExceptionLogId));
            ilstIndexingExceptionLog = crit.List<IndexingExceptionLog>();

            return ilstIndexingExceptionLog;
        }
        public IList<IndexingExceptionLog> GetAllActiveIndexingExceptionLog()
        {
            IList<IndexingExceptionLog> ilstIndexingExceptionLog = new List<IndexingExceptionLog>();
            session.GetISession().Close();
            ICriteria crit = session.GetISession().CreateCriteria(typeof(IndexingExceptionLog)).Add(Expression.Eq("Is_Active","Y" ));
            ilstIndexingExceptionLog = crit.List<IndexingExceptionLog>();

            return ilstIndexingExceptionLog;
        }
    }
}