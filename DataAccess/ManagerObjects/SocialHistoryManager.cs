using System;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.Linq;
using System.Collections;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Threading;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface ISocialHistoryManager : IManagerBase<SocialHistory, ulong>
    {
        SocialHistoryDTO SaveUpdateDeleteSocialHistory(IList<SocialHistory> insertList, IList<SocialHistory> updateList, IList<SocialHistory> deleteList, ulong HumanID, GeneralNotes generalNotesObject, string macAddress, IList<SocialHistoryMaster> MasterinsertList, IList<SocialHistoryMaster> MasterupdateList, IList<SocialHistoryMaster> MasterdeleteList);
        SocialHistoryDTO GetSocialHistoryByHumanID(ulong humanID, ulong encounterID, string medicalInfo,bool From);
        void SaveSocialHistoryforSummary(IList<SocialHistory> lstsocial);
        IList<SocialHistory> GetSocHisByHumanID(ulong ulHumanID);
    }
    public partial class SocialHistoryManager : ManagerBase<SocialHistory, ulong>, ISocialHistoryManager
    {
        #region Constructors

        public SocialHistoryManager()
            : base()
        {

        }
        public SocialHistoryManager(INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Get Methods       

        public SocialHistoryDTO GetSocialHistoryByHumanID(ulong HumanID, ulong encounterID, string medicalInfo,bool From)
        {
            //SocialHistoryDTO problemDTOObject = new SocialHistoryDTO();
            //ICriteria criteria = session.GetISession().CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", humanID));
            //problemDTOObject.SocialList = criteria.List<SocialHistory>();
            //criteria = session.GetISession().CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", humanID)).Add(Expression.Eq("Parent_Field", medicalInfo));
            //IList<GeneralNotes> ge = criteria.List<GeneralNotes>();
            //if (ge.Count > 0)
            //{
            //    problemDTOObject.GeneralNotesObject = ge[0];
            //}
            //return problemDTOObject;
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            SocialHistoryDTO problemDTOObject = new SocialHistoryDTO();
            EncounterManager encMngr = new EncounterManager();
            ulong getGenID = 0;
            //DetachedCriteria subcrit = DetachedCriteria.For<SocialHistory>().SetProjection(Projections.Max(Projections.Property("Encounter_ID"))).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Le("Encounter_ID", encounterID)).AddOrder(Order.Desc("Encounter_ID"));Commented for Bug Id:33627
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //Commented for Bug Id:33627
                //ICriteria criteria = iMySession.CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", HumanID)).Add(Subqueries.PropertyIn("Encounter_ID", subcrit));
                ////ICriteria criteria = session.GetISession().CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Encounter_ID", encounterID));
                //problemDTOObject.SocialList = criteria.List<SocialHistory>();
                //subcrit = DetachedCriteria.For<GeneralNotes>().SetProjection(Projections.Max(Projections.Property("Encounter_ID"))).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Le("Encounter_ID", encounterID)).AddOrder(Order.Desc("Encounter_ID"));
                IQuery query = iMySession.GetNamedQuery("Get.Social.History.And.Encounter.Details");
                query.SetParameter(0, HumanID);
                IList<SocialHistory> lstSocial = new List<SocialHistory>();
                ArrayList resultList = new ArrayList(query.List());
                Dictionary<ulong, DateTime> dictEnc = new Dictionary<ulong, DateTime>();
                foreach (object objResult in resultList)
                {
                    SocialHistory objSoc = new SocialHistory();
                    object[] lstObj = (object[])objResult;
                    objSoc.Id = Convert.ToUInt32(lstObj[0]);
                    objSoc.Human_ID = Convert.ToUInt32(lstObj[1]);
                    objSoc.Social_Info = Convert.ToString(lstObj[2]);
                    objSoc.Is_Present = Convert.ToString(lstObj[3]);
                    objSoc.Description = Convert.ToString(lstObj[4]);
                    objSoc.Value = Convert.ToString(lstObj[5]);
                    objSoc.Recodes = Convert.ToString(lstObj[6]);
                    objSoc.Is_Mandatory = Convert.ToString(lstObj[7]);
                    objSoc.Created_By = Convert.ToString(lstObj[8]);
                    objSoc.Created_Date_And_Time = Convert.ToDateTime(lstObj[9].ToString());
                    objSoc.Modified_By = Convert.ToString(lstObj[10]);
                    objSoc.Modified_Date_And_Time = Convert.ToDateTime(lstObj[11]);
                    objSoc.Version = Convert.ToInt32(lstObj[12]);
                    objSoc.Encounter_ID = Convert.ToUInt32(lstObj[13]);
                    if (!dictEnc.ContainsKey(Convert.ToUInt32(lstObj[14])))
                    dictEnc.Add(Convert.ToUInt32(lstObj[14]), Convert.ToDateTime(lstObj[15]));
                    lstSocial.Add(objSoc);
                }
                lstSocial = lstSocial.GroupBy(x => x.Id).Select(x => x.First()).ToList<SocialHistory>();
                if (lstSocial.Count > 0)
                    problemDTOObject.SocialList = (from obj in lstSocial where obj.Encounter_ID == encounterID select obj).ToList<SocialHistory>();

                ICriteria criteria = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_ID", encounterID));
                IList<GeneralNotes> ge = criteria.List<GeneralNotes>();
                ge = ge.OrderByDescending(item => item.Modified_Date_And_Time).ToList();
                if (ge.Count > 0)
                {
                    problemDTOObject.GeneralNotesObject = ge[0];
                }
                #region Commented
                //<<<<<<< SocialHistoryManager.cs
                //<<<<<<< SocialHistoryManager.cs
                //            bool Is_PreviousEnc = false;
                //            if (problemDTOObject.SocialList.Count == 0)
                //            {
                //                IList<Encounter> EncLst = new List<Encounter>();
                //                EncLst = encMngr.GetEncounterUsingHumanID(HumanID);

                //                if (EncLst.Count > 0)
                //                {
                //                    foreach (Encounter item in EncLst)
                //                    {
                //                        criteria = session.GetISession().CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Encounter_ID", item.Id));
                //                        problemDTOObject.SocialList = criteria.List<SocialHistory>();
                //                        if (problemDTOObject.SocialList != null && problemDTOObject.SocialList.Count > 0)
                //                        {
                //                            Is_PreviousEnc = true;

                //                            criteria = session.GetISession().CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_ID", item.Encounter_ID));
                //                            IList<GeneralNotes> geNotest = criteria.List<GeneralNotes>();
                //                            if (problemDTOObject.GeneralNotesObject == null && geNotest.Count > 0)
                //                                problemDTOObject.GeneralNotesObject = geNotest[0];
                //                            break;
                //                        }
                //                    }
                //                }

                //                if (!Is_PreviousEnc)
                //                {

                //                    criteria = session.GetISession().CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Encounter_ID", Convert.ToUInt64(0)));
                //                    problemDTOObject.SocialList = criteria.List<SocialHistory>();
                //                    criteria = session.GetISession().CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_Id", Convert.ToUInt64(0)));
                //                    if (problemDTOObject.GeneralNotesObject == null && ge.Count > 0)
                //                        problemDTOObject.GeneralNotesObject = ge[0];

                //                }
                //            }

                //=======
                #endregion
                bool Is_PreviousEnc = false;
                bool Is_PreviousEnc_GeneralNotes = false;
                DateTime curr_DOS=DateTime.MinValue;
                if (dictEnc.Count != 0 && dictEnc.Keys.Contains(encounterID))
                    curr_DOS = Convert.ToDateTime(dictEnc[encounterID]);
                //IList<Encounter> EncLst = new List<Encounter>();
                /*** commented for perfomance tuning
                    * It should be called only if socialhistorylist is empty
                    * added inside the condition  if (problemDTOObject.SocialList.Count == 0)
                    *   by Jisha   ***/
                //  EncLst = encMngr.GetEncounterUsingHumanID(HumanID);
                if (problemDTOObject.SocialList != null)
                {
                if (problemDTOObject.SocialList.Count == 0 )
                {
                    #region Commented for Bug ID: 33627
                    //EncLst = encMngr.GetEncounterUsingHumanID(HumanID);
                    //Encounter currentObj = (from objEnc in EncLst where objEnc.Id == encounterID select objEnc).ToList<Encounter>()[0];
                    //if (EncLst.Count > 0)
                    //{
                    //    foreach (Encounter item in EncLst)
                    //    {
                    //        //if (encounterID >= item.Id)//Commented for Bug ID: 33627
                    //        if (DateTime.Compare(item.Date_of_Service, currentObj.Date_of_Service) < 0)
                    //        {
                    //            criteria = iMySession.CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Encounter_ID", item.Id));
                    //            problemDTOObject.SocialList = criteria.List<SocialHistory>();
                    //            if (problemDTOObject.SocialList != null && problemDTOObject.SocialList.Count > 0)
                    //            {
                    //                Is_PreviousEnc = true;
                    //                //criteria = session.GetISession().CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_ID", item.Id));
                    //                //IList<GeneralNotes> geNotest = criteria.List<GeneralNotes>();
                    //                //if (problemDTOObject.GeneralNotesObject == null && geNotest.Count > 0)
                    //                //    problemDTOObject.GeneralNotesObject = geNotest[0];
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                    foreach (KeyValuePair<ulong,DateTime> entry in dictEnc)
                    {
                        if (DateTime.Compare(Convert.ToDateTime(entry.Value), curr_DOS) < 0)
                        {
                            if (lstSocial.Count > 0)
                            {
                                problemDTOObject.SocialList = (from obj in lstSocial where obj.Encounter_ID == Convert.ToUInt32(entry.Key) select obj).ToList<SocialHistory>();
                                getGenID = entry.Key;
                                Is_PreviousEnc = true;
                                break;
                            }

                        }
                    }

                    if (!Is_PreviousEnc)
                    {

                        criteria = iMySession.CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Encounter_ID", Convert.ToUInt64(0)));
                        problemDTOObject.SocialList = criteria.List<SocialHistory>();
                        //criteria = session.GetISession().CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_Id", Convert.ToUInt64(0)));
                        //if (problemDTOObject.GeneralNotesObject == null && ge.Count > 0)
                        //    problemDTOObject.GeneralNotesObject = ge[0];

                    }
                }
            }
            


                if (problemDTOObject.GeneralNotesObject == null)
                {
                    if (getGenID > 0)
                    {
                        criteria = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_ID", getGenID));
                        IList<GeneralNotes> geNotest = criteria.List<GeneralNotes>();
                        if (geNotest.Count > 0)
                        {
                            problemDTOObject.GeneralNotesObject = geNotest[0];
                            Is_PreviousEnc_GeneralNotes = true;
                        }
                    }
                    //foreach (Encounter item in EncLst)
                    //{
                    //    if (encounterID >= item.Id)
                    //    {
                    //        criteria = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_ID", item.Id));
                    //        IList<GeneralNotes> geNotest = criteria.List<GeneralNotes>();
                    //        if (problemDTOObject.GeneralNotesObject == null && geNotest.Count > 0)
                    //        {
                    //            problemDTOObject.GeneralNotesObject = geNotest[0];
                    //            Is_PreviousEnc_GeneralNotes = true;

                    //            break;
                    //        }

                    //    }
                    //}


                    if (!Is_PreviousEnc_GeneralNotes)
                    {
                        criteria = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_ID", Convert.ToUInt64(0)));
                        if (problemDTOObject.GeneralNotesObject == null && ge.Count > 0)
                            problemDTOObject.GeneralNotesObject = ge[0];


                    }
                }


                /***  added for perfomance tuning
                         * StaticList only for Social History, not required for Move to button
                         *   by Jisha  ***/
                //commented by vaishali on 18-11-2015
                //if (From)
                //{
                //    ICriteria criteriastatic = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Like("Field_Name", "%SOCIAL HISTORY OPTION FOR TOBACCO USE AND EXPOSURE%"));
                //    problemDTOObject.StaticList = criteriastatic.List<StaticLookup>();

                //}
                iMySession.Close();
            }
            return problemDTOObject;
        }

        int iTryCount = 0;

        public SocialHistoryDTO SaveUpdateDeleteSocialHistory(IList<SocialHistory> insertList, IList<SocialHistory> updateList, IList<SocialHistory> deleteList, ulong HumanID, GeneralNotes generalNotesObject, string macAddress, IList<SocialHistoryMaster> MasterinsertList, IList<SocialHistoryMaster> MasterupdateList, IList<SocialHistoryMaster> MasterdeleteList)
        {
            HumanManager objHumanManager = null;
            IList<Human> UpdateHumanList = new List<Human>();
            IList<GeneralNotes> generalNotesListInsert = new List<GeneralNotes>();
            IList<GeneralNotes> generalNotesListUpdate = new List<GeneralNotes>();
            SocialHistoryDTO socHisDTO = new SocialHistoryDTO();
            GeneralNotesManager generalNotesManager = new GeneralNotesManager();
            SocialHistoryMasterManager objSocialHistoryManagerMaster = new SocialHistoryMasterManager();
            IList<SocialHistory> SaveSocialHisListFinal = new List<SocialHistory>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (insertList != null && insertList.Any(a => a.Social_Info == "Marital Status"))
                {
                    UpdateHumanList = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanID)).List<Human>();
                    if (UpdateHumanList != null && UpdateHumanList.Count()>0)
                        UpdateHumanList[0].Marital_Status = insertList.Where(a => a.Social_Info == "Marital Status").Select(a => a.Value).ToArray<string>()[0];
                }
                else if (updateList != null && updateList.Any(a => a.Social_Info == "Marital Status"))
                {
                    UpdateHumanList = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanID)).List<Human>();
                    if (UpdateHumanList != null && UpdateHumanList.Count() > 0)
                        UpdateHumanList[0].Marital_Status = updateList.Where(a => a.Social_Info == "Marital Status").Select(a => a.Value).ToArray<string>()[0];
                }
                else if (MasterinsertList != null && MasterinsertList.Any(a => a.Social_Info == "Marital Status"))
                {
                    UpdateHumanList = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanID)).List<Human>();
                    if (UpdateHumanList != null && UpdateHumanList.Count() > 0)
                        UpdateHumanList[0].Marital_Status = MasterinsertList.Where(a => a.Social_Info == "Marital Status").Select(a => a.Value).ToArray<string>()[0];
                }
                else if (MasterupdateList != null && MasterupdateList.Any(a => a.Social_Info == "Marital Status"))
                {
                    UpdateHumanList = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanID)).List<Human>();
                    if (UpdateHumanList != null && UpdateHumanList.Count() > 0)
                        UpdateHumanList[0].Marital_Status = MasterupdateList.Where(a => a.Social_Info == "Marital Status").Select(a => a.Value).ToArray<string>()[0];
                }
                //changed by vaishali on 17-11-2015
                if (generalNotesObject != null && generalNotesObject.Id > 0)
                {
                    generalNotesObject.Notes = generalNotesObject.Notes;
                    generalNotesObject.Modified_By = generalNotesObject.Modified_By;
                    generalNotesObject.Modified_Date_And_Time = generalNotesObject.Modified_Date_And_Time;
                    generalNotesListUpdate.Add(generalNotesObject);
                    generalNotesListInsert = null;
                }
                else
                {
                    if (generalNotesObject.Parent_Field!="")
                        generalNotesListInsert.Add(generalNotesObject);
                    generalNotesListUpdate = null;
                }

                iMySession.Close();
            }
            bool bSocialHistory = false;
            bool bGeneralNotes = false;
            IList<SocialHistory> FinalSocialList = new List<SocialHistory>();
            IList<GeneralNotes> FinalGeneralNotes = new List<GeneralNotes>();
            iTryCount = 0;
        TryAgain:
            int iResult = 0;
            GenerateXml ObjXML = new GenerateXml();
            ISession MySession = Session.GetISession();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        if (insertList != null && insertList.Count == 0)
                        {
                            insertList = null;
                        }
                        if (updateList != null && updateList.Count == 0)
                        {
                            updateList = null;
                        }
                        if (deleteList != null && deleteList.Count == 0)
                        {
                            deleteList = null;
                        }


                        if (MasterinsertList != null && MasterinsertList.Count == 0)
                        {
                            MasterinsertList = null;
                        }
                        if (MasterupdateList != null && MasterupdateList.Count == 0)
                        {
                            MasterupdateList = null;
                        }
                        if (MasterdeleteList != null && MasterdeleteList.Count == 0)
                        {
                            MasterdeleteList = null;
                        }

                        objSocialHistoryManagerMaster.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref MasterinsertList, ref MasterupdateList, MasterdeleteList, MySession, macAddress, true, true, HumanID, string.Empty, ref ObjXML);
                        IList<SocialHistoryMaster> lstSaveUpdateDeleteSHMaster = new List<SocialHistoryMaster>();
                        if(MasterinsertList!=null&&MasterinsertList.Count()>0&&MasterupdateList!=null&&MasterupdateList.Count()>0)
                            lstSaveUpdateDeleteSHMaster = MasterinsertList.Concat(MasterupdateList).ToList<SocialHistoryMaster>();
                        else if (MasterinsertList != null && MasterinsertList.Count() > 0)
                            lstSaveUpdateDeleteSHMaster = MasterinsertList;
                        else if (MasterupdateList != null && MasterupdateList.Count() > 0)
                            lstSaveUpdateDeleteSHMaster = MasterupdateList;

                        //Update Master Id in PMH
                        if (insertList != null)
                        {
                            foreach (SocialHistory objTemp in insertList)
                            {
                                IList<SocialHistoryMaster> lstPMHmasterTemp = new List<SocialHistoryMaster>();
                                lstPMHmasterTemp = lstSaveUpdateDeleteSHMaster.Where(a => a.Social_Info.Trim().ToUpper() == objTemp.Social_Info.Trim().ToUpper()).ToList<SocialHistoryMaster>();
                                objTemp.Social_History_Master_ID = lstPMHmasterTemp[0].Id;
                                SaveSocialHisListFinal.Add(objTemp);
                            }
                        }

                        iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveSocialHisListFinal, ref updateList, deleteList, MySession, macAddress, true, true, HumanID, string.Empty, ref ObjXML);
                       // iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref insertList, ref updateList, deleteList, MySession, macAddress, true, true, HumanID, string.Empty, ref ObjXML);
                        if (SaveSocialHisListFinal == null)
                            SaveSocialHisListFinal = new List<SocialHistory>();
                        if (updateList == null)
                            updateList = new List<SocialHistory>();
                        if (SaveSocialHisListFinal.Count > 0 || updateList.Count>0)
                        FinalSocialList = SaveSocialHisListFinal.Concat(updateList).ToList<SocialHistory>();
                        else
                        {
                            for(int i=0;i<lstSaveUpdateDeleteSHMaster.Count;i++)
                            {
                                SocialHistory obj = new SocialHistory();
                                obj.Social_Info = lstSaveUpdateDeleteSHMaster[i].Social_Info;
                                obj.Value = lstSaveUpdateDeleteSHMaster[i].Value;
                                obj.Is_Mandatory = lstSaveUpdateDeleteSHMaster[i].Is_Mandatory;
                                obj.Is_Present = lstSaveUpdateDeleteSHMaster[i].Is_Present;
                                obj.Description = lstSaveUpdateDeleteSHMaster[i].Description;
                                obj.Human_ID = lstSaveUpdateDeleteSHMaster[i].Human_ID;
                                FinalSocialList.Add(obj);
                            }
                        }
                        
                        bSocialHistory = ObjXML.CheckDataConsistency(SaveSocialHisListFinal.Concat(updateList).Cast<object>().ToList(), false, "");  
                   
                        if (iResult == 2)
                        {
                            if (iTryCount < 5)
                            {
                                iTryCount++;
                                goto TryAgain;
                            }
                            else
                            {
                                trans.Rollback();
                                //MySession.Close();
                                throw new Exception("Deadlock is occured. Transaction failed");
                            }
                        }
                        else if (iResult == 1)
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Exception is occured. Transaction failed");
                        }

                        if (UpdateHumanList != null && UpdateHumanList.Count > 0)
                        {
                            objHumanManager = new HumanManager();
                            //changed by vaishali on 17-11-2015
                            IList<Human> insertHumanList = null;
                            iResult = objHumanManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref insertHumanList, ref UpdateHumanList, null, MySession, macAddress, true, false, UpdateHumanList[0].Id, "", ref ObjXML);

                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {

                                    trans.Rollback();
                                    // MySession.Close();
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {

                                trans.Rollback();
                                //MySession.Close();
                                throw new Exception("Exception is occured. Transaction failed");

                            }
                        }

                        if (generalNotesObject != null)
                        {
                            iResult = generalNotesManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref generalNotesListInsert, ref generalNotesListUpdate, null, MySession, macAddress, true, true, HumanID, "SocialHistory", ref ObjXML);
                            if (generalNotesListInsert == null)
                                generalNotesListInsert = new List<GeneralNotes>();
                            if (generalNotesListUpdate == null)
                                generalNotesListUpdate = new List<GeneralNotes>();
                            FinalGeneralNotes = generalNotesListInsert.Concat(generalNotesListUpdate).ToList<GeneralNotes>();
                            bGeneralNotes = ObjXML.CheckDataConsistency(generalNotesListInsert.Concat(generalNotesListUpdate).Cast<object>().ToList(), false, "SocialHistory");
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {

                                    trans.Rollback();
                                    // MySession.Close();
                                    throw new Exception("Deadlock is occured. Transaction failed");

                                }
                            }
                            else if (iResult == 1)
                            {

                                trans.Rollback();
                                //MySession.Close();
                                throw new Exception("Exception is occured. Transaction failed");

                            }
                        }
                        if (bSocialHistory && bGeneralNotes)
                        {
                            //trans.Commit();
                           // ObjXML.itemDoc.Save(ObjXML.strXmlFilePath);
                            int trycount = 0;
                        //trytosaveagain:
                            try
                            {
                                #region "Comment by Balaji.TJ - 2023-03-01"
                                //ObjXML.itemDoc.Save(ObjXML.strXmlFilePath);
                                #endregion

                                #region "Modified by Balaji.TJ - 2023-03-01"
                                if ((insertList!=null && insertList.Count > 0) || (updateList!=null && updateList.Count > 0) || (deleteList!=null && deleteList.Count > 0))
                                {
                                    WriteBlob(HumanID, ObjXML.itemDoc, MySession, insertList, updateList, deleteList, ObjXML, false);
                                }
                                else
                                {
                                    SocialHistoryMasterManager objSocialMasterManager = new SocialHistoryMasterManager();
                                    objSocialMasterManager.WriteBlob(HumanID, ObjXML.itemDoc, MySession, MasterinsertList, MasterupdateList, MasterdeleteList, ObjXML, false);
                                }
                                #endregion
                            }
                            catch (Exception xmlexcep)
                            {
                                //trycount++;
                                //if (trycount <= 3)
                                //{
                                //    int TimeMilliseconds = 0;
                                //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                //    Thread.Sleep(TimeMilliseconds);
                                //    string sMsg = string.Empty;
                                //    string sExStackTrace = string.Empty;

                                //    string version = "";
                                //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                //    string[] server = version.Split('|');
                                //    string serverno = "";
                                //    if (server.Length > 1)
                                //        serverno = server[1].Trim();

                                //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                //        sMsg = xmlexcep.InnerException.Message;
                                //    else
                                //        sMsg = xmlexcep.Message;

                                //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                                //        sExStackTrace = xmlexcep.StackTrace;

                                //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                //    string ConnectionData;
                                //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                //    {
                                //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                //        {
                                //            cmd.Connection = con;
                                //            try
                                //            {
                                //                con.Open();
                                //                cmd.ExecuteNonQuery();
                                //                con.Close();
                                //            }
                                //            catch
                                //            {
                                //            }
                                //        }
                                //    }
                                //    goto trytosaveagain;
                                //}
                            }
                            trans.Commit();
                        }
                        else
                            throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
                        //trans.Commit();
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        MySession.Close();
                    }                 
                }
                //IList<SocialHistory> CombinedList = new List<SocialHistory>();
                //if (insertList != null && insertList.Count > 0)
                //{
                //    CombinedList = CombinedList.Concat(insertList).ToList<SocialHistory>();
                //}
                //if (updateList != null && updateList.Count > 0)
                //{
                //    CombinedList = CombinedList.Concat(updateList).ToList<SocialHistory>();
                //}
                //if (CombinedList.Any(a => a.Social_Info == "Marital Status"))
                //    CarePlanSaveOrUpdate(CombinedList, macAddress);

            }

            catch (Exception ex1)
            {
                //MySession.Close();
                throw new Exception(ex1.Message);
            }
            if (FinalGeneralNotes.Count > 0)
                socHisDTO.GeneralNotesObject = FinalGeneralNotes[0];
            socHisDTO.SocialList = FinalSocialList;
            return socHisDTO;
        }

        public SocialHistoryDTO CreateDTOForSocialHistory(ulong HumanID, string medicalInfo, ulong Encounter_Id)
        {
            //SocialHistoryDTO objpro = new SocialHistoryDTO();
            //ICriteria criteria = session.GetISession().CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", HumanID));
            //objpro.SocialList = criteria.List<SocialHistory>();
            //criteria = session.GetISession().CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo));
            //IList<GeneralNotes> ge = criteria.List<GeneralNotes>();
            //if (ge.Count > 0)
            //{
            //    objpro.GeneralNotesObject = ge[0];
            //}
            //return objpro;
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

            SocialHistoryDTO objpro = new SocialHistoryDTO();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Encounter_ID", Encounter_Id));
                objpro.SocialList = criteria.List<SocialHistory>();
                criteria = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_ID", Encounter_Id));
                IList<GeneralNotes> ge = criteria.List<GeneralNotes>();
                if (ge.Count > 0)
                {
                    objpro.GeneralNotesObject = ge[0];
                }
                iMySession.Close();
            }
            return objpro;
        }

        public void SaveSocialHistoryforSummary(IList<SocialHistory> lstsocial)
        {
            IList<SocialHistory> SocHisTemp = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref lstsocial, ref SocHisTemp, null, string.Empty, false, false, 0, "");
        }

        public void CarePlanSaveOrUpdate(IList<SocialHistory> SocialHistoryLst, string MacAddress)
        {
            CarePlanManager objCarePlanManager = new CarePlanManager();
            IList<CarePlan> lstSave = new List<CarePlan>();
            IList<CarePlan> lstUpdate = new List<CarePlan>();
            IList<CarePlan> lstDelete = new List<CarePlan>();
            ulong uHuman_id = SocialHistoryLst[0].Human_ID;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
                IList<CarePlan> lst = iMySession.CreateCriteria(typeof(CarePlan)).Add(Expression.Eq("Care_Name_Value", "Marital Status")).Add(Expression.Eq("Encounter_ID", SocialHistoryLst[0].Encounter_ID)).Add(Expression.Eq("Human_ID", SocialHistoryLst[0].Human_ID)).List<CarePlan>();
                if (lst != null && lst.Count > 0)
                {
                    lstUpdate = lst;
                    lstUpdate[0].Status = SocialHistoryLst.Where(a => a.Social_Info  == "Marital Status").Select(a => a.Value).ToArray<string>()[0];
                    lstUpdate[0].Care_Plan_Notes = SocialHistoryLst.Where(a => a.Social_Info == "Marital Status").Select(a => a.Description).ToArray<string>()[0];
                }
                else
                {

                    CarePlan objCarePlan = new CarePlan ();
                    objCarePlan.Human_ID = SocialHistoryLst[0].Human_ID;
                    objCarePlan.Care_Name = "Patient Details";
                    objCarePlan.Care_Name_Value = "Marital Status";
                    objCarePlan.Encounter_ID = SocialHistoryLst[0].Encounter_ID;
                    objCarePlan.Status = SocialHistoryLst.Where(a => a.Social_Info == "Marital Status").Select(a => a.Value).ToArray<string>()[0];
                    objCarePlan.Care_Plan_Notes = SocialHistoryLst.Where(a => a.Social_Info == "Marital Status").Select(a => a.Description).ToArray<string>()[0];
                    lstSave.Add(objCarePlan);
                }
                iMySession.Close();
            }
            //objSocialHistoryManager.SaveUpdateDeleteWithTransaction(ref lstSave, lstUpdate, lstDelete, MacAddress);
            objCarePlanManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref lstSave, ref lstUpdate, lstDelete, string.Empty, true, false, SocialHistoryLst[0].Encounter_ID, string.Empty);

        }

        public IList<SocialHistory> GetSocHisByHumanID(ulong ulHumanID)
        {
            IList<SocialHistory> SocHistoryList = new List<SocialHistory>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession mySession = NHibernateSessionManager.Instance.CreateISession();
                //  ISQLQuery sql = session.GetISession().CreateSQLQuery("select m.* from rcopia_medication m where m.Human_ID='"+ulHumanID+"' group by m.last_modified_date").AddEntity("m",typeof(Rcopia_Medication));
                ICriteria crit = mySession.CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", ulHumanID));
                SocHistoryList = crit.List<SocialHistory>();
                mySession.Close();
            }
            return SocHistoryList;
        }
        #endregion
    }
}
