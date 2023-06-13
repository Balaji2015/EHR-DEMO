using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.Data;
using System.Linq;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.Core;



namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IAuditLogManager : IManagerBase<AuditLog, long>
    {
        void AppendToAuditLog(IList<AuditLog> AuditLogList, string MACAddress);
        void AppendNewEntryToAuditLog(IList<AuditLog> AuditLogList, string MACAddress);
        DataSet DtblAuditLogReport(DateTime From_Date, DateTime To_Date);
        //Added By Saravanakumar On 10-06-2013 for AuditLog delete
        void DeleteAuditLog();
    }

    public partial class AuditLogManager : ManagerBase<AuditLog, long>, IAuditLogManager
    {
        #region Constructors

        public AuditLogManager()
            : base()
        {
        }
        public AuditLogManager(INHibernateSession session)
            : base(session)
        {
        }

        public void AppendToAuditLog(IList<AuditLog> AuditLogList, string MACAddress)
        {
            IList<AuditLog> updateList = null;
            IList<AuditLog> deleteList = null;
            //SaveUpdateDeleteWithoutTransaction(ref AuditLogList, null, null, NHibernateSessionUtility.Instance.MySession, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref AuditLogList, ref updateList, deleteList, MACAddress, false, false,Convert.ToUInt32(AuditLogList[0].Human_ID), string.Empty);
        }

        public void AppendNewEntryToAuditLog(IList<AuditLog> AuditLogList, string MACAddress)
        {
            if (NHibernateSessionUtility.Instance.AuditTrailTableList.Contains(AuditLogList[0].Entity_Name.ToString()) == true)
            {
                //SaveUpdateDeleteWithTransaction(ref AuditLogList, null, null, MACAddress);
                IList<AuditLog> updateList = null;
                IList<AuditLog> deleteList = null;
                SaveUpdateDelete_DBAndXML_WithTransaction(ref AuditLogList, ref updateList, deleteList, MACAddress, false, false, Convert.ToUInt32(AuditLogList[0].Human_ID), string.Empty);
            }
        }
        public DataSet DtblAuditLogReport(DateTime From_Date, DateTime To_Date)
        {

            ArrayList arylstColumnsOfAuditLogReport;
            ArrayList arylstAuditLog;
            DataTable DtblAuditLogReport = new DataTable();
            DataSet AuditLogReport = new DataSet();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

                IQuery query2 = iMySession.GetNamedQuery("Fill.GetAuditLogReport.ColumnHeadings");
                arylstColumnsOfAuditLogReport = new ArrayList(query2.List());

                if (arylstColumnsOfAuditLogReport.Count > 0)
                {
                    object[] objlstColumns = (object[])arylstColumnsOfAuditLogReport[0];

                    for (int i = 0; i < objlstColumns.Length; i++)
                    {
                        if (i == 0)
                        {
                            DtblAuditLogReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.DateTime));
                        }
                        else
                        {
                            DtblAuditLogReport.Columns.Add(objlstColumns[i].ToString(), typeof(System.String));
                        }

                    }
                }

                IQuery query1 = iMySession.GetNamedQuery("Reports.AuditLogReport");
                query1.SetString(0, From_Date.ToString("yyyy-MM-dd HH:mm:ss"));
                query1.SetString(1, To_Date.ToString("yyyy-MM-dd HH:mm:ss"));
                arylstAuditLog = new ArrayList(query1.List());
                if (arylstAuditLog != null)
                {
                    for (int h = 0; h < arylstAuditLog.Count; h++)
                    {

                        object[] objlst = (object[])arylstAuditLog[h];
                        DataRow dr = DtblAuditLogReport.NewRow();
                        {

                            for (int k = 0; k < objlst.Length; k++)
                            {
                                try
                                {
                                    if (k == 0)
                                    {
                                        dr[k] = Convert.ToDateTime(objlst[k]);
                                    }
                                    else
                                    {
                                        dr[k] = objlst[k].ToString();
                                    }
                                }
                                catch
                                {
                                    //string s;
                                }
                            }
                            DtblAuditLogReport.Rows.Add(dr);
                        }
                    }



                }

                AuditLogReport.Tables.Add(DtblAuditLogReport);
                iMySession.Close();
            }
            return AuditLogReport;


        }

        //Added By Saravanakumar On 10-06-2013 for AuditLog delete
        public void DeleteAuditLog()
        {
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
                IQuery query = iMySession.GetNamedQuery("Delete.AuditLog");
                ArrayList arr;
                arr = new ArrayList(query.List());
                iMySession.Close();
            }
        }

        public void AppendSelectEntryToAuditLog(IList<AuditLog> AuditLogList, string MACAddress)
        {
            //Jira CAP-333 -oldcode
            //string[] table_names = AuditLogList.Select(item => item.Entity_Name.Split('.').Last()).ToArray<string>();
            //if (table_names != null && table_names.Count() > 0)
            //{
            //    MasterTableListManager mastertablemanager = new MasterTableListManager();
            //    IList<MasterTableList> masterlist = mastertablemanager.GetAuditTrailTableListByTableNames(table_names);
            //    IList<AuditLog> updateList = null;
            //    IList<AuditLog> deleteList = null;
            //    if (masterlist.Count > 0)
            //    {
            //        for (int i = 0; i < masterlist.Count;i++ )
            //        {
            //          //  SaveUpdateDeleteWithTransaction(ref AuditLogList, null, null, MACAddress);
            //            SaveUpdateDelete_DBAndXML_WithTransaction(ref AuditLogList, ref updateList, deleteList, MACAddress, false, false,Convert.ToUInt32(AuditLogList[0].Human_ID), string.Empty);
            //            return;
            //        }
            //    }
            //}

            ///Jira CAP-333 - newcode
            foreach (AuditLog auditlog in AuditLogList)
            {
                NHibernateSessionUtility.Instance.MyAuditLogList.Add(auditlog);
            }
            WebservicetriggerUsingDirectTransaction();

        }
        //BugID:49685
        public void InsertIntoAuditLog(string EntityName, string TransactionType, int HumanID, string UserName)
        {
            //IList<AuditLog> AuditLList = new List<AuditLog>();
            AuditLog aLog = new AuditLog();
            aLog.Human_ID = HumanID;
            aLog.Entity_Name = EntityName;
            aLog.Transaction_Type = TransactionType;
            aLog.Transaction_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(System.DateTime.Now);
            aLog.Transaction_By = UserName;
            aLog.New_Value = String.Empty;
            aLog.Old_Value = String.Empty;
            aLog.Entity_Id = 0;
            aLog.Attribute = String.Empty;

            //Jira CAP-333 - oldCode
            //AuditLList.Add(aLog);
            //AppendToAuditLog(AuditLList, string.Empty);
            //Jira CAP-333 - newCode
            NHibernateSessionUtility.Instance.MyAuditLogList.Add(aLog);
            WebservicetriggerUsingDirectTransaction();
        }
        #endregion


    }
}
