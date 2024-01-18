using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.Criterion;

namespace Acurus.Capella.DataAccess.ManagerObjects
{

    public partial interface IGeneralNotesManager : IManagerBase<GeneralNotes, ulong>
    {

        void SaveAndUpdateGeneralNotes(IList<GeneralNotes> saveList, IList<GeneralNotes> updateList, string macAddress);

        //int SaveUpdateDeleteGeneralNotes(IList<GeneralNotes> ListToInsertGeneralNotes, IList<GeneralNotes> ListToUpdateGEneralNotes, ISession mySession, string macAddress);

        IList<GeneralNotes> GetGeneralNotes(string parent_field, ulong humanID);

        int SaveUpdateDeleteGeneralNotes1(GeneralNotes generalNotesObject, ISession mySession, string macAddress);

        IList<GeneralNotes> GetGeneralNotesList(ulong encounterId, string[] parentField);
    }
    public partial class GeneralNotesManager : ManagerBase<GeneralNotes, ulong>, IGeneralNotesManager
    {
        #region Constructors
        public GeneralNotesManager()
            : base()
        {

        }
        public GeneralNotesManager
            (INHibernateSession session)
            : base(session)
        {

        }

        #endregion

        /* For saving and updating GeneralNotes without Transaction in DB and XML, the GeneralNotesManagerObject.SaveUpdateDelete_DBAndXML_WithoutTransaction() method 
         can be called directly in the respective manager classes. This method is commented to remove redundancy of code.*/ 
        
        //public int SaveUpdateDeleteGeneralNotes(IList<GeneralNotes> ListToInsertGeneralNotes, IList<GeneralNotes> ListToUpdateGEneralNotes, ISession mySession,string macAddress)
        //{
        //    int iResult = 0;
        //    if (ListToInsertGeneralNotes.Count == 0)
        //    {
        //        ListToInsertGeneralNotes = null;
        //    }
        //    if (ListToUpdateGEneralNotes.Count == 0)
        //    {
        //        ListToUpdateGEneralNotes = null;
        //    }

        //    iResult = SaveUpdateDeleteWithoutTransaction(ref ListToInsertGeneralNotes, ListToUpdateGEneralNotes, null, mySession, macAddress);

        //    return iResult;
        //}

        public void SaveAndUpdateGeneralNotes(IList<GeneralNotes> saveList, IList<GeneralNotes> updateList, string macAddress)
        {
            if (saveList.Count == 0)
            {
                saveList = null;
            }
            else if (updateList.Count == 0)
            {
                updateList = null;
            }
            //Commented the below Manager_Call since it is not used anywhere. 
           //SaveUpdateDeleteWithTransaction(ref saveList, updateList, null, macAddress);
        }

        public IList<GeneralNotes> GetGeneralNotes(string parent_field, ulong humanId)
        {
            IList<GeneralNotes> lstnotes = new List<GeneralNotes>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Like("Parent_Field", parent_field)).Add(Expression.Like("Human_ID", humanId));
                lstnotes = criteria.List<GeneralNotes>();
                iMySession.Close();
            }
            return lstnotes;
        }

        public int SaveUpdateDeleteGeneralNotes1(GeneralNotes generalNotesObject, ISession mySession,string macAddress)
        {
            int iResult = 0;

            IList<GeneralNotes> generalNotesList = new List<GeneralNotes>();
            //IList<GeneralNotes> insetList = null;
            generalNotesList.Add(generalNotesObject);
            //Commented the below Manager_Call since it is not used anywhere. 
            //if (generalNotesObject.Id != 0)
            //{
            //    iResult = SaveUpdateDeleteWithoutTransaction(ref insetList, generalNotesList, null, mySession, macAddress);
            //}
            //else
            //{
            //    iResult = SaveUpdateDeleteWithoutTransaction(ref generalNotesList, null, null, mySession, macAddress);
            //}
            return iResult;
        }

        public IList<GeneralNotes> GetGeneralNotes(ulong encounterId, string parentField)
        {
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //Jira CAP-1664
                //ICriteria criteria = iMySession.CreateCriteria(typeof(GeneralNotes))
                //                    .Add(Expression.Like("Parent_Field", parentField))
                //                    .Add(Expression.Like("Encounter_ID", encounterId));
                ICriteria criteria = iMySession.CreateCriteria(typeof(GeneralNotes))
                                    .Add(Expression.Eq("Parent_Field", parentField))
                                    .Add(Expression.Eq("Encounter_ID", encounterId));
                IList<GeneralNotes> ilstGeneralNotes = criteria.List<GeneralNotes>();
                iMySession.Close();
                return ilstGeneralNotes;
            }

        }
        public IList<GeneralNotes> GetGeneralNotesList(ulong encounterId, string[] parentField)
        {
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(GeneralNotes))
                                    .Add(Expression.In("Parent_Field", parentField))
                                    .Add(Expression.Eq("Encounter_ID", encounterId));
                IList<GeneralNotes> ilstGeneralNotes = criteria.List<GeneralNotes>();
                iMySession.Close();
                return ilstGeneralNotes;
            }

        }
    }
}
