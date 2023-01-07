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
    public interface INonDrugAllergyManager : IManagerBase<NonDrugAllergy, uint>
    {

        //Ginu
        NonDrugHistoryDTO SaveUpdateDeleteNonDrugHistory(IList<NonDrugAllergy> ExisList, IList<NonDrugAllergy> insertList, IList<NonDrugAllergy> updateList, IList<NonDrugAllergy> deleteList, ulong HumanID, GeneralNotes generalNotesobject, string macAddress, ulong Encounter_Id);
        NonDrugHistoryDTO GetNonDrugHistoryByHumanID(ulong humanID, ulong encounterID, string medicalInfo);

        //Ginu
    }
    public partial class NonDrugAllergyManager : ManagerBase<NonDrugAllergy, uint>, INonDrugAllergyManager
    {
        #region Constructors

        public NonDrugAllergyManager()
            : base()
        {

        }
        public NonDrugAllergyManager(INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Get Methods

        //Ginu

        public NonDrugHistoryDTO GetNonDrugHistoryByHumanID(ulong HumanID, ulong Encounter_Id, string medicalInfo)
        {
            NonDrugHistoryDTO objpro = new NonDrugHistoryDTO();
            EncounterManager encMngr = new EncounterManager();

            ICriteria criteria = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //criteria = iMySession.CreateCriteria(typeof(NonDrugAllergy)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Encounter_Id", Encounter_Id));
                //objpro.NonDrugList = criteria.List<NonDrugAllergy>();
                IQuery query = iMySession.GetNamedQuery("Get.Non.Drug.Allergy.And.Encounter.Details");
                query.SetParameter(0, HumanID);
                IList<NonDrugAllergy> lstAllNDA = new List<NonDrugAllergy>();
                ArrayList resultList = new ArrayList(query.List());
                Dictionary<ulong, DateTime> dictEnc = new Dictionary<ulong, DateTime>();
                foreach (object objResult in resultList)
                {
                    NonDrugAllergy objNDA = new NonDrugAllergy();
                    object[] lstObj = (object[])objResult;
                    objNDA.Id = Convert.ToUInt32(lstObj[0]);
                    objNDA.Human_ID = Convert.ToUInt32(lstObj[1]);
                    objNDA.Non_Drug_Allergy_History_Info = Convert.ToString(lstObj[2]);
                    objNDA.Is_Present = Convert.ToString(lstObj[3]);
                    objNDA.Description = Convert.ToString(lstObj[4]);
                    objNDA.Created_By = Convert.ToString(lstObj[5]);
                    objNDA.Created_Date_And_Time = Convert.ToDateTime(lstObj[6].ToString());
                    objNDA.Modified_By = Convert.ToString(lstObj[7]);
                    objNDA.Modified_Date_And_Time = Convert.ToDateTime(lstObj[8]);
                    objNDA.Version = Convert.ToInt32(lstObj[9]);
                    objNDA.Encounter_Id = Convert.ToUInt32(lstObj[10]);
                    if (!dictEnc.ContainsKey(Convert.ToUInt32(lstObj[11])))
                        dictEnc.Add(Convert.ToUInt32(lstObj[11]), Convert.ToDateTime(lstObj[12]));
                    lstAllNDA.Add(objNDA);
                }
                lstAllNDA = lstAllNDA.GroupBy(x => x.Id).Select(x => x.First()).ToList<NonDrugAllergy>();
                if (lstAllNDA.Count > 0)
                    objpro.NonDrugList = (from obj in lstAllNDA where obj.Encounter_Id == Encounter_Id select obj).ToList<NonDrugAllergy>();

                criteria = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_ID", Encounter_Id));
                IList<GeneralNotes> ge = criteria.List<GeneralNotes>();
                if (ge.Count > 0)
                    objpro.GeneralNotesObject = ge[0];

                bool Is_PreviousEnc = false;
                bool Is_PreviousEnc_GeneralNotes = false;
                ulong getGenID = 0;
                DateTime curr_DOS = DateTime.MinValue;
                if (dictEnc.Count != 0)
                    curr_DOS = Convert.ToDateTime(dictEnc[Encounter_Id]);
                if (objpro.NonDrugList != null)
                {
                    if (objpro.NonDrugList.Count == 0)
                    {
                        foreach (KeyValuePair<ulong, DateTime> entry in dictEnc)
                        {
                            if (DateTime.Compare(Convert.ToDateTime(entry.Value), curr_DOS) < 0)
                            {
                                if (lstAllNDA.Count > 0)
                                {
                                    objpro.NonDrugList = (from obj in lstAllNDA where obj.Encounter_Id == Convert.ToUInt32(entry.Key) select obj).ToList<NonDrugAllergy>();
                                    Is_PreviousEnc = true;
                                    getGenID = entry.Key;
                                    break;
                                }

                            }
                        }

                        if (!Is_PreviousEnc)
                        {
                            criteria = iMySession.CreateCriteria(typeof(NonDrugAllergy)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Encounter_Id", Convert.ToUInt64(0)));
                            objpro.NonDrugList = criteria.List<NonDrugAllergy>();
                        }
                    }
                }
                #region Commented for Bug Id: 33628
                //IList<Encounter> EncLst = new List<Encounter>();
                //EncLst = encMngr.GetEncounterUsingHumanID(HumanID);
                //Encounter currentObj=(from objEnc in EncLst where objEnc.Id==Encounter_Id select objEnc).ToList<Encounter>()[0];
                //if (objpro.NonDrugList.Count == 0)
                //{


                //    if (EncLst.Count > 0)
                //    {
                //        foreach (Encounter item in EncLst)
                //        {
                //            //if (Encounter_Id >= item.Id)//Commented for Bug Id: 33628
                //            if(DateTime.Compare(item.Date_of_Service,currentObj.Date_of_Service)<0)
                //            {
                //                criteria = iMySession.CreateCriteria(typeof(NonDrugAllergy)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Encounter_Id", item.Id));
                //                objpro.NonDrugList = criteria.List<NonDrugAllergy>();
                //                if (objpro.NonDrugList != null && objpro.NonDrugList.Count > 0)
                //                {
                //                    Is_PreviousEnc = true;

                //                    //criteria = session.GetISession().CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_ID", item.Id));
                //                    //IList<GeneralNotes> geNotest = criteria.List<GeneralNotes>();
                //                    //if (objpro.GeneralNotesObject == null && geNotest.Count > 0)
                //                    //    objpro.GeneralNotesObject = geNotest[0];
                //                    break;
                //                }
                //            }

                //        }

                //    }

                //    if (!Is_PreviousEnc)
                //    {
                //        criteria = iMySession.CreateCriteria(typeof(NonDrugAllergy)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Encounter_Id", Convert.ToUInt64(0)));
                //        objpro.NonDrugList = criteria.List<NonDrugAllergy>();
                //    }
                //}
                #endregion

                if (objpro.GeneralNotesObject == null)
                {
                    if (getGenID > 0)
                    {
                        criteria = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_ID", getGenID));
                        IList<GeneralNotes> geNotest = criteria.List<GeneralNotes>();
                        if (objpro.GeneralNotesObject == null && geNotest.Count > 0)
                        {
                            objpro.GeneralNotesObject = geNotest[0];
                            Is_PreviousEnc_GeneralNotes = true;
                        }
                    }
                    //foreach (Encounter item in EncLst)
                    //{
                    //    if (Encounter_Id >= item.Id)
                    //    {
                    //        criteria = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_ID", item.Id));
                    //        IList<GeneralNotes> geNotest = criteria.List<GeneralNotes>();
                    //        if (objpro.GeneralNotesObject == null && geNotest.Count > 0)
                    //        {
                    //            objpro.GeneralNotesObject = geNotest[0];
                    //            Is_PreviousEnc_GeneralNotes = true;

                    //            break;
                    //        }

                    //    }
                    //}


                    if (!Is_PreviousEnc_GeneralNotes)
                    {
                        criteria = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_ID", Convert.ToUInt64(0)));
                        if (objpro.GeneralNotesObject == null && ge.Count > 0)
                            objpro.GeneralNotesObject = ge[0];


                    }
                }
                iMySession.Close();
            }
            return objpro;

        }

        int iTryCount = 0;

        public NonDrugHistoryDTO SaveUpdateDeleteNonDrugHistory(IList<NonDrugAllergy> ExisList, IList<NonDrugAllergy> insertList, IList<NonDrugAllergy> updateList, IList<NonDrugAllergy> deleteList, ulong HumanID, GeneralNotes generalNotesObject, string macAddress, ulong Encounter_Id)
        {
            NonDrugHistoryDTO nonDrugDTO = new NonDrugHistoryDTO();
            List<GeneralNotes> combGnrlLst = new List<GeneralNotes>();
            IList<NonDrugAllergy> comblst = new List<NonDrugAllergy>();
            GeneralNotesManager generalNotesManager = new GeneralNotesManager();
            IList<GeneralNotes> generalNotesListInsert = new List<GeneralNotes>();
            IList<GeneralNotes> generalNotesListUpdate = new List<GeneralNotes>();
            IList<GeneralNotes> generalNotesMasterListInsert = new List<GeneralNotes>();
            IList<GeneralNotes> generalNotesMasterListUpdate = new List<GeneralNotes>();
            GenerateXml XMLObj = new GenerateXml();
            IList<NonDrugAllergy> lstSaveUpdateNDA= new List<NonDrugAllergy>();
            IList<NonDrugAllergyMaster> lstSaveUpdateMasterNDA = new List<NonDrugAllergyMaster>();
            IList<GeneralNotes> lstSaveUpdateNotes=new List<GeneralNotes>();
            IList<NonDrugAllergyMaster> _insertMasterList = new List<NonDrugAllergyMaster>();
            IList<NonDrugAllergyMaster> _updateMasterList = new List<NonDrugAllergyMaster>();
            IList<NonDrugAllergyMaster> _deleteMasterList = new List<NonDrugAllergyMaster>();
            bool bDataNDA = true, bDataNotes = true;
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
           


            iTryCount = 0;
        TryAgain:
            int iResult = 0;

            ISession MySession = Session.GetISession();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {

                    try
                    {
                        if (insertList.Count == 0)
                        {
                            insertList = null;
                        }
                        else if (insertList.Count > 0)
                        {
                            for (int i = 0; i < insertList.Count; i++)
                            {
                                NonDrugAllergyMaster objMaster = new NonDrugAllergyMaster();
                                if (insertList[i].Non_Drug_Allergy_History_Master_ID != 0)
                                {
                                    ICriteria crit = Session.GetISession().CreateCriteria(typeof(NonDrugAllergyMaster)).Add(Expression.Eq("Id", insertList[i].Non_Drug_Allergy_History_Master_ID));
                                    if (crit.List().Count != 0)
                                    {
                                        objMaster = crit.List<NonDrugAllergyMaster>()[0];
                                        objMaster.Non_Drug_Allergy_History_Info = insertList[i].Non_Drug_Allergy_History_Info;
                                        objMaster.Is_Deleted = "N";
                                        objMaster.Human_ID = insertList[i].Human_ID;
                                        objMaster.Is_Present = insertList[i].Is_Present;
                                        objMaster.Snomed_Code = insertList[i].Snomed_Code;
                                        objMaster.Description = insertList[i].Description;
                                        objMaster.Version = insertList[i].Version;
                                        objMaster.Modified_By = insertList[i].Created_By;
                                        objMaster.Modified_Date_And_Time = insertList[i].Created_Date_And_Time;
                                        _updateMasterList.Add(objMaster);
                                    }
                                }
                                else
                                {
                                    ICriteria crit = Session.GetISession().CreateCriteria(typeof(NonDrugAllergyMaster)).Add(Expression.Eq("Human_ID", insertList[i].Human_ID)).Add(Expression.Eq("Non_Drug_Allergy_History_Info", insertList[i].Non_Drug_Allergy_History_Info));
                                    if (crit.List().Count != 0)
                                    {
                                            objMaster = crit.List<NonDrugAllergyMaster>()[0];
                                            objMaster.Non_Drug_Allergy_History_Info = insertList[i].Non_Drug_Allergy_History_Info;
                                            objMaster.Is_Deleted = "N";
                                            objMaster.Human_ID = insertList[i].Human_ID;
                                            objMaster.Is_Present = insertList[i].Is_Present;
                                            objMaster.Snomed_Code = insertList[i].Snomed_Code;
                                            objMaster.Description = insertList[i].Description;
                                            objMaster.Version = insertList[i].Version;
                                            objMaster.Modified_By = insertList[i].Created_By;
                                            objMaster.Modified_Date_And_Time = insertList[i].Created_Date_And_Time;
                                            _updateMasterList.Add(objMaster);
                                        }
                                    else
                                    {
                                    objMaster.Non_Drug_Allergy_History_Info = insertList[i].Non_Drug_Allergy_History_Info;
                                    objMaster.Is_Deleted = "N";
                                    objMaster.Human_ID = insertList[i].Human_ID;
                                    objMaster.Is_Present = insertList[i].Is_Present;
                                    objMaster.Snomed_Code = insertList[i].Snomed_Code;
                                    objMaster.Description = insertList[i].Description;
                                    objMaster.Version = insertList[i].Version;
                                    objMaster.Created_By = insertList[i].Created_By;
                                    objMaster.Created_Date_And_Time = insertList[i].Created_Date_And_Time;
                                    _insertMasterList.Add(objMaster);
                                    }
                                }

                            }
 
                        }
                        if (updateList.Count == 0)
                        {
                            updateList = null;
                        }
                        else if(updateList.Count>0)
                        {
                             for (int i = 0; i < updateList.Count; i++)
                            {
                                NonDrugAllergyMaster objMaster = new NonDrugAllergyMaster();
                                if (updateList[i].Non_Drug_Allergy_History_Master_ID != 0)
                                {
                                    ICriteria crit = Session.GetISession().CreateCriteria(typeof(NonDrugAllergyMaster)).Add(Expression.Eq("Id", updateList[i].Non_Drug_Allergy_History_Master_ID));
                                    if (crit.List().Count != 0)
                                    {
                                        objMaster = crit.List<NonDrugAllergyMaster>()[0];
                                        objMaster.Non_Drug_Allergy_History_Info = updateList[i].Non_Drug_Allergy_History_Info;
                                        objMaster.Is_Deleted = "N";
                                        objMaster.Human_ID = updateList[i].Human_ID;
                                        objMaster.Is_Present = updateList[i].Is_Present;
                                        objMaster.Snomed_Code = updateList[i].Snomed_Code;
                                        objMaster.Description = updateList[i].Description;
                                        objMaster.Version = updateList[i].Version;
                                        objMaster.Modified_By = updateList[i].Created_By;
                                        objMaster.Modified_Date_And_Time = updateList[i].Created_Date_And_Time;
                                        _updateMasterList.Add(objMaster);
                                    }
                                }
                                else
                                {
                                    ICriteria crit = Session.GetISession().CreateCriteria(typeof(NonDrugAllergyMaster)).Add(Expression.Eq("Human_ID", updateList[i].Human_ID)).Add(Expression.Eq("Non_Drug_Allergy_History_Info", updateList[i].Non_Drug_Allergy_History_Info));
                                    if (crit.List().Count != 0)
                                    {
                                        objMaster = crit.List<NonDrugAllergyMaster>()[0];
                                        objMaster.Non_Drug_Allergy_History_Info = updateList[i].Non_Drug_Allergy_History_Info;
                                        objMaster.Is_Deleted = "N";
                                        objMaster.Human_ID = updateList[i].Human_ID;
                                        objMaster.Is_Present = updateList[i].Is_Present;
                                        objMaster.Snomed_Code = updateList[i].Snomed_Code;
                                        objMaster.Description = updateList[i].Description;
                                        objMaster.Version = updateList[i].Version;
                                        objMaster.Modified_By = updateList[i].Created_By;
                                        objMaster.Modified_Date_And_Time = updateList[i].Created_Date_And_Time;
                                        _updateMasterList.Add(objMaster);
                                    }
                                }
                            }
                        }
                        if (deleteList.Count == 0)
                        {
                            deleteList = null;
                        }
                        else if(deleteList.Count > 0)
                        {
                            for (int i = 0; i < deleteList.Count; i++)
                            {
                                NonDrugAllergyMaster objMaster = new NonDrugAllergyMaster();
                                if (deleteList[i].Non_Drug_Allergy_History_Master_ID != 0)
                                {
                                    ICriteria crit = Session.GetISession().CreateCriteria(typeof(NonDrugAllergyMaster)).Add(Expression.Eq("Id", deleteList[i].Non_Drug_Allergy_History_Master_ID));
                                    if (crit.List().Count != 0)
                                    {
                                        objMaster = crit.List<NonDrugAllergyMaster>()[0];
                                        objMaster.Non_Drug_Allergy_History_Info = deleteList[i].Non_Drug_Allergy_History_Info;
                                        objMaster.Is_Deleted = "Y";
                                        objMaster.Human_ID = deleteList[i].Human_ID;
                                        objMaster.Is_Present = deleteList[i].Is_Present;
                                        objMaster.Snomed_Code = deleteList[i].Snomed_Code;
                                        objMaster.Description = deleteList[i].Description;
                                        objMaster.Version = deleteList[i].Version;
                                        objMaster.Modified_By = deleteList[i].Created_By;
                                        objMaster.Modified_Date_And_Time = deleteList[i].Created_Date_And_Time;
                                        _updateMasterList.Add(objMaster);
                                    }
                                }
                                else
                                {
                                    ICriteria crit = Session.GetISession().CreateCriteria(typeof(NonDrugAllergyMaster)).Add(Expression.Eq("Human_ID", deleteList[i].Human_ID)).Add(Expression.Eq("Non_Drug_Allergy_History_Info", deleteList[i].Non_Drug_Allergy_History_Info));
                                    if (crit.List().Count != 0)
                                    {
                                        objMaster = crit.List<NonDrugAllergyMaster>()[0];
                                        objMaster.Non_Drug_Allergy_History_Info = deleteList[i].Non_Drug_Allergy_History_Info;
                                        objMaster.Is_Deleted = "Y";
                                        objMaster.Human_ID = deleteList[i].Human_ID;
                                        objMaster.Is_Present = deleteList[i].Is_Present;
                                        objMaster.Snomed_Code = deleteList[i].Snomed_Code;
                                        objMaster.Description = deleteList[i].Description;
                                        objMaster.Version = deleteList[i].Version;
                                        objMaster.Modified_By = deleteList[i].Created_By;
                                        objMaster.Modified_Date_And_Time = deleteList[i].Created_Date_And_Time;
                                        _updateMasterList.Add(objMaster);
                                    }
                                }
                            }
                        }

                         if (_insertMasterList.Count > 0 || _updateMasterList.Count > 0)
                        {
                            NonDrugAllergyMasterManager masterManager = new NonDrugAllergyMasterManager();
                            iResult = masterManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref _insertMasterList, ref _updateMasterList, null, MySession, macAddress, true, true, HumanID, string.Empty, ref XMLObj);
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
                                    throw new Exception("Ddeadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                            if (insertList != null)
                                lstSaveUpdateMasterNDA = lstSaveUpdateMasterNDA.Concat<NonDrugAllergyMaster>(_insertMasterList).ToList();
                            if (updateList != null)
                                lstSaveUpdateMasterNDA = lstSaveUpdateMasterNDA.Concat<NonDrugAllergyMaster>(_updateMasterList).ToList();

                            bDataNDA = XMLObj.CheckDataConsistency(lstSaveUpdateMasterNDA.Cast<object>().ToList(), true, string.Empty);

                        }

                         for (int i = 0; i < _insertMasterList.Count; i++)
                         {
                             for (int j = 0; j < insertList.Count; j++)
                             {
                                 if (insertList[j].Non_Drug_Allergy_History_Master_ID == 0 && _insertMasterList[i].Non_Drug_Allergy_History_Info == insertList[j].Non_Drug_Allergy_History_Info)
                                 {
                                     insertList[j].Non_Drug_Allergy_History_Master_ID = _insertMasterList[i].Id;
                                     break;
                                 }
                             }
                         }

                        iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref insertList, ref updateList, deleteList, MySession, macAddress, true, true, HumanID, string.Empty, ref XMLObj);
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
                                throw new Exception("Ddeadlock occurred. Transaction failed.");
                            }
                        }
                        else if (iResult == 1)
                        {
                            trans.Rollback();
                            throw new Exception("Exception occurred. Transaction failed.");
                        }
                        if (insertList != null)
                                    lstSaveUpdateNDA = lstSaveUpdateNDA.Concat<NonDrugAllergy>(insertList).ToList();
                        if (updateList != null)
                                    lstSaveUpdateNDA = lstSaveUpdateNDA.Concat<NonDrugAllergy>(updateList).ToList();
                        bDataNDA = XMLObj.CheckDataConsistency(lstSaveUpdateNDA.Cast<object>().ToList(), true,string.Empty);

                        if (generalNotesObject != null && generalNotesObject.Notes != string.Empty)
                        {
                            //Master General Notes transaction
                            //GeneralNotes objMasterGeneral = new GeneralNotes();
                            //ICriteria critMaster = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", "Non Drug Allergy History Master")).Add(Expression.Eq("Encounter_ID", Encounter_Id));
                            //IList<GeneralNotes> ResultMasterLst = critMaster.List<GeneralNotes>();
                            //if (ResultMasterLst != null && ResultMasterLst.Count > 0)
                            //{
                            //    ResultMasterLst[0].Encounter_ID = Encounter_Id;
                            //    ResultMasterLst[0].Parent_Field = "Non Drug Allergy History Master";
                            //    ResultMasterLst[0].Notes = generalNotesObject.Notes;
                            //    ResultMasterLst[0].Modified_By = generalNotesObject.Created_By;
                            //    ResultMasterLst[0].Modified_Date_And_Time = generalNotesObject.Created_Date_And_Time;
                            //    generalNotesListUpdate.Add(ResultMasterLst[0]);
                            //}
                            //else
                            //{
                            //    objMasterGeneral.Id = 0;
                            //    objMasterGeneral.Parent_Field = "Non Drug Allergy History Master";
                            //    objMasterGeneral.Encounter_ID = Encounter_Id;
                            //    objMasterGeneral.Version = 0;
                            //    objMasterGeneral.Human_ID = generalNotesObject.Human_ID;
                            //    objMasterGeneral.Notes = generalNotesObject.Notes;
                            //    objMasterGeneral.Created_By = generalNotesObject.Created_By;
                            //    objMasterGeneral.Created_Date_And_Time = generalNotesObject.Created_Date_And_Time;
                            //    generalNotesListInsert.Add(objMasterGeneral);
                            //}
                          



                            ICriteria crit = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", "Non Drug Allergy History")).Add(Expression.Eq("Encounter_ID", Encounter_Id));
                            IList<GeneralNotes> ResultLst = crit.List<GeneralNotes>();
                            iMySession.Close();
                            if (ResultLst != null && ResultLst.Count > 0)
                            {
                                ResultLst[0].Encounter_ID = Encounter_Id;
                                ResultLst[0].Notes = generalNotesObject.Notes;
                                ResultLst[0].Parent_Field = "Non Drug Allergy History";
                                ResultLst[0].Modified_By = generalNotesObject.Created_By;
                                ResultLst[0].Modified_Date_And_Time = generalNotesObject.Created_Date_And_Time;
                                generalNotesListUpdate.Add(ResultLst[0]);
                            }
                            else
                            {
                                generalNotesObject.Id = 0;
                                generalNotesObject.Encounter_ID = Encounter_Id;
                                generalNotesObject.Parent_Field = "Non Drug Allergy History";
                                generalNotesObject.Version = 0;
                                generalNotesListInsert.Add(generalNotesObject);
                            }
                            iResult = generalNotesManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref generalNotesListInsert, ref generalNotesListUpdate, null, MySession, macAddress, true, true, HumanID, "NonDrugAllergy", ref XMLObj);
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
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                            if (generalNotesListInsert != null)
                                    lstSaveUpdateNotes = lstSaveUpdateNotes.Concat<GeneralNotes>(generalNotesListInsert).ToList();
                            if (generalNotesListUpdate != null)
                                    lstSaveUpdateNotes = lstSaveUpdateNotes.Concat<GeneralNotes>(generalNotesListUpdate).ToList();
                            bDataNotes = XMLObj.CheckDataConsistency(lstSaveUpdateNotes.Cast<object>().ToList(), true, "NonDrugAllergy");
                            //bDataNotes = XMLObj.CheckDataConsistency(lstSaveUpdateNotes.Cast<object>().ToList(), true, "NonDrugAllergyMaster");
                        }
                        if (bDataNDA && bDataNotes)
                        {
                            //trans.Commit();
                           // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                            int trycount = 0;
                        trytosaveagain:
                            try
                            {
                                #region "Comment by Balaji.TJ - 2023-01-04"
                                //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                #endregion

                                #region "Modified by Balaji.TJ - 2023-01-04"
                                if (insertList != null || updateList != null || deleteList != null)
                                    WriteBlob(HumanID, XMLObj.itemDoc, MySession, insertList, updateList, deleteList, XMLObj, false);
                                else if (ExisList != null)
                                    WriteBlob(HumanID, XMLObj.itemDoc, MySession, null, ExisList, null, XMLObj, false);
                                #endregion
                            }
                            catch (Exception xmlexcep)
                            {
                                trycount++;
                                if (trycount <= 3)
                                {
                                    int TimeMilliseconds = 0;
                                    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                    Thread.Sleep(TimeMilliseconds);
                                    string sMsg = string.Empty;
                                    string sExStackTrace = string.Empty;

                                    string version = "";
                                    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                    string[] server = version.Split('|');
                                    string serverno = "";
                                    if (server.Length > 1)
                                        serverno = server[1].Trim();

                                    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                        sMsg = xmlexcep.InnerException.Message;
                                    else
                                        sMsg = xmlexcep.Message;

                                    if (xmlexcep != null && xmlexcep.StackTrace != null)
                                        sExStackTrace = xmlexcep.StackTrace;

                                    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                    string ConnectionData;
                                    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                    {
                                        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                        {
                                            cmd.Connection = con;
                                            try
                                            {
                                                con.Open();
                                                cmd.ExecuteNonQuery();
                                                con.Close();
                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                    goto trytosaveagain;
                                }
                            }
                            trans.Commit();
                        }
                        else
                            throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
                        
                        nonDrugDTO.NonDrugList = lstSaveUpdateNDA;
                        if(lstSaveUpdateNotes!=null && lstSaveUpdateNotes.Count>0)
                        nonDrugDTO.GeneralNotesObject = lstSaveUpdateNotes[0];
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        MySession.Close();
                    }
                }
            }
            catch (Exception ex1)
            {
                throw new Exception(ex1.Message);
            }
           
            return nonDrugDTO;
        }

        public NonDrugHistoryDTO CreateDTOForProblemHistory(ulong HumanID, string medicalInfo, ulong Encounter_Id)
        {
            NonDrugHistoryDTO problemDTOObject = new NonDrugHistoryDTO();
            IList<GeneralNotes> ge = new List<GeneralNotes>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(NonDrugAllergy)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Encounter_Id", Encounter_Id));
                problemDTOObject.NonDrugList = criteria.List<NonDrugAllergy>();
                criteria = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", medicalInfo)).Add(Expression.Eq("Encounter_ID", Encounter_Id));
                ge = criteria.List<GeneralNotes>();
                iMySession.Close();
            }
            if (ge.Count > 0)
            {
                problemDTOObject.GeneralNotesObject = ge[0];
            }
            return problemDTOObject;
        }
        //Ginu


        public void SaveinCopyPreviousEncounterNonDrugHistory(IList<NonDrugAllergy> insertList, IList<NonDrugAllergy> updateList, IList<NonDrugAllergy> deleteList, ulong HumanID, GeneralNotes generalNotesObject, string macAddress, ulong Encounter_Id)
        {
            GeneralNotesManager generalNotesManager = new GeneralNotesManager();
            IList<GeneralNotes> generalNotesListInsert = new List<GeneralNotes>();
            IList<GeneralNotes> generalNotesListUpdate = new List<GeneralNotes>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (generalNotesObject != null)
                {


                    ICriteria crit = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", "Non Drug Allergy History")).Add(Expression.Eq("Encounter_ID", Encounter_Id));
                    IList<GeneralNotes> ResultLst = crit.List<GeneralNotes>();
                    iMySession.Close();
                    //if (generalNotesObject.Id != 0)
                    //{
                    //    generalNotesListUpdate.Add(generalNotesObject);
                    //}
                    //else
                    //{
                    //    generalNotesListInsert.Add(generalNotesObject);
                    //}
                    if (ResultLst != null && ResultLst.Count > 0)
                    {
                        ResultLst[0].Encounter_ID = Encounter_Id;
                        ResultLst[0].Notes = generalNotesObject.Notes;
                        ResultLst[0].Modified_By = generalNotesObject.Modified_By;
                        ResultLst[0].Modified_Date_And_Time = generalNotesObject.Modified_Date_And_Time;
                        generalNotesListUpdate.Add(ResultLst[0]);
                    }
                    else
                    {
                        generalNotesObject.Id = 0;
                        generalNotesObject.Encounter_ID = Encounter_Id;
                        generalNotesObject.Version = 0;
                        generalNotesListInsert.Add(generalNotesObject);
                    }
                }

            }

            iTryCount = 0;
        TryAgain:
            int iResult = 0;

            using (ISession MySession = Session.GetISession())
            {
                // ITransaction trans = null;
                try
                {
                    using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {

                        try
                        {
                            //  trans = MySession.BeginTransaction();
                            if (insertList.Count == 0)
                            {
                                insertList = null;
                            }
                            if (updateList.Count == 0)
                            {
                                updateList = null;
                            }
                            if (deleteList.Count == 0)
                            {
                                deleteList = null;
                            }

                            iResult = SaveUpdateDeleteWithoutTransaction(ref insertList, updateList, deleteList, MySession, macAddress);
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
                                    //  MySession.Close();
                                    throw new Exception("Ddeadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                // MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }

                            if (generalNotesObject != null)
                            {
                                //GeneralNotesManager generalNotesManager = new GeneralNotesManager();
                                //IList<GeneralNotes> generalNotesListInsert = new List<GeneralNotes>();
                                //IList<GeneralNotes> generalNotesListUpdate = new List<GeneralNotes>();

                                //ICriteria crit = session.GetISession().CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Parent_Field", "Non Drug Allergy History")).Add(Expression.Eq("Encounter_ID", Encounter_Id));
                                //IList<GeneralNotes> ResultLst = crit.List<GeneralNotes>();

                                ////if (generalNotesObject.Id != 0)
                                ////{
                                ////    generalNotesListUpdate.Add(generalNotesObject);
                                ////}
                                ////else
                                ////{
                                ////    generalNotesListInsert.Add(generalNotesObject);
                                ////}
                                //if (ResultLst != null && ResultLst.Count > 0)
                                //{
                                //    ResultLst[0].Encounter_ID = Encounter_Id;
                                //    ResultLst[0].Notes = generalNotesObject.Notes;
                                //    ResultLst[0].Modified_By = generalNotesObject.Modified_By;
                                //    ResultLst[0].Modified_Date_And_Time = generalNotesObject.Modified_Date_And_Time;
                                //    generalNotesListUpdate.Add(ResultLst[0]);
                                //}
                                //else
                                //{
                                //    generalNotesObject.Id = 0;
                                //    generalNotesObject.Encounter_ID = Encounter_Id;
                                //    generalNotesObject.Version = 0;
                                //    generalNotesListInsert.Add(generalNotesObject);
                                //}

                                //iResult = generalNotesManager.SaveUpdateDeleteGeneralNotes(generalNotesListInsert, generalNotesListUpdate, MySession, macAddress);

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
                                        throw new Exception("Deadlock occurred. Transaction failed.");
                                    }
                                }
                                else if (iResult == 1)
                                {
                                    trans.Rollback();
                                    //MySession.Close();
                                    throw new Exception("Exception occurred. Transaction failed.");
                                }
                            }

                            MySession.Flush();
                            trans.Commit();
                        }
                        catch (NHibernate.Exceptions.GenericADOException ex)
                        {
                            trans.Rollback();
                            // MySession.Close();
                            throw new Exception(ex.Message);
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception(e.Message);
                        }
                        finally
                        {
                            MySession.Close();
                        }
                    }
                }
                catch (Exception ex1)
                {
                    //MySession.Close();
                    throw new Exception(ex1.Message);
                }

            }
        }
        #endregion
    }
}
