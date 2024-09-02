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

    public partial interface IBlobProgressNoteManager : IManagerBase<Blob_Progress_Note, ulong>
    {
        IList<Blob_Progress_Note> GetBlobProgressNotes(ulong ulEncounterID);

        void SaveBlobProgressNotesWithTransaction(IList<Blob_Progress_Note> ListToUpdateBlobProgressNotes, string MACAddress);

    }

    public partial class BlobProgressNoteManager : ManagerBase<Blob_Progress_Note, ulong>, IBlobProgressNoteManager
    {
        #region Constructors

        public BlobProgressNoteManager()
            : base()
        {

        }
        public BlobProgressNoteManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods


        public IList<Blob_Progress_Note> GetBlobProgressNotes(ulong ulEncounterID)
        {
            IList<Blob_Progress_Note> ilstBlobProgressNotes = new List<Blob_Progress_Note>();
            session.GetISession().Close();
            ICriteria crit = session.GetISession().CreateCriteria(typeof(Blob_Progress_Note)).Add(Expression.Eq("Id", ulEncounterID));
            ilstBlobProgressNotes = crit.List<Blob_Progress_Note>();

            return ilstBlobProgressNotes;
        }

        public void SaveBlobProgressNotesWithTransaction(IList<Blob_Progress_Note> ListToUpdateBlobProgressNotes, string MACAddress)
        {
            IList<Blob_Progress_Note> ilstBlobProgressNotes = null;
            SaveUpdateDeleteWithTransaction(ref ilstBlobProgressNotes, ListToUpdateBlobProgressNotes, null, MACAddress);
        }
        #endregion
    }
}
