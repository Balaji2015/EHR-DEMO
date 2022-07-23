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

    public partial interface IRcopia_SettingsManager : IManagerBase<Rcopia_Settings, ulong>
    {
        IList<Rcopia_Settings> GetRcopia_Settings(string sLegalOrg);
    }
    public partial class Rcopia_SettingsManager : ManagerBase<Rcopia_Settings, ulong>, IRcopia_SettingsManager
    {

        #region Constructors

        public Rcopia_SettingsManager()
            : base()
        {

        }
        public Rcopia_SettingsManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion


        #region Methods

        public IList<Rcopia_Settings> GetRcopia_Settings(string sLegalOrg)
        {
            ICriteria crit;
            crit = session.GetISession().CreateCriteria(typeof(Rcopia_Settings)).Add(Expression.Eq("Legal_Org", sLegalOrg));
            IList<Rcopia_Settings> ilstRcopSett = crit.List<Rcopia_Settings>();
            return ilstRcopSett;
        }

        #endregion

    }
}
