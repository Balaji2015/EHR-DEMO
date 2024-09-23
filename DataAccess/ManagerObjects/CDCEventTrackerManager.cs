using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.IO;
using System.Runtime.Serialization;
using System.Web.Mail;
using System.Data;
using System.Linq;
using System.Xml;

namespace Acurus.Capella.DataAccess.ManagerObjects
{

    public partial interface ICDCEventTrackerManager : IManagerBase<CDCEventTracker, ulong>
    {
        IList<CDCEventTracker> GetCDCEventTracker(ulong ulHumanID);

        void SaveCDCEventTrackerWithTransaction(IList<CDCEventTracker> ListToUpdateCDCEventTracker, string MACAddress);

    }

    public partial class CDCEventTrackerManager : ManagerBase<CDCEventTracker, ulong>, ICDCEventTrackerManager
    {
        #region Constructors

        public CDCEventTrackerManager()
            : base()
        {

        }
        public CDCEventTrackerManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods


        public IList<CDCEventTracker> GetCDCEventTracker(ulong ulHumanID)
        {
            IList<CDCEventTracker> ilstCDCEvent = new List<CDCEventTracker>();
            session.GetISession().Close();
            ICriteria crit = session.GetISession().CreateCriteria(typeof(CDCEventTracker)).Add(Expression.Eq("Human_ID", ulHumanID));
            ilstCDCEvent = crit.List<CDCEventTracker>();

            return ilstCDCEvent;
        }

        public void SaveCDCEventTrackerWithTransaction(IList<CDCEventTracker> ListToUpdateCDCEventTracker, string MACAddress)
        {
            IList<CDCEventTracker> ilstCDCEvent = null;
            SaveUpdateDeleteWithTransaction(ref ilstCDCEvent, ListToUpdateCDCEventTracker, null, MACAddress);
        }
        #endregion
    }
}
