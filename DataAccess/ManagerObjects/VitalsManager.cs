using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using Acurus.Capella.DataAccess;

using System.IO;
using System.Runtime.Serialization;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;



namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IVitalsManager : IManagerBase<PatientResults, ulong>
    {
        PatientResultsDTO GetPastVitalDetailsByPatientforRcopia(ulong HumanId, int PgNumber, int MaxResult);
        PatientResultsDTO GetPastVitalDetailsByPatient(ulong HumanId, int PgNumber, int MaxResult, ulong PhyID, int age, string Sex, string Category, ulong ScreenId);
        PatientResultsDTO GetPastVitalDetailsByPatientforGrowthChart(ulong HumanId, int PgNumber, int MaxResult);
        PatientResultsDTO GetPastVitalDetailsByEncounterID(ulong EncounterID, ulong PhyID, int age, string Sex, string Category, ulong ScreenId, ulong HumanID);
        PatientResultsDTO AddVitalDetails(IList<PatientResults> obList, ulong HumanId, int PgNumber, int MaxResult, string sMacAddress, ulong EncounterID, ulong PhyID, int age, string Sex, string Category, ulong ScreenId, string UserName);
        IList<DynamicScreen> LoadVitalsBasedOnPhysician(IList<ulong> MasterIDList, ulong ScreenId);
        PatientResultsDTO UpdateVitalDetails(IList<PatientResults> updtList, IList<PatientResults> saveList, IList<PatientResults> delList, ulong HumanId, int PgNumber, int MaxResult, string sMacAddress, ulong EncounterID, ulong PhyID, int age, string Sex, string Category, ulong ScreenId, string UserNam);
        PatientResultsDTO DeleteVitalDetails(IList<PatientResults> delList, ulong HumanId, int PgNumber, int MaxResult, string sMacAddress, ulong PhyID, int age, string Sex, string Category, ulong ScreenId);
        IList<object> GetLocalData(ulong PhyID, int age, string Sex, string Category, ulong ScreenId );
        DataSet GetRowCount(string strConfiguration);
        DataSet GetRowCountforLocalDB(string strLocal, string strLocalDB);
        DataSet GetLatestResults(IList<PatientResults> lstVitalHuman);
        IList<DynamicScreen> GetAcurusCodesListforFlowsheet(ulong physicianId);
        IList<PatientResults> GetPatientListByResultMasterID(ulong ResultMasterID);
        PatientResults GetResultByLoincObservationAndEncounterId(string Loinc_Identifier, ulong Human_Id, ulong Encounter_Id);
        void SaveVitalsforSummary(IList<PatientResults> lstvitals);
        void InsertPatientResults(IList<PatientResults> ilstPatientResults);
        void InsertPatientResults(IList<PatientResults> ilstPatientResults, ref ISession session);
        IList<PatientResults> GetVitalsByHumanEncounter(ulong encounterId, ulong humanId);
        IList<string> GetHeightandWeightbyEncounterID(ulong EncounterID, DateTime dtCreatedorModifedDate, ulong uHumanID);
        IList<PatientResults> GetResultByLoincObservationAndEncounterIdStage3(string Loinc_Identifier, ulong Human_Id, string fromdate, string todate);
    }
    public partial class VitalsManager : ManagerBase<PatientResults, ulong>, IVitalsManager
    {
        #region Constructors

        public VitalsManager()
            : base()
        {

        }
        public VitalsManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods

        public PatientResultsDTO GetPastVitalDetailsByPatientforRcopia(ulong HumanId, int PgNumber, int MaxResult)
        {
            PatientResultsDTO objDTO = new PatientResultsDTO();
            IList<PatientResults> VitalList = new List<PatientResults>();
            IList<int> MonthList = new List<int>();
            IList<int> YearList = new List<int>();
            ArrayList arrList = new ArrayList();
            ArrayList grpIdList = new ArrayList();
            IList<object> grpId = new List<object>();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IQuery query = iMySession.GetNamedQuery("Fill.Vitals.GetVitalsCountAndGroupId")
                            .SetString(0, HumanId.ToString());
            arrList = new ArrayList(query.List());
            objDTO.VitalCount = arrList.Count;
            ICriteria crit = iMySession.CreateCriteria(typeof(PatientResults))
                .SetProjection(Projections.Max("Vitals_Group_ID"));
            grpId = crit.List<object>();
            if (grpId.Count > 0)
                objDTO.MaxGroupId = Convert.ToUInt64(grpId[0]);
            else
                objDTO.MaxGroupId = 0;

            PgNumber = PgNumber - 1;
            int MaxResult1 = PgNumber * MaxResult + MaxResult;
            for (int i = PgNumber * MaxResult; i < MaxResult1; i++)
            {
                if (i < arrList.Count)
                    grpIdList.Add(arrList[i]);
                else
                    break;
            }
            if (grpIdList.Count != 0)
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.Vitals.GetVitalsByHumanID")
                                .SetString(0, HumanId.ToString())
                                .SetParameterList("GroupIdlist", grpIdList);

                arrList = new ArrayList(query1.List());
                if (arrList.Count > 0)
                {
                    foreach (object[] obj in arrList)
                    {
                        PatientResults ObjVital = new PatientResults();
                        ObjVital.Id = Convert.ToUInt64(obj[0]);
                        ObjVital.Encounter_ID = Convert.ToUInt64(obj[1]);
                        ObjVital.Human_ID = Convert.ToUInt64(obj[2]);
                        ObjVital.Physician_ID = Convert.ToUInt64(obj[3]);
                        ObjVital.Loinc_Observation = obj[4].ToString();
                        ObjVital.Value = obj[5].ToString();
                        ObjVital.Created_By = obj[6].ToString();
                        ObjVital.Created_Date_And_Time = Convert.ToDateTime(obj[7]);
                        ObjVital.Modified_By = obj[8].ToString();
                        ObjVital.Modified_Date_And_Time = Convert.ToDateTime(obj[9]);
                        ObjVital.Vitals_Group_ID = Convert.ToUInt64(obj[10]);
                        ObjVital.Version = Convert.ToInt16(obj[11]);
                        ObjVital.Captured_date_and_time = Convert.ToDateTime(obj[12]);
                        ObjVital.Internal_Property_Month = Convert.ToInt32(obj[13]);
                        ObjVital.Internal_Property_Year = Convert.ToInt32(obj[14]);
                        ObjVital.Notes = obj[15].ToString();
                        VitalList.Add(ObjVital);

                    }
                }
            }
            objDTO.VitalsList = VitalList;
            iMySession.Close();
            return objDTO;
        }
        public PatientResultsDTO GetPastVitalDetailsByPatient(ulong HumanId, int PgNumber, int MaxResult, ulong PhyID, int age, string Sex, string Category, ulong ScreenId)
        {
            PatientResultsDTO objDTO = new PatientResultsDTO();
            IList<PatientResults> VitalList = new List<PatientResults>();
            IList<int> MonthList = new List<int>();
            IList<int> YearList = new List<int>();
            ArrayList arrList = new ArrayList();
            ArrayList grpIdList = new ArrayList();
            IList<object> grpId = new List<object>();
            ISession mySessionObj = NHibernateSessionManager.Instance.CreateISession();

            IQuery query = mySessionObj.GetNamedQuery("Fill.Vitals.GetVitalsCountAndGroupId")
                            .SetString(0, HumanId.ToString());

            arrList = new ArrayList(query.List());
            objDTO.VitalCount = arrList.Count;

            ICriteria crit = mySessionObj.CreateCriteria(typeof(PatientResults))
                .SetProjection(Projections.Max("Vitals_Group_ID"));
            grpId = crit.List<object>();
            if (grpId.Count > 0)
                objDTO.MaxGroupId = Convert.ToUInt64(grpId[0]);
            else
                objDTO.MaxGroupId = 0;

            PgNumber = PgNumber - 1;
            int MaxResult1 = PgNumber * MaxResult + MaxResult;
            for (int i = PgNumber * MaxResult; i < MaxResult1; i++)
            {
                if (i < arrList.Count)
                    grpIdList.Add(arrList[i]);
                else
                    break;
            }
            if (grpIdList.Count != 0)
            {
                IQuery query1 = mySessionObj.GetNamedQuery("Fill.Vitals.GetVitalsByHumanID")
                                .SetString(0, HumanId.ToString())
                                .SetParameterList("GroupIdlist", grpIdList);

                arrList = new ArrayList(query1.List());
                if (arrList.Count > 0)
                {
                    foreach (object[] obj in arrList)
                    {
                        PatientResults ObjVital = new PatientResults();
                        ObjVital.Id = Convert.ToUInt64(obj[0]);
                        ObjVital.Encounter_ID = Convert.ToUInt64(obj[1]);
                        ObjVital.Human_ID = Convert.ToUInt64(obj[2]);
                        ObjVital.Physician_ID = Convert.ToUInt64(obj[3]);
                        ObjVital.Loinc_Observation = obj[4].ToString();
                        ObjVital.Value = obj[5].ToString();
                        ObjVital.Created_By = obj[6].ToString();
                        ObjVital.Created_Date_And_Time = Convert.ToDateTime(obj[7]);
                        ObjVital.Modified_By = obj[8].ToString();
                        ObjVital.Modified_Date_And_Time = Convert.ToDateTime(obj[9]);
                        ObjVital.Vitals_Group_ID = Convert.ToUInt64(obj[10]);
                        ObjVital.Version = Convert.ToInt16(obj[11]);
                        ObjVital.Captured_date_and_time = Convert.ToDateTime(obj[12]);
                        ObjVital.Internal_Property_Month = Convert.ToInt32(obj[13]);
                        ObjVital.Internal_Property_Year = Convert.ToInt32(obj[14]);
                        ObjVital.Notes = obj[15].ToString();
                        // ObjVital.Reason_For_Not_Performed = obj[16].ToString();
                        VitalList.Add(ObjVital);

                    }
                }
            }
            objDTO.VitalsList = VitalList;
            objDTO.ObjList = GetLocalData(PhyID, age, Sex, Category, ScreenId);
            mySessionObj.Close();
            return objDTO;
        }
        public PatientResultsDTO GetPastVitalDetailsByPatientforGrowthChart(ulong HumanId, int PgNumber, int MaxResult)
        {
            PatientResultsDTO objDTO = new PatientResultsDTO();
            IList<PatientResults> VitalList = new List<PatientResults>();
            IList<int> MonthList = new List<int>();
            IList<int> YearList = new List<int>();
            ArrayList arrList = new ArrayList();
            ArrayList grpIdList = new ArrayList();
            IList<object> grpId = new List<object>();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IQuery query = iMySession.GetNamedQuery("Fill.Vitals.GetVitalsCountAndGroupId")
                            .SetString(0, HumanId.ToString());

            arrList = new ArrayList(query.List());
            objDTO.VitalCount = arrList.Count;

            ICriteria crit = iMySession.CreateCriteria(typeof(PatientResults))
                .SetProjection(Projections.Max("Vitals_Group_ID"));
            grpId = crit.List<object>();
            if (grpId.Count > 0)
                objDTO.MaxGroupId = Convert.ToUInt64(grpId[0]);
            else
                objDTO.MaxGroupId = 0;

            PgNumber = PgNumber - 1;
            int MaxResult1 = PgNumber * MaxResult + MaxResult;
            for (int i = PgNumber * MaxResult; i < MaxResult1; i++)
            {
                if (i < arrList.Count)
                    grpIdList.Add(arrList[i]);
                else
                    break;
            }
            if (grpIdList.Count != 0)
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.Vitals.GetVitalsByHumanIDForGrowthChart")
                                .SetString(0, HumanId.ToString())
                                .SetParameterList("GroupIdlist", grpIdList);

                arrList = new ArrayList(query1.List());
                if (arrList.Count > 0)
                {
                    foreach (object[] obj in arrList)
                    {
                        PatientResults ObjVital = new PatientResults();
                        ObjVital.Id = Convert.ToUInt64(obj[0]);
                        ObjVital.Encounter_ID = Convert.ToUInt64(obj[1]);
                        ObjVital.Human_ID = Convert.ToUInt64(obj[2]);
                        ObjVital.Physician_ID = Convert.ToUInt64(obj[3]);
                        ObjVital.Loinc_Observation = obj[4].ToString();
                        ObjVital.Value = obj[5].ToString();
                        ObjVital.Created_By = obj[6].ToString();
                        ObjVital.Created_Date_And_Time = Convert.ToDateTime(obj[7]);
                        ObjVital.Modified_By = obj[8].ToString();
                        ObjVital.Modified_Date_And_Time = Convert.ToDateTime(obj[9]);
                        ObjVital.Vitals_Group_ID = Convert.ToUInt64(obj[10]);
                        ObjVital.Version = Convert.ToInt16(obj[11]);
                        ObjVital.Captured_date_and_time = Convert.ToDateTime(obj[12]);
                        ObjVital.Internal_Property_Month = Convert.ToInt32(obj[13]);
                        ObjVital.Internal_Property_Year = Convert.ToInt32(obj[14]);
                        ObjVital.Notes = obj[15].ToString();
                        VitalList.Add(ObjVital);

                    }
                }
            }
            objDTO.VitalsList = VitalList;

            //Commented by Pravin on 19-Jan-2012
            //ICriteria criteriaHuman = session.GetISession().CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanId));
            //if (criteriaHuman.List<Human>().Count > 0)
            //{
            //    objDTO.objHuman = criteriaHuman.List<Human>()[0];
            //}
            iMySession.Close();
            return objDTO;

        }
        public PatientResultsDTO GetPastVitalDetailsByEncounterID(ulong EncounterID, ulong PhyID, int age, string Sex, string Category, ulong ScreenId, ulong HumanID)
        {
            PatientResultsDTO objDTO = new PatientResultsDTO();
            IList<PatientResults> VitalList = new List<PatientResults>();
            IList<PatientResults> VitalListHuman = new List<PatientResults>();
            IList<object> grpId = new List<object>();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            ICriteria crit = iMySession.CreateCriteria(typeof(PatientResults))
                .SetProjection(Projections.Max("Vitals_Group_ID"));
            grpId = crit.List<object>();
            if (grpId.Count > 0)
                objDTO.MaxGroupId = Convert.ToUInt64(grpId[0]);
            else
                objDTO.MaxGroupId = 0;
            ICriteria criteria = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Human_ID", HumanID));
            VitalListHuman = criteria.List<PatientResults>();
            VitalList = (from a in VitalListHuman
                         where a.Encounter_ID == EncounterID
                         select a).ToList<PatientResults>();

            if (VitalList != null && VitalList.Count > 0)
            {
                IList<PatientResults> tempList = VitalList.Select(a => { a.Loinc_Observation = a.Loinc_Observation.Contains("$") ? a.Loinc_Observation.Replace("$", "Second") : a.Loinc_Observation; return a; }).ToList<PatientResults>();
                VitalList = tempList;
            }
            if (VitalList.Count > 0)
            {
                objDTO.VitalsList = VitalList;

            }
            if (HumanID != 0)
                objDTO.dsLatestResults = GetLatestResults(VitalListHuman);
            objDTO.ObjList = GetLocalData(PhyID, age, Sex, Category, ScreenId);
            iMySession.Close();
            return objDTO;
        }

        public PatientResultsDTO UpdateVitalDetails(IList<PatientResults> updtList, IList<PatientResults> saveList, IList<PatientResults> delList, ulong HumanId, int PgNumber, int MaxResult, string sMacAddress, ulong EncounterID, ulong PhyID, int age, string Sex, string Category, ulong ScreenId, string UserName)
        {
            PatientResultsDTO objPatientResultsDTO = new PatientResultsDTO();
            if (updtList != null && updtList.Count > 0)
            {
                IList<PatientResults> tempList = updtList.Select(a => { a.Loinc_Observation = a.Loinc_Observation.Contains("Second") ? a.Loinc_Observation.Replace("Second", "$") : a.Loinc_Observation; return a; }).ToList<PatientResults>();
                updtList = tempList;
            }
            if (saveList != null && saveList.Count > 0)
            {
                IList<PatientResults> tempList = saveList.Select(a => { a.Loinc_Observation = a.Loinc_Observation.Contains("Second") ? a.Loinc_Observation.Replace("Second", "$") : a.Loinc_Observation; return a; }).ToList<PatientResults>();
                saveList = tempList;
            }
            IList<PatientResults> save = null;
            IList<PatientResults> update = null;
            IList<PatientResults> delete = null;
            if (updtList != null && updtList.Count > 0)
            {
                update = updtList;
            }
            if (saveList != null && saveList.Count > 0)
            {
                save = saveList;
            }
            if (delList != null && delList.Count > 0)
            {
                delete = delList;
            }

            SaveUpdateDelete_DBAndXML_WithTransaction(ref save, ref update, delete, sMacAddress, true, true, HumanId, string.Empty);
            #region
            //if (update != null && update.Count > 0)
            //{
            //    GenerateXml XMLObj = new GenerateXml();
            //    int iTryCount = 0;
            //    bool bDataConsistent = true;
            //TryAgain:
            //    int iResult = 0;

            //    ISession MySession = Session.GetISession();
            //    try
            //    {
            //        using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            //        {
            //            try
            //            {
            //                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref save, ref update, null, MySession, sMacAddress, true, true, HumanId, string.Empty, ref XMLObj);
            //                if (iResult == 2)
            //                {
            //                    if (iTryCount < 5)
            //                    {
            //                        iTryCount++;
            //                        goto TryAgain;
            //                    }
            //                    else
            //                    {
            //                        trans.Rollback();
            //                        throw new Exception("Deadlock occurred. Transaction failed.");
            //                    }
            //                }
            //                else if (iResult == 1)
            //                {
            //                    trans.Rollback();
            //                    throw new Exception("Exception occurred. Transaction failed.");
            //                }
            //                IList<PatientResults> lstSaveUpdate = new List<PatientResults>();
            //                if (update != null)
            //                    lstSaveUpdate = lstSaveUpdate.Concat<PatientResults>(update).ToList();

            //                bDataConsistent = XMLObj.CheckDataConsistency(lstSaveUpdate.Cast<object>().ToList(), true, string.Empty);
            //                if (bDataConsistent)
            //                {
            //                    trans.Commit();
            //                    XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
            //                }
            //                else
            //                    throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
            //            }

            //            catch (NHibernate.Exceptions.GenericADOException ex)
            //            {
            //                trans.Rollback();
            //                // MySession.Close();
            //                throw new Exception(ex.Message);
            //            }
            //            catch (Exception e)
            //            {
            //                trans.Rollback();
            //                //MySession.Close();
            //                throw new Exception(e.Message);
            //            }
            //            finally
            //            {
            //                MySession.Close();
            //            }
            //        }

            //    }
            //    catch (Exception ex1)
            //    {
            //        //MySession.Close();
            //        throw new Exception(ex1.Message);
            //    }
            //}
            #endregion
            if (EncounterID == 0)
            {
                objPatientResultsDTO = GetPastVitalDetailsByPatient(HumanId, PgNumber, MaxResult, PhyID, age, Sex, Category, ScreenId);
            }
            else
            {
                objPatientResultsDTO = GetPastVitalDetailsByEncounterID(EncounterID, PhyID, age, Sex, Category, ScreenId, HumanId);
            }
            //GenerateXml XMLObj = new GenerateXml();
            //if (saveList != null && saveList.Count > 0)
            //{

            //    ulong humanId = saveList[0].Human_ID;
            //    List<object> lstObj = saveList.Cast<object>().ToList();
            //    // XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //    XMLObj.GenerateXmlSaveStatic(lstObj, humanId, string.Empty);
            //}
            //if (updtList != null && updtList.Count > 0)
            //{

            //    ulong humanId = updtList[0].Human_ID;
            //    List<object> lstObj = updtList.Cast<object>().ToList();
            //    // XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //    XMLObj.GenerateXmlUpdate(lstObj, humanId, string.Empty);
            //}
            //if (delList != null && delList.Count > 0)
            //{

            //    ulong humanId = delList[0].Human_ID;
            //    List<object> lstObj = delList.Cast<object>().ToList();
            //    XMLObj.DeleteXmlNode(humanId, lstObj, string.Empty);
            //}
            return objPatientResultsDTO;

        }
        public void VitalswithRcopia(ulong HumanId, ulong EncounterId)
        {
            IList<PatientResults> SavePatientResults = new List<PatientResults>();
            IList<PatientResults> UpdatePatientResults = new List<PatientResults>();
            IList<PatientResults> DeletePatientResults = new List<PatientResults>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Encounter_ID", EncounterId));
                UpdatePatientResults = crit.List<PatientResults>();
                var VitalRcopia = from v in UpdatePatientResults where v.Is_Sent_to_Rcopia == "N" select v;
                if (VitalRcopia.Count() > 0)
                {
                    //RCopiaTransactionManager objRcopia = new RCopiaTransactionManager();
                    //objRcopia.SendPatientToRCopia(HumanId, string.Empty);
                    for (int i = 0; i < UpdatePatientResults.Count; i++)
                    {
                        UpdatePatientResults[i].Is_Sent_to_Rcopia = "Y";
                    }
                    SaveUpdateDeleteWithTransaction(ref SavePatientResults, UpdatePatientResults, DeletePatientResults, string.Empty);
                }
                iMySession.Close();
            }

        }

        public PatientResultsDTO AddVitalDetails(IList<PatientResults> obList, ulong HumanId, int PgNumber, int MaxResult, string sMacAddress, ulong EncounterID, ulong PhyID, int age, string Sex, string Category, ulong ScreenId, string UserName)
        {
            PatientResultsDTO objPatientResultsDTO = new PatientResultsDTO();
            IList<PatientResults> UpdatePatientResult = null;
            if (obList != null && obList.Count > 0)
            {

                IList<PatientResults> tempList = obList.Select(a => { a.Loinc_Observation = a.Loinc_Observation.Contains("Second") ? a.Loinc_Observation.Replace("Second", "$") : a.Loinc_Observation; return a; }).ToList<PatientResults>();
                obList = tempList;
                SaveUpdateDelete_DBAndXML_WithTransaction(ref obList, ref UpdatePatientResult, null, sMacAddress, true, true, HumanId, string.Empty);

            }
            #region
            //if (obList != null && obList.Count > 0)
            //{
            //    GenerateXml XMLObj = new GenerateXml();
            //    int iTryCount = 0;
            //    bool bDataConsistent = true;
            //TryAgain:
            //    int iResult = 0;

            //    ISession MySession = Session.GetISession();
            //    try
            //    {
            //        using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            //        {
            //            try
            //            {
            //                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref obList, ref UpdatePatientResult, null, MySession, sMacAddress, true, true, HumanId, string.Empty, ref XMLObj);
            //                if (iResult == 2)
            //                {
            //                    if (iTryCount < 5)
            //                    {
            //                        iTryCount++;
            //                        goto TryAgain;
            //                    }
            //                    else
            //                    {
            //                        trans.Rollback();
            //                        throw new Exception("Deadlock occurred. Transaction failed.");
            //                    }
            //                }
            //                else if (iResult == 1)
            //                {
            //                    trans.Rollback();
            //                    throw new Exception("Exception occurred. Transaction failed.");
            //                }
            //                IList<PatientResults> lstSaveUpdate = new List<PatientResults>();
            //                if (obList != null)
            //                    lstSaveUpdate = lstSaveUpdate.Concat<PatientResults>(obList).ToList();

            //                bDataConsistent = XMLObj.CheckDataConsistency(lstSaveUpdate.Cast<object>().ToList(), true, string.Empty);
            //                if (bDataConsistent)
            //                {
            //                    trans.Commit();
            //                    XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
            //                }
            //                else
            //                    throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
            //            }

            //            catch (NHibernate.Exceptions.GenericADOException ex)
            //            {
            //                trans.Rollback();
            //                // MySession.Close();
            //                throw new Exception(ex.Message);
            //            }
            //            catch (Exception e)
            //            {
            //                trans.Rollback();
            //                //MySession.Close();
            //                throw new Exception(e.Message);
            //            }
            //            finally
            //            {
            //                MySession.Close();
            //            }
            //        }

            //    }
            //    catch (Exception ex1)
            //    {
            //        //MySession.Close();
            //        throw new Exception(ex1.Message);
            //    }
            //}
            #endregion
            if (EncounterID == 0)
            {
                objPatientResultsDTO = GetPastVitalDetailsByPatient(HumanId, PgNumber, MaxResult, PhyID, age, Sex, Category, ScreenId);
            }
            else
            {
                objPatientResultsDTO = GetPastVitalDetailsByEncounterID(EncounterID, PhyID, age, Sex, Category, ScreenId, HumanId);
            }
            //GenerateXml XMLObj = new GenerateXml();
            //if (obList.Count > 0)
            //{
            //    ulong encounterid = obList[0].Encounter_ID;
            //    List<object> lstObj = obList.Cast<object>().ToList();
            //    //XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //    XMLObj.GenerateXmlSaveStatic(lstObj, HumanId, string.Empty);
            //}
            return objPatientResultsDTO;
        }

        public PatientResultsDTO DeleteVitalDetails(IList<PatientResults> delList, ulong HumanId, int PgNumber, int MaxResult, string sMacAddress, ulong PhyID, int age, string Sex, string Category, ulong ScreenId)
        {
            IList<PatientResults> SavePatientResult = null;
            IList<PatientResults> UpdatePatientResult = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref SavePatientResult, ref UpdatePatientResult, delList, sMacAddress, false, false, 0, string.Empty);
            return GetPastVitalDetailsByPatient(HumanId, PgNumber, MaxResult, PhyID, age, Sex, Category, ScreenId);
        }
        //LoadVitalsBasedOnPhysician method was declared but never used.
        public IList<DynamicScreen> LoadVitalsBasedOnPhysician(IList<ulong> MasterVitalIDList, ulong ScreenId)
        {
            IList<DynamicScreen> ilstDynamicScreen = new List<DynamicScreen>();
            using (ISession MySessionObj = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = MySessionObj.CreateCriteria(typeof(DynamicScreen)).Add(Expression.Eq("Screen_ID", ScreenId))
                   .Add(Expression.In("Master_Vitals_ID", MasterVitalIDList.ToArray<ulong>()))
                   .AddOrder(Order.Asc("Sort_Order"));
                ilstDynamicScreen = criteria.List<DynamicScreen>();
                MySessionObj.Close();
            }
            return ilstDynamicScreen;
        }

        public DataSet GetRowCount(string strConfiguration)
        {
            IList<object> ResultList = new List<object>();
            DataSet ds = new DataSet();
            DataTable dtTable = new DataTable();
            DataRow dr = null;
            ISQLQuery sql2 = session.GetISession().CreateSQLQuery("show tables  FROM " + strConfiguration + " ");
            dtTable.Columns.Add("Schema Name");
            dtTable.Columns.Add("Table Name");
            dtTable.Columns.Add("No of Rows");
            foreach (string objtable in sql2.List())
            {
                dr = dtTable.NewRow();
                dr["Schema Name"] = strConfiguration;
                dr["Table Name"] = objtable.ToString();
                if (objtable.ToString() != "check" && objtable.ToString() != "rxnsat" && objtable.ToString() != "rxnconso")
                {
                    ISQLQuery sql1 = session.GetISession().CreateSQLQuery("select count(*)  FROM " + objtable.ToString() + " ");

                    foreach (Int64 Count in sql1.List())
                    {
                        dr["No of Rows"] = Count.ToString();

                    }
                }
                dtTable.Rows.Add(dr);
            }


            ds.Tables.Add(dtTable);
            return ds;
        }
        public DataSet GetRowCountforLocalDB(string strLocal, string strLocalDB)
        {
            IList<object> ResultList = new List<object>();
            DataSet ds = new DataSet();
            DataTable dtTable = new DataTable();
            DataRow dr = null;
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            // ISQLQuery sql2 = session.GetISession().CreateSQLQuery("SELECT table_name,table_rows FROM `information_schema`.`TABLES` where table_schema='" + strConfiguration + "'");
            ISQLQuery sql2 = iMySession.CreateSQLQuery("SELECT * FROM main.sqlite_master WHERE type='table'");
            dtTable.Columns.Add("Schema Name");
            dtTable.Columns.Add("Table Name");
            dtTable.Columns.Add("Table Rows");
            foreach (IList<object> objtable in sql2.List())
            {

                dr = dtTable.NewRow();
                dr["Schema Name"] = strLocalDB + " - " + strLocal;
                dr["Table Name"] = objtable[2].ToString();
                if (objtable[2].ToString() != "check")
                {
                    ISQLQuery sql1 = iMySession.CreateSQLQuery("select count(*)  FROM " + objtable[2].ToString() + " ");

                    foreach (Int64 Count in sql1.List())
                    {
                        dr["Table Rows"] = Count.ToString();

                    }
                }
                dtTable.Rows.Add(dr);
            }
            ds.Tables.Add(dtTable);
            iMySession.Close();
            return ds;
        }



        public DataSet GetLatestResults(IList<PatientResults> lstVitalHuman)
        {
            IList<PatientResults> LatestResults = new List<PatientResults>();
            IList<PatientResults> HBA1cResults = new List<PatientResults>();
            IList<PatientResults> egfrResults = new List<PatientResults>();
            IList<PatientResults> BloodFastingResults = new List<PatientResults>();
            IList<PatientResults> BloodPostResults = new List<PatientResults>();
            IList<PatientResults> HgbResults = new List<PatientResults>();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            HBA1cResults = (from v in lstVitalHuman
                            where v.Value != "" && (v.Loinc_Identifier == "55454-3")
                            orderby v.Captured_date_and_time descending
                            select v).Take(1).ToList<PatientResults>();
            egfrResults = (from v in lstVitalHuman
                           where v.Value != "" && (v.Loinc_Identifier == "50383-9")
                           orderby v.Captured_date_and_time descending
                           select v).Take(1).ToList<PatientResults>();
            BloodFastingResults = (from v in lstVitalHuman
                                   where v.Value != "" && (v.Loinc_Identifier == "35184-1")
                                   orderby v.Captured_date_and_time descending
                                   select v).Take(1).ToList<PatientResults>();
            BloodPostResults = (from v in lstVitalHuman
                                where v.Value != "" && (v.Loinc_Identifier == "44919-9")
                                orderby v.Captured_date_and_time descending
                                select v).Take(1).ToList<PatientResults>();
            HgbResults = (from v in lstVitalHuman
                          where v.Value != "" && (v.Loinc_Identifier == "718-7")
                          orderby v.Captured_date_and_time descending
                          select v).Take(1).ToList<PatientResults>();
            DataSet ds = new DataSet();
            DataTable dtTable = new DataTable();
            DataRow dr = null;

            dtTable.Columns.Add("Vital_Name");
            dtTable.Columns.Add("Vital_Value");
            dtTable.Columns.Add("Captured_date_and_time");
            dtTable.Columns.Add("Reason_For_Refuse");

            if (HBA1cResults.Count != 0)
            {
                dr = dtTable.NewRow();
                dr["Vital_Name"] = "HbA1C";
                dr["Vital_Value"] = HBA1cResults[0].Value + "%";
                dr["Captured_date_and_time"] = HBA1cResults[0].Captured_date_and_time;
                dr["Reason_For_Refuse"] = HBA1cResults[0].Notes;
                dtTable.Rows.Add(dr);
            }
            if (HgbResults.Count != 0)//BugID:51544
            {
                dr = dtTable.NewRow();
                dr["Vital_Name"] = "Hgb";
                dr["Vital_Value"] = HgbResults[0].Value + "g/dl";
                dr["Captured_date_and_time"] = HgbResults[0].Captured_date_and_time;
                dr["Reason_For_Refuse"] = HgbResults[0].Notes;
                dtTable.Rows.Add(dr);
            }
            if (egfrResults.Count != 0)
            {
                dr = dtTable.NewRow();
                dr["Vital_Name"] = "eGFR";
                dr["Vital_Value"] = egfrResults[0].Value + "ml/min/1.73m2";
                dr["Captured_date_and_time"] = egfrResults[0].Captured_date_and_time;
                dr["Reason_For_Refuse"] = egfrResults[0].Notes;
                dtTable.Rows.Add(dr);
            }
            if (BloodFastingResults.Count != 0)
            {
                dr = dtTable.NewRow();
                dr["Vital_Name"] = "Blood Sugar-Fasting";
                dr["Vital_Value"] = BloodFastingResults[0].Value + "mg/dl";
                dr["Captured_date_and_time"] = BloodFastingResults[0].Captured_date_and_time;
                dr["Reason_For_Refuse"] = BloodFastingResults[0].Notes;
                dtTable.Rows.Add(dr);
            }
            if (BloodPostResults.Count != 0)
            {
                dr = dtTable.NewRow();
                dr["Vital_Name"] = "Blood Sugar-Post Prandial";
                dr["Vital_Value"] = BloodPostResults[0].Value + "mg/dl";
                dr["Captured_date_and_time"] = BloodPostResults[0].Captured_date_and_time;
                dr["Reason_For_Refuse"] = BloodPostResults[0].Notes;
                dtTable.Rows.Add(dr);
            }
            ds.Tables.Add(dtTable);
            iMySession.Close();
            return ds;
        }
        #endregion
        #region IVitalsManager Members



        public IList<DynamicScreen> LoadDynamicScreenXML()
        {
            string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\Dynamic_Screen.xml");
            IList<DynamicScreen> lstDynamicScreen = new List<DynamicScreen>();
            if (File.Exists(strXmlFilePath) == true)
            {
                XmlDocument itemDoc = new XmlDocument();
                XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                XmlNodeList xmlTagName = null;
                itemDoc.Load(XmlText);
                XmlText.Close();

                if (itemDoc.GetElementsByTagName("DynamicScreenList")[0] != null)
                {
                    //Added by Balaji on 17-Nov-2015
                    if (itemDoc.GetElementsByTagName("DynamicScreenList").Count > 0)
                    {
                        xmlTagName = itemDoc.GetElementsByTagName("DynamicScreenList")[0].ChildNodes; ;

                        if (xmlTagName.Count > 0)
                        {
                            for (int j = 0; j < xmlTagName.Count; j++)
                            {

                                string TagName = xmlTagName[j].Name;
                                XmlSerializer xmlserializer = new XmlSerializer(typeof(DynamicScreen));
                                DynamicScreen objDynamicScreen = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as DynamicScreen;
                                IEnumerable<PropertyInfo> propInfo = null;
                                //Added by Balaji on 17-Nov-2015
                                if (objDynamicScreen != null)
                                {

                                    propInfo = from obji in ((DynamicScreen)objDynamicScreen).GetType().GetProperties() select obji;

                                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                                    {
                                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                                        {
                                            foreach (PropertyInfo property in propInfo)
                                            {
                                                if (property.Name == nodevalue.Name)
                                                {
                                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                                                        property.SetValue(objDynamicScreen, Convert.ToUInt64(nodevalue.Value), null);
                                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                                                        property.SetValue(objDynamicScreen, Convert.ToString(nodevalue.Value), null);
                                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        property.SetValue(objDynamicScreen, Convert.ToDateTime(nodevalue.Value), null);
                                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                                                        property.SetValue(objDynamicScreen, Convert.ToInt32(nodevalue.Value), null);
                                                    else
                                                        property.SetValue(objDynamicScreen, nodevalue.Value, null);
                                                }
                                            }
                                        }
                                    }
                                    lstDynamicScreen.Add(objDynamicScreen);
                                }
                            }
                        }
                    }
                }




            }

            return lstDynamicScreen;
        }
        public IList<MasterVitals> LoadMasterVitalsXML()
        {
            string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\Master_Vitals.xml");
            IList<MasterVitals> lstMasterVitals = new List<MasterVitals>();
            if (File.Exists(strXmlFilePath) == true)
            {
                XmlDocument itemDoc = new XmlDocument();
                XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                XmlNodeList xmlTagName = null;
                itemDoc.Load(XmlText);
                XmlText.Close();

                if (itemDoc.GetElementsByTagName("MasterVitalsList")[0] != null)
                {
                    //Added by Balaji on 17-Nov-2015
                    if (itemDoc.GetElementsByTagName("MasterVitalsList").Count > 0)
                    {
                        xmlTagName = itemDoc.GetElementsByTagName("MasterVitalsList")[0].ChildNodes; ;

                        if (xmlTagName.Count > 0)
                        {
                            for (int j = 0; j < xmlTagName.Count; j++)
                            {

                                string TagName = xmlTagName[j].Name;
                                XmlSerializer xmlserializer = new XmlSerializer(typeof(MasterVitals));
                                MasterVitals objMasterVitals = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as MasterVitals;
                                IEnumerable<PropertyInfo> propInfo = null;
                                //Added by Balaji on 17-Nov-2015
                                if (objMasterVitals != null)
                                {

                                    propInfo = from obji in ((MasterVitals)objMasterVitals).GetType().GetProperties() select obji;

                                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                                    {
                                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                                        {
                                            foreach (PropertyInfo property in propInfo)
                                            {
                                                if (property.Name == nodevalue.Name)
                                                {
                                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                                                        property.SetValue(objMasterVitals, Convert.ToUInt64(nodevalue.Value), null);
                                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                                                        property.SetValue(objMasterVitals, Convert.ToString(nodevalue.Value), null);
                                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        property.SetValue(objMasterVitals, Convert.ToDateTime(nodevalue.Value), null);
                                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                                                        property.SetValue(objMasterVitals, Convert.ToInt32(nodevalue.Value), null);
                                                    else
                                                        property.SetValue(objMasterVitals, nodevalue.Value, null);
                                                }
                                            }
                                        }
                                    }
                                    lstMasterVitals.Add(objMasterVitals);
                                }
                            }
                        }
                    }
                }




            }

            return lstMasterVitals;
        }

        public IList<object> GetLocalData(ulong PhyID, int age, string Sex, string Category, ulong ScreenId)
        {
            IList<object> objList = new List<object>();
            ulong[] MasterVitalIDList;
            string MasterID = string.Empty;
            IList<MasterVitals> masterlist = new List<MasterVitals>();
            using (ISession SessionObj = NHibernateSessionManager.Instance.CreateISession())
            {
                MapVitalsPhysicianManager objMapMngr = new MapVitalsPhysicianManager();
                IList<MapVitalsPhysician> mapVitalList = objMapMngr.GetVitalsForPhysician(PhyID);
                objList.Add(mapVitalList);
                if (mapVitalList != null && mapVitalList.Count > 0)
                {
                    MasterVitalIDList = (from obj in mapVitalList select obj.Master_Vitals_ID).ToArray<ulong>();
                    IList<DynamicScreen> dynList = LoadDynamicScreenXML();
                    dynList = (from a in dynList where a.Screen_ID == ScreenId && MasterVitalIDList.Contains(Convert.ToUInt64(a.Master_Vitals_ID)) orderby a.Sort_Order ascending select a).ToList<DynamicScreen>();
                    objList.Add(dynList);
                    masterlist = LoadMasterVitalsXML();
                    masterlist = (from a in masterlist where MasterVitalIDList.Contains(Convert.ToUInt64(a.Id)) select a).ToList<MasterVitals>();
                    objList.Add(masterlist);
                }
                else
                {
                    objList.Add(null);
                    objList.Add(null);
                }
                SessionObj.Close();
            }

            PercentileLookUpManager objPer = new PercentileLookUpManager();
            IList<PercentileLookUp> perlisct = objPer.GetPercentileLookUpByAgeAndSex(age, Sex, Category);
            objList.Add(perlisct);
            return objList;
        }
        public IList<DynamicScreen> GetAcurusCodesListforFlowsheet(ulong physicianId)
        {
            IList<DynamicScreen> DynamicList = new List<DynamicScreen>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                var lstVital = new MapVitalsPhysicianManager().GetVitalsForPhysician(physicianId);
                var lstControlNames = lstVital.Select(a => a.Vital_Text).ToList();

                var queryStr = @"SELECT L.* 
                                 FROM   DYNAMIC_SCREEN L 
                                 WHERE  L.CONTROL_NAME IN ( :LSTCONTROL_NAMES ) 
                                        AND L.ACURUS_RESULT_CODE <> '' 
                                        AND L.IS_FLOW_SHEET_REQUIRED = 'Y' 
                                 GROUP  BY CONTROL_NAME 
                                 ORDER  BY SORT_ORDER ";

                ISQLQuery sql = iMySession.CreateSQLQuery(queryStr).AddEntity("l.*", typeof(DynamicScreen));
                sql.SetParameterList("LSTCONTROL_NAMES", lstControlNames);
                DynamicList = sql.List<DynamicScreen>();
                iMySession.Close();
            }
            return DynamicList;
        }

        public IList<PatientResults> GetPatientListByResultMasterID(ulong ResultMasterID)
        {
            IList<PatientResults> isltPatientResults = new List<PatientResults>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("select p.* from patient_results p where p.Result_Master_ID=" + ResultMasterID.ToString()).AddEntity("p.*", typeof(PatientResults));
                isltPatientResults = sql.List<PatientResults>();
                iMySession.Close();
            }
            return isltPatientResults;
        }


        public PatientResultsDTO GetNotificationValues(PatientResultsDTO objPatientResultsDTOValues, ulong EncounterID, ulong HumanId, ulong PhyID, string UserName)
        {
            UserLookupManager objUserLookupManager = new UserLookupManager();
            IList<UserLookup> lst = objUserLookupManager.GetFieldLookupList(UserName, "MANAGE CCDS");
            if (lst != null && lst.Count > 0 && lst.Any(a => a.Value.Trim() == "Diabetes management based on Hemoglobin A1cl" || a.Value.Trim() == "High Blood pressure management" || a.Value.Trim() == "Diabetes: Low Density Lipoprotein (LDL) Management"))
            {
                //bool Is_Vitals = false;
                //bool Is_Assessment = true;
                //bool Is_Assessment_Or_Vitals = false;
                string value = string.Empty;
                //EncounterManager objEncounterManager = new EncounterManager();
                //value = objEncounterManager.GetNotification(EncounterID, HumanId, UserName, ref Is_Assessment, ref Is_Vitals, ref Is_Assessment_Or_Vitals);

                //if (Is_Vitals || Is_Assessment_Or_Vitals)
                //    objPatientResultsDTOValues.sNotification = value;
                //else
                //    objPatientResultsDTOValues.sNotification = string.Empty;
            }

            return objPatientResultsDTOValues;

        }
        #endregion


        public PatientResults GetResultByLoincObservationAndEncounterId(string Loinc_Identifier, ulong Human_Id, ulong Encounter_Id)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<PatientResults> objPatientResults = new List<PatientResults>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Loinc_Identifier", Loinc_Identifier)).Add(Expression.Eq("Human_ID", Human_Id)).Add(Expression.Eq("Encounter_ID", Encounter_Id));
                objPatientResults = crit.List<PatientResults>();
                iMySession.Close();
            }
            if (objPatientResults != null && objPatientResults.Count > 0)
                return objPatientResults[0];
            else
                return null;
        }
        public IList<PatientResults> GetResultByLoincObservationAndEncounterIdStage3(string Loinc_Identifier, ulong Human_Id, string fromdate, string todate)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<PatientResults> objPatientResults = new List<PatientResults>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                // ICriteria crit = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Loinc_Identifier", Loinc_Identifier)).Add(Expression.Eq("Human_ID", Human_Id)).Add(Expression.Eq("Encounter_ID", Encounter_Id));
                ISQLQuery sql = iMySession.CreateSQLQuery("select p.* from patient_results p where p.Captured_date_and_time>= STR_TO_DATE('" + fromdate + "', '%Y-%m-%d')  and p.Captured_date_and_time<= STR_TO_DATE('" + todate + "', '%Y-%m-%d') and human_id=" + Human_Id + " and Loinc_Identifier ='" + Loinc_Identifier + "'").AddEntity("p.*", typeof(PatientResults));
                objPatientResults = sql.List<PatientResults>();
                iMySession.Close();
            }

            return objPatientResults;
        }
        //Jira #CAP-630
        //public IList<PatientResults> GetPatientListByDos(string fromDate, string toDate, ulong human_id)
        //{
        //    IList<PatientResults> isltPatientResults = new List<PatientResults>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ISQLQuery sql = iMySession.CreateSQLQuery("select a.* from (select p.* from patient_results p where p.Captured_date_and_time>= STR_TO_DATE('" + fromDate + "', '%Y-%m-%d %H:%i:%s')  and p.Captured_date_and_time< STR_TO_DATE('" + toDate + "', '%Y-%m-%d %H:%i:%s') and human_id=" + human_id + " and Loinc_Observation like'%bp%'  and value like '%/%'order by Captured_date_and_time desc) a group by Loinc_Observation").AddEntity("a.*", typeof(PatientResults));
        //        isltPatientResults = sql.List<PatientResults>();
        //        iMySession.Close();
        //    }
        //    return isltPatientResults;
        //}
        public IList<PatientResults> GetPatientListByDos(string fromDate, string toDate, ulong human_id, ulong EncounterId)
        {
            IList<PatientResults> isltPatientResults = new List<PatientResults>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql;
                if (EncounterId == 0)
                    sql = iMySession.CreateSQLQuery("select a.* from (select p.* from patient_results p where p.Captured_date_and_time>= STR_TO_DATE('" + fromDate + "', '%Y-%m-%d %H:%i:%s')  and p.Captured_date_and_time< STR_TO_DATE('" + toDate + "', '%Y-%m-%d %H:%i:%s') and human_id=" + human_id + " and Loinc_Observation like'%bp%'  and value like '%/%'order by Captured_date_and_time desc) a group by Loinc_Observation").AddEntity("a.*", typeof(PatientResults));
                else
                    sql = iMySession.CreateSQLQuery("select a.* from (select p.* from patient_results p where p.Captured_date_and_time>= STR_TO_DATE('" + fromDate + "', '%Y-%m-%d %H:%i:%s')  and p.Captured_date_and_time< STR_TO_DATE('" + toDate + "', '%Y-%m-%d %H:%i:%s') and human_id=" + human_id + " and Encounter_id!=" + EncounterId + " and Loinc_Observation like'%bp%'  and value like '%/%'order by Captured_date_and_time desc) a group by Loinc_Observation").AddEntity("a.*", typeof(PatientResults));
                isltPatientResults = sql.List<PatientResults>();
                iMySession.Close();
            }
            return isltPatientResults;
        }

        public string GetAssesmentDetails(ulong humanid, DateTime DateofService)
        {
            string result="";
            IList<Assessment> isltPatientResults = new List<Assessment>();
            EncounterManager onjen=new EncounterManager();
            string encounter_id = onjen.GetPreviousEncounterID(humanid, DateofService);
            if(encounter_id!="")
            {
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("select a.* from assessment as a where icd='I10'  and encounter_id=" + Convert.ToUInt32(encounter_id)).AddEntity("a.*", typeof(Assessment));
                isltPatientResults = sql.List<Assessment>();
                if (isltPatientResults.Count > 0 && isltPatientResults[0].Id != 0)
            {
                result = isltPatientResults[0].Id.ToString();
            }
                iMySession.Close();
            }
            }
            return result;
        }

        public void SaveVitalsforSummary(IList<PatientResults> lstvitals)
        {
            IList<PatientResults> UpdatePatientResult = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref lstvitals, ref UpdatePatientResult, null, string.Empty, false, false, 0, string.Empty);
        }
        //InsertPatientResults Method Declared but never used.
        public void InsertPatientResults(IList<PatientResults> ilstPatientResults)
        {
            IList<PatientResults> UpdatePatientResult = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstPatientResults, ref UpdatePatientResult, null, string.Empty, false, false, 0, string.Empty);
        }
        //InsertPatientResults Method Declared but never used.
        public void InsertPatientResults(IList<PatientResults> ilstPatientResults, ref ISession session)
        {
            IList<PatientResults> UpdatePatientResult = null;
            GenerateXml XMLObj = new GenerateXml();
            SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ilstPatientResults, ref UpdatePatientResult, null, session, "", false, false, 0, string.Empty, ref XMLObj);
        }

        public IList<PatientResults> GetVitalsByHumanEncounter(ulong encounterId, ulong humanId)
        {
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                var querystr = @"SELECT V.* 
                                 FROM   PATIENT_RESULTS V 
                                        INNER JOIN DYNAMIC_SCREEN S 
                                                ON ( V.LOINC_OBSERVATION = S.CONTROL_NAME ) 
                                 WHERE  V.ENCOUNTER_ID = :ENCOUNTERID  
                                        AND V.HUMAN_ID = :HUMANID  
                                        AND RESULTS_TYPE = 'VITALS' 
                                        AND V.VITALS_GROUP_ID IN(SELECT MAX(VITALS_GROUP_ID) 
                                                                 FROM   PATIENT_RESULTS 
                                                                 WHERE  HUMAN_ID = V.HUMAN_ID 
                                                                        AND ENCOUNTER_ID = V.ENCOUNTER_ID) 
                                 GROUP  BY V.LOINC_OBSERVATION 
                                 ORDER  BY S.SORT_ORDER";

                ISQLQuery iSqlQuery = iMySession.CreateSQLQuery(querystr).AddEntity("V", typeof(PatientResults));

                iSqlQuery.SetParameter("ENCOUNTERID", encounterId);
                iSqlQuery.SetParameter("HUMANID", humanId);

                var lstPatientResults = iSqlQuery.List<PatientResults>();

                iMySession.Close();

                return lstPatientResults;
            }
        }
        public IList<string> GetHeightandWeightbyEncounterID(ulong EncounterID, DateTime dtCreatedorModifedDate, ulong uHumanID)
        {
            IList<string> ilstHeightAndWeight = new List<string>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = null;
                if (EncounterID != 0)
                    sql = iMySession.CreateSQLQuery("Select v.Loinc_Observation,v.Value from patient_results v where v.Loinc_Observation in('Weight','Height') and v.Results_Type='Vitals' and v.encounter_id='" + EncounterID + "'");
                else
                    sql = iMySession.CreateSQLQuery("Select v.Loinc_Observation,v.Value from patient_results v where v.Loinc_Observation in('Weight','Height') and v.Results_Type='Vitals' and v.encounter_id='" + EncounterID + "' and  v.human_id=" + uHumanID + " and if(v.modified_date_and_time<>'0001-01-01 00:00:00',DATE_FORMAT(v.modified_date_and_time,'%Y-%m-%d'),DATE_FORMAT(v.created_date_and_time,'%Y-%m-%d'))='" + dtCreatedorModifedDate.ToString("yyyy-MM-dd") + "'");
                ArrayList arr = new ArrayList(sql.List());
                if (arr != null)
                {
                    for (int i = 0; i < arr.Count; i++)
                    {
                        object[] obj = (object[])arr[i];
                        ilstHeightAndWeight.Add(obj[0].ToString() + "-" + obj[1].ToString());
                    }
                }
            }
            return ilstHeightAndWeight;
        }
    }
}
