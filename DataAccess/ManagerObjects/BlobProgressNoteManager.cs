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
        IList<Blob_Progress_Note> GetBlobProgressNotesByStatus(IList<string> lstStatus, string sDuration = "");
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
        public IList<Blob_Progress_Note> GetBlobProgressNotesByStatus(IList<string> lstStatus,string sDuration = "")
        {
            IList<Blob_Progress_Note> ilstBlobProgressNotes = new List<Blob_Progress_Note>();
            session.GetISession().Close();
            if (sDuration != "")
            {
                ISQLQuery sq = session.GetISession().CreateSQLQuery("SELECT B.* FROM cdc_progress_note B where B.Status not in (" + string.Join(",", lstStatus) + ") and Modified_Date_And_Time < NOW() - INTERVAL " + sDuration + " HOUR;")
                           .AddEntity("B", typeof(Blob_Progress_Note));
                ilstBlobProgressNotes = sq.List<Blob_Progress_Note>();
            }
            else
            {
                ISQLQuery sq = session.GetISession().CreateSQLQuery("SELECT B.* FROM cdc_progress_note B where B.Status not in (" + string.Join(",", lstStatus) + ");")
                           .AddEntity("B", typeof(Blob_Progress_Note));
                ilstBlobProgressNotes = sq.List<Blob_Progress_Note>();
            }
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
