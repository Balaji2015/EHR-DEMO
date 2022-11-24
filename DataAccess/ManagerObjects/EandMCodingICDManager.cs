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
    public partial interface IEAndMCodingICDManager : IManagerBase<EandMCodingICD, ulong>
    {
        int AppendToEMCodingICD(IList<EandMCodingICD> eandmICD, string MACAddress, ISession MySession, IList<EAndMCoding> eandmCode); //kumar.. changed from void to integer
      
        //Added by bala on 6-Nov-2012 for Checking G codes
        IList<EandMCodingICD> EandMcodingList(ulong encounterID);
        FillEandMCoding GetEandMcodingAndICDListbyEncounterID(ulong EncID);
        int GetEMICDCount(ulong ulEncID);
    }

    public partial class EandMCodingICDManager : ManagerBase<EandMCodingICD, ulong>, IEAndMCodingICDManager
    {
        #region Constructors

        public EandMCodingICDManager()
            : base()
        {

        }
        public EandMCodingICDManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region PublicMethods

        //public void AppendToEMCodingICD(IList<EandMCodingICD> eandmICD, string MACAddress, ISession MySession, ulong eandmID)
        public int AppendToEMCodingICD(IList<EandMCodingICD> eandmICD, string MACAddress, ISession MySession, IList<EAndMCoding> eandmCode)
        {
            IList<EandMCodingICD> eandmICDList = new List<EandMCodingICD>();
            EandMCodingICD eandmICDRecord = null;
            for (int i = 0; i < eandmCode.Count; i++)
            {
                for (int j = 0; j < eandmICD.Count; j++)
                {
                        eandmICDRecord = new EandMCodingICD();
                        eandmICDRecord.Created_By = eandmICD[j].Created_By;
                        eandmICDRecord.Created_Date_And_Time = eandmICD[j].Created_Date_And_Time;
                        ulong uleandmID = eandmCode[i].Id;
                        eandmICDRecord.Encounter_ID = eandmICD[j].Encounter_ID;
                        eandmICDRecord.ICD = eandmICD[j].ICD;
                        eandmICDRecord.ICD_Category = eandmICD[j].ICD_Category;
                        eandmICDRecord.ICD_Description = eandmICD[j].ICD_Description;
                        eandmICDRecord.Id = 0;
                        eandmICDRecord.Modified_By = eandmICD[j].Modified_By;
                        eandmICDRecord.Modified_Date_And_Time = eandmICD[j].Modified_Date_And_Time;
                        eandmICDRecord.Source = eandmICD[j].Source;
                        eandmICDRecord.Source_ID = eandmICD[j].Source_ID;
                        eandmICDRecord.Is_Delete = eandmICD[j].Is_Delete;
                        eandmICDRecord.Version = 0;
                        eandmICDList.Add(eandmICDRecord);
                }
            }

            return SaveUpdateDeleteWithoutTransaction(ref eandmICDList, null, null, MySession, MACAddress);
        }

        //public void UpdateEMCodingICD(IList<EandMCodingICD> OldeandmICD, IList<EandMCodingICD> NeweandmICD, string MACAddress, ISession MySession, ulong eandmID) //kumar
        public int UpdateEMCodingICD(IList<EandMCodingICD> OldeandmICD, IList<EandMCodingICD> NeweandmICD, string MACAddress, ISession MySession, ulong eandmID)
        {
            IList<EandMCodingICD> addlist = new List<EandMCodingICD>();
            IList<EandMCodingICD> updatelist = new List<EandMCodingICD>();
            IList<EandMCodingICD> deletelist = new List<EandMCodingICD>();

            IList<EandMCodingICD> temp;

            for (int i = 0; i < NeweandmICD.Count; i++)
            {
                var adding = from a in OldeandmICD where a.ICD == NeweandmICD[i].ICD select a;
                temp = adding.ToList<EandMCodingICD>();

                if (temp.Count > 0)
                {
                    if (NeweandmICD[i].ICD_Category != temp[0].ICD_Category)
                    {
                        NeweandmICD[i].Id = temp[0].Id;
                        updatelist.Add(NeweandmICD[i]);
                    }
                    else
                    {
                        NeweandmICD[i].Id = temp[0].Id;
                        updatelist.Add(NeweandmICD[i]);
                    }
                }
                else
                {
                    addlist.Add(NeweandmICD[i]);
                }
            }

            for (int i = 0; i < OldeandmICD.Count; i++)
            {
                //var adding = from a in NeweandmICD where a.ICD == OldeandmICD[i].ICD select a;
                //temp = adding.ToList<EandMCodingICD>();

                //if (temp.Count > 0)
                //{
                    deletelist.Add(OldeandmICD[i]);
                //}
            }
            return SaveUpdateDeleteWithoutTransaction(ref addlist, updatelist, deletelist, MySession, MACAddress);
        }

        //public void DeleteEMCodingICD(IList<EandMCodingICD> eandmICD, string MACAddress, ISession MySession) //kumar
        public int DeleteEMCodingICD(IList<EandMCodingICD> eandmICD, string MACAddress, ISession MySession)
        {
            IList<EandMCodingICD> add = null;

            return SaveUpdateDeleteWithoutTransaction(ref add, null, eandmICD, MySession, MACAddress);
        }

        //Added by bala on 6-Nov-2012 for Checking G codes
        public IList<EandMCodingICD> EandMcodingList(ulong encounterID)
        {
            IList<EandMCodingICD> EandMList = new List<EandMCodingICD>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit1 = iMySession.CreateCriteria(typeof(EandMCodingICD)).Add(Expression.Eq("Encounter_ID", encounterID));
                EandMList = crit1.List<EandMCodingICD>();
                iMySession.Close();
            }
            return EandMList;
        }

        public FillEandMCoding GetEandMcodingAndICDListbyEncounterID(ulong EncID)
        {
            FillEandMCoding EandMCodingdto = new FillEandMCoding();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit1 = iMySession.CreateCriteria(typeof(EAndMCoding)).Add(Expression.Eq("Encounter_ID", EncID)).Add(Expression.Eq("Is_Delete", "N"));
                EandMCodingdto.EandMCodingList = crit1.List<EAndMCoding>();

                ICriteria crit2 = iMySession.CreateCriteria(typeof(EandMCodingICD)).Add(Expression.Eq("Encounter_ID", EncID)).Add(Expression.Eq("Is_Delete", "N"));
                EandMCodingdto.EandMCodingICDList = crit2.List<EandMCodingICD>();
                iMySession.Close();
            }
            return EandMCodingdto;
        }
        public IList<EandMCodingICD> GetEandMcodingICDListbyEncounterID(ulong EncID)
        {
            IList<EandMCodingICD> EandMCodingdto = new List<EandMCodingICD>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit2 = iMySession.CreateCriteria(typeof(EandMCodingICD)).Add(Expression.Eq("Encounter_ID", EncID)).Add(Expression.Eq("Is_Delete", "N")).Add(Expression.Eq("Source","ASSESSMENT"));
                EandMCodingdto = crit2.List<EandMCodingICD>();
                iMySession.Close();
            }
            return EandMCodingdto;
        }

        public int GetEMICDCount(ulong ulEncID)
        {
            int count = 0; 
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {      
                ICriteria crit;
                crit = iMySession.CreateCriteria(typeof(EandMCodingICD)).Add(Expression.Eq("Encounter_ID", ulEncID)).Add(Expression.Eq("Is_Delete", "N"));
                count = crit.List<EandMCodingICD>().Count;
                iMySession.Close();
            }
            return count;
        }



        public IList<EandMCodingICD> GetEandMCodingICDPastEncounters(ulong encounterId, bool isFromArchive)
        {
            IList<EandMCodingICD> lst = new List<EandMCodingICD>();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                var querySQL = string.Format(@"SELECT E.* 
                                               FROM   {0} E  
WHERE   E.ENCOUNTER_ID = :ENCOUNTER_ID
                                               AND  is_Delete='N' and Source='ASSESSMENT'",
                                               isFromArchive ? "e_m_coding_icd_arc" : "e_m_coding_icd");

                var SQLQuery = iMySession.CreateSQLQuery(querySQL)
                       .AddEntity("E", typeof(EandMCodingICD));

                SQLQuery.SetParameter("ENCOUNTER_ID", encounterId);

                lst = SQLQuery.List<EandMCodingICD>();

                iMySession.Close();
            }
            return lst;
        }


        #endregion

    }
}
