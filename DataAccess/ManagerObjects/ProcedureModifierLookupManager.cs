using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.Criterion;
using System;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IProcedureModifierLookupManager : IManagerBase<ProcedureModifierLookupManager, ulong>
    {
        IList<ProcedureModifierLookup> GetProcedureList(List<string> CPT);
    }
    public partial class ProcedureModifierLookupManager : ManagerBase<ProcedureModifierLookupManager, ulong>, IProcedureModifierLookupManager
    {

        #region Constructors

        public ProcedureModifierLookupManager()
            : base()
        {

        }
        public ProcedureModifierLookupManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion


        #region Methods
        public IList<ProcedureModifierLookup> GetProcedureList(List<string> CPT)
        {
            IList<ProcedureModifierLookup> listPrb = new List<ProcedureModifierLookup>();
            //.Add(Expression.Eq("Procedure_Type", ProcedureType));
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ICriteria crit = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.In("Procedure_Code", CPT)).AddOrder(Order.Desc("Procedure_Charge"));
                ICriteria crit = iMySession.CreateCriteria(typeof(ProcedureModifierLookup)).Add(Expression.In("Procedure_Code", CPT)).AddOrder(Order.Asc("Sort_Order")).AddOrder(Order.Desc("RVU")).AddOrder(Order.Asc("Procedure_Code"));
                listPrb = crit.List<ProcedureModifierLookup>();
                iMySession.Close();
            }
            return listPrb;

        }
        

        #endregion
    }
}
