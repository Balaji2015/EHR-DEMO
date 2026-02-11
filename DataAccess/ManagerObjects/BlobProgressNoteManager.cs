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
        IList<Blob_Progress_Note> GetBlobProgressNotesByStatus(IList<string> lstNotStatus, IList<string> lstErrorMessage, string sDuration = "");
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
        public IList<Blob_Progress_Note> GetBlobProgressNotesByStatus(IList<string> lstNotStatus, IList<string> lstErrorMessage, string sDuration = "")
        {
            session.GetISession().Close();
            IList<Blob_Progress_Note> ilstBlobProgressNotes = new List<Blob_Progress_Note>();
            string sErrorDescription = string.Empty;
            string sDurationQuery = string.Empty;

            if (lstErrorMessage.Count > 0)
            {
                lstErrorMessage = lstErrorMessage.Select(x => "Error_Description like '%" + x + "%'").ToList();
                sErrorDescription = " or (B.status = 'error' and (" + string.Join(" or ", lstErrorMessage) + "))";
            }
            if (sDuration != "") { sDurationQuery = " and Modified_Date_And_Time < NOW() - INTERVAL " + sDuration + " HOUR"; }

            ISQLQuery sq = session.GetISession().CreateSQLQuery("SELECT B.* FROM cdc_progress_note B where (B.Status not in (" + string.Join(",", lstNotStatus) + ")" + sErrorDescription + ")" + sDurationQuery + ";")
                       .AddEntity("B", typeof(Blob_Progress_Note));
            ilstBlobProgressNotes = sq.List<Blob_Progress_Note>();
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
