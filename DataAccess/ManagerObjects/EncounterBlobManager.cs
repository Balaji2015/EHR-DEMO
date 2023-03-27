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
    public partial interface IEncounterBlobManager : IManagerBase<Encounter_Blob, ulong>
    {
        int SaveEncounterBlobWithoutTransaction(IList<Encounter_Blob> ListToInsertEncounterBlob, IList<Encounter_Blob> ListToUpdateEncounterBlob, ISession MySession, string MACAddress);

        IList<Encounter_Blob> GetEncounterBlob(ulong ulEncounterID);

        void SaveEncounterBlobWithTransaction( IList<Encounter_Blob> ListToUpdateEncounterBlob, string MACAddress);
    }

    public partial class EncounterBlobManager : ManagerBase<Encounter_Blob, ulong>, IEncounterBlobManager
    {
        #region Constructors

        public EncounterBlobManager()
            : base()
        {

        }
        public EncounterBlobManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods

        public int SaveEncounterBlobWithoutTransaction(IList<Encounter_Blob> ListToInsertEncounterBlob, IList<Encounter_Blob> ListToUpdateEncounterBlob, ISession MySession, string MACAddress)
        {
            return SaveUpdateDeleteWithoutTransaction(ref ListToInsertEncounterBlob, ListToUpdateEncounterBlob, null, MySession, MACAddress);
        }

        public IList<Encounter_Blob> GetEncounterBlob(ulong ulEncounterID)
        {
            IList<Encounter_Blob> ilstEncounterBlob = new List<Encounter_Blob>();
            //GitLab #3960  
            session.GetISession().Close();
            ICriteria crit = session.GetISession().CreateCriteria(typeof(Encounter_Blob)).Add(Expression.Eq("Id", ulEncounterID));
            ilstEncounterBlob = crit.List<Encounter_Blob>();

            return ilstEncounterBlob;
        }

        public void SaveEncounterBlobWithTransaction( IList<Encounter_Blob> ListToUpdateEncounterBlob, string MACAddress)
        {
            IList<Encounter_Blob> ilstEncounterBlob = null;
            SaveUpdateDeleteWithTransaction(ref ilstEncounterBlob,ListToUpdateEncounterBlob, null, MACAddress);
        }
        #endregion
    }
}
