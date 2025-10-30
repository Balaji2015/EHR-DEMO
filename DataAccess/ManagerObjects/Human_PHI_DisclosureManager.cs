using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using System.IO;
using System.Runtime.Serialization;
using Acurus.Capella.Core.DomainObjects;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IHuman_PHI_DisclosureManager : IManagerBase<Human_Phi_Disclosure, ulong>
    {
        IList<Human_Phi_Disclosure> GetHumanPhiDisclosureByHumanID(ulong ulHumanID);

        void SaveHumanPhiDisclosureWithTransaction(IList<Human_Phi_Disclosure> ilstHuman_Phi_Disclosure, IList<Human_Phi_Disclosure> ListToUpdateHuman_Phi_Disclosure, string MACAddress);
    }

    public partial class Human_PHI_DisclosureManager : ManagerBase<Human_Phi_Disclosure, ulong>, IHuman_PHI_DisclosureManager
    {
        #region Constructors

        public Human_PHI_DisclosureManager()
            : base()
        {

        }
        public Human_PHI_DisclosureManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods


        

        public IList<Human_Phi_Disclosure> GetHumanPhiDisclosureByHumanID(ulong ulHumanID)
        {
            IList<Human_Phi_Disclosure> ilstHuman_Phi_Disclosure = new List<Human_Phi_Disclosure>();
            session.GetISession().Close();
            ICriteria crit = session.GetISession().CreateCriteria(typeof(Human_Phi_Disclosure)).Add(Expression.Eq("Human_ID", ulHumanID));
            ilstHuman_Phi_Disclosure = crit.List<Human_Phi_Disclosure>();

            return ilstHuman_Phi_Disclosure;

        }
        public void SaveHumanPhiDisclosureWithTransaction(IList<Human_Phi_Disclosure> ilstHuman_Phi_Disclosure, IList<Human_Phi_Disclosure> ListToUpdateHuman_Phi_Disclosure, string MACAddress)
        {
            //Jira CAP-3817
            //SaveUpdateDeleteWithTransaction(ref ilstHuman_Phi_Disclosure, ListToUpdateHuman_Phi_Disclosure, null, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstHuman_Phi_Disclosure, ref ListToUpdateHuman_Phi_Disclosure, null, MACAddress, false, true, 0, string.Empty);
        }
        #endregion
    }
}
