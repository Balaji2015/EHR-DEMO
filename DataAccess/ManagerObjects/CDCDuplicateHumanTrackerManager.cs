using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.Criterion;

namespace Acurus.Capella.DataAccess.ManagerObjects
{

    public partial interface ICDCDuplicateHumanTrackerManager : IManagerBase<CDCDuplicateHumanTracker, ulong>
    {
        void SaveCDCDuplicateHumanTrackerWithTransaction(IList<CDCDuplicateHumanTracker> ListCDCDuplicateHumanTracker, string MACAddress);
    }

    public partial class CDCDuplicateHumanTrackerManager : ManagerBase<CDCDuplicateHumanTracker, ulong>, ICDCDuplicateHumanTrackerManager
    {
        #region Constructors
        public CDCDuplicateHumanTrackerManager()
            : base()
        {

        }
        public CDCDuplicateHumanTrackerManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods
        public void SaveCDCDuplicateHumanTrackerWithTransaction(IList<CDCDuplicateHumanTracker> ListCDCDuplicateHumanTracker, string MACAddress)
        {
            SaveUpdateDeleteWithTransaction(ref ListCDCDuplicateHumanTracker, null, null, MACAddress);
        }
        #endregion
    }
}
