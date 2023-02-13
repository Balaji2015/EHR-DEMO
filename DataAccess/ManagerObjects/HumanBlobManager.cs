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

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IHumanBlobManager : IManagerBase<Human_Blob, ulong>
    {
        int SaveHumanBlobWithoutTransaction(IList<Human_Blob> ListToInsertHumanBlob, IList<Human_Blob> ListToUpdateHumanBlob, ISession MySession, string MACAddress);

        IList<Human_Blob> GetHumanBlob(ulong ulHumanID);

        void SaveHumanBlobWithTransaction(IList<Human_Blob> ListToUpdateHumanBlob, string MACAddress);
    }

    public partial class HumanBlobManager : ManagerBase<Human_Blob, ulong>, IHumanBlobManager
    {
        #region Constructors

        public HumanBlobManager()
            : base()
        {

        }
        public HumanBlobManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods

        public int SaveHumanBlobWithoutTransaction(IList<Human_Blob> ListToInsertHumanBlob, IList<Human_Blob> ListToUpdateHumanBlob, ISession MySession, string MACAddress)
        {
            return SaveUpdateDeleteWithoutTransaction(ref ListToInsertHumanBlob, ListToUpdateHumanBlob, null, MySession, MACAddress);
        }

        public IList<Human_Blob> GetHumanBlob(ulong ulHumanID)
        {
            IList<Human_Blob> ilstHumanBlob = new List<Human_Blob>();
            session.GetISession().Close();
            ICriteria crit = session.GetISession().CreateCriteria(typeof(Human_Blob)).Add(Expression.Eq("Id", ulHumanID));
            ilstHumanBlob = crit.List<Human_Blob>();

            return ilstHumanBlob;
        }

        public void SaveHumanBlobWithTransaction(IList<Human_Blob> ListToUpdateHumanBlob, string MACAddress)
        {
            IList<Human_Blob> ilstHumanBlob = null;
            SaveUpdateDeleteWithTransaction(ref ilstHumanBlob, ListToUpdateHumanBlob, null, MACAddress);
        }
        #endregion
    }
}
