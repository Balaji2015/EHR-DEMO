using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.Linq;
using System.Xml;

namespace Acurus.Capella.DataAccess.ManagerObjects
{

    public partial interface IRcopia_AllergyManager : IManagerBase<Rcopia_Allergy, ulong>
    {
        void InsertOrUpdateAllergy(IList<Rcopia_Allergy> ilstAllergy, string sUserName, string MACAddress, DateTime dtClientDate);
        IList<Rcopia_Allergy> GetAllergyListUsingHumanId(ulong ulhumanID);
        IList<Rcopia_Allergy> GetRAllergyByHumanID(string ulHumanID);
    }
    public partial class Rcopia_AllergyManager : ManagerBase<Rcopia_Allergy, ulong>, IRcopia_AllergyManager
    {
        #region Constructors

        public Rcopia_AllergyManager()
            : base()
        {

        }
        public Rcopia_AllergyManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods

        public void InsertOrUpdateAllergy(IList<Rcopia_Allergy> ilstAllergy, string sUserName, string MACAddress, DateTime dtClientDate)
        {
            if (ilstAllergy.Count > 0)
            {
                IList<Rcopia_Allergy> Rcopia_AllergyList = new List<Rcopia_Allergy>();
                IList<Rcopia_Allergy> Rcopia_AllergyUpdateList = new List<Rcopia_Allergy>();
                for (int i = 0; i < ilstAllergy.Count; i++)
                {
                    if (ilstAllergy[i].Human_ID != 0)
                    {
                        Rcopia_Allergy objAllergy = GetRcopiaAllergyRecords(ilstAllergy[i].Id);
                        if (objAllergy == null)
                        {
                            if (Rcopia_AllergyList.Count > 0)
                            {
                                if (!Rcopia_AllergyList.Any(item => item.Id == ilstAllergy[i].Id))
                                {
                                    if (Rcopia_AllergyUpdateList.Count > 0)
                                    {
                                        if (!Rcopia_AllergyUpdateList.Any(item => item.Id == ilstAllergy[i].Id))
                                        {
                                            ilstAllergy[i].Created_By = sUserName;
                                            ilstAllergy[i].Created_Date_And_Time = dtClientDate;
                                            Rcopia_AllergyList.Add(ilstAllergy[i]);
                                        }
                                    }
                                    else
                                    {
                                        ilstAllergy[i].Created_By = sUserName;
                                        ilstAllergy[i].Created_Date_And_Time = dtClientDate;
                                        Rcopia_AllergyList.Add(ilstAllergy[i]);
                                    }
                                }
                            }
                            else
                            {
                                ilstAllergy[i].Created_By = sUserName;
                                //ilstAllergy[i].Created_Date_And_Time = DateTime.Now;
                                //ilstAllergy[i].Created_Date_And_Time = DateTime.UtcNow;
                                ilstAllergy[i].Created_Date_And_Time = dtClientDate;
                                Rcopia_AllergyList.Add(ilstAllergy[i]);
                            }
                        }
                        else
                        {
                            objAllergy.Modified_By = sUserName;
                            //objAllergy.Modified_Date_And_Time = DateTime.Now;
                            //objAllergy.Modified_Date_And_Time = DateTime.UtcNow;
                            objAllergy.Modified_Date_And_Time = dtClientDate;
                            Rcopia_Allergy objUpdateAllergy = UpdateAllergyRecords(objAllergy, ilstAllergy[i]);
                            Rcopia_AllergyUpdateList.Add(objUpdateAllergy);
                        }
                    }
                }
                ulong EncounterORHumanId = 0;
                if (Rcopia_AllergyList.Count > 0)
                    EncounterORHumanId = Rcopia_AllergyList[0].Human_ID;
                else if (Rcopia_AllergyUpdateList.Count>0)
                    EncounterORHumanId = Rcopia_AllergyUpdateList[0].Human_ID;
                if (EncounterORHumanId!=0)
                    SaveUpdateDelete_DBAndXML_WithTransaction(ref Rcopia_AllergyList, ref Rcopia_AllergyUpdateList, null, MACAddress, true, true, EncounterORHumanId, string.Empty);

                //SaveUpdateDeleteWithTransaction(ref Rcopia_AllergyList, Rcopia_AllergyUpdateList, null, MACAddress);
                //GenerateXml XMLObj = new GenerateXml();
                //if (Rcopia_AllergyList.Count > 0)
                //{
                //    for (int i = 0; i < Rcopia_AllergyList.Count; i++)
                //    { 
                //    ulong uHuman_id = Rcopia_AllergyList[i].Human_ID;
                //    List<object> lstObj = new List<object>();
                //    lstObj.Add(Rcopia_AllergyList[i]);
                //    lstObj = lstObj.Cast<object>().ToList();
                //    XMLObj.GenerateXmlSaveStatic(lstObj, uHuman_id, string.Empty);
                //    }
                //}
                //if (Rcopia_AllergyUpdateList.Count > 0)
                //{
                //    for (int i = 0; i < Rcopia_AllergyUpdateList.Count; i++)
                //    {
                //        ulong uHuman_id = Rcopia_AllergyUpdateList[i].Human_ID;
                //        List<object> lstObj = new List<object>();
                //        lstObj.Add(Rcopia_AllergyUpdateList[i]);
                //        lstObj = lstObj.Cast<object>().ToList();
                //        XMLObj.GenerateXmlUpdate(lstObj, uHuman_id, string.Empty);
                //    }
                //}
            }
        }

        public Rcopia_Allergy GetRcopiaAllergyRecords(ulong ulRcopia_ID)
        {
            Rcopia_Allergy objAllergy = null;
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(Rcopia_Allergy)).Add(Expression.Eq("Id", ulRcopia_ID));
                if (crit.List<Rcopia_Allergy>().Count > 0)
                {
                    objAllergy = crit.List<Rcopia_Allergy>()[0];
                }
                mySession.Close();
            }
            return objAllergy;
        }

        public Rcopia_Allergy UpdateAllergyRecords(Rcopia_Allergy objAllergy, Rcopia_Allergy objUpdateAllergy)
        {
            objAllergy.Allergy_Name = objUpdateAllergy.Allergy_Name;
            objAllergy.First_DataBank_Med_ID = objUpdateAllergy.First_DataBank_Med_ID;
            objAllergy.Last_Modified_By = objUpdateAllergy.Last_Modified_By;
            objAllergy.Last_Modified_Date = objUpdateAllergy.Last_Modified_Date;
            objAllergy.NDC_ID = objUpdateAllergy.NDC_ID;
            objAllergy.OnsetDate = objUpdateAllergy.OnsetDate;
            objAllergy.Reaction = objUpdateAllergy.Reaction;
            objAllergy.Status = objUpdateAllergy.Status;
            objAllergy.Deleted = objUpdateAllergy.Deleted;
            return objAllergy;
        }

        public IList<Rcopia_Allergy> GetAllergyListUsingHumanId(ulong ulhumanID)
        {
            IList<Rcopia_Allergy> ilstRcopia_Allergy = new List<Rcopia_Allergy>();
            //ISession mySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Rcopia_Allergy)).Add(Expression.Eq("Human_ID", ulhumanID)).Add(Expression.Eq("Deleted", "N"));
                ilstRcopia_Allergy = crit.List<Rcopia_Allergy>();
                iMySession.Close();
            }
            return ilstRcopia_Allergy;
        }
        public IList<Rcopia_Allergy> GetMaxID()
        {
            IList<Rcopia_Allergy> Rcopia_AllergyList = new List<Rcopia_Allergy>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession mySession = NHibernateSessionManager.Instance.CreateISession();
                ISQLQuery sql = session.GetISession().CreateSQLQuery("select m.* from Rcopia_Allergy m order by rcopia_id desc limit 1 ").AddEntity("m", typeof(Rcopia_Allergy));
                // ICriteria crit = mySession.CreateCriteria(typeof(Rcopia_Medication)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Deleted", "N"));
                Rcopia_AllergyList = sql.List<Rcopia_Allergy>();
                mySession.Close();
            }
            return Rcopia_AllergyList;
        }

        public IList<Rcopia_Allergy> GetRAllergyByHumanID(string ulHumanID)
        {
            IList<Rcopia_Allergy> rcopiaMedicationList = new List<Rcopia_Allergy>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession mySession = NHibernateSessionManager.Instance.CreateISession();
                ISQLQuery sql = session.GetISession().CreateSQLQuery("select m.* from rcopia_allergy m where m.Human_ID in ('" + ulHumanID + "')").AddEntity("m", typeof(Rcopia_Allergy));
                // ICriteria crit = mySession.CreateCriteria(typeof(Rcopia_Medication)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Deleted", "N"));
                rcopiaMedicationList = sql.List<Rcopia_Allergy>();
                mySession.Close();
            }
            return rcopiaMedicationList;
        }

        public IList<Rcopia_Allergy> GetAllergyListByIdStatus(ulong ulhumanID,string Status)
        {
            IList<Rcopia_Allergy> ilstRcopia_Allergy = new List<Rcopia_Allergy>();
            //ISession mySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit;
                if (Status.ToUpper() == "ACTIVE")
                {
                     crit = iMySession.CreateCriteria(typeof(Rcopia_Allergy)).Add(Expression.Eq("Human_ID", ulhumanID)).Add(Expression.Eq("Deleted", "N")).Add(Expression.Eq("Status", Status));
                }
                else
                {
                     crit = iMySession.CreateCriteria(typeof(Rcopia_Allergy)).Add(Expression.Eq("Human_ID", ulhumanID)).Add(Expression.Eq("Deleted", "N"));
                }
               
                ilstRcopia_Allergy = crit.List<Rcopia_Allergy>();
                iMySession.Close();
            }
            return ilstRcopia_Allergy;
        }


        public string UpdateRcopiaAllergy(IList<ulong> ilstRcopiaID, ulong ulHumanID, string sFacilityName, string sLegalOrg, string sUserName)
        {
            if (ilstRcopiaID.Count == 0)
            {
                return "Success";
            }
            RCopiaGenerateXML rcopiaXML = new RCopiaGenerateXML();
            string sInputXML = string.Empty;
            string sOutputXML = string.Empty;
            string sRequestXML = string.Empty;
            IList<ulong> ilstAllergyId = new List<ulong>();
            DateTime dtRCopia_Allergy_Last_Updated_Date_and_Time = new DateTime(2001, 01, 01);
            RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
            RCopiaTransactionManager rCopiaTransactionManager = new RCopiaTransactionManager();

            //To get the Medication list from DrFirst
            sInputXML = rcopiaXML.CreateUpdateAllergyXML(ulHumanID, dtRCopia_Allergy_Last_Updated_Date_and_Time, sLegalOrg, DateTime.MinValue);

            if (rcopiaSessionMngr.DownloadAddress != null && sInputXML != string.Empty)
            {
                sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.DownloadAddress + sInputXML, 1, sUserName);
                //Jira CAP-1366
                if (sOutputXML != null && sOutputXML.StartsWith("HttpPostError") == true)
                {
                    return "Error during getting the allergy records for the patient - " + sOutputXML;
                }

                XmlDocument XMLDoc = new XmlDocument();
                if (sOutputXML != null)
                {
                    XMLDoc.LoadXml(sOutputXML);
                    int iAllergyListCount = XMLDoc.SelectNodes("RCExtResponse/Response/AllergyList/Allergy").Count;
                    int iResopnseAllergyCount = 0;
                    for (int iCont = 1; iCont <= iAllergyListCount; iCont++)
                    {

                        ilstAllergyId = ilstRcopiaID.Where(x => x.ToString() == XMLDoc.SelectSingleNode("RCExtResponse/Response/AllergyList/Allergy[" + iCont + "]/RcopiaID").InnerText).ToList<ulong>();

                        if (ilstAllergyId.Count > 0)
                        {
                            sRequestXML = sRequestXML + XMLDoc.SelectSingleNode("RCExtResponse/Response/AllergyList/Allergy[" + iCont + "]").OuterXml;
                            iResopnseAllergyCount++;
                        }
                    }
                    if (ilstRcopiaID.Count != iResopnseAllergyCount)
                    {
                        return "Allergy record is not found in DrFirst for RCopia ID : " + string.Join(",", ilstRcopiaID);
                    }
                    sRequestXML = "<AllergyList>" + sRequestXML + "</AllergyList>";

                    sRequestXML = rCopiaTransactionManager.createSendAllergyXml(ulHumanID, sLegalOrg, sRequestXML);

                    // To update the medication data in DrFirst
                    if (rcopiaSessionMngr.UploadAddress != null && sRequestXML != string.Empty && sRequestXML.ToUpper().Contains("<DELETED>N</DELETED>") == true)
                    {
                        sRequestXML = sRequestXML.Replace("<Deleted>n</Deleted>", "<Deleted>y</Deleted>").Replace("<Status><Active /></Status>", "<Status><Deleted /></Status>");
                        sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.UploadAddress + sRequestXML, 1, sUserName);
                        //Jira CAP-1366
                        if (sOutputXML != null && sOutputXML.StartsWith("HttpPostError") == true)
                        {
                            return "Error during updating the allergy record - " + sOutputXML;
                        }
                        XmlDocument XMLResponse = new XmlDocument();
                        XMLResponse.LoadXml(sOutputXML);
                        int iResponseAllergyListCount = XMLResponse.SelectNodes("RCExtResponse/Response/AllergyList/Allergy").Count;
                        string sStatus = string.Empty;
                        string sTemp = string.Empty;
                        string sErrorRcopiaID = string.Empty;
                        for (int iCont = 1; iCont <= iResponseAllergyListCount; iCont++)
                        {

                            sStatus = XMLResponse.SelectSingleNode("RCExtResponse/Response/AllergyList/Allergy[" + iCont + "]/Status")?.InnerText;
                            sTemp = XMLResponse.SelectSingleNode("RCExtResponse/Response/AllergyList/Allergy[" + iCont + "]/RcopiaID")?.InnerText;
                            if (sTemp != null && sStatus != null && sStatus != "deleted")
                            {
                                sErrorRcopiaID = sErrorRcopiaID + ((sErrorRcopiaID != string.Empty) ? "," : "RCopia ID : ") + sTemp + " : " + sStatus;
                            }
                            else if (sStatus == null || sTemp == null)
                            {
                                sErrorRcopiaID = sErrorRcopiaID + ((sErrorRcopiaID != string.Empty) ? "," : "")
                                    + ((sStatus == null && sTemp != null) ? "RCopia ID : " + sTemp + " Status is null" :
                                    (sStatus != null && sTemp == null) ? "RCopia ID is null " + sStatus : "RCopia ID and Status are null");
                            }
                        }
                        if (sErrorRcopiaID != string.Empty)
                        {
                            return "Allergy is not updated in DrFirst. \n For refernce : " + sErrorRcopiaID;
                        }

                    }
                    else
                    {
                        return (rcopiaSessionMngr.UploadAddress == null) ? "rcopiaSessionMngr.UploadAddress is null" : (sRequestXML == string.Empty) ? "sRequestXML is Empty" : "There is no DELETED tag in Response or DELETED is in 'Y'";
                    }
                }


                //Download Rcopia Data from the DrFirst
                string sErrorMessage = string.Empty;
                Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
                DateTime dtClientDate = DateTime.UtcNow;
                if (sUserName != null && sFacilityName != null)
                    //Commented the Patient Level RCopia Download
                    sErrorMessage = objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.DownloadAddress, sUserName, string.Empty, dtClientDate, sFacilityName, 0, ulHumanID, sLegalOrg);
                if (sErrorMessage != string.Empty)
                {
                    return "DownloadRCopiaInfo-" + sErrorMessage;
                }
            }

            return "Success";
        }


        #endregion

    }
}
