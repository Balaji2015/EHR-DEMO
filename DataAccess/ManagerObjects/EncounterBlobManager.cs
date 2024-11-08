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
using System.Net;
using System.Text;
using System.Configuration;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IEncounterBlobManager : IManagerBase<Encounter_Blob, ulong>
    {
        int SaveEncounterBlobWithoutTransaction(IList<Encounter_Blob> ListToInsertEncounterBlob, IList<Encounter_Blob> ListToUpdateEncounterBlob, ISession MySession, string MACAddress);

        IList<Encounter_Blob> GetEncounterBlob(ulong ulEncounterID);

        void SaveEncounterBlobWithTransaction( IList<Encounter_Blob> ListToUpdateEncounterBlob, string MACAddress);
        void LockEncounter(ulong ulEncounterID, ulong ulHumanID, string sUserName, DateTime dtModifiedDateTime);
        
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
        //Jira CAP-340 - New method for lock the DOCUMENT_COMPLEATE Encounter 
        public void LockEncounter(ulong ulEncounterID, ulong ulHumanID ,string sUserName ,DateTime dtModifiedDateTime)
        {
            IList<Encounter_Blob> ilstEncounterBlob = new List<Encounter_Blob>();
            IList<Human_Blob> ilstHumanBlob = new List<Human_Blob>();
            HumanBlobManager humanBlobManager = new HumanBlobManager();
            EncounterManager mngrEncounter = new EncounterManager();
            IList<Encounter> ilstEncounter = new List<Encounter>();

            ilstEncounterBlob = GetEncounterBlob(ulEncounterID);
            //Jira CAP-1485 - Start
            //ilstHumanBlob = humanBlobManager.GetHumanBlob(ulHumanID);
            ilstEncounter = mngrEncounter.GetEncounterByEncounterID(ulEncounterID);
            if (ilstEncounter != null && ilstEncounter.Count > 0 && ilstEncounter[0].Human_ID != 0)
            {
                ilstHumanBlob = humanBlobManager.GetHumanBlob(ilstEncounter[0].Human_ID);
            }
            //Jira CAP-1485 - End

            if (ilstEncounterBlob.Count > 0 && ilstHumanBlob.Count > 0)
            {
                for (int iCount = 0; iCount < ilstEncounterBlob.Count; iCount++)
                {
                    ilstEncounterBlob[iCount].Human_XML = ilstHumanBlob[iCount].Human_XML;
                    ilstEncounterBlob[iCount].Modified_Date_And_Time = dtModifiedDateTime;
                    ilstEncounterBlob[iCount].Modified_By = sUserName;
                }
                SaveEncounterBlobWithTransaction(ilstEncounterBlob, string.Empty);

                //CAP-2522,CAP-2623
                //if ((ConfigurationSettings.AppSettings["IsAkidoNoteCDC"]?.ToString()?.ToUpper() ?? "") == "Y" && ulEncounterID != 0 && ilstEncounter.Any(x=> !x.Is_Signed_in_Akido_Note.Equals("Y", StringComparison.InvariantCultureIgnoreCase)))
                if ((ConfigurationSettings.AppSettings["IsAkidoNoteCDC"]?.ToString()?.ToUpper() ?? "") == "Y" && ulEncounterID != 0 && ilstEncounter.Any(x => !x.Encounter_Provider_Signed_Date.ToString().Contains("0001-01-01")))
                {
                    IsAkidoCDC(ulHumanID.ToString(), ulEncounterID.ToString(), sUserName, dtModifiedDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
        }

        //CAP-2522
        public static void IsAkidoCDC(string sHumanID, string sEncounterID, string sTransactionBy, string sTransactionDateTime)
        {
            //Jira CAP-1379
            int iRetryCount = 0;

        retry:
            try
            {
                iRetryCount = iRetryCount + 1;
                string akidoNoteCDCURL = System.Configuration.ConfigurationSettings.AppSettings["AkidoNoteCDCURL"].ToString();
                akidoNoteCDCURL = akidoNoteCDCURL.Replace("[CapellaHumanID]", sHumanID).Replace("[CapellaEncounterID]", sEncounterID).Replace("[CapellaTransactionBy]", sTransactionBy).Replace("[CapellaTransactionDateTime]", sTransactionDateTime);
                var myUri = new Uri(akidoNoteCDCURL);
                string AccessToken = System.Configuration.ConfigurationSettings.AppSettings["AkidoNoteCDCURLToken"].ToString();
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();

                var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                var json = myStreamReader.ReadToEnd();
                responseStream.Close();
                myWebResponse.Close();

                //if (json.ToString() != "[]")
                //{
                //    bIsAkidoEncounter = "true";
                //    //Jira CAP-1990
                //    string sPJason = json.Substring(1, json.Length - 2);
                //    var jsonObject = JObject.Parse(sPJason);
                //}
            }
            catch (Exception ex)
            {
                //Jira CAP-1379
                //bIsAkidoEncounter = "Exception";
                //sExMessage = ex.Message;
                //Console.WriteLine(ex.ToString());

                //Jira CAP-1379
                //if (iRetryCount < 3)
                //{
                //    Console.WriteLine("Retrying Count : " + iRetryCount + " -> " + ex.ToString());
                //    System.Threading.Thread.Sleep(new TimeSpan(0, 0, 2));
                //    goto retry;
                //}
                //else
                //{
                //    Console.WriteLine(ex.ToString());
                //}

            }

            //return bIsAkidoEncounter;
        }
        #endregion
    }
}
